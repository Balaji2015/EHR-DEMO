using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Threading;



namespace Acurus.Capella.UI
{
    public partial class frmMyQueueArchive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "MyQ - Archive" + " - " + ClientSession.UserName;
            if (!Page.IsPostBack)
            {
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        public static class DBConnector
        {
            static MySqlDataAdapter MyDataAdap = null;
            private static string ReadConnection()
            {
                string ConnectionData;
                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                return ConnectionData;
            }
            public static DataSet ReadData(string Query)
            {
                DataSet dsReturn = new DataSet();
                MyDataAdap = new MySqlDataAdapter(Query, ReadConnection());
                MyDataAdap.SelectCommand.CommandTimeout = 300;
                MyDataAdap.Fill(dsReturn);
                return dsReturn;
            }

            public static int WriteData(string Query)
            {
                int iReturn = 0;
                using (MySqlConnection con = new MySqlConnection(ReadConnection()))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query))
                    {
                        cmd.Connection = con;
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            iReturn = 1;
                        }
                        catch
                        {
                            iReturn = 2;
                        }
                    }
                }
                return iReturn;
            }
        }



        protected void SavedSuccessfully(object sender, EventArgs e)
        {
           // return "DisplayErrorMessage('380038');";
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('380018'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), " ", "DisplayErrorMessage('380018'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "sendsuccessfully" , "DisplayErrorMessage('380018'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('380018');", true);
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadArchive(string sShowall)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            try
            {
                string username = ClientSession.UserName;
//                string query = @"SELECT  cast(e.Appointment_Date as char(100)) as d,h.human_id,h.last_name,h.first_name,h.mi,
//    cast(h.Birth_Date as char(100)) as b,c.hpi_value,w.Current_Process,e.encounter_id
//    FROM wf_object_arc w left join encounter_arc e on (w.obj_system_id=e.encounter_id)
//    left join human h on (e.Human_ID=h.Human_ID) 
//    left join chief_complaint_arc c on (e.encounter_id=c.encounter_id and c.HPI_Element='Chief Complaints')
//    left join list_archive l on (l.encounter_id=e.encounter_id)
//    where w.current_process='PROVIDER_PROCESS' and w.obj_type='DOCUMENTATION' and w.current_owner='" + username + "' and e.Encounter_Provider_Signed_Date='0001-01-01' and l.encounter_id is null order by e.Appointment_Date desc";


                string query = @"SELECT  cast(e.Appointment_Date as char(100)) as d,h.human_id,h.last_name,h.first_name,h.mi,
    cast(h.Birth_Date as char(100)) as b,c.hpi_value,w.Current_Process,e.encounter_id
    FROM wf_object_arc w
    left join encounter_arc e on (w.obj_system_id=e.encounter_id)
    left join human h on (e.Human_ID=h.Human_ID) 
    left join chief_complaint_arc c on (e.encounter_id=c.encounter_id and c.HPI_Element='Chief Complaints')
    left join list_archive l on (l.encounter_id=e.encounter_id)

    where (e.Appointment_Date)>='2019-01-01' and w.current_process='PROVIDER_PROCESS' and w.obj_type='DOCUMENTATION' and
    w.current_owner='JTREMAZI' and
    e.Encounter_Provider_Signed_Date='0001-01-01' and l.encounter_id is null order by e.Appointment_Date desc";
                DataSet dsReturn = DBConnector.ReadData(query);
                DataTable dt = dsReturn.Tables[0];
                return JsonConvert.SerializeObject(dt);
            }
            catch(Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }


       
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool ProcessSignOnEncounter(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }

            string encounter =Convert.ToUInt64(data[0]).ToString();
            string localTime = data[1];  
            UtilityManager.ConvertToLocal(Convert.ToDateTime(localTime)).ToString("yyyy-MM-dd hh:mm:ss tt");

            string updateQuery = "update encounter_arc set Encounter_Provider_Signed_Date ='" +  TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "' where encounter_id='" + encounter + "'";

            DataSet dsReturn = DBConnector.ReadData(updateQuery);
            DataTable dt = dsReturn.Tables[0];
            return true;
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool SubmitEncounters(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return false;
            }

            string encounter = string.Empty;
            string localTime = string.Empty;
            string datetime = string.Empty;
            int iReturn = 0;
            for (int i = 0; i < data.Length; i++)
            {
                string[] dataMove = data[i].Split('~');
                encounter = Convert.ToUInt64(dataMove[0]).ToString();
                //localTime = dataMove[1];
                datetime = UtilityManager.ConvertToUniversal(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");

                if (dataMove[2] == "Sign")
                {
                    string updateQuery = "update encounter_arc set Encounter_Provider_Signed_Date ='" + datetime + "' where encounter_id='" + encounter + "'";
                    iReturn=  DBConnector.WriteData(updateQuery);
                    string FileName = "Encounter" + "_" + encounter + ".xml";
                    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                    if (File.Exists(strXmlFilePath) == true)
                    {
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                        itemDoc.Load(XmlText);
                        XmlNodeList xmlEncounter = itemDoc.GetElementsByTagName("EncounterList");
                        if (xmlEncounter != null && xmlEncounter.Count > 0)
                            xmlEncounter[0].ChildNodes[0].Attributes["Encounter_Provider_Signed_Date"].Value = datetime;

                        XmlText.Close();
                        //itemDoc.Save(strXmlFilePath);
                        int trycount = 0;
                    trytosaveagain:
                        try
                        {
                            itemDoc.Save(strXmlFilePath);
                        }
                        catch (Exception xmlexcep)
                        {
                            trycount++;
                            if (trycount <= 3)
                            {
                                int TimeMilliseconds = 0;
                                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                                Thread.Sleep(TimeMilliseconds);
                                string sMsg = string.Empty;
                                string sExStackTrace = string.Empty;

                                string version = "";
                                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                                string[] server = version.Split('|');
                                string serverno = "";
                                if (server.Length > 1)
                                    serverno = server[1].Trim();

                                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                                    sMsg = xmlexcep.InnerException.Message;
                                else
                                    sMsg = xmlexcep.Message;

                                if (xmlexcep != null && xmlexcep.StackTrace != null)
                                    sExStackTrace = xmlexcep.StackTrace;

                                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                                string ConnectionData;
                                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                                {
                                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                                    {
                                        cmd.Connection = con;
                                        try
                                        {
                                            con.Open();
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                goto trytosaveagain;
                            }
                        }

                    }
                }
                else if (dataMove[2] == "Work")
                {
                    string insertQuery = "insert into  list_archive values('" + encounter + "', '" + datetime + "')";
                    iReturn=  DBConnector.WriteData(insertQuery);
                }
                if (iReturn==2)
                {
                    //Cannot Save in DB
                    break;
                }
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SavedSuccessfully", "DisplayErrorMessage('160001');", true);
            }

            return true;

        }


        
    }
}