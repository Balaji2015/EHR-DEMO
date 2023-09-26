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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;


namespace Acurus.Capella.UI
{
    public partial class frmOtherHistory : System.Web.UI.Page
    {
        #region Declaration
        AdvanceDirectiveManager objAdvanceDirectiveManager = new AdvanceDirectiveManager();
        AdvanceDirectiveMasterManager objAdvanceDirectiveMasterManager = new AdvanceDirectiveMasterManager();
        PhysicianPatientManager objPhysicianPatientManager = new PhysicianPatientManager();
        PhysicianPatientMasterManager objPhysicianPatientMasterManager = new PhysicianPatientMasterManager();
        FillOtherHistory FillOtherHistoryLoad = new FillOtherHistory();
        FileManagementIndexManager objFMI = new FileManagementIndexManager();
        IList<FileManagementIndex> ilstFMI = new List<FileManagementIndex>();
        string ScreenMode = string.Empty;

        string Space_Data = "&nbsp;";
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            frmOtherHistory _source = (frmOtherHistory)sender;
            ScreenMode = _source.Page.Request.Params[0];
            DLC.DName = "pbD";
            if (!IsPostBack)
            {
                ClientSession.FlushSession();
                hdnAddorUpdate.Value = "ADD";
                DLC.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
                DLC.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
                DLC.txtDLC.Attributes.Add("onkeydown", "EnableSave(event);");
                ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                LoadGridWithPageNavigator();

                if (ViewState["FillOtherHistoryLoad"] != null)
                {
                    FillOtherHistoryLoad = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                    if (ScreenMode == "Queue")
                    {
                        if (FillOtherHistoryLoad.Advance_Directive != null && FillOtherHistoryLoad.Advance_Directive.Count > 0)
                        {
                            DLC.txtDLC.Text = FillOtherHistoryLoad.Advance_Directive[0].Comments;
                            if (FillOtherHistoryLoad.Advance_Directive[0].Status.Trim() == string.Empty)
                                cboAdvancedDirectived.SelectedIndex = 0;
                            else
                            {
                                var value = cboAdvancedDirectived.Items.Select((v, i) => new { value = v, index = i }).Where(a => a.value.Text.ToUpper().Trim() == FillOtherHistoryLoad.Advance_Directive[0].Status.ToUpper().Trim()).ToList().First();
                                cboAdvancedDirectived.SelectedIndex = value.index;
                            }
                        }
                        else if (FillOtherHistoryLoad.Advance_Directive_Master != null && FillOtherHistoryLoad.Advance_Directive_Master.Count > 0)
                        {
                            DLC.txtDLC.Text = FillOtherHistoryLoad.Advance_Directive_Master[0].Comments;
                            if (FillOtherHistoryLoad.Advance_Directive_Master[0].Status.Trim() == string.Empty)
                                cboAdvancedDirectived.SelectedIndex = 0;
                            else
                            {
                                var value = cboAdvancedDirectived.Items.Select((v, i) => new { value = v, index = i }).Where(a => a.value.Text.ToUpper().Trim() == FillOtherHistoryLoad.Advance_Directive_Master[0].Status.ToUpper().Trim()).ToList().First();
                                cboAdvancedDirectived.SelectedIndex = value.index;
                            }
                        }
                    }
                    else if (ScreenMode == "Menu")
                    {
                        if (FillOtherHistoryLoad.Advance_Directive_Master != null && FillOtherHistoryLoad.Advance_Directive_Master.Count > 0)
                        {
                            DLC.txtDLC.Text = FillOtherHistoryLoad.Advance_Directive_Master[0].Comments;
                            if (FillOtherHistoryLoad.Advance_Directive_Master[0].Status.Trim() == string.Empty)
                                cboAdvancedDirectived.SelectedIndex = 0;
                            else
                            {
                                var value = cboAdvancedDirectived.Items.Select((v, i) => new { value = v, index = i }).Where(a => a.value.Text.ToUpper().Trim() == FillOtherHistoryLoad.Advance_Directive_Master[0].Status.ToUpper().Trim()).ToList().First();
                                cboAdvancedDirectived.SelectedIndex = value.index;
                            }
                        }

                    }


                    
                }
                else
                {
                    if (grdProviderDeatils.DataSource == null)
                    {
                        grdProviderDeatils.DataSource = new string[] { };
                        grdProviderDeatils.DataBind();
                    }
                }
                if (UIManager.is_Menu_Level_PFSH)
                {
                    this.Page.Items.Add("Title", "frmOtherHistoryMenu");
                }
                ClientSession.processCheck = true;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                btnSave.Enabled = false;
                btnAdd.Enabled = false;
                if (ClientSession.UserPermission == "R" && !ClientSession.CheckUser)
                {
                    btnSave.Enabled = false;
                    btnAdd.Enabled = false;
                    btnClearAll.Enabled = false;
                    btnPhysicianClearAll.Enabled = false;
                    msktxtTelephone.Enabled = false;
                    btnFindProvider.Enabled = false;
                    if (grdProviderDeatils != null && grdProviderDeatils.Items.Count > 0)
                    {
                        grdProviderDeatils.Columns[0].Visible = false;
                        grdProviderDeatils.Columns[1].Visible = false;
                    }
                }


                ilstFMI = objFMI.GetFileListUsingHumanID(ClientSession.HumanId);
                if (ilstFMI != null && ilstFMI.Count > 0)
                {

                    lblStaus.Text = "Available";
                    lblStaus.ForeColor = System.Drawing.ColorTranslator.FromHtml("#1bc71b");
                    FileLink.Attributes.Remove("style");

                    DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//Advance_Directive//" + ClientSession.HumanId));
                    if (!virdir.Exists)
                    {
                        virdir.Create();
                    }
                    FTPImageProcess objftp = new FTPImageProcess();
                    Session["ADFilePAth"] = null;
                    bool status = objftp.DownloadFromADImageServer(ClientSession.HumanId.ToString(), Path.GetFileName(ilstFMI[0].Generate_Link_File_Path), virdir.ToString(),out string sCheckFileNotFoundException);
                    if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                        return;
                    }
                    string spath = virdir + "\\" + Path.GetFileName(ilstFMI[0].Generate_Link_File_Path);
                    Session["ADFilePAth"] = spath;
                }
                else
                {
                    FileLink.Disabled = true;
                }
            }
            
