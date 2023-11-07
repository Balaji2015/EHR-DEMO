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
using Acurus.Capella.Core.DTO;
using System.Text.RegularExpressions;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.UI;
using Telerik.Web.UI;
using System.IO;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Web.Services;

using System.Drawing;




namespace Acurus.Capella.UI
{
    public partial class frmPatientPayment : System.Web.UI.Page
    {
        FillQuickPatient objCheckOutLoad = null;
        HumanManager HumanMngr = new HumanManager();
        FillQuickPatient objCheckOut;
        IList<VisitPayment> SaveVisitPaymentList = new List<VisitPayment>();
        IList<Check> SaveCheckList = new List<Check>();
        IList<PPHeader> SavePPHeaderList = new List<PPHeader>();
        IList<PPLineItem> SavePPLineItemList = new List<PPLineItem>();
        IList<AccountTransaction> SaveAccountTransactionList = new List<AccountTransaction>();
        //IList<VisitPaymentArc> SaveVisitPaymentArcList = new List<VisitPaymentArc>();
        IList<CheckArc> SaveCheckArcList = new List<CheckArc>();
        //IList<PPHeaderArc> SavePPHeaderArcList = new List<PPHeaderArc>();
        //IList<PPLineItemArc> SavePPLineItemArcList = new List<PPLineItemArc>();
        //IList<AccountTransactionArc> SaveAccountTransactionArcList = new List<AccountTransactionArc>();
        IList<VisitPaymentHistory> SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
        //IList<VisitPaymentHistoryArc> SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();
        VisitPaymentManager visitMgr = new VisitPaymentManager();
        VisitPaymentArcManager visitArcMgr = new VisitPaymentArcManager();
        bool bValidPaymentInfo = false;
        StaticLookupManager staticLookupMngr = new StaticLookupManager();



