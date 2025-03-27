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
using System.Xml;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.IO;
using Acurus.Capella.Core.DTOJson;
using Newtonsoft.Json;

namespace Acurus.Capella.UI
{
    public partial class frmAddPhysician : System.Web.UI.Page
    {
        private ulong _ulPhyid = 0;
        int iCount = 0;
        private string _sPhyLastName = string.Empty;
        private string _sPhyFirstName = string.Empty;
        private string _sPhyNPI = string.Empty;
        private string _sPhySpecialty = string.Empty;
        private string _sPhyFacility = string.Empty;
        private string _sPhyAddress = string.Empty;
        private string _sPhyPhone = string.Empty;
        private string _sPhyFax = string.Empty;
        private string _sPhyCity = string.Empty;
        private string _sPhyState = string.Empty;
        private string _sPhyZip = string.Empty;
        private ulong _ulPhySplID = 0;
        PhysicianManager PhyMngr = new PhysicianManager();
        PhysicianSpecialtyManager PhySpcMng = new PhysicianSpecialtyManager();
        FacilityManager FacilityMngr = new FacilityManager();
        FindPhysican objFindPhy = new FindPhysican();
        StateManager StateMngr = new StateManager();
        MapFacilityPhysicianManager MapFacPhyMangr = new MapFacilityPhysicianManager();
        IList<FacilityLibrary> FacilityList = null;
        StaticLookupManager staticLookUpMngr = new StaticLookupManager();
        PhysicianLibrary objPhyLib = new PhysicianLibrary();
        IList<PhysicianLibrary> SavePhyList = new List<PhysicianLibrary>();
        IList<PhysicianLibrary> SavedPhyList = new List<PhysicianLibrary>();
        PhysicianSpecialty objPhysSpec = new PhysicianSpecialty();
        IList<PhysicianSpecialty> PhySpecSaveList = new List<PhysicianSpecialty>();
        IList<MapFacilityPhysician> MapFacPhysList = new List<MapFacilityPhysician>();
        MapFacilityPhysician objMapFacPhy = new MapFacilityPhysician();
        ulong ulMyLookupPhyID = 0;
        public virtual ulong ulPhyId
        {
            get { return _ulPhyid; }
            set { _ulPhyid = value; }
        }

        public virtual string sPhyLastName
        {
            get { return _sPhyLastName; }
            set { _sPhyLastName = value; }
        }
        public virtual string sPhyFirstName
        {
            get { return _sPhyFirstName; }
            set { _sPhyFirstName = value; }
        }
        public virtual string sPhyNPI
        {
            get { return _sPhyNPI; }
            set { _sPhyNPI = value; }
        }
        public virtual string sPhySpecialty
        {
            get { return _sPhySpecialty; }
            set { _sPhySpecialty = value; }
        }
        public virtual string sPhyFacility
        {
            get { return _sPhyFacility; }
            set { _sPhyFacility = value; }
        }

        public virtual ulong ulPhySplID
        {
            get { return _ulPhySplID; }
            set { _ulPhySplID = value; }
        }

