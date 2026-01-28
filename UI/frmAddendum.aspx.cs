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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Reflection;
using System.ComponentModel;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.IO;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmAddendum : SessionExpired
    {
        #region Declarations

        IList<AddendumNotes> loadAddendumNotesForPhysicianObj;
        IList<Encounter> EncountList = new List<Encounter>();
        EncounterManager EnctMang = new EncounterManager();
        static ulong curr_AddendumObjID = 0;
        int selectedEncounterNode = 0;
        //bool isLoad = true;
        bool isDirectMoveToProvider = true;
        bool isFormClosingRequired = false;
        bool isAddendumNotesChanged = false;
        DateTime dtLocalTime = DateTime.MinValue;

        #endregion

        #region Enum

        enum UserType
        {
            [Description("PHYSICIAN")]
            eUserType_Physician = 0,

            [Description("PHYSICIAN ASSISTANT")]
            eUserType_PhysicanAssistant = 1,

            [Description("MEDICAL ASSISTANT")]
            eUserType_MedicalAssistant = 2,

            [Description("CODER")]
            eUserType_Coder = 3,

            [Description("FRONT OFFICE")]
            eUserType_FrontOffice = 4,

            [Description("OFFICE MANAGER")]
            eUserType_OfficeManager = 5,
            //CAP-3511
            [Description("TECHNICIAN")]
            eUserType_Technician = 6
        };

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnUserRole.Value = ClientSession.UserRole.ToUpper();

            if (!IsPostBack)
            {
                #region **selectedEncounterNode not initialized in PatientChart(Selected Encounter is not shown by default), hence commented.
                /*selectedEncounterNode = frmPatientChart.selectedEncounterNode;
                if (selectedEncounterNode == -1)
                    return;*/
                #endregion

                if (Request["currentAddendumId"] != null && Request["currentAddendumId"].ToString() != string.Empty)
                    curr_AddendumObjID = Convert.ToUInt32(Request["currentAddendumId"].ToString());
                else
                    curr_AddendumObjID = 0;
                txtAddendumNotes.DName = "pbDropList";

                EncounterManager objEncounterManager = new EncounterManager();
                FillPatientChart objFillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, true, curr_AddendumObjID, "ADDENDUM", true);//0);
                FillEncounterandWFObject objFillEncounterandWFObject = objFillPatientChart.Fill_Encounter_and_WFObject;
                Session["objFillEncounterandWFObject"] = objFillEncounterandWFObject;
                ClientSession.PatientPaneList = objFillPatientChart.PatChartList;

                Title = "Amendment - " + ClientSession.UserName.ToUpper();
                pnlAttestation.Visible = lblAddendumSignedBy.Visible = txtAddendumSignedByText.Visible = lblAddendumSignedDateAndTime.Visible = txtAddendumSignedDateAndTimeText.Visible = btnMoveToProviderReview.Enabled = btnSaveAndClose.Enabled = false;

                if (ClientSession.UserRole!=null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))
                {
                    hdnIsAssistant.Value = "true";
                    pnlAttestation.Visible = lblAddendumTypedDateTime.Visible = txtAddendumTypedDateTimeText.Visible = true;
                }
                //CAP-3511
                else if (ClientSession.UserRole != null && (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician)))
                {
                    pnlAttestation.Visible = true;
                    lblSelectReviewPhysician.Visible = cboShowAllPhysicians.Visible = chkShowAllPhysicians.Visible = false;
                }
                else if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder))
                {
                    //= txtMedicalAssistantText.Enabled = txtMedicalRecNoText.Enabled 
                    txtAccountNoText.Enabled = txtAddendumTypedByText.Enabled = txtAddendumTypedDateTimeText.Enabled
                        = txtDOBText.Enabled = txtDOSText.Enabled = txtEncounterProviderText.Enabled = txtFacilityNameText.Enabled
                        = txtPatientNameText.Enabled = txtSexText.Enabled
                    = txtAddendumNotes.Enable = pnlAttestation.Visible
                    = lblSelectReviewPhysician.Visible = cboShowAllPhysicians.Visible
                    = chkShowAllPhysicians.Visible = btnSaveAndClose.Enabled = false;
                    btnMoveToProviderReview.Enabled = true;
                    lblAddendumSignedBy.Visible = txtAddendumSignedByText.Visible = lblAddendumSignedDateAndTime.Visible = txtAddendumSignedDateAndTimeText.Visible = true;
                    //tblAddendumNotes.SetRowSpan(gbAddendumNotes, 2);
                    pnlAmendmentSource.Enabled = false;
                }
                else
                {
                    lblAddendumTypedBy.Visible = txtAddendumTypedByText.Enabled = lblAddendumTypedDateTime.Visible = txtAddendumTypedDateTimeText.Enabled = false;
                    //tblAddendumNotes.SetRowSpan(gbAddendumNotes, 2);
                }

                //IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
                StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                //iFieldLookupList = objStaticLookupMgr.getStaticLookupByFieldName("IS_ADDENDUM_ACCEPT", "Sort_Order");
                string[] sIsAddendumAccept = new string[] { "", "Y", "N" };
                for (int i = 0; i < sIsAddendumAccept.Count(); i++)
                    cboAcceptOrDeny.Items.Add(new RadComboBoxItem(sIsAddendumAccept[i]));//iFieldLookupList[i].Value
                cboAcceptOrDeny.SelectedIndex = 0;

                loadAddendum();
                //Jira #CAP-700
                //if (rdProvider.Checked)
                //    cboAcceptOrDeny.Enabled = false;
                hdnIsLoad.Value = "false";

                txtAddendumNotes.txtDLC.Focus();

                string sTemp = string.Empty;
                string[] sPhyname = null;

                //IList<StaticLookup> StaticLst = objStaticLookupMgr.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                PhysicianManager objPhysicianManager = new PhysicianManager();
                PhysicianLibrary PhysicianList = objPhysicianManager.GetphysiciannameByPhyID(ClientSession.PhysicianId).Count > 0 ? objPhysicianManager.GetphysiciannameByPhyID(ClientSession.PhysicianId)[0] : new PhysicianLibrary();

                //CAP-3511
                if (ClientSession.UserRole != null && (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician)))
                {
                    //PhysicianLibrary CurrentPhysician=objPhysicianManager.
                    UserManager userMngr = new UserManager();
                    IList<User> userList = new List<User>();
                    userList = userMngr.GetUser(ClientSession.UserName);
                    IList<PhysicianLibrary> CurrentPhysician = objPhysicianManager.GetphysiciannameByPhyID(userList[0].Physician_Library_ID);
                    if (loadAddendumNotesForPhysicianObj.Count > 0 && objFillEncounterandWFObject.AddendumWFRecord.Current_Process == "ADDENDUM_REVIEW")
                    {
                        //sPhyname = StaticLst[0].Value.Split('|');
                        ////sTemp = sPhyname[0].Replace("<Physician>", PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + (string.IsNullOrEmpty(PhysicianList.PhyMiddleName) ? "" : " ") + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix);
                        //sTemp = sPhyname[0].Replace("<Physician>", CurrentPhysician[0].PhyPrefix + " " + CurrentPhysician[0].PhyFirstName + " " + CurrentPhysician[0].PhyMiddleName + (string.IsNullOrEmpty(CurrentPhysician[0].PhyMiddleName) ? "" : " ") + CurrentPhysician[0].PhyLastName + " " + CurrentPhysician[0].PhySuffix);
                        ////sTemp = sPhyname[0].Replace("<Physician>", ClientSession.PersonName);
                        ////string sReplace = sPhyname[1].Replace("on <Date>", "").Replace("<Physician>", PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + (string.IsNullOrEmpty(PhysicianList.PhyMiddleName) ? "" : " ") + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix);
                        //string sReplace = sPhyname[1].Replace("on <Date>", "").Replace("<Physician>", CurrentPhysician[0].PhyPrefix + " " + CurrentPhysician[0].PhyFirstName + " " + CurrentPhysician[0].PhyMiddleName + (string.IsNullOrEmpty(CurrentPhysician[0].PhyMiddleName) ? "" : " ") + CurrentPhysician[0].PhyLastName + " " + CurrentPhysician[0].PhySuffix);
                        //sReplace = sReplace.Replace(" at ", "");

                        sTemp = "Electronically Signed by " + CurrentPhysician[0].PhyPrefix + " " + CurrentPhysician[0].PhyFirstName + " " + CurrentPhysician[0].PhyMiddleName + (string.IsNullOrEmpty(CurrentPhysician[0].PhyMiddleName) ? "" : " ") + CurrentPhysician[0].PhyLastName + " " + CurrentPhysician[0].PhySuffix;
                        chkElectronicDigitalSignature.Text = sTemp;
                    }
                    else
                    {
                        //sPhyname = StaticLst[0].Value.Split('|');
                        ////sTemp = sPhyname[1].Replace("<Physician>", PhysicianList.PhyPrefix + " " + PhysicianList.PhyFirstName + " " + PhysicianList.PhyMiddleName + (string.IsNullOrEmpty(PhysicianList.PhyMiddleName) ? "" : " ") + PhysicianList.PhyLastName + " " + PhysicianList.PhySuffix);
                        //sTemp = sPhyname[1].Replace("<Physician>", CurrentPhysician[0].PhyPrefix + " " + CurrentPhysician[0].PhyFirstName + " " + CurrentPhysician[0].PhyMiddleName + (string.IsNullOrEmpty(CurrentPhysician[0].PhyMiddleName) ? "" : " ") + CurrentPhysician[0].PhyLastName + " " + CurrentPhysician[0].PhySuffix);
                        ////sTemp = sPhyname[1].Replace("<Physician>", ClientSession.PersonName);
                        //sTemp = sTemp.Replace("on <Date>", "");
                        //sTemp = sTemp.Replace(" at", "");
                        sTemp = "Electronically Signed by " + CurrentPhysician[0].PhyPrefix + " " + CurrentPhysician[0].PhyFirstName + " " + CurrentPhysician[0].PhyMiddleName + (string.IsNullOrEmpty(CurrentPhysician[0].PhyMiddleName) ? "" : " ") + CurrentPhysician[0].PhyLastName + " " + CurrentPhysician[0].PhySuffix;
                        chkElectronicDigitalSignature.Text = sTemp;
                    }
                }
                txtAddendumNotes.txtDLC.Attributes.Add("onkeypress", "return enableSave(event);");
                txtAddendumNotes.txtDLC.Attributes.Add("onchange", "return enableSave(event);");
            }
            if (txtAddendumNotes.txtDLC.Text == string.Empty && ClientSession.UserRole.ToUpper() != "CODER")
            {
                btnMoveToProviderReview.Enabled = false;
            }
            else
            {
                btnMoveToProviderReview.Enabled = true;
            }

            if (cboAcceptOrDeny.SelectedItem.Text == string.Empty && ClientSession.UserRole.ToUpper() == "CODER")
            {
                btnMoveToProviderReview.Enabled = false;
            }

            MessageWindow.VisibleStatusbar = false;
        }

        protected void cboShowAllPhysicians_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (hdnIsLoad.Value == "false")
                btnSaveAndClose.Enabled = true;
        }

        protected void chkShowAllPhysicians_CheckedChanged(object sender, EventArgs e)
        {
            FillPhysicianUser phyUserList;
            cboShowAllPhysicians.Items.Clear();

            int iIter = 0;
            
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];
             
            if (chkShowAllPhysicians.Checked == true)
            {
                PhysicianManager objPhysicianManager = new PhysicianManager();
                phyUserList = objPhysicianManager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                if (phyUserList != null)
                {

                    for (int i = 0; i < phyUserList.PhyList.Count; i++)
                    {
                        if (objFillEncounterandWFObject.EncounterWFRecord.Current_Process == "PROVIDER_PROCESS" && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                        {
                            if (phyUserList.UserList[i].role.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))
                                continue;
                        }
                        if (ClientSession.UserRole != null && phyUserList.UserList[i].role.ToUpper() != "PHYSICIAN ASSISTANT" && ClientSession.UserRole == "Physician Assistant")
                        {
                            //Old Code
                            //string sPhyName = phyUserList.PhyList[i].PhyPrefix + " " + phyUserList.PhyList[i].PhyFirstName + " " + phyUserList.PhyList[i].PhyMiddleName + " " + phyUserList.PhyList[i].PhyLastName + " " + phyUserList.PhyList[i].PhySuffix;
                            //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                            //Gitlab# 2485 - Physician Name Display Change
                            string sPhyName = string.Empty;
                            if (phyUserList.PhyList[i].PhyLastName != String.Empty)
                                sPhyName += phyUserList.PhyList[i].PhyLastName;
                            if (phyUserList.PhyList[i].PhyFirstName != String.Empty)
                            {
                                if (sPhyName != String.Empty)
                                    sPhyName += "," + phyUserList.PhyList[i].PhyFirstName;
                                else
                                    sPhyName += phyUserList.PhyList[i].PhyFirstName;
                            }
                            if (phyUserList.PhyList[i].PhyMiddleName != String.Empty)
                                sPhyName += " " + phyUserList.PhyList[i].PhyMiddleName;
                            if (phyUserList.PhyList[i].PhySuffix != String.Empty)
                                sPhyName += "," + phyUserList.PhyList[i].PhySuffix;
                            cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                            cboShowAllPhysicians.Items[iIter].Value = phyUserList.PhyList[i].Id.ToString();

                            if (Convert.ToUInt64(cboShowAllPhysicians.Items[iIter].Value) == ClientSession.PhysicianId)
                                cboShowAllPhysicians.SelectedIndex = iIter;

                            iIter = iIter + 1;
                        }
                        else
                        //GitLab - #4127
                        //if (ClientSession.UserRole != null && ClientSession.UserRole == "Medical Assistant")
                            if (ClientSession.UserRole != null && (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole == "Office Manager"))
                            {
                            //string sPhyName = phyUserList.PhyList[i].PhyPrefix + " " + phyUserList.PhyList[i].PhyFirstName + " " + phyUserList.PhyList[i].PhyMiddleName + " " + phyUserList.PhyList[i].PhyLastName + " " + phyUserList.PhyList[i].PhySuffix;
                            //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                            //Old Code
                            //string sPhyName = phyUserList.PhyList[i].PhyPrefix + " " + phyUserList.PhyList[i].PhyFirstName + " " + phyUserList.PhyList[i].PhyMiddleName + " " + phyUserList.PhyList[i].PhyLastName + " " + phyUserList.PhyList[i].PhySuffix;
                            //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                            //Gitlab# 2485 - Physician Name Display Change
                            string sPhyName = string.Empty;
                            if (phyUserList.PhyList[i].PhyLastName != String.Empty)
                                sPhyName += phyUserList.PhyList[i].PhyLastName;
                            if (phyUserList.PhyList[i].PhyFirstName != String.Empty)
                            {
                                if (sPhyName != String.Empty)
                                    sPhyName += "," + phyUserList.PhyList[i].PhyFirstName;
                                else
                                    sPhyName += phyUserList.PhyList[i].PhyFirstName;
                            }
                            if (phyUserList.PhyList[i].PhyMiddleName != String.Empty)
                                sPhyName += " " + phyUserList.PhyList[i].PhyMiddleName;
                            if (phyUserList.PhyList[i].PhySuffix != String.Empty)
                                sPhyName += "," + phyUserList.PhyList[i].PhySuffix;
                            cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                            cboShowAllPhysicians.Items[iIter].Value = phyUserList.PhyList[i].Id.ToString();

                                if (Convert.ToUInt64(cboShowAllPhysicians.Items[iIter].Value) == ClientSession.PhysicianId)
                                    cboShowAllPhysicians.SelectedIndex = iIter;

                                iIter = iIter + 1;
                            }
                    }
                }


                //spanAcceptOrDeny.Attributes.Remove("class"); /*added*/
                //spanAcceptOrDeny.Attributes.Add("class", "spanstyle");/*black*/

                //Jira #CAP-700
                // spanAcceptOrDeny.Attributes.Remove("class");

                //spanAcceptOrDeny.Attributes.Add("class", "MandLabelstyle");




            }
            else
            {
                PhysicianManager objPhysicianManager = new PhysicianManager();
                phyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);

                if (objFillEncounterandWFObject.EncounterWFRecord.Current_Process == "PROVIDER_PROCESS")
                {
                    IList<MapPhysicianPhysicianAssistant> mapList = new List<MapPhysicianPhysicianAssistant>();
                    MapPhysicianPhysicianAssitantManager objMapPhysicianPhysicianAssitantManager = new MapPhysicianPhysicianAssitantManager();

                    mapList = objMapPhysicianPhysicianAssitantManager.GetMapPhysicianPhyAsstList(Convert.ToInt32(ClientSession.PhysicianId));

                    for (int i = 0; i < mapList.Count; i++)
                    {
                        PhysicianLibrary phyLib = objPhysicianManager.GetphysiciannameByPhyID((ulong)mapList[i].Physician_ID).Count > 0 ? objPhysicianManager.GetphysiciannameByPhyID((ulong)mapList[i].Physician_ID)[0] : new PhysicianLibrary();
                        //Old Code
                        //string sPhyName = phyLib.PhyPrefix + " " + phyLib.PhyFirstName + " " + phyLib.PhyMiddleName + " " + phyLib.PhyLastName + " " + phyLib.PhySuffix;
                        //Gitlab# 2485 - Physician Name Display Change
                        string sPhyName = string.Empty;
                        if (phyUserList.PhyList[i].PhyLastName != String.Empty)
                            sPhyName += phyUserList.PhyList[i].PhyLastName;
                        if (phyUserList.PhyList[i].PhyFirstName != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + phyUserList.PhyList[i].PhyFirstName;
                            else
                                sPhyName += phyUserList.PhyList[i].PhyFirstName;
                        }
                        if (phyUserList.PhyList[i].PhyMiddleName != String.Empty)
                            sPhyName += " " + phyUserList.PhyList[i].PhyMiddleName;
                        if (phyUserList.PhyList[i].PhySuffix != String.Empty)
                            sPhyName += "," + phyUserList.PhyList[i].PhySuffix;

                        var vlist = from v in phyUserList.UserList where v.Physician_Library_ID == phyLib.Id select v;

                        if (vlist.ToList<User>().Count > 0)
                        {
                            //Old Code
                            //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(vlist.ToList<User>()[0].user_name.ToString() + " - " + sPhyName));
                            //Gitlab# 2485 - Physician Name Display Change
                            cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                            cboShowAllPhysicians.Items[i].Value = phyLib.Id.ToString();
                            if (Convert.ToUInt32(cboShowAllPhysicians.Items[i].Value) == Convert.ToInt32(objFillEncounterandWFObject.EncRecord.Appointment_Provider_ID))
                                cboShowAllPhysicians.SelectedIndex = i;
                        }
                    }
                }
                else
                {
                    int j = 0;
                    for (int i = 0; i < phyUserList.PhyList.Count; i++)
                    {
                        //Old Code
                        //string sPhyName = phyUserList.PhyList[i].PhyPrefix + " " + phyUserList.PhyList[i].PhyFirstName + " " + phyUserList.PhyList[i].PhyMiddleName + " " + phyUserList.PhyList[i].PhyLastName + " " + phyUserList.PhyList[i].PhySuffix;
                        //Gitlab# 2485 - Physician Name Display Change
                        string sPhyName = string.Empty;
                        if (phyUserList.PhyList[i].PhyLastName != String.Empty)
                            sPhyName += phyUserList.PhyList[i].PhyLastName;
                        if (phyUserList.PhyList[i].PhyFirstName != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + phyUserList.PhyList[i].PhyFirstName;
                            else
                                sPhyName += phyUserList.PhyList[i].PhyFirstName;
                        }
                        if (phyUserList.PhyList[i].PhyMiddleName != String.Empty)
                            sPhyName += " " + phyUserList.PhyList[i].PhyMiddleName;
                        if (phyUserList.PhyList[i].PhySuffix != String.Empty)
                            sPhyName += "," + phyUserList.PhyList[i].PhySuffix;
                        if (phyUserList.UserList[i].role.ToUpper() != "PHYSICIAN ASSISTANT" && ClientSession.UserRole == "Physician Assistant")
                        {
                            //Old Code
                            //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                            //Gitlab# 2485 - Physician Name Display Change
                            cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                            cboShowAllPhysicians.Items[j].Value = phyUserList.PhyList[i].Id.ToString();

                            if (Convert.ToUInt64(cboShowAllPhysicians.Items[j].Value) == ClientSession.PhysicianId)
                                cboShowAllPhysicians.SelectedIndex = j;
                            j++;
                        }
                        else
                            //GitLab - #4127
                            //if (ClientSession.UserRole != null && ClientSession.UserRole == "Medical Assistant" )
                            if (ClientSession.UserRole!=null && (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole == "Office Manager"))
                            {
                            //Old Code
                            //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                            //Gitlab# 2485 - Physician Name Display Change
                            cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                            cboShowAllPhysicians.Items[j].Value = phyUserList.PhyList[i].Id.ToString();

                                if (Convert.ToUInt64(cboShowAllPhysicians.Items[j].Value) == ClientSession.PhysicianId)
                                    cboShowAllPhysicians.SelectedIndex = j;
                                j++;
                            }
                    }
                }

                //spanAcceptOrDeny.Attributes.Add("class", "MandLabelstyle");/*red*/
                //spanAcceptOrDeny.Visible = true;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void btnMoveToProviderReview_Click(object sender, EventArgs e)
        {
            //Added by srividhya on 5-Aug-2014
            /*if (rdPatient.Checked == false && rdProvider.Checked == false && ClientSession.UserRole.ToUpper() != "CODER")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7490010');", true);
                return;
            }

            if (rdPatient.Checked && cboAcceptOrDeny.SelectedItem.Text == "" && ClientSession.UserRole.ToUpper() != "CODER")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7490011');", true);
                return;
            }

            if (txtAddendumNotes.txtDLC.Text.Trim() == string.Empty && ClientSession.UserRole.ToUpper() != "CODER")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7490012');", true);
                return;
            }*/

            AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];
             

            //if (frmMyQueue.currentAddendumId > 0 && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "openReviewOfException", "OpenException();", true);
            //    return;
            //}
            //else
            //{
            if (curr_AddendumObjID > 0 && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openReviewOfException", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}OpenException();", true);
                return;
            }
            else
            {
                if (ViewState["loadAddendumNotesForPhysicianObj"] != null)
                    loadAddendumNotesForPhysicianObj = (IList<AddendumNotes>)ViewState["loadAddendumNotesForPhysicianObj"];

                if (cboShowAllPhysicians.Text == string.Empty && (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_MedicalAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_OfficeManager) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490003');", true);
                    return;
                }
                //CAP-3511
                //if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))
                if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
                {
                    if ((loadAddendumNotesForPhysicianObj.Count == 0 && chkElectronicDigitalSignature.Checked == false) || (chkElectronicDigitalSignature.Checked == false))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490001');", true);
                        return;
                    }
                }

                bool isReview = false;
                AddendumNotes addendumNotes = new AddendumNotes();
                IList<AddendumNotes> addendumList = new List<AddendumNotes>();
                IList<AddendumNotes> tempList = new List<AddendumNotes>();

                if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_MedicalAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_OfficeManager))
                {
                    if (loadAddendumNotesForPhysicianObj.Count > 0)
                    {
                        addendumNotes = loadAddendumNotesForPhysicianObj[0];
                        addendumNotes.Provider_Signed_ID = 0;
                    }
                    else
                    {
                        addendumNotes.Human_ID = ClientSession.HumanId;
                        addendumNotes.Encounter_ID = ClientSession.EncounterId;
                        UserManager objUserManager = new UserManager();
                        IList<User> userList = objUserManager.GetUserList(ClientSession.LegalOrg);
                        string userName = userList.Any(a => a.user_name.ToUpper() == ClientSession.UserName.ToUpper()) ? userList.Where(d => d.user_name.ToUpper() == ClientSession.UserName.ToUpper()).Select(u => u.person_name).ToList()[0] : string.Empty;
                        addendumNotes.Created_By = userName;
                        addendumNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                        addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    
                    }

                    addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
                    addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
                    /*if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
                    {
                        string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                        txtAddendumNotes.txtDLC.Text = string.Empty;
                        if (split.Count() > 0)
                        {
                            for (int i = 0; i < split.Count(); i++)
                            {
                                split[i].Replace("\n", "");
                                txtAddendumNotes.txtDLC.Text += split[i].ToString();
                            }

                        }

                    }*/
                    addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
                    addendumList.Add(addendumNotes);

                    string sOwner = string.Empty;

                    if (loadAddendumNotesForPhysicianObj.Count > 0)
                    {
                        //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                        addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                        //Old Code
                        //objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, cboShowAllPhysicians.Text.Split('-')[0].Trim(), string.Empty, false, true, 1, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
                        //Gitlab# 2485 - Physician Name Display Change
                        //CAP-2788
                        sOwner = GetOwner();
                        objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, sOwner, string.Empty, false, true, 1, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);


                    }
                    else
                    {
                        //Old Code
                        //objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, cboShowAllPhysicians.Text.Split('-')[0].Trim(), string.Empty, true, true, 1, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
                        //Gitlab# 2485 - Physician Name Display Change
                        //CAP-2788
                        sOwner = GetOwner();
                        objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, sOwner, string.Empty, true, true, 1, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
                    }
                }
                //CAP-3511
                //else if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician))
                else if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
                {
                    if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() != "" && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() != "ADDENDUM_CORRECTION")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "isReview();", true);
                        return;

                        //int result = 1;// ApplicationObject.erroHandler.DisplayErrorMessage("7490005", this.Text);

                        //if (result == 1)
                        //    isReview = true;
                        //else if (result == 2)
                        //    isReview = false;
                        //else
                        //    return;
                    }

                    if (loadAddendumNotesForPhysicianObj.Count > 0)
                    {
                        addendumNotes = loadAddendumNotesForPhysicianObj[0];
                        //addendumNotes.Provider_Signed_ID = 0;
                    }
                    else
                    {
                        addendumNotes.Human_ID = ClientSession.HumanId;
                        addendumNotes.Encounter_ID = ClientSession.EncounterId;
                        addendumNotes.Created_By = ClientSession.UserName;
                        if (hdnLocalTime.Value != "")
                        {
                            addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                            addendumNotes.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                            //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                            addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                        }
                        else
                        {
                            addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                            addendumNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                            addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                        }
                    }
                    if (objFillEncounterandWFObject != null && objFillEncounterandWFObject.AddendumWFRecord != null && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW")
                    {
                        addendumNotes.Provider_Review_Signed_ID = ClientSession.CurrentPhysicianId;
                        addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))
                    {
                        addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId;//ClientSession.PhysicianId;
                        addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                    //CAP-3511
                    //if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician))
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
                    {
                        //if (loadAddendumNotesForPhysicianObj.Count > 0 && loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID > 0)
                        //{
                        //    addendumNotes.Provider_Review_Signed_ID = ClientSession.CurrentPhysicianId;// ClientSession.PhysicianId;
                        //    addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                        //}
                        if (objFillEncounterandWFObject != null && objFillEncounterandWFObject.AddendumWFRecord != null && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW")
                        {
                            addendumNotes.Provider_Review_Signed_ID = ClientSession.CurrentPhysicianId;
                            addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                        }
                        else
                        {
                            addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId; //ClientSession.PhysicianId;
                            addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                        }
                    }

                    addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
                    addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
                   /* if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
                    {
                        string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                        txtAddendumNotes.txtDLC.Text = string.Empty;
                        if (split.Count() > 0)
                        {
                            for (int i = 0; i < split.Count(); i++)
                            {
                                split[i].Replace("\n", "");
                                txtAddendumNotes.txtDLC.Text += split[i].ToString();
                            }

                        }

                    }*/
                    addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
                    addendumList.Add(addendumNotes);

                    string sOwner = string.Empty;

                    if (loadAddendumNotesForPhysicianObj.Count > 0)
                    {
                        //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                        addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                        //Old Code
                        //objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && !isReview) || ClientSession.UserRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : cboShowAllPhysicians.Text.Split('-')[0].Trim(), string.Empty, false, isReview, ClientSession.UserRole == "Physician Assistant" ? !isReview ? 1 : objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" ? 1 : 2 : 1, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
                        //Gitlab# 2485 - Physician Name Display Change
                        //CAP-2788
                        sOwner = GetOwner();
                        objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && !isReview) || ClientSession.UserRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : sOwner, string.Empty, false, isReview, ClientSession.UserRole == "Physician Assistant" ? !isReview ? 1 : objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" ? 1 : 2 : 1, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
                    }
                    else
                    {
                        //Old Code
                        //objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, isDirectMoveToProvider && !isReview ? "UNKNOWN" : cboShowAllPhysicians.Text.Split('-')[0].Trim(), ClientSession.UserRole, true, isReview, ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && isReview ? 3 : 2, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
                        //Gitlab# 2485 - Physician Name Display Change
                        //CAP-2788
                        sOwner = GetOwner();
                        objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, isDirectMoveToProvider && !isReview ? "UNKNOWN" : sOwner, ClientSession.UserRole, true, isReview, ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && isReview ? 3 : 2, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
                    }

                }
                else if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder))
                {
                    WFObjectManager objWFObjectManager = new WFObjectManager();
                    //  objWFObjectManager.MoveToNextProcess(frmMyQueue.currentAddendumId, "ADDENDUM", 1, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), ApplicationObject.macAddress, null, null);
                    objWFObjectManager.MoveToNextProcess(curr_AddendumObjID, "ADDENDUM", 1, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty, null, null);
                    ulong encID = addendumNotes.Encounter_ID;
                    //EncounterManager encounterMngr = new EncounterManager();
                    //if (encID != 0)
                    //{
                    //IList<Encounter> encRecord = encounterMngr.GetEncounterByEncounterID(encID);
                    //encRecord[0].Is_Progressnote_Generated = "N";
                    //IList<Encounter> emptySave = null;
                    //encounterMngr.SaveUpdateDeleteWithTransaction(ref emptySave, encRecord, null, string.Empty);
                    // }
                }

                //CAP-2614
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "Saved();DisplayErrorMessage('7490002'); window.top.location.href =  'frmMyQueueNew.aspx'", true);
                //frmMyQueue.currentAddendumId = 0;
                curr_AddendumObjID = 0;
                isFormClosingRequired = true;
                closeFormEvent();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close()", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            closeFormEvent();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close()", true);
        }

        protected void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            //Added by srividhya on 5-Aug-2014
            /*if (rdPatient.Checked == false && rdProvider.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7490010');", true);
                return;
            }

            if (rdPatient.Checked == true && cboAcceptOrDeny.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7490011');", true);
                return;
            }

            if (txtAddendumNotes.txtDLC.Text.Trim() == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7490012');", true);
                return;
            }*/

            if (cboShowAllPhysicians.Text == string.Empty && (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_MedicalAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_OfficeManager) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490003');", true);
                return;
            }

            AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];

            isDirectMoveToProvider = false;

            AddendumNotes addendumNotes = new AddendumNotes();
            IList<AddendumNotes> addendumList = new List<AddendumNotes>();
            IList<AddendumNotes> tempList = new List<AddendumNotes>();

            if (ViewState["loadAddendumNotesForPhysicianObj"] != null)
                loadAddendumNotesForPhysicianObj = (IList<AddendumNotes>)ViewState["loadAddendumNotesForPhysicianObj"];

            if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_MedicalAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_OfficeManager))
            {
                UserManager objUserManager = new UserManager();
                IList<User> userList = objUserManager.GetUserList(ClientSession.LegalOrg);
                if (loadAddendumNotesForPhysicianObj != null && loadAddendumNotesForPhysicianObj.Count > 0)
                    addendumNotes = loadAddendumNotesForPhysicianObj[0];
                else
                {
                    addendumNotes.Human_ID = ClientSession.HumanId;
                    addendumNotes.Encounter_ID = ClientSession.EncounterId;
                    string userName = userList.Any(a => a.user_name.ToUpper() == ClientSession.UserName.ToUpper()) ? userList.Where(d => d.user_name.ToUpper() == ClientSession.UserName.ToUpper()).Select(u => u.person_name).ToList()[0] : string.Empty;
                    addendumNotes.Created_By = userName;
                    addendumNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                    addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                }

                addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
                addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
               /* if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
                {
                    string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                    txtAddendumNotes.txtDLC.Text = string.Empty;
                    if (split !=  null && split.Count() > 0)
                    {
                        for (int i = 0; i < split.Count(); i++)
                        {
                            split[i].Replace("\n", "");
                            txtAddendumNotes.txtDLC.Text += split[i].ToString();
                        }

                    }

                }*/
                addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
                //addendumNotes.Provider_Signed_ID = userList.Any(a => a.user_name.ToUpper() == cboShowAllPhysicians.Text.Split('-')[0].Trim().ToUpper()) ? userList.Where(d => d.user_name.ToUpper() == cboShowAllPhysicians.Text.Split('-')[0].Trim().ToUpper()).Select(u => u.Physician_Library_ID).ToList()[0] : 0;
               // addendumNotes.Provider_Signed_ID = ulong.Parse(cboShowAllPhysicians.SelectedItem.Value);
               // addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId;
                if (chkElectronicDigitalSignature.Checked)
                {
                    if (objFillEncounterandWFObject!=null && objFillEncounterandWFObject.AddendumWFRecord != null && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW")
                    {
                        addendumNotes.Provider_Review_Signed_ID = ClientSession.CurrentPhysicianId;
                        addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                    else
                    {
                        addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId;// ulong.Parse(cboShowAllPhysicians.SelectedItem.Value);//Commented for Bug ID:37103
                        //addendumNotes.Provider_Signed_ID = userList.Any(a => a.user_name.ToUpper() == cboShowAllPhysicians.Text.Split('-')[0].Trim().ToUpper()) ? userList.Where(d => d.user_name.ToUpper() == cboShowAllPhysicians.Text.Split('-')[0].Trim().ToUpper()).Select(u => u.Physician_Library_ID).ToList()[0] : 0;
                        addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                }
                addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                addendumList.Add(addendumNotes);

                if (loadAddendumNotesForPhysicianObj != null && loadAddendumNotesForPhysicianObj.Count > 0)
                {
                    //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                    addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, ClientSession.UserName, ClientSession.UserRole, true, false, 0, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
                }
                else
                        objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, ClientSession.UserName, ClientSession.UserRole, true, false, 0, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
            }
            //CAP-3511
            //else if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician))
            else if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
            {
                if (loadAddendumNotesForPhysicianObj != null && loadAddendumNotesForPhysicianObj.Count > 0)
                    addendumNotes = loadAddendumNotesForPhysicianObj[0];
                else
                {
                    addendumNotes.Human_ID = ClientSession.HumanId;
                    addendumNotes.Encounter_ID = ClientSession.EncounterId;
                    addendumNotes.Created_By = ClientSession.UserName;
                    addendumNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                    addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                }

                addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
                addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
               /* if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
                {
                    string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                    txtAddendumNotes.txtDLC.Text = string.Empty;
                    if (split.Count() > 0)
                    {
                        for (int i = 0; i < split.Count(); i++)
                        {

                            txtAddendumNotes.txtDLC.Text += split[i].ToString();
                        }

                    }

                }*/
                addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
               // if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))

                if (chkElectronicDigitalSignature.Checked)
                {
                    if (objFillEncounterandWFObject != null && objFillEncounterandWFObject.AddendumWFRecord != null && objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW")
                    {
                        addendumNotes.Provider_Review_Signed_ID = ClientSession.CurrentPhysicianId;
                        addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                    else
                    {
                        addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId;// ulong.Parse(cboShowAllPhysicians.SelectedItem.Value);//Commented for Bug ID:37103
                        //addendumNotes.Provider_Signed_ID = userList.Any(a => a.user_name.ToUpper() == cboShowAllPhysicians.Text.Split('-')[0].Trim().ToUpper()) ? userList.Where(d => d.user_name.ToUpper() == cboShowAllPhysicians.Text.Split('-')[0].Trim().ToUpper()).Select(u => u.Physician_Library_ID).ToList()[0] : 0;
                        addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                }
                addendumList.Add(addendumNotes);

                if (loadAddendumNotesForPhysicianObj!=null&&loadAddendumNotesForPhysicianObj.Count > 0)
                {
                    //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                    if (addendumList != null && addendumList.Count>0&& addendumList[0].Created_Date_And_Time != null)
                        addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, ClientSession.UserName, ClientSession.UserRole, true, false, 0, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
                }
                else
                    objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, ClientSession.UserName, ClientSession.UserRole, true, false, 0, isDirectMoveToProvider, string.Empty, false);//, dtLocalTime);
            }

            //  frmMyQueue.currentAddendumId = 0;
            curr_AddendumObjID = 0;
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "Saved();DisplayErrorMessage('7490007');", true);
            isFormClosingRequired = true;
            closeFormEvent();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close()", true);
        }

        protected void btnReview_Click(object sender, EventArgs e)
        {

            if (rdPatient.Checked == false && rdProvider.Checked == false && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490010');", true);
                return;
            }

            if (rdPatient.Checked && cboAcceptOrDeny.SelectedItem.Text == "" && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490011');", true);
                return;
            }

            bool isReview = true;
            AddendumNotes addendumNotes = new AddendumNotes();
            IList<AddendumNotes> addendumList = new List<AddendumNotes>();
            IList<AddendumNotes> tempList = new List<AddendumNotes>();

            if (ViewState["loadAddendumNotesForPhysicianObj"] != null)
                loadAddendumNotesForPhysicianObj = (IList<AddendumNotes>)ViewState["loadAddendumNotesForPhysicianObj"];

            if (loadAddendumNotesForPhysicianObj.Count > 0)
            {
                addendumNotes = loadAddendumNotesForPhysicianObj[0];
                //addendumNotes.Provider_Signed_ID = 0;
            }
            else
            {
                addendumNotes.Human_ID = ClientSession.HumanId;
                addendumNotes.Encounter_ID = ClientSession.EncounterId;
                addendumNotes.Created_By = ClientSession.UserName;
                addendumNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
            }

            if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))
            {
                addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId;//ClientSession.PhysicianId;ClientSession.currentPhysician_ID;
                addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
            }
            //CAP-3511
            //if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician))
            if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
            {
                if (loadAddendumNotesForPhysicianObj.Count > 0 && loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID > 0)
                {
                    addendumNotes.Provider_Review_Signed_ID = ClientSession.PhysicianId;//ClientSession.currentPhysician_ID;
                    addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId; //ClientSession.PhysicianId;ClientSession.currentPhysician_ID;
                    addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
            }

            addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
            addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
           /* if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
            {
                string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                txtAddendumNotes.txtDLC.Text = string.Empty;
                if (split.Count() > 0)
                {
                    for (int i = 0; i < split.Count(); i++)
                    {
                        split[i].Replace("\n", "");
                        txtAddendumNotes.txtDLC.Text += split[i].ToString();
                    }

                }

            }*/
            addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
            addendumList.Add(addendumNotes);

            AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];

            if (loadAddendumNotesForPhysicianObj.Count > 0)
            {
                //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, "UNKNOWN", string.Empty, false, isReview, 1, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
            }
            else
                objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, "UNKNOWN", ClientSession.UserRole, true, isReview, 1, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "Saved();DisplayErrorMessage('7490002');", true);
            // frmMyQueue.currentAddendumId = 0;
            curr_AddendumObjID = 0;
            isFormClosingRequired = true;

            closeFormEvent();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close()", true);
        }

        protected void btnIsReview_Click(object sender, EventArgs e)
        {

            if (rdPatient.Checked == false && rdProvider.Checked == false && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490010');", true);
                return;
            }

            if (rdPatient.Checked && cboAcceptOrDeny.SelectedItem.Text == "" && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "NotSaved();DisplayErrorMessage('7490011');", true);
                return;
            }

            bool isReview = true;
            AddendumNotes addendumNotes = new AddendumNotes();
            IList<AddendumNotes> addendumList = new List<AddendumNotes>();
            IList<AddendumNotes> tempList = new List<AddendumNotes>();

            if (ViewState["loadAddendumNotesForPhysicianObj"] != null)
                loadAddendumNotesForPhysicianObj = (IList<AddendumNotes>)ViewState["loadAddendumNotesForPhysicianObj"];

            if (loadAddendumNotesForPhysicianObj.Count > 0)
            {
                addendumNotes = loadAddendumNotesForPhysicianObj[0];
                //addendumNotes.Provider_Signed_ID = 0;
            }
            else
            {
                addendumNotes.Human_ID = ClientSession.HumanId;
                addendumNotes.Encounter_ID = ClientSession.EncounterId;
                addendumNotes.Created_By = ClientSession.UserName;
                addendumNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                //dtLocalTime = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time);
                addendumNotes.Local_Time = UtilityManager.ConvertToLocal(addendumNotes.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
            }

            if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant))
            {
                addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId;//ClientSession.PhysicianId;ClientSession.currentPhysician_ID;
                addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
            }
            //CAP-3511
            //if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician))
            if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
            {
                if (loadAddendumNotesForPhysicianObj.Count > 0 && loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID > 0)
                {
                    addendumNotes.Provider_Review_Signed_ID = ClientSession.PhysicianId;//ClientSession.currentPhysician_ID;
                    addendumNotes.Provider_Review_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    addendumNotes.Provider_Signed_ID = ClientSession.CurrentPhysicianId; //ClientSession.PhysicianId;ClientSession.currentPhysician_ID;
                    addendumNotes.Provider_Signed_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
            }

            addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
            addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
            /*if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
            {
                string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                txtAddendumNotes.txtDLC.Text = string.Empty;
                if (split.Count() > 0)
                {
                    for (int i = 0; i < split.Count(); i++)
                    {
                        split[i].Replace("\n", "");
                        txtAddendumNotes.txtDLC.Text += split[i].ToString();
                    }

                }

            }*/
            addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
            addendumList.Add(addendumNotes);

            AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];

            string sOwner = string.Empty;
            if (loadAddendumNotesForPhysicianObj.Count > 0)
            {
                //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                //Old Code
                //objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && !isReview) || ClientSession.UserRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : cboShowAllPhysicians.Text.Split('-')[0].Trim(), string.Empty, false, isReview, ClientSession.UserRole == "Physician Assistant" ? !isReview ? 1 : objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" ? 1 : 2 : 1, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
                //Gitlab# 2485 - Physician Name Display Change
                //CAP-2788
                sOwner = GetOwner();
                objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && !isReview) || ClientSession.UserRole.ToUpper() == "PHYSICIAN" ? "UNKNOWN" : sOwner, string.Empty, false, isReview, ClientSession.UserRole == "Physician Assistant" ? !isReview ? 1 : objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" ? 1 : 2 : 1, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
            }
            else
            {
                //Old Code
                //objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, isDirectMoveToProvider && !isReview ? "UNKNOWN" : cboShowAllPhysicians.Text.Split('-')[0].Trim(), ClientSession.UserRole, true, isReview, ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && isReview ? 3 : 2, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
                //Gitlab# 2485 - Physician Name Display Change
                //CAP-2788
                sOwner = GetOwner();
                objAddendumNotesManager.saveUpdateAddendum(addendumList, tempList, objFillEncounterandWFObject.EncRecord.Facility_Name, isDirectMoveToProvider && !isReview ? "UNKNOWN" : sOwner, ClientSession.UserRole, true, isReview, ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && isReview ? 3 : 2, isDirectMoveToProvider, string.Empty, true);//, dtLocalTime);
            }
                

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "Saved();DisplayErrorMessage('7490002');", true);
            // frmMyQueue.currentAddendumId = 0;
            curr_AddendumObjID = 0;
            isFormClosingRequired = true;

            closeFormEvent();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close()", true);
        }

        protected void btnIsClose_Click(object sender, EventArgs e)
        {
            //frmMyQueue.currentAddendumId = 0;Commented for Addendum_Correction Workflow
            curr_AddendumObjID = 0;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close();refreshChart(" + ClientSession.EncounterId.ToString() + "); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        #endregion

        #region Method

        private void loadAddendum()
        {
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];
            //CAP-3511
            //if ((ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)
            //    || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder)) && (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION"
            //    || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW" || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CODING"
            //    || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CODING_2"))
            if ((ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician)
                || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder) || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)) && (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION"
                || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW" || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CODING"
                || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CODING_2"))
                lblAddendumSignedBy.Visible = txtAddendumSignedByText.Visible = lblAddendumSignedDateAndTime.Visible = txtAddendumSignedDateAndTimeText.Visible = true;
            int iIndex = -1;
            for (int iLoop = 0; iLoop < ClientSession.PatientPaneList.Count; iLoop++)
            {
                if (ClientSession.PatientPaneList[iLoop].Encounter_ID == ClientSession.EncounterId)
                {
                    iIndex = iLoop;
                    break;
                }
            }
            if (iIndex > -1)
            {
                txtPatientNameText.Text = ClientSession.PatientPaneList[iIndex].Last_Name + "," + ClientSession.PatientPaneList[selectedEncounterNode].First_Name;
                txtDOBText.Text = ClientSession.PatientPaneList[iIndex].Birth_Date.ToString("dd-MMM-yyyy");
                txtSexText.Text = ClientSession.PatientPaneList[iIndex].Sex;
                txtAccountNoText.Text = ClientSession.PatientPaneList[iIndex].Human_Id.ToString();
                //txtMedicalRecNoText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Medical_Record_Number;
                txtFacilityNameText.Text = objFillEncounterandWFObject.EncRecord.Facility_Name;
                txtDOSText.Text = UtilityManager.ConvertToLocal(ClientSession.PatientPaneList[iIndex].Date_of_Service).ToString("dd-MMM-yyyy");
                txtEncounterProviderText.Text = ClientSession.PatientPaneList[iIndex].Assigned_Physician;
            }
            #region **selectedEncounterNode not initialized in PatientChart(Selected Encounter is not shown by default), hence commented.
            /*txtPatientNameText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Last_Name + "," + ClientSession.PatientPaneList[selectedEncounterNode].First_Name;
            txtDOBText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Birth_Date.ToString("dd-MMM-yyyy");
            txtSexText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Sex;
            txtAccountNoText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Human_Id.ToString();
            //txtMedicalRecNoText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Medical_Record_Number;
            txtFacilityNameText.Text = objFillEncounterandWFObject.EncRecord.Facility_Name;
            txtDOSText.Text = UtilityManager.ConvertToLocal(ClientSession.PatientPaneList[selectedEncounterNode].Date_of_Service).ToString("dd-MMM-yyyy");
            txtEncounterProviderText.Text = ClientSession.PatientPaneList[selectedEncounterNode].Assigned_Physician;*/
            //txtMedicalAssistantText.Text = userList.Where(u => u.user_name == ClientSession.PatientPaneList[selectedEncounterNode].Assigned_Medical_Asst).Count() > 0 ? userList.Where(u => u.user_name == ClientSession.PatientPaneList[selectedEncounterNode].Assigned_Medical_Asst).ToList()[0].person_name : string.Empty;
            #endregion
            //CAP-3511
            if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_MedicalAssistant)
                || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician)
                || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)
                || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder)
                || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_OfficeManager)
                || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician))
            {
                loadAddendumNotesForPhysicianObj = new List<AddendumNotes>();
                AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
                //loadAddendumNotesForPhysicianObj = objAddendumNotesManager.getAddendumNotesForPhysician(frmMyQueue.currentAddendumId, ClientSession.EncounterId);
                loadAddendumNotesForPhysicianObj = objAddendumNotesManager.getAddendumNotesForPhysician(curr_AddendumObjID, ClientSession.EncounterId);
                ViewState["loadAddendumNotesForPhysicianObj"] = loadAddendumNotesForPhysicianObj;

                if (loadAddendumNotesForPhysicianObj.Count > 0)
                {
                    rdPatient.Checked = loadAddendumNotesForPhysicianObj[0].Addendum_Source.ToUpper() == "PATIENT" ? true : false;
                    rdProvider.Checked = loadAddendumNotesForPhysicianObj[0].Addendum_Source.ToUpper() == "PROVIDER" ? true : false;
                    if (loadAddendumNotesForPhysicianObj[0].Is_Accept.Trim() != "")
                        cboAcceptOrDeny.SelectedIndex = loadAddendumNotesForPhysicianObj[0].Is_Accept.ToUpper() == "Y" ? 1 : 2;
                    if (txtAddendumTypedByText.Visible)
                    {
                        IList<User> userList = new List<User>();
                        UserManager objUserManager=new UserManager();
                        userList = objUserManager.GetUserList(ClientSession.LegalOrg);
                        txtAddendumTypedByText.Text = loadAddendumNotesForPhysicianObj[0].Created_By == string.Empty ? ClientSession.UserName : userList.Where(u => u.user_name.ToUpper() == loadAddendumNotesForPhysicianObj[0].Created_By.ToUpper()).Count() > 0 ? userList.Where(u => u.user_name.ToUpper() == loadAddendumNotesForPhysicianObj[0].Created_By.ToUpper()).ToList()[0].person_name : loadAddendumNotesForPhysicianObj[0].Created_By;
                    }
                    txtAddendumTypedDateTimeText.Text = UtilityManager.ConvertToLocal(loadAddendumNotesForPhysicianObj[0].Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    txtAddendumNotes.txtDLC.Text = loadAddendumNotesForPhysicianObj[0].Addendum_Notes;

                    if (loadAddendumNotesForPhysicianObj[0].Encounter_ID != 0)
                    {
                        EncountList = EnctMang.GetEncounterByEncounterIDIncludeArchive(loadAddendumNotesForPhysicianObj[0].Encounter_ID);
                        if (EncountList.Count > 0)
                        {
                            txtFacilityNameText.Text = EncountList[0].Facility_Name;
                        }
                    }
                    //CAP-3511
                    //if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder)
                    //    || ((ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician)
                    //    || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)) && (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW")))
                    if (ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Coder)
                        || ((ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Physician)
                        || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_PhysicanAssistant)
                        || ClientSession.UserRole.ToUpper() == GetEnumDescription(UserType.eUserType_Technician)) && (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_REVIEW")))
                    {
                        ulong phyId = (loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID > 0 && loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID > 0) ? loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID : (loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID > 0) ? loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID : loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID;

                        PhysicianManager objPhysicianManager = new PhysicianManager();
                        PhysicianLibrary phyLib = objPhysicianManager.GetphysiciannameByPhyID(ClientSession.PhysicianId).Count > 0 ? objPhysicianManager.GetphysiciannameByPhyID(phyId)[0] : new PhysicianLibrary();

                        txtAddendumSignedByText.Text = phyLib != null ? phyLib.PhyPrefix + " " + phyLib.PhyFirstName + " " + phyLib.PhyMiddleName + " " + phyLib.PhyLastName + " " + phyLib.PhySuffix : string.Empty;

                        txtAddendumSignedDateAndTimeText.Text = loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID > 0 && loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID > 0 ? UtilityManager.ConvertToLocal(loadAddendumNotesForPhysicianObj[0].Provider_Signed_Date_And_Time).ToString("dd-MMM-yyyy hh:mm:ss tt") : (loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_ID > 0) ? UtilityManager.ConvertToLocal(loadAddendumNotesForPhysicianObj[0].Provider_Review_Signed_Date_And_Time).ToString("dd-MMM-yyyy hh:mm:ss tt") : UtilityManager.ConvertToLocal(loadAddendumNotesForPhysicianObj[0].Provider_Signed_Date_And_Time).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    }
                    if (txtAddendumTypedByText.Visible == true)
                    {
                        lblAddendumTypedBy.Visible = true;
                    }
                    if (txtAddendumTypedDateTimeText.Visible == true)
                    {
                        lblAddendumTypedDateTime.Visible = true;
                    }
                    //if(rdProvider.Checked==true)
                    //{
                    //    lblAcceptOrDeny.Text = "Accept or Deny ?";
                    //    lblAcceptOrDeny.ForeColor = System.Drawing.Color.Black;
                    //}
                    //else
                    //{
                    //    lblAcceptOrDeny.Text = "Accept or Deny ?*";
                    //    lblAcceptOrDeny.ForeColor = System.Drawing.Color.Red;
                    //}
                    if(rdProvider.Checked==true)
                    {

                        // spanAcceptOrDeny.Attributes.Remove("class"); /*added*/
                        //spanAcceptOrDeny.Attributes.Add("class", "spanstyle");/*black*/

                        spanAcceptOrDeny.Attributes.Remove("class");
                        spanAcceptOrDeny.Attributes.Add("class", "color:black;");
                        spanAcceptOrDeny.InnerText = "Accept or Deny ?";
                    }
                    else
                    {
                        //spanAcceptOrDeny.Text = "Accept or Deny ?*";
                        //spanAcceptOrDeny.ForeColor = System.Drawing.Color.Red;

                        //spanAcceptOrDeny.Attributes.Add("class", "MandLabelstyle");/*red*/
                        //spanAcceptOrDeny.Attributes.Add("class", "manredforstar");
                        spanAcceptOrDeny.Attributes.Remove("class");
                        spanAcceptOrDeny.Attributes.Add("class", "manredforstar");
                        spanAcceptOrDeny.InnerText = "Accept or Deny ?*";
                        spanAcceptOrDeny.Visible = true;


                    }

                }
                else
                {
                    if (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CODING" || objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CODING_2")
                        lblAddendumSignedBy.Visible = txtAddendumSignedByText.Visible = lblAddendumSignedDateAndTime.Visible = txtAddendumSignedDateAndTimeText.Visible = false;

                    txtAddendumTypedByText.Visible = lblAddendumTypedBy.Visible = lblAddendumTypedDateTime.Visible = txtAddendumTypedDateTimeText.Visible = btnMoveToProviderReview.Enabled = false;
                }
            }

            if (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Office Manager")
            {
                if (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" && loadAddendumNotesForPhysicianObj.Count > 0)
                    lblSelectReviewPhysician.Visible = cboShowAllPhysicians.Visible = chkShowAllPhysicians.Visible = false;
                else
                {
                    PhysicianManager objPhysicianManager = new PhysicianManager();
                    FillPhysicianUser phyUserList = objPhysicianManager.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);
                    int iLoop = 0;
                    if (loadAddendumNotesForPhysicianObj.Count > 0)
                    {
                        var phy = from obj in phyUserList.PhyList where obj.Id == loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID select obj;
                        if (phy.ToList<PhysicianLibrary>().Count == 0 || phy == null)
                        {
                            iLoop = 1;
                            cboShowAllPhysicians.Items.Add(new RadComboBoxItem(""));
                            cboShowAllPhysicians.Items[0].Value = "0";
                        }
                    }
                    int j = 0;
                    for (int i = 0; i < phyUserList.PhyList.Count; i++)
                    {
                        bool bAdded = false;
                        //Old Code
                        //string sPhyName = phyUserList.PhyList[i].PhyPrefix + " " + phyUserList.PhyList[i].PhyFirstName + " " + phyUserList.PhyList[i].PhyMiddleName + " " + phyUserList.PhyList[i].PhyLastName + " " + phyUserList.PhyList[i].PhySuffix;
                        //Gitlab# 2485 - Physician Name Display Change
                        string sPhyName = string.Empty;
                        if (phyUserList.PhyList[i].PhyLastName != String.Empty)
                            sPhyName += phyUserList.PhyList[i].PhyLastName;
                        if (phyUserList.PhyList[i].PhyFirstName != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + phyUserList.PhyList[i].PhyFirstName;
                            else
                                sPhyName += phyUserList.PhyList[i].PhyFirstName;
                        }
                        if (phyUserList.PhyList[i].PhyMiddleName != String.Empty)
                            sPhyName += " " + phyUserList.PhyList[i].PhyMiddleName;
                        if (phyUserList.PhyList[i].PhySuffix != String.Empty)
                            sPhyName += "," + phyUserList.PhyList[i].PhySuffix;
                        if (phyUserList.UserList[i].role.ToUpper() != "PHYSICIAN ASSISTANT" && ClientSession.UserRole == "Physician Assistant")
                        {
                            if (iLoop == 1)
                            {
                                //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                                cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                                cboShowAllPhysicians.Items[j + 1].Value = phyUserList.PhyList[i].Id.ToString();
                                bAdded = true;
                            }
                            else
                            {
                                //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                                cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                                cboShowAllPhysicians.Items[j].Value = phyUserList.PhyList[i].Id.ToString();
                                bAdded = true;
                            }
                            if (loadAddendumNotesForPhysicianObj.Count > 0 && Convert.ToUInt32(cboShowAllPhysicians.Items[j].Value) == loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID)
                                cboShowAllPhysicians.SelectedIndex = j;
                            else if (Convert.ToUInt32(cboShowAllPhysicians.Items[j].Value) == Convert.ToInt32(objFillEncounterandWFObject.EncRecord.Encounter_Provider_ID) && cboShowAllPhysicians.SelectedIndex == 0 && loadAddendumNotesForPhysicianObj.Count == 0)
                                cboShowAllPhysicians.SelectedIndex = j;
                            if (bAdded == true)
                                j++;
                        }
                        else
                            if (ClientSession.UserRole != "Physician Assistant")
                            {
                                if (iLoop == 1)
                                {
                                //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                                cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                                cboShowAllPhysicians.Items[j + 1].Value = phyUserList.PhyList[i].Id.ToString();
                                    bAdded = true;
                                }
                                else
                                {
                                //cboShowAllPhysicians.Items.Add(new RadComboBoxItem(phyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
                                cboShowAllPhysicians.Items.Add(new RadComboBoxItem(sPhyName));
                                cboShowAllPhysicians.Items[j].Value = phyUserList.PhyList[i].Id.ToString();
                                    bAdded = true;
                                }
                                if (loadAddendumNotesForPhysicianObj.Count > 0 && Convert.ToUInt32(cboShowAllPhysicians.Items[j].Value) == loadAddendumNotesForPhysicianObj[0].Provider_Signed_ID)
                                    cboShowAllPhysicians.SelectedIndex = j;
                                else if (Convert.ToUInt32(cboShowAllPhysicians.Items[j].Value) == Convert.ToInt32(objFillEncounterandWFObject.EncRecord.Encounter_Provider_ID) && cboShowAllPhysicians.SelectedIndex == 0 && loadAddendumNotesForPhysicianObj.Count == 0)
                                    cboShowAllPhysicians.SelectedIndex = j;
                                if (bAdded == true)
                                    j++;
                            }
                    }
                }
            }
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        private void closeFormEvent()
        {
            FillEncounterandWFObject objFillEncounterandWFObject = (FillEncounterandWFObject)Session["objFillEncounterandWFObject"];

            if (ClientSession.UserRole.ToUpper() != GetEnumDescription(UserType.eUserType_Coder) && !isFormClosingRequired)
            {
                //   if (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" && frmMyQueue.currentAddendumId > 0)
                if (objFillEncounterandWFObject.AddendumWFRecord.Current_Process.ToUpper() == "ADDENDUM_CORRECTION" && curr_AddendumObjID > 0)
                {
                    //if (System.Windows.Forms.Application.OpenForms.Cast<RadForm>().Any(f => f.Name == "frmReviewOfException"))
                    //    System.Windows.Forms.Application.OpenForms.Cast<RadForm>().Where(f => f.Name == "frmReviewOfException").ToList()[0].Close();

                    AddendumNotes addendumNotes = new AddendumNotes();
                    IList<AddendumNotes> addendumList = new List<AddendumNotes>();
                    IList<AddendumNotes> tempList = new List<AddendumNotes>();

                    if (ViewState["loadAddendumNotesForPhysicianObj"] != null)
                        loadAddendumNotesForPhysicianObj = (IList<AddendumNotes>)ViewState["loadAddendumNotesForPhysicianObj"];

                    if (loadAddendumNotesForPhysicianObj.Count > 0 && txtAddendumNotes.txtDLC.Text != loadAddendumNotesForPhysicianObj[0].Addendum_Notes)
                    {
                        addendumNotes = loadAddendumNotesForPhysicianObj[0];
                       /* if (txtAddendumNotes.txtDLC.Text != "" && txtAddendumNotes.txtDLC.Text.Contains("\n"))
                        {
                            string[] split = txtAddendumNotes.txtDLC.Text.Split('\n');
                            txtAddendumNotes.txtDLC.Text = string.Empty;
                            if (split.Count() > 0)
                            {
                                for (int i = 0; i < split.Count(); i++)
                                {
                                    split[i].Replace("\n", "");
                                    txtAddendumNotes.txtDLC.Text += split[i].ToString();
                                }

                            }

                        }*/
                        addendumNotes.Addendum_Notes = txtAddendumNotes.txtDLC.Text;
                        addendumNotes.Addendum_Source = rdProvider.Checked ? "PROVIDER" : "PATIENT";
                        addendumNotes.Is_Accept = cboAcceptOrDeny.SelectedItem.Text;
                        addendumList.Add(addendumNotes);
                        AddendumNotesManager objAddendumNotesManager = new AddendumNotesManager();
                        //dtLocalTime = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time);
                        addendumList[0].Local_Time = UtilityManager.ConvertToLocal(addendumList[0].Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                        objAddendumNotesManager.saveUpdateAddendum(tempList, addendumList, objFillEncounterandWFObject.EncRecord.Facility_Name, ClientSession.UserName, ClientSession.UserRole, true, false, 0, isDirectMoveToProvider, string.Empty, false);//,dtLocalTime);
                    }
                    //frmMyQueue.currentAddendumId = 0;Commented for Addendum_Correction Workflow
                    curr_AddendumObjID = 0;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close();refreshChart(" + ClientSession.EncounterId.ToString() + "); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
                //else if (isAddendumNotesChanged)
                //{
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "closeMessage();", true);

                    // if (ApplicationObject.erroHandler.DisplayErrorMessage("7490008", this.Text) == 1)
                    //frmMyQueue.currentAddendumId = 0;Commented for Addendum_Correction Workflow
                    //else
                    //    e.Cancel = true;

                    //if (ApplicationObject.erroHandler.DisplayErrorMessage("7490008", this.Text) == 1)
                    //curr_AddendumObjID = 0;Commented for Addendum_Correction Workflow
                    //else
                    //    e.Cancel = true;
                //}
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeAddendum", "window.close();refreshChart(" + ClientSession.EncounterId.ToString() + "); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        #endregion

        protected void rdPatient_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveAndClose.Enabled = true;
            btnMoveToProviderReview.Enabled = true;
            //cboAcceptOrDeny.Enabled = true;
            //lblAcceptOrDeny.ForeColor = System.Drawing.Color.Red;
            //Jira #CAP-700 -oldcode
            //spanAcceptOrDeny.Attributes.Remove("class"); /*added*/
            //spanAcceptOrDeny.Attributes.Add("class", "spanstyle");/*black*/

            //if(chkShowAllPhysicians.Checked == true)
            //{
            //    spanAcceptOrDeny.Attributes.Remove("class");

            //  spanAcceptOrDeny.Attributes.Add("class", "MandLabelstyle");
            //}
            //Jira #CAP-700-newcode
            spanAcceptOrDeny.Attributes.Remove("class");
            spanAcceptOrDeny.Attributes.Add("class", "manredforstar");
            spanAcceptOrDeny.InnerText = "Accept or Deny ?*";

        }

        protected void rdProvider_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveAndClose.Enabled = true;
            btnMoveToProviderReview.Enabled = true;
            //Jira #CAP-700
            //cboAcceptOrDeny.Enabled = false;

            //lblAcceptOrDeny.ForeColor = System.Drawing.Color.Black;
            //span1.ForeColor = System.Drawing.Color.Black;
            spanAcceptOrDeny.Attributes.Remove("class"); /*added*/
            //Jira #CAP-700
            //spanAcceptOrDeny.Attributes.Add("class", "spanstyle");/*black*/

            spanAcceptOrDeny.Attributes.Add("class", "color:black;");
            spanAcceptOrDeny.InnerText = "Accept or Deny ?";


        }

        protected void cboAcceptOrDeny_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnSaveAndClose.Enabled = true;
            btnMoveToProviderReview.Enabled = true;
        }

        protected void chkElectronicDigitalSignature_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveAndClose.Enabled = true;
            btnMoveToProviderReview.Enabled = true;
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {

        }
        //CAP-2788
        private string GetOwner()
        {
            UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
            if (ilstUserList?.User != null)
            {
                var filteredData = ilstUserList?.User.FirstOrDefault(a => a.Physician_Library_ID.ToUpper() == cboShowAllPhysicians.SelectedValue && a.Legal_Org.ToUpper() == ClientSession.LegalOrg);
                if (filteredData != null)
                {
                    return filteredData.User_Name.ToUpper();
                }
            }
            return string.Empty;
        }
    }
}
