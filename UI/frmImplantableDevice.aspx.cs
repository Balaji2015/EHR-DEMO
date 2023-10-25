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
using Telerik.Web;
using Telerik.Web.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Drawing;
using System.Net;
using System.Security.Authentication;

namespace Acurus.Capella.UI
{
    public partial class frmImplantableDevice : System.Web.UI.Page
    {
        string sGmdnPTDefinition = string.Empty;
        bool is_DI;
        string sDevice_ID_from_UDI = string.Empty;
        InHouseProcedureManager ObjInHouse_Mgr = new InHouseProcedureManager();
        public InHouseProcedureDTO objOtherProDTO = null;
        InHouseProcedure objOtherProc;
        public IList<InHouseProcedure> lstUpdate;

        public ulong EncounterID
        {
            get
            {

                return ViewState["EncounterID"] == null ? 0 : Convert.ToUInt32(ViewState["EncounterID"]);
            }
            set
            {
                ViewState["EncounterID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            pbProcedure.MyTextBox = ctmDLC_procedure.txtDLC;
            pbProcedure.MyTextBox.Height = 50;
            pbProcedure.MyPlaceHolder = phlProce;
            pbProcedure.MyGridHeight = 210;
            ctmDLC_procedure.txtDLC.TextMode = TextBoxMode.MultiLine;
            ctmDLC_procedure.txtDLC.Attributes.Add("onChange", "CCTextChanged()");
            ctmDLC_procedure.txtDLC.Attributes.Add("OnKeyPress", "GetKeyPress()");
            pbProcedure.pbCustomPhrases.Style.Add(HtmlTextWriterStyle.Display, "block");

            if (!IsPostBack)
            {
                ddActive.Items.Clear();
                ddActive.Items.Add("Active");
                ddActive.Items.Add("Inactive");

                //Assign Procedure code selected in procedure screen ,Notes,Save or update mode
                if (Request["ProcedureCode"] != null && Request["ProcedureCode"].Trim() != string.Empty)
                {
                    txtProcedure.Text = Request["ProcedureCode"];

                }
                if (Request["Notes"] != null && Request["Notes"].Trim() != string.Empty)
                {
                    ctmDLC_procedure.txtDLC.Text = Request["Notes"];
                }
                if (Request["EncounterID"] != null && Request["EncounterID"].Trim() != string.Empty)
                {
                    EncounterID = Convert.ToUInt32(Request["EncounterID"]);
                }
                else
                {
                    EncounterID = ClientSession.EncounterId;
                }
                if (Request["Issaveorupdate"] != null && Request["Issaveorupdate"].Trim() != string.Empty)
                {
                    if (Request["Issaveorupdate"] == "Add")
                        btnAdd.Text = "Save";
                    else
                    { 
                      
                        if (Request["UpdateKeyValue"] != null && Request["UpdateKeyValue"].Trim() != string.Empty)
                        {
                            Update(Request["UpdateKeyValue"].ToString());
                            btnAdd.Text = "Update";
                            btnAdd.ToolTip = "Update";                           
                            btnAdd.AccessKey = "u";
                            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                            text1.InnerText = "U";
                            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                            text2.InnerText = "pdate";
                        }
                    } 
                }

                //Assign Dummy procedure code
                if (txtProcedure.Text.Trim() == string.Empty)
                {
                    txtProcedure.Text = "99999-Implantable Devices";
                }
                btnFind.Enabled = false;
                hdnBtnFind.Value = "";
                btnAdd.Enabled = false;
            }
        }
      
        public void btnAdd_Click(object sender, EventArgs e)
        {
            if(hdnBtnFind.Value==""&&txtDeviceIdentifier.Text.Trim()!=""&&txtDeviceIdentifier.ReadOnly==false)
             {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty,
                  "Order_SaveUnsuccessful();DisplayErrorMessage('280021');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
                    btnFind.Enabled = true;
            }
            else if (txtProcedure.Text == "99999-Implantable Devices" && txtDeviceIdentifier.Text.Trim() == string.Empty && hdnBtnFind.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Implantablealert", "DisplayErrorMessage('280022');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            //CAP-1046
            else if (txtDescription.Text.Trim() == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Implantablealert", "DisplayErrorMessage('280023');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
            IList<InHouseProcedure> lstSave = new List<InHouseProcedure>();
            IList<InHouseProcedure> UpdateList = new List<InHouseProcedure>();
            IList<InHouseProcedure> lstDelete = new List<InHouseProcedure>();
            string sLocalTime = string.Empty;
            if (btnAdd.Text == "Save")
            {
                objOtherProc = new InHouseProcedure();
                if (txtProcedure.Text.Contains('-'))
                {
                    objOtherProc.Procedure_Code = txtProcedure.Text.Trim().Split('-')[0];
                    objOtherProc.Procedure_Code_Description = txtProcedure.Text.Replace(txtProcedure.Text.Trim().Split('-')[0] + "-", "");//txtProcedure.Text.Trim().Split('-')[1];//commented because some procedure code multiple '-'
                }
                objOtherProc.Human_ID = ClientSession.HumanId;
                objOtherProc.Encounter_ID = EncounterID;//ClientSession.EncounterId;
                objOtherProc.Physician_ID = ClientSession.PhysicianId;
                objOtherProc.Notes = ctmDLC_procedure.txtDLC.Text.Trim();
                objOtherProc.Internal_Property_File_Name = string.Empty;
                objOtherProc.Created_By = ClientSession.UserName;
                objOtherProc.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objOtherProc.Modified_By = ClientSession.UserName;
                objOtherProc.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objOtherProc.Serial_Number = txtSerialNumber.Text;
                objOtherProc.Lot_or_Batch = txtLotorBatch.Text;
                objOtherProc.Manufactured_Date = txtManufacturedDate.Text;
                objOtherProc.Expiration_Date = txtExpirationDate.Text;
                objOtherProc.Distinct_ID = txtDistinctID.Text;
                objOtherProc.Issuing_Agency = txtIssuingAgency.Text;
                objOtherProc.Brand_Name = txtBrandName.Text;
                objOtherProc.Version_Model = txtVersionModel.Text;
                objOtherProc.Company_Name = txtCompanyName.Text;
                objOtherProc.MRI_Safety_Status = txtMRISafetyStatus.Text;
                objOtherProc.GMDN_PT_Name = txtGMDNPTName.Text;
                objOtherProc.Description = txtDescription.Text;
                objOtherProc.Rubber_Content = txtRubberContent.Text;
                if (Session["sGmdnPTDefinition"] != null)
                {
                    sGmdnPTDefinition = Convert.ToString(Session["sGmdnPTDefinition"]);
                }
                objOtherProc.GMDN_PT_Definition = sGmdnPTDefinition;
                if (Session["is_Save"] != null)//Check with procedure code with UDI but they didnt click the find button
                {
                    if (txtDeviceIdentifier.Text != null && txtDeviceIdentifier.Text.Trim() != string.Empty)
                    {
                        if (ddActive.SelectedItem.Value == "Active")
                            objOtherProc.Is_Active = "Y";
                        else
                            objOtherProc.Is_Active = "N";
                    }
                    else
                    {
                        objOtherProc.Is_Active = string.Empty;
                    }
                    var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                    if (regexItem.IsMatch(txtDeviceIdentifier.Text))
                    {
                        objOtherProc.Device_Identifier_DI = txtDeviceIdentifier.Text;
                    }
                    else
                    {
                        objOtherProc.Device_Identifier_UDI = txtDeviceIdentifier.Text;
                        if (Session["Device_ID_from_UDI"] != null)
                        {
                            sDevice_ID_from_UDI = Convert.ToString(Session["Device_ID_from_UDI"]);
                        }
                        objOtherProc.Device_Identifier_DI = sDevice_ID_from_UDI;
                    }
                }
                sLocalTime = UtilityManager.ConvertToLocal(objOtherProc.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                lstSave.Add(objOtherProc);
                hdnBtnFind.Value = "";
                objOtherProDTO = ObjInHouse_Mgr.InsertInHouseProcedure(lstSave, null, "", "", sLocalTime, ClientSession.LegalOrg);
            }
            else
            {
                if (ViewState["lstUpdate"] != null)
                { 
                    lstUpdate = (IList<InHouseProcedure>)ViewState["lstUpdate"];
                string FileName = string.Empty;
                bool IsDelete = false;

                if (UIManager.IsAnnotation)
                {
                    if (lstUpdate[0].Internal_Property_File_Name != "")
                        FileName = lstUpdate[0].Internal_Property_File_Name;
                    else
                        FileName = string.Empty;
                }
                else
                {
                    if (lstUpdate[0].Internal_Property_File_Name != "")
                        FileName = lstUpdate[0].Internal_Property_File_Name;
                    else if (lstUpdate[0].Procedure_Code == "99999")
                        IsDelete = false;
                    else
                        IsDelete = true;
                }
                InHouseProcedure objSave = new InHouseProcedure();
                objSave = lstUpdate[0];
                if (txtProcedure.Text.Contains('-'))
                {
                    objSave.Procedure_Code = txtProcedure.Text.Trim().Split('-')[0];
                    objSave.Procedure_Code_Description = txtProcedure.Text.Replace(txtProcedure.Text.Trim().Split('-')[0] + "-", "");
                }
                objSave.Internal_Property_File_Name = FileName;
                objSave.Notes = ctmDLC_procedure.txtDLC.Text.Trim();
                objSave.Modified_By = ClientSession.UserName;
                objSave.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                sLocalTime = UtilityManager.ConvertToLocal(objSave.Modified_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                objSave.Serial_Number = txtSerialNumber.Text;
                objSave.Lot_or_Batch = txtLotorBatch.Text;
                objSave.Manufactured_Date = txtManufacturedDate.Text;
                objSave.Expiration_Date = txtExpirationDate.Text;
                objSave.Distinct_ID = txtDistinctID.Text;
                objSave.Issuing_Agency = txtIssuingAgency.Text;
                objSave.Brand_Name = txtBrandName.Text;
                objSave.Version_Model = txtVersionModel.Text;
                objSave.Company_Name = txtCompanyName.Text;
                objSave.MRI_Safety_Status = txtMRISafetyStatus.Text;
                objSave.GMDN_PT_Name = txtGMDNPTName.Text;
                objSave.Description = txtDescription.Text;
                objSave.Rubber_Content = txtRubberContent.Text;
                if (Session["sGmdnPTDefinition"] != null)
                {
                    sGmdnPTDefinition = Convert.ToString(Session["sGmdnPTDefinition"]);
                }
                objSave.GMDN_PT_Definition = sGmdnPTDefinition;
                if (txtDeviceIdentifier.Text.Trim() != null && txtDeviceIdentifier.Text.Trim() != string.Empty)
                {
                    if (ddActive.SelectedItem.Value == "Active")
                        objSave.Is_Active = "Y";
                    else
                        objSave.Is_Active = "N";
                }
                else
                {
                    objSave.Is_Active = string.Empty;
                }
                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                if (regexItem.IsMatch(txtDeviceIdentifier.Text))
                {
                    objSave.Device_Identifier_DI = txtDeviceIdentifier.Text;
                }
                else
                {
                    objSave.Device_Identifier_UDI = txtDeviceIdentifier.Text;
                    if (Session["Device_ID_from_UDI"] != null)
                    {
                        sDevice_ID_from_UDI = Convert.ToString(Session["Device_ID_from_UDI"]);
                    }
                    objSave.Device_Identifier_DI = sDevice_ID_from_UDI;
                }
                UpdateList.Add(objSave);
                hdnBtnFind.Value = "";

                foreach (InHouseProcedure objDelete in lstUpdate)
                {
                    if (UpdateList.Where(A => A.Id == objDelete.Id).Count() == 0)
                        lstDelete.Add(objDelete);
                }

                objOtherProDTO = ObjInHouse_Mgr.UpdateAndDeleteAndSaveInHouseProcedure(lstSave, UpdateList, lstDelete, IsDelete, "", sLocalTime, ClientSession.LegalOrg);

                btnClearAllImplantable.Text = "Clear All";
                btnClearAllImplantable.ToolTip = "Clear All";
                System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAllImplantable.FindControl("SpanClear");
                text3.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAllImplantable.FindControl("SpanClearAdditional");
                text4.InnerText = "lear All";
            }
            }
            btnAdd.Enabled = false;
            btnFind.Enabled = false;
            //if (ClientSession.EncounterId != 0)
            if (ClientSession.EncounterId != 0 && Request["ScreenMode"] != null && Request["ScreenMode"] != "Menu")
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
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully",
                "Savedsuccessfully();EnableSaveImplantableDiagnosticOrder('false');RefreshNotification('InHouseProcedure');", true);
        }
        }

        void Update(string groupKey)
        {
            btnClearAllImplantable.Enabled = false;
            objOtherProDTO = new InHouseProcedureDTO();
            var lstOtherProcedure = GetFromXML(EncounterID);//ClientSession.EncounterId);
            objOtherProDTO.OtherProcedure = lstOtherProcedure;
            lstUpdate = objOtherProDTO.OtherProcedure.Where(a => a.In_House_Procedure_Group_ID == Convert.ToUInt64(groupKey)).ToList<InHouseProcedure>();
            txtDeviceIdentifier.ReadOnly = true;
            txtDeviceIdentifier.BackColor = Color.FromArgb(191, 219, 255);
            txtDeviceIdentifier.Attributes.Remove("onclick");
            txtDeviceIdentifier.Attributes.Remove("onchange");
            if (lstUpdate[0].Device_Identifier_UDI!="")
                txtDeviceIdentifier.Text = txtDeviceIdentifier.ToolTip = lstUpdate[0].Device_Identifier_UDI;
            else
                txtDeviceIdentifier.Text = txtDeviceIdentifier.ToolTip = lstUpdate[0].Device_Identifier_DI;
            txtSerialNumber.Text = txtSerialNumber.ToolTip = lstUpdate[0].Serial_Number;
            txtLotorBatch.Text = txtLotorBatch.ToolTip = lstUpdate[0].Lot_or_Batch;
            txtManufacturedDate.Text = txtManufacturedDate.ToolTip = lstUpdate[0].Manufactured_Date;
            txtExpirationDate.Text = txtExpirationDate.ToolTip = lstUpdate[0].Expiration_Date;
            txtDistinctID.Text = txtDistinctID.ToolTip = lstUpdate[0].Distinct_ID;
            txtIssuingAgency.Text = txtIssuingAgency.ToolTip = lstUpdate[0].Issuing_Agency;
            txtBrandName.Text = txtBrandName.ToolTip = lstUpdate[0].Brand_Name;
            txtVersionModel.Text = txtVersionModel.ToolTip = lstUpdate[0].Version_Model;
            txtCompanyName.Text = txtCompanyName.ToolTip = lstUpdate[0].Company_Name;
            txtMRISafetyStatus.Text = txtMRISafetyStatus.ToolTip = lstUpdate[0].MRI_Safety_Status;
            txtGMDNPTName.Text = txtGMDNPTName.ToolTip = lstUpdate[0].GMDN_PT_Name;
            txtDescription.Text = txtDescription.ToolTip = lstUpdate[0].Description;
            txtRubberContent.Text = txtRubberContent.ToolTip = lstUpdate[0].Rubber_Content;
            if (lstUpdate[0].Is_Active.ToUpper().Trim() == "N")
                ddActive.SelectedValue = "Inactive";
            else
                ddActive.SelectedValue = "Active";
            ViewState["lstUpdate"] = lstUpdate;

        }

        void ClearImplantabledevice()
        {
            txtSerialNumber.Text = txtSerialNumber.ToolTip = string.Empty;
            txtLotorBatch.Text = txtLotorBatch.ToolTip = string.Empty;
            txtManufacturedDate.Text = txtManufacturedDate.ToolTip = string.Empty;
            txtExpirationDate.Text = txtExpirationDate.ToolTip = string.Empty;
            txtDistinctID.Text = txtDistinctID.ToolTip = string.Empty;
            txtIssuingAgency.Text = txtIssuingAgency.ToolTip = string.Empty;
            txtBrandName.Text = txtBrandName.ToolTip = string.Empty;
            txtVersionModel.Text = txtVersionModel.ToolTip = string.Empty;
            txtManufacturedDate.Text = txtManufacturedDate.ToolTip = string.Empty;
            txtCompanyName.Text = txtCompanyName.ToolTip = string.Empty;
            txtMRISafetyStatus.Text = txtMRISafetyStatus.ToolTip = string.Empty;
            txtGMDNPTName.Text = txtGMDNPTName.ToolTip = string.Empty;
            txtDescription.Text = txtDescription.ToolTip = string.Empty;
            txtRubberContent.Text = txtRubberContent.ToolTip = string.Empty;
            ddActive.SelectedValue = "Active";
            
            is_DI = false;
            sDevice_ID_from_UDI = string.Empty;
            sGmdnPTDefinition = string.Empty;
            btnFind.Enabled = false;
            hdnBtnFind.Value = "";
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StartLoadFind", "{sessionStorage.setItem('StartLoading', 'true');StartLoadFromPatChart();}", true);
            Session["is_Save"] = null;
            sGmdnPTDefinition = string.Empty;
            btnAdd.Enabled = true;
            bool sError = false;
            try
            {
                string sDeviceIdentifierURL = System.Configuration.ConfigurationSettings.AppSettings["DeviceIdentifier"];
                if (sDeviceIdentifierURL != null)
                {
                    if (txtDeviceIdentifier.Text.Trim() != "")
                    {
                        string sOriginalDeviceIdentifier = txtDeviceIdentifier.Text.TrimEnd(' ');
                        string sDeviceIdentifier = txtDeviceIdentifier.Text.TrimEnd(' ');
                        ClearImplantabledevice();
                        var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                        ObjInHouse_Mgr = new InHouseProcedureManager();
                        IList<InHouseProcedure> ilstProcdure = new List<InHouseProcedure>();

                        if (regexItem.IsMatch(sDeviceIdentifier))
                        {
                            is_DI = true;
                            sDeviceIdentifierURL = sDeviceIdentifierURL + "di=";
                            ilstProcdure = ObjInHouse_Mgr.GetImplantableDeviceInformation(sDeviceIdentifier, is_DI);
                        }
                        else
                        {
                            is_DI = false;
                            sDeviceIdentifierURL = sDeviceIdentifierURL + "udi=";
                            ilstProcdure = ObjInHouse_Mgr.GetImplantableDeviceInformation(sDeviceIdentifier, is_DI);
                        }


                        if (ilstProcdure != null && ilstProcdure.Count > 0)
                        {
                            if (is_DI)
                                txtDeviceIdentifier.Text = txtDeviceIdentifier.ToolTip = ilstProcdure[0].Device_Identifier_DI;
                            else
                                txtDeviceIdentifier.Text = txtDeviceIdentifier.ToolTip = ilstProcdure[0].Device_Identifier_UDI;
                            txtSerialNumber.Text = txtSerialNumber.ToolTip = ilstProcdure[0].Serial_Number;
                            txtLotorBatch.Text = txtLotorBatch.ToolTip = ilstProcdure[0].Lot_or_Batch;
                            txtManufacturedDate.Text = txtManufacturedDate.ToolTip = ilstProcdure[0].Manufactured_Date;
                            txtExpirationDate.Text = txtExpirationDate.ToolTip = ilstProcdure[0].Expiration_Date;
                            txtDistinctID.Text = txtDistinctID.ToolTip = ilstProcdure[0].Distinct_ID;
                            txtIssuingAgency.Text = txtIssuingAgency.ToolTip = ilstProcdure[0].Issuing_Agency;
                            txtBrandName.Text = txtBrandName.ToolTip = ilstProcdure[0].Brand_Name;
                            txtVersionModel.Text = txtVersionModel.ToolTip = ilstProcdure[0].Version_Model;
                            txtCompanyName.Text = txtCompanyName.ToolTip = ilstProcdure[0].Company_Name;
                            txtMRISafetyStatus.Text = txtMRISafetyStatus.ToolTip = ilstProcdure[0].MRI_Safety_Status;
                            txtGMDNPTName.Text = txtGMDNPTName.ToolTip = ilstProcdure[0].GMDN_PT_Name;
                            txtDescription.Text = txtDescription.ToolTip = ilstProcdure[0].Description;
                            txtRubberContent.Text = txtRubberContent.ToolTip = ilstProcdure[0].Rubber_Content;
                            Session["sGmdnPTDefinition"] = sGmdnPTDefinition = ilstProcdure[0].GMDN_PT_Definition;
                            if (txtProcedure.Text.Trim() != null && txtProcedure.Text.Trim() == string.Empty)
                            {
                                txtProcedure.Text = "99999 - Implantable Devices";
                            }
                            Session["is_Save"] = true;
                            hdnBtnFind.Value = "true";
                        }
                        else
                        {
                            sDeviceIdentifier = sDeviceIdentifier.Replace("%", "%25");//First replace the % ...
                            sDeviceIdentifier = sDeviceIdentifier.Replace("!", "%21").Replace("#", "%23").Replace("$", "%24").Replace("&", "%26").Replace("'", "%27").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("+", "%2B").Replace(",", "%2C").Replace("/", "%2F").Replace(":", "%3A").Replace(";", "%3B").Replace("=", "%3D").Replace("?", "%3F").Replace("@", "%40").Replace("[", "%5B").Replace("]", "%5D").Replace(" ", "%20").Replace("\"", "%22").Replace("-", "%2D").Replace(".", "%2E").Replace("<", "%3C").Replace(">", "%3E").Replace(@"\", "%5C").Replace("^", "%5E").Replace("_", "%5F").Replace("`", "%60").Replace("{", "%7B").Replace("|", "%7C").Replace("}", "%7D").Replace("~", "%7E");
                            sDeviceIdentifierURL = sDeviceIdentifierURL + sDeviceIdentifier;
                            XmlDocument DeviceDoc = new XmlDocument();
                            try
                            {
                                //For Bug Id :66981 
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                                //const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                                //const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                                //ServicePointManager.SecurityProtocol = Tls12;
                                DeviceDoc.Load(sDeviceIdentifierURL);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.InnerException);
                                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('280019');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                btnAdd.Enabled = false;
                                sError = true;
                            }
                            if (DeviceDoc != null)
                            {

                                if (DeviceDoc.GetElementsByTagName("deviceIdIssuingAgency") != null)
                                {
                                    txtIssuingAgency.Text = txtIssuingAgency.ToolTip = DeviceDoc.GetElementsByTagName("deviceIdIssuingAgency")[0].InnerText.TrimStart();

                                    //Get Serial no,Lot No,Manu and Exp date...
                                    Extract_Serial_LOT_DATES_From_Text(txtIssuingAgency.Text, sOriginalDeviceIdentifier);
                                }

                                if (DeviceDoc.GetElementsByTagName("brandName") != null)
                                {
                                    txtBrandName.Text = DeviceDoc.GetElementsByTagName("brandName")[0].InnerText.TrimStart();
                                    txtBrandName.ToolTip = txtBrandName.Text;
                                }

                                if (DeviceDoc.GetElementsByTagName("versionModelNumber") != null)
                                {
                                    txtVersionModel.Text = DeviceDoc.GetElementsByTagName("versionModelNumber")[0].InnerText.TrimStart();
                                    txtVersionModel.ToolTip = txtVersionModel.Text;
                                }

                                if (DeviceDoc.GetElementsByTagName("companyName") != null)
                                {
                                    txtCompanyName.Text = txtCompanyName.ToolTip = DeviceDoc.GetElementsByTagName("companyName")[0].InnerText.TrimStart();

                                }

                                if (DeviceDoc.GetElementsByTagName("MRISafetyStatus") != null)
                                {
                                    txtMRISafetyStatus.Text = DeviceDoc.GetElementsByTagName("MRISafetyStatus")[0].InnerText.TrimStart();
                                    txtMRISafetyStatus.ToolTip = txtMRISafetyStatus.Text;
                                }

                                if (DeviceDoc.GetElementsByTagName("gmdnPTName") != null)
                                {
                                    txtGMDNPTName.Text = DeviceDoc.GetElementsByTagName("gmdnPTName")[0].InnerText.TrimStart();
                                    txtGMDNPTName.ToolTip = txtGMDNPTName.Text;
                                }

                                if (DeviceDoc.GetElementsByTagName("deviceDescription") != null)
                                {
                                    txtDescription.Text = DeviceDoc.GetElementsByTagName("deviceDescription")[0].InnerText.TrimStart();
                                    txtDescription.ToolTip = txtDescription.Text;
                                }
                                if (DeviceDoc.GetElementsByTagName("gmdnPTDefinition") != null)
                                {
                                    Session["sGmdnPTDefinition"] = sGmdnPTDefinition = DeviceDoc.GetElementsByTagName("gmdnPTDefinition")[0].InnerText.TrimStart();

                                }
                                if (DeviceDoc.GetElementsByTagName("labeledContainsNRL") != null && DeviceDoc.GetElementsByTagName("labeledContainsNRL")[0].InnerText.ToUpper() == "TRUE")
                                {
                                    txtRubberContent.Text = "Yes";
                                    txtRubberContent.ToolTip = txtRubberContent.Text;
                                }
                                else if (DeviceDoc.GetElementsByTagName("labeledNoNRL") != null && DeviceDoc.GetElementsByTagName("labeledNoNRL")[0].InnerText.ToUpper() == "FALSE")
                                {
                                    txtRubberContent.Text = "No";
                                    txtRubberContent.ToolTip = txtRubberContent.Text;
                                }
                                else
                                {
                                    txtRubberContent.Text = string.Empty;
                                    txtRubberContent.ToolTip = txtRubberContent.Text;
                                }
                                if (txtProcedure.Text.Trim() != null && txtProcedure.Text.Trim() == string.Empty)
                                {
                                    txtProcedure.Text = "99999 - Implantable Devices";
                                }
                                Session["is_Save"] = true;
                                hdnBtnFind.Value = "true";
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EnterUDI", "DisplayErrorMessage('280020');{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                        btnAdd.Enabled = false;
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadDI", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                if (sError != true)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "EnterValidUDI", "DisplayErrorMessage('280019'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    btnAdd.Enabled = false;
                }
            }
        }


        string ExtractText(string sText, string sStart, string sEnd)
        {
            string sOutput = string.Empty;
            int iStart = sText.IndexOf(sStart);
            iStart = (iStart == -1) ? 0 : iStart + sStart.Length;
            int iEnd = sText.LastIndexOf(sEnd);
            if (iEnd == -1)
            {
                iEnd = sText.Length;
            }
            int len = iEnd - iStart;
            sOutput = sText.Substring(iStart, len);
            return sOutput;
        }

        void Extract_Serial_LOT_DATES_From_Text(string sAgency, string sDeviceID)
        {
            if (sAgency.ToUpper() == "GS1")
            {
                if (sDeviceID.Contains("(01)"))
                {
                    Session["Device_ID_from_UDI"] = sDevice_ID_from_UDI = SplitSpecialCharcter(sDeviceID, "(01)", 14);
                    if (sDevice_ID_from_UDI.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("(01)" + sDevice_ID_from_UDI, "");
                }
                if (sDeviceID.Contains("(11)"))
                {
                    string sManuDate = SplitSpecialCharcter(sDeviceID, "(11)", 6);
                    if (sManuDate.Trim() != string.Empty)
                    {
                        txtManufacturedDate.Text = txtManufacturedDate.ToolTip = DateTime.ParseExact(sManuDate, "yyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        sDeviceID = sDeviceID.Replace("(11)" + sManuDate, "");
                    }
                }
                if (sDeviceID.Contains("(17)"))
                {
                    string sExpDate = SplitSpecialCharcter(sDeviceID, "(17)", 6);
                    if (sExpDate.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDate, "yyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        sDeviceID = sDeviceID.Replace("(17)" + sExpDate, "");
                    }
                }
                if (sDeviceID.Contains("(10)"))
                {
                    txtLotorBatch.Text = txtLotorBatch.ToolTip = SplitSpecialCharcter(sDeviceID, "(10)", 20);
                    if (txtLotorBatch.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("(10)" + txtLotorBatch.Text, "");
                }
                if (sDeviceID.Contains("(21)"))
                {
                    txtSerialNumber.Text = txtSerialNumber.ToolTip = SplitSpecialCharcter(sDeviceID, "(21)", 20);
                    if (txtSerialNumber.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("(21)" + txtSerialNumber.Text, "");
                }
                if (sDeviceID.Contains("="))
                {
                    txtDistinctID.Text = txtDistinctID.ToolTip = SplitSpecialCharcter(sDeviceID, "=", 15);
                    if (txtDistinctID.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("=" + txtDistinctID.Text, "");
                }
            }
            else if (sAgency.ToUpper() == "ICCBBA")
            {
                if (sDeviceID.Contains("=/"))
                {
                    Session["Device_ID_from_UDI"] = sDevice_ID_from_UDI = SplitSpecialCharcter(sDeviceID, "=/", 16);
                    if (sDevice_ID_from_UDI.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("=" + sDevice_ID_from_UDI, "");
                }
                else
                {
                    //For Blood Bags
                    if (sDeviceID.Contains("=)"))
                    {
                        Session["Device_ID_from_UDI"] = sDevice_ID_from_UDI = SplitSpecialCharcter(sDeviceID, "=)", 10);
                        if (sDevice_ID_from_UDI.Trim() != string.Empty)
                            sDeviceID = sDeviceID.Replace("=)" + sDevice_ID_from_UDI, "");
                    }
                }
                if (sDeviceID.Contains("=>"))
                {
                    string sExpDate = SplitSpecialCharcter(sDeviceID, "=>", 6);
                    if (sExpDate.Trim() != string.Empty)
                    {
                        string sJulianYear = DateTime.ParseExact(sExpDate.Substring(1, 2), "yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        DateTime dt = Convert.ToDateTime(sJulianYear).AddDays(Convert.ToInt32(sExpDate.Substring(3, 3))).AddDays(-1);
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = dt.ToString("dd-MMM-yyyy");
                        sDeviceID = sDeviceID.Replace("=>" + sExpDate, "");
                    }
                }
                if (sDeviceID.Contains("=}"))
                {
                    string sManuDate = SplitSpecialCharcter(sDeviceID, "=}", 6);
                    if (sManuDate.Trim() != string.Empty)
                    {
                        string sJulianYear = DateTime.ParseExact(sManuDate.Substring(1, 2), "yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        DateTime dt = Convert.ToDateTime(sJulianYear).AddDays(Convert.ToInt32(sManuDate.Substring(3, 3))).AddDays(-1);
                        txtManufacturedDate.Text = txtManufacturedDate.ToolTip = dt.ToString("dd-MMM-yyyy");
                        sDeviceID = sDeviceID.Replace("=}" + sManuDate, "");
                    }
                }
                if (sDeviceID.Contains("=,"))
                {
                    txtSerialNumber.Text = txtSerialNumber.ToolTip = SplitSpecialCharcter(sDeviceID, "=,", 6);
                    if (txtSerialNumber.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("=," + txtSerialNumber.Text, "");
                }
                if (sDeviceID.Contains("&,1"))
                {
                    txtLotorBatch.Text = txtLotorBatch.ToolTip = SplitSpecialCharcter(sDeviceID, "&,1", 18);
                    if (txtLotorBatch.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("&,1" + txtLotorBatch.Text, "");
                }
                else
                {
                    //For Blood Bags
                    if (sDeviceID.Contains("&)"))
                    {
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = SplitSpecialCharcter(sDeviceID, "&)", 10);
                        if (txtLotorBatch.Text.Trim() != string.Empty)
                            sDeviceID = sDeviceID.Replace("&)" + txtLotorBatch.Text, "");
                    }
                }

                if (sDeviceID.Contains("="))
                {
                    txtDistinctID.Text = txtDistinctID.ToolTip = SplitSpecialCharcter(sDeviceID, "=", 15);
                    if (txtDistinctID.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("=" + txtDistinctID.Text, "");
                }

            }
            else if (sAgency.ToUpper() == "HIBCC")//No distinct Id
            {
                //Serial Number Expiration date in the format $$+
                if (sDeviceID.Contains("$$+7"))
                {
                    txtSerialNumber.Text = txtSerialNumber.ToolTip = SplitSpecialCharcter(sDeviceID, "$$+7", 18);
                    if (txtSerialNumber.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("$$+7" + txtSerialNumber.Text, "");
                }
                if (sDeviceID.Contains("$$+6"))
                {

                    string sExpDateSerialNo = SplitSpecialCharcter(sDeviceID, "$$+6", 7 + 18);
                    if (sExpDateSerialNo.Trim() != string.Empty)
                    {
                        string sJulianYear = DateTime.ParseExact(sExpDateSerialNo.Substring(0, 2), "yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy ");
                        DateTime dt = Convert.ToDateTime(sJulianYear).AddDays(Convert.ToInt32(sExpDateSerialNo.Substring(2, 3))).AddDays(-1);
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = dt.ToString("dd-MMM-yyyy ") + sExpDateSerialNo.Substring(5, 2) + ":00:00";
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = sExpDateSerialNo.Replace(sExpDateSerialNo.Substring(0, 7), "");
                        sDeviceID = sDeviceID.Replace("$$+6" + sExpDateSerialNo, "");
                    }

                }
                else if (sDeviceID.Contains("$$+5"))
                {
                    string sExpDateSerialNo = SplitSpecialCharcter(sDeviceID, "$$+5", 5 + 18);
                    // Convert.ToDateTime(sExpDateSerialNo.Substring(0, 5)).ToString("YYJJJ");
                    if (sExpDateSerialNo.Trim() != string.Empty)
                    {
                        string sJulianYear = DateTime.ParseExact(sExpDateSerialNo.Substring(0, 2), "yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        DateTime dt = Convert.ToDateTime(sJulianYear).AddDays(Convert.ToInt32(sExpDateSerialNo.Substring(2, 3))).AddDays(-1);
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = dt.ToString("dd-MMM-yyyy");
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = sExpDateSerialNo.Replace(sExpDateSerialNo.Substring(0, 5), "");
                        sDeviceID = sDeviceID.Replace("$$+5" + sExpDateSerialNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$+4"))
                {
                    string sExpDateSerialNo = SplitSpecialCharcter(sDeviceID, "$$+4", 8 + 18);
                    if (sExpDateSerialNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateSerialNo.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy ") + sExpDateSerialNo.Substring(6, 2) + ":00:00";
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = sExpDateSerialNo.Replace(sExpDateSerialNo.Substring(0, 8), "");
                        sDeviceID = sDeviceID.Replace("$$+4" + sExpDateSerialNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$+3"))
                {
                    string sExpDateSerialNo = SplitSpecialCharcter(sDeviceID, "$$+3", 6 + 18);
                    if (sExpDateSerialNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateSerialNo.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = sExpDateSerialNo.Replace(sExpDateSerialNo.Substring(0, 6), "");
                        sDeviceID = sDeviceID.Replace("$$+3" + sExpDateSerialNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$+2"))
                {
                    string sExpDateSerialNo = SplitSpecialCharcter(sDeviceID, "$$+2", 6 + 18);
                    if (sExpDateSerialNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateSerialNo.Substring(0, 6), "MMddyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = sExpDateSerialNo.Replace(sExpDateSerialNo.Substring(0, 6), "");
                        sDeviceID = sDeviceID.Replace("$$+2" + sExpDateSerialNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$+"))
                {
                    string sExpDateSerialNo = SplitSpecialCharcter(sDeviceID, "$$+", 4 + 18);
                    if (sExpDateSerialNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateSerialNo.Substring(0, 4), "MMyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = sExpDateSerialNo.Replace(sExpDateSerialNo.Substring(0, 4), "");
                        sDeviceID = sDeviceID.Replace("$$+" + sExpDateSerialNo, "");
                    }
                }
                else if (sDeviceID.Contains("$+"))
                {
                    txtSerialNumber.Text = txtSerialNumber.ToolTip = SplitSpecialCharcter(sDeviceID, "$+", 18);
                    if (txtSerialNumber.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("$+" + txtSerialNumber.Text, "");
                }
                else if (sDeviceID.Contains("/S"))
                {
                    txtSerialNumber.Text = txtSerialNumber.ToolTip = SplitSpecialCharcter(sDeviceID, "/S", 18);
                    if (txtSerialNumber.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("/S" + txtSerialNumber.Text, "");
                }
                else
                {
                    if (sDeviceID.Contains("/$"))
                    {
                        txtSerialNumber.Text = txtSerialNumber.ToolTip = SplitSpecialCharcter(sDeviceID, "/$", 18);
                        if (txtSerialNumber.Text.Trim() != string.Empty)
                            sDeviceID = sDeviceID.Replace("/$" + txtSerialNumber.Text, "");
                    }
                }
                //Lot Number Expiration date in the format $$
                if (sDeviceID.Contains("$$7"))
                {
                    txtLotorBatch.Text = txtLotorBatch.ToolTip = SplitSpecialCharcter(sDeviceID, "$$7", 18);
                    if (txtLotorBatch.Text.Trim() != string.Empty)
                        sDeviceID = sDeviceID.Replace("$$7" + txtLotorBatch.Text, "");
                }
                if (sDeviceID.Contains("$$6"))
                {
                    string sExpDateLotlNo = SplitSpecialCharcter(sDeviceID, "$$6", 7 + 18);
                    if (sExpDateLotlNo.Trim() != string.Empty)
                    {
                        string sJulianYear = DateTime.ParseExact(sExpDateLotlNo.Substring(0, 2), "yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy ");
                        DateTime dt = Convert.ToDateTime(sJulianYear).AddDays(Convert.ToInt32(sExpDateLotlNo.Substring(2, 3))).AddDays(-1);
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = dt.ToString("dd-MMM-yyyy ") + sExpDateLotlNo.Substring(5, 2) + ":00:00";
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = sExpDateLotlNo.Replace(sExpDateLotlNo.Substring(0, 7), "");
                        sDeviceID = sDeviceID.Replace("$$6" + sExpDateLotlNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$5"))
                {
                    string sExpDateLotlNo = SplitSpecialCharcter(sDeviceID, "$$5", 5 + 18);
                    if (sExpDateLotlNo.Trim() != string.Empty)
                    {
                        string sJulianYear = DateTime.ParseExact(sExpDateLotlNo.Substring(0, 2), "yy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        DateTime dt = Convert.ToDateTime(sJulianYear).AddDays(Convert.ToInt32(sExpDateLotlNo.Substring(2, 3))).AddDays(-1);
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = dt.ToString("dd-MMM-yyyy");
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = sExpDateLotlNo.Replace(sExpDateLotlNo.Substring(0, 5), "");
                        sDeviceID = sDeviceID.Replace("$$5" + sExpDateLotlNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$4"))
                {
                    string sExpDateLotlNo = SplitSpecialCharcter(sDeviceID, "$$4", 8 + 18);
                    if (sExpDateLotlNo.Trim() != string.Empty)
                    {
                        string sExpDate = DateTime.ParseExact(sExpDateLotlNo.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy ");
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = sExpDate + sExpDateLotlNo.Substring(6, 2) + ":00:00";
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = sExpDateLotlNo.Replace(sExpDateLotlNo.Substring(0, 8), "");
                        sDeviceID = sDeviceID.Replace("$$4" + sExpDateLotlNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$3"))
                {
                    string sExpDateLotlNo = SplitSpecialCharcter(sDeviceID, "$$3", 6 + 18);
                    if (sExpDateLotlNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateLotlNo.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = sExpDateLotlNo.Replace(sExpDateLotlNo.Substring(0, 6), "");
                        sDeviceID = sDeviceID.Replace("$$3" + sExpDateLotlNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$2"))
                {
                    string sExpDateLotlNo = SplitSpecialCharcter(sDeviceID, "$$2", 6 + 18);
                    if (sExpDateLotlNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateLotlNo.Substring(0, 6), "MMddyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = sExpDateLotlNo.Replace(sExpDateLotlNo.Substring(0, 6), "");
                        sDeviceID = sDeviceID.Replace("$$2" + sExpDateLotlNo, "");
                    }
                }
                else if (sDeviceID.Contains("$$"))
                {
                    string sExpDateLotlNo = SplitSpecialCharcter(sDeviceID, "$$", 4 + 18);
                    if (sExpDateLotlNo.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDateLotlNo.Substring(0, 4), "MMyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = sExpDateLotlNo.Replace(sExpDateLotlNo.Substring(0, 4), "");
                        sDeviceID = sDeviceID.Replace("$$" + sExpDateLotlNo, "");
                    }
                }
                else
                {
                    if (sDeviceID.Contains("$"))
                    {
                        txtLotorBatch.Text = txtLotorBatch.ToolTip = SplitSpecialCharcter(sDeviceID, "$", 18);
                        sDeviceID = sDeviceID.Replace("$" + txtLotorBatch.Text, "");
                    }
                }

                if (sDeviceID.Contains("/16D"))
                {
                    string sManuDate = SplitSpecialCharcter(sDeviceID, "/16D", 8);
                    if (sManuDate.Trim() != string.Empty)
                    {
                        txtManufacturedDate.Text = txtManufacturedDate.ToolTip = DateTime.ParseExact(sManuDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        sDeviceID = sDeviceID.Replace("/16D" + sManuDate, "");
                    }
                }

                if (sDeviceID.Contains("/14D"))
                {
                    string sExpDate = SplitSpecialCharcter(sDeviceID, "/14D", 8);
                    if (sExpDate.Trim() != string.Empty)
                    {
                        txtExpirationDate.Text = txtExpirationDate.ToolTip = DateTime.ParseExact(sExpDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");

                        sDeviceID = sDeviceID.Replace("/14D" + sExpDate, "");
                    }
                }

                if (sDeviceID.Contains("+"))
                {
                    sDeviceID = sDeviceID.Split('+')[1];
                    var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                    if (regexItem.IsMatch(sDeviceID))
                    {
                        Session["Device_ID_from_UDI"] = sDevice_ID_from_UDI = sDeviceID;
                    }
                    else
                    {
                        Session["Device_ID_from_UDI"] = sDevice_ID_from_UDI = FindSpecialCharcter(sDeviceID);
                    }
                }
            }
            else
            {
                //For other agencies we need the structure to calculate value
                txtSerialNumber.Text = txtSerialNumber.ToolTip = string.Empty;
                txtLotorBatch.Text = txtLotorBatch.ToolTip = string.Empty;
                txtManufacturedDate.Text = txtManufacturedDate.ToolTip = string.Empty;
                txtExpirationDate.Text = txtExpirationDate.ToolTip = string.Empty;
                txtDistinctID.Text = txtDistinctID.ToolTip = string.Empty;
            }
        }

        string SplitSpecialCharcter(string sSplitText, string sSplitChar, int iCount)
        {
            string sOutput = string.Empty;

            string[] sArry = sSplitText.Split(new string[] { sSplitChar }, StringSplitOptions.RemoveEmptyEntries);
            string sText = string.Empty;
            if (sArry.Count() == 1)
            {
                sText = sArry[0];
            }
            else
            {
                sText = sArry[1];
            }
            if (sText.Trim() != null && sText.Trim() != string.Empty)
            {
                if (sText.Length >= iCount)
                {
                    sOutput = sText.Substring(0, iCount);
                }
                else
                {
                    sOutput = FindSpecialCharcter(sText);
                }
            }
            return sOutput;
        }

        string FindSpecialCharcter(string text)
        {
            string sOutput = string.Empty;
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            foreach (char c in text.ToCharArray())
            {
                if (regexItem.IsMatch(c.ToString()))
                {
                    sOutput += c;
                }
                else
                {
                    break;
                }
            }
            return sOutput;
        }

        IList<InHouseProcedure> GetFromXML(ulong EncounterID)
        {

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //var _IsAvailable = File.Exists(strXmlFilePath);



            //if (_IsAvailable)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();
            //    if (itemDoc.GetElementsByTagName("InHouseProcedureList")[0] != null)
            //    {
            //        xmlTagName = itemDoc.GetElementsByTagName("InHouseProcedureList")[0].ChildNodes;
            //        IList<InHouseProcedure> ilst = new List<InHouseProcedure>();
            //        if (xmlTagName.Count > 0)
            //        {
            //            for (int j = 0; j < xmlTagName.Count; j++)
            //            {
            //                XmlSerializer xmlserializer = new XmlSerializer(typeof(InHouseProcedure));
            //                InHouseProcedure objInHouseProcedure = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as InHouseProcedure;

            //                IEnumerable<PropertyInfo> propInfo = null;

            //                propInfo = from obji in ((InHouseProcedure)objInHouseProcedure).GetType().GetProperties() select obji;
            //                if (xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value == Convert.ToString(EncounterID))
            //                {
            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(objInHouseProcedure, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(objInHouseProcedure, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(objInHouseProcedure, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(objInHouseProcedure, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(objInHouseProcedure, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    ilst.Add(objInHouseProcedure);
            //                }
            //            }
            //        }
            //        ilstInHouseProcedure = ilst.OrderByDescending(a => a.Modified_Date_And_Time).ToList();
            //    }
            //}
            IList<InHouseProcedure> ilstInHouseProcedure = new List<InHouseProcedure>();
            IList<InHouseProcedure> ilst = new List<InHouseProcedure>();

            IList<string> ilstInHouseProcedureTagList = new List<string>();
            ilstInHouseProcedureTagList.Add("InHouseProcedureList");

            IList<object> ilstInHouseProcedureBlobFinal = new List<object>();
            ilstInHouseProcedureBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstInHouseProcedureTagList);

            if (ilstInHouseProcedureBlobFinal != null && ilstInHouseProcedureBlobFinal.Count > 0)
            {
                if (ilstInHouseProcedureBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstInHouseProcedureBlobFinal[0]).Count; iCount++)
                    {
                        ilst.Add((InHouseProcedure)((IList<object>)ilstInHouseProcedureBlobFinal[0])[iCount]);
                    }
                    ilstInHouseProcedure = ilst.OrderByDescending(a => a.Modified_Date_And_Time).ToList();
                }
            }

            
          return ilstInHouseProcedure;
        }
    }
}