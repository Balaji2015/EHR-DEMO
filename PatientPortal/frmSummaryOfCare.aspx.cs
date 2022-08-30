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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using log4net;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml;
using System.Net;
using System.Threading;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.Script.Services;



namespace Acurus.Capella.PatientPortal
{
    public partial class frmSummaryOfCare : System.Web.UI.Page
    {
        iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
        iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
        iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        IList<FileManagementIndex> file_exam_lst = new List<FileManagementIndex>();
        IList<StaticLookup> lstStatic = new List<StaticLookup>();

        int prevNum = 0;
        string lastNumToAdd = string.Empty;
        string path = string.Empty;
        string sNoInformationInTable = string.Empty;
        string sReturn = string.Empty;
        PdfPCell HeadCell;
        PdfPCell cell;
        XmlDocument xmldoc = new XmlDocument();

        IList<Human> lsthuman = new List<Human>();
        IList<PhysicianLibrary> lstphysician = new List<PhysicianLibrary>();
        IList<Encounter> lstencounter = new List<Encounter>();
        IList<FacilityLibrary> lstfacility = new List<FacilityLibrary>();
        IList<ChiefComplaints> lstchiefcomlaint = new List<ChiefComplaints>();
        IList<SocialHistory> lstsocilahistory = new List<SocialHistory>();
        //IList<ProblemList> lstproblem = new List<ProblemList>();
        IList<Rcopia_Allergy> lstallergy = new List<Rcopia_Allergy>();
        IList<Rcopia_Medication> lstmedication = new List<Rcopia_Medication>();
        IList<Immunization> lstimmunization = new List<Immunization>();
        IList<TreatmentPlan> lsttreatmentplan = new List<TreatmentPlan>();
        IList<PatientResults> lstvitals = new List<PatientResults>();
        IList<CarePlan> lstcareplan = new List<CarePlan>();
        IList<ReferralOrder> lstreferalorder = new List<ReferralOrder>();
        IList<InHouseProcedure> lstprocedures = new List<InHouseProcedure>();
        IList<ResultMaster> lstresultmaster = new List<ResultMaster>();
        IList<ResultOBR> lstresultobr = new List<ResultOBR>();
        IList<ResultOBX> lstresultobx = new List<ResultOBX>();
        IList<ResultORC> lstresultorc = new List<ResultORC>();
        IList<Assessment> lstassesment = new List<Assessment>();
        IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
        ActivityLogManager ActivitylogMngr = new ActivityLogManager();
        ActivityLog activity = new ActivityLog();
        IList<InHouseProcedure> lstImplant = new List<InHouseProcedure>();
        IList<CarePlan> lstMentalStatus = new List<CarePlan>();
        IList<TreatmentPlan> lstGoalsSection = new List<TreatmentPlan>();
        IList<ProblemList> lsthealthconcern = new List<ProblemList>();
        IList<Orders> lstOrder = new List<Orders>();
        static string FilePath = string.Empty;
        static string FileName = string.Empty;
        static string Type = string.Empty;
        static string UniverTime = string.Empty;

        DateTime universalTime = DateTime.MinValue;
        int save_count = 0;
        // string sFileNameBind = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["FileName"] != null)
                {
                    if (Request["FileName"] != null && (Request["Type"] == "CCD" || Request["Type"] == "C32"))
                    {
                        Session["sFileNameBind"] = Path.GetFileName(Request["FileName"]);
                        FileName = Request["FileName"];
                        FilePath = Server.MapPath(Request["FileName"]);
                        Type = Request["Type"];
                        UniverTime = Request["UniversalTime"].ToString();
                        if (Request["Type"] == "CCD" || Request["Type"] == "C32")
                        {
                            universalTime = Convert.ToDateTime(Request["UniversalTime"]);
                            btnOK.Visible = true;
                        }
                        else
                            universalTime = DateTime.MinValue;

                        path = PrintPDF(Server.MapPath(Request["FileName"]), Request["Type"], universalTime);
                        
                        hdnHumanID.Value = ((IList<Human>)Session["HumanList"])[0].Id.ToString();
                        //if (ClientSession.Load_Summary_PDF == false)
                        //{
                        //    Match(hdnHumanID.Value);
                        //    ClientSession.Load_Summary_PDF = true;
                        //}
                        if (ViewState["Load_Summary_PDF"] ==null)
                        {
                            Match(hdnHumanID.Value);
                            ViewState["Load_Summary_PDF"] = "True";
                        }
                        PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                    }
                    else if (Request["FileName"] != null && Request["Type"] == "CCR")
                    {
                        PrintPDF_CCR(Server.MapPath(Request["FileName"]));
                        
                        PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                    }

                }

                

                StaticLookupManager objlookupmanager = new StaticLookupManager();
                if (Request["FileName"] != null && (Request["Type"] == "CCD" || Request["Type"] == "C32"))
                {
                    string[] StaticValues = { "All", "Allergy", "Immunization", "Medication", "Care Plan", "Reason for Referral", "Procedures", "Results", "Social History", "Vitals", "Instructions", "Chief Complaint", "Problems", "Provider", "Custodian", "Documentation Details", "Visit Details", "Author", "Medication Administered", "Functional Status", "Hospital Discharge Instructions", "Implant", "Mental Status", "Goals Section", "Health Concern", "Treatment Plan", "Laboratory Information" ,"Interventions","Health Status Evaluations/Outcomes"};
                    foreach (string values in StaticValues)
                    {
                        //cboSelection.Items.Add(new RadComboBoxItem(values));
                        System.Web.UI.WebControls.ListItem chkbx = new System.Web.UI.WebControls.ListItem();
                        chkbx.Text = values;
                        SummaryCheckList.Items.Add(chkbx);
                    }
                    //lstStatic = objlookupmanager.getStaticLookupByFieldName("SUMMARY OF CARE");
                    //lstStatic = lstStatic.OrderBy(a => a.Value).ToList();
                }
                else
                {
                    string[] StaticValues = { "All", "Results", "Problems", "Alerts", "Medications", "People", "Organizations" };
                    foreach (string values in StaticValues)
                    {
                        //cboSelection.Items.Add(new RadComboBoxItem(values));
                        System.Web.UI.WebControls.ListItem chkbx = new System.Web.UI.WebControls.ListItem();
                        chkbx.Text = values;
                        SummaryCheckList.Items.Add(chkbx);
                    }
                    // lstStatic = objlookupmanager.getStaticLookupByFieldName("SUMMARY OF CARE CCR");
                    // lstStatic = lstStatic.OrderBy(a => a.Value).ToList();
                }
                //cboSelection.FindItemByText("All").Selected = true;
                //if (lstStatic.Count > 0)
                //{
                //    for (int i = 0; i < lstStatic.Count; i++)
                //    {
                //        cboSelection.Items.Add(new RadComboBoxItem(lstStatic[i].Value));
                //    }
                //    cboSelection.FindItemByText("All").Selected = true;
                //}
                if (SummaryCheckList.Items.Count > 0)
                {
                    SummaryCheckList.Items[0].Selected = true;
                }
                hdnXMLPath.Value = Server.MapPath(Request["FileName"]);

            }

            if (Request.QueryString["pdf"] != null)
            {
                string strPdf = Request.QueryString["pdf"].ToString();
                FileStream fs = null;
                BinaryReader br = null;
                byte[] data = null;
                try
                {

                    fs = new FileStream(strPdf, FileMode.Open, FileAccess.Read, FileShare.Read);
                    br = new BinaryReader(fs, System.Text.Encoding.Default);
                    data = new byte[Convert.ToInt32(fs.Length)];
                    br.Read(data, 0, data.Length);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(data);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                    br.Close();
                    data = null;
                }
            }