        ulong humanID = 0;
        ulong ulEncounterID = 0;
        ulong GroupID = 0;
        int iReturn = 0;
        decimal PaymentAmount = 0;
        decimal RefundAmount = 0;
        decimal RecOnAcc = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Request["HumanId"] != null && Request["HumanId"] != null)
            {
                humanID = Convert.ToUInt64(Request["HumanId"]);
            }
            else
            {
                humanID = ClientSession.HumanId;
            }
            if (Request["EncounterID"] != null && Request["EncounterID"] != null)
            {
                ulEncounterID = Convert.ToUInt64(Request["EncounterID"]);
            }
            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);

            if (ClientSession.FacilityName != null)
            {
                hdnFacilityRole.Value = ClientSession.FacilityName;
            }
            if (hdndate.Value != "")
                lblcollection.Text = hdndate.Value;
            if (!IsPostBack)
            {
                ddlFacilitywithDOS.Attributes.Add("onchange", "Loading();");
                cboRelation.Attributes.Add("onchange", "enableAdd();");
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                hdnHumID.Value = humanID.ToString();


                int iCount = 0;
                EncounterManager objEncMngr = new EncounterManager();
                DateTime sCurrentDate;
                sCurrentDate = DateTime.Now;
                IList<Encounter> ilstEncounter = objEncMngr.GetEncountersforFacilityDOS(humanID, sCurrentDate, ClientSession.FacilityName);

                string ScreenMode = string.Empty;
                if (Request["sScreenMode"] != null)
                {
                    ScreenMode = Request["sScreenMode"];
                }

                int index = 0;
                if (ilstEncounter != null && ilstEncounter.Count > 0)
                {
                    ddlFacilitywithDOS.Items.Add("");
                    ddlVoucher.Items.Add("");
                    for (iCount = 0; iCount < ilstEncounter.Count; iCount++)
                    {

                        if (ilstEncounter[iCount].Is_Phone_Encounter != "Y" && ilstEncounter[iCount].Cancelation_Reason_Code == string.Empty && ilstEncounter[iCount].Reschedule_Reason_Code == string.Empty)
                        {
                            System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                            cboItem.Text = ilstEncounter[iCount].Facility_Name + " - " + UtilityManager.ConvertToLocal(ilstEncounter[iCount].Appointment_Date).ToString("dd-MMM-yyyy hh:mm tt");
                            cboItem.Value = ilstEncounter[iCount].Id.ToString() + "|" + ilstEncounter[iCount].Batch_Status.ToString() + "|" + ilstEncounter[iCount].Exam_Room.ToString();
                            hdnBatchStatus.Value = ilstEncounter[iCount].Batch_Status.ToString();
                            this.ddlFacilitywithDOS.Items.Add(cboItem);

                            System.Web.UI.WebControls.ListItem Encid = new System.Web.UI.WebControls.ListItem();
                            Encid.Text = ilstEncounter[iCount].Id.ToString();
                            this.ddlVoucher.Items.Add(Encid);
                            if (ulEncounterID.ToString() == ilstEncounter[iCount].Id.ToString())
                            {
                                //Cap - 1326
                                //index = iCount+1;
                                index = iCount;

                            }
                        }
                        //if (ScreenMode == "COLLECT COPAY")
                        //{
                        //    if (ilstEncounter[iCount].Id == ulEncounterID)
                        //        ddlFacilitywithDOS.SelectedIndex = iCount;
                        //}


                    }
                }
                //if (ScreenMode == "COLLECT COPAY")
                //{
                //    ddlFacilitywithDOS.Enabled = true;
                //    rdbFacilityName.Checked = true;
                //}
                if (ScreenMode == "COLLECT COPAY")
                {
                    rdbNewCollection.Enabled = false;
                    rdbVoucher.Enabled = false;
                    rdbFacilityName.Checked = true;
                    rdbFacilityName.Enabled = false;

                    ddlVoucher.CssClass = "nonEditabletxtbox";
                    ddlVoucher.Enabled = false;

                    ddlFacilitywithDOS.CssClass = "nonEditabletxtbox";
                    ddlFacilitywithDOS.Enabled = false;
                    ddlFacilitywithDOS.SelectedIndex = index;

                    lblnote.Visible = false;
                }
                DateTimePickerColorChangeForWindows(dtpCheckDate, false);
                MaskedTextBoxColorChange(dtpCheckDate, false);
                LoadPaymentInformation();

                ddlFacilityWithDOS_SelectedIndexChanged(sender, e);
                cboMethodOfPayment_SelectedIndexChanged(sender, e);
                ddlVoucher.CssClass = "nonEditabletxtbox";
                ddlVoucher.Enabled = false;

                ddlFacilitywithDOS.CssClass = "nonEditabletxtbox";
                ddlFacilitywithDOS.Enabled = false;

                //rdbFacilityName.Checked = true;
                cboMethodOfPayment.Enabled = true;
                cboMethodOfPayment.CssClass = "Editabletxtbox";

            }
        ln:
            try
            {
                string sdivPatientstrip = UtilityManager.FillPatientStrip(humanID);
                if (sdivPatientstrip != null)
                {
                    divPatientstrip.InnerText = sdivPatientstrip;
                }
            }
            catch (Exception ex)
            {
                //XmlText.Close();
                //Thread.Sleep(5000);
                UtilityManager.GenerateXML(humanID.ToString(), "Human");

                goto ln;
            }

            //string FileName = "Human" + "_" + humanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //try
            //{
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            try
            //            {
            //                itemDoc.Load(fs);
            //                XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
            //                Human objFillHuman = new Human();
            //                IList<Human> lstHuman = new List<Human>();
            //                if (xmlhumanList != null && xmlhumanList.Count > 0)
            //                {
            //                    objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
            //                    objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
            //                    objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
            //                    objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
            //                    objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
            //                    objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
            //                    objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
            //                    objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
            //                    objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
            //                    objFillHuman.Human_Type = xmlhumanList[0].Attributes.GetNamedItem("Human_Type").Value;
            //                    objFillHuman.Patient_Account_External = xmlhumanList[0].Attributes.GetNamedItem("Patient_Account_External").Value;
            //                    objFillHuman.Guarantor_Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Last_Name").Value;
            //                    objFillHuman.Guarantor_Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Last_Name").Value;
            //                    objFillHuman.Guarantor_MI = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_MI").Value;
            //                    objFillHuman.Guarantor_Sex = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Sex").Value;
            //                    objFillHuman.Guarantor_Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Street_Address1").Value;
            //                    objFillHuman.Street_Address1 = xmlhumanList[0].Attributes.GetNamedItem("Street_Address1").Value;
            //                    objFillHuman.Guarantor_Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Guarantor_Birth_Date").Value);
            //                    objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
            //                    lstHuman.Add(objFillHuman);
            //                }

            //                string phoneno = "";
            //                if (lstHuman != null && lstHuman.Count > 0)
            //                {
            //                    if (objFillHuman.Home_Phone_No.Length == 14)
            //                    {
            //                        phoneno = objFillHuman.Home_Phone_No;
            //                    }
            //                    else
            //                    {
            //                        phoneno = objFillHuman.Cell_Phone_Number;
            //                    }

            //                }

            //                string sPatientSex = string.Empty;
            //                if (objFillHuman.Sex != string.Empty)
            //                {
            //                    if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
            //                    {
            //                        sPatientSex = "UNK";
            //                    }
            //                    else
            //                    {
            //                        sPatientSex = objFillHuman.Sex.Substring(0, 1);
            //                    }
            //                }
            //                else
            //                {
            //                    sPatientSex = "";
            //                }
            //                if (lstHuman != null && lstHuman.Count > 0)
            //                {
            //                    if (objFillHuman.Patient_Account_External == "")
            //                    {
            //                        divPatientstrip.InnerText = humanID + "~" + objFillHuman.Last_Name + "~" + objFillHuman.First_Name + "~0~" + objFillHuman.Human_Type + "~"
            //                            + ClientSession.EncounterId + "~" + ClientSession.UserCurrentProcess +
            //                            "~" + sPatientSex + "~" + objFillHuman.Street_Address1 + "~" + objFillHuman.Birth_Date
            //                            + "~" + objFillHuman.Guarantor_Birth_Date + "~" + objFillHuman.Guarantor_First_Name + "~" + objFillHuman.Guarantor_Last_Name + "~" +
            //                            objFillHuman.Guarantor_MI + "~" + objFillHuman.Guarantor_Sex + "~" + objFillHuman.MI + "~" + objFillHuman.Suffix;
            //                        hdnPatientName.Value = divPatientstrip.InnerText.Split('|')[0];
            //                    }
            //                    else
            //                    {
            //                        divPatientstrip.InnerText = humanID + "~" + objFillHuman.Last_Name + "~" + objFillHuman.First_Name + "~"
            //                            + objFillHuman.Patient_Account_External + "~" + objFillHuman.Human_Type + "~" +
            //                            ClientSession.EncounterId + "~" + ClientSession.UserCurrentProcess +
            //                            "~" + sPatientSex + "~" + objFillHuman.Street_Address1 + "~" + objFillHuman.Birth_Date
            //                            + "~" + objFillHuman.Guarantor_Birth_Date + "~" + objFillHuman.Guarantor_First_Name + "~" + objFillHuman.Guarantor_Last_Name + "~" +
            //                            objFillHuman.Guarantor_MI + "~" + objFillHuman.Guarantor_Sex + "~" + objFillHuman.MI + "~" + objFillHuman.Suffix;
            //                        ;
            //                        hdnPatientName.Value = divPatientstrip.InnerText.Split('|')[0];
            //                    }

            //                    divPatientstrip.InnerText = " " + objFillHuman.Last_Name + "," + objFillHuman.First_Name +
            //       "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + " | " +
            //        objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
            //       (CalculateAge(objFillHuman.Birth_Date)).ToString() +
            //       "  year(s) | " + sPatientSex + " | Acc #:" + humanID +
            //       " | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
            //       "Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type;
            //                }

            //            }

            //            catch (Exception Ex)
            //            {

            //                // ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + humanID.ToString() + "','Human','payment');", true);


            //                //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

            //                return;
            //            }
            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message + " - " + strXmlFilePath);
            //}
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
        private void LoadPaymentInformation()
        {
            string[] FieldName = { "METHOD OF PAYMENT", "RELATIONSHIP_FOR_PAYMENT" };

            StaticLookupManager staticLookUpMngr = new StaticLookupManager();
            IList<StaticLookup> StaticLookupList = staticLookUpMngr.getStaticLookupByFieldName(FieldName);

            IList<StaticLookup> METHODOFPAYMENTStaticLst = null;
            METHODOFPAYMENTStaticLst = StaticLookupList.Where(l => l.Field_Name == "METHOD OF PAYMENT").ToList<StaticLookup>();

            cboMethodOfPayment.Items.Clear();
            cboMethodOfPayment.Items.Add("");

            if (METHODOFPAYMENTStaticLst != null && METHODOFPAYMENTStaticLst.Count > 0)
            {
                foreach (FieldLookup j in METHODOFPAYMENTStaticLst)
                {
                    cboMethodOfPayment.Items.Add(j.Value);
                }
            }

            IList<StaticLookup> RelationshipStaticLst = null;
            RelationshipStaticLst = StaticLookupList.Where(l => l.Field_Name == "RELATIONSHIP_FOR_PAYMENT").ToList<StaticLookup>();

            if (RelationshipStaticLst != null && RelationshipStaticLst.Count > 0)
            {

                foreach (FieldLookup j in RelationshipStaticLst)
                {
                    cboRelation.Items.Add(j.Value);
                }
            }
        }

        public int CalculateAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;
            return years;
        }

        private void LoadPaymentInfoGrid(IList<VisitPaymentDTO> VisitPaymentDTO)
        {
            grdPaymentInformation.DataSource = null;
            grdPaymentInformation.DataBind();
            DataTable dtInsuranace = new DataTable();
            dtInsuranace.Columns.Add("MethodofPayment", typeof(string));
            dtInsuranace.Columns.Add("CheckCardNo", typeof(string));
            dtInsuranace.Columns.Add("AuthNo", typeof(string));
            dtInsuranace.Columns.Add("PastDue", typeof(string));
            dtInsuranace.Columns.Add("PatientPayment", typeof(string));
            dtInsuranace.Columns.Add("RecOnAcc", typeof(string));
            dtInsuranace.Columns.Add("RefundAmount", typeof(string));
            dtInsuranace.Columns.Add("CheckDate", typeof(string));
            dtInsuranace.Columns.Add("PaymentNotes", typeof(string));
            dtInsuranace.Columns.Add("VisitID", typeof(string));
            dtInsuranace.Columns.Add("PPHeaderID", typeof(string));
            dtInsuranace.Columns.Add("PPLineID", typeof(string));
            dtInsuranace.Columns.Add("CheckID", typeof(string));
            dtInsuranace.Columns.Add("relationship", typeof(string));
            dtInsuranace.Columns.Add("Amtpaidby", typeof(string));
            dtInsuranace.Columns.Add("receiptdate", typeof(string));
            dtInsuranace.Columns.Add("PaymentNote", typeof(string));
            dtInsuranace.Columns.Add("TransactionDate&Time", typeof(string));
            dtInsuranace.Columns.Add("VoucherNo", typeof(string));


            if (VisitPaymentDTO != null && VisitPaymentDTO.Count > 0)
            {
                for (int i = 0; i < VisitPaymentDTO.Count; i++)
                {
                    DataRow dr = dtInsuranace.NewRow();
                    dr["MethodofPayment"] = VisitPaymentDTO[i].Method_of_Payment;
                    dr["CheckCardNo"] = VisitPaymentDTO[i].Check_Card_No;
                    dr["AuthNo"] = VisitPaymentDTO[i].Auth_No;
                    dr["PatientPayment"] = VisitPaymentDTO[i].Patient_Payment;
                    PaymentAmount = PaymentAmount + VisitPaymentDTO[i].Patient_Payment;
                    dr["RefundAmount"] = VisitPaymentDTO[i].Refund_Amount;
                    RefundAmount = RefundAmount + VisitPaymentDTO[i].Refund_Amount;
                    dr["RecOnAcc"] = VisitPaymentDTO[i].Rec_On_Acc;
                    RecOnAcc = RecOnAcc + VisitPaymentDTO[i].Rec_On_Acc;
                    dr["PastDue"] = hdnPastDue.Value;  // objCheckOutLoad.HumanObj.Past_Due; //txtPastDue.Text;
                    if (VisitPaymentDTO[i].Check_Date != DateTime.MinValue)
                    {
                        dr["CheckDate"] = VisitPaymentDTO[i].Check_Date.ToString("dd-MMM-yyyy");
                    }
                    dr["PaymentNotes"] = VisitPaymentDTO[i].Payment_Note;
                    dr["VisitID"] = VisitPaymentDTO[i].Visit_Payment_Id;
                    dr["PPHeaderID"] = VisitPaymentDTO[i].PP_Header_Id;
                    dr["PPLineID"] = VisitPaymentDTO[i].PP_Line_Item_Id;
                    dr["CheckID"] = VisitPaymentDTO[i].Check_Table_Int_Id;
                    dr["relationship"] = VisitPaymentDTO[i].Relationship;
                    dr["Amtpaidby"] = VisitPaymentDTO[i].Amount_Paid_By;
                    if (VisitPaymentDTO[i].Created_Date_and_Time != DateTime.MinValue)
                    {
                        dr["receiptdate"] = VisitPaymentDTO[i].Created_Date_and_Time.ToString("dd-MMM-yyyy");
                    }
                    dr["PaymentNote"] = VisitPaymentDTO[i].Payment_Note;
                    if (VisitPaymentDTO[i].Modified_Date_and_Time != DateTime.MinValue)
                    {
                        dr["TransactionDate&Time"] = UtilityManager.ConvertToLocal(VisitPaymentDTO[i].Modified_Date_and_Time).ToString("dd-MMM-yyyy hh:mm tt");

                    }
                    else
                    {
                        dr["TransactionDate&Time"] = UtilityManager.ConvertToLocal(VisitPaymentDTO[i].Created_Date_and_Time).ToString("dd-MMM-yyyy hh:mm tt");

                    }
                    dr["VoucherNo"] = VisitPaymentDTO[i].Voucher_No;

                    dtInsuranace.Rows.Add(dr);
                }
                grdPaymentInformation.DataSource = dtInsuranace;
                grdPaymentInformation.DataBind();
                txtTotalAmount.Text = Convert.ToString((PaymentAmount + RecOnAcc) - RefundAmount);

                hdnTotalPayment.Value = Convert.ToString((PaymentAmount + RecOnAcc) - RefundAmount);
                if (VisitPaymentDTO[0].Voucher_No != 0)
                    hdnVoucherNo.Value = VisitPaymentDTO[0].Voucher_No.ToString();
            }
            else
            {
                //DataRow dr = dtInsuranace.NewRow();
                //dtInsuranace.Rows.Add(dr);
                //grdPaymentInformation.DataSource = dtInsuranace;
                //grdPaymentInformation.DataBind();
                //grdPaymentInformation.Rows[0].Visible = false;
                //txtTotalAmount.Text = "0.00";
                //hdnTotalPayment.Value = "0";
                hdnTotalPayment.Value = "0.00";
                ClearGrid();
            }
            if (ClientSession.UserRole.ToUpper() != "OFFICE MANAGER")
            {
                for (int iNumber = 0; iNumber < grdPaymentInformation.Rows.Count; iNumber++)
                {
                    ImageButton lnkedit = (ImageButton)grdPaymentInformation.Rows[iNumber].FindControl("EditGridRow");
                    lnkedit.Visible = false;
                    lnkedit.ImageUrl = "~/Resources/edit disabled.png";
                    ImageButton lnkDel = (ImageButton)grdPaymentInformation.Rows[iNumber].FindControl("DeleteGridRow");
                    lnkDel.Visible = false;
                    lnkDel.ImageUrl = "~/Resources/close_disabled.png";
                }
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "TotalPaymenent", "setTotalPayment();", true);

        }



        protected void grdPaymentInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //if (e.row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.RowIndex == 0)
            //        e.Row.Style.Add("height", "50px");
            //}
            if (e.CommandName == "EditC")
            {
                // hdnBatchStatus.Value = lblStatus.Text;
                PaymentInformationClearAll();
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int Rowindex = row.RowIndex;
                grdPaymentInformation.SelectedIndex = Rowindex;
                if (cboMethodOfPayment.Items.Count > 0)
                    cboMethodOfPayment.SelectedIndex = cboMethodOfPayment.Items.IndexOf(cboMethodOfPayment.Items.FindByText(grdPaymentInformation.Rows[Rowindex].Cells[2].Text));
                cboMethodOfPayment_SelectedIndexChanged(sender, e);
                if (grdPaymentInformation.Rows[Rowindex].Cells[3].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[3].Text != "&nbsp;")
                    txtCheckNo.Text = grdPaymentInformation.Rows[Rowindex].Cells[3].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[4].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[4].Text != "&nbsp;")
                    txtAuthNo.Text = grdPaymentInformation.Rows[Rowindex].Cells[4].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[5].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[5].Text != "&nbsp;")
                    txtPastDue.Text = grdPaymentInformation.Rows[Rowindex].Cells[5].Text;
                txtPaymentAmount.Text = grdPaymentInformation.Rows[Rowindex].Cells[6].Text;
                txtRecOnAcc.Text = grdPaymentInformation.Rows[Rowindex].Cells[7].Text;
                txtRefundAmount.Text = grdPaymentInformation.Rows[Rowindex].Cells[8].Text;

                if (grdPaymentInformation.Rows[Rowindex].Cells[9].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[9].Text != "&nbsp;")
                    dtpCheckDate.Text = grdPaymentInformation.Rows[Rowindex].Cells[9].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[10].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[10].Text != "&nbsp;")
                    txtPaymentNote.Text = grdPaymentInformation.Rows[Rowindex].Cells[10].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[11].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[11].Text != "&nbsp;")
                    hdnVisitID.Value = grdPaymentInformation.Rows[Rowindex].Cells[11].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[12].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[12].Text != "&nbsp;")
                    hdnPPHeaderID.Value = grdPaymentInformation.Rows[Rowindex].Cells[12].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[13].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[13].Text != "&nbsp;")
                    hdnPPLineItemID.Value = grdPaymentInformation.Rows[Rowindex].Cells[13].Text;
                if (grdPaymentInformation.Rows[Rowindex].Cells[14].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[14].Text != "&nbsp;")
                    hdnCheckID.Value = grdPaymentInformation.Rows[Rowindex].Cells[14].Text;
                //if (grdPaymentInformation.Rows[Rowindex].Cells[18].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[18].Text != "&nbsp;")
                //    hdnVoucherNo.Value = grdPaymentInformation.Rows[Rowindex].Cells[18].Text;



                if (grdPaymentInformation.Rows[Rowindex].Cells[15].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[15].Text != "&nbsp;")
                {
                    if (grdPaymentInformation.Rows[Rowindex].Cells[15].Text == "Patient")
                    {
                        cboRelation.SelectedIndex = 0;
                    }
                    if (grdPaymentInformation.Rows[Rowindex].Cells[15].Text == "Others")
                    {
                        cboRelation.SelectedIndex = 1;
                    }
                }
                if (grdPaymentInformation.Rows[Rowindex].Cells[16].Text != "" && grdPaymentInformation.Rows[Rowindex].Cells[16].Text != "&nbsp;")
                    txtpaidBy.Text = grdPaymentInformation.Rows[Rowindex].Cells[16].Text;

                btnAdd.Text = "Update";
                btnClear.Text = "Cancel";

                spanPaymentNotes.Attributes.Remove("class"); /*added*/
                spanPaymentNotes.Attributes.Add("class", "spanstyle");
                spanPatientNotestar.Visible = false;
                cboMethodOfPayment.CssClass = "Editabletxtbox";
                cboMethodOfPayment.Enabled = true;
            }
            else if (e.CommandName == "DeleteRow")
            {
                //Fill grid values corresponding textbox

                PaymentInformationClearAll();
                GridViewRow rows = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int Rowindexs = rows.RowIndex;
                grdPaymentInformation.SelectedIndex = Rowindexs;
                if (cboMethodOfPayment.Items.Count > 0)
                    cboMethodOfPayment.SelectedIndex = cboMethodOfPayment.Items.IndexOf(cboMethodOfPayment.Items.FindByText(grdPaymentInformation.Rows[Rowindexs].Cells[2].Text));
                cboMethodOfPayment_SelectedIndexChanged(sender, e);
                if (grdPaymentInformation.Rows[Rowindexs].Cells[3].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[3].Text != "&nbsp;")
                    txtCheckNo.Text = grdPaymentInformation.Rows[Rowindexs].Cells[3].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[4].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[4].Text != "&nbsp;")
                    txtAuthNo.Text = grdPaymentInformation.Rows[Rowindexs].Cells[4].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[5].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[5].Text != "&nbsp;")
                    txtPastDue.Text = grdPaymentInformation.Rows[Rowindexs].Cells[5].Text;
                txtPaymentAmount.Text = grdPaymentInformation.Rows[Rowindexs].Cells[6].Text;
                txtRecOnAcc.Text = grdPaymentInformation.Rows[Rowindexs].Cells[7].Text;
                txtRefundAmount.Text = grdPaymentInformation.Rows[Rowindexs].Cells[8].Text;

                if (grdPaymentInformation.Rows[Rowindexs].Cells[9].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[9].Text != "&nbsp;")
                    dtpCheckDate.Text = grdPaymentInformation.Rows[Rowindexs].Cells[9].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[10].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[10].Text != "&nbsp;")
                    txtPaymentNote.Text = grdPaymentInformation.Rows[Rowindexs].Cells[10].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[11].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[11].Text != "&nbsp;")
                    hdnVisitID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[11].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[12].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[12].Text != "&nbsp;")
                    hdnPPHeaderID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[12].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[13].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[13].Text != "&nbsp;")
                    hdnPPLineItemID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[13].Text;
                if (grdPaymentInformation.Rows[Rowindexs].Cells[14].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[14].Text != "&nbsp;")
                    hdnCheckID.Value = grdPaymentInformation.Rows[Rowindexs].Cells[14].Text;

                if (grdPaymentInformation.Rows[Rowindexs].Cells[15].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[15].Text != "&nbsp;")
                {
                    if (grdPaymentInformation.Rows[Rowindexs].Cells[15].Text == "Patient")
                    {
                        cboRelation.SelectedIndex = 0;
                    }
                    if (grdPaymentInformation.Rows[Rowindexs].Cells[15].Text == "Others")
                    {
                        cboRelation.SelectedIndex = 1;
                    }
                }
                if (grdPaymentInformation.Rows[Rowindexs].Cells[16].Text != "" && grdPaymentInformation.Rows[Rowindexs].Cells[16].Text != "&nbsp;")
                    txtpaidBy.Text = grdPaymentInformation.Rows[Rowindexs].Cells[16].Text;

                btnAdd.Text = "Confirm Delete";
                paymentinformationdisableall();
                ComboBoxColorChange(cboRelation, false);
                //ComboBoxColorChange(cboMethodOfPayment, true);
                cboMethodOfPayment.CssClass = "nonEditabletxtbox";
                cboMethodOfPayment.Enabled = false;
                TextBoxColorChange(txtPaymentNote, true);
                spanPaymentNotes.Attributes.Remove("class"); /*added*/
                spanPaymentNotes.Attributes.Add("class", "MandLabelstyle");
                spanPatientNotestar.Visible = true;
                spanPaymentNotes.Style["margin-left"] = "17px";
                txtPaymentNote.Enabled = true;

            }
        }

        public void delete_SelectedRow()
        {
            int Rowindex = grdPaymentInformation.SelectedRow.RowIndex;
            ulong Check = 0;
            ulong PPHeaderId = 0;
            ulong PPLineId = 0;
            ulong VisitID = 0;
            VisitID = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[11].Text);
            PPHeaderId = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[12].Text);
            PPLineId = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[13].Text);
            Check = Convert.ToUInt64(grdPaymentInformation.Rows[Rowindex].Cells[14].Text);

            if (ddlFacilitywithDOS.SelectedValue == string.Empty || ddlFacilitywithDOS.SelectedItem.Value.Split('|')[2].ToString().ToUpper() == "MAIN")
            {
                SaveVisitPaymentList = new List<VisitPayment>();

                HumanManager HumanMngr = new HumanManager();
                FillQuickPatient objcheckout = HumanMngr.GetVisitPaymentDetails(grdPaymentInformation.Rows[Rowindex].Cells[11].Text, grdPaymentInformation.Rows[Rowindex].Cells[12].Text, grdPaymentInformation.Rows[Rowindex].Cells[13].Text, grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
                if (objcheckout.VisitPaymentList.Count > 0 && objcheckout.VisitPaymentList[0] != null)
                {
                    objcheckout.VisitPaymentList[0].Is_Delete = "Y";
                    objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
                    objcheckout.VisitPaymentList[0].Payment_Note = txtPaymentNote.Text;
                    SaveVisitPaymentList.Add(objcheckout.VisitPaymentList[0]);
                }
                PPHeaderManager PPHeaderMngr = new PPHeaderManager();
                SavePPHeaderList = new List<PPHeader>();
                if (objcheckout.PPHeaderList.Count > 0 && objcheckout.PPHeaderList[0] != null)
                {
                    objcheckout.PPHeaderList[0].Is_Delete = "Y";
                    objcheckout.PPHeaderList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
                    SavePPHeaderList.Add(objcheckout.PPHeaderList[0]);
                }
                PPLineItemManager PPLineMngr = new PPLineItemManager();
                SavePPLineItemList = new List<PPLineItem>();
                if (objcheckout.PPLineItemList.Count > 0 && objcheckout.PPLineItemList[0] != null)
                {
                    objcheckout.PPLineItemList[0].Is_Delete = "Y";
                    objcheckout.PPLineItemList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;
                    SavePPLineItemList.Add(objcheckout.PPLineItemList[0]);
                }
                CheckManager CheckMngr = new CheckManager();
                SaveCheckList = new List<Check>();
                if (objcheckout.CheckList.Count > 0 && objcheckout.CheckList[0] != null)
                {
                    objcheckout.CheckList[0].Is_Delete = "Y";
                    objcheckout.CheckList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
                    SaveCheckList.Add(objcheckout.CheckList[0]);
                }


                AccountTransactionManager AccTranMngr = new AccountTransactionManager();
                IList<AccountTransaction> ilistAccTran = new List<AccountTransaction>();
                if (objcheckout != null && objcheckout.AccountTransaction.Count > 0)
                {
                    for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
                    {
                        objcheckout.AccountTransaction[iNumber].Is_Delete = "Y";
                        objcheckout.AccountTransaction[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
                    }
                }

                VisitPaymentHistoryManager VisitPaymentHistoryMngr = new VisitPaymentHistoryManager();
                SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
                if (objcheckout != null && objcheckout.VisitPaymentHistoryList.Count > 0)
                {
                    objcheckout.VisitPaymentHistoryList[0].Payment_Note = txtPaymentNote.Text;
                    objcheckout.VisitPaymentHistoryList[0].Is_Delete = "Y";
                    objcheckout.VisitPaymentHistoryList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objcheckout.VisitPaymentHistoryList[0].Modified_By = ClientSession.UserName;
                    SaveVisitPaymenHistoryList.Add(objcheckout.VisitPaymentHistoryList[0]);

                    FillVisitPaymentHistory(objcheckout.VisitPaymentList[0], null, "DEBIT");
                }



                IList<VisitPaymentDTO> VisitPaymentDTO = new List<VisitPaymentDTO>();
                if (hdnEncounterID.Value != "")
                    VisitPaymentDTO = visitMgr.UpdateVisitPayment(SaveVisitPaymentList, SaveCheckList, SavePPHeaderList, SavePPLineItemList, ilistAccTran, SaveAccountTransactionList, SaveVisitPaymenHistoryList, Convert.ToUInt32(hdnEncounterID.Value));

                bValidPaymentInfo = true;
                SaveVisitPaymentList.Clear();
                LoadPaymentInfoGrid(VisitPaymentDTO);
                paymentinformationdisableall();
                PaymentInformationClearAll();
                cboMethodOfPayment.SelectedIndex = 0;
                btnAdd.Text = "Add";
                btnClear.Text = "Clear All";
                grdPaymentInformation.DataBind();
                if (grdPaymentInformation.Rows.Count == 1 && grdPaymentInformation.Rows[0].Visible == false)
                {
                    txtTotalAmount.Text = "0.00";
                }
            }
            //else
            //{
            //    SaveVisitPaymentArcList = new List<VisitPaymentArc>();

            //    HumanManager HumanMngr = new HumanManager();
            //    FillQuickPatientArc objcheckout = HumanMngr.GetVisitPaymentDetailsArc(grdPaymentInformation.Rows[Rowindex].Cells[11].Text, grdPaymentInformation.Rows[Rowindex].Cells[12].Text, grdPaymentInformation.Rows[Rowindex].Cells[13].Text, grdPaymentInformation.Rows[Rowindex].Cells[14].Text);
            //    if (objcheckout.VisitPaymentList.Count > 0 && objcheckout.VisitPaymentList[0] != null)
            //    {
            //        objcheckout.VisitPaymentList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
            //        SaveVisitPaymentArcList.Add(objcheckout.VisitPaymentList[0]);
            //    }
            //    SavePPHeaderArcList = new List<PPHeaderArc>();
            //    if (objcheckout.PPHeaderList.Count > 0 && objcheckout.PPHeaderList[0] != null)
            //    {
            //        objcheckout.PPHeaderList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
            //        SavePPHeaderArcList.Add(objcheckout.PPHeaderList[0]);
            //    }
            //    SavePPLineItemArcList = new List<PPLineItemArc>();
            //    if (objcheckout.PPLineItemList.Count > 0 && objcheckout.PPLineItemList[0] != null)
            //    {
            //        objcheckout.PPLineItemList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;
            //        SavePPLineItemArcList.Add(objcheckout.PPLineItemList[0]);
            //    }
            //    SaveCheckArcList = new List<CheckArc>();
            //    if (objcheckout.CheckList.Count > 0 && objcheckout.CheckList[0] != null)
            //    {
            //        objcheckout.CheckList[0].Is_Delete = "Y";
            //        objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
            //        SaveCheckArcList.Add(objcheckout.CheckList[0]);
            //    }
            //    IList<AccountTransactionArc> ilistAccTran = new List<AccountTransactionArc>();
            //    if (objcheckout != null && objcheckout.AccountTransaction.Count > 0)
            //    {
            //        for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
            //        {
            //            objcheckout.AccountTransaction[iNumber].Is_Delete = "Y";
            //            objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //            objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
            //        }
            //    }
            //    IList<VisitPaymentDTO> VisitPaymentDTO = new List<VisitPaymentDTO>();
            //    if (hdnEncounterID.Value != "")
            //        //VisitPaymentDTO = visitArcMgr.UpdateVisitPaymentArc(SaveVisitPaymentArcList, SaveCheckArcList, SavePPHeaderArcList, SavePPLineItemArcList, objcheckout.AccountTransaction, null, null, Convert.ToUInt32(hdnEncounterID.Value));
            //        VisitPaymentDTO = visitArcMgr.UpdateVisitPaymentArc(SaveVisitPaymentArcList, SaveCheckArcList, SavePPHeaderArcList, SavePPLineItemArcList, ilistAccTran, SaveAccountTransactionArcList, SaveVisitPaymenHistoryArcList, Convert.ToUInt32(hdnEncounterID.Value));

            //   // ddlVoucher.Items.Remove("");

            //        bValidPaymentInfo = true;
            //    SaveVisitPaymentList.Clear();
            //    LoadPaymentInfoGrid(VisitPaymentDTO);
            //    paymentinformationdisableall();
            //    PaymentInformationClearAll();
            //    cboMethodOfPayment.SelectedIndex = 0;
            //    ddlVoucher.ClearSelection();
            //    btnAdd.Text = "Add";
            //    btnClear.Text = "Clear All";
            //    grdPaymentInformation.DataBind();
            //    if (grdPaymentInformation.Rows.Count == 1 && grdPaymentInformation.Rows[0].Visible == false)
            //    {
            //        txtTotalAmount.Text = "0.00";
            //    }
            //}

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('180037');", true);
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            //rdbVoucher_CheckedChanged(sender, e);
            VisitPaymentManager objVoucherNo = new VisitPaymentManager();
            ArrayList listVoucherNo = objVoucherNo.GetAutoIncreamentVoucherNo(humanID, ClientSession.FacilityName);
            ddlVoucher.Items.Clear();
            ddlVoucher.Items.Add("");
            if (listVoucherNo != null && listVoucherNo.Count > 0)
            {

                for (int iCount = 0; iCount < listVoucherNo.Count; iCount++)
                {
                    System.Web.UI.WebControls.ListItem ddlItem = new System.Web.UI.WebControls.ListItem();
                    ddlItem.Value = listVoucherNo[iCount].ToString();
                    ddlVoucher.Items.Add(ddlItem);
                }
            }
            else
            {
                PaymentInformationClearAll();
                cboMethodOfPayment.SelectedIndex = 0;
                cboMethodOfPayment.CssClass = "nonEditabletxtbox";
                cboMethodOfPayment.Enabled = false;
                paymentinformationdisableall();
                btnAdd.Enabled = false;
            }
        }

        public void PaymentInformationClearAll()
        {
            cboRelation.SelectedIndex = 0;
            if (cboRelation.SelectedItem.Text == "Others")
            {
                txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0];
                TextBoxColorChange(txtpaidBy, true);
            }
            else if (cboRelation.SelectedItem.Text == "Patient")
            {
                txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                TextBoxColorChange(txtpaidBy, false);
            }
            txtAuthNo.Text = string.Empty;
            txtPaymentAmount.Text = "0.00";
            txtCheckNo.Text = string.Empty;
            txtRefundAmount.Text = "0.00";
            txtRecOnAcc.Text = "0.00";
            txtPaymentNote.Text = string.Empty;
            txtPastDue.Text = "0.00";
            chkMultiplePayments.Checked = false;
            paymentinformationdisableall();
        }

        public void paymentinformationdisableall()
        {
            TextBoxColorChange(txtCheckNo, false);
            TextBoxColorChange(txtAuthNo, false);
            TextBoxColorChange(txtPaymentAmount, false);
            DateTimePickerColorChangeForWindows(dtpCheckDate, false);
            TextBoxColorChange(txtPaymentNote, false);
            TextBoxColorChange(txtRefundAmount, false);
            TextBoxColorChange(txtRecOnAcc, false);
            ComboBoxColorChange(cboRelation, false);
        }

        public void DateTimePickerColorChangeForWindows(RadMaskedTextBox datetimepicker, Boolean bToNormal)
        {
            if (datetimepicker.ID != "dtpPatientDOB" && datetimepicker.ID != "dtpEffectiveStartDate")
            {
                if (bToNormal == false)
                {
                    datetimepicker.Enabled = false;
                    datetimepicker.Text = string.Empty;
                }
                else
                {
                    datetimepicker.Enabled = true;
                }
            }
            else
            {
                if (bToNormal == false)
                {
                    datetimepicker.Enabled = false;
                }
                else
                {
                    datetimepicker.Enabled = true;
                }
            }
        }

        public void TextBoxColorChange(TextBox txtbox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                txtbox.ReadOnly = true;
                txtbox.CssClass = "nonEditabletxtbox";

            }
            else
            {
                txtbox.ReadOnly = false;
                txtbox.CssClass = "Editabletxtbox";

            }
        }

        protected void cboMethodOfPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            // cboMethodOfPayment.Attributes.Add("onchange", "Loading();");
            if (chkMultiplePayments.Checked == true)
            {
                btnAdd.Enabled = true;
                btnClear.Enabled = true;
            }
            /*Disable the Payment information groupbox if 'Method of Payment' is empty*/
            if (cboMethodOfPayment.SelectedItem.Text == string.Empty)
            {
                paymentinformationdisableall();
                //PaymentInformationClearAll();
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, false);
                TextBoxColorChange(txtRefundAmount, false);

                TextBoxColorChange(txtpaidBy, false);
                ComboBoxColorChange(cboRelation, false);
                cboRelation.SelectedIndex = 0;
                btnAdd.Enabled = false;
                spanCheck.Attributes.Remove("class"); /*added*/
                spanCheck.Attributes.Add("class", "spanstyle");
                spanCheckStar.Visible = false;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                    TextBoxColorChange(txtpaidBy, false);
                }

            }
            /*Enable the PaymentAmount Textbox for "Cash"*/
            else if (cboMethodOfPayment.SelectedItem.Text.ToUpper() == "CASH")
            {
                paymentinformationdisableall();
                paymentamountenable();
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, true);
                TextBoxColorChange(txtRefundAmount, true);
                txtCheckNo.Text = string.Empty;
                btnAdd.Enabled = true;
                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                    TextBoxColorChange(txtpaidBy, false);
                }

                dtpCheckDate.Text = string.Empty;

                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;

                spanCheck.Attributes.Remove("class"); /*added*/
                spanCheck.Attributes.Add("class", "spanstyle");
                spanCheckStar.Visible = true;
                spanCheckStar.Visible = false;


            }
            /*Enable the PaymentAmount and Check no Textbox for "Cheque"*/
            else if (cboMethodOfPayment.SelectedItem.Text.ToUpper() == "CHECK")
            {
                btnAdd.Enabled = true;
                paymentamountenable();
                TextBoxColorChange(txtCheckNo, true);
                TextBoxColorChange(txtAuthNo, false);
                dtpCheckDate.Enabled = true;
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                DateTimePickerColorChangeForWindows(dtpCheckDate, true);
                TextBoxColorChange(txtRecOnAcc, true);
                TextBoxColorChange(txtRefundAmount, true);
                MaskedTextBoxColorChange(dtpCheckDate, true);
                dtpCheckDate.Enabled = true;
                txtCheckNo.Text = string.Empty;

                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                    TextBoxColorChange(txtpaidBy, false);
                }

                spanCheck.Attributes.Remove("class");

                spanCheck.Attributes.Add("class", "MandLabelstyle");
                spanCheckStar.Visible = true;


                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;


            }
            else if (cboMethodOfPayment.SelectedItem.Text.ToUpper() == "CREDIT CARD" || cboMethodOfPayment.SelectedItem.Text.ToUpper() == "DEBIT CARD")
            {
                btnAdd.Enabled = true;
                paymentinformationdisableall();
                paymentamountenable();
                //ChangeLabelStyle(lblCheckNo, false);
                TextBoxColorChange(txtCheckNo, true);
                TextBoxColorChange(txtAuthNo, true);
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, true);
                TextBoxColorChange(txtRefundAmount, true);
                txtCheckNo.Text = string.Empty;

                TextBoxColorChange(txtpaidBy, true);
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                    TextBoxColorChange(txtpaidBy, false);
                }

                spanCheck.Attributes.Remove("class");

                spanCheck.Attributes.Add("class", "MandLabelstyle");
                spanCheckStar.Visible = true;


                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;

            }
            /*Enable all the controls in Payment information Group box.*/
            else
            {
                btnAdd.Enabled = true;
                paymentamountenable();
                TextBoxColorChange(txtCheckNo, true);
                TextBoxColorChange(txtAuthNo, false);
                DateTimePickerColorChangeForWindows(dtpCheckDate, true);
                txtRecOnAcc.Text = "0.00";
                txtPaymentAmount.Text = "0.00";
                txtRefundAmount.Text = "0.00";
                TextBoxColorChange(txtRecOnAcc, false);
                TextBoxColorChange(txtRefundAmount, false);
                txtCheckNo.Text = string.Empty;
                ComboBoxColorChange(cboRelation, true);
                cboRelation.SelectedIndex = 0;
                if (cboRelation.SelectedItem.Text == "Patient")
                {
                    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                    TextBoxColorChange(txtpaidBy, false);
                }
                spanCheck.Attributes.Remove("class");

                spanCheck.Attributes.Add("class", "MandLabelstyle");
                spanCheckStar.Visible = true;



                spanRelation.Attributes.Remove("class"); /*added*/

                spanRelation.Attributes.Add("class", "MandLabelstyle");
                spanRelationstar.Visible = true;

                spanPaidBy.Attributes.Remove("class");

                spanPaidBy.Attributes.Add("class", "MandLabelstyle");
                spanPaidStar.Visible = true;

            }
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "  {warningmethod();}", true);
        }

        public void paymentamountenable()
        {
            TextBoxColorChange(txtPaymentAmount, true);
            TextBoxColorChange(txtPaymentNote, true);
        }

        public void ComboBoxColorChange(DropDownList combobox, Boolean bToNormal)
        {
            if (bToNormal == false)
            {
                combobox.Enabled = false;
                combobox.CssClass = "nonEditabletxtbox";

            }
            else
            {
                combobox.Enabled = true;
                combobox.CssClass = "Editabletxtbox";

            }
        }

        public void MaskedTextBoxColorChange(RadMaskedTextBox msktxtbox, bool bToNormal)
        {
            if (bToNormal == false)
            {
                msktxtbox.ReadOnly = true;
                msktxtbox.CssClass = "";
                msktxtbox.CssClass = "nonEditabletxtbox";
            }
            else
            {
                msktxtbox.ReadOnly = false;
                msktxtbox.CssClass = "";
                msktxtbox.CssClass = "Editabletxtbox";
            }
        }

        protected void ddlFacilityWithDOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnAdd.Enabled = true;
            hdnEncounterID.Value = "0";
            hdnVoucherNo.Value = "0";

            if (ddlFacilitywithDOS.SelectedValue != "")
            {
                cboMethodOfPayment.Enabled = true;
                cboMethodOfPayment.CssClass = "Editabletxtbox";
                hdnEncounterID.Value = ddlFacilitywithDOS.SelectedValue.Split('|')[0];
                string status = "";
                if (ddlFacilitywithDOS.SelectedValue.Split('|').Length > 1)
                    status = ddlFacilitywithDOS.SelectedValue.Split('|')[1];
                string encid = ddlFacilitywithDOS.SelectedValue.Split('|')[0];

                //ddlVoucher.SelectedItem.Text = encid;
                Human humanLoadRecord = null;
                if (hdnEncounterID.Value != string.Empty && hdnEncounterID.Value != "0")
                {
                    objCheckOutLoad = HumanMngr.LoadQuickPatientforPatientPayment(Convert.ToUInt64(hdnEncounterID.Value), true);
                }
                if (objCheckOutLoad != null && objCheckOutLoad.HumanObj != null)
                {
                    humanLoadRecord = objCheckOutLoad.HumanObj;
                }
                if (objCheckOutLoad == null || objCheckOutLoad.EncounterObj == null)
                {
                    return;
                }
                if (objCheckOutLoad.HumanObj != null)
                {
                    if (objCheckOutLoad.HumanObj.Declared_Bankruptcy == "Y")
                    {
                        lblPaymentInCollection.ForeColor = Color.Red;
                        lblDeclaredBankruptcy.ForeColor = Color.Blue;
                        lblDeclaredBankruptcy.Text += " " + objCheckOutLoad.HumanObj.Declared_Bankruptcy.ToUpper() + "";
                    }
                    if (objCheckOutLoad.HumanObj.Declared_Bankruptcy == "N")
                    {
                        lblDeclaredBankruptcy.Text += " " + objCheckOutLoad.HumanObj.Declared_Bankruptcy.ToUpper() + "";
                    }
                    lblPaymentInCollection.Text = "Patient In Collection : " + objCheckOutLoad.HumanObj.People_In_Collection.ToUpper() + " ";
                    txtPastDue.Text = Convert.ToString(objCheckOutLoad.HumanObj.Past_Due);
                    //if (objCheckOutLoad.EncounterObj.Batch_Status.ToUpper() == "CLOSED")
                    //{
                    //    lblStatus.Text = "Batch Closed!";
                    //}
                    //else
                    //{
                    //    lblStatus.Text = string.Empty;
                    //}
                    //hdnBatchStatus.Value = lblStatus.Text;
                    hdnPastDue.Value = objCheckOutLoad.HumanObj.Past_Due.ToString();
                }


                LoadPaymentInfoGrid(objCheckOutLoad.VisitPaymentDTO);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {

                ClearGrid();
                cboMethodOfPayment.Enabled = false;
                cboMethodOfPayment.CssClass = "nonEditabletxtbox";
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        void ClearGrid()
        {
            grdPaymentInformation.DataSource = null;
            grdPaymentInformation.DataBind();
            //if (ddlVoucher.SelectedItem != null)
            //    ddlVoucher.SelectedItem.Text = string.Empty;
            PaymentInformationClearAll();
            cboMethodOfPayment.SelectedIndex = 0;
            //txtTotalAmount.Text = string.Empty;
            hdnTotalPayment.Value = "0.00";
            spanPaymentNotes.Attributes.Remove("class"); /*added*/
            spanPaymentNotes.Attributes.Add("class", "spanstyle");
            spanPatientNotestar.Visible = false;
            btnAdd.Enabled = false;
            btnAdd.Text = "Add";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "TotalPaymenent", "setTotalPayment();", true);
        }

        protected void ddlVoucher_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnEncounterID.Value = "0";
            hdnVoucherNo.Value = "0";
            if (ddlVoucher.SelectedItem.Text == "")
            {
                cboMethodOfPayment.Enabled = false;
                cboMethodOfPayment.CssClass = "nonEditabletxtbox";
                ClearGrid();

            }
            else
            {
                cboMethodOfPayment.Enabled = true;
                cboMethodOfPayment.CssClass = "Editabletxtbox";

            }


            string voucherno = string.Empty;
            voucherno = ddlVoucher.SelectedValue;
            Human humanLoadRecord = null;
            if (voucherno != string.Empty)
            {
                if (voucherno.Contains("VN") == false)
                {
                    objCheckOutLoad = HumanMngr.LoadQuickPatientforPatientPayment(Convert.ToUInt64(voucherno.Replace("EN", "")), true);
                    hdnEncounterID.Value = voucherno.Replace("EN", "");
                }
                else
                {
                    objCheckOutLoad = HumanMngr.LoadQuickPatientforPatientPayment(Convert.ToUInt64(voucherno.Replace("VN", "")), false);
                    hdnVoucherNo.Value = voucherno.Replace("VN", "");

                }
            }
            if (objCheckOutLoad != null && objCheckOutLoad.HumanObj != null)
            {
                humanLoadRecord = objCheckOutLoad.HumanObj;
            }
            if (objCheckOutLoad == null || objCheckOutLoad.EncounterObj == null)
            {
                return;
            }

            LoadPaymentInfoGrid(objCheckOutLoad.VisitPaymentDTO);
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "StopLoading", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void cboRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRelation.SelectedItem.Text == "Others")
            {
                txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0];
                TextBoxColorChange(txtpaidBy, true);
            }
            else if (cboRelation.SelectedItem.Text == "Patient")
            {
                txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                TextBoxColorChange(txtpaidBy, false);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeLabel", "warningmethod();", true);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboMethodOfPayment.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380042');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (cboRelation.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380051');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (txtpaidBy.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380052');", true);
                btnAdd.Enabled = true;
                return;
            }
            if (gbPaymentInformation.Visible == true)
            {
                if (txtCheckNo.ReadOnly == false && txtCheckNo.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380021');", true);
                    chkMultiplePayments.Checked = false;
                    txtCheckNo.Focus();
                    return;
                }

            }

            if (btnAdd.Text == "Confirm Delete")
            {
                if (txtPaymentNote.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Delete-Alert", "alert('Please provide the reason for deletion in the payment note textbox'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                else
                {
                    //Jira Cap - 517
                    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('380060');", true);
                    delete_SelectedRow();
                    Button2_Click1(sender, e);
                    spanPaymentNotes.Attributes.Remove("class"); /*added*/
                    spanPaymentNotes.Attributes.Add("class", "spanstyle");
                    spanPatientNotestar.Visible = false;
                    btnAdd.Enabled = false;
                    cboMethodOfPayment.CssClass = "Editabletxtbox";
                    cboMethodOfPayment.Enabled = true;
                    return;
                }
            }

            if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
            {
                try
                {
                    DateTime dtTemp = Convert.ToDateTime(dtpCheckDate.Text);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('102012');", true);
                    btnAdd.Enabled = true;
                    return;
                }
            }

            //string encid = ddlFacilitywithDOS.SelectedValue.Split('|')[0];
            //EncounterManager EncounterMngr = new EncounterManager();
            //IList<Encounter> EncList = new List<Encounter>();
            //if (encid != null && encid != "")
            //    EncList = EncounterMngr.GetEncounterByEncounterID(Convert.ToUInt64(encid));

            //if (EncList != null && EncList.Count > 0)
            //{
            //    if (EncList[0].Batch_Status.ToUpper() == "CLOSED")
            //    {
            //        lblStatus.Text = "Batch Closed!";
            //    }

            //    else
            //    {
            //        lblStatus.Text = string.Empty;
            //    }
            //    hdnBatchStatus.Value = lblStatus.Text;

            //}

            //AutoIncreament:
            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);


            if (btnAdd.Text == "Add")
            {
                if (ddlFacilitywithDOS.SelectedValue == string.Empty || ddlFacilitywithDOS.SelectedItem.Value.Split('|')[2].ToString().ToUpper() == "MAIN")
                {
                    VisitPayment objPayment = new VisitPayment();
                    Check objCheck = new Check();
                    PPHeader objPPHeader = new PPHeader();
                    PPLineItem objPPLineItem = new PPLineItem();
                    AccountTransaction objAccountTransaction = new AccountTransaction();

                    objPayment.Facility_Name = ClientSession.FacilityName;

                    //string encounterid = ddlFacilitywithDOS.SelectedValue.Split('|')[0];
                    //string insertQuery = string.Empty;
                    //insertQuery = "insert into  visit_payment (Voucher_No) values('" + encounterid + "')";
                    //iReturn = DBConnector.WriteData(insertQuery);

                    if (txtPaymentAmount.ReadOnly == false)
                    {
                        objPayment.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        objCheck.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        objPPHeader.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                        objPPLineItem.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                        objAccountTransaction.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                    }

                    if (txtRefundAmount.Text != string.Empty)
                    {
                        objPayment.Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);
                    }
                    if (txtRecOnAcc.Text != string.Empty)
                        objPayment.Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);

                    if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                    {
                        objPayment.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                        objCheck.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                    }
                    else
                    {
                        objPayment.Check_Date = DateTime.MinValue;
                        objCheck.Check_Date = DateTime.MinValue;
                    }


                    objPayment.Relationship = cboRelation.SelectedItem.Text;
                    objPayment.Amount_Paid_By = txtpaidBy.Text;

                    objPayment.Check_Card_No = txtCheckNo.Text;
                    objPayment.Auth_No = txtAuthNo.Text;
                    objPayment.Method_of_Payment = cboMethodOfPayment.Text;
                    objPayment.Payment_Note = txtPaymentNote.Text;
                    objPayment.Human_ID = humanID;
                    objPayment.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);

                    if (grdPaymentInformation.Rows.Count > 0 && objPayment.Encounter_ID == 0)
                        objPayment.Voucher_No = Convert.ToUInt64(hdnVoucherNo.Value);

                    objPayment.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    objPayment.Created_By = ClientSession.UserName;
                    objPayment.Is_Delete = "N";

                    SaveVisitPaymentList.Add(objPayment);

                    FillVisitPaymentHistory(objPayment, null, string.Empty);

                    //  objCheck.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                    objCheck.Human_ID = humanID;
                    objCheck.Payment_Type = cboMethodOfPayment.Text;
                    objCheck.Created_By = ClientSession.UserName;

                    objCheck.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    objCheck.Carrier_Patient_Name = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name;
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objCheck.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        if (cboMethodOfPayment.Text == "Cash")
                        {
                            objCheck.Payment_ID = hdnEncounterID.Value;
                        }
                    }
                    if (txtCheckNo.Text != string.Empty)
                        objCheck.Payment_ID = txtCheckNo.Text;
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objCheck.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objCheck.Is_Delete = "N";
                    SaveCheckList.Add(objCheck);

                    // objPPHeader.Human_ID = Convert.ToUInt64(ClientSession.HumanId);

                    objPPHeader.Human_ID = humanID;
                    objPPHeader.Created_By = ClientSession.UserName;

                    objPPHeader.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    if (hdnEncounterID.Value != string.Empty)
                    {
                        objPPHeader.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    }
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objPPHeader.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value); ;
                    }
                    objPPHeader.Is_Delete = "N";
                    SavePPHeaderList.Add(objPPHeader);
                    objPPLineItem.Claim_Type = "PATIENT";
                    objPPLineItem.Line_Type = "UNAPPLIED";
                    objPPLineItem.Created_By = ClientSession.UserName;

                    objPPLineItem.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    //  objPPLineItem.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                    objPPLineItem.Human_ID = humanID;
                    objPPLineItem.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objPPLineItem.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objPPLineItem.Is_Delete = "N";
                    //objPPLineItem.Comments = "ON " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + " by " + ClientSession.UserName + ":" + "\n" + "Method of Payment:" + cboMethodOfPayment.Text + "\n" + "Check#:" + txtCheckNo.Text + "\n" + "CC Auth#:" + txtAuthNo.Text + "\n" + "Copay:" + txtPaymentAmount.Text + "\n" + "Recd on Acct:" + txtRecOnAcc.Text + "\n" + "Past Due:" + txtPastDue.Text + "\n" + "Refund Amt:" + txtRefundAmount.Text + "\n" + "Check Date:" + dtpCheckDate.Text + "\n" + "Payment Note:" + txtPaymentNote.Text + "\n" + "Relationship:" + cboRelation.Text + "\n" + "Paid By:" + txtpaidBy.Text;


                    SavePPLineItemList.Add(objPPLineItem);

                    // if (ddlFacilitywithDOS.SelectedValue == string.Empty)
                    if (rdbNewCollection.Checked == true || rdbVoucher.Checked == true)
                        objAccountTransaction.Deposit_Date = UtilityManager.ConvertToUniversal(DateTime.Now);
                    else
                        objAccountTransaction.Deposit_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(ddlFacilitywithDOS.SelectedItem.Text.Split(new char[] { '-' }, 2)[1]));
                    // objAccountTransaction.Human_ID = Convert.ToUInt64(ClientSession.HumanId);

                    objAccountTransaction.Human_ID = humanID;
                    objAccountTransaction.Claim_Type = "PATIENT";
                    objAccountTransaction.Line_Type = "UNAPPLIED";
                    objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                    objAccountTransaction.Created_By = ClientSession.UserName;

                    objAccountTransaction.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                    if (hdnCarrierId.Value != string.Empty)
                    {
                        objAccountTransaction.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                    }
                    objAccountTransaction.Is_Delete = "N";
                    SaveAccountTransactionList.Add(objAccountTransaction);

                    objAccountTransaction = new AccountTransaction();
                    if (txtRefundAmount.Text != string.Empty && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                    {
                        objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                        objAccountTransaction.Reversal_Refund_Category = "REFUND";
                        if (rdbNewCollection.Checked == true || rdbVoucher.Checked == true)
                            objAccountTransaction.Deposit_Date = UtilityManager.ConvertToUniversal(DateTime.Now);
                        else
                            objAccountTransaction.Deposit_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(ddlFacilitywithDOS.SelectedItem.Text.Split(new char[] { '-' }, 2)[1]));

                        //objAccountTransaction.Deposit_Date = UtilityManager.ConvertToLocal(Convert.ToDateTime(ddlFacilitywithDOS.SelectedItem.Text.Split(new char[] { '-' }, 2)[1])); // UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                        //  objAccountTransaction.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                        objAccountTransaction.Human_ID = humanID;
                        objAccountTransaction.Claim_Type = "PATIENT";
                        objAccountTransaction.Line_Type = "UNAPPLIED";
                        objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                        objAccountTransaction.Created_By = ClientSession.UserName;

                        objAccountTransaction.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                        objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                        objAccountTransaction.Is_Delete = "N";
                        //objAccountTransaction.Carrier_ID = Cash_Carrier_ID;
                        SaveAccountTransactionList.Add(objAccountTransaction);
                    }
                    // objvoucherno.Voucher_No = ddlFacilitywithDOS.SelectedValue.Split('|')[0];

                    IList<VisitPaymentDTO> VisitList;
                    if (hdnEncounterID.Value != string.Empty)
                    {
                        VisitList = visitMgr.SaveVisitPayment(Convert.ToUInt64(hdnEncounterID.Value), SaveVisitPaymentList.ToArray<VisitPayment>(), SaveCheckList.ToArray<Check>(),
                      SavePPHeaderList.ToArray<PPHeader>(), SavePPLineItemList.ToArray<PPLineItem>(), SaveAccountTransactionList.ToArray<AccountTransaction>(), SaveVisitPaymenHistoryList.ToArray<VisitPaymentHistory>(), string.Empty);
                        IList<VisitPaymentDTO> VisitPayDTO = new List<VisitPaymentDTO>();
                        //if (hdnEncounterID.Value != "")
                        //{
                        //    if (hdnBatchStatus.Value.ToString().ToUpper().Contains("CLOSED") == true)
                        //    {
                        //        EncounterManager encMngr = new EncounterManager();
                        //        IList<Encounter> enclist = new List<Encounter>();

                        //        enclist = encMngr.GetEncounterByEncounterID(Convert.ToUInt64(hdnEncounterID.Value));

                        //        if (enclist.Count > 0)
                        //        {
                        //            enclist[0].Batch_Status = "CORRECTED";
                        //            encMngr.UpdateE_SuperBill(enclist, string.Empty);
                        //            lblStatus.Text = string.Empty;
                        //        }
                        //    }
                        //}


                        // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380034');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "QuickpatientCreate", "savesuccessfully();", true);
                        bValidPaymentInfo = true;
                        SaveVisitPaymentList.Clear();
                        LoadPaymentInfoGrid(VisitList);
                        paymentinformationdisableall();
                        PaymentInformationClearAll();
                        cboMethodOfPayment.SelectedIndex = 0;

                        TextBoxColorChange(txtRecOnAcc, false);
                        TextBoxColorChange(txtRefundAmount, false);
                        TextBoxColorChange(txtpaidBy, false);
                        ComboBoxColorChange(cboRelation, false);
                        cboRelation.SelectedIndex = 0;
                        txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                    }
                    divLoading.Style.Add("display", "none");


                }
                //else
                //{
                //    VisitPaymentArc objPaymentArc = new VisitPaymentArc();
                //    CheckArc objCheckArc = new CheckArc();
                //    PPHeaderArc objPPHeaderArc = new PPHeaderArc();
                //    PPLineItemArc objPPLineItemArc = new PPLineItemArc();
                //    AccountTransactionArc objAccountTransactionArc = new AccountTransactionArc();

                //    if (txtPaymentAmount.ReadOnly == false)
                //    {
                //        objPaymentArc.Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                //        objCheckArc.Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                //        objPPHeaderArc.Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                //        objPPLineItemArc.Amount = Convert.ToDecimal(txtPaymentAmount.Text);
                //        objAccountTransactionArc.Amount = -(Convert.ToDecimal(txtPaymentAmount.Text));
                //    }

                //    if (txtRefundAmount.Text != string.Empty)
                //    {
                //        objPaymentArc.Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);
                //    }
                //    if (txtRecOnAcc.Text != string.Empty)
                //        objPaymentArc.Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);

                //    if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                //    {
                //        objPaymentArc.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                //        objCheckArc.Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                //    }
                //    else
                //    {
                //        objPaymentArc.Check_Date = DateTime.MinValue;
                //        objCheckArc.Check_Date = DateTime.MinValue;
                //    }


                //    objPaymentArc.Relationship = cboRelation.SelectedItem.Text;
                //    objPaymentArc.Amount_Paid_By = txtpaidBy.Text;

                //    objPaymentArc.Check_Card_No = txtCheckNo.Text;
                //    objPaymentArc.Auth_No = txtAuthNo.Text;
                //    objPaymentArc.Method_of_Payment = cboMethodOfPayment.Text;
                //    objPaymentArc.Payment_Note = txtPaymentNote.Text;
                //    objPaymentArc.Human_ID = humanID;
                //    objPaymentArc.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);

                //    objPaymentArc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //    objPaymentArc.Created_By = ClientSession.UserName;
                //    objPaymentArc.Is_Delete = "N";

                //    SaveVisitPaymentArcList.Add(objPaymentArc);
                //    FillVisitPaymentHistory(null, objPaymentArc, string.Empty);

                //    //  objCheck.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                //    objCheckArc.Human_ID = humanID;
                //    objCheckArc.Payment_Type = cboMethodOfPayment.Text;
                //    objCheckArc.Created_By = ClientSession.UserName;

                //    objCheckArc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //    objCheckArc.Carrier_Patient_Name = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name;
                //    if (hdnEncounterID.Value != string.Empty)
                //    {
                //        objCheckArc.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                //        if (cboMethodOfPayment.Text == "Cash")
                //        {
                //            objCheckArc.Payment_ID = hdnEncounterID.Value;
                //        }
                //    }
                //    if (txtCheckNo.Text != string.Empty)
                //        objCheckArc.Payment_ID = txtCheckNo.Text;
                //    if (hdnCarrierId.Value != string.Empty)
                //    {
                //        objCheckArc.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                //    }
                //    objCheckArc.Is_Delete = "N";
                //    SaveCheckArcList.Add(objCheckArc);

                //    // objPPHeader.Human_ID = Convert.ToUInt64(ClientSession.HumanId);

                //    objPPHeaderArc.Human_ID = humanID;
                //    objPPHeaderArc.Created_By = ClientSession.UserName;

                //    objPPHeaderArc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //    if (hdnEncounterID.Value != string.Empty)
                //    {
                //        objPPHeaderArc.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                //    }
                //    if (hdnCarrierId.Value != string.Empty)
                //    {
                //        objPPHeaderArc.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value); ;
                //    }
                //    objPPHeaderArc.Is_Delete = "N";
                //    SavePPHeaderArcList.Add(objPPHeaderArc);
                //    objPPLineItemArc.Claim_Type = "PATIENT";
                //    objPPLineItemArc.Line_Type = "UNAPPLIED";
                //    objPPLineItemArc.Created_By = ClientSession.UserName;

                //    objPPLineItemArc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //    //  objPPLineItem.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                //    objPPLineItemArc.Human_ID = humanID;
                //    objPPLineItemArc.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                //    if (hdnCarrierId.Value != string.Empty)
                //    {
                //        objPPLineItemArc.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                //    }
                //    objPPLineItemArc.Is_Delete = "N";
                //    //objPPLineItem.Comments = "ON " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + " by " + ClientSession.UserName + ":" + "\n" + "Method of Payment:" + cboMethodOfPayment.Text + "\n" + "Check#:" + txtCheckNo.Text + "\n" + "CC Auth#:" + txtAuthNo.Text + "\n" + "Copay:" + txtPaymentAmount.Text + "\n" + "Recd on Acct:" + txtRecOnAcc.Text + "\n" + "Past Due:" + txtPastDue.Text + "\n" + "Refund Amt:" + txtRefundAmount.Text + "\n" + "Check Date:" + dtpCheckDate.Text + "\n" + "Payment Note:" + txtPaymentNote.Text + "\n" + "Relationship:" + cboRelation.Text + "\n" + "Paid By:" + txtpaidBy.Text;


                //    SavePPLineItemArcList.Add(objPPLineItemArc);

                //    objAccountTransactionArc.Deposit_Date = UtilityManager.ConvertToLocal(Convert.ToDateTime(ddlFacilitywithDOS.SelectedItem.Text.Split(new char[] { '-' }, 2)[1]));
                //    // objAccountTransaction.Human_ID = Convert.ToUInt64(ClientSession.HumanId);

                //    objAccountTransactionArc.Human_ID = humanID;
                //    objAccountTransactionArc.Claim_Type = "PATIENT";
                //    objAccountTransactionArc.Line_Type = "UNAPPLIED";
                //    objAccountTransactionArc.Source_Type = "PP_LINE_ITEM";
                //    objAccountTransactionArc.Created_By = ClientSession.UserName;

                //    objAccountTransactionArc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //    objAccountTransactionArc.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                //    if (hdnCarrierId.Value != string.Empty)
                //    {
                //        objAccountTransactionArc.Carrier_ID = Convert.ToUInt32(hdnCarrierId.Value);
                //    }
                //    objAccountTransactionArc.Is_Delete = "N";
                //    SaveAccountTransactionArcList.Add(objAccountTransactionArc);

                //    objAccountTransactionArc = new AccountTransactionArc();
                //    if (txtRefundAmount.Text != string.Empty && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                //    {
                //        objAccountTransactionArc.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                //        objAccountTransactionArc.Reversal_Refund_Category = "REFUND";
                //        objAccountTransactionArc.Deposit_Date = UtilityManager.ConvertToLocal(Convert.ToDateTime(ddlFacilitywithDOS.SelectedItem.Text.Split(new char[] { '-' }, 2)[1])); // UtilityManager.ConvertToLocal(objCheckOut.EncounterObj.Appointment_Date);
                //        //  objAccountTransaction.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                //        objAccountTransactionArc.Human_ID = humanID;
                //        objAccountTransactionArc.Claim_Type = "PATIENT";
                //        objAccountTransactionArc.Line_Type = "UNAPPLIED";
                //        objAccountTransactionArc.Source_Type = "PP_LINE_ITEM";
                //        objAccountTransactionArc.Created_By = ClientSession.UserName;

                //        objAccountTransactionArc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //        objAccountTransactionArc.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                //        objAccountTransactionArc.Is_Delete = "N";
                //        //objAccountTransactipion.Carrier_ID = Cash_Carrier_ID;
                //        SaveAccountTransactionArcList.Add(objAccountTransactionArc);
                //    }

                //    IList<VisitPaymentDTO> VisitList;
                //    if (hdnEncounterID.Value != string.Empty)
                //    {
                //        VisitList = visitArcMgr.SaveVisitPaymentArc(Convert.ToUInt64(hdnEncounterID.Value), SaveVisitPaymentArcList.ToArray<VisitPaymentArc>(), SaveCheckArcList.ToArray<CheckArc>(),
                //        SavePPHeaderArcList.ToArray<PPHeaderArc>(), SavePPLineItemArcList.ToArray<PPLineItemArc>(), SaveAccountTransactionArcList.ToArray<AccountTransactionArc>(), SaveVisitPaymenHistoryArcList.ToArray<VisitPaymentHistoryArc>(), string.Empty);

                //        //string encounter = Convert.ToUInt64(hdnEncounterID.Value).ToString();

                //        //string updateQuery = "update encounter_arc set Batch_Status ='CORRECTED'  and Modified_Date_and_Time='" + TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "' where encounter_id='" + encounter + "'";

                //        //DataSet dsReturn = DBConnector.ReadData(updateQuery);
                //        //DataTable dt = dsReturn.Tables[0];

                //        lblStatus.Text = string.Empty;


                //        //ApplicationObject.erroHandler.DisplayErrorMessage("380034", "Quick Patient Create", this.Page);
                //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380034');", true);
                //        bValidPaymentInfo = true;
                //        SaveVisitPaymentList.Clear();
                //        LoadPaymentInfoGrid(VisitList);
                //        paymentinformationdisableall();
                //        PaymentInformationClearAll();
                //        cboMethodOfPayment.SelectedIndex = 0;

                //        TextBoxColorChange(txtRecOnAcc, false);
                //        TextBoxColorChange(txtRefundAmount, false);
                //        TextBoxColorChange(txtpaidBy, false);
                //        ComboBoxColorChange(cboRelation, false);
                //        cboRelation.SelectedIndex = 0;
                //        txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;
                //    }
                //    divLoading.Style.Add("display", "none");

                //}


            }
            else
            {
                //if (ddlFacilitywithDOS.SelectedValue != string.Empty)
                if (ddlFacilitywithDOS.SelectedValue == string.Empty || ddlFacilitywithDOS.SelectedItem.Value.Split('|')[2].ToString().ToUpper() == "MAIN")
                {
                    SaveVisitPaymentList = new List<VisitPayment>();
                    IList<AccountTransaction> ilistAccTran = new List<AccountTransaction>();

                    HumanManager HumanMngr = new HumanManager();
                    FillQuickPatient objcheckout = HumanMngr.GetVisitPaymentDetails(hdnVisitID.Value, hdnPPHeaderID.Value, hdnPPLineItemID.Value, hdnCheckID.Value);

                    if (hdnVisitID.Value != "")
                    {
                        if (objcheckout.VisitPaymentList[0] != null)
                        {
                            FillVisitPaymentHistory(objcheckout.VisitPaymentList[0], null, "DEBIT");

                            if (txtPaymentAmount.Text != "")
                                objcheckout.VisitPaymentList[0].Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                            if (txtRefundAmount.Text != string.Empty)
                                objcheckout.VisitPaymentList[0].Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);

                            objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                            objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
                            objcheckout.VisitPaymentList[0].Facility_Name = ClientSession.FacilityName;
                            if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                            {
                                objcheckout.VisitPaymentList[0].Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                            }
                            else
                            {
                                objcheckout.VisitPaymentList[0].Check_Date = DateTime.MinValue;


                            }
                            objcheckout.VisitPaymentList[0].Check_Card_No = txtCheckNo.Text;
                            objcheckout.VisitPaymentList[0].Auth_No = txtAuthNo.Text;
                            objcheckout.VisitPaymentList[0].Method_of_Payment = cboMethodOfPayment.Text;
                            objcheckout.VisitPaymentList[0].Payment_Note = txtPaymentNote.Text;

                            objcheckout.VisitPaymentList[0].Relationship = cboRelation.Text;
                            objcheckout.VisitPaymentList[0].Amount_Paid_By = txtpaidBy.Text;


                            if (txtRecOnAcc.Text != "")
                                objcheckout.VisitPaymentList[0].Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);


                            objcheckout.VisitPaymentList[0].Batch_Status = "OPEN";

                            SaveVisitPaymentList.Add(objcheckout.VisitPaymentList[0]);

                            FillVisitPaymentHistory(objcheckout.VisitPaymentList[0], null, "CREDIT");
                        }
                    }
                    if (hdnPPHeaderID.Value != "")
                    {
                        PPHeaderManager PPHeaderMngr = new PPHeaderManager();

                        SavePPHeaderList = new List<PPHeader>();
                        if (objcheckout.PPHeaderList.Count > 0 && objcheckout.PPHeaderList[0] != null)
                        {
                            if (txtPaymentAmount.Text != "")
                                objcheckout.PPHeaderList[0].Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);

                            objcheckout.PPHeaderList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                            objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
                            SavePPHeaderList.Add(objcheckout.PPHeaderList[0]);
                        }
                    }
                    if (hdnPPLineItemID.Value != "")
                    {
                        PPLineItemManager PPLineMngr = new PPLineItemManager();
                        SavePPLineItemList = new List<PPLineItem>();
                        //PPLineItem objPPLine = PPLineMngr.GetById(Convert.ToUInt32(hdnPPLineItemID.Value));
                        if (objcheckout.PPLineItemList.Count > 0 && objcheckout.PPLineItemList[0] != null)
                        {
                            if (txtPaymentAmount.Text != "")
                                objcheckout.PPLineItemList[0].Amount = Convert.ToDecimal(txtPaymentAmount.Text);

                            objcheckout.PPLineItemList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                            objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;


                            //  objcheckout.PPLineItemList[0].Comments += Environment.NewLine + "ON " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + " by " + ClientSession.UserName + ":" + "\n" + "Method of Payment:" + cboMethodOfPayment.Text + "\n" + "Check#:" + txtCheckNo.Text + "\n" + "CC Auth#:" + txtAuthNo.Text + "\n" + "Copay:" + txtPaymentAmount.Text + "\n" + "Recd on Acct:" + txtRecOnAcc.Text + "\n" + "Past Due:" + txtPastDue.Text + "\n" + "Refund Amt:" + txtRefundAmount.Text + "\n" + "Check Date:" + dtpCheckDate.Text + "\n" + "Payment Note:" + txtPaymentNote.Text + "\n" + "Relationship:" + cboRelation.Text + "\n" + "Paid By:" + txtpaidBy.Text;

                            SavePPLineItemList.Add(objcheckout.PPLineItemList[0]);
                        }
                        AccountTransactionManager AccTranMngr = new AccountTransactionManager();

                        if (objcheckout.AccountTransaction != null && objcheckout.AccountTransaction.Count > 0)
                        {
                            for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
                            {
                                if (txtPaymentAmount.Text != "")
                                    objcheckout.AccountTransaction[iNumber].Amount = -Convert.ToDecimal(txtPaymentAmount.Text);

                                objcheckout.AccountTransaction[iNumber].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                                objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;


                            }
                            AccountTransaction objAccountTransaction = new AccountTransaction();
                            IList<AccountTransaction> ilistRefundTrans = objcheckout.AccountTransaction.Where(a => a.Reversal_Refund_Category == "REFUND").ToList<AccountTransaction>();
                            if (ilistRefundTrans != null && ilistRefundTrans.Count > 0)
                            {
                                if (txtRefundAmount.Text != "")
                                    ilistRefundTrans[0].Amount = -Convert.ToDecimal(txtRefundAmount.Text);
                            }
                            else if (txtRefundAmount.Text != string.Empty && objcheckout.AccountTransaction.Count == 1 && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                            {
                                objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                                objAccountTransaction.Reversal_Refund_Category = "REFUND";
                                objAccountTransaction.Deposit_Date = objcheckout.AccountTransaction[0].Deposit_Date;
                                //  objAccountTransaction.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                                objAccountTransaction.Human_ID = humanID;
                                objAccountTransaction.Claim_Type = "PATIENT";
                                objAccountTransaction.Line_Type = "UNAPPLIED";
                                objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                                objAccountTransaction.Created_By = ClientSession.UserName;

                                objAccountTransaction.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                                objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                                objAccountTransaction.Is_Delete = "N";
                                //objAccountTransaction.Carrier_ID = Cash_Carrier_ID;
                                SaveAccountTransactionList.Add(objAccountTransaction);
                            }
                        }
                    }
                    if (hdnCheckID.Value != "")
                    {
                        CheckManager CheckMngr = new CheckManager();
                        SaveCheckList = new List<Check>();
                        if (objcheckout.CheckList.Count > 0 && objcheckout.CheckList[0] != null)
                        {
                            if (txtPaymentAmount.Text != "")
                                objcheckout.CheckList[0].Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);

                            objcheckout.PPHeaderList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                            objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
                            if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                            {
                                objcheckout.CheckList[0].Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                            }
                            else
                            {
                                objcheckout.CheckList[0].Check_Date = DateTime.MinValue;
                            }
                            objcheckout.CheckList[0].Payment_Type = cboMethodOfPayment.Text;
                            objcheckout.CheckList[0].Carrier_Patient_Name = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name;

                            if (txtCheckNo.Text != string.Empty)
                                objcheckout.CheckList[0].Payment_ID = txtCheckNo.Text;
                            SaveCheckList.Add(objcheckout.CheckList[0]);
                        }
                    }
                    IList<VisitPaymentDTO> VisitPayDTO = new List<VisitPaymentDTO>();
                    if (hdnEncounterID.Value != "")
                    {
                        VisitPayDTO = visitMgr.UpdateVisitPayment(SaveVisitPaymentList, SaveCheckList, SavePPHeaderList, SavePPLineItemList, ilistAccTran, SaveAccountTransactionList, SaveVisitPaymenHistoryList, Convert.ToUInt32(hdnEncounterID.Value));

                        //if (hdnBatchStatus.Value.ToString().ToUpper().Contains("CLOSED") == true)
                        //{
                        //    EncounterManager encMngr = new EncounterManager();
                        //    IList<Encounter> enclist = new List<Encounter>();

                        //    enclist = encMngr.GetEncounterByEncounterID(Convert.ToUInt64(hdnEncounterID.Value));

                        //    if (enclist.Count > 0)
                        //    {
                        //        enclist[0].Batch_Status = "CORRECTED";
                        //        encMngr.UpdateE_SuperBill(enclist, string.Empty);
                        //        lblStatus.Text = string.Empty;
                        //    }
                        //}
                    }

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380034');", true);
                    bValidPaymentInfo = true;
                    SaveVisitPaymentList.Clear();
                    LoadPaymentInfoGrid(VisitPayDTO);
                    paymentinformationdisableall();
                    PaymentInformationClearAll();
                    cboMethodOfPayment.SelectedIndex = 0;
                    TextBoxColorChange(txtRecOnAcc, false);
                    TextBoxColorChange(txtRefundAmount, false);
                    TextBoxColorChange(txtpaidBy, true);
                    ComboBoxColorChange(cboRelation, true);
                    TextBoxColorChange(txtpaidBy, false);
                    ComboBoxColorChange(cboRelation, false);
                    cboRelation.SelectedIndex = 0;
                    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;

                    btnAdd.Text = "Add";
                    btnClear.Text = "Clear All";
                }
                //else
                //{

                //    SaveVisitPaymentArcList = new List<VisitPaymentArc>();
                //    IList<AccountTransactionArc> ilistAccTran = new List<AccountTransactionArc>();

                //    HumanManager HumanMngr = new HumanManager();
                //    FillQuickPatientArc objcheckout = HumanMngr.GetVisitPaymentDetailsArc(hdnVisitID.Value, hdnPPHeaderID.Value, hdnPPLineItemID.Value, hdnCheckID.Value);

                //    if (hdnVisitID.Value != "")
                //    {
                //        if (objcheckout.VisitPaymentList[0] != null)
                //        {
                //            FillVisitPaymentHistory(null, objcheckout.VisitPaymentList[0], "DEBIT");

                //            if (txtPaymentAmount.Text != "")
                //                objcheckout.VisitPaymentList[0].Patient_Payment = Convert.ToDecimal(txtPaymentAmount.Text);
                //            if (txtRefundAmount.Text != string.Empty)
                //                objcheckout.VisitPaymentList[0].Refund_Amount = Convert.ToDecimal(txtRefundAmount.Text);

                //            objcheckout.VisitPaymentList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                //            objcheckout.VisitPaymentList[0].Modified_By = ClientSession.UserName;
                //            if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                //            {
                //                objcheckout.VisitPaymentList[0].Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                //            }
                //            else
                //            {
                //                objcheckout.VisitPaymentList[0].Check_Date = DateTime.MinValue;
                //            }
                //            objcheckout.VisitPaymentList[0].Check_Card_No = txtCheckNo.Text;
                //            objcheckout.VisitPaymentList[0].Auth_No = txtAuthNo.Text;
                //            objcheckout.VisitPaymentList[0].Method_of_Payment = cboMethodOfPayment.Text;
                //            objcheckout.VisitPaymentList[0].Payment_Note = txtPaymentNote.Text;

                //            objcheckout.VisitPaymentList[0].Relationship = cboRelation.Text;
                //            objcheckout.VisitPaymentList[0].Amount_Paid_By = txtpaidBy.Text;


                //            if (txtRecOnAcc.Text != "")
                //                objcheckout.VisitPaymentList[0].Rec_On_Acc = Convert.ToDecimal(txtRecOnAcc.Text);

                //            objcheckout.VisitPaymentList[0].Batch_Status = "OPEN";

                //            SaveVisitPaymentArcList.Add(objcheckout.VisitPaymentList[0]);

                //            FillVisitPaymentHistory(null, objcheckout.VisitPaymentList[0], "CREDIT");
                //        }
                //    }
                //    if (hdnPPHeaderID.Value != "")
                //    {
                //        PPHeaderManager PPHeaderMngr = new PPHeaderManager();

                //        SavePPHeaderArcList = new List<PPHeaderArc>();
                //        if (objcheckout.PPHeaderList[0] != null)
                //        {
                //            if (txtPaymentAmount.Text != "")
                //                objcheckout.PPHeaderList[0].Total_Payment = Convert.ToDecimal(txtPaymentAmount.Text);

                //            objcheckout.PPHeaderList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                //            objcheckout.PPHeaderList[0].Modified_By = ClientSession.UserName;
                //            SavePPHeaderArcList.Add(objcheckout.PPHeaderList[0]);
                //        }
                //    }
                //    if (hdnPPLineItemID.Value != "")
                //    {
                //        PPLineItemManager PPLineMngr = new PPLineItemManager();
                //        SavePPLineItemArcList = new List<PPLineItemArc>();
                //        //PPLineItem objPPLine = PPLineMngr.GetById(Convert.ToUInt32(hdnPPLineItemID.Value));
                //        if (objcheckout.PPLineItemList[0] != null)
                //        {
                //            if (txtPaymentAmount.Text != "")
                //                objcheckout.PPLineItemList[0].Amount = Convert.ToDecimal(txtPaymentAmount.Text);

                //            objcheckout.PPLineItemList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                //            objcheckout.PPLineItemList[0].Modified_By = ClientSession.UserName;


                //            // objcheckout.PPLineItemList[0].Comments += Environment.NewLine + "ON " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + " by " + ClientSession.UserName + ":" + "\n" + "Method of Payment:" + cboMethodOfPayment.Text + "\n" + "Check#:" + txtCheckNo.Text + "\n" + "CC Auth#:" + txtAuthNo.Text + "\n" + "Copay:" + txtPaymentAmount.Text + "\n" + "Recd on Acct:" + txtRecOnAcc.Text + "\n" + "Past Due:" + txtPastDue.Text + "\n" + "Refund Amt:" + txtRefundAmount.Text + "\n" + "Check Date:" + dtpCheckDate.Text + "\n" + "Payment Note:" + txtPaymentNote.Text + "\n" + "Relationship:" + cboRelation.Text + "\n" + "Paid By:" + txtpaidBy.Text;

                //            SavePPLineItemArcList.Add(objcheckout.PPLineItemList[0]);
                //        }
                //        AccountTransactionManager AccTranMngr = new AccountTransactionManager();

                //        if (objcheckout.AccountTransaction != null && objcheckout.AccountTransaction.Count > 0)
                //        {
                //            for (int iNumber = 0; iNumber < ilistAccTran.Count; iNumber++)
                //            {
                //                if (txtPaymentAmount.Text != "")
                //                    objcheckout.AccountTransaction[iNumber].Amount = -Convert.ToDecimal(txtPaymentAmount.Text);

                //                objcheckout.AccountTransaction[iNumber].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                //                objcheckout.AccountTransaction[iNumber].Modified_By = ClientSession.UserName;
                //            }
                //            AccountTransactionArc objAccountTransaction = new AccountTransactionArc();
                //            IList<AccountTransactionArc> ilistRefundTrans = objcheckout.AccountTransaction.Where(a => a.Reversal_Refund_Category == "REFUND").ToList<AccountTransactionArc>();
                //            if (ilistRefundTrans != null && ilistRefundTrans.Count > 0)
                //            {
                //                if (txtRefundAmount.Text != "")
                //                    ilistRefundTrans[0].Amount = -Convert.ToDecimal(txtRefundAmount.Text);
                //            }
                //            else if (txtRefundAmount.Text != string.Empty && objcheckout.AccountTransaction.Count == 1 && txtRefundAmount.Text != "0.00" && txtRefundAmount.Text != "0")
                //            {
                //                objAccountTransaction.Amount = -(Convert.ToDecimal(txtRefundAmount.Text));
                //                objAccountTransaction.Reversal_Refund_Category = "REFUND";
                //                objAccountTransaction.Deposit_Date = objcheckout.AccountTransaction[0].Deposit_Date;
                //                //  objAccountTransaction.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                //                objAccountTransaction.Human_ID = humanID;
                //                objAccountTransaction.Claim_Type = "PATIENT";
                //                objAccountTransaction.Line_Type = "UNAPPLIED";
                //                objAccountTransaction.Source_Type = "PP_LINE_ITEM";
                //                objAccountTransaction.Created_By = ClientSession.UserName;

                //                objAccountTransaction.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                //                objAccountTransaction.Encounter_ID = Convert.ToUInt64(hdnEncounterID.Value);
                //                objAccountTransaction.Is_Delete = "N";
                //                //objAccountTransaction.Carrier_ID = Cash_Carrier_ID;
                //                SaveAccountTransactionArcList.Add(objAccountTransaction);
                //            }
                //        }
                //    }
                //    if (hdnCheckID.Value != "")
                //    {
                //        CheckManager CheckMngr = new CheckManager();
                //        SaveCheckArcList = new List<CheckArc>();
                //        if (objcheckout.CheckList[0] != null)
                //        {
                //            if (txtPaymentAmount.Text != "")
                //                objcheckout.CheckList[0].Payment_Amount = Convert.ToDecimal(txtPaymentAmount.Text);

                //            objcheckout.PPHeaderList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                //            objcheckout.CheckList[0].Modified_By = ClientSession.UserName;
                //            if (dtpCheckDate.Text != "" && dtpCheckDate.Enabled == true)
                //            {
                //                objcheckout.CheckList[0].Check_Date = Convert.ToDateTime(dtpCheckDate.Text);
                //            }
                //            else
                //            {
                //                objcheckout.CheckList[0].Check_Date = DateTime.MinValue;
                //            }
                //            objcheckout.CheckList[0].Payment_Type = cboMethodOfPayment.Text;
                //            objcheckout.CheckList[0].Carrier_Patient_Name = divPatientstrip.InnerText.Split('|')[0]; //objCheckOutLoad.HumanObj.Last_Name;

                //            if (txtCheckNo.Text != string.Empty)
                //                objcheckout.CheckList[0].Payment_ID = txtCheckNo.Text;
                //            SaveCheckArcList.Add(objcheckout.CheckList[0]);
                //        }
                //    }
                //    IList<VisitPaymentDTO> VisitPayDTO = new List<VisitPaymentDTO>();
                //    if (hdnEncounterID.Value != "")
                //    {
                //        VisitPayDTO = visitArcMgr.UpdateVisitPaymentArc(SaveVisitPaymentArcList, SaveCheckArcList, SavePPHeaderArcList, SavePPLineItemArcList, ilistAccTran, SaveAccountTransactionArcList, SaveVisitPaymenHistoryArcList, Convert.ToUInt32(hdnEncounterID.Value));


                //        //string encounter = Convert.ToUInt64(hdnEncounterID.Value).ToString();

                //        //string updateQuery = "update encounter_arc set Batch_Status ='CORRECTED'  and Modified_Date_and_Time='" + TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "' where encounter_id='" + encounter + "'";

                //        //DataSet dsReturn = DBConnector.ReadData(updateQuery);
                //        //DataTable dt = dsReturn.Tables[0];
                //        lblStatus.Text = string.Empty;
                //    }

                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QuickpatientCreate", "DisplayErrorMessage('380034');", true);
                //    bValidPaymentInfo = true;
                //    SaveVisitPaymentList.Clear();
                //    LoadPaymentInfoGrid(VisitPayDTO);
                //    paymentinformationdisableall();
                //    PaymentInformationClearAll();
                //    cboMethodOfPayment.SelectedIndex = 0;
                //    TextBoxColorChange(txtRecOnAcc, false);
                //    TextBoxColorChange(txtRefundAmount, false);
                //    TextBoxColorChange(txtpaidBy, true);
                //    ComboBoxColorChange(cboRelation, true);
                //    TextBoxColorChange(txtpaidBy, false);
                //    ComboBoxColorChange(cboRelation, false);
                //    cboRelation.SelectedIndex = 0;
                //    txtpaidBy.Text = divPatientstrip.InnerText.Split('|')[0]; // objCheckOutLoad.HumanObj.Last_Name + ',' + objCheckOutLoad.HumanObj.First_Name;

                //    btnAdd.Text = "Add";
                //    btnClear.Text = "Clear All";

                //}
            }

            string YesNoMessage = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            btnAdd.Enabled = false;
            //lblPaymentNote.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ADD", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        private void FillVisitPaymentHistory(VisitPayment EditVisitPayment, VisitPaymentArc EditVisitPaymentArc, string sCreditType)
        {
            VisitPaymentHistory objVisitPaymentHistory = new VisitPaymentHistory();
            //VisitPaymentHistoryArc objVisitPaymentHistoryArc = new VisitPaymentHistoryArc();



            if (btnAdd.Text == "Add")
            {
                SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
                //SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();

                //if (ddlFacilitywithDOS.SelectedValue != string.Empty)

                if (ddlFacilitywithDOS.SelectedValue == string.Empty || ddlFacilitywithDOS.SelectedItem.Value.Split('|')[2].ToString().ToUpper() == "MAIN")
                {
                    objVisitPaymentHistory.Visit_Payment_ID = EditVisitPayment.Id;
                    objVisitPaymentHistory.Amount_Paid_By = EditVisitPayment.Amount_Paid_By;
                    objVisitPaymentHistory.Auth_No = EditVisitPayment.Auth_No;
                    objVisitPaymentHistory.Check_Card_No = EditVisitPayment.Check_Card_No;
                    objVisitPaymentHistory.Check_Date = EditVisitPayment.Check_Date;
                    objVisitPaymentHistory.Created_By = EditVisitPayment.Created_By;
                    objVisitPaymentHistory.Created_Date_And_Time = EditVisitPayment.Created_Date_And_Time;
                    objVisitPaymentHistory.Encounter_ID = EditVisitPayment.Encounter_ID;
                    objVisitPaymentHistory.Human_ID = EditVisitPayment.Human_ID;
                    objVisitPaymentHistory.Is_Delete = EditVisitPayment.Is_Delete;
                    objVisitPaymentHistory.Method_of_Payment = EditVisitPayment.Method_of_Payment;
                    objVisitPaymentHistory.Modified_By = EditVisitPayment.Modified_By;
                    objVisitPaymentHistory.Modified_Date_And_Time = EditVisitPayment.Modified_Date_And_Time;
                    objVisitPaymentHistory.Patient_Payment = (EditVisitPayment.Patient_Payment);
                    objVisitPaymentHistory.Payment_Message_ID = EditVisitPayment.Payment_Message_ID;
                    objVisitPaymentHistory.Payment_Note = EditVisitPayment.Payment_Note;
                    objVisitPaymentHistory.Rec_On_Acc = EditVisitPayment.Rec_On_Acc;
                    objVisitPaymentHistory.Refund_Amount = EditVisitPayment.Refund_Amount;
                    objVisitPaymentHistory.Relationship = EditVisitPayment.Relationship;
                    objVisitPaymentHistory.Version = EditVisitPayment.Version;
                    objVisitPaymentHistory.Voucher_No = EditVisitPayment.Voucher_No;
                    objVisitPaymentHistory.Batch_Status = EditVisitPayment.Batch_Status;
                    objVisitPaymentHistory.Facility_Name = EditVisitPayment.Facility_Name;

                    SaveVisitPaymenHistoryList.Add(objVisitPaymentHistory);
                }
                //else
                //{
                //    objVisitPaymentHistoryArc.Visit_Payment_ID = EditVisitPaymentArc.Id;
                //    objVisitPaymentHistoryArc.Amount_Paid_By = EditVisitPaymentArc.Amount_Paid_By;
                //    objVisitPaymentHistoryArc.Auth_No = EditVisitPaymentArc.Auth_No;
                //    objVisitPaymentHistoryArc.Check_Card_No = EditVisitPaymentArc.Check_Card_No;
                //    objVisitPaymentHistoryArc.Check_Date = EditVisitPaymentArc.Check_Date;
                //    objVisitPaymentHistoryArc.Created_By = EditVisitPaymentArc.Created_By;
                //    objVisitPaymentHistoryArc.Created_Date_And_Time = EditVisitPaymentArc.Created_Date_And_Time;
                //    objVisitPaymentHistoryArc.Encounter_ID = EditVisitPaymentArc.Encounter_ID;
                //    objVisitPaymentHistoryArc.Human_ID = EditVisitPaymentArc.Human_ID;
                //    objVisitPaymentHistoryArc.Is_Delete = EditVisitPaymentArc.Is_Delete;
                //    objVisitPaymentHistoryArc.Method_of_Payment = EditVisitPaymentArc.Method_of_Payment;
                //    objVisitPaymentHistoryArc.Modified_By = EditVisitPaymentArc.Modified_By;
                //    objVisitPaymentHistoryArc.Modified_Date_And_Time = EditVisitPaymentArc.Modified_Date_And_Time;
                //    objVisitPaymentHistoryArc.Patient_Payment = (EditVisitPaymentArc.Patient_Payment);
                //    objVisitPaymentHistoryArc.Payment_Message_ID = EditVisitPaymentArc.Payment_Message_ID;
                //    objVisitPaymentHistoryArc.Payment_Note = EditVisitPaymentArc.Payment_Note;
                //    objVisitPaymentHistoryArc.Rec_On_Acc = EditVisitPaymentArc.Rec_On_Acc;
                //    objVisitPaymentHistoryArc.Refund_Amount = EditVisitPaymentArc.Refund_Amount;
                //    objVisitPaymentHistoryArc.Relationship = EditVisitPaymentArc.Relationship;
                //    objVisitPaymentHistoryArc.Version = EditVisitPaymentArc.Version;
                //    objVisitPaymentHistoryArc.Voucher_No = EditVisitPaymentArc.Voucher_No;

                //    SaveVisitPaymenHistoryArcList.Add(objVisitPaymentHistoryArc);
                //}
            }
            else
            {
                //if (ddlFacilitywithDOS.SelectedValue != string.Empty)

                if (ddlFacilitywithDOS.SelectedValue == string.Empty || ddlFacilitywithDOS.SelectedItem.Value.Split('|')[2].ToString().ToUpper() == "MAIN")
                {
                    if (sCreditType == "DEBIT")
                    {
                        SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
                        //SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();

                        objVisitPaymentHistory.Visit_Payment_ID = EditVisitPayment.Id;
                        objVisitPaymentHistory.Amount_Paid_By = EditVisitPayment.Amount_Paid_By;
                        objVisitPaymentHistory.Auth_No = EditVisitPayment.Auth_No;
                        objVisitPaymentHistory.Check_Card_No = EditVisitPayment.Check_Card_No;
                        objVisitPaymentHistory.Check_Date = EditVisitPayment.Check_Date;
                        objVisitPaymentHistory.Created_By = ClientSession.UserName; // EditVisitPayment.Created_By;
                        objVisitPaymentHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); //EditVisitPayment.Created_Date_And_Time;
                        objVisitPaymentHistory.Encounter_ID = EditVisitPayment.Encounter_ID;
                        objVisitPaymentHistory.Human_ID = EditVisitPayment.Human_ID;
                        objVisitPaymentHistory.Is_Delete = EditVisitPayment.Is_Delete;
                        objVisitPaymentHistory.Method_of_Payment = EditVisitPayment.Method_of_Payment;
                        objVisitPaymentHistory.Modified_By = string.Empty; // EditVisitPayment.Modified_By;
                        objVisitPaymentHistory.Modified_Date_And_Time = DateTime.MinValue; // EditVisitPayment.Modified_Date_And_Time;
                        objVisitPaymentHistory.Patient_Payment = -(EditVisitPayment.Patient_Payment);
                        objVisitPaymentHistory.Payment_Message_ID = EditVisitPayment.Payment_Message_ID;
                        objVisitPaymentHistory.Payment_Note = string.Empty; // EditVisitPayment.Payment_Note;
                        objVisitPaymentHistory.Rec_On_Acc = -(EditVisitPayment.Rec_On_Acc);
                        objVisitPaymentHistory.Refund_Amount = -(EditVisitPayment.Refund_Amount);
                        objVisitPaymentHistory.Relationship = EditVisitPayment.Relationship;
                        objVisitPaymentHistory.Version = EditVisitPayment.Version;
                        objVisitPaymentHistory.Voucher_No = EditVisitPayment.Voucher_No;
                        objVisitPaymentHistory.Batch_Status = EditVisitPayment.Batch_Status;
                        objVisitPaymentHistory.Facility_Name = EditVisitPayment.Facility_Name;

                        SaveVisitPaymenHistoryList.Add(objVisitPaymentHistory);
                    }
                    else if (sCreditType == "CREDIT")
                    {

                        objVisitPaymentHistory = new VisitPaymentHistory();

                        objVisitPaymentHistory.Visit_Payment_ID = EditVisitPayment.Id;
                        objVisitPaymentHistory.Amount_Paid_By = EditVisitPayment.Amount_Paid_By;
                        objVisitPaymentHistory.Auth_No = EditVisitPayment.Auth_No;
                        objVisitPaymentHistory.Check_Card_No = EditVisitPayment.Check_Card_No;
                        objVisitPaymentHistory.Check_Date = EditVisitPayment.Check_Date;
                        objVisitPaymentHistory.Created_By = EditVisitPayment.Created_By;
                        objVisitPaymentHistory.Created_Date_And_Time = EditVisitPayment.Created_Date_And_Time;
                        objVisitPaymentHistory.Encounter_ID = EditVisitPayment.Encounter_ID;
                        objVisitPaymentHistory.Human_ID = EditVisitPayment.Human_ID;
                        objVisitPaymentHistory.Is_Delete = EditVisitPayment.Is_Delete;
                        objVisitPaymentHistory.Method_of_Payment = EditVisitPayment.Method_of_Payment;
                        objVisitPaymentHistory.Modified_By = EditVisitPayment.Modified_By;
                        objVisitPaymentHistory.Modified_Date_And_Time = EditVisitPayment.Modified_Date_And_Time;
                        objVisitPaymentHistory.Patient_Payment = EditVisitPayment.Patient_Payment;
                        objVisitPaymentHistory.Payment_Message_ID = EditVisitPayment.Payment_Message_ID;
                        objVisitPaymentHistory.Payment_Note = EditVisitPayment.Payment_Note;
                        objVisitPaymentHistory.Rec_On_Acc = EditVisitPayment.Rec_On_Acc;
                        objVisitPaymentHistory.Refund_Amount = EditVisitPayment.Refund_Amount;
                        objVisitPaymentHistory.Relationship = EditVisitPayment.Relationship;
                        objVisitPaymentHistory.Version = EditVisitPayment.Version;
                        objVisitPaymentHistory.Voucher_No = EditVisitPayment.Voucher_No;
                        objVisitPaymentHistory.Batch_Status = EditVisitPayment.Batch_Status;
                        objVisitPaymentHistory.Facility_Name = EditVisitPayment.Facility_Name;

                        SaveVisitPaymenHistoryList.Add(objVisitPaymentHistory);
                    }
                }
                //else
                //{
                //    if (sCreditType == "DEBIT")
                //    {
                //        SaveVisitPaymenHistoryList = new List<VisitPaymentHistory>();
                //        SaveVisitPaymenHistoryArcList = new List<VisitPaymentHistoryArc>();

                //        objVisitPaymentHistoryArc.Visit_Payment_ID = EditVisitPaymentArc.Id;
                //        objVisitPaymentHistoryArc.Amount_Paid_By = EditVisitPaymentArc.Amount_Paid_By;
                //        objVisitPaymentHistoryArc.Auth_No = EditVisitPaymentArc.Auth_No;
                //        objVisitPaymentHistoryArc.Check_Card_No = EditVisitPaymentArc.Check_Card_No;
                //        objVisitPaymentHistoryArc.Check_Date = EditVisitPaymentArc.Check_Date;
                //        objVisitPaymentHistoryArc.Created_By = EditVisitPaymentArc.Created_By;
                //        objVisitPaymentHistoryArc.Created_Date_And_Time = EditVisitPaymentArc.Created_Date_And_Time;
                //        objVisitPaymentHistoryArc.Encounter_ID = EditVisitPaymentArc.Encounter_ID;
                //        objVisitPaymentHistoryArc.Human_ID = EditVisitPaymentArc.Human_ID;
                //        objVisitPaymentHistoryArc.Is_Delete = EditVisitPaymentArc.Is_Delete;
                //        objVisitPaymentHistoryArc.Method_of_Payment = EditVisitPaymentArc.Method_of_Payment;
                //        objVisitPaymentHistoryArc.Modified_By = EditVisitPaymentArc.Modified_By;
                //        objVisitPaymentHistoryArc.Modified_Date_And_Time = EditVisitPaymentArc.Modified_Date_And_Time;
                //        objVisitPaymentHistoryArc.Patient_Payment = -(EditVisitPaymentArc.Patient_Payment);
                //        objVisitPaymentHistoryArc.Payment_Message_ID = EditVisitPaymentArc.Payment_Message_ID;
                //        objVisitPaymentHistoryArc.Payment_Note = EditVisitPaymentArc.Payment_Note;
                //        objVisitPaymentHistoryArc.Rec_On_Acc = -(EditVisitPaymentArc.Rec_On_Acc);
                //        objVisitPaymentHistoryArc.Refund_Amount = -(EditVisitPaymentArc.Refund_Amount);
                //        objVisitPaymentHistoryArc.Relationship = EditVisitPaymentArc.Relationship;
                //        objVisitPaymentHistoryArc.Version = EditVisitPaymentArc.Version;
                //        objVisitPaymentHistoryArc.Voucher_No = EditVisitPaymentArc.Voucher_No;

                //        SaveVisitPaymenHistoryArcList.Add(objVisitPaymentHistoryArc);
                //    }
                //    else if (sCreditType == "CREDIT")
                //    {

                //        objVisitPaymentHistoryArc = new VisitPaymentHistoryArc();

                //        objVisitPaymentHistoryArc.Visit_Payment_ID = EditVisitPaymentArc.Id;
                //        objVisitPaymentHistoryArc.Amount_Paid_By = EditVisitPaymentArc.Amount_Paid_By;
                //        objVisitPaymentHistoryArc.Auth_No = EditVisitPaymentArc.Auth_No;
                //        objVisitPaymentHistoryArc.Check_Card_No = EditVisitPaymentArc.Check_Card_No;
                //        objVisitPaymentHistoryArc.Check_Date = EditVisitPaymentArc.Check_Date;
                //        objVisitPaymentHistoryArc.Created_By = EditVisitPaymentArc.Created_By;
                //        objVisitPaymentHistoryArc.Created_Date_And_Time = EditVisitPaymentArc.Created_Date_And_Time;
                //        objVisitPaymentHistoryArc.Encounter_ID = EditVisitPaymentArc.Encounter_ID;
                //        objVisitPaymentHistoryArc.Human_ID = EditVisitPaymentArc.Human_ID;
                //        objVisitPaymentHistoryArc.Is_Delete = EditVisitPaymentArc.Is_Delete;
                //        objVisitPaymentHistoryArc.Method_of_Payment = EditVisitPaymentArc.Method_of_Payment;
                //        objVisitPaymentHistoryArc.Modified_By = EditVisitPaymentArc.Modified_By;
                //        objVisitPaymentHistoryArc.Modified_Date_And_Time = EditVisitPaymentArc.Modified_Date_And_Time;
                //        objVisitPaymentHistoryArc.Patient_Payment = EditVisitPaymentArc.Patient_Payment;
                //        objVisitPaymentHistoryArc.Payment_Message_ID = EditVisitPaymentArc.Payment_Message_ID;
                //        objVisitPaymentHistoryArc.Payment_Note = EditVisitPaymentArc.Payment_Note;
                //        objVisitPaymentHistoryArc.Rec_On_Acc = EditVisitPaymentArc.Rec_On_Acc;
                //        objVisitPaymentHistoryArc.Refund_Amount = EditVisitPaymentArc.Refund_Amount;
                //        objVisitPaymentHistoryArc.Relationship = EditVisitPaymentArc.Relationship;
                //        objVisitPaymentHistoryArc.Version = EditVisitPaymentArc.Version;
                //        objVisitPaymentHistoryArc.Voucher_No = EditVisitPaymentArc.Voucher_No;

                //        SaveVisitPaymenHistoryArcList.Add(objVisitPaymentHistoryArc);
                //    }
                //}
            }



        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Cancel")
            {
                btnAdd.Text = "Add";
                btnClear.Text = "Clear All";
            }
            cboMethodOfPayment.Enabled = true;
            cboMethodOfPayment.CssClass = "Editabletxtbox";
            spanPaymentNotes.Attributes.Remove("class"); /*added*/
            spanPaymentNotes.Attributes.Add("class", "spanstyle");
            spanPatientNotestar.Visible = false;
            spanPaymentNotes.Style["margin-left"] = "17px";
            btnAdd.Text = "Add";
            spanCheck.Attributes.Remove("class"); /*added*/
            spanCheck.Attributes.Add("class", "spanstyle");
            spanCheckStar.Visible = false;
            txtCheckNo.Text = string.Empty;
            txtAuthNo.Text = string.Empty;
        }

        protected void btnendwaitcursor_Click(object sender, EventArgs e)
        {
            divLoading.Style.Add("display", "none");
        }



        protected void btnPrintRecipt_Click(object sender, EventArgs e)
        {
            //lblStatus.Text = hdnBatchStatus.Value;

            PrintOrders print = new PrintOrders();
            string sOutput = string.Empty;

            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);

            if (hdnEncounterID.Value != "" && hdnEncounterID.Value != "0")  //(rdbFacilityName.Checked == true && hdnEncounterID.Value != "" && ddlFacilitywithDOS.SelectedValue != string.Empty) 
                sOutput = print.PrintReceipt(Convert.ToUInt64(hdnEncounterID.Value), Convert.ToUInt64(hdnHumID.Value), TargetFileDirectory, true);
            else if (hdnVoucherNo.Value != "" && hdnVoucherNo.Value != "0") // ((rdbVoucher.Checked == true && hdnVoucherNo.Value != "" && ddlVoucher.SelectedIndex != 0) || (rdbFacilityName.Checked == true && hdnVoucherNo.Value != "" && hdnVoucherNo.Value != "0")) // 
                sOutput = print.PrintReceipt(Convert.ToUInt64(hdnVoucherNo.Value), Convert.ToUInt64(hdnHumID.Value), TargetFileDirectory, false);

            if (sOutput == "No Receipt" || sOutput == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Check-PrintReceipt", "DisplayErrorMessage('110085');", true);
                return;
            }

            string sPrintPathName = sOutput.Split('|')[0];
            string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            //if (hdnFileName.Value == string.Empty)
            //{
            hdnFileName.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
            //}
            string FaxSubject = sOutput.Split('|')[1];

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Open Print Receipt-Window", "OpenPrintRecipt_Window('" + FaxSubject + "');", true);
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

            return cell;
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


        protected void rdbNewCollection_CheckedChanged(object sender, EventArgs e)
        {
            // ClearGrid();
            hdnEncounterID.Value = "0";
            hdnVoucherNo.Value = "0";

            rdbVoucher.Checked = false;
            ddlVoucher.Enabled = false;
            ddlVoucher.CssClass = "nonEditabletxtbox";
            if (ddlVoucher.Items.Count > 0)
            {
                ddlVoucher.SelectedIndex = 0;
            }


            ddlFacilitywithDOS.Enabled = false;
            ddlFacilitywithDOS.CssClass = "nonEditabletxtbox";
            if (ddlFacilitywithDOS.Items.Count > 0)
            {
                ddlFacilitywithDOS.SelectedIndex = 0;
            }

            cboMethodOfPayment.CssClass = "Editabletxtbox";
            cboMethodOfPayment.SelectedIndex = 0;
            cboMethodOfPayment.Enabled = true;
            ClearGrid();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }



        protected void rdbFacilityName_CheckedChanged(object sender, EventArgs e)
        {
            ClearGrid();

            hdnEncounterID.Value = "0";
            hdnVoucherNo.Value = "0";

            rdbVoucher.Checked = false;
            ddlVoucher.Enabled = false;
            ddlVoucher.CssClass = "nonEditabletxtbox";
            if (ddlVoucher.Items.Count > 0)
            {
                ddlVoucher.SelectedIndex = 0;
            }

            ddlFacilitywithDOS.Enabled = true;
            ddlFacilitywithDOS.CssClass = "Editabletxtbox";

            if (ddlFacilitywithDOS.Items.Count > 0)
            {
                cboMethodOfPayment.CssClass = "Editabletxtbox";
                cboMethodOfPayment.SelectedIndex = 0;
                cboMethodOfPayment.Enabled = true;
            }
            else
            {
                PaymentInformationClearAll();
                cboMethodOfPayment.SelectedIndex = 0;
                cboMethodOfPayment.CssClass = "nonEditabletxtbox";
                cboMethodOfPayment.Enabled = false;
                paymentinformationdisableall();
                btnAdd.Enabled = false;
            }

            if (ddlFacilitywithDOS.Items.Count > 1)
            {
                ddlFacilitywithDOS.SelectedIndex = 1;
                ddlFacilityWithDOS_SelectedIndexChanged(sender, e);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void rdbVoucher_CheckedChanged(object sender, EventArgs e)
        {
            ClearGrid();
            rdbFacilityName.Checked = false;
            ddlFacilitywithDOS.Enabled = false;
            ddlFacilitywithDOS.CssClass = "nonEditabletxtbox";
            if (ddlFacilitywithDOS.Items.Count > 0)
            {
                ddlFacilitywithDOS.SelectedIndex = 0;
            }

            ddlVoucher.Enabled = true;
            ddlVoucher.CssClass = "Editabletxtbox";
            ddlVoucher.Items.Clear();

            hdnEncounterID.Value = "0";
            hdnVoucherNo.Value = "0";
            cboMethodOfPayment.Enabled = false;
            cboMethodOfPayment.CssClass = "nonEditabletxtbox";
            VisitPaymentManager objVoucherNo = new VisitPaymentManager();
            ArrayList listVoucherNo = objVoucherNo.GetAutoIncreamentVoucherNo(humanID, ClientSession.FacilityName);
            ddlVoucher.Items.Add("");
            if (listVoucherNo != null && listVoucherNo.Count > 0)
            {

                for (int iCount = 0; iCount < listVoucherNo.Count; iCount++)
                {
                    System.Web.UI.WebControls.ListItem ddlItem = new System.Web.UI.WebControls.ListItem();
                    ddlItem.Value = listVoucherNo[iCount].ToString();
                    ddlVoucher.Items.Add(ddlItem);
                }
            }
            else
            {
                PaymentInformationClearAll();
                cboMethodOfPayment.SelectedIndex = 0;
                cboMethodOfPayment.CssClass = "nonEditabletxtbox";
                cboMethodOfPayment.Enabled = false;
                paymentinformationdisableall();
                btnAdd.Enabled = false;
            }

            if (ddlVoucher.Items.Count > 1)
            {
                ddlVoucher.SelectedIndex = 1;
                ddlVoucher_SelectedIndexChanged(sender, e);
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        [WebMethod(EnableSession = true)]
        public static string OpenFinancialReport(string strPatient, string strParameter, string strFromDate, string strToDate, string strPatientID, string strFacility)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string[] conString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ToString().Split(';');
            string sProjectType = System.Configuration.ConfigurationManager.AppSettings["ProjectName"].ToString();
            string sBIRTReportUrl = string.Empty;
            sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl_" + ClientSession.LegalOrg].ToString() + "CAPELLA_" + ClientSession.LegalOrg + "_" + "PATIENT_FINANCIAL_STATEMENT_REPORT" + ".rptdesign";

            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            string sDataBase = string.Empty;
            string sDataSource = string.Empty;
            string sUserId = string.Empty;
            string sPassword = string.Empty;
            string sPort = "3306";
            for (int i = 0; i < conString.Length; i++)
            {
                if (conString[i].ToString().ToUpper().Contains("DATABASE=") == true)
                {
                    sDataBase = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("DATA SOURCE") == true)
                {
                    sDataSource = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("USER ID") == true)
                {
                    sUserId = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PASSWORD") == true)
                {
                    sPassword = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PORT") == true)
                {
                    sPort = conString[i].ToString().Split('=')[1];
                }
            }
            //string sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;
            string sodaURL = string.Empty;
            string sAzure = System.Configuration.ConfigurationManager.AppSettings["Azure"].ToString();
            if (sAzure == "Y")
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase + "?useSSL=true&requireSSL=false";
            else
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;
            string sodaUser = sUserId;
            string sodaPassword = sPassword;
            string sDBConnection = "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword;

            string strPath = string.Empty;
            string strEncounterId = ClientSession.EncounterId.ToString();
            List<PatientPane> PatientPaneList = ClientSession.PatientPaneList.Where(a => a.Encounter_ID == ClientSession.EncounterId).ToList<PatientPane>();

            strPath = sBIRTReportUrl + "&ReportName=" + "PATIENT TRANSACTIONS" + "&PatientName=" + strPatient + "&Parameters=" + strParameter + "&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&PatientID=" + strPatientID + "&FacilityName=" + strFacility + "&legal_org=" + ClientSession.LegalOrg + "&Arc=" + "Y" + "&__title=" + "PATIENT TRANSACTIONS";
           // var result = new { BIRTUrl = sBIRTReportUrl, DBConnection = sDBConnection };
            var result = new { BIRTUrl = strPath, DBConnection = sDBConnection };
            return JsonConvert.SerializeObject(result);

        }

        //protected void txtTotalAmount_TextChanged(object sender, EventArgs e)
        //{
        //    txtTotalAmount.Text = string.Empty;
        //}

    }
    public static class DBConnector
    {
        static MySqlDataAdapter MyDataAdap = null;
        private static string ReadConnection()
        {
            string ConnectionData;
            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            return ConnectionData;
        }
        public static DataSet ReadData(string Query)
        {
            DataSet dsReturn = new DataSet();
            MyDataAdap = new MySqlDataAdapter(Query, ReadConnection());
            MyDataAdap.Fill(dsReturn);
            return dsReturn;
        }

        public static int WriteData(string Query)
        {
            int iReturn = 0;
            using (MySqlConnection con = new MySqlConnection(ReadConnection()))
            {
                using (MySqlCommand cmd = new MySqlCommand(Query))
                {
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        iReturn = 1;
                    }
                    catch (Exception ex)
                    {
                        iReturn = 2;
                    }
                }
            }
            return iReturn;
        }
    }
}