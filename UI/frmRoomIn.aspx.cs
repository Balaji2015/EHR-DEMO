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
using Acurus.Capella.Core.DomainObjects;
using Telerik.Web.UI;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Xml;
using System.IO;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmRoomIn : System.Web.UI.Page
    {
        RoomInLookupManager objRoomInLookupManager = new RoomInLookupManager();
        EncounterManager objEncounterManager = new EncounterManager();
        string ulObjectsystemID = string.Empty;
        string objectType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Room in" + " - " + ClientSession.UserName;
            if (!IsPostBack)
            {
                hdnMessageType.Value = string.Empty;
                lblPatientAccountNumber.Text = Request["AccountNumber"].ToString();
                lblpatientName.Text = Request["PatientName"].ToString();
                DateTime dtDOB = Convert.ToDateTime(Request["PatientDOB"].ToString());
                lblPatientDOB.Text = dtDOB.ToString("dd-MMM-yyyy");
                lblPhysicianName.Text = Request.QueryString["AssignedPhysician"];
                lblApptDateTime.Text = Request.QueryString["AppointmentDateTime"];
                lblPurposeofVisit.Text = Request.QueryString["TypeOfVisit"];
                cboExamRoom.Items.Clear();
                string ExamRoom = Request.QueryString["ExamRoom"];
                btnOK.Enabled = false;

                //Jira CAP-2785
                //XmlDocument xmldoc = new XmlDocument();
                //if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "room_in_lookup.xml"))
                //{
                //    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "room_in_lookup.xml");
                //    XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("facility");
                //    if (xmlFacilityList != null)
                //    {
                //        foreach (XmlNode xmlCurrentFaacilityNode in xmlFacilityList)
                //        {
                //            if (xmlCurrentFaacilityNode.Attributes["name"].Value == ClientSession.FacilityName)
                //            {
                //                foreach (XmlNode xmlRoomNodes in xmlCurrentFaacilityNode.ChildNodes)
                //                {
                //                    cboExamRoom.Items.Add(new RadComboBoxItem(xmlRoomNodes.Attributes["Name"].Value));
                //                }
                //            }
                //        }
                //    }
                //}

                //Jira CAP-2785

                Room_in_lookupList room_In_LookupList = new Room_in_lookupList();
                room_In_LookupList = ConfigureBase<Room_in_lookupList>.ReadJson("room_in_lookup.json");

                if (room_In_LookupList != null && room_In_LookupList.facility != null)
                {
                    List<Facility> ilstExamRoomFromFac = new List<Facility>();
                    ilstExamRoomFromFac = room_In_LookupList.facility.Where(x => x.name == ClientSession.FacilityName).ToList();

                    if ((ilstExamRoomFromFac?.Count ?? 0) > 0)
                    {
                        foreach (ExamRoom examRoom in ilstExamRoomFromFac[0].exam_room)
                        {
                            cboExamRoom.Items.Add(new RadComboBoxItem(examRoom.Name));
                        }
                    }
                }

                //Jira CAP-2842
                Encounter encounter = new Encounter();
                EncounterManager encounterManager = new EncounterManager();
                encounter = encounterManager.GetById(Convert.ToUInt64(Request["EnounterID"]!= null? Request["EnounterID"]:"0"));

                //Jira CAP-2842
                //if (cboExamRoom.Items.Count > 0 && ExamRoom!=string.Empty)
                if (cboExamRoom.Items.Count > 0 && encounter != null)
                {
                    for (int i = 0; i < cboExamRoom.Items.Count; i++)
                    {
                        //Jira CAP-2842
                        //if (cboExamRoom.Items[i].Text.ToString().ToUpper() == ExamRoom.ToUpper())
                        if (cboExamRoom.Items[i].Text.ToString().ToUpper() == (encounter.Exam_Room??"").ToUpper())
                        {
                            cboExamRoom.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            objectType = Request.QueryString["objectType"];
            ulObjectsystemID = Request.QueryString["ulObjectsystemID"];
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            objEncounterManager.UpdateEncounterForRoom(Convert.ToUInt64(Request["EnounterID"]), cboExamRoom.Text, ClientSession.UserName, objectType, System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now), string.Empty);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SavedSuccessfully", "CloseWindow();", true);
            btnOK.Enabled = false;
        }
    }
}
