using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Acurus.Capella.UI;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Newtonsoft.Json;

namespace Acurus.Capella.UI
{
    public partial class frmHistorySocial : SessionExpired
    {
        TableCell tc = new TableCell();
        HtmlGenericControl objLabel = new HtmlGenericControl();
        IList<SocialHistory> SocialHistoryDetails = null;
        SocialHistoryDTO problemDTO;
        SocialHistoryManager socialHistoryMngr = new SocialHistoryManager();
        IList<StaticLookup> objStaticLookup;
        Table tbldynamic = new Table();
        Table objTable12 = new Table();
        Dictionary<string, string> dictionary = null;
        GeneralNotes generalNotesObject = null;
        ArrayList mandatoryList = new ArrayList();
        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
        DateTime DOB = DateTime.MinValue;
        bool bCheckUnsaved = false;
        IList<StaticLookup> lststaticreason = new List<StaticLookup>();
        string sScreenMode = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            dictionary = new Dictionary<string, string>();
            chkShowAll.Attributes.Add("onclick", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}");
            if (UIManager.PFSH_OpeingFrom == "Menu")
            {
                lblNotes.Visible = false;
                DLC.Visible = false;
            }
            else
            {
                lblNotes.Visible = true;
                DLC.Visible = true;
            }

            if (!IsPostBack)
            {
                sScreenMode = UIManager.PFSH_OpeingFrom;
                lststaticreason = objStaticLookupMgr.getStaticLookupByFieldName("REASON NOT PERFORMED FOR TOBACCO USE AND EXPOSURE", "Sort_Order");
                for (int i = 0; i < lststaticreason.Count; i++)
                {
                    if (i == 0)
                        hdnreason.Value = lststaticreason[i].Field_Name + "|" + lststaticreason[i].Value;
                    else
                        hdnreason.Value = hdnreason.Value + "~" + lststaticreason[i].Field_Name + "|" + lststaticreason[i].Value;
                }

                ClientSession.IsDirtySocialHistory = true;
                if (SocialHistoryDetails == null)
                {
                    #region "Comment by balaji.TJ  - 2023-03-01"

                    //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";// "Base_XML" + "_" + ClientSession.EncounterId + ".xml";
                    //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                    //if (File.Exists(strXmlFilePath) == false)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "HideAllControls();", true);
                    //    return;
                    //}
                    #endregion
                    problemDTO = GetSocialHistory();
                    if (problemDTO.SocialList != null && problemDTO.SocialList.Count > 0)
                    {
                        SocialHistoryDetails = problemDTO.SocialList;
                        IList<SocialHistory> ilistMarital = SocialHistoryDetails.Where(a => a.Social_Info == "Marital Status").ToList<SocialHistory>();
                        if (ilistMarital == null || ilistMarital.Count == 0)
                        {

                            #region "Modified by balaji.TJ  - 2023-03-01" 
                            IList<string> ilstHScList = new List<string>();
                            ilstHScList.Add("HumanList");

                            IList<object> ilstHSBlobFinal = new List<object>();
                            ilstHSBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHScList);
                            if (ilstHSBlobFinal != null && ilstHSBlobFinal.Count > 0)
                            {
                                if (ilstHSBlobFinal[0] != null)
                                {
                                    for (int i = 0; i < ((List<object>)ilstHSBlobFinal[0]).Count; i++)
                                    {
                                        if (((Human)((List<object>)ilstHSBlobFinal[0])[i]).Marital_Status != "")
                                        {
                                            SocialHistory objSocHis = new SocialHistory();
                                            objSocHis.Social_Info = "Marital Status";
                                            objSocHis.Value = ((Human)((List<object>)ilstHSBlobFinal[0])[i]).Marital_Status;//itemDoc.GetElementsByTagName("Human")[0].Attributes["Marital_Status"].Value;
                                            objSocHis.Is_Present = "Y";
                                            SocialHistoryDetails.Add(objSocHis);
                                        }                                        
                                    }
                                }
                            }

                            #endregion

                            #region "Comment by balaji.TJ  - 2023-03-01"   

                            //string HumanFileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                            //string XmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                            //try
                            //{
                            //    if (File.Exists(XmlFilePath) == true)
                            //    {
                            //        XmlDocument itemDoc = new XmlDocument();
                            //        XmlTextReader XmlText = new XmlTextReader(XmlFilePath);
                            //        // itemDoc.Load(XmlText);
                            //        using (FileStream fs = new FileStream(XmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            //        {
                            //            itemDoc.Load(fs);

                            //            XmlText.Close();
                            //            if (itemDoc.GetElementsByTagName("Human").Count > 0 && itemDoc.GetElementsByTagName("Human")[0] != null && itemDoc.GetElementsByTagName("Human")[0].Attributes["Marital_Status"].Value != "")
                            //            {
                            //                SocialHistory objSocHis = new SocialHistory();
                            //                objSocHis.Social_Info = "Marital Status";
                            //                objSocHis.Value = itemDoc.GetElementsByTagName("Human")[0].Attributes["Marital_Status"].Value;
                            //                objSocHis.Is_Present = "Y";
                            //                SocialHistoryDetails.Add(objSocHis);
                            //            }
                            //            fs.Close();
                            //            fs.Dispose();
                            //        }
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    throw new Exception(ex.Message + " - " + XmlFilePath);
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        SocialHistoryDetails = new List<SocialHistory>();

                        #region "Modified by balaji.TJ  - 2023-03-01" 
                        IList<string> ilstHScList = new List<string>();
                        ilstHScList.Add("HumanList");

                        IList<object> ilstHSBlobFinal = new List<object>();
                        ilstHSBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHScList);
                        if (ilstHSBlobFinal != null && ilstHSBlobFinal.Count > 0)
                        {
                            if (ilstHSBlobFinal[0] != null)
                            {
                                for (int i = 0; i < ((List<object>)ilstHSBlobFinal[0]).Count; i++)
                                {
                                    if (((Human)((List<object>)ilstHSBlobFinal[0])[i]).Marital_Status != "")
                                    {
                                        SocialHistory objSocHis = new SocialHistory();
                                        objSocHis.Social_Info = "Marital Status";
                                        objSocHis.Value = ((Human)((List<object>)ilstHSBlobFinal[0])[i]).Marital_Status;//itemDoc.GetElementsByTagName("Human")[0].Attributes["Marital_Status"].Value;
                                        objSocHis.Is_Present = "Y";
                                        SocialHistoryDetails.Add(objSocHis);
                                        chkShowAll.Checked = true;
                                        bCheckUnsaved = true;
                                    }

                                    //SocialHistoryDetails.Add((SocialHistory)((List<object>)ilstHSBlobFinal[0])[i]);
                                }
                            }

                        }

                        #endregion

                        #region "Comment by balaji.TJ  - 2023-03-01"   

                        //string HumanFileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                        //string XmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                        //if (File.Exists(XmlFilePath) == true)
                        //{
                        //    XmlDocument itemDoc = new XmlDocument();
                        //    XmlTextReader XmlText = new XmlTextReader(XmlFilePath);
                        //    // itemDoc.Load(XmlText);
                        //    using (FileStream fs = new FileStream(XmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        //    {
                        //        itemDoc.Load(fs);

                        //        XmlText.Close();
                        //        if (itemDoc.GetElementsByTagName("Human").Count > 0 && itemDoc.GetElementsByTagName("Human")[0] != null && itemDoc.GetElementsByTagName("Human")[0].Attributes["Marital_Status"].Value != "")
                        //        {
                        //            SocialHistory objSocHis = new SocialHistory();
                        //            objSocHis.Social_Info = "Marital Status";
                        //            objSocHis.Value = itemDoc.GetElementsByTagName("Human")[0].Attributes["Marital_Status"].Value;
                        //            objSocHis.Is_Present = "Y";
                        //            SocialHistoryDetails.Add(objSocHis);
                        //            chkShowAll.Checked = true;
                        //            bCheckUnsaved = true;
                        //        }
                        //        fs.Close();
                        //        fs.Dispose();
                        //    }
                        //}
                        #endregion
                    }
                }
                Session["SocialHistoryDetails"] = SocialHistoryDetails;

                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                    DOB = ClientSession.PatientPaneList[0].Birth_Date;
                DLC.txtDLC.Attributes.Add("onkeypress", "return EnableSave('" + DLC.txtDLC.ClientID + "');");
                DLC.txtDLC.Attributes.Add("onchange", "EnableSave();");
                DLC.txtDLC.Attributes.Add("onkeyup", "AddUsersKeyDown(event);");
                ClientSession.processCheck = true;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                btnSave.Enabled = false;
                if (SocialHistoryDetails == null || SocialHistoryDetails.Count == 0)
                    chkShowAll.Checked = true;
                if (SocialHistoryDetails != null && SocialHistoryDetails.Count > 1)
                    ClientSession.IsDirtySocialHistory = false;
                else
                    ClientSession.IsDirtySocialHistory = true;
                Session["objStaticLookup"] = objStaticLookupMgr.getStaticLookupByFieldName("SOCIAL HISTORY", "Sort_Order");
                objStaticLookup = (IList<StaticLookup>)Session["objStaticLookup"];

