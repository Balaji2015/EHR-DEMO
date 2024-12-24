using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmDMEOrder : System.Web.UI.Page
    {
        OrdersManager objOrdersManager = new OrdersManager();
        ArrayList errList = new ArrayList();
        IList<PhysicianProcedure> procedureList = new List<PhysicianProcedure>();
        EAndMCodingManager objEAndMCodingManager = new EAndMCodingManager();
        AssessmentManager objAssessmentManager = new AssessmentManager();
        public IList<Assessment> AssessmentList = new List<Assessment>();
        public IList<ProblemList> ilstProblemList = new List<ProblemList>();
        OrdersDTO objOrderDTO = new OrdersDTO();
        string OrderType = "DME ORDER";
        ulong EditOrdersSubmitID = 0;
        string sScreenMode = string.Empty;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            LookUpPerRequest = LookUpValues;
            txtOrderNotes.DName = "pbDropdown";
            if (!IsPostBack)
            {
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
                    {
                        EncounterID = 0;
                    }
                    else
                    {
                        EncounterID = ClientSession.EncounterId;
                    }
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
                if (ClientSession.UserRole.ToUpper() == "CODER")
                {
                    tblSelectProcedure.Disabled = true; //tblSelectProcedure.Enabled = false;
                }
                else
                {
                    chklstFrequentlyUsedProcedures.Attributes.Add("onclick", "SelectItemsUnderHeader(this);");
                }

                if (Request["ScreenMode"] != null && Request["ScreenMode"].Trim() != string.Empty)
                {
                    sScreenMode = Request["ScreenMode"].ToString();
                }
                if (sScreenMode == "MyQ")
                {
                    btnMoveToNextProcess.Visible = true;
                }
                else
                {
                    btnMoveToNextProcess.Visible = false;
                }
                if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"] != "0")
                {
                    Session["OrderSubmitId"] = Request["OrderSubmitId"].ToString();
                }
                if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() == "TECHNICIAN")
                {
                    MoveToMA.Visible = false; 
                }
                else
                {

                }
                hdnPhysicianID.Value = PhysicianID.ToString();
                objOrderDTO = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(), false,ClientSession.LegalOrg);
                Session["objDMEDTO"] = objOrderDTO;
                FillAssesmentAndProblemListICD(objOrderDTO.AssessmentList, objOrderDTO.MedAdvProblemList);
                LoadOrders(objOrderDTO);
               
                cboLab.Items.Clear();
                ListItem cboItem = new ListItem();
                cboItem.Text = " ";
                cboItem.Value = "0";
                cboLab.Items.Add(cboItem);

                //XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));
                //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                //   .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() == "DME")
                //   .OrderBy(s => (int)s.Attribute("sort_order"));
                //if (xml != null)
                //{
                //    foreach (XElement LabElement in xml)
                //    {
                //        string xmlValue = LabElement.Attribute("name").Value;
                //        string xmlLabId = LabElement.Attribute("id").Value;
                //        cboItem = new ListItem();
                //        cboItem.Text = xmlValue;
                //        cboItem.Value = xmlLabId;
                //        cboItem.Attributes.Add("title", xmlValue);
                //        cboLab.Items.Add(cboItem);
                //    }
                //}

                //CAP-2773
                Lablist objlablist = new Lablist();
                objlablist = ConfigureBase<Lablist>.ReadJson("LabList.json");
                List<Labs> listLabList = new List<Labs>();
                listLabList = objlablist.Lab.Where(a => a.type == "DME").OrderBy(s => (int)Convert.ToInt32(s.sort_order)).ToList();
                if (listLabList != null && listLabList.Count > 0)
                {
                    foreach (Labs objlab in listLabList)
                    {
                        string xmlValue = objlab.name;
                        string xmlLabId = objlab.id;
                        cboItem = new ListItem();
                        cboItem.Text = xmlValue;
                        cboItem.Value = xmlLabId;
                        cboItem.Attributes.Add("title", xmlValue);
                        cboLab.Items.Add(cboItem);
                    }
                }

                //if(objOrderDTO.objHuman.Lab_Id!="0")
                //{
                AssignDefaultValueToLab(Convert.ToInt32(objOrderDTO.objHuman.Lab_Id));
                    Session["cboLab"] = objOrderDTO.objHuman.Lab_Id;
                //}
               
                SecurityServiceUtility obj = new SecurityServiceUtility();
                obj.ApplyUserPermissions(this.Page);
                btnEditQuantity.Disabled = true;
                btnAdd.Disabled = true;
            }
            CheckMovetoMA(chkMoveToMA.Checked);
            txtOrderNotes.txtDLC.Attributes.Add("onChange","NotesTextChanged()");
            txtOrderNotes.txtDLC.Attributes.Add("OnKeyPress", "NotesGetKeyPress()");
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
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();};disableDMEAutoSave();", true);
        }
        public void CheckMovetoMA(Boolean isChecked)
        {
            if (isChecked)
            {
                if (ClientSession.UserRole == "Medical Assistant")
                {
                    if (!lblDatelastseen.InnerText.Contains("*"))//if (lblDatelastseen.InnerText != "Date last seen by Physician*")
                      lblDatelastseen.InnerText += "*";
                    //lblDatelastseen.Attributes.Add("style", "color:red");
                    lblDatelastseen.Attributes.Add("class", "MandLabelstyle");
                    lblDatelastseen.InnerHtml = lblDatelastseen.InnerText;
                    if (!lblDatelastseen.InnerHtml.Contains("<span class='manredforstar'>*</span>"))
                        lblDatelastseen.InnerHtml = lblDatelastseen.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");

                    if (!lblDurationofneedforDME.InnerText.Contains("*"))//if (lblDurationofneedforDME.InnerText != "Duration of need for DME*")
                        lblDurationofneedforDME.InnerText += "*";
                    //lblDurationofneedforDME.Attributes.Add("style", "color:red");
                    lblDurationofneedforDME.Attributes.Add("class", "MandLabelstyle");
                    lblDurationofneedforDME.InnerHtml = lblDurationofneedforDME.InnerText;
                    if (!lblDurationofneedforDME.InnerHtml.Contains("<span class='manredforstar'>*</span>"))
                        lblDurationofneedforDME.InnerHtml = lblDurationofneedforDME.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");

                    if (!lblDurationofneedforsupplies.InnerText.Contains("*"))//if (lblDurationofneedforsupplies.InnerText != "Duration of need for supplies*")
                        lblDurationofneedforsupplies.InnerText += "*";
                    lblDurationofneedforsupplies.Attributes.Add("class", "MandLabelstyle");
                    lblDurationofneedforsupplies.InnerHtml = lblDurationofneedforsupplies.InnerText;
                    if (!lblDurationofneedforsupplies.InnerHtml.Contains("<span class='manredforstar'>*</span>"))
                        lblDurationofneedforsupplies.InnerHtml = lblDurationofneedforsupplies.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                    //lblDurationofneedforsupplies.Attributes.Add("style", "color:red");
                }
                else
                {
                    //lblDatelastseen.InnerText = lblDatelastseen.InnerText.Replace("*", "");
                    lblDatelastseen.InnerText = lblDatelastseen.InnerText.Replace("<span class='manredforstar'>*</span>","");
                    lblDatelastseen.InnerText = lblDatelastseen.InnerText.Trim();
                    lblDatelastseen.Attributes.Add("style", "color:black");
                    lblDatelastseen.Attributes.Add("class", "");

                 
                    //lblDurationofneedforDME.InnerText = lblDurationofneedforDME.InnerText.Replace("*", "");
                    lblDurationofneedforDME.InnerText = lblDurationofneedforDME.InnerText.Replace("<span class='manredforstar'>*</span>","");
                    lblDurationofneedforDME.InnerText = lblDurationofneedforDME.InnerText.Trim();
                    lblDurationofneedforDME.Attributes.Add("style", "color:black");
                    lblDurationofneedforDME.Attributes.Add("class", "");
                    //lblDurationofneedforsupplies.InnerText = lblDurationofneedforsupplies.InnerText.Replace("*", "");
                    lblDurationofneedforsupplies.InnerText = lblDurationofneedforsupplies.InnerText.Replace("<span class='manredforstar'>*</span>","");
                    lblDurationofneedforsupplies.InnerText = lblDurationofneedforsupplies.InnerText.Trim();
                    lblDurationofneedforsupplies.Attributes.Add("style", "color:black");
                    lblDurationofneedforsupplies.Attributes.Add("class","");
                }
            }
            else
            {
                if (!lblDatelastseen.InnerText.Contains("*"))// != "Date last seen by Physician*")
                    lblDatelastseen.InnerText += "*";
                lblDatelastseen.Attributes.Add("class", "MandLabelstyle");
                lblDatelastseen.InnerHtml = lblDatelastseen.InnerText;
                if(!lblDatelastseen.InnerHtml.Contains("<span class='manredforstar'>*</span>"))
                    lblDatelastseen.InnerHtml = lblDatelastseen.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                //lblDatelastseen.Attributes.Add("style", "color:red");
                if (!lblDurationofneedforDME.InnerText.Contains("*"))// != "Duration of need for DME*")
                    lblDurationofneedforDME.InnerText += "*";
                //lblDurationofneedforDME.Attributes.Add("style", "color:red");
                lblDurationofneedforDME.Attributes.Add("class", "MandLabelstyle");
                lblDurationofneedforDME.InnerHtml = lblDurationofneedforDME.InnerText;
                if(!lblDurationofneedforDME.InnerHtml.Contains("<span class='manredforstar'>*</span>"))
                    lblDurationofneedforDME.InnerHtml = lblDurationofneedforDME.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
                if (!lblDurationofneedforsupplies.InnerText.Contains("*"))// != "Duration of need for supplies*")
                    lblDurationofneedforsupplies.InnerText += "*";
                //lblDurationofneedforsupplies.Attributes.Add("style", "color:red");
                lblDurationofneedforsupplies.Attributes.Add("class", "MandLabelstyle");
                lblDurationofneedforsupplies.InnerHtml = lblDurationofneedforsupplies.InnerText;
                if(!lblDurationofneedforsupplies.InnerHtml.Contains("<span class='manredforstar'>*</span>"))
                    lblDurationofneedforsupplies.InnerHtml = lblDurationofneedforsupplies.InnerHtml.Replace("*", "<span class='manredforstar'>*</span>");
            }
        }
        public void ClearAll(bool IsNewOrder)
        {
            chklstAssessment.ClearSelection();
            for (int i = 0; i < AssessmentSource.Count; i++)
            {
                chklstAssessment.Items[i].Selected = false;
            }
            if (IsNewOrder)
            {
                cboLab.SelectedIndex = 0;
                cboLab.Items[cboLab.SelectedIndex].Text = string.Empty;

                txtCenterName.Value = "";
                txtLocation.Value = "";
                chkSelectALLICD.Checked = false;
                txtOrderNotes.txtDLC.Text = "";
                txtDurationofneedforDME.Value = "";
                txtDurationofneedforsupplies.Value = "";
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
                gbProcedures.Enabled = true;
                cboLab.Disabled = false;
                chkMoveToMA.Checked = false;
                CheckMovetoMA(chkMoveToMA.Checked);
                dtpLastseenbyPhysician.Value = "";
                btnEditQuantity.Disabled = true;
                if (Session["cboLab"] != null)
                {
                    AssignDefaultValueToLab(Convert.ToInt32(Session["cboLab"]));
                }
                chklstAssessment.Enabled = true;
            }
            if (cboLab.Items[cboLab.SelectedIndex].Value.Trim() != string.Empty)
            {
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, "DME ORDER", Convert.ToUInt32(cboLab.Items[cboLab.SelectedIndex].Value), ClientSession.LegalOrg);
            }
            else
            {
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, "DME ORDER", Convert.ToUInt32(cboLab.Items[1].Value), ClientSession.LegalOrg);
            }
            FillLabProcedure(procedureList, new List<string>());
            btnClearAll.Value = "Clear All";
            btnAdd.Value = "Add";
            btnAdd.Disabled = true;
            OrdersDTO objOrderDTO = new OrdersDTO();
            if (Session["objDMEDTO"] != null)
                objOrderDTO = (OrdersDTO)Session["objDMEDTO"];
            LoadOrders(objOrderDTO);

        }
        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            ProcedureSwitch(false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            LookUpValues = LookUpPerRequest;
        }
        protected void cboLab_SelectedIndexChanged(object sender, EventArgs e)
        {
            LookUpPerRequest.Clear();
            ulong selectedLabID = 0;
            if (cboLab.Items[cboLab.SelectedIndex].Value != string.Empty)
                selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
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
            txtLocation.Style["background"] = "#BFDBFF";
            txtCenterName.Style["background"] = "#BFDBFF";
            txtCenterName.Value = cboLab.Items[cboLab.SelectedIndex].Text;
            SetReadOnlyStyle(txtLocation);
            SetReadOnlyStyle(txtCenterName);
            XDocument xmlFacility = XDocument.Load(Server.MapPath(@"ConfigXML\Facility_Library.xml"));

            IEnumerable<XElement> xmlFac = xmlFacility.Element("FacilityList")
                .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);
            //.OrderBy(s => (string)s.Attribute("City"));
            if (xmlFac != null && xmlFac.Count() > 0)
            {
                LookUpPerRequest.Add("FacilityCity", xmlFac.Attributes("City").First().Value.ToString());
            }

            //XDocument xmlLabLocation = XDocument.Load(Server.MapPath(@"ConfigXML\LabLocationList.xml"));

            //if (xmlLabLocation != null && LookUpPerRequest["FacilityCity"] != null)
            //{
            //    if (LookUpPerRequest["FacilityCity"] != string.Empty)
            //    {
            //        IEnumerable<XElement> xmlLocation = null;
            //        try
            //        {
            //            xmlLocation = xmlLabLocation.Element("LabLocationList")
            // .Elements("LabLocation").Where(xx => xx.Attribute("labid").Value == (cboLab.Items[cboLab.SelectedIndex].Value) && xx.Attribute("city").Value.ToUpper() == (LookUpPerRequest["FacilityCity"].ToUpper()));

            //        }

            //        catch (Exception)
            //        {
            //            xmlLocation = null;
            //        }
            //        if (xmlLocation != null && xmlLocation.Count() > 0)
            //        {
            //            txtLocation.Value = xmlLocation.Attributes("city").First().Value.ToString();
            //            if (!LookUpPerRequest.ContainsKey("labLocID"))
            //                LookUpPerRequest.Add("labLocID", xmlLocation.Attributes("id").First().Value.ToString());
            //            else
            //                LookUpPerRequest["labLocID"] = xmlLocation.Attributes("id").First().Value.ToString();                        
            //        }
            //    }
            //}

            //CAP-2774
            LabLocationList objlabLocation = new LabLocationList();
            objlabLocation = ConfigureBase<LabLocationList>.ReadJson("LabLocationList.json");
            if (objlabLocation != null && LookUpPerRequest["FacilityCity"] != null)
            {
                if (LookUpPerRequest["FacilityCity"] != string.Empty)
                {
                    List<LabLocations> listLabLocation = new List<LabLocations>();
                    try
                    {
                        listLabLocation = objlabLocation.LabLocation.Where(a => a.labid == cboLab.Items[cboLab.SelectedIndex].Value.ToString() && a.city.ToUpper() == LookUpPerRequest["FacilityCity"].ToUpper().ToString()).ToList();
                    }
                    catch (Exception)
                    {
                        listLabLocation = null;
                    }
                    if (listLabLocation != null && listLabLocation.Count() > 0)
                    {
                        txtLocation.Value = listLabLocation[0].city.ToString();
                        if (!LookUpPerRequest.ContainsKey("labLocID"))
                            LookUpPerRequest.Add("labLocID", listLabLocation[0].id.ToString());
                        else
                            LookUpPerRequest["labLocID"] = listLabLocation[0].id.ToString();
                    }
                }
            }



            procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, "DME PROCEDURE", selectedLabID, ClientSession.LegalOrg);
            FillLabProcedure(procedureList, new List<string>());
        }
        void SetReadOnlyStyle(HtmlInputText objRadTextBox)
        {
            objRadTextBox.Attributes.Add("readOnly", "true");
        }
        protected void btninvisibleDiagnosis_Click(object sender, EventArgs e)
        {
            btnAdd.Disabled = false;
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
                            itm = new ListItem();
                            itm.Text = sFinalValue;
                            itm.Selected = true;
                            chklstAssessment.Items.Add(itm);
                            AssessmentSource.Add(sFinalValue.Split('-')[0], "OTHERS-0");
                        }
                    }
                    Session["Selected_ICDs"] = null;
                   
                }
            }
            //divLoading.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
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
                var underthisgroup = (from rec in labProList where rec.Order_Group_Name.ToUpper() == str.ToUpper() select rec).ToList<PhysicianProcedure>();
                foreach (var phypro in underthisgroup)
                {
                    ItemsToBeAdded.Add(phypro.Physician_Procedure_Code + "-" + phypro.Procedure_Description);
                }

            }
            int ItemsPerRow = 10;
            decimal tempDouble = ((decimal)ItemsToBeAdded.Count / (decimal)ItemsPerRow);
            int coulumns = (int)(Math.Ceiling(tempDouble));
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
                    tempItem.Value = "HEADERROW";
                    HeaderText = tempItem.Text;
                    ListViewHeader.Add("IsHeader-true;RespectiveHeader-" + HeaderText);
                }
                else
                {
                    tempItem.Text = ItemsToBeAdded[i].ToString();
                    tempItem.Value = ItemsToBeAdded[i].ToString() + "~" + HeaderText;
                    tempItem.Attributes.Add("style", "color:black;");
                    for (int itemval = 0; itemval < lstChkeditem.Count; itemval++)
                    {
                        if (lstChkeditem[itemval].Split('-')[0].Contains(ItemsToBeAdded[i].Split('-')[0]))
                            tempItem.Selected = true;
                    }
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

            hdnListViewArray.Value = string.Join(",", maxstringLengthColumnVies.Select(a => a.ToString()).ToArray<string>());
            //for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            //{
            //    chklstFrequentlyUsedProcedures.Items[i].Attributes.Add("onClick", "EnableImportResults();");
            //}
        }
        protected void btnSelectLocation_Click(object sender, EventArgs e)
        {
            if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
            {
                ulong selectedLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenLabLocationScreenKey", "OpenLabLocationScreen(" + selectedLabID.ToString() + ",'" + cboLab.Items[cboLab.SelectedIndex].Text + "');", true);
                btnAdd.Disabled = false;
            }
            else
            {
                errList = new ArrayList();
                errList.Add(lblLab.InnerText.Replace('*', ' '));
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230106');", true);
                cboLab.Focus();
            }
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
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, "DME PROCEDURE", selectedLabID, ClientSession.LegalOrg);
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
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public bool CheckForValidation()
        {
            errList = new ArrayList();


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
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblCenterName.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtCenterName.Focus();
                return false;
            }
            else if (lblLocation.InnerText.Contains('*') && txtLocation.Value.Trim() == string.Empty)
            {
                errList.Add(lblLocation.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblLocation.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtLocation.Focus();
                return false;
            }
            else if (lblDatelastseen.InnerText.Contains('*') && dtpLastseenbyPhysician.Value == "")
            {
                errList.Add(lblDatelastseen.InnerText.Replace('*', ' ').Trim());
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230160','','" + lblDatelastseen.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230160','','" + "Date last seen by Physician" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                //dtpLastseenbyPhysician.Focus();
                return false;
            }
            else if (lblDurationofneedforDME.InnerText.Contains('*') && (txtDurationofneedforDME.Value == string.Empty || txtDurationofneedforDME.Value == "0"))
            {
                errList.Add(lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230161','','" + "Duration of need for DME" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtDurationofneedforDME.Focus();
                return false;
            }

            else if (!(chkMoveToMA.Checked) && txtCenterName.Value.Trim() == string.Empty)
            {
                errList.Add(lblCenterName.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblCenterName.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtCenterName.Focus();
                return false;
            }
            else if (lblLocation.InnerText.Contains('*') && txtLocation.Value.Trim() == string.Empty)
            {
                errList.Add(lblLocation.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230106','','" + lblLocation.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtLocation.Focus();
                return false;
            }
            else if (!(chkMoveToMA.Checked) && dtpLastseenbyPhysician.Value == "")
            {
                errList.Add(lblDatelastseen.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230160','','" + "Date last seen by Physician" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                //dtpLastseenbyPhysician.Focus();
                return false;
            }
            else if (!(chkMoveToMA.Checked) && (txtDurationofneedforDME.Value == string.Empty || txtDurationofneedforDME.Value == "0"))
            {
                errList.Add(lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230161','','" + "Duration of need for DME" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtDurationofneedforDME.Focus();
                return false;
            }
            else if ((txtDurationofneedforsupplies.Value.Trim() == string.Empty || txtDurationofneedforsupplies.Value.Trim() =="0") && lblDurationofneedforsupplies.InnerText.Contains("*"))
            {
                errList.Add(lblDurationofneedforsupplies.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230162','','" + "Duration of need for supplies" + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtDurationofneedforsupplies.Focus();
                return false;
            }
            else if ((!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected == true)))
            {
                errList.Clear();
                string errorMsg = string.Empty;
                if (OrderType.ToUpper() == "DME ORDER")
                {
                    errList.Add("a Lab Procedure");
                    errorMsg += " a Lab Procedure";
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

            return true;
        }
        protected void chkSelectALLICD_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chklstAssessment.Items.Count; i++)
            {
                chklstAssessment.Items[i].Selected = chkSelectALLICD.Checked;
            }
            if (chkSelectALLICD.Checked == true)
            {
                //btnOrderSubmit.Disabled = false; 
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        bool ValidateSave()
        {
            bool bReturnValue = false;
            if (chkMoveToMA.Checked)
            {
                if (ClientSession.UserRole != "Medical Assistant" && ClientSession.UserCurrentProcess != "MA_REVIEW")
                    bReturnValue = IsICDIsCheckedWithOutCPT();
                else
                    bReturnValue = CheckForValidation();
            }
            else
            {
                bReturnValue = CheckForValidation();
            }
            return bReturnValue;

        }
        bool IsICDIsCheckedWithOutCPT()
        {
            if ((!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected == true)) && (!chklstAssessment.Items.Cast<ListItem>().Any(a => a.Selected == true)))
            {
                errList.Clear();
                if (OrderType.ToUpper() == "DME ORDER")
                    errList.Add("a Lab Procedure");
                else if (OrderType.ToUpper() == "IMAGE ORDER")
                    errList.Add("an Image Procedure");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230104','','" + errList[0].ToString() + "');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return false;
            }
            else
                return true;
        }
        public void AddDMEOrder()
        {
            //XDocument xmlLabLocation = XDocument.Load(Server.MapPath(@"ConfigXML\LabLocationList.xml"));
            //string sLocation = string.Empty;
            //if (txtLocation.Value != "")
            //{
            //    if (txtLocation.Value != string.Empty)
            //    {
            //        IEnumerable<XElement> xmlLocation = null;
            //        try
            //        {
            //            xmlLocation = xmlLabLocation.Element("LabLocationList")
            // .Elements("LabLocation").Where(xx => xx.Attribute("labid").Value == (cboLab.Items[cboLab.SelectedIndex].Value) && xx.Attribute("city").Value.ToUpper() == (txtLocation.Value.ToUpper()));

            //        }

            //        catch (Exception)
            //        {
            //            xmlLocation = null;
            //        }
            //        if (xmlLocation != null && xmlLocation.Count() > 0)
            //        {
            //            txtLocation.Value = xmlLocation.Attributes("city").First().Value.ToString();
            //            sLocation = xmlLocation.Attributes("id").First().Value.ToString();
            //        }
            //    }
            //}

            //CAP-2774
            LabLocationList objlabLocation = new LabLocationList();
            objlabLocation = ConfigureBase<LabLocationList>.ReadJson("LabLocationList.json");
            string sLocation = string.Empty;
            if (txtLocation.Value != "" && txtLocation.Value != string.Empty)
            {
                List<LabLocations> listLabLocation = new List<LabLocations>();
                try
                {
                    listLabLocation = objlabLocation.LabLocation.Where(a => a.labid == cboLab.Items[cboLab.SelectedIndex].Value.ToString() && a.city.ToUpper() == (txtLocation.Value.ToUpper())).ToList();
                }
                catch (Exception)
                {
                    listLabLocation = null;
                }
                if (listLabLocation != null && listLabLocation.Count() > 0)
                {
                    txtLocation.Value = listLabLocation[0].city.ToString();
                    sLocation = listLabLocation[0].id.ToString();
                }
            }

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
            }
            foreach (string str in SelectedProcedure)
            {
                objOrder = new Orders();
                objOrder.Encounter_ID = EncounterID;
                objOrder.Physician_ID = PhysicianID;
                objOrder.Human_ID = HumanID;
                objOrder.Created_By = ClientSession.UserName;
                objOrder.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
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

            }
            OrdersAssessment objOrdAss;
            for (int i = 0; i < SelectedAssessment.Count; i++)
            {
                if (AssessmentSource.Count > 0)
                {
                    if (AssessmentSource.ContainsKey(SelectedAssessment[i].ToString().Split('-')[0]))
                    {
                        objOrdAss = CreateOrderAssObj(SelectedAssessment[i], 0, AssessmentSource[SelectedAssessment[i].ToString().Split('-')[0]]);
                        objOrdAss.Order_Submit_ID = (ulong)(SaveOrderList.Count - 1);
                        SaveOrdersAssList.Add(objOrdAss);
                    }
                }
            }
            var DistinctSubmit = (from rec in SaveOrderList select rec.Order_Code_Type);
            DistinctSubmit = DistinctSubmit.Distinct();
            string sLocalTime = string.Empty;
            foreach (string str in DistinctSubmit)
            {
                objOrderSubmit = new OrdersSubmit();
                objOrderSubmit.Order_Code_Type = str;
                objOrderSubmit.Created_By = ClientSession.UserName;
                objOrderSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objOrderSubmit.Encounter_ID = EncounterID;
                objOrderSubmit.Human_ID = HumanID;
                if (chkMoveToMA.Checked == true)
                {
                    objOrderSubmit.Move_To_MA = "Y";
                }
                else
                {
                    objOrderSubmit.Move_To_MA = "N";
                }
                objOrderSubmit.Physician_ID = PhysicianID;
                objOrderSubmit.Facility_Name = ClientSession.FacilityName;
                objOrderSubmit.Height = string.Empty;
                objOrderSubmit.Weight = string.Empty;
                objOrderSubmit.Temperature = ilstTemp.Where(a => a.Value == str).Select(a => a.Key).SingleOrDefault();
                if (objOrderSubmit.Temperature == null)
                    objOrderSubmit.Temperature = string.Empty;
                if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
                    objOrderSubmit.Lab_ID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                objOrderSubmit.Lab_Location_Name = txtLocation.Value;
                objOrderSubmit.Lab_Name = cboLab.Items[cboLab.SelectedIndex].Text; //txtCenterName.Value;
                if (sLocation != string.Empty)
                    objOrderSubmit.Lab_Location_ID = Convert.ToUInt64(sLocation);
                else
                    objOrderSubmit.Lab_Location_ID = Convert.ToUInt64(0);
                objOrderSubmit.Order_Notes = txtOrderNotes.txtDLC.Text;
                objOrderSubmit.Order_Type = OrderType;
                objOrderSubmit.TestDate_In_Months = 0;
                objOrderSubmit.TestDate_In_Days = 0;
                objOrderSubmit.TestDate_In_Weeks = 0;
                objOrderSubmit.Quantity = 0;
                if (dtpLastseenbyPhysician.Value != null && dtpLastseenbyPhysician.Value != string.Empty && Convert.ToDateTime(dtpLastseenbyPhysician.Value) != DateTime.MinValue)
                    objOrderSubmit.Date_Last_Seen = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpLastseenbyPhysician.Value));
                else
                {
                    objOrderSubmit.Date_Last_Seen = DateTime.MinValue;
                }
                if (txtDurationofneedforDME.Value != string.Empty)
                    objOrderSubmit.Duration_for_DME_Need_in_Months = Convert.ToInt16(txtDurationofneedforDME.Value);
                if (txtDurationofneedforsupplies.Value != string.Empty)
                    objOrderSubmit.Duration_for_Supplies_Need_in_Months = Convert.ToInt16(txtDurationofneedforsupplies.Value);
                sLocalTime = UtilityManager.ConvertToUniversal(objOrderSubmit.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                SaveOrdersSubmitList.Add(objOrderSubmit);
            }
            var serializer = new NetDataContractSerializer();
            objOrdersManager = new OrdersManager();
            string sSelectedOrder = string.Empty;
            objOrdersManager.InsertToDMEOrders(SaveOrderList.ToArray<Orders>(), SaveOrdersSubmitList, SaveOrdersAssList.ToArray<OrdersAssessment>(), orderingProcedure.ToArray<string>(), EncounterID, OrderType, string.Empty,sLocalTime);
            OrdersDTO objOrderDTO = null;
            objOrderDTO = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(), false, ClientSession.LegalOrg);
            Session["objDMEDTO"] = objOrderDTO;
            LoadOrders(objOrderDTO);
            ClearAll(true);
        }
        public void UpdateDMEOrder()
        {
            //XDocument xmlLabLocation = XDocument.Load(Server.MapPath(@"ConfigXML\LabLocationList.xml"));
            //string sLocation = string.Empty;
            //if (txtLocation.Value != "")
            //{
            //    if (txtLocation.Value != string.Empty)
            //    {
            //        IEnumerable<XElement> xmlLocation = null;
            //        try
            //        {
            //            xmlLocation = xmlLabLocation.Element("LabLocationList")
            // .Elements("LabLocation").Where(xx => xx.Attribute("labid").Value == (cboLab.Items[cboLab.SelectedIndex].Value) && xx.Attribute("city").Value.ToUpper() == (txtLocation.Value.ToUpper()));

            //        }

            //        catch (Exception)
            //        {
            //            xmlLocation = null;
            //        }
            //        if (xmlLocation != null && xmlLocation.Count() > 0)
            //        {
            //            txtLocation.Value = xmlLocation.Attributes("city").First().Value.ToString();
            //            sLocation = xmlLocation.Attributes("id").First().Value.ToString();
            //        }
            //    }
            //}

            //CAP-2774
            LabLocationList objlabLocation = new LabLocationList();
            objlabLocation = ConfigureBase<LabLocationList>.ReadJson("LabLocationList.json");
            string sLocation = string.Empty;
            if (txtLocation.Value != "" && txtLocation.Value != string.Empty)
            {
                List<LabLocations> listLabLocation = new List<LabLocations>();
                try
                {
                    listLabLocation = objlabLocation.LabLocation.Where(a => a.labid == cboLab.Items[cboLab.SelectedIndex].Value.ToString() && a.city.ToUpper() == (txtLocation.Value.ToUpper())).ToList();
                }
                catch (Exception)
                {
                    listLabLocation = null;
                }
                if (listLabLocation != null && listLabLocation.Count() > 0)
                {
                    txtLocation.Value = listLabLocation[0].city.ToString();
                    sLocation = listLabLocation[0].id.ToString();
                }
            }

            OrdersDTO objOrderDTO = new OrdersDTO();
            if (Session["objDMEDTO"] != null)
                objOrderDTO = (OrdersDTO)Session["objDMEDTO"];


            IList<string> SelectedAssessment = new List<string>();
            IList<string> SelectedProcedure = new List<string>();
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
            if (objOrderDTO != null)
            {
                IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
                ilstOrderLabDetailsDTO = objOrderDTO.ilstOrderLabDetailsDTO;
                var objOrdersubmit = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == EditOrdersSubmitID select rec3).ToList<OrderLabDetailsDTO>();
                OriginalOrdersSubmit = objOrdersubmit[0].OrdersSubmit;
                OriginalOrdersList = objOrdersubmit.Where(a => a.ObjOrder.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.ObjOrder).ToList<Orders>();
                IList<ulong> OrderID = objOrdersubmit.Where(a => a.ObjOrder.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.ObjOrder.Id).Distinct().ToList<ulong>();
                //OriginalOrdersAssessment = objOrderDTO.OrderAssList.Where(a => OrderID.Contains(a.Order_ID)).ToList<OrdersAssessment>();
                OriginalOrdersAssessment = objOrderDTO.OrderAssList.Where(a => a.Order_Submit_ID == EditOrdersSubmitID).ToList<OrdersAssessment>();
            }
            //DiagnosticDTO objDiagnosticDTO = objOrdersManager.FillDiagnosticDTO(EditOrdersSubmitID, EncounterID, HumanID, PhysicianID, "DME ORDER", ClientSession.FacilityName);
            //if (objDiagnosticDTO != null)
            //{
            //    OriginalOrdersSubmit = objDiagnosticDTO.objOrdersSubmit;
            //    OriginalOrdersList = objDiagnosticDTO.OrdersLists.Where(a => a.Order_Submit_ID == EditOrdersSubmitID).ToList<Orders>();
            //    IList<ulong> OrderID = objDiagnosticDTO.OrdersLists.Where(a => a.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.Id).Distinct().ToList<ulong>();
            //    OriginalOrdersAssessment = objDiagnosticDTO.OrderAssList.Where(a => OrderID.Contains(a.Order_ID)).ToList<OrdersAssessment>();
            //}
            //OriginalOrdersSubmit = objOrdersSubmitManager.GetById(EditOrdersSubmitID);
            //OriginalOrdersAssessment = objOrdersAssessmentManager.GetOrderAssessmentByOrderSubmitID(EditOrdersSubmitID);
            //OriginalOrdersList = objOrdersAssessmentManager.GetOrders(EditOrdersSubmitID);
            OrderCodeLibraryManager objOrderCodeLibraryManager = new OrderCodeLibraryManager();
            OrderCodeLibrary objSelectedOrderCode = new OrderCodeLibrary();
            IList<string> OrderCodeType = new List<string>();
            string sLocalTime = string.Empty;
            OriginalProcedures = (IList<string>)Session["ProceduresViewList"];
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
                    objNewOrder.Modified_By = ClientSession.UserName;
                    objNewOrder.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    sLocalTime = UtilityManager.ConvertToUniversal(objNewOrder.Modified_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                }
                else
                {
                    objNewOrder = new Orders();
                    objNewOrder.Created_By = ClientSession.UserName;
                    objNewOrder.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    sLocalTime = UtilityManager.ConvertToUniversal(objNewOrder.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    objNewOrder.Encounter_ID = EncounterID;
                    objNewOrder.Physician_ID = PhysicianID;
                    objNewOrder.Human_ID = HumanID;
                }
                //Assign Procedure Details
                string procedure = string.Empty;
                if (splitText.Length > 0)
                {
                    objNewOrder.Lab_Procedure = splitText[0];
                    for (int i = 1; i < splitText.Length; i++)
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
                        objNewOrder.Order_Code_Type = cboLab.Items[cboLab.SelectedIndex].Text.ToUpper();
                    }
                }

                OrderCodeType.Add(objNewOrder.Order_Code_Type);
                if (OriginalProcedures.Contains(itemText))
                    UpdateOrdersList.Add(objNewOrder);
                else
                    SaveOrderList.Add(objNewOrder);
            }

            DeleteOrdersList = OriginalOrdersList.Except(UpdateOrdersList).ToList();

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
                DelListOrderSubmit.Add(UpdateOrderSubmit);
                UpdateOrdersList = UpdateOrdersList.Except(UpdateOrdersList.Where(a => a.Order_Submit_ID == UpdateOrderSubmit.Id).ToList<Orders>()).ToList<Orders>();
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
                    //UpdateOrderSubmit.Version = UpdateOrderSubmit.Version + 1;
                    IsUpdate = true;
                }
                else
                {
                    IsUpdate = false;
                    UpdateOrderSubmit = new OrdersSubmit();
                    UpdateOrderSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                UpdateOrderSubmit.Order_Code_Type = rec;
                UpdateOrderSubmit.Facility_Name = ClientSession.FacilityName;
                if (chkMoveToMA.Checked == true)
                {
                    UpdateOrderSubmit.Move_To_MA = "Y";
                }
                else
                {
                    UpdateOrderSubmit.Move_To_MA = "N";
                }
                if (cboLab.Items[cboLab.SelectedIndex].Selected != null)
                    UpdateOrderSubmit.Lab_ID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                UpdateOrderSubmit.Lab_Location_Name = txtLocation.Value;
                UpdateOrderSubmit.Lab_Name = txtCenterName.Value;
                if (sLocation != string.Empty)
                    UpdateOrderSubmit.Lab_Location_ID = Convert.ToUInt32(sLocation);
                else
                    UpdateOrderSubmit.Lab_Location_ID = Convert.ToUInt32(0);
                UpdateOrderSubmit.Order_Notes = txtOrderNotes.txtDLC.Text;
                UpdateOrderSubmit.Order_Type = OrderType;
                UpdateOrderSubmit.Human_ID = HumanID;
                UpdateOrderSubmit.Physician_ID = PhysicianID;
                UpdateOrderSubmit.Encounter_ID = EncounterID;
                UpdateOrderSubmit.Lab_Name = cboLab.Items[cboLab.SelectedIndex].Text;
                DateTime tempDate = DateTime.MinValue;
                if (dtpLastseenbyPhysician.Value != null && dtpLastseenbyPhysician.Value != string.Empty && Convert.ToDateTime(dtpLastseenbyPhysician.Value) != DateTime.MinValue)
                    UpdateOrderSubmit.Date_Last_Seen = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpLastseenbyPhysician.Value));
                else
                {
                    UpdateOrderSubmit.Date_Last_Seen = DateTime.MinValue;
                }
                if (txtDurationofneedforDME.Value != string.Empty)
                    UpdateOrderSubmit.Duration_for_DME_Need_in_Months = Convert.ToInt16(txtDurationofneedforDME.Value);
                if (txtDurationofneedforsupplies.Value != string.Empty)
                    UpdateOrderSubmit.Duration_for_Supplies_Need_in_Months = Convert.ToInt16(txtDurationofneedforsupplies.Value);
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
                        AssessmentObj.ICD_Description = currentICD.Substring(currentICD.IndexOf('-') + 1, currentICD.Length - currentICD.IndexOf('-') - 1);
                        //AssessmentObj.Internal_Property_Associated_Order_CPT = UpdateOrdersList[uo].Lab_Procedure;
                        AssessmentObj.Source = AssessmentSource[AssessmentObj.ICD].Split('-')[0];
                        AssessmentObj.Source_ID = Convert.ToUInt64(AssessmentSource[AssessmentObj.ICD].Split('-')[1]);
                        savOrdAssListFornewCPT.Add(AssessmentObj);
                    }
                }
            }
            if (DelListOrderSubmit.Count > 0)
            {
                for (int uo = 0; uo < DelListOrderSubmit.Count; uo++)
                {
                    IList<OrdersAssessment> DeleteOrderAssessment = OriginalOrdersAssessment.Where(o => o.Order_Submit_ID == DelListOrderSubmit[uo].Id).ToList<OrdersAssessment>();
                    List<string> ICDList = DeleteOrderAssessment.Select(c => c.ICD).ToList();
                    IList<string> DeleteAssessmentList = SelectedAssessment.Where(s => !ICDList.Any(ic => ic.ToString() == s.ToString().Split('-')[0])).ToList();
                    for (int ua = 0; ua < DeleteOrderAssessment.Count(); ua++)
                    {
                        delOrdAssList.Add(DeleteOrderAssessment[ua]);
                    }
                }
            }
            List<string> SelectedICD = SelectedAssessment.Select(s => s.Split('-')[0]).ToList();
            delOrdAssList = OriginalOrdersAssessment.Where(oa => !SelectedICD.Any(si => si == oa.ICD)).ToList<OrdersAssessment>();

            IList<ulong> DelOrderIDList = new List<ulong>();
            DelOrderIDList = DeleteOrdersList.Select(a => a.Id).ToList();

            objOrdersManager = new OrdersManager();
            objOrdersManager.DMEUpdateOrders(SaveOrderList, UpdateOrdersList, DeleteOrdersList, savOrdAssList, delOrdAssList, savOrdAssListFornewCPT, null, new string[] { }, new string[] { }, new string[] { }, EncounterID, OrderType.ToUpper(), string.Empty, SavListOrderSubmit, UpdListOrderSubmit, DelListOrderSubmit,sLocalTime);

            objOrderDTO = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(), false,ClientSession.LegalOrg);
            Session["objDMEDTO"] = objOrderDTO;
            LoadOrders(objOrderDTO);
            btnClearAll.Value = "Clear All";
            ClearAll(true);
            #endregion
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateSave())
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            

            if(btnAdd.Value=="Add")
            {
                AddDMEOrder();
            }
            else if(btnAdd.Value=="Update")
            {
                UpdateDMEOrder();
            }
            hdnMovetoNextProcess.Value = "true";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HiddenValue", "LabOrder_SavedSuccessfully();top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);

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
       
        protected void grdOrders_ItemCreated(object sender, GridItemEventArgs e)
        {
            e.Item.ToolTip = "";
            if (e.Item is GridDataItem)
            {
                GridDataItem gridItem = e.Item as GridDataItem;
                foreach (GridColumn column in grdOrders.MasterTableView.RenderColumns)
                {
                    if (column.UniqueName == "Edit")
                    {
                        gridItem[column.UniqueName].ToolTip = "Edit";
                    }
                    else if (column.UniqueName == "Del")
                    {
                        gridItem[column.UniqueName].ToolTip = "Delete";
                    }

                }
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetQuantity(string Quantity)
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

            Orders objorder = new Orders();
            OrdersManager objOrderMngr = new OrdersManager();
            ulong[] uID = new ulong[quantityp.Count()];

            for (int i = 0; i < quantityp.Count(); i++)
            {

                uID[i] = Convert.ToUInt32(quantityp[i]);
            }

            lstUpdate = objOrderMngr.GetOrdersByOrderID(uID);
            return JsonConvert.SerializeObject(lstUpdate);

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
            ulong uHumanID = 0;
            ulong uPhysicianID = 0;
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
                    objorder.Prior_Auth_Req = xid[2];
                    objorder.Beyond_Qty_Limit = xid[3];
                    objorder.Custom_Item = xid[4];
                    objorder.Justification = xid[5];
                    //string Qty = "x___" + objorder.Quantity + "___";
                    //  objorder.Lab_Procedure_Description = (objorder.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None).Count() > 1 ? (objorder.Lab_Procedure_Description.Split(new[] { "x_" }, StringSplitOptions.None)[0].ToString() + Qty) : objorder.Lab_Procedure_Description);
                    uEncounterID = objorder.Encounter_ID;
                    uHumanID = objorder.Human_ID;
                    uPhysicianID = objorder.Physician_ID;
                    lstUpdate.Add(objorder);
                }
                objOrderMngr.SaveUpdateDelete_DBAndXML_WithTransaction(ref lstsavenull, ref lstUpdate, null, string.Empty, true, true, uHumanID, string.Empty);

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
            OrdersManager objOrdersManager = new OrdersManager();
            OrdersDTO objOrderDTO = new OrdersDTO();
            objOrderDTO = objOrdersManager.LoadOrdersDME(uEncounterID, uPhysicianID, uHumanID, "DME ORDER", UtilityManager.ConvertToUniversal(), false, ClientSession.LegalOrg);
            HttpContext.Current.Session["objDMEDTO"] = objOrderDTO;
            
            return "";

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
        }
        private void LoadOrders(OrdersDTO objOrderDTO)
        {
            IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
            IList<OrdersAssessment> ilstOrdersAssessment = new List<OrdersAssessment>();
            if (grdOrders.DataSource != null)
                grdOrders.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("Edit", typeof(Bitmap));
            dt.Columns.Add("Del", typeof(Bitmap));
            dt.Columns.Add("Procedure", typeof(string));
            dt.Columns.Add("Diagnosis", typeof(string));
            dt.Columns.Add("VendorName", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Columns.Add("LastSeenbyPhysician", typeof(string));
            dt.Columns.Add("NeedforDMEDurationinMonths", typeof(string));
            dt.Columns.Add("NeedforSuppliesDurationinMonths", typeof(string));
            dt.Columns.Add("NotesInstructions", typeof(string));
            dt.Columns.Add("CurrentProcess", typeof(string));
            dt.Columns.Add("OrderID", typeof(string));
            dt.Columns.Add("OrderSubmitID", typeof(string));
            dt.Columns.Add("LabID", typeof(string));
            ilstOrdersAssessment = objOrderDTO.OrderAssList;
            ilstOrderLabDetailsDTO = objOrderDTO.ilstOrderLabDetailsDTO;
            var TotalSubmitedOrders = (from rec in ilstOrderLabDetailsDTO select rec.OrdersSubmit.Id).Distinct();
            foreach (var submitID in TotalSubmitedOrders)
            {
                OrdersDTO tagObj = new OrdersDTO();
                IList<OrderLabDetailsDTO> OrderLabDetailsDTOBag = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == submitID select rec3).ToList<OrderLabDetailsDTO>();
                IList<ulong> OrdersIDCorrespondToSub = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == submitID select rec3.ObjOrder.Id).Distinct().ToList();
                //IList<OrdersAssessment> OrdersAssessmentBag = (from rec3 in ilstOrdersAssessment where OrdersIDCorrespondToSub.Contains(rec3.Order_Submit_ID) select rec3).ToList<OrdersAssessment>();
                IList<OrdersAssessment> OrdersAssessmentBag = (from rec3 in ilstOrdersAssessment where rec3.Order_Submit_ID == submitID select rec3).ToList<OrdersAssessment>();
                tagObj.OrderAssList = OrdersAssessmentBag;
                tagObj.ilstOrderLabDetailsDTO = OrderLabDetailsDTOBag;
                StringBuilder ICD = new StringBuilder();
                string LabProcedure = string.Empty;
                string DiagnosisCodes = string.Empty;
                IList<OrderLabDetailsDTO> ForCorrespondingSubID = (from rec2 in ilstOrderLabDetailsDTO where rec2.OrdersSubmit.Id == submitID select rec2).ToList<OrderLabDetailsDTO>();
                if (!ForCorrespondingSubID[0].ObjOrder.Internal_Property_Current_Process.StartsWith("DELETED_OR"))
                {
                    DateTime CreatedOrModifiedDateAndTime = new DateTime();
                    if (ForCorrespondingSubID[0].OrdersSubmit.Modified_Date_And_Time.ToString("dd-MM-yyyy").StartsWith("01-01-0001"))
                        CreatedOrModifiedDateAndTime = UtilityManager.ConvertToLocal(ForCorrespondingSubID[0].OrdersSubmit.Created_Date_And_Time);
                    else
                        CreatedOrModifiedDateAndTime = ForCorrespondingSubID[0].OrdersSubmit.Modified_Date_And_Time;
                    IList<string> dupcheck = new List<string>();
                    string sTestCode = string.Empty;
                    string sICD = string.Empty;
                    foreach (OrderLabDetailsDTO obj1 in ForCorrespondingSubID)
                    {
                        LabProcedure += "  ,  " + obj1.ObjOrder.Lab_Procedure.Trim() + "-" + obj1.ObjOrder.Lab_Procedure_Description.Trim();
                        sTestCode += obj1.ObjOrder.Lab_Procedure.Trim() + ",";
                        IList<OrdersAssessment> ForThisOrder = (from rec2 in ilstOrdersAssessment where rec2.Order_Submit_ID == obj1.ObjOrder.Order_Submit_ID select rec2).ToList<OrdersAssessment>();
                        foreach (OrdersAssessment ordAss in ForThisOrder)
                        {
                            if (!dupcheck.Contains(ordAss.ICD.Trim() + "-" + ordAss.ICD_Description.Trim()))
                            {
                                dupcheck.Add(ordAss.ICD.Trim() + "-" + ordAss.ICD_Description.Trim());
                                ICD.Append("  ,  " + ordAss.ICD.Trim() + "-" + ordAss.ICD_Description.Trim());
                                sICD += ordAss.ICD.Trim() + ",";
                            }
                        }
                    }
                    DataRow dr = dt.NewRow();
                    //dr["Edit"]= global::Acurus.Capella.UI.Resources.edit;
                    //dr["Del"]= global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                    dr["Procedure"] = LabProcedure.Substring(4).Trim();
                    if (ICD != null && ICD.ToString().Trim() != "")
                        dr["Diagnosis"] = ICD.ToString().Substring(4).Trim();
                    dr["VendorName"] = ForCorrespondingSubID[0].OrdersSubmit.Lab_Name;
                    dr["Location"] = ForCorrespondingSubID[0].LabLocName;
                    dr["LastSeenbyPhysician"] = ForCorrespondingSubID[0].OrdersSubmit.Date_Last_Seen.ToString("dd-MMM-yyyy") != "01-Jan-0001" ? ForCorrespondingSubID[0].OrdersSubmit.Date_Last_Seen.ToString("dd-MMM-yyyy") : "";
                    dr["NeedforDMEDurationinMonths"] = ForCorrespondingSubID[0].OrdersSubmit.Duration_for_DME_Need_in_Months;//ForCorrespondingSubID[0].OrdersSubmit.Duration_for_DME_Need_in_Months!=0 ? ForCorrespondingSubID[0].OrdersSubmit.Duration_for_DME_Need_in_Months : 1;
                    dr["NeedforSuppliesDurationinMonths"] = ForCorrespondingSubID[0].OrdersSubmit.Duration_for_Supplies_Need_in_Months;
                    dr["NotesInstructions"] = ForCorrespondingSubID[0].OrdersSubmit.Order_Notes;
                    dr["CurrentProcess"] = ForCorrespondingSubID[0].ObjOrder.Internal_Property_Current_Process;
                    dr["OrderID"] = ForCorrespondingSubID[0].ObjOrder.Id.ToString();
                    dr["OrderSubmitID"] = submitID.ToString();
                    dr["LabID"] = ForCorrespondingSubID[0].OrdersSubmit.Lab_ID.ToString();
                    //dr["FillQuestionSetsValue"] = string.Join(",", ilstQuestionSet.ToArray<string>());
                    dt.Rows.Add(dr);
                }
            }
            grdOrders.DataSource = dt;
            this.grdOrders.MasterTableView.SortExpressions.Clear();

            // Create "Date" sorting
            //GridSortExpression expression = new GridSortExpression();
            //expression.FieldName = "CreatedOrModifiedDateAndTime1";
            //expression.SortOrder = GridSortOrder.Descending;

            //// Set initial sortexpression to the [Date] column
            //this.grdOrders.MasterTableView.SortExpressions.AddSortExpression(expression);
            grdOrders.DataBind();
        }

        protected void grdOrders_ItemCommand(object sender, GridCommandEventArgs e)
        {
            string sLocalTime = string.Empty;
            if (e.CommandArgument != string.Empty)
            {
                GridDataItem drv1 = grdOrders.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)];
                
                if (e.CommandName == "EditC")
                {
                    GridDataItem drv = grdOrders.MasterTableView.Items[Convert.ToInt32(e.CommandArgument)];
                    EditOrdersSubmitID = Convert.ToUInt32(drv1["OrderSubmitID"].Text);
                    hdnOrderSubmitID.Value = drv1["OrderSubmitID"].Text;
                    SelectLab(drv["VendorName"].Text, drv["LabID"].Text, drv["Diagnosis"].Text, drv["Procedure"].Text, EditOrdersSubmitID);
                    btnAdd.Value = "Update";
                    btnClearAll.Value = "Cancel";
                    btnEditQuantity.Disabled = false;
                    btnAdd.Disabled = true;
                    //ClearText();
                }
                else if (e.CommandName == "Del")
                {
                    if (Session["objDMEDTO"] != null)
                        objOrderDTO = (OrdersDTO)Session["objDMEDTO"];
                    EditOrdersSubmitID = Convert.ToUInt32(drv1["OrderSubmitID"].Text);
                    hdnOrderSubmitID.Value = drv1["OrderSubmitID"].Text;
                    if (objOrderDTO != null)
                    {
                        IList<Orders> DeleteOrdersList = new List<Orders>();
                        IList<OrdersAssessment> DeleteOrdersAssessment = new List<OrdersAssessment>();
                        IList<OrdersSubmit> DeleteOrdersSubmit = new List<OrdersSubmit>();
                        IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
                        OrdersSubmit OriginalOrdersSubmit = new OrdersSubmit();
                        ilstOrderLabDetailsDTO = objOrderDTO.ilstOrderLabDetailsDTO;
                        var objOrdersubmit = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == EditOrdersSubmitID select rec3).ToList<OrderLabDetailsDTO>();
                        OriginalOrdersSubmit = objOrdersubmit[0].OrdersSubmit;
                        DeleteOrdersSubmit.Add(OriginalOrdersSubmit);
                        DeleteOrdersList = objOrdersubmit.Where(a => a.ObjOrder.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.ObjOrder).ToList<Orders>();
                        IList<ulong> OrderID = objOrdersubmit.Where(a => a.ObjOrder.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.ObjOrder.Id).Distinct().ToList<ulong>();
                        DeleteOrdersAssessment = objOrderDTO.OrderAssList.Where(a => OrderID.Contains(a.Order_Submit_ID)).ToList<OrdersAssessment>();

                        IList<Orders> SaveOrderList = new List<Orders>();
                        IList<OrdersAssessment> savOrdAssList = new List<OrdersAssessment>();
                        IList<OrdersSubmit> SavListOrderSubmit = new List<OrdersSubmit>();

                        IList<Orders> UpdateOrdersList = new List<Orders>();
                        IList<OrdersAssessment> savOrdAssListFornewCPT = new List<OrdersAssessment>();
                        IList<OrdersSubmit> UpdListOrderSubmit = new List<OrdersSubmit>();

                        objOrdersManager = new OrdersManager();
                        objOrdersManager.DMEUpdateOrders(SaveOrderList, UpdateOrdersList, DeleteOrdersList, savOrdAssList, DeleteOrdersAssessment, savOrdAssListFornewCPT, null, new string[] { }, new string[] { }, new string[] { }, EncounterID, OrderType.ToUpper(), string.Empty, SavListOrderSubmit, UpdListOrderSubmit, DeleteOrdersSubmit,sLocalTime);

                        objOrderDTO = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(), false, ClientSession.LegalOrg);
                        Session["objDMEDTO"] = objOrderDTO;
                        LoadOrders(objOrderDTO);
                        btnAdd.Disabled = true;
                    }
                }
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string sLocalTime = string.Empty;
            DataTable dt = (DataTable)grdOrders.DataSource;
            if (Session["objDMEDTO"] != null)
                objOrderDTO = (OrdersDTO)Session["objDMEDTO"];
            EditOrdersSubmitID = Convert.ToUInt32(grdOrders.Items[Convert.ToInt32(hdnRowIndex.Value)].Cells[13].Text);
            hdnOrderSubmitID.Value = EditOrdersSubmitID.ToString();
            if (objOrderDTO != null)
            {
                IList<Orders> DeleteOrdersList = new List<Orders>();
                IList<OrdersAssessment> DeleteOrdersAssessment = new List<OrdersAssessment>();
                IList<OrdersSubmit> DeleteOrdersSubmit = new List<OrdersSubmit>();
                IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
                OrdersSubmit OriginalOrdersSubmit = new OrdersSubmit();
                ilstOrderLabDetailsDTO = objOrderDTO.ilstOrderLabDetailsDTO;
                var objOrdersubmit = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == EditOrdersSubmitID select rec3).ToList<OrderLabDetailsDTO>();
                OriginalOrdersSubmit = objOrdersubmit[0].OrdersSubmit;
                DeleteOrdersSubmit.Add(OriginalOrdersSubmit);
                DeleteOrdersList = objOrdersubmit.Where(a => a.ObjOrder.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.ObjOrder).ToList<Orders>();
                IList<ulong> OrderID = objOrdersubmit.Where(a => a.ObjOrder.Order_Submit_ID == EditOrdersSubmitID).Select(a => a.ObjOrder.Id).Distinct().ToList<ulong>();
                DeleteOrdersAssessment = objOrderDTO.OrderAssList.Where(a => OrderID.Contains(a.Order_Submit_ID)).ToList<OrdersAssessment>();

                IList<Orders> SaveOrderList = new List<Orders>();
                IList<OrdersAssessment> savOrdAssList = new List<OrdersAssessment>();
                IList<OrdersSubmit> SavListOrderSubmit = new List<OrdersSubmit>();

                IList<Orders> UpdateOrdersList = new List<Orders>();
                IList<OrdersAssessment> savOrdAssListFornewCPT = new List<OrdersAssessment>();
                IList<OrdersSubmit> UpdListOrderSubmit = new List<OrdersSubmit>();

                objOrdersManager = new OrdersManager();
                objOrdersManager.DMEUpdateOrders(SaveOrderList, UpdateOrdersList, DeleteOrdersList, savOrdAssList, DeleteOrdersAssessment, savOrdAssListFornewCPT, null, new string[] { }, new string[] { }, new string[] { }, EncounterID, OrderType.ToUpper(), string.Empty, SavListOrderSubmit, UpdListOrderSubmit, DeleteOrdersSubmit,sLocalTime);

                objOrderDTO = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(), false, ClientSession.LegalOrg);
                Session["objDMEDTO"] = objOrderDTO;
                LoadOrders(objOrderDTO);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();};RefreshNotification('Orders');", true);
            //        }
        }
        protected void tblSelectProcedure_Click(object sender, EventArgs e)
        {
            //if (btnOrderSubmit.Attributes["Tag"] != null && btnOrderSubmit.Attributes["Tag"].ToString() == "UPDATE")//btnOrderSubmit.Value.ToString().ToUpper() == "UPDATE")
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMessage", "DisplayErrorMessage('230142', '','');", true);
            //    //ApplicationObject.erroHandler.DisplayErrorMessage("230142", this.Text);
            //    return;
            //}
            ProcedureSwitch(false);
           // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
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
                procedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, "DME PROCEDURE", selectedLabID, ClientSession.LegalOrg);
                FillLabProcedure(procedureList, lstChkeditem);
            }
        }
        private void SelectLab(string sLab,string sID,string sAssessment,string sProcedure,ulong uOrdersubmitid)
        {
            if (Session["objDMEDTO"] != null)
                objOrderDTO = (OrdersDTO)Session["objDMEDTO"];
            ListItem cboItem = new ListItem();
            cboItem.Text = sLab;
            cboItem.Value = sID;
            cboLab.SelectedIndex = cboLab.Items.IndexOf(cboItem);
            cboLab_SelectedIndexChanged(new object(), new EventArgs());
            for (int i = 0; i < chklstAssessment.Items.Count; i++)
            {
                chklstAssessment.Items[i].Selected = false;
            }
            //To Select ICDs While Editing
            IList<string> SelectedICD1 = new List<string>();
            //SelectedICD = objOrderDTO.OrderAssList.Select(a => a.ICD + "-" + a.ICD_Description).ToList<string>();
            IList<string> SelectedICD = new List<string>();
            SelectedICD1=sAssessment.Split('-').ToList();
            foreach (string str1 in SelectedICD1)
            {
                SelectedICD = str1.Split(',').ToList();
                foreach (string sval in SelectedICD)
                {
                    IList<string> str2 = objOrderDTO.OrderAssList.Where(aa => aa.ICD == sval.Trim()).Select(a => a.ICD + "-" + a.ICD_Description).ToList<string>();
                    if (str2.Count>0)
                    {
                        string str=str2[0];
                        if (AssessmentSource.ContainsKey(str.Split('-')[0]))
                            chklstAssessment.Items[GetIndexOfAICD(str.Split('-')[0])].Selected = true;
                        else
                        {
                            ListItem objListItem = new ListItem();
                            objListItem.Text = str;
                            objListItem.Selected = true;
                            chklstAssessment.Items.Add(objListItem);
                            AssessmentSource.Add(str.Split('-')[0], "OTHERS-0");
                        }
                    }
                }
            }
            IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
            ilstOrderLabDetailsDTO=objOrderDTO.ilstOrderLabDetailsDTO;
            var objOrdersubmit = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == uOrdersubmitid select rec3).ToList<OrderLabDetailsDTO>();
            if (objOrdersubmit[0].OrdersSubmit.Duration_for_DME_Need_in_Months.ToString()!="0")
                txtDurationofneedforDME.Value = objOrdersubmit[0].OrdersSubmit.Duration_for_DME_Need_in_Months.ToString();
            if (objOrdersubmit[0].OrdersSubmit.Duration_for_Supplies_Need_in_Months.ToString()!="0")
                txtDurationofneedforsupplies.Value = objOrdersubmit[0].OrdersSubmit.Duration_for_Supplies_Need_in_Months.ToString();
            txtOrderNotes.txtDLC.Text = objOrdersubmit[0].OrdersSubmit.Order_Notes.ToString();
            chkMoveToMA.Checked = YesOrNo(objOrdersubmit[0].OrdersSubmit.Move_To_MA);
            CheckMovetoMA(chkMoveToMA.Checked);
            txtLocation.Value = objOrdersubmit[0].OrdersSubmit.Lab_Location_Name;
            if (!objOrdersubmit[0].OrdersSubmit.Date_Last_Seen.ToString().Contains("0001"))
                dtpLastseenbyPhysician.Value = Convert.ToDateTime(objOrdersubmit[0].OrdersSubmit.Date_Last_Seen).ToString("dd-MMM-yyyy");
            for (int i = 0; i < chklstFrequentlyUsedProcedures.Items.Count; i++)
            {
                chklstFrequentlyUsedProcedures.Items[i].Selected = false;
            }
            IList<ListItem> tempList = new List<ListItem>();
            IList<string> SelectedProcedure = new List<string>();
            SelectedProcedure = sProcedure.Split('-').ToList();
            IList<string> SelectedProcedureList = new List<string>();
            IList<string> SelectedProcedure1 = new List<string>();
            foreach (string str1 in SelectedProcedure)
            {
                if (str1 != string.Empty)
                {
                    SelectedProcedure1 = str1.Split(',').ToList();
                    foreach (string sval in SelectedProcedure1)
                    {
                        IList<string> str2 = objOrdersubmit.Where(aa => aa.ObjOrder.Lab_Procedure == sval.Trim()).Select(a =>a.ObjOrder.Id+"$"+ a.ObjOrder.Lab_Procedure + "-" + a.ObjOrder.Lab_Procedure_Description).ToList<string>();

                        if (str2.Count > 0)
                        {
                            string str3 = str2[0];
                            string str=str3.Split('$')[1];
                            ListItem foundItem = chklstFrequentlyUsedProcedures.Items.FindByText(str);
                            if (foundItem != null)
                            {
                                foundItem.Attributes.Add("orderid", str3.Split('$')[0]);
                                foundItem.Selected = true;
                                SelectedProcedureList.Add(foundItem.Text);
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

                                ListItem item = new ListItem(str);
                                item.Selected = true;
                                item.Attributes.Add("orderid", str3.Split('$')[0]);
                                tempList.Add(item);
                                SelectedProcedureList.Add(item.Text);
                                //lstvFrequentlyUsedLabProcedures.Items.Add(item);
                            }
                        }
                    }
                }
            }

            if (tempList.Count > 0)
            {
                int insertIndex = FindInsertStartIndex();
                foreach (ListItem obj in tempList)
                {
                    chklstFrequentlyUsedProcedures.Items.Insert(insertIndex, obj);
                }
            }
            Session["ProceduresViewList"] = SelectedProcedureList;
        }
        bool YesOrNo(string YOrN)
        {
            if (YOrN.ToUpper() == "Y")
            {
                return true;
            }
            else
                return false;
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

        protected void btnPrintRequestion_ServerClick(object sender, EventArgs e)
        {
            hdnSelectedItem.Value = string.Empty;
            string path = Server.MapPath("Documents/" + Session.SessionID);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string FileLocation = string.Empty;

            PrintOrders print = new PrintOrders();
            // FileLocation = print.CallPrintLabAndImageOrders(path,EncounterID);
            OrdersDTO objOrderDTO = null;
           // object objDTO;
            var serializer = new NetDataContractSerializer();
            //objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false));
            //objOrderDTO = (OrdersDTO)objDTO;
            objOrderDTO = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(DateTime.Now), false, ClientSession.LegalOrg);
            FillHumanDTO objFillHumnaDTO = new FillHumanDTO();
            if (Session["objFillHumanDTO"] != null)
                objFillHumnaDTO = (FillHumanDTO)Session["objFillHumanDTO"];

            //IList<string> LabAndOrderSubmitID = objOrdersManager.GetOrderSubmitIDForPrintRequestion(EncounterID, PhysicianID, HumanID);
            //IList<string> LabAndOrderSubmits = objOrdersManager.GetOrderSubmitIDForPrintRequestion(ClientSession.EncounterId, ClientSession.PhysicianId, ClientSession.HumanId,OrderType);
            //IList<string> LabAndOrderSubmitID = LabAndOrderSubmits.Distinct().ToList<string>();

            //foreach (string str in LabAndOrderSubmitID)
            //{
            //    if (str.StartsWith("1"))
            //        FileLocation = print.PrintRequisitionUsingDatafromDB(Convert.ToUInt32(str.Split('|')[1]), path, "ORDERS", ClientSession.EncounterId, OrderType);
            //    else
            //        FileLocation = print.PrintSplitRequisitionUsingDatafromDBQuest(Convert.ToUInt32(str.Split('|')[1]), path, "ORDERS", false, OrderType);

            //    string[] Split1 = new string[] { Server.MapPath("") };
            //    string[] FileName1 = FileLocation.Split(Split1, StringSplitOptions.RemoveEmptyEntries);


            //    if (FileName1.Count() > 0)
            //    {
            //        for (int i = 0; i < FileName1.Count(); i++)
            //        {
            //            if (hdnSelectedItem.Value == string.Empty)
            //            {
            //                hdnSelectedItem.Value = FileName1[i].ToString();
            //            }
            //            else
            //            {
            //                hdnSelectedItem.Value += "|" + FileName1[i].ToString();
            //            }

            //        }
            //    }
            //}

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
            }
            string FaxSubject = string.Empty;
            if (hdnSelectedItem.Value != string.Empty)
            {
                if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                {
                    if (objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit != null)
                    {
                        //"Order_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Id + 
                        FaxSubject = "_" + objFillHumnaDTO.First_Name + " " + objFillHumnaDTO.Last_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_Name + "_" + objOrderDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time;
                    }
                }

                ScriptManager.RegisterStartupScript(this, typeof(frmDMEOrder), string.Empty, "OpenPDFImage('" + FaxSubject.Replace(@"'", "$|~|$") + "');", true);
            }
                if (FileLocation==string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "DME Order", "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
            //else if (LabAndOrderSubmitID.Count <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "DME Order", "DisplayErrorMessage('230115'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    return;
            //}
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }

            if (cboLab.SelectedIndex == 0)
            {
                //btnOrderSubmit.Disabled = true; //btnOrderSubmit.Enabled = false;
            }
        }
        public bool CheckForValidation(string sDatelastseen, string sDurationofneedforDME, string sDurationofneedforsupplies, string s)
        {
            errList = new ArrayList();

            //if (lblDatelastseen.InnerText.Contains('*') && dtpLastseenbyPhysician.Value == "")
            //{
            //    errList.Add(lblDatelastseen.InnerText.Replace('*', ' ').Trim());
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230160','','" + lblDatelastseen.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
            //    //dtpLastseenbyPhysician.Focus();
            //    return false;
            //}
            //else if (lblDurationofneedforDME.InnerText.Contains('*') && txtDurationofneedforDME.Value == string.Empty)
            //{
            //    errList.Add(lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim());
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230161','','" + lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
            //    txtDurationofneedforDME.Focus();
            //    return false;
            //}

            if (!(chkMoveToMA.Checked) && dtpLastseenbyPhysician.Value == "")
            {
                errList.Add(lblDatelastseen.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230160','','" + lblDatelastseen.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                //dtpLastseenbyPhysician.Focus();
                return false;
            }
            else if (!(chkMoveToMA.Checked) && txtDurationofneedforDME.Value == string.Empty)
            {
                errList.Add(lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230161','','" + lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtDurationofneedforDME.Focus();
                return false;
            }
            else if (txtDurationofneedforsupplies.Value.Trim() == string.Empty && lblDurationofneedforsupplies.InnerText.Contains("*"))
            {
                errList.Add(lblDurationofneedforsupplies.InnerText.Replace('*', ' ').Trim());
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230162','','" + lblDurationofneedforsupplies.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("230106", OrderType.ToUpper(), errList, this);
                txtDurationofneedforsupplies.Focus();
                return false;
            }
            else if ((!chklstFrequentlyUsedProcedures.Items.Cast<ListItem>().Any(a => a.Selected == true)))
            {
                errList.Clear();
                string errorMsg = string.Empty;
                if (OrderType.ToUpper() == "DME ORDER")
                {
                    errList.Add("a Lab Procedure");
                    errorMsg += " a Lab Procedure";
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

            return true;
        }
        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {
            if (hdnType.Value == "Yes")
            {
                if (!ValidateSave())
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                if (btnAdd.Value == "Add")
                {
                    AddDMEOrder();
                }
                else if (btnAdd.Value == "Update")
                {
                    UpdateDMEOrder();
                }
                hdnType.Value = "";
            }
            ulong uOrderSubmitID=0;
            errList = new ArrayList();
            if (Session["OrderSubmitId"]!=null && Session["OrderSubmitId"] != string.Empty)
            {
                hdnTransferVaraible.Value = Convert.ToString(Session["OrderSubmitId"]);
                uOrderSubmitID=Convert.ToUInt32(Session["OrderSubmitId"]);
            }
            if (Session["objDMEDTO"] != null)
                objOrderDTO = (OrdersDTO)Session["objDMEDTO"];
            IList<string> str2 = objOrderDTO.OrderAssList.Where(aa => aa.Order_Submit_ID == uOrderSubmitID).Select(a => a.ICD + "-" + a.ICD_Description).ToList<string>();
            IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
            ilstOrderLabDetailsDTO = objOrderDTO.ilstOrderLabDetailsDTO;
            var objOrdersubmit = (from rec3 in ilstOrderLabDetailsDTO where rec3.OrdersSubmit.Id == uOrderSubmitID select rec3).ToList<OrderLabDetailsDTO>();
            if (objOrdersubmit.Count>0)
            {
                if (objOrdersubmit[0].OrdersSubmit.Date_Last_Seen.ToString().Contains("0001"))
                {
                    errList.Add(lblDatelastseen.InnerText.Replace('*', ' ').Trim());
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230160','','" + lblDatelastseen.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else if (objOrdersubmit[0].OrdersSubmit.Duration_for_DME_Need_in_Months.ToString() == string.Empty || objOrdersubmit[0].OrdersSubmit.Duration_for_DME_Need_in_Months.ToString() == "0")
                {
                    errList.Add(lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim());
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230161','','" + lblDurationofneedforDME.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    txtDurationofneedforDME.Focus();
                    return;
                }
                else if (objOrdersubmit[0].OrdersSubmit.Duration_for_Supplies_Need_in_Months.ToString() == string.Empty || objOrdersubmit[0].OrdersSubmit.Duration_for_Supplies_Need_in_Months.ToString() == "0")
                {
                    errList.Add(lblDurationofneedforsupplies.InnerText.Replace('*', ' ').Trim());
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230162','','" + lblDurationofneedforsupplies.InnerText.Replace('*', ' ').Trim().ToString() + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    txtDurationofneedforsupplies.Focus();
                    return;
                }
                else if (objOrdersubmit[0].ObjOrder.Lab_Procedure == string.Empty)
                {
                    errList.Clear();
                    string errorMsg = string.Empty;
                    if (OrderType.ToUpper() == "DME ORDER")
                    {
                        errList.Add("a Lab Procedure");
                        errorMsg += " a Lab Procedure";
                    }
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230104','','" + errorMsg + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else if (str2.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('230107'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
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
                ulong uLabID = 0;
                uLabID = Convert.ToUInt64(cboLab.Items[cboLab.SelectedIndex].Value);
                if (uLabID != 32)
                {
                    if (!btnAdd.Disabled)
                    {
                        if (hdnType.Value != "")
                        {
                            btnAdd_Click(sender, e);
                            if (hdnMovetoNextProcess.Value == "false")
                                return;
                        }
                    }

                    var serializer = new NetDataContractSerializer();
                    OrdersDTO objOrderDTO1 = null;
                    objOrderDTO1 = objOrdersManager.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, UtilityManager.ConvertToUniversal(), false, ClientSession.LegalOrg);//.LoadOrdersDME(EncounterID, PhysicianID, HumanID, OrderType, string.Empty, UtilityManager.ConvertToUniversal(DateTime.Now), false);
                   
                    if (objOrderDTO1.ilstOrderLabDetailsDTO.Count > 0 || objOrderDTO1.ilstOrdersSubmitForPartialOrders.Count > 0)
                    {

                        IList<OrdersSubmit> maReviewOrders = (from o in objOrderDTO1.ilstOrderLabDetailsDTO where o.OrdersSubmit.Id == order_submit_id && o.ObjOrder.Internal_Property_Current_Process == "MA_REVIEW" select o.OrdersSubmit).Distinct().ToList<OrdersSubmit>();
                        int emptyCount = (from o in maReviewOrders where o.Duration_for_DME_Need_in_Months == 0 || o.Duration_for_Supplies_Need_in_Months==0 || o.Date_Last_Seen == Convert.ToDateTime("0001-01-01 00:00:00") select o).ToList<OrdersSubmit>().Count;
                        IList<OrdersSubmit> maReviewOrders1 = (from o in objOrderDTO1.ilstOrdersSubmitForPartialOrders where o.Id == order_submit_id select o).Distinct().ToList<OrdersSubmit>();
                        if (maReviewOrders1 != null && maReviewOrders1.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230126'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //ApplicationObject.erroHandler.DisplayErrorMessage("230126", OrderType.ToUpper());
                            return;
                        }
                        if (emptyCount == 0)
                        {
                            WFObjectManager obj_workFlow = new WFObjectManager();
                            obj_workFlow.MoveToNextProcess(order_submit_id, "DME ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowCloseDiagnostics();", true);
                        }
                        else if (emptyCount == maReviewOrders.Count)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('230126'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            //ApplicationObject.erroHandler.DisplayErrorMessage("230126", OrderType.ToUpper());
                            return;
                        }

                    }
                }
                else
                {
                    Boolean bReturnValue;
                    bReturnValue = CheckForValidation();
                    if (!bReturnValue)
                        return;
                    FileManagementIndexManager objFileManagementMngr = new FileManagementIndexManager();
                    IList<FileManagementIndex> lstfilemanage = new List<FileManagementIndex>();
                    lstfilemanage = objFileManagementMngr.GetImagesforAnnotations(HumanID, order_submit_id, "ORDER,ABI,SPIROMETRY,SCAN");
                    if (lstfilemanage.Count > 0)
                    {
                        if (sProcess == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
                        {

                            PhysicianManager objPhysicianMngr = new PhysicianManager();
                            close_type = 8;
                            FillPhysicianUser PhyUserList = objPhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                            User_name = PhyUserList.UserList.Where(P => P.Physician_Library_ID == PhysicianID).ToList()[0].user_name;

                        }
                        else if (sProcess == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
                        {
                            close_type = 1;
                            User_name = "UNKNOWN";

                        }
                        else if (sProcess == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                        {
                            close_type = 9;
                            User_name = "UNKNOWN";
                        }
                        else if (sProcess == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                        {
                            close_type = 1;
                            User_name = "UNKNOWN";
                        }
                        if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
                        {
                            WFObjectManager obj_workFlow = new WFObjectManager();
                            obj_workFlow.MoveToNextProcess(order_submit_id, "DME ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);

                            btnMoveToNextProcess.Disabled = true; //btnMoveToNextProcess.Enabled = false;

                        }
                        else if (ClientSession.UserRole == "Medical Assistant")
                        {
                            WFObjectManager obj_workFlow = new WFObjectManager();
                            //if (uLabID != 32)
                            //    obj_workFlow.MoveToNextProcess(order_submit_id, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                            ////Response.Write("<script> self.close(); </script>");
                            //else
                            //{
                            obj_workFlow.MoveToNextProcess(order_submit_id, "DME ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                            if (sProcess == "MA_REVIEW")
                            {
                                PhysicianManager objPhysicianMngr = new PhysicianManager();
                                FillPhysicianUser PhyUserList = objPhysicianMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                                User_name = PhyUserList.UserList.Where(P => P.Physician_Library_ID == PhysicianID).ToList()[0].user_name;
                                obj_workFlow.MoveToNextProcess(order_submit_id, "DME ORDER", 8, User_name, UtilityManager.ConvertToUniversal(DateTime.Now), null, null, null);
                            }
                            // }
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowCloseDiagnostics();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMessage", "DisplayErrorMessage('115039', '',''); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        //ApplicationObject.erroHandler.DisplayErrorMessage("115039", this.Text);
                        return;
                    }
                }



                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('280013');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowClose();", true);
                Session["OrderSubmitId"] = string.Empty;

            }
        }

    }
}