            DLC.txtDLC.Attributes.Add("onChange", "CCTextChanged()");
            if (ViewState["FillOtherHistoryLoad"] != null)
                FillOtherHistoryLoad = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
        }

        FillOtherHistory LoadPhycisianPatientDtls()
        {
            bool _is_from_current_encounter_data = false;
            FillOtherHistory fillOthrHis = new FillOtherHistory();
            IList<PhysicianPatient> PhyPatlst = new List<PhysicianPatient>();
            IList<AdvanceDirective> AdvncDrctvelst = new List<AdvanceDirective>();
            IList<AdvanceDirectiveMaster> AdvncDrctveMasterlst = new List<AdvanceDirectiveMaster>();
            IList<PhysicianPatientMaster> PhyPatMasterlst = new List<PhysicianPatientMaster>();

            IList<object> ilstAdvanceDirectiveMasterBlobFinal = new List<object>();
            IList<string> ilstAdvanceDirectiveMasterTagList = new List<string>();

            #region Commented By Deepak


            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //   // itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);
            //        XmlText.Close();
            //        if (ScreenMode == "Queue")
            //        {
            //            if (itemDoc.GetElementsByTagName("AdvanceDirectiveList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("AdvanceDirectiveList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(AdvanceDirective));
            //                        AdvanceDirective AdvanceDirective = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as AdvanceDirective;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((AdvanceDirective)AdvanceDirective).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(AdvanceDirective, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(AdvanceDirective, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(AdvanceDirective, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(AdvanceDirective, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(AdvanceDirective, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        AdvncDrctvelst.Add(AdvanceDirective);
            //                        if (AdvanceDirective.Encounter_Id == ClientSession.EncounterId)
            //                            _is_from_current_encounter_data = true;
            //                    }
            //                    if (!_is_from_current_encounter_data)
            //                    {
            //                        AdvncDrctvelst.Clear();
            //                        fillOthrHis = LoadFromMaster(fillOthrHis);
            //                    }
            //                }
            //            }
            //            else if (itemDoc.GetElementsByTagName("AdvanceDirectiveMasterList")[0] != null)
            //            {
            //                fillOthrHis = LoadFromMaster(fillOthrHis);
            //            }
            //            if (itemDoc.GetElementsByTagName("PhysicianPatientList")[0] != null)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("PhysicianPatientList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {
            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(PhysicianPatient));
            //                        PhysicianPatient PhysicianPatient = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PhysicianPatient;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        propInfo = from obji in ((PhysicianPatient)PhysicianPatient).GetType().GetProperties() select obji;

            //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                        {
            //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(PhysicianPatient, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(PhysicianPatient, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(PhysicianPatient, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(PhysicianPatient, Convert.ToInt32(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(PhysicianPatient, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        PhyPatlst.Add(PhysicianPatient);
            //                        if (PhysicianPatient.Encounter_Id == ClientSession.EncounterId)
            //                            _is_from_current_encounter_data = true;
            //                    }
            //                    if (!_is_from_current_encounter_data)
            //                    {
            //                        PhyPatlst.Clear();
            //                        fillOthrHis = LoadFromMaster(fillOthrHis);
            //                    }
            //                }
            //            }
            //            else if (itemDoc.GetElementsByTagName("PhysicianPatientMasterList")[0] != null)
            //            {
            //                fillOthrHis = LoadFromMaster(fillOthrHis);
            //            }

            //            //Want to copy
            //            if (AdvncDrctvelst != null && AdvncDrctvelst.Count > 0)
            //            {
            //                IList<AdvanceDirective> lstAdnDirCurrEnc = new List<AdvanceDirective>();
            //                lstAdnDirCurrEnc = (from item in AdvncDrctvelst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<AdvanceDirective>();
            //                if (lstAdnDirCurrEnc != null && lstAdnDirCurrEnc.Count > 0)
            //                {
            //                    fillOthrHis.Advance_Directive = lstAdnDirCurrEnc;
            //                }
            //                else
            //                {
            //                    IList<ulong> lstEncId = (from item in AdvncDrctvelst select item.Encounter_Id).Distinct().ToList<ulong>();
            //                    ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //                    foreach (ulong item in lstEncId)
            //                        if (item > maxEncId && item < ClientSession.EncounterId)
            //                            maxEncId = item;
            //                    lstAdnDirCurrEnc = (from item in AdvncDrctvelst where item.Encounter_Id == maxEncId select item).ToList<AdvanceDirective>();
            //                    fillOthrHis.Advance_Directive = lstAdnDirCurrEnc;
            //                }
            //            }
            //            else
            //            {
            //                fillOthrHis.Advance_Directive = AdvncDrctvelst;
            //            }
            //            if (PhyPatlst != null && PhyPatlst.Count > 0)
            //            {
            //                IList<PhysicianPatient> lstPhyPatCurrEnc = new List<PhysicianPatient>();
            //                lstPhyPatCurrEnc = (from item in PhyPatlst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<PhysicianPatient>();
            //                if (lstPhyPatCurrEnc != null && lstPhyPatCurrEnc.Count > 0)
            //                {
            //                    fillOthrHis.PhysicianPatientList = lstPhyPatCurrEnc;
            //                }
            //                else
            //                {
            //                    IList<ulong> lstEncId = (from item in PhyPatlst select item.Encounter_Id).Distinct().ToList<ulong>();
            //                    ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
            //                    foreach (ulong item in lstEncId)
            //                        if (item > maxEncId && item < ClientSession.EncounterId)
            //                            maxEncId = item;
            //                    lstPhyPatCurrEnc = (from item in PhyPatlst where item.Encounter_Id == maxEncId select item).ToList<PhysicianPatient>();
            //                    fillOthrHis.PhysicianPatientList = lstPhyPatCurrEnc;
            //                }
            //            }
            //            else
            //            {
            //                fillOthrHis.PhysicianPatientList = PhyPatlst;
            //            }
            //        }
            //        else if (ScreenMode == "Menu")
            //        {
            //            fillOthrHis = LoadFromMaster(fillOthrHis);
            //        }
            //        fs.Close();
            //        fs.Dispose();

            //    }
            //}
            #endregion

            ilstAdvanceDirectiveMasterTagList.Add("AdvanceDirectiveList");
            ilstAdvanceDirectiveMasterTagList.Add("AdvanceDirectiveMasterList");
            ilstAdvanceDirectiveMasterTagList.Add("PhysicianPatientList");
            ilstAdvanceDirectiveMasterTagList.Add("PhysicianPatientMasterList");

            ilstAdvanceDirectiveMasterBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstAdvanceDirectiveMasterTagList);

            if (ScreenMode == "Queue")
            {
                if (ilstAdvanceDirectiveMasterBlobFinal != null && ilstAdvanceDirectiveMasterBlobFinal.Count > 0)
                {
                    if (ilstAdvanceDirectiveMasterBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[0]).Count; iCount++)
                        {
                            AdvncDrctvelst.Add((AdvanceDirective)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[0])[iCount]);
                            if (((AdvanceDirective)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[0])[iCount]).Encounter_Id == ClientSession.EncounterId)
                                _is_from_current_encounter_data = true;
                        }
                        if (!_is_from_current_encounter_data)
                        {
                            AdvncDrctvelst.Clear();
                            fillOthrHis = LoadFromMaster(fillOthrHis);
                        }
                    }
                    else if (ilstAdvanceDirectiveMasterBlobFinal[1] != null)
                    {
                        fillOthrHis = LoadFromMaster(fillOthrHis);
                    }
                    if (ilstAdvanceDirectiveMasterBlobFinal[2] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[2]).Count; iCount++)
                        {
                            PhyPatlst.Add((PhysicianPatient)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[2])[iCount]);
                            if (((PhysicianPatient)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[2])[iCount]).Encounter_Id == ClientSession.EncounterId)
                                _is_from_current_encounter_data = true;
                        }
                        if (!_is_from_current_encounter_data)
                        {
                            PhyPatlst.Clear();
                            fillOthrHis = LoadFromMaster(fillOthrHis);
                        }
                    }
                    else if (ilstAdvanceDirectiveMasterBlobFinal[3] != null)
                    {
                        fillOthrHis = LoadFromMaster(fillOthrHis);
                    }
                }

                if (AdvncDrctvelst != null && AdvncDrctvelst.Count > 0)
                {
                    IList<AdvanceDirective> lstAdnDirCurrEnc = new List<AdvanceDirective>();
                    lstAdnDirCurrEnc = (from item in AdvncDrctvelst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<AdvanceDirective>();
                    if (lstAdnDirCurrEnc != null && lstAdnDirCurrEnc.Count > 0)
                    {
                        fillOthrHis.Advance_Directive = lstAdnDirCurrEnc;
                    }
                    else
                    {
                        IList<ulong> lstEncId = (from item in AdvncDrctvelst select item.Encounter_Id).Distinct().ToList<ulong>();
                        ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                        foreach (ulong item in lstEncId)
                            if (item > maxEncId && item < ClientSession.EncounterId)
                                maxEncId = item;
                        lstAdnDirCurrEnc = (from item in AdvncDrctvelst where item.Encounter_Id == maxEncId select item).ToList<AdvanceDirective>();
                        fillOthrHis.Advance_Directive = lstAdnDirCurrEnc;
                    }
                }
                else
                {
                    fillOthrHis.Advance_Directive = AdvncDrctvelst;
                }
                if (PhyPatlst != null && PhyPatlst.Count > 0)
                {
                    IList<PhysicianPatient> lstPhyPatCurrEnc = new List<PhysicianPatient>();
                    lstPhyPatCurrEnc = (from item in PhyPatlst where item.Encounter_Id == ClientSession.EncounterId select item).ToList<PhysicianPatient>();
                    if (lstPhyPatCurrEnc != null && lstPhyPatCurrEnc.Count > 0)
                    {
                        fillOthrHis.PhysicianPatientList = lstPhyPatCurrEnc;
                    }
                    else
                    {
                        IList<ulong> lstEncId = (from item in PhyPatlst select item.Encounter_Id).Distinct().ToList<ulong>();
                        ulong maxEncId = (lstEncId.Min() < ClientSession.EncounterId) ? lstEncId.Min() : 0;
                        foreach (ulong item in lstEncId)
                            if (item > maxEncId && item < ClientSession.EncounterId)
                                maxEncId = item;
                        lstPhyPatCurrEnc = (from item in PhyPatlst where item.Encounter_Id == maxEncId select item).ToList<PhysicianPatient>();
                        fillOthrHis.PhysicianPatientList = lstPhyPatCurrEnc;
                    }
                }
                else
                {
                    fillOthrHis.PhysicianPatientList = PhyPatlst;
                }


            }
            else if (ScreenMode == "Menu")
            {
                fillOthrHis = LoadFromMaster(fillOthrHis);
            }

            return fillOthrHis;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
            if (FillOtherHistoryLoad != null)
            {
                
                if (txtProviderName.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhysicianSaveSuccessfully", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('845002');loadotherHistory();", true);
                    return;
                }
                else
                {
                    if (hdnAddorUpdate.Value.ToUpper() == "ADD")
                    {
                        if (ScreenMode == "Queue")
                        {
                            IList<PhysicianPatient> phyPatientList = new List<PhysicianPatient>();
                            PhysicianPatient phyPat = new PhysicianPatient();
                            phyPatientList = FillOtherHistoryLoad.PhysicianPatientList.ToList();
                            var PhysicianList = (from phy in phyPatientList
                                                 where phy.Physician_Name.Trim().ToLower() == txtProviderName.Text.Trim().ToLower()
                                                 select phy).ToList<PhysicianPatient>();

                            phyPat.Human_ID = ClientSession.HumanId;
                            if (msktxtTelephone.Text != string.Empty)
                            {
                                if (msktxtTelephone.Text.Replace(" ", "").Length == 10)
                                    phyPat.Phone_No = msktxtTelephone.TextWithPromptAndLiterals.TrimEnd('"').TrimStart('"');
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhoneValidation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('845004');loadotherHistory();", true);
                                    return;
                                }
                            }
                            else
                                phyPat.Phone_No = string.Empty;

                            phyPat.Encounter_Id = ClientSession.EncounterId;
                            phyPat.Physician_Name = txtProviderName.Text;
                            phyPat.Relationship = txtSpecialty.Text;
                            phyPat.Created_By = ClientSession.UserName;
                            phyPat.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            IList<PhysicianPatient> phyPatList = new List<PhysicianPatient>();
                            phyPatList.Add(phyPat);
                            foreach (PhysicianPatient item in phyPatientList)
                            {
                                if (item.Encounter_Id != ClientSession.EncounterId)
                                {
                                    item.Id = 0;
                                    item.Version = 0;
                                    item.Encounter_Id = ClientSession.EncounterId;
                                    item.Created_By = ClientSession.UserName;
                                    item.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    phyPatList.Add(item);
                                }
                            }
                            if (Session["LoadADMasterList"] != null)
                            {
                                IList<PhysicianPatientMaster> _loadMasterList = new List<PhysicianPatientMaster>();
                                _loadMasterList = (IList<PhysicianPatientMaster>)Session["LoadADMasterList"];
                                if (_loadMasterList.Count > 0)
                                {
                                    foreach (PhysicianPatientMaster item in _loadMasterList)
                                    {
                                        if (ScreenMode == "Queue")
                                        {
                                            PhysicianPatient obj = new PhysicianPatient();
                                            obj.Phone_No = item.Phone_No;
                                            obj.Physician_Name = item.Physician_Name;
                                            obj.Human_ID = item.Human_ID;
                                            obj.Encounter_Id = ClientSession.EncounterId;
                                            obj.Relationship = item.Relationship;
                                            obj.Created_By = ClientSession.UserName;
                                            obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                            obj.Physician_Patient_Master_ID = item.Id;
                                            phyPatList.Add(obj);
                                        }

                                    }
                                }
                            }
                            FillOtherHistory othrHis = new FillOtherHistory();
                            othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                            if (PhysicianList.Count == 0)
                            {
                                objPhysicianPatientManager.SavePhysicianPatient(phyPatList, othrHis.PhysicianPatientList, ClientSession.HumanId, string.Empty);
                                if (Session["LoadADMasterList"] != null)
                                {
                                    if (((IList<PhysicianPatientMaster>)Session["LoadADMasterList"]).Count > 0)
                                        Session["LoadADMasterList"] = null;
                                }
                                ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                                LoadGridWithPageNavigator();
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhysicianSaveSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                ClientSession.bPFSHVerified = false;
                                UIManager.IsPFSHVerified = true;
                            }
                            else
                            {
                                LoadGridWithPageNavigator();
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Radalert", "radalert('" + PhysicianList[0].Physician_Name + " Physician already exists.',350,150,'List of Specialists Involved in medical care');loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);
                            }
 
                        }
                        else if (ScreenMode == "Menu")
                        {
                            IList<PhysicianPatientMaster> phyPatientMasterList = new List<PhysicianPatientMaster>();
                            PhysicianPatientMaster phyPatMaster = new PhysicianPatientMaster();
                            phyPatientMasterList = FillOtherHistoryLoad.PhysicianPatientMasterList.ToList();
                            var PhysicianMasterList = (from phy in phyPatientMasterList
                                                 where phy.Physician_Name.Trim().ToLower() == txtProviderName.Text.Trim().ToLower()
                                                 select phy).ToList<PhysicianPatientMaster>();

                            phyPatMaster.Human_ID = ClientSession.HumanId;
                            if (msktxtTelephone.Text != string.Empty)
                            {
                                if (msktxtTelephone.Text.Replace(" ", "").Length == 10)
                                    phyPatMaster.Phone_No = msktxtTelephone.TextWithPromptAndLiterals.TrimEnd('"').TrimStart('"');
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhoneValidation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('845004');loadotherHistory();", true);
                                    return;
                                }
                            }
                            else
                                phyPatMaster.Phone_No = string.Empty;

                            phyPatMaster.Physician_Name = txtProviderName.Text;
                            phyPatMaster.Is_Deleted = "N";
                            phyPatMaster.Relationship = txtSpecialty.Text;
                            phyPatMaster.Created_By = ClientSession.UserName;
                            phyPatMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            IList<PhysicianPatientMaster> phyPatMasterList = new List<PhysicianPatientMaster>();
                            phyPatMasterList.Add(phyPatMaster);
                            FillOtherHistory othrHis = new FillOtherHistory();
                            othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                            if (PhysicianMasterList.Count == 0)
                            {
                                objPhysicianPatientMasterManager.SavePhysicianPatientMaster(phyPatMasterList, othrHis.PhysicianPatientMasterList, ClientSession.HumanId, string.Empty);
                                ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                                LoadGridWithPageNavigator();
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhysicianSaveSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                ClientSession.bPFSHVerified = false;
                                UIManager.IsPFSHVerified = true;
                            }
                            else
                            {
                                LoadGridWithPageNavigator();
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Radalert", "radalert('" + PhysicianMasterList[0].Physician_Name + " Physician already exists.',350,150,'List of Specialists Involved in medical care');loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();", true);
                            }

                        }
                        
                    }
                    else if (hdnAddorUpdate.Value.ToUpper() == "UPDATE")
                    {
                        ulong UpdateId = Convert.ToUInt32(ViewState["UpdateId"]);
                        IList<PhysicianPatientMaster> phyPatientMasterList = new List<PhysicianPatientMaster>();
                        PhysicianPatientMaster phyPatMaster = new PhysicianPatientMaster();
                        IList<PhysicianPatient> phyPatList = new List<PhysicianPatient>();
                        IList<PhysicianPatient> SavephyPatList = new List<PhysicianPatient>();
                        IList<PhysicianPatientMaster> phyPatMasterList = new List<PhysicianPatientMaster>();
                        IList<PhysicianPatientMaster> SavephyPatMasterList = new List<PhysicianPatientMaster>();
                        IList<ulong> lstId = new List<ulong>();

                        if (ScreenMode == "Queue")
                        {
                            if ((IList<PhysicianPatientMaster>)Session["LoadADMasterList"] == null)
                            {
                                var exist = (from li in FillOtherHistoryLoad.PhysicianPatientList
                                             where li.Id == UpdateId && li.Human_ID == ClientSession.HumanId
                                             select li);
                                PhysicianPatient phyPat = new PhysicianPatient();
                                IList<PhysicianPatient> phyPatientList = new List<PhysicianPatient>();
                                if (exist.Count() > 0)
                                {
                                    foreach (var exi in exist)
                                    {
                                        phyPat = exi;
                                        if (msktxtTelephone.Text != string.Empty)
                                        {
                                            if (msktxtTelephone.Text.Replace(" ", "").Length == 10)
                                                phyPat.Phone_No = msktxtTelephone.TextWithPromptAndLiterals.TrimEnd('"').TrimStart('"');
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhoneNumberValidaion", "PFSH_SaveUnsuccessful();DisplayErrorMessage('845004');loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                                msktxtTelephone.Focus();
                                                return;
                                            }
                                        }
                                        else
                                            phyPat.Phone_No = string.Empty;

                                       

                                        foreach (PhysicianPatient item in phyPatientList)
                                        {
                                            if (item.Encounter_Id != ClientSession.EncounterId)
                                            {
                                                lstId.Add(item.Id);
                                                item.Id = 0;
                                                item.Version = 0;
                                                item.Physician_Name = item.Physician_Name;
                                                item.Relationship = item.Relationship;
                                                item.Phone_No = item.Phone_No;
                                                item.Encounter_Id = ClientSession.EncounterId;
                                                item.Created_By = ClientSession.UserName;
                                                item.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                item.Modified_By = string.Empty;
                                                item.Modified_Date_And_Time = DateTime.MinValue;
                                                SavephyPatList.Add(item);
                                            }
                                        }

                                        if (!lstId.Any(a => a.ToString() == UpdateId.ToString()))
                                        {
                                            phyPat.Encounter_Id = ClientSession.EncounterId;
                                            phyPat.Physician_Name = txtProviderName.Text;
                                            phyPat.Relationship = txtSpecialty.Text;
                                            phyPat.Modified_By = ClientSession.UserName;
                                            phyPat.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                            phyPatList.Add(phyPat);
                                        }
                                        FillOtherHistory othrHis = new FillOtherHistory();
                                        othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                                        IList<PhysicianPatient> phylst = new List<PhysicianPatient>();
                                        phylst = othrHis.PhysicianPatientList;
                                        objPhysicianPatientManager.UpdatePhysicianPatient(phyPatList, SavephyPatList, ClientSession.HumanId, string.Empty, ClientSession.EncounterId);
                                    }
                                }
                            }
                            else
                            {
                                if ((ScreenMode == "Queue") && ((IList<PhysicianPatientMaster>)Session["LoadADMasterList"]).Count > 0)
                                {
                                    var exist = (from li in FillOtherHistoryLoad.PhysicianPatientMasterList
                                                 where li.Id == UpdateId && li.Human_ID == ClientSession.HumanId
                                                 select li);

                                    if (exist.Count() > 0)
                                    {
                                        foreach (var exi in exist)
                                        {
                                            phyPatMaster = exi;
                                            if (msktxtTelephone.Text != string.Empty)
                                            {
                                                if (msktxtTelephone.Text.Replace(" ", "").Length == 10)
                                                    phyPatMaster.Phone_No = msktxtTelephone.TextWithPromptAndLiterals.TrimEnd('"').TrimStart('"');
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhoneNumberValidaion", "PFSH_SaveUnsuccessful();DisplayErrorMessage('845004');loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                                    msktxtTelephone.Focus();
                                                    return;
                                                }
                                            }
                                            else
                                                phyPatMaster.Phone_No = string.Empty;



                                            foreach (PhysicianPatientMaster item in phyPatientMasterList)
                                            {
                                                lstId.Add(item.Id);
                                                item.Id = 0;
                                                item.Version = 0;
                                                item.Physician_Name = item.Physician_Name;
                                                item.Relationship = item.Relationship;
                                                item.Phone_No = item.Phone_No;
                                                item.Created_By = ClientSession.UserName;
                                                item.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                item.Modified_By = string.Empty;
                                                item.Modified_Date_And_Time = DateTime.MinValue;
                                                SavephyPatMasterList.Add(item);
                                            }

                                            if (!lstId.Any(a => a.ToString() == UpdateId.ToString()))
                                            {
                                                phyPatMaster.Physician_Name = txtProviderName.Text;
                                                phyPatMaster.Relationship = txtSpecialty.Text;
                                                phyPatMaster.Modified_By = ClientSession.UserName;
                                                phyPatMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                phyPatientMasterList.Add(phyPatMaster);
                                            }
                                            FillOtherHistory othrHis = new FillOtherHistory();
                                            othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                                            IList<PhysicianPatient> phylst = new List<PhysicianPatient>();
                                            phylst = othrHis.PhysicianPatientList;
                                            objPhysicianPatientMasterManager.UpdatePhysicianPatientMaster(phyPatientMasterList, SavephyPatMasterList, ClientSession.HumanId, string.Empty);

                                        }
                                    }

                                }//////
                            }
                            
                        }
                        else if (ScreenMode == "Menu")
                        {
                            var exist = (from li in FillOtherHistoryLoad.PhysicianPatientMasterList
                                         where li.Id == UpdateId && li.Human_ID == ClientSession.HumanId
                                         select li);
                          
                            if (exist.Count() > 0)
                            {
                                foreach (var exi in exist)
                                {
                                    phyPatMaster = exi;
                                    if (msktxtTelephone.Text != string.Empty)
                                    {
                                        if (msktxtTelephone.Text.Replace(" ", "").Length == 10)
                                            phyPatMaster.Phone_No = msktxtTelephone.TextWithPromptAndLiterals.TrimEnd('"').TrimStart('"');
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhoneNumberValidaion", "PFSH_SaveUnsuccessful();DisplayErrorMessage('845004');loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                            msktxtTelephone.Focus();
                                            return;
                                        }
                                    }
                                    else
                                        phyPatMaster.Phone_No = string.Empty;

                                    

                                    foreach (PhysicianPatientMaster item in phyPatientMasterList)
                                    {
                                            lstId.Add(item.Id);
                                            item.Id = 0;
                                            item.Version = 0;
                                            item.Physician_Name = item.Physician_Name;
                                            item.Relationship = item.Relationship;
                                            item.Phone_No = item.Phone_No;
                                            item.Created_By = ClientSession.UserName;
                                            item.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                            item.Modified_By = string.Empty;
                                            item.Modified_Date_And_Time = DateTime.MinValue;
                                            SavephyPatMasterList.Add(item);
                                    }

                                    if (!lstId.Any(a => a.ToString() == UpdateId.ToString()))
                                    {
                                        phyPatMaster.Physician_Name = txtProviderName.Text;
                                        phyPatMaster.Relationship = txtSpecialty.Text;
                                        phyPatMaster.Modified_By = ClientSession.UserName;
                                        phyPatMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        phyPatientMasterList.Add(phyPatMaster);
                                    }
                                    FillOtherHistory othrHis = new FillOtherHistory();
                                    othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                                    IList<PhysicianPatient> phylst = new List<PhysicianPatient>();
                                    phylst = othrHis.PhysicianPatientList;
                                    objPhysicianPatientMasterManager.UpdatePhysicianPatientMaster(phyPatientMasterList, SavephyPatMasterList, ClientSession.HumanId, string.Empty);
                                    
                                }
                            }
                        }
                        ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                        LoadGridWithPageNavigator();
                        System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                        text1.InnerText = "A";
                        System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAddAddtionalText");
                        text2.InnerText = "dd";
                        System.Web.UI.HtmlControls.HtmlGenericControl textClearAddOne = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextOne");
                        textClearAddOne.InnerText = "C";
                        System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAll");
                        textClear.InnerText = "l";
                        System.Web.UI.HtmlControls.HtmlGenericControl textClearAddTwo = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextTwo");
                        textClearAddTwo.InnerText = "ear All";
                        btnAdd.AccessKey = "A";
                        btnPhysicianClearAll.AccessKey = "l";
                        hdnAddorUpdate.Value = "ADD";
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PhysicianSaveSuccessfully", "SavedSuccessfully(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory();", true);
                        ClientSession.bPFSHVerified = false;
                        UIManager.IsPFSHVerified = true;
                        
                    }
                }
            }
            btnAdd.Enabled = false;
            txtProviderName.Text = string.Empty;
            txtSpecialty.Text = string.Empty;
            msktxtTelephone.Text = string.Empty;
            msktxtTelephone.Mask = "(###) ###-####";
            hdnAddEnable.Value = "false";
            if (hdnSaveEnable.Value.ToUpper() == "TRUE")
                btnSave.Enabled = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (DLC.txtDLC.Text.Trim() == string.Empty && cboAdvancedDirectived.Text.Trim() == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "NotesValidation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PFSH_SaveUnsuccessful();DisplayErrorMessage('845006');loadotherHistory();", true);
                return;
            }
           
            AdvanceDirective adv = new AdvanceDirective();
            AdvanceDirectiveMaster adMaster = new AdvanceDirectiveMaster();
            if (ScreenMode == "Menu")
            {
                if (FillOtherHistoryLoad.Advance_Directive_Master != null && FillOtherHistoryLoad.Advance_Directive_Master.Count > 0)
                {
                    AdvanceDirectiveMaster _tempAdMaster = FillOtherHistoryLoad.Advance_Directive_Master.Where(a => a.Human_ID == ClientSession.HumanId).ToList<AdvanceDirectiveMaster>()[0];
                    _tempAdMaster.Status = cboAdvancedDirectived.SelectedItem.Text;
                    _tempAdMaster.Comments = DLC.txtDLC.Text;
                    _tempAdMaster.Modified_By = ClientSession.UserName;
                    _tempAdMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objAdvanceDirectiveMasterManager.UpdateAdvanceDirectiveMaster(_tempAdMaster, ClientSession.HumanId, string.Empty);
                     ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ClientSession.bPFSHVerified = false;
                    UIManager.IsPFSHVerified = true;
                }
                else
                {
                    adMaster.Status = cboAdvancedDirectived.SelectedItem.Text;
                    adMaster.Comments = DLC.txtDLC.Text;
                    adMaster.Human_ID = ClientSession.HumanId;
                    adMaster.Created_By = ClientSession.UserName;
                    adMaster.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objAdvanceDirectiveMasterManager.SaveAdvanceDirectiveMaster(adMaster, ClientSession.HumanId, string.Empty);
                    ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ClientSession.bPFSHVerified = false;
                    UIManager.IsPFSHVerified = true;
                }
            }
            else
            {
                if (FillOtherHistoryLoad.Advance_Directive != null && FillOtherHistoryLoad.Advance_Directive.Count > 0)
                {
                    AdvanceDirective updateAdv = FillOtherHistoryLoad.Advance_Directive.Where(a => a.Human_ID == ClientSession.HumanId).ToList<AdvanceDirective>()[0];
                    updateAdv.Status = cboAdvancedDirectived.SelectedItem.Text;
                    updateAdv.Comments = DLC.txtDLC.Text;
                    if (updateAdv.Encounter_Id != ClientSession.EncounterId)
                    {
                        updateAdv.Id = 0;
                        updateAdv.Version = 0;
                        updateAdv.Encounter_Id = ClientSession.EncounterId;
                        updateAdv.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        updateAdv.Created_By = ClientSession.UserName;
                        objAdvanceDirectiveManager.SaveAdvanceDirective(updateAdv, ClientSession.HumanId, string.Empty);
                        ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                    }
                    else
                    {
                        updateAdv.Encounter_Id = ClientSession.EncounterId;
                        updateAdv.Advance_Directive_Master_ID = FillOtherHistoryLoad.Advance_Directive[0].Advance_Directive_Master_ID;
                        updateAdv.Modified_By = ClientSession.UserName;
                        updateAdv.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objAdvanceDirectiveManager.UpdateAdvanceDirective(updateAdv, ClientSession.HumanId, string.Empty);
                        ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                    }
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ClientSession.bPFSHVerified = false;
                    UIManager.IsPFSHVerified = true;
                }
                else if (FillOtherHistoryLoad.Advance_Directive_Master != null && FillOtherHistoryLoad.Advance_Directive_Master.Count > 0)
                {
                    adv.Status = cboAdvancedDirectived.SelectedItem.Text;
                    adv.Comments = DLC.txtDLC.Text;
                    adv.Human_ID = ClientSession.HumanId;
                    adv.Encounter_Id = ClientSession.EncounterId;
                    adv.Created_By = ClientSession.UserName;
                    adv.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    adv.Advance_Directive_Master_ID = FillOtherHistoryLoad.Advance_Directive_Master[0].Id;
                    objAdvanceDirectiveManager.SaveAdvanceDirective(adv, ClientSession.HumanId, string.Empty);
                    ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ClientSession.bPFSHVerified = false;
                    UIManager.IsPFSHVerified = true;
                }
                else 
                {
                    adv.Status = cboAdvancedDirectived.SelectedItem.Text;
                    adv.Comments = DLC.txtDLC.Text;
                    adv.Human_ID = ClientSession.HumanId;
                    adv.Encounter_Id = ClientSession.EncounterId;
                    adv.Created_By = ClientSession.UserName;
                    adv.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objAdvanceDirectiveManager.SaveAdvanceDirective(adv, ClientSession.HumanId, string.Empty);
                    ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully();EnablePFSH(" + ClientSession.EncounterId + ");loadotherHistory(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ClientSession.bPFSHVerified = false;
                    UIManager.IsPFSHVerified = true;

                }
            }
            
            btnSave.Enabled = false;
            hdnSaveEnable.Value = "false";
            if (hdnAddEnable.Value == "true")
                btnAdd.Enabled = true;

        }

        protected void grdProviderDeatils_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Trim() == "Edt")
            {
                txtProviderName.Text = (e.Item.Cells[4].Text.Trim() == Space_Data) ? string.Empty : e.Item.Cells[4].Text;
                txtSpecialty.Text = (e.Item.Cells[5].Text.Trim() == Space_Data) ? string.Empty : e.Item.Cells[5].Text;
                msktxtTelephone.Text = (e.Item.Cells[6].Text.Trim() == Space_Data) ? string.Empty : e.Item.Cells[6].Text;
                btnAdd.Enabled = true;
                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                text1.InnerText = "U";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAddAddtionalText");
                text2.InnerText = "pdate";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAddOne = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextOne");
                textClearAddOne.InnerText = "Ca";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAll");
                textClear.InnerText = "n";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAddTwo = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextTwo");
                textClearAddTwo.InnerText = "cel";
                btnAdd.AccessKey = "U";
                btnPhysicianClearAll.AccessKey = "n";
                hdnAddorUpdate.Value = "UPDATE";
                ViewState["UpdateId"] = e.Item.Cells[7].Text;
            }
            else if (e.CommandName.Trim() == "DeleteRows")
            {
                if (ScreenMode == "Menu")
                {
                    DeleteFromMaster(e.Item.Cells[7].Text, e.Item.Cells[4].Text);

                }
                else if (ScreenMode == "Queue")
                {
                    if (FillOtherHistoryLoad.PhysicianPatientList.Count > 0)
                    {
                        IList<PhysicianPatient> ilistPhySpecDelteList = new List<PhysicianPatient>();
                        ilistPhySpecDelteList = FillOtherHistoryLoad.PhysicianPatientList.Where(a => a.Physician_Name.Trim() == e.Item.Cells[4].Text.Trim() && a.Human_ID == ClientSession.HumanId && a.Id == Convert.ToUInt32(e.Item.Cells[7].Text)).ToList<PhysicianPatient>();
                        FillOtherHistory othrHis = new FillOtherHistory();
                        othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                        IList<PhysicianPatient> phylst = new List<PhysicianPatient>();
                        phylst = othrHis.PhysicianPatientList;
                        IList<PhysicianPatient> SaveList = new List<PhysicianPatient>();
                        SaveList = othrHis.PhysicianPatientList.ToList().FindAll(
                               delegate(PhysicianPatient surgical) { if (surgical.Encounter_Id != ClientSession.EncounterId && surgical.Id != Convert.ToUInt64(e.Item.Cells[7].Text))return true; else return false; });
                        SaveList.ToList().ForEach(action => new Action(delegate() { action.Id = 0; action.Version = 0; action.Created_By = ClientSession.UserName; action.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); action.Encounter_Id = ClientSession.EncounterId; }).Invoke());
                        objPhysicianPatientManager.DeletePhysicianPatient(ilistPhySpecDelteList, SaveList, ClientSession.HumanId, string.Empty, ClientSession.EncounterId);
                    }
                    else if(FillOtherHistoryLoad.PhysicianPatientMasterList.Count > 0)
                    {
                        DeleteFromMaster(e.Item.Cells[7].Text, e.Item.Cells[4].Text);
                    }

                }
                ViewState["FillOtherHistoryLoad"] = LoadPhycisianPatientDtls();
                LoadGridWithPageNavigator();

                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                text1.InnerText = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAddAddtionalText");
                text2.InnerText = "dd";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAddOne = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextOne");
                textClearAddOne.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAll");
                textClear.InnerText = "l";
                System.Web.UI.HtmlControls.HtmlGenericControl textClearAddTwo = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextTwo");
                textClearAddTwo.InnerText = "ear All";
                btnAdd.AccessKey = "A";
                btnPhysicianClearAll.AccessKey = "l";
                btnAdd.Enabled = false;
                txtProviderName.Text = string.Empty;
                txtSpecialty.Text = string.Empty;
                msktxtTelephone.Text = string.Empty;
                msktxtTelephone.Mask = "(###) ###-####";
            }
            if (hdnSaveEnable.Value.ToUpper() == "TRUE")
                btnSave.Enabled = true;

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "loadotherHistory(); ", true);
        }


        public void DeleteFromMaster(string gridDeleteIndex, string gridPhysicianName)
        {
            IList<PhysicianPatientMaster> ilistPhySpecDelteList = new List<PhysicianPatientMaster>();
            ilistPhySpecDelteList = FillOtherHistoryLoad.PhysicianPatientMasterList.Where(a => a.Physician_Name.Trim() == gridPhysicianName.Trim() && a.Human_ID == ClientSession.HumanId && a.Id == Convert.ToUInt32(gridDeleteIndex)).ToList<PhysicianPatientMaster>();
            FillOtherHistory othrHis = new FillOtherHistory();
            othrHis = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
            IList<PhysicianPatientMaster> phylst = new List<PhysicianPatientMaster>();
            phylst = othrHis.PhysicianPatientMasterList;
            IList<PhysicianPatientMaster> SaveList = new List<PhysicianPatientMaster>();
            SaveList = othrHis.PhysicianPatientMasterList.ToList().FindAll(
                   delegate(PhysicianPatientMaster surgical) { if (surgical.Id != Convert.ToUInt64(gridDeleteIndex))return true; else return false; });
            SaveList.ToList().ForEach(action => new Action(delegate() { action.Id = 0; action.Version = 0; action.Created_By = ClientSession.UserName; action.Created_Date_And_Time = UtilityManager.ConvertToUniversal(); }).Invoke());
            ilistPhySpecDelteList[0].Is_Deleted = "Y";
            objPhysicianPatientMasterManager.UpdatePhysicianPatientMaster(ilistPhySpecDelteList, null, ClientSession.HumanId, string.Empty);
        }

        protected void btnClearAllAdvancedDirectivedHidden_Click(object sender, EventArgs e)
        {
            if (ScreenMode == "Queue")
            {
                if (FillOtherHistoryLoad != null && FillOtherHistoryLoad.Advance_Directive != null && FillOtherHistoryLoad.Advance_Directive.Count > 0)
                {
                    DLC.txtDLC.Text = FillOtherHistoryLoad.Advance_Directive[0].Comments;
                    var value = cboAdvancedDirectived.Items.Select((v, i) => new { value = v, index = i }).Where(a => a.value.Text.Trim() == FillOtherHistoryLoad.Advance_Directive[0].Status.Trim()).ToList().First();
                    cboAdvancedDirectived.SelectedIndex = value.index;
                }
                else
                {
                    DLC.txtDLC.Text = string.Empty;
                    cboAdvancedDirectived.SelectedIndex = 0;
                }
            }
            else 
            {
                if (FillOtherHistoryLoad != null && FillOtherHistoryLoad.Advance_Directive_Master != null && FillOtherHistoryLoad.Advance_Directive_Master.Count > 0)
                {
                    DLC.txtDLC.Text = FillOtherHistoryLoad.Advance_Directive_Master[0].Comments;
                    var value = cboAdvancedDirectived.Items.Select((v, i) => new { value = v, index = i }).Where(a => a.value.Text.Trim() == FillOtherHistoryLoad.Advance_Directive_Master[0].Status.Trim()).ToList().First();
                    cboAdvancedDirectived.SelectedIndex = value.index;
                }
                else
                {
                    DLC.txtDLC.Text = string.Empty;
                    cboAdvancedDirectived.SelectedIndex = 0;
                }

            }
            btnSave.Enabled = false;
            if (hdnAddEnable.Value.ToUpper() == "TRUE")
                btnAdd.Enabled = true;
            hdnSaveEnable.Value = "false";
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "loadotherHistory(); ", true);
        }

        #endregion

        #region Methods

        private void LoadGrid(FillOtherHistory FillOtherHistoryLoad)
        {
            if (FillOtherHistoryLoad != null)
            {
                grdProviderDeatils.DataSource = null;
                DataTable objDataTable = new DataTable();
                DataColumn objDataColumn = null;
                objDataColumn = new DataColumn("ProviderName", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("Specialty", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("PhoneNumber", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                objDataColumn = new DataColumn("ID", typeof(string));
                objDataTable.Columns.Add(objDataColumn);
                if (ScreenMode == "Queue")
                {
                    if (FillOtherHistoryLoad.PhysicianPatientList != null && FillOtherHistoryLoad.PhysicianPatientList.Count > 0)
                    {
                        for (int i = 0; i < FillOtherHistoryLoad.PhysicianPatientList.Count; i++)
                        {
                            DataRow objDataRow = objDataTable.NewRow();
                            objDataRow["ProviderName"] = FillOtherHistoryLoad.PhysicianPatientList[i].Physician_Name;
                            objDataRow["Specialty"] = FillOtherHistoryLoad.PhysicianPatientList[i].Relationship;
                            objDataRow["PhoneNumber"] = FillOtherHistoryLoad.PhysicianPatientList[i].Phone_No;
                            objDataRow["ID"] = FillOtherHistoryLoad.PhysicianPatientList[i].Id;
                            objDataTable.Rows.Add(objDataRow);
                        }
                    }
                    else if (FillOtherHistoryLoad.PhysicianPatientMasterList != null && FillOtherHistoryLoad.PhysicianPatientMasterList.Count > 0)
                    {
                        for (int i = 0; i < FillOtherHistoryLoad.PhysicianPatientMasterList.Count; i++)
                        {
                            DataRow objDataRow = objDataTable.NewRow();
                            objDataRow["ProviderName"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Physician_Name;
                            objDataRow["Specialty"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Relationship;
                            objDataRow["PhoneNumber"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Phone_No;
                            objDataRow["ID"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Id;
                            objDataTable.Rows.Add(objDataRow);
                        }
 
                    }

                    grdProviderDeatils.DataSource = objDataTable;
                    grdProviderDeatils.DataBind();
                }
                else if (ScreenMode == "Menu" && FillOtherHistoryLoad.PhysicianPatientMasterList != null && FillOtherHistoryLoad.PhysicianPatientMasterList.Count > 0)
                {
                    for (int i = 0; i < FillOtherHistoryLoad.PhysicianPatientMasterList.Count; i++)
                    {
                        DataRow objDataRow = objDataTable.NewRow();
                        objDataRow["ProviderName"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Physician_Name;
                        objDataRow["Specialty"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Relationship;
                        objDataRow["PhoneNumber"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Phone_No;
                        objDataRow["ID"] = FillOtherHistoryLoad.PhysicianPatientMasterList[i].Id;
                        objDataTable.Rows.Add(objDataRow);
                    }
                }
                grdProviderDeatils.DataSource = objDataTable;
                grdProviderDeatils.DataBind();
            }
            else
            {
                if (grdProviderDeatils.DataSource == null)
                {
                    grdProviderDeatils.DataSource = new string[] { };
                    grdProviderDeatils.DataBind();
                }
            }
        }
        public void LoadGridWithPageNavigator()
        {
            if (ViewState["FillOtherHistoryLoad"] != null)
            {
                FillOtherHistoryLoad = (FillOtherHistory)ViewState["FillOtherHistoryLoad"];
                LoadGrid(FillOtherHistoryLoad);
            }
            else
            {
                grdProviderDeatils.DataSource = new string[] { };
                grdProviderDeatils.DataBind();
            }
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            text1.InnerText = "A";
            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAddAddtionalText");
            text2.InnerText = "dd";
            System.Web.UI.HtmlControls.HtmlGenericControl textClearAddOne = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextOne");
            textClearAddOne.InnerText = "C";
            System.Web.UI.HtmlControls.HtmlGenericControl textClear = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAll");
            textClear.InnerText = "l";
            System.Web.UI.HtmlControls.HtmlGenericControl textClearAddTwo = (System.Web.UI.HtmlControls.HtmlGenericControl)btnPhysicianClearAll.FindControl("SpanPhysicianClearAllAddtionalTextTwo");
            textClearAddTwo.InnerText = "ear All";
            btnAdd.AccessKey = "A";
            btnPhysicianClearAll.AccessKey = "l";
            hdnAddorUpdate.Value = "ADD";
            btnAdd.Enabled = false;
            if (hdnSaveEnable.Value.ToUpper() == "TRUE")
                btnSave.Enabled = true;

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "UpdateSuccessfully", "loadotherHistory(); ", true);
        }
        #endregion

        protected void FileLink_ServerClick(object sender, EventArgs e)
        {
            if (Session["ADFilePAth"] != null)
            {
                string sfileext = Session["ADFilePAth"].ToString();
                if(sfileext.Contains(".pdf")){
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Print Order", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}OpenADFiles();loadotherHistory();", true);
                    //spanProviderName.Attributes.Remove("class");
                    //spanProviderName.Attributes.Add("class", "MandLabelstyle");
                }
                else { 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Print Order", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}OpenADTiffImage('" + sfileext + "');loadotherHistory();", true);
                    //spanProviderName.Attributes.Remove("class");
                    //spanProviderName.Attributes.Add("class", "spanstyle");

                }
            }
       
        }

        public FillOtherHistory LoadFromMaster(FillOtherHistory fillOthrHis)
        {
            IList<AdvanceDirectiveMaster> AdvncDrctveMasterlst = new List<AdvanceDirectiveMaster>();
            IList<PhysicianPatientMaster> PhyPatMasterlst = new List<PhysicianPatientMaster>();

            IList<object> ilstAdvanceDirectiveMasterBlobFinal = new List<object>();
            IList<string> ilstAdvanceDirectiveMasterListTagList = new List<string>();

            #region Commented By Deepak


            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);
            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("AdvanceDirectiveMasterList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("AdvanceDirectiveMasterList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(AdvanceDirectiveMaster));
            //                    AdvanceDirectiveMaster AdvanceDirectiveMaster = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as AdvanceDirectiveMaster;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((AdvanceDirectiveMaster)AdvanceDirectiveMaster).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(AdvanceDirectiveMaster, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(AdvanceDirectiveMaster, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(AdvanceDirectiveMaster, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(AdvanceDirectiveMaster, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(AdvanceDirectiveMaster, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    AdvncDrctveMasterlst.Add(AdvanceDirectiveMaster);
            //                }
            //            }
            //        }
            //        if (itemDoc.GetElementsByTagName("PhysicianPatientMasterList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("PhysicianPatientMasterList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(PhysicianPatientMaster));
            //                    PhysicianPatientMaster _PhysicianPatientMaster = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PhysicianPatientMaster;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((PhysicianPatientMaster)_PhysicianPatientMaster).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(_PhysicianPatientMaster, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(_PhysicianPatientMaster, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(_PhysicianPatientMaster, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(_PhysicianPatientMaster, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(_PhysicianPatientMaster, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    if (_PhysicianPatientMaster.Is_Deleted != "Y")
            //                    PhyPatMasterlst.Add(_PhysicianPatientMaster);
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            #endregion

            ilstAdvanceDirectiveMasterListTagList.Add("AdvanceDirectiveMasterList");
            ilstAdvanceDirectiveMasterListTagList.Add("PhysicianPatientMasterList");

            ilstAdvanceDirectiveMasterBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstAdvanceDirectiveMasterListTagList);

            if (ilstAdvanceDirectiveMasterBlobFinal != null && ilstAdvanceDirectiveMasterBlobFinal.Count > 0)
            {
                if (ilstAdvanceDirectiveMasterBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[0]).Count; iCount++)
                    {

                        AdvncDrctveMasterlst.Add((AdvanceDirectiveMaster)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[0])[iCount]);
                    }
                }

                if (ilstAdvanceDirectiveMasterBlobFinal[1] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[1]).Count; iCount++)
                    {
                        if (((PhysicianPatientMaster)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[1])[iCount]).Is_Deleted != "Y")
                        {
                            PhyPatMasterlst.Add((PhysicianPatientMaster)((IList<object>)ilstAdvanceDirectiveMasterBlobFinal[1])[iCount]);
                        }
                    }
                }
            }


            if (AdvncDrctveMasterlst != null && AdvncDrctveMasterlst.Count > 0)
                fillOthrHis.Advance_Directive_Master = AdvncDrctveMasterlst;

            if (PhyPatMasterlst != null && PhyPatMasterlst.Count > 0)
            {
                fillOthrHis.PhysicianPatientMasterList = PhyPatMasterlst;
                if (ScreenMode == "Queue")
                    Session["LoadADMasterList"] = fillOthrHis.PhysicianPatientMasterList;
            }
            else 
            {
                if (ScreenMode == "Queue")
                    Session["LoadADMasterList"] = null;
            }
           

            return fillOthrHis;
 
        }
    }
}