        public virtual string sPhyAddress
        {
            get { return _sPhyAddress; }
            set { _sPhyAddress = value; }
        }
        public virtual string sPhyPhone
        {
            get { return _sPhyPhone; }
            set { _sPhyPhone = value; }
        }
        public virtual string sPhyFax
        {
            get { return _sPhyFax; }
            set { _sPhyFax = value; }
        }
        public virtual string sPhyCity
        {
            get { return _sPhyCity; }
            set { _sPhyCity = value; }
        }
        public virtual string sPhyState
        {
            get { return _sPhyState; }
            set { _sPhyState = value; }
        }
        public virtual string sPhyZip
        {
            get { return _sPhyZip; }
            set { _sPhyZip = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Title = "Add Physician - " + ClientSession.UserName;
            if (!IsPostBack)
            {
                if (Request["LastName"] != null)
                {
                    sPhyLastName = Request["LastName"];
                }
                if (Request["FirstName"] != null)
                {
                    sPhyFirstName = Request["FirstName"];
                }
                if (Request["NPI"] != null)
                {
                    sPhyNPI = Request["NPI"];
                }
                if (Request["Speciality"] != null)
                {
                    sPhySpecialty = Request["Speciality"];
                }
                if (Request["Facility"] != null)
                {
                    sPhyFacility = Request["Facility"];
                }
                if (Request["PhysicianAddress"] != null)
                {
                    sPhyAddress = Request["PhysicianAddress"];
                }
                if (Request["PhyCity"] != null)
                {
                    sPhyCity = Request["PhyCity"];
                }
                if (Request["PhyState"] != null)
                {
                    sPhyState = Request["PhyState"];
                }
                if (Request["PhyZip"] != null)
                {
                    sPhyZip = Request["PhyZip"];
                }
                IList<FacilityLibrary> facList;
                facList = ApplicationObject.facilityLibraryList;//Codereview FacilityMngr.GetFacilityList();
                ddlFacility.Items.Add("");
                if (facList != null)
                {
                    for (int i = 0; i < facList.Count; i++)
                    {
                        ListItem cboItem = new ListItem();
                        cboItem.Text = facList[i].Fac_Name;

                        this.ddlFacility.Items.Add(cboItem);
                    }
                }
                //IList<StaticLookup> StaticLst = null;
                //ddlSpecialty.Items.Add("");
                //StaticLst = staticLookUpMngr.getStaticLookupByFieldName("SPECIALTY", "Sort_Order");
                //if (StaticLst != null)
                //{
                //    foreach (var i in StaticLst)
                //    {
                //        ddlSpecialty.Items.Add(i.Value);

                //    }
                //}
                if (ClientSession.UserRole.ToString().ToUpper() != "OFFICE MANAGER")
                    ddlFacility.Enabled = false;
                //Jira CAP-2786
                //XmlDocument xmldoc = new XmlDocument();

                //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Speciality.xml");
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Speciality" + ".xml");
                //    XmlNodeList xmlSpecialityList = xmldoc.GetElementsByTagName("speciality");
                //    ddlSpecialty.Items.Add(new ListItem("", "0"));
                //    if (xmlSpecialityList.Count > 0)
                //    {
                //        foreach (XmlNode item in xmlSpecialityList)
                //        {
                //            if (item != null)
                //                ddlSpecialty.Items.Add(new ListItem(item.Attributes[0].Value));
                //        }
                //    }
                //}

                //Jira CAP-2786
                SpecialityList specialityList = new SpecialityList();
                specialityList = ConfigureBase<SpecialityList>.ReadJson("Speciality.json");

                if (specialityList != null && specialityList.speciality != null)
                {
                    ddlSpecialty.Items.Add(new ListItem("", "0"));
                    foreach (Speciality speciality in specialityList.speciality)
                    {
                        ddlSpecialty.Items.Add(new ListItem(speciality.name));
                    }
                }

                if (sPhyLastName != string.Empty)
                {
                    txtLastName.Text = sPhyLastName;
                }
                if (sPhyFirstName != string.Empty)
                {
                    txtFirstName.Text = sPhyFirstName;
                }
                if (sPhySpecialty != string.Empty)
                {
                    ddlSpecialty.SelectedItem.Text = sPhySpecialty;
                }
                if (sPhyFacility != string.Empty)
                {
                    ddlFacility.SelectedItem.Text = sPhyFacility;
                }
                if (sPhyNPI != string.Empty)
                {
                    txtNPI.Text = sPhyNPI;
                }
                if (sPhyAddress != string.Empty)
                {
                    txtAddress.Text = sPhyAddress;
                }
                if (sPhyCity != string.Empty)
                {
                    txtCity.Text = sPhyCity;
                }
                if (sPhyState != string.Empty)
                {
                    txtState.Text = sPhyState;
                }
                if (sPhyZip != string.Empty)
                {
                    msktxtZip.Text = sPhyZip;
                }
            }

        }

        protected void btnAddToLibrary_Click(object sender, EventArgs e)
        {
            #region Validation
            if (txtLastName.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Physician", "DisplayErrorMessage('172500');", true);
                return;
            }
            if (txtFirstName.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Physician", "DisplayErrorMessage('172501');", true);
                return;
            }
            if (ddlSpecialty.SelectedItem.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Add Physician", "DisplayErrorMessage('172502');", true);
                return;
            }
            #endregion

            #region DB Insert
            objPhyLib.PhyLastName = txtLastName.Text;
            objPhyLib.PhyFirstName = txtFirstName.Text;
            objPhyLib.PhyAddress1 = ddlFacility.SelectedItem.Text;
            objPhyLib.PhyNPI = txtNPI.Text;
            objPhyLib.PhyState = txtState.Text;
            objPhyLib.PhyCity = txtCity.Text;
            objPhyLib.PhyAddress2 = txtAddress.Text;
            if (msktxtFaxNumber.TextWithLiterals != "() -")
            {
                objPhyLib.PhyFax = msktxtFaxNumber.TextWithLiterals;
            }

            if (msktxtPhoneNum.TextWithLiterals != "() -")
            {
                objPhyLib.PhyTelephone = msktxtPhoneNum.TextWithLiterals;
            }
            if (msktxtZip.TextWithLiterals != "() -")
            {
                objPhyLib.PhyZip = msktxtZip.TextWithLiterals;
            }

            objPhyLib.CreatedDateAndTime = UtilityManager.ConvertToUniversal();
            objPhyLib.Sort_Order = 9999;
            objPhyLib.Category = "PROVIDER";

            SavePhyList.Add(objPhyLib);

            SavedPhyList = PhyMngr.SaveUpdateDeletePhysicianLibrary(SavePhyList, null, null, string.Empty);
            if (SavedPhyList.Count > 0)
            {
                objPhysSpec.Physician_ID = SavedPhyList[0].Id;
                objPhysSpec.Specialty = ddlSpecialty.SelectedItem.Text;
                objPhysSpec.Created_By = ClientSession.UserName;
                objPhysSpec.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objPhysSpec.Sort_Order = 9999;
                PhySpecSaveList.Add(objPhysSpec);
                PhySpcMng.SaveUpdateDeleteWithTransaction(ref PhySpecSaveList, null, null, string.Empty);
                hdnPhyID.Value = objPhysSpec.Physician_ID.ToString();
            }
            if (SavedPhyList.Count > 0 && ddlFacility.SelectedItem.Text != string.Empty)
            {
                objMapFacPhy.Facility_Name = ddlFacility.SelectedItem.Text;
                objMapFacPhy.Phy_Rec_ID = SavedPhyList[0].Id;
                objMapFacPhy.Status = "Y";
                objMapFacPhy.Sort_Order = 9999;
                MapFacPhysList.Add(objMapFacPhy);
                MapFacPhyMangr.SaveUpdateDeleteWithTransaction(ref MapFacPhysList, null, null, string.Empty);
                hdnPhyID.Value = SavedPhyList[0].Id.ToString();
            }
            #endregion

            #region XML Insert
            iCount = 0;
            PhysicianLibrary objPhy = SavedPhyList.Count > 0 ? SavedPhyList[0] : null;
            PhysicianSpecialty objPhySpec = PhySpecSaveList.Count > 0 ? PhySpecSaveList[0] : null;
            MapFacilityPhysician objPhyFac = MapFacPhysList.Count > 0 ? MapFacPhysList[0] : null;
            InsertPhysicianIntoXMLs(objPhy, objPhySpec, objPhyFac);
            #endregion
            hdnAddPhysician.Value = "Yes";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Add Physician", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}CloseAddPhysician();", true);

        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            ddlSpecialty.SelectedIndex = 0;
            ddlFacility.SelectedIndex = 0;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtNPI.Text = string.Empty;
            txtState.Text = string.Empty;
            msktxtZip.Text = string.Empty;
            msktxtFaxNumber.Text = string.Empty;
            msktxtPhoneNum.Text = string.Empty;
            hdnAddPhysician.Value = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Add Physician", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        void InsertPhysicianIntoXMLs(PhysicianLibrary objPhy, PhysicianSpecialty objPhySpec, MapFacilityPhysician objPhyFac)
        {
            if (objPhy != null)
            {
                #region Save into PhysicianAddressDetails.xml
                //string strPath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                //if (File.Exists(strPath1))
                //{
                //    try
                //    {
                //        XmlDocument itemDoc = new XmlDocument();
                //        XmlTextReader XmlText = new XmlTextReader(strPath1);
                //        itemDoc.Load(XmlText);
                //        XmlText.Close();
                //        XmlNode PhysicianAddressNode = itemDoc.SelectSingleNode("/PhysicianAddress");

                //        if (PhysicianAddressNode != null)
                //        {
                //            XmlNode Newnode = null;
                //            Newnode = itemDoc.CreateNode(XmlNodeType.Element, "p" + objPhy.Id.ToString(), "");
                //            XmlAttribute attlabel = null;

                //            attlabel = itemDoc.CreateAttribute("Physician_Address1");
                //            attlabel.Value = objPhy.PhyAddress1;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Address2");
                //            attlabel.Value = objPhy.PhyAddress2;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_City");
                //            attlabel.Value = objPhy.PhyCity;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_State");
                //            attlabel.Value = objPhy.PhyState;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Zip");
                //            attlabel.Value = objPhy.PhyZip;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Telephone");
                //            attlabel.Value = objPhy.PhyTelephone;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Fax");
                //            attlabel.Value = objPhy.PhyFax;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Specialties");
                //            attlabel.Value = objPhySpec != null ? objPhySpec.Specialty : "";
                //            Newnode.Attributes.Append(attlabel);

                //            //attlabel = itemDoc.CreateAttribute("NPI");//Commented for bugID: 48345
                //            attlabel = itemDoc.CreateAttribute("Physician_NPI");
                //            attlabel.Value = objPhy.PhyNPI;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_EMail");
                //            attlabel.Value = objPhy.PhyEMail;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_prefix");
                //            attlabel.Value = objPhy.PhyPrefix;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_First_Name");
                //            attlabel.Value = objPhy.PhyFirstName;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Last_Name");
                //            attlabel.Value = objPhy.PhyLastName;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Type");
                //            attlabel.Value = objPhy.PhyType;
                //            Newnode.Attributes.Append(attlabel);

                //            attlabel = itemDoc.CreateAttribute("Physician_Library_ID");
                //            attlabel.Value = objPhy.Id.ToString();
                //            Newnode.Attributes.Append(attlabel);

                //            PhysicianAddressNode.AppendChild(Newnode);
                //            itemDoc.Save(strPath1);
                //        }
                //    }
                //    catch 
                //    {
                //        iCount = iCount + 1;
                //        if (iCount <= 8)
                //        {
                //            InsertPhysicianIntoXMLs(objPhy, objPhySpec, objPhyFac);
                //        }
                //    }
                //}
                //CAP-2780
                PhysicianAddressDetailsList addressList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json"); ;

                if (addressList != null)
                {
                    var newAddress = new PhysicianAddress
                    {
                        Physician_Address1 = objPhy.PhyAddress1,
                        Physician_Address2 = objPhy.PhyAddress2,
                        Physician_City = objPhy.PhyCity,
                        Physician_State = objPhy.PhyState,
                        Physician_Zip = objPhy.PhyZip,
                        Physician_Telephone = objPhy.PhyTelephone,
                        Physician_Fax = objPhy.PhyFax,
                        Specialties = objPhySpec != null ? objPhySpec.Specialty : "",
                        Physician_NPI = objPhy.PhyNPI,
                        Physician_EMail = objPhy.PhyEMail,
                        Physician_prefix = objPhy.PhyPrefix,
                        Physician_First_Name = objPhy.PhyFirstName,
                        Physician_Last_Name = objPhy.PhyLastName,
                        Physician_Type = objPhy.PhyType,
                        Physician_Library_ID = objPhy.Id.ToString()
                    };

                    addressList.PhysicianAddress.Add(newAddress);

                    var updatedAddressContent = JsonConvert.SerializeObject(addressList);
                    //Cap - 3103
                    //ConfigureBase<PhysicianAddressDetailsList>.SaveJson("PhysicianAddressDetails.json", updatedAddressContent);
                }
                #endregion
                #region Save into PhysicianFacilityMapping.xml
                //CAP-2781 - Need to confirm
                //string strPath2 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
                //if (File.Exists(strPath2))
                //{
                //    try
                //    {
                //        XmlDocument itemDoc = new XmlDocument();
                //        XmlTextReader XmlText = new XmlTextReader(strPath2);
                //        itemDoc.Load(XmlText);
                //        XmlText.Close();
                //        if (objPhyFac != null)
                //        {
                //            XmlNode PhysicianFacilityNode = itemDoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + objPhyFac.Facility_Name.ToUpper().Trim() + "']");

                //            if (PhysicianFacilityNode != null)
                //            {
                //                XmlNode Newnode = null;
                //                Newnode = itemDoc.CreateNode(XmlNodeType.Element, "Physician", "");
                //                XmlAttribute attlabel = null;

                //                attlabel = itemDoc.CreateAttribute("prefix");
                //                attlabel.Value = objPhy.PhyPrefix;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("firstname");
                //                attlabel.Value = objPhy.PhyFirstName;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("middlename");
                //                attlabel.Value = objPhy.PhyMiddleName;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("lastname");
                //                attlabel.Value = objPhy.PhyLastName;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("suffix");
                //                attlabel.Value = objPhy.PhySuffix;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("username");
                //                attlabel.Value = "";
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("ID");
                //                attlabel.Value = objPhy.Id.ToString();
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("status");
                //                attlabel.Value = "Y";
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("npi");
                //                attlabel.Value = objPhy.PhyNPI;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("machine_technician_id");
                //                attlabel.Value = "0";
                //                Newnode.Attributes.Append(attlabel);

                //                PhysicianFacilityNode.AppendChild(Newnode);
                //                itemDoc.Save(strPath2);
                //            }
                //        }
                //        else
                //        {
                //            XmlNode UnMappedPhyListNode = itemDoc.SelectSingleNode("/ROOT/UnMappedPhyList");

                //            if (UnMappedPhyListNode != null)
                //            {
                //                XmlNode Newnode = null;
                //                Newnode = itemDoc.CreateNode(XmlNodeType.Element, "Physician", "");
                //                XmlAttribute attlabel = null;

                //                attlabel = itemDoc.CreateAttribute("prefix");
                //                attlabel.Value = objPhy.PhyPrefix;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("firstname");
                //                attlabel.Value = objPhy.PhyFirstName;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("middlename");
                //                attlabel.Value = objPhy.PhyMiddleName;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("lastname");
                //                attlabel.Value = objPhy.PhyLastName;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("suffix");
                //                attlabel.Value = objPhy.PhySuffix;
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("username");
                //                attlabel.Value = "";
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("ID");
                //                attlabel.Value = objPhy.Id.ToString();
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("status");
                //                attlabel.Value = "Y";
                //                Newnode.Attributes.Append(attlabel);

                //                attlabel = itemDoc.CreateAttribute("npi");
                //                attlabel.Value = objPhy.PhyNPI;
                //                Newnode.Attributes.Append(attlabel);

                //                UnMappedPhyListNode.AppendChild(Newnode);
                //                itemDoc.Save(strPath2);
                //            }
                //        }
                //    }
                //    catch
                //    {
                //        iCount = iCount + 1;
                //        if (iCount <= 8)
                //        {
                //            InsertPhysicianIntoXMLs(objPhy, objPhySpec, objPhyFac);
                //        }
                //    }
                //}
                //CAP-2781 -- Need Confirmation
                var physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json"); ;

                if (physicianFacilityMappingList != null)
                {
                    if (objPhyFac != null)
                    {
                        var physicianFacility = physicianFacilityMappingList.PhysicianFacility.FirstOrDefault(x => x.name == objPhyFac.Facility_Name.ToUpper().Trim());
                        if (physicianFacility != null)
                        {
                            Core.DTOJson.PhysicianList physician = new Core.DTOJson.PhysicianList();
                            physician.prefix = objPhy.PhyPrefix;
                            physician.firstname = objPhy.PhyFirstName;
                            physician.middlename = objPhy.PhyMiddleName;
                            physician.lastname = objPhy.PhyLastName;
                            physician.suffix = objPhy.PhySuffix;
                            physician.username = " ";
                            physician.ID = objPhy.Id.ToString();
                            physician.status = "Y";
                            physician.npi = objPhy.PhyNPI;
                            physician.machine_technician_id = "0";

                            physicianFacilityMappingList.PhysicianFacility.FirstOrDefault(x => x.name == objPhyFac.Facility_Name.ToUpper().Trim()).Physician.Add(physician);

                            var updatedFacilityMappingContent = JsonConvert.SerializeObject(physicianFacilityMappingList);
                            //Cap - 3094
                            //ConfigureBase<PhysicianFacility>.SaveJson("PhysicianFacilityMapping.json", updatedFacilityMappingContent);
                        }
                    }
                }
                else
                {

                    UnmappedPhysician unmappedPhysician = new UnmappedPhysician();
                    unmappedPhysician.prefix = objPhy.PhyPrefix;
                    unmappedPhysician.firstname = objPhy.PhyFirstName;
                    unmappedPhysician.middlename = objPhy.PhyMiddleName;
                    unmappedPhysician.lastname = objPhy.PhyLastName;
                    unmappedPhysician.suffix = objPhy.PhySuffix;
                    unmappedPhysician.username = objPhy.PhyUserName;
                    unmappedPhysician.ID = objPhy.PhyId.ToString();
                    unmappedPhysician.status = "Y";
                    unmappedPhysician.npi = objPhy.PhyNPI;

                    physicianFacilityMappingList.UnmappedPhysician.Add(unmappedPhysician);

                    var updatedFacilityMappingContent = JsonConvert.SerializeObject(physicianFacilityMappingList);
                    //Cap - 3094
                    //ConfigureBase<PhysicianFacility>.SaveJson("PhysicianFacilityMapping.json", updatedFacilityMappingContent);

                }
                #endregion
            }
        }
    }
}