            if (Request["UniversalTime"] != null)
                universalTime = Convert.ToDateTime(Request["UniversalTime"].ToString());
        }

        public string PrintPDF(string XmlPath, string sClinicalSummaryPath, DateTime universaltime)
        {
            universalTime = universaltime;
            XmlDocument xmldoc = new XmlDocument();
            XmlTextReader xmltext = new XmlTextReader(XmlPath);
            xmldoc.Load(xmltext);
            string path = Path.GetFileNameWithoutExtension(XmlPath);
            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
            string sPrintPathName = string.Empty;

            if (sClinicalSummaryPath == "ClinicalSummary")
            {
                string sFolderPathName = TargetFileDirectory;

                sPrintPathName = sFolderPathName + "\\" + path + ".pdf";
            }
            else
            {
                string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                Directory.CreateDirectory(sFolderPathName);
                sPrintPathName = sFolderPathName + "\\" + path + "_SummaryOfCare.pdf";
            }
            Session["PDF_Path"] = sPrintPathName;
            string sMyPathName = sPrintPathName;
            PdfWriter wr;
            string[] sFileName = sMyPathName.Split('\\');
            try
            {
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            catch
            {
                var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                //if(process.Count>0)
                // process[0].Kill();

                if (process != null)
                {
                    foreach (var obj in process)
                    {
                        if (!obj.HasExited)
                            obj.Kill();
                    }
                }
                Thread.Sleep(500);


                using (FileStream ms = new FileStream(sPrintPathName, FileMode.Create))
                {
                    wr = PdfWriter.GetInstance(doc, ms);
                }
                // wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            float X = 20f, Y = 20f;

            if (sClinicalSummaryPath == "PatientPortal")
                ViewState["PatientPortal"] = "PatientPortal";

            PrintPatient(doc, xmldoc);

            PrintReferringProvider(doc, xmldoc);

            PrintAuthor(doc, xmldoc);

            PrintCustodian(doc, xmldoc);

            PrintProvider(doc, xmldoc);

            PrintDocumentDetail(doc, xmldoc);

            PrintVisitDetail(doc, xmldoc);

            PrintTreatmentPlan(doc, xmldoc);

            PrintAllergy(doc, xmldoc);

            //  PrintEncounter(doc, xmldoc);

            PrintImmunization(doc, xmldoc);

            PrintMedication(doc, xmldoc);

            PrintCarePlan(doc, xmldoc);

            //PrintHospitalMedications(doc, xmldoc);

            PrintReasonForReferal(doc, xmldoc);
            if (sClinicalSummaryPath == "PatientPortal")
                PrintProblemsPatientPortal(doc, xmldoc);
            else
                PrintProblems(doc, xmldoc);

            PrintProcedures(doc, xmldoc);

            PrintFunctionalStatus(doc, xmldoc);

            PrintLaboratoryInformation(doc, xmldoc);

            if (sClinicalSummaryPath == "PatientPortal")
                PrintResultsforPatientPortal(doc, xmldoc);
            else
                PrintResults(doc, xmldoc);

            PrintLabTests(doc, xmldoc);

            PrintSocialHistory(doc, xmldoc);

            PrintVitals(doc, xmldoc);

            PrintHospitalInstructions(doc, xmldoc);

            PrintInstructions(doc, xmldoc);

            PrintChiefComplaint(doc, xmldoc);

            PrintMedicationAdministered(doc, xmldoc);

            PrintImplant(doc, xmldoc);

            PrintMentalStatus(doc, xmldoc);
            PrintGoalsSection(doc, xmldoc);
            PrintHealthConcernSection(doc, xmldoc);

            PrintInterventions(doc, xmldoc);
            PrintHealthStatusEvaluations(doc, xmldoc);

            doc.Close();
            xmltext.Close();

            if (sClinicalSummaryPath != "ClinicalSummary" && universaltime != DateTime.MinValue && sClinicalSummaryPath != "PatientPortal")
            {
                if (ClientSession.Save_Summary == false)
                {
                    SaveSummaryOfCareInDatabase();
                    ClientSession.Save_Summary = true;
                }
            }

            return sPrintPathName;
        }
        public string PrintPDF(string XmlPath, string sClinicalSummaryPath, DateTime universaltime,string FileName,string Type)
        {
            universalTime = universaltime;
            XmlDocument xmldoc = new XmlDocument();
            XmlTextReader xmltext = new XmlTextReader(XmlPath);
            xmldoc.Load(xmltext);
            string path = Path.GetFileNameWithoutExtension(XmlPath);
            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
            string sPrintPathName = string.Empty;

            if (sClinicalSummaryPath == "ClinicalSummary")
            {
                string sFolderPathName = TargetFileDirectory;

                sPrintPathName = sFolderPathName + "\\" + path + ".pdf";
            }
            else
            {
                string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                Directory.CreateDirectory(sFolderPathName);
                sPrintPathName = sFolderPathName + "\\" + path + "_SummaryOfCare.pdf";
            }
            Session["PDF_Path"] = sPrintPathName;
            string sMyPathName = sPrintPathName;
            PdfWriter wr;
            string[] sFileName = sMyPathName.Split('\\');
            try
            {
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            catch
            {
                var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                //if(process.Count>0)
                // process[0].Kill();

                if (process != null)
                {
                    foreach (var obj in process)
                    {
                        if (!obj.HasExited)
                            obj.Kill();
                    }
                }
                Thread.Sleep(500);


                using (FileStream ms = new FileStream(sPrintPathName, FileMode.Create))
                {
                    wr = PdfWriter.GetInstance(doc, ms);
                }
                // wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            float X = 20f, Y = 20f;

            if (sClinicalSummaryPath == "PatientPortal")
                ViewState["PatientPortal"] = "PatientPortal";

            PrintPatient(doc, xmldoc);

            PrintReferringProvider(doc, xmldoc);

            PrintAuthor(doc, xmldoc);

            PrintCustodian(doc, xmldoc);

            PrintProvider(doc, xmldoc);

            PrintDocumentDetail(doc, xmldoc);

            PrintVisitDetail(doc, xmldoc);

            PrintTreatmentPlan(doc, xmldoc);

            PrintAllergy(doc, xmldoc);

            //  PrintEncounter(doc, xmldoc);

            PrintImmunization(doc, xmldoc);

            PrintMedication(doc, xmldoc);

            PrintCarePlan(doc, xmldoc);

            //PrintHospitalMedications(doc, xmldoc);

            PrintReasonForReferal(doc, xmldoc);
            if (sClinicalSummaryPath == "PatientPortal")
                PrintProblemsPatientPortal(doc, xmldoc);
            else
                PrintProblems(doc, xmldoc, FileName, Type);

            PrintProcedures(doc, xmldoc);

            PrintFunctionalStatus(doc, xmldoc);

            PrintLaboratoryInformation(doc, xmldoc);

            if (sClinicalSummaryPath == "PatientPortal")
                PrintResultsforPatientPortal(doc, xmldoc);
            else
                PrintResults(doc, xmldoc, FileName, Type);

            PrintLabTests(doc, xmldoc);

            PrintSocialHistory(doc, xmldoc);

            PrintVitals(doc, xmldoc);

            PrintHospitalInstructions(doc, xmldoc);

            PrintInstructions(doc, xmldoc);

            PrintChiefComplaint(doc, xmldoc);

            PrintMedicationAdministered(doc, xmldoc);

            PrintImplant(doc, xmldoc);

            PrintMentalStatus(doc, xmldoc);
            PrintGoalsSection(doc, xmldoc);
            PrintHealthConcernSection(doc, xmldoc);

            PrintInterventions(doc, xmldoc);
            PrintHealthStatusEvaluations(doc, xmldoc);

            doc.Close();
            xmltext.Close();

            if (sClinicalSummaryPath != "ClinicalSummary" && universaltime != DateTime.MinValue && sClinicalSummaryPath != "PatientPortal")
            {
                if (ClientSession.Save_Summary == false)
                {
                    SaveSummaryOfCareInDatabase();
                    ClientSession.Save_Summary = true;
                }
            }

            return sPrintPathName;
        }

        public void PrintPDF_CCR(string XmlPath)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlTextReader xmltext = new XmlTextReader(XmlPath);
            xmldoc.Load(xmltext);
            string path = Path.GetFileNameWithoutExtension(XmlPath);
            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
            string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            Directory.CreateDirectory(sFolderPathName);
            string sPrintPathName = string.Empty;
            sPrintPathName = sFolderPathName + "\\" + path + "_SummaryOfCare.pdf";
            Session["PDF_Path"] = sPrintPathName;
            string sMyPathName = sPrintPathName;
            PdfWriter wr;
            string[] sFileName = sMyPathName.Split('\\');
            try
            {
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            catch
            {
                var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                process[0].Kill();
                Thread.Sleep(500);
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            float X = 20f, Y = 20f;

            Print_CCR_ContinuityOfCare(doc, xmldoc);

            Print_CCR_PatientDemographics(doc, xmldoc);

            Print_CCR_Alerts(doc, xmldoc);

            Print_CCR_Problems(doc, xmldoc);

            Print_CCR_Medications(doc, xmldoc);

            Prinit_CCR_Results(doc, xmldoc);

            Prinit_CCR_People(doc, xmldoc);

            Prinit_CCR_Organization(doc, xmldoc);

            doc.Close();
            xmltext.Close();
        }

        public static string StripTagsRegex(string source)
        {
            if (source == null)
                return string.Empty;
            else
                return Regex.Replace(source, "<.*?>", string.Empty);
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

        private DataSet ConvertHTMLTablesToDataSet(string HTML)
        {
            // Declarations 
            DataSet ds = new DataSet();
            DataTable dt = null;
            DataRow dr = null;
            DataColumn dc = null;
            string TableExpression = "<table[^>]*>(.*?)</table>";
            string HeaderExpression = "<th[^>]*>(.*?)</th>";
            string RowExpression = "<tr[^>]*>(.*?)</tr>";
            string ColumnExpression = "<td[^>]*>(.*?)</td>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;

            // Get a match for all the tables in the HTML 
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Loop through each table element 
            foreach (Match Table in Tables)
            {
                // Reset the current row counter and the header flag 
                iCurrentRow = 0;
                HeadersExist = false;

                // Add a new table to the DataSet 
                dt = new DataTable();

                //Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names) 
                if (Table.Value.Contains("<th"))
                {
                    // Set the HeadersExist flag 
                    HeadersExist = true;

                    // Get a match for all the rows in the table 
                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    // Loop through each header element 
                    foreach (Match Header in Headers)
                    {
                        dt.Columns.Add(Header.Groups[1].ToString());
                    }
                }
                else
                {
                    if (Request["Type"] != "C32")
                    {
                        for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                        {
                            dt.Columns.Add("Column " + iColumns);
                        }
                    }
                    else
                    {
                        for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                        {
                            dt.Columns.Add("Test");
                        }
                    }
                }


                //Get a match for all the rows in the table 

                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Loop through each row element 
                foreach (Match Row in Rows)
                {
                    // Only loop through the row if it isn't a header row 
                    if (!(iCurrentRow == 0 && HeadersExist))
                    {
                        // Create a new row and reset the current column counter 
                        dr = dt.NewRow();
                        iCurrentColumn = 0;

                        // Get a match for all the columns in the row 
                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                        // Loop through each column element 
                        foreach (Match Column in Columns)
                        {
                            // Add tohe value to the DataRow 
                            try
                            {
                                dr[iCurrentColumn] = Column.Groups[1].ToString();
                            }
                            catch
                            {
                                if (Request["Type"] == "C32")
                                {
                                    dt.Columns.Add(" Result");
                                    dr[iCurrentColumn] = Column.Groups[1].ToString();
                                }
                            }

                            // Increase the current column  
                            iCurrentColumn++;
                        }

                        // Add the DataRow to the DataTable 
                        dt.Rows.Add(dr);

                    }

                    // Increase the current row counter 
                    iCurrentRow++;
                }


                // Add the DataTable to the DataSet 
                ds.Tables.Add(dt);

            }

            return ds;

        }
        private DataSet ConvertHTMLTablesToDataSet(string HTML,string Type)
        {
            // Declarations 
            DataSet ds = new DataSet();
            DataTable dt = null;
            DataRow dr = null;
            DataColumn dc = null;
            string TableExpression = "<table[^>]*>(.*?)</table>";
            string HeaderExpression = "<th[^>]*>(.*?)</th>";
            string RowExpression = "<tr[^>]*>(.*?)</tr>";
            string ColumnExpression = "<td[^>]*>(.*?)</td>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;

            // Get a match for all the tables in the HTML 
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Loop through each table element 
            foreach (Match Table in Tables)
            {
                // Reset the current row counter and the header flag 
                iCurrentRow = 0;
                HeadersExist = false;

                // Add a new table to the DataSet 
                dt = new DataTable();

                //Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names) 
                if (Table.Value.Contains("<th"))
                {
                    // Set the HeadersExist flag 
                    HeadersExist = true;

                    // Get a match for all the rows in the table 
                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    // Loop through each header element 
                    foreach (Match Header in Headers)
                    {
                        dt.Columns.Add(Header.Groups[1].ToString());
                    }
                }
                else
                {
                    if (Type != "C32")
                    {
                        for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                        {
                            dt.Columns.Add("Column " + iColumns);
                        }
                    }
                    else
                    {
                        for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                        {
                            dt.Columns.Add("Test");
                        }
                    }
                }


                //Get a match for all the rows in the table 

                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Loop through each row element 
                foreach (Match Row in Rows)
                {
                    // Only loop through the row if it isn't a header row 
                    if (!(iCurrentRow == 0 && HeadersExist))
                    {
                        // Create a new row and reset the current column counter 
                        dr = dt.NewRow();
                        iCurrentColumn = 0;

                        // Get a match for all the columns in the row 
                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                        // Loop through each column element 
                        foreach (Match Column in Columns)
                        {
                            // Add tohe value to the DataRow 
                            try
                            {
                                dr[iCurrentColumn] = Column.Groups[1].ToString();
                            }
                            catch
                            {
                                if (Type == "C32")
                                {
                                    dt.Columns.Add(" Result");
                                    dr[iCurrentColumn] = Column.Groups[1].ToString();
                                }
                            }

                            // Increase the current column  
                            iCurrentColumn++;
                        }

                        // Add the DataRow to the DataTable 
                        dt.Rows.Add(dr);

                    }

                    // Increase the current row counter 
                    iCurrentRow++;
                }


                // Add the DataTable to the DataSet 
                ds.Tables.Add(dt);

            }

            return ds;

        }
        //private void setThePatientDetails(ulong humanID)
        //{
        //    if (humanID != 0)
        //    {
        //        HumanManager objhumanmanager = new HumanManager();
        //        IList<Human> humanList = null;
        //        humanList = objhumanmanager.GetPatientDetailsUsingPatientInformattion(humanID);
        //        if (humanList.Count > 0)
        //        {
        //            txtPatientName.Text = humanList[0].First_Name + " " + humanList[0].MI + " " + humanList[0].Last_Name;
        //            txtSex.Text = humanList[0].Sex;
        //            txtDOB.Text = humanList[0].Birth_Date.ToString("dd-MMM-yyyy");
        //            txtAccountNo.Text = humanList[0].Id.ToString();
        //        }
        //    }
        //}

        protected void Match(string sAccountNo)
        {
            if (sAccountNo != string.Empty)
            {

                FileManagementIndexManager objfilemanager = new FileManagementIndexManager();
                file_exam_lst = objfilemanager.GetIndexedListByHumanId(Convert.ToUInt64(sAccountNo), "SUMMARY OF CARE");
                FindFileIndex(file_exam_lst);
                prevNum = (int)Session["NumberCount"];

                string sStoringFormat = string.Empty;
                string sFTPAddress = string.Empty;
                string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
                string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                FTPImageProcess ftpImageProcess = new FTPImageProcess();
                IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
                if (ftpImageProcess.CreateDirectory(sAccountNo, ftpServerIP, ftpUserID, ftpPassword))
                {
                    string tempSelectPath = (string)Session["PDF_Path"];
                    lastNumToAdd = Convert.ToString(prevNum + 1);
                    if (lastNumToAdd.Length == 1)
                        lastNumToAdd = "0" + lastNumToAdd;
                    sStoringFormat = ClientSession.FacilityName.Replace("#", "") + "_SUMMARY_OF_CARE_" + DateTime.Now.ToString("yyyyMMdd") + "_" + sAccountNo + "_" + lastNumToAdd + ".pdf";
                    sFTPAddress = ftpImageProcess.UploadToImageServer(sAccountNo, ftpServerIP, ftpUserID, ftpPassword, tempSelectPath, sStoringFormat);
                    if (sFTPAddress != string.Empty)
                    {
                        FileManagementIndex objfileIndex = new FileManagementIndex();
                        objfileIndex.File_Path = sFTPAddress;
                        objfileIndex.Created_By = ClientSession.UserName;
                        objfileIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();//UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                        objfileIndex.Human_ID = Convert.ToUInt64(sAccountNo);
                        objfileIndex.Source = "SUMMARY OF CARE";
                        fileList.Add(objfileIndex);
                        IList<FileManagementIndex> fileIndex = objfilemanager.SaveUpdateDeleteFileManagementIndexforExamPhotos(fileList.ToArray(), null, null, ApplicationObject.macAddress, "SUMMARY OF CARE");
                    }
                }
                activity.Human_ID = Convert.ToUInt64(sAccountNo);
                activity.Encounter_ID = 0;
                activity.Sent_To = string.Empty;
                activity.Activity_Date_And_Time = Convert.ToDateTime(ClientSession.LocalTime);
                activity.Role = "Provider";
                activity.Subject = string.Empty;
                activity.Activity_Type = "CCD Matched";
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "Close();", true);
            }
            divLoading.Style.Add("display", "none");
        }

        void FindFileIndex(IList<FileManagementIndex> file_exam_lst)
        {
            int[] sortIndexNum = new int[file_exam_lst.Count];
            for (int i = 0; i < file_exam_lst.Count; i++)
            {
                if (file_exam_lst[i].File_Path != string.Empty)
                {
                    try
                    {
                        string fi = file_exam_lst[i].File_Path.Substring(file_exam_lst[i].File_Path.LastIndexOf("/") + 1);
                        prevNum = Convert.ToInt32(fi.Substring(fi.LastIndexOf("_") + 1, (fi.LastIndexOf(".") - 1) - fi.LastIndexOf("_")));
                        sortIndexNum[i] = prevNum;
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }

            if (file_exam_lst.Count > 0)
                prevNum = sortIndexNum.Max();

            Session["NumberCount"] = prevNum;
        }

        //protected void btnIVfindpatient_Click(object sender, EventArgs e)
        //{
        //    if (hdnHumanID.Value != null && hdnHumanID.Value != string.Empty)
        //    {
        //        ulong human_id = Convert.ToUInt32(hdnHumanID.Value.ToString());
        //        setThePatientDetails(human_id);
        //    }
        //    divLoading.Style.Add("display", "none");

        //}

        protected void cboSelection_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            System.IO.File.Delete((string)Session["PDF_Path"]);
            string XmlPath = Server.MapPath(Request["FileName"]);

            #region CCD

            if (Request["FileName"] != null && (Request["Type"] == "CCD" || Request["Type"] == "C32"))
            {
                if (cboSelection.Text == "All")
                {
                    path = PrintPDF(Server.MapPath(Request["FileName"]), "CCD", DateTime.MinValue);
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Allergy")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Allergy.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintAllergy(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                //else if (cboSelection.Text == "Encounter")
                //{

                //    XmlDocument xmldoc = new XmlDocument();
                //    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                //    xmldoc.Load(xmltext);
                //    string path = Path.GetFileNameWithoutExtension(XmlPath);
                //    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                //    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                //    Directory.CreateDirectory(sFolderPathName);
                //    string sPrintPathName = string.Empty;
                //    sPrintPathName = sFolderPathName + "\\" + path + "_Encounter.pdf";
                //    Session["PDF_Path"] = sPrintPathName;
                //    string sMyPathName = sPrintPathName;
                //    PdfWriter wr;
                //    string[] sFileName = sMyPathName.Split('\\');
                //    try
                //    {
                //        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                //    }
                //    catch
                //    {
                //        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                //        process[0].Kill();
                //        Thread.Sleep(500);
                //        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                //    }
                //    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                //    doc.Open();
                //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                //    float X = 20f, Y = 20f;
                //    PrintPatient(doc, xmldoc);
                //    PrintEncounter(doc, xmldoc);
                //    doc.Close();
                //    xmltext.Close();
                //    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                //}

                else if (cboSelection.Text == "Immunization")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Immunization.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintImmunization(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Medication")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Medication.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintMedication(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Care Plan")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_CarePlan.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintCarePlan(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Hospital Discharge Medications")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_HospitalMedications.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintHospitalMedications(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Reason for Referral")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_ReasonForReferal.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintReasonForReferal(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Procedures")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);

                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Procedures.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintProcedures(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Functional Status")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_FunctionalStatus.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintFunctionalStatus(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Results")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Results.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    PrintPatient(doc, xmldoc);
                    PrintResults(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Social History")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_SocialHistory.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintSocialHistory(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Vitals")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Vitals.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintVitals(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Hospital Discharge Instructions")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_HospitalInstructions.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintHospitalInstructions(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Instructions")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Instructions.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintInstructions(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Chief Complaint")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_ChiefComplaint.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintChiefComplaint(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Problems")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Problems.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintProblems(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Custodian")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Custodian.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintCustodian(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Provider")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Provider.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintProvider(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Documentation Details")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_DocumentationDetails.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintDocumentDetail(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Visit Details")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_VisitDetails.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintVisitDetail(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Author")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Author.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintAuthor(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Medication Administered")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_MedicationAdministered.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintMedicationAdministered(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Implant")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Implant.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintImplant(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Mental Status")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_MentalStatus.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintMentalStatus(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Health Concern")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_HealthConcern.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintHealthConcernSection(doc, xmldoc);

                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Goals Section")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Goal.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintGoalsSection(doc, xmldoc);

                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Treatment Plan")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_TreatmentPlan.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintTreatmentPlan(doc, xmldoc);

                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Laboratory Information")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_LabInformation.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintLaboratoryInformation(doc, xmldoc);

                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Interventions")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Interventions.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintInterventions(doc, xmldoc);

                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Health Status Evaluations/Outcomes")
                {

                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_HealthStatus.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;

                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    PrintPatient(doc, xmldoc);
                    PrintHealthStatusEvaluations(doc, xmldoc);

                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

            }

            #endregion

            #region CCR

            else
            {
                if (cboSelection.Text == "Alerts")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Alerts.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    Print_CCR_ContinuityOfCare(doc, xmldoc);
                    Print_CCR_PatientDemographics(doc, xmldoc);
                    Print_CCR_Alerts(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "All")
                {
                    PrintPDF_CCR(Server.MapPath(Request["FileName"]));
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
                else if (cboSelection.Text == "Problems")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Problems.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    Print_CCR_ContinuityOfCare(doc, xmldoc);
                    Print_CCR_PatientDemographics(doc, xmldoc);
                    Print_CCR_Problems(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Medications")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Medications.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    Print_CCR_ContinuityOfCare(doc, xmldoc);
                    Print_CCR_PatientDemographics(doc, xmldoc);
                    Print_CCR_Medications(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Results")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Results.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    Print_CCR_ContinuityOfCare(doc, xmldoc);
                    Print_CCR_PatientDemographics(doc, xmldoc);
                    Prinit_CCR_Results(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "People")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_People.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    Print_CCR_ContinuityOfCare(doc, xmldoc);
                    Print_CCR_PatientDemographics(doc, xmldoc);
                    Prinit_CCR_People(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }

                else if (cboSelection.Text == "Organizations")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XmlTextReader xmltext = new XmlTextReader(XmlPath);
                    xmldoc.Load(xmltext);
                    string path = Path.GetFileNameWithoutExtension(XmlPath);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
                    string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                    Directory.CreateDirectory(sFolderPathName);
                    string sPrintPathName = string.Empty;
                    sPrintPathName = sFolderPathName + "\\" + path + "_Organizations.pdf";
                    Session["PDF_Path"] = sPrintPathName;
                    string sMyPathName = sPrintPathName;
                    PdfWriter wr;
                    string[] sFileName = sMyPathName.Split('\\');
                    try
                    {
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    catch
                    {
                        var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                        process[0].Kill();
                        Thread.Sleep(500);
                        wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                    }
                    iTextSharp.text.Rectangle pageSize = doc.PageSize;
                    doc.Open();
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    float X = 20f, Y = 20f;
                    Print_CCR_ContinuityOfCare(doc, xmldoc);
                    Print_CCR_PatientDemographics(doc, xmldoc);
                    Prinit_CCR_Organization(doc, xmldoc);
                    doc.Close();
                    xmltext.Close();
                    PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                }
            }


            #endregion
        }
        public void PrintImplant(Document doc, XmlDocument xmldoc)
        {
            #region Implant
            sNoInformationInTable = string.Empty;
            DataSet dsImplant = null;
            XmlNodeList Doc_ImplantNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_ImplantNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "IMPLANTS")
                    {
                        //dsImplant = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsImplant = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsImplant = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }

                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsImplant != null)
            {
                PdfPTable patImplant = new PdfPTable(dsImplant.Tables[0].Columns.Count);
                patImplant.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Implants", HeadFont));
                HeadCell.Colspan = dsImplant.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patImplant.AddCell(HeadCell);

                for (int j = 0; j < dsImplant.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsImplant.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patImplant.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsImplant.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsImplant.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patImplant.AddCell(cell);
                        //   }

                    }
                }

                doc.Add(patImplant);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    for (int i = 0; i < dsImplant.Tables[0].Rows.Count; i++)
                    {
                        InHouseProcedure objInHouseProcedure = new InHouseProcedure();
                        objInHouseProcedure.GMDN_PT_Name = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("<tr><th>Implanted"));
                        //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                        objInHouseProcedure.Device_Identifier_UDI = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("UDI"));
                        lstImplant.Add(objInHouseProcedure);
                    }
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Implants", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["ImplantList"] = lstImplant;
            #endregion
        }

        public void PrintMentalStatus(Document doc, XmlDocument xmldoc)
        {
            #region MENTAL
            sNoInformationInTable = string.Empty;
            DataSet dsMentalStatus = null;
            XmlNodeList Doc_MentalStatusNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_MentalStatusNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "MENTAL STATUS")
                    {
                        //dsMentalStatus = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsMentalStatus = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsMentalStatus = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsMentalStatus != null && dsMentalStatus.Tables.Count > 0)
            {
                PdfPTable patMentalStatus = new PdfPTable(dsMentalStatus.Tables[0].Columns.Count);
                patMentalStatus.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Mental Status", HeadFont));
                HeadCell.Colspan = dsMentalStatus.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patMentalStatus.AddCell(HeadCell);

                for (int j = 0; j < dsMentalStatus.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsMentalStatus.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patMentalStatus.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsMentalStatus.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsMentalStatus.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patMentalStatus.AddCell(cell);
                        //   }

                    }
                }

                doc.Add(patMentalStatus);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    for (int i = 0; i < dsMentalStatus.Tables[0].Rows.Count; i++)
                    {
                        CarePlan objCarePlan = new CarePlan();
                        objCarePlan.Care_Name_Value = StripTagsRegex(dsMentalStatus.Tables[0].Rows[i].Field<string>("<tr><th>Status"));
                        //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                        objCarePlan.Plan_Date = StripTagsRegex(dsMentalStatus.Tables[0].Rows[i].Field<string>("Date"));
                        lstMentalStatus.Add(objCarePlan);
                    }
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Mental Status", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["MentalStatusList"] = lstMentalStatus;
            #endregion
        }


        public void PrintGoalsSection(Document doc, XmlDocument xmldoc)
        {
            #region Goals Section
            sNoInformationInTable = string.Empty;
            DataSet dsGoalsSection = null;
            XmlNodeList Doc_GoalsSectionNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_GoalsSectionNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "GOALS SECTION")
                    {
                        //dsGoalsSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsGoalsSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsGoalsSection = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsGoalsSection != null && dsGoalsSection.Tables.Count > 0)
            {
                PdfPTable patImplant = new PdfPTable(dsGoalsSection.Tables[0].Columns.Count);
                patImplant.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Goals Section", HeadFont));
                HeadCell.Colspan = dsGoalsSection.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patImplant.AddCell(HeadCell);

                for (int j = 0; j < dsGoalsSection.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsGoalsSection.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patImplant.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsGoalsSection.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsGoalsSection.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patImplant.AddCell(cell);
                        //   }

                    }
                }

                doc.Add(patImplant);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    for (int i = 0; i < dsGoalsSection.Tables[0].Rows.Count; i++)
                    {
                        TreatmentPlan objTreatmentPlan = new TreatmentPlan();
                        objTreatmentPlan.Plan = StripTagsRegex(dsGoalsSection.Tables[0].Rows[i].Field<string>("<tr><th>Goal"));
                        //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                        //objTreatmentPlan.Plan_Date = StripTagsRegex(dsGoalsSection.Tables[0].Rows[i].Field<string>("Date"));
                        lstGoalsSection.Add(objTreatmentPlan);
                    }
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Goals Section", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["GoalsSectionList"] = lstGoalsSection;
            #endregion
        }
        public void PrintImplantsection(Document doc, XmlDocument xmldoc)
        {
            #region implant  Section
            sNoInformationInTable = string.Empty;
            DataSet dsImplantSection = null;
            XmlNodeList Doc_GoalsSectionNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_GoalsSectionNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "IMPLANTS")
                    {
                        dsImplantSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);

                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsImplantSection != null && dsImplantSection.Tables.Count > 0)
            {
                PdfPTable patImplant = new PdfPTable(dsImplantSection.Tables[0].Columns.Count);
                patImplant.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Implants", HeadFont));
                HeadCell.Colspan = dsImplantSection.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patImplant.AddCell(HeadCell);

                for (int j = 0; j < dsImplantSection.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsImplantSection.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patImplant.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsImplantSection.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsImplantSection.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patImplant.AddCell(cell);
                        //   }

                    }
                }

                doc.Add(patImplant);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    for (int i = 0; i < dsImplantSection.Tables[0].Rows.Count; i++)
                    {
                        InHouseProcedure objInHouseProcedure = new InHouseProcedure();

                        objInHouseProcedure.GMDN_PT_Name = StripTagsRegex(dsImplantSection.Tables[0].Rows[i].Field<string>("<tr><th>Implanted"));
                        objInHouseProcedure.Device_Identifier_UDI = StripTagsRegex(dsImplantSection.Tables[0].Rows[i].Field<string>("<tr><th>UDI"));



                        lstImplant.Add(objInHouseProcedure);

                    }
                }
                catch
                {
                    //do nothing
                }
            }

            Session["ImplantList"] = lstImplant;
            #endregion
        }

        public void PrintHealthConcernSection(Document doc, XmlDocument xmldoc)
        {
            #region HealthConcern  Section
            sNoInformationInTable = string.Empty;
            DataSet dsHealthConcernSection = null;
            XmlNodeList Doc_GoalsSectionNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_GoalsSectionNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "HEALTH CONCERNS SECTION")
                    {
                        //dsHealthConcernSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsHealthConcernSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsHealthConcernSection = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsHealthConcernSection != null && dsHealthConcernSection.Tables.Count > 0)
            {
                for (int k = 0; k < dsHealthConcernSection.Tables.Count; k++)
                {
                    PdfPTable patImplant = new PdfPTable(dsHealthConcernSection.Tables[k].Columns.Count);
                    patImplant.WidthPercentage = 100;
                    if (k == 0)
                    {
                      
                        HeadCell = new PdfPCell(new Phrase("Health Concerns Section", HeadFont));
                        HeadCell.Colspan = dsHealthConcernSection.Tables[k].Columns.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patImplant.AddCell(HeadCell);
                    }
                    for (int j = 0; j < dsHealthConcernSection.Tables[k].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsHealthConcernSection.Tables[k].Columns[j].ColumnName);
                        //if (ColumnName.ToUpper() != "NDCID")
                        //{
                        cell = CreateCell(ColumnName, "", "");
                        patImplant.AddCell(cell);
                        // }
                    }

                    foreach (DataRow row in dsHealthConcernSection.Tables[k].Rows)
                    {
                        foreach (DataColumn column in dsHealthConcernSection.Tables[k].Columns)
                        {
                            //if (!row[column].ToString().Contains("NDCID"))
                            //{
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                            {
                                ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                            }

                            cell = CreateCell("", ColumnData, "");
                            patImplant.AddCell(cell);
                            //   }

                        }
                    }

                    doc.Add(patImplant);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                    try
                    {
                        for (int i = 0; i < dsHealthConcernSection.Tables[k].Rows.Count; i++)
                        {
                            ProblemList objProblemList = new ProblemList();
                            //objProblemList.Problem_Description = StripTagsRegex(dsHealthConcernSection.Tables[k].Rows[i].Field<string>("<tr><th>Observations"));
                            if (k == 0)
                            {
                                objProblemList.Problem_Description = StripTagsRegex(dsHealthConcernSection.Tables[k].Rows[i].Field<string>("<tr><th>Observations"));
                            }
                            else
                            {
                                objProblemList.Problem_Description = StripTagsRegex(dsHealthConcernSection.Tables[k].Rows[i].Field<string>("<tr><th>Concern"));
                            }
                            objProblemList.Status = "Active";
                            string diagdate = StripTagsRegex(dsHealthConcernSection.Tables[k].Rows[i].Field<string>("Date"));
                            if (diagdate.Length == 4)
                                objProblemList.Date_Diagnosed = diagdate;
                            else if (diagdate.Length == 9)
                                objProblemList.Date_Diagnosed = Convert.ToDateTime("01-" + diagdate.Split(',')[0].Trim() + "-" + diagdate.Split(',')[1].Trim()).ToString("dd-MM-yyyy");
                            else
                                objProblemList.Date_Diagnosed = Convert.ToDateTime(diagdate).ToString("dd-MM-yyyy");
                            //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                            //objTreatmentPlan.Plan_Date = StripTagsRegex(dsGoalsSection.Tables[0].Rows[i].Field<string>("Date"));

                            lsthealthconcern.Add(objProblemList);

                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Health Concerns Section", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["HealthConcernList"] = lsthealthconcern;
            #endregion
        }


        public void PrintInterventions(Document doc, XmlDocument xmldoc)
        {
            #region Interventions  Section
            sNoInformationInTable = string.Empty;
            DataSet dsInterventions = null;
            XmlNodeList Doc_GoalsSectionNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_GoalsSectionNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "INTERVENTIONS SECTION")
                    {
                        //dsHealthConcernSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsInterventions = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsInterventions = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsInterventions != null && dsInterventions.Tables.Count > 0)
            {
                for (int k = 0; k < dsInterventions.Tables.Count; k++)
                {
                    PdfPTable patImplant = new PdfPTable(dsInterventions.Tables[k].Columns.Count);
                    patImplant.WidthPercentage = 100;
                    if (k == 0)
                    {

                        HeadCell = new PdfPCell(new Phrase("Interventions Section", HeadFont));
                        HeadCell.Colspan = dsInterventions.Tables[k].Columns.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patImplant.AddCell(HeadCell);
                    }
                    for (int j = 0; j < dsInterventions.Tables[k].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsInterventions.Tables[k].Columns[j].ColumnName);
                        //if (ColumnName.ToUpper() != "NDCID")
                        //{
                        cell = CreateCell(ColumnName, "", "");
                        patImplant.AddCell(cell);
                        // }
                    }

                    foreach (DataRow row in dsInterventions.Tables[k].Rows)
                    {
                        foreach (DataColumn column in dsInterventions.Tables[k].Columns)
                        {
                            //if (!row[column].ToString().Contains("NDCID"))
                            //{
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                            {
                                ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                            }

                            cell = CreateCell("", ColumnData, "");
                            patImplant.AddCell(cell);
                            //   }

                        }
                    }

                    doc.Add(patImplant);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                    try
                    {
                        for (int i = 0; i < dsInterventions.Tables[k].Rows.Count; i++)
                        {
                            PatientResults objVitals = new PatientResults();

                            if ((StripTagsRegex(dsInterventions.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention"))).ToString() == "pulse oximetry monitoring")
                                objVitals.Loinc_Observation = "Pulse Oximetry";
                            else if (StripTagsRegex(dsInterventions.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention")) == "Oxygen administration by nasal cannula")
                                objVitals.Loinc_Observation = "Inhaled O2 Concentration";
                            else if (StripTagsRegex(dsInterventions.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention")) == "Elevate head of bed")
                                objVitals.Loinc_Observation = "Respiratory Rate";
                            else
                                objVitals.Loinc_Observation = StripTagsRegex((StripTagsRegex(dsInterventions.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention"))).ToString());
                            //string[] value_units = StripTagsRegex(Row_Value[c]).Split(' ');
                            //if (value_units[0].Contains("Ft"))
                            //{
                            //    string sFeet = ConvertFeetInchToInch(value_units[0].Remove(1), value_units[1].Remove(1));
                            //    objVitals.Value = sFeet;
                            //    objVitals.Units = "Ft Inch";

                            //}
                            //else
                            //{
                            //    objVitals.Value = value_units[0];
                            //    objVitals.Units = value_units[1];
                            //}
                            objVitals.Captured_date_and_time = Convert.ToDateTime((StripTagsRegex(dsInterventions.Tables[k].Rows[i].Field<string>("Date"))).ToString());
                            objVitals.Created_Date_And_Time = universalTime;
                            objVitals.Created_By = ClientSession.UserName;
                            objVitals.Results_Type = "Vitals";

                            lstvitals.Add(objVitals);

                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Interventions Section", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["Vitals"] = lstvitals;
            #endregion
        }

        public void PrintHealthStatusEvaluations(Document doc, XmlDocument xmldoc)
        {
            #region Health Status Section
            sNoInformationInTable = string.Empty;
            DataSet dsHealthStatus = null;
            XmlNodeList Doc_GoalsSectionNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_GoalsSectionNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "HEALTH STATUS EVALUATIONS/OUTCOMES SECTION")
                    {
                        //dsHealthConcernSection = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsHealthStatus = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsHealthStatus = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsHealthStatus != null && dsHealthStatus.Tables.Count > 0)
            {
                for (int k = 0; k < dsHealthStatus.Tables.Count; k++)
                {
                    PdfPTable patImplant = new PdfPTable(dsHealthStatus.Tables[k].Columns.Count);
                    patImplant.WidthPercentage = 100;
                    if (k == 0)
                    {

                        HeadCell = new PdfPCell(new Phrase("Health Status Evaluations/Outcomes Section", HeadFont));
                        HeadCell.Colspan = dsHealthStatus.Tables[k].Columns.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patImplant.AddCell(HeadCell);
                    }
                    for (int j = 0; j < dsHealthStatus.Tables[k].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsHealthStatus.Tables[k].Columns[j].ColumnName);
                        //if (ColumnName.ToUpper() != "NDCID")
                        //{
                        cell = CreateCell(ColumnName, "", "");
                        patImplant.AddCell(cell);
                        // }
                    }

                    foreach (DataRow row in dsHealthStatus.Tables[k].Rows)
                    {
                        foreach (DataColumn column in dsHealthStatus.Tables[k].Columns)
                        {
                            //if (!row[column].ToString().Contains("NDCID"))
                            //{
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                            {
                                ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                            }

                            cell = CreateCell("", ColumnData, "");
                            patImplant.AddCell(cell);
                            //   }

                        }
                    }

                    doc.Add(patImplant);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                    try
                    {
                        for (int i = 0; i < dsHealthStatus.Tables[k].Rows.Count; i++)
                        {
                            PatientResults objVitals = new PatientResults();

                            if ((StripTagsRegex(dsHealthStatus.Tables[k].Rows[i].Field<string>("<tr><th>Item"))).ToString() == "Pulse oximetry")
                                objVitals.Loinc_Observation = "Pulse Oximetry";
                            else if (StripTagsRegex(dsHealthStatus.Tables[k].Rows[i].Field<string>("<tr><th>Item")) == "Oxygen administration by nasal cannula")
                                objVitals.Loinc_Observation = "Inhaled O2 Concentration";
                            else if (StripTagsRegex(dsHealthStatus.Tables[k].Rows[i].Field<string>("<tr><th>Item")) == "Elvate head of bed")
                                objVitals.Loinc_Observation = "Respiratory Rate";
                            else
                                objVitals.Loinc_Observation = StripTagsRegex((StripTagsRegex(dsHealthStatus.Tables[k].Rows[i].Field<string>("<tr><th>Item"))).ToString());

                            objVitals.Value = StripTagsRegex((StripTagsRegex(dsHealthStatus.Tables[k].Rows[i].Field<string>("Outcome"))).ToString());
                            //string[] value_units = StripTagsRegex(Row_Value[c]).Split(' ');
                            //if (value_units[0].Contains("Ft"))
                            //{
                            //    string sFeet = ConvertFeetInchToInch(value_units[0].Remove(1), value_units[1].Remove(1));
                            //    objVitals.Value = sFeet;
                            //    objVitals.Units = "Ft Inch";

                            //}
                            //else
                            //{
                            //    objVitals.Value = value_units[0];
                            //    objVitals.Units = value_units[1];
                            //}
                            objVitals.Captured_date_and_time = Convert.ToDateTime((StripTagsRegex(dsHealthStatus.Tables[k].Rows[i].Field<string>("Date"))).ToString());
                            objVitals.Created_Date_And_Time = universalTime;
                            objVitals.Created_By = ClientSession.UserName;
                            objVitals.Results_Type = "Vitals";

                            lstvitals.Add(objVitals);
                        }
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("Health Status Evaluations/Outcomes Section", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["Vitals"] = lstvitals;
            #endregion
        }

        public void PrintAllergy(Document doc, XmlDocument xmldoc)
        {
            #region Allergy
            sNoInformationInTable = string.Empty;
            DataSet dsAllergy = null;
            XmlNodeList Doc_AllergyNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_AllergyNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "ALLERGIES, ADVERSE REACTIONS, ALERTS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ALLERGIES AND ADVERSE REACTIONS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ALLERGIES"))
                    {
                        //dsAllergy = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsAllergy = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsAllergy = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }

                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsAllergy != null && dsAllergy.Tables.Count > 0)
            {
                PdfPTable patAllergy = new PdfPTable(dsAllergy.Tables[0].Columns.Count);
                patAllergy.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("ALLERGY", HeadFont));
                HeadCell.Colspan = dsAllergy.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patAllergy.AddCell(HeadCell);

                for (int j = 0; j < dsAllergy.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsAllergy.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patAllergy.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsAllergy.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsAllergy.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patAllergy.AddCell(cell);
                        //   }

                    }
                }

                doc.Add(patAllergy);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    for (int i = 0; i < dsAllergy.Tables[0].Rows.Count; i++)
                    {
                        Rcopia_Allergy objallergy = new Rcopia_Allergy();
                        objallergy.Allergy_Name = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("<tr><th>Substance"));
                        objallergy.Reaction = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Reaction"));
                        // objallergy.NDC_ID = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("NDCID"));
                        objallergy.Created_Date_And_Time = universalTime;
                        objallergy.Created_By = ClientSession.UserName;
                        lstallergy.Add(objallergy);
                    }
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("ALLERGY", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["AllergyList"] = lstallergy;
            #endregion
        }

        public void PrintEncounter(Document doc, XmlDocument xmldoc)
        {

            #region Encounter
            sNoInformationInTable = string.Empty;
            DataSet dsEncounter = null;
            XmlNodeList Doc_Encounter_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Encounter_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS")
                    {
                        //dsEncounter = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsEncounter = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsEncounter = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsEncounter != null)
            {
                PdfPTable patEncounter = new PdfPTable(dsEncounter.Tables[0].Columns.Count);
                patEncounter.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("ENCOUNTER", HeadFont));
                HeadCell.Colspan = dsEncounter.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patEncounter.AddCell(HeadCell);


                for (int j = 0; j < dsEncounter.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsEncounter.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patEncounter.AddCell(cell);
                }

                foreach (DataRow row in dsEncounter.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsEncounter.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        //if (column.ColumnName == "Date" && ColumnData != string.Empty)
                        //{
                        //string year = ColumnData.Substring(0, 4);
                        //string month = ColumnData.Substring(4, 2);
                        //string date = ColumnData.Substring(6, 2);
                        //ColumnData = year + "-" + month + "-" + date;
                        //}
                        cell = CreateCell("", ColumnData, "");
                        patEncounter.AddCell(cell);
                    }
                }
                doc.Add(patEncounter);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("ENCOUNTER", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            #endregion
        }

        public void PrintImmunization(Document doc, XmlDocument xmldoc)
        {

            #region Immunization
            sNoInformationInTable = string.Empty;
            DataSet dsImmunization = null;
            XmlNodeList Doc_Immunization_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Immunization_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "IMMUNIZATIONS")
                    {
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                        {
                            dsImmunization = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        }
                        else if (elemParent.ChildNodes[i + 1].InnerXml.IndexOf("<table") > -1)
                        {
                            int start = elemParent.ChildNodes[i + 1].InnerXml.IndexOf("<table");
                            int end = elemParent.ChildNodes[i + 1].InnerXml.IndexOf("</table>");

                            dsImmunization = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml.Substring(start));
                        }
                        else
                        {
                            dsImmunization = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }


                        is_break = true;
                        break;

                    }
                }
                if (is_break == true)
                    break;
            }

            try
            {
                if (dsImmunization != null && dsImmunization.Tables.Count > 0)
                {
                    PdfPTable patImmunization = new PdfPTable(dsImmunization.Tables[0].Columns.Count);
                    patImmunization.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("IMMUNIZATION", HeadFont));
                    HeadCell.Colspan = dsImmunization.Tables[0].Columns.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patImmunization.AddCell(HeadCell);


                    for (int j = 0; j < dsImmunization.Tables[0].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsImmunization.Tables[0].Columns[j].ColumnName);
                        cell = CreateCell(ColumnName, "", "");
                        patImmunization.AddCell(cell);
                    }

                    foreach (DataRow row in dsImmunization.Tables[0].Rows)
                    {
                        foreach (DataColumn column in dsImmunization.Tables[0].Columns)
                        {
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            //Added By Vaishali for Bug Id:28529
                            if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                            {
                                ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                            }
                            if (ColumnData != "00010101")
                            {
                                ColumnData.TrimEnd(',');
                                cell = CreateCell("", ColumnData, "");
                                patImmunization.AddCell(cell);
                            }
                            else
                            {
                                cell = CreateCell("", string.Empty, "");
                                patImmunization.AddCell(cell);
                            }
                        }
                    }
                    doc.Add(patImmunization);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                    try
                    {
                        for (int i = 0; i < dsImmunization.Tables[0].Rows.Count; i++)
                        {
                            Immunization objimmunization = new Immunization();
                            objimmunization.Immunization_Description = StripTagsRegex(dsImmunization.Tables[0].Rows[i].Field<string>("<tr><th>Vaccine"));
                            objimmunization.Created_Date_And_Time = universalTime;
                            objimmunization.Created_By = ClientSession.UserName;
                            string date = StripTagsRegex(dsImmunization.Tables[0].Rows[i].Field<string>("Date"));
                            string year = string.Empty;
                            string month = string.Empty;
                            string date_1 = string.Empty;
                            try
                            {
                                if (date != "Unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                                {
                                    year = date.Substring(0, 4);
                                    month = date.Substring(4, 2);
                                    date_1 = date.Substring(6, 2);
                                    objimmunization.Given_Date = Convert.ToDateTime(year + "-" + month + "-" + date_1);
                                }
                            }
                            catch
                            {
                                if (date != "Unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                                {
                                    if (date.Contains(' '))
                                    {
                                        string[] strDate = date.Split(' ');
                                        if (strDate.Length == 3)
                                        {
                                            date_1 = strDate[0];
                                            month = strDate[1];
                                            year = strDate[2];
                                            objimmunization.Given_Date = Convert.ToDateTime(year + "-" + month + "-" + date_1);
                                        }
                                        else if (strDate.Length == 2)
                                        {
                                            month = strDate[0];
                                            year = strDate[1];
                                            objimmunization.Given_Date = Convert.ToDateTime(year + "-" + month + "-" + "01");
                                        }
                                        else
                                        {
                                            year = strDate[0];
                                            objimmunization.Given_Date = Convert.ToDateTime(year + "-" + "01" + "-" + "01");
                                        }
                                    }
                                }

                            }
                            lstimmunization.Add(objimmunization);
                        }
                    }
                    catch
                    {
                        //do nothing
                    }

                }
                else
                {
                    PdfPTable emptyPDF = new PdfPTable(1);
                    emptyPDF.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("IMMUNIZATION", HeadFont));
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    emptyPDF.AddCell(HeadCell);
                    if (sNoInformationInTable == string.Empty)
                    {
                        cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                    }
                    else
                    {
                        cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                    }
                    emptyPDF.AddCell(cell);
                    doc.Add(emptyPDF);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));
                }

                Session["ImmunizationList"] = lstimmunization;
            #endregion

            }
            catch
            {

            }

        }

        public void PrintMedication(Document doc, XmlDocument xmldoc)
        {

            #region Medication
            sNoInformationInTable = string.Empty;
            DataSet dsMedication = null;
            XmlNodeList Doc_Medication_Node = xmldoc.GetElementsByTagName("section");
            XmlElement medItemDoc = null;
            foreach (XmlElement elemParent in Doc_Medication_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "MEDICATIONS")
                    {

                        medItemDoc = elemParent;
                        //dsMedication = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                        {
                            dsMedication = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);

                        }
                        else
                        {
                            dsMedication = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsMedication != null && dsMedication.Tables.Count > 0)
            {
                PdfPTable patMedication = new PdfPTable(dsMedication.Tables[0].Columns.Count);
                patMedication.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("MEDICATIONS", HeadFont));
                HeadCell.Colspan = dsMedication.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patMedication.AddCell(HeadCell);


                for (int j = 0; j < dsMedication.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsMedication.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patMedication.AddCell(cell);
                }

                foreach (DataRow row in dsMedication.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsMedication.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        //added by vaishali for bug ID 28530
                        if ((ColumnData.EndsWith("()")) && ColumnData.Length > 0)
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 2);
                        }
                        if (ColumnData != "00010101" && ColumnData != "January 01,0001")
                        {
                            cell = CreateCell("", ColumnData, "");
                            patMedication.AddCell(cell);
                        }
                        else
                        {
                            cell = CreateCell("", string.Empty, "");
                            patMedication.AddCell(cell);
                        }
                        //if (column.ColumnName == "Start Date" && ColumnData != string.Empty && ColumnData.ToUpper() != "UNKNOWN")
                        //{
                        //    string year = ColumnData.Substring(0, 4);
                        //    string month = ColumnData.Substring(4, 2);
                        //    string date = ColumnData.Substring(6, 2);
                        //    ColumnData = year + "-" + month + "-" + date;
                        //}

                    }
                }
                doc.Add(patMedication);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));


                try
                {
                    for (int i = 0; i < dsMedication.Tables[0].Rows.Count; i++)
                    {
                        Rcopia_Medication objmedication = new Rcopia_Medication();
                        objmedication.Generic_Name = StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("<tr><th>Medication"));
                        string[] split = objmedication.Generic_Name.Split('[');
                        if (split.Length > 1)
                            objmedication.Brand_Name = split[1].Replace("]", "");
                        string date = StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Start Date"));
                        if (date != "Unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                        {
                            string year = date.Substring(0, 4);
                            string month = date.Substring(4, 2);
                            string date_1 = date.Substring(6, 2);
                            objmedication.Start_Date = Convert.ToDateTime(year + "-" + month + "-" + date_1);
                        }
                        //date = StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("End Date"));
                        //if (date != "unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                        //{
                        //    string year = date.Substring(0, 4);
                        //    string month = date.Substring(4, 2);
                        //    string date_1 = date.Substring(6, 2);
                        //    objmedication.Stop_Date = Convert.ToDateTime(year + "-" + month + "-" + date_1);
                        //}
                        if (StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS NEEDED" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS NEEDED FOR PAIN" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "WITH MEALS" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS DIRECTED" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "BETWEEN MEALS" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "ONE HOUR BEFORE MEALS" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "BEFORE EXERCISE" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "WITH A GLASS OF WATER" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AFTER MEALS")
                        {

                            objmedication.Dose_Other = StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions"));
                        }
                        else
                        {
                            objmedication.Dose_Timing = StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions"));
                        }

                        //dosevalue
                        try
                        {
                            objmedication.Dose = medItemDoc.GetElementsByTagName("doseQuantity")[i].Attributes.GetNamedItem("value").Value;
                        }
                        catch
                        {
                        }
                        objmedication.Deleted = "N";
                        objmedication.Created_Date_And_Time = universalTime;
                        objmedication.Created_By = ClientSession.UserName;
                        lstmedication.Add(objmedication);
                    }
                }
                catch
                {
                    //do nothing
                }

            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("MEDICATIONS", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            Session["MedicationList"] = lstmedication;
            #endregion
        }

        public void PrintCarePlan(Document doc, XmlDocument xmldoc)
        {

            #region Care Plan
            sNoInformationInTable = string.Empty;
            DataSet dsCarePlan = null;
            XmlNodeList Doc_CarePlan_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_CarePlan_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "CARE PLAN" || elemParent.ChildNodes[i].InnerText.ToUpper() == "PLAN OF CARE"))
                    {
                        // dsCarePlan = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsCarePlan = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsCarePlan = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsCarePlan != null && dsCarePlan.Tables.Count > 0)
            {
                PdfPTable patCarePlan = new PdfPTable(dsCarePlan.Tables[0].Columns.Count);
                patCarePlan.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("CARE PLAN", HeadFont));
                HeadCell.Colspan = dsCarePlan.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patCarePlan.AddCell(HeadCell);


                for (int j = 0; j < dsCarePlan.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsCarePlan.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patCarePlan.AddCell(cell);
                }

                foreach (DataRow row in dsCarePlan.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsCarePlan.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        //if (column.ColumnName == "Planned Date" && ColumnData != string.Empty)
                        //{
                        //    string year = ColumnData.Substring(0, 4);
                        //    string month = ColumnData.Substring(4, 2);
                        //    string date = ColumnData.Substring(6, 2);
                        //    ColumnData = year + "-" + month + "-" + date;
                        //}
                        cell = CreateCell("", ColumnData, "");
                        patCarePlan.AddCell(cell);
                    }
                }
                doc.Add(patCarePlan);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));


                string plan = string.Empty;
                for (int i = 0; i < dsCarePlan.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (plan == string.Empty)
                            plan = StripTagsRegex(dsCarePlan.Tables[0].Rows[i].Field<string>("<tr><th>Planned Activity")); // + " " + dsCarePlan.Tables[0].Rows[i].Field<string>("Planned Date"));
                        else
                            plan = StripTagsRegex(plan + "*" + dsCarePlan.Tables[0].Rows[i].Field<string>("<tr><th>Planned Activity")); //+ " " + dsCarePlan.Tables[0].Rows[i].Field<string>("Planned Date"));
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                if (plan != string.Empty)
                {
                    try
                    {
                        TreatmentPlan objTreatmentPlan = new TreatmentPlan();
                        objTreatmentPlan.Plan = plan;
                        objTreatmentPlan.Plan_Type = "PLAN";
                        objTreatmentPlan.Created_Date_And_Time = universalTime;
                        objTreatmentPlan.Created_By = ClientSession.UserName;
                        lsttreatmentplan.Add(objTreatmentPlan);
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("CARE PLAN", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["TreatmentPlanList"] = lsttreatmentplan;
            #endregion
        }

        public void PrintHospitalMedications(Document doc, XmlDocument xmldoc)
        {

            #region Hospital Medications
            sNoInformationInTable = string.Empty;
            DataSet dsHospitalMedications = null;
            XmlNodeList Doc_HospitalMedications_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_HospitalMedications_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "HOSPITAL DISCHARGE MEDICATIONS")
                    {
                        dsHospitalMedications = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsHospitalMedications != null && dsHospitalMedications.Tables.Count > 0)
            {
                PdfPTable patHospitalMedications = new PdfPTable(dsHospitalMedications.Tables[0].Columns.Count);
                patHospitalMedications.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("HOSPITAL DISCHARGE MEDICATIONS", HeadFont));
                HeadCell.Colspan = dsHospitalMedications.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patHospitalMedications.AddCell(HeadCell);


                for (int j = 0; j < dsHospitalMedications.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsHospitalMedications.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patHospitalMedications.AddCell(cell);
                }

                foreach (DataRow row in dsHospitalMedications.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsHospitalMedications.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        //if (column.ColumnName == "Start Date" && ColumnData != string.Empty)
                        //{
                        //    string year = ColumnData.Substring(0, 4);
                        //    string month = ColumnData.Substring(4, 2);
                        //    string date = ColumnData.Substring(6, 2);
                        //    ColumnData = year + "-" + month + "-" + date;
                        //}
                        cell = CreateCell("", ColumnData, "");
                        patHospitalMedications.AddCell(cell);
                    }
                }
                doc.Add(patHospitalMedications);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            #endregion
        }

        public void PrintReasonForReferal(Document doc, XmlDocument xmldoc)
        {

            #region Reason for Referal
            sNoInformationInTable = string.Empty;
            PdfPTable patReferal = new PdfPTable(new float[] { 900 });
            patReferal.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("REASON FOR REFERRAL", HeadFont));
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patReferal.AddCell(HeadCell);


            XmlNodeList Doc_ReferalReason_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_ReferalReason_Node)
            {
                bool is_break = false;
                bool is_Reason = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "REASON FOR REFERRAL")
                    {
                        is_Reason = true;
                    }
                    if (elemParent.ChildNodes[i].Name == "text" && is_Reason == true)
                    {
                        string reason = string.Empty;
                        if (elemParent.ChildNodes[i].ChildNodes != null && elemParent.ChildNodes[i].ChildNodes.Count == 0)
                        {
                            reason = elemParent.ChildNodes[i].InnerXml;
                        }
                        else
                        {
                            for (int j = 0; j < elemParent.ChildNodes[i].ChildNodes.Count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "paragraph")
                                {
                                    reason = elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                    break;
                                }
                            }
                        }
                        if (reason.Trim() == string.Empty && elemParent.ChildNodes[i].ChildNodes != null && elemParent.ChildNodes[i].ChildNodes.Count == 1)
                        {
                            reason = elemParent.ChildNodes[i].InnerXml;
                        }

                        if (reason != string.Empty)
                        {
                            cell = CreateCell("", "     " + StripTagsRegex(reason), "");
                            patReferal.AddCell(cell);
                            doc.Add(patReferal);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));

                            try
                            {
                                ReferralOrder objmanager = new ReferralOrder();
                                string reasontext = StripTagsRegex(reason);
                                objmanager.Reason_For_Referral = reasontext.Replace("\n", " ");
                                objmanager.Referral_Date = UtilityManager.ConvertToUniversal();
                                objmanager.Created_Date_And_Time = universalTime;
                                objmanager.Created_By = ClientSession.UserName;
                                lstreferalorder.Add(objmanager);
                            }
                            catch
                            {
                                //do nothing
                            }

                        }
                        else
                        {
                            sNoInformationInTable = elemParent.ChildNodes[i].InnerXml;
                            if (sNoInformationInTable == string.Empty)
                            {
                                cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                            }
                            else
                            {
                                cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                            }
                            patReferal.AddCell(cell);
                            doc.Add(patReferal);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            Session["ReferalOrderList"] = lstreferalorder;

            #endregion

        }

        public void PrintProcedures(Document doc, XmlDocument xmldoc)
        {
            #region Procedure
            sNoInformationInTable = string.Empty;
            string[] sstatuscode1 = new string[] { };
            DataSet dsProcedure = null;
            XmlNodeList Doc_Procedure_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Procedure_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "PROCEDURES")
                    {
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsProcedure = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsProcedure = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        //Dictionary<string, string> dictStatus = new Dictionary<string, string>();
                        try
                        {
                            int tsr = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml).Tables[0].Rows.Count;
                            string[] sstatuscode = new string[tsr];
                            for (int k = 1; k <= ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml).Tables[0].Rows.Count; k++)
                            {
                                string svalue = elemParent.ChildNodes[i + 1 + k].LastChild.ChildNodes[3].Attributes[0].Value;
                                string sname = elemParent.ChildNodes[i + 1 + k].LastChild.ChildNodes[3].Attributes[0].OwnerElement.Name;
                                if (sname == "statusCode")
                                {
                                    sstatuscode[k - 1] = svalue;
                                    //dictStatus.Add(sname+k, svalue);
                                }
                            }
                            sstatuscode1 = new string[] { };
                            sstatuscode1 = sstatuscode;
                        }
                        catch
                        {
                            dsProcedure = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }
            if (dsProcedure != null && dsProcedure.Tables.Count > 0)
            {
                PdfPTable patProcedure = new PdfPTable((dsProcedure.Tables[0].Columns.Count + 1));
                patProcedure.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("PROCEDURES", HeadFont));
                HeadCell.Colspan = dsProcedure.Tables[0].Columns.Count + 1;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patProcedure.AddCell(HeadCell);


                for (int j = 0; j < dsProcedure.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsProcedure.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patProcedure.AddCell(cell);
                }
                if (sstatuscode1.Length > 0)
                {
                    cell = CreateCell("Status", "", "");
                    patProcedure.AddCell(cell);
                }

                int y = 0;
                foreach (DataRow row in dsProcedure.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsProcedure.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        cell = CreateCell("", ColumnData, "");
                        patProcedure.AddCell(cell);
                    }
                    if (sstatuscode1.Length > 0)
                    {
                        cell = CreateCell("", sstatuscode1[y], "");
                        patProcedure.AddCell(cell);
                    }
                    y++;
                }
                doc.Add(patProcedure);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                for (int i = 0; i < dsProcedure.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        InHouseProcedure objprocedure = new InHouseProcedure();
                        objprocedure.Procedure_Code_Description = StripTagsRegex(dsProcedure.Tables[0].Rows[i].Field<string>("<tr><th>Procedure"));
                        objprocedure.Created_By = ClientSession.UserName;
                        string date = StripTagsRegex(dsProcedure.Tables[0].Rows[i].Field<string>("Date"));//added for 30254
                        if (date != "unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                        {

                            if (date.Length > 8)
                            {
                                string year = date.Substring(0, 4);
                                string month = date.Substring(4, 2);
                                string date_1 = date.Substring(6, 2);
                                objprocedure.Created_Date_And_Time = Convert.ToDateTime(year + "-" + month + "-" + date_1);
                            }
                            else if (date.Length < 8 && date.Length == 6)
                            {
                                string year = date.Substring(0, 4);
                                string month = date.Substring(4, 2);
                                objprocedure.Created_Date_And_Time = Convert.ToDateTime(year + "-" + month + "-" + "01");
                            }
                            else if (date.Length < 8 && date.Length == 4)
                            {
                                string year = date.Substring(0, 4);
                                objprocedure.Created_Date_And_Time = Convert.ToDateTime(year + "-" + "01" + "-" + "01");
                            }
                        } //*
                        lstprocedures.Add(objprocedure);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("PROCEDURES", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }


            Session["ProcedureList"] = lstprocedures;
            #endregion

        }

        public void PrintFunctionalStatus(Document doc, XmlDocument xmldoc)
        {

            #region Functional And Congnitive Status
            sNoInformationInTable = string.Empty;
            DataSet dsfunctional = null;
            XmlNodeList Doc_Functional_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Functional_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "FUNCTIONAL STATUS")
                    {
                        // dsfunctional = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);

                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsfunctional = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsfunctional = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsfunctional != null && dsfunctional.Tables.Count > 0)
            {
                PdfPTable patFunction = new PdfPTable(dsfunctional.Tables[0].Columns.Count);
                patFunction.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("FUNCTIONAL STATUS", HeadFont));
                HeadCell.Colspan = dsfunctional.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patFunction.AddCell(HeadCell);


                for (int j = 0; j < dsfunctional.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsfunctional.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patFunction.AddCell(cell);
                }

                foreach (DataRow row in dsfunctional.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsfunctional.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        cell = CreateCell("", ColumnData, "");
                        patFunction.AddCell(cell);
                    }
                }
                doc.Add(patFunction);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));



                for (int i = 0; i < dsfunctional.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        CarePlan objCarePlan = new CarePlan();
                        objCarePlan.Care_Name = "Functional Condition";
                        objCarePlan.Care_Name_Value = StripTagsRegex(dsfunctional.Tables[0].Rows[i].Field<string>("<tr><th>Functional Condition"));
                        objCarePlan.Status = StripTagsRegex(dsfunctional.Tables[0].Rows[i].Field<string>("Condition Status"));
                        string date = StripTagsRegex(dsfunctional.Tables[0].Rows[i].Field<string>("Effective Dates"));//added for 30257
                        if (date != "unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                        {

                            if (date.Length > 8)
                            {
                                string year = date.Substring(0, 4);
                                string month = date.Substring(4, 2);
                                string date_1 = date.Substring(6, 2);
                                objCarePlan.Plan_Date = year + "-" + month + "-" + date_1;
                            }
                            else if (date.Length < 8 && date.Length == 6)
                            {
                                string year = date.Substring(0, 4);
                                string month = date.Substring(4, 2);

                                objCarePlan.Plan_Date = year + "-" + month;
                            }
                            else if (date.Length < 8 && date.Length == 4)
                            {
                                string year = date.Substring(0, 4);
                                objCarePlan.Plan_Date = year;
                            }
                        }
                        objCarePlan.Created_Date_And_Time = universalTime;
                        objCarePlan.Created_By = ClientSession.UserName;
                        lstcareplan.Add(objCarePlan);
                    }
                    catch
                    {
                        //do nothing
                    }
                }

            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("FUNCTIONAL STATUS", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }


            #endregion
            Session["CarePlanList"] = lstcareplan;
        }

        public void PrintResults(Document doc, XmlDocument xmldoc)
        {
            try
            {
                #region CCD_Results

                sNoInformationInTable = string.Empty;
                if (Request["FileName"] != null && (Request["Type"] == "CCD" || Request["Type"] == "C32"))
                {
                    DataSet dsResults = null;
                    XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                    foreach (XmlElement elemParent in Doc_Results_Node)
                    {
                        bool is_break = false;
                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "RESULTS")
                            {
                                if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                                {
                                    dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                }
                                else
                                {
                                    dsResults = null;
                                    sNoInformationInTable = string.Empty;
                                    sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                                }
                                is_break = true;
                                break;

                            }
                        }
                        if (is_break == true)
                            break;
                    }

                    if (dsResults != null && dsResults.Tables.Count > 0)
                    {
                        PdfPTable patResults = new PdfPTable(dsResults.Tables[0].Columns.Count);
                        patResults.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                        HeadCell.Colspan = dsResults.Tables[0].Columns.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patResults.AddCell(HeadCell);


                        for (int j = 0; j < dsResults.Tables[0].Columns.Count; j++)
                        {
                            string ColumnName = StripTagsRegex(dsResults.Tables[0].Columns[j].ColumnName);
                            cell = CreateCell(ColumnName, "", "");
                            patResults.AddCell(cell);
                        }

                        foreach (DataRow row in dsResults.Tables[0].Rows)
                        {
                            foreach (DataColumn column in dsResults.Tables[0].Columns)
                            {
                                string ColumnData = StripTagsRegex(row[column].ToString());
                                if (ColumnData.Contains("&lt;"))
                                    ColumnData = ColumnData.Replace("&lt;", "<");
                                else if (ColumnData.Contains("&gt;"))
                                    ColumnData = ColumnData.Replace("&gt;", ">");
                                cell = CreateCell("", ColumnData, "");
                                patResults.AddCell(cell);
                            }
                        }
                        doc.Add(patResults);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));

                        ResultMaster objmaster = new ResultMaster();
                        objmaster.Is_Electronic_Mode = "N";
                        objmaster.Created_Date_And_Time = universalTime;
                        objmaster.Created_By = ClientSession.UserName;
                        lstresultmaster.Add(objmaster);


                        ResultOBR objobr = new ResultOBR();
                        objobr.OBR_Observation_Battery_Text = "Blood chemistry";
                        objobr.Created_Date_And_Time = universalTime;
                        objobr.Created_By = ClientSession.UserName;
                        objobr.OBR_Specimen_Collection_Date_And_Time = universalTime.ToString("yyyyMMddhhmmss");
                        lstresultobr.Add(objobr);


                        ResultORC objorc = new ResultORC();
                        objorc.Created_Date_And_Time = universalTime;
                        objorc.Created_By = ClientSession.UserName;
                        lstresultorc.Add(objorc);

                        for (int i = 0; i < dsResults.Tables[0].Rows.Count; i++)
                        {

                            ResultOBX objresult = new ResultOBX();
                            string order = StripTagsRegex(dsResults.Tables[0].Rows[i][0].ToString());
                            if (order != string.Empty)
                            {
                                try
                                {
                                    string[] orders = order.Split(' ');
                                    objresult.OBX_Loinc_Observation_Text = orders[0];
                                    if (order.Contains('(') == true)
                                    {
                                        string strOrder = order.Substring(order.IndexOf('('), order.Length - order.IndexOf('('));

                                        objresult.OBX_Reference_Range = strOrder;

                                    }
                                    //if (orders.Length >= 4)//for bug id 30256
                                    //{
                                    //    objresult.OBX_Reference_Range = orders[1] + " " + orders[2]+" "+orders[3];
                                    //}
                                    //else if (orders.Length >= 3)
                                    //{
                                    //    objresult.OBX_Reference_Range = orders[1] +" "+ orders[2];
                                    //} 
                                    else
                                    {
                                        objresult.OBX_Reference_Range = orders[1];
                                    }
                                    order = StripTagsRegex(dsResults.Tables[0].Rows[i].Field<string>("Blood chemistry"));
                                    orders = order.Split(' ');
                                    objresult.OBX_Observation_Value = orders[0];
                                    objresult.Created_Date_And_Time = universalTime;
                                    objresult.Created_By = ClientSession.UserName;
                                    lstresultobx.Add(objresult);
                                }
                                catch
                                {
                                    //do nothing
                                }
                            }
                        }

                    }
                    else
                    {
                        PdfPTable emptyPDF = new PdfPTable(1);
                        emptyPDF.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        emptyPDF.AddCell(HeadCell);
                        if (sNoInformationInTable == string.Empty)
                        {
                            cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                        }
                        else
                        {
                            cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                        }
                        emptyPDF.AddCell(cell);
                        doc.Add(emptyPDF);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }

                    Session["ResultMasterList"] = lstresultmaster;
                    Session["ResultObrList"] = lstresultobr;
                    Session["ResultObxList"] = lstresultobx;
                    Session["ResultOrcList"] = lstresultorc;
                }

                #endregion

                #region Print C32 Results
                else
                {
                    XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                    foreach (XmlElement elemParent in Doc_Results_Node)
                    {
                        bool is_break = false;
                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "TEST RESULTS")
                            {
                                int count = elemParent.ChildNodes[i + 1].ChildNodes.Count;
                                for (int j = 0; j < count; j++)
                                {
                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "paragraph" && elemParent.ChildNodes[i + 1].ChildNodes[j].InnerXml == "Lab Results")
                                    {
                                        IList<string> ColumnHead = new List<string>();
                                        IList<string> RowValue = new List<string>();
                                        int temp_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes.Count;
                                        for (int k = 0; k < temp_count; k++)
                                        {
                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "thead")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "th")
                                                            {
                                                                ColumnHead.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            else if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "tbody")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "td")
                                                            {
                                                                RowValue.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (ColumnHead.Count > 0)
                                        {
                                            PdfPTable patResults = new PdfPTable(ColumnHead.Count);
                                            patResults.WidthPercentage = 100;
                                            HeadCell = new PdfPCell(new Phrase("LAB RESULTS", HeadFont));
                                            HeadCell.Colspan = ColumnHead.Count;
                                            HeadCell.HorizontalAlignment = 1;
                                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                            patResults.AddCell(HeadCell);


                                            for (int x = 0; x < ColumnHead.Count; x++)
                                            {
                                                string ColumnName = StripTagsRegex(ColumnHead[x]);
                                                cell = CreateCell(ColumnName, "", "");
                                                patResults.AddCell(cell);
                                            }

                                            for (int k = 0; k < RowValue.Count; k++)
                                            {
                                                string ColumnName = StripTagsRegex(RowValue[k]);
                                                if (ColumnName.Contains("&lt;"))
                                                    ColumnName = ColumnName.Replace("&lt;", "<");
                                                else if (ColumnName.Contains("&gt;"))
                                                    ColumnName = ColumnName.Replace("&gt;", ">");
                                                cell = CreateCell("", ColumnName, "");
                                                patResults.AddCell(cell);
                                            }

                                            doc.Add(patResults);
                                            doc.Add(new Paragraph("   "));
                                            doc.Add(new Paragraph("   "));

                                        }


                                    }
                                    else if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "paragraph" && elemParent.ChildNodes[i + 1].ChildNodes[j].InnerXml == "Diagnostic Results")
                                    {
                                        IList<string> ColumnHead = new List<string>();
                                        IList<string> RowValue = new List<string>();
                                        int temp_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes.Count;
                                        for (int k = 0; k < temp_count; k++)
                                        {
                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "thead")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "th")
                                                            {
                                                                ColumnHead.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            else if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "tbody")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "td")
                                                            {
                                                                RowValue.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (ColumnHead.Count > 0)
                                        {
                                            PdfPTable patResults = new PdfPTable(ColumnHead.Count);
                                            patResults.WidthPercentage = 100;
                                            HeadCell = new PdfPCell(new Phrase("DIAGNOSTIC RESULTS", HeadFont));
                                            HeadCell.Colspan = ColumnHead.Count;
                                            HeadCell.HorizontalAlignment = 1;
                                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                            patResults.AddCell(HeadCell);


                                            for (int x = 0; x < ColumnHead.Count; x++)
                                            {
                                                string ColumnName = StripTagsRegex(ColumnHead[x]);
                                                cell = CreateCell(ColumnName, "", "");
                                                patResults.AddCell(cell);
                                            }

                                            for (int k = 0; k < RowValue.Count; k++)
                                            {
                                                string ColumnName = StripTagsRegex(RowValue[k]);
                                                cell = CreateCell("", ColumnName, "");
                                                patResults.AddCell(cell);
                                            }

                                            doc.Add(patResults);
                                            doc.Add(new Paragraph("   "));
                                            doc.Add(new Paragraph("   "));

                                        }
                                    }
                                }
                                is_break = true;
                                break;
                            }
                        }
                        if (is_break == true)
                            break;
                    }
                }

                #endregion

            }
            catch
            {
                DataSet dsResults = null;
                XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                foreach (XmlElement elemParent in Doc_Results_Node)
                {
                    bool is_break = false;
                    for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "RESULTS")
                        {
                            dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            is_break = true;
                            break;
                        }
                    }
                    if (is_break == true)
                        break;
                }

                if (dsResults != null && dsResults.Tables.Count > 0)
                {
                    PdfPTable patResults = new PdfPTable(dsResults.Tables[0].Columns.Count);
                    patResults.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                    HeadCell.Colspan = dsResults.Tables[0].Columns.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patResults.AddCell(HeadCell);


                    for (int j = 0; j < dsResults.Tables[0].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsResults.Tables[0].Columns[j].ColumnName);
                        cell = CreateCell(ColumnName, "", "");
                        patResults.AddCell(cell);
                    }

                    foreach (DataRow row in dsResults.Tables[0].Rows)
                    {
                        foreach (DataColumn column in dsResults.Tables[0].Columns)
                        {
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if (ColumnData.Contains("&lt;"))
                                ColumnData = ColumnData.Replace("&lt;", "<");
                            else if (ColumnData.Contains("&gt;"))
                                ColumnData = ColumnData.Replace("&gt;", ">");
                            cell = CreateCell("", ColumnData, "");
                            patResults.AddCell(cell);
                        }
                    }
                    doc.Add(patResults);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                }
            }

        }
        public void PrintResults(Document doc, XmlDocument xmldoc,string fileName,string Type)
        {
            try
            {
                #region CCD_Results

                sNoInformationInTable = string.Empty;
                if (fileName != null && (Type == "CCD" || Type == "C32"))
                {
                    DataSet dsResults = null;
                    XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                    foreach (XmlElement elemParent in Doc_Results_Node)
                    {
                        bool is_break = false;
                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "RESULTS")
                            {
                                if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                                {
                                    dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml,Type);
                                }
                                else
                                {
                                    dsResults = null;
                                    sNoInformationInTable = string.Empty;
                                    sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                                }
                                is_break = true;
                                break;

                            }
                        }
                        if (is_break == true)
                            break;
                    }

                    if (dsResults != null && dsResults.Tables.Count > 0)
                    {
                        PdfPTable patResults = new PdfPTable(dsResults.Tables[0].Columns.Count);
                        patResults.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                        HeadCell.Colspan = dsResults.Tables[0].Columns.Count;
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        patResults.AddCell(HeadCell);


                        for (int j = 0; j < dsResults.Tables[0].Columns.Count; j++)
                        {
                            string ColumnName = StripTagsRegex(dsResults.Tables[0].Columns[j].ColumnName);
                            cell = CreateCell(ColumnName, "", "");
                            patResults.AddCell(cell);
                        }

                        foreach (DataRow row in dsResults.Tables[0].Rows)
                        {
                            foreach (DataColumn column in dsResults.Tables[0].Columns)
                            {
                                string ColumnData = StripTagsRegex(row[column].ToString());
                                if (ColumnData.Contains("&lt;"))
                                    ColumnData = ColumnData.Replace("&lt;", "<");
                                else if (ColumnData.Contains("&gt;"))
                                    ColumnData = ColumnData.Replace("&gt;", ">");
                                cell = CreateCell("", ColumnData, "");
                                patResults.AddCell(cell);
                            }
                        }
                        doc.Add(patResults);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));

                        ResultMaster objmaster = new ResultMaster();
                        objmaster.Is_Electronic_Mode = "N";
                        objmaster.Created_Date_And_Time = universalTime;
                        objmaster.Created_By = ClientSession.UserName;
                        lstresultmaster.Add(objmaster);


                        ResultOBR objobr = new ResultOBR();
                        objobr.OBR_Observation_Battery_Text = "Blood chemistry";
                        objobr.Created_Date_And_Time = universalTime;
                        objobr.Created_By = ClientSession.UserName;
                        objobr.OBR_Specimen_Collection_Date_And_Time = universalTime.ToString("yyyyMMddhhmmss");
                        lstresultobr.Add(objobr);


                        ResultORC objorc = new ResultORC();
                        objorc.Created_Date_And_Time = universalTime;
                        objorc.Created_By = ClientSession.UserName;
                        lstresultorc.Add(objorc);

                        for (int i = 0; i < dsResults.Tables[0].Rows.Count; i++)
                        {

                            ResultOBX objresult = new ResultOBX();
                            string order = StripTagsRegex(dsResults.Tables[0].Rows[i][0].ToString());
                            if (order != string.Empty)
                            {
                                try
                                {
                                    string[] orders = order.Split(' ');
                                    objresult.OBX_Loinc_Observation_Text = orders[0];
                                    if (order.Contains('(') == true)
                                    {
                                        string strOrder = order.Substring(order.IndexOf('('), order.Length - order.IndexOf('('));

                                        objresult.OBX_Reference_Range = strOrder;

                                    }
                                    //if (orders.Length >= 4)//for bug id 30256
                                    //{
                                    //    objresult.OBX_Reference_Range = orders[1] + " " + orders[2]+" "+orders[3];
                                    //}
                                    //else if (orders.Length >= 3)
                                    //{
                                    //    objresult.OBX_Reference_Range = orders[1] +" "+ orders[2];
                                    //} 
                                    else
                                    {
                                        objresult.OBX_Reference_Range = orders[1];
                                    }
                                    order = StripTagsRegex(dsResults.Tables[0].Rows[i].Field<string>("Blood chemistry"));
                                    orders = order.Split(' ');
                                    objresult.OBX_Observation_Value = orders[0];
                                    objresult.Created_Date_And_Time = universalTime;
                                    objresult.Created_By = ClientSession.UserName;
                                    lstresultobx.Add(objresult);
                                }
                                catch
                                {
                                    //do nothing
                                }
                            }
                        }

                    }
                    else
                    {
                        PdfPTable emptyPDF = new PdfPTable(1);
                        emptyPDF.WidthPercentage = 100;
                        HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                        HeadCell.HorizontalAlignment = 1;
                        HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                        emptyPDF.AddCell(HeadCell);
                        if (sNoInformationInTable == string.Empty)
                        {
                            cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                        }
                        else
                        {
                            cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                        }
                        emptyPDF.AddCell(cell);
                        doc.Add(emptyPDF);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                    }

                    Session["ResultMasterList"] = lstresultmaster;
                    Session["ResultObrList"] = lstresultobr;
                    Session["ResultObxList"] = lstresultobx;
                    Session["ResultOrcList"] = lstresultorc;
                }

                #endregion

                #region Print C32 Results
                else
                {
                    XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                    foreach (XmlElement elemParent in Doc_Results_Node)
                    {
                        bool is_break = false;
                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "TEST RESULTS")
                            {
                                int count = elemParent.ChildNodes[i + 1].ChildNodes.Count;
                                for (int j = 0; j < count; j++)
                                {
                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "paragraph" && elemParent.ChildNodes[i + 1].ChildNodes[j].InnerXml == "Lab Results")
                                    {
                                        IList<string> ColumnHead = new List<string>();
                                        IList<string> RowValue = new List<string>();
                                        int temp_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes.Count;
                                        for (int k = 0; k < temp_count; k++)
                                        {
                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "thead")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "th")
                                                            {
                                                                ColumnHead.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            else if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "tbody")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "td")
                                                            {
                                                                RowValue.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (ColumnHead.Count > 0)
                                        {
                                            PdfPTable patResults = new PdfPTable(ColumnHead.Count);
                                            patResults.WidthPercentage = 100;
                                            HeadCell = new PdfPCell(new Phrase("LAB RESULTS", HeadFont));
                                            HeadCell.Colspan = ColumnHead.Count;
                                            HeadCell.HorizontalAlignment = 1;
                                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                            patResults.AddCell(HeadCell);


                                            for (int x = 0; x < ColumnHead.Count; x++)
                                            {
                                                string ColumnName = StripTagsRegex(ColumnHead[x]);
                                                cell = CreateCell(ColumnName, "", "");
                                                patResults.AddCell(cell);
                                            }

                                            for (int k = 0; k < RowValue.Count; k++)
                                            {
                                                string ColumnName = StripTagsRegex(RowValue[k]);
                                                if (ColumnName.Contains("&lt;"))
                                                    ColumnName = ColumnName.Replace("&lt;", "<");
                                                else if (ColumnName.Contains("&gt;"))
                                                    ColumnName = ColumnName.Replace("&gt;", ">");
                                                cell = CreateCell("", ColumnName, "");
                                                patResults.AddCell(cell);
                                            }

                                            doc.Add(patResults);
                                            doc.Add(new Paragraph("   "));
                                            doc.Add(new Paragraph("   "));

                                        }


                                    }
                                    else if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "paragraph" && elemParent.ChildNodes[i + 1].ChildNodes[j].InnerXml == "Diagnostic Results")
                                    {
                                        IList<string> ColumnHead = new List<string>();
                                        IList<string> RowValue = new List<string>();
                                        int temp_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes.Count;
                                        for (int k = 0; k < temp_count; k++)
                                        {
                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "thead")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "th")
                                                            {
                                                                ColumnHead.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            else if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].Name == "tbody")
                                            {
                                                int head_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes.Count;
                                                for (int l = 0; l < head_count; l++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].Name == "tr")
                                                    {
                                                        int item_count = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                        for (int m = 0; m < item_count; m++)
                                                        {
                                                            if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "td")
                                                            {
                                                                RowValue.Add(elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (ColumnHead.Count > 0)
                                        {
                                            PdfPTable patResults = new PdfPTable(ColumnHead.Count);
                                            patResults.WidthPercentage = 100;
                                            HeadCell = new PdfPCell(new Phrase("DIAGNOSTIC RESULTS", HeadFont));
                                            HeadCell.Colspan = ColumnHead.Count;
                                            HeadCell.HorizontalAlignment = 1;
                                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                            patResults.AddCell(HeadCell);


                                            for (int x = 0; x < ColumnHead.Count; x++)
                                            {
                                                string ColumnName = StripTagsRegex(ColumnHead[x]);
                                                cell = CreateCell(ColumnName, "", "");
                                                patResults.AddCell(cell);
                                            }

                                            for (int k = 0; k < RowValue.Count; k++)
                                            {
                                                string ColumnName = StripTagsRegex(RowValue[k]);
                                                cell = CreateCell("", ColumnName, "");
                                                patResults.AddCell(cell);
                                            }

                                            doc.Add(patResults);
                                            doc.Add(new Paragraph("   "));
                                            doc.Add(new Paragraph("   "));

                                        }
                                    }
                                }
                                is_break = true;
                                break;
                            }
                        }
                        if (is_break == true)
                            break;
                    }
                }

                #endregion

            }
            catch
            {
                DataSet dsResults = null;
                XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                foreach (XmlElement elemParent in Doc_Results_Node)
                {
                    bool is_break = false;
                    for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "RESULTS")
                        {
                            dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            is_break = true;
                            break;
                        }
                    }
                    if (is_break == true)
                        break;
                }

                if (dsResults != null && dsResults.Tables.Count > 0)
                {
                    PdfPTable patResults = new PdfPTable(dsResults.Tables[0].Columns.Count);
                    patResults.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                    HeadCell.Colspan = dsResults.Tables[0].Columns.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patResults.AddCell(HeadCell);


                    for (int j = 0; j < dsResults.Tables[0].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsResults.Tables[0].Columns[j].ColumnName);
                        cell = CreateCell(ColumnName, "", "");
                        patResults.AddCell(cell);
                    }

                    foreach (DataRow row in dsResults.Tables[0].Rows)
                    {
                        foreach (DataColumn column in dsResults.Tables[0].Columns)
                        {
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if (ColumnData.Contains("&lt;"))
                                ColumnData = ColumnData.Replace("&lt;", "<");
                            else if (ColumnData.Contains("&gt;"))
                                ColumnData = ColumnData.Replace("&gt;", ">");
                            cell = CreateCell("", ColumnData, "");
                            patResults.AddCell(cell);
                        }
                    }
                    doc.Add(patResults);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                }
            }

        }

        //Added for Lab Tests
        public void PrintLabTests(Document doc, XmlDocument xmldoc)
        {
            #region Lab Tests
            sNoInformationInTable = string.Empty;
            XmlNodeList Doc_ChiefComplaint_Node = xmldoc.GetElementsByTagName("section");

            DataSet dsOrder = null;
            XmlNodeList Doc_History_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_History_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "LABORATORY TESTS")
                    {
                        // dsHistory = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsOrder = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsOrder = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsOrder != null && dsOrder.Tables.Count > 0)
            {
                PdfPTable patChiefComplaint = new PdfPTable(dsOrder.Tables[0].Columns.Count);
                patChiefComplaint.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("LABORATORY TESTS", HeadFont));
                HeadCell.Colspan = dsOrder.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patChiefComplaint.AddCell(HeadCell);



                for (int j = 0; j < dsOrder.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsOrder.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patChiefComplaint.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsOrder.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsOrder.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patChiefComplaint.AddCell(cell);

                    }
                }

                doc.Add(patChiefComplaint);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("LABORATORY TESTS", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            //foreach (XmlElement elemParent in Doc_ChiefComplaint_Node)
            //{
            //    bool is_break = false;
            //    for (int i = 0; i < elemParent.ChildNodes.Count; i++)
            //    {
            //        if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "LABORATORY TESTS")
            //        {
            //             string TestCode = elemParent.ChildNodes[i + 1].InnerXml;
            //             string CodeSystem = elemParent.ChildNodes[i + 1].InnerXml;
            //             string Name = elemParent.ChildNodes[i + 1].InnerXml;
            //             string Date = elemParent.ChildNodes[i + 1].InnerXml;


            //                cell = CreateCell("Test Code", "", "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("", TestCode, "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("Code System", "", "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("", CodeSystem, "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("Name", "", "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("", Name, "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("Date", "", "");
            //                patChiefComplaint.AddCell(cell);
            //                cell = CreateCell("", Date, "");
            //                patChiefComplaint.AddCell(cell);

            //                doc.Add(patChiefComplaint);
            //                doc.Add(new Paragraph("   "));
            //                doc.Add(new Paragraph("   "));
            //            }
            //            is_break = true;
            //            break;
            //        }
            //    }

            #endregion

        }

        public void PrintLaboratoryInformation(Document doc, XmlDocument xmldoc)
        {
            #region LaboratoryInformation
            sNoInformationInTable = string.Empty;
            DataSet dsLab = null;
            XmlNodeList Doc_History_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_History_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "LABORATORY INFORMATION")
                    {
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsLab = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsLab = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsLab != null && dsLab.Tables.Count > 0)
            {
                PdfPTable patHistory = new PdfPTable(dsLab.Tables[0].Columns.Count);
                patHistory.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("LABORATORY INFORMATION", HeadFont));
                HeadCell.Colspan = dsLab.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patHistory.AddCell(HeadCell);


                for (int j = 0; j < dsLab.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsLab.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patHistory.AddCell(cell);
                }

                foreach (DataRow row in dsLab.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsLab.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        cell = CreateCell("", ColumnData, "");
                        patHistory.AddCell(cell);
                    }
                }
                doc.Add(patHistory);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                //for (int i = 0; i < dsLab.Tables[0].Rows.Count; i++)
                //{
                //    try
                //    {
                //        SocialHistory objhistory = new SocialHistory();
                //        objhistory.Social_Info = StripTagsRegex(dsLab.Tables[0].Rows[i].Field<string>("<tr><th>Social History Element"));
                //        objhistory.Description = StripTagsRegex(dsLab.Tables[0].Rows[i].Field<string>("Description"));
                //        objhistory.Created_Date_And_Time = universalTime;
                //        objhistory.Created_By = ClientSession.UserName;
                //        lstsocilahistory.Add(objhistory);
                //    }
                //    catch
                //    {
                //        //do nothing
                //    }
                //}

            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("LABORATORY INFORMATION", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            #endregion

        }

        public void PrintSocialHistory(Document doc, XmlDocument xmldoc)
        {
            #region Social History
            sNoInformationInTable = string.Empty;
            DataSet dsHistory = null;
            XmlNodeList Doc_History_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_History_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "SOCIAL HISTORY")
                    {
                        // dsHistory = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsHistory = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else
                        {
                            dsHistory = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsHistory != null && dsHistory.Tables.Count > 0)
            {
                PdfPTable patHistory = new PdfPTable(dsHistory.Tables[0].Columns.Count);
                patHistory.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("SOCIAL HISTORY", HeadFont));
                HeadCell.Colspan = dsHistory.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patHistory.AddCell(HeadCell);


                for (int j = 0; j < dsHistory.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsHistory.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patHistory.AddCell(cell);
                }

                foreach (DataRow row in dsHistory.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsHistory.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        cell = CreateCell("", ColumnData, "");
                        patHistory.AddCell(cell);
                    }
                }
                doc.Add(patHistory);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                for (int i = 0; i < dsHistory.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        SocialHistory objhistory = new SocialHistory();
                        objhistory.Social_Info = StripTagsRegex(dsHistory.Tables[0].Rows[i].Field<string>("<tr><th>Social History Element"));
                        objhistory.Description = StripTagsRegex(dsHistory.Tables[0].Rows[i].Field<string>("Description"));
                        objhistory.Created_Date_And_Time = universalTime;
                        objhistory.Created_By = ClientSession.UserName;
                        lstsocilahistory.Add(objhistory);
                    }
                    catch
                    {
                        //do nothing
                    }
                }

            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("SOCIAL HISTORY", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            Session["SocialHistoryList"] = lstsocilahistory;
            #endregion

        }

        public void PrintVitals(Document doc, XmlDocument xmldoc)
        {

            #region Vitals
            sNoInformationInTable = string.Empty;
            DataSet dsVitals = null;
            XmlNodeList Doc_Vitals_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Vitals_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "VITAL SIGNS")
                    {
                        try
                        {
                            // dsVitals = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                                dsVitals = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            else
                            {
                                dsVitals = null;
                                sNoInformationInTable = string.Empty;
                                sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                            }
                        }
                        catch
                        {

                        }

                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsVitals != null)
            {
                IList<string> Column_Header = new List<string>();
                IList<string> Row_Header = new List<string>();
                IList<string> Row_Value = new List<string>();

                foreach (XmlElement elem_Parent in Doc_Vitals_Node)
                {
                    bool is_break = false;
                    for (int i = 0; i < elem_Parent.ChildNodes.Count; i++)
                    {
                        if (elem_Parent.ChildNodes[i].Name == "title" && elem_Parent.ChildNodes[i].InnerText.ToUpper() == "VITAL SIGNS")
                        {
                            PatientResults objvitals = new PatientResults();

                            int count = elem_Parent.ChildNodes[i + 1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                            for (int j = 0; j < count; j++)
                            {

                                Column_Header.Add(elem_Parent.ChildNodes[i + 1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[j].InnerXml);
                            }

                            try
                            {
                                count = elem_Parent.ChildNodes[i + 1].ChildNodes[0].ChildNodes[1].ChildNodes.Count;
                                for (int j = 0; j < count; j++)
                                {
                                    Row_Header.Add(elem_Parent.ChildNodes[i + 1].ChildNodes[0].ChildNodes[1].ChildNodes[j].ChildNodes[0].InnerXml);
                                }

                                if (count > 0)
                                {
                                    int temp_count = elem_Parent.ChildNodes[i + 1].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes.Count;
                                    for (int k = 1; k < temp_count; k++)
                                    {
                                        for (int j = 0; j < count; j++)
                                        {
                                            Row_Value.Add(elem_Parent.ChildNodes[i + 1].ChildNodes[0].ChildNodes[1].ChildNodes[j].ChildNodes[k].InnerXml);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                //do nothing
                            }
                            is_break = true;
                            break;
                        }
                    }
                    if (is_break == true)
                        break;
                }


                //for Saving Summary Of Care

                try
                {
                    int index = 0;
                    if (Row_Value.Count > Row_Header.Count)
                    {
                        index = Row_Value.Count / 2;
                    }
                    else
                    {
                        index = Row_Value.Count;
                    }
                    int c = 0;
                    for (int a = 1; a <= Column_Header.Count - 1; a++)
                    {
                        for (int b = 0; b < index; b++)
                        {
                            try
                            {
                                PatientResults objvitals = new PatientResults();
                                string header = StripTagsRegex(Row_Header[b]);
                                if (header == "Blood Pressure")
                                    objvitals.Loinc_Observation = "BP-Sitting Sys/Dia";
                                else
                                    objvitals.Loinc_Observation = StripTagsRegex(Row_Header[b]);
                                string[] value_units = StripTagsRegex(Row_Value[c]).Split(' ');
                                if (value_units[0].Contains("Ft"))
                                {
                                    string sFeet = ConvertFeetInchToInch(value_units[0].Remove(1), value_units[1].Remove(1));
                                    objvitals.Value = sFeet;
                                    objvitals.Units = "Ft Inch";

                                }
                                else
                                {
                                    objvitals.Value = value_units[0];
                                    objvitals.Units = value_units[1];
                                }
                                objvitals.Captured_date_and_time = Convert.ToDateTime(StripTagsRegex(Column_Header[a].ToString()));
                                objvitals.Created_Date_And_Time = universalTime;
                                objvitals.Created_By = ClientSession.UserName;
                                objvitals.Results_Type = "Vitals";
                                lstvitals.Add(objvitals);
                                c++;
                            }
                            catch
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch
                {
                    //do nothing
                }


                //

                PdfPTable patVitals = new PdfPTable(Column_Header.Count);
                patVitals.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("VITALS", HeadFont));
                HeadCell.Colspan = Column_Header.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patVitals.AddCell(HeadCell);

                for (int i = 0; i < Column_Header.Count; i++)
                {
                    cell = CreateCell(StripTagsRegex(Column_Header[i]), "", "");
                    patVitals.AddCell(cell);
                }

                try
                {
                    int index = Row_Value.Count / Row_Header.Count;
                    int z = 0;
                    int row_index = 0;
                    int x = 1;
                    for (int i = 0; i < Row_Value.Count; i++)
                    {
                        bool header = false;
                        if (i == 0 || i % index == 0)
                        {
                            cell = CreateCell(StripTagsRegex(Row_Header[z]), "", "");
                            patVitals.AddCell(cell);
                            row_index = z;
                            z++;
                            header = true;
                        }
                        if (i == 0 || header == true)
                        {
                            cell = CreateCell("", StripTagsRegex(Row_Value[row_index]), "");
                            patVitals.AddCell(cell);
                        }
                        else
                        {
                            int count = x + index;
                            if (count < Row_Value.Count)
                            {
                                cell = CreateCell("", StripTagsRegex(Row_Value[count]), "");
                                patVitals.AddCell(cell);
                            }
                            else
                            {
                                cell = CreateCell("", StripTagsRegex(Row_Value[i]), "");
                                patVitals.AddCell(cell);
                            }
                            x++;
                        }
                    }
                }
                catch
                {
                    //do nothing
                }

                doc.Add(patVitals);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("VITALS", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            #endregion

            Session["Vitals"] = lstvitals;
        }


        public string ConvertFeetInchToInch(string s1, string s2)
        {
            if (s1 == string.Empty)
            {
                return string.Empty;
            }
            decimal inValue = 0;
            if (s2 != string.Empty)
                inValue = decimal.Round((Convert.ToDecimal(s1) * 12m) + Convert.ToDecimal(s2), 2);
            else
                inValue = decimal.Round((Convert.ToDecimal(s1) * 12m), 2);
            if (inValue != 0m)
                return inValue.ToString();
            else
                return string.Empty;

        }

        public void PrintHospitalInstructions(Document doc, XmlDocument xmldoc)
        {


            #region Hospital Discharge Instruction

            sNoInformationInTable = string.Empty;
            PdfPTable patHospital = new PdfPTable(new float[] { 900 });
            patHospital.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("HOSPITAL DISCHARGE INSTRUCTIONS", HeadFont));
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patHospital.AddCell(HeadCell);

            //Paragraph Hospitalparagraph = new Paragraph();
            //Hospitalparagraph.Add("HOSPITAL DISCHARGE INSTRUCTIONS:");
            //doc.Add(Hospitalparagraph);
            XmlNodeList Doc_Hospital_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Hospital_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "HOSPITAL DISCHARGE INSTRUCTIONS")
                    {
                        try
                        {
                            //iTextSharp.text.List list = new iTextSharp.text.List();
                            //iTextSharp.text.ListItem listItem;
                            //list.SetListSymbol("\u2022");
                            //list.IndentationLeft = 30f;
                            string content = elemParent.ChildNodes[i + 1].ChildNodes[0].InnerXml;
                            cell = CreateCell("", StripTagsRegex(content), "");
                            patHospital.AddCell(cell);
                            int list_count = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes.Count;
                            for (int j = 0; j < list_count; j++)
                            {
                                // listItem = new iTextSharp.text.ListItem(elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[2].ChildNodes[1].ChildNodes[j].InnerXml);
                                //list.Add(listItem);
                                string test = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].InnerXml;
                                cell = CreateCell("", StripTagsRegex(test), "");
                                patHospital.AddCell(cell);
                            }
                            doc.Add(patHospital);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                        }
                        catch
                        {
                            //do nothing
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }


            #endregion

        }

        public void PrintInstructions(Document doc, XmlDocument xmldoc)
        {

            #region Instruction
            sNoInformationInTable = string.Empty;
            PdfPTable patInstruction = new PdfPTable(new float[] { 900 });
            patInstruction.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("INSTRUCTIONS", HeadFont));
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patInstruction.AddCell(HeadCell);

            XmlNodeList Doc_Instruction_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Instruction_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "INSTRUCTIONS")
                    {
                        string instruction = elemParent.ChildNodes[i + 1].InnerXml;
                        if (instruction != string.Empty)
                        {
                            cell = CreateCell("", "       " + instruction, "");
                            patInstruction.AddCell(cell);
                            doc.Add(patInstruction);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }
            #endregion

        }

        public void PrintChiefComplaint(Document doc, XmlDocument xmldoc)
        {
            #region Chief Complaints
            sNoInformationInTable = string.Empty;
            PdfPTable patChiefComplaint = new PdfPTable(new float[] { 900 });
            patChiefComplaint.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("ASSESSMENT", HeadFont));
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patChiefComplaint.AddCell(HeadCell);

            XmlNodeList Doc_ChiefComplaint_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_ChiefComplaint_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "ASSESSMENTS")
                    {
                        string instruction = elemParent.ChildNodes[i + 1].InnerXml;
                        if (instruction != string.Empty)
                        {
                            cell = CreateCell("", "       " + instruction, "");
                            patChiefComplaint.AddCell(cell);
                            doc.Add(patChiefComplaint);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }
            #endregion

        }
        public void PrintProblemsPatientPortal(Document doc, XmlDocument xmldoc)
        {

            #region ProblemList
            sNoInformationInTable = string.Empty;
            XmlNodeList Doc_Problem_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Problem_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "PROBLEMS")
                    {

                        if (elemParent.ChildNodes[i + 1].ChildNodes[1] != null)
                        {
                            PdfPTable patProblemList = new PdfPTable(new float[] { 900, 900 });
                            patProblemList.WidthPercentage = 100;
                            HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                            HeadCell.Colspan = 2;
                            HeadCell.HorizontalAlignment = 1;
                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                            patProblemList.AddCell(HeadCell);
                            cell = CreateCell("Problems", "", "");
                            patProblemList.AddCell(cell);
                            cell = CreateCell("Problem Details", "", "");
                            patProblemList.AddCell(cell);
                            int list_count = elemParent.ChildNodes[i + 1].ChildNodes.Count;
                            int iCount = 0;
                            for (int j = 0; j < list_count; j++)
                            {
                                try
                                {
                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "list")
                                    {
                                        for (int x = 0; x < elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes.Count; x++)
                                        {
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes[0].Value;
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes[1].InnerText;
                                            string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[x].InnerText;

                                            //for bug id: 28517
                                            iCount = iCount + 1;
                                            if (columnvalue != string.Empty)
                                            {
                                                cell = CreateCell("", iCount.ToString(), "");
                                                patProblemList.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = CreateCell("", string.Empty, "");
                                                patProblemList.AddCell(cell);
                                            }
                                            //columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].InnerXml;
                                            cell = CreateCell("", StripTagsRegex(columnvalue), "");
                                            patProblemList.AddCell(cell);

                                            try
                                            {
                                                Assessment objassesment = new Assessment();
                                                string reason = StripTagsRegex(columnvalue);
                                                objassesment.ICD_Description = reason.Replace("\n", " ");
                                                if (objassesment.ICD_Description.Contains("Acute tonsillitis"))
                                                    objassesment.ICD_9 = "463";
                                                else if (objassesment.ICD_Description.Contains("Acute pharyngitis"))
                                                    objassesment.ICD_9 = "462";
                                                else if (objassesment.ICD_Description.Contains("Streptococcal sore throat"))
                                                    objassesment.ICD_9 = "034.0";
                                                objassesment.Created_Date_And_Time = universalTime;
                                                objassesment.Created_By = ClientSession.UserName;
                                                lstassesment.Add(objassesment);
                                            }
                                            catch
                                            {
                                                //do nothing
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].InnerXml;
                                        //for bug id: 28517
                                        if (columnvalue != string.Empty)
                                        {
                                            cell = CreateCell("", (j + 1).ToString(), "");
                                            patProblemList.AddCell(cell);
                                        }
                                        else
                                        {
                                            cell = CreateCell("", string.Empty, "");
                                            patProblemList.AddCell(cell);
                                        }
                                        cell = CreateCell("", StripTagsRegex(columnvalue), "");
                                        patProblemList.AddCell(cell);

                                        try
                                        {
                                            Assessment objassesment = new Assessment();
                                            string reason = StripTagsRegex(columnvalue);
                                            objassesment.ICD_Description = reason.Replace("\n", " ");
                                            if (objassesment.ICD_Description.Contains("Acute tonsillitis"))
                                                objassesment.ICD_9 = "463";
                                            else if (objassesment.ICD_Description.Contains("Acute pharyngitis"))
                                                objassesment.ICD_9 = "462";
                                            else if (objassesment.ICD_Description.Contains("Streptococcal sore throat"))
                                                objassesment.ICD_9 = "034.0";
                                            objassesment.Created_Date_And_Time = universalTime;
                                            objassesment.Created_By = ClientSession.UserName;
                                            lstassesment.Add(objassesment);
                                        }
                                        catch
                                        {
                                            //do nothing
                                        }
                                    }
                                    catch
                                    {
                                        //do nothing
                                    }
                                }
                            }
                            doc.Add(patProblemList);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                            is_break = true;
                            break;
                        }
                        else
                        {
                            DataSet dsProblem = null;
                            dsProblem = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            if (dsProblem != null && dsProblem.Tables.Count > 0)
                            {
                                PdfPTable patProblemList = new PdfPTable(dsProblem.Tables[0].Columns.Count);
                                patProblemList.WidthPercentage = 100;
                                HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                                HeadCell.Colspan = dsProblem.Tables[0].Columns.Count;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                patProblemList.AddCell(HeadCell);

                                for (int j = 0; j < dsProblem.Tables[0].Columns.Count; j++)
                                {
                                    string ColumnName = StripTagsRegex(dsProblem.Tables[0].Columns[j].ColumnName);
                                    cell = CreateCell(ColumnName, "", "");
                                    patProblemList.AddCell(cell);
                                }

                                foreach (DataRow row in dsProblem.Tables[0].Rows)
                                {
                                    foreach (DataColumn column in dsProblem.Tables[0].Columns)
                                    {
                                        string ColumnData = StripTagsRegex(row[column].ToString());
                                        cell = CreateCell("", ColumnData, "");
                                        patProblemList.AddCell(cell);
                                    }
                                }
                                doc.Add(patProblemList);
                                doc.Add(new Paragraph("   "));
                                doc.Add(new Paragraph("   "));
                            }
                            else
                            {
                                sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                                PdfPTable emptyPDF = new PdfPTable(1);
                                emptyPDF.WidthPercentage = 100;
                                HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                emptyPDF.AddCell(HeadCell);
                                if (sNoInformationInTable == string.Empty)
                                {
                                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                                }
                                else
                                {
                                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                                }
                                emptyPDF.AddCell(cell);
                                doc.Add(emptyPDF);
                                doc.Add(new Paragraph("   "));
                                doc.Add(new Paragraph("   "));
                            }

                            is_break = true;
                            break;
                        }
                    }
                }
                if (is_break == true)
                    break;
            }

            Session["AssesmentList"] = lstassesment;
            #endregion
        }

        public void PrintProblems(Document doc, XmlDocument xmldoc,string FileName,string Type)
        {

            #region ProblemList
            sNoInformationInTable = string.Empty;
            XmlNodeList Doc_Problem_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Problem_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "PROBLEMS")
                    {

                        if (elemParent.ChildNodes[i + 1].ChildNodes[1] != null)
                        {
                            PdfPTable patProblemList = new PdfPTable(new float[] { 900, 900 });
                            patProblemList.WidthPercentage = 100;
                            HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                            HeadCell.Colspan = 2;
                            HeadCell.HorizontalAlignment = 1;
                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                            patProblemList.AddCell(HeadCell);
                            cell = CreateCell("Problems", "", "");
                            patProblemList.AddCell(cell);
                            cell = CreateCell("Problem Details", "", "");
                            patProblemList.AddCell(cell);
                            int list_count = elemParent.ChildNodes[i + 1].ChildNodes.Count;
                            int iCount = 0;
                            for (int j = 0; j < list_count; j++)
                            {
                                try
                                {
                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "list")
                                    {
                                        for (int x = 0; x < elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes.Count; x++)
                                        {
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes[0].Value;
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes[1].InnerText;
                                            string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[x].InnerText;

                                            //for bug id: 28517
                                            iCount = iCount + 1;
                                            if (columnvalue != string.Empty)
                                            {
                                                cell = CreateCell("", iCount.ToString(), "");
                                                patProblemList.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = CreateCell("", string.Empty, "");
                                                patProblemList.AddCell(cell);
                                            }
                                            //columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].InnerXml;
                                            cell = CreateCell("", StripTagsRegex(columnvalue), "");
                                            patProblemList.AddCell(cell);

                                            try
                                            {
                                                Assessment objassesment = new Assessment();
                                                string reason = StripTagsRegex(columnvalue);
                                                objassesment.ICD_Description = reason.Replace("\n", " ");
                                                if (objassesment.ICD_Description.Contains("Acute tonsillitis"))
                                                    objassesment.ICD_9 = "463";
                                                else if (objassesment.ICD_Description.Contains("Acute pharyngitis"))
                                                    objassesment.ICD_9 = "462";
                                                else if (objassesment.ICD_Description.Contains("Streptococcal sore throat"))
                                                    objassesment.ICD_9 = "034.0";
                                                objassesment.Created_Date_And_Time = universalTime;
                                                objassesment.Created_By = ClientSession.UserName;
                                                lstassesment.Add(objassesment);
                                            }
                                            catch
                                            {
                                                //do nothing
                                            }
                                        }
                                    }

                                    else if (FileName != null && Type == "C32")
                                    {
                                        if (j < list_count - 1)
                                            j = j + 1;
                                        if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "list")
                                        {
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes[0].Value;
                                            try
                                            {
                                                string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].InnerText;
                                                //for bug id: 28517
                                                iCount = iCount + 1;
                                                if (columnvalue != string.Empty)
                                                {
                                                    cell = CreateCell("", iCount.ToString(), "");
                                                    patProblemList.AddCell(cell);
                                                }
                                                else
                                                {
                                                    cell = CreateCell("", string.Empty, "");
                                                    patProblemList.AddCell(cell);
                                                }
                                                //columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].InnerXml;
                                                string[] problrm = columnvalue.Split(':');

                                                cell = CreateCell("", problrm[0], "");
                                                patProblemList.AddCell(cell);
                                            }
                                            catch
                                            {
                                                cell = CreateCell("", "", "");
                                            }

                                        }
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].InnerXml;
                                        //for bug id: 28517
                                        if (columnvalue != string.Empty)
                                        {
                                            cell = CreateCell("", (j + 1).ToString(), "");
                                            patProblemList.AddCell(cell);
                                        }
                                        else
                                        {
                                            cell = CreateCell("", string.Empty, "");
                                            patProblemList.AddCell(cell);
                                        }
                                        cell = CreateCell("", StripTagsRegex(columnvalue), "");
                                        patProblemList.AddCell(cell);

                                        try
                                        {
                                            Assessment objassesment = new Assessment();
                                            string reason = StripTagsRegex(columnvalue);
                                            objassesment.ICD_Description = reason.Replace("\n", " ");
                                            if (objassesment.ICD_Description.Contains("Acute tonsillitis"))
                                                objassesment.ICD_9 = "463";
                                            else if (objassesment.ICD_Description.Contains("Acute pharyngitis"))
                                                objassesment.ICD_9 = "462";
                                            else if (objassesment.ICD_Description.Contains("Streptococcal sore throat"))
                                                objassesment.ICD_9 = "034.0";
                                            objassesment.Created_Date_And_Time = universalTime;
                                            objassesment.Created_By = ClientSession.UserName;
                                            lstassesment.Add(objassesment);
                                        }
                                        catch
                                        {
                                            //do nothing
                                        }
                                    }
                                    catch
                                    {
                                        //do nothing
                                    }
                                }
                            }
                            doc.Add(patProblemList);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                            is_break = true;
                            break;
                        }

                        else
                        {
                            DataSet dsProblem = null;
                            dsProblem = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            if (dsProblem != null && dsProblem.Tables.Count > 0)
                            {
                                PdfPTable patProblemList = new PdfPTable(dsProblem.Tables[0].Columns.Count);
                                patProblemList.WidthPercentage = 100;
                                HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                                HeadCell.Colspan = dsProblem.Tables[0].Columns.Count;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                patProblemList.AddCell(HeadCell);

                                for (int j = 0; j < dsProblem.Tables[0].Columns.Count; j++)
                                {
                                    string ColumnName = StripTagsRegex(dsProblem.Tables[0].Columns[j].ColumnName);
                                    cell = CreateCell(ColumnName, "", "");
                                    patProblemList.AddCell(cell);
                                }

                                foreach (DataRow row in dsProblem.Tables[0].Rows)
                                {
                                    foreach (DataColumn column in dsProblem.Tables[0].Columns)
                                    {
                                        string ColumnData = StripTagsRegex(row[column].ToString());
                                        cell = CreateCell("", ColumnData, "");
                                        patProblemList.AddCell(cell);
                                    }
                                }
                                doc.Add(patProblemList);
                                doc.Add(new Paragraph("   "));
                                doc.Add(new Paragraph("   "));
                            }

                            is_break = true;
                            break;
                        }
                    }
                }
                if (is_break == true)
                    break;
            }

            Session["AssesmentList"] = lstassesment;
            #endregion
        }

        public void PrintProblems(Document doc, XmlDocument xmldoc)
        {

            #region ProblemList
            sNoInformationInTable = string.Empty;
            XmlNodeList Doc_Problem_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_Problem_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "PROBLEMS")
                    {

                        if (elemParent.ChildNodes[i + 1].ChildNodes[1] != null)
                        {
                            PdfPTable patProblemList = new PdfPTable(new float[] { 900, 900 });
                            patProblemList.WidthPercentage = 100;
                            HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                            HeadCell.Colspan = 2;
                            HeadCell.HorizontalAlignment = 1;
                            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                            patProblemList.AddCell(HeadCell);
                            cell = CreateCell("Problems", "", "");
                            patProblemList.AddCell(cell);
                            cell = CreateCell("Problem Details", "", "");
                            patProblemList.AddCell(cell);
                            int list_count = elemParent.ChildNodes[i + 1].ChildNodes.Count;
                            int iCount = 0;
                            for (int j = 0; j < list_count; j++)
                            {
                                try
                                {
                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "list")
                                    {
                                        for (int x = 0; x < elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes.Count; x++)
                                        {
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes[0].Value;
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes[1].InnerText;
                                            string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[x].InnerText;

                                            //for bug id: 28517
                                            iCount = iCount + 1;
                                            if (columnvalue != string.Empty)
                                            {
                                                cell = CreateCell("", iCount.ToString(), "");
                                                patProblemList.AddCell(cell);
                                            }
                                            else
                                            {
                                                cell = CreateCell("", string.Empty, "");
                                                patProblemList.AddCell(cell);
                                            }
                                            //columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].InnerXml;
                                            cell = CreateCell("", StripTagsRegex(columnvalue), "");
                                            patProblemList.AddCell(cell);

                                            try
                                            {
                                                Assessment objassesment = new Assessment();
                                                string reason = StripTagsRegex(columnvalue);
                                                objassesment.ICD_Description = reason.Replace("\n", " ");
                                                if (objassesment.ICD_Description.Contains("Acute tonsillitis"))
                                                    objassesment.ICD_9 = "463";
                                                else if (objassesment.ICD_Description.Contains("Acute pharyngitis"))
                                                    objassesment.ICD_9 = "462";
                                                else if (objassesment.ICD_Description.Contains("Streptococcal sore throat"))
                                                    objassesment.ICD_9 = "034.0";
                                                objassesment.Created_Date_And_Time = universalTime;
                                                objassesment.Created_By = ClientSession.UserName;
                                                lstassesment.Add(objassesment);
                                            }
                                            catch
                                            {
                                                //do nothing
                                            }
                                        }
                                    }

                                    else if (Request["FileName"] != null && Request["Type"] == "C32")
                                    {
                                        if (j < list_count - 1)
                                            j = j + 1;
                                        if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "list")
                                        {
                                            //string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].Attributes[0].Value;
                                            try
                                            {
                                                string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].InnerText;
                                                //for bug id: 28517
                                                iCount = iCount + 1;
                                                if (columnvalue != string.Empty)
                                                {
                                                    cell = CreateCell("", iCount.ToString(), "");
                                                    patProblemList.AddCell(cell);
                                                }
                                                else
                                                {
                                                    cell = CreateCell("", string.Empty, "");
                                                    patProblemList.AddCell(cell);
                                                }
                                                //columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].InnerXml;
                                                string[] problrm = columnvalue.Split(':');

                                                cell = CreateCell("", problrm[0], "");
                                                patProblemList.AddCell(cell);
                                            }
                                            catch
                                            {
                                                cell = CreateCell("", "", "");
                                            }

                                        }
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].InnerXml;
                                        //for bug id: 28517
                                        if (columnvalue != string.Empty)
                                        {
                                            cell = CreateCell("", (j + 1).ToString(), "");
                                            patProblemList.AddCell(cell);
                                        }
                                        else
                                        {
                                            cell = CreateCell("", string.Empty, "");
                                            patProblemList.AddCell(cell);
                                        }
                                        cell = CreateCell("", StripTagsRegex(columnvalue), "");
                                        patProblemList.AddCell(cell);

                                        try
                                        {
                                            Assessment objassesment = new Assessment();
                                            string reason = StripTagsRegex(columnvalue);
                                            objassesment.ICD_Description = reason.Replace("\n", " ");
                                            if (objassesment.ICD_Description.Contains("Acute tonsillitis"))
                                                objassesment.ICD_9 = "463";
                                            else if (objassesment.ICD_Description.Contains("Acute pharyngitis"))
                                                objassesment.ICD_9 = "462";
                                            else if (objassesment.ICD_Description.Contains("Streptococcal sore throat"))
                                                objassesment.ICD_9 = "034.0";
                                            objassesment.Created_Date_And_Time = universalTime;
                                            objassesment.Created_By = ClientSession.UserName;
                                            lstassesment.Add(objassesment);
                                        }
                                        catch
                                        {
                                            //do nothing
                                        }
                                    }
                                    catch
                                    {
                                        //do nothing
                                    }
                                }
                            }
                            doc.Add(patProblemList);
                            doc.Add(new Paragraph("   "));
                            doc.Add(new Paragraph("   "));
                            is_break = true;
                            break;
                        }

                        else
                        {
                            DataSet dsProblem = null;
                            dsProblem = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            if (dsProblem != null && dsProblem.Tables.Count > 0)
                            {
                                PdfPTable patProblemList = new PdfPTable(dsProblem.Tables[0].Columns.Count);
                                patProblemList.WidthPercentage = 100;
                                HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                                HeadCell.Colspan = dsProblem.Tables[0].Columns.Count;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                                patProblemList.AddCell(HeadCell);

                                for (int j = 0; j < dsProblem.Tables[0].Columns.Count; j++)
                                {
                                    string ColumnName = StripTagsRegex(dsProblem.Tables[0].Columns[j].ColumnName);
                                    cell = CreateCell(ColumnName, "", "");
                                    patProblemList.AddCell(cell);
                                }

                                foreach (DataRow row in dsProblem.Tables[0].Rows)
                                {
                                    foreach (DataColumn column in dsProblem.Tables[0].Columns)
                                    {
                                        string ColumnData = StripTagsRegex(row[column].ToString());
                                        cell = CreateCell("", ColumnData, "");
                                        patProblemList.AddCell(cell);
                                    }
                                }
                                doc.Add(patProblemList);
                                doc.Add(new Paragraph("   "));
                                doc.Add(new Paragraph("   "));
                            }

                            is_break = true;
                            break;
                        }
                    }
                }
                if (is_break == true)
                    break;
            }

            Session["AssesmentList"] = lstassesment;
            #endregion
        }

        public void PrintPatient(Document doc, XmlDocument xmldoc)
        {
            #region Patient Detail
            sNoInformationInTable = string.Empty;
            string pat_Name = string.Empty;
            string middlename = string.Empty;
            string pat_Sex = string.Empty;
            string pat_DOB = string.Empty;
            string pat_Race = string.Empty;
            string pat_Ethnicity = string.Empty;
            string pat_PreferedLanguage = string.Empty;
            string pat_Address = string.Empty;
            string pat_Telephone = string.Empty;
            string pat_Cellphone = string.Empty;
            string pat_Workphone = string.Empty;
            string martialCode = string.Empty;
            string street = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string zip = string.Empty;
            string Granulary_Race = string.Empty;
            string Previous_Name = string.Empty;


            XmlNodeList Doc_parentNode = xmldoc.GetElementsByTagName("patientRole");
            foreach (XmlElement elemParent in Doc_parentNode)
            {
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "addr")
                    {
                        int name_count = elemParent.ChildNodes[i].ChildNodes.Count;


                        for (int k = 0; k < name_count; k++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[k].Name == "streetAddressLine")
                                street = elemParent.ChildNodes[i].ChildNodes[k].InnerXml;
                            else if (elemParent.ChildNodes[i].ChildNodes[k].Name == "city")
                                city = elemParent.ChildNodes[i].ChildNodes[k].InnerXml;
                            else if (elemParent.ChildNodes[i].ChildNodes[k].Name == "state")
                                state = elemParent.ChildNodes[i].ChildNodes[k].InnerXml;
                            else if (elemParent.ChildNodes[i].ChildNodes[k].Name == "postalCode")
                                zip = elemParent.ChildNodes[i].ChildNodes[k].InnerXml;

                        }
                        pat_Address = street + ", " + city + ", " + state + ", " + zip;

                    }

                    else if (elemParent.ChildNodes[i].Name == "telecom")
                    {
                        try
                        {
                            if (elemParent.ChildNodes[i].Attributes[1].Value == "MC")
                                pat_Cellphone = elemParent.ChildNodes[i].Attributes[0].Value;
                            else if (elemParent.ChildNodes[i].Attributes[1].Value == "HP")
                                pat_Telephone = elemParent.ChildNodes[i].Attributes[0].Value;
                            else if (elemParent.ChildNodes[i].Attributes[1].Value == "WP")
                                pat_Workphone = elemParent.ChildNodes[i].Attributes[0].Value;
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                    else if (elemParent.ChildNodes[i].Name == "patient")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "name" )
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Attributes.Count > 0)
                                {
                                    if (pat_Name.Trim() == string.Empty)//BugID:51216
                                    {
                                        int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                        string firstname = string.Empty;
                                        string lastName = string.Empty;
                                        string suffix = string.Empty;

                                        for (int k = 0; k < name_count; k++)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "given")
                                            {
                                                if (firstname == string.Empty)
                                                    firstname = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                                else
                                                {
                                                    if (middlename == string.Empty)
                                                        middlename = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                                }
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "family")
                                            {
                                                if (lastName == string.Empty)
                                                    lastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "suffix")//BugID:51052
                                            {
                                                if (suffix == string.Empty)
                                                    suffix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                            }
                                        }
                                        pat_Name = lastName + ", " + firstname + " " + middlename + " " + suffix;
                                    }
                                    
                                }
                                else
                                {
                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                    for (int k = 0; k < name_count; k++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "given")
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes.Count > 0)
                                                Previous_Name = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                        }
                                    }
                                }

                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "administrativeGenderCode")
                            {
                                //pat_Sex = elemParent.ChildNodes[i].ChildNodes[j].Attributes[2].Value;
                                //bool test = Regex.IsMatch(pat_Sex, @"\d");
                                //if (test == true)
                                //    pat_Sex = elemParent.ChildNodes[i].ChildNodes[j].Attributes[1].Value;
                                pat_Sex = elemParent.ChildNodes[i].ChildNodes[j].Attributes.GetNamedItem("code").Value;

                                if (pat_Sex.ToUpper() == "M")
                                {
                                    pat_Sex = "MALE (M)";

                                }
                                else if (pat_Sex.ToUpper() == "F")
                                {
                                    pat_Sex = "FEMALE (F)";
                                }
                                else
                                {
                                    pat_Sex = "UNKNOWN (U)";
                                }


                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "birthTime")
                            {
                                string birthdate = elemParent.ChildNodes[i].ChildNodes[j].Attributes[0].Value;
                                if (birthdate != string.Empty)
                                {
                                    //string year = birthdate.Substring(0, 4);
                                    //string month = birthdate.Substring(4, 2);
                                    //string date = birthdate.Substring(6, 2);
                                    //pat_DOB = year + "-" + month + "-" + date;
                                    pat_DOB = birthdate;
                                }
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "raceCode")
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Attributes.Count > 1)
                                {
                                    pat_Race = elemParent.ChildNodes[i].ChildNodes[j].Attributes[1].Value + ", " + elemParent.ChildNodes[i].ChildNodes[j].Attributes[0].Value;
                                }
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ethnicGroupCode")
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Attributes.Count > 1)
                                    pat_Ethnicity = elemParent.ChildNodes[i].ChildNodes[j].Attributes[1].Value + ", " + elemParent.ChildNodes[i].ChildNodes[j].Attributes[0].Value;
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "languageCommunication")
                            {
                                int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                for (int k = 0; k < name_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "languageCode")
                                    {
                                        pat_PreferedLanguage = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[0].Value;
                                    }

                                }
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "maritalStatusCode")
                            {
                                try
                                {
                                    martialCode = elemParent.ChildNodes[i].ChildNodes[j].Attributes[1].Value;
                                }
                                catch
                                {
                                    martialCode = "";
                                }
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "sdtc:raceCode")
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Attributes.Count > 1)
                                    Granulary_Race = ((pat_Race.Trim() != string.Empty && pat_Race.Split(',').Length > 0) ? pat_Race.Split(',')[0] + " " : string.Empty) + elemParent.ChildNodes[i].ChildNodes[j].Attributes[1].Value + ", " + elemParent.ChildNodes[i].ChildNodes[j].Attributes[0].Value;//BugID:51023
                            }
                        }
                    }

                }

            }
            XmlNodeList Doc = xmldoc.GetElementsByTagName("languageCode");
            foreach (XmlElement elemParent in Doc)
            {
                if (elemParent.Attributes[0].Value != string.Empty)
                {
                    StaticLookupManager stMngr = new StaticLookupManager();
                    IList<StaticLookup> LookupList = new List<StaticLookup>();
                    LookupList = stMngr.getStaticLookupByFieldNameandDescription("PREFERRED LANGUAGE", elemParent.Attributes[0].Value);
                    if (LookupList.Count > 0)
                        pat_PreferedLanguage = LookupList[0].Value + " (" + elemParent.Attributes[0].Value + ")";
                }
            }
            if (xmldoc.GetElementsByTagName("maritalStatusCode")[0] != null)
            {
                try
                {
                    martialCode = xmldoc.GetElementsByTagName("maritalStatusCode")[0].Attributes.GetNamedItem("displayName").Value;
                }
                catch
                {
                    martialCode = "";
                }
            }
            if (pat_Race == "")
                pat_Race = "UNKNOWN";
            if (pat_Ethnicity == "")
                pat_Ethnicity = "UNKNOWN";
            if (Granulary_Race == "")
                Granulary_Race = "UNKNOWN";
            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("PATIENT DETAIL", HeadFont));
            HeadCell.Colspan = 8;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Patient Name", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", pat_Name, "");
            patPatient.AddCell(cell);
            cell = CreateCell("Sex", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", pat_Sex, "");
            patPatient.AddCell(cell);
            cell = CreateCell("DOB", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", pat_DOB, "");
            patPatient.AddCell(cell);
            cell = CreateCell("Race", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", pat_Race, "");
            patPatient.AddCell(cell);
            cell = CreateCell("Ethnicity", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", pat_Ethnicity, "");
            patPatient.AddCell(cell);
            cell = CreateCell("Preferred Language", "", "");
            patPatient.AddCell(cell);
            /*Commended By Manimaran 
            if (pat_PreferedLanguage == string.Empty)
            {
                pat_PreferedLanguage = "eng";
            }*/
            cell = CreateCell("", pat_PreferedLanguage, "");
            patPatient.AddCell(cell);
            cell = CreateCell("Address", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", pat_Address, "");
            patPatient.AddCell(cell);

            cell = CreateCell("Home Telephone", "", "");
            patPatient.AddCell(cell);
            if (pat_Telephone != "tel:(111) 111-1111")
            {
                cell = CreateCell("", pat_Telephone, "");
                patPatient.AddCell(cell);
            }
            else
            {
                cell = CreateCell("", string.Empty, "");
                patPatient.AddCell(cell);
            }
            cell = CreateCell("Granular Race", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Granulary_Race, "");
            patPatient.AddCell(cell);

            cell = CreateCell("Mobile", "", "");
            patPatient.AddCell(cell);
            //For BugId: 28462
            if (pat_Cellphone != "tel:(111) 111-1111")
            {
                cell = CreateCell("", pat_Cellphone, "");
                patPatient.AddCell(cell);
            }
            else
            {
                cell = CreateCell("", string.Empty, "");
                patPatient.AddCell(cell);
            }


            cell = CreateCell("Previous Name", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Previous_Name, "");
            patPatient.AddCell(cell);

            cell = CreateCell("Work Telephone", "", "");
            patPatient.AddCell(cell);
            //For BugId: 28462
            if (pat_Workphone != "tel:(111) 111-1111")
            {
                cell = CreateCell("", pat_Workphone, "");
                patPatient.AddCell(cell);
            }
            else
            {
                cell = CreateCell("", string.Empty, "");
                patPatient.AddCell(cell);
            }

            patPatient.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));

            try
            {
                string[] patient_name = pat_Name.Split(',');
                string year = pat_DOB.Substring(0, 4);
                string month = pat_DOB.Substring(4, 2);
                string date = pat_DOB.Substring(6, 2);
                pat_DOB = year + "-" + month + "-" + date;
                DateTime date_time = Convert.ToDateTime(pat_DOB);

                HumanManager hnMngr = new HumanManager();
                IList<Human> hnList = hnMngr.GetHumanByName(patient_name[1].Trim().Split(' ')[0], patient_name[0].Replace(" ", ""), date_time, pat_Sex.Split(' ')[0]);
                Human objhuman = new Human();
                if (hnList.Count > 0)
                {
                    Session["HumanList"] = hnList;
                    if (ViewState["PatientPortal"] != null && ViewState["PatientPortal"] == "PatientPortal")
                        return;
                    else
                        ClientSession.HumanId = hnList[0].Id;
                    hdnHumanID.Value = hnList[0].Id.ToString();
                    return;
                }
                objhuman.Last_Name = patient_name[0].Replace(" ", "");
                objhuman.First_Name = patient_name[1].Trim().Split(' ')[0];// patient_name[1].Replace(" ", "");
                objhuman.MI = middlename;
                objhuman.Birth_Date = date_time;
                objhuman.Sex = pat_Sex.Split(' ')[0];
                objhuman.Race = pat_Race;
                objhuman.Ethnicity = pat_Ethnicity.Split(',')[0].ToString(); ;
                objhuman.Street_Address1 = street;
                objhuman.Marital_Status = martialCode;
                objhuman.City = city;
                objhuman.State = state;
                objhuman.Is_Sent_To_Rcopia = "N";
                objhuman.Created_Date_And_Time = universalTime;
                objhuman.Created_By = ClientSession.UserName;
                lsthuman.Add(objhuman);
            }
            catch
            {
                //do nothing
            }
            Session["HumanList"] = lsthuman;

            #endregion
        }

        public void PrintAuthor(Document doc, XmlDocument xmldoc)
        {
            #region Author
            sNoInformationInTable = string.Empty;
            string pat_AuthorName = string.Empty;
            string pat_AuthorAddres = string.Empty;
            string pat_AuthorTel = string.Empty;

            string street = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string Zip = string.Empty;

            PdfPTable patAuthor = new PdfPTable(new float[] { 900, 900 });
            patAuthor.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("AUTHOR", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patAuthor.AddCell(HeadCell);

            XmlNodeList Doc_authorNode = xmldoc.GetElementsByTagName("author");
            foreach (XmlElement elemParent in Doc_authorNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "assignedAuthor")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "assignedPerson")
                            {
                                int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                string firstname = string.Empty;
                                string lastName = string.Empty;
                                string Prefix = string.Empty;

                                for (int k = 0; k < name_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "name")
                                    {
                                        int child_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                        for (int x = 0; x < child_count; x++)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "given")
                                                firstname = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "family")
                                                lastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "prefix")
                                                Prefix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                        }
                                        break;
                                    }
                                }
                                pat_AuthorName = Prefix + " " + firstname + " " + lastName;
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "addr")
                            {
                                int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;

                                for (int k = 0; k < name_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "streetAddressLine")
                                        street = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "city")
                                        city = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "state")
                                        state = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "postalCode")
                                        Zip = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                }
                                pat_AuthorAddres = street + ", " + city + ", " + state + ", " + Zip;
                            }
                            else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "telecom")
                            {
                                try
                                {
                                    pat_AuthorTel = elemParent.ChildNodes[i].ChildNodes[j].Attributes[1].Value;
                                }
                                catch
                                {
                                    //do nothing
                                }
                            }

                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }


            //Start
            XmlNodeList Doc_ProviderNode = xmldoc.GetElementsByTagName("documentationOf");
            foreach (XmlElement elemParent in Doc_ProviderNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "serviceEvent")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "performer")
                            {
                                int per_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;

                                for (int k = 0; k < per_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "assignedEntity")
                                    {
                                        int ent_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                        for (int l = 0; l < ent_count; l++)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "assignedPerson")
                                            {
                                                int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                string LastName = string.Empty;
                                                string FirstName = string.Empty;
                                                string prefix = string.Empty;
                                                string suffix = string.Empty;

                                                for (int y = 0; y < name_count; y++)
                                                {
                                                    int name_count1 = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes.Count;
                                                    for (int x = 0; x < name_count1; x++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "given" && FirstName=="")
                                                            FirstName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                        else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "family")
                                                            LastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                        else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "prefix")
                                                            prefix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                        else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "suffix")
                                                            suffix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                    }

                                                }
                                                pat_AuthorName = prefix + " " + FirstName + " " + LastName + " " + suffix;
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }
            //End


            if (pat_AuthorName != string.Empty || pat_AuthorAddres != string.Empty || pat_AuthorTel != string.Empty)
            {
                cell = CreateCell("Author Name", "", "");
                patAuthor.AddCell(cell);
                cell = CreateCell("", pat_AuthorName, "");
                patAuthor.AddCell(cell);
                cell = CreateCell("Author Address", "", "");
                patAuthor.AddCell(cell);
                cell = CreateCell("", pat_AuthorAddres, "");
                patAuthor.AddCell(cell);
                cell = CreateCell("Author Telephone", "", "");
                patAuthor.AddCell(cell);
                cell = CreateCell("", pat_AuthorTel, "");
                patAuthor.AddCell(cell);

                doc.Add(patAuthor);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    string[] name = pat_AuthorName.Split(' ');
                    PhysicianLibrary objphylib = new PhysicianLibrary();
                    objphylib.PhyLastName = name[2].Replace("Dr ", "").Replace(" ", "");
                    objphylib.PhyFirstName = name[1].Replace("Dr ", "").Replace(" ", "");
                    objphylib.PhyAddress1 = street;
                    objphylib.PhyCity = city;
                    objphylib.PhyState = state;
                    objphylib.PhyTelephone = pat_AuthorTel.Replace("tel:", "").Trim();
                    objphylib.CreatedDateAndTime = universalTime;
                    lstphysician.Add(objphylib);
                }
                catch
                {
                    //do nothing
                }
            }
            Session["PhysicianLibrary"] = lstphysician;
            #endregion
        }

        public void PrintCustodian(Document doc, XmlDocument xmldoc)
        {
            #region Custodian
            sNoInformationInTable = string.Empty;
            string pat_CustodianName = string.Empty;
            string pat_CustodianAddress = string.Empty;
            string pat_CustodianTelephone = string.Empty;

            string street = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string Zip = string.Empty;

            PdfPTable patCustodian = new PdfPTable(new float[] { 900, 900 });
            patCustodian.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("CUSTODIAN", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patCustodian.AddCell(HeadCell);

            XmlNodeList Doc_CustodianNode = xmldoc.GetElementsByTagName("custodian");
            foreach (XmlElement elemParent in Doc_CustodianNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "assignedCustodian")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "representedCustodianOrganization")
                            {
                                int cust_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                for (int k = 0; k < cust_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "name")
                                    {
                                        pat_CustodianName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                    }
                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "addr")
                                    {
                                        int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;


                                        for (int x = 0; x < name_count; x++)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "streetAddressLine")
                                                street = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "city")
                                                city = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "state")
                                                state = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "postalCode")
                                                Zip = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                        }
                                        pat_CustodianAddress = street + ", " + city + ", " + state + ", " + Zip;
                                    }
                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "telecom")
                                    {
                                        try
                                        {
                                            if(elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[0].Name.ToUpper()=="VALUE")
                                            pat_CustodianTelephone = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[0].Value;
                                            else if(elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[1].Name.ToUpper()=="VALUE")
                                                    pat_CustodianTelephone = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[1].Value;

                                        }
                                        catch
                                        {
                                            //do nothing
                                        }
                                    }

                                }
                                break;
                            }
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (pat_CustodianName != string.Empty || pat_CustodianAddress != string.Empty || pat_CustodianTelephone != string.Empty)
            {
                cell = CreateCell("Custodian Name", "", "");
                patCustodian.AddCell(cell);
                cell = CreateCell("", pat_CustodianName, "");
                patCustodian.AddCell(cell);
                cell = CreateCell("Custodian Address", "", "");
                patCustodian.AddCell(cell);
                cell = CreateCell("", pat_CustodianAddress, "");
                patCustodian.AddCell(cell);
                cell = CreateCell("Custodian Telephone", "", "");
                patCustodian.AddCell(cell);
                cell = CreateCell("", pat_CustodianTelephone, "");
                patCustodian.AddCell(cell);

                doc.Add(patCustodian);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    FacilityLibrary objfacility = new FacilityLibrary();
                    objfacility.Fac_Name = pat_CustodianName;
                    objfacility.Fac_Address1 = street;
                    objfacility.Fac_City = city;
                    objfacility.Fac_State = state;
                    objfacility.Fac_Telephone = pat_CustodianTelephone.Replace("tel:", "").Trim();
                    objfacility.Created_Date_And_Time = System.DateTime.Now;//universalTime;
                    objfacility.Created_By = ClientSession.UserName;
                    lstfacility.Add(objfacility);
                }
                catch
                {
                    //do nothing
                }


            }
            Session["FacilityLibrary"] = lstfacility;
            #endregion
        }

        public void PrintProvider(Document doc, XmlDocument xmldoc)
        {
            #region Provider
            sNoInformationInTable = string.Empty;
            string pat_ProviderName = string.Empty;

            PdfPTable patProvider = new PdfPTable(new float[] { 900, 900 });
            patProvider.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("PROVIDER", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patProvider.AddCell(HeadCell);
            string suffix = string.Empty;
            XmlNodeList Doc_ProviderNode = xmldoc.GetElementsByTagName("documentationOf");
            foreach (XmlElement elemParent in Doc_ProviderNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "serviceEvent")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "performer")
                            {
                                int per_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;

                                for (int k = 0; k < per_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "assignedEntity")
                                    {
                                        int ent_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                        for (int l = 0; l < ent_count; l++)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "assignedPerson")
                                            {

                                                int pers_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count;

                                                for (int m = 0; m < pers_count; m++)
                                                {
                                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "name")
                                                    {
                                                        int child_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                        string firstname = string.Empty;
                                                        string lastName = string.Empty;
                                                        string Prefix = string.Empty;
                                                        for (int n = 0; n < child_count; n++)
                                                        {
                                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "given" && firstname=="")
                                                                firstname = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "family")
                                                                lastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "prefix")
                                                                Prefix = lastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            else if ((elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "suffix"))
                                                                suffix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                        }
                                                        pat_ProviderName = Prefix + " " + firstname + " " + lastName + " " + suffix;//BugID:51050
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (pat_ProviderName != string.Empty)
            {
                cell = CreateCell("Provider Name", "", "");
                patProvider.AddCell(cell);
                cell = CreateCell("", pat_ProviderName, "");
                patProvider.AddCell(cell);

                doc.Add(patProvider);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            #endregion

        }

        //Added for measures vdt
        public void PrintReferringProvider(Document doc, XmlDocument xmldoc)
        {
            #region DocumentDetails
            sNoInformationInTable = string.Empty;
            string pat_DocumentAddress = string.Empty;
            string pat_DocumentTel = string.Empty;
            string ProviderName = string.Empty;
            string CareTeamMembers = string.Empty;

            PdfPTable patDocument = new PdfPTable(new float[] { 900, 900 });
            patDocument.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("REFERRING PROVIDER", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patDocument.AddCell(HeadCell);



            XmlNodeList Doc_ProviderNode = xmldoc.GetElementsByTagName("documentationOf");
            foreach (XmlElement elemParent in Doc_ProviderNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "serviceEvent")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "performer")
                            {
                                int per_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                bool bCheckProvider = false;
                                for (int k = 0; k < per_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].Name == "performer" && elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "functionCode")
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes.Count > 0)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[3].Value == "Primary Care Provider")
                                            {
                                                bCheckProvider = true;
                                            }
                                        }
                                    }
                                    if (bCheckProvider)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "assignedEntity")
                                        {
                                            int ent_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                            for (int l = 0; l < ent_count; l++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "addr")
                                                {
                                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                    string street = string.Empty;
                                                    string city = string.Empty;
                                                    string state = string.Empty;
                                                    string zip = string.Empty;

                                                    for (int x = 0; x < name_count; x++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "streetAddressLine")
                                                            street = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                        else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "city")
                                                            city = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                        else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "state")
                                                            state = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                        else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "postalCode")
                                                            zip = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                    }
                                                    pat_DocumentAddress = street + ", " + city + ", " + state + ", " + zip;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "telecom")
                                                {
                                                    try
                                                    {
                                                        pat_DocumentTel = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[1].Value;
                                                    }
                                                    catch
                                                    {
                                                        //do nothing
                                                    }
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "assignedPerson")
                                                {
                                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                    string LastName = string.Empty;
                                                    string FirstName = string.Empty;
                                                    string Prefix = string.Empty;

                                                    for (int y = 0; y < name_count; y++)
                                                    {
                                                        int name_count1 = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes.Count;
                                                        for (int x = 0; x < name_count1; x++)
                                                        {
                                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "given")
                                                                FirstName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "family")
                                                                LastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "prefix")
                                                                Prefix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                        }
                                                    }
                                                    ProviderName = Prefix + " " + FirstName + " " + LastName;//BugID:51050
                                                }
                                            }
                                            if (!bCheckProvider)
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "assignedEntity")
                                        {
                                            int ent_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                            for (int l = 0; l < ent_count; l++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "assignedPerson")
                                                {
                                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                    string LastName = string.Empty;
                                                    string FirstName = string.Empty;

                                                    for (int y = 0; y < name_count; y++)
                                                    {
                                                        int name_count1 = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes.Count;
                                                        for (int x = 0; x < name_count1; x++)
                                                        {
                                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "given")
                                                                FirstName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "family")
                                                                LastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].InnerXml;
                                                        }
                                                    }
                                                    if (LastName != string.Empty && FirstName != string.Empty)//BugID:51050
                                                        CareTeamMembers = ProviderName + "\n" + FirstName + " " + LastName;
                                                    else if (FirstName!=string.Empty)
                                                       CareTeamMembers = ProviderName + "\n" + FirstName;
                                                    else if(LastName!=string.Empty)
                                                        CareTeamMembers = ProviderName + "\n" + LastName;
                                                    bCheckProvider = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!bCheckProvider)
                                    break;
                            }
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (pat_DocumentAddress != string.Empty || pat_DocumentTel != string.Empty)
            {
                cell = CreateCell("Provider Name", "", "");
                patDocument.AddCell(cell);
                cell = CreateCell("", ProviderName, "");
                patDocument.AddCell(cell);
                cell = CreateCell("Care Team Members", "", "");
                patDocument.AddCell(cell);
                cell = CreateCell("", CareTeamMembers, "");
                patDocument.AddCell(cell);
                cell = CreateCell("Office Contact Information", "", "");
                patDocument.AddCell(cell);
                cell = CreateCell("", pat_DocumentAddress, "");
                patDocument.AddCell(cell);
                cell = CreateCell("Author Telephone", "", "");
                patDocument.AddCell(cell);
                cell = CreateCell("", pat_DocumentTel, "");
                patDocument.AddCell(cell);

                doc.Add(patDocument);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            #endregion
        }

        public void PrintDocumentDetail(Document doc, XmlDocument xmldoc)
        {
            #region DocumentDetails
            sNoInformationInTable = string.Empty;
            string pat_DocumentAddress = string.Empty;
            string pat_DocumentTel = string.Empty;

            PdfPTable patDocument = new PdfPTable(new float[] { 900, 900 });
            patDocument.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("DOCUMENTATION DETAIL", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patDocument.AddCell(HeadCell);

            XmlNodeList Doc_ProviderNode = xmldoc.GetElementsByTagName("documentationOf");
            foreach (XmlElement elemParent in Doc_ProviderNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "serviceEvent")
                    {
                        int count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "performer")
                            {
                                int per_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;

                                for (int k = 0; k < per_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "assignedEntity")
                                    {
                                        int ent_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;
                                        for (int l = 0; l < ent_count; l++)
                                        {
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "addr")
                                            {
                                                int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                                string street = string.Empty;
                                                string city = string.Empty;
                                                string state = string.Empty;
                                                string zip = string.Empty;

                                                for (int x = 0; x < name_count; x++)
                                                {
                                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "streetAddressLine")
                                                        street = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "city")
                                                        city = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "state")
                                                        state = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].Name == "postalCode")
                                                        zip = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[x].InnerXml;
                                                }
                                                pat_DocumentAddress = street + ", " + city + ", " + state + ", " + zip;
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "telecom")
                                            {
                                                try
                                                {
                                                     if( elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[0].Name.ToUpper()=="VALUE")
                                                         pat_DocumentTel = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[0].Value;
                                            else if ( elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[1].Name.ToUpper()=="VALUE")
                                                         pat_DocumentTel = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[1].Value;

                                                 
                                                }
                                                catch
                                                {
                                                    //do nothing
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (pat_DocumentAddress != string.Empty || pat_DocumentTel != string.Empty)
            {
                cell = CreateCell("Documentation of Address", "", "");
                patDocument.AddCell(cell);
                cell = CreateCell("", pat_DocumentAddress, "");
                patDocument.AddCell(cell);
                cell = CreateCell("Documentation of Telephone", "", "");
                patDocument.AddCell(cell);
                cell = CreateCell("", pat_DocumentTel, "");
                patDocument.AddCell(cell);

                doc.Add(patDocument);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }

            #endregion
        }

        public void PrintVisitDetail(Document doc, XmlDocument xmldoc)
        {
            #region VisitDetails
            sNoInformationInTable = string.Empty;
            string pat_DOV = string.Empty;
            string pat_LOV = string.Empty;
            string pat_ROV = string.Empty;

            PdfPTable patVisit = new PdfPTable(new float[] { 900, 900 });
            patVisit.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("ENCOUNTER DETAIL", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patVisit.AddCell(HeadCell);


            DataSet dsEncounter = null;
            XmlNodeList Doc_EncounterNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_EncounterNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS DIAGNOSIS"))
                    {
                        dsEncounter = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        break;
                    }
                }

                if (dsEncounter != null)
                {
                    for (int i = 0; i < dsEncounter.Tables[0].Rows.Count; i++)
                    {
                        string Visit_Date = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("Date"));
                        if (Visit_Date != string.Empty)
                        {
                            //string year = Visit_Date.Substring(0, 4);
                            //string month = Visit_Date.Substring(4, 2);
                            //string date = Visit_Date.Substring(6, 2);
                            //pat_DOV = year + "-" + month + "-" + date;
                            pat_DOV = Visit_Date;
                        }
                        try
                        {
                            pat_LOV = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("Location"));
                        }
                        catch
                        {
                            pat_LOV = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("Location"));
                        }
                        try
                        {
                            pat_ROV = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("<tr><th>Encounter"));
                        }
                        catch
                        {
                            pat_ROV = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("<tr><th>Encounter Diagnosis"));
                        }

                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (pat_DOV != string.Empty || pat_LOV != string.Empty || pat_ROV != string.Empty)
            {
                cell = CreateCell("Date of Visit", "", "");
                patVisit.AddCell(cell);
                cell = CreateCell("", pat_DOV, "");
                patVisit.AddCell(cell);
                cell = CreateCell("Location of Visit", "", "");
                patVisit.AddCell(cell);
                cell = CreateCell("", pat_LOV, "");
                patVisit.AddCell(cell);
                cell = CreateCell("Encounter Diagnosis", "", "");//BugId:51017
                patVisit.AddCell(cell);
                cell = CreateCell("", pat_ROV, "");
                patVisit.AddCell(cell);

                doc.Add(patVisit);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));

                try
                {
                    Encounter objencounter = new Encounter();
                    objencounter.Facility_Name = pat_LOV;
                    string year = pat_DOV.Substring(0, 4);
                    string month = pat_DOV.Substring(4, 2);
                    string date = pat_DOV.Substring(6, 2);
                    try
                    {
                        if (pat_DOV.Length == 8)
                        {
                            hdnDateOfService.Value = year + "-" + month + "-" + date;
                            objencounter.Date_of_Service = UtilityManager.ConvertToUniversal(Convert.ToDateTime(year + "-" + month + "-" + date));

                        }

                        else
                        {
                            string datestring = pat_DOV.Split(',')[0].Split(' ')[1] + "-" + pat_DOV.Split(',')[0].Split(' ')[0] + pat_DOV.Split(',')[1].Split(' ')[1];
                            objencounter.Date_of_Service = UtilityManager.ConvertToUniversal(Convert.ToDateTime(datestring));
                            hdnDateOfService.Value = Convert.ToDateTime(datestring).ToString("yyyy-MM-dd");
                        }
                    }
                    catch
                    {



                    }

                    // hdnDateOfService.Value = year + "-" + month + "-" + date;
                    objencounter.Local_Time = UtilityManager.ConvertToLocal(objencounter.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt"); ;
                    objencounter.Purpose_of_Visit = pat_ROV;
                    objencounter.Created_Date_and_Time = universalTime;
                    objencounter.Created_By = ClientSession.UserName;
                    lstencounter.Add(objencounter);
                }
                catch
                {
                    //do nothing
                }
            }

            Session["Encounter"] = lstencounter;

            #endregion
        }

        public void PrintTreatmentPlan(Document doc, XmlDocument xmldoc)
        {
            #region Lab Tests
            sNoInformationInTable = string.Empty;
            XmlNodeList Doc_ChiefComplaint_Node = xmldoc.GetElementsByTagName("section");

            DataSet dsTreatementPlan = null;
            XmlNodeList Doc_History_Node = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_History_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "TREATMENT PLAN")
                    {
                        // dsHistory = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<table") == true)
                            dsTreatementPlan = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                        else if (elemParent.ChildNodes[i + 1].InnerXml.Trim(' ').StartsWith("<content") == true)
                        {
                            foreach (XmlNode xnode in elemParent.ChildNodes[i + 1].ChildNodes)
                            {
                                if (xnode.OuterXml.StartsWith("<table"))
                                {
                                    dsTreatementPlan = ConvertHTMLTablesToDataSet(xnode.OuterXml);
                                    break;
                                }

                            }
                        }
                        else
                        {
                            dsTreatementPlan = null;
                            sNoInformationInTable = string.Empty;
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                        }
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsTreatementPlan != null && dsTreatementPlan.Tables.Count > 0)
            {
                PdfPTable patChiefComplaint = new PdfPTable(dsTreatementPlan.Tables[0].Columns.Count);
                patChiefComplaint.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("TREATMENT PLAN", HeadFont));
                HeadCell.Colspan = dsTreatementPlan.Tables[0].Columns.Count;
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                patChiefComplaint.AddCell(HeadCell);



                for (int j = 0; j < dsTreatementPlan.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsTreatementPlan.Tables[0].Columns[j].ColumnName);
                    //if (ColumnName.ToUpper() != "NDCID")
                    //{
                    cell = CreateCell(ColumnName, "", "");
                    patChiefComplaint.AddCell(cell);
                    // }
                }

                foreach (DataRow row in dsTreatementPlan.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsTreatementPlan.Tables[0].Columns)
                    {
                        //if (!row[column].ToString().Contains("NDCID"))
                        //{
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patChiefComplaint.AddCell(cell);

                    }
                }

                doc.Add(patChiefComplaint);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }
            else
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase("TREATMENT PLAN", HeadFont));
                HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sNoInformationInTable == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                doc.Add(new Paragraph("   "));
            }


            #endregion

        }

        public void PrintMedicationAdministered(Document doc, XmlDocument xmldoc)
        {
            sNoInformationInTable = string.Empty;
            PdfPTable patVisit = new PdfPTable(new float[] { 900 });
            patVisit.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("MEDICATION ADMINISTERED", HeadFont));
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patVisit.AddCell(HeadCell);

            DataSet dsEncounter = null;
            XmlNodeList Doc_EncounterNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_EncounterNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "MEDICATIONS ADMINISTERED")
                    {
                        string medication = elemParent.ChildNodes[i + 1].InnerXml;
                        cell = CreateCell("", medication, "");
                        patVisit.AddCell(cell);
                        doc.Add(patVisit);
                        doc.Add(new Paragraph("   "));
                        doc.Add(new Paragraph("   "));
                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }
        }

        public void Print_CCR_ContinuityOfCare(Document doc, XmlDocument xmldoc)
        {

            string Date = string.Empty;
            string From = string.Empty;
            string To = string.Empty;
            string Purpose = string.Empty;


            XmlNodeList Doc_Date = xmldoc.GetElementsByTagName("ccr:DateTime");
            foreach (XmlElement elemParent in Doc_Date)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:ExactDateTime")
                        {
                            Date = elemParent.ChildNodes[i].InnerXml;
                            string[] split_date = Date.Split('T');
                            Date = split_date[0];
                            is_break = true;
                            break;
                        }
                    }
                }
                if (is_break == true)
                    break;
            }

            XmlNodeList Doc_From = xmldoc.GetElementsByTagName("ccr:Actor");
            foreach (XmlElement elemParent in Doc_From)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                {
                    bool is_author = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:ActorObjectID" && elemParent.ChildNodes[i].InnerXml == "AuthorID_01")
                            is_author = true;
                        if (elemParent.ChildNodes[i].Name == "ccr:Person" && is_author == true)
                        {
                            int person_count = elemParent.ChildNodes[i].ChildNodes.Count;

                            for (int j = 0; j < person_count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Name")
                                {
                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                    for (int k = 0; k < name_count; k++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:BirthName")
                                        {
                                            int birth_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;

                                            for (int l = 0; l < birth_count; l++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Given")
                                                {
                                                    From = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Family")
                                                {
                                                    From = From + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Suffix")
                                                {
                                                    From = From + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                            }
                                            is_break = true;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (is_break == true)
                {
                    Session["ProviderName"] = From;
                    break;
                }
            }


            XmlNodeList Doc_Type = xmldoc.GetElementsByTagName("ccr:From");
            foreach (XmlElement elemParent in Doc_Type)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "ccr:ActorLink")
                    {
                        int actor_count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < actor_count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:ActorRole")
                            {
                                int role_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                for (int k = 0; k < role_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:Text")
                                        From = From + " (" + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml + ")";
                                }
                                break;
                            }

                        }
                        break;
                    }
                }
            }


            XmlNodeList Doc_To = xmldoc.GetElementsByTagName("ccr:Actor");
            foreach (XmlElement elemParent in Doc_To)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                {
                    bool is_author = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:ActorObjectID" && elemParent.ChildNodes[i].InnerXml == "RecipientID_01")
                            is_author = true;
                        if (elemParent.ChildNodes[i].Name == "ccr:Person" && is_author == true)
                        {
                            int person_count = elemParent.ChildNodes[i].ChildNodes.Count;

                            for (int j = 0; j < person_count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Name")
                                {
                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                    for (int k = 0; k < name_count; k++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:BirthName")
                                        {
                                            int birth_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;

                                            for (int l = 0; l < birth_count; l++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Given")
                                                {
                                                    To = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Family")
                                                {
                                                    To = To + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Suffix")
                                                {
                                                    To = To + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                            }
                                            is_break = true;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                if (is_break == true)
                    break;
            }

            XmlNodeList Doc_To_Type = xmldoc.GetElementsByTagName("ccr:To");
            foreach (XmlElement elemParent in Doc_To_Type)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "ccr:ActorLink")
                    {
                        int actor_count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < actor_count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:ActorRole")
                            {
                                int role_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                for (int k = 0; k < role_count; k++)
                                {
                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:Text")
                                        To = To + " (" + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml + ")";
                                }
                                break;
                            }

                        }
                        break;
                    }
                }
            }

            XmlNodeList Doc_Purpose = xmldoc.GetElementsByTagName("ccr:Purpose");
            foreach (XmlElement elemParent in Doc_Purpose)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "ccr:Description")
                    {
                        int actor_count = elemParent.ChildNodes[i].ChildNodes.Count;
                        for (int j = 0; j < actor_count; j++)
                        {
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Text")
                            {
                                Purpose = elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                break;
                            }
                        }
                        break;
                    }
                }
            }


            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("CONTINUITY OF CARE RECORD", HeadFont));
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Date Created", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Date, "");
            patPatient.AddCell(cell);
            cell = CreateCell("From", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", From, "");
            patPatient.AddCell(cell);
            cell = CreateCell("To", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", To, "");
            patPatient.AddCell(cell);
            cell = CreateCell("Purpose", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Purpose, "");
            patPatient.AddCell(cell);

            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));

        }

        public void Print_CCR_PatientDemographics(Document doc, XmlDocument xmldoc)
        {

            string Name = string.Empty;
            string DOB = string.Empty;
            string Gender = string.Empty;
            string IdentifivationNumber = string.Empty;
            string Address = string.Empty;

            XmlNodeList Doc_Name = xmldoc.GetElementsByTagName("ccr:Actor");
            foreach (XmlElement elemParent in Doc_Name)
            {
                bool is_break = false;
                int count = elemParent.ChildNodes.Count;
                {
                    bool is_author = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:ActorObjectID" && elemParent.ChildNodes[i].InnerXml == "PatientID_1")
                            is_author = true;
                        if (elemParent.ChildNodes[i].Name == "ccr:Person" && is_author == true)
                        {
                            int person_count = elemParent.ChildNodes[i].ChildNodes.Count;

                            for (int j = 0; j < person_count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Name")
                                {
                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                    for (int k = 0; k < name_count; k++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:BirthName")
                                        {
                                            int birth_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes.Count;

                                            for (int l = 0; l < birth_count; l++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Given")
                                                {
                                                    if (Name == string.Empty)
                                                        Name = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Family")
                                                {
                                                    Name = Name + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "ccr:Suffix")
                                                {
                                                    Name = Name + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].InnerXml;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:DateOfBirth")
                                {
                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                    for (int k = 0; k < name_count; k++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:ExactDateTime")
                                        {
                                            DOB = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                            string[] split_date = DOB.Split('T');
                                            DOB = split_date[0];
                                            break;
                                        }
                                    }
                                }
                                else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Gender")
                                {
                                    int name_count = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes.Count;
                                    for (int k = 0; k < name_count; k++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Name == "ccr:Text")
                                        {
                                            Gender = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].InnerXml;
                                            break;
                                        }
                                    }
                                }

                            }

                        }
                        else if (elemParent.ChildNodes[i].Name == "ccr:IDs" && is_author == true)
                        {
                            int name_count = elemParent.ChildNodes[i].ChildNodes.Count;
                            for (int j = 0; j < name_count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:ID")
                                {
                                    IdentifivationNumber = elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                    break;
                                }
                            }
                        }
                        else if (elemParent.ChildNodes[i].Name == "ccr:Address" && is_author == true)
                        {
                            int name_count = elemParent.ChildNodes[i].ChildNodes.Count;
                            for (int j = 0; j < name_count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Line1")
                                {
                                    Address = elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                }
                                else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:City")
                                {
                                    Address = Address + " " + elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                }
                                else if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:State")
                                {
                                    Address = Address + " " + elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                }
                            }
                        }
                        else if (elemParent.ChildNodes[i].Name == "ccr:Telephone" && is_author == true)
                        {
                            int name_count = elemParent.ChildNodes[i].ChildNodes.Count;
                            for (int j = 0; j < name_count; j++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[j].Name == "ccr:Value")
                                {
                                    Address = Address + " " + elemParent.ChildNodes[i].ChildNodes[j].InnerXml;
                                    break;
                                }

                            }
                        }
                    }
                }

            }

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("PATIENT DEMOGRAPHICS", HeadFont));
            HeadCell.Colspan = 5;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Name", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Date Of Birth", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Gender", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Identification Numbers", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Address / Phone", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Name, "");
            patPatient.AddCell(cell);
            cell = CreateCell("", DOB, "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Gender, "");
            patPatient.AddCell(cell);
            cell = CreateCell("", IdentifivationNumber, "");
            patPatient.AddCell(cell);
            cell = CreateCell("", Address, "");
            patPatient.AddCell(cell);

            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));
        }

        public void Print_CCR_Alerts(Document doc, XmlDocument xmldoc)
        {

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("ALERTS", HeadFont));
            HeadCell.Colspan = 6;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Type", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Date", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Code", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Description", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Reaction", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Source", "", "");
            patPatient.AddCell(cell);

            string Type = string.Empty;
            string Date = string.Empty;
            string Code = string.Empty;
            string Description = string.Empty;
            string Reaction = string.Empty;
            string Source = string.Empty;


            XmlNodeList Doc_Alert = xmldoc.GetElementsByTagName("ccr:Body");
            foreach (XmlElement elemParent in Doc_Alert)
            {
                int count = elemParent.ChildNodes.Count;
                {
                    bool is_author = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:Alerts")
                        {
                            int name_count = elemParent.ChildNodes[i].ChildNodes.Count;
                            for (int k = 0; k < name_count; k++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[k].Name == "ccr:Alert")
                                {
                                    int birth_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes.Count;

                                    for (int l = 0; l < birth_count; l++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:DateTime")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:ApproximateDateTime")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Date = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Type")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Text")
                                                {
                                                    Type = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml;
                                                    break;
                                                }
                                                break;
                                            }
                                        }
                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Description")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Code")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Value")
                                                        {
                                                            Code = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                        }
                                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:CodingSystem")
                                                        {
                                                            Code = Code + " (" + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml + ")";
                                                        }
                                                    }
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Text")
                                                {
                                                    Description = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml;
                                                }

                                            }
                                        }

                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Reaction")
                                        {
                                            int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < temp_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Description")
                                                {
                                                    int desc_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < desc_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Reaction = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            break;
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    cell = CreateCell("", Type, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Date, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Code, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Description, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Reaction, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", (string)Session["ProviderName"], "");
                                    patPatient.AddCell(cell);
                                }
                            }
                        }
                    }
                }
            }

            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));
        }

        public void Print_CCR_Problems(Document doc, XmlDocument xmldoc)
        {

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("PROBLEMS", HeadFont));
            HeadCell.Colspan = 6;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Type", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Date", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Code", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Description", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Status", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Source", "", "");
            patPatient.AddCell(cell);


            string Type = string.Empty;
            string Date = string.Empty;
            string Code = string.Empty;
            string Description = string.Empty;
            string Status = string.Empty;
            string Source = string.Empty;

            XmlNodeList Doc_Alert = xmldoc.GetElementsByTagName("ccr:Body");
            foreach (XmlElement elemParent in Doc_Alert)
            {
                int count = elemParent.ChildNodes.Count;
                {
                    bool is_author = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:Problems")
                        {
                            int name_count = elemParent.ChildNodes[i].ChildNodes.Count;
                            for (int k = 0; k < name_count; k++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[k].Name == "ccr:Problem")
                                {
                                    Code = string.Empty;
                                    int birth_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes.Count;

                                    for (int l = 0; l < birth_count; l++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:DateTime")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:ApproximateDateTime")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Date = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Type")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Text")
                                                {
                                                    Type = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml;
                                                    break;
                                                }
                                                break;
                                            }
                                        }
                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Description")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Code")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Value")
                                                        {
                                                            if (Code == string.Empty)
                                                                Code = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            else
                                                                Code = Code + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                        }
                                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:CodingSystem")
                                                        {
                                                            Code = Code + " (" + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml + ")";
                                                        }
                                                    }
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Text")
                                                {
                                                    Description = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml;
                                                }

                                            }
                                        }

                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Status")
                                        {
                                            int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < temp_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Text")
                                                {
                                                    Status = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml;
                                                    break;
                                                }

                                            }
                                        }

                                    }
                                    cell = CreateCell("", Type, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Date, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Code, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Description, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Status, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", (string)Session["ProviderName"], "");
                                    patPatient.AddCell(cell);
                                }
                            }
                        }
                    }
                }
            }
            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));
        }

        public void Print_CCR_Medications(Document doc, XmlDocument xmldoc)
        {

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("MEDICATIONS", HeadFont));
            HeadCell.Colspan = 12;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Medication", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("RxNorm Code", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Date", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Status", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Form", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Strength", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Quantity", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("SIG", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Indications", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Instruction", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Refills", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Source", "", "");
            patPatient.AddCell(cell);


            string Medication = string.Empty;
            string RxNorm = string.Empty;
            string Date = string.Empty;
            string Status = string.Empty;
            string Form = string.Empty;
            string Strength = string.Empty;
            string Quantity = string.Empty;
            string SIG = string.Empty;
            string Indications = string.Empty;
            string Instruction = string.Empty;
            string Refills = string.Empty;
            string Source = string.Empty;

            XmlNodeList Doc_Alert = xmldoc.GetElementsByTagName("ccr:Body");
            foreach (XmlElement elemParent in Doc_Alert)
            {
                int count = elemParent.ChildNodes.Count;
                {
                    bool is_author = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "ccr:Medications")
                        {
                            int name_count = elemParent.ChildNodes[i].ChildNodes.Count;
                            for (int k = 0; k < name_count; k++)
                            {
                                if (elemParent.ChildNodes[i].ChildNodes[k].Name == "ccr:Medication")
                                {
                                    SIG = string.Empty;
                                    int birth_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes.Count;

                                    for (int l = 0; l < birth_count; l++)
                                    {
                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:DateTime")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:ApproximateDateTime")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Date = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Status")
                                        {
                                            int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < temp_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Text")
                                                {
                                                    Status = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].InnerXml;
                                                    break;
                                                }

                                            }
                                        }


                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Product")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:ProductName")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Medication = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;

                                                        }
                                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Code")
                                                        {
                                                            int med_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                            for (int j = 0; j < med_count; j++)
                                                            {
                                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[j].Name == "ccr:Value")
                                                                    RxNorm = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[j].InnerXml;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:BrandName")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Medication = Medication + " (" + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml + ")";

                                                        }
                                                    }
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Form")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Form = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;

                                                        }
                                                    }
                                                }
                                                else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Strength")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Value")
                                                        {
                                                            Strength = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                        }
                                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Units")
                                                        {
                                                            int child_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                            for (int x = 0; x < child_count; x++)
                                                            {
                                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Unit")
                                                                {
                                                                    Strength = Strength + " " + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }

                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:Directions")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Direction")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {

                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Dose")
                                                        {
                                                            int freq_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                            for (int x = 0; x < freq_count; x++)
                                                            {
                                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Value")
                                                                {
                                                                    SIG = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                }
                                                                else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Units")
                                                                {
                                                                    int val_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].ChildNodes.Count;
                                                                    for (int z = 0; z < val_count; z++)
                                                                    {
                                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].ChildNodes[z].Name == "ccr:Unit")
                                                                        {
                                                                            SIG = SIG + " " + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].ChildNodes[z].InnerXml;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Route")
                                                        {
                                                            int freq_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                            for (int x = 0; x < freq_count; x++)
                                                            {
                                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Text")
                                                                {
                                                                    SIG = SIG + " " + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                }
                                                            }

                                                        }


                                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Frequency")
                                                        {
                                                            int freq_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                            for (int x = 0; x < freq_count; x++)
                                                            {
                                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Value")
                                                                {
                                                                    if (SIG == string.Empty)
                                                                        SIG = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                    else
                                                                        SIG = SIG + " " + elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                }
                                                            }
                                                        }

                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        else if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].Name == "ccr:PatientInstructions")
                                        {
                                            int date_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes.Count;
                                            for (int m = 0; m < date_count; m++)
                                            {
                                                if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].Name == "ccr:Instruction")
                                                {
                                                    int temp_count = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes.Count;
                                                    for (int n = 0; n < temp_count; n++)
                                                    {
                                                        if (elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                        {
                                                            Instruction = elemParent.ChildNodes[i].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].InnerXml;
                                                            break;
                                                        }

                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    cell = CreateCell("", Medication, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", RxNorm, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Date, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Status, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Form, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Strength, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Quantity, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", SIG, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Indications, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Instruction, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", Refills, "");
                                    patPatient.AddCell(cell);
                                    cell = CreateCell("", (string)Session["ProviderName"], "");
                                    patPatient.AddCell(cell);
                                }
                            }
                        }
                    }
                }
            }
            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));
        }

        public void Prinit_CCR_Results(Document doc, XmlDocument xmldoc)
        {

            #region Results(Discrete)

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("RESULTS (DISCRETE)", HeadFont));
            HeadCell.Colspan = 4;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Test", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Date", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Result", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Source", "", "");
            patPatient.AddCell(cell);


            XmlNodeList Doc_Alert = xmldoc.GetElementsByTagName("ccr:Body");
            foreach (XmlElement elemParent in Doc_Alert)
            {
                string Test = string.Empty;
                string Date = string.Empty;
                string Result = string.Empty;
                string Source = string.Empty;
                string Side_header = string.Empty;
                int tcount = elemParent.ChildNodes.Count;
                {
                    for (int j = 0; j < tcount; j++)
                    {
                        if (elemParent.ChildNodes[j].Name == "ccr:Results")
                        {
                            int count = elemParent.ChildNodes[j].ChildNodes.Count;
                            {
                                for (int i = 0; i < count; i++)
                                {
                                    if (elemParent.ChildNodes[j].ChildNodes[i].Name == "ccr:Result")
                                    {

                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[5].ChildNodes[4].ChildNodes[0].Name == "ccr:Value")
                                        {
                                            int name_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes.Count;
                                            for (int k = 0; k < name_count; k++)
                                            {
                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].Name == "ccr:Description")
                                                {
                                                    int date_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes.Count;
                                                    for (int m = 0; m < date_count; m++)
                                                    {
                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:Text")
                                                        {
                                                            Test = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].InnerXml;
                                                            break;
                                                        }
                                                        break;
                                                    }
                                                }

                                                else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].Name == "ccr:DateTime")
                                                {
                                                    int date_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes.Count;
                                                    for (int m = 0; m < date_count; m++)
                                                    {
                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:ApproximateDateTime")
                                                        {
                                                            int temp_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count;
                                                            for (int n = 0; n < temp_count; n++)
                                                            {
                                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                                {
                                                                    Date = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerXml;
                                                                    break;
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }

                                                else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].Name == "ccr:Test")
                                                {
                                                    int date_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes.Count;
                                                    for (int m = 0; m < date_count; m++)
                                                    {
                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:Description")
                                                        {
                                                            int temp_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count;
                                                            for (int n = 0; n < temp_count; n++)
                                                            {
                                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                                {
                                                                    Side_header = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerXml;
                                                                }
                                                            }
                                                        }
                                                        else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:TestResult")
                                                        {
                                                            int temp_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count;
                                                            for (int n = 0; n < temp_count; n++)
                                                            {
                                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Value")
                                                                {
                                                                    Result = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerXml;
                                                                }
                                                                else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Units")
                                                                {
                                                                    int image_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                                    for (int x = 0; x < image_count; x++)
                                                                    {
                                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Unit")
                                                                        {
                                                                            Result = Result + " " + elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                }

                                            }
                                            cell = CreateCell("", Test, "");
                                            patPatient.AddCell(cell);
                                            cell = CreateCell("", Date, "");
                                            patPatient.AddCell(cell);
                                            cell = CreateCell(Side_header + " - ", Result, "");
                                            patPatient.AddCell(cell);
                                            cell = CreateCell("", (string)Session["ProviderName"], "");
                                            patPatient.AddCell(cell);


                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));

            #endregion

            #region Results(Report)

            PdfPTable patReport = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patReport.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("RESULTS (REPORT)", HeadFont));
            HeadCell.Colspan = 4;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patReport.AddCell(HeadCell);
            cell = CreateCell("Test", "", "");
            patReport.AddCell(cell);
            cell = CreateCell("Date", "", "");
            patReport.AddCell(cell);
            cell = CreateCell("Result", "", "");
            patReport.AddCell(cell);
            cell = CreateCell("Source", "", "");
            patReport.AddCell(cell);


            XmlNodeList Doc_Report = xmldoc.GetElementsByTagName("ccr:Body");
            foreach (XmlElement elemParent in Doc_Report)
            {
                string Test = string.Empty;
                string Date = string.Empty;
                string Result = string.Empty;
                string Source = string.Empty;
                string Side_header = string.Empty;
                int tcount = elemParent.ChildNodes.Count;
                {
                    for (int j = 0; j < tcount; j++)
                    {
                        if (elemParent.ChildNodes[j].Name == "ccr:Results")
                        {
                            int count = elemParent.ChildNodes[j].ChildNodes.Count;
                            {
                                for (int i = 0; i < count; i++)
                                {
                                    if (elemParent.ChildNodes[j].ChildNodes[i].Name == "ccr:Result")
                                    {
                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[5].ChildNodes[4].ChildNodes[0].Name == "ccr:Description")
                                        {
                                            int name_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes.Count;
                                            for (int k = 0; k < name_count; k++)
                                            {
                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].Name == "ccr:Description")
                                                {
                                                    int date_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes.Count;
                                                    for (int m = 0; m < date_count; m++)
                                                    {
                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:Text")
                                                        {
                                                            Test = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].InnerXml;
                                                            break;
                                                        }
                                                        break;
                                                    }
                                                }

                                                else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].Name == "ccr:DateTime")
                                                {
                                                    int date_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes.Count;
                                                    for (int m = 0; m < date_count; m++)
                                                    {
                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:ApproximateDateTime")
                                                        {
                                                            int temp_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count;
                                                            for (int n = 0; n < temp_count; n++)
                                                            {
                                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                                {
                                                                    Date = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerXml;
                                                                    break;
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }

                                                else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].Name == "ccr:Test")
                                                {
                                                    int date_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes.Count;
                                                    for (int m = 0; m < date_count; m++)
                                                    {
                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:Description")
                                                        {
                                                            int temp_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count;
                                                            for (int n = 0; n < temp_count; n++)
                                                            {
                                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Text")
                                                                {
                                                                    Side_header = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].InnerXml;
                                                                }
                                                            }
                                                        }
                                                        else if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].Name == "ccr:TestResult")
                                                        {
                                                            int temp_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes.Count;
                                                            for (int n = 0; n < temp_count; n++)
                                                            {
                                                                if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].Name == "ccr:Description")
                                                                {
                                                                    int image_count = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes.Count;
                                                                    for (int x = 0; x < image_count; x++)
                                                                    {
                                                                        if (elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[x].Name == "ccr:Text")
                                                                        {
                                                                            Result = elemParent.ChildNodes[j].ChildNodes[i].ChildNodes[k].ChildNodes[m].ChildNodes[n].ChildNodes[x].InnerXml;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                }

                                            }
                                            cell = CreateCell("", Test, "");
                                            patReport.AddCell(cell);
                                            cell = CreateCell("", Date, "");
                                            patReport.AddCell(cell);
                                            cell = CreateCell(Side_header + " - ", Result, "");
                                            patReport.AddCell(cell);
                                            cell = CreateCell("", (string)Session["ProviderName"], "");
                                            patReport.AddCell(cell);

                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            doc.Add(patReport);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));

            #endregion
        }

        public void Prinit_CCR_People(Document doc, XmlDocument xmldoc)
        {
            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("PEOPLE", HeadFont));
            HeadCell.Colspan = 6;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Name", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Specialty", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Relation", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Identification Numbers", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Phone", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Address / Email", "", "");
            patPatient.AddCell(cell);

            XmlNodeList Doc_From = xmldoc.GetElementsByTagName("ccr:Actors");
            foreach (XmlElement elemParent in Doc_From)
            {
                int count = elemParent.ChildNodes.Count;
                {
                    for (int a = 0; a < count; a++)
                    {
                        if (elemParent.ChildNodes[a].Name == "ccr:Actor")
                        {
                            if (elemParent.ChildNodes[a].ChildNodes[0].InnerXml == "PatientID_1")
                            {
                                string Name = string.Empty;
                                string Specialty = string.Empty;
                                string Relation = string.Empty;
                                string Identification = string.Empty;
                                string Phone = string.Empty;
                                string Address = string.Empty;

                                try
                                {
                                    Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerXml;
                                }
                                catch
                                {
                                    Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerXml;
                                }
                                Identification = elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[0].InnerXml;
                                Phone = elemParent.ChildNodes[a].ChildNodes[4].ChildNodes[0].InnerXml;
                                Address = elemParent.ChildNodes[a].ChildNodes[3].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[3].ChildNodes[1].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[3].ChildNodes[2].InnerXml;

                                cell = CreateCell("", Name, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Specialty, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Relation, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Identification, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Phone, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Address, "");
                                patPatient.AddCell(cell);
                            }

                            else if (elemParent.ChildNodes[a].ChildNodes[0].InnerXml == "AuthorID_01")
                            {
                                string Name = string.Empty;
                                string Specialty = string.Empty;
                                string Relation = string.Empty;
                                string Identification = string.Empty;
                                string Phone = string.Empty;
                                string Address = string.Empty;


                                Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerXml;
                                Phone = elemParent.ChildNodes[a].ChildNodes[3].ChildNodes[0].InnerXml;
                                Address = elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[1].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[2].InnerXml;

                                cell = CreateCell("", Name, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Specialty, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Relation, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Identification, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Phone, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Address, "");
                                patPatient.AddCell(cell);


                            }
                            else if (elemParent.ChildNodes[a].ChildNodes[0].InnerXml == "RecipientID_01")
                            {
                                string Name = string.Empty;
                                string Specialty = string.Empty;
                                string Relation = string.Empty;
                                string Identification = string.Empty;
                                string Phone = string.Empty;
                                string Address = string.Empty;


                                Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerXml;

                                cell = CreateCell("", Name, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Specialty, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Relation, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Identification, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Phone, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Address, "");
                                patPatient.AddCell(cell);

                            }
                        }

                    }
                }
            }
            doc.Add(patPatient);
            doc.Add(new Paragraph("   "));
            doc.Add(new Paragraph("   "));

        }

        public void Prinit_CCR_Organization(Document doc, XmlDocument xmldoc)
        {
            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("ORGANIZATIONS", HeadFont));
            HeadCell.Colspan = 6;
            HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
            patPatient.AddCell(HeadCell);
            cell = CreateCell("Name", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Specialty", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Relation", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Identification Numbers", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Phone", "", "");
            patPatient.AddCell(cell);
            cell = CreateCell("Address / Email", "", "");
            patPatient.AddCell(cell);


            XmlNodeList Doc_From = xmldoc.GetElementsByTagName("ccr:Actors");
            foreach (XmlElement elemParent in Doc_From)
            {
                int count = elemParent.ChildNodes.Count;
                {
                    for (int a = 0; a < count; a++)
                    {
                        if (elemParent.ChildNodes[a].Name == "ccr:Actor")
                        {
                            if (elemParent.ChildNodes[a].ChildNodes[0].InnerXml == "ManufID_1")
                            {
                                string Name = string.Empty;
                                string Specialty = string.Empty;
                                string Relation = string.Empty;
                                string Identification = string.Empty;
                                string Phone = string.Empty;
                                string Address = string.Empty;


                                Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].InnerXml;

                                cell = CreateCell("", Name, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Specialty, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Relation, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Identification, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Phone, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Address, "");
                                patPatient.AddCell(cell);
                            }
                            else if (elemParent.ChildNodes[a].ChildNodes[0].InnerXml == "ManufID_2")
                            {
                                string Name = string.Empty;
                                string Specialty = string.Empty;
                                string Relation = string.Empty;
                                string Identification = string.Empty;
                                string Phone = string.Empty;
                                string Address = string.Empty;


                                Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].InnerXml;

                                cell = CreateCell("", Name, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Specialty, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Relation, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Identification, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Phone, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Address, "");
                                patPatient.AddCell(cell);
                            }

                            else if (elemParent.ChildNodes[a].ChildNodes[0].InnerXml == "ClinicID_01")
                            {
                                string Name = string.Empty;
                                string Specialty = string.Empty;
                                string Relation = string.Empty;
                                string Identification = string.Empty;
                                string Phone = string.Empty;
                                string Address = string.Empty;


                                Name = elemParent.ChildNodes[a].ChildNodes[1].ChildNodes[0].InnerXml;
                                Address = elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[0].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[1].InnerXml + " " + elemParent.ChildNodes[a].ChildNodes[2].ChildNodes[2].InnerXml;

                                cell = CreateCell("", Name, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Specialty, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Relation, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Identification, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Phone, "");
                                patPatient.AddCell(cell);
                                cell = CreateCell("", Address, "");
                                patPatient.AddCell(cell);
                            }

                        }
                    }
                }
            }

            doc.Add(patPatient);
        }


        public void PrintResultsforPatientPortal(Document doc, XmlDocument xmldoc)
        {
            try
            {
                sNoInformationInTable = string.Empty;
                DataSet dsResults = null;
                XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                foreach (XmlElement elemParent in Doc_Results_Node)
                {
                    bool is_break = false;
                    for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "RESULTS")
                        {
                            dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            sNoInformationInTable = elemParent.ChildNodes[i + 1].InnerXml;
                            is_break = true;
                            break;
                        }
                    }
                    if (is_break == true)
                        break;
                }

                if (dsResults != null && dsResults.Tables.Count > 0)
                {
                    PdfPTable patResults = new PdfPTable(dsResults.Tables[0].Columns.Count);
                    patResults.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                    HeadCell.Colspan = dsResults.Tables[0].Columns.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patResults.AddCell(HeadCell);


                    for (int j = 0; j < dsResults.Tables[0].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsResults.Tables[0].Columns[j].ColumnName);
                        cell = CreateCell(ColumnName, "", "");
                        patResults.AddCell(cell);
                    }

                    foreach (DataRow row in dsResults.Tables[0].Rows)
                    {
                        foreach (DataColumn column in dsResults.Tables[0].Columns)
                        {
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if (ColumnData.Contains("&lt;"))
                                ColumnData = ColumnData.Replace("&lt;", "<");
                            else if (ColumnData.Contains("&gt;"))
                                ColumnData = ColumnData.Replace("&gt;", ">");
                            cell = CreateCell("", ColumnData, "");
                            patResults.AddCell(cell);
                        }
                    }
                    doc.Add(patResults);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                    ResultMaster objmaster = new ResultMaster();
                    objmaster.Is_Electronic_Mode = "N";
                    objmaster.Created_Date_And_Time = universalTime;
                    objmaster.Created_By = ClientSession.UserName;
                    lstresultmaster.Add(objmaster);


                    ResultOBR objobr = new ResultOBR();
                    objobr.OBR_Observation_Battery_Text = "Blood chemistry";
                    objobr.Created_Date_And_Time = universalTime;
                    objobr.Created_By = ClientSession.UserName;
                    objobr.OBR_Specimen_Collection_Date_And_Time = universalTime.ToString("yyyyMMddhhmmss");
                    lstresultobr.Add(objobr);


                    ResultORC objorc = new ResultORC();
                    objorc.Created_Date_And_Time = universalTime;
                    objorc.Created_By = ClientSession.UserName;
                    lstresultorc.Add(objorc);

                    for (int i = 0; i < dsResults.Tables[0].Rows.Count; i++)
                    {

                        ResultOBX objresult = new ResultOBX();
                        string order = StripTagsRegex(dsResults.Tables[0].Rows[i][0].ToString());
                        if (order != string.Empty)
                        {
                            try
                            {
                                string[] orders = order.Split(' ');
                                objresult.OBX_Loinc_Observation_Text = orders[0];
                                if (order.Contains('(') == true)
                                {
                                    string strOrder = order.Substring(order.IndexOf('('), order.Length - order.IndexOf('('));

                                    objresult.OBX_Reference_Range = strOrder;

                                }
                                //if (orders.Length >= 4)//for bug id 30256
                                //{
                                //    objresult.OBX_Reference_Range = orders[1] + " " + orders[2] + " " + orders[3];
                                //}
                                //else if (orders.Length >= 3)
                                //{
                                //    objresult.OBX_Reference_Range = orders[1] + " " + orders[2];
                                //}
                                else
                                {
                                    objresult.OBX_Reference_Range = orders[1];
                                }
                                order = StripTagsRegex(dsResults.Tables[0].Rows[i].Field<string>("Blood chemistry"));
                                orders = order.Split(' ');
                                objresult.OBX_Observation_Value = orders[0];
                                objresult.Created_Date_And_Time = universalTime;
                                objresult.Created_By = ClientSession.UserName;
                                lstresultobx.Add(objresult);
                            }
                            catch
                            {
                                //do nothing
                            }
                        }
                    }

                }
                else
                {
                    
                    PdfPTable emptyPDF = new PdfPTable(1);
                    emptyPDF.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    emptyPDF.AddCell(HeadCell);
                    if (sNoInformationInTable == string.Empty)
                    {
                        cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                    }
                    else
                    {
                        cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                    }
                    emptyPDF.AddCell(cell);
                    doc.Add(emptyPDF);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));
                }

                Session["ResultMasterList"] = lstresultmaster;
                Session["ResultObrList"] = lstresultobr;
                Session["ResultObxList"] = lstresultobx;
                Session["ResultOrcList"] = lstresultorc;

            }
            catch
            {
                DataSet dsResults = null;
                XmlNodeList Doc_Results_Node = xmldoc.GetElementsByTagName("section");
                foreach (XmlElement elemParent in Doc_Results_Node)
                {
                    bool is_break = false;
                    for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                    {
                        if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "RESULTS")
                        {
                            dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            is_break = true;
                            break;
                        }
                    }
                    if (is_break == true)
                        break;
                }

                if (dsResults != null && dsResults.Tables.Count > 0)
                {
                    PdfPTable patResults = new PdfPTable(dsResults.Tables[0].Columns.Count);
                    patResults.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                    HeadCell.Colspan = dsResults.Tables[0].Columns.Count;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    patResults.AddCell(HeadCell);


                    for (int j = 0; j < dsResults.Tables[0].Columns.Count; j++)
                    {
                        string ColumnName = StripTagsRegex(dsResults.Tables[0].Columns[j].ColumnName);
                        cell = CreateCell(ColumnName, "", "");
                        patResults.AddCell(cell);
                    }

                    foreach (DataRow row in dsResults.Tables[0].Rows)
                    {
                        foreach (DataColumn column in dsResults.Tables[0].Columns)
                        {
                            string ColumnData = StripTagsRegex(row[column].ToString());
                            if (ColumnData.Contains("&lt;"))
                                ColumnData = ColumnData.Replace("&lt;", "<");
                            else if (ColumnData.Contains("&gt;"))
                                ColumnData = ColumnData.Replace("&gt;", ">");
                            cell = CreateCell("", ColumnData, "");
                            patResults.AddCell(cell);
                        }
                    }
                    doc.Add(patResults);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));

                }
                else
                {
                    PdfPTable emptyPDF = new PdfPTable(1);
                    emptyPDF.WidthPercentage = 100;
                    HeadCell = new PdfPCell(new Phrase("RESULTS", HeadFont));
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = BaseColor.DARK_GRAY;
                    emptyPDF.AddCell(HeadCell);
                    if (sNoInformationInTable == string.Empty)
                    {
                        cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                    }
                    else
                    {
                        cell = CreateCell("", "     " + StripTagsRegex(sNoInformationInTable), "");
                    }
                    emptyPDF.AddCell(cell);
                    doc.Add(emptyPDF);
                    doc.Add(new Paragraph("   "));
                    doc.Add(new Paragraph("   "));
                }
            }
        }

        public void SaveSummaryOfCareInDatabase()
        {
            SummaryOfCareDTO objsummarydto = new SummaryOfCareDTO();
            objsummarydto.HumanList = (IList<Human>)Session["HumanList"];
            objsummarydto.PhysicianList = (IList<PhysicianLibrary>)Session["PhysicianLibrary"];
            objsummarydto.FacilityList = (IList<FacilityLibrary>)Session["FacilityLibrary"];
            objsummarydto.EncounterList = (IList<Encounter>)Session["Encounter"];
            //objsummarydto.AllergyList = (IList<Rcopia_Allergy>)Session["AllergyList"];
            objsummarydto.ImmunizationList = (IList<Immunization>)Session["ImmunizationList"];
            //objsummarydto.MedicationList = (IList<Rcopia_Medication>)Session["MedicationList"];
            //objsummarydto.ProblemList = (IList<ProblemList>)Session["ProblemList"];
            objsummarydto.SocialHistoryList = (IList<SocialHistory>)Session["SocialHistoryList"];
            objsummarydto.VitalList = (IList<PatientResults>)Session["Vitals"];
            objsummarydto.CareplanList = (IList<CarePlan>)Session["CarePlanList"];
            objsummarydto.TreatmentPlanList = (IList<TreatmentPlan>)Session["TreatmentPlanList"];
            objsummarydto.ReferalOrderList = (IList<ReferralOrder>)Session["ReferalOrderList"];
            objsummarydto.ProcedureList = (IList<InHouseProcedure>)Session["ProcedureList"];
            objsummarydto.ResultMasterList = (IList<ResultMaster>)Session["ResultMasterList"];
            objsummarydto.ResultObrList = (IList<ResultOBR>)Session["ResultObrList"];
            objsummarydto.ResultObxList = (IList<ResultOBX>)Session["ResultObxList"];
            objsummarydto.ResultOrcList = (IList<ResultORC>)Session["ResultOrcList"];


            objsummarydto.goalList = (IList<TreatmentPlan>)Session["GoalsSectionList"];
            objsummarydto.mentalstatuslist = (IList<CarePlan>)Session["MentalStatusList"];
            objsummarydto.ImplantList = (IList<InHouseProcedure>)Session["ImplantList"];
            objsummarydto.healthconcernlist = (IList<ProblemList>)Session["HealthConcernList"];

            EncounterManager objencounter = new EncounterManager();

            Session["sReturn"] = objencounter.SaveSummaryOfCare(objsummarydto, ClientSession.UserName, ClientSession.FacilityName, ClientSession.EncounterId, ClientSession.LegalOrg);
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            ActivityLog activity = new ActivityLog();
            activity.Activity_Type = "CCD Import";
            activity.Activity_Date_And_Time = DateTime.UtcNow;
            activity.Encounter_ID = (objsummarydto.EncounterList != null && objsummarydto.EncounterList.Count > 0 && objsummarydto.EncounterList[0].Id != 0) ? objsummarydto.EncounterList[0].Id : ClientSession.EncounterId;
            activity.Human_ID = (objsummarydto.HumanList!=null && objsummarydto.HumanList.Count>0 && objsummarydto.HumanList[0].Id!=0)?objsummarydto.HumanList[0].Id:ClientSession.HumanId;
            activity.Subject = Session["sFileNameBind"].ToString();
            activity.Activity_By = ClientSession.UserName;
            Session["ReconciledEncID"] = activity.Encounter_ID;//BugID:51164,to be used for ActivityLog entry - type : CCD RECONCILE
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            sReturn = Session["sReturn"].ToString();
            string[] shumEncSplit = sReturn.Split('_');
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            ActivityLog activity = new ActivityLog();
            activity.Activity_Type = "CCD Incorporated";
            activity.Activity_Date_And_Time = DateTime.UtcNow;
            activity.Encounter_ID = Convert.ToUInt64(shumEncSplit[1]);
            activity.Human_ID = Convert.ToUInt64(shumEncSplit[0]);
            activity.Subject = Session["sFileNameBind"].ToString();
            activity.Activity_By = ClientSession.UserName;
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            MoveFile(Request["FileName"]);

        }
        //BugID:51185 - File moved to ImportedFiles folder on Reconcile Click.
        public void MoveFile(string filename)
        {
            string PhiMailDirectory = System.Configuration.ConfigurationManager.AppSettings["phiMailDownloadDirectory"].ToString();

            string filrnsmrnew = Path.GetFileName(filename);
           
            if (!Directory.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles"))
                Directory.CreateDirectory(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles");
            if (File.Exists(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + filrnsmrnew))
                File.Delete(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + filrnsmrnew);
            File.Move(PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\" + filrnsmrnew, PhiMailDirectory + "\\" + ClientSession.PhysicianId + "\\ImportedFiles\\" + filrnsmrnew);
        }

        //protected void btnGenerate_Click(object sender, EventArgs e)
        //{
        //    IList<string> SelectedSummaryItems=new List<string>();
        //    for (int i = 0; i < SummaryCheckList.Items.Count; i++)
        //    {
        //        if (SummaryCheckList.Items[i].Selected == true)
        //            SelectedSummaryItems.Add(SummaryCheckList.Items[i].Value);
        //    }
           
        //    if (SelectedSummaryItems != null && SelectedSummaryItems.Count > 0)
        //    {
        //        if (SelectedSummaryItems.Count == 1 && SelectedSummaryItems[0] == "All")
        //            GenerateEntireSummary();
        //        else
        //        GeneratePDFforSummary(SelectedSummaryItems);
        //    }
        //}

        [System.Web.Services.WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GeneratePDFforSummary(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string retVal = string.Empty;
            frmSummaryOfCare objsumm = new frmSummaryOfCare();
            IList<string> SelectedSummaryItems = data.ToList<string>();
            if (SelectedSummaryItems != null && SelectedSummaryItems.Count > 0)
            {
                if (SelectedSummaryItems.Count == 1 && SelectedSummaryItems[0] == "All")
                    retVal=objsumm.GenerateEntireSummary();
                else
                   retVal= objsumm.GeneratePDFforSummary(SelectedSummaryItems);
            }
            return retVal;
        }
        
        public  string GeneratePDFforSummary(IList<string> SelectedSummaryItems)
        {
            System.IO.File.Delete((string)HttpContext.Current.Session["PDF_Path"]);
            string XmlPath = FilePath;

            
            XmlDocument xmldoc = new XmlDocument();
            XmlTextReader xmltext = new XmlTextReader(XmlPath);
            xmldoc.Load(xmltext);
            string path = Path.GetFileNameWithoutExtension(XmlPath);
            string TargetFileDirectory = Server.MapPath("Documents/" + HttpContext.Current.Session.SessionID);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 100, 70);
            string sFolderPathName = TargetFileDirectory + "\\" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            Directory.CreateDirectory(sFolderPathName);
            string sPrintPathName = string.Empty;
            sPrintPathName = sFolderPathName + "\\" + path ;
            HttpContext.Current.Session["PDF_Path"] = sPrintPathName;
            string sMyPathName = sPrintPathName;
            PdfWriter wr;
            string[] sFileName = sMyPathName.Split('\\');
            try
            {
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            catch
            {
                var process = System.Diagnostics.Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(sFileName[3])).ToList();
                process[0].Kill();
                Thread.Sleep(500);
                wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            float X = 20f, Y = 20f;
            PrintPatient(doc, xmldoc);
            for(int i=0;i<SelectedSummaryItems.Count;i++){
               
                switch (SelectedSummaryItems[i])
                {
                    case "Allergy":
                        {
                            PrintAllergy(doc, xmldoc);
                        } break;
                    case "Immunization": PrintImmunization(doc, xmldoc); break;
                    case "Medication": PrintMedication(doc, xmldoc); break;
                    case "Care Plan": PrintCarePlan(doc, xmldoc); break;
                    case "Reason for Referral": PrintReasonForReferal(doc, xmldoc); break;
                    case "Procedures": PrintProcedures(doc, xmldoc); break;
                    case "Results": PrintResults(doc, xmldoc, FileName, Type); break;
                    case "Social History": PrintSocialHistory(doc, xmldoc); break;
                    case "Vitals": PrintVitals(doc, xmldoc); break;
                    case "Instructions": PrintInstructions(doc, xmldoc); break;
                    case "Chief Complaint": PrintChiefComplaint(doc, xmldoc); break;
                    case "Problems": PrintProblems(doc, xmldoc, FileName, Type); break;
                    case "Provider": PrintProvider(doc, xmldoc); break;
                    case "Custodian": PrintCustodian(doc, xmldoc); break;
                    case "Documentation Details": PrintDocumentDetail(doc, xmldoc); break;
                    case "Visit Details": PrintVisitDetail(doc, xmldoc); break;
                    case "Author": PrintAuthor(doc, xmldoc); break;
                    case "Medication Administered": PrintMedicationAdministered(doc, xmldoc); break;
                    case "Functional Status": PrintFunctionalStatus(doc, xmldoc); break;
                    case "Hospital Discharge Instructions": PrintHospitalInstructions(doc, xmldoc); break;
                    case "Implant": PrintImplant(doc, xmldoc); break;
                    case "Mental Status": PrintMentalStatus(doc, xmldoc); break;
                    case "Goals Section": PrintGoalsSection(doc, xmldoc);break;
                    case "Health Concern": PrintHealthConcernSection(doc, xmldoc); break;
                    case "Treatment Plan": PrintTreatmentPlan(doc, xmldoc); break;
                    case "Laboratory Information": PrintLaboratoryInformation(doc, xmldoc); break;
                    case "Interventions": PrintInterventions(doc, xmldoc); break;
                    case "Health Status Evaluations/Outcomes": PrintHealthStatusEvaluations(doc, xmldoc); break;
                    default:break;
                }
               
            }
            doc.Close();
            xmltext.Close();
            string retVal="frmSummaryOfCare.aspx?FileName=" + FileName.ToString() + "&pdf=" + (string)HttpContext.Current.Session["PDF_Path"] + "&Type=" + Type.ToString() + "&UniversalTime=" + UniverTime;
            return retVal;
            //PDFLOAD.Attributes.Add("src", );
           
        }

        public  string GenerateEntireSummary()
        {
            System.IO.File.Delete((string)Session["PDF_Path"]);
            string retVal=string.Empty;
            if (FileName != null && (Type == "CCD" || Type == "C32"))
            {
               
                    path = PrintPDF(FilePath, "CCD", DateTime.MinValue,FileName,Type);
                    retVal = "frmSummaryOfCare.aspx?FileName=" + FileName.ToString() + "&pdf=" + (string)HttpContext.Current.Session["PDF_Path"] + "&Type=" + Type.ToString() + "&UniversalTime=" + UniverTime;
              
            }
            return retVal;
        }
    }
}
