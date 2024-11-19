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
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace Acurus.Capella.UI
{
    public partial class frmFindAllAppointments : System.Web.UI.Page
    {
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
            btnCancelAppointment.Attributes.Add("onclick", "return OpenCancelAppt();");
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            if (!IsPostBack)
            {
                //ClientSession.FlushSession();
                this.Page.Title = "Find Appointments" + "-" + ClientSession.UserName;
                //chkShowOldAppointments.Checked = true;
                //btnFirst.Enabled = false;
                //btnNext.Enabled = false;
                //btnLast.Enabled = false;
                //btnPrevious.Enabled = false;
                if (Request["HumanID"] != null)
                {
                    hdnHumanID.Value = Request["HumanID"].ToString();
                }
                if (hdnHumanID.Value.Trim() == string.Empty)
                {
                    return;

                }
                if (Request["IsFindPatientRequired"] != null && Request["IsFindPatientRequired"].ToString() == "N")
                {
                    btnFindPatient.Visible = false;
                }
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
                if (fillneweditappt != null)
                {
                    txtPatientName.Value = fillneweditappt.Last_Name + "," + fillneweditappt.First_Name +
                       "  " + fillneweditappt.MI + "  " + fillneweditappt.Suffix;
                    txtPatientAccountNO.Value = fillneweditappt.Human_ID.ToString();
                    txtPatientDOB.Value = fillneweditappt.Birth_Date.ToString("dd-MMM-yyyy");
                    hdnHumanID.Value = fillneweditappt.Human_ID.ToString();
                }
                //FindResult(sender, e);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult", "FillResult();", true);

                pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
                Session["PageCount"] = pageCount;
                RefreshPageButtons();

                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);

                //if (grdAppointment.Items.Count > 0)
                //{
                //    if (grdAppointment.Items.Count == 1 && (grdAppointment.Items[0].Cells[5].Text == string.Empty || grdAppointment.Items[0].Cells[5].Text == "&nbsp;"))
                //    {
                //        btnCancelAppointment.Enabled = false;
                //        btnEditAppointment.Enabled = false;
                //    }
                //    if (grdAppointment.Items[0].Cells.Count > 7)
                //    {
                //        if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "MA_PROCESS", true) == 0)
                //        {
                //            btnEditAppointment.Enabled = false;
                //        }
                //        if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) != 0)
                //        {
                //            btnCancelAppointment.Enabled = false;
                //        }
                //        else if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) == 0)
                //        {
                //            btnCancelAppointment.Enabled = true;
                //            //btnEditAppointment.Enabled = false;
                //        }
                //    }
                //}

                //if (grdAppointment.MasterTableView.Items.Count > 0)
                //{
                //    hdnSelectedIndex.Value = "0";
                //}

                //btnCancelAppointment.Enabled = false;
                btnCancelAppointment.Disabled = true;
            }

        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object FindResult()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string extra_search = HttpContext.Current.Request.Params["extra_search"];
            var searchData = JsonConvert.DeserializeObject<Dictionary<string, string>>(extra_search);
            string sHumanID = searchData["sHumanID"];
            string chkShowOldAppointments = searchData["chkShowOldAppointments"];
            IList<FillWillingonCancel> objMyApptList = new List<FillWillingonCancel>();
            EncounterManager EncounterMngr = new EncounterManager();
            //int page = 0;
            //if (pageIndex == 0)
            //{
            //    page = 1;

            //}
            //else
            //{
            //    page = PageNumber;
            //}
            if (sHumanID != string.Empty && sHumanID != "undefined" && System.Text.RegularExpressions.Regex.IsMatch(sHumanID, "^[0-9]*$") == true)
            {
                if (chkShowOldAppointments == "true")
                {
                    objMyApptList = EncounterMngr.GetAppointmentsforPatientwithStatus(Convert.ToUInt64(sHumanID), true, 0, 0);
                }
                else
                {
                    objMyApptList = EncounterMngr.GetAppointmentsforPatientwithStatus(Convert.ToUInt64(sHumanID), false, 0, 0);
                }
                //if (objMyApptList.Count > 0)
                //    TotalCount = objMyApptList[0].Count;
                //hdnTotalCount.Value = TotalCount.ToString();
                //mpnAppointment.TotalNoofDBRecords = objMyApdptList.ApptCount;
                //mpnAppointment_Load(sender, e);

                //FillResult(objMyApptList);
            }
            FillWillingonCancel[] objMyApptListduplicate= null;
            objMyApptListduplicate = objMyApptList.ToArray();
            objMyApptListduplicate = objMyApptListduplicate.OrderByDescending(a => a.Appointment_Date_Time).ToArray();
            var result = new
                {
                    data = Compress(JsonConvert.SerializeObject(objMyApptListduplicate)),
                    facilityLibrary = ApplicationObject.facilityLibraryList

            };
                return result;
        }
        private static string Compress(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
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

            //if (iEndPageNo == 0)
            //{
            //    btnFirst.Enabled = false;
            //    btnPrevious.Enabled = false;
            //    btnNext.Enabled = false;
            //    btnLast.Enabled = false;
            //    return;
            //}
            //else
            //{

            //}

            //if (PageNumber == 1)
            //{
            //    btnFirst.Enabled = false;
            //    btnPrevious.Enabled = false;
            //}
            //else
            //{
            //    btnFirst.Enabled = true;
            //    btnPrevious.Enabled = true;
            //}
            //if (iEndPageNo >= TotalCount)
            //{
            //    iEndPageNo = TotalCount;

            //    if (iStartPageNo == 0 && iEndPageNo != 0)
            //    {
            //        iStartPageNo = 1;
            //    }


                //btnLast.Enabled = false;
                //btnNext.Enabled = false;
            //}
            //else
            //{
            //    btnLast.Enabled = true;
            //    btnNext.Enabled = true;
            //}
        }
        private void RefreshPageButtons()
        {

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
            //lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + hdnTotalCount.Value.ToString();

            //if (iEndPageNo == 0)
            //{
            //    btnFirst.Enabled = false;
            //    btnPrevious.Enabled = false;
            //    btnNext.Enabled = false;
            //    btnLast.Enabled = false;
            //    return;
            //}
            //else
            //{
            //    // lblShowing.Show();
            //}

            //if (PageNumber == 1)
            //{
            //    btnFirst.Enabled = false;
            //    btnPrevious.Enabled = false;
            //}
            //else
            //{
            //    btnFirst.Enabled = true;
            //    btnPrevious.Enabled = true;
            //}
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
                    //lblShowing.Text = "Showing " + iStartPageNo.ToString() + " - " + (iEndPageNo).ToString() + " of " + hdnTotalCount.Value;
                    //btnLast.Enabled = false;
                    //btnNext.Enabled = false;
                }
                //else
                //{
                //    btnLast.Enabled = true;
                //    btnNext.Enabled = true;
                //}
            }
        }
        public void FillResult(IList<FillWillingonCancel> MyApptList)
        {
            //FillAppointment MyApptList = appointmentproxy.GetAppointmentsforPatientwithStatus(ulMyHumanID);


            //grdAppointment.DataSource = null;
            //grdAppointment.DataBind();
            //// string sMyPhyName=string.Empty;

            //DataTable dt = new DataTable();
            //dt.Columns.Add("AppointmentDate", typeof(string));
            //dt.Columns.Add("AppointmentTime", typeof(string));
            //dt.Columns.Add("ProviderName", typeof(string));
            //dt.Columns.Add("FacilityName", typeof(string));
            //dt.Columns.Add("Appt_ID", typeof(string));
            //dt.Columns.Add("CurrentProcess", typeof(string));
            //dt.Columns.Add("Appt_Provider_Id", typeof(string));
            //dt.Columns.Add("Test_Ordered", typeof(string));
            //dt.Columns.Add("Rescheduled_Appointment_Date", typeof(string));
            //dt.Columns.Add("Reason_for_Cancelation", typeof(string));
            //dt.Columns.Add("Is_Archieve", typeof(string));

            //if (MyApptList.Count > 0)
            //{
            //    MyApptList = MyApptList.OrderByDescending(a => a.Appointment_Date_Time).ToList<FillWillingonCancel>();
            //    for (int i = 0; i < MyApptList.Count; i++)
            //    {
            //        DataRow dr = dt.NewRow();
            //        if (chkShowOldAppointments.Checked == false)
            //        {
            //            if (UtilityManager.ConvertToLocal(MyApptList[i].Appointment_Date_Time) < DateTime.Today)
            //            {
            //                continue;
            //            }
            //        }
            //        //srividhya
            //        //string[] sel = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), MyApptList.Appointment_Date[i]).ToString().Split(' ');
            //        string[] sel = UtilityManager.ConvertToLocal(MyApptList[i].Appointment_Date_Time).ToString().Split(' ');
            //        string[] rescheduledate = UtilityManager.ConvertToLocal(MyApptList[i].Rescheduled_Appointment_Date).ToString().Split(' ');
            //        string scheduledate = string.Empty;
            //        if (sel.Length > 0)
            //        {
            //            //dr["AppointmentDate"] = Convert.ToDateTime(sel[0]).ToString("dd-MMM-yyyy");
            //            //dr["AppointmentTime"] = sel[1].ToString()+" "+ sel[2].ToString();
            //            dr["AppointmentDate"] = Convert.ToDateTime(sel[0]).ToString("dd-MMM-yyyy") + " " + sel[1].ToString() + " " + sel[2].ToString();
            //            //dr["AppointmentTime"] = sel[1].ToString() + " " + sel[2].ToString();
            //        }
            //        dr["ProviderName"] = MyApptList[i].Physician_Name.ToString(); ;
            //        dr["FacilityName"] = MyApptList[i].Facility_Name.ToString();
            //        dr["Appt_ID"] = MyApptList[i].Encounter_ID.ToString();
            //        dr["CurrentProcess"] = MyApptList[i].Current_Process.ToString();
            //        // if (sAncillary.Trim() != MyApptList[i].Facility_Name.ToString().Trim())
            //        var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == MyApptList[i].Facility_Name.ToString() select f;
            //        IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            //        if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
            //        {
            //            dr["Appt_Provider_Id"] = MyApptList[i].Physician_ID.ToString();
            //        }
            //        else
            //            dr["Appt_Provider_Id"] = MyApptList[i].Machine_Technician_Library_ID.ToString();
            //        dr["Test_Ordered"] = MyApptList[i].Test_Ordered.ToString();
            //        if (rescheduledate.Length > 0)
            //        {
            //            scheduledate = Convert.ToDateTime(rescheduledate[0]).ToString("dd-MMM-yyyy") + " " + rescheduledate[1].ToString() + " " + rescheduledate[2].ToString();
            //            if (scheduledate == "01-Jan-0001 12:00:00 AM")
            //            {
            //                dr["Rescheduled_Appointment_Date"] = "";
            //            }
            //            else
            //            {
            //                dr["Rescheduled_Appointment_Date"] = scheduledate;
            //            }

            //        }
            //        dr["Reason_for_Cancelation"] = MyApptList[i].Reason_for_Cancelation;
            //        dr["Is_Archieve"] = MyApptList[i].Human_Type;
            //        dt.Rows.Add(dr);

            //    }

            //    grdAppointment.DataSource = dt;
            //    grdAppointment.DataBind();
            //}
            //else
            //{
            //    DataRow dr = dt.NewRow();
            //    dt.Rows.Add(dr);
            //    grdAppointment.DataSource = dt;
            //    grdAppointment.DataBind();
            //    grdAppointment.Items[0].Visible = false;
            //}

            ////if (ClientSession.FacilityName != System.Configuration.ConfigurationManager.AppSettings["CMGFacilityName"].ToString())
            ////{
            //var vfacAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            //IList<FacilityLibrary> lstFacAncillary = vfacAncillary.ToList<FacilityLibrary>();
            //if (lstFacAncillary.Count > 0 && lstFacAncillary[0].Is_Ancillary != "Y")
            //{
            //    grdAppointment.Columns[7].Visible = false;
            //}

            //lblSearchResult.Visible = true;
            //if (MyApptList.Count > 0)
            //    lblSearchResult.Text = MyApptList[0].Count + " " + "Results Found";
            //else
            //    lblSearchResult.Text = "0 Results Found";
            //pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            //Session["PageCount"] = pageCount;
            //RefreshPageButtons();

            //if (grdAppointment.MasterTableView.Items.Count > 0)
            //    grdAppointment.MasterTableView.Items[0].Selected = true;
            //else
            //{
            //    object o1 = new object();
            //    EventArgs e1 = new EventArgs();
            //    //grdFindAllAppointment_SelectionChanged(o1, e1);
            //}
            //if (grdAppointment.MasterTableView.Items.Count > 0 && grdAppointment.Items[0].Visible == true)
            //{
            //    grdAppointment.MasterTableView.Items[0].Selected = true;
            //    btnEditAppointment.Enabled = true;
            //    btnCancelAppointment.Enabled = true;
            //    hdnSelectedIndex.Value = "0";
            //}
            //else
            //{
            //    btnEditAppointment.Enabled = false;
            //    btnCancelAppointment.Enabled = false;
            //}
            //if (grdAppointment.Items.Count > 0 && grdAppointment.Items[0].Visible == true)
            //{
            //    if (grdAppointment.Items[0].Cells.Count > 7)
            //    {
            //        if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) != 0)
            //        {
            //            btnCancelAppointment.Enabled = false;
            //            btnEditAppointment.Enabled = true;
            //            //btnEditAppointment.Enabled = false;
            //        }
            //        else if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) == 0)
            //        {
            //            btnCancelAppointment.Enabled = true;
            //        }
            //    }
            //}
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
                    //if (btnFirst.Enabled == false && btnPrevious.Enabled == false)
                    //{
                    //    Session["PageNumber"] = 1;
                    //}
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

            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult", "FillResult();", true);

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
                txtPatientName.Value = humanrecord.First_Name + " " + humanrecord.Last_Name + " " + humanrecord.MI;
                txtPatientAccountNO.Value = humanrecord.Id.ToString();
                txtPatientDOB.Value = humanrecord.Birth_Date.ToString("dd-MMM-yyyy");
                hdnHumanID.Value = humanrecord.Id.ToString();
            }
            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult_FindPatient", "FillResult();", true);
            pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            Session["PageCount"] = pageCount;
            RefreshPageButtons();
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkShowOldAppointments_CheckedChanged(object sender, EventArgs e)
        {
            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResultchkshwall", "FillResult();", true);
            //if (grdAppointment.MasterTableView.Items.Count > 0)
            //{
            //    grdAppointment.MasterTableView.Items[0].Selected = true;
            //    btnEditAppointment.Enabled = true;
            //    btnCancelAppointment.Enabled = true;
            //    hdnSelectedIndex.Value = "0";
            //}
            //else
            //{
            //    btnEditAppointment.Enabled = false;
            //    btnCancelAppointment.Enabled = false;
            //}
            //if (grdAppointment.Items.Count == 1 && (grdAppointment.Items[0].Cells[5].Text == string.Empty || grdAppointment.Items[0].Cells[5].Text == "&nbsp;"))
            //{
            //    btnCancelAppointment.Enabled = false;
            //    btnEditAppointment.Enabled = false;
            //}
            //if (grdAppointment.Items.Count > 0)
            //{
            //    if (grdAppointment.Items[0].Cells.Count > 7)
            //    {
            //        if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "MA_PROCESS", true) == 0)
            //        {
            //            btnEditAppointment.Enabled = false;
            //        }
            //        if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) != 0)
            //        {
            //            btnCancelAppointment.Enabled = false;

            //        }
            //        else if (string.Compare(grdAppointment.Items[0].Cells[7].Text, "SCHEDULED", true) == 0)
            //        {
            //            btnCancelAppointment.Enabled = true;
            //        }
            //    }
            //}
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult", "FillResult();", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult", "FillResult();", true);
            //btnCancelAppointment.Enabled = false;
            btnCancelAppointment.Disabled = true;
        }

        protected void btnEditAppointment_Click(object sender, EventArgs e)
        {
            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult", "FillResult();", true);
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
                txtPatientName.Value = humanrecord.First_Name + " " + humanrecord.Last_Name + " " + humanrecord.MI;
                txtPatientAccountNO.Value = humanrecord.Id.ToString();
                txtPatientDOB.Value = humanrecord.Birth_Date.ToString("dd-MMM-yyyy");
                hdnHumanID.Value = humanrecord.Id.ToString();
            }
            //FindResult(sender, e);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PageLoad_FillResult_FindPatientRefersh", "FillResult();", true);
            pageCount = Convert.ToInt32(GetTotalNoofDBRecords());
            Session["PageCount"] = pageCount;
            RefreshPageButtons();
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void grdAppointment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridDataItem item = (GridDataItem)grdAppointment.MasterTableView.Items[grdAppointment.SelectedItems[0].ItemIndex];
            //hdnSelectedIndex.Value = item.ItemIndex.ToString();

            //btnCancelAppointment.Enabled = false;
            //if (grdAppointment.SelectedItems == null)
            //{
            //    btnEditAppointment.Enabled = false;
            //    return;
            //}
            //else
            //{
            //    btnEditAppointment.Enabled = true;
            //}

            //if (string.Compare(item["CurrentProcess"].Text, "SCHEDULED", true) == 0)
            //{
            //    btnCancelAppointment.Enabled = true;
            //}
            //else
            //{
            //    btnCancelAppointment.Enabled = false;
            //}
        }

    }
}
