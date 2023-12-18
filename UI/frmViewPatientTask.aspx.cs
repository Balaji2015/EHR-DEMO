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
using Acurus.Capella.UI;

namespace Acurus.Capella.UI
{
    public partial class ViewPatientTask : System.Web.UI.Page
    {
        PatientNotesManager patientnotesmngr = new PatientNotesManager();
        DataTable dttable = new DataTable();
        HumanManager objhumanmngr = new HumanManager();

        DataRow drow;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "ViewPatientTask" + "-" + ClientSession.UserName;
           if (!IsPostBack)
            {
              //  ClientSession.FlushSession();
                if (Request["AccountNum"] != null)
                {
                    hdnParentscreen.Value = Convert.ToString(Request["ParentScreen"]);
                    txtAccount.Text = Convert.ToString(Request["AccountNum"]);
                    PatientDetails();
                }
                else if (ClientSession.HumanId > 0)
                {
                    //hdnParentscreen.Value = Convert.ToString(Request["ParentScreen"]);
                    txtAccount.Text = ClientSession.HumanId.ToString();
                    PatientDetails();
                }
                 
                btnViewMessages.Visible = false;
            }
           bool result = Convert.ToBoolean(HttpContext.Current.Session["IsPatientCommunicated"]);

           if (result)
           {
               btnViewMessages.Visible = true;
               HttpContext.Current.Session["IsPatientCommunicated"] = false;
           }
           //CAP-1442
           //else
           //    btnViewMessages.Visible = false;
        }

       
       
        void fillgrdPatientDetails()
        {
            dttable.Columns.Add("Created Date And Time", typeof(string));
            dttable.Columns.Add("Source", typeof(string));
            dttable.Columns.Add("Source ID", typeof(string));
            dttable.Columns.Add("Acc. #", typeof(string));
            dttable.Columns.Add("Patient Name", typeof(string));
            dttable.Columns.Add("DOB", typeof(string));
            dttable.Columns.Add("Description", typeof(string));
            dttable.Columns.Add("Notes", typeof(string));
            dttable.Columns.Add("Priority", typeof(string));
            dttable.Columns.Add("Facility", typeof(string));
            dttable.Columns.Add("Assigned To", typeof(string));
            dttable.Columns.Add("Created By", typeof(string));
         
            dttable.Columns.Add("Modified By", typeof(string));
            dttable.Columns.Add("Modified Date and Time", typeof(string));
          
        }
      

