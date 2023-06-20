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
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Telerik.Web.UI;
using log4net;

namespace Acurus.Capella.UI
{
    public partial class frmFindAllAppointments : System.Web.UI.Page
    {
        private static readonly ILog logFile = LogManager.GetLogger("Error");

        EncounterManager EncounterMngr = new EncounterManager();
        HumanManager humanMngr = new HumanManager();
        int TotalCount = 0;
        int PageIndex = 1;
        protected int pageIndex = 1;
        protected int pageCount = 0;
        protected int rowCount = 0;

        int PageNumber = 1;
        int MaxResultPerPage = 25;
        int TotalNoofDBRecords;
        Double myPageNumber;
        int iMyLastPageNo;
        //string sAncillary = string.Empty;
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    foreach (GridViewRow r in grdAppointment.Rows)
        //    {
        //        if (r.RowType == DataControlRowType.DataRow)
        //        {
        //            Page.ClientScript.RegisterForEventValidation
        //                    (r.UniqueID + "$ctl00");
        //            Page.ClientScript.RegisterForEventValidation
        //                    (r.UniqueID + "$ctl01");
        //            //Page.ClientScript.RegisterForEventValidation
        //            //       (r.UniqueID + "$ctl02");
        //        }
        //    }

        //    base.Render(writer);
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                logFile.Info(DateTime.UtcNow.ToString() + "- Page Load - Start", null);
                btnCancelAppointment.Attributes.Add("onclick", "return OpenCancelAppt();");
                //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                //{
                //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
                //}
                logFile.Info(DateTime.UtcNow.ToString() + "- PostBack - Start", null);
                if (!IsPostBack)
                {
                    logFile.Info(DateTime.UtcNow.ToString() + "- Button Enable - Start", null);
                    //ClientSession.FlushSession();
                    this.Page.Title = "Find Appointments" + "-" + ClientSession.UserName;
                    //chkShowOldAppointments.Checked = true;
                    btnFirst.Enabled = false;
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                    btnPrevious.Enabled = false;
                    logFile.Info(DateTime.UtcNow.ToString() + "- Button Enable - End", null);
                    logFile.Info(DateTime.UtcNow.ToString() + "- Hidden Human Value Set - Start", null);
                    if (Request["HumanID"] != null)
                    {
                        hdnHumanID.Value = Request["HumanID"].ToString();
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Hidden Human Value Set - End", null);
                    logFile.Info(DateTime.UtcNow.ToString() + "- Hidden Human Value Set Empty - Start", null);
                    if (hdnHumanID.Value.Trim() == string.Empty)
                    {
                        return;
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Hidden Human Value Set Empty - End", null);
                    logFile.Info(DateTime.UtcNow.ToString() + "- Find Patient Button Visible - Start", null);
                    if (Request["IsFindPatientRequired"] != null && Request["IsFindPatientRequired"].ToString() == "N")
                    {
                        btnFindPatient.Visible = false;
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Find Patient Button Visible - End", null);
                    logFile.Info(DateTime.UtcNow.ToString() + "- Fill New Edit Appointment - Start", null);
                    FillNewEditAppointment fillneweditappt = null;
                    if (hdnHumanID.Value != null && hdnHumanID.Value != "undefined" && hdnHumanID.Value.Trim() != "")
                    {
                        try
                        {
                            if (IsDigitsOnly(hdnHumanID.Value) && System.Text.RegularExpressions.Regex.IsMatch(hdnHumanID.Value, "^[0-9]*$") == true)
                            {
                                fillneweditappt = EncounterMngr.GetEncounterAndHumanRecord(0, Convert.ToUInt64(hdnHumanID.Value));
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Fill New Edit Appointment - End", null);
                    logFile.Info(DateTime.UtcNow.ToString() + "- Fill Patient Name - Start", null);
                    if (fillneweditappt != null)
                    {
                        txtPatientName.Text = fillneweditappt.Last_Name + "," + fillneweditappt.First_Name +
                           "  " + fillneweditappt.MI + "  " + fillneweditappt.Suffix;
                        txtPatientAccountNO.Text = fillneweditappt.Human_ID.ToString();
                        txtPatientDOB.Text = fillneweditappt.Birth_Date.ToString("dd-MMM-yyyy");
                        hdnHumanID.Value = fillneweditappt.Human_ID.ToString();
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Fill Patient Name - End", null);
                   
                        FindResult(sender, e);
                    
                    logFile.Info(DateTime.UtcNow.ToString() + "- Get Total No Of Record - Start", null);
                    pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
                    Session["PageCount"] = pageCount;
                    logFile.Info(DateTime.UtcNow.ToString() + "- Get Total No Of Record - End", null);
                    
                        RefreshPageButtons();

                    logFile.Info(DateTime.UtcNow.ToString() + "- Apply User Permissions - Start", null);
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                        objSecurity.ApplyUserPermissions(this.Page);
                    logFile.Info(DateTime.UtcNow.ToString() + "- Apply User Permissions - End", null);

                    logFile.Info(DateTime.UtcNow.ToString() + "- Grid Load Appointment - Start", null);
                    if (grdAppointment.Items.Count > 0)
                    {
                        logFile.Info(DateTime.UtcNow.ToString() + "- Button Diable - Start", null);
                        if (grdAppointment.Items.Count == 1 && (grdAppointment.Items[0].Cells[5].Text == string.Empty || grdAppointment.Items[0].Cells[5].Text == "&nbsp;"))
                        {
                            btnCancelAppointment.Enabled = false;
                            btnEditAppointment.Enabled = false;
                        }
                        logFile.Info(DateTime.UtcNow.ToString() + "- Button Diable - End", null);
                        if (grdAppointment.Items[0].Cells.Count > 7)
                        {
                            if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "MA_PROCESS", true) == 0)
                            {
                                btnEditAppointment.Enabled = false;
                            }
                            if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) != 0)
                            {
                                btnCancelAppointment.Enabled = false;
                            }
                            else if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) == 0)
                            {
                                btnCancelAppointment.Enabled = true;
                                //btnEditAppointment.Enabled = false;
                            }
                        }
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Grid Load Appointment - End", null);

                    logFile.Info(DateTime.UtcNow.ToString() + "- Grid Load Appointment MasterTable- Start", null);
                    if (grdAppointment.MasterTableView.Items.Count > 0)
                    {
                        hdnSelectedIndex.Value = "0";
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- Grid Load Appointment MasterTable- End", null);
                }
                logFile.Info(DateTime.UtcNow.ToString() + "- PostBack - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "- Page Load - End", null);

            }
            catch(Exception ex)
            {
                logFile.Info(DateTime.UtcNow.ToString() + ex, null);
            }
}
        bool IsDigitsOnly(string str)
        {
            try
            {
                logFile.Info(DateTime.UtcNow.ToString() + "- IsDigitsOnly - Start", null);
                foreach (char c in str)
                {
                    if (c < '0' || c > '9')
                        return false;
                }
                logFile.Info(DateTime.UtcNow.ToString() + "- IsDigitsOnly - End", null);
            }
            catch (Exception ex)
            {
                logFile.Info(DateTime.UtcNow.ToString() + ex, null);
            }

            return true;
        }
        void FindResult(object sender, EventArgs e)
        {
            try
            {
                logFile.Info(DateTime.UtcNow.ToString() + "-FindResult - Page No - Start", null);
                IList<FillWillingonCancel> objMyApptList = new List<FillWillingonCancel>();
                int page = 0;
                if (pageIndex == 0)
                {
                    page = 1;

                }
                else
                {
                    page = PageNumber;
                }
                logFile.Info(DateTime.UtcNow.ToString() + "-FindResult - Page No - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "-FindResult - Get Appointments For Patient with Status - Start", null);
                if (hdnHumanID.Value != string.Empty && hdnHumanID.Value != "undefined" && System.Text.RegularExpressions.Regex.IsMatch(hdnHumanID.Value, "^[0-9]*$") == true)
                {
                    if (chkShowOldAppointments.Checked == true)
                    {
                        objMyApptList = EncounterMngr.GetAppointmentsforPatientwithStatus(Convert.ToUInt64(hdnHumanID.Value), true, PageNumber, 25);
                    }
                    else
                    {
                        objMyApptList = EncounterMngr.GetAppointmentsforPatientwithStatus(Convert.ToUInt64(hdnHumanID.Value), false, PageNumber, 25);
                    }
                    if (objMyApptList.Count > 0)
                        TotalCount = objMyApptList[0].Count;
                    hdnTotalCount.Value = TotalCount.ToString();
                    //mpnAppointment.TotalNoofDBRecords = objMyApdptList.ApptCount;
                    //mpnAppointment_Load(sender, e);

                    FillResult(objMyApptList);
                }
                logFile.Info(DateTime.UtcNow.ToString() + "-FindResult - Get Appointments For Patient with Status - End", null);
            }
            catch(Exception ex)
            {
                logFile.Info(DateTime.UtcNow.ToString() + ex, null);
            }
        }

        public double GetTotalNoofDBRecords()
        {


            if (TotalCount != 0)
            {
                myPageNumber = (double)(TotalCount) / (double)(MaxResultPerPage);
                iMyLastPageNo = Convert.ToInt32(Math.Ceiling(myPageNumber));
                //Session["iMyLastPageNo"] = iMyLastPageNo;
                hdnLastPageNo.Value = iMyLastPageNo.ToString();
            }
            LinkButtonLoad();
            return myPageNumber;
        }
        private void LinkButtonLoad()
        {
            int iStartPageNo = 0;
            int iEndPageNo = 0;

            if (TotalCount == 0)
            {
                iStartPageNo = 0;
            }
            else
            {
                if (hdnLastPageNo.Value != string.Empty)
                {
                    iStartPageNo = ((Convert.ToInt32(hdnLastPageNo.Value) - 1) * MaxResultPerPage) + 1;
                }
            }
            if (hdnLastPageNo.Value != string.Empty)
            {
                iEndPageNo = ((Convert.ToInt32(hdnLastPageNo.Value)) * MaxResultPerPage);
            }

            if (iEndPageNo == 0)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                return;
            }
            else
            {

            }

            if (PageNumber == 1)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
            }
            if (iEndPageNo >= TotalCount)
            {
                iEndPageNo = TotalCount;

                if (iStartPageNo == 0 && iEndPageNo != 0)
                {
                    iStartPageNo = 1;
                }


                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }
        private void RefreshPageButtons()
        {
            try
            {
                logFile.Info(DateTime.UtcNow.ToString() + "- RefreshPageButtons - Start Page No - Start", null);
                int iStartPageNo = 0;
                int iEndPageNo = 0;
                if (hdnLastPageNo.Value != string.Empty)
                {

                    if (Convert.ToInt32(hdnLastPageNo.Value) == 0)
                    {
                        iStartPageNo = 0;
                    }
                    else
                    {
                        iStartPageNo = ((PageNumber - 1) * MaxResultPerPage) + 1;
                    }
                }
                iEndPageNo = (PageNumber * MaxResultPerPage);
                lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + hdnTotalCount.Value.ToString();
                logFile.Info(DateTime.UtcNow.ToString() + "-  RefreshPageButtons - Start Page No - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "-  RefreshPageButtons - Enable button - Start", null);
                if (iEndPageNo == 0)
                {
                    btnFirst.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                    return;
                }
                else
                {
                    // lblShowing.Show();
                }

                if (PageNumber == 1)
                {
                    btnFirst.Enabled = false;
                    btnPrevious.Enabled = false;
                }
                else
                {
                    btnFirst.Enabled = true;
                    btnPrevious.Enabled = true;
                }
                logFile.Info(DateTime.UtcNow.ToString() + "-  RefreshPageButtons - Enable button - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "-  RefreshPageButtons - Hidden Page No - Start", null);
                if (hdnTotalCount.Value != string.Empty)
                {
                    if (iEndPageNo >= Convert.ToInt32(hdnTotalCount.Value))
                    {
                        iEndPageNo = Convert.ToInt32(hdnTotalCount.Value);

                        if (iStartPageNo == 0 && iEndPageNo != 0)
                        {
                            iStartPageNo = 1;
                        }
                        if (hdnTotalCount.Value == "0")
                        {
                            iStartPageNo = 0;
                        }
                        lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + hdnTotalCount.Value;
                        btnLast.Enabled = false;
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        btnLast.Enabled = true;
                        btnNext.Enabled = true;
                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "-  RefreshPageButtons - Hidden Page No - End", null);
                }
            }
            catch(Exception ex)
            {
                logFile.Info(DateTime.UtcNow.ToString() + "-  RefreshPageButtons - "+ex, null);
            }
        }
        public void FillResult(IList<FillWillingonCancel> MyApptList)
        {
            //FillAppointment MyApptList = appointmentproxy.GetAppointmentsforPatientwithStatus(ulMyHumanID);
            try
            {
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Add Columns - Start", null);
                grdAppointment.DataSource = null;
                grdAppointment.DataBind();
                // string sMyPhyName=string.Empty;

                DataTable dt = new DataTable();
                dt.Columns.Add("AppointmentDate", typeof(string));
                dt.Columns.Add("AppointmentTime", typeof(string));
                dt.Columns.Add("ProviderName", typeof(string));
                dt.Columns.Add("FacilityName", typeof(string));
                dt.Columns.Add("Appt_ID", typeof(string));
                dt.Columns.Add("CurrentProcess", typeof(string));
                dt.Columns.Add("Appt_Provider_Id", typeof(string));
                dt.Columns.Add("Test_Ordered", typeof(string));
                dt.Columns.Add("Rescheduled_Appointment_Date", typeof(string));
                dt.Columns.Add("Reason_for_Cancelation", typeof(string));
                dt.Columns.Add("Is_Archieve", typeof(string));
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Add Columns - End", null);

                if (MyApptList.Count > 0)
                {
                    logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Load Values - Start", null);
                    MyApptList = MyApptList.OrderByDescending(a => a.Appointment_Date_Time).ToList<FillWillingonCancel>();
                    for (int i = 0; i < MyApptList.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        if (chkShowOldAppointments.Checked == false)
                        {
                            if (UtilityManager.ConvertToLocal(MyApptList[i].Appointment_Date_Time) < DateTime.Today)
                            {
                                continue;
                            }
                        }
                        //srividhya
                        //string[] sel = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), MyApptList.Appointment_Date[i]).ToString().Split(' ');
                        string[] sel = UtilityManager.ConvertToLocal(MyApptList[i].Appointment_Date_Time).ToString().Split(' ');
                        string[] rescheduledate = UtilityManager.ConvertToLocal(MyApptList[i].Rescheduled_Appointment_Date).ToString().Split(' ');
                        string scheduledate = string.Empty;
                        if (sel.Length > 0)
                        {
                            //dr["AppointmentDate"] = Convert.ToDateTime(sel[0]).ToString("dd-MMM-yyyy");
                            //dr["AppointmentTime"] = sel[1].ToString()+" "+ sel[2].ToString();
                            dr["AppointmentDate"] = Convert.ToDateTime(sel[0]).ToString("dd-MMM-yyyy") + " " + sel[1].ToString() + " " + sel[2].ToString();
                            //dr["AppointmentTime"] = sel[1].ToString() + " " + sel[2].ToString();
                        }
                        dr["ProviderName"] = MyApptList[i].Physician_Name.ToString(); ;
                        dr["FacilityName"] = MyApptList[i].Facility_Name.ToString();
                        dr["Appt_ID"] = MyApptList[i].Encounter_ID.ToString();
                        dr["CurrentProcess"] = MyApptList[i].Current_Process.ToString();
                        // if (sAncillary.Trim() != MyApptList[i].Facility_Name.ToString().Trim())
                        var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == MyApptList[i].Facility_Name.ToString() select f;
                        IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
                        {
                            dr["Appt_Provider_Id"] = MyApptList[i].Physician_ID.ToString();
                        }
                        else
                            dr["Appt_Provider_Id"] = MyApptList[i].Machine_Technician_Library_ID.ToString();
                        dr["Test_Ordered"] = MyApptList[i].Test_Ordered.ToString();
                        if (rescheduledate.Length > 0)
                        {
                            scheduledate = Convert.ToDateTime(rescheduledate[0]).ToString("dd-MMM-yyyy") + " " + rescheduledate[1].ToString() + " " + rescheduledate[2].ToString();
                            if (scheduledate == "01-Jan-0001 12:00:00 AM")
                            {
                                dr["Rescheduled_Appointment_Date"] = "";
                            }
                            else
                            {
                                dr["Rescheduled_Appointment_Date"] = scheduledate;
                            }

                        }
                        dr["Reason_for_Cancelation"] = MyApptList[i].Reason_for_Cancelation;
                        dr["Is_Archieve"] = MyApptList[i].Human_Type;
                        dt.Rows.Add(dr);

                    }
                    logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Load Values - End", null);
                    logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Data Bind - Start", null);
                    grdAppointment.DataSource = dt;
                    grdAppointment.DataBind();
                    logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Data Bind - End", null);
                }
                else
                {
                    logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Load Empty Values - Start", null);
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    grdAppointment.DataSource = dt;
                    grdAppointment.DataBind();
                    grdAppointment.Items[0].Visible = false;
                    logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Load Empty Values - End", null);
                }

                //if (ClientSession.FacilityName != System.Configuration.ConfigurationManager.AppSettings["CMGFacilityName"].ToString())
                //{
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Load Ancillary Value - Start", null);
                var vfacAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> lstFacAncillary = vfacAncillary.ToList<FacilityLibrary>();
                if (lstFacAncillary.Count > 0 && lstFacAncillary[0].Is_Ancillary != "Y")
                {
                    grdAppointment.Columns[7].Visible = false;
                }
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Load Ancillary Value - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Result count - Start", null);
                lblSearchResult.Visible = true;
                if (MyApptList.Count > 0)
                    lblSearchResult.Text = MyApptList[0].Count + " " + "Results Found";
                else
                    lblSearchResult.Text = "0 Results Found";
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Result count - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Page count - Start", null);
                pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
                Session["PageCount"] = pageCount;
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Result count - End", null);
                RefreshPageButtons();
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Grid Select Item - Start", null);
                if (grdAppointment.MasterTableView.Items.Count > 0)
                    grdAppointment.MasterTableView.Items[0].Selected = true;
                else
                {
                    object o1 = new object();
                    EventArgs e1 = new EventArgs();
                    //grdFindAllAppointment_SelectionChanged(o1, e1);
                }
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Grid Select Item - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Grid Master Table Selected - Start", null);
                if (grdAppointment.MasterTableView.Items.Count > 0 && grdAppointment.Items[0].Visible == true)
                {
                    grdAppointment.MasterTableView.Items[0].Selected = true;
                    btnEditAppointment.Enabled = true;
                    btnCancelAppointment.Enabled = true;
                    hdnSelectedIndex.Value = "0";
                }
                else
                {
                    btnEditAppointment.Enabled = false;
                    btnCancelAppointment.Enabled = false;
                }
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Grid Master Table Selected - End", null);
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Button Enable Disable - Start", null);
                if (grdAppointment.Items.Count > 0 && grdAppointment.Items[0].Visible == true)
                {
                    if (grdAppointment.Items[0].Cells.Count > 7)
                    {
                        if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) != 0)
                        {
                            btnCancelAppointment.Enabled = false;
                            btnEditAppointment.Enabled = true;
                            //btnEditAppointment.Enabled = false;
                        }
                        else if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) == 0)
                        {
                            btnCancelAppointment.Enabled = true;
                        }
                    }
                }
                logFile.Info(DateTime.UtcNow.ToString() + "- FillResult - Button Enable Disable - End", null);
            }
            catch(Exception ex)
            {
                logFile.Info(DateTime.UtcNow.ToString() + ex, null);
            }
        }
        protected void PageChangeEventHandler(object sender, CommandEventArgs e)
        {


            switch (e.CommandArgument.ToString())
            {

                case "First":

                    Session["PageNumber"] = 1;

                    break;


                case "Previous":

                    if (Convert.ToInt32(Session["PageNumber"]) > 1)
                    {
                        PageNumber = Convert.ToInt32(Session["PageNumber"]) - 1;
                        Session["PageNumber"] = PageNumber;
                    }

                    break;
                case "Next":
                    if (btnFirst.Enabled == false && btnPrevious.Enabled == false)
                    {
                        Session["PageNumber"] = 1;
                    }
                    if (Convert.ToInt32(Session["PageNumber"]) < Convert.ToInt32(hdnLastPageNo.Value))
                    {
                        PageNumber = Convert.ToInt32(Session["PageNumber"]) + 1;
                        Session["PageNumber"] = PageNumber;
                    }

                    break;

                case "Last":

                    PageNumber = Convert.ToInt32(hdnLastPageNo.Value);
                    break;

            }
            Session["PageNumber"] = PageNumber;

            FindResult(sender, e);

            RefreshPageButtons();
        }

