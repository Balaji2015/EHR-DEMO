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
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Xml;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.PatientPortal
{
    public partial class frmViewResult : System.Web.UI.Page
    {
        #region Commented
        //ulong UlHumanId = 0;
        //ulong UlOrderSubmitId = 0;
        //ulong UlResultId = 0;
        //ulong UlIndexId = 0;
        //ulong order_submit_id = 0;
        //ulong Physician_Id;
        //IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
        //IList<ResultMaster> result_master_list = new List<ResultMaster>();
        //bool load_documenttype = true;
        //IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
        //IList<Result_Date_Order> result_list = new List<Result_Date_Order>();
        //IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
        //string order_type = string.Empty;
        //public RadTab tb = null;
        //ResultMaster objresultmaster = null;
        //RadListBox lsttype = new RadListBox();
        //FillPhysicianUser PhyUserList;
        //string Obj_Type = string.Empty;
        //ulong Result_Master_ID = 0;

        //[Serializable]
        //public class Result_Date_Order
        //{
        //    public DateTime _Res_Date = DateTime.MinValue;
        //    public ulong _Ord_Id = 0;
        //    public string _Doc_Type = string.Empty;
        //    public string _Img_path = string.Empty;
        //    public ulong _Res_Id = 0;
        //    public string _Order_Description = string.Empty;
        //    public ulong _IndexId = 0;
        //    public ulong Order_Submit_Id
        //    {
        //        get { return _Ord_Id; }
        //        set { _Ord_Id = value; }
        //    }


        //    public DateTime Result_Date
        //    {
        //        get { return _Res_Date; }
        //        set { _Res_Date = value; }
        //    }

        //    public string Ord_Type
        //    {
        //        get { return _Doc_Type; }
        //        set { _Doc_Type = value; }
        //    }

        //    public ulong Result_Id
        //    {
        //        get { return _Res_Id; }
        //        set { _Res_Id = value; }
        //    }

        //    public string Img_Path
        //    {
        //        get { return _Img_path; }
        //        set { _Img_path = value; }
        //    }

        //    public string Order_Description
        //    {
        //        get { return _Order_Description; }
        //        set { _Order_Description = value; }
        //    }
        //    public ulong IndexId
        //    {
        //        get { return _IndexId; }
        //        set { _IndexId = value; }

        //    }

        //}

        //protected void Page_Load(object sender, EventArgs e)
        ////<<<<<<< frmViewResult.aspx.cs
        //{
        //    pbPlus.ImageUrl = "~/Resources/plus_new.gif";

        //    if (!IsPostBack)
        //    {

        //        DLC.DName = "pbDropdown";
        //        DLC.txtDLC.Attributes.Add("onkeypress", "txtProviderNotes_OnKeyPress();");
        //        DLC.txtDLC.Attributes.Add("onchange", "txtProviderNotes_OnKeyPress();");
        //        btnPrintEducatnMaterial.Visible = false;
        //        Session["Status_Flag"] = null;
        //        //HumanManager objhumanmanager = new HumanManager();
        //        //IList<Human> humanList = null;
        //        if (Request["HumanId"] != "undefined")
        //        {
        //            UlHumanId = Convert.ToUInt32(Request["HumanId"]);
        //        }
        //        else
        //        {
        //            UlHumanId = ClientSession.HumanId;
        //        }
        //        if (Request["PhysicianId"] != null)
        //        {
        //            Physician_Id = Convert.ToUInt32(Request["PhysicianId"]);
        //        }
        //        else
        //        {
        //            Physician_Id = ClientSession.PhysicianId;
        //        }
        //        Session["HumanId"] = UlHumanId;
        //        //humanList = objhumanmanager.GetPatientDetailsUsingPatientInformattion(UlHumanId);
        //        //if (humanList.Count > 0)
        //        //{
        //        //    //txtPatientName.Text = humanList[0].First_Name + " " + humanList[0].MI + " " + humanList[0].Last_Name;
        //        //    //txtSex.Text = humanList[0].Sex;
        //        //    //txtDOB.Text = humanList[0].Birth_Date.ToString("dd-MMM-yyyy");
        //        //    //txtAccountNo.Text = humanList[0].Id.ToString();
        //        //    //txtPatientType.Text = humanList[0].Human_Type;
        //        //    txtPatientInformation.Value = humanList[0].First_Name + " " + humanList[0].MI + " " + humanList[0].Last_Name + " | " + humanList[0].Birth_Date.ToString("dd-MMM-yyyy") + " | " + humanList[0].Sex + " | Acc #:" + humanList[0].Id.ToString() + " | " + humanList[0].Human_Type;
        //        //}
        //        //Added by Vaishali on 18-01-2016
        //        string FileName = "Human" + "_" + UlHumanId + ".xml";
        //        string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
        //        if (File.Exists(strXmlFilePath) == true)
        //        {

        //            XmlDocument itemDoc = new XmlDocument();
        //            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
        //            itemDoc.Load(XmlText);
        //            XmlText.Close();
        //            if (itemDoc.GetElementsByTagName("Human").Count > 0 && itemDoc.GetElementsByTagName("Human")[0] != null)
        //            {
        //                txtPatientInformation.Value = itemDoc.GetElementsByTagName("Human")[0].Attributes["First_Name"].Value + " " + itemDoc.GetElementsByTagName("Human")[0].Attributes["MI"].Value + " " + itemDoc.GetElementsByTagName("Human")[0].Attributes["Last_Name"].Value + " | " + Convert.ToDateTime(itemDoc.GetElementsByTagName("Human")[0].Attributes["Birth_Date"].Value).ToString("dd-MMM-yyyy") + " | " + itemDoc.GetElementsByTagName("Human")[0].Attributes["Sex"].Value + " | Acc #:" + itemDoc.GetElementsByTagName("Human")[0].Attributes["Id"].Value + " | " + itemDoc.GetElementsByTagName("Human")[0].Attributes["Human_Type"].Value;
        //            }
        //        }
        //        if (Request["Screen"] != null && Request["Screen"] == "Cmg")
        //        {
        //            pnlTree.Visible = false;
        //            tblView.Rows[2].Cells[0].Width = "1%";
        //            tblView.Rows[2].Cells[1].Width = "99%";
        //            tblView.Rows[2].Cells[1].ColSpan = 2;

        //            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        //            filelist = objfileproxy.GetImagesforAnnotations(ClientSession.HumanId, Convert.ToUInt64(Request["OrderSubmitId"].ToString()), "ORDER");
        //            IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
        //            lstfile = (from doc in filelist
        //                       where doc.Result_Master_ID == ClientSession.EncounterId && doc.Source == "ORDER"
        //                       select doc).ToList<FileManagementIndex>();
        //            if (lstfile.Count > 0)
        //            {
        //                if (lstfile[0].Source == "ORDER")
        //                {
        //                    string concatenatedFiles = string.Empty;
        //                    string filename = string.Empty;
        //                    foreach (FileManagementIndex fileItems in lstfile)
        //                    {
        //                        filename = Path.GetFileName(fileItems.File_Path);
        //                        concatenatedFiles = concatenatedFiles + filename + "~";
        //                    }

        //                    tb = new RadTab();
        //                    tb.Target = "tbOrder";
        //                    tb.Text = "Result Files";
        //                    //tabView.Tabs.Add(tb);
        //                    PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + concatenatedFiles;
        //                    PageViewResult.Selected = true;
        //                    // TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                }
        //            }
        //        }
        //        //$muthusamy on 17-12-2014 for LabResult Changes
        //        else if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
        //        {
        //            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        //            FileManagementDTO objdto = new FileManagementDTO();
        //            objdto = objfileproxy.GetResultusingResultMasterIdandHumanID(Convert.ToUInt32(Request["ResultMasterID"]), UlHumanId);
        //            result_master_list = objdto.ResultMasterList;
        //            Session["ResultList"] = result_master_list;
        //            Session["FileList"] = objdto.FileManagementList;
        //            loadlistforResult();
        //            //cboMoveToMA.Items.Add(new RadComboBoxItem(""));
        //            //for (int i = 0; i < objdto.UserList.Count; i++)
        //            //    cboMoveToMA.Items.Add(new RadComboBoxItem(objdto.UserList[i].user_name + "-" + objdto.UserList[i].person_name));
        //            XDocument xmlUser = XDocument.Load(Server.MapPath(@"ConfigXML\User.xml"));
        //            cboMoveToMA.Items.Clear();
        //            cboMoveToMA.Items.Add(new RadComboBoxItem(""));
        //            if (chkShowAll.Checked)
        //            {
        //                foreach (XElement elements in xmlUser.Descendants("UserList"))
        //                {
        //                    foreach (XElement UserElement in elements.Elements())
        //                    {
        //                        if (UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
        //                        {
        //                            string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
        //                            cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (XElement elements in xmlUser.Descendants("UserList"))
        //                {
        //                    foreach (XElement UserElement in elements.Elements())
        //                    {
        //                        if (UserElement.Attribute("Default_Facility").Value.ToUpper() == ClientSession.FacilityName.ToUpper() && UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
        //                        {
        //                            string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
        //                            cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
        //                        }
        //                    }
        //                }
        //            }
        //            if (ClientSession.UserRole.ToUpper() != "MEDICAL ASSISTANT")
        //            {
        //                cboMoveToMA.Visible = true;
        //                Label1.Visible = true;
        //                chkShowAll.Visible = true;
        //            }
        //            if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
        //            {
        //                btnMoveToMa.Text = "Move To Provider";
        //            }
        //        }
        //        //$muthusamy
        //        else
        //        {
        //            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        //            FileManagementDTO objdto = new FileManagementDTO();
        //            objdto = objfileproxy.GetResultandExamListusing(UlHumanId);
        //            filelist = objdto.FileManagementList;
        //            result_master_list = objdto.ResultMasterList;
        //            Session["FileList"] = filelist;
        //            Session["ResultList"] = result_master_list;
        //            loadlistforResult();
        //            Session["Load"] = load_documenttype;
        //        }

        //        if (Request["Screen"] == "Demographic")
        //        {
        //            if (filelist.Count > 0)
        //            {
        //                if (filelist[0].Document_Type != "Results")
        //                {
        //                    Session["doc_type"] = filelist[0].Document_Type;
        //                    loadTreeView(filelist[0].Document_Type, "");

        //                    for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //                    {
        //                        for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                        {
        //                            if (tvViewIndex.Nodes[i].Nodes[j].Value == filelist[0].File_Path)
        //                            {
        //                                tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                                TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    RadComboBoxItem tempItem = new RadComboBoxItem();
        //                    tempItem.Text = "Results";
        //                    tempItem.ToolTip = "Results";
        //                    cboDocumentType.Items.Add(tempItem);
        //                    cboDocumentType.Text = "Results";
        //                    loadTreeView("Results", "");

        //                }
        //            }

        //            else if (result_master_list.Count > 0)
        //            {
        //                RadComboBoxItem tempItem = new RadComboBoxItem();
        //                tempItem.Text = "Results";
        //                tempItem.ToolTip = "Results";
        //                cboDocumentType.Items.Add(tempItem);
        //                cboDocumentType.Text = "Results";
        //                loadTreeView("Results", "");

        //            }
        //            DLC.txtDLC.Enabled = false;
        //            txtMedicalAssistantNotes.Enabled = false;
        //            btnMoveToMa.Visible = false;
        //            btnMoveToNextProcess.Visible = false;
        //            btnSave.Visible = false;
        //            chkPhyName.Visible = false;
        //        }

        //        else if (Request["Screen"] == "MyQ" || Request["Screen"] == "OrderManagement" || Request["Screen"] == "ResultViewOrder")
        //        {
        //            pnlTree.Visible = true;
        //            RadComboBoxItem tempItem = new RadComboBoxItem();
        //            tempItem.Text = "Results";
        //            tempItem.ToolTip = "Results";
        //            cboDocumentType.Items.Add(tempItem);
        //            cboDocumentType.Text = "Results";
        //            Session["doc_type"] = "Results";
        //            loadTreeView("Results", "");


        //            // Add by Suvarnni for Bug id 28606
        //            string sTemp = string.Empty;
        //            string[] sPhyname = null;
        //            StaticLookupManager objstaticlookup = new StaticLookupManager();
        //            IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
        //            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        //            PhysicianManager objphysican = new PhysicianManager();
        //            if (Physician_Id != 0)
        //            {
        //                //PhysicianList = objphysican.GetphysiciannameByPhyID(ClientSession.PhysicianId);
        //                PhysicianList = objphysican.GetphysiciannameByPhyID(Physician_Id);
        //                sPhyname = StaticLst[0].Value.Split('|');
        //                sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
        //                sTemp = sTemp.Replace("<Date>", "");
        //                sTemp = sTemp.Replace(" on ", "");
        //                chkPhyName.Text = sTemp;
        //            }
        //            for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //            {
        //                for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                {
        //                    if (!tvViewIndex.Nodes[i].Nodes[j].Value.ToString().Contains(".tif"))
        //                    {
        //                        if (Convert.ToUInt32(tvViewIndex.Nodes[i].Nodes[j].Value) == Convert.ToUInt32(Request["OrderSubmitId"]))
        //                        {
        //                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                            break;
        //                        }
        //                    }


        //                }
        //            }

        //            //try
        //            //{
        //            //    tblView.Rows[2].Cells[0].Attributes.Remove("style");
        //            //    tblView.Rows[2].Cells[1].Attributes.Remove("style");
        //            //    //tblView.Rows[2].Cells[0].Width = "1%";
        //            //    //tblView.Rows[2].Cells[1].Width = "100%";
        //            //    tblView.Rows[2].Cells[1].ColSpan = 2;
        //            //}
        //            //catch
        //            //{
        //            //    tblView.Rows[2].Cells[0].ColSpan = 2;
        //            //    tblView.Rows[2].Cells[0].Width = "100%";

        //            //}



        //            objresultmaster = new ResultMaster();
        //            ResultMasterManager masterProxy = new ResultMasterManager();
        //            objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt32(Request["HumanId"]), Convert.ToUInt32(Request["OrderSubmitId"]));
        //            if (objresultmaster != null && objresultmaster.Id != 0)
        //            {
        //                DLC.txtDLC.Text = objresultmaster.Result_Review_Comments;
        //                //txtProviderNotes.Text = objresultmaster.Result_Review_Comments;
        //                txtMedicalAssistantNotes.Text = objresultmaster.MA_Notes;
        //                if (objresultmaster.Is_Electronic_Mode == string.Empty && (objresultmaster.Result_Review_Comments != string.Empty || objresultmaster.MA_Notes != string.Empty))
        //                {
        //                    Session["BoolNotes"] = true;
        //                }
        //                Session["Notes"] = objresultmaster;
        //            }
        //            else
        //            {
        //                Session["BoolNotes"] = false;
        //            }

        //            if (Request["Screen"] == "ResultView" || Request["Screen"] == "OrderManagement")
        //            {
        //                btnMoveToMa.Visible = false;
        //                //btnMoveToNextProcess.Visible = false;
        //                //chkPhyName.Visible = false;
        //                btnSave.Enabled = false;
        //                txtMedicalAssistantNotes.Enabled = false;
        //                txtMedicalAssistantNotes.Style.Add("background-color", "#ebebe4");
        //                DLC.txtDLC.Enabled = false;
        //                //txtProviderNotes.Enabled = false;

        //            }

        //        }

        //        else if (Request["Screen"] == "ResultView")
        //        {
        //            pnlTree.Visible = true;
        //            RadComboBoxItem tempItem = new RadComboBoxItem();
        //            tempItem.Text = "Results";
        //            tempItem.ToolTip = "Results";
        //            cboDocumentType.Items.Add(tempItem);
        //            cboDocumentType.Text = "Results";
        //            Session["doc_type"] = "Results";
        //            loadTreeView("Results", "");


        //            // Add by Suvarnni for Bug id 28606
        //            string sTemp = string.Empty;
        //            string[] sPhyname = null;
        //            StaticLookupManager objstaticlookup = new StaticLookupManager();
        //            IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
        //            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        //            PhysicianManager objphysican = new PhysicianManager();
        //            if (Physician_Id != 0)
        //            {
        //                PhysicianList = objphysican.GetphysiciannameByPhyID(Physician_Id);
        //                sPhyname = StaticLst[0].Value.Split('|');
        //                sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
        //                sTemp = sTemp.Replace("<Date>", "");
        //                sTemp = sTemp.Replace(" on ", "");
        //                chkPhyName.Text = sTemp;
        //            }

        //            for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //            {
        //                for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                {
        //                    if (!tvViewIndex.Nodes[i].Nodes[j].Value.ToString().Contains(".tif"))
        //                    {
        //                        if (Convert.ToUInt32(tvViewIndex.Nodes[i].Nodes[j].Value) == Convert.ToUInt32(Request["OrderSubmitId"]))
        //                        {
        //                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                            break;
        //                        }
        //                    }


        //                }
        //            }

        //            //try
        //            //{
        //            //    tblView.Rows[2].Cells[0].Attributes.Remove("style");
        //            //    tblView.Rows[2].Cells[1].Attributes.Remove("style");
        //            //    //tblView.Rows[2].Cells[0].Width = "1%";
        //            //    //tblView.Rows[2].Cells[1].Width = "100%";
        //            //    tblView.Rows[2].Cells[1].ColSpan = 2;
        //            //}
        //            //catch
        //            //{
        //            //    tblView.Rows[2].Cells[0].ColSpan = 2;
        //            //    tblView.Rows[2].Cells[0].Width = "100%";

        //            //}



        //            objresultmaster = new ResultMaster();
        //            ResultMasterManager masterProxy = new ResultMasterManager();
        //            objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt32(Request["HumanId"]), Convert.ToUInt32(Request["OrderSubmitId"]));
        //            if (objresultmaster != null && objresultmaster.Id != 0)
        //            {
        //                DLC.txtDLC.Text = objresultmaster.Result_Review_Comments;
        //                //txtProviderNotes.Text = objresultmaster.Result_Review_Comments;
        //                txtMedicalAssistantNotes.Text = objresultmaster.MA_Notes;
        //                if (objresultmaster.Is_Electronic_Mode == string.Empty && (objresultmaster.Result_Review_Comments != string.Empty || objresultmaster.MA_Notes != string.Empty))
        //                {
        //                    Session["BoolNotes"] = true;
        //                }
        //                Session["Notes"] = objresultmaster;
        //            }
        //            else
        //            {
        //                Session["BoolNotes"] = false;
        //            }

        //            if (Request["Screen"] == "ResultView" || Request["Screen"] == "OrderManagement")
        //            {
        //                btnMoveToMa.Visible = false;
        //                //btnMoveToNextProcess.Visible = false;
        //                //chkPhyName.Visible = false;
        //                btnSave.Enabled = false;
        //                txtMedicalAssistantNotes.Enabled = false;
        //                txtMedicalAssistantNotes.Style.Add("background-color", "#ebebe4");
        //                DLC.txtDLC.Enabled = false;
        //                //txtProviderNotes.Enabled = false;

        //            }

        //        }

        //        else
        //        {
        //            if (Request["Screen"] != null && Request["Screen"] == "Cmg")
        //            {
        //                //do nothing
        //            }
        //            else
        //            {
        //                if (Request["Type"] != null && Request["Type"].ToString() != "Results")
        //                {
        //                    UlOrderSubmitId = Convert.ToUInt32(Request["OrderSubmitId"]);
        //                    UlResultId = Convert.ToUInt32(Request["ResultId"]);
        //                    order_type = Request["Type"];
        //                    order_submit_id = UlOrderSubmitId;


        //                    ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];
        //                    IList<FileManagementIndex> temp_list = ((from doc in ilstscaninindex
        //                                                             where doc.Id == Convert.ToUInt32(Request["IndexId"])
        //                                                             select doc)).ToList<FileManagementIndex>();
        //                    string Filename = temp_list[0].File_Path.Substring(temp_list[0].File_Path.LastIndexOf("/") + 1);
        //                    string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        //                    string[] name = temp_file_name.Split('.');
        //                    string file_name = temp_list[0].Document_Sub_Type + "_" + name[0];
        //                    Session["doc_type"] = temp_list[0].Document_Type;
        //                    loadTreeView(temp_list[0].Document_Type, temp_list[0].Document_Sub_Type.ToString());
        //                    /* File Header */


        //                    //txtFileName.Text = temp_list[0].File_Path.Substring(temp_list[0].File_Path.LastIndexOf("/") + 1);
        //                    //txtDocType.Text = temp_list[0].Document_Type.ToString();
        //                    //txtSdocType.Text = temp_list[0].Document_Sub_Type.ToString();
        //                    //txtDocDate.Text = temp_list[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");
        //                    txtFileInformation.Value = temp_list[0].File_Path.Substring(temp_list[0].File_Path.LastIndexOf("/") + 1) + " | " + temp_list[0].Document_Type.ToString() + " | " + temp_list[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(temp_list[0].Created_Date_And_Time).ToString("dd-MMM-yyyy");//BugID:42809 old: temp_list[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");
        //                    for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //                    {
        //                        for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                        {
        //                            if (tvViewIndex.Nodes[i].Nodes[j].Value == temp_list[0].File_Path)
        //                            {
        //                                tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                                TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                                break;
        //                            }
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    order_submit_id = Convert.ToUInt32(Request["OrderSubmitId"]);
        //                    UlResultId = Convert.ToUInt32(Request["ResultId"]);

        //                    if (!cboDocumentType.Items.Any(a => a.Text == "Results"))
        //                    {
        //                        RadComboBoxItem tempItem = new RadComboBoxItem();
        //                        tempItem.Text = "Results";
        //                        tempItem.ToolTip = "Results";
        //                        cboDocumentType.Items.Add(tempItem);
        //                        cboDocumentType.Text = "Results";
        //                    }
        //                    ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];
        //                    if (ilstscaninindex.Count > 0 && Request["Type"] != "Results")
        //                    {
        //                        IList<FileManagementIndex> temp_list = ((from doc in ilstscaninindex
        //                                                                 where doc.Id == Convert.ToUInt32(Request["IndexId"])
        //                                                                 select doc)).ToList<FileManagementIndex>();
        //                        loadTreeView(ilstscaninindex[0].Document_Type.ToString(), "");
        //                    }
        //                    else
        //                        loadTreeView("Results", "");
        //                    try
        //                    {
        //                        if (UlResultId == 0)
        //                        {
        //                            if (Request["IndexId"] != null)
        //                            {
        //                                for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //                                {
        //                                    for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                                    {
        //                                        if (Convert.ToUInt64(tvViewIndex.Nodes[i].Nodes[j].Value) == order_submit_id && tvViewIndex.Nodes[i].Nodes[j].Target == Request["IndexId"])
        //                                        {
        //                                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                                            break;
        //                                        }

        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //                                {
        //                                    for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                                    {
        //                                        if (Convert.ToUInt64(tvViewIndex.Nodes[i].Nodes[j].Value) == order_submit_id)
        //                                        {
        //                                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                                            break;
        //                                        }

        //                                    }
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        //                            {
        //                                for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        //                                {

        //                                    if (tvViewIndex.Nodes[i].Nodes[j].Target != "ORDERS" && tvViewIndex.Nodes[i].Nodes[j].Target != null && tvViewIndex.Nodes[i].Nodes[j].Target != string.Empty)
        //                                    {
        //                                        if (Convert.ToUInt64(tvViewIndex.Nodes[i].Nodes[j].Target) == UlResultId)
        //                                        {
        //                                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        //                                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        //                                            break;
        //                                        }
        //                                    }

        //                                }
        //                            }

        //                        }
        //                    }
        //                    catch
        //                    {

        //                    }

        //                    objresultmaster = new ResultMaster();
        //                    ResultMasterManager masterProxy = new ResultMasterManager();
        //                    objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(UlHumanId, Convert.ToUInt32(Request["OrderSubmitId"]));
        //                    if (objresultmaster != null)
        //                    {
        //                        DLC.txtDLC.Text = objresultmaster.Result_Review_Comments;
        //                        //txtProviderNotes.Text = objresultmaster.Result_Review_Comments;
        //                        txtMedicalAssistantNotes.Text = objresultmaster.MA_Notes;
        //                        if (objresultmaster.Is_Electronic_Mode == string.Empty && (objresultmaster.Result_Review_Comments != string.Empty || objresultmaster.MA_Notes != string.Empty))
        //                        {
        //                            Session["BoolNotes"] = true;
        //                        }
        //                        Session["Notes"] = objresultmaster;
        //                    }
        //                    else
        //                    {
        //                        Session["BoolNotes"] = false;
        //                    }
        //                }
        //            }
        //            DLC.txtDLC.Enabled = false;
        //            //txtProviderNotes.Enabled = false;
        //            txtMedicalAssistantNotes.Enabled = false;
        //            btnMoveToMa.Visible = false;
        //            btnMoveToNextProcess.Visible = false;
        //            btnSave.Visible = false;
        //            chkPhyName.Visible = false;


        //        }

        //        btnSave.Enabled = false;
        //        hdnSave.Value = "false";
        //        btnSave.Attributes.Add("onclick", "btnSave_ClientClick();");
        //    }


        //    if (Request["Screen"] != null && Request["Screen"] == "MyQ" || Request["Screen"] == "ResultViewOrder")
        //    {
        //        DLC.txtDLC.Enabled = true;
        //        if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
        //        {
        //            DLC.txtDLC.Enabled = false;
        //            //DLC.pbDropdown.Disabled = true;
        //        }
        //        //txtProviderNotes.Enabled = true;
        //        txtMedicalAssistantNotes.Enabled = true;
        //        btnSave.Visible = true;
        //        if (hdnSave.Value == "false")
        //        {
        //            btnSave.Enabled = false;
        //        }
        //        else
        //        {
        //            btnSave.Enabled = true;
        //        }
        //        if (Request["Screen"] == "ResultViewOrder")
        //        {
        //            btnMoveToMa.Visible = false;
        //            //pnlFile.Visible = false;
        //        }

        //        if ((ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT") && Convert.ToUInt32(Request["PhysicianId"]) != 0)
        //        {
        //            string sTemp = string.Empty;
        //            string[] sPhyname = null;
        //            StaticLookupManager objstaticlookup = new StaticLookupManager();
        //            IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
        //            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        //            PhysicianManager objphysican = new PhysicianManager();
        //            PhysicianList = objphysican.GetphysiciannameByPhyID(Convert.ToUInt32(Request["PhysicianId"]));
        //            sPhyname = StaticLst[0].Value.Split('|');
        //            sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
        //            sTemp = sTemp.Replace("<Date>", "");
        //            sTemp = sTemp.Replace(" on ", "");
        //            chkPhyName.Text = sTemp;
        //            chkPhyName.Visible = true;
        //        }

        //        else
        //        {
        //            chkPhyName.Visible = false;
        //        }

        //    }
        //    else
        //    {
        //        DLC.txtDLC.Enabled = false;
        //        //txtProviderNotes.Enabled = false;
        //        txtMedicalAssistantNotes.Enabled = false;
        //        btnSave.Visible = false;
        //        btnMoveToMa.Visible = false;
        //        //btnMoveToNextProcess.Visible = false;
        //        //chkPhyName.Visible = false;
        //        if (Request["Screen"] == "ResultView" || Request["Screen"] == "OrderManagement")
        //        {
        //            if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //            {
        //                chkPhyName.Visible = true;
        //            }
        //            else
        //            {
        //                chkPhyName.Visible = false;
        //            }
        //            btnMoveToNextProcess.Visible = true;
        //        }
        //        else
        //        {
        //            btnMoveToNextProcess.Visible = false;
        //            chkPhyName.Visible = false;
        //        }
        //        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        //        DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        //txtProviderNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        //        //txtProviderNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        //txtProviderNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        //        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //    }

        //    if (Request["MA"] != null && Request["MA"] == "True")
        //    {
        //        btnMoveToMa.Text = "Move To Provider";
        //        chkPhyName.Visible = false;
        //    }


        //    if (Request["MoveToMA"] != null)
        //    {
        //        if (Request["MoveToMA"] == "True")
        //        {
        //            btnMoveToNextProcess.Visible = false;
        //            chkPhyName.Visible = false;
        //        }
        //    }
        //    //if (Request["CurrentProcess"] != null)
        //    //{
        //    //    string sCurrentProcess = Request["CurrentProcess"].Replace(" _", "_").ToString();


        //    //    //if ((sCurrentProcess == "MA_REVIEW" || sCurrentProcess == "BILLING_WAIT") && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
        //    //    //{
        //    //    //    btnMoveToNextProcess.Visible = false;
        //    //    //    chkPhyName.Visible = false;
        //    //    //    DLC.txtDLC.Enabled = true;
        //    //    //    //txtProviderNotes.Enabled = false;
        //    //    //    txtMedicalAssistantNotes.Enabled = false;
        //    //    //    txtMedicalAssistantNotes.Style.Add("background-color","#ebebe4");
        //    //    //}
        //    //    //else if ((sCurrentProcess == "RESULT_REVIEW" || sCurrentProcess == "BILLING_WAIT") && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
        //    //    //{
        //    //    //    btnMoveToNextProcess.Visible = false;
        //    //    //    chkPhyName.Visible = false;
        //    //    //    DLC.txtDLC.Enabled = false;
        //    //    //    //txtProviderNotes.Enabled = false;
        //    //    //    txtMedicalAssistantNotes.Enabled = false;
        //    //    //    txtMedicalAssistantNotes.Style.Add("background-color", "#ebebe4");
        //    //    //}
        //    //}
        //    txtMedicalAssistantNotes.Enabled = false;
        //    DLC.txtDLC.Enabled = false;

        //    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        //    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");

        //    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        //    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //    string sCurrentProcess = string.Empty;
        //    if (Request["CurrentProcess"] != null && Request["Type"] != "Results")
        //    {
        //        sCurrentProcess = Request["CurrentProcess"].Replace(" _", "_").ToString();
        //        if ((ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT") && sCurrentProcess == "RESULT_REVIEW")
        //        {
        //            DLC.txtDLC.Enabled = true;
        //            DLC.txtDLC.BackColor = System.Drawing.Color.White;
        //            DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //            DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        }
        //        else if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" && sCurrentProcess == "MA_RESULTS")
        //        {
        //            txtMedicalAssistantNotes.Enabled = true;
        //            txtMedicalAssistantNotes.BackColor = System.Drawing.Color.White;
        //            txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //            txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        //        }
        //    }

        //    if (PageViewResult.ContentUrl.EndsWith(".pdf") == true || tabView.MultiPage.SelectedPageView.ContentUrl.EndsWith(".pdf") == true)
        //    {
        //        btnPrintEducatnMaterial.Visible = true;
        //    }
        //    if (pnlTree.Visible == false)
        //    {
        //        try
        //        {
        //            tblView.Rows[2].Cells[0].Attributes.Remove("style");
        //            tblView.Rows[2].Cells[1].Attributes.Remove("style");
        //            //tblView.Rows[2].Cells[0].Width = "1%";
        //            //tblView.Rows[2].Cells[1].Width = "100%";
        //            tblView.Rows[2].Cells[1].ColSpan = 2;
        //        }
        //        catch
        //        {
        //            tblView.Rows[2].Cells[0].ColSpan = 2;
        //            tblView.Rows[2].Cells[0].Width = "100%";

        //        }

        //    }
        //}
        ////=======
        ////{
        ////    pbPlus.ImageUrl = "~/Resources/plus_new.gif";
        ////    if (!IsPostBack)
        ////    {
        ////        btnPrintEducatnMaterial.Visible = true;
        ////        Session["Status_Flag"] = null;
        ////        HumanManager objhumanmanager = new HumanManager();
        ////        IList<Human> humanList = null;
        ////        humanList = objhumanmanager.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt32(Request["HumanId"]));
        ////        if (humanList.Count > 0)
        ////        {
        ////            txtPatientName.Text = humanList[0].First_Name + " " + humanList[0].MI + " " + humanList[0].Last_Name;
        ////            txtSex.Text = humanList[0].Sex;
        ////            txtDOB.Text = humanList[0].Birth_Date.ToString("dd-MMM-yyyy");
        ////            txtAccountNo.Text = humanList[0].Id.ToString();
        ////            txtPatientType.Text = humanList[0].Human_Type;
        ////        }

        ////        UlHumanId = Convert.ToUInt32(Request["HumanId"]);
        ////        if (Request["Screen"] != null && Request["Screen"] == "Cmg")
        ////        {
        ////            pnlTree.Visible = false;
        ////            tblView.Rows[2].Cells[0].Width = "1%";
        ////            tblView.Rows[2].Cells[1].Width = "99%";
        ////            tblView.Rows[2].Cells[1].ColSpan = 2;

        ////            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        ////            filelist = objfileproxy.GetImagesforAnnotations(ClientSession.HumanId, Convert.ToUInt64(Request["OrderSubmitId"].ToString()), "ORDER");
        ////            IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
        ////            lstfile = (from doc in filelist
        ////                       where doc.Result_Master_ID == ClientSession.EncounterId && doc.Source == "ORDER"
        ////                       select doc).ToList<FileManagementIndex>();
        ////            if (lstfile.Count > 0)
        ////            {
        ////                if (lstfile[0].Source == "ORDER")
        ////                {
        ////                    tb = new RadTab();
        ////                    tb.Target = "tbOrder";
        ////                    tb.Text = "Result Files";
        ////                    tabView.Tabs.Add(tb);
        ////                    PageViewResult.ContentUrl = "frmOnlineDocuments.aspx?Screen=" + "CmgResult" + "&OrderSubmitId=" + Request["OrderSubmitId"].ToString();
        ////                    PageViewResult.Selected = true;
        ////                    // TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        ////            FileManagementDTO objdto = new FileManagementDTO();
        ////            objdto = objfileproxy.GetResultandExamListusing(UlHumanId);
        ////            filelist = objdto.FileManagementList;
        ////            result_master_list = objdto.ResultMasterList;
        ////            Session["FileList"] = filelist;
        ////            Session["ResultList"] = result_master_list;
        ////            loadlistforResult();
        ////            Session["Load"] = load_documenttype;
        ////        }

        ////        if (Request["Screen"] == "Demographic")
        ////        {
        ////            if (filelist.Count > 0)
        ////            {
        ////                if (filelist[0].Document_Type != "Results")
        ////                {
        ////                    Session["doc_type"] = filelist[0].Document_Type;
        ////                    loadTreeView(filelist[0].Document_Type, "");

        ////                    for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        ////                    {
        ////                        for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        ////                        {
        ////                            if (tvViewIndex.Nodes[i].Nodes[j].Value == filelist[0].File_Path)
        ////                            {
        ////                                tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        ////                                TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        ////                                break;
        ////                            }
        ////                        }
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    RadComboBoxItem tempItem = new RadComboBoxItem();
        ////                    tempItem.Text = "Results";
        ////                    tempItem.ToolTip = "Results";
        ////                    cboDocumentType.Items.Add(tempItem);
        ////                    cboDocumentType.Text = "Results";
        ////                    loadTreeView("Results", "");

        ////                }
        ////            }

        ////            else if (result_master_list.Count > 0)
        ////            {
        ////                RadComboBoxItem tempItem = new RadComboBoxItem();
        ////                tempItem.Text = "Results";
        ////                tempItem.ToolTip = "Results";
        ////                cboDocumentType.Items.Add(tempItem);
        ////                cboDocumentType.Text = "Results";
        ////                loadTreeView("Results", "");

        ////            }

        ////            txtProviderNotes.Enabled = false;
        ////            txtMedicalAssistantNotes.Enabled = false;
        ////            btnMoveToMa.Visible = false;
        ////            btnMoveToNextProcess.Visible = false;
        ////            btnSave.Visible = false;
        ////            chkPhyName.Visible = false;
        ////        }

        ////        else if (Request["Screen"] == "MyQ" || Request["Screen"] == "ResultView" || Request["Screen"] == "OrderManagement" || Request["Screen"] == "ResultViewOrder")
        ////        {
        ////            RadComboBoxItem tempItem = new RadComboBoxItem();
        ////            tempItem.Text = "Results";
        ////            tempItem.ToolTip = "Results";
        ////            cboDocumentType.Items.Add(tempItem);
        ////            cboDocumentType.Text = "Results";
        ////            Session["doc_type"] = "Results";
        ////            loadTreeView("Results", "");
        ////            pnlTree.Visible = false;
        ////            // Add by Suvarnni for Bug id 28606
        ////             string sTemp = string.Empty;
        ////             string[] sPhyname = null;
        ////             StaticLookupManager objstaticlookup = new StaticLookupManager();
        ////             IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
        ////             IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        ////             PhysicianManager objphysican = new PhysicianManager();
        ////             PhysicianList = objphysican.GetphysiciannameByPhyID(ClientSession.PhysicianId);
        ////             sPhyname = StaticLst[0].Value.Split('|');
        ////             sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
        ////             sTemp = sTemp.Replace("<Date>", "");
        ////             sTemp = sTemp.Replace(" on ", "");
        ////             chkPhyName.Text = sTemp;

        ////            for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        ////            {
        ////                for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        ////                {
        ////                    if (!tvViewIndex.Nodes[i].Nodes[j].Value.ToString().Contains(".tif"))
        ////                    {
        ////                        if (Convert.ToUInt64(tvViewIndex.Nodes[i].Nodes[j].Value) == Convert.ToUInt64(Request["OrderSubmitId"]))
        ////                        {
        ////                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        ////                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        ////                            break;
        ////                        }
        ////                    }
        ////                }
        ////            }

        ////            try
        ////            {
        ////                tblView.Rows[2].Cells[0].Width = "1%";
        ////                tblView.Rows[2].Cells[1].Width = "99%";
        ////                tblView.Rows[2].Cells[1].ColSpan = 2;
        ////            }
        ////            catch
        ////            {
        ////                tblView.Rows[2].Cells[0].ColSpan = 2;
        ////                tblView.Rows[2].Cells[0].Width = "100%";

        ////            }



        ////            objresultmaster = new ResultMaster();
        ////            ResultMasterManager masterProxy = new ResultMasterManager();
        ////            objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt32(Request["HumanId"]), Convert.ToUInt32(Request["OrderSubmitId"]));
        ////            if (objresultmaster != null && objresultmaster.Id!=0)
        ////            {
        ////                txtProviderNotes.Text = objresultmaster.Result_Review_Comments;
        ////                txtMedicalAssistantNotes.Text = objresultmaster.MA_Notes;
        ////                if (objresultmaster.Is_Electronic_Mode == string.Empty && (objresultmaster.Result_Review_Comments != string.Empty || objresultmaster.MA_Notes != string.Empty))
        ////                {
        ////                    Session["BoolNotes"] = true;
        ////                }
        ////                Session["Notes"] = objresultmaster;
        ////            }
        ////            else
        ////            {
        ////                Session["BoolNotes"] = false;
        ////            }

        ////            if (Request["Screen"] == "ResultView" || Request["Screen"] == "OrderManagement")
        ////            {
        ////                btnMoveToMa.Visible = false;
        ////                //btnMoveToNextProcess.Visible = false;
        ////                //chkPhyName.Visible = false;
        ////                btnSave.Enabled = false;
        ////                txtMedicalAssistantNotes.Enabled = false;
        ////                txtProviderNotes.Enabled = false;

        ////            }

        ////        }


        ////        else
        ////        {
        ////            if (Request["Screen"] != null && Request["Screen"] == "Cmg")
        ////            {
        ////                //do nothing
        ////            }
        ////            else
        ////            {
        ////                if (Request["Type"] != null && Request["Type"].ToString() != "Results")
        ////                {
        ////                    UlOrderSubmitId = Convert.ToUInt32(Request["OrderSubmitId"]);
        ////                    UlResultId = Convert.ToUInt32(Request["ResultId"]);
        ////                    order_type = Request["Type"];
        ////                    order_submit_id = UlOrderSubmitId;


        ////                    ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];
        ////                    IList<FileManagementIndex> temp_list = ((from doc in ilstscaninindex
        ////                                                             where doc.Id == Convert.ToUInt32(Request["IndexId"])
        ////                                                             select doc)).ToList<FileManagementIndex>();
        ////                    string Filename = temp_list[0].File_Path.Substring(temp_list[0].File_Path.LastIndexOf("/") + 1);
        ////                    string temp_file_name = Filename.Substring(Filename.LastIndexOf("_") + 1);
        ////                    string[] name = temp_file_name.Split('.');
        ////                    string file_name = temp_list[0].Document_Sub_Type + "_" + name[0];
        ////                    Session["doc_type"] = temp_list[0].Document_Type;
        ////                    loadTreeView(temp_list[0].Document_Type, "");
        ////                    for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        ////                    {
        ////                        for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        ////                        {
        ////                            if (tvViewIndex.Nodes[i].Nodes[j].Value == temp_list[0].File_Path)
        ////                            {
        ////                                tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        ////                                TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        ////                                break;
        ////                            }
        ////                        }
        ////                    }

        ////                }
        ////                else
        ////                {
        ////                    order_submit_id = Convert.ToUInt32(Request["OrderSubmitId"]);
        ////                    UlResultId = Convert.ToUInt32(Request["ResultId"]);

        ////                    if (!cboDocumentType.Items.Any(a => a.Text == "Results"))
        ////                    {
        ////                        RadComboBoxItem tempItem = new RadComboBoxItem();
        ////                        tempItem.Text = "Results";
        ////                        tempItem.ToolTip = "Results";
        ////                        cboDocumentType.Items.Add(tempItem);
        ////                        cboDocumentType.Text = "Results";
        ////                    }
        ////                    loadTreeView("Results", "");
        ////                    try
        ////                    {
        ////                        if (UlResultId == 0)
        ////                        {
        ////                            for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        ////                            {
        ////                                for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        ////                                {
        ////                                    if (Convert.ToUInt64(tvViewIndex.Nodes[i].Nodes[j].Value) == order_submit_id)
        ////                                    {
        ////                                        tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        ////                                        TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        ////                                        break;
        ////                                    }

        ////                                }
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            for (int i = 0; i < tvViewIndex.Nodes.Count; i++)
        ////                            {
        ////                                for (int j = 0; j < tvViewIndex.Nodes[i].Nodes.Count; j++)
        ////                                {

        ////                                    if (tvViewIndex.Nodes[i].Nodes[j].Target != "ORDERS" && tvViewIndex.Nodes[i].Nodes[j].Target != null && tvViewIndex.Nodes[i].Nodes[j].Target != string.Empty)
        ////                                    {
        ////                                        if (Convert.ToUInt64(tvViewIndex.Nodes[i].Nodes[j].Target) == UlResultId)
        ////                                        {
        ////                                            tvViewIndex.Nodes[i].Nodes[j].Selected = true;
        ////                                            TabLoad(tvViewIndex.Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[i].Nodes[j].Target);
        ////                                            break;
        ////                                        }
        ////                                    }

        ////                                }
        ////                            }

        ////                        }
        ////                    }
        ////                    catch
        ////                    {

        ////                    }

        ////                    objresultmaster = new ResultMaster();
        ////                    ResultMasterManager masterProxy = new ResultMasterManager();
        ////                    objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt32(Request["HumanId"]), Convert.ToUInt32(Request["OrderSubmitId"]));
        ////                    if (objresultmaster != null)
        ////                    {
        ////                        txtProviderNotes.Text = objresultmaster.Result_Review_Comments;
        ////                        txtMedicalAssistantNotes.Text = objresultmaster.MA_Notes;
        ////                        if (objresultmaster.Is_Electronic_Mode == string.Empty && (objresultmaster.Result_Review_Comments != string.Empty || objresultmaster.MA_Notes != string.Empty))
        ////                        {
        ////                            Session["BoolNotes"] = true;
        ////                        }
        ////                        Session["Notes"] = objresultmaster;
        ////                    }
        ////                    else
        ////                    {
        ////                        Session["BoolNotes"] = false;
        ////                    }
        ////                }
        ////            }

        ////            txtProviderNotes.Enabled = false;
        ////            txtMedicalAssistantNotes.Enabled = false;
        ////            btnMoveToMa.Visible = false;
        ////            btnMoveToNextProcess.Visible = false;
        ////            btnSave.Visible = false;
        ////            chkPhyName.Visible = false;


        ////        }

        ////        btnSave.Enabled = false;
        ////        hdnSave.Value = "false";
        ////    }


        ////    if (Request["Screen"] != null && Request["Screen"] == "MyQ" || Request["Screen"] == "ResultViewOrder")
        ////    {
        ////        txtProviderNotes.Enabled = true;
        ////        txtMedicalAssistantNotes.Enabled = true;
        ////        btnSave.Visible = true;
        ////        if(hdnSave.Value=="false")
        ////        {
        ////            btnSave.Enabled=false;
        ////        }
        ////        else
        ////        {
        ////            btnSave.Enabled=true;
        ////        }
        ////        if (Request["Screen"] == "ResultViewOrder")
        ////        {
        ////            btnMoveToMa.Visible = false;
        ////        }

        ////        if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
        ////        {
        ////            string sTemp = string.Empty;
        ////            string[] sPhyname = null;
        ////            StaticLookupManager objstaticlookup = new StaticLookupManager();
        ////            IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
        ////            IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
        ////            PhysicianManager objphysican = new PhysicianManager();
        ////            PhysicianList = objphysican.GetphysiciannameByPhyID(ClientSession.PhysicianId);
        ////            sPhyname = StaticLst[0].Value.Split('|');
        ////            sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
        ////            sTemp = sTemp.Replace("<Date>", "");
        ////            sTemp = sTemp.Replace(" on ", "");
        ////            chkPhyName.Text = sTemp;
        ////            chkPhyName.Visible = true;
        ////        }

        ////        else
        ////        {
        ////            chkPhyName.Visible = false;
        ////        }

        ////    }
        ////    else
        ////    {
        ////        txtProviderNotes.Enabled = false;
        ////        txtMedicalAssistantNotes.Enabled = false;
        ////        btnSave.Visible = false;
        ////        btnMoveToMa.Visible = false;                 
        ////        //btnMoveToNextProcess.Visible = false;
        ////        //chkPhyName.Visible = false;
        ////        if (Request["Screen"] == "ResultView" || Request["Screen"] == "OrderManagement")
        ////         {
        ////             if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
        ////             {
        ////                 chkPhyName.Visible = true;
        ////             }
        ////             else
        ////             {
        ////                 chkPhyName.Visible = false;                          
        ////             }
        ////             btnMoveToNextProcess.Visible = true; 
        ////         }
        ////         else
        ////         {
        ////             btnMoveToNextProcess.Visible = false;
        ////             chkPhyName.Visible = false;
        ////         }

        ////        txtProviderNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        ////        txtProviderNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        ////        txtProviderNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        ////        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
        ////        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
        ////        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
        ////    }

        ////    if(Request["MA"]!=null && Request["MA"] == "True")
        ////    {
        ////        btnMoveToMa.Text="Move To Provider";
        ////        chkPhyName.Visible = false;
        ////    }

        ////    if(Request["HumanID"]!=null)
        ////    ClientSession.HumanId =Convert.ToUInt64( Request["HumanID"]);

        ////if (Request["MoveToMA"] != null)
        ////{
        ////    if (Request["MoveToMA"] == "True")
        ////    {
        ////        btnMoveToNextProcess.Visible = false;
        ////        chkPhyName.Visible = false;
        ////    }                 
        ////}
        ////}
        ////>>>>>>> 1.66

        //public void loadlistforResult()
        //{
        //    filelist = (IList<FileManagementIndex>)Session["FileList"];
        //    result_master_list = (IList<ResultMaster>)Session["ResultList"];

        //    if (result_master_list.Count > 0)
        //    {
        //        ulong order_submit_id = 0;
        //        for (int i = 0; i < result_master_list.Count; i++)
        //        {
        //            if (result_master_list[i].Order_ID != 0)
        //            {
        //                if (result_master_list[i].Order_ID != order_submit_id)
        //                {
        //                    ilstresultmaster.Add(result_master_list[i]);
        //                    order_submit_id = result_master_list[i].Order_ID;
        //                }
        //            }
        //            else
        //            {
        //                ilstresultmaster.Add(result_master_list[i]);
        //                order_submit_id = result_master_list[i].Order_ID;
        //            }

        //        }
        //    }


        //    if (ilstresultmaster.Count > 0 && filelist.Count > 0)
        //    {
        //        ilstscaninindex = new List<FileManagementIndex>();
        //        for (int i = 0; i < filelist.Count; i++)
        //        {
        //            bool blScan = false;
        //            for (int j = 0; j < ilstresultmaster.Count; j++)
        //            {
        //                if (ilstresultmaster[j].Order_ID != 0)
        //                {
        //                    if (filelist[i].Order_ID == ilstresultmaster[j].Order_ID)
        //                    {
        //                        ilstresultmaster[j].Is_Scan_order = "Y";
        //                        blScan = true;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (blScan == false)
        //            {
        //                if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
        //                {
        //                    ilstscaninindex.Add(filelist[i]);
        //                }
        //                else if (filelist[i].Order_ID != 0)
        //                {
        //                    ilstscaninindex.Add(filelist[i]);
        //                }

        //            }
        //        }
        //    }

        //    else if ((ilstresultmaster.Count == 0 && filelist.Count > 0))
        //    {
        //        for (int i = 0; i < filelist.Count; i++)
        //        {
        //            ilstscaninindex.Add(filelist[i]);
        //        }
        //    }

        //    if (ilstresultmaster.Count > 0)
        //    {
        //        ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();
        //    }
        //    if (ilstscaninindex.Count > 0)
        //    {
        //        ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();
        //    }

        //    Session["IndexList"] = ilstscaninindex;
        //    Session["ResultMasterList"] = ilstresultmaster;
        //}

        //public void loadTreeView(string document_type, string document_subtype)
        //{
        //    if (Session["Load"] != null)
        //        load_documenttype = (bool)Session["Load"];
        //    ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];
        //    ilstresultmaster = (IList<ResultMaster>)Session["ResultMasterList"];

        //    if (document_subtype == "")
        //        Session["doc_sub_type"] = "ALL_RESULTS";

        //    if (load_documenttype == true)
        //    {
        //        load_documenttype = false;
        //        cboDocumentType.Items.Clear();
        //        if (ilstscaninindex.Count > 0)
        //        {
        //            IList<string> docType = ((from doc in ilstscaninindex
        //                                      orderby doc.Document_Type ascending
        //                                      select doc.Document_Type).Distinct()).ToList<string>();

        //            IList<string> documentType = new List<string>();


        //            bool blresult = false;
        //            for (int i = 0; i < docType.Count; i++)
        //            {
        //                if (docType[i] == "DIAGNOSTIC ORDER" || docType[i] == "IMAGE ORDER")
        //                {
        //                    if (blresult == false)
        //                    {
        //                        docType[i] = "Results";
        //                        documentType.Add(docType[i]);
        //                    }
        //                    blresult = true;
        //                }
        //                else
        //                {
        //                    documentType.Add(docType[i]);
        //                }
        //            }

        //            for (int i = 0; i < documentType.Count; i++)
        //            {
        //                RadComboBoxItem tempItem = new RadComboBoxItem();
        //                tempItem.Text = documentType[i];
        //                tempItem.ToolTip = documentType[i];
        //                cboDocumentType.Items.Add(tempItem);
        //            }

        //            if (result_master_list.Count > 0)
        //            {
        //                if (!cboDocumentType.Items.Any(a => a.Text.Trim() == "Results"))
        //                {
        //                    RadComboBoxItem tempItem = new RadComboBoxItem();
        //                    tempItem.Text = "Results";
        //                    tempItem.ToolTip = "Results";
        //                    cboDocumentType.Items.Add(tempItem);

        //                }
        //            }

        //            if (Request["Type"] == "Results")
        //            {
        //                cboDocumentType.FindItemByText("Results").Selected = true;
        //            }
        //            else
        //            {
        //                if (cboDocumentType.Items.Count > 0 && Session["doc_type"] != null)
        //                {
        //                    cboDocumentType.FindItemByText((string)Session["doc_type"]).Selected = true;
        //                }
        //                else if (cboDocumentType.Items.Count > 0 && Session["doc_type"] == null && document_type != string.Empty && document_type != "")
        //                {
        //                    cboDocumentType.FindItemByText(document_type).Selected = true;
        //                }
        //                //cboDocumentType.FindItemByText((string)Session["doc_type"]).Selected = true;
        //            }
        //        }
        //        else
        //        {
        //            if (!cboDocumentType.Items.Any(a => a.Text == "Results"))
        //            {
        //                RadComboBoxItem tempItem = new RadComboBoxItem();
        //                tempItem.Text = "Results";
        //                tempItem.ToolTip = "Results";
        //                cboDocumentType.Items.Add(tempItem);
        //                //cboDocumentType.Text == "Results"
        //            }

        //        }

        //        load_documenttype = false;
        //        Session["Load"] = load_documenttype;
        //    }
        //    tvViewIndex.Nodes.Clear();
        //    IList<FileManagementIndex> lstDoc_Type = new List<FileManagementIndex>();
        //    IList<string> documentSubType;
        //    if (document_subtype == "")
        //    {
        //        if (ilstscaninindex.Count > 0)
        //        {
        //            //if (document_type.ToUpper() == "RESULTS")
        //            //{
        //            //    lstDoc_Type = ((from doc in ilstscaninindex
        //            //                    where doc.Document_Type == "DIAGNOSTIC ORDER"
        //            //                    select doc)).ToList<FileManagementIndex>();

        //            //    documentSubType = ((from doc in ilstscaninindex
        //            //                        where doc.Document_Type == "DIAGNOSTIC ORDER"
        //            //                        select doc.Document_Sub_Type).Distinct()).ToList<string>();

        //            //}
        //            //else
        //            //{
        //            lstDoc_Type = ((from doc in ilstscaninindex
        //                            where doc.Document_Type == document_type
        //                            select doc)).ToList<FileManagementIndex>();

        //            documentSubType = ((from doc in ilstscaninindex
        //                                where doc.Document_Type == cboDocumentType.SelectedItem.Text
        //                                select doc.Document_Sub_Type).Distinct()).ToList<string>();

        //            //}
        //            txtDocumentSubType.Text = "";


        //            for (int j = 0; j < documentSubType.Count; j++)
        //            {
        //                if (txtDocumentSubType.Text == string.Empty)
        //                {
        //                    txtDocumentSubType.Text = documentSubType[j];
        //                }
        //                else
        //                {
        //                    txtDocumentSubType.Text = txtDocumentSubType.Text + "," + documentSubType[j];
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!cboDocumentType.Items.Any(a => a.Text.Trim() == "Results"))
        //            {
        //                RadComboBoxItem tempItem = new RadComboBoxItem();
        //                tempItem.Text = "Results";
        //                tempItem.ToolTip = "Results";
        //                cboDocumentType.Items.Add(tempItem);
        //                //cboDocumentType.Text == "Results";

        //            }
        //        }


        //    }
        //    else if (document_subtype != string.Empty)
        //    {
        //        txtDocumentSubType.Text = "";
        //        txtDocumentSubType.Text = document_subtype;
        //        foreach (string item in document_subtype.Split(','))
        //        {
        //            IList<FileManagementIndex> lstDoc = ((from doc in ilstscaninindex
        //                                                  where doc.Document_Type == document_type && doc.Document_Sub_Type == item
        //                                                  select doc)).ToList<FileManagementIndex>();
        //            lstDoc_Type = lstDoc_Type.Concat(lstDoc).ToList();

        //        }


        //    }

        //    if (lstDoc_Type != null && lstDoc_Type.Count > 0)
        //    {
        //        /* File Header */
        //        //txtFileName.Text = lstDoc_Type[0].File_Path.Substring(lstDoc_Type[0].File_Path.LastIndexOf("/") + 1);
        //        //txtDocType.Text = lstDoc_Type[0].Document_Type.ToString();
        //        //txtSdocType.Text = lstDoc_Type[0].Document_Sub_Type.ToString();
        //        //txtDocDate.Text = lstDoc_Type[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");
        //        txtFileInformation.Value = lstDoc_Type[0].File_Path.Substring(lstDoc_Type[0].File_Path.LastIndexOf("/") + 1) + " | " + lstDoc_Type[0].Document_Type.ToString() + " | " + lstDoc_Type[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstDoc_Type[0].Created_Date_And_Time).ToString("dd-MMM-yyyy");//BugID:42809
        //    }

        //    TreeView(lstDoc_Type);

        //    tvViewIndex.ExpandAllNodes();

        //}

        //public void TreeView(IList<FileManagementIndex> lstDoc_Type)
        //{

        //    if (cboDocumentType.SelectedItem.Text == "Results")//Added a condition to check "Type" to handle scanned files of DOC_TYPE "results" BugID:43099
        //    {
        //        tvViewIndex.Nodes.Clear();
        //        TreeviewforResults();
        //    }
        //    else
        //    {
        //        for (int j = 0; j < lstDoc_Type.Count; j++)
        //        {
        //            if (lstDoc_Type[j].Source == "SCAN")
        //            {
        //                RadTreeNode tnPatientchart = new RadTreeNode();
        //                RadTreeNode NodeName = null;
        //                RadTreeNode DateOfDocument = null;
        //                RadTreeNode tnMainNode = tvViewIndex.Nodes.FindNodeByText(lstDoc_Type[j].Document_Date.ToString("dd-MMM-yyyy"));

        //                if (tnMainNode != null)
        //                {
        //                    tnPatientchart = tnMainNode;
        //                    DateOfDocument = tnPatientchart;
        //                }

        //                else
        //                {
        //                    string sDocDate = lstDoc_Type[j].Document_Date.ToString("dd-MMM-yyyy");

        //                    NodeName = new RadTreeNode();
        //                    NodeName.Text = lstDoc_Type[j].Document_Sub_Type.ToString();
        //                    DateOfDocument = new RadTreeNode();
        //                    //DateOfDocument.Name = lstDoc_Type[j].Document_Date.ToLocalTime().ToString("dd-MMM-yyyy");
        //                    DateOfDocument.Text = UtilityManager.ConvertToLocal(lstDoc_Type[j].Document_Date).ToString("dd-MMM-yyyy");
        //                    NodeName.Nodes.Add(DateOfDocument);
        //                    tvViewIndex.Nodes.Add(NodeName);

        //                }

        //                string File_Name = lstDoc_Type[j].File_Path.Substring(lstDoc_Type[j].File_Path.LastIndexOf("/") + 1);
        //                string file = File_Name.Substring(File_Name.LastIndexOf("_") + 1);
        //                string[] temp_name = file.Split('.');
        //                string filename = lstDoc_Type[j].Document_Sub_Type + "_" + temp_name[0];

        //                RadTreeNode tnDateOfDocument = DateOfDocument.Nodes.FindNodeByText(filename);

        //                if (tnDateOfDocument == null)
        //                {
        //                    RadTreeNode SubnodeforImagepath = new RadTreeNode();
        //                    SubnodeforImagepath.Text = filename;
        //                    //SubnodeforImagepath.Name = File_Name;
        //                    SubnodeforImagepath.Value = lstDoc_Type[j].File_Path;
        //                    SubnodeforImagepath.Target = lstDoc_Type[j].Id.ToString();//Recent change for BugID:43099
        //                    DateOfDocument.Nodes.Add(SubnodeforImagepath);
        //                }
        //            }
        //        }
        //    }
        //}

        //public void TreeviewforResults()
        //{
        //    tvViewIndex.Nodes.Clear();
        //    ////// Result Master///////
        //    RadTreeNode tnResults = new RadTreeNode();
        //    RadTreeNode tnPatDischargeSummary = new RadTreeNode();
        //    RadTreeNode tnPatClinicalSummary = new RadTreeNode();
        //    RadTreeNode tnPatEdu = new RadTreeNode();
        //    tnPatClinicalSummary.Text = "Patient Clinical Summary";
        //    tnPatEdu.Text = "Patient Education";
        //    tnPatDischargeSummary.Text = "Patient Discharge Summary";
        //    //tnResults.Name = "Results";
        //    tnResults.Text = "Results";
        //    tvViewIndex.Nodes.Add(tnResults);



        //    ////// Result Master///////
        //    ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];
        //    ilstresultmaster = (IList<ResultMaster>)Session["ResultMasterList"];
        //    //vince Feb-08-2013-Start
        //    if (result_list.Count > 0)
        //    {
        //        result_list.Clear();
        //    }

        //    for (int i = 0; i < ilstresultmaster.Count; i++)
        //    {
        //        if (ilstresultmaster[i].MSH_Date_And_Time_Of_Message != string.Empty)
        //        {
        //            Result_Date_Order objresult = new Result_Date_Order();
        //            string formatString = "yyyyMMddHHmmss";
        //            string sample = ilstresultmaster[i].MSH_Date_And_Time_Of_Message;
        //            if (sample.Contains('-'))
        //            {
        //                sample = sample.Split('-')[0].ToString();
        //            }
        //            if (sample.Length == 14)
        //                formatString = "yyyyMMddHHmmss";
        //            else if (sample.Length == 12)
        //                formatString = "yyyyMMddHHmm";

        //            DateTime dt = DateTime.ParseExact(sample, formatString, null);

        //            objresult.Result_Date = dt;
        //            objresult.Result_Id = ilstresultmaster[i].Id;
        //            objresult.Img_Path = string.Empty;
        //            objresult.Order_Submit_Id = ilstresultmaster[i].Order_ID;
        //            objresult.Ord_Type = ilstresultmaster[i].Order_Type;
        //            objresult.Order_Description = ilstresultmaster[i].Orders_Description;
        //            result_list.Add(objresult);
        //        }
        //    }

        //    ulong ord_sb_id = 0;
        //    bool CheckCondition = true;
        //    if (Session["doc_sub_type"] != null && Session["doc_sub_type"] == "ALL_RESULTS")
        //    {
        //        CheckCondition = false;
        //    }
        //    for (int i = 0; i < ilstscaninindex.Count; i++)
        //    {
        //        //if (ilstscaninindex[i].Order_ID != 0)
        //        //{
        //        //    if (ord_sb_id != ilstscaninindex[i].Order_ID)
        //        //    {
        //        //        Result_Date_Order objresult = new Result_Date_Order();

        //        //        objresult.Result_Date = ilstscaninindex[i].Created_Date_And_Time;
        //        //        objresult.Result_Id = 0;
        //        //        objresult.Img_Path = ilstscaninindex[i].File_Path;
        //        //        objresult.Order_Submit_Id = ilstscaninindex[i].Order_ID;
        //        //        objresult.Ord_Type = ilstscaninindex[i].Document_Type;
        //        //        objresult.Order_Description = ilstscaninindex[i].Orders_Description;
        //        //        objresult.IndexId = ilstscaninindex[i].Id;
        //        //        result_list.Add(objresult);
        //        //        if (Request.QueryString["Screen"] != null)
        //        //        {
        //        //            if (Request.QueryString["Screen"] != "ResultView" && Request.QueryString["Screen"] != "OrderManagement" && Request.QueryString["Screen"] != "ResultViewOrder")
        //        //            { ord_sb_id = ilstscaninindex[i].Order_ID; }
        //        //        }

        //        //    }
        //        //}
        //        if (CheckCondition)
        //        {
        //            if (ilstscaninindex[i].Document_Type.ToUpper().Trim() == "RESULTS" && ilstscaninindex[i].Document_Sub_Type.ToUpper().Trim() == "LABORATORY")
        //            {
        //                Result_Date_Order objresult = new Result_Date_Order();

        //                objresult.Result_Date = ilstscaninindex[i].Document_Date;
        //                objresult.Result_Id = 0;
        //                objresult.Img_Path = ilstscaninindex[i].File_Path;
        //                objresult.Order_Submit_Id = ilstscaninindex[i].Order_ID;
        //                objresult.Ord_Type = ilstscaninindex[i].Document_Type;
        //                objresult.Order_Description = ilstscaninindex[i].Orders_Description;
        //                objresult.IndexId = ilstscaninindex[i].Id;
        //                result_list.Add(objresult);
        //                if (Request.QueryString["Screen"] != null)
        //                {
        //                    if (Request.QueryString["Screen"] != "ResultView" && Request.QueryString["Screen"] != "OrderManagement" && Request.QueryString["Screen"] != "ResultViewOrder")
        //                    { ord_sb_id = ilstscaninindex[i].Order_ID; }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Result_Date_Order objresult = new Result_Date_Order();

        //            objresult.Result_Date = ilstscaninindex[i].Document_Date;
        //            objresult.Result_Id = 0;
        //            objresult.Img_Path = ilstscaninindex[i].File_Path;
        //            objresult.Order_Submit_Id = ilstscaninindex[i].Order_ID;
        //            objresult.Ord_Type = ilstscaninindex[i].Document_Type;
        //            objresult.Order_Description = ilstscaninindex[i].Orders_Description;
        //            objresult.IndexId = ilstscaninindex[i].Id;
        //            result_list.Add(objresult);
        //            if (Request.QueryString["Screen"] != null)
        //            {
        //                if (Request.QueryString["Screen"] != "ResultView" && Request.QueryString["Screen"] != "OrderManagement" && Request.QueryString["Screen"] != "ResultViewOrder")
        //                { ord_sb_id = ilstscaninindex[i].Order_ID; }
        //            }
        //        }

        //    }

        //    result_list = result_list.OrderByDescending(a => a.Result_Date).ToList<Result_Date_Order>();
        //    Session["ResultOrderList"] = result_list;

        //    for (int i = 0; i < result_list.Count; i++)
        //    {
        //        //Result Master//
        //        if (result_list[i].Result_Id != 0)
        //        {

        //            DateTime dt = result_list[i].Result_Date;
        //            string sDate = UtilityManager.ConvertToLocal(dt).ToString("dd-MMM-yyyy hh:mm tt").ToString();
        //            //string sDate = dt.ToString("dd-MMM-yyyy hh:mm tt").ToString();
        //            RadTreeNode tnDateofDoc = new RadTreeNode();
        //            //tnDateofDoc.Name = sDate;
        //            tnDateofDoc.Text = sDate;
        //            tnDateofDoc.ToolTip = result_list[i].Order_Description;
        //            tnDateofDoc.Value = result_list[i].Order_Submit_Id.ToString();
        //            tnDateofDoc.Target = result_list[i].Result_Id.ToString();

        //            if (result_list[i].Order_Description.Split('-')[0].Trim().ToUpper() == "Patient Discharge Summary".Trim().ToUpper())
        //            {
        //                if (tnPatDischargeSummary.Nodes.Count == 0)
        //                {
        //                    tvViewIndex.Nodes.Add(tnPatDischargeSummary);
        //                }
        //                tnPatDischargeSummary.Nodes.Add(tnDateofDoc);
        //            }

        //            else if (result_list[i].Order_Description.Split('-')[0].Trim().ToUpper() == "Patient Clinical Summary".Trim().ToUpper())
        //            {
        //                if (tnPatClinicalSummary.Nodes.Count == 0)
        //                {
        //                    tvViewIndex.Nodes.Add(tnPatClinicalSummary);
        //                }
        //                tnPatClinicalSummary.Nodes.Add(tnDateofDoc);
        //            }
        //            else if (result_list[i].Order_Description.Split('-')[0].Trim().ToUpper() == "Pat Edu".Trim().ToUpper())
        //            {
        //                if (tnPatEdu.Nodes.Count == 0)
        //                {
        //                    tvViewIndex.Nodes.Add(tnPatEdu);
        //                }
        //                tnPatEdu.Nodes.Add(tnDateofDoc);
        //            }
        //            else
        //                tnResults.Nodes.Add(tnDateofDoc);

        //        }
        //        //Scanning
        //        else
        //        {
        //            RadTreeNode tnResults_node = null;
        //            RadTreeNode tnResult = null;
        //            RadTreeNode tn_encounterNode = tvViewIndex.Nodes.FindNodeByText("Results");
        //            tnResults_node = tn_encounterNode;

        //            if (tnResults_node != null)
        //                tnResult = tnResults_node;
        //            else
        //            {
        //                tnResult = new RadTreeNode();
        //                //tnResult.Name = "Results";
        //                tnResult.Text = "Results";
        //                tnResult.Nodes.Add(tnResult);
        //            }


        //            string sDocDate = result_list[i].Result_Date.ToString("dd-MMM-yyyy hh:mm tt");

        //            RadTreeNode tnDateOfDocument = new RadTreeNode();
        //            tnDateOfDocument = new RadTreeNode();
        //            //tnDateOfDocument.Name = result_list[i].Result_Date.ToLocalTime().ToString("dd-MMM-yyyy hh:mm tt");
        //            if (Request.QueryString["Screen"] != null)
        //            {
        //                if (Request.QueryString["Screen"] == "ResultView" || Request.QueryString["Screen"] == "OrderManagement")
        //                {
        //                    tnDateOfDocument.Target = result_list[i].Img_Path.ToString();
        //                }
        //            }
        //            else if (Request["Type"] == "Results")
        //            {
        //                tnDateOfDocument.Target = Convert.ToString(result_list[i].IndexId);
        //            }
        //            else
        //            {
        //                tnDateOfDocument.Target = "ORDERS";
        //            }
        //            tnDateOfDocument.Text = UtilityManager.ConvertToLocal(result_list[i].Result_Date).ToString("dd-MMM-yyyy hh:mm tt");

        //            tnDateOfDocument.ToolTip = result_list[i].Order_Description;
        //            //strToolTip = tnDateOfDocument.ToolTip;
        //            //Mytooltip = new ToolTip();
        //            tnDateOfDocument.Value = result_list[i].Order_Submit_Id.ToString();
        //            tnDateOfDocument.Target = result_list[i].IndexId.ToString();
        //            if (Request["Type"] != null && (Request["Type"] == "File" || Request["Type"] == "Results"))
        //            {
        //                tnDateOfDocument.Value = result_list[i].Img_Path.ToString();
        //                tnDateOfDocument.Target = result_list[i].IndexId.ToString();
        //            }
        //            tnResult.Nodes.Add(tnDateOfDocument);
        //        }

        //    }
        //}

        //protected void cboDocumentType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    loadTreeView(cboDocumentType.Text, "");
        //    pbPlus.ID = "pbPlus";

        //}

        //protected void tvViewIndex_NodeClick(object sender, RadTreeNodeEventArgs e)
        //{

        //    if (Request.QueryString["Screen"] != null)
        //    {

        //        if (Request.QueryString["Screen"] == "ResultView" || Request.QueryString["Screen"] == "OrderManagement")
        //        {
        //            PageViewScan.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + e.Node.Target;
        //            PageViewScan.Selected = true;
        //            //if (Path.GetExtension(e.Node.Target).ToUpper().Trim() == ".CSV")
        //            //{
        //            //    if (!tabView.Tabs.Any(t => t.Text.ToUpper().Contains("ABI RESULTS")))
        //            //    {
        //            //        tb = new RadTab();
        //            //        tb.Text = "ABI Results";
        //            //        tabView.Tabs.Add(tb);
        //            //    }
        //            //    PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Request["HumanId"].ToString();
        //            //    PageViewABIResults.Selected = true;
        //            //}
        //            //else
        //            //{
        //            //    if (!tabView.Tabs.Any(t => t.Text.ToUpper().Contains("RESULT FILES")))
        //            //    {
        //            //        tb = new RadTab();
        //            //        tb.Text = "Result Files";
        //            //        tabView.Tabs.Add(tb);
        //            //    }
        //            //    PageViewScan.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + e.Node.Target;
        //            //    PageViewScan.Selected = true;
        //            //    tabView.Tabs.Where(t => t.Text.ToUpper().Contains("RESULT FILES")).Select(a => a.Selected = true);
        //            //}
        //        }
        //        if (Request.QueryString["Screen"] == "MyQ")
        //        {
        //            TabLoad(e.Node.Value.ToString(), e.Node.Target.ToString());
        //        }
        //    }
        //    else
        //    {
        //        TabLoad(e.Node.Value.ToString(), e.Node.Target.ToString());
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    SaveNotes();
        //    //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        //public void TabLoad(string KeyValue, string TargetValue)
        //{
        //    Session["Order_Id"] = 0;
        //    string sfileExtension = (Path.GetExtension(KeyValue.ToString())).ToString();
        //    filelist = (IList<FileManagementIndex>)Session["FileList"];

        //    txtMedicalAssistantNotes.Text = string.Empty;
        //    DLC.txtDLC.Text = string.Empty;
        //    //Srividhya added the file types other than tif on 2-jan-2015-bug id:29130
        //    if (sfileExtension == ".tif" || sfileExtension == ".pdf" || sfileExtension == ".png" || sfileExtension == ".gif" || sfileExtension == ".bmp" || sfileExtension == ".jpg" || sfileExtension == ".jpeg" || sfileExtension == ".JPG" || sfileExtension == ".dcm")
        //    {
        //        //BEGIN:---To change the title whenever a subnode is clicked BugID:43099
        //        ulong file_id = Convert.ToUInt64(TargetValue);
        //        IList<FileManagementIndex> lstScanFiles = (from doc in filelist where doc.Id == file_id select doc).ToList<FileManagementIndex>();
        //        if (lstScanFiles.Count > 0)
        //            txtFileInformation.Value = lstScanFiles[0].File_Path.Substring(lstScanFiles[0].File_Path.LastIndexOf("/") + 1) + " | " + lstScanFiles[0].Document_Type.ToString() + " | " + lstScanFiles[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstScanFiles[0].Created_Date_And_Time).ToString("dd-MMM-yyyy");
        //        //END:---
        //        tabView.Tabs.Clear();
        //        PageViewScan.ContentUrl = string.Empty;
        //        PageViewResult.ContentUrl = string.Empty;
        //        PageViewResultFiles.ContentUrl = string.Empty;
        //        PageViewSpirometryResults.ContentUrl = string.Empty;
        //        PageViewABIResults.ContentUrl = string.Empty;
        //        PageViewMessageLog.ContentUrl = string.Empty;
        //        PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + KeyValue.ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Request["HumanId"].ToString();
        //        PageViewScan.Selected = true;


        //    }
        //    else
        //    {
        //        tabView.Tabs.Clear();
        //        PageViewScan.ContentUrl = string.Empty;
        //        PageViewResult.ContentUrl = string.Empty;
        //        PageViewResultFiles.ContentUrl = string.Empty;
        //        PageViewSpirometryResults.ContentUrl = string.Empty;
        //        PageViewABIResults.ContentUrl = string.Empty;
        //        PageViewMessageLog.ContentUrl = string.Empty;

        //        ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];
        //        ilstresultmaster = (IList<ResultMaster>)Session["ResultMasterList"];
        //        result_master_list = (IList<ResultMaster>)Session["ResultList"];

        //        if (cboDocumentType.Text == "Results")
        //        {
        //            try
        //            {

        //                objresultmaster = new ResultMaster();
        //                ResultMasterManager masterProxy = new ResultMasterManager();
        //                objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt32(Request["HumanId"]), Convert.ToUInt64(KeyValue));
        //                if (objresultmaster != null)
        //                {
        //                    DLC.txtDLC.Text = objresultmaster.Result_Review_Comments;
        //                    txtMedicalAssistantNotes.Text = objresultmaster.MA_Notes;
        //                    if (objresultmaster.Is_Electronic_Mode == string.Empty && (objresultmaster.Result_Review_Comments != string.Empty || objresultmaster.MA_Notes != string.Empty))
        //                    {
        //                        Session["BoolNotes"] = true;
        //                    }
        //                    Session["Notes"] = objresultmaster;
        //                }
        //                else
        //                {
        //                    Session["BoolNotes"] = false;
        //                }
        //                ulong order_id = Convert.ToUInt64(KeyValue);
        //                Session["Order_Id"] = order_id;
        //                ulong Result_Master_Id = 0;
        //                IList<string> Tab_Name = new List<string>();
        //                if (order_id != 0)
        //                {
        //                    if (result_master_list.Count > 0)
        //                    {
        //                        IList<ResultMaster> lstresult = new List<ResultMaster>();
        //                        lstresult = ((from doc in result_master_list
        //                                      where doc.Order_ID == order_id
        //                                      orderby doc.Id descending
        //                                      select doc)).ToList<ResultMaster>();

        //                        if (lstresult.Count > 0)
        //                        {
        //                            Result_Master_Id = lstresult[0].Id;
        //                            Session["Result_Master_Id"] = Result_Master_Id;
        //                            Tab_Name.Add("Result Master");
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    Result_Master_Id = Convert.ToUInt64(TargetValue);
        //                    Tab_Name.Add("Result Master");

        //                }

        //                if (filelist.Count > 0)
        //                {
        //                    if (order_id != 0)
        //                    {
        //                        IList<string> lstfile = new List<string>();
        //                        lstfile = ((from doc in filelist
        //                                    where doc.Order_ID == order_id
        //                                    orderby doc.Id descending
        //                                    select doc.Source).Distinct()).ToList<string>();


        //                        if (lstfile.Count > 0)
        //                        {
        //                            for (int i = 0; i < lstfile.Count; i++)
        //                            {
        //                                Tab_Name.Add(lstfile[i]);
        //                            }
        //                        }
        //                    }
        //                }

        //                for (int i = 0; i < Tab_Name.Count; i++)
        //                {
        //                    tb = new RadTab();
        //                    tb.Target = "tb" + Tab_Name[i].ToLower();
        //                    if (Tab_Name[i].ToLower() == "scan")
        //                    {
        //                        btnPrintEducatnMaterial.Visible = false;
        //                        IList<string> lstScanId = ((from doc in filelist
        //                                                    where doc.Order_ID == order_id
        //                                                    select doc.File_Path).Distinct()).ToList<string>();
        //                        tb.Text = "Scanned Results";
        //                        /* File Header */
        //                        IList<FileManagementIndex> lstScanIds = (from doc in filelist where doc.Order_ID == order_id select doc).ToList<FileManagementIndex>();

        //                        //txtFileName.Text = lstScanIds[0].File_Path.Substring(lstScanIds[0].File_Path.LastIndexOf("/") + 1);
        //                        //txtDocType.Text = "Results";
        //                        //txtSdocType.Text = lstScanIds[0].Document_Sub_Type.ToString();
        //                        //txtDocDate.Text = lstScanIds[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");
        //                        txtFileInformation.Value = lstScanIds[0].File_Path.Substring(lstScanIds[0].File_Path.LastIndexOf("/") + 1) + " | " + "Results" + " | " + lstScanIds[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstScanIds[0].Created_Date_And_Time).ToString("dd-MMM-yyyy");//BugID:42809
        //                    }

        //                    else if (Tab_Name[i].ToLower() == "order")
        //                    {
        //                        tb.Text = "Result Files";
        //                    }
        //                    else if (Tab_Name[i].ToLower() == "result master")
        //                    {
        //                        //Session["Result_Master_Id"] = Result_Master_Id;
        //                        //tb.Text = "Results";

        //                        //ulong Result_Master_ID;
        //                        //Result_Master_ID = (ulong)Session["Result_Master_Id"];
        //                        PDFGenerator objPDFGenerator = new PDFGenerator();
        //                        string filepath = Server.MapPath("Documents/" + Session.SessionID);
        //                        hdnSelectedItem.Value = String.Empty;
        //                        string filename = objPDFGenerator.GenerateRequestionForLabcorp(Result_Master_Id, filepath);
        //                        string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
        //                        string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
        //                        string file = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
        //                        string loc = "DYNAMIC";
        //                        if (hdnSelectedItem.Value == string.Empty)
        //                        {
        //                            hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
        //                            /* File Header */
        //                            //txtFileName.Text = FileName[0].ToString();
        //                            //txtDocType.Text = "Results";
        //                            //txtSdocType.Text = "";
        //                            //txtDocDate.Text = "";

        //                            txtFileInformation.Value = FileName[0].ToString() + " | " + "Results";
        //                            tb.Value = order_id.ToString();
        //                            tabView.Tabs.Add(tb);
        //                        }
        //                        else
        //                        {
        //                            hdnSelectedItem.Value += "|" + FileName[0].ToString();
        //                        }
        //                        PageViewResult.ContentUrl = hdnSelectedItem.Value.ToString();
        //                        PageViewResult.Selected = true;

        //                    }
        //                    else if (Tab_Name[i].ToLower() == "abi")
        //                    {
        //                        tb.Text = "ABI Results";
        //                        tb.Value = order_id.ToString();
        //                        tabView.Tabs.Add(tb);

        //                    }
        //                    else if (Tab_Name[i].ToLower() == "spirometry")
        //                    {
        //                        tb.Text = "Spirometry Results";
        //                        tb.Value = order_id.ToString();
        //                        tabView.Tabs.Add(tb);

        //                    }
        //                    else if (Tab_Name[i].ToLower() == "message")
        //                    {
        //                        tb.Text = "Message Log";
        //                        tb.Value = order_id.ToString();
        //                        tabView.Tabs.Add(tb);
        //                    }



        //                }

        //                for (int i = 0; i < Tab_Name.Count; i++)
        //                {
        //                    if (Tab_Name[i].ToLower() == "scan")
        //                    {
        //                        IList<string> lstScanId = ((from doc in filelist
        //                                                    where doc.Order_ID == order_id
        //                                                    select doc.File_Path).Distinct()).ToList<string>();
        //                        if (filelist.Count == 1 && filelist.Count > 0)
        //                        {
        //                            /* Changing The Image Viewer To View the Multiple Results Attached To Same Order  */
        //                            PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Request["HumanId"].ToString();
        //                        }
        //                        else
        //                        {
        //                            /* To Avoid Alteration Of Existing Working , Left The Code Block For Single Result To Order Mapping */
        //                            PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Request["HumanId"].ToString();
        //                        }
        //                        PageViewScan.Selected = true;
        //                        break;
        //                    }

        //                    else if (Tab_Name[i].ToLower() == "order")
        //                    {
        //                        FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        //                        filelist = objfileproxy.GetImagesforAnnotations(Convert.ToUInt32(Session["HumanId"]), Convert.ToUInt64(KeyValue), "ORDER");
        //                        IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
        //                        lstfile = (from doc in filelist
        //                                   where doc.Source == "ORDER"
        //                                   select doc).ToList<FileManagementIndex>();
        //                        if (lstfile.Count > 0)
        //                        {
        //                            if (lstfile[0].Source == "ORDER")
        //                            {
        //                                string concatenatedFiles = string.Empty;

        //                                string filename = string.Empty;
        //                                lstfile.OrderBy(a => a.Id).FirstOrDefault();
        //                                tb = new RadTab();
        //                                tb.Target = "tbOrder";
        //                                tb.Text = "Result Files";
        //                                tabView.Tabs.Add(tb);
        //                                if (Request["Type"] == "Results")
        //                                {
        //                                    IList<FileManagementIndex> IndexList = new List<FileManagementIndex>();
        //                                    IndexList = lstfile.Where(f => f.Id == Convert.ToUInt32(TargetValue)).ToList<FileManagementIndex>();
        //                                    if (IndexList.Count > 0)
        //                                    {
        //                                        PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + IndexList[0].File_Path;
        //                                    }
        //                                    else if (lstfile.Count > 0)
        //                                    {
        //                                        PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[lstfile.Count - 1].File_Path;
        //                                    }
        //                                }
        //                                else if (Request.QueryString["Screen"] == "ResultView" || Request.QueryString["Screen"] == "OrderManagement")
        //                                {
        //                                    PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + TargetValue;
        //                                }
        //                                else if (lstfile.Count > 0)
        //                                {
        //                                    PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[lstfile.Count - 1].File_Path;
        //                                }
        //                                else
        //                                {
        //                                    PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[01].File_Path;
        //                                }
        //                                PageViewResult.Selected = true;
        //                                txtFileInformation.Value = lstfile[0].File_Path.Split('/')[5] + "|" + lstfile[0].Document_Type.ToString() + " | " + lstfile[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstfile[0].Created_Date_And_Time).ToString("dd-MMM-yyyy");//BugID:42809
        //                            }
        //                        }

        //                        //PageViewResultFiles.ContentUrl = "frmImageViewer.aspx?Screen=ResultView" + "&OrderSubmitId=" + Request["OrderSubmitId"];
        //                        //PageViewResultFiles.Selected = true;
        //                        break;
        //                    }
        //                    else if (Tab_Name[i].ToLower() == "result master")
        //                    {
        //                        //PageViewResult.ContentUrl = "frmResult.aspx?Result_Master_ID=" + Result_Master_Id + "&strScreenName=PATIENT CHART";
        //                        //PageViewResult.Selected = true;
        //                        //break;
        //                    }
        //                    else if (Tab_Name[i].ToLower() == "abi")
        //                    {
        //                        PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Request["HumanId"].ToString();
        //                        PageViewABIResults.Selected = true;
        //                        break;
        //                    }
        //                    else if (Tab_Name[i].ToLower() == "spirometry")
        //                    {
        //                        PageViewSpirometryResults.ContentUrl = "frmSpirometry.aspx?HumanId=" + Request["HumanId"].ToString();
        //                        PageViewSpirometryResults.Selected = true;
        //                        break;
        //                    }
        //                    else if (Tab_Name[i].ToLower() == "message")
        //                    {
        //                        break;
        //                    }
        //                }

        //            }
        //            catch
        //            {
        //                //do nothing

        //            }
        //        }

        //    }
        //}


        //protected void pbPlus_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (pbPlus.ID == "pbPlus")
        //    {
        //        HtmlTable ctrl = (HtmlTable)Page.FindControl("tblTree");
        //        lsttype = new RadListBox();
        //        lsttype.ID = "lstDynamic";
        //        lsttype.Height = 100;
        //        lsttype.Width = txtDocumentSubType.Width;
        //        lsttype.OnClientSelectedIndexChanged = "SelectedIndexChanged";
        //        ctrl.Rows[3].Cells[0].Controls.Add(lsttype);
        //        pbPlus.ImageUrl = "~/Resources/minus_new.gif";
        //        pbPlus.ID = "pbMinus";
        //        LoadlstNotes();
        //        lsttype.Visible = true;
        //    }
        //    else
        //    {
        //        lsttype.Visible = false;
        //        pbPlus.ImageUrl = "~/Resources/plus_new.gif";
        //        pbPlus.ID = "pbPlus";
        //    }


        //}

        //public void LoadlstNotes()
        //{
        //    lsttype.Items.Clear();

        //    ilstscaninindex = (IList<FileManagementIndex>)Session["IndexList"];

        //    if (cboDocumentType.Text != "Results")
        //    {
        //        IList<string> documentType = ((from doc in ilstscaninindex
        //                                       where doc.Document_Type == cboDocumentType.Text && doc.Source == "SCAN"
        //                                       select doc.Document_Sub_Type).Distinct()).ToList<string>();

        //        if (documentType.Count > 0)
        //        {
        //            for (int i = 0; i < documentType.Count; i++)
        //            {
        //                RadListBoxItem tempItem = new RadListBoxItem();
        //                tempItem.Text = documentType[i];
        //                tempItem.ToolTip = documentType[i];
        //                lsttype.Items.Add(tempItem);
        //            }
        //        }
        //    }
        //}



        //protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        //{


        //    WFObjectManager obj_workFlow = new WFObjectManager();
        //    ulong LabId = 0;
        //    if (Request["LabId"] != null)
        //        LabId = Convert.ToUInt32(Request["LabId"]);
        //    else if (Request["Screen"] != null && Request["Screen"] == "ResultViewOrder")
        //        LabId = 32;

        //    if (LabId == 32)
        //    {
        //        if (btnSave.Enabled == true)
        //        {
        //            SaveNotes();
        //        }

        //        FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
        //        IList<FileManagementIndex> lstfilemanage = new List<FileManagementIndex>();
        //        lstfilemanage = objfileproxy.GetImagesforAnnotations(Convert.ToUInt64(Request["HumanID"]), Convert.ToUInt64(Request["OrderSubmitId"]), "ORDER,ABI,SPIROMETRY,SCAN");
        //        if (lstfilemanage.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115039'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            return;

        //        }

        //        OrdersSubmitManager objordeproxy = new OrdersSubmitManager();
        //        IList<OrdersSubmit> lstordersubmit = new List<OrdersSubmit>();
        //        lstordersubmit = objordeproxy.GetOrdersSubmitListbyID(Convert.ToUInt64(Request["OrderSubmitId"]));

        //        if (lstordersubmit.Count > 0)
        //        {
        //            if (lstordersubmit[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115040'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //                return;
        //            }

        //        }
        //    }

        //    string Current_Process = string.Empty;
        //    int close_type = 0;
        //    string User_name = string.Empty;
        //    string[] current_process = new string[] { Request["CurrentProcess"].Replace(" _", "_").ToString() };

        //    if (Request["CurrentProcess"] != null)
        //        Current_Process = Request["CurrentProcess"].Replace(" _", "_").ToString();

        //    UserManager objPhy = new UserManager();
        //    string sPhysicianName = string.Empty;

        //    IList<User> PhyUserList = objPhy.getUserByPHYID(ClientSession.PhysicianId);

        //    sPhysicianName = PhyUserList[0].user_name;

        //    User_name = PhyUserList[0].user_name;



        //    if (Current_Process == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
        //    {
        //        close_type = 8;

        //    }
        //    else if (Current_Process == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
        //    {
        //        close_type = 1;
        //        User_name = ClientSession.UserName;

        //    }
        //    else if (Current_Process == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
        //    {
        //        close_type = 9;
        //        User_name = "UNKNOWN";
        //    }
        //    else if (Current_Process == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
        //    {
        //        close_type = 1;
        //        User_name = "UNKNOWN";
        //    }
        //    else if (Current_Process == "MA_RESULTS")
        //    {
        //        close_type = 3;
        //        User_name = "UNKNOWN";
        //    }








        //    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
        //    {

        //        if (chkPhyName.Checked == true)
        //        {
        //            if (btnSave.Enabled == true)
        //            {
        //                SaveNotes();
        //            }

        //            IList<string> _Status_Flag = new List<string>();
        //            if (Session["Status_Flag"] != null)
        //            {
        //                _Status_Flag = (IList<string>)Session["Status_Flag"];
        //            }
        //            // string[] current_process = new string[] { "RESULT_REVIEW" };
        //            if (_Status_Flag.Count == 0)
        //            {
        //                //$muthusamy on 23-12-2014 for LabResult Changes
        //                if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
        //                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
        //                else
        //                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
        //                //$muthusamy 
        //            }
        //            else
        //            {
        //                if ((_Status_Flag.Contains("P") && _Status_Flag.Contains("F")) || _Status_Flag.Contains("F"))
        //                {
        //                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
        //                }
        //                else if (_Status_Flag.Contains("P") && !_Status_Flag.Contains("F"))
        //                {
        //                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115034'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            return;
        //        }
        //    }

        //    else
        //    {
        //        //$muthusamy on 23-12-2014 for LabResult Changes
        //        if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 1, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        else
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        //$muthusamy
        //    }
        //    //Added for moving order from MA_REVIEW to RESULT_REVIEW (Bug Id: 29134)
        //    //Begin
        //    if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
        //    {
        //        if (Current_Process == "MA_REVIEW")
        //        {
        //            string sPhyName = string.Empty;
        //            UserManager objPhyMngr = new UserManager();

        //            IList<User> PhyList = objPhyMngr.getUserByPHYID(ClientSession.PhysicianId);

        //            sPhyName = PhyList[0].user_name;
        //            string[] current_proc = new string[] { "ORDER_GENERATE" };
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 8, sPhyName, UtilityManager.ConvertToUniversal(), null, current_proc, null);


        //        }
        //    }
        //    //End
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MovedSuccessfully", "DisplayErrorMessage('050002');", true);
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", "End(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        //protected void btnMoveToMa_Click(object sender, EventArgs e)
        //{
        //    if (btnSave.Enabled == true)
        //    {
        //        SaveNotes();
        //    }
        //    WFObjectManager obj_workFlow = new WFObjectManager();
        //    if (btnMoveToMa.Text == "Move To Provider")
        //    {
        //        PhysicianManager objphymanager = new PhysicianManager();
        //        PhyUserList = objphymanager.GetPhysicianandUser(false, string.Empty);
        //        string sPhysicianName = string.Empty;
        //        string[] current_process = new string[] { "MA_RESULTS" };
        //        for (int i = 0; i < PhyUserList.UserList.Count; i++)
        //        {
        //            if (Convert.ToUInt64(PhyUserList.UserList[i].Physician_Library_ID) == Convert.ToUInt64(Request.QueryString["PhysicianId"]))
        //            {
        //                sPhysicianName = PhyUserList.UserList[i].user_name;
        //                break;
        //            }
        //        }
        //        if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
        //        {
        //            if (sPhysicianName == "")
        //            {
        //                WFObject objWorkflowList = new WFObject();
        //                objWorkflowList = obj_workFlow.GetByObjectSystemId(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT");
        //                if (objWorkflowList != null)
        //                {
        //                    sPhysicianName = objWorkflowList.Process_Allocation.Split('|')[1].Split('-')[1];
        //                }

        //            }
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, sPhysicianName, UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        }
        //        else
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, sPhysicianName, UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        //$muthusamy
        //    }
        //    else
        //    {
        //        if (cboMoveToMA.Text == string.Empty && cboMoveToMA.Visible == true)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115046'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            return;
        //        }
        //        string[] current_process = new string[] { "RESULT_REVIEW" };
        //        IList<Encounter> lstenc = new List<Encounter>();
        //        EncounterManager objencmanager = new EncounterManager();
        //        if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
        //        {
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        }
        //        else if (Convert.ToUInt64(Request["EncounterId"]) != 0)
        //        {
        //            lstenc = objencmanager.GetEncounterByEncounterID(Convert.ToUInt64(Request["EncounterId"]));
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, lstenc.Count > 0 ? lstenc[0].Assigned_Med_Asst_User_Name : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        }
        //        else
        //        {
        //            obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
        //        }
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", "End(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        //public void SaveNotes()
        //{
        //    ResultMasterManager masterProxy = new ResultMasterManager();
        //    IList<ResultMaster> lstResultMaster = new List<ResultMaster>();
        //    ResultMaster objResultMaster = new ResultMaster();
        //    if (Session["BoolNotes"] != null && (bool)Session["BoolNotes"] == false)
        //    {
        //        objResultMaster.PID_External_Patient_ID = Convert.ToString(Request["HumanId"]);
        //        objResultMaster.PID_Alternate_Patient_ID = objResultMaster.PID_External_Patient_ID;
        //        objResultMaster.Result_Review_Comments = DLC.txtDLC.Text;
        //        objResultMaster.MA_Notes = txtMedicalAssistantNotes.Text;
        //        objResultMaster.Created_By = ClientSession.UserName;
        //        objResultMaster.Order_ID = Convert.ToUInt32(Session["Order_Id"]);
        //        objResultMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //        lstResultMaster.Add(objResultMaster);
        //        masterProxy.SaveResultMasterThroughViewIndexedImages(lstResultMaster.ToArray<ResultMaster>(), string.Empty);
        //        objresultmaster = new ResultMaster();
        //        objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt32(Request["HumanId"]), Convert.ToUInt32(Session["Order_Id"]));
        //        Session["BoolNotes"] = true;
        //        Session["Notes"] = objresultmaster;
        //    }
        //    else
        //    {
        //        if (Session["Notes"] != null)
        //        {
        //            objresultmaster = (ResultMaster)Session["Notes"];
        //            objresultmaster.Modified_By = ClientSession.UserName;
        //            objresultmaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
        //            objresultmaster.Result_Review_Comments = DLC.txtDLC.Text;
        //            objresultmaster.MA_Notes = txtMedicalAssistantNotes.Text;
        //            masterProxy.SaveAndUpdateResultReview(objresultmaster, string.Empty);
        //        }

        //    }
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "SaveViewResults();", true);
        //    btnSave.Enabled = false;
        //    hdnSave.Value = "false";
        //}


        //protected void tabView_TabClick(object sender, RadTabStripEventArgs e)
        //{
        //    filelist = (IList<FileManagementIndex>)Session["FileList"];

        //    if (e.Tab.Text == "Scanned Results")
        //    {
        //        ulong order_id = (ulong)Session["Order_Id"];

        //        IList<string> lstScanId = ((from doc in filelist
        //                                    where doc.Order_ID == order_id
        //                                    select doc.File_Path).Distinct()).ToList<string>();

        //        PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Request["HumanId"].ToString() + "&Doc=" + cboDocumentType.Text + "&DDate=" + tvViewIndex.SelectedNode.Text.ToString();
        //        PageViewScan.Selected = true;

        //    }

        //    else if (e.Tab.Text == "Result Files")
        //    {

        //        IList<string> lstScanId = ((from doc in filelist
        //                                    where doc.Order_ID == Convert.ToUInt32(Request["OrderSubmitId"])
        //                                    select doc.File_Path).Distinct()).ToList<string>();
        //        PageViewResultFiles.ContentUrl = "frmImageViewer.aspx?Source=RESULT&FilePath=" + lstScanId[0].ToString();
        //        PageViewResultFiles.Selected = true;

        //    }
        //    else if (e.Tab.Text == "Results")
        //    {
        //        ulong result_id = (ulong)Session["Result_Master_Id"];
        //        PageViewResult.ContentUrl = "frmResult.aspx?Result_Master_ID=" + result_id.ToString() + "&strScreenName=PATIENT CHART";
        //        PageViewResult.Selected = true;

        //    }
        //    else if (e.Tab.Text == "ABI Results")
        //    {
        //        PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Request["HumanId"].ToString();
        //        PageViewABIResults.Selected = true;

        //    }
        //    else if (e.Tab.Text == "Spirometry Results")
        //    {
        //        PageViewSpirometryResults.ContentUrl = "frmSpirometry.aspx?HumanId=" + Request["HumanId"].ToString();
        //        PageViewSpirometryResults.Selected = true;

        //    }
        //    else if (e.Tab.Text == "Scanned Results")
        //    {
        //        tb.Text = "Message Log";

        //    }
        //}


        //protected void btnPrint_Click(object sender, EventArgs e)
        //{
        //    ulong Result_Master_ID;
        //    Result_Master_ID = (ulong)Session["Result_Master_Id"];
        //    PDFGenerator objPDFGenerator = new PDFGenerator();
        //    string filepath = Server.MapPath("Documents/" + Session.SessionID);
        //    hdnSelectedItem.Value = String.Empty;
        //    string filename = objPDFGenerator.GenerateRequestionForLabcorp(Result_Master_ID, filepath);
        //    string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
        //    string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
        //    string file = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
        //    string loc = "DYNAMIC";
        //    if (hdnSelectedItem.Value == string.Empty)
        //    {
        //        hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
        //    }
        //    else
        //    {
        //        hdnSelectedItem.Value += "|" + FileName[0].ToString();
        //    }
        //    RadScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);

        //}

        //protected void btnPrintEducatnMaterial_Click(object sender, EventArgs e)
        //{
        //    ResultMasterManager objResultMasterManager = new ResultMasterManager();
        //    result_master_list = (IList<ResultMaster>)Session["ResultList"];

        //    ulong Result_Master_ID;
        //    Result_Master_ID = result_master_list[0].Id;
        //    FillResultDTO objFillResultDTO = new FillResultDTO();
        //    Stream strm = null;
        //    var serializer = new NetDataContractSerializer();
        //    strm = objResultMasterManager.GetResultByResultMasterID(Result_Master_ID);
        //    object ol = (object)serializer.ReadObject(strm);
        //    objFillResultDTO = ol as FillResultDTO;
        //    string urls = "";
        //    string value = string.Empty;
        //    for (int i = 0; i < objFillResultDTO.ResultOBXList.Count; i++)
        //    {
        //        value = objFillResultDTO.ResultOBXList[i].OBX_Loinc_Identifier;
        //        //if (value != "")
        //        //{
        //        //    string pageurl = "http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=" + value + "&informationRecipient.languageCode.c = en";
        //        //    // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "window.open('" + pageurl + "','_blank');", true);
        //        //    urls += pageurl + ";";

        //        //}          
        //        if (value != "")
        //        {
        //            urls += value + ";";
        //        }

        //    }
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "openLink('" + urls + "');", true);
        //}

        //protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    XDocument xmlUser = XDocument.Load(Server.MapPath(@"ConfigXML\User.xml"));
        //    cboMoveToMA.Items.Clear();
        //    cboMoveToMA.Items.Add(new RadComboBoxItem(""));
        //    if (chkShowAll.Checked)
        //    {
        //        foreach (XElement elements in xmlUser.Descendants("UserList"))
        //        {
        //            foreach (XElement UserElement in elements.Elements())
        //            {
        //                if (UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
        //                {
        //                    string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
        //                    cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (XElement elements in xmlUser.Descendants("UserList"))
        //        {
        //            foreach (XElement UserElement in elements.Elements())
        //            {
        //                if (UserElement.Attribute("Default_Facility").Value.ToUpper() == ClientSession.FacilityName.ToUpper() && UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
        //                {
        //                    string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
        //                    cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
        //                }
        //            }
        //        }
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}
        #endregion
        //BugID:43099 --Revamp--
        #region  Server.MapPath


        public RadTab tb = null;
        ResultMaster objresultmaster = null;
        RadListBox lsttype = new RadListBox();
        FillPhysicianUser PhyUserList;

        [Serializable]
        public class Result_Files_List
        {
            public DateTime _Res_Date = DateTime.MinValue;
            public ulong _Ord_Id = 0;
            public string _Doc_Type = string.Empty;
            public string _Sub_Doc_Type = string.Empty;
            public string _Img_path = string.Empty;
            public ulong _Res_Id = 0;
            public string _Order_Description = string.Empty;
            public ulong _IndexId = 0;

            public string Doc_Type
            {
                get { return _Doc_Type; }
                set { _Doc_Type = value; }
            }
            public string Sub_Doc_Type
            {
                get { return _Sub_Doc_Type; }
                set { _Sub_Doc_Type = value; }
            }
            public ulong Order_Submit_Id
            {
                get { return _Ord_Id; }
                set { _Ord_Id = value; }
            }
            public DateTime Result_Date
            {
                get { return _Res_Date; }
                set { _Res_Date = value; }
            }
            public string Ord_Type
            {
                get { return _Doc_Type; }
                set { _Doc_Type = value; }
            }
            public ulong Result_Id
            {
                get { return _Res_Id; }
                set { _Res_Id = value; }
            }
            public string Img_Path
            {
                get { return _Img_path; }
                set { _Img_path = value; }
            }

            public string Order_Description
            {
                get { return _Order_Description; }
                set { _Order_Description = value; }
            }
            public ulong IndexId
            {
                get { return _IndexId; }
                set { _IndexId = value; }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ulong human_id = 0;
            string Document_type = string.Empty;
            ulong Key_id = 0;

            if (!IsPostBack)
            {

                if (Request["Openingfrom"] != null && Request["Openingfrom"] == "MyorderQueue")
                {
                    btnpatientChart1.Visible = true;
                    btnFindAppointments.Visible = true;
                    btnePrescribe.Visible = true;
                }

                DLC.DName = "pbDropdown";
                DLC.txtDLC.Attributes.Add("onkeypress", "txtProviderNotes_OnKeyPress();");
                DLC.txtDLC.Attributes.Add("onchange", "txtProviderNotes_OnKeyPress();");
                btnSave.Attributes.Add("onclick", "btnSave_ClientClick();");
                Session["Status_Flag"] = null;
                if (Request["HumanID"] != null)
                {
                    human_id = Convert.ToUInt64(Request["HumanID"].ToString());
                }
                Session["human_id"] = human_id;

                #region Populate Patient Info

                #region  Commented By Deepak
                //string FileName = "Human" + "_" + human_id + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                // if (File.Exists(strXmlFilePath) == true)
                // {

                //XmlDocument itemDoc = new XmlDocument();
                //XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);

                // using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                // {
                //itemDoc.Load(fs);

                //XmlNodeList xmlhumanList = itemDoc.GetElementsByTagName("Human");
                //Human objFillHuman = new Human();
                //IList<Human> lstHuman = new List<Human>();
                //if (xmlhumanList != null && xmlhumanList.Count > 0)
                //{
                //    objFillHuman.Id = Convert.ToUInt64(xmlhumanList[0].Attributes.GetNamedItem("Id").Value);
                //    objFillHuman.Birth_Date = Convert.ToDateTime(xmlhumanList[0].Attributes.GetNamedItem("Birth_Date").Value);
                //    objFillHuman.First_Name = xmlhumanList[0].Attributes.GetNamedItem("First_Name").Value;
                //    objFillHuman.Last_Name = xmlhumanList[0].Attributes.GetNamedItem("Last_Name").Value;
                //    objFillHuman.MI = xmlhumanList[0].Attributes.GetNamedItem("MI").Value;
                //    objFillHuman.Sex = xmlhumanList[0].Attributes.GetNamedItem("Sex").Value;
                //    objFillHuman.Suffix = xmlhumanList[0].Attributes.GetNamedItem("Suffix").Value;
                //    objFillHuman.Medical_Record_Number = xmlhumanList[0].Attributes.GetNamedItem("Medical_Record_Number").Value;
                //    objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Home_Phone_No").Value;
                //    objFillHuman.Human_Type = xmlhumanList[0].Attributes.GetNamedItem("Human_Type").Value;
                //    objFillHuman.Patient_Account_External = xmlhumanList[0].Attributes.GetNamedItem("Patient_Account_External").Value;
                //    //objFillHuman.Home_Phone_No = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;
                //    objFillHuman.Cell_Phone_Number = xmlhumanList[0].Attributes.GetNamedItem("Cell_Phone_Number").Value;

                //    lstHuman.Add(objFillHuman);
                //}
                #endregion
                IList<string> ilstHumanTag = new List<string>();
                ilstHumanTag.Add("HumanList");

                IList<object> ilstHumanBlobList = new List<object>();
                ilstHumanBlobList = UtilityManager.ReadBlob(human_id, ilstHumanTag);
                IList<Human> lstHuman = new List<Human>();
                Human objFillHuman = new Human();

                if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
                {
                    if (ilstHumanBlobList[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                        {
                            objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                            lstHuman.Add(objFillHuman);
                        }
                    }
                }

                string phoneno = "";
                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            if(objFillHuman.Home_Phone_No.Length == 14)
                            {
                                phoneno = objFillHuman.Home_Phone_No;
                            }
                            else
                            {
                                phoneno = objFillHuman.Cell_Phone_Number;
                            }

                        }

                        if (lstHuman != null && lstHuman.Count > 0)
                        {
                            txtPatientInformation.Value = objFillHuman.Last_Name + "," + objFillHuman.First_Name +
                       " " + objFillHuman.MI + " " + objFillHuman.Suffix + " | " +
                        objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
                       (CalculateAge(objFillHuman.Birth_Date)).ToString() +
                       " year(s) | " + objFillHuman.Sex.Substring(0, 1) + " | Acc #:" + objFillHuman.Id +
                       " | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
                       "Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type;
                        }
                   // }
                //}
                
                #endregion

                #region Opening from Patient_Pane
                if (Request["Opening_from"] != null && Request["Opening_from"] == "Patient_Pane") //Opening from Left side Patient Pane
                {
                    if (Request["Doc_type"] != null)
                    {
                        Document_type = Request["Doc_type"].ToString();
                    }
                    if (Request["Key_id"] != null)
                    {
                        Key_id = Convert.ToUInt64(Request["Key_id"].ToString());
                    }
                    Session["Doc_type"] = Document_type;
                    Session["Key_id"] = Key_id;

                    btnPrintEducatnMaterial.Visible = true;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    cboMoveToMA.Visible = false;
                    Label1.Visible = false;
                    btnMoveToNextProcess.Visible = false;
                    btnSave.Visible = false;
                    chkPhyName.Visible = false;
                    DLC.txtDLC.Enabled = false;
                    txtMedicalAssistantNotes.Enabled = false;
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    trPatInfo.Visible = false;//Added for BugID:45808
                    btnClose.Visible = false;
                    ConstructTreeView(human_id, Document_type, Key_id);
                }
                #endregion
                #region Opening from OrdersQ
                else if (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersQ") //Opening from My Orders Q
                {
                    Document_type = "Results";

                    if (Request["ResultMasterID"] != null)
                    {
                        Key_id = Convert.ToUInt64(Request["ResultMasterID"].ToString());
                    }
                    else if (Request["File_Ref_ID"] != null && Request["File_Ref_ID"] != "")
                    {
                        Key_id = Convert.ToUInt64(Request["File_Ref_ID"].ToString());
                    }
                    Session["Doc_type"] = Document_type;
                    Session["Key_id"] = Key_id;

                    if ((ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant") && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "RESULT_REVIEW")
                    {
                        string sTemp = string.Empty;
                        string[] sPhyname = null;
                        StaticLookupManager objstaticlookup = new StaticLookupManager();
                        IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                        IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                        PhysicianManager objphysican = new PhysicianManager();
                        if (Request["PhysicianId"] != null && Convert.ToUInt32(Request["PhysicianId"]) != 0)
                        {
                            PhysicianList = objphysican.GetphysiciannameByPhyID(Convert.ToUInt32(Request["PhysicianId"]));
                            if (PhysicianList != null && PhysicianList.Count > 0)
                            {
                                if (StaticLst != null && StaticLst.Count > 0)
                                {
                                    sPhyname = StaticLst[0].Value.Split('|');
                                    if (sPhyname != null && sPhyname.Length > 1 && sPhyname[1].Trim() != string.Empty)
                                    {
                                        sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                                    }
                                }

                            }
                            sTemp = sTemp.Replace("<Date>", "");
                            sTemp = sTemp.Replace(" on ", "");
                            chkPhyName.Text = sTemp;
                        }
                        txtMedicalAssistantNotes.Enabled = false;
                        btnSave.Enabled = false;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");

                        if (chkShowAll.Visible == true)
                        {
                            //CAP-2953
                            //XDocument xmlUser = null;
                            //if (File.Exists(Server.MapPath(@"ConfigXML\User.xml")))
                            //    xmlUser = XDocument.Load(Server.MapPath(@"ConfigXML\User.xml"));
                            //cboMoveToMA.Items.Clear();
                            //cboMoveToMA.Items.Add(new RadComboBoxItem(""));
                            //if (xmlUser != null)
                            //{
                            //    if (chkShowAll.Checked)
                            //    {
                            //        foreach (XElement elements in xmlUser.Descendants("UserList"))
                            //        {
                            //            foreach (XElement UserElement in elements.Elements())
                            //            {
                            //                if (UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
                            //                {
                            //                    string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
                            //                    cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                            //                }
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        foreach (XElement elements in xmlUser.Descendants("UserList"))
                            //        {
                            //            foreach (XElement UserElement in elements.Elements())
                            //            {
                            //                if (UserElement.Attribute("Default_Facility").Value.ToUpper() == ClientSession.FacilityName.ToUpper() && UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
                            //                {
                            //                    string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
                            //                    cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                            //                }
                            //            }
                            //        }
                            //    }
                            //}

                            UserList objUser = new UserList();
                            var ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
                            if (ilstUserList?.User != null)
                            {
                                objUser.User = ilstUserList.User;
                                cboMoveToMA.Items.Clear();
                                cboMoveToMA.Items.Add(new RadComboBoxItem(""));
                                if (chkShowAll.Checked)
                                {
                                    if (objUser.User != null && objUser.User.Count() > 0)
                                    {
                                        var vuser = objUser.User.Where(a => a.Role.ToString().ToUpper() == "MEDICAL ASSISTANT").ToList();
                                        if (vuser != null && vuser.Count() > 0)
                                        {
                                            for (int i = 0; i < vuser.Count(); i++)
                                            {
                                                string xmlValue = vuser[i].User_Name + "-" + vuser[i].person_name;
                                                cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (objUser.User != null && objUser.User.Count() > 0)
                                    {
                                        var vdefaultuser = objUser.User.Where(a => a.Default_Facility == ClientSession.FacilityName.ToUpper() && a.Role.ToString().ToUpper() == "MEDICAL ASSISTANT").ToList();
                                        if (vdefaultuser != null && vdefaultuser.Count() > 0)
                                        {
                                            for (int i = 0; i < vdefaultuser.Count(); i++)
                                            {
                                                string xmlValue = vdefaultuser[i].User_Name + "-" + vdefaultuser[i].person_name;
                                                cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                                            }
                                        }
                                    }
                                }
                            }
                            btnPrintEducatnMaterial.Visible = true;

                        }
                    }
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT" && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "MA_RESULTS")
                    {

                        btnMoveToMa.Text = "Save & Move To Provider";
                        chkShowAll.Visible = false;
                        cboMoveToMA.Visible = false;
                        Label1.Visible = false;
                        btnSave.Enabled = false;
                        chkPhyName.Visible = false;
                        DLC.txtDLC.Enabled = false;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    }

                    ConstructTreeView(human_id, Document_type, Key_id);
                    ResultMasterManager masterProxy = new ResultMasterManager();
                    if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")//to handle Diagnostic_results which do not have OrderID.//BugID:45807
                    {
                        PageViewResult.Height = 480;
                        tvViewIndex.Height = 432;
                    }
                    else
                    {
                        tvViewIndex.Height = 506;
                        PageViewResult.Height = 555;
                    }

                }
                #endregion
                #region Opening from Order Management Screen(CPOE-Order)
                if (Request["Opening_from"] != null && Request["Opening_from"] == "OrderManagementScreen") //Opening from Order Management Screen (Menu Level)
                {
                    Document_type = "Results";
                    if (Request["File_Ref_ID"] != null)
                        Key_id = Convert.ToUInt64(Request["File_Ref_ID"].ToString());

                    Session["Key_id"] = Key_id;
                    Session["Doc_type"] = Document_type;

                    string sTemp = string.Empty;
                    string[] sPhyname = null;
                    StaticLookupManager objstaticlookup = new StaticLookupManager();
                    IList<StaticLookup> StaticLst = objstaticlookup.getStaticLookupByFieldName("WELLNESS NOTE FOR PROVIDER SIGN WITH CHANGES");
                    IList<PhysicianLibrary> PhysicianList = new List<PhysicianLibrary>();
                    PhysicianManager objphysican = new PhysicianManager();
                    if (Request["PhysicianId"] != null && Convert.ToUInt32(Request["PhysicianId"]) != 0)
                    {
                        PhysicianList = objphysican.GetphysiciannameByPhyID(Convert.ToUInt32(Request["PhysicianId"]));
                        if (PhysicianList != null && PhysicianList.Count > 0)
                        {
                            if (StaticLst != null && StaticLst.Count > 0)
                            {
                                sPhyname = StaticLst[0].Value.Split('|');
                                if (sPhyname != null && sPhyname.Length > 1 && sPhyname[1].Trim() != string.Empty)
                                {
                                    sTemp = sPhyname[1].Replace("<Physician>", PhysicianList[0].PhyPrefix + " " + PhysicianList[0].PhyFirstName + " " + PhysicianList[0].PhyMiddleName + " " + PhysicianList[0].PhyLastName + " " + PhysicianList[0].PhySuffix);
                                }
                            }

                        }
                        sTemp = sTemp.Replace("<Date>", "");
                        sTemp = sTemp.Replace(" on ", "");
                        chkPhyName.Text = sTemp;
                    }

                    ConstructTreeView(human_id, Document_type, Key_id);
                    btnPrintEducatnMaterial.Visible = true;
                    DLC.txtDLC.Enabled = false;
                    txtMedicalAssistantNotes.Enabled = false;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    cboMoveToMA.Visible = false;
                    Label1.Visible = false;
                    btnMoveToNextProcess.Visible = false;
                    btnSave.Visible = false;
                    chkPhyName.Visible = false;
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                }
                #endregion
                #region Opening from OrdersList
                if (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersList") //Opening from Diagnostic Order - Orders List
                {
                    Document_type = "Results";
                    Session["Doc_type"] = Document_type;
                    Key_id = 0;
                    Session["Key_id"] = Key_id;

                    DLC.txtDLC.Enabled = false;
                    txtMedicalAssistantNotes.Enabled = false;
                    btnMoveToMa.Visible = false;
                    chkShowAll.Visible = false;
                    cboMoveToMA.Visible = false;
                    Label1.Visible = false;
                    btnMoveToNextProcess.Visible = false;
                    btnSave.Visible = false;
                    chkPhyName.Visible = false;
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    ConstructTreeView(human_id, Document_type, Key_id);
                }
                #endregion

                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLog activity = new ActivityLog();

                activity.Human_ID = Convert.ToUInt64(Session["human_id"].ToString());
                activity.Encounter_ID = 0;
                activity.Activity_Type = "Patient is accessing the document - Document Type - " + Document_type + " and Document ID - " + Key_id.ToString();
                activity.Activity_By = ClientSession.UserName;
                activity.Activity_Date_And_Time = DateTime.Now;
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            }
            else
            {
                if (hdnSave.Value == "true")
                {
                    btnSave.Enabled = true;
                }
            }
        }
        private string ConstructTreeView(ulong humanId, string Doc_type, ulong KeyID)
        {
            loadlistforResult();
            IList<Result_Files_List> result_files = new List<Result_Files_List>();
            LoadTreeViewData(out result_files);
            LoadTreeView(result_files);
            Session["Scan_Index"] = null;
            Session["Scan_Index"] = "INDEX_SCREEN";
            if (Session["Key_id"] != null)
                HighlightSelectedItem(Convert.ToUInt64(Session["Key_id"]));
            return string.Empty;
        }

        public int CalculateAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;
            return years;
        }

        public void loadlistforResult()
        {
            FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
            FileManagementDTO objdto = new FileManagementDTO();
            //objdto = objfileproxy.GetResultandExamListusing(Convert.ToUInt64(Session["human_id"]));
            objdto = objfileproxy.GetResultandExamListusing(Convert.ToUInt64(Session["human_id"]), Convert.ToUInt32(Request["OrderSubmitId"]));
            IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
            IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            filelist = objdto.FileManagementList;
            result_master_list = objdto.ResultMasterList;
            Session["fileList"] = filelist;
            Session["result_master_list"] = result_master_list;

            if (result_master_list != null && result_master_list.Count > 0)
            {
                ulong order_submit_id = 0;
                for (int i = 0; i < result_master_list.Count; i++)
                {
                    if (result_master_list[i].Order_ID != 0)
                    {
                        if (result_master_list[i].Order_ID != order_submit_id)
                        {
                            ilstresultmaster.Add(result_master_list[i]);
                            order_submit_id = result_master_list[i].Order_ID;
                        }
                    }
                    else
                    {
                        ilstresultmaster.Add(result_master_list[i]);
                        order_submit_id = result_master_list[i].Order_ID;
                    }

                }
            }


            if (ilstresultmaster.Count > 0 && filelist != null && filelist.Count > 0)
            {
                ilstscaninindex = new List<FileManagementIndex>();
                for (int i = 0; i < filelist.Count; i++)
                {
                    bool blScan = false;
                    for (int j = 0; j < ilstresultmaster.Count; j++)
                    {
                        if (ilstresultmaster[j].Order_ID != 0)
                        {
                            if (filelist[i].Order_ID == ilstresultmaster[j].Order_ID)
                            {
                                ilstresultmaster[j].Is_Scan_order = "Y";
                                blScan = true;
                                break;
                            }
                        }
                    }
                    //if (blScan == false)//BugID:49161
                    //{
                    if (filelist[i].Order_ID == 0 && filelist[i].Document_Sub_Type != "Lab Result Form")
                    {
                        ilstscaninindex.Add(filelist[i]);
                    }
                    else if (filelist[i].Order_ID != 0)
                    {
                        ilstscaninindex.Add(filelist[i]);
                    }

                    //}
                }
            }

            else if ((ilstresultmaster.Count == 0 && filelist != null && filelist.Count > 0))
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    ilstscaninindex.Add(filelist[i]);
                }
            }

            if (ilstresultmaster != null && ilstresultmaster.Count > 0)
            {
                ilstresultmaster = ilstresultmaster.OrderByDescending(a => a.Created_Date_And_Time).ToList<ResultMaster>();
            }
            if (ilstscaninindex != null && ilstscaninindex.Count > 0)
            {
                ilstscaninindex = ilstscaninindex.OrderByDescending(a => a.Order_ID).ToList<FileManagementIndex>();
            }
            Session["ilstscanindex"] = ilstscaninindex;
            Session["ilstresultmaster"] = ilstresultmaster;
        }
        private IList<Result_Files_List> LoadTreeViewData(out IList<Result_Files_List> ResultLists)
        {
            IList<FileManagementIndex> ilstscaninindex = new List<FileManagementIndex>();
            IList<ResultMaster> ilstresultmaster = new List<ResultMaster>();
            ilstscaninindex = (IList<FileManagementIndex>)Session["ilstscanindex"];
            ilstresultmaster = (IList<ResultMaster>)Session["ilstresultmaster"];
            ResultLists = new List<Result_Files_List>();

            IList<string> DocTypes = new List<string>();
            if (ilstscaninindex != null)
            {
                for (int i = 0; i < ilstscaninindex.Count; i++)
                {
                    Result_Files_List objresult = new Result_Files_List();
                    if (ilstscaninindex[i].Order_ID != 0)
                    {
                        OrdersSubmit ordersList = new OrdersSubmit();
                        OrdersSubmitManager OsManager = new OrdersSubmitManager();
                        ordersList = OsManager.GetById(ilstscaninindex[i].Order_ID);
                        if (ordersList != null && ordersList.Id != 0)
                        {
                            if (ordersList.Bill_Type.Trim() != string.Empty) //BugID:42001 ,41884
                            {
                                objresult.Result_Date = ordersList.Created_Date_And_Time;//Results entered from diagnostic_order screen
                            }
                            else
                            {
                                objresult.Result_Date = ilstscaninindex[i].Document_Date;//Results uploaded from upload scan documents screen
                            }
                        }
                        else
                        {
                            objresult.Result_Date = ilstscaninindex[i].Document_Date;
                        }
                    }
                    else
                    {
                        objresult.Result_Date = ilstscaninindex[i].Document_Date;
                    }
                    objresult.Result_Id = 0;
                    objresult.Img_Path = ilstscaninindex[i].File_Path;
                    objresult.Doc_Type = ilstscaninindex[i].Document_Type;
                    objresult.Sub_Doc_Type = ilstscaninindex[i].Document_Sub_Type;
                    objresult.Order_Submit_Id = ilstscaninindex[i].Order_ID;
                    objresult.Ord_Type = ilstscaninindex[i].Document_Type;
                    objresult.Order_Description = ilstscaninindex[i].Orders_Description;
                    objresult.IndexId = ilstscaninindex[i].Id;
                    if (Session["Doc_type"] != null && (ilstscaninindex[i].Document_Type.ToUpper().Trim() == Session["Doc_type"].ToString().ToUpper().Trim()))
                    {
                        ResultLists.Add(objresult);
                    }
                    if ((DocTypes.IndexOf(ilstscaninindex[i].Document_Type) == -1) && (ilstscaninindex[i].Document_Type.Trim() != string.Empty))
                    {
                        DocTypes.Add(ilstscaninindex[i].Document_Type);
                    }
                }
            }


            if (Session["Doc_type"].ToString().ToUpper().Trim() == "RESULTS")
            {
                if (ilstresultmaster != null)
                {
                    for (int i = 0; i < ilstresultmaster.Count; i++)
                    {
                        if (ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time != string.Empty)
                        {
                            Result_Files_List objresult = new Result_Files_List();
                            string formatString = "yyyyMMddHHmmss";
                            string sample = ilstresultmaster[i].OBR_Specimen_Collected_Date_And_Time;
                            if (sample.Contains('-'))
                            {
                                sample = sample.Split('-')[0].ToString();
                            }
                            if (sample.Length == 14)
                                formatString = "yyyyMMddHHmmss";
                            else if (sample.Length == 12)
                                formatString = "yyyyMMddHHmm";

                            DateTime dt = DateTime.ParseExact(sample, formatString, null);

                            objresult.Result_Date = dt;
                            objresult.Result_Id = ilstresultmaster[i].Id;
                            objresult.IndexId = 0;
                            objresult.Doc_Type = "Results";
                            string sub_doc = "Laboratory";
                            if (ilstresultmaster[i].Orders_Description.Split('-')[0].Trim().ToUpper() == "Patient Discharge Summary".Trim().ToUpper())
                                sub_doc = "Patient Discharge Summary";
                            else if (ilstresultmaster[i].Orders_Description.Split('-')[0].Trim().ToUpper() == "Patient Clinical Summary".Trim().ToUpper())
                                sub_doc = "Patient Clinical Summary";
                            else if (ilstresultmaster[i].Orders_Description.Split('-')[0].Trim().ToUpper() == "Pat Edu".Trim().ToUpper())
                                sub_doc = "Patient Education";
                            objresult.Sub_Doc_Type = sub_doc;
                            objresult.Img_Path = string.Empty;
                            objresult.Order_Submit_Id = ilstresultmaster[i].Order_ID;
                            objresult.Ord_Type = ilstresultmaster[i].Order_Type;
                            objresult.Order_Description = ilstresultmaster[i].Orders_Description;
                            ResultLists.Add(objresult);
                            if (DocTypes.IndexOf("Results") == -1)
                            {
                                DocTypes.Add("Results");
                            }
                        }
                    }
                }
            }
            if (DocTypes != null)
            {
                for (int i = 0; i < DocTypes.Count; i++)
                {
                    RadComboBoxItem tempItem = new RadComboBoxItem();
                    tempItem.Text = DocTypes[i];
                    tempItem.ToolTip = DocTypes[i];
                    if (cboDocumentType.Items.FindItemByText(DocTypes[i]) == null)
                        cboDocumentType.Items.Add(tempItem);
                }
            }

            if (Session["Doc_type"] != null && cboDocumentType.Items.Count > 0 && Session["Doc_type"].ToString() != "")
            {
                if (cboDocumentType.FindItemByText((string)Session["Doc_type"].ToString()) != null)//BugID:48326
                    cboDocumentType.FindItemByText((string)Session["Doc_type"].ToString()).Selected = true;
            }
            return ResultLists;
        }
        private void LoadTreeView(IList<Result_Files_List> Res_files)
        {
            tvViewIndex.Nodes.Clear();
            RadTreeNode tnMainNode = new RadTreeNode();
            tnMainNode.Text = Session["Doc_type"].ToString();
            tvViewIndex.Nodes.Add(tnMainNode);
            IList<string> SubNodelst = new List<string>();
            Res_files = Res_files.OrderByDescending(a => a._Res_Date).ToList<Result_Files_List>();
            if (Session["Doc_type"] != null && Session["Doc_type"].ToString().ToUpper().Trim() == "RESULTS")
            {
                SubNodelst = (from obj in Res_files select obj.Sub_Doc_Type.ToString()).Distinct().ToList<string>();
                foreach (string s in SubNodelst)
                {
                    RadTreeNode tnSubNode = new RadTreeNode();
                    tnSubNode.Text = s;
                    tnMainNode.Nodes.Add(tnSubNode);
                    foreach (Result_Files_List rs in Res_files.Where(a => a.Sub_Doc_Type == s))
                    {
                        RadTreeNode tnChildNode = new RadTreeNode();
                        if (rs.Sub_Doc_Type == "Laboratory")
                        {
                            tnChildNode.Text = rs.Result_Date.ToString("dd-MMM-yyyy hh:mm tt");
                        }
                        else
                        {
                            tnChildNode.Text = UtilityManager.ConvertToLocal(rs.Result_Date).ToString("dd-MMM-yyyy hh:mm tt");
                        }
                        if (rs.Img_Path == string.Empty)
                            tnChildNode.Target = rs.Order_Submit_Id.ToString();
                        else
                            tnChildNode.Target = rs.Img_Path;
                        if (rs.IndexId == 0)
                            tnChildNode.Value = rs.Result_Id.ToString();
                        else
                            tnChildNode.Value = rs.IndexId.ToString();
                        tnChildNode.Attributes.Add("OrderSubmitID", rs.Order_Submit_Id.ToString());
                        tnSubNode.Nodes.Add(tnChildNode);
                    }
                }
            }
            else
            {
                SubNodelst = (from obj in Res_files select obj.Result_Date.ToString("dd-MMM-yyyy").ToString()).Distinct().ToList<string>();
                foreach (string s in SubNodelst)
                {
                    RadTreeNode tnSubNode = new RadTreeNode();
                    tnSubNode.Text = s;
                    tnMainNode.Nodes.Add(tnSubNode);
                    foreach (Result_Files_List rs in Res_files.Where(a => a.Result_Date.ToString("dd-MMM-yyyy") == s))
                    {
                        string File_Name = rs.Img_Path.Substring(rs.Img_Path.LastIndexOf("/") + 1);
                        string file = File_Name.Substring(File_Name.LastIndexOf("_") + 1);
                        string[] temp_name = file.Split('.');
                        string filename = rs.Sub_Doc_Type + "_" + temp_name[0];

                        RadTreeNode tnChildNode = new RadTreeNode();
                        tnChildNode.Text = filename;
                        tnChildNode.Target = rs.Img_Path;
                        tnChildNode.Value = rs.IndexId.ToString();
                        tnChildNode.Attributes.Add("OrderSubmitID", rs.Order_Submit_Id.ToString());
                        tnSubNode.Nodes.Add(tnChildNode);
                    }
                }
            }

            tvViewIndex.ExpandAllNodes();

        }
        private void HighlightSelectedItem(ulong keyID)
        {
            string Matching_ItemID = string.Empty;
            bool node_found = false;
            if (!IsPostBack && Request["Opening_from"] != null && ((Request["Opening_from"] == "OrdersQ" && Request["ObjType"] != "DIAGNOSTIC_RESULT" && keyID == 0) || (Request["Opening_from"] == "OrderManagementScreen")))
            {
                if (Request["OrderSubmitId"] != null)
                    Matching_ItemID = Request["OrderSubmitId"];
                Session["OpenedItemID"] = Matching_ItemID;// BugID:45807
                for (int i = 0; i < tvViewIndex.Nodes[0].Nodes.Count; i++)
                {
                    for (int j = 0; j < tvViewIndex.Nodes[0].Nodes[i].Nodes.Count; j++)
                    {
                        foreach (string key in tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes.Keys)
                        {
                            if (tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes[key] == Matching_ItemID)
                            {
                                tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Selected = true;
                                TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["OrderSubmitID"]);//added OrderSubmitID in list for BugID:45708
                                node_found = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Matching_ItemID = keyID.ToString();
                Session["OpenedItemID"] = Matching_ItemID; // BugID:45807
                for (int i = 0; i < tvViewIndex.Nodes[0].Nodes.Count; i++)
                {
                    for (int j = 0; j < tvViewIndex.Nodes[0].Nodes[i].Nodes.Count; j++)
                    {
                        if (tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value == Matching_ItemID)
                        {
                            tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Selected = true;
                            TabLoad(tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Value, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Target, tvViewIndex.Nodes[0].Nodes[i].Nodes[j].Attributes["OrderSubmitID"]);//added OrderSubmitID in list for BugID:45708
                            node_found = true;
                            break;
                        }
                    }
                }
            }
            if (!node_found)
            {
                tabView.Tabs.Clear();
                PageViewScan.ContentUrl = string.Empty;
                PageViewResult.ContentUrl = string.Empty;
                PageViewResultFiles.ContentUrl = string.Empty;
                PageViewSpirometryResults.ContentUrl = string.Empty;
                PageViewABIResults.ContentUrl = string.Empty;
                PageViewMessageLog.ContentUrl = string.Empty;
                txtFileInformation.Value = "";
                //BugID:46221
                DLC.txtDLC.Enabled = false;
                DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtMedicalAssistantNotes.Enabled = false;
                txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtProvNoteshistory.Enabled = false;
                txtMedNoteshistory.Enabled = false;
                btnSave.Enabled = false;
                btnMoveToMa.Enabled = false;
                btnMoveToNextProcess.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "wrong patient order mapping", @"<script type='text/javascript'>alert('Result is matched to the wrong patient.Please contact support.');</script>");//BugID:48326
            }

        }

        public void TabLoad(string KeyValue, string TargetValue, string OrderSubID)//added OrderSubmitID in list for BugID:45708
        {
            Session["Order_Id"] = 0;
            Session["Notes"] = null;//BugID:46316- by default its given a value null.
            ulong order_id = 0;
            string sfileExtension = (Path.GetExtension(TargetValue.ToString())).ToString();
            hdnpath.Value = TargetValue + "~";
            hdnfileindexid.Value = KeyValue;
            txtMedicalAssistantNotes.Text = string.Empty;
            DLC.txtDLC.Text = string.Empty;
            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            filelist = (IList<FileManagementIndex>)Session["filelist"];


            result_master_list = (IList<ResultMaster>)Session["result_master_list"];

            //Added for Enabling controls only for that particular item opened from Q.Start BugID:45807
            if (Request["Opening_from"] != null && Request["Opening_from"] == "OrdersQ")
            {
                if (Session["OpenedItemID"] != null && (Session["OpenedItemID"].ToString() == KeyValue || Session["OpenedItemID"].ToString() == OrderSubID))
                {
                    if ((ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" || ClientSession.UserRole == "Physician Assistant") && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "RESULT_REVIEW")
                    {
                        txtMedicalAssistantNotes.Enabled = false;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.Enabled = true;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                    }
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT" && Request["CurrentProcess"] != null && Request["CurrentProcess"].ToUpper().Trim() == "MA_RESULTS")
                    {
                        DLC.txtDLC.Enabled = false;
                        DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                        DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                        txtMedicalAssistantNotes.Enabled = true;
                        txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("White");
                    }
                    chkPhyName.Enabled = true;
                    chkShowAll.Enabled = true;
                    cboMoveToMA.Enabled = true;
                    btnMoveToMa.Enabled = true;
                    btnMoveToNextProcess.Enabled = true;
                }
                else
                {
                    chkPhyName.Enabled = false;
                    chkShowAll.Enabled = false;
                    cboMoveToMA.Enabled = false;
                    btnMoveToMa.Enabled = false;
                    btnMoveToNextProcess.Enabled = false;
                    DLC.txtDLC.Enabled = false;
                    DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.Enabled = false;
                    txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                    txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                    txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                }
            }
            //End BugID:45807
            //Srividhya added the file types other than tif on 2-jan-2015-bug id:29130
            if (sfileExtension.ToUpper() == ".TIF" || sfileExtension.ToUpper() == ".PDF" || sfileExtension.ToUpper() == ".PNG" || sfileExtension.ToUpper() == ".GIF" || sfileExtension.ToUpper() == ".BMP" || sfileExtension.ToUpper() == ".JPG" || sfileExtension.ToUpper() == ".JPEG" || sfileExtension.ToUpper() == ".JPG" || sfileExtension.ToUpper() == ".DCM")
            {
                //BEGIN:---To change the title whenever a subnode is clicked BugID:43099
                UInt64 file_id = 0;
                if (UInt64.TryParse(KeyValue, out file_id))
                {
                    IList<FileManagementIndex> lstScanFiles = (from doc in filelist where doc.Id == file_id select doc).ToList<FileManagementIndex>();
                    if (lstScanFiles != null && lstScanFiles.Count > 0)
                    {
                        if (lstScanFiles[0].Result_Master_ID != 0)
                        {
                            hdnFaxpath.Value = lstScanFiles[0].Order_ID.ToString() + " $ " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy h:mm:ss tt") + "$" + lstScanFiles[0].Result_Master_ID;
                        }
                        else
                        {
                            hdnFaxpath.Value = lstScanFiles[0].Order_ID.ToString() + " $ " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy h:mm:ss tt") + " $ " + lstScanFiles[0].Id;
                        }
                        //hdnFaxpath.Value = lstScanFiles[0].Order_ID.ToString() + " $ " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy h:mm:ss tt");
                        hdnpath.Value = hdnpath.Value + lstScanFiles[0].Generate_Link_File_Path.ToString();
                        //modified for integrum
                        txtFileInformation.Value = lstScanFiles[0].File_Path.Substring(lstScanFiles[0].File_Path.LastIndexOf("/") + 1) + " | " + lstScanFiles[0].Document_Type.ToString() + " | " + lstScanFiles[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstScanFiles[0].Document_Date).ToString("dd-MMM-yyyy");
                        Session["Scan_Index"] = null;
                        if (lstScanFiles[0].Scan_Index_Conversion_ID != 0 && lstScanFiles[0].Source.ToUpper() == "SCAN")
                            Session["Scan_Index"] = "INDEX_SCREEN";
                    }
                }

                //END:---
                tabView.Tabs.Clear();
                PageViewScan.ContentUrl = string.Empty;
                PageViewResult.ContentUrl = string.Empty;
                PageViewResultFiles.ContentUrl = string.Empty;
                PageViewSpirometryResults.ContentUrl = string.Empty;
                PageViewABIResults.ContentUrl = string.Empty;
                PageViewMessageLog.ContentUrl = string.Empty;
                PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + TargetValue.ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Session["human_id"].ToString();
                PageViewScan.Selected = true;
                if (OrderSubID != null)//added  for BugID:45708
                {
                    ResultMaster objresultmaster = new ResultMaster();
                    ResultMasterManager masterProxy = new ResultMasterManager();
                    objresultmaster = masterProxy.GetReviewCommentsForViewIndexedImages(Convert.ToUInt64(Session["human_id"]), OrderSubID);
                    if (objresultmaster != null && objresultmaster.Id != 0)
                    {
                        //BugID:46221 -- Prov Notes History and Med Notes History
                        txtMedicalAssistantNotes.Text = string.Empty;
                        DLC.txtDLC.Text = string.Empty;
                        txtMedNoteshistory.Text = (objresultmaster.MA_Notes.Trim() != string.Empty && (objresultmaster.MA_Notes.IndexOf("):") == -1)) ?
                            "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.MA_Notes : objresultmaster.MA_Notes;
                        txtMedNoteshistory.Text = txtMedNoteshistory.Text.Replace("<br/>", "\n");
                        txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                            "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.Result_Review_Comments : objresultmaster.Result_Review_Comments;
                        txtProvNoteshistory.Text = txtProvNoteshistory.Text.Replace("<br/>", "\n");
                        Session["Notes"] = objresultmaster;
                    }
                    else
                    {
                        txtMedicalAssistantNotes.Text = string.Empty;
                        DLC.txtDLC.Text = string.Empty;
                        txtMedNoteshistory.Text = string.Empty;
                        txtProvNoteshistory.Text = string.Empty;
                    }
                }

            }
            else
            {
                tabView.Tabs.Clear();
                PageViewScan.ContentUrl = string.Empty;
                PageViewResult.ContentUrl = string.Empty;
                PageViewResultFiles.ContentUrl = string.Empty;
                PageViewSpirometryResults.ContentUrl = string.Empty;
                PageViewABIResults.ContentUrl = string.Empty;
                PageViewMessageLog.ContentUrl = string.Empty;

                if (cboDocumentType.SelectedItem.Text == "Results")
                {
                    try
                    {
                        if (UInt64.TryParse(TargetValue, out order_id))
                        {
                            Session["Order_Id"] = order_id;
                        }
                        ulong Result_Master_Id = 0;
                        IList<string> Tab_Name = new List<string>();
                        if (order_id != 0)
                        {
                            if (result_master_list != null && result_master_list.Count > 0)
                            {
                                IList<ResultMaster> lstresult = new List<ResultMaster>();
                                lstresult = ((from doc in result_master_list
                                              where doc.Order_ID == order_id
                                              orderby doc.MSH_Date_And_Time_Of_Message descending
                                              select doc)).ToList<ResultMaster>();
                                //BugID:48413 Tabs are sorted by MSH_Date_And_Time_Of_Message descending
                                if (lstresult != null && lstresult.Count > 0)
                                {
                                    Result_Master_Id = lstresult[0].Id;
                                    Session["Result_Master_Id"] = Result_Master_Id;
                                    Tab_Name.Add("Result Master");
                                    //BugID:47602
                                    foreach (ResultMaster rm in lstresult)
                                    {
                                        tb = new RadTab();
                                        tb.Target = "Result Master";
                                        PDFGenerator pdfGenertor = new PDFGenerator();
                                        string dateTime = pdfGenertor.SetDateTimeToControl(rm.MSH_Date_And_Time_Of_Message, "DateAndTime");
                                        if (dateTime != string.Empty)
                                            tb.Text = dateTime.Substring(0, dateTime.LastIndexOf(':')) + " " + dateTime.Substring(dateTime.LastIndexOf(':')).Split(' ')[1];
                                        tb.Value = rm.Id.ToString();
                                        tabView.Tabs.Add(tb);
                                    }
                                    if (tabView.Tabs.Count > 0)
                                        tabView.SelectedIndex = 0;
                                }

                            }
                        }
                        else
                        {
                            UInt64.TryParse(KeyValue, out Result_Master_Id);
                            Tab_Name.Add("Result Master");

                        }
                        ResultMaster objresultmaster = new ResultMaster();
                        ResultMasterManager masterProxy = new ResultMasterManager();
                        if (TargetValue == "0")//to handle Diagnostic_results which do not have OrderID.//BugID:45807
                        {
                            objresultmaster = masterProxy.GetById(Convert.ToUInt64(KeyValue));
                        }
                        else
                        {
                            objresultmaster = masterProxy.GetById(Convert.ToUInt64(Session["Result_Master_Id"]));//BugID:47602
                        }
                        if (objresultmaster != null && objresultmaster.Id != 0)
                        {
                            //BugID:46221 -- Prov Notes History and Med Notes History
                            txtMedicalAssistantNotes.Text = string.Empty;
                            DLC.txtDLC.Text = string.Empty;
                            txtMedNoteshistory.Text = (objresultmaster.MA_Notes.Trim() != string.Empty && (objresultmaster.MA_Notes.IndexOf("):") == -1)) ?
                            "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                            "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.MA_Notes : objresultmaster.MA_Notes;
                            txtMedNoteshistory.Text = txtMedNoteshistory.Text.Replace("<br/>", "\n");
                            txtProvNoteshistory.Text = (objresultmaster.Result_Review_Comments.Trim() != string.Empty && (objresultmaster.Result_Review_Comments.IndexOf("):") == -1)) ?
                                "@" + (objresultmaster.Modified_By.Trim() != string.Empty ? objresultmaster.Modified_By : objresultmaster.Created_By) +
                                "(" + (objresultmaster.Modified_Date_And_Time != DateTime.MinValue ? UtilityManager.ConvertToLocal(objresultmaster.Modified_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") : UtilityManager.ConvertToLocal(objresultmaster.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")) + "): " + objresultmaster.Result_Review_Comments : objresultmaster.Result_Review_Comments;
                            txtProvNoteshistory.Text = txtProvNoteshistory.Text.Replace("<br/>", "\n");
                            Session["Notes"] = objresultmaster;
                        }
                        else
                        {
                            txtMedicalAssistantNotes.Text = string.Empty;
                            DLC.txtDLC.Text = string.Empty;
                            txtMedNoteshistory.Text = string.Empty;
                            txtProvNoteshistory.Text = string.Empty;
                        }

                        for (int i = 0; i < Tab_Name.Count; i++)
                        {
                            tb = new RadTab();
                            tb.Target = "tb" + Tab_Name[i].ToLower();
                            if (Tab_Name[i].ToLower() == "scan")
                            {
                                btnPrintEducatnMaterial.Visible = false;
                                IList<string> lstScanId = ((from doc in filelist
                                                            where doc.Order_ID == order_id
                                                            select doc.File_Path).Distinct()).ToList<string>();
                                tb.Text = "Scanned Results";
                                /* File Header */
                                IList<FileManagementIndex> lstScanIds = (from doc in filelist where doc.Order_ID == order_id select doc).ToList<FileManagementIndex>();
                                if (lstScanIds != null && lstScanIds.Count > 0)
                                    //modified for integrum
                                    txtFileInformation.Value = lstScanIds[0].File_Path.Substring(lstScanIds[0].File_Path.LastIndexOf("/") + 1) + " | " + "Results" + " | " + lstScanIds[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstScanIds[0].Document_Date).ToString("dd-MMM-yyyy");//BugID:42809
                            }

                            else if (Tab_Name[i].ToLower() == "order")
                            {
                                tb.Text = "Result Files";
                            }
                            else if (Tab_Name[i].ToLower() == "result master")
                            {
                                PDFGenerator objPDFGenerator = new PDFGenerator();
                                string filepath = string.Empty;
                                //if(Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                                filepath = Server.MapPath("Documents/" + Session.SessionID);
                                hdnSelectedItem.Value = String.Empty;
                                string file = string.Empty;
                                string filename = objPDFGenerator.GenerateRequestionForLabcorp(Result_Master_Id, filepath);
                                hdnFaxpath.Value = filename +"|"+ objresultmaster.Order_ID;
                                string[] Split = null;
                                //if(Directory.Exists(Server.MapPath("Documents\\" + Session.SessionID)))
                                Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                                string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                                if (FileName != null && FileName.Length > 0)
                                    file = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
                                string loc = "DYNAMIC";
                                if (hdnSelectedItem.Value == string.Empty)
                                {
                                    if (FileName != null && FileName.Length > 0)
                                    {
                                        hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
                                        txtFileInformation.Value = FileName[0].ToString() + " | " + "Results";
                                    }

                                    tb.Value = order_id.ToString();
                                    tabView.Tabs.Add(tb);
                                }
                                else
                                {
                                    if (FileName != null && FileName.Length > 0)
                                    {
                                        hdnSelectedItem.Value += "|" + FileName[0].ToString();
                                    }
                                }
                                PageViewResult.ContentUrl = hdnSelectedItem.Value.ToString();
                                PageViewResult.Selected = true;

                            }
                            else if (Tab_Name[i].ToLower() == "abi")
                            {
                                tb.Text = "ABI Results";
                                tb.Value = order_id.ToString();
                                tabView.Tabs.Add(tb);

                            }
                            else if (Tab_Name[i].ToLower() == "spirometry")
                            {
                                tb.Text = "Spirometry Results";
                                tb.Value = order_id.ToString();
                                tabView.Tabs.Add(tb);

                            }
                            else if (Tab_Name[i].ToLower() == "message")
                            {
                                tb.Text = "Message Log";
                                tb.Value = order_id.ToString();
                                tabView.Tabs.Add(tb);
                            }



                        }

                        for (int i = 0; i < Tab_Name.Count; i++)
                        {
                            if (Tab_Name[i].ToLower() == "scan")
                            {
                                IList<string> lstScanId = ((from doc in filelist
                                                            where doc.Order_ID == order_id
                                                            select doc.File_Path).Distinct()).ToList<string>();
                                if (filelist.Count == 1 && filelist.Count > 0)
                                {
                                    /* Changing The Image Viewer To View the Multiple Results Attached To Same Order  */
                                    if (lstScanId != null && lstScanId.Count > 0)
                                        PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Session["human_id"].ToString();
                                }
                                else
                                {
                                    /* To Avoid Alteration Of Existing Working , Left The Code Block For Single Result To Order Mapping */
                                    if (lstScanId != null && lstScanId.Count > 0)
                                        PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Session["human_id"].ToString();
                                }
                                PageViewScan.Selected = true;
                                break;
                            }

                            else if (Tab_Name[i].ToLower() == "order")
                            {
                                FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                                // IList<FileManagementIndex> filelist_Orders = objfileproxy.GetImagesforAnnotations(Convert.ToUInt32(Session["HumanId"]), Convert.ToUInt64(TargetValue), "ORDER");
                                IList<FileManagementIndex> lstfile = new List<FileManagementIndex>();
                                lstfile = (from doc in filelist
                                           where doc.Source == "ORDER"
                                           select doc).ToList<FileManagementIndex>();
                                if (lstfile != null && lstfile.Count > 0)
                                {
                                    if (lstfile[0].Source == "ORDER")
                                    {
                                        string concatenatedFiles = string.Empty;

                                        string filename = string.Empty;
                                        lstfile.OrderBy(a => a.Id).FirstOrDefault();
                                        tb = new RadTab();
                                        tb.Target = "tbOrder";
                                        tb.Text = "Result Files";
                                        tabView.Tabs.Add(tb);
                                        if (Request["Type"] == "Results")
                                        {
                                            IList<FileManagementIndex> IndexList = new List<FileManagementIndex>();
                                            IndexList = lstfile.Where(f => f.Id == Convert.ToUInt32(KeyValue)).ToList<FileManagementIndex>();
                                            if (IndexList != null && IndexList.Count > 0)
                                            {
                                                PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + IndexList[0].File_Path;
                                            }
                                            else if (lstfile != null && lstfile.Count > 0)
                                            {
                                                PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[lstfile.Count - 1].File_Path;
                                            }
                                        }
                                        else if (Request.QueryString["Screen"] == "ResultView" || Request.QueryString["Screen"] == "OrderManagement")
                                        {
                                            PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + TargetValue;
                                        }
                                        else if (lstfile != null && lstfile.Count > 0)
                                        {
                                            PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[lstfile.Count - 1].File_Path;
                                        }
                                        else
                                        {
                                            PageViewResult.ContentUrl = "frmImageViewer.aspx?Source=" + "CmgResult" + "&FilePath=" + lstfile[01].File_Path;
                                        }
                                        PageViewResult.Selected = true;
                                        //modified for Integrum
                                        txtFileInformation.Value = lstfile[0].File_Path.Split('/')[5] + "|" + lstfile[0].Document_Type.ToString() + " | " + lstfile[0].Document_Sub_Type.ToString() + " | " + UtilityManager.ConvertToLocal(lstfile[0].Document_Date).ToString("dd-MMM-yyyy");//BugID:42809
                                    }
                                }

                                //PageViewResultFiles.ContentUrl = "frmImageViewer.aspx?Screen=ResultView" + "&OrderSubmitId=" + Request["OrderSubmitId"];
                                //PageViewResultFiles.Selected = true;
                                break;
                            }
                            else if (Tab_Name[i].ToLower() == "result master")
                            {
                                //PageViewResult.ContentUrl = "frmResult.aspx?Result_Master_ID=" + Result_Master_Id + "&strScreenName=PATIENT CHART";
                                //PageViewResult.Selected = true;
                                //break;
                            }
                            else if (Tab_Name[i].ToLower() == "abi")
                            {
                                PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Session["human_id"].ToString();
                                PageViewABIResults.Selected = true;
                                break;
                            }
                            else if (Tab_Name[i].ToLower() == "spirometry")
                            {
                                PageViewSpirometryResults.ContentUrl = "frmSpirometry.aspx?HumanId=" + Session["human_id"].ToString();
                                PageViewSpirometryResults.Selected = true;
                                break;
                            }
                            else if (Tab_Name[i].ToLower() == "message")
                            {
                                break;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        //do nothing
                        throw e;

                    }
                }

            }
        }
        protected void tvViewIndex_NodeClick1(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Nodes.Count == 0 && e.Node.Attributes["OrderSubmitId"] != null)//BugID:48326
            {
                TabLoad(e.Node.Value.ToString(), e.Node.Target.ToString(), e.Node.Attributes["OrderSubmitId"].ToString());

                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLog activity = new ActivityLog();

                activity.Human_ID = Convert.ToUInt64(Session["human_id"].ToString());
                activity.Encounter_ID = 0;
                activity.Activity_Type = "Patient is accessing the document - Document Type - " + cboDocumentType.SelectedItem.Text + " and Document ID - " + e.Node.Value.ToString();
                activity.Activity_By = ClientSession.UserName;
                activity.Activity_Date_And_Time = DateTime.Now;
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        #endregion
        #region itemEvents
        protected void pbFilter_Click(object sender, ImageClickEventArgs e)
        {
            //pbPlus.ID = "pbPlus";
            //if (cboDocumentType.Text == "Results")
            //{
            //    loadTreeView(cboDocumentType.Text, "");
            //}
            //else
            //{
            //    loadTreeView(cboDocumentType.Text, txtDocumentSubType.Text);
            //}
        }
        protected void cboDocumentType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //loadTreeView(cboDocumentType.Text, "");
            if (cboDocumentType != null && cboDocumentType.Items.Count > 0 && cboDocumentType.SelectedItem.Text != string.Empty)
            {
                Session["Notes"] = null;//BugID:46316 -- by default value is null.
                Session["Doc_type"] = cboDocumentType.SelectedItem.Text;
                DLC.txtDLC.Text = string.Empty;
                txtMedicalAssistantNotes.Text = string.Empty;
                txtProvNoteshistory.Text = string.Empty;
                txtMedNoteshistory.Text = string.Empty;
                DLC.txtDLC.Enabled = false;
                txtMedicalAssistantNotes.Enabled = false;
                DLC.txtDLC.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                DLC.txtDLC.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                DLC.txtDLC.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtMedicalAssistantNotes.BackColor = System.Drawing.ColorTranslator.FromHtml("#BFDBFF");
                txtMedicalAssistantNotes.BorderColor = System.Drawing.ColorTranslator.FromHtml("Black");
                txtMedicalAssistantNotes.ForeColor = System.Drawing.ColorTranslator.FromHtml("Black");
                ConstructTreeView(Convert.ToUInt64(Session["human_id"]), cboDocumentType.SelectedItem.Text, 0);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);//to stopload after documenttype is loaded.
                pbPlus.ID = "pbPlus";
            }


        }
        protected void pbPlus_Click(object sender, ImageClickEventArgs e)
        {
            //if (pbPlus.ID == "pbPlus")
            //{
            //    HtmlTable ctrl = (HtmlTable)Page.FindControl("tblTree");
            //    lsttype = new RadListBox();
            //    lsttype.ID = "lstDynamic";
            //    lsttype.Height = 100;
            //    lsttype.Width = txtDocumentSubType.Width;
            //    lsttype.OnClientSelectedIndexChanged = "SelectedIndexChanged";
            //    ctrl.Rows[3].Cells[0].Controls.Add(lsttype);
            //    pbPlus.ImageUrl = "~/Resources/minus_new.gif";
            //    pbPlus.ID = "pbMinus";
            //    LoadlstNotes();
            //    lsttype.Visible = true;
            //}
            //else
            //{
            //    lsttype.Visible = false;
            //    pbPlus.ImageUrl = "~/Resources/plus_new.gif";
            //    pbPlus.ID = "pbPlus";
            //}


        }

        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {


            WFObjectManager obj_workFlow = new WFObjectManager();
            ulong LabId = 0;
            if (Request["LabId"] != null)
                LabId = Convert.ToUInt32(Request["LabId"]);
            else if (Request["Screen"] != null && Request["Screen"] == "ResultViewOrder")
                LabId = 32;
            //if (btnSave.Enabled == true)//bug Id:56084 
            // {
            SaveNotes();
            //  }
            if (LabId == 32)
            {
                FileManagementIndexManager objfileproxy = new FileManagementIndexManager();
                IList<FileManagementIndex> lstfilemanage = new List<FileManagementIndex>();
                lstfilemanage = objfileproxy.GetImagesforAnnotations(Convert.ToUInt64(Request["HumanID"]), Convert.ToUInt64(Request["OrderSubmitId"]), "ORDER,ABI,SPIROMETRY,SCAN");
                if (lstfilemanage != null && lstfilemanage.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115039'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;

                }

                OrdersSubmitManager objordeproxy = new OrdersSubmitManager();
                IList<OrdersSubmit> lstordersubmit = new List<OrdersSubmit>();
                lstordersubmit = objordeproxy.GetOrdersSubmitListbyID(Convert.ToUInt64(Request["OrderSubmitId"]));

                if (lstordersubmit != null && lstordersubmit.Count > 0)
                {
                    if (lstordersubmit[0].Specimen_Collection_Date_And_Time == DateTime.MinValue)
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115040'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                        return;
                    }

                }
            }

            string Current_Process = string.Empty;
            int close_type = 0;
            string User_name = string.Empty;
            string[] current_process = new string[] { Request["CurrentProcess"].Replace(" _", "_").ToString() };

            if (Request["CurrentProcess"] != null)
                Current_Process = Request["CurrentProcess"].Replace(" _", "_").ToString();

            UserManager objPhy = new UserManager();
            string sPhysicianName = string.Empty;

            IList<Core.DomainObjects.User> PhyUserList = objPhy.getUserByPHYID(ClientSession.PhysicianId);
            if (PhyUserList != null && PhyUserList.Count > 0)
            {
                sPhysicianName = PhyUserList[0].user_name;
                User_name = PhyUserList[0].user_name;
            }

            if (Current_Process == "ORDER_GENERATE" && ClientSession.UserRole == "Medical Assistant")
            {
                close_type = 8;

            }
            else if (Current_Process == "MA_REVIEW" && ClientSession.UserRole == "Medical Assistant")
            {
                close_type = 1;
                User_name = ClientSession.UserName;

            }
            else if (Current_Process == "ORDER_GENERATE" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
            {
                close_type = 9;
                User_name = "UNKNOWN";
            }
            else if (Current_Process == "RESULT_REVIEW" && (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant"))
            {
                close_type = 1;
                User_name = "UNKNOWN";
            }
            else if (Current_Process == "MA_RESULTS")
            {
                close_type = 3;
                User_name = "UNKNOWN";
            }








            if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
            {

                if (chkPhyName.Checked == true)
                {
                    if (btnSave.Enabled == true)
                    {
                        SaveNotes();
                    }

                    IList<string> _Status_Flag = new List<string>();
                    if (Session["Status_Flag"] != null)
                    {
                        _Status_Flag = (IList<string>)Session["Status_Flag"];
                    }
                    // string[] current_process = new string[] { "RESULT_REVIEW" };
                    if (_Status_Flag != null)
                    {
                        if (_Status_Flag.Count == 0)
                        {
                            //$muthusamy on 23-12-2014 for LabResult Changes
                            if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                                obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                            else
                                obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                            //$muthusamy 
                        }
                        else
                        {
                            if ((_Status_Flag.Contains("P") && _Status_Flag.Contains("F")) || _Status_Flag.Contains("F"))
                            {
                                obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                            }
                            else if (_Status_Flag.Contains("P") && !_Status_Flag.Contains("F"))
                            {
                                obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                            }
                        }
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "DisplayErrorMessage('115034'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;
                }
            }

            else
            {
                //$muthusamy on 23-12-2014 for LabResult Changes
                if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 1, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
                else
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", close_type, User_name, UtilityManager.ConvertToUniversal(), null, current_process, null);
                //$muthusamy
            }
            //Added for moving order from MA_REVIEW to RESULT_REVIEW (Bug Id: 29134)
            //Begin
            if (ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT")
            {
                if (Current_Process == "MA_REVIEW")
                {

                    string sPhyName = string.Empty;
                    UserManager objPhyMngr = new UserManager();

                    IList<Core.DomainObjects.User> PhyList = objPhyMngr.getUserByPHYID(ClientSession.PhysicianId);
                    if (PhyList != null && PhyList.Count > 0)
                        sPhyName = PhyList[0].user_name;
                    string[] current_proc = new string[] { "ORDER_GENERATE" };
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 8, sPhyName, UtilityManager.ConvertToUniversal(), null, current_proc, null);


                }
            }
            //End
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MovedSuccessfully", "DisplayErrorMessage('050002');", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", "End(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NextResult", "ViewNextResult();", true);//BugID:41027 -- move to next result
        }
        protected void btnMoveToMa_Click(object sender, EventArgs e)
        {
            //  if (btnSave.Enabled == true)//bug Id:56084 
            // {
            SaveNotes();
            // }
            WFObjectManager obj_workFlow = new WFObjectManager();
            if (btnMoveToMa.Text == "Save & Move To Provider")
            {
                PhysicianManager objphymanager = new PhysicianManager();
                PhyUserList = objphymanager.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
                string sPhysicianName = string.Empty;
                string[] current_process = new string[] { "MA_RESULTS" };
                if (PhyUserList != null && PhyUserList.UserList != null && PhyUserList.UserList.Count > 0)
                {
                    for (int i = 0; i < PhyUserList.UserList.Count; i++)
                    {
                        if (Convert.ToUInt64(PhyUserList.UserList[i].Physician_Library_ID) == Convert.ToUInt64(Request.QueryString["PhysicianId"]))
                        {
                            sPhysicianName = PhyUserList.UserList[i].user_name;
                            break;
                        }
                    }
                }

                if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                {
                    if (sPhysicianName == "")
                    {
                        WFObject objWorkflowList = new WFObject();
                        objWorkflowList = obj_workFlow.GetByObjectSystemId(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT");
                        if (objWorkflowList != null)
                        {
                            sPhysicianName = objWorkflowList.Process_Allocation.Split('|')[1].Split('-')[1];
                        }

                    }
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, sPhysicianName, UtilityManager.ConvertToUniversal(), null, current_process, null);
                }
                else
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 2, sPhysicianName, UtilityManager.ConvertToUniversal(), null, current_process, null);
                //$muthusamy
            }
            else
            {
                //For Bug Id 56084-4.9.18
                //if (cboMoveToMA.Text == string.Empty && cboMoveToMA.Visible == true)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115046'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                //    return;
                //}
                string[] current_process = new string[] { "RESULT_REVIEW" };
                IList<Encounter> lstenc = new List<Encounter>();
                EncounterManager objencmanager = new EncounterManager();
                if (Request["ObjType"] != null && Request["ObjType"].ToString().ToUpper() == "DIAGNOSTIC_RESULT")
                {
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["ResultMasterID"]), "DIAGNOSTIC_RESULT", 2, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                }
                //commented for BugID:45893
                //else if (Convert.ToUInt64(Request["EncounterId"]) != 0)
                //{
                //    lstenc = objencmanager.GetEncounterByEncounterID(Convert.ToUInt64(Request["EncounterId"]));
                //    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, lstenc.Count > 0 ? lstenc[0].Assigned_Med_Asst_User_Name : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                //}
                //else 
                //{
                //    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, "UNKNOWN", UtilityManager.ConvertToUniversal(), null, current_process, null);
                //}
                //added for BgID:45893 to move item to respective MA's Queue.
                else
                {
                    obj_workFlow.MoveToNextProcess(Convert.ToUInt64(Request["OrderSubmitId"]), "DIAGNOSTIC ORDER", 7, cboMoveToMA.Text.Contains('-') ? cboMoveToMA.Text.Split('-')[0] : string.Empty, UtilityManager.ConvertToUniversal(), null, current_process, null);
                }
            }
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Close", "End(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NextResult", "ViewNextResult();", true);//BugID:41027 -- move to next result
        }
        public void SaveNotes()
        {
            ResultMasterManager masterProxy = new ResultMasterManager();
            IList<ResultMaster> lstResultMaster = new List<ResultMaster>();
            ResultMaster objResultMaster = new ResultMaster();
            ResultMasterManager rsManager = new ResultMasterManager();
            ulong resMasID = 0;
            //For Bug Id 56084-4.9.18
            //if (ClientSession.UserRole.ToUpper().Trim() == "MEDICAL ASSISTANT" && txtMedicalAssistantNotes.Text=="")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "alert('Please Enter Medical Assistant Notes'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
            //    return;
            //}
            //if ((ClientSession.UserRole.ToUpper().Trim() == "PHYSICIAN" ||  ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT") && DLC.txtDLC.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "alert('Please Enter Provider  Notes'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
            //    return;
            //}
            if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"] != string.Empty && Convert.ToUInt32(Request["OrderSubmitId"]) != 0)
            {
                lstResultMaster = rsManager.GetResultReviewNotesBasedOnOrderSubmitId(Convert.ToUInt32(Request["OrderSubmitId"]));
                if (lstResultMaster != null && lstResultMaster.Count > 0)
                {
                    if (lstResultMaster.Count == 1)
                    {
                        objResultMaster = lstResultMaster[0];
                    }
                    else if (lstResultMaster.Count > 1)
                    {
                        if (Session["Result_Master_Id"] != null && UInt64.TryParse(Session["Result_Master_Id"].ToString(), out resMasID))
                        {
                            IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                            lstResMaster = lstResultMaster.Where(a => a.Id == resMasID).ToList<ResultMaster>();
                            if (lstResMaster != null && lstResMaster.Count > 0)
                                objResultMaster = lstResMaster[0];
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                                return;
                            }
                        }
                        else if (Request["ResultMasterID"] != null && UInt64.TryParse(Request["ResultMasterID"], out resMasID))
                        {
                            IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                            lstResMaster = lstResultMaster.Where(a => a.Id == resMasID).ToList<ResultMaster>();
                            if (lstResMaster != null && lstResMaster.Count > 0)
                                objResultMaster = lstResMaster[0];
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                                return;
                            }
                        }
                    }
                    if (objResultMaster != null && objResultMaster.Id != 0)
                    {
                        objResultMaster.Modified_By = ClientSession.UserName;
                        objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                }
                else
                {
                    objResultMaster = new ResultMaster();
                    objResultMaster.PID_External_Patient_ID = Convert.ToString(Request["HumanId"]);
                    objResultMaster.PID_Alternate_Patient_ID = objResultMaster.PID_External_Patient_ID;
                    objResultMaster.Created_By = ClientSession.UserName;
                    objResultMaster.Order_ID = Convert.ToUInt32(Request["OrderSubmitId"]);
                    objResultMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
            }
            else if (Session["Result_Master_Id"] != null && UInt64.TryParse(Session["Result_Master_Id"].ToString(), out resMasID))
            {
                IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                lstResMaster = lstResultMaster.Where(a => a.Id == resMasID).ToList<ResultMaster>();
                if (lstResMaster != null && lstResMaster.Count > 0)
                    objResultMaster = lstResMaster[0];
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115058'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;
                }
            }
            else if (Request["ResultMasterID"] != null && UInt64.TryParse(Request["ResultMasterID"], out resMasID))
            {
                IList<ResultMaster> lstResMaster = new List<ResultMaster>();
                objResultMaster = rsManager.GetById(resMasID);
                if (objResultMaster != null && objResultMaster.Id != 0)
                {
                    objResultMaster.Modified_By = ClientSession.UserName;
                    objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "DisplayErrorMessage('115057'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();StopLoadingImage();}", true);
                    return;
                }
            }

            if (objResultMaster != null)
            {
                if (DLC.txtDLC.Text.Trim() != string.Empty)
                {
                    objResultMaster.Result_Review_Comments = (txtProvNoteshistory.Text.Trim() != string.Empty ? txtProvNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + DLC.txtDLC.Text;
                    txtProvNoteshistory.Text = objResultMaster.Result_Review_Comments.Replace("<br/>", "\n");
                    DLC.txtDLC.Text = string.Empty;
                }
                else
                {
                    //For Bug Id 56084-4.9.18
                    if (DLC.txtDLC.Enabled == true)
                    {
                        objResultMaster.Result_Review_Comments = (txtProvNoteshistory.Text.Trim() != string.Empty ? txtProvNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]";
                        txtProvNoteshistory.Text = objResultMaster.Result_Review_Comments.Replace("<br/>", "\n");
                        DLC.txtDLC.Text = string.Empty;
                    }

                }
                if (txtMedicalAssistantNotes.Text.Trim() != string.Empty)
                {
                    objResultMaster.MA_Notes = (txtMedNoteshistory.Text.Trim() != string.Empty ? txtMedNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + txtMedicalAssistantNotes.Text;
                    txtMedNoteshistory.Text = objResultMaster.MA_Notes.Replace("<br/>", "\n");
                    txtMedicalAssistantNotes.Text = string.Empty;
                }
                else
                {
                    //For Bug Id 56084-4.9.18
                    if (txtMedicalAssistantNotes.Enabled == true)
                    {
                        objResultMaster.MA_Notes = (txtMedNoteshistory.Text.Trim() != string.Empty ? txtMedNoteshistory.Text + "<br/>" : string.Empty) + "@" + ClientSession.UserName + "(" + UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy hh:mm tt") + "): " + "[No Comments]";
                        txtMedNoteshistory.Text = objResultMaster.MA_Notes.Replace("<br/>", "\n");
                        txtMedicalAssistantNotes.Text = string.Empty;
                    }
                }
                objResultMaster = rsManager.SaveResultMasterItem(objResultMaster,0);
                Session["Notes"] = objResultMaster;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "SaveViewResults();", true);
            btnSave.Enabled = false;
            hdnSave.Value = "false";
        }


        protected void tabView_TabClick(object sender, RadTabStripEventArgs e)
        {
            IList<FileManagementIndex> filelist = new List<FileManagementIndex>();
            filelist = (IList<FileManagementIndex>)Session["filelist"];

            if (e.Tab.Text == "Scanned Results")
            {
                ulong order_id = (ulong)Session["Order_Id"];

                IList<string> lstScanId = ((from doc in filelist
                                            where doc.Order_ID == order_id
                                            select doc.File_Path).Distinct()).ToList<string>();
                if (lstScanId != null && lstScanId.Count > 0)
                {
                    PageViewScan.ContentUrl = "frmImageViewer.aspx?FilePath=" + lstScanId[0].ToString().Replace("#", "HASHSYMBOL") + "&Source=RESULT" + "&HumanId=" + Request["HumanId"].ToString() + "&Doc=" + cboDocumentType.Text + "&DDate=" + tvViewIndex.SelectedNode.Text.ToString();
                    PageViewScan.Selected = true;
                }


            }

            else if (e.Tab.Text == "Result Files")
            {

                IList<string> lstScanId = ((from doc in filelist
                                            where doc.Order_ID == Convert.ToUInt32(Request["OrderSubmitId"])
                                            select doc.File_Path).Distinct()).ToList<string>();
                if (lstScanId != null && lstScanId.Count > 0)
                {
                    PageViewResultFiles.ContentUrl = "frmImageViewer.aspx?Source=RESULT&FilePath=" + lstScanId[0].ToString();
                    PageViewResultFiles.Selected = true;
                }


            }
            else if (e.Tab.Text == "Results")
            {
                ulong result_id = (ulong)Session["Result_Master_Id"];
                PageViewResult.ContentUrl = "frmResult.aspx?Result_Master_ID=" + result_id.ToString() + "&strScreenName=PATIENT CHART";
                PageViewResult.Selected = true;

            }
            else if (e.Tab.Text == "ABI Results")
            {
                PageViewABIResults.ContentUrl = "frmABIResult.aspx?HumanId=" + Request["HumanId"].ToString();
                PageViewABIResults.Selected = true;

            }
            else if (e.Tab.Text == "Spirometry Results")
            {
                PageViewSpirometryResults.ContentUrl = "frmSpirometry.aspx?HumanId=" + Request["HumanId"].ToString();
                PageViewSpirometryResults.Selected = true;

            }
            else if (e.Tab.Text == "Scanned Results")
            {
                tb.Text = "Message Log";

            }
            //BugID:47602
            else if (e.Tab.Target == "Result Master")
            {
                PDFGenerator objPDFGenerator = new PDFGenerator();
                string filepath = Server.MapPath("Documents/" + Session.SessionID);
                hdnSelectedItem.Value = String.Empty;
                string file = string.Empty;
                string filename = objPDFGenerator.GenerateRequestionForLabcorp(Convert.ToUInt32(e.Tab.Value), filepath);
               
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                if (FileName != null && FileName.Length > 0)
                    file = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
                if (hdnSelectedItem.Value == string.Empty)
                {
                    if (FileName != null && FileName.Length > 0)
                    {
                        hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
                        txtFileInformation.Value = FileName[0].ToString() + " | " + "Results";
                    }

                    //tb.Value = order_id.ToString();
                    //tabView.Tabs.Add(tb);
                }
                else
                {
                    if (FileName != null && FileName.Length > 0)
                    {
                        hdnSelectedItem.Value += "|" + FileName[0].ToString();
                    }
                }
                PageViewResult.ContentUrl = hdnSelectedItem.Value.ToString();
                PageViewResult.Selected = true;

                ResultMaster objresultmaster = new ResultMaster();
                ResultMasterManager masterProxy = new ResultMasterManager();
                objresultmaster = masterProxy.GetById(Convert.ToUInt64(e.Tab.Value));
                if (objresultmaster != null && objresultmaster.Id != 0)
                {
                    hdnFaxpath.Value = filename + "|" + objresultmaster.Order_ID;
                    txtProvNoteshistory.Text = objresultmaster.Result_Review_Comments;
                    txtProvNoteshistory.Text = txtProvNoteshistory.Text.Replace("<br/>", "\n");
                    txtMedNoteshistory.Text = objresultmaster.MA_Notes;
                    txtMedNoteshistory.Text = txtMedNoteshistory.Text.Replace("<br/>", "\n");
                    Session["Notes"] = objresultmaster;
                    Session["Result_Master_Id"] = objresultmaster.Id;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ulong Result_Master_ID;
            Result_Master_ID = (ulong)Session["Result_Master_Id"];
            PDFGenerator objPDFGenerator = new PDFGenerator();
            string filepath = string.Empty;
            if (Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                filepath = Server.MapPath("Documents/" + Session.SessionID);
            hdnSelectedItem.Value = String.Empty;
            string file = string.Empty;
            string filename = objPDFGenerator.GenerateRequestionForLabcorp(Result_Master_ID, filepath);
            string[] Split = null;
            if (Directory.Exists(Server.MapPath("Documents/" + Session.SessionID)))
                Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
            string[] FileName = filename.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            if (FileName != null && FileName.Length > 0)
                file = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
            string loc = "DYNAMIC";
            if (hdnSelectedItem.Value == string.Empty)
            {
                if (FileName != null && FileName.Length > 0)
                    hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[0].ToString();
            }
            else
            {
                if (FileName != null && FileName.Length > 0)
                    hdnSelectedItem.Value += "|" + FileName[0].ToString();
            }
            RadScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF();", true);

        }

        protected void btnPrintEducatnMaterial_Click(object sender, EventArgs e)
        {
            ulong Result_Master_ID;
            string urls = "";
            string value = string.Empty;
            bool ShowAlert = true;
            //BugID:48680
            ResultMaster objresMaster = new ResultMaster();
            if (Session["Notes"] != null)
            {
                objresMaster = (ResultMaster)Session["Notes"];
                Result_Master_ID = objresMaster.Id;
                ResultOBXManager obxMgr = new ResultOBXManager();
                IList<ResultOBX> ResultOBXList = new List<ResultOBX>();
                ResultOBXList = obxMgr.GetResultByMasterID(Result_Master_ID);
                if (ResultOBXList != null && ResultOBXList.Count > 0)
                {
                    for (int i = 0; i < ResultOBXList.Count; i++)
                    {
                        value = ResultOBXList[i].OBX_Loinc_Identifier;
                        if (value != "")
                        {
                            urls += value + ";";
                        }
                    }
                }

                if (urls.Trim() != String.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "openLink('" + urls + "');", true);
                    ShowAlert = false;
                }
                else
                {
                    ShowAlert = true;
                }
            }
            if (ShowAlert)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "No Valid Loinc Observation", "alert('The required information to bring the patient education material is not present.');", true);
            }

            #region OLD CODE
            /* 
            ResultMasterManager objResultMasterManager = new ResultMasterManager();
            IList<ResultMaster> result_master_list = new List<ResultMaster>();
            result_master_list = (IList<ResultMaster>)Session["result_master_list"];
            Result_Master_ID = result_master_list[0].Id;
            FillResultDTO objFillResultDTO = new FillResultDTO();
            Stream strm = null;
            var serializer = new NetDataContractSerializer();
            strm = objResultMasterManager.GetResultByResultMasterID(Result_Master_ID);
            object ol = (object)serializer.ReadObject(strm);
            objFillResultDTO = ol as FillResultDTO;

            for (int i = 0; i < objFillResultDTO.ResultOBXList.Count; i++)
            {
                value = objFillResultDTO.ResultOBXList[i].OBX_Loinc_Identifier;
                //if (value != "")
                //{
                //    string pageurl = "http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=" + value + "&informationRecipient.languageCode.c = en";
                //    // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "window.open('" + pageurl + "','_blank');", true);
                //    urls += pageurl + ";";

                //}          
                if (value != "")
                {
                    urls += value + ";";
                }

            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "openLink('" + urls + "');", true);
         */
            #endregion
        }

        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {


            //CAP-2953
            //XDocument xmlUser = null;
            //if (File.Exists(Server.MapPath(@"ConfigXML\User.xml")))
            //    xmlUser = XDocument.Load(Server.MapPath(@"ConfigXML\User.xml"));
            //cboMoveToMA.Items.Clear();
            //cboMoveToMA.Items.Add(new RadComboBoxItem(""));
            //if (chkShowAll.Checked)
            //{
            //    foreach (XElement elements in xmlUser.Descendants("UserList"))
            //    {
            //        foreach (XElement UserElement in elements.Elements())
            //        {
            //            if (UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
            //            {
            //                string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
            //                cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (XElement elements in xmlUser.Descendants("UserList"))
            //    {
            //        foreach (XElement UserElement in elements.Elements())
            //        {
            //            if (UserElement.Attribute("Default_Facility").Value.ToUpper() == ClientSession.FacilityName.ToUpper() && UserElement.Attribute("Role").Value.ToUpper() == "MEDICAL ASSISTANT")
            //            {
            //                string xmlValue = UserElement.Attribute("User_Name").Value + "-" + UserElement.Attribute("person_name").Value;
            //                cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
            //            }
            //        }
            //    }
            //}

            UserList objUser = new UserList();
            var ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
            if (ilstUserList?.User != null)
            {
                objUser.User = ilstUserList.User;
                cboMoveToMA.Items.Clear();
                cboMoveToMA.Items.Add(new RadComboBoxItem(""));
                if (chkShowAll.Checked)
                {
                    if (objUser.User != null && objUser.User.Count() > 0)
                    {
                        var vuser = objUser.User.Where(a => a.Role.ToString().ToUpper() == "MEDICAL ASSISTANT").ToList();
                        if (vuser != null && vuser.Count() > 0)
                        {
                            for (int i = 0; i < vuser.Count(); i++)
                            {
                                string xmlValue = vuser[i].User_Name + "-" + vuser[i].person_name;
                                cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                            }
                        }
                    }
                }
                else
                {
                    if (objUser.User != null && objUser.User.Count() > 0)
                    {
                        var vdefaultuser = objUser.User.Where(a => a.Default_Facility == ClientSession.FacilityName.ToUpper() && a.Role.ToString().ToUpper() == "MEDICAL ASSISTANT").ToList();
                        if (vdefaultuser != null && vdefaultuser.Count() > 0)
                        {
                            for (int i = 0; i < vdefaultuser.Count(); i++)
                            {
                                string xmlValue = vdefaultuser[i].User_Name + "-" + vdefaultuser[i].person_name;
                                cboMoveToMA.Items.Add(new RadComboBoxItem(xmlValue, xmlValue));
                            }
                        }
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "ErrmormsgMa", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveNotes();
        }
        #endregion
    }
}
