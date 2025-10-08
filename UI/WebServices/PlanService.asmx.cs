using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Script.Serialization;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for PlanService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PlanService : System.Web.Services.WebService
    {

        #region PlanTab
        [WebMethod(EnableSession = true)]
        public string LoadPlanTab()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            SecurityServiceUtility SecurityService = new SecurityServiceUtility();
            IList<string> Plan_tab_to_disable = SecurityService.GetListTabtoDisable("frmPlanTab");
            string ClientSessionDetails = string.Empty;
            if (Plan_tab_to_disable != null && Plan_tab_to_disable.Count > 0)
            {
                var tabs_to_disable = Plan_tab_to_disable.Select(a => new
                {
                    tab = a
                });
                ClientSessionDetails = ClientSession.UserName + "&" + ClientSession.UserRole + "&" + ClientSession.UserCurrentProcess + "&" + UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date + '-' + Session.SessionID + "-Disable_Tab=" + JsonConvert.SerializeObject(tabs_to_disable);

            }
            else
            {
                ClientSessionDetails = ClientSession.UserName + "&" + ClientSession.UserRole + "&" + ClientSession.UserCurrentProcess + "&" + UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date + '-' + Session.SessionID + "-Disable_Tab=" + "";
            }

            return ClientSessionDetails;
        }
        #endregion

        #region GeneralPlan

        [WebMethod(EnableSession = true)]
        public string LoadPlan()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            TreatmentPlanManager treatmentPlanMngr = new TreatmentPlanManager();
            IList<TreatmentPlan> Treatment_Plan_List = new List<TreatmentPlan>();
            IList<Documents> DocumentList = new List<Documents>();
            IList<InHouseProcedure> InProcList = new List<InHouseProcedure>();
            Encounter EncObj = new Encounter();
            EncObj = ClientSession.FillEncounterandWFObject.EncRecord;
            string strPlan = string.Empty;
            string BPStatusValue = String.Empty;
            HttpContext.Current.Session["DocumentList"] = null;
            //BugID:47887
            string Return_IN_Value = String.Empty;
            FillTreatmentPlan objFillTrtmntPlan = treatmentPlanMngr.GetTreatmentPlanUsingWithEncounterId(ClientSession.EncounterId, ClientSession.HumanId, ClientSession.UserName, out BPStatusValue);
            //if (EncObj != null && EncObj.Return_In_Months == 0 && EncObj.Return_In_Weeks == 0 && EncObj.Return_In_Days == 0)
            //{
            //    if (BPStatusValue != String.Empty)
            //    {
            //        string strFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            //        if (File.Exists(strFilePath) == true)
            //        {
            //            XmlDocument xmldoc = new XmlDocument();
            //            XmlTextReader xmltext = new XmlTextReader(strFilePath);
            //            xmldoc.Load(xmltext);
            //            XmlNodeList xmlNodelst = xmldoc.GetElementsByTagName("BpStatusBasedFollowUpValueList");
            //            if (xmlNodelst.Count > 0)
            //            {
            //                if (xmlNodelst.Count > 0 && xmlNodelst[0].ChildNodes.Count > 0)
            //                {
            //                    for (int i = 0; i < xmlNodelst[0].ChildNodes.Count; i++)
            //                    {
            //                        if (xmlNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Field_Name").Value == BPStatusValue)
            //                        {
            //                            if (xmlNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Type").Value.IndexOf("Months") != -1)
            //                            {
            //                                EncObj.Return_In_Months = Convert.ToInt32(xmlNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Value").Value);
            //                            }
            //                            else if (xmlNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Type").Value.IndexOf("Weeks") != -1)
            //                            {
            //                                EncObj.Return_In_Weeks = Convert.ToInt32(xmlNodelst[0].ChildNodes[i].Attributes.GetNamedItem("Value").Value);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            HttpContext.Current.Session["TreatmentPlan"] = objFillTrtmntPlan.Treatment_Plan_List;
            HttpContext.Current.Session["DocumentList"] = objFillTrtmntPlan.FillDocumentList.DocumentsList;
           // HttpContext.Current.Session["ReturnINValue"] = Return_IN_Value;

            Treatment_Plan_List = objFillTrtmntPlan.Treatment_Plan_List;
            DocumentList = objFillTrtmntPlan.FillDocumentList.DocumentsList;
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            #region TreatmentPlan Get

            //Jira CAP-1163 - Old Code
            //IList<string> ilstplanTagList = new List<string>();
            //ilstplanTagList.Add("TreatmentPlanList");



            //IList<object> ilstplanlobFinal = new List<object>();
            //ilstplanlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstplanTagList);

            //if (ilstplanlobFinal != null && ilstplanlobFinal.Count > 0)
            //{
            //    if (ilstplanlobFinal[0] != null)
            //    {
            //        for (int iCount = 0; iCount < ((IList<object>)ilstplanlobFinal[0]).Count; iCount++)
            //        {
            //            objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstplanlobFinal[0])[iCount]);
            //        }
            //    }
            //}

            //Jira CAP-1163 - New Code
            TreatmentPlanManager mngrTreatmentPlan = new TreatmentPlanManager();
            objTreatmentPlan = mngrTreatmentPlan.GetTreatmentPlan(ClientSession.EncounterId);

            //    string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == ClientSession.EncounterId)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
            //                        TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        TreatmentPlan = (TreatmentPlan)TreatmentPlan;
            //                        propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {

            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(TreatmentPlan, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }

            //                        }
            //                        objTreatmentPlan.Add(TreatmentPlan);
            //                    }
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            #endregion

            var TreatmenPlanList = objTreatmentPlan.Select(a => new
            {
                Plan_Type = a.Plan_Type,
                Plan = a.Plan,
                Plan_For_Plan = a.Plan_For_Plan,
                Plan_ID = a.Id,
                Plan_Ref = a.Plan_Reference,
                plan_followup_snomed = a.Followup_Plan_Snomed
            }).Where(a => a.Plan_Type != "PLAN" && a.Plan.Replace("*", "").Trim() != string.Empty).OrderBy(a => a.Plan_Type).OrderByDescending(a => a.Plan_Ref);

            //Jira CAP-1163
            //IList<TreatmentPlan> tpPlan = objTreatmentPlan.Where(a => a.Plan_Type == "PLAN").ToList<TreatmentPlan>();
            IList<TreatmentPlan> tpPlan = objTreatmentPlan.Where(a => a.Plan_Type == "PLAN" && a.Amendment_Type == "" && a.Corrections_to_be_made == "").ToList<TreatmentPlan>();
            if (tpPlan != null && tpPlan.Count > 0)
            {
                for (int iCount = 0; iCount < tpPlan.Count; iCount++)
                {
                    if (iCount != 0)
                    {
                        strPlan += Environment.NewLine + Environment.NewLine;
                    }

                    if (tpPlan[iCount].Plan != string.Empty)
                    {
                        strPlan += tpPlan[iCount].Plan.Replace("&#xD;&#xA;", "\n");
                        if (strPlan.EndsWith(Environment.NewLine))
                        {
                            strPlan = strPlan.TrimEnd('\r', '\n');
                        }
                        if (!strPlan.Trim().EndsWith("*"))
                        {
                            strPlan += Environment.NewLine + "* ";
                        }
                    }
                    else
                    {
                        strPlan += "* ";
                    }

                }
            }
            else
            {
                strPlan += "* ";
            }


            string sTemp = string.Empty;
            string UserCurrentProcess = string.Empty;
            if (ClientSession.UserCurrentProcess == "PROVIDER_PROCESS")
            {
                sTemp = "Electronically Signed by" + ClientSession.SummaryList.Split('|')[4].ToString();
            }
            else if (ClientSession.UserCurrentProcess == "PROVIDER_REVIEW")
            {
                //Jira CAP-1451 - Old Code
                //sTemp = "I" + ClientSession.SummaryList.Split('|')[4].ToString() + " have reviewed the chart and agree with the management plan with the changes to the plan as indicated."
                //         + "Electronically Signed by" + ClientSession.SummaryList.Split('|')[4].ToString();
                //Jira CAP-1451 - New Code
                sTemp = "Electronically Signed by" + ClientSession.SummaryList.Split('|')[4].ToString();
            }
            else
            {
                if (ClientSession.SummaryList.Split('|').Count() >= 4)
                {
                    sTemp = "Electronically Signed by" + ClientSession.SummaryList.Split('|')[4].ToString();
                }
            }
            StaticLookupManager objstatic = new StaticLookupManager();
            IList<StaticLookup> lststatic = new List<StaticLookup>();
            lststatic = objstatic.getStaticLookupByFieldName("FOLLOWUP FOR BMI");
            //var DefaultList = lststatic.Select(a => new
            //{
            //   Default_Value=a.Value
            //}).Where(a => a.Default_Value == "Y");

            var DefaultList = String.Join(", ", (from m in lststatic where m.Default_Value == "Y" select m.Value).ToArray());

            string JPlan = new JavaScriptSerializer().Serialize(TreatmenPlanList);
            var result = new { data = DocumentList, EncRec = EncObj, signtxt = sTemp, PlanOthers = JPlan, Plan = strPlan, defualt_Value = DefaultList };
            return JsonConvert.SerializeObject(result);
        }
        [WebMethod(EnableSession = true)]
        public string SavePlan(object[] data, object[] PlanfromOthers)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "";
            }
            //Jira CAP-3455
            ////Jira CAP-2830
            //string sIsAkidoEncounter = "false";
            //string sExMessage = "";
            //string sStatus = "";
            //if (objectDate["ElectronicDeclrChecked"].ToString() == "true")
            //{
            //    sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(ClientSession.EncounterId.ToString(), out sExMessage, out sStatus);
            //    if (sIsAkidoEncounter == "true")
            //    {
            //        var Validationresult = new { ValidationMessage = "110812" };
            //        return JsonConvert.SerializeObject(Validationresult);
            //    }
            //    else if (sIsAkidoEncounter == "Exception")
            //    {
            //        var Validationresult = new { ValidationMessage = "110813_"+ DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt")+"_"+ sExMessage.Replace("'", "") };
            //        return JsonConvert.SerializeObject(Validationresult);
            //    }
            //}
            ////Jira CAP-2830 - End

            //Jira CAP-3455
            string sIsAkidoEncounter = "false";
            string sExMessage = "";
            string sStatus = "";
            string sIsSignedInCapella = "false";
            var objectDate = (Dictionary<string, object>)data.FirstOrDefault();
            sIsSignedInCapella = objectDate["ElectronicDeclrChecked"].ToString();

            if (sIsSignedInCapella == "true")
            {
                sIsAkidoEncounter = UtilityManager.IsAkidoEncounter(ClientSession.EncounterId.ToString(), out sExMessage, out sStatus);
                if (sIsAkidoEncounter == "true" || sIsAkidoEncounter == "Exception")
                {
                    if (data.FirstOrDefault() is Dictionary<string, object> dict)
                    {
                        dict["ElectronicDeclrChecked"] = "false";
                    }
                }
            }
            //Jira CAP-3455 - End
            UtilityManager objUtilityMngr = new UtilityManager();
            IList<TreatmentPlan> Treatment_Plan = ((IList<TreatmentPlan>)HttpContext.Current.Session["TreatmentPlan"]);
            IList<Documents> DocumentList = (IList<Documents>)HttpContext.Current.Session["DocumentList"];
            Encounter EncRecord = ClientSession.FillEncounterandWFObject.EncRecord;
            string SelectedDocItems = string.Empty;
            string strTreatmntPlan = string.Empty;
            DateTime dtMySignDateTime = DateTime.MinValue;
            IList<string> SelectedItems = new List<string>();
            IList<Documents> SaveDocList = new List<Documents>();
            IList<Documents> UpdateDoclist = new List<Documents>();
            IList<Documents> DeleteDoclist = new List<Documents>();
            IList<TreatmentPlan> SaveList = new List<TreatmentPlan>();
            IList<TreatmentPlan> UpdateList = new List<TreatmentPlan>();
            IList<TreatmentPlan> DeleteList = new List<TreatmentPlan>();
            TreatmentPlan Treatment = new TreatmentPlan();
            IList<TreatmentPlan> objTreatmentPlan = new List<TreatmentPlan>();
            DocumentManager DocumentMngr = new DocumentManager();
            Documents objDocument = new Documents();
            string strGiven_to = string.Empty;
            string strRelationship = string.Empty;

            foreach (object value in data)
            {
                Dictionary<string, object> dicValues = new Dictionary<string, object>();
                dicValues = (Dictionary<string, object>)value;
                EncRecord.Is_Document_Given = dicValues["Is_Document_Given"] == null ? string.Empty : dicValues["Is_Document_Given"].ToString();
                // DateTime dtDue_on = Convert.ToDateTime(dicValues["Due_On"]);
                if (dicValues["Due_On"] != null && !dicValues["Due_On"].Equals(" "))
                {

                    EncRecord.Due_On = Convert.ToDateTime(dicValues["Due_On"]);
                }
                else
                {
                    EncRecord.Due_On = DateTime.MinValue;
                }
                EncRecord.Return_In_Days = dicValues["ReturnDays"] == null ? 0 : Convert.ToInt32(dicValues["ReturnDays"]);
                EncRecord.Return_In_Weeks = dicValues["ReturnWeeks"] == null ? 0 : Convert.ToInt32(dicValues["ReturnWeeks"]);
                EncRecord.Return_In_Months = dicValues["ReturnMonths"] == null ? 0 : Convert.ToInt32(dicValues["ReturnMonths"]);
                //BugID:48018
                //if (Convert.ToInt32(dicValues["ReturnDays"]) == 0 && Convert.ToInt32(dicValues["ReturnWeeks"]) == 0 && Convert.ToInt32(dicValues["ReturnMonths"]) == 0 && dicValues["Due_On"].Equals(" ") && HttpContext.Current.Session["ReturnINValue"] != null)
                //{
                //    string[] Enc_returnIn = HttpContext.Current.Session["ReturnINValue"].ToString().Split(':');
                //    if (Enc_returnIn.Length > 0)
                //        if (Enc_returnIn[0].IndexOf("Month") != -1)
                //            EncRecord.Return_In_Months = Convert.ToInt32(Enc_returnIn[1]);
                //        else if (Enc_returnIn[0].IndexOf("Week") != -1)
                //            EncRecord.Return_In_Weeks = Convert.ToInt32(Enc_returnIn[1]);
                //}
                EncRecord.Follow_Reason_Notes = dicValues["FollowReasonNotes"] == null ? string.Empty : dicValues["FollowReasonNotes"].ToString();
                EncRecord.Is_PFSH_Verified = dicValues["PFSHVerified"] == null ? string.Empty : dicValues["PFSHVerified"].ToString();
                EncRecord.Is_PRN = dicValues["PRNChecked"] == null ? string.Empty : dicValues["PRNChecked"].ToString();
                EncRecord.Is_After_Studies = dicValues["AfterStudieschecked"] == null ? string.Empty : dicValues["AfterStudieschecked"].ToString();


                if (dicValues["ElectronicDeclrChecked"] != null && dicValues["ElectronicDeclrChecked"].ToString() == "true")
                {
                    //GitLab - #3974
                    //dtMySignDateTime = DateTime.Now;
                    //EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToUniversal(dtMySignDateTime);
                    EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToLocal(dtMySignDateTime);
                }
                if (dicValues["SurgeryDeclrChecked"] != null && dicValues["SurgeryDeclrChecked"].ToString() == "true")
                {
                    EncRecord.Proceed_with_Surgery_Planned = "Y";
                }
                else
                {
                    EncRecord.Proceed_with_Surgery_Planned = "N";
                }
                SelectedDocItems = dicValues["CheckedDocumnts"] == null ? string.Empty : dicValues["CheckedDocumnts"].ToString();
                if (SelectedDocItems != null && SelectedDocItems != "")
                {
                    for (int i = 0; i < SelectedDocItems.Split('-').Count(); i++)
                    {
                        SelectedItems.Add(SelectedDocItems.Split('-')[i].ToString());
                    }
                }
                strGiven_to = dicValues["Givento"] == null ? string.Empty : dicValues["Givento"].ToString();
                strRelationship = dicValues["Relationship"] == null ? string.Empty : dicValues["Relationship"].ToString();
                strTreatmntPlan = dicValues["TreatmntList"] == null ? string.Empty : dicValues["TreatmntList"].ToString();
                //IList<string> SelectedItems =
            }
            #region encounter
            EncRecord.Modified_By = ClientSession.UserName;
            EncRecord.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
            if (strRelationship == string.Empty)
            {
                if (EncRecord.Source_Of_Information == string.Empty)
                {
                    EncRecord.Source_Of_Information = "Self";
                }
            }
            else
            {
                EncRecord.Source_Of_Information = strRelationship;
            }

            if (EncRecord.Encounter_Provider_ID == 0)
            {
                EncRecord.Encounter_Provider_ID = Convert.ToInt32(ClientSession.PhysicianId);
            }
            EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            #endregion

            #region DocumentSave

            for (int i = 0; i < SelectedItems.Count; i++)
            {
                var doc = (from d in DocumentList where d.Document_Type.ToUpper() == SelectedItems[i].ToString().ToUpper() select d);
                objDocument = new Documents();
                if (doc.ToList<Documents>().Count > 0)
                {
                    objDocument = doc.ToList<Documents>()[0];
                    objDocument.Created_By = ClientSession.UserName;
                    objDocument.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objDocument.Given_Date = DateTime.Now;
                    objDocument.Document_Type = SelectedItems[i].ToString();
                    objDocument.Relationship = strRelationship;
                    objDocument.Given_To = strGiven_to;
                    UpdateDoclist.Add(objDocument);
                }
                else
                {
                    objDocument.Encounter_ID = ClientSession.EncounterId;
                    objDocument.Human_ID = ClientSession.HumanId;
                    objDocument.Physician_ID = ClientSession.PhysicianId;
                    objDocument.Created_By = ClientSession.UserName;
                    objDocument.Given_By = ClientSession.UserName;
                    objDocument.Given_Date = DateTime.Now;
                    objDocument.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objDocument.Document_Type = SelectedItems[i].ToString();
                    objDocument.Relationship = strRelationship;
                    objDocument.Given_To = strGiven_to;
                    SaveDocList.Add(objDocument);
                }
            }
            if (DocumentList != null && DocumentList.Count > 0)
            {
                for (int i = 0; i < DocumentList.Count; i++)
                {
                    bool bResult = false;
                    for (int j = 0; j < SelectedItems.Count; j++)
                    {
                        if (SelectedItems[j].ToString().ToUpper() == DocumentList[i].Document_Type.ToUpper())
                        {
                            bResult = true;
                            break;
                        }
                    }
                    if (!bResult)
                    {
                        DocumentList[i].Given_By = ClientSession.UserName;
                        DeleteDoclist.Add(DocumentList[i]);
                    }
                }
            }
            #endregion
            #region TreatmentPlan Get
            //Jira CAP-1163 - Old Code
            //IList<string> ilstplanTagList = new List<string>();
            //ilstplanTagList.Add("TreatmentPlanList");



            //IList<object> ilstplanlobFinal = new List<object>();
            //ilstplanlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstplanTagList);

            //if (ilstplanlobFinal != null && ilstplanlobFinal.Count > 0)
            //{
            //    if (ilstplanlobFinal[0] != null)
            //    {
            //        for (int iCount = 0; iCount < ((IList<object>)ilstplanlobFinal[0]).Count; iCount++)
            //        {
            //            objTreatmentPlan.Add((TreatmentPlan)((IList<object>)ilstplanlobFinal[0])[iCount]);
            //        }
            //    }
            //}


            //Jira CAP-1163 - New Code
            TreatmentPlanManager mngrTreatmentPlan = new TreatmentPlanManager();
            objTreatmentPlan = mngrTreatmentPlan.GetTreatmentPlan(ClientSession.EncounterId);


            //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("TreatmentPlanList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("TreatmentPlanList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == ClientSession.EncounterId)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(TreatmentPlan));
            //                        TreatmentPlan TreatmentPlan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as TreatmentPlan;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((TreatmentPlan)TreatmentPlan).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {

            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(TreatmentPlan, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(TreatmentPlan, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(TreatmentPlan, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(TreatmentPlan, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(TreatmentPlan, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }

            //                        }
            //                        objTreatmentPlan.Add(TreatmentPlan);
            //                    }
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            #endregion
            #region Treatment
            if (PlanfromOthers.Length > 0)
            {
                for (int i = 0; i < PlanfromOthers.Length; i++)
                {
                    TreatmentPlan objPlan = null;
                    Dictionary<string, object> PlanValues = new Dictionary<string, object>();
                    PlanValues = (Dictionary<string, object>)PlanfromOthers[i];
                    ulong ID = Convert.ToUInt64(PlanValues["Id"].ToString());
                    IList<TreatmentPlan> itemlst = objTreatmentPlan.Where(a => a.Id == ID).ToList<TreatmentPlan>();

                    if (itemlst != null && itemlst.Count > 0)
                        objPlan = itemlst[0];
                    if (objPlan != null)
                    {
                        objPlan.Id = ID;
                        objPlan.Plan = PlanValues["Plan"].ToString().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
                        objPlan.Plan_For_Plan = PlanValues["Plan_For_Plan"].ToString().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
                        if (objPlan.Plan_For_Plan.Trim() != string.Empty)
                            objPlan.Followup_Plan_Snomed = objUtilityMngr.GetSnomedfromStaticLookup("FollowupList", (objPlan.Plan.IndexOf("BMI") > -1 ? "BMI" : ""), objPlan.Plan_For_Plan);
                        if (objPlan.Followup_Plan_Snomed == "")
                            objPlan.Followup_Plan_Snomed = objUtilityMngr.GetSnomedfromStaticLookup("FollowupReasonnotperformedList", (objPlan.Plan.IndexOf("BMI") > -1 ? "FOLLOWUP_REASON_NOT_PERFORMED FOR BMI" : ""), objPlan.Plan_For_Plan);
                        objPlan.Modified_By = ClientSession.UserName;
                        objPlan.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateList.Add(objPlan);
                    }
                }
            }
            string sHeaderText = string.Empty;
            string sText = string.Empty;
            string sLocalText = string.Empty;
            strTreatmntPlan = strTreatmntPlan.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
            strTreatmntPlan = strTreatmntPlan.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");//BugID:43718,51650 
            //Jira CAP-1163
            //IList<TreatmentPlan> PlanDatainPlan = objTreatmentPlan.Where(a => a.Plan_Type == "PLAN").ToList<TreatmentPlan>();
            IList<TreatmentPlan> PlanDatainPlan = objTreatmentPlan.Where(a => a.Plan_Type == "PLAN" && a.Amendment_Type == "" && a.Corrections_to_be_made == "").ToList<TreatmentPlan>();
            //Jira CAP-1163 - Old Code
            //if (PlanDatainPlan != null && PlanDatainPlan.Count > 0)
            //{
            //    TreatmentPlan objPlan = PlanDatainPlan[0];
            //    objPlan.Plan = strTreatmntPlan;
            //    objPlan.Modified_By = ClientSession.UserName;
            //    objPlan.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            //    UpdateList.Add(objPlan);
            //}
            //else
            //{
            //Jira CAP-1163 - New Code
            //Delete TreatmentPlan
            if (PlanDatainPlan != null && PlanDatainPlan.Count > 0)
            {
                DeleteList = PlanDatainPlan;
            }
            //Insert TreatmentPlan
            TreatmentPlan objPlannew = new TreatmentPlan();
            objPlannew.Human_ID = ClientSession.HumanId;
            objPlannew.Encounter_Id = ClientSession.EncounterId;
            objPlannew.Physician_Id = ClientSession.PhysicianId;
            objPlannew.Created_By = ClientSession.UserName;
            objPlannew.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            objPlannew.Local_Time = UtilityManager.ConvertToLocal(objPlannew.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
            objPlannew.Plan_Type = "PLAN";
            string plan_txt = string.Empty;
            plan_txt = strTreatmntPlan;
            objPlannew.Plan = plan_txt;
            objPlannew.Version = 0;
            objPlannew.Source_ID = 0;
            SaveList.Add(objPlannew);
            //}

            #endregion
            FillTreatmentPlan objPlanAfterSave = DocumentMngr.SaveDocumentsandPlanList(EncRecord, SaveDocList.ToArray<Documents>(), UpdateDoclist.ToArray<Documents>(), DeleteDoclist.ToArray<Documents>(), SaveList.ToArray<TreatmentPlan>(), UpdateList.ToArray<TreatmentPlan>(), DeleteList.ToArray<TreatmentPlan>(), ClientSession.EncounterId, ClientSession.UserName, string.Empty);
            HttpContext.Current.Session["TreatmentPlan"] = objPlanAfterSave.Treatment_Plan_List;
            HttpContext.Current.Session["DocumentList"] = objPlanAfterSave.FillDocumentList.DocumentsList;
            ClientSession.FillEncounterandWFObject.EncRecord = objPlanAfterSave.FillDocumentList.EncounterObj;
            //BugID:48018,48017
            Encounter EncObj = new Encounter();
            EncObj = ClientSession.FillEncounterandWFObject.EncRecord;
            //Jira CAP-3455
            if (sIsSignedInCapella == "true")
            {
                if (sIsAkidoEncounter == "true")
                {
                    var Validationresult = new { ValidationMessage = "110812" };
                    return JsonConvert.SerializeObject(Validationresult);
                }
                else if (sIsAkidoEncounter == "Exception")
                {
                    var Validationresult = new { ValidationMessage = "110813_" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + "_" + sExMessage.Replace("'", "") };
                    return JsonConvert.SerializeObject(Validationresult);
                }
            }
            //Jira CAP-3455 - End
            var result = new { EncRecord = EncObj };
            return JsonConvert.SerializeObject(result);
        }
        [WebMethod(EnableSession = true)]
        public string PrintDocument(string[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string screen = string.Empty;
            string DownloadDoc = string.Empty;
            string summary = string.Empty;
            bool Progress = false;
            bool Care = false;
            bool WellnessNotes = false;
            IList<string> SelectedItems = new List<string>();
            string Checked_Docs = string.Empty;
            string strGiven = string.Empty;
            string strRelationship = string.Empty;
            string strIsDocumentGiven = string.Empty;
            string filesNotFound = string.Empty;
            IList<Documents> DocumentList = (IList<Documents>)HttpContext.Current.Session["DocumentList"];
            string strXMlPath = string.Empty;
            string strPrintFilePath = string.Empty;
            string strfilename = string.Empty;
            string strselectedItem = string.Empty;
            string strSessionId = string.Empty;
            if (data.Count() > 0)
            {
                Checked_Docs = data[0].ToString();
                strGiven = data[1].ToString();
                strRelationship = data[2].ToString();
                strIsDocumentGiven = data[3].ToString();
                strSessionId = data[4].ToString();
            }

            for (int i = 0; i < Checked_Docs.Split(':').Count(); i++)
            {
                SelectedItems.Add(Checked_Docs.Split(':')[i].ToString());
            }

            Encounter EncRecord = new Encounter();
            EncRecord = ClientSession.FillEncounterandWFObject.EncRecord;

            string[] GetFiles = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath + "Documents\\Physician_Specific_Documents\\Patient Education\\");

            string[] Separator = new string[] { HostingEnvironment.ApplicationPhysicalPath + "Documents\\Physician_Specific_Documents\\Patient Education\\" };

            IList<string> FilesNotFound = new List<string>();


            foreach (string s in GetFiles)
            {
                string[] SplitedDocName = s.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

                FilesNotFound.Add(SplitedDocName[0].ToString());
            }
            for (int i = 0; i < SelectedItems.Count; i++)
            {

                if (SelectedItems[i].ToString().ToUpper().Contains("PROGRESS NOTE") == true || SelectedItems[i].ToString().ToUpper().Contains("CONSULTATION NOTE") == true)
                {
                    if (!Progress)
                    {

                        IList<string> lstNotes = SelectedItems.Where(a => a.ToUpper().Contains("PROGRESS NOTE") || a.ToUpper().Contains("CONSULTATION NOTE")).ToList<string>();

                        if (lstNotes.Count > 1)
                        {
                            screen = "Pro|Con";
                        }
                        else if (SelectedItems[i].ToString().ToUpper().Contains("PROGRESS NOTE"))
                        {
                            screen = "Pro";
                        }
                        else if (SelectedItems[i].ToString().ToUpper().Contains("CONSULTATION NOTE"))
                            screen = "Con";
                        Progress = true;

                    }
                }
                else if (SelectedItems[i].ToString().ToUpper().Contains("CARE NOTE") == true || SelectedItems[i].ToString().ToUpper().Contains("TREATMENT PLAN NOTE") == true)
                {
                    if (!Care)
                    {

                        IList<string> lstNotes = SelectedItems.Where(a => a.ToUpper().Contains("CARE NOTE") || a.ToUpper().Contains("TREATMENT PLAN NOTE")).ToList<string>();

                        if (lstNotes.Count > 1)
                        {
                            screen = "Care|Treat";
                        }
                        else if (SelectedItems[i].ToString().ToUpper().Contains("CARE NOTE"))
                        {
                            screen = "Care";
                        }
                        else if (SelectedItems[i].ToString().ToUpper().Contains("TREATMENT PLAN NOTE"))
                            screen = "Treat";
                        Care = true;

                    }
                }
                else if (SelectedItems[i].ToString().ToUpper().Contains("WELLNESS NOTE") == true)
                {
                    if (!WellnessNotes)
                    {
                        if (screen != null && screen != "")
                        {
                            screen = screen + "|Wellness";
                        }
                        else
                        {
                            screen = "Wellness";
                        }
                        WellnessNotes = true;
                    }
                }
                else if (SelectedItems[i].ToString().ToUpper().Contains("CLINICAL SUMMARY") == true)
                {
                    string sMyPath = string.Empty;
                    summary = "frmClinicalSummary";
                    //frmClinicalSummary objfrmClinical = new frmClinicalSummary();
                    //ArrayList FileLocation = objfrmClinical.PrintClinicalSummary(ClientSession.EncounterId, ClientSession.HumanId, false, ref sMyPath, string.Empty, true, false);

                    //string[] Split = new string[] { HostingEnvironment.ApplicationPhysicalPath + "Documents\\" + strSessionId };
                    //if (FileLocation[0].ToString().EndsWith(".xml") == true)
                    //{

                    //    if (strXMlPath == string.Empty)
                    //    {
                    //        string[] XMLFileName = FileLocation[0].ToString().Split(Split, StringSplitOptions.RemoveEmptyEntries);
                    //        if (strXMlPath == string.Empty)
                    //        {
                    //            strXMlPath = "Documents\\" + strSessionId.ToString() + XMLFileName[0].ToString();
                    //        }
                    //        if (strXMlPath != string.Empty)
                    //        {
                    //            DirectoryInfo ObjSearchDir = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath + strXMlPath);
                    //            if (!Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet").Exists)
                    //            {
                    //                Directory.CreateDirectory(ObjSearchDir.Parent.Parent.FullName + "\\stylesheet");
                    //            }
                    //            System.IO.File.Copy(HostingEnvironment.ApplicationPhysicalPath + "SampleXML/CDA.xsl", HostingEnvironment.ApplicationPhysicalPath + "Documents/" + strSessionId.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl", true);
                    //            strXMlPath = HostingEnvironment.ApplicationPhysicalPath + "Documents/" + strSessionId.ToString() + "/" + ObjSearchDir.Parent.Parent + "/stylesheet/CDA.xsl";
                    //        }

                    //    }
                    //    summary = strXMlPath;
                    //}


                }
                else if (SelectedItems[i].ToString().Split('|')[1].ToUpper().Contains(".DO"))
                {
                    if (DownloadDoc != null && DownloadDoc != "")
                    {
                        DownloadDoc = DownloadDoc + "|" + SelectedItems[i].ToString().Split('|')[1];
                    }
                    else
                    {
                        DownloadDoc = SelectedItems[i].ToString().Split('|')[1];
                    }
                }
                else
                {
                    if (!FilesNotFound.Any(a => a.ToString() == SelectedItems[i].Split('|')[1].ToString()))
                    {
                        if (SelectedItems[i].Split('|')[0].ToString() == "Recommended Material")
                        {
                            strfilename = "Recommended Material";
                            continue;
                        }

                        if (filesNotFound == string.Empty)
                        {
                            filesNotFound = SelectedItems[i].Split('|')[0].ToString();
                        }
                        else
                        {
                            filesNotFound += " , " + SelectedItems[i].Split('|')[0].ToString();
                        }

                        continue;
                    }
                    if (strselectedItem == string.Empty)
                    {
                        strselectedItem = SelectedItems[i].Split('|')[1].ToString().Replace(".pdf", "");
                    }
                    else
                    {
                        strselectedItem += "|" + SelectedItems[i].Split('|')[1].ToString().Replace(".pdf", "");
                    }
                }

            }
            IList<Documents> SaveList = new List<Documents>();
            IList<Documents> Updatelist = new List<Documents>();
            IList<Documents> Deletelist = new List<Documents>();
            Documents objDocument = null;

            for (int i = 0; i < SelectedItems.Count; i++)
            {
                objDocument = new Documents();
                var doc = (from d in DocumentList where d.Document_Type.ToUpper() == SelectedItems[i].Split('|')[0].ToString().ToUpper() select d);

                if (doc.ToList<Documents>().Count() > 0)
                {
                    objDocument = doc.ToList<Documents>()[0];
                    objDocument.Given_To = strGiven;
                    objDocument.Relationship = strRelationship;
                    objDocument.Given_By = ClientSession.UserName;
                    objDocument.Given_Date = UtilityManager.ConvertToUniversal();
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
                    objDocument.Relationship = strRelationship;
                    objDocument.Created_By = ClientSession.UserName;

                    //objDocument.Created_Date_And_Time = utc;
                    objDocument.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    objDocument.Document_Type = SelectedItems[i].Split('|')[0].ToString();
                    objDocument.Given_To = strGiven;
                    objDocument.Given_By = ClientSession.UserName;
                    objDocument.Given_Date = UtilityManager.ConvertToUniversal();
                    SaveList.Add(objDocument);
                }
            }
            if (DocumentList != null && DocumentList.Count > 0)
            {
                for (int i = 0; i < DocumentList.Count; i++)
                {
                    bool bResult = false;
                    for (int j = 0; j < SelectedItems.Count; j++)
                    {
                        if (SelectedItems[j].Split('|')[0].ToString().ToUpper() == DocumentList[i].Document_Type.ToUpper())
                        {
                            bResult = true;
                            break;
                        }

                    }
                    if (!bResult)
                    {
                        DocumentList[i].Given_By = ClientSession.UserName;
                        Deletelist.Add(DocumentList[i]);
                    }

                }
            }



            EncRecord.Is_Document_Given = strIsDocumentGiven;
            EncRecord.Modified_By = ClientSession.UserName;
            EncRecord.Modified_Date_and_Time = UtilityManager.ConvertToUniversal();
            EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            DocumentManager DocumentMngr = new DocumentManager();
            FillDocuments objFillDocuments = new FillDocuments();
            objFillDocuments = DocumentMngr.SaveUpdateDeleteDocument(SaveList.ToArray<Documents>(), Updatelist.ToArray<Documents>(), Deletelist.ToArray<Documents>(), EncRecord, ClientSession.EncounterId, string.Empty, true);
            HttpContext.Current.Session["TreatmentPlan"] = objFillDocuments.Treatment_Plan_List;
            HttpContext.Current.Session["DocumentList"] = objFillDocuments.DocumentsList;
            ClientSession.FillEncounterandWFObject.EncRecord = objFillDocuments.EncounterObj;
           
            
            string sFaxFirstname = string.Empty;
            string sFaxLastName = string.Empty;
            IList<Human> lsthuman = new List<Human>();
            IList<string> ilstGeneralPlanTagList = new List<string>();
            ilstGeneralPlanTagList.Add("HumanList");
            IList<object> ilstGeneralPlanBlobFinal = new List<object>();
            ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstGeneralPlanTagList);
            if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
            {
                if (ilstGeneralPlanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                    {
                        //objFillHuman = (Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount];
                        lsthuman.Add((Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                    }
                    sFaxFirstname = "_" + lsthuman[0].First_Name;
                    sFaxLastName = "_" + lsthuman[0].Last_Name;


                }
            }
            //string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";
            //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
            //if (File.Exists(strXmlHumanPath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
            //    using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
            //        {
            //            if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
            //            {
            //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
            //                    sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
            //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
            //                    sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();

            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}

            string sFaxSubject = sFaxLastName + sFaxFirstname +"_"+DateTime.Now.ToString("dd-MMM-yyyy");

            string project = "";
            if (Progress == false)
                project = "UCM";

            var result = new { SelectedItem = strselectedItem, DownloadDoc = DownloadDoc, Files = filesNotFound, Screen = screen, Summary = summary, Project = project, HumanId = ClientSession.HumanId, EncId = ClientSession.EncounterId, FaxSubject = sFaxSubject };
            //  var result = new { SelectedItem = strselectedItem, Screen = screen, Summary = summary };
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod(EnableSession = true)]
        public string CopyPreviousPlan()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<TreatmentPlan> Treatment_Plan_List = new List<TreatmentPlan>();
            string strPlan = string.Empty;

            TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();

            FillTreatmentPlan objFillTrtmntPlan = objTreatmentPlanManager.GetTreatmentPlanForPastEncounter(ClientSession.EncounterId,
                                                                                                   ClientSession.HumanId, ClientSession.PhysicianId);
            bool isFound = false;
            if (objFillTrtmntPlan != null)
            {

                IList<TreatmentPlan> tpPlan = objFillTrtmntPlan.Treatment_Plan_List.Where(a => a.Plan_Type == "PLAN").ToList<TreatmentPlan>();

                //Jira CAP-1163 - Old Code
                //if (tpPlan != null && tpPlan.Count > 0 && tpPlan[0].Plan != string.Empty)
                //{
                //    strPlan = tpPlan[0].Plan;
                //    if (strPlan.EndsWith(Environment.NewLine))
                //    {
                //        strPlan = strPlan.TrimEnd('\r', '\n');
                //    }
                //    if (!strPlan.Trim().EndsWith("*"))
                //    {
                //        strPlan += Environment.NewLine + "* ";
                //    }
                //    isFound = true;
                //}
                //else
                //{
                //    strPlan += "* ";
                //}
                //Jira CAP-1163 - New Code 
                if (tpPlan != null && tpPlan.Count > 0)
                {
                    for (int iCount = 0; iCount < tpPlan.Count; iCount++)
                    {
                        if (iCount != 0)
                        {
                            strPlan += Environment.NewLine + Environment.NewLine;
                        }

                        if (tpPlan[iCount].Plan != string.Empty)
                        {
                            strPlan += tpPlan[iCount].Plan;
                            if (strPlan.EndsWith(Environment.NewLine))
                            {
                                strPlan = strPlan.TrimEnd('\r', '\n');
                            }
                            if (!strPlan.Trim().EndsWith("*"))
                            {
                                strPlan += Environment.NewLine + "* ";
                            }
                            isFound = true;
                        }
                        else
                        {
                            strPlan += "* ";
                        }

                    }
                }
                else
                {
                    strPlan += "* ";
                }

                var result = new { PlanOthers = "", Plan = strPlan, PreviousList = objFillTrtmntPlan.PreviousEncounterId, Process = objFillTrtmntPlan.IsPhysicianProcess, PreviousData = isFound };
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                isFound = false;
                var result = new { PlanOthers = "", Plan = "", PreviousList = 0, Process = "", PreviousData = isFound };
                return JsonConvert.SerializeObject(result);
            }



            /*  object result;

              var Treatment_Plan_List = objFillTreatmentPlanDTO.Treatment_Plan_List;

              if (objFillTreatmentPlanDTO != null)
              {
                  string strtest = string.Empty;


                  if (Treatment_Plan_List != null && Treatment_Plan_List.Count > 0)
                  {
                      IList<string> strList = new List<string>();
                      strList.Add("ASSESSMENT :");
                      strList.Add("DIAGNOSTIC ORDER :");
                      strList.Add("IMAGE ORDER :");
                      strList.Add("REFERRAL ORDER :");
                      strList.Add("PROCEDURES :");
                      strList.Add("IMMUNIZATION/INJECTION :");
                      strList.Add("PLAN :");

                      for (int i = 0; i < strList.Count; i++)
                      {

                          string str = strList[i].Replace(":", "").Trim();
                          IList<TreatmentPlan> Plan_List = (from s in Treatment_Plan_List where s.Plan_Type == str select s).ToList<TreatmentPlan>();
                          if (Plan_List != null && Plan_List.Count > 0)
                          {
                              if (str == "REFERRAL ORDER")
                              {

                              }
                              strtest = Plan_List[0].Plan;
                              if (!strtest.EndsWith("\r\n"))
                              {
                                  strtest = strtest + "\r\n";
                              }
                              else
                              {
                                  strtest = Plan_List[0].Plan;
                              }

                              if (strList[i].ToString().Contains("ASSESSMENT") == true)
                              {
                                  string sPlan = string.Empty;
                                  string strPlanVar = strtest.Replace("\r\n", "\n");
                                  string[] strArray = strPlanVar.Split('\n');
                                  SortedDictionary<string, string> dPlan = new SortedDictionary<string, string>();
                                  string sPrimaryICD = string.Empty;
                                  for (int j = 0; j < strArray.Length; j++)
                                  {
                                      if (sPrimaryICD == string.Empty && strArray[0].ToString() != string.Empty)
                                      {
                                          sPrimaryICD = strArray[0].ToString() + "\r\n";
                                          continue;
                                      }
                                      if (strArray[j].ToString() == string.Empty)
                                          break;
                                      if (strArray[j].ToString().Contains("ICD-") == true)
                                      {
                                          int iStart = strArray[j].IndexOf("ICD-") + 5;
                                          int iLength = strArray[j].LastIndexOf(")") - iStart;
                                          if (dPlan.ContainsKey(strArray[j].Substring(iStart, iLength)) == false)
                                          {
                                              dPlan.Add(strArray[j].Substring(iStart, iLength), strArray[j].ToString());
                                          }
                                      }
                                      else
                                      {
                                          sPlan += strArray[j].ToString() + "\r\n";
                                      }

                                  }
                                  strtest = string.Empty;
                                  foreach (KeyValuePair<string, string> item in dPlan)
                                  {
                                      strtest += item.Value + "\r\n";
                                  }
                                  if (sPlan != string.Empty)
                                  {
                                      strtest += sPlan;
                                  }
                                  strtest = sPrimaryICD + strtest;
                              }
                              strPlan += Plan_List[0].Plan_Type + " :" + Environment.NewLine + strtest + Environment.NewLine;

                          }
                      }
                  }
                  IList<TreatmentPlan> Treatmentlst = (from t in Treatment_Plan_List where t.Plan_Type == "PLAN" select t).ToList<TreatmentPlan>();
                  bool isFound = true;

                  if (Treatmentlst.Count == 0)
                  {
                      strPlan += "PLAN :";
                      isFound = false;
                  }
                  else if (strPlan.EndsWith(Environment.NewLine))
                  {
                      strPlan = strPlan.TrimEnd('\r', '\n');
                  }
                  if (!strPlan.EndsWith("*"))
                  {
                      strPlan += Environment.NewLine + "* ";
                  }

                  result = new
                  {
                      PreviousList = objFillTreatmentPlanDTO.PreviousEncounterId,
                      Plan = string.IsNullOrEmpty(strtest.Trim()) ? string.Empty : strPlan,
                      Process = objFillTreatmentPlanDTO.IsPhysicianProcess,
                      PreviousData = isFound
                  };
              }
              else
              {
                  result = new
                  {
                      PreviousList = 0,
                      Plan = "",
                      Process = false,
                      PreviousData = false
                  };
              }

              return JsonConvert.SerializeObject(result);
             */
        }

        [WebMethod(EnableSession = true)]
        public string GetValue(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string result = string.Empty;
            if (data.ToUpper() == "SELF")
            {
                result = ClientSession.PatientPaneList[0].Last_Name + "," + ClientSession.PatientPaneList[0].First_Name + " " + ClientSession.PatientPaneList[0].MI + " " + ClientSession.PatientPaneList[0].Suffix;
            }
            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #region CarePlan

        public static int finalYear { get; set; }
        public static string sSex { get; set; }

        [WebMethod(EnableSession = true)]
        public string LoadeCarePlan()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HttpContext.Current.Session["CarePlanLst"] = null;
            CarePlanManager objCarePlanManager = new CarePlanManager();
            IList<CarePlan> objCarePlanList = new List<CarePlan>();
            IList<CarePlanDTO> objCarePlanDTOList = new List<CarePlanDTO>();
            CarePlan objCarePlan = new CarePlan();
           // string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";

            DateTime PatientDOB = DateTime.MinValue;
            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                PatientDOB = ClientSession.PatientPaneList[0].Birth_Date;
                sSex = ClientSession.PatientPaneList[0].Sex;

            }
            finalYear = CalculateAgeInMonths(PatientDOB, DateTime.Now);
            StaticLookupManager slmanager = new StaticLookupManager();
            IList<StaticLookup> slList = new List<StaticLookup>();
            bool setValue = false;
            if (ClientSession.UserCurrentProcess.ToUpper() == "DICTATION_REVIEW" || ClientSession.UserCurrentProcess.ToUpper() == "CODER_REVIEW_CORRECTION" || ClientSession.UserCurrentProcess.ToUpper() == "PROVIDER_PROCESS" || ClientSession.UserCurrentProcess.ToUpper() == "MA_REVIEW" || ClientSession.UserCurrentProcess.ToUpper() == "MA_PROCESS")//changed PHYSICIAN_CORRECTION to CODER_REVIEW_CORRECTION for Workflow change
            {
                setValue = true;
            }
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();
            //    XmlNodeList xmlTagName;

            //    xmlTagName = itemDoc.GetElementsByTagName("CarePlan");
            //    if (xmlTagName.Count > 0)
            //    {
            //        for (int j = 0; j < xmlTagName.Count; j++)
            //        {
            //            string TagName = xmlTagName[j].Name;
            //            XmlSerializer xmlserializer = new XmlSerializer(typeof(CarePlan));
            //            CarePlan careplan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as CarePlan;
            //            IEnumerable<PropertyInfo> propInfo = null;
            //            propInfo = from obji in ((CarePlan)careplan).GetType().GetProperties() select obji;

            //            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //            {
            //                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                {
            //                    foreach (PropertyInfo property in propInfo)
            //                    {
            //                        if (property.Name == nodevalue.Name)
            //                        {
            //                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                property.SetValue(careplan, Convert.ToUInt64(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                property.SetValue(careplan, Convert.ToString(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                property.SetValue(careplan, Convert.ToDateTime(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                property.SetValue(careplan, Convert.ToInt32(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "BOOLEAN")
            //                                property.SetValue(careplan, Convert.ToBoolean(nodevalue.Value), null);
            //                            else
            //                                property.SetValue(careplan, nodevalue.Value, null);
            //                        }
            //                    }
            //                }

            //            }
            //            objCarePlanList.Add(careplan);
            //        }
            //    }
            //}
            //if (objCarePlanList.Count == 0)
            //{
            objCarePlanDTOList = objCarePlanManager.GetPlanCareFromServerforThin(ClientSession.EncounterId, ClientSession.HumanId, sSex, finalYear);
            var version = (from list in objCarePlanDTOList where list.Version != 0 select list.Version);
            //Jira CAP-339
            Hashtable Careplan_master_id = new Hashtable();
            if (objCarePlanDTOList.Count > 0)
            {
                for (int i = 0; i < objCarePlanDTOList.Count; i++)
                {
                    if (version.Count() == 0)
                    {
                        if (objCarePlanDTOList[i].Plan_Date != string.Empty)
                        {
                            string Validdate = string.Empty;
                            if (objCarePlanDTOList[i].Plan_Date.Split('-').Length == 3)//BugID:43717,43934
                            {
                                try
                                {
                                    if (objCarePlanDTOList[i].Plan_Date.Contains("01-Jan"))
                                    {
                                        if (objCarePlanDTOList[i].Plan_Date.Split(' ').Length >= 2)
                                            Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                        else
                                            Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date + " 6:30:00 PM")).ToString("yyyy-MMM-dd");
                                    }
                                    else
                                    {
                                        if (objCarePlanDTOList[i].Parent_Rule_ID != 1)
                                        {
                                            if (objCarePlanDTOList[i].Plan_Date.Split(' ').Length >= 2)
                                                Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                            else
                                                Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date + " 6:30:00 PM")).ToString("yyyy-MMM-dd");
                                        }
                                        else
                                        {
                                            Validdate = Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date).ToString("yyyy-MMM-dd");//BugID:44068
                                            //Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                        }
                                    }
                                }
                                catch
                                {
                                    if (objCarePlanDTOList[i].Plan_Date.Split(' ').Length >= 2)
                                        Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                    else
                                        Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date + " 6:30:00 PM")).ToString("yyyy-MMM-dd");
                                }
                            }
                            else
                            {
                                Validdate = objCarePlanDTOList[i].Plan_Date;
                            }
                            string[] dates = Validdate.Split('-');
                            if (dates[0].Length == 4)
                            {
                                objCarePlanDTOList[i].Plan_Date = Validdate;
                            }
                            else
                            {
                                if (dates.Length == 3)
                                {
                                    if (dates[0] != "00")
                                        Validdate = dates[2].Substring(0, 4) + "-" + dates[1] + "-" + dates[0];//BugID:43717,43934
                                    else
                                        Validdate = dates[2].Substring(0, 4) + "-" + dates[1];//BugID:43717,43934
                                }
                                if (dates.Length == 2)
                                {
                                    Validdate = dates[1].Substring(0, 4) + "-" + dates[0];//BugID:43717,43934
                                }

                                objCarePlanDTOList[i].Plan_Date = Validdate;

                            }

                        }
                        if ((objCarePlanDTOList[i].Status_Value != string.Empty || objCarePlanDTOList[i].Status_Value != "") && (objCarePlanDTOList[i].Status != string.Empty || objCarePlanDTOList[i].Status != "") && (objCarePlanDTOList[i].Status.ToUpper() != "YES" && objCarePlanDTOList[i].Status.ToUpper() != "NO"))
                        {
                            objCarePlanDTOList[i].Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value) + " - " + Convert.ToString(objCarePlanDTOList[i].Status);
                        }
                        else
                        {
                            objCarePlanDTOList[i].Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value);
                            objCarePlanDTOList[i].Status = Convert.ToString(objCarePlanDTOList[i].Status);
                        }
                        //BugID:47880
                        if (setValue && objCarePlanDTOList[i].Care_Name_Value.IndexOf("Tobacco") != -1 && objCarePlanDTOList[i].Status.Trim() != String.Empty)
                        {
                            string[] sStatus = objCarePlanDTOList[i].Status.Split('|');
                            if (sStatus.Length == 2 && sStatus[0] == "Light cigarette smoker" && IsTobaccoCPTPresent_In_EandMCode(ClientSession.EncounterId))
                            {
                                slList = slmanager.getStaticLookupByFieldNameDefaultValue("FOLLOWUP FOR PREVENTIVE CARE AND SCREENING: TOBACCO USE: SCREENING AND CESSATION INTERVENTION", sStatus[0]);
                                if (slList != null && slList.Count > 0)
                                {
                                    objCarePlanDTOList[i].Status = objCarePlanDTOList[i].Status + "|" + slList[0].Value;
                                }

                            }
                        }
                        if (setValue && objCarePlanDTOList[i].Care_Name_Value.IndexOf("BP") != -1 && objCarePlanDTOList[i].Status.Trim() != String.Empty)
                        {
                            string sStatus = objCarePlanDTOList[i].Status;
                            if (sStatus == "Pre-Hypertensive" || sStatus == "First Hypertensive")
                            {
                                slList = slmanager.getStaticLookupByFieldNameDefaultValue("FOLLOWUP FOR BP SYS/DIA", sStatus);
                                if (slList != null && slList.Count > 0)
                                {
                                    objCarePlanDTOList[i].Status = objCarePlanDTOList[i].Status + "|" + slList[0].Value;
                                }
                            }
                            else
                            {
                                objCarePlanDTOList[i].Status = "";
                            }
                        }
                        //Jira CAP-339
                        Careplan_master_id.Add(objCarePlanDTOList[i].Care_Plan_Lookup_ID, objCarePlanDTOList[i].Master_ID);
                    }
                    else
                    {
                        objCarePlan = new CarePlan();
                        objCarePlan.Human_ID = ClientSession.HumanId;
                        objCarePlan.Encounter_ID = ClientSession.EncounterId;
                        objCarePlan.Physician_ID = ClientSession.PhysicianId;
                        objCarePlan.Care_Name = Convert.ToString(objCarePlanDTOList[i].Care_Name);
                        objCarePlan.Care_Name_Value = Convert.ToString(objCarePlanDTOList[i].Care_Name_Value);
                        //objCarePlan.Status = Convert.ToString(objCarePlanDTOList[i].Status);
                        objCarePlan.Care_Plan_Notes = Convert.ToString(objCarePlanDTOList[i].Care_Plan_Notes);
                        objCarePlan.Created_By = Convert.ToString(objCarePlanDTOList[i].Created_By);
                        objCarePlan.Created_Date_And_Time = Convert.ToDateTime(objCarePlanDTOList[i].Created_Date_And_Time);
                        objCarePlan.Modified_By = Convert.ToString(objCarePlanDTOList[i].Modified_By);
                        objCarePlan.Modified_Date_And_Time = Convert.ToDateTime(objCarePlanDTOList[i].Modified_Date_And_Time);
                        objCarePlan.Care_Plan_Lookup_ID = Convert.ToUInt64(objCarePlanDTOList[i].Care_Plan_Lookup_ID);
                        objCarePlan.Id = Convert.ToUInt64(objCarePlanDTOList[i].Plan_Care_ID);
                        objCarePlan.Version = Convert.ToInt32(objCarePlanDTOList[i].Version);
                        //if (objCarePlanDTOList[i].Plan_Date != string.Empty || objCarePlanDTOList[i].Plan_Date != "")
                        //{
                        //    DateTime Plan_date = Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date);
                        //    objCarePlan.Plan_Date = UtilityManager.ConvertToLocal(Plan_date).ToString("yyyy-MMM-dd");
                        //}
                        if (objCarePlanDTOList[i].Plan_Date.Trim() != string.Empty && objCarePlanDTOList[i].Plan_Date.Split('-').Length == 3)//BugID:43717,43934
                        {
                            DateTime Plan_date;
                            if (objCarePlanDTOList[i].Plan_Date.Split(' ').Length >= 2)
                                Plan_date = Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date);
                            else
                                Plan_date = Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date + " 6:30:00 PM");
                            //jira #CAP-187 - old code
                            //objCarePlan.Plan_Date = UtilityManager.ConvertToLocal(Plan_date).ToString("yyyy-MMM-dd");
                            //jira #CAP-187 - new code
                            if (objCarePlan.Care_Name_Value == "Flu Vaccine" || objCarePlan.Care_Name_Value == "Pneumonia Vaccine" || objCarePlan.Care_Name_Value == "Colorectal Screen")
                            {
                                objCarePlan.Plan_Date = Plan_date.ToString("yyyy-MMM-dd");
                            }
                            else {
                                objCarePlan.Plan_Date = UtilityManager.ConvertToLocal(Plan_date).ToString("yyyy-MMM-dd");
                            }
                        }
                        else
                        {
                            objCarePlan.Plan_Date = objCarePlanDTOList[i].Plan_Date;
                        }
                        //else
                        //{
                        //    objCarePlan.Plan_Date = "";
                        //}
                        // objCarePlan.Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value);
                        if ((objCarePlanDTOList[i].Status_Value != string.Empty || objCarePlanDTOList[i].Status_Value != "") && (objCarePlanDTOList[i].Status != string.Empty || objCarePlanDTOList[i].Status != "") && (objCarePlanDTOList[i].Status.ToUpper() != "YES" && objCarePlanDTOList[i].Status.ToUpper() != "NO"))
                        {
                            objCarePlan.Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value) + " - " + Convert.ToString(objCarePlanDTOList[i].Status);
                        }
                        else
                        {
                            objCarePlan.Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value);
                            objCarePlan.Status = Convert.ToString(objCarePlanDTOList[i].Status);
                        }
                        if (setValue && objCarePlan.Care_Name_Value.IndexOf("Tobacco") != -1 && objCarePlan.Status.Trim() != String.Empty)
                        {
                            string[] sStatus = objCarePlan.Status.Split('|');
                            if (sStatus.Length == 2 && sStatus[0] == "Light cigarette smoker" && IsTobaccoCPTPresent_In_EandMCode(ClientSession.EncounterId))
                            {
                                slList = slmanager.getStaticLookupByFieldNameDefaultValue("FOLLOWUP FOR PREVENTIVE CARE AND SCREENING: TOBACCO USE: SCREENING AND CESSATION INTERVENTION", sStatus[0]);
                                if (slList != null && slList.Count > 0)
                                {
                                    objCarePlan.Status = objCarePlan.Status + "|" + slList[0].Value;
                                }

                            }
                        }
                        if (setValue && objCarePlan.Care_Name_Value.IndexOf("BP") != -1 && objCarePlanDTOList[i].Status.Trim() != String.Empty)
                        {
                            string sStatus = objCarePlanDTOList[i].Status;
                            if (sStatus == "Pre-Hypertensive" || sStatus == "First Hypertensive")
                            {
                                slList = slmanager.getStaticLookupByFieldNameDefaultValue("FOLLOWUP FOR BP SYS/DIA", sStatus);
                                if (slList != null && slList.Count > 0)
                                {
                                    objCarePlan.Status = objCarePlan.Status + "|" + slList[0].Value;
                                }

                            }
                        }
                        objCarePlanList.Add(objCarePlan);
                        //Jira CAP-339
                        Careplan_master_id.Add(objCarePlanDTOList[i].Care_Plan_Lookup_ID, objCarePlanDTOList[i].Master_ID);
                    }
                }
                //CAP-3755
                HttpContext.Current.Session["CarePlanLookupLst"] = objCarePlanDTOList;
            }
            if (objCarePlanList.Count > 0)
            {
                HttpContext.Current.Session["CarePlanLst"] = objCarePlanList;
            }

            if (objCarePlanList.Count > 0)
            {
                var result = new { objCarePlanList = objCarePlanList, DOB = PatientDOB.ToString(), currentProcess = ClientSession.UserCurrentProcess, MasterID= Careplan_master_id };
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                var result = new { objCarePlanList = objCarePlanDTOList, DOB = PatientDOB.ToString(), currentProcess = ClientSession.UserCurrentProcess, MasterID = Careplan_master_id };
                return JsonConvert.SerializeObject(result);
            }

            //}
            //else
            //{
            //    HttpContext.Current.Session["CarePlanLst"] = objCarePlanList;
            //    var result = new { objCarePlanList = objCarePlanList, DOB = PatientDOB.ToString(), currentProcess = ClientSession.UserCurrentProcess };
            //    return JsonConvert.SerializeObject(result);
            //}
        }

        [WebMethod(EnableSession = true)]
        public string SaveCarePlan(object[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            UtilityManager objUtilityMngr = new UtilityManager();
            IList<CarePlan> CarePlanLst = new List<CarePlan>();

            if (HttpContext.Current.Session["CarePlanLst"] != null)
            {
                CarePlanLst = ((IList<CarePlan>)HttpContext.Current.Session["CarePlanLst"]);
            }
            IList<CarePlanDTO> CarePlanDTOLst = new List<CarePlanDTO>();
            if (HttpContext.Current.Session["CarePlanLookupLst"] != null)
            {
                CarePlanDTOLst = ((IList<CarePlanDTO>)HttpContext.Current.Session["CarePlanLookupLst"]);
            }
            IList<CarePlan> lsttempcareplan = new List<CarePlan>();
            IList<CarePlan> SaveCarePlanLst = new List<CarePlan>();
            IList<CarePlan> UpdateCarePlanLst = new List<CarePlan>();
            CarePlanManager objCarePlanManager = new CarePlanManager();
            lsttempcareplan = objCarePlanManager.GetCarePlanByEncounter(ClientSession.EncounterId);
            CarePlan objCarePlan = null;

            for (int i = 0; i < data.Length; i++)
            {
                objCarePlan = new CarePlan();
                Dictionary<string, object> dicValues = new Dictionary<string, object>();
                dicValues = (Dictionary<string, object>)data[i];

                objCarePlan.Encounter_ID = ClientSession.EncounterId;
                objCarePlan.Human_ID = ClientSession.HumanId;
                objCarePlan.Physician_ID = ClientSession.PhysicianId;
                objCarePlan.Care_Name = dicValues["Care_Name"]==null?string.Empty:dicValues["Care_Name"].ToString();
                objCarePlan.Care_Name_Value =dicValues["Care_Name_Value"]==null?string.Empty:dicValues["Care_Name_Value"].ToString();
                objCarePlan.Care_Plan_Lookup_ID = dicValues["Care_Plan_Lookup_ID"]==null?0:Convert.ToUInt32(dicValues["Care_Plan_Lookup_ID"].ToString());
                objCarePlan.Care_Plan_Notes =  dicValues["Care_Plan_Notes"]==null?string.Empty:dicValues["Care_Plan_Notes"].ToString();
                if (objCarePlan.Care_Plan_Notes.Trim() != string.Empty)
                    objCarePlan.Snomed_Code = objUtilityMngr.GetSnomedfromStaticLookup("FollowupList", objCarePlan.Care_Name_Value, objCarePlan.Care_Plan_Notes);
                objCarePlan.Plan_Date = dicValues["Plan_Date"]==null?string.Empty:dicValues["Plan_Date"].ToString();
                if (objCarePlan.Plan_Date != string.Empty)
                {
                    string Validdate = string.Empty;
                    if (objCarePlan.Plan_Date == "")
                        Validdate = "";
                    else
                        Validdate = objCarePlan.Plan_Date;
                    if (Validdate != string.Empty)
                    {
                        if (Validdate.ToString().EndsWith("-") == true)
                        {
                            string[] dates = Validdate.Split('-');
                            if (dates[2] == "" && dates[0] != "" && dates[1] != "")
                                Validdate = Validdate.ToString().Remove(8, 1);
                            else if (dates[1] == "" && dates[0] != "" && dates[2] != "")
                                Validdate = Validdate.ToString().Remove(4, 4);
                            else if (dates[1] == "" && dates[0] != "" && dates[2] == "")
                                Validdate = Validdate.ToString().Remove(4, 2);
                        }
                        if (Validdate != string.Empty && Validdate.Split('-').Length == 3)
                            objCarePlan.Plan_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Validdate)).ToString("yyyy-MMM-dd");
                        else
                            objCarePlan.Plan_Date = Validdate;
                    }
                }
                else
                {
                    string Validdate = "";
                    objCarePlan.Plan_Date = Validdate;
                }

                //objCarePlan.Status_Value = dicValues["Status_Value"].ToString();

                //objCarePlan.Status = dicValues["Status"].ToString();
                //BugID:53926 
                if (dicValues["Status_Value"]!=null && dicValues["Status_Value"].ToString().Contains(" - "))
                {
                    dicValues["Status_Value"] = dicValues["Status_Value"].ToString().Replace(" - ", "~");
                    objCarePlan.Status_Value = dicValues["Status_Value"].ToString().Split('~')[0];
                    if (dicValues["Status_Value"].ToString().Split('~').Length > 1)
                        objCarePlan.Status = dicValues["Status_Value"].ToString().Split('~')[1];
                }
                else if (dicValues["Status_Value"]!=null && dicValues["Status_Value"].ToString().Contains("/"))
                {
                    dicValues["Status_Value"] = dicValues["Status_Value"].ToString().Replace(",", "~");
                    objCarePlan.Status_Value = dicValues["Status_Value"].ToString().Split('~')[0];
                    if (dicValues["Status_Value"].ToString().Split('~').Length > 1)
                        objCarePlan.Status = dicValues["Status_Value"].ToString().Split('~')[1];
                }
                //if (dicValues["Status_Value"].ToString().Contains('-'))
                //{
                //    dicValues["Status_Value"] = dicValues["Status_Value"].ToString().Replace(" - ", "~");
                //    objCarePlan.Status_Value = dicValues["Status_Value"].ToString().Split('~')[0];
                //    objCarePlan.Status = dicValues["Status_Value"].ToString().Split('~')[1];
                //}
                else
                {
                    objCarePlan.Status_Value = dicValues["Status_Value"]==null?string.Empty:dicValues["Status_Value"].ToString();
                    objCarePlan.Status = dicValues["Status"]==null?string.Empty:dicValues["Status"].ToString();
                }

                if (objCarePlan.Status.ToUpper() != "NO" && objCarePlan.Status.ToUpper() != "")
                {
                    objCarePlan.Internal_Property_bInsert = true;
                }
                //CAP-3755
                if (CarePlanDTOLst != null)
                {
                    var carePlanLookup = CarePlanDTOLst.FirstOrDefault(a => a.Care_Plan_Lookup_ID == objCarePlan.Care_Plan_Lookup_ID);
                    objCarePlan.Care_Plan_Loinc_Code = carePlanLookup.Care_Plan_Loinc_Code;
                    var possible_Options = carePlanLookup.Options.Split('|');
                    int selectedIndex = Array.IndexOf(possible_Options, objCarePlan.Status);
                    if (carePlanLookup.Options_Loinc_Code?.Split('|')?.Length > selectedIndex && selectedIndex != -1)
                    {
                        objCarePlan.Selected_Option_Loinc_Code = carePlanLookup.Options_Loinc_Code.Split('|')[selectedIndex];
                    }
                }

                var countlist = (from m in lsttempcareplan where m.Care_Name_Value == dicValues["Care_Name_Value"].ToString() select m).ToList();

                if (countlist.Count() == 0)
                {
                    objCarePlan.Created_By = ClientSession.PhysicianUserName;
                    objCarePlan.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    SaveCarePlanLst.Add(objCarePlan);
                }
                else
                {
                    objCarePlan.Id = countlist[0].Id;
                    objCarePlan.Version = countlist[0].Version;
                    objCarePlan.Modified_By = ClientSession.PhysicianUserName;
                    objCarePlan.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                   // objCarePlan.Created_By = CarePlanLst[i].Created_By;
                    //objCarePlan.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                    UpdateCarePlanLst.Add(objCarePlan);
                }

                //if (CarePlanLst.Count == 0)
                //{
                //    objCarePlan.Created_By = ClientSession.PhysicianUserName;
                //    objCarePlan.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                //    SaveCarePlanLst.Add(objCarePlan);
                //}
                //else
                //{
                //    objCarePlan.Id = CarePlanLst[i].Id;
                //    objCarePlan.Version = CarePlanLst[i].Version;
                //    objCarePlan.Modified_By = ClientSession.PhysicianUserName;
                //    objCarePlan.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                //    objCarePlan.Created_By = CarePlanLst[i].Created_By;
                //    objCarePlan.Created_Date_And_Time = CarePlanLst[i].Created_Date_And_Time;
                //    UpdateCarePlanLst.Add(objCarePlan);
                //}
            }

            IList<CarePlan> lstCarePlanLstdelete = new List<CarePlan>();
            IList<ulong> temp1 = new List<ulong>();
            IList<ulong> temp2 = new List<ulong>();

            lstCarePlanLstdelete = SaveCarePlanLst.Union(UpdateCarePlanLst).ToList<CarePlan>();

            //lstCarePlanLstdelete = lsttempcareplan.Except(lstCarePlanLstdelete).ToList<CarePlan>();

            temp1 = lsttempcareplan.Select(a => a.Care_Plan_Lookup_ID).ToList<ulong>();
            temp2 = lstCarePlanLstdelete.Select(a => a.Care_Plan_Lookup_ID).ToList<ulong>();

            temp2 = temp1.Except(temp2).ToList<ulong>();

            lstCarePlanLstdelete = (from m in lsttempcareplan where temp2.Contains(m.Care_Plan_Lookup_ID) select m).ToList<CarePlan>();
            // Care_Plan_Lookup_ID

            if (SaveCarePlanLst.Count > 0)
            {
                CarePlanLst = objCarePlanManager.SaveCarePlanXML(SaveCarePlanLst.ToArray<CarePlan>(), sSex, finalYear, string.Empty);
            }
            if (UpdateCarePlanLst.Count>0)
            {
                CarePlanLst = objCarePlanManager.UpdateCarePLanXML(UpdateCarePlanLst.ToArray<CarePlan>(), sSex, finalYear, string.Empty);
            }

            objCarePlanManager.SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveCarePlanLst, ref UpdateCarePlanLst, null, string.Empty, true, true, ClientSession.EncounterId, string.Empty);
            if(lstCarePlanLstdelete.Count>0)

            {
                for (int j = 0; j < lstCarePlanLstdelete.Count; j++)
                {
                    lstCarePlanLstdelete[j].Care_Plan_Notes = "";
                    lstCarePlanLstdelete[j].Plan_Date="01-01-0001";
                    lstCarePlanLstdelete[j].Status="";
                    lstCarePlanLstdelete[j].Status_Value = "";
                }
                    objCarePlanManager.DeleteCarePLanXML(lstCarePlanLstdelete.ToArray<CarePlan>(), sSex, finalYear, string.Empty);
            }
           

            IList<CarePlan> lstCarePlanLst = new List<CarePlan>();

            foreach (CarePlan obj in SaveCarePlanLst)
            {
                if ((obj.Status_Value != string.Empty || obj.Status_Value != "") && (obj.Status != string.Empty || obj.Status != ""))
                {
                    obj.Status_Value = Convert.ToString(obj.Status_Value) + " - " + Convert.ToString(obj.Status);
                }
                else
                {
                    obj.Status_Value = Convert.ToString(obj.Status_Value);
                    obj.Status = Convert.ToString(obj.Status);
                }
                lstCarePlanLst.Add(obj);

            }

            foreach (CarePlan obj in UpdateCarePlanLst)
            {
                if ((obj.Status_Value != string.Empty || obj.Status_Value != "") && (obj.Status != string.Empty || obj.Status != ""))
                {
                    obj.Status_Value = Convert.ToString(obj.Status_Value) + " - " + Convert.ToString(obj.Status);
                }
                else
                {
                    obj.Status_Value = Convert.ToString(obj.Status_Value);
                    obj.Status = Convert.ToString(obj.Status);
                }
                lstCarePlanLst.Add(obj);

            }


            foreach (CarePlan obj in lstCarePlanLst)
            {

                if (obj.Plan_Date != string.Empty && obj.Plan_Date.Split('-').Length == 3)
                    obj.Plan_Date = UtilityManager.ConvertToLocal(Convert.ToDateTime(obj.Plan_Date + " 6:30:00 PM")).ToString("yyyy-MMM-dd");
            }

            var tobaccocount = (from m in lstCarePlanLst where m.Care_Plan_Notes.Contains("Cessation counseling visit; intermediate - 3 to 10 mins")
                                    || m.Care_Plan_Notes.Contains("Cessation counseling visit; intensive - More than 10 minutes")
                                select m).ToList();
           if (tobaccocount.Count() > 0)
           {
               if (ClientSession.FillEncounterandWFObject != null)
               {
                   if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                   {

                       ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved = "N";
                       IList<Encounter> lst = new List<Encounter>();
                       IList<Encounter> lsttemp = new List<Encounter>();
                       lst.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                       EncounterManager obj = new EncounterManager();
                       obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.FillEncounterandWFObject.EncRecord.Id, string.Empty);
                       ClientSession.FillEncounterandWFObject.EncRecord = lst[0];
                   }
               }

           }
            if (HttpContext.Current.Session["CarePlanLst"] != null)
            {
                CarePlanLst = ((IList<CarePlan>)HttpContext.Current.Session["CarePlanLst"]);
                for (int i = 0; i < CarePlanLst.Count; i++)
                    CarePlanLst[i].Version = CarePlanLst[i].Version + 1;
                HttpContext.Current.Session["CarePlanLst"] = CarePlanLst;

            }

            //  HttpContext.Current.Session["CarePlanLst"] = lstCarePlanLst;
            return JsonConvert.SerializeObject(lstCarePlanLst);
            //HttpContext.Current.Session["CarePlanLst"] = CarePlanLst;
            //return JsonConvert.SerializeObject(CarePlanLst);

        }

        [WebMethod(EnableSession = true)]
        public string CopyPreviousCarePlan()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            CarePlanManager objCarePlanManager = new CarePlanManager();
            IList<CarePlanDTO> objCarePlanDTOList = new List<CarePlanDTO>();
            DateTime PatientDOB = DateTime.MinValue;
            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                PatientDOB = ClientSession.PatientPaneList[0].Birth_Date;
                sSex = ClientSession.PatientPaneList[0].Sex;

            }
            finalYear = CalculateAgeInMonths(PatientDOB, DateTime.Now);
            objCarePlanDTOList = objCarePlanManager.GetCarePlanForPastEncounter(ClientSession.EncounterId,
                                                                                ClientSession.HumanId,
                                                                                ClientSession.PhysicianId, sSex, finalYear);

            Hashtable Careplan_master_id = new Hashtable();

            if (objCarePlanDTOList.Count > 0)
            {
                for (int i = 0; i < objCarePlanDTOList.Count; i++)
                {

                    if (objCarePlanDTOList[i].Plan_Date != string.Empty)
                    {
                        string Validdate = string.Empty;
                        if (objCarePlanDTOList[i].Plan_Date.Split('-').Length == 3)//BugID:43717,43934
                        {
                            try
                            {
                                if (objCarePlanDTOList[i].Plan_Date.Contains("01-Jan"))
                                    Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                else
                                {
                                    if (objCarePlanDTOList[i].Parent_Rule_ID != 1)
                                        Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                    else
                                    {
                                        Validdate = Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date).ToString("yyyy-MMM-dd");//BugID:44068
                                        //Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                                    }
                                }
                            }
                            catch
                            {
                                Validdate = UtilityManager.ConvertToLocal(Convert.ToDateTime(objCarePlanDTOList[i].Plan_Date)).ToString("yyyy-MMM-dd");
                            }
                        }
                        else
                        {
                            Validdate = objCarePlanDTOList[i].Plan_Date;
                        }
                        string[] dates = Validdate.Split('-');
                        if (dates[0].Length == 4)
                        {
                            objCarePlanDTOList[i].Plan_Date = Validdate;
                        }
                        else
                        {
                            if (dates.Length == 3)
                            {
                                if (dates[0] != "00")
                                    Validdate = dates[2].Substring(0, 4) + "-" + dates[1] + "-" + dates[0];//BugID:43717,43934
                                else
                                    Validdate = dates[2].Substring(0, 4) + "-" + dates[1];//BugID:43717,43934
                            }
                            if (dates.Length == 2)
                            {
                                Validdate = dates[1].Substring(0, 4) + "-" + dates[0];//BugID:43717,43934
                            }

                            objCarePlanDTOList[i].Plan_Date = Validdate;

                        }

                    }

                    Careplan_master_id.Add(objCarePlanDTOList[i].Care_Plan_Lookup_ID, objCarePlanDTOList[i].Master_ID);
                    //var planDate = objCarePlanDTOList[i].Plan_Date;

                    //if (string.IsNullOrEmpty(planDate))
                    //{
                    //    continue;
                    //}

                    //string Validdate = string.Empty;

                    //try
                    //{
                    //    if (planDate.Contains("01-Jan"))
                    //        Validdate = planDate;
                    //    else
                    //    {
                    //        if (objCarePlanDTOList[i].Parent_Rule_ID != 1)
                    //            Validdate = Convert.ToDateTime(planDate).ToString("yyyy-MMM-dd");
                    //        else
                    //            Validdate = Convert.ToDateTime(planDate).ToString("yyyy-MMM-dd");
                    //    }
                    //}
                    //catch
                    //{
                    //    Validdate = planDate;
                    //}

                    //string[] dates = Validdate.Split('-');

                    //if (dates[0].Length == 4)
                    //{
                    //    objCarePlanDTOList[i].Plan_Date = Validdate;
                    //}
                    //else
                    //{
                    //    if (dates.Length == 3)
                    //    {
                    //        if (dates[0] != "00")
                    //            Validdate = dates[2].Substring(0, 4) + dates[1] + dates[0];
                    //        else
                    //            Validdate = dates[2].Substring(0, 4) + dates[1];
                    //    }
                    //    if (dates.Length == 2)
                    //    {
                    //        Validdate = dates[1].Substring(0, 4) + dates[0];
                    //    }

                    //    objCarePlanDTOList[i].Plan_Date = Validdate;
                    //}


                    if ((objCarePlanDTOList[i].Status_Value != string.Empty || objCarePlanDTOList[i].Status_Value != "") && (objCarePlanDTOList[i].Status != string.Empty || objCarePlanDTOList[i].Status != "") && (objCarePlanDTOList[i].Status.ToUpper() != "YES" && objCarePlanDTOList[i].Status.ToUpper() != "NO"))
                    {
                        objCarePlanDTOList[i].Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value) + " - " + Convert.ToString(objCarePlanDTOList[i].Status);
                    }
                    else
                    {
                        objCarePlanDTOList[i].Status_Value = Convert.ToString(objCarePlanDTOList[i].Status_Value);
                        objCarePlanDTOList[i].Status = Convert.ToString(objCarePlanDTOList[i].Status);
                    }
                }
            }
            int careplanlistcount = 0;
            var FinalList = (from m in objCarePlanDTOList where m.Care_Plan_Notes != "" || m.Plan_Date != "" || m.Status != "" select m).ToList();
            careplanlistcount = FinalList.Count();
            int[] arrChangedListId = FinalList.Select(x => (int)x.Care_Plan_Lookup_ID).ToArray();
           
            var result = new { objCarePlanList = objCarePlanDTOList, CareplanListcount = careplanlistcount, MasterID = Careplan_master_id,ChangedMasterId = arrChangedListId };
            return JsonConvert.SerializeObject(result);
            // return JsonConvert.SerializeObject(objCarePlanDTOList);
        }

        public static int CalculateAgeInMonths(DateTime birthDate, DateTime now)
        {
            int leap = 0;
            int Months = 0;
            if (1 == now.Month || 3 == now.Month || 5 == now.Month || 7 == now.Month || 8 == now.Month ||
            10 == now.Month || 12 == now.Month)
            {
                leap = 31;
            }
            else if (2 == now.Month)
            {
                if (0 == (now.Year % 4))
                {
                    if (0 == (now.Year % 400))
                    {
                        leap = 29;
                    }
                    else if (0 == (now.Year % 100))
                    {
                        leap = 28;
                    }

                    leap = 29;
                }
                else
                {
                    leap = 28;
                }

            }
            else
            {
                leap = 30;
            }


            if (leap == 28)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }
            else if (leap == 31)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }
            else if (leap == 29)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (366 / 12));
            }
            else if (leap == 30)
            {
                Months = Convert.ToInt16(now.Subtract(birthDate).Days / (365.25 / 12));
            }

            return Months;
        }
        #endregion

        #region Preventive Screen Plan

        [WebMethod(EnableSession = true)]
        public string LoadPreventiveScreenPlan()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            PreventiveScreenManager objPreventiveScreenManager = new PreventiveScreenManager();
            IList<PreventiveScreen> PreventiveScreenLst = new List<PreventiveScreen>();
            PreventiveScreen objPreventivePlan = new PreventiveScreen();
            HttpContext.Current.Session["PreventiveScreenLst"] = null;

            PreventiveScreenLst = objPreventiveScreenManager.GetPreventiveScreenPlanDetails(ClientSession.EncounterId, ClientSession.HumanId);
            HttpContext.Current.Session["PreventiveScreenLst"] = PreventiveScreenLst;
            string sGender = string.Empty;
            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                sGender = ClientSession.PatientPaneList[0].Sex;
            }
            var result = new { PreventiveScreenLst = PreventiveScreenLst, currentProcess = ClientSession.UserCurrentProcess, Gender = sGender };
            return JsonConvert.SerializeObject(result);
            //var result = new { PreventiveScreenLst = PreventiveScreenLst, currentProcess = ClientSession.UserCurrentProcess };
            //return JsonConvert.SerializeObject(result);
        }

        [WebMethod(EnableSession = true)]
        public string SavePreventiveScreenPlan(object[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<PreventiveScreen> PreventiveScreenLst = new List<PreventiveScreen>();

            if (HttpContext.Current.Session["PreventiveScreenLst"] != null)
            {
                PreventiveScreenLst = ((IList<PreventiveScreen>)HttpContext.Current.Session["PreventiveScreenLst"]);
            }

            IList<PreventiveScreen> lstPreventivescreen = new List<PreventiveScreen>();
            PreventiveScreenManager objPrevewntvScrnManager = new PreventiveScreenManager();
            IList<PreventiveScreen> SavePreventiveScreenLst = new List<PreventiveScreen>();
            IList<PreventiveScreen> UpdatePreventiveScreenLst = new List<PreventiveScreen>();

            PreventiveScreen objPreventiveScreen = null;

            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    objPreventiveScreen = new PreventiveScreen();
                    Dictionary<string, object> dicValues = new Dictionary<string, object>();
                    dicValues = (Dictionary<string, object>)data[i];
                    objPreventiveScreen.Encounter_ID = ClientSession.EncounterId;
                    objPreventiveScreen.Human_ID = ClientSession.HumanId;
                    objPreventiveScreen.Physician_ID = ClientSession.PhysicianId;
                    objPreventiveScreen.Preventive_Screening_Notes = dicValues["Preventive_Screening_Notes"]==null?string.Empty:dicValues["Preventive_Screening_Notes"].ToString();
                    objPreventiveScreen.Preventive_Service = dicValues["Preventive_Service"]==null?string.Empty:dicValues["Preventive_Service"].ToString();
                    objPreventiveScreen.Preventive_Service_Value = dicValues["Preventive_Service_Value"]==null?string.Empty:dicValues["Preventive_Service_Value"].ToString();
                    objPreventiveScreen.Status = dicValues["Status"]==null?string.Empty:dicValues["Status"].ToString();
                    //CAP-1690
                    if (PreventiveScreenLst.Count == 0 || PreventiveScreenLst.Count != data.Length)
                    {
                        objPreventiveScreen.Created_By = ClientSession.PhysicianUserName;
                        objPreventiveScreen.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        SavePreventiveScreenLst.Add(objPreventiveScreen);
                    }
                    else
                    {
                        objPreventiveScreen.Id = PreventiveScreenLst[i].Id;
                        objPreventiveScreen.Version = PreventiveScreenLst[i].Version;
                        objPreventiveScreen.Modified_By = ClientSession.PhysicianUserName;
                        objPreventiveScreen.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objPreventiveScreen.Created_By = PreventiveScreenLst[i].Created_By;
                        objPreventiveScreen.Created_Date_And_Time = PreventiveScreenLst[i].Created_Date_And_Time;
                        UpdatePreventiveScreenLst.Add(objPreventiveScreen);
                    }
                }
            }
            //CAP-1690
            if (SavePreventiveScreenLst.Count > 0 && PreventiveScreenLst.Count == 0)
            {
                lstPreventivescreen = objPrevewntvScrnManager.SavePreventiveList(SavePreventiveScreenLst, string.Empty);
            }
            //CAP-1690
            else if (SavePreventiveScreenLst.Count > 0 && PreventiveScreenLst.Count != data.Length)
            {
                lstPreventivescreen = objPrevewntvScrnManager.SaveDeletePreventiveList(SavePreventiveScreenLst, PreventiveScreenLst, string.Empty);
            }
            else
            {
                lstPreventivescreen = objPrevewntvScrnManager.UpdatePreventiveList(UpdatePreventiveScreenLst, string.Empty);
            }

            HttpContext.Current.Session["PreventiveScreenLst"] = lstPreventivescreen;
            return JsonConvert.SerializeObject(lstPreventivescreen);
        }
        [WebMethod(EnableSession = true)]
        public string CopyPreviousPreventiveScreenPlan()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<PreventiveScreenDTO> lstPreventiveCare = new List<PreventiveScreenDTO>();
            PreventiveScreenManager objPrevewntvScrnManager = new PreventiveScreenManager();

            bool isPreviousEncounter = false;
            bool isPhysicianProcess = false;

            lstPreventiveCare = objPrevewntvScrnManager.GetPreventivePlanForPastEncounter(ClientSession.EncounterId, ClientSession.HumanId, ClientSession.PhysicianId);

            if (lstPreventiveCare.Count == 0)
            {
                isPreviousEncounter = false;
                isPhysicianProcess = false;
            }
            else if (lstPreventiveCare.Count >= 1)
            {
                isPreviousEncounter = (lstPreventiveCare[0].PEnc != 0);
                isPhysicianProcess = lstPreventiveCare[0].Physician_Process;
            }
           
            var result = new
            {
                data = lstPreventiveCare,
                IsPreviousEncounter = isPreviousEncounter,
                IsPhyEnc = isPhysicianProcess
            };

            return JsonConvert.SerializeObject(result);
        }
        #endregion

        //BugID:47955 START
        #region ServProcCodesCPTCheck
        private bool IsTobaccoCPTPresent_In_EandMCode(ulong EncounterID)
        {
            bool IsTobaccoCPTPresent = false, IsTobaccoCPTNotDeleted = false;

            IList<EAndMCoding> lsteandm = new List<EAndMCoding>();
            IList<string> ilstGeneralPlanTagList = new List<string>();
            ilstGeneralPlanTagList.Add("EAndMCodingList");
            IList<object> ilstGeneralPlanBlobFinal = new List<object>();
            ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(EncounterID, ilstGeneralPlanTagList);
            if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
            {
                if (ilstGeneralPlanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                    {
                        //objFillHuman = (Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount];
                        lsteandm.Add((EAndMCoding)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                    }
                  


                }
                for (int i = 0; i < lsteandm.Count; i++)
                {
                    if ((lsteandm[i].Procedure_Code == "99406" || lsteandm[i].Procedure_Code == "99407") && lsteandm[i].Encounter_ID == EncounterID)
                    {
                        IsTobaccoCPTPresent = true;
                        if (lsteandm[i].Is_Delete == "N")
                        {
                            IsTobaccoCPTNotDeleted = true;
                            break;
                        }
                    }
                }
            }
            //string filename = "Encounter" + "_" + EncounterID + ".xml";
            //string XmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], filename);
            //if (File.Exists(XmlFilePath))
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(XmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    //  itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(XmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        #region EAndMCodingList
            //        if (itemDoc.GetElementsByTagName("EAndMCodingList") != null && itemDoc.GetElementsByTagName("EAndMCodingList").Count > 0)
            //        {
            //                xmlTagName = itemDoc.GetElementsByTagName("EAndMCodingList")[0].ChildNodes;
            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(EAndMCoding));
            //                        EAndMCoding EAndMCoding = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EAndMCoding;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        if (EAndMCoding != null)
            //                        {
            //                            propInfo = from obji in ((EAndMCoding)EAndMCoding).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(EAndMCoding, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(EAndMCoding, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(EAndMCoding, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(EAndMCoding, Convert.ToInt32(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                property.SetValue(EAndMCoding, Convert.ToDecimal(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(EAndMCoding, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            if ((EAndMCoding.Procedure_Code == "99406" || EAndMCoding.Procedure_Code == "99407") && EAndMCoding.Encounter_ID == EncounterID)
            //                            {
            //                                IsTobaccoCPTPresent = true;
            //                                if (EAndMCoding.Is_Delete == "N")
            //                                {
            //                                    IsTobaccoCPTNotDeleted = true;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //        }
            //        #endregion
            //        fs.Close();
            //        fs.Dispose();
            //    }
            // }
            if ((!IsTobaccoCPTPresent) || (IsTobaccoCPTPresent && IsTobaccoCPTNotDeleted))
            {
                return true;
            }
            else
                return false;
        }
        #endregion
        //BugID:47955 END
    }
}
