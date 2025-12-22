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
using Acurus.Capella.Core.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.Services;
using System.Text.RegularExpressions;

using EMRDirect.phiMail;

namespace Acurus.Capella.PatientPortal
{
    public partial class frmClinicalSummary : System.Web.UI.Page
    {
        Human hn = null;
        IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        Encounter Enc = null;
        ChiefComplaintsManager ClinicalMngr = new ChiefComplaintsManager();
        FillClinicalSummary WellnessNotes = null;
        InsurancePlanManager InsurancePlanMngr = new InsurancePlanManager();
        PhysicianManager physicianMngr = new PhysicianManager();
        ActivityLog activity = new ActivityLog();
        IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
        ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
        iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
        iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        protected void Page_Load(object sender, EventArgs e)
        {
            //DLCRecAdd.DName = "pbDropdown";
            if (!IsPostBack)
            {
                chkCheckAll.Checked = true;
                for (int i = 0; i < pnlEncounterDetails.Controls.Count; i++)
                {
                    if (pnlEncounterDetails.Controls[i] is CheckBox)
                    {
                        ((CheckBox)pnlEncounterDetails.Controls[i]).Checked = true;
                    }

                }
            }
            //DLCRecAdd.txtDLC.Attributes.Add("KeyFrom", "EMAIL");
            //DLCRecAdd.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            //DLCRecAdd.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            hdnXmlPath.Value = string.Empty;
            string sMyPath = string.Empty;
            if (ClientSession.UserName != string.Empty)
            {


                ArrayList FileLocation = PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, true, ref sMyPath, string.Empty, false, false);

                hdnPrintFilePath.Value = string.Empty;
                hdnXmlPath.Value = string.Empty;
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                //if (FileLocation[1].ToString().EndsWith(".pdf") == true)
                //{
                //    string sPrintPathName = FileLocation[1].ToString();

                //    string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                //    if (hdnPrintFilePath.Value == string.Empty)
                //    {
                //        hdnPrintFilePath.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                //    }
                //    else
                //    {
                //        hdnPrintFilePath.Value += "|" + FileName[0].ToString();
                //    }
                //}
                if (FileLocation != null && FileLocation.Count != 0)
                {
                    if (FileLocation[0].ToString().EndsWith(".xml") == true)
                    {

                        if (hdnXmlPath.Value != null && hdnXmlPath.Value == string.Empty)
                        {
                            string[] XMLFileName = FileLocation[0].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);
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


                if (hdnXmlPath.Value != null && hdnXmlPath.Value != string.Empty)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);

                if (hdnXmlPath.Value != null && hdnXmlPath.Value != string.Empty)
                {
                    // lblAttachment.Visible = true;
                    lblAttachment.Text = hdnXmlPath.Value;
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "StopLoad();", true);
            }
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "GENERATE - CCD";
            alManager.InsertIntoAuditLog("EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
        }
        public void OpenXML(string XmLLocation)
        {
            try
            {
                //string XMLPath = hdnXmlPath.Value.Replace(".xml", "_New.xml");
                //string[] index = XMLPath.Split('\\');
                //string name = index[index.Length - 1];
                //Response.Clear();
                //Response.ContentType = "text/xml";
                //Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}", name));
                //Response.TransmitFile(Server.MapPath(hdnXmlPath.Value.Replace(".xml", "_New.xml")));

                //Response.End();


                if (hdnXmlPath.Value != null)
                {
                    //Audit Log entry for Export.
                    AuditLogManager alManager = new AuditLogManager();
                    alManager.InsertIntoAuditLog("Clinical Exchange", "COPY", Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
                    string XMLPath = hdnXmlPath.Value;
                    Response.Clear();
                    Response.ContentType = "Application/xml";
                    //Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + XMLPath);
                    Response.TransmitFile(Server.MapPath(XMLPath));
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                //  HttpContext.Current.Response.Write(ex.Message);
            }
        }

        public string GenerateCCD(uint EncID, ulong HumanID)
        {
            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            Directory.CreateDirectory(sFolderPathName);

            string sPrintPathName = string.Empty;

            sPrintPathName = sFolderPathName + "\\" + "Clinical_Summary_" + HumanID.ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
            //CAP-4030
            //string sCheckedItems = "Reason Of Visit,Vitals,Clinical Instruction,Immunizations,Mental Status,Care Plan,Laboratory Test(s),Smoking Status,Allergy,Functional Status,Procedure(s),Laboratory Values/Results,Encounter,Goals,Assessment,Medication,Medications Administered During visit,Treatment Plan,Problem List,Reason for Referral,Implants,Future Appointment,Health Concern,Lab Test,Laboratory Information,Diagnostics Tests Pending,Future Scheduled Tests,Patient Decision Aids, Social History";
            string sCheckedItems = "Reason Of Visit,Vitals,Clinical Instruction,Immunizations,Mental Status,Care Plan,Laboratory Test(s),Smoking Status,Allergy,Functional Status,Procedure(s),Laboratory Values/Results,Encounter,Goals,Assessment,Medication,Medications Administered During visit,Treatment Plan,Problem List,Reason for Referral,Implants,Future Appointment,Health Concern,Lab Test,Laboratory Information,Diagnostics Tests Pending,Future Scheduled Tests,Patient Decision Aids,Payer";

            string sStatus = UtilityManager.GenerateCCD(HumanID, EncID, sCheckedItems, sPrintPathName, string.Empty);
            //if (sStatus == "Success")
            //{
            //    //string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            //    //string[] XMLFileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            //    //if (hdnXmlPath.Value == string.Empty || hdnXmlPath.Value == null )
            //    //{
            //    //    hdnXmlPath.Value = "Documents\\" + Session.SessionID.ToString() + XMLFileName[0].ToString();
            //    //}
            //    //if (hdnXmlPath.Value != null && hdnXmlPath.Value != string.Empty)
            //    //{
            //    //    DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(hdnXmlPath.Value));
            //    //    if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
            //    //    {
            //    //        Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
            //    //    }
            //    //    System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath("Documents/" + Session.SessionID.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl"), true);
            //    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenAltovaPDF();", true);

            //    //    //AuditLogManager alManager = new AuditLogManager();
            //    //    //string TransactionType = "GENERATE - CCD";
            //    //    //alManager.InsertIntoAuditLog("EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
            //    //}
            //    //else
            //    //{
            //    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenErrorAltova();", true);
            //    //    return sStatus;
            //    //}
            //    sPrintPathName = sPrintPathName + "$" + sStatus;
            //}
            //else if (sStatus == "1011192")
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenWarningAltova();", true);
            //    return sPrintPathName + "$" + sStatus;
            //}
            //else
            //{
            //   // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenErrorAltova();", true);
            //    return sPrintPathName + "$" + sStatus;
            //}
            return sPrintPathName + "$" + sStatus;
        }
        public ArrayList PrintClinicalSummary(ulong ulEncounterId, ulong ulHumanId, Boolean bOpen, ref string sMyPathName, string sFolderPathName, bool isExport, bool isPatientPortal)
        {
            ArrayList result = new ArrayList();

            if (ulEncounterId == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('110035');", true);
                return result;
            }

            //*************************************************************
            //To print the patient details
            WellnessNotes = ClinicalMngr.GetClinicalSummary(ulEncounterId, ulHumanId);
            //hn = EncounterManager.Instance.GetHumanByHumanID(ulHumanId);
            // Enc = EncounterManager.Instance.GetEncounterByEncID(ulEncounterId);
            if (WellnessNotes != null && WellnessNotes.Encounter[0] != null)
            {
                Enc = WellnessNotes.Encounter[0];
            }

            PhysicianList = WellnessNotes.phyList;

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            //doc.SetPageSize(new iTextSharp.text.Rectangle(800, 800));
            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }

            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            Directory.CreateDirectory(sFolderPathName);

            //To find the Primary Plan Name
            string PriPlan = string.Empty;
            IList<PatientInsuredPlan> PatInsList = new List<PatientInsuredPlan>();
            if (WellnessNotes != null && WellnessNotes.Pat_Ins_Plan != null)
                PatInsList = WellnessNotes.Pat_Ins_Plan;// Proxy.Util.EncounterManager.Instance.getPatientInsuredDetailsUsingPatHumanId(ulHumanId);
            for (int i = 0; i < PatInsList.Count; i++)
            {
                if (PatInsList[i].Insurance_Type.ToUpper() == "PRIMARY")
                {
                    IList<InsurancePlan> InsPlan = InsurancePlanMngr.GetInsurancebyID(PatInsList[i].Insurance_Plan_ID);
                    //PriPlan = InsPlan.Ins_Plan_Name;
                    PriPlan = InsPlan[0].External_Plan_Number;
                }
            }


            //if (sFolderPathName == string.Empty)
            //{
            //    sPrintPathName = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ClinicalSummaryPathName"] + "\\" + WellnessNotes.Last_Name + "_" + WellnessNotes.First_Name + "_" +
            //     PriPlan + "_" + Enc.Facility_Name + ".pdf";
            //}
            //else
            //{
            //    sPrintPathName = sFolderPathName + "\\" + WellnessNotes.Last_Name + "_" + WellnessNotes.First_Name + "_" +
            //     PriPlan + "_" + Enc.Facility_Name + ".pdf";
            //}
            string sPrintPathName = string.Empty;

            sPrintPathName = sFolderPathName + "\\" + WellnessNotes.Last_Name + "_" + WellnessNotes.First_Name + "_" +
                   PriPlan + "_" + Enc.Facility_Name.Replace("#", "") + "_" + ulHumanId + "_" + ulEncounterId + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";

            //*************************************************************HL7
            Hashtable hashCheckList = new Hashtable();

            if (isExport)
            {
                hashCheckList.Add("chkReasonOfVisit", true);
                hashCheckList.Add("chkCarePlan", true);
                hashCheckList.Add("chkProcedures", true);
                hashCheckList.Add("chkClinicalInstruction", true);
                hashCheckList.Add("chkSmokingStatus", true);
                hashCheckList.Add("chkLaboratoryResultValues", true);
                hashCheckList.Add("chkImmunization", true);
                hashCheckList.Add("chkAllergies", true);
                hashCheckList.Add("chkEncounter", true);
                hashCheckList.Add("chkMedicationAdministrative", true);
                hashCheckList.Add("chkMedication", true);
                hashCheckList.Add("chkReasonforReferral", true);
                hashCheckList.Add("chkProblemList", true);
                hashCheckList.Add("chkVitals", true);
                hashCheckList.Add("chkChiefComplaints", true);
                hashCheckList.Add("chkImplant", true);
                hashCheckList.Add("chkMentalStatus", true);
                hashCheckList.Add("chkFunctionalStatus", true);
                hashCheckList.Add("chkHealthConcern", true);
                hashCheckList.Add("chkTreatmentPlan", true);
                hashCheckList.Add("chkGoals", true);
                hashCheckList.Add("chkLabTest", true);
                hashCheckList.Add("chkLab", true);


            }
            else
            {
                for (int i = 0; i < pnlEncounterDetails.Controls.Count; i++)
                {
                    if (pnlEncounterDetails.Controls[i] is CheckBox)
                    {
                        hashCheckList.Add(((CheckBox)pnlEncounterDetails.Controls[i]).ID, ((CheckBox)pnlEncounterDetails.Controls[i]).Checked);
                    }

                }
            }

            UtilityManager umanger = new UtilityManager();
            string sValue = string.Empty;
            IList<CarePlan> cpFinalList = new List<CarePlan>();
            for (int i = 0; i < WellnessNotes.Care_Plan_Cognitive_Function_MentalStatus.Count; i++)
            {
                sValue = umanger.GetFieldNameForSnomedCodefromStaticLookup("FollowupList", WellnessNotes.Care_Plan_Cognitive_Function_MentalStatus[i].Snomed_Code);
                if (sValue != string.Empty)
                {
                    if (sValue.Contains(','))
                    {
                        string[] ccObject = (string[])sValue.Split(',');
                        foreach (string s in ccObject)
                        {
                            CarePlan obj = new CarePlan();
                            obj.Snomed_Code = s.Split('~')[0];
                            obj.Care_Name_Value = s.Split('~')[1];
                            obj.Plan_Date = WellnessNotes.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date;
                            cpFinalList.Add(obj);
                        }
                    }
                    else
                    {
                        CarePlan obj = new CarePlan();
                        obj.Snomed_Code = sValue.Split('~')[0];
                        obj.Care_Name_Value = sValue.Split('~')[1];
                        obj.Plan_Date = WellnessNotes.Care_Plan_Cognitive_Function_MentalStatus[i].Plan_Date;
                        cpFinalList.Add(obj);
                    }
                }
            }
            WellnessNotes.Care_Plan_Cognitive_Function_MentalStatus = cpFinalList;

            sValue = string.Empty;
            IList<CarePlan> cpFunctionalFinalList = new List<CarePlan>();
            for (int i = 0; i < WellnessNotes.Care_Plan_FunctionalStatus.Count; i++)
            {
                sValue = umanger.GetFieldNameForSnomedCodefromStaticLookup("FollowupList", WellnessNotes.Care_Plan_FunctionalStatus[i].Snomed_Code);
                if (sValue != string.Empty)
                {
                    if (sValue.Contains(','))
                    {
                        string[] ccObject = (string[])sValue.Split(',');
                        foreach (string s in ccObject)
                        {
                            CarePlan obj = new CarePlan();
                            obj.Snomed_Code = s.Split('~')[0];
                            obj.Care_Name_Value = s.Split('~')[1];
                            obj.Plan_Date = WellnessNotes.Care_Plan_FunctionalStatus[i].Plan_Date;
                            cpFunctionalFinalList.Add(obj);
                        }
                    }
                    else
                    {
                        CarePlan obj = new CarePlan();
                        obj.Snomed_Code = sValue.Split('~')[0];
                        obj.Care_Name_Value = sValue.Split('~')[1];
                        obj.Plan_Date = WellnessNotes.Care_Plan_FunctionalStatus[i].Plan_Date;
                        cpFunctionalFinalList.Add(obj);
                    }
                }
            }
            WellnessNotes.Care_Plan_FunctionalStatus = cpFunctionalFinalList;
            if(PhysicianList.Count==0)
            {
                PhysicianLibrary obj = new PhysicianLibrary();
                obj.PhyFirstName = "Acurus";
                obj.PhyLastName = " Capella EHR v5.4";
                PhysicianList.Add(obj);
            }

            HL7Generator hl7Gen = new HL7Generator();
            XmlDocument xmlDoc = hl7Gen.CreateCCDXML(PhysicianList[0], WellnessNotes, sPrintPathName.Replace(".pdf", ".xml"), hashCheckList);
            result.Add(sPrintPathName.Replace(".pdf", ".xml"));

            //for pdf
            object sPdf;
            //frmSummaryOfCare objSummaryofcare = new frmSummaryOfCare();

            //string sResult = objSummaryofcare.PrintPDF(sPrintPathName.Replace(".pdf", ".xml"), "ClinicalSummary");

            //result.Add(sResult);


            //if (isPatientPortal == false)
            //    result = ImportCCD(xmlDoc, sPrintPathName, bOpen, ulHumanId, isExport);
            //else
            //    result.Add(sPrintPathName.Replace(".pdf", ".xml"));
            //*************************************************************
            //XmlNodeList xmlReqNode = null;

            //sMyPathName = sPrintPathName;
            //PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            //iTextSharp.text.Rectangle pageSize = doc.PageSize;
            //doc.Open();

            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //float X = 20f, Y = 20f;

            ////To add a line
            //PdfContentByte con = wr.DirectContent;
            //con.BeginText();

            //xmlReqNode = xmlDoc.GetElementsByTagName("title");
            //Y += 10;
            //con.SetFontAndSize(bfTimes, 14);
            //con.SetColorFill(BaseColor.BLACK);
            //con.SetTextMatrix(pageSize.GetLeft(X) + 250, pageSize.GetTop(Y));
            //con.ShowText(xmlReqNode[0].InnerText);
            //Y += 10;
            //con.MoveTo(pageSize.GetLeft(X), pageSize.GetTop(Y));
            //con.LineTo(pageSize.GetRight(X), pageSize.GetTop(Y));
            //con.Stroke();
            //con.EndText();

            //#region Patient

            //PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900 });
            ////patCCTable.SpacingBefore = 880f; ;
            //patPatient.WidthPercentage = 100;
            //PdfPCell HeadCell = new PdfPCell(new Phrase("Patient", HeadFont));
            //HeadCell.Colspan = 4;
            //HeadCell.HorizontalAlignment = 1;
            //HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            //patPatient.AddCell(HeadCell);
            //PdfPCell cell = CreateCell("Name", "", "");
            //patPatient.AddCell(cell);
            //cell = CreateCell("Date of Birth", "", "");
            //patPatient.AddCell(cell);
            //cell = CreateCell("Gender", "", "");
            //patPatient.AddCell(cell);
            //cell = CreateCell("Address", "", "");
            //patPatient.AddCell(cell);

            //string sPatientName = string.Empty;

            //xmlReqNode = xmlDoc.GetElementsByTagName("family");
            //sPatientName = xmlReqNode[0].InnerText;
            //xmlReqNode = xmlDoc.GetElementsByTagName("given");
            //sPatientName = sPatientName + "," + xmlReqNode[0].InnerText +
            //           "  " + xmlReqNode[1].InnerText;
            //xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
            //sPatientName = sPatientName + "  " + xmlReqNode[0].InnerText;
            //cell = CreateCell("", sPatientName, "");
            //patPatient.AddCell(cell);
            //xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
            //cell = CreateCell("", xmlReqNode[0].Attributes[0].Value, "");
            //patPatient.AddCell(cell);
            //xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
            //cell = CreateCell("", xmlReqNode[0].Attributes[1].Value, "");
            //patPatient.AddCell(cell);
            //string sAddress = string.Empty;
            //xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            //sAddress = xmlReqNode[0].InnerText;
            //xmlReqNode = xmlDoc.GetElementsByTagName("city");
            //sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
            //xmlReqNode = xmlDoc.GetElementsByTagName("state");
            //sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
            //xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            //sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
            //cell = CreateCell("", sAddress, "");
            //patPatient.AddCell(cell);

            //patPatient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            ////con.EndText();
            //doc.Add(patPatient);
            //doc.Add(new Paragraph("   "));
            //doc.Add(new Paragraph("   "));
            //#endregion

            //#region ProblemList
            //xmlReqNode = xmlDoc.GetElementsByTagName("table");
            //if (HL7Generator.iProcedureCount != string.Empty)
            //{
            //    PdfPTable patProblemList = new PdfPTable(new float[] { 900, 900, 900, 900 });
            //    patProblemList.WidthPercentage = 100;
            //    HeadCell = new PdfPCell(new Phrase("Problem List", HeadFont));
            //    HeadCell.Colspan = xmlReqNode[Convert.ToInt32(Convert.ToInt32(HL7Generator.iProblemCount))].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
            //    HeadCell.HorizontalAlignment = 1;
            //    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            //    patProblemList.AddCell(HeadCell);

            //    for (int i = 0; i < xmlReqNode[Convert.ToInt32(HL7Generator.iProblemCount)].ChildNodes.Count; i++)
            //    {
            //        for (int j = 0; j < xmlReqNode[Convert.ToInt32(HL7Generator.iProblemCount)].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
            //        {
            //            if (i == 0)
            //            {
            //                cell = CreateCell(xmlReqNode[Convert.ToInt32(HL7Generator.iProblemCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
            //                patProblemList.AddCell(cell);
            //            }
            //            else
            //            {
            //                cell = CreateCell("", xmlReqNode[Convert.ToInt32(HL7Generator.iProblemCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
            //                patProblemList.AddCell(cell);
            //            }
            //        }
            //    }
            //    patProblemList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //    doc.Add(patProblemList);
            //    doc.Add(new Paragraph("   "));
            //    doc.Add(new Paragraph("   "));
            //}
            //#endregion

            //#region MedicationList
            //xmlReqNode = xmlDoc.GetElementsByTagName("table");
            //if (HL7Generator.iMedicationCount != string.Empty)
            //{
            //    PdfPTable patMedicationList = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900, 900, 900, 900, 900 });
            //    patMedicationList.WidthPercentage = 100;
            //    HeadCell = new PdfPCell(new Phrase("Medication", HeadFont));
            //    HeadCell.Colspan = xmlReqNode[Convert.ToInt32(Convert.ToInt32(HL7Generator.iMedicationCount))].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
            //    HeadCell.HorizontalAlignment = 1;
            //    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            //    patMedicationList.AddCell(HeadCell);

            //    for (int i = 0; i < xmlReqNode[Convert.ToInt32(HL7Generator.iMedicationCount)].ChildNodes.Count; i++)
            //    {
            //        for (int j = 0; j < xmlReqNode[Convert.ToInt32(HL7Generator.iMedicationCount)].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
            //        {
            //            if (i == 0)
            //            {
            //                cell = CreateCell(xmlReqNode[Convert.ToInt32(HL7Generator.iMedicationCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
            //                patMedicationList.AddCell(cell);
            //            }
            //            else
            //            {
            //                cell = CreateCell("", xmlReqNode[Convert.ToInt32(HL7Generator.iMedicationCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
            //                patMedicationList.AddCell(cell);
            //            }
            //        }
            //    }
            //    patMedicationList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //    doc.Add(patMedicationList);
            //    doc.Add(new Paragraph("   "));
            //    doc.Add(new Paragraph("   "));
            //}
            //#endregion

            //#region AllergyList
            //xmlReqNode = xmlDoc.GetElementsByTagName("table");
            //if (HL7Generator.iAllergyCount != string.Empty)
            //{
            //    PdfPTable patAllergyList = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900 });
            //    patAllergyList.WidthPercentage = 100;
            //    HeadCell = new PdfPCell(new Phrase("Allergy", HeadFont));
            //    HeadCell.Colspan = xmlReqNode[Convert.ToInt32(HL7Generator.iAllergyCount)].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
            //    HeadCell.HorizontalAlignment = 1;
            //    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            //    patAllergyList.AddCell(HeadCell);

            //    for (int i = 0; i < xmlReqNode[Convert.ToInt32(HL7Generator.iAllergyCount)].ChildNodes.Count; i++)
            //    {
            //        for (int j = 0; j < xmlReqNode[Convert.ToInt32(HL7Generator.iAllergyCount)].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
            //        {
            //            if (i == 0)
            //            {
            //                cell = CreateCell(xmlReqNode[Convert.ToInt32(HL7Generator.iAllergyCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
            //                patAllergyList.AddCell(cell);
            //            }
            //            else
            //            {
            //                cell = CreateCell("", xmlReqNode[Convert.ToInt32(HL7Generator.iAllergyCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
            //                patAllergyList.AddCell(cell);
            //            }
            //        }
            //    }
            //    patAllergyList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //    doc.Add(patAllergyList);
            //    doc.Add(new Paragraph("   "));
            //    doc.Add(new Paragraph("   "));
            //}
            //#endregion

            //#region ResultList
            //xmlReqNode = xmlDoc.GetElementsByTagName("table");
            //if (HL7Generator.iTestResultCount != string.Empty)
            //{
            //    PdfPTable patResultList = new PdfPTable(new float[] { 900, 900, 900, 900, 900 });
            //    patResultList.WidthPercentage = 100;
            //    HeadCell = new PdfPCell(new Phrase("Test Results", HeadFont));
            //    HeadCell.Colspan = xmlReqNode[Convert.ToInt32(HL7Generator.iTestResultCount)].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
            //    HeadCell.HorizontalAlignment = 1;
            //    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            //    patResultList.AddCell(HeadCell);

            //    for (int i = 0; i < xmlReqNode[Convert.ToInt32(HL7Generator.iTestResultCount)].ChildNodes.Count; i++)
            //    {
            //        for (int j = 0; j < xmlReqNode[Convert.ToInt32(HL7Generator.iTestResultCount)].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
            //        {
            //            if (i == 0)
            //            {
            //                cell = CreateCell(xmlReqNode[Convert.ToInt32(HL7Generator.iTestResultCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
            //                patResultList.AddCell(cell);
            //            }
            //            else
            //            {
            //                cell = CreateCell("", xmlReqNode[Convert.ToInt32(HL7Generator.iTestResultCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
            //                patResultList.AddCell(cell);
            //            }
            //        }
            //    }
            //    patResultList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //    doc.Add(patResultList);
            //    doc.Add(new Paragraph("   "));
            //    doc.Add(new Paragraph("   "));
            //}
            //#endregion

            //#region Procedures
            //xmlReqNode = xmlDoc.GetElementsByTagName("table");
            //if (HL7Generator.iProcedureCount != string.Empty)
            //{
            //    PdfPTable patProceduresList = new PdfPTable(new float[] { 900, 900, 900, 900 });
            //    patProceduresList.WidthPercentage = 100;
            //    HeadCell = new PdfPCell(new Phrase("Procedures", HeadFont));
            //    HeadCell.Colspan = xmlReqNode[Convert.ToInt32(HL7Generator.iProcedureCount)].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
            //    HeadCell.HorizontalAlignment = 1;
            //    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            //    patProceduresList.AddCell(HeadCell);

            //    for (int i = 0; i < xmlReqNode[Convert.ToInt32(HL7Generator.iProcedureCount)].ChildNodes.Count; i++)
            //    {
            //        for (int j = 0; j < xmlReqNode[Convert.ToInt32(HL7Generator.iProcedureCount)].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
            //        {
            //            if (i == 0)
            //            {
            //                cell = CreateCell(xmlReqNode[Convert.ToInt32(HL7Generator.iProcedureCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
            //                patProceduresList.AddCell(cell);
            //            }
            //            else
            //            {
            //                cell = CreateCell("", xmlReqNode[Convert.ToInt32(HL7Generator.iProcedureCount)].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
            //                patProceduresList.AddCell(cell);
            //            }
            //        }
            //    }
            //    patProceduresList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //    doc.Add(patProceduresList);
            //    doc.Add(new Paragraph("   "));
            //    doc.Add(new Paragraph("   "));
            //}
            //#endregion

            //doc.Close();

            return result;
        }


        public ArrayList ImportCCD(XmlDocument xmlDoc, string sPrintPathName, Boolean bOpen, ulong ulMyHumanID, bool isExport)
        {
            ArrayList result = new ArrayList();
            XmlNodeList xmlReqNode = null;
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            PdfWriter wr;
            string[] sFileName = sPrintPathName.Split('\\');
            try
            {
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.OpenOrCreate));
            }
            catch
            {
                var process = System.Diagnostics.Process.GetProcesses().Where(a => a.MainWindowTitle.Trim() != string.Empty && a.MainWindowTitle.StartsWith(sFileName[3])).ToList();
                if (process != null)
                {
                    foreach (var obj in process)
                    {
                        if (!obj.HasExited)
                            obj.Kill();
                    }
                }
                Thread.Sleep(500);
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.OpenOrCreate));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            HeaderEventGenerate headerEvent = new HeaderEventGenerate(hn, PhysicianList, Enc, ulMyHumanID, WellnessNotes);
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnEndPage(wr, doc);

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            float X = 20f, Y = 20f;

