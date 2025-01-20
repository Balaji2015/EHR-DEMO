/*********************************************************************************************************************************
 *  Form Name : frmIndexing | Module Name : Scanning And Indexing | User Role : Medical Assistants , Front-Office , Physician
**********************************************************************************************************************************/
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DTOJson;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.UI;
using Acurus.Capella.UI.Extensions;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
//using Telerik.Web;
//using Telerik.Web.UI;
//using Telerik.Web.UI.Upload;



namespace Acurus.Capella.UI
{
    public partial class frmIndexing : System.Web.UI.Page
    {

        #region "Declaration & Instantiation"
        Scan_IndexManager scanIndesmanager = new Scan_IndexManager();
        IList<scan_index> scanIndexList = null;
        IList<Orders> lstorders = new List<Orders>();
        IList<int> selectedPageNumbers = new List<int>();
        IList<int> TotalPagesSelected = new List<int>();
        IList<string> lstDocuments = new List<string>();
        IList<Scan> lstScanList = new List<Scan>();
        DateTime selectedDate = DateTime.MinValue;
        ulong scan_ID = 0;
        ulong human_id = 0;

        StringBuilder file_name = new StringBuilder();
        StringBuilder filePath = new StringBuilder();
        StringBuilder sFacilityName = new StringBuilder();

        #endregion

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Attributes.Add("onclick", "return btnSave_Clicked();");
            //if (!RadScriptManager1.IsInAsyncPostBack)
            //{
            if (!IsPostBack)
            {
                ClientSession.HumanId = 0;
                //hdnScreenMode.Value = "Bulk Scanning and Fax";
                //ClientSession.FacilityName = "160 E. ARTESIA STREET, STE #360";
                //ClientSession.UserName = "JPRASAD";
                //ClientSession.UserRole = "Physician";
                //hdnLocalTime.Value = "480";
                //hdnLocalDate.Value = "2/21/2020";
                //hdnUniversaloffset.Value = "-05.00";
                //hdnLocalDateAndTime.Value = "10:29:00 PM";
                ////human_id = Convert.ToUInt64("532567");
                //scan_ID = Convert.ToUInt64("102097");

                if (Request.QueryString["FileName"] != null && Request.QueryString["FileName"].Trim() != "")
                {
                    file_name = new StringBuilder();
                    file_name.Append(Request.QueryString["FileName"].ToString().Replace("HASHSYMBOL", "#"));
                    Session["FileName"] = file_name.ToString();
                }
                if (Request.QueryString["HumanId"] != null && Request.QueryString["HumanId"].Trim() != "")
                {
                    human_id = Convert.ToUInt64(Request.QueryString["HumanId"]);
                    hdnHumanID.Value = human_id.ToString();
                }

                if (Request.QueryString["UserName"] != null && Request.QueryString["UserName"].Trim() != "")
                {
                    ClientSession.UserName = Convert.ToString(Request.QueryString["UserName"]);
                }

                if (Request.QueryString["FacilityName"] != null && Request.QueryString["FacilityName"].Trim() != "")
                {
                    ClientSession.FacilityName = Convert.ToString(Request.QueryString["FacilityName"]);
                }

                if (Request.QueryString["ScreenMode"] != null && Request.QueryString["ScreenMode"].Trim() != "")
                {
                    hdnScreenMode.Value = Request.QueryString["ScreenMode"];
                }

                if (Request.QueryString["ScanId"] != null && Request.QueryString["ScanId"].Trim() != "")
                {
                    scan_ID = Convert.ToUInt32(Request.QueryString["ScanId"]);
                }
                if (Request.QueryString["UserRole"] != null && Request.QueryString["UserRole"].Trim() != "")
                {
                    ClientSession.UserRole = Request.QueryString["UserRole"];
                }
                //if (Request.QueryString["LocalOffSetTime"] != null && Request.QueryString["LocalOffSetTime"].Trim() != "")
                //{
                //    hdnLocalTime.Value = Request.QueryString["LocalOffSetTime"];
                //}
                //if (Request.QueryString["LocalDate"] != null && Request.QueryString["LocalDate"].Trim() != "")
                //{
                //    hdnLocalDate.Value = Request.QueryString["LocalDate"];
                //}
                //if (Request.QueryString["UniversalTime"] != null && Request.QueryString["UniversalTime"].Trim() != "")
                //{
                //    hdnUniversaloffset.Value = Request.QueryString["UniversalTime"];
                //}
                //if (Request.QueryString["LocalTime"] != null && Request.QueryString["LocalTime"].Trim() != "")
                //{
                //    hdnLocalDateAndTime.Value = Request.QueryString["LocalTime"];
                //}
                if (Request.QueryString["CurrentZone"] != null && Request.QueryString["CurrentZone"].Trim() != "")
                {
                    //string Offset = Request.QueryString["CurrentZone"];
                    StringBuilder Offset = new StringBuilder();
                    Offset.Append(Request.QueryString["CurrentZone"]);
                    if (Offset.ToString().StartsWith("-"))
                    {
                        DateTime localTime = DateTime.Now.ToUniversalTime().Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0));
                        dtpDocumentDate.Value = localTime.ToString("dd-MMM-yyyy");
                        dtpScannedDate.Value = localTime.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        DateTime localTime = DateTime.Now.ToUniversalTime().AddMinutes(Convert.ToDouble(Offset.ToString()));
                        dtpDocumentDate.Value = localTime.ToString("dd-MMM-yyyy");
                        dtpScannedDate.Value = localTime.ToString("dd-MMM-yyyy");
                    }
                }
                else
                {
                    dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                //ClientSession.LocalOffSetTime = hdnLocalTime.Value;
                //ClientSession.LocalDate = hdnLocalDate.Value;
                //ClientSession.UniversalTime = hdnUniversaloffset.Value;
                //ClientSession.LocalTime = hdnLocalDateAndTime.Value;
                Session["BrowseLoadList"] = null;
                Session["LoadList"] = null;
                Session["IndexList"] = null;

                hdnIsMyScan.Value = "";
                hdnScanningLocal.Value = ConfigurationManager.AppSettings["ScanningPath_Local"];
                hdnPDFurl.Value = ConfigurationManager.AppSettings["ScanningPathUrl"];

                ddlSourceType.Items.Add("SCAN");
                ddlSourceType.Items.Add("FAX");

                LoadFacilityList(ddlSourceType.Text);

                PageLabel.InnerText = "/0";
                PageBox.Value = "0";

                rdbRemoteDir.Checked = true;
                rdbAll.Checked = true;
                txtSelectedPages.Disabled = true;

                /* My Scan Queue */
                if (scan_ID != 0)
                {
                    //string simagePathname = string.Empty;
                    hdnIsMyScan.Value = "true";
                    btnSave.Enabled = false;
                    SelectDir.Attributes.Add("disabled", "disabled");

                    Scan scannedFileInfo = new Scan();
                    Scan_IndexDTO LoadList = null;

                    LoadList = scanIndesmanager.GetScannedListByScanID(scan_ID, 0);
                    LoadDocType();

                    scannedFileInfo = LoadList.ScanDetails;
                    scanIndexList = LoadList.ScanIndexList;

                    if (scanIndexList.Count > 0)
                        ClientSession.HumanId = scanIndexList[0].Human_ID;
                    Session["FileName"] = scannedFileInfo.Scanned_File_Name;

                    //string _fileName = scannedFileInfo.Scanned_File_Name;
                    hdnfilepath.Value = Path.GetDirectoryName(scannedFileInfo.Scanned_File_Path);
                    hdnFileName.Value = Path.GetFileName(scannedFileInfo.Scanned_File_Path);
                    //simagePathname = file_name = scannedFileInfo.Scanned_File_Path;
                    file_name = new StringBuilder();
                    file_name.Append(scannedFileInfo.Scanned_File_Path);
                    if (scannedFileInfo.Scan_Type != null && scannedFileInfo.Scan_Type.Contains('-'))
                    {
                        if (scannedFileInfo.Scan_Type.Split('-')[1].Replace(" ", "") != "LOCAL")
                            ddlSourceType.SelectedValue = scannedFileInfo.Scan_Type.Split('-')[1].Replace(" ", "");
                        else
                            rdbLocalDir.Checked = true;

                    }

                    hdnPagecount.Value = scannedFileInfo.No_of_Pages.ToString();


                    //if (simagePathname != string.Empty)
                    //{
                    //    /* Open File */
                    //    try
                    //    {
                    //        IList<Scan> lstBrowselist = new List<Scan>();
                    //        lstBrowselist.Add(scannedFileInfo);
                    //        Session["BrowseLoadList"] = lstBrowselist;
                    //       // LoadFiles();
                    //       // OpenFile(simagePathname, Session["Page_Count"].ToString());
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileAccessingException", "alert('Scanned File Not available in the location, it may be deleted or corrupted.Please Contact Support..'); setTimeout(function () { self.close(); });", true);
                    //    }

                    //    hdnPageBox.Value = "1";
                    //    PageBox.Value = "1";
                    //}

                    if (scanIndexList != null && scanIndexList.Count > 0)
                    {
                    retry:
                        try
                        {
                            setThePatientDetails(scanIndexList[0].Human_ID);
                        }
                        catch (Exception ex)
                        {
                            // ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.HumanId.ToString() + "','Human','indexing');", true);

                            UtilityManager.GenerateXML(scanIndexList[0].Human_ID.ToString(), "Human");
                            //return;
                            goto retry;
                        }
                        dtpDocumentDate.Value = UtilityManager.ConvertToLocal(scanIndexList[0].Document_Date).ToString("dd-MMM-yyyy");//.ToString("d-MMM-yyyy");
                        //For Bug Id: 68850
                        // dtpScannedDate.Value = UtilityManager.ConvertToLocal(scannedFileInfo.Scanned_Date).ToString("dd-MMM-yyyy");// DateTime.Now.ToString("dd-MMM-yyyy");////.ToString("d-MMM-yyyy");
                        dtpScannedDate.Value = scannedFileInfo.Scanned_Date.ToString("dd-MMM-yyyy");
                        LoadGridView(scanIndexList);
                        Session.Add("IndexList", scanIndexList);
                        btnMoveToNextProcess.Disabled = false;
                    }
                    else
                    {
                        btnMoveToNextProcess.Disabled = true;
                        dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");//.ToString("d-MMM-yyyy");//BUGID:44213
                        dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");//.ToString("d-MMM-yyyy");
                        Session.Remove("IndexList");
                        grdIndexing.DataSource = new string[] { };
                        grdIndexing.DataBind();
                    }

                    //Session["SaveFileName"] = simagePathname;
                    //if (Path.GetFileName(simagePathname).Split('.')[1].ToUpper() == "PDF")
                    if (Path.GetFileName(file_name.ToString()).Split('.')[1].ToUpper() == "PDF")
                    {
                        PDFholder.Style.Add("display", "block");
                        bigImagePDF.Style.Add("display", "block");
                        imgholder.Style.Add("display", "none");
                        // _plcImgsThumbs.Style.Add("display", "none");
                    }
                    else
                    {
                        PDFholder.Style.Add("display", "none");
                        bigImagePDF.Style.Add("display", "none");
                        imgholder.Style.Add("display", "block");
                        // _plcImgsThumbs.Style.Add("display", "block");
                    }
                }
                //online Documents//
                else
                {
                    SelectDir.Attributes.Remove("disabled");
                    dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");//.ToString("d-MMM-yyyy"); //BUGID:44213
                    dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");//.ToString("d-MMM-yyyy");
                    btnSave.Enabled = false;
                    LoadDocType();
                    // LoadFiles();

                    //string path = (string)Session["FilePath"];
                    //string full_path = string.Empty;
                    sFacilityName = new StringBuilder();
                    if (ddSelectedFacility.Text.Contains("~"))
                        sFacilityName.Append(ddSelectedFacility.Text.Split('~')[1].Trim());
                    else
                        sFacilityName.Append(ddSelectedFacility.Text);
                    // sFacilityName = ddSelectedFacility.Text.Replace(" ", "_").Replace("#", "").Replace(",", "");

                    if (file_name != null && file_name.Length != 0)
                    {
                        //if (!file_name.Contains("Indexing_Files"))
                        //{
                        //    if (file_name.Contains(ConfigurationManager.AppSettings["FaxpathoutputIndexing"]))
                        //        full_path = file_name;
                        //    else
                        //        full_path = path + "\\" + sFacilityName + "\\Scanned_Images\\" + Path.GetFileName(Path.GetDirectoryName(file_name)) + "\\" + Path.GetFileName(file_name);
                        //    // full_path = path + "\\" + ddSelectedFacility.Text + "\\Scanned_Images\\" + Path.GetFileName(Path.GetDirectoryName(file_name)) + "\\" + Path.GetFileName(file_name);
                        //}
                        //else
                        //{
                        //    full_path = "\\atala-capture-upload\\Indexing_Files\\" + Path.GetFileName(file_name);
                        //}
                        //full_path = file_name;
                        if (file_name.ToString().Split('.')[1].ToUpper() == "PDF")
                        {
                            PDFholder.Style.Add("display", "block");
                            bigImagePDF.Style.Add("display", "block");
                            imgholder.Style.Add("display", "none");
                            // _plcImgsThumbs.Style.Add("display", "none");
                        }
                        else
                        {
                            PDFholder.Style.Add("display", "none");
                            imgholder.Style.Add("display", "block");
                            //  _plcImgsThumbs.Style.Add("display", "block");
                        }
                    }
                    //sScan_Type = "Online Chart";
                    //scan_ID = ulScanID;
                    /* Open File */
                    //if (full_path != string.Empty)
                    //    OpenFile(full_path, Session["Page_Count"].ToString());
                    //else
                    //{
                    //    hdnPageBox.Value = "0";
                    //    PageBox.Value = "1";
                    //}
                    /* Populating human ID, from opened from patient demographics  */
                    //if (human_id != 0)
                    //{
                    //    setThePatientDetails(human_id);
                    //}

                    /* To restrict page box page Selection */
                    //cboFromPage.Attributes["max"] = ((int)Session["Page_Count"]).ToString();
                    //cboToPage.Attributes["max"] = ((int)Session["Page_Count"]).ToString();


                    //hdnPageBox.Value = "0";
                    grdIndexing.DataSource = new string[] { };
                    grdIndexing.DataBind();
                    // btnMoveToNextProcess.Disabled = true;


                }
                Session["ScanId"] = scan_ID;
                //Session["ScanType"] = sScan_Type;
                HideOrders();
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    hdnIsAncillary.Value = ilstFacAncillary[0].Fac_Name;
                }

                //if (grdIndexing.Items.Count == 0)
                //{
                //    btnFindPatient.Disabled = false;
                //}
                //else
                //{
                //    btnFindPatient.Disabled = true;
                //}

                //if (Session["Page_Count"] != null)// && Convert.ToString((int)Session["Page_Count"]) != string.Empty)
                //{
                //    PageLabel.InnerText = "/ " + Session["Page_Count"].ToString();//Convert.ToString((int)Session["Page_Count"]);
                //    PageBox.Value = "1";

