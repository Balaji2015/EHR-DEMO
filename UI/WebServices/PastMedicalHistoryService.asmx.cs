using System;
using System.Collections;
using System.Collections.Generic;
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
using AjaxControlToolkit.Design;
using AjaxControlToolkit;
using Telerik.Web.Design;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Web.Services;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Xml.Serialization;


namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for PastMedicalHistoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PastMedicalHistoryService : System.Web.Services.WebService
    {
        PastMedicalHistoryManager objPastMedicalHistoryManager = new PastMedicalHistoryManager();
        EncounterManager objEncounterManager = new EncounterManager();
        public static IList<PastMedicalHistory> savedLst = new List<PastMedicalHistory>();
        public static IList<GeneralNotes> savedNotesLst = new List<GeneralNotes>();
        IList<Encounter> EncList = new List<Encounter>();
        string openingfrom = string.Empty;
        public string changeFormat(string oldfrmdate)
        {
            string newfrmdate = string.Empty;
            if (oldfrmdate != string.Empty && oldfrmdate != "Current")
            {
                string[] aryFromDate = oldfrmdate.Split('-');
                if (aryFromDate.Length == 3)
                {
                    if (aryFromDate[0].Trim() != string.Empty && aryFromDate[1].Trim() != string.Empty && aryFromDate[2].Trim() != string.Empty)
                    {
                        if (aryFromDate[0].Trim().Length == 1)
                            newfrmdate = aryFromDate[2] + "-" + aryFromDate[1] + "-" + "0" + aryFromDate[0].Trim();
                        else
                            newfrmdate = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];

                    }
                }
                else if (aryFromDate.Length == 2)
                    newfrmdate = aryFromDate[1] + "-" + aryFromDate[0];
                else
                    newfrmdate = oldfrmdate;
            }
            else
            {
                newfrmdate = oldfrmdate;
            }
            return newfrmdate;
        }

        [WebMethod(EnableSession = true)]
        public string LoadPFSHtab(string HumanID, string OpeningFrom, string PhysicianName, string PhysicianID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string PFSHvalue = string.Empty;
            IList<Encounter> enclst = new List<Encounter>();
            SecurityServiceUtility SecurityService = new SecurityServiceUtility();
            IList<string> PFSH_tab_to_disable = SecurityService.GetListTabtoDisable("frmPFSH");            
            string Source_of_info = string.Empty;
            string PFSHVerified = string.Empty;
            string OthersVAl = string.Empty;
            if (OpeningFrom == "Queue")
            {                
                #region "Modified by balaji.TJ  - 2023-01-05" 
                IList<string> ilstPastMHisList = new List<string>();
                ilstPastMHisList.Add("EncounterList");
                
                IList<object> ilstPastMHBlobFinal = new List<object>();
                ilstPastMHBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstPastMHisList);
                if (ilstPastMHBlobFinal != null && ilstPastMHBlobFinal.Count > 0)
                {
                    if (ilstPastMHBlobFinal[0] != null && ((IList<object>)ilstPastMHBlobFinal[0]).Count > 0)
                    {
                        for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[0]).Count; i++)
                        {
                            Source_of_info =((Encounter)((List<object>)ilstPastMHBlobFinal[0])[i]).Source_Of_Information.Trim();
                            if(((Encounter)((List<object>)ilstPastMHBlobFinal[0])[i]).Is_PFSH_Verified.Trim() == "Y" && ClientSession.bPFSHVerified)
                                PFSHVerified = "Y";
                            else
                                PFSHVerified = "N";
                            OthersVAl = ((Encounter)((List<object>)ilstPastMHBlobFinal[0])[i]).If_Source_Of_Information_Others.Trim();

                        }
                    }
                }

                #endregion

                #region "Comment by balaji.TJ  - 2023-01-05"
              
                //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //string FileNamenew = string.Empty;
                //string strXmlFilePathmew = string.Empty;

                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    //itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                //        {
                           
                //            XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;
                //            if (xmlTagName.Count > 0)
                //            {
                //                Source_of_info = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes["Source_Of_Information"].Value.Trim();
                //                if (itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes["Is_PFSH_Verified"].Value.Trim() == "Y" && ClientSession.bPFSHVerified)
                //                    PFSHVerified = "Y";
                //                else
                //                    PFSHVerified = "N";
                //                OthersVAl = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes[0].Attributes["If_Source_Of_Information_Others"].Value;
                //                //string TagName = xmlTagName[0].Name;
                //                //XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
                //                //Encounter Encounter = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[0])) as Encounter;
                //                //IEnumerable<PropertyInfo> propInfo = null;
                //                //propInfo = from obji in ((Encounter)Encounter).GetType().GetProperties() select obji;
                //                //for (int i = 0; i < xmlTagName[0].Attributes.Count; i++)
                //                //{
                //                //    XmlNode nodevalue = xmlTagName[0].Attributes[i];
                //                //    {
                //                //        if (propInfo != null)
                //                //        {
                //                //            foreach (PropertyInfo property in propInfo)
                //                //            {
                //                //                if (property.Name == nodevalue.Name)
                //                //                {
                //                //                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                //                        property.SetValue(Encounter, Convert.ToUInt64(nodevalue.Value), null);
                //                //                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                //                        property.SetValue(Encounter, Convert.ToString(nodevalue.Value), null);
                //                //                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                //                        property.SetValue(Encounter, Convert.ToDateTime(nodevalue.Value), null);
                //                //                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                //                        property.SetValue(Encounter, Convert.ToInt32(nodevalue.Value), null);
                //                //                    else
                //                //                        property.SetValue(Encounter, nodevalue.Value, null);
                //                //                }
                //                //            }
                //                //        }
                //                //    }

                //                //}
                //                //enclst.Add(Encounter);
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                #endregion
                //  HttpContext.Current.Session["Encnter"] = enclst;
                //string Source_of_info = string.Empty;
                //string PFSHVerified = string.Empty;
                //string OthersVAl = string.Empty;

                //IList<Encounter> EncList = (IList<Encounter>)HttpContext.Current.Session["Encnter"];
                //if (EncList != null && EncList.Count > 0)
                //{
                //    Source_of_info = EncList[0].Source_Of_Information.Trim();
                //    if (EncList[0].Is_PFSH_Verified.Trim() == "Y" && ClientSession.bPFSHVerified)
                //        PFSHVerified = "Y";
                //    else
                //        PFSHVerified = "N";
                //    OthersVAl = EncList[0].If_Source_Of_Information_Others;
                //}
                UIManager.PFSH_OpeingFrom = OpeningFrom;
                //  PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + ClientSession.HumanId + "&PhysicianID=" + ClientSession.PhysicianId + "&" + ClientSession.PhysicianUserName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
                if (PFSH_tab_to_disable != null && PFSH_tab_to_disable.Count > 0)
                {
                    var tabs_to_disable = PFSH_tab_to_disable.Select(a => new
                    {
                        tab = a
                    });
                    PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + ClientSession.HumanId + "&PhysicianID=" + ClientSession.PhysicianId + "&" + ClientSession.PhysicianUserName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "") + "-Disable_Tab=" + JsonConvert.SerializeObject(tabs_to_disable);

                }
                else
                {
                    PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + ClientSession.HumanId + "&PhysicianID=" + ClientSession.PhysicianId + "&" + ClientSession.PhysicianUserName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "") + "-Disable_Tab=" + "";
                }
            }
            if (OpeningFrom == "Menu")
            {
                //string FileName = "Human" + "_" + HumanID + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["XMLPath"], FileName);
                //string FileNamenew = string.Empty;
                //string strXmlFilePathmew = string.Empty;
                /*
                if (File.Exists(strXmlFilePath) == true)
                {
                    XmlDocument itemDoc = new XmlDocument();
                    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                    itemDoc.Load(XmlText);
                    XmlText.Close();
                    if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
                    {

                        XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;
                        if (xmlTagName.Count > 0)
                        {
                            string TagName = xmlTagName[0].Name;
                            XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
                            Encounter Encounter = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[0])) as Encounter;
                            IEnumerable<PropertyInfo> propInfo = null;
                            Encounter = (Encounter)Encounter;
                            propInfo = from obji in ((Encounter)Encounter).GetType().GetProperties() select obji;

                            for (int i = 0; i < xmlTagName[0].Attributes.Count; i++)
                            {
                                XmlNode nodevalue = xmlTagName[0].Attributes[i];
                                {
                                    foreach (PropertyInfo property in propInfo)
                                    {
                                        if (property.Name == nodevalue.Name)
                                        {
                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                property.SetValue(Encounter, Convert.ToUInt64(nodevalue.Value), null);
                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                property.SetValue(Encounter, Convert.ToString(nodevalue.Value), null);
                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                property.SetValue(Encounter, Convert.ToDateTime(nodevalue.Value), null);
                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                property.SetValue(Encounter, Convert.ToInt32(nodevalue.Value), null);
                                            else
                                                property.SetValue(Encounter, nodevalue.Value, null);
                                        }
                                    }
                                }

                            }
                            enclst.Add(Encounter);
                        }
                    }
                }
                HttpContext.Current.Session["Encnter"] = enclst;
                string Source_of_info = string.Empty;
                string PFSHVerified = string.Empty;
                string OthersVAl = string.Empty;

                IList<Encounter> EncList = (IList<Encounter>)HttpContext.Current.Session["Encnter"];
                if (EncList != null && EncList.Count > 0)
                {
                    Source_of_info = EncList[0].Source_Of_Information.Trim();

                    if (EncList[0].Is_PFSH_Verified.Trim() == "Y")
                        PFSHVerified = "Y";

                    OthersVAl = EncList[0].If_Source_Of_Information_Others;
                }
                */
                if (PhysicianID != string.Empty)
                    ClientSession.PhysicianId = Convert.ToUInt32(PhysicianID);
               
                UIManager.PFSH_OpeingFrom = OpeningFrom;
                if (PhysicianName == string.Empty)
                    PhysicianName = ClientSession.UserName;
                // PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + HumanID + "&PhysicianID=" + ClientSession.PhysicianId + "&" + PhysicianName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
                if (PFSH_tab_to_disable != null && PFSH_tab_to_disable.Count > 0)
                {
                    var tabs_to_disable = PFSH_tab_to_disable.Select(a => new
                    {
                        tab = a
                    });
                    PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + HumanID + "&PhysicianID=" + ClientSession.PhysicianId + "&" + PhysicianName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "") + "-Disable_Tab=" + JsonConvert.SerializeObject(tabs_to_disable);
                }
                else
                    PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + HumanID + "&PhysicianID=" + ClientSession.PhysicianId + "&" + PhysicianName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "") + "-Disable_Tab=" + "";
            }
            return PFSHvalue;
        }



        [WebMethod(EnableSession = true)]
        public string LoadPFSHtabMenu(string HumanID, string OpeningFrom, string PhysicianName, string PhysicianID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string PFSHvalue = string.Empty;
            IList<Encounter> enclst = new List<Encounter>();
            SecurityServiceUtility SecurityService = new SecurityServiceUtility();
            IList<string> PFSH_tab_to_disable = SecurityService.GetListTabtoDisable("frmPFSH");
            //string FileName = "Human" + "_" + HumanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["XMLPath"], FileName);
            //string FileNamenew = string.Empty;
            //string strXmlFilePathmew = string.Empty;

            if (PhysicianID != string.Empty)
                ClientSession.PhysicianId = Convert.ToUInt32(PhysicianID);
            string Source_of_info = string.Empty;
            string PFSHVerified = string.Empty;
            string OthersVAl = string.Empty;
            UIManager.PFSH_OpeingFrom = OpeningFrom;
            if (PhysicianName == string.Empty)
                PhysicianName = ClientSession.UserName;
            // PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + HumanID + "&PhysicianID=" + ClientSession.PhysicianId + "&" + PhysicianName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
            if (PFSH_tab_to_disable != null && PFSH_tab_to_disable.Count > 0)
            {
                var tabs_to_disable = PFSH_tab_to_disable.Select(a => new
                {
                    tab = a
                });
                PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + HumanID + "&PhysicianID=" + ClientSession.PhysicianId + "&" + PhysicianName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "") + "-Disable_Tab=" + JsonConvert.SerializeObject(tabs_to_disable);
            }
            else
                PFSHvalue = "OpeingFrom=" + UIManager.PFSH_OpeingFrom + "&HumanID=" + HumanID + "&PhysicianID=" + ClientSession.PhysicianId + "&" + PhysicianName + "&" + ClientSession.UserCurrentProcess + "~" + Source_of_info + "~" + PFSHVerified + "~" + OthersVAl + "~" + ClientSession.EncounterId + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "") + "-Disable_Tab=" + "";
            return PFSHvalue;
        }
        [WebMethod(EnableSession = true)]
        public string UpdateMaritalStatusSocialHistory(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            SocialHistoryDTO problemDTO;
            SocialHistoryManager socialHistoryMngr = new SocialHistoryManager();
            problemDTO = socialHistoryMngr.GetSocialHistoryByHumanID(ClientSession.HumanId, ClientSession.EncounterId, "SOCIAL HISTORY", true);
            if (problemDTO.SocialList != null && problemDTO.SocialList.Count > 0)
            {
                IList<SocialHistory> historyLst = problemDTO.SocialList;
                var maritalRec = from objHistory in historyLst where objHistory.Social_Info.ToUpper() == "MARITAL STATUS" select objHistory;
                if (maritalRec.ToList<SocialHistory>().Count == 0)
                {
                    HumanManager humanMngr = new HumanManager();
                    IList<Human> ilstHuman = humanMngr.GetPatientDetailsUsingPatientInformattion(ClientSession.HumanId);
                    SocialHistory obj = new SocialHistory();
                    if (ilstHuman[0].Marital_Status != string.Empty)
                    {
                        obj.Human_ID = ClientSession.HumanId;
                        obj.Encounter_ID = ClientSession.EncounterId;
                        obj.Social_Info = "Marital Status";
                        obj.Value = ilstHuman[0].Marital_Status;
                        obj.Is_Present = "Y";
                        obj.Created_By = ClientSession.UserName;
                        //string strtime = hdnDateTime.Value.ToString().Split('G').ElementAt(0).ToString();
                        obj.Created_Date_And_Time = Convert.ToDateTime(data);
                        historyLst.Clear();
                        historyLst.Add(obj);
                        socialHistoryMngr.SaveUpdateDeleteWithTransaction(ref historyLst, null, null, string.Empty);
                    }

                }
                GenerateXml XMLObj = new GenerateXml();
                if (historyLst != null && historyLst.Count > 0)
                {
                    ulong encounterid = historyLst[0].Encounter_ID;
                    List<object> lstObj = historyLst.Cast<object>().ToList();
                    //XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);//not in use
                }
            }
            else
            {
                HumanManager humanMngr = new HumanManager();
                IList<Human> ilstHuman = humanMngr.GetPatientDetailsUsingPatientInformattion(ClientSession.HumanId);
                SocialHistory obj = new SocialHistory();
                IList<SocialHistory> historyLst = new List<SocialHistory>();
                if (ilstHuman != null && ilstHuman.Count > 0 && ilstHuman[0].Marital_Status != string.Empty)
                {
                    obj.Human_ID = ClientSession.HumanId;
                    obj.Encounter_ID = ClientSession.EncounterId;
                    obj.Social_Info = "Marital Status";
                    obj.Value = ilstHuman[0].Marital_Status;
                    obj.Is_Present = "Y";
                    obj.Created_By = ClientSession.UserName;
                    // string strtime = hdnDateTime.Value.ToString().Split('G').ElementAt(0).ToString();
                    obj.Created_Date_And_Time = Convert.ToDateTime(data);
                    historyLst.Clear();
                    historyLst.Add(obj);
                    socialHistoryMngr.SaveUpdateDeleteWithTransaction(ref historyLst, null, null, string.Empty);
                }


                GenerateXml XMLObj = new GenerateXml();
                if (historyLst != null && historyLst.Count > 0)
                {
                    ulong encounterid = historyLst[0].Encounter_ID;
                    List<object> lstObj = historyLst.Cast<object>().ToList();
                    // XMLObj.GenerateXmlSave(lstObj, encounterid, string.Empty);//not in use
                }
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string LoadPatientHistory()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<PastMedicalHistory> PastMedicalHistorylst = new List<PastMedicalHistory>();
            GeneralNotes GeneralNoteslst = new GeneralNotes();

            #region "Modified by balaji.TJ  - 2023-01-05" 
            if (savedLst != null && savedLst.Count > 0 && savedLst[0].Encounter_Id != ClientSession.EncounterId)//&& savedLst[0].Human_ID!=ClientSession.HumanId
            {
                savedLst.Clear();
                savedNotesLst.Clear();
            }

            IList<string> ilstPastMHisList = new List<string>();
            ilstPastMHisList.Add("PastMedicalHistoryList");
            ilstPastMHisList.Add("GeneralNotesPastMedicalHistoryList");
            IList<PastMedicalHistory> lstPMHAllEnc = new List<PastMedicalHistory>();
            IList<GeneralNotes> lstGenNotesAllEnc = new List<GeneralNotes>();
            IList<object> ilstPastMHBlobFinal = new List<object>();
            ilstPastMHBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPastMHisList);
            if (ilstPastMHBlobFinal != null && ilstPastMHBlobFinal.Count > 0)
            {
                if (ilstPastMHBlobFinal[0] != null && ((IList<object>)ilstPastMHBlobFinal[0]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[0]).Count; i++)
                    {
                        lstPMHAllEnc.Add((PastMedicalHistory)((IList<object>)ilstPastMHBlobFinal[0])[i]);
                    }
                }
                if (ilstPastMHBlobFinal[1] != null && ((IList<object>)ilstPastMHBlobFinal[1]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[1]).Count; i++)
                    {
                        lstGenNotesAllEnc.Add((GeneralNotes)((IList<object>)ilstPastMHBlobFinal[1])[i]); 
                    }
                }
            }
            if (lstPMHAllEnc != null && lstPMHAllEnc.Count > 0)
            {
                IList<PastMedicalHistory> lstPMHCurrEnc = new List<PastMedicalHistory>();
                lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == ClientSession.EncounterId select item).ToList<PastMedicalHistory>();
                if (lstPMHCurrEnc != null && lstPMHCurrEnc.Count > 0)
                {
                    PastMedicalHistorylst = lstPMHCurrEnc;
                }
                else
                {
                    ulong maxEncId = 0;
                    IList<ulong> lstEncId = (from item in lstPMHAllEnc select item.Encounter_Id).Distinct().ToList<ulong>();
                    if (lstEncId.Count > 0)
                        maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                    foreach (ulong item in lstEncId)
                        if (item > maxEncId && item < ClientSession.EncounterId)
                            maxEncId = item;
                    lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == maxEncId select item).ToList<PastMedicalHistory>();
                    PastMedicalHistorylst = lstPMHCurrEnc;
                }
            }
            else
                PastMedicalHistorylst = lstPMHAllEnc;

            if (lstGenNotesAllEnc != null && lstGenNotesAllEnc.Count > 0)
            {
                IList<GeneralNotes> lstGenCurrEnc = new List<GeneralNotes>();
                lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == ClientSession.EncounterId select item).ToList<GeneralNotes>();
                if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                    GeneralNoteslst = lstGenCurrEnc[0];
                else
                {
                    ulong maxEncId = 0;
                    IList<ulong> lstEncId = (from item in lstGenNotesAllEnc select item.Encounter_ID).Distinct().ToList<ulong>();
                    if (lstEncId.Count > 0)
                        maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                    foreach (ulong item in lstEncId)
                        if (item > maxEncId && item < ClientSession.EncounterId)
                            maxEncId = item;
                    lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == maxEncId select item).ToList<GeneralNotes>();
                    if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                        GeneralNoteslst = lstGenCurrEnc[0];
                }
            }

            #endregion

            #region "Comment by balaji.TJ  - 2023-01-05"

            //// string FileName = "Base_XML" + "_" + "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string FileNamenew = string.Empty;
            //string strXmlFilePathmew = string.Empty;
            //if (savedLst != null && savedLst.Count > 0 && savedLst[0].Encounter_Id != ClientSession.EncounterId)//&& savedLst[0].Human_ID!=ClientSession.HumanId
            //{
            //    savedLst.Clear();
            //    savedNotesLst.Clear();
            //}
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);
            //        XmlText.Close();
            //        IList<PastMedicalHistory> lstPMHAllEnc = new List<PastMedicalHistory>();
            //        if (itemDoc.GetElementsByTagName("PastMedicalHistoryList")[0] != null)
            //        {

            //            XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("PastMedicalHistoryList")[0].ChildNodes;
            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(PastMedicalHistory));
            //                    PastMedicalHistory PastMedicalHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PastMedicalHistory;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((PastMedicalHistory)PastMedicalHistory).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(PastMedicalHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(PastMedicalHistory, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(PastMedicalHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(PastMedicalHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(PastMedicalHistory, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }
            //                    lstPMHAllEnc.Add(PastMedicalHistory);
            //                }
            //            }
            //        }
            //        if (lstPMHAllEnc != null && lstPMHAllEnc.Count > 0)
            //        {
            //            IList<PastMedicalHistory> lstPMHCurrEnc = new List<PastMedicalHistory>();
            //            lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == ClientSession.EncounterId select item).ToList<PastMedicalHistory>();
            //            if (lstPMHCurrEnc != null && lstPMHCurrEnc.Count > 0)
            //            {
            //                PastMedicalHistorylst = lstPMHCurrEnc;
            //            }
            //            else
            //            {
            //                ulong maxEncId = 0;
            //                IList<ulong> lstEncId = (from item in lstPMHAllEnc select item.Encounter_Id).Distinct().ToList<ulong>();
            //                if (lstEncId.Count > 0)
            //                    maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //                foreach (ulong item in lstEncId)
            //                    if (item > maxEncId && item < ClientSession.EncounterId)
            //                        maxEncId = item;
            //                lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == maxEncId select item).ToList<PastMedicalHistory>();
            //                PastMedicalHistorylst = lstPMHCurrEnc;
            //            }
            //        }
            //        else
            //            PastMedicalHistorylst = lstPMHAllEnc;
            //        IList<GeneralNotes> lstGenNotesAllEnc = new List<GeneralNotes>();
            //        if (itemDoc.GetElementsByTagName("GeneralNotesPastMedicalHistoryList")[0] != null)
            //        {
            //            XmlNodeList xmlTagNameGnrl = itemDoc.GetElementsByTagName("GeneralNotesPastMedicalHistoryList")[0].ChildNodes;

            //            if (xmlTagNameGnrl.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagNameGnrl.Count; j++)
            //                {
            //                    string TagName = xmlTagNameGnrl[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
            //                    GeneralNotes generalnotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagNameGnrl[j])) as GeneralNotes;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((GeneralNotes)generalnotes).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagNameGnrl[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagNameGnrl[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(generalnotes, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(generalnotes, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(generalnotes, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(generalnotes, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(generalnotes, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }
            //                    //GeneralNoteslst = generalnotes;
            //                    lstGenNotesAllEnc.Add(generalnotes);
            //                }
            //            }
            //        }

            //        if (lstGenNotesAllEnc != null && lstGenNotesAllEnc.Count > 0)
            //        {
            //            IList<GeneralNotes> lstGenCurrEnc = new List<GeneralNotes>();
            //            lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == ClientSession.EncounterId select item).ToList<GeneralNotes>();
            //            if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
            //                GeneralNoteslst = lstGenCurrEnc[0];
            //            else
            //            {
            //                ulong maxEncId = 0;
            //                IList<ulong> lstEncId = (from item in lstGenNotesAllEnc select item.Encounter_ID).Distinct().ToList<ulong>();
            //                if (lstEncId.Count > 0)
            //                    maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //                foreach (ulong item in lstEncId)
            //                    if (item > maxEncId && item < ClientSession.EncounterId)
            //                        maxEncId = item;
            //                lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == maxEncId select item).ToList<GeneralNotes>();
            //                if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
            //                    GeneralNoteslst = lstGenCurrEnc[0];
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            #endregion
            if ((PastMedicalHistorylst != null && PastMedicalHistorylst.Count != 0) || (GeneralNoteslst != null))
            {
                var objResult = new object[2];
                if (PastMedicalHistorylst != null && PastMedicalHistorylst.Count != 0)
                {
                    savedLst = PastMedicalHistorylst;
                    var PastMedical = PastMedicalHistorylst.Select(a => new
                    {
                        value = a.Past_Medcial_History_ID,
                        description = a.Past_Medical_Info,
                        frmdate = changeFormat(a.From_Date),
                        todate = changeFormat(a.To_Date),
                        notes = a.Notes,
                        ispresent = a.Is_present,
                        Id = a.Id,
                        version = a.Version
                    });
                    objResult[0] = PastMedical;

                }
                if (GeneralNoteslst != null && GeneralNoteslst.Id != 0)
                {
                    savedNotesLst.Add(GeneralNoteslst);
                }
                objResult[1] = GeneralNoteslst.Notes;
                return JsonConvert.SerializeObject(objResult);
            }

            else
                return JsonConvert.SerializeObject(string.Empty);
        }
        //[WebMethod(EnableSession = true)]
        //public string SavePatientHistory(object[] data)
        //{
        //    if (ClientSession.UserName == string.Empty)
        //    {
        //        HttpContext.Current.Response.StatusCode = 999;
        //        HttpContext.Current.Response.Status = "999 Session Expired";
        //        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //        return "Session Expired";
        //    }
        //    ProblemHistoryDTO pblmHistoryDTO = new ProblemHistoryDTO();
        //    IList<PastMedicalHistory> SaveList = new List<PastMedicalHistory>();
        //    IList<PastMedicalHistory> UpdateList = new List<PastMedicalHistory>();
        //    IList<PastMedicalHistory> DeleteList = new List<PastMedicalHistory>();
        //    PastMedicalHistoryManager objPastMedicalHistoryManager = new PastMedicalHistoryManager();
        //    IList<PastMedicalHistory> Temp = new List<PastMedicalHistory>();
        //    IList<ulong> PMHids = new List<ulong>();
        //    Dictionary<string, string> ICDCodes = new Dictionary<string, string>();
        //    if (data.Length > 3)//when no PastMedicalHistory present
        //    {
        //        for (int i = 0; i < data.Length - 3; i++)
        //        {
        //            PastMedicalHistory objPast = new PastMedicalHistory();
        //            Dictionary<string, object> patientValues = new Dictionary<string, object>();
        //            patientValues = (Dictionary<string, object>)data[i];
        //            objPast.Is_present = patientValues["IsPresent"].ToString();

        //            if (patientValues["FDate"] != null)
        //                objPast.From_Date = changeFormat(patientValues["FDate"].ToString());
        //            else
        //                objPast.From_Date = string.Empty;
        //            if (patientValues["IsCurrent"].ToString() == string.Empty)
        //            {
        //                if (patientValues["TDate"] != null)
        //                    objPast.To_Date = changeFormat(patientValues["TDate"].ToString());
        //                else
        //                    objPast.To_Date = string.Empty;
        //            }
        //            else
        //                objPast.To_Date = patientValues["IsCurrent"].ToString();


        //            string[] sInfo = new string[2];
        //            sInfo[0] = patientValues["ID"].ToString();
        //            sInfo[1] = patientValues["ICDcode"].ToString();
        //            objPast.Past_Medical_Info = sInfo[0];
        //            objPast.AHA_Question_ICD = sInfo[1];
        //            objPast.Human_ID = ClientSession.HumanId;
        //            objPast.Is_Mandatory = "N";
        //            objPast.Notes = patientValues["Notes"].ToString();
        //            objPast.Encounter_Id = ClientSession.EncounterId;
        //            if (!ICDCodes.ContainsKey(objPast.AHA_Question_ICD))
        //            {
        //                ICDCodes.Add(objPast.AHA_Question_ICD, patientValues["ICD9Code"].ToString());
        //            }
        //            if (Convert.ToInt32(patientValues["version"]) > 0)
        //            {
        //                objPast.Version = Convert.ToInt32(patientValues["version"]);
        //            }
        //            objPast.Id = Convert.ToUInt32(patientValues["PMH_id"]);
        //            Temp.Add(objPast);
        //        }
        //    }
        //    if (savedLst == null)
        //        savedLst = new List<PastMedicalHistory>();
        //    if (savedLst.Count == 0)
        //    {
        //        SaveList = Temp;
        //        foreach (PastMedicalHistory obj in SaveList)
        //        {
        //            obj.Created_By = ClientSession.UserName;
        //            obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

        //        }
        //    }
        //    else if (savedLst[0].Encounter_Id != ClientSession.EncounterId)
        //    {
        //        SaveList = Temp;
        //        foreach (PastMedicalHistory obj in SaveList)
        //        {
        //            obj.Created_By = ClientSession.UserName;
        //            obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

        //        }
        //    }
        //    else if (savedLst[0].Encounter_Id == ClientSession.EncounterId)
        //    {
        //        foreach (PastMedicalHistory objPMH in Temp)
        //        {
        //            //HistoryList=savedLst.Where(h=>h.Past_Medical_Info.ToUpper()==objPMH.Past_Medical_Info.ToUpper()).ToList<PastMedicalHistory>();
        //            var obj = (from sList in savedLst where sList.Past_Medical_Info.ToUpper() == objPMH.Past_Medical_Info.ToUpper() select sList).ToList<PastMedicalHistory>();
        //            if (obj.Count > 0)//Update
        //            {
        //                PastMedicalHistory objPast = new PastMedicalHistory();
        //                objPast = (PastMedicalHistory)obj[0];
        //                objPast.AHA_Question_ICD = objPMH.AHA_Question_ICD;
        //                objPast.Is_present = objPMH.Is_present;
        //                objPast.From_Date = objPMH.From_Date;
        //                objPast.To_Date = objPMH.To_Date;
        //                objPast.Past_Medical_Info = objPMH.Past_Medical_Info;
        //                objPast.Human_ID = ClientSession.HumanId;
        //                objPast.Is_Mandatory = "N";
        //                objPast.Notes = objPMH.Notes;
        //                objPast.Encounter_Id = ClientSession.EncounterId;
        //                objPast.Modified_By = ClientSession.UserName;
        //                objPast.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                objPast.Version = objPMH.Version;
        //                UpdateList.Add(objPast);


        //            }
        //            else//Save
        //            {
        //                objPMH.Created_By = ClientSession.UserName;
        //                objPMH.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                SaveList.Add(objPMH);
        //            }
        //        }
        //        if (savedLst.Count > 0 && savedLst[0].Encounter_Id == ClientSession.EncounterId)
        //        {
        //            if (Temp.Count > 0)
        //                DeleteList = (savedLst.Where(p => !Temp.Any(p2 => p2.Past_Medical_Info == p.Past_Medical_Info))).ToList<PastMedicalHistory>();
        //            else
        //                DeleteList = savedLst;
        //        }
        //    }
        //    if (savedNotesLst == null)
        //    {
        //        savedNotesLst = new List<GeneralNotes>();
        //    }
        //    Dictionary<string, object> item = (Dictionary<string, object>)data[data.Length - 3];
        //    GeneralNotes objGNotes = new GeneralNotes();
        //    if (savedNotesLst.Count > 0)
        //    {
        //        if (ClientSession.EncounterId == savedNotesLst[0].Encounter_ID)
        //        {
        //            objGNotes = savedNotesLst[0];
        //        }
        //        else
        //        {
        //            savedNotesLst[0].Id = 0;
        //            savedNotesLst[0].Version = 0;
        //            savedNotesLst[0].Encounter_ID = 0;
        //            objGNotes = savedNotesLst[0];
        //        }
        //    }
        //    objGNotes.Human_ID = ClientSession.HumanId;
        //    objGNotes.Parent_Field = "Past Medical History";
        //    objGNotes.Notes = item["GeneralNotes"].ToString();
        //    objGNotes.Encounter_ID = ClientSession.EncounterId;

        //    Dictionary<string, object> time = (Dictionary<string, object>)data[data.Length - 2];
        //    string strtime = time["LocalTime"].ToString().Split('G').ElementAt(0).ToString();
        //    DateTime utc = Convert.ToDateTime(strtime);

        //    Dictionary<string, object> V_year = (Dictionary<string, object>)data[data.Length - 1];
        //    string Ver_yr = V_year["VersionYr"].ToString();
        //    // objPastMedicalHistoryManager.SaveUpdateDeleteProblemMedicalHistory(SaveList, UpdateList, DeleteList,);

        //    pblmHistoryDTO = objPastMedicalHistoryManager.SaveUpdateDeleteProblemMedicalHistory(SaveList, UpdateList, DeleteList, objGNotes, string.Empty, ClientSession.EncounterId, ClientSession.HumanId, ClientSession.PhysicianId, ClientSession.UserName, utc, "Past Medical History", Ver_yr, ICDCodes);
        //    IList<int> ilstChangeSummaryBar = new List<int>() { 4 };
        //    List<string> lstToolTip = new List<string>();
        //    string ProblemListText = "";
        //    var sProblemList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);
        //    ClientSession.bPFSHVerified = false;
        //    sProblemList = sProblemList.Where(a => a.ToUpper().StartsWith("PROBLEMLIST-")).ToList();
        //    if (sProblemList.Count > 0)
        //        ProblemListText = sProblemList[0].Replace("ProblemList-", "");
        //    if (pblmHistoryDTO.PastMedicalList != null || pblmHistoryDTO.GeneralNotesObject != null)
        //    {
        //        var ObjResult = new object[4];
        //        savedLst = pblmHistoryDTO.PastMedicalList;
        //        savedNotesLst.Add(pblmHistoryDTO.GeneralNotesObject);
        //        var PastMedical = pblmHistoryDTO.PastMedicalList.Select(a => new
        //        {
        //            value = a.Past_Medcial_History_ID,
        //            description = a.Past_Medical_Info,
        //            frmdate = changeFormat(a.From_Date),
        //            todate = changeFormat(a.To_Date),
        //            notes = a.Notes,
        //            ispresent = a.Is_present,
        //            Id = a.Id,
        //            version = a.Version
        //        });
        //        ObjResult[0] = PastMedical;
        //        var genrlNotes = objGNotes.Notes;
        //        ObjResult[1] = genrlNotes;
        //        ObjResult[2] = ProblemListText;
        //        ObjResult[3] = ClientSession.EncounterId;
        //        return JsonConvert.SerializeObject(ObjResult);
        //    }

        //    else
        //    {
        //        savedLst = new List<PastMedicalHistory>();
        //        savedNotesLst = new List<GeneralNotes>();
        //        return JsonConvert.SerializeObject(string.Empty);
        //    }
        //}

        [WebMethod(EnableSession = true)]
        public string PFSH_click(string S_of_info, string hdnDateTime, string txtS_of_info)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            var focus = "false";
            var hdntxtOthersValue = string.Empty;
            ClientSession.bPFSHVerified = true;
            EncList = objEncounterManager.GetEncounterByEncounterID(ClientSession.EncounterId);
            //string FileName = "Base_XML" + "_" + "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //string FileNamenew = string.Empty;
            //string strXmlFilePathmew = string.Empty;

            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();
            //    if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
            //    {

            //        XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;
            //        if (xmlTagName.Count > 0)
            //        {
            //            string TagName = xmlTagName[0].Name;
            //            XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
            //            Encounter Encounter = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[0])) as Encounter;
            //            IEnumerable<PropertyInfo> propInfo = null;
            //            Encounter = (Encounter)Encounter;
            //            propInfo = from obji in ((Encounter)Encounter).GetType().GetProperties() select obji;

            //            for (int i = 0; i < xmlTagName[0].Attributes.Count; i++)
            //            {
            //                XmlNode nodevalue = xmlTagName[0].Attributes[i];
            //                {
            //                    foreach (PropertyInfo property in propInfo)
            //                    {
            //                        if (property.Name == nodevalue.Name)
            //                        {
            //                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                property.SetValue(Encounter, Convert.ToUInt64(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                property.SetValue(Encounter, Convert.ToString(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                property.SetValue(Encounter, Convert.ToDateTime(nodevalue.Value), null);
            //                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                property.SetValue(Encounter, Convert.ToInt32(nodevalue.Value), null);
            //                            else
            //                                property.SetValue(Encounter, nodevalue.Value, null);
            //                        }
            //                    }
            //                }

            //            }
            //            EncList.Add(Encounter);
            //        }
            //    }
            //}
            if (EncList != null && EncList.Count > 0)
            {
                IList<Encounter> UpdateEncounterList = new List<Encounter>();
                Encounter currentEncounter = new Encounter();
                currentEncounter = EncList[0];
                currentEncounter.Is_PFSH_Verified = "Y";
                currentEncounter.Source_Of_Information = S_of_info.ToString();
                currentEncounter.Modified_By = ClientSession.UserName;
                currentEncounter.Local_Time = UtilityManager.ConvertToLocal(currentEncounter.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                if (hdnDateTime.Trim() != string.Empty)
                    currentEncounter.Modified_Date_and_Time = Convert.ToDateTime(hdnDateTime.ToString());
                if (S_of_info == "Other" || S_of_info == "Others")
                {
                    if (txtS_of_info == string.Empty)
                    {
                        focus = "true";
                        return focus;
                    }
                    else
                    {
                        currentEncounter.If_Source_Of_Information_Others = txtS_of_info;
                        hdntxtOthersValue = txtS_of_info;
                    }

                }
                else
                {
                    currentEncounter.If_Source_Of_Information_Others = string.Empty;
                    hdntxtOthersValue = string.Empty;

                }
                UpdateEncounterList.Add(currentEncounter);

                objEncounterManager.UpdateEncounterList(UpdateEncounterList, string.Empty, new object[] { "false" });
                // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('180005');", true);
                // btnPFSH.Enabled = false;
                currentEncounter.Version += 1;
                ClientSession.FillEncounterandWFObject.EncRecord = currentEncounter;
                hdntxtOthersValue = txtS_of_info;
                //GenerateXml XMLObj = new GenerateXml();
                //UpdateEncounterList[0].Local_Time = UtilityManager.ConvertToLocal(UpdateEncounterList[0].Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                //XMLObj.GenerateXmlSave(UpdateEncounterList.Cast<object>().ToList(), ClientSession.EncounterId, string.Empty);
                UIManager.IsPFSHVerified = false;
            }
            //For ACO Validation 
            HumanManager objHumanManagerACO = new HumanManager();
            bool flgVal = objHumanManagerACO.IsACOValid(ClientSession.HumanId);

            // ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SetACOFlag", "SetACOFlag('" + (objHumanManagerACO.IsACOValid(ClientSession.HumanId)).ToString() + "');", true);

            // Session["PFSHEnabled"] = "False";
            // hdnTabPFSHEnable.Value = "";

            UIManager.IsPFSHVerified = false;
            string value = focus + "~" + hdntxtOthersValue + "~" + flgVal;
            return value;
        }
        [WebMethod(EnableSession = true)]
        public string LoadPastMedicalHistory()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sScreenMode = UIManager.PFSH_OpeingFrom;
            IList<PastMedicalHistory> PastMedicalHistorylst = new List<PastMedicalHistory>();
            IList<PastMedicalHistoryMaster> lstPastHisMaster = new List<PastMedicalHistoryMaster>();
            IList<PastMedicalHistoryMaster> lstPastHisMasterTemp = new List<PastMedicalHistoryMaster>();
            GeneralNotes GeneralNoteslst = new GeneralNotes();

            #region "Modified by balaji.TJ  - 2023-01-05" 
            
            IList<string> ilstPastMHisList = new List<string>();
            ilstPastMHisList.Add("PastMedicalHistoryList");
            ilstPastMHisList.Add("ProblemListList");
            ilstPastMHisList.Add("GeneralNotesPastMedicalHistoryList"); 
            ilstPastMHisList.Add("PastMedicalHistoryMasterList");

            IList<PastMedicalHistory> lstPMHAllEnc = new List<PastMedicalHistory>();
            IList<ProblemList> lstAllProblemList = new List<ProblemList>();
            IList<GeneralNotes> lstGenNotesAllEnc = new List<GeneralNotes>();
            IList<object> ilstPastMHBlobFinal = new List<object>();
            ilstPastMHBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPastMHisList);

            if (ilstPastMHBlobFinal != null && ilstPastMHBlobFinal.Count > 0)
            {
                if (ilstPastMHBlobFinal[0] != null && ((IList<object>)ilstPastMHBlobFinal[0]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[0]).Count; i++)
                    {
                        lstPMHAllEnc.Add((PastMedicalHistory)((IList<object>)ilstPastMHBlobFinal[0])[i]);
                    }
                }
                if (ilstPastMHBlobFinal[1] != null && ((IList<object>)ilstPastMHBlobFinal[1]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[1]).Count; i++)
                    {
                        lstAllProblemList.Add((ProblemList)((IList<object>)ilstPastMHBlobFinal[1])[i]);
                    }
                }
                HttpContext.Current.Session["ProblemList"] = lstAllProblemList;

                if (ilstPastMHBlobFinal[2] != null && ((IList<object>)ilstPastMHBlobFinal[2]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[2]).Count; i++)
                    {
                        lstGenNotesAllEnc.Add((GeneralNotes)((IList<object>)ilstPastMHBlobFinal[2])[i]);
                    }
                }

                if (ilstPastMHBlobFinal[3] != null && ((IList<object>)ilstPastMHBlobFinal[3]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[3]).Count; i++)
                    {
                        lstPastHisMasterTemp.Add((PastMedicalHistoryMaster)((IList<object>)ilstPastMHBlobFinal[3])[i]);
                    }

                    if (lstPastHisMasterTemp.Count > 0)
                    {
                        lstPastHisMaster = lstPastHisMasterTemp.Where(p => p.Is_Deleted == "N").ToList();
                    }
                    HttpContext.Current.Session["PastMedicalMaster"] = lstPastHisMaster;
                }

            }

            if (lstPMHAllEnc != null && lstPMHAllEnc.Count > 0)
            {
                IList<PastMedicalHistory> lstPMHCurrEnc = new List<PastMedicalHistory>();
                lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == ClientSession.EncounterId select item).ToList<PastMedicalHistory>();
                if (lstPMHCurrEnc != null && lstPMHCurrEnc.Count > 0)
                    PastMedicalHistorylst = lstPMHCurrEnc;                
            }
            else
                PastMedicalHistorylst = lstPMHAllEnc;

            if (lstGenNotesAllEnc != null && lstGenNotesAllEnc.Count > 0)
            {
                IList<GeneralNotes> lstGenCurrEnc = new List<GeneralNotes>();
                if (ClientSession.EncounterId != 0)
                {
                    lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == ClientSession.EncounterId select item).ToList<GeneralNotes>();
                    if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                    {
                        GeneralNoteslst = lstGenCurrEnc[0];
                    }
                    else
                    {   //For Git Id : 1162
                        ulong maxEncId = 0;
                        IList<ulong> lstEncId = (from item in lstGenNotesAllEnc select item.Encounter_ID).Distinct().ToList<ulong>();
                        if (lstEncId.Count > 0)
                            maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                        foreach (ulong item in lstEncId)
                            if (item > maxEncId && item < ClientSession.EncounterId)
                                maxEncId = item;
                        lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == maxEncId select item).ToList<GeneralNotes>();
                        if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                        {
                            GeneralNoteslst = lstGenCurrEnc[0];
                        }
                    }
                }               
            }
            
            #endregion

            #region "code comment by balaji.TJ  - 2023-01-05" 
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["XMLPath"], FileName);

            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        IList<PastMedicalHistory> lstPMHAllEnc = new List<PastMedicalHistory>();
            //        if (itemDoc.GetElementsByTagName("PastMedicalHistoryList")[0] != null)
            //        {
            //            PastMedicalHistory objPastMedicalHistory = new PastMedicalHistory();
            //            XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("PastMedicalHistoryList")[0].ChildNodes;
            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(PastMedicalHistory));
            //                    objPastMedicalHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PastMedicalHistory;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((PastMedicalHistory)objPastMedicalHistory).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(objPastMedicalHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(objPastMedicalHistory, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(objPastMedicalHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(objPastMedicalHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(objPastMedicalHistory, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }
            //                    lstPMHAllEnc.Add(objPastMedicalHistory);
            //                }

            //            }
            //        }

            //        if (lstPMHAllEnc != null && lstPMHAllEnc.Count > 0)
            //        {
            //            IList<PastMedicalHistory> lstPMHCurrEnc = new List<PastMedicalHistory>();
            //            lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == ClientSession.EncounterId select item).ToList<PastMedicalHistory>();
            //            if (lstPMHCurrEnc != null && lstPMHCurrEnc.Count > 0)
            //                PastMedicalHistorylst = lstPMHCurrEnc;
            //            //Now its load from Master Table.V dont look the previous encounter from past medical history table
            //            //else
            //            //{
            //            //    ulong maxEncId = 0;
            //            //    IList<ulong> lstEncId = (from item in lstPMHAllEnc select item.Encounter_Id).Distinct().ToList<ulong>();
            //            //    if (lstEncId.Count > 0)
            //            //        maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //            //    foreach (ulong item in lstEncId)
            //            //        if (item > maxEncId && item < ClientSession.EncounterId)
            //            //            maxEncId = item;
            //            //    lstPMHCurrEnc = (from item in lstPMHAllEnc where item.Encounter_Id == maxEncId select item).ToList<PastMedicalHistory>();
            //            //    PastMedicalHistorylst = lstPMHCurrEnc;
            //            //}
            //        }
            //        else
            //            PastMedicalHistorylst = lstPMHAllEnc;
            //        IList<ProblemList> lstAllProblemList = new List<ProblemList>();
            //        if (itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
            //        {
            //            XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;
            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
            //                    ProblemList PbList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((ProblemList)PbList).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(PbList, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(PbList, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(PbList, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(PbList, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(PbList, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }
            //                    //SavedProblemListlstAllProblemList.Add(PbList);
            //                    lstAllProblemList.Add(PbList);
            //                }
            //            }
            //        }

            //        //if (lstAllProblemList.Count > 0)
            //        //{
            //        HttpContext.Current.Session["ProblemList"] = lstAllProblemList;
            //        //}



            //        IList<GeneralNotes> lstGenNotesAllEnc = new List<GeneralNotes>();
            //        if (itemDoc.GetElementsByTagName("GeneralNotesPastMedicalHistoryList")[0] != null)
            //        {
            //            XmlNodeList xmlTagNameGnrl = itemDoc.GetElementsByTagName("GeneralNotesPastMedicalHistoryList")[0].ChildNodes;

            //            if (xmlTagNameGnrl.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagNameGnrl.Count; j++)
            //                {
            //                    string TagName = xmlTagNameGnrl[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
            //                    GeneralNotes generalnotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagNameGnrl[j])) as GeneralNotes;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((GeneralNotes)generalnotes).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagNameGnrl[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagNameGnrl[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(generalnotes, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(generalnotes, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(generalnotes, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(generalnotes, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(generalnotes, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }
            //                    lstGenNotesAllEnc.Add(generalnotes);
            //                }
            //            }
            //        }
            //        if (lstGenNotesAllEnc != null && lstGenNotesAllEnc.Count > 0)
            //        {
            //            IList<GeneralNotes> lstGenCurrEnc = new List<GeneralNotes>();
            //            if (ClientSession.EncounterId != 0)
            //            {
            //                lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == ClientSession.EncounterId select item).ToList<GeneralNotes>();
            //                if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
            //                {
            //                    GeneralNoteslst = lstGenCurrEnc[0];
            //                }
            //                else
            //                {   //For Git Id : 1162
            //                    ulong maxEncId = 0;
            //                    IList<ulong> lstEncId = (from item in lstGenNotesAllEnc select item.Encounter_ID).Distinct().ToList<ulong>();
            //                    if (lstEncId.Count > 0)
            //                        maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //                    foreach (ulong item in lstEncId)
            //                        if (item > maxEncId && item < ClientSession.EncounterId)
            //                            maxEncId = item;
            //                    lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == maxEncId select item).ToList<GeneralNotes>();
            //                    if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
            //                    {
            //                        GeneralNoteslst = lstGenCurrEnc[0];
            //                    }
            //                }
            //            }
            //            //else
            //            //{
            //            //ulong maxEncId = 0;
            //            //IList<ulong> lstEncId = (from item in lstGenNotesAllEnc select item.Encounter_ID).Distinct().ToList<ulong>();
            //            //if (lstEncId.Count > 0)
            //            //    maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //            //foreach (ulong item in lstEncId)
            //            //    if (item > maxEncId && item < ClientSession.EncounterId)
            //            //        maxEncId = item;
            //            //lstGenCurrEnc = (from item in lstGenNotesAllEnc where item.Encounter_ID == maxEncId select item).ToList<GeneralNotes>();
            //            //if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
            //            //{
            //            //    GeneralNoteslst = lstGenCurrEnc[0];
            //            //}
            //            // }
            //        }

            //        //Master Table load
            //        if (itemDoc.GetElementsByTagName("PastMedicalHistoryMasterList")[0] != null)
            //        {
            //            XmlNodeList xmlTagName = itemDoc.GetElementsByTagName("PastMedicalHistoryMasterList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(PastMedicalHistoryMaster));
            //                    PastMedicalHistoryMaster objHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PastMedicalHistoryMaster;

            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((PastMedicalHistoryMaster)objHistory).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            if (propInfo != null && propInfo.Count() > 0)
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {

            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(objHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(objHistory, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(objHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(objHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(objHistory, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    lstPastHisMasterTemp.Add(objHistory);
            //                }
            //            }
            //            if (lstPastHisMasterTemp.Count > 0)
            //            {
            //                lstPastHisMaster = lstPastHisMasterTemp.Where(p => p.Is_Deleted == "N").ToList();


            //                //IList<PastMedicalHistory> PastMedicalHistorylstTemp = new List<PastMedicalHistory>();

            //                //if (lstPastHisMaster != null && lstPastHisMaster.Count() > 0)
            //                //{
            //                //    foreach (PastMedicalHistoryMaster objMaster in lstPastHisMaster)
            //                //    {
            //                //        PastMedicalHistory objtempFH = new PastMedicalHistory();
            //                //        objtempFH.Human_ID = objMaster.Human_ID;
            //                //        objtempFH.Past_Medical_Info = objMaster.Past_Medical_Info;
            //                //        objtempFH.From_Date = objMaster.From_Date;
            //                //        objtempFH.To_Date = objMaster.To_Date;
            //                //        objtempFH.Is_present = objMaster.Is_present;
            //                //        objtempFH.AHA_Question_ICD = objMaster.AHA_Question_ICD;
            //                //        objtempFH.Is_Mandatory = objMaster.Is_Mandatory;
            //                //        objtempFH.Notes = objMaster.Notes;
            //                //        objtempFH.Created_By = ClientSession.UserName;
            //                //        objtempFH.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
            //                //        objtempFH.Encounter_Id = ClientSession.EncounterId;
            //                //        PastMedicalHistorylstTemp.Add(objtempFH);
            //                //    }
            //                //    HttpContext.Current.Session["PastMedicalMaster"] = PastMedicalHistorylstTemp;

            //                //}

            //            }
            //            HttpContext.Current.Session["PastMedicalMaster"] = lstPastHisMaster;
            //        }
            //        fs.Close();
            //        fs.Dispose();

            //    }
            //}
            #endregion
            //Load Data for Menu level and fresh encounter from master table 
            if ((sScreenMode != "" && sScreenMode.ToUpper() == "MENU") || (PastMedicalHistorylst != null && PastMedicalHistorylst.Count == 0))
            {
                // var objResult = new object[2];//Object[5] because same script function used for save and load.
                var objResult = new object[5];
                if (((PastMedicalHistorylst != null && PastMedicalHistorylst.Count != 0) || (GeneralNoteslst != null)))
                {
                    if (lstPastHisMaster != null && lstPastHisMaster.Count != 0)
                    {

                        HttpContext.Current.Session["PastMedicalMaster"] = lstPastHisMaster;
                        var PastMedicalMaster = lstPastHisMaster.Select(a => new
                        {

                            description = a.Past_Medical_Info,
                            frmdate = changeFormat(a.From_Date),
                            todate = changeFormat(a.To_Date),
                            notes = a.Notes,
                            ispresent = a.Is_present,
                            Id = "0",
                            version = a.Version
                        });
                        objResult[0] = PastMedicalMaster;
                    }
                    if (GeneralNoteslst != null && GeneralNoteslst.Id != 0)
                    {
                        HttpContext.Current.Session["GeneralNotesList"] = GeneralNoteslst;
                    }
                    objResult[1] = GeneralNoteslst.Notes;
                    objResult[4] = sScreenMode;
                }
                return JsonConvert.SerializeObject(objResult);
            }
            else if (((PastMedicalHistorylst != null && PastMedicalHistorylst.Count != 0) || (GeneralNoteslst != null)))
            {
                var objResult = new object[5];
                if (PastMedicalHistorylst != null && PastMedicalHistorylst.Count != 0)
                {
                    //SavedPastMedicalList = PastMedicalHistorylst;
                    HttpContext.Current.Session["PastMedicalList"] = PastMedicalHistorylst;
                    var PastMedical = PastMedicalHistorylst.Select(a => new
                    {
                        value = a.Past_Medcial_History_ID,
                        description = a.Past_Medical_Info,
                        frmdate = changeFormat(a.From_Date),
                        todate = changeFormat(a.To_Date),
                        notes = a.Notes,
                        ispresent = a.Is_present,
                        Id = a.Id,
                        version = a.Version
                    });
                    objResult[0] = PastMedical;

                }
                if (GeneralNoteslst != null && GeneralNoteslst.Id != 0)
                {
                    //SavedGeneralNotesList.Add(GeneralNoteslst);
                    HttpContext.Current.Session["GeneralNotesList"] = GeneralNoteslst;
                }
                objResult[1] = GeneralNoteslst.Notes;
                objResult[4] = sScreenMode;
                return JsonConvert.SerializeObject(objResult);
            }

            else
                return JsonConvert.SerializeObject(string.Empty);

        }

        [WebMethod(EnableSession = true)]
        public string SavePastMedicalHistory(object[] data, object[] Mod_lst)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<PastMedicalHistory> SavedPastMedicalList = new List<PastMedicalHistory>();
            IList<GeneralNotes> SavedGeneralNotesList = new List<GeneralNotes>();
            IList<ProblemList> SavedProblemList = new List<ProblemList>();
            ProblemHistoryDTO pblmHistoryDTO = new ProblemHistoryDTO();
            IList<PastMedicalHistory> SaveList = new List<PastMedicalHistory>();
            IList<PastMedicalHistory> UpdateList = new List<PastMedicalHistory>();
            IList<PastMedicalHistory> DeleteList = new List<PastMedicalHistory>();
            PastMedicalHistoryManager objPastMedicalHistoryManager = new PastMedicalHistoryManager();
            IList<PastMedicalHistory> CurrentPastMedList = new List<PastMedicalHistory>();
            IList<ulong> PMHids = new List<ulong>();
            Dictionary<string, string> ICDCodes = new Dictionary<string, string>();
            string sScreenMode = UIManager.PFSH_OpeingFrom;

            IList<PastMedicalHistoryMaster> CurrentPastMedListMaster = new List<PastMedicalHistoryMaster>();
            IList<PastMedicalHistoryMaster> DeleteListMasterTemp = new List<PastMedicalHistoryMaster>();



            if (data.Length > 3)//when no PastMedicalHistory present
            {
                for (int i = 0; i < data.Length - 3; i++)
                {
                    PastMedicalHistory objPast = new PastMedicalHistory();
                    Dictionary<string, object> patientValues = new Dictionary<string, object>();
                    patientValues = (Dictionary<string, object>)data[i];
                    objPast.Id = Convert.ToUInt32(patientValues["PMH_id"]);
                    objPast.Is_present = Convert.ToString(patientValues["IsPresent"]);
                    if (patientValues["FDate"] != null)
                        objPast.From_Date = changeFormat(patientValues["FDate"].ToString());
                    else
                        objPast.From_Date = string.Empty;
                    if (patientValues["IsCurrent"].ToString() == string.Empty)
                    {
                        if (patientValues["TDate"] != null)
                            objPast.To_Date = changeFormat(patientValues["TDate"].ToString());
                        else
                            objPast.To_Date = string.Empty;
                    }
                    else
                        objPast.To_Date = Convert.ToString(patientValues["IsCurrent"]);
                    objPast.Past_Medical_Info = Convert.ToString(patientValues["ID"]);
                    objPast.AHA_Question_ICD = Convert.ToString(patientValues["ICDcode"]);
                    objPast.Human_ID = ClientSession.HumanId;
                    objPast.Is_Mandatory = "N";
                    objPast.Notes = Convert.ToString(patientValues["Notes"]);
                    objPast.Encounter_Id = ClientSession.EncounterId;
                    objPast.Created_By = ClientSession.UserName;
                    objPast.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    if (!ICDCodes.ContainsKey(objPast.AHA_Question_ICD))
                    {
                        ICDCodes.Add(objPast.AHA_Question_ICD, patientValues["ICD9Code"].ToString());
                    }
                    CurrentPastMedList.Add(objPast);
                }
            }

            #region PastMedicalList
            if (HttpContext.Current.Session["PastMedicalList"] != null)
            {
                SavedPastMedicalList = (IList<PastMedicalHistory>)HttpContext.Current.Session["PastMedicalList"];
            }
            if (SavedPastMedicalList == null)
                SavedPastMedicalList = new List<PastMedicalHistory>();
            if (SavedPastMedicalList.Count == 0)
            {
                SaveList = CurrentPastMedList;
            }
            else if (SavedPastMedicalList[0].Encounter_Id != ClientSession.EncounterId)
            {
                SaveList = CurrentPastMedList;
            }
            else if (SavedPastMedicalList[0].Encounter_Id == ClientSession.EncounterId)
            {
                foreach (PastMedicalHistory objPMH in CurrentPastMedList)
                {
                    //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                    var obj = (from sList in SavedPastMedicalList where sList.Past_Medical_Info.Trim().ToUpper() == objPMH.Past_Medical_Info.Trim().ToUpper() select sList).ToList<PastMedicalHistory>();
                    if (obj.Count > 0)//Update
                    {
                        PastMedicalHistory objPast = new PastMedicalHistory();
                        objPast = (PastMedicalHistory)obj[0];
                        objPast.AHA_Question_ICD = objPMH.AHA_Question_ICD;
                        objPast.Is_present = objPMH.Is_present;
                        objPast.From_Date = objPMH.From_Date;
                        objPast.To_Date = objPMH.To_Date;
                        objPast.Past_Medical_Info = objPMH.Past_Medical_Info;
                        objPast.Human_ID = ClientSession.HumanId;
                        objPast.Is_Mandatory = "N";
                        objPast.Notes = objPMH.Notes;
                        objPast.Encounter_Id = ClientSession.EncounterId;
                        objPast.Modified_By = ClientSession.UserName;
                        objPast.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateList.Add(objPast);
                    }
                    else//Save
                    {
                        objPMH.Created_By = ClientSession.UserName;
                        objPMH.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        SaveList.Add(objPMH);
                    }
                }
                if (SavedPastMedicalList.Count > 0 && SavedPastMedicalList[0].Encounter_Id == ClientSession.EncounterId)
                {
                    if (CurrentPastMedList.Count > 0)
                    {
                        //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                        DeleteList = (SavedPastMedicalList.Where(p => !CurrentPastMedList.Any(p2 => p2.Past_Medical_Info.Trim().ToUpper() == p.Past_Medical_Info.Trim().ToUpper()))).ToList<PastMedicalHistory>();
                    }
                    else
                        DeleteList = SavedPastMedicalList;
                }
            }
            //Menu
            #region GetMastertable
            IList<PastMedicalHistoryMaster> lstPastHisMaster = new List<PastMedicalHistoryMaster>();
            IList<PastMedicalHistoryMaster> lstPastHisMasterTemp = new List<PastMedicalHistoryMaster>();
            //if (HttpContext.Current.Session["PastMedicalMaster"] != null)
            //{
            //    lstPastHisMaster = (IList<PastMedicalHistoryMaster>)HttpContext.Current.Session["PastMedicalMaster"];
            //}
            //else
            //{
            #region "Modified by balaji.TJ  - 2023-01-05" 

            IList<string> ilstPastMHisList = new List<string>();            
            ilstPastMHisList.Add("PastMedicalHistoryMasterList");
           
            IList<object> ilstPastMHBlobFinal = new List<object>();
            ilstPastMHBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPastMHisList);

            if (ilstPastMHBlobFinal != null && ilstPastMHBlobFinal.Count > 0)
            {
                if (ilstPastMHBlobFinal[0] != null && ((IList<object>)ilstPastMHBlobFinal[0]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstPastMHBlobFinal[0]).Count; i++)
                    {
                        lstPastHisMasterTemp.Add((PastMedicalHistoryMaster)((IList<object>)ilstPastMHBlobFinal[0])[i]);
                    }
                }
            }
            if (lstPastHisMasterTemp !=null && lstPastHisMasterTemp.Count > 0)
            {
                lstPastHisMaster = lstPastHisMasterTemp.Where(p => p.Is_Deleted == "N").ToList();
                HttpContext.Current.Session["PastMedicalMaster"] = lstPastHisMaster;                
            }
            #endregion

            #region "Code comment by balaji.TJ  - 2023-01-05" 
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //XmlDocument itemDoc = new XmlDocument();
            //XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //XmlNodeList xmlTagName = null;
            //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    itemDoc.Load(fs);
            //    XmlText.Close();
            //    if (itemDoc.GetElementsByTagName("PastMedicalHistoryMasterList")[0] != null)
            //    {
            //        xmlTagName = itemDoc.GetElementsByTagName("PastMedicalHistoryMasterList")[0].ChildNodes;

            //        if (xmlTagName.Count > 0)
            //        {
            //            for (int j = 0; j < xmlTagName.Count; j++)
            //            {
            //                XmlSerializer xmlserializer = new XmlSerializer(typeof(PastMedicalHistoryMaster));
            //                PastMedicalHistoryMaster objHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PastMedicalHistoryMaster;

            //                IEnumerable<PropertyInfo> propInfo = null;
            //                propInfo = from obji in ((PastMedicalHistoryMaster)objHistory).GetType().GetProperties() select obji;

            //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                {
            //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                    {
            //                        if (propInfo != null && propInfo.Count() > 0)
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {

            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(objHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(objHistory, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(objHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(objHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(objHistory, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                lstPastHisMasterTemp.Add(objHistory);
            //            }
            //        }
            //    }
            //    fs.Close();
            //    fs.Dispose();
            //}

            //if (lstPastHisMasterTemp.Count > 0)
            //{
            //    lstPastHisMaster = lstPastHisMasterTemp.Where(p => p.Is_Deleted == "N").ToList();
            //    HttpContext.Current.Session["PastMedicalMaster"] = lstPastHisMaster;
            //    //IList<PastMedicalHistory> PastMedicalHistorylstTemp = new List<PastMedicalHistory>();

            //    //if (lstPastHisMaster != null && lstPastHisMaster.Count() > 0)
            //    //{
            //    //    foreach (PastMedicalHistoryMaster objMaster in lstPastHisMaster)
            //    //    {
            //    //        PastMedicalHistory objtempFH = new PastMedicalHistory();
            //    //        objtempFH.Human_ID = objMaster.Human_ID;
            //    //        objtempFH.Past_Medical_Info = objMaster.Past_Medical_Info;
            //    //        objtempFH.From_Date = objMaster.From_Date;
            //    //        objtempFH.To_Date = objMaster.To_Date;
            //    //        objtempFH.Is_present = objMaster.Is_present;
            //    //        objtempFH.AHA_Question_ICD = objMaster.AHA_Question_ICD;
            //    //        objtempFH.Is_Mandatory = objMaster.Is_Mandatory;
            //    //        objtempFH.Notes = objMaster.Notes;
            //    //        objtempFH.Created_By = ClientSession.UserName;
            //    //        objtempFH.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
            //    //        objtempFH.Encounter_Id = ClientSession.EncounterId;
            //    //        PastMedicalHistorylstTemp.Add(objtempFH);
            //    //    }
            //    //    HttpContext.Current.Session["PastMedicalMaster"] = PastMedicalHistorylstTemp;

            //    //}
            //}
            //// }
            #endregion
            #endregion
            if (UIManager.PFSH_OpeingFrom == "Menu" || (HttpContext.Current.Session["PastMedicalList"] == null && HttpContext.Current.Session["PastMedicalMaster"] != null))
            {
                if (HttpContext.Current.Session["PastMedicalMaster"] != null)
                {
                    CurrentPastMedListMaster = (IList<PastMedicalHistoryMaster>)HttpContext.Current.Session["PastMedicalMaster"];
                    if (CurrentPastMedListMaster.Count > 0)
                    {
                        //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                        DeleteListMasterTemp = (CurrentPastMedListMaster.Where(p => !SaveList.Any(p2 => p2.Past_Medical_Info.Trim().ToUpper() == p.Past_Medical_Info.Trim().ToUpper()))).ToList<PastMedicalHistoryMaster>();
                    }
                }
            }

            #endregion

            #region GeneralNotes
            if (HttpContext.Current.Session["GeneralNotesList"] != null)
            {
                SavedGeneralNotesList.Add((GeneralNotes)HttpContext.Current.Session["GeneralNotesList"]);
            }
            if (SavedGeneralNotesList == null)
            {
                SavedGeneralNotesList = new List<GeneralNotes>();
            }
            IList<GeneralNotes> SaveGeneralNotes = new List<GeneralNotes>();
            IList<GeneralNotes> UpdateGeneralNotes = new List<GeneralNotes>();
            Dictionary<string, object> item = (Dictionary<string, object>)data[data.Length - 3];
            GeneralNotes objGNotes = new GeneralNotes();
            objGNotes.Human_ID = ClientSession.HumanId;
            objGNotes.Parent_Field = "Past Medical History";
            objGNotes.Notes = Convert.ToString(item["GeneralNotes"]);
            objGNotes.Encounter_ID = ClientSession.EncounterId;
            Dictionary<string, object> time = (Dictionary<string, object>)data[data.Length - 2];
            string strtime = time["LocalTime"].ToString().Split('G').ElementAt(0).ToString();
            DateTime utc = Convert.ToDateTime(strtime);
            if (SavedGeneralNotesList.Count == 0)
            {
                objGNotes.Created_By = ClientSession.UserName;
                objGNotes.Created_Date_And_Time = utc;
                SaveGeneralNotes.Add(objGNotes);
            }
            else if (SavedGeneralNotesList.Count > 0)
            {
                if (ClientSession.EncounterId == SavedGeneralNotesList[0].Encounter_ID)
                {
                    objGNotes = SavedGeneralNotesList[0];
                    objGNotes.Notes = Convert.ToString(item["GeneralNotes"]);
                    objGNotes.Modified_By = ClientSession.UserName;
                    objGNotes.Modified_Date_And_Time = utc;
                    UpdateGeneralNotes.Add(objGNotes);
                }
                else
                {
                    objGNotes.Created_By = ClientSession.UserName;
                    objGNotes.Created_Date_And_Time = utc;
                    SaveGeneralNotes.Add(objGNotes);
                }
            }
            #endregion

            #region ProblemList
            if (HttpContext.Current.Session["ProblemList"] != null)
            {
                SavedProblemList = (IList<ProblemList>)HttpContext.Current.Session["ProblemList"];
            }
            IList<ProblemList> SaveProblemList = new List<ProblemList>();
            IList<ProblemList> UpdateProblemList = new List<ProblemList>();

            //to prevent occurence of unmodified ICD's in IncompleteProblemList of Assessment
            IList<string> modified_Controls = new List<string>();
            foreach (object obj in Mod_lst)
            {
                modified_Controls.Add(obj.ToString());
            }

            string[] Others = { "Other1", "Other2", "Other3", "Other4", "Other5" };
            for (int i = 0; i < CurrentPastMedList.Count; i++)
            {
                ProblemList ObjProblem = new ProblemList();
                if (!Others.Contains(CurrentPastMedList[i].Past_Medical_Info))
                {
                    if (SavedProblemList.Any(s => s.ICD == CurrentPastMedList[i].AHA_Question_ICD))
                    {
                        string ProblemStatus = CurrentPastMedList[i].To_Date == "Current" ? "Active" : CurrentPastMedList[i].To_Date != string.Empty ? "Resolved" : CurrentPastMedList[i].From_Date == string.Empty ? "Inactive" : CurrentPastMedList[i].To_Date == string.Empty ? "Active" : string.Empty;
                        ObjProblem = SavedProblemList.Where(a => a.ICD == CurrentPastMedList[i].AHA_Question_ICD).ToList<ProblemList>()[0];
                        if ((modified_Controls.Contains(ObjProblem.ICD)) && (ObjProblem.Date_Diagnosed != CurrentPastMedList[i].From_Date || ObjProblem.Status != ProblemStatus || ObjProblem.Is_Active != CurrentPastMedList[i].Is_present || SavedPastMedicalList.Any(p => p.AHA_Question_ICD == CurrentPastMedList[i].AHA_Question_ICD && p.Notes != CurrentPastMedList[i].Notes)))//(modified_Controls.Contains(ObjProblem.ICD) to check if the problemlist icd is actually modified before changing Is_Active to 'Y'
                        {
                            ObjProblem.Status = ProblemStatus;
                            ObjProblem.Date_Diagnosed = CurrentPastMedList[i].From_Date;
                            if (ObjProblem.Reference_Source.Contains("Deleted"))
                                ObjProblem.Reference_Source = ObjProblem.Reference_Source.Replace("|Deleted", "");
                            if (!ObjProblem.Reference_Source.Contains("AHA Questionnaire"))
                            {
                                ObjProblem.Reference_Source += "|AHA Questionnaire";
                            }

                            ObjProblem.Is_Active = CurrentPastMedList[i].Is_present;
                            ObjProblem.Version_Year = "ICD_10";
                            ObjProblem.Modified_By = ClientSession.UserName;
                            ObjProblem.Modified_Date_And_Time = utc;
                            UpdateProblemList.Add(ObjProblem);
                        }
                    }
                    else
                    {
                        if (CurrentPastMedList[i].Is_present == "Y")
                        {
                            ObjProblem.Encounter_ID = ClientSession.EncounterId;
                            ObjProblem.Human_ID = ClientSession.HumanId;
                            ObjProblem.Physician_ID = ClientSession.PhysicianId;
                            ObjProblem.ICD = CurrentPastMedList[i].AHA_Question_ICD;
                            ObjProblem.Problem_Description = CurrentPastMedList[i].Past_Medical_Info;
                            ObjProblem.Status = CurrentPastMedList[i].To_Date == "Current" ? "Active" : CurrentPastMedList[i].To_Date != string.Empty ? "Resolved" : CurrentPastMedList[i].From_Date == string.Empty ? "Inactive" : CurrentPastMedList[i].To_Date == string.Empty ? "Active" : string.Empty;
                            ObjProblem.Date_Diagnosed = CurrentPastMedList[i].From_Date;
                            ObjProblem.Is_Active = CurrentPastMedList[i].Is_present;
                            ObjProblem.Reference_Source = "AHA Questionnaire";
                            ObjProblem.Version_Year = "ICD_10";
                            if (ICDCodes.ContainsKey(ObjProblem.ICD))
                            {
                                ObjProblem.ICD_9 = ICDCodes[ObjProblem.ICD].ToString();
                                ObjProblem.ICD_9_Description = ObjProblem.Problem_Description;
                            }
                            ObjProblem.Created_By = ClientSession.UserName;
                            ObjProblem.Created_Date_And_Time = utc;
                            SaveProblemList.Add(ObjProblem);
                        }
                    }
                }
            }
            if (DeleteList != null && DeleteList.Count > 0)//Delete ProblemList
            {
                foreach (var DeleteItem in DeleteList)
                {
                    if (!Others.Contains(DeleteItem.Past_Medical_Info))
                    {
                        if (SavedProblemList.Any(a => a.ICD == DeleteItem.AHA_Question_ICD))
                        {
                            ProblemList ProblemLst = SavedProblemList.Where(a => a.ICD == DeleteItem.AHA_Question_ICD).ToList<ProblemList>()[0];
                            if (ProblemLst.Reference_Source.Contains("Deleted"))
                                ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
                            if (!ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
                            {
                                ProblemLst.Reference_Source += "|AHA Questionnaire";
                            }
                            ProblemLst.Status = "Inactive";
                            ProblemLst.Is_Active = "N";
                            ProblemLst.Modified_By = ClientSession.UserName;
                            ProblemLst.Modified_Date_And_Time = utc;
                            ProblemLst.Version_Year = "ICD_10";
                            UpdateProblemList.Add(ProblemLst);
                        }
                    }
                }

            }

            //For bug Id
            if (UIManager.PFSH_OpeingFrom == "Menu")
            {
                if (DeleteListMasterTemp != null && DeleteListMasterTemp.Count > 0)//Delete ProblemList
                {
                    foreach (var DeleteItem in DeleteListMasterTemp)
                    {
                        if (!Others.Contains(DeleteItem.Past_Medical_Info))
                        {
                            if (SavedProblemList.Any(a => a.ICD == DeleteItem.AHA_Question_ICD))
                            {
                                ProblemList ProblemLst = SavedProblemList.Where(a => a.ICD == DeleteItem.AHA_Question_ICD).ToList<ProblemList>()[0];
                                if (ProblemLst.Reference_Source.Contains("Deleted"))
                                    ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
                                if (!ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
                                {
                                    ProblemLst.Reference_Source += "|AHA Questionnaire";
                                }
                                ProblemLst.Status = "Inactive";
                                ProblemLst.Is_Active = "N";
                                ProblemLst.Modified_By = ClientSession.UserName;
                                ProblemLst.Modified_Date_And_Time = utc;
                                ProblemLst.Version_Year = "ICD_10";
                                UpdateProblemList.Add(ProblemLst);
                            }
                        }
                    }

                }
            }
            //if (SavedProblemList != null && SavedProblemList.Count == 0)
            //{
            //    SaveProblemList = SaveList.Where(s => s.Is_present == "Y" && !Others.Contains(s.Past_Medical_Info)).Select(s => new ProblemList()
            //    {
            //        Encounter_ID = s.Encounter_Id,
            //        Human_ID = s.Human_ID,
            //        Physician_ID = ClientSession.PhysicianId,
            //        ICD = s.AHA_Question_ICD,
            //        Problem_Description = s.Past_Medical_Info,
            //        Status = s.To_Date == "Current" ? "Active" : s.To_Date != string.Empty ? "Resolved" : s.From_Date == string.Empty ? "Inactive" : s.To_Date == string.Empty ? "Active" : string.Empty,
            //        Date_Diagnosed = s.From_Date,
            //        Is_Active = s.Is_present,
            //        Reference_Source = "AHA Questionnaire",
            //        Version_Year = "ICD_10",
            //        ICD_9 = ICDCodes.ContainsKey(s.AHA_Question_ICD) ? ICDCodes[s.AHA_Question_ICD] : string.Empty,
            //        ICD_9_Description = ICDCodes.ContainsKey(s.AHA_Question_ICD) ? s.Past_Medical_Info : string.Empty,
            //        Created_By = ClientSession.UserName,
            //        Created_Date_And_Time = utc
            //    }).ToList<ProblemList>();
            //}
            var curr_Insert = from objIns in SaveProblemList where objIns.Status == "Active" select objIns;
            var curr_Update = from objUp in UpdateProblemList where objUp.Status == "Active" select objUp;
            if (curr_Insert.ToList().Count > 0 || curr_Update.ToList().Count > 0)
            {
                var GetActiveProb = from obj in SavedProblemList where obj.ICD == "0000" select obj;
                if (GetActiveProb.ToList().Count > 0)
                {
                    ProblemList prblst = GetActiveProb.ToList<ProblemList>()[0];
                    prblst.Is_Active = "N";
                    prblst.Modified_By = ClientSession.UserName;
                    prblst.Modified_Date_And_Time = utc;
                    UpdateProblemList.Add(prblst);
                }
            }
            #endregion



            #region PastMedicalHistoryMasterTable
            //Save PastmedicalHistoryMaster table
            IList<PastMedicalHistoryMaster> SaveListMaster = new List<PastMedicalHistoryMaster>();
            IList<PastMedicalHistoryMaster> UpdateListMaster = new List<PastMedicalHistoryMaster>();
            IList<PastMedicalHistoryMaster> DeleteListMaster = new List<PastMedicalHistoryMaster>();

            IList<PastMedicalHistoryMaster> lstPMHmasterTemp = new List<PastMedicalHistoryMaster>();
            if (SaveList.Count > 0)
                foreach (PastMedicalHistory objpmh in SaveList)
                {
                    PastMedicalHistoryMaster objAddPMHMaster = new PastMedicalHistoryMaster();
                    //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                    lstPMHmasterTemp = lstPastHisMaster.Where(a => a.Past_Medical_Info.Trim().ToUpper() == objpmh.Past_Medical_Info.Trim().ToUpper() && a.Is_Deleted == "N").ToList<PastMedicalHistoryMaster>();
                    if (lstPMHmasterTemp.Count() == 0)
                    {
                        objAddPMHMaster.Human_ID = objpmh.Human_ID;
                        objAddPMHMaster.Past_Medical_Info = objpmh.Past_Medical_Info;
                        objAddPMHMaster.From_Date = objpmh.From_Date;
                        objAddPMHMaster.To_Date = objpmh.To_Date;
                        objAddPMHMaster.Is_present = objpmh.Is_present;
                        objAddPMHMaster.AHA_Question_ICD = objpmh.AHA_Question_ICD;
                        objAddPMHMaster.Is_Mandatory = objpmh.Is_Mandatory;
                        objAddPMHMaster.Notes = objpmh.Notes;
                        objAddPMHMaster.Created_By = objpmh.Created_By;
                        objAddPMHMaster.Created_Date_And_Time = objpmh.Created_Date_And_Time;
                        objAddPMHMaster.Modified_By = objpmh.Modified_By;
                        objAddPMHMaster.Modified_Date_And_Time = objpmh.Modified_Date_And_Time;
                        SaveListMaster.Add(objAddPMHMaster);
                    }
                    else
                    {
                        IList<PastMedicalHistoryMaster> tempSessionData = new List<PastMedicalHistoryMaster>();
                        //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                        tempSessionData = lstPastHisMaster.Where(a => a.Past_Medical_Info.Trim().ToUpper() == objpmh.Past_Medical_Info.Trim().ToUpper() && a.Is_Deleted == "N").ToList();
                        foreach (PastMedicalHistoryMaster temp in tempSessionData)
                        {
                            objAddPMHMaster = temp;
                        }
                        objAddPMHMaster.From_Date = objpmh.From_Date;
                        objAddPMHMaster.To_Date = objpmh.To_Date;
                        objAddPMHMaster.Is_present = objpmh.Is_present;
                        objAddPMHMaster.Is_Mandatory = objpmh.Is_Mandatory;
                        objAddPMHMaster.Notes = objpmh.Notes;
                        objAddPMHMaster.Modified_By = objpmh.Modified_By;
                        objAddPMHMaster.Modified_Date_And_Time = objpmh.Modified_Date_And_Time;
                        UpdateListMaster.Add(objAddPMHMaster);
                    }
                }
            //update
            if (UpdateList.Count() > 0)
            {
                foreach (PastMedicalHistory objpmhUpdate in UpdateList)
                {
                    PastMedicalHistoryMaster objAddPMHMaster = new PastMedicalHistoryMaster();
                    IList<PastMedicalHistoryMaster> tempSessionData = new List<PastMedicalHistoryMaster>();
                    //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                    tempSessionData = lstPastHisMaster.Where(a => a.Past_Medical_Info.Trim().ToUpper() == objpmhUpdate.Past_Medical_Info.Trim().ToUpper() && a.Is_Deleted == "N").ToList();
                    foreach (PastMedicalHistoryMaster temp in tempSessionData)
                    {
                        objAddPMHMaster = temp;
                    }
                    objAddPMHMaster.Past_Medical_Info = objpmhUpdate.Past_Medical_Info;
                    objAddPMHMaster.From_Date = objpmhUpdate.From_Date;
                    objAddPMHMaster.To_Date = objpmhUpdate.To_Date;
                    objAddPMHMaster.Is_present = objpmhUpdate.Is_present;
                    objAddPMHMaster.AHA_Question_ICD = objpmhUpdate.AHA_Question_ICD;
                    objAddPMHMaster.Is_Mandatory = objpmhUpdate.Is_Mandatory;
                    objAddPMHMaster.Notes = objpmhUpdate.Notes;
                    objAddPMHMaster.Modified_By = objpmhUpdate.Modified_By;
                    objAddPMHMaster.Modified_Date_And_Time = objpmhUpdate.Modified_Date_And_Time;
                    UpdateListMaster.Add(objAddPMHMaster);
                }
            }

            //delete
            if (UIManager.PFSH_OpeingFrom != "Menu")
            {
                if (DeleteList.Count() > 0)
                {
                    foreach (PastMedicalHistory objpmhDelete in DeleteList)
                    {
                        PastMedicalHistoryMaster objAddPMHMaster = new PastMedicalHistoryMaster();
                        IList<PastMedicalHistoryMaster> tempSessionData = new List<PastMedicalHistoryMaster>();
                        //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                        tempSessionData = lstPastHisMaster.Where(a => a.Past_Medical_Info.Trim().ToUpper() == objpmhDelete.Past_Medical_Info.Trim().ToUpper() && a.Is_Deleted == "N").ToList();
                        foreach (PastMedicalHistoryMaster temp in tempSessionData)
                        {
                            objAddPMHMaster = temp;
                            objAddPMHMaster.Is_Deleted = "Y";
                            objAddPMHMaster.Modified_By = ClientSession.UserName;
                            objAddPMHMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateListMaster.Add(objAddPMHMaster);
                        }

                        // DeleteListMaster.Add(objAddPMHMaster);
                    }
                }
            }
            //For menu and load from master table
            if (UIManager.PFSH_OpeingFrom == "Menu" || (HttpContext.Current.Session["PastMedicalList"] == null && HttpContext.Current.Session["PastMedicalMaster"] != null))
            {
                //For menu level delete list
                if (DeleteListMasterTemp.Count() > 0)
                {
                    foreach (PastMedicalHistoryMaster objpmhDelete in DeleteListMasterTemp)
                    {
                        PastMedicalHistoryMaster objAddPMHMaster = new PastMedicalHistoryMaster();
                        IList<PastMedicalHistoryMaster> tempSessionData = new List<PastMedicalHistoryMaster>();
                        //Jira Cap-348 - Index Out of Range error message when Provider adds Arthropathy in PMH
                        tempSessionData = lstPastHisMaster.Where(a => a.Past_Medical_Info.Trim().ToUpper() == objpmhDelete.Past_Medical_Info.Trim().ToUpper() && a.Is_Deleted == "N").ToList();
                        foreach (PastMedicalHistoryMaster temp in tempSessionData)
                        {
                            objAddPMHMaster = temp;
                            objAddPMHMaster.Is_Deleted = "Y";
                            objAddPMHMaster.Modified_By = ClientSession.UserName;
                            objAddPMHMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            UpdateListMaster.Add(objAddPMHMaster);
                        }
                    }
                }
            }
            //Save Master First
            #endregion

            if (UIManager.PFSH_OpeingFrom == "Menu")
            {
                SaveList = new List<PastMedicalHistory>();
                UpdateList = new List<PastMedicalHistory>();
                DeleteList = new List<PastMedicalHistory>();
                SaveGeneralNotes = new List<GeneralNotes>();
                UpdateGeneralNotes = new List<GeneralNotes>();
            }
            //pblmHistoryDTO = objPastMedicalHistoryManager.SaveUpdateDeletePastMedicalHistory(SaveList, UpdateList, DeleteList, SaveProblemList, UpdateProblemList, SaveGeneralNotes, UpdateGeneralNotes, string.Empty);
            if (SaveList.Count() == 0 && UpdateList.Count() == 0 && DeleteList.Count() == 0 && SaveProblemList.Count() == 0 && UpdateProblemList.Count() == 0 && SaveGeneralNotes.Count() == 0 && UpdateGeneralNotes.Count() == 0 && SaveListMaster.Count() == 0 && UpdateListMaster.Count() == 0 && DeleteListMaster.Count() == 0)
            {
                HttpContext.Current.Session["ProblemList"] = null;
                HttpContext.Current.Session["GeneralNotesList"] = null;
                HttpContext.Current.Session["PastMedicalList"] = null;
                HttpContext.Current.Session["PastMedicalMaster"] = null;
                return JsonConvert.SerializeObject("");
            }
            else
                pblmHistoryDTO = objPastMedicalHistoryManager.SaveUpdateDeletePastMedicalHistory(SaveList, UpdateList, DeleteList, SaveProblemList, UpdateProblemList, SaveGeneralNotes, UpdateGeneralNotes, string.Empty, ClientSession.HumanId, SaveListMaster, UpdateListMaster, DeleteListMaster);

            HttpContext.Current.Session["ProblemList"] = null;
            HttpContext.Current.Session["GeneralNotesList"] = null;
            HttpContext.Current.Session["PastMedicalList"] = null;
            HttpContext.Current.Session["PastMedicalMaster"] = null;
            IList<int> ilstChangeSummaryBar = new List<int>() { 4 };
            List<string> lstToolTip = new List<string>();
            string ProblemListText = string.Empty;
            SavedGeneralNotesList.Clear();
            var sProblemList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);
            ClientSession.bPFSHVerified = false;
            sProblemList = sProblemList.Where(a => a.ToUpper().StartsWith("PROBLEMLIST-")).ToList();
            if (sProblemList.Count > 0)
                ProblemListText = sProblemList[0].Replace("ProblemList-", "");
            if (pblmHistoryDTO.ProblemList != null && pblmHistoryDTO.ProblemList.Count > 0)
            {
                try
                {
                    if (SavedProblemList.Count > 0)
                    {
                        foreach (ProblemList ObjProblemList in pblmHistoryDTO.ProblemList)
                        {
                            IList<ProblemList> listProblem = SavedProblemList.Where(pb => pb.ICD == ObjProblemList.ICD).ToList<ProblemList>();
                            if (listProblem != null && listProblem.Count > 0)
                            {
                                SavedProblemList.Remove(listProblem[0]);
                            }
                            SavedProblemList.Add(ObjProblemList);
                        }
                    }
                    else
                    {
                        SavedProblemList = pblmHistoryDTO.ProblemList;
                    }
                    HttpContext.Current.Session["ProblemList"] = SavedProblemList;
                }
                catch (Exception Ex)
                {
                    string ExcMs = Ex.Message;
                }
            }
            else if (SavedProblemList.Count > 0)
            {
                HttpContext.Current.Session["ProblemList"] = SavedProblemList;
            }

            if ((sScreenMode != "" && sScreenMode.ToUpper() == "MENU") || (pblmHistoryDTO.PastMedicalList != null && pblmHistoryDTO.PastMedicalList.Count == 0))
            {
                var objResult = new object[5];
                if (((pblmHistoryDTO.PastMedicalMasterList != null && pblmHistoryDTO.PastMedicalMasterList.Count != 0) || (pblmHistoryDTO.GeneralNotesObject != null)))
                {
                    IList<PastMedicalHistoryMaster> lsttemp = new List<PastMedicalHistoryMaster>();
                    lsttemp = pblmHistoryDTO.PastMedicalMasterList.Where(p => p.Is_Deleted == "N").ToList();
                    if (lsttemp != null && lsttemp.Count != 0)
                    {

                        HttpContext.Current.Session["PastMedicalMaster"] = lsttemp;
                        var PastMedicalMaster = lsttemp.Select(a => new
                        {

                            description = a.Past_Medical_Info,
                            frmdate = changeFormat(a.From_Date),
                            todate = changeFormat(a.To_Date),
                            notes = a.Notes,
                            ispresent = a.Is_present,
                            Id = "0",
                            version = a.Version
                        });
                        objResult[0] = PastMedicalMaster;
                    }
                    if (pblmHistoryDTO.GeneralNotesObject != null && pblmHistoryDTO.GeneralNotesObject.Id != 0)
                    {
                        HttpContext.Current.Session["GeneralNotesList"] = pblmHistoryDTO.GeneralNotesObject;
                        objResult[1] = pblmHistoryDTO.GeneralNotesObject.Notes;
                    }
                    objResult[4] = sScreenMode;
                }
                return JsonConvert.SerializeObject(objResult);
            }


            if (pblmHistoryDTO.PastMedicalList != null || pblmHistoryDTO.GeneralNotesObject != null)
            {
                var ObjResult = new object[5];
                //SavedPastMedicalList = pblmHistoryDTO.PastMedicalList;
                //SavedGeneralNotesList.Add(pblmHistoryDTO.GeneralNotesObject);
                HttpContext.Current.Session["PastMedicalList"] = pblmHistoryDTO.PastMedicalList;
                HttpContext.Current.Session["GeneralNotesList"] = pblmHistoryDTO.GeneralNotesObject;
                HttpContext.Current.Session["PastMedicalMaster"] = pblmHistoryDTO.PastMedicalMasterList;
                if (pblmHistoryDTO.PastMedicalList != null && pblmHistoryDTO.PastMedicalList.Count > 0)//BugID:44827
                {
                    var PastMedical = pblmHistoryDTO.PastMedicalList.Select(a => new
                    {
                        value = a.Past_Medcial_History_ID,
                        description = a.Past_Medical_Info,
                        frmdate = changeFormat(a.From_Date),
                        todate = changeFormat(a.To_Date),
                        notes = a.Notes,
                        ispresent = a.Is_present,
                        Id = a.Id,
                        version = a.Version
                    });
                    ObjResult[0] = PastMedical;
                }

                if (ObjResult[0] == null)
                    ObjResult[0] = "";
                ObjResult[1] = pblmHistoryDTO.GeneralNotesObject.Notes;
                ObjResult[2] = ProblemListText;
                ObjResult[3] = ClientSession.EncounterId;
                ObjResult[4] = sScreenMode;
                return JsonConvert.SerializeObject(ObjResult);
            }
            else
            {
                HttpContext.Current.Session["ProblemList"] = null;
                HttpContext.Current.Session["GeneralNotesList"] = null;
                HttpContext.Current.Session["PastMedicalList"] = null;
                HttpContext.Current.Session["PastMedicalMaster"] = null;
                SavedPastMedicalList = new List<PastMedicalHistory>();
                SavedProblemList = new List<ProblemList>();
                SavedGeneralNotesList = new List<GeneralNotes>();
                return JsonConvert.SerializeObject(string.Empty);
            }
        }

        //Modified by Naveena For Past medical history Master table 22.1.2019
        //[WebMethod(EnableSession = true)]
        //public string SavePastMedicalHistory(object[] data, object[] Mod_lst)
        //{
        //    if (ClientSession.UserName == string.Empty)
        //    {
        //        HttpContext.Current.Response.StatusCode = 999;
        //        HttpContext.Current.Response.Status = "999 Session Expired";
        //        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //        return "Session Expired";
        //    }
        //    IList<PastMedicalHistory> SavedPastMedicalList = new List<PastMedicalHistory>();
        //    IList<GeneralNotes> SavedGeneralNotesList = new List<GeneralNotes>();
        //    IList<ProblemList> SavedProblemList = new List<ProblemList>();
        //    ProblemHistoryDTO pblmHistoryDTO = new ProblemHistoryDTO();
        //    IList<PastMedicalHistory> SaveList = new List<PastMedicalHistory>();
        //    IList<PastMedicalHistory> UpdateList = new List<PastMedicalHistory>();
        //    IList<PastMedicalHistory> DeleteList = new List<PastMedicalHistory>();
        //    PastMedicalHistoryManager objPastMedicalHistoryManager = new PastMedicalHistoryManager();
        //    IList<PastMedicalHistory> CurrentPastMedList = new List<PastMedicalHistory>();
        //    IList<ulong> PMHids = new List<ulong>();
        //    Dictionary<string, string> ICDCodes = new Dictionary<string, string>();
        //    if (data.Length > 3)//when no PastMedicalHistory present
        //    {
        //        for (int i = 0; i < data.Length - 3; i++)
        //        {
        //            PastMedicalHistory objPast = new PastMedicalHistory();
        //            Dictionary<string, object> patientValues = new Dictionary<string, object>();
        //            patientValues = (Dictionary<string, object>)data[i];
        //            objPast.Id = Convert.ToUInt32(patientValues["PMH_id"]);
        //            objPast.Is_present = Convert.ToString(patientValues["IsPresent"]);
        //            if (patientValues["FDate"] != null)
        //                objPast.From_Date = changeFormat(patientValues["FDate"].ToString());
        //            else
        //                objPast.From_Date = string.Empty;
        //            if (patientValues["IsCurrent"].ToString() == string.Empty)
        //            {
        //                if (patientValues["TDate"] != null)
        //                    objPast.To_Date = changeFormat(patientValues["TDate"].ToString());
        //                else
        //                    objPast.To_Date = string.Empty;
        //            }
        //            else
        //                objPast.To_Date = Convert.ToString(patientValues["IsCurrent"]);
        //            objPast.Past_Medical_Info = Convert.ToString(patientValues["ID"]);
        //            objPast.AHA_Question_ICD = Convert.ToString(patientValues["ICDcode"]);
        //            objPast.Human_ID = ClientSession.HumanId;
        //            objPast.Is_Mandatory = "N";
        //            objPast.Notes = Convert.ToString(patientValues["Notes"]);
        //            objPast.Encounter_Id = ClientSession.EncounterId;
        //            objPast.Created_By = ClientSession.UserName;
        //            objPast.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //            if (!ICDCodes.ContainsKey(objPast.AHA_Question_ICD))
        //            {
        //                ICDCodes.Add(objPast.AHA_Question_ICD, patientValues["ICD9Code"].ToString());
        //            }
        //            CurrentPastMedList.Add(objPast);
        //        }
        //    }

        //    #region PastMedicalList
        //    if (HttpContext.Current.Session["PastMedicalList"] != null)
        //    {
        //        SavedPastMedicalList = (IList<PastMedicalHistory>)HttpContext.Current.Session["PastMedicalList"];
        //    }
        //    if (SavedPastMedicalList == null)
        //        SavedPastMedicalList = new List<PastMedicalHistory>();
        //    if (SavedPastMedicalList.Count == 0)
        //    {
        //        SaveList = CurrentPastMedList;
        //    }
        //    else if (SavedPastMedicalList[0].Encounter_Id != ClientSession.EncounterId)
        //    {
        //        SaveList = CurrentPastMedList;
        //    }
        //    else if (SavedPastMedicalList[0].Encounter_Id == ClientSession.EncounterId)
        //    {
        //        foreach (PastMedicalHistory objPMH in CurrentPastMedList)
        //        {
        //            var obj = (from sList in SavedPastMedicalList where sList.Past_Medical_Info.ToUpper() == objPMH.Past_Medical_Info.ToUpper() select sList).ToList<PastMedicalHistory>();
        //            if (obj.Count > 0)//Update
        //            {
        //                PastMedicalHistory objPast = new PastMedicalHistory();
        //                objPast = (PastMedicalHistory)obj[0];
        //                objPast.AHA_Question_ICD = objPMH.AHA_Question_ICD;
        //                objPast.Is_present = objPMH.Is_present;
        //                objPast.From_Date = objPMH.From_Date;
        //                objPast.To_Date = objPMH.To_Date;
        //                objPast.Past_Medical_Info = objPMH.Past_Medical_Info;
        //                objPast.Human_ID = ClientSession.HumanId;
        //                objPast.Is_Mandatory = "N";
        //                objPast.Notes = objPMH.Notes;
        //                objPast.Encounter_Id = ClientSession.EncounterId;
        //                objPast.Modified_By = ClientSession.UserName;
        //                objPast.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                UpdateList.Add(objPast);
        //            }
        //            else//Save
        //            {
        //                objPMH.Created_By = ClientSession.UserName;
        //                objPMH.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                SaveList.Add(objPMH);
        //            }
        //        }
        //        if (SavedPastMedicalList.Count > 0 && SavedPastMedicalList[0].Encounter_Id == ClientSession.EncounterId)
        //        {
        //            if (CurrentPastMedList.Count > 0)
        //                DeleteList = (SavedPastMedicalList.Where(p => !CurrentPastMedList.Any(p2 => p2.Past_Medical_Info == p.Past_Medical_Info))).ToList<PastMedicalHistory>();
        //            else
        //                DeleteList = SavedPastMedicalList;
        //        }
        //    }
        //    #endregion
        //    #region GeneralNotes
        //    if (HttpContext.Current.Session["GeneralNotesList"] != null)
        //    {
        //        SavedGeneralNotesList.Add((GeneralNotes)HttpContext.Current.Session["GeneralNotesList"]);
        //    }
        //    if (SavedGeneralNotesList == null)
        //    {
        //        SavedGeneralNotesList = new List<GeneralNotes>();
        //    }
        //    IList<GeneralNotes> SaveGeneralNotes = new List<GeneralNotes>();
        //    IList<GeneralNotes> UpdateGeneralNotes = new List<GeneralNotes>();
        //    Dictionary<string, object> item = (Dictionary<string, object>)data[data.Length - 3];
        //    GeneralNotes objGNotes = new GeneralNotes();
        //    objGNotes.Human_ID = ClientSession.HumanId;
        //    objGNotes.Parent_Field = "Past Medical History";
        //    objGNotes.Notes = Convert.ToString(item["GeneralNotes"]);
        //    objGNotes.Encounter_ID = ClientSession.EncounterId;
        //    Dictionary<string, object> time = (Dictionary<string, object>)data[data.Length - 2];
        //    string strtime = time["LocalTime"].ToString().Split('G').ElementAt(0).ToString();
        //    DateTime utc = Convert.ToDateTime(strtime);
        //    if (SavedGeneralNotesList.Count == 0)
        //    {
        //        objGNotes.Created_By = ClientSession.UserName;
        //        objGNotes.Created_Date_And_Time = utc;
        //        SaveGeneralNotes.Add(objGNotes);
        //    }
        //    else if (SavedGeneralNotesList.Count > 0)
        //    {
        //        if (ClientSession.EncounterId == SavedGeneralNotesList[0].Encounter_ID)
        //        {
        //            objGNotes = SavedGeneralNotesList[0];
        //            objGNotes.Notes = Convert.ToString(item["GeneralNotes"]);
        //            objGNotes.Modified_By = ClientSession.UserName;
        //            objGNotes.Modified_Date_And_Time = utc;
        //            UpdateGeneralNotes.Add(objGNotes);
        //        }
        //        else
        //        {
        //            objGNotes.Created_By = ClientSession.UserName;
        //            objGNotes.Created_Date_And_Time = utc;
        //            SaveGeneralNotes.Add(objGNotes);
        //        }
        //    }
        //    #endregion
        //    #region ProblemList
        //    if (HttpContext.Current.Session["ProblemList"] != null)
        //    {
        //        SavedProblemList = (IList<ProblemList>)HttpContext.Current.Session["ProblemList"];
        //    }
        //    IList<ProblemList> SaveProblemList = new List<ProblemList>();
        //    IList<ProblemList> UpdateProblemList = new List<ProblemList>();

        //    //to prevent occurence of unmodified ICD's in IncompleteProblemList of Assessment
        //    IList<string> modified_Controls = new List<string>();
        //    foreach (object obj in Mod_lst)
        //    {
        //        modified_Controls.Add(obj.ToString());
        //    }

        //    string[] Others = { "Other1", "Other2", "Other3", "Other4", "Other5" };
        //    for (int i = 0; i < CurrentPastMedList.Count; i++)
        //    {
        //        ProblemList ObjProblem = new ProblemList();
        //        if (!Others.Contains(CurrentPastMedList[i].Past_Medical_Info))
        //        {
        //            if (SavedProblemList.Any(s => s.ICD == CurrentPastMedList[i].AHA_Question_ICD))
        //            {
        //                string ProblemStatus = CurrentPastMedList[i].To_Date == "Current" ? "Active" : CurrentPastMedList[i].To_Date != string.Empty ? "Resolved" : CurrentPastMedList[i].From_Date == string.Empty ? "Inactive" : CurrentPastMedList[i].To_Date == string.Empty ? "Active" : string.Empty;
        //                ObjProblem = SavedProblemList.Where(a => a.ICD == CurrentPastMedList[i].AHA_Question_ICD).ToList<ProblemList>()[0];
        //                if ((modified_Controls.Contains(ObjProblem.ICD)) && (ObjProblem.Date_Diagnosed != CurrentPastMedList[i].From_Date || ObjProblem.Status != ProblemStatus || ObjProblem.Is_Active != CurrentPastMedList[i].Is_present || SavedPastMedicalList.Any(p => p.AHA_Question_ICD == CurrentPastMedList[i].AHA_Question_ICD && p.Notes != CurrentPastMedList[i].Notes)))//(modified_Controls.Contains(ObjProblem.ICD) to check if the problemlist icd is actually modified before changing Is_Active to 'Y'
        //                {
        //                    ObjProblem.Status = ProblemStatus;
        //                    ObjProblem.Date_Diagnosed = CurrentPastMedList[i].From_Date;
        //                    if (ObjProblem.Reference_Source.Contains("Deleted"))
        //                        ObjProblem.Reference_Source = ObjProblem.Reference_Source.Replace("|Deleted", "");
        //                    if (!ObjProblem.Reference_Source.Contains("AHA Questionnaire"))
        //                    {
        //                        ObjProblem.Reference_Source += "|AHA Questionnaire";
        //                    }

        //                    ObjProblem.Is_Active = CurrentPastMedList[i].Is_present;
        //                    ObjProblem.Version_Year = "ICD_10";
        //                    ObjProblem.Modified_By = ClientSession.UserName;
        //                    ObjProblem.Modified_Date_And_Time = utc;
        //                    UpdateProblemList.Add(ObjProblem);
        //                }
        //            }
        //            else
        //            {
        //                if (CurrentPastMedList[i].Is_present == "Y")
        //                {
        //                    ObjProblem.Encounter_ID = ClientSession.EncounterId;
        //                    ObjProblem.Human_ID = ClientSession.HumanId;
        //                    ObjProblem.Physician_ID = ClientSession.PhysicianId;
        //                    ObjProblem.ICD = CurrentPastMedList[i].AHA_Question_ICD;
        //                    ObjProblem.Problem_Description = CurrentPastMedList[i].Past_Medical_Info;
        //                    ObjProblem.Status = CurrentPastMedList[i].To_Date == "Current" ? "Active" : CurrentPastMedList[i].To_Date != string.Empty ? "Resolved" : CurrentPastMedList[i].From_Date == string.Empty ? "Inactive" : CurrentPastMedList[i].To_Date == string.Empty ? "Active" : string.Empty;
        //                    ObjProblem.Date_Diagnosed = CurrentPastMedList[i].From_Date;
        //                    ObjProblem.Is_Active = CurrentPastMedList[i].Is_present;
        //                    ObjProblem.Reference_Source = "AHA Questionnaire";
        //                    ObjProblem.Version_Year = "ICD_10";
        //                    if (ICDCodes.ContainsKey(ObjProblem.ICD))
        //                    {
        //                        ObjProblem.ICD_9 = ICDCodes[ObjProblem.ICD].ToString();
        //                        ObjProblem.ICD_9_Description = ObjProblem.Problem_Description;
        //                    }
        //                    ObjProblem.Created_By = ClientSession.UserName;
        //                    ObjProblem.Created_Date_And_Time = utc;
        //                    SaveProblemList.Add(ObjProblem);
        //                }
        //            }
        //        }
        //    }
        //    if (DeleteList != null && DeleteList.Count > 0)//Delete ProblemList
        //    {
        //        foreach (var DeleteItem in DeleteList)
        //        {
        //            if (!Others.Contains(DeleteItem.Past_Medical_Info))
        //            {
        //                if (SavedProblemList.Any(a => a.ICD == DeleteItem.AHA_Question_ICD))
        //                {
        //                    ProblemList ProblemLst = SavedProblemList.Where(a => a.ICD == DeleteItem.AHA_Question_ICD).ToList<ProblemList>()[0];
        //                    if (ProblemLst.Reference_Source.Contains("Deleted"))
        //                        ProblemLst.Reference_Source = ProblemLst.Reference_Source.Replace("|Deleted", "");
        //                    if (!ProblemLst.Reference_Source.Contains("AHA Questionnaire"))
        //                    {
        //                        ProblemLst.Reference_Source += "|AHA Questionnaire";
        //                    }
        //                    ProblemLst.Status = "Inactive";
        //                    ProblemLst.Is_Active = "N";
        //                    ProblemLst.Modified_By = ClientSession.UserName;
        //                    ProblemLst.Modified_Date_And_Time = utc;
        //                    ProblemLst.Version_Year = "ICD_10";
        //                    UpdateProblemList.Add(ProblemLst);
        //                }
        //            }
        //        }

        //    }
        //    //if (SavedProblemList != null && SavedProblemList.Count == 0)
        //    //{
        //    //    SaveProblemList = SaveList.Where(s => s.Is_present == "Y" && !Others.Contains(s.Past_Medical_Info)).Select(s => new ProblemList()
        //    //    {
        //    //        Encounter_ID = s.Encounter_Id,
        //    //        Human_ID = s.Human_ID,
        //    //        Physician_ID = ClientSession.PhysicianId,
        //    //        ICD = s.AHA_Question_ICD,
        //    //        Problem_Description = s.Past_Medical_Info,
        //    //        Status = s.To_Date == "Current" ? "Active" : s.To_Date != string.Empty ? "Resolved" : s.From_Date == string.Empty ? "Inactive" : s.To_Date == string.Empty ? "Active" : string.Empty,
        //    //        Date_Diagnosed = s.From_Date,
        //    //        Is_Active = s.Is_present,
        //    //        Reference_Source = "AHA Questionnaire",
        //    //        Version_Year = "ICD_10",
        //    //        ICD_9 = ICDCodes.ContainsKey(s.AHA_Question_ICD) ? ICDCodes[s.AHA_Question_ICD] : string.Empty,
        //    //        ICD_9_Description = ICDCodes.ContainsKey(s.AHA_Question_ICD) ? s.Past_Medical_Info : string.Empty,
        //    //        Created_By = ClientSession.UserName,
        //    //        Created_Date_And_Time = utc
        //    //    }).ToList<ProblemList>();
        //    //}
        //    var curr_Insert = from objIns in SaveProblemList where objIns.Status == "Active" select objIns;
        //    var curr_Update = from objUp in UpdateProblemList where objUp.Status == "Active" select objUp;
        //    if (curr_Insert.ToList().Count > 0 || curr_Update.ToList().Count > 0)
        //    {
        //        var GetActiveProb = from obj in SavedProblemList where obj.ICD == "0000" select obj;
        //        if (GetActiveProb.ToList().Count > 0)
        //        {
        //            ProblemList prblst = GetActiveProb.ToList<ProblemList>()[0];
        //            prblst.Is_Active = "N";
        //            prblst.Modified_By = ClientSession.UserName;
        //            prblst.Modified_Date_And_Time = utc;
        //            UpdateProblemList.Add(prblst);
        //        }
        //    }
        //    #endregion
        //    //pblmHistoryDTO = objPastMedicalHistoryManager.SaveUpdateDeletePastMedicalHistory(SaveList, UpdateList, DeleteList, SaveProblemList, UpdateProblemList, SaveGeneralNotes, UpdateGeneralNotes, string.Empty);
        //    pblmHistoryDTO = objPastMedicalHistoryManager.SaveUpdateDeletePastMedicalHistory(SaveList, UpdateList, DeleteList, SaveProblemList, UpdateProblemList, SaveGeneralNotes, UpdateGeneralNotes, string.Empty, ClientSession.HumanId);
        //    HttpContext.Current.Session["ProblemList"] = null;
        //    HttpContext.Current.Session["GeneralNotesList"] = null;
        //    HttpContext.Current.Session["PastMedicalList"] = null;
        //    IList<int> ilstChangeSummaryBar = new List<int>() { 4 };
        //    List<string> lstToolTip = new List<string>();
        //    string ProblemListText = string.Empty;
        //    SavedGeneralNotesList.Clear();
        //    var sProblemList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);
        //    ClientSession.bPFSHVerified = false;
        //    sProblemList = sProblemList.Where(a => a.ToUpper().StartsWith("PROBLEMLIST-")).ToList();
        //    if (sProblemList.Count > 0)
        //        ProblemListText = sProblemList[0].Replace("ProblemList-", "");
        //    if (pblmHistoryDTO.ProblemList != null && pblmHistoryDTO.ProblemList.Count > 0)
        //    {
        //        try
        //        {
        //            if (SavedProblemList.Count > 0)
        //            {
        //                foreach (ProblemList ObjProblemList in pblmHistoryDTO.ProblemList)
        //                {
        //                    IList<ProblemList> listProblem = SavedProblemList.Where(pb => pb.ICD == ObjProblemList.ICD).ToList<ProblemList>();
        //                    if (listProblem != null && listProblem.Count > 0)
        //                    {
        //                        SavedProblemList.Remove(listProblem[0]);
        //                    }
        //                    SavedProblemList.Add(ObjProblemList);
        //                }
        //            }
        //            else
        //            {
        //                SavedProblemList = pblmHistoryDTO.ProblemList;
        //            }
        //            HttpContext.Current.Session["ProblemList"] = SavedProblemList;
        //        }
        //        catch (Exception Ex)
        //        {
        //            string ExcMs = Ex.Message;
        //        }
        //    }
        //    else if (SavedProblemList.Count > 0)
        //    {
        //        HttpContext.Current.Session["ProblemList"] = SavedProblemList;
        //    }
        //    if (pblmHistoryDTO.PastMedicalList != null || pblmHistoryDTO.GeneralNotesObject != null)
        //    {
        //        var ObjResult = new object[4];
        //        //SavedPastMedicalList = pblmHistoryDTO.PastMedicalList;
        //        //SavedGeneralNotesList.Add(pblmHistoryDTO.GeneralNotesObject);
        //        HttpContext.Current.Session["PastMedicalList"] = pblmHistoryDTO.PastMedicalList;
        //        HttpContext.Current.Session["GeneralNotesList"] = pblmHistoryDTO.GeneralNotesObject;
        //        if (pblmHistoryDTO.PastMedicalList != null && pblmHistoryDTO.PastMedicalList.Count > 0)//BugID:44827
        //        {
        //            var PastMedical = pblmHistoryDTO.PastMedicalList.Select(a => new
        //            {
        //                value = a.Past_Medcial_History_ID,
        //                description = a.Past_Medical_Info,
        //                frmdate = changeFormat(a.From_Date),
        //                todate = changeFormat(a.To_Date),
        //                notes = a.Notes,
        //                ispresent = a.Is_present,
        //                Id = a.Id,
        //                version = a.Version
        //            });
        //            ObjResult[0] = PastMedical;
        //        }

        //        if (ObjResult[0] == null)
        //            ObjResult[0] = "";
        //        ObjResult[1] = pblmHistoryDTO.GeneralNotesObject.Notes;
        //        ObjResult[2] = ProblemListText;
        //        ObjResult[3] = ClientSession.EncounterId;
        //        return JsonConvert.SerializeObject(ObjResult);
        //    }
        //    else
        //    {
        //        HttpContext.Current.Session["ProblemList"] = null;
        //        HttpContext.Current.Session["GeneralNotesList"] = null;
        //        HttpContext.Current.Session["PastMedicalList"] = null;
        //        SavedPastMedicalList = new List<PastMedicalHistory>();
        //        SavedProblemList = new List<ProblemList>();
        //        SavedGeneralNotesList = new List<GeneralNotes>();
        //        return JsonConvert.SerializeObject(string.Empty);
        //    }
        //}

    }
}
