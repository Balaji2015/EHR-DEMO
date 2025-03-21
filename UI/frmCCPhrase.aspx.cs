using System;
using System.Collections;
using System.Collections.Generic;
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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Threading;
using Acurus.Capella.Core.DTOJson;
using Telerik.Web.UI.Gauge;

namespace Acurus.Capella.UI
{
    public partial class frmCCPhrase : System.Web.UI.Page
    {
        IList<FillChiefComplaint> fillCC = new List<FillChiefComplaint>();
        FillChiefComplaint fillCc = new FillChiefComplaint();

        IList<ChiefComplaints> addList = new List<ChiefComplaints>();
        ChiefComplaints saveComplaints = new ChiefComplaints();
        ChiefComplaintsManager objCC = new ChiefComplaintsManager();
        ulong ulMyEncounterID, ulMyHumanID, ulMyPhysicianID = 0;

        IList<ChiefComplaints> ChiefComplaintsLst = new List<ChiefComplaints>();

        protected void Page_Load(object sender, EventArgs e)
        {
            ulMyEncounterID = ClientSession.EncounterId;
            ulMyHumanID = ClientSession.HumanId;
            ulMyPhysicianID = ClientSession.PhysicianId;
            if (!IsPostBack)
            {
                ctmDLCChief_Complaints.txtDLC.TextMode = TextBoxMode.MultiLine;
                ctmDLCHPI_Notes.txtDLC.TextMode = TextBoxMode.MultiLine;
                //Gitlab# 2716 - Load transaction data from the Transaction Table instead of XML
                ChiefComplaintsLst = objCC.GetComplaintsByEncID(ClientSession.EncounterId, false);
                fillCc.CurrentCCList = ChiefComplaintsLst;
                if (fillCc.CurrentCCList != null && fillCc.CurrentCCList.Count == 0 && ClientSession.FillEncounterandWFObject != null && ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    fillCc.Purpose_Of_Visit = ClientSession.FillEncounterandWFObject.EncRecord.Purpose_of_Visit;
                }

                fillCC.Add(fillCc);

                Session["fillCC"] = fillCC;

                SecurityServiceUtility obj = new SecurityServiceUtility();
                obj.ApplyUserPermissions(this.Page);
                btnAdd.Disabled = true;

                if (fillCC != null)
                {
                    if (fillCC[0].CurrentCCList.Count > 0)
                    {
                        for (int i = 0; i < fillCC[0].CurrentCCList.Count; i++)
                        {
                            Control txtctrl = FindControl("ctmDLC" + fillCC[0].CurrentCCList[i].HPI_Element.Replace(" ", "_"));
                            if (txtctrl != null)
                            {
                                if (((UserControls.CustomDLCNew)txtctrl).txtDLC.Text == string.Empty)
                                    ((UserControls.CustomDLCNew)txtctrl).txtDLC.Text = fillCC[0].CurrentCCList[i].HPI_Value;
                                else
                                    ((UserControls.CustomDLCNew)txtctrl).txtDLC.Text += " " + fillCC[0].CurrentCCList[i].HPI_Value;
                            }
                        }
                    }
                    else if (fillCC[0].Purpose_Of_Visit.Trim() != "")
                    {
                        if (ctmDLCChief_Complaints.txtDLC.Enabled)
                        {
                            ctmDLCChief_Complaints.txtDLC.Text = fillCC[0].Purpose_Of_Visit;
                            if (!(btnClearall.Disabled))
                            {
                                CheckSave.Value = "true";
                                btnAdd.Disabled = false;
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PurposeOfVisit", "PurposeOfVisit();", true);
                            }
                        }
                    }
                }

                //code comment by balaji.T
                //if (ClientSession.UserRole.ToUpper() == "CODER")
                //{
                //    //pbChiefComplaints.Enable = false;
                //    //pbNotes.Enable = false;
                //}
                //StaticLookupManager objStaticLookupMngr = new StaticLookupManager();
                //IList<StaticLookup> ilstStaticLookup = new List<StaticLookup>();
                //ilstStaticLookup = objStaticLookupMngr.getStaticLookupByFieldName("Medication Reason Not Performed");
                //if (ilstStaticLookup.Count > 0)
                //{
                //    cboCurrentMedicationDocumented.Items.Add("");
                //    ListItem lstMedicationDocumented = null;
                //    for (int i = 0; i < ilstStaticLookup.Count; i++)
                //    {
                //        lstMedicationDocumented = new ListItem();
                //        lstMedicationDocumented.Text = ilstStaticLookup[i].Value;
                //        lstMedicationDocumented.Value = ilstStaticLookup[i].Description;
                //        cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                //    }
                //}


                //string sLookupReasonnotPerformedXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                //if (File.Exists(sLookupReasonnotPerformedXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(sLookupReasonnotPerformedXmlFilePath);
                //    itemDoc.Load(XmlText);
                //    XmlText.Close();
                //    XmlNodeList xmlNodeList1 = itemDoc.GetElementsByTagName("MedicationReasonNotPerformedList");
                //    if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
                //    {
                //        cboCurrentMedicationDocumented.Items.Add("");
                //        ListItem lstMedicationDocumented = null;
                //        for (int j = 0; j < xmlNodeList1[0].ChildNodes.Count; j++)
                //        {
                //            lstMedicationDocumented = new ListItem();
                //            lstMedicationDocumented.Text = xmlNodeList1[0].ChildNodes[j].Attributes["Value"].Value.ToString();
                //            lstMedicationDocumented.Value = xmlNodeList1[0].ChildNodes[j].Attributes["Description"].Value.ToString();
                //            cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                //        }
                //    }
                //    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("FlowSheetList");
                //    if (xmlNodeList != null && xmlNodeList.Count > 0)
                //    {
                //        Dictionary<string, string> dictFlowSheet = new Dictionary<string, string>();
                //        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                //        {
                //            //cboFlowSheetType.Items.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                //            dictFlowSheet.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[0].Value);
                //        }

                //        cboFlowSheetType.DataSource = dictFlowSheet;
                //        cboFlowSheetType.DataTextField = "Key";
                //        cboFlowSheetType.DataValueField = "Value";
                //        cboFlowSheetType.DataBind();
                //    }
                //    //Get Flow sheet period list from ststiclookup XML
                //    XmlNodeList xmlNodePeriodList = itemDoc.GetElementsByTagName("FlowSheetPeriodList");
                //    if (xmlNodePeriodList != null && xmlNodePeriodList.Count > 0)
                //    {
                //        for (int j = 0; j < xmlNodePeriodList[0].ChildNodes.Count; j++)
                //        {
                //            cboFlowSheetPeriod.Items.Add(xmlNodePeriodList[0].ChildNodes[j].Attributes[0].Value);
                //        }
                //    }
                //}
                //CAP-2787
                StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
                if (staticLookupList != null) { 
                   
                   var medicationReasonNotPerformedList = staticLookupList.MedicationReasonNotPerformedList.ToList();
                   if (medicationReasonNotPerformedList != null)
                   {
                       ListItem lstMedicationDocumented = null;
                        //Cap - 3069
                        cboCurrentMedicationDocumented.Items.Add("");
                        foreach (var medicationReasonNotPerformed in medicationReasonNotPerformedList)
                       {
                           if (medicationReasonNotPerformed != null)
                           {
                               lstMedicationDocumented = new ListItem();
                               lstMedicationDocumented.Text = medicationReasonNotPerformed.value;
                               lstMedicationDocumented.Value = medicationReasonNotPerformed.Description;
                               cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                           }
                       }
                   }

                   var flowSheetList = staticLookupList.FlowSheetList.ToList();
                   if (flowSheetList != null)
                   {
                       Dictionary<string, string> dictFlowSheet = new Dictionary<string, string>();
                       foreach (var flowSheet in flowSheetList)
                       {
                           if (flowSheetList != null)
                           {
                               dictFlowSheet.Add(flowSheet.value, flowSheet.Field_Name);
                           }
                       }
                       cboFlowSheetType.DataSource = dictFlowSheet;
                       cboFlowSheetType.DataTextField = "Key";
                       cboFlowSheetType.DataValueField = "Value";
                       cboFlowSheetType.DataBind();
                   }

                   var flowSheetPeriodList = staticLookupList.FlowSheetPeriodList.ToList();
                   if (flowSheetPeriodList != null)
                   {
                       foreach (var flowSheetPeriod in flowSheetPeriodList)
                       {
                            cboFlowSheetPeriod.Items.Add(flowSheetPeriod.value);
                       }
                    }
                }
                //Get Flow sheet list from staticlookup XML
                //string sLookupXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                //if (File.Exists(sLookupXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(sLookupXmlFilePath);
                //    itemDoc.Load(XmlText);
                //    XmlText.Close();
                //    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("FlowSheetList");
                //    if (xmlNodeList!=null && xmlNodeList.Count > 0)
                //    {
                //        Dictionary<string, string> dictFlowSheet = new Dictionary<string, string>();
                //        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                //        {
                //            //cboFlowSheetType.Items.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                //            dictFlowSheet.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[0].Value);
                //        }

                //        cboFlowSheetType.DataSource = dictFlowSheet;
                //        cboFlowSheetType.DataTextField = "Key";
                //        cboFlowSheetType.DataValueField = "Value";
                //        cboFlowSheetType.DataBind();
                //    }
                //    //Get Flow sheet period list from ststiclookup XML
                //    XmlNodeList xmlNodePeriodList = itemDoc.GetElementsByTagName("FlowSheetPeriodList");
                //    if (xmlNodePeriodList != null && xmlNodePeriodList.Count > 0)
                //    {
                //        for (int j = 0; j < xmlNodePeriodList[0].ChildNodes.Count; j++)
                //        {
                //            cboFlowSheetPeriod.Items.Add(xmlNodePeriodList[0].ChildNodes[j].Attributes[0].Value);
                //        }
                //    }
                //}
            }
            IList<Encounter> objEncounter = new List<Encounter>();
            objEncounter.Add(ClientSession.FillEncounterandWFObject.EncRecord);
            if (objEncounter.Count > 0)
            {
                ulong count = objEncounter[0].No_of_Med_Orders_In_Paper;
                if (txtcount.Value == string.Empty && (!IsPostBack))
                {
                    if (count != 0)
                    {
                        txtcount.Value = count.ToString();
                    }
                    else
                    {
                        txtcount.Value = count.ToString();
                    }
                }
                if (!IsPostBack)
                {
                    if (objEncounter[0].Is_Medication_Reviewed == "Y")
                    {
                        chkCurrentMedicationDocumented.Checked = true;
                    }
                    else
                    {
                        if (objEncounter[0].Is_Medication_Reviewed == "" && ClientSession.UserCurrentProcess != "PROVIDER_REVIEW" && ClientSession.UserRole.ToUpper() != "CODER" && ClientSession.UserRole.ToUpper() != "TECHNICIAN" && ClientSession.UserCurrentProcess != "TECHNICIAN_PROCESS")//BugID:47790 - added UserRole and Current_process Check//CMG Ancilliary
                        {
                            chkCurrentMedicationDocumented.Checked = true;
                            CheckSave.Value = "true";
                        }
                        else
                            chkCurrentMedicationDocumented.Checked = false;
                    }
                    cboCurrentMedicationDocumented.SelectedIndex = cboCurrentMedicationDocumented.Items.IndexOf(cboCurrentMedicationDocumented.Items.FindByText(objEncounter[0].Reason_Not_Performed_Medication_Reviewed));
                }
            }
            if (chkCurrentMedicationDocumented.Checked)
            {
                cboCurrentMedicationDocumented.Disabled = true;
                lblCurrentMedicationDocumented.InnerText = lblCurrentMedicationDocumented.InnerText.Replace("*", "").TrimEnd();
                lblCurrentMedicationDocumented.Attributes.Add("style", "color:black");
                cboCurrentMedicationDocumented.SelectedIndex = 0;
            }
            else
            {
                cboCurrentMedicationDocumented.Disabled = false;
                if (!lblCurrentMedicationDocumented.InnerText.Contains("*"))
                {
                    lblCurrentMedicationDocumented.InnerText += " *";
                    lblCurrentMedicationDocumented.Attributes.Add("style", "color:red");
                }
            }
            if (Session["fillCC"] != null)
                fillCC = (IList<FillChiefComplaint>)Session["fillCC"];

            ctmDLCChief_Complaints.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            ctmDLCHPI_Notes.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            ctmDLCChief_Complaints.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
            ctmDLCHPI_Notes.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");

            //BugID:47790 - added UserRole and Current_process Check
            if (ClientSession.UserCurrentProcess != "PROVIDER_PROCESS" && ClientSession.UserCurrentProcess != "PHYSICIAN_CORRECTION")
            {
                fldsetMedDocumentation.Attributes.Add("disabled", "true");
            }
            if (ClientSession.UserRole.ToUpper() == "CODER" || ClientSession.UserRole.ToUpper() == "TECHNICIAN" || ClientSession.UserCurrentProcess == "TECHNICIAN_PROCESS")//CMG Ancilliary
            {
                btnCopyCC.Attributes.Add("disabled", "true");
                btnAdd.Attributes.Add("disabled", "true");
                btnClearall.Attributes.Add("disabled", "true");
                btnAdd.Disabled = true;
                hdnTechnician.Value = "true";
            }
            //if (ClientSession.UserCurrentProcess == "DISABLE")
            //{
            //    btnAdd.Disabled = true;
            //}

            if (ClientSession.UserCurrentProcess == "DISABLE")
            {
                hdnDisableCurrentProcess.Value = "true";
                if (hdnDisableCurrentProcess.Value == "true")
                {
                    btnAdd.Disabled = true;

                }
            }


        }
        protected void chkCurrentMedication_ServerChange(object sender, EventArgs e)
        {
            btnAdd.Disabled = false;
            if (chkCurrentMedicationDocumented.Checked == false)
            {
                cboCurrentMedicationDocumented.Items.Clear();
                //string sLookupReasonnotPerformedXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                //if (File.Exists(sLookupReasonnotPerformedXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(sLookupReasonnotPerformedXmlFilePath);
                //    itemDoc.Load(XmlText);
                //    XmlText.Close();
                //    XmlNodeList xmlNodeList1 = itemDoc.GetElementsByTagName("MedicationReasonNotPerformedList");
                //    if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
                //    {
                //        cboCurrentMedicationDocumented.Items.Add("");
                //        ListItem lstMedicationDocumented = null;
                //        for (int j = 0; j < xmlNodeList1[0].ChildNodes.Count; j++)
                //        {
                //            lstMedicationDocumented = new ListItem();
                //            lstMedicationDocumented.Text = xmlNodeList1[0].ChildNodes[j].Attributes["Value"].Value.ToString();
                //            lstMedicationDocumented.Value = xmlNodeList1[0].ChildNodes[j].Attributes["Description"].Value.ToString();
                //            cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                //        }
                //    }
                //}
                //CAP-2787
                StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
                var medicationReasonNotPerformedList = staticLookupList.MedicationReasonNotPerformedList.ToList();
                if (medicationReasonNotPerformedList != null)
                {
                    cboCurrentMedicationDocumented.Items.Add("");
                    ListItem lstMedicationDocumented = null;
                    foreach (var medicationReasonNotPerformed in medicationReasonNotPerformedList)
                    {
                        if (medicationReasonNotPerformed != null)
                        {
                            lstMedicationDocumented = new ListItem();
                            lstMedicationDocumented.Text = medicationReasonNotPerformed.value.ToString();
                            lstMedicationDocumented.Value = medicationReasonNotPerformed.Description.ToString();
                            cboCurrentMedicationDocumented.Items.Add(lstMedicationDocumented);
                        }
                    }
                }
            }
        }
        void SaveMethod(Control pair)
        {
            DateTime utc = new DateTime();
            if (hdnLocalTime.Value != "")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                if (strtime != null && strtime != "")
                    utc = Convert.ToDateTime(strtime);
            }

