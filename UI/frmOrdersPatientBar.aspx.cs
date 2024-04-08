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
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
//using Telerik.WinControls.UI;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Telerik.Web.UI;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;

namespace Acurus.Capella.UI
{
    public partial class frmOrdersPatientBar : System.Web.UI.Page
    {
        EncounterManager objEncounterMngr = new EncounterManager();
        IList<PatientPane> patientPane = new List<PatientPane>();


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
        public string ScreenMode
        {
            get
            {
                return ViewState["ScreenMode"] == null ? string.Empty : Convert.ToString(ViewState["ScreenMode"]);
            }
            set
            {
                ViewState["ScreenMode"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (Context.Session != null)
            {
                if (ClientSession.UserName == string.Empty)
                {
                    string cookie = Request.Headers["Cookie"];
                    if ((cookie != null) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        Server.Transfer("~/frmSessionExpired.aspx");
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (Request["HumanID"] != null)
                {
                    HumanID = Convert.ToUInt32(Request["HumanID"]);
                }
                else
                {
                    HumanID = ClientSession.HumanId;
                }
                EncounterID = 0;
                string sPriPlan = string.Empty;
                string sSecPlan = string.Empty;
                string sPriCarrier = string.Empty;
                string sSecCarrier = string.Empty;
                RefreshPatientBar();
                FillPhysician();
                SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
                objSecurityServiceUtility.ApplyUserPermissions(this);
                //commented by vaishali on 26-11-2015
                //objSecurityServiceUtility.ApplyPBACPermissions(this);
                if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant" )
                {
                    cboPhysician.Enabled = false;
                    btnFindPhysician.Enabled = false;
                    chkShowAll.Enabled = false;
                }
                else if (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole.ToUpper() == "SCRIBE")//&& ClientSession.UserPermission.Trim().ToUpper() == "U")
                {
                    cboPhysician.Enabled = true;
                    chkShowAll.Enabled = true;
                    btnFindPhysician.Enabled = true;
                }
                else if (ClientSession.UserRole == "Front Office")
                {
                    cboPhysician.Enabled = true;
                    chkShowAll.Enabled = true;
                    btnFindPhysician.Enabled = true;
                }
                btnOrder.Visible = false;
                //CAP-1114
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false; localStorage.setItem('bSave', 'true'); ", true);
            }
            
        }
        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }
        public void RefreshPatientBar()
        {
            string sPriPlan = string.Empty;
            string sSecPlan = string.Empty;
            string sPriCarrier = string.Empty;
            string sSecCarrier = string.Empty;
            ulong ulMyHumanID = HumanID;
            //Added by Saravanakumar on 20-06-2013 Bugid:15301
            //Srividhya added the last parameter as set as true
            //patientPane = objEncounterMngr.FillPatientPane(ulMyHumanID, ApplicationObject.macAddress, ClientSession.UserName,true);//Commented For Bug ID: 37106
            FillPatientChart objChart = new FillPatientChart();
            objChart= ClientSession.FillPatientChart;
            if (objChart.PatChartList != null && objChart.PatChartList.Count > 0) //if (patientPane != null && patientPane.Count > 0)
            {
                if (objChart.PatChartList[objChart.PatChartList.Count - 1].Insurance_Plan_ID != null)
                {
                    for (int i = 0; i < objChart.PatChartList[objChart.PatChartList.Count - 1].Insurance_Plan_ID.Count; i++)
                    {
                        if (objChart.PatChartList[objChart.PatChartList.Count - 1].Insurance_Type[i].ToString() == "PRIMARY")
                        {
                            sPriPlan = objChart.PatChartList[objChart.PatChartList.Count - 1].Ins_Plan_Name[i].ToString();
                            sPriCarrier = objChart.PatChartList[objChart.PatChartList.Count - 1].CarrierName[i].ToString();
                        }

                        if (objChart.PatChartList[objChart.PatChartList.Count - 1].Insurance_Type[i].ToString() == "SECONDARY")
                        {
                            sSecPlan = objChart.PatChartList[objChart.PatChartList.Count - 1].Ins_Plan_Name[i].ToString();
                            sSecCarrier = objChart.PatChartList[objChart.PatChartList.Count - 1].CarrierName[i].ToString();
                        }
                    }
                }
            }
            var temp = objChart.PatChartList.Where(a => a.Human_Id == ulMyHumanID);
            int j = 0;
            foreach (var rec in temp)
            {
                if (j > 0)
                    break;
                lblPatientStrip.Items[0].Text = FillPatientSummaryBarforPatientChart(rec.Last_Name, rec.First_Name, rec.MI, rec.Suffix, rec.Birth_Date, rec.Human_Id, rec.Medical_Record_Number, rec.HomePhoneNo, rec.CellPhoneNo, rec.Sex, rec.Patient_Status, rec.SSN, sPriPlan, sPriCarrier, sSecPlan, sSecCarrier,rec.Patient_Type);
                //lblPatientStrip.Caption = UtilityManager.FillPatientSummaryBarforPatientChart(rec.Last_Name, rec.First_Name, rec.MI, rec.Suffix, rec.Birth_Date, rec.Human_Id, rec.Medical_Record_Number, rec.HomePhoneNo, rec.Sex, rec.Patient_Status, rec.SSN, sPriPlan, sPriCarrier, sSecPlan, sSecCarrier);
                j++;
            }

            lblPatientStrip.ToolTip = lblPatientStrip.Items[0].Text;
        }
        IList<PhysicianLibrary> GetPhysicianList(string sFacility)
        {
            IList<PhysicianLibrary> lstPhysician=new List<PhysicianLibrary>();
            XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlDocumentType.CreateReader());
            XmlNodeList lstNodes;
            if(sFacility!=string.Empty)
                lstNodes = xmlDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + sFacility + "']/Physician");
            else
                lstNodes = xmlDoc.SelectNodes("/ROOT/PhyList/Facility/Physician");
            foreach (XmlNode itemNode in lstNodes)
            {
                PhysicianLibrary objPhy = new PhysicianLibrary();
                objPhy.PhyPrefix = itemNode.Attributes["prefix"].Value.ToString();
                objPhy.PhyFirstName = itemNode.Attributes["firstname"].Value.ToString();
                objPhy.PhyMiddleName = itemNode.Attributes["middlename"].Value.ToString();
                objPhy.PhyLastName = itemNode.Attributes["lastname"].Value.ToString();
                objPhy.Category = itemNode.Attributes["username"].Value.ToString();//Temporarily Used to Store UserName
                objPhy.PhySuffix = itemNode.Attributes["suffix"].Value.ToString();
                objPhy.Id = ulong.Parse(itemNode.Attributes["ID"].Value.ToString());
                //GitLab # 2364 - Enabled Menu level Order for Ancillary FO
                if (objPhy.Category != string.Empty && itemNode.Attributes["machine_technician_id"].Value.ToString() == "0")
                {
                    lstPhysician.Add(objPhy);
                }
            }
            return lstPhysician.Distinct().OrderBy(o => o.PhyLastName).ToList() ;
        }
        void FillPhysician()
        {
            PhysicianManager objPhysicianManager = new PhysicianManager();
            bool NotInList = true;
            //FillPhysicianUser PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName);
            IList<PhysicianLibrary> PhyList = GetPhysicianList(ClientSession.FacilityName);
            NotInList = FillComboBox(PhyList);
            //CAP-1910
            if (!PhyList.Any(a => a.Id == ClientSession.PhysicianId))
            {
                cboPhysician.Items.Clear();
                chkShowAll.Checked = true;
                PhyList = GetPhysicianList(string.Empty);
                NotInList = FillComboBox(PhyList);
            }
            if (ClientSession.UserRole.ToUpper()=="SCRIBE")
            {
                cboPhysician.SelectedIndex = 0;
                OpenOrdersScreen();
                return;
            }

            //if ((ClientSession.UserRole != "Medical Assistant" && !ClientSession.UserPermissionDTO.ListProc.Contains("MA_PROCESS")) && NotInList)
            if (NotInList)
            {
                chkShowAll.Checked = true;
                //GitLab # 2364 - Enabled Menu level Order for Ancillary FO
                //if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant" )
                //{
                   cboPhysician.Items.Clear();
                    //PhyUserList = objPhysicianManager.GetPhysicianandUser(false, string.Empty);
                    PhyList = GetPhysicianList(string.Empty);
                    NotInList = FillComboBox(PhyList);
                //}
            }
            //if (UIManager.Is_Dictation)//Added By ThiyagarajanM 11-07-2013 For Dictation
            //{
            //    chkShowAll.Checked = true;
            //}
            if (ClientSession.UserRole == "Medical Assistant")
            {
                cboPhysician.SelectedIndex = 0;
                OpenOrdersScreen();
            }
        }
        bool FillComboBox(IList<PhysicianLibrary> PhyList)
        {
            bool NotInList = true;
            cboPhysician.Items.Add(new RadComboBoxItem(""));
            cboPhysician.Items[0].Value = "0";
            cboPhysician.Items[0].Selected = false;
            //CAP-1897
            string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
            XmlDocument xmldoc = new XmlDocument();
            if (File.Exists(strXmlFilePathTech))
            {
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
            }
            for (int i = 0; i < PhyList.Count; i++)
            {
                string sPhyName = string.Empty;
                if (PhyList[i].PhyColor != "" && PhyList[i].PhyColor != "0" && File.Exists(strXmlFilePathTech))
                {
                    XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhyList[i].PhyColor);
                    if (xmlTec != null && xmlTec[0] != null)
                    {
                        sPhyName = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyLastName;
                    }
                }
                else
                {
                    //Old Code
                    // string sPhyName = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                    //Gitlab# 2485 - Physician Name Display Change
                    //PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                    if (PhyList[i].PhyLastName != String.Empty)
                        sPhyName += PhyList[i].PhyLastName;
                    if (PhyList[i].PhyFirstName != String.Empty)
                    {
                        if (sPhyName != String.Empty)
                            sPhyName += "," + PhyList[i].PhyFirstName;
                        else
                            sPhyName += PhyList[i].PhyFirstName;
                    }
                    if (PhyList[i].PhyMiddleName != String.Empty)
                        sPhyName += " " + PhyList[i].PhyMiddleName;
                    if (PhyList[i].PhySuffix != String.Empty)
                        sPhyName += "," + PhyList[i].PhySuffix;
                }
                //Old Code
                //Telerik.Web.UI.RadComboBoxItem tempItem = new Telerik.Web.UI.RadComboBoxItem(PhyList[i].Category.ToString() + " - " + sPhyName);//Instead of PhyUserList.UserList[i].username;
                //Gitlab# 2485 - Physician Name Display Change
                Telerik.Web.UI.RadComboBoxItem tempItem = new Telerik.Web.UI.RadComboBoxItem(sPhyName);//Instead of PhyUserList.UserList[i].username;
                cboPhysician.Items.Add(tempItem);
                cboPhysician.Items[i + 1].Selected = false;
                cboPhysician.Items[i + 1].Value = PhyList[i].Id.ToString();
                if (ClientSession.PhysicianId == PhyList[i].Id && ClientSession.UserRole != "Medical Assistant")
                {
                    //cboPhysician.SelectedItem.Text = cboPhysician.Items[i].Text;
                    cboPhysician.Items[i + 1].Selected = true;
                    OpenOrdersScreen();
                }
                //GitLab # 2364 - Enabled Menu level Order for Ancillary FO
                NotInList = false;
            }
            return NotInList;
        }
        public string FillPatientSummaryBarforPatientChart(string LastName, string FirstName, string MI, string Suffix, DateTime DOB, ulong ulHumanID, string MedRecNo, string HomePhoneNo, string CellPhoneNo, string Sex, string PatientStatus, string SSN, string sPriPlan, string sPriCarrier, string sSecPlan, string sSecCarrier, string PatientType)
        {

            string sMySummary;
            string phoneno = "";
            if(HomePhoneNo.Length == 14)
            {
                phoneno = HomePhoneNo;
            }
            else
            {
                phoneno = CellPhoneNo;
            }

            string sSex = string.Empty;
            if (Sex != null && Sex != string.Empty)
                sSex = Sex.Substring(0, 1);

            if (PatientStatus == "DECEASED")
            {
                sMySummary = LastName + "," + FirstName +
                   "  " + MI + "  " + Suffix + "   |   " +
                   DOB.ToString("dd-MMM-yyyy") + "   |   " +
                   (CalculateAge(DOB)).ToString() +
                   "  year(s)    |   " + sSex + "   |   " + PatientStatus + "   |   Acc #:" + ulHumanID +
                   "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
                   "Phone #:" + phoneno + "   |   ";
            }
            else
            {
                sMySummary = LastName + "," + FirstName +
               "  " + MI + "  " + Suffix + "   |   " +
               DOB.ToString("dd-MMM-yyyy") + "   |   " +
               (CalculateAge(DOB)).ToString() +
               "  year(s)    |   " + sSex + "   |   Acc #:" + ulHumanID +
               "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
               "Phone #:" + phoneno + "   |   ";

            }
            if (PatientType != string.Empty)
            {
                sMySummary += "Patient Type:" + PatientType + "   |   ";
            }
            if (sPriPlan != string.Empty)
            {
                sMySummary += "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "   |   ";
            }
            if (sSecPlan != string.Empty)
            {
                sMySummary += "Sec Plan:" + sSecCarrier + " - " + sSecPlan + "   |   ";
            }
            //if (SSN != string.Empty)
            //{
            //    sMySummary += "SSN:" + SSN + "   |   ";
            //}

            return sMySummary;
        }
        protected void btnOrder_Click(object sender, EventArgs e)
        {
            bool SubmitOrNot;
            string MedicalAssistance = string.Empty;
            if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole.ToUpper() == "SCRIBE")
            {
                MedicalAssistance = "UNKNOWN";
            }
            IList<string> lstOrderType = new List<string>();
            OrdersManager objOrdersManager = new OrdersManager();
            SubmitOrNot = objOrdersManager.SubmitOrdersWithOutEncounter(HumanID, 0, string.Empty, string.Empty, ClientSession.UserName, DateTime.Now, MedicalAssistance, lstOrderType.ToArray<string>(), ClientSession.FacilityName);
            if (SubmitOrNot)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("8507001", this.Text);
            }
            else
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("8507002", this.Text);
            }
        }

        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            //FillPhysicianUser PhyUserList;
            string sPrevSelected = cboPhysician.SelectedValue;
            cboPhysician.Items.Clear();
            cboPhysician.Items.Add(new RadComboBoxItem(""));
            cboPhysician.Items[0].Value = "0";
            //PhysicianID = 0;
            PhysicianManager objPhysicianManager = new PhysicianManager();
            if (chkShowAll.Checked == true)
            {

                StaticLookupManager objStaticLookupManager = new StaticLookupManager();
                IList<PhysicianLibrary> templist = objPhysicianManager.GetPhysicianandUser(true, objStaticLookupManager.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.ToUpper(), ClientSession.LegalOrg).PhyList;
                //PhyUserList = objPhysicianManager.GetPhysicianandUser(false, string.Empty);
                IList<PhysicianLibrary> PhyList = UtilityManager.GetPhysicianList(string.Empty, ClientSession.LegalOrg);               
                IList<string> tempPhyList = templist.Where(a => a.Category.ToUpper() == "MACHINE").Select(a => a.PhyPrefix + " " + a.PhyFirstName + " " + a.PhyMiddleName + " " + a.PhyLastName + " " + a.PhySuffix).ToList<string>();
                //CAP-1897
                string strXmlFilePathTech = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\machine_technician.xml");
                XmlDocument xmldoc = new XmlDocument();
                if (File.Exists(strXmlFilePathTech))
                {
                    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "machine_technician" + ".xml");
                }
                int j = 1;
                for (int i = 0; i < PhyList.Count; i++)
                {
                    string sPhyName = string.Empty;
                    if (PhyList[i].PhyColor != "" && PhyList[i].PhyColor != "0" && File.Exists(strXmlFilePathTech))
                    {
                        XmlNodeList xmlTec = xmldoc.GetElementsByTagName("MachineTechnician" + PhyList[i].PhyColor);
                        if (xmlTec != null && xmlTec[0] != null)
                        {
                            sPhyName = xmlTec[0].Attributes.GetNamedItem("machine_name").Value + " - " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyLastName;
                        }
                    }
                    else
                    {
                        //Old Code
                        //string sPhyName = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                        //Gitlab# 2485 - Physician Name Display Change
                        if (PhyList[i].PhyLastName != String.Empty)
                            sPhyName += PhyList[i].PhyLastName;
                        if (PhyList[i].PhyFirstName != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + PhyList[i].PhyFirstName;
                            else
                                sPhyName += PhyList[i].PhyFirstName;
                        }
                        if (PhyList[i].PhyMiddleName != String.Empty)
                            sPhyName += " " + PhyList[i].PhyMiddleName;
                        if (PhyList[i].PhySuffix != String.Empty)
                            sPhyName += "," + PhyList[i].PhySuffix;
                    }

                    if (!tempPhyList.Contains(sPhyName))
                    {
                        //cboPhysician.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyList[i].Category.ToString() + " - " + sPhyName));
                        //Old Code
                        //cboPhysician.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyList[i].PhyUserName.ToString() + " - " + sPhyName));
                        //Gitlab# 2485 - Physician Name Display Change
                        cboPhysician.Items.Add(new Telerik.Web.UI.RadComboBoxItem(sPhyName));
                        cboPhysician.Items[j].Value = PhyList[i].Id.ToString();

                        //if (Convert.ToUInt64(cboPhysician.Items[j].Value) == PhysicianID)
                        //{
                        //    cboPhysician.SelectedIndex = j;
                        //}
                        j = j + 1;
                    }
                }
            }
            else
            {
                //PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName);
                IList<PhysicianLibrary> PhyList = GetPhysicianList(ClientSession.FacilityName);

                for (int i = 0; i < PhyList.Count; i++)
                {
                    //Old Code
                    //string sPhyName = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                    //Gitlab# 2485 - Physician Name Display Change
                 
                    string sPhyName = string.Empty;
                    if (PhyList[i].PhyLastName != String.Empty)
                        sPhyName += PhyList[i].PhyLastName;
                    if (PhyList[i].PhyFirstName != String.Empty)
                    {
                        if (sPhyName != String.Empty)
                            sPhyName += "," + PhyList[i].PhyFirstName;
                        else
                            sPhyName += PhyList[i].PhyFirstName;
                    }
                    if (PhyList[i].PhyMiddleName != String.Empty)
                        sPhyName += " " + PhyList[i].PhyMiddleName;
                    if (PhyList[i].PhySuffix != String.Empty)
                        sPhyName += "," + PhyList[i].PhySuffix;

                    //Old Code
                    //cboPhysician.Items.Add(new Telerik.Web.UI.RadComboBoxItem(PhyList[i].Category.ToString() + " - " + sPhyName));
                    //Gitlab# 2485 - Physician Name Display Change
                    cboPhysician.Items.Add(new Telerik.Web.UI.RadComboBoxItem(sPhyName));
                    cboPhysician.Items[i+1].Value = PhyList[i].Id.ToString();

                    //if (Convert.ToUInt64(cboPhysician.Items[i+1].Value) == PhysicianID)
                    //{
                    //    cboPhysician.SelectedIndex = i;
                    //}
                }
            }
            if (sPrevSelected != string.Empty && sPrevSelected != "0" && cboPhysician.Items.FindItemByValue(sPrevSelected)!=null)
                cboPhysician.Items.FindItemByValue(sPrevSelected).Selected = true;
        }

        protected void btnFindPhysician_Click(object sender, EventArgs e)
        {
            if (hdnTransferVaraible.Value != null && hdnTransferVaraible.Value.ToString() != string.Empty)
            {               
                string[] PassedObjects = hdnTransferVaraible.Value.ToString().Split('$');
                string sPhyName = PassedObjects[0].Split('=')[1];
                string sPhySpecialty = PassedObjects[1].Split('=')[1];
                string sPhyFacility = PassedObjects[2].Split('=')[1];
                ulong ulPhyId = Convert.ToUInt32(PassedObjects[3].Split('=')[1]);
                if (sPhyName != string.Empty)
                {
                    if (ulPhyId != 0 && !cboPhysician.Items.Select(a => a.Value).Any(a => a == ulPhyId.ToString()))
                    {
                        RadComboBoxItem objFindPhysician = new RadComboBoxItem();
                        objFindPhysician.Text = sPhyName;
                        objFindPhysician.Value = ulPhyId.ToString();
                        cboPhysician.Items.Add(objFindPhysician);
                        cboPhysician.SelectedIndex = cboPhysician.Items.FindItemIndexByText(sPhyName);
                        OpenOrdersScreen();
                    }
                    else
                    {
                        cboPhysician.SelectedIndex = cboPhysician.Items.Where(a => a.Value == ulPhyId.ToString()).Select(a => a.Index).FirstOrDefault();
                        OpenOrdersScreen();
                    }
                }                
            }
        }

        protected void cboPhysician_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OpenOrdersScreen();
            if ((cboPhysician.SelectedItem != null) && (cboPhysician.SelectedIndex != 0))
            {
                //if (cboPhysician.Items.FindItemByText("")!=null)
                //    cboPhysician.Items.Remove(cboPhysician.Items.FindItemByText(""));
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        void OpenOrdersScreen()
        {
            if ((cboPhysician.SelectedItem != null) && (cboPhysician.SelectedIndex != 0))
            {
                PhysicianID = Convert.ToUInt16(((RadComboBoxItem)cboPhysician.SelectedItem).Value);
                //iframeOrderPatientBar.Attributes["src"] = "frmOrders.aspx?HumanID=" + HumanID.ToString() + "&EncounterID=0&PhysicianID=" + Convert.ToUInt16(((RadComboBoxItem)cboPhysician.SelectedItem).Value) + "&ScreenMode=Menu";
                iframeOrderPatientBar.Attributes["src"] = "HtmlOrdersTab.html?HumanID=" + HumanID.ToString() + "&EncounterID=0&PhysicianID=" + Convert.ToUInt16(((RadComboBoxItem)cboPhysician.SelectedItem).Value) + "&ScreenMode=Menu";
                
            }
            else
            {
                iframeOrderPatientBar.Attributes.Remove("src");
            }
        }
        //protected void btnReset_Click(object sender, EventArgs e)
        //{
        //}

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script> self.close();</script>");
        }
    }
}
