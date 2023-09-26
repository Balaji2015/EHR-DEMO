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
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml;
using System.Net;
using System.Threading;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Web.Script.Services;



namespace Acurus.Capella.UI
{
    public partial class frmSummaryOfCare : System.Web.UI.Page
    {
        iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, new BaseColor(60, 141, 188)); // iTextSharp.text.BaseColor.BLUE);
        iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("Calibri", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
        iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        BaseColor MyColor = new BaseColor(8, 128, 170); //60, 141, 188);

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
                StaticLookupManager objlookupmanager = new StaticLookupManager();
                if (Request["FileName"] != null && (Request["Type"] == "CCD" || Request["Type"] == "C32"))
                {
                    //string[] StaticValues = { "All", "Provider", "Custodian", "Documentation Details", "Visit Details", "Author" };
                    string[] StaticValues = { "All" };
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

                        if (((IList<Human>)Session["HumanList"]).Count > 0)
                            hdnHumanID.Value = ((IList<Human>)Session["HumanList"])[0].Id.ToString();
                        //if (ClientSession.Load_Summary_PDF == false)
                        //{
                        //    Match(hdnHumanID.Value);
                        //    ClientSession.Load_Summary_PDF = true;
                        //}
                        if (ViewState["Load_Summary_PDF"] == null && Request["IsFMIEntry"] == null && Request["IsFMIEntry"] != "Y")
                        {
                            Match(hdnHumanID.Value);
                            ViewState["Load_Summary_PDF"] = "True";
                        }
                        PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString() + "&IsFMIEntry=Y");
                    }
                    else if (Request["FileName"] != null && Request["Type"] == "CCR")
                    {
                        PrintPDF_CCR(Server.MapPath(Request["FileName"]));

                        PDFLOAD.Attributes.Add("src", "frmSummaryOfCare.aspx?FileName=" + Request["FileName"].ToString() + "&pdf=" + (string)Session["PDF_Path"] + "&Type=" + Request["Type"].ToString() + "&UniversalTime=" + Request["UniversalTime"].ToString());
                    }

                }





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

            SummaryCheckList.Attributes.Add("onclick", "onchangeCheckBox();");
            //SummaryCheckList.SelectedIndexChanged += new EventHandler(SummaryCheckList_SelectedIndexChanged);
        }

        public string PrintPDF(string XmlPath, string sClinicalSummaryPath, DateTime universaltime)
        {
            universalTime = universaltime;
            XmlDocument xmldoc = new XmlDocument();
            XmlTextReader xmltext = new XmlTextReader(XmlPath);
            xmldoc.Load(xmltext);
            string path = Path.GetFileNameWithoutExtension(XmlPath);
            string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 70, 70);
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
                sPrintPathName = sFolderPathName + "\\SummaryOfCare_" + path + ".pdf";
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
                    ms.Close();
                    ms.Dispose();
                }
                // wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //float X = 20f, Y = 20f;

            if (sClinicalSummaryPath == "PatientPortal")
                ViewState["PatientPortal"] = "PatientPortal";

            PrintPatient(doc, xmldoc);

            PrintReferringProvider(doc, xmldoc);

            PrintAuthor(doc, xmldoc);

            PrintCustodian(doc, xmldoc);

            PrintProvider(doc, xmldoc);

            PrintDocumentDetail(doc, xmldoc);

            XmlNodeList Doc_Section_Node = xmldoc.GetElementsByTagName("section");
            System.Web.UI.WebControls.ListItem chkbx = new System.Web.UI.WebControls.ListItem();
            string sTitle = string.Empty;
            foreach (XmlElement elemParent in Doc_Section_Node)
            {
                sTitle = PrintSection(doc, elemParent,string.Empty);

                if (sTitle != string.Empty)
                {
                    chkbx = new System.Web.UI.WebControls.ListItem();
                    chkbx.Text = sTitle;
                    SummaryCheckList.Items.Add(chkbx);
                }
            }

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

        public string PrintPDF(string XmlPath, string sClinicalSummaryPath, DateTime universaltime, string FileName, string Type)
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
                sPrintPathName = sFolderPathName + "\\SummaryOfCare_" + path + ".pdf";
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
                   ms.Close();
                    ms.Dispose();
                }
                // wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            }
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //float X = 20f, Y = 20f;

            if (sClinicalSummaryPath == "PatientPortal")
                ViewState["PatientPortal"] = "PatientPortal";

            PrintPatient(doc, xmldoc);

            PrintReferringProvider(doc, xmldoc);

            PrintAuthor(doc, xmldoc);

            PrintCustodian(doc, xmldoc);

            PrintProvider(doc, xmldoc);

            PrintDocumentDetail(doc, xmldoc);

            XmlNodeList Doc_Section_Node = xmldoc.GetElementsByTagName("section");
            System.Web.UI.WebControls.ListItem chkbx = new System.Web.UI.WebControls.ListItem();
            string sTitle = string.Empty;
            foreach (XmlElement elemParent in Doc_Section_Node)
            {
                sTitle = PrintSection(doc, elemParent,string.Empty);

                //if (sTitle != string.Empty)
                //{
                //    chkbx = new System.Web.UI.WebControls.ListItem();
                //    chkbx.Text = sTitle;
                //    if (SummaryCheckList != null)
                //        SummaryCheckList.Items.Add(chkbx);
                //}
            }

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
            sPrintPathName = sFolderPathName + "\\SummaryOfCare_" + path + ".pdf";
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
            //float X = 20f, Y = 20f;

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
            //return Regex.Replace(source, "<.*?>", string.Empty);
            {
                if (source.Contains("<br></br>") == true)
                {
                    string str = "\"urn:hl7-org:v3\"";
                    source = source.Replace("<paragraph xmlns=" + str + ">", Environment.NewLine + Environment.NewLine + Environment.NewLine);
                    return Regex.Replace(source.Replace("</br", Environment.NewLine + "</"), "<.*?>", string.Empty).Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                }
                else
                {
                    return Regex.Replace(source.Replace("</", Environment.NewLine + "</"), "<.*?>", string.Empty).Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
                }
            }
        }

        private PdfPCell CreateCell(string HeaderText, string ValueText, string ModuleText)
        {
            PdfPCell cell = new PdfPCell();
            cell.UseVariableBorders = true;
            cell.BorderColorLeft = BaseColor.WHITE;
            cell.BorderColorTop = BaseColor.WHITE;
            cell.BorderColorRight = BaseColor.WHITE;
            cell.BorderColorBottom = new BaseColor(193, 238, 254);
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            //cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            //cell.ExtraParagraphSpace = 2;
            Paragraph par = new Paragraph(HeaderText, reducedFont);
            //par.Alignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            cell.AddElement(par);
            par = new Paragraph(ValueText, normalFont);
            //par.Alignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            cell.AddElement(par);
            par = new Paragraph(ModuleText, HeadFont);
            //par.Alignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            cell.AddElement(par);
            return cell;
        }

        private DataSet ConvertHTMLTablesToDataSet(string HTML)
        {
            // Declarations 
            DataSet ds = new DataSet();
            DataTable dt = null;
            DataRow dr = null;
            //DataColumn dc = null;
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
                        dt.Columns.Add(Header.Groups[1].ToString().Replace("<tr>", "").Replace("<th>", ""));
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

        private DataSet ConvertHTMLTablesToDataSet(string HTML, string Type)
        {
            // Declarations 
            DataSet ds = new DataSet();
            DataTable dt = null;
            DataRow dr = null;
            //DataColumn dc = null;
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

        protected void Match(string sAccountNo)
        {
            Boolean bAlreadyImport = (Boolean)Session["SummaryCareAlreadyImported"];

            if (bAlreadyImport == true)
                return;

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
                //if (ftpImageProcess.CreateDirectory(sAccountNo, ftpServerIP, ftpUserID, ftpPassword))
                bool bCreateDirectory = ftpImageProcess.CreateDirectory(sAccountNo, ftpServerIP, ftpUserID, ftpPassword,out string sCheckFileNotFoundException);
                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                    return;
                }
                if (bCreateDirectory)
                {
                    string tempSelectPath = (string)Session["PDF_Path"];
                    lastNumToAdd = Convert.ToString(prevNum + 1);
                    if (lastNumToAdd.Length == 1)
                        lastNumToAdd = "0" + lastNumToAdd;
                    sStoringFormat = ClientSession.FacilityName.Replace("#", "") + "_SUMMARY_OF_CARE_" + DateTime.Now.ToString("yyyyMMdd") + "_" + sAccountNo + "_" + lastNumToAdd + ".pdf";
                    sFTPAddress = ftpImageProcess.UploadToImageServer(sAccountNo, ftpServerIP, ftpUserID, ftpPassword, tempSelectPath, sStoringFormat, out string sCheckFileNotFoundExceptions);
                    if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                        return;
                    }
                    if (sFTPAddress != string.Empty)
                    {
                        FileManagementIndex objfileIndex = new FileManagementIndex();
                        objfileIndex.File_Path = sFTPAddress;
                        objfileIndex.Created_By = ClientSession.UserName;
                        objfileIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();//UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnLocalTime.Value));
                        objfileIndex.Human_ID = Convert.ToUInt64(sAccountNo);
                        objfileIndex.Source = "SUMMARY OF CARE";
                        fileList.Add(objfileIndex);
                        IList<FileManagementIndex> fileIndex = objfilemanager.SaveUpdateDeleteFileManagementIndexforExamPhotos(fileList.ToArray(), null, null, string.Empty, "SUMMARY OF CARE");
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

        public void PrintResults(Document doc, XmlDocument xmldoc, string fileName, string Type)
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
                                    dsResults = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml, Type);
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
                        HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        HeadCell.BackgroundColor = MyColor; // BaseColor.DARK_GRAY;
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
                        HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        HeadCell.BackgroundColor = MyColor; // BaseColor.DARK_GRAY;
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
                                            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            HeadCell.BackgroundColor = MyColor; // BaseColor.DARK_GRAY;
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
                                            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            HeadCell.HorizontalAlignment = 1;
                                            HeadCell.BackgroundColor = MyColor; // BaseColor.DARK_GRAY;
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
                    HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    HeadCell.HorizontalAlignment = 1;
                    HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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

        public string PrintSection(Document doc, XmlElement elemParent, string selectedItem)
        {

            #region Section
            sNoInformationInTable = string.Empty;

            DataSet dsSection = null;
            string sContent = string.Empty;

            string sTitle = string.Empty;

            XmlNodeList Doc_title_Node = xmldoc.GetElementsByTagName("title");
            sTitle = elemParent.GetElementsByTagName("title")[0].InnerText;

            if (selectedItem != string.Empty && selectedItem.ToUpper() != sTitle.ToUpper())
                return string.Empty;

            if (elemParent.InnerXml.Contains("<table") == true && elemParent.InnerXml.Contains("<thead") == true)
            {
                dsSection = ConvertHTMLTablesToDataSet(elemParent.GetElementsByTagName("text")[0].InnerXml);
            }
            else if (elemParent.InnerXml.Contains("<paragraph") == true || elemParent.InnerXml.Contains("<text") == true)
            {
                sContent = elemParent.GetElementsByTagName("text")[0].InnerXml;
            }
            else if (elemParent.InnerXml.Contains("<list") == true)
            {
                sContent = elemParent.GetElementsByTagName("list")[0].InnerXml;
            }
            //else if (elemParent.InnerXml.Contains("<table")==true)
            //{
            //    dsSection = ConvertHTMLTablesToDataSet(elemParent.GetElementsByTagName("text")[0].InnerXml);
            //}

            if (dsSection != null && dsSection.Tables.Count > 0)
            {
                PdfPTable patSection = new PdfPTable(dsSection.Tables[0].Columns.Count);
                patSection.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase(sTitle, HeadFont));
                HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                HeadCell.MinimumHeight = 18;
                HeadCell.Colspan = dsSection.Tables[0].Columns.Count;
                //HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = MyColor;  //BaseColor.DARK_GRAY;
                patSection.AddCell(HeadCell);

                for (int j = 0; j < dsSection.Tables[0].Columns.Count; j++)
                {
                    string ColumnName = StripTagsRegex(dsSection.Tables[0].Columns[j].ColumnName);
                    cell = CreateCell(ColumnName, "", "");
                    patSection.AddCell(cell);
                }

                foreach (DataRow row in dsSection.Tables[0].Rows)
                {
                    foreach (DataColumn column in dsSection.Tables[0].Columns)
                    {
                        string ColumnData = StripTagsRegex(row[column].ToString());
                        if ((ColumnData.EndsWith(",")) && (ColumnData.Length > 0))
                        {
                            ColumnData = ColumnData.Substring(0, ColumnData.Length - 1);
                        }

                        cell = CreateCell("", ColumnData, "");
                        patSection.AddCell(cell);
                    }
                }

                doc.Add(patSection);
                doc.Add(new Paragraph("   "));
                //doc.Add(new Paragraph("   "));

                XmlElement medItemDoc = null;
                string reason = string.Empty;

                if (elemParent.InnerXml.ToUpper().Contains("<TITLE>MEDICATIONS<") == true || elemParent.InnerXml.ToUpper().Contains("<TITLE>VITAL SIGNS<") == true || elemParent.InnerXml.ToUpper().Contains("<TITLE>PROBLEMS<") == true)
                {
                    XmlNodeList Doc_Medication_Node = xmldoc.GetElementsByTagName("section");

                    foreach (XmlElement elemParent1 in Doc_Medication_Node)
                    {
                        bool is_break = false;
                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "MEDICATIONS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "PROBLEMS"))
                            {
                                medItemDoc = elemParent;
                                break;
                            }
                        }
                        if (is_break == true)
                            break;
                    }

                }
                if (elemParent.InnerXml.ToUpper().Contains("<TITLE>REASON FOR REFERRAL<") == true)
                {
                    XmlNodeList Doc_ReferalReason_Node = xmldoc.GetElementsByTagName("section");
                    foreach (XmlElement elemParent1 in Doc_ReferalReason_Node)
                    {
                        // bool is_break = false;
                        bool is_Reason = false;
                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "REASON FOR REFERRAL")
                            {
                                is_Reason = true;
                            }
                            if (elemParent.ChildNodes[i].Name == "text" && is_Reason == true)
                            {

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
                            }
                        }
                    }
                }

                if (selectedItem == string.Empty)
                    ImportSection(dsSection, sTitle, medItemDoc, reason);
            }
            else if (sContent != string.Empty)
            {
                PdfPTable emptyPDF = new PdfPTable(1);
                emptyPDF.WidthPercentage = 100;
                HeadCell = new PdfPCell(new Phrase(sTitle, HeadFont));
                HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //HeadCell.HorizontalAlignment = 1;
                HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
                emptyPDF.AddCell(HeadCell);
                if (sContent == string.Empty)
                {
                    cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
                }
                else
                {
                    cell = CreateCell("", StripTagsRegex(sContent), "");
                }
                emptyPDF.AddCell(cell);
                doc.Add(emptyPDF);
                doc.Add(new Paragraph("   "));
                //doc.Add(new Paragraph("   "));
            }
            else
            {
                sTitle = string.Empty;
            }

            return sTitle;

            #endregion

        }

        public void ImportSection(DataSet dsSection, string sTitle, XmlElement elemParent, string reason)
        {
            switch (sTitle.ToUpper())
            {
                case "IMPLANTS":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                InHouseProcedure objInHouseProcedure = new InHouseProcedure();
                                objInHouseProcedure.GMDN_PT_Name = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Implanted"));
                                //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                                objInHouseProcedure.Device_Identifier_UDI = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("UDI"));
                                lstImplant.Add(objInHouseProcedure);
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["ImplantList"] = lstImplant;
                        break;
                    }
                case "MENTAL STATUS":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                CarePlan objCarePlan = new CarePlan();
                                objCarePlan.Care_Name_Value = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Status"));
                                //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                                objCarePlan.Plan_Date = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Date"));
                                lstMentalStatus.Add(objCarePlan);
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["MentalStatusList"] = lstMentalStatus;
                        break;
                    }
                case "GOALS SECTION":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                TreatmentPlan objTreatmentPlan = new TreatmentPlan();
                                objTreatmentPlan.Plan = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Goal"));
                                //objInHouseProcedure.Reaction = StripTagsRegex(dsImplant.Tables[0].Rows[i].Field<string>("Area"));
                                //objTreatmentPlan.Plan_Date = StripTagsRegex(dsGoalsSection.Tables[0].Rows[i].Field<string>("Date"));
                                lstGoalsSection.Add(objTreatmentPlan);
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["GoalsSectionList"] = lstGoalsSection;
                        break;
                    }
                case "HEALTH CONCERNS SECTION":
                    {
                        try
                        {
                            for (int k = 0; k < dsSection.Tables.Count; k++)
                            {
                                for (int i = 0; i < dsSection.Tables[k].Rows.Count; i++)
                                {
                                    ProblemList objProblemList = new ProblemList();
                                    //objProblemList.Problem_Description = StripTagsRegex(dsHealthConcernSection.Tables[k].Rows[i].Field<string>("<tr><th>Observations"));
                                    if (k == 0)
                                    {
                                        objProblemList.Problem_Description = StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Observations"));
                                    }
                                    else
                                    {
                                        objProblemList.Problem_Description = StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Concern"));
                                    }
                                    objProblemList.Status = "Active";
                                    string diagdate = StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("Date"));
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
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["HealthConcernList"] = lsthealthconcern;
                        break;
                    }
                case "INTERVENTIONS":
                case "INTERVENTIONS SECTION":
                    {
                        try
                        {
                            for (int k = 0; k < dsSection.Tables.Count; k++)
                            {
                                for (int i = 0; i < dsSection.Tables[k].Rows.Count; i++)
                                {
                                    PatientResults objVitals = new PatientResults();

                                    if ((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention"))).ToString() == "pulse oximetry monitoring")
                                        objVitals.Loinc_Observation = "Pulse Oximetry";
                                    else if (StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention")) == "Oxygen administration by nasal cannula")
                                        objVitals.Loinc_Observation = "Inhaled O2 Concentration";
                                    else if (StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention")) == "Elevate head of bed")
                                        objVitals.Loinc_Observation = "Respiratory Rate";
                                    else
                                        objVitals.Loinc_Observation = StripTagsRegex((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Planned Intervention"))).ToString());
                                    objVitals.Captured_date_and_time = Convert.ToDateTime((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("Date"))).ToString());
                                    objVitals.Created_Date_And_Time = universalTime;
                                    objVitals.Created_By = ClientSession.UserName;
                                    objVitals.Results_Type = "Vitals";

                                    lstvitals.Add(objVitals);

                                }
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["Vitals"] = lstvitals;
                        break;
                    }
                case "HEALTH STATUS EVALUATIONS/OUTCOMES SECTION":
                    {
                        try
                        {
                            for (int k = 0; k < dsSection.Tables.Count; k++)
                            {
                                for (int i = 0; i < dsSection.Tables[k].Rows.Count; i++)
                                {
                                    PatientResults objVitals = new PatientResults();

                                    if ((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Item"))).ToString() == "Pulse oximetry")
                                        objVitals.Loinc_Observation = "Pulse Oximetry";
                                    else if (StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Item")) == "Oxygen administration by nasal cannula")
                                        objVitals.Loinc_Observation = "Inhaled O2 Concentration";
                                    else if (StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Item")) == "Elvate head of bed")
                                        objVitals.Loinc_Observation = "Respiratory Rate";
                                    else
                                        objVitals.Loinc_Observation = StripTagsRegex((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("<tr><th>Item"))).ToString());

                                    objVitals.Value = StripTagsRegex((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("Outcome"))).ToString());
                                    objVitals.Captured_date_and_time = Convert.ToDateTime((StripTagsRegex(dsSection.Tables[k].Rows[i].Field<string>("Date"))).ToString());
                                    objVitals.Created_Date_And_Time = universalTime;
                                    objVitals.Created_By = ClientSession.UserName;
                                    objVitals.Results_Type = "Vitals";

                                    lstvitals.Add(objVitals);
                                }
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["Vitals"] = lstvitals;
                        break;
                    }
                case "ALLERGY":
                case "ALLERGIES, ADVERSE REACTIONS, ALERTS":
                case "ALLERGIES AND ADVERSE REACTIONS":
                case "ALLERGIES":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                Rcopia_Allergy objallergy = new Rcopia_Allergy();
                                objallergy.Allergy_Name = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Substance"));
                                objallergy.Reaction = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Reaction"));
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
                        Session["AllergyList"] = lstallergy;
                        break;
                    }
                case "IMMUNIZATION":
                case "IMMUNIZATIONS":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                Immunization objimmunization = new Immunization();
                                objimmunization.Immunization_Description = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Vaccine"));
                                objimmunization.Created_Date_And_Time = universalTime;
                                objimmunization.Created_By = ClientSession.UserName;
                                string date = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Date"));
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
                        Session["ImmunizationList"] = lstimmunization;
                        break;
                    }
                case "MEDICATIONS":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                Rcopia_Medication objmedication = new Rcopia_Medication();
                                objmedication.Generic_Name = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Medication"));
                                string[] split = objmedication.Generic_Name.Split('[');
                                if (split.Length > 1)
                                    objmedication.Brand_Name = split[1].Replace("]", "");
                                string date = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Start Date"));
                                if (date != "Unknown" && date != string.Empty && (!date.Contains("-") || !date.Contains("/")))
                                {
                                    string year = date.Substring(0, 4);
                                    string month = date.Substring(4, 2);
                                    string date_1 = date.Substring(6, 2);
                                    objmedication.Start_Date = Convert.ToDateTime(year + "-" + month + "-" + date_1);
                                }
                                if (StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS NEEDED" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS NEEDED FOR PAIN" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "WITH MEALS" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS DIRECTED" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "BETWEEN MEALS" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "ONE HOUR BEFORE MEALS" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "BEFORE EXERCISE" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "WITH A GLASS OF WATER" ||
                                    StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AFTER MEALS")
                                {

                                    objmedication.Dose_Other = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions"));
                                }
                                else
                                {
                                    objmedication.Dose_Timing = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Directions"));
                                }

                                //dosevalue
                                try
                                {
                                    objmedication.Dose = elemParent.GetElementsByTagName("doseQuantity")[i].Attributes.GetNamedItem("value").Value;
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
                        Session["MedicationList"] = lstmedication;
                        break;
                    }
                case "CARE PLAN":
                case "PLAN OF CARE":
                    {
                        try
                        {
                            string plan = string.Empty;
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    if (plan == string.Empty)
                                        plan = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Planned Activity")); // + " " + dsCarePlan.Tables[0].Rows[i].Field<string>("Planned Date"));
                                    else
                                        plan = StripTagsRegex(plan + "*" + dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Planned Activity")); //+ " " + dsCarePlan.Tables[0].Rows[i].Field<string>("Planned Date"));
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
                                    objTreatmentPlan.Local_Time = UtilityManager.ConvertToLocal(objTreatmentPlan.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                                    objTreatmentPlan.Created_By = ClientSession.UserName;
                                    lsttreatmentplan.Add(objTreatmentPlan);
                                }
                                catch
                                {
                                    //do nothing
                                }
                            }
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["TreatmentPlanList"] = lsttreatmentplan;
                        break;
                    }
                case "REASON FOR REFERRAL":
                    {
                        try
                        {
                            if (reason != string.Empty && dsSection == null)
                            {
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
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["ReferalOrderList"] = lstreferalorder;
                        break;
                    }
                case "ENCOUNTERS":
                case "ENCOUNTERS DIAGNOSIS":
                    {
                        try
                        {
                            //if (reason != string.Empty && dsSection == null)
                            //{
                            try
                            {
                                if (dsSection != null && dsSection.Tables.Count > 0)
                                {
                                    for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                                    {
                                        Encounter objencounter = new Encounter();
                                        objencounter.Facility_Name = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Location")); ;
                                        string pat_DOV = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Date"));
                                        string pat_ROV = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Encounter"));
                                        string year = pat_DOV.Substring(0, 4);
                                        string month = pat_DOV.Substring(4, 2);
                                        string date = pat_DOV.Substring(6, 2);

                                        if (pat_DOV.Length == 8)
                                        {
                                            hdnDateOfService.Value = year + "-" + month + "-" + date;
                                            objencounter.Date_of_Service = UtilityManager.ConvertToUniversal(Convert.ToDateTime(year + "-" + month + "-" + date));
                                            objencounter.Appointment_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(year + "-" + month + "-" + date));

                                        }

                                        else
                                        {
                                            string datestring = string.Empty;
                                            if (pat_DOV.Split(',')[1].Contains(' ') == true)
                                                datestring = pat_DOV.Split(',')[0].Split(' ')[1] + "-" + pat_DOV.Split(',')[0].Split(' ')[0] + pat_DOV.Split(',')[1].Split(' ')[1];
                                            else
                                                datestring = pat_DOV.Split(',')[1] + "-" + pat_DOV.Split(',')[0].Split(' ')[0] + "-" + pat_DOV.Split(',')[0].Split(' ')[1];

                                            objencounter.Date_of_Service = UtilityManager.ConvertToUniversal(Convert.ToDateTime(datestring));
                                            objencounter.Appointment_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(datestring));

                                            hdnDateOfService.Value = Convert.ToDateTime(datestring).ToString("yyyy-MM-dd");
                                        }

                                        objencounter.Local_Time = UtilityManager.ConvertToLocal(objencounter.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt"); ;
                                        objencounter.Purpose_of_Visit = pat_ROV;
                                        objencounter.Created_Date_and_Time = universalTime;
                                        objencounter.Created_By = ClientSession.UserName;
                                        lstencounter.Add(objencounter);
                                    }
                                }
                            }
                            catch
                            {
                                //do nothing
                            }

                            //}
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["Encounter"] = lstencounter;
                        break;
                    }
                case "PROCEDURES":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    InHouseProcedure objprocedure = new InHouseProcedure();
                                    objprocedure.Procedure_Code_Description = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Procedure"));
                                    objprocedure.Created_By = ClientSession.UserName;
                                    string date = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Date"));//added for 30254
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
                        catch
                        {
                            //do nothing
                        }
                        Session["ProcedureList"] = lstprocedures;
                        break;
                    }
                case "FUNCTIONAL STATUS":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    CarePlan objCarePlan = new CarePlan();
                                    objCarePlan.Care_Name = "Functional Condition";
                                    objCarePlan.Care_Name_Value = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Functional Condition"));
                                    objCarePlan.Status = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Condition Status"));
                                    string date = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Effective Dates"));//added for 30257
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
                        catch
                        {
                            //do nothing
                        }
                        Session["CarePlanList"] = lstcareplan;
                        break;
                    }
                case "RESULTS":
                    {
                        try
                        {
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

                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {

                                ResultOBX objresult = new ResultOBX();
                                string order = StripTagsRegex(dsSection.Tables[0].Rows[i][0].ToString());
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
                                        else
                                        {
                                            objresult.OBX_Reference_Range = orders[1];
                                        }
                                        order = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Blood chemistry"));
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
                        catch
                        {
                            //do nothing
                        }
                        Session["ResultMasterList"] = lstresultmaster;
                        Session["ResultObrList"] = lstresultobr;
                        Session["ResultObxList"] = lstresultobx;
                        Session["ResultOrcList"] = lstresultorc;
                        break;
                    }
                case "SOCIAL HISTORY":
                    {
                        try
                        {
                            for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    SocialHistory objhistory = new SocialHistory();
                                    objhistory.Social_Info = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("<tr><th>Social History Element"));
                                    objhistory.Description = StripTagsRegex(dsSection.Tables[0].Rows[i].Field<string>("Description"));
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
                        catch
                        {
                            //do nothing
                        }
                        Session["SocialHistoryList"] = lstsocilahistory;
                        break;
                    }
                case "VITAL SIGNS":
                    {
                        try
                        {
                            IList<string> Column_Header = new List<string>();
                            IList<string> Row_Header = new List<string>();
                            IList<string> Row_Value = new List<string>();

                            if (elemParent != null)
                            {
                                foreach (XmlElement elem_Parent in elemParent)
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
                        }
                        catch
                        {
                            //do nothing
                        }
                        Session["Vitals"] = lstvitals;
                        break;
                    }

                case "PROBLEMS":
                    {
                        if (elemParent != null)
                        {
                            foreach (XmlElement elemParent1 in elemParent)
                            {
                                bool is_break = false;
                                for (int i = 0; i < elemParent1.ChildNodes.Count; i++)
                                {
                                    if (elemParent1.ChildNodes[i].Name == "title" && elemParent1.ChildNodes[i].InnerText.ToUpper() == "PROBLEMS")
                                    {

                                        if (elemParent1.ChildNodes[i + 1].ChildNodes[1] != null)
                                        {
                                            int list_count = elemParent1.ChildNodes[i + 1].ChildNodes.Count;
                                            int iCount = 0;
                                            for (int j = 0; j < list_count; j++)
                                            {
                                                try
                                                {
                                                    if (elemParent1.ChildNodes[i + 1].ChildNodes[j].Name == "list")
                                                    {
                                                        for (int x = 0; x < elemParent1.ChildNodes[i + 1].ChildNodes[j].ChildNodes.Count; x++)
                                                        {
                                                            string columnvalue = elemParent1.ChildNodes[i + 1].ChildNodes[j].ChildNodes[x].InnerText;

                                                            iCount = iCount + 1;

                                                            try
                                                            {
                                                                Assessment objassesment = new Assessment();
                                                                reason = StripTagsRegex(columnvalue);
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
                                                        string columnvalue = elemParent1.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].InnerXml;

                                                        try
                                                        {
                                                            Assessment objassesment = new Assessment();
                                                            reason = StripTagsRegex(columnvalue);
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
                                            is_break = true;
                                            break;
                                        }
                                    }
                                }
                                if (is_break == true)
                                    break;
                            }
                        }
                        Session["AssesmentList"] = lstassesment;
                        break;
                    }
            }
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
                            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            HeadCell.HorizontalAlignment = 1;
                            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                                HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                                HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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

        public void PrintProblems(Document doc, XmlDocument xmldoc, string FileName, string Type)
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
                            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                                HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = MyColor; // BaseColor.DARK_GRAY;
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
                                PdfPTable emptyPDF = new PdfPTable(1);
                                emptyPDF.WidthPercentage = 100;
                                HeadCell = new PdfPCell(new Phrase("PROBLEM LIST", HeadFont));
                                HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                HeadCell.HorizontalAlignment = 1;
                                HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
                                emptyPDF.AddCell(HeadCell);
                                cell = CreateCell("", "     " + StripTagsRegex("No known data found"), "");
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


            XmlNodeList Doc_Node = xmldoc.GetElementsByTagName("title");
            string sTitle = Doc_Node[0].InnerXml;

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
                            if (elemParent.ChildNodes[i].Attributes["use"].Value == "MC")
                            {
                                pat_Cellphone = elemParent.ChildNodes[i].Attributes["value"].Value;
                            }
                            else if (elemParent.ChildNodes[i].Attributes["use"].Value == "HP")
                            {
                                if (pat_Telephone == string.Empty)
                                    pat_Telephone = elemParent.ChildNodes[i].Attributes["value"].Value;
                                else
                                    pat_Telephone = pat_Telephone + ", " + elemParent.ChildNodes[i].Attributes["value"].Value;
                            }
                            else if (elemParent.ChildNodes[i].Attributes["use"].Value == "WP")
                            {
                                pat_Workphone = elemParent.ChildNodes[i].Attributes["value"].Value;
                            }
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
                            if (elemParent.ChildNodes[i].ChildNodes[j].Name == "name")
                            {
                                //if (elemParent.ChildNodes[i].ChildNodes[j].Attributes.Count > 0)
                                //{
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
                                    //pat_Name = lastName + ", " + firstname + " " + middlename + " " + suffix;
                                    pat_Name = firstname + ", " + lastName + " " + middlename + " " + suffix;
                                    //}

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

            //var FontColour = new BaseColor(5, 128, 170);
            var FontColour = MyColor;
            Paragraph para = new Paragraph();
            para.Font = iTextSharp.text.FontFactory.GetFont("Calibri", 16, iTextSharp.text.Font.BOLD, MyColor);
            para.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            para.Add(sTitle);
            doc.Add(para);
            doc.Add(new Paragraph("   "));

            PdfPTable patPatient = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patPatient.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("Patient Detail", HeadFont));
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.MinimumHeight = 18;
            HeadCell.Colspan = 8;
            //HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = MyColor; ; // BaseColor.DARK_GRAY;
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
            //doc.Add(new Paragraph("   "));
            //doc.Add(new Paragraph("   "));

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
                objhuman.Legal_Org = ClientSession.LegalOrg;
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

            PdfPTable patAuthor = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patAuthor.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("Author", HeadFont));
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.MinimumHeight = 18;
            HeadCell.Colspan = 4;
            //HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                                            {
                                                if (firstname == string.Empty)
                                                {
                                                    firstname = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                                }
                                                else
                                                {
                                                    firstname = firstname + " " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                                }
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "family")
                                            {
                                                lastName = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "prefix")
                                            {
                                                Prefix = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].Name == "suffix")
                                            {
                                                lastName = lastName + ", " + elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[x].InnerXml;
                                            }
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
                                if (street != string.Empty)
                                    pat_AuthorAddres = street;
                                if (city != string.Empty)
                                    pat_AuthorAddres = pat_AuthorAddres + ", " + city;
                                if (state != string.Empty)
                                    pat_AuthorAddres = pat_AuthorAddres + ", " + state;
                                if (Zip != string.Empty)
                                    pat_AuthorAddres = pat_AuthorAddres + ", " + Zip;

                                //pat_AuthorAddres = street + ", " + city + ", " + state + ", " + Zip;
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
            if (pat_AuthorName == string.Empty)
            {
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
                                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[y].ChildNodes[x].Name == "given" && FirstName == "")
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
                cell = CreateCell("", "", "");
                patAuthor.AddCell(cell);
                cell = CreateCell("", "", "");
                patAuthor.AddCell(cell);

                doc.Add(patAuthor);
                //doc.Add(new Paragraph("   "));
                //doc.Add(new Paragraph("   "));

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

            PdfPTable patCustodian = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patCustodian.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("Custodian", HeadFont));
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.MinimumHeight = 18;
            HeadCell.Colspan = 4;
            //HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[0].Name.ToUpper() == "VALUE")
                                                pat_CustodianTelephone = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[0].Value;
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[1].Name.ToUpper() == "VALUE")
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
            HeadCell = new PdfPCell(new Phrase("Provider", HeadFont));
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.MinimumHeight = 18;
            HeadCell.Colspan = 2;
            //HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = MyColor; //BaseColor.DARK_GRAY;
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
                                                            if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].ChildNodes[m].ChildNodes[n].Name == "given" && firstname == "")
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
                //doc.Add(new Paragraph("   "));
                //doc.Add(new Paragraph("   "));
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

            PdfPTable patDocument = new PdfPTable(new float[] { 900, 900, 900, 900 });
            patDocument.WidthPercentage = 100;
            HeadCell = new PdfPCell(new Phrase("Referring Provider", HeadFont));
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.MinimumHeight = 18;
            HeadCell.Colspan = 4;
            //HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                                            //if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].Attributes[3].Value == "Primary Care Provider")
                                            //{
                                            bCheckProvider = true;
                                            //}
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

                                                    if (street != string.Empty)
                                                        pat_DocumentAddress = street;
                                                    if (city != string.Empty)
                                                        pat_DocumentAddress = pat_DocumentAddress + ", " + city;
                                                    if (state != string.Empty)
                                                        pat_DocumentAddress = pat_DocumentAddress + ", " + state;
                                                    if (zip != string.Empty)
                                                        pat_DocumentAddress = pat_DocumentAddress + ", " + zip;
                                                    //pat_DocumentAddress = street + ", " + city + ", " + state + ", " + zip;
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
                                                    else if (FirstName != string.Empty)
                                                        CareTeamMembers = ProviderName + "\n" + FirstName;
                                                    else if (LastName != string.Empty)
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
                cell = CreateCell("Referring Provider Name", "", "");
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
                //doc.Add(new Paragraph("   "));
                //doc.Add(new Paragraph("   "));
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
            HeadCell = new PdfPCell(new Phrase("Documentation Detail", HeadFont));
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.MinimumHeight = 18;
            HeadCell.Colspan = 2;
            //HeadCell.HorizontalAlignment = 1;
            HeadCell.BackgroundColor = MyColor; //BaseColor.DARK_GRAY;
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
                                                if (street != string.Empty)
                                                    pat_DocumentAddress = street;
                                                if (city != string.Empty)
                                                    pat_DocumentAddress = pat_DocumentAddress + ", " + city;
                                                if (state != string.Empty)
                                                    pat_DocumentAddress = pat_DocumentAddress + ", " + state;
                                                if (zip != string.Empty)
                                                    pat_DocumentAddress = pat_DocumentAddress + ", " + zip;
                                                //pat_DocumentAddress = street + ", " + city + ", " + state + ", " + zip;
                                            }
                                            else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Name == "telecom")
                                            {
                                                try
                                                {
                                                    if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[0].Name.ToUpper() == "VALUE")
                                                        pat_DocumentTel = elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[0].Value;
                                                    else if (elemParent.ChildNodes[i].ChildNodes[j].ChildNodes[k].ChildNodes[l].Attributes[1].Name.ToUpper() == "VALUE")
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

            //if (pat_DocumentAddress != string.Empty || pat_DocumentTel != string.Empty)
            //{
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
            //doc.Add(new Paragraph("   "));
            //}

            #endregion
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
                //bool is_break = false;
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
                //bool is_break = false;
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
                //bool is_break = false;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                // bool is_break = false;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                    //bool is_author = false;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                    HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                    HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
                    HeadCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    HeadCell.BackgroundColor = MyColor;// BaseColor.DARK_GRAY;
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
            IList<ActivityLog> checkActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            checkActivityLogList = ActivitylogMngr.GetActivityByActivityTypeandSubject("CCD Import", Session["sFileNameBind"].ToString());

            if (checkActivityLogList.Count > 0)
            {
                Session["SummaryCareAlreadyImported"] = true;
                return;
            }
            else
            {
                Session["SummaryCareAlreadyImported"] = false;
            }

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

            Session["sReturn"] = objencounter.SaveSummaryOfCare(objsummarydto, ClientSession.UserName, ClientSession.FacilityName, ClientSession.EncounterId,ClientSession.LegalOrg);
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLog activity = new ActivityLog();
            activity.Activity_Type = "CCD Import";
            activity.Activity_Date_And_Time = DateTime.UtcNow;
            activity.Encounter_ID = (objsummarydto.EncounterList != null && objsummarydto.EncounterList.Count > 0 && objsummarydto.EncounterList[0].Id != 0) ? objsummarydto.EncounterList[0].Id : ClientSession.EncounterId;
            activity.Human_ID = (objsummarydto.HumanList != null && objsummarydto.HumanList.Count > 0 && objsummarydto.HumanList[0].Id != 0) ? objsummarydto.HumanList[0].Id : ClientSession.HumanId;
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

        [System.Web.Services.WebMethod(EnableSession = true)]
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
                    retVal = objsumm.GenerateEntireSummary();
                else
                    retVal = objsumm.GeneratePDFforSummary(SelectedSummaryItems);
            }
            return retVal;
        }

        public string GeneratePDFforSummary(IList<string> SelectedSummaryItems)
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
            sPrintPathName = sFolderPathName + "\\SummaryOfCare_" + path + ".pdf";
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
            //float X = 20f, Y = 20f;
            PrintPatient(doc, xmldoc);

            XmlNodeList Doc_Section_Node = xmldoc.GetElementsByTagName("section");
            System.Web.UI.WebControls.ListItem chkbx = new System.Web.UI.WebControls.ListItem();
            string sTitle = string.Empty;
            for (int i = 0; i < SelectedSummaryItems.Count; i++)
            {
                foreach (XmlElement elemParent in Doc_Section_Node)
                {
                    sTitle = PrintSection(doc, elemParent, SelectedSummaryItems[i]);

                    //if (sTitle != string.Empty)
                    //{
                    //    chkbx = new System.Web.UI.WebControls.ListItem();
                    //    chkbx.Text = sTitle;
                    //    if (SummaryCheckList != null)
                    //        SummaryCheckList.Items.Add(chkbx);
                    //}
                }
            }
            //for (int i = 0; i < SelectedSummaryItems.Count; i++)
            //{

            //    switch (SelectedSummaryItems[i])
            //    {
            //        case "Allergy":
            //            {
            //                PrintAllergy(doc, xmldoc);
            //            } break;
            //        case "Immunization": PrintImmunization(doc, xmldoc); break;
            //        case "Medication": PrintMedication(doc, xmldoc); break;
            //        case "Care Plan": PrintCarePlan(doc, xmldoc); break;
            //        case "Reason for Referral": PrintReasonForReferal(doc, xmldoc); break;
            //        case "Procedures": PrintProcedures(doc, xmldoc); break;
            //        case "Results": PrintResults(doc, xmldoc, FileName, Type); break;
            //        case "Social History": PrintSocialHistory(doc, xmldoc); break;
            //        case "Vitals": PrintVitals(doc, xmldoc); break;
            //        case "Instructions": PrintInstructions(doc, xmldoc); break;
            //        case "Chief Complaint": PrintChiefComplaint(doc, xmldoc); break;
            //        case "Problems": PrintProblems(doc, xmldoc, FileName, Type); break;
            //        case "Provider": PrintProvider(doc, xmldoc); break;
            //        case "Custodian": PrintCustodian(doc, xmldoc); break;
            //        case "Documentation Details": PrintDocumentDetail(doc, xmldoc); break;
            //        case "Visit Details": PrintVisitDetail(doc, xmldoc); break;
            //        case "Author": PrintAuthor(doc, xmldoc); break;
            //        case "Medication Administered": PrintMedicationAdministered(doc, xmldoc); break;
            //        case "Functional Status": PrintFunctionalStatus(doc, xmldoc); break;
            //        case "Hospital Discharge Instructions": PrintHospitalInstructions(doc, xmldoc); break;
            //        case "Implant": PrintImplant(doc, xmldoc); break;
            //        case "Mental Status": PrintMentalStatus(doc, xmldoc); break;
            //        case "Goals Section": PrintGoalsSection(doc, xmldoc); break;
            //        case "Health Concern": PrintHealthConcernSection(doc, xmldoc); break;
            //        case "Treatment Plan": PrintTreatmentPlan(doc, xmldoc); break;
            //        case "Laboratory Information": PrintLaboratoryInformation(doc, xmldoc); break;
            //        case "Interventions": PrintInterventions(doc, xmldoc); break;
            //        case "Health Status Evaluations/Outcomes": PrintHealthStatusEvaluations(doc, xmldoc); break;
            //        case "Reason for Visit": PrintReasonForVisit(doc, xmldoc); break;
            //        case "Hospital Course": PrintHospitalCourse(doc, xmldoc); break;
            //        case "Advance Directives": PrintAdvanceDirectives(doc, xmldoc); break;
            //        case "Family History": PrintFamilyHistory(doc, xmldoc); break;
            //        case "Chief Complaint And Reason For Visit": PrintChiefComplaintAndReasonForVisit(doc, xmldoc); break;
            //        case "History Of Present Illness": PrintHistoryOfPresentIllness(doc, xmldoc); break;
            //        case "Encounters": PrintEncounter(doc, xmldoc); break;
            //        default: break;
            //    }

            //}
            doc.Close();
            xmltext.Close();
            //string retVal = "frmSummaryOfCare.aspx?FileName=" + FileName.ToString() + "&pdf=" + (string)HttpContext.Current.Session["PDF_Path"] + "&Type=" + Type.ToString() + "&UniversalTime=" + UniverTime;
            //return retVal;
            Uri CurrentURL = new Uri(Request.Url.ToString());
            PDFLOAD.Attributes.Add("src", CurrentURL.Scheme + Uri.SchemeDelimiter + Request.Url.Authority + "//Documents//" + Session.SessionID + "//" + System.Configuration.ConfigurationSettings.AppSettings["SummaryCarePathName"] + "//" + DateTime.Now.ToString("yyyyMMdd") + "//" + "SummaryOfCare_" + path + ".pdf");
            return string.Empty;
        }

        public string GenerateEntireSummary()
        {
            System.IO.File.Delete((string)Session["PDF_Path"]);
            string retVal = string.Empty;
            if (FileName != null && (Type == "CCD" || Type == "C32"))
            {

                path = PrintPDF(FilePath, "CCD", DateTime.MinValue, FileName, Type);
                retVal = "frmSummaryOfCare.aspx?FileName=" + FileName.ToString() + "&pdf=" + (string)HttpContext.Current.Session["PDF_Path"] + "&Type=" + Type.ToString() + "&UniversalTime=" + UniverTime + "\\" + path + ".pdf";

            }
            return retVal;
        }

        protected void SummaryCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isChkboxSelected = SummaryCheckList.Items.OfType<System.Web.UI.WebControls.ListItem>().Where(a => a.Selected == true);
             if (isChkboxSelected.Count() > 0 )
             {
                 IList<string> sChkTextList = new List<string>();
                 foreach (var item in isChkboxSelected)
                 {
                     sChkTextList.Add(item.Text);
                     
                 }
                 //for (int i = 0; i < sChkTextList.Count; i++)
                 //{
                 //    if (sChkTextList.Contains("All") == true && sChkTextList[i].ToString() != "All")
                 //    {
                 //        SummaryCheckList.Items.FindByText(sChkTextList[i].ToString()).Selected = false;
                 //        sChkTextList.RemoveAt(i);
                 //    }
                 //}


                 if (sChkTextList.Count == 1 && sChkTextList[0] == "All")
                     GenerateEntireSummary();
                 else
                    GeneratePDFforSummary(sChkTextList);
             }
             else
             {
                 SummaryCheckList.Items.FindByText("All").Selected = true;
                 GenerateEntireSummary();
             }
        }
    }
}
