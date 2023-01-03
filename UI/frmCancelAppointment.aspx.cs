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
using System.IO;

namespace Acurus.Capella.UI
{
    public partial class frmCancelAppointment : System.Web.UI.Page
    {
        StaticLookupManager StaticMngr = new StaticLookupManager();
        EncounterManager EncounterMngr = new EncounterManager();
        WFObjectManager wfMngr = new WFObjectManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientSession.FlushSession();
                FillReasonCode();
                if (Request["EncounterID"] != null)
                {
                    hdnEncounterID.Value = Request["EncounterID"].ToString();
                }
                if (ClientSession.PhysicianId != null && ClientSession.PhysicianId > 0)
                {
                    hdnPhysicianId.Value = ClientSession.PhysicianId.ToString();
                }

                if (Request["PhoneEncounter"] != null)
                {
                    hdnPhoneEncounter.Value = Request["PhoneEncounter"].ToString();
                    // this.Page.Title = "Phone Encounter Cancel Appointment - " + ClientSession.UserName;
                }
                this.Page.Title = "Cancel Appointment" + "-" + ClientSession.UserName;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);

                txtReasonText.Attributes.Add("maxlength", "500");
            }
        }
        private void FillReasonCode()
        {
            IList<StaticLookup> fieldlist;
            FieldLookup lookup;


            fieldlist = StaticMngr.getStaticLookupByFieldName("CANCEL REASON CODE", "Sort_Order");


            ddlReasonCode.Items.Clear();

            //added by Ginu on 2-Aug-2010
            if (fieldlist != null)
            {
                for (int i = 0; i < fieldlist.Count; i++)
                {
                    lookup = fieldlist[i];
                    ddlReasonCode.Items.Add(lookup.Value);
                }
            }
            ddlReasonCode.Items.Add("Other");
            ddlReasonCode.SelectedIndex = 0;
            if (ddlReasonCode.Text.Trim() != "Other")
            {
                txtReasonText.Text = ddlReasonCode.Text;
                //rchtxtReasonText.Focus();
                //rchtxtReasonText.SelectionStart= cboReasonCode.Text.Length;

                // TextBoxColorChange(txtReasonText, false);
            }
            else
            {
                txtReasonText.Text = string.Empty;
                txtReasonText.Focus();

                // TextBoxColorChange(txtReasonText, true);
            }

        }

        protected void btnOkForAppt_Click(object sender, EventArgs e)
        {
            if (txtReasonText.Text.Trim() == string.Empty)
            {
                //ApplicationObject.erroHandler.DisplayErrorMessage("110033", "Cancel Appointment", this.Page);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110033');", true);
                return;
            }
            if (hdnEncounterID.Value != string.Empty)
            {
                Encounter EncRecord = new Encounter(); ;

                IList<Encounter> EncList = EncounterMngr.GetEncounterByEncounterID(Convert.ToUInt64(hdnEncounterID.Value));
                if (EncList != null && EncList.Count > 0)
                {
                    EncRecord = EncList[0];
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "DisplayErrorMessage('110090');", true);
                    return;
                }


                EncRecord.Reason_for_Cancelation = txtReasonText.Text;
                EncRecord.Cancelation_Reason_Code = ddlReasonCode.Text;
                if (hdnLocalTime.Value != string.Empty)
                {
                    EncRecord.Modified_Date_and_Time = Convert.ToDateTime(hdnLocalTime.Value);
                }
                EncRecord.Modified_By = ClientSession.UserName;

                //string FileName = "Encounter" + "_" + EncRecord.Id.ToString() + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                //if (File.Exists(strXmlFilePath) == false)
                //{
                    EncounterMngr.UpdateEncounterForRCM(EncRecord, null, false, string.Empty, string.Empty, new object[] { "false" });
                //}
                //else
                //{
                //    EncounterMngr.UpdateEncounterList(EncRecord, string.Empty);
                //}

                // EncounterMngr.UpdateEncounterForRCM(EncRecord, string.Empty, string.Empty, new object[] { "false" });

                //sMyReason = EncRecord.Reason_for_Cancelation;
                if (hdnPhoneEncounter.Value == "Y")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "PhoneEncounterCancel(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    divLoading.Style.Add("display", "none");
                }
                else
                {
                    wfMngr.MoveToNextProcess(Convert.ToUInt64(hdnEncounterID.Value), "ENCOUNTER", 3, "UNKNOWN", Convert.ToDateTime(hdnLocalTime.Value), string.Empty, null, null);
                    EncounterMngr.TriggerSPforProvReviewStatusTracker("INVALID", EncRecord.Id);//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                    //ApplicationObject.erroHandler.DisplayErrorMessage("110011", "Cancel Appointment", this.Page);
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Edit Appointment", "CancelAppointment(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    divLoading.Style.Add("display", "none");
                }
            }
        }

        protected void ddlReasonCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReasonCode.Text.Trim() != "Other")
            {
                txtReasonText.Text = ddlReasonCode.Text;
                //rchtxtReasonText.Focus();
                //rchtxtReasonText.SelectionStart= cboReasonCode.Text.Length;
                txtReasonText.ReadOnly = true;

                txtReasonText.CssClass = "nonEditabletxtbox";
            }
            else
            {
                txtReasonText.Text = string.Empty;
                txtReasonText.Focus();
                txtReasonText.ReadOnly = false;
                txtReasonText.CssClass = "Editabletxtbox";
            }
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Cancel Appointment", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void TextBoxColorChange(TextBox txtbox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                txtbox.ReadOnly = true;
                txtbox.BackColor = System.Drawing.Color.FromArgb(191, 219, 255);

            }
            else
            {
                txtbox.ReadOnly = false;
                //txtbox.BackColor = System.Drawing.Color.White;
                txtbox.Attributes.Add("class", "Editabletxtbox");



            }
        }

        protected void pbReasonCodeLibrary_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
