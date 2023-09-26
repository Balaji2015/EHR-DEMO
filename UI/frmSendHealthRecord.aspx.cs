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
using System.Net.Mail;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EMRDirect.phiMail;
using System.IO;

using System.Net;


namespace Acurus.Capella.UI
{
    public partial class frmSendHealthRecord : System.Web.UI.Page
    {
        IList<PhysicianLibrary> lstPhy = null;
        int flag = 0;
        static string sIS_Patient_Portal;//BugID:50185 

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSend.Attributes.Add("onclick", "onsendclicked()");
            if (rdbtnProvider.Checked == false)
                cboProvider.Enabled = false;
            if (!IsPostBack)
            {
                PhysicianManager objPhysicianManager = new PhysicianManager();
                FillPhysicianUser PhyUserList;
                cboProvider.Enabled = false;
                txtSentTo.Enabled = false;
                btnSend.Enabled = false;
                //txtDirectAddress.Enabled = false;

                if (Request["IS_Patient_Portal"] != null && Request["IS_Patient_Portal"] != "")
                {
                    sIS_Patient_Portal = Request["IS_Patient_Portal"].ToString();
                }
                if (Request["ScreenMode"] != null && Request["ScreenMode"] != "")
                {
                    hdnScreenMode.Value = Request["ScreenMode"].ToString();
                }
                if (Request["Role"] != null && Request["Role"] != "")
                {
                    hdnRole.Value = Request["Role"].ToString();
                }
                if (hdnScreenMode.Value != null && (hdnScreenMode.Value == "ReplyMessage" || hdnScreenMode.Value == "ForwardMessage"))
                {
                    if (hdnScreenMode.Value == "ReplyMessage" && Session["Content"] != null)
                    {
                        string Message = Session["Content"].ToString();
                        SplitMessage();
                        if (txtSentTo.Text != string.Empty)
                        {
                            rdbtnSentTo.Checked = true;
                            txtSentTo.Enabled = true;

                        }
                        txtMessage.Text = Environment.NewLine + Message;
                        txtMessage.SelectionOnFocus = Telerik.Web.UI.SelectionOnFocus.CaretToBeginning;
                        //  btnCancel.Visible = true;

                    }
                    else if (hdnScreenMode.Value == "ForwardMessage" && Session["Content"] != null)
                    {
                        string Message = Session["Content"].ToString();
                        SplitMessage();
                        txtSentTo.Text = "";
                        txtMessage.Text = Environment.NewLine + Environment.NewLine + "----- Forwarded Message -----" + Message;
                        txtMessage.SelectionOnFocus = Telerik.Web.UI.SelectionOnFocus.CaretToBeginning;
                        //  btnCancel.Visible = true;
                    }
                }
                //else
                //    btnCancel.Visible = false;
                if (Request["FileName"] != null)
                {
                    string strFileName = (string)Request["FileName"];
                    strFileName = strFileName.Replace("$$", "\\");
                    if (strFileName.Contains('|'))
                    {
                        string[] Files = strFileName.Split('|');
                        lblAttachment.Text = (Files[0].IndexOf("\\") == -1) ? Files[0] : Files[0].Substring(Files[0].LastIndexOf("\\")).Replace("\\", "");//BugID:49297
                        lblAttachment1.Text = (Files[1].IndexOf("\\") == -1) ? Files[1] : Files[1].Substring(Files[1].LastIndexOf("\\")).Replace("\\", "");
                    }
                    else if (strFileName.IndexOf("\\") > -1)
                    {
                        lblAttachment.Text = strFileName.Substring(strFileName.LastIndexOf("\\")).Replace("\\", "");
                    }
                    else
                    {
                        lblAttachment.Text = strFileName;
                    }

                }
                if (Request["Attachment"] != null && Request["Attachment"].ToString() != "")
                {


                    string[] files = Request["Attachment"].Split('|');
                    for (int k = 0; k < files.Length; k++)
                    {

                        string orig_image = files[k].ToString();
                        if (lblforwardattachment.Text == "")
                            lblforwardattachment.Text = orig_image;
                        else
                            lblforwardattachment.Text = lblforwardattachment.Text + "|" + orig_image;
                    }
                }
                if (Request["Encounter_ID"] != null && Request["Encounter_ID"] != "0" && Request["Encounter_ID"] != "")
                {
                    ulong EncounterID = Convert.ToUInt32(Request["Encounter_ID"]);

                    Encounter encList = new Encounter();
                    EncounterManager encMngr = new EncounterManager();
                    encList = encMngr.GetById(EncounterID);
                    cboProvider.Items.Clear();
                    PhyUserList = objPhysicianManager.GetPhysicianandUser(false, "", ClientSession.LegalOrg);
                    int j = 1;
                    cboProvider.Items.Add(new RadComboBoxItem(""));
                    IList<string> ilstPhyMailId = new List<string>();
                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {
                        if (PhyUserList.PhyList[i].PhyEMail.Trim() != "" && PhyUserList.PhyList[i].PhyEMail.Trim() != string.Empty)
                        {
                            ilstPhyMailId.Add(PhyUserList.PhyList[i].PhyEMail.ToString());
                            string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix + " - " + PhyUserList.PhyList[i].PhyEMail.ToString();
                            cboProvider.Items.Add(new RadComboBoxItem(sPhyName));

                            cboProvider.Items[j].Value = PhyUserList.PhyList[i].Id.ToString() + "-" + PhyUserList.PhyList[i].PhyEMail.ToString();
                            if (sIS_Patient_Portal != "YES")
                            {
                                if (Convert.ToInt32(cboProvider.Items[j].Value.Split('-')[0]) == encList.Encounter_Provider_ID)
                                {
                                    cboProvider.SelectedIndex = j;
                                }
                            }
                            j++;
                        }
                    }
                    cboProvider.Items.Add(new RadComboBoxItem("Others"));
                    cboProvider.Items[j].Value = "Others";
                    ViewState["MailList"] = ilstPhyMailId;
                    //cboProvider.Items.Add(new RadComboBoxItem("Others"));
                    HumanManager humanMngr = new HumanManager();
                    Human human = new Human();
                    human = humanMngr.GetById(encList.Human_ID);
                    if (Request["FileName"] != null)// || rbtDirectAddress.Checked ==true)
                    {
                        txtSubject.Text = ((Request["FileName"].ToString().IndexOf(".zip")) != -1) ? human.Last_Name + " " + human.First_Name + " " + human.MI + "_Summary" : human.Last_Name + " " + human.First_Name + " " + human.MI + "_Summary_" + encList.Date_of_Service.ToString("dd-MMM-yyyy");
                        rdbtnProvider.Checked = true;
                        cboProvider.Enabled = true;
                        rdbtnSentTo.Checked = false;
                        txtSentTo.Enabled = false;

                    }
                    else
                    {
                        // txtSubject.Text = "";
                        //txtSentTo.Enabled = true;
                        //rdbtnSentTo.Checked = true;
                        //cboProvider.SelectedIndex = 0;
                        //cboProvider.Enabled = false;

                        rdbtnProvider.Checked = true;
                        cboProvider.Enabled = true;
                        rdbtnSentTo.Checked = false;
                        txtSentTo.Enabled = false;
                    }


                    hdnHumanID.Value = Convert.ToString(human.Id);
                    //rdbtnProvider.Checked = true;
                    if (Request["HumanEmailID"] != null)
                    {
                        txtSentTo.Enabled = true;
                        txtSentTo.Text = Request["HumanEmailID"].ToString();
                    }
                }
                else
                {
                    IList<string> ilstPhyMailId = new List<string>();
                    cboProvider.Items.Clear();
                    PhyUserList = objPhysicianManager.GetPhysicianandUser(false, "", ClientSession.LegalOrg);
                    cboProvider.Items.Add(new RadComboBoxItem(""));
                    int j = 1;
                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {

                        if (PhyUserList.PhyList[i].PhyEMail != "")
                        {
                            ilstPhyMailId.Add(PhyUserList.PhyList[i].PhyEMail.ToString());
                            string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix + " - " + PhyUserList.PhyList[i].PhyEMail.ToString();
                            cboProvider.Items.Add(new RadComboBoxItem(sPhyName));
                            cboProvider.Items[j].Value = PhyUserList.PhyList[i].Id.ToString() + "-" + PhyUserList.PhyList[i].PhyEMail.ToString();
                            j++;
                        }

                    }
                    cboProvider.Items.Add(new RadComboBoxItem("Others"));
                    cboProvider.Items[j].Value = "Others";
                    ViewState["MailList"] = ilstPhyMailId;
                    //cboProvider.Items.Add(new RadComboBoxItem("Others"));
                }
                if (Request["Scn_Name"] == "Reminder")
                {
                    txtSentTo.Text = Request["To"];
                    txtSubject.Text = Request["sub"];
                    cboProvider.Enabled = false;
                    PhysicianManager obj = new PhysicianManager();
                    lstPhy = obj.GetphysiciannameByPhyID(ClientSession.PhysicianId);
                    if (lstPhy.Count > 0)
                        Session["fromMail"] = lstPhy[0].PhyEMail;
                    if (Request["RuleID"] != null && Request["RuleID"] != string.Empty)
                    {
                        RuleMasterManager RuleMasterMngr = new RuleMasterManager();
                        RuleMaster objRuleMaster = RuleMasterMngr.GetById(Convert.ToUInt32(Request["RuleID"]));
                        txtMessage.Text = objRuleMaster.Expected_Action;
                    }
                }

                //if (Request["Bulkaccess"] != null && Request["Bulkaccess"].ToString().ToUpper() == "YES")
                //{
                //    cboProvider.Enabled = false;
                //    rdbtnProvider.Enabled = false;
                //    rdbtnSentTo.Checked = true;
                //    txtSentTo.Enabled = true;
                //    rdbtnProvider.Checked = false;
                //}

                if (ClientSession.PhysicainDetails != null && ClientSession.PhysicainDetails.Count > 0 && sIS_Patient_Portal == "NO")
                {
                    cboFrom.Items.Clear();
                    if (ClientSession.PhysicainDetails[0].PhyEMail.Trim() == "" && ClientSession.PhysicainDetails[0].Physician_Other_EMail_Username.Trim() == "")
                    {
                        pnlSend.Enabled = false;
                    }
                    else
                    {
                        if (ClientSession.PhysicainDetails[0].PhyEMail.Trim() != "")
                        {
                            cboFrom.Items.Add(new RadComboBoxItem(ClientSession.PhysicainDetails[0].PhyEMail));
                        }
                        if (ClientSession.PhysicainDetails[0].Physician_Other_EMail_Username.Trim() != "")
                        {
                            cboFrom.Items.Add(new RadComboBoxItem(ClientSession.PhysicainDetails[0].Physician_Other_EMail_Username));
                        }
                        if (ClientSession.PhysicainDetails[0].Physician_MDoffice_EMail_Username.Trim() != "")
                        {
                            cboFrom.Items.Add(new RadComboBoxItem(ClientSession.PhysicainDetails[0].Physician_MDoffice_EMail_Username));
                        }
                    }
                }
                if (sIS_Patient_Portal == "YES")
                {
                    //For patient portal invisible the From address
                    cboFrom.Visible = false;
                    lblFrom.Visible = false;
                    rdbUnEncrypted.Enabled = true;
                }
                if (sIS_Patient_Portal == "NO")//BugID:50185 
                {
                    //For patient portal visible the From address
                    cboFrom.Visible = true;
                    lblFrom.Visible = true;
                    rdbUnEncrypted.Enabled = false;
                }

                if (txtSentTo.Text != string.Empty)
                {
                    rdbtnSentTo.Checked = true;
                    txtSentTo.Enabled = true;
                    rdbtnProvider.Checked = false;
                    cboProvider.Enabled = false;
                    cboProvider.SelectedIndex = 0;
                    txtDirectAddress.Text = "";
                    txtDirectAddress.Enabled = false;
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Attachment attach = null;
            string sForCancel = hdnType.Value;
            string sForCancelInsend = hdnMessageType.Value;
            string strFileName = (string)Request["FileName"];
            List<string> FilePath = new List<string>();
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            ActivityLog activity = new ActivityLog();
            flag = 0;

            bool flagdirect = true;

            string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
            string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            string sAttachFileName = "";
            string localpathAttachment = "";
            string sFTPAddress = string.Empty;
            string[] filename = lblforwardattachment.Text.Split('|');
            if (lblforwardattachment.Text != "")
            {
                FTPImageProcess ftpImageProcess = new FTPImageProcess();
                string sFacilityPath = DateTime.Now.ToString("yyyyMMdd") + @"\";
                string sDestinationFTPPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["ftpMailpath"], sFacilityPath);
                //if (ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword))
                bool bCreateDirectory = ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword,out string sCheckFileNotFoundException);
                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                    return;
                }
                if (bCreateDirectory)
                {
                    if (filename.Length > 0)
                    {
                        for (int j = 0; j < filename.Length; j++)
                        {


                            string pdffilepath = "";


                            pdffilepath = filename[j];

                            string localPath = string.Empty;

                            string ftpUserName = string.Empty;

                            string simagePathname = string.Empty;
                            string source = string.Empty;
                            string file_path = string.Empty;
                            string _fileName = string.Empty;




                            localPath = System.Configuration.ConfigurationSettings.AppSettings["LocalPath"];
                            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"].ToString();
                            //+"//" + System.Configuration.ConfigurationSettings.AppSettings["ftpMailpath"].ToString();

                            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];


                            string localpath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//MAILINBOX");
                            DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//MAILINBOX"));
                            if (!virdir.Exists)
                            {
                                virdir.Create();
                            }
                            FileInfo[] file = virdir.GetFiles();
                            for (int i = 0; i < file.Length; i++)
                            {
                                if (Path.GetFileName(file[i].FullName) == Path.GetFileName(filename[j]))
                                {
                                    File.Delete(file[i].FullName);
                                    break;
                                }
                            }

                            FTPImageProcess ftpImage = new FTPImageProcess();


                            // DirectoryInfo childDir = new DirectoryInfo(new FileInfo(files[i]).DirectoryName);
                            string[] sDirName = filename[j].Split('/');
                            string ftpip = Path.Combine(ftpServerIP, sDirName[sDirName.Length - 2]);
                            ftpImage.DownloadFromImageServer("0", ftpip, ftpUserName, ftpPassword, Path.GetFileName(filename[j]), localpath, out string sCheckFileNotFoundExceptionses);
                            if (sCheckFileNotFoundExceptionses != "" && sCheckFileNotFoundExceptionses.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptionses.Split('~')[1] + "\");", true);
                                return;
                            }
                            string orig_image = localpath + "\\" + Path.GetFileName(filename[j]);


                            string sStoringFormat = Path.GetFileNameWithoutExtension(filename[j]) + DateTime.Now.ToString("yyyymmddhhmmss") + Path.GetExtension(pdffilepath);


                            sFTPAddress = ftpImageProcess.UploadToImageServer(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, orig_image, sStoringFormat, out string sCheckFileNotFoundExceptions);
                            if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                return;
                            }
                            if (sAttachFileName == string.Empty)
                                sAttachFileName = sFTPAddress;
                            else
                                sAttachFileName += "|" + sFTPAddress;