            //To add a line
            PdfContentByte con = wr.DirectContent;
            con.BeginText();
            if (xmlDoc.GetElementsByTagName("title") != null)
                xmlReqNode = xmlDoc.GetElementsByTagName("title");
            Y += 10;
            con.SetFontAndSize(bfTimes, 14);
            con.SetColorFill(BaseColor.BLACK);
            con.SetTextMatrix(pageSize.GetLeft(X) + 250, pageSize.GetTop(Y));
            if (xmlReqNode != null && xmlReqNode.Count > 0)
            {
                con.ShowText(xmlReqNode[0].InnerText);
            }
            Y += 10;
            con.MoveTo(pageSize.GetLeft(X), pageSize.GetTop(Y));
            con.LineTo(pageSize.GetRight(X), pageSize.GetTop(Y));
            con.Stroke();
            con.EndText();

            #region Patient

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900, 900, 900 });
            //patCCTable.SpacingBefore = 880f; ;
            patPatient.WidthPercentage = 100;
            PdfPCell HeadCell = new PdfPCell(new Phrase("Patient", HeadFont));
            HeadCell.Colspan = 8;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            PdfPCell cell = CreateCell("Name", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Date of Birth", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Sex", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Address", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Identification Number", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Race", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Ethnicity", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Preferred language", "", "");
            patPatient.AddCell(cell);

            string sPatientName = string.Empty;

            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            if (xmlReqNode != null && xmlReqNode.Count > 0)
            {
                sPatientName = xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("given");
                sPatientName = sPatientName + "," + xmlReqNode[0].InnerText +
                           "  " + xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
                sPatientName = sPatientName + "  " + xmlReqNode[0].InnerText;
                cell = CreateCell("", sPatientName, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
                cell = CreateCell("", xmlReqNode[0].Attributes[0].Value, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
                cell = CreateCell("", xmlReqNode[0].Attributes[1].Value, "");
                patPatient.AddCell(cell);
                string sAddress = string.Empty;
                xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                sAddress = xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("city");
                sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("state");
                sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
                cell = CreateCell("", sAddress, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("id");
                cell = CreateCell("", xmlReqNode[1].Attributes[0].Value, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("Race");
                cell = CreateCell("", xmlReqNode[0].InnerText, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("Ethnicity");
                cell = CreateCell("", xmlReqNode[0].InnerText, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("Preferredlanguage");
                cell = CreateCell("", xmlReqNode[0].InnerText, "");
                patPatient.AddCell(cell);
            }
            patPatient.AddCell(cell);

            patPatient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //con.EndText();
            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));
            #endregion
            #region Physician

            PdfPTable Physician = new PdfPTable(new float[] { 900, 900, 900 });
            //patCCTable.SpacingBefore = 880f; ;
            Physician.WidthPercentage = 100;
            PdfPCell HeadCell1 = new PdfPCell(new Phrase("Physician", HeadFont));
            HeadCell1.Colspan = 3;
            HeadCell1.HorizontalAlignment = 1;
            HeadCell1.BackgroundColor = BaseColor.DARK_GRAY;
            Physician.AddCell(HeadCell1);
            PdfPCell cell1 = CreateCell("Name", "", "");
            Physician.AddCell(cell1);
            cell1 = CreateCell("Address", "", "");
            Physician.AddCell(cell1);
            cell1 = CreateCell("Phone Number", "", "");
            Physician.AddCell(cell1);

            string sPhysicianName = string.Empty;

            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            if (xmlReqNode != null && xmlReqNode.Count > 0)
            {
                sPhysicianName = xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("given");
                sPhysicianName = sPhysicianName + "," + xmlReqNode[1].InnerText +
                           "  " + xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
                sPhysicianName = sPhysicianName + "  " + xmlReqNode[1].InnerText;
                cell1 = CreateCell("", sPhysicianName, "");
                Physician.AddCell(cell1);
                string sAddress = string.Empty;
                xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                sAddress = xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("city");
                sAddress = sAddress + "\n" + xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("state");
                sAddress = sAddress + "\n" + xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                sAddress = sAddress + "\n" + xmlReqNode[1].InnerText;
                cell1 = CreateCell("", sAddress, "");
                Physician.AddCell(cell1);
                xmlReqNode = xmlDoc.GetElementsByTagName("id");
                cell1 = CreateCell("", xmlReqNode[1].Attributes[0].Value, "");
                xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
                cell1 = CreateCell("", xmlReqNode[0].Attributes[0].Value, "");
                Physician.AddCell(cell1);

            }
            Physician.AddCell(cell1);

            Physician.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            //con.EndText();
            doc.Add(Physician);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));
            #endregion
            XmlNodeList xmlParentNode = xmlDoc.GetElementsByTagName("title");
            for (int iNode = 1; iNode < xmlParentNode.Count; iNode++)
            {
                #region Vitals
                xmlReqNode = xmlDoc.GetElementsByTagName("table");
                if (xmlParentNode[iNode].InnerText == "Vital Signs")
                {
                    PdfPTable patVitalsList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                    patVitalsList.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                    HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patVitalsList.AddCell(HeadCell);

                    for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                    {
                        for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                        {
                            if (i == 0)
                            {
                                cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                patVitalsList.AddCell(cell);
                            }
                            else
                            {
                                cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                patVitalsList.AddCell(cell);
                            }
                        }
                    }
                    patVitalsList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                    doc.Add(patVitalsList);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));
                }
                #endregion

                #region ProblemList
                xmlReqNode = xmlDoc.GetElementsByTagName("table");
                if (xmlParentNode[iNode].InnerText == "Problems")
                {
                    PdfPTable patProblemList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                    patProblemList.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                    HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patProblemList.AddCell(HeadCell);

                    for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                    {
                        for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                        {
                            if (i == 0)
                            {
                                cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                patProblemList.AddCell(cell);
                            }
                            else
                            {
                                cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                patProblemList.AddCell(cell);
                            }
                        }
                    }
                    patProblemList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                    doc.Add(patProblemList);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));
                }
                #endregion

                #region MedicationList
                if (isExport || chkImmunization.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Medications")
                    {
                        PdfPTable patMedicationList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patMedicationList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patMedicationList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patMedicationList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patMedicationList.AddCell(cell);
                                }
                            }
                        }
                        patMedicationList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patMedicationList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion

                #region AllergyList
                if (isExport || chkAllergies.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Allergies and Adverse Reactions")
                    {
                        PdfPTable patAllergyList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patAllergyList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patAllergyList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patAllergyList.AddCell(cell);
                                }
                                else
                                {
                                    //Added by Saravanakumar On 21-01-2013 Bugid:12101 
                                    if (xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText == DateTime.MinValue.ToString("dd-MMM-yyyy"))
                                    {
                                        xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText = string.Empty;
                                        cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    }
                                    else
                                    {
                                        cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    }

                                    patAllergyList.AddCell(cell);
                                }
                            }
                        }
                        patAllergyList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patAllergyList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion



                #region Procedures
                if (isExport || chkProcedures.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Procedures")
                    {
                        PdfPTable patProceduresList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patProceduresList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patProceduresList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patProceduresList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patProceduresList.AddCell(cell);
                                }
                            }
                        }
                        patProceduresList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patProceduresList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion
                #region CarePlan
                if (isExport || chkCarePlan.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Plan")
                    {
                        PdfPTable patPlanList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patPlanList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patPlanList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patPlanList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patPlanList.AddCell(cell);
                                }
                            }
                        }
                        patPlanList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patPlanList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion

                #region Reasonforvisit
                if (isExport || chkReasonOfVisit.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Purpose Of Visit")
                    {
                        PdfPTable patPurposeList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patPurposeList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patPurposeList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patPurposeList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patPurposeList.AddCell(cell);
                                }
                            }
                        }
                        patPurposeList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patPurposeList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion
                #region ImmunizationList
                if (isExport || chkImmunization.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Immunizations")
                    {
                        PdfPTable patProblemList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patProblemList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patProblemList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patProblemList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patProblemList.AddCell(cell);
                                }
                            }
                        }
                        patProblemList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patProblemList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion
                #region SocialHistory
                if (isExport || chkSmokingStatus.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Social History")
                    {
                        PdfPTable patProblemList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patProblemList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patProblemList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patProblemList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patProblemList.AddCell(cell);
                                }
                            }
                        }
                        patProblemList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patProblemList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion
                #region ClinicalInstruction
                if (isExport || chkClinicalInstruction.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Instructions")
                    {
                        PdfPTable patProceduresList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patProceduresList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patProceduresList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patProceduresList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patProceduresList.AddCell(cell);
                                }
                            }
                        }
                        patProceduresList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patProceduresList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion
                #region ResultList
                if (isExport || chkLaboratoryResultValues.Checked == true)
                {
                    xmlReqNode = xmlDoc.GetElementsByTagName("table");
                    if (xmlParentNode[iNode].InnerText == "Test Results")
                    {
                        PdfPTable patResultList = new PdfPTable(xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
                        patResultList.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase(xmlParentNode[iNode].InnerText, HeadFont));
                        HeadCell.Colspan = xmlReqNode[iNode - 1].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patResultList.AddCell(HeadCell);

                        for (int i = 0; i < xmlReqNode[iNode - 1].ChildNodes.Count; i++)
                        {
                            for (int j = 0; j < xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    cell = CreateCell(xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "", "");
                                    patResultList.AddCell(cell);
                                }
                                else
                                {
                                    cell = CreateCell("", xmlReqNode[iNode - 1].ChildNodes[i].ChildNodes[0].ChildNodes[j].InnerText, "");
                                    patResultList.AddCell(cell);
                                }
                            }
                        }
                        patResultList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        doc.Add(patResultList);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }
                }
                #endregion
            }
            doc.Close();

            if (bOpen == true)
            {
                //System.Diagnostics.Process.Start(sPrintPathName);
            }
            result.Add(sPrintPathName.Replace(".pdf", ".xml"));
            result.Add(sPrintPathName);
            return result;

        }

        public ArrayList ImportCCR(XmlDocument xmlDoc, string sPrintPathName, Boolean bOpen, ulong ulMyHumanID)
        {
            ArrayList result = new ArrayList();
            try
            {


                XmlNodeList xmlReqNode = null;
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.OpenOrCreate));
                iTextSharp.text.Rectangle pageSize = doc.PageSize;
                HeaderEventGenerate headerEvent = new HeaderEventGenerate(hn, PhysicianList, Enc, ulMyHumanID, WellnessNotes);
                doc.Open();
                wr.PageEvent = headerEvent;
                headerEvent.OnEndPage(wr, doc);

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                float X = 20f, Y = 20f;

                //To add a line
                PdfContentByte con = wr.DirectContent;
                con.BeginText();

                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Purpose");
                Y += 10;
                con.SetFontAndSize(bfTimes, 14);
                con.SetColorFill(BaseColor.BLACK);
                con.SetTextMatrix(pageSize.GetLeft(X) + 250, pageSize.GetTop(Y));
                con.ShowText(xmlReqNode[0].InnerText);
                Y += 10;
                con.MoveTo(pageSize.GetLeft(X), pageSize.GetTop(Y));
                con.LineTo(pageSize.GetRight(X), pageSize.GetTop(Y));
                con.Stroke();
                con.EndText();

                #region Comment
                #region Patient

                PdfPTable patPatient = new PdfPTable(6);
                //patCCTable.SpacingBefore = 880f; ;
                patPatient.WidthPercentage = 100;
                PdfPCell HeadCell = new PdfPCell(new Phrase("Patient", HeadFont));
                HeadCell.Colspan = 6;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patPatient.AddCell(HeadCell);
                PdfPCell cell = CreateCell("Name", "", "");
                patPatient.AddCell(cell);
                cell = CreateCell("Date of Birth", "", "");
                patPatient.AddCell(cell);
                cell = CreateCell("Gender", "", "");
                patPatient.AddCell(cell);
                cell = CreateCell("Address", "", "");
                patPatient.AddCell(cell);
                cell = CreateCell("Identification Number", "", "");
                patPatient.AddCell(cell);
                cell = CreateCell("Identification Number Type", "", "");
                patPatient.AddCell(cell);

                string sPatientName = string.Empty;

                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Family");
                sPatientName = xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Given");
                sPatientName = sPatientName + "," + xmlReqNode[0].InnerText +
                           "  " + xmlReqNode[1].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Suffix");
                sPatientName = sPatientName + "  " + xmlReqNode[0].InnerText;
                cell = CreateCell("", sPatientName, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:ExactDateTime");
                cell = CreateCell("", xmlReqNode[1].InnerText, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Gender");
                cell = CreateCell("", xmlReqNode[0].ChildNodes[0].InnerText, "");
                patPatient.AddCell(cell);
                string sAddress = string.Empty;
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Line1");
                sAddress = xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:City");
                sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:State");
                sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:PostalCode");
                sAddress = sAddress + "\n" + xmlReqNode[0].InnerText;
                cell = CreateCell("", sAddress, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:ID");
                cell = CreateCell("", xmlReqNode[0].InnerText, "");
                patPatient.AddCell(cell);
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:IssuedBy");
                cell = CreateCell("", xmlReqNode[0].ChildNodes[1].ChildNodes[0].InnerText, "");
                patPatient.AddCell(cell);

                patPatient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                //con.EndText();
                doc.Add(patPatient);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion

                #region Problem
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Problem");
                PdfPTable patProblemList = new PdfPTable(6);
                patProblemList.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Problems", HeadFont));
                HeadCell.Colspan = 6;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patProblemList.AddCell(HeadCell);
                cell = CreateCell("Type", "", "");
                patProblemList.AddCell(cell);
                cell = CreateCell("Description", "", "");
                patProblemList.AddCell(cell);
                cell = CreateCell("ICD9 Value", "", "");
                patProblemList.AddCell(cell);
                cell = CreateCell("SNOMED Value", "", "");
                patProblemList.AddCell(cell);
                cell = CreateCell("Status", "", "");
                patProblemList.AddCell(cell);
                cell = CreateCell("Date Diagnosed", "", "");
                patProblemList.AddCell(cell);

                for (int i = 0; i < xmlReqNode.Count; i++)
                {
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patProblemList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].InnerText, "");
                    patProblemList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patProblemList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patProblemList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].InnerText, "");
                    patProblemList.AddCell(cell);
                    string sTemp = string.Empty;
                    try
                    {
                        sTemp = xmlReqNode[i].ChildNodes[6].ChildNodes[0].InnerText;
                    }
                    catch
                    {

                    }
                    cell = CreateCell("", sTemp, "");
                    patProblemList.AddCell(cell);
                }

                patProblemList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                doc.Add(patProblemList);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion

                #region Alerts
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Alert");
                PdfPTable patAlertList = new PdfPTable(12);
                patAlertList.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Alert", HeadFont));
                HeadCell.Colspan = 12;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patAlertList.AddCell(HeadCell);
                cell = CreateCell("Type", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Code Value", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Code System", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Description", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Description Value", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Description Code System", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Status", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Product Name", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Product Value", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Product System", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Reaction", "", "");
                patAlertList.AddCell(cell);
                cell = CreateCell("Adverse Event Date", "", "");
                patAlertList.AddCell(cell);

                for (int i = 0; i < xmlReqNode.Count; i++)
                {
                    try
                    {
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[2].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[2].ChildNodes[1].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[1].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[6].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[6].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[6].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[7].ChildNodes[0].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                        cell = CreateCell("", xmlReqNode[i].ChildNodes[8].ChildNodes[0].InnerText, "");
                        patAlertList.AddCell(cell);
                    }
                    catch
                    {

                    }
                }

                patAlertList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                doc.Add(patAlertList);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion

                #region Medications
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Medication");
                PdfPTable patMedicationList = new PdfPTable(11);
                patMedicationList.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Medications", HeadFont));
                HeadCell.Colspan = 11;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patMedicationList.AddCell(HeadCell);
                cell = CreateCell("ProductName", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Code System", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Code Value", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("BrandName", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Strength", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Units", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Form", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Frequency", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Instruction", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Date Started", "", "");
                patMedicationList.AddCell(cell);
                cell = CreateCell("Status", "", "");
                patMedicationList.AddCell(cell);


                for (int i = 0; i < xmlReqNode.Count; i++)
                {
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[1].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[2].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[3].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText, "");
                    patMedicationList.AddCell(cell);
                    string sTemp1 = string.Empty;
                    string sTemp2 = string.Empty;
                    try
                    {
                        sTemp1 = xmlReqNode[i].ChildNodes[7].ChildNodes[0].InnerText;
                    }
                    catch
                    {
                    }
                    try
                    {
                        sTemp2 = xmlReqNode[i].ChildNodes[8].ChildNodes[0].InnerText;
                    }
                    catch
                    {

                    }
                    cell = CreateCell("", sTemp1, "");
                    patMedicationList.AddCell(cell);
                    cell = CreateCell("", sTemp2, "");
                    patMedicationList.AddCell(cell);
                }

                patMedicationList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                doc.Add(patMedicationList);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion

                #region Immunization
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Immunization");
                PdfPTable patImmuList = new PdfPTable(9);
                patImmuList.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Immunization", HeadFont));
                HeadCell.Colspan = 9;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patImmuList.AddCell(HeadCell);
                cell = CreateCell("ProductName", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("CVX Code System", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("CVX Code Value", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("RxNorm Code System", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("RxNorm Code Value", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("Manufacturer", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("Direction", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("Site", "", "");
                patImmuList.AddCell(cell);
                cell = CreateCell("Reaction", "", "");
                patImmuList.AddCell(cell);


                for (int i = 0; i < xmlReqNode.Count; i++)
                {
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[6].ChildNodes[0].ChildNodes[0].InnerText, "");
                    patImmuList.AddCell(cell);
                }

                patImmuList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                doc.Add(patImmuList);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion

                #region Results
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Result");
                PdfPTable patResultList = new PdfPTable(12);
                patResultList.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Result", HeadFont));
                HeadCell.Colspan = 12;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patResultList.AddCell(HeadCell);
                cell = CreateCell("Type", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Description", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Description Code Value", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Description Code System", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Test Description", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Test Code", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Test Code System", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Status", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Result Value", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Normal Value", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Abnormal Flag", "", "");
                patResultList.AddCell(cell);
                cell = CreateCell("Date Performed", "", "");
                patResultList.AddCell(cell);


                for (int i = 0; i < xmlReqNode.Count; i++)
                {
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[1].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[4].ChildNodes[0].InnerText + xmlReqNode[i].ChildNodes[5].ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[5].ChildNodes[5].ChildNodes[0].InnerText, "");
                    patResultList.AddCell(cell);
                    string sTemp1 = string.Empty;
                    string sTemp2 = string.Empty;
                    try
                    {
                        sTemp1 = xmlReqNode[i].ChildNodes[6].ChildNodes[0].InnerText;
                    }
                    catch
                    {
                    }
                    try
                    {
                        sTemp2 = xmlReqNode[i].ChildNodes[7].ChildNodes[0].InnerText;
                    }
                    catch
                    {

                    }
                    cell = CreateCell("", sTemp1, "");
                    patResultList.AddCell(cell);
                    cell = CreateCell("", sTemp2, "");
                    patResultList.AddCell(cell);
                }

                patResultList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                doc.Add(patResultList);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion

                #region Procedures
                xmlReqNode = xmlDoc.GetElementsByTagName("ccr:Procedure");
                PdfPTable patProcList = new PdfPTable(9);
                patProcList.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Procedure", HeadFont));
                HeadCell.Colspan = 9;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patProcList.AddCell(HeadCell);
                cell = CreateCell("Type", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("Description", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("CPT Code Value", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("ICD Code Value", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("Status", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("Substance", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("Method", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("Position", "", "");
                patProcList.AddCell(cell);
                cell = CreateCell("Site", "", "");
                patProcList.AddCell(cell);

                for (int i = 0; i < xmlReqNode.Count; i++)
                {
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[3].ChildNodes[2].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[4].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[7].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[8].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[9].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                    cell = CreateCell("", xmlReqNode[i].ChildNodes[10].ChildNodes[0].InnerText, "");
                    patProcList.AddCell(cell);
                }

                patProcList.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                doc.Add(patProcList);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
                #endregion
                #endregion

                doc.Close();

                if (bOpen == true)
                {
                    //System.Diagnostics.Process.Start(sPrintPathName);
                }
                result.Add(sPrintPathName.Replace(".pdf", ".xml"));
                result.Add(sPrintPathName);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('000016');", true);
                //ApplicationObject.erroHandler.DisplayErrorMessage("000016", "IMPORT CCR");
            }
            return result;

        }

        private PdfPCell CreateCell(string HeaderText, string ValueText, string ModuleText)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph par = new Paragraph(HeaderText, reducedFont);
            cell.AddElement(par);
            par = new Paragraph(ValueText, normalFont);
            cell.AddElement(par);
            par = new Paragraph(ModuleText, HeadFont);
            cell.AddElement(par);
            return cell;
        }
        public class HeaderEventGenerate : PdfPageEventHelper
        {
            Human hn = null;
            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();

            PhysicianManager physicianMngr = new PhysicianManager();
            InsurancePlanManager InsurancePlanMngr = new InsurancePlanManager();
            StaticLookupManager StaticLookupMngr = new StaticLookupManager();
            Encounter Enc = null;
            ulong ulHumanId = 0;
            //Latha - 10 Aug 2011 - Start - AllLookups singleton
            //AllLookups allLookups = new AllLookups();
            //Latha - 10 Aug 2011 - En
            FillClinicalSummary WellnessNotes = null;


            public HeaderEventGenerate(Human Myhn, IList<PhysicianLibrary> MyPhysicianList, Encounter MyEnc, ulong ulMyHumanId, FillClinicalSummary MyWellnessNotes)
            {
                hn = Myhn;
                PhysicianList = MyPhysicianList;
                Enc = MyEnc;
                ulHumanId = ulMyHumanId;
                WellnessNotes = MyWellnessNotes;
            }


            public override void OnEndPage(PdfWriter writer, Document document)
            {
                //base.OnEndPage(writer, document);

                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
                cb.SetTextMatrix(pageSize.GetLeft(50), pageSize.GetBottom(60));

                IList<StaticLookup> fieldList = new List<StaticLookup>();
                string sTemp = string.Empty;
                //if (Enc.Is_Physician_Asst_Process == "N")
                //{

                if (Enc != null)
                {
                    IList<PhysicianLibrary> TempPhy = new List<PhysicianLibrary>();
                    TempPhy = physicianMngr.GetphysiciannameByPhyID(Convert.ToUInt64(Enc.Encounter_Provider_ID));
                    if (UtilityManager.ConvertToLocal(Enc.Encounter_Provider_Signed_Date).ToString("dd-MMM-yyyy") != "01-Jan-0001")
                    {

                        fieldList = StaticLookupMngr.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN");
                        if (fieldList != null && fieldList.Count > 0)
                            sTemp = fieldList[0].Value;
                        if (TempPhy != null && TempPhy.Count > 0)
                            sTemp = sTemp.Replace("<Physician>", TempPhy[0].PhyPrefix + " " + TempPhy[0].PhyFirstName + " " + TempPhy[0].PhyMiddleName + " " + TempPhy[0].PhyLastName + " " + TempPhy[0].PhySuffix);
                        sTemp = sTemp.Replace("<Date>", UtilityManager.ConvertToLocal(Enc.Encounter_Provider_Signed_Date).ToString("dd-MMM-yyyy hh:mm tt"));
                        cb.ShowText(sTemp);
                    }
                    //}
                    //else
                    //{
                    if (Enc.Is_Physician_Asst_Process == "Y")
                    {
                        cb.SetTextMatrix(pageSize.GetLeft(50), pageSize.GetBottom(50));
                        if (UtilityManager.ConvertToLocal(Enc.Encounter_Provider_Review_Signed_Date).ToString("dd-MMM-yyyy") != "01-Jan-0001")
                        {
                            if (WellnessNotes.TreatmentPlan.Count > 0)
                            {
                                if (WellnessNotes.TreatmentPlan[0].Addendum_Plan != null)
                                {
                                    //Latha - 10 Aug 2011 - Start - AllLookups singleton
                                    fieldList = StaticLookupMngr.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                                    //Latha - 10 Aug 2011 - End
                                    if (fieldList != null && fieldList.Count > 0)
                                        sTemp = fieldList[0].Value;
                                    if (PhysicianList != null && PhysicianList.Count > 0)
                                        sTemp = sTemp.Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                                    sTemp = sTemp.Replace("<Date>", UtilityManager.ConvertToLocal(Enc.Encounter_Provider_Review_Signed_Date).ToString("dd-MMM-yyyy hh:mm tt"));
                                    string[] sNew = sTemp.Split('|');
                                    cb.ShowText(sNew[0]);
                                    cb.SetTextMatrix(pageSize.GetLeft(50), pageSize.GetBottom(40));
                                    cb.ShowText(sNew[1]);
                                }
                            }
                            else
                            {
                                //Latha - 10 Aug 2011 - Start - AllLookups singleton
                                fieldList = StaticLookupMngr.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITHOUT CHANGES");
                                //Latha - 10 Aug 2011 - End
                                if (fieldList != null && fieldList.Count > 0)
                                    sTemp = fieldList[0].Value;
                                if (PhysicianList != null && PhysicianList.Count > 0)
                                    sTemp = sTemp.Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                                sTemp = sTemp.Replace("<Date>", UtilityManager.ConvertToLocal(Enc.Encounter_Provider_Review_Signed_Date).ToString("dd-MMM-yyyy hh:mm tt"));
                                string[] sNew = sTemp.Split('|');
                                cb.ShowText(sNew[0]);
                                cb.SetTextMatrix(pageSize.GetLeft(50), pageSize.GetBottom(40));
                                cb.ShowText(sNew[1]);
                            }

                        }
                    }
                    //}
                }

                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
                cb.SetTextMatrix(pageSize.GetRight(70), pageSize.GetBottom(30));
                cb.ShowText("Page " + writer.PageNumber.ToString());
                cb.EndText();
            }
        }

        protected void btnDownloadXML_Click(object sender, EventArgs e)
        {
            OpenXML(hdnXmlPath.Value);
        }

        protected void btnSendSummary_Click(object sender, EventArgs e)
        {
            IList<string> sRecList = new List<string>();
            //string[] sListRec = txtRecAdd.Text.Split(new char[] { ',' });
            string[] sListRec = DLCRecAdd.txtDLC.Text.Split(new char[] { ',' });
            if (sListRec != null)
            {
                foreach (string element in sListRec)
                {
                    sRecList.Insert(sRecList.Count, element);
                }
            }
            ComposeEmail(sRecList, lblAttachment.Text, txtMailText.Text, ClientSession.HumanId.ToString());
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "SEND";
            alManager.InsertIntoAuditLog("EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
        }

        public void ComposeEmail(IList<string> sRecipient, string sAttachmentPath, string sContent, string sSub)
        {
            PhiMailConnector pcConnection;
            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"] != null)
                    PhiMailConnector.SetTrustAnchor((System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"].ToString()));
                PhiMailConnector.SetCheckRevocation(false);
                pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["phiMailPortNo"]));
            }
            catch (Exception ex)
            {
                // ex.Message;
                return;
            }
            try
            {
                bool send = true;
               // pcConnection.AuthenticateUser(System.Configuration.ConfigurationSettings.AppSettings["phiMailUsername"].ToString(), System.Configuration.ConfigurationSettings.AppSettings["phiMailPassword"].ToString());
                pcConnection.AuthenticateUser(System.Configuration.ConfigurationSettings.AppSettings["phiMailEdgeUsername"].ToString(), System.Configuration.ConfigurationSettings.AppSettings["phiMailPassword"].ToString());
                if (send)
                {
                    FileInfo fiAttachment = new FileInfo(sAttachmentPath);
                    // string sSub_Def = fiAttachment.Name.Split("_")[0];
                    try
                    {
                        foreach (string rec_adderess in sRecipient)
                        {
                            pcConnection.AddRecipient(rec_adderess);
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007014');", true);
                        //txtRecAdd.Focus();
                        DLCRecAdd.txtDLC.Focus();
                        return;
                        // ex.Message;
                        //throw ex; Please Enter a Valid Direct Mail ID.
                    }
                    pcConnection.SetSubject(sSub); // Added Human ID As Subject Line For The Purpose Of Retrieval
                    if (sContent != string.Empty)
                    {
                        pcConnection.AddText(sContent);
                    }
                    else
                    {

                        pcConnection.AddText(fiAttachment.Name + " ~ Attached");
                    }

                    pcConnection.AddCDA(File.ReadAllText(Server.MapPath(sAttachmentPath)), fiAttachment.Name);
                    pcConnection.SetDeliveryNotification(true);

                    List<PhiMailConnector.SendResult> sendRes = pcConnection.Send();

                    if (sendRes[0].Succeeded)
                    {
                        Activity_Log_Entry(sRecipient, sSub, sContent);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007009','','" + sendRes[0].Recipient + "');", true);
                        pcConnection.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007015');", true);
                //txtRecAdd.Focus();
                DLCRecAdd.txtDLC.Focus();
                return;
            }
            pcConnection.Close();
        }

        public void Activity_Log_Entry(IList<string> sRecipient, string sSub, string sMsg)
        {

            activity.Human_ID = ClientSession.HumanId;
            activity.Encounter_ID = ClientSession.EncounterId;

            activity.Sent_To = sRecipient[0];
            activity.Activity_Date_And_Time = Convert.ToDateTime(ClientSession.LocalTime);
            activity.Role = "Provider";
            activity.Subject = sSub;
            activity.Message = sMsg;
            activity.Activity_Type = "CCD Export";
            activity.Activity_By = ClientSession.UserName;
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

        }

        private bool MailValidation(string Mail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                   @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                   @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(Mail))
                return (true);
            else
                return (false);
        }

        protected void btnCaregenerate_Click(object sender, EventArgs e)
        {
            hdnXmlPath.Value = string.Empty;
            FillClinicalSummary WellnessNotes = new FillClinicalSummary();
            ArrayList FileLocation = new ArrayList();
            ChiefComplaintsManager objChiefComplaintMngr = new ChiefComplaintsManager();
            WellnessNotes = objChiefComplaintMngr.GetCarePlanCDA(ClientSession.EncounterId, ClientSession.HumanId);
            Encounter Enc = new Encounter();
            if (WellnessNotes != null && WellnessNotes.Encounter[0] != null)
            {
                Enc = WellnessNotes.Encounter[0];
            }
            PhysicianLibrary PhysicianList = new PhysicianLibrary();
            string sFolderPathName = string.Empty;
            if(WellnessNotes.phyList!=null && WellnessNotes.phyList.Count>0)
            PhysicianList = WellnessNotes.phyList[0];
            if (PhysicianList==null || PhysicianList.Id == 0)
            {
                PhysicianLibrary obj = new PhysicianLibrary();
                obj.PhyFirstName = "Acurus";
                obj.PhyLastName = " Capella EHR v5.4";
                PhysicianList=obj;
            }
            string TargetFileDirectory = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "Documents\\" + Session.SessionID);
            sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["CareNotePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            Directory.CreateDirectory(sFolderPathName);

            string sPrintPathName = string.Empty;
            sPrintPathName = sFolderPathName + "\\" + WellnessNotes.Last_Name + "_" + WellnessNotes.First_Name + "_" + Enc.Facility_Name.Replace("#", "") + ClientSession.HumanId + "_" + ClientSession.EncounterId + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
            HL7Generator hl7Gen = new HL7Generator();
            XmlDocument xmlDoc = hl7Gen.CreateCarePlanXML(PhysicianList, WellnessNotes, sPrintPathName.Replace(".pdf", ".xml"));
            FileLocation.Add(sPrintPathName.Replace(".pdf", ".xml"));

            string sDownloadFile = string.Empty;
            string[] Split = new string[] { Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "Documents\\" + Session.SessionID) };
            if (FileLocation != null && FileLocation.Count != 0)
            {
                if (FileLocation != null && FileLocation.Count != 0)
                {
                    if (FileLocation[0].ToString().EndsWith(".xml") == true)
                    {
                        string[] XMLFileName = FileLocation[0].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);
                        if (hdnXmlPath.Value == string.Empty)
                        {
                            hdnXmlPath.Value = "Documents\\" + Session.SessionID.ToString() + XMLFileName[0].ToString();
                        }

                        DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(hdnXmlPath.Value));
                        if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
                        {
                            Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
                        }
                        System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath("Documents/" + Session.SessionID.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl"), true);

                    }

                }
            }

            if (hdnXmlPath.Value != null && hdnXmlPath.Value != string.Empty)
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);

            if (hdnXmlPath.Value != null && hdnXmlPath.Value != string.Empty)
            {
                // lblAttachment.Visible = true;
                lblAttachment.Text = hdnXmlPath.Value;

            }
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "GENERATE - CARE NOTE";
            alManager.InsertIntoAuditLog("EXPORT", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685
        }

        protected void chkReasonforReferral_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