        protected void btnFindPatient_Click(object sender, EventArgs e)
        {
            Human humanrecord = new Human();

            if (hdnHumanID.Value != string.Empty)
            {

            }
            else
            {
                return;
            }

            IList<Human> humanlist = humanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(hdnHumanID.Value));
            if (humanlist.Count > 0 && humanlist != null)
            {
                humanrecord = humanlist[0];
            }
            if (humanrecord != null)
            {
                txtPatientName.Text = humanrecord.First_Name + " " + humanrecord.Last_Name + " " + humanrecord.MI;
                txtPatientAccountNO.Text = humanrecord.Id.ToString();
                txtPatientDOB.Text = humanrecord.Birth_Date.ToString("dd-MMM-yyyy");
                hdnHumanID.Value = humanrecord.Id.ToString();
            }
            FindResult(sender, e);
            pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            Session["PageCount"] = pageCount;
            RefreshPageButtons();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkShowOldAppointments_CheckedChanged(object sender, EventArgs e)
        {
            FindResult(sender, e);
            if (grdAppointment.MasterTableView.Items.Count > 0)
            {
                grdAppointment.MasterTableView.Items[0].Selected = true;
                btnEditAppointment.Enabled = true;
                btnCancelAppointment.Enabled = true;
                hdnSelectedIndex.Value = "0";
            }
            else
            {
                btnEditAppointment.Enabled = false;
                btnCancelAppointment.Enabled = false;
            }
            if (grdAppointment.Items.Count == 1 && (grdAppointment.Items[0].Cells[5].Text == string.Empty || grdAppointment.Items[0].Cells[5].Text == "&nbsp;"))
            {
                btnCancelAppointment.Enabled = false;
                btnEditAppointment.Enabled = false;
            }
            if (grdAppointment.Items.Count > 0)
            {
                if (grdAppointment.Items[0].Cells.Count > 7)
                {
                    if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "MA_PROCESS", true) == 0)
                    {
                        btnEditAppointment.Enabled = false;
                    }
                    if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) != 0)
                    {
                        btnCancelAppointment.Enabled = false;
                       
                    }
                    else if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) == 0)
                    {
                        btnCancelAppointment.Enabled = true;
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            FindResult(sender, e);
            btnCancelAppointment.Enabled = false;
        }

        protected void btnEditAppointment_Click(object sender, EventArgs e)
        {
            FindResult(sender, e);
        }

        protected void btnFindPatientRefresh_Click(object sender, EventArgs e)
        {
            Human humanrecord = new Human();

            if (hdnHumanID.Value != string.Empty)
            {

            }
            else
            {
                return;
            }

            IList<Human> humanlist = humanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(hdnHumanID.Value));
            if (humanlist.Count > 0 && humanlist != null)
            {
                humanrecord = humanlist[0];
            }
            if (humanrecord != null)
            {
                txtPatientName.Text = humanrecord.First_Name + " " + humanrecord.Last_Name + " " + humanrecord.MI;
                txtPatientAccountNO.Text = humanrecord.Id.ToString();
                txtPatientDOB.Text = humanrecord.Birth_Date.ToString("dd-MMM-yyyy");
                hdnHumanID.Value = humanrecord.Id.ToString();
            }
            FindResult(sender, e);
            pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            Session["PageCount"] = pageCount;
            RefreshPageButtons();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void grdAppointment_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridDataItem item = (GridDataItem)grdAppointment.MasterTableView.Items[grdAppointment.SelectedItems[0].ItemIndex];
            hdnSelectedIndex.Value = item.ItemIndex.ToString();

            btnCancelAppointment.Enabled = false;
            if (grdAppointment.SelectedItems == null)
            {
                btnEditAppointment.Enabled = false;
                return;
            }
            else
            {
                btnEditAppointment.Enabled = true;
            }

            if (string.Compare(item["CurrentProcess"].Text, "SCHEDULED", true) == 0)
            {
                btnCancelAppointment.Enabled = true;
            }
            else
            {
                btnCancelAppointment.Enabled = false;
            }
        }

    }
}