            saveComplaints = new ChiefComplaints();
            saveComplaints.HPI_Element = ((UserControls.CustomDLCNew)pair).ID.Substring(6).Replace("_", " ");
            saveComplaints.HPI_Value = ((UserControls.CustomDLCNew)pair).txtDLC.Text.Trim();
            saveComplaints.Encounter_ID = ulMyEncounterID;
            saveComplaints.Human_ID = ulMyHumanID;
            saveComplaints.Physician_ID = ulMyPhysicianID;
            saveComplaints.Created_Date_And_Time = utc;
            saveComplaints.Created_By = ClientSession.UserName;
            addList.Add(saveComplaints);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var sComplaintText = string.Empty;
            DateTime utc = new DateTime();
            if (hdnLocalTime.Value != "")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                if (strtime != null && strtime != "")
                    utc = Convert.ToDateTime(strtime);
            }

            addList = new List<ChiefComplaints>();

            IList<ChiefComplaints> updateList = new List<ChiefComplaints>();
            IList<ChiefComplaints> deleteList = new List<ChiefComplaints>();
            IList<Control> lst_AllText_Control = new List<Control>() { ctmDLCChief_Complaints, ctmDLCHPI_Notes };
            IList<Control> lst_Fill_TextControl = lst_AllText_Control.Where(q => ((UserControls.CustomDLCNew)q).txtDLC.Text.Trim() != string.Empty).ToList<Control>();

