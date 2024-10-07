using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Web.Script.Services;
using System.Xml.Linq;

namespace Acurus.Capella.UI
{
    public partial class frmMyQueueNew : System.Web.UI.Page
    {
        public static int iDefaultDays
        {
            get;
            set;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Jira CAP-953
            if (ClientSession.UserPermissionDTO != null && ClientSession.UserPermissionDTO.Userscntab != null)
            {
                var MyuserScnTab = from c in ClientSession.UserPermissionDTO.Userscntab where c.scn_id == Convert.ToUInt64("101131") && c.user_name == ClientSession.UserName select c;

                if (MyuserScnTab != null && MyuserScnTab.ToList<user_scn_tab>().Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MyQueue", "{sessionStorage.setItem('IsAkidoPhysician', 'YES');}", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "MyQueue", "{sessionStorage.setItem('IsAkidoPhysician', 'NO');}", true);
                }
            }

            if (ClientSession.Is_All_Facilities.ToUpper() == "Y")
            {
                chkViewAllFacilities.Visible = true;
                lblViewAllFac.Visible= true;
                chkViewAllFacilities.Checked = true;
            }
            else
            {
                chkViewAllFacilities.Checked = false;
                chkViewAllFacilities.Visible = false;
                lblViewAllFac.Visible = false;
            }

            if (Request.Cookies["Human_Details"] != null)
            {
                Response.Cookies["Human_Details"].Expires = DateTime.Today.AddDays(-1);
            }
            ClientSession.IsDirtySocialHistory = false;
            ClientSession.HumanId = 0;
            ClientSession.EncounterId = 0;
            ClientSession.UserCurrentProcess = string.Empty;
            ClientSession.CurrentObjectType = string.Empty;
            ClientSession.FillEncounterandWFObject = null;
            ClientSession.FillPatientChart.Fill_Encounter_and_WFObject = null;
            iDefaultDays = DefaultDays();
        }
        public int DefaultDays()
        {
            int iDefaultDays = 0;
            string xmlFilepathUser = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\User.xml");
            if (File.Exists(xmlFilepathUser))
            {
                // XmlDocument xdoc = new XmlDocument();
                // XmlTextReader itext = new XmlTextReader(xmlFilepathUser);
                // xdoc.Load(itext);
                // itext.Close();
                // XmlNodeList xnodelst = xdoc.GetElementsByTagName("User");

                ////var att= from m in xnodelst where m.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && m.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != ClientSession.PhysicianId.ToString
                // if (xnodelst != null && xnodelst.Count > 0)
                // {
                //     foreach (XmlNode xnode in xnodelst)
                //     {
                //         if (xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() != "0" && xnode.Attributes.GetNamedItem("Physician_Library_ID").Value.ToString() == ClientSession.PhysicianId.ToString())
                //         {
                //             if (xnode.Attributes.GetNamedItem("Default_MyQ_Days").Value != "")
                //                 iDefaultDays = Convert.ToInt32(xnode.Attributes.GetNamedItem("Default_MyQ_Days").Value);
                //         }
                //     }
                // }

                XDocument xmluserdoc = XDocument.Load(xmlFilepathUser);

                IEnumerable<XElement> xmluser = xmluserdoc.Element("UserList")
                    .Elements("User").Where(aa => aa.Attribute("Physician_Library_ID").Value.ToString() != "0" && aa.Attribute("Physician_Library_ID").Value.ToString() == ClientSession.PhysicianId.ToString());

                if (xmluser != null && xmluser.Count() > 0)
                {
                    iDefaultDays = Convert.ToInt32(xmluser.Attributes("Default_MyQ_Days").First().Value);
                }


            }
            return iDefaultDays;
        }
        [WebMethod(EnableSession = true)]
        public static string EncounterLoad(string sShowall)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue EncounterLoad : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            string[] ProcessType = new string[2];
            ProcessType[0] = "ASSIGNED";
            string[] ObjType = new string[2];
            ObjType[0] = "ENCOUNTER";
            ObjType[1] = "DOCUMENTATION";
            //ObjType[2] = "DOCUMENT REVIEW";
            IList<MyQ> PatientQ;

            WFObjectManager wfMngr = new WFObjectManager();
            Hashtable LoadMyQ = new Hashtable();
            IList<MyQueueCountDTO> QCount = new List<MyQueueCountDTO>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue EncounterLoad : LoadMyQHashTable DB call Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            LoadMyQ = wfMngr.LoadMyQHashTable("ALL", ObjType, ProcessType, ClientSession.UserName, false, iDefaultDays, ClientSession.FacilityName);//ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue EncounterLoad : LoadMyQHashTable DB call End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            PatientQ = (IList<MyQ>)LoadMyQ["MyQ"];
            var pat = new List<MyQ>();
            QCount = (IList<MyQueueCountDTO>)LoadMyQ["Qcount"];

            if (PatientQ != null)
            {
                //  pat = PatientQ.Where(a => a.Current_Owner != "UNKNOWN" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && (a.EHR_Obj_Type == "ENCOUNTER" || a.EHR_Obj_Type == "DOCUMENTATION" || a.EHR_Obj_Type == "DOCUMENT REVIEW" || a.EHR_Obj_Type == "PHONE ENCOUNTER")).ToList<MyQ>();
                pat = PatientQ.Where(a => a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && (a.EHR_Obj_Type == "ENCOUNTER" || a.EHR_Obj_Type == "DOCUMENTATION" || a.EHR_Obj_Type == "DOCUMENT REVIEW" || a.EHR_Obj_Type == "PHONE ENCOUNTER")).ToList<MyQ>();

                pat = pat.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
            }

            string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();

            //if (ClientSession.FacilityName == sAncillary.Trim())
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                sAncillary = "true";
                //var patnew = new List<MyQ>();
                //foreach (MyQ order in pat)
                //{
                //    if (order.Order_Submit_ID != 0)
                //    {
                //        OrdersSubmitManager objorders = new OrdersSubmitManager();
                //        order.Test_Details = objorders.GetTestUsingOrderSubmitID(order.Order_Submit_ID);
                //        order.Ordering_Physician = objorders.GetOrderingPhysicianByOrderSubmitID(order.Order_Submit_ID);//,order.Encounter_ID);//BugID:52738 

                //    }
                //    patnew.Add(order);
                //}
                //pat = patnew;
            }
            else
            {
                sAncillary = "false";
            }

