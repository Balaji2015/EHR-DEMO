using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web;
using Telerik.Web.UI;
using Acurus.Capella.UI;
using Telerik.Web.UI.Upload;
using Acurus.Capella.Core.DTO;
using System.Diagnostics;
using Ionic.Zip;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Globalization;


namespace Acurus.Capella.UI
{
    public partial class frmExamPhotos : System.Web.UI.Page
    {
        IList<FileManagementIndex> file_exam_lst = new List<FileManagementIndex>();
        int prevNum = 0;
        string lastNumToAdd = string.Empty;
        ABI_Results objABI_Result;

        FileManagementDTO ObjFileDto = new FileManagementDTO();
        bool IsCmg = false;
        ulong OrderSubmit_ID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(Invisiblebtn);
            dtpTestTakenDate.MaxDate = DateTime.Now;
            if (ClientSession.UserRole == "Medical Assistant")
            {
                pbLibrary.Enabled = false;
                pbLibrary.ImageUrl = "~/Resources/Database Disable.png";
            }
            if (ClientSession.UserCurrentProcess == "DISABLE")
            {
                pbLibrary.ImageUrl = "~/Resources/Database Disable.png";
                GridButtonColumn grdbtnCol = (GridButtonColumn)grdPhoto.Columns[8];
                grdbtnCol.ImageUrl = "~/Resources/Down_Disabled.bmp";

                grdbtnCol = (GridButtonColumn)grdPhoto.Columns[9];
                grdbtnCol.ImageUrl = "~/Resources/Down_Disabled.bmp";
            }
            if (!IsPostBack)
            {

                if (Request["frompatientChart"] == null)
                {
                    FileManagementIndexManager objfilemanager = new FileManagementIndexManager();
                    if (Request["type"] != null && Request["type"].ToString() == "Result Upload")
                    {
                        hdnFormType.Value = Convert.ToString(Request["type"]);
                        if (Request["OrderSubmit_ID"] != null)
                            OrderSubmit_ID = Convert.ToUInt64(Request["OrderSubmit_ID"].ToString());
                        if (ClientSession.HumanId != 0)
                            hdnHumanId.Value = ClientSession.HumanId.ToString();
                        UploadImage.AllowedFileExtensions = new string[] { ".png", ".tif", ".jpeg", ".jpg", ".gif", ".docx", ".pdf", ".xls", ".bmp", ".doc", ".xlsx", ".dcm", ".txt", ".ppt", ".pptx" };
                        ObjFileDto = objfilemanager.LoadFileIndex_and_ViewResult(ClientSession.EncounterId, ClientSession.HumanId, OrderSubmit_ID, "ORDER,SCAN", IsCmg);
                        file_exam_lst = ObjFileDto.FileManagementList;
                        if (file_exam_lst.Count > 0)
                        {
                            btnMoveToNextProcess.Enabled = true;
                        }
                        FindFileIndex(file_exam_lst);
                        Session["ExamList"] = file_exam_lst;
                        Panel2.Visible = false;
                        btnMove.Enabled = false;
                    }
                    else
                    {
                        btnMove.Visible = false;
                        btnMoveToNextProcess.Visible = false;

                        //UploadImage.AllowedFileExtensions = new string[] { ".png", ".tif", ".jpeg", ".jpg", ".gif", ".docx", ".pdf", ".xls", ".bmp", ".doc", ".xlsx",".dcm",".txt",".ppt",".pptx"};
                        LoadExam(0);
                        //LoadExam();

                        file_exam_lst = objfilemanager.GetIndexedListByHumanId(ClientSession.HumanId, "Exam"); ;
                        Session["ExamList"] = file_exam_lst;
                        LoadGrid();
                        dtpTestTakenDate.SelectedDate = DateTime.Now;
                        btnSave.Enabled = false;
                    }
                    ClientSession.processCheck = true;
                    SecurityServiceUtility ObjSSU = new SecurityServiceUtility();
                    ObjSSU.ApplyUserPermissions(this.Page);
                    //if (ClientSession.UserRole == "Medical Assistant")
                    //{
                    //    cboPhysicianName.Enabled = true;
                    //    chkShowAllPhysicians.Enabled = true;
                    //}
                    //else
                    //{
                    //    cboPhysicianName.Enabled = false;
                    //    chkShowAllPhysicians.Enabled = false;
                    //    LoadExam(0);
                    //}

                    if (Request["type"] != null)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Resize();", true);

                        grdPhoto.Height = 5;
                        lblGroupType1.Visible = false;
                        lblGroupType2.Visible = false;
                        lblTestTakenOn1.Visible = false;
                        lblTestTakenOn2.Visible = false;
                        cboGroupType.Enabled = false;
                        cboGroupType.Visible = false;
                        cboGroupType.Text = "  ";
                        dtpTestTakenDate.Visible = false;
                        pbLibrary.Visible = false;
                        btnMove.Visible = true;
                        grdPhoto.Visible = false;
                        btnMove.Enabled = false;
                        btnSave.Enabled = true;
                        UploadImage.Enabled = true;
                        btnClear.Enabled = true;

                        if (Request["IsCmg"] == null)
                            IsCmg = true;

                        if (Request["OrderSubmit_ID"] != null)
                            OrderSubmit_ID = Convert.ToUInt64(Request["OrderSubmit_ID"].ToString());
                    }

                    btnSave.Enabled = false;
                    if (cboGroupType.Enabled == true && ClientSession.UserRole != "Physician")
                    {
                        cboPhysicianName.Enabled = true;
                        chkShowAllPhysicians.Enabled = true;
                    }
                    else
                    {
                        cboPhysicianName.Enabled = false;
                        chkShowAllPhysicians.Enabled = false;
                        LoadExam(0);
                    }
                }
                else
                {
                    if (Request["frompatientChart"] != null && Request["frompatientChart"].ToString().ToUpper() == "YES")
                    {
                        btnMoveToNextProcess.Visible = false;
                        cboPhysicianName.Enabled = true;
                        chkShowAllPhysicians.Enabled = true;
                    }
                    if (Request["FileMngID"] != null)
                        hdnfileid.Value = Request["FileMngID"].ToString();//For Bug Id 53266
                    btnMove.Visible = false;
                    openimagefrompatientchart(Request["DocDate"].ToString(), Request["DocType"].ToString());
                    dtpTestTakenDate.SelectedDate = DateTime.Now;
                    dtpTestTakenDate.MaxDate = DateTime.Now;
                }
            }
            //string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            //if (sAncillary.Trim() == ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name.Trim().ToUpper())
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                chkShowAllPhysicians.Checked = true;
            }
            if (Request["frompatientChart"] == null)
            {
                FillCboPhysician(ClientSession.FacilityName.ToUpper());
                LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
                if (IsPostBack && Request["type"] != null && Request["type"].ToString() != "Result Upload") { btnSave.Enabled = false; }
                if (Request["hdnMoveToMA"] != null)
                {
                    hdnMoveToMAID.Value = Request["hdnMoveToMA"].ToString();
                }
                if (Request["CurrentProcess"] != null)
                {
                    hdnCurrentProcess.Value = Request["CurrentProcess"].Replace(" _", "_").ToString();
                    if ((hdnCurrentProcess.Value == "MA_REVIEW" || hdnCurrentProcess.Value == "BILLING_WAIT") && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                    {
                        btnMoveToNextProcess.Enabled = false;
                    }
                    else if ((hdnCurrentProcess.Value == "RESULT_REVIEW" || hdnCurrentProcess.Value == "BILLING_WAIT") && ClientSession.UserRole == "Medical Assistant")
                    {
                        btnMoveToNextProcess.Enabled = false;
                    }
                }

                RadViewer.Title = "Image Viewer";
                hdnHumanId.Value = ClientSession.HumanId.ToString();
                hdnOrderSubmitId.Value = Request["OrderSubmit_ID"];
                if (Request["hdnOrder"] != null)
                {
                    hdnOrder.Value = Request["hdnOrder"].ToString();
                }
                //added by balaji.TJ
                if (Session["ResultExist"] != null && ((bool)Session["ResultExist"]))
                {
                    btnMove.Enabled = true;
                    //Session["ResultExist"] = null;
                }
                else
                {
                    btnMove.Enabled = false;
                }
            }
            if (chkShowAllPhysicians.Checked)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();", true);
            }
            if (hdnLibraryCheck.Value != null)
            {
                if (hdnLibraryCheck.Value.ToString() == "true")
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }


        }


        public void openimagefrompatientchart(string filedate, string doctype)
        {
            FileManagementIndexManager objfilemanager = new FileManagementIndexManager();
            FilePaths.Clear();
            fExten.Clear();
            bool isViewable = false;
            hdnDate.Value = filedate;
            hdnDocumentType.Value = doctype;
            hdnHumanId.Value = ClientSession.HumanId.ToString();
            file_exam_lst = objfilemanager.GetIndexedListByHumanId(ClientSession.HumanId, "Exam"); ;
            Session["ExamList"] = file_exam_lst;
            IList<FileManagementIndex> File_ListForDownlaod = new List<FileManagementIndex>();
            IList<FileManagementIndex> lstTemp = (from doc in file_exam_lst
                                                  where (doc.Document_Type == hdnDocumentType.Value) && (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == Convert.ToDateTime(hdnDate.Value).ToString("dd-MMM-yyyy"))
                                                  select doc).ToList<FileManagementIndex>();
            File_ListForDownlaod = File_ListForDownlaod.Concat(lstTemp).ToList();

            //if (lstTemp.Count > 1)
            //{
            //    DownloadFile();
            //}
            //else
            {
                foreach (FileManagementIndex item in lstTemp)
                {
                    fExten.Add(item.File_Path.Substring(item.File_Path.LastIndexOf('.')).ToString().ToUpper());
                }
                fExten = fExten.ToList<string>();
                //fExten = fExten.Distinct().ToList<string>();
                // fExten = fExten.ToList<string>();

                //if (fExten.Count > 1)
                //{
                //    isViewable = false;
                //}
                //else
                //{
                //    isViewable = true;
                //}
                if (fExten.Count() == 1 && fExten[0].ToUpper() == ".PDF")
                {
                    isViewable = true;
                }
                else
                {
                    for (int k = 0; k < fExten.Count; k++)
                    {
                        if (fExten[k] == ".JPG" || fExten[k] == ".PNG" || fExten[k] == ".JPEG" || fExten[k] == ".BMP" || fExten[k] == ".PDF")
                        {
                            isViewable = true;
                        }
                        else
                        {
                            isViewable = false;
                            break;
                        }
                    }
                }
                if (!isViewable)
                {
                    DownloadFile();
                }
                else
                {
                    string target = string.Empty;
                    string Notes = string.Empty;
                    foreach (FileManagementIndex item in lstTemp)
                    {
                        target += item.File_Path.ToString() + "|";
                        Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                    }
                    string sDoctype = hdnDocumentType.Value; //e.Item.Cells[3].Text.ToString();
                    string sDocdate = hdnDate.Value; //e.Item.Cells[5].Text.ToString();
                    IList<string> ExamString = new List<string>();
                    Session.Add("ExamData", target);
                    Session.Add("NotesExam", Notes);
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Open", "OpenViewerMaximized('" + sDoctype + "');", true);
                }
            }
            FillCboPhysician(ClientSession.FacilityName.ToUpper());
            LoadExam(0);
            LoadGrid();
            //if (chkShowAllPhysicians.Checked)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();", true);
            //}
            //  divform.Visible = false;
        }


        public void FillCboPhysician(string sFacility_Name)
        {
            bool Is_CMG_Ancillary = false;

            IList<string> PhyIDlist = new List<string>();
            Dictionary<string, string> hashUser = new Dictionary<string, string>();
            //string Ancilliary_FacilityName = string.Empty;
            //if (System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"] != null)
            //    Ancilliary_FacilityName = System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"].ToString();

            //if (Ancilliary_FacilityName.ToString().ToUpper() == ClientSession.FacilityName.ToString().ToUpper())
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName.ToString() select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                Is_CMG_Ancillary = true;
            }
            cboPhysicianName.Items.Clear();
            /* Code Block to populate physician list */
            // bool showAll = true;
            XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            //  StaticLookup objStatics = null;
            ListItem liDropdown = null;
            IList<string> PhyASStIDlist = new List<string>();
            IList<string> PhyIDList = new List<string>();
            int i = 0;

            foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyAsstList").Elements())
            {
                //PhyASStIDlist.Add(elements.Attribute("ID").Value);
                //i++;
                PhyASStIDlist.Add(elements.Attribute("ID").Value);
                if (elements.Attribute("ID").Value.ToString() == ClientSession.PhysicianId.ToString())
                {
                    PhyIDlist.Add(elements.Attribute("Physician_ID").Value);
                    i++;
                }
            }

            IList<ListItem> liComboItems = new List<ListItem>();
            //#GitLab #3813
            //foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyList").Elements())
            //{
            //    string xmlValue = elements.Attribute("name").Value;
            //    if (xmlValue.ToUpper() == ClientSession.FacilityName.ToUpper())
            //    {
            //       // if (ClientSession.FillEncounterandWFObject.EncRecord.Facility_Name.ToUpper() != Ancilliary_FacilityName.Trim().ToUpper() && xmlValue.ToUpper() != Ancilliary_FacilityName.Trim().ToUpper())//For bug Id  52933
            //        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
            //        {
            //            foreach (XElement phyItems in elements.Elements())
            //            {

            //                string phyName = string.Empty;

            //                string username = phyItems.Attribute("username").Value;
            //                string prefix = phyItems.Attribute("prefix").Value;
            //                string firstname = phyItems.Attribute("firstname").Value;
            //                string middlename = phyItems.Attribute("middlename").Value;
            //                string lastname = phyItems.Attribute("lastname").Value;
            //                string suffix = phyItems.Attribute("suffix").Value;
            //                string phyID = phyItems.Attribute("ID").Value;
            //                //Old Code
            //                //if (prefix != "")
            //                //{
            //                //    phyName += prefix + " ";
            //                //}
            //                //if (firstname != "")
            //                //{
            //                //    phyName += firstname + " ";
            //                //}
            //                //if (middlename != "")
            //                //{
            //                //    phyName += middlename + " ";
            //                //}
            //                //if (lastname != "")
            //                //{
            //                //    phyName += lastname + " ";
            //                //}
            //                //if (suffix != "")
            //                //{
            //                //    phyName += suffix;
            //                //}

            //                //Gitlab# 2485 - Physician Name Display Change
            //                if (lastname != String.Empty)
            //                    phyName += lastname;
            //                if (firstname != String.Empty)
            //                {
            //                    if (phyName != String.Empty)
            //                        phyName += "," + firstname;
            //                    else
            //                        phyName += firstname;
            //                }
            //                if (middlename != String.Empty)
            //                    phyName += " " + middlename;
            //                if (suffix != String.Empty)
            //                    phyName += "," + suffix;

            //                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (PhyASStIDlist.Contains(phyID)))
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    if (Is_CMG_Ancillary)//BugID:52889 -- Added only distinct PhyLibIds as There are multiple entries for same technician with different machine_technician_Ids -- does not occur for other roles(only for technician)
            //                    {

            //                        if (PhyIDList.IndexOf(phyID) == -1)
            //                        {
            //                            PhyIDList.Add(phyID);
            //                            liDropdown = new ListItem(username + " - " + phyName, phyID);
            //                            liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                            liDropdown.Attributes.Add("default", "true");
            //                            liDropdown.Attributes.CssStyle.Add("display", "");
            //                            liComboItems.Add(liDropdown);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (username != string.Empty)
            //                        {
            //                            //Old Code
            //                            //liDropdown = new ListItem(username + " - " + phyName, phyID);
            //                            //Gitlab# 2485 - Physician Name Display Change
            //                            liDropdown = new ListItem(phyName, phyID);
            //                            liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                            liDropdown.Attributes.Add("default", "true");
            //                            liDropdown.Attributes.CssStyle.Add("display", "");
            //                            liComboItems.Add(liDropdown);
            //                        }
            //                    }

            //                }


            //            }

            //        }


            //    }
            //    else
            //    {
            //        foreach (XElement phyItems in elements.Elements())
            //        {

            //            string phyName = string.Empty;

            //            string username = phyItems.Attribute("username").Value;
            //            string prefix = phyItems.Attribute("prefix").Value;
            //            string firstname = phyItems.Attribute("firstname").Value;
            //            string middlename = phyItems.Attribute("middlename").Value;
            //            string lastname = phyItems.Attribute("lastname").Value;
            //            string suffix = phyItems.Attribute("suffix").Value;
            //            string phyID = phyItems.Attribute("ID").Value;
            //            //Old Code
            //            //if (prefix != "")
            //            //{
            //            //    phyName += prefix + " ";
            //            //}
            //            //if (firstname != "")
            //            //{
            //            //    phyName += firstname + " ";
            //            //}
            //            //if (middlename != "")
            //            //{
            //            //    phyName += middlename + " ";
            //            //}
            //            //if (lastname != "")
            //            //{
            //            //    phyName += lastname + " ";
            //            //}
            //            //if (suffix != "")
            //            //{
            //            //    phyName += suffix;
            //            //}

            //            //Gitlab# 2485 - Physician Name Display Change
            //            if (lastname != String.Empty)
            //                phyName += lastname;
            //            if (firstname != String.Empty)
            //            {
            //                if (phyName != String.Empty)
            //                    phyName += "," + firstname;
            //                else
            //                    phyName += firstname;
            //            }
            //            if (middlename != String.Empty)
            //                phyName += " " + middlename;
            //            if (suffix != String.Empty)
            //                phyName += "," + suffix;

            //            if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (PhyASStIDlist.Contains(phyID)))
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                if (username != string.Empty && phyItems.Attribute("Legal_Org").Value == ClientSession.LegalOrg)
            //                {
            //                    //Old Code
            //                    //liDropdown = new ListItem(username + " - " + phyName, phyID);
            //                    //Gitlab# 2485 - Physician Name Display Change
            //                    liDropdown = new ListItem(phyName, phyID);
            //                    liDropdown.Attributes.Add("default", "false");
            //                    liDropdown.Attributes.Add("FacilityName", xmlValue);
            //                    liDropdown.Attributes.CssStyle.Add("display", "none");
            //                    liComboItems.Add(liDropdown);
            //                }
            //            }

            //        }

            //    }
            //}
            {
                foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyList").Elements())
                {
                    string xmlValue = elements.Attribute("name").Value;
                    // if (xmlValue != string.Empty && xmlValue.ToUpper() == ClientSession.FacilityName.ToUpper())
                    if (elements.Attribute("Legal_Org") != null && xmlValue != string.Empty && sFacility_Name != "" && xmlValue.ToUpper() == sFacility_Name.ToUpper() && elements.Attribute("Legal_Org").Value == ClientSession.LegalOrg)
                    {
                        foreach (XElement phyItems in elements.Elements())
                        {
                            string phyName = string.Empty;
                            string username = string.Empty;
                            string prefix = string.Empty;
                            string firstname = string.Empty;
                            string middlename = string.Empty;
                            string lastname = string.Empty;
                            string suffix = string.Empty;
                            string phyID = string.Empty;

                            if (phyItems.Attribute("username").Value != null)
                                username = phyItems.Attribute("username").Value;
                            if (phyItems.Attribute("prefix").Value != null)
                                prefix = phyItems.Attribute("prefix").Value;
                            if (phyItems.Attribute("firstname").Value != null)
                                firstname = phyItems.Attribute("firstname").Value;
                            if (phyItems.Attribute("middlename").Value != null)
                                middlename = phyItems.Attribute("middlename").Value;
                            if (phyItems.Attribute("lastname").Value != null)
                                lastname = phyItems.Attribute("lastname").Value;
                            if (phyItems.Attribute("suffix").Value != null)
                                suffix = phyItems.Attribute("suffix").Value;
                            if (phyItems.Attribute("ID").Value != null)
                                phyID = phyItems.Attribute("ID").Value;

                            //old code
                            //if (prefix != "")
                            //{
                            //    phyName += prefix + " ";
                            //}
                            //if (firstname != "")
                            //{
                            //    phyName += firstname + " ";
                            //}
                            //if (middlename != "")
                            //{
                            //    phyName += middlename + " ";
                            //}
                            //if (lastname != "")
                            //{
                            //    phyName += lastname + " ";
                            //}
                            //if (suffix != "")
                            //{
                            //    phyName += suffix;
                            //}
                            //Gitlab# 2485 - Physician Name Display Change
                            if (lastname != String.Empty)
                                phyName += lastname;
                            if (firstname != String.Empty)
                            {
                                if (phyName != String.Empty)
                                    phyName += "," + firstname;
                                else
                                    phyName += firstname;
                            }
                            if (middlename != String.Empty)
                                phyName += " " + middlename;
                            if (suffix != String.Empty)
                                phyName += "," + suffix;
                            if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (PhyASStIDlist.Contains(phyID)))
                            {
                                continue;
                            }
                            else
                            {
                                if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                                {
                                    if (PhyIDlist.Contains(phyID) && username != string.Empty && hashUser.ContainsKey(phyID) == false)
                                    {
                                        hashUser.Add(phyID.ToString(), username);
                                        liDropdown = new ListItem(phyName, phyID);
                                        liDropdown.Attributes.Add("FacilityName", xmlValue);
                                        liDropdown.Attributes.Add("default", "true");
                                        liDropdown.Attributes.CssStyle.Add("display", "");
                                        liComboItems.Add(liDropdown);
                                    }
                                }
                                else if (username != string.Empty && hashUser.ContainsKey(phyID) == false)
                                {
                                    //Old Code
                                    //liDropdown = new ListItem(username + " - " + phyName, phyID);
                                    //Gitlab# 2485 - Physician Name Display Change
                                    hashUser.Add(phyID.ToString(), username);
                                    liDropdown = new ListItem(phyName, phyID);
                                    liDropdown.Attributes.Add("FacilityName", xmlValue);
                                    liDropdown.Attributes.Add("default", "true");
                                    liDropdown.Attributes.CssStyle.Add("display", "");
                                    liComboItems.Add(liDropdown);
                                }
                            }
                        }
                    }
                    else if (elements.Attribute("Legal_Org") != null && elements.Attribute("Legal_Org").Value == ClientSession.LegalOrg)
                    {
                        foreach (XElement phyItems in elements.Elements())
                        {
                            string phyName = string.Empty;
                            string username = string.Empty;
                            string prefix = string.Empty;
                            string firstname = string.Empty;
                            string middlename = string.Empty;
                            string lastname = string.Empty;
                            string suffix = string.Empty;
                            string phyID = string.Empty;

                            if (phyItems.Attribute("username").Value != null)
                                username = phyItems.Attribute("username").Value;
                            if (phyItems.Attribute("prefix").Value != null)
                                prefix = phyItems.Attribute("prefix").Value;
                            if (phyItems.Attribute("firstname").Value != null)
                                firstname = phyItems.Attribute("firstname").Value;
                            if (phyItems.Attribute("middlename").Value != null)
                                middlename = phyItems.Attribute("middlename").Value;
                            if (phyItems.Attribute("lastname").Value != null)
                                lastname = phyItems.Attribute("lastname").Value;
                            if (phyItems.Attribute("suffix").Value != null)
                                suffix = phyItems.Attribute("suffix").Value;
                            if (phyItems.Attribute("ID").Value != null)
                                phyID = phyItems.Attribute("ID").Value;

                            //old code
                            //if (prefix != "")
                            //{
                            //    phyName += prefix + " ";
                            //}
                            //if (firstname != "")
                            //{
                            //    phyName += firstname + " ";
                            //}
                            //if (middlename != "")
                            //{
                            //    phyName += middlename + " ";
                            //}
                            //if (lastname != "")
                            //{
                            //    phyName += lastname + " ";
                            //}
                            //if (suffix != "")
                            //{
                            //    phyName += suffix;
                            //}
                            //Gitlab# 2485 - Physician Name Display Change
                            if (lastname != String.Empty)
                                phyName += lastname;
                            if (firstname != String.Empty)
                            {
                                if (phyName != String.Empty)
                                    phyName += "," + firstname;
                                else
                                    phyName += firstname;
                            }
                            if (middlename != String.Empty)
                                phyName += " " + middlename;
                            if (suffix != String.Empty)
                                phyName += "," + suffix;
                            if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && (PhyASStIDlist.Contains(phyID)))
                            {
                                continue;
                            }
                            else
                            {
                                if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && PhyIDlist.Contains(phyID) && username != string.Empty && hashUser.ContainsKey(phyID) == false)
                                {
                                    continue;
                                    //hashUser.Add(phyID.ToString(), username);
                                    //liDropdown = new ListItem(phyName, phyID);
                                    //liDropdown.Attributes.Add("FacilityName", xmlValue);
                                    //liDropdown.Attributes.Add("default", "true");
                                    //liDropdown.Attributes.CssStyle.Add("display", "");
                                    //liComboItems.Add(liDropdown);

                                }
                                else if (username != string.Empty && hashUser.ContainsKey(phyID) == false)
                                {
                                    //Old Code
                                    //liDropdown = new ListItem(username + " - " + phyName, phyID);
                                    //Gitlab# 2485 - Physician Name Display Change
                                    hashUser.Add(phyID.ToString(), username);
                                    liDropdown = new ListItem(phyName, phyID);
                                    liDropdown.Attributes.Add("default", "false");
                                    liDropdown.Attributes.Add("FacilityName", xmlValue);
                                    liDropdown.Attributes.CssStyle.Add("display", "none");
                                    liComboItems.Add(liDropdown);
                                }
                            }

                        }
                    }
                }
            }
            IList<ListItem> sortlst = liComboItems.OrderBy(x => x.Text).ToList();
            // cboPhysicianName.Items.AddRange(sortlst.Distinct().ToArray());//bugId:38683  
            Encounter EncRecord = ClientSession.FillEncounterandWFObject.EncRecord;
            cboPhysicianName.Items.AddRange(sortlst.ToArray());
            ListItem phyEmptyItem = new ListItem("", "0");
            cboPhysicianName.Items.Insert(0, phyEmptyItem);
            liComboItems.OrderBy(x => x.Value).ToList();

            if (!IsPostBack)
            {
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
                {
                    IEnumerable<XElement> MapFacilityPhysician = (from x in xmlDocumentType.Elements("ROOT").Elements("PhyAsstList")
                                                               .Elements("PhysicianAssistant")
                                                                  where x.Attribute("ID").Value == ClientSession.PhysicianId.ToString()
                                                                  select x);
                    if (MapFacilityPhysician.Count() > 0)
                    {
                        ulong uDefaultPhysicianID = 0;
                        //foreach (XElement phyItems in MapFacilityPhysician.Elements())
                        foreach (XElement phyItems in MapFacilityPhysician)
                        {
                            uDefaultPhysicianID = phyItems.Attribute("Default_Physician").Value != "" ? (Convert.ToUInt32(phyItems.Attribute("Default_Physician").Value)) : (0);
                        }
                        ListItem SelectedPhysician = (cboPhysicianName.Items.FindByValue(uDefaultPhysicianID.ToString()));
                        if (SelectedPhysician != null)
                        {
                            hdnindex.Value = cboPhysicianName.Items.IndexOf(SelectedPhysician).ToString();
                            cboPhysicianName.SelectedIndex = Convert.ToInt32(hdnindex.Value);
                            hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
                        }
                    }
                }
                else
                {
                    ListItem SelectedPhysician = (cboPhysicianName.Items.FindByValue(ClientSession.PhysicianId.ToString()));
                    if (SelectedPhysician != null)
                    {
                        hdnindex.Value = cboPhysicianName.Items.IndexOf(SelectedPhysician).ToString();
                        cboPhysicianName.SelectedIndex = Convert.ToInt32(hdnindex.Value);
                        hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
                    }
                }
                //for (int i = 0; i < cboPhysicianName.Items.Count; i++)
                //{
                //  if (Convert.ToUInt32(cboPhysicianName.Items[i].Value) == Convert.ToInt32(EncRecord.Appointment_Provider_ID))
                //    {
                //        cboPhysicianName.SelectedIndex = i;
                //        hdnindex.Value = cboPhysicianName.SelectedIndex.ToString();
                //        hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
                //    }
                //}
            }
            else
            {
                ulong iIndex = 0;
                if (ulong.TryParse(hdnindex.Value, out iIndex) && iIndex != 0)
                {
                    cboPhysicianName.SelectedIndex = Convert.ToInt32(hdnindex.Value);
                    hdnLocalPhy.Value = cboPhysicianName.SelectedValue + '~' + cboPhysicianName.SelectedItem.Text.Split('-')[0];
                }
                else
                {
                    hdnLocalPhy.Value = string.Empty;
                }
            }
            // Session["PhysicianList"] = PhyUserList;

        }
        public void LoadExam(UInt32 EncProvID)
        {
            cboGroupType.Items.Clear();
            IList<UserLookup> userLookup = new List<UserLookup>();
            UserLookupManager localFieldLookupManager = new UserLookupManager();
            if (EncProvID != 0)
                userLookup = localFieldLookupManager.GetFieldLookupList(EncProvID, "EXAM GROUP");
            else
                userLookup = localFieldLookupManager.GetFieldLookupList(ClientSession.PhysicianId, "EXAM GROUP");

            if (userLookup.Count > 0)
            {
                for (int i = 0; i < userLookup.Count; i++)
                { cboGroupType.Items.Add(new RadComboBoxItem(userLookup[i].Value)); }
            }

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

                    }
                }
            }

            if (file_exam_lst.Count > 0)
                prevNum = sortIndexNum.Max();

            Session["NumberCount"] = prevNum;
        }
        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {
            try
            {
                string Current_Process = string.Empty;
                if (Request["CurrentProcess"] != null)
                    Current_Process = Request["CurrentProcess"].Replace(" _", "_").ToString();

                int close_type = 0;
                string User_name = string.Empty;
                string[] current_process = new string[] { Request["CurrentProcess"].Replace(" _", "_").ToString() };



                UserManager objPhy = new UserManager();
                string sPhysicianName = string.Empty;


                IList<User> PhyUserList = objPhy.getUserByPHYID(ClientSession.PhysicianId);

                sPhysicianName = PhyUserList[0].user_name;

                User_name = PhyUserList[0].user_name;
                if (Current_Process == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
                {
                    close_type = 8;
                    //User_name = ClientSession.UserName;//by Balaji

                }
                else if (Current_Process == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
                {
                    close_type = 2;
                    User_name = ClientSession.UserName;

                }
                //else if (Current_Process == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                //{
                //    close_type = 9;
                //    User_name = "UNKNOWN";
                //}
                //else if (Current_Process == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
                //{
                //    close_type = 1;
                //    User_name = "UNKNOWN";
                //}

                //if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
                //{
                //    obj_workFlow.MoveToNextProcess(OrderSubmit_ID, "DIAGNOSTIC ORDER", close_type, sPhysicianName, UtilityManager.ConvertToUniversal(), string.Empty, current_process, null);
                //    //thick  obj_workFlow.MoveToNextProcess(OrderSubmit_ID, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(), null);

                //}

                OrderSubmit_ID = Convert.ToUInt64(Request["OrderSubmit_ID"].ToString());
                if (ClientSession.UserRole == "Medical Assistant")
                {
                    WFObjectManager obj_workFlow = new WFObjectManager();
                    if (Current_Process != "MA_REVIEW")
                    {
                        obj_workFlow.MoveToNextProcess(OrderSubmit_ID, "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(), string.Empty, current_process, null);
                    }
                    else
                    {
                        obj_workFlow.MoveToNextProcess(OrderSubmit_ID, "DIAGNOSTIC ORDER", 2, User_name, UtilityManager.ConvertToUniversal(), string.Empty, current_process, null);
                    }

                    //vince March 13 2013 for double push orders //
                    //if (Current_Process == "MA_REVIEW")
                    //{
                    //    string[] current_proc = new string[] { "ORDER_GENERATE" };
                    //    obj_workFlow.MoveToNextProcess(OrderSubmit_ID, "DIAGNOSTIC ORDER", 8, sPhysicianName, UtilityManager.ConvertToUniversal(), string.Empty, current_proc, null);
                    //}

                    btnMoveToNextProcess.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MovedSuccessfully", "DisplayErrorMessage('050002');", true);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "WinClose();", true);
                    return;
                    //this.Close();  hdnOrder.Value == "false"
                }
                if ((ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant") && hdnMoveToMAID.Value == "false")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openform", "btnView_Clicked();", true);
                    return;
                }
                if ((ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant") && hdnOrder.Value == "false")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "openform", "btnView_Clicked();", true);
                    return;
                }
            }
            catch
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (Request["OrderSubmit_ID"] != null)
                OrderSubmit_ID = Convert.ToUInt64(Request["OrderSubmit_ID"].ToString());
            string sStoringFormat = string.Empty;
            string sFTPAddress = string.Empty;
            string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
            string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            FTPImageProcess ftpImageProcess = new FTPImageProcess();
            FileManagementDTO ObjFileManagementDTO = new FileManagementDTO();
            FileManagementIndexManager objfilemanger = new FileManagementIndexManager();
            IList<FileManagementIndex> fileList = new List<FileManagementIndex>();
            bool Fsave = false;

            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("atala-capture-upload/" + Session.SessionID + "/Exam_Photos"));
            if (!dir.Exists)
            {
                dir.Create();
            }

            if (Session["NumberCount"] != null)
            {
                lastNumToAdd = Session["NumberCount"].ToString();
            }
            if (lastNumToAdd != null && lastNumToAdd != string.Empty)
                prevNum = Convert.ToInt32(lastNumToAdd);
            if (Request["type"] != null && Request["type"].ToString() == "Result Upload")
            {
                if (UploadImage.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile file in UploadImage.UploadedFiles)
                    {
                        prevNum++;
                        lastNumToAdd = prevNum.ToString();
                        string server_path = string.Empty;
                        string SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/Exam_Photos/" + Path.GetFileName(file.FileName.Replace(",", " ")));
                        file.SaveAs(SelectedFilePath);
                        if (ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserID, ftpPassword))
                        {
                            if (!IsCmg)
                            {
                                if (file.FileName.Contains(".csv"))
                                {
                                    if (!(PatientIdCheckForCSVFile(SelectedFilePath)))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "openform", "SaveUnsuccessful();DisplayErrorMessage('744004');", true);
                                        return;
                                    }
                                }
                                if ((file.GetExtension() == ".csv" && CSVReader.ReadCSVFile(SelectedFilePath, true).Columns.Count == 2) || (file.GetExtension() == ".pdf" && PatientIdCheckForPDFFile(SelectedFilePath)))
                                {
                                    if (file.GetExtension().Contains(".csv"))
                                    {
                                        StaticLookupManager objStatic = new StaticLookupManager();
                                        IList<StaticLookup> LookUpListForABI = objStatic.getStaticLookupByFieldName("ABI RESULT");
                                        objABI_Result = new ABI_Results();
                                        string ABI_Value = string.Empty;
                                        List<string> lstfileInformation = new List<string>();
                                        using (System.IO.StreamReader readFile = new System.IO.StreamReader(SelectedFilePath))
                                        {
                                            string line;
                                            while ((line = readFile.ReadLine()) != null)
                                            {
                                                lstfileInformation.Add(line.ToString());
                                            }
                                        }
                                        for (int j = 0; j < LookUpListForABI.Count; j++)
                                        {
                                            IList<string> order = OrderBy_CsvFile(LookUpListForABI[j].Value, lstfileInformation);
                                            if (order.Count != 0)
                                            {
                                                string Column = string.Empty;
                                                string[] TildSplit = order[0].ToString().Split(',');
                                                if (j == 2)
                                                {
                                                    if (TildSplit[1].ToString() == "M")
                                                        TildSplit[1] = "Male";
                                                    else
                                                        TildSplit[1] = "Female";
                                                }
                                                if (j == 4)
                                                    TildSplit[1] = TildSplit[1].ToString() + " Yrs";
                                                if (j == 3)
                                                    TildSplit[1] = String.Format("{0:dd-MMM -yyyy }", UtilityManager.ConvertToLocal(Convert.ToDateTime(TildSplit[1].ToString())));
                                                if (j == 24 || j == 25 || j == 7)
                                                    TildSplit[1] = String.Format("{0:dd-MMM -yyyy hh:mm}", UtilityManager.ConvertToLocal(Convert.ToDateTime(TildSplit[1].ToString())));


                                                if (ABI_Value == string.Empty)
                                                {
                                                    ABI_Value = LookUpListForABI[j].Value.ToString() + "~" + TildSplit[1].ToString();
                                                }
                                                else
                                                {
                                                    ABI_Value += System.Environment.NewLine + LookUpListForABI[j].Value.ToString() + "~" + TildSplit[1].ToString();
                                                }
                                            }
                                        }
                                        objABI_Result.File_Name = file.FileName;
                                        objABI_Result.ABI_Field_Value_OR_File_Path = ABI_Value;
                                        objABI_Result.ABI_File_Type = "csv";
                                        objABI_Result.Order_ID = OrderSubmit_ID;
                                        Fsave = true;
                                    }
                                    else
                                    {
                                        sStoringFormat = ClientSession.FacilityName.Replace("#", "").Replace(",", "") + "_ABI_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + file.GetExtension();
                                        File.Move(SelectedFilePath, SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat);
                                        string tempSelectPath = SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat;
                                        sFTPAddress = ftpImageProcess.UploadToImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserID, ftpPassword, tempSelectPath, sStoringFormat);
                                        objABI_Result.File_Name = sFTPAddress;
                                        objABI_Result.ABI_File_Type = "pdf";
                                        objABI_Result.Order_ID = OrderSubmit_ID;
                                        sFTPAddress = string.Empty;
                                        Fsave = true;
                                        // prevNum++;
                                    }
                                    objABI_Result.Patient_ID = ClientSession.HumanId;
                                    objABI_Result.Created_By = ClientSession.UserName;
                                    objABI_Result.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    objABI_Result.Modified_By = "";
                                    objABI_Result.Modified_Date_And_Time = DateTime.MinValue;
                                    ObjFileManagementDTO.lstABI.Add(objABI_Result);
                                }
                                else if (file.GetExtension() == ".csv" && CSVReader.ReadCSVFile(SelectedFilePath, true).Columns.Count > 2)//Spiro
                                {
                                    StaticLookupManager objStaticLookupManager = new StaticLookupManager();
                                    //Spirometry
                                    Fsave = true;
                                    ArrayList XAxislist = new ArrayList();
                                    ArrayList YAxislist = new ArrayList();
                                    Spirometry objspirometry = new Spirometry();//Object Creatiosn
                                    string _Flow_Volume_Axis = string.Empty;
                                    string _Volume_Time_Axis = string.Empty;
                                    DataTable csvdataTable = CSVReader.ReadCSVFile(SelectedFilePath, true);
                                    IList<StaticLookup> stFieldLookUp = objStaticLookupManager.getStaticLookupByFieldName("SPIROMETRY PATIENT INFORMATION", "Sort_Order");
                                    string grid_cell_values = string.Empty;
                                    for (int j = 0; j < stFieldLookUp.Count; j++)
                                    {
                                        if (grid_cell_values.Trim() == string.Empty)
                                            grid_cell_values = GridLoadPAtientInformationOrder(stFieldLookUp[j].Value.ToString(), csvdataTable.AsEnumerable().ToList());
                                        else
                                            grid_cell_values += System.Environment.NewLine + GridLoadPAtientInformationOrder(stFieldLookUp[j].Value.ToString(), csvdataTable.AsEnumerable().ToList());

                                    }
                                    objspirometry.Patient_And_Test_Indormation = grid_cell_values;////////////Patient Information
                                    DataTable dataTableresult = new DataTable();
                                    IList<StaticLookup> stFieldLook = objStaticLookupManager.getStaticLookupByFieldName("SPIROMETRY TEST RESULTS", "Sort_Order");
                                    foreach (var item in stFieldLook)
                                    {
                                        Getresult(item.Value, csvdataTable.AsEnumerable().ToList(), dataTableresult);
                                    }
                                    grid_cell_values = string.Empty;
                                    for (int r = 0; r < dataTableresult.Rows.Count; r++)
                                    {
                                        string value = string.Empty;
                                        for (int j = 0; j < dataTableresult.Columns.Count; j++)
                                        {
                                            if (j == 0)
                                                value = dataTableresult.Rows[r].ItemArray[j].ToString();
                                            else
                                                value += "," + dataTableresult.Rows[r].ItemArray[j].ToString();
                                        }
                                        if (grid_cell_values == string.Empty)
                                            grid_cell_values = value;
                                        else
                                            grid_cell_values += System.Environment.NewLine + value;
                                    }
                                    objspirometry.Test_Results_Information = grid_cell_values;////////////Result Value

                                    /////Graphs
                                    # region multiple curve
                                    for (int t = 0; t < csvdataTable.Rows.Count; t++)
                                    {
                                        if (GetXAxisForGraphOne(csvdataTable.Rows[t].ItemArray[0].ToString()))
                                        {
                                            for (int n = 0; n < 8; n++)
                                            {
                                                XAxislist.Clear();
                                                YAxislist.Clear();
                                                if (csvdataTable.Rows[t].ItemArray[n + 1].ToString().Trim() != string.Empty && csvdataTable.Rows[t].ItemArray[n + 1].ToString().Trim() != "0")
                                                {
                                                    int value = Convert.ToInt16(csvdataTable.Rows[t].ItemArray[n + 1]);
                                                    for (int j = 30; j <= value * 30; j = j + 30)
                                                    {
                                                        float _xValue = ((float)j / 1000);
                                                        XAxislist.Add(_xValue);
                                                    }
                                                    for (int m = 0; m < csvdataTable.Rows.Count; m++)
                                                    {
                                                        if (GetGraphOne(csvdataTable.Rows[m].ItemArray[n].ToString()))
                                                        {
                                                            int p = m + 1;
                                                            for (int k = p; k < csvdataTable.Rows.Count; k++)
                                                            {
                                                                if (csvdataTable.Rows[k].ItemArray[n].ToString().Trim() != string.Empty)
                                                                {
                                                                    int Yvalue;
                                                                    if ((csvdataTable.Rows[k].ItemArray[n].ToString()).StartsWith("-"))
                                                                        Yvalue = Convert.ToInt16((csvdataTable.Rows[k].ItemArray[n].ToString()).Substring(1));
                                                                    else
                                                                        Yvalue = Convert.ToInt16(csvdataTable.Rows[k].ItemArray[n]);

                                                                    float _yValue = ((float)Yvalue / 100);
                                                                    YAxislist.Add(_yValue);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    string XandYValue = string.Empty;
                                                    for (int x = 0; x < YAxislist.Count; x++)
                                                    {
                                                        if (XandYValue.Trim() == string.Empty)
                                                            XandYValue = "(" + XAxislist[x].ToString() + "," + YAxislist[x].ToString() + ")";
                                                        else
                                                            XandYValue += "$" + "(" + XAxislist[x].ToString() + "," + YAxislist[x].ToString() + ")";

                                                    }
                                                    if (_Flow_Volume_Axis.Trim() == string.Empty)
                                                        _Flow_Volume_Axis = XandYValue;
                                                    else
                                                        _Flow_Volume_Axis += "~" + XandYValue;

                                                }
                                            }
                                        }
                                        else if (GetXAxisForGraphTwo(csvdataTable.Rows[t].ItemArray[0].ToString()))
                                        {
                                            ArrayList XAxislistForSecondGraph = new ArrayList();
                                            ArrayList YAxislistForSecondGraph = new ArrayList();
                                            for (int n = 0; n < 8; n++)
                                            {
                                                XAxislistForSecondGraph.Clear();
                                                YAxislistForSecondGraph.Clear();
                                                //Volume Time Chart
                                                if (csvdataTable.Rows[t].ItemArray[n + 1].ToString().Trim() != string.Empty && csvdataTable.Rows[t].ItemArray[n + 1].ToString().Trim() != "0")
                                                {
                                                    int value = Convert.ToInt16(csvdataTable.Rows[t].ItemArray[n + 1]);
                                                    for (int j = 60; j <= value * 60; j = j + 60)
                                                    {
                                                        float _xValue = ((float)j / 1000);
                                                        XAxislistForSecondGraph.Add(_xValue);
                                                    }
                                                    for (int m = 0; m < csvdataTable.Rows.Count; m++)
                                                    {
                                                        if (GetGraphSecond(csvdataTable.Rows[m].ItemArray[n + 8].ToString()))
                                                        {
                                                            int p = m + 1;
                                                            for (int k = p; k < csvdataTable.Rows.Count; k++)
                                                            {
                                                                if (csvdataTable.Rows[k].ItemArray[n + 8].ToString().Trim() != string.Empty)
                                                                {
                                                                    int Yvalue;
                                                                    if ((csvdataTable.Rows[k].ItemArray[n + 8].ToString()).StartsWith("-"))
                                                                        Yvalue = Convert.ToInt16((csvdataTable.Rows[k].ItemArray[n + 8].ToString()).Substring(1));
                                                                    else
                                                                        Yvalue = Convert.ToInt16(csvdataTable.Rows[k].ItemArray[n + 8]);

                                                                    float _yValue = ((float)Yvalue / 100);
                                                                    YAxislistForSecondGraph.Add(_yValue);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    string XandYValue = string.Empty;
                                                    for (int x = 0; x < YAxislistForSecondGraph.Count; x++)
                                                    {
                                                        if (XandYValue.Trim() == string.Empty)
                                                            XandYValue = "(" + XAxislistForSecondGraph[x].ToString() + "," + YAxislistForSecondGraph[x].ToString() + ")";
                                                        else
                                                            XandYValue += "$" + "(" + XAxislistForSecondGraph[x].ToString() + "," + YAxislistForSecondGraph[x].ToString() + ")";
                                                    }
                                                    if (_Volume_Time_Axis.Trim() == string.Empty)
                                                        _Volume_Time_Axis = XandYValue;
                                                    else
                                                        _Volume_Time_Axis += "~" + XandYValue;

                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    objspirometry.Flow_Volume_Values = _Flow_Volume_Axis;
                                    objspirometry.Volume_Time_Values = _Volume_Time_Axis;
                                    objspirometry.Patient_ID = ClientSession.HumanId;
                                    objspirometry.Created_By = ClientSession.UserName;
                                    objspirometry.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    objspirometry.In_House_Procedure_ID = OrderSubmit_ID;//////Changed
                                    objspirometry.File_Name = file.FileName; ;
                                    ObjFileManagementDTO.lstSpirometry.Add(objspirometry);
                                    //End Spirometry

                                }
                                else
                                {
                                    FileManagementIndexManager FileManagementIndexMngr = new FileManagementIndexManager();
                                    IList<FileManagementIndex> ilstFileList = FileManagementIndexMngr.GetFileCount(ClientSession.HumanId, "ORDER", string.Empty, string.Empty);
                                    if (Request["type"].ToString() == "Result Upload")
                                    // sStoringFormat = ClientSession.FacilityName.Replace("#", "") + "_ORDER_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + file.GetExtension();
                                    {
                                        if (prevNum > 1)
                                        {
                                            sStoringFormat = ClientSession.FacilityName.Replace("#", "") + "_ORDER_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + file.GetExtension();
                                        }
                                        else
                                        {
                                            sStoringFormat = ClientSession.FacilityName.Replace("#", "").Replace(",", "") + "_ORDER_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + (ilstFileList.Count + 1) + file.GetExtension();
                                        }
                                    }
                                    if (File.Exists(SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat))
                                    {
                                        File.Delete(SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat);

                                    }
                                    File.Move(SelectedFilePath, SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat);
                                    string tempSelectPath = SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat;

                                    sFTPAddress = ftpImageProcess.UploadToImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserID, ftpPassword, tempSelectPath, sStoringFormat);
                                }
                            }
                            else
                            {
                                sStoringFormat = ClientSession.FacilityName.Replace("#", "").Replace(",", "") + "_ORDER_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(file.FileName.Replace(",", ""));
                                File.Move(SelectedFilePath, SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat);
                                string tempSelectPath = SelectedFilePath.Remove(SelectedFilePath.LastIndexOf("\\")) + "\\" + sStoringFormat;
                                sFTPAddress = ftpImageProcess.UploadToImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserID, ftpPassword, tempSelectPath, sStoringFormat);
                            }
                            if (!Fsave)
                            {
                                if (sFTPAddress != string.Empty)
                                {
                                    FileManagementIndex objfileIndex = new FileManagementIndex();
                                    objfileIndex.File_Path = sFTPAddress;
                                    objfileIndex.Created_By = ClientSession.UserName;
                                    objfileIndex.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    objfileIndex.Human_ID = ClientSession.HumanId;
                                    objfileIndex.Document_Type = cboGroupType.Text;
                                    objfileIndex.Order_ID = ClientSession.EncounterId;
                                    objfileIndex.Document_Date = UtilityManager.ConvertToUniversal();
                                    objfileIndex.Result_Master_ID = ClientSession.EncounterId;
                                    if (Request["type"].ToString() == "Result Upload")
                                    {
                                        objfileIndex.Source = "ORDER";
                                        objfileIndex.Order_ID = OrderSubmit_ID;
                                        if (IsCmg)
                                            objfileIndex.Result_Master_ID = ClientSession.EncounterId;
                                        else
                                            objfileIndex.Result_Master_ID = 0;
                                    }
                                    fileList.Add(objfileIndex);
                                    ObjFileManagementDTO.FileManagementList.Add(objfileIndex);
                                    //prevNum++;
                                }
                                else
                                {
                                    //ArrayList Msg_text = new ArrayList();
                                    //Msg_text.Add(lstFileName.Items[i].Text.ToString());
                                    //ApplicationObject.erroHandler.DisplayErrorMessage("270110", this.Text, Msg_text);
                                }
                            }
                        }
                    }
                    if (!IsCmg)
                    {
                        if (ObjFileManagementDTO.lstABI.Count == 0 && ObjFileManagementDTO.lstSpirometry.Count == 0)
                        {
                            IList<FileManagementIndex> fileIndex = objfilemanger.SaveUpdateDeleteFileManagementIndexforExamPhotos(fileList.ToArray(), null, null, string.Empty, "");
                            btnMove.Enabled = true; btnMoveToNextProcess.Enabled = true;
                        }
                        else
                        {
                            ObjFileManagementDTO = objfilemanger.SaveUpdateDelete_FileManagementIndex_Abi_Spirometry(ObjFileManagementDTO, null, null, string.Empty);
                            btnMove.Enabled = true;
                        }
                        if ((ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant") && hdnMoveToMAID.Value == "false")
                        {
                            btnMoveToNextProcess.Enabled = true;
                        }
                        if ((ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant") && hdnOrder.Value == "false")
                        {
                            btnMoveToNextProcess.Enabled = true;
                        }
                        //if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant")
                        //{
                        //    btnMoveToNextProcess.Enabled = false;
                        //}
                        else
                        {
                            btnMoveToNextProcess.Enabled = true;
                        }

                        if (Request["type"] != null && Request["type"].ToString() != "Result Upload")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully()", true);
                        }
                        // 270101
                        btnMove.Enabled = true;
                    }
                    else
                    {
                        IList<FileManagementIndex> lst = objfilemanger.SaveUpdatDeleteForCMG(fileList.ToArray(), new List<FileManagementIndex>(), new List<FileManagementIndex>(), string.Empty);
                        btnMove.Enabled = true;

                        FindFileIndex(lst);
                        if (Request["type"] != null && Request["type"].ToString() != "Result Upload")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "var which_tab = window.parent.parent.theForm.hdnTabClick.value.split('$#$')[0]; var screen_name; if (which_tab.indexOf('btn') > -1) { screen_name = 'MoveToButtonsClick';} else if (which_tab == 'first') { screen_name = '';} else if (which_tab != 'first' && which_tab != 'CC / HPI' && which_tab != 'QUESTIONNAIRE' && which_tab != 'PFSH' && which_tab != 'ROS' && which_tab != 'VITALS' && which_tab != 'EXAM' && which_tab != 'TEST' && which_tab != 'ASSESSMENT' && which_tab != 'ORDERS' && which_tab != 'eRx' && which_tab != 'SERV./PROC. CODES' && which_tab != 'PLAN' && which_tab != 'SUMMARY'){ screen_name = 'ExamTabClick'; }else {screen_name = 'EncounterTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('270101');", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('270102');", true);
                    return;
                }

                if (fileList.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Keys", "window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false", true);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('270101');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                }
            }
            #region ExamPhoto
            else  //ExamPhoto
            {
                if (UploadImage.UploadedFiles.Count > 0)
                {
                    if (dtpTestTakenDate.IsEmpty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "SaveUnsuccessful();AutoSaveUnsuccessful();DisplayErrorMessage('8511007');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                        return;
                    }
                    FileInfo[] files = dir.GetFiles();
                    for (int j = 0; j < files.Length; j++)
                    {
                        File.Delete(files[j].FullName);
                    }
                    prevNum = (int)Session["NumberCount"];
                    IList<FileManagementIndex> file_lst = new List<FileManagementIndex>();
                    foreach (UploadedFile file in UploadImage.UploadedFiles)
                    {

                        if (file.FileName.ToUpper().Contains(".TIF") || file.FileName.ToUpper().Contains(".PNG") || file.FileName.ToUpper().Contains(".BMP") || file.FileName.ToUpper().Contains(".GIF") || file.FileName.ToUpper().Contains(".JPG") || file.FileName.ToUpper().Contains(".JPEG") || file.FileName.ToUpper().Contains(".DOCX") || file.FileName.ToUpper().Contains(".PDF") || file.FileName.ToUpper().Contains(".XLS") || file.FileName.ToUpper().Contains(".XLSX") || file.FileName.ToUpper().Contains(".DOC") || file.FileName.ToUpper().Contains(".TXT") || file.FileName.ToUpper().Contains(".DCM") || file.FileName.ToUpper().Contains(".PPT") || file.FileName.ToUpper().Contains(".PPTX") || file.FileName.ToUpper().Contains(".XML"))
                        {
                            string server_path = string.Empty;
                            string SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/Exam_Photos/" + Path.GetFileName(file.FileName));
                            file.SaveAs(SelectedFilePath);
                            if (ftpImageProcess.CreateDirectory(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserID, ftpPassword))
                            {
                                lastNumToAdd = Convert.ToString(prevNum + 1);
                                if (lastNumToAdd.Length == 1)
                                    lastNumToAdd = "0" + lastNumToAdd;
                                sStoringFormat = ClientSession.FacilityName.Replace("#", "").Replace(",", "") + "_EXAM_" + DateTime.Now.ToString("yyyyMMdd") + "_" + ClientSession.HumanId.ToString() + "_" + lastNumToAdd + Path.GetExtension(file.FileName.Replace(",", ""));
                                server_path = ftpImageProcess.UploadToImageServer(ClientSession.HumanId.ToString(), ftpServerIP, ftpUserID, ftpPassword, SelectedFilePath, sStoringFormat);
                                if (server_path != string.Empty)
                                {
                                    FileManagementIndex objfile = new FileManagementIndex();
                                    objfile.Human_ID = ClientSession.HumanId;
                                    string sselecteddate = dtpTestTakenDate.SelectedDate.Value.ToString("yyyy-MM-dd ");
                                    string sTime = DateTime.Now.ToString("HH:mm:ss");
                                    sselecteddate = sselecteddate + sTime;
                                    objfile.Document_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(sselecteddate));
                                    //objfile.Document_Date = UtilityManager.ConvertToUniversal(dtpTestTakenDate.SelectedDate.Value);//For bug id 39375
                                    objfile.Document_Type = cboGroupType.Text;
                                    objfile.Encounter_ID = ClientSession.EncounterId;
                                    objfile.Appointment_Provider_ID = Convert.ToUInt64(cboPhysicianName.SelectedItem.Value);
                                    objfile.File_Path = server_path;
                                    objfile.Source = "EXAM";
                                    objfile.Created_By = ClientSession.UserName;
                                    objfile.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    file_lst.Add(objfile);
                                    prevNum++;
                                    Session["NumberCount"] = prevNum;
                                }
                            }
                        }
                    }
                    if (file_lst.Count > 0)
                    {
                        file_exam_lst = objfilemanger.SaveUpdateDeleteFileManagementIndexforExamPhotos(file_lst, null, null, string.Empty, "");
                        if (Session["ExamList"] != null)
                        {
                            foreach (FileManagementIndex item in file_exam_lst)
                            {
                                ((IList<FileManagementIndex>)Session["ExamList"]).Add(item);
                            }
                        }
                        else
                        { Session["ExamList"] = file_exam_lst; }
                        LoadGrid();
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "AutoSaveSuccessful();DisplayErrorMessage('270101');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "SaveUnsuccessful();DisplayErrorMessage('270102');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                    return;
                }
                dtpTestTakenDate.SelectedDate = DateTime.Now;
            }
            //divLoading.Style.Add("display", "none");
            if (hdnFormType.Value.Trim() == "ResultUpload")
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "document.getElementById('divWaitLoading').style.display = 'none';", true);
            else
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            #endregion
            btnMove.Attributes.Add("ResultUpload", "TRUE");
            btnSave.Enabled = false;
            FillCboPhysician(ClientSession.FacilityName.ToUpper());
            LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
            if (chkShowAllPhysicians.Checked)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();", true);
            }
        }
        #region Abi
        IList<string> OrderBy_CsvFile(string line, List<string> list)
        {
            var ABI_list = from l in list where l.StartsWith(line.Replace(" ", "")) select l;
            return ABI_list.ToList<string>();
        }

        public bool PatientIdCheckForPDFFile(string file)
        {

            try
            {
                using (File.Open(file, FileMode.Open)) // the using statement performs the close automatically!
                {
                    // return true;
                }
            }
            catch (IOException)
            {
                // File_another_process = false;
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('744010');", true);
                return false;
            }

            bool bIsPatient = false;
            Acurus.Capella.UI.PDFParser pdfParser = new Acurus.Capella.UI.PDFParser();
            List<string> list = new List<string>();
            pdfParser.ExtractText(file, file.Substring(0, file.Length - 4).ToString() + ".txt");
            using (StreamReader reader = new StreamReader(file.Substring(0, file.Length - 4).ToString() + ".txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] == "Patient ID: ")
                {
                    if (Convert.ToUInt32(list[j + 2]) == ClientSession.HumanId)
                    {
                        bIsPatient = true;
                        break;
                    }
                }
            }
            File.Delete(file.Substring(0, file.Length - 4).ToString() + ".txt");
            return bIsPatient;

        }



        public bool PatientIdCheckForCSVFile(string file)
        {
            bool bIsPatient = false;
            DataTable csvdataTable = CSVReader.ReadCSVFile(file, true);

            for (int i = 0; i < csvdataTable.Rows.Count; i++)
            {
                if (csvdataTable.Rows[i].ItemArray[0].ToString().Trim().Contains("PatientID"))
                {

                    if (Convert.ToUInt32(csvdataTable.Rows[i].ItemArray[1]) == ClientSession.HumanId)
                    {
                        bIsPatient = true;
                        break;
                    }
                }
            }



            return bIsPatient;
        }



        public bool CheckCSVfile(string file, string ChooseFile)
        {
            bool bIsPatient = false;
            DataTable csvdataTable = CSVReader.ReadCSVFile(file, true);
            if (ChooseFile.Trim() == "ABI")
            {
                if (csvdataTable.Columns.Count > 2)
                    bIsPatient = true;
            }
            else if (ChooseFile.Trim() == "Spirometry")
            {
                if (csvdataTable.Columns.Count <= 2)
                    bIsPatient = true;
            }
            return bIsPatient;
        }


        #endregion
        protected void btnLibrary_Click(object sender, EventArgs e)
        {
            LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
        }
        public void LoadGrid()
        {
            file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
            if (file_exam_lst != null && file_exam_lst.Count > 0)
            {
                grdPhoto.DataSource = null;
                grdPhoto.DataBind();
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Group Type", typeof(string)));
                dt.Columns.Add(new DataColumn("No of Images", typeof(string)));
                dt.Columns.Add(new DataColumn("Test Taken Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("File Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Physician name", typeof(string)));
                dt.Columns.Add(new DataColumn("PhyID", typeof(string)));
                dt.Columns.Add(new DataColumn("FileID", typeof(string)));

                IList<string> lstDate = ((from doc in file_exam_lst
                                          orderby doc.Document_Date descending
                                          select doc.Document_Date.ToString("dd-MMM-yyyy")).Distinct()).ToList<string>();
                IList<FileManagementIndex> filelist = (from doc in file_exam_lst
                                                       orderby doc.Document_Date descending
                                                       select doc).ToList<FileManagementIndex>();
                for (int k = 0; k < lstDate.Count; k++)
                {

                    IList<string> file_management_list_phy = ((from doc in file_exam_lst
                                                               where doc.Document_Date.ToString("dd-MMM-yyyy") == lstDate[k]
                                                               orderby doc.Document_Date descending
                                                               select doc.Appointment_Provider_ID.ToString()).Distinct()).ToList<string>();

                    for (int m = 0; m < file_management_list_phy.Count; m++)
                    {

                        IList<string> lstExamdate = ((from doc in file_exam_lst
                                                      where doc.Document_Date.ToString("dd-MMM-yyyy") == lstDate[k] &&
                                                      doc.Document_Type != string.Empty
                                                      && doc.Appointment_Provider_ID.ToString() == file_management_list_phy[m]

                                                      select doc.Document_Type).Distinct()).ToList<string>();

                        for (int n = 0; n < lstExamdate.Count; n++)
                        {
                            string name = "";

                            IList<FileManagementIndex> file_management_list = (from doc in file_exam_lst
                                                                               where doc.Document_Date.ToString("dd-MMM-yyyy") == lstDate[k]
                                                                               && doc.Document_Type == lstExamdate[n]
                                                                               && doc.Appointment_Provider_ID.ToString() == file_management_list_phy[m]
                                                                               select doc).ToList<FileManagementIndex>();

                            DateTime testtaken = file_management_list[0].Document_Date;
                            dr = dt.NewRow();
                            dr["Group Type"] = file_management_list[0].Document_Type;
                            dr["No of Images"] = file_management_list.Count;
                            dr["File Name"] = string.Empty;//file_exam_lst.Where(a=>a.Document_Type==lstDocType[m]).Select(a=>a.File_Path);

                            DateTime examdate = DateTime.Parse(UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                            dr["Test Taken Date"] = examdate.Date;//UtilityManager.ConvertToLocal(testtaken).ToString("dd-MMM-yyyy");
                            dr["PhyID"] = file_management_list[0].Appointment_Provider_ID;
                            PhysicianManager obj = new PhysicianManager();
                            PhysicianLibrary phy = new PhysicianLibrary();
                            try
                            {
                                if (file_management_list[0].Appointment_Provider_ID != 0)
                                {
                                    phy = obj.GetById(file_management_list[0].Appointment_Provider_ID);
                                    name = phy.PhyPrefix + " " + phy.PhyFirstName + " " + phy.PhyMiddleName + " " + phy.PhyLastName + " " + phy.PhySuffix;
                                }

                                dr["Physician name"] = name;
                            }
                            catch
                            {
                                dr["Physician name"] = "";
                            }
                            string id = string.Empty;
                            for (int y = 0; y < file_management_list.Count; y++)
                            {
                                if (y == 0)
                                    id = file_management_list[y].Id.ToString();
                                else
                                    id = id + "|" + file_management_list[y].Id.ToString();
                            }
                            dr["FileID"] = id;
                            dt.Rows.Add(dr);
                        }

                    }
                }
                grdPhoto.DataSource = dt;
                grdPhoto.DataBind();
                int[] sortIndexNum = new int[file_exam_lst.Count];
                for (int i = 0; i < file_exam_lst.Count; i++)
                {
                    if (file_exam_lst[i].File_Path != string.Empty)
                    {
                        //bool result = ftpImage.DownloadFromImageServer(ulMyHumanID.ToString(), ftpServerIP, ftpUserID, ftpPassword, "TN_"+fileIndex[i].File_Path.Substring(fileIndex[i].File_Path.LastIndexOf("/") + 1), localPath + "\\" + fileIndex[i].Document_Type+"\\Thumbnail");
                        string fi = file_exam_lst[i].File_Path.Substring(file_exam_lst[i].File_Path.LastIndexOf("/") + 1);
                        prevNum = Convert.ToInt32(fi.Substring(fi.LastIndexOf("_") + 1, (fi.LastIndexOf(".") - 1) - fi.LastIndexOf("_")));
                        sortIndexNum[i] = prevNum;
                    }
                }
                prevNum = sortIndexNum.Max();
                Session["NumberCount"] = prevNum;
            }
            else
            {
                grdPhoto.DataSource = new string[] { };
                grdPhoto.DataBind();
                Session["NumberCount"] = prevNum;

            }
        }
        public string FileLocalPath = string.Empty;
        List<string> FilePaths = new List<string>();
        List<string> fExten = new List<string>();
        protected void grdExamPhotos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {


                if (e.CommandName == "DeleteRow")
                {
                    Session["DocumentType"] = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[4].Text;
                    Session["DocumentDate"] = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[3].Text;
                    hdndocphysician.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[8].Text.ToString();

                    file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
                    IList<FileManagementIndex> lstgrouptype = (from doc in file_exam_lst
                                                               where (doc.Document_Type == (string)Session["DocumentType"]) &&
                                                               (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") ==
                                                               Convert.ToDateTime((string)Session["DocumentDate"]).ToString("dd-MMM-yyyy")) &&
                                                               doc.Appointment_Provider_ID.ToString() == hdndocphysician.Value
                                                               select doc).ToList<FileManagementIndex>();

                    FileManagementIndexManager objfilemanger = new FileManagementIndexManager();
                    objfilemanger.SaveUpdateDeleteFileManagementIndexforExamPhotos(null, null, lstgrouptype, string.Empty, "");
                    if (Session["ExamList"] != null)
                    {
                        foreach (FileManagementIndex item in lstgrouptype)
                        {
                            ((IList<FileManagementIndex>)Session["ExamList"]).Remove(item);
                        }
                    }

                    LoadGrid();
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DeleteSuccessfully", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('85110011');", true);
                    FillCboPhysician(ClientSession.FacilityName.ToUpper());
                    LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
                    if (chkShowAllPhysicians.Checked)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();", true);
                    }

                }


                else if (e.CommandName == "ViewRow")
                {
                    IFileManagementIndexManager objfilemanager = new FileManagementIndexManager();
                    FilePaths.Clear();
                    fExten.Clear();
                    bool isViewable = false;
                    hdnDate.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[3].Text;
                    hdnDocumentType.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[4].Text;
                    hdnHumanId.Value = ClientSession.HumanId.ToString();
                    hdndocphysician.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[8].Text.ToString();
                    hdnfileid.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[9].Text.ToString();
                    file_exam_lst = objfilemanager.GetIndexedListByHumanId(ClientSession.HumanId, "Exam");
                    // file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
                    IList<FileManagementIndex> File_ListForDownlaod = new List<FileManagementIndex>();
                    IList<FileManagementIndex> lstTemp = (from doc in file_exam_lst
                                                          where (doc.Document_Type == hdnDocumentType.Value) &&
                                                          (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == Convert.ToDateTime(hdnDate.Value).ToString("dd-MMM-yyyy"))
                                                          && doc.Appointment_Provider_ID.ToString() == hdndocphysician.Value
                                                          select doc).ToList<FileManagementIndex>();
                    File_ListForDownlaod = File_ListForDownlaod.Concat(lstTemp).ToList();

                    //if (lstTemp.Count > 1)
                    //{
                    //    DownloadFile();
                    //}
                    //else
                    // {
                    foreach (FileManagementIndex item in lstTemp)
                    {
                        fExten.Add(item.File_Path.Substring(item.File_Path.LastIndexOf('.')).ToString().ToUpper());
                    }

                    // fExten = fExten.Distinct().ToList<string>();

                    fExten = fExten.ToList<string>();
                    //if (fExten.Count > 1)
                    //{
                    //    isViewable = false;
                    //}
                    //else
                    //{
                    //    isViewable = true;
                    //}
                    if (fExten.Count() == 1 && fExten[0].ToUpper() == ".PDF")
                    {
                        isViewable = true;
                    }
                    else
                    {
                        for (int k = 0; k < fExten.Count; k++)
                        {
                            if (fExten[k] == ".JPG" || fExten[k] == ".PNG" || fExten[k] == ".JPEG" || fExten[k] == ".BMP" || fExten[k] == ".PDF")
                            {
                                isViewable = true;
                            }
                            else
                            {
                                isViewable = false;
                                break;
                            }
                        }
                    }
                    if (!isViewable)
                    {
                        DownloadFile();
                    }
                    else
                    {
                        string target = string.Empty;
                        string Notes = string.Empty;
                        foreach (FileManagementIndex item in lstTemp)
                        {
                            target += item.File_Path.ToString() + "|";
                            Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                        }
                        string sDoctype = hdnDocumentType.Value; //e.Item.Cells[3].Text.ToString();
                        string sDocdate = hdnDate.Value; //e.Item.Cells[5].Text.ToString();
                        string DocPhy = hdndocphysician.Value;
                        IList<string> ExamString = new List<string>();
                        Session.Add("ExamData", target);
                        Session.Add("NotesExam", Notes);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Open", "OpenViewerMaximized('" + sDoctype + "','" + sDocdate + "','" + DocPhy + "');", true);
                    }
                    //  }
                    FillCboPhysician(ClientSession.FacilityName.ToUpper());
                    LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
                    if (chkShowAllPhysicians.Checked)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();", true);
                    }
                }
                else if (e.CommandName == "CompareRow")
                {
                    IFileManagementIndexManager objfilemanager = new FileManagementIndexManager();
                    FilePaths.Clear();
                    fExten.Clear();
                    bool isViewable = false;
                    hdnDate.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[3].Text;
                    hdnDocumentType.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[4].Text;
                    hdnHumanId.Value = ClientSession.HumanId.ToString();
                    file_exam_lst = objfilemanager.GetIndexedListByHumanId(ClientSession.HumanId, "Exam");
                    hdndocphysician.Value = ((GridItem)(grdPhoto.Items[e.CommandArgument.ToString()])).Cells[8].Text.ToString();
                    // file_exam_lst = (IList<FileManagementIndex>)Session["ExamList"];
                    IList<FileManagementIndex> File_ListForDownlaod = new List<FileManagementIndex>();
                    IList<FileManagementIndex> lstTemp = (from doc in file_exam_lst
                                                          where (doc.Document_Type == hdnDocumentType.Value) &&
                                                          (UtilityManager.ConvertToLocal(doc.Document_Date).ToString("dd-MMM-yyyy") == Convert.ToDateTime(hdnDate.Value).ToString("dd-MMM-yyyy"))
                                                          && doc.Appointment_Provider_ID.ToString() == hdndocphysician.Value

                                                          select doc).ToList<FileManagementIndex>();
                    File_ListForDownlaod = File_ListForDownlaod.Concat(lstTemp).ToList();

                    //if (lstTemp.Count > 1)
                    //{
                    //    DownloadFile();
                    //}
                    //else
                    // {
                    foreach (FileManagementIndex item in lstTemp)
                    {
                        fExten.Add(item.File_Path.Substring(item.File_Path.LastIndexOf('.')).ToString().ToUpper());
                    }
                    fExten = fExten.Distinct().ToList<string>();

                    for (int k = 0; k < fExten.Count; k++)
                    {
                        if (fExten[k] == ".JPG" || fExten[k] == ".PNG" || fExten[k] == ".JPEG" || fExten[k] == ".BMP" || fExten[k] == ".PDF")
                        {
                            isViewable = true;
                        }
                        else
                        {
                            isViewable = false;
                            break;
                        }
                    }
                    if (!isViewable)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "comparefiles", "alert('The selected File Cannot be Compared since it is not Image File.');", true);
                    }
                    else
                    {
                        string target = string.Empty;
                        string Notes = string.Empty;
                        foreach (FileManagementIndex item in lstTemp)
                        {
                            target += item.File_Path.ToString() + "|";
                            Notes += item.Exam_Photos_Notes.ToString() + "^$#@^" + item.Id.ToString() + "~";
                        }
                        string sDoctype = hdnDocumentType.Value; //e.Item.Cells[3].Text.ToString();
                        string sDocdate = hdnDate.Value; //e.Item.Cells[5].Text.ToString();
                        string DocPhy = hdndocphysician.Value;
                        IList<string> ExamString = new List<string>();
                        Session.Add("ExamData", target);
                        Session.Add("NotesExam", Notes);
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Open", "OpenViewerforCompare('" + sDoctype + "','" + sDocdate + "','" + DocPhy + "');", true);
                    }
                    //  }
                    FillCboPhysician(ClientSession.FacilityName.ToUpper());
                    LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
                    if (chkShowAllPhysicians.Checked)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();", true);
                    }
                }
            }
            catch (Exception ItemCommend) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Exam Photos Item Command", "alert('" + ItemCommend.Message + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true); }
        }
        protected void grdhuman_SortCommand1(object sender, GridSortCommandEventArgs e)
        {
            if (e.SortExpression == "Group Type")
            {
                LoadGrid();
            }
            if (e.SortExpression == "Physician name")
            {
                LoadGrid();
            }
            if (e.SortExpression == "No of Images")
            {
                LoadGrid();
            }
            if (e.SortExpression == "Test Taken Date")
            {
                LoadGrid();
            }
        }
        public void DownloadFile()
        {

            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            FTPImageProcess _ftpImageProcess = new FTPImageProcess();
            string localPath = string.Empty;
            string ftpServerIP = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;
            string simagePathname = string.Empty;
            string source = string.Empty;
            string file_path = string.Empty;
            string _fileName = string.Empty;
            IList<String> lstfile_name = new List<string>();
            string fileid = hdnfileid.Value;
            string[] ids = fileid.Split('|');

            localPath = System.Configuration.ConfigurationSettings.AppSettings["LocalPath"];
            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];

            IList<FileManagementIndex> lstFileIndex = new List<FileManagementIndex>();
            FileManagementIndexManager _fileIndexMngr = new FileManagementIndexManager();

            //lstFileIndex = 

            //lstFileIndex = _fileIndexMngr.GetIndexedListByHumanId(Convert.ToUInt64(hdnHumanId.Value), "EXAM");

            lstFileIndex = (IList<FileManagementIndex>)Session["ExamList"];

            //IList<FileManagementIndex> lstgrouptype = (from doc in lstFileIndex
            //                                           where (doc.Document_Type == hdnDocumentType.Value) && (doc.Document_Date.ToString("dd-MMM-yyyy") == (UtilityManager.ConvertToUniversal(Convert.ToDateTime(hdnDate.Value))).ToString("dd-MMM-yyyy"))
            //                                           select doc).ToList<FileManagementIndex>();


            IList<FileManagementIndex> lstgrouptype = (from doc in lstFileIndex
                                                       where ids.Contains(doc.Id.ToString())
                                                       select doc).ToList<FileManagementIndex>(); ;

            string grouplocalPath = localPath + "\\" + hdnDocumentType.Value + "\\" + (Convert.ToDateTime(hdnDate.Value)).ToString("dd-MMM-yyyy");
            DirectoryInfo dir = new DirectoryInfo(grouplocalPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
            DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//Exam_Photos"));
            if (!virdir.Exists)
            {
                virdir.Create();
            }
            FileInfo[] file = virdir.GetFiles();
            for (int i = 0; i < file.Length; i++)
            {
                File.Delete(file[i].FullName);
            }
            string[] files = Directory.GetFiles(grouplocalPath);
            FTPImageProcess ftpImage = new FTPImageProcess();
            string[] DisplayFiles = new string[lstgrouptype.Count];
            for (int i = 0; i < lstgrouptype.Count; i++)
            {

                ftpImage.DownloadFromImageServer(hdnHumanId.Value, ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(lstgrouptype[i].File_Path), grouplocalPath);
                string orig_image = grouplocalPath + "\\" + Path.GetFileName(lstgrouptype[i].File_Path);
                FileLocalPath = orig_image;
                FilePaths.Add(FileLocalPath);
            }
            using (ZipFile zip = new ZipFile())
            {
                for (int i = 0; i < FilePaths.Count; i++)
                {
                    zip.AddFile(FilePaths[i], "");
                }
                string fileName = hdnDocumentType.Value.Replace(' ', '_') + "_" + hdnDate.Value + "(" + ClientSession.HumanId + ").zip";
                zip.Save(Path.Combine(virdir.FullName, fileName));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "downloadURI('" + HttpUtility.UrlEncode(Page.ResolveClientUrl("atala-capture-download//" + Session.SessionID + "//Exam_Photos//" + fileName)) + "','" + fileName + "');", true);

                //Response.Clear();
                //Response.AppendHeader("Content-Disposition", "attachment;filename=" + hdnDocumentType.Value + "_" + hdnDate.Value + "(" + ClientSession.HumanId + ").zip");
                //Response.ContentType = "application/zip";
                //zip.Save(Response.OutputStream);
                //Response.Flush();
                //Response.End();
            }




        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            UploadImage.UploadedFiles.Clear();
            dtpTestTakenDate.SelectedDate = DateTime.Now;
            cboGroupType.SelectedIndex = 0;
            btnSave.Enabled = false;
            FillCboPhysician(ClientSession.FacilityName.ToUpper());
            LoadExam(Convert.ToUInt32(cboPhysicianName.SelectedValue));
            if (chkShowAllPhysicians.Checked)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "shwallchk", "chkShowAllPhysicians_CheckedChanged();disableAutoSave();", true);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DisableAutoSave", "disableAutoSave();", true);//BugID:53431 
        }
        protected void pbLibrary_Click(object sender, ImageClickEventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "openAddorUpdate();", true);
            //RadAddWindow.Visible = true;
            //RadAddWindow.NavigateUrl = "frmAddorUpdateKeywords.aspx?FieldName=EXAM GROUP";
            //RadAddWindow.Height = 600;
            //RadAddWindow.VisibleOnPageLoad = true;
            //RadAddWindow.Width = 800;
            //RadAddWindow.CenterIfModal = true;
            //RadAddWindow.VisibleTitlebar = true;
            //RadAddWindow.VisibleStatusbar = false;
            //RadAddWindow.KeepInScreenBounds = true;
            //RadAddWindow.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move;
            //RadAddWindow.OnClientBeforeClose = "UpdateKeyWords";


        }

        protected void LibraryButton_Click(object sender, EventArgs e)
        {
            LoadExam(0);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Loaded", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void grdExamPhotos_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    ImageButton imagBtn = (ImageButton)e.Item.FindControl("pbView");

                    imagBtn.Attributes["href"] = "javascript:void(0);";

                    //imagBtn.Attributes["onclick"] = String.Format("return ShowEditForm('"+e.Item.UniqueID+"');");

                    string doc_type = ((System.Data.DataRowView)e.Item.DataItem).Row.ItemArray[0].ToString();
                    string doc_date = ((System.Data.DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString();
                    imagBtn.Attributes["onclick"] = String.Format("return ShowEditForm('" + ClientSession.HumanId.ToString() + ",ExamPhotos," + doc_type + "," + doc_date + "');");


                }
            }
            catch
            {
                //do nothing
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            UploadImage.UploadedFiles.Clear();
            dtpTestTakenDate.SelectedDate = DateTime.Now;
            cboGroupType.SelectedIndex = 0;
            btnSave.Enabled = false;

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            cboGroupType.Items.Clear();
            UserLookupManager localFieldLookupManager = new UserLookupManager();
            IList<UserLookup> userLookup = localFieldLookupManager.GetFieldLookupList(ClientSession.PhysicianId, "EXAM GROUP");
            if (userLookup.Count > 0)
            {
                for (int i = 0; i < userLookup.Count; i++)
                {
                    cboGroupType.Items.Add(new RadComboBoxItem(userLookup[i].Value));
                }
            }
            RadAddWindow.Visible = false;
        }

        //Spirometry Methods
        public string GridLoadPAtientInformationOrder(string value, IEnumerable<DataRow> csvdataTable)
        {
            string Grid_Value = string.Empty;

            var data = (from da in csvdataTable where da.ItemArray[0].ToString().Trim() == value.Trim() select da).ToList();
            if (data.Count > 0)
            {
                if (data[0].ItemArray[0].ToString().Trim() == "Name")
                {
                    Grid_Value = "Name" + "," + data[0].ItemArray[1].ToString() + " " + data[0].ItemArray[2].ToString();
                }
                else if (data[0].ItemArray[0].ToString().Trim() == "BirthDate")
                {
                    Grid_Value = "Birth Date" + "," + data[0].ItemArray[1].ToString();
                }
                else if (data[0].ItemArray[0].ToString().Trim() == "PatientID")
                {
                    Grid_Value = "Patient ID" + "," + data[0].ItemArray[1].ToString();
                }
                else if (data[0].ItemArray[0].ToString().Trim() == "NoOfTrials")
                {
                    Grid_Value = "No Of Trials" + "," + data[0].ItemArray[1].ToString();
                }
                else if (data[0].ItemArray[0].ToString().Trim() == "TypeOfTest")
                {
                    Grid_Value = "Type Of Test" + "," + data[0].ItemArray[1].ToString();
                }
                else if (data[0].ItemArray[0].ToString() == "Gender")
                {
                    if (data[0].ItemArray[1].ToString().Trim() == "0")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Male";
                    else if (data[0].ItemArray[1].ToString().Trim() == "1")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Female";
                }
                else if (data[0].ItemArray[0].ToString() == "Smoker")
                {
                    if (data[0].ItemArray[1].ToString().Trim() == "0")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Yes";
                    else if (data[0].ItemArray[1].ToString().Trim() == "1")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "No";
                    else if (data[0].ItemArray[1].ToString().Trim() == "2")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Ex";
                }
                else if (data[0].ItemArray[0].ToString() == "Asthma")
                {
                    if (data[0].ItemArray[1].ToString().Trim() == "0")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "No";
                    else if (data[0].ItemArray[1].ToString().Trim() == "1")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Possible";
                    else if (data[0].ItemArray[1].ToString().Trim() == "2")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Yes";
                }
                else if (data[0].ItemArray[0].ToString() == "Ethnic")
                {
                    if (data[0].ItemArray[1].ToString().Trim() == "0")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "African";
                    else if (data[0].ItemArray[1].ToString().Trim() == "1")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Caucasian / European";
                    else if (data[0].ItemArray[1].ToString().Trim() == "2")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Mexican";
                    else if (data[0].ItemArray[1].ToString().Trim() == "3")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Asian";
                    else if (data[0].ItemArray[1].ToString().Trim() == "4")
                        Grid_Value = data[0].ItemArray[0].ToString() + "," + "Other";
                }
                else if (data[0].ItemArray[0].ToString().Trim() == "Weight")
                {
                    int weight = Convert.ToInt32(data[0].ItemArray[1]) / 100;
                    Grid_Value = data[0].ItemArray[0].ToString() + "," + weight + "  Kg";
                }
                else if (data[0].ItemArray[0].ToString().Trim() == "Height")
                {
                    Grid_Value = data[0].ItemArray[0].ToString() + "," + data[0].ItemArray[1].ToString() + " cm";
                }
                else
                    Grid_Value = data[0].ItemArray[0].ToString() + "," + data[0].ItemArray[1].ToString();
            }
            return Grid_Value;

        }



        public void Getresult(string value, IEnumerable<DataRow> csvdataTable, DataTable dataTableresult)
        {
            var data = (from da in csvdataTable where da.ItemArray[0].ToString().Trim() == value.Trim() select da).ToList();

            if (data.Count > 0)
            {
                if (data[0].ItemArray[0].ToString().Trim() == "TrialNo")
                {
                    dataTableresult.Columns.Add("parameter", typeof(string));
                    for (int x = 0; x < 8; x++)
                    {
                        if (data[0].ItemArray[x + 1].ToString() != string.Empty && data[0].ItemArray[x + 1].ToString() != "0")
                            dataTableresult.Columns.Add("trial " + data[0].ItemArray[x + 1].ToString(), typeof(string));
                    }
                }
                else
                {
                    object[] array = new object[dataTableresult.Columns.Count];
                    for (int j = 0; j < dataTableresult.Columns.Count; j++)
                    {
                        array[j] = data[0].ItemArray[j].ToString().Trim();
                    }
                    dataTableresult.Rows.Add(array);
                }
            }

        }

        //Graphs
        public bool GetGraphOne(string rowvalue)
        {
            bool result = false;
            if (rowvalue.Trim().StartsWith("CurveFV"))
            {
                result = true;
            }
            return result;

        }
        public bool GetGraphSecond(string rowvalue)
        {
            bool result = false;
            if (rowvalue.Trim().StartsWith("CurveVT"))
            {
                result = true;
            }
            return result;
        }
        public bool GetXAxisForGraphOne(string rowvalue)
        {
            bool result = false;
            if (rowvalue == "CuFVLen")
            {
                result = true;
            }
            return result;
        }
        public bool GetXAxisForGraphTwo(string rowvalue)
        {
            bool result = false;
            if (rowvalue == "CuVTLen")
            {
                result = true;
            }
            return result;
        }
        protected void btnDisableOnload_Click(object sender, EventArgs e)
        {
            RadViewer.VisibleOnPageLoad = false;
        }
        protected void Invisiblebtn_Click(object sender, EventArgs e)
        {
            FileLocalPath = Session["FileLocalPath"].ToString();

            HttpContext context = HttpContext.Current;
            using (MemoryStream mem = new MemoryStream())
            {
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(FileLocalPath));
                context.Response.TransmitFile(FileLocalPath);
                context.ApplicationInstance.CompleteRequest();
            }
        }
        [WebMethod(EnableSession = true)]
        public static string loadcboExamValues(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            UInt32 EncProviderID = 0;
            if (data.ToString() != string.Empty)
            {
                EncProviderID = Convert.ToUInt32(data.ToString().Split('~')[0]);
            }
            IList<UserLookup> userLookup = new List<UserLookup>();
            UserLookupManager localFieldLookupManager = new UserLookupManager();

            if (EncProviderID != 0)
                userLookup = localFieldLookupManager.GetFieldLookupList(EncProviderID, "EXAM GROUP");
            else
                userLookup = localFieldLookupManager.GetFieldLookupList(ClientSession.PhysicianId, "EXAM GROUP");
            string[] ExamValues = new string[userLookup.Count];

            if (userLookup.Count > 0)
            {

                for (int i = 0; i < userLookup.Count; i++)
                { ExamValues[i] = userLookup[i].Value; }
            }
            return JsonConvert.SerializeObject(ExamValues);
        }
    }
}











