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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.Core.DTO;
using System.Drawing;
using Acurus.Capella.UI;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using System.Web.Services;
using Newtonsoft.Json;
using System.IO;
using System.Xml;



namespace Acurus.Capella.UI
{
    public partial class frmAddorViewMessage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            DLC.txtDLC.Attributes.Add("onkeypress", "EnableAll(this);");
            DLC.txtDLC.Attributes.Add("onchange", "EnableAll(this);");
            DLC.txtDLC.Attributes.Add("onkeyup", "EnableAll(this);");
            if (!IsPostBack)
            {
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
            }
            lblassignedto.ForeColor = Color.Red;
            lblassignedto.Text = "Assigned To*";
            ulong ulHumanID = 0;
            if (Request["AccountNum"] != null)
            {
                ulHumanID = Convert.ToUInt64(Request["AccountNum"]);
            }
            else
            {
                ulHumanID = ClientSession.HumanId;
            }
            txtAccount.Text = ulHumanID.ToString();
        ln:
            try
            {
                string sdivPatientstrip = UtilityManager.FillPatientStrip(ulHumanID);
                if (sdivPatientstrip != null)
                {
                    divPatientstrip.InnerText = sdivPatientstrip;
                }
            }
            catch (Exception ex)
            {
                //XmlText.Close();
                //Thread.Sleep(5000);
                UtilityManager.GenerateXML(ulHumanID.ToString(), "Human");

                goto ln;
            }


            //Human objFillHuman = new Human();
            //IList<Human> lstHuman = new List<Human>();
            //string sBirth_Date = string.Empty;
            //string sPatientstrip = string.Empty;
            //XmlTextReader XmlText = null;
            //if (ulHumanID != 0)
            //{
            //ln:

            //    string FileName = "Human" + "_" + ulHumanID + ".xml";
            //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //    try
            //    {
            //        if (File.Exists(strXmlFilePath) == true)
            //        {
            //            XmlDocument itemDoc = new XmlDocument();
            //            XmlText = new XmlTextReader(strXmlFilePath);
            //            XmlNodeList xmlTagName = null;
            //            using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //            {
            //                itemDoc.Load(fs);
            //                XmlText.Close();

            //                if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
            //                {
            //                    xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

            //                    if (xmlTagName != null)
            //                    {
            //                        for (int j = 0; j < xmlTagName.Count; j++)
            //                        {
            //                            if (xmlTagName[j].Attributes["Id"].Value == ulHumanID.ToString())
            //                            {
            //                                objFillHuman.Birth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value);
            //                                sBirth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value).ToString("dd-MMM-yyyy");
            //                            if (System.Text.RegularExpressions.Regex.IsMatch(xmlTagName[j].Attributes["Id"].Value, "^[0-9]*$") == true)
            //                            {
            //                                objFillHuman.Id = Convert.ToUInt32(xmlTagName[j].Attributes["Id"].Value);
            //                            }
            //                                objFillHuman.Last_Name = xmlTagName[j].Attributes["Last_Name"].Value;
            //                                objFillHuman.First_Name = xmlTagName[j].Attributes["First_Name"].Value;
            //                                objFillHuman.MI = xmlTagName[j].Attributes["MI"].Value;
            //                                objFillHuman.Suffix = xmlTagName[j].Attributes["Suffix"].Value;
            //                                objFillHuman.Sex = xmlTagName[j].Attributes["Sex"].Value;
            //                                objFillHuman.Work_Phone_No = xmlTagName[j].Attributes["Work_Phone_No"].Value;
            //                                objFillHuman.Work_Phone_Ext = xmlTagName[j].Attributes["Work_Phone_Ext"].Value;
            //                                objFillHuman.Home_Phone_No = xmlTagName[j].Attributes["Home_Phone_No"].Value;
            //                                objFillHuman.Cell_Phone_Number = xmlTagName[j].Attributes["Cell_Phone_Number"].Value;
            //                                if (xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != null && xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != string.Empty)
            //                                    objFillHuman.ACO_Is_Eligible_Patient = xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value.ToString();
            //                                else
            //                                    objFillHuman.ACO_Is_Eligible_Patient = "";
            //                            objFillHuman.Human_Type = xmlTagName[j].Attributes["Human_Type"].Value;