                //}
                //else
                //    PageBox.Value = "0";

            }
            //PageBox.Value = "1";

            //}



            if (!IsPostBack)
            {
                cboStandingOrders.DataSource = null;
                cboStandingOrders.DataBind();
                upPatientDetails.Update();
            }

            if (hdnUpdateMode.Value == "Reset" && hdnIsEditgrid.Value == "")
            {
                btnSave.Text = "Add";
                btnClearAll.Text = "Reset";
                hdnUpdateMode.Value = string.Empty;
                //CAP-969
                btnMoveToNextProcess.Disabled = false;
            }
            if (PatientDetails.Text != string.Empty)
            {

                PatientDetails.Attributes.Remove("class");
                PatientDetails.Attributes.Add("class", "patientPaneDisabled"); PatientDetails.CssClass = "patientPaneDisabled";
            }
            else
            {
                PatientDetails.Attributes.Remove("class");
                PatientDetails.Attributes.Add("class", "patientPaneEnabled");
            }
            //upPatientDetails.Update();
            if (file_name == null || file_name.Length == 0)
            {
                PDFholder.Style.Add("display", "none");
                bigImagePDF.Style.Add("display", "none");
                imgholder.Style.Add("display", "block");
                //  _plcImgsThumbs.Style.Add("display", "block");
            }

            if (human_id != 0)
            {
                //hdnIsMyScan.Value = "true";
                PatientDetails.Text = HumanDetails(human_id.ToString());
                //PatientDetails.CssClass = "patientPaneDisabled";
                PatientDetails.Attributes.Remove("class");
                PatientDetails.Attributes.Add("class", "patientPaneDisabled"); PatientDetails.CssClass = "patientPaneDisabled";
                upPatientDetails.Update();
                btnFindPatient.Disabled = true;
                btnMoveToNextProcess.Disabled = false;


            }


            if (hdnHumanID.Value == "" && PatientDetails.Text != "")
            {
                if (PatientDetails.Text.Contains(':'))
                    hdnHumanID.Value = PatientDetails.Text.Split(':')[1].Split('|')[0];
            }

            if (hdnHumanID.Value != "")
            {
                ulong ulongValue = 0;
                bool isNumber = ulong.TryParse(hdnHumanID.Value, out ulongValue);
                if (isNumber == true)
                {
                    ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
                }
            }
            if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER" && hdnHumanID.Value.Trim() != "")
            {
                VisibleOrders();
            }
            else
            {
                HideOrders();
            }
            if (cboDocumentType.SelectedValue.ToUpper() == "ENCOUNTERS")
            {
                dtpDocumentDate.Disabled = true;
                lblDocDate.Attributes.Remove("class");
                lblDocDate.Attributes.Add("class", "spanstyle");
                DocDateStar.Visible = false;

                ddEncPhyName.Enabled = true;
                lblDosPhy.Attributes.Remove("class");
                lblDosPhy.Attributes.Add("class", "MandLabelstyle");
                DosPhyStar.Visible = true;
                EncMsg.Visible = true;

            }
            else
            {
                dtpDocumentDate.Disabled = false;
                lblDocDate.Attributes.Remove("class");
                lblDocDate.Attributes.Add("class", "MandLabelstyle");
                DocDateStar.Visible = true;

                ddEncPhyName.Enabled = false;
                lblDosPhy.Attributes.Remove("class");
                lblDosPhy.Attributes.Add("class", "spanstyle");
                DosPhyStar.Visible = false;
                EncMsg.Visible = false;
                if (ddEncPhyName.Items != null && ddEncPhyName.Items.Count > 0)
                {
                    if (ddEncPhyName.SelectedIndex > -1)
                    {
                        ddEncPhyName.ClearSelection();
                    }
                    ddEncPhyName.SelectedIndex = 0;
                    ddEncPhyName.Items.Clear();
                }
            }
            if (rdbRemoteDir.Checked == true)
            {
                if (ddlSourceType.Text.ToUpper() == "FAX")
                {
                    btnMovetoNonMedicalFolder.Visible = false;// hide button as per git lab id 1099
                    //string sfilePath = ConfigurationManager.AppSettings["ScanningPath_Fax"] + ConfigurationManager.AppSettings["ftpFaxpath"];
                    StringBuilder sfilePath = new StringBuilder();
                    sfilePath.Append(ConfigurationManager.AppSettings["ScanningPath_Fax"] + ConfigurationManager.AppSettings["ftpFaxpath"]);
                    //string sFolderStructure = string.Empty;
                    //if (ddSelectedFacility.Text.Contains("~"))
                    //{
                    //    sFolderStructure = ddSelectedFacility.Text.Split('~')[0].Trim() + ConfigurationManager.AppSettings["FaxpathoutputIndexing"];
                    //}
                    //else
                    //{
                    //    sFolderStructure = ddSelectedFacility.Text.Replace(" ", "_").Replace("#", "").Replace(",", "") + ConfigurationManager.AppSettings["FaxpathoutputIndexing"];
                    //}

                    //string sPath = Path.Combine(sfilePath, sFolderStructure);
                    //sPath = sPath.Replace(@"/", @"\");
                    //hdnScanningLocal.Value = sPath.Substring(0, sPath.LastIndexOf('\\'));
                    hdnScanningLocal.Value = sfilePath.ToString().Replace(@"/", @"\").Replace(@"\\\\", @"\\").Replace(@"\\\", @"\\");
                    hdnPDFurl.Value = ConfigurationManager.AppSettings["FAXScanningPathUrl"];
                }
                else
                {
                    hdnScanningLocal.Value = ConfigurationManager.AppSettings["ScanningPath_Local"];
                    hdnPDFurl.Value = ConfigurationManager.AppSettings["ScanningPathUrl"];
                    btnMovetoNonMedicalFolder.Visible = false;
                }
            }


            //if (rdbRemoteDir.Checked == true)
            //{
            //    dtpScannedDate.Disabled = false;
            //    chckShowOldFiles.Enabled = true;
            //    ddlSourceType.Enabled = true;
            //    btnFindDocuments.Disabled = true;
            //    fileupload.Enabled = false;
            //    ddSelectedFacility.Enabled = true;
            //}
            //else
            //{
            //    dtpScannedDate.Disabled = true;
            //    chckShowOldFiles.Enabled = false;
            //    ddlSourceType.Enabled = false;
            //    btnFindDocuments.Disabled = false;
            //    fileupload.Enabled = true;
            //    ddSelectedFacility.Enabled = false;
            //}

        }

        protected void cboDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdnIsEditgrid.Value != "")
            {
                btnSave.Text = "Update";
                btnClearAll.Text = "Cancel";
                hdnUpdateMode.Value = string.Empty;
            }
            //Cap - 1141
            else
            {
                btnSave.Text = "Add";
                btnClearAll.Text = "Reset";
            }
            if (btnSave.Text == "Update")
            {
                rdbAll.Checked = false;
                rdbPageRange.Checked = true;
            }
            if (rdbAll.Checked == true)
            {
                btnSave.Enabled = false;
                txtSelectedPages.Value = "";
                txtSelectedPages.Disabled = true;
                btnMoveToNextProcess.Disabled = false;
            }
            else if (rdbPageRange.Checked == true)
            {
                btnSave.Enabled = true;
                txtSelectedPages.Disabled = false;
                //CAP-969
                if (grdIndexing.Items.Count > 0)
                { btnMoveToNextProcess.Disabled = false; }
                else
                { btnMoveToNextProcess.Disabled = true; }
            }

            if (hdnHumanID.Value == "" && PatientDetails.Text != "")
            {
                if (PatientDetails.Text.Contains(':'))
                    hdnHumanID.Value = PatientDetails.Text.Split(':')[1].Split('|')[0];
            }
            if (hdnHumanID.Value != "")
            {
                ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
            }
            //if (Session["FileName"] != null)
            //{
            //    if (Path.GetExtension(Session["FileName"].ToString().ToUpper()) == ".PDF")
            //    {
            //        //btnCurrentPage.Visible = false;
            //    }
            //}
            //upIndexingDetails.Update();
            if (cboDocumentType.SelectedValue.ToUpper() == "ENCOUNTERS")
            {
                LoadEncPhysician();
            }
            else
            {
                dtpDocumentDate.Disabled = false;
                lblDocDate.Attributes.Remove("class");
                lblDocDate.Attributes.Add("class", "MandLabelstyle");
                DocDateStar.Visible = true;

                ddEncPhyName.Enabled = false;
                lblDosPhy.Attributes.Remove("class");
                lblDosPhy.Attributes.Add("class", "spanstyle");
                DosPhyStar.Visible = false;
                EncMsg.Visible = false;
                if (ddEncPhyName.Items != null && ddEncPhyName.Items.Count > 0)
                {
                    ddEncPhyName.Items.Clear();
                }
            }
            if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER" && hdnHumanID.Value == "")
            //if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER" && PatientDetails.CssClass == "patientPaneEnabled")
            {
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ordersValidation", "alert('Please Choose Patient to load outstanding orders');", true);
                
                //Jira CAP-1212 - In ErrorMessage.Xml also removed the "first" word.
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ordersValidation", "StopLoadOnUploadFile();alert('Please Select Patient first, to get outstanding orders');", true);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ordersValidation", "StopLoadOnUploadFile();alert('Please Select the Patient to get outstanding orders');", true);
                if (cboDocumentType.Items != null && cboDocumentType.Items.Count > 0)
                {
                    if (cboDocumentType.SelectedIndex > -1)
                    {
                        cboDocumentType.ClearSelection();
                    }

                    cboDocumentType.SelectedIndex = 0;
                }
                cboDocumentSubType.Items.Clear();
                divLoading.Style.Add("display", "none");
                waitCursor.Update();
                upIndexingDetails.Update();
                return;
            }
            else
            {
                cboStandingOrders.Items.Clear();
                cboLab.Items.Clear();
                if (cboPhysician.Items != null && cboPhysician.Items.Count > 0)
                {
                    if (cboPhysician.SelectedIndex > -1)
                    {
                        cboPhysician.ClearSelection();
                    }
                }
                //for git id 600
                //if (rdbRemoteDir.Checked == true&&ddSelectedFacility.Text.ToUpper()==ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                //     chkShowAll.Checked = true;
                //else if (ClientSession.FacilityName.ToUpper() == ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                //   chkShowAll.Checked = true;
                //else
                //    chkShowAll.Checked = false;




                if (cboOrderPhysician.Items != null && cboOrderPhysician.Items.Count > 0)
                {
                    if (cboOrderPhysician.SelectedIndex > -1)
                    {
                        cboOrderPhysician.ClearSelection();
                    }
                }
                chkOrderingPhyShowAll.Checked = false;

            }
            if (cboDocumentType.Items != null && cboDocumentType.Items.Count > 0)
            {
                if (cboDocumentType.SelectedIndex > 0)
                {
                    cboDocumentSubType.Enabled = true;
                    //btnSave.Enabled = true;
                }
            }

            LoadDocumentSubType(cboDocumentType.SelectedItem.Text.ToUpper());



            #region "Diagnostic Orders"
            if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER")
            {
                spanoutstandorder.Attributes.Remove("class");
                spanoutstandorder.Attributes.Add("class", "MandLabelstyle");
                spanorderstar.Visible = true;

                spanphy.Attributes.Remove("class");
                spanphy.Attributes.Add("class", "MandLabelstyle");
                spanphystar.Visible = true;

                #region "Loading Outstanding Orders"
                // string Cpt_Data = HttpUtility.HtmlDecode("~");
                //is_found = false;

                VisibleOrders();
                cboStandingOrders.Items.Clear();
                cboLab.Items.Clear();
                cboPhysician.Enabled = true;
                cboStandingOrders.Enabled = true;
                cboLab.Enabled = true;
                chkShowAll.Enabled = true;
                chkOrderingPhyShowAll.Enabled = true;
                cboOrderPhysician.Enabled = true;
                //if (btnSave.Text == "Add")
                //{
                //    is_found = false;
                //}


                // string tempItem = cboDocumentType.SelectedItem.Value;
                lstorders = new List<Orders>();
                IList<Orders> lstorderstemp = new List<Orders>();
                OrdersManager objorders = new OrdersManager();
                lstorderstemp = objorders.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId(cboDocumentType.SelectedItem.Value, "ORDER_GENERATE", ClientSession.HumanId);
                IList<string> ilstOrdersubmit = new List<string>();
                //Cap - 1118
                IList<scan_index> lstscan = new List<scan_index>();
                Scan_IndexManager objmanager = new Scan_IndexManager();
                lstscan = objmanager.GetScannedObjectByHumanID(ClientSession.HumanId);

                //string sFacilityCmg = ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper();
                //string sFacilityAncillary = ConfigurationManager.AppSettings["AncillaryLab"].Trim().ToUpper();
                IList<string> lstordersCMG = new List<string>();
                //var IscheckCMGAncillary = lstorderstemp.Where(aa => aa.Order_Code_Type.ToUpper().Trim() == ConfigurationManager.AppSettings["AncillaryLab"].Trim().ToUpper()).ToList();
                var lab = from l in ApplicationObject.facilityLibraryList where l.Fac_Name == ClientSession.FacilityName select l;
                IList<FacilityLibrary> facLabList = lab.ToList<FacilityLibrary>();
                var IscheckCMGAncillary = lstorderstemp.Where(aa => facLabList.Any(bb => bb.Short_Name.ToUpper() == aa.Order_Code_Type.ToString().ToUpper())).ToList();

                IDictionary<string, string> ilstOrdersubmitParentorder = new Dictionary<string, string>();
                if (IscheckCMGAncillary.Count > 0)
                {

                    //XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));
                    //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                    //   .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() != "DME" && a.Attribute("name").Value.ToString() == facLabList[0].Short_Name)
                    //   .OrderBy(s => (int)s.Attribute("sort_order"));
                    //string xmlValue = string.Empty;                    
                    //if (xml != null)
                    //{
                    //    foreach (XElement LabElement in xml)
                    //    {
                    //        xmlValue = LabElement.Attribute("id").Value;
                    //    }
                    //}

                    //CAP-2773
                    Lablist objlablist = new Lablist();
                    objlablist = ConfigureBase<Lablist>.ReadJson("LabList.json");
                    List<Labs> listLabList = new List<Labs>();
                    listLabList = objlablist.Lab.Where(a => a.type != "DME" && a.name == facLabList[0].Short_Name)
                        .OrderBy(s => (int)Convert.ToInt32(s.sort_order)).ToList();
                    string xmlValue = string.Empty;
                    if (listLabList != null)
                    {
                        foreach (Labs objlab in listLabList)
                        {
                            xmlValue = objlab.id;
                        }
                    }

                    if (xmlValue != string.Empty)
                    {
                        ilstOrdersubmit = objorders.GetOrderByHuman(ClientSession.HumanId, ClientSession.FacilityName, Convert.ToUInt64(xmlValue));//, ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper());
                        lstordersCMG = objorders.GetOrdersforCMGAncillary(cboDocumentType.SelectedItem.Value, "ORDER_GENERATE", ClientSession.HumanId, Convert.ToUInt64(xmlValue));//, ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper());
                        ilstOrdersubmitParentorder = objorders.GetOrdersforCMGAncillaryParentOrders(lstordersCMG);
                    }
                }


                List<string> svalue = new List<string>();
                if (ilstOrdersubmit.Count > 0)
                {
                    var svalues = (from o in ilstOrdersubmit select o.Split('|')[0].Trim()).ToList();
                    svalue = svalues.Where(aa => !lstordersCMG.Any(oo => oo.ToString() == aa.ToString().Trim())).ToList();
                }

                if (facLabList.Count > 0 && facLabList[0].Is_Ancillary == "Y")
                {
                    //if (svalue.Count > 0 && lstordersCMG.Count>0)
                    //{
                    //    lstorders = lstorderstemp.Where(item => !svalue.Any(i => i.ToString().Trim() == item.Order_Submit_ID.ToString())).ToList<Orders>();
                    //}
                    lstorders = lstorderstemp.Where(item => lstordersCMG.Any(cmglst => cmglst.ToString() == item.Order_Submit_ID.ToString())).ToList();
                }
                else
                {
                    lstorders = lstorderstemp.Where(item => !svalue.Any(i => i.ToString().Trim() == item.Order_Submit_ID.ToString())).ToList<Orders>();
                }

                Session["OrdersList"] = lstorders;

                #endregion

                #region "Loading Lab List"

                LoadLabCombo();

                #endregion


                //if (sFacilityCmg.ToUpper() == ClientSession.FacilityName.ToUpper())
                //{
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    ShowAllphysicianforCMGAncillary();
                    ShowAllOrderingphysicianforCMGAncillary();
                    chkShowAll.Checked = true;
                    chkOrderingPhyShowAll.Checked = true;

                }
                else
                {
                    #region "Loading Physician List"

                    LoadPhysicianCombo();

                    #endregion

                    #region "Loading Ordering Physician List"

                    LoadOrderingPhysicianCombo();

                    #endregion

                    chkShowAll.Checked = false;
                    chkOrderingPhyShowAll.Checked = false;
                }



                #region "Populating The Orders"
                if (lstorders.Count > 0 && lstorders[0].Id != 0)
                {
                    cboStandingOrders.Items.Clear();

                    int submittedOrder = lstorders.GroupBy(a => a.Order_Submit_ID).Count();
                    ListItem orderItems = null;

                    //Remove the paper order from the outstanding orders
                    var temp = lstorders.Where(a => a.Lab_Procedure != "Paper Order").Select(a => a.Order_Submit_ID).Distinct();
                    if (temp != null)
                    {
                        var facilityAncillary = from f in ApplicationObject.facilityLibraryList where f.Is_Ancillary == "Y" select f;
                        IList<FacilityLibrary> ilstAncillaryFac = facilityAncillary.ToList<FacilityLibrary>();

                        foreach (ulong submittedorders in temp)
                        {

                            orderItems = new ListItem();
                            //string OrderText = "";
                            StringBuilder OrderText = new StringBuilder();
                            Boolean bIsSkip = false;
                            foreach (Orders item in lstorders.Where(a => a.Order_Submit_ID == submittedorders))
                            {
                                bIsSkip = false;
                                var AncLab = (from l in ilstAncillaryFac where l.Short_Name.ToUpper() == item.Order_Code_Type.ToUpper() select l).ToList();
                                //Cap - 1118
                                var scanorder = (from m in lstscan where m.Order_ID == submittedorders select m).ToList();
                                if (AncLab.Count > 0)
                                {
                                    if (item.Encounter_ID == 0)
                                    {
                                        bIsSkip = true;
                                        continue;
                                    }
                                    else
                                    {
                                        EncounterManager encMngr = new EncounterManager();
                                        IList<Encounter> encList = encMngr.GetEncounterByEncounterIDIncludeArchive(item.Encounter_ID);
                                        if (encList.Count > 0)
                                        {
                                            if (encList[0].Order_Submit_ID == 0)
                                            {
                                                bIsSkip = true;
                                                continue;
                                            }
                                        }
                                    }
                                }
                                //Cap - 1118
                                else if (scanorder.Count > 0)
                                {
                                    bIsSkip = true;
                                    continue;

                                }

                                if (item.Lab_Procedure != "" && item.Lab_Procedure_Description != "")
                                {
                                    //OrderText += (item.Lab_Procedure + " - " + item.Lab_Procedure_Description + Cpt_Data);
                                    OrderText.Append(item.Lab_Procedure + " - " + item.Lab_Procedure_Description + HttpUtility.HtmlDecode("~"));

                                }
                                else
                                {
                                    OrderText.Append("");

                                }

                            }
                            DateTime OrderDateAndTime = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Created_Date_And_Time;
                            DateTime SpecCollectionDate = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Internal_Property_Spec_Collection_Date;
                            ulong LabID = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Internal_Property_LabID;
                            ulong PhyID = 0;
                            //For Bug Id : 70899
                            //if (ilstOrdersubmitParentorder.Count > 0)
                            //{
                            //    var svaluephyID = ilstOrdersubmitParentorder.Where(aa => aa.Key == submittedorders.ToString()).Select(aa => aa.Value).FirstOrDefault(); ;
                            //    if (svaluephyID != null && svaluephyID != string.Empty)
                            //        PhyID = Convert.ToUInt32(svaluephyID);
                            //}
                            //else
                            //    PhyID = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Physician_ID;


                            if (lstorders.Count > 0)
                                PhyID = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Physician_ID;

                            if (PhyID == 0 && ilstOrdersubmitParentorder.Count > 0)
                            {
                                var svaluephyID = ilstOrdersubmitParentorder.Where(aa => aa.Key == submittedorders.ToString()).Select(aa => aa.Value).FirstOrDefault(); ;
                                if (svaluephyID != null && svaluephyID != string.Empty)
                                    PhyID = Convert.ToUInt32(svaluephyID);
                            }


                            if (PhyID == 0)
                            {
                                //string sPhysicainID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
                                if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] != null)
                                    PhyID = Convert.ToUInt32(ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]);
                            }

                            //OrderText += "|[ #" + submittedorders.ToString() + " ]|";

                            OrderText.Append("|[ #" + submittedorders.ToString() + " ]|");
                            String dtSpecCollectionDateConverted = string.Empty;

                            if (Request.QueryString["CurrentZone"] != null)
                            {
                                //string Offset = Request.QueryString["CurrentZone"];
                                StringBuilder Offset = new StringBuilder();
                                Offset.Append(Request.QueryString["CurrentZone"]);
                                if (Offset.ToString().StartsWith("-"))
                                {
                                    DateTime localTime = OrderDateAndTime.AddMinutes(Convert.ToDouble(Offset.ToString()));
                                    // OrderText += localTime.ToString("dd-MMM-yyyy hh:mm tt");
                                    OrderText.Append(localTime.ToString("dd-MMM-yyyy hh:mm tt"));
                                    if (SpecCollectionDate != DateTime.MinValue)
                                    { dtSpecCollectionDateConverted = SpecCollectionDate.AddMinutes(Convert.ToDouble(Offset.ToString())).ToString("dd-MMM-yyyy hh:mm tt"); }
                                    //else

                                    //{ dtSpecCollectionDateConverted = DateTime.UtcNow.AddMinutes(Convert.ToDouble(Offset)).ToString("d-MMM-yyyy hh:mm tt"); }


                                }
                                else
                                {
                                    DateTime localTime = OrderDateAndTime.Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0));
                                    //OrderText += localTime.ToString("dd-MMM-yyyy hh:mm tt");
                                    OrderText.Append(localTime.ToString("dd-MMM-yyyy hh:mm tt"));
                                    if (SpecCollectionDate != DateTime.MinValue)
                                    {
                                        dtSpecCollectionDateConverted = SpecCollectionDate.Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0)).ToString("dd-MMM-yyyy hh:mm tt");
                                    }
                                    //else

                                    //{ dtSpecCollectionDateConverted = DateTime.UtcNow.Subtract(new TimeSpan(0, Convert.ToInt32(Offset), 0)).ToString("d-MMM-yyyy hh:mm tt"); }

                                }
                            }

                            if (bIsSkip == false)
                            {
                                orderItems.Text = OrderText.ToString();
                                //string orderValue = PhyID.ToString() + "|" + LabID.ToString() + "|" + submittedorders.ToString();
                                StringBuilder orderValue = new StringBuilder();
                                orderValue.Append(PhyID.ToString() + "|" + LabID.ToString() + "|" + submittedorders.ToString());
                                if (dtSpecCollectionDateConverted != string.Empty)
                                {
                                    //orderValue += "|" + dtSpecCollectionDateConverted;
                                    orderValue.Append("|" + dtSpecCollectionDateConverted);
                                }
                                orderItems.Value = orderValue.ToString();
                                orderItems.Attributes.Add("title", OrderText.ToString());
                                cboStandingOrders.Items.Add(orderItems);
                            }

                        }
                    }

                    ListItem chooseOrder = new ListItem();
                    chooseOrder.Text = "Paper Order";
                    chooseOrder.Value = "Paper Order";
                    cboStandingOrders.Items.Add(chooseOrder);

                    //ListItem makeNewOrder = new ListItem();
                    //makeNewOrder.Text = "Create New Order";
                    //makeNewOrder.Attributes.CssStyle.Add("background-color", "#bfdbff");
                    //makeNewOrder.Attributes.CssStyle.Add("cursor", "pointer");
                    //makeNewOrder.Value = "";
                    //cboStandingOrders.Items.Insert(1, makeNewOrder);
                    if (cboStandingOrders.Items != null && cboStandingOrders.Items.Count > 0)
                    {
                        if (cboStandingOrders.SelectedIndex > -1)
                        {
                            cboStandingOrders.ClearSelection();
                        }
                        cboStandingOrders.SelectedIndex = 0;
                    }
                    if (cboStandingOrders.SelectedItem.Text != "Paper Order" && cboStandingOrders.SelectedItem.Text.Contains('|'))
                    {
                        //Ref:bug Id 61172 ,61261 ,61260
                        //Some order_sibmit "Physician_id" not in Physicianfacilitymappping but they are in User table .So v check this condition to avoid crashing
                        if (cboPhysician.Items.FindByValue(cboStandingOrders.SelectedItem.Value.Split('|')[0]) != null)
                            cboPhysician.SelectedValue = cboStandingOrders.SelectedItem.Value.Split('|')[0];//100 | 2;
                        else
                        {
                            //string sPhysicainID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
                            if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] != string.Empty && cboPhysician.Items.FindByValue(ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]) != null)
                                cboPhysician.SelectedValue = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];

                        }
                        if (cboOrderPhysician.Items.FindByValue(cboStandingOrders.SelectedItem.Value.Split('|')[0]) != null)
                            cboOrderPhysician.SelectedValue = cboStandingOrders.SelectedItem.Value.Split('|')[0];

                        if (cboLab.Items.FindByValue(cboStandingOrders.SelectedItem.Value.Split('|')[1]) != null)
                            cboLab.SelectedValue = cboStandingOrders.SelectedItem.Value.Split('|')[1];
                    }
                }
                else
                {

                    //ListItem tempcomboItem = new ListItem();
                    //tempcomboItem.Text = "No Order(s) Found";
                    //tempcomboItem.Value = "No Order(s) Found";
                    //cboStandingOrders.Items.Insert(0, tempcomboItem);

                    ListItem chooseOrder = new ListItem();
                    chooseOrder.Text = "Paper Order";
                    chooseOrder.Value = "Paper Order";
                    cboStandingOrders.Items.Add(chooseOrder);
                    if (cboStandingOrders.Items != null && cboStandingOrders.Items.Count > 0)
                    {
                        if (cboStandingOrders.SelectedIndex > -1)
                        {
                            cboStandingOrders.ClearSelection();
                        }
                        cboStandingOrders.SelectedIndex = 0;
                    }

                    //ListItem makeNewOrder = new ListItem();
                    //makeNewOrder.Text = "Create New Order";
                    //makeNewOrder.Attributes.CssStyle.Add("background-color", "#bfdbff");
                    //makeNewOrder.Attributes.CssStyle.Add("cursor", "pointer");
                    //makeNewOrder.Value = "";
                    //cboStandingOrders.Items.Insert(1, makeNewOrder);
                    //cboStandingOrders.SelectedIndex = 0;
                    //cboStandingOrders.Items.Insert(0, makeNewOrder);

                }
                Session["OrdersList"] = lstorders;
                #endregion


                //if (cboStandingOrders.SelectedItem.Value.Trim() == string.Empty)
                if (cboStandingOrders.SelectedItem.Value.Trim() == "Paper Order")
                {
                    // cboPhysician.Enabled = true;
                    if (cboPhysician.Items != null && cboPhysician.Items.Count > 0)
                    {
                        if (cboPhysician.SelectedIndex > -1)
                        {
                            cboPhysician.ClearSelection();
                        }
                    }

                    //For Bug ID :60363 Enhancement 
                    // string sPhysicainID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
                    //if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] != string.Empty && cboPhysician.Items != null && cboPhysician.Items.Count > 0 && cboPhysician.Items.FindByValue(ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]) != null)
                    //{
                    //    cboPhysician.SelectedValue = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
                    //    // bug id 62687
                    //    cboPhysician.Enabled = true;
                    //}
                    cboPhysician.Enabled = true;
                    if (cboLab.Items != null && cboLab.Items.Count > 0)
                    {
                        if (cboLab.SelectedIndex > -1)
                        {
                            cboLab.ClearSelection();
                        }
                    }
                    if (cboStandingOrders.Items != null && cboStandingOrders.Items.Count > 0)
                    {
                        if (cboStandingOrders.SelectedIndex > -1)
                        {
                            cboStandingOrders.ClearSelection();
                        }
                    }
                    cboLab.Enabled = true;

                    Labspan.Attributes.Remove("class");
                    Labspan.Attributes.Add("class", "spanstyle");
                    //slabMandatory.Attributes.Remove("class");
                    slabMandatory.Visible = false;
                    chkShowAll.Enabled = true;
                    chkOrderingPhyShowAll.Enabled = true;
                    btnSearchCpt.Disabled = false;
                    dtpSpecCollection.Disabled = false;
                    cboIs_Interperated.Enabled = true;

                    cboOrderPhysician.Enabled = true;
                    spnOrderPhy.Attributes.Remove("class");
                    spnOrderPhy.Attributes.Add("class", "MandLabelstyle");
                    spnOrderPhyStar.Visible = true;
                    if (cboOrderPhysician.Items != null && cboOrderPhysician.Items.Count > 0)
                    {
                        if (cboOrderPhysician.SelectedIndex > -1)
                        {
                            cboOrderPhysician.ClearSelection();
                        }
                    }
                }
                else
                {
                    if (cboPhysician.Items != null && cboPhysician.Items.Count > 0)
                    {
                        if (cboPhysician.SelectedIndex > -1)
                        {
                            cboPhysician.ClearSelection();
                        }
                    }
                    cboPhysician.Enabled = true;
                    cboOrderPhysician.Enabled = false;
                    cboLab.Enabled = false;
                    Labspan.Attributes.Remove("class");
                    Labspan.Attributes.Add("class", "MandLabelstyle");
                    // slabMandatory.Attributes.Add("class", "manredforstar");
                    slabMandatory.Visible = true;
                    chkShowAll.Enabled = true;
                    //chkShowAll.Checked = false;
                    chkOrderingPhyShowAll.Enabled = false;
                    btnSearchCpt.Disabled = true;
                    dtpSpecCollection.Disabled = true;
                    cboIs_Interperated.Enabled = true;

                    cboOrderPhysician.Enabled = false;
                    spnOrderPhy.Attributes.Remove("class");
                    spnOrderPhy.Attributes.Add("class", "MandLabelstyle");
                    spnOrderPhyStar.Visible = true;
                }
                updateOrders.Update();
            }
            else
            {
                //Cap - 1121
                chkReviewandSign.Checked = false;
                HideOrders();

            }

            #endregion
            divLoading.Style.Add("display", "none");
            waitCursor.Update();
            upIndexingDetails.Update();
            chkExternalMedicalRecord.Checked = false;
            //if (btnSave.Text == "Update")
            //{
            //    btnSave.Text = "Update";
            //}


        }

        protected void grdIndexing_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {

                //string scan_index_id = e.Item.Cells[8].Text;
                IList<scan_index> insertList = new List<scan_index>();
                Scan_IndexDTO AddedListOfScanIndex = null;
                IList<scan_index> index_lst = (IList<scan_index>)Session["IndexList"];
                IList<scan_index> temp_lst = (from doc in index_lst
                                              where doc.Id == Convert.ToUInt64(e.Item.Cells[8].Text)
                                              select doc).ToList<scan_index>();

                if (temp_lst.Count > 0)
                    scan_ID = temp_lst[0].Scan_ID;
                if (hdnHumanID.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HUmanID", "StopLoadOnUploadFile();DisplayErrorMessage('390006');", true);
                    return;
                }

                AddedListOfScanIndex = scanIndesmanager.SaveUpdateDeleteScanIndex(insertList.ToArray<scan_index>(), new List<scan_index>(), temp_lst.ToArray<scan_index>(), Convert.ToUInt64(hdnHumanID.Value), scan_ID, string.Empty);
                if (temp_lst.Count > 0)
                    index_lst.Remove(temp_lst[0]);
                Session["IndexList"] = index_lst;
                btnSave.Enabled = false;
                btnSave.Text = "Add";
                btnClearAll.Text = "Reset";
                rdbPageRange.Checked = false;
                txtSelectedPages.Disabled = true;
                rdbAll.Checked = true;
                btnMoveToNextProcess.Disabled = false;

                if (index_lst != null && index_lst.Count > 0)
                {
                    LoadGridView(index_lst);
                }
                else
                {
                    grdIndexing.DataSource = new string[] { };
                    grdIndexing.DataBind();
                    btnFindPatient.Disabled = false;
                    //btnMoveToNextProcess.Disabled = true;
                    upPatientDetails.Update();
                }
                updateGrid.Update();
                waitCursor.Update();
                //Cap - 1139
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Deletegrid", "if(document.getElementById('grdIndexing').rows.length <= 1){localStorage.setItem('IsSaveClickedSucessfull','');} ", true);
            }

            else if (e.CommandName == "EditRow")
            {
                // string scan_index_id = e.Item.Cells[8].Text;

                hdnIsEditgrid.Value = "true";
                EditGrid(Convert.ToUInt64(e.Item.Cells[8].Text));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (rdbAll.Checked == true)
            //{
            //    btnSave.Enabled = false;
            //    txtSelectedPages.Value = "";
            //    txtSelectedPages.Disabled = true;
            //    btnMoveToNextProcess.Disabled = false;
            //}
            //else if (rdbPageRange.Checked == true)
            //{
            //    btnSave.Enabled = true;
            //    txtSelectedPages.Disabled = false;
            //    btnMoveToNextProcess.Disabled = true;
            //}

            if (hdnIsEditgrid.Value == "true")
            {
                btnSave.Text = "Update";

            }
            else
            {
                btnSave.Text = "Add";
            }
            if (hdnHumanID.Value == "" && PatientDetails.Text != "")
            {
                if (PatientDetails.Text.Contains(':'))
                    hdnHumanID.Value = PatientDetails.Text.Split(':')[1].Split('|')[0];
            }
            if (hdnHumanID.Value == "")// || PatientDetails.Text == "")
            {
                divLoading.Style.Add("display", "none");
                //CAP-1295
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HUmanID", "document.getElementById('divLoading').style.display = 'none'; StopLoadOnUploadFile();DisplayErrorMessage('390006');", true);
                return;
            }
            file_name = new StringBuilder();
            file_name.Append(Path.GetFileName(hdnsourceFile.Value));
            Session["FileName"] = file_name.ToString();
            //if (file_name == "")
            //{
            //    divLoading.Style.Add("display", "none");
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileChoose", "StopLoadOnUploadFile();DisplayErrorMessage('114017');", true);
            //    return;
            //}
            //if (Path.GetFileNameWithoutExtension(file_name).ToString().Contains((Path.GetExtension(file_name))) == true)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Fileextension", "StopLoadOnUploadFile();alert('The selected file can not be indexed as the file type extension (Example .pdf) is repeated twice.  Please rescan the file with proper naming convention.');", true);
            //    return;
            //}
            //if (Path.GetExtension(file_name).ToString().ToUpper() == ".PDF" && Path.GetFileNameWithoutExtension(file_name).ToString().Contains("#") == true)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Fileextension", "StopLoadOnUploadFile();alert('The selected file can not be indexed as the file has the special character such as #.Please rename the file and retry again.');", true);
            //    return;
            //}
            //if (cboDocumentType.SelectedIndex == 0)
            //{
            //    divLoading.Style.Add("display", "none");
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DocType", "StopLoadOnUploadFile();DisplayErrorMessage('115043');", true);
            //    return;
            //}
            //if (cboDocumentSubType.SelectedIndex == 0)
            //{
            //    divLoading.Style.Add("display", "none");
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Subdoctype", "StopLoadOnUploadFile();DisplayErrorMessage('115044');", true);
            //    return;
            //}
            if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER")
            {

                if (cboOrderPhysician.SelectedIndex == 0)
                {
                    divLoading.Style.Add("display", "none");
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileChoose", "document.getElementById('divLoading').style.display = 'none'; StopLoadOnUploadFile();DisplayErrorMessage('115065');", true);
                    return;
                }
                if (cboPhysician.SelectedIndex == 0)
                {
                    divLoading.Style.Add("display", "none");
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileChoose", "document.getElementById('divLoading').style.display = 'none'; StopLoadOnUploadFile();DisplayErrorMessage('114018');", true);
                    return;
                }
            }

            ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
            //Session["BoolDateFlag"] = dateFlag;
            //Session["BoolInvalidPage"] = is_Invalid_Page;
            ulong local_order_id = 0;
            bool tag_item = true;
            //bool sWebportal = false;
            //if (Session["Is_Web_Portal"] != null)
            //    sWebportal = (bool)Session["Is_Web_Portal"];



            if (rdbAll.Checked == true && hdnPagecount.Value != null)
                txtSelectedPages.Value = "1-" + hdnPagecount.Value.ToString();
            if (txtSelectedPages.Value.Replace(",", "").Trim() == "" || txtSelectedPages.Value.Replace(",", "").Trim() == "0")//BugID:54356
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SelectedPages", "displaySelectedPagesAlert();", true);
                return;
            }
            sFacilityName = new StringBuilder();
            if (rdbRemoteDir.Checked == true)
            {
                if (ddSelectedFacility.Text.Contains("~"))
                    sFacilityName.Append(ddSelectedFacility.Text.Split('~')[1].Trim());
                else
                    sFacilityName.Append(ddSelectedFacility.Text);
            }
            else
            {
                sFacilityName.Append(ClientSession.FacilityName);
            }

            //if (txtSelectedPages.Value.Contains(",") && txtSelectedPages.Value.Contains("-"))
            //{

            //    foreach (string  i in txtSelectedPages.Value.Split(','))
            //    {
            //        int ii = Convert.ToInt32(i);
            //        if(ii>Convert.ToInt64(Session["Page_Count"].ToString()))
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SelectedPages", "StopLoadOnUploadFile();DisplayErrorMessage('114019');", true);
            //            return;
            //        }

            //    }

            //}
            //if (Convert.ToInt64(txtSelectedPages.Value.Trim()) > Convert.ToInt64(Session["Page_Count"].ToString()))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SelectedPages", "StopLoadOnUploadFile();DisplayErrorMessage('114019');", true);
            //    return;
            //}
            //if (Convert.ToInt64(txtSelectedPages.Value.Trim()) > Convert.ToInt64(Session["Page_Count"].ToString()))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SelectedPages", "StopLoadOnUploadFile();DisplayErrorMessage('114019');", true);
            //    return;
            //}

            //if (sWebportal == true)
            //{
            //    if (cboDocumentType.SelectedValue.Trim().ToUpper() == "LEGAL DOCUMENTS")
            //    {
            //        if ((cboDocumentSubType.SelectedValue.Trim().ToUpper() == "ADVANCE DIRECTIVE" || cboDocumentSubType.SelectedValue.Trim().ToUpper() == "BIRTH PLAN") && Path.GetExtension(file_name).ToUpper() != ".PDF")
            //        {
            //            // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ADAlert", "alert('Selected Subdocument type allows only PDF files.');", true);

            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ADAlert", "displayAlert();", true);

            //            return;
            //        }
            //    }
            //}
            string[] splittedPages = txtSelectedPages.Value.Split(',');
            for (int i = 0; i < splittedPages.Length; i++)
            {
                if (splittedPages[i].Contains('-'))
                {
                    string[] splittedPages1 = splittedPages[i].Split('-');
                    if (splittedPages1[0].ToString().Trim() != string.Empty && splittedPages1[1].ToString().Trim() != string.Empty)//BugID:54356
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(splittedPages1[0], "^[0-9]*$") == true && System.Text.RegularExpressions.Regex.IsMatch(splittedPages1[1], "^[0-9]*$") == true)
                        {
                            if (Convert.ToInt32(splittedPages1[0]) <= Convert.ToInt32(splittedPages1[1]))
                                AddToArrayList(Convert.ToInt32(splittedPages1[0]), Convert.ToInt32(splittedPages1[1]));
                            else
                                AddToArrayList(Convert.ToInt32(splittedPages1[1]), Convert.ToInt32(splittedPages1[0]));
                        }
                    }
                }
                else
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(splittedPages[i], "^[0-9]*$") == true && splittedPages[i].ToString().Trim() != string.Empty)//BugID:54356
                    {
                        AddToArrayList(Convert.ToInt32(splittedPages[i]));
                    }
                }
            }


            #region "Indexing For Orders"
            ulong phy_id = 0;
            string phy_name = string.Empty;
            if (OrdersPanel.Enabled == true)
            {
                if (OrdersPanel.Enabled == true & !cboStandingOrders.SelectedValue.Contains('|'))
                {
                    if (cboStandingOrders.Visible)
                    {
                        ListItem tempdocitem = (ListItem)cboStandingOrders.SelectedItem;
                        if (tempdocitem != null && tempdocitem.Value == string.Empty)
                        {
                            tag_item = false;
                        }
                    }

                    //if (cboLab.SelectedItem.Text != string.Empty)
                    //{
                    //    Lab_id = Convert.ToUInt64(cboLab.SelectedItem.Value);
                    //}

                    //For Git id 600
                    //if (cboPhysician.SelectedItem.Text != string.Empty)
                    //{
                    //    phy_id = Convert.ToUInt32(cboPhysician.SelectedItem.Value.ToString());
                    //}
                    if (cboOrderPhysician.SelectedItem.Text != string.Empty)
                    {
                        phy_id = Convert.ToUInt32(cboOrderPhysician.SelectedItem.Value.ToString());
                    }
                    if (cboPhysician.SelectedItem.Text != string.Empty)
                    {
                        phy_name = cboPhysician.SelectedItem.Text.ToString();
                    }
                    string[] orders_procedure_split = null;
                    string[] stringSeparators = new string[] { HttpUtility.HtmlDecode("~") };
                    string[] orders_split = null;

                    IList<Orders> insertOrderList = new List<Orders>();
                    IList<OrdersSubmit> insertordersubmitList = new List<OrdersSubmit>();

                    //string tempDocItem = cboDocumentType.SelectedItem.Value;



                    OrdersSubmit objOrdersSubmit = new OrdersSubmit();
                    objOrdersSubmit.Human_ID = ClientSession.HumanId;
                    objOrdersSubmit.Lab_Name = cboLab.SelectedItem.Text;
                    objOrdersSubmit.Physician_ID = phy_id;
                    objOrdersSubmit.Created_By = ClientSession.UserName;
                    objOrdersSubmit.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    //objOrdersSubmit.Facility_Name = ddSelectedFacility.Text;
                    objOrdersSubmit.Facility_Name = sFacilityName.ToString();
                    objOrdersSubmit.Lab_ID = Convert.ToUInt32(cboLab.SelectedValue);

                    if (dtpSpecCollection.Value != "")
                    {
                        objOrdersSubmit.Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpSpecCollection.Value));
                    }
                    else
                    {
                        objOrdersSubmit.Specimen_Collection_Date_And_Time = DateTime.UtcNow;

                    }
                    insertordersubmitList.Add(objOrdersSubmit);

                    Orders objOrder = new Orders();
                    try
                    {
                        orders_procedure_split = cboStandingOrders.SelectedItem.Text.Split('~');
                    }
                    catch
                    {

                    }
                    if (orders_procedure_split.Length > 0)
                    {
                        for (int i = 0; i < orders_procedure_split.Length; i++)
                        {
                            objOrder = new Orders();
                            if (orders_procedure_split[i] != "")
                            {
                                orders_split = orders_procedure_split[i].Split(new char[] { '-' }, 2);
                                objOrder.Lab_Procedure = orders_split[0];
                                if (orders_split.Count() > 1)
                                    objOrder.Lab_Procedure_Description = orders_split[1];
                            }
                            else if (i != 0)
                            {
                                break;
                            }
                            else
                            {
                                objOrder.Lab_Procedure = "";
                                objOrder.Lab_Procedure_Description = "";
                            }
                            objOrder.Human_ID = ClientSession.HumanId;
                            objOrder.Physician_ID = phy_id;
                            objOrdersSubmit.Order_Type = cboDocumentType.SelectedItem.Value;
                            objOrder.Created_By = ClientSession.UserName;
                            objOrder.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            insertOrderList.Add(objOrder);
                        }
                    }
                    else
                    {
                        objOrder.Human_ID = ClientSession.HumanId;
                        objOrder.Physician_ID = phy_id;
                        objOrder.Lab_Procedure = "";
                        objOrder.Lab_Procedure_Description = "";
                        objOrdersSubmit.Order_Type = cboDocumentType.SelectedItem.Value;
                        objOrder.Created_By = ClientSession.UserName;
                        objOrder.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        insertOrderList.Add(objOrder);

                    }
                    OrdersManager objorder = new OrdersManager();
                    // local_order_id = objorder.InsertDummyOrder(insertordersubmitList.ToArray(), insertOrderList.ToArray(), cboDocumentType.SelectedItem.Value.ToString(), ddSelectedFacility.Text, string.Empty);
                    local_order_id = objorder.InsertDummyOrder(insertordersubmitList.ToArray(), insertOrderList.ToArray(), cboDocumentType.SelectedItem.Value.ToString(), sFacilityName.ToString(), string.Empty);
                }
                else
                {
                    try
                    {
                        //string tempOrdersItem = cboStandingOrders.SelectedValue.Split('|')[2].ToString();
                        local_order_id = Convert.ToUInt32(cboStandingOrders.SelectedValue.Split('|')[2].ToString());
                        //Fixed for CMG lab ancillary if physician_id=0
                        IList<Orders> ilstCMGOrders = new List<Orders>();
                        ilstCMGOrders = ((IList<Orders>)Session["OrdersList"]).Where(item => item.Order_Submit_ID == local_order_id && item.Physician_ID == Convert.ToUInt32("0")).ToList<Orders>();
                        if (ilstCMGOrders.Count() > 0)
                        {
                            OrdersManager objordertemp = new OrdersManager();
                            OrdersSubmitManager objordersubmittemp = new OrdersSubmitManager();
                            IList<OrdersSubmit> ilstCMGOrdersubmitUpdate = new List<OrdersSubmit>();
                            IList<Orders> lstFinalOrderUpdate = new List<Orders>();

                            ilstCMGOrdersubmitUpdate = objordersubmittemp.GetOrdersSubmitListbyID(local_order_id);
                            //Update Ordersubmit
                            if (ilstCMGOrdersubmitUpdate.Count() > 0 && cboOrderPhysician.SelectedItem.Text != string.Empty)
                            {
                                ilstCMGOrdersubmitUpdate[0].Physician_ID = Convert.ToUInt32(cboOrderPhysician.SelectedItem.Value.ToString());
                                ilstCMGOrdersubmitUpdate[0].Modified_By = ClientSession.UserName;
                                ilstCMGOrdersubmitUpdate[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            }

                            //Update Orders
                            IList<Orders> ilstCMGOrdersupdate = new List<Orders>();
                            ilstCMGOrdersupdate = objordertemp.GetOrdersByOrderSubmitID(new List<int>() { Convert.ToInt32(local_order_id) });
                            if (ilstCMGOrdersupdate.Count() > 0 && cboOrderPhysician.SelectedItem.Text != string.Empty)
                            {
                                for (int j = 0; j < ilstCMGOrdersupdate.Count; j++)
                                {
                                    ilstCMGOrdersupdate[j].Physician_ID = Convert.ToUInt32(cboOrderPhysician.SelectedItem.Value.ToString());
                                    ilstCMGOrdersupdate[j].Modified_By = ClientSession.UserName;
                                    ilstCMGOrdersupdate[j].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                }
                            }
                            //ilstCMGOrdersubmit = ((IList<OrdersSubmit>)Session["OrdersSubmitList"]).Where(item => item.Id == local_order_id).ToList<OrdersSubmit>();
                            objordertemp.UpdateOrderAndOrdersSubmit(ilstCMGOrdersubmitUpdate.ToArray(), ilstCMGOrdersupdate.ToArray(), string.Empty);
                        }
                    }
                    catch { }
                }
                Session["OrdersList"] = lstorders;
            }

            #endregion

            scan_index scanIndexObject = null;
            scanIndexObject = new scan_index();
            IList<scan_index> insertList = new List<scan_index>();
            IList<scan_index> updateList = new List<scan_index>();
            Scan_IndexDTO AddedListOfScanIndex = null;
            //string sScan_Type = string.Empty;
            StringBuilder sScan_Type = new StringBuilder(); ;
            if (btnSave.Text == "Add")
            {
                settingTheObject(ref scanIndexObject, "Add", local_order_id);
                insertList.Add(scanIndexObject);
            }
            else
            {
                scanIndexObject = ((IList<scan_index>)Session["EditList"])[0];
                settingTheObject(ref scanIndexObject, "Update", local_order_id);
                updateList.Add(scanIndexObject);
            }
            if (btnSave.Text == "Add" && ((grdIndexing.Items.Count == 0 && file_name.Length != 0)))// || ((string)Session["LastIndexFileName"] != null && (string)Session["LastIndexFileName"] != file_name)))
            {
                //Session["LastIndexFileName"] = file_name;
                //string full_path = hdnsourceFile.Value.Replace(@"/", @"\").Replace(@"\\", @"\");
                StringBuilder full_path = new StringBuilder();
                full_path.Append(hdnsourceFile.Value.Replace(@"/", @"\").Replace(@"\\", @"\"));
                DateTime dtScanReceivedDate = Convert.ToDateTime(dtpScannedDate.Value);

                sScan_Type = new StringBuilder();
                if (rdbRemoteDir.Checked == true)
                    sScan_Type.Append("Online Chart - " + ddlSourceType.Text);
                else if (rdbLocalDir.Checked == true)
                    sScan_Type.Append("Online Chart - LOCAL");

                if (hdnIsMyScan.Value != "true")
                    scan_ID = 0;

                // AddedListOfScanIndex = scanIndesmanager.SaveUpdateDeleteOnlineDocuments(insertList.ToArray<scan_index>(), new List<scan_index>(), new List<scan_index>(), ClientSession.HumanId, scan_ID, string.Empty, ClientSession.UserName, ddSelectedFacility.Text, full_path, (int)Session["Page_Count"], Path.GetFileName(full_path));// txtFileName.Value);
                int ipagecount = 1;
                if (hdnPagecount.Value != null && hdnPagecount.Value != "" && System.Text.RegularExpressions.Regex.IsMatch(hdnPagecount.Value, "^[0-9]*$") == true)
                {
                    ipagecount = Convert.ToInt32(hdnPagecount.Value);
                }
                AddedListOfScanIndex = scanIndesmanager.SaveUpdateDeleteOnlineDocuments(insertList.ToArray<scan_index>(), new List<scan_index>(), new List<scan_index>(), ClientSession.HumanId, scan_ID, string.Empty, ClientSession.UserName, sFacilityName.ToString(), full_path.ToString(), ipagecount, Path.GetFileName(full_path.ToString()), dtScanReceivedDate, sScan_Type.ToString());
                if (insertList.Count > 0)
                    scan_ID = insertList[0].Scan_ID;



                IList<scan_index> indexListOnSession = ((IList<scan_index>)Session["IndexList"]);
                if (indexListOnSession == null)
                {
                    indexListOnSession = new List<scan_index>();
                }
                foreach (scan_index item in insertList)
                {
                    indexListOnSession.Add(item);
                }

                Session["IndexList"] = indexListOnSession;
                LoadGridView(indexListOnSession);
            }
            else
            {
                AddedListOfScanIndex = scanIndesmanager.SaveUpdateDeleteScanIndex(insertList.ToArray<scan_index>(), updateList.ToArray<scan_index>(), new List<scan_index>(), ClientSession.HumanId, scan_ID, string.Empty);
                if (insertList.Count > 0)
                    scan_ID = insertList[0].Scan_ID;
                else
                {
                    if (updateList.Count > 0)
                        scan_ID = updateList[0].Scan_ID;
                }
                IList<scan_index> indexListOnSession = ((IList<scan_index>)Session["IndexList"]);
                if (indexListOnSession == null)
                {
                    indexListOnSession = new List<scan_index>();
                }

                foreach (scan_index item in insertList)
                {
                    indexListOnSession.Add(item);
                }
                //record variable not used

                if (updateList != null && updateList.Count > 0)
                {
                    foreach (scan_index item in updateList)
                    {
                        if (indexListOnSession != null && indexListOnSession.Count > 0)//BugID:54354
                        {
                            //CAP-790
                            if (indexListOnSession.FirstOrDefault(a => a.Id == item.Id) != null)
                            {
                                var record = indexListOnSession.FirstOrDefault(a => a.Id == item.Id).Version++;
                            }
                        }

                    }
                }

                Session["IndexList"] = indexListOnSession;
                LoadGridView(indexListOnSession);
            }



            Session["ScanId"] = scan_ID;
            btnSave.Text = "Add";
            //rdbPageRange.Checked = true;
            //txtSelectedPages.Disabled = false;
            //btnSave.Enabled = true;
            rdbAll.Checked = true;
            btnUpload.Disabled = false;
            //For Fax change it is commited
            //cboDocumentType.SelectedIndex = 0;
            //cboDocumentSubType.SelectedItem.Text = ""; ;
            if (cboIs_Interperated.Items != null && cboIs_Interperated.Items.Count > 0)
            {
                if (cboIs_Interperated.SelectedIndex > -1)
                {
                    cboIs_Interperated.ClearSelection();
                }

                cboIs_Interperated.SelectedIndex = 0;
            }
            // cboDocumentSubType.Items.Clear();
            //

            txtSelectedPages.Value = string.Empty;
            btnClearAll.Text = "Reset";
            // cboFromPage.Value = "";
            // cboToPage.Value = "";
            selectedPageNumbers.Clear();
            TotalPagesSelected.Clear();
            if (OrdersPanel.Enabled == true)
            {
                cboStandingOrders.Items.Clear();
                if (cboLab.Items != null && cboLab.Items.Count > 0)
                {
                    if (cboLab.SelectedIndex > -1)
                    {
                        cboLab.ClearSelection();
                    }

                    cboLab.SelectedIndex = 0;

                }
                if (cboPhysician.Items != null && cboPhysician.Items.Count > 0)
                {
                    if (cboPhysician.SelectedIndex > -1)
                    {
                        cboPhysician.ClearSelection();
                    }
                    cboPhysician.SelectedIndex = 0;
                }
                if (cboIs_Interperated.Items != null && cboIs_Interperated.Items.Count > 0)
                {
                    if (cboIs_Interperated.SelectedIndex > -1)
                    {
                        cboIs_Interperated.ClearSelection();
                    }
                    cboIs_Interperated.SelectedIndex = 0;
                }
                dtpSpecCollection.Value = "";
            }
            HideOrders();
            //if (grdIndexing.Items.Count > 0)
            //    btnFindPatient.Disabled = true;
            //else
            //    btnFindPatient.Disabled = false;
            // chkShowAll.Enabled = true;
            //else
            // Labspan.Attributes.Remove("class");
            //Labspan.Attributes.Add("class", "MandLabelstyle");
            //  slabMandatory.Attributes.Add("class", "manredforstar");
            slabMandatory.Visible = false;

            waitCursor.Update();
            updateOrders.Update();
            updateGrid.Update();
            rdbAll.Checked = true;
            btnSave.Enabled = false;
            txtSelectedPages.Value = "";
            txtSelectedPages.Disabled = true;
            btnMoveToNextProcess.Disabled = false;
            //Cap - 571
            chkReviewandSign.Checked = false;
            // IsSaveClickedSucessfull.Value = "Success";
            if (IsClickDirectUpload.Value == "Yes")
            {
                IsClickDirectUpload.Value = "No";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ClickMovetoNextProcess", "ShowLoading();ClickMovetoNextProcess();", true);
            }
            else
            {
                if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER")
                {
                    LoadDocType();
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfullyAlert", "document.getElementById('hdnIsEditgrid').value='';localStorage.setItem('IsSaveClickedSucessfull', 'Success');CheckAll();DisplayErrorMessage('114003');", true);
                divLoading.Style.Add("display", "none");
            }
            IsClickDirectUpload.Value = "No";
            hdnIsEditgrid.Value = "";
            //CAP-1633
            dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");

            //IList<Scan> ilstBrowseScan = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]);
            //IList<Scan> NewScanlst = new List<Scan>();
            //IList<string> lstDocuments = new List<string>();
            //if (ilstBrowseScan != null)
            //{
            //    foreach (Scan scan in ilstBrowseScan)
            //    {
            //        if (File.Exists(file_name))
            //        {
            //            if (scan.Scanned_File_Name != Path.GetFileName(file_name))
            //            {
            //                NewScanlst.Add(scan);
            //                lstDocuments.Add(scan.Scanned_File_Name);
            //            }
            //        }

            //    }
            //}
            //Session["BrowseLoadList"] = NewScanlst;
            //Session["BrowseFileNames"] = lstDocuments;
        }

        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {
            if (rdbAll.Checked == true)
            {
                btnSave.Enabled = false;
                txtSelectedPages.Value = "";
                txtSelectedPages.Disabled = true;
                btnMoveToNextProcess.Disabled = false;
            }
            else if (rdbPageRange.Checked == true)
            {
                btnSave.Enabled = true;
                txtSelectedPages.Disabled = false;
                btnMoveToNextProcess.Disabled = true;
            }
            if (hdnHumanID.Value == "" && PatientDetails.Text != "")
            {
                if (PatientDetails.Text.Contains(':'))
                    hdnHumanID.Value = PatientDetails.Text.Split(':')[1].Split('|')[0];
            }
            if (hdnHumanID.Value == "")//|| PatientDetails.Text == "")
            {
                divLoading.Style.Add("display", "none");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "HUmanID", "StopLoadOnUploadFile();DisplayErrorMessage('390006');", true);
                return;
            }
            file_name = new StringBuilder();
            file_name.Append(hdnsourceFile.Value);//(string)Session["FileName"];
            if (file_name.Length == 0)
            {
                divLoading.Style.Add("display", "none");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileChoose", "StopLoadOnUploadFile();DisplayErrorMessage('114017');", true);
                return;
            }
            if (Path.GetFileNameWithoutExtension(file_name.ToString()).ToString().Contains((Path.GetExtension(file_name.ToString()))) == true)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Fileextension", "StopLoadOnUploadFile();alert('The selected file can not be indexed as the file type extension (Example .pdf) is repeated twice.  Please rescan the file with proper naming convention.');", true);
                return;
            }
            if (Path.GetExtension(file_name.ToString()).ToString().ToUpper() == ".PDF" && Path.GetFileNameWithoutExtension(file_name.ToString()).ToString().Contains("#") == true)
            {
                //CAP-1556
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Fileextension", "StopLoadOnUploadFile(); document.getElementById('divLoading').style.display = 'none'; alert('The selected file can not be indexed as the file has the special character such as #.Please rename the file and retry again.');", true);
                return;
            }

            //if (hdnIsGrid.Value != "true")
            //{
            //    if (cboDocumentType.SelectedIndex == 0)
            //    {
            //        divLoading.Style.Add("display", "none");
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DocType", "StopLoadOnUploadFile();DisplayErrorMessage('115043');", true);
            //        return;
            //    }
            //    if (cboDocumentSubType.SelectedIndex == 0)
            //    {
            //        divLoading.Style.Add("display", "none");
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Subdoctype", "StopLoadOnUploadFile();DisplayErrorMessage('115044');", true);
            //        return;
            //    }
            //    if (cboDocumentType.SelectedValue.ToUpper() == "DIAGNOSTIC ORDER")
            //    {
            //        if (cboPhysician.SelectedIndex == 0)
            //        {
            //            divLoading.Style.Add("display", "none");
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "FileChoose", "StopLoadOnUploadFile();DisplayErrorMessage('114018');", true);
            //            return;
            //        }

            //    }
            //}

            MoveToNextProcess();
            //left_out_Page = string.Empty;
            if (Request.QueryString["CurrentZone"] != null && Request.QueryString["CurrentZone"].Trim() != "")
            {
                //string Offset = Request.QueryString["CurrentZone"];
                StringBuilder Offset = new StringBuilder();
                Offset.Append(Request.QueryString["CurrentZone"]);
                if (Offset.ToString().StartsWith("-"))
                {
                    DateTime localTime = DateTime.Now.ToUniversalTime().Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0));
                    dtpDocumentDate.Value = localTime.ToString("dd-MMM-yyyy");
                    //  dtpScannedDate.Value = localTime.ToString("dd-MMM-yyyy");
                }
                else
                {
                    DateTime localTime = DateTime.Now.ToUniversalTime().AddMinutes(Convert.ToDouble(Offset.ToString()));
                    dtpDocumentDate.Value = localTime.ToString("dd-MMM-yyyy");
                    // dtpScannedDate.Value = localTime.ToString("dd-MMM-yyyy");
                }
            }
            else
            {
                dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                // dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            hdnHumanID.Value = "";
            rdbAll.Checked = true;
            btnSave.Enabled = false;
            txtSelectedPages.Value = "";
            txtSelectedPages.Disabled = true;
            btnMoveToNextProcess.Disabled = false;
            LoadDocType();
            hdnIsEditgrid.Value = "";
            if (hdnIsMyScan.Value == "true")
            {
                hdnIsMyScan.Value = "";
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "CloseAndDisplayAlert", "document.getElementById('hdnIsEditgrid').value='';CloseAndDisplayAlert();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ClickClear", "document.getElementById('hdnIsEditgrid').value='';ClickClear();DisplayErrorMessage('115018');", true);
            }
        }

        //protected void btnClearAll_Click(object sender, EventArgs e)
        //{
        //    btnSave.Text = "Add";
        //    btnClearAll.Text = "Reset";
        //    txtSelectedPages.Value = "";

        //    cboIs_Interperated.SelectedIndex = 0;
        //    dtpSpecCollection.Value = "";
        //    HideOrders();
        //    if (cboStandingOrders.Items.Count > 0)
        //    {
        //        cboStandingOrders.Items.Clear();
        //    }
        //    cboPhysician.Items.Clear();
        //    cboLab.Items.Clear();
        //    btnSave.Enabled = false;
        //    rdbAll.Checked = true;
        //    //cboDocumentType.SelectedIndex = 0;
        //    //cboDocumentSubType.Items.Clear();
        //    LoadDocType();
        //    //btnMoveToNextProcess.Disabled = true;
        //    //if (Request.QueryString["HumanId"] == "" && Request.QueryString["HumanId"] == null)
        //    //{
        //    //    PatientDetails.Text = "";
        //    //    btnFindPatient.Disabled = true;
        //    //}

        //    ddSelectedFacility.Text = ClientSession.FacilityName;


        //    if (Request.QueryString["CurrentZone"] != null)
        //    {
        //        string Offset = Request.QueryString["CurrentZone"];
        //        if (Offset.StartsWith("-"))
        //        {
        //            DateTime localTime = DateTime.Now.ToUniversalTime().Subtract(new TimeSpan(0, Convert.ToInt32(Offset), 0));
        //            dtpDocumentDate.Value = localTime.ToShortDateString();
        //            dtpScannedDate.Value = localTime.ToShortDateString();
        //            productionTime = localTime;
        //        }
        //        else
        //        {
        //            DateTime localTime = DateTime.Now.ToUniversalTime().AddMinutes(Convert.ToDouble(Offset));
        //            dtpDocumentDate.Value = localTime.ToShortDateString();
        //            dtpScannedDate.Value = localTime.ToShortDateString();
        //            productionTime = localTime;
        //        }
        //    }
        //    else
        //    {
        //        dtpDocumentDate.Value = DateTime.Now.ToShortDateString();
        //        dtpScannedDate.Value = DateTime.Now.ToShortDateString();
        //    }

        //}

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {

            MoveToNextProcess();

        }

        //protected void btnSearchCpt_Click(object sender, EventArgs e)
        //{
        //  //  RadProcedureWindow.VisibleOnPageLoad = true;
        //}

        protected void btnInvisible_Click(object sender, EventArgs e)
        {


            btnSave.Text = "Add";
            btnSave.Enabled = false;
            if (cboDocumentType.Items != null && cboDocumentType.Items.Count > 0)
            {
                if (cboDocumentType.SelectedIndex > -1)
                {
                    cboDocumentType.ClearSelection();
                }
                cboDocumentType.SelectedIndex = 0;
            }
            if (cboDocumentSubType.Items != null && cboDocumentSubType.Items.Count > 0)
            {
                if (cboDocumentSubType.SelectedIndex > -1)
                {
                    cboDocumentSubType.ClearSelection();
                }
                cboDocumentSubType.SelectedIndex = 0;
                //cboDocumentSubType.SelectedItem.Text = "";
            }
            if (ddEncPhyName.Items != null && ddEncPhyName.Items.Count > 0)
            {
                if (ddEncPhyName.SelectedIndex > -1)
                {
                    ddEncPhyName.ClearSelection();
                }
                ddEncPhyName.SelectedIndex = 0;
            }
            txtSelectedPages.Value = string.Empty;
            btnClearAll.Text = "Reset";
            // cboFromPage.Value = "";
            HideOrders();
            divLoading.Style.Add("display", "none");
            waitCursor.Update();
            updateGrid.Update();
        }

        protected void btnIVfindpatient_Click(object sender, EventArgs e)
        {

            if (hdnHumanID.Value != null && hdnHumanID.Value != string.Empty)
            {

                if (hdnHumanID != null && hdnHumanID.Value != "0" && hdnHumanID.Value != "undefined")
                {
                    ulong human_id = Convert.ToUInt32(hdnHumanID.Value.ToString());
                    if (PatientDetails.Text != string.Empty)
                    {
                        cboDocumentType.Enabled = true;
                        if (cboDocumentType.Items != null && cboDocumentType.Items.Count > 0)
                        {
                            if (cboDocumentType.SelectedIndex > -1)
                            {
                                cboDocumentType.ClearSelection();
                            }
                            cboDocumentType.SelectedIndex = 0;
                        }
                        HideOrders();
                    }
                    else
                    {
                        cboDocumentType.Enabled = false;
                    }
                }

            }
            divLoading.Style.Add("display", "none");
            waitCursor.Update();

        }

        protected void btnIVProcedure_Click(object sender, EventArgs e)
        {
            //RadProcedureWindow.VisibleOnPageLoad = false;

            if (hdnProcedure.Value != null && hdnProcedure.Value != string.Empty)
            {
                string[] objSearch = hdnProcedure.Value.Split('|');
                //string orders = string.Empty;
                StringBuilder orders = new StringBuilder();
                for (int i = 0; i < objSearch.Length; i++)
                {
                    //orders += objSearch[i].ToString() + "~";
                    orders.Append(objSearch[i].ToString() + "~");
                }
                //string temp_order = orders.Substring(0, orders.Length - 2);

                for (int i = 0; i < cboStandingOrders.Items.Count; i++)
                {
                    if (cboStandingOrders.Items[i].Text.Contains("|") == false && cboStandingOrders.Items[i].Text != "Paper Order")
                    {
                        cboStandingOrders.Items.Remove(cboStandingOrders.Items.FindByText(cboStandingOrders.Items[i].Text));
                    }
                }

                ListItem comboItem = new ListItem(orders.ToString());
                comboItem.Text = orders.ToString();
                //string sPhyId = cboPhysician.SelectedValue;
                StringBuilder sPhyId = new StringBuilder();
                sPhyId.Append(cboOrderPhysician.SelectedValue);
                comboItem.Value = cboOrderPhysician.SelectedValue + "|" + cboLab.SelectedValue;
                cboStandingOrders.Items.Insert(0, orders.ToString());
                ListItem selectedOrder = cboStandingOrders.Items.FindByText(orders.ToString());
                //cboStandingOrders.SelectedItem.Text = orders;
                if (selectedOrder != null)
                {
                    cboStandingOrders.ClearSelection();
                    selectedOrder.Selected = true;
                }
                cboIs_Interperated.Enabled = true;
                cboStandingOrders.Enabled = true;
                dtpSpecCollection.Disabled = false;
                // LoadPhysicianCombo();
                //string sFacilityCmg = ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper();
                //if (sFacilityCmg.ToUpper() == ClientSession.FacilityName.ToUpper())
                //{
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    ShowAllphysicianforCMGAncillary();
                    ShowAllOrderingphysicianforCMGAncillary();
                    chkShowAll.Checked = true;
                    chkOrderingPhyShowAll.Checked = true;
                }
                else
                {
                    LoadPhysicianCombo();
                    LoadOrderingPhysicianCombo();
                    chkShowAll.Checked = false;
                    chkOrderingPhyShowAll.Checked = false;
                }

                // For git id 600
                //if (cboPhysician.Items.FindByValue(sPhyId.ToString()) != null)
                //    cboPhysician.SelectedValue = sPhyId.ToString();
                //cboPhysician.Enabled = false;
                cboPhysician.Enabled = true;
                cboLab.Enabled = false;
                btnSearchCpt.Disabled = true;
                chkShowAll.Enabled = true;
                LoadOrderingPhysicianCombo();
                chkOrderingPhyShowAll.Enabled = false;
                if (cboOrderPhysician.Items.FindByValue(sPhyId.ToString()) != null)
                    cboOrderPhysician.SelectedValue = sPhyId.ToString();
                cboOrderPhysician.Enabled = false;
            }
            divLoading.Style.Add("display", "none");
            waitCursor.Update();

        }

        #endregion

        #region "File Operations"
        public static System.Drawing.Image[] splitTiffImageToSeparateImages(string sourceFile, IList<int> pageNumbers)
        {
            System.Drawing.Image sourceImage = null;
            System.Drawing.Image returnImage = null;
            System.Drawing.Image[] returnImageList = new System.Drawing.Image[pageNumbers.Count];

            try
            {
                if (sourceFile != string.Empty)
                {
                    FileStream fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    sourceImage = System.Drawing.Image.FromStream(fileStream);
                    int j = 0;
                    for (int i = 0; i < pageNumbers.Count; i++)
                    {
                        returnImage = getTiffImage(sourceImage, pageNumbers[i] - 1);
                        returnImage.Tag = i;
                        if (i == 2)
                        {
                            returnImage.Tag = i + 1;
                        }
                        returnImageList[j] = returnImage;
                        j++;
                    }
                    sourceImage.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                returnImage = null;
            }

            return returnImageList;
        }

        public static System.Drawing.Image getTiffImage(System.Drawing.Image sourceImage, int pageNumber)
        {
            MemoryStream ms = null;
            System.Drawing.Image returnImage = null;

            try
            {
                ms = new MemoryStream();
                Guid objGuid = sourceImage.FrameDimensionsList[0];
                FrameDimension objDimension = new FrameDimension(objGuid);
                sourceImage.SelectActiveFrame(objDimension, pageNumber);
                sourceImage.Save(ms, ImageFormat.Tiff);
                returnImage = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ms.Close();
            }

            return returnImage;
        }
        
        public static bool saveMultipage(System.Drawing.Image[] bmp, string location, string type, out string sCheckFileNotFoundException)
        {
            sCheckFileNotFoundException = "";
            if (bmp != null && location != null)
            {
                //Jira #CAP-39
                int iTryCount = 1;
            TryAgain:
                try
                {
                    ImageCodecInfo codecInfo = getCodecForstring(type);
                    //CAP-935
                    if (bmp.Length == 1 && bmp[0] != null)
                    {
                        EncoderParameters iparams = new EncoderParameters(1);
                        System.Drawing.Imaging.Encoder iparam = System.Drawing.Imaging.Encoder.Compression;
                        EncoderParameter iparamPara = new EncoderParameter(iparam, (long)(EncoderValue.CompressionCCITT4));
                        iparams.Param[0] = iparamPara;
                        bmp[0].Save(location, codecInfo, iparams);
                    }
                    else if (bmp.Length > 1 && bmp[0] != null)
                    {
                        EncoderParameter SaveEncodeParam;
                        EncoderParameter CompressionEncodeParam;


                        System.Drawing.Imaging.Encoder saveEncoder = System.Drawing.Imaging.Encoder.SaveFlag;
                        System.Drawing.Imaging.Encoder compressionEncoder = System.Drawing.Imaging.Encoder.Compression;
                        EncoderParameters EncoderParams = new EncoderParameters(2);

                        SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.MultiFrame);
                        CompressionEncodeParam = new EncoderParameter(compressionEncoder, (long)EncoderValue.CompressionCCITT4);
                        EncoderParams.Param[0] = CompressionEncodeParam;
                        EncoderParams.Param[1] = SaveEncodeParam;

                        if (location != string.Empty)
                        {

                            try
                            {
                                File.Delete(location);
                            }
                            catch
                            { }
                        }
                        bmp[0].Save(location, codecInfo, EncoderParams);
                        //string s = "haiiii";
                        StringBuilder s = new StringBuilder();
                        s.Append("haiiii");
                        bmp[0].Tag = (object)s;
                        for (int i = 1; i < bmp.Length; i++)
                        {
                            if (bmp[i] == null)
                                break;

                            SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.FrameDimensionPage);
                            CompressionEncodeParam = new EncoderParameter(compressionEncoder, (long)EncoderValue.CompressionCCITT4);


                            EncoderParams.Param[0] = CompressionEncodeParam;
                            EncoderParams.Param[1] = SaveEncodeParam;
                            if (bmp[i] != null)
                            {
                                bmp[i].Tag = (object)s.ToString();
                            }
                            bmp[0].SaveAdd(bmp[i], EncoderParams);
                        }

                        SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.Flush);
                        EncoderParams.Param[0] = SaveEncodeParam;
                        bmp[0].SaveAdd(EncoderParams);
                    }
                    return true;
                }
                catch (System.Exception ee)
                {
                    string sErrorMessage = "";
                    if (UtilityManager.CheckFileNotFoundException(ee, out sErrorMessage))
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + sErrorMessage + "');", true);
                        sCheckFileNotFoundException = "CheckFileNotFoundException ~" + sErrorMessage;
                        return false;
                    }
                    else
                    {
                        //Jira #CAP-39
                        if (iTryCount <= 3)
                        {
                            iTryCount = iTryCount + 1;
                            Thread.Sleep(1500);
                            goto TryAgain;
                        }
                        else
                        {
                            UtilityManager.RetryExecptionLog(ee, iTryCount);
                            if (ee.Message != null)
                            {
                                throw new Exception(ee.Message + "  Error in saving as multipage ");
                            }
                            else
                            {
                                throw new Exception(" Error in saving as multipage ");
                            }
                        }
                    }
                }
            }
            return false;

        }

        private static ImageCodecInfo getCodecForstring(string type)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            if (info != null)
            {
                for (int i = 0; i < info.Length; i++)
                {
                    //string EnumName = type.ToString();
                    if (info[i].FormatDescription.Equals(type.ToString()))
                    {
                        return info[i];
                    }
                }
            }
            return null;
        }

        #endregion

        #region "Methods"
        /// <summary>
        /// To load the Physician list to screen from XML available in the Config Directory
        /// </summary>
        /// <exception cref="FileNotFound">Occurs if XML file not available in the config directory</exception>
        public void LoadPhysicianCombo()
        {
            //string sDefPhyID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
            cboPhysician.Items.Clear();
            /* Code Block to populate physician list */
            //XDocument xmlDocumentType = null;
            //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml")))
            //    xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            ListItem liDropdown = null;
            IList<ListItem> liComboItems = new List<ListItem>();
            if (physicianFacilityMappingList != null)
            {
                foreach (var facility in physicianFacilityMappingList.PhysicianFacility)
                {
                    //string xmlValue = elements.Attribute("name").Value;
                    if (ddSelectedFacility.Text.ToUpper() != null)
                    {
                        var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == facility.name select f;
                        IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                        if (facility.name.ToUpper() == ddSelectedFacility.Text.ToUpper() && ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")// && elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                        {
                            foreach (var phyItems in facility.Physician)
                            {
                                //Old Code
                                //StringBuilder phyName = new StringBuilder();
                                //StringBuilder username = new StringBuilder();
                                //StringBuilder prefix = new StringBuilder();
                                //StringBuilder firstname = new StringBuilder();
                                //StringBuilder middlename = new StringBuilder();
                                //StringBuilder lastname = new StringBuilder();
                                //StringBuilder suffix = new StringBuilder();
                                //StringBuilder phyID = new StringBuilder();

                                //if (phyItems.Attribute("username").Value != null)
                                //    username.Append(phyItems.Attribute("username").Value);
                                //if (phyItems.Attribute("prefix").Value != null)
                                //    prefix.Append(phyItems.Attribute("prefix").Value);
                                //if (phyItems.Attribute("firstname").Value != null)
                                //    firstname.Append(phyItems.Attribute("firstname").Value);
                                //if (phyItems.Attribute("middlename").Value != null)
                                //    middlename.Append(phyItems.Attribute("middlename").Value);
                                //if (phyItems.Attribute("lastname").Value != null)
                                //    lastname.Append(phyItems.Attribute("lastname").Value);
                                //if (phyItems.Attribute("suffix").Value != null)
                                //    suffix.Append(phyItems.Attribute("suffix").Value);
                                ////if (phyItems.Attribute("phyID").Value != null)
                                //// phyID = phyItems.Attribute("phyID").Value;
                                //if (phyItems.Attribute("ID").Value != null)
                                //    phyID.Append(phyItems.Attribute("ID").Value);

                                //if (prefix.Length != 0)
                                //{
                                //    phyName.Append(prefix.ToString());
                                //}
                                //if (firstname.Length != 0)
                                //{
                                //    phyName.Append(firstname.ToString());
                                //}
                                //if (middlename.Length != 0)
                                //{
                                //    phyName.Append(middlename.ToString());
                                //}
                                //if (lastname.Length != 0)
                                //{
                                //    phyName.Append(lastname.ToString());
                                //}
                                //if (suffix.Length != 0)
                                //{
                                //    phyName.Append(suffix.ToString());
                                //}
                                //Gitlab# 2485 - Physician Name Display Change
                                string phyName = String.Empty;
                                if (phyItems.lastname != String.Empty)
                                    phyName += phyItems.lastname;
                                if (phyItems.firstname != String.Empty)
                                {
                                    if (phyName != String.Empty)
                                        phyName += "," + phyItems.firstname;
                                    else
                                        phyName += phyItems.firstname;
                                }
                                if (phyItems.middlename != String.Empty)
                                    phyName += " " + phyItems.middlename;
                                if (phyItems.suffix != String.Empty)
                                    phyName += "," + phyItems.suffix;

                                liDropdown = new ListItem(phyName, phyItems.ID);
                                liDropdown.Attributes.Add("default", "true");
                                liComboItems.Add(liDropdown);

                            }

                        }
                        else
                        {
                            var vfacAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == facility.name select f;
                            IList<FacilityLibrary> lstFacAncillary = vfacAncillary.ToList<FacilityLibrary>();
                            if (lstFacAncillary.Count > 0 && lstFacAncillary[0].Is_Ancillary != "Y" && facility.Legal_Org == ClientSession.LegalOrg)// && elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                            {
                                foreach (var phyItems in facility.Physician)
                                {
                                    //Old Code
                                    //StringBuilder phyName = new StringBuilder();
                                    //StringBuilder username = new StringBuilder();
                                    //StringBuilder prefix = new StringBuilder();
                                    //StringBuilder firstname = new StringBuilder();
                                    //StringBuilder middlename = new StringBuilder();
                                    //StringBuilder lastname = new StringBuilder();
                                    //StringBuilder suffix = new StringBuilder();
                                    //StringBuilder phyID = new StringBuilder();

                                    //if (phyItems.Attribute("username").Value != null)
                                    //    username.Append(phyItems.Attribute("username").Value);
                                    //if (phyItems.Attribute("prefix").Value != null)
                                    //    prefix.Append(phyItems.Attribute("prefix").Value);
                                    //if (phyItems.Attribute("firstname").Value != null)
                                    //    firstname.Append(phyItems.Attribute("firstname").Value);
                                    //if (phyItems.Attribute("middlename").Value != null)
                                    //    middlename.Append(phyItems.Attribute("middlename").Value);
                                    //if (phyItems.Attribute("lastname").Value != null)
                                    //    lastname.Append(phyItems.Attribute("lastname").Value);
                                    //if (phyItems.Attribute("suffix").Value != null)
                                    //    suffix.Append(phyItems.Attribute("suffix").Value);
                                    ////if (phyItems.Attribute("phyID").Value != null)
                                    //// phyID = phyItems.Attribute("phyID").Value;
                                    //if (phyItems.Attribute("ID").Value != null)
                                    //    phyID.Append(phyItems.Attribute("ID").Value);

                                    //if (prefix.Length != 0)
                                    //{
                                    //    phyName.Append(prefix.ToString());
                                    //}
                                    //if (firstname.Length != 0)
                                    //{
                                    //    phyName.Append(firstname.ToString());
                                    //}
                                    //if (middlename.Length != 0)
                                    //{
                                    //    phyName.Append(middlename.ToString());
                                    //}
                                    //if (lastname.Length != 0)
                                    //{
                                    //    phyName.Append(lastname.ToString());
                                    //}
                                    //if (suffix.Length != 0)
                                    //{
                                    //    phyName.Append(suffix.ToString());
                                    //}
                                    //Gitlab# 2485 - Physician Name Display Change
                                    string phyName = String.Empty;
                                    if (phyItems.lastname != String.Empty)
                                        phyName += phyItems.lastname;
                                    if (phyItems.firstname != String.Empty)
                                    {
                                        if (phyName != String.Empty)
                                            phyName += "," + phyItems.firstname;
                                        else
                                            phyName += phyItems.firstname;
                                    }
                                    if (phyItems.middlename != String.Empty)
                                        phyName += " " + phyItems.middlename;
                                    if (phyItems.suffix != String.Empty)
                                        phyName += "," + phyItems.suffix;


                                    liDropdown = new ListItem(phyName, phyItems.ID);
                                    if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] == phyItems.ID)
                                        liDropdown.Attributes.Add("default", "true");
                                    else
                                        liDropdown.Attributes.Add("default", "false");
                                    liDropdown.Attributes.CssStyle.Add("display", "none");
                                    liComboItems.Add(liDropdown);
                                }
                            }
                        }
                    }
                }
            }
            IList<ListItem> sortlst = liComboItems.OrderByDescending(x => x.Attributes["default"]).ToList();
            sortlst = sortlst.OrderBy(x => x.Text).Distinct().ToList();
            cboPhysician.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboPhysician.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();
        }

        public void LoadOrderingPhysicianCombo()
        {
            //string sDefPhyID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
            cboOrderPhysician.Items.Clear();
            /* Code Block to populate physician list */
            XDocument xmlDocumentType = null;
            //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml")))
            //xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            ListItem liDropdown = null;
            IList<ListItem> liComboItems = new List<ListItem>();
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null) { 
                foreach (var facility in physicianFacilityMappingList.PhysicianFacility)
                {
                    //string xmlValue = elements.Attribute("name").Value;
                    if (ddSelectedFacility.Text.ToUpper() != null)
                    {
                        //if (elements.Attribute("name").Value.ToUpper() == ddSelectedFacility.Text.ToUpper() && elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                        //{
                        var vfacAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == facility.name select f;
                        IList<FacilityLibrary> lstFacAncillary = vfacAncillary.ToList<FacilityLibrary>();
                        //Cap - 1119
                        //if (elements.Attribute("name").Value.ToUpper() == ddSelectedFacility.Text.ToUpper() && lstFacAncillary.Count > 0 && lstFacAncillary[0].Is_Ancillary != "Y")// && elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                        if (facility.name.ToUpper() == ddSelectedFacility.Text.ToUpper() && lstFacAncillary.Count > 0 && lstFacAncillary[0].Is_Ancillary != "Y" && facility.Legal_Org == ClientSession.LegalOrg)
                        {
                            foreach (var phyItems in facility.Physician)
                            {
                                //Old Code 
                                //StringBuilder phyName = new StringBuilder();
                                //StringBuilder username = new StringBuilder();
                                //StringBuilder prefix = new StringBuilder();
                                //StringBuilder firstname = new StringBuilder();
                                //StringBuilder middlename = new StringBuilder();
                                //StringBuilder lastname = new StringBuilder();
                                //StringBuilder suffix = new StringBuilder();
                                //StringBuilder phyID = new StringBuilder();

                                //if (phyItems.Attribute("username").Value != null)
                                //    username.Append(phyItems.Attribute("username").Value);
                                //if (phyItems.Attribute("prefix").Value != null)
                                //    prefix.Append(phyItems.Attribute("prefix").Value);
                                //if (phyItems.Attribute("firstname").Value != null)
                                //    firstname.Append(phyItems.Attribute("firstname").Value);
                                //if (phyItems.Attribute("middlename").Value != null)
                                //    middlename.Append(phyItems.Attribute("middlename").Value);
                                //if (phyItems.Attribute("lastname").Value != null)
                                //    lastname.Append(phyItems.Attribute("lastname").Value);
                                //if (phyItems.Attribute("suffix").Value != null)
                                //    suffix.Append(phyItems.Attribute("suffix").Value);
                                ////if (phyItems.Attribute("phyID").Value != null)
                                //// phyID = phyItems.Attribute("phyID").Value;
                                //if (phyItems.Attribute("ID").Value != null)
                                //    phyID.Append(phyItems.Attribute("ID").Value);

                                //if (prefix.Length != 0)
                                //{
                                //    phyName.Append(prefix.ToString());
                                //}
                                //if (firstname.Length != 0)
                                //{
                                //    phyName.Append(firstname.ToString());
                                //}
                                //if (middlename.Length != 0)
                                //{
                                //    phyName.Append(middlename.ToString());
                                //}
                                //if (lastname.Length != 0)
                                //{
                                //    phyName.Append(lastname.ToString());
                                //}
                                //if (suffix.Length != 0)
                                //{
                                //    phyName.Append(suffix.ToString());
                                //}

                                //Gitlab# 2485 - Physician Name Display Change
                                string phyName = String.Empty;
                                if (phyItems.lastname != String.Empty)
                                    phyName += phyItems.lastname;
                                if (phyItems.firstname != String.Empty)
                                {
                                    if (phyName != String.Empty)
                                        phyName += "," + phyItems.firstname;
                                    else
                                        phyName += phyItems.firstname;
                                }
                                if (phyItems.middlename != String.Empty)
                                    phyName += " " + phyItems.middlename;
                                if (phyItems.suffix != String.Empty)
                                    phyName += "," + phyItems.suffix;



                                liDropdown = new ListItem(phyName, phyItems.ID);
                                liDropdown.Attributes.Add("default", "true");
                                liComboItems.Add(liDropdown);

                            }

                        }
                        else
                        {
                            //if (elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                            //{
                            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == facility.name select f;
                            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y" && facility.Legal_Org == ClientSession.LegalOrg)// && elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                            {
                                foreach (var phyItems in facility.Physician)
                                {
                                    //Old Code
                                    //StringBuilder phyName = new StringBuilder();
                                    //StringBuilder username = new StringBuilder();
                                    //StringBuilder prefix = new StringBuilder();
                                    //StringBuilder firstname = new StringBuilder();
                                    //StringBuilder middlename = new StringBuilder();
                                    //StringBuilder lastname = new StringBuilder();
                                    //StringBuilder suffix = new StringBuilder();
                                    //StringBuilder phyID = new StringBuilder();

                                    //if (phyItems.Attribute("username").Value != null)
                                    //    username.Append(phyItems.Attribute("username").Value);
                                    //if (phyItems.Attribute("prefix").Value != null)
                                    //    prefix.Append(phyItems.Attribute("prefix").Value);
                                    //if (phyItems.Attribute("firstname").Value != null)
                                    //    firstname.Append(phyItems.Attribute("firstname").Value);
                                    //if (phyItems.Attribute("middlename").Value != null)
                                    //    middlename.Append(phyItems.Attribute("middlename").Value);
                                    //if (phyItems.Attribute("lastname").Value != null)
                                    //    lastname.Append(phyItems.Attribute("lastname").Value);
                                    //if (phyItems.Attribute("suffix").Value != null)
                                    //    suffix.Append(phyItems.Attribute("suffix").Value);
                                    ////if (phyItems.Attribute("phyID").Value != null)
                                    //// phyID = phyItems.Attribute("phyID").Value;
                                    //if (phyItems.Attribute("ID").Value != null)
                                    //    phyID.Append(phyItems.Attribute("ID").Value);

                                    //if (prefix.Length != 0)
                                    //{
                                    //    phyName.Append(prefix.ToString());
                                    //}
                                    //if (firstname.Length != 0)
                                    //{
                                    //    phyName.Append(firstname.ToString());
                                    //}
                                    //if (middlename.Length != 0)
                                    //{
                                    //    phyName.Append(middlename.ToString());
                                    //}
                                    //if (lastname.Length != 0)
                                    //{
                                    //    phyName.Append(lastname.ToString());
                                    //}
                                    //if (suffix.Length != 0)
                                    //{
                                    //    phyName.Append(suffix.ToString());
                                    //}

                                    //Gitlab# 2485 - Physician Name Display Change
                                    string phyName = String.Empty;
                                    if (phyItems.lastname != String.Empty)
                                        phyName += phyItems.lastname;
                                    if (phyItems.firstname != String.Empty)
                                    {
                                        if (phyName != String.Empty)
                                            phyName += "," + phyItems.firstname;
                                        else
                                            phyName += phyItems.firstname;
                                    }
                                    if (phyItems.middlename != String.Empty)
                                        phyName += " " + phyItems.middlename;
                                    if (phyItems.suffix != String.Empty)
                                        phyName += "," + phyItems.suffix;

                                    liDropdown = new ListItem(phyName, phyItems.ID);
                                    if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] == phyItems.ID)
                                        liDropdown.Attributes.Add("default", "true");
                                    else
                                        liDropdown.Attributes.Add("default", "false");
                                    liDropdown.Attributes.CssStyle.Add("display", "none");
                                    liComboItems.Add(liDropdown);

                                }
                            }
                        }
                    }
                } 
            }
            IList<ListItem> sortlst = liComboItems.OrderByDescending(x => x.Attributes["default"]).ToList();
            sortlst = sortlst.OrderBy(x => x.Text).Distinct().ToList();
            cboOrderPhysician.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboOrderPhysician.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();
        }


        public void LoadLabCombo()
        {
            cboLab.Items.Clear();
            /* Code Block to populate Lab list */

            //XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));
            //ListItem liDropdown = null;
            //IList<ListItem> liComboItems = new List<ListItem>();
            //foreach (XElement elements in xmlDocumentType.Elements("LabList").Elements())
            //{
            //    //string username = elements.Attribute("name").Value;
            //    //string id = elements.Attribute("id").Value;
            //    //liDropdown = new ListItem(username, id);
            //    liDropdown = new ListItem(elements.Attribute("name").Value, elements.Attribute("id").Value);
            //    liComboItems.Add(liDropdown);
            //}

            //CAP-2773
            Lablist objlablist = new Lablist();
            objlablist = ConfigureBase<Lablist>.ReadJson("LabList.json");
            List<Labs> listLabList = new List<Labs>();
            listLabList = objlablist.Lab.ToList();
            ListItem liDropdown = null;
            IList<ListItem> liComboItems = new List<ListItem>();
            if (listLabList != null)
            {
                foreach (Labs objlab in listLabList)
                {
                    liDropdown = new ListItem(objlab.name, objlab.id);
                    liComboItems.Add(liDropdown);
                }
            }


            IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text).ToList();
            cboLab.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboLab.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();

        }

        //public void MoveToNextProcess_Validation()
        //{
        //    MoveToNextProcess();
        //}

        //public bool getIndexedPages()
        //{
        //    //string left_out_Page = string.Empty;
        //    StringBuilder left_out_Page = new StringBuilder();
        //    int totalPageCount = Convert.ToInt32(hdnPagecount.Value);
        //    scan_ID = (ulong)Session["ScanId"];
        //    ArrayList pageList = new ArrayList();
        //    ArrayList remainingList = new ArrayList();
        //    bool isAllPagesIndexed = true;
        //    Scan_IndexManager scanManager = new Scan_IndexManager();

        //    scanIndexList = (IList<scan_index>)Session["IndexList"];

        //    if (scanIndexList.Count > 0)
        //    {
        //        for (int i = 0; i < scanIndexList.Count; i++)
        //        {
        //            if (scanIndexList[i].Page_Selected.Split(',').Length > 0)
        //            {
        //                string[] primSplitPages = scanIndexList[i].Page_Selected.Split(',');
        //                foreach (string page in primSplitPages)
        //                {
        //                    if (page.Contains('-'))
        //                    {
        //                        string[] secSplitPages = page.Split('-');
        //                        for (int k = Convert.ToInt32(secSplitPages[0].ToString()); k <= Convert.ToInt32(secSplitPages[1].ToString()); k++)
        //                        {
        //                            pageList.Add(k);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        pageList.Add(Convert.ToInt32(page.ToString()));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                pageList.Add(Convert.ToInt32(scanIndexList[i].Page_Selected));
        //            }
        //        }
        //    }

        //    for (int i = 1; i <= totalPageCount; i++)
        //    {
        //        if (!pageList.Contains(i))
        //        {
        //            isAllPagesIndexed = false;
        //            remainingList.Add(i);
        //        }
        //    }

        //    if (remainingList.Count > 1)
        //    {
        //        for (int i = 0; i < remainingList.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                if (Convert.ToInt16(remainingList[i].ToString()) + 1 != Convert.ToInt16(remainingList[i + 1].ToString()))
        //                    left_out_Page.Append(remainingList[i].ToString() + ", ");
        //                else
        //                    left_out_Page.Append(remainingList[i].ToString() + " ~ ");
        //            }
        //            else if (i == remainingList.Count - 1)
        //            {
        //                left_out_Page.Append(remainingList[i].ToString());
        //            }
        //            else
        //            {
        //                if (Convert.ToInt16(remainingList[i].ToString()) + 1 != Convert.ToInt16(remainingList[i + 1].ToString()))
        //                {
        //                    left_out_Page.Append(remainingList[i].ToString() + ", ");


        //                    if ((i + 1) != remainingList.Count - 1)
        //                    {
        //                        if (Convert.ToInt16(remainingList[i + 1].ToString()) + 1 == Convert.ToInt16(remainingList[i + 2].ToString()))
        //                            left_out_Page.Append(remainingList[i + 1].ToString() + " ~ ");
        //                    }
        //                    else if ((i + 1) == remainingList.Count - 1)
        //                    {
        //                        left_out_Page.Append(remainingList[i + 1].ToString());
        //                        break;
        //                    }
        //                }
        //                else if (Convert.ToInt16(remainingList[i].ToString()) + 1 == Convert.ToInt16(remainingList[i + 1].ToString()))
        //                {
        //                    if (left_out_Page.ToString().EndsWith(", "))
        //                    {
        //                        left_out_Page.Append(remainingList[i].ToString() + " ~ ");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (remainingList.Count == 1)
        //    {
        //        left_out_Page.Append(remainingList[0].ToString());
        //    }

        //    if (left_out_Page.ToString().EndsWith(", ") || left_out_Page.ToString().EndsWith("~ "))
        //    {
        //        left_out_Page.ToString().Substring(0, left_out_Page.Length - 2);
        //    }
        //    return isAllPagesIndexed;
        //}

        public void MoveToNextProcess()
        {
            //sScan_Type = (string)Session["ScanType"];
            // string uri = string.Empty;
            IList<FileManagementIndex> fileManagementIndexList = new List<FileManagementIndex>();
            IList<WFObject> lstWF_object = new List<WFObject>();
            FileManagementIndexManager fileManagementIndexmanager = new FileManagementIndexManager();
            scanIndexList = new List<scan_index>();

            if (Session["IndexList"] != null)
                scanIndexList = (IList<scan_index>)Session["IndexList"];
            if (scanIndexList.Count == 0)
            {
                if (Session["ScanId"] != null)
                {
                    scanIndesmanager = new Scan_IndexManager();
                    scanIndexList = scanIndesmanager.GetScannedObjectScanID(Convert.ToUInt64(Session["ScanId"]));
                }
            }
            if (scanIndexList != null && scanIndexList.Count > 0)
            {
                scan_ID = scanIndexList[0].Scan_ID;
            }
            ulong[] uScanID = new ulong[scanIndexList.Count];
            string ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"];
            string serverPath = string.Empty;

            #region FTP Transfer
            FTPImageProcess _ftpImageProcess = new FTPImageProcess();

            //if (_ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, string.Empty, string.Empty))
            bool bCreateDirectory = _ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, string.Empty, string.Empty,out string sCheckFileNotFoundException);
            if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                return;
            }
            if (bCreateDirectory)
            {
                //DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/atala-capture-upload/" + Session.SessionID));
                //if (!directory.Exists)
                //{
                //    directory.Create();
                //}
                //string sDestinationPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["UNCPath"], ClientSession.HumanId.ToString());
                //DirectoryInfo directory = new DirectoryInfo(Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["UNCPath"], ClientSession.HumanId.ToString()));
                //if (!directory.Exists)
                //{
                //    directory.Create();
                //}

                for (int i = 0; i < scanIndexList.Count; i++)
                {
                    if (scanIndexList[i].Indexed_File_Path.Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NetworkError", "StopLoadOnUploadFile();alert('There is a temporary error occured. Please Login again and retry. If issue persists, Please contact Support.');", true);
                        return;
                    }
                    serverPath = _ftpImageProcess.UploadToImageServer(ClientSession.HumanId.ToString(), ftpServerIP, string.Empty, string.Empty, scanIndexList[i].Indexed_File_Path, string.Empty, out string sCheckFileNotFoundExceptions);
                    if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                        return;
                    }
                    //try
                    //{
                    //    //File Moved to Image Server without Ftp.
                    //    File.Copy(scanIndexList[i].Indexed_File_Path, Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["UNCPath"], ClientSession.HumanId.ToString()) + "\\");
                    //    serverPath = Path.Combine(Path.Combine(ConfigurationManager.AppSettings["ftpServerIP"], ClientSession.HumanId.ToString()), Path.GetFileName(scanIndexList[i].Indexed_File_Path));
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    if (serverPath != string.Empty)
                    {
                        FileManagementIndex filemanagementIndex = new FileManagementIndex();
                        filemanagementIndex.Created_By = scanIndexList[i].Created_By;
                        filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        filemanagementIndex.Document_Date = scanIndexList[i].Document_Date;
                        filemanagementIndex.Document_Type = scanIndexList[i].Document_Type;
                        filemanagementIndex.Document_Sub_Type = scanIndexList[i].Document_Sub_Type;
                        filemanagementIndex.Source = "SCAN";
                        filemanagementIndex.Human_ID = scanIndexList[i].Human_ID;
                        filemanagementIndex.Order_ID = scanIndexList[i].Order_ID;
                        filemanagementIndex.Scan_Index_Conversion_ID = scanIndexList[i].Id;
                        filemanagementIndex.File_Path = serverPath;
                        filemanagementIndex.Encounter_ID = scanIndexList[i].Encounter_ID;
                        filemanagementIndex.Is_Delete = "N";
                        uScanID[i] = scanIndexList[i].Scan_ID;
                        if (scanIndexList[i].Document_Sub_Type.Trim().ToUpper() == "ADVANCE DIRECTIVE" || scanIndexList[i].Document_Sub_Type.Trim().ToUpper() == "BIRTH PLAN")
                        {
                            // string filedirectorypath = _ftpImageProcess.UploadToADfiletoServerIndexing(ClientSession.HumanId.ToString(), scanIndexList[i].Indexed_File_Path);
                            //filemanagementIndex.Generate_Link_File_Path = filedirectorypath;
                            filemanagementIndex.Generate_Link_File_Path = _ftpImageProcess.UploadToADfiletoServerIndexing(ClientSession.HumanId.ToString(), scanIndexList[i].Indexed_File_Path,this,out string sCheckFileNotFoundExce);
                            if (sCheckFileNotFoundExce != "" && sCheckFileNotFoundExce.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExce.Split('~')[1] + "\");", true);
                                return;
                            }
                        }
                        fileManagementIndexList.Add(filemanagementIndex);

                    }
                    else
                    {
                        ArrayList templist = new ArrayList();
                        templist.Add(scanIndexList[i].Indexed_File_Path.Substring(scanIndexList[i].Indexed_File_Path.LastIndexOf("\\") + 1));
                    }
                }
            }
            #endregion

            bool IsOrder = false;
            if (scanIndexList != null && scanIndexList.Count > 0)
            {
                var IscheckOrder = scanIndexList.Where(aa => aa.Order_ID != 0).ToList();
                if (IscheckOrder.Count > 0)
                {
                    IsOrder = true;
                }
            }


            // fileManagementIndexmanager.SaveUpdateDeleteFileManagementIndexForOnline_and_Wfobject(fileManagementIndexList.ToArray(), scan_ID, string.Empty, UtilityManager.ConvertToUniversal());

            fileManagementIndexmanager.SaveUpdateDeleteFileManagementIndexForOnline_and_Wfobject(fileManagementIndexList.ToArray(), uScanID, string.Empty, UtilityManager.ConvertToUniversal());//, scanIndexList, (IDictionary<ulong, string>)Session["usernameIDMap"]);
            //for Bug id :65117 

            if (IsOrder == true)
            {
                if (Session["usernameIDMap"] == null)
                {
                    updateOrderObjects();
                }
            }
            for (int j = 0; j < scanIndexList.Count; j++)
            {
                if (Convert.ToUInt64(scanIndexList[j].Order_ID) > 0)
                {
                    //if (Session["usernameIDMap"] == null)
                    //{
                    //    updateOrderObjects();
                    //}
                    //string Physician_Name = "0";
                    StringBuilder Physician_Name = new StringBuilder();
                    Physician_Name.Append("0");
                    //if (scanIndexList[j].Physician_ID.ToString() != "0")
                    //{
                    //    Physician_Name = new StringBuilder();
                    //    Physician_Name.Append(((IDictionary<ulong, string>)Session["usernameIDMap"])[scanIndexList[j].Physician_ID].ToString());
                    //}
                    if (scanIndexList[j].Appointment_Provider_ID.ToString() != "0")
                    {
                        Physician_Name = new StringBuilder();
                        Physician_Name.Append(((IDictionary<ulong, string>)Session["usernameIDMap"])[scanIndexList[j].Appointment_Provider_ID].ToString());
                    }
                    //For Bug Id:69702 Some case current owner is blank so fix this bug.
                    if (Physician_Name.ToString().Trim() == "" || Physician_Name.ToString().Trim() == "0")
                    {
                        OrdersSubmitManager objOrdersubmit = new OrdersSubmitManager();
                        ulong uPhyId = objOrdersubmit.GetOrderingPhysicianIdByOrderSubmitID(scanIndexList[j].Order_ID);

                        Physician_Name = new StringBuilder();
                        Physician_Name.Append(((IDictionary<ulong, string>)Session["usernameIDMap"])[uPhyId]);
                    }
                    //MoveToNextWorkFlow(scanIndexList[j].Order_ID, "DIAGNOSTIC ORDER", physicianName);
                    string[] current_process = new string[] { "ORDER_GENERATE" };
                    WFObjectManager WFProxy = new WFObjectManager();
                    //cap - 571
                    //WFProxy.MoveToNextProcess(scanIndexList[j].Order_ID, "DIAGNOSTIC ORDER", 8, Physician_Name.ToString(), UtilityManager.ConvertToUniversal(DateTime.Now), null, current_process, null);
                    if (scanIndexList[j].Is_Manually_Reviewed_And_Signed == "Y")
                        //Cap - 1250
                        //WFProxy.MoveToNextProcess(scanIndexList[j].Order_ID, "DIAGNOSTIC ORDER", 9, Physician_Name.ToString(), UtilityManager.ConvertToUniversal(DateTime.Now), null, current_process, null);
                        WFProxy.MoveToNextProcess(scanIndexList[j].Order_ID, "DIAGNOSTIC ORDER", 9, "UNKNOWN", UtilityManager.ConvertToUniversal(DateTime.Now), null, current_process, null);
                    else
                    {
                        WFProxy.MoveToNextProcess(scanIndexList[j].Order_ID, "DIAGNOSTIC ORDER", 8, Physician_Name.ToString(), UtilityManager.ConvertToUniversal(DateTime.Now), null, current_process, null);
                    }
                }
            }

            btnMoveToNextProcess.Disabled = true;
            #region "Scanned File Trashing"
            //Jira #CAP-39
            int iTryCount = 1;
        TryAgain:
            try
            {
                if (Session["IndexList"] != null)
                {
                    IList<scan_index> deleteIndexList = (IList<scan_index>)Session["IndexList"];
                    //string temp_name = string.Empty;
                    StringBuilder temp_name = new StringBuilder();
                    if (Session["ImagePath"] != null)
                        temp_name.Append((string)Session["ImagePath"]);
                    //Remove the indexed images from human_id folder
                    if (deleteIndexList.Count > 0)
                    {
                        bigImagePDF.Attributes.Add("src", "");

                        for (int j = 0; j < deleteIndexList.Count; j++)
                        {

                            DirectoryInfo childDir = new DirectoryInfo(new FileInfo(deleteIndexList[j].Indexed_File_Path).DirectoryName);
                            if (childDir.GetFiles().Count() > 0 && childDir.GetFiles().Length == 0)
                            {
                                try
                                {
                                    Directory.Delete(childDir.FullName, true);
                                }
                                catch (Exception ex)
                                {
                                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "frmIndexing Line No - 2430 - DeletePath - " + childDir.FullName + " - " + ex.Message, DateTime.Now, "0", "frmimageviewer");
                                }
                            }
                            else
                            {
                                if (File.Exists(deleteIndexList[j].Indexed_File_Path) == true)
                                {
                                    try
                                    {
                                        File.Delete(deleteIndexList[j].Indexed_File_Path);
                                    }
                                    catch (Exception ex)
                                    {
                                        UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "frmIndexing Line No - 2443 - DeletePath - " + deleteIndexList[j].Indexed_File_Path + " - " + ex.Message, DateTime.Now, "0", "frmimageviewer");
                                    }
                                }
                            }
                        }

                        try
                        {
                            if (temp_name.Length != 0)
                                File.Delete(temp_name.ToString());
                        }
                        catch (Exception ex)
                        {
                            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "frmIndexing Line No - 2456 - DeletePathTemp - " + temp_name.ToString() + " - " + ex.Message, DateTime.Now, "0", "frmimageviewer");
                        }
                    }
                }
                if (Session["BrowseLoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Count > 0 && (string)Session["FileName"] != null)
                {
                    file_name = new StringBuilder();
                    file_name.Append((string)Session["FileName"]);
                    IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Where(a => a.Scanned_File_Name == file_name.ToString()).ToList<Scan>();
                    if (lstTemp != null && lstTemp.Count > 0)
                    {
                        foreach (Scan item in lstTemp)
                        {
                            try
                            {
                                if (File.Exists(item.Scanned_File_Path) == true)
                                {
                                    File.Delete(item.Scanned_File_Path);
                                }
                            }
                            catch
                            {
                                if (File.Exists(item.Scanned_File_Path) == true)
                                {
                                    DirectoryInfo dirt = new DirectoryInfo(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\Waiting_For_Delete");
                                    try
                                    {
                                        if (!dirt.Exists)
                                        {
                                            dirt.Create();
                                        }
                                        File.Move(item.Scanned_File_Path, ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\Waiting_For_Delete\\" + Path.GetFileName(item.Scanned_File_Path));
                                    }
                                    catch (Exception ex)
                                    {
                                        UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "frmIndexing Line No - 2486 - MovePathSource - " + item.Scanned_File_Path + " - Destination - " + ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\Waiting_For_Delete" + " - " + ex.Message, DateTime.Now, "0", "frmimageviewer");
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                string sErrorMessage = "";
                if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                }
                else
                {
                    //Jira #CAP-39
                    if (iTryCount <= 3)
                    {
                        iTryCount = iTryCount + 1;
                        Thread.Sleep(1500);
                        goto TryAgain;
                    }
                    else
                    {
                        UtilityManager.RetryExecptionLog(ex, iTryCount);
                        throw ex;
                    }
                }
            }


            #endregion

            divLoading.Style.Add("display", "none");
            waitCursor.Update();
            //Remove the uploaded files from session //This session used for patient portal also.
            if (Session["BrowseLoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Count > 0 && (string)Session["FileName"] != null)
            {
                file_name = new StringBuilder();
                file_name.Append((string)Session["FileName"]);
                IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Where(a => a.Scanned_File_Name == file_name.ToString()).ToList<Scan>();
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    foreach (Scan item in lstTemp)
                    {
                        ((IList<Scan>)Session["BrowseLoadList"]).Remove(item);

                    }
                    // Session["BrowseFileNames"] = lstTemp.Distinct().ToList();
                }
            }

            if (HttpContext.Current.Session["LocalUploadFiles"] != null && ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Count > 0 && file_name.ToString() != "")//(string)Session["LastIndexFileName"] != null)
            {
                IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Where(a => a.Scanned_File_Name == file_name.ToString()).ToList<Scan>();
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    foreach (Scan item in lstTemp)
                    {
                        ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Remove(item);
                    }
                }
                HttpContext.Current.Session["LocalUploadFiles"] = lstScanList = (IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"];
            }

            if (Session["LoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["LoadList"]).Count > 0 && (string)Session["FileName"] != null)
            {
                file_name = new StringBuilder();
                file_name.Append((string)Session["FileName"]);
                IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["LoadList"]).Where(a => a.Scanned_File_Name == file_name.ToString()).ToList<Scan>();
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    foreach (Scan item in lstTemp)
                    {
                        ((IList<Scan>)Session["LoadList"]).Remove(item);

                    }
                }
            }


            //Remove the screen CLose and show only error message
            // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DisplayAlert", "DisplayErrorMessage('115018');", true);

            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "CloseAndDisplayAlert", "CloseAndDisplayAlert();", true);
        }

        public bool MoveToNextWorkFlow(ulong Order_Submit_ID, string Obj_Type, string Physician_Name)
        {
            string[] current_process = new string[] { "ORDER_GENERATE" };
            bool result = true;
            WFObjectManager WFProxy = new WFObjectManager();
            WFObjectManager TempWF = new WFObjectManager();

            try
            {
                WFProxy.MoveToNextProcess(Order_Submit_ID, Obj_Type, 8, Physician_Name, UtilityManager.ConvertToUniversal(DateTime.Now), null, current_process, null);
            }
            catch
            {
                result = false;
            }


            return result;
        }

        private void AddToArrayList(int fromPage, int toPage)
        {
            if (selectedPageNumbers.Count == 0)
            {
                for (int i = fromPage; i <= toPage; i++)
                {
                    selectedPageNumbers.Add(i);
                    TotalPagesSelected.Add(i);
                }
            }
            else
            {
                for (int i = fromPage; i <= toPage; i++)
                {
                    var root = from pastList in selectedPageNumbers
                               where pastList == i
                               select pastList;
                    if (root.Count() == 0)
                    {
                        selectedPageNumbers.Add(i);
                    }
                    root = from pastList in TotalPagesSelected
                           where pastList == i
                           select pastList;
                    if (root.Count() == 0)
                    {
                        TotalPagesSelected.Add(i);
                    }
                }
            }
            Session["SelectedPages"] = selectedPageNumbers;
        }

        private void AddToArrayList(int currentPage)
        {
            var root = from pastList in selectedPageNumbers
                       where Convert.ToInt32(pastList) == Convert.ToInt32(currentPage)
                       select pastList;

            if (root.Count() == 0)
            {
                selectedPageNumbers.Add(currentPage);
            }
            Session["SelectedPages"] = selectedPageNumbers;
        }

        public void HideOrders()
        {
            OrdersPanel.Enabled = false;
            dtpSpecCollection.Disabled = true;
            btnSearchCpt.Disabled = true;
            chkShowAll.Checked = false;
            chkOrderingPhyShowAll.Checked = false;

            dtpSpecCollection.Value = "";
            spanoutstandorder.Attributes.Remove("class");
            spanoutstandorder.Attributes.Add("class", "spanstyle");
            spanorderstar.Visible = false;
            Labspan.Attributes.Remove("class");
            Labspan.Attributes.Add("class", "spanstyle");
            slabMandatory.Visible = false;
            spanphy.Attributes.Remove("class");
            spanphy.Attributes.Add("class", "spanstyle");
            spanphystar.Visible = false;

            //Accordion wnd
            dOrder.Attributes.Remove("data-target");
            dOrder.Attributes.Remove("class");
            dOrder.Attributes.Add("class", "panel-headingdisable LabelStyle");

            dOrderCollapse.Attributes.Remove("class");
            dOrderCollapse.Attributes.Add("class", "panelborderboxIndexing panel-collapse collapse");
        }

        public void VisibleOrders()
        {
            OrdersPanel.Enabled = true;
            dtpSpecCollection.Disabled = false;
            btnSearchCpt.Disabled = false;
            chkShowAll.Checked = false;
            chkOrderingPhyShowAll.Checked = false;

            //Accordion wnd
            dOrder.Attributes.Add("data-target", "#dOrderCollapse");
            dOrder.Attributes.Remove("class");
            dOrder.Attributes.Add("class", "panel-headingIndexing LabelStyle");

            dOrderCollapse.Attributes.Remove("class");
            dOrderCollapse.Attributes.Add("class", "panelborderboxIndexing panel-collapse collapse in");

        }

        public void LoadDocumentSubType(string Item)
        {
            IList<StaticLookup> docSublist = new List<StaticLookup>();
            if (Session["DocSubList"] != null)
            {
                docSublist = (IList<StaticLookup>)Session["DocSubList"];
                IList<StaticLookup> userLookup = (from doc in docSublist where doc.Field_Name.ToUpper() == Item.ToUpper() select doc).ToList<StaticLookup>();
                cboDocumentSubType.Items.Clear();

                DataTable dt = new DataTable();
                DataColumn col1 = new DataColumn("Text", typeof(string));
                dt.Columns.Add(col1);
                DataColumn col2 = new DataColumn("Value", typeof(string));
                dt.Columns.Add(col2);
                for (int j = 0; j < userLookup.Count; j++)
                {

                    DataRow dr = dt.NewRow();
                    dr["Text"] = userLookup[j].Value;
                    dr["Value"] = userLookup[j].Value;
                    dt.Rows.Add(dr);
                }

                cboDocumentSubType.DataValueField = "Value";
                cboDocumentSubType.DataTextField = "Text";
                cboDocumentSubType.DataSource = dt;
                cboDocumentSubType.DataBind();
                cboDocumentSubType.Items.Insert(0, "");

                IList<StaticLookup> defaultItemSelected = (from doc in docSublist where doc.Field_Name.ToUpper() == Item.ToUpper() && doc.Default_Value.ToLower() != "" select doc).ToList<StaticLookup>();
                if (defaultItemSelected.Count() > 0)
                {
                    // string xmlDefault = defaultItemSelected[0].Default_Value;
                    if (cboDocumentSubType.Items != null && cboDocumentSubType.Items.Count > 0)
                    {
                        if (cboDocumentSubType.Items.FindByText(defaultItemSelected[0].Default_Value) != null)
                        {
                            if (cboDocumentSubType.SelectedIndex > -1)
                            {
                                cboDocumentSubType.ClearSelection();
                            }
                            cboDocumentSubType.Items.FindByText(defaultItemSelected[0].Default_Value).Selected = true;
                        }
                    }
                }
                else
                {
                    if (cboDocumentSubType.Items != null && cboDocumentSubType.Items.Count > 0)
                    {
                        if (cboDocumentSubType.SelectedIndex > -1)
                        {
                            cboDocumentSubType.ClearSelection();
                        }
                        cboDocumentSubType.SelectedIndex = 0;
                    }
                }

                cboDocumentSubType.Enabled = true;


            }
        }

        public int getPageCount(string fileName)
        {
            int pageCount = 0;

            System.Drawing.Image Tiff = System.Drawing.Image.FromFile(fileName);
            pageCount = Tiff.GetFrameCount(FrameDimension.Page);
            return pageCount;
        }

        private void settingTheObject(ref scan_index scanIndexObject, string action, ulong Order_ID)
        {
            //string drt_path = string.Empty;
            StringBuilder drt_path = new StringBuilder(); ;
            //string sourceFile = hdnsourceFile.Value;//(string)Session["ImagePath"];
            StringBuilder sourceFile = new StringBuilder();
            sourceFile.Append(hdnsourceFile.Value);
            if (sourceFile != null && sourceFile.Length != 0)
            {
                if (rdbRemoteDir.Checked == true)
                {
                    if (ddlSourceType.Text.ToUpper().Trim() == "SCAN")
                    {
                        drt_path.Append(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ddSelectedFacility.Text + "\\Indexed_Images\\" + ClientSession.HumanId.ToString());
                    }
                    else
                    {
                        if (ddSelectedFacility.Text.Contains('~'))
                        {
                            drt_path.Append(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ddSelectedFacility.Text.Split('~')[0].Trim() + "\\Indexed_Images\\" + ClientSession.HumanId.ToString());
                            // drt_path = ConfigurationManager.AppSettings["ScanningPath_Fax"] + "\\" + ConfigurationManager.AppSettings["ftpFaxpath"] + "\\" + ddSelectedFacility.Text.Split('~')[0].Trim() + "\\Indexed_Images\\" + ClientSession.HumanId.ToString();
                        }
                        else
                        {
                            drt_path.Append(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ddSelectedFacility.Text + "\\Indexed_Images\\" + ClientSession.HumanId.ToString());
                            //drt_path = ConfigurationManager.AppSettings["ScanningPath_Fax"] + "\\" + ConfigurationManager.AppSettings["ftpFaxpath"] + "\\" + ddSelectedFacility.Text + "\\Indexed_Images\\" + ClientSession.HumanId.ToString();

                        }
                    }
                }
                else
                {
                    drt_path.Append(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ClientSession.FacilityName + "\\Indexed_Images\\" + ClientSession.HumanId.ToString());

                }
                DirectoryInfo dirt = new DirectoryInfo(drt_path.ToString());
                if (!dirt.Exists)
                {
                    try
                    {
                        dirt.Create();
                    }
                    catch (Exception ex)
                    {
                        if (ex is IOException)
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NetworkError", "alert('Problem in Saving Files to Clinical NAS. Please Contact Support.');", true);
                            return;
                        }
                    }
                }
                string lastNumToAdd = string.Empty;
                int prevNum = 0;
                if (prevNum == 0 && hdnNo.Value == string.Empty)
                {
                    IList<scan_index> scan_index_lst = new List<scan_index>();
                    scan_index_lst = (IList<scan_index>)Session["IndexList"];
                    if (scan_index_lst != null && scan_index_lst.Count > 0)
                    {
                        int[] sortIndexNum = new int[scan_index_lst.Count];
                        for (int i = 0; i < scan_index_lst.Count; i++)//foreach (FileInfo fi in fileInfo)
                        {
                            if (scan_index_lst[i].Indexed_File_Path != null && scan_index_lst[i].Indexed_File_Path.Trim() != string.Empty && scan_index_lst[i].Indexed_File_Path.Trim() != "")
                            {
                                //string sfile_name = Path.GetFileName(scan_index_lst[i].Indexed_File_Path);
                                StringBuilder sfile_name = new StringBuilder();
                                sfile_name.Append(Path.GetFileName(scan_index_lst[i].Indexed_File_Path));
                                if (sfile_name.ToString().Contains('_') && sfile_name.ToString().Contains('.'))
                                {
                                    prevNum = Convert.ToInt32(sfile_name.ToString().Substring(sfile_name.ToString().LastIndexOf("_") + 1, (sfile_name.ToString().LastIndexOf(".") - 1) - sfile_name.ToString().LastIndexOf("_")));
                                    sortIndexNum[i] = prevNum;
                                }
                            }
                        }
                        prevNum = sortIndexNum.Max();
                        hdnNo.Value = Convert.ToString(prevNum + 1);
                        lastNumToAdd = Convert.ToString(prevNum + 1);
                        if (lastNumToAdd.Length == 1)
                            lastNumToAdd = "0" + lastNumToAdd;

                    }
                    else
                    {
                        Scan_IndexManager objScanIndexMngr = new Scan_IndexManager();
                        IList<scan_index> ilstScanindex = new List<scan_index>();
                        ilstScanindex = objScanIndexMngr.GetScanIndexForHuman(ClientSession.HumanId);
                        if (ilstScanindex != null && ilstScanindex.Count > 0)
                        {
                            int[] sortIndexNum = new int[ilstScanindex.Count];
                            for (int i = 0; i < ilstScanindex.Count; i++)
                            {
                                if (ilstScanindex[i].Indexed_File_Path != null && ilstScanindex[i].Indexed_File_Path.Trim() != string.Empty && ilstScanindex[i].Indexed_File_Path.Trim() != "")
                                {
                                    //string sfile_name = Path.GetFileName(ilstScanindex[i].Indexed_File_Path);
                                    StringBuilder sfile_name = new StringBuilder();
                                    sfile_name.Append(Path.GetFileName(ilstScanindex[i].Indexed_File_Path));

                                    if (sfile_name.ToString().Contains('_') && sfile_name.ToString().Contains('.'))
                                    {
                                        prevNum = Convert.ToInt32(sfile_name.ToString().Substring(sfile_name.ToString().LastIndexOf("_") + 1, (sfile_name.ToString().LastIndexOf(".") - 1) - sfile_name.ToString().LastIndexOf("_")));
                                        sortIndexNum[i] = prevNum;
                                    }
                                }
                            }
                            prevNum = sortIndexNum.Max();
                            hdnNo.Value = Convert.ToString(prevNum + 1);
                            lastNumToAdd = Convert.ToString(prevNum + 1);
                            if (lastNumToAdd.Length == 1)
                                lastNumToAdd = "0" + lastNumToAdd;
                        }
                        else
                        {
                            hdnNo.Value = 1.ToString();
                            lastNumToAdd = hdnNo.Value;
                            if (lastNumToAdd.Length == 1)
                                lastNumToAdd = "0" + lastNumToAdd;

                        }
                    }

                }
                else
                {
                    hdnNo.Value = (Convert.ToInt32(hdnNo.Value) + 1).ToString();
                    lastNumToAdd = hdnNo.Value;
                    if (lastNumToAdd.Length == 1)
                        lastNumToAdd = "0" + lastNumToAdd;
                }

                //if (rdbRemoteDir.Checked == true)
                //{
                //    if (ddSelectedFacility.Text.Contains('~'))
                //    {
                //        filePath = ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ddSelectedFacility.Text.Split('~')[0].Trim() + "\\Indexed_Images\\" + ClientSession.HumanId.ToString() + "\\" + ddSelectedFacility.Text.Replace("#", "").Replace(",", "_") + "_" + "ONLINE" + "_" + ClientSession.UserName + "_" + Convert.ToDateTime(dtpDocumentDate.Value).ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(sourceFile);
                //    }
                //    else
                //        filePath = ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ddSelectedFacility.Text + "\\Indexed_Images\\" + ClientSession.HumanId.ToString() + "\\" + ddSelectedFacility.Text.Replace("#", "").Replace(",", "_") + "_" + "OLD" + "_" + ClientSession.UserName + "_" + Convert.ToDateTime(dtpDocumentDate.Value).ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(sourceFile);
                //}
                //else
                //{
                //    filePath = ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + ClientSession.FacilityName + "\\Indexed_Images\\" + ClientSession.HumanId.ToString() + "\\" + ddSelectedFacility.Text.Replace("#", "").Replace(",", "_") + "_" + "OLD" + "_" + ClientSession.UserName + "_" + Convert.ToDateTime(dtpDocumentDate.Value).ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(sourceFile);
                //}

                filePath = new StringBuilder();
                if (rdbRemoteDir.Checked == true)
                    filePath.Append(Path.Combine(drt_path.ToString(), ddSelectedFacility.Text.Replace("#", "").Replace(",", "_") + "_" + "ONLINE" + "_" + ClientSession.UserName + "_" + Convert.ToDateTime(dtpDocumentDate.Value).ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(sourceFile.ToString()).ToLower()));
                else
                    filePath.Append(Path.Combine(drt_path.ToString(), ClientSession.FacilityName.Replace("#", "").Replace(",", "_") + "_" + "ONLINE" + "_" + ClientSession.UserName + "_" + Convert.ToDateTime(dtpDocumentDate.Value).ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(sourceFile.ToString()).ToLower()));

                //Extract pages
                if (Session["SelectedPages"] != null && (sourceFile.ToString().ToLower().Contains(".tif") || sourceFile.ToString().ToLower().Contains(".tiff")))
                {
                    selectedPageNumbers = (IList<int>)Session["SelectedPages"];
                    System.Drawing.Image[] splittedFiles = null;
                    splittedFiles = splitTiffImageToSeparateImages(sourceFile.ToString(), selectedPageNumbers);
                    //CAP-935
                    bool res = saveMultipage(splittedFiles, filePath.ToString(), "TIFF",out string sCheckFileNotFoundException);

                    if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                        return;
                    }   

                    if (res == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NetworkError", "alert('Problem in Saving Files to Clinical NAS. Please Contact Support.');", true);
                        return;
                    }
                }
                else if (sourceFile.ToString().ToLower().Contains(".pdf"))
                {
                    selectedPageNumbers = (IList<int>)Session["SelectedPages"];
                    ExtractPDFPage(sourceFile.ToString(), filePath.ToString(), selectedPageNumbers);

                }
                else
                {
                    int iTryCount = 1;
                TryAgain:
                    try
                    {
                        System.IO.File.Copy(sourceFile.ToString(), filePath.ToString(), true);
                    }
                    catch (Exception ex)
                    {
                        string sErrorMessage = "";
                        if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                        }
                        else
                        {
                            //Jira #CAP-39
                            if (iTryCount <= 3)
                            {
                                iTryCount = iTryCount + 1;
                                Console.WriteLine(ex.Message);
                                Thread.Sleep(1500);
                                goto TryAgain;
                            }
                            else { UtilityManager.RetryExecptionLog(ex, iTryCount); throw (ex); }
                        }
                    }
                }

                if (Session["ScanId"] != null)
                    scan_ID = (ulong)Session["ScanId"];
                if (action == "Add")
                {
                    scanIndexObject.Human_ID = ClientSession.HumanId;
                    scanIndexObject.Scan_ID = scan_ID;
                }
                scanIndexObject.Indexed_File_Path = filePath.ToString();
                scanIndexObject.Page_Selected = txtSelectedPages.Value;
                scanIndexObject.Document_Type = cboDocumentType.SelectedItem.Text;
                scanIndexObject.Document_Sub_Type = cboDocumentSubType.Text;
                //Cap - 571
                if (chkReviewandSign.Checked)
                    scanIndexObject.Is_Manually_Reviewed_And_Signed = "Y";
                else
                    scanIndexObject.Is_Manually_Reviewed_And_Signed = "N";
                //CAP-2847
                if (chkExternalMedicalRecord.Checked)
                    scanIndexObject.Is_External_Medical_Record = "Y";
                else
                    scanIndexObject.Is_External_Medical_Record = "N";
                if (cboDocumentType.SelectedValue.ToUpper() != "ENCOUNTERS")
                    scanIndexObject.Document_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpDocumentDate.Value + " 07:30:00"));// + DateTime.Now.TimeOfDay));
                else
                {
                    scanIndexObject.Document_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(ddEncPhyName.SelectedItem.Text.Split('~')[0].Split(' ')[0].Replace(" ", "") + " 07:30:00"));// + DateTime.Now.TimeOfDay));
                    scanIndexObject.Encounter_ID = Convert.ToUInt64(ddEncPhyName.SelectedItem.Value);
                }

                if (cboIs_Interperated.SelectedItem.Text != string.Empty)
                {
                    scanIndexObject.Is_Narrative_Interpretation = cboIs_Interperated.SelectedValue.ToString();
                }

                if (action == "Update")
                {
                    //Cap - 571
                    if (chkReviewandSign.Checked)
                        scanIndexObject.Is_Manually_Reviewed_And_Signed = "Y";
                    else
                        scanIndexObject.Is_Manually_Reviewed_And_Signed = "N";
                    //CAP-2847
                    if (chkExternalMedicalRecord.Checked)
                        scanIndexObject.Is_External_Medical_Record = "Y";
                    else
                        scanIndexObject.Is_External_Medical_Record = "N";
                    scanIndexObject.Modified_By = ClientSession.UserName;
                    scanIndexObject.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    scanIndexObject.Created_By = ClientSession.UserName;
                    scanIndexObject.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                scanIndexObject.Order_ID = Order_ID;
                if (Order_ID != 0)
                {
                    scanIndexObject.Physician_ID = Convert.ToUInt32(cboPhysician.SelectedValue);
                    //Save ReviewPhysican Id
                    scanIndexObject.Appointment_Provider_ID = Convert.ToUInt32(cboPhysician.SelectedValue);
                }
            }
        }

        private void LoadGridView(IList<scan_index> objectToLoad_data)
        {

            if (objectToLoad_data != null)
            {
                IList<scan_index> objectToLoad = objectToLoad_data.Select(a => { if (a.Modified_Date_And_Time == DateTime.MinValue) a.Modified_Date_And_Time = a.Created_Date_And_Time; else { a.Modified_Date_And_Time = a.Modified_Date_And_Time; } return a; }).OrderByDescending(a => a.Modified_Date_And_Time).ToList<scan_index>();

                grdIndexing.DataSource = null;
                grdIndexing.DataBind();

                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Document Sub Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Document Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Patient Name", typeof(string)));
                dt.Columns.Add(new DataColumn("File Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Selected Pages", typeof(string)));
                dt.Columns.Add(new DataColumn("Scan Index Id", typeof(string)));
                dt.Columns.Add(new DataColumn("Order", typeof(string)));
                dt.Columns.Add(new DataColumn("Order Submit Id", typeof(string)));
                dt.Columns.Add(new DataColumn("FilePath", typeof(string)));

                for (int i = 0; i < objectToLoad.Count; i++)
                {

                    dr = dt.NewRow();
                    dr["Document Sub Type"] = objectToLoad[i].Document_Sub_Type;
                    dr["Document Date"] = UtilityManager.ConvertToLocal(objectToLoad[i].Document_Date).ToString("dd-MMM-yyyy");
                    if (objectToLoad[i].Human_ID != 0)
                    {
                        dr["Patient Name"] = HumanDetails(objectToLoad[i].Human_ID.ToString());
                    }
                    else
                    {
                        dr["Patient Name"] = "";

                    }
                    dr["File Name"] = Path.GetFileName(objectToLoad[i].Indexed_File_Path);
                    dr["Selected Pages"] = objectToLoad[i].Page_Selected;
                    dr["Scan Index Id"] = objectToLoad[i].Id.ToString();
                    dr["Order"] = objectToLoad[i].Object_Type.ToString();
                    dr["Order Submit Id"] = objectToLoad[i].Order_ID.ToString();
                    dr["FilePath"] = objectToLoad[i].Indexed_File_Path;
                    dt.Rows.Add(dr);
                }

                grdIndexing.DataSource = dt;
                grdIndexing.DataBind();
                btnMoveToNextProcess.Disabled = false;
            }

            else
            {
                grdIndexing.DataSource = new string[] { };
                grdIndexing.DataBind();
                btnMoveToNextProcess.Disabled = true;
            }
            updateControlButtons.Update();
        }

        private void setThePatientDetails(Scan_IndexDTO scan)
        {
            if (scan != null)
            {
                PatientDetails.Text = scan.Last_Name + ", " + scan.MI + " " + scan.First_Name + "|" + scan.Birth_Date.ToString("dd-MMM-yyyy") + "|" + scan.Sex + " |Acc #:" + scan.ScanIndexList[0].Human_ID;
                PatientDetails.Attributes.Add("class", "patientPaneDisabled"); PatientDetails.CssClass = "patientPaneDisabled";
                hdnHumanID.Value = scan.ScanIndexList[0].Human_ID.ToString();
                cboDocumentType.Enabled = true;
                btnMoveToNextProcess.Disabled = false;
            }

        }

        private void setThePatientDetails(ulong humanID)
        {
            if (humanID != 0)
            {
                string sdivPatientstrip = UtilityManager.FillPatientStrip(humanID);
                if (sdivPatientstrip != null)
                {
                    PatientDetails.Text = sdivPatientstrip;
                    PatientDetails.Attributes.Remove("class");
                    PatientDetails.Attributes.Add("class", "patientPaneDisabled"); PatientDetails.CssClass = "patientPaneDisabled";
                    cboDocumentType.Enabled = true;
                    hdnHumanID.Value = humanID.ToString();
                }

                //string FileName = "Human" + "_" + humanID + ".xml";
                //string strXmlFilePath = Path.Combine(ConfigurationManager.AppSettings["XMLPath"], "Human" + "_" + humanID + ".xml");
                //if (File.Exists(Path.Combine(ConfigurationManager.AppSettings["XMLPath"], "Human" + "_" + humanID + ".xml")) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(Path.Combine(ConfigurationManager.AppSettings["XMLPath"], "Human" + "_" + humanID + ".xml"));
                //    try
                //    {
                //        XmlNodeList xmlTagName = null;
                //        itemDoc.Load(XmlText);
                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

                //            if (xmlTagName.Count > 0)
                //            {
                //                PatientDetails.Text = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value + ", " + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value + " " + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("MI").Value + "|" + Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("dd-MMM-yyyy") + "|" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Sex").Value + " |Acc #:" + humanID;
                //                PatientDetails.Attributes.Remove("class");
                //                PatientDetails.Attributes.Add("class", "patientPaneDisabled"); PatientDetails.CssClass = "patientPaneDisabled";
                //                cboDocumentType.Enabled = true;
                //                hdnHumanID.Value = humanID.ToString();
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        XmlText.Close();
                //        //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

                //        throw ex;
                //    }
                //}
            }
        }
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
        public void LoadDocType()
        {
            //cboDocumentType.Items.Clear();
            //IList<StaticLookup> docSublist = new List<StaticLookup>();
            //XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\Doctype.xml"));
            //StaticLookup objStatics = null;
            //ListItem liDropdown = null;
            ////string DefDoctype = string.Empty;
            //StringBuilder DefDoctype = new StringBuilder();
            //foreach (XElement elements in xmlDocumentType.Descendants("DocElement"))
            //{
            //    //string xmlValue = elements.Attribute("name").Value;
            //    if (elements.Attribute("name").Value.ToUpper() == "RESULTS")
            //    {
            //        liDropdown = new ListItem(elements.Attribute("name").Value, "DIAGNOSTIC ORDER");
            //    }
            //    else
            //    {
            //        liDropdown = new ListItem(elements.Attribute("name").Value, elements.Attribute("name").Value);
            //    }
            //    cboDocumentType.Items.Add(liDropdown);
            //    // string xmlDefault = elements.Attribute("Default_Value").Value;
            //    if (elements.Attribute("Default_Value").Value != null && elements.Attribute("Default_Value").Value.Trim() != "" && DefDoctype.Length == 0)
            //    {
            //        DefDoctype.Append(elements.Attribute("Default_Value").Value);
            //    }

            //    int sortOrder = 0;
            //    foreach (XElement subDocs in elements.Elements())
            //    {
            //        //string subDoc = subDocs.Attribute("name").Value;
            //        sortOrder++;
            //        objStatics = new StaticLookup();
            //        objStatics.Field_Name = elements.Attribute("name").Value;
            //        objStatics.Value = subDocs.Attribute("name").Value;
            //        objStatics.Sort_Order = sortOrder;
            //        objStatics.Default_Value = subDocs.Attribute("Default_Value").Value;
            //        docSublist.Add(objStatics);

            //    }
            //}

            //CAP-2767
            cboDocumentType.Items.Clear();
            IList<StaticLookup> docSublist = new List<StaticLookup>();
            doctypeList objDoctypeList = new doctypeList();
            objDoctypeList = ConfigureBase<doctypeList>.ReadJson("Doctype.json");
            StaticLookup objStatics = null;
            ListItem liDropdown = null;
            StringBuilder DefDoctype = new StringBuilder();
            foreach (Doctype dt in objDoctypeList.DocType)
            {
                if (dt.name.ToUpper() == "RESULTS")
                    liDropdown = new ListItem(dt.name, "DIAGNOSTIC ORDER");
                else
                    liDropdown = new ListItem(dt.name, dt.name);
                cboDocumentType.Items.Add(liDropdown);
                if (dt.Default_Value != null && dt.Default_Value.Trim() != "" && DefDoctype.Length == 0)
                    DefDoctype.Append(dt.Default_Value);
                int sortOrder = 0;

                foreach (subDoc sb in dt.subDoc)
                {
                    sortOrder++;
                    objStatics = new StaticLookup();
                    objStatics.Field_Name = dt.name;
                    objStatics.Value = sb.name;
                    objStatics.Sort_Order = sortOrder;
                    objStatics.Default_Value = sb.Default_Value;
                    docSublist.Add(objStatics);
                }
            }
            cboDocumentType.Items.Insert(0, "");
            cboDocumentSubType.Items.Insert(0, "");
            Session["DocSubList"] = docSublist;
            if (DefDoctype.Length != 0)
            {
                Session["Doctype"] = DefDoctype.ToString();
                cboDocumentType.Items.FindByText(DefDoctype.ToString()).Selected = true;
                LoadDocumentSubType(DefDoctype.ToString());
            }

        }

        public void EditGrid(ulong scanIndexID)
        {
            btnSave.Enabled = true;
            rdbPageRange.Checked = true;
            rdbAll.Checked = false;
            cboPhysician.Enabled = true;
            cboStandingOrders.Enabled = true;
            cboStandingOrders.Items.Clear();

            hdnUpdateMode.Value = "";

            ulong order_submit_id = 0;
            if (Session["IndexList"] != null)
            {
                IList<scan_index> index_lst = (IList<scan_index>)Session["IndexList"];
                IList<scan_index> temp_lst = (from doc in index_lst
                                              where doc.Id == Convert.ToUInt64(scanIndexID)
                                              select doc).ToList<scan_index>();
                Session.Add("EditList", temp_lst);
                //Cap - 571
                if (temp_lst[0].Is_Manually_Reviewed_And_Signed == "Y")
                    chkReviewandSign.Checked = true;
                else
                    chkReviewandSign.Checked = false;
                //CAP-2847
                if (temp_lst[0].Is_External_Medical_Record == "Y")
                    chkExternalMedicalRecord.Checked = true;
                else
                    chkExternalMedicalRecord.Checked = false;

                IList<Scan> temp_Scan_lst = new List<Scan>();
                ScanManager objScnmngr = new ScanManager();
                //For bug Id :
                if (index_lst.Count > 0)
                    temp_Scan_lst = objScnmngr.GetScanDocumentsListByID(index_lst[0].Scan_ID);

                if (temp_Scan_lst.Count > 0)
                {
                    file_name = new StringBuilder();
                    file_name.Append(temp_Scan_lst[0].Scanned_File_Path);
                    hdnPagecount.Value = temp_Scan_lst[0].No_of_Pages.ToString();
                    PageLabel.InnerText = "/ " + hdnPagecount.Value.ToString();// Convert.ToString((int)Session["Page_Count"]);
                    Session["FileName"] = Path.GetFileName(file_name.ToString());
                    Session["ImagePath"] = file_name.ToString();
                }
                /* Populating Particular Record */

                if (temp_lst[0].Document_Type == "Results")
                {
                    cboDocumentType.SelectedValue = "DIAGNOSTIC ORDER";
                    spanoutstandorder.Attributes.Remove("class");
                    spanoutstandorder.Attributes.Add("class", "MandLabelstyle");
                    spanorderstar.Visible = true;

                    spanphy.Attributes.Remove("class");
                    spanphy.Attributes.Add("class", "MandLabelstyle");
                    spanphystar.Visible = true;

                    //Accordion wnd
                    dOrder.Attributes.Add("data-target", "#dOrderCollapse");
                    dOrder.Attributes.Remove("class");
                    dOrder.Attributes.Add("class", "panel-headingIndexing LabelStyle");

                    dOrderCollapse.Attributes.Remove("class");
                    dOrderCollapse.Attributes.Add("class", "panelborderboxIndexing panel-collapse collapse in");
                }
                else
                {
                    //Accordion wnd
                    dOrder.Attributes.Remove("data-target");
                    dOrder.Attributes.Remove("class");
                    dOrder.Attributes.Add("class", "panel-headingdisable LabelStyle");

                    dOrderCollapse.Attributes.Remove("class");
                    dOrderCollapse.Attributes.Add("class", "panelborderboxIndexing panel-collapse collapse");

                    cboDocumentType.SelectedValue = temp_lst[0].Document_Type;
                    spanoutstandorder.Attributes.Remove("class");
                    spanoutstandorder.Attributes.Add("class", "spanstyle");
                    spanorderstar.Visible = false;

                    spanphy.Attributes.Remove("class");
                    spanphy.Attributes.Add("class", "spanstyle");
                    spanphystar.Visible = false;

                    Labspan.Attributes.Remove("class");
                    Labspan.Attributes.Add("class", "spanstyle");
                    //slabMandatory.Attributes.Remove("class");
                    slabMandatory.Visible = false;

                    if (temp_lst[0].Document_Type.ToUpper() == "ENCOUNTERS")// && temp_lst[0].Encounter_ID != 0)
                    {
                        LoadEncPhysician();
                        if (temp_lst[0].Encounter_ID != 0)
                            ddEncPhyName.SelectedValue = temp_lst[0].Encounter_ID.ToString();
                    }
                    else
                    {
                        dtpDocumentDate.Value = UtilityManager.ConvertToLocal(temp_lst[0].Document_Date).ToString("dd-MMM-yyyy");

                        if (ddEncPhyName.Items != null && ddEncPhyName.Items.Count > 0)
                        {
                            if (ddEncPhyName.SelectedIndex > -1)
                            {
                                ddEncPhyName.ClearSelection();
                            }
                            ddEncPhyName.SelectedIndex = 0;
                            ddEncPhyName.Items.Clear();
                        }
                        dtpDocumentDate.Disabled = false;
                        lblDocDate.Attributes.Remove("class");
                        lblDocDate.Attributes.Add("class", "MandLabelstyle");
                        DocDateStar.Visible = true;

                        ddEncPhyName.Enabled = false;
                        lblDosPhy.Attributes.Remove("class");
                        lblDosPhy.Attributes.Add("class", "spanstyle");
                        DosPhyStar.Visible = false;
                        EncMsg.Visible = false;

                    }
                }
                LoadDocumentSubType(temp_lst[0].Document_Type.ToUpper());
                PatientDetails.Text = HumanDetails(temp_lst[0].Human_ID.ToString());
                PatientDetails.Attributes.Remove("class");
                PatientDetails.Attributes.Add("class", "patientPaneDisabled"); PatientDetails.CssClass = "patientPaneDisabled";
                cboDocumentSubType.SelectedValue = temp_lst[0].Document_Sub_Type;
                //dtpDocumentDate.Value = UtilityManager.ConvertToLocal(temp_lst[0].Document_Date).ToString("dd-MMM-yyyy");
                txtSelectedPages.Value = temp_lst[0].Page_Selected;

                btnSave.Text = "Update";
                btnClearAll.Text = "Cancel";
                upPatientDetails.Update();
                upIndexingDetails.Update();



                order_submit_id = temp_lst.First().Order_ID;
                //phy_id = temp_lst.First().Physician_ID;
                //string Cpt_Data = HttpUtility.HtmlDecode("~");

                if (temp_lst[0].Order_ID != 0)
                {
                    lstorders = new List<Orders>();
                    order_submit_id = temp_lst[0].Order_ID;
                    ulong humanID = temp_lst[0].Human_ID;
                    VisibleOrders();
                    lstorders = (IList<Orders>)Session["OrdersList"];
                    cboStandingOrders.Items.Clear();
                    cboPhysician.Items.Clear();
                    cboStandingOrders.Visible = true;
                    cboPhysician.Visible = true;
                    chkShowAll.Visible = true;
                    cboOrderPhysician.Visible = true;
                    chkOrderingPhyShowAll.Visible = true;
                    cboLab.Visible = true;
                    btnSearchCpt.Visible = true;
                    dtpSpecCollection.Visible = true;
                    //string _order_date = string.Empty;

                    OrdersManager objorders = new OrdersManager();
                    //lstorders = objorders.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "ORDER_GENERATE", ClientSession.HumanId); ;

                    //Cap - 1118
                    IList<scan_index> lstscan = new List<scan_index>();
                    Scan_IndexManager objmanager = new Scan_IndexManager();
                    lstscan = objmanager.GetScannedObjectByHumanID(ClientSession.HumanId);


                    IList<Orders> lstorderstemp = new List<Orders>();
                    lstorderstemp = objorders.GetLabProcedureBy_ObjectType_And_CurrentProcess_And_HumanId("DIAGNOSTIC ORDER", "ORDER_GENERATE", ClientSession.HumanId);
                    IList<string> ilstOrdersubmit = new List<string>();

                    //string sFacilityAncillary = ConfigurationManager.AppSettings["AncillaryLab"].Trim().ToUpper();
                    IList<string> lstordersCMG = new List<string>();
                    //var IscheckCMGAncillary = lstorderstemp.Where(aa => aa.Order_Code_Type.ToUpper().Trim() == ConfigurationManager.AppSettings["AncillaryLab"].ToUpper().Trim()).ToList();
                    var lab = from l in ApplicationObject.facilityLibraryList where l.Fac_Name == ClientSession.FacilityName select l;
                    IList<FacilityLibrary> facLabList = lab.ToList<FacilityLibrary>();
                    var IscheckCMGAncillary = lstorderstemp.Where(aa => facLabList.Any(bb => bb.Short_Name.ToUpper() == aa.Order_Code_Type.ToString().ToUpper())).ToList();

                    IDictionary<string, string> ilstOrdersubmitParentorder = new Dictionary<string, string>();
                    if (IscheckCMGAncillary.Count > 0)
                    {

                        //XDocument xmlLab = XDocument.Load(Server.MapPath(@"ConfigXML\LabList.xml"));
                        //IEnumerable<XElement> xml = xmlLab.Element("LabList")
                        //   .Elements("Lab").Where(a => a.Attribute("type").Value.ToString() != "DME" && a.Attribute("name").Value.ToString() == facLabList[0].Short_Name)
                        //   .OrderBy(s => (int)s.Attribute("sort_order"));
                        //string xmlValue = string.Empty;
                        //if (xml != null)
                        //{
                        //    foreach (XElement LabElement in xml)
                        //    {
                        //        xmlValue = LabElement.Attribute("id").Value;
                        //    }
                        //}
                        //CAP-2773
                        Lablist objlablist = new Lablist();
                        objlablist = ConfigureBase<Lablist>.ReadJson("LabList.json");
                        List<Labs> listLabList = new List<Labs>();
                        listLabList = objlablist.Lab.Where(a => a.type != "DME" && a.name == facLabList[0].Short_Name)
                            .OrderBy(s => (int)Convert.ToInt32(s.sort_order)).ToList();
                        string xmlValue = string.Empty;
                        if (listLabList != null)
                        {
                            foreach (Labs objlab in listLabList)
                            {
                                xmlValue = objlab.id;
                            }
                        }

                        if (xmlValue != string.Empty)
                        {
                            ilstOrdersubmit = objorders.GetOrderByHuman(ClientSession.HumanId, ClientSession.FacilityName, Convert.ToUInt64(xmlValue));//, ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper());
                            lstordersCMG = objorders.GetOrdersforCMGAncillary("DIAGNOSTIC ORDER", "ORDER_GENERATE", ClientSession.HumanId, Convert.ToUInt64(xmlValue));//, ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper());
                            ilstOrdersubmitParentorder = objorders.GetOrdersforCMGAncillaryParentOrders(lstordersCMG);
                        }
                    }

                    List<string> svalue = new List<string>();
                    if (ilstOrdersubmit.Count > 0)
                    {
                        var svalues = (from o in ilstOrdersubmit select o.Split('|')[0].Trim()).ToList();
                        svalue = svalues.Where(aa => !lstordersCMG.Any(oo => oo.ToString() == aa.ToString().Trim())).ToList();
                    }
                    // lstorders = lstorderstemp.Where(item => !svalue.Any(i => i.ToString().Trim() == item.Order_Submit_ID.ToString())).ToList<Orders>();

                    if (facLabList.Count > 0 && facLabList[0].Is_Ancillary == "Y")
                    {
                        //if (svalue.Count > 0 && lstordersCMG.Count > 0)
                        //{
                        //    lstorders = lstorderstemp.Where(item => !svalue.Any(i => i.ToString().Trim() == item.Order_Submit_ID.ToString())).ToList<Orders>();
                        //}
                        lstorders = lstorderstemp.Where(item => lstordersCMG.Any(cmglst => cmglst.ToString() == item.Order_Submit_ID.ToString())).ToList();
                    }
                    else
                    {
                        lstorders = lstorderstemp.Where(item => !svalue.Any(i => i.ToString().Trim() == item.Order_Submit_ID.ToString())).ToList<Orders>();
                    }


                    cboLab.Items.Insert(0, "");
                    cboLab.SelectedItem.Text = "";
                    if (lstorders != null && lstorders.Count() > 0)
                    {
                        for (int i = 0; i < lstorders[0].lstLab.Count; i++)
                        {
                            ListItem cboItem = new ListItem(lstorders[0].lstLab[i].Lab_Name, lstorders[0].lstLab[i].Id.ToString());
                            cboLab.Items.Add(cboItem);
                        }
                    }
                    cboPhysician.Items.Insert(0, "");
                    cboPhysician.SelectedItem.Text = "";

                    upIndexingDetails.Update();


                    if (lstorders.Count > 0 && lstorders[0].Id != 0)
                    {
                        #region "Populating The Orders"

                        int submittedOrder = lstorders.GroupBy(a => a.Order_Submit_ID).Count();
                        ListItem orderItems = null;


                        var temp = lstorders.Where(a => a.Lab_Procedure != "Paper Order").Select(a => a.Order_Submit_ID).Distinct();
                        if (temp != null)
                        {
                            var facilityAncillary = from f in ApplicationObject.facilityLibraryList where f.Is_Ancillary == "Y" select f;
                            IList<FacilityLibrary> ilstAncillaryFac = facilityAncillary.ToList<FacilityLibrary>();

                            foreach (ulong submittedorders in temp)
                            {
                                orderItems = new ListItem();
                                //string OrderText = "";
                                StringBuilder OrderText = new StringBuilder();
                                Boolean bIsSkip = false;
                                foreach (Orders item in lstorders.Where(a => a.Order_Submit_ID == submittedorders))
                                {

                                    bIsSkip = false;
                                    var AncLab = (from l in ilstAncillaryFac where l.Short_Name.ToUpper() == item.Order_Code_Type.ToUpper() select l).ToList();
                                    //Cap - 1118
                                    var scanorder = (from m in lstscan where m.Order_ID == submittedorders && m.Id != scanIndexID select m).ToList();
                                    if (AncLab.Count > 0)
                                    {
                                        if (item.Encounter_ID == 0)
                                        {
                                            bIsSkip = true;
                                            continue;
                                        }
                                        else
                                        {
                                            EncounterManager encMngr = new EncounterManager();
                                            IList<Encounter> encList = encMngr.GetEncounterByEncounterID(item.Encounter_ID);
                                            if (encList.Count > 0)
                                            {
                                                if (encList[0].Order_Submit_ID == 0)
                                                {
                                                    bIsSkip = true;
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    //Cap - 1118
                                    else if (scanorder.Count > 0)
                                    {
                                        bIsSkip = true;
                                        continue;

                                    }

                                    if (item.Lab_Procedure != "" && item.Lab_Procedure_Description != "")
                                    {
                                        //OrderText += (item.Lab_Procedure + " - " + item.Lab_Procedure_Description + HttpUtility.HtmlDecode("~"));
                                        OrderText.Append(item.Lab_Procedure + " - " + item.Lab_Procedure_Description + HttpUtility.HtmlDecode("~"));
                                    }
                                    else
                                    {
                                        OrderText.Append("");
                                    }

                                }
                                DateTime OrderDateAndTime = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Created_Date_And_Time;
                                DateTime SpecCollectionDate = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Internal_Property_Spec_Collection_Date;
                                ulong LabID = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Internal_Property_LabID;
                                ulong PhyID = 0;
                                //For Bug Id : 70899
                                //if (ilstOrdersubmitParentorder.Count > 0)
                                //{
                                //    var svaluephyID = ilstOrdersubmitParentorder.Where(aa => aa.Key == submittedorders.ToString()).Select(aa => aa.Value).FirstOrDefault(); ;
                                //    if (svaluephyID != null && svaluephyID != string.Empty)
                                //        PhyID = Convert.ToUInt32(svaluephyID);
                                //}
                                //else
                                //    PhyID = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Physician_ID;


                                if (lstorders.Count > 0)
                                    PhyID = lstorders.Where(a => a.Order_Submit_ID == submittedorders).FirstOrDefault().Physician_ID;

                                if (PhyID == 0 && ilstOrdersubmitParentorder.Count > 0)
                                {
                                    var svaluephyID = ilstOrdersubmitParentorder.Where(aa => aa.Key == submittedorders.ToString()).Select(aa => aa.Value).FirstOrDefault(); ;
                                    if (svaluephyID != null && svaluephyID != string.Empty)
                                        PhyID = Convert.ToUInt32(svaluephyID);
                                }

                                if (PhyID == 0)
                                {
                                    //string sPhysicainID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
                                    if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] != null)
                                        PhyID = Convert.ToUInt32(ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]);
                                }

                                OrderText.Append("|[ #" + submittedorders.ToString() + " ]|");
                                String dtSpecCollectionDateConverted = string.Empty;

                                if (Request.QueryString["CurrentZone"] != null)
                                {
                                    //string Offset = Request.QueryString["CurrentZone"];
                                    StringBuilder Offset = new StringBuilder();
                                    Offset.Append(Request.QueryString["CurrentZone"]);
                                    if (Offset.ToString().StartsWith("-"))
                                    {
                                        DateTime localTime = OrderDateAndTime.AddMinutes(Convert.ToDouble(Offset.ToString()));
                                        OrderText.Append(localTime.ToString("dd-MMM-yyyy hh:mm tt"));
                                        if (SpecCollectionDate != DateTime.MinValue)
                                        { dtSpecCollectionDateConverted = SpecCollectionDate.AddMinutes(Convert.ToDouble(Offset.ToString())).ToString("dd-MMM-yyyy hh:mm tt"); }
                                    }
                                    else
                                    {
                                        DateTime localTime = OrderDateAndTime.Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0));
                                        OrderText.Append(localTime.ToString("dd-MMM-yyyy hh:mm tt"));
                                        if (SpecCollectionDate != DateTime.MinValue)
                                        { dtSpecCollectionDateConverted = SpecCollectionDate.Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0)).ToString("dd-MMM-yyyy hh:mm tt"); }
                                    }
                                }
                                if (bIsSkip == false)
                                {
                                    orderItems.Text = OrderText.ToString();
                                    //string orderValue = PhyID.ToString() + "|" + LabID.ToString() + "|" + submittedorders.ToString();
                                    StringBuilder orderValue = new StringBuilder();
                                    orderValue.Append(PhyID.ToString() + "|" + LabID.ToString() + "|" + submittedorders.ToString());
                                    if (dtSpecCollectionDateConverted != string.Empty)
                                    {
                                        //orderValue += "|" + dtSpecCollectionDateConverted;
                                        orderValue.Append("|" + dtSpecCollectionDateConverted);
                                    }
                                    orderItems.Value = orderValue.ToString();
                                    orderItems.Attributes.Add("title", OrderText.ToString());
                                    cboStandingOrders.Items.Add(orderItems);
                                }
                            }
                        }
                        ListItem chooseOrder = new ListItem();
                        chooseOrder.Text = "Paper Order";
                        chooseOrder.Value = "Paper Order";
                        cboStandingOrders.Items.Add(chooseOrder);
                        if (cboStandingOrders.Items != null && cboStandingOrders.Items.Count > 0)
                        {
                            if (cboStandingOrders.SelectedIndex > -1)
                            {
                                cboStandingOrders.ClearSelection();
                            }
                            cboStandingOrders.SelectedIndex = 0;
                        }

                        #endregion
                    }

                    //string sFacilityCmg = ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper();
                    //if (sFacilityCmg.ToUpper() == ClientSession.FacilityName.ToUpper())
                    //{
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;

                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")// && elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                    {
                        ShowAllphysicianforCMGAncillary();
                        ShowAllOrderingphysicianforCMGAncillary();
                        chkShowAll.Checked = true;
                        chkOrderingPhyShowAll.Checked = true;
                    }
                    else
                    {

                        LoadPhysicianCombo();
                        LoadOrderingPhysicianCombo();

                        chkShowAll.Checked = false;
                        chkOrderingPhyShowAll.Checked = false;
                    }



                    #region "Loading Lab List"

                    LoadLabCombo();

                    #endregion

                    #region "Selecting the Appropriate Order"
                    /* Selecting Lab, Physicians for the corresponding Order */
                    //string OrderedPhysician = lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id).Physician_ID.ToString();
                    //string OrderedLab = lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id).Internal_Property_LabID.ToString();
                    //string OrderedID = lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id).Order_Submit_ID.ToString();
                    //string sPaperOrder = lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id).Lab_Procedure.ToString();

                    StringBuilder OrderedPhysician = new StringBuilder();
                    if (lstorders != null && lstorders.Count > 0)
                        OrderedPhysician.Append(lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id)?.Physician_ID.ToString() ?? "");

                    StringBuilder OrderedLab = new StringBuilder();
                    if (lstorders != null && lstorders.Count > 0)
                        OrderedLab.Append(lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id)?.Internal_Property_LabID.ToString() ?? "");

                    StringBuilder OrderedID = new StringBuilder();
                    if (lstorders != null && lstorders.Count > 0)
                        OrderedID.Append(lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id)?.Order_Submit_ID.ToString() ?? "");

                    StringBuilder sPaperOrder = new StringBuilder();
                    if (lstorders != null && lstorders.Count > 0)
                        sPaperOrder.Append(lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id)?.Lab_Procedure.ToString() ?? "");

                    if (OrderedPhysician.ToString() == "0")
                    {
                        //string sPhysicainID = ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"];
                        if (ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"] != null)
                            OrderedPhysician.Append(ConfigurationManager.AppSettings["DefaultPhysicianIDIndexing"]);
                    }

                    // string selectedValue = OrderedPhysician + "|" + OrderedLab + "|" + OrderedID;
                    StringBuilder selectedValue = new StringBuilder();
                    selectedValue.Append(OrderedPhysician + "|" + OrderedLab + "|" + OrderedID);
                    DateTime orderedDatetime = DateTime.MinValue;
                    if (lstorders != null && lstorders.Count > 0)
                    {
                        //Cap - 1419
                        var orderdate = lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id);

                        if (orderdate != null)
                            orderedDatetime = lstorders.FirstOrDefault(a => a.Order_Submit_ID == order_submit_id).Internal_Property_Spec_Collection_Date;
                    }
                    //string ConvertedorderedDatetime = string.Empty;
                    StringBuilder ConvertedorderedDatetime = new StringBuilder();

                    if (orderedDatetime != DateTime.MinValue)
                    {
                        if (Request.QueryString["CurrentZone"] != null)
                        {
                            //string Offset = Request.QueryString["CurrentZone"];
                            StringBuilder Offset = new StringBuilder();
                            Offset.Append(Request.QueryString["CurrentZone"]);
                            if (Offset.ToString().StartsWith("-"))
                            {
                                if (orderedDatetime != DateTime.MinValue)
                                { ConvertedorderedDatetime.Append(orderedDatetime.AddMinutes(Convert.ToDouble(Offset.ToString())).ToString("dd-MMM-yyyy hh:mm tt")); }

                            }
                            else
                            {
                                if (orderedDatetime != DateTime.MinValue)
                                { ConvertedorderedDatetime.Append(orderedDatetime.Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0)).ToString("dd-MMM-yyyy hh:mm tt")); }
                            }
                        }
                        if (ConvertedorderedDatetime.Length != 0)
                            selectedValue.Append("|" + ConvertedorderedDatetime);
                        dtpSpecCollection.Value = ConvertedorderedDatetime.ToString();
                    }
                    //if (cboPhysician.Items.FindByValue(OrderedPhysician.ToString()) != null)
                    //    cboPhysician.SelectedValue = OrderedPhysician.ToString();

                    if (cboOrderPhysician.Items.FindByValue(OrderedPhysician.ToString()) != null)
                        cboOrderPhysician.SelectedValue = OrderedPhysician.ToString();

                    if (cboPhysician.Items.FindByValue(temp_lst[0].Appointment_Provider_ID.ToString()) != null)
                        cboPhysician.SelectedValue = temp_lst[0].Appointment_Provider_ID.ToString();

                    if (sPaperOrder.ToString() != "Paper Order")
                    {
                        cboLab.SelectedValue = string.IsNullOrEmpty(OrderedLab.ToString()) ? "0" : OrderedLab.ToString();
                        ListItem CheckSelectedValue = cboStandingOrders.Items.FindByValue(selectedValue.ToString());
                        if (CheckSelectedValue != null && CheckSelectedValue.Value != "")
                        {
                            cboStandingOrders.SelectedValue = selectedValue.ToString();
                            cboStandingOrders.Attributes.Add("title", cboStandingOrders.SelectedItem.Text.ToString());
                        }
                    }
                    else
                    {
                        if (OrderedLab.ToString() != "0")
                            cboLab.SelectedValue = OrderedLab.ToString();
                        //ListItem CheckSelectedValue = cboStandingOrders.Items.FindByValue(selectedValue);
                        //if (CheckSelectedValue != null && CheckSelectedValue.Value != "")
                        //{
                        cboStandingOrders.SelectedValue = sPaperOrder.ToString();
                        cboStandingOrders.Attributes.Add("title", cboStandingOrders.SelectedItem.Text.ToString());
                        //}
                    }
                    cboIs_Interperated.SelectedValue = temp_lst[0].Is_Narrative_Interpretation;
                    #endregion

                }
                else
                {
                    HideOrders();
                    updateOrders.Update();
                }
                if (rdbAll.Checked == true)
                {
                    btnSave.Enabled = false;
                    txtSelectedPages.Value = "";
                    txtSelectedPages.Disabled = true;
                    btnMoveToNextProcess.Disabled = false;
                }
                else if (rdbPageRange.Checked == true)
                {
                    btnSave.Enabled = true;
                    txtSelectedPages.Disabled = false;
                    btnMoveToNextProcess.Disabled = true;
                }
                if (cboStandingOrders.SelectedValue != "Paper Order")
                {
                    cboLab.Enabled = false;
                    //cboPhysician.Enabled = false;
                    cboPhysician.Enabled = true;
                    dtpSpecCollection.Disabled = true;
                    cboOrderPhysician.Enabled = false;
                    chkShowAll.Enabled = true;
                    chkOrderingPhyShowAll.Enabled = false;
                    chkOrderingPhyShowAll.Checked = false;
                    btnSearchCpt.Disabled = true;
                }
                else
                {
                    cboLab.Enabled = true;
                    cboPhysician.Enabled = true;
                    chkShowAll.Enabled = true;
                    chkOrderingPhyShowAll.Enabled = true;
                    btnSearchCpt.Disabled = false;
                    dtpSpecCollection.Disabled = false;
                    cboOrderPhysician.Enabled = true;
                    spnOrderPhy.Attributes.Remove("class");
                    spnOrderPhy.Attributes.Add("class", "MandLabelstyle");
                    spnOrderPhyStar.Visible = true;
                }
            }
            //CAP-2847
            if (chkExternalMedicalRecord.Checked == true)
            {
                chkOrderingPhyShowAll.Checked = true;
                chkShowAll.Checked = true;
                chkReviewandSign.Checked = true;
                cboStandingOrders.Enabled = false;
                cboPhysician.Enabled = false;
                cboOrderPhysician.Enabled = false;
                chkReviewandSign.Enabled = false;
                chkOrderingPhyShowAll.Enabled = false;
                chkShowAll.Enabled = false;
            }
            cboIs_Interperated.Enabled = true;
            updateOrders.Update();
            btnSave.Enabled = true;
            btnSave.Text = "Update";
            btnClearAll.Text = "Cancel";
            hdnUpdateMode.Value = "";
            hdnUpdateMode.Value = string.Empty;
            divLoading.Style.Add("display", "none");
            waitCursor.Update();

            //Cap - 1294
            //CAP-1516
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EditGridupdate", "localStorage.setItem('IsSaveClickedSucessfull','');", true);

        }

        public void updateOrderObjects()
        {
            //XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            IDictionary<ulong, string> phyuserNameIDMap = new Dictionary<ulong, string>();
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null)
            {
                foreach (var facility in physicianFacilityMappingList.PhysicianFacility)
                {
                    foreach (var phyItems in facility.Physician)
                    {
                        //string username = elements.Attribute("username").Value;
                        // string phyID = elements.Attribute("ID").Value;
                        try
                        {
                            phyuserNameIDMap.Add(Convert.ToUInt32(phyItems.ID), phyItems.username);
                        }
                        catch { }
                    }

                }
            }

            Session.Add("usernameIDMap", phyuserNameIDMap);
        }

        #endregion

        #region "Image Operations"

        public int TotalTIFPgs
        {
            get
            {
                if (ViewState["TotalTIFPgs"] == null)
                {
                    TIF TheFile = new TIF(FilePath);
                    ViewState["TotalTIFPgs"] = TheFile.PageCount;
                    TheFile.Dispose();
                }
                return System.Convert.ToInt16(ViewState["TotalTIFPgs"]);
            }
            set
            {
                ViewState["TotalTIFPgs"] = value;
            }
        }

        public String FilePath
        {
            get
            {
                if (ViewState["FilePath"] == null)
                {
                    return "";
                }
                return ViewState["FilePath"].ToString();
            }
            set
            {
                ViewState["FilePath"] = value;
            }
        }

        public void ExtractPDFPage(string sSourcePdfPath, string sDestinationPdfPath, IList<int> iPageNumber)
        {
            PdfReader objPDFReader = null;
            PdfCopy ObjPdfCopyProvider = null;
            PdfImportedPage ObjImportedPage = null;
            iTextSharp.text.Document sourceDocument = null;
            //Jira #CAP-39
            int iTryCount = 1;
        TryAgain:
            try
            {
                if (iPageNumber != null && iPageNumber.Count > 0)
                {
                    objPDFReader = new PdfReader(sSourcePdfPath);
                    sourceDocument = new iTextSharp.text.Document(objPDFReader.GetPageSizeWithRotation(iPageNumber[0]));
                    ObjPdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(sDestinationPdfPath, System.IO.FileMode.Create));
                    sourceDocument.Open();
                    foreach (int pageNumber in iPageNumber)
                    {
                        //CAP-793
                        try
                        {
                            if (pageNumber > 0)
                            {
                                ObjImportedPage = ObjPdfCopyProvider.GetImportedPage(objPDFReader, pageNumber);
                                ObjPdfCopyProvider.AddPage(ObjImportedPage);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    sourceDocument.Close();
                    objPDFReader.Close();
                }

            }
            catch (Exception ex)
            {
                string sErrorMessage = "";
                if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" " + sErrorMessage + "\");", true);
                }
                else
                {
                    //Jira #CAP-39
                    if (iTryCount <= 3)
                    {
                        iTryCount = iTryCount + 1;
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(1500);
                        goto TryAgain;
                    }
                    else
                    {
                        try
                        {
                            UtilityManager.RetryExecptionLog(ex, iTryCount, "SourcePath: " + sSourcePdfPath + " ~ DestinationPath: " + sDestinationPdfPath);
                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Key", "alert('Something went wrong. Please reload the page and try again');", true);
                        }
                    }

                }
            }
        }
        #endregion

        protected void btnFindDocuments_Click(object sender, EventArgs e)
        {

            Session.Remove("IndexList");
            grdIndexing.DataSource = new string[] { };
            grdIndexing.DataBind();
            _imgBig.Attributes.Add("src", "");
            bigImagePDF.Attributes.Add("src", "");
            Session["BrowseLoadList"] = null;
            Session["LoadList"] = null;

        }


        protected void btnUpload_ServerClick(object sender, EventArgs e)
        {
            Session.Remove("IndexList");
            grdIndexing.DataSource = new string[] { };
            grdIndexing.DataBind();

            try
            {
                if (ClientSession.FacilityName != null && ClientSession.FacilityName != "" && ddSelectedFacility != null && ddSelectedFacility.Items.FindByValue(ClientSession.FacilityName.ToString().Trim()) != null && ddSelectedFacility.SelectedItem.Text != string.Empty)
                {
                    ddSelectedFacility.Text = ClientSession.FacilityName;
                }
                //string sFacilityName = string.Empty;
                //Cap - 1097
                //StringBuilder sFacilityName = new StringBuilder();
                if (fileupload.HasFile)
                {
                    string[] sFileExtension = { ".tif", ".jpeg", ".png", ".jpg", ".bmp", ".pdf", ".tiff" };

                    Session["BrowseLoadList"] = null;
                    Session["LoadList"] = null;
                    // Session["SelectedDateList"] = null;
                    //Session["BrowseFileNames"] = null;
                    //if (Request.QueryString["Screen"] != null && Request.QueryString["Screen"] == "PatientPortalOnlineDoumnets")//For patient portal
                    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Clearall", "clickClearAll();", true);

                    //Jira #CAP-39
                    int iTryCount = 1;
                TryAgain:
                    try
                    {
                        //Cap - 1097
                        StringBuilder sFacilityName = new StringBuilder();

                        if (rdbRemoteDir.Checked == true)
                        {
                            if (ddSelectedFacility.Text.Contains('~'))
                                sFacilityName.Append(ddSelectedFacility.Text.Split('~')[1].Trim());
                            else
                                sFacilityName.Append(ddSelectedFacility.Text);
                        }
                        else// Browse Functionality
                            sFacilityName.Append(ClientSession.FacilityName);

                        //string sDirPath = ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File";
                        //Server.MapPath("~/atala-capture-upload/Indexing_Files/");
                        DirectoryInfo dir = new DirectoryInfo(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File");
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }

                        FileInfo[] dirfiles = dir.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                        HttpFileCollection uploadedFiles = Request.Files;
                        if (uploadedFiles.Count == 1)
                        {
                            if (!sFileExtension.Contains(Path.GetExtension(uploadedFiles[0].FileName.ToLower())))
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Fileextension", "StopLoadOnUploadFile();alert('Please select supported file format(*.tif,*.png,*.jpeg,*.pdf,*.bmp,*.jpg)');", true);
                                return;
                            }
                        }


                        //if (Session["BrowseFileNames"] != null)
                        //{
                        //    lstDocuments = ((IList<string>)HttpContext.Current.Session["BrowseFileNames"]).Distinct().ToList();
                        //}


                        if (uploadedFiles.Count > 0)
                        {
                            for (int fileCount = 0; fileCount < uploadedFiles.Count; fileCount++)
                            {
                                HttpPostedFile currentFile = uploadedFiles[fileCount];
                                //string fileName = Path.GetFileName(currentFile.FileName);
                                //string sExtension = Path.GetExtension(currentFile.FileName).ToLower();
                                if (sFileExtension.Contains(Path.GetExtension(currentFile.FileName).ToLower()))
                                {
                                    //Gitlab #3943 - File Name Length Issue
                                    int iFile_Length = (ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File" + "/").Length + Path.GetExtension(currentFile.FileName).Length;

                                    if ((ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File" + "/" + Path.GetFileName(currentFile.FileName)).Length >= 260)
                                    {
                                        int iAllowed_File_Length = 259 - iFile_Length;
                                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Fileextension", "StopLoadOnUploadFile();DisplayErrorMessage('114022','','" + iAllowed_File_Length + "');", true);
                                        return;
                                    }

                                    if (currentFile.ContentLength > 0)
                                    {
                                        lstDocuments.Add(Path.GetFileName(currentFile.FileName));
                                        //string SelectedFilePath = ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File" + "/" + Path.GetFileName(currentFile.FileName);
                                        StringBuilder SelectedFilePath = new StringBuilder();

                                        SelectedFilePath.Append(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File" + "/" + Path.GetFileName(currentFile.FileName));

                                        if (dirfiles.Length > 0)
                                        {
                                            if (File.Exists(SelectedFilePath.ToString()) == true)
                                            {
                                                IList<Scan> ilstScan = new List<Scan>();
                                                ilstScan = ((IList<Scan>)HttpContext.Current.Session["LoadList"]);
                                                if (Session["LoadList"] != null)
                                                {
                                                    bool CheckHasFile = ilstScan.Any(cus => cus.Scanned_File_Name == Path.GetFileName(currentFile.FileName));
                                                    if (CheckHasFile == false)
                                                    {
                                                        for (int j = 0; j < dirfiles.Length; j++)
                                                        {
                                                            if (Path.GetFileName(currentFile.FileName) == dirfiles[j].Name)
                                                            {
                                                                try
                                                                {
                                                                    File.Delete(dirfiles[j].FullName);
                                                                }
                                                                catch { }
                                                            }
                                                        }
                                                        currentFile.SaveAs(SelectedFilePath.ToString());
                                                    }
                                                    else
                                                    {
                                                        //already file Exists
                                                        try
                                                        {
                                                            File.Delete(SelectedFilePath.ToString());
                                                        }
                                                        catch { }
                                                        currentFile.SaveAs(SelectedFilePath.ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        //already file Exists
                                                        File.Delete(SelectedFilePath.ToString());
                                                    }
                                                    catch { }
                                                    currentFile.SaveAs(SelectedFilePath.ToString());
                                                }
                                            }
                                            else
                                            {
                                                currentFile.SaveAs(SelectedFilePath.ToString());
                                            }
                                        }
                                        else
                                        {
                                            currentFile.SaveAs(SelectedFilePath.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        if (lstDocuments.Count > 0)
                        {
                            ScanManager scanProxy = new ScanManager();
                            lstScanList = scanProxy.GetOnlineDocumentsList(sFacilityName.ToString(), lstDocuments.Distinct().ToArray(), selectedDate);

                            //DateTime dtCurDate = UtilityManager.ConvertToUniversal();
                            //lstScanList = scanProxy.GetOnlineDocumentsList(ddSelectedFacility.Text, lstDocuments.Distinct().ToArray(), dtCurDate);

                            IList<Scan> lsifile = new List<Scan>();
                            if (lstScanList.Count > 0)
                            {
                                foreach (Scan scannedRecord in lstScanList)
                                {
                                    scannedRecord.Scanned_File_Path = Path.Combine(ConfigurationManager.AppSettings["ScanningPath_Local"] + "\\" + sFacilityName + "\\Scanned_Images\\" + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File", scannedRecord.Scanned_File_Name);
                                    lsifile.Add(scannedRecord);
                                }
                            }
                            Session["BrowseLoadList"] = lsifile;
                            // Session["BrowseFileNames"] = lsifile.Distinct().ToList();
                        }

                        if (lstScanList.Count > 0)
                        {
                            Session["LocalUploadFiles"] = lstScanList;
                        }
                        //CAP-1225
                        LoadPhysicianCombo();
                    }
                    catch (Exception ex)
                    {
                        string sErrorMessage = "";
                        if (UtilityManager.CheckFileNotFoundException(ex, out sErrorMessage))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\" "+ sErrorMessage+ "\");", true);
                        }
                        else
                        {
                            //Jira #CAP-39
                            if (iTryCount <= 3)
                            {
                                iTryCount = iTryCount + 1;
                                Thread.Sleep(1500);
                                goto TryAgain;
                            }
                            else
                            {
                                UtilityManager.RetryExecptionLog(ex, iTryCount);
                                Console.Write(ex);
                                throw (ex);
                            }
                        }
                    }
                }
                rdbAll.Checked = true;
                if (human_id != 0)
                    btnMoveToNextProcess.Disabled = false;
                else
                    btnMoveToNextProcess.Disabled = true;
                txtSelectedPages.Disabled = true;
                rdbPageRange.Checked = false;
                //CAP-1225
                cboDocumentType.SelectedIndex = 0;
                cboDocumentSubType.SelectedIndex = 0;
                EncMsg.Visible = false;
                HideOrders();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw (ex);
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadingImage", "StopLoadOnUploadFile();", true);
        }

        string HumanDetails(string shumanId)
        {
            string humanDetails = "";
            IList<string> ilstIndexTagList = new List<string>();
            ilstIndexTagList.Add("HumanList");

            IList<object> ilstIndexBlobFinal = new List<object>();
            ilstIndexBlobFinal = UtilityManager.ReadBlob(Convert.ToUInt64(shumanId), ilstIndexTagList);

            if (ilstIndexBlobFinal != null && ilstIndexBlobFinal.Count > 0)
            {
                if (ilstIndexBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstIndexBlobFinal[0]).Count; iCount++)
                    {
                        humanDetails = ((Human)((IList<object>)ilstIndexBlobFinal[0])[iCount]).Last_Name + ", " + ((Human)((IList<object>)ilstIndexBlobFinal[0])[iCount]).First_Name + " " + ((Human)((IList<object>)ilstIndexBlobFinal[0])[iCount]).MI + "|" + ((Human)((IList<object>)ilstIndexBlobFinal[0])[iCount]).Birth_Date.ToString("dd-MMM-yyyy") + "|" + ((Human)((IList<object>)ilstIndexBlobFinal[0])[iCount]).Sex + " |Acc #:" + shumanId; ;
                        hdnHumanID.Value = shumanId.ToString();
                        ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
                    }
                }
            }

            // string FileName = "Human" + "_" + shumanId + ".xml";
            //string strXmlFilePath = Path.Combine(ConfigurationManager.AppSettings["XMLPath"], "Human_" + shumanId + ".xml");
            //try
            //{
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //        XmlNodeList xmlTagName = null;
            //        itemDoc.Load(XmlText);
            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                humanDetails = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value + ", " + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value + " " + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("MI").Value + "|" + Convert.ToDateTime(itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Birth_Date").Value).ToString("dd-MMM-yyyy") + "|" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Sex").Value + " |Acc #:" + shumanId;
            //                hdnHumanID.Value = shumanId.ToString();
            //                ClientSession.HumanId = Convert.ToUInt32(hdnHumanID.Value);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "frmIndexing Line No - 3831 - XmlPath - " + strXmlFilePath + " - " + ex.Message, DateTime.Now, "0", "frmimageviewer");

            //}

            return humanDetails;
        }

        protected void btnclearAfterupload_Click(object sender, EventArgs e)
        {

            if (ClientSession.FacilityName != null && ClientSession.FacilityName != "" && ddSelectedFacility != null && ddSelectedFacility.Items.FindByValue(ClientSession.FacilityName.ToString().Trim()) != null && ddSelectedFacility.SelectedItem.Text != string.Empty)
            {
                ddSelectedFacility.Text = ClientSession.FacilityName;
            }
            Session.Remove("IndexList");
            //Session["FileID"] = null;
            _imgBig.Attributes.Add("src", "");
            bigImagePDF.Attributes.Add("src", "");
            PageLabel.InnerText = "/0";
            //PageBox.Value = "1";
            PageBox.Value = "0";
            hdnHumanID.Value = "";
            btnMoveToNextProcess.Disabled = true;
            rdbAll.Checked = true;
            //CAP-1779 - Patient name not displayed  by default after clicking the Upload button
            var hid = Request.QueryString["HumanId"];
            if (btnFindPatient.Disabled == false && Convert.ToUInt64(string.IsNullOrEmpty(hid) ? "0" : hid) == 0)
            {
                PatientDetails.Text = "";
                PatientDetails.Attributes.Remove("class");
                PatientDetails.Attributes.Add("class", "patientPaneEnabled");
            }

            divLoading.Style.Add("display", "none");
            waitCursor.Update();

            if (cboIs_Interperated.Items != null && cboIs_Interperated.Items.Count > 0)
            {
                if (cboIs_Interperated.SelectedIndex > -1)
                {
                    cboIs_Interperated.ClearSelection();
                }

                cboIs_Interperated.SelectedIndex = 0;
            }
            dtpSpecCollection.Value = "";
            HideOrders();
            if (cboStandingOrders.Items.Count > 0)
            {
                cboStandingOrders.Items.Clear();
            }
            if (cboPhysician.Items.Count > 0)
            {
                cboPhysician.Items.Clear();
            }
            if (cboLab.Items.Count > 0)
            {
                cboLab.Items.Clear();
            }

            if (cboOrderPhysician.Items.Count > 0)
            {
                cboOrderPhysician.Items.Clear();
            }

            //LoadDocType();
            // LoadFiles();
            //if (file_name != null && file_name != string.Empty)
            //{
            //    Session["FileName"] = file_name;
            //    OpenFile(file_name, Session["Page_Count"].ToString());
            //}
            grdIndexing.DataSource = new string[] { };
            grdIndexing.DataBind();
        }

        protected void btnResetFields_Click(object sender, EventArgs e)
        {


            btnSave.Text = "Add";
            btnClearAll.Text = "Reset";
            txtSelectedPages.Value = "";

            if (cboIs_Interperated.Items != null && cboIs_Interperated.Items.Count > 0)
            {
                if (cboIs_Interperated.SelectedIndex > -1)
                {
                    cboIs_Interperated.ClearSelection();
                }
                cboIs_Interperated.SelectedIndex = 0;
            }
            dtpSpecCollection.Value = "";
            HideOrders();
            if (cboStandingOrders.Items.Count > 0)
            {
                cboStandingOrders.Items.Clear();
            }
            if (cboPhysician.Items.Count > 0)
            {
                cboPhysician.Items.Clear();
            }
            if (cboLab.Items.Count > 0)
            {
                cboLab.Items.Clear();
            }
            if (cboOrderPhysician.Items.Count > 0)
            {
                cboOrderPhysician.Items.Clear();
            }
            btnSave.Enabled = false;
            //rdbAll.Checked = true;
            if (grdIndexing.Items.Count == 0)
            {
                btnMoveToNextProcess.Disabled = true;
            }
            else
            {
                btnMoveToNextProcess.Disabled = false;
            }
            //Session["FileID"] = null;
            //Session["FileName"] = null;
            //_imgBig.Attributes.Add("src", "");
            //bigImagePDF.Attributes.Add("src", "");
            if (ddSelectedFacility.Items.FindByValue(ClientSession.FacilityName.ToString().Trim()) != null && ddSelectedFacility.SelectedItem.Text != string.Empty)
                ddSelectedFacility.Text = ClientSession.FacilityName;
            if (Request.QueryString["CurrentZone"] != null)
            {
                // string Offset = Request.QueryString["CurrentZone"];
                StringBuilder Offset = new StringBuilder();
                Offset.Append(Request.QueryString["CurrentZone"]);
                if (Offset.ToString().StartsWith("-"))
                {
                    DateTime localTime = DateTime.Now.ToUniversalTime().Subtract(new TimeSpan(0, Convert.ToInt32(Offset.ToString()), 0));
                    dtpDocumentDate.Value = localTime.ToShortDateString();
                    dtpScannedDate.Value = localTime.ToShortDateString();
                    dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    //productionTime = localTime;
                }
                else
                {
                    DateTime localTime = DateTime.Now.ToUniversalTime().AddMinutes(Convert.ToDouble(Offset.ToString()));
                    dtpDocumentDate.Value = localTime.ToShortDateString();
                    dtpScannedDate.Value = localTime.ToShortDateString();
                    dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    // productionTime = localTime;
                }
            }
            else
            {
                dtpDocumentDate.Value = DateTime.Now.ToShortDateString();
                dtpScannedDate.Value = DateTime.Now.ToShortDateString();
                dtpDocumentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                dtpScannedDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            PageLabel.InnerText = "/0";
            //PageBox.Value = "1";
            PageBox.Value = "0";
            LoadDocType();
            //LoadFiles();
            //if (file_name != null && file_name != string.Empty)
            //{
            //    Session["FileName"] = file_name;
            //    OpenFile(file_name, Session["Page_Count"].ToString());
            //}
            rdbAll.Checked = true;
            btnMoveToNextProcess.Disabled = true;
            txtSelectedPages.Disabled = true;
            rdbPageRange.Checked = false;
            if (cboDocumentType.SelectedValue.ToUpper() != "ENCOUNTERS")
            {
                dtpDocumentDate.Disabled = false;
                lblDocDate.Attributes.Remove("class");
                lblDocDate.Attributes.Add("class", "MandLabelstyle");
                DocDateStar.Visible = true;

                ddEncPhyName.Enabled = false;
                lblDosPhy.Attributes.Remove("class");
                lblDosPhy.Attributes.Add("class", "spanstyle");
                DosPhyStar.Visible = false;
                EncMsg.Visible = false;
            }
            if (ddEncPhyName.Items != null && ddEncPhyName.Items.Count > 0)
            {
                if (ddEncPhyName.SelectedIndex > -1)
                {
                    ddEncPhyName.ClearSelection();
                }
                ddEncPhyName.SelectedIndex = 0;
                ddEncPhyName.Items.Clear();
            }

        }

        [WebMethod(EnableSession = true)]
        public static string SourceImageDelete(string fileName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            //string status = "";
            StringBuilder status = new StringBuilder();



            //string ScnFileName = Path.GetFileName(fileName);
            ScanManager obj = new ScanManager();
            IList<Scan> lstscan = new List<Scan>();
            //Cap - 1292
            int flag = 0;
            if (fileName.Contains("/") == true)
            {
                fileName = fileName.Replace(@"/", @"\");
                fileName = Regex.Replace(fileName, @"\\", @"\");
                //Cap - 1292
                // lstscan = obj.GetScanDocumentsByScanFilePathDeleteCheck(fileName);
                flag = obj.GetScanDocumentsByScanFilePathDeleteCheck(fileName);
                //lstscan = obj.GetScanDocumentsByScanFilePathDeleteCheck(fileName.Replace(@"/", @"\").Replace(@"\\", @"\"));

            }
            else
                //Cap - 1292
                //lstscan = obj.GetScanDocumentsByScanFilePathDeleteCheck(fileName);
                flag = obj.GetScanDocumentsByScanFilePathDeleteCheck(fileName);
            //Cap - 1292
            //if (lstscan.Count == 0)
            if (flag == 0)
            {
                status.Append("Success");
            }
            else
            {
                status.Append("Selected File is already being indexed. So the selected page cannot be deleted.");
            }

            return status.ToString();
        }

        public void btnhdnloadfile_Click(object sender, EventArgs e)
        {

            string fileName = hdnfilenamedelete.Value;

            int iTryCount = 0;
        Move:
            try
            {
                if (File.Exists(fileName) == true)
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    { }

                    //remove the deleted file from session
                    if (rdbLocalDir.Checked == true)
                    {
                        if (Session["LocalUploadFiles"] != null && ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Count > 0 && fileName != null)
                        {
                            IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Where(a => a.Scanned_File_Name == Path.GetFileName(fileName)).ToList<Scan>();
                            if (lstTemp != null && lstTemp.Count > 0)
                            {
                                foreach (Scan item in lstTemp)
                                {
                                    ((IList<Scan>)Session["LocalUploadFiles"]).Remove(item);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception em)
            {
                if (iTryCount < 3)
                {
                    iTryCount++;
                    Thread.Sleep(2000);
                    goto Move;
                }
                else
                {
                    throw em;
                }
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "deletefile", "{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
        }
        [WebMethod(EnableSession = true)]
        public static string DeleteThumbnail(string ImagePath, string delPgNo)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //protected void btnDeleteThumbnail_Click(object sender, EventArgs e)
            //{
            //string ImagePath = hdnfilenamedelete.Value;
            int sTotalPagenumber = 0;

            int iTryCount = 0;
        Move:
            try
            {
                if (File.Exists(ImagePath) == true)
                {
                    // string delPgNo = hdndeletePgNo.Value;
                    IList<int> selectedPageNumbers = new List<int>();

                    if (ImagePath.ToLower().Contains(".tif"))
                    {
                        using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(ImagePath))
                        {
                            sTotalPagenumber = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                            imgbg.Dispose();
                        }
                        for (int i = 1; i <= sTotalPagenumber; i++)
                        {
                            //Remove the selected page and merge the all other pages as new file
                            if (delPgNo != i.ToString())
                                selectedPageNumbers.Add(i);
                        }


                        System.Drawing.Image[] splittedFiles = null;
                        splittedFiles = splitTiffImageToSeparateImages(ImagePath, selectedPageNumbers);
                        bool res = saveMultipage(splittedFiles, ImagePath, "TIFF", out string sCheckFileNotFoundException);
                        if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                            return sCheckFileNotFoundException;
                        }
                        if (ImagePath != null && ImagePath != string.Empty)
                        {
                            using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(ImagePath))
                            {
                                sTotalPagenumber = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                imgbg.Dispose();
                            }
                            //LoadFiles();
                            //OpenFile(ImagePath, sTotalPagenumber.ToString());
                        }
                    }
                }

                return "success";
            }
            catch (Exception em)
            {
                //if (em.Message.Contains("used by another process"))
                //{
                if (iTryCount < 3)
                {
                    iTryCount++;
                    Thread.Sleep(2000);
                    goto Move;
                }
                else
                {
                    throw em;
                }

                //}
            }

        }

        protected void hdnMoventoNonmedicalFolder_Click(object sender, EventArgs e)
        {




            //string path = string.Empty;
            //string filePath = hdnfilenamedelete.Value;
            //string facilityName = ddSelectedFacility.Text.ToString();
            //string scandate = dtpScannedDate.Value.ToString().Trim();
            //string ImagePathNonMedical = string.Empty;

            StringBuilder filePath = new StringBuilder();
            filePath.Append(hdnfilenamedelete.Value);

            StringBuilder ImagePathNonMedical = new StringBuilder();
            //For Fax also v need to move the the non medical file to Normal FTP server from FAX server.
            ImagePathNonMedical.Append(ConfigurationManager.AppSettings["ScanningPath_Local"].ToString() + "\\" + ddSelectedFacility.Text.ToString() + ConfigurationManager.AppSettings["IndexingNonMedicaFolder"].ToString() + Convert.ToDateTime(dtpScannedDate.Value.ToString().Trim()).ToString("yyyyMMdd") + "\\" + Path.GetFileName(filePath.ToString()));
            int iTryCount = 0;
        Move:
            try
            {
                if (filePath.Length != 0 && File.Exists(filePath.ToString()) == true)
                {
                    DirectoryInfo DirNonMedFolder = new DirectoryInfo(Path.GetDirectoryName(ImagePathNonMedical.ToString()));
                    if (!DirNonMedFolder.Exists)
                        Directory.CreateDirectory(DirNonMedFolder.FullName);

                    //File moved to NON-Medical folder.
                    File.Move(filePath.ToString(), ImagePathNonMedical.ToString());

                    //Saved in File mngt Index table for Tracking the Non medical files and the  talend moved the files to respective Dr. Folder.
                    FileManagementIndexManager fileManagementIndexmanager = new FileManagementIndexManager();
                    IList<FileManagementIndex> filemanagementIndexList = new List<FileManagementIndex>();
                    FileManagementIndex filemanagementIndex = new FileManagementIndex();
                    filemanagementIndex.Created_By = ClientSession.UserName;
                    filemanagementIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    filemanagementIndex.Source = "NON MEDICAL";
                    filemanagementIndex.Relationship = "READY FOR MOVE";
                    filemanagementIndex.Document_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtpScannedDate.Value + " 07:30:00"));
                    filemanagementIndex.Human_ID = 0;
                    filemanagementIndex.File_Path = ImagePathNonMedical.ToString();
                    filemanagementIndex.Encounter_ID = 0;
                    filemanagementIndex.Is_Delete = "N";
                    filemanagementIndexList.Add(filemanagementIndex);
                    fileManagementIndexmanager.SaveFileManagementIndexforNonMedicalFiles(filemanagementIndexList);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Filemovedalert", "StopLoadOnUploadFile();DisplayErrorMessage('114020');", true);
                }
            }
            catch (Exception em)
            {
                if (iTryCount < 3)
                {
                    iTryCount++;
                    Thread.Sleep(2000);
                    goto Move;
                }
                else
                {
                    throw em;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "deletefile", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
        }

        protected void ddlSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilityList(ddlSourceType.Text);
            if (rdbRemoteDir.Checked == true)
            {
                if (ddlSourceType.Text == "FAX")
                    btnMovetoNonMedicalFolder.Visible = false;// hide button as per git lab id 1099
                else
                    btnMovetoNonMedicalFolder.Visible = false;
            }
        }


        public void LoadFacilityList(string SourceType)
        {
            IList<FacilityLibrary> facList;
            //facList = ApplicationObject.facilityLibraryList;//FacilityMngr.GetFacilityList();
            var fac = from f in ApplicationObject.facilityLibraryList where f.Legal_Org == ClientSession.LegalOrg select f;
            facList = fac.ToList<FacilityLibrary>();
            this.ddSelectedFacility.Items.Clear();
            if (facList != null)
            {
                for (int i = 0; i < facList.Count; i++)
                {
                    System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                    cboItem.Text = facList[i].Fac_Name;
                    // cboItem.Value = facList[i].Id.ToString();
                    this.ddSelectedFacility.Items.Add(cboItem);
                }
            }

            //For Fax we take the Folders from Staticlookup XML

            if (SourceType == "FAX")
            {
                //CAP-2787
                //XmlDocument xmldoc = new XmlDocument();
                //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                if (File.Exists(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.json")) == true)
                {
                    //xmldoc.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml"));
                    StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
                    //XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("FaxFolderNameList");
                    var lstFaxFolderList = staticLookupList.FaxFolderNameList;
                    if ((lstFaxFolderList?.Count??0) > 0)
                    {
                        foreach (var faxFolderList in lstFaxFolderList)
                        {
                            System.Web.UI.WebControls.ListItem cboItem = new System.Web.UI.WebControls.ListItem();
                            cboItem.Text = faxFolderList.FolderName;
                            this.ddSelectedFacility.Items.Add(cboItem);
                        }
                    }
                }
            }

            if (ClientSession.FacilityName != null && ClientSession.FacilityName != "" && ddSelectedFacility != null)
            {
                ddSelectedFacility.Text = ClientSession.FacilityName;
            }
        }

        protected void btnUpdateCancelok_Click(object sender, EventArgs e)
        {
            btnSave.Text = "Add";
            btnClearAll.Text = "Reset";
            upPatientDetails.Update();
            //txtSelectedPages.Value = "";
            //if (cboIs_Interperated.Items != null && cboIs_Interperated.Items.Count > 0)
            //{
            //    if (cboIs_Interperated.SelectedIndex > -1)
            //    {
            //        cboIs_Interperated.ClearSelection();
            //    }
            //    cboIs_Interperated.SelectedIndex = 0;
            //}
            //dtpSpecCollection.Value = "";
            //HideOrders();
            //if (cboStandingOrders.Items.Count > 0)
            //{
            //    cboStandingOrders.Items.Clear();
            //}
            //if (cboPhysician.Items.Count > 0)
            //{
            //    cboPhysician.Items.Clear();
            //}
            //if (cboLab.Items.Count > 0)
            //{
            //    cboLab.Items.Clear();
            //}

            //btnSave.Enabled = false;
            ////rdbAll.Checked = true;
            //if (grdIndexing.Items.Count == 0)
            //{
            //    btnMoveToNextProcess.Disabled = true;
            //}
            //else
            //{
            //    btnMoveToNextProcess.Disabled = false;
            //}


            //PageLabel.InnerText = "/0";
            //PageBox.Value = "0";
            //LoadDocType();
            //rdbAll.Checked = true;
            //btnMoveToNextProcess.Disabled = true;
            //txtSelectedPages.Disabled = true;
            //rdbPageRange.Checked = false;
        }

        protected void hdnSingleFileCheck_Click(object sender, EventArgs e)
        {
            //Remove the last index file from browse 
            if (Session["BrowseLoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Count > 0 && hdnLastIndexFileName.Value != "")//(string)Session["LastIndexFileName"] != null)
            {
                //  IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Where(a => a.Scanned_File_Name == (string)Session["LastIndexFileName"]).ToList<Scan>();
                IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["BrowseLoadList"]).Where(a => a.Scanned_File_Name == hdnLastIndexFileName.Value).ToList<Scan>();
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    foreach (Scan item in lstTemp)
                    {
                        ((IList<Scan>)Session["BrowseLoadList"]).Remove(item);
                    }
                }
            }
            grdIndexing.DataSource = new string[] { };
            grdIndexing.DataBind();
            // string sFileName = HttpUtility.UrlDecode(hdnFileName.Value);
            // string pageCount = hdnPagecount.Value;
            hdnFileName.Value = HttpUtility.UrlDecode(hdnFileName.Value);
            //Session["Page_Count"] = Convert.ToInt16(pageCount);
            //Session["FileName"] = Path.GetFileName(sFileName);
            //Session["ImagePath"] = sFileName;
            //Session["FileID"] = hdnFileID.Value;
            //LoadFiles();
            //OpenFile(sFileName, pageCount);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "deletefile", " { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);

        }

        [WebMethod(EnableSession = true)]
        public static string LoadGrid(string IsRemote, string TypeofScan, string sSelectedDate, string sFacility, string sLastIndexFile)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //string filePath = string.Empty;
            StringBuilder filePath = new StringBuilder();

            //string sFolderStructure = string.Empty;
            StringBuilder sFolderStructure = new StringBuilder();

            //string sFacilityName = string.Empty;
            StringBuilder sFacilityName = new StringBuilder();





            IList<string> lstDocuments = new List<string>();
            IList<Scan> lstScanList = new List<Scan>();
            //string sFullFilePath = string.Empty;
            StringBuilder sFullFilePath = new StringBuilder();

            //string Notification = string.Empty;
            StringBuilder Notification = new StringBuilder();


            if (IsRemote == "true")
            {
                if (TypeofScan.ToUpper() == "SCAN")
                {
                    filePath.Append(ConfigurationManager.AppSettings["ScanningPath_Local"]);
                    sFolderStructure.Append(sFacility + "\\Scanned_Images\\");
                    sFacilityName.Append(sFacility);
                }
                else
                {
                    filePath.Append(ConfigurationManager.AppSettings["ScanningPath_Fax"] + ConfigurationManager.AppSettings["ftpFaxpath"]);
                    if (sFacility.Contains("~"))
                    {
                        sFolderStructure.Append(sFacility.Split('~')[0].Trim() + ConfigurationManager.AppSettings["FaxpathoutputIndexing"]);
                        sFacilityName.Append(sFacility.Split('~')[1].Trim());
                    }
                    else
                    {
                        sFolderStructure.Append(sFacility.Replace(" ", "_").Replace("#", "").Replace(",", "") + ConfigurationManager.AppSettings["FaxpathoutputIndexing"]);
                        sFacilityName.Append(sFacility);
                    }
                }
                DirectoryInfo onlineChartdirInfo = new DirectoryInfo(filePath + "\\" + sFolderStructure);
                if (onlineChartdirInfo.Exists)
                {

                    FileInfo[] onlineChartfileInfo = null;
                    DateTime selectedDate = Convert.ToDateTime(sSelectedDate);
                    onlineChartdirInfo = new DirectoryInfo(filePath + "\\" + sFolderStructure + selectedDate.ToString("yyyyMMdd"));
                    sFullFilePath.Append(filePath + "\\" + sFolderStructure + selectedDate.ToString("yyyyMMdd") + "\\");
                    if (onlineChartdirInfo.Exists)
                    {
                        onlineChartfileInfo = onlineChartdirInfo.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                        string[] sFileExtension = { ".tif", ".jpeg", ".png", ".jpg", ".bmp", ".pdf", ".tiff" };
                        if (onlineChartfileInfo.Count() > 0)
                        {
                            foreach (FileInfo fi in onlineChartfileInfo)
                            {
                                //string sExtension = Path.GetExtension(fi.FullName).ToLower();
                                if (sFileExtension.Contains(Path.GetExtension(fi.FullName).ToLower()))
                                {
                                    lstDocuments.Add(fi.ToString());
                                }
                            }
                            if (lstDocuments.Count > 0)
                            {
                                ScanManager scanProxy = new ScanManager();
                                lstScanList = scanProxy.GetOnlineDocumentsList(sFacilityName.ToString(), lstDocuments.ToArray(), selectedDate);

                                if (lstScanList.Count > 0)
                                {
                                    HttpContext.Current.Session["LoadList"] = lstScanList;
                                }
                                //Remove the last index file from Remote 
                                if (HttpContext.Current.Session["LoadList"] != null && ((IList<Scan>)HttpContext.Current.Session["LoadList"]).Count > 0 && sLastIndexFile != "")//(string)Session["LastIndexFileName"] != null)
                                {
                                    IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["LoadList"]).Where(a => a.Scanned_File_Name == Path.GetFileName(sLastIndexFile)).ToList<Scan>();
                                    if (lstTemp != null && lstTemp.Count > 0)
                                    {
                                        foreach (Scan item in lstTemp)
                                        {
                                            ((IList<Scan>)HttpContext.Current.Session["LoadList"]).Remove(item);
                                        }
                                    }
                                    HttpContext.Current.Session["LoadList"] = lstScanList = (IList<Scan>)HttpContext.Current.Session["LoadList"];
                                }

                            }
                            if (lstScanList.Count == 0)
                                Notification.Append("No Files to load in the selected directory!");
                        }
                        else
                        {
                            Notification.Append("No Files to load in the selected directory!");
                        }
                    }
                    else
                    {
                        Notification.Append("Scanning Folder doesn't Exist!");
                    }
                }
            }
            else
            {
                filePath.Append(ConfigurationManager.AppSettings["ScanningPath_Local"]);
                sFolderStructure.Append(ClientSession.FacilityName + "\\Scanned_Images\\");
                sFullFilePath.Append(filePath.ToString() + "\\" + sFolderStructure + DateTime.Now.ToString("yyyyMMdd") + "\\Local_Indexing_File\\");
                lstScanList = new List<Scan>();
                lstScanList = ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]);

                //Remove the last index file from browse 
                if (HttpContext.Current.Session["LocalUploadFiles"] != null && ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Count > 0 && sLastIndexFile != "")//(string)Session["LastIndexFileName"] != null)
                {
                    IList<Scan> lstTemp = ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Where(a => a.Scanned_File_Name == Path.GetFileName(sLastIndexFile)).ToList<Scan>();
                    if (lstTemp != null && lstTemp.Count > 0)
                    {
                        foreach (Scan item in lstTemp)
                        {
                            ((IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"]).Remove(item);
                        }
                    }
                    HttpContext.Current.Session["LocalUploadFiles"] = lstScanList = (IList<Scan>)HttpContext.Current.Session["LocalUploadFiles"];
                }

            }
            //string bIsDelete = "false";
            StringBuilder bIsDelete = new StringBuilder();
            bIsDelete.Append("false");
            if (ConfigurationManager.AppSettings["EnableDeleteIndexing"] != null)
            {
                string[] Userrole = ConfigurationManager.AppSettings["EnableDeleteIndexing"].ToString().ToUpper().Split('|');
                if (Userrole.Contains(ClientSession.UserRole.ToUpper()))
                {
                    bIsDelete = new StringBuilder();
                    bIsDelete.Append("true");
                }
                else
                {
                    bIsDelete = new StringBuilder();
                    bIsDelete.Append("false");
                }

            }
            var result = new { FilesList = lstScanList, IsDeleteEnable = bIsDelete.ToString(), FilePath = sFullFilePath.ToString().Replace("\\", "\\\\"), IsNotification = Notification.ToString() };



            return JsonConvert.SerializeObject(result);

        }

        [WebMethod(EnableSession = true)]
        public static string OpenGridFile(string filename, string filepath)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            StringBuilder sFullPath = new StringBuilder();
            sFullPath.Append(Path.Combine(filepath, filename));

            int pageCount = 1;
            try
            {
                if (File.Exists(sFullPath.ToString()))
                {
                    if (Path.GetExtension(filename).ToLower() == ".pdf")
                    {
                        using (FileStream fs = new FileStream(sFullPath.ToString(), FileMode.Open, FileAccess.Read))
                        {
                            StreamReader sr = new StreamReader(fs);
                            // string pdf = sr.ReadToEnd();
                            Regex rx = new Regex(@"/Type\s*/Page[^s]");
                            MatchCollection match = rx.Matches(sr.ReadToEnd());
                            pageCount = match.Count;
                            if (pageCount == 0)
                            {
                                PdfReader pdfReader = new PdfReader(sFullPath.ToString());
                                pageCount = pdfReader.NumberOfPages;
                            }
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                    else
                    {
                        using (System.Drawing.Image imgbg = System.Drawing.Image.FromFile(sFullPath.ToString()))
                        {
                            pageCount = imgbg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                            imgbg.Dispose();
                        }
                    }
                }
            }
            catch
            {

            }

            return pageCount.ToString();
        }

        [WebMethod(EnableSession = true)]
        public static string PendingFile()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HttpContext.Current.Session["FileName"] = null;
            HttpContext.Current.Session["IndexList"] = null;
            return "Success";
        }
        void LoadEncPhysician()
        {
            dtpDocumentDate.Disabled = true;
            lblDocDate.Attributes.Remove("class");
            lblDocDate.Attributes.Add("class", "spanstyle");
            DocDateStar.Visible = false;

            ddEncPhyName.Enabled = true;
            lblDosPhy.Attributes.Remove("class");
            lblDosPhy.Attributes.Add("class", "MandLabelstyle");
            DosPhyStar.Visible = true;
            EncMsg.Visible = true;
            if (hdnHumanID.Value != "")
            {
                EncounterManager objEncMngr = new EncounterManager();
                IList<string> lstEnc = new List<string>();
                string sMonths = ConfigurationManager.AppSettings["IndexingEncounterDOSMonths"];
                lstEnc = objEncMngr.GetEncounterListForIndexing(Convert.ToUInt64(hdnHumanID.Value), sMonths);
                if (lstEnc.Count() > 0)
                {
                    ddEncPhyName.Items.Clear();
                    ddEncPhyName.Items.Add(string.Empty);
                    string sEncDate = string.Empty;
                    string sLatestEncDate = string.Empty;
                    foreach (string sEnc in lstEnc)
                    {
                        ListItem EncPhy = new ListItem();
                        sEncDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(sEnc.Split('^')[0].Split('~')[0])).ToString("dd-MMM-yyyy hh:mm tt");
                        if (sLatestEncDate == string.Empty)
                            sLatestEncDate = sEncDate;
                        EncPhy.Text = sEncDate + " ~ " + sEnc.Split('^')[0].Split('~')[1];
                        EncPhy.Value = sEnc.Split('^')[1];
                        ddEncPhyName.Items.Add(EncPhy);

                    }
                    if (Convert.ToDateTime(sLatestEncDate).ToString("dd-MMM-yyyy") == Convert.ToDateTime(dtpDocumentDate.Value).ToString("dd-MMM-yyyy"))
                        ddEncPhyName.SelectedIndex = 1;
                }
                else
                {
                    ddEncPhyName.Items.Add(string.Empty);
                }
            }
            else
            {
                ddEncPhyName.Items.Add(string.Empty);
            }

        }

        public void ShowAllphysicianforCMGAncillary()
        {
            cboPhysician.Items.Clear();
            XDocument xmlDocumentType = null;
            //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml")))
            //    xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            //CAP-2781
            ListItem liDropdown = null;
            IList<ListItem> liComboItems = new List<ListItem>();
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null)
            {
                foreach (var facility in physicianFacilityMappingList.PhysicianFacility)
                {
                    //if (elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                    //{
                    //Cap - 1343
                    //var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == elements.Attribute("name").Value select f;
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == facility.name && facility.Legal_Org == ClientSession.LegalOrg select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                    {
                        foreach (var phyItems in facility.Physician)
                        {
                            StringBuilder phyName = new StringBuilder();
                            StringBuilder username = new StringBuilder();
                            StringBuilder prefix = new StringBuilder();
                            StringBuilder firstname = new StringBuilder();
                            StringBuilder middlename = new StringBuilder();
                            StringBuilder lastname = new StringBuilder();
                            StringBuilder suffix = new StringBuilder();
                            StringBuilder phyID = new StringBuilder();
                            if (phyItems.username != null)
                                username.Append(phyItems.username);
                            if (phyItems.prefix != null)
                                prefix.Append(phyItems.prefix);
                            if (phyItems.firstname != null)
                                firstname.Append(phyItems.firstname);
                            if (phyItems.middlename != null)
                                middlename.Append(phyItems.middlename);
                            if (phyItems.lastname != null)
                                lastname.Append(phyItems.lastname);
                            if (phyItems.suffix != null)
                                suffix.Append(phyItems.suffix);
                            //if (phyItems.Attribute("phyID").Value != null)
                            // phyID = phyItems.Attribute("phyID").Value;
                            if (phyItems.ID != null)
                                phyID.Append(phyItems.ID);

                            if (prefix.Length != 0)
                            {
                                phyName.Append(prefix.ToString());
                            }
                            if (firstname.Length != 0)
                            {
                                phyName.Append(firstname.ToString());
                            }
                            if (middlename.Length != 0)
                            {
                                phyName.Append(middlename.ToString());
                            }
                            if (lastname.Length != 0)
                            {
                                phyName.Append(lastname.ToString());
                            }
                            if (suffix.Length != 0)
                            {
                                phyName.Append(suffix.ToString());
                            }
                            if (username.Length != 0)
                            {
                                liDropdown = new ListItem(username.ToString() + "-" + phyName.ToString(), phyID.ToString());
                                liDropdown.Attributes.Add("default", "true");
                                liComboItems.Add(liDropdown);
                            }
                        }
                    }
                }
            }
            IList<ListItem> sortlst = liComboItems.OrderByDescending(x => x.Attributes["default"]).ToList();
            sortlst = sortlst.OrderBy(x => x.Text).Distinct().ToList();
            cboPhysician.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboPhysician.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();

        }
        public void ShowAllOrderingphysicianforCMGAncillary()
        {
            cboOrderPhysician.Items.Clear();
            XDocument xmlDocumentType = null;
            //if (File.Exists(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml")))
            //    xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            ListItem liDropdown = null;
            IList<ListItem> liComboItems = new List<ListItem>();
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null)
            {
                foreach (var facility in physicianFacilityMappingList.PhysicianFacility)
                {
                    //if (elements.Attribute("name").Value.ToUpper() != ConfigurationManager.AppSettings["CMGFacilityName"].Trim().ToUpper())
                    //{
                    //Cap - 1119
                    //var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == elements.Attribute("name").Value select f;
                    var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == facility.name && facility.Legal_Org == ClientSession.LegalOrg select f;
                    IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                    if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                    {
                        foreach (var phyItems in facility.Physician)
                        {
                            StringBuilder phyName = new StringBuilder();
                            StringBuilder username = new StringBuilder();
                            StringBuilder prefix = new StringBuilder();
                            StringBuilder firstname = new StringBuilder();
                            StringBuilder middlename = new StringBuilder();
                            StringBuilder lastname = new StringBuilder();
                            StringBuilder suffix = new StringBuilder();
                            StringBuilder phyID = new StringBuilder();
                            if (phyItems.username != null)
                                username.Append(phyItems.username);
                            if (phyItems.prefix != null)
                                prefix.Append(phyItems.prefix);
                            if (phyItems.firstname != null)
                                firstname.Append(phyItems.firstname);
                            if (phyItems.middlename != null)
                                middlename.Append(phyItems.middlename);
                            if (phyItems.lastname != null)
                                lastname.Append(phyItems.lastname);
                            if (phyItems.suffix != null)
                                suffix.Append(phyItems.suffix);
                            //if (phyItems.Attribute("phyID").Value != null)
                            // phyID = phyItems.Attribute("phyID").Value;
                            if (phyItems.ID != null)
                                phyID.Append(phyItems.ID);

                            if (prefix.Length != 0)
                            {
                                phyName.Append(prefix.ToString());
                            }
                            if (firstname.Length != 0)
                            {
                                phyName.Append(firstname.ToString());
                            }
                            if (middlename.Length != 0)
                            {
                                phyName.Append(middlename.ToString());
                            }
                            if (lastname.Length != 0)
                            {
                                phyName.Append(lastname.ToString());
                            }
                            if (suffix.Length != 0)
                            {
                                phyName.Append(suffix.ToString());
                            }
                            if (username.Length != 0)
                            {
                                liDropdown = new ListItem(username.ToString() + "-" + phyName.ToString(), phyID.ToString());
                                liDropdown.Attributes.Add("default", "true");
                                liComboItems.Add(liDropdown);
                            }
                        }
                    }

                }
            }
            IList<ListItem> sortlst = liComboItems.OrderByDescending(x => x.Attributes["default"]).ToList();
            sortlst = sortlst.OrderBy(x => x.Text).Distinct().ToList();
            cboOrderPhysician.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboOrderPhysician.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();

        }

    }
}