        void fillGrid()
        {
            fillgrdPatientDetails();
            IList<PatientNotes> patientdetails = new List<PatientNotes>();

            if (chkShowTask.Checked == false)
            {
                patientdetails = patientnotesmngr.Getpatientdetails(txtAccount.Text);
            }
            else
            {
                patientdetails = patientnotesmngr.Getpatienttask(ClientSession.UserName, txtAccount.Text);
            }

            if (patientdetails.Count != 0)
            {
                for (int i = 0; i < patientdetails.Count; i++)
                {
                    drow = dttable.NewRow();

                    //srividhya
                    //DateTime dtCreatedDate = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), patientdetails[i].Created_Date_And_Time);
                    DateTime dtCreatedDate = UtilityManager.ConvertToLocal(patientdetails[i].Created_Date_And_Time);
                    if (dtCreatedDate.ToString("yyyy-MM-dd") == "0001-01-01")
                    {
                        drow["Created Date And Time"] = "";
                    }
                    else
                    {
                        drow["Created Date And Time"] = dtCreatedDate.ToString("dd-MMM-yyyy hh:mm tt");// patientdetails[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                    }

                    //if (patientdetails[i].Created_Date_And_Time.ToString("yyyy-MM-dd") == "0001-01-01")
                    //{
                    //    drow["Created Date And Time"] = "";
                    //}
                    //else
                    //{
                    //    drow["Created Date And Time"] = patientdetails[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                    //}
                    drow["Source"] = patientdetails[i].Source;
                    //if (Request["EncounterID"] != null)
                    //{
                    drow["Source ID"] = patientdetails[i].SourceID;
                    //}
                    //else
                    //{
                    //    drow["Source ID"] = 0;
                    //}
                    drow["Acc. #"] = patientdetails[i].Human_ID;
                    IList<Human> patientdetail = objhumanmngr.patientdetails(patientdetails[i].Human_ID.ToString());
                    if (patientdetail.Count != 0)
                    {
                        drow["Patient Name"] = patientdetail[0].First_Name;
                        drow["DOB"] = patientdetail[0].Birth_Date.ToString("dd-MMM-yyyy");
                    }
                    drow["Description"] = patientdetails[i].Message_Description;
                    drow["Notes"] = patientdetails[i].Notes.Replace("<br />", "\r\n");
                    drow["Priority"] = patientdetails[i].Priority;
                    drow["Facility"] = patientdetails[i].Facility_Name;
                    drow["Assigned To"] = patientdetails[i].Assigned_To;
                    drow["Created By"] = patientdetails[i].Created_By;
                    drow["Modified By"] = patientdetails[i].Modified_By;
                    
                    //srividhya
                    //DateTime dtModifiedDate = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), patientdetails[i].Modified_Date_And_Time);
                    DateTime dtModifiedDate = UtilityManager.ConvertToLocal(patientdetails[i].Modified_Date_And_Time);
                    if (dtModifiedDate.ToString("yyyy-MM-dd") == "0001-01-01")
                    {
                        drow["Modified Date And Time"] = "";
                    }
                    else
                    {
                        drow["Modified Date And Time"] = dtModifiedDate.ToString("dd-MMM-yyyy hh:mm tt");// patientdetails[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                    }

                    //if (patientdetails[i].Modified_Date_And_Time.ToString() !="1/1/0001 12:00:00 AM")
                    //{
                    //    drow["Modified Date and Time"] = patientdetails[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                    //}
                    //else
                    //{
                    //    drow["Modified Date and Time"] = "";
                    //}
                    dttable.Rows.Add(drow);

                }
                grdPatientDetails.DataSource = dttable;
                grdPatientDetails.DataBind();
                grdPatientDetails.Columns[2].Visible = false;
            }
            else
            {
                grdPatientDetails.DataSource = null;
                grdPatientDetails.DataBind();

            }
            //else
            //{
            //    drow = dttable.NewRow();
            //    dttable.Rows.Add(drow);
            //    grdPatientDetails.DataSource = dttable;
            //    grdPatientDetails.DataBind();
            //}
           

            
        }

     protected void chkShowTask_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowTask.Checked == true)
            {
                fillgrdPatientDetails();
                IList<PatientNotes> patientdetails = patientnotesmngr.Getpatienttask(ClientSession.UserName, txtAccount.Text);

                if (patientdetails.Count != 0)
                {
                    for (int i = 0; i < patientdetails.Count; i++)
                    {
                        drow = dttable.NewRow();

                        //srividhya
                        //DateTime dtCreatedDate = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), patientdetails[i].Created_Date_And_Time);
                        DateTime dtCreatedDate = UtilityManager.ConvertToLocal(patientdetails[i].Created_Date_And_Time);
                        if (dtCreatedDate.ToString("yyyy-MM-dd") == "0001-01-01")
                        {
                            drow["Created Date And Time"] = "";
                        }
                        else
                        {
                            drow["Created Date And Time"] = dtCreatedDate.ToString("dd-MMM-yyyy hh:mm tt");// patientdetails[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                        }
                        //if (patientdetails[i].Created_Date_And_Time.ToString("yyyy-MM-dd") == "0001-01-01")
                        //{
                        //    drow["Created Date And Time"] = "";
                        //}
                        //else
                        //{
                        //    drow["Created Date And Time"] = patientdetails[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                        //}
                        drow["Source"] = patientdetails[i].Source;
                        drow["Source ID"] = Convert.ToInt32(Request["EncounterID"]);
                        drow["Acc. #"] = patientdetails[i].Human_ID;
                        IList<Human> patientdetail = objhumanmngr.patientdetails(patientdetails[i].Human_ID.ToString());
                        if (patientdetail.Count != 0)
                        {
                            drow["Patient Name"] = patientdetail[0].First_Name;
                            drow["DOB"] = patientdetail[0].Birth_Date.ToString("dd-MMM-yyyy");
                        }
                        drow["Description"] = patientdetails[i].Message_Description;
                        drow["Notes"] = patientdetails[i].Notes.Replace("<br />", "\r\n");
                        drow["Priority"] = patientdetails[i].Priority;
                        drow["Facility"] = patientdetails[i].Facility_Name;
                        drow["Assigned To"] = patientdetails[i].Assigned_To;
                        drow["Created By"] = ClientSession.UserName;
                        drow["Modified By"] = patientdetails[i].Modified_By;

                        //srividhya
                        //DateTime dtModifiedDate = ToLoalTime.LocalTimeFromTimeOffset(Session["LocalTime"].ToString(), patientdetails[i].Modified_Date_And_Time);
                        DateTime dtModifiedDate = UtilityManager.ConvertToLocal(patientdetails[i].Modified_Date_And_Time);
                        if (dtModifiedDate.ToString("yyyy-MM-dd") == "0001-01-01")
                        {
                            drow["Modified Date And Time"] = "";
                        }
                        else
                        {
                            drow["Modified Date And Time"] = dtModifiedDate.ToString("dd-MMM-yyyy hh:mm tt");// patientdetails[i].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                        }

                        //if (patientdetails[i].Modified_Date_And_Time.ToString() != "1/1/0001 12:00:00 AM")
                        //{
                        //    drow["Modified Date and Time"] = patientdetails[i].Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                        //}
                        //else
                        //{
                        //    drow["Modified Date and Time"] = "";
                        //}

                        //drow["Message Date and Time"] = patientdetaregisils[i].Message_Date_And_Time;
                        dttable.Rows.Add(drow);

                    }
                    grdPatientDetails.DataSource = dttable;
                    grdPatientDetails.DataBind();
                    grdPatientDetails.Columns[2].Visible = false;
                }
            }
           if(chkShowTask.Checked==false)
            {
                fillGrid();
            }
            //else
            //{
            //    drow = dttable.NewRow();
            //    dttable.Rows.Add(drow);
            //    grdPatientDetails.DataSource = dttable;
            //    grdPatientDetails.DataBind();
            //}
           ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "patienttaskload();", true);

        }

        protected void btnaddMessages_Click(object sender, EventArgs e)
        {
            fillGrid();
        }
        void PatientDetails()
        {
            IList<Human> patientdetail = objhumanmngr.patientdetails(txtAccount.Text);
            if (patientdetail.Count != 0)
            {
                txtPatientName.Text = patientdetail[0].First_Name.TrimEnd(',');
                txtDOB.Text = patientdetail[0].Birth_Date.ToString("dd-MMM-yyyy");
                txtPatientStatus.Text = patientdetail[0].Patient_Status;
                txtPatientType.Text = patientdetail[0].Human_Type;

                fillGrid();
            }

        }

        protected void btnGetAccNo_Click(object sender, EventArgs e)
        {
            txtAccount.Text = hdnAccNo.Value;// txtAccount.Text;

            //Bug id :26980
            //if (txtAccount.Text!="")
            //    ClientSession.HumanId =Convert.ToUInt32(txtAccount.Text);
            PatientDetails();
        }
   }
}