            IList<FillChiefComplaint> Resultlst = new List<FillChiefComplaint>(); ;

            if (ctmDLCChief_Complaints.txtDLC.Text.Trim() != string.Empty)
            {
                if (fillCC != null && fillCC.Count > 0)
                {
                    IList<Encounter> objEncounter = new List<Encounter>();
                    IList<Encounter> objEnc = new List<Encounter>();
                    objEncounter.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                    if (objEncounter.Count > 0)
                    {
                        
                        if (txtcount.Value != null && txtcount.Value.Trim() != "" && System.Text.RegularExpressions.Regex.IsMatch(txtcount.Value, "^[0-9]*$")==true)
                        {
                            objEncounter[0].No_of_Med_Orders_In_Paper = Convert.ToUInt16(txtcount.Value);
                        }
                        if (chkCurrentMedicationDocumented.Checked)
                            objEncounter[0].Is_Medication_Reviewed = "Y";
                        else
                            objEncounter[0].Is_Medication_Reviewed = "N";
                        if (cboCurrentMedicationDocumented.Items.Count > 0)
                        {
                            objEncounter[0].Reason_Not_Performed_Medication_Reviewed = cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Text;
                            objEncounter[0].Snomed_Reason_Not_Performed_Med_Reviewed = cboCurrentMedicationDocumented.Items[cboCurrentMedicationDocumented.SelectedIndex].Value;
                        }
                        else
                        {
                            objEncounter[0].Reason_Not_Performed_Medication_Reviewed = "";
                            objEncounter[0].Snomed_Reason_Not_Performed_Med_Reviewed = "";
                        }
                    }


                    if (fillCC[0].CurrentCCList.Count == 0)
                    {
                        foreach (Control pair in lst_Fill_TextControl)
                        {
                            SaveMethod(pair);
                        }
                        Resultlst = objCC.SaveComplaints(addList.ToArray(), ref objEncounter, "");

                        Session["fillCC"] = Resultlst;
                    }
                    else
                    {
                        if (Session["fillCC"] != null)
                            fillCC = (IList<FillChiefComplaint>)Session["fillCC"];

                        if (lst_AllText_Control != null && lst_AllText_Control.Count > 0)
                            foreach (Control pair in lst_AllText_Control)
                            {
                                if (fillCC != null && fillCC[0].CurrentCCList.Where(A => A.HPI_Element == pair.ID.Substring(6).Replace("_", " ")).Count() > 0)
                                {

                                    IList<ChiefComplaints> Update_OR_Delete = fillCC[0].CurrentCCList.Where(A => A.HPI_Element == pair.ID.Substring(6).Replace("_", " ")).ToList<ChiefComplaints>();
                                    if (((UserControls.CustomDLCNew)pair).txtDLC.Text.Trim() == "")
                                        deleteList.Add(Update_OR_Delete[0]);
                                    else
                                    {
                                        Update_OR_Delete[0].HPI_Value = ((UserControls.CustomDLCNew)pair).txtDLC.Text.Trim();
                                        Update_OR_Delete[0].Modified_By = ClientSession.UserName;
                                        Update_OR_Delete[0].Modified_Date_And_Time = utc;
                                        updateList.Add(Update_OR_Delete[0]);
                                    }
                                }
                                else if (((UserControls.CustomDLCNew)pair).txtDLC.Text.Trim() != "")
                                {
                                    SaveMethod(pair);
                                }
                            }

                        Resultlst = objCC.UpdateComplaints(ref addList, ref updateList, deleteList.ToArray(), ref objEncounter, "");
                        Session["fillCC"] = Resultlst;
                        IList<ChiefComplaints> lstcheck = new List<ChiefComplaints>();
                        if (addList != null && addList.Count > 0)
                            lstcheck = addList;
                        if (updateList != null && updateList.Count > 0)
                            lstcheck = lstcheck.Concat(updateList).ToList();
                              }

                    if (objEncounter.Count > 0)
                    {
                        ClientSession.FillEncounterandWFObject.EncRecord = objEncounter[0];
                    }
                    IList<int> ilstChangeSummaryBar = new List<int>();
                    List<string> lstToolTip = new List<string>();

                    ilstChangeSummaryBar.Add(3);

                    var CCListCheck = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);

                    if (CCListCheck != null && CCListCheck.Count > 0)
                    {
                        CCListCheck = CCListCheck.Where(a => a.ToUpper().StartsWith("CC-")).ToList();
                        sComplaintText = CCListCheck[0].Replace("CC-", "");
                    }

                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "summaryMsg","SetUpdatedCC('" + sComplaintText.Replace("'", "&#39;") + "','" + (lstToolTip.Count > 0 ? lstToolTip[0] : string.Empty).Replace("'", "&#39;") + "' )", true);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SetUpdatedCC('" + sComplaintText.Replace("'", "&#39;").Replace("\n", "&#xA;") + "','" + (lstToolTip.Count > 0 ? lstToolTip[0] : string.Empty).Replace("'", "&#39;").Replace("\n", "&#xA;") + "' );SavedSuccessfully(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}drawChart();RefreshNotification('RcopiaMedication');", true);

                    btnAdd.Disabled = true;

                    if (HdnCopyButton.Value == "trueValidate")
                    {
                        btnCopyCC_Click(new object(), new EventArgs());
                        HdnCopyButton.Value = "";
                        btnAdd.Disabled = false;
                    }
                }
            }
            else
            {
                btnAdd.Disabled = false;
                ctmDLCChief_Complaints.txtDLC.Focus();
            }
            CheckSave.Value = "false";
        }

        public void CopyPreviousForCC(ulong HumanID, ulong EncounterID, ulong PhysicianID)
        {
            //RadWindow1.VisibleOnPageLoad = false;

            IList<FillChiefComplaint> fillCCForCopyPrevious = new List<FillChiefComplaint>();



            fillCCForCopyPrevious = objCC.GetComplaintsforPastEncounter(HumanID, EncounterID, PhysicianID);

            if (fillCCForCopyPrevious != null && fillCCForCopyPrevious.Count > 0)
            {
                if (fillCCForCopyPrevious[0].PEncID == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "onCopyPrevious('7540004');if($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == '') { $('#btnAdd')[0].disabled = false; window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true; localStorage.setItem('bSave', 'false'); }", true);
                    return;
                }
                else if (!fillCCForCopyPrevious[0].Physician_Process)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "onCopyPrevious('210016');if($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == '') { $('#btnAdd')[0].disabled = false; window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true; localStorage.setItem('bSave', 'false'); }", true);
                    return;
                }

                if (fillCCForCopyPrevious[0].CopypreviousEncounterList.Count > 0)
                {

                    ctmDLCChief_Complaints.txtDLC.Text = string.Empty;
                    ctmDLCHPI_Notes.txtDLC.Text = string.Empty;

                    for (int i = 0; i < fillCCForCopyPrevious[0].CopypreviousEncounterList.Count; i++)
                    {
                        Control txtctrl = FindControl("ctmDLC" + fillCCForCopyPrevious[0].CopypreviousEncounterList[i].HPI_Element.Replace(" ", "_"));
                        if (txtctrl != null)
                            ((UserControls.CustomDLCNew)txtctrl).txtDLC.Text = fillCCForCopyPrevious[0].CopypreviousEncounterList[i].HPI_Value;
                    }

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "onCopyPrevious('');if($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == '') { $('#btnAdd')[0].disabled = false; window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true; localStorage.setItem('bSave', 'false'); }", true);
                }
                else
                {
                    if (fillCCForCopyPrevious[0].Purpose_Of_Visit.Trim() != string.Empty)
                        ctmDLCChief_Complaints.txtDLC.Text = fillCCForCopyPrevious[0].Purpose_Of_Visit;

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "onCopyPrevious('170014');if($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == '') { $('#btnAdd')[0].disabled = false; window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true; localStorage.setItem('bSave', 'false'); }", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "onCopyPrevious('7540004');if($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == '') { $('#btnAdd')[0].disabled = false; window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true; localStorage.setItem('bSave', 'false'); }", true);
                return;
            }
        }

        protected void btnCopyCC_Click(object sender, EventArgs e)
        {
            CopyPreviousForCC(ulMyHumanID, ulMyEncounterID, ulMyPhysicianID);
        }

        protected void btnClearall_ServerClick(object sender, EventArgs e)
        {
            btnAdd.Disabled = false;
        }
    }
}
