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
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.Design;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;

namespace Acurus.Capella.UI
{
    public partial class frmHistoryNonDrugAllergy : System.Web.UI.Page
    {

        #region Declaration

        Table objTable = new Table();
        Table objTable12 = new Table();
        TableCell tc = null;
        HtmlGenericControl objLabelHeader = new HtmlGenericControl();
        Label objLabel = null;
        CheckBox objCheckBox = null;
        CustomDLCNew objTextBox = null;

        NonDrugAllergyManager objNonDrugAllergyManager = new NonDrugAllergyManager();
        NonDrugAllergyMasterManager objNonDrugAllergyMasterManager = new NonDrugAllergyMasterManager();
        GeneralNotes GeneralNotesObject = new GeneralNotes();
        UserLookupManager objUserLookupManager = new UserLookupManager();

        NonDrugHistoryDTO NonDrugHistoryDTO = null;
        Dictionary<string, string> dictionary = null;
        IList<NonDrugAllergy> NonDrugAllergyLoadList = null;
        IList<NonDrugAllergyMaster> NonDrugAllergyLoadMasterList = null;
        IList<FieldLookup> NonDrugAllergyFieldLookupList = null;
        ulong EncounterId = 0;
        ulong HumanId = 0;
        ulong PhysicianId = 0;
        bool isshowallEnable = true;
        bool is_FieldLookUp = true;
        string ScreenMode = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            frmHistoryNonDrugAllergy _source = (frmHistoryNonDrugAllergy)sender;
            ScreenMode = _source.Page.Request.Params[0];


            if (ScreenMode.ToUpper() == "MENU")
            {
                lblGeneralNotes.Visible = false;
                DLC.Visible = false;
            }


            if (ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN")
                hdnLibraryIcon.Value = "Physician";
            else
                hdnLibraryIcon.Value = "";

            EncounterId = ClientSession.EncounterId;
            HumanId = ClientSession.HumanId;
            PhysicianId = ClientSession.PhysicianId;


            objTable.ID = "tblTest";
            dictionary = new Dictionary<string, string>();
            DLC.txtDLC.MaxLength = 255;
            if (!IsPostBack)
            {
                ClientSession.FlushSession();
                DLC.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
                DLC.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
                DLC.txtDLC.Attributes.Add("onchange", "checkLength(this);");
                chkShowAll.Enabled = false;



                if (string.Compare(Convert.ToString(Session["Client_FromTab"]), "PFSH_Non Drug Allergy", true) == 0)
                {
                    NonDrugHistoryDTO = (NonDrugHistoryDTO)Session["Client_FromTabValues"];
                    Session["Client_FromTabValues"] = null;
                }
                else
                    // NonDrugHistoryDTO = objNonDrugAllergyManager.GetNonDrugHistoryByHumanID(ClientSession.HumanId, EncounterId, "Non Drug Allergy History");
                    NonDrugHistoryDTO = GetNonDrugHistory();
                if (NonDrugHistoryDTO != null)
                {
                    if (NonDrugHistoryDTO.NonDrugList != null && NonDrugHistoryDTO.NonDrugList.Count > 0)
                    {
                        Session["NonDrugAllergyLoadList"] = NonDrugHistoryDTO.NonDrugList;
                        is_FieldLookUp = false;
                        chkShowAll.Enabled = true;
                        isshowallEnable = true;
                    }
                    else if (NonDrugHistoryDTO.NonDrugMasterList != null && NonDrugHistoryDTO.NonDrugMasterList.Count > 0)
                    {
                        Session["NonDrugAllergyLoadMasterList"] = NonDrugHistoryDTO.NonDrugMasterList;
                        is_FieldLookUp = false;
                        chkShowAll.Enabled = true;
                        isshowallEnable = true;
                    }
                    if (NonDrugHistoryDTO.GeneralNotesObject != null)
                    {
                        DLC.txtDLC.Text = NonDrugHistoryDTO.GeneralNotesObject.Notes;
                        Session["GeneralNotes"] = NonDrugHistoryDTO.GeneralNotesObject;
                    }
                }

                if (is_FieldLookUp)
                {
                    Session["NonDrugAllergyFieldLookupList"] = objUserLookupManager.GetFieldLookupList(PhysicianId, "NON DRUG ALLERGY INFO").ToArray();
                    //chkShowAll.Checked = true;
                    chkShowAll.Enabled = false;
                    isshowallEnable = false;

                }

                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);



            }
            bool ctrl = false;
            //DLC.pbDropdown.Attributes.Add("onClick", "Enable_OR_Disable()");
            //DLC.pbDropdown.Click += new ImageClickEventHandler(pbDropdown_Click);
            DLC.txtDLC.Attributes.Add("onChange", "CCTextChanged()");

            if (Session["GeneralNotes"] != null)
                GeneralNotesObject = (GeneralNotes)Session["GeneralNotes"];

            if (Session["NonDrugAllergyLoadList"] != null && ((IList<NonDrugAllergy>)Session["NonDrugAllergyLoadList"]).Count > 0)
            {
                NonDrugAllergyLoadList = (IList<NonDrugAllergy>)Session["NonDrugAllergyLoadList"];
                if (NonDrugAllergyLoadList != null && NonDrugAllergyLoadList.Count > 0)
                {
                    foreach (NonDrugAllergy item in NonDrugAllergyLoadList)
                    {
                        if (!dictionary.ContainsKey(item.Non_Drug_Allergy_History_Info))
                        {
                            createControls(item.Non_Drug_Allergy_History_Info, item.Is_Present, item.Description, item);
                            dictionary.Add(item.Non_Drug_Allergy_History_Info, item.Version.ToString());
                        }
                    }
                    if (!ctrl)
                        createHeaderControls(ref ctrl);

                }
            }
            else if (Session["NonDrugAllergyLoadMasterList"] != null && ((IList<NonDrugAllergyMaster>)Session["NonDrugAllergyLoadMasterList"]).Count > 0)
            {
                NonDrugAllergyLoadMasterList = (IList<NonDrugAllergyMaster>)Session["NonDrugAllergyLoadMasterList"];
                if (NonDrugAllergyLoadMasterList != null && NonDrugAllergyLoadMasterList.Count > 0)
                {
                    foreach (NonDrugAllergyMaster item in NonDrugAllergyLoadMasterList)
                    {
                        if (!dictionary.ContainsKey(item.Non_Drug_Allergy_History_Info))
                        {
                            createControlsFromMaster(item.Non_Drug_Allergy_History_Info, item.Is_Present, item.Description, item);
                            dictionary.Add(item.Non_Drug_Allergy_History_Info, item.Version.ToString());
                        }
                    }
                    if (!ctrl)
                        createHeaderControls(ref ctrl);

                }
            }
            else
            {
                if (Session["NonDrugAllergyFieldLookupList"] != null && !chkShowAll.Checked)
                {
                    NonDrugAllergyFieldLookupList = (IList<FieldLookup>)Session["NonDrugAllergyFieldLookupList"];

                    foreach (FieldLookup item1 in NonDrugAllergyFieldLookupList)
                    {
                        if (!dictionary.ContainsKey(item1.Value))
                        {
                            createControls(item1.Value, string.Empty, string.Empty, null);
                            dictionary.Add(item1.Value, item1.Description);
                        }
                    }
                    if (!ctrl)
                        createHeaderControls(ref ctrl);

                }
            }


