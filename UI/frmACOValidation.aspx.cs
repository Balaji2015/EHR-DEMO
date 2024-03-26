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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using Acurus.Capella.Core.DTO;

namespace Acurus.Capella.UI
{
    public partial class frmACOValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                chkPFSHVerified.Checked = true;
                // ClientSession.bPFSHVerified = true;
                if (Request["IsPFSHVald"] != null && Request["IsPFSHVald"].ToString() == "Y")
                {

                    if (Request["btnName"] != null && Request["btnName"].ToString() != string.Empty)
                    {

                        if (Request["btnName"].ToString().ToUpper() == "MOVE TO CHECKOUT" && (ClientSession.UserRole.Trim().ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN"))
                        {

                            pnlVerifyPFSH.Visible = false;
                        }


                        if (Request["btnName"].ToString().ToUpper() == "MOVE TO NEXT PROCESS" && ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN")
                        {
                            pnlVerifyPFSH.Visible = false;
                        }

                        if (Request["btnName"].ToString().ToUpper() == "MOVE TO MA" && ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN")
                        {
                            pnlVerifyPFSH.Visible = true;
                            pnlACOParticipation.Visible = true;
                        }


                        if (Request["btnName"].ToString().ToUpper() == "MOVE TO PROVIDER" && ClientSession.UserRole.Trim().ToUpper() == "MEDICAL ASSISTANT")
                        {
                            pnlVerifyPFSH.Visible = true;
                            pnlACOParticipation.Visible = true;
                        }




                    }

                    // pnlVerifyPFSH.Visible = true;
                    // chkPFSHVerified.Checked = ClientSession.bPFSHVerified;
                    this.Title = "ACO Validation";
                }
                else
                {
                    pnlVerifyPFSH.Visible = false;
                    this.Title = "ACO";
                }
                if (Request["btnName"] != null && Request["btnName"].ToString() != string.Empty)
                {
                    btnSave.Text = Request["btnName"].ToString();
                }
                if (Request["HumanID"] != null && Request["HumanID"].ToString() != "")
                {
                    hdnACOHumanID.Value = Request["HumanID"].ToString();
                }
                else
                {
                    hdnACOHumanID.Value = ClientSession.HumanId.ToString();
                }
                Human objHuman = new Human();
                HumanManager objHumanManager = new HumanManager();

                /***  commented for perfomance tuning  by Jisha -  16/04/2015   ***/

                //objHuman = objHumanManager.GetById(Convert.ToUInt32(hdnACOHumanID.Value));

                /***  added for perfomance tuning
                   * returns only Patient_Discussed,Discussed_By,ACO_Is_Eligible_Patient
                   *   by Jisha   ***/
                IList<string> lsthuman = new List<string>();              
                lsthuman = objHumanManager.GetACODetails(Convert.ToUInt32(hdnACOHumanID.Value));
                if (lsthuman != null)
                {
                    if (lsthuman[0].ToString() == "N")
                    {
                        pnlACOParticipation.Enabled = false;
                        pnlACOParticipation.Style.Add(HtmlTextWriterStyle.Display, "none");
                        pnlACOParticipation.Style.Add(HtmlTextWriterStyle.Height, "0px");
                        this.Title = "PFSH Verification";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "ReAdjustWindowSize();", true);
                        if (pnlVerifyPFSH.Visible == false)
                        {
                            //HtmlGenericControl divHide = new HtmlGenericControl("div");
                            //divHide.ID = "overLay";
                            //divHide.Attributes.Add("class", "modal");
                            //HtmlGenericControl message = new HtmlGenericControl("h4");
                            //message.InnerHtml = "This patient is not eligible for ACO";
                            //divHide.Controls.Add(new HtmlGenericControl("br"));
                            //divHide.Controls.Add(new HtmlGenericControl("br"));
                            //divHide.Controls.Add(message);
                            //pnlACOParticipation.Controls.Add(divHide);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('1091010');var o = new Object();o=true;returnToParent(o);", true);
                        }
                    }
                    else
                    {
                        pnlACOParticipation.Enabled = true;
                    }


                    if (pnlACOParticipation.Enabled)
                    {
                        StaticLookupManager objStaticLookupManager = new StaticLookupManager();
                        IList<StaticLookup> ilstStaticLookup = new List<StaticLookup>();

                        /***  commented for perfomance tuning
                           * removed ACO PATIENT DISCUSSED                      
                           *   by Jisha ***/

                        // ilstStaticLookup = objStaticLookupManager.getStaticLookupByFieldName(new string[] { "ACO PATIENT DISCUSSED", "ACO PATIENT ACCEPTED" });

                        ilstStaticLookup = objStaticLookupManager.getStaticLookupByFieldName(new string[] { "ACO PATIENT ACCEPTED" });

                        foreach (StaticLookup objStaticLookup in ilstStaticLookup.Where(a => a.Field_Name == "ACO PATIENT ACCEPTED"))
                        {
                            ListItem newItem = new ListItem();
                            newItem.Text = objStaticLookup.Value;
                            newItem.Value = objStaticLookup.Description;
                            cboACODiscusion.Items.Add(newItem);
                            if (objStaticLookup.Value == objStaticLookup.Default_Value)
                            {
                                newItem.Selected = true;
                            }
                            else
                            {
                                newItem.Selected = false;
                            }
                        }
                        if ((Convert.ToUInt32(lsthuman[2]) != 0 || lsthuman[1].ToString() != ""))
                        {
                            ToggleShowAllPhysician(lsthuman[1].ToString());
                            // btnSave.Enabled = false;
                            if (Session["PFSHEnabled"] == "True")
                            {
                                btnSave.Enabled = true;
                            }
                            else
                            {
                                btnSave.Enabled = false;
                            }
                        }
                        else
                        {
                            ToggleShowAllPhysician(string.Empty);


                        }
                        if (!(Request["IsPFSHVald"] != null && Request["IsPFSHVald"].ToString() == "Y"))
                        {
                            cboPhysicianNames.SelectedIndex = -1;
                            cboPhysicianNames.SelectedValue = ClientSession.PhysicianId.ToString();
                        }
                        if ((Convert.ToUInt32(lsthuman[2]) != 0 || lsthuman[1].ToString() != ""))
                        {
                            cboACODiscusion.ClearSelection();
                            for (int i = 0; i < cboACODiscusion.Items.Count; i++)
                            {
                                if (cboACODiscusion.Items[i] != cboACODiscusion.Items.FindByValue(lsthuman[1].ToString()))
                                {
                                    cboACODiscusion.Items[i].Selected = false;
                                }
                                else
                                {
                                    cboACODiscusion.Items[i].Selected = true;
                                    cboACODiscusion.SelectedIndex = i;
                                }
                            }
                            cboPhysicianNames.ClearSelection();
                            try
                            {
                                cboPhysicianNames.ClearSelection();
                                for (int i = 0; i < cboPhysicianNames.Items.Count; i++)
                                {
                                    if (cboPhysicianNames.Items[i] != cboPhysicianNames.Items.FindByValue(lsthuman[2].ToString()))
                                    {
                                        cboPhysicianNames.Items[i].Selected = false;
                                    }
                                    else
                                    {
                                        cboPhysicianNames.Items[i].Selected = true;
                                        cboPhysicianNames.SelectedIndex = i;
                                    }
                                }
                            }
                            catch 
                            {

                                if (Convert.ToUInt32(lsthuman[2]) != 0)
                                {
                                    chkShowAllPhysician.Checked = true;
                                    ToggleShowAllPhysician(string.Empty);
                                    cboPhysicianNames.ClearSelection();
                                    for (int i = 0; i < cboACODiscusion.Items.Count; i++)
                                    {
                                        if (cboPhysicianNames.Items[i] != cboPhysicianNames.Items.FindByValue(lsthuman[2].ToString()))
                                        {
                                            cboPhysicianNames.Items[i].Selected = false;
                                        }
                                        else
                                        {
                                            cboPhysicianNames.Items[i].Selected = true;
                                            cboPhysicianNames.SelectedIndex = i;
                                        }
                                    }
                                }
                                else
                                {
                                    cboPhysicianNames.SelectedIndex = -1;
                                    cboPhysicianNames.Text = "";
                                }
                            }
                        }


                    }
                    string sHumanName = lsthuman[3].ToString() + "," + lsthuman[4].ToString() + " " + lsthuman[5].ToString() + " " + lsthuman[6].ToString();
                    hdnHumanName.Value = sHumanName;
                }
            }
            if (btnSave.Text.ToUpper() == "MOVE TO CHECKOUT" || btnSave.Text.ToUpper() == "MOVE TO MA" || btnSave.Text.ToUpper() == "MOVE TO NEXT PROCESS" || btnSave.Text.ToUpper() == "MOVE TO PROVIDER")
            {
                btnSave.Enabled = true;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool Flag = true;
            string source_information = string.Empty;

            if (pnlVerifyPFSH.Visible && (btnSave.Text.ToUpper() == "MOVE TO CHECKOUT" || btnSave.Text.ToUpper() == "MOVE TO NEXT PROCESS"))
            {
                if (pnlACOParticipation.Enabled)
                {
                    if (cboPhysicianNames.Text == string.Empty && cboACODiscusion.Text == "Y")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "PhysicianComboVal", "DisplayErrorMessage('1091011');", true);
                        return;
                    }
                    HumanManager objHumanManager = new HumanManager();

                    objHumanManager.UpdateACOQueries(Convert.ToUInt32(hdnACOHumanID.Value), "", cboACODiscusion.SelectedValue, tblPhysicianDetails.Visible ? Convert.ToUInt32(cboPhysicianNames.SelectedValue) : tblPhysicianDetails.Visible ? Convert.ToUInt32(cboPhysicianNames.SelectedValue) : ClientSession.PhysicianId, ClientSession.EncounterId, Flag, source_information, ClientSession.UserName, UtilityManager.ConvertToUniversal());
                    ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "PhysicianComboVal", "DisplayErrorMessage('1091012');", true);
                    //EncounterManager objEncounterManager=new EncounterManager();
                    //IList<Encounter> EncList = objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                    //if (EncList != null && EncList.Count > 0)
                    //{
                    //    Encounter currentEncounter = new Encounter();
                    //    currentEncounter = EncList[0];
                    //    currentEncounter.Is_PFSH_Verified = "Y";
                    //    currentEncounter.Modified_By = ClientSession.UserName;
                    //    currentEncounter.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
                    //    objEncounterManager.UpdateEncounter(currentEncounter, string.Empty, new object[] { "false" });
                    //}
                }
                string OpenPrintDocumentsParameters = string.Empty;
                OpenPrintDocumentsParameters = "o.EID=" + ClientSession.EncounterId + ";o.SPHYID=" + ClientSession.PhysicianId + ";o.HID=" + hdnACOHumanID.Value + ";o.UNAME='" + ClientSession.UserName + "';o.BUTTONNAME='" + btnSave.Text + "';o.HNAME='" + hdnHumanName.Value.ToString() + "';o.EPROID=" + ClientSession.PhysicianId + ";o.MTR=false;o.DS=false;";
                ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "OpenPrintDocuments", "var o = new Object();" + OpenPrintDocumentsParameters + "returnToParent(o);", true);
            }
            else if (pnlVerifyPFSH.Visible && (btnSave.Text == "Move to Provider" || btnSave.Text == "Move to MA"))
            {
                ClientSession.bPFSHVerified = true;
                //(string)ApplicationData.Current.LocalSettings.Values["test"];
                bool IsPassedValidation = false;

                SocialHistoryManager objSocialHistoryManager = new SocialHistoryManager();
                IList<SocialHistory> SocialHistoryDetails = null;
                SocialHistoryDTO problemDTO = new SocialHistoryDTO();
                problemDTO = objSocialHistoryManager.GetSocialHistoryByHumanID(Convert.ToUInt32(hdnACOHumanID.Value), ClientSession.EncounterId, "SOCIAL HISTORY", false);
                SocialHistoryDetails = problemDTO.SocialList;
                StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                IList<StaticLookup> objStaticLookup = new List<StaticLookup>();
                objStaticLookup = objStaticLookupMgr.getStaticLookupByFieldName("SOCIAL HISTORY", "Sort_Order");
                objStaticLookup = objStaticLookup.Where(a => a.Description == "MANDATORY").ToList<StaticLookup>();

                if (ClientSession.IsDirtySocialHistory)
                {
                    foreach (StaticLookup obj in objStaticLookup)
                    {
                        if (SocialHistoryDetails.Any(a => a.Social_Info == obj.Value))
                        {
                            IsPassedValidation = true;
                            break;
                        }
                    }
                }
                else
                {
                    IsPassedValidation = true;
                }

                /*Commented for perfomance tuning - combine the condition IsPassedValidation --jisha*/
                //if (IsPassedValidation)
                //{
                   
                    //ClientSession.bPFSHVerified = true;
                    //EncounterManager objEncounterManager = new EncounterManager();
                    //IList<Encounter> EncList = null;
                    //EncList = objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                    //if (EncList != null && EncList.Count > 0)
                    //{
                    //    Encounter currentEncounter = new Encounter();
                    //    currentEncounter = EncList[0];
                    //    currentEncounter.Is_PFSH_Verified = "Y";
                    //    if (Request["source_information"] != null && Request["source_information"] != string.Empty && Request["source_information"] != "false")
                    //    {
                    //        currentEncounter.Source_Of_Information = Request["source_information"].ToString();

                    //    }
                    //    else
                    //    {
                    //        currentEncounter.Source_Of_Information = "Self";
                    //    }
                    //    currentEncounter.Modified_By = ClientSession.UserName;
                    //    currentEncounter.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
                    //    Session["PFSHEnabled"] = "False";
                    //    objEncounterManager.UpdateEncounter(currentEncounter, string.Empty, new object[] { "false" });
                    //}
                //}
                string ScriptToBeInjected = string.Empty;
                if (IsPassedValidation)
                {
                    if (pnlACOParticipation.Enabled)
                    {
                        if (cboPhysicianNames.Text == string.Empty && cboACODiscusion.Text == "Y")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "PhysicianComboVal", "DisplayErrorMessage('1091011');", true);
                            return;
                        }


                        if (Request["source_information"] != null && Request["source_information"] != string.Empty && Request["source_information"] != "false")
                        {
                            source_information = Request["source_information"].ToString();
                        }
                        else
                        {
                            source_information = "Self";
                        }

                        HumanManager objHumanManager = new HumanManager();

                        if (hdnACOHumanID.Value != "" && cboACODiscusion.SelectedValue != null)
                            objHumanManager.UpdateACOQueries(Convert.ToUInt32(hdnACOHumanID.Value), "", cboACODiscusion.SelectedValue, tblPhysicianDetails.Visible ? Convert.ToUInt32(cboPhysicianNames.SelectedValue) : ClientSession.PhysicianId, ClientSession.EncounterId, Flag, source_information, ClientSession.UserName, UtilityManager.ConvertToUniversal());

                        // ClientSession.bPFSHVerified = false;
                        ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "PhysicianComboVal", "DisplayErrorMessage('1091012');", true);

                    }
                    else
                    {
                        ClientSession.bPFSHVerified = true;
                        EncounterManager objEncounterManager = new EncounterManager();
                        IList<Encounter> EncList = null;
                        EncList = objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                        if (EncList != null && EncList.Count > 0)
                        {
                            Encounter currentEncounter = new Encounter();
                            currentEncounter = EncList[0];
                            currentEncounter.Is_PFSH_Verified = "Y";
                            if (Request["source_information"] != null && Request["source_information"] != string.Empty && Request["source_information"] != "false")
                            {
                                currentEncounter.Source_Of_Information = Request["source_information"].ToString();

                            }
                            else
                            {
                                currentEncounter.Source_Of_Information = "Self";
                            }
                            currentEncounter.Modified_By = ClientSession.UserName;
                            currentEncounter.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
                            Session["PFSHEnabled"] = "False";
                            objEncounterManager.UpdateEncounter(currentEncounter, string.Empty, new object[] { "false" });
                        }

                        
                    }
                    ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "CancelKeyDefault", "var o = new Object();o=true;returnToParent(o);", true);
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('450009');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "CancelKeyDefault", "alert('Please check the mandatory items marked in red color in social history.');", true);
                }

            }

            else if (pnlVerifyPFSH.Visible == false)
            {
                if (pnlACOParticipation.Enabled)
                {
                    if (cboPhysicianNames.Text == string.Empty && cboACODiscusion.Text == "Y")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "PhysicianComboVal", "DisplayErrorMessage('1091011');", true);
                        return;
                    }
                    HumanManager objHumanManager = new HumanManager();
                    Flag = false;
                    objHumanManager.UpdateACOQueries(Convert.ToUInt32(hdnACOHumanID.Value), "", cboACODiscusion.SelectedValue, Convert.ToUInt32(cboPhysicianNames.SelectedValue == "" ? "0" : cboPhysicianNames.SelectedValue), ClientSession.EncounterId, Flag, source_information, ClientSession.UserName, UtilityManager.ConvertToUniversal());
                    ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "PhysicianComboVal", "DisplayErrorMessage('1091012');", true);
                    ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "CancelKeyDefault", "var o = new Object();o=true;returnToParent(o);", true);
                }
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["PFSHEnabled"] = "True";
            ScriptManager.RegisterStartupScript(this, typeof(frmACOValidation), "CancelKeyDefault", "var o = null;returnToParent(o);", true);
        }

        void ToggleShowAllPhysician(string sPhysician_ID)
        {
            PhysicianManager objPhysicianManager = new PhysicianManager();
            MapPhysicianPhysicianAssitantManager ObjUserMgr = new MapPhysicianPhysicianAssitantManager();
            WFObject ehrwfobj = new WFObject();
            ehrwfobj = ClientSession.FillEncounterandWFObject.EncounterWFRecord;

            FillPhysicianUser PhyUserList = new FillPhysicianUser();
            Encounter EncRecord = new Encounter();
            cboPhysicianNames.Items.Clear();

            int iIter = 0;

            //OnLoad to check if the physician name from db for 'discussed by' is available in the physician list-by Pujhitha for bugID:26190
            if (chkShowAllPhysician.Checked == false)
            {
                PhyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName,ClientSession.LegalOrg);

                if (sPhysician_ID.Trim() != string.Empty)
                {
                    bool bPhysician_Found = false;
                    for (int k = 0; k < PhyUserList.PhyList.Count; k++)
                    {
                        if (PhyUserList.PhyList[k].Id.ToString().Trim() == sPhysician_ID.Trim())
                            bPhysician_Found = true;
                    }
                    if (bPhysician_Found == false)
                        chkShowAllPhysician.Checked = true;
                }
            }
            //----------------------------------------------------------------------------------------------------------------
            if (chkShowAllPhysician.Checked == true)
            {
                PhyUserList = objPhysicianManager.GetPhysicianandUser(false, ClientSession.FacilityName, ClientSession.LegalOrg);

                //Restrict The Machine Names  15344  On 18-04-2013 
                if (PhyUserList.PhyList != null && PhyUserList.PhyList.Count > 0)
                {
                    PhyUserList.PhyList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { PhyList = PhysicianList }.PhyList).ToList<PhysicianLibrary>();

                    PhyUserList.UserList = (from PhysicianList in PhyUserList.PhyList join UserList in PhyUserList.UserList on PhysicianList.Id equals UserList.Physician_Library_ID where PhysicianList.Category.ToUpper().Trim() != "MACHINE" select new { UseList = UserList }.UseList).ToList<User>();

                }


                for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                {
                    if (ehrwfobj != null && ehrwfobj.Current_Process == "PROVIDER_PROCESS" && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    {
                        if (PhyUserList.UserList[i].role.ToUpper() == "PHYSICIAN ASSISTANT")
                        {
                            continue;
                        }
                    }

                    string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                    cboPhysicianNames.Items.Add(new ListItem((PhyUserList.UserList[i].user_name.ToString() + " - " + sPhyName)));
                    cboPhysicianNames.Items[iIter].Value = PhyUserList.PhyList[i].Id.ToString();

                    if (Convert.ToUInt64(cboPhysicianNames.Items[iIter].Value) == ClientSession.PhysicianId)
                    {
                        cboPhysicianNames.ClearSelection();
                        cboPhysicianNames.SelectedIndex = iIter;

                        cboPhysicianNames.SelectedValue = ClientSession.PhysicianId.ToString();
                    }
                    iIter = iIter + 1;
                }
            }
            else
            {
                if (ehrwfobj.Current_Process == "PROVIDER_PROCESS")
                {
                    IList<MapPhysicianPhysicianAssistant> mapList = new List<MapPhysicianPhysicianAssistant>();
                    mapList = ObjUserMgr.GetMapPhysicianPhyAsstList(Convert.ToInt32(ClientSession.PhysicianId));

                    for (int i = 0; i < mapList.Count; i++)
                    {
                        PhysicianLibrary phyLib = objPhysicianManager.GetphysiciannameByPhyID(Convert.ToUInt64(mapList[i].Physician_ID))[0];
                        string sPhyName = phyLib.PhyPrefix + " " + phyLib.PhyFirstName + " " + phyLib.PhyMiddleName + " " + phyLib.PhyLastName + " " + phyLib.PhySuffix;

                        var vlist = from v in PhyUserList.UserList where v.Physician_Library_ID == phyLib.Id select v;

                        if (vlist.ToList<User>().Count > 0)
                        {
                            cboPhysicianNames.Items.Add(new ListItem((vlist.ToList<User>()[0].user_name.ToString() + " - " + sPhyName)));
                            cboPhysicianNames.Items[i].Value = phyLib.Id.ToString();

                            if (Convert.ToUInt32(cboPhysicianNames.Items[i].Value) == Convert.ToInt32(EncRecord.Appointment_Provider_ID))
                            {
                                cboPhysicianNames.ClearSelection();
                                cboPhysicianNames.SelectedIndex = i;
                            }
                        }
                    }
                }
                else
                {
                    cboPhysicianNames.Items.Add(new ListItem(string.Empty));
                    cboPhysicianNames.ClearSelection();
                    cboPhysicianNames.SelectedIndex = 0;
                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {
                        string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;

                        cboPhysicianNames.Items.Add(new ListItem((PhyUserList.UserList[i].user_name.ToString() + " - " + sPhyName)));
                        cboPhysicianNames.Items[i + 1].Value = PhyUserList.PhyList[i].Id.ToString();

                        if (Convert.ToUInt64(cboPhysicianNames.Items[i + 1].Value) == ClientSession.PhysicianId)
                        {
                            cboPhysicianNames.ClearSelection();
                            cboPhysicianNames.SelectedIndex = i + 1;
                        }
                    }
                }
            }
        }


        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            string temp = cboPhysicianNames.SelectedValue;
            ToggleShowAllPhysician(string.Empty);
            if (chkShowAllPhysician.Checked && temp != string.Empty)
            {
                cboPhysicianNames.ClearSelection();
                cboPhysicianNames.Items.FindByValue(temp).Selected = true;
            }
            if (btnSave.Enabled == false)
            {
                btnSave.Enabled = true;
            }
        }

        protected void chkPFSHVerified_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void cboACODiscusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled == false)
            {
                btnSave.Enabled = true;
            }
        }

        protected void cboPhysicianNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled == false)
            {
                btnSave.Enabled = true;
            }

        }
    }
}