                Session["objStaticLookupComboValues"] = objStaticLookupMgr.getStaticLookupByFieldNamelikeSorder("SOCIAL HISTORY OPTION FOR");

            }
            SocialHistoryDetails = (IList<SocialHistory>)Session["SocialHistoryDetails"];
            LoadData();
            if (!ClientSession.CheckUser && ClientSession.UserPermission == "R")
            {
                btnSave.Enabled = false;
                btnClearAll.Enabled = false;
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();LoadSocialHistory();LoadcboTobacco();}", true);
        }

        SocialHistoryDTO GetSocialHistory()
        {
            SocialHistoryDTO SocialHisDTO = new SocialHistoryDTO();
            IList<SocialHistory> SocHislst = new List<SocialHistory>();
            IList<GeneralNotes> lstGenNotesAll = new List<GeneralNotes>();
            IList<SocialHistoryMaster> SocHisMasterlst = new List<SocialHistoryMaster>();
            IList<SocialHistoryMaster> SocHisMasterlstTemp = new List<SocialHistoryMaster>();
            GeneralNotes genrlNotesSoc = new GeneralNotes();

            #region "Modified by balaji.TJ  - 2023-03-01"
            IList<string> listHistorysocialList = new List<string>();
            listHistorysocialList.Add("SocialHistoryList");
            listHistorysocialList.Add("GeneralNotesSocialHistoryList");
            listHistorysocialList.Add("SocialHistoryMasterList");

            IList<object> ilstHistorysocialBlobFinal = new List<object>();
            ilstHistorysocialBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, listHistorysocialList);
            if (ilstHistorysocialBlobFinal != null && ilstHistorysocialBlobFinal.Count > 0)
            {
                if (ilstHistorysocialBlobFinal[0] != null)
                {
                    for (int i = 0; i < ((IList<object>)ilstHistorysocialBlobFinal[0]).Count; i++)
                    {
                        SocHislst.Add((SocialHistory)((IList<object>)ilstHistorysocialBlobFinal[0])[i]);
                    }
                    Session["SocialHistoryCheck"] = SocHisMasterlst;
                }

                if (ilstHistorysocialBlobFinal[1] != null)
                {
                    for (int J = 0; J < ((IList<object>)ilstHistorysocialBlobFinal[1]).Count; J++)
                    {
                        lstGenNotesAll.Add((GeneralNotes)((IList<object>)ilstHistorysocialBlobFinal[1])[J]);
                    }
                }
                if (ilstHistorysocialBlobFinal[2] != null)
                {
                    for (int k = 0; k < ((IList<object>)ilstHistorysocialBlobFinal[2]).Count; k++)
                    {
                        SocHisMasterlstTemp.Add((SocialHistoryMaster)((IList<object>)ilstHistorysocialBlobFinal[2])[k]);
                    }

                    if (SocHisMasterlstTemp.Count > 0)
                    {
                        SocHisMasterlst = SocHisMasterlstTemp.Where(p => p.Is_Deleted == "N").ToList();
                    }
                    Session["SocialHistoryMaster"] = SocHisMasterlst;
                }
            }

            #endregion

            #region "Comment by balaji.TJ  - 2023-03-01"
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //try
            //{
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //        XmlNodeList xmlTagName = null;
            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        {
            //            itemDoc.Load(fs);

            //            XmlText.Close();

            //            if (itemDoc.GetElementsByTagName("SocialHistoryList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("SocialHistoryList")[0].ChildNodes;

            //                if (xmlTagName != null && xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(SocialHistory));
            //                        SocialHistory SocialHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as SocialHistory;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((SocialHistory)SocialHistory).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(SocialHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(SocialHistory, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(SocialHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(SocialHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(SocialHistory, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }

            //                        SocHislst.Add(SocialHistory);
            //                        //This session is used to find either the S.H load from master table or SH table
            //                        Session["SocialHistoryCheck"] = SocHisMasterlst;
            //                    }
            //                }
            //            }
            //            if (itemDoc.GetElementsByTagName("GeneralNotesSocialHistoryList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesSocialHistoryList")[0].ChildNodes;

            //                if (xmlTagName != null && xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
            //                        GeneralNotes GeneralNotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as GeneralNotes;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((GeneralNotes)GeneralNotes).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(GeneralNotes, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(GeneralNotes, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(GeneralNotes, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(GeneralNotes, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(GeneralNotes, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        lstGenNotesAll.Add(GeneralNotes);
            //                    }
            //                }
            //            }
            //            if (itemDoc.GetElementsByTagName("SocialHistoryMasterList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("SocialHistoryMasterList")[0].ChildNodes;
            //                if (xmlTagName != null && xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(SocialHistoryMaster));
            //                        SocialHistoryMaster SocialHistorymas = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as SocialHistoryMaster;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((SocialHistoryMaster)SocialHistorymas).GetType().GetProperties() select obji;
            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                if (propInfo != null)
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(SocialHistorymas, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(SocialHistorymas, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(SocialHistorymas, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(SocialHistorymas, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(SocialHistorymas, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        SocHisMasterlstTemp.Add(SocialHistorymas);

            //                        if (SocHisMasterlstTemp.Count > 0)
            //                        {
            //                            SocHisMasterlst = SocHisMasterlstTemp.Where(p => p.Is_Deleted == "N").ToList();

            //                        }

            //                    }
            //                }
            //            }
            //            Session["SocialHistoryMaster"] = SocHisMasterlst;
            //            fs.Close();
            //            fs.Dispose();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message + " - " + strXmlFilePath);
            //}
            #endregion

            if ((sScreenMode != "" && sScreenMode.ToUpper() == "MENU") || (SocHislst != null && SocHislst.Count == 0))
            {

                IList<SocialHistory> lstSocCurrEnc = new List<SocialHistory>();
                if (SocHisMasterlst != null && SocHisMasterlst.Count() > 0)
                {
                    foreach (SocialHistoryMaster objMaster in SocHisMasterlst)
                    {

                        SocialHistory objtempSH = new SocialHistory();
                        objtempSH.Human_ID = objMaster.Human_ID;
                        objtempSH.Social_Info = objMaster.Social_Info;
                        objtempSH.Is_Present = objMaster.Is_Present;
                        objtempSH.Description = objMaster.Description;
                        objtempSH.Value = objMaster.Value;
                        objtempSH.Recodes = objMaster.Recodes;
                        objtempSH.Is_Mandatory = objMaster.Is_Mandatory;
                        objtempSH.Created_By = ClientSession.UserName;
                        objtempSH.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                        objtempSH.Encounter_ID = ClientSession.EncounterId;
                        objtempSH.Snomed_Reason_Not_Performed = objMaster.Snomed_Reason_Not_Performed;
                        lstSocCurrEnc.Add(objtempSH);

                    }
                    SocialHisDTO.SocialList = lstSocCurrEnc;
                }
            }
            else if (SocHislst != null && SocHislst.Count > 0)
            {


                IList<SocialHistory> lstSocCurrEnc = new List<SocialHistory>();
                lstSocCurrEnc = (from item in SocHislst where item.Encounter_ID == ClientSession.EncounterId select item).ToList<SocialHistory>();
                if (lstSocCurrEnc != null && lstSocCurrEnc.Count > 0)
                {
                    SocialHisDTO.SocialList = lstSocCurrEnc;
                }
                else
                {
                    //ulong maxEncId = 0;
                    //IList<ulong> lstEncId = (from item in SocHislst select item.Encounter_ID).Distinct().ToList<ulong>();
                    //if (lstEncId != null && lstEncId.Count > 0)
                    //maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                    //foreach (ulong item in lstEncId)
                    //    if (item > maxEncId && item < ClientSession.EncounterId)
                    //        maxEncId = item;
                    //lstSocCurrEnc = (from item in SocHislst where item.Encounter_ID == maxEncId select item).ToList<SocialHistory>();

                    if (SocHisMasterlst != null && SocHisMasterlst.Count() > 0)
                    {
                        foreach (SocialHistoryMaster objMaster in SocHisMasterlst)
                        {

                            SocialHistory objtempSH = new SocialHistory();
                            objtempSH.Human_ID = objMaster.Human_ID;
                            objtempSH.Social_Info = objMaster.Social_Info;
                            objtempSH.Is_Present = objMaster.Is_Present;
                            objtempSH.Description = objMaster.Description;
                            objtempSH.Value = objMaster.Value;
                            objtempSH.Recodes = objMaster.Recodes;
                            objtempSH.Is_Mandatory = objMaster.Is_Mandatory;
                            objtempSH.Created_By = ClientSession.UserName;
                            objtempSH.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); ;
                            objtempSH.Encounter_ID = ClientSession.EncounterId;
                            objtempSH.Snomed_Reason_Not_Performed = objMaster.Snomed_Reason_Not_Performed;
                            lstSocCurrEnc.Add(objtempSH);

                        }
                        SocialHisDTO.SocialList = lstSocCurrEnc;
                    }
                }
            }
            else
            {

                SocialHisDTO.SocialList = SocHislst;
            }

            if (lstGenNotesAll != null && lstGenNotesAll.Count > 0)
            {
                IList<GeneralNotes> lstGenCurrEnc = new List<GeneralNotes>();
                lstGenCurrEnc = (from item in lstGenNotesAll where item.Encounter_ID == ClientSession.EncounterId select item).ToList<GeneralNotes>();
                if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                {
                    genrlNotesSoc = lstGenCurrEnc[0];
                }
                else
                {
                    ulong maxEncId = 0;
                    IList<ulong> lstEncId = (from item in lstGenNotesAll select item.Encounter_ID).Distinct().ToList<ulong>();
                    if (lstEncId != null && lstEncId.Count > 0)
                        maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                    foreach (ulong item in lstEncId)
                        if (item > maxEncId && item < ClientSession.EncounterId)
                            maxEncId = item;
                    lstGenCurrEnc = (from item in lstGenNotesAll where item.Encounter_ID == maxEncId select item).ToList<GeneralNotes>();
                    if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                    {
                        genrlNotesSoc = lstGenCurrEnc[0];
                        //genrlNotesDrug.Id = 0;
                    }
                }
            }

            SocialHisDTO.GeneralNotesObject = genrlNotesSoc;
            //SocialHisDTO.SocialList = SocHislst;
            Session["SocialHistoryDetails"] = SocialHisDTO.SocialList;
            //Changed by Vaishali on 16-11-2015
            if (SocialHisDTO != null && SocialHisDTO.GeneralNotesObject != null && SocialHisDTO.GeneralNotesObject.Id != 0)
            {
                Session["GeneralNotesObject"] = SocialHisDTO.GeneralNotesObject;
                DLC.txtDLC.Text = SocialHisDTO.GeneralNotesObject.Notes;
                generalNotesObject = (GeneralNotes)Session["GeneralNotesObject"];
            }
            return SocialHisDTO;
        }

        private void LoadData()
        {
            bool ctrl = false;
            if (Session["objStaticLookup"] == null)
            {
                Session["objStaticLookup"] = objStaticLookupMgr.getStaticLookupByFieldName("SOCIAL HISTORY", "Sort_Order");
            }
            if (chkShowAll.Checked == true)
            {
                objStaticLookup = (IList<StaticLookup>)Session["objStaticLookup"];
                for (int i = 0; i < objStaticLookup.Count; i++)
                {
                    if (SocialHistoryDetails != null)
                    {
                        IList<SocialHistory> ilistSocHis = SocialHistoryDetails.Where(a => a.Social_Info == objStaticLookup[i].Value).ToList<SocialHistory>();
                        if (ilistSocHis != null && ilistSocHis.Count() > 0)
                        {
                            string Is_Mandatory = "N";
                            if (objStaticLookup[i].Description == "MANDATORY")
                                Is_Mandatory = "Y";
                            if (!dictionary.Keys.Contains(ilistSocHis[0].Social_Info))
                            {
                                //CreateDynamicControlsForSocial(ilistSocHis[0].Social_Info, ilistSocHis[0].Is_Mandatory, ilistSocHis[0]);//For BUg id:63606
                                //Cap - 3604
                                //CreateDynamicControlsForSocial(ilistSocHis[0].Social_Info, Is_Mandatory, ilistSocHis[0]);
                                //Cap - 3594
                                if (objStaticLookup[i].Doc_Type == "" || objStaticLookup[i].Doc_Type.ToUpper() == ClientSession.PatientPaneList[0].Sex.ToUpper())
                                {
                                    CreateDynamicControlsForSocial(ilistSocHis[0].Social_Info, Is_Mandatory, ilistSocHis[0], objStaticLookup[i].Default_Value);
                                    dictionary.Add(ilistSocHis[0].Social_Info, ilistSocHis[0].Id.ToString());
                                }
                            }
                        }
                        else
                        {
                            string Is_Mandatory = "N";
                            if (objStaticLookup[i].Description == "MANDATORY")
                                Is_Mandatory = "Y";
                            if (!dictionary.Keys.Contains(objStaticLookup[i].Value))
                            {
                                //Cap - 3604
                                //CreateDynamicControlsForSocial(objStaticLookup[i].Value, Is_Mandatory, null);
                                //Cap - 3594
                                if (objStaticLookup[i].Doc_Type == "" || objStaticLookup[i].Doc_Type.ToUpper() == ClientSession.PatientPaneList[0].Sex.ToUpper())
                                {
                                    CreateDynamicControlsForSocial(objStaticLookup[i].Value, Is_Mandatory, null, objStaticLookup[i].Default_Value);
                                    dictionary.Add(objStaticLookup[i].Value, "0");
                                }
                            }
                        }
                    }
                }
                if (!ctrl)
                    createHeaderControls(ref ctrl);
            }
            else if (SocialHistoryDetails != null && SocialHistoryDetails.Count > 0 && Session["objStaticLookup"] != null)
            {
                objStaticLookup = (IList<StaticLookup>)Session["objStaticLookup"];
                for (int i = 0; i < SocialHistoryDetails.Count; i++)
                {
                    string Is_Mandatory = "N";
                    IList<StaticLookup> ilistSLookup = objStaticLookup.Where(a => a.Value == SocialHistoryDetails[i].Social_Info).ToList<StaticLookup>();
                    if (ilistSLookup != null && ilistSLookup.Count() > 0)
                    {
                        if (ilistSLookup[0].Description == "MANDATORY")
                            Is_Mandatory = "Y";
                    }
                    if (!dictionary.Keys.Contains(SocialHistoryDetails[i].Social_Info))
                    {
                        //CreateDynamicControlsForSocial(SocialHistoryDetails[i].Social_Info, SocialHistoryDetails[i].Is_Mandatory, SocialHistoryDetails[i]); //For BUg id:63606
                        //Cap - 3604
                        //CreateDynamicControlsForSocial(SocialHistoryDetails[i].Social_Info, Is_Mandatory, SocialHistoryDetails[i]);
                        //Cap - 3594
                        if (ilistSLookup[0].Doc_Type == "" || ilistSLookup[0].Doc_Type.ToUpper() == ClientSession.PatientPaneList[0].Sex.ToUpper())
                        {
                            CreateDynamicControlsForSocial(SocialHistoryDetails[i].Social_Info, Is_Mandatory, SocialHistoryDetails[i], ilistSLookup[0].Default_Value);
                            dictionary.Add(SocialHistoryDetails[i].Social_Info, SocialHistoryDetails[i].Id.ToString());
                        }
                    }
                }
                if (!ctrl)
                    createHeaderControls(ref ctrl);
            }
            if ((SocialHistoryDetails != null && SocialHistoryDetails.Count == 0) || bCheckUnsaved == true)
            {
                chkShowAll.Enabled = false;
                chkShowAll.Checked = true;
            }
        }

        /* private void LoadData()
        {
            bool ctrl = false;
            if (Session["objStaticLookup"] == null)
            {
                Session["objStaticLookup"] = objStaticLookupMgr.getStaticLookupByFieldName("SOCIAL HISTORY", "Sort_Order");
            }
            if (chkShowAll.Checked == true)
            {
                objStaticLookup = (IList<StaticLookup>)Session["objStaticLookup"];
                for (int i = 0; i < objStaticLookup.Count; i++)
                {
                    if (SocialHistoryDetails != null)
                    {
                        IList<SocialHistory> ilistSocHis = SocialHistoryDetails.Where(a => a.Social_Info == objStaticLookup[i].Value).ToList<SocialHistory>();
                        if (ilistSocHis != null && ilistSocHis.Count() > 0)
                        {
                            string Is_Mandatory = "N";
                            if (objStaticLookup[i].Description == "MANDATORY")
                                Is_Mandatory = "Y";
                            if (!dictionary.Keys.Contains(ilistSocHis[0].Social_Info))
                            {
                                //CreateDynamicControlsForSocial(ilistSocHis[0].Social_Info, ilistSocHis[0].Is_Mandatory, ilistSocHis[0]);//For BUg id:63606
                                CreateDynamicControlsForSocial(ilistSocHis[0].Social_Info, Is_Mandatory, ilistSocHis[0]);
                                dictionary.Add(ilistSocHis[0].Social_Info, ilistSocHis[0].Id.ToString());
                            }
                        }
                        else
                        {
                            string Is_Mandatory = "N";
                            if (objStaticLookup[i].Description == "MANDATORY")
                                Is_Mandatory = "Y";
                            if (!dictionary.Keys.Contains(objStaticLookup[i].Value))
                            {
                                CreateDynamicControlsForSocial(objStaticLookup[i].Value, Is_Mandatory, null);
                                dictionary.Add(objStaticLookup[i].Value, "0");
                            }
                        }
                    }
                }
                if (!ctrl)
                    createHeaderControls(ref ctrl);
            }
            else if (SocialHistoryDetails != null && SocialHistoryDetails.Count > 0)
            {
                objStaticLookup = (IList<StaticLookup>)Session["objStaticLookup"];
                for (int i = 0; i < SocialHistoryDetails.Count; i++)
                {
                    string Is_Mandatory = "N";
                    if (objStaticLookup[i].Description == "MANDATORY")
                        Is_Mandatory = "Y";
                    if (!dictionary.Keys.Contains(SocialHistoryDetails[i].Social_Info))
                    {
                        //CreateDynamicControlsForSocial(SocialHistoryDetails[i].Social_Info, SocialHistoryDetails[i].Is_Mandatory, SocialHistoryDetails[i]); //For BUg id:63606
                        CreateDynamicControlsForSocial(SocialHistoryDetails[i].Social_Info, Is_Mandatory, SocialHistoryDetails[i]);
                        dictionary.Add(SocialHistoryDetails[i].Social_Info, SocialHistoryDetails[i].Id.ToString());
                    }
                }
                if (!ctrl)
                    createHeaderControls(ref ctrl);
            }
            if ((SocialHistoryDetails != null && SocialHistoryDetails.Count == 0) || bCheckUnsaved == true)
            {
                chkShowAll.Enabled = false;
                chkShowAll.Checked = true;
            }
        } */

        void createHeaderControls(ref bool ctrl)
        {
            TableRow Headertr2 = new TableRow();
            tc = new TableCell();
            tc.Width = Unit.Pixel(130);
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            if (UIManager.PFSH_OpeingFrom == "Menu")
                tc.Width = 46;
            else
                tc.Width = 35;
            Headertr2.Cells.Add(tc);



            tc = new TableCell();
            objLabel = new HtmlGenericControl();
            objLabel.ID = "lblYess";
            objLabel.InnerText = "Yes";
            objLabel.Attributes.Add("class", "LabelStyleBold");
            //objLabel.Style.Add("font-size", "small");
            //objLabel.Style.Add("font-family", "Serif");
            tc.Controls.Add(objLabel);
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = 5;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            objLabel = new HtmlGenericControl();
            objLabel.ID = "lblNoo";
            objLabel.InnerText = "No";
            //objLabel.Style.Add("font-size", "small");
            //objLabel.Style.Add("font-family", "Serif");
            objLabel.Attributes.Add("class", "LabelStyleBold");
            tc.Width = 150;
            tc.Controls.Add(objLabel);
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = 50;
            Headertr2.Cells.Add(tc);

            objLabel = new HtmlGenericControl();
            objLabel.ID = "lblStatuss";
            objLabel.InnerText = "Status";
            //objLabel.Style.Add("font-size", "small");
            //objLabel.Style.Add("font-family", "Serif");
            objLabel.Attributes.Add("class", "LabelStyleBold");
            tc.Width = 370;
            tc.Controls.Add(objLabel);
            Headertr2.Cells.Add(tc);


            tc = new TableCell();
            tc.Width = 50;
            Headertr2.Cells.Add(tc);

            objLabel = new HtmlGenericControl();
            objLabel.ID = "lblDescriptionn";
            objLabel.InnerText = "Comments/Reason not Performed";
            objLabel.Attributes.Add("class", "LabelStyleBold");
            //objLabel.Style.Add("font-size", "small");
            //objLabel.Style.Add("font-family", "Serif");
            tc.Width = 420;
            tc.Controls.Add(objLabel);
            Headertr2.Cells.Add(tc);

            objTable12.Rows.Add(Headertr2);
            divSocialHistoryHeaderControls.Controls.Add(objTable12);

            Test();
            ctrl = true;
        }

        void Test()
        {
            TableRow Headertr2 = new TableRow();
            tc = new TableCell();
            tc.Width = Unit.Pixel(130);
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            if (UIManager.PFSH_OpeingFrom == "Menu")
                tc.Width = 53;
            else
                tc.Width = 42;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            CheckBox objCheckBox = new CheckBox();
            objCheckBox.ID = "chkAllYes";
            objCheckBox.Attributes.Add("onclick", "CheckChanged('" + objCheckBox.ID + "');");
            tc.Width = 35;
            tc.Controls.Add(objCheckBox);
            objCheckBox.EnableViewState = false;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = 5;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            CheckBox objCheckBox1 = new CheckBox();
            objCheckBox1.ID = "chkAllNo";
            objCheckBox1.Attributes.Add("onclick", "CheckChanged('" + objCheckBox1.ID + "');");
            tc.Controls.Add(objCheckBox1);
            objCheckBox1.EnableViewState = false;
            Headertr2.Cells.Add(tc);

            objTable12.Rows.Add(Headertr2);
            divSocialHistoryHeaderControls.Controls.Add(objTable12);
            if (ClientSession.UserRole.Trim() == "Coder" || ClientSession.UserPermission == "R" || ClientSession.UserCurrentProcess == "CHECK_OUT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty))
            {
                DLC.txtDLC.Enabled = false;
                DLC.pbDropdown.Disabled = false;
                DLC.pbDropdown.Style.Add("background", "#808080");
                objCheckBox.Enabled = false;
                objCheckBox1.Enabled = false;
            }
        }

        public void CreateDynamicControlsForSocial(string HistoryInfo, string Is_Mandatory, SocialHistory pastMedicalList, string Default_Value)
        {
                TableCell tc = new TableCell();
                TableRow tr1 = new TableRow();
                TableRow tr3 = new TableRow();
                Label lblsocial = new Label();

                lblsocial.ID = "lbl" + HistoryInfo.Replace(" ", "");
                lblsocial.Text = HistoryInfo;
                lblsocial.EnableViewState = false;
                //lblsocial.CssClass = "Editabletxtbox";
                //lblsocial.Font.Name = FontFamily.GenericSansSerif.ToString();

                // lblsocial.Font.Size = new FontUnit("8.5pt");

                if (DOB == DateTime.MinValue)
                {
                    if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                        DOB = ClientSession.PatientPaneList[0].Birth_Date;
                }
                if (Is_Mandatory == "Y" && UtilityManager.CalculateAge(DOB) >= 13)
                {
                    lblsocial.Text += " *";
                    lblsocial.Attributes.Add("mand", "Yes");
                    // lblsocial.ForeColor = Color.Red;
                    mandatoryList.Add("lblsocial" + HistoryInfo);
                }
                else
                {
                    // lblsocial.ForeColor = Color.Black;
                    lblsocial.Attributes.Add("mand", "No");
                }
                lblsocial.Width = 175;
                tc.Controls.Add(lblsocial);
                tr1.Cells.Add(tc);

                tc = new TableCell();
                CheckBox chkBoxYes = new CheckBox();
                chkBoxYes.ID = "chkYes" + HistoryInfo.Replace(" ", "");

                chkBoxYes.Width = 40;
                if (pastMedicalList != null)
                {
                    if (pastMedicalList.Is_Present == "Y")
                        chkBoxYes.Checked = true;
                }
                //changed by vaishali on 19-11-2015
                if (!HistoryInfo.ToUpper().Contains("TOBACCO"))
                    chkBoxYes.Attributes.Add("onclick", "return enableField('" + chkBoxYes.ID + "');");
                else
                {
                    chkBoxYes.Attributes.Add("onclick", "return LoadTobaccoList();");
                }

                tc.Controls.Add(chkBoxYes);
                tr1.Cells.Add(tc);

                tc = new TableCell();
                CheckBox chkBoxNo = new CheckBox();
                chkBoxNo.ID = "chkNo" + HistoryInfo.Replace(" ", "");
                chkBoxNo.Width = 50;
                if (pastMedicalList != null)
                {
                    if (pastMedicalList.Is_Present == "N")
                        chkBoxNo.Checked = true;
                }
                //changed by vaishali on 19-11-2015
                if (!(HistoryInfo.ToUpper().Contains("TOBACCO")))
                    chkBoxNo.Attributes.Add("onclick", "return enableField('" + chkBoxNo.ID + "');");
                else
                {
                    chkBoxNo.Attributes.Add("onclick", "return LoadTobaccoList();");
                }
                tc.Controls.Add(chkBoxNo);
                tr1.Cells.Add(tc);
                //Cap - 3604
                var options = new HtmlSelect();
                if (Default_Value == "TEXTBOX")
                {
                    tc = new TableCell();
                    TextBox txt = new TextBox();
                    txt.ID = "txt" + HistoryInfo.Replace(" ", "");
                    txt.Text = "";
                    txt.Attributes.Add("class", "Editabletxtbox");
                    txt.Attributes.Add("onkeyup", "myAutocomplete(this);");
                    txt.Attributes.Add("onkeypress", "myAutocomplete(this);");
                    txt.EnableViewState = false;
                    txt.Width = Unit.Pixel(310);
                if (chkBoxYes.Checked == true)
                {
                    txt.Enabled = false;
                }

                else
                {
                    txt.Enabled = false;
                    txt.Text = "";
                }

                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                    img.ID = "img" + HistoryInfo.Replace(" ", "");                    
                    img.Width = 12;
                    img.Height = 12;
                    img.Style.Add("margin-left", "-15px");
                    img.Attributes.Add("onclick", "ClearTextbox(event);");

                if (chkBoxYes.Checked == true)
                {
                    img.Enabled = true;
                    img.ImageUrl = "Resources/Delete-Blue.png";
                }

                else
                {
                    img.Enabled = false;
                    img.ImageUrl = "Resources/Delete-Grey.png";
                    img.Attributes.Add("onclick", "");
                    txt.Text = "";
                }

                    tc.Controls.Add(txt);
                    tc.Controls.Add(img);
                    tr1.Cells.Add(tc);
                }
                else
                {

                    tc = new TableCell();
                    //RadComboBox options = new RadComboBox();
                    //options.ID = "cbo" + HistoryInfo.Replace(" ", "");
                    //options.AutoPostBack = false;
                    //options.AllowCustomText = false;
                    //options.Attributes.Add("onkeypress", "EnableSave();");
                    //options.OnClientSelectedIndexChanged = "OnClientSelectedIndex";
                    //options.CssClass = "Editabletxtbox";

                    options.ID = "cbo" + HistoryInfo.Replace(" ", "");
                    //options.AutoPostBack = false;
                    //options.AllowCustomText = false;
                    options.Attributes.Add("onchange", "OnClientSelectedIndex();");
                    // options.OnClientSelectedIndexChanged = "OnClientSelectedIndex";
                    //options.Attributes.Add("onSelectedIndexChanged", "OnClientSelectedIndex();");
                    options.Attributes.Add("class", "Editabletxtbox");
                    if (!(HistoryInfo.Replace(" ", "").ToUpper().Contains("TOBACCO")))
                    {
                        if (chkBoxYes.Checked == true)
                            //options.Enabled = true;
                            options.Disabled = false;
                        else
                            //options.Enabled = false;
                            options.Disabled = true;
                    }
                    else
                    {
                        if (chkBoxYes.Checked || chkBoxNo.Checked)
                            // options.Enabled = true;
                            options.Disabled = false;
                        else
                            //options.Enabled = false;
                            options.Disabled = true;
                        //options.Attributes.Add("Style", "Height:320px;");
                        //options.Height = Unit.Pixel(320);
                    }

                    // options.Width = 320;
                    //options.BackColor = Color.White;

                    //options.Attributes.Add("Style", "");
                    options.Attributes.Add("Style", "BackColor:White;Width:320px;");

                    tc.Controls.Add(options);
                    tr1.Cells.Add(tc);
                }

                CustomDLCNew userCtrl = (CustomDLCNew)LoadControl("~/UserControls/customDLCNew.ascx");
                tc = new TableCell();
                Panel objPanel = new Panel();
                objPanel.Style.Add(HtmlTextWriterStyle.Width, "100%");
                objPanel.Style.Add(HtmlTextWriterStyle.Height, "100%");
                objPanel.Style.Add(HtmlTextWriterStyle.FontSize, "Small");
                tc.Controls.Add(objPanel);
                userCtrl.ID = "DLC" + HistoryInfo.Replace(" ", ""); ;
                userCtrl.txtDLC.Attributes.Add("onkeypress", "EnableSave('" + userCtrl.txtDLC.ClientID + "');");
                userCtrl.txtDLC.Attributes.Add("onkeyup", "AddUsersKeyDown(event);");
                userCtrl.txtDLC.Attributes.Add("onchange", "EnableSave('" + userCtrl.txtDLC.ClientID + "');");
                userCtrl.TextControlID = chkBoxNo.ID + "," + chkBoxYes.ID + "," + HistoryInfo;
                //userCtrl.pbDropdown.Attributes.Add("onclick", "return pbDropDown('" + userCtrl.ID + "_pbDropdown','" + userCtrl.ID + "_listDLC','" + HistoryInfo + "')");
                userCtrl.ListboxHeight = (Unit)50;
                userCtrl.TextboxHeight = new Unit("40px");
                userCtrl.TextboxWidth = new Unit("500px");
                userCtrl.Value = HistoryInfo;
                //userCtrl.Enable = false;            
                objPanel.Controls.Add(userCtrl);
                tr1.Cells.Add(tc);
                if (chkBoxYes.Checked == true || chkBoxNo.Checked == true)
                    userCtrl.Enable = true;
                tc = new TableCell();
                Label lblLine = new Label();
                lblLine.ID = "lblLine" + HistoryInfo;
                lblLine.EnableViewState = false;
                tc.ColumnSpan = 10;
                tc.Controls.Add(lblLine);
                tr3.Cells.Add(tc);
                tbldynamic.Rows.Add(tr1);
                tbldynamic.Rows.Add(tr3);
                divSocialHistoryControls.Controls.Add(tbldynamic);
                if (options.Items.Count > 1 && !options.Items[1].Value.Contains(HistoryInfo.Replace(" ", "")))
                {
                    LoadOptionForCombo(HistoryInfo);
                }

                userCtrl.pbDropdown.Attributes.Add("onclick", "return pbDropDown('" + userCtrl.ID + "_pbDropdown','" + userCtrl.ID + "_listDLC','" + HistoryInfo + "')");
                string chkYes = Request.Form["chkYes" + HistoryInfo.Replace(" ", "")];
                string chkNo = Request.Form["chkNo" + HistoryInfo.Replace(" ", "")];
                if (pastMedicalList != null)
                {
                    userCtrl.txtDLC.Text = pastMedicalList.Description;
                    if (IsPostBack)
                    {
                        if (!(HistoryInfo.Replace(" ", "").ToUpper().Contains("TOBACCO")))
                        {
                            if (chkYes == "on")
                                //options.Enabled = true;
                                options.Disabled = false;
                            else
                                //options.Enabled = false;
                                options.Disabled = true;
                        }
                        else
                        {
                            if (chkYes == "on" || chkNo == "on")
                                // options.Enabled = true;
                                options.Disabled = false;
                            else
                                //options.Enabled = false;
                                options.Disabled = true;
                            //options.Attributes.Add("Style", "Height:320px;");
                            //options.Height = Unit.Pixel(320);
                        }
                        //if (chkYes == "on" || chkNo == "on")
                        //{
                        //    ////userCtrl.Enable = true;
                        //    //options.Enabled = true;
                        //    //options.Attributes.Add("Enabled", "true");

                        //}
                        //else
                        //{
                        //    //options.Enabled = false;
                        //    //options.Attributes.Add("Enabled", "false");
                        //    options.Disabled = true;
                        //}
                    }
                }
                else
                {
                    if (!(HistoryInfo.Replace(" ", "").ToUpper().Contains("TOBACCO")))
                    {
                        if (chkYes == "on")
                            //options.Enabled = true;
                            options.Disabled = false;
                        else
                            //options.Enabled = false;
                            options.Disabled = true;
                    }
                    else
                    {
                        if (chkYes == "on" || chkNo == "on")
                            // options.Enabled = true;
                            options.Disabled = false;
                        else
                            //options.Enabled = false;
                            options.Disabled = true;
                        //options.Attributes.Add("Style", "Height:320px;");
                        //options.Height = Unit.Pixel(320);
                    }
                    //if (chkYes == "on" || chkNo == "on")
                    //{
                    //    //userCtrl.Enable = true;
                    //    //options.Enabled = true;
                    //    //options.Attributes.Add("Enabled", "true");
                    //    options.Disabled = false;
                    //}
                    //else
                    //{
                    //    //options.Enabled = false;
                    //    //options.Attributes.Add("Enabled", "false");
                    //    options.Disabled= true;
                    //}
                }
                //Cap - 3604
                //if (!IsPostBack || options.Items.Count == 0)
                if ((!IsPostBack || options.Items.Count == 0) && Default_Value != "TEXTBOX")
                {
                    LoadOptionForCombo(HistoryInfo);
                }
                else if (Default_Value == "TEXTBOX" && pastMedicalList != null)
                {
                    TextBox txt = divSocialHistoryControls.FindControl("txt" + HistoryInfo.Replace(" ", "")) as TextBox;
                    txt.Text = pastMedicalList.Value;
                    txt.Attributes.Add("OccupationVal", pastMedicalList.Value);
                }

                if (HistoryInfo.ToUpper().Contains("TOBACCO"))
                {
                    if (chkBoxYes.Checked == true)
                    {
                        LoadTobacco(true, HistoryInfo);
                    }
                    else if (chkBoxNo.Checked == true)
                    {
                        LoadTobacco(false, HistoryInfo);
                    }
                    else
                        LoadTobacco(null, HistoryInfo);
                }

                if (pastMedicalList != null && pastMedicalList.Is_Present == "Y" && !(HistoryInfo.ToUpper().Contains("TOBACCO")))
                {
                    if (options.Items.Count > 0)
                        //options.SelectedIndex = options.Items.IndexOf(options.Items.FindItemByText(pastMedicalList.Value));
                        options.SelectedIndex = options.Items.IndexOf(options.Items.FindByText(pastMedicalList.Value));
                    //options.Enabled = true;
                    //options.Attributes.Add("Enabled", "true");
                    //options.Disabled= false;
                }
                else if (pastMedicalList != null && (pastMedicalList.Is_Present == "N" || pastMedicalList.Is_Present == "Y"))
                {
                    if (options.Items.Count > 0)
                        //options.SelectedIndex = options.Items.IndexOf(options.Items.FindItemByText(pastMedicalList.Value));
                        options.SelectedIndex = options.Items.IndexOf(options.Items.FindByText(pastMedicalList.Value));
                }

                if (ClientSession.UserRole.Trim() == "Coder" || ClientSession.UserPermission == "R" || ClientSession.UserCurrentProcess == "CHECK_OUT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty))
                {
                    userCtrl.Enable = false;
                    //options.Enabled = false;
                    options.Disabled = true;
                    chkBoxYes.Enabled = false;
                    chkBoxNo.Enabled = false;
                }
        }

        public void LoadOptionForCombo(string fieldName)
        {
            //fieldName = fieldName.Replace(" ", "");
            // Cap - 801
            //IList<StaticLookup> objStaticLookupComboVAlues = (IList<StaticLookup>)Session["objStaticLookupComboValues"];
            IList<StaticLookup> objStaticLookupComboVAlues = new List<StaticLookup>();
            if (Session["objStaticLookupComboValues"] != null && (IList<StaticLookup>)Session["objStaticLookupComboValues"] != null)
            {
                objStaticLookupComboVAlues = (IList<StaticLookup>)Session["objStaticLookupComboValues"];
            }
            var valueslist = (from m in objStaticLookupComboVAlues where m.Field_Name.Contains(fieldName.ToUpper()) select m).ToList<StaticLookup>();

            //IList<String> istaticLookup = null;
            //if (fieldName.ToUpper().Contains("ALCOHOLINTAKE"))
            //    istaticLookup = new List<String> { "Occasional", "Daily", "Weekly", "Monthly", "Once a year", "Rarely", "Frequently" };
            //else if (fieldName.Contains("SMOKINGHABIT"))
            //    istaticLookup = new List<String> {"Current every day smoker|449868002","Current some day smoker|428041000124106","Former smoker|8517006","Never smoker|266919005","Smoker, current status unknown|77176002",
            //        "Unknown if ever smoked|266927001","Heavy Tobacco smoker|428071000124103","Light Tobacco smoker|428061000124105"};
            //else if (fieldName.Contains("DRUG USE"))
            //    istaticLookup = new List<String> {"Occasional","Daily","Weekly","Monthly","Once a year","Type: Illegal drugs","Type: recreational drugs","Route: Inhalation","Route: Ingestion",
            //        "Route: Infusion" };
            //else if (fieldName.Contains("DIETARYPATTERNS"))
            //    istaticLookup = new List<String> { "Balanced diet", "Unbalanced diet", "Vegetarian", "Non Vegetarian", "Starve", "Fat diet", "Non-fat diet" };
            //else if (fieldName.Contains("COFFEECONSUMPTION"))
            //    istaticLookup = new List<String> { "Regular", "Irregular" };
            //else if (fieldName.Contains("EmploymentDetails"))
            //    istaticLookup = new List<String> { "Not Working", "Retired", "Working - Full Time", "Working - Part Time" };
            //else if (fieldName.Contains("ExercisePatterns"))
            //    istaticLookup = new List<String> { "Regular", "Irregular" };
            //else if (fieldName.Contains("HomeEnvironment"))
            //    istaticLookup = new List<String> { "Calm", "Noisy", "Pleasant", "Enjoyable", "Disturbing", "Unpleasant" };
            //else if (fieldName.Contains("MaritalStatus"))
            //    istaticLookup = new List<String> { "Married", "Divorced", "Widowed", "Other", "Single", };
            //else if (fieldName.Contains("Occupation"))
            //    istaticLookup = new List<String> { "Salaried", "Self Employed", "Business Owners", "Contract Employee" };
            //else if (fieldName.Contains("SleepPatterns"))
            //    istaticLookup = new List<String> { "Calm sleep", "Disturbed", "Irregular sleep" };
            //else if (fieldName.Contains("SexuallyActive"))
            //    istaticLookup = new List<String> { };
            //else if (fieldName.Contains("DetailsofDrugmisusebehavior"))
            //    istaticLookup = new List<String> { "Never", "Occasional", "Daily", "Weekly", "Monthly", "Once a year", "Type: Illegal drugs", "Type: recreational drugs", "Route: Inhalation", "Route: Ingestion", "Route: Infusion", "Yes", "No" };
            //else if (fieldName.Contains("Prozacdependent"))
            //    istaticLookup = new List<String> { "Never", "Rare", "Occasional", "Frequent" };
            //else if (fieldName.Contains("ToxicExposure"))
            //    istaticLookup = new List<String> { "Yes", "No" };
            //else if (fieldName.Contains("Other"))
            //    istaticLookup = new List<String> { };

            //RadComboBox cmbBox = (RadComboBox)divSocialHistoryControls.FindControl("cbo" + fieldName.Replace(" ", ""));
            HtmlSelect cmbBox = (HtmlSelect)divSocialHistoryControls.FindControl("cbo" + fieldName.Replace(" ", ""));
            cmbBox.Items.Clear();

            //if (valueslist != null && valueslist.Count > 0)
            //{
            //    cmbBox.Items.Add(new RadComboBoxItem());
            //    for (int j = 0; j < valueslist.Count; j++)
            //    {
            //        RadComboBoxItem tempItem = new RadComboBoxItem();
            //        tempItem.Text = valueslist[j].Value;
            //        if (valueslist[j].Description != "")
            //            tempItem.Value = fieldName + "-" + valueslist[j].Description;
            //        else
            //            tempItem.Value = fieldName + j;
            //        cmbBox.Items.Add(tempItem);
            //    }
            //}

            if (valueslist != null && valueslist.Count > 0)
            {
                cmbBox.Items.Add(new ListItem("",""));

                for (int j = 0; j < valueslist.Count; j++)
                {
                    //var tempItem = new RadComboBoxItem();
                    //tempItem.Text = valueslist[j].Value;
                    if (valueslist[j].Description != "")
                        cmbBox.Items.Add(new ListItem(valueslist[j].Value, fieldName + "-" + valueslist[j].Description));
                    else
                        cmbBox.Items.Add(new ListItem(valueslist[j].Value, fieldName + j));
                }
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            if (DLC.txtDLC.Text != string.Empty && DLC.txtDLC.Text.Length > 32767)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Validation", "{PFSH_SaveUnsuccessful();DisplayErrorMessage('180032');sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                DLC.txtDLC.Focus();
                return;
            }
            if (!Validation())
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Validation", "PFSH_SaveUnsuccessful();DisplayErrorMessage('180020'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            UtilityManager objUtilityMngr = new UtilityManager();
            IList<SocialHistory> listOfSocialToAdd = new List<SocialHistory>();
            IList<SocialHistory> listOfSocialToUpdate = new List<SocialHistory>();
            IList<SocialHistory> listOfSocialToDelete = new List<SocialHistory>();
            SocialHistory SocialHistoryObject = null;
            TextBox description;
            foreach (KeyValuePair<string, string> item in dictionary)
            {
                CheckBox chk = ((CheckBox)divSocialHistoryControls.FindControl("chkYes" + item.Key.Replace(" ", "")));
                CheckBox chkNo = ((CheckBox)divSocialHistoryControls.FindControl("chkNo" + item.Key.Replace(" ", "")));
                //RadComboBox rcb = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed"));//added by Shilpa-reason_not_performed cbo
                CustomDLCNew sSnomed = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", "")));
                TextBox txt = (TextBox)divSocialHistoryControls.FindControl("txt" + item.Key.Replace(" ", "")); 

                if (chk.ID.Contains("chkYes"))
                {
                    if (item.Value == "0")
                    {
                        SocialHistoryObject = new SocialHistory();
                        SocialHistoryObject.Created_By = ClientSession.UserName;
                        SocialHistoryObject.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                    else
                    {
                        if (chkNo.Checked == false && chk.Checked == false && sSnomed.txtDLC.Text.Trim() == string.Empty)//added by Shilpa-reason_not_performed cbo-included condition for cbo
                        {
                            SocialHistoryObject = (from pastList in SocialHistoryDetails
                                                   where pastList.Id == Convert.ToUInt64(item.Value)
                                                   select pastList).ToList<SocialHistory>()[0];

                            if (!listOfSocialToDelete.Contains(SocialHistoryObject) && SocialHistoryObject.Encounter_ID == ClientSession.EncounterId)
                            {
                                SocialHistoryObject.Modified_By = ClientSession.UserName;
                                SocialHistoryObject.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                listOfSocialToDelete.Add(SocialHistoryObject);
                            }
                            continue;
                        }
                        else
                        {
                            SocialHistoryObject = (from pastList in SocialHistoryDetails
                                                   where pastList.Id == Convert.ToUInt64(item.Value)
                                                   select pastList).ToList<SocialHistory>()[0];
                            if ((chk.Checked && SocialHistoryObject.Is_Present == "N") || (chkNo.Checked && SocialHistoryObject.Is_Present == "Y"))
                            {
                                SocialHistoryObject.Modified_By = ClientSession.UserName;
                                SocialHistoryObject.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            }
                        }
                    }
                }
                //added by Shilpa-reason_not_performed cbo
                //SocialHistoryObject.Reason_Not_Performed = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed")).Items.Count > 0 ? ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed")).SelectedItem.Text : string.Empty;
                //string re_NotPerformedcodes = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed")).Items.Count > 0 ? ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed")).SelectedItem.Value.Split(new string[] { "$#%" }, StringSplitOptions.None)[0] : string.Empty;
                //if (re_NotPerformedcodes != "" && re_NotPerformedcodes.Split('-').Count() > 1)
                //    SocialHistoryObject.Snomed_Reason_Not_Performed = re_NotPerformedcodes.Split('-')[1];
                //SocialHistoryObject.Reason_Not_Performed = string.Empty;
                bool bSnomedCheck = false;
                if (sSnomed.txtDLC.Text.Trim() != string.Empty)
                {
                    if (sSnomed.TextControlID.Split(',')[sSnomed.TextControlID.Split(',').Count() - 1] != string.Empty)// "REASON NOT PERFORMED FOR " +
                    {
                        string sSnomed_Code = objUtilityMngr.GetSnomedfromStaticLookup("ReasonNotPerformedList", sSnomed.TextControlID.Split(',')[sSnomed.TextControlID.Split(',').Count() - 1], sSnomed.txtDLC.Text);
                        if (sSnomed.TextControlID.Split(',')[sSnomed.TextControlID.Split(',').Count() - 1].ToUpper().Contains("TOBACCO"))
                        {
                            if (sSnomed_Code == "" && chk.Checked == false && chkNo.Checked == false)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", " displayalerttobacco();", true);
                                return;
                            }
                        }

                        SocialHistoryObject.Snomed_Reason_Not_Performed = sSnomed_Code;
                        bSnomedCheck = true;
                    }
                }
                else
                    SocialHistoryObject.Snomed_Reason_Not_Performed = string.Empty;
                
                //Cap - 3594
                if ( chk.ID.Contains("chkYes") && chk.Checked == true && chk.ID == "chkYesPregnancyStatus")
                {
                    SocialHistoryObject.Is_Present = "Y";
                    SocialHistoryObject.Recodes = "77386006";
                }
                else if (chkNo.ID.Contains("chkNo") && chkNo.Checked == true && chkNo.ID == "chkNoPregnancyStatus")
                {
                    SocialHistoryObject.Is_Present = "N";
                    SocialHistoryObject.Recodes = "60001007";
                }
                //Cap - 3604
                //if (chk.ID.Contains("chkYes") && chk.Checked == true)
                else if (chk.ID.Contains("chkYes") && chk.Checked == true && txt ==null)
                {
                    SocialHistoryObject.Is_Present = "Y";
                    //SocialHistoryObject.Value = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedItem.Text : string.Empty;
                    //string recodes = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedItem.Value.Split(new string[] { "$#%" }, StringSplitOptions.None)[0] : string.Empty;
                    SocialHistoryObject.Value = ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items[((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedIndex].Text : string.Empty;
                    string recodes = ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items[((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedIndex].Value.Split(new string[] { "$#%" }, StringSplitOptions.None)[0] : string.Empty;

                    if (recodes != "" && recodes.Split('-').Count() > 1)
                        SocialHistoryObject.Recodes = recodes.Split('-')[1];
                    SocialHistoryObject.Snomed_Reason_Not_Performed = "";//added by Shilpa-reason_not_performed cbo

                }
                //Cap - 3604                
                else if (chk.ID.Contains("chkYes") && chk.Checked == true && txt!= null)
                {
                    string hdnId = "hdn" + item.Key.Replace(" ", "");
                    HiddenField hdn = divSocialHistoryControls.FindControl(hdnId) as HiddenField;
                    SocialHistoryObject.Is_Present = "Y";
                    SocialHistoryObject.Value = Request.Form["txt" + item.Key.Replace(" ", "")];
                    SocialHistoryObject.Recodes = hdn.Value;
                    SocialHistoryObject.Snomed_Reason_Not_Performed = "";//added by Shilpa-reason_not_performed cbo

                }
                //Cap - 3604
                //else if (chkNo.ID.Contains("chkNo") && chkNo.Checked == true)
                else if (chkNo.ID.Contains("chkNo") && chkNo.Checked == true && txt == null)
                {
                    SocialHistoryObject.Is_Present = "N";
                    //SocialHistoryObject.Value = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedItem.Text : string.Empty;
                    //string recodes = ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedItem.Value.Split(new string[] { "$#%" }, StringSplitOptions.None)[0] : string.Empty;
                    SocialHistoryObject.Value = ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items[((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedIndex].Text : string.Empty;
                    string recodes = ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items.Count > 0 ? ((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).Items[((HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""))).SelectedIndex].Value.Split(new string[] { "$#%" }, StringSplitOptions.None)[0] : string.Empty;

                    if (recodes != "" && recodes.Split('-').Count() > 1)
                        SocialHistoryObject.Recodes = recodes.Split('-')[1];
                    SocialHistoryObject.Snomed_Reason_Not_Performed = "";//added by Shilpa-reason_not_performed cbo

                }
                //Cap - 3604
                else if (chkNo.ID.Contains("chkNo") && chkNo.Checked == true && txt != null)
                {
                    SocialHistoryObject.Is_Present = "N";
                    SocialHistoryObject.Value = txt.Text;
                    SocialHistoryObject.Recodes = "";
                    SocialHistoryObject.Snomed_Reason_Not_Performed = "";//added by Shilpa-reason_not_performed cbo

                }
                else if (bSnomedCheck) //(SocialHistoryObject.Snomed_Reason_Not_Performed.Trim() != "")//added by Shilpa-reason_not_performed cbo
                {
                    SocialHistoryObject.Is_Present = "";
                    SocialHistoryObject.Recodes = "";
                    SocialHistoryObject.Value = "";
                }
                else
                    continue;



                description = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC;
                //if (description != null)
                //{
                //    //if (description.Text.Length > 255)
                //    //{
                //    //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Validation", "PFSH_SaveUnsuccessful();DisplayErrorMessage('180033'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                //    //    description.Focus();
                //    //    return;
                //    //}
                //}

                SocialHistoryObject.Social_Info = item.Key;
                SocialHistoryObject.Human_ID = ClientSession.HumanId;
                SocialHistoryObject.Description = description.Text;
                Label mandatorylbl = (Label)divSocialHistoryControls.FindControl("lbl" + item.Key.Replace(" ", ""));
                //if (mandatorylbl.ForeColor == Color.Red)
                if (mandatorylbl.Text.Contains("*"))
                    SocialHistoryObject.Is_Mandatory = "Y";

                if (item.Value == "0")
                {
                    SocialHistoryObject.Encounter_ID = ClientSession.EncounterId;
                    listOfSocialToAdd.Add(SocialHistoryObject);
                }
                else
                {

                    if (SocialHistoryObject.Encounter_ID == ClientSession.EncounterId)
                    {
                        SocialHistoryObject.Modified_By = ClientSession.UserName;
                        SocialHistoryObject.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        listOfSocialToUpdate.Add(SocialHistoryObject);
                    }
                    else
                    {
                        SocialHistoryObject.Id = 0;
                        SocialHistoryObject.Version = 0;
                        SocialHistoryObject.Encounter_ID = ClientSession.EncounterId;
                        listOfSocialToAdd.Add(SocialHistoryObject);
                    }
                }
            }
            if (Session["GeneralNotesObject"] != null)
            {
                generalNotesObject = (GeneralNotes)Session["GeneralNotesObject"];
                if (generalNotesObject.Id > 0 && generalNotesObject.Encounter_ID == ClientSession.EncounterId)
                {
                    generalNotesObject.Notes = DLC.txtDLC.Text;
                    generalNotesObject.Modified_By = ClientSession.UserName;
                    generalNotesObject.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    generalNotesObject = new GeneralNotes();
                    generalNotesObject.Encounter_ID = ClientSession.EncounterId;
                    generalNotesObject.Human_ID = ClientSession.HumanId;
                    generalNotesObject.Parent_Field = "Social History";
                    generalNotesObject.Notes = DLC.txtDLC.Text;
                    generalNotesObject.Created_By = ClientSession.UserName;
                    generalNotesObject.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
            }
            else
            {
                generalNotesObject = new GeneralNotes();
                generalNotesObject.Encounter_ID = ClientSession.EncounterId;
                generalNotesObject.Human_ID = ClientSession.HumanId;
                generalNotesObject.Parent_Field = "Social History";
                generalNotesObject.Notes = DLC.txtDLC.Text;
                generalNotesObject.Created_By = ClientSession.UserName;
                generalNotesObject.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            }

            #region GetMastertable
            IList<SocialHistoryMaster> lstScoialHisMaster = new List<SocialHistoryMaster>();
            if (HttpContext.Current.Session["SocialHistoryMaster"] != null)
            {
                lstScoialHisMaster = (IList<SocialHistoryMaster>)HttpContext.Current.Session["SocialHistoryMaster"];
            }
            else
            {
                #region "Modified By Balaji.TJ  - 2023-03-01"
                IList<string> lstSocHisMasLst = new List<string>();
                lstSocHisMasLst.Add("SocialHistoryMasterList");

                IList<object> ilstSocHisBlobFinal = new List<object>();
                ilstSocHisBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, lstSocHisMasLst);

                if (ilstSocHisBlobFinal != null && ilstSocHisBlobFinal.Count > 0)
                {
                    if (ilstSocHisBlobFinal[0] != null)
                    {
                        for (int i = 0; i < ((IList<object>)ilstSocHisBlobFinal[0]).Count; i++)
                        {
                            lstScoialHisMaster.Add((SocialHistoryMaster)((IList<object>)ilstSocHisBlobFinal[0])[i]);
                        }
                    }
                }

                #endregion
                #region "Comment By Balaji.TJ  - 2023-03-01"

                //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //XmlDocument itemDoc = new XmlDocument();
                //XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //XmlNodeList xmlTagName = null;
                //using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    itemDoc.Load(fs);
                //    XmlText.Close();
                //    if (itemDoc.GetElementsByTagName("SocialHistoryMasterList")[0] != null)
                //    {
                //        xmlTagName = itemDoc.GetElementsByTagName("SocialHistoryMasterList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(SocialHistoryMaster));
                //                SocialHistoryMaster objHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as SocialHistoryMaster;

                //                IEnumerable<PropertyInfo> propInfo = null;
                //                propInfo = from obji in ((SocialHistoryMaster)objHistory).GetType().GetProperties() select obji;

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
                //                lstScoialHisMaster.Add(objHistory);
                //            }
                //        }
                //    }
                //    fs.Close();
                //    fs.Dispose();

                //}
                #endregion
            }
            #endregion


            #region PastMedicalHistoryMasterTable
            //Save PastmedicalHistoryMaster table
            IList<SocialHistoryMaster> SaveListMaster = new List<SocialHistoryMaster>();
            IList<SocialHistoryMaster> UpdateListMaster = new List<SocialHistoryMaster>();
            IList<SocialHistoryMaster> DeleteListMaster = new List<SocialHistoryMaster>();
            //Social_History_ID, Human_ID, Social_Info, Is_Present, , Value, , Is_Mandatory, Created_By, Created_Date_And_Time,
            //Modified_By, Modified_Date_And_Time, Version, Encounter_ID, Snomed_Reason_Not_Performed, Social_History_Master_ID
            IList<SocialHistoryMaster> lstPMHmasterTemp = new List<SocialHistoryMaster>();
            if (listOfSocialToAdd.Count > 0)
                foreach (SocialHistory objpmh in listOfSocialToAdd)
                {
                    SocialHistoryMaster objAddPMHMaster = new SocialHistoryMaster();
                    lstPMHmasterTemp = lstScoialHisMaster.Where(a => a.Social_Info.Trim().ToUpper() == objpmh.Social_Info.Trim().ToUpper() && a.Is_Deleted == "N").ToList<SocialHistoryMaster>();
                    if (lstPMHmasterTemp.Count() == 0)
                    {
                        objAddPMHMaster.Human_ID = objpmh.Human_ID;
                        objAddPMHMaster.Social_Info = objpmh.Social_Info;
                        objAddPMHMaster.Is_Present = objpmh.Is_Present;
                        objAddPMHMaster.Description = objpmh.Description;
                        objAddPMHMaster.Value = objpmh.Value;
                        objAddPMHMaster.Recodes = objpmh.Recodes;
                        objAddPMHMaster.Is_Mandatory = objpmh.Is_Mandatory;
                        objAddPMHMaster.Created_By = ClientSession.UserName;
                        objAddPMHMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        //objAddPMHMaster.Modified_By = ClientSession.UserName;//objpmh.Modified_By;
                        //objAddPMHMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); ;// objpmh.Modified_Date_And_Time;
                        objAddPMHMaster.Snomed_Reason_Not_Performed = objpmh.Snomed_Reason_Not_Performed;

                        SaveListMaster.Add(objAddPMHMaster);
                    }
                    else
                    {
                        IList<SocialHistoryMaster> tempSessionData = new List<SocialHistoryMaster>();
                        tempSessionData = lstScoialHisMaster.Where(a => a.Social_Info == objpmh.Social_Info && a.Is_Deleted == "N").ToList();
                        foreach (SocialHistoryMaster temp in tempSessionData)
                        {
                            objAddPMHMaster = temp;
                        }
                        objAddPMHMaster.Is_Present = objpmh.Is_Present;
                        objAddPMHMaster.Description = objpmh.Description;
                        objAddPMHMaster.Value = objpmh.Value;
                        objAddPMHMaster.Is_Mandatory = objpmh.Is_Mandatory;
                        objAddPMHMaster.Recodes = objpmh.Recodes;
                        objAddPMHMaster.Is_Mandatory = objpmh.Is_Mandatory;
                        objAddPMHMaster.Modified_By = ClientSession.UserName;//objpmh.Modified_By;
                        objAddPMHMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal(); //objpmh.Modified_Date_And_Time;
                        UpdateListMaster.Add(objAddPMHMaster);
                    }
                }
            //update
            if (listOfSocialToUpdate.Count() > 0)
            {
                foreach (SocialHistory objpmhUpdate in listOfSocialToUpdate)
                {
                    SocialHistoryMaster objAddPMHMaster = new SocialHistoryMaster();
                    IList<SocialHistoryMaster> tempSessionData = new List<SocialHistoryMaster>();
                    tempSessionData = lstScoialHisMaster.Where(a => a.Social_Info == objpmhUpdate.Social_Info && a.Is_Deleted == "N").ToList();
                    foreach (SocialHistoryMaster temp in tempSessionData)
                    {
                        objAddPMHMaster = temp;
                    }
                    objAddPMHMaster.Is_Present = objpmhUpdate.Is_Present;
                    objAddPMHMaster.Description = objpmhUpdate.Description;
                    objAddPMHMaster.Value = objpmhUpdate.Value;
                    objAddPMHMaster.Is_Mandatory = objpmhUpdate.Is_Mandatory;
                    objAddPMHMaster.Recodes = objpmhUpdate.Recodes;
                    objAddPMHMaster.Is_Mandatory = objpmhUpdate.Is_Mandatory;
                    objAddPMHMaster.Modified_By = ClientSession.UserName;//objpmhUpdate.Modified_By;
                    objAddPMHMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();//objpmhUpdate.Modified_Date_And_Time;
                    UpdateListMaster.Add(objAddPMHMaster);
                }
            }

            //delete
            IList<SocialHistoryMaster> SocHisMasterChecklst = null;
            IList<SocialHistoryMaster> DeleteListMasterTemp = new List<SocialHistoryMaster>();
            if (HttpContext.Current.Session["SocialHistoryCheck"] != null)
            {
                SocHisMasterChecklst = (IList<SocialHistoryMaster>)HttpContext.Current.Session["SocialHistoryCheck"];

            }
            //if (UIManager.PFSH_OpeingFrom == "Menu" || (HttpContext.Current.Session["SocialHistoryCheck"] == null  && HttpContext.Current.Session["SocialHistoryMaster"] != null))For bug Id :61754
            if (UIManager.PFSH_OpeingFrom == "Menu" || ((SocHisMasterChecklst == null || SocHisMasterChecklst.Count == 0) && HttpContext.Current.Session["SocialHistoryMaster"] != null))
            {
                if (HttpContext.Current.Session["SocialHistoryMaster"] != null)
                {
                    IList<SocialHistoryMaster> CurrentPastMedListMaster = (IList<SocialHistoryMaster>)HttpContext.Current.Session["SocialHistoryMaster"];
                    if (CurrentPastMedListMaster.Count > 0)
                        DeleteListMasterTemp = (CurrentPastMedListMaster.Where(p => !listOfSocialToAdd.Concat(listOfSocialToUpdate).Any(p2 => p2.Social_Info == p.Social_Info))).ToList<SocialHistoryMaster>();

                    foreach (SocialHistoryMaster temp in DeleteListMasterTemp)
                    {

                        SocialHistoryMaster objAddPMHMaster = new SocialHistoryMaster();
                        objAddPMHMaster = temp;
                        objAddPMHMaster.Is_Deleted = "Y";
                        objAddPMHMaster.Modified_By = ClientSession.UserName;
                        objAddPMHMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateListMaster.Add(objAddPMHMaster);
                    }
                }
            }

            if (listOfSocialToDelete.Count() > 0)
            {
                foreach (SocialHistory objpmhDelete in listOfSocialToDelete)
                {
                    SocialHistoryMaster objAddPMHMaster = new SocialHistoryMaster();
                    IList<SocialHistoryMaster> tempSessionData = new List<SocialHistoryMaster>();
                    tempSessionData = lstScoialHisMaster.Where(a => a.Social_Info == objpmhDelete.Social_Info && a.Is_Deleted == "N").ToList();
                    foreach (SocialHistoryMaster temp in tempSessionData)
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
            //Save Master First
            #endregion

            if (UIManager.PFSH_OpeingFrom == "Menu")
            {
                listOfSocialToAdd = new List<SocialHistory>();
                listOfSocialToUpdate = new List<SocialHistory>();
                listOfSocialToDelete = new List<SocialHistory>();
                generalNotesObject = new GeneralNotes();
            }
            problemDTO = socialHistoryMngr.SaveUpdateDeleteSocialHistory(listOfSocialToAdd, listOfSocialToUpdate, listOfSocialToDelete, ClientSession.HumanId, generalNotesObject, string.Empty, SaveListMaster, UpdateListMaster, DeleteListMaster);
            IList<SocialHistoryMaster> lstSocHisMaster = null;
            if (SaveListMaster.Count() > 0 && UpdateListMaster.Count() > 0)
            {
                lstSocHisMaster = new List<SocialHistoryMaster>();
                lstSocHisMaster = SaveListMaster.Concat(UpdateListMaster).Where(p => p.Is_Deleted == "N").ToList();
            }
            else if (SaveListMaster.Count() > 0)
            {
                lstSocHisMaster = new List<SocialHistoryMaster>();
                lstSocHisMaster = SaveListMaster;
            }
            else if (UpdateListMaster.Count() > 0)
            {
                lstSocHisMaster = new List<SocialHistoryMaster>();
                lstSocHisMaster = UpdateListMaster.Where(p => p.Is_Deleted == "N").ToList();
            }
            Session["SocialHistoryMaster"] = lstSocHisMaster;
            if (ClientSession.EncounterId != 0 && UIManager.PFSH_OpeingFrom != "Menu")
            {
                if (ClientSession.FillEncounterandWFObject != null)
                {
                    if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                    {
                        if (ClientSession.FillEncounterandWFObject.EncRecord.Id > 0)
                        {
                            ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved = "N";
                            IList<Encounter> lst = new List<Encounter>();
                            IList<Encounter> lsttemp = new List<Encounter>();
                            lst.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                            EncounterManager obj = new EncounterManager();
                            obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.FillEncounterandWFObject.EncRecord.Id, string.Empty);
                            ClientSession.FillEncounterandWFObject.EncRecord = lst[0];

                        }

                        else
                        {
                            IList<Encounter> lst = new List<Encounter>();
                            IList<Encounter> lsttemp = new List<Encounter>();


                            EncounterManager encobj = new EncounterManager();
                            lst = encobj.GetEncounterByEncounterID(ClientSession.EncounterId);
                            if (lst.Count > 0)
                            {
                                lst[0].is_serviceprocedure_saved = "N";
                                encobj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.EncounterId, string.Empty);

                            }
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Save", " warningmethod();SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + "); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} RefreshNotification('SocialHistory');LoadSocialHistory();", true);
            ClientSession.bPFSHVerified = false;
            UIManager.IsPFSHVerified = true;
            btnSave.Enabled = false;
            if (problemDTO.SocialList != null)
            {
                if (problemDTO.SocialList.Count > 1)
                    ClientSession.IsDirtySocialHistory = false;
                else
                    ClientSession.IsDirtySocialHistory = true;
            }
            Session["SocialHistoryDetails"] = problemDTO.SocialList;
            SocialHistoryDetails = problemDTO.SocialList;
            Session["GeneralNotesObject"] = problemDTO.GeneralNotesObject;
            //LoadAfterClearAll();//added by Shilpa-reason_not_performed cbo
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            LoadAfterClearAll();
            CheckBox checkYesBox = (CheckBox)divSocialHistoryHeaderControls.FindControl("chkAllYes");
            if (checkYesBox != null)
                checkYesBox.Checked = false;
            CheckBox checkNoBox = (CheckBox)divSocialHistoryHeaderControls.FindControl("chkAllNo");
            if (checkNoBox != null)
                checkNoBox.Checked = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        void LoadAfterClearAll()
        {
            //problemDTO = socialHistoryMngr.GetSocialHistoryByHumanID(ClientSession.HumanId, ClientSession.EncounterId, "SOCIAL HISTORY", true);
            problemDTO = new SocialHistoryDTO();
            if (UIManager.PFSH_OpeingFrom == "Menu")
            {
                IList<SocialHistoryMaster> lstSocialHistoryMaster = new List<SocialHistoryMaster>();
                if (Session["SocialHistoryMaster"] != null)
                {
                    lstSocialHistoryMaster = (List<SocialHistoryMaster>)Session["SocialHistoryMaster"];
                    problemDTO.GeneralNotesObject = (GeneralNotes)Session["GeneralNotesObject"]; //Commented For Bug ID:36466
                }
                else
                {
                    problemDTO = GetSocialHistory();
                    problemDTO.GeneralNotesObject = null;//Added For Bug ID:36466
                }
                generalNotesObject = problemDTO.GeneralNotesObject;
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    CheckBox checkYesBox = (CheckBox)divSocialHistoryControls.FindControl("chkYes" + item.Key.Replace(" ", ""));
                    CheckBox checkNoBox = (CheckBox)divSocialHistoryControls.FindControl("chkNo" + item.Key.Replace(" ", ""));
                    //RadComboBox cboOption = (RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""));
                    HtmlSelect cboOption = (HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""));
                    //RadComboBox cboReasonOption = (RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed");//added by Shilpa-reason_not_performed cbo
                    TextBox description = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC;
                    CustomDLCNew userCtrl = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", "")));
                    #region Commented By Deepak 
                    //CAP-1557 - Uncomment Code
                    var rootMaster = new List<SocialHistoryMaster>();
                    if (lstSocialHistoryMaster != null)
                    {
                        rootMaster = (from pastList in lstSocialHistoryMaster
                                      where pastList.Social_Info == item.Key
                                      select pastList).ToList<SocialHistoryMaster>();
                    }
                    if (checkYesBox != null && checkNoBox != null && cboOption != null && description != null && userCtrl != null)
                    {
                        if (rootMaster.Count() > 0)
                        {
                            SocialHistoryMaster ProblemHistoryObject = rootMaster.ToList<SocialHistoryMaster>()[0];
                            if (ProblemHistoryObject.Is_Present == "Y")
                            {
                                checkYesBox.Checked = true;
                                checkNoBox.Checked = false;
                                cboOption.Disabled = false;

                                for (int i = 0; i < cboOption.Items.Count; i++)
                                {
                                    if (cboOption.Items[i].Text == ProblemHistoryObject.Value)
                                        cboOption.SelectedIndex = i;
                                }
                                description.Text = ProblemHistoryObject.Description;
                                description.Enabled = true;
                            }
                            else if (ProblemHistoryObject.Is_Present == "N")
                            {
                                checkYesBox.Checked = false;
                                checkNoBox.Checked = true;
                                if (item.Key == "Tobacco Use and Exposure")
                                {
                                    for (int i = 0; i < cboOption.Items.Count; i++)
                                    {
                                        if (cboOption.Items[i].Text == ProblemHistoryObject.Value)
                                            cboOption.SelectedIndex = i;
                                    }
                                    cboOption.Disabled = false;
                                }
                                else
                                {
                                    //cboOption.ClearSelection();
                                    ClearSelection(cboOption);
                                    // cboOption.SelectedIndex = 0;
                                    cboOption.Disabled = true;
                                }

                                description.Text = ProblemHistoryObject.Description;
                                description.Enabled = true;
                            }
                            else if (ProblemHistoryObject.Is_Present.Trim() == "")//added by Shilpa-reason_not_performed cbo
                            {
                                checkYesBox.Checked = false;
                                checkNoBox.Checked = false;
                                description.Text = ProblemHistoryObject.Description;
                                //description.Enabled = false;
                                //cboOption.ClearSelection();
                                ClearSelection(cboOption);
                                //cboOption.SelectedIndex = 0;
                                cboOption.Disabled = true;
                                //if (ProblemHistoryObject.Snomed_Reason_Not_Performed.Trim() != "")
                                //{
                                //for (int i = 0; i < cboReasonOption.Items.Count; i++)
                                //{
                                //    if (cboReasonOption.Items[i].Text == ProblemHistoryObject.Reason_Not_Performed)
                                //        cboReasonOption.SelectedIndex = i;
                                //}
                                //cboReasonOption.Enabled = true;
                                //}
                            }
                        }
                        else
                        {
                            checkYesBox.Checked = false;
                            checkNoBox.Checked = false;
                            ClearSelection(cboOption);
                            description.Text = string.Empty;
                            //description.Enabled = false;
                            userCtrl.Enable = false;
                            cboOption.Disabled = true;
                            //cboReasonOption.Enabled = true;
                        }
                    }
                    #endregion
                    if (item.Key.Replace(" ", "") == "TobaccoUseandExposure")
                    {
                        cboOption.Items.Insert(0, new ListItem("", ""));
                        cboOption.Items[0].Attributes.Add("Option", "Yes");
                    }
                    cboOption.SelectedIndex = 0;
                    cboOption.Disabled = true;
                    checkYesBox.Checked = false;
                    checkNoBox.Checked = false;
                    description.Text = string.Empty;
                    userCtrl.Enable = true;

                }
            }
            else
            {
                if (Session["SocialHistoryDetails"] != null)
                {
                    problemDTO.SocialList = (List<SocialHistory>)Session["SocialHistoryDetails"];
                    problemDTO.GeneralNotesObject = (GeneralNotes)Session["GeneralNotesObject"]; //Commented For Bug ID:36466
                }
                else
                {
                    problemDTO = GetSocialHistory();
                    problemDTO.GeneralNotesObject = null;//Added For Bug ID:36466
                }
                SocialHistoryDetails = problemDTO.SocialList;
                generalNotesObject = problemDTO.GeneralNotesObject;
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    CheckBox checkYesBox = (CheckBox)divSocialHistoryControls.FindControl("chkYes" + item.Key.Replace(" ", ""));
                    CheckBox checkNoBox = (CheckBox)divSocialHistoryControls.FindControl("chkNo" + item.Key.Replace(" ", ""));
                    //RadComboBox cboOption = (RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""));
                    HtmlSelect cboOption = (HtmlSelect)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", ""));
                    TextBox description = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC;
                    CustomDLCNew userCtrl = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", "")));

                    if (item.Key.Replace(" ", "") == "TobaccoUseandExposure")
                    {
                        cboOption.Items.Insert(0, new ListItem("", ""));
                        cboOption.Items[0].Attributes.Add("Option", "Yes");
                    }
                    cboOption.SelectedIndex = 0;
                    cboOption.Disabled = true;
                    checkYesBox.Checked = false;
                    checkNoBox.Checked=false;
                    description.Text = string.Empty;
                    userCtrl.Enable = true;
                    #region tempcomment by deepak
                    //CAP-1557 - Uncomment Code
                    //RadComboBox cboReasonOption = (RadComboBox)divSocialHistoryControls.FindControl("cbo" + item.Key.Replace(" ", "") + "ReasonNotPerformed");//added by Shilpa-reason_not_performed cbo
                    //TextBox description = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC;
                    //CustomDLCNew userCtrl = ((CustomDLCNew)divSocialHistoryControls.FindControl("DLC" + item.Key.Replace(" ", "")));
                    var root = new List<SocialHistory>();
                    if (SocialHistoryDetails != null)
                    {
                        root = (from pastList in SocialHistoryDetails
                                where pastList.Social_Info == item.Key
                                select pastList).ToList<SocialHistory>();
                    }
                    if (checkYesBox != null && checkNoBox != null && cboOption != null && description != null && userCtrl != null)
                    {
                        if (root.Count() > 0)
                        {
                            SocialHistory ProblemHistoryObject = root.ToList<SocialHistory>()[0];
                            if (ProblemHistoryObject.Is_Present == "Y")
                            {
                                checkYesBox.Checked = true;
                                checkNoBox.Checked = false;
                                //cboOption.Enabled = true;
                                cboOption.Disabled = false;
                                for (int i = 0; i < cboOption.Items.Count; i++)
                                {
                                    if (cboOption.Items[i].Text == ProblemHistoryObject.Value)
                                        cboOption.SelectedIndex = i;
                                }
                                description.Text = ProblemHistoryObject.Description;
                                description.Enabled = true;
                            }
                            else if (ProblemHistoryObject.Is_Present == "N")
                            {
                                checkYesBox.Checked = false;
                                checkNoBox.Checked = true;
                                if (item.Key == "Tobacco Use and Exposure")
                                {
                                    for (int i = 0; i < cboOption.Items.Count; i++)
                                    {
                                        if (cboOption.Items[i].Text == ProblemHistoryObject.Value)
                                            cboOption.SelectedIndex = i;
                                    }
                                    //cboOption.Enabled = true;
                                    cboOption.Disabled = false;
                                }
                                else
                                {
                                    //cboOption.ClearSelection();
                                    ClearSelection(cboOption);
                                    //cboOption.Enabled = false;
                                    cboOption.Disabled = true;
                                }

                                description.Text = ProblemHistoryObject.Description;
                                description.Enabled = true;
                            }
                            else if (ProblemHistoryObject.Is_Present.Trim() == "")//added by Shilpa-reason_not_performed cbo
                            {
                                checkYesBox.Checked = false;
                                checkNoBox.Checked = false;
                                description.Text = ProblemHistoryObject.Description;
                                //description.Enabled = false;
                                //cboOption.ClearSelection();
                                ClearSelection(cboOption);
                                cboOption.SelectedIndex = 0;
                                //cboOption.Enabled = false;
                                cboOption.Disabled = true;
                                //if (ProblemHistoryObject.Snomed_Reason_Not_Performed.Trim() != "")
                                //{
                                //for (int i = 0; i < cboReasonOption.Items.Count; i++)
                                //{
                                //    if (cboReasonOption.Items[i].Text == ProblemHistoryObject.Reason_Not_Performed)
                                //        cboReasonOption.SelectedIndex = i;
                                //}
                                //cboReasonOption.Enabled = true;
                                //}
                            }
                        }
                        else
                        {
                            checkYesBox.Checked = false;
                            checkNoBox.Checked = false;
                            //cboOption.ClearSelection();
                            ClearSelection(cboOption);
                            description.Text = string.Empty;
                            //description.Enabled = false;
                            //userCtrl.Enable = false;
                            userCtrl.Enable = true;
                            //cboOption.Enabled = false;
                            cboOption.Disabled = true;
                            //cboReasonOption.Enabled = true;
                        }
                    }
                }
            }
            if (generalNotesObject != null)
                DLC.txtDLC.Text = generalNotesObject.Notes;
            else
                DLC.txtDLC.Text = string.Empty;
            #endregion
        }

        public bool Validation()
        {
            bool result = true;
            if (mandatoryList.Count > 0)
            {
                for (int i = 0; i < mandatoryList.Count; i++)
                {
                    CheckBox yeschkCtrl = (CheckBox)divSocialHistoryControls.FindControl(mandatoryList[i].ToString().Replace(" ", "").Replace("lblsocial", "chkYes"));
                    if (yeschkCtrl != null && !yeschkCtrl.Checked)
                    {
                        CheckBox nochkCtrl = (CheckBox)divSocialHistoryControls.FindControl(mandatoryList[i].ToString().Replace(" ", "").Replace("lblsocial", "chkNo"));
                        if (nochkCtrl != null && !nochkCtrl.Checked)
                        {
                            if (mandatoryList[i].ToString().IndexOf("Tobacco") > -1)//added by Shilpa-reason_not_performed cbo
                            {
                                string ComboBoxID = mandatoryList[i].ToString().Replace(" ", "").Replace("lblsocial", "DLC");
                                CustomDLCNew rcbReasonNtPerformed = (CustomDLCNew)divSocialHistoryControls.FindControl(ComboBoxID);
                                if (rcbReasonNtPerformed.txtDLC != null && rcbReasonNtPerformed.txtDLC.Text == "")
                                {
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                result = false;
                                break;
                            }
                        }
                    }

                }
            }
            return result;
        }

        public void LoadTobacco(bool? bValue, string fieldName)
        {
            //CAP-2143
            List<string> istaticLookup = new List<string>();
            if (Session["objStaticLookupComboValues"] != null && (IList<StaticLookup>)Session["objStaticLookupComboValues"] != null)
            {
                IList<StaticLookup> staticLookupList = (IList<StaticLookup>)Session["objStaticLookupComboValues"];
                staticLookupList = staticLookupList.Where(a => a.Field_Name.Contains("SOCIAL HISTORY OPTION FOR TOBACCO")).ToList();
                foreach (var item in staticLookupList)
                {
                    string Field_Name = item.Field_Name.Split(' ')[item.Field_Name.Split(' ').Length - 1];
                    Field_Name = Field_Name == "YES" ? "Yes" : Field_Name == "NO" ? "No" : "";
                    istaticLookup.Add(string.Format("{0}|{1}|{2}|{3}", item.Value, item.Description, Field_Name, item.Default_Value));
                }
            }

            //istaticLookup = new List<String>{"Light cigarette smoker|160603005|Yes|Light cigarette smoker","Moderate cigarette smoker|160604004|Yes|Light cigarette smoker","Heavy cigarette smoker|160605003|Yes|Light cigarette smoker","Very heavy cigarette smoker|160606002|Yes|Light cigarette smoker","Rolls own cigarettes|160619003|Yes|Light cigarette smoker",
            //        "Snuff user|228494002|Yes|Light cigarette smoker","User of moist powdered tobacco|228504007|Yes|Light cigarette smoker","Chews plug tobacco|228514003|Yes|Light cigarette smoker","Chews twist tobacco|228515002|Yes|Light cigarette smoker","Chews loose leaf tobacco|228516001|Yes|Light cigarette smoker","Chews fine cut tobacco|228517005|Yes|Light cigarette smoker",
            //        "Chews products containing tobacco|228518000|Yes|Light cigarette smoker","Occasional cigarette smoker|230059006|Yes|Light cigarette smoker","Chain smoker|230065006|Yes|Light cigarette smoker","Trivial cigarette smoker|266920004|Yes|Light cigarette smoker","Occasional tobacco smoker|428041000124106|Yes|Light cigarette smoker","Light tobacco smoker|428061000124105|Yes|Light cigarette smoker","Heavy tobacco smoker|428071000124103|Yes|Light cigarette smoker",
            //        "Smokes tobacco daily|449868002|Yes|Light cigarette smoker","Cigar smoker|59978006|Yes|Light cigarette smoker","Cigarette smoker|65568007|Yes|Light cigarette smoker","Smoker|77176002|Yes|Light cigarette smoker","Chews tobacco|81703003|Yes|Light cigarette smoker","Pipe smoker|82302008|Yes|Light cigarette smoker"};
            //istaticLookup.AddRange(new List<String>{"Non-smoker for personal reasons|105539002|No|Current non-smoker","Non-smoker for religious reasons|105540000|No|Current non-smoker",
            //        "Non-smoker for medical reasons|105541001|No|Current non-smoker","Current non-smoker|160618006|No|Current non-smoker","Ex-pipe smoker|160620009|No|Current non-smoker","Ex-cigar smoker|160621008|No|Current non-smoker","Does not use moist powdered tobacco|228501004|No|Current non-smoker","Never used moist powdered tobacco|228502006|No|Current non-smoker",
            //        "Ex-user of moist powdered tobacco|228503001|No|Current non-smoker","Never chewed tobacco|228512004|No|Current non-smoker","Never smoked tobacco|266919005|No|Current non-smoker","Ex-trivial cigarette smoker|266921000|No|Current non-smoker","Ex-light cigarette smoker|266922007|No|Current non-smoker","Ex-moderate cigarette smoker|266923002|No|Current non-smoker",
            //        "Ex-heavy cigarette smoker|266924008|No|Current non-smoker","Ex-very heavy cigarette smoker|266925009|No|Current non-smoker","Ex-cigarette smoker amount unknown|266928006|No|Current non-smoker","Ex-cigarette smoker|281018007|No|Current non-smoker","Intolerant ex-smoker|360890004|No|Current non-smoker","Aggressive ex-smoker|360900008|No|Current non-smoker",
            //        "Aggressive non-smoker|360918006|No|Current non-smoker","Intolerant non-smoker|360929005|No|Current non-smoker","Current non smoker but past smoking history unknown|405746006|No|Current non-smoker","Tolerant ex-smoker|53896009|No|Current non-smoker","Non-smoker|8392000|No|Current non-smoker","Ex-smoker|8517006|No|Current non-smoker","Tolerant non-smoker|87739003|No|Current non-smoker"});


//            istaticLookup = new List<string>{"Light cigarette smoker|230060001|Yes|Light cigarette smoker",
//"Moderate cigarette smoker|230062009|Yes|Light cigarette smoker",
//"Heavy cigarette smoker|230063004|Yes|Light cigarette smoker",
//"Very heavy cigarette smoker|230064005|Yes|Light cigarette smoker",
//"Rolls own cigarettes|160619003|Yes|Light cigarette smoker",
//"Snuff user|228494002|Yes|Light cigarette smoker",
//"User of moist powdered tobacco|228504007|Yes|Light cigarette smoker",
//"Chews plug tobacco|228514003|Yes|Light cigarette smoker",
//"Chews twist tobacco|228515002|Yes|Light cigarette smoker",
//"Chews loose leaf tobacco|228516001|Yes|Light cigarette smoker",
//"Chews fine cut tobacco|228517005|Yes|Light cigarette smoker",
//"Chews products containing tobacco|228518000|Yes|Light cigarette smoker",
//"Occasional cigarette smoker|230059006|Yes|Light cigarette smoker",
//"Chain smoker|230065006|Yes|Light cigarette smoker",
//"Trivial cigarette smoker|266920004|Yes|Light cigarette smoker",
//"Occasional tobacco smoker|428041000124106|Yes|Light cigarette smoker",
//"Light tobacco smoker|428061000124105|Yes|Light cigarette smoker",
//"Heavy tobacco smoker|428071000124103|Yes|Light cigarette smoker",
//"Smokes tobacco daily|449868002|Yes|Light cigarette smoker",
//"Cigar smoker|59978006|Yes|Light cigarette smoker",
//"Cigarette smoker|65568007|Yes|Light cigarette smoker",
//"Smoker|77176002|Yes|Light cigarette smoker",
//"Chews tobacco|81703003|Yes|Light cigarette smoker",
//"Pipe smoker|82302008|Yes|Light cigarette smoker",
// "Light cigarette smoker (1-9 cigs/day)|160603005|Yes|Light cigarette smoker",
//"Moderate cigarette smoker (10-19 cigs/day)|160604004|Yes|Light cigarette smoker",
//"Heavy cigarette smoker (20-39 cigs/day)|160605003|Yes|Light cigarette smoker",
//"Very heavy cigarette smoker (40+ cigs/day)|160606002|Yes|Light cigarette smoker"};

//            istaticLookup.AddRange(new List<string> {"Non-smoker for personal reasons|105539002|No|Current non-smoker",
//"Non-smoker for religious reasons|105540000|No|Current non-smoker",
//"Non-smoker for medical reasons|105541001|No|Current non-smoker",
//"Current non-smoker|160618006|No|Current non-smoker",
//"Ex-pipe smoker|160620009|No|Current non-smoker",
//"Ex-cigar smoker|160621008|No|Current non-smoker",
//"Does not use moist powdered tobacco|228501004|No|Current non-smoker",
//"Never used moist powdered tobacco|228502006|No|Current non-smoker",
//"Ex-user of moist powdered tobacco|228503001|No|Current non-smoker",
//"Never chewed tobacco|228512004|No|Current non-smoker",
//"Never smoked tobacco|266919005|No|Current non-smoker",
//"Ex-trivial cigarette smoker|266921000|No|Current non-smoker",
//"Ex-light cigarette smoker|266922007|No|Current non-smoker",
//"Ex-moderate cigarette smoker|266923002|No|Current non-smoker",
//"Ex-heavy cigarette smoker|266924008|No|Current non-smoker",
//"Ex-very heavy cigarette smoker|266925009|No|Current non-smoker",
//"Ex-cigarette smoker amount unknown|266928006|No|Current non-smoker",
//"Ex-cigarette smoker|281018007|No|Current non-smoker",
//"Intolerant ex-smoker|360890004|No|Current non-smoker",
//"Aggressive ex-smoker|360900008|No|Current non-smoker",
//"Aggressive non-smoker|360918006|No|Current non-smoker",
//"Intolerant non-smoker|360929005|No|Current non-smoker",
//"Current non smoker but past smoking history unknown|405746006|No|Current non-smoker",
//"Tolerant ex-smoker|53896009|No|Current non-smoker",
//"Non-smoker|8392000|No|Current non-smoker",
//"Ex-smoker|8517006|No|Current non-smoker",
//"Tolerant non-smoker|87739003|No|Current non-smoker"});

            // RadComboBox cmbBox = (RadComboBox)divSocialHistoryControls.FindControl("cbo" + fieldName.Replace(" ", ""));
            HtmlSelect cmbBox = (HtmlSelect)divSocialHistoryControls.FindControl("cbo" + fieldName.Replace(" ", ""));
            cmbBox.Items.Clear();
            //cmbBox.OnClientLoad = "LoadcboTobacco";
            //cmbBox.Attributes.Add("onclick", "LoadcboTobacco()");
            if (bValue != null)
                cmbBox.Attributes.Add("CheckedValue", bValue.ToString());
            else
                cmbBox.Attributes.Add("CheckedValue", "None");
            if (istaticLookup != null)
            {
                //cmbBox.Items.Add(new RadComboBoxItem());
                //for (int j = 0; j < istaticLookup.Count; j++)
                //{
                //    RadComboBoxItem tempItem = new RadComboBoxItem();
                //    string[] Tobacco = istaticLookup[j].Split('|');
                //    tempItem.Text = Tobacco[0];
                //    tempItem.Value = fieldName.Replace(" ", "") + "-" + Tobacco[1] + "-" + Tobacco[3];
                //    tempItem.Attributes.Add("Option", Tobacco[2]);
                //    cmbBox.Items.Add(tempItem);
                //}
                for (int j = 0; j < istaticLookup.Count; j++)
                {
                    string[] Tobacco = istaticLookup[j].Split('|');
                    cmbBox.Items.Add(new ListItem(Tobacco[0], fieldName.Replace(" ", "") + "-" + Tobacco[1] + "-" + Tobacco[3]));
                    cmbBox.Items[j].Attributes.Add("Option", Tobacco[2]);

                }
            }
        }

        public void ClearSelection(HtmlSelect Iteams)
        {
            //for (int iCount=0;iCount< Iteams.Size;iCount++)
            //{
                Iteams.SelectedIndex=0;
            //}
        }
        //Cap - 3604
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetOccupationIndustry(string text_searched, string TextLabel)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";

            }
            StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
            IList<StaticLookup> lststaticOccupation = new List<StaticLookup>();

            lststaticOccupation = objStaticLookupMgr.GetStaticLookupByFieldNameAndPartialValue("SOCIAL HISTORY OPTION FOR "+TextLabel, text_searched);

            if (lststaticOccupation.Count() == 0)
            {
                var lstFinalResult = new
                {
                    Result = "No matches found."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
            else
            {
                var lstFinalResult = new
                {
                    Matching_Result = lststaticOccupation
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }


        }
    }
}
