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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Acurus.Capella.UI
{
    public partial class frmImmunization : System.Web.UI.Page
    {
        #region Declaration
        ImmunizationDTO objImmunizationFill;
        ImmunizationManager objImmuMgr = new ImmunizationManager();
        Immunization objImmunization;
        ImmunizationHistory objImmunizationHistory;
        IList<Immunization> ImmunListBasedOnEncID = null;
        IList<Immunization> Immunizationlist = null;
        public IList<Immunization> lstUpdate = new List<Immunization>();
        //IList<FileManagementIndex> lstFile;
        IList<Immunization> ImmunizationList = new List<Immunization>();
        IList<ImmunizationHistory> ImmunizationHistoryList = new List<ImmunizationHistory>();
        ArrayList tempCheckedListItems = new ArrayList();

        int age;

        public ulong ulMyGroupID;
        public ulong ulUpdateDelId;
        public ulong ulMyHumanID;
        public ulong ulMyPhysicianID;

        bool bEdit = false;

        string CVXDesc = string.Empty;
        string manufacturer = string.Empty;
        string mvx_code = string.Empty;
        string AdminUnit = string.Empty;
        string AdminUnitIdentifier = string.Empty;
        string Process = string.Empty;
        string sPlanText = string.Empty;
        //Added by Saravanan for Influeza macra
        IList<string> RefusedCPTS = new List<string>();
        public ulong HumanID
        {
            get
            {
                return ViewState["HumanID"] == null ? 0 : Convert.ToUInt32(ViewState["HumanID"]);
            }
            set
            {
                ViewState["HumanID"] = value;
            }
        }
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
        public ulong PhysicianID
        {
            get
            {
                return ViewState["PhysicianID"] == null ? 0 : Convert.ToUInt32(ViewState["PhysicianID"]);
            }
            set
            {
                ViewState["PhysicianID"] = value;
            }
        }
        public string ScreenMode
        {
            get
            {
                return ViewState["ScreenMode"] == null ? string.Empty : Convert.ToString(ViewState["ScreenMode"]);
            }
            set
            {
                ViewState["ScreenMode"] = value;
            }
        }
        #endregion

        #region Mehods

        private void FillImmunizationProcedure(IList<PhysicianProcedure> immProList)
        {
            string CheckedItem = string.Empty;
            if (chklstImmunizationProcedures.CheckedItems != null && chklstImmunizationProcedures.CheckedItems.Count > 0)
                CheckedItem = chklstImmunizationProcedures.CheckedItems[0].Value.ToString();
            chklstImmunizationProcedures.Items.Clear();
            IList<PhysicianProcedure> listProcedure = immProList;
            //Checkin for null value added by Janani on 2/8/10
            if (listProcedure != null)
            {
                if (listProcedure.Count > 0)
                {
                    for (int i = 0; i < listProcedure.Count; i++)
                    {
                        if (CheckedItem.Equals((listProcedure[i].Physician_Procedure_Code + "-" + listProcedure[i].Procedure_Description).ToString()))
                        {
                            chklstImmunizationProcedures.Items.Add(new RadListBoxItem(listProcedure[i].Physician_Procedure_Code + "-" + listProcedure[i].Procedure_Description));
                            chklstImmunizationProcedures.Items[i].Checked = true;
                        }
                        else
                            chklstImmunizationProcedures.Items.Add(new RadListBoxItem(listProcedure[i].Physician_Procedure_Code + "-" + listProcedure[i].Procedure_Description));
                    }
                }
            }
        }

        private void GridFill()
        {
            /***************************************************************************************************************************************
            DESCRIPTION: This method will be called when click the button add it will add the saved values into the gridview grdImmunizations.
            ***************************************************************************************************************************************/
            IList<Immunization> ImmunizationTabList = new List<Immunization>();
            IList<Immunization> ImmunizationMenuList = new List<Immunization>();
            IList<ImmunizationHistory> ImmHislst = new List<ImmunizationHistory>();
            ImmunizationDTO ImmunizationDTO = new ImmunizationDTO();

            IList<string> ilstImmunizationTag = new List<string>();
            ilstImmunizationTag.Add("ImmunizationList");
            ilstImmunizationTag.Add("ImmunizationHistoryList");

            IList<object> ilstImmnBlobList = new List<object>();
            ilstImmnBlobList = UtilityManager.ReadBlob(HumanID, ilstImmunizationTag);

            if (ilstImmnBlobList != null && ilstImmnBlobList.Count > 0)
            {
                if (ilstImmnBlobList[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstImmnBlobList[0]).Count; iCount++)
                    {

                        if (((Immunization)((IList<object>)ilstImmnBlobList[0])[iCount]).Encounter_Id == 0)
                        {
                            ImmunizationMenuList.Add((Immunization)((IList<object>)ilstImmnBlobList[0])[iCount]);
                        }
                        else
                        {
                            if (((Immunization)((IList<object>)ilstImmnBlobList[0])[iCount]).Encounter_Id == ClientSession.EncounterId || (hdnEncID.Value.Trim() != "" && ((Immunization)((IList<object>)ilstImmnBlobList[0])[iCount]).Encounter_Id == Convert.ToUInt64(hdnEncID.Value)))//Changed for BugID:47181
                            {
                                ImmunizationTabList.Add((Immunization)((IList<object>)ilstImmnBlobList[0])[iCount]);
                            }
                        }

                    }

                }

                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                {
                    age = Convert.ToInt32(UtilityManager.CalculateAge(Convert.ToDateTime(ClientSession.PatientPaneList[0].Birth_Date)));

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
                }

                if (ilstImmnBlobList[1] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstImmnBlobList[1]).Count; iCount++)
                    {
                        ImmHislst.Add((ImmunizationHistory)((IList<object>)ilstImmnBlobList[1])[iCount]);
                    }
                }

            }


            //string FileName = "Human" + "_" + HumanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    //  itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();

            //        if (itemDoc.GetElementsByTagName("ImmunizationList") != null)
            //        {
            //            if (itemDoc.GetElementsByTagName("ImmunizationList").Count > 0)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("ImmunizationList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(Immunization));
            //                        Immunization Immunization = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Immunization;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        if (Immunization != null)
            //                        {
            //                            propInfo = from obji in ((Immunization)Immunization).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(Immunization, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(Immunization, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(Immunization, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(Immunization, Convert.ToInt32(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                property.SetValue(Immunization, Convert.ToDecimal(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(Immunization, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            if (Immunization.Encounter_Id == 0)
            //                            {
            //                                ImmunizationMenuList.Add(Immunization);
            //                            }
            //                            else
            //                            {
            //                                if (Immunization.Encounter_Id == ClientSession.EncounterId || (hdnEncID.Value.Trim() != "" && Immunization.Encounter_Id == Convert.ToUInt64(hdnEncID.Value)))//Changed for BugID:47181
            //                                {
            //                                    ImmunizationTabList.Add(Immunization);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        if (itemDoc.GetElementsByTagName("Age") != null)
            //        {
            //            if (itemDoc.GetElementsByTagName("Age").Count > 0)
            //            {
            //                string sAge = itemDoc.GetElementsByTagName("Age")[0].Attributes[0].Value;
            //                if (sAge != "")
            //                {
            //                    age = Convert.ToInt32(sAge);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Age Tag Missing", "DisplayErrorMessage('000039');", true);//Throw error when Age is missing
            //        }
            //        if (itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationHistory));
            //                    ImmunizationHistory ImmunizationHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ImmunizationHistory;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((ImmunizationHistory)ImmunizationHistory).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(ImmunizationHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(ImmunizationHistory, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(ImmunizationHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(ImmunizationHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                        property.SetValue(ImmunizationHistory, Convert.ToDecimal(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(ImmunizationHistory, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    ImmHislst.Add(ImmunizationHistory);
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}

            grdImmunizations.DataSource = null;
            grdImmunizations.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add("Edit", typeof(Bitmap));
            dt.Columns.Add("Del", typeof(Bitmap));
            dt.Columns.Add(new DataColumn("Immn./Inj. Procedure", typeof(string)));
            dt.Columns.Add(new DataColumn("Administered Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Dose", typeof(string)));
            dt.Columns.Add(new DataColumn("Route of Administration", typeof(string)));
            dt.Columns.Add(new DataColumn("Immn./Inj. Source", typeof(string)));
            dt.Columns.Add(new DataColumn("Id", typeof(string)));
            dt.Columns.Add(new DataColumn("NDC", typeof(string)));
            dt.Columns.Add(new DataColumn("Notes", typeof(string)));
            dt.Columns.Add("View", typeof(Bitmap));

            if ((Request["ScreenMode"] != null && Request["ScreenMode"].ToString().ToUpper() == "MENU"))
            {
                if (ImmunizationMenuList != null)
                {
                    for (int i = 0; i < ImmunizationMenuList.Count; i++)
                    {
                        ImmunizationList.Add(ImmunizationMenuList[i]);
                    }
                }
            }
            else if (Request["Screen"] != null && Request["Screen"].ToString() == "MyQ")
            {
                if (ImmunizationMenuList != null)
                {
                    for (int i = 0; i < ImmunizationMenuList.Count; i++)
                    {
                        ImmunizationList.Add(ImmunizationMenuList[i]);
                    }
                }
                if (ImmunizationTabList != null)
                {
                    for (int i = 0; i < ImmunizationTabList.Count; i++)
                    {
                        ImmunizationList.Add(ImmunizationTabList[i]);
                    }
                }
            }
            else
            {
                if (ImmunizationTabList != null)
                {
                    for (int i = 0; i < ImmunizationTabList.Count; i++)
                    {
                        ImmunizationList.Add(ImmunizationTabList[i]);
                    }
                }
            }


            if (ImmunizationList != null && ImmunizationList.Count > 0)
            {
                for (int i = 0; i < ImmunizationList.Count; i++)
                {
                    if (ImmunizationList[i].Is_Deleted != "Y")
                    {

                        if (Request["Screen"] != null && Request["Screen"].ToString() == "MyQ")//Changed for BugID:47181
                            goto lbl_a;
                        if (hdnGroupID.Value != "" && (Convert.ToUInt32(hdnGroupID.Value) != 0 && ImmunizationList[i].Immunization_Group_ID != Convert.ToUInt32(hdnGroupID.Value)))//ulMyGroupID != 0 && ImmunizationList[i].Immunization_Group_ID != ulMyGroupID)
                        {
                            continue;
                        }
                    lbl_a:
                        dr = dt.NewRow();
                        dr["Immn./Inj. Procedure"] = ImmunizationList[i].Procedure_Code + "-" + ImmunizationList[i].Immunization_Description;
                        if (ImmunizationList[i].Given_Date.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                        {
                            dr["Administered Date"] = ImmunizationList[i].Given_Date.ToString("dd-MMM-yyyy");
                        }
                        if (ImmunizationList[i].Dose != 0)
                        {
                            dr["Dose"] = ImmunizationList[i].Dose;
                        }
                        dr["Route of Administration"] = ImmunizationList[i].Route_of_Administration;
                        dr["Immn./Inj. Source"] = ImmunizationList[i].Immunization_Source;
                        dr["Id"] = ImmunizationList[i].Id;
                        dr["NDC"] = ImmunizationList[i].NDC;
                        dr["Notes"] = ImmunizationList[i].Notes;
                        dt.Rows.Add(dr);

                    }
                }
            }
            grdImmunizations.DataSource = dt;
            grdImmunizations.DataBind();

            if (ClientSession.UserCurrentProcess == "MA_REVIEW")
            {
                grdImmunizations.Columns[1].Visible = false;
            }

            if (ImmHislst != null && ImmHislst.Count > 0)
            {
                ImmunizationHistoryList = ImmHislst.Where(ih => ImmunizationList.Any(i => i.Id == ih.Immunization_Order_ID)).ToList<ImmunizationHistory>();

            }

            //Commented By Suvarnni For Code Review Bug on 19.01.2016
            //foreach (GridDataItem item in grdImmunizations.Items)
            //{
            //    IList<Immunization> lstImage = ImmunizationList.Where(a => a.Id == Convert.ToUInt64(item["Id"].Text)).ToList<Immunization>();
            //    if (lstImage.Count > 0 && lstImage[0].File_Management_Index_Id == string.Empty)
            //    {
            //        TableCell selectCell = item["View"];
            //        ImageButton gd = (ImageButton)selectCell.Controls[0];
            //        gd.ImageUrl = "~/Resources/Down_Disabled.bmp";
            //    }
            //}
        }

        void chklstImmunizationCheckItem(int itemindex)
        {
            ModalWindow.VisibleOnPageLoad = false;
            ModalWindow.Visible = false;

            RadWindowPlan.VisibleOnPageLoad = false;
            RadWindowPlan.Visible = false;

            RadWindow2.VisibleOnPageLoad = false;
            RadWindow2.Visible = false;

            RadWindow1.VisibleOnPageLoad = false;
            RadWindow1.Visible = false;

            for (int i = 0; i < chklstImmunizationProcedures.Items.Count; i++)
            {
                if (itemindex != i)
                {
                    chklstImmunizationProcedures.Items[i].Checked = false;
                    chklstImmunizationProcedures.Items[i].Selected = false;
                }
            }
            if (chklstImmunizationProcedures.CheckedItems.Count > 0)
            {
                txtImmunizationProcedure.Text = chklstImmunizationProcedures.CheckedItems[0].Value.ToString();
            }
            else
            {
                txtImmunizationProcedure.Text = string.Empty;
                txtCVXCode.Text = string.Empty;
            }

            //To get CVX code for selected CPT
            if (txtImmunizationProcedure.Text != string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "EnableSaveDiagnosticOrder('true');", true);
                btnAdd.Enabled = true;
                btnTestArea.Enabled = true;
                string[] split = txtImmunizationProcedure.Text.Split('-');
                if (split.Length > 0)
                {
                    ProcedureCodeLibraryManager objProCodeMngr = new ProcedureCodeLibraryManager();
                    IList<ProcedureCodeLibrary> proList = objProCodeMngr.SearchProcedureCodeBasedOnCPT(split[0]);
                    if (proList != null)
                    {
                        if (proList.Count > 0)
                        {
                            txtCVXCode.Text = proList[0].CVX_Code;
                            CVXDesc = proList[0].CVX_Code_Description;
                        }
                    }
                }
            }
        }

        private void InsertOrUpdate()
        {
            if (txtImmunizationProcedure.Text != string.Empty)
            {
                string[] split = txtImmunizationProcedure.Text.Split('-');
                if (split.Length > 0)
                {
                    objImmunization.Procedure_Code = split[0];
                    objImmunizationHistory.Procedure_Code = split[0];
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (i == 1)
                        {
                            objImmunization.Immunization_Description = split[i];
                            objImmunizationHistory.Immunization_Description = split[i];
                        }
                        else
                        {
                            objImmunization.Immunization_Description += "-" + split[i];
                            objImmunizationHistory.Immunization_Description += "-" + split[i];
                        }
                    }
                }
            }

            if (dtpVisitDate.SelectedDate.ToString() != "")
                objImmunization.Visit_Date = dtpVisitDate.SelectedDate.Value;

            objImmunization.CVX_Code = txtCVXCode.Text;

            if (chkVisgiven.Checked)
            {
                objImmunization.Is_VIS_Given = "Y";
                objImmunizationHistory.Is_VIS_Given = "Y";
                if (dtpDateOnVis.SelectedDate != null && dtpDateOnVis.SelectedDate.ToString() != " ")
                {
                    objImmunization.Date_on_Vis = dtpDateOnVis.SelectedDate.Value;
                }
                else
                {
                    objImmunization.Date_on_Vis = DateTime.MinValue;
                }
                objImmunization.VIS_Given_Date = dtpVisGiven.SelectedDate.Value;
            }
            else
            {
                objImmunization.Is_VIS_Given = "N";
                objImmunizationHistory.Is_VIS_Given = "N";
            }
            if (chkAuthorizationRequired.Checked)
            {
                objImmunization.Authorization_Required = "Y";
            }
            else
            {
                objImmunization.Authorization_Required = "N";
            }


            objImmunization.CVX_Code_Description = CVXDesc;
            if (chkYes.Checked)
            {
                objImmunization.Lot_Number = txtLotNumber.Text;
                objImmunization.NDC = txtNDC.Text;
                objImmunization.Manufacturer = cboManufacturer.Text;
                objImmunization.MVX_Code = mvx_code;

                if (txtAdminAmt.Text != string.Empty)
                    objImmunization.Administered_Amount = Convert.ToDecimal(txtAdminAmt.Text);
                else
                    objImmunization.Administered_Amount = Convert.ToDecimal("0");
                objImmunization.Administered_Unit = cboAdminUnit.Text;

                if (cboAdminUnit.Items.FindItemByText(cboAdminUnit.Text) != null)
                {
                    objImmunization.Administered_Unit_Identifier = cboAdminUnit.Items.FindItemByText(cboAdminUnit.Text).Value;
                }
                objImmunization.Location = cboLocation.Text;
                if (dtpExpiryDate.SelectedDate != null)
                {
                    objImmunization.Expiry_Date = dtpExpiryDate.SelectedDate.Value;
                }
                else
                {
                    objImmunization.Expiry_Date = DateTime.MinValue;
                }
                if (txtDose.Text != string.Empty)
                {
                    objImmunization.Dose = Convert.ToUInt64(txtDose.Text);
                    objImmunizationHistory.Dose = Convert.ToUInt64(txtDose.Text);
                }
                else
                {
                    objImmunization.Dose = 0;
                    objImmunizationHistory.Dose = 0;
                }
                if (cboDoseno.Text != string.Empty)
                {
                    objImmunization.Dose_No = Convert.ToUInt64(cboDoseno.Text);
                    objImmunizationHistory.Dose_No = Convert.ToUInt64(cboDoseno.Text);
                }
                else
                {
                    objImmunization.Dose_No = 0;
                    objImmunizationHistory.Dose_No = 0;
                }
                objImmunization.Given_By = txtGivenBy.Text;
                if (dtpGivenDate.SelectedDate != null)
                {
                    objImmunization.Given_Date = dtpGivenDate.SelectedDate.Value;
                }
                else
                {
                    objImmunization.Given_Date = DateTime.MinValue;
                }
                objImmunization.Immunization_Source = cboImmunizationSource.Text;
            }
            else
            {
                objImmunization.Lot_Number = string.Empty;
                objImmunization.Manufacturer = string.Empty;
                objImmunization.MVX_Code = string.Empty;
                objImmunization.Administered_Amount = 0m;
                objImmunization.Administered_Unit = string.Empty;
                objImmunization.Administered_Unit_Identifier = string.Empty;
                objImmunization.Location = string.Empty;
                objImmunization.Expiry_Date = DateTime.MinValue;
                objImmunization.Dose = 0;
                objImmunizationHistory.Dose = 0;
                objImmunization.Dose_No = 0;
                objImmunizationHistory.Dose_No = 0;
                objImmunization.Given_By = string.Empty;
                objImmunization.Given_Date = DateTime.MinValue;
                objImmunization.Immunization_Source = string.Empty;
                objImmunization.NDC = string.Empty;
            }
            objImmunization.Route_of_Administration = cboRouteOfAdministration.Text;
            objImmunization.Notes = txtNotes.txtDLC.Text;
            objImmunization.Document_Type = txtDLCDocumentType.txtDLC.Text;
            objImmunization.Vfc = cboVFC.Text;
            objImmunization.Immunization_Evidence = cboImmunizationEve.Text;

            //Certificate
            if (chkRefused.Checked)
                objImmunization.Is_Administration_Refused = "Y";
            else
                objImmunization.Is_Administration_Refused = "N";

            objImmunization.Refused_Administration = cboRefused.Text;
            if (cboRefused.SelectedItem != null)
                objImmunization.Snomed_Code = cboRefused.SelectedItem.Value.Split('|')[0];
            else
                objImmunization.Snomed_Code = string.Empty;
            objImmunization.Eligibility_Captured = cboEligibility.Text;
            objImmunization.Immunization_Information_Source = cboInformationSource.Text;
            objImmunization.Observation = cboObservation.Text;
            //end

            objImmunization.Facility_Name = ClientSession.FacilityName;
            if (chkYes.Checked == true)
            {
                objImmunization.Vaccine_In_House = "Y";
            }
            else if (chkNo.Checked == true)
            {
                objImmunization.Vaccine_In_House = "N";
            }
            if (objImmunizationHistory != null)
            {
                if (dtpGivenDate.SelectedDate != null)
                    objImmunizationHistory.Administered_Date = dtpGivenDate.SelectedDate.Value.ToString("dd-MMM-yyyy");
                objImmunizationHistory.CVX_Code = txtCVXCode.Text;
                objImmunizationHistory.Date_On_Vis = objImmunization.Date_on_Vis;
                objImmunizationHistory.Expiry_Date = objImmunization.Expiry_Date;
                objImmunizationHistory.Encounter_ID = EncounterID;
                objImmunizationHistory.Human_ID = HumanID;
                objImmunizationHistory.Immunization_Source = objImmunization.Immunization_Source;
                objImmunizationHistory.Location = objImmunization.Location;
                objImmunizationHistory.Lot_Number = objImmunization.Lot_Number;
                objImmunizationHistory.Manufacturer = objImmunization.Manufacturer;
                objImmunizationHistory.Notes = objImmunization.Notes;
                objImmunizationHistory.Physician_ID = PhysicianID;
                objImmunizationHistory.Route_Of_Administration = objImmunization.Route_of_Administration;
                objImmunizationHistory.Vis_Given_Date = objImmunization.VIS_Given_Date;
                objImmunizationHistory.Date_On_Vis = objImmunization.Date_on_Vis;
                objImmunizationHistory.Immunization_Order_ID = objImmunization.Id;
            }

        }

        public void ClearAll()
        {
            lblAllergy.Text = string.Empty;
            chklstImmunizationProcedures.ClearChecked();
            //chkVisgiven.Checked = false;
            //dtpVisGiven.Enabled = false;
            //dtpDateOnVis.Enabled = false;
            txtCVXCode.Text = string.Empty;
            txtDose.Text = string.Empty;
            txtGivenBy.Text = string.Empty;
            txtImmunizationProcedure.Text = string.Empty;
            txtLotNumber.Text = string.Empty;
            cboManufacturer.ClearSelection();
            cboManufacturer.CloseDropDownOnBlur = true;
            dtpDateOnVis.Clear();
            dtpExpiryDate.Clear();
            dtpGivenDate.Clear();
            dtpVisGiven.Clear();
            dtpVisitDate.Clear();
            if (hdnLocalTime.Value.Trim() != string.Empty)
            {
                dtpVisGiven.MaxDate = Convert.ToDateTime(hdnLocalTime.Value);
                dtpDateOnVis.MaxDate = Convert.ToDateTime(hdnLocalTime.Value);
                try
                {
                    dtpVisitDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpVisGiven.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpDateOnVis.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                }
                catch
                {
                    dtpVisitDate.MinDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpVisitDate.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpVisGiven.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                    dtpDateOnVis.SelectedDate = Convert.ToDateTime(hdnLocalTime.Value);
                }
            }
            cboLocation.ClearSelection();
            cboRouteOfAdministration.ClearSelection();
            cboVFC.ClearSelection();
            cboDoseno.ClearSelection();
            txtNotes.txtDLC.Text = string.Empty;
            txtDLCDocumentType.txtDLC.Text = string.Empty;
            cboImmunizationSource.ClearSelection();
            chkAuthorizationRequired.Checked = false;
            this.btnAdd.Text = "Add";
            this.btnClearAll.Text = "Clear All";
            btnAdd.AccessKey = "a";
            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            text1.InnerText = "A";
            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
            text2.InnerText = "dd";
            System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
            text3.InnerText = "C";
            System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
            text4.InnerText = "lear All";
            txtAdminAmt.Text = string.Empty;
            cboAdminUnit.ClearSelection();
            txtNDC.Text = string.Empty;
            chkNo.Checked = false;
            if (bEdit == true)
            {
                chkYes.Checked = false;
            }
            else
            {
                chkYes.Checked = true;
                bEdit = false;
            }
            cboRefused.ClearSelection();
            chkRefused.Checked = false;
            cboEligibility.ClearSelection();
            cboInformationSource.ClearSelection();
            cboObservation.ClearSelection();
            cboImmunizationEve.ClearSelection();

            if (Process.ToUpper() == "MA_REVIEW")
            {
                pnlOrderDetails.Enabled = false;
                pnlVaccineAdminDetails.Enabled = false;
                chklstImmunizationProcedures.Enabled = false;
                txtNotes.Enable = false;
                txtNotes.pbDropdown.Disabled = true;
            }
            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord != null && ClientSession.FillEncounterandWFObject.EncRecord.Id != 0)
                {
                    //dtpGivenDate.MinDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.Date;
                    //dtpExpiryDate.MinDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.Date;
                    //dtpVisitDate.MinDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.Date;
                    dtpGivenDate.SelectedDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;//Administered Date always set to Encounter DOS and is ReadOnly.
                    dtpGivenDate.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                    dtpExpiryDate.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                    dtpVisitDate.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;

                    //for git Id 1433
                    dtpVisGiven.SelectedDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;//Administered Date always set to Encounter DOS and is ReadOnly.
                    dtpVisGiven.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                    dtpVisGiven.MaxDate = DateTime.Now;

                    dtpVisitDate.SelectedDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                    dtpVisitDate.MaxDate = DateTime.Now;

                    dtpGivenDate.MaxDate = DateTime.Now;
                }
                else
                {
                    dtpGivenDate.SelectedDate = DateTime.Now;
                    dtpGivenDate.MinDate = DateTime.MinValue;
                    dtpExpiryDate.MinDate = DateTime.MinValue;
                    dtpVisitDate.MinDate = DateTime.MinValue;

                    //for git Id 1433
                    dtpGivenDate.MaxDate = DateTime.Now;

                    dtpVisitDate.SelectedDate = DateTime.Now;
                    dtpVisitDate.MaxDate = DateTime.Now;

                    dtpVisGiven.MinDate = DateTime.MinValue;
                    dtpVisGiven.SelectedDate = DateTime.Now;
                    dtpVisGiven.MaxDate = DateTime.Now;
                }
            }
            chkRefusedAdministration(chkRefused.Checked);
        }

        private void ImmunizationUpdate()
        {
            /***************************************************************************************************************************************
            DESCRIPTION: This method will be called when you click the column index zero for update.it will put the values of the clicking rowindex
            (which you want to update)into the controls of the groupbox gbImmunizationDetail. 
            ***************************************************************************************************************************************/
            if (lstUpdate.Count > 0)
            {
                txtImmunizationProcedure.Text = lstUpdate[0].Procedure_Code + "-" + lstUpdate[0].Immunization_Description;
                int result = chklstImmunizationProcedures.FindItemIndexByValue(lstUpdate[0].Procedure_Code + "-" + lstUpdate[0].Immunization_Description);

                if (result != -1)
                {
                    chklstImmunizationProcedures.Items[result].Checked = true;
                }
                else
                {
                    tempCheckedListItems.Add(new RadListBoxItem(lstUpdate[0].Procedure_Code + "-" + lstUpdate[0].Immunization_Description));
                    RadListBoxItem ite = new RadListBoxItem(lstUpdate[0].Procedure_Code + "-" + lstUpdate[0].Immunization_Description);
                    ite.Checked = true;
                    chklstImmunizationProcedures.Items.Add(ite);
                }
                if (ViewState["RefusedCPTS"] != null)
                {
                    RefusedCPTS = (IList<string>)ViewState["RefusedCPTS"];
                }
                if (RefusedCPTS.Count > 0)
                {
                    string[] split = txtImmunizationProcedure.Text.Split('-');
                    var sRefuedCPT = (from p in RefusedCPTS where p.ToString() == split[0].ToString() select p).ToList();
                    if (split[0] == sRefuedCPT.FirstOrDefault())
                    {
                        lblAllergy.Text = OutputAllergy();
                    }
                }
            }
            if (objImmunization != null)
            {
                if (objImmunization.Visit_Date != DateTime.MinValue)
                    dtpVisitDate.SelectedDate = objImmunization.Visit_Date;

                if (objImmunization.Is_VIS_Given == "Y")
                {
                    chkVisgiven.Checked = true;
                    dtpVisGiven.Enabled = true;
                    dtpDateOnVis.Enabled = true;

                }
                else
                {
                    //chkVisgiven.Checked = false;
                    //dtpVisGiven.Enabled = false;
                    //dtpDateOnVis.Enabled = false;
                    //dtpVisGiven.Clear();
                    //dtpDateOnVis.Clear();
                    chkVisgiven.Checked = true;
                    dtpVisGiven.Enabled = true;
                    dtpDateOnVis.Enabled = true;
                }
                txtCVXCode.Text = objImmunization.CVX_Code;
                if (objImmunization.Dose != 0)
                {
                    txtDose.Text = objImmunization.Dose.ToString();
                }
                if (objImmunization.Dose_No != 0)
                {
                    if (cboDoseno.Items.FindItemByText(objImmunization.Dose_No.ToString()) != null)
                        cboDoseno.Items.FindItemByText(objImmunization.Dose_No.ToString()).Selected = true;
                }
                txtGivenBy.Text = objImmunization.Given_By;
                if (objImmunization.Given_Date != DateTime.MinValue)
                {
                    dtpGivenDate.SelectedDate = objImmunization.Given_Date;
                }

                if (cboImmunizationEve.Items.FindItemByText(objImmunization.Immunization_Evidence) != null)
                    cboImmunizationEve.Items.FindItemByText(objImmunization.Immunization_Evidence).Selected = true;

                if (cboLocation.Items.FindItemByText(objImmunization.Location) != null)
                    cboLocation.Items.FindItemByText(objImmunization.Location).Selected = true;

                txtLotNumber.Text = objImmunization.Lot_Number;

                if (cboManufacturer.Items.FindItemByText(objImmunization.Manufacturer) != null)
                    cboManufacturer.Items.FindItemByText(objImmunization.Manufacturer).Selected = true;

                cboManufacturer.CloseDropDownOnBlur = true;

                if (objImmunization.Administered_Amount != 0)
                {
                    txtAdminAmt.Text = objImmunization.Administered_Amount.ToString();
                }

                if (cboAdminUnit.Items.FindItemByText(objImmunization.Administered_Unit) != null)
                    cboAdminUnit.Items.FindItemByText(objImmunization.Administered_Unit).Selected = true;

                if (objImmunization.Date_on_Vis != DateTime.MinValue)
                {
                    if (objImmunization.Date_on_Vis != DateTime.MinValue)
                        dtpDateOnVis.SelectedDate = objImmunization.Date_on_Vis;
                }
                if (objImmunization.VIS_Given_Date != DateTime.MinValue)
                {
                    dtpVisGiven.SelectedDate = objImmunization.VIS_Given_Date;
                }
                if (objImmunization.Expiry_Date != DateTime.MinValue)
                {
                    dtpExpiryDate.SelectedDate = objImmunization.Expiry_Date;
                }

                if (cboRouteOfAdministration.Items.FindItemByText(objImmunization.Route_of_Administration) != null)
                    cboRouteOfAdministration.Items.FindItemByText(objImmunization.Route_of_Administration).Selected = true;

                txtNotes.txtDLC.Text = objImmunization.Notes;
                txtDLCDocumentType.txtDLC.Text = objImmunization.Document_Type;
                if (cboVFC.Items.FindItemByText(objImmunization.Vfc.Trim()) != null)
                    cboVFC.Items.FindItemByText(objImmunization.Vfc.Trim()).Selected = true;

                if (cboImmunizationSource.Items.FindItemByText(objImmunization.Immunization_Source) != null)
                    cboImmunizationSource.Items.FindItemByText(objImmunization.Immunization_Source).Selected = true;

                if (objImmunization.Authorization_Required == "Y")
                {
                    chkAuthorizationRequired.Checked = true;
                }
                else
                {
                    chkAuthorizationRequired.Checked = false;
                }

                manufacturer = objImmunization.Manufacturer;
                mvx_code = objImmunization.MVX_Code;
                AdminUnit = objImmunization.Administered_Unit;
                AdminUnitIdentifier = objImmunization.Administered_Unit_Identifier;
                CVXDesc = objImmunization.CVX_Code_Description;
                if (objImmunization.Vaccine_In_House.ToUpper() == "Y")
                {
                    chkYes.Checked = true;
                }
                else if (objImmunization.Vaccine_In_House.ToUpper() == "N")
                {
                    chkNo.Checked = true;
                }
                txtNDC.Text = objImmunization.NDC;

                //Certificate

                if (objImmunization.Is_Administration_Refused == "Y")
                {
                    cboRefused.Enabled = true;
                    cboRefused.Items.FindItemByText(objImmunization.Refused_Administration).Selected = true;
                    chkRefused.Checked = true;
                    cboEligibility.Enabled = false;
                    cboInformationSource.Enabled = false;
                    cboObservation.Enabled = false;
                }
                else
                {
                    chkRefused.Checked = false;
                    cboRefused.Enabled = false;
                    cboEligibility.Enabled = true;
                    cboInformationSource.Enabled = true;
                    cboObservation.Enabled = true;
                }

                if (cboEligibility.Items.FindItemByText(objImmunization.Eligibility_Captured) != null)
                    cboEligibility.Items.FindItemByText(objImmunization.Eligibility_Captured).Selected = true;

                if (cboInformationSource.Items.FindItemByText(objImmunization.Immunization_Information_Source) != null)
                    cboInformationSource.Items.FindItemByText(objImmunization.Immunization_Information_Source).Selected = true;

                if (cboObservation.Items.FindItemByText(objImmunization.Observation) != null)
                    cboObservation.Items.FindItemByText(objImmunization.Observation).Selected = true;
            }
            //end
        }

        private void GetCurrentDate()
        {
            dtpVisGiven.SelectedDate = null;
            //dtpGivenDate.SelectedDate = null;//Administered Date always set to Encounter DOS and is ReadOnly.
            dtpExpiryDate.SelectedDate = null;
            dtpDateOnVis.SelectedDate = null;
        }

        private void chkRefusedAdministration(bool bChecked)
        {
            if (bChecked == false)
            {
                cboRefused.Enabled = false;
                cboEligibility.Enabled = true;
                cboInformationSource.Enabled = true;
                cboObservation.Enabled = true;
                txtLotNumber.Enabled = true;
                cboManufacturer.Enabled = true;
                dtpExpiryDate.Enabled = true;
                cboImmunizationSource.Enabled = true;
                txtAdminAmt.Enabled = true;
                cboAdminUnit.Enabled = true;
                txtGivenBy.Enabled = true;
                dtpGivenDate.Enabled = true; //Administered Date always set to Encounter DOS and is ReadOnly.
                cboLocation.Enabled = true;
                txtDose.Enabled = true;
                cboDoseno.Enabled = true;
                txtNDC.Enabled = true;
            }
            else
            {
                cboRefused.Enabled = true;
                cboEligibility.Enabled = false;
                cboInformationSource.Enabled = false;
                cboObservation.Enabled = false;
                txtLotNumber.Enabled = false;
                cboManufacturer.Enabled = false;
                dtpExpiryDate.Enabled = false;
                cboImmunizationSource.Enabled = false;
                txtAdminAmt.Enabled = false;
                cboAdminUnit.Enabled = false;
                txtGivenBy.Enabled = false;
                dtpGivenDate.Enabled = false;
                cboLocation.Enabled = false;
                txtDose.Enabled = false;
                cboDoseno.Enabled = false;
                txtNDC.Enabled = false;

            }
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            Process = ClientSession.UserCurrentProcess;
            hdnCurrentProcess.Value = ClientSession.UserCurrentProcess.ToUpper();
            Immunizationlist = new List<Immunization>();
            ImmunListBasedOnEncID = new List<Immunization>();
            objImmunizationFill = new ImmunizationDTO();
            objImmunizationHistory = new ImmunizationHistory();
            chkVisgiven.Checked = true;
            chkVisgiven.Enabled = false;
            if (!IsPostBack)
            {

                RadWindow2.VisibleOnPageLoad = false;
                RadWindow2.Visible = false;

                if (Request["HumanID"] != null && Request["HumanID"].Trim() != string.Empty)
                {
                    HumanID = Convert.ToUInt32(Request["HumanID"]);
                    hdnhumanid.Value = HumanID.ToString();
                }
                else
                {
                    HumanID = ClientSession.HumanId;
                }
                if (Request["EncounterID"] != null && Request["EncounterID"].Trim() != string.Empty)
                {
                    EncounterID = Convert.ToUInt32(Request["EncounterID"]);
                    hdnEncID.Value = EncounterID.ToString();
                }
                else
                {
                    EncounterID = ClientSession.EncounterId;
                }
                if (Request["PhysicianID"] != null && Request["PhysicianID"].Trim() != string.Empty)
                {
                    PhysicianID = Convert.ToUInt32(Request["PhysicianID"]);
                    hdnPhyId.Value = PhysicianID.ToString();
                }
                else
                {
                    PhysicianID = ClientSession.PhysicianId;
                }
                if (Request["ScreenMode"] != null)
                {
                    ScreenMode = Convert.ToString(Request["ScreenMode"]);
                }
                if (Request["Screen"] != null)
                {
                    ScreenMode = Convert.ToString(Request["Screen"]);
                }
                //srividhya added on 14-dec-2015
                if (Request["OrderSubmitId"] != null && Request["OrderSubmitId"].ToString() != "")
                {
                    hdnGroupID.Value = Request["OrderSubmitId"].ToString();
                }
                if (ScreenMode == string.Empty)
                {
                    btnMoveToNextProcess.Visible = false;
                }
                else
                {
                    btnMoveToNextProcess.Visible = true;
                }

                IList<StaticLookup> iFieldValues = new List<StaticLookup>();

                txtNotes.txtDLC.TextMode = TextBoxMode.MultiLine;
                txtDLCDocumentType.txtDLC.TextMode = TextBoxMode.MultiLine;

                GridFill();
                objImmunizationFill = objImmuMgr.LoadImmunization(HumanID, PhysicianID, EncounterID, DateTime.Now, ClientSession.LegalOrg);
                objImmunizationFill.Immunization = ImmunizationList;
                objImmunizationFill.ImmunizationHistoryList = ImmunizationHistoryList;
                ViewState["Immunization"] = objImmunizationFill;

                FillImmunizationProcedure(objImmunizationFill.phyProcedureList);
                grdImmunizations.Visible = true;

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "VACCINE TYPE").ToList();
                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "IMMUNIZATIONLOCATION").ToList();
                this.cboLocation.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        this.cboLocation.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "ROUTE OF ADMINISTRATION").OrderBy(a => a.Sort_Order).ToList();
                this.cboRouteOfAdministration.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        item.Value = iFieldValues[i].Description.ToString();
                        this.cboRouteOfAdministration.Items.Add(item);
                    }
                }
                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "VFC STATUS").ToList();
                this.cboVFC.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem tempComboBoxItem = new RadComboBoxItem(iFieldValues[i].Value.ToString().Trim());
                        tempComboBoxItem.ToolTip = iFieldValues[i].Value.ToString().Trim();
                        this.cboVFC.Items.Add(tempComboBoxItem);
                        if (i == iFieldValues.Count - 1)
                            cboVFC.Text = iFieldValues[i].Default_Value.Trim();

                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "DOSE").ToList();
                this.cboDoseno.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {

                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        item.Value = iFieldValues[i].Description.ToString();
                        this.cboDoseno.Items.Add(item);
                    }

                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "IMMUNIZATION SOURCE").ToList();
                this.cboImmunizationSource.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        item.Value = iFieldValues[i].Description.ToString();
                        this.cboImmunizationSource.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "IMMUNIZATION ADMINISTRATION UNITS").ToList();
                this.cboAdminUnit.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        item.Value = iFieldValues[i].Description.ToString();
                        cboAdminUnit.Items.Add(item);
                    }
                }

                //  GetManufacturerList()

                this.cboManufacturer.Items.Add(new RadComboBoxItem(""));
                if (objImmunizationFill.manufacturerList.Count != 0)
                {
                    for (int i = 0; i < objImmunizationFill.manufacturerList.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = objImmunizationFill.manufacturerList[i].Manufacturer_Name;
                        item.ToolTip = objImmunizationFill.manufacturerList[i].Manufacturer_Name;
                        item.Value = objImmunizationFill.manufacturerList[i].MVX_Code;
                        this.cboManufacturer.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "OBSERVATION").ToList();
                this.cboObservation.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Description.ToString();
                        item.ToolTip = iFieldValues[i].Description.ToString();
                        this.cboObservation.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "IMMUNIZATION_INFORMATION_SOURCE").ToList();
                this.cboInformationSource.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Description.ToString();
                        item.ToolTip = iFieldValues[i].Description.ToString();
                        this.cboInformationSource.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "ELIGIBILITY_CAPTURED").ToList();
                this.cboEligibility.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        this.cboEligibility.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "REFUSED_ADMINISTRATION").ToList();
                this.cboRefused.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Description.ToString();
                        item.Value = iFieldValues[i].Default_Value.ToString() + "|" + iFieldValues[i].Doc_Type.ToString();
                        item.ToolTip = iFieldValues[i].Description.ToString();
                        this.cboRefused.Items.Add(item);
                    }
                }

                iFieldValues = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "IMMUNIZATION EVIDENCE").ToList();
                this.cboImmunizationEve.Items.Add(new RadComboBoxItem(""));
                if (iFieldValues != null)
                {
                    for (int i = 0; i < iFieldValues.Count; i++)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = iFieldValues[i].Value.ToString();
                        item.ToolTip = iFieldValues[i].Value.ToString();
                        this.cboImmunizationEve.Items.Add(item);
                    }
                }

                //Added for Influenza Macra
                RefusedCPTS = objImmunizationFill.objStaticLookupList.Where(a => a.Field_Name == "CPT_FOR_REFUSED_ADMINISTRATION").Select(aa => aa.Value).ToList();

                ViewState["RefusedCPTS"] = RefusedCPTS;

                SecurityServiceUtility obj = new SecurityServiceUtility();
                obj.ApplyUserPermissions(this.Page);

                chkYes.Checked = true;
                if (ClientSession.FillEncounterandWFObject != null)
                {
                    if (ClientSession.FillEncounterandWFObject.EncRecord != null && ClientSession.FillEncounterandWFObject.EncRecord.Id != 0)
                    {
                        //dtpGivenDate.MinDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.Date;
                        //dtpExpiryDate.MinDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.Date;
                        //dtpVisitDate.MinDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.Date;
                        //hdnDOS.Value = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("MM/dd/yyyy");
                        dtpGivenDate.SelectedDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;//Administered Date always set to Encounter DOS and is ReadOnly.
                        dtpGivenDate.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;


                        dtpExpiryDate.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;

                        dtpVisitDate.SelectedDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                        dtpVisitDate.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                        dtpVisitDate.MaxDate = DateTime.Now;

                        hdnDOS.Value = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).ToString("MM/dd/yyyy");

                        //for git Id 1433
                        dtpVisGiven.SelectedDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;//Administered Date always set to Encounter DOS and is ReadOnly.
                        dtpVisGiven.MinDate = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).Date;
                        dtpVisGiven.MaxDate = DateTime.Now;



                        dtpGivenDate.MaxDate = DateTime.Now;
                    }
                    else
                    {
                        dtpGivenDate.SelectedDate = DateTime.Now;
                        dtpGivenDate.MinDate = DateTime.MinValue;
                        dtpExpiryDate.MinDate = DateTime.MinValue;
                        dtpVisitDate.MinDate = DateTime.MinValue;
                        hdnDOS.Value = DateTime.MinValue.ToString("MM/DD/YYYY");
                        //for git Id 1433
                        dtpGivenDate.MaxDate = DateTime.Now;

                        dtpVisitDate.SelectedDate = DateTime.Now;
                        dtpVisitDate.MaxDate = DateTime.Now;

                        dtpVisGiven.MinDate = DateTime.MinValue;
                        dtpVisGiven.SelectedDate = DateTime.Now;
                        dtpVisGiven.MaxDate = DateTime.Now;

                    }
                }

                //dtpVisGiven.Enabled = false;
                //dtpDateOnVis.Enabled = false;
                //dtpVisGiven.MaxDate = DateTime.Now;
                //dtpDateOnVis.MaxDate = DateTime.Now;

                btnAdd.Enabled = false;
                hdnLoad.Value = "true";
                btnTestArea.Enabled = false;
                if (chkRefused.Checked == false)
                {
                    cboRefused.Enabled = false;
                }
                else
                {
                    cboRefused.Enabled = true;
                }
                //if (Process.ToUpper() == "MA_REVIEW")
                //{
                //    chklstImmunizationProcedures.Enabled = false;
                //    pnlmmunizationDetail.Enabled = false;
                //    btnManageFrequentlyUsedImmunProc.Enabled = false;
                //    gbImmunization.Enabled = false;
                //    txtNotes.Enable = false;
                //    txtNotes.pbDropdown.Disabled = true;
                //}
                if (age <= 18)
                {
                    lblVFC.Text = string.Empty;
                    lblVFC.Text = "VFC Status" + "*";
                    lblVFC.ForeColor = Color.Red;
                }
                //dtpVisGiven.SelectedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                //dtpDateOnVis.SelectedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "EnableSaveDiagnosticOrder('false');", true);
            }
            //dtpGivenDate.Enabled = false;//Administered Date always set to Encounter DOS and is ReadOnly.
            txtNotes.txtDLC.Attributes.Add("onchange", "EnableSave();");
            txtNotes.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
            txtDLCDocumentType.txtDLC.Attributes.Add("onchange", "EnableSave();");
            txtDLCDocumentType.txtDLC.Attributes.Add("onkeypress", "EnableSave();");
            if (ViewState["Immunization"] != null)
                objImmunizationFill = (ImmunizationDTO)ViewState["Immunization"];
            if (ScreenMode.ToUpper().Trim() == "MYQ")
            {
                immGrid.Style.Add("height", "685px");
            }
            //Commented By Suvarnni For Code Review Bug on 19.01.2016:37226

            //if (objImmunizationFill.ObjHuman != null)
            //    age = UtilityManager.CalculateAge(objImmunizationFill.ObjHuman.Birth_Date);
            //else
            //    age = UtilityManager.CalculateAge(ClientSession.PatientPaneList.Where(a => a.Human_Id == HumanID).Select(a => a.Birth_Date).ToList()[0]);            
        }

        protected void chklstImmunizationProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAllergy.Text = string.Empty;
            if (((RadListBox)sender).SelectedItem.Checked)
                ((RadListBox)sender).SelectedItem.Checked = false;
            else
                ((RadListBox)sender).SelectedItem.Checked = true;

            chklstImmunizationCheckItem(((RadListBox)sender).SelectedItem.Index);

            ((RadListBox)sender).ClearSelection();
            if (chkVisgiven.Checked == true)
            {
                dtpVisGiven.Enabled = true;
                dtpDateOnVis.Enabled = true;
            }
            else
            {
                //dtpVisGiven.Enabled = false;
                //dtpDateOnVis.Enabled = false;
                dtpVisGiven.Enabled = true;
                dtpDateOnVis.Enabled = true;
            }
            if (ViewState["RefusedCPTS"] != null)
            {
                RefusedCPTS = (IList<string>)ViewState["RefusedCPTS"];
            }
            if (RefusedCPTS.Count > 0)
            {
                string[] split = txtImmunizationProcedure.Text.Split('-');
                var sRefuedCPT = (from p in RefusedCPTS where p.ToString() == split[0].ToString() select p).ToList();
                if (split[0] == sRefuedCPT.FirstOrDefault())
                {
                    lblAllergy.Text = OutputAllergy();
                }
            }
            chkRefusedAdministration(chkRefused.Checked);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chklstImmunizationProcedures_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            lblAllergy.Text = string.Empty;
            RadWindow1.VisibleOnPageLoad = false;
            chklstImmunizationCheckItem(e.Item.Index);
            chklstImmunizationProcedures.Items[e.Item.Index].Selected = true;
            if (chkVisgiven.Checked == true)
            {
                dtpVisGiven.Enabled = true;
                dtpDateOnVis.Enabled = true;
            }
            else
            {
                //dtpVisGiven.Enabled = false;
                //dtpDateOnVis.Enabled = false;
                dtpVisGiven.Enabled = true;
                dtpDateOnVis.Enabled = true;
            }
            if (ViewState["RefusedCPTS"] != null)
            {
                RefusedCPTS = (IList<string>)ViewState["RefusedCPTS"];
            }
            if (RefusedCPTS.Count > 0)
            {
                string[] split = txtImmunizationProcedure.Text.Split('-');
                var sRefuedCPT = (from p in RefusedCPTS where p.ToString() == split[0].ToString() select p).ToList();
                if (split[0] == sRefuedCPT.FirstOrDefault())
                {
                    lblAllergy.Text = OutputAllergy();
                }
            }
            chkRefusedAdministration(chkRefused.Checked);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public string OutputAllergy()
        {
            string sOutput = string.Empty;
            IList<NonDrugAllergy> NonDruglst = new List<NonDrugAllergy>();
            IList<Rcopia_Allergy> Rcopia_Allergylst = new List<Rcopia_Allergy>();
            IList<ProblemList> Problemlst = new List<ProblemList>();

            IList<string> ilstImmnAllergyList = new List<string>();
            ilstImmnAllergyList.Add("NonDrugAllergyList");
            ilstImmnAllergyList.Add("Rcopia_AllergyList");
            ilstImmnAllergyList.Add("ProblemListList");

            IList<object> ilstImmnAllergyFinal = new List<object>();
            ilstImmnAllergyFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstImmnAllergyList);

            if (ilstImmnAllergyFinal != null && ilstImmnAllergyFinal.Count > 0)
            {

                if (ilstImmnAllergyFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstImmnAllergyFinal[0]).Count; iCount++)
                    {
                        NonDruglst.Add((NonDrugAllergy)((IList<object>)ilstImmnAllergyFinal[0])[iCount]);
                    }
                }

                if (ilstImmnAllergyFinal[1] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstImmnAllergyFinal[1]).Count; iCount++)
                    {
                        Rcopia_Allergylst.Add((Rcopia_Allergy)((IList<object>)ilstImmnAllergyFinal[1])[iCount]);
                    }
                }

                if (ilstImmnAllergyFinal[2] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstImmnAllergyFinal[2]).Count; iCount++)
                    {
                        Problemlst.Add((ProblemList)((IList<object>)ilstImmnAllergyFinal[2])[iCount]);
                    }
                }
            }


            //    string FileName = "Human" + "_" + ClientSession.HumanId + ".xml"; //"Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    //  itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();


            //        if (itemDoc.GetElementsByTagName("NonDrugAllergyList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("NonDrugAllergyList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
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
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(NonDrugAllergy, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(NonDrugAllergy, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(NonDrugAllergy, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(NonDrugAllergy, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(NonDrugAllergy, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }

            //                    NonDruglst.Add(NonDrugAllergy);
            //                }
            //            }
            //        }
            //        if (itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {

            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Allergy));
            //                    Rcopia_Allergy Rcopia_Allergy = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Allergy;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    //NonDrugAllergy = (NonDrugAllergy)NonDrugAllergy;
            //                    propInfo = from obji in ((Rcopia_Allergy)Rcopia_Allergy).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(Rcopia_Allergy, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(Rcopia_Allergy, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(Rcopia_Allergy, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(Rcopia_Allergy, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(Rcopia_Allergy, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }

            //                    Rcopia_Allergylst.Add(Rcopia_Allergy);
            //                }
            //            }
            //        }
            //        if (itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {

            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
            //                    ProblemList ProblemList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((ProblemList)ProblemList).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(ProblemList, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(ProblemList, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(ProblemList, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(ProblemList, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(ProblemList, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    Problemlst.Add(ProblemList);
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}
            sOutput = "Allergic to ";
            string sreturn = string.Empty;
            string sInfluenza = System.Configuration.ConfigurationSettings.AppSettings["InfluenzaImmunization"];
            sreturn = InfluenzaorDrug("DrugAllergy", sInfluenza);
            if (sreturn != string.Empty)
            {
                List<string> sarr = sreturn.Split(',').ToList();
                if (sarr.Count() > 0)
                {
                    var AllergyDescription = Rcopia_Allergylst.Where(ss => ss.NDC_ID.Split(',').Any(aa => sarr.Contains(aa))).Select(aa => aa.Allergy_Name);

                    //var AllergyDescription = (from a in Rcopia_Allergylst where sarr.Contains(a.NDC_ID) select a.Allergy_Name);
                    if (AllergyDescription.Count() > 0)
                    {
                        //sOutput = AllergyDescription.FirstOrDefault();
                        sOutput += "Influenza";
                    }
                }
            }

            sreturn = InfluenzaorDrug("NonDrugAllergy", sInfluenza);
            if (sreturn != string.Empty)
            {
                List<string> sarr = sreturn.Split(',').ToList();
                // List<string> sarr = (from a in sarr1 select a.Split('$')[1]).ToList();

                //List<string> sarr = sreturn.Split(',').ToList();
                for (int i = 0; i < sarr.Count; i++)
                {
                    var NonDrugDescription = NonDruglst.Where(ss => ss.Snomed_Code.Split(',').Any(aa => aa == sarr[i].Split('$')[1]) && ss.Is_Present == "Y").Select(aa => aa.Description).ToList();
                    if (NonDrugDescription.Count() > 0)
                    {
                        if (sOutput != "Allergic to ")
                            sOutput += ", " + sarr[i].Split('$')[0].ToString();
                        else
                            sOutput += sarr[i].Split('$')[0].ToString();
                    }
                }
                //var NonDrugDescription = NonDruglst.Where(ss => ss.Snomed_Code.Split(',').Any(aa => sarr.Contains(aa)) && ss.Is_Present=="Y").Select(aa => aa.Description).ToList();
            }

            //Manage Problem ICD Calculation
            sreturn = InfluenzaorDrug("ProblemList", sInfluenza);
            if (sreturn != string.Empty)
            {
                List<string> sarr = sreturn.Split(',').ToList();
                if (sarr.Count() > 0)
                {
                    var ProblemlstDescription = (from n in Problemlst where sarr.Contains(n.ICD) && n.Is_Active == "Y" select n).ToList();
                    if (ProblemlstDescription.Count() > 0)
                    {
                        if (sOutput != "Allergic to ")
                        {
                            if (!sOutput.Contains("Eggs"))
                                sOutput += ", " + "Eggs";
                        }
                        else
                            sOutput += "Eggs";
                    }
                    else
                    {
                        int iCount = 0;
                        //string[] sIcd=sreturn.Split(',');
                        IList<string> sIcd = new List<string>();
                        sIcd = sreturn.Split(',').ToList<string>();
                        AssessmentManager objAssessmentMngr = new AssessmentManager();
                        iCount = objAssessmentMngr.AssessmentCount(ClientSession.HumanId, sIcd);
                        if (iCount > 0)
                        {
                            if (sOutput != "Allergic to ")
                            {
                                if (!sOutput.Contains("Eggs"))
                                    sOutput += ", " + "Eggs";
                            }
                            else
                                sOutput += "Eggs";
                        }
                    }
                }
            }
            //End

            if (sOutput == "Allergic to ")
                sOutput = string.Empty;
            return sOutput;
        }
        public string InfluenzaorDrug(string sSource, string sValue)
        {
            string sreturn = string.Empty;
            if (sValue != string.Empty)
            {
                string[] arrInfluenzaorDrug = sValue.Split('|');
                for (int i = 0; i < arrInfluenzaorDrug.Count(); i++)
                {
                    string[] sarr = arrInfluenzaorDrug[i].Split('-');
                    if (sSource.ToUpper() == sarr[0].ToString().ToUpper())
                    {
                        sreturn = sarr[1].ToString();
                    }
                }
            }
            return sreturn;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            RadWindow1.VisibleOnPageLoad = false;
            RadWindow2.VisibleOnPageLoad = false;
            string sLocalTime = string.Empty;
            IList<string> orderingProcedure = new List<string>();
            string itemText = txtImmunizationProcedure.Text;
            string[] splitText = itemText.Split('-');
            string immunProcedure = string.Empty;
            if (splitText.Length > 0)
            {
                for (int i = 1; i < splitText.Length; i++)
                {
                    if (i == 1)
                        immunProcedure = splitText[i];
                    else
                        immunProcedure += "-" + splitText[i];
                }
                if (txtNotes.txtDLC.Text != string.Empty)
                {
                    immunProcedure += " - " + txtNotes.txtDLC.Text;
                }
                orderingProcedure.Add(immunProcedure);
            }

            if (btnAdd.Text == "Update")
            {
                if (ViewState["Id"] != null && ViewState["Id"].ToString() != string.Empty)
                    ulUpdateDelId = (ulong)ViewState["Id"];
                objImmunization = (from obj in objImmunizationFill.Immunization where obj.Id == ulUpdateDelId select obj).ToList<Immunization>()[0];

                InsertOrUpdate();
                string FileName = string.Empty;
                bool IsDelete = false;
                lstUpdate = objImmunizationFill.Immunization.Where(a => a.Id == ulUpdateDelId).ToList<Immunization>();
                if (lstUpdate.Count > 0)
                {
                    //if (!UIManager.IsTestArea && lstUpdate[0].File_Management_Index_Id != string.Empty)
                    //{
                    //    string[] fileNam = objImmunizationFill.lstFileManagementIndex.Where(r => r.Id == Convert.ToUInt64(lstUpdate[0].File_Management_Index_Id)).Select(a => a.File_Path).ToArray();
                    //    lstUpdate[0].Internal_Property_FileName = fileNam[0];
                    //}
                    if (UIManager.IsAnnotation)
                    {
                        if (lstUpdate[0].Internal_Property_FileName != "")
                            FileName = lstUpdate[0].Internal_Property_FileName;
                        else
                            FileName = string.Empty;
                        //FileName = UIManager.DB_Filepath;
                    }
                    else
                    {
                        if (lstUpdate[0].Internal_Property_FileName != "")
                            FileName = lstUpdate[0].Internal_Property_FileName;
                        else
                            IsDelete = true;
                    }
                }
                objImmunization.Internal_Property_FileName = FileName;
                objImmunization.Modified_By = ClientSession.UserName;
                objImmunization.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                sLocalTime = UtilityManager.ConvertToLocal(objImmunization.Modified_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");

                if (objImmunizationHistory != null)
                {
                    objImmunizationHistory.Modified_By = ClientSession.UserName;
                    objImmunizationHistory.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    sLocalTime = UtilityManager.ConvertToLocal(objImmunizationHistory.Modified_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");

                    objImmunizationHistory.Is_Deleted = "N";
                }
                if (ViewState["sPlanText"] != null)
                    sPlanText = (string)ViewState["sPlanText"];
                //else
                //    sPlanText = string.Empty;
                objImmunization.Is_Deleted = "N";
                sLocalTime = UtilityManager.ConvertToLocal(objImmunization.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                objImmunizationFill = objImmuMgr.UpdateToImmunization(objImmunization, objImmunizationHistory, EncounterID, "", sPlanText, immunProcedure, IsDelete, sLocalTime);
                ViewState["Immunization"] = objImmunizationFill;
                ImmunListBasedOnEncID = objImmunizationFill.Immunization;
                GridFill();
                this.btnAdd.Text = "Add";
                btnAdd.AccessKey = "a";
                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                text1.InnerText = "A";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                text2.InnerText = "dd";
                GetCurrentDate();
                ClearAll();

                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully();", true);
                if (tempCheckedListItems.Count > 0)
                {
                    for (int i = 0; i < tempCheckedListItems.Count; i++)
                    {
                        chklstImmunizationProcedures.Items.Remove(new RadListBoxItem(tempCheckedListItems[i].ToString()));
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "AutoSave", "EnableSaveDiagnosticOrder('false');", true);
                btnAdd.Enabled = false;
                btnTestArea.Enabled = false;

                //UIManager.DB_Filepath = string.Empty;
                //UIManager.IsTestArea = false;
            }
            else
            {

                if ((chkNo.Checked == true) || (chkYes.Checked == true))
                {
                    objImmunization = new Immunization();
                    objImmunizationHistory = new ImmunizationHistory();
                    objImmunization.Encounter_Id = EncounterID;
                    objImmunization.Human_ID = HumanID;
                    objImmunization.Physician_Id = PhysicianID;
                    objImmunization.Facility_Name = ClientSession.FacilityName;
                    InsertOrUpdate();
                    objImmunization.Internal_Property_FileName = string.Empty;
                    //objImmunization.Internal_Property_FileName = UIManager.DB_Filepath;
                    objImmunization.Created_By = ClientSession.UserName;
                    objImmunization.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    sLocalTime = UtilityManager.ConvertToUniversal(objImmunization.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
                    //objImmunization.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objImmunizationHistory.Created_By = ClientSession.UserName;
                    objImmunization.Is_Deleted = "N";
                    objImmunizationHistory.Is_Deleted = "N";
                    IList<ImmunizationHistory> ilstSaveImmunizationHistory = new List<ImmunizationHistory>();
                    ilstSaveImmunizationHistory.Add(objImmunizationHistory);
                    #region
                    //Add Bug ID:66113 

                    IList<ImmunizationHistory> ImmHislst = new List<ImmunizationHistory>();
                    IList<ImmunizationMasterHistory> ImmMasterHislst = new List<ImmunizationMasterHistory>();
                    bool _is_from_current_encounter_data = false;
                    bool _is_entries_deleted = true;
                    IList<string> ilstImmunizationTagList = new List<string>();
                    ilstImmunizationTagList.Add("ImmunizationHistoryList");                   

                    IList<object> ilstImmunizationBlobFinal = new List<object>();
                    ilstImmunizationBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstImmunizationTagList);

                    if (ilstImmunizationBlobFinal != null && ilstImmunizationBlobFinal.Count > 0)
                    {
                        if (ilstImmunizationBlobFinal[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstImmunizationBlobFinal[0]).Count; iCount++)
                            {
                               string sDeleted = ((ImmunizationHistory)((IList<object>)ilstImmunizationBlobFinal[0])[iCount]).Is_Deleted;
                                if (sDeleted == "N")
                                {
                                    _is_entries_deleted = false;
                                    break;
                                }
                            }
                            if (!_is_entries_deleted)
                            {
                                for (int iCount = 0; iCount < ((IList<object>)ilstImmunizationBlobFinal[0]).Count; iCount++)
                                {
                                    ImmHislst.Add((ImmunizationHistory)((IList<object>)ilstImmunizationBlobFinal[0])[iCount]);
                                    if (((ImmunizationHistory)((IList<object>)ilstImmunizationBlobFinal[0])[iCount]).Encounter_ID == ClientSession.EncounterId)
                                        //{
                                        _is_from_current_encounter_data = true;
                                    //}
                                }
                                    if (_is_from_current_encounter_data)
                                    {
                                        //ilstSaveImmunizationHistory.Add(objImmunizationHistory);
                                    }
                                    else
                                    {

                                        IList<string> ilstImmunizationHisTagList = new List<string>();
                                        ilstImmunizationHisTagList.Add("ImmunizationMasterHistoryList");

                                        IList<object> ilstImmunizationHisBlobFinal = new List<object>();
                                        ilstImmunizationHisBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstImmunizationHisTagList);

                                        if (ilstImmunizationHisBlobFinal != null && ilstImmunizationHisBlobFinal.Count > 0)
                                        {
                                            if (ilstImmunizationHisBlobFinal[0] != null)
                                            {
                                                for (int icount = 0; icount < ((IList<object>)ilstImmunizationHisBlobFinal[0]).Count; icount++)
                                                {
                                                    string sDeleted = ((ImmunizationMasterHistory)((IList<object>)ilstImmunizationHisBlobFinal[0])[icount]).Is_Deleted;
                                                    if (sDeleted != "Y")
                                                        ImmMasterHislst.Add(((ImmunizationMasterHistory)((IList<object>)ilstImmunizationHisBlobFinal[0])[icount]));
                                                }
                                            }
                                            if (ImmMasterHislst != null && ImmMasterHislst.Count > 0)
                                            {
                                                foreach (ImmunizationMasterHistory item in ImmMasterHislst)
                                                {
                                                    ImmunizationHistory objHistory = new ImmunizationHistory();
                                                    //bugId:61103 
                                                    //AddorUpdateImmunizationHistory(objHistory);

                                                    objHistory.Immunization_Description = item.Immunization_Description;
                                                    objHistory.CVX_Code = item.CVX_Code;
                                                    objHistory.Route_Of_Administration = item.Route_Of_Administration;

                                                    objHistory.Is_VIS_Given = item.Is_VIS_Given;
                                                    objHistory.Date_On_Vis = item.Date_On_Vis;
                                                    objHistory.Vis_Given_Date = item.Vis_Given_Date;
                                                    objHistory.Procedure_Code = item.Procedure_Code;
                                                    objHistory.Human_ID = ClientSession.HumanId;
                                                    objHistory.Physician_ID = ClientSession.PhysicianId;

                                                    objHistory.Lot_Number = item.Lot_Number;

                                                    objHistory.Administered_Amount = item.Administered_Amount;

                                                    objHistory.Administered_Date = item.Administered_Date;

                                                    objHistory.Manufacturer = item.Manufacturer;
                                                    objHistory.Administered_Unit = item.Administered_Unit;

                                                    objHistory.Expiry_Date = item.Expiry_Date;
                                                    objHistory.Immunization_Source = item.Immunization_Source;
                                                    objHistory.Location = item.Location;
                                                    objHistory.Protection_State = item.Protection_State;

                                                    objHistory.Dose = item.Dose;

                                                    objHistory.Dose_No = item.Dose_No;
                                                    objHistory.Notes = item.Notes;
                                                    objHistory.Snomed_Code = item.Snomed_Code;
                                                    objHistory.Immunization_History_Master_ID = item.Id;
                                                    objHistory.Created_By = ClientSession.UserName;
                                                    objHistory.Is_Deleted = "N";
                                                    objHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                    objHistory.Encounter_ID = ClientSession.EncounterId;
                                                    objHistory.Version = item.Version;
                                                    ilstSaveImmunizationHistory.Add(objHistory);
                                                }
                                            }
                                        }

                                    }
                                //}
                            }


                            else if(ilstImmunizationBlobFinal[0] != null && ((IList<object>)ilstImmunizationBlobFinal[0]).Count ==0)
                            {
                                      IList<string> ilstImmunizationHisTagList = new List<string>();
                                        ilstImmunizationHisTagList.Add("ImmunizationMasterHistoryList");

                                        IList<object> ilstImmunizationHisBlobFinal = new List<object>();
                                        ilstImmunizationHisBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstImmunizationHisTagList);

                                        if (ilstImmunizationHisBlobFinal != null && ilstImmunizationHisBlobFinal.Count > 0)
                                        {
                                            if (ilstImmunizationHisBlobFinal[0] != null)
                                            {
                                                for (int icount = 0; icount < ((IList<object>)ilstImmunizationHisBlobFinal[0]).Count; icount++)
                                                {
                                                    string sDeleted = ((ImmunizationMasterHistory)((IList<object>)ilstImmunizationHisBlobFinal[0])[icount]).Is_Deleted;
                                                    if (sDeleted != "Y")
                                                        ImmMasterHislst.Add(((ImmunizationMasterHistory)((IList<object>)ilstImmunizationHisBlobFinal[0])[icount]));
                                                }
                                            }
                                            if (ImmMasterHislst != null && ImmMasterHislst.Count > 0)
                                            {
                                                foreach (ImmunizationMasterHistory item in ImmMasterHislst)
                                                {
                                                    ImmunizationHistory objHistory = new ImmunizationHistory();
                                                    //bugId:61103 
                                                    //AddorUpdateImmunizationHistory(objHistory);

                                                    objHistory.Immunization_Description = item.Immunization_Description;
                                                    objHistory.CVX_Code = item.CVX_Code;
                                                    objHistory.Route_Of_Administration = item.Route_Of_Administration;

                                                    objHistory.Is_VIS_Given = item.Is_VIS_Given;
                                                    objHistory.Date_On_Vis = item.Date_On_Vis;
                                                    objHistory.Vis_Given_Date = item.Vis_Given_Date;
                                                    objHistory.Procedure_Code = item.Procedure_Code;
                                                    objHistory.Human_ID = ClientSession.HumanId;
                                                    objHistory.Physician_ID = ClientSession.PhysicianId;

                                                    objHistory.Lot_Number = item.Lot_Number;

                                                    objHistory.Administered_Amount = item.Administered_Amount;

                                                    objHistory.Administered_Date = item.Administered_Date;

                                                    objHistory.Manufacturer = item.Manufacturer;
                                                    objHistory.Administered_Unit = item.Administered_Unit;

                                                    objHistory.Expiry_Date = item.Expiry_Date;
                                                    objHistory.Immunization_Source = item.Immunization_Source;
                                                    objHistory.Location = item.Location;
                                                    objHistory.Protection_State = item.Protection_State;

                                                    objHistory.Dose = item.Dose;

                                                    objHistory.Dose_No = item.Dose_No;
                                                    objHistory.Notes = item.Notes;
                                                    objHistory.Snomed_Code = item.Snomed_Code;
                                                    objHistory.Immunization_History_Master_ID = item.Id;
                                                    objHistory.Created_By = ClientSession.UserName;
                                                    objHistory.Is_Deleted = "N";
                                                    objHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                    objHistory.Encounter_ID = ClientSession.EncounterId;
                                                    objHistory.Version = item.Version;
                                                    ilstSaveImmunizationHistory.Add(objHistory);
                                                }
                                            }
                                        }

                                    }
                                }

                            }

                        

                    

            //ImmunizationHistoryDTO ResultList = new ImmunizationHistoryDTO();
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //ImmunizationHistoryManager immunizationMngr = new ImmunizationHistoryManager();
            //IList<ImmunizationHistory> ImmHislst = new List<ImmunizationHistory>();
            //bool _is_from_current_encounter_data = false;
            //bool _is_entries_deleted = true;
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        //BugId:61234
            //        if (itemDoc.GetElementsByTagName("ImmunizationHistoryList") != null && itemDoc.GetElementsByTagName("ImmunizationHistoryList").Count > 0)
            //        {
            //            XmlNodeList xmlDeleteCheckTagName = itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0].ChildNodes;
            //            _is_entries_deleted = true;

            //            for (int j = 0; j < xmlDeleteCheckTagName.Count; j++)
            //            {
            //                for (int i = 0; i < xmlDeleteCheckTagName[j].Attributes.Count; i++)
            //                {
            //                    if (xmlDeleteCheckTagName[j].Attributes[i].Name == "Is_Deleted")
            //                    {
            //                        if (xmlDeleteCheckTagName[j].Attributes[i].InnerText == "N")
            //                        {
            //                            _is_entries_deleted = false;
            //                            break;
            //                        }
            //                    }

            //                }
            //            }

            //        }
            //        if (!_is_entries_deleted && itemDoc.GetElementsByTagName("ImmunizationHistoryList") != null && itemDoc.GetElementsByTagName("ImmunizationHistoryList").Count > 0)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0].ChildNodes;

            //            if (xmlTagName != null && xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationHistory));
            //                    ImmunizationHistory ImmunizationHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ImmunizationHistory;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((ImmunizationHistory)ImmunizationHistory).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            if (propInfo != null)
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name.ToUpper() == nodevalue.Name.ToUpper())
            //                                    {
            //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                            property.SetValue(ImmunizationHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                            property.SetValue(ImmunizationHistory, Convert.ToString(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                            property.SetValue(ImmunizationHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                            property.SetValue(ImmunizationHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                        else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                            property.SetValue(ImmunizationHistory, Convert.ToDecimal(nodevalue.Value), null);
            //                                        else
            //                                            property.SetValue(ImmunizationHistory, nodevalue.Value, null);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }

            //                    ImmHislst.Add(ImmunizationHistory);
            //                    if (ImmunizationHistory.Encounter_ID == ClientSession.EncounterId)
            //                        _is_from_current_encounter_data = true;
            //                }
            //                if (_is_from_current_encounter_data)
            //                {
            //                    //ilstSaveImmunizationHistory.Add(objImmunizationHistory);
            //                }
            //                else
            //                {
            //                    ImmunizationMasterHistoryManager immunizationMasterMngr = new ImmunizationMasterHistoryManager();
            //                    IList<ImmunizationMasterHistory> ImmMasterHislst = new List<ImmunizationMasterHistory>();
            //                    if (File.Exists(strXmlFilePath) == true)
            //                    {
            //                        XmlDocument itemDocMaster = new XmlDocument();
            //                        XmlTextReader XmlTextMaster = new XmlTextReader(strXmlFilePath);
            //                        XmlNodeList xmlTagNameMaster = null;
            //                        using (FileStream fsMaster = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //                        {
            //                            itemDocMaster.Load(fsMaster);

            //                            XmlTextMaster.Close();

            //                            if (itemDocMaster.GetElementsByTagName("ImmunizationMasterHistoryList") != null && itemDocMaster.GetElementsByTagName("ImmunizationMasterHistoryList").Count > 0)
            //                            {
            //                                xmlTagNameMaster = itemDocMaster.GetElementsByTagName("ImmunizationMasterHistoryList")[0].ChildNodes;

            //                                if (xmlTagNameMaster != null && xmlTagNameMaster.Count > 0)
            //                                {
            //                                    for (int j = 0; j < xmlTagNameMaster.Count; j++)
            //                                    {
            //                                        string TagName = xmlTagNameMaster[j].Name;
            //                                        XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationMasterHistory));
            //                                        ImmunizationMasterHistory ImmunizationMasterHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagNameMaster[j])) as ImmunizationMasterHistory;
            //                                        IEnumerable<PropertyInfo> propInfo = null;
            //                                        propInfo = from obji in ((ImmunizationMasterHistory)ImmunizationMasterHistory).GetType().GetProperties() select obji;

            //                                        for (int i = 0; i < xmlTagNameMaster[j].Attributes.Count; i++)
            //                                        {
            //                                            XmlNode nodevalue = xmlTagNameMaster[j].Attributes[i];
            //                                            {
            //                                                if (propInfo != null)
            //                                                {
            //                                                    foreach (PropertyInfo property in propInfo)
            //                                                    {
            //                                                        if (property.Name.ToUpper() == nodevalue.Name.ToUpper())
            //                                                        {
            //                                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                                property.SetValue(ImmunizationMasterHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                                property.SetValue(ImmunizationMasterHistory, Convert.ToString(nodevalue.Value), null);
            //                                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                                property.SetValue(ImmunizationMasterHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                                property.SetValue(ImmunizationMasterHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                                            else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                                property.SetValue(ImmunizationMasterHistory, Convert.ToDecimal(nodevalue.Value), null);
            //                                                            else
            //                                                                property.SetValue(ImmunizationMasterHistory, nodevalue.Value, null);
            //                                                        }
            //                                                    }
            //                                                }
            //                                            }
            //                                        }
            //                                        if (ImmunizationMasterHistory.Is_Deleted != "Y")
            //                                            ImmMasterHislst.Add(ImmunizationMasterHistory);
            //                                    }
            //                                }
            //                            }
            //                            fsMaster.Close();
            //                            fsMaster.Dispose();
            //                        }
            //                    }
            //                    if (ImmMasterHislst != null && ImmMasterHislst.Count > 0)
            //                    {
            //                        foreach (ImmunizationMasterHistory item in ImmMasterHislst)
            //                        {
            //                            ImmunizationHistory objHistory = new ImmunizationHistory();
            //                            //bugId:61103 
            //                            //AddorUpdateImmunizationHistory(objHistory);

            //                            objHistory.Immunization_Description = item.Immunization_Description;
            //                            objHistory.CVX_Code = item.CVX_Code;
            //                            objHistory.Route_Of_Administration = item.Route_Of_Administration;

            //                            objHistory.Is_VIS_Given = item.Is_VIS_Given;
            //                            objHistory.Date_On_Vis = item.Date_On_Vis;
            //                            objHistory.Vis_Given_Date = item.Vis_Given_Date;
            //                            objHistory.Procedure_Code = item.Procedure_Code;
            //                            objHistory.Human_ID = ClientSession.HumanId;
            //                            objHistory.Physician_ID = ClientSession.PhysicianId;

            //                            objHistory.Lot_Number = item.Lot_Number;

            //                            objHistory.Administered_Amount = item.Administered_Amount;

            //                            objHistory.Administered_Date = item.Administered_Date;

            //                            objHistory.Manufacturer = item.Manufacturer;
            //                            objHistory.Administered_Unit = item.Administered_Unit;

            //                            objHistory.Expiry_Date = item.Expiry_Date;
            //                            objHistory.Immunization_Source = item.Immunization_Source;
            //                            objHistory.Location = item.Location;
            //                            objHistory.Protection_State = item.Protection_State;

            //                            objHistory.Dose = item.Dose;

            //                            objHistory.Dose_No = item.Dose_No;
            //                            objHistory.Notes = item.Notes;
            //                            objHistory.Snomed_Code = item.Snomed_Code;
            //                            objHistory.Immunization_History_Master_ID = item.Id;
            //                            objHistory.Created_By = ClientSession.UserName;
            //                            objHistory.Is_Deleted = "N";
            //                            objHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            //                            objHistory.Encounter_ID = ClientSession.EncounterId;
            //                            objHistory.Version = item.Version;
            //                            ilstSaveImmunizationHistory.Add(objHistory);
            //                        }
            //                    }
            //                    //Master
            //                }
            //            }
            //        }
            //        else if (itemDoc.GetElementsByTagName("ImmunizationHistoryList") != null && itemDoc.GetElementsByTagName("ImmunizationHistoryList").Count == 0)
            //        {
            //            ImmunizationMasterHistoryManager immunizationMasterMngr = new ImmunizationMasterHistoryManager();
            //            IList<ImmunizationMasterHistory> ImmMasterHislst = new List<ImmunizationMasterHistory>();
            //            if (File.Exists(strXmlFilePath) == true)
            //            {
            //                XmlDocument itemDocMaster = new XmlDocument();
            //                XmlTextReader XmlTextMaster = new XmlTextReader(strXmlFilePath);
            //                XmlNodeList xmlTagNameMaster = null;
            //                using (FileStream fsMaster = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //                {
            //                    itemDocMaster.Load(fsMaster);

            //                    XmlTextMaster.Close();

            //                    if (itemDocMaster.GetElementsByTagName("ImmunizationMasterHistoryList") != null && itemDocMaster.GetElementsByTagName("ImmunizationMasterHistoryList").Count > 0)
            //                    {
            //                        xmlTagNameMaster = itemDocMaster.GetElementsByTagName("ImmunizationMasterHistoryList")[0].ChildNodes;

            //                        if (xmlTagNameMaster != null && xmlTagNameMaster.Count > 0)
            //                        {
            //                            for (int j = 0; j < xmlTagNameMaster.Count; j++)
            //                            {
            //                                string TagName = xmlTagNameMaster[j].Name;
            //                                XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationMasterHistory));
            //                                ImmunizationMasterHistory ImmunizationMasterHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagNameMaster[j])) as ImmunizationMasterHistory;
            //                                IEnumerable<PropertyInfo> propInfo = null;
            //                                propInfo = from obji in ((ImmunizationMasterHistory)ImmunizationMasterHistory).GetType().GetProperties() select obji;

            //                                for (int i = 0; i < xmlTagNameMaster[j].Attributes.Count; i++)
            //                                {
            //                                    XmlNode nodevalue = xmlTagNameMaster[j].Attributes[i];
            //                                    {
            //                                        if (propInfo != null)
            //                                        {
            //                                            foreach (PropertyInfo property in propInfo)
            //                                            {
            //                                                if (property.Name.ToUpper() == nodevalue.Name.ToUpper())
            //                                                {
            //                                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                        property.SetValue(ImmunizationMasterHistory, Convert.ToUInt64(nodevalue.Value), null);
            //                                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                        property.SetValue(ImmunizationMasterHistory, Convert.ToString(nodevalue.Value), null);
            //                                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                        property.SetValue(ImmunizationMasterHistory, Convert.ToDateTime(nodevalue.Value), null);
            //                                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                        property.SetValue(ImmunizationMasterHistory, Convert.ToInt32(nodevalue.Value), null);
            //                                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
            //                                                        property.SetValue(ImmunizationMasterHistory, Convert.ToDecimal(nodevalue.Value), null);
            //                                                    else
            //                                                        property.SetValue(ImmunizationMasterHistory, nodevalue.Value, null);
            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                if (ImmunizationMasterHistory.Is_Deleted != "Y")
            //                                    ImmMasterHislst.Add(ImmunizationMasterHistory);
            //                            }
            //                        }
            //                    }
            //                    fsMaster.Close();
            //                    fsMaster.Dispose();
            //                }
            //            }


            //            if (ImmMasterHislst != null && ImmMasterHislst.Count > 0)
            //            {
            //                foreach (ImmunizationMasterHistory item in ImmMasterHislst)
            //                {
            //                    ImmunizationHistory objHistory = new ImmunizationHistory();
            //                    //bugId:61103 
            //                    //AddorUpdateImmunizationHistory(objHistory);

            //                    objHistory.Immunization_Description = item.Immunization_Description;
            //                    objHistory.CVX_Code = item.CVX_Code;
            //                    objHistory.Route_Of_Administration = item.Route_Of_Administration;

            //                    objHistory.Is_VIS_Given = item.Is_VIS_Given;
            //                    objHistory.Date_On_Vis = item.Date_On_Vis;
            //                    objHistory.Vis_Given_Date = item.Vis_Given_Date;
            //                    objHistory.Procedure_Code = item.Procedure_Code;
            //                    objHistory.Human_ID = ClientSession.HumanId;
            //                    objHistory.Physician_ID = ClientSession.PhysicianId;

            //                    objHistory.Lot_Number = item.Lot_Number;

            //                    objHistory.Administered_Amount = item.Administered_Amount;

            //                    objHistory.Administered_Date = item.Administered_Date;

            //                    objHistory.Manufacturer = item.Manufacturer;
            //                    objHistory.Administered_Unit = item.Administered_Unit;

            //                    objHistory.Expiry_Date = item.Expiry_Date;
            //                    objHistory.Immunization_Source = item.Immunization_Source;
            //                    objHistory.Location = item.Location;
            //                    objHistory.Protection_State = item.Protection_State;

            //                    objHistory.Dose = item.Dose;

            //                    objHistory.Dose_No = item.Dose_No;
            //                    objHistory.Notes = item.Notes;
            //                    objHistory.Snomed_Code = item.Snomed_Code;
            //                    objHistory.Immunization_History_Master_ID = item.Id;
            //                    objHistory.Created_By = ClientSession.UserName;
            //                    objHistory.Is_Deleted = "N";
            //                    objHistory.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            //                    objHistory.Encounter_ID = ClientSession.EncounterId;
            //                    objHistory.Version = item.Version;
            //                    ilstSaveImmunizationHistory.Add(objHistory);
            //                }
            //            }
            //            //Master
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}

            //End

            #endregion
            bool IsInjection = false;
                    if (txtImmunizationProcedure.Text.ToUpper().StartsWith("J".ToUpper()))
                    {
                        IsInjection = true;
                    }
                    //if (IsInjection)
                    //{
                    //    objImmunizationFill = objImmuMgr.InsertToImmunization(objImmunization, null, EncounterID, "", immunProcedure);
                    //}
                    //else
                    //{
                    //    objImmunizationFill = objImmuMgr.InsertToImmunization(objImmunization, objImmunizationHistory, EncounterID, "", immunProcedure);
                    //}Bug ID : 66113 
                    if (IsInjection)
                    {
                        objImmunizationFill = objImmuMgr.InsertToImmunizationAndHistory(objImmunization, null, EncounterID, "", immunProcedure, sLocalTime);
                    }
                    else
                    {
                        objImmunizationFill = objImmuMgr.InsertToImmunizationAndHistory(objImmunization, ilstSaveImmunizationHistory, EncounterID, "", immunProcedure, sLocalTime);
                    }
                    ViewState["Immunization"] = objImmunizationFill;
                    GridFill();
                    ImmunListBasedOnEncID = objImmunizationFill.Immunization;
                    GetCurrentDate();
                    ClearAll();

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "SavedSuccessfully();if(window.parent.parent.theForm.hdnTabClick != undefined && window.parent.parent.theForm.hdnTabClick != null) var TabClick =window.parent.parent.theForm.hdnTabClick; else var TabClick = top.window.document.getElementById('ctl00_C5POBody_hdnTabClick'); var which_tab = TabClick.value.split('$#$')[0]; var screen_name;if (which_tab.indexOf('btn') > -1) { screen_name = 'MoveToButtonsClick'; } else if (which_tab == 'first') { screen_name = ''; } else if (which_tab != 'first' && which_tab != 'CC / HPI' && which_tab != 'QUESTIONNAIRE' && which_tab != 'PFSH' && which_tab != 'ROS' && which_tab != 'VITALS' && which_tab != 'EXAM' && which_tab != 'TEST' && which_tab != 'ASSESSMENT' && which_tab != 'ORDERS' && which_tab != 'eRx' && which_tab != 'SERV./PROC. CODES' && which_tab != 'PLAN' && which_tab != 'SUMMARY'){ screen_name = 'OrdersTabClick'; }else {screen_name = 'EncounterTabClick'; } SavedSuccessfully_NowProceed(screen_name);DisplayErrorMessage('290001');EnableSaveDiagnosticOrder('false');", true);

                    btnAdd.Enabled = false;
                    btnTestArea.Enabled = false;

                    //UIManager.DB_Filepath = string.Empty;
                    //UIManager.IsTestArea = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "Order_SaveUnsuccessful();DisplayErrorMessage('290007');", true);
                }
            }
            if (EncounterID != 0)
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
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoad", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ClientSession.bPFSHVerified = false;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            RadWindow1.VisibleOnPageLoad = false;
            RadWindow1.Visible = false;
            ClearAll();

            this.btnAdd.Text = "Add";
            this.btnClearAll.Text = "Clear All";
            btnAdd.AccessKey = "a";
            System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
            text1.InnerText = "A";
            System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
            text2.InnerText = "dd";
            System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
            text3.InnerText = "C";
            System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
            text4.InnerText = "lear All";
            this.btnAdd.Enabled = false;
            this.btnTestArea.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "DisableAutoSave", "disableAutoSave();", true);
        }

        protected void grdImmunizations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            ModalWindowPrint.VisibleOnPageLoad = false;
            RadWindowPlan.VisibleOnPageLoad = false;
            RadWindow1.VisibleOnPageLoad = false;
            RadWindow2.VisibleOnPageLoad = false;
            ModalWindow.VisibleOnPageLoad = false;

            lstUpdate.Clear();
            if (ViewState["Immunization"] != null)
                objImmunizationFill = (ImmunizationDTO)ViewState["Immunization"];
            string UpdateID = ((GridItem)(grdImmunizations.Items[e.CommandArgument.ToString()])).Cells[19].Text;
            if (UpdateID != string.Empty)
            {
                lstUpdate = objImmunizationFill.Immunization.Where(a => a.Id == Convert.ToUInt64(UpdateID)).ToList<Immunization>();
                ViewState["Id"] = Convert.ToUInt64(UpdateID);
            }
            if (Process.ToUpper() == "MA_REVIEW")
            {
                chklstImmunizationProcedures.Enabled = false;
                pnlmmunizationDetail.Enabled = false;
                btnManageFrequentlyUsedImmunProc.Enabled = false;
                gbImmunization.Enabled = false;
                txtNotes.Enable = false;
                txtNotes.pbDropdown.Disabled = true;
            }

            if (e.CommandName == "")
            {
                bEdit = true;
                ClearAll();
                //  ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Startloadinggrid", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
                if (UpdateID != string.Empty)
                {
                    if (Process.ToUpper() == "MA_REVIEW")
                    {
                        pnlOrderDetails.Enabled = true;
                        pnlVaccineAdminDetails.Enabled = true;
                        pnlmmunizationDetail.Enabled = true;
                        chklstImmunizationProcedures.Enabled = false;
                        txtNotes.Enable = true;
                        txtNotes.pbDropdown.Disabled = false;
                    }

                    ulUpdateDelId = Convert.ToUInt64(UpdateID);
                    if ((from obj in objImmunizationFill.Immunization where obj.Id == ulUpdateDelId select obj).ToList<Immunization>().Count > 0)
                    {
                        objImmunization = (from obj in objImmunizationFill.Immunization where obj.Id == ulUpdateDelId select obj).ToList<Immunization>()[0];
                        sPlanText = string.Empty;
                        sPlanText = objImmunization.Immunization_Description;
                        if (objImmunization.Notes != string.Empty)
                            sPlanText += " - " + objImmunization.Notes;
                    }

                    ViewState["sPlanText"] = sPlanText;
                    ImmunizationUpdate();
                }
                if (Process.ToUpper() == "MA_PROCESS" || Process.ToUpper() == "PROVIDER_PROCESS")
                {
                    if (chkNo.Checked == true)
                    {
                        pnlVaccineAdminDetails.Enabled = false;
                        cboInformationSource.SelectedIndex = 0;
                        cboEligibility.SelectedIndex = 0;
                        cboObservation.SelectedIndex = 0;
                    }
                    else
                    {
                        pnlVaccineAdminDetails.Enabled = true;
                    }
                }
                this.btnAdd.Text = "Update";
                btnAdd.AccessKey = "u";
                System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdd");
                text1.InnerText = "U";
                System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnAdd.FindControl("SpanAdditionalword");
                text2.InnerText = "pdate";
                this.btnClearAll.Text = "Cancel";
                System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClear");
                text3.InnerText = "C";
                System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearAll.FindControl("SpanClearAdditional");
                text4.InnerText = "ancel";
                //Commented By Suvarnni For Code Review Bug on 19.01.2016
                // lstFile = new List<FileManagementIndex>();
                //if (lstUpdate.Count > 0)
                //{
                //    foreach (string st in lstUpdate[0].File_Management_Index_Id.Split('|').ToArray().Where(s => s != String.Empty).ToArray())
                //    {
                //        try
                //        {
                //            if (objImmunizationFill.lstFileManagementIndex.Count > 0)
                //                lstFile.Add(objImmunizationFill.lstFileManagementIndex.Where(F => F.Id == Convert.ToUInt64(st)).ToList<FileManagementIndex>()[0]);
                //        }
                //        catch
                //        {

                //        }
                //    }
                //}

                btnAdd.Enabled = true;
                btnTestArea.Enabled = true;
                chkRefusedAdministration(chkRefused.Checked);
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "StopLoadingAutosave", "EnableSaveDiagnosticOrder('true'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else if (e.CommandName == "Del")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Delete", "DeleteGrid();", true);

            }
            //Commented By Suvarnni For Code Review Bug on 19.01.2016
            //else if (e.CommandName == "View")
            //{
            //    if (lstUpdate.Count > 0)
            //    {
            //        if (lstUpdate[0].File_Management_Index_Id != string.Empty)
            //        {
            //            IList<FileManagementIndex> lstFilemgnt = new List<FileManagementIndex>();
            //            if (objImmunizationFill.lstFileManagementIndex.Count > 0)
            //                lstFilemgnt = objImmunizationFill.lstFileManagementIndex.Where(F => F.Id == Convert.ToUInt64(lstUpdate[0].File_Management_Index_Id)).ToList<FileManagementIndex>();
            //            RadWindow2.Title = "View Problem Area";
            //            RadWindow2.VisibleOnPageLoad = true;
            //            RadWindow2.Visible = true;
            //            RadWindow2.NavigateUrl = "frmViewProcedure.aspx?procedureType=" + "Immunization" + "&lstFile=" + lstFilemgnt[0].File_Path;
            //            RadWindow2.VisibleOnPageLoad = true;
            //            RadWindow2.Visible = true;
            //        }
            //    }
            //}
        }

        //Commented By Suvarnni For Code Review Bug on 19.01.2016:37226
        //protected void btnTestArea_Click(object sender, EventArgs e)
        //{
        //    RadWindowPlan.VisibleOnPageLoad = false;
        //    RadWindowPlan.Visible = false;
        //    ModalWindow.VisibleOnPageLoad = false;
        //    ModalWindow.Visible = false;
        //    if (chklstImmunizationProcedures.CheckedItems.Count > 0)
        //    {
        //        objImmunizationFill = (ImmunizationDTO)ViewState["Immunization"];
        //        if (objImmunizationFill != null)
        //        {
        //            UIManager.DB_Filepath = string.Empty;
        //            btnAdd.Enabled = true;
        //            btnTestArea.Enabled = true;
        //            if (btnAdd.Text == "Add")
        //            {
        //                RadWindow1.Title = "Problem Area";
        //                RadWindow1.NavigateUrl = "frmTestArea.aspx?procedureType=" + "Immunization" + "&lstFile=" + string.Empty + "&bSave=" + "true&FileCount=" + objImmunizationFill.File_Count + "";
        //                RadWindow1.VisibleOnPageLoad = true;
        //                RadWindow1.Visible = true;
        //                RadWindow1.OnClientClose = "BeforeCloseForTestArea";
        //            }
        //            else
        //            {
        //                string lstFile = string.Empty;
        //                ulong PrimaryKey = 0;
        //                if (ViewState["Id"] != string.Empty)
        //                    PrimaryKey = Convert.ToUInt64(ViewState["Id"]);
        //                string[] FileMgnt_Id = objImmunizationFill.Immunization.Where(a => a.Id == PrimaryKey).Select(b => b.File_Management_Index_Id).ToArray();
        //                if (FileMgnt_Id.Count() != 0 && FileMgnt_Id[0] != string.Empty)
        //                    lstFile = string.Join("~", objImmunizationFill.lstFileManagementIndex.Where(r => r.Id == Convert.ToUInt64(FileMgnt_Id[0])).Select(a => a.File_Path).ToArray());
        //                RadWindow1.Title = "Problem Area";
        //                RadWindow1.NavigateUrl = "frmTestArea.aspx?procedureType=" + "Immunization" + "&lstFile=" + lstFile + "&bSave=" + "false&FileCount=" + objImmunizationFill.File_Count + "&HumanID=" + HumanID + "&EncounterID=" + EncounterID + "&PhysicianID=" + PhysicianID;
        //                RadWindow1.VisibleOnPageLoad = true;
        //                RadWindow1.Visible = true;
        //                RadWindow1.OnClientClose = "BeforeCloseForTestArea";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        txtImmunizationProcedure.Text = string.Empty;
        //    }

        //    btnAdd.Enabled = true;
        //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            string sPln = string.Empty;
            if (ViewState["Id"] != string.Empty)
                ulUpdateDelId = Convert.ToUInt64(ViewState["Id"]);
            if (objImmunizationFill != null)
            {
                lstUpdate = objImmunizationFill.Immunization.Where(a => a.Id == ulUpdateDelId).ToList<Immunization>();
                if ((from obj in objImmunizationFill.Immunization where obj.Id == ulUpdateDelId select obj).ToList<Immunization>().Count > 0)
                {
                    objImmunization = (from obj in objImmunizationFill.Immunization where obj.Id == ulUpdateDelId select obj).ToList<Immunization>()[0];
                    objImmunization.Modified_By = ClientSession.UserName;
                    objImmunization.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objImmunization.Is_Deleted = "Y";
                    sPln = "* " + objImmunization.Immunization_Description;
                    if (objImmunization.Notes != string.Empty)
                        sPln += " - " + objImmunization.Notes;
                    try
                    {
                        if (objImmunizationFill.ImmunizationHistoryList != null && objImmunizationFill.ImmunizationHistoryList.Count > 0)
                        {
                            objImmunizationHistory = (from obj in objImmunizationFill.ImmunizationHistoryList where obj.Immunization_Order_ID == objImmunization.Id select obj).ToList<ImmunizationHistory>()[0];
                            objImmunization.Modified_By = ClientSession.UserName;
                            objImmunization.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            objImmunizationHistory.Is_Deleted = "Y";
                        }
                        else
                        {
                            objImmunizationHistory = null;
                        }
                    }
                    catch (Exception)
                    {
                        objImmunizationHistory = null;
                    }
                    //Commented By Suvarnni For Code Review Bug on 19.01.2016
                    // IList<string> lstdelete = new List<string>();
                    //if (lstUpdate.Count > 0)
                    //{
                    //    foreach (string st in lstUpdate[0].File_Management_Index_Id.Split('|').ToArray().Where(s => s != String.Empty).ToArray())
                    //    {
                    //        if (objImmunizationFill.lstFileManagementIndex.Where(F => F.Id == Convert.ToUInt64(st)).ToList<FileManagementIndex>().Count > 0)
                    //            lstdelete.Add(objImmunizationFill.lstFileManagementIndex.Where(F => F.Id == Convert.ToUInt64(st)).ToList<FileManagementIndex>()[0].File_Path);
                    //    }
                    //}
                    //string localPath = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"];
                    //string path = localPath + "\\Image_Immunization\\" + ulMyPhysicianID.ToString() + "\\" + ulMyHumanID.ToString() + "\\Images";
                    //DirectoryInfo dir = new DirectoryInfo(path);
                    //if (dir.Exists)
                    //{
                    //    foreach (string file in lstdelete)
                    //    {
                    //        if (File.Exists(path + "\\" + file.Substring(file.LastIndexOf('/') + 1))) ;
                    //        {
                    //            File.Delete(path + "\\" + file.Substring(file.LastIndexOf('/') + 1));
                    //        }
                    //    }
                    //}
                }
                objImmunizationFill = objImmuMgr.DeleteImmunization(objImmunization, objImmunizationHistory, EncounterID, "", sPln);

                //objImmuMgr.UpdateToImmunization(objImmunization, objImmunizationHistory, EncounterID, "", sPlanText, immunProcedure, false);
            }
            ViewState["Immunization"] = objImmunizationFill;
            ImmunListBasedOnEncID = objImmunizationFill.Immunization;
            if (objImmunization.Encounter_Id != 0)
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
            GridFill();
            ClearAll();
            btnAdd.Enabled = false;
            btnTestArea.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DeleteSuccessfully(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();RefreshNotification('ImmunizationOrder');}", true);
        }

        protected void btnRefreshImm_proce_Click(object sender, EventArgs e)
        {
            if (ViewState["Immunization"] != null)
                objImmunizationFill = (ImmunizationDTO)ViewState["Immunization"];
            EAndMCodingManager objEAndMCodingManager = new EAndMCodingManager();
            objImmunizationFill.phyProcedureList = objEAndMCodingManager.GetPhysicianProcedure(PhysicianID, "IMMUNIZATION PROCEDURE", 0, ClientSession.LegalOrg);

            FillImmunizationProcedure(objImmunizationFill.phyProcedureList);
            ViewState["Immunization"] = objImmunizationFill;

            ModalWindow.VisibleOnPageLoad = false;
            ModalWindow.Visible = false;

            RadWindowPlan.VisibleOnPageLoad = false;
            RadWindowPlan.Visible = false;

            RadWindow1.VisibleOnPageLoad = false;
            RadWindow1.Visible = false;

            ModalWindowPrint.VisibleOnPageLoad = false;
            ModalWindowPrint.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        //Commented By Suvarnni For Code Review Bug on 19.01.2016:37226
        //protected void btnPlan_Click(object sender, EventArgs e)
        //{
        //}

        protected void btnPrintVIS_Click(object sender, EventArgs e)
        {
            RadWindow2.VisibleOnPageLoad = false;
            RadWindow2.Visible = false;

            RadWindowPlan.VisibleOnPageLoad = false;
            RadWindowPlan.Visible = false;

            RadWindow1.VisibleOnPageLoad = false;
            RadWindow1.Visible = false;

            if (txtImmunizationProcedure.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('290010'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                txtImmunizationProcedure.Focus();
                return;
            }
            string[] split = txtImmunizationProcedure.Text.Split('-');
            string visNames = string.Empty;

            if (split.Length > 0)
            {
                VaccineInfoStatementProcedureManager objVaccineInfoStatementManager = new VaccineInfoStatementProcedureManager();
                IList<VaccineInfoStatement> VISList = objVaccineInfoStatementManager.GetVaccineInfoStatementForSelectedprocedure(split[0]);

                if (VISList != null && VISList.Count > 0)
                {
                    hdnSelectedItem.Value = string.Empty;
                    string FileLocation = string.Empty;
                    bool bfilesExist = false;
                    foreach (VaccineInfoStatement vccInfo in VISList)
                    {
                        if (FileLocation == string.Empty)
                            FileLocation = new FileInfo(Server.MapPath("Documents\\Physician_Specific_Documents\\Vaccination Information Statement\\") + vccInfo.File_Name_Path.ToString()).FullName;//tempPath +                    
                        else
                            FileLocation += "|" + new FileInfo(Server.MapPath("Documents\\Physician_Specific_Documents\\Vaccination Information Statement\\") + vccInfo.File_Name_Path.ToString()).FullName;//tempPath +                         
                    }

                    if (FileLocation != string.Empty)
                    {
                        string[] FileName = FileLocation.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        if (FileName.Count() > 0)
                        {
                            for (int i = 0; i < FileName.Count(); i++)
                            {
                                if (File.Exists(FileLocation.Split('|')[i]))
                                    bfilesExist = true;
                            }
                        }
                        hdnSelectedItem.Value = FileLocation;
                    }
                    if (hdnSelectedItem.Value != string.Empty && bfilesExist)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Print Order", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PrintVIS();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('290019'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('290013'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ModalWindow.Visible = false;
            ModalWindow.VisibleOnPageLoad = false;

            RadWindow2.Visible = false;
            RadWindow2.VisibleOnPageLoad = false;

            MessageWindow.VisibleOnPageLoad = false;

            if (ViewState["Immunization"] != null)
                objImmunizationFill = (ImmunizationDTO)ViewState["Immunization"];

            HumanManager ObjEncounterManager = new HumanManager();
            IList<Human> humanRecord = ObjEncounterManager.GetPatientDetailsUsingPatientInformattion(HumanID);

            PhysicianManager objPhyMgr = new PhysicianManager();
            IList<PhysicianLibrary> objPhysician = objPhyMgr.GetphysiciannameByPhyID(PhysicianID);
            if (objImmunizationFill.Immunization.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('290011'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                string sPrintPathName = string.Empty;
                string path = Server.MapPath("Documents/" + Session.SessionID);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                PrintOrders print = new PrintOrders();
                if (objPhysician.Count > 0 && humanRecord.Count > 0)
                {
                    sPrintPathName = print.PrintImmunizationOrders(objImmunizationFill, objPhysician[0], humanRecord[0], path);
                }
                string[] Split = new string[] { Server.MapPath("Documents\\" + Session.SessionID) };
                string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                if (sPrintPathName != string.Empty)
                {
                    if (SelectedItem.Value == string.Empty)
                    {
                        SelectedItem.Value = "Documents\\" + Session.SessionID.ToString() + "\\" + FileName[0].ToString();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Print Order", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}PrintImmunizationOrderPDF();", true);
                }
            }
        }

        protected void btnMoveToNextProcess_Click(object sender, EventArgs e)
        {
            if (Request["ScreenMode"] != null && Request["ScreenMode"].ToString().ToUpper() == "MENU")
            {
                bool SubmitOrNot;
                string MedicalAssistance = string.Empty;
                if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant")
                {
                    MedicalAssistance = "UNKNOWN";
                }
                IList<string> lstOrderType = new List<string>();
                OrdersManager objOrdersManager = new OrdersManager();

                SubmitOrNot = objOrdersManager.SubmitOrdersWithOutEncounter(HumanID, 0, "Immunization", string.Empty, ClientSession.UserName, DateTime.Now, MedicalAssistance, lstOrderType.ToArray<string>(), ClientSession.FacilityName);
                if (SubmitOrNot)
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('8507001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('8507002'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
            }
            else
            {
                //Srividhya added on 14-dec-2015
                if (Convert.ToUInt32(hdnGroupID.Value) > 0)
                {
                    WFObjectManager objWfObjectManager = new WFObjectManager();
                    objWfObjectManager.MoveToNextProcess(Convert.ToUInt32(hdnGroupID.Value), "IMMUNIZATION ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), string.Empty, null, null);

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "saveimmunization", "DisplayErrorMessage('8507001'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "WindowClose();", true);
                }
                //Srividhya commented on 14-dec-2015
                //IList<Immunization> immunList = new List<Immunization>();
                //immunList = objImmuMgr.GetImmunizationUsingHumanIDEncID(HumanID, EncounterID);
                //if (immunList.Count > 0)
                //{
                //    if (immunList[0].Immunization_Group_ID != 0)
                //    {
                //        WFObjectManager objWfObjectManager = new WFObjectManager();
                //        objWfObjectManager.MoveToNextProcess(immunList[0].Immunization_Group_ID, "IMMUNIZATION ORDER", 1, "UNKNOWN", UtilityManager.ConvertToUniversal(), string.Empty, null, null);
                //    }
                //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "SaveSuccessfully", "DisplayErrorMessage('8507001');", true);
                //}
            }
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            btnAdd_Click(new object(), new EventArgs());

        }

        protected void grdImmunizations_ItemCreated(object sender, GridItemEventArgs e)
        {
            e.Item.ToolTip = "";
            if (e.Item is GridDataItem)
            {
                GridDataItem gridItem = e.Item as GridDataItem;
                foreach (GridColumn column in grdImmunizations.MasterTableView.RenderColumns)
                {
                    if (column.UniqueName == "Del")
                    {
                        gridItem[column.UniqueName].ToolTip = "Delete";
                    }
                    if (column.UniqueName == "Edit")
                    {
                        gridItem[column.UniqueName].ToolTip = "Edit";
                    }
                }
            }
        }
        #endregion
    }
}

