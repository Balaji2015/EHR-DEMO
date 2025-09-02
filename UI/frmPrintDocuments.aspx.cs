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
using Acurus.Capella.Core.DTO;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmPrintDocuments : System.Web.UI.Page
    {
        PhysicianManager phyMngr = new PhysicianManager();
        UserManager UserMngr = new UserManager();
        EncounterManager encMngr = new EncounterManager();
        DocumentManager DocumentMngr = new DocumentManager();
        StaticLookupManager staticMngr = new StaticLookupManager();
        UserLookupManager userLookupMngr = new UserLookupManager();
        //CreateExceptionManager objCreateExceptionMngr = new CreateExceptionManager();
        //code Modified by balaji.TJ
        protected void Page_Load(object sender, EventArgs e)
        {           
            btnSave.Attributes.Add("onclick", "return ValidationPrintDocmove();");
            btnMovetoPhyAsst.Attributes.Add("onclick", "return ValidationPrintDoc();");

            btnmovetoscribe.Attributes.Add("onclick", "return ValidationPrintDocscribe();");
            if (!IsPostBack)
            {
                btnmovetoscribe.Visible = false;
                ClientSession.processCheck = false;
                SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
                objSecurityServiceUtility.ApplyUserPermissions(this);
                //chkDueOn.Checked = false;            
                txtNotes.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
                txtNotes.txtDLC.Attributes.Add("onclick", "EnableSave();");
                txtAddendumToPlan.Attributes.Add("onkeypress", "EnableSave();");
                txtAddendumToPlan.Attributes.Add("onclick", "EnableSave();");
                txtCorrectionToPlan.Attributes.Add("onkeypress", "EnableSave();");
                txtCorrectionToPlan.Attributes.Add("onclick", "EnableSave();");
                IList<StaticLookup> CommonList = new List<StaticLookup>();
                string StaticLookupValues = "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES";
                FillDocuments objdocumentDTO = new FillDocuments();
                string sTemp = string.Empty;
                string[] sPhyname = null;
                PhysicianLibrary PhysicianList = null;
                PhysicianLibrary LoginPhysicianList = null;
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (ClientSession.UserCurrentProcess == "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess == "PROVIDER_REVIEW_2"))
                {
                    radbtnCorrection.Enabled = true;
                }

                  if (Request["Scribe"] != null)
                {
                    radbtnCorrection.Enabled = true;
                    btnMovetoPhyAsst.Text = "Move to Scribe";
                    hdnscribe.Value = Request["Scribe"].ToString();
                    
                }
                  else
                  {
                      hdnscribe.Value = "";
                  }
                if (ClientSession.UserRole.ToUpper() == "CODER" || ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" && Request["EPROID"] != null && Request["EPROID"] != "")
                {
                    objdocumentDTO = DocumentMngr.GetAllPrintDocumentDetails(StaticLookupValues, Convert.ToUInt64(Request["EPROID"]), "PATIENT EDUCATION DOCUMENTS", Convert.ToUInt64(ClientSession.PhysicianId), ClientSession.EncounterId);
                    IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                    ilstPhysicianLibrary = objdocumentDTO.PhysicianLibraryList;
                    if (ilstPhysicianLibrary != null && ilstPhysicianLibrary.Count > 0)
                        PhysicianList = ilstPhysicianLibrary[0];
                }
                else if (ClientSession.UserRole.ToUpper() != "MEDICAL ASSISTANT" && Request["EPROID"] != null && Request["EPROID"] != "")
                {
                    if (ClientSession.CurrentPhysicianId != 0)
                    {
                        IList<PhysicianLibrary> LoginPhysicianLists = new List<PhysicianLibrary>();
                        objdocumentDTO = DocumentMngr.GetAllPrintDocumentDetails(StaticLookupValues, Convert.ToUInt64(Request["EPROID"]), "PATIENT EDUCATION DOCUMENTS", Convert.ToUInt64(ClientSession.CurrentPhysicianId), ClientSession.EncounterId);
                        LoginPhysicianLists = objdocumentDTO.PhysicianLibraryList;
                        if (LoginPhysicianLists != null && LoginPhysicianLists.Count > 0)
                            LoginPhysicianList = LoginPhysicianLists[0];
                    }
                    if (ClientSession.PhysicianId != 0)
                    {
                        IList<PhysicianLibrary> PhysicianLists = new List<PhysicianLibrary>();
                        objdocumentDTO = DocumentMngr.GetAllPrintDocumentDetails(StaticLookupValues, Convert.ToUInt64(Request["EPROID"]), "PATIENT EDUCATION DOCUMENTS", Convert.ToUInt64(ClientSession.PhysicianId), ClientSession.EncounterId);
                        PhysicianLists = objdocumentDTO.PhysicianLibraryList;
                        if (PhysicianLists != null && PhysicianLists.Count > 0)
                            PhysicianList = PhysicianLists[0];
                    }
                }
                cboRelationship.Enabled = false;
                cboIsDocumentGiven.Enabled = false;
                txtGivenTo.Enabled = false;
                txtGivenTo.CssClass = "nonEditabletxtbox";

                if (Request["DS"] != null)
                {
                    DigitalSign.Value = Request["DS"].ToString();
                    if (Convert.ToBoolean(Request["DS"]) == false)
                    {
                        pnlElectronicSignature.Enabled = false;
                        pnlAddendum.Enabled = false;
                        trAlignment.Visible = true;
                        trPanel1.Visible = true;
                        trpnlElectronicSignature.Visible = true;
                        trpnlAddendum.Visible = true;
                        btnSave.Text = "Save";
                        btnMovetoPhyAsst.Visible = false;
                        chkSurgeryDeclaration.Visible = false;
                        ProceedwithSurgeryasPlanned.Visible = false;
                    }
                }
                // FillDocuments objdocument = new FillDocuments();
                radbtnAgreePlan.Checked = true;//BugID:52803
                if (radbtnAgreePlan.Checked == true)
                    txtAddendumToPlan.Enabled = false;
                Encounter EncRecord = new Encounter();
                IList<StaticLookup> StaticLst = null;
                IList<UserLookup> FieldLookUpList = new List<UserLookup>();
                if (objdocumentDTO.lstUserLookup != null)
                {
                    FieldLookUpList = objdocumentDTO.lstUserLookup;
                    FieldLookUpList = FieldLookUpList.OrderBy(a => a.Value).ToList();
                    if (FieldLookUpList != null)
                    {
                        chklstSelectDocuments.Items.Clear();
                        if (FieldLookUpList.Count > 0)
                        {
                            for (int i = 0; i < FieldLookUpList.Count; i++)
                            {
                                ListItem item = new ListItem();
                                item.Text = FieldLookUpList[i].Value;
                                item.Value = FieldLookUpList[i].Description;
                                this.chklstSelectDocuments.Items.Add(item);
                            }
                        }
                    }
                }
                if (chklstSelectDocuments.Items.Count > 0)
                {
                    if (objdocumentDTO.DocumentsList != null && objdocumentDTO.DocumentsList.Count > 0)
                    {
                        for (int i = 0; i < objdocumentDTO.DocumentsList.Count; i++)
                        {
                            for (int j = 0; j < chklstSelectDocuments.Items.Count; j++)
                            {
                                if (chklstSelectDocuments.Items[j].Text.Trim() == objdocumentDTO.DocumentsList[i].Document_Type.Trim())
                                    chklstSelectDocuments.Items[j].Selected = true;
                            }
                        }
                    }
                }
                 if (Request["Scribe"] != null)
                {
                   
                    radbtnCorrection.Text = "Corrections to be Made by Scribe";
                    radbtnCorrection.Checked = true;
                    

                }
                ViewState["FillDocuments"] = objdocumentDTO;
                if (objdocumentDTO != null && objdocumentDTO.EncounterObj != null)
                {
                    //if (objdocumentDTO.EncounterObj.Due_On != DateTime.MinValue)
                    //{
                    //    if (objdocumentDTO.EncounterObj.Return_In_Days == 0 && objdocumentDTO.EncounterObj.Return_In_Months == 0 && objdocumentDTO.EncounterObj.Return_In_Weeks == 0)
                    //    {
                    //        chkDueOn.Checked = true;
                    //        txtDueon.Enabled = true;
                    //        txtDueon.Visible = true;
                    //        txtDueon.SelectedDate = (objdocumentDTO.EncounterObj.Due_On);                            
                    //    }
                    //    if (chkDueOn.Checked == false)
                    //    {
                    //        txtDueon.Attributes.Add("readonly", "readonly");
                    //        txtDueon.Attributes.Add("disabled", "disabled");
                    //        txtDueon.Enabled = false;
                    //    }
                    //}
                    if (objdocumentDTO.EncounterObj.Follow_Reason_Notes != string.Empty)
                        txtNotes.txtDLC.Text = objdocumentDTO.EncounterObj.Follow_Reason_Notes;

                    if (objdocumentDTO.EncounterObj.Return_In_Weeks != 0)
                    {
                        chkReturnIn.Checked = true;
                        //chkDueOn.Checked = false;
                        txtReturnIn.Enabled = true;
                        txtRetrunWeeks.Enabled = true;
                        txtRetrunDays.Enabled = true;
                        txtReturnIn.CssClass = "Editabletxtbox";
                        txtRetrunWeeks.CssClass = "Editabletxtbox";
                        txtRetrunDays.CssClass = "Editabletxtbox";
                        //txtDueon.Attributes.Add("disabled", "disabled");
                        txtRetrunWeeks.Text = Convert.ToString(objdocumentDTO.EncounterObj.Return_In_Weeks);
                    }

                    if (objdocumentDTO.EncounterObj.Return_In_Months != 0)
                    {
                        chkReturnIn.Checked = true;
                        //chkDueOn.Checked = false;
                        txtReturnIn.Enabled = true;
                        txtRetrunWeeks.Enabled = true;
                        txtRetrunDays.Enabled = true;
                        txtReturnIn.CssClass = "Editabletxtbox";
                        txtRetrunWeeks.CssClass = "Editabletxtbox";
                        txtRetrunDays.CssClass = "Editabletxtbox";
                        //txtDueon.Attributes.Add("disabled", "disabled");
                        txtReturnIn.Text = Convert.ToString(objdocumentDTO.EncounterObj.Return_In_Months);
                    }

                    if (objdocumentDTO.EncounterObj.Return_In_Days != 0)
                    {
                        chkReturnIn.Checked = true;
                        //chkDueOn.Checked = false;
                        txtReturnIn.Enabled = true;
                        txtRetrunWeeks.Enabled = true;
                        txtRetrunDays.Enabled = true;
                        txtReturnIn.CssClass = "Editabletxtbox";
                        txtRetrunWeeks.CssClass = "Editabletxtbox";
                        txtRetrunDays.CssClass = "Editabletxtbox";
                        //txtDueon.Attributes.Add("disabled", "disabled");
                        txtRetrunDays.Text = Convert.ToString(objdocumentDTO.EncounterObj.Return_In_Days);
                    }
                    if (chkReturnIn.Checked == false)
                    {
                        txtReturnIn.Attributes.Add("disabled", "disabled");
                        txtRetrunDays.Attributes.Add("disabled", "disabled");
                        txtRetrunWeeks.Attributes.Add("disabled", "disabled");

                        txtReturnIn.CssClass = "nonEditabletxtbox";
                        txtRetrunWeeks.CssClass = "nonEditabletxtbox";
                        txtRetrunDays.CssClass = "nonEditabletxtbox";
                    }
                }
                if (objdocumentDTO != null && objdocumentDTO.Treatment_Plan_List != null && objdocumentDTO.Treatment_Plan_List.Count > 0
                    && objdocumentDTO.Treatment_Plan_List[0].Id != 0)
                {
                    if ((objdocumentDTO.Treatment_Plan_List.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type.ToUpper().Equals(radbtnCorrection.Text.ToUpper()))).ToList().Count > 0)
                    {
                        IList<TreatmentPlan> ilstTreatementplan = new List<TreatmentPlan>();
                        ilstTreatementplan = objdocumentDTO.Treatment_Plan_List.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type.ToUpper().Equals(radbtnCorrection.Text.ToUpper())).ToList<TreatmentPlan>();
                        radbtnAgreePlan.Checked = false;
                        radbtnAgreewithChanges.Checked = false;
                        radbtnCorrection.Checked = true;
                        txtAddendumToPlan.Text = ilstTreatementplan[0].Addendum_Plan;
                        txtCorrectionToPlan.Text = ilstTreatementplan[0].Corrections_to_be_made;
                    }
                    else if ((objdocumentDTO.Treatment_Plan_List.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type.ToUpper().Equals(radbtnAgreewithChanges.Text.ToUpper()))).ToList().Count > 0)
                    {
                        IList<TreatmentPlan> ilstTreatementplan = new List<TreatmentPlan>();
                        ilstTreatementplan = objdocumentDTO.Treatment_Plan_List.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type.ToUpper().Equals(radbtnAgreewithChanges.Text.ToUpper())).ToList<TreatmentPlan>();
                        radbtnAgreePlan.Checked = false;
                        radbtnAgreewithChanges.Checked = true;
                        radbtnCorrection.Checked = false;
                        txtAddendumToPlan.Text = ilstTreatementplan[0].Addendum_Plan;
                        txtCorrectionToPlan.Text = ilstTreatementplan[0].Corrections_to_be_made;
                    }
                    else if ((objdocumentDTO.Treatment_Plan_List.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type.ToUpper().Equals(radbtnAgreePlan.Text.ToUpper()))).ToList().Count > 0)
                    {
                        IList<TreatmentPlan> ilstTreatementplan = new List<TreatmentPlan>();
                        ilstTreatementplan = objdocumentDTO.Treatment_Plan_List.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type.ToUpper().Equals(radbtnAgreePlan.Text.ToUpper())).ToList<TreatmentPlan>();
                        radbtnAgreePlan.Checked = true;
                        radbtnAgreewithChanges.Checked = false;
                        radbtnCorrection.Checked = false;
                        txtAddendumToPlan.Text = ilstTreatementplan[0].Addendum_Plan;
                        txtCorrectionToPlan.Text = ilstTreatementplan[0].Corrections_to_be_made;
                    }
                    else
                    {
                        radbtnAgreePlan.Checked = true;
                        radbtnAgreewithChanges.Checked = false;
                        radbtnCorrection.Checked = false;
                        txtAddendumToPlan.Text = "";
                        txtCorrectionToPlan.Text = "";
                    }



                    //txtAddendumToPlan.Text = objdocumentDTO.Treatment_Plan_List[0].Addendum_Plan;
                    //txtCorrectionToPlan.Text = objdocumentDTO.Treatment_Plan_List[0].Corrections_to_be_made;
                    //if(objdocumentDTO.Treatment_Plan_List[0].Amendment_Type.ToUpper().Equals(radbtnAgreePlan.Text.ToUpper()))
                    //{
                    //    radbtnAgreePlan.Checked = true;
                    //    radbtnAgreewithChanges.Checked = false;
                    //    radbtnCorrection.Checked = false;
                    //}
                    //if (objdocumentDTO.Treatment_Plan_List[0].Amendment_Type.ToUpper().Equals(radbtnAgreewithChanges.Text.ToUpper()))
                    //{
                    //    radbtnAgreePlan.Checked = false;
                    //    radbtnAgreewithChanges.Checked = true;
                    //    radbtnCorrection.Checked = false;
                    //}
                    //if (objdocumentDTO.Treatment_Plan_List[0].Amendment_Type.ToUpper().Equals(radbtnCorrection.Text.ToUpper()))
                    //{
                    //    radbtnAgreePlan.Checked = false;
                    //    radbtnAgreewithChanges.Checked = false;
                    //    radbtnCorrection.Checked = true;
                    //}
                }
                if (objdocumentDTO != null)
                    userName.Value = objdocumentDTO.EncPhyuserName.ToString();
                if (objdocumentDTO != null && objdocumentDTO.DocumentsList != null && objdocumentDTO.DocumentsList.Count > 0)
                {
                    cboRelationship.SelectedIndex = cboRelationship.Items.FindItemByText(objdocumentDTO.DocumentsList[0].Relationship).Index;
                    txtGivenTo.Text = objdocumentDTO.DocumentsList[0].Given_To;
                }
                if (objdocumentDTO != null && objdocumentDTO.EncounterObj != null)
                {
                    EncRecord = objdocumentDTO.EncounterObj;
                    if (EncRecord.Is_Document_Given != string.Empty)
                    {
                        if (cboIsDocumentGiven.Items.Count > 0)
                            cboIsDocumentGiven.Items.FindItemByText(EncRecord.Is_Document_Given).Selected = true;
                    }
                    if (EncRecord.Proceed_with_Surgery_Planned=="Y")
                    {
                        chkSurgeryDeclaration.Checked = true;
                    }
                }
                else
                {
                    if (Request["HNAME"] != null)
                        txtGivenTo.Text = Request["HNAME"].ToString();
                }
                if (EncRecord != null)
                {
                    if (EncRecord.Is_PFSH_Verified.ToUpper() == "Y")
                    {
                        EncRecord = objdocumentDTO.EncounterObj;
                        if (EncRecord.Is_Document_Given != string.Empty)
                            cboIsDocumentGiven.Items.FindItemByText(EncRecord.Is_Document_Given).Selected = true;
                    }
                    if (EncRecord != null)
                    {
                        if (EncRecord.Is_PFSH_Verified.ToUpper() == "Y")
                        {
                            chkPFSHVerified.Checked = true;
                            chkPFSHVerified.Enabled = false;
                        }
                        else
                        {
                            chkPFSHVerified.Checked = true;
                            chkPFSHVerified.Enabled = true;
                        }
                    }
                    CommonList = objdocumentDTO.lstStaticLookup;
                    if (CommonList != null)
                        StaticLst = CommonList.Where(a => a.Field_Name == "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES").ToList<StaticLookup>();
                    if (ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
                    {
                        //if (StaticLst != null && StaticLst.Count > 0 && StaticLst[0].Value.Contains('|')) //code added by balaji.TJ 2015-11-28
                        //{
                        //    sPhyname = StaticLst[0].Value.Split('|');
                        //    //code Modified by balaji.TJ 2015-11-28
                        //    if (sPhyname != null && sPhyname.Length > 0)
                        //    {
                        //        sTemp = sPhyname[0].Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
                        //        sTemp = sTemp.Replace("<Date>", "");
                        //        sTemp = sTemp.Replace(" on ", "");
                        //        chkElectronicDeclaration.Text = sTemp;
                        //    }
                        //}

                        StaticLst = staticMngr.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN");

                        if (StaticLst != null & StaticLst.Count > 0)
                            sTemp = StaticLst[0].Value.Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");

                        sTemp = sTemp.Replace("<Date>", "");
                        sTemp = sTemp.Replace(" on ", "");
                        sTemp = sTemp.Replace(":", "");

                        chkElectronicDeclaration.Text = sTemp;
                    }
                    else if (ClientSession.UserCurrentProcess == "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess == "PROVIDER_REVIEW_2")
                    {
                        if (ClientSession.FillEncounterandWFObject != null && ClientSession.FillEncounterandWFObject.EncRecord!= null && ClientSession.FillEncounterandWFObject.EncRecord.Assigned_Scribe_User_Name.Trim() != string.Empty)
                        btnmovetoscribe.Visible = true;
                        if (StaticLst != null && StaticLst[0].Value.Contains('|')) //code added by balaji.TJ 2015-11-28
                            sPhyname = StaticLst[0].Value.Split('|');
                        //code Modified by balaji.TJ 2015-11-28
                        if (sPhyname != null && sPhyname.Length > 0)
                        {
                            sTemp = sPhyname[0].Replace("<Physician>", LoginPhysicianList != null ? LoginPhysicianList.PhyPrefix + " " + LoginPhysicianList.PhyFirstName + " " + LoginPhysicianList.PhyMiddleName + " " + LoginPhysicianList.PhyLastName + " " + LoginPhysicianList.PhySuffix : ""); //sPhyname[0].Replace("<Physician>", PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix);
                            string sReplace = sPhyname[1].Replace("<Date>", "").Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
                            sReplace = sReplace.Replace(" on ", "");
                            chkElectronicDeclaration.Text = sTemp + sReplace;
                        }
                        //btnSave.Text = "Sign and Save";
                    }
                    else
                    {
                        StaticLst = staticMngr.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN");

                        //if (StaticLst != null && StaticLst[0].Value.Contains('|')) //code added by balaji.TJ 2015-11-28
                        //    sPhyname = StaticLst[0].Value.Split('|');

                        //code Modified by balaji.TJ 2015-11-28
                        //if (sPhyname != null && sPhyname.Length > 0)
                        //{
                        //    sTemp = sPhyname[0].Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
                        //    sTemp = sTemp.Replace("<Date>", "");
                        //    sTemp = sTemp.Replace(" on ", "");
                        //    sTemp = sTemp.Replace(":", "");
                        //}

                        if (StaticLst != null & StaticLst.Count>0)
                            sTemp = StaticLst[0].Value.Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
                      
                        sTemp = sTemp.Replace("<Date>", "");
                        sTemp = sTemp.Replace(" on ", "");
                        sTemp = sTemp.Replace(":", "");

                        chkElectronicDeclaration.Text = sTemp;
                    }
                    //txtDueon.MinDate = DateTime.Today.Date;
                    //if (txtDueon.SelectedDate <= UtilityManager.ConvertToLocal(Convert.ToDateTime(EncRecord.Due_On)))
                    //    txtDueon.SelectedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(EncRecord.Due_On));
                    //else if (Convert.ToString(objdocumentDTO.EncounterObj.Return_In_Months) != "" || Convert.ToString(objdocumentDTO.EncounterObj.Return_In_Weeks) != "" || Convert.ToString(objdocumentDTO.EncounterObj.Return_In_Days) != "")
                    //{
                    //    txtDueon.MinDate = DateTime.Today.Date;
                    //    txtDueon.Enabled = false;                        
                    //    chkDueOn.Checked = false;
                    //}
                    //else
                    //{
                    //    txtDueon.MinDate = DateTime.Today.Date;
                    //    txtDueon.SelectedDate = DateTime.Now;
                    //    chkDueOn.Checked = true;
                    //}
                }


                chkSurgeryDeclaration.Visible = false;
                ProceedwithSurgeryasPlanned.Visible = false;
                XDocument xmlDocumentType = null;
                //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml")))
                //    xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\
                //    PhysicianFacilityMapping.xml"));
                //ListItem liDropdown = null;
                IList<ListItem> liComboItems = new List<ListItem>();
                //CAP-2781
                PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
                if (physicianFacilityMappingList != null)
                {
                    foreach (var facility in physicianFacilityMappingList.PhysicianFacility)
                    {
                        string xmlValue = facility.name;
                        if (ClientSession.FacilityName != null)
                        {
                            if (xmlValue.ToUpper().StartsWith("SURGERY-") == true)
                            {
                                foreach (var phyItems in facility.Physician)
                                {
                                    string phyName = string.Empty;
                                    string username = string.Empty;
                                    string phyID = string.Empty;

                                    if (phyItems.username != null)
                                        username = phyItems.username;

                                    if (username == ClientSession.UserName)
                                    {
                                        chkSurgeryDeclaration.Visible = true;
                                        ProceedwithSurgeryasPlanned.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (ClientSession.UserCurrentProcess.Trim().ToUpper() == "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess.Trim().ToUpper() == "PROVIDER_REVIEW_2")
                {
                    
                   
                    if (ClientSession.FillEncounterandWFObject != null && ClientSession.FillEncounterandWFObject.EncRecord != null && ClientSession.FillEncounterandWFObject.EncRecord.Assigned_Scribe_User_Name.Trim() != string.Empty)
                    {
                        radbtnAgreePlan.Checked = true;
                        txtCorrectionToPlan.Text = "";
                        chkElectronicDeclaration.Enabled = true;
                        radbtnCorrection.Checked = false;
                    }



                }
            }

            if (ClientSession.UserCurrentProcess.Trim().ToUpper() == "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess.Trim().ToUpper() == "PROVIDER_REVIEW_2")
            {
                //btnSave.Text = "Sign and Save";
                if (radbtnAgreewithChanges.Checked)               
                    txtAddendumToPlan.Enabled = true;
               
                       


            }
            else if (Request["Scribe"] != null)
            {
                if (radbtnAgreewithChanges.Checked)
                    txtAddendumToPlan.Enabled = true;
                radbtnCorrection.Enabled = true;
                radbtnCorrection.Checked = true;
                txtAddendumToPlan.Enabled = false;
                radbtnCorrection.Text = "Corrections to be Made by Scribe";
                btnMovetoPhyAsst.Text = "Move to Scribe";

            }
            else
            {
                btnSave.Text = "Save";
                tdBtnMovetoPhyAsst.Visible = false;
                btnMovetoPhyAsst.Visible = false;
                chkSurgeryDeclaration.Visible = false;
                ProceedwithSurgeryasPlanned.Visible = false;
            }
            if (radbtnAgreewithChanges.Checked)
                txtAddendumToPlan.Enabled = true;
            else
                txtAddendumToPlan.Enabled = false;
            if (radbtnAgreePlan.Checked || radbtnAgreewithChanges.Checked)
                btnSave.Enabled = true;
            if (radbtnCorrection.Checked)
                btnMovetoPhyAsst.Enabled = true;
            //Added for Provider_Review PhysicianAssistant WorkFlow Change
            if (radbtnCorrection.Checked)
                txtCorrectionToPlan.Enabled = true;
            else
                txtCorrectionToPlan.Enabled = false;
            if (Request["OPENING_FROM"] != null && Request["OPENING_FROM"] == "ProcessEncounter" && (ClientSession.UserCurrentProcess.Trim().ToUpper() == "PROVIDER_REVIEW_CORRECTION" || ClientSession.UserCurrentProcess.Trim().ToUpper() == "SCRIBE_CORRECTION" || ClientSession.UserCurrentProcess.Trim().ToUpper() == "SCRIBE_REVIEW_CORRECTION"))
            {
                pnlFollowUp.Visible= false;
                pnlDocuments.Visible = false;
                Alignment.Visible = false;
                Panel1.Visible = false;
                pnlElectronicSignature.Visible = false;
                radbtnAgreePlan.Visible = false;
                radbtnAgreewithChanges.Visible = false;
                txtAddendumToPlan.Visible = false;
                btnPrint.Visible = false;
                btnSave.Visible = false;
                btnMovetoPhyAsst.Visible = false;
                radbtnCorrection.Enabled = false;
                txtCorrectionToPlan.Enabled = false;
                btnClose.Text = "Close";
            }

            //Jira CAP-2581
            if (ClientSession.LegalOrg.ToUpper() == "CMG")
            {
                radbtnCorrection.Enabled = false;
                txtCorrectionToPlan.Enabled = false;
            }
            
        }
        #region
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    //txtDueon.MinDate = DateTime.Today.Date; //code added by balaji.TJ 2015-11-23
        //    //btnSave.Attributes.Add("onclick", " PrintDocumentwaitcursor();");
        //    btnSave.Attributes.Add("onclick", "return ValidationPrintDoc();");
        //    if (!IsPostBack)
        //    {
        //        ClientSession.processCheck = false;
        //        SecurityServiceUtility objSecurityServiceUtility = new SecurityServiceUtility();
        //        objSecurityServiceUtility.ApplyUserPermissions(this);
        //        chkDueOn.Checked = false;
        //        // cboReturnIn.Enabled = false;
        //        //dtpDueon.Enabled = false;
        //        // dtpDueon.MinDate = DateTime.Now;
        //        // txtDueon.SelectedDate = DateTime.Now;
        //        //  txtDueon.MinDate = DateTime.Now;
        //        //btnSave.Attributes.Add("onclick", "return ValidationPrintDoc();"); comment by balaji.TJ 2015-12-23
        //        txtNotes.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
        //        txtNotes.txtDLC.Attributes.Add("onclick", "EnableSave();");
        //        txtAddendumToPlan.Attributes.Add("onkeypress", "EnableSave();");
        //        txtAddendumToPlan.Attributes.Add("onclick", "EnableSave();");

        //        //txtNotes.txtDLC.Attributes.Add("onchange", "ButtonEnable();");
        //        //code modified by balaji.TJ 2015-11-26
        //        string[] StaticLookupValues = new string[] { "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
        //        //string[] StaticLookupValues = new string[] { "RETURN IN", "SOURCE OF INFORMATION", "DOCUMENT GIVEN", "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES" };
        //        IList<StaticLookup> CommonList = staticMngr.getStaticLookupByFieldName(StaticLookupValues);
        //        //IList<StaticLookup> ReturnInList = CommonList.Where(a => a.Field_Name == "RETURN IN").ToList<StaticLookup>();
        //        //foreach (StaticLookup obj in ReturnInList)
        //        //{
        //        //    RadComboBoxItem item = new RadComboBoxItem();
        //        //    item.Text = obj.Value;
        //        //   // cboReturnIn.Items.Add(item);
        //        //}
        //        cboRelationship.Enabled = false;
        //        cboIsDocumentGiven.Enabled = false;
        //        txtGivenTo.Enabled = false;

        //        if (Request["DS"] != null)
        //        {
        //            DigitalSign.Value = Request["DS"].ToString();
        //            if (Convert.ToBoolean(Request["DS"]) == false)
        //            {
        //                pnlElectronicSignature.Enabled = false;
        //                pnlAddendum.Enabled = false;
        //                trAlignment.Visible = true;
        //                trPanel1.Visible = true;
        //                trpnlElectronicSignature.Visible = true;
        //                trpnlAddendum.Visible = true;
        //                //trpnlAddendum.Style.Add("display", "none");
        //                btnSave.Text = "Save";
        //                // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Printdocumentwindow();", true);
        //            }
        //        }
        //        FillDocuments objdocument = new FillDocuments();
        //        radbtnAgreePlan.Checked = true;
        //        if (radbtnAgreePlan.Checked == true)
        //            txtAddendumToPlan.Enabled = false;
        //        //CalendarExtender1.Format = "dd-MMM-yyyy";
        //        Encounter EncRecord = new Encounter();
        //        //CalendarExtender1.StartDate = DateTime.Now;
        //        IList<StaticLookup> StaticLst = null;
        //        //code added by balaji.TJ 2015-26-11
        //        IList<UserLookup> FieldLookUpList = new List<UserLookup>();
        //        if (Request["EPROID"] != null && Request["EPROID"] != "")
        //            FieldLookUpList = userLookupMngr.GetFieldLookupList(Convert.ToUInt64(Request["EPROID"]), "PATIENT EDUCATION DOCUMENTS");
        //        FieldLookUpList = FieldLookUpList.OrderBy(a => a.Value).ToList();
        //        if (FieldLookUpList != null)
        //        {
        //            chklstSelectDocuments.Items.Clear();
        //            if (FieldLookUpList.Count > 0)
        //            {
        //                for (int i = 0; i < FieldLookUpList.Count; i++)
        //                {
        //                    ListItem item = new ListItem();
        //                    item.Text = FieldLookUpList[i].Value;
        //                    item.Value = FieldLookUpList[i].Description;
        //                    this.chklstSelectDocuments.Items.Add(item);
        //                }
        //            }
        //        }
        //        //StaticLst = CommonList.Where(a => a.Field_Name == "SOURCE OF INFORMATION").ToList<StaticLookup>();
        //        //if (StaticLst != null)
        //        //{
        //        //    cboRelationship.Items.Clear();
        //        //    cboRelationship.Items.Add(new RadComboBoxItem(""));
        //        //    for (int i = 0; i < StaticLst.Count; i++)
        //        //    {
        //        //        cboRelationship.Items.Add(new RadComboBoxItem(StaticLst[i].Value));

        //        //    }
        //        //}
        //        IList<string> SelectedDocuments = chklstSelectDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text).ToList<string>();
        //        //StaticLst = CommonList.Where(a => a.Field_Name == "DOCUMENT GIVEN").ToList<StaticLookup>();
        //        //if (StaticLst != null && StaticLst.Count > 0)
        //        //{
        //        ////    for (int i = 0; i < StaticLst.Count; i++)
        //        ////    {
        //        ////        cboIsDocumentGiven.Items.Add(new RadComboBoxItem(StaticLst[i].Value));

        //        ////    }
        //        //    cboIsDocumentGiven.Text = StaticLst[0].Value;
        //        //}
        //        //added by balaji.T
        //        objdocument = DocumentMngr.GetDocumentsList(ClientSession.EncounterId);
        //        if (chklstSelectDocuments.Items.Count > 0)
        //        {
        //            if (objdocument.DocumentsList.Count > 0)
        //            {
        //                for (int i = 0; i < objdocument.DocumentsList.Count; i++)
        //                {
        //                    //chklstSelectDocuments.SelectedItem.Text = objdocument.DocumentsList[i].Document_Type;
        //                    for (int j = 0; j < chklstSelectDocuments.Items.Count; j++)
        //                    {
        //                        if (chklstSelectDocuments.Items[j].Text.Trim() == objdocument.DocumentsList[i].Document_Type.Trim())
        //                            chklstSelectDocuments.Items[j].Selected = true;
        //                    }
        //                }
        //            }
        //        }
        //        ViewState["FillDocuments"] = objdocument;
        //        if (objdocument != null && objdocument.EncounterObj != null)
        //        {
        //            if (objdocument.EncounterObj.Due_On != DateTime.MinValue)
        //            {
        //                if (objdocument.EncounterObj.Return_In_Days == 0 && objdocument.EncounterObj.Return_In_Months == 0 && objdocument.EncounterObj.Return_In_Weeks == 0)
        //                {
        //                    chkDueOn.Checked = true;
        //                    txtDueon.Enabled = true;
        //                    txtDueon.Visible = true;
        //                    txtDueon.SelectedDate = (objdocument.EncounterObj.Due_On);
        //                }
        //                if (chkDueOn.Checked == false)
        //                {
        //                    txtDueon.Attributes.Add("readonly", "readonly");
        //                    txtDueon.Attributes.Add("disabled", "disabled");
        //                    txtDueon.Enabled = false;
        //                }
        //            }
        //            if (objdocument.EncounterObj.Follow_Reason_Notes != string.Empty)
        //                txtNotes.txtDLC.Text = objdocument.EncounterObj.Follow_Reason_Notes;

        //            if (objdocument.EncounterObj.Return_In_Weeks != 0)
        //            {
        //                chkReturnIn.Checked = true;
        //                chkDueOn.Checked = false;
        //                txtReturnIn.Enabled = true;
        //                txtRetrunWeeks.Enabled = true;
        //                txtRetrunDays.Enabled = true;
        //                //cboReturnIn.Enabled = true;
        //                txtDueon.Attributes.Add("disabled", "disabled");
        //                txtRetrunWeeks.Text = Convert.ToString(objdocument.EncounterObj.Return_In_Weeks);
        //                //cboReturnIn.SelectedIndex = cboReturnIn.Items.FindItemByText("Weeks").Index;
        //            }

        //            if (objdocument.EncounterObj.Return_In_Months != 0)
        //            {
        //                chkReturnIn.Checked = true;
        //                chkDueOn.Checked = false;
        //                txtReturnIn.Enabled = true;
        //                txtRetrunWeeks.Enabled = true;
        //                txtRetrunDays.Enabled = true;
        //                //cboReturnIn.Enabled = true;
        //                txtDueon.Attributes.Add("disabled", "disabled");
        //                txtReturnIn.Text = Convert.ToString(objdocument.EncounterObj.Return_In_Months);
        //                //cboReturnIn.SelectedIndex = cboReturnIn.Items.FindItemByText("Months").Index;
        //            }

        //            if (objdocument.EncounterObj.Return_In_Days != 0)
        //            {
        //                chkReturnIn.Checked = true;
        //                chkDueOn.Checked = false;
        //                txtReturnIn.Enabled = true;
        //                txtRetrunWeeks.Enabled = true;
        //                txtRetrunDays.Enabled = true;
        //                // cboReturnIn.Enabled = true;
        //                txtDueon.Attributes.Add("disabled", "disabled");
        //                txtRetrunDays.Text = Convert.ToString(objdocument.EncounterObj.Return_In_Days);
        //                //cboReturnIn.SelectedIndex = cboReturnIn.Items.FindItemByText("Days").Index;
        //            }
        //            if (chkReturnIn.Checked == false)
        //            {
        //                txtReturnIn.Attributes.Add("disabled", "disabled");
        //                txtRetrunDays.Attributes.Add("disabled", "disabled");
        //                txtRetrunWeeks.Attributes.Add("disabled", "disabled");
        //            }

        //        }
        //        if (objdocument != null)
        //            userName.Value = objdocument.EncPhyuserName.ToString();
        //        if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
        //        {
        //            cboRelationship.SelectedIndex = cboRelationship.Items.FindItemByText(objdocument.DocumentsList[0].Relationship).Index;
        //            txtGivenTo.Text = objdocument.DocumentsList[0].Given_To;
        //        }
        //        if (objdocument != null && objdocument.EncounterObj != null)
        //        {
        //            EncRecord = objdocument.EncounterObj;
        //            if (EncRecord.Is_Document_Given != string.Empty)
        //            {
        //                //cboIsDocumentGiven.Text = EncRecord.Is_Document_Given;
        //                if (cboIsDocumentGiven.Items.Count > 0)
        //                    cboIsDocumentGiven.Items.FindItemByText(EncRecord.Is_Document_Given).Selected = true;
        //            }

        //        }
        //        else
        //        {
        //            if (Request["HNAME"] != null)
        //                txtGivenTo.Text = Request["HNAME"].ToString();
        //        }
        //        if (EncRecord != null)
        //        {
        //            if (EncRecord.Is_PFSH_Verified.ToUpper() == "Y")
        //            {
        //                EncRecord = objdocument.EncounterObj;
        //                if (EncRecord.Is_Document_Given != string.Empty)
        //                    // cboIsDocumentGiven.Text = EncRecord.Is_Document_Given;
        //                    cboIsDocumentGiven.Items.FindItemByText(EncRecord.Is_Document_Given).Selected = true;
        //            }
        //            if (EncRecord != null)
        //            {
        //                if (EncRecord.Is_PFSH_Verified.ToUpper() == "Y")
        //                {
        //                    chkPFSHVerified.Checked = true;
        //                    chkPFSHVerified.Enabled = false;
        //                }
        //                else
        //                {
        //                    chkPFSHVerified.Checked = true;
        //                    chkPFSHVerified.Enabled = true;
        //                }
        //            }
        //            string sTemp = string.Empty;
        //            string[] sPhyname = null;
        //            PhysicianLibrary PhysicianList = null;
        //            PhysicianLibrary LoginPhysicianList = null;
        //            if (CommonList != null)
        //                StaticLst = CommonList.Where(a => a.Field_Name == "WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES").ToList<StaticLookup>();
        //            if (ClientSession.UserRole.ToUpper() == "CODER" || ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
        //            {
        //                // PhysicianList = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ClientSession.PhysicianId))[0]; //code comment by balaji.TJ 2015-11-28
        //                //code Added by balaji.TJ 2015-11-28
        //                IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
        //                ilstPhysicianLibrary = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ClientSession.PhysicianId));
        //                if (ilstPhysicianLibrary.Count > 0)
        //                    PhysicianList = ilstPhysicianLibrary[0];
        //            }
        //            else if (ClientSession.UserRole.ToUpper() != "MEDICAL ASSISTANT")
        //            {
        //                //changed by srividhya on 3-dec-2015==changed from ClientSession.PhysicianId to ClientSession.CurrentPhysicianId in the below if condition. bug id:29150.
        //                if (ClientSession.CurrentPhysicianId != 0)
        //                {
        //                    //LoginPhysicianList = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ClientSession.CurrentPhysicianId))[0]; //code comment by balaji.TJ 2015-11-28
        //                    //code Added by balaji.TJ 2015-11-28
        //                    IList<PhysicianLibrary> LoginPhysicianLists = new List<PhysicianLibrary>();
        //                    LoginPhysicianLists = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ClientSession.CurrentPhysicianId));
        //                    if (LoginPhysicianLists.Count > 0)
        //                        LoginPhysicianList = LoginPhysicianLists[0];
        //                }
        //                if (ClientSession.PhysicianId != 0)
        //                {
        //                    //PhysicianList = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ClientSession.PhysicianId))[0]; //code comment by balaji.TJ 2015-11-28
        //                    //code Added by balaji.TJ 2015-11-28
        //                    IList<PhysicianLibrary> PhysicianLists = new List<PhysicianLibrary>();
        //                    PhysicianLists = phyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(ClientSession.PhysicianId));
        //                    if (PhysicianLists.Count > 0)
        //                        PhysicianList = PhysicianLists[0];
        //                }
        //            }
        //            if (ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
        //            {
        //                if (StaticLst != null && StaticLst[0].Value.Contains('|')) //code added by balaji.TJ 2015-11-28
        //                {
        //                    sPhyname = StaticLst[0].Value.Split('|');
        //                    //code Modified by balaji.TJ 2015-11-28
        //                    if (sPhyname != null && sPhyname.Length > 0)
        //                    {
        //                        sTemp = sPhyname[1].Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
        //                        sTemp = sTemp.Replace("<Date>", "");
        //                        sTemp = sTemp.Replace(" on ", "");
        //                        chkElectronicDeclaration.Text = sTemp;
        //                    }
        //                }
        //            }
        //            else if (ClientSession.UserCurrentProcess == "PROVIDER_REVIEW")
        //            {
        //                if (StaticLst != null && StaticLst[0].Value.Contains('|')) //code added by balaji.TJ 2015-11-28
        //                    sPhyname = StaticLst[0].Value.Split('|');
        //                //code Modified by balaji.TJ 2015-11-28
        //                if (sPhyname != null && sPhyname.Length > 0)
        //                {
        //                    sTemp = sPhyname[0].Replace("<Physician>", LoginPhysicianList != null ? LoginPhysicianList.PhyPrefix + " " + LoginPhysicianList.PhyFirstName + " " + LoginPhysicianList.PhyMiddleName + " " + LoginPhysicianList.PhyLastName + " " + LoginPhysicianList.PhySuffix : ""); //sPhyname[0].Replace("<Physician>", PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix);
        //                    string sReplace = sPhyname[1].Replace("<Date>", "").Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
        //                    sReplace = sReplace.Replace(" on ", "");
        //                    chkElectronicDeclaration.Text = sTemp + sReplace;
        //                }
        //                btnSave.Text = "Sign and Save";
        //            }
        //            else
        //            {
        //                if (StaticLst != null && StaticLst[0].Value.Contains('|')) //code added by balaji.TJ 2015-11-28
        //                    sPhyname = StaticLst[0].Value.Split('|');
        //                //code Modified by balaji.TJ 2015-11-28
        //                if (sPhyname != null && sPhyname.Length > 0)
        //                {
        //                    sTemp = sPhyname[1].Replace("<Physician>", PhysicianList != null ? PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + " " + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix : "");
        //                    sTemp = sTemp.Replace("<Date>", "");
        //                    sTemp = sTemp.Replace(" on ", "");
        //                }
        //                chkElectronicDeclaration.Text = sTemp;
        //            }
        //            txtDueon.MinDate = DateTime.Today.Date;
        //            if (txtDueon.SelectedDate <= UtilityManager.ConvertToLocal(Convert.ToDateTime(EncRecord.Due_On)))
        //                txtDueon.SelectedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(EncRecord.Due_On));
        //            else if (Convert.ToString(objdocument.EncounterObj.Return_In_Months) != "" || Convert.ToString(objdocument.EncounterObj.Return_In_Weeks) != "" || Convert.ToString(objdocument.EncounterObj.Return_In_Days) != "")
        //            {
        //                txtDueon.MinDate = DateTime.Today.Date;
        //                txtDueon.Enabled = false;
        //                chkDueOn.Checked = false;
        //            }
        //            else
        //            {
        //                txtDueon.MinDate = DateTime.Today.Date;
        //                txtDueon.SelectedDate = DateTime.Now;
        //                chkDueOn.Checked = true;
        //            }
        //        }
        //        else
        //        {
        //            txtDueon.MinDate = DateTime.Today.Date;
        //            txtDueon.SelectedDate = DateTime.Now;
        //            chkDueOn.Checked = true;
        //        }

        //        // dtpDueon.Attributes.Add("readonly", "readonly");
        //        //cboReturnIn.Attributes.Add("disabled", "disabled");              
        //        // txtDueon.Attributes.Add("readonly", "readonly");
        //        // txtDueon.Attributes.Add("disabled", "disabled");                
        //        //txtDueon.Enabled = true;
        //        btnSave.Enabled = false;
        //    }
        //    //txtDueon.MinDate = DateTime.Today.Date; //code changed by nijanthan on30-dec-2015                                
        //    if (ClientSession.UserCurrentProcess == "PROVIDER_REVIEW")
        //    {
        //        btnSave.Text = "Sign and Save";
        //        if (radbtnAgreewithChanges.Checked)
        //            txtAddendumToPlan.Enabled = true;
        //    }
        //    else
        //        btnSave.Text = "Save";
        //}
        #endregion
        public void TextBoxColorChange(Boolean bToNormal)
        {
            if (bToNormal == false)
            {

                txtGivenTo.ReadOnly = true;
                if (Request["HNAME"] != null)
                {
                    txtGivenTo.Text = Request["HNAME"].ToString();
                }
                txtGivenTo.CssClass = "nonEditabletxtbox";
                //txtGivenTo.EnabledStyle.BackColor = System.Drawing.Color.SkyBlue;
                //txtGivenTo.HoveredStyle.BackColor = System.Drawing.Color.SkyBlue;
                //txtGivenTo.FocusedStyle.BackColor = System.Drawing.Color.SkyBlue;
            }
            else
            {
                txtGivenTo.Text = string.Empty;
                txtGivenTo.ReadOnly = false;
                txtGivenTo.CssClass = "Editabletxtbox";
                //txtGivenTo.EnabledStyle.BackColor = System.Drawing.Color.White;
                //txtGivenTo.HoveredStyle.BackColor = System.Drawing.Color.White;
                //txtGivenTo.FocusedStyle.BackColor = System.Drawing.Color.White;
            }
        }

      
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string YesNoCancel = hdnMessageType.Value;
        //    hdnMessageType.Value = string.Empty;
        //    //code modified by balaji 2015-11-26
        //    DateTime utc = new DateTime();
        //    if (hdnLocalTime.Value != "")
        //    {
        //        string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
        //        utc = Convert.ToDateTime(strtime);
        //    }
        //    IList<TreatmentPlan> SaveList = new List<TreatmentPlan>();
        //    IList<TreatmentPlan> UpdateList = new List<TreatmentPlan>();
        //    //code modified by balaji 2015-11-26
        //    FillDocuments objdocument = new FillDocuments();
        //    if(ViewState["FillDocuments"]!=null)
        //     objdocument = (FillDocuments)ViewState["FillDocuments"];
        //    Encounter EncRecord = new Encounter();
        //    if(ViewState["FillDocuments"]!=null)
        //     EncRecord = ((FillDocuments)ViewState["FillDocuments"]).EncounterObj;
        //    IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
        //    if(ViewState["FillDocuments"]!=null)
        //     Treatment_Plan = ((FillDocuments)ViewState["FillDocuments"]).Treatment_Plan_List;

        //    //if (txtDueon.SelectedDate == null)
        //    //{
        //    //    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('380006');", true);
        //    //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "FromDateYearValidation", "DisplayErrorMessage('150004');", true);
        //    //    txtDueon.Focus();
        //    //    return;
        //    //}
        //    //if (radbtnAgreewithChanges.Checked)
        //    //{
        //    //    if (txtAddendumToPlan.Text == string.Empty)
        //    //    {
        //    //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " ('010012');", true);
        //    //        txtAddendumToPlan.Focus();
        //    //        return;
        //    //    }
        //    //}

        //    if (pnlElectronicSignature.Enabled == true)
        //    {
        //        if (chkElectronicDeclaration.Checked == false)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010015'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            return;
        //        }
        //        MySignID.Value = ClientSession.PhysicianId.ToString();

        //        SignedDateandTime.Value = (UtilityManager.ConvertToLocal(utc)).ToString();
        //    }
        //    if (pnlElectronicSignature.Enabled == false)
        //    {
        //        MySignID.Value = ClientSession.PhysicianId.ToString();

        //        SignedDateandTime.Value = (UtilityManager.ConvertToLocal(utc)).ToString();
        //    }

        //    //if (chkPFSHVerified.Enabled == true && chkPFSHVerified.Checked == false)
        //    //{
        //    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('110808');", true);
        //    //    chkPFSHVerified.Focus();
        //    //    return;
        //    //}

        //    if (radbtnAgreewithChanges.Checked)
        //    {
        //        TreatmentPlan objPlan = null;
        //        if (Treatment_Plan != null && Treatment_Plan.Count > 0)
        //        {
        //            for (int i = 0; i < Treatment_Plan.Count; i++)
        //            {
        //                objPlan = Treatment_Plan[i];
        //                objPlan.Addendum_Plan = txtAddendumToPlan.Text;
        //                objPlan.Modified_By = ClientSession.UserName;
        //                objPlan.Modified_Date_And_Time = utc;
        //                UpdateList.Add(objPlan);
        //            }
        //        }
        //        else
        //        {
        //            objPlan = new TreatmentPlan();
        //            objPlan.Human_ID = ClientSession.HumanId;
        //            objPlan.Encounter_Id = ClientSession.EncounterId;
        //            if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //            {
        //                objPlan.Physician_Id = ClientSession.PhysicianId;
        //            }
        //            else
        //            {
        //                //code added by balaji.TJ 2015-11-26
        //                if (Request["EPROID"] != null && Request["EPROID"]!="")
        //                objPlan.Physician_Id = Convert.ToUInt64(Request["EPROID"]); ;
        //            }
        //            objPlan.Plan = string.Empty;
        //            objPlan.Created_By = ClientSession.UserName;
        //            objPlan.Created_Date_And_Time = utc;
        //            objPlan.Addendum_Plan = txtAddendumToPlan.Text;
        //            objPlan.Modified_By = ClientSession.UserName;
        //            objPlan.Modified_Date_And_Time = utc;
        //            SaveList.Add(objPlan);
        //        }
        //    }
        //    //btnSave.Enabled = false;
        //    IList<Documents> SaveDocList = new List<Documents>();
        //    IList<Documents> UpdateDoclist = new List<Documents>();
        //    IList<Documents> DeleteDoclist = new List<Documents>();
        //    Documents objDocument = null;
        //    IList<string> SelectedDocuments = (chklstSelectDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text)).ToList<string>();
        //    if (SelectedDocuments != null) //code added by balaji.TJ 2015-11-26
        //    {
        //        for (int i = 0; i < SelectedDocuments.Count; i++)
        //        {
        //            var doc = (from d in objdocument.DocumentsList where d.Document_Type.ToUpper() == SelectedDocuments[i].ToString().ToUpper() select d);
        //            objDocument = new Documents();
        //            if (doc != null && doc.Count() > 0)
        //            {
        //                objDocument = doc.ToList<Documents>()[0];
        //                objDocument.Created_By = ClientSession.UserName;
        //                objDocument.Created_Date_And_Time = utc;
        //                objDocument.Given_To = txtGivenTo.Text;
        //                objDocument.Relationship = cboRelationship.Text;

        //                UpdateDoclist.Add(objDocument);
        //            }
        //            else
        //            {
        //                objDocument.Encounter_ID = ClientSession.EncounterId;
        //                objDocument.Human_ID = ClientSession.HumanId;
        //                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //                {
        //                    objDocument.Physician_ID = ClientSession.PhysicianId;
        //                }
        //                else
        //                {
        //                    if (Request["EPROID"] != null && Request["EPROID"]!="") //code added by balaji.TJ 2015-26-11
        //                    objDocument.Physician_ID = Convert.ToUInt64(Request["EPROID"]);
        //                }
        //                objDocument.Relationship = cboRelationship.Text;
        //                objDocument.Created_By = ClientSession.UserName;
        //                objDocument.Created_Date_And_Time = utc;
        //                objDocument.Document_Type = SelectedDocuments[i].ToString();
        //                objDocument.Given_To = txtGivenTo.Text;
        //                SaveDocList.Add(objDocument);
        //            }
        //        }
        //    }
        //    if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
        //    {
        //        for (int i = 0; i < objdocument.DocumentsList.Count; i++)
        //        {
        //            bool bResult = false;
        //            if (SelectedDocuments != null)
        //            {
        //                for (int j = 0; j < SelectedDocuments.Count; j++)
        //                {
        //                    if (SelectedDocuments[j].ToString().ToUpper() == objdocument.DocumentsList[i].Document_Type.ToUpper())
        //                    {
        //                        bResult = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (!bResult)
        //            {
        //                objdocument.DocumentsList[i].Given_By = ClientSession.UserName;
        //                DeleteDoclist.Add(objdocument.DocumentsList[i]);
        //            }
        //        }
        //    }
        //    EncRecord.Is_Document_Given = cboIsDocumentGiven.Text;

        //    //if (dtpDueon.SelectedDate != null)
        //    //{
        //    //    if (dtpDueon.SelectedDate.Value != null)
        //    //        EncRecord.Due_On = dtpDueon.SelectedDate.Value;
        //    //}
        //    if (txtDueon.SelectedDate != null)
        //    {
        //        if (txtDueon.SelectedDate.Value != null)
        //            EncRecord.Due_On = txtDueon.SelectedDate.Value;
        //    }
        //    else
        //    {
        //        if (txtDueon.SelectedDate == null)
        //            EncRecord.Due_On = DateTime.MinValue;
        //    }
           
     
        //        if (txtRetrunDays.Text != string.Empty)
        //        {
        //            EncRecord.Return_In_Days = Convert.ToInt32(txtRetrunDays.Text);              
        //        }
        //        else
        //        {
        //            EncRecord.Return_In_Days = 0;
        //        }
       
        //        if (txtReturnIn.Text != string.Empty)
        //        {
        //            EncRecord.Return_In_Months = Convert.ToInt32(txtReturnIn.Text);                    
        //        }
        //        else
        //        {
        //            EncRecord.Return_In_Months = 0;
        //        }
           
        //        if (txtRetrunWeeks.Text != string.Empty)
        //        {
        //            EncRecord.Return_In_Weeks = Convert.ToInt32(txtRetrunWeeks.Text);                  
        //        }
        //        else
        //        {
        //            EncRecord.Return_In_Weeks = 0;

        //        }
        //    //}
        //    //if (txtReturnIn.Text == string.Empty)
        //    //{
        //    //    EncRecord.Return_In_Days = 0;
        //    //    EncRecord.Return_In_Weeks = 0;
        //    //    EncRecord.Return_In_Months = 0;
        //    //}
        //    //if (dtpDueon.SelectedDate != null)
        //    //{
        //    //    EncRecord.Due_On = dtpDueon.SelectedDate.Value;
        //    //}
        //    EncRecord.Follow_Reason_Notes = txtNotes.txtDLC.Text;
        //    EncRecord.Modified_By = ClientSession.UserName;
        //    EncRecord.Modified_Date_and_Time = utc;
        //    if (EncRecord != null)
        //    {
        //        if (chkPFSHVerified.Checked == true && chkPFSHVerified.Enabled == true)
        //        {
        //            EncRecord.Is_PFSH_Verified = "Y";

        //        }
        //        //if (bMySign == true)
        //        //{
        //        if (chkElectronicDeclaration.Visible == true)
        //        {
        //            if (SignedDateandTime.Value!="" && Convert.ToDateTime(SignedDateandTime.Value) != DateTime.MinValue) 
        //            EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(SignedDateandTime.Value));
        //        }
        //        if (EncRecord.Encounter_Provider_ID == 0)
        //        {
        //            if (MySignID.Value.Trim() != "")
        //            {
        //                EncRecord.Encounter_Provider_ID = Convert.ToInt32(MySignID.Value);
        //            }
        //        }
        //        //Added by Naveena
        //        EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");

        //        //}
        //    }
        //    objdocument = DocumentMngr.SaveDocumentsAndMoveToNextProcess(SaveDocList.ToArray<Documents>(), UpdateDoclist.ToArray<Documents>(), DeleteDoclist.ToArray<Documents>(), SaveList.ToArray<TreatmentPlan>(), UpdateList.ToArray<TreatmentPlan>(), null, ClientSession.EncounterId, ClientSession.HumanId, Convert.ToUInt64(Request["SPHYID"].ToString()), EncRecord, ClientSession.UserName, Request["BUTTONNAME"].ToString(), ClientSession.FacilityName, utc, Convert.ToBoolean(Request["MTR"]), string.Empty,ClientSession.UserRole);
        //    EncRecord = objdocument.EncounterObj;
        //    ClientSession.FillEncounterandWFObject.EncRecord = EncRecord;
        //    ViewState["FillDocuments"] = objdocument; //110800
        //    if (YesNoCancel != "Yes")
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('110800');ClosePrintDoc(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //        // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('110800');", true);
        //    }
        //}
        void SaveEncounterDocumentDetails(int CloseType,string ButtonName)
        {
            string YesNoCancel = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            //code modified by balaji 2015-11-26
            DateTime utc = new DateTime();
            if (hdnLocalTime.Value != "")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                utc = Convert.ToDateTime(strtime);
            }
            IList<TreatmentPlan> SaveList = new List<TreatmentPlan>();
            IList<TreatmentPlan> UpdateList = new List<TreatmentPlan>();
            //code modified by balaji 2015-11-26
            FillDocuments objdocument = new FillDocuments();
            if (ViewState["FillDocuments"] != null)
                objdocument = (FillDocuments)ViewState["FillDocuments"];
            Encounter EncRecord = new Encounter();
            if (ViewState["FillDocuments"] != null)
                EncRecord = ((FillDocuments)ViewState["FillDocuments"]).EncounterObj;
            IList<TreatmentPlan> Treatment_Plan = new List<TreatmentPlan>();
            if (ViewState["FillDocuments"] != null)
                Treatment_Plan = ((FillDocuments)ViewState["FillDocuments"]).Treatment_Plan_List;

            if (pnlElectronicSignature.Enabled == true)
            {
                //if (chkElectronicDeclaration.Enabled==true && chkElectronicDeclaration.Checked == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010015'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    return ;
                //}
                if (hdnChkElecDeclaration.Value=="true")
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('010015');EnableSave(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    hdnSaveDetails.Value = "true";
                    return;
                }
                MySignID.Value = ClientSession.PhysicianId.ToString();

                SignedDateandTime.Value = (UtilityManager.ConvertToLocal(utc)).ToString();
            }
            if (pnlElectronicSignature.Enabled == false)
            {
                MySignID.Value = ClientSession.PhysicianId.ToString();

                SignedDateandTime.Value = (UtilityManager.ConvertToLocal(utc)).ToString();
            }
            if ((pnlAddendum.Enabled))
            {
                TreatmentPlan objPlan = null;
                IList<TreatmentPlan> ilstTreatementplan = new List<TreatmentPlan>();
                if (Treatment_Plan.Count > 0 && ButtonName == "MOVE TO PHYSICIAN ASSISTANT")
                {
                    string sAmendment_Type = string.Empty;
                    if (radbtnAgreewithChanges.Checked)
                    {
                        sAmendment_Type = radbtnAgreewithChanges.Text;
                    }
                    else if (radbtnCorrection.Checked)
                    {
                        sAmendment_Type = radbtnCorrection.Text;
                    }
                    else if (radbtnAgreePlan.Checked)
                    {
                        sAmendment_Type = radbtnAgreePlan.Text;
                    }
                    ilstTreatementplan = Treatment_Plan.Where(aa => aa.Plan_Type == "PLAN" && aa.Amendment_Type == sAmendment_Type).ToList<TreatmentPlan>();
                }
                if (ilstTreatementplan != null && ilstTreatementplan.Count > 0)
                {
                    for (int i = 0; i < ilstTreatementplan.Count; i++)
                    {
                        objPlan = ilstTreatementplan[i];
                        if (radbtnAgreewithChanges.Checked)
                        {
                            objPlan.Addendum_Plan = txtAddendumToPlan.Text;
                            objPlan.Amendment_Type = radbtnAgreewithChanges.Text;
                        }
                        else if (radbtnCorrection.Checked)
                        {
                            objPlan.Corrections_to_be_made = txtCorrectionToPlan.Text;
                            objPlan.Amendment_Type = radbtnCorrection.Text;
                        }
                        else if (radbtnAgreePlan.Checked)
                        {
                            objPlan.Amendment_Type = radbtnAgreePlan.Text;
                        }
                        objPlan.Modified_By = ClientSession.UserName;
                        objPlan.Modified_Date_And_Time = utc;
                        UpdateList.Add(objPlan);
                    }
                }
                else
                {
                    objPlan = new TreatmentPlan();
                    objPlan.Human_ID = ClientSession.HumanId;
                    objPlan.Encounter_Id = ClientSession.EncounterId;
                    objPlan.Plan_Type = "PLAN";
                    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    {
                        objPlan.Physician_Id = ClientSession.PhysicianId;
                    }
                    else
                    {
                        //code added by balaji.TJ 2015-11-26
                        if (Request["EPROID"] != null && Request["EPROID"] != "")
                            objPlan.Physician_Id = Convert.ToUInt64(Request["EPROID"]); ;
                    }
                    objPlan.Plan = string.Empty;
                    objPlan.Created_By = ClientSession.UserName;
                    objPlan.Created_Date_And_Time = utc;
                    objPlan.Local_Time = UtilityManager.ConvertToLocal(objPlan.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    if (radbtnAgreewithChanges.Checked)
                    {
                        objPlan.Addendum_Plan = txtAddendumToPlan.Text;
                        objPlan.Amendment_Type = radbtnAgreewithChanges.Text;
                    }
                    else if (radbtnCorrection.Checked)
                    {
                        objPlan.Corrections_to_be_made = txtCorrectionToPlan.Text;
                        objPlan.Amendment_Type = radbtnCorrection.Text;
                    }
                    else if (radbtnAgreePlan.Checked)
                    {
                        objPlan.Amendment_Type = radbtnAgreePlan.Text;
                    }
                    objPlan.Modified_By = ClientSession.UserName;
                    objPlan.Modified_Date_And_Time = utc;
                    SaveList.Add(objPlan);
                }
            }
            //btnSave.Enabled = false;
            IList<Documents> SaveDocList = new List<Documents>();
            IList<Documents> UpdateDoclist = new List<Documents>();
            IList<Documents> DeleteDoclist = new List<Documents>();
            Documents objDocument = null;
            IList<string> SelectedDocuments = (chklstSelectDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text)).ToList<string>();
            if (SelectedDocuments != null) //code added by balaji.TJ 2015-11-26
            {
                for (int i = 0; i < SelectedDocuments.Count; i++)
                {
                    var doc = (from d in objdocument.DocumentsList where d.Document_Type.ToUpper() == SelectedDocuments[i].ToString().ToUpper() select d);
                    objDocument = new Documents();
                    if (doc != null && doc.Count() > 0)
                    {
                        objDocument = doc.ToList<Documents>()[0];
                        objDocument.Created_By = ClientSession.UserName;
                        objDocument.Created_Date_And_Time = utc;
                        objDocument.Given_To = txtGivenTo.Text;
                        objDocument.Relationship = cboRelationship.Text;

                        UpdateDoclist.Add(objDocument);
                    }
                    else
                    {
                        objDocument.Encounter_ID = ClientSession.EncounterId;
                        objDocument.Human_ID = ClientSession.HumanId;
                        if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                        {
                            objDocument.Physician_ID = ClientSession.PhysicianId;
                        }
                        else
                        {
                            if (Request["EPROID"] != null && Request["EPROID"] != "") //code added by balaji.TJ 2015-26-11
                                objDocument.Physician_ID = Convert.ToUInt64(Request["EPROID"]);
                        }
                        objDocument.Relationship = cboRelationship.Text;
                        objDocument.Created_By = ClientSession.UserName;
                        objDocument.Created_Date_And_Time = utc;
                        objDocument.Document_Type = SelectedDocuments[i].ToString();
                        objDocument.Given_To = txtGivenTo.Text;
                        SaveDocList.Add(objDocument);
                    }
                }
            }
            if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
            {
                for (int i = 0; i < objdocument.DocumentsList.Count; i++)
                {
                    bool bResult = false;
                    if (SelectedDocuments != null)
                    {
                        for (int j = 0; j < SelectedDocuments.Count; j++)
                        {
                            if (SelectedDocuments[j].ToString().ToUpper() == objdocument.DocumentsList[i].Document_Type.ToUpper())
                            {
                                bResult = true;
                                break;
                            }
                        }
                    }
                    if (!bResult)
                    {
                        objdocument.DocumentsList[i].Given_By = ClientSession.UserName;
                        DeleteDoclist.Add(objdocument.DocumentsList[i]);
                    }
                }
            }
            EncRecord.Is_Document_Given = cboIsDocumentGiven.Text;

            //if (txtDueon.SelectedDate != null)
            //{
            //    if (txtDueon.SelectedDate.Value != null)
            //        EncRecord.Due_On = txtDueon.SelectedDate.Value;
            //}
            //else
            //{
            //    if (txtDueon.SelectedDate == null)
            //        EncRecord.Due_On = DateTime.MinValue;
            //}


            if (txtRetrunDays.Text != string.Empty)
            {
                EncRecord.Return_In_Days = Convert.ToInt32(txtRetrunDays.Text);
            }
            else
            {
                EncRecord.Return_In_Days = 0;
            }

            if (txtReturnIn.Text != string.Empty)
            {
                EncRecord.Return_In_Months = Convert.ToInt32(txtReturnIn.Text);
            }
            else
            {
                EncRecord.Return_In_Months = 0;
            }

            if (txtRetrunWeeks.Text != string.Empty)
            {
                EncRecord.Return_In_Weeks = Convert.ToInt32(txtRetrunWeeks.Text);
            }
            else
            {
                EncRecord.Return_In_Weeks = 0;

            }
            
            EncRecord.Follow_Reason_Notes = txtNotes.txtDLC.Text;
            EncRecord.Modified_By = ClientSession.UserName;
            EncRecord.Modified_Date_and_Time = utc;
            if (EncRecord != null)
            {
                
                //if (bMySign == true)
                //{
                //if (chkElectronicDeclaration.Visible == true)
                //{
                //    if (SignedDateandTime.Value != "" && Convert.ToDateTime(SignedDateandTime.Value) != DateTime.MinValue)
                //        EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(SignedDateandTime.Value));
                //}
                //if (EncRecord.Encounter_Provider_ID == 0)
                //{
                //    if (MySignID.Value.Trim() != "")
                //    {
                //        EncRecord.Encounter_Provider_ID = Convert.ToInt32(MySignID.Value);
                //    }
                //}
                //Added by Naveena
                

                //}
                if (ButtonName.ToUpper() == "MOVE TO NEXT PROCESS")
                {
                    if (ClientSession.UserCurrentProcess.ToUpper() == "PROVIDER_REVIEW" || ClientSession.UserCurrentProcess.ToUpper() == "PROVIDER_REVIEW_2")//BugID:52856
                    {
                       EncRecord.Encounter_Provider_Review_ID = Convert.ToInt32(ClientSession.CurrentPhysicianId);
                       if (chkElectronicDeclaration.Visible == true)
                       {
                           if (SignedDateandTime.Value != "" && Convert.ToDateTime(SignedDateandTime.Value) != DateTime.MinValue)
                               EncRecord.Encounter_Provider_Review_Signed_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(SignedDateandTime.Value));
                       }
                    }
                    EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                    if (chkPFSHVerified.Checked == true && chkPFSHVerified.Enabled == true)
                    {
                        EncRecord.Is_PFSH_Verified = "Y";

                    }
                }
                
            }

            if (chkSurgeryDeclaration.Visible==true && chkSurgeryDeclaration.Checked==true)
            {
                objdocument = DocumentMngr.SaveDocumentsAndMoveToNextProcess(SaveDocList.ToArray<Documents>(), UpdateDoclist.ToArray<Documents>(), DeleteDoclist.ToArray<Documents>(), SaveList.ToArray<TreatmentPlan>(), UpdateList.ToArray<TreatmentPlan>(), null, ClientSession.EncounterId, ClientSession.HumanId, Convert.ToUInt64(Request["SPHYID"].ToString()), EncRecord, ClientSession.UserName, ButtonName, ClientSession.FacilityName, utc, Convert.ToBoolean(Request["MTR"]), string.Empty, ClientSession.UserRole, CloseType,true);
            }
            else
            {
                objdocument = DocumentMngr.SaveDocumentsAndMoveToNextProcess(SaveDocList.ToArray<Documents>(), UpdateDoclist.ToArray<Documents>(), DeleteDoclist.ToArray<Documents>(), SaveList.ToArray<TreatmentPlan>(), UpdateList.ToArray<TreatmentPlan>(), null, ClientSession.EncounterId, ClientSession.HumanId, Convert.ToUInt64(Request["SPHYID"].ToString()), EncRecord, ClientSession.UserName, ButtonName, ClientSession.FacilityName, utc, Convert.ToBoolean(Request["MTR"]), string.Empty, ClientSession.UserRole, CloseType,false);
            }

            //Jira CAP-3577
            if (ButtonName.ToUpper() == "MOVE TO NEXT PROCESS" || ButtonName.ToUpper() == "MOVE TO PHYSICIAN ASSISTANT")
            {
                if ((ConfigurationSettings.AppSettings["IsAkidoNoteCDC"]?.ToString()?.ToUpper() ?? "") == "Y" && ClientSession.EncounterId != 0 && !Convert.ToDateTime(EncRecord.Encounter_Provider_Signed_Date).ToString("yyyy-MM-dd").Contains("0001-01-01"))
                {
                    EncounterBlobManager.IsAkidoCDC(ClientSession.HumanId.ToString(), ClientSession.EncounterId.ToString(), ClientSession.UserName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            hdnSaveDetails.Value = "false";
            EncRecord = objdocument.EncounterObj;
            ClientSession.FillEncounterandWFObject.EncRecord = EncRecord;
            ViewState["FillDocuments"] = objdocument; //110800
            if ((ClientSession.UserCurrentProcess == "PROVIDER_PROCESS" || ClientSession.UserCurrentProcess == "SCRIBE_PROCESS") && ButtonName == "MOVE TO CHECKOUT")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('110800');ClosePrintDoc(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('110800');ClosePrintDocMovetoMyQ(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
        protected void cboRelationship_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cboRelationship.Enabled = true;
            cboRelationship.CssClass = "spanstyle";
            cboIsDocumentGiven.Enabled = true;
            cboIsDocumentGiven.CssClass = "spanstyle";
            //btnSave.Enabled = true;;
            if (cboRelationship.Text.Trim() == string.Empty)
            {
                txtGivenTo.Enabled = false;
                txtGivenTo.CssClass = "nonEditabletxtbox";
            }
            else
            {
                txtGivenTo.Enabled = true;
                txtGivenTo.CssClass = "Editabletxtbox";
            }
            if (cboRelationship.Text.ToUpper() == "SELF")          
                TextBoxColorChange(false);          
            else           
                TextBoxColorChange(true);
            //Cap - 936
            if (cboIsDocumentGiven.SelectedIndex == 0 && chklstSelectDocuments.Items.Cast<ListItem>().Count(li => li.Selected) >0)
            {
                cboIsDocumentGiven.SelectedIndex = 1;
            }
            //if (chkDueOn.Checked)           //code added by balaji.TJ 2015-11-26
            //    txtDueon.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Relationshiploadingstop();", true);
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DateTime utc = new DateTime();
            if (hdnLocalTime.Value != "")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                utc = Convert.ToDateTime(strtime);
            }
            string screen = string.Empty;
            bool Progress = false;
            bool care = false;
            bool WellnessNotes = false;
            string screen_ucm = string.Empty;

            hdnSelectedItem.Value = string.Empty;
            hdnXmlPath.Value = string.Empty;
            string filesNotFound = string.Empty;
            IList<string> SelectedItems = chklstSelectDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Value + "|" + b.Text).ToList<string>();
            //IList<string> SelectedItems = chklstSelectDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text).ToList<string>();
            string sFaxSubject = string.Empty;
            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
            {
                string sFaxFirstname = string.Empty;
                string sFaxLastName = string.Empty;

                string sPatientStrip = string.Empty;

                IList<string> ilstHumanTag = new List<string>();
                ilstHumanTag.Add("HumanList");

                IList<object> ilstHumanBlobList = new List<object>();
                ilstHumanBlobList = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHumanTag);

                Human objFillHuman = new Human();

                if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
                {
                    if (ilstHumanBlobList[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                        {
                            objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                        }
                    }
                }
                if(objFillHuman.First_Name!=null)
                {
                    sFaxFirstname = objFillHuman.First_Name;
                }
                if(objFillHuman.Last_Name!=null)
                {
                    sFaxLastName = objFillHuman.Last_Name;
                }

                //string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";
                //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
                //if (File.Exists(strXmlHumanPath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                //    using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                //        {
                //            if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                //            {
                //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                //                    sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                //                    sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
                 sFaxSubject = sFaxLastName + sFaxFirstname + "_" + DateTime.Now.ToString("dd-MMM-yyyy");

            }

            string[] GetFiles = Directory.GetFiles(Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education"));
            string[] Separator = new string[] { Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\") };

            IList<string> FilesNotFound = new List<string>();
            if (SelectedItems != null && SelectedItems.Count == 0)
            {
                //code added by balaji.TJ 2015-11-23
                //if (chkDueOn.Checked)
                //{
                //    txtDueon.Enabled = true;
                //    cboRelationship.Enabled = true;
                //    cboIsDocumentGiven.Enabled = true;
                //}
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('110805'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            if (GetFiles != null && GetFiles.Length > 0) //code added by balaji.TJ 2015-26-11
            {
                foreach (string s in GetFiles)
                {
                    string[] SplitedDocName = s.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
                    if (SplitedDocName.Length > 0)  //code added by balaji.TJ 2015-28-11
                        FilesNotFound.Add(SplitedDocName[0].ToString());
                }
            }
            Encounter EncRecord = new Encounter();
            FillDocuments objdocument = DocumentMngr.GetDocumentsList(ClientSession.EncounterId);
            string DownloadDoc = string.Empty;
            IList<Documents> SaveList = new List<Documents>();
            IList<Documents> Updatelist = new List<Documents>();
            IList<Documents> Deletelist = new List<Documents>();
            Documents objDocument = null;
            if (SelectedItems != null && SelectedItems.Count > 0) //code added by balaji.TJ 2015-11-26
            {
                for (int i = 0; i < SelectedItems.Count; i++)
                {
                    if (SelectedItems[i].ToString().ToUpper().Contains("PROGRESS NOTE") == true)
                    {
                        if (!Progress)
                        {
                            IList<string> lstNotes = SelectedItems.Where(a => a.ToUpper().Contains("PROGRESS NOTE") || a.ToUpper().Contains("CONSULTATION NOTE")).ToList<string>();

                            RadWindow2.VisibleOnPageLoad = true;
                            RadWindow2.Height = 10;
                            RadWindow2.Width = 10;
                            if (lstNotes != null && lstNotes.Count > 1)
                                screen = "Pro|Con";
                            else
                                screen = "Pro";
                            Progress = true;
                        }

                        //frmNewProgressNotes obj = new frmNewProgressNotes();
                        //obj.ShowProgressNotes(ClientSession.Selectedencounterid, true, true, new Acurus.Capella.Core.DomainObjects.Encounter(),hdnLocalTime.Value);

                    }
                    else if (SelectedItems[i].ToString().ToUpper().Contains("CARE NOTE") == true)
                    {
                        if (!care)
                        {
                            IList<string> lstNotes = SelectedItems.Where(a => a.ToUpper().Contains("CARE NOTE") || a.ToUpper().Contains("TREATMENT PLAN NOTE")).ToList<string>();

                            RadWindow4.VisibleOnPageLoad = true;
                            RadWindow4.Height = 10;
                            RadWindow4.Width = 10;
                            if (lstNotes != null && lstNotes.Count > 1)
                                screen_ucm = "Care|Treat";
                            else
                                screen_ucm = "Care";
                            care = true;
                        }

                    }
                    else if (SelectedItems[i].ToString().Split('|')[1].ToUpper() == "TREATMENT PLAN NOTE")
                    {
                        string sMyPath = string.Empty;
                        if (!care)
                        {

                            RadWindow5.VisibleOnPageLoad = true;

                            RadWindow5.Height = 10;
                            RadWindow5.Width = 10;

                            IList<string> lstNotes = SelectedItems.Where(a => a.ToUpper().Contains("CARE NOTE") || a.ToUpper().Contains("TREATMENT PLAN NOTE")).ToList<string>();

                            //string screen = string.Empty;
                            if (lstNotes != null && lstNotes.Count > 1) //code added by balaji.TJ 2015-11-26
                                screen_ucm = "Care|Treat";
                            else
                                screen_ucm = "Treat";


                            care = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "treatment_plan", "openCareTreatPlan_PrintDocument('" + screen_ucm + "','" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        }
                        //frmConsultationNotes obj = new frmConsultationNotes();

                        //obj.ShowProgressNotes(ClientSession.Selectedencounterid, true, new Acurus.Capella.Core.DomainObjects.Encounter(), new System.Collections.Generic.List<Acurus.Capella.Core.DTO.PatientPane>(),hdnLocalTime.Value);
                    }
                    else if (SelectedItems[i].ToString().ToUpper().Contains("WELLNESS NOTE") == true)
                    {
                        if (!WellnessNotes)
                        {
                            if (screen_ucm != null && screen_ucm != "")
                            {
                                screen_ucm = screen_ucm + "|Wellness";
                            }
                            else
                            {
                                screen_ucm = "Wellness";
                            }
                            WellnessNotes = true;
                        }
                    }
                    else if (SelectedItems[i].ToString().Split('|')[1].ToUpper() == "CLINICAL SUMMARY")
                    {
                        string sMyPath = string.Empty;
                        frmClinicalSummary objfrmClinical = new frmClinicalSummary();
                        ArrayList FileLocation = objfrmClinical.PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, false, ref sMyPath, string.Empty, true, false);
                        // ArrayList FileLocation = PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, true, ref sMyPath, string.Empty, false, false);

                        hdnPrintFilePath.Value = string.Empty;
                        hdnXmlPath.Value = string.Empty;

                        string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                        if (FileLocation[0].ToString().EndsWith(".xml") == true)
                        {
                            if (hdnXmlPath.Value == string.Empty)
                            {
                                string[] XMLFileName = new string[] { };
                                if (Split != null && Split.Length > 0)
                                    XMLFileName = FileLocation[0].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);
                                if (hdnXmlPath.Value == string.Empty)
                                {
                                    hdnXmlPath.Value = "Documents\\" + Session.SessionID.ToString() + XMLFileName[0].ToString();
                                }
                                if (hdnXmlPath.Value != string.Empty)
                                {
                                    DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(hdnXmlPath.Value));
                                    if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
                                    {
                                        Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
                                    }
                                    System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath("Documents/" + Session.SessionID.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl"), true);

                                }

                            }

                        }
                    }
                    //else if (SelectedItems[i].ToString().Split('|')[1].ToUpper() == "WELLNESS NOTE")
                    //{
                    //    string sMyPath = string.Empty;
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "WELLNESS NOTE", "WellNotesWindow_PrintDocument(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    //}
                    else if (SelectedItems[i].ToString().Split('|')[1].ToUpper() == "CONSULTATION NOTE")
                    {
                        string sMyPath = string.Empty;
                        if (!Progress)
                        {

                            RadWindow3.VisibleOnPageLoad = true;

                            RadWindow3.Height = 10;
                            RadWindow3.Width = 10;

                            IList<string> lstNotes = SelectedItems.Where(a => a.ToUpper().Contains("PROGRESS NOTE") || a.ToUpper().Contains("CONSULTATION NOTE")).ToList<string>();

                            //string screen = string.Empty;
                            if (lstNotes != null && lstNotes.Count > 1) //code added by balaji.TJ 2015-11-26
                                screen = "Pro|Con";
                            else
                                screen = "Con";


                            Progress = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CONSULTATION NOTE", "openProgress_PrintDocument('" + screen + "','" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        }
                        //frmConsultationNotes obj = new frmConsultationNotes();

                        //obj.ShowProgressNotes(ClientSession.Selectedencounterid, true, new Acurus.Capella.Core.DomainObjects.Encounter(), new System.Collections.Generic.List<Acurus.Capella.Core.DTO.PatientPane>(),hdnLocalTime.Value);
                    }
                    else if (SelectedItems[i].ToString().Split('|')[0].ToUpper().Contains(".DO"))
                    {
                        if (DownloadDoc != null && DownloadDoc != "")
                        {
                            DownloadDoc = DownloadDoc + "|" + SelectedItems[i].ToString().Split('|')[0];
                        }
                        else
                        {
                            DownloadDoc = SelectedItems[i].ToString().Split('|')[0];
                        }
                    }
                    else
                    {
                        if (!FilesNotFound.Any(a => a.ToString() == SelectedItems[i].ToString().Split('|')[0].ToString()))
                        {
                            if (filesNotFound == string.Empty)
                            {
                                filesNotFound = SelectedItems[i].ToString().Split('|')[1];
                            }
                            else
                            {
                                filesNotFound += " , " + SelectedItems[i].ToString().Split('|')[1];
                            }

                            continue;
                        }
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = SelectedItems[i].ToString().Replace(".pdf", "").Split('|')[0];
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + SelectedItems[i].ToString().Replace(".pdf", "").Split('|')[0];
                        }

                    }
                }
            }
            if (hdnSelectedItem.Value == string.Empty)
            {
                if (filesNotFound != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "file-not-found", "DisplayErrorMessageList('110806','" + filesNotFound + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    cboRelationship.Enabled = true;
                }
            }

            for (int i = 0; i < SelectedItems.Count; i++)
            {
                var doc = (from d in objdocument.DocumentsList where d.Document_Type.ToUpper() == SelectedItems[i].Split('|')[1].ToString().ToUpper() select d);

                objDocument = new Documents();
                if (doc != null && doc.Count() > 0) //code added by balaji.TJ 2015-28-11
                {

                    objDocument = doc.ToList<Documents>()[0];
                    objDocument.Given_To = txtGivenTo.Text;
                    objDocument.Relationship = cboRelationship.Text;
                    objDocument.Given_By = ClientSession.UserName;
                    objDocument.Given_Date = utc;
                    Updatelist.Add(objDocument);
                }
                else
                {
                    objDocument.Encounter_ID = ClientSession.EncounterId;
                    objDocument.Human_ID = ClientSession.HumanId;
                    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    {
                        objDocument.Physician_ID = ClientSession.PhysicianId;
                    }
                    else
                    {
                        objDocument.Physician_ID = Convert.ToUInt64(ClientSession.PhysicianId);
                    }
                    objDocument.Relationship = cboRelationship.Text;
                    objDocument.Created_By = ClientSession.UserName;
                    objDocument.Created_Date_And_Time = utc;
                    objDocument.Document_Type = SelectedItems[i].Split('|')[1].ToString();
                    objDocument.Given_To = txtGivenTo.Text;
                    objDocument.Given_By = ClientSession.UserName;
                    objDocument.Given_Date = utc;
                    SaveList.Add(objDocument);
                }
            }
            if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
            {
                for (int i = 0; i < objdocument.DocumentsList.Count; i++)
                {
                    bool bResult = false;
                    for (int j = 0; j < SelectedItems.Count; j++)
                    {
                        if (SelectedItems[j].Split('|')[1].ToString().ToUpper() == objdocument.DocumentsList[i].Document_Type.ToUpper())
                        {
                            bResult = true;
                            break;
                        }

                    }
                    if (!bResult)
                    {
                        objdocument.DocumentsList[i].Given_By = ClientSession.UserName;
                        Deletelist.Add(objdocument.DocumentsList[i]);
                    }

                }
            }
            IList<Encounter> encList = new List<Encounter>();
            EncounterManager encMngr = new EncounterManager();
            encList = encMngr.GetEncounterByEncounterID(ClientSession.EncounterId);
            //EncRecord.Version = encList[0].Version;
            if (encList != null && encList.Count > 0) //code added by balaji.TJ 2015-28-11
            {
                EncRecord = encList[0];
                EncRecord.Is_Document_Given = cboIsDocumentGiven.Text;
                EncRecord.Modified_By = ClientSession.UserName;
                EncRecord.Modified_Date_and_Time = utc;
                EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            }

            objdocument = DocumentMngr.SaveUpdateDeleteDocument(SaveList.ToArray<Documents>(), Updatelist.ToArray<Documents>(), Deletelist.ToArray<Documents>(), EncRecord, ClientSession.EncounterId, string.Empty, true);
            ViewState["FillDocuments"] = objdocument;
            if (hdnSelectedItem.Value != string.Empty)
            {
                string scrn = string.Empty;
                string prjct = string.Empty;
                if (screen == string.Empty)
                {
                    scrn = screen_ucm;
                    prjct = "UCM";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPDF", "OpenPDF_PrintDocument('" + filesNotFound + "','" + scrn + "','" + prjct + "','" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                cboRelationship.Enabled = true;
                cboIsDocumentGiven.Enabled = true;
            }
            if (screen != string.Empty && hdnSelectedItem.Value == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), " openProgress", " openProgress_PrintDocument('" + screen + "','" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else if (screen_ucm != string.Empty && hdnSelectedItem.Value == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), " openCareTreat", " openCareTreatPlan_PrintDocument('" + screen_ucm + "','" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            if (hdnXmlPath.Value != string.Empty && hdnSelectedItem.Value == string.Empty && screen == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenClinicalSmry", "OpenClinicalSmry_PrintDocument('" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else if (hdnXmlPath.Value != string.Empty && hdnSelectedItem.Value == string.Empty && screen_ucm == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenClinicalSmry", "OpenClinicalSmry_PrintDocument('" + sFaxSubject + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            if (hdnSelectedItem.Value != string.Empty || DownloadDoc != string.Empty)
            {
                Regex regex = new Regex(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$");
                Match match = regex.Match(hdnSelectedItem.Value + ".pdf");

                if (!match.Success)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessageList('110806','" + hdnSelectedItem.Value + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDFStatic('" + filesNotFound + "','" + string.Empty + "','" + DownloadDoc + "','" + sFaxSubject + "');", true);
            }
        }

        //protected void imgDocumentsLibrary_Click(object sender, ImageClickEventArgs e)
        //{
        //    IList<UserLookup> FieldLookUpList = userLookupMngr.GetFieldLookupList(Convert.ToUInt64(Request["EPROID"]), "PATIENT EDUCATION DOCUMENTS");
        //    if (FieldLookUpList != null)
        //    {
        //        chklstSelectDocuments.Items.Clear();
        //        if (FieldLookUpList.Count > 0)
        //        {
        //            for (int i = 0; i < FieldLookUpList.Count; i++)
        //            {
        //                this.chklstSelectDocuments.Items.Add(FieldLookUpList[i].Value);

        //            }
        //        }
        //    }
        //}

        protected void btnInvisible_Click(object sender, EventArgs e)
        {
            //code added by balaji.TJ 2015-26-11
            IList<UserLookup> FieldLookUpList = new List<UserLookup>();
            if(Request["EPROID"]!=null && Request["EPROID"]!="")
             FieldLookUpList = userLookupMngr.GetFieldLookupList(Convert.ToUInt64(Request["EPROID"]), "PATIENT EDUCATION DOCUMENTS");
            FieldLookUpList = FieldLookUpList.OrderBy(a => a.Value).ToList();
            if (FieldLookUpList != null && FieldLookUpList.Count > 0)
            {
                chklstSelectDocuments.Items.Clear();
                //if (FieldLookUpList.Count > 0)
                //{
                    for (int i = 0; i < FieldLookUpList.Count; i++)
                    {
                        this.chklstSelectDocuments.Items.Add(FieldLookUpList[i].Value);

                    }
                //}
            }
        }

        protected void radbtnAgreePlan_CheckedChanged(object sender, EventArgs e)
        {
            txtAddendumToPlan.Enabled = false;
            if (radbtnAgreewithChanges.Checked == true)
            {
                radbtnAgreewithChanges.Checked = false;
            }
        }

        protected void radbtnAgreewithChanges_CheckedChanged(object sender, EventArgs e)
        {
            txtAddendumToPlan.Enabled = true;
            if (radbtnAgreePlan.Checked == true)
            {
                radbtnAgreePlan.Checked = false;
            }
        }
        //BugID:40876
        protected void btnClearDrpdwn_Click(object sender, EventArgs e)
        {
            cboRelationship.ClearSelection();
            cboRelationship.Items[0].Selected = true;
            cboRelationship.SelectedIndex = 0;
            cboRelationship.Enabled = false;
            //Cap - 936
            cboIsDocumentGiven.Enabled = false;
            cboIsDocumentGiven.Items[0].Selected = true;
            cboIsDocumentGiven.SelectedIndex = 0;
            txtGivenTo.Enabled = false;
            txtGivenTo.CssClass = "nonEditabletxtbox";
            txtGivenTo.Text = string.Empty;
        }
        //Changed for Provider_Review PhysicianAssistant WorkFlow Change
        protected void btnSignMove_Click(object sender, EventArgs e)
        {
            int CloseType = 2;
            if (btnSave.Text == "Save")
            {
                SaveEncounterDocumentDetails(CloseType, "MOVE TO CHECKOUT");
            }
            else
            {


                SaveEncounterDocumentDetails(CloseType, "MOVE TO NEXT PROCESS");
            }
        }

        protected void btnMovetoPhyAsst_Click(object sender, EventArgs e)
        {
            int CloseType = 1;

            if( hdnscribe.Value==string.Empty)
            SaveEncounterDocumentDetails(CloseType,"MOVE TO PHYSICIAN ASSISTANT");
            else
            {
                CloseType = 6;
                SaveEncounterDocumentDetails(CloseType, "MOVE TO PHYSICIAN ASSISTANT");
            }
        }

        protected void btnmovetoscribe_Click(object sender, EventArgs e)
        {
            radbtnCorrection.Text = "Corrections to be Made by Scribe";
           int  CloseType = 6;
            SaveEncounterDocumentDetails(CloseType, "MOVE TO PHYSICIAN ASSISTANT");
                    

        }

    }
}


