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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Telerik.Web.UI;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmManualResultEntry : SessionExpired
    {
        #region Declaration
        AcurusResultsMappingManager acurusResultProxy = new AcurusResultsMappingManager();
        ResultMasterManager objResultMasterProxy = new ResultMasterManager();
        LabLocationManager objLabLocProxy = new LabLocationManager();
        EncounterManager EncMngr = new EncounterManager();
        LabManager objLabproxy = new LabManager();
        PhysicianManager phyProxy = new PhysicianManager();
        DateTime utc;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                //objSecurity.ApplyUserPermissions(this.Page);
               
                DataTable dt = new DataTable();
                grdSubComponent.DataSource = dt;
                grdSubComponent.DataBind();
                AddFlagColumn();
                cboReportStatus.SelectedIndex = 0;               
                RefreshPatientBar();
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {
                    hdnPhyID.Value = ClientSession.PhysicianId.ToString();
                }
                // dtpDateReported.SelectedDate = DateTime.Now;
                IList<ResultEntryDTO> ResultDto = new List<ResultEntryDTO>();
                ResultDto = objResultMasterProxy.GetResultEntry(Convert.ToUInt64(Request["MyHumanID"]),"Load");
                hdnHumanID.Value = Request["MyHumanID"].ToString();
                ViewState["ResultDTO"] = ResultDto;
                ViewState["FileIndex"] = ResultDto[0].FileManagementIndex;
                if (ResultDto != null && ResultDto.Count > 0)
                {
                    FillTestOrderedTreeView();
                }
                btnAdd.ToolTip = "&Save";
                btnAdd.Enabled = false;
                btnSaveandMovetonextprocess.Enabled = false;
                dtpDateCollected.SelectedDate = DateTime.Now;
                dtpDateReported.SelectedDate = DateTime.Now;
                btnAdd.Attributes.Add("onclick", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}");
                btnSaveandMovetonextprocess.Attributes.Add("onclick", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}");
                //btnAddNewRow.Attributes.Add("onclick", "btnAddNewRow();");
            }



        }
        public void RefreshPatientBar()
        {           
            string strpnl = ClientSession.PatientPane;
            hdnBirthDate.Value = strpnl.Split('|')[1].ToString();
            pnlBarGroupTabs.Items[0].Text = ClientSession.PatientPane;
            pnlBarGroupTabs.Items[0].ToolTip = pnlBarGroupTabs.Items[0].Text;
            if (strpnl.Split('|')[3].ToString() == "F")
            {
                Gender.Value = "Female";
            }
            else
            {
                Gender.Value = "Male";
            }
         

        }
        IList<ResultEntryDTO> SaveResultMaster()
        {
            IList<ResultEntryDTO> ResultDto = (IList<ResultEntryDTO>)ViewState["ResultDTO"];
            //if ()
            //{
            //    UpdateResultMaster();
            //    return;
            //}
            FileManagementIndex objFileManagement = new FileManagementIndex();
            int masterItemNumber = 0;
            int ObrItemNumber = 0;
            int ObxItemNmber = 0;
            //int OrcItemNumber = 0;
            IList<ResultOBX> lstResultOBX = new List<ResultOBX>();
            string[] separator = new string[] { "@" };
            IList<string> sOrderSubmitIDList = new List<string>();
            string[] strID = hdnOrderSubmitID.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string a in strID)
            {
                sOrderSubmitIDList.Add(a.ToString());
            }
            PatientResults objPatientResults = new PatientResults();
            IList<PatientResults> ilisPatientResults = new List<PatientResults>();
            IList<ResultEntryDTO> SaveList = new List<ResultEntryDTO>();
            IList<ResultORC> lstResultORC = new List<ResultORC>();
            IList<ResultZPS> lstResultZPS = new List<ResultZPS>();

            #region Result Master
            IList<ResultMaster> lstResultmaster = new List<ResultMaster>();
            IList<ResultNTE> lstResultNTE = new List<ResultNTE>();
            HumanManager humanMngr = new HumanManager();
          //  Human HumanRecord = humanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request["MyHumanID"]))[0];
            Human HumanRecord = new Human();

            IList<string> ilstGeneralPlanTagList = new List<string>();
            ilstGeneralPlanTagList.Add("HumanList");
            IList<Human> lsthuman = new List<Human>();

            IList<object> ilstGeneralPlanBlobFinal = new List<object>();
            ilstGeneralPlanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId , ilstGeneralPlanTagList);
            if (ilstGeneralPlanBlobFinal != null && ilstGeneralPlanBlobFinal.Count > 0)
            {
                if (ilstGeneralPlanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstGeneralPlanBlobFinal[0]).Count; iCount++)
                    {
                        lsthuman.Add((Human)((IList<object>)ilstGeneralPlanBlobFinal[0])[iCount]);
                    }
                }
            }
            if(lsthuman.Count>0)
            {
                HumanRecord.Id = Convert.ToUInt32(lsthuman[0].Id);
                HumanRecord.First_Name = lsthuman[0].First_Name;
                HumanRecord.Last_Name = lsthuman[0].Last_Name;
                HumanRecord.MI = lsthuman[0].MI;
                HumanRecord.Birth_Date = Convert.ToDateTime(lsthuman[0].Birth_Date);
                HumanRecord.Sex = lsthuman[0].Sex;
                HumanRecord.Street_Address1 = lsthuman[0].Street_Address1;
                HumanRecord.Street_Address2 = lsthuman[0].Street_Address2;
                HumanRecord.City = lsthuman[0].City;
                HumanRecord.State = lsthuman[0].State;
                HumanRecord.ZipCode = lsthuman[0].ZipCode;
                HumanRecord.Home_Phone_No = lsthuman[0].Home_Phone_No;
                HumanRecord.SSN = lsthuman[0].SSN;
            }

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);

            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();

            //    if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
            //    {
            //        xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

            //        if (xmlTagName.Count > 0)
            //        {
            //            foreach (XmlNode node in xmlTagName)
            //            {
            //                HumanRecord.Id = Convert.ToUInt32(node.Attributes["Id"].Value);
            //                HumanRecord.First_Name = node.Attributes["First_Name"].Value;
            //                HumanRecord.Last_Name = node.Attributes["Last_Name"].Value;
            //                HumanRecord.MI = node.Attributes["MI"].Value;
            //                HumanRecord.Birth_Date = Convert.ToDateTime(node.Attributes["Birth_Date"].Value);
            //                HumanRecord.Sex = node.Attributes["Sex"].Value;
            //                HumanRecord.Street_Address1 = node.Attributes["Street_Address1"].Value;
            //                HumanRecord.Street_Address2 = node.Attributes["Street_Address2"].Value;
            //                HumanRecord.City = node.Attributes["City"].Value;
            //                HumanRecord.State = node.Attributes["State"].Value;
            //                HumanRecord.ZipCode = node.Attributes["ZipCode"].Value;
            //                HumanRecord.Home_Phone_No = node.Attributes["Home_Phone_No"].Value;
            //                HumanRecord.SSN = node.Attributes["SSN"].Value;
            //            }
            //        }
            //    }

            //}
           
            //Human HumanRecord = EncounterManager.Instance.GetHumanByHumanID(Convert.ToUInt64(Request["MyHumanID"]));
            IList<ResultOBR> lstResultOBR = new List<ResultOBR>();
            IList<string> OrderSubmitIDLst = new List<string>();
            for (int i = 0; i < sOrderSubmitIDList.Count; i++)
            {
                string[] sProcedure = sOrderSubmitIDList[i].Split('|');
                OrderSubmitIDLst.Add(sProcedure[0].ToString());
            }
            OrderSubmitIDLst = OrderSubmitIDLst.Distinct().ToList<string>();
            ResultMaster objResultMaster = null;
            int newIndex = 0;
            for (int k = 0; k < OrderSubmitIDLst.Count; k++)
            {
                objResultMaster = new ResultMaster();
                objResultMaster.Order_ID = Convert.ToUInt64(OrderSubmitIDLst[k]);
                objResultMaster.Is_Electronic_Mode = "N";
                objResultMaster.File_Name = string.Empty;
                objResultMaster.Matching_Patient_Id = HumanRecord.Id;

                if (SaveAndMoveClick.Value == "TRUE")
                {
                    objResultMaster.Is_Filled = "Y";
                }
                else
                {
                    objResultMaster.Is_Filled = "N";
                }
                if (cboReportStatus.Text == "FINAL")
                {
                    objResultMaster.PID_Status_Of_Specimen = "F";
                }
                else if (cboReportStatus.Text == "PRELIMINARY")
                {
                    objResultMaster.PID_Status_Of_Specimen = "P";
                }
                else if (cboReportStatus.Text == "CORRECTED RESULTS")
                {
                    objResultMaster.PID_Status_Of_Specimen = "C";
                }
                if (dtpDateReported.SelectedDate == dtpDateReported.MinDate)
                {
                    objResultMaster.MSH_Date_And_Time_Of_Message = (DateTime.Now).ToString("yyyyMMddHHmm");
                }

                else
                {
                    objResultMaster.MSH_Date_And_Time_Of_Message = (dtpDateReported.SelectedDate.Value).ToString("yyyyMMddHHmm");
                }
                if (HumanRecord != null)
                {
                    objResultMaster.PID_External_Patient_ID = Convert.ToString(HumanRecord.Id);
                    objResultMaster.PID_Alternate_Patient_ID = Convert.ToString(HumanRecord.Id);
                    objResultMaster.PID_Patient_First_Name = HumanRecord.First_Name;
                    objResultMaster.PID_Patient_Last_Name = HumanRecord.Last_Name;
                    objResultMaster.PID_Patient_Middle_Name = HumanRecord.MI;
                    objResultMaster.PID_Patient_Date_Of_Birth = HumanRecord.Birth_Date.ToString("yyyyMMdd");
                    if (HumanRecord.Sex != null && HumanRecord.Sex!="")
                        objResultMaster.PID_Patient_Gender = HumanRecord.Sex.Substring(0, 1);
                    objResultMaster.PID_Patient_Address1 = HumanRecord.Street_Address1;
                    objResultMaster.PID_Patient_Address2 = HumanRecord.Street_Address2;
                    objResultMaster.PID_Patient_City = HumanRecord.City;
                    objResultMaster.PID_Patient_State = HumanRecord.State;
                    objResultMaster.PID_Patient_Zip = HumanRecord.ZipCode;
                    objResultMaster.PID_Patient_Home_Phone = HumanRecord.Home_Phone_No;
                    if (chkFasting.Checked)
                        objResultMaster.PID_Fasting = "Y";
                    else
                        objResultMaster.PID_Fasting = "N";
                    objResultMaster.PID_Patient_SSN = HumanRecord.SSN;

                    IList<OrdersSubmit> orderSubmitList = new List<OrdersSubmit>();
                    for (int i = 0; i < ResultDto.Count; i++)
                    {
                        var query = from os in ResultDto[i].OrderSubmit where os.Id == Convert.ToUInt64(OrderSubmitIDLst[k]) select os;
                        orderSubmitList = query.ToList<OrdersSubmit>();
                    }

                    if (orderSubmitList.Count != 0)
                    {
                        objResultMaster.Lab_ID = orderSubmitList[0].Lab_ID;
                    }
                    else if (ulLabID.Value != string.Empty)
                    {
                        objResultMaster.Lab_ID = Convert.ToUInt64(ulLabID.Value);
                    }



                }
                lstResultmaster.Add(objResultMaster);
                ViewState["RMaster"] = lstResultmaster;
                masterItemNumber = lstResultmaster.Count - 1;
            #endregion


                # region Result ORC
                ResultORC objResultORC = FillResultORC(new ResultORC());
                objResultORC.Created_By = ClientSession.UserName;
                objResultORC.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objResultORC.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                lstResultORC.Add(objResultORC);
                # endregion


                # region Result ZPS
                ResultZPS objResultZPS = FillResultZPS(new ResultZPS());
                objResultZPS.Created_By = ClientSession.UserName;
                objResultZPS.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                lstResultZPS.Add(objResultZPS);
                # endregion


                # region Result OBR
                string[] sProcedure = null;
                IList<string> sLabProcedureList = new List<string>();

                for (int l = 0; l < sOrderSubmitIDList.Count; l++)
                {
                    sProcedure = sOrderSubmitIDList[l].Split('|');
                    if (sProcedure[0].ToString() == OrderSubmitIDLst[k])
                    {
                        sLabProcedureList.Add(sProcedure[1].ToString());
                    }
                }
                string[] sObservationText = null;
                string[] seprator = new string[1] { " , " };
                sLabProcedureList = sLabProcedureList.Distinct().ToList<string>();
                for (int i = 0; i < sLabProcedureList.Count; i++, newIndex++)
                {
                    ResultOBR objResultOBR = new ResultOBR();
                    sObservationText = sLabProcedureList[i].Split(':');
                    objResultOBR.OBR_Observation_Battery_Identifier = sObservationText[0];
                    if (objResultOBR.OBR_Observation_Battery_Text == "")
                    {
                        if (sObservationText.Length == 2)
                        {
                            objResultOBR.OBR_Observation_Battery_Text = sObservationText[1];
                        }
                        else if (sObservationText.Length == 3)
                        {
                            objResultOBR.OBR_Observation_Battery_Text = sObservationText[1] + "-" + sObservationText[2];
                        }
                        else if (sObservationText.Length == 4)
                        {
                            objResultOBR.OBR_Observation_Battery_Text = sObservationText[1] + "-" + sObservationText[2] + "-" + sObservationText[3];
                        }
                        else if (sObservationText.Length == 5)
                        {
                            objResultOBR.OBR_Observation_Battery_Text = sObservationText[1] + "-" + sObservationText[2] + "-" + sObservationText[3] + "-" + sObservationText[4];
                        }

                    }
                    objResultOBR.OBR_Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(dtpDateCollected.SelectedDate.Value).ToString("yyyyMMddHHmm");
                    objResultOBR.Created_By = ClientSession.UserName;
                    objResultOBR.Result_Master_ID = Convert.ToUInt64(masterItemNumber);

                    objResultOBR.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    if (cboReportStatus.Text == "FINAL")
                    {
                        objResultOBR.OBR_Order_Result_Status = "F";
                    }
                    else if (cboReportStatus.Text == "PRELIMINARY")
                    {
                        objResultOBR.OBR_Order_Result_Status = "P";
                    }
                    else if (cboReportStatus.Text == "CORRECTED RESULTS")
                    {
                        objResultOBR.OBR_Order_Result_Status = "C";
                    }
                    lstResultOBR.Add(objResultOBR);
                    if (lstResultOBR.Count > 0)
                    {
                        ObrItemNumber = lstResultOBR.Count - 1;
                    }

                # endregion


                    # region Result OBX
                    GridItem item = grdSubComponent.Items[0];

                    ResultOBX objResultOBX = default(ResultOBX);

                    for (int a = 0; a < grdSubComponent.Items.Count; a++)
                    {
                        if (grdSubComponent.Items[a].Cells[16].Text.ToString().Split('-')[0] == sLabProcedureList[i].Split(':')[0])
                        {
                            if (grdSubComponent.Items[a].Cells[11].Text.ToString() == OrderSubmitIDLst[k].ToString())
                            {
                                objResultOBX = new ResultOBX();
                                objResultOBX.OBX_Observation_Identifier = grdSubComponent.Items[a].Cells[13].Text.ToString();
                                objResultOBX.OBX_Observation_Text = ((RadTextBox)grdSubComponent.Items[a].FindControl("Test")).Text;
                                objResultOBX.OBX_Name_Of_Coding_System = "L";

                                objResultOBX.OBX_Observation_Value = ((RadTextBox)grdSubComponent.Items[a].FindControl("Result")).Text;
                                objResultOBX.OBX_Units = ((RadTextBox)grdSubComponent.Items[a].FindControl("Units")).Text;
                                objResultOBX.OBX_Reference_Range = ((RadTextBox)grdSubComponent.Items[a].FindControl("Refrange")).Text;
                                string OBX_Abnormal_Flag = ((RadComboBox)grdSubComponent.Items[a].FindControl("Flag")).Text;
                                if (OBX_Abnormal_Flag != string.Empty)
                                {
                                    ((RadComboBox)grdSubComponent.Items[a].FindControl("Flag")).SelectedIndex = ((RadComboBox)grdSubComponent.Items[a].FindControl("Flag")).FindItemByText(OBX_Abnormal_Flag).Index;
                                    objResultOBX.OBX_Abnormal_Flag = ((RadComboBox)grdSubComponent.Items[a].FindControl("Flag")).Items[((RadComboBox)grdSubComponent.Items[a].FindControl("Flag")).SelectedIndex].Text;
                                }
                                objResultOBX.Created_By = ClientSession.UserName;
                                objResultOBX.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                ViewState["OBX"] = objResultOBX;
                                objResultOBX.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                objResultOBX.Result_OBR_ID = Convert.ToUInt64(newIndex);
                                lstResultOBX.Add(objResultOBX);

                                if (lstResultOBX.Count > 0)
                                {
                                    ObxItemNmber = lstResultOBX.Count - 1;
                                }
                                objPatientResults = new PatientResults();
                                objPatientResults.Abnormal_Flags = grdSubComponent.Items[a].Cells[6].Text.ToString();
                                objPatientResults.Created_By = ClientSession.UserName;
                                objPatientResults.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                objPatientResults.Human_ID = Convert.ToUInt64(lstResultmaster[0].PID_Alternate_Patient_ID);
                                //objPatientResults.Physician_ID = 100;
                                objPatientResults.Physician_ID = Convert.ToUInt32( hdnPhyID.Value);
                                objPatientResults.Acurus_Result_Code = grdSubComponent.Items[a].Cells[14].Text.ToString();
                                objPatientResults.Acurus_Result_Description = grdSubComponent.Items[a].Cells[15].Text.ToString();
                                objPatientResults.Reference_Range = grdSubComponent.Items[a].Cells[8].Text.ToString();
                                //  dtpDateCollected.DateInput.DateFormat = "yyyy-MM-dd hh:mm:ss";
                                string value = dtpDateCollected.SelectedDate.ToString();
                                System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                                dateInfo.ShortDatePattern = "yyyy-MM-dd hh:mm:ss";
                                DateTime validDate = Convert.ToDateTime(value, dateInfo);
                                objPatientResults.Captured_date_and_time = UtilityManager.ConvertToUniversal(validDate);
                                objPatientResults.Units = ((RadTextBox)grdSubComponent.Items[a].FindControl("Units")).Text;
                                objPatientResults.Value = ((RadTextBox)grdSubComponent.Items[a].FindControl("Result")).Text;
                                objPatientResults.Results_Type = "Manual Results";
                                var ResultMasterId = from r in ((IList<ResultMaster>)ViewState["RMaster"]) where r.Order_ID == Convert.ToUInt64(grdSubComponent.Items[a].Cells[11].Text) select r;
                                if (ResultMasterId.ToList<ResultMaster>().Count > 0)
                                {
                                    objPatientResults.Result_Master_ID = ResultMasterId.ToList<ResultMaster>()[0].Id;
                                }
                                objPatientResults.Result_Master_ID = lstResultmaster[0].Id;
                                ViewState["PR"] = objPatientResults;
                                ilisPatientResults.Add(objPatientResults);
                            }
                        }
                    }

                    #endregion

                    #region Result NTE


                    ResultNTE objResultNTE = default(ResultNTE);
                    for (int b = 0; b < grdSubComponent.Items.Count; b++)
                    {
                        if (grdSubComponent.Items[b].Cells[16].Text.ToString().Split('-')[0] == sLabProcedureList[i].Split(':')[0])
                        {
                            objResultNTE = new ResultNTE();
                            if (((RadTextBox)grdSubComponent.Items[b].FindControl("Notes")).Text != string.Empty)
                            {
                                int OBR_ID = 0;
                                int OBX_ID = 0;
                                for (int index = 0; index < sLabProcedureList.Count; index++)
                                {
                                    if (sLabProcedureList[index].Split(':')[0].ToString() == grdSubComponent.Items[b].Cells[16].Text.ToString().Split('-')[0])
                                    {
                                        OBR_ID = index;
                                        break;
                                    }
                                }
                                for (int index = 0; index < lstResultOBX.Count; index++)
                                {
                                    if (lstResultOBX[index].OBX_Observation_Identifier == grdSubComponent.Items[b].Cells[13].Text.ToString())
                                    {
                                        OBX_ID = index;
                                        //break;
                                    }
                                }

                                objResultNTE.Comment_Type = "ASR Comments";
                                objResultNTE.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                objResultNTE.Result_OBR_ID = Convert.ToUInt64(OBR_ID);
                                objResultNTE.Result_OBX_ID = Convert.ToUInt64(OBX_ID);
                                objResultNTE.Created_By = ClientSession.UserName;
                                objResultNTE.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                objResultNTE.NTE_Comment_Text = ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text;
                                lstResultNTE.Add(objResultNTE);
                            }
                        }
                    }
                }
                    #endregion
                //#region FileManagementIndex
                //if ((FileManagementIndex)grdResultsNotTied.SelectedRows[0].Tag != null)
                //{
                //    objFileManagement = (FileManagementIndex)grdResultsNotTied.SelectedRows[0].Tag;
                //}        
                //#endregion


            }

            SaveAndMoveClick.Value = "FALSE";

            return objResultMasterProxy.BatchOperationsThroughResultEntry(lstResultmaster.ToArray<ResultMaster>(), lstResultOBR.ToArray<ResultOBR>(), lstResultORC.ToArray<ResultORC>(), lstResultOBX.ToArray<ResultOBX>(), lstResultNTE.ToArray<ResultNTE>(), lstResultZPS.ToArray<ResultZPS>(), ilisPatientResults.ToArray<PatientResults>(), string.Empty);
            dtpDateCollected.DateInput.DateFormat = "dd-MMM-yyyy hh:mmtt";


        }
        IList<ResultEntryDTO> UpdateResultMaster()
        {
            //grdSubComponent.DataSource = (DataTable)ViewState["DataSource"];
            //grdSubComponent.DataBind();
            IList<ResultMaster> UpdateResultMasterLst = new List<ResultMaster>();
            if (ViewState["RMaster"] != null)
            {
                //UpdateResultMasterLst = (IList<ResultMaster>)txtReportStatus.Tag;
                for (int i = 0; i < ((IList<ResultMaster>)ViewState["RMaster"]).Count; i++)
                {
                    ResultMaster objResultMaster = ((IList<ResultMaster>)ViewState["RMaster"])[i];
                    if (dtpDateReported.SelectedDate == dtpDateReported.MinDate)
                    {
                        objResultMaster.MSH_Date_And_Time_Of_Message = (DateTime.Now).ToString("yyyyMMddHHmm");
                    }

                    else
                    {
                        objResultMaster.MSH_Date_And_Time_Of_Message = (dtpDateReported.SelectedDate.Value).ToString("yyyyMMddHHmm");
                    }
                    if (chkFasting.Checked == true)
                    {
                        objResultMaster.PID_Fasting = "Y";
                    }
                    else
                    {
                        objResultMaster.PID_Fasting = "N";
                    }
                    if (cboReportStatus.Text == "FINAL")
                    {
                        objResultMaster.PID_Status_Of_Specimen = "F";
                    }
                    else if (cboReportStatus.Text == "PRELIMINARY")
                    {
                        objResultMaster.PID_Status_Of_Specimen = "P";
                    }
                    else if (cboReportStatus.Text == "CORRECTED RESULTS")
                    {
                        objResultMaster.PID_Status_Of_Specimen = "C";
                    }
                    if (SaveAndMoveClick.Value == "TRUE")
                    {
                        objResultMaster.Is_Filled = "Y";
                    }
                    else
                    {
                        objResultMaster.Is_Filled = "N";
                    }
                    objResultMaster.Modified_By = ClientSession.UserName;
                    objResultMaster.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    UpdateResultMasterLst.Add(objResultMaster);
                }
            }
            IList<ResultORC> UpdateResultORCLst = new List<ResultORC>();

            // ResultORC objResultORC = FillResultORC(new ResultORC());
            if (ViewState["ORC"] != null)
            {
                //UpdateResultORCLst = (IList<ResultORC>)txtControlNo.Tag;
                for (int i = 0; i < ((IList<ResultORC>)ViewState["ORC"]).Count; i++)
                {
                    ResultORC objResultORC = FillResultORC(((IList<ResultORC>)ViewState["ORC"])[i]);
                    objResultORC.Modified_By = ClientSession.UserName;
                    objResultORC.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    UpdateResultORCLst.Add(objResultORC);
                }

            }

            IList<ResultZPS> UpdateResultZPSLst = new List<ResultZPS>();
            if (ViewState["ZPS"] != null)
            {
                for (int i = 0; i < ((IList<ResultZPS>)ViewState["ZPS"]).Count; i++)
                {
                    ResultZPS objResultZPS = FillResultZPS(((IList<ResultZPS>)ViewState["ZPS"])[i]);
                    objResultZPS.Modified_By = ClientSession.UserName;
                    objResultZPS.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    UpdateResultZPSLst.Add(objResultZPS);
                }
            }

            IList<ResultOBR> UpdateResultOBRLst = new List<ResultOBR>();
            if (ViewState["OBR"] != null)
            {
                for (int i = 0; i < ((IList<ResultOBR>)ViewState["OBR"]).Count; i++)
                {
                    ResultOBR objResultOBR = ((IList<ResultOBR>)ViewState["OBR"])[i];
                    if (cboReportStatus.Text == "FINAL")
                    {
                        objResultOBR.OBR_Order_Result_Status = "F";
                    }
                    else if (cboReportStatus.Text == "PRELIMINARY")
                    {
                        objResultOBR.OBR_Order_Result_Status = "P";
                    }
                    else if (cboReportStatus.Text == "CORRECTED RESULTS")
                    {
                        objResultOBR.OBR_Order_Result_Status = "C";
                    }
                    // dtpDateCollected.DateInput.DateFormat = "yyyyMMddHHmm";
                    objResultOBR.OBR_Specimen_Collection_Date_And_Time = UtilityManager.ConvertToUniversal(dtpDateCollected.SelectedDate.Value).ToString("yyyyMMddHHmm");
                    objResultOBR.Modified_By = ClientSession.UserName;
                    objResultOBR.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    UpdateResultOBRLst.Add(objResultOBR);
                }
            }

            IList<ResultOBX> InsertResultOBXlst = new List<ResultOBX>();
            IList<ResultOBX> UpdateResultOBXlst = new List<ResultOBX>();
            IList<PatientResults> AddPatientResults = new List<PatientResults>();
            IList<PatientResults> UpdatePatientResults = new List<PatientResults>();
            IList<ResultNTE> InsertResultNTElst = new List<ResultNTE>();
            IList<ResultNTE> UpdateResultNTElst = new List<ResultNTE>();
            IList<ResultNTE> DeleteResultNTElst = new List<ResultNTE>();
            ResultOBX objResultOBX = null;
            for (int i = 0; i < ((IList<ResultOBR>)ViewState["OBR"]).Count; i++)
            {
                var sProcedureNames = from c in ((IList<ResultOBX>)ViewState["OBX"]) where ((IList<ResultOBR>)ViewState["OBR"])[i].Id == c.Result_OBR_ID select c;
                IList<ResultOBX> OBXList = sProcedureNames.ToList<ResultOBX>();
                for (int j = 0; j < OBXList.Count; j++)
                {
                    OBXTagList.Add(OBXList[j]);
                }
            }
            for (int i = 0; i < grdSubComponent.Items.Count; i++)
            {
                objResultOBX = new ResultOBX();
                PatientResults objPatientResults = new PatientResults();
                GridDataItem grdRowInfo = grdSubComponent.Items[i];
                if (grdRowInfo.Cells[21].Text.ToString() == string.Empty)
                {
                    objResultOBX.OBX_Observation_Identifier = grdSubComponent.Items[i].Cells[13].Text.ToString();//need to care
                    objResultOBX.OBX_Observation_Text = ((RadTextBox)grdSubComponent.Items[i].FindControl("Test")).Text;
                    objResultOBX.OBX_Name_Of_Coding_System = "L";
                    objResultOBX.OBX_Observation_Value = ((RadTextBox)grdSubComponent.Items[i].FindControl("Result")).Text;
                    objResultOBX.OBX_Units = ((RadTextBox)grdSubComponent.Items[i].FindControl("Units")).Text;
                    objResultOBX.OBX_Reference_Range = ((RadTextBox)grdSubComponent.Items[i].FindControl("Refrange")).Text;
                    string OBX_Abnormal_Flag = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).Text;
                    objResultOBX.OBX_Abnormal_Flag = OBX_Abnormal_Flag;
                    //if (OBX_Abnormal_Flag != string.Empty)
                    //{
                    //    ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).SelectedIndex = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).FindItemByText(OBX_Abnormal_Flag).Index;
                    //    objResultOBX.OBX_Abnormal_Flag = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).Items[((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).SelectedIndex].Text;
                    //}
                    var resultOBRID = from r in ((IList<ResultOBR>)ViewState["OBR"]) where r.OBR_Observation_Battery_Identifier == grdSubComponent.Items[i].Cells[16].Text.ToString().Split('-')[0] select r;
                    if (resultOBRID.ToList<ResultOBR>().Count > 0)
                    {
                        objResultOBX.Result_OBR_ID = resultOBRID.ToList<ResultOBR>()[0].Id;
                        objResultOBX.Result_Master_ID = resultOBRID.ToList<ResultOBR>()[0].Result_Master_ID;
                    }

                    objResultOBX.Created_By = ClientSession.UserName;
                    objResultOBX.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    InsertResultOBXlst.Add(objResultOBX);

                    #region Add Patient Results
                    objPatientResults = new PatientResults();
                    objPatientResults.Abnormal_Flags = grdSubComponent.Items[i].Cells[6].Text.ToString();
                    objPatientResults.Created_By = ClientSession.UserName;
                    objPatientResults.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objPatientResults.Human_ID = Convert.ToUInt64(UpdateResultMasterLst[0].PID_Alternate_Patient_ID);
                   // objPatientResults.Physician_ID = 100;
                    objPatientResults.Physician_ID = Convert.ToUInt32(hdnPhyID.Value);
                    objPatientResults.Acurus_Result_Code = grdSubComponent.Items[i].Cells[14].Text.ToString();
                    objPatientResults.Acurus_Result_Description = grdSubComponent.Items[i].Cells[15].Text.ToString();
                    objPatientResults.Reference_Range = grdSubComponent.Items[i].Cells[8].Text.ToString();
                    string value = dtpDateCollected.SelectedDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt");
                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    dateInfo.ShortDatePattern = "yyyy-MM-dd hh:mm:ss tt";
                    DateTime validDate = Convert.ToDateTime(value, dateInfo);
                    objPatientResults.Captured_date_and_time = UtilityManager.ConvertToUniversal(validDate);
                    objPatientResults.Units = grdSubComponent.Items[i].Cells[7].Text.ToString();
                    objPatientResults.Value = ((RadTextBox)grdSubComponent.Items[i].FindControl("Result")).Text;
                    objPatientResults.Result_Master_ID = objResultOBX.Result_Master_ID;
                    objPatientResults.Results_Type = "Manual Results";
                    AddPatientResults.Add(objPatientResults);
                    #endregion

                }
                else
                {
                    objResultOBX = OBXTagList.Where(a => a.Id == Convert.ToUInt64(grdRowInfo.Cells[21].Text)).ToList<ResultOBX>()[0];
                    objResultOBX.OBX_Observation_Identifier = grdSubComponent.Items[i].Cells[13].Text.ToString();
                    objResultOBX.OBX_Observation_Text = ((RadTextBox)grdSubComponent.Items[i].FindControl("Test")).Text;
                    objResultOBX.OBX_Name_Of_Coding_System = "L";
                    objResultOBX.OBX_Observation_Value = ((RadTextBox)grdSubComponent.Items[i].FindControl("Result")).Text;
                    objResultOBX.OBX_Units = ((RadTextBox)grdSubComponent.Items[i].FindControl("Units")).Text;
                    objResultOBX.OBX_Reference_Range = ((RadTextBox)grdSubComponent.Items[i].FindControl("Refrange")).Text;
                    string OBX_Abnormal_Flag = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).Text;
                    objResultOBX.OBX_Abnormal_Flag = OBX_Abnormal_Flag;
                    //if (OBX_Abnormal_Flag != string.Empty)
                    //{
                    //    ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).SelectedIndex = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).FindItemByText(OBX_Abnormal_Flag).Index;
                    //    objResultOBX.OBX_Abnormal_Flag = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).Items[((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).SelectedIndex].Text;
                    //}
                    objResultOBX.Modified_By = ClientSession.UserName;
                    objResultOBX.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    UpdateResultOBXlst.Add(objResultOBX);


                    objPatientResults = ((IList<PatientResults>)ViewState["PR"]).Where(a => a.Result_OBX_ID == Convert.ToUInt64(grdRowInfo.Cells[21].Text)).ToList<PatientResults>()[0];
                    objPatientResults.Abnormal_Flags = grdSubComponent.Items[i].Cells[6].Text.ToString();
                    objPatientResults.Created_By = ClientSession.UserName;
                    objPatientResults.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objPatientResults.Human_ID = Convert.ToUInt64(UpdateResultMasterLst[0].PID_Alternate_Patient_ID);
                   // objPatientResults.Physician_ID = 100;
                    objPatientResults.Physician_ID = Convert.ToUInt32(hdnPhyID.Value);
                    DateTime dtpSpecimentCollected = DateTime.MinValue;
                    string sCollectedDateandTime = UpdateResultOBRLst[0].OBR_Specimen_Collection_Date_And_Time;
                    dtpSpecimentCollected = DateTime.ParseExact(sCollectedDateandTime, "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);
                    string value = dtpSpecimentCollected.ToString("yyyy-MM-dd hh:mm");
                    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                    dateInfo.ShortDatePattern = "yyyy-MM-dd hh:mm";
                    DateTime validDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(value, dateInfo));
                    objPatientResults.Captured_date_and_time = UtilityManager.ConvertToUniversal(validDate);
                    objPatientResults.Reference_Range = grdSubComponent.Items[i].Cells[8].Text.ToString();
                    objPatientResults.Units = grdSubComponent.Items[i].Cells[7].Text.ToString();
                    objPatientResults.Value = ((RadTextBox)grdSubComponent.Items[i].FindControl("Result")).Text;
                    objPatientResults.Results_Type = "Manual Results";
                    UpdatePatientResults.Add(objPatientResults);
                }
                ResultNTE objResultNTE = null;
                if (((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text != string.Empty && ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text != "&nbsp;" && grdSubComponent.Items[i].Cells[9].Text.ToString() == string.Empty)
                {
                    objResultNTE = new ResultNTE();
                    int OBX_ID = 0;
                    var query = from c in UpdateResultOBRLst where (c.OBR_Observation_Battery_Identifier == grdSubComponent.Items[i].Cells[16].Text.ToString().Split('-')[0]) select c;
                    if (query.ToList<ResultOBR>().Count > 0)
                    {
                        objResultNTE.Result_OBR_ID = query.ToList<ResultOBR>()[0].Id;
                        if (objResultOBX.OBX_Observation_Identifier == grdSubComponent.Items[i].Cells[13].Text.ToString())
                        {
                            objResultNTE.Result_OBX_ID = objResultOBX.Id;

                        }
                        else if (objResultNTE.Result_OBX_ID == 0)
                        {
                            // objResultNTE.Result_OBX_ID = 0;
                            for (int index = 0; index < InsertResultOBXlst.Count; index++)
                            {
                                if (InsertResultOBXlst[index].OBX_Observation_Identifier == grdSubComponent.Items[i].Cells[13].Text.ToString())
                                {
                                    OBX_ID = index;
                                    objResultNTE.Result_OBX_ID = Convert.ToUInt64(OBX_ID);
                                    break;
                                }
                            }
                        }


                    }

                    objResultNTE.Comment_Type = "ASR Comments";
                    objResultNTE.NTE_Comment_Text = ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text;
                    var ResultMasterID = from r in ((IList<ResultOBR>)ViewState["OBR"]) where r.Id == objResultNTE.Result_OBR_ID select r;
                    if (ResultMasterID.ToList<ResultOBR>().Count > 0)
                    {
                        objResultNTE.Result_Master_ID = ResultMasterID.ToList<ResultOBR>()[0].Result_Master_ID;
                    }
                    objResultNTE.Created_By = ClientSession.UserName;
                    objResultNTE.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    InsertResultNTElst.Add(objResultNTE);
                }
                else if (((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text.Trim() != string.Empty && ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text != "&nbsp;" && grdSubComponent.Items[i].Cells[9].Text.ToString() == string.Empty)
                {
                    if (((IList<ResultNTE>)ViewState["NTE"]) != null)
                    {
                        var query = from c in ((IList<ResultNTE>)ViewState["NTE"]) where c.Id == Convert.ToUInt64(grdSubComponent.Items[i].Cells[9].Text) select c;
                        objResultNTE = query.ToList<ResultNTE>()[0];

                        RemoveDeleteLst(ref DeleteResultNTElst, objResultNTE.Id);
                        objResultNTE.NTE_Comment_Text = ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text;
                        objResultNTE.Modified_By = ClientSession.UserName;
                        objResultNTE.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateResultNTElst.Add(objResultNTE);
                    }
                }
                else if (grdSubComponent.Items[i].Cells[9].Text.ToString() != string.Empty && ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text.Trim() != string.Empty)
                {
                    if (((IList<ResultNTE>)ViewState["NTE"]) != null)
                    {
                        var query = from c in ((IList<ResultNTE>)ViewState["NTE"]) where c.Id == Convert.ToUInt64(grdSubComponent.Items[i].Cells[9].Text) select c;
                        objResultNTE = query.ToList<ResultNTE>()[0];

                        RemoveDeleteLst(ref DeleteResultNTElst, objResultNTE.Id);
                        objResultNTE.NTE_Comment_Text = ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text;
                        objResultNTE.Modified_By = ClientSession.UserName;
                        objResultNTE.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        UpdateResultNTElst.Add(objResultNTE);
                    }
                }
            }
            AddModifiedDetials(ref DeleteResultNTElst);

            //if (DeleteResultOBXlst.Count > 0 && DeleteResultOBXlst[0] != null)
            //{
            //    AddModifiedDetials(ref DeleteResultOBXlst);
            //}
            //else
            //{
            //    DeleteResultOBXlst = new List<ResultOBX>();
            //}
            SaveAndMoveClick.Value = "FALSE";
            IList<ResultOBX> obxDelList = new List<ResultOBX>();
            if (ViewState["OBXDEL"] != null)
            {
                obxDelList = (IList<ResultOBX>)ViewState["OBXDEL"];
                ViewState["OBXDEL"] = null;
            }
            IList<PatientResults> DelPatientResults = new List<PatientResults>();
            if (ViewState["PATDEL"] != null)
            {
                DelPatientResults = (IList<PatientResults>)ViewState["PATDEL"];
                ViewState["PATDEL"] = null;
            }


            return objResultMasterProxy.UpdateOperationsThroughResultEntry(UpdateResultMasterLst.ToArray<ResultMaster>(), UpdateResultOBRLst.ToArray<ResultOBR>(), UpdateResultORCLst.ToArray<ResultORC>(), InsertResultOBXlst.ToArray<ResultOBX>(), UpdateResultOBXlst.ToArray<ResultOBX>(), obxDelList.ToArray<ResultOBX>(), InsertResultNTElst.ToArray<ResultNTE>(), UpdateResultNTElst.ToArray<ResultNTE>(), DeleteResultNTElst.ToArray<ResultNTE>(), UpdateResultZPSLst.ToArray<ResultZPS>(), AddPatientResults.ToArray<PatientResults>(), UpdatePatientResults.ToArray<PatientResults>(), DelPatientResults.ToArray<PatientResults>(), string.Empty);


        }
        ResultORC FillResultORC(ResultORC objResultORC)
        {
            PhysicianLibrary objPhysician = new PhysicianLibrary();
            if (hdnPhyID.Value.Trim() != string.Empty)
            {
               // objPhysician = phyProxy.GetphysiciannameByPhyID(Convert.ToUInt64(hdnPhyID.Value))[0];
                objPhysician = GetPhysicianDetailsFromXml(Convert.ToUInt64(hdnPhyID.Value));
            }


            if (objPhysician != null)
            {
                if (objPhysician.PhyNPI != "")
                {
                    objResultORC.ORC_Ordering_Provider_ID = objPhysician.PhyNPI;
                }
                if (objPhysician.PhyLastName != "")
                {
                    objResultORC.ORC_Ordering_Provider_Last_Name = objPhysician.PhyLastName;
                }
                if (objPhysician.PhyFirstName != "")
                {
                    objResultORC.ORC_Ordering_Provider_First_Initial = objPhysician.PhyFirstName;
                }
                if (objPhysician.PhyMiddleName != "")
                {
                    objResultORC.ORC_Ordering_Provider_Middle_Initial = objPhysician.PhyMiddleName;
                }
                if (objPhysician.PhyPrefix != "")
                {
                    objResultORC.ORC_Ordering_Provider_Prefix = objPhysician.PhyPrefix;
                }
                if (objPhysician.PhySuffix != "")
                {
                    objResultORC.ORC_Ordering_Provider_Suffix = objPhysician.PhySuffix;
                }

                objPhysician = null;
            }

            return objResultORC;
        }
        ResultZPS FillResultZPS(ResultZPS objResultZPS)
        {
            objResultZPS.ZPS_Facility_Name = txtLabName.Text;

            return objResultZPS;
        }

        #region Delete
        void RemoveDeleteLst<T>(ref IList<T> ResultLst, ulong ResultID)
        {
            IList<T> LstT = ResultLst;
            ResultLst = new List<T>();
            if (LstT != null && LstT.Count > 0)
            {
                T objT = default(T);
                for (int i = 0; i < LstT.Count; i++)
                {
                    objT = LstT[i];
                    if (ResultID != (ulong)objT.GetType().GetProperty("Id").GetValue(objT, null))
                        ResultLst.Add(objT);
                }
            }
        }
        #endregion
        #region Clearall
        void ClearAll()
        {

            btnAdd.ToolTip = "&Save";
            cboReportStatus.Text = string.Empty;
            SaveAndMoveClick.Value = "FALSE";


            //DeleteResultOBXlst = new List<ResultOBX>();
            ViewState["PATDEL"] = null;

            //dtpDateCollected.SelectedDate = DateTime.Now;
            if (hdnLocalTime.Value != "")
            {
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                utc = Convert.ToDateTime(strtime);
                dtpDateCollected.SelectedDate = UtilityManager.ConvertToLocal(utc);

                //dtpDateReported.SelectedDate = DateTime.Now;
                dtpDateReported.SelectedDate = UtilityManager.ConvertToLocal(utc);
            }
            chkFasting.Checked = false;
            chkFasting.Enabled = true;
            txtLabName.Text = string.Empty;
            chkFasting.Checked = false;
            cboReportStatus.Text = string.Empty;
            txtPhysicianName.Text = string.Empty;
            btnAdd.Enabled = false;
            btnSaveandMovetonextprocess.Enabled = false;
            dtpDateCollected.Enabled = true;
            dtpDateReported.SelectedDate = DateTime.Now;
            dtpDateCollected.SelectedDate = DateTime.Now;
        }
        #endregion
        void AddModifiedDetials<T>(ref IList<T> ResultLst)
        {
            // IList<T> LstT=new List<T>();
            if (ResultLst != null && ResultLst.Count > 0)
            {
                // T objT=default(T);
                for (int i = 0; i < ResultLst.Count; i++)
                {
                    //objT = ResultLst[i];
                    ResultLst[i].GetType().GetProperty("Modified_By").SetValue(ResultLst[i], ClientSession.UserName, null);
                    ResultLst[i].GetType().GetProperty("Modified_Date_And_Time").SetValue(ResultLst[i], UtilityManager.ConvertToUniversal(), null);
                    // LstT.Add(objT);
                }
            }
            // return LstT;
        }
        DataTable CreateGridDataSource()
        {
            DataTable dtFillGrid = new DataTable();

            dtFillGrid.Columns.Add("Del", typeof(Bitmap));

            dtFillGrid.Columns.Add("Test", typeof(string));
            dtFillGrid.Columns.Add("Result", typeof(string));
            dtFillGrid.Columns.Add("Flag", typeof(string));
            dtFillGrid.Columns.Add("Units", typeof(string));
            dtFillGrid.Columns.Add("Refrange", typeof(string));
            dtFillGrid.Columns.Add("NTE", typeof(string));
            dtFillGrid.Columns.Add("Source", typeof(string));
            dtFillGrid.Columns.Add("Order Submit ID", typeof(string));
            dtFillGrid.Columns.Add("Current Process", typeof(string));
            dtFillGrid.Columns.Add("Test code", typeof(string));
            dtFillGrid.Columns.Add("Acurus Result Code", typeof(string));
            dtFillGrid.Columns.Add("Acurus Result Description", typeof(string));
            dtFillGrid.Columns.Add("ProcedureCode", typeof(string));
            dtFillGrid.Columns.Add("SNo", typeof(string));
            dtFillGrid.Columns.Add("OrderCodeDescription", typeof(string));
            dtFillGrid.Columns.Add("Order Code", typeof(string));
            dtFillGrid.Columns.Add("Notes", typeof(string));
            dtFillGrid.Columns.Add("OBX ID", typeof(string));
            return dtFillGrid;
        }
        DataTable CreateGridDataSourceForTestOrdered()
        {
            DataTable dtFillGirdTestOrdered = new DataTable();          
            dtFillGirdTestOrdered.Columns.Add("Image", typeof(Bitmap));
            dtFillGirdTestOrdered.Columns.Add("DateTime", typeof(DateTime));
            dtFillGirdTestOrdered.Columns.Add("Lab", typeof(string));
            dtFillGirdTestOrdered.Columns.Add("Tag", typeof(string));
            dtFillGirdTestOrdered.Columns.Add("EnableImage", typeof(string));
            foreach (GridColumn col in grdTestOrdered.Columns)
            {
                if (col.HeaderText.ToUpper() == "DATETIME")
                {
                    GridDateTimeColumn dv = (GridDateTimeColumn)col;
                    dv.DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}";
                }
            }
            return dtFillGirdTestOrdered;

        }
        IList<ResultOBX> OBXTagList = new List<ResultOBX>();
        void FillSubComponentGrid(IList<ResultOBR> lstResultOBR, IList<ResultOBX> lstResultOBX, IList<ResultNTE> lstResultNTE, IList<PatientResults> PatientResultList)
        {
            //OBXTagList = new List<ResultOBX>();
            IList<string> OrderSubmitIDListForWorkflow = new List<string>();
            if (lstResultOBX != null)
            {

                DataRow drGridFill = null;
                DataTable dtGridFill = CreateGridDataSource();

                for (int i = 0; i < lstResultOBR.Count; i++)
                {
                    var sProcedureNames = from c in lstResultOBX where lstResultOBR[i].Id == c.Result_OBR_ID select c;
                    IList<ResultOBX> OBXList = sProcedureNames.ToList<ResultOBX>();
                    for (int j = 0; j < OBXList.Count; j++)
                    {
                        drGridFill = dtGridFill.NewRow();
                        //  drGridFill["Del"] = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                        if (OBXList[j].OBX_Observation_Text != string.Empty)
                        {
                            drGridFill["Test"] = OBXList[j].OBX_Observation_Text;
                        }
                        if (OBXList[j].OBX_Observation_Identifier != string.Empty)
                        {
                            drGridFill["Test code"] = OBXList[j].OBX_Observation_Identifier;
                        }

                        drGridFill["Result"] = OBXList[j].OBX_Observation_Value;
                        drGridFill["Flag"] = OBXList[j].OBX_Abnormal_Flag;
                        drGridFill["Units"] = OBXList[j].OBX_Units;
                        drGridFill["Refrange"] = OBXList[j].OBX_Reference_Range;
                        drGridFill["ProcedureCode"] = lstResultOBR[i].OBR_Observation_Battery_Identifier + "-" + lstResultOBR[i].OBR_Observation_Battery_Text;

                        var OrderSubmitID = from c in ((IList<ResultMaster>)ViewState["RMaster"]) where c.Id == OBXList[j].Result_Master_ID select c;
                        IList<ResultMaster> MasterList = OrderSubmitID.ToList<ResultMaster>();
                        drGridFill["Order Submit ID"] = MasterList[0].Order_ID;
                        this.grdSubComponent.MasterTableView.GroupByExpressions.Clear();
                        GridTableView tableViewOrders = new GridTableView();
                        GridGroupByExpression expression = new GridGroupByExpression();
                        GridGroupByField gridGroupByField = new GridGroupByField();
                        gridGroupByField = new GridGroupByField();
                        gridGroupByField.FieldName = "ProcedureCode";
                        gridGroupByField.HeaderText = "Procedure Code";
                        gridGroupByField.HeaderValueSeparator = " : ";
                        gridGroupByField.FormatString = "<strong>{0}</strong>";


                        expression.SelectFields.Add(gridGroupByField);
                        expression.GroupByFields.Add(gridGroupByField);
                        tableViewOrders.GroupByExpressions.Add(expression);
                        this.grdSubComponent.MasterTableView.GroupByExpressions.Add(expression);

                        if (lstResultNTE != null && lstResultNTE.Count > 0)
                        {
                            IList<ResultNTE> lstNTE = (from n in lstResultNTE where n.Result_OBR_ID == lstResultOBR[i].Id && n.Result_OBX_ID == OBXList[j].Id && n.Comment_Type.ToUpper() == "ASR COMMENTS" select n).ToList<ResultNTE>();
                            if (lstNTE.Count > 0)
                            {
                                drGridFill["Notes"] = lstNTE[0].NTE_Comment_Text.ToString();
                                drGridFill["NTE"] = lstNTE[0].Id.ToString();
                            }
                        }

                        OrderSubmitIDListForWorkflow.Add(drGridFill["Order Submit ID"].ToString());

                        if (hdnOrderSubmitWorkflow.Value == string.Empty)
                        {
                            hdnOrderSubmitWorkflow.Value = drGridFill["Order Submit ID"].ToString();
                        }
                        else
                        {
                            hdnOrderSubmitWorkflow.Value += "||" + drGridFill["Order Submit ID"].ToString();
                        }
                        hdnHeader.Value = drGridFill["ProcedureCode"].ToString();
                        drGridFill["OBX ID"] = OBXList[j].Id.ToString();
                        OBXTagList.Add(OBXList[j]);
                        dtGridFill.Rows.Add(drGridFill);


                    }


                }
                this.grdSubComponent.MasterTableView.ToolTip = hdnHeader.Value;
                grdSubComponent.DataSource = dtGridFill;
                grdSubComponent.DataBind();
                ViewState["DataSource"] = dtGridFill;
              
                DataColumn[] keyColumns = new DataColumn[1];
                dtGridFill.DefaultView.Sort = "ProcedureCode";

                DataView dv = dtGridFill.DefaultView;
                dv.Sort = "ProcedureCode Asc";
                DataTable sortedDT = dv.ToTable();
                IList<string> FlagList = new List<string>() { " ", "Low", "High", "Very Low", "Very High", "<", ">", "Normal", "Abnormal", "Within range", "Out of range", "L", "H", "LL", "HH", "A" };
                for (int j = 0; j < grdSubComponent.Items.Count; j++)
                {
                    RadComboBox objCombo = ((RadComboBox)grdSubComponent.Items[j].FindControl("Flag"));
                    objCombo.DataSource = FlagList;
                    objCombo.DataBind();
                    if (sortedDT.Rows[j]["Flag"].ToString() != string.Empty)
                    {
                        objCombo.SelectedIndex = objCombo.FindItemByText(sortedDT.Rows[j]["Flag"].ToString()).Index;
                        objCombo.SelectedValue = objCombo.Items[objCombo.SelectedIndex].Text;
                    }
             
                }


            }

        }
        void FillValuesintoGrid()
        {
            hdnPhyID.Value = string.Empty;
            hdnLabName.Value = string.Empty;

            grdSubComponent.DataSource = null;
            IList<ResultEntryDTO> ResultDto = (IList<ResultEntryDTO>)ViewState["ResultDTO"];
            IList<string> SearchOrdersList = new List<string>();
            IList<ResultEntryDTO> OrderAndSubmitList = new List<ResultEntryDTO>();
            ClearAll();
            DataTable dtGridFill = CreateGridDataSource();
            grdSubComponent.DataSource = dtGridFill;
            grdSubComponent.DataBind();
            if (hdnIsRowClick.Value != string.Empty)
            {
                btnAdd.Enabled = true;
                btnSaveandMovetonextprocess.Enabled = true;
                hdnIsRowClick.Value = string.Empty;
            }
            ViewState["DataSource"] = dtGridFill;
            hdnOrderSubmitID.Value = string.Empty;
            hdnOrderSubmitWorkflow.Value = string.Empty;
            IList<string> OrderSubmitIDListForWorkflow = new List<string>();
            cboReportStatus.Text = "FINAL";            
            IList<string> strOrderSubmitID = new List<string>();
            DateTime dtBirthdate = DateTime.Now;
            int iAgeDiff = Convert.ToInt32(DateTime.Now.Year - dtBirthdate.Year);
            if (grdTestOrdered.SelectedItems.Count > 0)
            {
                if (grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Contains("ResultMasterID") == true)
                {
                    if (grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().EndsWith(",") == true)
                    {
                        grdTestOrdered.SelectedItems[0].Cells[5].Text = grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Substring(0, grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Length - 1);
                    }

                    IList<ResultMaster> UpdateResultMasterList = new List<ResultMaster>();
                    IList<ResultORC> UpdateResultORCList = new List<ResultORC>();
                    IList<ResultOBR> UpdateResultOBRList = new List<ResultOBR>();
                    IList<ResultOBX> UpdateResultOBXList = new List<ResultOBX>();
                    IList<ResultNTE> UpdateResultNTEList = new List<ResultNTE>();
                    IList<ResultZPS> UpdateResultZPSList = new List<ResultZPS>();
                    IList<PatientResults> UpdatePatientResultList = new List<PatientResults>();
                    string[] sResultMasterID = grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Replace("ResultMasterID", " ").Split(',');
                    for (int j = 0; j < sResultMasterID.Length; j++)
                    {
                        ulong ulResultMasterID = Convert.ToUInt64(sResultMasterID[j].ToString().Replace("ResultMasterID", ""));
                        OrderAndSubmitList = objResultMasterProxy.GetListOfResultsFromResultMasterID(ulResultMasterID, Convert.ToUInt64(Request["MyHumanID"]));

                        for (int l = 0; l < OrderAndSubmitList.Count; l++)
                        {

                            btnAdd.ToolTip = "Sa&ve";
                            IList<PatientResults> PatientResultList = OrderAndSubmitList[l].PatientResultList;
                            IList<ResultMaster> ResultMasterList = OrderAndSubmitList[l].ResultMasterList;
                            IList<ResultORC> ResultORCList = OrderAndSubmitList[l].ResultORCList;
                            IList<ResultOBR> ResultOBRList = OrderAndSubmitList[l].ResultOBRList;
                            IList<ResultOBX> ResultOBXList = OrderAndSubmitList[l].ResultOBXList;
                            IList<ResultNTE> ResultNTEList = OrderAndSubmitList[l].ResultNTEList;
                            IList<ResultZPS> ResultZPSList = OrderAndSubmitList[l].ResultZPSList;

                            UpdateResultMasterList = UpdateResultMasterList.Concat(OrderAndSubmitList[l].ResultMasterList).ToList<ResultMaster>();
                            UpdateResultORCList = UpdateResultORCList.Concat(OrderAndSubmitList[l].ResultORCList).ToList<ResultORC>();
                            UpdateResultOBRList = UpdateResultOBRList.Concat(OrderAndSubmitList[l].ResultOBRList).ToList<ResultOBR>();
                            UpdateResultOBXList = UpdateResultOBXList.Concat(OrderAndSubmitList[l].ResultOBXList).ToList<ResultOBX>();
                            UpdateResultNTEList = UpdateResultNTEList.Concat(OrderAndSubmitList[l].ResultNTEList).ToList<ResultNTE>();
                            UpdateResultZPSList = UpdateResultZPSList.Concat(OrderAndSubmitList[l].ResultZPSList).ToList<ResultZPS>();
                            UpdatePatientResultList = UpdatePatientResultList.Concat(OrderAndSubmitList[l].PatientResultList).ToList<PatientResults>();


                            ViewState["RMaster"] = UpdateResultMasterList;
                            ViewState["ORC"] = UpdateResultORCList;
                            ViewState["ZPS"] = UpdateResultZPSList;
                            ViewState["OBR"] = UpdateResultOBRList;
                            ViewState["OBX"] = UpdateResultOBXList;
                            ViewState["PR"] = UpdatePatientResultList;
                            ViewState["NTE"] = UpdateResultNTEList;
                            //ViewState["RMaster"] =OrderAndSubmitList[l].ResultMasterList;
                            //ViewState["ORC"] = OrderAndSubmitList[l].ResultORCList;
                            //ViewState["ZPS"] = OrderAndSubmitList[l].ResultZPSList;
                            //ViewState["OBR"] = OrderAndSubmitList[l].ResultOBRList;
                            //ViewState["OBX"] = OrderAndSubmitList[l].ResultOBXList;
                            //ViewState["PR"] = OrderAndSubmitList[l].PatientResultList;
                            //ViewState["NTE"] = OrderAndSubmitList[l].ResultNTEList;
                            //PatientResultList = OrderAndSubmitList[l].PatientResultList;

                            for (int i = 0; i < OrderAndSubmitList[l].ResultMasterList.Count; i++)
                            {
                                FillSubComponentGrid(ResultOBRList, ResultOBXList, ResultNTEList, PatientResultList);
                            }

                            # region Result Master
                            //CAP-3584
                            if (OrderAndSubmitList[l].ResultMasterList.Any() && OrderAndSubmitList[l].ResultMasterList[0].MSH_Date_And_Time_Of_Message != string.Empty)
                            {
                                //Commented And Added By Saravanakumar On 25-06-2013 BugId:16874
                                //dtpDateReported.Value = DateTime.ParseExact(OrderAndSubmitList[l].ResultMasterList[0].MSH_Date_And_Time_Of_Message, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                dtpDateReported.SelectedDate = DateTime.ParseExact(OrderAndSubmitList[l].ResultMasterList[0].MSH_Date_And_Time_Of_Message, "yyyyMMddHHmm", CultureInfo.InvariantCulture);

                            }
                            else
                            {
                                dtpDateReported.DateInput.DateFormat = "";
                            }
                            IList<OrdersSubmit> orderSubmitList = new List<OrdersSubmit>();
                            for (int i = 0; i < ResultDto.Count; i++)
                            {
                                for (int a = 0; a < OrderSubmitIDListForWorkflow.Distinct().ToList<string>().Count; a++)
                                {
                                    var query = from os in ResultDto[i].OrderSubmit where os.Id == Convert.ToUInt64(OrderSubmitIDListForWorkflow[a]) select os;
                                    orderSubmitList = query.ToList<OrdersSubmit>();
                                }
                            }
                            if (orderSubmitList.Count > 0)
                            {
                                if (orderSubmitList[0].Fasting == "Y")
                                {
                                    chkFasting.Enabled = false;
                                    chkFasting.Checked = true;
                                }
                                else if (orderSubmitList[0].Fasting == "N")
                                {
                                    chkFasting.Enabled = false;
                                    chkFasting.Checked = false;
                                }


                            }
                            if (chkFasting.Enabled == true)
                            {
                                //CAP-3584
                                if (OrderAndSubmitList[l].ResultMasterList.Any() && OrderAndSubmitList[l].ResultMasterList[0].PID_Fasting == "Y")
                                {
                                    chkFasting.Checked = true;
                                }
                                else
                                {
                                    chkFasting.Checked = false;

                                }
                            }

                            #endregion


                            # region Result ORC

                            if (OrderAndSubmitList[l].ResultORCList.Count > 0)
                            {
                                txtPhysicianName.Text = OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Prefix + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_First_Initial + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Last_Name + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Middle_Initial + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Suffix;
                                PhysicianLibrary objphysician = phyProxy.GetPhysicianByNPI(OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_ID)[0];
                                hdnPhyID.Value = objphysician.Id.ToString();
                            }


                            #endregion

                            # region Result OBR
                            //CAP-3584
                            if (OrderAndSubmitList[l].ResultOBRList.Any())
                            {
                                dtpDateCollected.SelectedDate = UtilityManager.ConvertToLocal(DateTime.ParseExact(OrderAndSubmitList[l].ResultOBRList[0].OBR_Specimen_Collection_Date_And_Time, "yyyyMMddHHmm", CultureInfo.InvariantCulture));
                            }

                            if (OrderAndSubmitList[l].ResultOBRList.Any() && OrderAndSubmitList[l].ResultOBRList[0].OBR_Order_Result_Status.ToUpper() == "F")
                            {
                                cboReportStatus.Text = "FINAL";
                                cboReportStatus.SelectedIndex = cboReportStatus.Items.FindItemByText(cboReportStatus.Text).Index;
                            }
                            else if (OrderAndSubmitList[l].ResultOBRList.Any() && OrderAndSubmitList[l].ResultOBRList[0].OBR_Order_Result_Status.ToUpper() == "P")
                            {
                                cboReportStatus.Text = "PRELIMINARY";
                                cboReportStatus.SelectedIndex = cboReportStatus.Items.FindItemByText(cboReportStatus.Text).Index;
                            }
                            else if (OrderAndSubmitList[l].ResultOBRList.Any() && OrderAndSubmitList[l].ResultOBRList[0].OBR_Order_Result_Status.ToUpper() == "C")
                            {
                                cboReportStatus.Text = "CORRECTED RESULTS";
                                cboReportStatus.SelectedIndex = cboReportStatus.Items.FindItemByText(cboReportStatus.Text).Index;
                            }
                            #endregion

                            # region Result ZPS
                            if (OrderAndSubmitList[l].ResultZPSList.Count > 0)
                            {
                                txtLabName.Text = OrderAndSubmitList[l].ResultZPSList[0].ZPS_Facility_Name;

                            }
                            if (orderSubmitList.Count > 0)
                            {
                                if (orderSubmitList[0].Specimen_Collection_Date_And_Time.ToString("yyyy-mm-dd hh:mm:ss") != "0001-01-01 00:00:00" && orderSubmitList[0].Specimen_Collection_Date_And_Time.ToString("yyyy-mm-dd hh:mm:ss") != "0001-00-01 12:00:00")
                                {
                                    dtpDateCollected.Enabled = false;
                                    string date = (orderSubmitList[0].Specimen_Collection_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                                else
                                {
                                    dtpDateCollected.Enabled = true;
                                    string date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                            }
                            else
                            {
                                //CAP-3584
                                if (UpdatePatientResultList.Any() && UpdatePatientResultList[0].Captured_date_and_time != null)
                                {
                                    dtpDateCollected.Enabled = true;
                                    string date = UtilityManager.ConvertToLocal(UpdatePatientResultList[0].Captured_date_and_time).ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                                //CAP-3584
                                else if (orderSubmitList.Any() && orderSubmitList[0].Specimen_Collection_Date_And_Time != null)
                                {
                                    dtpDateCollected.Enabled = true;
                                    string date = (orderSubmitList[0].Specimen_Collection_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                            }
                            #endregion
                        }
                    }

                    // DisplayLabDetails();

                }
                else if ((grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Contains("BlankOrder")) == true)
                {
                  
                    if (btnAdd.Enabled == true)
                    {
                    }
                    cboReportStatus.Text = "FINAL";
                    //int iAgeDiff = Convert.ToInt32(DateTime.Now.Year - dtBirthdate.Year);
                    SearchOrdersList.Clear();

                    string[] sOrderCodeList = grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Replace("BlankOrder", "").Split(',');
                    for (int k = 0; k < sOrderCodeList.Length - 1; k++)
                    {
                        strOrderSubmitID.Add(sOrderCodeList[k].ToString());
                    }
                    //  ulong OrderPhysicianID = 0;
                    // string OrderLabName = string.Empty;

                    for (int l = 0; l < ResultDto.Count; l++)
                    {
                        var ord = from s in ResultDto[l].OrderSubmit where s.Id == Convert.ToUInt64(strOrderSubmitID[0].ToString()) select s;
                        IList<OrdersSubmit> ordersList = ord.ToList<OrdersSubmit>();
                        hdnPhyID.Value = ordersList[0].Physician_ID.ToString();
                        hdnLabName.Value = ordersList[0].Lab_Name;
                        ulLabID.Value = ordersList[0].Lab_ID.ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenSearchOrders('SCAN');", true);
                    
                }
                else
                {
                    for (int b = 0; b < grdSubComponent.Items.Count; b++)
                    {
                        RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[b].FindControl("Flag");
                        objRadComboBox.DataSource = AddFlagColumn();
                        objRadComboBox.DataBind();
                    }
                    cboReportStatus.Text = "FINAL";
                    string Procedure = string.Empty;
                    string sTag = grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString();
                    //string sTag = lstResultItems.Items[lstResultItems.SelectedIndex].Tag.ToString();
                    string[] sOrderCodeList = sTag.Replace("OrderCode", "").Split(',');

                    IList<string> strOrderCode = new List<string>();
                    strOrderSubmitID = new List<string>();
                    for (int k = 0; k < sOrderCodeList.Length; k++)
                    {
                        if (sOrderCodeList[k].Contains("-") == true)
                        {
                            strOrderCode.Add(sOrderCodeList[k].Split('-')[0]);
                            strOrderSubmitID.Add(sOrderCodeList[k].Split('-')[1]);
                        }

                    }
                    IList<OrdersSubmit> orderSubmitList = new List<OrdersSubmit>();
                    for (int i = 0; i < ResultDto.Count; i++)
                    {
                        if (strOrderSubmitID.Count > 0)
                        {
                            var query = from os in ResultDto[i].OrderSubmit where os.Id == Convert.ToUInt64(strOrderSubmitID[0]) select os;
                            orderSubmitList = query.ToList<OrdersSubmit>();

                        }
                        if (orderSubmitList.Count() > 0)
                        {
                            ulLabID.Value = orderSubmitList[0].Lab_ID.ToString();
                            hdnPhyID.Value = orderSubmitList[0].Physician_ID.ToString();
                        }
                    }


                    DataRow drFillGrid = null;
                    dtGridFill.Rows.Clear();
                    for (int b = 0; b < strOrderCode.Count(); b++)
                    {
                        ArrayList arryLst = acurusResultProxy.GetAcurusResultMappingListForMRE(strOrderCode[b].ToString(), Gender.Value.ToUpper(), iAgeDiff, Convert.ToInt32(ulLabID.Value));
                        if (arryLst != null && arryLst.Count > 0)
                        {
                            for (int i = 0; i < arryLst.Count; i++)
                            {
                                object[] objlst = (object[])arryLst[i];

                                drFillGrid = dtGridFill.NewRow();
                                // drFillGrid["Del"] = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                                if (objlst[2] != null)
                                    drFillGrid["Units"] = objlst[2].ToString();
                                if (objlst[3] != null)
                                    drFillGrid["Refrange"] = objlst[3].ToString();
                                drFillGrid["Test code"] = objlst[0].ToString();
                                drFillGrid["Test"] = objlst[1].ToString();
                                drFillGrid["Order Code"] = objlst[6].ToString();
                                drFillGrid["OrderCodeDescription"] = objlst[7].ToString();
                                drFillGrid["Acurus Result Code"] = objlst[4].ToString();
                                drFillGrid["Acurus Result Description"] = objlst[5].ToString();
                                drFillGrid["ProcedureCode"] = objlst[6].ToString() + "-" + objlst[7].ToString();
                                for (int j = 0; j < sOrderCodeList.Length - 1; j++)
                                {
                                    if (sOrderCodeList[j].Contains(drFillGrid["Order Code"].ToString()) == true)
                                    {
                                        drFillGrid["Order Submit ID"] = sOrderCodeList[j].Split('-')[1].ToString();
                                        //sOrderSubmitIDList.Add((drFillGrid["Order Submit ID"] + "|" + objlst[6].ToString() + ":" + objlst[7].ToString()));
                                        if (hdnOrderSubmitID.Value == string.Empty)
                                        {
                                            hdnOrderSubmitID.Value = drFillGrid["Order Submit ID"] + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                        }
                                        else
                                        {
                                            hdnOrderSubmitID.Value += "@" + drFillGrid["Order Submit ID"] + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                        }
                                        // OrderSubmitIDListForWorkflow.Add(drFillGrid["Order Submit ID"].ToString());
                                        if (hdnOrderSubmitWorkflow.Value == string.Empty)
                                        {
                                            hdnOrderSubmitWorkflow.Value = drFillGrid["Order Submit ID"].ToString();
                                        }
                                        else
                                        {
                                            hdnOrderSubmitWorkflow.Value += "||" + drFillGrid["Order Submit ID"].ToString();
                                        }
                                    }
                                }
                                dtGridFill.Rows.Add(drFillGrid);
                            }


                            foreach (GridColumn col in grdSubComponent.Columns)
                            {
                                if (col.HeaderText.ToUpper() == "NTE" || col.HeaderText.ToUpper() == "Source" || col.HeaderText.ToUpper() == "Order Submit ID" || col.HeaderText.ToUpper() == "Current Process" || col.HeaderText.ToUpper() == "Test code" || col.HeaderText.ToUpper() == "Acurus Result Description" || col.HeaderText.ToUpper() == "Acurus Result Code" || col.HeaderText.ToUpper() == "SNo" || col.HeaderText.ToUpper() == "ProcedureCode" || col.HeaderText.ToUpper() == "Tag" || col.HeaderText.ToUpper() == "Order Code" || col.HeaderText.ToUpper() == "OrderCodeDescription")
                                {
                                    col.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            IList<Orders> ordersList = null;
                            drFillGrid = dtGridFill.NewRow();

                            // drFillGrid["Del"].Value = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                            for (int j = 0; j < sOrderCodeList.Length - 1; j++)
                            {
                                if (sOrderCodeList[j].Contains(strOrderCode[b].ToString()) == true)
                                {
                                    drFillGrid["Order Submit ID"] = sOrderCodeList[j].Split('-')[1].ToString();
                                }
                            }
                            for (int a = 0; a < ResultDto.Count; a++)
                            {
                                var ord = from s in ResultDto[a].Orders where s.Order_Submit_ID == Convert.ToUInt64(drFillGrid["Order Submit ID"]) && s.Lab_Procedure == strOrderCode[b].ToString() select s;
                                ordersList = ord.ToList<Orders>();
                            }
                            // sOrderSubmitIDList.Add((drFillGrid["Order Submit ID"] + "|" + ordersList[0].Lab_Procedure + ":" + ordersList[0].Lab_Procedure_Description));
                            if (hdnOrderSubmitID.Value == string.Empty)
                            {
                                hdnOrderSubmitID.Value = drFillGrid["Order Submit ID"] + "|" + ordersList[0].Lab_Procedure + ":" + ordersList[0].Lab_Procedure_Description;
                            }
                            else
                            {
                                hdnOrderSubmitID.Value += "@" + (drFillGrid["Order Submit ID"] + "|" + ordersList[0].Lab_Procedure + ":" + ordersList[0].Lab_Procedure_Description);
                            }
                            drFillGrid["ProcedureCode"] = ordersList[0].Lab_Procedure + "-" + ordersList[0].Lab_Procedure_Description;

                            //  OrderSubmitIDListForWorkflow.Add(drFillGrid["Order Submit ID"].ToString());
                            if (hdnOrderSubmitWorkflow.Value == string.Empty)
                            {
                                hdnOrderSubmitWorkflow.Value = drFillGrid["Order Submit ID"].ToString();
                            }
                            else
                            {
                                hdnOrderSubmitWorkflow.Value += "||" + drFillGrid["Order Submit ID"].ToString();
                            }

                            dtGridFill.Rows.Add(drFillGrid);

                        }
                        GridTableView tableViewOrders = new GridTableView();
                        GridGroupByExpression expression = new GridGroupByExpression();
                        GridGroupByField gridGroupByField = new GridGroupByField();
                        gridGroupByField = new GridGroupByField();
                        gridGroupByField.FieldName = "ProcedureCode";
                        gridGroupByField.HeaderText = "Procedure Code";
                        gridGroupByField.HeaderValueSeparator = " : ";
                        gridGroupByField.FormatString = "<strong>{0}</strong>";
                        hdnHeader.Value = drFillGrid["ProcedureCode"].ToString();


                        expression.SelectFields.Add(gridGroupByField);
                        expression.GroupByFields.Add(gridGroupByField);
                        tableViewOrders.GroupByExpressions.Add(expression);
                        this.grdSubComponent.MasterTableView.GroupByExpressions.Add(expression);

                    }
                    DisplayLabDetails();
                    this.grdSubComponent.MasterTableView.ToolTip = hdnHeader.Value;
                    grdSubComponent.DataSource = dtGridFill;
                    grdSubComponent.DataBind();
                    ViewState["DataSource"] = dtGridFill;
                }



            }
            for (int k = 0; k < grdSubComponent.Items.Count; k++)
            {
                RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[k].FindControl("Flag");
                objRadComboBox.DataSource = AddFlagColumn();
                objRadComboBox.DataBind();
            }

            //}

            //btnAdd.Enabled = false;
        }
        public void DisplayLabDetails()
        {
            PhysicianLibrary objPhysician = new PhysicianLibrary();
            IList<OrdersSubmit> orderSubmitList = new List<OrdersSubmit>();
            string[] separator = new string[] { "||" };
            IList<string> OrderSubmitIDListForWorkflow = new List<string>();
            string[] strID = hdnOrderSubmitWorkflow.Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string a in strID)
            {
                OrderSubmitIDListForWorkflow.Add(a.ToString());
            }
            IList<ResultEntryDTO> ResultDto = (IList<ResultEntryDTO>)ViewState["ResultDTO"];
            for (int i = 0; i < ResultDto.Count; i++)
            {
                for (int j = 0; j < OrderSubmitIDListForWorkflow.Distinct().ToList<string>().Count; j++)
                {
                    var query = from os in ResultDto[i].OrderSubmit where os.Id == Convert.ToUInt64(OrderSubmitIDListForWorkflow[j]) select os;
                    orderSubmitList = query.ToList<OrdersSubmit>();
                }
            }

            if (orderSubmitList.Count > 0)
            {
                if (orderSubmitList[0].Lab_Name != string.Empty)
                {
                    txtLabName.Text = orderSubmitList[0].Lab_Name;

                }
                if (orderSubmitList[0].Physician_ID != 0)
                {
                   // objPhysician = phyProxy.GetphysiciannameByPhyID(orderSubmitList[0].Physician_ID)[0];
                    objPhysician = GetPhysicianDetailsFromXml(Convert.ToUInt64(orderSubmitList[0].Physician_ID));

                    txtPhysicianName.Text = objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix;
                }
                if (orderSubmitList[0].Specimen_Collection_Date_And_Time.ToString("yyyy-mm-dd hh:mm:ss") != "0001-01-01 00:00:00" && orderSubmitList[0].Specimen_Collection_Date_And_Time.ToString("yyyy-mm-dd hh:mm:ss") != "0001-00-01 12:00:00")
                {
                    dtpDateCollected.Enabled = false;
                    string date = (orderSubmitList[0].Specimen_Collection_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                    dtpDateCollected.SelectedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(date));

                }
                else
                {
                    dtpDateCollected.Enabled = true;
                    string date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);

                }
            }
            else
            {
                dtpDateCollected.Enabled = true;
                string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
                strtime = Convert.ToDateTime(strtime).ToString("dd-MMM-yyyy hh:mm tt");
                dtpDateCollected.SelectedDate = UtilityManager.ConvertToLocal(Convert.ToDateTime(strtime));
            }
            if (orderSubmitList.Count > 0)
            {
                chkFasting.Enabled = false;
                if (orderSubmitList[0].Fasting == "Y")
                {
                    chkFasting.Checked = true;
                }
                else
                {
                    chkFasting.Checked = false;
                }
            }
            else
            {
                chkFasting.Enabled = true;
            }
        }
        IList<string> AddFlagColumn()
        {
          
            IList<string> FlagList = new List<string>() { " ", "Low", "High", "Very Low", "Very High", "<", ">", "Normal", "Abnormal", "Within range", "Out of range", "L", "H", "LL", "HH", "A" };
           
            IList<string> reportlist = new List<string> { "FINAL", "PRELIMINARY", "CORRECTED RESULTS" };
            if (cboReportStatus.SelectedIndex <= -1)
            {
               // IList<StaticLookup> staticlookupList = staticMngr.getStaticLookupByFieldName("REPORT STATUS");
                if (reportlist != null && reportlist.Count > 0)
                {
                    for (int i = 0; i < reportlist.Count; i++)
                    {
                        RadComboBoxItem cboItem = new RadComboBoxItem();
                        cboItem.Text = reportlist[i].ToString();
                        cboItem.ToolTip = reportlist[i].ToString();
                        cboReportStatus.Items.Add(cboItem);

                    }
                }
            }

            return FlagList;
        }
        private void FillTestOrderedTreeView()
        {
            grdTestOrdered.DataSource = null;
            IList<ResultEntryDTO> ResultDto = (IList<ResultEntryDTO>)ViewState["ResultDTO"];
            DataTable dtGridFillTestOrdered = CreateGridDataSourceForTestOrdered();
            grdTestOrdered.DataSource = dtGridFillTestOrdered;
            grdTestOrdered.DataBind();
            Dictionary<string, ResultEntryDTO> dResultEntry = new Dictionary<string, ResultEntryDTO>();

            DataRow dtGridFillTest = null;

            for (int i = 0; i < ResultDto.Count; i++)
            {

                for (int k = 0; k < ResultDto[i].OrderSubmit.Count; k++)
                {
                    string value = string.Empty;
                    if (ResultDto[i].OrderSubmit[k].Specimen_Collection_Date_And_Time.ToString("yyyy-MM-dd") != "0001-01-01")
                    {
                        value = ResultDto[i].OrderSubmit[k].Specimen_Collection_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                    }
                    else
                    {
                        value = ResultDto[i].OrderSubmit[k].Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm tt");
                    }

                    dtGridFillTest = dtGridFillTestOrdered.NewRow();
                    var query = from c in ResultDto[i].FileManagementIndex where c.Order_ID == ResultDto[i].OrderSubmit[k].Id select c;
                    IList<FileManagementIndex> FileManagementList = query.ToList<FileManagementIndex>();
                    if (FileManagementList.Count > 0)
                    {

                        dtGridFillTest["EnableImage"] = "TRUE";
                    }
                    else
                    {
                        dtGridFillTest["EnableImage"] = "FALSE";
                    }
                    dtGridFillTest["DateTime"] = UtilityManager.ConvertToLocal(Convert.ToDateTime(value));
                    dtGridFillTest["Lab"] = ResultDto[i].OrderSubmit[k].Lab_Name;


                    var res = from s in ResultDto[i].ResultMasterList where s.Order_ID == ResultDto[i].OrderSubmit[k].Id select s;
                    IList<ResultMaster> resultMasterList = res.ToList<ResultMaster>();

                    if (resultMasterList.Count > 0)
                    {
                        string sTag = string.Empty;
                        if (dtGridFillTest != null)
                        {
                            sTag = "ResultMasterID" + resultMasterList[0].Id.ToString() + ",";
                            dtGridFillTest["Tag"] += sTag;
                        }
                    }
                    else
                    {
                        var ord = from s in ResultDto[i].Orders where s.Order_Submit_ID == ResultDto[i].OrderSubmit[k].Id select s;
                        IList<Orders> ordersList = ord.ToList<Orders>();

                        string sTag = string.Empty;
                        for (int j = 0; j < ordersList.Count; j++)
                        {
                            if (ordersList[j].Lab_Procedure != string.Empty)
                            {
                                sTag += "OrderCode" + ordersList[j].Lab_Procedure + "-" + ordersList[j].Order_Submit_ID.ToString() + ",";
                            }
                            else
                            {
                                sTag += "BlankOrder" + ordersList[j].Order_Submit_ID.ToString() + ",";
                            }
                        }
                        if (dtGridFillTest != null)
                            dtGridFillTest["Tag"] += sTag;

                    }
                    try
                    {
                        dtGridFillTestOrdered.Rows.Add(dtGridFillTest);
                    }
                    catch
                    {
                    }
                }
                if (i == 0)
                {
                    if (ResultDto[i].ResultOBRList != null)
                    {
                        for (int j = 0; j < ResultDto[i].ResultOBRList.Count; j++)
                        {

                            string sCollectionDate = ResultDto[i].ResultOBRList[j].OBR_Specimen_Collection_Date_And_Time;

                            string formatString = "yyyyMMddHHmmss";
                            string sample = sCollectionDate;
                            if (sample.Length == 14)
                                formatString = "yyyyMMddHHmmss";
                            else if (sample.Length == 12)
                                formatString = "yyyyMMddHHmm";
                            DateTime dt = UtilityManager.ConvertToLocal(DateTime.ParseExact(sample, formatString, null));
                            string sDate = dt.ToString("dd-MMM-yyyy hh:mm tt").ToString();
                            if (ResultDto != null && ResultDto.Count > 0)
                            {
                                var query = from c in ResultDto[i].ResultOBRList where ResultDto[i].ResultZPSList[j].Result_Master_ID == c.Result_Master_ID select ResultDto[i].ResultZPSList[j].ZPS_Facility_Name;
                                string sLabName = query.ToList<string>()[0];
                                if (dResultEntry.ContainsKey(sDate + " - " + sLabName) == false)
                                {
                                    dtGridFillTest = dtGridFillTestOrdered.NewRow();
                                    // dtGridFillTest["Image"] = global::Acurus.Capella.UI.Properties.Resources.Show_Image_Disable;
                                    dtGridFillTest["DateTime"] = dt;



                                    dtGridFillTest["Lab"] = sLabName;

                                    dtGridFillTest["Tag"] = "ResultMasterID" + ResultDto[i].ResultOBRList[j].Result_Master_ID.ToString();

                                    dtGridFillTestOrdered.Rows.Add(dtGridFillTest);
                                    dResultEntry.Add(sDate + " - " + sLabName, ResultDto[i]);
                                }
                            }
                        }
                    }
                }
                DataView dv = dtGridFillTestOrdered.DefaultView;
                dv.Sort = "DateTime Desc";
                DataTable sortedDT = dv.ToTable();

                grdTestOrdered.DataSource = sortedDT;
                grdTestOrdered.DataBind();
            }
            foreach (GridDataItem item in grdTestOrdered.Items)
            {
                if (item.Cells[6].Text.ToString() == "TRUE")
                {
                    TableCell selectCell = item["Image"];
                    ImageButton gd = (ImageButton)selectCell.Controls[0];
                    gd.ImageUrl = "~/Resources/blue_open.png";
                }
                else
                {
                    TableCell selectCell = item["Image"];
                    ImageButton gd = (ImageButton)selectCell.Controls[0];
                    gd.ImageUrl = "~/Resources/Show_Image_Disable.jpg";
                }




            }
        }

        protected void grdSubComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

        }

        protected void grdTestOrdered_ItemCommand(object sender, GridCommandEventArgs e)
        {
               
            if (hdnIsRowClick.Value != string.Empty)
            {
                btnAdd.Enabled = true;
                btnSaveandMovetonextprocess.Enabled = true;
                return;
            }
            else
            {
                FillValuesintoGrid();
                btnSaveandMovetonextprocess.Enabled = true;
            }
            //iPreviousRow = e.RowIndex;
            //SameRowindex = e.RowIndex;
            //}
            IList<ResultEntryDTO> ResultDto = (IList<ResultEntryDTO>)ViewState["ResultDTO"];
            if (e.CommandName == "Image")
            {
                IList<string> strOrderSubmitID = new List<string>();
                if (e.Item.Cells[5].Text.ToString().Contains("BlankOrder") == true)
                {
                    string[] sOrderCodeList = e.Item.Cells[5].Text.ToString().Replace("BlankOrder", "").Split(',');
                    for (int k = 0; k < sOrderCodeList.Length - 1; k++)
                    {
                        strOrderSubmitID.Add(sOrderCodeList[k].ToString());
                    }
                }
                else if (e.Item.Cells[5].Text.ToString().Contains("OrderCode") == true)
                {
                    string[] sOrderCodeList = e.Item.Cells[5].Text.ToString().Replace("OrderCode", "").Split(',');


                    for (int k = 0; k < sOrderCodeList.Length; k++)
                    {

                        strOrderSubmitID.Add(sOrderCodeList[0].Split('-')[1]);
                    }
                }
                else if (e.Item.Cells[5].Text.ToString().Contains("ResultMasterID") == true)
                {
                    if (ResultDto.Count > 0)
                    {
                        string[] sResultCodeList = e.Item.Cells[5].Text.ToString().Replace("ResultMasterID", "").Split(',');
                        for (int j = 0; j < sResultCodeList.Length - 1; j++)
                        {
                            for (int k = 0; k < ResultDto[j].ResultMasterList.Count; k++)
                            {
                                var res = from s in ResultDto[j].ResultMasterList where s.Id == Convert.ToUInt64(sResultCodeList[0]) select s;
                                IList<ResultMaster> resultMasterList = res.ToList<ResultMaster>();
                                if (resultMasterList.Count > 0)
                                {
                                    strOrderSubmitID.Add(resultMasterList[0].Order_ID.ToString());
                                }
                            }

                        }
                    }
                }
                for (int i = 0; i < strOrderSubmitID.Distinct().ToList<string>().Count; i++)
                {
                    if (ResultDto.Count > 0)
                    {
                        IList<FileManagementIndex> FileManagementList = new List<FileManagementIndex>();
                        for (int j = 0; j < ResultDto.Count; j++)
                        {
                            var query = from c in ResultDto[j].FileManagementIndex where c.Order_ID == Convert.ToUInt64(strOrderSubmitID[i]) select c;
                            FileManagementList = query.ToList<FileManagementIndex>();
                        }



                        if (FileManagementList.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenViewIndexedImage('" + FileManagementList[0].File_Path.Replace("#", "HASHSYMBOL") + "','" + hdnHumanID.Value + "');", true);
                            //frmViewIndexedImages frmView = new frmViewIndexedImages(FileManagementList[0], ulHumanID, false, string.Empty, string.Empty);
                            //frmView.Show();
                            //frmView.BringToFront();
                        }
                    }
                }

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);  
        }

        protected void grdTestOrdered_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

        }

        protected void txtLabName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {           
            IList<ResultEntryDTO> ResultDto = new List<ResultEntryDTO>();
            string YesNoCancel = hdnMessageType.Value;
            string IsRowClick = hdnIsRowClick.Value;
            hdnIsRowClick.Value = string.Empty;
            hdnMessageType.Value = string.Empty;
            if (grdSubComponent.Items.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200003'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);             
                btnAdd.Enabled = true;
                btnSaveandMovetonextprocess.Enabled = true;
                SaveAndMoveClick.Value = "TRUE";
                return;
            }
            if (txtPhysicianName.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200025'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);             
                txtPhysicianName.Focus();
                btnAdd.Enabled = true;
                btnSaveandMovetonextprocess.Enabled = true;
                SaveAndMoveClick.Value = "TRUE";
                return;

            }
            if (dtpDateCollected.DateInput.SelectedDate == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200026'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                dtpDateCollected.Focus();
                btnAdd.Enabled = true;
                btnSaveandMovetonextprocess.Enabled = true;
                SaveAndMoveClick.Value = "TRUE";
                return;
            }
            if (dtpDateReported.DateInput.SelectedDate == null)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200041'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("7200026", this.Text);
                dtpDateReported.Focus();
                // bSave = true;
                return;
            }
            for (int i = 0; i < grdSubComponent.Items.Count; i++)
            {
                if (((RadTextBox)grdSubComponent.Items[i].FindControl("Test")).Text == string.Empty)
                {

                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200020'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);                
                    grdSubComponent.Focus();
                    return;
                }
            }
            if (cboReportStatus.Text == string.Empty && cboReportStatus.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200031'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                // ApplicationObject.erroHandler.DisplayErrorMessage("7200031", this.Text);
                //  bSave = true;
                return;
            }
            if (dtpDateCollected.SelectedDate < Convert.ToDateTime(hdnBirthDate.Value))
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200033'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }

            if (dtpDateReported.SelectedDate < Convert.ToDateTime(hdnBirthDate.Value) && dtpDateReported.SelectedDate != dtpDateReported.MinDate)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200037'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            if (btnAdd.ToolTip == "&Save")
            {               
                ResultDto = SaveResultMaster();              
            }
            else
            {
                ResultDto = UpdateResultMaster();              
            }
            ClearAll();

            //ResultDto = objResultMasterProxy.GetResultEntry(Convert.ToUInt64(Request["MyHumanID"]));  
            ResultDto[0].FileManagementIndex = (IList<FileManagementIndex>)ViewState["FileIndex"];
            ViewState["ResultDTO"] = ResultDto;
            FillTestOrderedTreeView();
            FillValuesintoGrid();            
            if (hdnMoveToNext.Value.ToUpper() == "TRUE")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7200005'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                hdnMoveToNext.Value = "";
            }
            else if (IsRowClick == "true")
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200039'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                FillGrid();
            }
            else if (YesNoCancel != "Yes" || (hdnOpenningMode.Value != string.Empty && YesNoCancel == "Yes"))
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('7200039'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MRE", "CancelYesNo(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            btnAdd.Enabled = false;
            SaveAndMoveClick.Value = "FALSE";      
            ClearAll();          
        }

        protected void grdSubComponent_ItemCreated(object sender, GridItemEventArgs e)
        {
            e.Item.ToolTip = "";
            if (e.Item is GridDataItem)
            {
                GridDataItem gridItem = e.Item as GridDataItem;
                foreach (GridColumn column in grdSubComponent.MasterTableView.RenderColumns)
                {
                    if (column.UniqueName == "Del")
                    {
                        gridItem[column.UniqueName].ToolTip = "Delete";
                    }


                }
            }
        }

        protected void grdSubComponent_ItemUpdated(object sender, GridUpdatedEventArgs e)
        {
            int index = e.Item.RowIndex;
            GridItem row = grdSubComponent.Items[index];

            // Get the controls that contain the updated values. In this
            // example, the updated values are contained in the TextBox 
            // controls declared in the edit item templates of each TemplateField 
            // column fields in the GridView control.
            RadTextBox lastName = (RadTextBox)row.FindControl("Result");
            // RadTextBox firstName = (RadTextBox)row.FindControl("FirstNameTextBox");

            // Add the updated values to the NewValues dictionary. Use the
            // parameter names declared in the parameterized update query 
            // string for the key names.
            e.Item.UpdateValues("Result");
            // e.NewValues["au_fname"] = firstName.Text;

        }

        protected void grdSubComponent_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Add":
                    {
                        if ((e.Item is GridEditFormInsertItem) && e.Item.IsInEditMode)
                        {
                            GridEditFormInsertItem item = (GridEditFormInsertItem)e.Item;
                            TextBox txt = item["OrderSubmitID"].Controls[0] as TextBox;  //use findControl to access template columns
                        }
                        grdSubComponent.Rebind();
                        break;
                    }
            }
            IList<ResultEntryDTO> OrderAndSubmitList = new List<ResultEntryDTO>();

            if (e.CommandName == "Del")
            {
                IList<PatientResults> DeletePatientResults = null;
                IList<ResultOBX> DeleteResultOBXlst = null;
                if (ViewState["OBXDEL"] == null)
                {
                    DeleteResultOBXlst = new List<ResultOBX>();
                }
                else
                {
                    DeleteResultOBXlst = (IList<ResultOBX>)ViewState["OBXDEL"];

                }
                if (ViewState["PATDEL"] == null)
                {
                    DeletePatientResults = new List<PatientResults>();
                }
                else
                {
                    DeletePatientResults = (IList<PatientResults>)ViewState["PATDEL"];
                }
                //int iMsg = ApplicationObject.erroHandler.DisplayErrorMessage("7200000", this.Text);

                //if (iMsg == 1)
                //{
                IList<ResultOBX> OBXTagList = new List<ResultOBX>();
                if (ViewState["OBR"] != null)
                {
                    for (int i = 0; i < ((IList<ResultOBR>)ViewState["OBR"]).Count; i++)
                    {
                        var sProcedureNames = from c in ((IList<ResultOBX>)ViewState["OBX"]) where ((IList<ResultOBR>)ViewState["OBR"])[i].Id == c.Result_OBR_ID select c;
                        IList<ResultOBX> OBXList = sProcedureNames.ToList<ResultOBX>();
                        for (int j = 0; j < OBXList.Count; j++)
                        {
                            OBXTagList.Add(OBXList[j]);
                        }
                    }
                }
                if (grdTestOrdered.SelectedItems.Count > 0)
                {
                    if (grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Contains("ResultMasterID") == true)
                    {
                        string[] sResultMasterID = grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Replace("ResultMasterID", " ").Split(',');
                        ulong[] ulResultMasterIDList = new ulong[sResultMasterID.Length - 1];
                        for (int j = 0; j < sResultMasterID.Length; j++)
                        {
                            ulong ulResultMasterID = Convert.ToUInt64(sResultMasterID[j].ToString().Replace("ResultMasterID", ""));
                            OrderAndSubmitList = objResultMasterProxy.GetListOfResultsFromResultMasterID(ulResultMasterID, Convert.ToUInt64(Request["MyHumanID"]));    
                        }
                    }
                }
                ResultOBX objdelOBX = null;
                if (grdSubComponent.Items[Convert.ToInt32(e.CommandArgument)].Cells[21].Text.ToString() != string.Empty)
                    objdelOBX = OBXTagList.Where(a => a.Id == Convert.ToUInt64(grdSubComponent.Items[Convert.ToInt32(e.CommandArgument)].Cells[21].Text)).ToList<ResultOBX>()[0];
                if (objdelOBX != null)
                {
                    string DeletedText = ((RadTextBox)grdSubComponent.Items[Convert.ToInt32(e.CommandArgument)].FindControl("Test")).Text;// grdSubComponent.Items[e.Item.ItemIndex].Cells[4].Text.ToString();
                    for (int i = 0; i < OrderAndSubmitList.Count; i++)
                    {
                        var DelList = from p in OrderAndSubmitList[i].PatientResultList where p.Result_OBX_ID == Convert.ToUInt64(grdSubComponent.Items[Convert.ToInt32(e.CommandArgument)].Cells[21].Text.ToString()) select p;
                        if (DelList.ToList<PatientResults>().Count > 0)
                        {
                            DeletePatientResults.Add(DelList.ToList<PatientResults>()[0]);

                        }
                    }
                    //GridEditableItem item = e.Item as GridEditableItem;
                    //e.Item.OwnerTableView.PerformDelete(item, true);
                    //DeleteResultOBXlst.Add(objdelOBX);
                    //ViewState["PATDEL"] = DeletePatientResults;
                    //ViewState["OBXDEL"] = DeleteResultOBXlst;
                }

                string ID = e.Item.OwnerTableView.DataKeyValues[Convert.ToInt32(e.CommandArgument)]["Test"].ToString();
                if (ID != "")
                {

                    DataTable table = (DataTable)ViewState["DataSource"];
                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = table.Columns["Test"];
                    table.PrimaryKey = keyColumns;
                    if (table.Rows.Find(ID) != null)
                    {
                        table.Rows.Find(ID).Delete();
                        table.AcceptChanges();
                        ViewState["DataSource"] = table;
                    }

                }
                else
                {

                    DataTable table = (DataTable)ViewState["DataSource"];
                    DataColumn[] keyColumns = new DataColumn[1];
                    table.DefaultView.Sort = "ProcedureCode";

                    DataView dv = table.DefaultView;
                    dv.Sort = "ProcedureCode Asc";
                    DataTable sortedDT = dv.ToTable();

                    int index = Convert.ToInt32(e.CommandArgument);
                    sortedDT.Rows[index].Delete();
                    sortedDT.AcceptChanges();

                    ViewState["DataSource"] = sortedDT;

                }

                btnAdd.Enabled = true;
                if (btnAdd.ToolTip == "Sa&ve")
                {
                    btnAdd.ToolTip = "Sa&ve";
                }


                grdSubComponent.DataSource = (DataTable)ViewState["DataSource"]; ;
                grdSubComponent.DataBind();

                DeleteResultOBXlst.Add(objdelOBX);
                ViewState["PATDEL"] = DeletePatientResults;
                ViewState["OBXDEL"] = DeleteResultOBXlst;

                for (int j = 0; j < grdSubComponent.Items.Count; j++)
                {
                    RadComboBox objCombo = ((RadComboBox)grdSubComponent.Items[j].FindControl("Flag"));

                    objCombo.DataSource = AddFlagColumn();
                    objCombo.DataBind();
                    DataTable dt = (DataTable)ViewState["DataSource"];
                    if (dt.Rows[j]["Flag"].ToString() != string.Empty)
                    {
                        objCombo.SelectedIndex = objCombo.FindItemByText(dt.Rows[j]["Flag"].ToString()).Index;
                        objCombo.SelectedValue = objCombo.Items[objCombo.SelectedIndex].Text;
                    }
                }

                // grdSubComponent.Rows.RemoveAt(e.RowIndex);

                // }
            }
        }

        protected void pbTestOrdered_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnSelectedItem.Value != string.Empty)
            {
                // sOrderSubmitIDList.Clear();
                // grdTestOrdered.CurrentRow = null;
                ClearAll();
                hdnOrderSubmitID.Value = string.Empty;
                DataTable dtGridFill = CreateGridDataSource();
                grdSubComponent.DataSource = dtGridFill;

                cboReportStatus.Text = "FINAL";
                //HumanManager humanMngr = new HumanManager();
                //Human HumanRecord = humanMngr.GetPatientDetailsUsingPatientInformattion(Convert.ToUInt64(Request["MyHumanID"]))[0];
                ulong ulPhysicianId = 0;
                if (hdnPhyID.Value != string.Empty)
                {
                    ulPhysicianId = Convert.ToUInt64(hdnPhyID.Value.ToString());
                }
              //  PhysicianLibrary objPhysician = phyProxy.GetphysiciannameByPhyID(ulPhysicianId)[0];
                PhysicianLibrary objPhysician = GetPhysicianDetailsFromXml(ulPhysicianId);
                txtLabName.Text = hdnLabName.Value.ToString();
                IList<string> sOrderSubmitIDList = new List<string>();
                ulLabID.Value = hdnLabID.Value;
                txtPhysicianName.Text = objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix;
                IList<string> sOrderSubmitIDforMove = new List<string>();
                IList<string> SearchOrdersList = new List<string>();
                if (hdnSelectedItem.Value != string.Empty)
                {
                    string[] objSearch = hdnSelectedItem.Value.Split('|');
                    for (int i = 0; i < objSearch.Length; i++)
                    {
                        SearchOrdersList.Add(objSearch[i].ToString());
                    }
                }
                string[] seprator = new string[1] { "  ,  " };
                IList<string> strOrderCode = new List<string>();
                string sLabProcedure = string.Empty;
                DataRow drFillGrid = null;

                int iAgeDiff = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(hdnBirthDate.Value).Year);
                for (int j = 0; j < SearchOrdersList.Count; j++)
                {
                    strOrderCode = new List<string>();
                    strOrderCode.Add(SearchOrdersList[j].Split('-')[0].ToString());
                    for (int k = 0; k < strOrderCode.Count; k++)
                    {
                        ArrayList arryLst = acurusResultProxy.GetAcurusResultMappingListForMRE(strOrderCode[k].ToString(), Gender.Value.ToUpper(), iAgeDiff, Convert.ToInt32(ulLabID.Value));

                        if (arryLst != null && arryLst.Count > 0)
                        {
                            for (int i = 0; i < arryLst.Count; i++)
                            {
                                object[] objlst = (object[])arryLst[i];
                                drFillGrid = dtGridFill.NewRow();
                                // drFillGrid["Del"] = 
                                if (objlst[2] != null)
                                    drFillGrid["Units"] = objlst[2].ToString();
                                if (objlst[3] != null)
                                    drFillGrid["Refrange"] = objlst[3].ToString();
                                drFillGrid["Test code"] = objlst[0].ToString();
                                drFillGrid["Test"] = objlst[1].ToString();
                                drFillGrid["Order Code"] = objlst[6].ToString();
                                drFillGrid["OrderCodeDescription"] = objlst[7].ToString();
                                drFillGrid["Acurus Result Code"] = objlst[4].ToString();
                                drFillGrid["Acurus Result Description"] = objlst[5].ToString();
                                drFillGrid["ProcedureCode"] = objlst[6].ToString() + "-" + objlst[7].ToString();
                                drFillGrid["Order Submit ID"] = "0";                               
                                if (hdnOrderSubmitID.Value == string.Empty)
                                {
                                    hdnOrderSubmitID.Value = drFillGrid["Order Submit ID"].ToString() + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                }
                                else
                                {
                                    hdnOrderSubmitID.Value += "@" + drFillGrid["Order Submit ID"].ToString() + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                }
                                //sOrderSubmitIDList.Add(drFillGrid["Order Submit ID"].ToString() + "|" + objlst[6].ToString() + ":" + objlst[7].ToString());
                                dtGridFill.Rows.Add(drFillGrid);
                            }

                        }

                        else
                        {
                            drFillGrid = dtGridFill.NewRow();                            
                            drFillGrid["ProcedureCode"] = SearchOrdersList[j].ToString();
                            drFillGrid["Order Submit ID"] = "0";                          
                            if (hdnOrderSubmitID.Value == string.Empty)
                            {
                                hdnOrderSubmitID.Value = drFillGrid["Order Submit ID"].ToString() + "|" + SearchOrdersList[j].ToString().Replace("-", ":");
                            }
                            else
                            {
                                hdnOrderSubmitID.Value += "@" + drFillGrid["Order Submit ID"].ToString() + "|" + SearchOrdersList[j].ToString().Replace("-", ":");
                            }                         
                            hdnHeader.Value = drFillGrid["ProcedureCode"].ToString();
                            dtGridFill.Rows.Add(drFillGrid);
                        }

                        GridTableView tableViewOrders = new GridTableView();
                        GridGroupByExpression expression = new GridGroupByExpression();
                        GridGroupByField gridGroupByField = new GridGroupByField();
                        gridGroupByField = new GridGroupByField();
                        gridGroupByField.FieldName = "ProcedureCode";
                        gridGroupByField.HeaderText = "Procedure Code";
                        gridGroupByField.HeaderValueSeparator = " : ";
                        gridGroupByField.FormatString = "<strong>{0}</strong>";


                        expression.SelectFields.Add(gridGroupByField);
                        expression.GroupByFields.Add(gridGroupByField);
                        tableViewOrders.GroupByExpressions.Add(expression);
                        this.grdSubComponent.MasterTableView.GroupByExpressions.Add(expression);

                        this.grdSubComponent.MasterTableView.ToolTip = hdnHeader.Value;
                        grdSubComponent.DataSource = dtGridFill;
                        grdSubComponent.DataBind();
                        ViewState["DataSource"] = dtGridFill;
                        for (int h = 0; h < grdSubComponent.Items.Count; h++)
                        {
                            RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[h].FindControl("Flag");
                            objRadComboBox.DataSource = AddFlagColumn();
                            objRadComboBox.DataBind();
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);  
        }


        protected void btnSaveandMovetonextprocess_Click(object sender, EventArgs e)
        {            
            SaveAndMoveClick.Value = "TRUE";
            hdnMoveToNext.Value = "TRUE";
            IList<string> sOrderSubmitIDforMove = new List<string>();

            btnAdd_Click(sender, e);
            //if (bSave == true)
            //{
            //SaveAndMoveClick.Value = "FALSE";
            //return;
            //}
            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('7200005');", true);

            
            IList<ResultEntryDTO> ResultDto = new List<ResultEntryDTO>();
            if (Request["MyHumanID"] != null)
            {
                ResultDto = objResultMasterProxy.GetResultEntry(Convert.ToUInt64(Request["MyHumanID"]),"Load");
                FillTestOrderedTreeView();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);  
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearAll();
            grdTestOrdered.SelectedIndexes.Clear();
            DataTable dt = new DataTable();
            grdSubComponent.DataSource = dt;
            grdSubComponent.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);  
        }
    

        public int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            if (hdnSelectedItem.Value != string.Empty)
            {
                // sOrderSubmitIDList.Clear();
                // grdTestOrdered.CurrentRow = null;
                ClearAll();
                hdnOrderSubmitID.Value = string.Empty;
                DataTable dtGridFill = CreateGridDataSource();
                grdSubComponent.DataSource = dtGridFill;
                IList<string> strOrderSubmitID = new List<string>();
                cboReportStatus.Text = "FINAL";

                ulong ulPhysicianId = 0;
                if (hdnPhyID.Value != string.Empty)
                {
                    ulPhysicianId = Convert.ToUInt64(hdnPhyID.Value.ToString());
                }
               // PhysicianLibrary objPhysician = phyProxy.GetphysiciannameByPhyID(ulPhysicianId)[0];
                PhysicianLibrary objPhysician = GetPhysicianDetailsFromXml(Convert.ToUInt64(ulPhysicianId));
                txtLabName.Text = hdnLabName.Value.ToString();
                IList<string> sOrderSubmitIDList = new List<string>();
                ulLabID.Value = hdnLabID.Value;
                txtPhysicianName.Text = objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix;
                IList<string> sOrderSubmitIDforMove = new List<string>();
                IList<string> SearchOrdersList = new List<string>();
                string[] sOrderCodeList = new string[] { };
                //code modified by balaji
                if (grdSubComponent.SelectedItems.Count > 0 || (grdTestOrdered.SelectedItems.Count > 0 && grdTestOrdered.SelectedItems[0].Cells[5].Text.Contains("BlankOrder")))//||grdSubComponent.Items.Count==0)
                    sOrderCodeList = grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Replace("BlankOrder", "").Split(',');
                for (int k = 0; k < sOrderCodeList.Length - 1; k++)
                {
                    strOrderSubmitID.Add(sOrderCodeList[k].ToString());
                }
                if (hdnSelectedItem.Value != string.Empty)
                {
                    string[] objSearch = hdnSelectedItem.Value.Split('|');
                    for (int i = 0; i < objSearch.Length; i++)
                    {
                        SearchOrdersList.Add(objSearch[i].ToString());
                    }
                }
                string[] seprator = new string[1] { "  ,  " };
                IList<string> strOrderCode = new List<string>();
                string sLabProcedure = string.Empty;
                DataRow drGridfill = null;


                int iAgeDiff = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(hdnBirthDate.Value).Year);
                for (int j = 0; j < SearchOrdersList.Count; j++)
                {
                    strOrderCode = new List<string>();
                    strOrderCode.Add(SearchOrdersList[j].Split('-')[0].ToString());
                    for (int m = 0; m < strOrderCode.Count; m++)
                    {
                        ArrayList arryLst = acurusResultProxy.GetAcurusResultMappingListForMRE(strOrderCode[m].ToString(), Gender.Value.ToUpper(), iAgeDiff, Convert.ToInt32(ulLabID.Value));

                        if (arryLst != null && arryLst.Count > 0)
                        {
                            for (int i = 0; i < arryLst.Count; i++)
                            {
                                object[] objlst = (object[])arryLst[i];
                                drGridfill = dtGridFill.NewRow();
                                //  drGridfill["Del"] = 
                                if (objlst[2] != null)
                                    drGridfill["Units"] = objlst[2].ToString();
                                if (objlst[3] != null)
                                    drGridfill["Refrange"] = objlst[3].ToString();
                                drGridfill["Test code"] = objlst[0].ToString();
                                drGridfill["Order Code"] = objlst[6].ToString();
                                drGridfill["OrderCodeDescription"] = objlst[7].ToString();
                                drGridfill["Test"] = objlst[1].ToString();
                                drGridfill["Acurus Result Code"] = objlst[4].ToString();
                                drGridfill["Acurus Result Description"] = objlst[5].ToString();
                                drGridfill["ProcedureCode"] = objlst[6].ToString() + "-" + objlst[7].ToString();
                                hdnHeader.Value = drGridfill["ProcedureCode"].ToString();
                                if (grdTestOrdered.SelectedItems.Count > 0)
                                {
                                    if (grdTestOrdered.SelectedItems[0].Cells[5].Text.ToString().Contains("BlankOrder") == true)
                                    {
                                        // string[] sOrderCodeList = grdTestOrdered.SelectedRows[0].Tag.ToString().Replace("BlankOrder", "").Split(',');
                                        for (int k = 0; k < sOrderCodeList.Length - 1; k++)
                                        {
                                            strOrderSubmitID.Add(sOrderCodeList[k].ToString());
                                        }
                                    }
                                }
                                if (strOrderSubmitID.Count > 0)
                                {
                                    for (int l = 0; l < strOrderSubmitID.Distinct().ToList<string>().Count; l++)
                                    {
                                        drGridfill["Order Submit ID"] = strOrderSubmitID[l].ToString();
                                    }
                                }
                                else
                                {
                                    drGridfill["Order Submit ID"] = "0";
                                }
                                // sOrderSubmitIDList.Add(drGridfill["Order Submit ID"].ToString() + "|" + objlst[6].ToString() + ":" + objlst[7].ToString());
                                if (hdnOrderSubmitID.Value == string.Empty)
                                {
                                    hdnOrderSubmitID.Value = drGridfill["Order Submit ID"].ToString() + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                }
                                else
                                {
                                    hdnOrderSubmitID.Value += "@" + drGridfill["Order Submit ID"].ToString() + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                }
                                // OrderSubmitIDListForWorkflow.Add(drGridfill["Order Submit ID"].ToString());
                                if (hdnOrderSubmitWorkflow.Value == string.Empty)
                                {
                                    hdnOrderSubmitWorkflow.Value = drGridfill["Order Submit ID"].ToString();
                                }
                                else
                                {
                                    hdnOrderSubmitWorkflow.Value += "||" + drGridfill["Order Submit ID"].ToString();
                                }
                                dtGridFill.Rows.Add(drGridfill);
                            }

                        }

                        else
                        {
                            drGridfill = dtGridFill.NewRow();

                            // row.Cells["Del"].Value = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                            drGridfill["ProcedureCode"] = SearchOrdersList[j].ToString();
                            hdnHeader.Value = drGridfill["ProcedureCode"].ToString();
                            if (grdTestOrdered.SelectedItems.Count > 0)
                            {
                                if (grdTestOrdered.SelectedItems[0].Cells[5].ToString().Contains("BlankOrder") == true)
                                {
                                    // string[] sOrderCodeList = grdTestOrdered.SelectedRows[0].Tag.ToString().Replace("BlankOrder", "").Split(',');
                                    for (int k = 0; k < sOrderCodeList.Length - 1; k++)
                                    {
                                        strOrderSubmitID.Add(sOrderCodeList[k].ToString());
                                    }
                                }
                            }
                            if (strOrderSubmitID.Count > 0)
                            {
                                for (int l = 0; l < strOrderSubmitID.Distinct().ToList<string>().Count; l++)
                                {
                                    drGridfill["Order Submit ID"] = strOrderSubmitID[l].ToString();
                                }
                            }
                            else
                            {
                                drGridfill["Order Submit ID"] = "0";
                            }
                            //sOrderSubmitIDList.Add(drGridfill["Order Submit ID"].ToString() + "|" + SearchOrdersList[j].ToString().Replace("-", ":"));
                            if (hdnOrderSubmitID.Value == string.Empty)
                            {
                                hdnOrderSubmitID.Value = drGridfill["Order Submit ID"].ToString() + "|" + SearchOrdersList[j].ToString().Replace("-", ":");
                            }
                            else
                            {
                                hdnOrderSubmitID.Value += "@" + drGridfill["Order Submit ID"].ToString() + "|" + SearchOrdersList[j].ToString().Replace("-", ":");
                            }
                            // OrderSubmitIDListForWorkflow.Add(drGridfill["Order Submit ID"].ToString());
                            if (hdnOrderSubmitWorkflow.Value == string.Empty)
                            {
                                hdnOrderSubmitWorkflow.Value = drGridfill["Order Submit ID"].ToString();
                            }
                            else
                            {
                                hdnOrderSubmitWorkflow.Value += "||" + drGridfill["Order Submit ID"].ToString();
                            }

                            dtGridFill.Rows.Add(drGridfill);
                        }
                        GridTableView tableViewOrders = new GridTableView();
                        GridGroupByExpression expression = new GridGroupByExpression();
                        GridGroupByField gridGroupByField = new GridGroupByField();
                        gridGroupByField = new GridGroupByField();
                        gridGroupByField.FieldName = "ProcedureCode";
                        gridGroupByField.HeaderText = "Procedure Code";
                        gridGroupByField.HeaderValueSeparator = " : ";
                        gridGroupByField.FormatString = "<strong>{0}</strong>";


                        expression.SelectFields.Add(gridGroupByField);
                        expression.GroupByFields.Add(gridGroupByField);
                        tableViewOrders.GroupByExpressions.Add(expression);
                        this.grdSubComponent.MasterTableView.GroupByExpressions.Add(expression);
                    }


                }
              //  DisplayLabDetails();
                this.grdSubComponent.MasterTableView.ToolTip = hdnHeader.Value;

                grdSubComponent.DataSource = dtGridFill;
                grdSubComponent.DataBind();
                ViewState["DataSource"] = dtGridFill;

            }
            for (int k = 0; k < grdSubComponent.Items.Count; k++)
            {
                RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[k].FindControl("Flag");
                objRadComboBox.DataSource = AddFlagColumn();
                objRadComboBox.DataBind();
            }

        }

        protected void btnAddNewRow_Click(object sender, EventArgs e)
        {
            if (grdSubComponent.SelectedItems.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage(7200038); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            
            //Added for avoid refresh grid selected values
            DataTable dtGridFill = CreateGridDataSource();
            for(int i=0;i<grdSubComponent.Items.Count;i++)
            {
                GridDataItem dataItem = (GridDataItem)grdSubComponent.Items[i];
                DataRow drRow = dtGridFill.NewRow();
                drRow["Test"] = ((RadTextBox)grdSubComponent.Items[i].FindControl("Test")).Text;
                drRow["Result"] = ((RadTextBox)grdSubComponent.Items[i].FindControl("Result")).Text;
                drRow["Flag"] = ((RadComboBox)grdSubComponent.Items[i].FindControl("Flag")).SelectedItem.Text;
                drRow["Units"] = ((RadTextBox)grdSubComponent.Items[i].FindControl("Units")).Text;
                drRow["Refrange"] = ((RadTextBox)grdSubComponent.Items[i].FindControl("Refrange")).Text;
                drRow["NTE"] = dataItem["NTE"].Text;
                drRow["Source"] = dataItem["Source"].Text;
                drRow["Order Submit ID"] = dataItem["OrderSubmitID"].Text;
                drRow["Current Process"] = dataItem["CurrentProcess"].Text;
                drRow["Test code"] = dataItem["Testcode"].Text;
                drRow["Acurus Result Code"] = dataItem["AcurusResultCode"].Text;
                drRow["Acurus Result Description"] = dataItem["AcurusResultDescription"].Text;
                drRow["ProcedureCode"] = dataItem["ProcedureCode"].Text;
                drRow["SNo"] = dataItem["SNo"].Text;
                drRow["OrderCodeDescription"] = dataItem["OrderCodeDescription"].Text;
                drRow["Order Code"] = dataItem["OrderCode"].Text;
                drRow["Notes"] = ((RadTextBox)grdSubComponent.Items[i].FindControl("Notes")).Text;
                drRow["OBX ID"] = dataItem["OBXID"].Text;

                dtGridFill.Rows.Add(drRow);
            }
            //End
            
            //DataTable dtGridFill = ((DataTable)ViewState["DataSource"]);                     
            //DataRow drRow = dtGridFill.NewRow();
            DataRow drRow1 = dtGridFill.NewRow();

            drRow1["ProcedureCode"] = hdnSelectedPanel.Value.ToString();
            if (hdnAddNewOrderSubmitID.Value!=string.Empty)
                drRow1["Order Submit ID"] = hdnAddNewOrderSubmitID.Value.ToString();
            else
                drRow1["Order Submit ID"] = "0";
            dtGridFill.Rows.Add(drRow1);
            grdSubComponent.DataSource = dtGridFill;
            grdSubComponent.DataBind();
            for (int h = 0; h < grdSubComponent.Items.Count; h++)
            {
                RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[h].FindControl("Flag");
                objRadComboBox.DataSource = AddFlagColumn();
                objRadComboBox.DataBind();
                if (dtGridFill.Rows[h]["Flag"].ToString() != string.Empty)
                {
                    objRadComboBox.SelectedIndex = objRadComboBox.FindItemByText(dtGridFill.Rows[h]["Flag"].ToString()).Index;
                    objRadComboBox.SelectedValue = objRadComboBox.Items[objRadComboBox.SelectedIndex].Text;
                }
            }
            btnAdd.Enabled = true;
            btnSaveandMovetonextprocess.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);            
        }
        protected void btnFillGrid_Click(object sender, EventArgs e)
        {
            FillGrid();
            btnAdd.Enabled = false;           
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);      
        }
        void FillGrid()
        {
            hdnPhyID.Value = string.Empty;
            hdnLabName.Value = string.Empty;
            YesNoCancelMessage.Value = string.Empty;
            int SelectedIndex;
            grdSubComponent.DataSource = null;
            IList<ResultEntryDTO> ResultDto = (IList<ResultEntryDTO>)ViewState["ResultDTO"];
            IList<string> SearchOrdersList = new List<string>();
            IList<ResultEntryDTO> OrderAndSubmitList = new List<ResultEntryDTO>();
            ClearAll();
            DataTable dtGridFill = CreateGridDataSource();
            grdSubComponent.DataSource = dtGridFill;
            grdSubComponent.DataBind();
            if (hdnIsRowClick.Value != string.Empty)
            {
                btnAdd.Enabled = true;
                btnSaveandMovetonextprocess.Enabled = true;
                hdnIsRowClick.Value = string.Empty;
            }
            ViewState["DataSource"] = dtGridFill;
            hdnOrderSubmitID.Value = string.Empty;
            hdnOrderSubmitWorkflow.Value = string.Empty;
            IList<string> OrderSubmitIDListForWorkflow = new List<string>();
            cboReportStatus.Text = "FINAL";
            IList<string> strOrderSubmitID = new List<string>();
            DateTime dtBirthdate = DateTime.Now;
            int iAgeDiff = Convert.ToInt32(DateTime.Now.Year - dtBirthdate.Year);
            if (hdnSelectedIndex.Value != string.Empty)
            {
                SelectedIndex = Convert.ToInt32(hdnSelectedIndex.Value);
            }
            else
            {
                return;
            }
            if (SelectedIndex >= 0)
            {
                grdTestOrdered.Items[SelectedIndex].Selected = true;
                if (grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().Contains("ResultMasterID") == true)
                {
                    if (grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().EndsWith(",") == true)
                    {
                        grdTestOrdered.Items[SelectedIndex].Cells[5].Text = grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().Substring(0, grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().Length - 1);
                    }
                    IList<ResultMaster> UpdateResultMasterList = new List<ResultMaster>();
                    IList<ResultORC> UpdateResultORCList = new List<ResultORC>();
                    IList<ResultOBR> UpdateResultOBRList = new List<ResultOBR>();
                    IList<ResultOBX> UpdateResultOBXList = new List<ResultOBX>();
                    IList<ResultNTE> UpdateResultNTEList = new List<ResultNTE>();
                    IList<ResultZPS> UpdateResultZPSList = new List<ResultZPS>();
                    IList<PatientResults> UpdatePatientResultList = new List<PatientResults>();
                    string[] sResultMasterID = grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().Replace("ResultMasterID", " ").Split(',');
                    for (int j = 0; j < sResultMasterID.Length; j++)
                    {
                        ulong ulResultMasterID = Convert.ToUInt64(sResultMasterID[j].ToString().Replace("ResultMasterID", ""));
                        OrderAndSubmitList = objResultMasterProxy.GetListOfResultsFromResultMasterID(ulResultMasterID, Convert.ToUInt64(Request["MyHumanID"]));

                        for (int l = 0; l < OrderAndSubmitList.Count; l++)
                        {

                            btnAdd.ToolTip = "Sa&ve";
                            IList<PatientResults> PatientResultList = OrderAndSubmitList[l].PatientResultList;
                            IList<ResultMaster> ResultMasterList = OrderAndSubmitList[l].ResultMasterList;
                            IList<ResultORC> ResultORCList = OrderAndSubmitList[l].ResultORCList;
                            IList<ResultOBR> ResultOBRList = OrderAndSubmitList[l].ResultOBRList;
                            IList<ResultOBX> ResultOBXList = OrderAndSubmitList[l].ResultOBXList;
                            IList<ResultNTE> ResultNTEList = OrderAndSubmitList[l].ResultNTEList;
                            IList<ResultZPS> ResultZPSList = OrderAndSubmitList[l].ResultZPSList;

                            UpdateResultMasterList = UpdateResultMasterList.Concat(OrderAndSubmitList[l].ResultMasterList).ToList<ResultMaster>();
                            UpdateResultORCList = UpdateResultORCList.Concat(OrderAndSubmitList[l].ResultORCList).ToList<ResultORC>();
                            UpdateResultOBRList = UpdateResultOBRList.Concat(OrderAndSubmitList[l].ResultOBRList).ToList<ResultOBR>();
                            UpdateResultOBXList = UpdateResultOBXList.Concat(OrderAndSubmitList[l].ResultOBXList).ToList<ResultOBX>();
                            UpdateResultNTEList = UpdateResultNTEList.Concat(OrderAndSubmitList[l].ResultNTEList).ToList<ResultNTE>();
                            UpdateResultZPSList = UpdateResultZPSList.Concat(OrderAndSubmitList[l].ResultZPSList).ToList<ResultZPS>();
                            UpdatePatientResultList = UpdatePatientResultList.Concat(OrderAndSubmitList[l].PatientResultList).ToList<PatientResults>();


                            ViewState["RMaster"] = UpdateResultMasterList;
                            ViewState["ORC"] = UpdateResultORCList;
                            ViewState["ZPS"] = UpdateResultZPSList;
                            ViewState["OBR"] = UpdateResultOBRList;
                            ViewState["OBX"] = UpdateResultOBXList;
                            ViewState["PR"] = UpdatePatientResultList;
                            ViewState["NTE"] = UpdateResultNTEList;

                            for (int i = 0; i < OrderAndSubmitList[l].ResultMasterList.Count; i++)
                            {
                                FillSubComponentGrid(ResultOBRList, ResultOBXList, ResultNTEList, PatientResultList);
                            }

                            # region Result Master
                            if (OrderAndSubmitList[l].ResultMasterList[0].MSH_Date_And_Time_Of_Message != string.Empty)
                            {
                                dtpDateReported.SelectedDate = DateTime.ParseExact(OrderAndSubmitList[l].ResultMasterList[0].MSH_Date_And_Time_Of_Message, "yyyyMMddHHmm", CultureInfo.InvariantCulture);

                            }
                            else
                            {
                                dtpDateReported.DateInput.DateFormat = " ";
                            }
                            IList<OrdersSubmit> orderSubmitList = new List<OrdersSubmit>();
                            for (int i = 0; i < ResultDto.Count; i++)
                            {
                                for (int a = 0; a < OrderSubmitIDListForWorkflow.Distinct().ToList<string>().Count; a++)
                                {
                                    var query = from os in ResultDto[i].OrderSubmit where os.Id == Convert.ToUInt64(OrderSubmitIDListForWorkflow[a]) select os;
                                    orderSubmitList = query.ToList<OrdersSubmit>();
                                }
                            }
                            if (orderSubmitList.Count > 0)
                            {
                                if (orderSubmitList[0].Fasting == "Y")
                                {
                                    chkFasting.Enabled = false;
                                    chkFasting.Checked = true;
                                }
                                else if (orderSubmitList[0].Fasting == "N")
                                {
                                    chkFasting.Enabled = false;
                                    chkFasting.Checked = false;
                                }


                            }
                            if (chkFasting.Enabled == true)
                            {
                                if (OrderAndSubmitList[l].ResultMasterList[0].PID_Fasting == "Y")
                                {
                                    chkFasting.Checked = true;
                                }
                                else
                                {
                                    chkFasting.Checked = false;

                                }
                            }

                            #endregion


                            # region Result ORC

                            if (OrderAndSubmitList[l].ResultORCList.Count > 0)
                            {
                                txtPhysicianName.Text = OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Prefix + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_First_Initial + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Last_Name + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Middle_Initial + " " + OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_Suffix;
                                PhysicianLibrary objphysician = phyProxy.GetPhysicianByNPI(OrderAndSubmitList[l].ResultORCList[0].ORC_Ordering_Provider_ID)[0];
                                hdnPhyID.Value = objphysician.Id.ToString();
                            }


                            #endregion

                            # region Result OBR
                            dtpDateCollected.SelectedDate = UtilityManager.ConvertToLocal(DateTime.ParseExact(OrderAndSubmitList[l].ResultOBRList[0].OBR_Specimen_Collection_Date_And_Time, "yyyyMMddHHmm", CultureInfo.InvariantCulture));

                            if (OrderAndSubmitList[l].ResultOBRList[0].OBR_Order_Result_Status.ToUpper() == "F")
                            {
                                cboReportStatus.Text = "FINAL";
                                cboReportStatus.SelectedIndex = cboReportStatus.Items.FindItemByText(cboReportStatus.Text).Index;
                            }
                            else if (OrderAndSubmitList[l].ResultOBRList[0].OBR_Order_Result_Status.ToUpper() == "P")
                            {
                                cboReportStatus.Text = "PRELIMINARY";
                                cboReportStatus.SelectedIndex = cboReportStatus.Items.FindItemByText(cboReportStatus.Text).Index;
                            }
                            else if (OrderAndSubmitList[l].ResultOBRList[0].OBR_Order_Result_Status.ToUpper() == "C")
                            {
                                cboReportStatus.Text = "CORRECTED RESULTS";
                                cboReportStatus.SelectedIndex = cboReportStatus.Items.FindItemByText(cboReportStatus.Text).Index;
                            }
                            #endregion

                            # region Result ZPS
                            if (OrderAndSubmitList[l].ResultZPSList.Count > 0)
                            {
                                txtLabName.Text = OrderAndSubmitList[l].ResultZPSList[0].ZPS_Facility_Name;

                            }
                            if (orderSubmitList.Count > 0)
                            {
                                if (orderSubmitList[0].Specimen_Collection_Date_And_Time.ToString("yyyy-mm-dd hh:mm:ss") != "0001-01-01 00:00:00" && orderSubmitList[0].Specimen_Collection_Date_And_Time.ToString("yyyy-mm-dd hh:mm:ss") != "0001-00-01 12:00:00")
                                {
                                    dtpDateCollected.Enabled = false;
                                    string date = (orderSubmitList[0].Specimen_Collection_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                                else
                                {
                                    dtpDateCollected.Enabled = true;
                                    string date = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                            }
                            else
                            {
                                if (UpdatePatientResultList[0].Captured_date_and_time != null)
                                {
                                    dtpDateCollected.Enabled = true;
                                    string date = UtilityManager.ConvertToLocal(UpdatePatientResultList[0].Captured_date_and_time).ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                                else
                                {
                                    dtpDateCollected.Enabled = true;
                                    string date = (orderSubmitList[0].Specimen_Collection_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");
                                    dtpDateCollected.SelectedDate = Convert.ToDateTime(date);
                                }
                            }
                            #endregion
                        }
                    }

                    // DisplayLabDetails();

                }
                else if ((grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().Contains("BlankOrder")) == true)
                {
                    cboReportStatus.Text = "FINAL";
                    SearchOrdersList.Clear();
                    string[] sOrderCodeList = grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString().Replace("BlankOrder", "").Split(',');
                    for (int k = 0; k < sOrderCodeList.Length - 1; k++)
                    {
                        strOrderSubmitID.Add(sOrderCodeList[k].ToString());
                    }
                    for (int l = 0; l < ResultDto.Count; l++)
                    {
                        var ord = from s in ResultDto[l].OrderSubmit where s.Id == Convert.ToUInt64(strOrderSubmitID[0].ToString()) select s;
                        IList<OrdersSubmit> ordersList = ord.ToList<OrdersSubmit>();
                        hdnPhyID.Value = ordersList[0].Physician_ID.ToString();
                        hdnLabName.Value = ordersList[0].Lab_Name;
                        ulLabID.Value = ordersList[0].Lab_ID.ToString();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenSearchOrders('SCAN');", true);
                }
                else
                {
                    for (int b = 0; b < grdSubComponent.Items.Count; b++)
                    {
                        RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[b].FindControl("Flag");
                        objRadComboBox.DataSource = AddFlagColumn();
                        objRadComboBox.DataBind();
                    }
                    cboReportStatus.Text = "FINAL";
                    string Procedure = string.Empty;
                    string sTag = grdTestOrdered.Items[SelectedIndex].Cells[5].Text.ToString();
                    //string sTag = lstResultItems.Items[lstResultItems.SelectedIndex].Tag.ToString();
                    string[] sOrderCodeList = sTag.Replace("OrderCode", "").Split(',');

                    IList<string> strOrderCode = new List<string>();
                    strOrderSubmitID = new List<string>();
                    for (int k = 0; k < sOrderCodeList.Length; k++)
                    {
                        if (sOrderCodeList[k].Contains("-") == true)
                        {
                            strOrderCode.Add(sOrderCodeList[k].Split('-')[0]);
                            strOrderSubmitID.Add(sOrderCodeList[k].Split('-')[1]);
                        }

                    }
                    IList<OrdersSubmit> orderSubmitList = new List<OrdersSubmit>();
                    for (int i = 0; i < ResultDto.Count; i++)
                    {
                        if (strOrderSubmitID.Count > 0)
                        {
                            var query = from os in ResultDto[i].OrderSubmit where os.Id == Convert.ToUInt64(strOrderSubmitID[0]) select os;
                            orderSubmitList = query.ToList<OrdersSubmit>();

                        }
                        if (orderSubmitList.Count() > 0)
                        {
                            ulLabID.Value = orderSubmitList[0].Lab_ID.ToString();
                            hdnPhyID.Value = orderSubmitList[0].Physician_ID.ToString();
                        }
                    }


                    DataRow drFillGrid = null;
                    dtGridFill.Rows.Clear();
                    for (int b = 0; b < strOrderCode.Count(); b++)
                    {
                        ArrayList arryLst = acurusResultProxy.GetAcurusResultMappingListForMRE(strOrderCode[b].ToString(), Gender.Value.ToUpper(), iAgeDiff, Convert.ToInt32(ulLabID.Value));
                        if (arryLst != null && arryLst.Count > 0)
                        {
                            for (int i = 0; i < arryLst.Count; i++)
                            {
                                object[] objlst = (object[])arryLst[i];

                                drFillGrid = dtGridFill.NewRow();
                                // drFillGrid["Del"] = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                                if (objlst[2] != null)
                                    drFillGrid["Units"] = objlst[2].ToString();
                                if (objlst[3] != null)
                                    drFillGrid["Refrange"] = objlst[3].ToString();
                                drFillGrid["Test code"] = objlst[0].ToString();
                                drFillGrid["Test"] = objlst[1].ToString();
                                drFillGrid["Order Code"] = objlst[6].ToString();
                                drFillGrid["OrderCodeDescription"] = objlst[7].ToString();
                                drFillGrid["Acurus Result Code"] = objlst[4].ToString();
                                drFillGrid["Acurus Result Description"] = objlst[5].ToString();
                                drFillGrid["ProcedureCode"] = objlst[6].ToString() + "-" + objlst[7].ToString();
                                for (int j = 0; j < sOrderCodeList.Length - 1; j++)
                                {
                                    if (sOrderCodeList[j].Contains(drFillGrid["Order Code"].ToString()) == true)
                                    {
                                        drFillGrid["Order Submit ID"] = sOrderCodeList[j].Split('-')[1].ToString();
                                        //sOrderSubmitIDList.Add((drFillGrid["Order Submit ID"] + "|" + objlst[6].ToString() + ":" + objlst[7].ToString()));
                                        if (hdnOrderSubmitID.Value == string.Empty)
                                        {
                                            hdnOrderSubmitID.Value = drFillGrid["Order Submit ID"] + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                        }
                                        else
                                        {
                                            hdnOrderSubmitID.Value += "@" + drFillGrid["Order Submit ID"] + "|" + objlst[6].ToString() + ":" + objlst[7].ToString();
                                        }
                                        // OrderSubmitIDListForWorkflow.Add(drFillGrid["Order Submit ID"].ToString());
                                        if (hdnOrderSubmitWorkflow.Value == string.Empty)
                                        {
                                            hdnOrderSubmitWorkflow.Value = drFillGrid["Order Submit ID"].ToString();
                                        }
                                        else
                                        {
                                            hdnOrderSubmitWorkflow.Value += "||" + drFillGrid["Order Submit ID"].ToString();
                                        }
                                    }
                                }
                                dtGridFill.Rows.Add(drFillGrid);
                            }


                            foreach (GridColumn col in grdSubComponent.Columns)
                            {
                                if (col.HeaderText.ToUpper() == "NTE" || col.HeaderText.ToUpper() == "Source" || col.HeaderText.ToUpper() == "Order Submit ID" || col.HeaderText.ToUpper() == "Current Process" || col.HeaderText.ToUpper() == "Test code" || col.HeaderText.ToUpper() == "Acurus Result Description" || col.HeaderText.ToUpper() == "Acurus Result Code" || col.HeaderText.ToUpper() == "SNo" || col.HeaderText.ToUpper() == "ProcedureCode" || col.HeaderText.ToUpper() == "Tag" || col.HeaderText.ToUpper() == "Order Code" || col.HeaderText.ToUpper() == "OrderCodeDescription")
                                {
                                    col.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            IList<Orders> ordersList = null;
                            drFillGrid = dtGridFill.NewRow();

                            // drFillGrid["Del"].Value = global::Acurus.Capella.UI.Properties.Resources.close_small_pressed;
                            for (int j = 0; j < sOrderCodeList.Length - 1; j++)
                            {
                                if (sOrderCodeList[j].Contains(strOrderCode[b].ToString()) == true)
                                {
                                    drFillGrid["Order Submit ID"] = sOrderCodeList[j].Split('-')[1].ToString();
                                }
                            }
                            for (int a = 0; a < ResultDto.Count; a++)
                            {
                                var ord = from s in ResultDto[a].Orders where s.Order_Submit_ID == Convert.ToUInt64(drFillGrid["Order Submit ID"]) && s.Lab_Procedure == strOrderCode[b].ToString() select s;
                                ordersList = ord.ToList<Orders>();
                            }
                            // sOrderSubmitIDList.Add((drFillGrid["Order Submit ID"] + "|" + ordersList[0].Lab_Procedure + ":" + ordersList[0].Lab_Procedure_Description));
                            if (hdnOrderSubmitID.Value == string.Empty)
                            {
                                hdnOrderSubmitID.Value = drFillGrid["Order Submit ID"] + "|" + ordersList[0].Lab_Procedure + ":" + ordersList[0].Lab_Procedure_Description;
                            }
                            else
                            {
                                hdnOrderSubmitID.Value += "@" + (drFillGrid["Order Submit ID"] + "|" + ordersList[0].Lab_Procedure + ":" + ordersList[0].Lab_Procedure_Description);
                            }
                            drFillGrid["ProcedureCode"] = ordersList[0].Lab_Procedure + "-" + ordersList[0].Lab_Procedure_Description;

                            //  OrderSubmitIDListForWorkflow.Add(drFillGrid["Order Submit ID"].ToString());
                            if (hdnOrderSubmitWorkflow.Value == string.Empty)
                            {
                                hdnOrderSubmitWorkflow.Value = drFillGrid["Order Submit ID"].ToString();
                            }
                            else
                            {
                                hdnOrderSubmitWorkflow.Value += "||" + drFillGrid["Order Submit ID"].ToString();
                            }

                            dtGridFill.Rows.Add(drFillGrid);

                        }
                        GridTableView tableViewOrders = new GridTableView();
                        GridGroupByExpression expression = new GridGroupByExpression();
                        GridGroupByField gridGroupByField = new GridGroupByField();
                        gridGroupByField = new GridGroupByField();
                        gridGroupByField.FieldName = "ProcedureCode";
                        gridGroupByField.HeaderText = "Procedure Code";
                        gridGroupByField.HeaderValueSeparator = " : ";
                        gridGroupByField.FormatString = "<strong>{0}</strong>";
                        hdnHeader.Value = drFillGrid["ProcedureCode"].ToString();


                        expression.SelectFields.Add(gridGroupByField);
                        expression.GroupByFields.Add(gridGroupByField);
                        tableViewOrders.GroupByExpressions.Add(expression);
                        this.grdSubComponent.MasterTableView.GroupByExpressions.Add(expression);

                    }
                    DisplayLabDetails();
                    this.grdSubComponent.MasterTableView.ToolTip = hdnHeader.Value;
                    grdSubComponent.DataSource = dtGridFill;
                    grdSubComponent.DataBind();
                    ViewState["DataSource"] = dtGridFill;
                }

            }
            for (int k = 0; k < grdSubComponent.Items.Count; k++)
            {
                RadComboBox objRadComboBox = (RadComboBox)grdSubComponent.Items[k].FindControl("Flag");
                objRadComboBox.DataSource = AddFlagColumn();
                objRadComboBox.DataBind();
            }
        }
        public PhysicianLibrary GetPhysicianDetailsFromXml(ulong PhyID) {
            PhysicianLibrary objPhysician = new PhysicianLibrary();
            XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
            //CAP-2781
            PhysicianFacilityMappingList physicianFacilityMappingList = ConfigureBase<PhysicianFacilityMappingList>.ReadJson("PhysicianFacilityMapping.json");
            if (physicianFacilityMappingList != null)
            {
                //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianFacilityMapping" + ".xml");
                //XmlNode nodeMatchingPhysician = xmldoc.SelectSingleNode("/ROOT/PhyList/Facility/Physician[@ID='" + hdnPhyID.Value.Trim() + "']");
                var matchingPhysician = physicianFacilityMappingList.PhysicianFacility.SelectMany(x=> x.Physician).FirstOrDefault(y => y.ID == hdnPhyID.Value.Trim());
                if (matchingPhysician != null)
                {
                    objPhysician.PhyPrefix = matchingPhysician.prefix;
                    objPhysician.PhyFirstName = matchingPhysician.firstname;
                    objPhysician.PhyMiddleName = matchingPhysician.middlename;
                    objPhysician.PhyLastName = matchingPhysician.lastname;
                    objPhysician.PhySuffix = matchingPhysician.suffix;
                    objPhysician.PhyId = Convert.ToUInt32(matchingPhysician.ID);
                    objPhysician.Id = Convert.ToUInt32(matchingPhysician.ID);
                    objPhysician.PhyNPI = matchingPhysician.npi;
                }
            }
            return objPhysician;
        }

    
    }

}