            //                            lstHuman.Add(objFillHuman);
            //                            }
            //                        }

            //                        string phoneno = "";

            //                        if (lstHuman != null && lstHuman.Count > 0)
            //                        {

            //                            if (objFillHuman.Home_Phone_No.Length == 14)
            //                            {
            //                                phoneno = objFillHuman.Home_Phone_No;
            //                            }
            //                            else
            //                            {
            //                                phoneno = objFillHuman.Cell_Phone_Number;
            //                            }

            //                        }

            //                        string sPatientSex = string.Empty;


            //                        if (objFillHuman.Sex != string.Empty)
            //                        {
            //                            if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
            //                            {
            //                                sPatientSex = "UNK";
            //                            }
            //                            else
            //                            {
            //                                sPatientSex = objFillHuman.Sex.Substring(0, 1);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            sPatientSex = "";
            //                        }

            //                        string sAcoEligiblePatient = string.Empty;
            //                        sAcoEligiblePatient = objFillHuman.ACO_Is_Eligible_Patient;

            //                        sPatientstrip = " " + objFillHuman.Last_Name + "," + objFillHuman.First_Name
            //                            + "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + " | " +
            //    objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
            //   (CalculateAge(objFillHuman.Birth_Date)).ToString() +
            //   "  year(s) | " + sPatientSex + " | Acc #:" + ulHumanID.ToString() +
            //   " | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
            //   "Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type + " | ";

            //                        if (sAcoEligiblePatient != null && sAcoEligiblePatient != string.Empty && sAcoEligiblePatient != "N")
            //                        {
            //                            sPatientstrip += sAcoEligiblePatient + "   |   ";
            //                        }

            //                        divPatientstrip.InnerText = sPatientstrip;
            //                    }
            //                }
            //                fs.Close();
            //                fs.Dispose();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        XmlText.Close();
            //        //Thread.Sleep(5000);
            //        UtilityManager.GenerateXML(ulHumanID.ToString(), "Human");

            //        goto ln;
            //    }
            //}

            #region OldCode
            //txtAccount.Text = Request["AccountNum"].ToString();
            ////if (Request["openingfrom"] != null && Request["openingfrom"].ToString().ToUpper() == "TASK")
            ////{
            ////    btnPatient.Visible = true;
            ////}

            //if (Request["PatientName"] != null)
            //{
            //    txtPatientName.Text = Request["PatientName"].TrimEnd(',');
            //    if (Request["PatientDOB"] != null && Request["PatientDOB"] != "")
            //        txtPatientDOB.Text = Request["PatientDOB"];
            //    if (Request["HumanType"] != null)
            //        txtPatientType.Text = Request["HumanType"].ToString();
            //}
            //else
            //{
            //    string FileName = "Human" + "_" + txtAccount.Text + ".xml";
            //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //    try
            //    {
            //        if (File.Exists(strXmlFilePath) == true)
            //        {
            //            XmlDocument itemDoc = new XmlDocument();
            //            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //            try
            //            {
            //                // itemDoc.Load(XmlText);
            //                using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //                {
            //                    itemDoc.Load(fs);

            //                    XmlText.Close();
            //                    if (itemDoc.GetElementsByTagName("Human").Count > 0 && itemDoc.GetElementsByTagName("Human")[0] != null)
            //                    {
            //                        txtPatientName.Text = itemDoc.GetElementsByTagName("Human")[0].Attributes["Last_Name"].Value + "," + itemDoc.GetElementsByTagName("Human")[0].Attributes["First_Name"].Value + " " + itemDoc.GetElementsByTagName("Human")[0].Attributes["MI"].Value;
            //                        txtPatientDOB.Text = Convert.ToDateTime(itemDoc.GetElementsByTagName("Human")[0].Attributes["Birth_Date"].Value).ToString("dd-MMM-yyyy");
            //                        txtPatientType.Text = itemDoc.GetElementsByTagName("Human")[0].Attributes["Human_Type"].Value;

