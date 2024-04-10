using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using System.Xml.Linq;
using Telerik.Web;
using Telerik.Web.UI;
using Acurus.Capella.UI;
using Telerik.Web.UI.Upload;
using System.IO;
using System.Web.Services;
using Newtonsoft.Json;
using System.Configuration;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Data;
using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;

namespace Acurus.Capella.UI
{
    public partial class frmEFax : System.Web.UI.Page
    {
        ActivityLog objactivitylog = new ActivityLog();
        ActivityLogManager objActivityMngr = new ActivityLogManager();
        string spath = string.Empty;
        string sFilePath = "";
        //Cap - 1918
        string sMenuLevelEFax = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                if (Request["DMEOrder"] != null)
                {
                    spath = Path.GetFileName(Request["DMEOrder"].ToString());
                    hdnAttachfile.Value = spath;
                    lblattach.InnerText = spath;
                    sFilePath = Request["DMEOrder"].ToString();
                }
                if (Request["ProgressNotes"] != null)
                {
                    spath = Path.GetFileName(Request["ProgressNotes"]);

                    lblattach.InnerText = spath;
                    hdnAttachfile.Value = lblattach.InnerText;
                    sFilePath = Request["ProgressNotes"].ToString();
                }
                if (Request["Result"] != null)
                {
                    spath = Request["Result"].ToString().Split('|')[0];
                    lblattach.InnerText = Path.GetFileName(spath);
                    sFilePath = Request["Result"].ToString().Split('|')[0];
                    hdnAttachfile.Value = lblattach.InnerText;
                }
                //Cap - 1414, 1415, 1449
                //if (Request["RefProvider"] != null && Request["RefProvider"] != string.Empty)
                if (Request["RefProvider"] != null && Request["RefProvider"] != string.Empty && Request["sIsConsultation"]!=null && Request["sIsConsultation"] == "Y")
                {
                    //Jira CAP-1358
                    //txtRecName.Value = Request["RefProvider"].ToString();
                    txtRecName.Value = Request["RefProvider"].ToString().Replace("~$~", "#");
                    var RefProvider = txtRecName.Value.Split('|');
                    for (int i = 0; i < RefProvider.Count(); i++)
                    {
                        if (RefProvider[i].ToString().Contains("Fax No:") || RefProvider[i].ToString().Contains("FAX:"))
                        {
                            string Fax = Regex.Replace(RefProvider[i].ToString().Split(':')[1], @"\s", "");
                            msktxtRecipientFax.Value = Fax;
                            break;
                        }
                    }

                    // Commented due to Jira CAP-1415
                    ////Jira CAP-1387
                    //string sEmail = txtRecName.Value;
                    //if (sEmail.Contains("Email"))
                    //{
                    //    sEmail = sEmail.Substring(sEmail.IndexOf("Email"));
                    //    if (sEmail != string.Empty && sEmail.Split('|')[0].Split(':').Length >= 2 && sEmail.Split('|')[0].Split(':')[1] != "" && sEmail.Split('|')[0].Split(':')[1] != " ")
                    //    {
                    //        txtRecipientmail.Value = sEmail.Split('|')[0].Split(':')[1];
                    //    }
                    //}


                }
                //Cap - 1918
                if (Request["Mode"] != null)
                {
                    sMenuLevelEFax = Request["Mode"].ToString();
                }

                    CreateEmptyHeader();
                hdnGroupID.Value = objActivityMngr.GetGroupID().ToString();
                hdnfilePath.Value = sFilePath;
            }
            //Cap - 1918
            if (Request["Mode"] != null)
            {
                sMenuLevelEFax = Request["Mode"].ToString();
            }

