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
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;

using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using System.IO;
using System.Xml;
using System.Web.Services;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Acurus.Capella.UI
{
    public partial class frmPatientChart : System.Web.UI.Page
    {
        EncounterManager objEncounterManager = new EncounterManager();
        StaticLookupManager objStaticLookupManager = new StaticLookupManager();
        UserLookupManager objUserLookupManager = new UserLookupManager();
        Boolean bFormLoad = false;
        string sToolText = string.Empty;
        bool Is_CMG = false;
        string sGroup_ID_Log = string.Empty;
        public void LoadSummaryFromOtherForms(bool bLoad)
        {

            FillPatientChart objPatChart = new FillPatientChart();


            if (Request["HumanID"] != null && Request["HumanID"] != "undefined")
            {
                //Bug id 70531 - To avoid crash error if the input HumanID is not valid
                try
                {
                    ClientSession.HumanId = Convert.ToUInt64(Request["HumanID"].ToString().Replace("?", ""));
                }
                catch
                {

                }
            }
            if (Request["EncounterID"] != null)
            {
                ClientSession.Selectedencounterid = Convert.ToUInt64(Request["EncounterID"]);
                ClientSession.EncounterId = Convert.ToUInt64(Request["EncounterID"]);
            }
            if (Request["Source"] != null && Request["Source"] == "WindowItem")
            {
                ArrayList lstWindow = ClientSession.WindowList;
                string sID = string.Empty;
                string shumanID = string.Empty;
                string sObjType = string.Empty;
                foreach (object window in lstWindow)
                {
                    if (window.ToString().Contains(ClientSession.HumanId.ToString()))
                    {
                        sID = window.ToString().Split('#')[1].Split('$')[0];
                        sObjType = window.ToString().Split('#')[1].Split('$')[1];
                        //shumanID=sValue[3].Split('#')[0];
                        string[] sText = window.ToString().Split('#')[0].Split('~');
                        shumanID = sText[sText.Length - 1];
                    }
                }
                if (sID != string.Empty)
                {
                    ClientSession.Selectedencounterid = Convert.ToUInt64(sID);
                    ClientSession.EncounterId = Convert.ToUInt64(sID);
                }
                if (shumanID != string.Empty)
                {
                    ClientSession.HumanId = Convert.ToUInt64(shumanID);
                }

                ClientSession.CurrentObjectType = sObjType;
                ClientSession.SetSelectedTab = string.Empty;
            }
            sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - PageLoad : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");

            if (HttpContext.Current.Session["PatChartRedirectVlaues"] != null)//Added for CarePointe
            {
                string PatChartValues = HttpContext.Current.Session["PatChartRedirectVlaues"].ToString();
                ClientSession.HumanId = Convert.ToUInt64(PatChartValues.Split('&')[0]);
                ClientSession.EncounterId = Convert.ToUInt64(PatChartValues.Split('&')[1]);
                ClientSession.Selectedencounterid = Convert.ToUInt64(PatChartValues.Split('&')[1]);
                hdnLocalTime.Value = PatChartValues.Split('&')[2];
                WFObjectManager wfm = new WFObjectManager();
                string Obj_Type = "";
                string Curr_Process = "";
                bool Owner_Enc_Mismatch = true;
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call Backend API GetWfObjDetails: Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                if (ClientSession.UserName != null && ClientSession.EncounterId != null)
                    wfm.GetWfObjDetails(ClientSession.UserName, ClientSession.EncounterId, out Obj_Type, out Curr_Process, out Owner_Enc_Mismatch);
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call Backend API GetWfObjDetails: End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");

                hdnOwnerEncMismatch.Value = Owner_Enc_Mismatch.ToString().ToUpper();
                hdnOwnerEncMismatchEncID.Value = ClientSession.EncounterId.ToString();
                if (!Owner_Enc_Mismatch)
                {
                    ClientSession.UserCurrentProcess = Curr_Process;
                    ClientSession.CurrentObjectType = Obj_Type;
                }
            }
            //
            //
            //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //if (File.Exists(strXmlFilePath) == false && ClientSession.EncounterId > 0)
            //{
            EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
            Encounter_Blob objEncounterblob = null;
            IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ClientSession.EncounterId);
            if (ilstEncounterBlob.Count == 0 && ClientSession.EncounterId > 0)
            {
                //objEncounterblob = ilstEncounterBlob[0];

                throw new Exception("Encounter XML is not found for Encounter ID " + ClientSession.EncounterId + ". Please contact support.");

                string sDirectoryPath = string.Empty;
                if (Directory.Exists(HttpContext.Current.Server.MapPath("Template_XML")))
                    sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                string sXmlPath = string.Empty;
                if (File.Exists(Path.Combine(sDirectoryPath, "Base_XML.xml")))
                    sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
            loop:
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                try
                {
                    itemDoc.Load(XmlText);
                }
                //catch
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                //    return;
                //}
                catch (Exception ex)
                {
                    // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);

                        XmlText.Close();
                        ///UtilityManager.GenerateXML(ClientSession.EncounterId.ToString(), "Encounter");
                        //goto loop;
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId.ToString() + "','Encounter','patientchart');", true);


                        //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                        return;
                    }

                }
                XmlText.Close();

                XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                if (xmlAgenode != null && xmlAgenode.Count > 0)
                    xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

                //itemDoc.Save(strXmlFilePath);
            //    int trycount = 0;
            //trytosaveagain:
                try
                {
                    //itemDoc.Save(strXmlFilePath);

                    IList<Encounter_Blob> ilstUpdateBlob = new List<Encounter_Blob>();
                    byte[] bytes = null;
                    try
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                    }
                    catch (Exception ex)
                    {

                    }
                    objEncounterblob.Encounter_XML = bytes;
                    ilstUpdateBlob.Add(objEncounterblob);
                    EncounterBlobMngr.SaveEncounterBlobWithTransaction(ilstUpdateBlob, string.Empty);
                }
                catch (Exception xmlexcep)
                {
                    throw new Exception(xmlexcep.Message.ToString());
                    //trycount++;
                    //if (trycount <= 3)
                    //{
                    //    int TimeMilliseconds = 0;
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //    Thread.Sleep(TimeMilliseconds);
                    //    string sMsg = string.Empty;
                    //    string sExStackTrace = string.Empty;

                    //    string version = "";
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //    string[] server = version.Split('|');
                    //    string serverno = "";
                    //    if (server.Length > 1)
                    //        serverno = server[1].Trim();

                    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //        sMsg = xmlexcep.InnerException.Message;
                    //    else
                    //        sMsg = xmlexcep.Message;

                    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //        sExStackTrace = xmlexcep.StackTrace;

                    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //    string ConnectionData;
                    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //    {
                    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //        {
                    //            cmd.Connection = con;
                    //            try
                    //            {
                    //                con.Open();
                    //                cmd.ExecuteNonQuery();
                    //                con.Close();
                    //            }
                    //            catch
                    //            {
                    //            }
                    //        }
                    //    }
                    //    goto trytosaveagain;
                    //}
                }
            }

            //string FileName1 = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath1 = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName1);

            //if (File.Exists(strXmlFilePath1) == false && ClientSession.HumanId > 0)
            //{


            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            Human_Blob objHumanblob = null;
            IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ClientSession.HumanId);
            if (ilstHumanBlob.Count == 0 && ClientSession.HumanId > 0)
            {
                //objHumanblob = ilstHumanBlob[0];

                throw new Exception("Human XML is not found for Human ID " + ClientSession.HumanId + ". Please contact support.");

                string sDirectoryPath = string.Empty;
                if (Directory.Exists(HttpContext.Current.Server.MapPath("Template_XML")))
                    sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                string sXmlPath = string.Empty;
                if (File.Exists(Path.Combine(sDirectoryPath, "Base_XML.xml")))
                    sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");

                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                try
                {
                    itemDoc.Load(XmlText);
                }
                catch
                {
                    XmlText.Close();

                    //ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.HumanId.ToString() + "','Human','patientchart');", true);


                    //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    return;
                    
                }
                XmlText.Close();
                //itemDoc.Save(strXmlFilePath1);
            //    int trycount = 0;
            //trytosaveagain:
                try
                {
                    //itemDoc.Save(strXmlFilePath1);

                    IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                    byte[] bytes = null;
                    try
                    {
                        bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                    }
                    catch (Exception ex)
                    {

                    }
                    objHumanblob.Human_XML = bytes;
                    ilstUpdateBlob.Add(objHumanblob);
                    HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
                }
                catch (Exception xmlexcep)
                {
                    throw new Exception(xmlexcep.Message.ToString());

                    //trycount++;
                    //if (trycount <= 3)
                    //{
                    //    int TimeMilliseconds = 0;
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //    Thread.Sleep(TimeMilliseconds);
                    //    string sMsg = string.Empty;
                    //    string sExStackTrace = string.Empty;

                    //    string version = "";
                    //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //    string[] server = version.Split('|');
                    //    string serverno = "";
                    //    if (server.Length > 1)
                    //        serverno = server[1].Trim();

                    //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //        sMsg = xmlexcep.InnerException.Message;
                    //    else
                    //        sMsg = xmlexcep.Message;

                    //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //        sExStackTrace = xmlexcep.StackTrace;

                    //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //    string ConnectionData;
                    //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //    {
                    //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //        {
                    //            cmd.Connection = con;
                    //            try
                    //            {
                    //                con.Open();
                    //                cmd.ExecuteNonQuery();
                    //                con.Close();
                    //            }
                    //            catch
                    //            {
                    //            }
                    //        }
                    //    }
                    //    goto trytosaveagain;
                    //}
                }
            }


            //
            //Commented to fill FillPatientChart object to be used in Addendum, the commented call returns Empty object.
            //if (hdnLocalTime.Value.Trim() == string.Empty)
            //    ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.Selectedencounterid, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, bLoad, Convert.ToUInt32(hdnAddendumID.Value));//0);
            //else
            //    ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.Selectedencounterid, UtilityManager.ConvertToLocal(DateTime.ParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null)), string.Empty, ClientSession.UserName, bLoad, Convert.ToUInt32(hdnAddendumID.Value));// 0);
            bool bIsArchive = false;
            if (ClientSession.CurrentObjectType.ToUpper() == "ADDENDUM")
                bIsArchive = true;
            DateTime tempDate = DateTime.MinValue;
        ln:
            try
            {
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call LoadPatientChart Manager API : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                if (hdnLocalTime.Value != null && hdnLocalTime.Value.Trim() == string.Empty || DateTime.TryParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null, System.Globalization.DateTimeStyles.None, out tempDate) == false)
                    ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, true, Convert.ToUInt32(hdnAddendumID.Value), ClientSession.CurrentObjectType, bIsArchive);//0);
                else
                {
                    ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.ParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null)), string.Empty, ClientSession.UserName, true, Convert.ToUInt32(hdnAddendumID.Value), ClientSession.CurrentObjectType, bIsArchive);// 0);
                }

                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call LoadPatientChart Manager API : End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            }
            //catch (Exception ex)
            //{

            //    if (ex.Message.ToLower().Contains("unexpected end of file") == true)
            //    {
            //        ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //        return;
            //    }
            //    else if (ex.Message.ToLower().Contains("is an unexpected token") == true)
            //    {
            //        ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //        return;
            //    }
            //    else if (ex.Message.ToLower().Contains("input string was not") == true)
            //    {
            //        ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
            //        return;
            //    }
            //}
            catch (Exception ex)
            {
                // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                {


                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.HumanId.ToString() + "','Human','patientchart');", true);


                    //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    return;
                    // goto ln;
                }

            }
            if (ClientSession.FillPatientChart.PatChartList != null && ClientSession.FillPatientChart.PatChartList.Count > 0 && ClientSession.FillPatientChart.PatChartList[0].PhotoPath.Trim().ToString() != string.Empty && ClientSession.FillPatientChart.PatChartList[0].PhotoPath.Trim().ToString() != "")
                LoadPatientPhoto();


            //if (ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Id > 0)
            //{
            //    ulong encounterid = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Id;
            //    if (encounterid != null && encounterid != 0)
            //    {
            //        if (File.Exists(strXmlFilePath) == true && encounterid != 0)
            //        {
            //            XmlDocument itemDoc = new XmlDocument();
            //            XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //            itemDoc.Load(XmlText);
            //            XmlText.Close();
            //            XmlNodeList xmlEncounterListnode = itemDoc.GetElementsByTagName("EncounterList");
            //            if (xmlEncounterListnode != null)
            //            {
            //                if (xmlEncounterListnode.Count > 0)
            //                {
            //                    if (xmlEncounterListnode[0].ChildNodes.Count > 0)
            //                    {
            //                        xmlEncounterListnode[0].ChildNodes[0].Attributes.GetNamedItem("Local_Time").Value = UtilityManager.ConvertToLocal(ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            //                        itemDoc.Save(strXmlFilePath);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //GenerateXml XMLObj = new GenerateXml();
            //if (ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord != null && ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Id > 0)
            //{
            //    ulong encounterid = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Id;
            //    IList<Encounter> ilstencounter = new List<Encounter>();
            //    ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Local_Time = UtilityManager.ConvertToLocal(ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            //    ilstencounter.Add(ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord);
            //    if (ilstencounter != null && ilstencounter.Count > 0)
            //    {
            //        List<object> lstObj = ilstencounter.Cast<object>().ToList();
            //        XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);
            //    }
            //}


            if (Request["from"] != null && Request["from"].ToString().IndexOf("viewresult") > -1)
            {
                tblPatientSummary.Visible = false;

            }
            if (Request["from"] != null && Request["from"].ToString().IndexOf("openpatientchart") > -1)
            {
                tblPatientSummary.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "invisibleResult();", true);
            }

            // if (!IsPostBack)
            {

                //Commented by Srividhya - since this screen is called from Charge Posting screen
                //ClientSession.FlushSession();
                //if (Request["HumanID"] != null)
                //{
                //    ClientSession.HumanId = Convert.ToUInt64(Request["HumanID"]);
                //}
                //ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.Selectedencounterid, UtilityManager.ConvertToLocal(DateTime.Now), string.Empty, ClientSession.UserName, bLoad, 0);
                if (ClientSession.HumanId != null)
                    hdnHumanNo.Value = ClientSession.HumanId.ToString();
                hdnIndexId.Value = "0";
                hdnOrderSubmitId.Value = "0";
                hdnResultId.Value = "0";
                //Added By priyangha 
                if (ClientSession.HumanId != null && ClientSession.HumanId != 0)
                {
                    hdnHumanNo.Value = ClientSession.HumanId.ToString();
                }
            }

            //int uselectedEncounterNode = 0;

            //IList<string> problemListCodesWithParentCodes = new List<string>();
            //problemListCodesWithParentCodes.Clear();
            //var distinct = from h in objPatChart.PblmListParentICD group h by new { h } into g select new { g.Key.h };

            //foreach (var code in distinct)
            //{

            //    var duplicate = (from dup in problemListCodesWithParentCodes
            //                     where dup.Contains(code.h)
            //                     select dup);

            //    if (duplicate.Count() == 0)
            //    {
            //        if (code.h != string.Empty)
            //        {
            //            problemListCodesWithParentCodes.Add(code.h);
            //        }
            //    }
            //    else
            //    {
            //        if (code.h.Split('!')[0] != duplicate.First().Split('!')[0])
            //        {
            //            problemListCodesWithParentCodes.Add(code.h);
            //        }
            //    }
            //}

            //ClientSession.ProblemListCodesWithParentCodes = problemListCodesWithParentCodes;
            //   LoadPatientSummaryBar();
        }

        //public void LoadPatientSummaryBar()
        //{

        //    FillPatientChart objPatChart = new FillPatientChart();
        //    objPatChart = ClientSession.FillPatientChart;
        //    //objEncounterManager.LoadPatientSummaryBar
        //    IList<string> sSummaryLst = new List<string>();

        //    //MyToolTip = new ToolTip();
        //    if (ClientSession.UserName == string.Empty)
        //        return;


        //    sSummaryLst.Add("Allergies");
        //    sSummaryLst.Add("Chief complaints");
        //    sSummaryLst.Add("Problem list");
        //    sSummaryLst.Add("Vitals");
        //    sSummaryLst.Add("Medications");
        //    lblVitals.InnerText = "Vitals:";
        //    lblProblemList.InnerText = "Problem list:";
        //    lblMedication.InnerText = "Medications:";
        //    lblCheifComplaints.InnerText = "Chief complaints:";
        //    lblAllergies.InnerText = "Allergies:";

        //    string strText = string.Empty;
        //    //float size = (float)100 / (float)FieldLookupList.Count;


        //    //for (int i = 0; i < sSummaryLst.Count; i++)
        //    //{
        //    //   // sSummaryLst.Add(FieldLookupList[i].Value.ToUpper());
        //    //    switch (sSummaryLst[i].ToUpper())
        //    //    {
        //    //        case "VITALS":
        //    //           // Panel pnl = CreatePanelForPatientSummary("Vitals", i);
        //    //          //  pnl.ID = "pnlVitals";
        //    //           // vitalsTip.TargetControlID = "pnlVitals";
        //    //          //  vitalsTip.Text = string.Empty;
        //    //          //  FillVitals(objPatChart.PatientSummary.VitalsList, pnl);
        //    //            pnlVitals.Controls.Clear();
        //    //            FillVitals(objPatChart.PatientSummary.VitalsList, pnlVitals);
        //    //            break;
        //    //        case "PROBLEM LIST":
        //    //            //Panel pnlPrblst = CreatePanelForPatientSummary("ProblemList", i);
        //    //            //FillProblemList(objPatChart.PblmMedList, pnlPrblst);
        //    //            FillProblemList(objPatChart.PblmMedList, pnlProblemList);
        //    //            break;
        //    //        case "ALLERGIES":
        //    //            //strText = "An HTML element is an individual component of an HTML document or web page, once this has been parsed into the Document Object Model. HTML is composed of a tree of HTML elements and other nodes, such as text nodes. Each element can have HTML attributes specified. Elements can also have content, including other elements and text. HTML elements represent semantics, or meaning. For example, the title element represents the title of the document.In the HTML syntax, most elements are written with a start tag and an end tag, with the content in between. An HTML tag is composed of the name of the element, surrounded by angle brackets. An end tag also has a slash after the opening angle bracket, to distinguish it from the start tag. For example, a paragraph, which is represented by the p element, would be written as";
        //    //            strText = FillAllergies(objPatChart.PatientSummary.NonDrugAllergyList, objPatChart.PatientSummary.AllergyList);
        //    //            CreatePatientSummaryControls("Allergies", i, strText);
        //    //            break;
        //    //        case "STANDING ORDERS":
        //    //            strText = FillStandingOrders();
        //    //            CreatePatientSummaryControls("StandingOrders", i, strText);
        //    //            break;
        //    //        case "MEDICATIONS":
        //    //            //Panel pnlMed = CreatePanelForPatientSummary("Medications", i);
        //    //            //FillMedicationPane(objPatChart.PatientSummary.MedicationList, objPatChart.PatientSummary.EncounterIDList, objPatChart.PatientSummary.EncounterDateList, objPatChart.PatientSummary.MedHistoryList, pnlMed);
        //    //            pnlMedication.Controls.Clear();
        //    //            FillMedicationPane(objPatChart.PatientSummary.MedicationList, objPatChart.PatientSummary.EncounterIDList, objPatChart.PatientSummary.EncounterDateList, objPatChart.PatientSummary.MedHistoryList, pnlMedication);
        //    //            break;
        //    //        case "CHIEF COMPLAINTS":
        //    //            strText = FillChiefComplaints(objPatChart.PatientSummary.ChiefComplaintList);
        //    //            CreatePatientSummaryControls("ChiefComplaints", i, strText);
        //    //            break;

        //    //        default:
        //    //            strText = sSummaryLst[i] + " :" + Environment.NewLine + Environment.NewLine;
        //    //            string sReplace = sSummaryLst[i].Replace(" ", "");
        //    //            CreatePatientSummaryControls(sReplace, i, strText);
        //    //            break;
        //    //    }
        //    //}
        //    return;
        //    for (int i = 0; i < sSummaryLst.Count; i++)
        //    {
        //        // sSummaryLst.Add(FieldLookupList[i].Value.ToUpper());
        //        switch (sSummaryLst[i].ToUpper())
        //        {
        //            case "VITALS":
        //                // Panel pnl = CreatePanelForPatientSummary("Vitals", i);
        //                //  pnl.ID = "pnlVitals";
        //                // vitalsTip.TargetControlID = "pnlVitals";
        //                //  vitalsTip.Text = string.Empty;
        //                //  FillVitals(objPatChart.PatientSummary.VitalsList, pnl);
        //                //pnlVitals.Controls.Clear();
        //                FillVitals(objPatChart.PatientSummary.VitalsList);
        //                break;
        //            case "PROBLEM LIST":
        //                FillProblemList(objPatChart.PblmMedList);
        //                break;
        //            case "ALLERGIES":
        //                //strText = "An HTML element is an individual component of an HTML document or web page, once this has been parsed into the Document Object Model. HTML is composed of a tree of HTML elements and other nodes, such as text nodes. Each element can have HTML attributes specified. Elements can also have content, including other elements and text. HTML elements represent semantics, or meaning. For example, the title element represents the title of the document.In the HTML syntax, most elements are written with a start tag and an end tag, with the content in between. An HTML tag is composed of the name of the element, surrounded by angle brackets. An end tag also has a slash after the opening angle bracket, to distinguish it from the start tag. For example, a paragraph, which is represented by the p element, would be written as";
        //                // strText = FillAllergies(objPatChart.PatientSummary.NonDrugAllergyList, objPatChart.PatientSummary.AllergyList);
        //                CreatePatientSummaryControls("Allergies", i, strText);
        //                break;
        //            case "MEDICATIONS":
        //                //Panel pnlMed = CreatePanelForPatientSummary("Medications", i);
        //                //FillMedicationPane(objPatChart.PatientSummary.MedicationList, objPatChart.PatientSummary.EncounterIDList, objPatChart.PatientSummary.EncounterDateList, objPatChart.PatientSummary.MedHistoryList, pnlMed);
        //                // pnlMedication.Controls.Clear();
        //                FillMedicationPane(objPatChart.PatientSummary.MedicationList, objPatChart.PatientSummary.EncounterIDList, objPatChart.PatientSummary.EncounterDateList, objPatChart.PatientSummary.MedHistoryList);
        //                break;
        //            case "CHIEF COMPLAINTS":
        //                strText = FillChiefComplaints(objPatChart.PatientSummary.ChiefComplaintList);
        //                CreatePatientSummaryControls("ChiefComplaints", i, strText);
        //                break;

        //            default:
        //                strText = sSummaryLst[i] + " :" + Environment.NewLine + Environment.NewLine;
        //                string sReplace = sSummaryLst[i].Replace(" ", "");
        //                CreatePatientSummaryControls(sReplace, i, strText);
        //                break;
        //        }
        //    }

        //    //imgSummaryTip.Text = string.Empty;
        //    //imgOverAllSummary.ToolTip = sToolText.Replace("<br/>", "\n");

        //    //imgSummaryTip.ToolTip = sToolText.Replace("\n", "<br/>");
        //    //imgSummaryTip.Text = sToolText.Replace("\n", "<br/>");         
        //    sToolText = string.Empty;//for bug id 29125
        //    //MyToolTip.InitialDelay = 0;
        //    //MyToolTip.ShowAlways = true;
        //    //MyToolTip.UseAnimation = false;
        //    //MyToolTip.Active = true;
        //    //pictureBox2.MouseHover += new EventHandler(pictureBox2_MouseHover);
        //    //pictureBox2.MouseEnter += new EventHandler(pictureBox2_MouseEnter);

        //}
        //private string FillChiefComplaints(IList<ChiefComplaints> ChiefComplaintsList)
        //{
        //    string Text = string.Empty;

        //    if (ChiefComplaintsList != null)
        //    {
        //        var cc = (from h in ChiefComplaintsList select new { h.HPI_Value }).Distinct();

        //        foreach (var c in cc)
        //        {
        //            Text = Text + c.HPI_Value + Environment.NewLine;
        //        }
        //    }

        //    Text = Text.Insert(0, "Chief Complaints :" + "<br/>");
        //    return Text;
        //}
        //private void FillMedicationPane(IList<Rcopia_Medication> MedicationList, IList<ulong> EncounterIDList, IList<DateTime> EncounterDateList, IList<string> MedicationHistoryList)
        //{
        //    string sMedicationsText = string.Empty;
        //    string pnlMedication_toolTip = string.Empty;
        //    string Text = "Medication :" + Environment.NewLine;
        //    sMedicationsText += Text;
        //    //int LocationY = CreateLablesForPatientSummary(Text, pnlMed, 0, Color.Black.Name, "Medication");
        //    DateTime startdate = DateTime.MinValue;
        //    DateTime stopdate = DateTime.MinValue;
        //    if (MedicationList.Count != 0 && MedicationList != null)
        //    {
        //        if (EncounterIDList.Count > 1 && EncounterDateList.Count > 1)
        //        {
        //            for (int i = 0; i < EncounterIDList.Count; i++)
        //            {
        //                for (int j = 0; j < EncounterDateList.Count; j++)
        //                {
        //                    if (EncounterIDList[i].ToString() == ClientSession.EncounterId.ToString())
        //                    {
        //                        stopdate = EncounterDateList[0].Date;
        //                        if (i == 0 && j == 0)
        //                        {
        //                            startdate = EncounterDateList[j + 1].Date;
        //                        }

        //                    }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (EncounterIDList.Count != 0 && EncounterDateList.Count != 0)
        //            {
        //                if (EncounterIDList[0].ToString() == ClientSession.EncounterId.ToString())
        //                {
        //                    stopdate = EncounterDateList[0].Date;
        //                    startdate = DateTime.MinValue;
        //                }
        //            }
        //        }

        //    }
        //    string strMedicationlist = string.Empty;
        //    if (EncounterIDList.Count != 0 && EncounterIDList != null)
        //    {
        //        if (MedicationList.Count != 0 && MedicationList != null)
        //        {

        //            for (int i = 0; i < MedicationList.Count; i++)
        //            {
        //                string sColor = Color.Black.Name;
        //                strMedicationlist = string.Empty;
        //                if ((MedicationList[i].Start_Date.Date >= startdate.Date || MedicationList[i].Start_Date.Date <= startdate.Date) && (MedicationList[i].Stop_Date.Date <= stopdate.Date && MedicationList[i].Stop_Date.Date >= startdate.Date && MedicationList[i].Stop_Date.Date != DateTime.MinValue.Date))
        //                {


        //                    //RED
        //                    if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
        //                    {
        //                        // strMedicationlist = MedicationList[i].Brand_Name + "( " + MedicationList[i].Generic_Name + " ) " + MedicationList[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing;
        //                        strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";
        //                        sColor = Color.Red.Name;
        //                    }
        //                    else
        //                    {
        //                        strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";
        //                        sColor = Color.Red.Name;
        //                    }

        //                }



        //                else if ((MedicationList[i].Start_Date.Date >= startdate.Date || MedicationList[i].Start_Date.Date <= startdate.Date) && (MedicationList[i].Stop_Date.Date > stopdate.Date || MedicationList[i].Stop_Date.Date == DateTime.MinValue.Date))
        //                {
        //                    //BLACK
        //                    if (MedicationList[i].Brand_Name != MedicationList[i].Generic_Name)
        //                    {
        //                        strMedicationlist = MedicationList[i].Brand_Name + " " + MedicationList[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";
        //                        sColor = Color.Black.Name;
        //                    }
        //                    else
        //                    {
        //                        strMedicationlist = MedicationList[i].Generic_Name + " " + MedicationList[i].Strength + " " + MedicationList[i].Form + " : " + MedicationList[i].Dose + " " + MedicationList[i].Dose_Unit + " " + MedicationList[i].Route + " " + MedicationList[i].Dose_Timing + "\n";
        //                        sColor = Color.Black.Name;
        //                    }
        //                }

        //                else if (MedicationList[i].Start_Date.Date <= startdate.Date && (MedicationList[i].Stop_Date.Date > stopdate.Date || MedicationList[i].Stop_Date.Date < stopdate.Date))
        //                {
        //                    strMedicationlist = string.Empty;
        //                }

        //                sMedicationsText += strMedicationlist;
        //                if (strMedicationlist != null && strMedicationlist != string.Empty) ;
        //                //LocationY = LocationY + CreateLablesForPatientSummary(strMedicationlist, pnlMed, LocationY, sColor, "Medication");
        //            }

        //        }
        //    }
        //    sToolText += sMedicationsText + Environment.NewLine;
        //    pnlMedication_toolTip = sMedicationsText;
        //    sMedicationsText = sMedicationsText.Replace("\r\n", "<br/>");
        //    lblMedication.InnerHtml = sMedicationsText.Replace("\n", "<br/>");
        //    if (sToolText.Trim() != string.Empty)
        //        Session["Client_SummaryMedication"] = sToolText.Contains("Medication :") ? sToolText.Substring(sToolText.IndexOf("Medication :")) : string.Empty;
        //    //MyToolTip.InitialDelay = 0;
        //    //MyToolTip.ShowAlways = true;
        //    //MyToolTip.UseAnimation = false;
        //    //MyToolTip.Active = true;
        //    //pnlMed.MouseHover += new EventHandler(pnlMed_MouseHover);
        //    //pnlMed.MouseEnter += new EventHandler(pnl_MouseEnter);
        //    pnlMedication.Attributes.Add("title", pnlMedication_toolTip);

        //}

        //private void CreatePatientSummaryControls(string CtrlName, int Index, string Text)
        //{
        //    //Panel pnl = new Panel();
        //    ////pnl.CssClass = "FixedHeightAndScroll";
        //    //pnl.ID = "pnl" + CtrlName.Trim();
        //    //pnl.Height = Unit.Pixel(70);
        //    //// pnl.CssClass = "AbsoulteStyle";
        //    //pnl.ToolTip = Text.Replace("<br/>", "\n");
        //    string tooltip = string.Empty;
        //    string pnlAllergies_toolTip = string.Empty;
        //    string pnlCheifComplaints_toolTip = string.Empty;
        //    if (CtrlName == "Allergies")
        //    {
        //        lblAllergies.InnerHtml = Text + Environment.NewLine;
        //        if (sToolText.Contains(Text) == true)
        //        {
        //            sToolText = string.Empty;
        //        }
        //        sToolText += Text + Environment.NewLine; ;

        //        tooltip = lblAllergies.InnerText;
        //        pnlAllergies_toolTip = tooltip.Replace("<br/>", "\n");
        //        pnlAllergies.Attributes.Add("title", pnlAllergies_toolTip);
        //    }
        //    else if (CtrlName == "ChiefComplaints")
        //    {
        //        lblCheifComplaints.InnerHtml = Text + Environment.NewLine;
        //        if (sToolText.Contains(Text) == true)
        //        {
        //            sToolText = string.Empty;
        //        }
        //        sToolText += Text + Environment.NewLine; ;
        //        tooltip = lblCheifComplaints.InnerText;
        //        pnlCheifComplaints_toolTip = tooltip.Replace("<br/>", "\n");
        //        pnlCheifComplaints.Attributes.Add("title", pnlCheifComplaints_toolTip);
        //    }

        //}
        //private string FillAllergies(IList<NonDrugAllergy> NonDrugAllergyList, IList<Rcopia_Allergy> DrugAllergyList)
        //{
        //    string Text = string.Empty;
        //    if (DrugAllergyList != null && DrugAllergyList.Count > 0)
        //    {
        //        Text = Text + "DrugAllergy:" + "<br/>";
        //        if (DrugAllergyList.Count > 1)
        //        {
        //            DrugAllergyList = (from allergyList in DrugAllergyList where allergyList.Allergy_Name != "nkda" select allergyList).ToList<Rcopia_Allergy>();
        //        }
        //        for (int i = 0; i < DrugAllergyList.Count; i++)
        //        {
        //            Text = Text + DrugAllergyList[i].Allergy_Name;
        //            if (DrugAllergyList[i].Reaction != string.Empty)
        //                Text = Text + " - " + DrugAllergyList[i].Reaction;
        //            Text = Text + "<br/>";
        //        }

        //    }

        //    if (NonDrugAllergyList != null && NonDrugAllergyList.Count > 0)
        //    {
        //        Text = Text + "NonDrugAllergy:" + "<br/>";
        //        for (int i = 0; i < NonDrugAllergyList.Count; i++)
        //        {
        //            Text = Text + NonDrugAllergyList[i].Non_Drug_Allergy_History_Info;
        //            if (NonDrugAllergyList[i].Description != string.Empty)
        //                Text = Text + " - " + NonDrugAllergyList[i].Description;
        //            Text = Text + "<br/>";
        //        }
        //    }

        //    Text = Text.Insert(0, "Allergies :" + "<br/>");
        //    if (Text.Trim() != string.Empty && Text.Contains("NonDrugAllergy"))
        //        Session["SummaryAllergy"] = Text.Substring(0, Text.IndexOf("NonDrugAllergy"));
        //    return Text;
        //}
        //private void FillProblemList(IList<ProblemList> problemListItems)
        //{
        //    //Todo: Use PatientPaneObject to fill problem list

        //    string strProblemtxt = string.Empty;
        //    string Text = "Problem List :" + Environment.NewLine;
        //    strProblemtxt += Text;
        //    string pnlProblemList_toolTip = string.Empty;
        //    string strProblemList = string.Empty;

        //    //int LocationY = CreateLablesForPatientSummary(Text, pnlprblst, 0, Color.Black.Name, "ProblemList");

        //    //added by pravin-04-12-2012
        //    IList<ProblemList> Activelist = new List<ProblemList>();

        //    var act = problemListItems.Where(a => a.Status == "Active" && a.Date_Diagnosed != string.Empty).OrderByDescending(a => a.Date_Diagnosed).ThenBy(a => a.Created_Date_And_Time).ToList().Concat(problemListItems.Where(a => a.Status == "Active" && a.Date_Diagnosed == string.Empty).OrderByDescending(a => a.Created_Date_And_Time).ToList());
        //    Activelist = act.ToList<ProblemList>();

        //    IList<ProblemList> InActivelist = new List<ProblemList>();

        //    var inact = problemListItems.Where(b => b.Status.ToUpper() == "INACTIVE" && b.Date_Diagnosed != string.Empty).OrderByDescending(b => b.Date_Diagnosed).ThenBy(b => b.Created_Date_And_Time).ToList().Concat(problemListItems.Where(b => b.Status.ToUpper() == "INACTIVE" && b.Date_Diagnosed == string.Empty).OrderByDescending(b => b.Created_Date_And_Time).ToList());
        //    InActivelist = inact.ToList<ProblemList>();


        //    if (problemListItems != null && problemListItems.Count > 0)
        //    {
        //        if (Activelist.Count > 0 && Activelist != null)
        //        {
        //            for (int h = 0; h < Activelist.Count; h++)
        //            {
        //                strProblemList = string.Empty;
        //                string sColor = Color.Black.Name;
        //                string[] DateDiagnosed = null;
        //                string diagnosed = string.Empty;
        //                if (Activelist[h].Date_Diagnosed != string.Empty)
        //                {
        //                    DateDiagnosed = Activelist[h].Date_Diagnosed.Split('-');

        //                    if (DateDiagnosed.Length == 2 || DateDiagnosed.Length == 1)
        //                    {
        //                        if (DateDiagnosed.Length == 1)
        //                        {
        //                            diagnosed = " (" + Activelist[h].Date_Diagnosed + ")";
        //                        }
        //                        else
        //                        {
        //                            Activelist[h].Date_Diagnosed = DateDiagnosed[1] + "-" + DateDiagnosed[0] + "-01";

        //                            string smonth = Convert.ToDateTime(Activelist[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
        //                            string[] sarrMonth = smonth.Split('-');


        //                            diagnosed = " (" + sarrMonth[1] + "-" + DateDiagnosed[0] + ")";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Activelist[h].Date_Diagnosed = Convert.ToDateTime(Activelist[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
        //                        diagnosed = " (" + Activelist[h].Date_Diagnosed + ")";
        //                    }
        //                }
        //                if (Activelist[h].ICD_Code != string.Empty)
        //                {

        //                    if (Activelist[h].Status == "Active")
        //                    {
        //                        strProblemList += Activelist[h].ICD_Code + "-" + Activelist[h].Problem_Description + diagnosed + "\n";
        //                        sColor = Color.Black.Name;
        //                    }

        //                }
        //                else if (Activelist[h].ICD_Code == string.Empty && Activelist[h].Problem_Description != string.Empty)
        //                {
        //                    if (Activelist[h].Status == "Active")
        //                    {
        //                        strProblemList += Activelist[h].Problem_Description + diagnosed + "\n";
        //                        sColor = Color.Black.Name;

        //                    }
        //                }
        //                strProblemtxt += strProblemList;
        //                //LocationY = LocationY + CreateLablesForPatientSummary(strProblemList, pnlprblst, LocationY, sColor, "ProblemList");
        //            }
        //        }
        //    }

        //    if (problemListItems != null && problemListItems.Count > 0)
        //    {
        //        if (InActivelist.Count > 0 && InActivelist != null)
        //        {
        //            strProblemList = Environment.NewLine + "Inactive ProblemList :" + Environment.NewLine;
        //            strProblemtxt += strProblemList;
        //            // LocationY = LocationY + CreateLablesForPatientSummary(strProblemList, pnlprblst, LocationY, Color.Black.Name, "ProblemList");
        //            for (int h = 0; h < InActivelist.Count; h++)
        //            {
        //                strProblemList = string.Empty;
        //                string sColor = Color.Black.Name;
        //                string[] DateDiagnosed = null;
        //                string diagnosed = string.Empty;
        //                if (InActivelist[h].Date_Diagnosed != string.Empty)
        //                {
        //                    DateDiagnosed = InActivelist[h].Date_Diagnosed.Split('-');

        //                    if (DateDiagnosed.Length == 2 || DateDiagnosed.Length == 1)
        //                    {
        //                        if (DateDiagnosed.Length == 1)
        //                        {
        //                            diagnosed = " (" + InActivelist[h].Date_Diagnosed + ")";
        //                        }
        //                        else
        //                        {
        //                            InActivelist[h].Date_Diagnosed = DateDiagnosed[1] + "-" + DateDiagnosed[0] + "-01";

        //                            string smonth = Convert.ToDateTime(InActivelist[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
        //                            string[] sarrMonth = smonth.Split('-');


        //                            //Activelist[h].Date_Diagnosed = Convert.ToDateTime(Activelist[h].Date_Diagnosed.ToString("dd-MMM-dd"));
        //                            diagnosed = " (" + sarrMonth[1] + "-" + DateDiagnosed[0] + ")";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        InActivelist[h].Date_Diagnosed = Convert.ToDateTime(InActivelist[h].Date_Diagnosed).ToString("dd-MMM-yyyy");
        //                        diagnosed = " (" + InActivelist[h].Date_Diagnosed + ")";
        //                    }



        //                }
        //                if (InActivelist[h].ICD_Code != string.Empty)
        //                {
        //                    //if (Activelist[h].Status == "Active")
        //                    //{
        //                    //    strProblemList += Activelist[h].ICD_Code + "." + Activelist[h].Problem_Description + diagnosed + "\n";
        //                    //    sColor = Color.Black.Name;

        //                    //}
        //                    if (InActivelist[h].Status == "Inactive")
        //                    {
        //                        strProblemList += InActivelist[h].ICD_Code + "-" + InActivelist[h].Problem_Description + diagnosed + "\n";
        //                        sColor = Color.Black.Name;

        //                    }
        //                }
        //                else if (InActivelist[h].ICD_Code == string.Empty && InActivelist[h].Problem_Description != string.Empty)
        //                {
        //                    //if (Activelist[h].Status == "Active")
        //                    //{
        //                    //    strProblemList += Activelist[h].Problem_Description + diagnosed + "\n";
        //                    //    sColor = Color.Black.Name;

        //                    //}
        //                    if (InActivelist[h].Status == "Inactive")
        //                    {
        //                        strProblemList += InActivelist[h].Problem_Description + diagnosed + "\n";
        //                        sColor = Color.Black.Name;

        //                    }
        //                }
        //                //}
        //                strProblemtxt += strProblemList;

        //                // LocationY = LocationY + CreateLablesForPatientSummary(strProblemList, pnlprblst, LocationY, sColor, "ProblemList");
        //            }
        //        }
        //    }


        //    strProblemtxt = strProblemtxt.Replace("\r\n", "<br/>");
        //    lblProblemList.InnerHtml = strProblemtxt.Replace("\n", "<br/>");

        //    ////added by Pravin

        //    pnlProblemList_toolTip = strProblemtxt;
        //    sToolText += strProblemtxt + Environment.NewLine;
        //    pnlProblemList.Attributes.Add("title", pnlProblemList_toolTip);


        //    //MyToolTip.InitialDelay = 0;
        //    //MyToolTip.ShowAlways = true;
        //    //MyToolTip.UseAnimation = false;
        //    //MyToolTip.Active = true;
        //    //pnlprblst.MouseHover += new EventHandler(pnlprblst_MouseHover);
        //    //pnlprblst.MouseEnter += new EventHandler(pnl_MouseEnter);
        //}
        //private void FillVitals(IList<PatientResults> VitalsList)
        //{
        //    string sVitalsText = string.Empty;
        //    string pnlVitals_toolTip = string.Empty;
        //    IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
        //    if (ClientSession.VitalLimitLookup.Count > 0)
        //        iFieldLookupList = ClientSession.VitalLimitLookup;
        //    else
        //    {
        //        iFieldLookupList = objStaticLookupManager.getStaticLookupByFieldName("VITALS LIMIT LEVEL");
        //        ClientSession.VitalLimitLookup = iFieldLookupList;
        //    }
        //    string Text = "Vitals :" + Environment.NewLine;
        //    sVitalsText += Text;
        //    // int LocationY = CreateLablesForPatientSummary(Text, pnl, 0, Color.Black.Name, "Vitals");
        //    DateTime MaxCapturedDate = DateTime.MinValue;
        //    if (VitalsList != null && VitalsList.Count > 0)
        //    {
        //        MaxCapturedDate = VitalsList.Max(a => a.Captured_date_and_time);
        //    }
        //    if (VitalsList != null)
        //    {
        //        for (int i = 0; i < VitalsList.Count; i++)
        //        {
        //            if (VitalsList[i].Loinc_Observation.Contains("Second"))
        //            {
        //                VitalsList[i].Loinc_Observation = VitalsList[i].Loinc_Observation.Remove(VitalsList[i].Loinc_Observation.IndexOf("Second"), 6);
        //            }
        //            if (VitalsList[i].Loinc_Observation.Contains("$"))
        //            {
        //                VitalsList[i].Loinc_Observation = VitalsList[i].Loinc_Observation.Remove(VitalsList[i].Loinc_Observation.IndexOf("$"), 1);
        //            }
        //            if (i != VitalsList.Count - 1)
        //            {
        //                if (VitalsList[i + 1].Loinc_Observation.Contains("Second"))
        //                {
        //                    VitalsList[i + 1].Loinc_Observation = VitalsList[i + 1].Loinc_Observation.Remove(VitalsList[i + 1].Loinc_Observation.IndexOf("Second"), 6);
        //                }
        //                if (VitalsList[i + 1].Loinc_Observation.Contains("$"))
        //                {
        //                    VitalsList[i + 1].Loinc_Observation = VitalsList[i + 1].Loinc_Observation.Remove(VitalsList[i + 1].Loinc_Observation.IndexOf("$"), 1);
        //                }
        //            }
        //            if (i != VitalsList.Count - 2 && i != VitalsList.Count - 1)
        //            {
        //                if (VitalsList[i + 2].Loinc_Observation.Contains("Second"))
        //                {
        //                    VitalsList[i + 2].Loinc_Observation = VitalsList[i + 2].Loinc_Observation.Remove(VitalsList[i + 2].Loinc_Observation.IndexOf("Second"), 6);
        //                }
        //                if (VitalsList[i + 2].Loinc_Observation.Contains("$"))
        //                {
        //                    VitalsList[i + 2].Loinc_Observation = VitalsList[i + 2].Loinc_Observation.Remove(VitalsList[i + 2].Loinc_Observation.IndexOf("$"), 1);
        //                }
        //            }
        //            if (VitalsList[i].Value != string.Empty)
        //            {
        //                if (VitalsList[i].Loinc_Observation == "Height" && VitalsList[i].Value != string.Empty)
        //                {
        //                    if (VitalsList[i].Value.Contains("'") == false)
        //                    {
        //                        string sValue = ConversionOnRetrieval(VitalsList[i].Loinc_Observation, VitalsList[i].Value);
        //                        VitalsList[i].Value = sValue;
        //                    }
        //                    else
        //                    {
        //                        VitalsList[i].Value = VitalsList[i].Value;
        //                    }
        //                }
        //                IList<string> sList = (from h in iFieldLookupList where h.Value.ToUpper() == VitalsList[i].Loinc_Observation.ToUpper() select h.Description).ToList<string>();
        //                string sColor = Color.Black.Name;
        //                if (sList.Count != 0)
        //                    sColor = ValidateVitalUnits(VitalsList[i].Loinc_Observation, VitalsList[i].Value, sList[0]);
        //                if (VitalsList[i].Loinc_Observation == "Height" && VitalsList[i].Value != string.Empty)
        //                {
        //                    string[] d = VitalsList[i].Value.Split('\'');
        //                    Text = VitalsList[i].Loinc_Observation + " - " + d[0] + " Feet" + d[1] + " Inches" + "\n";
        //                }
        //                else
        //                {
        //                    if (i != VitalsList.Count - 1)
        //                    {
        //                        if (VitalsList[i].Loinc_Observation == VitalsList[i + 1].Loinc_Observation)
        //                        {
        //                            if (VitalsList[i].Captured_date_and_time == VitalsList[i + 1].Captured_date_and_time)
        //                            {
        //                                if (VitalsList[i + 1].Value != string.Empty)
        //                                {
        //                                    Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation) + " ," + VitalsList[i + 1].Value + AddVitalUnits(VitalsList[i + 1].Loinc_Observation) + "\n";
        //                                }
        //                                else
        //                                {
        //                                    Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation);

        //                                }
        //                            }
        //                            else if ((VitalsList[i].Captured_date_and_time > VitalsList[i + 1].Captured_date_and_time))
        //                            {
        //                                Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation) + "\n";
        //                            }
        //                            else if ((VitalsList[i].Captured_date_and_time < VitalsList[i + 1].Captured_date_and_time))
        //                            {
        //                                if (VitalsList[i + 1].Value != string.Empty)
        //                                {
        //                                    Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i + 1].Value + AddVitalUnits(VitalsList[i + 1].Loinc_Observation) + "\n";
        //                                }
        //                            }
        //                        }
        //                        if (i != VitalsList.Count - 2)
        //                        {
        //                            if (VitalsList[i].Loinc_Observation == VitalsList[i + 2].Loinc_Observation)
        //                            {
        //                                if (VitalsList[i].Captured_date_and_time == VitalsList[i + 2].Captured_date_and_time)
        //                                {
        //                                    if (VitalsList[i + 2].Value != string.Empty)
        //                                    {
        //                                        Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation) + " ," + VitalsList[i + 2].Value + AddVitalUnits(VitalsList[i + 2].Loinc_Observation) + "\n";
        //                                    }
        //                                    else
        //                                    {
        //                                        Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation);
        //                                    }
        //                                }
        //                                else if ((VitalsList[i].Captured_date_and_time > VitalsList[i + 2].Captured_date_and_time))
        //                                {
        //                                    Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation) + "\n";
        //                                }
        //                                else if ((VitalsList[i].Captured_date_and_time < VitalsList[i + 2].Captured_date_and_time))
        //                                {
        //                                    if (VitalsList[i + 2].Value != string.Empty)
        //                                    {
        //                                        Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i + 2].Value + AddVitalUnits(VitalsList[i + 2].Loinc_Observation) + "\n";
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (!sVitalsText.Contains(VitalsList[i].Loinc_Observation) && !Text.Contains(VitalsList[i].Loinc_Observation))
        //                    {
        //                        Text = VitalsList[i].Loinc_Observation + " - " + VitalsList[i].Value + AddVitalUnits(VitalsList[i].Loinc_Observation) + "\n";
        //                    }
        //                }
        //                if (VitalsList[i].Value != " ")
        //                {
        //                    sVitalsText += Text;
        //                    //LocationY = LocationY + CreateLablesForPatientSummary(Text, pnl, LocationY, sColor, "Vitals");
        //                    Text = string.Empty;
        //                }
        //            }
        //        }
        //    }
        //    sToolText += sVitalsText + Environment.NewLine;
        //    vitalsTip.Text = sVitalsText;
        //    vitalsTip.ToolTip = sVitalsText;
        //    pnlVitals_toolTip = sVitalsText;
        //    sVitalsText = sVitalsText.Replace("\r\n", "<br/>");
        //    lblVitals.InnerHtml = sVitalsText.Replace("\n", "<br/>");
        //    //MyToolTip.InitialDelay = 0;
        //    //MyToolTip.ShowAlways = true;
        //    //MyToolTip.UseAnimation = false;
        //    //MyToolTip.Active = true;
        //    //pnl.MouseHover += new EventHandler(pnl_MouseHover);
        //    //pnl.MouseEnter += new EventHandler(pnl_MouseEnter);
        //    pnlVitals.Attributes.Add("title", pnlVitals_toolTip);

        //}

        //public string ValidateVitalUnits(string Name, string Value, string AbnormalValue)
        //{
        //    if (Value != string.Empty)
        //    {
        //        switch (Name)
        //        {
        //            case "Body Temperature":
        //                {
        //                    string[] Split = AbnormalValue.Split('-');
        //                    if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "BP-Sitting Sys/Dia":
        //                {
        //                    string[] SplitNormal = Value.Split('/');
        //                    string[] Split = AbnormalValue.Split('/');
        //                    if (Convert.ToInt16(SplitNormal[0]) < Convert.ToInt16(Split[0]) || Convert.ToInt16(SplitNormal[1]) > Convert.ToInt16(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "BP-Standing Sys/Dia":
        //                {
        //                    string[] SplitNormal = Value.Split('/');
        //                    string[] Split = AbnormalValue.Split('/');
        //                    if (Convert.ToInt16(SplitNormal[0]) < Convert.ToInt16(Split[0]) || Convert.ToInt16(SplitNormal[1]) > Convert.ToInt16(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "BP-Lying Sys/Dia":
        //                {
        //                    string[] SplitNormal = Value.Split('/');
        //                    string[] Split = AbnormalValue.Split('/');
        //                    if (Convert.ToInt16(SplitNormal[0]) < Convert.ToInt16(Split[0]) || Convert.ToInt16(SplitNormal[1]) > Convert.ToInt16(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "Respiratory Rate":
        //                {
        //                    string[] Split = AbnormalValue.Split('-');
        //                    if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "Heart Rate":
        //                {
        //                    string[] Split = AbnormalValue.Split('-');
        //                    if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "Pulse Oximetry":
        //                {
        //                    string[] Split = AbnormalValue.Split('-');
        //                    if (Convert.ToDecimal(Value) < Convert.ToDecimal(Split[0]) || Convert.ToDecimal(Value) > Convert.ToDecimal(Split[1]))
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "BMI Status":
        //                {
        //                    if (Value.Contains(AbnormalValue) == false)
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "HbA1C Status":
        //                {
        //                    if (Value != AbnormalValue)
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "eGFR Status":
        //                {
        //                    if (Value != AbnormalValue)
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "Blood Sugar-Post Prandial Status":
        //                {
        //                    if (Value.Contains(AbnormalValue) == false)
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }
        //            case "Blood Sugar-Fasting Status":
        //                {
        //                    if (Value.Contains(AbnormalValue) == false)
        //                        return Color.Red.Name;
        //                    else
        //                        return Color.Black.Name;
        //                }

        //            default:
        //                {
        //                    return Color.Black.Name;
        //                }
        //        }
        //    }
        //    else
        //    {
        //        return Color.Black.Name;
        //    }
        //}
        //public string ConversionOnRetrieval(string vitalName, string vitalValue)
        //{
        //    int j = 0;
        //    string MethdName = ConvertInchtoFeetInch(vitalValue.ToString());
        //    string[] Splitter = { ".", "(", ",", ")" };
        //    string[] MthdInfo = MethdName.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
        //    if (MethdName.Length > 0)
        //    {

        //        string[] Arguments = new string[MthdInfo.Length];
        //        string ClassName = string.Empty;
        //        //string MethodName = MthdInfo[1];
        //        Arguments[j] = vitalValue;
        //        j++;
        //        return MethdName;
        //    }
        //    else
        //        return string.Empty;
        //}
        //string ConvertInchtoFeetInch(string s)
        //{
        //    if (s == string.Empty)
        //    {
        //        return s;
        //    }
        //    decimal inch = Convert.ToDecimal(s);
        //    decimal feet = Math.Floor(inch / 12m);
        //    decimal remainInch = decimal.Round((inch % 12m), 2);
        //    return feet.ToString() + "'" + " " + remainInch.ToString() + "''";
        //}
        //public string AddVitalUnits(string Name)
        //{
        //    switch (Name)
        //    {
        //        case "Height":
        //            {
        //                return " Feet'" + " inches";
        //            }
        //        case "Weight":
        //            {
        //                return " lbs";
        //            }
        //        case "Body Temperature":
        //            {
        //                return " F";
        //            }
        //        case "BP-Sitting Sys/Dia":
        //            {
        //                return " mm/Hg";
        //            }
        //        case "BP-Standing Sys/Dia":
        //            {
        //                return " mm/Hg";
        //            }
        //        case "BP-Lying Sys/Dia":
        //            {
        //                return " mm/Hg";
        //            }
        //        case "Respiratory Rate":
        //            {
        //                return " Breaths/min";
        //            }
        //        case "Heart Rate":
        //            {
        //                return " Beats/min";
        //            }
        //        case "Pulse Oximetry":
        //            {
        //                return " %";
        //            }
        //        case "HbA1C":
        //            {
        //                return " %";
        //            }
        //        case "eGFR":
        //            {
        //                return " ml/min/1.73m2";
        //            }
        //        case "Blood Sugar-Fasting":
        //            {
        //                return " mg/dl";
        //            }
        //        case "Blood Sugar-Post Prandial":
        //            {
        //                return " mg/dl";
        //            }
        //        case "BASAL":
        //            {
        //                return " units/ hr";
        //            }
        //        case "Head Circumference":
        //            {
        //                return " Inches";
        //            }
        //        case "Oxygen Intake":
        //            {
        //                return " LPM";
        //            }
        //        case "Oxygen Saturation":
        //            {
        //                return " %";
        //            }
        //        case "Neck Size":
        //            {
        //                return " Inches";
        //            }
        //        case "Pulse Rate":
        //            {
        //                return " Beats/min";
        //            }
        //        case "Waist Circumference":
        //            {
        //                return " cm";
        //            }
        //        case "LDL":
        //            {
        //                return " mg/dl";
        //            }
        //        case "HDL":
        //            {
        //                return " mg/dl";
        //            }
        //        case "Total Cholesterol":
        //            {
        //                return " mg/dl";
        //            }
        //        case "Triglycerides":
        //            {
        //                return " mg/dl";
        //            }
        //        case "Creatinine":
        //            {
        //                return " mg/dl";
        //            }
        //        case "TSH":
        //            {
        //                return " mIU/L";
        //            }
        //        case "Hgb":
        //            {
        //                return " g/dL";
        //            }
        //        case "Calcium Score":
        //            {
        //                return " mg/dL";
        //            }
        //        case "Lp(a)":
        //            {
        //                return " mg/dL";
        //            }
        //        case "ISF": //added by naveena for bug_id=26435 on 31.10.2014
        //            {
        //                return " Mg/dL";
        //            }
        //        case "ICR":
        //            {
        //                return " gms";
        //            }
        //        case "PT":
        //            {
        //                return " sec.";
        //            }
        //        case "Urine for Microalbumin":
        //            {
        //                return " mg/dL";
        //            }
        //        default:
        //            {
        //                return string.Empty;
        //            }
        //    }
        //}

        public void LoadPatientPhoto()
        {
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms  - LoadPatientPhoto Frontend API [To load patient photo] : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            FTPImageProcess ftpImage = new FTPImageProcess();
            string ftpServerIP = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;

            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"];
            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];

            string sPath = Page.MapPath("atala-capture-download/" + Session.SessionID);

            DirectoryInfo virdir = new DirectoryInfo(sPath);
            if (!virdir.Exists)
            {
                virdir.Create();
            }

            ftpImage.DownloadFromImageServer("PatientPhoto", ftpServerIP, ftpUserName, ftpPassword, Path.GetFileName(ClientSession.FillPatientChart.PatChartList[0].PhotoPath.ToString()), sPath);

            imgOverAllSummary.ImageUrl = "~//atala-capture-download/" + Session.SessionID + "//" + Path.GetFileName(ClientSession.FillPatientChart.PatChartList[0].PhotoPath.ToString());
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms  - LoadPatientPhoto Frontend API [To load patient photo]: End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
        }

        public class Result_Date_Order
        {
            public DateTime _Res_Date = DateTime.MinValue;
            public ulong _Ord_Id = 0;
            public string _Doc_Type = string.Empty;
            public string _Img_path = string.Empty;
            public ulong _Res_Id = 0;
            public string _Order_Description = string.Empty;

            public ulong Order_Submit_Id
            {
                get { return _Ord_Id; }
                set { _Ord_Id = value; }
            }


            public DateTime Result_Date
            {
                get { return _Res_Date; }
                set { _Res_Date = value; }
            }

            public string Ord_Type
            {
                get { return _Doc_Type; }
                set { _Doc_Type = value; }
            }

            public ulong Result_Id
            {
                get { return _Res_Id; }
                set { _Res_Id = value; }
            }

            public string Img_Path
            {
                get { return _Img_path; }
                set { _Img_path = value; }
            }

            public string Order_Description
            {
                get { return _Order_Description; }
                set { _Order_Description = value; }
            }

        }
        public void FillPatientPane()
        {
            FillPatientChart objPatChart = new FillPatientChart();
            if (ClientSession.FillPatientChart != null)
                objPatChart = ClientSession.FillPatientChart;

            if (ClientSession.HumanId == 0)
            {
                lblPatientStrip.InnerText = " ";
                return;
            }
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - FillPatientPane API [Fill the Patient Summary Bar] : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            string sPriPlan = string.Empty;
            string sSecPlan = string.Empty;
            string sPriCarrier = string.Empty;
            string sSecCarrier = string.Empty;

            HtmlGenericControl icon = new HtmlGenericControl("i");
            //icon.Attributes.Add("class", "fa fa-medkit");
            //icon.Style.Value = "font-size:20px;color:#057105;cursor:pointer;";
            //icon.Attributes.Add("onclick", "getInsuranceDetails();");
            //icon.Attributes.Add("id", "spnPatientstrip");
            LiteralControl li = new LiteralControl();
            HtmlGenericControl sqre = new HtmlGenericControl();
            sqre.Attributes.Add("id", "sqre");
            sqre.Attributes.Add("class", "tooltpShow");
            sqre.Style.Value = "position: absolute;background-color: white;border-top: 1px solid #ccc;display:none;margin-top: 22px;margin-left: -10px;width: 12px;height: 12px;transform: rotate(45deg);border-left: 1px solid #ccc;";
            sqre.InnerText = " ";
            HtmlGenericControl tooltp = new HtmlGenericControl("span");
            tooltp.Style.Value = "position: absolute;background-color: white;display:none;border: 1px solid #ccc;border-radius: 6px;color: black;font-weight: normal;margin-top: 27px;margin-left: -25px;padding: 5px;";
            tooltp.Attributes.Add("id", "tooltp");
            tooltp.Attributes.Add("class", "tooltpShow");
            HtmlGenericControl imgWAIT = new HtmlGenericControl("img");
            imgWAIT.Attributes.Add("src", "./Resources/wait.ico");
            imgWAIT.Style.Add("padding-top", "11px");
            imgWAIT.Style.Add("display", "none");
            imgWAIT.Attributes.Add("id", "imgwait");

            if (objPatChart.PatChartList != null && objPatChart.PatChartList.Count > 0)
            {
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - FillPatientPane - Call FillPatientSummaryBarforPatientChart : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                string strPatientstrip = FillPatientSummaryBarforPatientChart(objPatChart.PatChartList[0].Last_Name, objPatChart.PatChartList[0].First_Name, objPatChart.PatChartList[0].MI, objPatChart.PatChartList[0].Suffix, objPatChart.PatChartList[0].Birth_Date, objPatChart.PatChartList[0].Human_Id, objPatChart.PatChartList[0].Medical_Record_Number, objPatChart.PatChartList[0].HomePhoneNo, objPatChart.PatChartList[0].Sex, objPatChart.PatChartList[0].Patient_Status, objPatChart.PatChartList[0].SSN, objPatChart.PatChartList[0].Patient_Type, sPriPlan, sPriCarrier, sSecPlan, sSecCarrier, objPatChart.PatChartList[0].CellPhoneNo, objPatChart.PatChartList[0].PastDue, objPatChart.PatChartList[0].ACO_Is_Eligible_Patient);
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - FillPatientPane - Call FillPatientSummaryBarforPatientChart : End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                string strPatientstriptext = string.Empty;
                if (sPriPlan != string.Empty || sSecPlan != string.Empty)
                {
                    strPatientstriptext = strPatientstrip.Split('^')[0];
                }
                else
                {
                    strPatientstriptext = strPatientstrip;
                }

                //lblPatientStrip.InnerText = strPatientstriptext.Replace("^", "");
                li.Text = strPatientstriptext.Replace("^", "");
                hdnPatientStrip.Value = strPatientstriptext.Replace("^", ""); //.Replace("<span style='color: red;'>", "").Replace("</span>", "");

                if (lblPatientStrip.InnerText.Length < 195)
                {
                    icon.Attributes.Add("class", "fa fa-medkit");
                    icon.Style.Value = "font-size:20px;color:#057105;cursor:pointer;visible: false;";
                    icon.Attributes.Add("onclick", "getInsuranceDetails();");
                    icon.Attributes.Add("id", "spnPatientstrip");
                    //spnPatientstrip1.Visible = false;
                }
                else
                {
                    icon.Attributes.Add("class", "fa fa-medkit");
                    icon.Style.Value = "font-size:20px;color:#057105;cursor:pointer;position:static;visible: true;margin-left: 2px;"; //margin-left: -366px;
                    icon.Attributes.Add("onclick", "getInsuranceDetails();");
                    icon.Attributes.Add("id", "spnPatientstrip");

                    //lblPatientStrip.InnerText = string.Concat(strPatientstriptext.Substring(0, 208), "...");
                    //lblPatientStrip.InnerText = string.Concat(strPatientstriptext.Substring(0, 195), "...");
                    li.Text = string.Concat(strPatientstriptext.Substring(0, 195), "...");
                }


                lblPatientStrip.Controls.Add(li);
                lblPatientStrip.Controls.Add(icon);
                lblPatientStrip.Controls.Add(imgWAIT);
                lblPatientStrip.Controls.Add(tooltp);
                lblPatientStrip.Controls.Add(sqre);
            }

            string patientName = string.Empty;
            if (objPatChart.PatChartList != null && objPatChart.PatChartList.Count > 0)
            {
                patientName = objPatChart.PatChartList[0].Last_Name + "  " + objPatChart.PatChartList[0].First_Name + "  " + objPatChart.PatChartList[0].MI;
                AddWindowItem("Patient Chart - " + patientName + " ~ " + objPatChart.PatChartList[0].Human_Id + "#" + ClientSession.Selectedencounterid.ToString() + "$" + ClientSession.CurrentObjectType + "^" + objPatChart.PatChartList[0].Date_of_Service);
            }
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - FillPatientPane API [Fill the Patient Summary Bar]: End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
        }
        public int CalculateAge(DateTime birthDate)
        {

            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = 0;
            if (birthDate != null)
                years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }

        public string FillPatientSummaryBarforPatientChart(string LastName, string FirstName, string MI, string Suffix, DateTime DOB, ulong ulHumanID, string MedRecNo, string HomePhoneNo, string Sex, string PatientStatus, string SSN, string PatientType, string sPriPlan, string sPriCarrier, string sSecPlan, string sSecCarrier, string CellPhoneNo, string PastDue, string sAcoEligiblePatient)
        {

            string sMySummary;
            string sPatientSex = string.Empty;
            if (Sex != string.Empty) //For Bug Id 48937 
            {
                if (Sex.Substring(0, 1).ToUpper() == "U")
                {
                    sPatientSex = "UNK";
                }
                else
                {
                    sPatientSex = Sex.Substring(0, 1);
                }

            }
            else
            {
                sPatientSex = " ";
            }

            string phoneno = "";
            if (HomePhoneNo.Length == 14)
            {
                phoneno = HomePhoneNo;
            }
            else
            {
                phoneno = CellPhoneNo;
            }


            if (PatientStatus == "DECEASED")
            {
                //sMySummary = LastName + "," + FirstName +
                //   "  " + MI + "  " + Suffix + "   |   " +
                //   DOB.ToString("dd-MMM-yyyy") + "   |   " +
                //   (CalculateAge(DOB)).ToString() +
                //   "  year(s)    |   " + sPatientSex + "   |   " + PatientStatus + "   |   Acct #:" + ulHumanID +
                //   "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
                //   "Phone #:" + phoneno + "   |   ";

                sMySummary = LastName + "," + FirstName +
                   "  " + MI + "  " + Suffix + "   |   " +
                   DOB.ToString("dd-MMM-yyyy") + "   |   " +
                   (CalculateAge(DOB)).ToString() +
                   "  year(s)    |   " + sPatientSex + "   |   Acct #:" + ulHumanID +
                   "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
                   "Phone #:" + phoneno + "   |   " + PatientStatus + "   |   ";
            }
            else
            {
                sMySummary = LastName + "," + FirstName +
               "  " + MI + "  " + Suffix + "   |   " +
               DOB.ToString("dd-MMM-yyyy") + "   |   " +
               (CalculateAge(DOB)).ToString() +
               "  year(s)    |   " + sPatientSex + "   |   Acct #:" + ulHumanID +
               "   |   " + "Med Rec #:" + MedRecNo + "   |   " +
               "Phone #:" + phoneno + "   |   ";

            }

            ClientSession.SummaryList = "Name: " + LastName + " " + FirstName + " " + MI + " " + Suffix + " | DOB:  " + DOB.ToString("dd-MMM-yyyy") + " | Acc #:" + ulHumanID;
            if (PatientType != string.Empty)
            {
                sMySummary += "Patient Type:" + PatientType + "   |   ";
            }
            if (sPriPlan != string.Empty)
            {
                sMySummary += "^" + "Pri Plan:" + sPriCarrier + " - " + sPriPlan + "   |   ";
            }
            if (sSecPlan != string.Empty)
            {
                sMySummary += "^" + "Sec Plan:" + sSecCarrier + " - " + sSecPlan + "   |   ";
            }
            if (SSN != string.Empty)
            {
                sMySummary += "^" + "SSN:" + SSN + "   |   ";
            }

            sMySummary += "Past Due: $ " + PastDue + "   |   ";

            if (sAcoEligiblePatient != null && sAcoEligiblePatient != string.Empty && sAcoEligiblePatient != "N")
            {
                sMySummary += sAcoEligiblePatient + "   |   ";
            }

            //sMySummary += "<span style='color: red;'>" + "Past Due: $ " + PastDue + "   |   " + "</span>";
            /*
>>>>>>> 1.182.14.65.6.6.10.1.4.1.10.3.2.2.4.1.2.1.2.1.8.2.8.1.2.2.26.1.2.1.2.3.8.2.8.1.6.1.2.7.2.1.8.3.10.2.4.4
            string HumanFile= string.Empty;
            if(ClientSession.HumanId != null)
             HumanFile = "Human" + "_" + ClientSession.HumanId + ".xml";
            string sHumanFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], HumanFile);

            if (File.Exists(sHumanFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(sHumanFilePath);
                try
                {
                    itemDoc.Load(XmlText);
                }
                catch 
                {
                    ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                    return string.Empty;
                }

                XmlText.Close();
                XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                if (xmlAgenode != null && xmlAgenode.Count > 0)
                    xmlAgenode[0].Attributes[0].Value = CalculateAge(DOB).ToString();
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                }
                itemDoc.Save(sHumanFilePath);
            }*/

            ClientSession.PatientPane = sMySummary;

            return sMySummary;
        }
        FillPatientChart objPatChart = new FillPatientChart();
        string refreshTabName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            Patientchartload();
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PatientChartLoad", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }

        public void Patientchartload()
        {
            //EncounterManager objUserLookupManager = new EncounterManager();
            //IList<string> userLookup = objUserLookupManager.GetEncounterListArray(ClientSession.HumanId);
            string ChildTabName = string.Empty;
            string cboSourceOfInformation = string.Empty;
            if (Request.QueryString["openingfrom"] != null)
            {
                //ClientSession.SelectedFrom = Request.QueryString["openingfrom"].ToString();
                ClientSession.SelectedFrom = Request.QueryString["openingfrom"].ToString().Replace("?", "");
            }
            else
            {
                ClientSession.SelectedFrom = string.Empty;
            }

            if (Request.QueryString["tabName"] != null)
                refreshTabName = Request.QueryString["tabName"].ToString();

            if (Request.QueryString["ChildTabName"] != null)
                ChildTabName = Request.QueryString["ChildTabName"].ToString();

            if (Request.QueryString["cboValue"] != null)
                cboSourceOfInformation = Request.QueryString["cboValue"].ToString();


            if (Request.QueryString["hdnLocalTime"] != null)
            {
                hdnLocalTime.Value = Request.QueryString["hdnLocalTime"].ToString();
            }


            if (Request.QueryString["currentAddendumId"] != null)
                hdnAddendumID.Value = Request.QueryString["currentAddendumId"].ToString();
            else
                hdnAddendumID.Value = "0";

            //ClientSession.UserName = "JPRASAD";
            //ulMyHumanID = 48202;
            //ulMySelectedEncID = 142477;
            //if (!(Request.QueryString["openingfrom"] != null && Request.QueryString["openingfrom"].ToString() == "Menu"))
            //{
            //    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "OpeningHumanXMLfromMyQ");

            //}
            FillPatientChart objPatChart = new FillPatientChart();

            // if (!IsPostBack)
            {




                if (Request["HumanID"] != null && Request["HumanID"] != "undefined")
                {
                    //Bug id 70531 - To avoid crash error if the input HumanID is not valid
                    try
                    {
                        ClientSession.HumanId = Convert.ToUInt64(Request["HumanID"].ToString().Replace("?", ""));
                    }
                    catch
                    {

                    }
                }
                if (Request["EncounterID"] != null)
                {
                    ClientSession.Selectedencounterid = Convert.ToUInt64(Request["EncounterID"]);
                    ClientSession.EncounterId = Convert.ToUInt64(Request["EncounterID"]);
                }
                if (Request["Source"] != null && Request["Source"] == "WindowItem")
                {
                    ArrayList lstWindow = ClientSession.WindowList;
                    string sID = string.Empty;
                    string shumanID = string.Empty;
                    string sObjType = string.Empty;
                    foreach (object window in lstWindow)
                    {
                        if (window.ToString().Contains(ClientSession.HumanId.ToString()))
                        {
                            sID = window.ToString().Split('#')[1].Split('$')[0];
                            //sObjType = window.ToString().Split('#')[1].Split('$')[1];
                            sObjType = window.ToString().Split('#')[1].Split('$')[1].Split('^')[0];
                            //shumanID=sValue[3].Split('#')[0];
                            string[] sText = window.ToString().Split('#')[0].Split('~');
                            shumanID = sText[sText.Length - 1];
                        }
                    }
                    if (sID != string.Empty)
                    {
                        ClientSession.Selectedencounterid = Convert.ToUInt64(sID);
                        ClientSession.EncounterId = Convert.ToUInt64(sID);
                    }
                    if (shumanID != string.Empty)
                    {
                        ClientSession.HumanId = Convert.ToUInt64(shumanID);
                    }

                    ClientSession.CurrentObjectType = sObjType;
                    ClientSession.SetSelectedTab = string.Empty;
                }
                sGroup_ID_Log = ClientSession.EncounterId.ToString() + "-" + ClientSession.HumanId.ToString() + "-" + ClientSession.PhysicianId.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF");
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - PageLoad : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");

                if (HttpContext.Current.Session["PatChartRedirectVlaues"] != null)//Added for CarePointe
                {
                    string PatChartValues = HttpContext.Current.Session["PatChartRedirectVlaues"].ToString();
                    ClientSession.HumanId = Convert.ToUInt64(PatChartValues.Split('&')[0]);
                    ClientSession.EncounterId = Convert.ToUInt64(PatChartValues.Split('&')[1]);
                    ClientSession.Selectedencounterid = Convert.ToUInt64(PatChartValues.Split('&')[1]);
                    hdnLocalTime.Value = PatChartValues.Split('&')[2];
                    WFObjectManager wfm = new WFObjectManager();
                    string Obj_Type = "";
                    string Curr_Process = "";
                    bool Owner_Enc_Mismatch = true;
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call Backend API GetWfObjDetails: Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                    if (ClientSession.UserName != null && ClientSession.EncounterId != null)
                        wfm.GetWfObjDetails(ClientSession.UserName, ClientSession.EncounterId, out Obj_Type, out Curr_Process, out Owner_Enc_Mismatch);
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call Backend API GetWfObjDetails: End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");

                    hdnOwnerEncMismatch.Value = Owner_Enc_Mismatch.ToString().ToUpper();
                    hdnOwnerEncMismatchEncID.Value = ClientSession.EncounterId.ToString();
                    if (!Owner_Enc_Mismatch)
                    {
                        ClientSession.UserCurrentProcess = Curr_Process;
                        ClientSession.CurrentObjectType = Obj_Type;
                    }
                }
                //
                //
                //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

                //if (File.Exists(strXmlFilePath) == false && ClientSession.EncounterId > 0)
                //{

                EncounterBlobManager EncounterBlobMngr = new EncounterBlobManager();
                Encounter_Blob objEncounterblob = null;
                IList<Encounter_Blob> ilstEncounterBlob = EncounterBlobMngr.GetEncounterBlob(ClientSession.EncounterId);
                if (ilstEncounterBlob.Count == 0 && ClientSession.EncounterId > 0)
                {
                    //objEncounterblob = ilstEncounterBlob[0];

                    throw new Exception("Encounter XML is not found for Encounter ID " + ClientSession.EncounterId + ". Please contact support.");

                    string sDirectoryPath = string.Empty;
                    if (Directory.Exists(HttpContext.Current.Server.MapPath("Template_XML")))
                        sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    string sXmlPath = string.Empty;
                    if (File.Exists(Path.Combine(sDirectoryPath, "Base_XML.xml")))
                        sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");
                loop:
                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    try
                    {
                        itemDoc.Load(XmlText);
                    }
                    //catch
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                    //    return;
                    //}
                    catch (Exception ex)
                    {
                        // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1011190');", true);
                            XmlText.Close();

                            //UtilityManager.GenerateXML(ClientSession.EncounterId.ToString(), "Encounter");
                            //goto loop;
                            ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.EncounterId.ToString() + "','Encounter','patientchart');", true);


                            //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                            return;
                        }

                    }
                    XmlText.Close();

                    XmlNodeList xmlAgenode = itemDoc.GetElementsByTagName("Age");
                    if (xmlAgenode != null && xmlAgenode.Count > 0)
                        xmlAgenode[0].ParentNode.RemoveChild(xmlAgenode[0]);

                   // itemDoc.Save(strXmlFilePath);

                //    int trycount = 0;
                //trytosaveagain:
                    try
                    {
                        //itemDoc.Save(strXmlFilePath);

                        IList<Encounter_Blob> ilstUpdateBlob = new List<Encounter_Blob>();
                        byte[] bytes = null;
                        try
                        {
                            bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                        }
                        catch (Exception ex)
                        {

                        }
                        objEncounterblob.Encounter_XML = bytes;
                        ilstUpdateBlob.Add(objEncounterblob);
                        EncounterBlobMngr.SaveEncounterBlobWithTransaction(ilstUpdateBlob, string.Empty);
                    }
                    catch (Exception xmlexcep)
                    {
                        throw new Exception(xmlexcep.Message.ToString());
                        //trycount++;
                        //if (trycount <= 3)
                        //{
                        //    int TimeMilliseconds = 0;
                        //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //    Thread.Sleep(TimeMilliseconds);
                        //    string sMsg = string.Empty;
                        //    string sExStackTrace = string.Empty;

                        //    string version = "";
                        //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //    string[] server = version.Split('|');
                        //    string serverno = "";
                        //    if (server.Length > 1)
                        //        serverno = server[1].Trim();

                        //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //        sMsg = xmlexcep.InnerException.Message;
                        //    else
                        //        sMsg = xmlexcep.Message;

                        //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //        sExStackTrace = xmlexcep.StackTrace;

                        //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //    string ConnectionData;
                        //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //    {
                        //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //        {
                        //            cmd.Connection = con;
                        //            try
                        //            {
                        //                con.Open();
                        //                cmd.ExecuteNonQuery();
                        //                con.Close();
                        //            }
                        //            catch
                        //            {
                        //            }
                        //        }
                        //    }
                        //    goto trytosaveagain;
                        //}
                    }
                }

                //string FileName1 = "Human" + "_" + ClientSession.HumanId + ".xml";
                //string strXmlFilePath1 = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName1);

                //if (File.Exists(strXmlFilePath1) == false && ClientSession.HumanId > 0)
                //{

                HumanBlobManager HumanBlobMngr = new HumanBlobManager();
                Human_Blob objHumanblob = null;
                IList<Human_Blob> ilstHumanBlob = HumanBlobMngr.GetHumanBlob(ClientSession.HumanId);
                if (ilstHumanBlob.Count == 0 && ClientSession.HumanId > 0)
                {
                    //objHumanblob = ilstHumanBlob[0];

                    throw new Exception("Human XML is not found for Human ID " + ClientSession.HumanId + ". Please contact support.");

                    string sDirectoryPath = string.Empty;
                    if (Directory.Exists(HttpContext.Current.Server.MapPath("Template_XML")))
                        sDirectoryPath = HttpContext.Current.Server.MapPath("Template_XML");
                    string sXmlPath = string.Empty;
                    if (File.Exists(Path.Combine(sDirectoryPath, "Base_XML.xml")))
                        sXmlPath = Path.Combine(sDirectoryPath, "Base_XML.xml");

                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader XmlText = new XmlTextReader(sXmlPath);
                    try
                    {
                        itemDoc.Load(XmlText);
                    }
                    catch
                    {
                        XmlText.Close();
                        // ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.HumanId.ToString() + "','Human','patientchart');", true);


                        //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

                        return;
                    }
                    XmlText.Close();
                   // itemDoc.Save(strXmlFilePath1);
                //    int trycount = 0;
                //trytosaveagain:
                    try
                    {
                        //itemDoc.Save(strXmlFilePath1);

                        IList<Human_Blob> ilstUpdateBlob = new List<Human_Blob>();
                        byte[] bytes = null;
                        try
                        {
                            bytes = System.Text.Encoding.Default.GetBytes(itemDoc.OuterXml);
                        }
                        catch (Exception ex)
                        {

                        }
                        objHumanblob.Human_XML = bytes;
                        ilstUpdateBlob.Add(objHumanblob);
                        HumanBlobMngr.SaveHumanBlobWithTransaction(ilstUpdateBlob, string.Empty);
                    }
                    catch (Exception xmlexcep)
                    {
                        throw new Exception(xmlexcep.Message.ToString());
                        //trycount++;
                        //if (trycount <= 3)
                        //{
                        //    int TimeMilliseconds = 0;
                        //    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                        //        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                        //    Thread.Sleep(TimeMilliseconds);
                        //    string sMsg = string.Empty;
                        //    string sExStackTrace = string.Empty;

                        //    string version = "";
                        //    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                        //        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                        //    string[] server = version.Split('|');
                        //    string serverno = "";
                        //    if (server.Length > 1)
                        //        serverno = server[1].Trim();

                        //    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                        //        sMsg = xmlexcep.InnerException.Message;
                        //    else
                        //        sMsg = xmlexcep.Message;

                        //    if (xmlexcep != null && xmlexcep.StackTrace != null)
                        //        sExStackTrace = xmlexcep.StackTrace;

                        //    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        //    string ConnectionData;
                        //    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        //    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                        //    {
                        //        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                        //        {
                        //            cmd.Connection = con;
                        //            try
                        //            {
                        //                con.Open();
                        //                cmd.ExecuteNonQuery();
                        //                con.Close();
                        //            }
                        //            catch
                        //            {
                        //            }
                        //        }
                        //    }
                        //    goto trytosaveagain;
                        //}
                    }
                }


                //
                //Commented to fill FillPatientChart object to be used in Addendum, the commented call returns Empty object.
                //if (hdnLocalTime.Value.Trim() == string.Empty)
                //    ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.Selectedencounterid, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, bLoad, Convert.ToUInt32(hdnAddendumID.Value));//0);
                //else
                //    ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.Selectedencounterid, UtilityManager.ConvertToLocal(DateTime.ParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null)), string.Empty, ClientSession.UserName, bLoad, Convert.ToUInt32(hdnAddendumID.Value));// 0);
                bool bIsArchive = false;
                if (ClientSession.CurrentObjectType.ToUpper() == "ADDENDUM")
                    bIsArchive = true;
                DateTime tempDate = DateTime.MinValue;
            ln:
                try
                {
                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call LoadPatientChart Manager API : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                    if (hdnLocalTime.Value != null && hdnLocalTime.Value.Trim() == string.Empty || DateTime.TryParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null, System.Globalization.DateTimeStyles.None, out tempDate) == false)
                        ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.UtcNow), string.Empty, ClientSession.UserName, true, Convert.ToUInt32(hdnAddendumID.Value), ClientSession.CurrentObjectType, bIsArchive);//0);
                    else
                    {
                        ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.EncounterId, UtilityManager.ConvertToLocal(DateTime.ParseExact(hdnLocalTime.Value.Trim(), "M/d/yyyy H:m:s", null)), string.Empty, ClientSession.UserName, true, Convert.ToUInt32(hdnAddendumID.Value), ClientSession.CurrentObjectType, bIsArchive);// 0);
                    }

                    UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - LoadSummaryFromOtherForms API - Call LoadPatientChart Manager API : End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
                }
                //catch (Exception ex)
                //{

                //    if (ex.Message.ToLower().Contains("unexpected end of file") == true)
                //    {
                //        ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                //        return;
                //    }
                //    else if (ex.Message.ToLower().Contains("is an unexpected token") == true)
                //    {
                //        ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                //        return;
                //    }
                //    else if (ex.Message.ToLower().Contains("input string was not") == true)
                //    {
                //        ScriptManager.RegisterStartupScript(this, typeof(frmPatientChart), "ErrorMessage", "alert('The XML file is corrupted. Kindly contact support team to regenerate the XML.');", true);
                //        return;
                //    }
                //}
                catch (Exception ex)
                {
                    // if (ex.Message.ToLower().Contains("input string was not") == true || ex.Message.ToLower().Contains("element") == true||ex.Message.ToLower().Contains("unexpected end of file") == true || ex.Message.ToLower().Contains("is an unexpected token") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "RegenerateXML('" + ClientSession.HumanId.ToString() + "','Human','patientchart');", true);


                        //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                        return;
                        // goto ln;
                    }

                }
                if (ClientSession.FillPatientChart.PatChartList != null && ClientSession.FillPatientChart.PatChartList.Count > 0 && ClientSession.FillPatientChart.PatChartList[0].PhotoPath.Trim().ToString() != string.Empty && ClientSession.FillPatientChart.PatChartList[0].PhotoPath.Trim().ToString() != "")
                    LoadPatientPhoto();




                if (Request["from"] != null && Request["from"].ToString().IndexOf("viewresult") > -1)
                {
                    tblPatientSummary.Visible = false;

                }
                if (Request["from"] != null && Request["from"].ToString().IndexOf("openpatientchart") > -1)
                {
                    tblPatientSummary.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "invisibleResult();", true);
                }

                //  if (!IsPostBack)
                {

                    //Commented by Srividhya - since this screen is called from Charge Posting screen
                    //ClientSession.FlushSession();
                    //if (Request["HumanID"] != null)
                    //{
                    //    ClientSession.HumanId = Convert.ToUInt64(Request["HumanID"]);
                    //}
                    //ClientSession.FillPatientChart = objEncounterManager.LoadPatientChart(ClientSession.HumanId, ClientSession.Selectedencounterid, UtilityManager.ConvertToLocal(DateTime.Now), string.Empty, ClientSession.UserName, bLoad, 0);
                    if (ClientSession.HumanId != null)
                        hdnHumanNo.Value = ClientSession.HumanId.ToString();
                    hdnIndexId.Value = "0";
                    hdnOrderSubmitId.Value = "0";
                    hdnResultId.Value = "0";
                    //Added By priyangha 
                    if (ClientSession.HumanId != null && ClientSession.HumanId != 0)
                    {
                        hdnHumanNo.Value = ClientSession.HumanId.ToString();
                    }
                }



            }
            // LoadSummaryFromOtherForms(true);

            if (ClientSession.FillPatientChart != null)
                objPatChart = ClientSession.FillPatientChart;

            //if (!IsPostBack)
            {
                SecurityServiceUtility objsecurity = new SecurityServiceUtility();
                objsecurity.ApplyUserPermissions(this.Page);
                // ClientSession.UserCurrentOwner = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.DocumentationWFRecord.Current_Owner;
                if (ClientSession.EncounterId != null)
                    FillClientSession(ClientSession.EncounterId, true);

                hdnQencounterId.Value = ClientSession.EncounterId.ToString();
                // Index value is added by Manimozhi - For Medication Refresh in Patient Summary Bar. 
                //string sSentToRCopia = string.Empty;
                string Patient_name = string.Empty;
                //if (objPatChart.PatChartList != null && objPatChart.PatChartList.Count > 0)
                //{
                //    sSentToRCopia = objPatChart.PatChartList[0].Is_Sent_To_RCopia;
                //}
                if (hdnOwnerEncMismatch.Value.ToUpper() != "TRUE")
                {
                    if (Request["Index"] == null)
                    {
                        if (Request["ScreenMode"] != null && Request["ScreenMode"] == "Menu")
                        {
                            //Srividhya added on 3-Jul-2014
                            UIManager.is_Menu_Level_PFSH = true;
                            ClientSession.EncounterId = 0;
                        }
                        else
                        {
                            UIManager.is_Menu_Level_PFSH = false;
                        }
                        if (ClientSession.EncounterId != null && ClientSession.EncounterId != 0 && Request["ScreenName"] != "PFSH")
                        {
                            //if (Is_CMG)
                            //{
                            //    EncounterContainer.Attributes["src"] = "frmCMGTest.aspx?tabName=Test Details";
                            //    Is_CMG = false;
                            //}
                            //else
                            //For Bug ID 74727
                            //EncounterContainer.Attributes["src"] = "frmEncounter.aspx?tabName=" + refreshTabName + "&sMySentToRCopia=" + sSentToRCopia + "&ChildTabName=" + ChildTabName + "&cboValue=" + cboSourceOfInformation + "&Date=" + hdnLocalTime.Value + "&currentAddendumId=" + hdnAddendumID.Value;
                            EncounterContainer.Attributes["src"] = "frmEncounter.aspx?tabName=" + refreshTabName + "&ChildTabName=" + ChildTabName + "&cboValue=" + cboSourceOfInformation + "&Date=" + hdnLocalTime.Value + "&currentAddendumId=" + hdnAddendumID.Value;
                        }
                    }
                    //else if (Is_CMG)
                    //{
                    //    EncounterContainer.Attributes["src"] = "frmCMGTest.aspx?tabName=Test Details";
                    //    Is_CMG = false;
                    //}
                    else
                    {
                        EncounterContainer.Attributes["src"] = "frmEncounter.aspx?tabName=" + refreshTabName + "&Index=" + Request["Index"].ToString() + "&ChildTabName=" + ChildTabName + "&cboValue=" + cboSourceOfInformation + "&Date=" + hdnLocalTime.Value + "&currentAddendumId=" + hdnAddendumID.Value;//?EncounterID=" + ClientSession.EncounterId.ToString() + "&HumanID=" + ClientSession.HumanId.ToString() + "&ulMyPhysicianID=" + ClientSession.PhysicianId.ToString() + "&SelectedEncounterID=0";
                        //For Bug ID 74727 
                        // EncounterContainer.Attributes["src"] = "frmEncounter.aspx?tabName=" + refreshTabName + "&sMySentToRCopia=" + sSentToRCopia + "&Index=" + Request["Index"].ToString() + "&ChildTabName=" + ChildTabName + "&cboValue=" + cboSourceOfInformation + "&Date=" + hdnLocalTime.Value + "&currentAddendumId=" + hdnAddendumID.Value;//?EncounterID=" + ClientSession.EncounterId.ToString() + "&HumanID=" + ClientSession.HumanId.ToString() + "&ulMyPhysicianID=" + ClientSession.PhysicianId.ToString() + "&SelectedEncounterID=0";
                    }
                }

                // EncounterContainer.Attributes["src"] = "frmEncounter.aspx?tabName='" + refreshTabName + "'&sMySentToRCopia=" + sSentToRCopia;//?EncounterID=" + ClientSession.EncounterId.ToString() + "&HumanID=" + ClientSession.HumanId.ToString() + "&ulMyPhysicianID=" + ClientSession.PhysicianId.ToString() + "&SelectedEncounterID=0";
                //this.Title = UtilityManager.AssignScreenTitle(this, this.Text) +

                //" - " + objPatChart.PatChartList[0].Last_Name + "," + objPatChart.PatChartList[0].First_Name +
                //"  " + objPatChart.PatChartList[0].MI + "  " + objPatChart.PatChartList[0].Suffix;
                //Patient_name = " - " + objPatChart.PatChartList[0].Last_Name + "," + objPatChart.PatChartList[0].First_Name +
                //"  " + objPatChart.PatChartList[0].MI + "  " + objPatChart.PatChartList[0].Suffix;
                var EncList = from el in objPatChart.PatChartList where el.Date_of_Service.Date != DateTime.MinValue select el;

                ClientSession.PatientPaneList = EncList.ToList<PatientPane>();
                //if (ClientSession.PatientPaneList.Count > 0)
                //{
                //UIManager.Birth_Date_For_Social = PatientPaneDetails[0].Birth_Date;
                // }
                //FillPatientPane();
                //IList<string> problemListCodesWithParentCodes = new List<string>();
                //problemListCodesWithParentCodes.Clear();
                //var distinct = from h in objPatChart.PblmListParentICD group h by new { h } into g select new { g.Key.h };

                //foreach (var code in distinct)
                //{
                //    var duplicate = (from dup in problemListCodesWithParentCodes
                //                     where dup.Contains(code.h)
                //                     select dup);
                //    if (duplicate.Count() == 0)
                //    {
                //        if (code.h != string.Empty)
                //        {
                //            problemListCodesWithParentCodes.Add(code.h);
                //        }
                //    }
                //    else
                //    {
                //        if (code.h.Split('!')[0] != duplicate.First().Split('!')[0])
                //        {
                //            problemListCodesWithParentCodes.Add(code.h);
                //        }
                //    }
                //}
                //trvPatinetChart.ExpandAllNodes();

                //ClientSession.ProblemListCodesWithParentCodes = problemListCodesWithParentCodes;
                RadModalWindow.Visible = false;
                //Changed PHYSICIAN_CORRECTION to CODER_REVIEW_CORRECTION
                if (ClientSession.UserCurrentProcess.ToString().ToUpper() == "CODER_REVIEW_CORRECTION")
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenReviewCoding", "OpenReviewCodingException();", true);
                    ////RadModalWindow.Visible = true;
                    ////RadModalWindow.Title = "Exception";                    
                    ////RadModalWindow.NavigateUrl = "frmException.aspx?formName=" + "Feedback for Coding Exception";
                    ////RadModalWindow.VisibleTitlebar = true;
                    ////RadModalWindow.VisibleStatusbar = false;
                    ////RadModalWindow.VisibleOnPageLoad = true;
                    ////RadModalWindow.Height = 750;
                    ////RadModalWindow.Width = 900;


                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "FeedbackCodingException(" + hdnAddendumID.Value + ");", true);
                }
                //Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                if (ClientSession.UserCurrentProcess.ToString().ToUpper() == "PROVIDER_REVIEW_2")
                {
                    string sHumanName = string.Empty;
                    Human objhuman = new Human();
                    HumanManager objHuamManager = new HumanManager();
                    if (ClientSession.HumanId != null)
                        objhuman = objHuamManager.GetById(ClientSession.HumanId);
                    if (objhuman != null && objhuman.Id != 0)
                        sHumanName = objhuman.Last_Name + "," + objhuman.First_Name + " " + objhuman.MI + " " + objhuman.Suffix;

                    RadModalWindow.Visible = true;
                    RadModalWindow.VisibleOnPageLoad = true;
                    RadModalWindow.Height = Unit.Pixel(730);
                    RadModalWindow.Width = Unit.Pixel(855);
                    RadModalWindow.VisibleStatusbar = false;
                    RadModalWindow.KeepInScreenBounds = true;
                    RadModalWindow.ReloadOnShow = true;
                    //RadModalWindow.OnClientClose = "ClosePrintDocuments";
                    //ViewState["Chkoutbtn"] = false;
                    ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    RadModalWindow.NavigateUrl = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' +
                            "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + "&OPENING_FROM=ProcessEncounter" +
                             "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "true";
                }
                if (ClientSession.UserCurrentProcess.ToString().ToUpper() == "PROVIDER_REVIEW_CORRECTION" || ClientSession.UserCurrentProcess.ToString().ToUpper() == "SCRIBE_CORRECTION" || ClientSession.UserCurrentProcess.ToString().ToUpper() == "SCRIBE_REVIEW_CORRECTION")
                {
                    string sHumanName = string.Empty;
                    Human objhuman = new Human();
                    HumanManager objHuamManager = new HumanManager();
                    if (ClientSession.HumanId != null)
                        objhuman = objHuamManager.GetById(ClientSession.HumanId);
                    if (objhuman != null && objhuman.Id != 0)
                        sHumanName = objhuman.Last_Name + "," + objhuman.First_Name + " " + objhuman.MI + " " + objhuman.Suffix;

                    string url = "";
                    if (ClientSession.UserCurrentProcess.ToString().ToUpper() == "SCRIBE_CORRECTION" || ClientSession.UserCurrentProcess.ToString().ToUpper() == "SCRIBE_REVIEW_CORRECTION")
                    {
                        url = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' +
                                  "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + "&OPENING_FROM=ProcessEncounter" +
                                   "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "true" + "&Scribe=" + "true";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ReviewCorrectionScreen", "OpenPrintDocReviewCorrection('" + url + "');", true);
                    }
                    else
                    {
                        url = "frmPrintDocuments.aspx?EID=" + ClientSession.EncounterId + "&SPHYID=" + '0' +
                             "&HID=" + ClientSession.HumanId + "&UNAME=" + ClientSession.UserName + "&BUTTONNAME=" + "&OPENING_FROM=ProcessEncounter" +
                              "&HNAME=" + sHumanName + "&EPROID=" + ClientSession.PhysicianId + "&MTR=" + "false" + "&DS=" + "true";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ReviewCorrectionScreen", "OpenPrintDocReviewCorrection('" + url + "');", true);


                    }
                }
                //Old Code
                //hdnFacilityRole.Value = ClientSession.FacilityName + "&" + ClientSession.UserRole + "&" + ClientSession.UserName;
                //Gitlab# 2485 - Physician Name Display Change
                hdnFacilityRole.Value = ClientSession.FacilityName + "&" + ClientSession.UserRole + "&" + ClientSession.UserName+ "&" + ClientSession.PhysicianId;
                //if (ClientSession.UserCurrentProcess.ToString().ToUpper() == "REVIEW_CODING")
                //{
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenCreateCoding", "OpenCreateCodingException();", true);
                // }
                if (ClientSession.UserCurrentProcess != null && ClientSession.UserCurrentProcess.ToString().ToUpper() == "REVIEW_CODING_2")
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenCreateCoding", "OpenCreateCodingException();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenCreateCodingException();", true);
                }
                //for BugID:38984- to open task with patientchart as background.
                if (Request["Openform"] != null && Request["Openform"] == "Tasks")
                {
                    RadModalWindow.Visible = true;
                    RadModalWindow.NavigateUrl = "frmPatientCommunication.aspx?AccountNum=" + Request["AccountNum"] + "&parentscreen=" + "MyQ" + "&MessageID=" + Request["MessageID"];
                    RadModalWindow.VisibleTitlebar = true;
                    RadModalWindow.VisibleStatusbar = false;
                    RadModalWindow.VisibleOnPageLoad = true;
                    RadModalWindow.Behaviors = WindowBehaviors.Close;
                    RadModalWindow.Height = 810;
                    RadModalWindow.Width = 1050;
                }
                if (Request["fromMyOrderQ"] != null && Request["fromMyOrderQ"].ToUpper() == "TRUE")//BugID:42368
                {
                    string Curr_process = Request["CurrentProcess"];
                    string OrderType = Request["OrderType"];
                    string HumanID = Request["HumanID"];
                    string EncounterID = Request["EncounterID"];
                    string PhysicianId = Request["PhysicianId"];
                    string OrderSubmitId = Request["OrderSubmitId"];
                    string ObjType = Request["ObjType"];
                    string ResultMasterID = Request["ResultMasterID"];
                    string LabId = Request["LabId"];


                    if (Curr_process == "MA_REVIEW" && (OrderType == "DIAGNOSTIC ORDER" || OrderType == "IMAGE ORDER"))
                    {
                        RadModalWindow.Visible = true;
                        if (ClientSession.UserName != null)
                            RadModalWindow.Title = OrderType + " - " + ClientSession.UserName;
                        RadModalWindow.NavigateUrl = "frmOrdersList.aspx?HumanID=" + HumanID + "&EncounterID=" + EncounterID + "&PhysicianId=" + PhysicianId + "&OrderSubmitId=" + OrderSubmitId + "&ScreenMode=MyQ";
                        RadModalWindow.VisibleTitlebar = true;
                        RadModalWindow.VisibleStatusbar = false;
                        RadModalWindow.VisibleOnPageLoad = true;
                        RadModalWindow.Behaviors = WindowBehaviors.Close;
                        RadModalWindow.Height = 665;
                        RadModalWindow.Width = 1030;
                    }
                    else if (OrderType == "IMMUNIZATION ORDER")
                    {
                        RadModalWindow.Visible = true;
                        if (ClientSession.UserName != null)
                            RadModalWindow.Title = OrderType + " - " + ClientSession.UserName;
                        RadModalWindow.NavigateUrl = "frmImmunization.aspx?HumanID=" + HumanID + "&EncounterID=" + EncounterID + "&PhysicianId=" + PhysicianId + "&OrderSubmitId=" + OrderSubmitId + "&LabId=" + LabId + "&Screen=MyQ";
                        RadModalWindow.VisibleTitlebar = true;
                        RadModalWindow.VisibleStatusbar = false;
                        RadModalWindow.VisibleOnPageLoad = true;
                        RadModalWindow.Behaviors = WindowBehaviors.Close;
                        RadModalWindow.Height = 800;
                        RadModalWindow.Width = 1010;
                    }
                    else if (Curr_process == "MA_REVIEW" && OrderType == "REFERRAL ORDER")
                    {
                        RadModalWindow.Visible = true;
                        if (ClientSession.UserName != null)
                            RadModalWindow.Title = OrderType + " - " + ClientSession.UserName;
                        RadModalWindow.NavigateUrl = "frmReferralOrder.aspx?HumanID=" + HumanID + "&EncounterID=" + EncounterID + "&PhysicianId=" + PhysicianId + "&OrderSubmitId=" + OrderSubmitId + "&ScreenMode=Myqueue";
                        RadModalWindow.VisibleTitlebar = true;
                        RadModalWindow.VisibleStatusbar = false;
                        RadModalWindow.VisibleOnPageLoad = true;
                        RadModalWindow.Behaviors = WindowBehaviors.Close;
                        RadModalWindow.Height = 750;
                        RadModalWindow.Width = 1000;
                    }
                    else if (Curr_process == "PHYSICIAN_VERIFY" && OrderType == "REFERRAL ORDER")
                    {
                        RadModalWindow.Visible = true;
                        if (ClientSession.UserName != null)
                            RadModalWindow.Title = OrderType + " - " + ClientSession.UserName;
                        RadModalWindow.NavigateUrl = "frmReferralOrder.aspx?HumanID=" + HumanID + "&EncounterID=" + EncounterID + "&PhysicianId=" + PhysicianId + "&OrderSubmitId=" + OrderSubmitId + "&ScreenMode=Myqueue";
                        RadModalWindow.VisibleTitlebar = true;
                        RadModalWindow.VisibleStatusbar = false;
                        RadModalWindow.VisibleOnPageLoad = true;
                        RadModalWindow.Behaviors = WindowBehaviors.Close;
                        RadModalWindow.Height = 830;
                        RadModalWindow.Width = 1000;
                    }
                }
            }
            //bFormLoad = false;

            //if (sMyObjType == "ADDENDUM")
            //{
            //    frmAddendum frmAddendumObj = new frmAddendum(objPatChart, ulMyHumanID, ulMySelectedEncID, ulMySelectedPhyID);
            //    frmAddendumObj.Show();
            //}
            ArrayList ary = new ArrayList();
            if (objPatChart != null && objPatChart.Fill_Encounter_and_WFObject != null)
            {
                ary.Add(objPatChart.Fill_Encounter_and_WFObject.EncounterWFRecord.Current_Owner);
                ary.Add(objPatChart.Fill_Encounter_and_WFObject.DocumentationWFRecord.Current_Owner);
                // ary.Add(objPatChart.Fill_Encounter_and_WFObject.DocReviewWFRecord.Current_Owner);
                ary.Add(objPatChart.Fill_Encounter_and_WFObject.AddendumWFRecord.Current_Owner);
            }
            ClientSession.UserCurrentList = ary;
            FillPatientPane();

            if (IsPostBack)
            {
                if (Request["ScreenName"] != null && Request["ScreenName"].ToUpper() == "PHONEENCOUNTER")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenPhoneEncounter", "OpenPhoneEncounter();", true);
                }
            }


            //IList<string> problemListCodesWithParentCodes = new List<string>();
            //trvPatinetChart.ExpandAllNodes();

            // ClientSession.ProblemListCodesWithParentCodes = problemListCodesWithParentCodes;
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            
            if (Request["OpenACO"] != null)
            {
                string value = string.Empty;
                if (ClientSession.FillPatientChart != null)
                    value = ClientSession.FillPatientChart.Notification.Replace("\r\n", "$");
                RadModalWindow.Visible = true;
                RadModalWindow.Title = "ACO Validation";
                RadModalWindow.NavigateUrl = "frmACOValidation.aspx?HumanID=" + hdnHumanNo.Value.ToString();
                RadModalWindow.VisibleTitlebar = true;
                RadModalWindow.VisibleStatusbar = false;
                RadModalWindow.VisibleOnPageLoad = true;
                RadModalWindow.Height = 150;
                RadModalWindow.Width = 550;
            }
            //else if (Request["ScreenMode"] == null && ClientSession.UserCurrentProcess.Trim() != string.Empty && ClientSession.UserCurrentProcess != "CODER_REVIEW_CORRECTION" && ClientSession.FillPatientChart.Notification.Trim() != string.Empty && !IsPostBack && ClientSession.FacilityName.ToUpper() != ConfigurationSettings.AppSettings["CMGFacilityName"].ToString().ToUpper())
            else if (Request["ScreenMode"] == null && ClientSession.UserCurrentProcess.Trim() != string.Empty && ClientSession.UserCurrentProcess != "CODER_REVIEW_CORRECTION" && ClientSession.FillPatientChart.Notification.Trim() != string.Empty && !IsPostBack && ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary != "Y")
            {
                // string value = ClientSession.FillPatientChart.Notification.Replace("\r\n", "$");
                if (refreshTabName == string.Empty)
                {
                    RadWindowNotification.Visible = true;
                    RadWindowNotification.Title = "Notification";
                    //RadModalWindow.NavigateUrl = "frmNotification.aspx?Message=" + value;
                    RadWindowNotification.NavigateUrl = "frmNotification.aspx";
                    RadWindowNotification.VisibleTitlebar = true;
                    RadWindowNotification.VisibleStatusbar = false;
                    RadWindowNotification.VisibleOnPageLoad = true;
                    RadWindowNotification.Height = 250;
                    RadWindowNotification.Width = 600;
                }

            }
            if (Request["isOpenAddendum"] != null && Request["isOpenAddendum"].ToString() == "true")
            {
                ulong curAddendum = 0;
                if (Request["isOpenAddendum"] != null && Request["isOpenAddendum"].ToString() != string.Empty)
                    curAddendum = Convert.ToUInt32(Request["currentAddendumId"].ToString());
                //ClientSession.currAddendumID = curAddendum;//For Bug Id 54759
                if (ClientSession.UserCurrentProcess != null && ClientSession.UserCurrentProcess.ToString().ToUpper() == "ADDENDUM_CORRECTION")
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenReviewCoding", "OpenReviewCodingException();", true);
                    //RadModalWindow.Visible = true;
                    //RadModalWindow.Title = "Exception";                    
                    //RadModalWindow.NavigateUrl = "frmException.aspx?formName=" + "Feedback for Coding Exception";
                    //RadModalWindow.VisibleTitlebar = true;
                    //RadModalWindow.VisibleStatusbar = false;
                    //RadModalWindow.VisibleOnPageLoad = true;
                    //RadModalWindow.Height = 750;
                    //RadModalWindow.Width = 900;                   
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "FeedbackCodingException(" + curAddendum + ");", true);
                    //ScriptManager.RegisterStartupScript(thmenuis, this.GetType(), string.Empty, "FeedbackCodingException();", true);//For Bug Id 54759
                }
                else
                {
                    RadModalWindow.NavigateUrl = "frmAddendum.aspx?currentAddendumId=" + curAddendum.ToString();
                    RadModalWindow.Visible = true;
                    RadModalWindow.VisibleOnPageLoad = true;
                    RadModalWindow.Width = 1140;
                    RadModalWindow.Height = 500;
                    if (ClientSession.UserRole != null && ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                    {
                        RadModalWindow.Height = 520;
                    }
                }
                //if (ClientSession.UserCurrentProcess.ToUpper() == "ADDENDUM_CORRECTION")
                //    RadModalWindow.Height = 800;
            }
            if (Request["Notification_Type"] != null && Request["Notification_Type"].ToString().ToUpper() == "ALL")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "RefreshNotification('All');", true);
            }
            //else
            //{ 
            //  //This part added in the CC page load
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "RefreshNotification('Notify');", true);
            //}

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Load", " StopLoadFromPatChart();ToolStripAlertHidexml();", true);
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - PageLoad : End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart : End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");

        }
        //protected void trvPatinetChart_SelectedNodeChanged(object sender, EventArgs e)
        //{
        //    EncounterContainer.Attributes["src"] = "frmEncounter.aspx?Date=" + hdnLocalTime.Value + "&currentAddendumId=" + hdnAddendumID.Value;//?EncounterID=" + ClientSession.EncounterId.ToString() + "&HumanID=" + ClientSession.HumanId.ToString() + "&ulMyPhysicianID=" + ClientSession.PhysicianId.ToString() + "&SelectedEncounterID=" +ClientSession.Selectedencounterid.ToString();
        //}

        public static int selectedEncounterNode = -1;
        public bool Right_click = false;

        /*
        void SetFloatingSummary()
        {
            ChiefComplaintsManager objChiefComplaintsManager = new ChiefComplaintsManager();

            var Isupdated = false;
            if (ClientSession.FillEncounterandWFObject.EncRecord.Appointment_Provider_ID ==
                ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID)
            {
                Isupdated = objChiefComplaintsManager.SetIsCopyPreviousEncounter(ClientSession.EncounterId, ClientSession.HumanId,
                     ClientSession.PhysicianId, string.Empty);
            }

            else if (ClientSession.FillEncounterandWFObject.EncRecord.Appointment_Provider_ID !=
                ClientSession.FillEncounterandWFObject.EncRecord.Encounter_Provider_ID)
            {
                if (UIManager.select_physician_id != 0)
                    Isupdated = objChiefComplaintsManager.SetIsCopyPreviousEncounter(ClientSession.EncounterId, ClientSession.HumanId,
                           UIManager.select_physician_id, string.Empty);
            }            
        }*/
        //[WebMethod(EnableSession = true)]
        //public static string insertlog(string HumanID, string Log, string EncounterID)
        //{
        //    WorkFlowManager objWF = new WorkFlowManager();
        //    UtilityManager.inserttologgingtable(EncounterID, HumanID, ClientSession.UserName, ClientSession.PhysicianId.ToString(), Log);
        //    return "Success";
        //}

        [WebMethod(EnableSession = true)]
        public static string RegenerateXMLbyTalend(string id, string XMLType)
        {
            string result = UtilityManager.GenerateXML(id, XMLType);
            return result;
        }

        FillEncounterandWFObject FillClientSession(ulong EncounterID, bool IsLoadFromBD)
        {
            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - FillClientSession [To load encounter data into Session Memory] : Start", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            FillEncounterandWFObject fillEncWFObj = null;
            //var ddfdf = UIManager.ULPreviousEnc;
            // if (IsLoadFromBD)
            //  fillEncWFObj = objEncounterManager.GetEncounterandWFObject(EncounterID, Convert.ToUInt32(hdnAddendumID.Value));//0);
            //else
            //{
            //FillPatientChart objPatChart = new FillPatientChart();;
            if (ClientSession.FillPatientChart != null)
                objPatChart = ClientSession.FillPatientChart;

            fillEncWFObj = objPatChart.Fill_Encounter_and_WFObject;

            if (fillEncWFObj == null)
                return fillEncWFObj;

            // }
            ClientSession.UserCurrentList.Clear();
            if (ClientSession.CurrentObjectType == "ENCOUNTER")
            {
                ClientSession.UserCurrentProcess = fillEncWFObj.EncounterWFRecord.Current_Process;
                ClientSession.UserCurrentOwner = fillEncWFObj.EncounterWFRecord.Current_Owner;
            }
            else if (ClientSession.CurrentObjectType == "DOCUMENTATION")
            {
                ClientSession.UserCurrentProcess = fillEncWFObj.DocumentationWFRecord.Current_Process;
                ClientSession.UserCurrentOwner = fillEncWFObj.DocumentationWFRecord.Current_Owner;
            }
            //else if (ClientSession.CurrentObjectType == "DOCUMENT REVIEW")
            //{
            //    ClientSession.UserCurrentProcess = fillEncWFObj.DocReviewWFRecord.Current_Process;
            //    ClientSession.UserCurrentOwner = fillEncWFObj.DocReviewWFRecord.Current_Owner;
            //}
            else if (ClientSession.CurrentObjectType == "ADDENDUM")
            {
                ClientSession.UserCurrentProcess = fillEncWFObj.AddendumWFRecord.Current_Process;
                ClientSession.UserCurrentOwner = fillEncWFObj.AddendumWFRecord.Current_Owner;
            }
            ClientSession.UserCurrentList.Add(fillEncWFObj.EncounterWFRecord.Current_Owner);
            ClientSession.UserCurrentList.Add(fillEncWFObj.DocumentationWFRecord.Current_Owner);
            // ClientSession.UserCurrentList.Add(fillEncWFObj.DocReviewWFRecord.Current_Owner);
            ClientSession.UserCurrentList.Add(fillEncWFObj.AddendumWFRecord.Current_Owner);
            // if (fillEncWFObj.EncRecord.Facility_Name.ToUpper() == ConfigurationSettings.AppSettings["CMGFacilityName"].ToString().ToUpper())
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == fillEncWFObj.EncRecord.Facility_Name select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
                Is_CMG = true;
            else
                Is_CMG = false;

            UtilityManager.inserttologgingtable(ClientSession.EncounterId.ToString(), ClientSession.HumanId.ToString(), ClientSession.UserName, ClientSession.PhysicianId.ToString(), "Patient Chart - FillClientSession  [To load encounter data into Session Memory] : End", DateTime.Now, sGroup_ID_Log, "frmPatientChart");
            return fillEncWFObj;
        }

        //Added by Srividhya
        void Page_PreInit(Object sender, EventArgs e)
        {


            if (Request.QueryString["ScreenName"] != null)
            {
                if (Request.QueryString["ScreenName"].ToString().ToUpper() == "CHARGEPOSTING")
                {
                    this.MasterPageFile = "~/DemoGraphicsEmpty.Master";
                }
            }


        }

        public void AddWindowItem(string sPage)
        {

            bool bPresent = false;
            int iLoop = 0;
            ArrayList windowLst = null;
            if (ClientSession.WindowList != null)
                windowLst = (ArrayList)ClientSession.WindowList;

            for (iLoop = 0; iLoop < windowLst.Count; iLoop++)
            {
                if (windowLst[iLoop].ToString().Contains('#'))
                {
                    if (sPage.Split('#')[1].Split('$')[0] == windowLst[iLoop].ToString().Split('#')[1].Split('$')[0] && sPage.Split('#')[1].Split('$')[0] != "0")
                    {
                        if (sPage.Split('~')[1].Split('#')[0] != windowLst[iLoop].ToString().Split('~')[1].Split('#')[0])
                        {
                            ClientSession.Selectedencounterid = 0;
                            sPage = sPage.Split('#')[0] + "#" + ClientSession.Selectedencounterid.ToString() + "$" + string.Empty + "^" + sPage.Split('^')[1];
                        }
                    }
                }
                if (windowLst[iLoop].ToString().Contains(sPage) || (windowLst[iLoop].ToString().Contains(sPage.Split('~')[1].Split('#')[0])))
                {
                    bPresent = true;
                }
            }
            if (bPresent == false)
            {
                windowLst.Add(sPage);
            }
            ClientSession.WindowList = windowLst;

        }
        public void hdnbtngeneratexml_Click(object sender, EventArgs e)
        {

            //  Patientchartload();
        }
    }

}