            // if (Session["NonDrugAllergyFieldLookupList"] != null && (chkShowAll.Checked||!chkShowAll.Enabled))is_FieldLookUp
            //Cap - 1178
            //if (Session["NonDrugAllergyFieldLookupList"] != null && is_FieldLookUp)
            if (Session["NonDrugAllergyFieldLookupList"] != null && is_FieldLookUp && (chkShowAll.Checked==true || !chkShowAll.Enabled))
            {
                NonDrugAllergyFieldLookupList = (IList<FieldLookup>)Session["NonDrugAllergyFieldLookupList"];

                foreach (FieldLookup item1 in NonDrugAllergyFieldLookupList)
                {
                    if (!dictionary.ContainsKey(item1.Value))
                    {
                        createControls(item1.Value, string.Empty, string.Empty, null);
                        dictionary.Add(item1.Value, item1.Description);
                    }
                }
                if (!ctrl)
                    createHeaderControls(ref ctrl);
            }
            if (!IsPostBack)
            {
                ClientSession.processCheck = true;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);

                if (isshowallEnable)
                    chkShowAll.Enabled = true;
                else
                    chkShowAll.Enabled = false;

                if (string.Compare(Convert.ToString(Session["Client_FromTab"]), "PFSH_Non Drug Allergy", true) == 0)
                {
                    btnSave.Enabled = Session["Client_IsFromEdit"] != null ? Convert.ToBoolean(Session["Client_IsFromEdit"]) : true;
                    Session["Client_IsFromEdit"] = null;
                    Session["Client_FromTab"] = null;
                }
                else
                    btnSave.Enabled = false;
            }
            if (ClientSession.UserRole.Trim().ToUpper() == "CODER" || ClientSession.UserCurrentOwner.Trim() == "UNKNOWN" || (!ClientSession.CheckUser && ClientSession.UserPermission == "R"))
            {
                chkShowAll.Enabled = false;
                btnClearAll.Enabled = false;
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EndWaitCursor", "EndWaitCursor();", true);
        }

        NonDrugHistoryDTO GetNonDrugHistory()
        {
            NonDrugHistoryDTO nonDrugDTO = new NonDrugHistoryDTO();
            if (ScreenMode == "Queue")
            {
                bool _is_from_current_encounter_data = false;
                IList<NonDrugAllergy> NonDruglst = new List<NonDrugAllergy>();
                IList<GeneralNotes> lstGenNotesAll = new List<GeneralNotes>();
                GeneralNotes genrlNotesDrug = new GeneralNotes();

                #region "Code Modified by balaji.TJ - 2023-04-01"

                IList<string> ilstNonDrugTagList = new List<string>();
                ilstNonDrugTagList.Add("NonDrugAllergyList");
                ilstNonDrugTagList.Add("NonDrugAllergyMasterList");
                ilstNonDrugTagList.Add("GeneralNotesNonDrugAllergyList");

                IList<object> ilstNonDrugBlobFinal = new List<object>();
                ilstNonDrugBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstNonDrugTagList);

                if (ilstNonDrugBlobFinal != null && ilstNonDrugBlobFinal.Count > 0)
                {
                    if (ilstNonDrugBlobFinal[0] != null && ((IList<object>)ilstNonDrugBlobFinal[0]).Count >0)
                    {
                        for (int i = 0; i < ((IList<object>)ilstNonDrugBlobFinal[0]).Count; i++)
                        {
                            NonDruglst.Add((NonDrugAllergy)((IList<object>)ilstNonDrugBlobFinal[0])[i]);

                            if (((NonDrugAllergy)((List<object>)ilstNonDrugBlobFinal[0])[i]).Encounter_Id == ClientSession.EncounterId)
                                _is_from_current_encounter_data = true;
                        }
                        if (!_is_from_current_encounter_data)
                        {
                            NonDruglst.Clear();
                            nonDrugDTO = LoadFromMaster(nonDrugDTO);
                        }
                    }
                    else if (ilstNonDrugBlobFinal[1] != null && ((IList<object>)ilstNonDrugBlobFinal[1]).Count > 0)
                    {
                        nonDrugDTO = LoadFromMaster(nonDrugDTO);
                    }
                    if (ilstNonDrugBlobFinal[2] != null && ((IList<object>)ilstNonDrugBlobFinal[2]).Count > 0)
                    {
                        for (int j = 0; j < ((IList<object>)ilstNonDrugBlobFinal[2]).Count; j++)
                        {
                            lstGenNotesAll.Add((GeneralNotes)((IList<object>)ilstNonDrugBlobFinal[2])[j]);
                        }
                    }
                   
                }
                #endregion

                #region "Comment by balaji.TJ -2023-01-04"
                //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml"; //"Encounter" + "_" + ClientSession.EncounterId + ".xml";
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
                //        if (itemDoc.GetElementsByTagName("NonDrugAllergyList") != null && itemDoc.GetElementsByTagName("NonDrugAllergyList").Count > 0)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("NonDrugAllergyList")[0].ChildNodes;
                //            if (xmlTagName != null && xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(NonDrugAllergy));
                //                    NonDrugAllergy NonDrugAllergy = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as NonDrugAllergy;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    //NonDrugAllergy = (NonDrugAllergy)NonDrugAllergy;
                //                    propInfo = from obji in ((NonDrugAllergy)NonDrugAllergy).GetType().GetProperties() select obji;

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
                //                                            property.SetValue(NonDrugAllergy, Convert.ToUInt64(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                            property.SetValue(NonDrugAllergy, Convert.ToString(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                            property.SetValue(NonDrugAllergy, Convert.ToDateTime(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                            property.SetValue(NonDrugAllergy, Convert.ToInt32(nodevalue.Value), null);
                //                                        else

                //                                            property.SetValue(NonDrugAllergy, nodevalue.Value, null);
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }

                //                    NonDruglst.Add(NonDrugAllergy);
                //                    if (NonDrugAllergy.Encounter_Id == ClientSession.EncounterId)
                //                        _is_from_current_encounter_data = true;
                //                }
                //                if (!_is_from_current_encounter_data)
                //                {
                //                    NonDruglst.Clear();
                //                    nonDrugDTO = LoadFromMaster(nonDrugDTO);
                //                }
                //            }

                //        }
                //        else if (itemDoc.GetElementsByTagName("NonDrugAllergyMasterList") != null && itemDoc.GetElementsByTagName("NonDrugAllergyMasterList").Count > 0)
                //        {
                //            nonDrugDTO = LoadFromMaster(nonDrugDTO);
                //        }
                //        if (itemDoc.GetElementsByTagName("GeneralNotesNonDrugAllergyList") != null && itemDoc.GetElementsByTagName("GeneralNotesNonDrugAllergyList").Count > 0)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesNonDrugAllergyList")[0].ChildNodes;