            //if (!IsPostBack)
            //{
            //    txtSenderName.Text = ClientSession.UserName;
            //    txtSenderCompany.Text = ClientSession.FacilityName;
            //    //txtSenderMaskFax.Text = "9092973242";
            //    XDocument xmlFacility = XDocument.Load(Server.MapPath(@"ConfigXML\Facility_Library.xml"));
            //    IEnumerable<XElement> xmlFac = xmlFacility.Element("FacilityList")
            //        .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);
            //    if (xmlFac != null && xmlFac.Count() > 0)
            //    {
            //        //balaji
            //        //if (xmlFac.Attributes("Facility_Fax") != null)
            //        //     txtSenderMaskFax.Text = xmlFac.Attributes("Facility_Fax").First().Value.ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            //    }
            //    //lnkActiveLog.Attributes.Add("onclick", "return ActivityHistoryClick();");
            //}
        }
        public void CreateEmptyHeader()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Company", typeof(string));
            dt.Columns.Add("Fax", typeof(string));
            dt.Columns.Add("EMail", typeof(string));
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            //grdEFax.DataSource = dt;
            //grdEFax.DataBind();
            //grdEFax.Rows[0].Visible = false;
        }
        //Cap - 1414, 1415, 1449
        //public static string GetFaxload()
        [WebMethod(EnableSession = true)]
        public static string GetFaxload(string sIsConsultation)
        {
            string spath = string.Empty;
            string lblattach = string.Empty, SenderMaskFax = string.Empty;
            string PhyId = string.Empty;
            string PhyFax = string.Empty;
            string phyEmail = string.Empty;
            string PersonName = string.Empty;
            //Cap - 1414, 1415, 1449
            string sPatientName = string.Empty;

            EncounterManager EncManager = new EncounterManager();
            Encounter objEncounter = null;
            HumanManager objHumanMngr = new HumanManager();

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            if (ClientSession.HumanId != 0)
            {
                Human objhuman = new Human();
                IList<Human> HumanList = objHumanMngr.GetPatientDetailsUsingPatientInformattion(ClientSession.HumanId);
                if (HumanList != null && HumanList.Count > 0)
                {
                    objhuman = HumanList[0];
                }
                if (objhuman != null)
                {
                    sPatientName = objhuman.First_Name +" "+ objhuman.Last_Name;
                }
            }
            XDocument xmluser = XDocument.Load(HttpContext.Current.Server.MapPath(@"ConfigXML\User.xml"));
            if (sIsConsultation == "Y")
            {

                objEncounter = EncManager.GetEncounterByEncounterIDArchive(ClientSession.EncounterId);
                if (objEncounter.Id != 0)
                {
                    PhyId = objEncounter.Encounter_Provider_ID.ToString();
                }
                IEnumerable<XElement> xmluserid = xmluser.Element("UserList")
                    .Elements("User").Where(aa => aa.Attribute("Physician_Library_ID").Value.ToString() == PhyId);
                if (xmluserid != null && xmluserid.Count() > 0)
                {
                    if (xmluserid.Attributes("person_name") != null)
                        PersonName = xmluserid.Attributes("person_name").First().Value.ToString();
                }
            }
            else
            {
                IEnumerable<XElement> xmluserid = xmluser.Element("UserList")
                    .Elements("User").Where(aa => aa.Attribute("User_Name").Value.ToString() == ClientSession.UserName);
                if (xmluserid != null && xmluserid.Count() > 0)
                {
                    if (xmluserid.Attributes("Physician_Library_ID") != null)
                        PhyId = xmluserid.Attributes("Physician_Library_ID").First().Value.ToString();
                    if (xmluserid.Attributes("person_name") != null)
                        PersonName = xmluserid.Attributes("person_name").First().Value.ToString();
                }
            }
            if (PhyId != "" && PhyId != "0")
            {
                //Cap - 1548
                if (sIsConsultation != "Y")
                {
                    XmlDocument xmldoc1 = new XmlDocument();
                    string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                    if (File.Exists(strXmlFilePath1) == true)
                    {
                        //logger.Debug("Reading PhysicianAddressDetails.xml");
                        xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + PhyId);
                        if (nodeMatchingPhysicianAddress != null)
                        {

                            PhyFax = nodeMatchingPhysicianAddress.Attributes["Physician_Fax"].Value.ToString();
                            phyEmail = nodeMatchingPhysicianAddress.Attributes["Physician_EMail"].Value.ToString();
                            //logger.Debug("XML tag '/PhysicianAddress/p" + physician_id + "' found");
                        }
                        //else
                        //logger.Debug("XML tag '/PhysicianAddress/p" + physician_id + "' not found");
                    }
                }
                if (PhyFax.Trim() != "")
                {
                    SenderMaskFax = PhyFax.ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                }
                else
                {
                    XDocument xmlFacility = XDocument.Load(HttpContext.Current.Server.MapPath(@"ConfigXML\Facility_Library.xml"));
                    IEnumerable<XElement> xmlFac;
                    if (objEncounter != null)
                    {
                        xmlFac = xmlFacility.Element("FacilityList")
                          .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == objEncounter.Facility_Name);

                    }
                    else
                    {
                        xmlFac = xmlFacility.Element("FacilityList")
                          .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);

                    }

                    if (xmlFac != null && xmlFac.Count() > 0)
                    {
                        if (xmlFac.Attributes("Facility_Fax") != null)
                            SenderMaskFax = xmlFac.Attributes("Facility_Fax").First().Value.ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                    }
                }
            }

            else
            {
                if (sIsConsultation != "Y")
                {
                    XDocument xmlFacility = XDocument.Load(HttpContext.Current.Server.MapPath(@"ConfigXML\Facility_Library.xml"));
                    IEnumerable<XElement> xmlFac = xmlFacility.Element("FacilityList")
                        .Elements("Facility").Where(aa => aa.Attribute("Name").Value.ToString() == ClientSession.FacilityName);
                    if (xmlFac != null && xmlFac.Count() > 0)
                    {
                        if (xmlFac.Attributes("Facility_Fax") != null)
                            SenderMaskFax = xmlFac.Attributes("Facility_Fax").First().Value.ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                    }
                }
            }
            IList<StaticLookup> LookUpList = new List<StaticLookup>();
            StaticLookupManager StaticLookupMngr = new StaticLookupManager();
            //Jira CAP-1416
            //string[] sfield = new string[] { "Fax_Priority", "Fax_Cover Page", "SIGNATURE" };
            string[] sfield = new string[] { "Fax_Priority", "Fax_Cover Page", "EFAX_SIGNATURE" };
            if (sIsConsultation == "Y")
            {
                sfield = new string[] { "Fax_Priority", "Fax_Cover Page", "EFAX_SIGNATURE_CONSULTATION" };
            }
                LookUpList = StaticLookupMngr.getStaticLookupByFieldName(sfield);
            string Nameoftheuser = string.Empty;
            string FacilityAddress = string.Empty;
            string FacilityPhoneNumber = string.Empty;
            string FacilityFaxNumber = string.Empty;
            string sFacilityName = string.Empty;
            FacilityManager fmg = new FacilityManager();
            IList<FacilityLibrary> lsitFacilitylib = new List<FacilityLibrary>(); //
            if (objEncounter != null)
            {
                lsitFacilitylib = fmg.GetFacilityByFacilityname(objEncounter.Facility_Name);
            }
            else
            {
                lsitFacilitylib = fmg.GetFacilityByFacilityname(ClientSession.FacilityName);
            }


            if (ClientSession.UserRole == "Physician")
            {
                PhysicianManager phymg = new PhysicianManager();
                IList<PhysicianLibrary> listPhy = new List<PhysicianLibrary>();

                if (objEncounter != null)
                {
                    listPhy = phymg.Get_PhysicianList(Convert.ToString(objEncounter.Encounter_Provider_ID));
                }
                else
                {
                    listPhy = phymg.Get_PhysicianList(Convert.ToString(ClientSession.PhysicianId));
                }


                if (listPhy != null && listPhy.Count > 0)
                {
                    Nameoftheuser = listPhy[0].PhyPrefix + " " + listPhy[0].PhyFirstName + " " + listPhy[0].PhyMiddleName + " " + listPhy[0].PhyLastName + " " + listPhy[0].PhySuffix;
                    if (listPhy[0].PhyTelephone != "")
                        FacilityPhoneNumber = listPhy[0].PhyTelephone;
                    else
                        FacilityPhoneNumber = lsitFacilitylib[0].Fac_Telephone;
                    if (listPhy[0].PhyFax != "")
                        FacilityFaxNumber = listPhy[0].PhyFax;
                    else
                        FacilityFaxNumber = lsitFacilitylib[0].Fac_Fax;
                }

                if (lsitFacilitylib != null && lsitFacilitylib.Count > 0)
                {
                    FacilityAddress = lsitFacilitylib[0].Fac_Address1;
                    sFacilityName = lsitFacilitylib[0].Fac_Name;
                }
            }
            else
            {
                //Nameoftheuser
                Nameoftheuser = PersonName;
                if (lsitFacilitylib != null && lsitFacilitylib.Count > 0)
                {
                    sFacilityName = lsitFacilitylib[0].Fac_Name;
                    FacilityAddress = lsitFacilitylib[0].Fac_Address1;
                    FacilityPhoneNumber = lsitFacilitylib[0].Fac_Telephone;
                    FacilityFaxNumber = lsitFacilitylib[0].Fac_Fax;
                }

            }
            var result = new { SenderName = ClientSession.UserName, SenderCompany = sFacilityName, txtSenderMaskFax = SenderMaskFax, LookUpList = LookUpList, Nameoftheuser = Nameoftheuser, FaciltyName = ConfigurationManager.AppSettings["ClientName"].ToString(), FacilityAddress = FacilityAddress, FacilityPhoneNumber = FacilityPhoneNumber, FacilityFaxNumber = FacilityFaxNumber, Email = phyEmail, sPatientName = sPatientName };
            return JsonConvert.SerializeObject(result);
        }

            protected void btnSendfax_Click(object sender, EventArgs e)
            {
                Document doc = new Document();
            try
            {
                string sAttachFileName = string.Empty;



                string ftpUserID = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                string ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];
                string ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];

                /*Old code*/
                //string UNCAuthPath = System.Configuration.ConfigurationSettings.AppSettings["UNCAuthPathFax"];
                //string UNCPath = System.Configuration.ConfigurationSettings.AppSettings["UNCPathFax"];
                //string ftpIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIPFax"];
                //string userName = System.Configuration.ConfigurationSettings.AppSettings["UserNameFax"];
                //string password = System.Configuration.ConfigurationSettings.AppSettings["PasswordFax"];
                //string domain = System.Configuration.ConfigurationSettings.AppSettings["DomainFax"];
                //FAXCOMEXLib.FaxServer fs = new FAXCOMEXLib.FaxServer();
                //fs.Connect(Environment.MachineName);
                //FAXCOMEXLib.FaxDocument fd = new FAXCOMEXLib.FaxDocument();
                string sFTPAddress = string.Empty;
                if (UploadImage.UploadedFiles.Count > 0 || hdnfilePath.Value != string.Empty)
                {
                    //Image Server Uploading

                    // string sDestinationFTPPath = "EFAX\\" + ClientSession.FacilityName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
                    string sFacilityPath = ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + System.Configuration.ConfigurationSettings.AppSettings["Faxpathoutput"] + DateTime.Now.ToString("yyyyMMdd") + @"\";
                    string sDestinationFTPPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["ftpFaxpath"], sFacilityPath);
                    FTPImageProcess ftpImageProcess = new FTPImageProcess();
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/"));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }

                    int i = 1;
                    // if (ftpImageProcess.CreateDirectoryFAX(UNCAuthPath, UNCPath, ftpIP, userName, password, domain, sDestinationFTPPath))
                    //if (ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword))
                    bool bCreateDirectory = ftpImageProcess.CreateDirectory(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, out string sCheckFileNotFoundException);
                    if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                        return;
                    }
                    if (bCreateDirectory)
                    {
                        if (UploadImage.UploadedFiles.Count > 0 || Request["Result"] != null)
                        {
                            foreach (UploadedFile file in UploadImage.UploadedFiles)
                            {
                                string SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileName(file.FileName));
                                if (File.Exists(SelectedFilePath))
                                {
                                    File.Delete(SelectedFilePath);
                                }
                                file.SaveAs(SelectedFilePath);
                                string pdffilepath = "";

                                if (Path.GetExtension(SelectedFilePath).ToUpper() == ".PNG" || Path.GetExtension(SelectedFilePath).ToUpper() == ".GIF" || Path.GetExtension(SelectedFilePath).ToUpper() == ".JPG" || Path.GetExtension(SelectedFilePath).ToUpper() == ".JPEG")
                                {
                                    pdffilepath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + "image.pdf");

                                    //Create image in PDF For Bug id: 64259 //Ref:link https://www.mikesdotnetting.com/article/87/itextsharp-working-with-images
                                    doc = new Document();
                                    PdfWriter.GetInstance(doc, new FileStream(pdffilepath, FileMode.Create));
                                    doc.Open();
                                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(SelectedFilePath);
                                    img.ScaleToFit(500f, 500f);
                                    doc.Add(img);
                                    doc.Close();

                                    //using (var fileStream = new FileStream(pdffilepath, FileMode.Create, FileAccess.Write))
                                    //{
                                    //    var document1 = new Document(new Rectangle(625, 975));

                                    //    float X = 80f, Y = 20f;
                                    //    Rectangle pageSize = new Rectangle(625, 975);
                                    //    var writerpdf = PdfWriter.GetInstance(document1, fileStream);
                                    //   // string path = (SelectedFilePath + "/" + Path.GetFileName(spath));
                                    //    document1.Open();
                                    //    document1.NewPage();
                                    //    var con = writerpdf.DirectContent;
                                    //    con.BeginText();
                                    //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(SelectedFilePath);
                                    //    logo.ScaleAbsolute(500, 500);
                                    //    logo.SetAbsolutePosition(pageSize.GetLeft(10), 0);
                                    //    con.AddImage(logo);
                                    //    con.EndText();
                                    //    document1.Close();
                                    //    writerpdf.Close();

                                    //}
                                }
                                else
                                {
                                    pdffilepath = SelectedFilePath;

                                }
                                // string sStoringFormat = "EFAX_" + ClientSession.HumanId.ToString() + "_" + txtSenderName.Value.Replace("'", "") + "_" + txtRecName.Value.Split('|')[0].Replace("'", "") + "_" + Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(file.FileName);
                                //  string sStoringFormat = "EFAX_" + ClientSession.HumanId.ToString() + "_"  + Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(file.FileName);
                                string sStoringFormat = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + i.ToString() + Path.GetExtension(pdffilepath);
                                i++;
                                //sFTPAddress = ftpImageProcess.UploadToImageServerFAX(UNCAuthPath, UNCPath, ftpIP, userName, password, domain, sDestinationFTPPath, SelectedFilePath, sStoringFormat);
                                sFTPAddress = ftpImageProcess.UploadToImageServer(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, pdffilepath, sStoringFormat, out string sCheckFileNotFoundExceptions);
                                if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                    return;
                                }
                                // string sFilePath = sFacilityPath + sStoringFormat;
                                if (sAttachFileName == string.Empty)
                                    sAttachFileName = sFTPAddress;
                                else
                                    sAttachFileName += " | " + sFTPAddress;
                                // var fax = FaxResource.Create(
                                //    from: txtSenderName.Text,
                                //    to: txtSenderMaskFax.Text.Replace("-", ""),
                                //    mediaUrl: new Uri("https://www.twilio.com/docs/documents/25/justthefaxmaam.pdf")
                                //);


                                //fd = new FAXCOMEXLib.FaxDocument();
                                //fd.CoverPageType = FAXCOMEXLib.FAX_COVERPAGE_TYPE_ENUM.fcptSERVER;
                                //fd.CoverPage = "generic";// "C:\\Users\\administrator\\Desktop\\123.cov";
                                //// fd.AttachFaxToReceipt = false;

                                ////fd.Note = "Here is the info you requested " + i.ToString() + " Page";
                                //fd.Body = Server.MapPath("atala-capture-upload/" + Session.SessionID + Path.Combine(sDestinationFTPPath, sStoringFormat));
                                //fd.ReceiptAddress = txtRecipientcompany.Text;

                                //fd.Subject = "Today's fax" + txtCoverpage.txtDLC.Text;

                                //fd.Sender.Name = txtSenderName.Text;

                                //fd.Sender.Company = txtSenderCompany.Text;
                                //fd.Sender.Email = txtSenderEmail.Text;
                                //fd.Sender.FaxNumber = txtSenderMaskFax.Text.Replace("-", "");
                                //fd.Sender.SaveDefaultSender();
                                //fd.Recipients.Add(txtSenderMaskFax.Text.Replace("-", ""), txtSenderName.Text);

                                ////This JobID[0] can be used to store the JobID of the fax job for later reference.
                                //string[] JobID = (string[])fd.ConnectedSubmit(fs);
                            }
                        }
                        if (hdnfilePath.Value != string.Empty)
                        {
                            string SelectedFilePath = string.Empty;
                            if (Request["Result"] != null && hdnfilePath.Value.ToUpper().IndexOf("FTP") >= 0)
                            {

                                string localPathre = System.Configuration.ConfigurationSettings.AppSettings["LocalPath"];
                                string ftpServerIPre = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
                                string ftpUserNamere = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
                                string ftpPasswordre = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];

                                //SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd"));
                                dir = new DirectoryInfo(Server.MapPath("atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/"));
                                if (!dir.Exists)
                                {
                                    dir.Create();
                                }
                                SelectedFilePath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileName(Request["Result"].ToString().Split('|')[0]));
                                string pdffilepath = string.Empty;


                                if (File.Exists(SelectedFilePath))
                                {
                                    File.Delete(SelectedFilePath);
                                }
                                string HumanId = new DirectoryInfo(hdnfilePath.Value.Replace("ftp:", "")).Parent.Name;
                                string sLocalPath = Path.GetDirectoryName(SelectedFilePath);

                                // ftpImageProcess.DownloadFromImageServer(ClientSession.HumanId.ToString(), ftpServerIPre, ftpUserNamere, ftpPasswordre, Path.GetFileName(hdnfilePath.Value), SelectedFilePath);
                                //ftpImageProcess.DownloadFromImageServer(HumanId, ftpServerIPre, ftpUserNamere, ftpPasswordre, Path.GetFileName(hdnfilePath.Value), SelectedFilePath);

                                ftpImageProcess.DownloadFromImageServer(HumanId, ftpServerIPre, ftpUserNamere, ftpPasswordre, Path.GetFileName(hdnfilePath.Value), sLocalPath, out string sCheckFileNotFoundExc);
                                if (sCheckFileNotFoundExc != "" && sCheckFileNotFoundExc.Contains("CheckFileNotFoundException"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExc.Split('~')[1] + "\");", true);
                                    return;
                                }
                                if (Path.GetExtension(hdnfilePath.Value).ToUpper() == ".PNG" || Path.GetExtension(hdnfilePath.Value).ToUpper() == ".GIF" || Path.GetExtension(hdnfilePath.Value).ToUpper() == ".JPG" || Path.GetExtension(hdnfilePath.Value).ToUpper() == ".JPEG" || Path.GetExtension(hdnfilePath.Value).ToUpper() == ".BMP")
                                {
                                    pdffilepath = Server.MapPath("~/atala-capture-upload/" + Session.SessionID + "/EFAX/" + ClientSession.FacilityName.Replace(" ", "_").Replace("#", "").Replace(",", "") + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + "image.pdf");

                                    //Create image in PDF For Bug id: 64259 //Ref:link https://www.mikesdotnetting.com/article/87/itextsharp-working-with-images
                                    doc = new Document();
                                    PdfWriter.GetInstance(doc, new FileStream(pdffilepath, FileMode.Create));
                                    doc.Open();
                                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(SelectedFilePath);
                                    img.ScaleToFit(500f, 500f);
                                    doc.Add(img);
                                    doc.Close();

                                    //using (var fileStream = new FileStream(pdffilepath, FileMode.Create, FileAccess.Write))
                                    //{
                                    //    var document1 = new Document(new Rectangle(625, 975));
                                    //    Rectangle pageSize = new Rectangle(625, 975);
                                    //    var writerpdf = PdfWriter.GetInstance(document1, fileStream);
                                    //    string path=(SelectedFilePath + "/" + Path.GetFileName(spath));
                                    //    document1.Open();
                                    //    document1.NewPage();
                                    //    var con = writerpdf.DirectContent;
                                    //    con.BeginText();
                                    //    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(path);
                                    //   logo.ScaleAbsolute(500, 500);
                                    //   logo.SetAbsolutePosition(pageSize.GetLeft(10) , 0);
                                    //    con.AddImage(logo);
                                    //    con.EndText();
                                    //    document1.Close();
                                    //    writerpdf.Close();
                                    //}
                                }
                                else
                                {
                                    // pdffilepath = Server.MapPath(SelectedFilePath + "/" + Path.GetFileName(hdnfilePath.Value));
                                    pdffilepath = SelectedFilePath;

                                }
                                string sStoringFormat = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(pdffilepath);
                                sFTPAddress = ftpImageProcess.UploadToImageServer(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, pdffilepath, sStoringFormat, out string sCheckFileNotFoundExceptions);
                                if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                    return;
                                }
                                // string sFilePath = sFacilityPath + sStoringFormat;
                                if (sAttachFileName == string.Empty)
                                    sAttachFileName = sFTPAddress;
                                else
                                    sAttachFileName += " | " + sFTPAddress;
                            }
                            else
                            {
                                SelectedFilePath = hdnfilePath.Value;
                                //  string sStoringFormat = "EFAX_" + ClientSession.HumanId.ToString() + "_" + Path.GetFileNameWithoutExtension(spath) + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(spath);
                                // string sStoringFormat = "EFAX_" + ClientSession.HumanId.ToString() + "_" + txtSenderName.Value.Replace("'", "") + "_" + txtRecName.Value.Split('|')[0].Replace("'", "") + "_" + Path.GetFileNameWithoutExtension(spath) + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(spath);
                                // sFTPAddress = ftpImageProcess.UploadToImageServerFAX(UNCAuthPath, UNCPath, ftpIP, userName, password, domain, sDestinationFTPPath, SelectedFilePath, sStoringFormat);

                                string sStoringFormat = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(hdnfilePath.Value);
                                sFTPAddress = ftpImageProcess.UploadToImageServer(sDestinationFTPPath, ftpServerIP, ftpUserID, ftpPassword, SelectedFilePath, sStoringFormat, out string sCheckFileNotFoundExceptions);
                                if (sCheckFileNotFoundExceptions != "" && sCheckFileNotFoundExceptions.Contains("CheckFileNotFoundException"))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundExceptions.Split('~')[1] + "\");", true);
                                    return;
                                }
                                //  sFilePath = sFacilityPath + sStoringFormat;
                                if (sAttachFileName == string.Empty)
                                    sAttachFileName = sFTPAddress;
                                else
                                    sAttachFileName += " | " + sFTPAddress;
                            }
                            //fd = new FAXCOMEXLib.FaxDocument();
                            //fd.CoverPageType = FAXCOMEXLib.FAX_COVERPAGE_TYPE_ENUM.fcptSERVER;
                            //fd.CoverPage = "generic";
                            //fd.Body = spath;
                            //fd.ReceiptAddress = txtRecipientcompany.Text;

                            //fd.Subject = "Today's fax" + txtCoverpage.txtDLC.Text;

                            //fd.Sender.Name = txtSenderName.Text;

                            //fd.Sender.Company = txtSenderCompany.Text;
                            //fd.Sender.Email = txtSenderEmail.Text;
                            //fd.Sender.FaxNumber = txtSenderMaskFax.Text.Replace("-", "");
                            //fd.Sender.SaveDefaultSender();
                            //fd.Recipients.Add(txtSenderMaskFax.Text.Replace("-", ""), txtSenderName.Text);

                            ////This JobID[0] can be used to store the JobID of the fax job for later reference.
                            //string[] JobID = (string[])fd.ConnectedSubmit(fs);

                        }
                    }
                }
                //else
                //{
                //    fd = new FAXCOMEXLib.FaxDocument();

                //    fd.CoverPageType = FAXCOMEXLib.FAX_COVERPAGE_TYPE_ENUM.fcptSERVER;
                //    fd.CoverPage = "generic";// "C:\\Users\\administrator\\Desktop\\123.cov";

                //    fd.ReceiptAddress = txtRecipientcompany.Text;

                //    fd.Subject = "Today's fax" + txtCoverpage.txtDLC.Text;

                //    fd.Sender.Name = txtSenderName.Text;

                //    fd.Sender.Company = txtSenderCompany.Text;
                //    fd.Sender.Email = txtSenderEmail.Text;
                //    fd.Sender.FaxNumber = txtSenderMaskFax.Text.Replace("-", "");
                //    fd.Sender.SaveDefaultSender();
                //    fd.Recipients.Add(txtSenderMaskFax.Text.Replace("-", ""), txtSenderName.Text);


                //    string[] JobID = (string[])fd.ConnectedSubmit(fs);
                //}



                //FAXCOMEXLib.FaxOutgoingQueue foq = fs.Folders.OutgoingQueue;
                //foq.Refresh();
                //string returnval = "";

                //foreach (FAXCOMEXLib.FaxOutgoingJob foj in foq.GetJobs())
                //{
                //    returnval += foj.Id + ": " + foj.SubmissionTime + ": " + foj.Status + "/" + foj.ExtendedStatus + Convert.ToChar(10);
                //}

                //fs.Disconnect();

                //else /*unsupported file*/
                //{
                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('380057'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                //}
                //AddGrid();
                string GridJSON = hdnAddgrid.Value;
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(GridJSON);
                IList<ActivityLog> lstactivitylog = new List<ActivityLog>();
                //for (int r = 0; r < grdEFax.Rows.Count; r++)
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    objactivitylog = new ActivityLog();
                    //Cap - 1918
                    //objactivitylog.Human_ID = 0;
                    //objactivitylog.Encounter_ID = 0;
                    if (sMenuLevelEFax == string.Empty)
                    {
                        objactivitylog.Human_ID = ClientSession.HumanId;
                        objactivitylog.Encounter_ID = ClientSession.EncounterId;
                    }
                    else
                    {
                        objactivitylog.Human_ID = 0;
                        objactivitylog.Encounter_ID = 0;
                    }

                    objactivitylog.Activity_Type = "EFAX";
                    objactivitylog.Activity_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objactivitylog.Subject = txtSubject.Value;//"";
                    objactivitylog.Role = "";
                    objactivitylog.Encrypted_Message = "";
                    objactivitylog.Activity_By = ClientSession.UserName;
                    //Sender
                    objactivitylog.Fax_Sender_Name = txtSenderName.Value;
                    objactivitylog.Fax_Sender_Company = txtSenderCompany.Value;
                    //balaji
                    objactivitylog.Fax_Sender_Number = "+1" + txtSenderMaskFax.Value.Replace("-", "").Replace("(", "").Replace(")", "");
                    objactivitylog.Sent_To = txtSenderEmail.Value;
                    //Recipient
                    objactivitylog.Fax_Recipient_Company = dt.Rows[r].ItemArray[2].ToString(); //grdEFax.Rows[r].Cells[3].Text.ToString();//txtRecipientcompany.Value;
                    objactivitylog.Fax_Recipient_Name = dt.Rows[r].ItemArray[1].ToString(); //grdEFax.Rows[r].Cells[2].Text.ToString();//txtRecName.Value;
                                                                                            //balaji
                                                                                            //if (msktxtRecipientFax.Value != "")
                    objactivitylog.Fax_Recipient_Number = dt.Rows[r].ItemArray[3].ToString(); //grdEFax.Rows[r].Cells[4].Text.ToString();//"+1" + msktxtRecipientFax.Value.Replace("-", "").Replace("(", "").Replace(")", "");
                    objactivitylog.From_Address = dt.Rows[r].ItemArray[4].ToString();//grdEFax.Rows[r].Cells[5].Text.ToString();//txtRecipientmail.Value;

                    objactivitylog.Message = txtareaCoverpage.Value;
                    objactivitylog.Fax_Status = "READY TO SEND";
                    objactivitylog.Fax_File_Path = sAttachFileName.Replace(@"\", @"/");
                    //if (chkProvider.Checked)
                    //    objactivitylog.Fax_Recipient_Category = chkProvider.Value;
                    //else if (chkpatient.Checked)
                    //    objactivitylog.Fax_Recipient_Category = chkpatient.Value;
                    objactivitylog.Fax_Recipient_Category = dt.Rows[r].ItemArray[0].ToString();//grdEFax.Rows[r].Cells[1].Text.ToString();
                    objactivitylog.Group_ID = Convert.ToInt32(hdnGroupID.Value);
                    objactivitylog.Fax_Priority = hdnpriority.Value.ToString().Split('|')[0]; //DropDwnpriority.Items[DropDwnpriority.SelectedIndex].Value;
                    objactivitylog.Fax_Cover_Page_Template_Name = hdnpriority.Value.ToString().Split('|')[1];//DropDwncoverpage.Items[DropDwncoverpage.SelectedIndex].Text;
                    lstactivitylog.Add(objactivitylog);
                }

                objActivityMngr.SaveActivityLogManager(lstactivitylog, string.Empty);

                //balaji
                btnSendfax.Disabled = true;
                ClearFields();
                CreateEmptyHeader();
                hdnGroupID.Value = (Convert.ToInt32(hdnGroupID.Value) + 1).ToString();
                ViewState.Remove("dtEFaxGrid");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveSucessfully", "DisplayErrorMessage('1011133');window.close();Closefax();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveSucessfully", "Efaxsaveend();", true);
            }
            catch
            {

            }
            finally
            {
                doc.Close();
            }
            }

            void ClearFields()
            {
                //txtSenderName.Text;
                // txtSenderCompany.Text;
                //txtSenderMaskFax.Text;
                msktxtRecipientFax.Value = "";
                //txtSenderEmail.Value = string.Empty;
                txtRecipientcompany.Value = string.Empty;
                txtRecName.Value = string.Empty;
                //balaji
                //msktxtRecipientFax.Text = string.Empty;
                txtRecipientmail.Value = string.Empty;
                //txtCoverpage.txtDLC.Text = string.Empty;
                txtSubject.Value = string.Empty;

            }
            [WebMethod(EnableSession = true)]
            public static string EFaxoutboxload()
            {
                if (ClientSession.UserName == string.Empty)
                {
                    HttpContext.Current.Response.StatusCode = 999;
                    HttpContext.Current.Response.Status = "999 Session Expired";
                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                    return "Session Expired";
                }
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                List<string> ActivityType = new List<string>();
                ActivityType.Add("EFax");
                string sActivityLog = string.Empty;
                IList<ActivityLog> ActivityLogTempList = new List<ActivityLog>();
                ActivityLogTempList = ActivitylogMngr.GetActivityTypeByusername(ActivityType, ClientSession.UserName.ToString());
                ActivityLogList = ActivityLogTempList.OrderByDescending(a => a.Activity_Date_And_Time).ToList();
                for (int i = 0; i < ActivityLogList.Count; i++)
                {
                    ActivityLogList[i].Activity_Date_And_Time = UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time);
                }

                //IActivityLogList = ActivityLogList.Select(a => new { a.Activity_By });
                var vEFaxoutboxload = new { ActivityLogList = ActivityLogList };

                return JsonConvert.SerializeObject(vEFaxoutboxload);
                //return JsonConvert.SerializeObject(ActivityLogList);
            }
            [WebMethod(EnableSession = true)]
            public static string GetActivities(string FieldValue)
            {
                if (ClientSession.UserName == string.Empty)
                {
                    HttpContext.Current.Response.StatusCode = 999;
                    HttpContext.Current.Response.Status = "999 Session Expired";
                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                    return "Session Expired";
                }
                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                List<string> ActivityType = new List<string>();
                ActivityType.Add(FieldValue);
                string sActivityLog = string.Empty;
                ActivityLogList = ActivitylogMngr.GetActivityTypeByusername(ActivityType, ClientSession.UserName.ToString());
                for (int i = 0; i < ActivityLogList.Count; i++)
                {
                    if (ActivityLogList[i].Activity_Type.ToUpper() == "EFAX")
                    {
                        if (sActivityLog == string.Empty)
                        {
                            sActivityLog = "*" + "Sent " + Path.GetFileName(ActivityLogList[i].Fax_File_Path.Replace("|", ",")) + " on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") +
                                " by " + ActivityLogList[i].Fax_Sender_Name + "-" + ActivityLogList[i].Fax_Sender_Number + " to " + ActivityLogList[i].Fax_Recipient_Name + "-" + ActivityLogList[i].Fax_Recipient_Number;
                        }
                        else
                        {
                            sActivityLog += Environment.NewLine + "*" + "Sent " + Path.GetFileName(ActivityLogList[i].Fax_File_Path.Replace("|", ",")) + " on " + UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt") +
                               " by " + ActivityLogList[i].Fax_Sender_Name + "-" + ActivityLogList[i].Fax_Sender_Number + " to " + ActivityLogList[i].Fax_Recipient_Name + "-" + ActivityLogList[i].Fax_Recipient_Number;
                        }
                    }
                }
                return sActivityLog;


            }

            [WebMethod(EnableSession = true)]
            public static string LoadFaxDetails(string FaxNumber)
            {
                if (ClientSession.UserName == string.Empty)
                {
                    HttpContext.Current.Response.StatusCode = 999;
                    HttpContext.Current.Response.Status = "999 Session Expired";
                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                    return "Session Expired";
                }
                ContextManager objContextmngr = new ContextManager();
                IList<Context> lstContext = new List<Context>();
                lstContext = objContextmngr.SearchFAX(FaxNumber);
                IList<string> FinalLst = new List<string>();
                FinalLst = lstContext.Select(a => a.Context_Name + "^" + a.Context_Company_Name + "^" + a.Context_Fax_Number + "^" + a.Context_Email).ToList<string>();
                return JsonConvert.SerializeObject(FinalLst);
            }
            [WebMethod(EnableSession = true)]
            public static string OrdersList(string Ordersid)
            {
                if (ClientSession.UserName == string.Empty)
                {
                    HttpContext.Current.Response.StatusCode = 999;
                    HttpContext.Current.Response.Status = "999 Session Expired";
                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                    return "Session Expired";
                }
                OrdersManager orm = new OrdersManager();
                string result = orm.GetFaxOrders(Ordersid);
                var vresult = new { VendorName = result.Split('|')[0], PatientName = result.Split('|')[1] };
                //ContextManager objContextmngr = new ContextManager();
                //IList<Context> lstContext = new List<Context>();
                //lstContext = objContextmngr.SearchFAX(FaxNumber);
                //IList<string> FinalLst = new List<string>();
                //FinalLst = lstContext.Select(a => a.Context_Name + "^" + a.Context_Company_Name + "^" + a.Context_Fax_Number + "^" + a.Context_Email).ToList<string>();
                return JsonConvert.SerializeObject(vresult);
            }
            [WebMethod(EnableSession = true)]
            public static string EFaxoutboxRetry(string ActivityId)
            {
                string result = string.Empty;
                if (ClientSession.UserName == string.Empty)
                {
                    HttpContext.Current.Response.StatusCode = 999;
                    HttpContext.Current.Response.Status = "999 Session Expired";
                    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                    return "Session Expired";
                }

                ActivityLogManager ActivitylogMngr = new ActivityLogManager();
                IList<ActivityLog> UpdateActivityLogList = new List<ActivityLog>();
                UpdateActivityLogList = ActivitylogMngr.GetFaxActivity(ActivityId);
                if (UpdateActivityLogList.Count > 0)
                {
                    UpdateActivityLogList[0].Fax_Status = "READY TO SEND";
                    UpdateActivityLogList[0].Error_Description = string.Empty;
                    IList<ActivityLog> saveActivityLogList = new List<ActivityLog>();
                    ActivitylogMngr.SaveUpdateDeleteWithTransaction(ref saveActivityLogList, UpdateActivityLogList, null, string.Empty);
                }

                IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
                List<string> ActivityType = new List<string>();
                ActivityType.Add("EFax");
                string sActivityLog = string.Empty;
                IList<ActivityLog> ActivityLogTempList = new List<ActivityLog>();
                ActivityLogTempList = ActivitylogMngr.GetActivityTypeByusername(ActivityType, ClientSession.UserName.ToString());
                ActivityLogList = ActivityLogTempList.OrderByDescending(a => a.Activity_Date_And_Time).ToList();
                for (int i = 0; i < ActivityLogList.Count; i++)
                {
                    ActivityLogList[i].Activity_Date_And_Time = UtilityManager.ConvertToLocal(ActivityLogList[i].Activity_Date_And_Time);
                }
                var vEFaxoutboxload = new { ActivityLogList = ActivityLogList };

                return JsonConvert.SerializeObject(vEFaxoutboxload);
            }

            protected void btnNew_Click(object sender, EventArgs e)
            {
                AddGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveSucessfully", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            public void AddGrid()
            {



                if (txtRecName.Value != string.Empty || msktxtRecipientFax.Value != "")
                {
                    DataTable dt = null;
                    if (ViewState["dtEFaxGrid"] != null)
                        dt = (DataTable)ViewState["dtEFaxGrid"];
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Category", typeof(string));
                        dt.Columns.Add("Name", typeof(string));
                        dt.Columns.Add("Company", typeof(string));
                        dt.Columns.Add("Fax", typeof(string));
                        dt.Columns.Add("EMail", typeof(string));
                    }
                    DataRow dr = dt.NewRow();
                    string sCategory = string.Empty;
                    string sFax = string.Empty;
                    if (msktxtRecipientFax.Value != "")
                        sFax = "+1" + msktxtRecipientFax.Value.Replace("-", "").Replace("(", "").Replace(")", "");
                    if (chkProvider.Checked)
                        sCategory = chkProvider.Value;
                    else if (chkpatient.Checked)
                        sCategory = chkpatient.Value;
                    dr["Category"] = sCategory;
                    dr["Name"] = txtRecName.Value;
                    dr["Company"] = txtRecipientcompany.Value;
                    dr["Fax"] = sFax;
                    dr["EMail"] = txtRecipientmail.Value;
                    dt.Rows.Add(dr);
                    //grdEFax.DataSource = dt;
                    //grdEFax.DataBind();
                    ViewState["dtEFaxGrid"] = dt;
                    ClearFields();
                    btnSendfax.Disabled = false;
                }
            }
            protected void grdEFax_RowCommand(object sender, GridViewCommandEventArgs e)
            {
                if (e.CommandName == "DeleteRow")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int Rowindex = row.RowIndex;
                    if (ViewState["dtEFaxGrid"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["dtEFaxGrid"];
                        dt.Rows.RemoveAt(Rowindex);
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["dtEFaxGrid"] = dt;
                            //grdEFax.DataSource = dt;
                            //grdEFax.DataBind();
                        }
                        else
                            CreateEmptyHeader();
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveSucessfully", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
    }