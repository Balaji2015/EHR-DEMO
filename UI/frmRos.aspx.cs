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
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using System.Drawing;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Acurus.Capella.UI
{
    public partial class frmRos : SessionExpired
    {
        #region Declaration

        IList<FieldLookup> symptomNamesLookUp = null;

        TableRow objPanelTableRow = new TableRow();
        TableCell objPanelTableCell = new TableCell();
        FillROS fillRosPastEncounter = new FillROS();
        int symptomWidth = 100;
        ROSManager objROSManager = new ROSManager();
        bool Is_copy_previous = false;
        #endregion


        void CopyPrevControls(FillROS fillROSEncounter, bool isOK)
        {
            if (isOK)
            {
                pnlReviewOfSystems.Controls.Clear();
            }

            IList<int> noOfSymptomRows = new List<int>();
            IList<int> symptomMaxHeight = new List<int>();

            groupBoxCreation(fillROSEncounter, noOfSymptomRows, symptomMaxHeight);
            checkBoxAndOtherControlsCreation(fillROSEncounter, noOfSymptomRows, symptomMaxHeight);
            chkAllOtherSystemsNormal.Attributes.Add("OnClick", "chkAllOtherSystemsNormalClick('" + systemNamesForEventHF.Value + "')");
        }

        #region Events

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var fillRos = new FillROS();

                if (Request.Form["__EVENTTARGET"] == "btnPastEncounter")
                {
                    fillRosPastEncounter = objROSManager.GetRosAndGeneralNotesForPastEncounter(ClientSession.EncounterId,
                                                                                               ClientSession.HumanId,
                                                                                               ClientSession.PhysicianId);

                    if (fillRosPastEncounter.Ros_List.Count > 0)
                    {
                        //Jira CAP-1923
                        //Session["fillRos"] = fillRosPastEncounter;
                        Session["fillPrevRos"] = fillRosPastEncounter;
                        CopyPrevControls(fillRosPastEncounter, true);
                    }
                    else
                    {
                        fillRos = (Session["fillRos"] != null && ((FillROS)Session["fillRos"]).Ros_List.Count>0) ? (FillROS)Session["fillRos"] :
                                objROSManager.GetROSAndGeneralNotesByEncounterId(ClientSession.EncounterId, ClientSession.HumanId, false);
                        CopyPrevControls(fillRos, false);
                        //Jira CAP-1923
                        //Session["fillRos"] = fillRosPastEncounter;
                        Session["fillRos"] = fillRos;
                        //Jira CAP-1923
                        Session["fillPrevRos"] = fillRosPastEncounter;
                    }
                }
                else
                {
                    fillRos = Session["fillRos"] != null ? (FillROS)Session["fillRos"] :
                            objROSManager.GetROSAndGeneralNotesByEncounterId(ClientSession.EncounterId, ClientSession.HumanId, false);
                    CopyPrevControls(fillRos, false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (isResetOK.Value != "true")
            {
                IList<int> noOfSymptomRows = new List<int>();
                IList<int> symptomMaxHeight = new List<int>();
                FillROS fillRos = new FillROS();

                if (HdnCopyButton.Value == "CheckSave")
                {
                    HdnCopyButton.Value = "";
                    CopyPreviousForROS(ClientSession.HumanId, ClientSession.EncounterId, ClientSession.PhysicianId, false);
                }
                if (!IsPostBack)
                {
                    ROSManager objROSManager = new ROSManager();
                    ClientSession.FlushSession();
                    IList<string> ilstROSTagList = new List<string>();
                    ilstROSTagList.Add("ROSList");
                    ilstROSTagList.Add("GeneralNotesROSList");
                    ilstROSTagList.Add("GeneralNotesROSGeneralNotesList");
                                      

                    IList<object> ilstROSBlobFinal = new List<object>();
                    ilstROSBlobFinal = UtilityManager.ReadBlob( ClientSession.EncounterId, ilstROSTagList);

                    if (ilstROSBlobFinal != null && ilstROSBlobFinal.Count > 0)
                    {
                        if (ilstROSBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstROSBlobFinal[0]).Count; iCount++)
                            {
                                fillRos.Ros_List.Add((ROS)((IList<object>)ilstROSBlobFinal[0])[iCount]);
                            }
                        }

                        if (ilstROSBlobFinal[1] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstROSBlobFinal[1]).Count; iCount++)
                            {
                                fillRos.General_Notes_List.Add((GeneralNotes)((IList<object>)ilstROSBlobFinal[1])[iCount]);
                            }
                        }

                        if (ilstROSBlobFinal[2] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstROSBlobFinal[2]).Count; iCount++)
                            {
                                fillRos.ROS_GeneralNotes_List.Add((GeneralNotes)((IList<object>)ilstROSBlobFinal[2])[iCount]);
                            }
                        }
                    }

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
                    //        if (itemDoc.GetElementsByTagName("ROSList")[0] != null)
                    //        {
                    //            xmlTagName = itemDoc.GetElementsByTagName("ROSList")[0].ChildNodes;
                    //            IList<ROS> ilst = new List<ROS>();
                    //            if (xmlTagName.Count > 0)
                    //            {
                    //                for (int j = 0; j < xmlTagName.Count; j++)
                    //                {
                    //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ROS));
                    //                    ROS objros = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ROS;

                    //                    IEnumerable<PropertyInfo> propInfo = null;
                    //                    propInfo = from obji in ((ROS)objros).GetType().GetProperties() select obji;

                    //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                    //                    {
                    //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                    //                        {
                    //                            foreach (PropertyInfo property in propInfo)
                    //                            {
                    //                                if (property.Name == nodevalue.Name)
                    //                                {
                    //                                    if (propInfo != null)
                    //                                    {
                    //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                    //                                            property.SetValue(objros, Convert.ToUInt64(nodevalue.Value), null);
                    //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                    //                                            property.SetValue(objros, Convert.ToString(nodevalue.Value), null);
                    //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                    //                                            property.SetValue(objros, Convert.ToDateTime(nodevalue.Value), null);
                    //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                    //                                            property.SetValue(objros, Convert.ToInt32(nodevalue.Value), null);
                    //                                        else
                    //                                            property.SetValue(objros, nodevalue.Value, null);
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }

                    //                    ilst.Add(objros);
                    //                    fillRos.Ros_List = ilst;

                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //fillRos = objROSManager.GetROSAndGeneralNotesByEncounterId(ClientSession.EncounterId, ClientSession.HumanId, false);
                    //            fillRos.General_Notes_List = new List<GeneralNotes>();
                    //            fillRos.ROS_GeneralNotes_List = new List<GeneralNotes>();
                    //            fillRos.Ros_List = new List<ROS>();
                    //        }
                    //        //
                    //        if (itemDoc.GetElementsByTagName("GeneralNotesROSList")[0] != null)
                    //        {
                    //            xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesROSList")[0].ChildNodes;
                    //            IList<GeneralNotes> ilst = new List<GeneralNotes>();
                    //            if (xmlTagName.Count > 0)
                    //            {
                    //                for (int j = 0; j < xmlTagName.Count; j++)
                    //                {
                    //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                    //                    GeneralNotes generalnotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as GeneralNotes;
                    //                    IEnumerable<PropertyInfo> propInfo = null;

                    //                    propInfo = from obji in ((GeneralNotes)generalnotes).GetType().GetProperties() select obji;

                    //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                    //                    {
                    //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                    //                        {
                    //                            foreach (PropertyInfo property in propInfo)
                    //                            {
                    //                                if (property.Name == nodevalue.Name)
                    //                                {
                    //                                    if (propInfo != null)
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
                    //                    ilst.Add(generalnotes);
                    //                    fillRos.General_Notes_List = ilst;
                    //                }
                    //            }
                    //        }
                    //        //
                    //        if (itemDoc.GetElementsByTagName("GeneralNotesROSGeneralNotesList")[0] != null)
                    //        {
                    //            xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesROSGeneralNotesList")[0].ChildNodes;

                    //            if (xmlTagName.Count > 0)
                    //            {
                    //                IList<GeneralNotes> ilst = new List<GeneralNotes>();
                    //                for (int j = 0; j < xmlTagName.Count; j++)
                    //                {
                    //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                    //                    GeneralNotes generalnotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as GeneralNotes;
                    //                    IEnumerable<PropertyInfo> propInfo = null;

                    //                    propInfo = from obji in ((GeneralNotes)generalnotes).GetType().GetProperties() select obji;

                    //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                    //                    {
                    //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                    //                        {
                    //                            if (propInfo != null)
                    //                            {
                    //                                foreach (PropertyInfo property in propInfo)
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
                    //                    ilst.Add(generalnotes);
                    //                    fillRos.ROS_GeneralNotes_List = ilst;
                    //                }
                    //            }

                    //        }
                    //        fs.Close();
                    //        fs.Dispose();
                    //    }
                    //}
                    ClientSession.FlushSession();
                    dlcROS.DName = "pbGeneralNotesDropDown";

                    hdnChkToggleState.Value = "false";

                    Session["fillRos"] = fillRos;
                    if (fillRos.ROS_GeneralNotes_List != null && fillRos.ROS_GeneralNotes_List.Count > 0)
                        dlcROS.txtDLC.Text = fillRos.ROS_GeneralNotes_List[0].Notes;

                    groupBoxCreation(fillRos, noOfSymptomRows, symptomMaxHeight);
                    checkBoxAndOtherControlsCreation(fillRos, noOfSymptomRows, symptomMaxHeight);
                    chkAllOtherSystemsNormal.Attributes.Add("OnClick", "chkAllOtherSystemsNormalClick('" + systemNamesForEventHF.Value + "')");
                }
                else
                    btnSave.Enabled = isResetOK.Value == "false" ? true : false;

                if (Session["fillRos"] != null)
                    fillRos = (FillROS)Session["fillRos"];


                dlcROS.txtDLC.Attributes.Add("onkeyup", "btnSaveEnabled(false)");
                dlcROS.txtDLC.Attributes.Add("onchange", "btnSaveEnabled(false)");
                // dlcROS.pbDropdown.Attributes.Add("onclick", "enableOrDiasble()");
                //dlcROS.pbDropdown.Click += new ImageClickEventHandler(pbDropdown_Click);
                btnSave.Attributes.Add("onClick", "saveClientClick();");

                //btnPastEncounter.Attributes.Add("onClick", "btnSaveEnabled(false)");
                if (!IsPostBack)
                {
                    ClientSession.processCheck = true;
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);
                    btnSave.Enabled = false;
                    // For Floating Summary - Edited by Ponmozhi Vendan T #START
                    if (string.Compare(Convert.ToString(Session["Client_FromTab"]), "ROS", true) == 0)
                    {
                        Session["Client_FromTab"] = null;
                        btnSave.Enabled = Session["Client_IsFromEdit"] != null ? Convert.ToBoolean(Session["Client_IsFromEdit"]) : true;
                        Session["Client_IsFromEdit"] = null;
                    }
                    // For Floating Summary - Edited by Ponmozhi Vendan T #END
                }
            }
        }

        void pbDropdown_Click(object sender, ImageClickEventArgs e)
        {
            if (isbtnSaveOrDisable.Value == "true")
                btnSave.Enabled = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var isAlert = checkAndInsert();

            if (HdnCopyButton.Value == "trueValidate")
            {
                IList<int> noOfSymptomRows = new List<int>();
                IList<int> symptomMaxHeight = new List<int>();
                //pnlReviewOfSystems.Controls.Clear();
                //fillRosPastEncounter = (FillROS)Session["fillRos"];
                //groupBoxCreation(fillRosPastEncounter, noOfSymptomRows, symptomMaxHeight);
                //checkBoxAndOtherControlsCreation(fillRosPastEncounter, noOfSymptomRows, symptomMaxHeight);
                fillRosPastEncounter = objROSManager.GetRosAndGeneralNotesForPastEncounter(ClientSession.EncounterId, ClientSession.HumanId, ClientSession.PhysicianId);
                //Jira CAP-1923
                //Session["fillRos"] = fillRosPastEncounter;
                Session["fillPrevRos"] = fillRosPastEncounter;
                CopyPreviousForROS(ClientSession.HumanId, ClientSession.EncounterId, ClientSession.PhysicianId, isAlert);
            }
        }

        protected void btnCopyPrevHidden_Click(object sender, EventArgs e)
        {
            CopyPreviousForROS(ClientSession.HumanId, ClientSession.EncounterId, ClientSession.PhysicianId, false);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //if (isResetOK.Value == "true")
            //{
            //    isResetOK.Value = string.Empty;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "reLoadROS();", true);
            //}
        }

        protected void btnPastEncounter_Click(object sender, EventArgs e)
        {
            Is_copy_previous = true;
            CopyPreviousForROS(ClientSession.HumanId, ClientSession.EncounterId, ClientSession.PhysicianId, false);
            Is_copy_previous = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnFloatingSummary_Click(object sender, EventArgs e)
        {
            ulong _PhysicianID = 0;
            if (hdnCopyPreviousPhysicianId.Value != null && hdnCopyPreviousPhysicianId.Value.Trim() != string.Empty)
                _PhysicianID = Convert.ToUInt32(hdnCopyPreviousPhysicianId.Value);

            if (_PhysicianID != 0)
                CopyPreviousForROS(ClientSession.HumanId, ClientSession.EncounterId, _PhysicianID, false);

            RadWindow1.VisibleOnPageLoad = false;
        }

        #endregion

        #region Method

        private string arrayListToString(string[] arrayList)
        {
            string strFinal = string.Empty;
            for (int i = 0; i <= arrayList.Count() - 1; i++)
            {
                if (i > 0)
                    strFinal += "|";
                strFinal += arrayList[i].ToString();
            }
            return strFinal;
        }

        public void groupBoxCreation(FillROS fillRos, IList<int> noOfSymptomRows, IList<int> symptomMaxHeight)
        {
            UserLookupManager objUserLookupManager = new UserLookupManager();

            IList<GeneralNotes> generalNotesList = new List<GeneralNotes>();
            IList<ROS> rosList = new List<ROS>();
            IList<string> symptomCountList = new List<string>();
            IList<int> symptomHeight = new List<int>();
            IList<string> systemListString = new List<string>();
            IList<string> symptomListString = new List<string>();
            IList<FieldLookup> systemNames = new List<FieldLookup>();
            IList<Panel> groupBox = new List<Panel>();

            Table objPanelTable = new Table();

            symptomNamesLookUp = new List<FieldLookup>();



            int noOfSymptoms = 0;
            int currentSystemNumber = 0;

            if (fillRos.Ros_List != null && fillRos.Ros_List.Count > 0 && fillRos.General_Notes_List != null && fillRos.General_Notes_List.Count > 0)
            {
                rosList = fillRos.Ros_List;
                //generalNotesList = fillRos.General_Notes_List;
            }

            //if (rosList != null && rosList.Count == 0)
            //{
            IList<string> SystemNames = new List<string>();
            string sex = string.Empty;

            if (ClientSession.PatientPaneList != null)
            {
                if (ClientSession.PatientPaneList.Count > 0)
                {
                    //Cap - 1599
                    //sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                    if (ClientSession.PatientPaneList[0].Sex.ToUpper().Contains("MALE"))
                    {
                        sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                    }
                    else
                    {
                        sex = ClientSession.PatientPaneList[0].Sex.ToUpper();
                    }
                }
                else
                {
                    IList<string> ilsHumanTagList = new List<string>();
                    ilsHumanTagList.Add("HumanList");


                    IList<Human> lsthuman = new List<Human>();

                    // if (Is_copy_previous == true)
                    IList<object> humanBlobFinal = new List<object>();
                    humanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilsHumanTagList);

                    if (humanBlobFinal != null && humanBlobFinal.Count > 0)
                    {
                        if (humanBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)humanBlobFinal[0]).Count; iCount++)
                            {
                                lsthuman.Add((Human)((IList<object>)humanBlobFinal[0])[iCount]);
                            }

                        }

                    }
                    if (lsthuman.Count > 0)
                    {
                        //Cap - 1599
                        //sex = lsthuman[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                        if (lsthuman[0].Sex.ToUpper().Contains("MALE") == true)
                        {
                            sex = lsthuman[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                        }
                        else
                        {
                            sex = lsthuman[0].Sex.ToUpper();
                        }
                    }

                    //string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + ClientSession.HumanId.ToString() + ".xml");
                    //if (File.Exists(strXmlEncounterPath) == true)
                    //{
                    //    using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //    {
                    //        XmlDocument itemDoc = new XmlDocument();
                    //        XmlTextReader xmltxtReader = new XmlTextReader(fs);
                    //        itemDoc.Load(xmltxtReader);
                    //        xmltxtReader.Close();
                    //        XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("HumanList");
                    //        if (xmlNodeList.Count > 0)
                    //        {
                    //            if (xmlNodeList[0].ChildNodes.Count > 0)
                    //            {
                    //                sex = xmlNodeList[0].ChildNodes[0].Attributes["Sex"].Value.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                    //            }

                    //        }
                    //        fs.Close();
                    //        fs.Dispose();
                    //    }
                    //}

                }
            }
            symptomNamesLookUp = objUserLookupManager.GetFieldLookupListforPartialField(ClientSession.PhysicianId, "ROS SYMPTOM FOR", sex).ToArray();
            if (symptomNamesLookUp.Count > 0)
            {
                SystemNames = symptomNamesLookUp.OrderBy(a => a.Sort_Order).Select(a => a.Doc_Type).Distinct().ToList<string>();
                //For Bug Id : 64799 //To load default general notes
                IList<FieldLookup> symptomNamesLookUpwithDefaultnotes = symptomNamesLookUp.Where(aa => aa.Doc_Sub_Type != "").GroupBy(aa => aa.Doc_Type).Select(aa => aa.FirstOrDefault()).ToList<FieldLookup>();
                if (symptomNamesLookUpwithDefaultnotes != null && symptomNamesLookUpwithDefaultnotes.Count() > 0)
                {
                    Session["SymptomNameswithDefaultNotes"] = symptomNamesLookUpwithDefaultnotes;
                }
            }

            if (SystemNames.Count > 0)
                systemNamesForEventHF.Value = arrayListToString(SystemNames.ToArray());


            for (int i = 0; i < SystemNames.Count; i++)
            {
                noOfSymptoms = symptomNamesLookUp.Count(c => c.Field_Name == ("ROS SYMPTOM FOR " + SystemNames[i].ToUpper()));
                if (noOfSymptoms == 0)
                    continue;

                systemGroupBoxCreation(groupBox, SystemNames[i]);
                symptomCountList.Add(noOfSymptoms.ToString());
                currentSystemNumber++;

                int tempNoOfSymptoms = noOfSymptoms % 2 == 0 ? (int)(noOfSymptoms / 2) : (int)(noOfSymptoms / 2) + 1;
                noOfSymptomRows.Add(tempNoOfSymptoms);

                symptomListString = symptomNamesLookUp.Where(n => n.Field_Name == ("ROS SYMPTOM FOR " + SystemNames[i].ToUpper())).Select(s => s.Value).ToList();

                foreach (var item in symptomListString)
                {
                    string txt = item;
                    //Jira CAP-81
                    //Font f = new Font(FontFamily.GenericSansSerif, 8.2f, FontStyle.Regular);
                    Font f = new Font(FontFamily.GenericSansSerif, 8.2f);
                    Bitmap bp = new Bitmap(1, 1);
                    Graphics gp = Graphics.FromImage(bp);
                    int height = (int)gp.MeasureString(txt, f, symptomWidth).Height;
                    int temp = height >= 50 ? 2 : 0;
                    symptomHeight.Add(height + temp);
                }

                if (currentSystemNumber != 0 && currentSystemNumber % 3 == 0)
                {
                    if (symptomHeight.Count > 0)
                        symptomMaxHeight.Add(symptomHeight.Max());

                    objPanelTable.Controls.Add(objPanelTableRow);
                    objPanelTableRow = new TableRow();

                    groupBox.Clear();
                    symptomCountList.Clear();
                    currentSystemNumber = 0;
                }
            }
            if (currentSystemNumber < 3)
            {
                if (symptomHeight.Count > 0)
                    symptomMaxHeight.Add(symptomHeight.Max());

                symptomHeight.Clear();
                currentSystemNumber = 0;
                objPanelTable.Controls.Add(objPanelTableRow);
            }

            this.pnlReviewOfSystems.Controls.Add(objPanelTable);

            if (currentSystemNumber > 0)
                groupBox.Clear();

            /* systemNames = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, "ROSSYSTEM", sex, "Sort_Order").ToArray();
             systemNamesForEventHF.Value = arrayListToString(systemNames.Select(s => s.Value).Distinct().ToArray());

             if (systemNames != null)
             {
                 string[] systemNameListString = systemNames.Select(s => ("ROS SYMPTOM FOR " + s.Value.ToUpper())).ToArray();
                 symptomNamesLookUp = objUserLookupManager.GetFieldLookupListInfoForAll(string.Empty, ClientSession.PhysicianId, systemNameListString).ToArray();

                 for (int i = 0; i < systemNames.Count; i++)
                 {
                     noOfSymptoms = symptomNamesLookUp.Count(c => c.Field_Name == ("ROS SYMPTOM FOR " + systemNames[i].Value.ToUpper()));
                     if (noOfSymptoms == 0)
                         continue;

                     systemGroupBoxCreation(groupBox, systemNames[i].Value);
                     symptomCountList.Add(noOfSymptoms.ToString());
                     currentSystemNumber++;

                     int tempNoOfSymptoms = noOfSymptoms % 2 == 0 ? (int)(noOfSymptoms / 2) : (int)(noOfSymptoms / 2) + 1;
                     noOfSymptomRows.Add(tempNoOfSymptoms);

                     symptomListString = symptomNamesLookUp.Where(n => n.Field_Name == ("ROS SYMPTOM FOR " + systemNames[i].Value.ToUpper())).Select(s => s.Value).ToList();

                     foreach (var item in symptomListString)
                     {
                         string txt = item;
                         Font f = new Font(FontFamily.GenericSansSerif, 8.2f, FontStyle.Regular);
                         Bitmap bp = new Bitmap(1, 1);
                         Graphics gp = Graphics.FromImage(bp);
                         int height = (int)gp.MeasureString(txt, f, symptomWidth).Height;
                         int temp = height >= 50 ? 2 : 0;
                         symptomHeight.Add(height + temp);
                     }

                     if (currentSystemNumber != 0 && currentSystemNumber % 3 == 0)
                     {
                         if (symptomHeight.Count > 0)
                             symptomMaxHeight.Add(symptomHeight.Max());

                         objPanelTable.Controls.Add(objPanelTableRow);
                         objPanelTableRow = new TableRow();

                         groupBox.Clear();
                         symptomCountList.Clear();
                         currentSystemNumber = 0;
                     }
                 }

                 if (currentSystemNumber < 3)
                 {
                     if (symptomHeight.Count > 0)
                         symptomMaxHeight.Add(symptomHeight.Max());

                     symptomHeight.Clear();
                     currentSystemNumber = 0;
                     objPanelTable.Controls.Add(objPanelTableRow);
                 }

                 this.pnlReviewOfSystems.Controls.Add(objPanelTable);
                 //Session["RosTable"] = objPanelTable;
             }
             if (currentSystemNumber > 0)
                 groupBox.Clear();
             */
        }

        private void systemGroupBoxCreation(IList<Panel> groupBox, string systemName)
        {
            Panel gb = new Panel();
            gb.ID = "gb_" + systemName;
            gb.GroupingText = systemName;
            //gb.Font.Name = FontFamily.GenericSansSerif.ToString();
            //gb.Font.Size = FontUnit.Small;
            gb.CssClass = "spanstyle";
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(gb);
            objPanelTableCell.VerticalAlign = VerticalAlign.Top;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            groupBox.Add(gb);
        }

        public void checkBoxAndOtherControlsCreation(FillROS fillROS, IList<int> noOfSymptomRows, IList<int> symptomMaxHeight)
        {
            var tempNoOfSymptomRows = noOfSymptomRows.Select((val, index) => new { Index = index, Value = val }).GroupBy(x => x.Index / 3).Select(x => x.Select(v => v.Value).ToList()).ToList();
            //Check to load default value 
            //var chkAllSystemGenNotes = fillROS.ROS_GeneralNotes_List.Where(a => a.Notes.Trim() != "").ToList();
            //var chkGenNotes = fillROS.General_Notes_List.Where(a => a.Notes.Trim() != "").ToList();


            for (int i = 0; i < pnlReviewOfSystems.Controls.Count; i++)
            {
                if (pnlReviewOfSystems.Controls[i].GetType().ToString().Contains("Table") == true)
                {
                    int co = 0;

                    foreach (TableRow row in ((Table)pnlReviewOfSystems.Controls[i]).Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            if (fillROS.Ros_List.Count == 0 && fillROS.ROS_GeneralNotes_List.Count == 0 && fillROS.General_Notes_List.Count == 0)
                            {
                                //For Bug Id : 64799 //To load default Check box notes
                                DynamicCheckBoxCreation(fillROS, (Panel)cell.Controls[0], ((Panel)cell.Controls[0]).GroupingText, symptomNamesLookUp.Where(n => n.Field_Name == ("ROS SYMPTOM FOR " + ((Panel)cell.Controls[0]).GroupingText.ToUpper())).Select(s => s.Value + "|" + s.Default_Value).ToList(), tempNoOfSymptomRows[co].Max(), symptomMaxHeight.Count > 0 ? symptomMaxHeight[co] : 5);
                            }
                            else
                            {
                                DynamicCheckBoxCreation(fillROS, (Panel)cell.Controls[0], ((Panel)cell.Controls[0]).GroupingText, symptomNamesLookUp.Where(n => n.Field_Name == ("ROS SYMPTOM FOR " + ((Panel)cell.Controls[0]).GroupingText.ToUpper())).Select(s => s.Value + "|").ToList(), tempNoOfSymptomRows[co].Max(), symptomMaxHeight.Count > 0 ? symptomMaxHeight[co] : 5);
                            }
                        }
                        co++;
                    }
                }
            }
        }

        private void DynamicCheckBoxCreation(FillROS fillROS, Panel grp, string system, IList<string> sympNames, int maxSymptomRow, int symptomMaxHeight)
        {
            IList<string> symptomListString = new List<string>();
            IList<string> symptomStatusListString = new List<string>();
            IList<ROS> rosList = new List<ROS>();
            IList<string> symptomNames = new List<string>();

            rosList = fillROS.Ros_List;

            grp.Controls.Clear();

            //if (rosList != null && rosList.Count == 0)
            symptomNames = sympNames;
            IDictionary<string, string> lstSympStatus = new Dictionary<string, string>();
            if (rosList.Where(s => s.System_Name == grp.GroupingText) != null && rosList.Where(s => s.System_Name == grp.GroupingText).Count() != 0)
            {
                foreach (string strSymptom in symptomNames)
                {
                    string StatusValue = string.Empty;
                    IList<string> status = rosList.Where(s => s.Symptom_Name == strSymptom.Split('|')[0] && s.System_Name == grp.GroupingText).Select(s => s.Status).ToList();
                    if (status != null && status.Count > 0)
                        StatusValue = status[0].ToString();
                    lstSympStatus.Add(strSymptom.Split('|')[0], StatusValue.Trim());
                }
            }
            else
            {
                foreach (string strSymptom in symptomNames)
                {
                    // string StatusValue = string.Empty;
                    string StatusValue = strSymptom.Split('|')[1];
                    lstSympStatus.Add(strSymptom.Split('|')[0], StatusValue.Trim());
                }
            }

            Table objPanelTable = new Table();
            objPanelTable.ID = "table_" + grp.GroupingText;
            objPanelTableRow = new TableRow();

            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = symptomWidth;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblAll = new Label();
            lblAll.ID = "lblAll" + grp.GroupingText;
            lblAll.Text = "All";
            //  lblAll.Font.Name = FontFamily.GenericSansSerif.ToString();
            // lblAll.Font.Size = FontUnit.Small;
            lblAll.CssClass = "spanstyle";
            lblAll.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = 10;

            objPanelTableCell.Controls.Add(lblAll);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblOthers = new Label();
            lblOthers.ID = "lblOthers" + grp.GroupingText;
            lblOthers.Text = "Others";
            //  lblOthers.Font.Name = FontFamily.GenericSansSerif.ToString();
            // lblOthers.Font.Size = FontUnit.Small;
            lblOthers.CssClass = "spanstyle";
            lblOthers.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = 10;
            objPanelTableCell.Controls.Add(lblOthers);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = symptomWidth;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            //Label lblAll1 = new Label();
            //lblAll1.ID = "lblAll1" + grp.GroupingText;
            //lblAll1.Text = "All";
            //lblAll1.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lblAll1.Font.Size = FontUnit.Small;
            //lblAll1.EnableViewState = false;
            //objPanelTableCell = new TableCell();
            //objPanelTableCell.Width = 10;
            //objPanelTableCell.Controls.Add(lblAll1);
            //objPanelTableRow.Controls.Add(objPanelTableCell);

            //Label lblOthers1 = new Label();
            //lblOthers1.ID = "lblOthers1" + grp.GroupingText;
            //lblOthers1.Text = "Others";
            //lblOthers1.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lblOthers1.Font.Size = FontUnit.Small;
            //lblOthers1.EnableViewState = false;
            //objPanelTableCell = new TableCell();
            //objPanelTableCell.Width = 10;
            //objPanelTableCell.Controls.Add(lblOthers1);
            //objPanelTableRow.Controls.Add(objPanelTableCell);
            //objPanelTable.Controls.Add(objPanelTableRow);

            objPanelTableRow = new TableRow();
            objPanelTableCell = new TableCell();
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblYesAll1 = new Label();
            lblYesAll1.Text = "Y";
            lblYesAll1.ID = "lblYes_" + grp.GroupingText + "_1";
            // lblYesAll1.Font.Name = FontFamily.GenericSansSerif.ToString();
            // lblYesAll1.Font.Size = FontUnit.Small;
            lblYesAll1.CssClass = "spanstyle";
            lblYesAll1.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(lblYesAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblNoAll1 = new Label();
            lblNoAll1.Text = "N";
            lblNoAll1.ID = "lblNo_" + grp.GroupingText + "_1";
            lblYesAll1.CssClass = "spanstyle";
            lblNoAll1.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(lblNoAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblYesAll2 = new Label();
            lblYesAll2.Text = "Y";
            lblYesAll2.ID = "lblYes_" + grp.GroupingText + "_2";
            lblYesAll2.CssClass = "spanstyle";
            lblYesAll2.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(lblYesAll2);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblNoAll2 = new Label();
            lblNoAll2.Text = "N";
            lblNoAll2.ID = "lblNo_" + grp.GroupingText + "_2";
            lblNoAll2.CssClass = "spanstyle";
            lblNoAll2.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(lblNoAll2);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTable.Controls.Add(objPanelTableRow);

            objPanelTableRow = new TableRow();
            objPanelTableCell = new TableCell();
            objPanelTableRow.Controls.Add(objPanelTableCell);

            CheckBox chkYesAll1 = new CheckBox();
            chkYesAll1.ID = "chkYes_" + grp.GroupingText + "_All";
            chkYesAll1.Attributes.Add("OnClick", "checkStateWhenAllSymptomsCheckBoxChecked('" + chkYesAll1.ID + "')");
            chkYesAll1.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(chkYesAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            CheckBox chkNoAll1 = new CheckBox();
            chkNoAll1.ID = "chkNo_" + grp.GroupingText + "_All";
            chkNoAll1.Attributes.Add("OnClick", "checkStateWhenAllSymptomsCheckBoxChecked('" + chkNoAll1.ID + "')");
            chkNoAll1.EnableViewState = false;
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(chkNoAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTable.Controls.Add(objPanelTableRow);

            if (symptomNames != null && symptomNames.Count > 0)
                symptomLabelsAndCheckBox(fillROS.General_Notes_List, grp, objPanelTable, symptomNames, lstSympStatus, maxSymptomRow, symptomMaxHeight);

            grp.Controls.Add(objPanelTable);
        }

        private void symptomLabelsAndCheckBox(IList<GeneralNotes> generalNotesList, Panel grp, Table objPanelTable, IList<string> symptoms, IDictionary<string, string> symptomsStatuses, int maxSymptomRow, int symptomMaxHeight)
        {
            if (symptoms != null)
            {
                Label grwLbl;
                CheckBox chkYes;
                CheckBox chkNo;

                for (int i = 0; i < symptoms.Count; i++)
                {
                    string symptom = symptoms[i].Split('|')[0];
                    string symptomStatus = symptomsStatuses[symptom].ToString();
                    objPanelTableRow = new TableRow();

                    grwLbl = new Label();
                    grwLbl.Text = symptoms[i].Split('|')[0];
                    grwLbl.ID = "lblSymptom_" + grp.GroupingText + "_" + grwLbl.Text;
                    grwLbl.CssClass = "spanstyle";
                    grwLbl.Height = symptomMaxHeight;
                    grwLbl.EnableViewState = false;
                    grwLbl.Width = new Unit(120, UnitType.Pixel);//
                    objPanelTableCell = new TableCell();
                    objPanelTableCell.Controls.Add(grwLbl);
                    objPanelTableRow.Controls.Add(objPanelTableCell);

                    chkYes = new CheckBox();
                    chkYes.ID = "chkYes_" + grp.GroupingText + "_" + grwLbl.Text;
                    chkYes.Checked = symptomStatus.ToUpper() == "YES" ? true : false;
                    chkYes.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkYes.ID + "')");
                    chkYes.EnableViewState = false;
                    objPanelTableCell = new TableCell();
                    objPanelTableCell.Controls.Add(chkYes);
                    objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                    objPanelTableRow.Controls.Add(objPanelTableCell);

                    chkNo = new CheckBox();
                    chkNo.ID = "chkNo_" + grp.GroupingText + "_" + grwLbl.Text;
                    chkNo.Checked = symptomStatus.ToUpper() == "NO" ? true : false;
                    chkNo.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkNo.ID + "')");
                    chkNo.EnableViewState = false;
                    objPanelTableCell = new TableCell();
                    objPanelTableCell.Controls.Add(chkNo);
                    objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                    objPanelTableRow.Controls.Add(objPanelTableCell);

                    i++;

                    if (i < symptoms.Count)
                    {
                        symptom = symptoms[i].Split('|')[0];
                        symptomStatus = symptomsStatuses[symptom].ToString();

                        grwLbl = new Label();
                        grwLbl.Text = symptoms[i].Split('|')[0];
                        grwLbl.ID = "lblSymptom_" + grp.GroupingText + "_" + grwLbl.Text;
                        grwLbl.CssClass = "spanstyle";
                        grwLbl.Height = symptomMaxHeight;
                        grwLbl.EnableViewState = false;
                        grwLbl.Width = new Unit(120, UnitType.Pixel);//
                        objPanelTableCell = new TableCell();
                        objPanelTableCell.Controls.Add(grwLbl);
                        objPanelTableRow.Controls.Add(objPanelTableCell);

                        chkYes = new CheckBox();
                        chkYes.ID = "chkYes_" + grp.GroupingText + "_" + grwLbl.Text;
                        chkYes.Checked = symptomStatus.ToUpper() == "YES" ? true : false;
                        chkYes.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkYes.ID + "')");
                        chkYes.EnableViewState = false;
                        objPanelTableCell = new TableCell();
                        objPanelTableCell.Controls.Add(chkYes);
                        objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                        objPanelTableRow.Controls.Add(objPanelTableCell);

                        chkNo = new CheckBox();
                        chkNo.ID = "chkNo_" + grp.GroupingText + "_" + grwLbl.Text;
                        chkNo.Checked = symptomStatus.ToUpper() == "NO" ? true : false;
                        chkNo.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkNo.ID + "')");
                        chkNo.EnableViewState = false;
                        objPanelTableCell = new TableCell();
                        objPanelTableCell.Controls.Add(chkNo);
                        objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                        objPanelTableRow.Controls.Add(objPanelTableCell);
                    }

                    objPanelTable.Controls.Add(objPanelTableRow);
                }
            }

            int tempNoOfSymptoms = symptoms.Count % 2 == 0 ? (int)(symptoms.Count / 2) : (int)(symptoms.Count / 2) + 1;
            int length = maxSymptomRow - tempNoOfSymptoms;

            for (int i = 0; i < length; i++)
            {
                objPanelTableRow = new TableRow();
                Label lblSpace2 = new Label();
                lblSpace2.ID = "lblSpace2_" + i.ToString() + "_" + grp.GroupingText;
                lblSpace2.Height = symptomMaxHeight;
                lblSpace2.EnableViewState = false;
                objPanelTableCell = new TableCell();
                objPanelTableCell.Controls.Add(lblSpace2);
                objPanelTableCell.ColumnSpan = 6;
                objPanelTableRow.Controls.Add(objPanelTableCell);
                objPanelTable.Controls.Add(objPanelTableRow);
            }

            GroupBoxOtherControlsCreation(generalNotesList, grp, objPanelTable);
        }

        private void GroupBoxOtherControlsCreation(IList<GeneralNotes> generalNotesList, Panel grp, Table objPanelTable)
        {
            objPanelTableRow = new TableRow();

            Label lbl = new Label();
            lbl.ID = "lbl_" + grp.GroupingText;
            lbl.Text = "Notes";
            lbl.BackColor = Color.White;
            lbl.CssClass = "spanstyle";
            lbl.EnableViewState = false;

            objPanelTableRow = new TableRow();
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(lbl);
            objPanelTableRow.Controls.Add(objPanelTableCell);
            objPanelTable.Controls.Add(objPanelTableRow);

            objPanelTableRow = new TableRow();



            CustomDLCNew dlc = (CustomDLCNew)LoadControl("~/UserControls/CustomDLCNew.ascx");
            dlc.ID = "dlc_" + grp.GroupingText.Replace(" ", "");
            dlc.TextboxHeight = new Unit("40px");
            dlc.TextboxWidth = new Unit("230px");
            dlc.ListboxHeight = new Unit("100px");
            dlc.Value = "NOTES FOR " + grp.GroupingText.ToUpper();
            //dlc.GetTheStatusFromClientSession(this.Page.AppRelativeVirtualPath);
            //dlc.SetTheUBACForDynamicControls();

            if (generalNotesList != null && generalNotesList.Count > 0)
            {
                var gnrlNotes = (from notes in generalNotesList
                                 where notes.Parent_Field.ToUpper() == "SYSTEM" && notes.Name_Of_The_Field.ToUpper() == grp.GroupingText.ToUpper()
                                 select notes);

                if (gnrlNotes != null && gnrlNotes.Count() != 0)
                {
                    foreach (var gnr in gnrlNotes)
                        dlc.txtDLC.Text = gnr.Notes;
                }
            }
            else //For Bug Id : 64799 //To load default general notes
            {
                if (Session["SymptomNameswithDefaultNotes"] != null)
                {
                    IList<FieldLookup> lstFiledLU = (IList<FieldLookup>)Session["SymptomNameswithDefaultNotes"];
                    if (lstFiledLU.Count() > 0 && lstFiledLU.Where(a => a.Doc_Type == grp.GroupingText).ToList().Count() > 0)
                    {
                        dlc.txtDLC.Text = lstFiledLU.Where(a => a.Doc_Type == grp.GroupingText).ToList()[0].Doc_Sub_Type;
                    }
                }
            }

            dlc.txtDLC.Attributes.Add("onkeyup", "btnSaveEnabled(false)");
            dlc.txtDLC.Attributes.Add("onchange", "btnSaveEnabled(false)");
            //dlc.pbDropdown.Attributes.Add("onclick", "enableOrDiasble()");
            //dlc.pbDropdown.Click += new ImageClickEventHandler(pbDropdown_Click);

            objPanelTableCell = new TableCell();
            Panel objPanel = new Panel();
            objPanel.Style.Add(HtmlTextWriterStyle.Width, "100%");
            objPanel.Style.Add(HtmlTextWriterStyle.Height, "100%");
            objPanel.Style.Add(HtmlTextWriterStyle.FontSize, "Small");
            objPanelTableCell.Controls.Add(objPanel);
            objPanel.Controls.Add(dlc);
            objPanelTableCell.ColumnSpan = 6;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTable.Controls.Add(objPanelTableRow);


        }

        private void loadNotes(string tempSys)
        {
            UserLookupManager objUserLookupManager = new UserLookupManager();
            IList<FieldLookup> fieldlist = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, tempSys).ToArray();

            if (fieldlist != null)
            {
                foreach (var item in fieldlist)
                {
                    RadListBoxItem tempItem = new RadListBoxItem();
                    tempItem.Text = item.Value;
                    tempItem.ToolTip = "Click to add Notes";
                }
            }
        }

        //TODO - Clean up?
        private bool checkAndInsert()
        {
            var isAvailable = false;
            UserLookupManager objUserLookupManager = new UserLookupManager();
            CheckBox chkYesSymptom = new CheckBox();
            CheckBox chkNoSymptom = new CheckBox();
            Panel gr = new Panel();
            CheckBox chkSystemAndSymptom = new CheckBox();
            Panel grSystemAndSymptom = new Panel();
            IList<ROS> rosListToInsert = new List<ROS>();
            IList<ROS> rosListToUpdate = new List<ROS>();
            IList<ROS> rosListToDelete = new List<ROS>();
            IList<GeneralNotes> generalNotesToInsert = new List<GeneralNotes>();
            IList<GeneralNotes> generalNotesToUpdate = new List<GeneralNotes>();
            IList<FieldLookup> systemNames = new List<FieldLookup>();
            GeneralNotes gnrlNotesObj = null;
            IList<string> strListCheckBoxSymptomName = new List<string>();
            IList<string> strListUnchecked = new List<string>();
            IList<string> strListChecked = new List<string>();
            int symptomGrowLabelCount = 0;
            ROS rosObjSystemAndSymptom = null;
            Label grwSymptom = new Label();
            IList<ROS> rosListOfSystemAndSymptom = new List<ROS>();
            bool insertFlag = false;
            bool updateFlag = false;
            string strStatus = string.Empty;

            FillROS fillRos = new FillROS();
            IList<ROS> rosList = new List<ROS>();

            if (Session["fillRos"] != null)
            {
                fillRos = (FillROS)Session["fillRos"];
                if (fillRos.Ros_List != null && fillRos.Ros_List.Count > 0)
                    rosList = fillRos.Ros_List;
            }
            // IList<ROS> rosList =fillRos.Ros_List;//For Bug ID : 74659

            ROSManager objROSManager = new ROSManager();
            ClientSession.FlushSession();
            IList<string> ilstROSTagList = new List<string>();
            ilstROSTagList.Add("ROSList");
            ilstROSTagList.Add("GeneralNotesROSList");
            ilstROSTagList.Add("GeneralNotesROSGeneralNotesList");

                                          
            IList<ROS> ilst = new List<ROS>();
            IList<GeneralNotes> ilstGen = new List<GeneralNotes>(); ;
            IList<GeneralNotes> ilstROSGen = new List<GeneralNotes>(); ;
            // if (Is_copy_previous == true)
            IList<object> ilstROSBlobFinal = new List<object>();
            ilstROSBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstROSTagList);

            if (ilstROSBlobFinal != null && ilstROSBlobFinal.Count > 0)
            {
                if (ilstROSBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstROSBlobFinal[0]).Count; iCount++)
                    {
                        ilst.Add((ROS)((IList<object>)ilstROSBlobFinal[0])[iCount]);
                    }
                  
                }

                if (ilstROSBlobFinal[1] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstROSBlobFinal[1]).Count; iCount++)
                    {
                        ilstGen.Add((GeneralNotes)((IList<object>)ilstROSBlobFinal[1])[iCount]);
                    }
                   
                }

                if (ilstROSBlobFinal[2] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstROSBlobFinal[2]).Count; iCount++)
                    {
                        ilstROSGen.Add((GeneralNotes)((IList<object>)ilstROSBlobFinal[2])[iCount]);
                    }
                   
                }
            }

            //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            {
                //try
                //{
                //    if (File.Exists(strXmlFilePath) == true)
                //    {
                //        XmlDocument itemDoc = new XmlDocument();
                //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //        XmlNodeList xmlTagName = null;
                //        // itemDoc.Load(XmlText);
                //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //        {
                //            itemDoc.Load(fs);

                //            XmlText.Close();
                //            if (itemDoc.GetElementsByTagName("ROSList")[0] != null)
                //            {
                //                xmlTagName = itemDoc.GetElementsByTagName("ROSList")[0].ChildNodes;
                //                ilst = new List<ROS>();
                //                if (xmlTagName.Count > 0)
                //                {
                //                    for (int ji = 0; ji < xmlTagName.Count; ji++)
                //                    {
                //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(ROS));
                //                        ROS objros = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[ji])) as ROS;

                //                        IEnumerable<PropertyInfo> propInfo = null;
                //                        propInfo = from obji in ((ROS)objros).GetType().GetProperties() select obji;

                //                        for (int i = 0; i < xmlTagName[ji].Attributes.Count; i++)
                //                        {
                //                            XmlNode nodevalue = xmlTagName[ji].Attributes[i];
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name == nodevalue.Name)
                //                                    {
                //                                        {
                //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                                property.SetValue(objros, Convert.ToUInt64(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                property.SetValue(objros, Convert.ToString(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                property.SetValue(objros, Convert.ToDateTime(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                property.SetValue(objros, Convert.ToInt32(nodevalue.Value), null);
                //                                            else
                //                                                property.SetValue(objros, nodevalue.Value, null);
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }

                //                        ilst.Add(objros);
                //                        //fillRos.Ros_List = ilst;

                //                    }
                //                }
                //                if (itemDoc.GetElementsByTagName("GeneralNotesROSList")[0] != null)
                //                {
                //                    xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesROSList")[0].ChildNodes;
                //                    ilstGen = new List<GeneralNotes>();
                //                    if (xmlTagName.Count > 0)
                //                    {
                //                        for (int jk = 0; jk < xmlTagName.Count; jk++)
                //                        {
                //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                //                            GeneralNotes generalnotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[jk])) as GeneralNotes;
                //                            IEnumerable<PropertyInfo> propInfo = null;

                //                            propInfo = from obji in ((GeneralNotes)generalnotes).GetType().GetProperties() select obji;

                //                            for (int i = 0; i < xmlTagName[jk].Attributes.Count; i++)
                //                            {
                //                                XmlNode nodevalue = xmlTagName[jk].Attributes[i];
                //                                {
                //                                    foreach (PropertyInfo property in propInfo)
                //                                    {
                //                                        if (property.Name == nodevalue.Name)
                //                                        {
                //                                            {
                //                                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                                    property.SetValue(generalnotes, Convert.ToUInt64(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                    property.SetValue(generalnotes, Convert.ToString(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                    property.SetValue(generalnotes, Convert.ToDateTime(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                    property.SetValue(generalnotes, Convert.ToInt32(nodevalue.Value), null);
                //                                                else
                //                                                    property.SetValue(generalnotes, nodevalue.Value, null);
                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                            ilstGen.Add(generalnotes);
                //                        }
                //                    }
                //                }
                //                if (itemDoc.GetElementsByTagName("GeneralNotesROSGeneralNotesList")[0] != null)
                //                {
                //                    xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesROSGeneralNotesList")[0].ChildNodes;

                //                    if (xmlTagName.Count > 0)
                //                    {
                //                        ilstROSGen = new List<GeneralNotes>();
                //                        for (int jl = 0; jl < xmlTagName.Count; jl++)
                //                        {
                //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                //                            GeneralNotes generalnotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[jl])) as GeneralNotes;
                //                            IEnumerable<PropertyInfo> propInfo = null;

                //                            propInfo = from obji in ((GeneralNotes)generalnotes).GetType().GetProperties() select obji;

                //                            for (int i = 0; i < xmlTagName[jl].Attributes.Count; i++)
                //                            {
                //                                XmlNode nodevalue = xmlTagName[jl].Attributes[i];
                //                                {
                //                                    foreach (PropertyInfo property in propInfo)
                //                                    {
                //                                        if (property.Name == nodevalue.Name)
                //                                        {
                //                                            {
                //                                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                                    property.SetValue(generalnotes, Convert.ToUInt64(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                    property.SetValue(generalnotes, Convert.ToString(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                    property.SetValue(generalnotes, Convert.ToDateTime(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                    property.SetValue(generalnotes, Convert.ToInt32(nodevalue.Value), null);
                //                                                else
                //                                                    property.SetValue(generalnotes, nodevalue.Value, null);
                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                            }

                //                            ilstROSGen.Add(generalnotes);
                //                        }
                //                    }
                //                }
                //            }
                //            fs.Close();
                //            fs.Dispose();
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message + " - " + strXmlFilePath);
                //}

            }

            IList<string> SystemNames = new List<string>();
            string sex = string.Empty;

            if (ClientSession.PatientPaneList != null)
            {
                if (ClientSession.PatientPaneList.Count > 0)
                {
                    //Cap - 1599
                    //sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                    if (ClientSession.PatientPaneList[0].Sex.ToUpper().Contains("MALE"))
                    {
                        sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                    }
                    else
                    {
                        sex = ClientSession.PatientPaneList[0].Sex.ToUpper();
                    }
                }
                else
                {
                    IList<string> ilsHumanTagList = new List<string>();
                    ilsHumanTagList.Add("HumanList");


                    IList<Human> lsthuman = new List<Human>();
                   
                    // if (Is_copy_previous == true)
                    IList<object> humanBlobFinal = new List<object>();
                    humanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilsHumanTagList);

                    if (humanBlobFinal != null && humanBlobFinal.Count > 0)
                    {
                        if (humanBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)humanBlobFinal[0]).Count; iCount++)
                            {
                                lsthuman.Add((Human)((IList<object>)humanBlobFinal[0])[iCount]);
                            }

                        }
                        
                    }
                    if(lsthuman.Count>0)
                    {
                        //Cap - 1599
                        //sex = lsthuman[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                        if (lsthuman[0].Sex.ToUpper().Contains("MALE") == true)
                        {
                            sex = lsthuman[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                        }
                        else
                        {
                            sex = lsthuman[0].Sex.ToUpper();
                        }
                    }

                    //string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + ClientSession.HumanId.ToString() + ".xml");
                    //try
                    //{
                    //    if (File.Exists(strXmlEncounterPath) == true)
                    //    {
                    //        using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //        {
                    //            XmlDocument itemDoc = new XmlDocument();
                    //            XmlTextReader xmltxtReader = new XmlTextReader(fs);
                    //            itemDoc.Load(xmltxtReader);
                    //            xmltxtReader.Close();
                    //            XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("HumanList");
                    //            if (xmlNodeList.Count > 0)
                    //            {
                    //                if (xmlNodeList[0].ChildNodes.Count > 0)
                    //                {
                    //                    sex = xmlNodeList[0].ChildNodes[0].Attributes["Sex"].Value.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                    //                }

                    //            }
                    //            fs.Close();
                    //            fs.Dispose();
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception(ex.Message + " - " + strXmlEncounterPath);
                    //}

                }
            }
            symptomNamesLookUp = objUserLookupManager.GetFieldLookupListforPartialField(ClientSession.PhysicianId, "ROS SYMPTOM FOR", sex).ToArray();
            SystemNames = symptomNamesLookUp.OrderBy(a => a.Sort_Order).Select(a => a.Doc_Type).Distinct().ToList<string>();


            //string sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
            //systemNames = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, "ROSSYSTEM", sex, "Sort_Order").ToArray();
            //string[] systemNameListString = systemNames.Select(s => ("ROS SYMPTOM FOR " + s.Value.ToUpper())).ToArray();
            //symptomNamesLookUp = objUserLookupManager.GetFieldLookupListInfoForAll(string.Empty, ClientSession.PhysicianId, systemNameListString).ToArray();
            for (int i = 0; i < SystemNames.Count; i++)
            {
                grSystemAndSymptom = (Panel)pnlReviewOfSystems.FindControl("gb_" + SystemNames[i]);
                if (grSystemAndSymptom == null)
                {
                    continue;
                }
                chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_All");
                if (chkSystemAndSymptom.Checked == false)
                    strListUnchecked.Add(chkSystemAndSymptom.ID);
                else
                    strListChecked.Add(chkSystemAndSymptom.ID);

                chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_All");
                if (chkSystemAndSymptom.Checked == false)
                    strListUnchecked.Add(chkSystemAndSymptom.ID);
                else
                    strListChecked.Add(chkSystemAndSymptom.ID);

                for (int j = 0; j < symptomNamesLookUp.Count(c => c.Field_Name == ("ROS SYMPTOM FOR " + SystemNames[i].ToUpper())); j++)
                {
                    IList<string> symptomNamesLookUpTemp = symptomNamesLookUp.Where(c => c.Field_Name == ("ROS SYMPTOM FOR " + SystemNames[i].ToUpper())).Select(s => s.Value).ToList();

                    grwSymptom = (Label)pnlReviewOfSystems.FindControl("lblSymptom_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUpTemp[j]);
                    symptomGrowLabelCount++;
                    rosObjSystemAndSymptom = new ROS();
                    rosObjSystemAndSymptom.System_Name = grSystemAndSymptom.GroupingText;
                    rosObjSystemAndSymptom.Symptom_Name = grwSymptom.Text;
                    rosListOfSystemAndSymptom.Add(rosObjSystemAndSymptom);

                    chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUpTemp[j]);
                    if (chkSystemAndSymptom.Checked == false)
                        strListUnchecked.Add(chkSystemAndSymptom.ID);
                    else
                        strListChecked.Add(chkSystemAndSymptom.ID);

                    chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUpTemp[j]);
                    if (chkSystemAndSymptom.Checked == false)
                        strListUnchecked.Add(chkSystemAndSymptom.ID);
                    else
                        strListChecked.Add(chkSystemAndSymptom.ID);
                }
            }

            if (symptomGrowLabelCount == rosListOfSystemAndSymptom.Count)
            {
                for (int j = 0; j < rosListOfSystemAndSymptom.Count; j++)
                {
                    strStatus = string.Empty;
                    Control grpctrl = pnlReviewOfSystems.FindControl("gb_" + rosListOfSystemAndSymptom[j].System_Name);
                    gr = (Panel)grpctrl;
                    Control chkControlYes = gr.FindControl("chkYes_" + rosListOfSystemAndSymptom[j].System_Name + "_" + rosListOfSystemAndSymptom[j].Symptom_Name);
                    chkYesSymptom = (CheckBox)chkControlYes;
                    string[] strCheckStatusYes = chkYesSymptom.ID.Split('_');

                    Control chkControlNo = gr.FindControl("chkNo_" + rosListOfSystemAndSymptom[j].System_Name + "_" + rosListOfSystemAndSymptom[j].Symptom_Name);
                    chkNoSymptom = (CheckBox)chkControlNo;
                    string[] strCheckStatusNo = chkNoSymptom.ID.Split('_');

                    if (chkYesSymptom.Checked == true || chkNoSymptom.Checked == true)
                    {
                        if (chkNoSymptom.Checked == true)
                        {
                            strStatus = "No";
                        }
                        else
                        {
                            strStatus = "Yes";
                        }
                        if (rosList != null && rosList.Count > 0)
                        {
                            IList<string> symptomName = rosList.Where(a => a.Symptom_Name == rosListOfSystemAndSymptom[j].Symptom_Name && a.System_Name == rosListOfSystemAndSymptom[j].System_Name).Select(a => a.Symptom_Name).ToList<string>();

                            if (symptomName != null && symptomName.Count > 0)
                            {
                                insertFlag = false;
                                updateFlag = true;
                            }
                            else
                            {
                                insertFlag = true;
                                updateFlag = false;
                            }
                        }
                        else
                        {
                            insertFlag = true;
                            updateFlag = false;
                        }
                        if (insertFlag == true)
                        {
                            ROS rosobj = new ROS();
                            rosobj.Encounter_Id = ClientSession.EncounterId;
                            rosobj.Human_ID = ClientSession.HumanId;
                            rosobj.Physician_Id = ClientSession.PhysicianId;
                            rosobj.System_Name = rosListOfSystemAndSymptom[j].System_Name;
                            rosobj.Symptom_Name = rosListOfSystemAndSymptom[j].Symptom_Name;
                            rosobj.Status = strStatus;
                            rosobj.Created_By = ClientSession.UserName;
                            rosobj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            rosListToInsert.Add(rosobj);
                        }
                        else if (updateFlag == true)
                        {
                            ROS rosToUpdate = (from rosChanged in rosList
                                               where rosChanged.System_Name == rosListOfSystemAndSymptom[j].System_Name &&
                                               rosChanged.Symptom_Name == rosListOfSystemAndSymptom[j].Symptom_Name
                                               select rosChanged).ToList()[0];
                            rosToUpdate.Status = strStatus;
                            /**/
                            if (rosToUpdate.Encounter_Id != ClientSession.EncounterId)
                            {
                                /**/
                                //Check whether current encounter have ROS data
                                if (ilst != null && ilst.Count > 0)
                                {
                                    //Jira CAP-1923
                                    //if (j >= ilst.Count)
                                    //{
                                    //Jira CAP-1923
                                    //ROS rosToversionUpdate = (from rosChanged in ilst
                                    //                          where rosChanged.System_Name == rosListOfSystemAndSymptom[j].System_Name &&
                                    //                          rosChanged.Symptom_Name == rosListOfSystemAndSymptom[j].Symptom_Name
                                    //                          select rosChanged).ToList()[0];
                                    IList<ROS> rosToversionUpdate = (from rosChanged in ilst
                                                                     where rosChanged.System_Name == rosListOfSystemAndSymptom[j].System_Name &&
                                                                     rosChanged.Symptom_Name == rosListOfSystemAndSymptom[j].Symptom_Name
                                                                     select rosChanged).ToList();
                                    //Jira CAP-1923
                                    if (rosToversionUpdate.Count > 0)
                                    {
                                        //rosToUpdate = rosToversionUpdate;
                                        rosToUpdate.Modified_By = ClientSession.UserName;
                                        rosToUpdate.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        rosToUpdate.Version = rosToversionUpdate[0].Version;
                                        rosToUpdate.Encounter_Id = ClientSession.EncounterId;
                                        rosToUpdate.Created_By = rosToversionUpdate[0].Created_By;
                                        rosToUpdate.Created_Date_And_Time = rosToversionUpdate[0].Created_Date_And_Time;
                                        rosToUpdate.Modified_By = ClientSession.UserName;
                                        rosToUpdate.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        rosToUpdate.Id = rosToversionUpdate[0].Id;
                                        rosListToUpdate.Add(rosToUpdate);
                                    }
                                    else
                                    {
                                        ROS rosobj = new ROS();
                                        rosobj.Encounter_Id = ClientSession.EncounterId;
                                        rosobj.Human_ID = ClientSession.HumanId;
                                        rosobj.Physician_Id = ClientSession.PhysicianId;
                                        rosobj.System_Name = rosListOfSystemAndSymptom[j].System_Name;
                                        rosobj.Symptom_Name = rosListOfSystemAndSymptom[j].Symptom_Name;
                                        rosobj.Status = strStatus;
                                        rosobj.Created_By = ClientSession.UserName;
                                        rosobj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        rosListToInsert.Add(rosobj);
                                    }
                                    //Jira CAP-1923
                                    //}
                                    //else
                                    //{
                                    //    ROS rosobj = new ROS();
                                    //    rosobj.Encounter_Id = ClientSession.EncounterId;
                                    //    rosobj.Human_ID = ClientSession.HumanId;
                                    //    rosobj.Physician_Id = ClientSession.PhysicianId;
                                    //    rosobj.System_Name = rosListOfSystemAndSymptom[j].System_Name;
                                    //    rosobj.Symptom_Name = rosListOfSystemAndSymptom[j].Symptom_Name;
                                    //    rosobj.Status = strStatus;
                                    //    rosobj.Created_By = ClientSession.UserName;
                                    //    rosobj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    //    rosListToInsert.Add(rosobj);
                                    //}


                                }
                                else
                                {
                                    rosToUpdate.Version = 0;
                                    rosToUpdate.Id = 0;
                                    rosToUpdate.Encounter_Id = ClientSession.EncounterId;
                                    rosListToInsert.Add(rosToUpdate);
                                }
                            }
                            else
                            {
                                rosToUpdate.Modified_By = ClientSession.UserName;
                                rosToUpdate.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                rosListToUpdate.Add(rosToUpdate);
                            }
                        }
                    }
                    else
                    {
                        if (rosList != null && rosList.Count > 0)
                        {
                            IList<string> symptomName = rosList.Where(a => a.Symptom_Name == rosListOfSystemAndSymptom[j].Symptom_Name && a.System_Name == rosListOfSystemAndSymptom[j].System_Name).Select(a => a.Symptom_Name).ToList<string>();

                            if (symptomName != null && symptomName.Count > 0)
                            {
                                ROS rosToDelete = (from rosChanged in rosList
                                                   where rosChanged.System_Name == rosListOfSystemAndSymptom[j].System_Name &&
                                                   rosChanged.Symptom_Name == rosListOfSystemAndSymptom[j].Symptom_Name
                                                   select rosChanged).ToList()[0];
                                if (rosToDelete.Encounter_Id == ClientSession.EncounterId)
                                    rosListToDelete.Add(rosToDelete);
                            }
                        }
                    }
                }
                var text = rosListOfSystemAndSymptom.Select(s => s.System_Name).Distinct().ToList();

                foreach (var text2 in text)
                {
                    Control grpControl = pnlReviewOfSystems.FindControl("gb_" + text2);
                    gr = (Panel)grpControl;

                    CustomDLCNew txt = (CustomDLCNew)gr.FindControl("dlc_" + text2.Replace(" ", ""));

                    var gn = fillRos.General_Notes_List.Where(n => n.Name_Of_The_Field == text2).ToList();
                    if (gn != null && gn.Count() != 0)
                    {
                        foreach (var g in gn)
                        {
                            if (g.Encounter_ID != ClientSession.EncounterId)
                            {
                                if (ilstGen != null && ilstGen.Count > 0)
                                {
                                    GeneralNotes Gennotes = (from Notes in ilstGen
                                                             where Notes.Name_Of_The_Field == text2
                                                             select Notes).ToList()[0];
                                    gnrlNotesObj = Gennotes;
                                    gnrlNotesObj.Human_ID = ClientSession.HumanId;
                                    gnrlNotesObj.Encounter_ID = ClientSession.EncounterId;
                                    gnrlNotesObj.Notes = txt.txtDLC.Text;
                                    gnrlNotesObj.Modified_By = ClientSession.UserName;
                                    gnrlNotesObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    gnrlNotesObj.Id = Gennotes.Id;
                                    generalNotesToUpdate.Add(gnrlNotesObj);
                                }
                                else
                                {
                                    gnrlNotesObj = new GeneralNotes();
                                    gnrlNotesObj.Encounter_ID = ClientSession.EncounterId;
                                    gnrlNotesObj.Human_ID = ClientSession.HumanId;
                                    gnrlNotesObj.Name_Of_The_Field = text2;
                                    gnrlNotesObj.Parent_Field = "System";
                                    gnrlNotesObj.Notes = txt.txtDLC.Text;
                                    gnrlNotesObj.Created_By = ClientSession.UserName;
                                    gnrlNotesObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    generalNotesToInsert.Add(gnrlNotesObj);

                                }
                            }
                            else
                            {
                                //Added for Bug ID : 47476 
                                gnrlNotesObj = g;
                                gnrlNotesObj.Human_ID = ClientSession.HumanId;
                                gnrlNotesObj.Encounter_ID = ClientSession.EncounterId;
                                gnrlNotesObj.Notes = txt.txtDLC.Text;
                                gnrlNotesObj.Modified_By = ClientSession.UserName;
                                gnrlNotesObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                gnrlNotesObj.Id = g.Id;
                                generalNotesToUpdate.Add(gnrlNotesObj);
                            }
                        }
                    }
                    else if (gn != null && gn.Count() == 0)
                    {
                        gnrlNotesObj = new GeneralNotes();
                        gnrlNotesObj.Encounter_ID = ClientSession.EncounterId;
                        gnrlNotesObj.Human_ID = ClientSession.HumanId;
                        gnrlNotesObj.Name_Of_The_Field = text2;
                        gnrlNotesObj.Parent_Field = "System";
                        gnrlNotesObj.Notes = txt.txtDLC.Text;
                        gnrlNotesObj.Created_By = ClientSession.UserName;
                        gnrlNotesObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        generalNotesToInsert.Add(gnrlNotesObj);
                    }
                }

                GeneralNotes rosGeneralNotes = null;

                if (fillRos.General_Notes_List.Count == 0)
                {
                    rosGeneralNotes = new GeneralNotes();
                    rosGeneralNotes.Encounter_ID = ClientSession.EncounterId;
                    rosGeneralNotes.Human_ID = ClientSession.HumanId;
                    rosGeneralNotes.Parent_Field = "ROS GENERAL NOTES";
                    rosGeneralNotes.Notes = dlcROS.txtDLC.Text;
                    rosGeneralNotes.Created_By = ClientSession.UserName;
                    rosGeneralNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                }
                else
                {
                    if (ilstROSGen != null && ilstROSGen.Count > 0)
                    {
                        var rosGenNotes = ilstROSGen.Where(r => r.Encounter_ID == ClientSession.EncounterId && r.Parent_Field.ToUpper() == "ROS GENERAL NOTES").ToList();
                        if (rosGenNotes.Count > 0)
                        {
                            rosGeneralNotes = new GeneralNotes();
                            rosGeneralNotes = rosGenNotes[0];
                            rosGeneralNotes.Notes = dlcROS.txtDLC.Text;
                            rosGeneralNotes.Modified_By = ClientSession.UserName;
                            rosGeneralNotes.Version = rosGenNotes[0].Version;
                            rosGeneralNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            rosGeneralNotes.Version = rosGenNotes[0].Version;
                            rosGeneralNotes.Created_By = rosGenNotes[0].Created_By;
                            rosGeneralNotes.Created_Date_And_Time = rosGenNotes[0].Created_Date_And_Time;
                        }
                    }
                    else
                    {
                        var rosGenNotes = fillRos.ROS_GeneralNotes_List.Where(r => r.Encounter_ID == ClientSession.EncounterId && r.Parent_Field.ToUpper() == "ROS GENERAL NOTES").ToList();
                        if (rosGenNotes.Count > 0)
                        {
                            rosGeneralNotes = new GeneralNotes();
                            rosGeneralNotes = rosGenNotes[0];
                            rosGeneralNotes.Notes = dlcROS.txtDLC.Text;
                            rosGeneralNotes.Modified_By = ClientSession.UserName;
                            rosGeneralNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        }
                        else//Copy Previous ROS
                        {
                            rosGeneralNotes = new GeneralNotes();
                            rosGeneralNotes.Encounter_ID = ClientSession.EncounterId;
                            rosGeneralNotes.Human_ID = ClientSession.HumanId;
                            rosGeneralNotes.Parent_Field = "ROS GENERAL NOTES";
                            rosGeneralNotes.Notes = dlcROS.txtDLC.Text;
                            rosGeneralNotes.Created_By = ClientSession.UserName;
                            rosGeneralNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                        }
                    }
                }

                 objROSManager = new ROSManager();
                List<string> ilstSystemName = new List<string>();
                ulong encounterid = 0;
                ulong humanid = 0;

                if (rosListToInsert != null && rosListToInsert.Count > 0)
                {
                    encounterid = rosListToInsert[0].Encounter_Id;
                    humanid = rosListToInsert[0].Human_ID;
                    for (int i = 0; i < rosListToInsert.Count; i++)
                    {
                        if (rosListToInsert[i].Status.TrimEnd(' ') != "")
                        {
                            ilstSystemName.Add(rosListToInsert[i].System_Name);
                        }
                    }
                    for (int i = 0; i < generalNotesToInsert.Count; i++)
                    {
                        if (generalNotesToInsert[i].Notes.TrimEnd(' ') != "")
                        {
                            ilstSystemName.Add(generalNotesToInsert[i].Name_Of_The_Field);
                        }
                    }
                }
                if (rosListToUpdate != null && rosListToUpdate.Count > 0)
                {
                    encounterid = rosListToUpdate[0].Encounter_Id;
                    humanid = rosListToUpdate[0].Human_ID;
                    for (int i = 0; i < rosListToUpdate.Count; i++)
                    {
                        if (rosListToUpdate[i].Status.TrimEnd(' ') != "")
                        {
                            ilstSystemName.Add(rosListToUpdate[i].System_Name);
                        }
                    }
                    for (int i = 0; i < generalNotesToUpdate.Count; i++)
                    {
                        if (generalNotesToUpdate[i].Notes.TrimEnd(' ') != "")
                        {
                            ilstSystemName.Add(generalNotesToUpdate[i].Name_Of_The_Field);
                        }
                    }
                }

                if (rosListToInsert != null && rosListToInsert.Count == 0 && rosListToUpdate != null && rosListToUpdate.Count == 0)
                {
                    if (generalNotesToInsert.Count > 0)
                    {
                        encounterid = generalNotesToInsert[0].Encounter_ID;
                        humanid = generalNotesToInsert[0].Human_ID;
                    }
                    else if (generalNotesToUpdate.Count > 0)
                    {
                        encounterid = generalNotesToUpdate[0].Encounter_ID;
                        humanid = generalNotesToUpdate[0].Human_ID;
                    }
                    for (int i = 0; i < generalNotesToInsert.Count; i++)
                    {
                        if (generalNotesToInsert[i].Notes.TrimEnd(' ') != "")
                        {
                            ilstSystemName.Add(generalNotesToInsert[i].Name_Of_The_Field);
                        }
                    }
                    for (int i = 0; i < generalNotesToUpdate.Count; i++)
                    {
                        if (generalNotesToUpdate[i].Notes.TrimEnd(' ') != "")
                        {
                            ilstSystemName.Add(generalNotesToUpdate[i].Name_Of_The_Field);
                        }
                    }
                }

                if (ilstSystemName != null && ilstSystemName.Count > 0)
                {
                    ilstSystemName = ilstSystemName.Distinct().ToList();
                }
                else
                {
                    ilstSystemName = new List<string>();
                }

                    Session["fillRos"] = objROSManager.BatchOperationsToRosAndGeneralNotes(rosListToInsert.ToArray<ROS>(), rosListToUpdate.ToArray<ROS>(), rosListToDelete.ToArray<ROS>(),
                    generalNotesToInsert.ToArray<GeneralNotes>(), generalNotesToUpdate.ToArray<GeneralNotes>(), rosGeneralNotes, 
                    ClientSession.EncounterId, string.Empty, ilstSystemName);

                Is_copy_previous = false;
                //For Summary System_Name

                //List<string> ilstSystemName = new List<string>();
                //ulong encounterid = 0;
                //ulong humanid = 0;

                //if (rosListToInsert != null && rosListToInsert.Count > 0)
                //{
                //    encounterid = rosListToInsert[0].Encounter_Id;
                //    humanid = rosListToInsert[0].Human_ID;
                //    for (int i = 0; i < rosListToInsert.Count; i++)
                //    {
                //        if (rosListToInsert[i].Status.TrimEnd(' ') != "")
                //        {
                //            ilstSystemName.Add(rosListToInsert[i].System_Name);
                //        }
                //    }
                //    for (int i = 0; i < generalNotesToInsert.Count; i++)
                //    {
                //        if (generalNotesToInsert[i].Notes.TrimEnd(' ') != "")
                //        {
                //            ilstSystemName.Add(generalNotesToInsert[i].Name_Of_The_Field);
                //        }
                //    }
                //}
                //if (rosListToUpdate != null && rosListToUpdate.Count > 0)
                //{
                //    encounterid = rosListToUpdate[0].Encounter_Id;
                //    humanid = rosListToUpdate[0].Human_ID;
                //    for (int i = 0; i < rosListToUpdate.Count; i++)
                //    {
                //        if (rosListToUpdate[i].Status.TrimEnd(' ') != "")
                //        {
                //            ilstSystemName.Add(rosListToUpdate[i].System_Name);
                //        }
                //    }
                //    for (int i = 0; i < generalNotesToUpdate.Count; i++)
                //    {
                //        if (generalNotesToUpdate[i].Notes.TrimEnd(' ') != "")
                //        {
                //            ilstSystemName.Add(generalNotesToUpdate[i].Name_Of_The_Field);
                //        }
                //    }
                //}

                //if (rosListToInsert != null && rosListToInsert.Count == 0 && rosListToUpdate != null && rosListToUpdate.Count == 0)
                //{
                //    if (generalNotesToInsert.Count > 0)
                //    {
                //        encounterid = generalNotesToInsert[0].Encounter_ID;
                //        humanid = generalNotesToInsert[0].Human_ID;
                //    }
                //    else if (generalNotesToUpdate.Count > 0)
                //    {
                //        encounterid = generalNotesToUpdate[0].Encounter_ID;
                //        humanid = generalNotesToUpdate[0].Human_ID;
                //    }
                //    for (int i = 0; i < generalNotesToInsert.Count; i++)
                //    {
                //        if (generalNotesToInsert[i].Notes.TrimEnd(' ') != "")
                //        {
                //            ilstSystemName.Add(generalNotesToInsert[i].Name_Of_The_Field);
                //        }
                //    }
                //    for (int i = 0; i < generalNotesToUpdate.Count; i++)
                //    {
                //        if (generalNotesToUpdate[i].Notes.TrimEnd(' ') != "")
                //        {
                //            ilstSystemName.Add(generalNotesToUpdate[i].Name_Of_The_Field);
                //        }
                //    }
                //}
               
                //if (ilstSystemName != null && ilstSystemName.Count > 0)
                //{
                //    ilstSystemName = ilstSystemName.Distinct().ToList();
                    
                    //try
                    //{
                      
                    //    if ( encounterid > 0)
                    //    {
                           
                    //        XmlDocument itemDoc = new XmlDocument();
                            
                            //XmlTextReader itemReader = new XmlTextReader(strXmlFilePath);
                            //itemDoc.Load(obj.ReadxmlBlob("Encounter", ClientSession.EncounterId).InnerXml);
                            ////itemReader.Close();

                            //XmlNodeList xmlsysCheck = itemDoc.GetElementsByTagName("ROSSystemList");
                            //if (xmlsysCheck[0] != null)
                            //{
                            //    XmlNodeList ParentNodeList = itemDoc.GetElementsByTagName("ROSSystemList");
                            //    XmlNodeList xmlModules = itemDoc.GetElementsByTagName("Modules");
                            //    xmlModules[0].RemoveChild(ParentNodeList[0]);
                            //}
                            //if (xmlsysCheck[0] == null && ilstSystemName.Count > 0)
                            //{
                            //    XmlNode xmlSystemNodeParent = itemDoc.CreateNode(XmlNodeType.Element, "ROSSystemList", "");
                            //    XmlNodeList xmlModule = itemDoc.GetElementsByTagName("Modules");
                            //    xmlModule[0].AppendChild(xmlSystemNodeParent);
                            //}
                            //XmlNode xmlSystemNode = null;
                            //XmlAttribute attSysName = null;
                            //XmlAttribute attEncounterid = null;
                            //XmlAttribute atthuman_id = null;

                            //for (int i = 0; i < ilstSystemName.Count; i++)
                            //{
                            //    xmlSystemNode = itemDoc.CreateNode(XmlNodeType.Element, "SystemName", "");

                            //    attSysName = itemDoc.CreateAttribute("System_Name");
                            //    attSysName.Value = ilstSystemName[i];
                            //    xmlSystemNode.Attributes.Append(attSysName);

                            //    attEncounterid = itemDoc.CreateAttribute("Encounter_ID");
                            //    attEncounterid.Value = encounterid.ToString();
                            //    xmlSystemNode.Attributes.Append(attEncounterid);

                            //    atthuman_id = itemDoc.CreateAttribute("Human_ID");
                            //    atthuman_id.Value = humanid.ToString();
                            //    xmlSystemNode.Attributes.Append(atthuman_id);

                            //    XmlNodeList xmlsysList = itemDoc.GetElementsByTagName("ROSSystemList");
                            //    xmlsysList[0].AppendChild(xmlSystemNode);
                            //}
                          //  itemDoc.Save(strXmlFilePath);
                    //        int trycount = 0;
                    //    trytosaveagain:
                    //        try
                    //        {
                    //           // objROSManager.WriteBlob(ClientSession.EncounterId, itemDoc, MySession, ListToInsert, ListToUpdate, ListToDelete, xmlobjEncounter, false);

                    //            // itemDoc.Save(strXmlFilePath);
                    //        }
                    //        catch (Exception xmlexcep)
                    //        {
                    //            trycount++;
                    //            if (trycount <= 3)
                    //            {
                    //                int TimeMilliseconds = 0;
                    //                if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                    //                    TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                    //                Thread.Sleep(TimeMilliseconds);
                    //                string sMsg = string.Empty;
                    //                string sExStackTrace = string.Empty;

                    //                string version = "";
                    //                if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                    //                    version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                    //                string[] server = version.Split('|');
                    //                string serverno = "";
                    //                if (server.Length > 1)
                    //                    serverno = server[1].Trim();

                    //                if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                    //                    sMsg = xmlexcep.InnerException.Message;
                    //                else
                    //                    sMsg = xmlexcep.Message;

                    //                if (xmlexcep != null && xmlexcep.StackTrace != null)
                    //                    sExStackTrace = xmlexcep.StackTrace;

                    //                string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //                string ConnectionData;
                    //                ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    //                using (MySqlConnection con = new MySqlConnection(ConnectionData))
                    //                {
                    //                    using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                    //                    {
                    //                        cmd.Connection = con;
                    //                        try
                    //                        {
                    //                            con.Open();
                    //                            cmd.ExecuteNonQuery();
                    //                            con.Close();
                    //                        }
                    //                        catch
                    //                        {
                    //                        }
                    //                    }
                    //                }
                    //                goto trytosaveagain;
                    //            }
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception(ex.Message );
                    //}

                //}
                //else
                //{
                //    try
                //    {
                //       // if (File.Exists(strXmlFilePath))
                //        {
                //            XmlDocument itemDoc = new XmlDocument();
                //           // XmlTextReader itemReader = new XmlTextReader(strXmlFilePath);
                //           // itemDoc.Load(obj.ReadxmlBlob("Encounter", ClientSession.EncounterId).InnerXml);
                //          //  itemReader.Close();

                //            XmlNodeList xmlsysCheck = itemDoc.GetElementsByTagName("ROSSystemList");
                //            if (xmlsysCheck[0] != null)
                //            {
                //                XmlNodeList ParentNodeList = itemDoc.GetElementsByTagName("ROSSystemList");
                //                XmlNodeList xmlModules = itemDoc.GetElementsByTagName("Modules");
                //                xmlModules[0].RemoveChild(ParentNodeList[0]);
                //            }
                //           // itemDoc.Save(strXmlFilePath);
                //            int trycount = 0;
                //        trytosaveagain:
                //            try
                //            {
                //                //itemDoc.Save(strXmlFilePath);
                //            }
                //            catch (Exception xmlexcep)
                //            {
                //                trycount++;
                //                if (trycount <= 3)
                //                {
                //                    int TimeMilliseconds = 0;
                //                    if (System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"] != null)
                //                        TimeMilliseconds = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ThreadSleepTime"]);

                //                    Thread.Sleep(TimeMilliseconds);
                //                    string sMsg = string.Empty;
                //                    string sExStackTrace = string.Empty;

                //                    string version = "";
                //                    if (System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"] != null)
                //                        version = System.Configuration.ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

                //                    string[] server = version.Split('|');
                //                    string serverno = "";
                //                    if (server.Length > 1)
                //                        serverno = server[1].Trim();

                //                    if (xmlexcep.InnerException != null && xmlexcep.InnerException.Message != null)
                //                        sMsg = xmlexcep.InnerException.Message;
                //                    else
                //                        sMsg = xmlexcep.Message;

                //                    if (xmlexcep != null && xmlexcep.StackTrace != null)
                //                        sExStackTrace = xmlexcep.StackTrace;

                //                    string insertQuery = "insert into  stats_apperrorlog values(0,'" + sMsg.Replace(@"\\", @"\\\\").Replace(@"\", @"\\").Replace(@"\\\\\\\\", @"\\\\").Replace("'", "") + Environment.NewLine + " Retry: " + trycount + "', '" + serverno + "','" + DateTime.Now + "','','0','0','0','" + sExStackTrace.Replace("'", "") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                //                    string ConnectionData;
                //                    ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //                    using (MySqlConnection con = new MySqlConnection(ConnectionData))
                //                    {
                //                        using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                //                        {
                //                            cmd.Connection = con;
                //                            try
                //                            {
                //                                con.Open();
                //                                cmd.ExecuteNonQuery();
                //                                con.Close();
                //                            }
                //                            catch
                //                            {
                //                            }
                //                        }
                //                    }
                //                    goto trytosaveagain;
                //                }
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception(ex.Message );
                //    }
                //}

                fillRos = (FillROS)Session["fillRos"];

                //divLoading.Style.Add("display", "none");

                if ((fillRos.Ros_List != null && fillRos.Ros_List.Count > 0) || (fillRos.General_Notes_List != null && fillRos.General_Notes_List.Count > 0))
                {
                    rosList = fillRos.Ros_List;

                    if (string.Compare(HdnCopyButton.Value, "trueValidate", true) != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "savedSuccessfully();", true);
                    }
                    else
                    {
                        isAvailable = true;
                    }
                }

                rosListToInsert.Clear();
                rosListToUpdate.Clear();
                rosListToDelete.Clear();
                generalNotesToInsert.Clear();
                generalNotesToUpdate.Clear();

            }

            return isAvailable;
        }

        public void CopyPreviousForROS(ulong HumanID, ulong EncounterID, ulong PhysicianID, bool isAlert)
        {
            //Jira CAP-1923
            //if (Session["fillRos"] == null)
            if (Session["fillPrevRos"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                       string.Empty,
                                                       "  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}",
                                                       true);
                return;// Throw session expired message.
            }

            ROSManager objROSManager = new ROSManager();
            FillROS fillRosPastEncounter = new FillROS();

            //Jira CAP-1923
            //fillRosPastEncounter = (FillROS)Session["fillRos"];
            fillRosPastEncounter = (FillROS)Session["fillPrevRos"];

            if (fillRosPastEncounter != null)
            {
                if (fillRosPastEncounter.PreviousEnc == 0)
                {
                    if (isAlert)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        string.Empty,
                                                        "savedSuccessfully(); onCopyPrevious('210010');",
                                                        true);
                    }
                    else

                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                       string.Empty,
                                                       " onCopyPrevious('210010');",
                                                       true);
                    return;
                }
                else if (!fillRosPastEncounter.Physician_Process)
                {
                    if (isAlert)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                           @" savedSuccessfully(); onCopyPrevious('210016');", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                            @" onCopyPrevious('210016');", true);
                    return;
                }
                else if (fillRosPastEncounter.Ros_List != null)
                {
                    if (fillRosPastEncounter.Ros_List.Count == 0)
                    {
                        if (isAlert)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                               @" savedSuccessfully(); onCopyPrevious('170014');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                                @" onCopyPrevious('170014');", true);
                        return;
                    }
                    else
                    {
                        IList<FieldLookup> systemNames = new List<FieldLookup>();
                        UserLookupManager objUserLookupManager = new UserLookupManager();
                        //string sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                        //systemNames = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, "ROSSYSTEM", sex, "Sort_Order").ToArray();
                        //string[] systemNameListString = systemNames.Select(s => ("ROS SYMPTOM FOR " + s.Value.ToUpper())).ToArray();
                        //symptomNamesLookUp = objUserLookupManager.GetFieldLookupListInfoForAll(string.Empty, ClientSession.PhysicianId, systemNameListString).ToArray();

                        IList<string> SystemNames = new List<string>();
                        string sex = string.Empty;

                        if (ClientSession.PatientPaneList != null)
                        {
                            if (ClientSession.PatientPaneList.Count > 0)
                            {
                                //Cap - 1599
                                //sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                                if (ClientSession.PatientPaneList[0].Sex.ToUpper().Contains("MALE"))
                                {
                                    sex = ClientSession.PatientPaneList[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                                }
                                else
                                {
                                    sex = ClientSession.PatientPaneList[0].Sex.ToUpper();
                                }
                            }
                            else
                            {
                                IList<string> ilsHumanTagList = new List<string>();
                                ilsHumanTagList.Add("HumanList");


                                IList<Human> lsthuman = new List<Human>();

                                // if (Is_copy_previous == true)
                                IList<object> humanBlobFinal = new List<object>();
                                humanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilsHumanTagList);

                                if (humanBlobFinal != null && humanBlobFinal.Count > 0)
                                {
                                    if (humanBlobFinal[0] != null)
                                    {
                                        for (int iCount = 0; iCount < ((IList<object>)humanBlobFinal[0]).Count; iCount++)
                                        {
                                            lsthuman.Add((Human)((IList<object>)humanBlobFinal[0])[iCount]);
                                        }

                                    }

                                }
                                if (lsthuman.Count > 0)
                                {
                                    //Cap - 1599
                                    //sex = lsthuman[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                                    if (lsthuman[0].Sex.ToUpper().Contains("MALE") == true)
                                    {
                                        sex = lsthuman[0].Sex.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                                    }
                                    else
                                    {
                                        sex = lsthuman[0].Sex.ToUpper();
                                    }
                                }
                                //string strXmlEncounterPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], "Human_" + ClientSession.HumanId.ToString() + ".xml");
                                //if (File.Exists(strXmlEncounterPath) == true)
                                //{
                                //    using (FileStream fs = new FileStream(strXmlEncounterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                                //    {
                                //        XmlDocument itemDoc = new XmlDocument();
                                //        XmlTextReader xmltxtReader = new XmlTextReader(fs);
                                //        itemDoc.Load(xmltxtReader);
                                //        xmltxtReader.Close();
                                //        XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("HumanList");
                                //        if (xmlNodeList.Count > 0)
                                //        {
                                //            if (xmlNodeList[0].ChildNodes.Count > 0)
                                //            {
                                //                sex = xmlNodeList[0].ChildNodes[0].Attributes["Sex"].Value.ToUpper() == "MALE" ? "FEMALE" : "MALE";
                                //            }

                                //        }
                                //        fs.Close();
                                //        fs.Dispose();
                                //    }
                                //}

                            }
                        }
                        symptomNamesLookUp = objUserLookupManager.GetFieldLookupListforPartialField(ClientSession.PhysicianId, "ROS SYMPTOM FOR", sex).ToArray();
                        SystemNames = symptomNamesLookUp.OrderBy(a => a.Sort_Order).Select(a => a.Doc_Type).Distinct().ToList<string>();

                        Panel grSystemAndSymptom = new Panel();
                        CheckBox chkYesSystemAndSymptom = new CheckBox();
                        CheckBox chkNoSystemAndSymptom = new CheckBox();

                        btnSave.Enabled = true;

                        var fillros = fillRosPastEncounter.Ros_List.GroupBy(a => a.System_Name);
                        string systemName = string.Empty;

                        //Clear all the Checkboxes and Notes fields before setting value from Previous Encounter.
                        CheckBox chkSystemAndSymptom = new CheckBox();
                        Label grwSymptom = new Label();
                        ROS rosObjSystemAndSymptom = null;
                        IList<ROS> rosListOfSystemAndSymptom = new List<ROS>();
                        int symptomGrowLabelCount = 0;

                        for (int i = 0; i < SystemNames.Count; i++)
                        {
                            grSystemAndSymptom = (Panel)pnlReviewOfSystems.FindControl("gb_" + SystemNames[i]);
                            if (grSystemAndSymptom == null)
                            {
                                continue;
                            }
                            chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_All");
                            chkSystemAndSymptom.Checked = false;

                            chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_All");
                            chkSystemAndSymptom.Checked = false;

                            for (int j = 0; j < symptomNamesLookUp.Count(c => c.Field_Name == ("ROS SYMPTOM FOR " + SystemNames[i].ToUpper())); j++)
                            {
                                IList<string> symptomNamesLookUpTemp = symptomNamesLookUp.Where(c => c.Field_Name == ("ROS SYMPTOM FOR " + SystemNames[i].ToUpper())).Select(s => s.Value).ToList();

                                grwSymptom = (Label)pnlReviewOfSystems.FindControl("lblSymptom_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUpTemp[j]);
                                symptomGrowLabelCount++;
                                rosObjSystemAndSymptom = new ROS();
                                rosObjSystemAndSymptom.System_Name = grSystemAndSymptom.GroupingText;
                                rosObjSystemAndSymptom.Symptom_Name = grwSymptom.Text;
                                rosListOfSystemAndSymptom.Add(rosObjSystemAndSymptom);

                                chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUpTemp[j]);
                                chkSystemAndSymptom.Checked = false;

                                chkSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUpTemp[j]);
                                chkSystemAndSymptom.Checked = false;
                            }
                            CustomDLCNew DLC = (CustomDLCNew)pnlReviewOfSystems.FindControl("dlc_" + grSystemAndSymptom.GroupingText.Replace(" ", ""));
                            DLC.txtDLC.Text = string.Empty;
                        }

                        foreach (var items in fillros)
                        {
                            foreach (var item in items)
                            {
                                if (pnlReviewOfSystems.FindControl("gb_" + items.Key) != null)
                                    grSystemAndSymptom = (Panel)pnlReviewOfSystems.FindControl("gb_" + items.Key);
                                if (pnlReviewOfSystems.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_" + item.Symptom_Name) != null)
                                    chkYesSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_" + item.Symptom_Name);
                                if (pnlReviewOfSystems.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_" + item.Symptom_Name) != null)
                                    chkNoSystemAndSymptom = (CheckBox)pnlReviewOfSystems.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_" + item.Symptom_Name);
                                if (chkNoSystemAndSymptom != null && chkYesSystemAndSymptom != null)
                                {
                                    if (string.Compare(item.Status, "Yes", true) == 0)
                                    {
                                        chkYesSystemAndSymptom.Checked = true;
                                        chkNoSystemAndSymptom.Checked = false;
                                    }
                                    else if (string.Compare(item.Status, "No", true) == 0)
                                    {
                                        chkYesSystemAndSymptom.Checked = false;
                                        chkNoSystemAndSymptom.Checked = true;
                                    }
                                    else
                                    {
                                        chkYesSystemAndSymptom.Checked = false;
                                        chkNoSystemAndSymptom.Checked = false;
                                    }
                                }
                            }
                            string text = string.Empty;

                            if (fillRosPastEncounter.General_Notes_List != null &&
                                fillRosPastEncounter.General_Notes_List.Count > 0)

                                text = fillRosPastEncounter.General_Notes_List
                                       .Where(a => a.Name_Of_The_Field.Trim() == items.Key)
                                       .ToList()[0].Notes;

                            CustomDLCNew DLC = (CustomDLCNew)pnlReviewOfSystems.FindControl("dlc_" + grSystemAndSymptom.GroupingText.Replace(" ", ""));
                            DLC.txtDLC.Text = text;
                        }


                        if (isAlert)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                             "AutoSave",
                                                             " savedSuccessfully(); onCopyPrevious('');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                                "AutoSave",
                                                                " onCopyPrevious('');", true);

                    }

                    if (fillRosPastEncounter.ROS_GeneralNotes_List != null &&
                        fillRosPastEncounter.ROS_GeneralNotes_List.Count > 0)
                    {
                        dlcROS.txtDLC.Text = fillRosPastEncounter.ROS_GeneralNotes_List[0].Notes;
                        btnSave.Enabled = true;
                    }
                }
                else
                {
                    if (isAlert)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                         " savedSuccessfully(); onCopyPrevious('170014');", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                         " onCopyPrevious('170014');", true);

                    return;
                }
            }
            else
            {
                if (isAlert)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                     " savedSuccessfully(); onCopyPrevious('170014');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                                     " onCopyPrevious('170014');", true);

                return;
            }
        }
        #endregion
    }
}