            ulong Encountershowallcount = 0;
            Acurus.Capella.DataAccess.ManagerObjects.ObjectManager objMngr = new Acurus.Capella.DataAccess.ManagerObjects.ObjectManager();
            Encountershowallcount = Convert.ToUInt32(LoadMyQ["EncounterShowallCount"]); //objMngr.GetEncountershowallcount(ProcessType[0], ClientSession.UserName, ObjType);

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue EncounterLoad : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var result = new { data = pat, count = QCount, role = ClientSession.UserRole, Ancillary = sAncillary, EncounterCount = Encountershowallcount };
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod(EnableSession = true)]
        public static string MyEncounterLoad(string sShowall,string sViewAllFacilities)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            if (ClientSession.UserRole != "Medical Assistant" && ClientSession.UserRole != "Front Office" && ClientSession.UserRole != "Surgery Coordinator" && ClientSession.UserRole.ToUpper() != "SCRIBE")//BugID:53790
            {
                string sAncillary = string.Empty;
                //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                //{
                //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
                //}
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();


                string[] ProcessType = new string[2];
                ProcessType[0] = "ASSIGNED";

                string[] ObjType = new string[3];
                ObjType[0] = "ENCOUNTER";
                ObjType[1] = "DOCUMENTATION";
                ObjType[2] = "DOCUMENT REVIEW";
                IList<MyQ> PatientQ;

                WFObjectManager wfMngr = new WFObjectManager();
                Hashtable LoadMyQ = new Hashtable();
                IList<MyQueueCountDTO> QCount = new List<MyQueueCountDTO>();
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad LoadMyQHashTable DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                LoadMyQ = wfMngr.LoadMyQHashTable("ALL", ObjType, ProcessType, ClientSession.UserName, false, iDefaultDays, ClientSession.FacilityName);//ClientSession.DefaultNoofDays);
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad LoadMyQHashTable DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

                PatientQ = (IList<MyQ>)LoadMyQ["MyQ"];
                var pat = new List<MyQ>();
                QCount = (IList<MyQueueCountDTO>)LoadMyQ["Qcount"];
                ulong Encountershowallcount = Convert.ToUInt32(LoadMyQ["EncounterShowallCount"]);
                if (PatientQ != null)
                {
                    //pat = PatientQ.Where(a => a.Current_Owner != "UNKNOWN" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && (a.EHR_Obj_Type == "ENCOUNTER" || a.EHR_Obj_Type == "DOCUMENTATION" || a.EHR_Obj_Type == "DOCUMENT REVIEW" || a.EHR_Obj_Type == "PHONE ENCOUNTER")).ToList<MyQ>();

                    pat = PatientQ.Where(a => a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && (a.EHR_Obj_Type == "ENCOUNTER" || a.EHR_Obj_Type == "DOCUMENTATION" || a.EHR_Obj_Type == "DOCUMENT REVIEW" || a.EHR_Obj_Type == "PHONE ENCOUNTER")).ToList<MyQ>();
                    pat = pat.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
                }
                //if (ClientSession.FacilityName == sAncillary.Trim())
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    sAncillary = "true";
                    //var patnew = new List<MyQ>();
                    //foreach (MyQ order in pat)
                    //{

                    //    if (order.Order_Submit_ID != 0)
                    //    {
                    //        OrdersSubmitManager objorders = new OrdersSubmitManager();
                    //        order.Test_Details = objorders.GetTestUsingOrderSubmitID(order.Order_Submit_ID);
                    //        order.Ordering_Physician = objorders.GetOrderingPhysicianByOrderSubmitID(order.Order_Submit_ID);//BugID:52738 

                    //    }
                    //    patnew.Add(order);
                    //}
                    //pat = patnew;
                }
                else
                {
                    sAncillary = "false";
                }
                var result = new { data = pat, count = QCount, role = ClientSession.UserRole, Ancillary = sAncillary, EncounterCount = Encountershowallcount };
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                return JsonConvert.SerializeObject(result);

            }
            else
            {
                string[] ProcessType = new string[2];
                ProcessType[0] = "UNASSIGNED";

                string[] ObjType = new string[2];
                ObjType[0] = "ENCOUNTER";
                ObjType[1] = "DOCUMENTATION";

                IList<MyQ> PatientQ;
                WFObjectManager wfMngr = new WFObjectManager();
                Hashtable LoadMyQ = new Hashtable();
                IList<MyQueueCountDTO> QCount = new List<MyQueueCountDTO>();
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad LoadMyQHashTable DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                if (sViewAllFacilities == "Checked")
                {
                    LoadMyQ = wfMngr.LoadMyQHashTable("ViewAllFacilities~"+ ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, false, iDefaultDays, "");//ClientSession.DefaultNoofDays);
                }
                else
                {
                    LoadMyQ = wfMngr.LoadMyQHashTable(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, false, iDefaultDays, "");//ClientSession.DefaultNoofDays);
                }
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad LoadMyQHashTable DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                PatientQ = (IList<MyQ>)LoadMyQ["MyQ"];
                var pat = new List<MyQ>();
                QCount = (IList<MyQueueCountDTO>)LoadMyQ["Qcount"];
                // ulong Encountershowallcount=Convert.ToUInt32(LoadMyQ["EncounterShowallCount"]);
                if (PatientQ != null)
                {
                    //pat = PatientQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "INTERNAL ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                    //pat = PatientQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                    pat = PatientQ.Where(a => a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT").ToList<MyQ>();
                    pat = pat.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
                }
                /* To Load Exam Room Content */
                var eRoomList = new List<String>();
                XmlDocument xmldoc = new XmlDocument();
                if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "room_in_lookup.xml"))
                {
                    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "room_in_lookup.xml");
                    XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("facility");
                    if (xmlFacilityList != null)
                    {
                        foreach (XmlNode xmlCurrentFaacilityNode in xmlFacilityList)
                        {
                            if (xmlCurrentFaacilityNode.Attributes["name"].Value == ClientSession.FacilityName)
                            {
                                foreach (XmlNode xmlRoomNodes in xmlCurrentFaacilityNode.ChildNodes)
                                {
                                    eRoomList.Add(xmlRoomNodes.Attributes["Name"].Value);
                                }
                            }
                        }
                    }
                }
                //var result = new { data = pat, count = QCount, dataEroom = eRoomList, role = ClientSession.UserRole, EncounterCount = Encountershowallcount };
                var result = new { data = pat, count = QCount, dataEroom = eRoomList, role = ClientSession.UserRole };
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue MyEncounterLoad : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                return JsonConvert.SerializeObject(result);
            }
        }
        [WebMethod(EnableSession = true)]
        public static string LoadMyTaskCompleted(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[1];
            ObjType[0] = "TASK";
            IList<MyQ> MyTask;
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask GetListObjects DB call : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            MyTask = wfMngr.GetListObjectsCompleted("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);//ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask GetListObjects DB call : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var TaskQ = from g in MyTask where g.Message_Description != string.Empty select g;
            IList<MyQ> TaskList = TaskQ.ToList<MyQ>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(TaskQ.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string LoadMyTask(string sShowall, string sOpenTask = "")
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[1];
            ObjType[0] = "TASK";
            IList<MyQ> MyTask;
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask GetListObjects DB call : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            if (sOpenTask == "Checked")
            {
                MyTask = wfMngr.GetListObjectsOpenTaskCreatedByMe("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);
            }
            else
            {
                MyTask = wfMngr.GetListObjects("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);//ClientSession.DefaultNoofDays);
            }

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask GetListObjects DB call : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyTask : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(MyTask.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string LoadMyOrder(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyOrder : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[7];
            ObjType[0] = "DIAGNOSTIC ORDER";
            ObjType[1] = "DME ORDER";
            // ObjType[2] = "INTERNAL ORDER";//For Bug Id 54510
            ObjType[3] = "IMMUNIZATION ORDER";
            ObjType[4] = "REFERRAL ORDER";
            //$muthusamy on 16-12-2014 for LabResult Changes
            ObjType[5] = "DIAGNOSTIC_RESULT";
            ObjType[6] = "DME ORDER";
            //$muthusamy 
            IList<MyQ> MyHome;

            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyOrder GetListObjects DB call : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            MyHome = wfMngr.GetListObjects("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);//ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyOrder GetListObjects DB call : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var MyOrdersQ = from g in MyHome where g.Current_Owner != "UNKNOWN" orderby g.Created_Date_And_Time descending select g;
            MyOrdersQ = from g in MyOrdersQ orderby g.Is_Abnormal descending select g;
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyOrder : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(MyOrdersQ.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string LoadMyScan(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyScan : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[2];
            ObjType[0] = "SCAN";
            // ObjType[1] = "SCAN RESULT";
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyScan GetListObjects DB call : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            IList<MyQ> MyHome = wfMngr.GetListObjects("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);//ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyScan GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var MyScanList = from g in MyHome where g.Current_Owner != "UNKNOWN" orderby g.Scanned_Date descending select g;
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyScan : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(MyScanList.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string LoadMyPrescription(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyPrescription : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[1];
            ObjType[0] = "E-PRESCRIBE";

            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyPrescription GetListObjects DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            IList<MyQ> MyHome = wfMngr.GetListObjects("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);//ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyPrescription GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var MyPrescList = from g in MyHome where g.Current_Owner != "UNKNOWN" select g;
            MyPrescList = MyPrescList.OrderByDescending(a => a.Prescription_Date);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyPrescription : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(MyPrescList.ToList<MyQ>());
        }

        [WebMethod(EnableSession = true)]
        public static string OpenAmendment()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            if (ClientSession.EncounterId == 0)
            {
                return "7490013";
            }
            //Added the Manager Call to fill the Session Object since this is not filled on opening frmSummaryNew.
            EncounterManager objEncounterManager = new EncounterManager();
            //ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, true, 0, "ADDENDUM", true);//0);
            //ClientSession.FillEncounterandWFObject = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject;
            //ClientSession.PatientPaneList = ClientSession.FillPatientChart.PatChartList;

            FillPatientChart objFillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, true, 0, "ADDENDUM", true);//0);
            FillEncounterandWFObject objFillEncounterandWFObject = objFillPatientChart.Fill_Encounter_and_WFObject;

            if (ClientSession.UserRole.ToUpper() == "CODER" && objFillEncounterandWFObject == null)
            {
                string[] ProcessType = new string[1];
                ProcessType[0] = "ASSIGNED";

                string[] ObjType = new string[1];
                ObjType[0] = "ADDENDUM";

                IList<MyQ> myQ;
                var serializer = new NetDataContractSerializer();
                WFObjectManager objWFObjectManager = new WFObjectManager();
                myQ = objWFObjectManager.GetListofObjects("ALL", ObjType, ProcessType, ClientSession.UserName, true, iDefaultDays);//ClientSession.DefaultNoofDays);
                if (myQ.Count < 1)
                {
                    //DislayErrorMessage
                    return "7490009";
                }
                else
                {
                    if (objFillEncounterandWFObject != null)
                    {
                        return "OpenAddendumForm";
                    }
                }
            }
            if (objFillEncounterandWFObject.DocumentationWFRecord.Current_Process == string.Empty)
            {
                //DislayErrorMessage
                return "7490006";
            }
            if (ApplicationObject.processMasterList.Any(a => a.Process_Name == objFillPatientChart.Fill_Encounter_and_WFObject.DocumentationWFRecord.Current_Process && a.Is_Addendum_Allowed == "N"))
            {
                //DislayErrorMessage
                return "7490006";
            }

            if (objFillEncounterandWFObject != null)
            {
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {
                    //ModalWindow.Height = 470;
                }
                return "OpenAddendumForm";

            }
            else
                return "7490006";
        }

        [WebMethod(EnableSession = true)]
        public static string LoadMyAmendment(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyAmendment : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[1];
            ObjType[0] = "ADDENDUM";

            IList<MyQ> MyQ;

            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyAmendment GetListObjects DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            MyQ = wfMngr.GetListObjects("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyAmendment GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            MyQ = (MyQ.OrderByDescending(a => a.Appt_Date_Time)).ToList<MyQ>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyAmendment : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(MyQ.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string chkShowAllMyEncounter(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyEncounter ShowAll : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "ASSIGNED";

            string[] ObjType = new string[4];
            ObjType[0] = "ENCOUNTER";
            ObjType[1] = "DOCUMENTATION";
            //  ObjType[2] = "PHONE ENCOUNTER";
            ObjType[2] = "DOCUMENT REVIEW";

            IList<MyQ> MyQ;
            WFObjectManager wfMngr = new WFObjectManager();
            string sAncillary = string.Empty;
            MyQ = wfMngr.GetListObjects("ALL", ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, ClientSession.FacilityName);// ClientSession.DefaultNoofDays);
            if (MyQ != null && MyQ.Count > 0)
            {
                MyQ = MyQ.Where(a => a.Current_Process != "DICTATION_WAIT" && a.Current_Process != "DICTATION_EXCEPTION").ToList<MyQ>();
                MyQ = MyQ.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
                //For ancillary to change the "facility" column  to "test details"

                //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
                //{
                //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
                //}
                var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                //if (ClientSession.FacilityName == sAncillary.Trim())
                if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                {
                    sAncillary = "true";
                    //var patnew = new List<MyQ>();
                    //foreach (MyQ order in MyQ)
                    //{
                    //    if (order.Order_Submit_ID != 0)
                    //    {
                    //        OrdersSubmitManager objorders = new OrdersSubmitManager();
                    //        order.Test_Details = objorders.GetTestUsingOrderSubmitID(order.Order_Submit_ID);
                    //        order.Ordering_Physician = objorders.GetOrderingPhysicianByOrderSubmitID(order.Order_Submit_ID);//BugID:52738 
                    //    }
                    //    patnew.Add(order);
                    //}
                    //MyQ = patnew;
                }
                else
                {
                    sAncillary = "false";
                }
            }
            ulong Encountershowallcount = 0;
            Acurus.Capella.DataAccess.ManagerObjects.ObjectManager objMngr = new Acurus.Capella.DataAccess.ManagerObjects.ObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyEncounter ShowAll GetEncountershowallcount DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            Encountershowallcount = objMngr.GetEncountershowallcount(ProcessType[0], ClientSession.UserName, ObjType, ClientSession.FacilityName);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyEncounter ShowAll GetEncountershowallcount DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var result = new { data = MyQ.ToList<MyQ>(), Ancillary = sAncillary, EncounterCount = Encountershowallcount };
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadMyEncounter ShowAll : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(result);
            // return JsonConvert.SerializeObject(MyQ.ToList<MyQ>());
        }

        [WebMethod(EnableSession = true)]
        public static bool ProcessEncounter(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }
            //string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessEncounter : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            EncounterManager EncMngr = new EncounterManager();
            //ClientSession.TemplateId = 0;
            ClientSession.HumanId = Convert.ToUInt64(data[0]);
            ClientSession.EncounterId = Convert.ToUInt64(data[1]);
            //string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            //if (data[2].Trim().ToUpper() != "CMG LAB AND ANCILLARY")
            //if (data[2].Trim().ToUpper() != sAncillary)
            // if (ClientSession.FacilityName.Trim().ToUpper() != sAncillary)
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
            {
                ClientSession.PhysicianId = Convert.ToUInt64(data[3]);
            }
            ClientSession.CurrentObjectType = data[5].ToString();
            ClientSession.UserCurrentProcess = data[4].ToString();
            ClientSession.Selectedencounterid = Convert.ToUInt64(data[1]);
            DateTime dt = Convert.ToDateTime(data[6].ToString());
            DateTime apptDt = Convert.ToDateTime(data[8].ToString());
            string localTime = data[7];
            string sLocal_Time = UtilityManager.ConvertToLocal(Convert.ToDateTime(localTime)).ToString("yyyy-MM-dd hh:mm:ss tt");
            bool bIsCheckinProvider = false;
            /*if (ClientSession.UserCurrentProcess == "MA_PROCESS" || ClientSession.UserCurrentProcess == "TECHNICIAN_PROCESS")*///to update DOS when Technician/MA processes the encounter for the first time -- CMG Ancilliary
            if (ClientSession.UserCurrentProcess == "MA_PROCESS" || ClientSession.UserCurrentProcess == "TECHNICIAN_PROCESS" || ClientSession.UserCurrentProcess == "PROVIDER_PROCESS") //to update DOS when Technician/MA/Provider processes the encounter for the first time -- CMG Ancilliary 
            {
                if (data[6] == "0001-01-01T00:00:00")
                {
                    //Jira CAP-2129
                    if (ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
                    {
                        bIsCheckinProvider = true;
                    }
                    //if (data[7].ToString() != string.Empty && Convert.ToDateTime(data[7].ToString().Split(' ')[0]) == Convert.ToDateTime(apptDt.ToString().Split(' ')[0]))
                    //{
                    //    dt = Convert.ToDateTime(data[7].ToString());
                    //}
                    //else
                    //{
                    //    dt = apptDt;
                    //}

                    if (sLocal_Time != string.Empty && Convert.ToDateTime(sLocal_Time.Split(' ')[0]) == Convert.ToDateTime(apptDt.ToString().Split(' ')[0]))
                    {
                        dt = Convert.ToDateTime(data[7].ToString());
                    }
                    else
                    {
                        dt = UtilityManager.ConvertToUniversal(apptDt);
                        sLocal_Time = apptDt.ToString("yyyy-MM-dd hh:mm:ss tt");
                    }

                    //string FileName = "Human" + "_" + data[0] + ".xml";
                    //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                    XmlTextReader XmlText = null;

                    XmlDocument xmlDoc = new XmlDocument();
                    string sXMLContent = String.Empty;

                //if (File.Exists(strXmlFilePath) == true)
                //{
                loop:
                    try
                    {


                        //XmlDocument itemDoc = new XmlDocument();
                        //XmlText = new XmlTextReader(strXmlFilePath);
                        //itemDoc.Load(XmlText);
                        //XmlText.Close();

                        HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                        IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(data[0]));
                        if (ilstHumanBlob.Count > 0)
                        {
                            sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                            if (sXMLContent.Substring(0, 1) != "<")
                                sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                            xmlDoc.LoadXml(sXMLContent);
                        }
                        else
                        {
                            throw new Exception("Human XML is not found");
                        }


                    }
                    catch (Exception ex)
                    {
                        //sXmlText.Close();
                        UtilityManager.GenerateXML(data[0], "Human");
                        goto loop;
                    }
                //}



                //FileName = "Encounter" + "_" + data[1] + ".xml";
                //strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                // XmlTextReader XmlText = null;

                // if (File.Exists(strXmlFilePath) == true)
                // {
                ln:
                    try
                    {


                        //XmlDocument itemDoc = new XmlDocument();
                        //XmlText = new XmlTextReader(strXmlFilePath);
                        //itemDoc.Load(XmlText);
                        //XmlText.Close();

                        EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                        IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(Convert.ToUInt64(data[1]));

                        if (ilstEncounterBlob.Count > 0)
                        {

                            try
                            {
                                sXMLContent = System.Text.Encoding.UTF8.GetString(ilstEncounterBlob[0].Encounter_XML);
                                if (sXMLContent.Substring(0, 1) != "<")
                                    sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                                xmlDoc.LoadXml(sXMLContent);
                            }
                            catch
                            {
                                throw new Exception("Encounter XML is invalid");
                            }
                        }
                        //else
                        //{
                        //    throw new Exception("Encounter XML is not found");
                        //}


                    }
                    catch (Exception ex)
                    {
                        //XmlText.Close();
                        UtilityManager.GenerateXML(data[1], "Encounter");
                        goto ln;
                    }
                    //}


                    EncMngr.UpdateDateOfService(Convert.ToUInt64(data[1]), dt, string.Empty, sLocal_Time);
                }
            }
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessEncounter : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            //Jira CAP-2129
            //if (ClientSession.UserCurrentProcess == "TECHNICIAN_PROCESS")//BugID:52898
            if (ClientSession.UserCurrentProcess == "TECHNICIAN_PROCESS" || bIsCheckinProvider)//BugID:52898
                frmRCopiaToolbar.LoadNotification("All");
            return true;
        }
        [WebMethod(EnableSession = true)]
        public static bool ProcessAddendum(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }
            ClientSession.SetSelectedTab = string.Empty;
            if (data[1] != string.Empty)
                ClientSession.EncounterId = Convert.ToUInt64(data[1]);
            if (data[2] != string.Empty)
                ClientSession.PhysicianId = Convert.ToUInt64(data[2]);
            if (data[0] != string.Empty)
                ClientSession.HumanId = Convert.ToUInt64(data[0]);
            ClientSession.CurrentObjectType = data[4].ToString();
            ClientSession.UserCurrentProcess = data[3].ToString();

            ProcessMaster ProcMasterRecord;

            var proc = from p in ApplicationObject.processMasterList where p.Process_Name == data[3].ToString() select p;
            ProcMasterRecord = proc.ToList<ProcessMaster>()[0];

            //Jira #CAP-580
            ClientSession.Selectedencounterid = 0;
            return true;
        }
        [WebMethod(EnableSession = true)]
        public static bool ProcessPrescription(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }
            return true;
        }
        [WebMethod(EnableSession = true)]
        public static bool ProcessTask(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;  
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }
            if (data.Contains("GeneralQ") == true)
            {
                WFObjectManager wfMngr = new WFObjectManager();
                PatientNotesManager patnotesMngr = new PatientNotesManager();

                wfMngr.UpdateOwner(Convert.ToUInt64(data[4]), data[3], ClientSession.UserName, string.Empty);
                patnotesMngr.UpdateAssignTo(Convert.ToUInt64(data[4]), ClientSession.UserName);
            }
            ClientSession.UserCurrentList.Add(ClientSession.UserName);
            ClientSession.UserCurrentOwner = ClientSession.UserName;
            if (data[0] != "")
                ClientSession.HumanId = Convert.ToUInt32(data[0]);
            // ClientSession.FacilityName = data[1];
            return true;
        }
        [WebMethod(EnableSession = true)]
        public static bool ProcessOrder(string CurrentProcess)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }
            //  ClientSession.UserCurrentProcess = CurrentProcess;
            ArrayList tempList = ClientSession.UserCurrentList;
            tempList.Add(ClientSession.UserName);
            ClientSession.UserCurrentList = tempList;
            ClientSession.UserCurrentOwner = ClientSession.UserName;



            return true;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadEncounter(string sViewAllFacilities)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounter : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            string[] ProcessType = new string[2];
            ProcessType[0] = "UNASSIGNED";

            string[] ObjType = new string[2];
            ObjType[0] = "ENCOUNTER";
            ObjType[1] = "DOCUMENTATION";
            IList<MyQ> PatientQ;
            WFObjectManager wfMngr = new WFObjectManager();
            Hashtable LoadMyQ = new Hashtable();
            IList<MyQueueCountDTO> QCount = new List<MyQueueCountDTO>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounter LoadMyQHashTable DB call : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            if (sViewAllFacilities == "Checked")
            {
                LoadMyQ = wfMngr.LoadMyQHashTable("ViewAllFacilities~" + ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, false, iDefaultDays, "");// ClientSession.DefaultNoofDays);
            }
            else
            {
                LoadMyQ = wfMngr.LoadMyQHashTable(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, false, iDefaultDays, "");// ClientSession.DefaultNoofDays);
            }
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounter LoadMyQHashTable DB call : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            PatientQ = (IList<MyQ>)LoadMyQ["MyQ"];
            var pat = new List<MyQ>();
            QCount = (IList<MyQueueCountDTO>)LoadMyQ["Qcount"];
            if (PatientQ != null)
            {
                //pat = PatientQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "INTERNAL ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                //pat = PatientQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                pat = PatientQ.Where(a => a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT").ToList<MyQ>();
                pat = pat.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
            }
            /* To Load Exam Room Content */
            var eRoomList = new List<String>();
            XmlDocument xmldoc = new XmlDocument();
            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "room_in_lookup.xml"))
            {
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "room_in_lookup.xml");
                XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("facility");
                if (xmlFacilityList != null)
                {
                    foreach (XmlNode xmlCurrentFaacilityNode in xmlFacilityList)
                    {
                        if (xmlCurrentFaacilityNode.Attributes["name"].Value == ClientSession.FacilityName)
                        {
                            foreach (XmlNode xmlRoomNodes in xmlCurrentFaacilityNode.ChildNodes)
                            {
                                eRoomList.Add(xmlRoomNodes.Attributes["Name"].Value);
                            }
                        }
                    }
                }
            }
            //  ulong Encountershowallcount = 0;
            //  Acurus.Capella.DataAccess.ManagerObjects.ObjectManager objMngr = new Acurus.Capella.DataAccess.ManagerObjects.ObjectManager();
            // Encountershowallcount = objMngr.GetEncountershowallcount(ProcessType[0], ClientSession.UserName, ObjType, ClientSession.FacilityName);

            //var result = new { data = pat, count = QCount, dataEroom = eRoomList, role = ClientSession.UserRole, EncounterCount = Encountershowallcount };
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounter : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            var result = new { data = pat, count = QCount, dataEroom = eRoomList, role = ClientSession.UserRole };
            return JsonConvert.SerializeObject(result);
        }
        [WebMethod(EnableSession = true)]
        public static string MoveToMyOrder(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            //Jira CAP-1123
            IList<MyQ> OrdersQ = new List<MyQ>();
            for (int i = 0; i < data.Length; i++)
            {
                string[] dataMove = data[i].Split('~');
                WFObjectManager wfMngr = new WFObjectManager();
                ulong selectedID = Convert.ToUInt64(dataMove[0]);
                if (selectedID != 0)
                {
                    wfMngr.UpdateOwner(Convert.ToUInt64(dataMove[1]), dataMove[2].ToString(), ClientSession.UserName, string.Empty);

                }
                if (selectedID == 0)
                {
                    wfMngr.UpdateOwner(Convert.ToUInt64(dataMove[1]), dataMove[2].ToString(), ClientSession.UserName, string.Empty);
                }
                //LoadOrders();


                //For Refresh Movetomyorder
                bool bValue = false;
                if (dataMove[3].ToString() == "Checked")
                    bValue = true;
                string[] ProcessType = new string[2];
                ProcessType[0] = "UNASSIGNED";

                string[] ObjType = new string[6];
                ObjType[0] = "DIAGNOSTIC ORDER";
                ObjType[1] = "DME ORDER";
                // ObjType[1] = "IMAGE ORDER";
                // ObjType[2] = "INTERNAL ORDER";//For Bug Id 54510
                ObjType[3] = "IMMUNIZATION ORDER";
                ObjType[4] = "REFERRAL ORDER";
                ObjType[5] = "DME ORDER";
                
                OrdersQ = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
                OrdersQ = OrdersQ.Where(a => a.Current_Owner == "UNKNOWN").ToList<MyQ>();
            }
            return JsonConvert.SerializeObject(OrdersQ.ToList<MyQ>());
        }

        [WebMethod(EnableSession = true)]
        public static string LoadOrder(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadOrder : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[2];
            ProcessType[0] = "UNASSIGNED";

            string[] ObjType = new string[6];
            ObjType[0] = "DIAGNOSTIC ORDER";
            ObjType[1] = "DME ORDER";
            // ObjType[1] = "IMAGE ORDER";
            //ObjType[2] = "INTERNAL ORDER";//For Bug Id 54510
            ObjType[3] = "IMMUNIZATION ORDER";
            ObjType[4] = "REFERRAL ORDER";
            ObjType[5] = "DME ORDER";
            IList<MyQ> OrdersQ = new List<MyQ>();
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadOrder GetListObjects DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            OrdersQ = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadOrder GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            OrdersQ = OrdersQ.Where(a => a.Current_Owner == "UNKNOWN").ToList<MyQ>();
            OrdersQ = OrdersQ.OrderByDescending(a => a.Created_Date_And_Time).ToList<MyQ>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadOrder : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(OrdersQ.ToList<MyQ>());
        }
        //Commented for BugId:36500
        //[WebMethod(EnableSession = true)]
        //public static string LoadScan(string sShowall)
        //{
        //    //bool bValue = false;
        //    //if (sShowall == "Checked")
        //    //    bValue = true;
        //    //string[] ProcessType = new string[1];
        //    //ProcessType[0] = "UNASSIGNED";
        //    //string[] ObjType = new string[2];
        //    //ObjType[0] = "SCAN";
        //    //ObjType[1] = "SCAN RESULT";
        //    //WFObjectManager wfMngr = new WFObjectManager();    
        //    //IList<MyQ> ScanQ = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, ClientSession.DefaultNoofDays);
        //    //var ScanList = from s in ScanQ where s.Current_Owner == "UNKNOWN" orderby s.Scanned_Date descending select s;
        //    //return JsonConvert.SerializeObject(ScanList.ToList<MyQ>());
        //}
        [WebMethod(EnableSession = true)]
        public static string LoadAmend(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadAddendum : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] processType = new string[2];
            processType[0] = "UNASSIGNED";
            string[] ObjType = new string[1];
            ObjType[0] = "ADDENDUM";
            IList<MyQ> addendumGeneralQList;
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadAddendum GetListObjects DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            addendumGeneralQList = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, processType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadAddendum GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            var addendumGeneralQ = from p in addendumGeneralQList where p.Current_Owner == "UNKNOWN" select p;
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadAddendum : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(addendumGeneralQ.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string LoadEncounterTabClick(string sShowall,string sViewAllFacilities)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounterTabClick : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[1];
            ProcessType[0] = "UNASSIGNED";

            string[] ObjType = new string[2];
            ObjType[0] = "ENCOUNTER";
            ObjType[1] = "DOCUMENTATION";
            //  ObjType[2] = "PHONE ENCOUNTER";

            IList<MyQ> MyQ;
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounterTabClick GetListObjects DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            if (sViewAllFacilities == "Checked")
            {
                MyQ = wfMngr.GetListObjects("ViewAllFacilities~" + ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays); 
            }
            else
            {
                MyQ = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays); 
            }

                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounterTabClick GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                if (MyQ != null && MyQ.Count > 0)
                {
                    //MyQ = MyQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "INTERNAL ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                    MyQ = MyQ.Where(a => a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT").ToList<MyQ>();

                    //  MyQ = MyQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                    MyQ = MyQ.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
                }

                //ulong Encountershowallcount = 0;
                //Acurus.Capella.DataAccess.ManagerObjects.ObjectManager objMngr = new Acurus.Capella.DataAccess.ManagerObjects.ObjectManager();
                //Encountershowallcount = objMngr.GetEncountershowallcount(ProcessType[0], ClientSession.UserName, ObjType, ClientSession.FacilityName);



                //var result = new { data = MyQ, role = ClientSession.UserRole, EncounterCount = Encountershowallcount };
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadEncounterTabClick : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                var result = new { data = MyQ, role = ClientSession.UserRole };
                return JsonConvert.SerializeObject(result);
            }



        //Commented for BugId:36500
        //[WebMethod(EnableSession = true)]
        //public static string MoveToMyScan(string[] data)
        //{            
        //    WFObjectManager wfMngr = new WFObjectManager();
        //    ulong selectedID = Convert.ToUInt32(data[0]);
        //    bool bValue=false;
        //    IList<WFObject> lstwfObject = wfMngr.GetListofObjectsByObySystemIdforScan(selectedID);
        //    if (lstwfObject != null && lstwfObject.Count > 0)
        //    {
        //        if (lstwfObject[0].Current_Owner.Trim().ToUpper() == "UNKNOWN")
        //        {
        //            if (data[1] == "RESULT_ENTRY")
        //                wfMngr.UpdateOwner(selectedID, "SCAN RESULT", ClientSession.UserName, string.Empty);
        //            else
        //                wfMngr.UpdateOwner(selectedID, "SCAN", ClientSession.UserName, string.Empty);                                        
        //        }                
        //    }
        //    string[] ProcessType = new string[1];
        //    ProcessType[0] = "UNASSIGNED";
        //    string[] ObjType = new string[2];
        //    ObjType[0] = "SCAN";
        //    ObjType[1] = "SCAN RESULT";
        //    if (data[2].ToString() == "Checked")
        //        bValue = true;
        //    IList<MyQ> ScanQ = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, ClientSession.DefaultNoofDays);
        //    var ScanList = from s in ScanQ where s.Current_Owner == "UNKNOWN" orderby s.Scanned_Date descending select s;
        //    return JsonConvert.SerializeObject(ScanList.ToList<MyQ>());
        //}
        [WebMethod(EnableSession = true)]
        public static string MoveToMyAmendment(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            WFObjectManager wfMngr = new WFObjectManager();
            for (int iCount = 0; iCount < data.Length; iCount++)
            {
                //wfMngr.UpdateOwner(Convert.ToUInt64(data[1]), data[0], ClientSession.UserName, string.Empty);
                wfMngr.UpdateOwner(Convert.ToUInt64(data[iCount].Split('~')[1]), data[iCount].Split('~')[0], ClientSession.UserName, string.Empty);
            }
            string[] processType = new string[2];
            processType[0] = "UNASSIGNED";
            string[] ObjType = new string[1];
            ObjType[0] = "ADDENDUM";
            IList<MyQ> addendumGeneralQList;
            addendumGeneralQList = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, processType, ClientSession.UserName, false, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
            var addendumGeneralQ = from p in addendumGeneralQList where p.Current_Owner == "UNKNOWN" select p;
            return JsonConvert.SerializeObject(addendumGeneralQ.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string MoveToMyTask(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            WFObjectManager wfMngr = new WFObjectManager();
            PatientNotesManager patnotesMngr = new PatientNotesManager();

            for (int iCount = 0; iCount < data.Length; iCount++)
            {
                //wfMngr.UpdateOwner(Convert.ToUInt64(data[1]), data[0], ClientSession.UserName, string.Empty);
                wfMngr.UpdateOwner(Convert.ToUInt64(data[iCount].Split('~')[1]), data[iCount].Split('~')[0], ClientSession.UserName, string.Empty);

                patnotesMngr.UpdateAssignTo(Convert.ToUInt64(data[iCount].Split('~')[1]), ClientSession.UserName);
            }
            //string[] processType = new string[2];
            //processType[0] = "UNASSIGNED";
            //string[] ObjType = new string[1];
            //ObjType[0] = "ADDENDUM";
            IList<MyQ> addendumGeneralQList = new List<MyQ>();
            //addendumGeneralQList = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, processType, ClientSession.UserName, false, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
            //var addendumGeneralQ = from p in addendumGeneralQList where p.Current_Owner == "UNKNOWN" select p;
            //return JsonConvert.SerializeObject(addendumGeneralQList.ToList<MyQ>());
            return JsonConvert.SerializeObject(addendumGeneralQList);
        }
        [WebMethod(EnableSession = true)]
        public static string MoveToMyEncounters(string[] data,string sViewAllFacilities)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<MyQ> MyQ = new List<MyQ>(); ;
            bool bValue = false;
            for (int i = 0; i < data.Length; i++)
            {
                string[] dataMove = data[i].Split('~');
                if (dataMove[0].ToString() == "Checked")
                    bValue = true;
                string[] ProcessType = new string[1];
                ProcessType[0] = "UNASSIGNED";

                string[] ObjType = new string[4];
                ObjType[0] = "ENCOUNTER";
                ObjType[1] = "DOCUMENTATION";
                //   ObjType[2] = "PHONE ENCOUNTER";
                ObjType[2] = "DOCUMENT REVIEW";
                WFObjectManager wfMngr = new WFObjectManager();
                EncounterManager EncMngr = new EncounterManager();
                string currentProcess = dataMove[1].ToString();
                string ExamRoom = string.Empty;
                string localTime = dataMove[3].ToString();

                if (currentProcess == "MA_PROCESS" || currentProcess == "TECHNICIAN_PROCESS")//CMG Ancilliary
                {
                    ExamRoom = dataMove[5].ToString();
                    string encounterID = dataMove[2];
                    string objType = dataMove[4];
                    if (sViewAllFacilities == "Checked")
                    {
                        MyQ = EncMngr.UpdateEncounterMoveTo(Convert.ToUInt64(dataMove[2]), ClientSession.UserName, ClientSession.UserName, Convert.ToDateTime(localTime),
                                                        ClientSession.CurrentObjectType, ExamRoom, "ViewAllFacilities~" + ClientSession.FacilityName, objType, ProcessType, bValue, iDefaultDays, string.Empty);
                    }
                    else
                    {
                        MyQ = EncMngr.UpdateEncounterMoveTo(Convert.ToUInt64(dataMove[2]), ClientSession.UserName, ClientSession.UserName, Convert.ToDateTime(localTime),
                                                            ClientSession.CurrentObjectType, ExamRoom, ClientSession.FacilityName, objType, ProcessType, bValue, iDefaultDays, string.Empty);
                    }
                    if (dataMove[6] != "")
                        ClientSession.HumanId = Convert.ToUInt64(dataMove[6]);
                    if (dataMove[2] != "")
                        ClientSession.EncounterId = Convert.ToUInt64(dataMove[2]);
                    ClientSession.UserCurrentProcess = dataMove[1].ToString();
                    frmRCopiaToolbar.LoadNotification("All");//BugID:52865 
                }
                else
                {
                    string wfObjectID = dataMove[2];
                    string objType = dataMove[4];
                    if (sViewAllFacilities == "Checked")
                    {
                        MyQ = EncMngr.UpdateEncounterMoveTo(Convert.ToUInt64(dataMove[2]), ClientSession.UserName, ClientSession.UserName, Convert.ToDateTime(localTime),
                            ClientSession.CurrentObjectType, ExamRoom, "ViewAllFacilities~" + ClientSession.FacilityName, objType, ProcessType, bValue, iDefaultDays, string.Empty);
                    }
                    else
                    {
                        MyQ = EncMngr.UpdateEncounterMoveTo(Convert.ToUInt64(dataMove[2]), ClientSession.UserName, ClientSession.UserName, Convert.ToDateTime(localTime),
                            ClientSession.CurrentObjectType, ExamRoom, ClientSession.FacilityName, objType, ProcessType, bValue, iDefaultDays, string.Empty);
                    }
                }

                if (MyQ != null && MyQ.Count > 0)
                {
                    //MyQ = MyQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "INTERNAL ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();
                    // MyQ = MyQ.Where(a => a.Current_Owner == "UNKNOWN" && a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT" && a.Facility_Name == ClientSession.FacilityName).ToList<MyQ>();

                    MyQ = MyQ.Where(a => a.EHR_Obj_Type != "DIAGNOSTIC ORDER" && a.EHR_Obj_Type != "DME ORDER" && a.EHR_Obj_Type != "IMAGE ORDER" && a.EHR_Obj_Type != "SCAN" && a.EHR_Obj_Type != "IMMUNIZATION ORDER" && a.EHR_Obj_Type != "REFERRAL ORDER" && a.Current_Process != "DICTATION_EXCEPTION" && a.Current_Process != "DICTATION_WAIT").ToList<MyQ>();

                    MyQ = MyQ.OrderByDescending(a => a.Appt_Date_Time).ToList<MyQ>();
                }
            }




            return JsonConvert.SerializeObject(MyQ.ToList<MyQ>());


        }
        [WebMethod(EnableSession = true)]
        public static string ProcessGenEncounter(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessGenEncounter : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            WFObjectManager wfMngr = new WFObjectManager();
            EncounterManager EncMngr = new EncounterManager();
            //ClientSession.TemplateId = 0;
            ClientSession.HumanId = Convert.ToUInt64(data[0]);
            ClientSession.EncounterId = Convert.ToUInt64(data[1]);
            ClientSession.PhysicianId = Convert.ToUInt64(data[2]);
            ClientSession.CurrentObjectType = data[4].ToString();
            ClientSession.UserCurrentProcess = data[3].ToString();
            ClientSession.Selectedencounterid = Convert.ToUInt64(data[1]);
            DateTime dt = new DateTime();
            string ExamRoom = data[7].ToString();
            string localTime = data[6];
            DateTime apptDt = Convert.ToDateTime(data[9].ToString());
            string sLocal_Time = UtilityManager.ConvertToLocal(Convert.ToDateTime(localTime)).ToString("yyyy-MM-dd hh:mm:ss tt");

            //jira #cap190 - Workflow issue - check for current owner
            WFObjectManager objWfMngr = new WFObjectManager();
            WFObject objDocWfObject = new WFObject();
            objDocWfObject = objWfMngr.GetByObjectSystemId(ClientSession.EncounterId, "DOCUMENTATION");
            if (objDocWfObject != null && objDocWfObject.Current_Owner.ToUpper() != "UNKNOWN")
            {
                return "UNKNOWN";
            }


            if (ClientSession.UserCurrentProcess == "SCRIBE_PROCESS" || ClientSession.UserCurrentProcess == "AKIDO_SCRIBE_PROCESS" || ClientSession.UserCurrentProcess == "MA_PROCESS" || ClientSession.UserCurrentProcess == "TECHNICIAN_PROCESS" || ClientSession.UserCurrentProcess == "AKIDO_SCRIBE_QC_PROCESS" || ClientSession.UserCurrentProcess == "TRANSCRIPT_PROCESS" || ClientSession.UserCurrentProcess == "TRANSCRIPT_QC_PROCESS")//to update DOS when Technician/MA processes the encounter for the first time -- CMG Ancilliary
            {

                if (data[5] == "0001-01-01T00:00:00")
                {
                    //if (data[6] != string.Empty && Convert.ToDateTime(data[6].ToString().Split(' ')[0]) == Convert.ToDateTime(apptDt.ToString().Split(' ')[0]))//For bug id 42870
                    if (sLocal_Time != string.Empty && Convert.ToDateTime(sLocal_Time.Split(' ')[0]) == Convert.ToDateTime(apptDt.ToString().Split(' ')[0]))
                    {
                        dt = Convert.ToDateTime(data[6].ToString());
                    }
                    else
                    {
                        // dt = apptDt; //For bug id 42870
                        dt = UtilityManager.ConvertToUniversal(apptDt);
                        sLocal_Time = apptDt.ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                }
                else
                {
                    dt = Convert.ToDateTime(data[5].ToString());
                }
                //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessGenEncounter UpdateEncounterforMyQueue DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");


                //string FileName = "Human" + "_" + data[0] + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                XmlDocument xmlDoc = new XmlDocument();
                string sXMLContent = String.Empty;

            //XmlTextReader XmlText = null;

            //if (File.Exists(strXmlFilePath) == true)
            //{
            ln:
                try
                {


                    //XmlDocument itemDoc = new XmlDocument();
                    //XmlText = new XmlTextReader(strXmlFilePath);
                    //itemDoc.Load(XmlText);
                    //XmlText.Close();

                    HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                    IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(Convert.ToUInt64(data[0]));
                    if (ilstHumanBlob.Count > 0)
                    {
                        sXMLContent = System.Text.Encoding.UTF8.GetString(ilstHumanBlob[0].Human_XML);
                        if (sXMLContent.Substring(0, 1) != "<")
                            sXMLContent = sXMLContent.Substring(1, sXMLContent.Length - 1);
                        xmlDoc.LoadXml(sXMLContent);
                    }
                    else
                    {
                        throw new Exception("Human XML is not found");
                    }

                }
                catch (Exception ex)
                {
                    // XmlText.Close();
                    UtilityManager.GenerateXML(data[0], "Human");
                    goto ln;
                }
                //}


                if (ClientSession.UserCurrentProcess == "MA_PROCESS")
                    EncMngr.UpdateEncounterforMyQueue(Convert.ToUInt64(data[1]), ClientSession.UserName, ClientSession.UserName, Convert.ToDateTime(localTime), dt, ClientSession.CurrentObjectType, ExamRoom, string.Empty, sLocal_Time);
                else
                    EncMngr.UpdateEncounterforMyQueue(Convert.ToUInt64(data[1]), "", ClientSession.UserName, Convert.ToDateTime(localTime), dt, ClientSession.CurrentObjectType, ExamRoom, string.Empty, sLocal_Time);


                // UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessGenEncounter UpdateEncounterforMyQueue DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            }
            else
            {
                WFObjectManager objWFObjectManager = new WFObjectManager();
                //UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessGenEncounter UpdateOwner DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
                objWFObjectManager.UpdateOwner(ClientSession.EncounterId, ClientSession.CurrentObjectType, ClientSession.UserName, string.Empty);
                // UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessGenEncounter UpdateOwner DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            }

            // UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue ProcessGenEncounter : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            //return true;
            //return dt.ToString("dd-MM-yyyy hh:mm:ss tt");
            return dt.ToString("MM/dd/yyyy hh:mm:ss tt");
        }

        [WebMethod(EnableSession = true)]
        public static string CheckEncounterForPFSH(string HumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            ulong uHumanId = 0;
            if (HumanID != string.Empty)
                uHumanId = Convert.ToUInt32(HumanID);

            EncounterManager objEncounterManager = new EncounterManager();
            IList<Encounter> lstEnc = new List<Encounter>();
            lstEnc = objEncounterManager.GetEncounterUsingHumanIDOrderByEncID(uHumanId);
            if (lstEnc.Count == 0)
                return "NO_NEW_APPOINTMENTS";
            else
            {
                WFObjectManager objWFObjectManager = new WFObjectManager();
                IList<WFObject> lstWFObject = new List<WFObject>();
                lstWFObject = objWFObjectManager.GetWFObjectsByObjSystemIdForPFSH(lstEnc[0].Id);
                List<string> lstProcess = new List<string>(new string[] { "SCHEDULED", "MA_PROCESS", "PROVIDER_PROCESS", "PHYSICIAN_CORRECTION" });
                if (lstWFObject.Count > 0)
                {
                    IList<WFObject> resultSet = lstWFObject.Where(u => lstProcess.Contains(u.Current_Process)).ToList<WFObject>();
                    if (resultSet.Count > 0)
                    {
                        ClientSession.EncounterId = lstEnc[0].Id;
                        return "OpenPFSHForEnc=" + lstEnc[0].Id.ToString();
                    }
                    else
                        return "NO_NEW_APPOINTMENTS";
                }
                else
                    return "NO_NEW_APPOINTMENTS";
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string PerformMovetoNextProcess(string HumanIDlst, string EncIDlst)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            ulong Encounter_Id = 0;
            ulong Human_Id = 0;
            Encounter objEnc = new Encounter();
            ulong SelectPhy_Id = 0;
            int CloseType = 2;
            string UserName = ClientSession.UserName;
            string UserRole = ClientSession.UserRole.ToUpper();
            string FacilityName = ClientSession.FacilityName;
            string ButtonName = "MOVE TO NEXT PROCESS";
            string MAC_Address = string.Empty;
            DateTime dtCurrentDateTime = DateTime.UtcNow;
            bool bIsReview = false;
            IList<string> Encounter_Idlst = new List<string>();
            IList<string> Human_Idlst = new List<string>();
            if (HumanIDlst != string.Empty && EncIDlst != string.Empty)
            {
                Human_Idlst = HumanIDlst.Split(',');
                Encounter_Idlst = EncIDlst.Split(',');
            }
            for (int i = 0; i < Human_Idlst.Count; i++)
            {
                Human_Id = Convert.ToUInt64(Human_Idlst[i]);
                Encounter_Id = Convert.ToUInt64(Encounter_Idlst[i]);
                EncounterManager EncManager = new EncounterManager();
                objEnc = EncManager.GetById(Encounter_Id);
                objEnc.Encounter_Provider_Review_ID = Convert.ToInt32(ClientSession.CurrentPhysicianId);//BugID:52856
                objEnc.Encounter_Provider_Review_Signed_Date = UtilityManager.ConvertToUniversal(DateTime.Now);
                objEnc.Modified_By = ClientSession.UserName;
                objEnc.Modified_Date_and_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
                EncManager.MoveToNextProcessFromPrintDocuments(objEnc, Encounter_Id, Human_Id, SelectPhy_Id, UserName,
                    ButtonName, FacilityName, MAC_Address, dtCurrentDateTime, bIsReview, UserRole, CloseType);
            }
            return string.Empty;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadGeneralTask(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadTask : Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            bool bValue = false;
            if (sShowall == "Checked")
                bValue = true;
            string[] ProcessType = new string[2];
            ProcessType[0] = "UNASSIGNED";

            string[] ObjType = new string[1];
            ObjType[0] = "TASK";
            IList<MyQ> TaskQ = new List<MyQ>();
            WFObjectManager wfMngr = new WFObjectManager();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadTask GetListObjects DB call: Start", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            TaskQ = wfMngr.GetListObjects(ClientSession.FacilityName, ObjType, ProcessType, ClientSession.UserName, bValue, iDefaultDays, string.Empty);// ClientSession.DefaultNoofDays);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadTask GetListObjects DB call: End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");

            //TaskQ = TaskQ.Where(a => a.Current_Owner == "UNKNOWN").ToList<MyQ>();
            //OrdersQ = OrdersQ.OrderByDescending(a => a.Created_Date_And_Time).ToList<MyQ>();
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "MyQueue LoadTask : End", DateTime.Now, sGroup_ID_Log, "frmMyQueueNew");
            return JsonConvert.SerializeObject(TaskQ.ToList<MyQ>());
        }
        [WebMethod(EnableSession = true)]
        public static string AllTabCount(string sTabName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }


            IList<MyQueueCountDTO> GenQCount = new List<MyQueueCountDTO>();
            IList<MyQueueCountDTO> MyQCount = new List<MyQueueCountDTO>();

            IList<MyQueueCountDTO> QCount = new List<MyQueueCountDTO>();
            WFObjectManager wfMngr = new WFObjectManager();

            if (sTabName == "MyQueue")
            {
                string[] ProcessTypeMyQ = new string[2];
                ProcessTypeMyQ[0] = "ASSIGNED";
                MyQCount = wfMngr.AllTabCount("ALL", ProcessTypeMyQ, ClientSession.UserName, iDefaultDays);
            }
            else if (sTabName == "GenQueue")
            {

                string[] ProcessType = new string[2];
                ProcessType[0] = "UNASSIGNED";
                GenQCount = wfMngr.AllTabCount(ClientSession.FacilityName, ProcessType, ClientSession.UserName, iDefaultDays);
            }

            //Adding GenQ Count
            if (GenQCount != null && GenQCount.Count > 0)
            {
                QCount = GenQCount;
            }

            //Adding MyQ Count
            if (MyQCount != null && MyQCount.Count > 0)
            {
                //QCount[0].My_Amendmnt_Count = MyQCount[0].My_Amendmnt_Count;
                //QCount[0].My_DiagRslt_Order_Count = MyQCount[0].My_DiagRslt_Order_Count;
                //QCount[0].My_Diag_Order_Count = MyQCount[0].My_Diag_Order_Count;
                //QCount[0].My_Dict_Count = MyQCount[0].My_Dict_Count;
                //QCount[0].My_Immun_Order_Count = MyQCount[0].My_Immun_Order_Count;
                //QCount[0].My_inter_Order_Count = MyQCount[0].My_inter_Order_Count;
                //QCount[0].My_Order_Count = MyQCount[0].My_Order_Count;
                //QCount[0].My_Presc_Count = MyQCount[0].My_Presc_Count;
                //QCount[0].My_Refer_Order_Count = MyQCount[0].My_Refer_Order_Count;
                //QCount[0].My_Scan_Count = MyQCount[0].My_Scan_Count;
                //QCount[0].My_Task_Count = MyQCount[0].My_Task_Count;
                QCount = MyQCount;
            }

            var result = new { count = QCount };
            return JsonConvert.SerializeObject(result);

        }

    }
}