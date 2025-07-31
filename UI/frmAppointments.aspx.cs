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
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Globalization;
using System.Xml;
using System.Diagnostics;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using System.Threading;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmAppointments : System.Web.UI.Page
    {
        #region Declarations
        //iTextSharp.text.Font boldFont = iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        //iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        //iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
        //iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
        //iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);

        EncounterManager EncMngr = new EncounterManager();
        WFObjectManager wfMngr = new WFObjectManager();
        StaticLookupManager staticLookupMngr = new StaticLookupManager();

        private const string WalkedAwayMenuText = "Walked Away";
        private const string NoShowMenuText = "No Show";
        private const string ShowPatientSummaryText = "Show Patient Summary";
        private const string UndoText = "Undo";
        private const string ElectronicSuperBill = "Electronic Super Bill";
        private const string PaperSuperBill = "Print Super Bill";
        private const string AuthRequest = "Auth Request";
        private const string UpdateAuthResponse = "Update Auth Response";
        private const string CreateNewAuth = "Create New Auth";
        Boolean bFormLoad = false;
        DateTime dtLastDate = DateTime.MinValue;
        public string sFillLoad = string.Empty;

        IList<StaticLookup> objStaticLookup = new List<StaticLookup>();
        IList<ProcessMaster> proclist;
        IList<string> ColorList;
        // string sAncillary = string.Empty;
        bool Is_Ancillary = false;

        //---UNUSED VARIABLES
        //bool btime = true;
        //DateTime dtMyDefaultDate = DateTime.Now;
        //private const string AttachSuperBill = "Attach Super Bill";
        //private const string EVMenuText = "Eligibility Verification";
        //private const string PrintReceiptText = "Print Receipt";
        //private const string CancelApptMenuText = "Cancel Appointment";
        //private const string CheckedInMenuText = "Checked In";
        //ProcessMasterManager ProcMasterMngr = new ProcessMasterManager();
        //FacilityManager FacilityMngr = new FacilityManager();
        //PhysicianManager PhyMngr = new PhysicianManager();
        //AppointmentLookupManager AppointmentMngr = new AppointmentLookupManager();
        #endregion

        #region Events
        void Page_PreInit(Object sender, EventArgs e)
        {
            if (Request.QueryString["MasterPage"] != null)
            {
                if (Request.QueryString["MasterPage"].ToString().ToUpper().Trim() == "YES")
                {
                    this.MasterPageFile = "~/DemoGraphicsEmpty.Master";

                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //CAP-1752
                hdnIsSSOLogin.Value = ConfigurationSettings.AppSettings["IsSSOLogin"]??"N";
                //logger.Debug("Page Load occuring for the first time");
                btnClose.Visible = false;
                System.Diagnostics.Stopwatch LoadTime = new System.Diagnostics.Stopwatch();
                LoadTime.Start();
                IList<FacilityLibrary> facList;
                IList<FacilityLibrary> TempFac = new List<FacilityLibrary>();
                if (hdnSourceScreen != null)
                {
                    hdnSourceScreen.Value = Request.QueryString["hdnSourceScreen"];
                }
                hdnRole.Value = ClientSession.UserRole.ToUpper().ToString();
                //Added by bala for Color coding in sheduler
                //logger.Debug("getStaticLookupByFieldName DB Call Starting");
                //Stopwatch getStaticLookupByFieldNameDBCall = new Stopwatch();
                //getStaticLookupByFieldNameDBCall.Start();
                //objStaticLookup = staticLookupMngr.getStaticLookupByFieldName("CATEGORY OF BLOCK", "Sort_Order");
                //getStaticLookupByFieldNameDBCall.Stop();
                //logger.Debug("getStaticLookupByFieldName DB Call Completed. Time Taken : " + getStaticLookupByFieldNameDBCall.Elapsed.Seconds + "." + getStaticLookupByFieldNameDBCall.Elapsed.Milliseconds + "s.");
                //Session["TypeofvisitColorList"] = objStaticLookup;
                switch (hdnSourceScreen.Value)
                {
                    case "AppointmentFacility":
                        //logger.Debug("Screen Setup is for Physician");
                        btnClose.Visible = true;
                        this.Page.Title = "Physician Calendar All Facility" + "-" + ClientSession.UserName;
                        hdnTitle.Value = this.Page.Title;
                        chkShowActive.Text = "Show All";
                        //CAP-3268
                        //chkShowAllPhysicians.Visible = true;
                        divShowAllPhysicians.Visible = true;
                        divShowAllProviders.Visible = false;
                        chkShowActive.Checked = false;
                        btnBlockDays.Visible = false;
                        btnFindAppointments.Enabled = true;
                        btnPhysiciancalenderFacility.Visible = false;
                        pnlFacilityHeader.InnerText = "Providers";
                        pnlProvidersHeader.InnerText = "Facility";
                        //Stream phylis = PhyMngr.GetPhysicianList();
                        //var serializer = new NetDataContractSerializer();
                        //object obj = (object)serializer.ReadObject(phylis);
                        IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
                        //logger.Debug("Request['hdnApptPhyId']=" + Request["hdnApptPhyId"]);
                        if (Request.QueryString["hdnApptPhyId"] != string.Empty)
                        {
                            hdnApptPhyId.Value = Request.QueryString["hdnApptPhyId"];
                        }
                        //logger.Debug("Request['hdnApptFacName']=" + Request["hdnApptFacName"]);
                        if (Request.QueryString["hdnApptFacName"] != string.Empty)
                        {
                            hdnApptFacName.Value = Request.QueryString["hdnApptFacName"].Replace("_", "#");
                        }
                        //logger.Debug("Loading Physician list for specified facility from XML");
                        PhyList = UtilityManager.GetPhysicianList(hdnApptFacName.Value, ClientSession.LegalOrg);//PhyMngr.GetPhysicianListbyFacility(hdnApptFacName.Value, "Y");
                        PhyList = PhyList.OrderBy(a => a.PhyLastName).ToList<PhysicianLibrary>();
                        if (PhyList != null)
                        {
                            //logger.Debug("Physician list is not null. Count = " + PhyList.Count);
                            for (int i = 0; i < PhyList.Count; i++)
                            {
                                System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                                //cboItem.Text = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                                //Old Code
                                //cboItem.Text = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName;
                                //Gitlab# 2485 - Physician Name Display Change
                                if (PhyList[i].PhyLastName != String.Empty)
                                    cboItem.Text += PhyList[i].PhyLastName;
                                if (PhyList[i].PhyFirstName != String.Empty)
                                {
                                    if (cboItem.Text != String.Empty)
                                        cboItem.Text += "," + PhyList[i].PhyFirstName;
                                    else
                                        cboItem.Text += PhyList[i].PhyFirstName;
                                }
                                if (PhyList[i].PhyMiddleName != String.Empty)
                                    cboItem.Text += " " + PhyList[i].PhyMiddleName;
                                if (PhyList[i].PhySuffix != String.Empty)
                                    cboItem.Text += "," + PhyList[i].PhySuffix;

                                cboItem.Value = PhyList[i].Id.ToString();
                                this.cboFacilityName.Items.Add(cboItem);
                                if (hdnApptPhyId.Value == PhyList[i].Id.ToString())
                                {
                                    cboFacilityName.Items.FindByText(cboItem.Text).Selected = true;
                                    cboFacilityName.Attributes.Add("Tag", PhyList[i].Id.ToString());
                                }

                            }
                            Session["PhysicianList"] = PhyList;
                        }
                        //else
                        //logger.Debug("Physician list is null");

                        #region ModifiedRegionForPerformanceTuning
                        if (hdnApptFacName.Value == string.Empty)
                        {
                            //logger.Debug("Loading Facility list for specified Physician from XML");
                            IList<MapFacilityPhysician> mapfacList = UtilityManager.GetFacilityListMappedToPhysician(cboFacilityName.Attributes["Tag"].ToString());
                            if (mapfacList != null && mapfacList.Count != 0)
                            {
                                TempFac = ApplicationObject.facilityLibraryList.Where(item => item.Fac_Name == mapfacList[0].Facility_Name).ToList<FacilityLibrary>();
                                //logger.Debug("Facility list is not null. Count = " + mapfacList.Count);
                            }
                            else
                                TempFac = ApplicationObject.facilityLibraryList;
                        }
                        else
                            TempFac = ApplicationObject.facilityLibraryList.Where(item => item.Fac_Name == hdnApptFacName.Value).ToList<FacilityLibrary>();

                        //---Commented by Pujhitha. from this DB call only the start and end time is extracted. 
                        //---These values can be obtained from Session["PhysicianList"] which has these required values and is filled from XML
                        //IList<MapFacilityPhysician> mapfacList = new List<MapFacilityPhysician>();
                        //IList<FacilityLibrary> facilityList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
                        //MapFacilityPhysicianManager mapPhyfac = new MapFacilityPhysicianManager();
                        //if (cboFacilityName.Attributes["Tag"] != null)
                        //    mapfacList = mapPhyfac.GetMapFacilityListbyPhyID(Convert.ToUInt64(cboFacilityName.Attributes["Tag"].ToString()));
                        //if (hdnApptFacName.Value == string.Empty)
                        //{

                        //    var fac = from f in facilityList where f.Fac_Name == mapfacList[0].Facility_Name select f;
                        //    TempFac = fac.ToList<FacilityLibrary>();
                        //}
                        //else
                        //{

                        //    var fac = from f in facilityList where f.Fac_Name == hdnApptFacName.Value select f;
                        //    TempFac = fac.ToList<FacilityLibrary>();
                        //}
                        #endregion

                        break;

                    case "":
                        //logger.Debug("Screen Setup is for FO");
                        this.Page.Title = "Appointments" + "-" + ClientSession.UserName;
                        hdnTitle.Value = this.Page.Title;
                        //CAP-3268
                        //chkShowAllPhysicians.Visible = false;
                        divShowAllPhysicians.Visible = false;
                        divShowAllProviders.Visible = true;
                        divCheckBoxShowAllProviders.Visible = false;
                        //facList = ApplicationObject.facilityLibraryList;//FacilityMngr.GetFacilityList();
                        var faclist = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                        facList = faclist.ToList<FacilityLibrary>();
                        //logger.Debug("Filling Facility Combobox");
                        if (facList != null)
                        {
                            //logger.Debug("Facility list count=" + facList.Count.ToString());
                            for (int i = 0; i < facList.Count; i++)
                            {
                                System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                                cboItem.Text = facList[i].Fac_Name;
                                // cboItem.Value = facList[i].Id.ToString();
                                this.cboFacilityName.Items.Add(cboItem);
                            }
                        }
                        //else
                        //logger.Debug("Facility list is null. Note it is Application Object. So it must be some serious issue.");
                        if (ClientSession.FacilityName != null)
                        {
                            cboFacilityName.Text = ClientSession.FacilityName;
                        }
                        if (facList.Count > 0 && cboFacilityName.Text != string.Empty)
                        {
                            var fac1 = from f in facList where f.Fac_Name == cboFacilityName.Text select f;
                            TempFac = fac1.ToList<FacilityLibrary>();
                        }


                        break;

                    case "Menu":
                        if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                        {
                            //logger.Debug("Screen Setup is for Physician");
                            this.Page.Title = "Physician Calendar All Facility" + "-" + ClientSession.UserName;
                            hdnTitle.Value = this.Page.Title;
                            chkShowActive.Text = "Show All";
                            //CAP-3268
                            //chkShowAllPhysicians.Visible = true;
                            divShowAllPhysicians.Visible = true;
                            divShowAllProviders.Visible = false;
                            divCheckBoxShowAllProviders.Visible = true;
                            chkShowActive.Checked = false;
                            btnBlockDays.Enabled = true;
                            //btnFindAppointments.Enabled = false;\\For bug id 45801
                            btnFindAppointments.Enabled = true;
                            btnPhysiciancalenderFacility.Visible = false;
                            pnlFacilityHeader.InnerText = "Providers";
                            pnlProvidersHeader.InnerText = "Facility";
                            hdnSourceScreen.Value = "AppointmentFacility";
                            //Stream phylis = PhyMngr.GetPhysicianList();
                            //var serializer = new NetDataContractSerializer();
                            //object obj = (object)serializer.ReadObject(phylis);
                            IList<PhysicianLibrary> PhyList1 = new List<PhysicianLibrary>();
                            if (ClientSession.PhysicianId != null && ClientSession.PhysicianId > 0)
                            {
                                hdnApptPhyId.Value = ClientSession.PhysicianId.ToString();
                            }
                            if (ClientSession.FacilityName != null)
                            {
                                hdnApptFacName.Value = ClientSession.FacilityName.ToString();
                            }

                            bool bPhysicianMappedToLoggedInFacility = true;
                            //logger.Debug("Loading Physician list for specified facility from XML");
                            PhyList1 = UtilityManager.GetPhysicianList(hdnApptFacName.Value, ClientSession.LegalOrg);//.Where(item => item.Is_Active.ToUpper() == "Y").ToList<PhysicianLibrary>();//PhyMngr.GetPhysicianListbyFacility(hdnApptFacName.Value, "Y");
                            if (!PhyList1.Any(phy => phy.Id == ClientSession.PhysicianId))
                            {
                                PhyList1 = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);
                                //CAP-3268
                                //chkShowAllPhysicians.Checked = true;
                                divShowAllPhysicians.Visible = true;
                                divShowAllProviders.Visible = false;
                                divCheckBoxShowAllProviders.Visible = true;
                                rdoAllActivePhysicians.Checked = true;
                            }
                            else
                            {
                                rdoActivePhysicians.Checked = true;
                            }
                            PhyList1 = PhyList1.OrderBy(a => a.PhyLastName).ToList<PhysicianLibrary>();
                            if (PhyList1 != null)
                            {
                                //logger.Debug("Physician list is not null. Count = " + PhyList1.Count);
                                string PrvdName = string.Empty;

                                for (int i = 0; i < PhyList1.Count; i++)
                                {
                                    System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                                    /*string str = PhyList1[i].PhyPrefix + " " + PhyList1[i].PhyFirstName + " " + PhyList1[i].PhyMiddleName + " " + PhyList1[i].PhyLastName;
                                    PrvdName = str;
                                    //if (str.Length > 31)
                                    //{
                                    //    PrvdName = str.Substring(0, 30);
                                    //}
                                    cboItem.Attributes.Add("title", str);
                                    cboItem.Text = PrvdName;
                                    cboItem.Value = PhyList1[i].Id.ToString();
                                    this.cboFacilityName.Items.Add(cboItem);*/

                                    //Jira CAP-2777
                                    //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                                    //if (File.Exists(strXmlFilePathTech) == true)
                                    {
                                        //Jira CAP-2777
                                        //XmlDocument xmldoc = new XmlDocument();
                                        //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                                        if (PhyList1[i].PhyColor != "" && PhyList1[i].PhyColor != "0")
                                        {
                                            //Jira CAP-2777
                                            //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhyList1[i].PhyColor);

                                            //if (xmlTec != null && xmlTec[0] != null)
                                            //{
                                            //    cboItem.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhyList1[i].PhyFirstName + " " + PhyList1[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                            //    cboItem.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                                            //    cboItem.Attributes.Add("title", cboItem.Text);
                                            //}

                                            //Jira CAP-2777
                                            MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                                            machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                                            if (machinetechnicianList?.MachineTechnician != null)
                                            {
                                                List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                                machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhyList1[i].PhyColor).ToList();
                                                if ((machinetechnicians?.Count ?? 0) > 0)
                                                {
                                                    cboItem.Text = machinetechnicians[0].machine_name + " - " + PhyList1[i].PhyFirstName + " " + PhyList1[i].PhyLastName;
                                                    cboItem.Value = machinetechnicians[0].machine_technician_library_id;
                                                    cboItem.Attributes.Add("title", cboItem.Text);
                                                }
                                            }


                                        }
                                        else
                                        {
                                            //Old Code
                                            //cboItem.Text = PhyList1[i].PhyPrefix + " " + PhyList1[i].PhyFirstName + " " + PhyList1[i].PhyMiddleName + " " + PhyList1[i].PhyLastName;
                                            //Gitlab# 2485 - Physician Name Display Change
                                            if (PhyList1[i].PhyLastName != String.Empty)
                                                cboItem.Text += PhyList1[i].PhyLastName;
                                            if (PhyList1[i].PhyFirstName != String.Empty)
                                            {
                                                if (cboItem.Text != String.Empty)
                                                    cboItem.Text += "," + PhyList1[i].PhyFirstName;
                                                else
                                                    cboItem.Text += PhyList1[i].PhyFirstName;
                                            }
                                            if (PhyList1[i].PhyMiddleName != String.Empty)
                                                cboItem.Text += " " + PhyList1[i].PhyMiddleName;
                                            if (PhyList1[i].PhySuffix != String.Empty)
                                                cboItem.Text += "," + PhyList1[i].PhySuffix;
                                            cboItem.Value = PhyList1[i].Id.ToString();
                                            cboItem.Attributes.Add("title", cboItem.Text);
                                        }
                                        this.cboFacilityName.Items.Add(cboItem); ;
                                    }


                                    if (hdnApptPhyId.Value == PhyList1[i].Id.ToString())
                                    {
                                        cboFacilityName.Items.FindByText(cboItem.Text).Selected = true;
                                        cboFacilityName.Attributes.Add("Tag", PhyList1[i].Id.ToString());
                                    }
                                }
                                SortPhysician();
                                Session["PhysicianList"] = PhyList1;
                            }
                            //else
                            //logger.Debug("Physician list is null");
                            //------------------------------------------
                            #region ModifiedRegionForPerformanceTuning
                            if (hdnApptFacName.Value == string.Empty && bPhysicianMappedToLoggedInFacility)
                            {
                                //logger.Debug("Loading Facility list for specified Physician from XML");
                                IList<MapFacilityPhysician> mapfacList = UtilityManager.GetFacilityListMappedToPhysician(cboFacilityName.Attributes["Tag"].ToString());
                                if (mapfacList != null && mapfacList.Count != 0)
                                {
                                    TempFac = ApplicationObject.facilityLibraryList.Where(item => item.Fac_Name == mapfacList[0].Facility_Name).ToList<FacilityLibrary>();
                                    //logger.Debug("Facility list is not null. Count = " + mapfacList.Count);
                                }
                                else
                                    TempFac = ApplicationObject.facilityLibraryList;
                            }
                            else
                                TempFac = ApplicationObject.facilityLibraryList.Where(item => item.Fac_Name == hdnApptFacName.Value).ToList<FacilityLibrary>();
                            //IList<MapFacilityPhysician> mapfacList1 = new List<MapFacilityPhysician>();
                            //IList<FacilityLibrary> facilityList1 = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
                            //MapFacilityPhysicianManager mapPhyfac1 = new MapFacilityPhysicianManager();
                            //if (cboFacilityName.Attributes["Tag"] != null)
                            //{
                            //    mapfacList1 = mapPhyfac1.GetMapFacilityListbyPhyID(Convert.ToUInt64(cboFacilityName.Attributes["Tag"].ToString()));
                            //}
                            //if (hdnApptFacName.Value == string.Empty)
                            //{
                            //    if (facilityList1.Count > 0)
                            //    {
                            //        var fac = from f in facilityList1 where f.Fac_Name == mapfacList1[0].Facility_Name select f;
                            //        TempFac = fac.ToList<FacilityLibrary>();
                            //    }


                            //}
                            //else
                            //{
                            //    if (facilityList1.Count > 0)
                            //    {
                            //        var fac = from f in facilityList1 where f.Fac_Name == hdnApptFacName.Value select f;
                            //        TempFac = fac.ToList<FacilityLibrary>();
                            //    }

                            //}
                            #endregion
                        }
                        else
                        {
                            //logger.Debug("Screen Setup is for FO");
                            this.Page.Title = "Appointments" + "-" + ClientSession.UserName;
                            hdnTitle.Value = this.Page.Title;
                            //chkShowAllPhysicians.Visible = false;
                            //CAP-3268
                            divShowAllPhysicians.Visible = false;
                            divShowAllProviders.Visible = true;
                            divCheckBoxShowAllProviders.Visible = false;
                            //facList = ApplicationObject.facilityLibraryList;

                            var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                            facList = fac.ToList<FacilityLibrary>();

                            //logger.Debug("Filling Facility Combobox");
                            if (facList != null)
                            {
                                //logger.Debug("Facility list count=" + facList.Count.ToString());
                                string FacName = string.Empty;
                                for (int i = 0; i < facList.Count; i++)
                                {
                                    System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                                    string FacName1 = facList[i].Fac_Name;
                                    FacName = facList[i].Fac_Name;
                                    //if (facList[i].Fac_Name.Length > 31)
                                    //{
                                    //    FacName = facList[i].Fac_Name.Substring(0, 30);
                                    //}
                                    cboItem.Attributes.Add("title", FacName1);
                                    cboItem.Text = FacName;
                                    cboItem.Value = FacName1;
                                    this.cboFacilityName.Items.Add(cboItem);
                                }
                                SortPhysician();
                            }
                            //else
                            //logger.Debug("Facility list is null. Note it is Application Object. So it must be some serious issue.");

                            //----------------------------
                            if (ClientSession.FacilityName != null)
                            {
                                cboFacilityName.Text = ClientSession.FacilityName;
                            }
                            if (facList.Count > 0 && cboFacilityName.Text != string.Empty)
                            {
                                var facmnu = from f in facList where f.Fac_Name == cboFacilityName.Text select f;
                                TempFac = facmnu.ToList<FacilityLibrary>();
                            }

                        }
                        break;
                }
                if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
                {
                    btnBlockDays.Enabled = false;
                }
                IList<ProcessMaster> TempProcList;
                TempProcList = ApplicationObject.processMasterList;
                if (TempProcList != null && TempProcList.Count > 0)
                {
                    var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
                    proclist = ColoredProcess.ToList<ProcessMaster>();

                    var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
                    ColorList = DistinctColor.ToList<string>();
                }
                //else
                //logger.Debug("Process Master list is null. Note it is Application Object. So it must be some serious issue.");
                if (ColorList != null && ColorList.Count > 0)
                {
                    Session["ColorList"] = ColorList;
                }
                if (proclist != null && proclist.Count > 0)
                {
                    Session["proclist"] = proclist;
                }

                //srividhya
                //if (Session["LocalDate"] != null)
                if (ClientSession.LocalDate != null && ClientSession.LocalDate != string.Empty)
                {
                    //set default date
                    Calendar1.SelectedDate = DateTime.Now;
                    //sriivdhya
                    //Calendar1.SelectedDate = DateTime.ParseExact(Session["LocalDate"].ToString(), "M/dd/yyyy", CultureInfo.InvariantCulture);
                    //assign current date
                    try
                    {
                        if (ClientSession.LocalDate == "")
                            ClientSession.LocalDate = hdnLocalDate.Value;
                        Calendar1.SelectedDate = DateTime.ParseExact(ClientSession.LocalDate.ToString(), "M/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        Calendar1.SelectedDate = DateTime.Now;
                        //logger.Debug("Conversion of LocalTime of value='" + ClientSession.LocalDate.ToString() + "' to DateTime threw an error.", exp);
                        // throw (exp);
                    }
                    schAppointmentScheduler.SelectedDate = Calendar1.SelectedDate;
                    //Calendar1.TodayDayStyle.BackColor = Color.Beige; //commented by nijanthan
                }
                //logger.Debug("Request['Duedate']=" + Request["Duedate"]);
                if (Request["Duedate"] != null)
                {
                    try
                    {
                        Calendar1.SelectedDate = Convert.ToDateTime(Request["Duedate"]);
                    }
                    catch (Exception exp)
                    {
                        //logger.Debug("Conversion of Due Date to DateTime threw an error.", exp);
                        throw (exp);
                    }
                    //Calendar1.VisibleDate = Convert.ToDateTime(Request["Duedate"]); //commented by nijanthan
                    schAppointmentScheduler.SelectedDate = Calendar1.SelectedDate;
                }
                //else if (Session["Localtime"] != null)
                //{
                //    Calendar1.SelectedDate = ToLoalTime.LocalTimeFromTimeOffset(Session["Localtime"].ToString(), DateTime.Now); ;
                //    schAppointmentScheduler.SelectedDate = Calendar1.SelectedDate;
                //}
                if (Calendar1.SelectedDate.ToString("dd-MMM-yyyy") != string.Empty)
                {
                    hdnSelectedDate.Value = Calendar1.SelectedDate.ToString("dd-MMM-yyyy");
                }
                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();
                if (TempFac != null && TempFac.Count() > 0)
                {
                    if (TempFac[0].Start_Time != null && TempFac[0].Start_Time != string.Empty)
                    {
                        try
                        {
                            dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of Start Time of value='" + TempFac[0].Start_Time + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }
                    if (TempFac[0].Start_Time != null && TempFac[0].End_Time != string.Empty)
                    {
                        try
                        {
                            dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
                        }
                        catch (Exception exp)
                        {
                            //logger.Debug("Conversion of End Time of value='" + TempFac[0].End_Time + "' to DateTime threw an error.", exp);
                            throw (exp);
                        }
                    }
                }

                try
                {
                    schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Start Hour of value='" + dtStart.Hour + "' to double threw an error.", exp);
                    throw (exp);
                }
                try
                {
                    schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of End Hour of value='" + dtEnd.Hour + "' to double threw an error.", exp);
                    throw (exp);
                }

                Stopwatch Timer1 = new Stopwatch();
                Timer1.Start();
                cboFacilityName_SelectedIndexChanged(sender, e);
                Timer1.Stop();
                //logger.Debug("Loading Facilities or Physicians for combobox and filling the Scheduler as per Physician, Facility and Date. Time Taken : " + Timer1.Elapsed.Seconds + "." + Timer1.Elapsed.Milliseconds + "s.");
                LoadTime.Stop();
                lblLoad.Text = "Page Load -Minutes: " + LoadTime.Elapsed.Minutes.ToString() + " Seconds :" + LoadTime.Elapsed.Seconds.ToString() + " MilliSec : " + LoadTime.Elapsed.Milliseconds.ToString();
            }
            if (hdnTitle.Value != "")
                this.Page.Title = hdnTitle.Value;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            if (ClientSession.UserRole.ToUpper() == "CODER")
            {
                cboFacilityName.Enabled = false;
                chklstProviders.Enabled = false;
                //CAP-3491
                //chkShowActive.Enabled = false;
            }
            schAppointmentScheduler.EnableAjaxSkinRendering = true;
            //OverAllPageLoad.Stop();
            //logger.Debug("--------------------frmAppointments Page Load Completed. Time Taken :'" + OverAllPageLoad.Elapsed.Seconds + "." + OverAllPageLoad.Elapsed.Milliseconds + "s'--------------------");
        }

        protected void cboFacilityName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CAP-3268
            if (rdoInActiveProviders.Checked)
            {
                rdoInActiveProviders.Checked = false;
                rdoActiveProviders.Checked = true;
            }
            //CAP-3491
            if (rdoInActivePhysicians.Checked)
            {
                chkShowActive.Checked = true;
            }
            #region New Code
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            //CAP-3491
            //if (ApplicationObject.facilityLibraryList != null && cboFacilityName.SelectedItem != null)
            //{
            //    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacilityName.SelectedItem.Text select f;
            //    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            //    if (cboFacilityName.SelectedItem != null)
            //    {
            //        //if (sAncillary.Trim() != cboFacilityName.SelectedItem.Text.Trim())
            //        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
            //        {
            //            chkShowActive.Enabled = true;
            //        }
            //        else
            //        {
            //            chkShowActive.Enabled = false;
            //        }
            //    }

            //    else
            //        chkShowActive.Enabled = true;
            //}
            if (hdnSourceScreen.Value == "AppointmentFacility" || ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                FillCheckListBoxForPhysicianSetUp(hdnApptFacName.Value, true);
            else
                FillCheckListBoxForFOSetUp(cboFacilityName.Text, true);

            if (chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Any(li => li.Selected))
                schAppointmentScheduler.Visible = true;

            FillAllAppointmentsForDate();
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            #endregion

            #region Old Code
            //if (hdnSourceScreen.Value == "AppointmentFacility" || ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            //{
            //    //---commented by Pujhitha for new functionality of Show All Providers.
            //    //string sActive = string.Empty;
            //    //if (chkShowActive.Checked == true)
            //    //{
            //    //    sActive = "Y";
            //    //}
            //    //else
            //    //{
            //    //    sActive = "N";
            //    //}
            //    IList<PhysicianLibrary> phyList = UtilityManager.GetPhysicianList(hdnApptFacName.Value);//PhyMngr.GetPhysicianListbyFacility(hdnApptFacName.Value, sActive);

            //    //IList<StaticLookup> ApptSlotLength = new List<StaticLookup>();
            //    //ApptSlotLength = staticLookupMngr.getStaticLookupByFieldName("APPOINTMENT SLOT LENGTH");
            //    //IList<PhysicianLibrary> phyList = (IList<PhysicianLibrary>)Session["PhysicianList"];// PhyMngr.GetPhysicianListbyFacility(hdnApptFacName.Value, sActive); //Codereview


            //    //var phy = from p in phyList
            //    //          where p.PhyPrefix == cboFacilityName.Text.Split(' ')[0] && p.PhyFirstName == cboFacilityName.Text.Split(' ')[1]
            //    //              && p.PhyMiddleName == cboFacilityName.Text.Split(' ')[2] && p.PhyLastName.Contains(cboFacilityName.Text.Split(' ')[3]) == true
            //    //          select p.Id;
            //    if (phyList.Count > 0)
            //    {
            //        var phy = from p in phyList
            //                  where p.PhyPrefix == cboFacilityName.SelectedValue.Split(' ')[0] && p.PhyFirstName == cboFacilityName.SelectedValue.Split(' ')[1]
            //                      && p.PhyMiddleName == cboFacilityName.SelectedValue.Split(' ')[2] && p.PhyLastName == cboFacilityName.SelectedValue.Split(' ')[3]
            //                  select p.Id;
            //        if (phy.Count() > 0)
            //            hdnApptPhyId.Value = phy.First().ToString();
            //        #region ModifiedRegionForPerformanceTuning

            //        IList<MapFacilityPhysician> facList = new List<MapFacilityPhysician>();
            //        MapFacilityPhysicianManager mapPhyfac = new MapFacilityPhysicianManager();

            //        if (phy != null && phy.Count() > 0)
            //        {
            //            facList = UtilityManager.GetFacilityListMappedToPhysician(phy.First().ToString());//mapPhyfac.GetMapFacilityListbyPhyID(Convert.ToUInt64(phy.First()));
            //        }

            //        IList<FacilityLibrary> facilityList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
            //        IList<FacilityLibrary> TempFac;
            //        if (facList.Count == 0)
            //        {
            //            facList = null;
            //            TempFac = facilityList;
            //            schAppointmentScheduler.Visible = true;
            //            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Appointment Facility", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('110058');", true);
            //            //schAppointmentScheduler.Visible = false;
            //            //return;
            //        }
            //        else
            //        {
            //            var fac = from f in facilityList where f.Fac_Name == facList[0].Facility_Name select f;
            //            TempFac = fac.ToList<FacilityLibrary>();
            //            schAppointmentScheduler.Visible = true;
            //        }

            //        DateTime dtStart = new DateTime();
            //        DateTime dtEnd = new DateTime();
            //        if (TempFac[0].Start_Time != string.Empty)
            //        {
            //            dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
            //        }
            //        if (TempFac[0].End_Time != string.Empty)
            //        {
            //            dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
            //        }

            //        try
            //        {
            //            schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
            //            schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
            //            //if (ApptSlotLength.Count > 0)
            //            //{
            //            schAppointmentScheduler.MinutesPerRow = TempFac[0].Slot_Length;
            //            //}
            //        }
            //        catch
            //        {
            //        }
            //        chklstProviders.Items.Clear();
            //        if (facList != null)
            //        {
            //            for (int i = 0; i < facList.Count; i++)
            //            {
            //                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //                item.Text = facList[i].Facility_Name;
            //                item.Value = facList[i].Phy_Rec_ID.ToString();
            //                chklstProviders.Items.Add(item);
            //                if (facList[i].Sort_Order == 1)
            //                    chklstProviders.Items[i].Selected = true;
            //                else
            //                    chklstProviders.Items[i].Selected = false;
            //            }
            //        }
            //        else
            //        {
            //            facList = UtilityManager.GetFacilityListMappedToPhysician(hdnApptPhyId.Value);
            //            for (int i = 0; i < facilityList.Count; i++)
            //            {
            //                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //                item.Text = facilityList[i].Fac_Name;
            //                item.Value = facList.Any(fac => fac.Facility_Name.ToUpper().Trim() == facilityList[i].Fac_Name.ToUpper().Trim()) ? hdnApptPhyId.Value : (0).ToString();
            //                chklstProviders.Items.Add(item);
            //                if (facilityList[i].Fac_Name.ToUpper().Trim() == ClientSession.FacilityName.ToUpper().Trim())
            //                    chklstProviders.Items[i].Selected = true;
            //                else
            //                    chklstProviders.Items[i].Selected = false;
            //            }
            //        }

            //        //if (chklstProviders.Items.Count > 0)
            //        //{
            //        //    IList<AppointmentLookup> apptLookupList = new List<AppointmentLookup>();
            //        //    apptLookupList = AppointmentMngr.GetAppointmentLookupList(TempFac[0].Fac_Name);

            //        //    for (int i = 0; i < apptLookupList.Count; i++)
            //        //    {
            //        //        for (int j = 0; j < chklstProviders.Items.Count; j++)
            //        //        {
            //        //            if (Convert.ToUInt64(chklstProviders.Items[j].Value) == apptLookupList[i].Physician_ID)
            //        //            {
            //        //                chklstProviders.Items[j].Selected = true;
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //        #endregion
            //    }
            //    FillAllAppointmentsForDate();
            //}
            //else
            //{
            //    IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();

            //    //string sActive = string.Empty;
            //    //if (chkShowActive.Checked == true)
            //    //{
            //    //    sActive = "Y";
            //    //}
            //    //else
            //    //{
            //    //    sActive = "N";
            //    //}
            //    IList<FacilityLibrary> facList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
            //    IList<FacilityLibrary> TempFac;
            //    var fac = from f in facList where f.Fac_Name.ToString().Contains(cboFacilityName.Text) select f;
            //    TempFac = fac.ToList<FacilityLibrary>();

            //    if (TempFac != null)
            //    {
            //        DateTime dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
            //        DateTime dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
            //        try
            //        {
            //            schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
            //            schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
            //            schAppointmentScheduler.MinutesPerRow = TempFac[0].Slot_Length;
            //        }
            //        catch
            //        {
            //        }
            //    }
            //    if(chkShowActive.Checked)
            //        PhysicianList = UtilityManager.GetPhysicianList("");
            //    else
            //        PhysicianList = UtilityManager.GetPhysicianList(cboFacilityName.SelectedValue);//PhyMngr.GetPhysicianListbyFacility(cboFacilityName.SelectedValue, sActive);
            //    //Session["PhysicianList"] = PhysicianList;
            //    chklstProviders.Items.Clear();

            //    if (PhysicianList != null)
            //    {
            //        if (PhysicianList.Count == 0)
            //        {
            //            schAppointmentScheduler.Visible = false;
            //            return;
            //        }
            //        else
            //        {
            //            schAppointmentScheduler.Visible = true;
            //        }
            //        //if (cboFacilityName.Text.ToUpper() == System.Configuration.ConfigurationManager.AppSettings["CMGFacilityName"].ToString().ToUpper())//staticLookupMngr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.ToUpper())
            //        //{
            //        //    PhysicianList = PhysicianList.Where(a => a.Category == "MACHINE").ToList<PhysicianLibrary>();
            //        //}
            //        // new code instead of application_lookup
            //        XmlDocument xmldoc = new XmlDocument();
            //        string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
            //        string sDefaultPhysicians = "";
            //        if (File.Exists(strXmlFilePath) == true)
            //        {
            //            xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");

            //            XmlNode nodeMatchingFacility = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + cboFacilityName.SelectedValue.Trim().ToUpper() + "']");
            //            sDefaultPhysicians = nodeMatchingFacility.Attributes["default-physician-id"].Value.ToString();
            //        }
            //        string[] lstDefaultPhysicians = sDefaultPhysicians.Split(',');
            //        for (int i = 0; i < PhysicianList.Count; i++)
            //        {
            //            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //            //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            //            item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
            //            //string sPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            //            //item.Text = sPhyName;
            //            item.Value = PhysicianList[i].Id.ToString();
            //            chklstProviders.Items.Add(item);
            //            if (lstDefaultPhysicians.Any(curritem => curritem == PhysicianList[i].Id.ToString()))
            //                chklstProviders.Items[i].Selected = true;
            //            else
            //                chklstProviders.Items[i].Selected = false;
            //        }

            //    }

            //    //if (chklstProviders.Items.Count > 0)
            //    //{
            //    //    IList<AppointmentLookup> apptLookupList = new List<AppointmentLookup>();
            //    //    apptLookupList = AppointmentMngr.GetAppointmentLookupList(cboFacilityName.SelectedValue);

            //    //    for (int i = 0; i < apptLookupList.Count; i++)
            //    //    {
            //    //        for (int j = 0; j < chklstProviders.Items.Count; j++)
            //    //        {
            //    //            if (Convert.ToUInt64(chklstProviders.Items[j].Value) == apptLookupList[i].Physician_ID)
            //    //            {
            //    //                chklstProviders.Items[j].Selected = true;
            //    //            }
            //    //        }
            //    //    }
            //    //}
            //    FillAllAppointmentsForDate();
            //}
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            #endregion
        }

        protected void chklstProviders_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Added by Bala for Month View
            if (schAppointmentScheduler.SelectedView == SchedulerViewType.MonthView || hdnSchedulerView.Value == "SwitchToMonthView")
            {

                var isChkboxSelected = chklstProviders.Items.OfType<System.Web.UI.WebControls.ListItem>().Where(a => a.Selected == true);
                if (isChkboxSelected.Count() > 1 && hdnSchedulerView.Value != string.Empty)
                {
                    IList<string> sChkTextList = new List<string>();
                    foreach (var item in isChkboxSelected)
                    {
                        sChkTextList.Add(item.Text);
                    }
                    for (int i = 0; i < sChkTextList.Count; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        else
                        {
                            chklstProviders.Items.FindByText(sChkTextList[i].ToString()).Selected = false;
                        }
                    }
                    hdnSchedulerView.Value = string.Empty;

                }
                else
                {
                    //string value = string.Empty;

                    //string result = Request.Form["__EVENTTARGET"];

                    //string[] checkedBox = result.Split('$'); ;

                    //int index = checkedBox.Length - 1;

                    //if (chklstProviders.Items[index].Selected)
                    //{
                    //    value = chklstProviders.Items[index].Text;
                    //}
                    //for (int j = 0; j < chklstProviders.Items.Count; j++)
                    //{
                    //    if (chklstProviders.Items[j].Text == value)
                    //    {
                    //        chklstProviders.Items[j].Selected = true;
                    //    }
                    //    else
                    //    {
                    //        chklstProviders.Items[j].Selected = false;
                    //    }
                    //}
                }

            }

            //Added by bala
            var isAnySelected = chklstProviders.Items.OfType<System.Web.UI.WebControls.ListItem>().Where(a => a.Selected == true);
            if (isAnySelected.Count() == 0)
            {
                schAppointmentScheduler.Visible = false;
                RefreshMyResources();
                return;
            }
            schAppointmentScheduler.Visible = true;
            FillAllAppointmentsForDate();
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            hdnFindApptHumanID.Value = "";
            System.Diagnostics.Stopwatch ts = new System.Diagnostics.Stopwatch();
            ts.Start();
            IList<FacilityLibrary> facList;
            //facList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
            var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
            facList = fac.ToList<FacilityLibrary>();
            if (facList == null || (Calendar1.SelectedDates != null && Calendar1.SelectedDates.Count == 0))
            {
                return;
            }
            if (Calendar1.SelectedDates.Count > 0)
            {
                if (Calendar1.SelectedDates.Count == 1)
                {
                    Calendar1.SelectedDate = Calendar1.SelectedDates[0].Date;
                }
                else if (Calendar1.SelectedDates.Count == 2)
                {
                    Calendar1.SelectedDate = Calendar1.SelectedDates[1].Date;
                }
                else if (Calendar1.SelectedDates.Count == 3)
                {
                    Calendar1.SelectedDate = Calendar1.SelectedDates[2].Date;
                }
            }

            if (Calendar1.SelectedDate == dtLastDate)
            {
                return;
            }

            if (bFormLoad == false && sFillLoad == string.Empty)
            {
                FillAllAppointmentsForDate();
            }
            schAppointmentScheduler.SelectedDate = Calendar1.SelectedDate;
            hdnSelectedDate.Value = Calendar1.SelectedDate.ToString("dd-MMM-yyyy");
            ts.Stop();
            lblCalenderClick.Text = "Calendar-Minutes: " + ts.Elapsed.Minutes.ToString() + " Seconds :" + ts.Elapsed.Seconds.ToString() + " MilliSec : " + ts.Elapsed.Milliseconds.ToString();
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //logger.Debug("frmAppointments btnRefresh Click Event Started");
            ModalWindow.Visible = false;
            System.Diagnostics.Stopwatch Refresh = new System.Diagnostics.Stopwatch();
            Refresh.Start();
            bool bChklstProvidersItemsLoaded = false;
            if (hdnSelectedSlotDate.Value != string.Empty && hdnSelectedSlotDate.Value != "undefined")
            {
                try
                {
                    Calendar1.SelectedDate = Convert.ToDateTime(hdnSelectedSlotDate.Value);
                    Calendar1.FocusedDate = Convert.ToDateTime(hdnSelectedSlotDate.Value);//commented by nijanthan
                }
                catch (Exception exp)
                {
                    //logger.Debug("Conversion of Calender Selected Date from hdnSelectedSlotDate.Value='" + hdnSelectedSlotDate.Value + "' to DateTime threw an error.", exp);
                    throw (exp);
                }
                schAppointmentScheduler.SelectedDate = Calendar1.SelectedDate;
                if (hdnApptFacName.Value != string.Empty)
                {
                    cboFacilityName.Text = hdnApptFacName.Value;
                }

                Stopwatch Timer1 = new Stopwatch();
                Timer1.Start();
                if (hdnSourceScreen.Value == "AppointmentFacility" || ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    FillCheckListBoxForPhysicianSetUp(hdnApptFacName.Value, true);
                else
                    FillCheckListBoxForFOSetUp(cboFacilityName.Text, true);

                if (chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Any(li => li.Selected))
                    schAppointmentScheduler.Visible = true;
                bChklstProvidersItemsLoaded = true;
                Timer1.Stop();
                //logger.Debug("Loading Facilities or Physicians for combobox and filling the Scheduler as per Physician, Facility and Date. Time Taken : " + Timer1.Elapsed.Seconds + "." + Timer1.Elapsed.Milliseconds + "s.");
            }

            if (hdnEditApptPhyID.Value != string.Empty && hdnEditApptPhyID.Value.ToUpper() != "UNDEFINED" && !bChklstProvidersItemsLoaded)
            {

                List<System.Web.UI.WebControls.ListItem> selected = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).ToList();
                if (hdnSourceScreen.Value == "AppointmentFacility")
                {
                    if (!(ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT"))
                        FillCheckListBoxForPhysicianSetUp(hdnApptFacName.Value, false);
                }
                else
                    FillCheckListBoxForFOSetUp(cboFacilityName.Text, false);
                for (int j = 0; j < chklstProviders.Items.Count; j++)
                {
                    if (selected.Any(item => item.Text.ToString() == chklstProviders.Items[j].Text.ToString()) || chklstProviders.Items[j].Value.Trim() == hdnEditApptPhyID.Value.Trim())
                        chklstProviders.Items[j].Selected = true;
                    else
                        chklstProviders.Items[j].Selected = false;
                }
                if (chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Any(li => li.Selected))
                    schAppointmentScheduler.Visible = true;
            }
            Stopwatch Timer2 = new Stopwatch();
            Timer2.Start();
            FillAllAppointmentsForDate();
            Timer2.Stop();
            //logger.Debug("filling the Scheduler as per Physician, Facility and Date. Time Taken : " + Timer2.Elapsed.Seconds + "." + Timer2.Elapsed.Milliseconds + "s.");

            Refresh.Stop();
            lblFillAppointments.Text = "Refresh -Minutes: " + Refresh.Elapsed.Minutes.ToString() + " Seconds :" + Refresh.Elapsed.Seconds.ToString() + " MilliSec : " + Refresh.Elapsed.Milliseconds.ToString();
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //Jira CAP-1953
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EmptyValue", "$('#ctl00_C5POBody_hdnStopLoading')[0].value='';", true);
            //logger.Debug("frmAppointments btnRefresh Click Event Completed. Time Taken: " + Refresh.Elapsed.Seconds + "." + Refresh.Elapsed.Milliseconds + "s.");
        }

        protected void btnShowToday_Click(object sender, EventArgs e)
        {
            //CAP-836
            try
            {
                if (!string.IsNullOrEmpty(ClientSession.LocalDate))
                {
                    Calendar1.SelectedDate = Convert.ToDateTime(ClientSession.LocalDate);

                    schAppointmentScheduler.SelectedDate = Calendar1.SelectedDate;
                    if (Calendar1.SelectedDate.ToString("dd-MMM-yyyy") != string.Empty)
                    {
                        hdnSelectedDate.Value = Calendar1.SelectedDate.ToString("dd-MMM-yyyy");
                    }
                    Calendar1.FocusedDate = Convert.ToDateTime(ClientSession.LocalDate);
                    FillAllAppointmentsForDate();
                }
            }
            catch { }
        }

        protected void schAppointmentScheduler_AppointmentContextMenuItemClicked(object sender, AppointmentContextMenuItemClickedEventArgs e)
        {
            if (e.MenuItem.Text == "Cancel")
            {

            }
        }

        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {
            int iCloseType = 0;

            if (hdnSelectedMenu.Value != string.Empty)
            {
                switch (hdnSelectedMenu.Value)
                {
                    case WalkedAwayMenuText:

                        WFObjectManager wfObjMngr = new WFObjectManager();
                        WFObject DocumentationWfObject = null;
                        DocumentationWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(hdnEncounterID.Value), "DOCUMENTATION");

                        if (DocumentationWfObject.Current_Process != "MA_PROCESS")
                        {
                            //Jira CAP-1953
                            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "alert('The selected appointment is not in MA_PROCESS. So, this can not be marked as WALKED_AWAY.'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "ProcessMissMatchAlert('not in MA_PROCESS-WALKED_AWAY');", true);
                            return;
                        }
                        //if (DocumentationWfObject.Current_Process == "PROVIDER_PROCESS_WAIT")
                        //{
                        wfObjMngr.DeleteDocumentationObject(DocumentationWfObject);
                        //}
                        WFObject BillingWfObject = null;
                        BillingWfObject = wfObjMngr.GetByObjectSystemId(Convert.ToUInt64(hdnEncounterID.Value), "BILLING");
                        //if (DocumentationWfObject.Current_Process == "PROVIDER_PROCESS_WAIT")
                        //{
                        wfObjMngr.DeleteDocumentationObject(BillingWfObject);
                        //}
                        iCloseType = 2;
                        break;
                    case NoShowMenuText:
                        //Jira CAP-1953
                        WFObjectManager wfObjMngrnsh = new WFObjectManager();
                        WFObject DocumentationWfObjectnsh = null;
                        DocumentationWfObjectnsh = wfObjMngrnsh.GetByObjectSystemId(Convert.ToUInt64(hdnEncounterID.Value), "Encounter");
                        if (DocumentationWfObjectnsh.Current_Process != "SCHEDULED")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "ProcessMissMatchAlert('not in SCHEDULED-NO_SHOW');", true);
                            return;
                        }
                        iCloseType = 2;
                        break;
                    case ElectronicSuperBill:
                        iCloseType = 4;
                        ulong ulEncounterID = 0;
                        DateTime dt = DateTime.MinValue;
                        ulong SelectedPhyId = 0;
                        if (hdnEncounterID.Value != string.Empty && hdnLocalTime.Value != string.Empty)
                        {
                            ulEncounterID = Convert.ToUInt64(hdnEncounterID.Value);
                            dt = Convert.ToDateTime(hdnLocalTime.Value);
                        }
                        if (hdnApptPhyId.Value != string.Empty)
                        {
                            SelectedPhyId = Convert.ToUInt64(hdnApptPhyId.Value);
                        }
                        EncMngr.InsertToWfobjectforElectronicsuperbill(ulEncounterID, dt, hdnApptFacName.Value, ClientSession.UserName, SelectedPhyId);
                        FillAllAppointmentsForDate();
                        return;

                    case UndoText:
                        //Jira CAP-1953
                        WFObjectManager wfObjMngrnundo = new WFObjectManager();
                        WFObject DocumentationWfObjectnundo = null;
                        DocumentationWfObjectnundo = wfObjMngrnundo.GetByObjectSystemId(Convert.ToUInt64(hdnEncounterID.Value), "Encounter");
                        hdnMyEncounterStatus.Value = DocumentationWfObjectnundo.Current_Process;

                        //Jira CAP-1953 - commented
                        //if (hdnMyEncounterStatus.Value.ToUpper() == "MA_PROCESS" || hdnMyEncounterStatus.Value.ToUpper() == "CHECK_OUT_WAIT")
                        //{
                        //    iCloseType = 1;
                        //}
                        //else if (hdnMyEncounterStatus.Value.ToUpper() == "NO_SHOW")
                        if (hdnMyEncounterStatus.Value.ToUpper() == "NO_SHOW")
                        {
                            iCloseType = 2;
                        }
                        else if (hdnMyEncounterStatus.Value.ToUpper() == "WALKED_AWAY")
                        {
                            iCloseType = 1;
                        }
                        //else if (hdnMyEncounterStatus.Value.ToUpper() == "CHECK_OUT_WAIT")
                        //{
                        //    iCloseType = 4;
                        //    WFObject objDocSuperBill = wfMngr.GetByObjectSystemIDandCurrentProcess(Convert.ToUInt64(hdnEncounterID.Value), "DOCUMENTATION");
                        //    if (objDocSuperBill.Current_Process.ToUpper() == "E_SUPERBILL")
                        //    {
                        //        // wfMngr.DeleteESuperBillDocumentation(Convert.ToUInt64(hdnEncounterID.Value), "DOCUMENTATION");
                        //        wfMngr.MoveToNextProcess(Convert.ToUInt64(hdnEncounterID.Value), "DOCUMENTATION", 5, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty, null, null);
                        //    }
                        //    else
                        //    {
                        //        return;
                        //    }
                        //}
                        else
                        {
                            //Jira CAP-1953
                            //return;
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "ProcessMissMatchAlert('in "+ hdnMyEncounterStatus.Value + "-Undo');", true);
                            return;
                        }
                        if (hdnEncounterID.Value != string.Empty && hdnLocalTime.Value != string.Empty)
                        {
                            //Jira #CAP-121 - Appointment Empty Current Process is fixed
                            int iMoveToPreviousProcess = wfMngr.MoveToPreviousProcess(Convert.ToUInt64(hdnEncounterID.Value), "ENCOUNTER", iCloseType, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty);
                            if (iMoveToPreviousProcess == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DisplayErrorMessage('110096');", true);
                                return;
                            }
                            if (hdnMyEncounterStatus.Value.ToUpper() == "NO_SHOW" || hdnMyEncounterStatus.Value.ToUpper() == "WALKED_AWAY")//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                                EncMngr.TriggerSPforProvReviewStatusTracker("WALK_IN", Convert.ToUInt64(hdnEncounterID.Value));
                            FillAllAppointmentsForDate();
                        }
                        return;

                    case PaperSuperBill:
                        IList<Encounter> objEncounterList = new List<Encounter>();
                        ulong ulEncID = 0;
                        DateTime dt1 = DateTime.MinValue;
                        //ulong SelectedPhyId1 = 0;
                        if (hdnEncounterID.Value != string.Empty)
                        {
                            ulEncID = Convert.ToUInt64(hdnEncounterID.Value);
                        }
                        if (hdnApptPhyId.Value != string.Empty)
                        {
                            SelectedPhyId = Convert.ToUInt64(hdnApptPhyId.Value);
                        }
                        // objEncounterList = EncMngr.GetEncounterByEncounterID(ulEncID);

                        //if (objEncounterList.Count > 0)
                        //{
                        //if (objEncounterList[0].Encounter_Mode == string.Empty)
                        //{
                        //    objEncounterList[0].Encounter_Mode = "EHR";
                        //    EncMngr.UpdateEncounter(objEncounterList[0], string.Empty, new object[] { "false" });
                        //}

                        //if (objEncounterList[0].Encounter_Mode == "EHR")
                        //{
                        //objEncounterList[0].Encounter_Mode = "PAPER SUPERBILL";
                        //EncMngr.UpdateEncounter(objEncounterList[0], string.Empty, new object[] { "false" });
                        //EncMngr.InsertToWfobjectforPrintsuperbill(ulEncID, dt1, hdnApptFacName.Value, ClientSession.UserName, SelectedPhyId1);
                        FillAllAppointmentsForDate();
                        //}
                        //}
                        break;

                }

                IList<Encounter> lstencounter = new List<Encounter>();
                EncounterManager obj = new EncounterManager();
                lstencounter = obj.GetEncounterByEncounterID(Convert.ToUInt64(hdnEncounterID.Value));
                //CAP-1275
                if (hdnSelectedMenu.Value == WalkedAwayMenuText && lstencounter.Count > 0)
                {
                    //Jira CAP-2956
                    bool bIsWriteinBlob = false;
                    if (lstencounter.FirstOrDefault().Date_of_Service.ToString("yyyy-MM-dd").Contains("0001-01-01"))
                    {
                        bIsWriteinBlob = false;
                    }
                    else
                    {
                        bIsWriteinBlob = true;
                    }
                    lstencounter[0].Date_of_Service = DateTime.MinValue;
                    //Jira CAP-2956
                    //obj.UpdateEncounter(lstencounter[0], string.Empty, new object[] { "false" });
                    obj.UpdateEncounter(lstencounter[0], string.Empty, new object[] { "false" }, bIsWriteinBlob);
                }
                if (lstencounter.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "alert('The selected appointment is moved to archive table. So, this transaction is not supported.'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    return;
                }
                if (hdnEncounterID.Value != string.Empty && hdnLocalTime.Value != string.Empty && hdnSelectedMenu.Value != PaperSuperBill)
                {
                    wfMngr.MoveToNextProcess(Convert.ToUInt64(hdnEncounterID.Value), "ENCOUNTER", iCloseType, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty, null, null);
                    if (hdnSelectedMenu.Value == "Walked Away" || hdnSelectedMenu.Value == "No Show")
                        EncMngr.TriggerSPforProvReviewStatusTracker("INVALID", Convert.ToUInt64(hdnEncounterID.Value));
                    FillAllAppointmentsForDate();
                }
            }
        }

        protected void chkShowActive_CheckedChanged(object sender, EventArgs e)
        {
            #region New Code
            #region Physician Login
            if (chkShowActive.Text == "Show All")
            {
                if (chkShowActive.Checked == true)
                {
                    IList<FacilityLibrary> FacList = new List<FacilityLibrary>();
                    DateTime dtStart = new DateTime();
                    DateTime dtEnd = new DateTime();

                    //FacList = ApplicationObject.facilityLibraryList;
                    //CAP-3268
                    PhysicianManager physicianManager = new PhysicianManager();
                    //CAP-3493
                    var lstMappedFacility = physicianManager.GetFacilityListMappedByPhysician(Convert.ToUInt64(string.IsNullOrEmpty(hdnApptPhyId.Value) ? "0" : hdnApptPhyId.Value));
                    var lstFacilityNames = lstMappedFacility.Select(a => a.Facility_Name).ToList();

                    var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg && lstFacilityNames.Contains(f.Fac_Name) select f;
                    FacList = fac.ToList<FacilityLibrary>();
                    //CAP-3493
                    if (FacList != null && FacList.Count > 0)
                    {
                        if (FacList[0].Start_Time != string.Empty)
                            dtStart = Convert.ToDateTime(FacList[0].Start_Time);
                        if (FacList[0].End_Time != string.Empty)
                            dtEnd = Convert.ToDateTime(FacList[0].End_Time);
                    }

                    try
                    {
                        schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
                        schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
                    }
                    catch
                    {
                    }
                    List<System.Web.UI.WebControls.ListItem> selected = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).ToList();
                    chklstProviders.Items.Clear();

                    if (lstMappedFacility != null)
                    {
                        if (lstMappedFacility.Count == 0)
                        {
                            schAppointmentScheduler.Visible = false;
                            return;
                        }
                        else
                        {
                            schAppointmentScheduler.Visible = true;
                        }
                        for (int i = 0; i < lstMappedFacility.Count; i++)
                        {
                            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                            item.Attributes["data-status"] = lstMappedFacility[i].Status;
                            item.Text = lstMappedFacility[i].Facility_Name;
                            chklstProviders.Items.Add(item);
                            if (selected.Any(item1 => item1.Text.ToString() == chklstProviders.Items[i].Text.ToString()))
                                chklstProviders.Items[i].Selected = true;
                            else
                                chklstProviders.Items[i].Selected = false;
                        }
                    }
                    if (selected.Count == 0)
                    {
                        schAppointmentScheduler.Visible = false;
                        RefreshMyResources();
                    }
                }
                else
                {
                    IList<MapFacilityPhysician> FacList = new List<MapFacilityPhysician>();
                    if (cboFacilityName.SelectedItem != null)
                    {
                        //CAP-3491
                        //FacList = UtilityManager.GetFacilityListMappedToPhysician(cboFacilityName.SelectedItem.Value);
                        PhysicianManager physicianManager = new PhysicianManager();
                        FacList = physicianManager.GetFacilityListMappedByPhysician(Convert.ToUInt64(string.IsNullOrEmpty(hdnApptPhyId.Value) ? "0" : hdnApptPhyId.Value));
                        if(FacList != null && FacList.Count > 0)
                        {
                            FacList = FacList.Where(a => a.Status == "Y").ToList();
                        }
                    }

                    List<System.Web.UI.WebControls.ListItem> selected = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).ToList();
                    chklstProviders.Items.Clear();

                    if (FacList != null)
                    {
                        if (FacList.Count == 0)
                        {
                            schAppointmentScheduler.Visible = false;
                            return;
                        }
                        else
                        {
                            schAppointmentScheduler.Visible = true;
                        }

                        for (int i = 0; i < FacList.Count; i++)
                        {
                            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                            item.Text = FacList[i].Facility_Name;
                            item.Attributes["data-status"] = FacList[i].Status;
                            chklstProviders.Items.Add(item);
                            chklstProviders.Items[i].Selected = true;
                            if (selected.Any(item1 => item1.Text.ToString() == chklstProviders.Items[i].Text.ToString()))
                                chklstProviders.Items[i].Selected = true;
                            else
                                chklstProviders.Items[i].Selected = false;
                        }
                        List<System.Web.UI.WebControls.ListItem> newly_selected = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).ToList();
                        if (newly_selected.Count == 0)
                        {
                            schAppointmentScheduler.Visible = false;
                            RefreshMyResources();
                        }
                        else if (newly_selected.Count != selected.Count)
                            FillAllAppointmentsForDate();
                    }
                }
            }
            #endregion
            #region FO or MA Login
            else
            {
                //CAP-3268
                if (rdoAllActiveProviders.Checked)
                { chkShowActive.Checked = true; }
                if (rdoActiveProviders.Checked || rdoInActiveProviders.Checked)
                { chkShowActive.Checked = false; }

                IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();

                //IList<FacilityLibrary> facList = ApplicationObject.facilityLibraryList;
                var faclist = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
                IList<FacilityLibrary> facList = faclist.ToList<FacilityLibrary>();
                IList<FacilityLibrary> TempFac = new List<FacilityLibrary>();
                if (facList.Count > 0 && cboFacilityName.Text != string.Empty)
                {
                    var fac = from f in facList where f.Fac_Name == cboFacilityName.Text select f;
                    TempFac = fac.ToList<FacilityLibrary>();

                }
                if (TempFac[0].Start_Time != string.Empty)
                {
                    dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
                }
                if (TempFac[0].End_Time != string.Empty)
                {
                    dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
                }

                try
                {
                    schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
                    schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
                }
                catch
                {
                }
                //CAP-3268
                //if (chkShowActive.Checked)
                //    PhysicianList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);
                //else
                //    PhysicianList = UtilityManager.GetPhysicianList(cboFacilityName.SelectedItem.Text, ClientSession.LegalOrg);
                if (rdoInActiveProviders.Checked)
                {
                    if (!string.IsNullOrEmpty(cboFacilityName.SelectedItem.Text))
                        PhysicianList = UtilityManager.GetInActiveProviderList(cboFacilityName.SelectedItem.Text, ClientSession.LegalOrg, false);
                }
                else
                {
                    if (chkShowActive.Checked || rdoAllActiveProviders.Checked)
                        PhysicianList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);
                    else
                    {
                        PhysicianList = UtilityManager.GetPhysicianList(cboFacilityName.SelectedItem.Text, ClientSession.LegalOrg);
                        var otherPhysicianList = UtilityManager.GetInActiveProviderList(cboFacilityName.SelectedItem.Text, ClientSession.LegalOrg, true);
                        foreach (var item in otherPhysicianList)
                        {
                            PhysicianList.Add(item);
                        }
                    }
                }

                List<System.Web.UI.WebControls.ListItem> selected = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).ToList();
                chklstProviders.Items.Clear();
                //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                //{
                //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
                //}

                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacilityName.SelectedItem.Text select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();

                if (PhysicianList != null)
                {
                    if (PhysicianList.Count == 0)
                    {
                        schAppointmentScheduler.Visible = false;
                        return;
                    }
                    else
                    {
                        schAppointmentScheduler.Visible = true;
                    }

                    for (int i = 0; i < PhysicianList.Count; i++)
                    {
                        //System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                        //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                        //item.Value = PhysicianList[i].Id.ToString();
                        //chklstProviders.Items.Add(item);

                        System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                        //BugID:53256
                        // if (sAncillary != string.Empty && sAncillary == cboFacilityName.SelectedItem.Text.Trim() && !chkShowActive.Checked)
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && !chkShowActive.Checked)
                        {
                            pnlProvidersHeader.InnerText = "Machine - Technician";
                        }
                        else
                        {
                            pnlProvidersHeader.InnerText = "Providers";
                        }
                        //Jira CAP-2777
                        //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                        //if (File.Exists(strXmlFilePathTech) == true)
                        {
                            //Jira CAP-2777
                            //XmlDocument xmldoc = new XmlDocument();
                            //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                            if (PhysicianList[i].PhyColor != "" && PhysicianList[i].PhyColor != "0")
                            {
                                //Jira CAP-2777
                                //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                                //item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                //item.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;

                                MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                                machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                                if (machinetechnicianList?.MachineTechnician != null)
                                {
                                    List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                    machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                    var filterData = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().ToList();
                                    if ((machinetechnicians?.Count ?? 0) > 0 && !filterData.Any(a => a.Value == machinetechnicians[0].machine_technician_library_id))
                                    {
                                        item.Text = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                        item.Value = machinetechnicians[0].machine_technician_library_id;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }

                            }
                            else
                            {
                                //Old Code
                                //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                //Gitlab# 2485 - Physician Name Display Change
                                if (PhysicianList[i].PhyLastName != String.Empty)
                                    item.Text += PhysicianList[i].PhyLastName;
                                if (PhysicianList[i].PhyFirstName != String.Empty)
                                {
                                    if (item.Text != String.Empty)
                                        item.Text += "," + PhysicianList[i].PhyFirstName;
                                    else
                                        item.Text += PhysicianList[i].PhyFirstName;
                                }
                                if (PhysicianList[i].PhyMiddleName != String.Empty)
                                    item.Text += " " + PhysicianList[i].PhyMiddleName;
                                if (PhysicianList[i].PhySuffix != String.Empty)
                                    item.Text += "," + PhysicianList[i].PhySuffix;

                                item.Value = PhysicianList[i].Id.ToString();
                            }
                            chklstProviders.Items.Add(item);
                        }

                        //CAP-3268
                        //if (selected.Any(item1 => item1.Text.ToString() == chklstProviders.Items[i].Text.ToString()))
                        //    chklstProviders.Items[i].Selected = true;
                        //else
                        //    chklstProviders.Items[i].Selected = false;
                        if (selected.Any(item1 => item1.Text.ToString() == chklstProviders.Items[i].Text.ToString()))
                            chklstProviders.Items[i].Selected = true;
                        //CAP-3268
                        else if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && hdnApptPhyId.Value == PhysicianList[i].PhyColor.ToString())
                            chklstProviders.Items[i].Selected = true;
                        else if ((ilstFacAncillary.Count == 0 || ilstFacAncillary[0].Is_Ancillary != "Y") && hdnApptPhyId.Value == PhysicianList[i].Id.ToString())
                            chklstProviders.Items[i].Selected = true;
                        else
                            chklstProviders.Items[i].Selected = false;
                    }
                    SortPhysician();
                    List<System.Web.UI.WebControls.ListItem> newly_selected = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).ToList();
                    if (newly_selected.Count == 0)
                    {
                        schAppointmentScheduler.Visible = false;
                        RefreshMyResources();
                    }
                    else if (newly_selected.Count != selected.Count)
                        FillAllAppointmentsForDate();
                    else if (!newly_selected.Any(a => selected.Any(x => x.Text == a.Text)))
                        FillAllAppointmentsForDate();
                }
            }
            #endregion
            #endregion
            #region Old Code
            //if (chkShowActive.Text == "Show All")
            //{
            //    if (chkShowActive.Checked == true)
            //    {
            //        FacilityManager facMngr = new FacilityManager();
            //        IList<FacilityLibrary> FacList = new List<FacilityLibrary>();
            //        IList<PhysicianLibrary> phyList = new List<PhysicianLibrary>();
            //        DateTime dtStart = new DateTime();
            //        DateTime dtEnd = new DateTime();
            //        FacList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
            //        //FacList = FacilityMngr.GetFacilityList();
            //        //if (Session["PhysicianList"] != null)
            //        //{
            //        //    phyList = (IList<PhysicianLibrary>)Session["PhysicianList"];  //Codereview PhyMngr.GetPhysicianListbyFacility(hdnApptFacName.Value, "Y");
            //        //}
            //        //else
            //        phyList = UtilityManager.GetPhysicianList(hdnApptFacName.Value);


            //        var phy = from p in phyList
            //                  where p.PhyPrefix == cboFacilityName.Text.Split(' ')[0] && p.PhyFirstName == cboFacilityName.Text.Split(' ')[1]
            //                      && p.PhyMiddleName == cboFacilityName.Text.Split(' ')[2] && p.PhyLastName == cboFacilityName.Text.Split(' ')[3]
            //                  select p.Id;

            //        hdnApptPhyId.Value = phy.First().ToString();
            //        IList<MapFacilityPhysician> FacilityList = new List<MapFacilityPhysician>();
            //        MapFacilityPhysicianManager mapPhyfac = new MapFacilityPhysicianManager();
            //        FacilityList = UtilityManager.GetFacilityListMappedToPhysician(phy.First().ToString());//mapPhyfac.GetMapFacilityListbyPhyID(Convert.ToUInt64(phy.First()));

            //        if (FacList[0].Start_Time != string.Empty)
            //        {
            //            dtStart = Convert.ToDateTime(FacList[0].Start_Time);
            //        }
            //        if (FacList[0].End_Time != string.Empty)
            //        {
            //            dtEnd = Convert.ToDateTime(FacList[0].End_Time);
            //        }


            //        try
            //        {
            //            schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
            //            schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
            //        }
            //        catch
            //        {
            //        }

            //        chklstProviders.Items.Clear();

            //        if (FacList != null)
            //        {
            //            if (FacList.Count == 0)
            //            {
            //                schAppointmentScheduler.Visible = false;
            //                return;
            //            }
            //            else
            //            {
            //                schAppointmentScheduler.Visible = true;
            //            }
            //            for (int i = 0; i < FacList.Count; i++)
            //            {
            //                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //                item.Text = FacList[i].Fac_Name;
            //                item.Value = phy.First().ToString();
            //                chklstProviders.Items.Add(item);

            //            }
            //            if (chklstProviders.Items.Count > 0)
            //            {
            //                for (int i = 0; i < FacilityList.Count; i++)
            //                {
            //                    for (int j = 0; j < chklstProviders.Items.Count; j++)
            //                    {
            //                        if (chklstProviders.Items[j].Text == FacilityList[i].Facility_Name)
            //                        {
            //                            chklstProviders.Items[j].Selected = true;
            //                        }
            //                    }
            //                }
            //            }
            //            FillAllAppointmentsForDate();

            //        }

            //    }
            //    else
            //    {
            //        // new functionality - show all implies all physicians irrespective of facility - Pujhitha
            //        IList<PhysicianLibrary> phyList = UtilityManager.GetPhysicianList("");//PhyMngr.GetPhysicianListbyFacility(hdnApptFacName.Value, "N"); //Codereview (IList<PhysicianLibrary>)Session["PhysicianList"];
            //        var phy = from p in phyList
            //                  where p.PhyPrefix == cboFacilityName.Text.Split(' ')[0] && p.PhyFirstName == cboFacilityName.Text.Split(' ')[1]
            //                      && p.PhyMiddleName == cboFacilityName.Text.Split(' ')[2] && p.PhyLastName == cboFacilityName.Text.Split(' ')[3]
            //                  select p.Id;

            //        FacilityManager facMngr = new FacilityManager();
            //        IList<MapFacilityPhysician> FacList = new List<MapFacilityPhysician>();
            //        FacList = UtilityManager.GetFacilityListMappedToPhysician(phy.First().ToString());//facMngr.GetFacilityByPhysicianID(Convert.ToUInt64(phy.First()));

            //        chklstProviders.Items.Clear();

            //        if (FacList != null)
            //        {
            //            if (FacList.Count == 0)
            //            {
            //                schAppointmentScheduler.Visible = false;
            //                return;
            //            }
            //            else
            //            {
            //                schAppointmentScheduler.Visible = true;
            //            }

            //            for (int i = 0; i < FacList.Count; i++)
            //            {
            //                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //                item.Text = FacList[i].Facility_Name;
            //                item.Value = phy.First().ToString();
            //                chklstProviders.Items.Add(item);
            //                chklstProviders.Items[i].Selected = true;
            //            }

            //            FillAllAppointmentsForDate();
            //        }
            //    }
            //}
            //else
            //{
            //    IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
            //    DateTime dtStart = new DateTime();
            //    DateTime dtEnd = new DateTime();
            //    //string sActive = string.Empty;
            //    //if (chkShowActive.Checked == true)
            //    //{
            //    //    sActive = "Y";
            //    //}
            //    //else
            //    //{
            //    //    sActive = "N";
            //    //}
            //    IList<FacilityLibrary> facList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
            //    IList<FacilityLibrary> TempFac = new List<FacilityLibrary>();
            //    if (facList.Count > 0 && cboFacilityName.Text != string.Empty)
            //    {
            //        var fac = from f in facList where f.Fac_Name == cboFacilityName.Text select f;
            //        TempFac = fac.ToList<FacilityLibrary>();

            //    }
            //    if (TempFac[0].Start_Time != string.Empty)
            //    {
            //        dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
            //    }
            //    if (TempFac[0].End_Time != string.Empty)
            //    {
            //        dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
            //    }

            //    try
            //    {
            //        schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
            //        schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
            //    }
            //    catch
            //    {
            //    }

            //    if (chkShowActive.Checked)
            //        PhysicianList = UtilityManager.GetPhysicianList("");//PhyMngr.GetPhysicianListbyFacility(cboFacilityName.SelectedValue, sActive);
            //    else
            //        PhysicianList = UtilityManager.GetPhysicianList(cboFacilityName.SelectedValue.ToUpper());
            //    // Session["PhysicianList"] = PhysicianList;
            //    chklstProviders.Items.Clear();

            //    if (PhysicianList != null)
            //    {
            //        if (PhysicianList.Count == 0)
            //        {
            //            schAppointmentScheduler.Visible = false;
            //            return;
            //        }
            //        else
            //        {
            //            schAppointmentScheduler.Visible = true;
            //        }

            //        // new code instead of application_lookup
            //        XmlDocument xmldoc = new XmlDocument();
            //        string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
            //        string sDefaultPhysicians = "";
            //        if (File.Exists(strXmlFilePath) == true)
            //        {
            //            xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");

            //            XmlNode nodeMatchingFacility = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + cboFacilityName.SelectedValue.Trim().ToUpper() + "']");
            //            sDefaultPhysicians = nodeMatchingFacility.Attributes["default-physician-id"].Value.ToString();
            //        }
            //        string[] lstDefaultPhysicians = sDefaultPhysicians.Split(',');
            //        for (int i = 0; i < PhysicianList.Count; i++)
            //        {
            //            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //            // item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            //            item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
            //            //string sPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            //            //item.Text = sPhyName;
            //            item.Value = PhysicianList[i].Id.ToString();
            //            chklstProviders.Items.Add(item);
            //            if (lstDefaultPhysicians.Any(curritem => curritem == PhysicianList[i].Id.ToString()))
            //                chklstProviders.Items[i].Selected = true;
            //            else
            //                chklstProviders.Items[i].Selected = false;
            //        }
            //        if (chklstProviders.Items.Count > 0 && chklstProviders.SelectedItem != null)
            //        {
            //            FillAllAppointmentsForDate();
            //        }
            //        //if (chklstProviders.Items.Count > 0)
            //        //{


            //        //    IList<AppointmentLookup> apptLookupList = new List<AppointmentLookup>();
            //        //    apptLookupList = AppointmentMngr.GetAppointmentLookupList(cboFacilityName.SelectedValue);

            //        //    for (int i = 0; i < apptLookupList.Count; i++)
            //        //    {
            //        //        for (int j = 0; j < chklstProviders.Items.Count; j++)
            //        //        {
            //        //            if (Convert.ToUInt64(chklstProviders.Items[j].Value) == apptLookupList[i].Physician_ID)
            //        //            {
            //        //                chklstProviders.Items[j].Selected = true;
            //        //            }
            //        //        }
            //        //    }
            //        //}

            //    }
            //}
            #endregion
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        //protected void btnLoadPhysician_Click(object sender, EventArgs e)
        //{
        //    if (hdnSourceScreen.Value == "AppointmentFacility")
        //    {
        //        //commented for code review IList<PhysicianLibrary> phyList = (IList<PhysicianLibrary>)Session["PhysicianList"];
        //        if (ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN ASSISTANT")
        //        {

        //        }
        //        else
        //        {
        //            //string sActive = string.Empty;
        //            //if (chkShowActive.Checked == true)
        //            //{
        //            //    sActive = "Y";
        //            //}
        //            //else
        //            //{
        //            //    sActive = "N";
        //            //}
        //            IList<PhysicianLibrary> phyList = UtilityManager.GetPhysicianList(cboFacilityName.SelectedValue.ToUpper());//PhyMngr.GetPhysicianListbyFacility(cboFacilityName.SelectedValue, sActive); //Codereview (IList<PhysicianLibrary>)Session["PhysicianList"];
        //            if (bTriggeredFromRefreshButton)
        //            {
        //                bTriggeredFromRefreshButton = false;
        //                if (!phyList.Any(item => item.Id.ToString() == hdnEditApptPhyID.Value))
        //                {
        //                    phyList = UtilityManager.GetPhysicianList("");
        //                    chkShowActive.Checked = true;
        //                }
        //            }
        //            var phy = from p in phyList
        //                      where p.PhyPrefix == cboFacilityName.Text.Split(' ')[0] && p.PhyFirstName == cboFacilityName.Text.Split(' ')[1]
        //                          && p.PhyMiddleName == cboFacilityName.Text.Split(' ')[2] && p.PhyLastName == cboFacilityName.Text.Split(' ')[3]
        //                      select p.Id;


        //            IList<MapFacilityPhysician> facList = new List<MapFacilityPhysician>();
        //            MapFacilityPhysicianManager mapPhyfac = new MapFacilityPhysicianManager();
        //            facList = UtilityManager.GetFacilityListMappedToPhysician(phy.First().ToString());//mapPhyfac.GetMapFacilityListbyPhyID(Convert.ToUInt64(phy.First()));

        //            IList<FacilityLibrary> facilityList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
        //            IList<FacilityLibrary> TempFac = new List<FacilityLibrary>();
        //            if (facilityList.Count > 0 && facList[0].Facility_Name != string.Empty)
        //            {
        //                var fac = from f in facilityList where f.Fac_Name == facList[0].Facility_Name select f;
        //                TempFac = fac.ToList<FacilityLibrary>();
        //            }

        //            DateTime dtStart = new DateTime();
        //            DateTime dtEnd = new DateTime();
        //            if (TempFac[0].Start_Time != string.Empty)
        //            {
        //                dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
        //            }
        //            if (TempFac[0].End_Time != string.Empty)
        //            {
        //                dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
        //            }

        //            try
        //            {
        //                schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
        //                schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
        //            }
        //            catch
        //            {
        //            }

        //            chklstProviders.Items.Clear();

        //            if (facList != null)
        //            {
        //                if (facList.Count == 0)
        //                {
        //                    schAppointmentScheduler.Visible = false;
        //                    return;
        //                }
        //                else
        //                {
        //                    schAppointmentScheduler.Visible = true;
        //                }

        //                for (int i = 0; i < facList.Count; i++)
        //                {
        //                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
        //                    item.Text = facList[i].Facility_Name;
        //                    item.Value = facList[i].Phy_Rec_ID.ToString();
        //                    chklstProviders.Items.Add(item);

        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();

        //        //string sActive = string.Empty;
        //        //if (chkShowActive.Checked == true)
        //        //{
        //        //    sActive = "Y";
        //        //}
        //        //else
        //        //{
        //        //    sActive = "N";
        //        //}
        //        IList<FacilityLibrary> facList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
        //        IList<FacilityLibrary> TempFac = new List<FacilityLibrary>();
        //        if (facList.Count > 0 && cboFacilityName.Text != string.Empty)
        //        {
        //            var fac = from f in facList where f.Fac_Name == cboFacilityName.Text select f;
        //            TempFac = fac.ToList<FacilityLibrary>();

        //        }
        //        DateTime dtStart = new DateTime();
        //        DateTime dtEnd = new DateTime();
        //        if (TempFac[0].Start_Time != string.Empty)
        //        {
        //            dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
        //        }
        //        if (TempFac[0].End_Time != string.Empty)
        //        {
        //            dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
        //        }

        //        try
        //        {
        //            schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
        //            schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
        //        }
        //        catch
        //        {
        //        }
        //        PhysicianList = UtilityManager.GetPhysicianList(cboFacilityName.SelectedValue.ToUpper());//PhyMngr.GetPhysicianListbyFacility(cboFacilityName.SelectedValue, sActive);
        //        if (bTriggeredFromRefreshButton)
        //        {
        //            bTriggeredFromRefreshButton = false;
        //            if (!PhysicianList.Any(item => item.Id.ToString() == hdnEditApptPhyID.Value))
        //            {
        //                PhysicianList = UtilityManager.GetPhysicianList("");
        //                chkShowActive.Checked = true;
        //            }
        //        }
        //        chklstProviders.Items.Clear();

        //        if (PhysicianList != null)
        //        {
        //            if (PhysicianList.Count == 0)
        //            {
        //                schAppointmentScheduler.Visible = false;
        //                return;
        //            }
        //            else
        //            {
        //                schAppointmentScheduler.Visible = true;
        //            }
        //            for (int i = 0; i < PhysicianList.Count; i++)
        //            {
        //                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
        //                //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
        //                item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
        //                //string sPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
        //                //item.Text = sPhyName;
        //                item.Value = PhysicianList[i].Id.ToString();
        //                chklstProviders.Items.Add(item);

        //            }
        //        }
        //    }
        //}

        protected void schAppointmentScheduler_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
        {
            //IList<string> objTypeOfVisit = new List<string>();
            //IList<StaticLookup> objcolor = new List<StaticLookup>();
            //if (Session["TypeofVisit"] != null)
            //{
            //    objTypeOfVisit = (IList<string>)Session["TypeofVisit"]; //ViewState["TypeofVisit"];
            //}
            //if (Session["TypeofvisitColorList"] != null)
            //{
            //    objcolor = (IList<StaticLookup>)Session["TypeofvisitColorList"];//ViewState["TypeofvisitColorList"];
            //}

            //int year = 0;
            //int month = 0;
            //int day = 0;
            //int fhour = 0;
            //int fminiute = 0;
            //int fsec = 0;
            //int thour = 0;
            //int tminiute = 0;
            //int tsec = 0;
            //if (objTypeOfVisit != null)
            //{
            //    for (int i = 0; i < objTypeOfVisit.Count; i++)
            //    {
            //        string[] split = objTypeOfVisit[i].Split('|');

            //        fhour = Convert.ToInt32(split[1].Split(':')[0]);
            //        fminiute = Convert.ToInt32(split[1].Split(':')[1]);
            //        fsec = 0;
            //        thour = Convert.ToInt32(split[2].Split(':')[0]);
            //        tminiute = Convert.ToInt32(split[2].Split(':')[1]);
            //        tsec = 0;
            //        year = Convert.ToInt32(Convert.ToDateTime(split[3]).ToString("dd-MM-yyyy").Split('-')[2]);
            //        month = Convert.ToInt32(Convert.ToDateTime(split[3]).ToString("dd-MM-yyyy").Split('-')[1]);
            //        day = Convert.ToInt32(Convert.ToDateTime(split[3]).ToString("dd-MM-yyyy").Split('-')[0]);
            //        if (e.TimeSlot.Start >= new DateTime(year, month, day, fhour, fminiute, fsec) & e.TimeSlot.End <= new DateTime(year, month, day, thour, tminiute, tsec))
            //        {
            //            if (hdnSourceScreen.Value == "AppointmentFacility")
            //            {
            //                if (e.TimeSlot.Resource.Key.ToString() == split[4].ToString())
            //                {
            //                    e.TimeSlot.CssClass = objcolor.Where(a => a.Value == split[0].ToString()).ToList<StaticLookup>()[0].Description;
            //                    e.TimeSlot.Control.ToolTip = split[0].ToString();
            //                }
            //            }
            //            else
            //            {
            //                if (e.TimeSlot.Resource.Key.ToString() == split[4].ToString())
            //                {
            //                    e.TimeSlot.CssClass = objcolor.Where(a => a.Value == split[0].ToString()).ToList<StaticLookup>()[0].Description;
            //                    //e.TimeSlot.CssClass =  (from doc in objcolor where doc.Value== split[0].ToString() select doc.Description).ToString();
            //                    e.TimeSlot.Control.ToolTip = split[0].ToString();

            //                }
            //            }
            //        }
            //    }
            //}
            if (schAppointmentScheduler.SelectedView == SchedulerViewType.MonthView)
            {
                if (e.TimeSlot.Start.Date.DayOfWeek == DayOfWeek.Saturday || e.TimeSlot.Start.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    e.TimeSlot.CssClass = "MyClass";
                }
            }
        }

        protected void btnPhysiciancalenderFacility_Click(object sender, EventArgs e)
        {
            IList<ulong> PhyIDList = new List<ulong>();
            //CAP-3493
            if (chklstProviders.Items.Count > 0)
            {
                for (int k = 0; k < chklstProviders.Items.Count; k++)
                {
                    if (chklstProviders.Items[k].Selected == true)
                    {
                        PhyIDList.Add(Convert.ToUInt64(chklstProviders.Items[k].Value));
                        //Added by bala for Appointment facility base screen
                        hdnApptPhyId.Value = chklstProviders.Items[k].Value;
                    }
                }
            }
            else
            {
                hdnApptPhyId.Value = "0";
            }
            hdnSourceScreen.Value = "AppointmentFacility";
            hdnApptFacName.Value = cboFacilityName.SelectedItem.Text.Replace("#", "_");
            ModalWindow.NavigateUrl = "frmAppointments.aspx?hdnApptPhyId=" + hdnApptPhyId.Value + "&hdnApptFacName=" + hdnApptFacName.Value + "&hdnSourceScreen=" + hdnSourceScreen.Value + "&MasterPage=YES";
            //ModalWindow.Height = Unit.Pixel(890);
            ModalWindow.Height = Unit.Pixel(700);
            ModalWindow.VisibleOnPageLoad = true;
            //ModalWindow.Width = Unit.Pixel(1265);
            ModalWindow.Width = Unit.Pixel(1250);
            //Jira CAP-1217
            //ModalWindow.Behaviors = WindowBehaviors.Close;
            ModalWindow.Behaviors = WindowBehaviors.None;
            ModalWindow.Visible = true;
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "AppointmentFacility", "OpenPhysicianFacility();", true);
            hdnSourceScreen.Value = "";
        }

        protected void chkShowAllPhysicians_CheckedChanged(object sender, EventArgs e)
        {

            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
            //CAP-3268
            //if (chkShowAllPhysicians.Checked == true)
            //            PhysicianList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);//PhyMngr.LoadPhysicianList();
            //    else
            //    {
            //    if (chklstProviders.SelectedItem != null)
            //        PhysicianList = UtilityManager.GetPhysicianList(chklstProviders.SelectedItem.Text, ClientSession.LegalOrg);//PhyMngr.GetPhysicianListbyFacility(chklstProviders.SelectedItem.Text, "Y");
            //}
            if (rdoInActivePhysicians.Checked)
            {
                PhysicianList = UtilityManager.GetInActiveProviderList(ClientSession.FacilityName, ClientSession.LegalOrg, false);
            }
            else
            {
                if (rdoAllActivePhysicians.Checked)
                    PhysicianList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);
                else
                {
                    PhysicianList = UtilityManager.GetPhysicianList(ClientSession.FacilityName, ClientSession.LegalOrg);
                    var otherPhysicianList = UtilityManager.GetInActiveProviderList(cboFacilityName.SelectedItem.Text, ClientSession.LegalOrg, true);
                    foreach (var item in otherPhysicianList)
                    {
                        PhysicianList.Add(item);
                    }
                }
            }

            if (PhysicianList.Count > 0)
                Session["PhysicianList"] = PhysicianList;

            this.cboFacilityName.Items.Clear();

            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}

            if (PhysicianList != null)
            {
                for (int i = 0; i < PhysicianList.Count; i++)
                {
                    //System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                    ////cboItem.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                    //cboItem.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                    //cboItem.Value = PhysicianList[i].Id.ToString();
                    //this.cboFacilityName.Items.Add(cboItem);
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    //BugID:53256
                    //Jira CAP-2777
                    //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                    //if (File.Exists(strXmlFilePathTech) == true)
                    {
                        //Jira CAP-2777
                        //XmlDocument xmldoc = new XmlDocument();
                        //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                        if (PhysicianList[i].PhyColor != "" && PhysicianList[i].PhyColor != "0")
                        {
                            //Jira CAP-2777
                            //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                            //item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                            //item.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                            //Jira CAP-2777
                            MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                            machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                            if (machinetechnicianList?.MachineTechnician != null)
                            {
                                List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                if (machinetechnicians.Count > 0)
                                {
                                    item.Text = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                    item.Value = machinetechnicians[0].machine_technician_library_id;
                                }
                            }
                        }
                        else
                        {
                            //old code
                            // item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                            //Gitlab# 2485 - Physician Name Display Change
                            if (PhysicianList[i].PhyLastName != String.Empty)
                                item.Text += PhysicianList[i].PhyLastName;
                            if (PhysicianList[i].PhyFirstName != String.Empty)
                            {
                                if (item.Text != String.Empty)
                                    item.Text += "," + PhysicianList[i].PhyFirstName;
                                else
                                    item.Text += PhysicianList[i].PhyFirstName;
                            }
                            if (PhysicianList[i].PhyMiddleName != String.Empty)
                                item.Text += " " + PhysicianList[i].PhyMiddleName;
                            if (PhysicianList[i].PhySuffix != String.Empty)
                                item.Text += "," + PhysicianList[i].PhySuffix;
                            item.Value = PhysicianList[i].Id.ToString();
                        }
                        this.cboFacilityName.Items.Add(item);
                    }
                    if (hdnApptPhyId.Value == PhysicianList[i].Id.ToString())
                    {
                        //CAP-3458
                        cboFacilityName.ClearSelection();
                        cboFacilityName.Items.FindByText(item.Text).Selected = true;
                        cboFacilityName.Attributes.Add("Tag", PhysicianList[i].Id.ToString());
                    }
                }
                SortPhysician();
                cboFacilityName_SelectedIndexChanged(sender, e);
            }
            
            if (rdoInActivePhysicians.Checked)
            {
                chkShowActive.Checked = true;
            }
            else
            {
                //CAP-3491
                //chkShowActive.Enabled = false;
                chkShowActive.Checked = false;
            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        //Added by bala for bug id - 18123
        protected void btnBlockDays_Click(object sender, EventArgs e)
        {
            hdnPhysisicanChecked.Value = string.Empty;

            for (int k = 0; k < chklstProviders.Items.Count; k++)
            {
                if (chklstProviders.Items[k].Selected == true)
                {
                    if (hdnPhysisicanChecked.Value == string.Empty)
                    {
                        hdnPhysisicanChecked.Value = chklstProviders.Items[k].Text;
                    }
                    else
                    {
                        hdnPhysisicanChecked.Value += "|" + chklstProviders.Items[k].Text;
                    }
                }
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "AppointmentFacility", "OpenBlockDays('" + cboFacilityName.SelectedItem.Text.Replace("#", "_") + "','" + hdnPhysisicanChecked.Value + "');",true);
            if (ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN ASSISTANT")
            {
                ModalWindow.NavigateUrl = "frmBlockDays.aspx?FacilityName=" + ClientSession.FacilityName.Replace("#", "_") + "&PhysicianSelected=" + hdnPhysisicanChecked.Value;
            }
            else
            {
                ModalWindow.NavigateUrl = "frmBlockDays.aspx?FacilityName=" + ClientSession.FacilityName.Replace("#", "_") + "&PhysicianSelected=" + hdnPhysisicanChecked.Value;
            }
            ModalWindow.Height = 540;
            ModalWindow.VisibleOnPageLoad = true;
            ModalWindow.Width = 1250;
            ModalWindow.CenterIfModal = true;
            ModalWindow.VisibleTitlebar = true;
            ModalWindow.VisibleStatusbar = false;
            ModalWindow.Behaviors = WindowBehaviors.None;
            //ModalWindow.Behaviors = WindowBehaviors.Close;
            ModalWindow.OnClientClose = "refreshbtnclick";


        }

        protected void schAppointmentScheduler_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
        {
            if (e.Command.ToString() == "SwitchToWeekView")
            {
                hdnSchedulerView.Value = e.Command.ToString();
                FillAllAppointmentsForDate();
                if (hdnSchedulerView.Value != string.Empty)
                {
                    Session["hdnSchedulerView"] = hdnSchedulerView.Value;
                }

            }
            else if (e.Command.ToString() == "SwitchToMonthView")
            {
                hdnSchedulerView.Value = e.Command.ToString();
                FillAllAppointmentsForDate();
                if (hdnSchedulerView.Value != string.Empty)
                {
                    Session["hdnSchedulerView"] = hdnSchedulerView.Value;
                }
            }

            if (e.SelectedDate != DateTime.MinValue)
                Calendar1.SelectedDate = e.SelectedDate;
            updpnlScheduler.Update();
        }

        protected void btnPrintSuperBill_Click(object sender, EventArgs e)
        {

            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font boldFont = iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            //Srividhya added the Print SuperBill Code
            ulong ulEncounterID = Convert.ToUInt64(hdnEncounterID.Value);

            EncounterManager EncMngr = new EncounterManager();
            SuperBillDTO superbillDTO = new SuperBillDTO();

            superbillDTO = EncMngr.GetEncounterDetailsforPaperSuperBill(ulEncounterID);

            if (superbillDTO != null)
            {
                //hdnFileName.Value = "Documents\\" + Session.SessionID + "\\" + DateTime.Now.ToString("yyyyMMdd");
                //string sDirPath = Server.MapPath("Documents\\" + Session.SessionID + "\\" + DateTime.Now.ToString("yyyyMMdd"));

                //DirectoryInfo ObjSearchDir= new DirectoryInfo(sDirPath);

                //if (!ObjSearchDir.Exists)
                //{
                //    ObjSearchDir.Create();
                //}

                //string sPrintPathName = sDirPath + "\\" +superbillDTO.Appt_Prov_Name+".pdf";

                //string[] Split = new string[] { Server.MapPath("") };
                //string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                //if (hdnFileName.Value == string.Empty)
                //{
                //    hdnFileName.Value = FileName[0].ToString();
                //}

                string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                if (!ObjSearchDir.Exists)
                {
                    ObjSearchDir.Create();
                }
                string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

                string sPrintPathName = TargetFileDirectory + "\\" + superbillDTO.Appt_Prov_Name + ".pdf";

                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                if (hdnFileName.Value == string.Empty)
                {
                    hdnFileName.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                }
                else
                {
                    hdnFileName.Value += "|" + FileName[0].ToString();
                }

                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 30, 30, 30, 30);

                //doc.NewPage();

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                iTextSharp.text.Rectangle pageSize = doc.PageSize;
                HeaderEventGenerate headerEvent = new HeaderEventGenerate();
                doc.Open();
                wr.PageEvent = headerEvent;
                headerEvent.OnStartPage(wr, doc);
                headerEvent.OnEndPage(wr, doc);

                PdfPTable pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });//60,60,40

                //pdfTable.WidthPercentage = 100;

                PdfPCell pdfCell = new PdfPCell(new Phrase("Dr. JEERADI PRASAD", boldFont));

                pdfCell.Colspan = 6;
                pdfCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfTable.AddCell(pdfCell);
                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("PATIENT ACCOUNT#:", "Header");//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Human_ID.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("DOCTOR:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Appt_Prov_Name, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("APPT DATE:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Appt_Date, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("PATIENT NAME:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_Name, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("PRI. INS.:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Pri_Ins_Name, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("APPT TIME:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Appt_Time, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("PATIENT DOB:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_DOB, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("PRI. COPAY.:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Pri_Copay.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("APPT LOC/FAC:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Appt_Facility, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("SEX/AGE:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_Sex, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("PRI. MEM. ID:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Pri_Member_ID, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("APPT LEN:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.App_Length, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("STREET ADDRESS:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_Address, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("SEC. INS.:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Sec_Ins_Name, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("REASON:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Reason, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("CITY, STATE, ZIP:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_City + ", " + superbillDTO.Patient_State + ", " + superbillDTO.Patient_ZipCode, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("SEC. COPAY:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Sec_Copay.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("ACCT BAL:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Acc_Balance.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("HOME TELEPHONE#:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_HomePhoneNo, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("SEC. MEM. ID:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Sec_Member_ID, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("PATIENT DUE:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Patient_Due.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("SSN:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.SSN, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("ACCOUNT TYPE:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Account_Type, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("VOUCHER:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Voucher_No.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                pdfTable = new PdfPTable(new float[] { 25, 35, 20, 50, 20, 30 });
                pdfCell = CreateCell("REFERRING DR:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 2;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Referring_Prov_Name, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 2;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("LAST DX:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 2;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 2;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell("AUTH #:", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 2;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCell(superbillDTO.Auth_No, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 2;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);
                //doc.Add(new Paragraph("\n"));

                // add a image    
                //pdfTable = new PdfPTable(new float[] { 20, 20, 20 });
                pdfTable = new PdfPTable(new float[] { 90 });
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath("SuperBill_Template") + "\\" + superbillDTO.Appt_Prov_ID + ".jpg");
                //pdfCell = new PdfPCell(jpg);
                //jpg.BorderWidthLeft = 1;
                //jpg.BorderWidthRight = 1;
                //jpg.BorderWidthTop = 0;
                //jpg.BorderWidthBottom = 1;                                

                pdfTable.AddCell(jpg);

                doc.Add(pdfTable);

                doc.Close();
                //=================================                
            }

            ModalWindow.NavigateUrl = "frmPrintPDF.aspx?SI=" + hdnFileName.Value + "&Location=DYNAMIC";
            ModalWindow.Height = 780;
            ModalWindow.VisibleOnPageLoad = true;
            ModalWindow.Width = 930;
            ModalWindow.CenterIfModal = true;
            ModalWindow.VisibleTitlebar = true;
            ModalWindow.VisibleStatusbar = false;

            //srividhya
            ModalWindow.ReloadOnShow = true;
            ModalWindow.ShowContentDuringLoad = false;
            ModalWindow.Behaviors = WindowBehaviors.Close | WindowBehaviors.Maximize;

            ModalWindow.OnClientClose = "PrintSuperBillClick";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);
        }

        protected void btnPrintRecipt_Click(object sender, EventArgs e)
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

            sOutput = print.PrintReceipt(Convert.ToUInt64(hdnEncounterID.Value), Convert.ToUInt64(hdnHumanID.Value), TargetFileDirectory, true);

            if (sOutput == "No Receipt")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "DisplayErrorMessage('110085');", true);
                return;
            }

            string sPrintPathName = sOutput.Split('|')[0];
            string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            if (hdnFileName.Value == string.Empty)
            {
                hdnFileName.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
            }
            string FaxSubject = sOutput.Split('|')[1];

            ModalWindow.Visible = true;
            string sIframeHeight = "";
            if (hdnLaptopView.Value == "false")
            {
                ModalWindow.Height = 715;
                sIframeHeight = "600px";
            }
            else
            {
                ModalWindow.Height = 600;
                sIframeHeight = "490px";
            }

            ModalWindow.NavigateUrl = "frmPrintPDF.aspx?SI=" + hdnFileName.Value + "&Location=DYNAMIC&ButtonName=Print Receipt&PDFLOADHeight=" + sIframeHeight + "&FaxSubject=" + FaxSubject;

            ModalWindow.VisibleOnPageLoad = true;
            ModalWindow.Width = 930;
            ModalWindow.CenterIfModal = true;
            ModalWindow.VisibleTitlebar = true;
            ModalWindow.VisibleStatusbar = false;
            //srividhya
            ModalWindow.ReloadOnShow = true;
            ModalWindow.ShowContentDuringLoad = false;
            ModalWindow.Behaviors = WindowBehaviors.Close | WindowBehaviors.Maximize;

            ModalWindow.OnClientClose = "PrintReciptClick";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);
        }
        #endregion

        #region Methods
        public void FillAllAppointmentsForDate()
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            //CAP-790 - Object reference not set to an instance of an object.
            IList<FacilityLibrary> ilstFacAncillary = new List<FacilityLibrary>();
            if (ApplicationObject.facilityLibraryList != null)
            {
                //CAP-2025 -  In the provider login, the block days are not shown. In other login, the block days are shown. [ZDT# 194169]
                if (pnlProvidersHeader.InnerText == "Providers" || pnlProvidersHeader.InnerText == "Machine - Technician")
                {
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacilityName.SelectedItem.Text select f;
                    ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                }
                else
                {
                    var providers = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).Select(li => li.Text).ToList<string>();
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where providers.Contains(f.Fac_Name) select f;
                    ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                }
            }

            #region New Code
            //CAP-1857 - Block days is not shown in the appointment scheduler for the technicians in provider's login.
            hdnSchedulerView.Value = "SwitchToWeekView";
            if (schAppointmentScheduler.SelectedView.ToString().Trim() == "WeekView")
            {
                hdnSchedulerView.Value = "SwitchToWeekView";
            }
            else if (schAppointmentScheduler.SelectedView.ToString().Trim() == "MonthView")
            {
                hdnSchedulerView.Value = "SwitchToMonthView";
            }
            //CAP-2430
            else
            {
                hdnSchedulerView.Value = "SwitchToDayView";
            }
            #region Physician Login
            if (hdnSourceScreen.Value == "AppointmentFacility")
            {
                if (chklstProviders.SelectedItem == null || Calendar1.SelectedDate.ToString("dd-MMM-yyyy") == "01-Jan-1980" || Calendar1.SelectedDate == DateTime.MinValue)
                    return;
                #region Variables Declarations

                IList<ProcessMaster> tempProc = new List<ProcessMaster>();
                IList<ProcessMaster> TempProcList;
                IList<string> facilityList = new List<string>();
                IList<ulong> PhyIDList = new List<ulong>();
                FillAppointment LoadApptList;
                Telerik.Web.UI.Appointment appt;

                #endregion

                #region DB Call

                if (System.Text.RegularExpressions.Regex.IsMatch(hdnApptPhyId.Value, "^[0-9]*$") == true)
                {
                    //CAP-3493
                    PhyIDList.Add(Convert.ToUInt64(string.IsNullOrEmpty(hdnApptPhyId.Value) ? "0" : hdnApptPhyId.Value));
                }
                facilityList = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).Select(li => li.Text).ToList<string>();
                string time_taken = "";
                Stopwatch LoadAppointmentsDBCall = new Stopwatch();
                LoadAppointmentsDBCall.Start();
                DateTime StartDateTime = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, Calendar1.SelectedDate.Day, 0, 0, 0);
                StartDateTime = UtilityManager.ConvertToUniversal(StartDateTime);
                DateTime EndDateTime = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, Calendar1.SelectedDate.Day, 23, 59, 59);
                EndDateTime = UtilityManager.ConvertToUniversal(EndDateTime);

                //For Bug ID 61776
                //if (cboFacilityName.SelectedItem != null && sAncillary.Trim() != cboFacilityName.SelectedItem.Text.Trim())
                if (cboFacilityName.SelectedItem != null && ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                {
                    Is_Ancillary = false;
                    LoadApptList = EncMngr.GetAppointmentsByDatePhysicianFacility(StartDateTime, EndDateTime, PhyIDList.ToArray<ulong>(), facilityList.ToArray<string>(), hdnSchedulerView.Value, out time_taken, false, Is_Ancillary);
                }
                else
                {
                    //CAP-1857 - Block days is not shown in the appointment scheduler for the technicians in provider's login.
                    Is_Ancillary = true;
                    LoadApptList = EncMngr.GetAppointmentsByDatePhysicianFacility(StartDateTime, EndDateTime, PhyIDList.ToArray<ulong>(), facilityList.ToArray<string>(), hdnSchedulerView.Value, out time_taken, false, Is_Ancillary);
                }
                LoadAppointmentsDBCall.Stop();
                time_taken += "Overall Time Taken :" + LoadAppointmentsDBCall.Elapsed.Seconds + "." + LoadAppointmentsDBCall.Elapsed.Milliseconds + "s.";
                #endregion

                #region Fill Scheduler with Appointments
                schAppointmentScheduler.Appointments.Clear();

                TempProcList = ApplicationObject.processMasterList;
                if (TempProcList.Count > 0)
                {
                    var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
                    proclist = ColoredProcess.ToList<ProcessMaster>();

                    var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
                    ColorList = DistinctColor.ToList<string>();
                }

                for (int i = 0; i < LoadApptList.EncounterId.Count; i++)
                {
                    try
                    {
                        TimeSpan ts = new TimeSpan(0, LoadApptList.Duration_Minutes[i], 0);

                        //string EvStatus = "";
                        string IsACOEligible = string.Empty;

                        //Gitlab #3552
                        //if (ClientSession.UserPermissionDTO.Scntab != null)
                        //{
                        //    var scn_id = (from p in ClientSession.UserPermissionDTO.Scntab where p.SCN_Name == "frmPerformEV" select p).ToList();
                        //    if (scn_id.Count() > 0)
                        //    {
                        //        var SendEV = from p in ClientSession.UserPermissionDTO.Screens where p.SCN_ID == Convert.ToInt32(scn_id[0].SCN_ID) && p.Permission == "U" select p;
                        //        if (SendEV.Count() > 0)
                        //        {
                        //            if (LoadApptList.Perform_EV_Status != null && LoadApptList.Perform_EV_Status.Count() > 0)
                        //            {
                        //                if (LoadApptList.Perform_EV_Status[i] != null && LoadApptList.Perform_EV_Status[i] != string.Empty)
                        //                {
                        //                    EvStatus = "  - " + LoadApptList.Perform_EV_Status[i].ToString();
                        //                }


                        //                else
                        //                {
                        //                    EvStatus = "  - EV-NOT PERFORMED";
                        //                }
                        //            }
                        //            else if ((LoadApptList.EVMode != null && LoadApptList.EVMode.Count() > 0 && LoadApptList.EVMode[i] != null && LoadApptList.EVMode[i] != string.Empty && LoadApptList.EVMode[i].ToString().ToUpper().Contains("MANUAL")))
                        //            {
                        //                EvStatus = " - EV-PERFORMED MANUALLY";
                        //            }

                        //            else
                        //            {
                        //                EvStatus = "  - EV-NOT PERFORMED";
                        //            }
                        //        }
                        //        else
                        //        {
                        //            EvStatus = "  - EV-NOT PERFORMED";
                        //        }
                        //    }
                        //}

                        if (LoadApptList.Is_ACO_Eligible[i] != string.Empty && LoadApptList.Is_ACO_Eligible[i] != "N")
                            IsACOEligible = "  - " + LoadApptList.Is_ACO_Eligible[i];

                        appt = new Telerik.Web.UI.Appointment(LoadApptList.Human_ID[i].ToString() + "-", UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]), UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)), " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString() + IsACOEligible);//EvStatus + IsACOEligible);
                        //CAP-3272
                        string is_Auth_Verified = "";
                        if (LoadApptList.Is_Auth_Verified != null)
                        {
                            is_Auth_Verified = LoadApptList.Is_Auth_Verified[i].ToString();
                        }
                        appt.ToolTip = LoadApptList.Human_ID[i].ToString() + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.TypeofVisit[i].ToString() + " - " + (is_Auth_Verified == "Y" ? "Auth is Verified" : "Auth is Not Verified");
                        appt.Subject = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]).ToString("hh:mm:ss tt") + "-" + UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)).ToString("hh:mm:ss tt") + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString() + IsACOEligible; //EvStatus + IsACOEligible;
                        if (LoadApptList.Preferred_Language[i].ToString() != null && LoadApptList.Preferred_Language[i].ToString() != string.Empty)
                        {
                            appt.Subject += "- " + LoadApptList.Preferred_Language[i].ToString();
                        }
                        appt.ID = LoadApptList.EncounterId[i] + "-" + LoadApptList.Payment_Paid[i] + "-" + LoadApptList.E_Super_Bill[i];// + "-" + LoadApptList.Document_Type[i];
                        appt.Start = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]);
                        appt.End = appt.Start.AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds);
                        //appt.End = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
                        appt.Description = LoadApptList.ApptStatus[i];
                        appt.Font.Bold = true;
                        appt.Font.Size = 9;
                        IList<string> matchingFacility = facilityList.Where(item => item == LoadApptList.FacilityName[i]).ToList<string>();

                        if (matchingFacility != null && matchingFacility.Count > 0)
                        {
                            appt.Resources.Add(new Resource("Facility", matchingFacility[0], matchingFacility[0]));
                        }
                        appt.Attributes.Add("Is_General_Queue_Appoinment", (LoadApptList.Is_General_Queue_Appoinment.Count > 0) ? LoadApptList.Is_General_Queue_Appoinment[i] : "".ToString());
                        if (proclist.Count > 0)
                        {
                            var Choose = from c in proclist where c.Process_Name == appt.Description select c;
                            if (Choose != null && Choose.Count() > 0)
                                tempProc = Choose.ToList<ProcessMaster>();
                            else
                                tempProc = null;
                        }
                        if (tempProc != null && tempProc.Count > 0)
                        {
                            IList<string> colrList = ColorList.Where(item => item.ToUpper() == tempProc[0].Process_Color.ToUpper()).ToList<string>();
                            if (colrList.Count > 0)//&& LoadApptList.Document_Type[i] != "Y")
                            {
                                appt.BackColor = Color.FromName(tempProc[0].Process_Color);
                                appt.BorderColor = Color.Black;
                                schAppointmentScheduler.InsertAppointment(appt);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                }
                #endregion

                #region Fill Scheduler with Blocked Appointment Time Slots

                IList<Blockdays> blockList;
                blockList = LoadApptList.blockList;

                for (int i = 0; i < blockList.Count; i++)
                {
                    try
                    {
                        DateTime from = Convert.ToDateTime(blockList[i].From_Time);
                        DateTime to = Convert.ToDateTime(blockList[i].To_Time);

                        int i1 = (to - from).Hours * 60;
                        int i2 = (to - from).Minutes;
                        TimeSpan ts = new TimeSpan(0, i1 + i2, 0);

                        DateTime dt = blockList[i].Block_Date.AddHours(Convert.ToDateTime(blockList[i].From_Time).Hour).AddMinutes(Convert.ToDateTime(blockList[i].From_Time).Minute);

                        appt = new Telerik.Web.UI.Appointment();

                        //if (blockList[i].Block_Type != "RECURSIVE" && blockList[i].Block_Type != "NON RECURSIVE")
                        //{
                        //    TypeofVistList.Add(blockList[i].Block_Type.ToString() + "|" + blockList[i].From_Time.ToString() + "|" + blockList[i].To_Time.ToString() + "|" + blockList[i].Block_Date.ToString() + "|" + blockList[i].Facility_Name);
                        //    continue;
                        //}

                        string sStart = Convert.ToDateTime(blockList[i].Block_Date).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].From_Time).ToShortTimeString();
                        appt.Start = Convert.ToDateTime(sStart);
                        string sEnd = Convert.ToDateTime(blockList[i].Block_Date).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].To_Time).ToShortTimeString();
                        appt.End = Convert.ToDateTime(sEnd);
                        appt.Subject = Convert.ToDateTime(sStart).ToString("hh:mm:ss tt") + "-" + Convert.ToDateTime(sEnd).ToString("hh:mm:ss tt") + "-" + blockList[i].Reason;
                        string ProviderName = string.Empty;
                        appt.Resources.Add(new Resource("Facility", blockList[i].Facility_Name, blockList[i].Facility_Name));
                        appt.Description = string.Empty;
                        //To assign the Color
                        if (System.Configuration.ConfigurationManager.AppSettings["AppointmentBlockColor"] == null)
                        {
                            throw new Exception("Please specify Appointment Back Color in the Config File");
                        }

                        Color tmpForeColor = Color.FromName(System.Configuration.ConfigurationManager.AppSettings["AppointmentBlockColor"]);

                        appt.BackColor = tmpForeColor;
                        appt.BorderColor = Color.Black;
                        appt.Font.Bold = true;
                        schAppointmentScheduler.InsertAppointment(appt);

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                #endregion
                RefreshMyResources();
                //Session["TypeofVisit"] = TypeofVistList;
            }
            #endregion
            #region FO or MA Login
            else
            {
                if (chklstProviders.SelectedItem == null || Calendar1.SelectedDate.ToString("dd-MMM-yyyy") == "01-Jan-1980" || Calendar1.SelectedDate == DateTime.MinValue)
                    return;

                #region Variables Declarations

                IList<ProcessMaster> tempProc = new List<ProcessMaster>();
                IList<ulong> PhyIDList = new List<ulong>();
                FillAppointment LoadApptList;
                IList<string> facilityList = new List<string>();
                Telerik.Web.UI.Appointment appt;

                #endregion

                #region DBCall

                hdnPhyIDColor.Value = string.Empty;
                for (int k = 0; k < chklstProviders.Items.Count; k++)
                {
                    if (chklstProviders.Items[k].Selected == true && System.Text.RegularExpressions.Regex.IsMatch(chklstProviders.Items[k].Value, "^[0-9]*$") == true)
                    {
                        PhyIDList.Add(Convert.ToUInt64(chklstProviders.Items[k].Value));
                        hdnApptPhyId.Value = chklstProviders.Items[k].Value;
                        hdnPhyIDColor.Value = chklstProviders.Items[k].Value;
                    }
                }

                facilityList.Add(cboFacilityName.SelectedItem.Text);

                if (Calendar1.SelectedDate == DateTime.MinValue)
                {
                    Calendar1.SelectedDate = UtilityManager.ConvertToLocal(DateTime.UtcNow);
                }
                string time_taken = "";
                Stopwatch LoadAppointmentsDBCall = new Stopwatch();
                LoadAppointmentsDBCall.Start();
                DateTime StartDateTime = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, Calendar1.SelectedDate.Day, 0, 0, 0);
                StartDateTime = UtilityManager.ConvertToUniversal(StartDateTime);
                DateTime EndDateTime = new DateTime(Calendar1.SelectedDate.Year, Calendar1.SelectedDate.Month, Calendar1.SelectedDate.Day, 23, 59, 59);
                EndDateTime = UtilityManager.ConvertToUniversal(EndDateTime);

                // if (sAncillary.Trim() != cboFacilityName.SelectedItem.Text.Trim())
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                {
                    Is_Ancillary = false;
                    LoadApptList = EncMngr.GetAppointmentsByDatePhysicianFacility(StartDateTime, EndDateTime, PhyIDList.ToArray<ulong>(), facilityList.ToArray<string>(), hdnSchedulerView.Value, out time_taken, true, Is_Ancillary);
                }
                else
                {
                    Is_Ancillary = true;
                    LoadApptList = EncMngr.GetAppointmentsByDatePhysicianFacility(StartDateTime, EndDateTime, PhyIDList.ToArray<ulong>(), facilityList.ToArray<string>(), hdnSchedulerView.Value, out time_taken, true, Is_Ancillary);
                }
                LoadAppointmentsDBCall.Stop();
                time_taken += "Overall Time Taken :" + LoadAppointmentsDBCall.Elapsed.Seconds + "." + LoadAppointmentsDBCall.Elapsed.Milliseconds + "s.";
                #endregion

                #region Fill Scheduler with Appointments
                schAppointmentScheduler.Appointments.Clear();
                for (int i = 0; i < LoadApptList.EncounterId.Count; i++)
                {
                    try
                    {
                        //string EvStatus = "";
                        string IsACOEligible = string.Empty;

                        if (ClientSession.UserPermissionDTO.Scntab != null)
                        {
                            var scn_id = (from p in ClientSession.UserPermissionDTO.Scntab where p.SCN_Name == "frmPerformEV" select p).ToList();
                            if (scn_id.Count() > 0)
                            {
                                var SendEV = from p in ClientSession.UserPermissionDTO.Screens where p.SCN_ID == Convert.ToInt32(scn_id[0].SCN_ID) && p.Permission == "U" select p;
                                //if (SendEV.Count() > 0)
                                //{
                                //    if (LoadApptList.Perform_EV_Status != null && LoadApptList.Perform_EV_Status.Count() > 0)
                                //    {
                                //        if (LoadApptList.Perform_EV_Status[i] != null && LoadApptList.Perform_EV_Status[i] != string.Empty && LoadApptList.Perform_EV_Status[i].Trim().ToUpper().Contains("EV") == true)
                                //        {
                                //            EvStatus = "  - EV Status - " + LoadApptList.Perform_EV_Status[i].ToString();
                                //        }
                                //        else if (LoadApptList.Perform_EV_Status[i] != null && LoadApptList.Perform_EV_Status[i] != string.Empty && LoadApptList.Perform_EV_Status[i].Trim().ToUpper().Contains("INVALID") == true)
                                //        {
                                //            EvStatus = "  - EV Status - " + LoadApptList.Perform_EV_Status[i].ToString();
                                //        }
                                //        else if (LoadApptList.Perform_EV_Status[i] != null && LoadApptList.Perform_EV_Status[i] != string.Empty)
                                //        {
                                //            EvStatus = "  - EV Status - Inprogress";
                                //        }
                                //        else
                                //        {
                                //            EvStatus = "  - EV Status - NOT PERFORMED";
                                //        }
                                //    }
                                //    else
                                //    {
                                //        EvStatus = "  - EV Status - NOT PERFORMED";
                                //    }
                                //}

                                //Gitlab #3552
                                //if (SendEV.Count() > 0)
                                //{
                                //    if (LoadApptList.Perform_EV_Status != null && LoadApptList.Perform_EV_Status.Count() > 0)
                                //    {
                                //        if (LoadApptList.Perform_EV_Status[i] != null && LoadApptList.Perform_EV_Status[i] != string.Empty)
                                //        {
                                //            EvStatus = "  - " + LoadApptList.Perform_EV_Status[i].ToString();
                                //        }

                                //        else if ((LoadApptList.EVMode != null && LoadApptList.EVMode.Count() > 0 && LoadApptList.EVMode[i] != null && LoadApptList.EVMode[i] != string.Empty && LoadApptList.EVMode[i].ToString().ToUpper().Contains("MANUAL")))
                                //        {
                                //            EvStatus = " - EV-PERFORMED MANUALLY";
                                //        }
                                //        else
                                //        {
                                //            EvStatus = "  - EV-NOT PERFORMED";
                                //        }
                                //    }
                                //    else
                                //    {
                                //        EvStatus = "  - EV-NOT PERFORMED";
                                //    }
                                //}
                                //else
                                //{
                                //    EvStatus = "  - EV-NOT PERFORMED";
                                //}
                            }
                        }

                        if (LoadApptList.Is_ACO_Eligible.Count < i && LoadApptList.Is_ACO_Eligible[i] != string.Empty && LoadApptList.Is_ACO_Eligible[i] != "N")
                            IsACOEligible = "  - " + LoadApptList.Is_ACO_Eligible[i];

                        TimeSpan ts = new TimeSpan(0, LoadApptList.Duration_Minutes[i], 0);
                        appt = new Telerik.Web.UI.Appointment(LoadApptList.Human_ID[i].ToString() + "-", UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]), UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)), " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i] + IsACOEligible);// EvStatus + IsACOEligible);
                        //CAP-3272
                        string is_Auth_Verified = "";
                        if(LoadApptList.Is_Auth_Verified != null)
                        {
                            is_Auth_Verified = LoadApptList.Is_Auth_Verified[i].ToString();
                        }
                        if (LoadApptList.Is_Medicare_Plan[i].ToString() == "Y")
                        {
                            appt.ToolTip = LoadApptList.Human_ID[i].ToString() + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.TypeofVisit[i].ToString() + "(MEDICARE)" + " - " + (is_Auth_Verified == "Y" ? "Auth is Verified ; " : "Auth is Not Verified ; ") + "\n" + LoadApptList.Outstanding_Orders[i].ToString();
                        }
                        else
                        {
                            appt.ToolTip = LoadApptList.Human_ID[i].ToString() + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.TypeofVisit[i].ToString() + " - " + (is_Auth_Verified == "Y" ? "Auth is Verified ; " : "Auth is Not Verified ; ") + "\n" + LoadApptList.Outstanding_Orders[i].ToString();
                        }
                        appt.Subject = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]).ToString("hh:mm:ss tt") + "-" + UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)).ToString("hh:mm:ss tt") + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString() + "\n" + LoadApptList.Outstanding_Orders[i].ToString() + IsACOEligible;// EvStatus + IsACOEligible;
                        if (LoadApptList.Preferred_Language[i].ToString() != null && LoadApptList.Preferred_Language[i].ToString() != string.Empty)
                        {
                            appt.Subject += "- " + LoadApptList.Preferred_Language[i].ToString();
                        }
                        appt.ID = LoadApptList.EncounterId[i] + "-" + LoadApptList.Payment_Paid[i] + "-" + LoadApptList.E_Super_Bill[i] + /*"-" + LoadApptList.Document_Type[i] +*/ "-" + LoadApptList.Birth_Date[i].ToString() + "-" + LoadApptList.Human_Type[i];
                        if (ClientSession.UserRole.Trim().ToUpper() == "OFFICE MANAGER")
                            appt.ID += "-" + LoadApptList.Is_Batch_Created[i];
                        appt.Start = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]);
                        appt.End = appt.Start.AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds);

                        //appt.End = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
                        appt.Description = LoadApptList.ApptStatus[i];
                        appt.Font.Bold = true;
                        appt.Font.Size = 9;
                        //appt.Resources.Add(new Resource("Physician", "4", "Quinton -  Pererras Ernie"));
                        appt.Resources.Add(new Resource("Physician", LoadApptList.Appointment_Provider_ID[i].ToString(), LoadApptList.PhysicianName[i]));
                        appt.Attributes.Add("Is_General_Queue_Appoinment", (LoadApptList.Is_General_Queue_Appoinment.Count > 0 )? LoadApptList.Is_General_Queue_Appoinment[i] : "".ToString());
                        IList<ProcessMaster> TempProcList;
                        TempProcList = ApplicationObject.processMasterList;
                        if (TempProcList.Count > 0)
                        {
                            var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
                            proclist = ColoredProcess.ToList<ProcessMaster>();
                            var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
                            ColorList = DistinctColor.ToList<string>();
                        }
                        if (proclist.Count > 0)
                        {
                            var Choose = from c in proclist where c.Process_Name == appt.Description select c;//(IList<ProcessMaster>)Session["proclist"] 
                            if (Choose != null && Choose.Count() > 0)
                                tempProc = Choose.ToList<ProcessMaster>();
                            else
                                tempProc = null;
                        }

                        if (tempProc != null && tempProc.Count > 0)
                        {
                            IList<string> colrList = ColorList.Where(item => item.ToUpper() == tempProc[0].Process_Color.ToUpper()).ToList<string>();
                            if (colrList.Count > 0)//&& LoadApptList.Document_Type[i] != "Y")
                            {
                                appt.BackColor = Color.FromName(tempProc[0].Process_Color);
                                appt.BorderColor = Color.Black;
                                schAppointmentScheduler.InsertAppointment(appt);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                }
                #endregion

                #region Fill Scheduler with Blocked Appointment Time Slots

                IList<Blockdays> blockList;
                blockList = LoadApptList.blockList;
                for (int i = 0; i < blockList.Count; i++)
                {
                    try
                    {
                        DateTime from = Convert.ToDateTime(blockList[i].From_Time);
                        DateTime to = Convert.ToDateTime(blockList[i].To_Time);

                        int i1 = (to - from).Hours * 60;
                        int i2 = (to - from).Minutes;
                        TimeSpan ts = new TimeSpan(0, i1 + i2, 0);

                        DateTime dt = blockList[i].Block_Date.AddHours(Convert.ToDateTime(blockList[i].From_Time).Hour).AddMinutes(Convert.ToDateTime(blockList[i].From_Time).Minute);

                        appt = new Telerik.Web.UI.Appointment();
                        //if (blockList[i].Block_Type != "RECURSIVE" && blockList[i].Block_Type != "NON RECURSIVE")
                        //{
                        //    TypeofVistList.Add(blockList[i].Block_Type.ToString() + "|" + blockList[i].From_Time.ToString() + "|" + blockList[i].To_Time.ToString() + "|" + blockList[i].Block_Date.ToString() + "|" + blockList[i].Physician_ID);
                        //    continue;
                        //}
                        string sStart = Convert.ToDateTime(blockList[i].Block_Date).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].From_Time).ToShortTimeString();
                        appt.Start = Convert.ToDateTime(sStart);
                        string sEnd = Convert.ToDateTime(blockList[i].Block_Date).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].To_Time).ToShortTimeString();
                        appt.End = Convert.ToDateTime(sEnd);
                        appt.Subject = Convert.ToDateTime(sStart).ToString("hh:mm:ss tt") + "-" + Convert.ToDateTime(sEnd).ToString("hh:mm:ss tt") + "-" + blockList[i].Reason;
                        string ProviderName = string.Empty;
                        for (int j = 0; j < chklstProviders.Items.Count; j++)
                        {
                            if (chklstProviders.Items[j].Selected == true)
                            {
                                //  if (sAncillary.Trim() != cboFacilityName.SelectedItem.Text.Trim())
                                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                                {
                                    if (chklstProviders.Items[j].Value == blockList[i].Physician_ID.ToString())
                                    {
                                        ProviderName = chklstProviders.Items[j].Text;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (chklstProviders.Items[j].Value == blockList[i].Machine_Technician_Library_ID.ToString())
                                    {
                                        ProviderName = chklstProviders.Items[j].Text;
                                        break;
                                    }
                                }
                            }
                        }
                        // if (sAncillary.Trim() != cboFacilityName.SelectedItem.Text.Trim())
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                        {
                            appt.Resources.Add(new Resource("Physician", blockList[i].Physician_ID.ToString(), ProviderName));
                        }
                        else
                        {
                            appt.Resources.Add(new Resource("Physician", blockList[i].Machine_Technician_Library_ID.ToString(), ProviderName));
                        }
                        appt.Description = string.Empty;
                        //To assign the Color
                        if (System.Configuration.ConfigurationManager.AppSettings["AppointmentBlockColor"] == null)
                        {
                            throw new Exception("Plese specify Appointment Back Color in the Config File");
                        }
                        IList<ProcessMaster> TempProcList;
                        TempProcList = ApplicationObject.processMasterList;
                        if (TempProcList.Count > 0)
                        {
                            var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
                            proclist = ColoredProcess.ToList<ProcessMaster>();
                            var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
                            ColorList = DistinctColor.ToList<string>();
                        }

                        Color tmpForeColor = Color.FromName(System.Configuration.ConfigurationManager.AppSettings["AppointmentBlockColor"]);
                        IList<string> colrList = ColorList;
                        appt.BackColor = tmpForeColor;
                        appt.BorderColor = Color.Black;
                        appt.Font.Bold = true;
                        schAppointmentScheduler.InsertAppointment(appt);
                    }
                    catch
                    {

                    }
                }

                #endregion
                RefreshMyResources();
                hdnApptFacName.Value = cboFacilityName.SelectedItem.Text;
                //Session["TypeofVisit"] = TypeofVistList;
            }
            #endregion
            #endregion
            #region Old Code
            //IList<string> TypeofVistList = new List<string>();
            ////StaticLookupManager staticLookupMngr = new StaticLookupManager();
            ////IList<StaticLookup> objStaticLookup = new List<StaticLookup>();
            ////objStaticLookup = staticLookupMngr.getStaticLookupByFieldName("TYPE OF VISIT", "Sort_Order");
            //if (schAppointmentScheduler.SelectedView.ToString().Trim() == "WeekView")
            //{
            //    hdnSchedulerView.Value = "SwitchToWeekView";
            //}
            //else if (schAppointmentScheduler.SelectedView.ToString().Trim() == "MonthView")
            //{
            //    hdnSchedulerView.Value = "SwitchToMonthView";
            //}
            //if (hdnSourceScreen.Value == "AppointmentFacility")
            //{
            //    IList<ProcessMaster> tempProc = new List<ProcessMaster>();
            //    IList<string> facilityList = new List<string>();
            //    if (chklstProviders.SelectedItem == null)
            //    {
            //        return;
            //    }

            //    schAppointmentScheduler.Appointments.Clear();
            //    IList<ulong> PhyIDList = new List<ulong>();
            //    for (int k = 0; k < chklstProviders.Items.Count; k++)
            //    {
            //        try
            //        {
            //            if (chklstProviders.Items[k].Selected == true)
            //            {
            //                PhyIDList.Add(Convert.ToUInt64(chklstProviders.Items[k].Value));
            //            }
            //        }
            //        catch
            //        {
            //            PhyIDList.Add(Convert.ToUInt64(hdnApptPhyId.Value));
            //        }
            //    }

            //    if (Calendar1.SelectedDate.ToString("dd-MMM-yyyy") == "01-Jan-1980")
            //    {
            //        return;
            //    }
            //    FillAppointment LoadApptList;
            //    IList<MapFacilityPhysician> facList = new List<MapFacilityPhysician>();
            //    facilityList = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).Select(li => li.Text).ToList<string>();
            //    LoadApptList = EncMngr.GetAppointmentByDateandPhyRCMFacility(Calendar1.SelectedDate, PhyIDList.ToArray<ulong>(), facilityList.ToArray<string>(), hdnSchedulerView.Value);
            //    //LoadApptList = EncMngr.GetAppointmentByDateandPhyRCMFacility(Calendar1.SelectedDate, PhyIDList[0], facilityList.ToArray<string>(), hdnSchedulerView.Value);//Commanded By Manimaran for Doesnot reflect provider Block Days

            //    Telerik.Web.UI.Appointment appt;
            //    for (int i = 0; i < LoadApptList.EncounterId.Count; i++)
            //    {

            //        try
            //        {
            //            TimeSpan ts = new TimeSpan(0, LoadApptList.Duration_Minutes[i], 0);

            //            //srividhya
            //            //appt = new Telerik.Web.UI.Appointment(LoadApptList.Human_ID[i].ToString() + "-", ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i]), ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)), " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i]);
            //            appt = new Telerik.Web.UI.Appointment(LoadApptList.Human_ID[i].ToString() + "-", UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]), UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)), " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i]);
            //            appt.ToolTip = LoadApptList.Human_ID[i].ToString() + " - " + LoadApptList.PatientName[i].ToString();
            //            //srividhya
            //            //appt.Subject = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i]).ToString("hh:mm:ss tt") + "-" + ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)).ToString("hh:mm:ss tt") + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString();
            //            appt.Subject = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]).ToString("hh:mm:ss tt") + "-" + UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)).ToString("hh:mm:ss tt") + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString();
            //            appt.ID = LoadApptList.EncounterId[i] + "-" + LoadApptList.Payment_Paid[i] + "-" + LoadApptList.E_Super_Bill[i] + "-" + LoadApptList.Document_Type[i];
            //            //srividhya
            //            //appt.Start = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i]);
            //            appt.Start = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]);
            //            //srividhya
            //            //appt.End = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
            //            appt.End = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
            //            appt.Description = LoadApptList.ApptStatus[i];
            //            appt.Font.Bold = true;
            //            appt.Font.Size = 9;
            //            for (int j = 0; j < facilityList.Count; j++)
            //            {
            //                if (LoadApptList.FacilityName[i] == facilityList[j])
            //                {
            //                    appt.Resources.Add(new Resource("Facility", facList[j].Facility_Name, facList[j].Facility_Name));
            //                }
            //            }
            //            //appt.Resources.Add(new Resource("Physician", LoadApptList.Appointment_Provider_ID[i].ToString(), LoadApptList.PhysicianName[i]));

            //            IList<ProcessMaster> TempProcList;
            //            TempProcList = ApplicationObject.processMasterList;
            //            if (TempProcList.Count > 0)
            //            {
            //                var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
            //                proclist = ColoredProcess.ToList<ProcessMaster>();
            //            }
            //            if (TempProcList.Count > 0)
            //            {
            //                var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
            //                ColorList = DistinctColor.ToList<string>();
            //            }
            //            if (proclist.Count > 0)
            //            {
            //                var Choose = from c in proclist where c.Process_Name == appt.Description select c;
            //                tempProc = Choose.ToList<ProcessMaster>();
            //            }

            //            if (tempProc[0].Process_Color != string.Empty)
            //            {
            //                Color tmpForeColor = Color.FromName(tempProc[0].Process_Color);

            //            }

            //            IList<string> colrList = ColorList;
            //            if (colrList.Count > 0)
            //            {
            //                for (int q = 0; q < colrList.Count; q++)
            //                {
            //                    if (tempProc[0].Process_Color == colrList[q].ToString())
            //                    {
            //                        // appt.CssClass = "rsWrap";
            //                        if (LoadApptList.Document_Type[i] != "DOCUMENTATION")
            //                        {
            //                            appt.BackColor = Color.FromName(tempProc[0].Process_Color);
            //                            appt.BorderColor = Color.Black;
            //                            schAppointmentScheduler.InsertAppointment(appt);
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        catch
            //        {

            //        }
            //    }
            //    #region BlockDaysAppointment

            //    IList<Blockdays> blockList;
            //    blockList = LoadApptList.blockList;

            //    for (int i = 0; i < blockList.Count; i++)
            //    {
            //        try
            //        {
            //            DateTime from = Convert.ToDateTime(blockList[i].From_Time);
            //            DateTime to = Convert.ToDateTime(blockList[i].To_Time);

            //            int i1 = (to - from).Hours * 60;
            //            int i2 = (to - from).Minutes;
            //            TimeSpan ts = new TimeSpan(0, i1 + i2, 0);

            //            DateTime dt = blockList[i].Block_Date.AddHours(Convert.ToDateTime(blockList[i].From_Time).Hour).AddMinutes(Convert.ToDateTime(blockList[i].From_Time).Minute);

            //            appt = new Telerik.Web.UI.Appointment();

            //            //if (Calendar1.SelectedDate <= blockList[i].To_Date_Choosen && Calendar1.SelectedDate >= blockList[i].From_Date_Choosen && Calendar1.SelectedDate.ToString("dddd") == blockList[i].Day_Choosen)
            //            //{

            //            if (blockList[i].Block_Type != "RECURSIVE" && blockList[i].Block_Type != "NON RECURSIVE")
            //            {

            //                TypeofVistList.Add(blockList[i].Block_Type.ToString() + "|" + blockList[i].From_Time.ToString() + "|" + blockList[i].To_Time.ToString() + "|" + blockList[i].Block_Date.ToString() + "|" + blockList[i].Facility_Name);
            //                continue;
            //            }

            //            //appt.UniqueId = new EventId(0);
            //            // appt.ResourceId = new EventId(blockList[i].Physician_ID);
            //            string sStart = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].From_Time).ToShortTimeString();
            //            //string sStart = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].From_Time).ToShortTimeString();
            //            appt.Start = Convert.ToDateTime(sStart);
            //            string sEnd = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].To_Time).ToShortTimeString();
            //            // string sEnd = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].To_Time).ToShortTimeString();
            //            appt.End = Convert.ToDateTime(sEnd);
            //            appt.Subject = Convert.ToDateTime(sStart).ToString("hh:mm:ss tt") + "-" + Convert.ToDateTime(sEnd).ToString("hh:mm:ss tt") + "-" + blockList[i].Reason;
            //            string ProviderName = string.Empty;
            //            //for (int j = 0; j < chklstProviders.Items.Count; j++)
            //            //{
            //            //    if (chklstProviders.Items[j].Selected == true)
            //            //    {
            //            //        if (chklstProviders.Items[j].Value == blockList[i].Physician_ID.ToString())
            //            //        {
            //            //            ProviderName = chklstProviders.Items[j].Text;
            //            //            break;
            //            //        }
            //            //    }
            //            //}

            //            //appt.Resources.Add(new Resource("Facility", facList[0].Facility_Name, facList[0].Facility_Name));//commented by Nijanthan
            //            appt.Resources.Add(new Resource("Facility", blockList[i].Facility_Name, blockList[i].Facility_Name));
            //            //appt.Resources.Add(new Resource("Physician", blockList[i].Physician_ID.ToString(), ProviderName));
            //            appt.Description = string.Empty;
            //            //To assign the Color
            //            if (System.Configuration.ConfigurationSettings.AppSettings["AppointmentBlockColor"] == null)
            //            {
            //                throw new Exception("Plese specify Appointment Back Color in the Config File");
            //            }

            //            Color tmpForeColor = Color.FromName(System.Configuration.ConfigurationSettings.AppSettings["AppointmentBlockColor"]);

            //            IList<ProcessMaster> TempProcList;
            //            TempProcList = ApplicationObject.processMasterList;
            //            if (TempProcList.Count > 0)
            //            {
            //                var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
            //                proclist = ColoredProcess.ToList<ProcessMaster>();
            //                var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
            //                ColorList = DistinctColor.ToList<string>();
            //            }
            //            IList<string> colrList = ColorList;//(IList<string>)Session["ColorList"];
            //            //if (colrList.Count > 0)
            //            //{
            //            //    for (int q = 0; q < colrList.Count; q++)
            //            //    {
            //            //if (tempProc[0].Process_Color == colrList[q].ToString())
            //            //{
            //            appt.BackColor = tmpForeColor;
            //            appt.BorderColor = Color.Black;
            //            appt.Font.Bold = true;
            //            schAppointmentScheduler.InsertAppointment(appt);

            //            //    }
            //            //}
            //            //}

            //        }

            //        //}

            //        catch
            //        {

            //        }
            //    }
            //    #endregion
            //    RefreshMyResources();
            //    Session["TypeofVisit"] = TypeofVistList;

            //}
            //else
            //{
            //    IList<ProcessMaster> tempProc = new List<ProcessMaster>();

            //    if (chklstProviders.SelectedItem == null)
            //    {
            //        return;
            //    }

            //    schAppointmentScheduler.Appointments.Clear();
            //    IList<ulong> PhyIDList = new List<ulong>();
            //    hdnPhyIDColor.Value = string.Empty;
            //    for (int k = 0; k < chklstProviders.Items.Count; k++)
            //    {
            //        if (chklstProviders.Items[k].Selected == true)
            //        {
            //            PhyIDList.Add(Convert.ToUInt64(chklstProviders.Items[k].Value));
            //            //Added by bala for Appointment facility base screen
            //            hdnApptPhyId.Value = chklstProviders.Items[k].Value;
            //            hdnPhyIDColor.Value = chklstProviders.Items[k].Value;
            //        }
            //    }

            //    if (Calendar1.SelectedDate.ToString("dd-MMM-yyyy") == "01-Jan-1980")
            //    {
            //        return;
            //    }
            //    FillAppointment LoadApptList;
            //    IList<string> facilityList = new List<string>();
            //    facilityList.Add(cboFacilityName.SelectedValue);

            //    if (Calendar1.SelectedDate == DateTime.MinValue)
            //    {
            //        Calendar1.SelectedDate = UtilityManager.ConvertToLocal(DateTime.UtcNow);
            //    }

            //    LoadApptList = EncMngr.GetAppointmentByDateandPhyRCM(Calendar1.SelectedDate, PhyIDList.ToArray<ulong>(), facilityList.ToArray<string>(), hdnSchedulerView.Value);
            //    Telerik.Web.UI.Appointment appt;
            //    for (int i = 0; i < LoadApptList.EncounterId.Count; i++)
            //    {

            //        try
            //        {
            //            TimeSpan ts = new TimeSpan(0, LoadApptList.Duration_Minutes[i], 0);

            //            //srividhya
            //            //appt = new Telerik.Web.UI.Appointment(LoadApptList.Human_ID[i].ToString() + "-", ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i]), ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)), " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i]);
            //            appt = new Telerik.Web.UI.Appointment(LoadApptList.Human_ID[i].ToString() + "-", UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]), UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)), " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i]);
            //            if (LoadApptList.Is_Medicare_Plan[i].ToString() == "Y")
            //            {
            //                appt.ToolTip = LoadApptList.Human_ID[i].ToString() + " - " + LoadApptList.PatientName[i].ToString() + "(MEDICARE)";
            //            }
            //            else
            //            {
            //                appt.ToolTip = LoadApptList.Human_ID[i].ToString() + " - " + LoadApptList.PatientName[i].ToString();
            //            }
            //            //srividhya
            //            //appt.Subject = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i]).ToString("hh:mm:ss tt") + "-" + ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)).ToString("hh:mm:ss tt") + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString();
            //            appt.Subject = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]).ToString("hh:mm:ss tt") + "-" + UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds)).ToString("hh:mm:ss tt") + " - " + LoadApptList.PatientName[i].ToString() + " - " + LoadApptList.ApptStatus[i].ToString();
            //            appt.ID = LoadApptList.EncounterId[i] + "-" + LoadApptList.Payment_Paid[i] + "-" + LoadApptList.E_Super_Bill[i] + "-" + LoadApptList.Document_Type[i] + "-" + LoadApptList.Birth_Date[i].ToString() + "-" + LoadApptList.Human_Type[i];
            //            //srividhya
            //            //appt.Start = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i]);
            //            appt.Start = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i]);
            //            //srividhya
            //            //appt.End = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
            //            appt.End = UtilityManager.ConvertToLocal(LoadApptList.Appointment_Date[i].AddHours(ts.Hours).AddMinutes(ts.Minutes).AddSeconds(ts.Seconds));
            //            appt.Description = LoadApptList.ApptStatus[i];
            //            appt.Font.Bold = true;
            //            appt.Font.Size = 9;
            //            appt.Resources.Add(new Resource("Physician", LoadApptList.Appointment_Provider_ID[i].ToString(), LoadApptList.PhysicianName[i]));

            //            IList<ProcessMaster> TempProcList;
            //            TempProcList = ApplicationObject.processMasterList;
            //            if (TempProcList.Count > 0)
            //            {
            //                var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
            //                proclist = ColoredProcess.ToList<ProcessMaster>();
            //                var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
            //                ColorList = DistinctColor.ToList<string>();
            //            }
            //            if (proclist.Count > 0)
            //            {
            //                var Choose = from c in proclist where c.Process_Name == appt.Description select c;//(IList<ProcessMaster>)Session["proclist"] 
            //                tempProc = Choose.ToList<ProcessMaster>();
            //            }

            //            Color tmpForeColor = Color.FromName(tempProc[0].Process_Color);
            //            IList<string> colrList = ColorList;//(IList<string>)Session["ColorList"];
            //            if (colrList.Count > 0)
            //            {
            //                for (int q = 0; q < colrList.Count; q++)
            //                {
            //                    if (tempProc[0].Process_Color == colrList[q].ToString())
            //                    {
            //                        // appt.CssClass = "rsWrap";
            //                        if (LoadApptList.Document_Type[i] != "DOCUMENTATION")
            //                        {
            //                            appt.BackColor = Color.FromName(tempProc[0].Process_Color);
            //                            appt.BorderColor = Color.Black;
            //                            schAppointmentScheduler.InsertAppointment(appt);
            //                        }


            //                    }
            //                }
            //            }
            //        }

            //        catch
            //        {

            //        }
            //    }
            //    #region BlockDaysAppointment

            //    IList<Blockdays> blockList;
            //    blockList = LoadApptList.blockList;
            //    for (int i = 0; i < blockList.Count; i++)
            //    {
            //        try
            //        {
            //            DateTime from = Convert.ToDateTime(blockList[i].From_Time);
            //            DateTime to = Convert.ToDateTime(blockList[i].To_Time);

            //            int i1 = (to - from).Hours * 60;
            //            int i2 = (to - from).Minutes;
            //            TimeSpan ts = new TimeSpan(0, i1 + i2, 0);

            //            DateTime dt = blockList[i].Block_Date.AddHours(Convert.ToDateTime(blockList[i].From_Time).Hour).AddMinutes(Convert.ToDateTime(blockList[i].From_Time).Minute);

            //            appt = new Telerik.Web.UI.Appointment();

            //            //if (Calendar1.SelectedDate <= blockList[i].To_Date_Choosen && Calendar1.SelectedDate >= blockList[i].From_Date_Choosen && Calendar1.SelectedDate.ToString("dddd") == blockList[i].Day_Choosen)
            //            //{
            //            if (blockList[i].Block_Type != "RECURSIVE" && blockList[i].Block_Type != "NON RECURSIVE")
            //            {

            //                TypeofVistList.Add(blockList[i].Block_Type.ToString() + "|" + blockList[i].From_Time.ToString() + "|" + blockList[i].To_Time.ToString() + "|" + blockList[i].Block_Date.ToString() + "|" + blockList[i].Physician_ID);
            //                continue;
            //            }
            //            //appt.UniqueId = new EventId(0);
            //            // appt.ResourceId = new EventId(blockList[i].Physician_ID);
            //            string sStart = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].From_Time).ToShortTimeString();
            //            //string sStart = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].From_Time).ToShortTimeString();
            //            appt.Start = Convert.ToDateTime(sStart);
            //            string sEnd = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].To_Time).ToShortTimeString();
            //            // string sEnd = Convert.ToDateTime(blockList[i].From_Date_Choosen).ToShortDateString() + " " + Convert.ToDateTime(blockList[i].To_Time).ToShortTimeString();
            //            appt.End = Convert.ToDateTime(sEnd);
            //            appt.Subject = Convert.ToDateTime(sStart).ToString("hh:mm:ss tt") + "-" + Convert.ToDateTime(sEnd).ToString("hh:mm:ss tt") + "-" + blockList[i].Reason;
            //            string ProviderName = string.Empty;
            //            for (int j = 0; j < chklstProviders.Items.Count; j++)
            //            {
            //                if (chklstProviders.Items[j].Selected == true)
            //                {
            //                    if (chklstProviders.Items[j].Value == blockList[i].Physician_ID.ToString())
            //                    {
            //                        ProviderName = chklstProviders.Items[j].Text;
            //                        break;
            //                    }
            //                }
            //            }

            //            appt.Resources.Add(new Resource("Physician", blockList[i].Physician_ID.ToString(), ProviderName));
            //            appt.Description = string.Empty;
            //            //To assign the Color
            //            if (System.Configuration.ConfigurationSettings.AppSettings["AppointmentBlockColor"] == null)
            //            {
            //                throw new Exception("Plese specify Appointment Back Color in the Config File");
            //            }
            //            IList<ProcessMaster> TempProcList;
            //            TempProcList = ApplicationObject.processMasterList;
            //            if (TempProcList.Count > 0)
            //            {
            //                var ColoredProcess = (from colorprocess in TempProcList where colorprocess.Process_Color != string.Empty select colorprocess);
            //                proclist = ColoredProcess.ToList<ProcessMaster>();
            //                var DistinctColor = (from colorporcess in TempProcList select colorporcess.Process_Color).Distinct();
            //                ColorList = DistinctColor.ToList<string>();
            //            }

            //            Color tmpForeColor = Color.FromName(System.Configuration.ConfigurationSettings.AppSettings["AppointmentBlockColor"]);
            //            IList<string> colrList = ColorList;//(IList<string>)Session["ColorList"];
            //            //if (colrList.Count > 0)
            //            //{
            //            //    for (int q = 0; q < colrList.Count; q++)
            //            //    {
            //            //if (tempProc[0].Process_Color == colrList[q].ToString())
            //            //{
            //            appt.BackColor = tmpForeColor;
            //            appt.BorderColor = Color.Black;
            //            appt.Font.Bold = true;
            //            schAppointmentScheduler.InsertAppointment(appt);

            //            //    }
            //            //}
            //            //}



            //        }

            //        //}

            //        catch
            //        {

            //        }
            //    }

            //    #endregion
            //    RefreshMyResources();
            //    hdnApptFacName.Value = cboFacilityName.SelectedValue;
            //    Session["TypeofVisit"] = TypeofVistList;
            //}
            #endregion
        }

        private void RefreshMyResources()
        {
            if (hdnSourceScreen.Value == "AppointmentFacility")
            {
                try
                {
                    this.schAppointmentScheduler.Resources.Clear();
                    this.schAppointmentScheduler.ResourceTypes.Clear();
                }
                catch
                {

                }

                ResourceType restype = new ResourceType("Facility");
                schAppointmentScheduler.ResourceTypes.Add(restype);
                //Session["restype"] = restype;
                for (int j = 0; j < chklstProviders.Items.Count; j++)
                {
                    if (chklstProviders.Items[j].Selected == true)
                    {
                        try
                        {
                            schAppointmentScheduler.Resources.Add(new Resource("Facility", chklstProviders.Items[j].Text, chklstProviders.Items[j].Text));
                        }
                        catch
                        {
                        }
                        schAppointmentScheduler.GroupBy = "Facility";
                    }
                }
                schAppointmentScheduler.GroupingDirection = GroupingDirection.Horizontal;
            }
            else
            {
                try
                {
                    this.schAppointmentScheduler.Resources.Clear();
                    this.schAppointmentScheduler.ResourceTypes.Clear();
                }
                catch
                {
                }

                ResourceType restype = new ResourceType("Physician");
                schAppointmentScheduler.ResourceTypes.Add(restype);
                //Session["restype"] = restype;
                for (int j = 0; j < chklstProviders.Items.Count; j++)
                {
                    if (chklstProviders.Items[j].Selected == true)
                    {
                        try
                        {
                            //schAppointmentScheduler.Resources.Add(new Resource("Physician", "4", "Quinton -  Pererras Ernie"));
                            schAppointmentScheduler.Resources.Add(new Resource("Physician", chklstProviders.Items[j].Value.ToString(), chklstProviders.Items[j].Text));
                        }
                        catch
                        {
                        }
                        schAppointmentScheduler.GroupBy = "Physician";
                    }
                }
                schAppointmentScheduler.GroupingDirection = GroupingDirection.Horizontal;
            }
            //  RefreshActiveView();
        }

        public void FillCheckListBoxForPhysicianSetUp(string facility_name, bool bSelectDefault)
        {
            //bug id 71401
            if (cboFacilityName.SelectedItem != null)
                hdnApptPhyId.Value = cboFacilityName.SelectedItem.Value.ToString();

            //Commented for bug id 71401 - Start
            //IList<PhysicianLibrary> phyList = UtilityManager.GetPhysicianList(facility_name);

            //if (phyList != null && phyList.Count > 0)// For physician logging into unmapped facility, this list will not contain the physician.
            //{
            //    var phy = from p in phyList
            //              where p.PhyPrefix == cboFacilityName.SelectedItem.Text.Split(' ')[0]
            //                    && p.PhyFirstName == cboFacilityName.SelectedItem.Text.Split(' ')[1]
            //                    && p.PhyMiddleName == cboFacilityName.SelectedItem.Text.Split(' ')[2]
            //                    && p.PhyLastName == cboFacilityName.SelectedItem.Text.Split(' ')[3]
            //              select p.Id;
            //    if (phy.Count() > 0)// For physician logged in into unmapped facility, this list will be empty. 
            //        //hdnApptPhyId.Value will consist of the value of logged in physician which is loaded during page load
            //        hdnApptPhyId.Value = phy.First().ToString();
            //Commented for bug id 71401 - End

            IList<MapFacilityPhysician> facList = new List<MapFacilityPhysician>();

            //Commented for bug id 71401 - Start
            //if (phy != null && phy.Count() > 0)// For physician logged into unmapped facility, this list will be empty. 
            ////facList will be empty, i.e count 0
            //{
            //    facList = UtilityManager.GetFacilityListMappedToPhysician(phy.First().ToString());
            //}
            //Commented for bug id 71401 - End

            if (hdnApptPhyId.Value.ToString() != string.Empty)// For physician logged into unmapped facility, this list will be empty. 
                                                              //facList will be empty, i.e count 0
            {
                //CAP-3268
                //facList = UtilityManager.GetFacilityListMappedToPhysician(hdnApptPhyId.Value.ToString());
                PhysicianManager physicianManager = new PhysicianManager();
                facList = physicianManager.GetFacilityListMappedByPhysician(Convert.ToUInt64(hdnApptPhyId.Value));
                //CAP-3491
                if (facList != null && facList.Count > 0 && chkShowActive.Checked == false)
                {
                    facList = facList.Where(a => a.Status == "Y").ToList();
            }
            }
            //IList<FacilityLibrary> facilityList = ApplicationObject.facilityLibraryList;
            var faclist = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
            IList<FacilityLibrary> facilityList = faclist.ToList<FacilityLibrary>();
            IList<FacilityLibrary> TempFac;
            if (facList.Count == 0)// For physician logged into unmapped facility, this list will be empty. 
                                   //facList will be empty, i.e count 0
            {
                facList = null;
                TempFac = facilityList;
                schAppointmentScheduler.Visible = false;
                chkShowActive.Checked = true;
            }
            else
            {
                var fac = from f in facilityList where f.Fac_Name == facList[0].Facility_Name select f;
                TempFac = fac.ToList<FacilityLibrary>();
                schAppointmentScheduler.Visible = true;
                //CAP-3491
                //chkShowActive.Checked = false;
            }

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            if (TempFac.Count > 0)
            {
                if (TempFac[0].Start_Time != string.Empty)
                    dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
                if (TempFac[0].End_Time != string.Empty)
                    dtEnd = Convert.ToDateTime(TempFac[0].End_Time);
            }

            try
            {
                schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
                schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
                schAppointmentScheduler.MinutesPerRow = TempFac[0].Slot_Length;
            }
            catch
            {
            }
            chklstProviders.Items.Clear();
            if (facList != null)// For physician logged into unmapped facility, this list will be null.
            {
                for (int i = 0; i < facList.Count; i++)
                {
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    item.Text = facList[i].Facility_Name;
                    item.Attributes["data-status"] = facList[i].Status;
                    //item.Value = facList[i].Phy_Rec_ID.ToString();
                    chklstProviders.Items.Add(item);
                    if (bSelectDefault)
                    {
                        //if (facList[i].Facility_Name.Trim() == ClientSession.FacilityName.Trim())
                        //    chklstProviders.Items[i].Selected = true;
                        //else
                        //    chklstProviders.Items[i].Selected = false;
                        chklstProviders.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                //facList = UtilityManager.GetFacilityListMappedToPhysician(hdnApptPhyId.Value);
                PhysicianManager physicianManager = new PhysicianManager();
                //CAP-3493
                facList = physicianManager.GetFacilityListMappedByPhysician(Convert.ToUInt64(string.IsNullOrEmpty(hdnApptPhyId.Value) ? "0" : hdnApptPhyId.Value));
                for (int i = 0; i < facList.Count; i++)
                {
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    item.Text = facList[i].Facility_Name;
                    item.Attributes["data-status"] = facList[i].Status;
                    //item.Value = facList.Any(fac => fac.Facility_Name.ToUpper().Trim() == facilityList[i].Fac_Name.ToUpper().Trim()) ? hdnApptPhyId.Value : (0).ToString();
                    chklstProviders.Items.Add(item);
                    if (bSelectDefault)
                    {
                        //if (facilityList[i].Fac_Name.Trim() == ClientSession.FacilityName.Trim())
                        //    chklstProviders.Items[i].Selected = true;
                        //else
                        //    chklstProviders.Items[i].Selected = false;
                        chklstProviders.Items[i].Selected = true;
                    }
                }
                chkShowActive.Checked = true;
            }
            //}
        }

        public void FillCheckListBoxForFOSetUp(string facility_name, bool bSelectDefault)
        {
            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
            //IList<FacilityLibrary> facList = ApplicationObject.facilityLibraryList;
            var faclist = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
            IList<FacilityLibrary> facList = faclist.ToList<FacilityLibrary>();
            IList<FacilityLibrary> TempFac;
            var fac = from f in facList where f.Fac_Name.ToString().Contains(facility_name) select f;
            TempFac = fac.ToList<FacilityLibrary>();

            if (TempFac != null)
            {
                try
                {
                    DateTime dtStart = Convert.ToDateTime(TempFac[0].Start_Time);
                    DateTime dtEnd = Convert.ToDateTime(TempFac[0].End_Time);

                    schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
                    schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
                    schAppointmentScheduler.MinutesPerRow = TempFac[0].Slot_Length;
                }
                catch
                {
                    DateTime dtStart = Convert.ToDateTime("08:00:00");
                    DateTime dtEnd = Convert.ToDateTime("17:00:00");

                    schAppointmentScheduler.DayStartTime = TimeSpan.FromHours(Convert.ToDouble(dtStart.Hour));
                    schAppointmentScheduler.DayEndTime = TimeSpan.FromHours(Convert.ToDouble(dtEnd.Hour));
                    schAppointmentScheduler.MinutesPerRow = 15;
                }
            }
            if (rdoInActiveProviders.Checked)
            {
                PhysicianList = UtilityManager.GetInActiveProviderList(cboFacilityName.SelectedItem.Text, ClientSession.LegalOrg, false);
            }
            else
            {
                if (chkShowActive.Checked || rdoAllActiveProviders.Checked)
                    PhysicianList = UtilityManager.GetPhysicianList("", ClientSession.LegalOrg);
                else
                {
                    PhysicianList = UtilityManager.GetPhysicianList(facility_name, ClientSession.LegalOrg);
                    var otherPhysicianList = UtilityManager.GetInActiveProviderList(facility_name, ClientSession.LegalOrg, true);
                    foreach (var item in otherPhysicianList)
                    {
                        PhysicianList.Add(item);
                    }
                }
            }
            chklstProviders.Items.Clear();

            if (PhysicianList != null)
            {
                if (PhysicianList.Count == 0)
                {
                    schAppointmentScheduler.Visible = false;
                    return;
                }
                else
                {
                    schAppointmentScheduler.Visible = true;
                }

                XmlDocument xmldoc = new XmlDocument();
                //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
                //CAP-2781
                string sDefaultPhysicians = "";
                PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
                if (physicianFacilityMappingList != null)
                {
                    //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");

                    //XmlNode nodeMatchingFacility = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + cboFacilityName.SelectedItem.Text.Trim() + "']");
                    var matchingFacility = physicianFacilityMappingList.PhysicianFacility.FirstOrDefault(x=> x.name == cboFacilityName.SelectedItem.Text.Trim());
                    if (matchingFacility != null)
                    {
                        sDefaultPhysicians = matchingFacility.defaultphysicianid;
                    }
                    //CAP-3500
                    else
                    {
                        PhysicianManager physicianManager = new PhysicianManager();
                        var lstMatchingFacility = physicianManager.GetDefaultPhysicianByFacility(cboFacilityName.SelectedItem.Text.Trim());
                        matchingFacility = lstMatchingFacility.FirstOrDefault();
                        //CAP-3518
                        sDefaultPhysicians = matchingFacility?.defaultphysicianid ?? "";
                    }
                }
                string[] lstDefaultPhysicians = sDefaultPhysicians.Split(',');

                //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                //{
                //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
                //}
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == cboFacilityName.SelectedItem.Text select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                for (int i = 0; i < PhysicianList.Count; i++)
                {
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();

                    /* if (sAncillary != string.Empty && sAncillary == cboFacilityName.SelectedItem.Text.Trim())
                     {
                         pnlProvidersHeader.InnerText = "Machine - Technician";
                         string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                         if (File.Exists(strXmlFilePathTech) == true)
                         {
                             xmldoc = new XmlDocument();
                             xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                             if (PhysicianList[i].PhyColor!="" && PhysicianList[i].PhyColor != "0")
                             {
                                 XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                                 item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                 item.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                             }
                             else
                             {
                                 item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                 item.Value = PhysicianList[i].Id.ToString();
                             }

                         }
                     }
                     else
                     {
                         pnlProvidersHeader.InnerText = "Providers";
                         item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                         item.Value = PhysicianList[i].Id.ToString();
                     }
                    chklstProviders.Items.Add(item);*/
                    //BugID:53256
                    //if (sAncillary != string.Empty && sAncillary == cboFacilityName.SelectedItem.Text.Trim() && !chkShowActive.Checked)
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y" && !chkShowActive.Checked)
                    {
                        pnlProvidersHeader.InnerText = "Machine - Technician";
                    }
                    else
                    {
                        pnlProvidersHeader.InnerText = "Providers";
                    }

                    //Jira CAP-2777
                    //string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                    //if (File.Exists(strXmlFilePathTech) == true)
                    {
                        //Jira CAP-2777
                        //xmldoc = new XmlDocument();
                        //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                        if (PhysicianList[i].PhyColor != "" && PhysicianList[i].PhyColor != "0")
                        {
                            //Jira CAP-2777
                            //XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhysicianList[i].PhyColor);
                            //if (xmlTec != null && xmlTec[0] != null)
                            //{
                            //    item.Text = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                            //    item.Value = xmlTec[0].Attributes.GetNamedItem("machine_technician_library_id").Value;
                            //}
                            MachinetechnicianList machinetechnicianList = new MachinetechnicianList();
                            machinetechnicianList = ConfigureBase<MachinetechnicianList>.ReadJson("machine_technician.json");
                            if (machinetechnicianList?.MachineTechnician != null)
                            {
                                List<Machinetechnician> machinetechnicians = new List<Machinetechnician>();
                                machinetechnicians = machinetechnicianList.MachineTechnician.Where(x => x.machine_technician_library_id == PhysicianList[i].PhyColor).ToList();
                                var filterData = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().ToList();
                                if ((machinetechnicians?.Count ?? 0) > 0 && !filterData.Any(a => a.Value == machinetechnicians[0].machine_technician_library_id))
                                {
                                    item.Text = machinetechnicians[0].machine_name + " - " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyLastName; //PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                                    item.Value = machinetechnicians[0].machine_technician_library_id;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            //old code
                            //item.Text = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName;
                            //Gitlab# 2485 - Physician Name Display Change
                            if (PhysicianList[i].PhyLastName != String.Empty)
                                item.Text += PhysicianList[i].PhyLastName;
                            if (PhysicianList[i].PhyFirstName != String.Empty)
                            {
                                if (item.Text != String.Empty)
                                    item.Text += "," + PhysicianList[i].PhyFirstName;
                                else
                                    item.Text += PhysicianList[i].PhyFirstName;
                            }
                            if (PhysicianList[i].PhyMiddleName != String.Empty)
                                item.Text += " " + PhysicianList[i].PhyMiddleName;
                            if (PhysicianList[i].PhySuffix != String.Empty)
                                item.Text += "," + PhysicianList[i].PhySuffix;
                            item.Value = PhysicianList[i].Id.ToString();
                        }
                        chklstProviders.Items.Add(item);
                    }
                    if (bSelectDefault)
                    {
                        if (lstDefaultPhysicians.Any(curritem => curritem == PhysicianList[i].Id.ToString()))
                            chklstProviders.Items[i].Selected = true;
                        else
                            chklstProviders.Items[i].Selected = false;
                    }
                }
                SortPhysician();
            }
        }

        //CAP-1949 - User list Alpha sort is missing in Appointment scheduler and Create order when click show All, so its difficult to find the user
        private void SortPhysician()
        {
            if (pnlProvidersHeader.InnerText == "Providers" || pnlProvidersHeader.InnerText == "Machine - Technician")
            {
                var comboBoxItems = chklstProviders.Items.Cast<System.Web.UI.WebControls.ListItem>().ToList();
                chklstProviders.Items.Clear();
                chklstProviders.Items.AddRange(comboBoxItems.OrderBy(a => a.Text.Trim()).ToArray());
            }
            else
            {
                var comboBoxItems = cboFacilityName.Items.Cast<System.Web.UI.WebControls.ListItem>().ToList();
                cboFacilityName.Items.Clear();
                cboFacilityName.Items.AddRange(comboBoxItems.OrderBy(a => a.Text.Trim()).ToArray());
            }
        }
        #endregion

        #region PDF Declarations and Methods

        private class HeaderEventGenerate : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetRight(220), pageSize.GetTop(40));
                //cb.ShowText(frmRxOrder.patientInfo);
                cb.EndText();
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetRight(70), pageSize.GetBottom(40));
                cb.ShowText("Page " + writer.PageNumber.ToString());
                cb.EndText();
            }
        }

        private PdfPCell CreateCell(string sText, string sType)
        {
            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell();

            if (sType == "Header")
            {
                Paragraph par = new Paragraph(sText, reducedFont);
                cell.AddElement(par);
            }
            else if (sType == "Text")
            {
                Paragraph par = new Paragraph(sText, normalFont);
                cell.AddElement(par);
            }

            //par = new Paragraph(ValueText, normalFont);
            //cell.AddElement(par);
            return cell;
        }

        private string ReturnSpace(string sText)
        {
            string sReturnSpace = "";

            //90 - Max length of the 2nd column in print superbill
            if (sText != "")
            {
                int iIndexSpace = 90 - sText.Length;

                for (int k = 0; k < iIndexSpace; k++)
                {
                    if (sReturnSpace == "")
                        sReturnSpace = " ";
                    else
                        sReturnSpace = sReturnSpace + " ";
                }
            }

            return sReturnSpace;
        }

        private PdfPCell CreateCellDynamic(string HeaderText, string ValueText, string ModuleText)
        {
            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
            PdfPCell cell = new PdfPCell();
            Paragraph par = new Paragraph(HeaderText, reducedFont);
            cell.AddElement(par);
            par = new Paragraph(ValueText, normalFont);
            cell.AddElement(par);
            par = new Paragraph(ModuleText, HeadFont);
            cell.AddElement(par);
            return cell;
        }

        #endregion

        protected void btnBulkEncTemplate_Click(object sender, EventArgs e)
        {
            //if (ClientSession.UserRole != null && hdnSourceScreen.Value == "AppointmentFacility" || ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            //{
            //    //Physican,PhysicanAssistant
            //    BulkTemplateForMultipleFacilitySameProvider();
            //}
            //else
            //{
            //    //MA,FO
            //    BulkTemplateForMultipleProviderSameFacility();
            //}

        }

        // Method not in use
        //void BulkTemplateForMultipleFacilitySameProvider()
        //{

        //    hdnPhysisicanChecked.Value = string.Empty;
        //    string sFacility = string.Empty;
        //    for (int k = 0; k < chklstProviders.Items.Count; k++)
        //    {
        //        if (chklstProviders.Items[k].Selected == true)
        //        {
        //            if (hdnPhysisicanChecked.Value != null && hdnPhysisicanChecked.Value == string.Empty && sFacility == string.Empty)
        //            {
        //                hdnPhysisicanChecked.Value = chklstProviders.Items[k].Text;
        //                sFacility = chklstProviders.Items[k].Value;
        //            }
        //            else
        //            {
        //                hdnPhysisicanChecked.Value += "|" + chklstProviders.Items[k].Text;
        //                sFacility += "|" + chklstProviders.Items[k].Value;
        //            }
        //        }
        //    }
        //    if (sFacility != string.Empty)
        //    {
        //        string sPhyID = cboFacilityName.Text;
        //        string sPhysicianNameSelected = hdnPhysisicanChecked.Value;
        //        DateTime sSelectedDate = DateTime.MinValue;
        //        sSelectedDate = Convert.ToDateTime(hdnSelectedDate.Value);

        //        IList<Encounter> lstEnc = new List<Encounter>();
        //        EncounterManager objEnc = new EncounterManager();
        //        string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Encounter_Template.xsl");
        //        string sProjectName = System.Configuration.ConfigurationSettings.AppSettings["ProjectName"];
        //        UtilityManager umanger = new UtilityManager();

        //        lstEnc = objEnc.GetAppointmentForBulkEncounterTemplateFacility(sFacility, sSelectedDate, sPhyID);
        //        if (lstEnc.Count > 0)
        //        {
        //            string NotesNameConvention = System.Configuration.ConfigurationSettings.AppSettings["EncounterTempelateNamingConvention"];
        //            string NotesName = string.Empty;
        //            string sFolderWithFacility = Server.MapPath("atala-capture-download/" + Session.SessionID + "/" + System.Configuration.ConfigurationSettings.AppSettings["ProjectName"]);

        //            string[] Facilityarry = sFacility.Split('|');
        //            //foreach (string s in Facilityarry)
        //            //{
        //            //    string sFolderFac= sFolderWithFacility + "/" + s;
        //            //    DirectoryInfo objdirect = new DirectoryInfo(sFolderFac);
        //            //    if (!objdirect.Exists)
        //            //        objdirect.Create();
        //            //}

        //            foreach (string s in Facilityarry)
        //            {
        //                IList<Encounter> lstTempEnc = new List<Encounter>();
        //                lstTempEnc = lstEnc.Where(a => a.Facility_Name == s).ToList<Encounter>();
        //                if (lstTempEnc.Count > 0)
        //                {
        //                    string sPhysicianName = Path.Combine(sFolderWithFacility, s + "/" + cboFacilityName.SelectedItem.Text);
        //                    DirectoryInfo obj = new DirectoryInfo(sPhysicianName);
        //                    if (!obj.Exists)
        //                    {
        //                        obj.Create();
        //                    }
        //                    foreach (FileInfo file in obj.GetFiles())
        //                    {
        //                        file.Delete();
        //                    }
        //                    foreach (Encounter enc in lstTempEnc)
        //                    {
        //                        umanger = new UtilityManager();
        //                        string sHumanId = enc.Human_ID.ToString();
        //                        string Encounter_Id = enc.Encounter_ID.ToString();

        //                        string sHumanXmlPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + sHumanId + ".xml");


        //                        //Update TargetEncounter Tag in Human Xml.,For XSLT to find the current encounter id
        //                        if (File.Exists(sHumanXmlPath) == true)
        //                        {
        //                            XmlDocument itemDoc = new XmlDocument();
        //                            XmlTextReader XmlText = new XmlTextReader(sHumanXmlPath);
        //                            itemDoc.Load(XmlText);
        //                            XmlNodeList xmlTargetEncounterId = itemDoc.GetElementsByTagName("TargetEncounterId");
        //                            if (xmlTargetEncounterId != null && xmlTargetEncounterId.Count > 0)
        //                                xmlTargetEncounterId[0].InnerXml = Encounter_Id + "_" + UtilityManager.ConvertToLocal(enc.Appointment_Date).ToString("yyyy-MM-dd hh:mm:ss tt");
        //                            else
        //                            {
        //                                //If TargetEncounter Node Not exists in human Xml we Need to create the traget tag.
        //                                XmlNodeList xmlSectionList = itemDoc.GetElementsByTagName("Modules");
        //                                XmlNode Newnode = null;
        //                                Newnode = itemDoc.CreateNode(XmlNodeType.Element, "TargetEncounterId", "");
        //                                Newnode.InnerText = Encounter_Id.ToString() + "_" + UtilityManager.ConvertToLocal(enc.Appointment_Date).ToString("yyyy-MM-dd hh:mm:ss tt");
        //                                xmlSectionList[0].AppendChild(Newnode);
        //                            }
        //                            XmlText.Close();
        //                           // itemDoc.Save(sHumanXmlPath);
        //                            int trycount = 0;
        //                        trytosaveagain:
        //                            try
        //                            {
        //                                itemDoc.Save(sHumanXmlPath);
        //                            }
        //                            catch (Exception xmlexcep)
        //                            {
        //                                trycount++;
        //                                if (trycount <= 3)
        //                                {
        //                                    int TimeMilliseconds = 0;
        //                                    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                                        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                                    Thread.Sleep(TimeMilliseconds);
        //                                    string sMsg = string.Empty;
        //                                    string sExStackTrace = string.Empty;

        //                                    string version = "";
        //                                    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                                        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                                    string[] server = version.Split('|');
        //                                    string serverno = "";
        //                                    if (server.Length > 1)
        //                                        serverno = server[1].Trim();

        //                                    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                                        sMsg = xmlexcep.InnerException.Message;
        //                                    else
        //                                        sMsg = xmlexcep.Message;

        //                                    if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                                        sExStackTrace = xmlexcep.StackTrace;

        //                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                                    string ConnectionData;
        //                                    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                                    using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                                    {
        //                                        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                                        {
        //                                            cmd.Connection = con;
        //                                            try
        //                                            {
        //                                                con.Open();
        //                                                cmd.ExecuteNonQuery();
        //                                                con.Close();
        //                                            }
        //                                            catch
        //                                            {
        //                                            }
        //                                        }
        //                                    }
        //                                    goto trytosaveagain;
        //                                }
        //                            }


        //                            Encounter_Id = "Encounter_" + Convert.ToDateTime(enc.Appointment_Date).ToString("yyyyMMdd");

        //                            NotesName = umanger.NamingConventionGeneration(NotesNameConvention, sHumanId, Encounter_Id, s, cboFacilityName.SelectedItem.Text);
        //                            string WordOutputName = NotesName + ".doc";
        //                            string outputDocument = Path.Combine(sPhysicianName, WordOutputName);
        //                            DataSet ds;
        //                            XmlDataDocument xmlDoc;
        //                            XslCompiledTransform xslTran;
        //                            XmlElement root;
        //                            XPathNavigator nav;
        //                            XmlTextWriter writer;
        //                            XsltSettings settings = new XsltSettings(true, false);
        //                            ds = new DataSet();
        //                            ds.ReadXml(sHumanXmlPath);
        //                            xmlDoc = new XmlDataDocument(ds);
        //                            xslTran = new XslCompiledTransform();
        //                            using (var stream = File.Open(xsltFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                            {
        //                                xslTran.Load(xsltFile, settings, new XmlUrlResolver());
        //                            }
        //                            root = xmlDoc.DocumentElement;
        //                            nav = root.CreateNavigator();
        //                            if (File.Exists(outputDocument))
        //                            {
        //                                File.Delete(outputDocument);
        //                            }
        //                            writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
        //                            xslTran.Transform(nav, writer);
        //                            writer.Close();
        //                            writer = null;
        //                            nav = null;
        //                            root = null;
        //                            xmlDoc = null;
        //                            ds = null;
        //                        }
        //                    }
        //                }
        //            }
        //            Response.Clear();
        //            Response.ContentType = "application/zip";
        //            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", sProjectName + ".zip"));
        //            using (ZipFile zip = new ZipFile())
        //            {
        //                zip.AddSelectedFiles("*", sFolderWithFacility, string.Empty, true);
        //                zip.Save(Response.OutputStream);
        //            }
        //            Response.End();
        //        }
        //        else
        //        {
        //            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", "DisplayErrorMessage('110089');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        }
        //    }
        //    else
        //    {
        //        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", "DisplayErrorMessage('110091');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //    }
        //}
        //method not in use
        //void BulkTemplateForMultipleProviderSameFacility()
        //{
        //    hdnPhysisicanChecked.Value = string.Empty;
        //    string sPhysicianID = string.Empty;
        //    for (int k = 0; k < chklstProviders.Items.Count; k++)
        //    {
        //        if (chklstProviders.Items[k].Selected == true)
        //        {
        //            if (hdnPhysisicanChecked.Value == string.Empty && sPhysicianID == string.Empty)
        //            {
        //                hdnPhysisicanChecked.Value = chklstProviders.Items[k].Text;
        //                sPhysicianID = chklstProviders.Items[k].Value;
        //            }
        //            else
        //            {
        //                hdnPhysisicanChecked.Value += "|" + chklstProviders.Items[k].Text;
        //                sPhysicianID += "," + chklstProviders.Items[k].Value;
        //            }
        //        }
        //    }
        //    if (sPhysicianID != string.Empty)
        //    {
        //        string sFaciltyName = cboFacilityName.Text;
        //        string sPhysicianNameSelected = hdnPhysisicanChecked.Value;
        //        DateTime sSelectedDate = DateTime.MinValue;
        //        sSelectedDate = Convert.ToDateTime(hdnSelectedDate.Value);

        //        IList<Encounter> lstEnc = new List<Encounter>();
        //        EncounterManager objEnc = new EncounterManager();
        //        string xsltFile = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "EHR_Encounter_Template.xsl");
        //        string sProjectName = System.Configuration.ConfigurationSettings.AppSettings["ProjectName"];
        //        UtilityManager umanger = new UtilityManager();

        //        lstEnc = objEnc.GetAppointmentForBulkEncounterTemplate(sPhysicianID, sSelectedDate, sFaciltyName);
        //        if (lstEnc.Count > 0)
        //        {
        //            string NotesNameConvention = System.Configuration.ConfigurationSettings.AppSettings["EncounterTempelateNamingConvention"];
        //            string NotesName = string.Empty;
        //            string sFolderWithFacility = Server.MapPath("atala-capture-download/" + Session.SessionID + "/" + System.Configuration.ConfigurationSettings.AppSettings["ProjectName"]);
        //            DirectoryInfo objdirect = new DirectoryInfo(sFolderWithFacility);
        //            if (!objdirect.Exists)
        //                objdirect.Create();

        //            string[] PhysicianID = sPhysicianID.Split(',');

        //            foreach (string s in PhysicianID)
        //            {
        //                IList<Encounter> lstTempEnc = new List<Encounter>();
        //                lstTempEnc = lstEnc.Where(a => a.Encounter_Provider_ID == Convert.ToInt32(s)).ToList<Encounter>();
        //                if (lstTempEnc.Count > 0)
        //                {
        //                    string sPhysicianName = Path.Combine(sFolderWithFacility, sFaciltyName + "/" + chklstProviders.Items.FindByValue(s).Text);
        //                    DirectoryInfo obj = new DirectoryInfo(sPhysicianName);
        //                    if (!obj.Exists)
        //                    {
        //                        obj.Create();
        //                    }
        //                    foreach (FileInfo file in obj.GetFiles())
        //                    {
        //                        file.Delete();
        //                    }
        //                    foreach (Encounter enc in lstTempEnc)
        //                    {
        //                        umanger = new UtilityManager();
        //                        string sHumanId = enc.Human_ID.ToString();
        //                        string Encounter_Id = enc.Encounter_ID.ToString();
        //                        string sHumanXmlPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + sHumanId + ".xml");

        //                        //Update TargetEncounter Tag in Human Xml.,For XSLT to find the current encounter id
        //                        if (File.Exists(sHumanXmlPath) == true)
        //                        {
        //                            XmlDocument itemDoc = new XmlDocument();
        //                            XmlTextReader XmlText = new XmlTextReader(sHumanXmlPath);
        //                            itemDoc.Load(XmlText);
        //                            XmlNodeList xmlTargetEncounterId = itemDoc.GetElementsByTagName("TargetEncounterId");
        //                            if (xmlTargetEncounterId != null && xmlTargetEncounterId.Count > 0)
        //                                xmlTargetEncounterId[0].InnerXml = Encounter_Id + "_" + UtilityManager.ConvertToLocal(enc.Appointment_Date).ToString("yyyy-MM-dd hh:mm:ss tt");
        //                            else
        //                            {
        //                                //If TargetEncounter Node Not exists in human Xml we Need to create the traget tag.
        //                                XmlNodeList xmlSectionList = itemDoc.GetElementsByTagName("Modules");
        //                                XmlNode Newnode = null;
        //                                Newnode = itemDoc.CreateNode(XmlNodeType.Element, "TargetEncounterId", "");
        //                                Newnode.InnerText = Encounter_Id.ToString() + "_" + UtilityManager.ConvertToLocal(enc.Appointment_Date).ToString("yyyy-MM-dd hh:mm:ss tt");
        //                                xmlSectionList[0].AppendChild(Newnode);
        //                            }
        //                            XmlText.Close();
        //                            //itemDoc.Save(sHumanXmlPath);
        //                            int trycount = 0;
        //                        trytosaveagain:
        //                            try
        //                            {
        //                                itemDoc.Save(sHumanXmlPath);
        //                            }
        //                            catch (Exception xmlexcep)
        //                            {
        //                                trycount++;
        //                                if (trycount <= 3)
        //                                {
        //                                    int TimeMilliseconds = 0;
        //                                    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
        //                                        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

        //                                    Thread.Sleep(TimeMilliseconds);
        //                                    string sMsg = string.Empty;
        //                                    string sExStackTrace = string.Empty;

        //                                    string version = "";
        //                                    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
        //                                        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

        //                                    string[] server = version.Split('|');
        //                                    string serverno = "";
        //                                    if (server.Length > 1)
        //                                        serverno = server[1].Trim();

        //                                    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
        //                                        sMsg = xmlexcep.InnerException.Message;
        //                                    else
        //                                        sMsg = xmlexcep.Message;

        //                                    if (xmlexcep != null && xmlexcep.StackTrace != null)
        //                                        sExStackTrace = xmlexcep.StackTrace;

        //                                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
        //                                    string ConnectionData;
        //                                    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        //                                    using (MySqlConnection con = new MySqlConnection(ConnectionData))
        //                                    {
        //                                        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
        //                                        {
        //                                            cmd.Connection = con;
        //                                            try
        //                                            {
        //                                                con.Open();
        //                                                cmd.ExecuteNonQuery();
        //                                                con.Close();
        //                                            }
        //                                            catch
        //                                            {
        //                                            }
        //                                        }
        //                                    }
        //                                    goto trytosaveagain;
        //                                }
        //                            }
        //                            Encounter_Id = "Encounter_" + Convert.ToDateTime(enc.Appointment_Date).ToString("yyyyMMdd");
        //                            NotesName = umanger.NamingConventionGeneration(NotesNameConvention, sHumanId, Encounter_Id, sFaciltyName, chklstProviders.Items.FindByValue(s).Text);
        //                            string WordOutputName = NotesName + ".doc";
        //                            string outputDocument = Path.Combine(sPhysicianName, WordOutputName);
        //                            DataSet ds;
        //                            XmlDataDocument xmlDoc;
        //                            XslCompiledTransform xslTran;
        //                            XmlElement root;
        //                            XPathNavigator nav;
        //                            XmlTextWriter writer;
        //                            XsltSettings settings = new XsltSettings(true, false);
        //                            ds = new DataSet();
        //                            ds.ReadXml(sHumanXmlPath);
        //                            xmlDoc = new XmlDataDocument(ds);
        //                            xslTran = new XslCompiledTransform();
        //                            using (var stream = File.Open(xsltFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                            {
        //                                xslTran.Load(xsltFile, settings, new XmlUrlResolver());
        //                            }
        //                            root = xmlDoc.DocumentElement;
        //                            nav = root.CreateNavigator();
        //                            if (File.Exists(outputDocument))
        //                            {
        //                                File.Delete(outputDocument);
        //                            }
        //                            writer = new XmlTextWriter(outputDocument, System.Text.Encoding.UTF8);
        //                            xslTran.Transform(nav, writer);
        //                            writer.Close();
        //                            writer = null;
        //                            nav = null;
        //                            root = null;
        //                            xmlDoc = null;
        //                            ds = null;
        //                        }
        //                    }
        //                }
        //            }
        //            Response.Clear();
        //            Response.ContentType = "application/zip";
        //            Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", sProjectName + ".zip"));
        //            using (ZipFile zip = new ZipFile())
        //            {
        //                zip.AddSelectedFiles("*", sFolderWithFacility, string.Empty, true);
        //                zip.Save(Response.OutputStream);
        //            }
        //            Response.End();
        //        }
        //        else
        //        {
        //            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", "DisplayErrorMessage('110089');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        }
        //    }
        //    else
        //    {
        //        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", "DisplayErrorMessage('110088');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //    }
        //}


    }
}