                            if (localpathAttachment == string.Empty)
                                localpathAttachment = orig_image;
                            else
                                localpathAttachment += "|" + orig_image;
                            FilePath.Add(orig_image);
                        }
                    }

                }
            }

            if (strFileName != null && strFileName != "")
            {
                FTPImageProcess ftpImageProcess = new FTPImageProcess();
                string sFacilityPath = DateTime.Now.ToString("yyyyMMdd") + @"\";
                string sDestinationFTPPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["ftpMailpath"], sFacilityPath);
                //if (ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword))
                bool bCreateDirectory = ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword,out string sCheckFileNotFoundException);
                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                    return;
                }
                if (bCreateDirectory)
                {





                    string localPath = string.Empty;

                    string ftpUserName = string.Empty;

                    string simagePathname = string.Empty;
                    string source = string.Empty;
                    string file_path = string.Empty;
                    string _fileName = string.Empty;




                    localPath = System.Configuration.ConfigurationSettings.AppSettings["LocalPath"];
                    ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"].ToString();
                    //+"//" + System.Configuration.ConfigurationSettings.AppSettings["ftpMailpath"].ToString();

                    ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                    ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];




                    FTPImageProcess ftpImage = new FTPImageProcess();


                    // DirectoryInfo childDir = new DirectoryInfo(new FileInfo(files[i]).DirectoryName);
                    //  string[] sDirName = filename[j].Split('/');
                    //  string ftpip = Path.Combine(ftpServerIP, sDirName[sDirName.Length - 2]);
                    // ftpImage.DownloadFromImageServer("0", ftpip, ftpUserName, ftpPassword, Path.GetFileName(filename[j]), localpath);
                    for (int k = 0; k < strFileName.Split('|').Count(); k++)
                    {




                        if (strFileName.Split('|')[k] != "")
                        {
                            // string orig_image = localpath + "\\" + Path.GetFileName(filename[j]);

                            string localpath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//MAILINBOX");
                            DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//MAILINBOX"));
                            if (!virdir.Exists)
                            {
                                virdir.Create();
                            }
                            string SelectedFilePath = Server.MapPath("~/atala-capture-download/" + Session.SessionID + "/MAILINBOX/" + "/" + Path.GetFileName(strFileName.Split('|')[k].Replace("$$", "\\")));
                            if (File.Exists(SelectedFilePath))
                            {
                                File.Delete(SelectedFilePath);
                            }
                            if (strFileName.Split('|')[k].ToString().ToUpper().IndexOf(".ZIP") > 0)
                                File.Copy(Server.MapPath("//atala-capture-download//" + Session.SessionID + "//" + strFileName.Split('|')[k].Replace("$$", "\\")), SelectedFilePath);
                            else
                                File.Copy(Server.MapPath(strFileName.Split('|')[k].Replace("$$", "\\")), SelectedFilePath);


                            string pdffilepath = "";


                            pdffilepath = SelectedFilePath;
                            string sStoringFormat = Path.GetFileNameWithoutExtension(SelectedFilePath) + DateTime.Now.ToString("yyyymmddhhmmss") + Path.GetExtension(SelectedFilePath);


                            sFTPAddress = ftpImageProcess.UploadToImageServer(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, pdffilepath, sStoringFormat,out string sCheckFileNotFoundExceptions);
                            if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                return;
                            }
                            if (sAttachFileName == string.Empty)
                                sAttachFileName = sFTPAddress;
                            else
                                sAttachFileName += "|" + sFTPAddress;


                            if (localpathAttachment == string.Empty)
                                localpathAttachment = pdffilepath;
                            else
                                localpathAttachment += "|" + pdffilepath;

                            FilePath.Add(pdffilepath);
                        }
                    }



                }
            }
            if (UploadImage.UploadedFiles.Count > 0)
            {

                string sFacilityPath = DateTime.Now.ToString("yyyyMMdd") + @"\";
                string sDestinationFTPPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["ftpMailpath"], sFacilityPath);
                FTPImageProcess ftpImageProcess = new FTPImageProcess();
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("atala-capture-upload/" + Session.SessionID + "/MAILINBOX/" + DateTime.Now.ToString("yyyyMMdd") + "/"));
                if (!dir.Exists)
                {
                    dir.Create();
                }

                int i = 1;

                //if (ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword))
                bool bCreateDirectory = ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, out string sCheckFileNotFoundException);
                if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                    return;
                }
                if (bCreateDirectory)
                {
                    if (UploadImage.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile file in UploadImage.UploadedFiles)
                        {
                            string SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/MAILINBOX/" + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddhhmmss") +  Path.GetExtension(file.FileName));
                            if (File.Exists(SelectedFilePath))
                            {
                                File.Delete(SelectedFilePath);
                            }
                            file.SaveAs(SelectedFilePath);
                            string pdffilepath = "";


                            pdffilepath = SelectedFilePath;


                            string sStoringFormat = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyymmddhhmmss") + Path.GetExtension(pdffilepath);
                            i++;

                            sFTPAddress = ftpImageProcess.UploadToImageServer(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, pdffilepath, sStoringFormat, out string sCheckFileNotFoundExceptions);
                            if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                return;
                            }
                            if (sAttachFileName == string.Empty)
                                sAttachFileName = sFTPAddress;
                            else
                                sAttachFileName += "|" + sFTPAddress;


                            if (localpathAttachment == string.Empty)
                                localpathAttachment = pdffilepath;
                            else
                                localpathAttachment += "|" + pdffilepath;


                            FilePath.Add(pdffilepath);

                        }
                    }

                }
            }
            if (rdbtnProvider.Checked)
            {
                string address = "";
                if (txtDirectAddress.Text != "")
                    address = txtDirectAddress.Text;
                else
                    address = cboProvider.SelectedItem.Value.Split('-')[1];
                string[] sListaddress = address.Split(new char[] { '.' });
                if (sListaddress.Length > 2)
                {
                    flagdirect = true;
                }
                else
                {
                    flagdirect = false;
                }
            }
            // if (rdbtnProvider.Checked && flagdirect)

            if (hdnMailtype.Value.ToUpper() == "Y")
            {


                //string sDirectAddress = "";
                //if (txtDirectAddress.Text != "")
                //    sDirectAddress = txtDirectAddress.Text;
                //else
                //    sDirectAddress = cboProvider.SelectedItem.Value.Split('-')[1];

                //IList<string> sRecList = new List<string>();
                //string[] sListRec = sDirectAddress.Split(new char[] { ',' });
                //foreach (string element in sListRec)
                //{
                //    sRecList.Insert(sRecList.Count, element);

                //}
                if (strFileName != null)
                {
                    if (strFileName.Contains("|"))
                    {
                        string[] Files = strFileName.Split('|');
                        for (int FCount = 0; FCount < Files.Count(); FCount++)
                        {
                            if (Files[FCount].IndexOf("Bulk_Acess_SOC_") != -1)
                                strFileName = "atala-capture-download//" + Session.SessionID + "//" + Files[FCount];
                            else
                                strFileName = Files[FCount].Replace("$$", "\\");

                            FilePath.Add(Server.MapPath(strFileName));




                        }

                    }
                    else
                    {
                        if (strFileName.IndexOf("Bulk_Acess_SOC_") != -1)
                            strFileName = "atala-capture-download//" + Session.SessionID + "//" + strFileName;
                        else
                            strFileName = strFileName.Replace("$$", "\\");

                        FilePath.Add(Server.MapPath(strFileName));


                    }

                }

                string toaddress = "";
                int port = 0;
                string password = string.Empty;

                if (sIS_Patient_Portal != "YES" && ClientSession.PhysicainDetails != null && ClientSession.PhysicainDetails.Count > 0)
                {
                    if (cboFrom.Text.ToString() == ClientSession.PhysicainDetails[0].PhyEMail)
                    {
                        if (ClientSession.PhysicainDetails[0].Physician_EMail_Port != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(ClientSession.PhysicainDetails[0].Physician_EMail_Port, "^[0-9]*$")==true)
                        {
                            port = Convert.ToInt32(ClientSession.PhysicainDetails[0].Physician_EMail_Port);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007017');", true);
                            return;
                        }
                        
                        if (ClientSession.PhysicainDetails[0].Physician_EMail_Password != string.Empty)
                        {
                            password = ClientSession.PhysicainDetails[0].Physician_EMail_Password;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007018');", true);
                            return;
                        }
                    }
                    else if (cboFrom.Text.ToString() == ClientSession.PhysicainDetails[0].Physician_Other_EMail_Username)
                    {
                        if (ClientSession.PhysicainDetails[0].Physician_Other_EMail_Port != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(ClientSession.PhysicainDetails[0].Physician_Other_EMail_Port, "^[0-9]*$")==true)
                        {
                            port = Convert.ToInt32(ClientSession.PhysicainDetails[0].Physician_Other_EMail_Port);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007017');", true);
                            return;
                        }

                        if (ClientSession.PhysicainDetails[0].Physician_Other_EMail_Password != string.Empty)
                        {
                            password = ClientSession.PhysicainDetails[0].Physician_Other_EMail_Password;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007018');", true);
                            return;
                        }
                    }

                    if (txtDirectAddress.Text != "")
                    {
                        toaddress = txtDirectAddress.Text;
                    }

                    else if (cboProvider.Text != "")
                    {
                        toaddress = cboProvider.Text.Split(new string[] { " - " }, StringSplitOptions.None)[1];
                        // toaddress = cboProvider.Text.Split(' - ')[1];
                    }
                    else if (txtSentTo.Text != "")
                    {

                        toaddress = txtSentTo.Text;
                    }
                    if (hdnMailtype.Value.ToUpper() == "Y")
                    {
                        ComposeEmail(cboFrom.Text, FilePath, txtMessage.Text, txtSubject.Text, port, password, toaddress);
                    }
                    else
                    {

                    }
                }
                else
                {
                    //For Patient Portal
                    // ComposeEmailPatientPortal(sRecList, FilePath, txtMessage.Text, txtSubject.Text);
                }


                txtDirectAddress.Text = "";
                txtDirectAddress.Enabled = false;
                cboProvider.Enabled = false;

                if (flag != 1)
                {
                    activity = new ActivityLog();
                    if (hdnHumanID.Value != "")
                        activity.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                    else
                        activity.Human_ID = 0;
                    if (Request["Encounter_ID"] != "" && Request["Encounter_ID"] != null)
                        activity.Encounter_ID = Convert.ToUInt32(Request["Encounter_ID"]);
                    else
                        activity.Encounter_ID = 0;
                    if (Request["FileName"] != null)
                    {
                        string FileName = Request["FileName"].ToString();
                        if (FileName.Contains('|') && FileName.Contains(".zip"))//BugID:49597 
                        {
                            activity.Activity_Type = "Transmitted Zip BOTH PDF XML";
                            activity.Encounter_ID = 0;
                        }
                        else if (FileName.Contains('|'))
                        {
                            activity.Activity_Type = "Transmitted BOTH PDF XML";
                        }
                        else if (FileName.Contains(".xml"))
                        {
                            activity.Activity_Type = "Transmitted XML";
                        }
                        else if (FileName.Contains(".pdf"))
                        {
                            activity.Activity_Type = "Transmitted PDF";
                        }
                    }
                    else
                    {
                        activity.Activity_Type = "Send Message";
                    }
                    if (hdnLocalTime.Value != string.Empty)
                        activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    //if (cboProvider.SelectedIndex > 0 && cboProvider.SelectedItem.Text != "Others")
                    //    activity.Sent_To = toaddress;
                    //else
                        activity.Sent_To = toaddress;


                    //now support two phy mail id so from address taken based on From text box
                    if (Request["IS_Patient_Portal"] != null && Request["IS_Patient_Portal"] != "" && Request["IS_Patient_Portal"].ToString().Trim().ToUpper() == "YES")//BugID:49606
                        sIS_Patient_Portal = "YES";
                    if (sIS_Patient_Portal != "YES")
                        activity.From_Address = cboFrom.Text;
                    else if (Request["LoginEmailID"] != null && Request["LoginEmailID"].ToString().Trim() != string.Empty)
                        activity.From_Address = Convert.ToString(Request["LoginEmailID"]);

                    activity.Subject = txtSubject.Text;
                    activity.Message = txtMessage.Text;
                    ArrayList message = ActivitylogMngr.sha2message(txtMessage.Text);
                    if (message.Count > 0)
                        activity.Encrypted_Message = message[0].ToString();
                    activity.Role = hdnRole.Value;
                    activity.Fax_File_Path = sAttachFileName;
                    ActivityLogList.Add(activity);
                    ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
                }
                btnSend.Enabled = false;

                lblAttachment.Text = "";
                lblAttachment1.Text = "";//BugID:50313
            }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('420057');", true);

        //    }
            //}
            #region
            ////if (rbtDirectAddress.Checked == true)
            //else if (cboProvider.Text != "Others" && txtDirectAddress.Text.Trim() != "")
            //{
            //    FilePath.Clear();
            //    if (strFileName != null && strFileName != string.Empty)
            //    {
            //        if (strFileName.Contains("|"))
            //        {
            //            string[] Files = strFileName.Split('|');
            //            foreach (string File in Files)
            //                FilePath.Add(File.Replace("$$", "\\"));

        //        }
            //        else
            //        {
            //            FilePath.Add(strFileName.Replace("$$", "\\"));
            //        }
            //    }
            //    if (txtSubject.Text == string.Empty || txtSubject.Text == null)
            //    {
            //        hdnType.Value = string.Empty;
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295015');", true);
            //        return;
            //    }
            //    if (txtMessage.Text == string.Empty || txtMessage.Text == null)
            //    {
            //        hdnType.Value = string.Empty;
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295016');", true);
            //        return;
            //    }
            //    if (txtDirectAddress.Text == string.Empty)
            //    {
            //        hdnType.Value = string.Empty;
            //        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('420057');", true);
            //        return;
            //    }
            //    IList<string> sRecList = new List<string>();
            //    string[] sListRec = txtDirectAddress.Text.Split(new char[] { ',' });
            //    foreach (string element in sListRec)
            //    {
            //        sRecList.Insert(sRecList.Count, element);
            //    }
            //    ComposeEmail(sRecList, FilePath, txtMessage.Text, txtSubject.Text);
            //    btnSend.Enabled = false;
            //}
            #endregion
            //else
            //{
            //  else if (rdbEncrypted.Checked == false)
            else
            {
                string toaddress = "";
                try
                {

                    

                    if (txtDirectAddress.Text != "")
                    {
                        toaddress = txtDirectAddress.Text;
                    }

                    else if (cboProvider.Text != "")
                    {
                        toaddress = cboProvider.Text.Split(new string[] { " - " }, StringSplitOptions.None)[1];
                        // toaddress = cboProvider.Text.Split(' - ')[1];
                    }
                    else if (txtSentTo.Text != "")
                    {

                        toaddress = txtSentTo.Text;
                    }
                    string[] receipient = toaddress.Split(';');

                    MailMessage mail = new MailMessage();

                    mail.From = new MailAddress(cboFrom.Text);
                    foreach (string s in receipient)
                    {
                        mail.To.Add(s);
                    }

                    mail.Subject = txtSubject.Text;
                    mail.Body = txtMessage.Text;
                    string sYahoomail = strFileName;
                    string[] Files;
                    if (sYahoomail != null)
                    {

                        if (sYahoomail.Contains("|"))
                        {
                            Files = sYahoomail.Split('|');
                            for (int FCount = 0; FCount < Files.Count(); FCount++)
                            {
                                if (Files[FCount].IndexOf("Bulk_Acess_SOC_") != -1)
                                    sYahoomail = "atala-capture-download//" + Session.SessionID + "//" + Files[FCount];
                                else
                                    sYahoomail = Files[FCount].Replace("$$", "\\");

                                attach = new Attachment(Server.MapPath(sYahoomail));
                                mail.Attachments.Add(attach);
                            }

                        }
                        else
                        {
                            if (sYahoomail.IndexOf("Bulk_Acess_SOC_") != -1)
                            {
                                if (sYahoomail.Contains("atala-capture-download") != true)
                                    sYahoomail = "atala-capture-download//" + Session.SessionID + "//" + sYahoomail;
                            }
                            else
                                sYahoomail = sYahoomail.Replace("$$", "\\");

                            string path = Server.MapPath(sYahoomail);
                            attach = new Attachment(path);
                            mail.Attachments.Add(attach);
                        }
                        Files = localpathAttachment.Trim().Split('|');
                        for (int FCount = 0; FCount < Files.Count(); FCount++)
                        {

                            strFileName = Files[FCount].Replace("$$", "\\");
                            if (strFileName != "")
                            {
                                attach = new Attachment(strFileName);
                                mail.Attachments.Add(attach);
                            }

                        }

                    }
                    else
                    {
                        Files = localpathAttachment.Trim().Split('|');
                        for (int FCount = 0; FCount < Files.Count(); FCount++)
                        {

                            strFileName = Files[FCount].Replace("$$", "\\");
                            if (strFileName != "")
                            {
                                attach = new Attachment(strFileName);
                                mail.Attachments.Add(attach);
                            }

                        }

                    }

                    string smptservername = "";
                    string frommail = "";
                    string password = "";
                    int port = 0;
                    if (ClientSession.PhysicainDetails != null && ClientSession.PhysicainDetails.Count > 0 && sIS_Patient_Portal == "NO")
                    {

                        if (ClientSession.PhysicainDetails[0].PhyEMail.Trim() == cboFrom.Text)
                        {
                            frommail = ClientSession.PhysicainDetails[0].PhyEMail.Trim();
                            smptservername = ClientSession.PhysicainDetails[0].Mail_Server_Address.Trim();
                            if (System.Text.RegularExpressions.Regex.IsMatch(ClientSession.PhysicainDetails[0].Physician_EMail_Port, "^[0-9]*$") == true)
                            {
                                port = Convert.ToInt32(ClientSession.PhysicainDetails[0].Physician_EMail_Port);
                            }
                            password = ClientSession.PhysicainDetails[0].Physician_EMail_Password.Trim();
                        }
                        else if (ClientSession.PhysicainDetails[0].Physician_Other_EMail_Username.Trim() == cboFrom.Text)
                        {
                            frommail = ClientSession.PhysicainDetails[0].Physician_Other_EMail_Username.Trim();
                            smptservername = ClientSession.PhysicainDetails[0].Physician_Other_EMail_Server_Address.Trim();
                            if (System.Text.RegularExpressions.Regex.IsMatch(ClientSession.PhysicainDetails[0].Physician_EMail_Port, "^[0-9]*$") == true)
                            {
                                port = Convert.ToInt32(ClientSession.PhysicainDetails[0].Physician_Other_EMail_Port);
                            }
                            password = ClientSession.PhysicainDetails[0].Physician_Other_EMail_Password.Trim();
                        }
                        else if (ClientSession.PhysicainDetails[0].Physician_MDoffice_EMail_Username.Trim() == cboFrom.Text)
                        {
                            frommail = ClientSession.PhysicainDetails[0].Physician_MDoffice_EMail_Username.Trim();
                            smptservername = System.Configuration.ConfigurationSettings.AppSettings["MDOfficeMail"].ToString();
                            if (System.Text.RegularExpressions.Regex.IsMatch(System.Configuration.ConfigurationSettings.AppSettings["MDOfficeMailPort"].ToString(), "^[0-9]*$") == true)
                            {
                                port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MDOfficeMailPort"].ToString());
                            }
                            password = ClientSession.PhysicainDetails[0].Physician_MDoffice_EMail_Password.Trim();
                        }

                    }

                    SmtpClient SmtpServer = new SmtpClient(smptservername);
                    SmtpServer.Port = port;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(frommail, password);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "alert('Message Sending Failed');", true);
                    return;
                    //MessageBox.Show(ex.ToString());
                }
                activity = new ActivityLog();
                if (hdnHumanID.Value != "")
                    activity.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
                else
                    activity.Human_ID = 0;
                if (Request["Encounter_ID"] != "" && Request["Encounter_ID"] != null)
                    activity.Encounter_ID = Convert.ToUInt32(Request["Encounter_ID"]);
                else
                    activity.Encounter_ID = 0;
                if (Request["FileName"] != null)
                {
                    string FileName = Request["FileName"].ToString();
                    if (FileName.Contains('|') && FileName.Contains(".zip"))//BugID:49597 
                    {
                        activity.Activity_Type = "Transmitted Zip BOTH PDF XML";
                        activity.Encounter_ID = 0;
                    }
                    else if (FileName.Contains('|'))
                    {
                        activity.Activity_Type = "Transmitted BOTH PDF XML";
                    }
                    else if (FileName.Contains(".xml"))
                    {
                        activity.Activity_Type = "Transmitted XML";
                    }
                    else if (FileName.Contains(".pdf"))
                    {
                        activity.Activity_Type = "Transmitted PDF";
                    }
                }
                else
                {
                    activity.Activity_Type = "Send Message";
                }
                if (hdnLocalTime.Value != string.Empty)
                    activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);

                activity.Sent_To = toaddress;

                //activity.From_Address = "support@acurussolutions.com";//For bug ID 51330
                //if (Request["LoginEmailID"] != null && Request["LoginEmailID"].ToString().Trim() != string.Empty)
                //    activity.From_Address = Convert.ToString(Request["LoginEmailID"]);

                if (sIS_Patient_Portal != "YES")
                    activity.From_Address = cboFrom.Text;
                else if (Request["LoginEmailID"] != null && Request["LoginEmailID"].ToString().Trim() != string.Empty)
                    activity.From_Address = Convert.ToString(Request["LoginEmailID"]);

                activity.Subject = txtSubject.Text;
                activity.Message = txtMessage.Text;
                ArrayList message = ActivitylogMngr.sha2message(txtMessage.Text);
                if (message.Count > 0)
                    activity.Encrypted_Message = message[0].ToString();
                activity.Role = hdnRole.Value;
                activity.Fax_File_Path = sAttachFileName;
                ActivityLogList.Add(activity);
                ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);


            }
            //else
            //{
            //    IList<StaticLookup> objSenderAddress = new List<StaticLookup>();
            //    StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
            //    objSenderAddress = objStaticLookupMgr.getStaticLookupByFieldName("E MAIL", "Sort_Order");


            //    SmtpClient dd = new SmtpClient(objSenderAddress[0].Default_Value, 25);
            //    string ToMailId = string.Empty;
            //    List<string> ToAddress = new List<string>();
            //    dd.EnableSsl = true;
            //    dd.UseDefaultCredentials = true;
            //    if (objSenderAddress.Count > 0)
            //        dd.Credentials = new System.Net.NetworkCredential(objSenderAddress[0].Value, objSenderAddress[0].Description);

            //    MailMessage mailMessage = new MailMessage();
            //    mailMessage.IsBodyHtml = true;
            //    if (objSenderAddress.Count > 0)
            //        mailMessage.From = new MailAddress(objSenderAddress[0].Value);
            //    // string sYahoomail = strFileName;
            //    if (strFileName != null)
            //    {
            //        string[] Files;
            //        if (strFileName.Contains("|"))
            //        {
            //            Files = strFileName.Split('|');
            //            for (int FCount = 0; FCount < Files.Count(); FCount++)
            //            {
            //                if (Files[FCount].IndexOf("Bulk_Acess_SOC_") != -1)
            //                    strFileName = "atala-capture-download//" + Session.SessionID + "//" + Files[FCount];
            //                else
            //                    strFileName = Files[FCount].Replace("$$", "\\");
            //                //Attachment attach = new Attachment(Server.MapPath("~" + strFileName));
            //                attach = new Attachment(Server.MapPath(strFileName));
            //                mailMessage.Attachments.Add(attach);
            //                // attach.Dispose();

            //            }

            //        }
            //        else
            //        {
            //            if (strFileName.IndexOf("Bulk_Acess_SOC_") != -1)
            //                strFileName = "atala-capture-download//" + Session.SessionID + "//" + strFileName;
            //            else
            //                strFileName = strFileName.Replace("$$", "\\");
            //            // Attachment attach = new Attachment(Server.MapPath("~" + strFileName));
            //            attach = new Attachment(Server.MapPath(strFileName));
            //            mailMessage.Attachments.Add(attach);
            //            // attach.Dispose();
            //        }
            //        Files = localpathAttachment.Trim().Split('|');
            //        for (int FCount = 0; FCount < Files.Count(); FCount++)
            //        {

            //            strFileName = Files[FCount].Replace("$$", "\\");
            //            if (strFileName != "")
            //            {
            //                attach = new Attachment(strFileName);
            //                mailMessage.Attachments.Add(attach);
            //            }

            //        }

            //    }
            //    else
            //    {
            //        string[] Files = localpathAttachment.Trim().Split('|');
            //        for (int FCount = 0; FCount < Files.Count(); FCount++)
            //        {

            //            strFileName = Files[FCount].Replace("$$", "\\");
            //            if (strFileName != "")
            //            {
            //                attach = new Attachment(strFileName);
            //                mailMessage.Attachments.Add(attach);
            //            }

            //        }

            //    }
            //    //commemted for direct project measure
            //    //  if (cboProvider.SelectedIndex > 0)
            //    // {
            //    // ToMailId = cboProvider.SelectedItem.Value.Split('-')[1];
            //    // ToAddress.Add(ToMailId);

            //    // }
            //    //  else
            //    //  {
            //    //Regex n = new Regex("(?<user>[^@]+)@(?<host>.+)");
            //    //Regex.IsMatch(txtSentTo.Text,
            //    // "^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*$",
            //    //  RegexOptions.IgnoreCase);

            //    //Match v = n.Match(txtSentTo.Text);
            //    ////Match s = n.Match(txtDirectAddress.Text);
            //    //////if ((txtDirectAddress.Text == string.Empty && rdbtnProvider.Checked) || (!s.Success && rdbtnProvider.Checked))
            //    //////{
            //    //////    hdnType.Value = string.Empty;
            //    //////    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    //////    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('420057');", true);
            //    //////    return;
            //    //////}

            //    //if ((txtSentTo.Text == string.Empty && rdbtnSentTo.Checked) ||( !v.Success && rdbtnSentTo.Checked))
            //    //{
            //    //    hdnType.Value = string.Empty;
            //    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('420057');", true);
            //    //    return;
            //    //}
            //    char[] SplitChar = { ',', ';' };
            //    string[] MailIDs = new string[] { }; ;

            //    MailIDs = txtSentTo.Text.Split(SplitChar);

            //    for (int i = 0; i < MailIDs.Count(); i++)
            //    {
            //        if (MailIDs[i].ToString().Trim() != string.Empty)
            //            ToAddress.Add(MailIDs[i].ToString().Trim());
            //        if (!(rdbtnProvider.Checked))
            //        {
            //            //if (ViewState["MailList"] != null)
            //            //{
            //            //    IList<string> ilstPhyMail = new List<string>();
            //            //    ilstPhyMail = (IList<string>)ViewState["MailList"];
            //            //    if (ilstPhyMail.Count > 0)
            //            //    {
            //            //        var isMailID = (from m in ilstPhyMail where m == MailIDs[i].ToString().Trim() select m).ToList();
            //            //        if (isMailID.Count == 0)
            //            //        {
            //            //            HumanManager objHumanMngr = new HumanManager();
            //            //            Boolean iMailCheck = objHumanMngr.Is_Available_E_Mail(MailIDs[i].ToString().Trim());
            //            //            if (!iMailCheck)
            //            //            {
            //            //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //            //                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1011074');", true);

            //            //                if (attach != null)
            //            //                    attach.Dispose();
            //            //                txtSentTo.Enabled = true;
            //            //                return;
            //            //            }
            //            //        }
            //            //    }
            //            //}
            //        }
            //        else
            //        {
            //            ToAddress.Add(cboProvider.SelectedItem.Value.Split('-')[1].ToString());
            //            break;
            //        }

            //    }
            //    //  }
            //    for (int ids = 0; ToAddress.Count > 0 && ids < ToAddress.Count; ids++)
            //    {
            //        mailMessage.To.Clear();
            //        ToMailId = ToAddress[ids].ToString();
            //        if (!MailValidation(ToMailId))
            //        {
            //            hdnType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295014');", true);
            //            return;
            //        }
            //        try
            //        {
            //            mailMessage.To.Add(new MailAddress(ToMailId));
            //        }
            //        catch
            //        {
            //            hdnType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295014');", true);
            //            return;
            //        }
            //        mailMessage.Subject = txtSubject.Text;
            //        //if (mailMessage.Subject == string.Empty || mailMessage.Subject == null)
            //        //{
            //        //    hdnType.Value = string.Empty;
            //        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295015');", true);
            //        //    return;
            //        //}
            //        mailMessage.Body = txtMessage.Text;
            //        //if (mailMessage.Body == string.Empty || mailMessage.Body == null)
            //        //{
            //        //    hdnType.Value = string.Empty;
            //        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295016');", true);
            //        //    return;
            //        //}
            //        try
            //        {
            //            dd.Send(mailMessage);
            //            // else

            //            //else
            //            //{
            //            //    //
            //            //    string mailID = "capellatest@yahoo.com";
            //            //    string mailPswd = "Acurus1!";
            //            //    string result = string.Empty;
            //            //    using (MailMessage mail = new MailMessage())
            //            //    {
            //            //        try
            //            //        {
            //            //            mail.From = new MailAddress(mailID);
            //            //            mail.To.Add(txtSentTo.Text);
            //            //            mail.Subject = txtSubject.Text;
            //            //            mail.Body = txtMessage.Text; ;
            //            //            mail.IsBodyHtml = true;

            //            //            // mail.Attachments.Add(new Attachment(filepath));
            //            //            if (sYahoomail != null)
            //            //            {
            //            //                if (sYahoomail.Contains("|"))
            //            //                {
            //            //                    string[] Files = sYahoomail.Split('|');
            //            //                    for (int FCount = 0; FCount < Files.Count(); FCount++)
            //            //                    {
            //            //                        if (Files[FCount].IndexOf("Bulk_Acess_SOC_") != -1)
            //            //                            sYahoomail = "atala-capture-download//" + Session.SessionID + "//" + Files[FCount];
            //            //                        else
            //            //                            sYahoomail = Files[FCount].Replace("$$", "\\");
            //            //                        //Attachment attach = new Attachment(Server.MapPath("~" + strFileName));
            //            //                        attach = new Attachment(Server.MapPath(sYahoomail));
            //            //                        mail.Attachments.Add(attach);
            //            //                        // attach.Dispose();

            //            //                    }

            //            //                }
            //            //                else
            //            //                {
            //            //                    if (sYahoomail.IndexOf("Bulk_Acess_SOC_") != -1)
            //            //                    {
            //            //                        if (sYahoomail.Contains("atala-capture-download") != true)
            //            //                            sYahoomail = "atala-capture-download//" + Session.SessionID + "//" + sYahoomail;
            //            //                    }
            //            //                    else
            //            //                        sYahoomail = sYahoomail.Replace("$$", "\\");
            //            //                    // Attachment attach = new Attachment(Server.MapPath("~" + strFileName));


            //            //                    string path = Server.MapPath(sYahoomail);
            //            //                    attach = new Attachment(path);
            //            //                    mail.Attachments.Add(attach);
            //            //                    // attach.Dispose();
            //            //                }

            //            //            }
            //            //            int iPort = 587;
            //            //            SmtpClient smtp = new SmtpClient("smtp.mail.yahoo.com", iPort);
            //            //            smtp.EnableSsl = true;
            //            //            smtp.Credentials = new NetworkCredential(mailID, mailPswd);
            //            //            smtp.Send(mail);
            //            //        }
            //            //        catch (Exception ex)
            //            //        {
            //            //            Console.WriteLine("Mail Not Send Physician Id - {0},And Exception {1}", ex);
            //            //        }

            //            //    }

            //            //    ///



            //            //    ////Unencrypt
            //            //    //MailMessage mail = new MailMessage();
            //            //    //SmtpClient SmtpServer = new SmtpClient("smtp.mail.yahoo.com");
            //            //    //string mailID = System.Configuration.ConfigurationSettings.AppSettings["YahooMailID"].ToString();
            //            //    //string mailPswd = System.Configuration.ConfigurationSettings.AppSettings["YahooMailPassword"].ToString();
            //            //    //mail.From = new MailAddress(mailID);
            //            //    // mail.To.Add(new MailAddress(txtSentTo.Text));
            //            //    // mail.Subject = txtSubject.Text;
            //            //    // mail.Body = txtMessage.Text;
            //            //    //SmtpServer.Port = 587;
            //            //    //SmtpServer.Credentials = new System.Net.NetworkCredential(mailID, mailPswd);
            //            //    //SmtpServer.EnableSsl = true;

            //            //    //if (strFileName != null)
            //            //    //{
            //            //    //    if (strFileName.Contains("|"))
            //            //    //    {
            //            //    //        string[] Files = strFileName.Split('|');
            //            //    //        for (int FCount = 0; FCount < Files.Count(); FCount++)
            //            //    //        {
            //            //    //            if (Files[FCount].IndexOf("Bulk_Acess_SOC_") != -1)
            //            //    //                strFileName = "atala-capture-download//" + Session.SessionID + "//" + Files[FCount];
            //            //    //            else
            //            //    //                strFileName = Files[FCount].Replace("$$", "\\");
            //            //    //            //Attachment attach = new Attachment(Server.MapPath("~" + strFileName));
            //            //    //            attach = new Attachment(Server.MapPath(strFileName));
            //            //    //            mail.Attachments.Add(attach);
            //            //    //            // attach.Dispose();

            //            //    //        }

            //            //    //    }
            //            //    //    else
            //            //    //    {
            //            //    //        if (strFileName.IndexOf("Bulk_Acess_SOC_") != -1)
            //            //    //        {
            //            //    //            if (strFileName.Contains("atala-capture-download") != true)
            //            //    //                strFileName = "atala-capture-download//" + Session.SessionID + "//" + strFileName;
            //            //    //        }
            //            //    //        else
            //            //    //            strFileName = strFileName.Replace("$$", "\\");
            //            //    //        // Attachment attach = new Attachment(Server.MapPath("~" + strFileName));


            //            //    //        string path = Server.MapPath(strFileName);
            //            //    //        attach = new Attachment(path);
            //            //    //        mail.Attachments.Add(attach);
            //            //    //        // attach.Dispose();
            //            //    //    }

            //            //    //}



            //            //    //SmtpServer.Send(mail);

            //            //}
            //        }
            //        catch
            //        {
            //            hdnType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('1007004');", true);
            //            return;
            //        }

            //        btnSend.Enabled = false;
            //        if (hdnHumanID.Value != "")
            //            activity.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
            //        else
            //            activity.Human_ID = 0;
            //        if (Request["Encounter_ID"] != "" && Request["Encounter_ID"] != null)
            //            activity.Encounter_ID = Convert.ToUInt32(Request["Encounter_ID"]);
            //        else
            //            activity.Encounter_ID = 0;
            //        if (Request["FileName"] != null)
            //        {
            //            string FileName = Request["FileName"].ToString();
            //            if (FileName.Contains('|') && FileName.Contains(".zip"))//BugID:49597 
            //            {
            //                activity.Activity_Type = "Transmitted Zip BOTH PDF XML";
            //                activity.Encounter_ID = 0;
            //            }
            //            else if (FileName.Contains('|'))
            //            {
            //                activity.Activity_Type = "Transmitted BOTH PDF XML";
            //            }
            //            else if (FileName.Contains(".xml"))
            //            {
            //                activity.Activity_Type = "Transmitted XML";
            //            }
            //            else if (FileName.Contains(".pdf"))
            //            {
            //                activity.Activity_Type = "Transmitted PDF";
            //            }
            //            else if (FileName.Contains("_PDF_") && FileName.Contains(".zip"))
            //            {
            //                activity.Activity_Type = "Transmitted zip PDF";
            //                activity.Encounter_ID = 0;
            //            }
            //            else if (FileName.Contains("_XML_") && FileName.Contains(".zip"))
            //            {
            //                activity.Activity_Type = "Transmitted zip XML";
            //                activity.Encounter_ID = 0;
            //            }


            //            if (hdnLocalTime.Value != string.Empty)
            //                activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            //            if (rdbtnSentTo.Enabled == true) //BugID:50189
            //                activity.Sent_To = ToMailId;
            //            else if (cboProvider.SelectedIndex > 0)
            //                activity.Sent_To = cboProvider.SelectedItem.Value.Split('-')[1];
            //            //if (cboProvider.SelectedIndex > 0)
            //            //    activity.Sent_To = cboProvider.SelectedItem.Value.Split('-')[1];
            //            //else
            //            //    activity.Sent_To = ToMailId;

            //            if (sIS_Patient_Portal != "YES")
            //                activity.From_Address = cboFrom.Text;
            //            else if (Request["LoginEmailID"] != null && Request["LoginEmailID"].ToString().Trim() != string.Empty)
            //                activity.From_Address = Convert.ToString(Request["LoginEmailID"]);


            //            //if (Request["LoginEmailID"] != null && Request["LoginEmailID"].ToString().Trim() != string.Empty)
            //            //    activity.From_Address = Convert.ToString(Request["LoginEmailID"]);//BugID:49583 
            //            activity.Role = hdnRole.Value;
            //            activity.Subject = txtSubject.Text;
            //            activity.Message = txtMessage.Text;
            //            ArrayList message = ActivitylogMngr.sha2message(txtMessage.Text);
            //            if (message.Count > 0)
            //                activity.Encrypted_Message = message[0].ToString();

            //            activity.Fax_File_Path = sAttachFileName;
            //            ActivityLogList.Add(activity);
            //            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);

            //        }
            //        else if (hdnRole.Value == "Provider")
            //        {

            //            activity.Activity_Type = "Send Message";
            //            if (hdnLocalTime.Value != string.Empty)
            //                activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            //            if (cboProvider.SelectedIndex > 0)
            //                activity.Sent_To = cboProvider.SelectedItem.Value.Split('-')[1];
            //            else
            //                activity.Sent_To = ToMailId;
            //            HumanManager humanMngr = new HumanManager();
            //            Human human = new Human();
            //            human = humanMngr.GetHumanIfDuplicateEMail(ToMailId);
            //            if (human != null)
            //            {
            //                //activity.Human_ID = human.Id;
            //                //IList<Encounter> encList = new List<Encounter>();
            //                //EncounterManager encMngr = new EncounterManager();
            //                //encList = encMngr.GetEncounterUsingHumanID(human.Id);
            //                //if (encList.Count > 0)
            //                //{
            //                //    activity.Encounter_ID = encList[0].Id;
            //                //}
            //            }
            //            else
            //            {
            //                activity.Human_ID = 0;
            //                activity.Encounter_ID = 0;
            //            }


            //            //  activity.From_Address = Convert.ToString(Request["LoginEmailID"]);

            //            if (sIS_Patient_Portal != "YES")
            //                activity.From_Address = cboFrom.Text;
            //            else if (Request["LoginEmailID"] != null && Request["LoginEmailID"].ToString().Trim() != string.Empty)
            //                activity.From_Address = Convert.ToString(Request["LoginEmailID"]);

            //            activity.Subject = txtSubject.Text;
            //            activity.Message = txtMessage.Text;
            //            ArrayList message = ActivitylogMngr.sha2message(txtMessage.Text);
            //            if (message.Count > 0)
            //                activity.Encrypted_Message = message[0].ToString();
            //            activity.Role = hdnRole.Value;
            //            activity.Fax_File_Path = sAttachFileName;
            //            ActivityLogList.Add(activity);
            //            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            //        }
            //        else
            //        {

            //            if (Request["Scn_Name"] == "Reminder")
            //            {
            //                string[] lstHuman = Request["lstHuman"].Split('|'); ;
            //                activity.From_Address = Convert.ToString(Session["fromMail"]);
            //                activity.Activity_Type = "EMail";
            //                //  HumanManager humanMngr = new HumanManager();
            //                // Human human = new Human();
            //                // human = humanMngr.GetHumanIfDuplicateEMail(ToMailId);
            //                // if (human != null)
            //                // {
            //                //  activity.Human_ID = human.Id;
            //                // }
            //                //else
            //                //{ activity.Human_ID = 0; }
            //                try
            //                {
            //                    activity.Human_ID = Convert.ToUInt64(lstHuman[ids]);
            //                }
            //                catch
            //                {
            //                }

            //                IList<Encounter> encList = new List<Encounter>();
            //                EncounterManager encMngr = new EncounterManager();
            //                encList = encMngr.GetEncounterUsingHumanID(Convert.ToUInt64(lstHuman[ids]));
            //                if (encList.Count > 0)
            //                {
            //                    activity.Encounter_ID = encList[0].Id;
            //                }
            //            }
            //            else
            //            {
            //                activity.From_Address = Convert.ToString(Request["LoginEmailID"]);
            //                activity.Activity_Type = "Send Message";
            //            }

            //            if (hdnLocalTime.Value != string.Empty)
            //                activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            //            if (cboProvider.SelectedIndex > 0)
            //                activity.Sent_To = cboProvider.SelectedItem.Value.Split('-')[1];
            //            else
            //                activity.Sent_To = ToMailId;

            //            activity.Subject = txtSubject.Text;
            //            activity.Message = txtMessage.Text;
            //            ArrayList message = ActivitylogMngr.sha2message(txtMessage.Text);
            //            if (message.Count > 0)
            //                activity.Encrypted_Message = message[0].ToString();
            //            activity.Role = hdnRole.Value;
            //            activity.Fax_File_Path = sAttachFileName;
            //            ActivityLogList.Add(activity);
            //            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
            //        }
            //    }

            //    if (hdnRole.Value.ToUpper() == "PATIENT" || hdnRole.Value.ToUpper() == "REPRESENTATIVE")
            //    {
            //        if (sForCancel == "Yes")
            //        {
            //            hdnType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ShowErrorMessageList('9093039');close();", true);
            //        }
            //        else if (sForCancelInsend == "Yes")
            //        {
            //            hdnMessageType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ShowErrorMessageList('9093039');closeForSend();", true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "ShowErrorMessageList('9093039');", true);
            //        }
            //    }
            //    else
            //    {
            //        if (sForCancel == "Yes")
            //        {
            //            hdnType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295013');close();", true);
            //        }
            //        else if (sForCancelInsend == "Yes")
            //        {
            //            hdnMessageType.Value = string.Empty;
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295013');closeForSend();", true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('295013');", true);
            //        }
            //    }
            //    lblAttachment.Text = "";
            //    lblAttachment1.Text = "";//BugID:50313
            //    rdbtnProvider.Checked = true;
            //}

            //}
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "DisplayErrorMessage('295013');closeForSend();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            txtMessage.Text = "";
            txtSentTo.Text = "";
            txtSubject.Text = "";
            rdbtnSentTo.Checked = false;
            rdbtnProvider.Checked = false;
            //rbtDirectAddress.Checked = false;
            //txtDirectAddress.Text = "";
            cboProvider.SelectedIndex = 0;
            cboProvider.Enabled = false;
            //if (strFileName != null && strFileName != "")
            //    File.Delete(Server.MapPath(strFileName));
            if (attach != null)
                attach.Dispose();



        }

        protected void rdbtnProvider_CheckedChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = true;
            txtSentTo.Enabled = false;
            cboProvider.Enabled = true;
            txtSentTo.Text = "";
            cboProvider.SelectedIndex = 0;
            txtDirectAddress.Enabled = false;
            txtDirectAddress.Text = "";
        }

        protected void rdbtnSentTo_CheckedChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = true;
            txtSentTo.Enabled = true;
            cboProvider.Enabled = false;
            cboProvider.SelectedIndex = 0;
            txtDirectAddress.Text = "";
            txtDirectAddress.Enabled = false;
            //txtDirectAddress.Text = "";
        }

        protected void cboProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhysicianManager objPhysicianMngr = new PhysicianManager();
            IList<PhysicianLibrary> ilstLib = new List<PhysicianLibrary>();
            if (cboProvider.Text == "Others")
            {
                txtDirectAddress.Enabled = true;
                txtDirectAddress.Text = "";
                txtDirectAddress.Focus();
            }
            else
            {
                txtDirectAddress.Enabled = false;
                txtDirectAddress.Text = "";

            }
            btnSend.Enabled = true;
        }

        //protected void rdbtnDirectAddress_CheckedChanged(object sender, EventArgs e)
        //{
        //    btnSend.Enabled = true;
        //    txtSentTo.Enabled = false;
        //    cboProvider.Enabled = false;
        //    cboProvider.SelectedIndex = 0;
        //    txtDirectAddress.Enabled = true;
        //}

        private void SplitMessage()
        {
            if (Session["Content"] != null)
            {
                string Message = Session["Content"].ToString();
                string[] SplitMessage = Regex.Split(Message, "\r\n");
                for (int i = 0; i < SplitMessage.Count(); i++)
                {
                    if (SplitMessage[i].Contains("From :"))
                    {
                        txtSentTo.Text = Regex.Split(SplitMessage[i], ": ")[1].ToString().Trim();
                        break;
                    }

                }
                for (int i = 0; i < SplitMessage.Count(); i++)
                {

                    if (SplitMessage[i].Contains("Re :") || SplitMessage[i].Contains("Re:"))
                    {
                        txtSubject.Text = "Re :" + SplitMessage[i].Split(':')[2].ToString().Trim();
                        break;
                    }
                    else if (SplitMessage[i].Contains("Subject :"))
                    {
                        //txtSubject.Text = "Re :" +SplitMessage[i].Split(':')[1].ToString().Trim();
                        txtSubject.Text = SplitMessage[i].Replace("Subject :", "Re :").ToString().Trim();
                        break;
                    }


                }

            }
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
        public void ComposeEmail(string sRecipient, List<string> sAttachmentPath, string sContent, string sSub, int port, string password, string toaddress)
        {
            string sForCancel = hdnType.Value;
            string sForcancelInSend = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            hdnType.Value = string.Empty;
            PhiMailConnector pcConnection;

            try
            {
                PhiMailConnector.SetTrustAnchor((System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"].ToString()));
                PhiMailConnector.SetCheckRevocation(false);
                // pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["phiMailPortNo"]));
                pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), port);
            }
            catch (Exception ex)
            {
                Response.Write("Exception Caught @" + ex.Message + "\n" + ex.Source + "\n" + ex.Data);
                return;
            }
            try
            {
                bool send = true;
                // pcConnection.AuthenticateUser(System.Configuration.ConfigurationSettings.AppSettings["phiMailUsername"].ToString(), System.Configuration.ConfigurationSettings.AppSettings["phiMailPassword"].ToString());
                pcConnection.AuthenticateUser(sRecipient.ToString(), password);

                if (send)
                {
                    try
                    {
                        //foreach (string rec_adderess in sRecipient)
                        //{
                        // pcConnection.AddRecipient(toaddress);
                        //}

                        string[] toreceipient = sRecipient.Split(';');
                        foreach (string rec_adderess in toreceipient)
                        {
                            pcConnection.AddRecipient(rec_adderess);
                        }
                    }
                    catch 
                    {
                        //throw ex;
                       // string test = "";
                    }
                    pcConnection.SetSubject(sSub);
                    if (sContent != string.Empty)
                    {
                        pcConnection.AddText(sContent);
                    }
                    foreach (string file in sAttachmentPath) // To Attach All The Files With This MailMessage
                    {
                        // string FinalPath = file.Remove(0, 1);
                        if (file.Contains(".xml"))
                        {
                            FileInfo filename = new FileInfo(file); // To Remove The // in front Of File Path to avoid production issue
                            pcConnection.AddCDA(File.ReadAllText(file), filename.Name);
                        }
                        else if (file.Contains(".pdf"))
                        {
                            FileInfo filename = new FileInfo(file);
                            pcConnection.AddRaw(File.ReadAllBytes(file), filename.Name);
                        }
                    }
                    pcConnection.SetDeliveryNotification(true);
                    List<PhiMailConnector.SendResult> sendRes = pcConnection.Send();
                    if (sendRes != null && sendRes.Count > 0 && sendRes[0].Succeeded)
                    {
                        //ActivityLogEntry(sRecipient);
                        if (sForCancel == "Yes")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');close();", true);
                        }
                        else if (sForcancelInSend == "Yes")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');closeForSend();", true);
                        }
                        else
                        {
                            if (txtDirectAddress.Text == "")
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');", true);
                            else
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + txtDirectAddress.Text + "');", true);

                        }
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMsg", "ShowSuccess(" + sendRes[0].Recipient + ");", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMsg", "ShowFailure(" + sendRes[0].Recipient + "Description :" + sendRes[0].ErrorText.ToString() + ");", true);
                        pcConnection.Clear();
                    }
                }
            }
            catch 
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMsg", "alert('Please enter Valid Mail Address.');", true);
                pcConnection.Clear();
                flag = 1;

            }
            pcConnection.Close();
        }
        private void ActivityLogEntry(IList<string> sRec)
        {
            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            ActivityLog activity = new ActivityLog();
            string FileName = string.Empty;
            if (hdnHumanID.Value != "")
                activity.Human_ID = Convert.ToUInt64(hdnHumanID.Value);
            else
                activity.Human_ID = 0;
            if (Request["Encounter_ID"] != "" && Request["Encounter_ID"] != null)
                activity.Encounter_ID = Convert.ToUInt32(Request["Encounter_ID"]);


            if (Request["FileName"] != null)
            {
                FileName = Request["FileName"].ToString();
            }
            if (FileName.Contains('|'))
            {
                activity.Activity_Type = "Transmitted BOTH PDF XML";
            }
            else if (FileName.Contains(".pdf"))
            {
                activity.Activity_Type = "Transmitted PDF";
            }
            else if (FileName.Contains(".xml"))
            {
                activity.Activity_Type = "Transmitted XML";
            }
            else      //added for bug_id 29418
            {
                activity.Activity_Type = "Send";
                activity.From_Address = Convert.ToString(Request["LoginEmailID"]);
            }
            if (hdnLocalTime.Value != string.Empty)
                activity.Activity_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
            if (cboProvider.SelectedIndex > 0)
            {
                if (cboProvider.SelectedItem.Value.Trim() != "")
                {
                    activity.Sent_To = cboProvider.SelectedItem.Value.Split('-')[1];
                }
                else
                {
                    activity.Sent_To = sRec[0];
                }
            }
            else
                activity.Sent_To = sRec[0];
            activity.Role = hdnRole.Value;
            activity.Subject = txtSubject.Text;
            activity.Message = txtMessage.Text;
            ArrayList message = ActivitylogMngr.sha2message(txtMessage.Text);
            if (message.Count > 0)
                activity.Encrypted_Message = message[0].ToString();

            // activity.Fax_File_Name = sAttachFileName;
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);
        }

        protected void cboFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            rdbtnProvider.Enabled = true;
        }


        public void ComposeEmailPatientPortal(IList<string> sRecipient, List<string> sAttachmentPath, string sContent, string sSub)
        {
            string sForCancel = hdnType.Value;
            string sForcancelInSend = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            hdnType.Value = string.Empty;
            PhiMailConnector pcConnection;

            try
            {
                PhiMailConnector.SetTrustAnchor((System.Configuration.ConfigurationSettings.AppSettings["phiMailCertficatepath"].ToString()));
                PhiMailConnector.SetCheckRevocation(false);
                pcConnection = new PhiMailConnector(System.Configuration.ConfigurationSettings.AppSettings["phiMailServer"].ToString(), Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["phiMailPortNo"]));
            }
            catch (Exception ex)
            {
                Response.Write("Exception Caught @" + ex.Message + "\n" + ex.Source + "\n" + ex.Data);
                return;
            }
            try
            {
                bool send = true;
                pcConnection.AuthenticateUser(System.Configuration.ConfigurationSettings.AppSettings["phiMailUsername"].ToString(), System.Configuration.ConfigurationSettings.AppSettings["phiMailPassword"].ToString());
                if (send)
                {
                    try
                    {
                        foreach (string rec_adderess in sRecipient)
                        {
                            pcConnection.AddRecipient(rec_adderess);
                        }
                    }
                    catch 
                    {
                        //throw ex;
                        //string test = "";
                    }
                    pcConnection.SetSubject(sSub);
                    if (sContent != string.Empty)
                    {
                        pcConnection.AddText(sContent);
                    }
                    foreach (string file in sAttachmentPath) // To Attach All The Files With This MailMessage
                    {
                        // string FinalPath = file.Remove(0, 1);
                        if (file.Contains(".xml"))
                        {
                            FileInfo filename = new FileInfo(file); // To Remove The // in front Of File Path to avoid production issue
                            pcConnection.AddCDA(File.ReadAllText(file), filename.Name);
                        }
                        else if (file.Contains(".pdf"))
                        {
                            FileInfo filename = new FileInfo(file);
                            pcConnection.AddRaw(File.ReadAllBytes(file), filename.Name);
                        }
                        else
                        {
                            FileInfo filename = new FileInfo(file);
                            pcConnection.AddRaw(File.ReadAllBytes(file), filename.Name);
                        }
                    }
                    pcConnection.SetDeliveryNotification(true);
                    List<PhiMailConnector.SendResult> sendRes = pcConnection.Send();
                    if (sendRes != null && sendRes.Count > 0 && sendRes[0].Succeeded)
                    {
                        //ActivityLogEntry(sRecipient);
                        if (sForCancel == "Yes")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');close();", true);
                        }
                        else if (sForcancelInSend == "Yes")
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');closeForSend();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Orders", "DisplayErrorMessage('1007007','','" + sendRes[0].Recipient + "');", true);
                        }
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMsg", "ShowSuccess(" + sendRes[0].Recipient + ");", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMsg", "ShowFailure(" + sendRes[0].Recipient + "Description :" + sendRes[0].ErrorText.ToString() + ");", true);
                        pcConnection.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Exception Caught @" + ex.Message + "\n" + ex.Source + "\n" + ex.Data);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowMsg", "alert('Please enter Valid Mail Address.');", true);
                pcConnection.Clear();
                flag = 1;

            }
            pcConnection.Close();
        }

    }
}
