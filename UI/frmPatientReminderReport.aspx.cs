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
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using System.Net.Mail;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmPatientReminderReport : System.Web.UI.Page
    {
        string strRule = string.Empty;

        object misValue = System.Reflection.Missing.Value;
        IList<StaticLookup> defValues;
        iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        IList<FacilityLibrary> facilityList;
        FacilityLibrary objFacility;
        PhysicianManager phyMngr = new PhysicianManager();
        string sFreq = string.Empty;
        string sAlert = string.Empty;
        DateTime dtLast = DateTime.MinValue;
        StaticLookupManager staticMngr = new StaticLookupManager();
        FacilityManager facMngr = new FacilityManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //grdPatientReminder.da
              //  grdPatientReminder.DataSource = new[] {""};
                IList<StaticLookup> iFieldLookup = new List<StaticLookup>();
                StaticLookupManager objLookupManager = new StaticLookupManager();

                iFieldLookup = objLookupManager.getStaticLookupByFieldName("COMMUNICATION TYPE");
                for (int c = 0; c < iFieldLookup.Count; c++)
                {
                    cbocommunicat.Items.Add(new RadComboBoxItem(iFieldLookup[c].Value));
                }
               
           



                FillPhysicianUser objPhysicianUser = null;
                objPhysicianUser = phyMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                if (objPhysicianUser.PhyList != null)
                {
                 
                    for (int i = 0; i < objPhysicianUser.PhyList.Count; i++)
                    {
                        cboPhysicianName.Items.Add(new RadComboBoxItem(objPhysicianUser.PhyList[i].PhyPrefix + " " + objPhysicianUser.PhyList[i].PhyFirstName + " " + objPhysicianUser.PhyList[i].PhyMiddleName + " " + objPhysicianUser.PhyList[i].PhyLastName + " " + objPhysicianUser.PhyList[i].PhySuffix));
                        cboPhysicianName.Items[i].Value = objPhysicianUser.PhyList[i].Id.ToString();
                    }
                }

                cboRule.Items.Clear();
                cboRule.Items.Add(new System.Web.UI.WebControls.ListItem(""));

                RuleMasterManager objRuleMasterManager = new RuleMasterManager();

                IList<RuleMaster> ruleList = objRuleMasterManager.GetRuleName(ClientSession.LegalOrg);
              
                if(ruleList != null)
                {
                    for (int i = 0; i < ruleList.Count; i++)
                    {
                        System.Web.UI.WebControls.ListItem tempItem = new System.Web.UI.WebControls.ListItem();
                        tempItem.Text = ruleList[i].Rule_Name.ToString();
                        cboRule.Items.Add(tempItem);
                        cboRule.Items[i+1].Value = ruleList[i].Id.ToString();

                    }
                }


                defValues = staticMngr.getStaticLookupByFieldName("PATIENT REMAINDER LETTER TEXT");
                facilityList = facMngr.GetFacilityList();
                try
                {
                    objFacility = (from obj in facilityList where obj.Fac_Name == ClientSession.FacilityName select obj).ToList<FacilityLibrary>()[0];
                }
                catch
                {
                    objFacility = null;
                }
                //CAP-3698
                if (grdPatientReminder.DataSource == null)
                {
                    grdPatientReminder.DataSource = new string[] { };
                    grdPatientReminder.DataBind();
                }
            }
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            //if (txtRuleName.Text == string.Empty)
            if (cboRule.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420000');", true);

                return;
            }
            if (cboPhysicianName.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420001');", true);
                return;
            }

            DataSet dsRule = new DataSet();
            UtilityManager uti = new UtilityManager();
            RuleMasterManager RuleMngr = new RuleMasterManager();
            //dsRule = RuleMngr.GetPatientRemainderForRuleId(Convert.ToUInt64(txtRuleName.Attributes["Tag"]), Convert.ToInt32(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value), UtilityManager.ConvertToUniversal());
            dsRule = RuleMngr.GetPatientRemainderForRuleId(Convert.ToUInt64(cboRule.Items[cboRule.SelectedIndex].Value), Convert.ToInt32(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value), UtilityManager.ConvertToUniversal());
            if (dsRule.Tables[0].Rows.Count > 0)
            {
                DataTable temptable = dsRule.Tables[0].Rows.OfType<DataRow>().Skip((mpnHospitalizationHistory.PageNumber - 1) * mpnHospitalizationHistory.MaxResultPerPage).Take(mpnHospitalizationHistory.MaxResultPerPage).CopyToDataTable();
                // dsRule = new DataSet();
                dsRule.Tables.Add(temptable);

                //CAP-3698
                //grdPatientReminder.DataSource = dsRule.Tables[1];

                if (dsRule.Tables.Count > 1)
                {
                    if (dsRule.Tables[1].Rows.Count != 0)
                    {
                        grdPatientReminder.DataSource = dsRule.Tables[1];
                    }
                    else
                        grdPatientReminder.DataSource = new string[] { };
                }
                else
                    grdPatientReminder.DataSource = new string[] { };
                mpnHospitalizationHistory.TotalNoofDBRecords = dsRule.Tables[0].Rows.Count;
                grdPatientReminder.DataBind();
                lblCount.Visible = true;

                Session["dt"] = dsRule.Tables[0];
            }
            lblCount.Text = dsRule.Tables[0].Rows.Count + "  Result(s) Found   ";
        
            if (grdPatientReminder.Items.Count > 0)
            {
                btnPrintPdf.Enabled = true;
                btnPrintExcel.Enabled = true;
                btnPrintinEnglish.Enabled = true;
            }
            divLoading.Style.Add("display", "none");
        }
        public void FirstPageNavigator(object sender, EventArgs e)
        {

            try
            {
                DataSet dsRule = new DataSet();
                UtilityManager uti = new UtilityManager();
                RuleMasterManager RuleMngr = new RuleMasterManager();
                //dsRule = RuleMngr.GetPatientRemainderForRuleId(Convert.ToUInt64(txtRuleName.Attributes["Tag"]), Convert.ToInt32(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value), UtilityManager.ConvertToUniversal());
                dsRule = RuleMngr.GetPatientRemainderForRuleId(Convert.ToUInt64(cboRule.Items[cboRule.SelectedIndex].Value), Convert.ToInt32(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value), UtilityManager.ConvertToUniversal());

                DataTable temptable = dsRule.Tables[0].Rows.OfType<DataRow>().Skip((mpnHospitalizationHistory.PageNumber - 1) * mpnHospitalizationHistory.MaxResultPerPage).Take(mpnHospitalizationHistory.MaxResultPerPage).CopyToDataTable();
                // dsRule = new DataSet();
                dsRule.Tables.Add(temptable);

                grdPatientReminder.DataSource = dsRule.Tables[1];
                mpnHospitalizationHistory.TotalNoofDBRecords = dsRule.Tables[0].Rows.Count;
                grdPatientReminder.DataBind();
                lblCount.Visible = true;

                Session["dt"] = dsRule.Tables[0];
                lblCount.Text = dsRule.Tables[0].Rows.Count + "  Result(s) Found   ";
            }
            catch
            {

            }

        }
        protected void btnRule_Click(object sender, EventArgs e)
        {
            if (hdnRule.Value.ToString() != string.Empty)
            {
                string[] splitRule = hdnRule.Value.Split('|');
               // txtRuleName.Text = splitRule[1];
               // txtRuleName.Attributes.Add("Tag", splitRule[0]);
                txtRuleDescription.Text = splitRule[2];
                if (Convert.ToDateTime(splitRule[3]).ToString("dd-MMM-yyyy") == DateTime.MinValue.ToString("dd-MMM-yyyy"))
                    txtLastRunDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                else
                    txtLastRunDate.Text = Convert.ToDateTime(splitRule[3]).ToString("dd-MMM-yyyy");
                txtActionNeeded.Text = splitRule[4];
                sFreq = splitRule[5];
                sAlert = splitRule[6];
                dtLast = Convert.ToDateTime(splitRule[3]);
                hdnRuleID.Value = splitRule[0];
            }
        }

        protected void btnPrintinEnglish_Click(object sender, EventArgs e)
        {
            SelectedItem.Value = string.Empty;
            if (grdPatientReminder.SelectedItems.Count > 0)
            {

                if (defValues != null)
                {
                    string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                    DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                    if (!ObjSearchDir.Exists)
                    {
                        ObjSearchDir.Create();
                    }

                    string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);
                    string initialText = defValues[0].Value;
                    string letterText = string.Empty;

                    string PersonName = string.Empty;
                    //CAP-2788
                    UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
                    if (ilstUserList?.User != null)
                    {
                        var filteredData = ilstUserList?.User.FirstOrDefault(a => a.User_Name == ClientSession.UserName);
                        if (filteredData != null)
                        { PersonName = filteredData.person_name; }
                    }


                    letterText = initialText.Replace("<Action>", txtActionNeeded.Text);
                    letterText = letterText.Replace("<Last Name>", grdPatientReminder.SelectedItems[0].Cells[4].Text.ToString());
                    letterText = letterText.Replace("<Person Name>", PersonName);
                    if (objFacility != null)
                        letterText = letterText.Replace("<Tel Number>", objFacility.Fac_Telephone);
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
                    string sPrintPathName = string.Empty;
                    string folderPath = TargetFileDirectory + System.Configuration.ConfigurationSettings.AppSettings["PatientRemainderPrintPath"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(folderPath);
                    HumanManager humanMngr = new HumanManager();
                    Human objHuman = humanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(grdPatientReminder.SelectedItems[0].Cells[2].Text))[0];
                    if (objHuman != null)
                        sPrintPathName = folderPath + "\\Patient_Remainder_Letter_" + objHuman.Id.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd hh mm ss tt") + ".pdf";
                    else
                        sPrintPathName = folderPath + "\\Patient_Remainder_Letter_" + DateTime.Now.ToString("yyyyMMdd hh mm ss tt") + ".pdf";
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    Acurus.Capella.UI.PrintOrders.HeaderEventGenerate headerEvent = new Acurus.Capella.UI.PrintOrders.HeaderEventGenerate();
                    doc.Open();
                    wr.PageEvent = headerEvent;
                    headerEvent.OnStartPage(wr, doc);
                    headerEvent.OnEndPage(wr, doc);
                    Paragraph par = new Paragraph(System.Configuration.ConfigurationSettings.AppSettings["PatientRemainderLetterHeaderText"], reducedFont);
                    par.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    doc.Add(par);
                    par = new Paragraph(objFacility.Fac_Name, normalFont);
                    par.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    doc.Add(par);
                    par = new Paragraph(objFacility.Fac_Address1 + "," + objFacility.Fac_City + " " + objFacility.Fac_State + " " + objFacility.Fac_Zip, normalFont);
                    par.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    doc.Add(par);
                    doc.Add(new Paragraph("\n"));

                    string DearText = "Dear ";
                    if (objHuman != null)
                    {
                        switch (objHuman.Sex.ToUpper())
                        {
                            case "MALE":
                                DearText += "Mr. " + objHuman.Last_Name;
                                break;
                            case "FEMALE":
                                DearText += "Ms. " + objHuman.Last_Name;
                                break;

                        }
                    }
                    doc.Add(new Paragraph(DearText, normalFont));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph(letterText, normalFont));
                    doc.Close();
                    // System.Diagnostics.Process.Start(sPrintPathName);
                    string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                    string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    if (SelectedItem.Value == string.Empty)
                    {
                        SelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                    }
                    else
                    {
                        SelectedItem.Value += "|" + FileName[0].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420003');", true);

                return;
            }

        }

        protected void btnPrintPdf_Click(object sender, EventArgs e)
        {
            SelectedItem.Value = string.Empty;
            if (grdPatientReminder.Items.Count > 0)
            {
                strRule = hdnRule.Value.ToString();
                if (hdnRule.Value != string.Empty)
                {
                    string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

                    DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

                    if (!ObjSearchDir.Exists)
                    {
                        ObjSearchDir.Create();
                    }

                    string TargetFileDirectory = Server.MapPath("Documents\\" + Session.SessionID);
                    string strDate = string.Empty;
                    string[] splitRule = strRule.Split('|');
                    if (Convert.ToDateTime(splitRule[3]) == DateTime.MinValue)
                        strDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    else
                        strDate = Convert.ToDateTime(splitRule[3]).ToString("dd-MMM-yyyy");


                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 25, 25, 25, 25);
                    string sPrintPathName = string.Empty;
                    string folderPath = TargetFileDirectory + System.Configuration.ConfigurationSettings.AppSettings["PatientRemainderPrintPath"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(folderPath);
                    sPrintPathName = folderPath + "\\Patient_Remainder_Report_" + DateTime.Now.ToString("yyyyMMdd hh mm ss tt") + ".pdf";
                    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    Acurus.Capella.UI.PrintOrders.HeaderEventGenerate headerEvent = new Acurus.Capella.UI.PrintOrders.HeaderEventGenerate();
                    doc.Open();
                    wr.PageEvent = headerEvent;
                    headerEvent.OnStartPage(wr, doc);
                    headerEvent.OnEndPage(wr, doc);
                    Paragraph par = new Paragraph(System.Configuration.ConfigurationSettings.AppSettings["PatientRemainderLetterHeaderText"], reducedFont);
                    par.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    doc.Add(par);
                    par = new Paragraph(objFacility.Fac_Name, normalFont);
                    par.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    doc.Add(par);
                    par = new Paragraph(objFacility.Fac_Address1 + "," + objFacility.Fac_City + " " + objFacility.Fac_State + " " + objFacility.Fac_Zip, normalFont);
                    par.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    doc.Add(par);
                    doc.Add(new Paragraph("\n"));
                    PdfPTable table = new PdfPTable(new float[] { 15, 30, 15, 40 });
                    table.WidthPercentage = 100f;
                    PdfPCell cell = new PdfPCell(new Phrase("Rule Name:", reducedFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(splitRule[1], normalFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Description:", reducedFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(splitRule[2], normalFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    //doc.Add(new Paragraph("\n"));

                    table = new PdfPTable(new float[] { 15, 30, 15, 40 });
                    table.WidthPercentage = 100f;
                    cell = new PdfPCell(new Phrase("Action Needed:", reducedFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(splitRule[4], normalFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Physician Name:", reducedFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(cboPhysicianName.Text, normalFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    //doc.Add(new Paragraph("\n"));

                    table = new PdfPTable(new float[] { 15, 30, 15, 40 });
                    table.WidthPercentage = 100f;
                    cell = new PdfPCell(new Phrase("Report Run Date:", reducedFont));
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(strDate, normalFont));
                    cell.Colspan = 3;
                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(cell);
                    doc.Add(table);
                    doc.Add(new Paragraph("\n"));

                    table = new PdfPTable(new float[] { 10, 20, 15, 15, 15, 10, 10, 10, 25 });
                    table.WidthPercentage = 100f;
                    cell = new PdfPCell(new Phrase("Account #", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Pateint Name", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Date of Birth", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Commn. Mode", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Preferred Language", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Home Phone #", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Cell Phone #", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Work Phone #", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("E Mail", reducedFont));
                    cell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_CENTER;
                    table.AddCell(cell);


                    for (int i = 0; i < grdPatientReminder.Items.Count; i++)
                    {
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[2].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[3].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(Convert.ToDateTime(grdPatientReminder.Items[i].Cells[4].Text).ToString("dd-MMM-yyyy"), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[5].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[6].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[7].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[8].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[9].Text.ToString(), normalFont));
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(grdPatientReminder.Items[i].Cells[10].Text.ToString(), normalFont));
                        table.AddCell(cell);
                    }

                    doc.Add(table);
                    doc.Close();
                    string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                    string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    if (SelectedItem.Value == string.Empty)
                    {
                        SelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                    }
                    else
                    {
                        SelectedItem.Value += "|" + FileName[0].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);
                    // System.Diagnostics.Process.Start(sPrintPathName);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420004');", true);

                return;
            }
        }

        protected void btnPrintExcel_Click(object sender, EventArgs e)
        {
            DataView dv = new DataView();
            DataSet dsreport = new DataSet();
            DataTable dtResult = new DataTable();
            dtResult = (System.Data.DataTable)Session["dt"];

            //DataTable tempTab = new DataTable();
            //for (int x = 0; x < dtResult.Rows.Count; x++)
            //{
            //    tempTab.Rows.Add(dtResult.Rows[x]);
            //}

            if (dtResult == null)
            {
                return;
            }
            dsreport = new DataSet();

            System.Data.DataTable dtNew = new System.Data.DataTable();


            dtNew = dtResult.Copy();

            dsreport.Tables.Add(dtNew);

            dv = new DataView(dsreport.Tables[0]);


            if (dsreport.Tables[0].Rows.Count > 0)
            {
                string filename = "Patient_Reminder_Report_" + DateTime.Now.ToString("yyyyMMdd hh mm ss tt") + ".xls";

                Response.Charset = "UTF-8";
                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                StringBuilder sResponseMessage = new StringBuilder();
                sResponseMessage.Append("<table  border=\"1\" width=\"50%\">");
                int col;
                sResponseMessage.Append("<tr align=\"center\" >");

                sResponseMessage.Append("<td colspan=\"8\" align=\"center\"><strong>" + "PATIENT REMINDER REPORT" + "</strong></td>");
                sResponseMessage.Append("<tr align=\"center\" >");
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\"><strong>" + " Rule Name : " + "</strong></td>");
                //sResponseMessage.Append("<td colspan=\"1\" align=\"left\">" + txtRuleName.Text);
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\">" + cboRule.SelectedItem.ToString());
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\"><strong>" + " Description  : " + "</strong></td>");
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\">" + txtRuleDescription.Text);
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\"><strong>" + " Action Needed  : " + "</strong></td>");
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\">" + txtActionNeeded.Text);
                //sResponseMessage.Append("<td colspan=\"1\" align=\"left\"><strong>" + " Physician Name  : " + "</strong></td>");
                //sResponseMessage.Append("<td colspan=\"1\" align=\"left\">" + cboPhysicianName.Text);
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\"><strong>" + " Report Run Date  : " + "</strong></td>");
                sResponseMessage.Append("<td colspan=\"1\" align=\"left\">" + txtLastRunDate.Text);
                sResponseMessage.Append("<tr align=\"center\" >");


                for (col = 0; col < dv.Table.Columns.Count; col++)
                {
                    if (grdPatientReminder.Columns[col].Display == true)
                    {

                        sResponseMessage.Append("<td width=\"20%\"><strong>" + dv.Table.Columns[col].ColumnName + "</strong></td>");

                    }
                }
                sResponseMessage.Append("</tr>");
                foreach (DataRowView drv in dv)
                {

                    sResponseMessage.Append("<tr>");
                    for (col = 0; col < dv.Table.Columns.Count; col++)
                    {

                        sResponseMessage.Append("<td width='50%'>" + drv[col].ToString() + "</td>");
                    }
                    sResponseMessage.Append("</tr>");

                }
                sResponseMessage.Append("</table>");
                Response.Write(sResponseMessage);
                Response.Flush();
                Response.End();
            }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
           
            
             if (cbocommunicat.Text.ToUpper() == "EMAIL")
            {
            

            HumanManager objhum=new HumanManager();

            bool DECLINED = false;
            string to_address = string.Empty;
            string Mail = "False";
            bool Pat = false;
            int CheckedCount = 0;
            int EmailCount = 0;
            bool IsMessage = false;
            string lstHumanId = string.Empty;
            IList<string> lstMail = new List<string>();
            for (int i = 0; i < grdPatientReminder.Items.Count; i++)
            {

                CheckBox chkBox = (CheckBox)grdPatientReminder.Items[i].FindControl("Primary");
                if (chkBox.Checked)
                {
                    CheckedCount++;
                    if (grdPatientReminder.Items[i].Cells[6].Text.ToUpper() != "DECLINED TO SPECIFY")
                    {

                        Human obj = objhum.GetById(Convert.ToUInt32(grdPatientReminder.Items[i].Cells[3].Text));

                        if (obj.EMail.Trim() != string.Empty && obj.EMail.Contains("@"))
                        {
                            EmailCount++;
                            to_address += obj.EMail + ";";

                            if (lstHumanId == string.Empty)
                                lstHumanId = grdPatientReminder.Items[i].Cells[3].Text;
                            else
                                lstHumanId += "|" + grdPatientReminder.Items[i].Cells[3].Text;

                            lstMail.Add("EMAIL");
                        }



                    }
                    else
                    {
                        DECLINED = true;
                    }
                    Pat = true;
                }

            }
            if (EmailCount == CheckedCount)
            {
                IsMessage = true;
            }
               
           IList<string> lstcount= lstMail.Where(a=>a.ToUpper()=="EMAIL").ToList<string>();

           if (lstcount.Count == 0)
           {
               Mail = "True";
           }
           if (Pat)
           {
               ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "SendMsg(" + "'" + to_address + "'" + "," + "'" + txtRuleDescription.Text + "'" + "," + "'" + Mail + "'" + "," + "'" + lstHumanId + "'" + "," + "'" + CheckedCount + "'" + "," + "'" + IsMessage + "'" + ");", true);
           }
           else
           {
               ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420006');", true);
           }



            if (DECLINED)
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420008');", true);


             }
            
             else
             {
                 IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                 ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                 ActivityLog activity = null;
                 bool  Pat = false;

                 bool DECLINED = false;
                
                 for (int i = 0; i < grdPatientReminder.Items.Count; i++)
                 {

                     CheckBox chkBox = (CheckBox)grdPatientReminder.Items[i].FindControl("Primary");
                     if (chkBox.Checked)
                     {
                         Pat = true;
                         if (grdPatientReminder.Items[i].Cells[6].Text.ToUpper() != "DECLINED TO SPECIFY")
                         {
                            
                             activity = new ActivityLog();

                             //   if (grdPatientReminder.Items[i].Cells[6].Text.ToUpper() == "DELICINED TO SPECIFY" || grdPatientReminder.Items[i].Cells[6].Text.ToUpper().Trim() == "&NBSP;")
                             // activity.Activity_Type = "Phone";
                             // else
                             activity.Activity_Type = cbocommunicat.Text.ToUpper();

                             activity.Human_ID = Convert.ToUInt32(grdPatientReminder.Items[i].Cells[3].Text);
                             if (activity.Human_ID == ClientSession.HumanId)
                                 activity.Encounter_ID = ClientSession.EncounterId;//BugID:44920,44718
                             activity.Subject = "";
                             activity.Message = "";
                             activity.Role = "";
                             //activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                             activity.Activity_Date_And_Time = DateTime.Now; ;
                             ActivityLogList.Add(activity);
                         }
                         else
                         {
                             DECLINED = true;
                         }

                     }
                 }

                 if (!Pat)
                     ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420006');", true);

                 if(DECLINED)
                     ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7420008');", true);


                 if (ActivityLogList.Count > 0)
                 {
                     ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
                     ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7410002');", true);
                 }
             }
        
        }

        protected void cboRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdnRule.Value.ToString() != string.Empty)
            {
                int id = Convert.ToInt16(hdnRule.Value.ToString());
                RuleMasterManager objRuleMasterManager = new RuleMasterManager();

                IList<RuleMaster> ruleList = objRuleMasterManager.GetRuleName(id,ClientSession.LegalOrg);               
                if (ruleList.Count > 0)
                {
                    txtRuleDescription.Text = ruleList[0].Rule_Description;
                    if (Convert.ToDateTime(ruleList[0].Last_Run_Date).ToString("dd-MMM-yyyy") == DateTime.MinValue.ToString("dd-MMM-yyyy"))
                        txtLastRunDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    else
                        txtLastRunDate.Text = Convert.ToDateTime(ruleList[0].Last_Run_Date).ToString("dd-MMM-yyyy");
                    txtActionNeeded.Text = ruleList[0].Expected_Action;
                    sFreq = ruleList[0].Frequency.ToString();
                    sAlert = ruleList[0].Alert.ToString();
                    dtLast = Convert.ToDateTime(ruleList[0].Last_Run_Date);
                    hdnRuleID.Value = ruleList[0].Id.ToString();
                }

            }
        }
    }
}
