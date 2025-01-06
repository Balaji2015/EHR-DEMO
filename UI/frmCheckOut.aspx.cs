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
using Telerik.Web.UI;
using Acurus.Capella.Core.DTO;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using System.Text.RegularExpressions;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
//using Acurus.Capella.Proxy.LookupTables;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Web.Hosting;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmCheckOut : System.Web.UI.Page
    {
        EncounterManager encMngr = new EncounterManager();
        UserLookupManager localFieldLookupManager = new UserLookupManager();
        PhysicianManager phyMngr = new PhysicianManager();
        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();


        FillDocuments objdocument = new FillDocuments();
        public List<PatientPane> paneDetails = new List<PatientPane>();
        DocumentManager objDocumentProxy = new DocumentManager();
        string sHuman = string.Empty, sEncPhyUserName = string.Empty;
        PrintOrders print = new PrintOrders();
        bool Is_Saved = false;
        public List<FillHumanDTO> patientdetail = null;
        Encounter fillEncounterlist = null;
        frmEditAppointment editAppt = null;
        public bool is_FormClosed = false;
        public bool Is_AutoSave = false;
        public bool Is_CheckOut = false;
        DataSet ds = new DataSet();
        DataTable dtTable = new DataTable();
        DataRow dr = null;
        DateTime dueDate;
        string humanID;
        string EncounterID;
        string PhysicianID;
        IEnumerable<XElement> ilstPhysician = null;
        string OrderType = "DIAGNOSTIC ORDER";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "load", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            humanID = Request.QueryString["HumanID"];
            EncounterID = Request.QueryString["EncounterID"];
            PhysicianID = Request.QueryString["PhysicianId"];

            if (humanID != null && EncounterID != null && PhysicianID != null)
            {
                ClientSession.HumanId = Convert.ToUInt32(humanID);
                ClientSession.EncounterId = Convert.ToUInt32(EncounterID);
                ClientSession.PhysicianId = Convert.ToUInt32(PhysicianID);
            }
            DLC.DName = "pbD";

            Encounter EncRecord;
            FillHumanDTO humanRecord = null;
            CheckOutDTO checkOutDtoLoadList = new CheckOutDTO();
            IList<FillHumanDTO> patientPane = new List<FillHumanDTO>();
           // PhysicianLibrary objPhysician = null;
            IList<string> docPath = new List<string>();
            string PhyName = string.Empty;

            checkOutDtoLoadList = encMngr.CheckOutLoad(ClientSession.HumanId, ClientSession.EncounterId, string.Empty, ClientSession.UserName);
            EncRecord = checkOutDtoLoadList.DocumentList.EncounterObj;
            // humanRecord = checkOutDtoLoadList.objhuman;//For bug_id 37280 
            OrdersManager objOrderMngr = new OrdersManager();
            //GitLab #3811
            //humanRecord = objOrderMngr.GetHumanByIdForCheckout(ClientSession.HumanId);
            humanRecord = objOrderMngr.GetHumanByIdForCheckout(ClientSession.HumanId, ClientSession.EncounterId);

            HiddenhumanDetails.Value = humanRecord.Human_ID.ToString() + "~" + humanRecord.Last_Name + "," + humanRecord.First_Name + "~" + humanRecord.Birth_Date + "~" +
               "" + "~" + humanRecord.Home_Phone_No + "~" + humanRecord.cell_phone_no + "~" + EncRecord.Encounter_Provider_ID.ToString();

            Session["humanRecord"] = humanRecord;
            Session["checkOutDtoLoadList"] = checkOutDtoLoadList;
            Session["EncRecord"] = EncRecord;

            hdnHumanID.Value = ClientSession.HumanId.ToString();
            hdnEncID.Value = ClientSession.EncounterId.ToString();
            hdnEncStatus.Value = ClientSession.UserCurrentProcess;
            hdnFacility.Value = ClientSession.FacilityName;
            hdnPhyID.Value = ClientSession.PhysicianId.ToString();

            sHuman = humanRecord.Last_Name + "," + humanRecord.First_Name + " " + humanRecord.MI + " " + humanRecord.Suffix;
            patientPane.Add(humanRecord);
            //added for consultation and progress notes
            PatientPane obj_PatientPane = new PatientPane();
            obj_PatientPane.First_Name = patientPane[0].First_Name;
            obj_PatientPane.Last_Name = patientPane[0].Last_Name;
            obj_PatientPane.Human_Id = patientPane[0].Human_ID;
            obj_PatientPane.Medical_Record_Number = patientPane[0].Medical_Record_Number;
            obj_PatientPane.MI = patientPane[0].MI;
          //  obj_PatientPane.Cell_Phone_No = patientPane[0].cell_phone_no;

            paneDetails.Add(obj_PatientPane);

            //DLC.txtDLC.Attributes.Add("OnTextChanged", "TextChange()");
            //DLC.txtDLC.Attributes.Add("OnKeyPress", "TextChange()");
            DLC.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
            DLC.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            DLC.txtDLC.Attributes.Add("UserRole", ClientSession.UserRole);



            //objPhysician = phyMngr.GetphysiciannameByPhyID(ClientSession.PhysicianId)[0];
            //For BugID:37284 
            //XDocument xmlPhysician = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
            //ilstPhysician = xmlPhysician.Element("ROOT").Element("PhyList").Elements("Facility").Elements("Physician").Where(aa => aa.Attribute("ID").Value.ToString() == ClientSession.PhysicianId.ToString());
            //if (ilstPhysician != null && ilstPhysician.Count() > 0)
            //{
            //    PhyName = ilstPhysician.Attributes("prefix").First().Value.ToString() + " " + ilstPhysician.Attributes("firstname").First().Value.ToString() + " " + ilstPhysician.Attributes("middlename").First().Value.ToString() + " " + ilstPhysician.Attributes("lastname").First().Value.ToString();// +" " + ilstPhysician.Attributes("suffix").First().Value.ToString();
            //}
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = new PhysicianFacilityMappingList();
            physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");

            if (physicianFacilityMappingList != null && physicianFacilityMappingList.PhysicianFacility != null)
            {
                var physician = physicianFacilityMappingList.PhysicianFacility.SelectMany(x=> x.Physician).FirstOrDefault(y => y.ID == ClientSession.PhysicianId.ToString());
                if (physician != null)
                {

                    PhyName = (physician.prefix ?? "") + " " +
                              (physician.firstname ?? "") + " " +
                              (physician.middlename ?? "") + " " +
                              (physician.lastname ?? "");

                }
            }

            //End
            //if (objPhysician != null)
            //{
            //    PhyName = objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix;
            //}
            hdnPhyName.Value = PhyName;
            objdocument = checkOutDtoLoadList.DocumentList;


            string sPriPlan = string.Empty;
            string sSecPlan = string.Empty;
            string sPriCarrier = string.Empty;
            string sSecCarrier = string.Empty;

            if (patientPane != null && patientPane.Count > 0)
            {
                var pat1 = from p in patientPane where p.Insurance_Type.ToUpper() == "PRIMARY" select p;
                if (pat1.ToList<FillHumanDTO>().Count > 0)
                {
                    sPriPlan = pat1.ToList<FillHumanDTO>()[0].Ins_Plan_Name;
                    sPriCarrier = pat1.ToList<FillHumanDTO>()[0].CarrierName;
                }

                var pat2 = from p in patientPane where p.Insurance_Type.ToUpper() == "SECONDARY" select p;
                if (pat2.ToList<FillHumanDTO>().Count > 0)
                {
                    sSecPlan = pat2.ToList<FillHumanDTO>()[0].Ins_Plan_Name;
                    sSecCarrier = pat2.ToList<FillHumanDTO>()[0].CarrierName;
                }
            }





            lblPatientStrip.Items[0].Text = FillPatientSummaryBarforPatientChart(patientPane[0].Last_Name, patientPane[0].First_Name, patientPane[0].MI, patientPane[0].Suffix, patientPane[0].Birth_Date, patientPane[0].Human_ID, patientPane[0].Medical_Record_Number, patientPane[0].Home_Phone_No, patientPane[0].cell_phone_no , patientPane[0].Sex, patientPane[0].Patient_Status, patientPane[0].SSN, sPriPlan, sPriCarrier, sSecPlan, sSecCarrier);

            lblPatientStrip.Items[0].Text += "   | DOS:" + UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("dd-MMM-yyyy");

            if (patientPane.Count > 0)
            {
                lblPatientStrip.Items[0].Text += "   | Assigned Phy:" + patientPane[0].Assigned_Physician;
            }

            string PanelToolTip = lblPatientStrip.Items[0].Text;

            int indexPri = PanelToolTip.IndexOf("Pri Plan:");
            int indexSec = PanelToolTip.IndexOf("Sec Plan:");
            int indexSSN = PanelToolTip.IndexOf("SSN:");
            if (indexPri != -1)
            {
                lblPatientStrip.ToolTip = PanelToolTip.Insert(indexPri, "\n");
            }
            else if (indexSec != -1)
            {
                lblPatientStrip.ToolTip = PanelToolTip.Insert(indexSec, "\n");
            }
            else if (indexSSN != -1)
            {
                lblPatientStrip.ToolTip = PanelToolTip.Insert(indexSSN, "\n");
            }
            else
            {
                lblPatientStrip.ToolTip = lblPatientStrip.Items[0].Text;
            }
            // Commented for Bug id=28850
            //lblPatientStrip.ToolTip += "   | DOS:" + UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("dd-MMM-yyyy");
            //if (patientPane.Count > 0)
            //{
            //    lblPatientStrip.ToolTip += "   | Assigned Phy:" + patientPane[0].Assigned_Physician;
            //}
            if (!IsPostBack)
            {
                //ClientSession.FlushSession();
                txtDocumentsgivento.BackColor = Color.FromArgb(191, 219, 255);
                txtDueDate.BackColor = Color.FromArgb(191, 219, 255);
                txtFollowReasonNotes.BackColor = Color.FromArgb(191, 219, 255);
                txtDocumentsgivento.BorderWidth = 1;
                txtDueDate.BorderWidth = 1;
                txtFollowReasonNotes.BorderWidth = 1;
                txtDocumentsgivento.BorderColor = Color.Black;
                txtDueDate.BorderColor = Color.Black;
                txtFollowReasonNotes.BorderColor = Color.Black;

                if (EncRecord.Return_In_Days == 0 && EncRecord.Return_In_Months == 0 && EncRecord.Return_In_Weeks == 0)
                {
                    dueDate = EncRecord.Due_On;
                }
                else
                {
                    dueDate = DateTime.Now.AddDays(EncRecord.Return_In_Days + (7 * EncRecord.Return_In_Weeks)).AddMonths(EncRecord.Return_In_Months);
                }
                hdnDuedate.Value = dueDate.ToString();
                if (dueDate.Date != DateTime.MinValue.Date)
                {
                    txtDueDate.Text = dueDate.ToString("dd-MMM-yyyy");

                }
                txtFollowReasonNotes.Text = EncRecord.Follow_Reason_Notes;
                LoadCheckout();


                if (ClientSession.PhysicianId != 0)
                {
                    IList<UserLookup> FieldLookUpList = null;
                    FieldLookUpList = localFieldLookupManager.GetFieldLookupList(Convert.ToUInt64(EncRecord.Encounter_Provider_ID), "PATIENT EDUCATION DOCUMENTS");
                    if (FieldLookUpList != null)
                    {
                        if (FieldLookUpList.Count > 0)
                        {
                            for (int i = 0; i < FieldLookUpList.Count; i++)
                            {
                                ListItem lstItem = new ListItem();
                                lstItem.Text = FieldLookUpList[i].Value;
                                lstItem.Value = FieldLookUpList[i].Description;
                                this.chklstPrintDocuments.Items.Add(lstItem);
                                docPath.Add(FieldLookUpList[i].Description);

                                //this.chklstPrintDocuments.Items.Add(FieldLookUpList[i].Value);
                                //docPath.Add(FieldLookUpList[i].Description);
                            }
                        }
                    }
                }
                IList<StaticLookup> StaticLookUpList;
                StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                StaticLookUpList = objStaticLookupMgr.getStaticLookupByFieldName(new string[] { "SOURCE OF INFORMATION", "DOCUMENT GIVEN", "RELATION_CHECKOUT" });
                IList<StaticLookup> lstSource = StaticLookUpList.Where(q => q.Field_Name == "RELATION_CHECKOUT").ToList();
                if (lstSource != null)
                {
                    cboRelationship.Items.Add(new RadComboBoxItem(""));
                    for (int i = 0; i < lstSource.Count; i++)
                    {
                        cboRelationship.Items.Add(new RadComboBoxItem(lstSource[i].Value));
                    }
                }
                IList<StaticLookup> lstDocuments = StaticLookUpList.Where(q => q.Field_Name == "DOCUMENT GIVEN").ToList();
                if (lstDocuments != null && lstDocuments.Count > 0)
                {
                    //cboIsDocumentGiven.Items.Add(new RadComboBoxItem(""));
                    for (int i = 0; i < lstDocuments.Count; i++)
                    {
                        cboIsDocumentGiven.Items.Add(new RadComboBoxItem(lstDocuments[i].Value));
                    }
                    string[] strDoc = (from d in StaticLookUpList select d.Value).ToArray<string>();
                    cboIsDocumentGiven.DataSource = strDoc;
                    cboIsDocumentGiven.Text = strDoc[0];
                }
                if (objdocument != null)
                {
                    sEncPhyUserName = objdocument.EncPhyuserName;
                }
                if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
                {
                    for (int i = 0; i < objdocument.DocumentsList.Count; i++)
                    {
                        for (int j = 0; j < chklstPrintDocuments.Items.Count; j++)
                        {
                            if (objdocument.DocumentsList[i].Document_Type.ToUpper() == chklstPrintDocuments.Items[j].ToString().ToUpper())
                                chklstPrintDocuments.Items[j].Selected = true;

                        }
                    }
                    for (int i = 0; i < cboRelationship.Items.Count; i++)
                    {
                        if (cboRelationship.Items[i].Text == objdocument.DocumentsList[0].Relationship)
                        {
                            cboRelationship.SelectedIndex = i;
                        }

                    }
                    txtDocumentsgivento.Text = objdocument.DocumentsList[0].Given_To;

                }
                else
                {
                    if (humanRecord != null)
                    {
                        for (int i = 0; i < cboRelationship.Items.Count; i++)
                        {
                            if (cboRelationship.Items[i].Text == "Self")
                            {
                                cboRelationship.SelectedIndex = i;
                            }

                        }
                        txtDocumentsgivento.Text = sHuman;

                    }
                }
                if (objdocument != null && objdocument.EncounterObj != null)
                {
                    EncRecord = objdocument.EncounterObj;
                    if (EncRecord.Is_Document_Given != string.Empty)
                    {
                        for (int i = 0; i < cboIsDocumentGiven.Items.Count; i++)
                        {
                            if (cboIsDocumentGiven.Items[i].Text == EncRecord.Is_Document_Given)
                            {
                                cboIsDocumentGiven.SelectedIndex = i;
                            }

                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "unloadwait", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {
                LoadCheckout();
            }
            Is_AutoSave = false;

            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);

        }
        public string FillPatientSummaryBarforPatientChart(string LastName, string FirstName, string MI, string Suffix, DateTime DOB, ulong ulHumanID, string MedRecNo, string HomePhoneNo, string CellPhoneNo, string Sex, string PatientStatus, string SSN, string sPriPlan, string sPriCarrier, string sSecPlan, string sSecCarrier)
        {

            string sMySummary = "";
            string phoneno = "";
            if(HomePhoneNo.Length == 14)
            {
                phoneno = HomePhoneNo;
            }
            else
            {
                phoneno = CellPhoneNo;
            }
            

            if (Sex != "")
            {
                if (PatientStatus == "DECEASED")
                {
                    sMySummary = LastName + "," + FirstName +
                       "  " + MI + "  " + Suffix + "   |   " +
                       DOB.ToString("dd-MMM-yyyy") + "   |   " +
                       (CalculateAge(DOB)).ToString() +
                       "  year(s)    |   " + Sex.Substring(0, 1) + "   |   " + PatientStatus + "   |   Acc #:" + ulHumanID +
                       "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
                       "Phone #:" + phoneno + "   |   ";
                }
                else
                {
                    sMySummary = LastName + "," + FirstName +
                   "  " + MI + "  " + Suffix + "   |   " +
                   DOB.ToString("dd-MMM-yyyy") + "   |   " +
                   (CalculateAge(DOB)).ToString() +
                   "  year(s)    |   " + Sex.Substring(0, 1) + "   |   Acc #:" + ulHumanID +
                   "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
                   "Phone #:" + phoneno + "   |   ";

                }
            }

            if (sPriPlan != string.Empty)
            {
                sMySummary += "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "   |   ";
            }
            if (sSecPlan != string.Empty)
            {
                sMySummary += "Sec Plan:" + sSecCarrier + " - " + sSecPlan + "   |   ";
            }
            if (SSN != string.Empty)
            {
                sMySummary += "SSN:" + SSN + "   |   ";
            }

            return sMySummary;
        }
        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }
        private void LoadCheckout()
        {
            CheckOutDTO checkOutDtoLoadList = new CheckOutDTO();
            if (Session["checkOutDtoLoadList"] != null)
            {
                checkOutDtoLoadList = (CheckOutDTO)Session["checkOutDtoLoadList"];
                if (checkOutDtoLoadList != null)
                {
                    DLC.txtDLC.Text = checkOutDtoLoadList.DocumentList.EncounterObj.Check_Out_Notes;
                    FillOrdersGrid();
                }
            }
        }
        private void FillOrdersGrid()
        {
            CheckOutDTO checkOutDtoLoadList = new CheckOutDTO();
            if (Session["checkOutDtoLoadList"] != null)
            {
                checkOutDtoLoadList = (CheckOutDTO)Session["checkOutDtoLoadList"];
            }
            if (dtTable.Rows.Count > 0)
                dtTable.Rows.Clear();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("Order Type");
                dtTable.Columns.Add("Procedure/Rx/Referral");
                dtTable.Columns.Add("Lab/Facility");
                dtTable.Columns.Add("Location");
                dtTable.Columns.Add("Order ID");
                dtTable.Columns.Add("Required Specimen");
                dtTable.Columns.Add("Is Specimen In House");
                dtTable.Columns.Add("Is Specimen Collected");
                dtTable.Columns.Add("Order Submit ID");
                dtTable.Columns.Add("ModifiedDateTime");
                dtTable.Columns.Add("Auth. Req.");
            }

            Hashtable labRowIndex = new Hashtable();
            Hashtable imageRowIndex = new Hashtable();
            //int rowIndex = 0;


            var TotalSubmitedOrders = (from rec in checkOutDtoLoadList.CheckoutOrdersList select rec.Order_ID).Distinct();
            foreach (var submitID in TotalSubmitedOrders)
            {

                CheckOutDTO tagObj = new CheckOutDTO();
                IList<CheckoutOrdersDTO> OrderLabDetailsDTOBag = (from rec3 in checkOutDtoLoadList.CheckoutOrdersList where rec3.Order_ID == submitID select rec3).ToList<CheckoutOrdersDTO>();
                IList<ulong> OrdersIDCorrespondToSub = (from rec3 in checkOutDtoLoadList.CheckoutOrdersList where rec3.Order_ID == submitID select rec3.Order_ID).Distinct().ToList();


                tagObj.CheckoutOrdersList = checkOutDtoLoadList.CheckoutOrdersList;
                StringBuilder ICD = new StringBuilder();
                string LabProcedure = string.Empty;
                string ReasonforReferral = string.Empty;
                string SpecimenInHouse = string.Empty;
                IList<CheckoutOrdersDTO> ForCorrespondingSubID = (from rec2 in checkOutDtoLoadList.CheckoutOrdersList where rec2.Order_ID == submitID select rec2).ToList<CheckoutOrdersDTO>();
                string SpecimenType = string.Empty;
                string SpecimenQty = string.Empty;
                string AuthReq = ForCorrespondingSubID[0].Authorization_Required;
                string File_Path = string.Empty;

                IList<string> dupcheck = new List<string>();
                string sTestCode = string.Empty;
                string sICD = string.Empty;
                foreach (CheckoutOrdersDTO objpro in ForCorrespondingSubID)
                {
                    LabProcedure += "  ,  " + objpro.Procedure_Code.Trim() + "-" + objpro.Procedure_Code_Description.Trim();
                    sTestCode += objpro.Procedure_Code.Trim() + ",";

                    if (objpro.Order_Type == "REFERRAL ORDER")
                    {
                        ReasonforReferral += "  ,  " + objpro.Reason_For_Referral.Trim();
                        sTestCode += objpro.Reason_For_Referral.Trim() + ",";
                    }

                }
                SpecimenInHouse = ForCorrespondingSubID[0].Specimen_In_House;

                dr = dtTable.NewRow();

                if (ForCorrespondingSubID[0].Order_Type == "DIAGNOSTIC ORDER" || ForCorrespondingSubID[0].Order_Type == "DME ORDER" || ForCorrespondingSubID[0].Order_Type == "IMAGE ORDER" || ForCorrespondingSubID[0].Order_Type == "IMMUNIZATION ORDER")
                {
                    dr["Order Type"] = ForCorrespondingSubID[0].Order_Type;
                    dr["Procedure/Rx/Referral"] = LabProcedure.Substring(4).Trim();
                    dr["Lab/Facility"] = ForCorrespondingSubID[0].labName;
                    dr["Location"] = ForCorrespondingSubID[0].labLocName;
                    dr["Order ID"] = ForCorrespondingSubID[0].Order_ID.ToString(); ;
                    dr["Required Specimen"] = ForCorrespondingSubID[0].specimen;
                    dr["Is Specimen In House"] = ForCorrespondingSubID[0].Specimen_In_House;
                    if (ForCorrespondingSubID[0].specimen_ID != 0)
                    {
                        dr["Is Specimen Collected"] = "Y";
                    }
                    else
                    {
                        dr["Is Specimen Collected"] = "N";
                    }
                    dr["ModifiedDateTime"] = ForCorrespondingSubID[0].Modified_Date_And_Time;
                    dr["Auth. Req."] = AuthReq;
                }
                else if (ForCorrespondingSubID[0].Order_Type == "REFERRAL ORDER")
                {
                    dr["Order Type"] = ForCorrespondingSubID[0].Order_Type;
                    dr["Procedure/Rx/Referral"] = "Referred  for " + ReasonforReferral.Substring(4).Trim();
                    dr["Lab/Facility"] = ForCorrespondingSubID[0].To_Facility_Name;
                    dr["Order ID"] = ForCorrespondingSubID[0].Order_ID.ToString(); ;
                    dr["ModifiedDateTime"] = ForCorrespondingSubID[0].Modified_Date_And_Time;
                    dr["Auth. Req."] = AuthReq;
                }
                else if (ForCorrespondingSubID[0].Order_Type == "INTERNAL ORDER")
                {
                    dr["Order Type"] = ForCorrespondingSubID[0].Order_Type;
                    dr["Procedure/Rx/Referral"] = LabProcedure.Substring(4).Trim();
                    dr["Lab/Facility"] = ForCorrespondingSubID[0].To_Facility_Name;
                    dr["Order ID"] = ForCorrespondingSubID[0].Order_ID.ToString(); ;
                    dr["ModifiedDateTime"] = ForCorrespondingSubID[0].Modified_Date_And_Time;
                    dr["Auth. Req."] = AuthReq;
                }
                dtTable.Rows.Add(dr);
            }
            if (ds.Tables.Count == 0)
                ds.Tables.Add(dtTable);
            grdorders.DataSource = ds.Tables[0];
            grdorders.DataBind();
        }

        protected void btnPrintLabOrder_Click(object sender, EventArgs e)
        {
            FillOrdersGrid();
            OrdersManager objOrderProxy = new OrdersManager();
            PhysicianLibrary objPhysician = null;
            FillHumanDTO humanRecord = null;
            hdnSelectedItem.Value = string.Empty;
            if (Session["humanRecord"] != null)
                humanRecord = (FillHumanDTO)Session["humanRecord"];
            CheckOutPrintOrdersDTO checkOutPrintOrdersDTO = new CheckOutPrintOrdersDTO();
            //Added for BugID:37287 
            if (ilstPhysician != null && ilstPhysician.Count() > 0)
            {
                objPhysician = new PhysicianLibrary();
                objPhysician.Id = Convert.ToUInt32(ilstPhysician.Attributes("ID").First().Value.ToString());
                objPhysician.PhyPrefix = ilstPhysician.Attributes("prefix").First().Value.ToString();
                objPhysician.PhyFirstName = ilstPhysician.Attributes("firstname").First().Value.ToString();
                objPhysician.PhyMiddleName = ilstPhysician.Attributes("middlename").First().Value.ToString();
                objPhysician.PhyLastName = ilstPhysician.Attributes("lastname").First().Value.ToString();
                objPhysician.PhySuffix = ilstPhysician.Attributes("suffix").First().Value.ToString();
            }
            //End
            //objPhysician = phyMngr.GetphysiciannameByPhyID(ClientSession.PhysicianId)[0];
            checkOutPrintOrdersDTO = objOrderProxy.GetOrdersForCheckoutPrint(ClientSession.EncounterId, ClientSession.HumanId);
            if (checkOutPrintOrdersDTO.ImmunizationDTOobj.Immunization.Count > 0 || checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO.Count > 0 || checkOutPrintOrdersDTO.Order_Details_Dto_Image_Order_List.ilstOrderLabDetailsDTO.Count > 0 || checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList.Count > 0)
            {
                if (checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO.Count > 0)
                {
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    string FileLocation = print.CallPrintLabAndImageOrders(TargetFileDirectory, ClientSession.EncounterId, null, humanRecord, OrderType);
                    string[] Split = new string[] { Server.MapPath("") };
                    string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < FileName.Length; i++)
                    {
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            //hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[i].ToString();//changed by naveena for bug_id 26491 
                            hdnSelectedItem.Value = FileName[i].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName[i].ToString();
                        }
                    }
                    string FaxSubject = string.Empty;
                    if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                    {
                        if (checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO != null)
                        {
                            if (checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO.Count > 0)
                            {
                                FaxSubject = "Order_" + humanRecord.First_Name + " " + humanRecord.Last_Name + "_" + checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");

                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDF('" + FaxSubject + "');", true);
                }
                if (checkOutPrintOrdersDTO.Order_Details_Dto_Image_Order_List.ilstOrderLabDetailsDTO.Count > 0)
                {
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    string FileLocation = print.PrintLabAndImageOrders(checkOutPrintOrdersDTO.Order_Details_Dto_Image_Order_List, Convert.ToUInt64(objPhysician.Id), humanRecord, "IMAGE ORDER", TargetFileDirectory);
                    string[] Split = new string[] { Server.MapPath("") };
                    string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < FileName.Length; i++)
                    {
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            //hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[i].ToString();
                            hdnSelectedItem.Value = FileName[i].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName[i].ToString();
                        }
                    }
                    string FaxSubject = string.Empty;
                    if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                    {
                        if (checkOutPrintOrdersDTO.Order_Details_Dto_Image_Order_List.ilstOrderLabDetailsDTO != null)
                        {
                            if (checkOutPrintOrdersDTO.Order_Details_Dto_Image_Order_List.ilstOrderLabDetailsDTO.Count > 0)
                            {
                                FaxSubject = "Order_" + humanRecord.First_Name + " " + humanRecord.Last_Name + "_" + checkOutPrintOrdersDTO.Order_Details_Dto_Image_Order_List.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");

                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDF('" + FaxSubject + "');", true);
                }
                if (checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO.Count > 0)
                {
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    string FileLocation = string.Empty;
                    IList<string> LabAndOrderSubmitID = objOrderProxy.GetOrderSubmitIDForPrintRequestion(ClientSession.EncounterId, Convert.ToUInt64(objPhysician.Id), ClientSession.HumanId, OrderType);
                    foreach (string str in LabAndOrderSubmitID)
                    {
                        if (str.StartsWith("1"))
                            FileLocation = print.PrintRequisitionUsingDatafromDB(Convert.ToUInt32(str.Split('|')[1]), TargetFileDirectory, "ORDERS", ClientSession.EncounterId, OrderType);
                        else
                            FileLocation = print.PrintSplitRequisitionUsingDatafromDBQuest(Convert.ToUInt32(str.Split('|')[1]), TargetFileDirectory, "ORDERS", false, OrderType);

                        string[] Split1 = new string[] { Server.MapPath("") };
                        string[] FileName1 = FileLocation.Split(Split1, StringSplitOptions.RemoveEmptyEntries);
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = FileName1[0].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName1[0].ToString();
                        }
                    }
                    string FaxSubject = string.Empty;
                    if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                    {
                        if (checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO != null)
                        {
                            if (checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO.Count > 0)
                            {
                                FaxSubject = "Order_" + humanRecord.First_Name + " " + humanRecord.Last_Name + "_" + checkOutPrintOrdersDTO.Order_Details_Dto_Lab_Order_List.ilstOrderLabDetailsDTO[0].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy");

                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDF('" + FaxSubject + "');", true);
                }
                if (checkOutPrintOrdersDTO.ImmunizationDTOobj.Immunization.Count > 0)
                {
                    Human objHuman = print.GetHumanByHumanID(humanRecord.Human_ID);
                    string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                    string FileLocation = print.PrintImmunizationOrders(checkOutPrintOrdersDTO.ImmunizationDTOobj, objPhysician, objHuman, TargetFileDirectory);
                    string[] Split = new string[] { Server.MapPath("Documents/" + Session.SessionID) };
                    string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < FileName.Length; i++)
                    {
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[i].ToString();
                            //hdnSelectedItem.Value = FileName[i].ToString();
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + FileName[i].ToString();
                        }
                    }
                    string FaxSubject = string.Empty;
                    if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                    {
                        if (checkOutPrintOrdersDTO.ImmunizationDTOobj.Immunization != null)
                        {
                            if (checkOutPrintOrdersDTO.ImmunizationDTOobj.Immunization.Count > 0)
                            {
                                FaxSubject = "Order_" + objHuman.First_Name + " " + objHuman.Last_Name + "_" + checkOutPrintOrdersDTO.ImmunizationDTOobj.Immunization[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");

                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDF('" + FaxSubject + "');", true);
                }
                if (checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList.Count > 0)
                {
                    var distinctPhyList = (from obj in checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList select new { obj.Referral_Specialty }).Distinct();
                    foreach (var PhyID in distinctPhyList)
                    {
                        IList<ReferralOrder> refList = (from obj in checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList where obj.Referral_Specialty == PhyID.Referral_Specialty select obj).ToList<ReferralOrder>();
                        string TargetFileDirectory = Server.MapPath("Documents/" + Session.SessionID);
                        string FileLocation = print.PrintReferralOrders(refList, ClientSession.PhysicianId, checkOutPrintOrdersDTO.Referral_Order_Dto, humanRecord, PhyID.Referral_Specialty, TargetFileDirectory);

                        string[] Split = new string[] { Server.MapPath("Documents/" + Session.SessionID) };
                        string[] FileName = FileLocation.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < FileName.Length; i++)
                        {
                            if (hdnSelectedItem.Value == string.Empty)
                            {
                                hdnSelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + FileName[i].ToString();
                                //hdnSelectedItem.Value = FileName[i].ToString();
                            }
                            else
                            {
                                hdnSelectedItem.Value += "|" + FileName[i].ToString();
                            }
                        }
                    }
                    string FaxSubject = string.Empty;
                    if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
                    {
                        if (checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList != null)
                        {
                            if (checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList.Count > 0)
                            {
                                FaxSubject = "Order_" + humanRecord.First_Name + " " + humanRecord.Last_Name + "_" + checkOutPrintOrdersDTO.Referral_Order_Dto.RefOrdList[0].Created_Date_And_Time.ToString("dd-MMM-yyyy");

                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDF('" + FaxSubject + "');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Checkout", "DisplayErrorMessage('430009');", true);
                return;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        //protected void btnCheckOut_Click(object sender, EventArgs e)
        //{
        //    if (txtDueDate.Text != string.Empty || txtFollowReasonNotes.Text != string.Empty)
        //    {
        //        if (editAppt != null)
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CellSelected();", true);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CellSelected();", true);
        //        }
        //        hdnMessageType.Value = string.Empty;
        //    }
        //    else
        //    {
        //        hdnIsCheckout.Value = "true";
        //        btnFollowupAppointment_Click(sender, e);
        //    }

        //}

        protected void btnFollowupAppointment_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);

            hdnAppointment.Value = "";
            if (hdnIsCheckout.Value == "true")
            {
                Encounter EncRecord = new Encounter();
                if (Session["EncRecord"] != null)
                    EncRecord = (Encounter)Session["EncRecord"];
                EncounterManager checkOutProxy = new EncounterManager();
                IList<Documents> SaveList = new List<Documents>();
                IList<Documents> Updatelist = new List<Documents>();
                IList<Documents> Deletelist = new List<Documents>();

                // EncRecord.Check_Out_Notes = DLC.txtDLC.Text;
                EncRecord.Check_Out_Notes = Request.Form[DLC.ID + "$txtDLC"].Trim();
                EncRecord.Modified_By = ClientSession.UserName;
                if (hdnLocalTime.Value != string.Empty)
                    EncRecord.Modified_Date_and_Time = Convert.ToDateTime(hdnLocalTime.Value); //UtilityManager.ConvertToUniversal();

                var totalcount = chklstPrintDocuments.Items.Cast<ListItem>().Where(item => item.Selected);

                //IEnumerable<string> totalcount = chklstPrintDocuments.Items.Cast<ListItem>()
                //                   .Where(i => i.Selected)
                //                   .Select(i => i.Text);


                if (chklstPrintDocuments.Items.Count > 0 && totalcount.Count() > 0)
                {
                    Documents objDocument = null;

                    for (int i = 0; i < chklstPrintDocuments.Items.Count; i++)
                    {
                        objDocument = new Documents();

                        if (chklstPrintDocuments.Items[i].Selected)
                        {

                            var doc = (from d in objdocument.DocumentsList
                                       where d.Document_Type.ToUpper() == chklstPrintDocuments.Items[i].Text.ToUpper()
                                       select d);
                            if (doc.Count() > 0)
                            {
                                objDocument = doc.ToList<Documents>()[0];
                                objDocument.Created_By = ClientSession.UserName;
                                if (hdnLocalTime.Value != string.Empty)
                                    objDocument.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value); //UtilityManager.ConvertToUniversal();
                                objDocument.Given_To = txtDocumentsgivento.Text;
                                objDocument.Relationship = cboRelationship.Text;
                                Updatelist.Add(objDocument);
                            }
                            else
                            {
                                objDocument.Encounter_ID = ClientSession.EncounterId;
                                objDocument.Human_ID = ClientSession.HumanId;
                                objDocument.Physician_ID = ClientSession.PhysicianId;
                                objDocument.Relationship = cboRelationship.Text;
                                objDocument.Created_By = ClientSession.UserName;
                                if (hdnLocalTime.Value != string.Empty)
                                    objDocument.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value); //UtilityManager.ConvertToUniversal();
                                objDocument.Document_Type = chklstPrintDocuments.Items[i].Text.ToString();
                                objDocument.Given_To = txtDocumentsgivento.Text;
                                SaveList.Add(objDocument);
                            }
                        }

                    }
                    //if (chklstPrintDocuments.Items.Count > 0 && totalcount.Count() > 0)
                    //{

                    //    Documents objDocument = null;
                    //    for (int i = 0; i < totalcount.Count(); i++)
                    //    {
                    //        var doc = (from d in objdocument.DocumentsList
                    //                   where d.Document_Type.ToUpper() == chklstPrintDocuments.Items[i].Selected.ToString().ToUpper() 
                    //                   select d);

                    //        objDocument = new Documents();
                    //        if (doc.Count() > 0)
                    //        {
                    //            objDocument = doc.ToList<Documents>()[0];
                    //            objDocument.Created_By = ClientSession.UserName;
                    //            if (hdnLocalTime.Value != string.Empty)
                    //                objDocument.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value); //UtilityManager.ConvertToUniversal();
                    //            objDocument.Given_To = txtDocumentsgivento.Text;
                    //            objDocument.Relationship = cboRelationship.Text;
                    //            Updatelist.Add(objDocument);
                    //        }
                    //        else
                    //        {
                    //            objDocument.Encounter_ID = ClientSession.EncounterId;
                    //            objDocument.Human_ID = ClientSession.HumanId;
                    //            objDocument.Physician_ID = ClientSession.PhysicianId;
                    //            objDocument.Relationship = cboRelationship.Text;
                    //            objDocument.Created_By = ClientSession.UserName;
                    //            if (hdnLocalTime.Value != string.Empty)
                    //                objDocument.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value); //UtilityManager.ConvertToUniversal();
                    //            objDocument.Document_Type = chklstPrintDocuments.Items[i].Text.ToString();
                    //            objDocument.Given_To = txtDocumentsgivento.Text;
                    //            SaveList.Add(objDocument);
                    //        }
                    //    }
                    if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
                    {
                        //for (int i = 0; i < Updatelist.Count; i++)
                        //{
                        //  bool bResult = false;
                        //  var totalcount1 = chklstPrintDocuments.Items.Cast<ListItem>().Where(item => item.Selected);
                        if (Updatelist.Count > 0)
                        {
                            for (int j = 0; j < objdocument.DocumentsList.Count; j++)
                            {


                                var doc = (from d in Updatelist
                                           where d.Document_Type.ToUpper() == objdocument.DocumentsList[j].Document_Type.ToUpper()
                                           select d);

                                if (doc.Count() == 0)
                                {
                                    objdocument.DocumentsList[j].Given_By = ClientSession.UserName;
                                    Deletelist.Add(objdocument.DocumentsList[j]);
                                }

                            }
                        }
                        //if (bResult)
                        //{

                        // }



                        //}
                    }
                    EncRecord.Is_Document_Given = cboIsDocumentGiven.Text;


                }
                if (EncounterID != "")
                    EncRecord.Encounter_ID = Convert.ToUInt32(EncounterID);
                if (hdnIsFormClosed.Value == "true")
                {
                    checkOutProxy.BatchOperationsOnCheckOut(SaveList.ToArray<Documents>(), Updatelist.ToArray<Documents>(), Deletelist.ToArray<Documents>(), EncRecord, ClientSession.HumanId, ClientSession.EncounterId, string.Empty, ClientSession.UserName, Convert.ToDateTime(hdnLocalTime.Value), true);
                }
                else
                {
                    checkOutProxy.BatchOperationsOnCheckOut(SaveList.ToArray<Documents>(), Updatelist.ToArray<Documents>(), Deletelist.ToArray<Documents>(), EncRecord, ClientSession.HumanId, ClientSession.EncounterId, string.Empty, ClientSession.UserName, Convert.ToDateTime(hdnLocalTime.Value), false);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "DisplayErrorMessage('430001');", true);
                if (is_FormClosed == false || is_FormClosed == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "btnClose_Clicked();", true);
                }
                hdnMessageType.Value = string.Empty;

            }
            else
            {
                if (txtDueDate.Text != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup", "OpenfollowupAppointment();", true);
                }
                else
                {
                    hdnDuedate.Value = hdnLocalTime.Value.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup", "OpenfollowupAppointment();", true);
                }

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnPrintDocuments_Click(object sender, EventArgs e)
        {

            if (HiddenDLC.Value != "")
                DLC.txtDLC.Text = HiddenDLC.Value;
            FillOrdersGrid();
            hdnNotes.Value = string.Empty;
            Encounter EncRecord = new Encounter();
            if (Session["EncRecord"] != null)
                EncRecord = (Encounter)Session["EncRecord"];
            hdnSelectedItem.Value = string.Empty;
            string filesNotFound = string.Empty;
            string screen = string.Empty;
            hdnXmlPath.Value = string.Empty;
            //IList<string> SelectedItems = chklstPrintDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text).ToList<string>();
            // IList<string> SelectedItems = chklstPrintDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text).ToList<string>();
            //IList<string> SelectedItems = chklstPrintDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Value.ToString().Replace(".pdf","")).ToList<string>();

            //string[] GetFiles = Directory.GetFiles(Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education"));
            //string[] Separator = new string[] { Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\") };
            // string[] GetFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath + "Documents\\Physician_Specific_Documents\\Patient Education\\");

            // string[] Separator = new string[] { HostingEnvironment.ApplicationPhysicalPath + "Documents\\Physician_Specific_Documents\\Patient Education\\" };
            //  IList<string> FilesNotFound = new List<string>();

            IList<string> SelectedItems = chklstPrintDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Value + "|" + b.Text).ToList<string>();
            //IList<string> SelectedItems = chklstSelectDocuments.Items.Cast<ListItem>().Where(a => a.Selected).Select(b => b.Text).ToList<string>();

          

            string[] GetFiles = Directory.GetFiles(Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education"));
            string[] Separator = new string[] { Server.MapPath("Documents\\Physician_Specific_Documents\\Patient Education\\") };

            IList<string> FilesNotFound = new List<string>();

            if (SelectedItems.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "DisplayErrorMessage('110805'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                return;
            }
            //comment for bug id: 37187
            //foreach (string s in GetFiles)
            //{
            //    string[] SplitedDocName = s.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            //   FilesNotFound.Add(SplitedDocName[0].ToString().Replace(".pdf", ""));
            //}

            if (GetFiles != null && GetFiles.Length > 0) //code added by balaji.TJ 2015-26-11
            {
                foreach (string s in GetFiles)
                {
                    string[] SplitedDocName = s.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
                    if (SplitedDocName.Length > 0)  //code added by balaji.TJ 2015-28-11
                        FilesNotFound.Add(SplitedDocName[0].ToString());
                }
            }

            //Encounter EncRecord = new Encounter();
            //FillDocuments objdocument = DocumentMngr.GetDocumentsList(ClientSession.EncounterId);

            IList<Documents> SaveList = new List<Documents>();
            IList<Documents> Updatelist = new List<Documents>();
            IList<Documents> Deletelist = new List<Documents>();
            string DownloadDoc = string.Empty;
            Documents objDocument = null;
            if (SelectedItems != null && SelectedItems.Count > 0) //code added by balaji.TJ 2015-11-26
            {
                for (int i = 0; i < SelectedItems.Count; i++)
                {

                    if (SelectedItems[i].ToString().ToUpper().Contains("PROGRESS NOTE"))
                    {
                        //frmNewProgressNotes obj = new frmNewProgressNotes();
                        //obj.ShowProgressNotes(ClientSession.Selectedencounterid, true, true, new Acurus.Capella.Core.DomainObjects.Encounter(),hdnLocalTime.Value);
                        if (hdnNotes.Value == string.Empty)
                        {
                            hdnNotes.Value = "Pro";
                        }
                        else
                        {
                            hdnNotes.Value += "|Pro";
                        }

                    }
                    else if (SelectedItems[i].ToString().ToUpper().Contains("CONSULTATION NOTE"))
                    {
                        //string sMyPath = string.Empty;
                        //frmConsultationNotes obj = new frmConsultationNotes();

                        //obj.ShowProgressNotes(ClientSession.Selectedencounterid, true, new Acurus.Capella.Core.DomainObjects.Encounter(), new System.Collections.Generic.List<Acurus.Capella.Core.DTO.PatientPane>(), hdnLocalTime.Value);
                        if (hdnNotes.Value == string.Empty)
                        {
                            hdnNotes.Value = "Con";
                        }
                        else
                        {
                            hdnNotes.Value += "|Con";
                        }

                    }
                    //else if (SelectedItems[i].ToString().ToUpper() == "CLINICAL SUMMARY")
                    //{
                    //    string sMyPath = string.Empty;
                    //    frmClinicalSummary objfrmClinical = new frmClinicalSummary();
                    //    ArrayList FileLocation = objfrmClinical.PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, false, ref sMyPath, string.Empty, true, false);
                    //   // ArrayList FileLocation = PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, true, ref sMyPath, string.Empty, false, false);

                    //    hdnPrintFilePath.Value = string.Empty;                   
                    //    string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                    //    if (FileLocation[0].ToString().EndsWith(".xml") == true)
                    //    {

                    //        if (hdnXmlPath.Value == string.Empty)
                    //        {
                    //            string[] XMLFileName = FileLocation[0].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    //            if (hdnXmlPath.Value == string.Empty)
                    //            {
                    //                hdnXmlPath.Value = "Documents\\" + Session.SessionID.ToString() + XMLFileName[0].ToString();
                    //            }
                    //            if (hdnXmlPath.Value != string.Empty)
                    //            {
                    //                DirectoryInfo ObjSearchDir = new DirectoryInfo(Server.MapPath(hdnXmlPath.Value));
                    //                if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
                    //                {
                    //                    Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
                    //                }
                    //                System.IO.File.Copy(Server.MapPath("SampleXML/CDA.xsl"), Server.MapPath("Documents/" + Session.SessionID.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl"), true);

                    //            }

                    //        }
                    //    }

                    //}

                    else if (SelectedItems[i].ToString().Split('|')[1].ToUpper() == "CLINICAL SUMMARY")
                    {
                        string sMyPath = string.Empty;
                        frmClinicalSummary objfrmClinical = new frmClinicalSummary();
                        ArrayList FileLocation = objfrmClinical.PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, false, ref sMyPath, string.Empty, true, false);
                        // ArrayList FileLocation = PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, true, ref sMyPath, string.Empty, false, false);

                        hdnPrintFilePath.Value = string.Empty;
                        hdnXmlPath.Value = string.Empty;

                        string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                        if (FileLocation[0].ToString().EndsWith(".xml") == true)
                        {
                            if (hdnXmlPath.Value == string.Empty)
                            {
                                string[] XMLFileName = new string[] { };
                                if (Split != null && Split.Length > 0)
                                    XMLFileName = FileLocation[0].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);
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

                    else if (SelectedItems[i].Split('|')[1].ToUpper() == "WELLNESS NOTE")
                    {
                        string sMyPath = string.Empty;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Open", "wellnessnote(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

                    }
                    else if (SelectedItems[i].ToString().Split('|')[0].ToUpper().Contains(".DO"))
                    {
                        if (DownloadDoc != null && DownloadDoc != "")
                        {
                            DownloadDoc = DownloadDoc + "|" + SelectedItems[i].ToString().Split('|')[0];
                        }
                        else
                        {
                            DownloadDoc = SelectedItems[i].ToString().Split('|')[0];
                        }
                    }
                    //else
                    //{
                    //    if (!FilesNotFound.Any(a => a.ToString() == SelectedItems[i].ToString()))
                    //    {
                    //        if (filesNotFound == string.Empty)
                    //        {
                    //            filesNotFound = SelectedItems[i].ToString();
                    //        }
                    //        else
                    //        {
                    //            filesNotFound += " , " + SelectedItems[i].ToString();
                    //        }

                    //        continue;
                    //    }
                    //    if (hdnSelectedItem.Value == string.Empty)
                    //    {
                    //        hdnSelectedItem.Value = SelectedItems[i].ToString();
                    //    }
                    //    else
                    //    {
                    //        hdnSelectedItem.Value += "|" + SelectedItems[i].ToString();
                    //    }

                    //}
                    else
                    {
                        if (!FilesNotFound.Any(a => a.ToString() == SelectedItems[i].ToString().Split('|')[0].ToString()))
                        {
                            if (filesNotFound == string.Empty)
                            {
                                filesNotFound = SelectedItems[i].ToString().Split('|')[1];
                            }
                            else
                            {
                                filesNotFound += " , " + SelectedItems[i].ToString().Split('|')[1];
                            }

                            continue;
                        }
                        if (hdnSelectedItem.Value == string.Empty)
                        {
                            hdnSelectedItem.Value = SelectedItems[i].ToString().Replace(".pdf", "").Split('|')[0];
                        }
                        else
                        {
                            hdnSelectedItem.Value += "|" + SelectedItems[i].ToString().Replace(".pdf", "").Split('|')[0];
                        }

                    }
                }
            }

           // if (hdnSelectedItem.Value == string.Empty)
            if (SelectedItems.Count>0)
            {
                if (filesNotFound != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessageList('110806','" + filesNotFound + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    cboRelationship.Enabled = true;
                }
            }



            for (int i = 0; i < SelectedItems.Count; i++)
            {
                var doc = (from d in objdocument.DocumentsList where d.Document_Type.ToUpper() == SelectedItems[i].Split('|')[1].ToString().ToUpper() select d);

                objDocument = new Documents();
                if (doc != null && doc.Count() > 0) //code added by balaji.TJ 2015-28-11
                {

                    objDocument = doc.ToList<Documents>()[0];
                    objDocument.Given_To = objDocument.Given_To = txtDocumentsgivento.Text;

                    objDocument.Relationship = cboRelationship.Text;
                    objDocument.Given_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objDocument.Given_Date = Convert.ToDateTime(hdnLocalTime.Value);
                    }//UtilityManager.ConvertToUniversal();
                    Updatelist.Add(objDocument);
                }
                else
                {
                    objDocument.Encounter_ID = ClientSession.EncounterId;
                    objDocument.Human_ID = ClientSession.HumanId;
                    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    {
                        objDocument.Physician_ID = ClientSession.PhysicianId;
                    }
                    else
                    {
                        objDocument.Physician_ID = Convert.ToUInt64(ClientSession.PhysicianId);
                    }
                    objDocument.Relationship = cboRelationship.Text;
                    objDocument.Created_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                    {
                        objDocument.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    }//UtilityManager.ConvertToUniversal();
                    objDocument.Document_Type = SelectedItems[i].ToString();
                    objDocument.Given_To = txtDocumentsgivento.Text;
                    objDocument.Given_By = ClientSession.UserName;
                    if (hdnLocalTime.Value != string.Empty)
                        objDocument.Given_Date = Convert.ToDateTime(hdnLocalTime.Value);//UtilityManager.ConvertToUniversal();
                    SaveList.Add(objDocument);
                }
            }




            if (objdocument != null && objdocument.DocumentsList != null && objdocument.DocumentsList.Count > 0)
            {
                for (int i = 0; i < objdocument.DocumentsList.Count; i++)
                {
                    bool bResult = false;
                    for (int j = 0; j < SelectedItems.Count; j++)
                    {
                        if (SelectedItems[j].Split('|')[1].ToString().ToUpper() == objdocument.DocumentsList[i].Document_Type.ToUpper())
                        {
                            bResult = true;
                            break;
                        }

                    }
                    if (!bResult)
                    {
                        objdocument.DocumentsList[i].Given_By = ClientSession.UserName;
                        Deletelist.Add(objdocument.DocumentsList[i]);
                    }

                }
            }


            EncRecord.Is_Document_Given = cboIsDocumentGiven.Text;
            EncRecord.Modified_By = ClientSession.UserName;
            if (hdnLocalTime.Value != string.Empty)
            {
                EncRecord.Modified_Date_and_Time = Convert.ToDateTime(hdnLocalTime.Value);
            }
            EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            //EncRecord.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
            objdocument = objDocumentProxy.SaveUpdateDeleteDocument(SaveList.ToArray<Documents>(), Updatelist.ToArray<Documents>(), Deletelist.ToArray<Documents>(), EncRecord, ClientSession.EncounterId, string.Empty, true);
            if (hdnSelectedItem.Value != string.Empty || DownloadDoc!=string.Empty)
            {
                //MessageWindow.Visible = true;
                //MessageWindow.VisibleOnPageLoad = true;
                //MessageWindow.Height = Unit.Pixel(700);
                //MessageWindow.Width = Unit.Pixel(830);
                //MessageWindow.VisibleStatusbar = false;
                //MessageWindow.KeepInScreenBounds = true;

                //MessageWindow.NavigateUrl = "frmPrintPDF.aspx?SI=" + hdnSelectedItem.Value.ToString() + "&Location=" + "STATIC";

                Regex regex = new Regex(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$");
                Match match = regex.Match(hdnSelectedItem.Value+".pdf");

                if (!match.Success)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessageList('110806','" + hdnSelectedItem.Value + "'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    return;
                }
                   
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenPDFStatic('" + filesNotFound + "','" + hdnNotes.Value + "','"+DownloadDoc+"');", true);
            }
            if (hdnNotes.Value != string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "openProgress('" + hdnNotes.Value + "');", true);
            }
            if (hdnXmlPath.Value != string.Empty && hdnSelectedItem.Value == string.Empty && screen == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "OpenClinicalSmry();", true);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnPaymentCollection_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Payment", "OpenPaymentCollection();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //string QueryString = "EncounterID=" + hdnEncID.Value + "&humanID=" + hdnHumanID.Value + "&EncStatus=" + hdnEncStatus.Value + "&bShowPat=" + false + "&sScreenMode=" + "";
            //MessageWindow.NavigateUrl = "frmQuickPatientCreate.aspx?" + QueryString;

            //MessageWindow.Height = 930;
            //MessageWindow.VisibleOnPageLoad = true;
            //MessageWindow.Width = 1250;
            //MessageWindow.CenterIfModal = true;
            //MessageWindow.VisibleTitlebar = true;
            //MessageWindow.VisibleStatusbar = false;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            is_FormClosed = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Checkout", "CellSelectedforClose();", true);
        }

        protected void imgLibrary_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void cboRelationship_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            if (HiddenDLC.Value != "")
                DLC.txtDLC.Text = HiddenDLC.Value;
            txtDocumentsgivento.Text = "";
            if (cboRelationship.SelectedItem.Text.ToUpper() == "SELF" || cboRelationship.SelectedItem.Text == "")
            {
                txtDocumentsgivento.ReadOnly = true;
                txtDocumentsgivento.BackColor = Color.FromArgb(191, 219, 255);
                txtDocumentsgivento.BorderColor = Color.Black;
                if (cboRelationship.SelectedItem.Text.ToUpper() == "SELF")
                    txtDocumentsgivento.Text = sHuman;
            }
            else
            {
                txtDocumentsgivento.ReadOnly = false;
                txtDocumentsgivento.BackColor = Color.White;
                txtDocumentsgivento.HoveredStyle.BackColor = Color.White;
                txtDocumentsgivento.FocusedStyle.BackColor = Color.White;
                txtDocumentsgivento.CssClass = "Editabletxtbox";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {

        }

    }
}