            //                        if (itemDoc.GetElementsByTagName("Human")[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != null && itemDoc.GetElementsByTagName("Human")[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != string.Empty && itemDoc.GetElementsByTagName("Human")[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != "N")
            //                            txtPatientType.Text += " | " + itemDoc.GetElementsByTagName("Human")[0].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value;
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                // ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + txtAccount.Text.ToString() + "','Human','task');", true);


            //                //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

            //                return;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message + " - " + strXmlFilePath);
            //    }
            //}
            #endregion
            //}
            //else if (ClientSession.HumanId > 0)
            //{
            //    if (ClientSession.PatientPane != null && ClientSession.PatientPane != string.Empty)
            //    {
            //        divPatientstrip.InnerText = ClientSession.PatientPane;
            //    }
            //    #region OldCode
            //    //txtAccount.Text = ClientSession.HumanId.ToString();
            //    //if (ClientSession.PatientPane != null && ClientSession.PatientPane != string.Empty)
            //    //{
            //    //    string[] HumanDetails = ClientSession.PatientPane.Split('|');
            //    //    txtPatientName.Text = HumanDetails[0].Trim();
            //    //    txtPatientDOB.Text = HumanDetails[1].Trim();
            //    //    if (HumanDetails[7].Split(':').Count() > 1)
            //    //        txtPatientType.Text = HumanDetails[7].Split(':')[1].Trim();
            //    //    if (HumanDetails.Length > 10)
            //    //    {
            //    //        if (HumanDetails[HumanDetails.Length - 2].Trim() != string.Empty && HumanDetails[HumanDetails.Length-2].Trim() != "N" && HumanDetails[HumanDetails.Length-3].Trim().Contains("Past Due:")==true)
            //    //            txtPatientType.Text += " | " + HumanDetails[HumanDetails.Length-2].Trim();
            //    //    }
            //    //}
            //    #endregion
            //}
            if (Request["IsMYQ"] != null && Request["IsMYQ"] == "N")
                txtCreatedBy.Text = ClientSession.UserName;
            //FillMessageGrid();
            if (Request["parentscreen"] != null && (Request["parentscreen"] == "MyQ" || Request["parentscreen"] == "PatientChart")
                && (Request["MessageID"] != null && System.Text.RegularExpressions.Regex.IsMatch(Request["MessageID"], "^[0-9]*$") == true && Convert.ToUInt32(Request["MessageID"]) > 0))
            {
                RowForAll.Style.Add("display", "none");
                Tr1ForHide.Style.Add("display", "none");
                lblassignedto.ForeColor = Color.Red;
                if (Request["parentscreen"] != null && Request["parentscreen"] == "PatientChart")
                {
                    chkshowall.Width = 50;
                    txtCallerName.ReadOnly = true;
                    txtCreatedBy.ReadOnly = true;
                    chkshowall.Enabled = false;
                    //Gitlab# 2685 - Visible “add to patient chart” checkbox
                    // ChkPatientChart.Enabled = false;
                    ChkPatientChart.Visible = true;
                    DLC.txtDLC.ReadOnly = true;
                    DLC.txtDLC.Enabled = false;
                    pnlMessageInfo.Visible = false;
                    pnlSearchMessage.Visible = false;
                    pnlMessageInfo.Visible = false;
                }
                else
                {
                    lblassignedto.ForeColor = Color.Red;
                    lblassignedto.Text = "Assigned To*";
                    ChkPatientChart.Visible = true;
                }
            }
            else
            {
                //Gitlab# 2685 - Visible “add to patient chart” checkbox
                // ChkPatientChart.Visible = false;
                ChkPatientChart.Visible = true;
                //FillMessageGrid();
                RowForHide.Style.Add("display", "none");
            }
            if (txtAccount.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Patient Communication", "DisplayErrorMessage('7580010');sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();", true);
                return;
            }
            //  }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Patient Communication StopLoad", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);//added for BugID:45808
        }
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
        [WebMethod(EnableSession = true)]
        public static string FillMessageGrid(string sHumanId)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "";
            }
            PatientDetailDto patientdetaildto = new PatientDetailDto();
            PatientNotesManager patientnotesmngr = new PatientNotesManager();
            if (sHumanId != null && sHumanId != "" && sHumanId != "0")
            {
                patientdetaildto = patientnotesmngr.GetPopupDetailsNew(sHumanId);
            }
            else
            {
                patientdetaildto = patientnotesmngr.GetPopupDetailsNew(ClientSession.HumanId.ToString());
            }

            if (patientdetaildto.ilstPatientNotes.Count > 0)
            {
                HttpContext.Current.Session.Add("WFOBJECTlist", patientdetaildto.ilstWFObj);
                for (int i = 0; i < patientdetaildto.ilstPatientNotes.Count; i++)
                {
                    if (patientdetaildto.ilstPatientNotes[i].Created_Date_And_Time != null && patientdetaildto.ilstPatientNotes[i].Created_Date_And_Time.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        patientdetaildto.ilstPatientNotes[i].Created_Date_And_Time = UtilityManager.ConvertToLocal(patientdetaildto.ilstPatientNotes[i].Created_Date_And_Time);
                    }
                    if (patientdetaildto.ilstPatientNotes[i].Modified_Date_And_Time != null && patientdetaildto.ilstPatientNotes[i].Modified_Date_And_Time.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        patientdetaildto.ilstPatientNotes[i].Modified_Date_And_Time = UtilityManager.ConvertToLocal(patientdetaildto.ilstPatientNotes[i].Modified_Date_And_Time);
                    }
                    //CAP-692 - Convert in to UTC time to Local time.
                    if (patientdetaildto.ilstPatientNotes[i].Message_Date_And_Time != null
                        && patientdetaildto.ilstPatientNotes[i].Message_Date_And_Time.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        patientdetaildto.ilstPatientNotes[i].Message_Date_And_Time = UtilityManager.ConvertToLocal(patientdetaildto.ilstPatientNotes[i].Message_Date_And_Time);
                    }
                }
            }
            return JsonConvert.SerializeObject(patientdetaildto);
        }
        [WebMethod(EnableSession = true)]
        public static string LoadPatientCommunication(string MessageID, string ParentScreen)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string[] FieldName = new string[8];
            FieldName[0] = "MESSAGE ORIGIN";
            FieldName[1] = "Type";
            FieldName[2] = "MESSAGE DESCRIPTION";
            FieldName[3] = "RelationShip";
            FieldName[4] = "PRIORITY FOR MESSAGE";
            //FieldName[5] = "TASK MESSAGE";
            StaticLookupManager objStaticLookUpMngr = new StaticLookupManager();
            IList<StaticLookup> ilstStaticLookUp = new List<StaticLookup>();
            ilstStaticLookUp = objStaticLookUpMngr.getStaticLookupByFieldName(FieldName, "Sort_Order");
            //ilstStaticLookUp = ilstStaticLookUp.OrderBy(s => s.Description).ToList<StaticLookup>();
            IList<PatientNotes> MessageDetails = new List<PatientNotes>();
            PatientNotesManager objPatNotesMngr = new PatientNotesManager();
            if ((ParentScreen == "MyQ" || ParentScreen == "PatientChart") && (MessageID != "" && Convert.ToUInt32(MessageID) > 0))
            {
                MessageDetails = objPatNotesMngr.GetPatientNotesByMsgID(Convert.ToUInt32(MessageID));
                IList<string> patientlst = new List<string>();
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(ClientSession.FacilityName, ClientSession.LegalOrg);
                if (MessageDetails.Count > 0 && !patientlst.Any(a=>a.Contains(MessageDetails[0].Assigned_To)))
                {
                    patientlst = objPatNotesMngr.MapPhysicianUserListForFacility("SHOW ALL", ClientSession.LegalOrg);
                }
                var result = new { DropDown = ilstStaticLookUp, Message = MessageDetails, Facility = MessageDetails[0].Facility_Name, AssignedTo = patientlst };
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                IList<string> patientlst = new List<string>();
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(ClientSession.FacilityName, ClientSession.LegalOrg);
                var Result = new { DropDown = ilstStaticLookUp, Message = MessageDetails, Facility = ClientSession.FacilityName, AssignedTo = patientlst };
                return JsonConvert.SerializeObject(Result);
            }
        }
        [WebMethod(EnableSession = true)]
        public static string laodAssigned(string chkshowall, string facility)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<string> patientlst = new List<string>();
            PatientNotesManager objPatNotesMngr = new PatientNotesManager();
            if (chkshowall == "false" && facility == "")
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(ClientSession.FacilityName, ClientSession.LegalOrg);
            else if (chkshowall == "false" && facility != "")
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility(facility, ClientSession.LegalOrg);
            else
                patientlst = objPatNotesMngr.MapPhysicianUserListForFacility("SHOW ALL", ClientSession.LegalOrg);
            var Result = new { AssignedTo = patientlst };
            return JsonConvert.SerializeObject(Result);
        }
        [WebMethod(EnableSession = true)]
        public static string SaveMenuClick(string AccNo, string AssignedTo, string RelationShip, string FacilityName, string CallerName,
            string MessageOrigin, string Priority, string MessageType, string DLC, string Type, string MessageDate,
            string PatientChart, string ParentScreen, string StartTime, string sBtnText, string sMessageID, string WFOBJID, string DateTime)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            PatientNotesManager patientnotesmngr = new PatientNotesManager();
            if (sBtnText == "Add")
            {
                IList<PatientNotes> patientlst = new List<PatientNotes>();
                PatientNotes objpatientnotes = new PatientNotes();
                objpatientnotes.Human_ID = Convert.ToUInt64(AccNo);
                objpatientnotes.Assigned_To = AssignedTo;
                objpatientnotes.Relationship = RelationShip;
                objpatientnotes.Facility_Name = FacilityName;
                objpatientnotes.Caller_Name = CallerName;
                objpatientnotes.Message_Orign = MessageOrigin;
                objpatientnotes.Priority = Priority;
                objpatientnotes.Message_Description = MessageType;
                //CAP-674 - If the note is coming then concat.
                if (!string.IsNullOrEmpty(DLC))
                { objpatientnotes.Notes = "@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm:ss tt") + "): " + DLC; }
                objpatientnotes.Created_By = ClientSession.UserName;
                objpatientnotes.Type = Type;
                if ((MessageDate != null) && MessageDate != "" && (MessageDate.ToString() != "0001-01-01"))
                {
                    //objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(MessageDate);//BugID:43162
                    objpatientnotes.Message_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(MessageDate));
                }
                objpatientnotes.Is_PatientChart = PatientChart;
                if (ParentScreen == "EditAppointMents")
                {
                    objpatientnotes.Source = "Appointments";
                }
                objpatientnotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                if (Type == "TASK")
                {
                    WFObject objWf = new WFObject();
                    objWf.Current_Process = "START";
                    objWf.Current_Owner = AssignedTo;
                    objWf.Fac_Name = FacilityName;
                    objWf.Obj_Type = "TASK";
                    objWf.Current_Arrival_Time = UtilityManager.ConvertToUniversal();
                    patientnotesmngr.SavePatientMessage(objpatientnotes, objWf, null);
                }
                else
                {
                    patientlst.Add(objpatientnotes);
                    IList<PatientNotes> patientdetail = patientnotesmngr.AddPatientlst(patientlst, null, null);
                }
                HttpContext.Current.Session["IsPatientCommunicated"] = true;
            }
            else if (sBtnText == "Update")
            {
                //HttpContext.Current.Session["WFOBJECTlist"];               
                IList<PatientNotes> patientnoteslist = new List<PatientNotes>();
                IList<PatientNotes> patientupdatelst = new List<PatientNotes>();
                PatientNotes objpatientnotes = new PatientNotes();
                patientnoteslist = patientnotesmngr.GetPatientNotesByMsgID(Convert.ToUInt32(sMessageID));
                if (patientnoteslist.Count > 0)
                {
                    objpatientnotes = patientnoteslist[0];

                    objpatientnotes.Human_ID = Convert.ToUInt64(AccNo);
                    objpatientnotes.Assigned_To = AssignedTo;
                    objpatientnotes.Relationship = RelationShip;
                    objpatientnotes.Facility_Name = FacilityName;
                    objpatientnotes.Caller_Name = CallerName;
                    objpatientnotes.Message_Orign = MessageOrigin;
                    objpatientnotes.Priority = Priority;
                    objpatientnotes.Message_Description = MessageType;
                    //CAP-674 - If the note is coming then concat.
                    if (!string.IsNullOrEmpty(DLC))
                    {
                        objpatientnotes.Notes = objpatientnotes.Notes + Environment.NewLine + "@" + ClientSession.UserName + "(" + Convert.ToDateTime(DateTime).ToString("dd-MMM-yyyy hh:mm:ss tt") + "): " + DLC;  //DLC;
                    }
                    objpatientnotes.Modified_By = ClientSession.UserName;
                    objpatientnotes.Type = Type;
                    if ((MessageDate != null) && MessageDate != "" && (MessageDate.ToString() != "0001-01-01"))
                    {
                        //objpatientnotes.Message_Date_And_Time = Convert.ToDateTime(MessageDate);//BugID:43162
                        objpatientnotes.Message_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(MessageDate));
                    }
                    objpatientnotes.Is_PatientChart = PatientChart;
                    if (ParentScreen == "EditAppointMents")
                    {
                        objpatientnotes.Source = "Appointments";
                    }
                    objpatientnotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    patientupdatelst.Add(objpatientnotes);

                    WFObject objWf = new WFObject();
                    IList<WFObject> WFObjList = new List<WFObject>();
                    if (HttpContext.Current.Session["WFOBJECTlist"] != null)
                    {
                        var wfobject = HttpContext.Current.Session["WFOBJECTlist"] as List<WFObject>;
                        if (wfobject != null && wfobject.Count > 0)
                        {
                            wfobject = wfobject.Where(a => a.Id == Convert.ToUInt32(WFOBJID) && a.Obj_Type.ToUpper() == "TASK").ToList<WFObject>();

                            if (wfobject != null && wfobject.Count > 0)
                            {
                                objWf = wfobject[0];
                                objWf.Current_Owner = AssignedTo;
                                objWf.Fac_Name = FacilityName;
                                objWf.Current_Arrival_Time = UtilityManager.ConvertToUniversal();
                                //objWf.Version += 1;
                                WFObjList.Add(objWf);
                            }
                        }
                    }

                    IList<PatientNotes> patientdetail = patientnotesmngr.AddPatientlst(null, patientupdatelst, WFObjList);
                }
            }

            // IList<PatientNotes> MessageDetails = new List<PatientNotes>();
            //MessageDetails = patientnotesmngr.GetMessageDetails(string.Empty, string.Empty, AccNo);
            return JsonConvert.SerializeObject("");
        }
        [WebMethod(EnableSession = true)]
        public static string PrintGrid(string strHumanId)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            int strHumanID = Convert.ToInt32(strHumanId);
            string strPath, strFooterText = string.Empty;

            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            //string[] conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString).ToString().Split(';');
            string[] conString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ToString().Split(';');
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

            //  IList<PatientPane> PatientPaneList = ClientSession.PatientPaneList.Where(a => a.Encounter_ID == ClientSession.EncounterId).ToList<PatientPane>();
            // string strDemographics = PatientPaneList[0].Last_Name + ", " + PatientPaneList[0].First_Name + " " + PatientPaneList[0].MI + " " + PatientPaneList[0].Suffix + " | " + Convert.ToDateTime(PatientPaneList[0].Birth_Date).ToString("dd-MMM-yyyy") + " | " + PatientPaneList[0].Sex + " | Acc #:" + PatientPaneList[0].Human_Id + " | " + PatientPaneList[0].Patient_Type + " | " + strDos + " | " + strProvider;
            string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl_" + ClientSession.LegalOrg].ToString();
            strPath = sBIRTReportUrl + "Patient Communication Report.rptdesign" + sDBConnection + "&strHumanId=" + strHumanID + "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword + "&legal_org=" + ClientSession.LegalOrg;
            return strPath;
        }
        [WebMethod(EnableSession = true)]
        public static string SaveMyQClick(string AccNo, string AssignedTo, string RelationShip, string FacilityName, string CallerName,
          string MessageOrigin, string Priority, string MessageType, string DLC, string Type, string MessageDate,
          string PatientChart, string Button, string MessageID, string StartTime, string HistoryNotes, string datetime)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            PatientNotesManager objPatNotesMngr = new PatientNotesManager();
            PatientNotes objPatNotes = new PatientNotes();
            IList<PatientNotes> PatientNotesList = new List<PatientNotes>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick : Start", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick Manager call - GetPatientNotesByMsgID : Start", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
            PatientNotesList = objPatNotesMngr.GetPatientNotesByMsgID(Convert.ToUInt32(MessageID));
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick Manager call - GetPatientNotesByMsgID : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
            if (PatientNotesList.Count > 0)
            {
                objPatNotes = PatientNotesList[0];
                objPatNotes.Is_PatientChart = PatientChart;
                objPatNotes.Type = Type;
                objPatNotes.Relationship = RelationShip;
                objPatNotes.Message_Description = MessageType;
                objPatNotes.Caller_Name = CallerName;
                objPatNotes.Message_Orign = MessageOrigin;
                if (!string.IsNullOrWhiteSpace(AssignedTo))
                {
                    objPatNotes.Assigned_To = AssignedTo;
                }
                objPatNotes.Facility_Name = FacilityName;
                if (MessageDate != null && MessageDate != "")
                    objPatNotes.Message_Date_And_Time = UtilityManager.ConvertToUniversal(Convert.ToDateTime(MessageDate));
                //objPatNotes.Message_Date_And_Time = Convert.ToDateTime(MessageDate);//BugID:43162
                //CAP-674 - If the note is coming then concat.
                if (!string.IsNullOrEmpty(DLC))
                { objPatNotes.Notes = HistoryNotes + "\n@" + ClientSession.UserName + "(" + Convert.ToDateTime(datetime).ToString("dd-MMM-yyyy hh:mm:ss tt") + "): " + DLC; }
                objPatNotes.Priority = Priority;
                objPatNotes.Modified_By = ClientSession.UserName;
                objPatNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            }
            string sCheckAssigned = "true";
            if (Button == "Save")
            {
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick ManagerCall Save - updatePatientMessageAlone : Start", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                objPatNotesMngr.updatePatientMessageAlone(objPatNotes);
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick ManagerCall Save - updatePatientMessageAlone : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                // IList<PatientNotes> MessageDetails = new List<PatientNotes>();
                string MessageDetails = HistoryNotes + "\n@" + ClientSession.UserName + "(" + Convert.ToDateTime(datetime).ToString("dd-MMM-yyyy hh:mm:ss tt") + "): " + DLC; //objPatNotesMngr.GetMessageDetails(string.Empty, string.Empty, AccNo);
                return JsonConvert.SerializeObject(MessageDetails);
            }
            else if (Button == "Send")
            {
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick ManagerCall Send - UpdatePatientMessage : Start", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                objPatNotesMngr.UpdatePatientMessage(objPatNotes, string.Empty, 1, AssignedTo, UtilityManager.ConvertToUniversal(), null);
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick ManagerCall Send - UpdatePatientMessage : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                if (AssignedTo == ClientSession.UserName)
                {
                    sCheckAssigned = "false";
                }
                //return "";
                return JsonConvert.SerializeObject(sCheckAssigned);
            }
            else if (Button == "Task Complete")
            {
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick ManagerCall Task Complete - UpdatePatientMessage : Start", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                objPatNotesMngr.UpdatePatientMessage(objPatNotes, "TASK", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), null);
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick ManagerCall Task Complete - UpdatePatientMessage : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "SaveMyQClick : End", DateTime.Now, sGroup_ID_Log, "frmPatientCommunication");
                return "";
            }

            return "";
        }
        [WebMethod(EnableSession = true)]
        public static string SearchClick(string Description, string Notes, string AccNo)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            PatientNotesManager objPatNotesMngr = new PatientNotesManager();
            IList<PatientNotes> MessageDetails = new List<PatientNotes>();
            MessageDetails = objPatNotesMngr.GetMessageDetails(Description, Notes, AccNo);
            return JsonConvert.SerializeObject(MessageDetails);
        }

        public int CalculateAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;
            return years;
        }
    }
}