                //            if (xmlTagName != null && xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                //                    GeneralNotes GeneralNotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[0])) as GeneralNotes;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    //GeneralNotes = (GeneralNotes)GeneralNotes;
                //                    propInfo = from obji in ((GeneralNotes)GeneralNotes).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[0].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[0].Attributes[i];
                //                        {
                //                            if (propInfo != null)
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name == nodevalue.Name)
                //                                    {
                //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                            property.SetValue(GeneralNotes, Convert.ToUInt64(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                            property.SetValue(GeneralNotes, Convert.ToString(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                            property.SetValue(GeneralNotes, Convert.ToDateTime(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                            property.SetValue(GeneralNotes, Convert.ToInt32(nodevalue.Value), null);
                //                                        else
                //                                            property.SetValue(GeneralNotes, nodevalue.Value, null);
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    lstGenNotesAll.Add(GeneralNotes);
                //                }
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //    #region Commented
                //    /*if (xmlTagName.Count > 0)
                //    {

                //        string TagName = xmlTagName[0].Name;
                //        XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                //        GeneralNotes GeneralNotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[0])) as GeneralNotes;
                //        IEnumerable<PropertyInfo> propInfo = null;
                //        //GeneralNotes = (GeneralNotes)GeneralNotes;
                //        propInfo = from obji in ((GeneralNotes)GeneralNotes).GetType().GetProperties() select obji;

                //        for (int i = 0; i < xmlTagName[0].Attributes.Count; i++)
                //        {
                //            XmlNode nodevalue = xmlTagName[0].Attributes[i];
                //            {
                //                foreach (PropertyInfo property in propInfo)
                //                {
                //                    if (property.Name == nodevalue.Name)
                //                    {
                //                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                            property.SetValue(GeneralNotes, Convert.ToUInt64(nodevalue.Value), null);
                //                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                            property.SetValue(GeneralNotes, Convert.ToString(nodevalue.Value), null);
                //                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                            property.SetValue(GeneralNotes, Convert.ToDateTime(nodevalue.Value), null);
                //                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                            property.SetValue(GeneralNotes, Convert.ToInt32(nodevalue.Value), null);
                //                        else
                //                            property.SetValue(GeneralNotes, nodevalue.Value, null);
                //                    }
                //                }
                //            }
                //        }
                //        genrlNotesDrug = GeneralNotes;
                //    }
                //}*/
                //    #endregion
                //}


                #endregion
                if (NonDruglst != null && NonDruglst.Count > 0)
                {
                    IList<NonDrugAllergy> lstNDACurrEnc = new List<NonDrugAllergy>();
                    lstNDACurrEnc = (from item in NonDruglst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<NonDrugAllergy>();
                    if (lstNDACurrEnc != null && lstNDACurrEnc.Count > 0)
                    {
                        nonDrugDTO.NonDrugList = lstNDACurrEnc;
                    }
                    else
                    {
                        ulong maxEncId = 0;
                        IList<ulong> lstEncId = (from item in NonDruglst select item.Encounter_Id).Distinct().ToList<ulong>();
                        if (lstEncId != null && lstEncId.Count > 0)
                            maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                        foreach (ulong item in lstEncId)
                            if (item > maxEncId && item < ClientSession.EncounterId)
                                maxEncId = item;
                        lstNDACurrEnc = (from item in NonDruglst where item.Encounter_Id == maxEncId select item).ToList<NonDrugAllergy>();
                        nonDrugDTO.NonDrugList = lstNDACurrEnc;
                    }
                }
                else
                {
                    nonDrugDTO.NonDrugList = NonDruglst;
                }

                if (lstGenNotesAll != null && lstGenNotesAll.Count > 0)
                {
                    IList<GeneralNotes> lstGenCurrEnc = new List<GeneralNotes>();
                    lstGenCurrEnc = (from item in lstGenNotesAll where item.Encounter_ID == ClientSession.EncounterId select item).ToList<GeneralNotes>();
                    if (lstGenCurrEnc != null && lstGenCurrEnc.Count > 0)
                    {
                        genrlNotesDrug = lstGenCurrEnc[0];
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
                            genrlNotesDrug = lstGenCurrEnc[0];
                            genrlNotesDrug.Id = 0;
                        }
                    }
                }

                nonDrugDTO.GeneralNotesObject = genrlNotesDrug;
                //nonDrugDTO.NonDrugList = NonDruglst;
                if (nonDrugDTO.NonDrugList.Count > 0)
                    Session["NonDrugAllergyLoadList"] = nonDrugDTO.NonDrugList;
            }
            else if (ScreenMode == "Menu")
            {

                nonDrugDTO = LoadFromMaster(nonDrugDTO);
            }
            return nonDrugDTO;
        }
        void createHeaderControls(ref bool ctrl)
        {
            TableRow Headertr2 = new TableRow();


            tc = new TableCell();
            tc.Width = Unit.Pixel(141);
            Headertr2.Cells.Add(tc);


            tc = new TableCell();
            tc.Width = 35;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            objLabelHeader = new HtmlGenericControl();
            objLabelHeader.ID = "lblYess";
            objLabelHeader.InnerText = "Yes";
            objLabelHeader.Attributes.Add("class", "LabelStyleBold");
            tc.Controls.Add(objLabelHeader);
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = 5;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            objLabelHeader = new HtmlGenericControl();
            objLabelHeader.ID = "lblNoo";
            objLabelHeader.InnerText = "No";
            objLabelHeader.Attributes.Add("class", "LabelStyleBold");
            tc.Width = 150;
            tc.Controls.Add(objLabelHeader);
            Headertr2.Cells.Add(tc);

            //
            tc = new TableCell();
            tc.Width = 50;
            Headertr2.Cells.Add(tc);

            objLabelHeader = new HtmlGenericControl();
            objLabelHeader.ID = "lblDescriptionn";
            objLabelHeader.InnerText = "Description";
            objLabelHeader.Attributes.Add("class", "LabelStyleBold");
            objLabelHeader.Style.Add("padding-left", "20px");
            tc.Width = 135;
            tc.Controls.Add(objLabelHeader);
            Headertr2.Cells.Add(tc);

            //

            objTable12.Rows.Add(Headertr2);
            divNonDrugAllergyHeaderControls.Controls.Add(objTable12);

            CreateSecondHeaderRow();
            ctrl = true;

        }


        void CreateSecondHeaderRow()
        {
            TableRow Headertr2 = new TableRow();
            tc = new TableCell();
            tc.Width = Unit.Pixel(141);
            Headertr2.Cells.Add(tc);


            tc = new TableCell();
            tc.Width = 47;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            CheckBox objCheckBox = new CheckBox();
            objCheckBox.ID = "chkAllYes";
            objCheckBox.Attributes.Add("onclick", "AllOthersYesOrNo('" + objCheckBox.ID + "');");
            tc.Width = 20;
            tc.Controls.Add(objCheckBox);
            objCheckBox.EnableViewState = false;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = 5;
            Headertr2.Cells.Add(tc);

            tc = new TableCell();
            CheckBox objCheckBox1 = new CheckBox();
            objCheckBox1.ID = "chkAllNo";
            objCheckBox1.Attributes.Add("onclick", "AllOthersYesOrNo('" + objCheckBox1.ID + "');");
            tc.Controls.Add(objCheckBox1);
            objCheckBox1.EnableViewState = false;
            tc.Width = 20;
            Headertr2.Cells.Add(tc);

            objTable12.Rows.Add(Headertr2);
            divNonDrugAllergyHeaderControls.Controls.Add(objTable12);
        }

        public void createControls(string value, string CheckBoxValue, string Notes, NonDrugAllergy NonDrugAllergyData)
        {

            TableRow tr = new TableRow();

            objLabel = new Label();
            tc = new TableCell();
            tc.Width = Unit.Percentage(20);
            objLabel.ID = "lbl" + value;
            objLabel.EnableViewState = false;
            objLabel.Text = value;
            objLabel.Width = 165;
            objLabel.Attributes.Add("class", "spanstyle");
            //BugID:47705
            if (value == "Food" || (NonDrugAllergyData != null && NonDrugAllergyData.Non_Drug_Allergy_History_Info == "Food"))
            {
                objLabel.Attributes.CssStyle.Add("color", "#6504d0");
            }
            tc.Controls.Add(objLabel);
            tr.Cells.Add(tc);

            //some cells have been commented for alignment issue by Pujhitha for BugID:26535
            //tc = new TableCell();
            //tc.Width = 30;
            //tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = Unit.Percentage(3.5);
            objCheckBox = new CheckBox();
            objCheckBox.ID = "chkYes" + value;
            objCheckBox.EnableViewState = false;
            objCheckBox.Checked = CheckBoxValue == "Y" ? true : false;
            objCheckBox.Attributes.Add("onclick", "enableField('" + objCheckBox.ID + "');");
            tc.Controls.Add(objCheckBox);
            tr.Cells.Add(tc);


            //tc = new TableCell();
            //tc.Width = 5;
            //tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = Unit.Percentage(3.5);
            objCheckBox = new CheckBox();
            objCheckBox.ID = "chkNo" + value;
            objCheckBox.EnableViewState = false;
            objCheckBox.Checked = CheckBoxValue == "N" ? true : false;
            objCheckBox.Attributes.Add("onclick", "enableField('" + objCheckBox.ID + "');");
            tc.Controls.Add(objCheckBox);
            tr.Cells.Add(tc);


            //tc = new TableCell();
            //tc.Width = 60;
            //tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = Unit.Percentage(72);
            Panel objPanel = new Panel();
            objPanel.Style.Add(HtmlTextWriterStyle.Width, "100%");
            objPanel.Style.Add(HtmlTextWriterStyle.Height, "100%");
            objPanel.Style.Add(HtmlTextWriterStyle.FontSize, "Small");
            tc.Controls.Add(objPanel);
            CustomDLCNew userCtrl = (CustomDLCNew)LoadControl("~/UserControls/customDLCNew.ascx");
            userCtrl.ID = "DLC" + value.Replace(" ", "");
            userCtrl.txtDLC.Attributes.Add("UserRole", ClientSession.UserRole);
            userCtrl.TextboxHeight = new Unit("40px");
            userCtrl.ListboxHeight = new Unit("80px");//BugID:47705
            userCtrl.TextboxWidth = new Unit("645px");
            userCtrl.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
            userCtrl.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            //userCtrl.pbDropdown.Attributes.Add("onClick", "Enable_OR_Disable()");
            // userCtrl.pbDropdown.Click += new ImageClickEventHandler(pbDropdown_Click);
            userCtrl.txtDLC.Attributes.Add("onChange", "CCTextChanged()");
            userCtrl.txtDLC.Attributes.Add("onchange", "checkLength(this);");
            userCtrl.txtDLC.Text = Notes;
            userCtrl.Value = value;
            userCtrl.Enable = true;
            userCtrl.txtDLC.Enabled = true;
            userCtrl.txtDLC.MaxLength = 255;
            objPanel.Controls.Add(userCtrl);
            tr.Cells.Add(tc);
            //userCtrl.SetTheUBACorPBACForHistoryControls(this.Page);

            //TableRow tr1 = new TableRow();
            //objLabel = new Label();
            //objLabel.EnableViewState = false;
            //tc = new TableCell();
            //objLabel.Text = "_____________________________________________________________________________________________________________________________________"; //"___________________________________________________________________________________________________________________________________________________________________________";
            //tc.ColumnSpan = 4;

            //tc.Controls.Add(objLabel);
            //tr1.Cells.Add(tc);

            objTable.Rows.Add(tr);
            //objTable.Rows.Add(tr1);

            divNonDrugAllergy.Controls.Add(objTable);

            if (NonDrugAllergyData != null)
            {
                if (IsPostBack)
                    UserControlEnableOrDisable(userCtrl, value);
            }
            else
                UserControlEnableOrDisable(userCtrl, value);
        }

        public void createControlsFromMaster(string value, string CheckBoxValue, string Notes, NonDrugAllergyMaster NonDrugAllergyMasterData)
        {

            TableRow tr = new TableRow();

            objLabel = new Label();
            tc = new TableCell();
            tc.Width = Unit.Percentage(20);
            objLabel.ID = "lbl" + value;
            objLabel.EnableViewState = false;
            objLabel.Text = value;
            objLabel.Width = 165;
            objLabel.Attributes.Add("class", "spanstyle");
            //BugID:47705
            if (value == "Food" || (NonDrugAllergyMasterData != null && NonDrugAllergyMasterData.Non_Drug_Allergy_History_Info == "Food"))
            {
                objLabel.Attributes.CssStyle.Add("color", "#6504d0");
            }
            tc.Controls.Add(objLabel);
            tr.Cells.Add(tc);

            //some cells have been commented for alignment issue by Pujhitha for BugID:26535
            //tc = new TableCell();
            //tc.Width = 30;
            //tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = Unit.Percentage(3.5);
            objCheckBox = new CheckBox();
            objCheckBox.ID = "chkYes" + value;
            objCheckBox.EnableViewState = false;
            objCheckBox.Checked = CheckBoxValue == "Y" ? true : false;
            objCheckBox.Attributes.Add("onclick", "enableField('" + objCheckBox.ID + "');");
            tc.Controls.Add(objCheckBox);
            tr.Cells.Add(tc);


            //tc = new TableCell();
            //tc.Width = 5;
            //tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = Unit.Percentage(3.5);
            objCheckBox = new CheckBox();
            objCheckBox.ID = "chkNo" + value;
            objCheckBox.EnableViewState = false;
            objCheckBox.Checked = CheckBoxValue == "N" ? true : false;
            objCheckBox.Attributes.Add("onclick", "enableField('" + objCheckBox.ID + "');");
            tc.Controls.Add(objCheckBox);
            tr.Cells.Add(tc);


            //tc = new TableCell();
            //tc.Width = 60;
            //tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Width = Unit.Percentage(72);
            Panel objPanel = new Panel();
            objPanel.Style.Add(HtmlTextWriterStyle.Width, "100%");
            objPanel.Style.Add(HtmlTextWriterStyle.Height, "100%");
            objPanel.Style.Add(HtmlTextWriterStyle.FontSize, "Small");
            tc.Controls.Add(objPanel);
            CustomDLCNew userCtrl = (CustomDLCNew)LoadControl("~/UserControls/customDLCNew.ascx");
            userCtrl.ID = "DLC" + value.Replace(" ", "");
            userCtrl.txtDLC.Attributes.Add("UserRole", ClientSession.UserRole);
            userCtrl.TextboxHeight = new Unit("40px");
            userCtrl.ListboxHeight = new Unit("80px");//BugID:47705
            userCtrl.TextboxWidth = new Unit("645px");
            userCtrl.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
            userCtrl.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            //userCtrl.pbDropdown.Attributes.Add("onClick", "Enable_OR_Disable()");
            // userCtrl.pbDropdown.Click += new ImageClickEventHandler(pbDropdown_Click);
            userCtrl.txtDLC.Attributes.Add("onChange", "CCTextChanged()");
            userCtrl.txtDLC.Attributes.Add("onchange", "checkLength(this);");
            userCtrl.txtDLC.Text = Notes;
            userCtrl.Value = value;
            userCtrl.Enable = true;
            userCtrl.txtDLC.Enabled = true;
            userCtrl.txtDLC.MaxLength = 255;
            objPanel.Controls.Add(userCtrl);
            tr.Cells.Add(tc);
            //userCtrl.SetTheUBACorPBACForHistoryControls(this.Page);

            //TableRow tr1 = new TableRow();
            //objLabel = new Label();
            //objLabel.EnableViewState = false;
            //tc = new TableCell();
            //objLabel.Text = "_____________________________________________________________________________________________________________________________________"; //"___________________________________________________________________________________________________________________________________________________________________________";
            //tc.ColumnSpan = 4;

            //tc.Controls.Add(objLabel);
            //tr1.Cells.Add(tc);

            objTable.Rows.Add(tr);
            //objTable.Rows.Add(tr1);

            divNonDrugAllergy.Controls.Add(objTable);

            if (NonDrugAllergyMasterData != null)
            {
                if (IsPostBack)
                    UserControlEnableOrDisable(userCtrl, value);
            }
            else
                UserControlEnableOrDisable(userCtrl, value);
        }

        public void UserControlEnableOrDisable(CustomDLCNew userCtrl, string value)
        {
            string chkYes = Request.Form["chkYes" + value];
            string chkNo = Request.Form["chkNo" + value];

            if ((Request.Form["chkYes" + value] != null && Request.Form["chkYes" + value].ToString() == "on") || (Request.Form["chkNo" + value] != null && Request.Form["chkNo" + value].ToString() == "on"))
                userCtrl.Enable = true;
        }

        void pbDropdown_Click(object sender, ImageClickEventArgs e)
        {
            if (Hidden1.Value == "True")
                btnSave.Enabled = true;
        }

        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox objChk = (CheckBox)sender;
            if (objChk.Checked)
            {
                Session["NonDrugAllergyFieldLookupList"] = objUserLookupManager.GetFieldLookupList(PhysicianId, "NON DRUG ALLERGY INFO").ToArray();
                if (Session["NonDrugAllergyFieldLookupList"] != null)
                {
                    NonDrugAllergyFieldLookupList = (IList<FieldLookup>)Session["NonDrugAllergyFieldLookupList"];

                    foreach (FieldLookup item in NonDrugAllergyFieldLookupList)
                    {
                        if (!dictionary.ContainsKey(item.Value))
                        {
                            createControls(item.Value, string.Empty, string.Empty, null);
                            dictionary.Add(item.Value, item.Description);
                        }
                    }
                }
            }
            
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ScreenMode == "Queue")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                DateTime utc = Convert.ToDateTime(strtime);
                ClientSession.bPFSHVerified = false;
                IList<NonDrugAllergy> SaveList = new List<NonDrugAllergy>();
                IList<NonDrugAllergy> UpdateList = new List<NonDrugAllergy>();
                IList<NonDrugAllergy> DeleteList = new List<NonDrugAllergy>();
                NonDrugAllergy objNonDrugAllergy = null;
                if (NotesDuplicate(DLC.txtDLC.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "txtBoxValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180042');", true);
                    return;
                }
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    objNonDrugAllergy = new NonDrugAllergy();
                    if (((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)) != null)
                    {
                        if (((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)).Checked)
                            objNonDrugAllergy.Is_Present = "Y";
                    }

                    if (((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)) != null)
                    {
                        if (((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)).Checked)
                            objNonDrugAllergy.Is_Present = "N";
                    }
                    if (((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC != null)
                        objNonDrugAllergy.Description = ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC.Text;
                    UtilityManager um = new UtilityManager();
                    objNonDrugAllergy.Snomed_Code = um.GetSnomedfromStaticLookup("FoodAllergySnomedList", "FOOD", objNonDrugAllergy.Description);//BugID:47705

                    if (NonDrugAllergyLoadList != null && NonDrugAllergyLoadList.Count > 0 && NonDrugAllergyLoadList.Any(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key))
                    {
                        NonDrugAllergy objList = NonDrugAllergyLoadList.Where(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key).ToList<NonDrugAllergy>()[0];
                        if (objNonDrugAllergy.Is_Present.Trim() == string.Empty)
                        {
                            if (objList.Encounter_Id == EncounterId)
                                DeleteList.Add(objList);
                        }
                        else
                        {
                            if (NonDrugAllergyLoadList.Where(a => a.Non_Drug_Allergy_History_Info == item.Key).Any(a => a.Is_Present != objNonDrugAllergy.Is_Present || a.Description != objNonDrugAllergy.Description))
                            {
                                objList.Is_Present = objNonDrugAllergy.Is_Present;
                                objList.Description = objNonDrugAllergy.Description;
                                objList.Snomed_Code = objNonDrugAllergy.Snomed_Code;
                                objList.Modified_By = ClientSession.UserName;
                                objList.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                if (objList.Encounter_Id == EncounterId)
                                    UpdateList.Add(objList);
                            }
                            if (objList.Encounter_Id != EncounterId)
                            {
                                objList.Encounter_Id = ClientSession.EncounterId;
                                objList.Version = 0;
                                objList.Id = 0;
                                SaveList.Add(objList);
                            }

                        }
                    }
                    else if (NonDrugAllergyLoadMasterList != null && NonDrugAllergyLoadMasterList.Count > 0 && NonDrugAllergyLoadMasterList.Any(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key))
                    {
                        NonDrugAllergyMaster objMasterList = NonDrugAllergyLoadMasterList.Where(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key).ToList<NonDrugAllergyMaster>()[0];
                        NonDrugAllergy objList = new NonDrugAllergy();
                        objList.Human_ID = objMasterList.Human_ID;
                        objList.Description = objMasterList.Description;
                        objList.Non_Drug_Allergy_History_Master_ID = objMasterList.Id;
                        objList.Non_Drug_Allergy_History_Info = objMasterList.Non_Drug_Allergy_History_Info;
                        objList.Snomed_Code = objMasterList.Snomed_Code;
                        objList.Is_Present = objMasterList.Is_Present;
                        //objList.Encounter_Id = EncounterId;

                        if (objNonDrugAllergy.Is_Present.Trim() == string.Empty)
                        {
                            //if (objList.Encounter_Id == EncounterId)
                            DeleteList.Add(objList);
                        }
                        else
                        {
                            if (NonDrugAllergyLoadMasterList.Where(a => a.Non_Drug_Allergy_History_Info == item.Key).Any(a => a.Is_Present != objNonDrugAllergy.Is_Present || a.Description != objNonDrugAllergy.Description))
                            {
                                objList.Is_Present = objNonDrugAllergy.Is_Present;
                                objList.Description = objNonDrugAllergy.Description;
                                objList.Snomed_Code = objNonDrugAllergy.Snomed_Code;
                                objList.Modified_By = ClientSession.UserName;
                                objList.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                if (objList.Encounter_Id == EncounterId)
                                    UpdateList.Add(objList);
                            }
                            if (objList.Encounter_Id != EncounterId)
                            {
                                objList.Encounter_Id = ClientSession.EncounterId;
                                objList.Version = 0;
                                objList.Id = 0;
                                SaveList.Add(objList);
                            }

                        }

                    }
                    else
                    {
                        //CAP-2063
                        if (objNonDrugAllergy.Is_Present != string.Empty || !string.IsNullOrEmpty(objNonDrugAllergy.Description))
                        {
                            objNonDrugAllergy.Human_ID = HumanId;
                            objNonDrugAllergy.Non_Drug_Allergy_History_Info = item.Key;
                            objNonDrugAllergy.Created_By = ClientSession.UserName;
                            objNonDrugAllergy.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            objNonDrugAllergy.Encounter_Id = ClientSession.EncounterId;
                            SaveList.Add(objNonDrugAllergy);
                        }
                    }
                }
                GeneralNotes objGeneralNotes = new GeneralNotes();
                if (GeneralNotesObject != null)
                {

                    if (GeneralNotesObject.Id == 0)
                    {
                        objGeneralNotes.Human_ID = HumanId;
                        objGeneralNotes.Encounter_ID = EncounterId;
                        objGeneralNotes.Parent_Field = "Non Drug Allergy History";
                        objGeneralNotes.Notes = DLC.txtDLC.Text;
                        objGeneralNotes.Created_By = ClientSession.UserName;
                        objGeneralNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                    else
                    {
                        objGeneralNotes = GeneralNotesObject;
                        objGeneralNotes.Notes = DLC.txtDLC.Text;
                        objGeneralNotes.Modified_By = ClientSession.UserName;
                        objGeneralNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    }
                }

                NonDrugHistoryDTO = objNonDrugAllergyManager.SaveUpdateDeleteNonDrugHistory(NonDrugAllergyLoadList, SaveList, UpdateList, DeleteList, HumanId, objGeneralNotes, string.Empty, ClientSession.EncounterId);
                NonDrugHistoryDTO = GetNonDrugHistory();
                Session["NonDrugAllergyLoadList"] = NonDrugHistoryDTO.NonDrugList;

                Session["GeneralNotes"] = NonDrugHistoryDTO.GeneralNotesObject;

                if (Session["NonDrugAllergyLoadList"] != null)
                    NonDrugAllergyLoadList = (IList<NonDrugAllergy>)Session["NonDrugAllergyLoadList"];


                if (Session["GeneralNotes"] != null)
                    GeneralNotesObject = (GeneralNotes)Session["GeneralNotes"];
                if (hdnTabSelected.Value == "true")
                {
                    // UIManager.PatientSummaryBarRefreshed = true;
                    hdnTabSelected.Value = "false";
                }

                // For Patient Summary Bar Updation
                string AllergyText = "";
                List<string> lstToolTip = new List<string>();
                //var AllergyList = new UtilityManager().LoadPatientSummaryList(out lstToolTip);
                IList<int> ilstChangeSummaryBar = new List<int>() { 1 };
                var AllergyList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);

                AllergyList = AllergyList.Where(a => a.ToUpper().StartsWith("ALLERGY-")).ToList();
                if (AllergyList.Count > 0)
                    AllergyText = AllergyList[0].Replace("Allergy-", "");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SetSummary", "SetAllergySummary('" + AllergyText.Replace("'", "&#39;") + "','" + (lstToolTip.Count > 0 ? lstToolTip[0].Replace("\n", "<br/>") : string.Empty).Replace("'", "&#39;") + "');SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");", true);
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");", true);

                ClientSession.bPFSHVerified = false;
                UIManager.IsPFSHVerified = true;
                btnSave.Enabled = false;
                //if (UIManager.PFSH_OpeingFrom.Replace("?", "").Trim() != "Menu")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NonDrugAllergyHistoryPFSH", "EnablePFSH();", true);
                //}

            }
            else if (ScreenMode == "Menu")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                DateTime utc = Convert.ToDateTime(strtime);
                ClientSession.bPFSHVerified = false;
                IList<NonDrugAllergyMaster> SaveList = new List<NonDrugAllergyMaster>();
                IList<NonDrugAllergyMaster> UpdateList = new List<NonDrugAllergyMaster>();
                IList<NonDrugAllergyMaster> DeleteList = new List<NonDrugAllergyMaster>();
                NonDrugAllergyMaster objNonDrugAllergyMaster = null;
                if (NotesDuplicate(DLC.txtDLC.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "txtBoxValidation", "top.window.document.getElementById('ctl00_Loading').style.display = 'none'; {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('180042');", true);
                    return;
                }
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    objNonDrugAllergyMaster = new NonDrugAllergyMaster();
                    if (((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)) != null)
                    {
                        if (((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)).Checked)
                        {
                            objNonDrugAllergyMaster.Is_Present = "Y";
                            objNonDrugAllergyMaster.Is_Deleted = "N";
                        }
                    }

                    if (((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)) != null)
                    {
                        if (((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)).Checked)
                        {
                            objNonDrugAllergyMaster.Is_Present = "N";
                            objNonDrugAllergyMaster.Is_Deleted = "N";
                        }
                    }
                    if (((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC != null)
                        objNonDrugAllergyMaster.Description = ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC.Text;
                    UtilityManager um = new UtilityManager();
                    objNonDrugAllergyMaster.Snomed_Code = um.GetSnomedfromStaticLookup("FoodAllergySnomedList", "FOOD", objNonDrugAllergyMaster.Description);//BugID:47705

                    if (NonDrugAllergyLoadMasterList != null && NonDrugAllergyLoadMasterList.Count > 0 && NonDrugAllergyLoadMasterList.Any(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key))
                    {
                        NonDrugAllergyMaster objList = NonDrugAllergyLoadMasterList.Where(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key).ToList<NonDrugAllergyMaster>()[0];
                        if (objNonDrugAllergyMaster.Is_Present.Trim() == string.Empty)
                        {
                            objList.Is_Deleted = "Y";
                            UpdateList.Add(objList);
                            //DeleteList.Add(objList);
                        }
                        else

                        {
                            if (NonDrugAllergyLoadMasterList.Where(a => a.Non_Drug_Allergy_History_Info == item.Key).Any(a => a.Is_Present != objNonDrugAllergyMaster.Is_Present || a.Description != objNonDrugAllergyMaster.Description))
                            {
                                objList.Is_Present = objNonDrugAllergyMaster.Is_Present;
                                objList.Description = objNonDrugAllergyMaster.Description;
                                objList.Snomed_Code = objNonDrugAllergyMaster.Snomed_Code;
                                objList.Is_Deleted = objNonDrugAllergyMaster.Is_Deleted;
                                objList.Modified_By = ClientSession.UserName;
                                //if (hdnLocalTime.Value != string.Empty)
                                //objList.Modified_Date_And_Time = utc; //Convert.ToDateTime(hdnLocalTime.Value);
                                objList.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                                UpdateList.Add(objList);
                            }
                            //if (objList.Encounter_Id != EncounterId)
                            //{
                            //    objList.Encounter_Id = ClientSession.EncounterId;
                            //    objList.Version = 0;
                            //    objList.Id = 0;
                            //    SaveList.Add(objList);
                            //}

                        }
                    }
                    else
                    {
                        if (objNonDrugAllergyMaster.Is_Present != string.Empty)
                        {
                            objNonDrugAllergyMaster.Human_ID = HumanId;
                            objNonDrugAllergyMaster.Non_Drug_Allergy_History_Info = item.Key;
                            objNonDrugAllergyMaster.Created_By = ClientSession.UserName;
                            //objNonDrugAllergy.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            //if (hdnLocalTime.Value != string.Empty)
                            //objNonDrugAllergy.Created_Date_And_Time = utc;//Convert.ToDateTime(hdnLocalTime.Value);
                            objNonDrugAllergyMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                            SaveList.Add(objNonDrugAllergyMaster);
                        }
                    }
                }
                //GeneralNotes objGeneralNotes = new GeneralNotes();
                //if (GeneralNotesObject != null)
                //{
                //    if (GeneralNotesObject.Id == 0)
                //    {
                //        objGeneralNotes.Human_ID = HumanId;
                //        objGeneralNotes.Encounter_ID = EncounterId;
                //        objGeneralNotes.Parent_Field = "Non Drug Allergy History Master";
                //        objGeneralNotes.Notes = DLC.txtDLC.Text;
                //        objGeneralNotes.Created_By = ClientSession.UserName;
                //        objGeneralNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                //    }
                //    else
                //    {
                //        objGeneralNotes = GeneralNotesObject;
                //        objGeneralNotes.Notes = DLC.txtDLC.Text;
                //        objGeneralNotes.Modified_By = ClientSession.UserName;
                //        objGeneralNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                //    }
                //}

                //NonDrugHistoryDTO = objNonDrugAllergyMasterManager.SaveUpdateDeleteNonDrugHistoryMaster(NonDrugAllergyLoadMasterList, SaveList, UpdateList, DeleteList, HumanId, objGeneralNotes, string.Empty, ClientSession.EncounterId);
                NonDrugHistoryDTO = objNonDrugAllergyMasterManager.SaveUpdateDeleteNonDrugHistoryMaster(NonDrugAllergyLoadMasterList, SaveList, UpdateList, DeleteList, HumanId, null, string.Empty, ClientSession.EncounterId);
                NonDrugHistoryDTO = GetNonDrugHistory();
                Session["NonDrugAllergyLoadMasterList"] = NonDrugHistoryDTO.NonDrugMasterList;

                Session["GeneralNotes"] = NonDrugHistoryDTO.GeneralNotesObject;

                if (Session["NonDrugAllergyLoadMasterList"] != null)
                    NonDrugAllergyLoadMasterList = (IList<NonDrugAllergyMaster>)Session["NonDrugAllergyLoadMasterList"];


                if (Session["GeneralNotes"] != null)
                    GeneralNotesObject = (GeneralNotes)Session["GeneralNotes"];
                if (hdnTabSelected.Value == "true")
                {
                    // UIManager.PatientSummaryBarRefreshed = true;
                    hdnTabSelected.Value = "false";
                }

                // For Patient Summary Bar Updation
                string AllergyText = "";
                List<string> lstToolTip = new List<string>();
                //var AllergyList = new UtilityManager().LoadPatientSummaryList(out lstToolTip);
                IList<int> ilstChangeSummaryBar = new List<int>() { 1 };
                var AllergyList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);

                AllergyList = AllergyList.Where(a => a.ToUpper().StartsWith("ALLERGY-")).ToList();
                if (AllergyList.Count > 0)
                    AllergyText = AllergyList[0].Replace("Allergy-", "");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SetSummary", "SetAllergySummary('" + AllergyText.Replace("'", "&#39;") + "','" + (lstToolTip.Count > 0 ? lstToolTip[0].Replace("\n", "<br/>") : string.Empty).Replace("'", "&#39;") + "');SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");", true);
                //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");", true);

                ClientSession.bPFSHVerified = false;
                UIManager.IsPFSHVerified = true;
                btnSave.Enabled = false;
                //if (UIManager.PFSH_OpeingFrom.Replace("?", "").Trim() != "Menu")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NonDrugAllergyHistoryPFSH", "EnablePFSH();", true);
                //}

            }

        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {

            ((CheckBox)divNonDrugAllergy.FindControl("chkAllYes")).Checked = false;
            ((CheckBox)divNonDrugAllergy.FindControl("chkAllNo")).Checked = false;

            //btnSave.Enabled = true;
            foreach (KeyValuePair<string, string> item in dictionary)
            {
                if (NonDrugAllergyLoadList != null && NonDrugAllergyLoadList.Count > 0 && NonDrugAllergyLoadList.Any(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key.Trim()))
                {
                    NonDrugAllergy objNonDrugAllergy = NonDrugAllergyLoadList.Where(a => a.Non_Drug_Allergy_History_Info.Trim() == item.Key.Trim()).ToList<NonDrugAllergy>()[0];


                    if (objNonDrugAllergy.Is_Present == "Y")
                    {
                        ((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)).Checked = true;
                        ((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)).Checked = false;
                    }
                    else
                    {
                        ((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)).Checked = true;
                        ((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)).Checked = false;
                    }

                    ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC.Text = objNonDrugAllergy.Description;
                    ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC.Enabled = true;
                }
                else
                {
                    ((CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key)).Checked = false;
                    ((CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key)).Checked = false;
                    ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).txtDLC.Text = string.Empty;
                    ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", ""))).Enable = false;

                }



            }
            if (GeneralNotesObject != null)
                DLC.txtDLC.Text = GeneralNotesObject.Notes;
            else
                DLC.txtDLC.Text = string.Empty;



        }

        /*protected void pbD_Click(object sender, ImageClickEventArgs e)
        {
            ListBox objListBox = new ListBox();
            objListBox.ID = "test";
            objListBox.Items.Add(new ListItem("test1"));
            objListBox.Items.Add(new ListItem("test2"));
            objListBox.Items.Add(new ListItem("test3"));
            objListBox.Items.Add(new ListItem("test4"));
            objListBox.Items.Add(new ListItem("test5"));
            objListBox.Items.Add(new ListItem("test6"));
            objListBox.Visible = true;
            objListBox.Width = 300;
            objListBox.Height = 100;
        }*/

        public bool NotesDuplicate(string Value)
        {
            bool is_Duplicate = false;

            string[] aryValue = Value.Split(',');
            for (int i = 0; i < aryValue.Count(); i++)
                aryValue[i] = aryValue[i].Trim();
            if (aryValue != null && aryValue.Length > 0)
            {
                HashSet<string> hashSet = new HashSet<string>(aryValue);
                if (hashSet.Count != aryValue.Length)
                    is_Duplicate = true;
            }
            return is_Duplicate;
        }


        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (ClientSession.UserRole.Trim().ToUpper() != "CODER")
            {
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    //CAP-824:Multiple controls with the same ID
                    var htmlCheckBoxCount = divNonDrugAllergy.Controls.OfType<CheckBox>().Count(controls => controls.ID == "chkYes" + item.Key);
                    var htmlCheckNoBoxCount = divNonDrugAllergy.Controls.OfType<CheckBox>().Count(controls => controls.ID == "chkNo" + item.Key);
                    var htmlDlcCount = divNonDrugAllergy.Controls.OfType<CustomDLCNew>().Count(controls => controls.ID == "DLC" + item.Key.Replace(" ", ""));
                    if (htmlCheckBoxCount == 1 && htmlCheckNoBoxCount == 1 && htmlDlcCount == 1)
                    {
                    CheckBox htmlCheckBox = (CheckBox)divNonDrugAllergy.FindControl("chkYes" + item.Key);
                    CheckBox htmlCheckNoBox = (CheckBox)divNonDrugAllergy.FindControl("chkNo" + item.Key);
                    CustomDLCNew DlcNew = ((CustomDLCNew)divNonDrugAllergy.FindControl("DLC" + item.Key.Replace(" ", "")));
                    DlcNew.txtDLC.MaxLength = 255;
                    if (htmlCheckBox != null && htmlCheckNoBox != null && DlcNew != null)
                        if (!ClientSession.processCheck && ClientSession.UserPermission.ToUpper().Trim() == "R")
                        {

                            htmlCheckBox.Enabled = false;
                            htmlCheckNoBox.Enabled = false;
                            DlcNew.Enable = false;

                            CheckBox htmlAllYesCheckBox = (CheckBox)divNonDrugAllergy.FindControl("chkAllYes");
                            CheckBox htmlAllNoCheckBox = (CheckBox)divNonDrugAllergy.FindControl("chkAllNo");

                            htmlAllYesCheckBox.Enabled = false;
                            htmlAllNoCheckBox.Enabled = false;
                            btnClearAll.Enabled = false;
                            btnSave.Enabled = false;
                        }
                        else
                        {

                            if (!htmlCheckNoBox.Checked && !htmlCheckBox.Checked)
                            {
                                DlcNew.txtDLC.Enabled = false;

                                DlcNew.pbDropdown.Disabled = false;
                                DlcNew.pbDropdown.Attributes.Add("class", "pbDropdownBackgrounddisable");
                                // DlcNew.pbDropdown.Style.Add("background", "#808080");


                            }
                            else
                            {
                                DlcNew.txtDLC.Enabled = true;

                                DlcNew.pbDropdown.Disabled = false;
                                DlcNew.pbDropdown.Attributes.Add("class", "pbDropdownBackground");
                                // DlcNew.pbDropdown.Style.Add("background", "col-6-btn margintop5px");


                            }
                        }
                }
                }

            }

        }

        NonDrugHistoryDTO LoadFromMaster(NonDrugHistoryDTO nonDrugDTO)
        {
            IList<NonDrugAllergyMaster> NonDrugMasterlst = new List<NonDrugAllergyMaster>();
            IList<GeneralNotes> lstGenNotesAll = new List<GeneralNotes>();
            GeneralNotes genrlNotesDrug = new GeneralNotes();

            #region "Code Modified by balaji.TJ - 2023-04-01"
            IList<string> ilstNonDrugTagList = new List<string>();
            ilstNonDrugTagList.Add("NonDrugAllergyMasterList");
            ilstNonDrugTagList.Add("GeneralNotesROSList");

            IList<object> ilstNonDrugBlobList = new List<object>();
            ilstNonDrugBlobList = UtilityManager.ReadBlob(ClientSession.HumanId, ilstNonDrugTagList);

            if (ilstNonDrugBlobList != null && ilstNonDrugBlobList.Count > 0)
            {
                if (ilstNonDrugBlobList[0] != null && ((IList<object>)ilstNonDrugBlobList[0]).Count > 0)
                {
                    for (int i = 0; i < ((IList<object>)ilstNonDrugBlobList[0]).Count; i++)
                    {
                        if (((NonDrugAllergyMaster)((List<object>)ilstNonDrugBlobList[0])[i]).Is_Deleted != "Y")
                            NonDrugMasterlst.Add((NonDrugAllergyMaster)((IList<object>)ilstNonDrugBlobList[0])[i]);                       
                    }                    
                }
                if (ilstNonDrugBlobList[1] != null && ((IList<object>)ilstNonDrugBlobList[1]).Count > 0)
                {
                    for (int K = 0; K < ((IList<object>)ilstNonDrugBlobList[1]).Count; K++)
                    {
                        lstGenNotesAll.Add((GeneralNotes)((IList<object>)ilstNonDrugBlobList[1])[K]);
                    }

                }
            }

            #endregion

            #region "Comment by balaji.TJ  - 2023-01-04"   
            /*
            string FileName = "Human" + "_" + ClientSession.HumanId + ".xml"; //"Encounter" + "_" + ClientSession.EncounterId + ".xml";
            string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            if (File.Exists(strXmlFilePath) == true)
            {
                XmlDocument itemDoc = new XmlDocument();
                XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                XmlNodeList xmlTagName = null;
                using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    itemDoc.Load(fs);
                    XmlText.Close();
                    if (itemDoc.GetElementsByTagName("NonDrugAllergyMasterList") != null && itemDoc.GetElementsByTagName("NonDrugAllergyMasterList").Count > 0)
                    {
                        xmlTagName = itemDoc.GetElementsByTagName("NonDrugAllergyMasterList")[0].ChildNodes;
                        if (xmlTagName != null && xmlTagName.Count > 0)
                        {
                            for (int j = 0; j < xmlTagName.Count; j++)
                            {
                                string TagName = xmlTagName[j].Name;
                                XmlSerializer xmlserializer = new XmlSerializer(typeof(NonDrugAllergyMaster));
                                NonDrugAllergyMaster NonDrugAllergyMaster = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as NonDrugAllergyMaster;
                                IEnumerable<PropertyInfo> propInfo = null;
                                propInfo = from obji in ((NonDrugAllergyMaster)NonDrugAllergyMaster).GetType().GetProperties() select obji;

                                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                                {
                                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                                    {
                                        if (propInfo != null)
                                        {
                                            foreach (PropertyInfo property in propInfo)
                                            {
                                                if (property.Name == nodevalue.Name)
                                                {
                                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                        property.SetValue(NonDrugAllergyMaster, Convert.ToUInt64(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                        property.SetValue(NonDrugAllergyMaster, Convert.ToString(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        property.SetValue(NonDrugAllergyMaster, Convert.ToDateTime(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                        property.SetValue(NonDrugAllergyMaster, Convert.ToInt32(nodevalue.Value), null);
                                                    else
                                                        property.SetValue(NonDrugAllergyMaster, nodevalue.Value, null);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (NonDrugAllergyMaster.Is_Deleted != "Y")
                                    NonDrugMasterlst.Add(NonDrugAllergyMaster);
                            }
                        }
                    }
                    if (itemDoc.GetElementsByTagName("GeneralNotesNonDrugAllergyList") != null && itemDoc.GetElementsByTagName("GeneralNotesNonDrugAllergyList").Count > 0)
                    {
                        xmlTagName = itemDoc.GetElementsByTagName("GeneralNotesNonDrugAllergyList")[0].ChildNodes;

                        if (xmlTagName != null && xmlTagName.Count > 0)
                        {
                            for (int j = 0; j < xmlTagName.Count; j++)
                            {
                                string TagName = xmlTagName[j].Name;
                                XmlSerializer xmlserializer = new XmlSerializer(typeof(GeneralNotes));
                                GeneralNotes GeneralNotes = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[0])) as GeneralNotes;
                                IEnumerable<PropertyInfo> propInfo = null;
                                //GeneralNotes = (GeneralNotes)GeneralNotes;
                                propInfo = from obji in ((GeneralNotes)GeneralNotes).GetType().GetProperties() select obji;

                                for (int i = 0; i < xmlTagName[0].Attributes.Count; i++)
                                {
                                    XmlNode nodevalue = xmlTagName[0].Attributes[i];
                                    {
                                        if (propInfo != null)
                                        {
                                            foreach (PropertyInfo property in propInfo)
                                            {
                                                if (property.Name == nodevalue.Name)
                                                {
                                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                                                        property.SetValue(GeneralNotes, Convert.ToUInt64(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                                                        property.SetValue(GeneralNotes, Convert.ToString(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                                                        property.SetValue(GeneralNotes, Convert.ToDateTime(nodevalue.Value), null);
                                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                                                        property.SetValue(GeneralNotes, Convert.ToInt32(nodevalue.Value), null);
                                                    else
                                                        property.SetValue(GeneralNotes, nodevalue.Value, null);
                                                }
                                            }
                                        }
                                    }
                                }
                                lstGenNotesAll.Add(GeneralNotes);
                            }
                        }
                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            */
            #endregion
            if (NonDrugMasterlst != null && NonDrugMasterlst.Count > 0)
            {
                nonDrugDTO.NonDrugMasterList = NonDrugMasterlst;
                nonDrugDTO.GeneralNotesObject = genrlNotesDrug;
                if (ScreenMode == "Queue")
                    Session["NonDrugAllergyLoadMasterList"] = nonDrugDTO.NonDrugMasterList;
            }
            else
            {
                if (ScreenMode == "Queue")
                    Session["NonDrugAllergyLoadMasterList"] = null;
            }
            return nonDrugDTO;

        }
    }
}
