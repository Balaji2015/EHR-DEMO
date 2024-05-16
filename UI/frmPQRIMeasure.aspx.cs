using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Telerik.Web.UI;
//using Ionic.Zip;

namespace Acurus.Capella.UI
{
    public partial class frmPQRIMeasure : System.Web.UI.Page
    {
        IList<PQRI_Measure> PQRIlist = new List<PQRI_Measure>();
        PhysicianManager phyMngr = new PhysicianManager();
        ulong Physician_Id = 0;
        DataSet dsreport = new DataSet();
        IList<PhysicianLibrary> PhysicianLibrarylst = new List<PhysicianLibrary>();
        IList<FacilityLibrary> lstfacility = new List<FacilityLibrary>();
        FacilityManager objfacility = new FacilityManager();
        string sClientTin = string.Empty;
        string sClientName = string.Empty;
        int iRandomNo = 15;
        int flagmedication = 0;
        UtilityManager objutilitymngr = new UtilityManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //DirectoryInfo directorySelected = new DirectoryInfo(Server.MapPath("Documents/Test/CMS 65"));

            //foreach (FileInfo fileToCompress in directorySelected.GetFiles("*.xml"))
            //{
            //    AddFileToZip("Output.zip", fileToCompress.FullName);
            //}

            // Test();

            // return;

            if (!IsPostBack)
            {
                if (ClientSession.PhysicainDetails.Count == 0)
                    ClientSession.PhysicainDetails = phyMngr.GetphysiciannameByPhyID(ClientSession.PhysicianId);
                //ClientSession.FacilityLibraryList = objfacility.GetFacilityListByFacilityName(ClientSession.FacilityName);//BugID:
                //lstfacility = objfacility.GetFacilityListByFacilityName(ClientSession.FacilityName);


                if (grdPQRIMeasure.DataSource == null)
                {
                    grdPQRIMeasure.DataSource = new string[] { };
                    grdPQRIMeasure.DataBind();


                }

                btnCATIZIP.Enabled = false;
                btnCQM.Enabled = false;
                //Commented by naveena for performance tuning
                //StaticLookupManager objStaticLookupManager = new StaticLookupManager();
                //IList<StaticLookup> lstState = objStaticLookupManager.getStaticLookupByFieldName("REPORTING FOR");
                //cboStage.Items.Add(new RadComboBoxItem(" "));
                //if (lstState != null && lstState.Count > 0)
                //{
                //    for (int i = 0; i < lstState.Count; i++)
                //    {
                //        cboStage.Items.Add(new RadComboBoxItem(lstState[i].Value));
                //        if (lstState[i].Value.Trim() == "Stage 2")
                //            cboStage.SelectedIndex = i + 1;
                //    }
                //}

                //if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                //{
                //    FillPhysicianUser PhyUserList;
                //    PhyUserList = phyMngr.GetPhysicianandUser(false, string.Empty);
                //    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                //    {
                //        RadComboBoxItem lst = new RadComboBoxItem();
                //        lst.Text = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                //        lst.Value = PhyUserList.PhyList[i].Id.ToString();
                //        cboProviderName.Items.Add(lst);
                //        if (ClientSession.PhysicianId == PhyUserList.PhyList[i].Id)
                //        {
                //            cboProviderName.SelectedIndex = cboProviderName.FindItemIndexByValue(lst.Value);
                //        }
                //    }
                //    chkProviderName.Visible = false;
                //}
                //else
                //{
                //    FillPhysicianUser PhyUserList;
                //    PhyUserList = phyMngr.GetPhysicianandUser(true, ClientSession.FacilityName);
                //    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                //    {
                //        RadComboBoxItem lst = new RadComboBoxItem();
                //        lst.Text = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                //        lst.Value = PhyUserList.PhyList[i].Id.ToString();
                //        cboProviderName.Items.Add(lst);
                //    }
                //}
                //

                //Get Physician list from lookup xml
                //IList<string> PhyLst = new List<string>();
                //XDocument xmlDocumentType = XDocument.Load(Server.MapPath(@"ConfigXML\PhysicianFacilityMapping.xml"));
                //foreach (XElement elements in xmlDocumentType.Elements("ROOT").Elements("PhyList").Elements())
                //{
                //    if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                //    {
                //        foreach (XElement xnode in elements.Elements())
                //        {
                //            string s = xnode.Attribute("prefix").Value + " " + xnode.Attribute("firstname").Value + " " + xnode.Attribute("middlename").Value + " " + xnode.Attribute("lastname").Value + " " + xnode.Attribute("suffix").Value + " " + "~" + xnode.Attribute("ID").Value;
                //            if (PhyLst.IndexOf(s) == -1)
                //                PhyLst.Add(s);
                //        }
                //    }
                //    else
                //    {
                //        string xmlValue = elements.Attribute("name").Value;
                //        if (xmlValue.ToUpper() == ClientSession.FacilityName.ToUpper())
                //        {
                //            foreach (XElement xnode in elements.Elements())
                //            {
                //                string s = xnode.Attribute("prefix").Value + " " + xnode.Attribute("firstname").Value + " " + xnode.Attribute("middlename").Value + " " + xnode.Attribute("lastname").Value + " " + xnode.Attribute("suffix").Value + " " + "~" + xnode.Attribute("ID").Value;
                //                if (PhyLst.IndexOf(s) == -1)
                //                    PhyLst.Add(s);
                //            }
                //        }
                //    }
                //}
                //if (PhyLst != null && PhyLst.Count > 0)
                //{

                //    foreach (string plst in PhyLst)
                //    {
                //        RadComboBoxItem lst = new RadComboBoxItem();
                //        lst.Text = plst.Split('~')[0];
                //        lst.Value = plst.Split('~')[1];
                //        cboProviderName.Items.Add(lst);
                //        if (ClientSession.PhysicianId.ToString() == plst.Split('~')[1])
                //        {
                //            cboProviderName.SelectedIndex = cboProviderName.Items.IndexOf(lst);
                //        }

                //    }

                //}
                cboProviderName.Items.Clear();
                FillPhysicianUser PhyUserList;
                if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant")
                    PhyUserList = phyMngr.GetPhysicianandUser(false, ClientSession.FacilityName, ClientSession.LegalOrg);
                else
                    PhyUserList = phyMngr.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);
                if (PhyUserList != null && PhyUserList.PhyList.Count > 0) //code added by balaji.Tj 2015-12-09
                {
                    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                    {
                        //Old Code
                        //string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                        //Gitlab# 2485 - Physician Name Display Change
                        string sPhyName = string.Empty;
                        if (PhyUserList.PhyList[i].PhyLastName != String.Empty)
                            sPhyName += PhyUserList.PhyList[i].PhyLastName;
                        if (PhyUserList.PhyList[i].PhyFirstName != String.Empty)
                        {
                            if (sPhyName != String.Empty)
                                sPhyName += "," + PhyUserList.PhyList[i].PhyFirstName;
                            else
                                sPhyName += PhyUserList.PhyList[i].PhyFirstName;
                        }
                        if (PhyUserList.PhyList[i].PhyMiddleName != String.Empty)
                            sPhyName += " " + PhyUserList.PhyList[i].PhyMiddleName;
                        if (PhyUserList.PhyList[i].PhySuffix != String.Empty)
                            sPhyName += "," + PhyUserList.PhyList[i].PhySuffix;
                        //Old Code
                        //cboProviderName.Items.Add(PhyUserList.UserList[i].user_name.ToString() + " - " + sPhyName);
                        cboProviderName.Items.Add(sPhyName);
                        cboProviderName.Items[i].Value = PhyUserList.PhyList[i].Id.ToString();
                        cboProviderName.ToolTip = cboProviderName.SelectedItem.Text;

                        if (ClientSession.UserRole == "Physician" || ClientSession.UserRole == "Physician Assistant")
                        {
                            cboProviderName.Enabled = false;
                            //added by balaji.T
                            if (PhyUserList.UserList[i].user_name.ToString() == ClientSession.UserName)
                            {
                                cboProviderName.Text = ClientSession.UserName;
                                cboProviderName.SelectedIndex = i;
                            }

                            //chkProviderName.Visible = false;
                        }
                    }
                }
                if (ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT")
                {
                    chkProviderName.Visible = false;
                    cboProviderName.Enabled = false;
                }
                else
                {
                    chkProviderName.Visible = true;
                    cboProviderName.Enabled = true;
                }
                dtpToDate.SelectedDate = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-12-31");
                dtpFromDate.SelectedDate = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-01-01");
            }
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            #region OldCode
            //if (dtpFromDate.SelectedDate.ToString() == "")
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090011');", true);
            //    return;
            //}
            //if (dtpToDate.SelectedDate.ToString() == "")
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090012');", true);
            //    return;
            //}
            //if (dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('180609');", true);
            //    return;
            //}
            //Physician_Id = Convert.ToUInt16(cboProviderName.SelectedValue);

            //PQRI_MeasureManager objPQRI = new PQRI_MeasureManager();
            ////PQRIlist = objPQRI.GetPqriMeasureList(Physician_Id, dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value);

            ////Stage 2 Measure
            //if (cboStage.Text == "Stage 2")
            //    PQRIlist = objPQRI.GetNewPqriMeasureList(Physician_Id, dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value);//Stage two measure comment on 25.11.2017 

            ////Stage 3 Measure
            //if (cboStage.Text == "Stage 3")
            //{


            //    IList<PQRI_Measure> ilstPQRI = new List<PQRI_Measure>();
            //    string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\pqri.xml");
            //    if (File.Exists(strXmlFilePath) == true)
            //    {
            //        XmlDocument itemDoc = new XmlDocument();
            //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //        XmlNodeList xmlTagName = null;
            //        itemDoc.Load(XmlText);
            //        XmlText.Close();

            //        if (itemDoc.GetElementsByTagName("pqriList")[0] != null && itemDoc.GetElementsByTagName("pqriList").Count > 0)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("pqriList")[0].ChildNodes;
            //            if (xmlTagName.Count > 0)
            //            {
            //                PQRI_Measure objMeasure = new PQRI_Measure();
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    objMeasure = new PQRI_Measure();
            //                    objMeasure.Measure_No = xmlTagName[j].Attributes.GetNamedItem("Measure_No").Value;
            //                    objMeasure.Measure_Name = xmlTagName[j].Attributes.GetNamedItem("Measure_Name").Value;
            //                    objMeasure.Sort_Order = Convert.ToInt32(xmlTagName[j].Attributes.GetNamedItem("Sort_Order").Value);
            //                    ilstPQRI.Add(objMeasure);
            //                }
            //            }
            //        }
            //    }
            //    PQRIlist = objPQRI.GetPqriMeasureStageThreeList(Physician_Id, dtpFromDate.SelectedDate.Value, dtpToDate.SelectedDate.Value, ilstPQRI);
            //}


            //PQRIlist = (from doc in PQRIlist
            //            orderby doc.Sort_Order ascending
            //            select doc).ToList<PQRI_Measure>();


            //IList<string[]> NumeratorList = new List<string[]>();
            //IList<string[]> DenominatorList = new List<string[]>();
            //IList<string[]> DenominatorExclusionList = new List<string[]>();

            //IList<string[]> DenominatorExceptionList = new List<string[]>();
            //for (int i = 0; i < PQRIlist.Count; i++)
            //{
            //    NumeratorList = NumeratorList.Concat(PQRIlist[i].ICDCPTNumeratorList).ToList<string[]>();
            //    DenominatorList = DenominatorList.Concat(PQRIlist[i].ICDCPTDenominatorList).ToList<string[]>();
            //    DenominatorExclusionList = DenominatorExclusionList.Concat(PQRIlist[i].ICDCPTDenominatorExclusionList).ToList<string[]>();
            //    DenominatorExceptionList = DenominatorExceptionList.Concat(PQRIlist[i].ICDCPTDenominatorExceptionList).ToList<string[]>();
            //}
            //IList<PQRIResultDTO> ResultDto = new List<PQRIResultDTO>();
            //if (DenominatorList.Count > 0)
            //{
            //    for (int i = 0; i < DenominatorList.Count; i++)
            //    {

            //        PQRIResultDTO resultDto = new PQRIResultDTO();
            //        if (DenominatorList[i][0] != "")
            //            resultDto.EncounterID = Convert.ToUInt32(DenominatorList[i][0]);
            //        resultDto.HumanID = Convert.ToUInt32(DenominatorList[i][1]);
            //        resultDto.ICD = DenominatorList[i][2].ToString();

            //        resultDto.ProcedureCode = DenominatorList[i][3].ToString();
            //        resultDto.Value = DenominatorList[i][4].ToString();
            //        resultDto.LoincIdentifier = DenominatorList[i][5].ToString();
            //        if (DenominatorList[i][6] != "")
            //            resultDto.ResultDateandTime = Convert.ToDateTime(DenominatorList[i][6].ToString());

            //        resultDto.MeasureNo = DenominatorList[i][7].ToString();
            //        resultDto.MeasureName = DenominatorList[i][8].ToString();

            //        ResultDto.Add(resultDto);
            //    }
            //    for (int k = 0; k < NumeratorList.Count; k++)
            //    {
            //        PQRIResultDTO resultNumeratorDto = new PQRIResultDTO();
            //        if (NumeratorList[k][0] != "")
            //            resultNumeratorDto.EncounterID = Convert.ToUInt32(NumeratorList[k][0]);
            //        resultNumeratorDto.HumanID = Convert.ToUInt32(NumeratorList[k][1]);
            //        resultNumeratorDto.ICD = NumeratorList[k][2].ToString();
            //        resultNumeratorDto.ProcedureCode = NumeratorList[k][3].ToString();
            //        resultNumeratorDto.Value = NumeratorList[k][4].ToString();
            //        resultNumeratorDto.LoincIdentifier = NumeratorList[k][5].ToString();
            //        if (NumeratorList[k][6] != "")
            //            resultNumeratorDto.ResultDateandTime = Convert.ToDateTime(NumeratorList[k][6].ToString());
            //        resultNumeratorDto.MeasureNo = NumeratorList[k][7].ToString();
            //        resultNumeratorDto.NDCID = NumeratorList[k][8].ToString();
            //        resultNumeratorDto.Recodes = NumeratorList[k][9].ToString();
            //        resultNumeratorDto.MeasureName = NumeratorList[k][10].ToString();
            //        ResultDto.Add(resultNumeratorDto);
            //    }
            //    for (int i = 0; i < DenominatorExclusionList.Count; i++)
            //    {
            //        PQRIResultDTO resultDenominatorExclusionDto = new PQRIResultDTO();
            //        if (DenominatorExclusionList[i][0] != "")
            //            resultDenominatorExclusionDto.EncounterID = Convert.ToUInt32(DenominatorExclusionList[i][0]);
            //        resultDenominatorExclusionDto.HumanID = Convert.ToUInt32(DenominatorExclusionList[i][1]);
            //        resultDenominatorExclusionDto.ICD = DenominatorExclusionList[i][2].ToString();
            //        resultDenominatorExclusionDto.ProcedureCode = DenominatorExclusionList[i][3].ToString();
            //        resultDenominatorExclusionDto.Value = DenominatorExclusionList[i][4].ToString();
            //        resultDenominatorExclusionDto.LoincIdentifier = DenominatorExclusionList[i][5].ToString();
            //        if (DenominatorExclusionList[i][6] != "")
            //            resultDenominatorExclusionDto.ResultDateandTime = Convert.ToDateTime(DenominatorExclusionList[i][6].ToString());
            //        resultDenominatorExclusionDto.MeasureNo = DenominatorExclusionList[i][7].ToString();
            //        resultDenominatorExclusionDto.MeasureName = DenominatorExclusionList[i][8].ToString();
            //        ResultDto.Add(resultDenominatorExclusionDto);
            //    }
            //    for (int i = 0; i < DenominatorExceptionList.Count; i++)
            //    {
            //        PQRIResultDTO resultDenominatorExceptionDto = new PQRIResultDTO();
            //        if (DenominatorExceptionList[i][0] != "")
            //            resultDenominatorExceptionDto.EncounterID = Convert.ToUInt32(DenominatorExceptionList[i][0]);
            //        resultDenominatorExceptionDto.HumanID = Convert.ToUInt32(DenominatorExceptionList[i][1]);
            //        resultDenominatorExceptionDto.ICD = DenominatorExceptionList[i][2].ToString();
            //        resultDenominatorExceptionDto.ProcedureCode = DenominatorExceptionList[i][3].ToString();
            //        resultDenominatorExceptionDto.Value = DenominatorExceptionList[i][4].ToString();
            //        resultDenominatorExceptionDto.LoincIdentifier = DenominatorExceptionList[i][5].ToString();
            //        if (DenominatorExceptionList[i][6] != "")
            //            resultDenominatorExceptionDto.ResultDateandTime = Convert.ToDateTime(DenominatorExceptionList[i][6].ToString());
            //        resultDenominatorExceptionDto.MeasureNo = DenominatorExceptionList[i][7].ToString();
            //        resultDenominatorExceptionDto.MeasureName = DenominatorExceptionList[i][8].ToString();
            //        ResultDto.Add(resultDenominatorExceptionDto);
            //    }
            //}
            //int iResult = ResultDto.Select(a => a.HumanID).Distinct().Count();
            //if (PQRIlist != null && PQRIlist.Count > 0)
            //{
            //    grdPQRIMeasure.DataSource = null;
            //    LoadGrid(PQRIlist);
            //    btnCQM.Enabled = true;
            //    btnCATIZIP.Enabled = true;

            //}

            //// LoadXML(ResultDto);
            //Session["CQMList"] = ResultDto;

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            #endregion 

            if (dtpFromDate.SelectedDate.ToString() == "")
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090011');", true);
                return;
            }
            if (dtpToDate.SelectedDate.ToString() == "")
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('7090012');", true);
                return;
            }
            if (dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, "DisplayErrorMessage('180609');", true);
                return;
            }
            Physician_Id = Convert.ToUInt16(cboProviderName.SelectedValue);

            PQRI_MeasureManager objPQRI = new PQRI_MeasureManager();
            CQMSummaryManager cqmSummaryMngr = new CQMSummaryManager();

            //Stage 3 Measure
            if (cboStage.Text == "Stage 3")
            {
                PQRIlist = cqmSummaryMngr.GetCQMSummary(ClientSession.LegalOrg, dtpFromDate.SelectedDate.Value.Year.ToString(), Physician_Id);
            }

            PQRIlist = (from doc in PQRIlist
                        orderby doc.Sort_Order ascending
                        select doc).ToList<PQRI_Measure>();


            IList<string[]> NumeratorList = new List<string[]>();
            IList<string[]> DenominatorList = new List<string[]>();
            IList<string[]> DenominatorExclusionList = new List<string[]>();

            IList<string[]> DenominatorExceptionList = new List<string[]>();
            for (int i = 0; i < PQRIlist.Count; i++)
            {
                NumeratorList = NumeratorList.Concat(PQRIlist[i].ICDCPTNumeratorList).ToList<string[]>();
                DenominatorList = DenominatorList.Concat(PQRIlist[i].ICDCPTDenominatorList).ToList<string[]>();
                DenominatorExclusionList = DenominatorExclusionList.Concat(PQRIlist[i].ICDCPTDenominatorExclusionList).ToList<string[]>();
                DenominatorExceptionList = DenominatorExceptionList.Concat(PQRIlist[i].ICDCPTDenominatorExceptionList).ToList<string[]>();
            }
            IList<PQRIResultDTO> ResultDto = new List<PQRIResultDTO>();
            if (DenominatorList.Count > 0)
            {
                for (int i = 0; i < DenominatorList.Count; i++)
                {

                    PQRIResultDTO resultDto = new PQRIResultDTO();
                    if (DenominatorList[i][0] != "")
                        resultDto.EncounterID = Convert.ToUInt32(DenominatorList[i][0]);
                    resultDto.HumanID = Convert.ToUInt32(DenominatorList[i][1]);
                    resultDto.ICD = DenominatorList[i][2].ToString();

                    resultDto.ProcedureCode = DenominatorList[i][3].ToString();
                    resultDto.Value = DenominatorList[i][4].ToString();
                    resultDto.LoincIdentifier = DenominatorList[i][5].ToString();
                    if (DenominatorList[i][6] != "")
                        resultDto.ResultDateandTime = Convert.ToDateTime(DenominatorList[i][6].ToString());

                    resultDto.MeasureNo = DenominatorList[i][7].ToString();
                    resultDto.MeasureName = DenominatorList[i][8].ToString();

                    ResultDto.Add(resultDto);
                }
                for (int k = 0; k < NumeratorList.Count; k++)
                {
                    PQRIResultDTO resultNumeratorDto = new PQRIResultDTO();
                    if (NumeratorList[k][0] != "")
                        resultNumeratorDto.EncounterID = Convert.ToUInt32(NumeratorList[k][0]);
                    resultNumeratorDto.HumanID = Convert.ToUInt32(NumeratorList[k][1]);
                    resultNumeratorDto.ICD = NumeratorList[k][2].ToString();
                    resultNumeratorDto.ProcedureCode = NumeratorList[k][3].ToString();
                    resultNumeratorDto.Value = NumeratorList[k][6].ToString();
                    resultNumeratorDto.LoincIdentifier = NumeratorList[k][5].ToString();
                    // if (NumeratorList[k][4] != "")
                    //  resultNumeratorDto.ResultDateandTime = Convert.ToDateTime(NumeratorList[k][4].ToString());
                    resultNumeratorDto.MeasureNo = NumeratorList[k][7].ToString();
                    resultNumeratorDto.NDCID = NumeratorList[k][8].ToString();
                    resultNumeratorDto.Recodes = NumeratorList[k][9].ToString();
                    resultNumeratorDto.MeasureName = NumeratorList[k][10].ToString();
                    ResultDto.Add(resultNumeratorDto);
                }
                for (int i = 0; i < DenominatorExclusionList.Count; i++)
                {
                    PQRIResultDTO resultDenominatorExclusionDto = new PQRIResultDTO();
                    if (DenominatorExclusionList[i][0] != "")
                        resultDenominatorExclusionDto.EncounterID = Convert.ToUInt32(DenominatorExclusionList[i][0]);
                    resultDenominatorExclusionDto.HumanID = Convert.ToUInt32(DenominatorExclusionList[i][1]);
                    resultDenominatorExclusionDto.ICD = DenominatorExclusionList[i][2].ToString();
                    resultDenominatorExclusionDto.ProcedureCode = DenominatorExclusionList[i][3].ToString();
                    resultDenominatorExclusionDto.Value = DenominatorExclusionList[i][4].ToString();
                    resultDenominatorExclusionDto.LoincIdentifier = DenominatorExclusionList[i][5].ToString();
                    if (DenominatorExclusionList[i][6] != "")
                        resultDenominatorExclusionDto.ResultDateandTime = Convert.ToDateTime(DenominatorExclusionList[i][6].ToString());
                    resultDenominatorExclusionDto.MeasureNo = DenominatorExclusionList[i][7].ToString();
                    resultDenominatorExclusionDto.MeasureName = DenominatorExclusionList[i][8].ToString();
                    ResultDto.Add(resultDenominatorExclusionDto);
                }
                for (int i = 0; i < DenominatorExceptionList.Count; i++)
                {
                    PQRIResultDTO resultDenominatorExceptionDto = new PQRIResultDTO();
                    if (DenominatorExceptionList[i][0] != "")
                        resultDenominatorExceptionDto.EncounterID = Convert.ToUInt32(DenominatorExceptionList[i][0]);
                    resultDenominatorExceptionDto.HumanID = Convert.ToUInt32(DenominatorExceptionList[i][1]);
                    resultDenominatorExceptionDto.ICD = DenominatorExceptionList[i][2].ToString();
                    resultDenominatorExceptionDto.ProcedureCode = DenominatorExceptionList[i][3].ToString();
                    resultDenominatorExceptionDto.Value = DenominatorExceptionList[i][4].ToString();
                    resultDenominatorExceptionDto.LoincIdentifier = DenominatorExceptionList[i][5].ToString();
                    if (DenominatorExceptionList[i][6] != "")
                        resultDenominatorExceptionDto.ResultDateandTime = Convert.ToDateTime(DenominatorExceptionList[i][6].ToString());
                    resultDenominatorExceptionDto.MeasureNo = DenominatorExceptionList[i][7].ToString();
                    resultDenominatorExceptionDto.MeasureName = DenominatorExceptionList[i][8].ToString();
                    ResultDto.Add(resultDenominatorExceptionDto);
                }
            }
            int iResult = ResultDto.Select(a => a.HumanID).Distinct().Count();
            if (PQRIlist != null && PQRIlist.Count > 0)
            {
                grdPQRIMeasure.DataSource = null;
                LoadGrid(PQRIlist);
                btnCQM.Enabled = true;
                btnCATIZIP.Enabled = true;

            }

            // LoadXML(ResultDto);
            Session["CQMList"] = ResultDto;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);



        }

        private bool CheckDate()
        {
            bool Status = false;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                DateTime d1 = dtpFromDate.SelectedDate.Value;
                DateTime d2 = dtpToDate.SelectedDate.Value;
                TimeSpan span = d2 - d1;
                int Days = span.Days;
                if (Days < 0)
                {
                    Status = false;
                }
                else
                {
                    Status = true;
                }

            }
            return Status;
        }


        public void LoadGrid(IList<PQRI_Measure> PQRI_lst)
        {
            IDictionary<string, string> DCQM = new Dictionary<string, string>();
            IDictionary<string, string> DsubCQM = new Dictionary<string, string>();
            grdPQRIMeasure.DataSource = null;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Measure Name", typeof(string));
            dt.Columns.Add("Measure in Detail", typeof(string));
            dt.Columns.Add("Numerator", typeof(string));
            dt.Columns.Add("Denominator", typeof(string));
            dt.Columns.Add("Rate", typeof(string));
            dt.Columns.Add("Rate Required", typeof(string));
            dt.Columns.Add("Cleared", typeof(string));
            dt.Columns.Add("Initial Patient Population", typeof(string));
            dt.Columns.Add("Denominator Exclusion", typeof(string));
            dt.Columns.Add("Denominator Exception", typeof(string));
            dt.Columns.Add("Numerator Human Id", typeof(string));
            dt.Columns.Add("Denominator Human Id", typeof(string));
            dt.Columns.Add("Numerator Exclusion", typeof(string));

            //int iCMS138Count = 1;
            for (int i = 0; i < PQRI_lst.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Measure Name"] = PQRI_lst[i].Measurement_No.Replace("_Population1", "").Replace("_Population2", "").Replace("_Population3", "");
                dr["Measure in Detail"] = PQRI_lst[i].Measurement_Name;
                dr["Numerator"] = PQRI_lst[i].Numerator;
                dr["Denominator"] = PQRI_lst[i].Denominator;
                dr["Rate"] = PQRI_lst[i].Percentage;
                dr["Rate Required"] = PQRI_lst[i].RequiredPercentage;
                dr["Cleared"] = PQRI_lst[i].Cleared;
                if (!PQRI_lst[i].Measurement_No.Contains("("))
                {
                    //if (PQRI_lst[i].Measurement_No.Contains("CMS138"))
                    //{
                    //    DCQM.Add(PQRI_lst[i].Measurement_No.Trim()+"_"+ iCMS138Count.ToString(), (PQRI_lst[i].Percentage.Trim() == string.Empty) ? "0" : PQRI_lst[i].Percentage.Trim());
                    //    iCMS138Count = iCMS138Count + 1;
                    //}
                    //else
                    //{
                    DCQM.Add(PQRI_lst[i].Measurement_No.Trim(), (PQRI_lst[i].Percentage.Trim() == string.Empty) ? "0" : PQRI_lst[i].Percentage.Trim());
                    //}
                }
                //if (DCQM.Any(a => a.Key.Contains(PQRI_lst[i].Measure_No.Trim())&&a.Key.Contains(".")))
                //    DsubCQM.Add(PQRI_lst[i].Measure_No.Trim(), PQRI_lst[i].Measure_No.Trim());//PQRI_lst[i].Measure_No.Trim().Contains(".") ? PQRI_lst[i].Measure_No.Trim().Substring(0, PQRI_lst[i].Measure_No.Trim().Length - PQRI_lst[i].Measure_No.Trim().IndexOf(".")) : PQRI_lst[i].Measure_No.Trim());
                dr["Initial Patient Population"] = PQRI_lst[i].InitialPatientPopulation;
                dr["Denominator Exclusion"] = PQRI_lst[i].DenominatorExclusion;
                dr["Denominator Exception"] = PQRI_lst[i].DenominatorException;
                if (PQRI_lst[i].ICDCPTNumeratorList != null && PQRI_lst[i].ICDCPTNumeratorList.Count > 0)
                    dr["Numerator Human Id"] = String.Join(",", PQRI_lst[i].ICDCPTNumeratorList.Select(a => a[1]).Distinct().ToArray());
                else
                    dr["Numerator Human Id"] = 0;
                string denominator = "";
                if (PQRI_lst[i].ICDCPTDenominatorList != null && PQRI_lst[i].ICDCPTDenominatorList.Count > 0)
                {
                    // dr["Denominator Human Id"] = String.Join(",", PQRI_lst[i].ICDCPTDenominatorList.Select(a => a[1]).Distinct().ToArray());
                    denominator = String.Join(",", PQRI_lst[i].ICDCPTDenominatorList.Select(a => a[1]).Distinct().ToArray());
                }
                if (PQRI_lst[i].ICDCPTDenominatorExceptionList != null && PQRI_lst[i].ICDCPTDenominatorExceptionList.Count > 0)
                {
                    if (denominator == "")
                        denominator =  String.Join(",", PQRI_lst[i].ICDCPTDenominatorExceptionList.Select(a => a[1]).Distinct().ToArray());

                    else

                        denominator = denominator+ "," + String.Join(",", PQRI_lst[i].ICDCPTDenominatorExceptionList.Select(a => a[1]).Distinct().ToArray());
                }
                if (PQRI_lst[i].ICDCPTDenominatorExclusionList != null && PQRI_lst[i].ICDCPTDenominatorExclusionList.Count > 0)
                {
                    if (denominator == "")
                        denominator =  String.Join(",", PQRI_lst[i].ICDCPTDenominatorExclusionList.Select(a => a[1]).Distinct().ToArray());

                    else
                        denominator = denominator + "," + String.Join(",", PQRI_lst[i].ICDCPTDenominatorExclusionList.Select(a => a[1]).Distinct().ToArray());
                }
                if (denominator == "")
                    dr["Denominator Human Id"] = 0;
                else
                    dr["Denominator Human Id"] = denominator;

                dr["Numerator Exclusion"] = PQRI_lst[i].NumeratorExclusion;
                dt.Rows.Add(dr);

                string temp = "";
                temp = String.Join(",", PQRI_lst[i].ICDCPTDenominatorExclusionList.Select(a => a[1]).Distinct().ToArray());
            }
            dsreport.Tables.Add(dt);

            grdPQRIMeasure.DataSource = dsreport.Tables[0];
            grdPQRIMeasure.DataBind();

            Session["MeasurePercentage"] = DCQM;
            //Session["SubMeasurePercentage"] = DsubCQM;

        }

        #region CAT I1

        public void LoadXML(IList<PQRIResultDTO> PQRIResultDTO)
        {
            //int p = 0;
            //int h = 0;
            IList<PQRIResultDTO> PQRIResultDTOList = PQRIResultDTO;

            //var s = PQRIResultDTOList.GroupBy(a => a.HumanID);
            //IList<ulong> lstHuman = new List<ulong>();
            //foreach (var item in s)
            //    lstHuman.Add(item.Key);


            //IList<string> MeasuresLst = new List<string>();

            var Measures = PQRIResultDTOList.Select(a => a.MeasureNo).Distinct();//.GroupBy(a => a.MeasureNo);

            //var test=PQRIResultDTOList.Select
            //foreach (var item in Measures)
            //{

            //    MeasuresLst.Add(item);
            //}


            IDictionary<string, string> DMeasureNamesLst = (IDictionary<string, string>)Session["MeasurePercentage"];

            //IDictionary<string, string> DSubMeasureNamesLst = (IDictionary<string, string>)Session["SubMeasurePercentage"];


            foreach (var MeasureName in DMeasureNamesLst)
            {
                string Name = MeasureName.Key.Replace(" ", "");
                if (MeasureName.Key.Contains("."))
                    Name = MeasureName.Key.Substring(0, MeasureName.Key.IndexOf(".")).Replace(" ", "");




                IList<ulong> lstHuman = PQRIResultDTOList.Where(a => a.MeasureName == MeasureName.Key.Replace(" ", "")).Select(a => a.HumanID).Distinct().ToList<ulong>();



                foreach (ulong Item_Human_ID in lstHuman)
                {
                    //var xDox = null;
                    StringBuilder XMLInsertsb = new StringBuilder();
                    HumanManager objHumanManager = new HumanManager();
                    EncounterManager objEncounterManager = new EncounterManager();
                    PQRI_DataManager objPQRIManager = new PQRI_DataManager();

                    IList<PQRI_Data> lst_Pqri_Data = objPQRIManager.GetPQRIListBySection();


                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\XML Master Template.xml"));

                    string FolderName = string.Empty;

                    if (MeasureName.Key.Trim() == "CMS 69.1")
                        FolderName = "CMS 69";
                    else
                        FolderName = MeasureName.Key;

                    //DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + MeasureName.Key)); Old
                    DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName));
                    if (!objdirect.Exists)
                        objdirect.Create();

                    Human HumanLst = objHumanManager.GetById(Item_Human_ID);

                    #region
                    XmlNodeList xmlReqNode = null;
                    xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
                    //xmlReqNode[0].InnerText = DateTime.Now.ToString();
                    xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");

                    PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
                    IList<PatientInsuredPlan> PatientInsuredPlanlst = objPatientInsuredPlanManager.GetActiveInsurancePoliciesByHumanId(Item_Human_ID);
                    if (PatientInsuredPlanlst.Count > 0)
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("id");
                        xmlReqNode[1].Attributes[0].Value = PatientInsuredPlanlst[0].Policy_Holder_ID;
                    }
                    else
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("id");
                        xmlReqNode[1].Attributes[0].Value = "0";

                    }
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[0].InnerText = HumanLst.Street_Address1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[0].InnerText = HumanLst.City;
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[0].InnerText = HumanLst.State;
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[0].InnerText = HumanLst.ZipCode;
                    xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
                    xmlReqNode[0].Attributes[1].Value = HumanLst.Work_Phone_No;
                    xmlReqNode = xmlDoc.GetElementsByTagName("given");
                    xmlReqNode[0].InnerText = HumanLst.First_Name;
                    xmlReqNode = xmlDoc.GetElementsByTagName("family");
                    xmlReqNode[0].InnerText = HumanLst.Last_Name;
                    xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
                    xmlReqNode[0].Attributes[0].Value = (HumanLst.Sex.ToUpper() == "MALE") ? "M" : "F";
                    xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
                    xmlReqNode[0].Attributes[0].Value = HumanLst.Race_No;
                    xmlReqNode[0].Attributes[1].Value = HumanLst.Race;
                    xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
                    xmlReqNode[0].Attributes[0].Value = HumanLst.Date_Of_Death.ToString("yyyyMMddhhmmss");
                    xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                    xmlReqNode[0].Attributes[0].Value = Convert.ToString(HumanLst.Ethnicity_No);
                    xmlReqNode[0].Attributes[1].Value = HumanLst.Ethnicity;

                    //Physiican
                    xmlReqNode = xmlDoc.GetElementsByTagName("time");
                    xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
                    xmlReqNode = xmlDoc.GetElementsByTagName("id");
                    PhysicianPatientManager objPhysicianPatientManager = new PhysicianPatientManager();
                    PhysicianManager objPhysicianManager = new PhysicianManager();
                    IList<PhysicianLibrary> PhysicianLibrarylst = objPhysicianManager.GetphysiciannameByPhyID(ClientSession.PhysicianId);
                    xmlReqNode[2].Attributes[0].Value = PhysicianLibrarylst[0].PhyNPI;
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyAddress1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyAddress1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyState;
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyZip;
                    xmlReqNode = xmlDoc.GetElementsByTagName("country");
                    xmlReqNode[1].InnerText = "US";
                    xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
                    xmlReqNode[1].Attributes[1].Value = PhysicianLibrarylst[0].PhyTelephone;

                    //CHAPARRAL MEDICAL GROUP INC
                    xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
                    xmlReqNode[2].Attributes[1].Value = "CHAPARRAL123";
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[2].InnerText = "1904 N ORANGE GROVE AVE CHAPARRAL MEDICAL GROUP TEST";
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[2].InnerText = "POMONA TEST";
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[2].InnerText = "CA TEST";
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[2].InnerText = "601420 TEST";
                    xmlReqNode = xmlDoc.GetElementsByTagName("country");
                    xmlReqNode[2].InnerText = "US TEST";

                    //bc01a5d1-3a34-4286-82cc-43eb04c972a7
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[3].InnerText = PhysicianLibrarylst[0].PhyAddress1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[3].InnerText = PhysicianLibrarylst[0].PhyAddress1;
                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[3].InnerText = PhysicianLibrarylst[0].PhyState;
                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[3].InnerText = PhysicianLibrarylst[0].PhyZip;
                    xmlReqNode = xmlDoc.GetElementsByTagName("country");
                    xmlReqNode[3].InnerText = "US";
                    xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
                    xmlReqNode[3].Attributes[1].Value = PhysicianLibrarylst[0].PhyTelephone;

                    //legalAuthenticator && assignedPerson
                    xmlReqNode = xmlDoc.GetElementsByTagName("given");
                    xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyFirstName;
                    xmlReqNode = xmlDoc.GetElementsByTagName("family");
                    xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyLastName;

                    //serviceEvent

                    IList<Encounter> EncLst = objEncounterManager.GetEncoutnerListByPhyID((int)ClientSession.PhysicianId);
                    string sLow = string.Empty;
                    string sHigh = string.Empty;
                    //  string  sLow = string.Empty;
                    if (EncLst != null && EncLst.Count > 0)
                    {
                        sLow = ConvertToLocal(EncLst.OrderBy(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
                        sHigh = ConvertToLocal(EncLst.OrderByDescending(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
                        if (sLow.StartsWith("0001")==true)
                        {
                            sLow = sHigh;
                        }
                    }
                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    xmlReqNode[0].Attributes[0].Value = sLow;
                    xmlReqNode = xmlDoc.GetElementsByTagName("high");
                    xmlReqNode[0].Attributes[0].Value = sHigh;

                    //performer
                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    xmlReqNode[1].Attributes[0].Value = sLow;
                    xmlReqNode = xmlDoc.GetElementsByTagName("high");
                    xmlReqNode[1].Attributes[0].Value = sHigh;

                    //assignedEntity
                    xmlReqNode = xmlDoc.GetElementsByTagName("id");
                    xmlReqNode[6].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;//"66666";

                    // CHOSEN IN MEAsURE CALCULATOR
                    //  xmlReqNode = xmlDoc.GetElementsByTagName("item");
                    // xmlReqNode[0].InnerText = "Reporting Parameter " + dtpFromDate.SelectedDate.Value.ToString("yyyyMMddhhmmss") + " To " + dtpToDate.SelectedDate.Value.ToString("yyyyMMddhhmmss");
                    #endregion

                    //1.Encounter Xml File Indert
                    IList<Encounter> EncList = new List<Encounter>();
                    IList<ulong> Items_Enc = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.EncounterID != 0).Select(a => a.EncounterID).ToList<ulong>(); //(from e in PQRIResultDTOList where e.HumanID == Item_Human_ID select  new {e.ICD}).ToList();//.ToList<string>();
                    if (Items_Enc != null && Items_Enc.Count > 0)
                    {
                        EncList = objEncounterManager.GetEncounterList(Items_Enc);
                    }

                    StreamWriter file = new StreamWriter(HttpContext.Current.Server.MapPath("SampleXML" + "\\Test.txt"));



                    IList<PQRIResultDTO> objICDlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.ICD.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    IList<string[]> ICD = new List<string[]>();
                    //Dictionary<string, string> DICD = new Dictionary<string, string>();

                    foreach (PQRIResultDTO item in objICDlst)
                    {

                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.ICD.Trim()) && !ICD.Any(a => a[0].Trim() == item.ICD.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
                        {

                            ICD.Add(new string[] { item.ICD.Trim(), item.EncounterID.ToString().Trim() });
                            PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim()).ToList<PQRI_Data>()[0];
                            Encounter objICD_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            XMLInsertsb.Append(SubXMLLoad(ICD_Item_lst, item, objICD_Enc, sValueSet).ToString());
                        }
                        else
                        {
                            file.WriteLine(item.ICD.Trim());




                        }

                    }

                    //IList<string> Loinc = new List<string>();
                    IList<string[]> Loinc = new List<string[]>();
                    IList<PQRIResultDTO> objLoinclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.LoincIdentifier.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    string sValueSetLoinc = string.Empty;
                    foreach (PQRIResultDTO item in objLoinclst)
                    {
                        if (item.LoincIdentifier.Trim() == "55284-4")
                        {

                            PQRI_Data Dia_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8462-4").ToList<PQRI_Data>()[0];
                            sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8462-4" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            XMLInsertsb.Append(SubXMLLoadForSysAndDia(Dia_Item_lst, item, "55284-4", sValueSetLoinc).ToString());

                            PQRI_Data Sys_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8480-6").ToList<PQRI_Data>()[0];
                            sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8480-6" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            XMLInsertsb.Append(SubXMLLoadForSysAndDia(Sys_Item_lst, item, "55284-4", sValueSetLoinc).ToString());


                        }
                        else if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim()) && !Loinc.Any(a => a[0].Trim() == item.LoincIdentifier.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
                        {
                            //Loinc.Add(item.LoincIdentifier.Trim());
                            Loinc.Add(new string[] { item.LoincIdentifier.Trim(), item.EncounterID.ToString().Trim() });
                            PQRI_Data Loinc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim()).ToList<PQRI_Data>()[0];
                            Encounter objLoinc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];

                            sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>().Count > 0 ? lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set : string.Empty;
                            XMLInsertsb.Append(SubXMLLoad(Loinc_Item_lst, item, objLoinc_Enc, sValueSetLoinc).ToString());
                        }
                        else
                        {
                            file.WriteLine(item.LoincIdentifier.Trim());

                        }

                    }
                    IList<string[]> Procedure = new List<string[]>();

                    IList<PQRIResultDTO> objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    //if (MeasureName.Key == "CMS127N")
                    //    objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    foreach (PQRIResultDTO item in objProclst)
                    {



                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim()) && !Procedure.Any(a => a[0].Trim() == item.ProcedureCode.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
                        {


                            //Procedure.Add(item.ProcedureCode.Trim());
                            Procedure.Add(new string[] { item.ProcedureCode.Trim(), item.EncounterID.ToString().Trim() });
                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim()).ToList<PQRI_Data>()[0];
                            Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];

                            //if(item.ProcedureCode.Trim()=="90732")
                            //    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "33" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            //else
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            XMLInsertsb.Append(SubXMLLoad(Proc_Item_lst, item, objProc_Enc, sValueSet).ToString());
                        }
                        else
                        {
                            file.WriteLine(item.ProcedureCode.Trim());


                        }

                    }

                    if (MeasureName.Key.Replace(" ", "") == "CMS127")
                    {
                        IList<PQRIResultDTO> objProclst127 = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID == 0 && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
                        foreach (PQRIResultDTO item in objProclst127)
                        {
                            if (item.MeasureNo == "CMS127N" && !Procedure.Any(a => a[0].Trim() == item.ProcedureCode.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
                            {
                                Procedure.Add(new string[] { item.ProcedureCode.Trim(), item.HumanID.ToString().Trim() });
                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim()).ToList<PQRI_Data>()[0];
                                string sValueSet = string.Empty;
                                if (item.ProcedureCode.Trim() == "90732")
                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "33" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                                else
                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;

                                XMLInsertsb.Append(SubXMLLoad(Proc_Item_lst, item, null, sValueSet).ToString());
                            }
                        }
                    }



                    //Recodes

                    IList<string[]> Recodes = new List<string[]>();
                    IList<PQRIResultDTO> objRecodeslst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.Recodes.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    foreach (PQRIResultDTO item in objRecodeslst)
                    {
                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.Recodes.Trim()) && !Recodes.Any(a => a[0].Trim() == item.Recodes.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
                        {
                            //Procedure.Add(item.ProcedureCode.Trim());
                            Recodes.Add(new string[] { item.Recodes.Trim(), item.HumanID.ToString().Trim() });
                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.Recodes.Trim()).ToList<PQRI_Data>()[0];
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            if (item.EncounterID == 0)
                            {
                                XMLInsertsb.Append(SubXMLLoad(Proc_Item_lst, item, null, sValueSet).ToString());
                            }
                            else
                            {
                                Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                                XMLInsertsb.Append(SubXMLLoad(Proc_Item_lst, item, objProc_Enc, sValueSet).ToString());
                            }
                        }
                        else
                        {
                            file.WriteLine(item.ProcedureCode.Trim());


                        }
                    }

                    //NDC_ID

                    IList<string[]> NDC_ID = new List<string[]>();
                    IList<PQRIResultDTO> objNDC_IDlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.NDCID.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    foreach (PQRIResultDTO item in objNDC_IDlst)
                    {

                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.NDCID.Trim()) && !NDC_ID.Any(a => a[0].Trim() == item.NDCID.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
                        {
                            //Procedure.Add(item.ProcedureCode.Trim());
                            NDC_ID.Add(new string[] { item.NDCID.Trim(), item.HumanID.ToString().Trim() });
                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.NDCID.Trim()).ToList<PQRI_Data>()[0];
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.NDCID.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            if (item.EncounterID == 0)
                            {
                                XMLInsertsb.Append(SubXMLLoad(Proc_Item_lst, item, null, sValueSet).ToString());
                            }
                            else
                            {
                                Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                                XMLInsertsb.Append(SubXMLLoad(Proc_Item_lst, item, objProc_Enc, sValueSet).ToString());
                            }
                        }
                        else
                        {
                            file.WriteLine(item.ProcedureCode.Trim());


                        }
                    }


                    xmlReqNode = xmlDoc.GetElementsByTagName("Test");
                    xmlReqNode[0].InnerXml = XMLInsertsb.ToString();
                    xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<Test>", "").Replace("</Test>", "");
                    //xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + MeasureName.Key) + "\\" + HumanLst.Id + "_" + HumanLst.First_Name + "_" + HumanLst.Last_Name + ".xml");
                    xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName) + "\\" + HumanLst.Id + "_" + HumanLst.First_Name + "_" + HumanLst.Last_Name + ".xml");
                    file.Close();
                }
            }
        }

        public StringBuilder SubXMLLoad(PQRI_Data PQRI_Data_value, PQRIResultDTO PQRIResultDTO_Value, Encounter EncLst, string sValueSet)
        {


            StringBuilder SubXMLsb = new StringBuilder();
            StringBuilder objEncMainsb;
            StringBuilder AddEncountersb;
            StringBuilder Loopsb;
            var xDox = new XDocument();

            switch (PQRI_Data_value.PQRI_Selection_XML.Trim())
            {







                case "Encounter_Section_CAT_I.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_Section_CAT_I.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 158);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    if (PQRI_Data_value.PQRI_Value.Trim() == "99202" || PQRI_Data_value.PQRI_Value.Trim() == "99381")
                    {

                        StringBuilder AddEncountersb1 = new StringBuilder();

                        StringBuilder Loopsb1 = new StringBuilder();
                        Loopsb1.Append(objEncMainsb.ToString());
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.464.1003.101.12.1048");
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("code=") + 6, "185349003");
                        Loopsb1.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                        Loopsb1.Insert(Loopsb1.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                        AddEncountersb1.AppendFormat(Loopsb1.ToString());
                        SubXMLsb.Append(AddEncountersb1.ToString());
                    }

                    break;
                case "OBS_Tobacco use.xml"://Encoutner Id=0
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Tobacco use.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    //SocialHistoryManager objSocialHistoryManager = new SocialHistoryManager();
                    //objSocialHistoryManager.GetSocialHistoryByHumanID(


                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, PQRIResultDTO_Value.HumanID);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());


                    break;

                case "Substance administration.xml"://NCIDC
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Substance administration.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, PQRIResultDTO_Value.HumanID.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("code code=") + 11, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;
                case "ProcedurePerformed.xml":
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ProcedurePerformed.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;
                case "OBS_Diabetes.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Diabetes.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;
                case "OBS_PREGNANCY_DX.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_PREGNANCY_DX.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value.Trim() == "661.3" ? "O62.3" : PQRI_Data_value.PQRI_Value.Trim());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;

                case "OBS_PREGNANCY.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_PREGNANCY.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;

                case "OBS_Phy Exam_Finding_BMI.xml"://Doubt
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Phy Exam_Finding_BMI.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, PQRIResultDTO_Value.Value);
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;
                case "Procedure_Activity_Act.xml":
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedure_Activity_Act.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());
                    break;
                case "OBS_Diagnosis Active.xml":
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Diagnosis Active.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;
                //case "OBS_Physical Exam_Finding_BP_Diastolic.xml":

                //    break;
                //case "OBS_Physical Exam_Finding_BP_Systolic.xml":

                //    break;
                case "OBS_Lab result.xml":
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Lab result.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());


                    break;

                case "OBS_Dental Carries.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Dental Carries.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());

                    break;


                case "Medication_Admin_Pneu_Vac.xml":
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Medication_Admin_Pneu_Vac.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, (EncLst == null) ? PQRIResultDTO_Value.HumanID.ToString() : EncLst.Id.ToString());//PQRIResultDTO_Value
                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("code=") + 6, (PQRI_Data_value.PQRI_Value.Trim() == "90732") ? "33" : PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, (EncLst == null) ? ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss") : ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, (EncLst == null) ? ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss") : ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, (EncLst == null) ? ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss") : ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, (EncLst == null) ? ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss") : ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());
                    break;

                case "Procedure Performed _current med_documented.xml":
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedure Performed _current med_documented.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());


                    Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("CPT displayName =") + 6, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    AddEncountersb.AppendFormat(Loopsb.ToString());
                    SubXMLsb.Append(AddEncountersb.ToString());
                    break;
                case "Encounter_CMS130_CAT1.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_CMS130_CAT1.xml"));

                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("displayName=") + 8, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 7, PQRI_Data_value.Standard_concept);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddHours(1).ToString("yyyyMMddhhmmss"));


                    break;

                case "Acute_Inpatient_CMS130_CATI.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Acute_Inpatient_CMS130_CATI.xml"));
                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddHours(1).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 7, PQRI_Data_value.Standard_concept);
                    break;

                case "Device_CMS130_CATI.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Device_CMS130_CATI.xml"));


                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("displayName=") + 8, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 7, PQRI_Data_value.Standard_concept);
                    break;
                case "Laboratory_Test_CMS130_CATI.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Laboratory_Test_CMS130_CATI.xml"));


                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("displayName=") + 8, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 7, PQRI_Data_value.Standard_concept);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<value=") + 8, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

                    break;
                case "Medication_CMS130_CATI.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Medication_CMS130_CATI.xml"));

                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    break;
                case "Procedure_Activity_Act_CMS130_CATI.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedure_Activity_Act_CMS130_CATI.xml"));

                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("displayName=") + 8, PQRI_Data_value.PQRI_Description);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddHours(1).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 7, PQRI_Data_value.Standard_concept);

                    break;
                case "Procedure_cms130_CATI.xml":

                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedure_cms130_CATI.xml"));

                    objEncMainsb = new StringBuilder(xDox.ToString());
                    objEncMainsb.Remove(0, 157);
                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                    AddEncountersb = new StringBuilder();

                    Loopsb = new StringBuilder();
                    Loopsb.Append(objEncMainsb.ToString());
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
                    Loopsb.Insert(Loopsb.ToString().IndexOf("value=") + 7, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 7, PQRI_Data_value.Standard_concept);
                    break;

                default:
                    break;
            }
            return SubXMLsb;
        }

        public StringBuilder SubXMLLoadForSysAndDia(PQRI_Data PQRI_Data_value, PQRIResultDTO PQRIResultDTO_Value, string Loinc_Identifier, string sValueSet)
        {
            StringBuilder SubXMLsb = new StringBuilder();
            StringBuilder objEncMainsb;
            StringBuilder AddEncountersb;
            StringBuilder Loopsb;
            var xDox = new XDocument();
            VitalsManager objVitalsManager = new VitalsManager();
            PatientResults objPatientResults = objVitalsManager.GetResultByLoincObservationAndEncounterId(Loinc_Identifier, PQRIResultDTO_Value.HumanID, PQRIResultDTO_Value.EncounterID);//we want to change

            if (objPatientResults != null)
            {
                string[] ary = objPatientResults.Value.Split('/');
                switch (PQRI_Data_value.PQRI_Selection_XML.Trim())
                {

                    case "OBS_Physical Exam_Finding_BP_Diastolic.xml":


                        xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Physical Exam_Finding_BP_Diastolic.xml"));
                        objEncMainsb = new StringBuilder(xDox.ToString());
                        objEncMainsb.Remove(0, 157);
                        objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                        AddEncountersb = new StringBuilder();

                        Loopsb = new StringBuilder();
                        Loopsb.Append(objEncMainsb.ToString());
                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                        Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, objPatientResults.Encounter_ID.ToString());
                        Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                        Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                        Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, objPatientResults.Captured_date_and_time.ToString("yyyyMMddhhmmss"));
                        Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, objPatientResults.Captured_date_and_time.ToString("yyyyMMddhhmmss"));
                        Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, ary[1]);
                        AddEncountersb.AppendFormat(Loopsb.ToString());
                        SubXMLsb.Append(AddEncountersb.ToString());

                        break;



                    case "OBS_Physical Exam_Finding_BP_Systolic.xml":
                        xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Physical Exam_Finding_BP_Systolic.xml"));
                        objEncMainsb = new StringBuilder(xDox.ToString());
                        objEncMainsb.Remove(0, 157);
                        objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
                        AddEncountersb = new StringBuilder();

                        Loopsb = new StringBuilder();
                        Loopsb.Append(objEncMainsb.ToString());
                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
                        Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, objPatientResults.Encounter_ID.ToString());
                        Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
                        Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
                        Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, objPatientResults.Captured_date_and_time.ToString("yyyyMMddhhmmss"));
                        Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, objPatientResults.Captured_date_and_time.ToString("yyyyMMddhhmmss"));
                        Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, ary[0]);
                        AddEncountersb.AppendFormat(Loopsb.ToString());
                        SubXMLsb.Append(AddEncountersb.ToString());


                        break;
                    default:
                        break;
                }
            }
            return SubXMLsb;
        }

        #endregion CAT I End

        #region CAT III

        protected void btnCQM_Click(object sender, EventArgs e)
        {
            //Test();
            if (cboStage.Text == "Stage 3")
            {
                CQMStage3CATLoad();
            }
            else
            {
                CQMCATLoad();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Download", "DownloadFile();", true);

            btnCQM.Enabled = false;

        }
        public void btndownload_Click(object sender, EventArgs e)
        {
            DownLoadZIPFormateCATII(Server.MapPath("Documents/" + Session.SessionID + "/CQMIII"));
        }
        public void CQMStage3CATLoad()
        {
            HumanManager objHumanManager = new HumanManager();

            IList<Human> NumeratorHumanList69 = new List<Human>();
            IList<Human> DenmoniatorHumanList9 = new List<Human>();
            IList<Human> DenomiExclusionHumanList69 = new List<Human>();

            PQRI_MeasureManager objPQRI = new PQRI_MeasureManager();

            IList<PQRI_Measure> ilstPqriMsr = objPQRI.GetPQRIMeasureDetailsByYear("2024");

            string sMeasureNumber = string.Empty;



            PhysicianLibrarylst = phyMngr.GetphysiciannameByPhyID(ClientSession.PhysicianId);
            lstfacility = objfacility.GetFacilityListByFacilityName(ClientSession.FacilityName);
            //Dictionary<string, int[]> Dlst = null;
            StringBuilder sbLoad = new StringBuilder();

            IDictionary<string, string> DCQM = (IDictionary<string, string>)Session["MeasurePercentage"];

            IList<PQRIResultDTO> PQRIDTO = (IList<PQRIResultDTO>)Session["CQMList"];


            IList<ulong> NumeratorList68 = PQRIDTO.Where(a => a.MeasureNo == "CMS68N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList68 = PQRIDTO.Where(a => a.MeasureNo == "CMS68D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE68List = PQRIDTO.Where(a => a.MeasureNo == "CMS68DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX68List = PQRIDTO.Where(a => a.MeasureNo == "CMS68DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();
            StringBuilder cms68 = MeasureHeaderCount(NumeratorList68, DenmoniatorList68, DE68List, DEX68List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd2");

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS68") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();
            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS68_v11_CAT_III_Header.xml", DenmoniatorList68, NumeratorList68, new List<ulong>(), DEX68List, DCQM["CMS68v11"], "CMS68v11", null).ToString());
            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList68, NumeratorList68, new List<ulong>(), DEX68List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());






            IList<ulong> NumeratorList69 = PQRIDTO.Where(a => a.MeasureNo == "CMS69N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList69 = PQRIDTO.Where(a => a.MeasureNo == "CMS69D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE69List = PQRIDTO.Where(a => a.MeasureNo == "CMS69DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX69List = PQRIDTO.Where(a => a.MeasureNo == "CMS69DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS69") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS69_v10_CAT_III_Header.xml", DenmoniatorList69, NumeratorList69, DE69List, DEX69List, DCQM["CMS69v10"], "CMS69v10", null).ToString());
            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList69, NumeratorList69, DE69List, DEX69List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());

            StringBuilder cms69 = MeasureHeaderCount(NumeratorList69, DenmoniatorList69, DE69List, DEX69List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd1");


            IList<ulong> NumeratorList138_Population1 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population1N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList138_Population1 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population1D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE138List = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1DE").Select(a => a.HumanID).ToList<ulong>();
            IList<ulong> DE138List_Population1 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population1DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX138List_Population1 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population1DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();
             StringBuilder cms138_Population1 = MeasureHeaderCount(NumeratorList138_Population1, DenmoniatorList138_Population1, DE138List_Population1, DEX138List_Population1, "E35791DF-5B25-41BB-B260-673337BC44A6");

            IList<ulong> NumeratorList138_Population2 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population2N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList138_Population2 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population2D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE138List = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1DE").Select(a => a.HumanID).ToList<ulong>();
            IList<ulong> DE138List_Population2 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population2DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX138List_Population2 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population2DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();
           // sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS138_v10_CAT_III_Header.xml", DenmoniatorList138_Population2, NumeratorList138_Population2, DE138List_Population2, DEX138List_Population2, DCQM["CMS138v10_Population2"], "CMS138v10_Population2", null).ToString());
          //  StringBuilder cms138_Population2 = MeasureHeaderCount(NumeratorList138_Population2, DenmoniatorList138_Population2, DE138List_Population2, DEX138List_Population2, "E35791DF-5B25-41BB-B260-673337BC44A6");

            IList<ulong> NumeratorList138_Population3 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population3N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList138_Population3 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population3D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE138List = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1DE").Select(a => a.HumanID).ToList<ulong>();
            IList<ulong> DE138List_Population3 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population3DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX138List_Population3 = PQRIDTO.Where(a => a.MeasureNo == "CMS138_Population3DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS138") == true && a.Measurement_No.Contains("Population1") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();
            string sMsrToboco2 = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS138") == true && a.Measurement_No.Contains("Population2") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();
            string sMsrToboco3 = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS138") == true && a.Measurement_No.Contains("Population3") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIItobaccostage3("CMS138_v10_CAT_III_Header.xml", DenmoniatorList138_Population1,
            //    NumeratorList138_Population1, DE138List_Population1, DEX138List_Population1,
            //    DCQM["CMS138v10_Population1"], "CMS138v10_Population1",
            //    DenmoniatorList138_Population2, NumeratorList138_Population2, DE138List_Population2, DEX138List_Population2, DCQM["CMS138v10_Population2"],
            //    "CMS138v10_Population2", DenmoniatorList138_Population3, NumeratorList138_Population3, DE138List_Population3, DEX138List_Population3,
            //    DCQM["CMS138v10_Population3"], "CMS138v10_Population3", null).ToString());

            string sMeasureHeaderXML = sMeasureNumber.Split('_')[0];
            sbLoad.Append(SubXMLLoadForCQMIIItobaccostage3(sMeasureHeaderXML + "_CAT_III_Header.xml", DenmoniatorList138_Population1,
                NumeratorList138_Population1, DE138List_Population1, DEX138List_Population1,
                DCQM[sMeasureNumber], sMeasureNumber,
                DenmoniatorList138_Population2, NumeratorList138_Population2, DE138List_Population2, DEX138List_Population2, DCQM[sMsrToboco2],
                sMsrToboco2, DenmoniatorList138_Population3, NumeratorList138_Population3, DE138List_Population3, DEX138List_Population3,
                DCQM[sMsrToboco3], sMsrToboco3, null).ToString());


            // sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS138_v10_CAT_III_Header.xml", DenmoniatorList138_Population3, NumeratorList138_Population3, DE138List_Population3, DEX138List_Population3, DCQM["CMS138v10_Population3"], "CMS138v10_Population3", null).ToString());
            // StringBuilder cms138_Population3 = MeasureHeaderCount(NumeratorList138_Population3, DenmoniatorList138_Population3, DE138List_Population3, DEX138List_Population3, "E35791DF-5B25-41BB-B260-673337BC44A6");

            IList<ulong> NumeratorList127 = PQRIDTO.Where(a => a.MeasureNo == "CMS127N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList127 = PQRIDTO.Where(a => a.MeasureNo == "CMS127D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE127List = PQRIDTO.Where(a => a.MeasureNo == "CMS127DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX127List = PQRIDTO.Where(a => a.MeasureNo == "CMS127DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS127") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS127_v10_CAT_III_Header.xml", DenmoniatorList127, NumeratorList127, DE127List, DEX127List, DCQM["CMS127v10"], "CMS127v10", null).ToString());
            //StringBuilder cms127 = MeasureHeaderCount(NumeratorList127, DenmoniatorList127, DE127List, DEX127List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd7");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber + "_CAT_III_Header.xml", DenmoniatorList127, NumeratorList127, DE127List, DEX127List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms127 = MeasureHeaderCount(NumeratorList127, DenmoniatorList127, DE127List, DEX127List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd7");




            IList<ulong> NumeratorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS165N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS165D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS165DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX165List = PQRIDTO.Where(a => a.MeasureNo == "CMS165DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS165") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();


            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS165_v10_CAT_III_Header.xml", DenmoniatorList165, NumeratorList165, DE165List, DEX165List, DCQM["CMS165v10"], "CMS165v10", null).ToString());
            //StringBuilder cms165 = MeasureHeaderCount(NumeratorList165, DenmoniatorList165, DE165List, DEX165List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd4");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList165, NumeratorList165, DE165List, DEX165List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms165 = MeasureHeaderCount(NumeratorList165, DenmoniatorList165, DE165List, DEX165List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd4");



            IList<ulong> NumeratorList22 = PQRIDTO.Where(a => a.MeasureNo == "CMS22N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList22 = PQRIDTO.Where(a => a.MeasureNo == "CMS22D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE22List = PQRIDTO.Where(a => a.MeasureNo == "CMS22DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX22List = PQRIDTO.Where(a => a.MeasureNo == "CMS22DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS22") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS22_v10_CAT_III_Header.xml", DenmoniatorList22, NumeratorList22, DE22List, DEX22List, DCQM["CMS22v10"], "CMS22v10", null).ToString());
            //StringBuilder cms22 = MeasureHeaderCount(NumeratorList22, DenmoniatorList22, DE22List, DEX22List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd3");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList22, NumeratorList22, DE22List, DEX22List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms22 = MeasureHeaderCount(NumeratorList22, DenmoniatorList22, DE22List, DEX22List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd3");

            IList<ulong> NumeratorList125 = PQRIDTO.Where(a => a.MeasureNo == "CMS125N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList125 = PQRIDTO.Where(a => a.MeasureNo == "CMS125D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE125List = PQRIDTO.Where(a => a.MeasureNo == "CMS125DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX125List = PQRIDTO.Where(a => a.MeasureNo == "CMS125DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS125") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS125_v10_CAT_III_Header.xml", DenmoniatorList125, NumeratorList125, DE125List, DEX125List, DCQM["CMS125v10"], "CMS125v10", null).ToString());
            //StringBuilder cms125 = MeasureHeaderCount(NumeratorList125, DenmoniatorList125, DE125List, DEX125List, "E35791DF-5B25-41BB-B260-673337BC44A8");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList125, NumeratorList125, DE125List, DEX125List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms125 = MeasureHeaderCount(NumeratorList125, DenmoniatorList125, DE125List, DEX125List, "E35791DF-5B25-41BB-B260-673337BC44A8");


            IList<ulong> NumeratorList147 = PQRIDTO.Where(a => a.MeasureNo == "CMS147N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList147 = PQRIDTO.Where(a => a.MeasureNo == "CMS147D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE147List = PQRIDTO.Where(a => a.MeasureNo == "CMS147DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX147List = PQRIDTO.Where(a => a.MeasureNo == "CMS147DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS147") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS147_v11_CAT_III_Header.xml", DenmoniatorList147, NumeratorList147, DE147List, DEX147List, DCQM["CMS147v11"], "CMS147v11", null).ToString());
            //StringBuilder cms147 = MeasureHeaderCount(NumeratorList147, DenmoniatorList147, DE147List, DEX147List, "E35791DF-5B25-41BB-B260-673337BC44A5");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList147, NumeratorList147, DE147List, DEX147List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms147 = MeasureHeaderCount(NumeratorList147, DenmoniatorList147, DE147List, DEX147List, "E35791DF-5B25-41BB-B260-673337BC44A5");


            IList<ulong> NumeratorList122 = PQRIDTO.Where(a => a.MeasureNo == "CMS122N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList122 = PQRIDTO.Where(a => a.MeasureNo == "CMS122D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE122List = PQRIDTO.Where(a => a.MeasureNo == "CMS122DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX122List = PQRIDTO.Where(a => a.MeasureNo == "CMS122DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS122") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS122_v10_CAT_III_Header.xml", DenmoniatorList122, NumeratorList122, DE122List, DEX122List, DCQM["CMS122v10"], "CMS122v10", null).ToString());
            //StringBuilder cms122 = MeasureHeaderCount(NumeratorList122, DenmoniatorList122, DE122List, DEX122List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd9");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+"_CAT_III_Header.xml", DenmoniatorList122, NumeratorList122, DE122List, DEX122List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms122 = MeasureHeaderCount(NumeratorList122, DenmoniatorList122, DE122List, DEX122List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd9");

            IList<ulong> NumeratorList130 = PQRIDTO.Where(a => a.MeasureNo == "CMS130N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList130 = PQRIDTO.Where(a => a.MeasureNo == "CMS130D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE130List = PQRIDTO.Where(a => a.MeasureNo == "CMS130DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DEX130List = PQRIDTO.Where(a => a.MeasureNo == "CMS130DEX").Select(a => a.HumanID).Distinct().ToList<ulong>();

            sMeasureNumber = ilstPqriMsr.Where(a => a.Measurement_No.Contains("CMS130") == true).Select(a => a.Measurement_No).Distinct().ToList()[0].ToString();

            //sbLoad.Append(SubXMLLoadForCQMIIIstage3("CMS130v10Stage3_Header.xml", DenmoniatorList130, NumeratorList130, DE130List, DEX130List, DCQM["CMS130v10"], "CMS130v10", null).ToString());
            //StringBuilder cms130 = MeasureHeaderCount(NumeratorList130, DenmoniatorList130, DEX130List, DE130List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd2");

            sbLoad.Append(SubXMLLoadForCQMIIIstage3(sMeasureNumber+ "_CAT_III_Header.xml", DenmoniatorList130, NumeratorList130, DE130List, DEX130List, DCQM[sMeasureNumber], sMeasureNumber, null).ToString());
            StringBuilder cms130 = MeasureHeaderCount(NumeratorList130, DenmoniatorList130, DEX130List, DE130List, "b6ac13e2-beb8-4e4f-94ed-fcc397406cd2");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\QRDAStage3_CAT_III_Header.xml"));
            XmlNodeList xmlReqNode = null;

            PhysicianManager objPhysicianManager = new PhysicianManager();
            EncounterManager objEncounterManager = new EncounterManager();


            DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID + "/CQMIII"));
            if (!objdirect.Exists)
                objdirect.Create();

            //xmlReqNode = xmlDoc.GetElementsByTagName("templateId");
            //xmlReqNode[0].Attributes[1].Value = "2016-02-01";
            //xmlReqNode[11].Attributes[1].Value = "2016-02-01";

            xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMdd");
            xmlReqNode = xmlDoc.GetElementsByTagName("time");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMdd");
            xmlReqNode[1].Attributes[0].Value = DateTime.Now.ToString("yyyyMMdd");
            xmlReqNode[2].Attributes[0].Value = DateTime.Now.ToString("yyyyMMdd");
            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[2].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyLastName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyFirstName;

            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            xmlReqNode[0].Attributes[1].Value = lstfacility[0].Fac_Telephone;
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[0].InnerText = lstfacility[0].Fac_Address1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[0].InnerText = lstfacility[0].Fac_City;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[0].InnerText = lstfacility[0].Fac_State;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[0].InnerText = lstfacility[0].Fac_Zip;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[0].InnerText = "US";


            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
            xmlReqNode[1].Attributes[1].Value = lstfacility[0].Fac_Telephone;
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[1].InnerText = lstfacility[0].Fac_Address1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[1].InnerText = lstfacility[0].Fac_City;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[1].InnerText = lstfacility[0].Fac_State;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[1].InnerText = lstfacility[0].Fac_Zip;
            xmlReqNode = xmlDoc.GetElementsByTagName("country");
            xmlReqNode[1].InnerText = "US";


            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyLastName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyFirstName;




            // IList<Encounter> EncLst = objEncounterManager.GetEncoutnerListByPhyID((int)ClientSession.PhysicianId);
            string sLow = Convert.ToDateTime(dtpFromDate.SelectedDate).ToString("yyyyMMdd");
            string sHigh = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyyMMdd");
            if (sLow.StartsWith("0001") == true)
            {
                sHigh = sLow;
            }
            // string firstrdos = "";
            // string lastrdos = "";

            //  string  sLow = string.Empty;
            //if (EncLst != null && EncLst.Count > 0)
            //{
            //    sLow = ConvertToLocal((from m in EncLst where Convert.ToDateTime(m.Date_of_Service).ToString("yyyy-MM-dd") != "0001-01-01" select m).OrderBy(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMdd");
            //    sHigh = ConvertToLocal((from m in EncLst where Convert.ToDateTime(m.Date_of_Service).ToString("yyyy-MM-dd") != "0001-01-01" select m).OrderByDescending(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMdd");
            //    firstrdos = ConvertToLocal((from m in EncLst where Convert.ToDateTime(m.Date_of_Service).ToString("yyyy-MM-dd") != "0001-01-01" select m).OrderBy(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("dd MMMM yyyy");
            //    lastrdos = ConvertToLocal((from m in EncLst where Convert.ToDateTime(m.Date_of_Service).ToString("yyyy-MM-dd") != "0001-01-01" select m).OrderByDescending(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("dd MMMM yyyy");
            //}
            xmlReqNode = xmlDoc.GetElementsByTagName("low");
            xmlReqNode[0].Attributes[0].Value = sLow;
            xmlReqNode = xmlDoc.GetElementsByTagName("high");
            xmlReqNode[0].Attributes[0].Value = sHigh;

            xmlReqNode = xmlDoc.GetElementsByTagName("low");
            xmlReqNode[1].Attributes[0].Value = sLow;
            xmlReqNode = xmlDoc.GetElementsByTagName("high");
            xmlReqNode[1].Attributes[0].Value = sHigh;



            xmlReqNode = xmlDoc.GetElementsByTagName("low");
            xmlReqNode[2].Attributes[0].Value = sLow;
            xmlReqNode = xmlDoc.GetElementsByTagName("high");
            xmlReqNode[2].Attributes[0].Value = sHigh;


            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[2].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;//System.Configuration.ConfigurationSettings.AppSettings["ClientNPI"].ToString();
            xmlReqNode[3].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;//System.Configuration.ConfigurationSettings.AppSettings["ClientNPI"].ToString();
            xmlReqNode[5].Attributes[1].Value = System.Configuration.ConfigurationSettings.AppSettings["ClientNPI"].ToString();
            xmlReqNode[4].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;
            xmlReqNode[11].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;
            xmlReqNode[12].Attributes[1].Value = System.Configuration.ConfigurationSettings.AppSettings["ClientNPI"].ToString();

            //// CHOSEN IN MEAsURE CALCULATOR
            //xmlReqNode = xmlDoc.GetElementsByTagName("");
            //xmlReqNode[0].InnerText = "Reporting period: " + dtpFromDate.SelectedDate.Value.ToString("dd MMMM yyyy") + " - " + dtpToDate.SelectedDate.Value.ToString("dd MMMM yyyy");
            // xmlReqNode[1].InnerText = "First encounter: " + firstrdos;
            // xmlReqNode[2].InnerText = "Last encounter:  " + lastrdos;

            //    XmlNodeList xmlReqNode = null;


            xmlReqNode = xmlDoc.GetElementsByTagName("textcountMedication");
            xmlReqNode[0].InnerXml = cms68.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountBMI");
            xmlReqNode[0].InnerXml = cms69.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountVaccine");
            xmlReqNode[0].InnerXml = cms127.ToString();

            //xmlReqNode = xmlDoc.GetElementsByTagName("textcountobacco");
            //xmlReqNode[0].InnerXml = cms138_Population1.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountBP");
            xmlReqNode[0].InnerXml = cms165.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountHba1c");
            xmlReqNode[0].InnerXml = cms122.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountimmun");
            xmlReqNode[0].InnerXml = cms147.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountBreast");
            xmlReqNode[0].InnerXml = cms125.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountBPfollowup");
            xmlReqNode[0].InnerXml = cms22.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("textcountColorectal");
            xmlReqNode[0].InnerXml = cms130.ToString();

            xmlReqNode = xmlDoc.GetElementsByTagName("CATIIILOAD");
            xmlReqNode[0].InnerXml = sbLoad.ToString();
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<CATIIILOAD>", "").Replace("</CATIIILOAD>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountMedication>", "").Replace("</textcountMedication>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountBMI>", "").Replace("</textcountBMI>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountVaccine>", "").Replace("</textcountVaccine>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountobacco>", "").Replace("</textcountobacco>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountBP>", "").Replace("</textcountBP>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountHba1c>", "").Replace("</textcountHba1c>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountimmun>", "").Replace("</textcountimmun>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountBreast>", "").Replace("</textcountBreast>", "");

            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountBPfollowup>", "").Replace("</textcountBPfollowup>", "");
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<textcountColorectal>", "").Replace("</textcountColorectal>", "");

            xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMIII") + "\\CQMCATIII.xml");

        }
        public void CQMCATLoad()
        {


            Dictionary<string, int[]> Dlst = null;
            StringBuilder sbLoad = new StringBuilder();

            IDictionary<string, string> DCQM = (IDictionary<string, string>)Session["MeasurePercentage"];

            IList<PQRIResultDTO> PQRIDTO = (IList<PQRIResultDTO>)Session["CQMList"];

            IList<ulong> NumeratorList = PQRIDTO.Where(a => a.MeasureNo == "CMS65N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList = PQRIDTO.Where(a => a.MeasureNo == "CMS65D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE65List = PQRIDTO.Where(a => a.MeasureNo == "CMS65DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS65v2_Header.xml", DenmoniatorList, NumeratorList, DE65List, DCQM["CMS 65"], "CMS65", null).ToString());


            IList<ulong> NumeratorList68 = PQRIDTO.Where(a => a.MeasureNo == "CMS68N").Select(a => a.HumanID).ToList<ulong>();
            IList<ulong> DenmoniatorList68 = PQRIDTO.Where(a => a.MeasureNo == "CMS68D").Select(a => a.HumanID).ToList<ulong>();
            //IList<ulong> DE68List = PQRIDTO.Where(a => a.MeasureNo == "CMS68DE").Select(a => a.HumanID).ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS68v2_Header.xml", DenmoniatorList68, NumeratorList68, new List<ulong>(), DCQM["CMS 68"], "CMS68", null).ToString());


            IList<ulong> NumeratorList75 = PQRIDTO.Where(a => a.MeasureNo == "CMS75N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList75 = PQRIDTO.Where(a => a.MeasureNo == "CMS75D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE75List = PQRIDTO.Where(a => a.MeasureNo == "CMS75DE").Select(a => a.HumanID).ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS75v1_Header.xml", DenmoniatorList75, NumeratorList75, new List<ulong>(), DCQM["CMS 75"], "CMS75", null).ToString());

            IList<ulong> NumeratorList127 = PQRIDTO.Where(a => a.MeasureNo == "CMS127N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList127 = PQRIDTO.Where(a => a.MeasureNo == "CMS127D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            // IList<ulong> DE127List = PQRIDTO.Where(a => a.MeasureNo == "CMS127DE").Select(a => a.HumanID).ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS127v1_Header.xml", DenmoniatorList127, NumeratorList127, new List<ulong>(), DCQM["CMS 127"], "CMS127", null).ToString());

            IList<ulong> NumeratorList69 = PQRIDTO.Where(a => a.MeasureNo == "CMS69N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList69 = PQRIDTO.Where(a => a.MeasureNo == "CMS69D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE69List = PQRIDTO.Where(a => a.MeasureNo == "CMS69DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS69v1_Header.xml", DenmoniatorList69, NumeratorList69, DE69List, DCQM["CMS 69"], "CMS69", null).ToString());

            IList<ulong> NumeratorList69New = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList69New = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE69NewList = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS69v1_Header.xml", DenmoniatorList69New, NumeratorList69New, DE69NewList, DCQM["CMS 69.1"], "CMS69.1", null).ToString());


            IList<ulong> NumeratorList138 = PQRIDTO.Where(a => a.MeasureNo == "CMS138N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList138 = PQRIDTO.Where(a => a.MeasureNo == "CMS138D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE138List = PQRIDTO.Where(a => a.MeasureNo == "CMS69.1DE").Select(a => a.HumanID).ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS138v1_Header.xml", DenmoniatorList138, NumeratorList138, new List<ulong>(), DCQM["CMS 138"], "CMS138", null).ToString());

            IList<ulong> NumeratorList148 = PQRIDTO.Where(a => a.MeasureNo == "CMS148N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList148 = PQRIDTO.Where(a => a.MeasureNo == "CMS148D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE148List = PQRIDTO.Where(a => a.MeasureNo == "CMS148DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS148v1_Header.xml", DenmoniatorList148, NumeratorList148, DE148List, DCQM["CMS 148"], "CMS148", null).ToString());


            //Modification 155 
            //155
            IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> CommonDenmoniatorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> CommonDE155List = PQRIDTO.Where(a => a.MeasureNo == "CMS155DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //155(12-17)
            IList<ulong> NumeratorList155TS = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> CommonDenmoniatorList155TS = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> CommonDE155ListTS = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //155(3-11)
            IList<ulong> NumeratorList155TL = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> CommonDenmoniatorList155TL = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> CommonDE155ListTL = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();

            Dlst = new Dictionary<string, int[]>();
            int[] ary = { NumeratorList155TS.Count, CommonDenmoniatorList155TS.Count, CommonDE155ListTS.Count };
            Dlst.Add("CMS155(12-17)", ary);
            int[] ary1 = { NumeratorList155TL.Count, CommonDenmoniatorList155TL.Count, CommonDE155ListTL.Count };
            Dlst.Add("CMS155(3-11)", ary1);
            sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", CommonDenmoniatorList155, NumeratorList155, CommonDE155List, DCQM["CMS 155"], "CMS155", Dlst).ToString());

            //155.1
            IList<ulong> NumeratorList155PO = PQRIDTO.Where(a => a.MeasureNo == "CMS155.1N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> CommonDenmoniatorList155PO = CommonDenmoniatorList155;
            //IList<ulong> CommonDenmoniatorList155PO = CommonDE155List;
            //155.1(12-17)
            IList<ulong> NumeratorList155POTS = PQRIDTO.Where(a => a.MeasureNo == "CMS155.1(12-17)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong>  Denominator155ts= CommonDenmoniatorList155TS;
            //IList<ulong> DenominatorExclusionts = CommonDE155ListTS;
            //155.1(3-11)
            IList<ulong> NumeratorList155POTL = PQRIDTO.Where(a => a.MeasureNo == "CMS155.1(3-11)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> Denmklndasjkds = CommonDenmoniatorList155TL;
            //IList<ulong> fkjsdfgds = CommonDE155ListTL;

            Dlst = new Dictionary<string, int[]>();
            int[] aryPO = { NumeratorList155POTS.Count, CommonDenmoniatorList155TS.Count, CommonDE155ListTS.Count };
            Dlst.Add("CMS155(12-17)", aryPO);
            int[] aryPO1 = { NumeratorList155POTL.Count, CommonDenmoniatorList155TL.Count, CommonDE155ListTL.Count };
            Dlst.Add("CMS155(3-11)", aryPO1);
            sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", CommonDenmoniatorList155, NumeratorList155PO, CommonDE155List, DCQM["CMS 155.1"], "CMS155.1", Dlst).ToString());


            //155.2
            IList<ulong> NumeratorList155PT = PQRIDTO.Where(a => a.MeasureNo == "CMS155.2N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> CommonDenmoniatorList155PO = CommonDenmoniatorList155;
            //IList<ulong> CommonDenmoniatorList155PO = CommonDE155List;
            //155.2(12-17)
            IList<ulong> NumeratorList155PTTS = PQRIDTO.Where(a => a.MeasureNo == "CMS155.2(12-17)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> CommonDenmoniatorList155PO = CommonDenmoniatorList155;
            //IList<ulong> CommonDenmoniatorList155PO = CommonDE155List;
            //155.2(3-11)
            IList<ulong> NumeratorList155PTTL = PQRIDTO.Where(a => a.MeasureNo == "CMS155.2(3-11)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> CommonDenmoniatorList155PO = CommonDenmoniatorList155;
            //IList<ulong> CommonDenmoniatorList155PO = CommonDE155List;

            Dlst = new Dictionary<string, int[]>();
            int[] aryPT = { NumeratorList155PTTS.Count, CommonDenmoniatorList155TS.Count, CommonDE155ListTS.Count };
            Dlst.Add("CMS155(12-17)", aryPT);
            int[] aryPT1 = { NumeratorList155PTTL.Count, CommonDenmoniatorList155TL.Count, CommonDE155ListTL.Count };
            Dlst.Add("CMS155(3-11)", aryPT1);
            sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", CommonDenmoniatorList155, NumeratorList155PT, CommonDE155List, DCQM["CMS 155.2"], "CMS155.2", Dlst).ToString());

            #region
            //IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());

            //IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155.1(12-17)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());

            //IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155.2(12-17)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS155(12-17)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());

            //IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());

            //IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155.1(3-11)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            ////sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());

            //IList<ulong> NumeratorList155 = PQRIDTO.Where(a => a.MeasureNo == "CMS155.2(3-11)N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS155(3-11)DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());
            ////
            //IList<ulong> NumeratorList155PO = PQRIDTO.Where(a => a.MeasureNo == "CMS155.1N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DenmoniatorList155PO = PQRIDTO.Where(a => a.MeasureNo == "CMS155D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE155POList = PQRIDTO.Where(a => a.MeasureNo == "CMS155DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PO, NumeratorList155PO, DE155POList, DCQM["CMS 155.1"], "CMS155.1").ToString());

            //IList<ulong> NumeratorList155PT = PQRIDTO.Where(a => a.MeasureNo == "CMS155.2N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DenmoniatorList155PT = PQRIDTO.Where(a => a.MeasureNo == "CMS155D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //IList<ulong> DE155PTList = PQRIDTO.Where(a => a.MeasureNo == "CMS155DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            //sbLoad.Append(SubXMLLoadForCQMIII("CMS155v1_Header.xml", DenmoniatorList155PT, NumeratorList155PT, DE155PTList, DCQM["CMS 155.2"], "CMS155.2").ToString());
            #endregion


            IList<ulong> NumeratorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS165N").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DenmoniatorList165 = PQRIDTO.Where(a => a.MeasureNo == "CMS165D").Select(a => a.HumanID).Distinct().ToList<ulong>();
            IList<ulong> DE165List = PQRIDTO.Where(a => a.MeasureNo == "CMS165DE").Select(a => a.HumanID).Distinct().ToList<ulong>();
            sbLoad.Append(SubXMLLoadForCQMIII("CMS165v1_Header.xml", DenmoniatorList165, NumeratorList165, DE165List, DCQM["CMS 165"], "CMS165", null).ToString());


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\QRDA_CAT_III_Header.xml"));
            XmlNodeList xmlReqNode = null;

            PhysicianManager objPhysicianManager = new PhysicianManager();
            EncounterManager objEncounterManager = new EncounterManager();
            IList<PhysicianLibrary> PhysicianLibrarylst = objPhysicianManager.GetphysiciannameByPhyID(ClientSession.PhysicianId);

            DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID + "/CQMIII"));
            if (!objdirect.Exists)
                objdirect.Create();


            xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
            xmlReqNode = xmlDoc.GetElementsByTagName("time");
            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[2].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyLastName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyFirstName;
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyAddress1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyCity;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyState;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[0].InnerText = PhysicianLibrarylst[0].PhyZip;
            xmlReqNode = xmlDoc.GetElementsByTagName("time");
            xmlReqNode[1].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
            /////
            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyAddress1;
            xmlReqNode = xmlDoc.GetElementsByTagName("city");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyCity;
            xmlReqNode = xmlDoc.GetElementsByTagName("state");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyState;
            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyZip;
            xmlReqNode = xmlDoc.GetElementsByTagName("given");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyLastName;
            xmlReqNode = xmlDoc.GetElementsByTagName("family");
            xmlReqNode[1].InnerText = PhysicianLibrarylst[0].PhyFirstName;
            IList<Encounter> EncLst = objEncounterManager.GetEncoutnerListByPhyID((int)ClientSession.PhysicianId);
            string sLow = string.Empty;
            string sHigh = string.Empty;
            //  string  sLow = string.Empty;
            if (EncLst != null && EncLst.Count > 0)
            {
                sLow = ConvertToLocal(EncLst.OrderBy(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
                sHigh = ConvertToLocal(EncLst.OrderByDescending(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
                if (sLow.StartsWith("0001")==true)
                {
                    sLow = sHigh;
                }
            }
            xmlReqNode = xmlDoc.GetElementsByTagName("low");
            xmlReqNode[0].Attributes[0].Value = sLow;
            xmlReqNode = xmlDoc.GetElementsByTagName("high");
            xmlReqNode[0].Attributes[0].Value = sHigh;
            xmlReqNode = xmlDoc.GetElementsByTagName("low");
            xmlReqNode[1].Attributes[0].Value = sLow;
            xmlReqNode = xmlDoc.GetElementsByTagName("high");
            xmlReqNode[1].Attributes[0].Value = sHigh;
            xmlReqNode = xmlDoc.GetElementsByTagName("low");
            xmlReqNode[2].Attributes[0].Value = sLow;
            xmlReqNode = xmlDoc.GetElementsByTagName("high");
            xmlReqNode[2].Attributes[0].Value = sHigh;
            xmlReqNode = xmlDoc.GetElementsByTagName("id");
            xmlReqNode[8].Attributes[1].Value = PhysicianLibrarylst[0].PhyNPI;

            // CHOSEN IN MEAsURE CALCULATOR
            xmlReqNode = xmlDoc.GetElementsByTagName("item");
            xmlReqNode[0].InnerText = "Reporting Parameter " + dtpFromDate.SelectedDate.Value.ToString("yyyyMMddhhmmss") + " To " + dtpToDate.SelectedDate.Value.ToString("yyyyMMddhhmmss");

            xmlReqNode = xmlDoc.GetElementsByTagName("CATIIILOAD");
            xmlReqNode[0].InnerXml = sbLoad.ToString();
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<CATIIILOAD>", "").Replace("</CATIIILOAD>", "");
            xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMIII") + "\\CQMCATIII.xml");

        }

        public StringBuilder SubXMLLoadForCQMIII(string MeasureName, IList<ulong> DLst, IList<ulong> NLst, IList<ulong> DELst, string CQMPercentage, string CMSName, Dictionary<string, int[]> DLst155)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlReqNode = null;
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\" + MeasureName));

            //DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID));
            //if (!objdirect.Exists)
            //    objdirect.Create();

            PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();

            IList<PQRI_Data> resultlst = objPQRI_DataManager.GetPQRIListByStandardConceptAndPQRIType("REFERENCE OBS ROOT ID", CMSName);
            string RootID = string.Empty;


            StringBuilder objMeasuresb = new StringBuilder();
            var xDox = new XDocument();


            StringBuilder objIPPsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\IPP.xml"));
            objIPPsb = new StringBuilder(xDox.ToString());
            objIPPsb.Remove(0, 157);
            objIPPsb.Remove(objIPPsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.NQF_Number.Trim() == "IPP").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst.Count != 0)
            {

                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, DLst.Count);
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, "Method Returns");
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, IPPAndDenametorLoad(DLst, RootID, CMSName).ToString());

                if (CMSName.StartsWith("CMS155"))
                {
                    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                }

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));



            }
            else
            {
                //RootID = resultlst.Where(a => a.NQF_Number.Trim() == "IPP").ToList<PQRI_Data>()[0].PQRI_Value;
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, "0");
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, CATIIILoadForEmptyXML(RootID, CMSName).ToString());
                if (CMSName.StartsWith("CMS155"))
                {
                    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                }

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));
            }


            StringBuilder objDenominatorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator.xml"));
            objDenominatorsb = new StringBuilder(xDox.ToString());
            objDenominatorsb.Remove(0, 157);
            objDenominatorsb.Remove(objDenominatorsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.NQF_Number.Trim() == "DENOM").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst.Count != 0)
            {

                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, DLst.Count);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, IPPAndDenametorLoad(DLst, RootID, CMSName).ToString());
                if (CMSName.StartsWith("CMS155"))
                    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }
            else
            {
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, "0");
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, CATIIILoadForEmptyXML(RootID, CMSName).ToString());
                if (CMSName.StartsWith("CMS155"))
                    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }



            StringBuilder objNumeratorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Numerator.xml"));
            objNumeratorsb = new StringBuilder(xDox.ToString());
            objNumeratorsb.Remove(0, 157);
            objNumeratorsb.Remove(objNumeratorsb.Length - 20, 20);

            RootID = resultlst.Where(a => a.NQF_Number.Trim() == "NUMER").ToList<PQRI_Data>()[0].PQRI_Value;
            if (NLst.Count != 0)
            {
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, NLst.Count);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, IPPAndDenametorLoad(NLst, RootID, CMSName).ToString());
                if (CMSName.StartsWith("CMS155"))
                    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }
            else
            {
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, "0");
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, CATIIILoadForEmptyXML(RootID, CMSName).ToString());
                if (CMSName.StartsWith("CMS155"))
                    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }




            StringBuilder objDEsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_Exclusion.xml"));
            objDEsb = new StringBuilder(xDox.ToString());
            objDEsb.Remove(0, 157);
            objDEsb.Remove(objDEsb.Length - 20, 20);

            RootID = resultlst.Where(a => a.NQF_Number.Trim() == "DENEX").ToList<PQRI_Data>()[0].PQRI_Value;

            if (DELst.Count != 0)
            {
                objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, DELst.Count);
                objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, IPPAndDenametorLoad(DELst, RootID, CMSName).ToString());
                if (CMSName.StartsWith("CMS155"))
                    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
            }
            else
            {
                if (RootID.Trim() != string.Empty)
                {
                    objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, "0");
                    objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, CATIIILoadForEmptyXML(RootID, CMSName).ToString());
                    if (CMSName.StartsWith("CMS155"))
                        objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                }
            }

            //Exception

            if (CMSName.Trim() == "CMS68" || CMSName.Trim() == "CMS138")
            {
                RootID = resultlst.Where(a => a.NQF_Number.Trim() == "DENEXCEP").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEXsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_Exception.xml"));
                objDEXsb = new StringBuilder(xDox.ToString());
                objDEXsb.Remove(0, 157);
                objDEXsb.Remove(objDEXsb.Length - 20, 20);

                objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, "0");
                objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, CATIIILoadForEmptyXML(RootID, CMSName).ToString());
                objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
            }




            xmlReqNode = xmlDoc.GetElementsByTagName("value");
            xmlReqNode[0].Attributes[1].Value = CQMPercentage;

            xmlReqNode = xmlDoc.GetElementsByTagName("Text");
            xmlReqNode[0].InnerXml = objMeasuresb.ToString();
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<Text>", "").Replace("</Text>", "");

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(xmlDoc.InnerXml);
            //xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID) + "\\" +MeasureName.Split('.')[0]+ ".xml");
            StringBuilder sb = new StringBuilder();
            sb1.Remove(0, 156);
            sb1.Remove(sb1.Length - 19, 19);

            return sb.Append(sb1.ToString());
        }


        public StringBuilder SubXMLLoadForCQMIIItobaccostage3(string MeasureName, IList<ulong> DLst, IList<ulong> NLst,
            IList<ulong> DELst, IList<ulong> DEXLst, string CQMPercentage, string CMSName, IList<ulong> DLst1, IList<ulong> NLst1,
            IList<ulong> DELst1, IList<ulong> DEXLst1, string CQMPercentage1, string CMSName1, IList<ulong> DLst2, IList<ulong> NLst2,
            IList<ulong> DELst2, IList<ulong> DEXLst2, string CQMPercentage2, string CMSName2,
            Dictionary<string, int[]> DLst155)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlReqNode = null;
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\" + MeasureName));

            //DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID));
            //if (!objdirect.Exists)
            //    objdirect.Create();

            PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();


            IList<PQRI_Data> resultlst = objPQRI_DataManager.GetPQRIListByStandardConceptAndPQRIType("REFERENCE OBS ROOT ID_STAGE3", 
                CMSName);
            string RootID = string.Empty;


            StringBuilder objMeasuresb = new StringBuilder();
            var xDox = new XDocument();


            //IntialPopulation


            StringBuilder objIPPsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\IPPStage3.xml"));
            objIPPsb = new StringBuilder(xDox.ToString());
            objIPPsb.Remove(0, 157);
            objIPPsb.Remove(objIPPsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "IPOP").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count != 0)
            {

                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count);


                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, "Method Returns");
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, IPPAndDenametorLoadStage3(DLst.Union(DEXLst).Union(DELst).ToList<ulong>(), RootID, CMSName).ToString());

                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));



            }
            else
            {

                //RootID = resultlst.Where(a => a.NQF_Number.Trim() == "IPP").ToList<PQRI_Data>()[0].PQRI_Value;
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, "0");
                // objIPPsb.Replace(@"value=""", @"nullFlavor=""NA""");
                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));
            }


            // Denominator

            StringBuilder objDenominatorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\DenominatorStage3.xml"));
            objDenominatorsb = new StringBuilder(xDox.ToString());
            objDenominatorsb.Remove(0, 157);
            objDenominatorsb.Remove(objDenominatorsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENOM").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst.Count != 0)
            {

                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, DLst.Count);
                // objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //   objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, IPPAndDenametorLoadStage3(DLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }
            else
            {
                //objDenominatorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, "0");
                //objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }


            //Numerator

            StringBuilder objNumeratorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\NumeratorStage3.xml"));
            objNumeratorsb = new StringBuilder(xDox.ToString());
            objNumeratorsb.Remove(0, 157);
            objNumeratorsb.Remove(objNumeratorsb.Length - 20, 20);

            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "NUMER").ToList<PQRI_Data>()[0].PQRI_Value;
            if (NLst.Count != 0)
            {
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, NLst.Count);
                //  objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, IPPAndDenametorLoadStage3(NLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }
            else
            {
                // objNumeratorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, "0");
                // objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                // objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }


            //Exclusion
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExclusionStage3.xml"));
                objDEsb = new StringBuilder(xDox.ToString());
                objDEsb.Remove(0, 157);
                objDEsb.Remove(objDEsb.Length - 20, 20);


                if (DELst.Count != 0)
                {
                    objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, DELst.Count);
                    //   objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, IPPAndDenametorLoadStage3(DELst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                }
                else
                {
                    if (RootID.Trim() != string.Empty)
                    {
                        //objDEsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                    }
                }
            }

            //Exception
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEXsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExceptionStage3.xml"));
                objDEXsb = new StringBuilder(xDox.ToString());
                objDEXsb.Remove(0, 157);
                objDEXsb.Remove(objDEXsb.Length - 20, 20);

                if (DEXLst.Count != 0)
                {
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, DEXLst.Count);
                    // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, IPPAndDenametorLoadStage3(DEXLst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                }
                else
                {





                    if (RootID.Trim() != string.Empty)
                    {
                        // objDEXsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                    }
                }
            }




            xmlReqNode = xmlDoc.GetElementsByTagName("value");

            if (DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count != 0)
                xmlReqNode[0].Attributes[1].Value = CQMPercentage;
            else
            {
                xmlReqNode[0].Attributes.RemoveAll();
                XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
                xAttribute.Value = "REAL";
                xmlReqNode[0].Attributes.Append(xAttribute);
                XmlAttribute xAttributenull = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
                xAttributenull.Value = "NA";
                xmlReqNode[0].Attributes.Append(xAttributenull);
            }








            resultlst = objPQRI_DataManager.GetPQRIListByStandardConceptAndPQRIType("REFERENCE OBS ROOT ID_STAGE3",
               CMSName1);

            //IntialPopulation

            objIPPsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\IPPStage3.xml"));
            objIPPsb = new StringBuilder(xDox.ToString());
            objIPPsb.Remove(0, 157);
            objIPPsb.Remove(objIPPsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "IPOP").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>().Count != 0)
            {

                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>().Count);


                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, "Method Returns");
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, IPPAndDenametorLoadStage3(DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>(), RootID, CMSName1).ToString());

                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));



            }
            else
            {

                //RootID = resultlst.Where(a => a.NQF_Number.Trim() == "IPP").ToList<PQRI_Data>()[0].PQRI_Value;
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, "0");
                // objIPPsb.Replace(@"value=""", @"nullFlavor=""NA""");
                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, CATIIILoadForEmptyXMLStage3(RootID, CMSName1).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));
            }


            // Denominator

            objDenominatorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\DenominatorStage3.xml"));
            objDenominatorsb = new StringBuilder(xDox.ToString());
            objDenominatorsb.Remove(0, 157);
            objDenominatorsb.Remove(objDenominatorsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENOM").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>().Count != 0)
            {

                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>().Count);
                // objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //   objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, IPPAndDenametorLoadStage3(DLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }
            else
            {
                //objDenominatorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, "0");
                //objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }


            //Numerator

            objNumeratorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\NumeratorStage3.xml"));
            objNumeratorsb = new StringBuilder(xDox.ToString());
            objNumeratorsb.Remove(0, 157);
            objNumeratorsb.Remove(objNumeratorsb.Length - 20, 20);

            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "NUMER").ToList<PQRI_Data>()[0].PQRI_Value;
            if (NLst1.Count != 0)
            {
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, NLst1.Count);
                //  objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, IPPAndDenametorLoadStage3(NLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }
            else
            {
                // objNumeratorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, "0");
                // objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                // objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }


            //Exclusion
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExclusionStage3.xml"));
                objDEsb = new StringBuilder(xDox.ToString());
                objDEsb.Remove(0, 157);
                objDEsb.Remove(objDEsb.Length - 20, 20);


                if (DELst1.Count != 0)
                {
                    objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, DELst1.Count);
                    //   objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, IPPAndDenametorLoadStage3(DELst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                }
                else
                {
                    if (RootID.Trim() != string.Empty)
                    {
                        //objDEsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                    }
                }
            }

            //Exception
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEXsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExceptionStage3.xml"));
                objDEXsb = new StringBuilder(xDox.ToString());
                objDEXsb.Remove(0, 157);
                objDEXsb.Remove(objDEXsb.Length - 20, 20);

                if (DEXLst1.Count != 0)
                {
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, DEXLst1.Count);
                    // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, IPPAndDenametorLoadStage3(DEXLst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                }
                else
                {





                    if (RootID.Trim() != string.Empty)
                    {
                        // objDEXsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                    }
                }
            }




            xmlReqNode = xmlDoc.GetElementsByTagName("value");

            if (DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>().Count != 0)
                xmlReqNode[0].Attributes[1].Value = CQMPercentage1;
            else
            {
                xmlReqNode[0].Attributes.RemoveAll();
                XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
                xAttribute.Value = "REAL";
                xmlReqNode[0].Attributes.Append(xAttribute);
                XmlAttribute xAttributenull = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
                xAttributenull.Value = "NA";
                xmlReqNode[0].Attributes.Append(xAttributenull);
            }
            //xmlReqNode[0].Attributes[1].re




            //IntialPopulation
            resultlst = objPQRI_DataManager.GetPQRIListByStandardConceptAndPQRIType("REFERENCE OBS ROOT ID_STAGE3",
             CMSName2);
            objIPPsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\IPPStage3.xml"));
            objIPPsb = new StringBuilder(xDox.ToString());
            objIPPsb.Remove(0, 157);
            objIPPsb.Remove(objIPPsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "IPOP").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst2.Union(DEXLst2).Union(DELst2).ToList<ulong>().Count != 0)
            {

                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, DLst2.Union(DEXLst2).Union(DELst2).ToList<ulong>().Count);


                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, "Method Returns");
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, IPPAndDenametorLoadStage3(DLst1.Union(DEXLst1).Union(DELst1).ToList<ulong>(), RootID, CMSName1).ToString());

                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));



            }
            else
            {

                //RootID = resultlst.Where(a => a.NQF_Number.Trim() == "IPP").ToList<PQRI_Data>()[0].PQRI_Value;
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, "0");
                // objIPPsb.Replace(@"value=""", @"nullFlavor=""NA""");
                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, CATIIILoadForEmptyXMLStage3(RootID, CMSName1).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));
            }


            // Denominator

            objDenominatorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\DenominatorStage3.xml"));
            objDenominatorsb = new StringBuilder(xDox.ToString());
            objDenominatorsb.Remove(0, 157);
            objDenominatorsb.Remove(objDenominatorsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENOM").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst2.Count != 0)
            {

                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, DLst2.Count);
                // objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //   objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, IPPAndDenametorLoadStage3(DLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }
            else
            {
                //objDenominatorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, "0");
                //objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }


            //Numerator

            objNumeratorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\NumeratorStage3.xml"));
            objNumeratorsb = new StringBuilder(xDox.ToString());
            objNumeratorsb.Remove(0, 157);
            objNumeratorsb.Remove(objNumeratorsb.Length - 20, 20);

            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "NUMER").ToList<PQRI_Data>()[0].PQRI_Value;
            if (NLst2.Count != 0)
            {
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, NLst2.Count);
                //  objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, IPPAndDenametorLoadStage3(NLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }
            else
            {
                // objNumeratorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, "0");
                // objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                // objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }


            //Exclusion
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExclusionStage3.xml"));
                objDEsb = new StringBuilder(xDox.ToString());
                objDEsb.Remove(0, 157);
                objDEsb.Remove(objDEsb.Length - 20, 20);


                if (DELst2.Count != 0)
                {
                    objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, DELst2.Count);
                    //   objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, IPPAndDenametorLoadStage3(DELst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                }
                else
                {
                    if (RootID.Trim() != string.Empty)
                    {
                        //objDEsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                    }
                }
            }

            //Exception
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEXsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExceptionStage3.xml"));
                objDEXsb = new StringBuilder(xDox.ToString());
                objDEXsb.Remove(0, 157);
                objDEXsb.Remove(objDEXsb.Length - 20, 20);

                if (DEXLst2.Count != 0)
                {
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, DEXLst2.Count);
                    // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, IPPAndDenametorLoadStage3(DEXLst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                }
                else
                {





                    if (RootID.Trim() != string.Empty)
                    {
                        // objDEXsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                    }
                }
            }




            xmlReqNode = xmlDoc.GetElementsByTagName("value");

            if (DLst2.Union(DEXLst2).Union(DELst2).ToList<ulong>().Count != 0)
                xmlReqNode[0].Attributes[1].Value = CQMPercentage2;
            else
            {
                xmlReqNode[0].Attributes.RemoveAll();
                XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
                xAttribute.Value = "REAL";
                xmlReqNode[0].Attributes.Append(xAttribute);
                XmlAttribute xAttributenull = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
                xAttributenull.Value = "NA";
                xmlReqNode[0].Attributes.Append(xAttributenull);
            }


            xmlReqNode = xmlDoc.GetElementsByTagName("Text");
            xmlReqNode[0].InnerXml = objMeasuresb.ToString();
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<Text>", "").Replace("</Text>", "");

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(xmlDoc.InnerXml);
            //xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID) + "\\" +MeasureName.Split('.')[0]+ ".xml");
            StringBuilder sb = new StringBuilder();
            sb1.Remove(0, 156);
            sb1.Remove(sb1.Length - 19, 19);

            return sb.Append(sb1.ToString());
        }



        public StringBuilder SubXMLLoadForCQMIIIstage3(string MeasureName, IList<ulong> DLst, IList<ulong> NLst, IList<ulong> DELst, IList<ulong> DEXLst, string CQMPercentage, string CMSName, Dictionary<string, int[]> DLst155)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlReqNode = null;
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\" + MeasureName));

            //DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID));
            //if (!objdirect.Exists)
            //    objdirect.Create();

            PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();

            IList<PQRI_Data> resultlst = objPQRI_DataManager.GetPQRIListByStandardConceptAndPQRIType("REFERENCE OBS ROOT ID_STAGE3", CMSName.Replace("_Population1", "").Replace("_Population2", "").Replace("_Population3", ""));
            string RootID = string.Empty;


            StringBuilder objMeasuresb = new StringBuilder();
            var xDox = new XDocument();


            //IntialPopulation

            StringBuilder objIPPsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\IPPStage3.xml"));
            objIPPsb = new StringBuilder(xDox.ToString());
            objIPPsb.Remove(0, 157);
            objIPPsb.Remove(objIPPsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "IPOP").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count != 0)
            {

                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count);


                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, "Method Returns");
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, IPPAndDenametorLoadStage3(DLst.Union(DEXLst).Union(DELst).ToList<ulong>(), RootID, CMSName).ToString());

                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));



            }
            else
            {

                //RootID = resultlst.Where(a => a.NQF_Number.Trim() == "IPP").ToList<PQRI_Data>()[0].PQRI_Value;
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("value=") + 7, "0");
                // objIPPsb.Replace(@"value=""", @"nullFlavor=""NA""");
                // objIPPsb.Insert(objIPPsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //objIPPsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objIPPsb.Insert(objIPPsb.ToString().IndexOf("<TextIPP>") + 9, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //{
                //    objIPPsb.Insert(objIPPsb.ToString().IndexOf("</TextIPP>") + 10, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());
                //}

                objMeasuresb.Append(objIPPsb.ToString().Replace("<TextIPP>", "").Replace("</TextIPP>", ""));
            }


            // Denominator

            StringBuilder objDenominatorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\DenominatorStage3.xml"));
            objDenominatorsb = new StringBuilder(xDox.ToString());
            objDenominatorsb.Remove(0, 157);
            objDenominatorsb.Remove(objDenominatorsb.Length - 20, 20);
            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENOM").ToList<PQRI_Data>()[0].PQRI_Value;
            if (DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count != 0)
            {

                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count);
                // objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //   objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, IPPAndDenametorLoadStage3(DLst.Union(DEXLst).Union(DELst).ToList<ulong>(), RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }
            else
            {
                //objDenominatorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("value=") + 7, "0");
                //objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objDenominatorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("<TextDenominator>") + 17, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objDenominatorsb.Insert(objDenominatorsb.ToString().IndexOf("</TextDenominator>") + 18, Load155SubXml(DLst155["CMS155(12-17)"][1], DLst155["CMS155(3-11)"][1], RootID).ToString());

                objMeasuresb.Append(objDenominatorsb.ToString().Replace("<TextDenominator>", "").Replace("</TextDenominator>", ""));
            }


            //Numerator

            StringBuilder objNumeratorsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\NumeratorStage3.xml"));
            objNumeratorsb = new StringBuilder(xDox.ToString());
            objNumeratorsb.Remove(0, 157);
            objNumeratorsb.Remove(objNumeratorsb.Length - 20, 20);

            RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "NUMER").ToList<PQRI_Data>()[0].PQRI_Value;
            if (NLst.Count != 0)
            {
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, NLst.Count);
                //  objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                //  objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, IPPAndDenametorLoadStage3(NLst, RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }
            else
            {
                // objNumeratorsb.Replace(@"value=""", @"nullFlavor=""NA""");
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("value=") + 7, "0");
                // objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                // objNumeratorsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("<TextNumerator>") + 15, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                //if (CMSName.StartsWith("CMS155"))
                //    objNumeratorsb.Insert(objNumeratorsb.ToString().IndexOf("</TextNumerator>") + 16, Load155SubXml(DLst155["CMS155(12-17)"][0], DLst155["CMS155(3-11)"][0], RootID).ToString());

                objMeasuresb.Append(objNumeratorsb.ToString().Replace("<TextNumerator>", "").Replace("</TextNumerator>", ""));
            }


            //Exclusion
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEX").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExclusionStage3.xml"));
                objDEsb = new StringBuilder(xDox.ToString());
                objDEsb.Remove(0, 157);
                objDEsb.Remove(objDEsb.Length - 20, 20);


                if (DELst.Count != 0)
                {
                    objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, DELst.Count);
                    //   objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, IPPAndDenametorLoadStage3(DELst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                }
                else
                {
                    if (RootID.Trim() != string.Empty)
                    {
                        //objDEsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEsb.Insert(objDEsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEsb.Insert(objDEsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEsb.Insert(objDEsb.ToString().IndexOf("<TextDenominatorExclusion>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEsb.ToString().Replace("<TextDenominatorExclusion>", "").Replace("</TextDenominatorExclusion>", ""));
                    }
                }
            }

            //Exception
            if (resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>().Count > 0)
            {
                RootID = resultlst.Where(a => a.PQRI_Calculation_Method.Trim() == "DENEXCEP").ToList<PQRI_Data>()[0].PQRI_Value;

                StringBuilder objDEXsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Denominator_ExceptionStage3.xml"));
                objDEXsb = new StringBuilder(xDox.ToString());
                objDEXsb.Remove(0, 157);
                objDEXsb.Remove(objDEXsb.Length - 20, 20);

                if (DEXLst.Count != 0)
                {
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, DEXLst.Count);
                    // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                    objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, IPPAndDenametorLoadStage3(DEXLst, RootID, CMSName).ToString());
                    //if (CMSName.StartsWith("CMS155"))
                    //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                    objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                }
                else
                {





                    if (RootID.Trim() != string.Empty)
                    {
                        // objDEXsb.Replace(@"value=""", @"nullFlavor=""NA""");
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("value=") + 7, "0");
                        // objDEXsb.Insert(objDEXsb.ToString().IndexOf("extension=") + 11, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        //  objDEsb.Insert(objIPPsb.ToString().IndexOf("root=") + 6, RootID);
                        objDEXsb.Insert(objDEXsb.ToString().IndexOf("<TextDenominatorException>") + 26, CATIIILoadForEmptyXMLStage3(RootID, CMSName).ToString());
                        //if (CMSName.StartsWith("CMS155"))
                        //    objDEsb.Insert(objDEsb.ToString().IndexOf("</TextDenominatorExclusion>") + 27, Load155SubXml(DLst155["CMS155(12-17)"][2], DLst155["CMS155(3-11)"][2], RootID).ToString());

                        objMeasuresb.Append(objDEXsb.ToString().Replace("<TextDenominatorException>", "").Replace("</TextDenominatorException>", ""));
                    }
                }
            }




            xmlReqNode = xmlDoc.GetElementsByTagName("value");

            if (DLst.Union(DEXLst).Union(DELst).ToList<ulong>().Count != 0)
                xmlReqNode[0].Attributes[1].Value = CQMPercentage;
            else
            {
                xmlReqNode[0].Attributes.RemoveAll();
                XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
                xAttribute.Value = "REAL";
                xmlReqNode[0].Attributes.Append(xAttribute);
                XmlAttribute xAttributenull = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
                xAttributenull.Value = "NA";
                xmlReqNode[0].Attributes.Append(xAttributenull);
            }
            //xmlReqNode[0].Attributes[1].re
            xmlReqNode = xmlDoc.GetElementsByTagName("Text");
            xmlReqNode[0].InnerXml = objMeasuresb.ToString();
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<Text>", "").Replace("</Text>", "");

            StringBuilder sb1 = new StringBuilder();
            sb1.Append(xmlDoc.InnerXml);
            //xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID) + "\\" +MeasureName.Split('.')[0]+ ".xml");
            StringBuilder sb = new StringBuilder();
            sb1.Remove(0, 156);
            sb1.Remove(sb1.Length - 19, 19);

            return sb.Append(sb1.ToString());
        }


        public StringBuilder IPPAndDenametorLoad(IList<ulong> HumanLst, string RootId, string CMSName)
        {
            StringBuilder objFinalsb = new StringBuilder();
            StaticLookupManager objStaticLookupManager = new StaticLookupManager();
            HumanManager objHumanManager = new HumanManager();
            IList<Human> lstHuman = objHumanManager.GetHumanListById(HumanLst);

            var xDox = new XDocument();
            if (lstHuman != null && lstHuman.Count > 0)
            {
                IList<Human> Malelst = lstHuman.Where(a => a.Sex == "MALE").ToList<Human>();
                IList<Human> Femalelst = lstHuman.Where(a => a.Sex == "FEMALE").ToList<Human>();

                StringBuilder objSexsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Sex_Supplement.xml"));
                objSexsb = new StringBuilder(xDox.ToString());
                objSexsb.Remove(0, 157);
                objSexsb.Remove(objSexsb.Length - 20, 20);

                StringBuilder objFinalSexsb = new StringBuilder();
                StringBuilder objSexMalesb = new StringBuilder();
                if (Malelst != null && Malelst.Count > 0)
                {

                    objSexMalesb.Append(objSexsb.ToString());
                    objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "M");
                    objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("value=") + 7, Malelst.Count.ToString());
                    objFinalSexsb.Append(objSexMalesb.ToString());
                }
                StringBuilder objSexFemalesb = new StringBuilder();
                if (Femalelst != null && Femalelst.Count > 0)
                {

                    objSexFemalesb.Append(objSexsb.ToString());
                    objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "F");
                    objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("value=") + 7, Femalelst.Count.ToString());
                    objFinalSexsb.Append(objSexFemalesb.ToString());
                }

                //Ethinicity_Supplement
                //var aryEthinicity = lstHuman.Select(a => a.Ethnicity).GroupBy(a=>a)
                StringBuilder objEthinicitysb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Ethinicity_Supplement.xml"));
                objEthinicitysb = new StringBuilder(xDox.ToString());
                objEthinicitysb.Remove(0, 157);
                objEthinicitysb.Remove(objEthinicitysb.Length - 20, 20);


                var groups = from h in lstHuman
                             group h by h.Ethnicity into g
                             select new { GroupName = g.Key, Members = g };

                IList<string> lstEthi = new List<string>();

                foreach (var item in groups)
                    lstEthi.Add(item.GroupName);

                StringBuilder objFinalEthinicitysb = new StringBuilder();

                foreach (string items in lstEthi)
                {
                    StringBuilder objEthisb = new StringBuilder();
                    IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("ETHNICITY");
                    objEthisb.Append(objEthinicitysb);
                    objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>().Count > 0 ? lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>()[0].Default_Value : string.Empty);
                    objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, items);
                    objEthisb.Insert(objEthisb.ToString().LastIndexOf("value=") + 7, lstHuman.Count(a => a.Ethnicity.Trim() == items.Trim()));
                    objFinalEthinicitysb.Append(objEthisb.ToString());

                }

                //Race Supplement

                StringBuilder objRacesb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Race_Supplement.xml"));
                objRacesb = new StringBuilder(xDox.ToString());
                objRacesb.Remove(0, 157);
                objRacesb.Remove(objRacesb.Length - 20, 20);


                var Race = from h in lstHuman
                           group h by h.Race into g
                           select new { GroupName = g.Key, Members = g };

                IList<string> lstRace = new List<string>();

                foreach (var item in Race)
                    lstRace.Add(item.GroupName);
                StringBuilder objFinalRacesb = new StringBuilder();
                foreach (string items in lstRace)
                {
                    StringBuilder objRCsb = new StringBuilder();
                    IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("RACE");
                    objRCsb.Append(objRacesb);
                    objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>().Count > 0 ? lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>()[0].Default_Value : string.Empty);
                    objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, items);
                    objRCsb.Insert(objRCsb.ToString().LastIndexOf("value=") + 7, lstHuman.Count(a => a.Race.Trim() == items.Trim()));
                    objFinalRacesb.Append(objRCsb.ToString());

                }

                /////Payer

                StringBuilder objPayersb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Payer_Supplement.xml"));
                objPayersb = new StringBuilder(xDox.ToString());
                objPayersb.Remove(0, 157);
                objPayersb.Remove(objPayersb.Length - 20, 20);

                PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
                IList<PatientInsuredPlan> objPatientInsuredPlan = objPatientInsuredPlanManager.GetPatInsPlanDetails(HumanLst);

                PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();
                IList<PQRI_Data> lstPQRI = objPQRI_DataManager.GetPQRIListByStandardConcept("Payer");
                StringBuilder objFinalPayersb = new StringBuilder();
                int iCount = 0;
                foreach (PQRI_Data item in lstPQRI)
                {

                    if (item.NQF_Number.Trim() != string.Empty)
                    {
                        if (objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.NQF_Number.Trim()) > 0)
                        {
                            StringBuilder objPayerNewsb = new StringBuilder();
                            objPayerNewsb.Append(objPayersb);
                            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.221.5") - 29, item.PQRI_Value);
                            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.221.5") - 14, item.PQRI_Type);
                            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("value=") + 7, objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.NQF_Number.Trim()));
                            objFinalPayersb.Append(objPayerNewsb.ToString());
                            iCount += objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.NQF_Number.Trim());
                        }

                    }

                }

                if (objPatientInsuredPlan != null && (objPatientInsuredPlan.Count - iCount++) != 0)
                {
                    IList<PQRI_Data> lstPqri = lstPQRI.Where(a => a.NQF_Number.Trim() == string.Empty).ToList<PQRI_Data>();
                    if (lstPqri != null)
                    {

                        StringBuilder objPayerCountsb = new StringBuilder();
                        objPayerCountsb.Append(objPayersb);
                        objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.221.5") - 29, lstPqri[0].PQRI_Value);
                        objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.221.5") - 14, lstPqri[0].PQRI_Type);
                        objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("value=") + 7, objPatientInsuredPlan.Count - iCount++);
                        objFinalPayersb.Append(objPayerCountsb.ToString());
                    }

                }




                objFinalsb.Append(objFinalSexsb.ToString());
                objFinalsb.Append(objFinalEthinicitysb.ToString());
                objFinalsb.Append(objFinalRacesb.ToString());
                objFinalsb.Append(objFinalPayersb.ToString());

                /////Reference_Obs
                if (!CMSName.StartsWith("CMS155"))
                {
                    StringBuilder objRefsb = new StringBuilder();
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reference_Obs.xml"));
                    objRefsb = new StringBuilder(xDox.ToString());
                    objRefsb.Remove(0, 157);
                    objRefsb.Remove(objRefsb.Length - 20, 20);

                    objRefsb.Insert(objRefsb.ToString().IndexOf("root=") + 6, RootId);
                    objFinalsb.Append(objRefsb.ToString());
                }


            }
            else /////Human Id =0;
            {
                //objFinalsb.Append(CATIIILoadForEmptyXML().ToString());
            }
            return objFinalsb;

        }
        public StringBuilder MeasureHeaderCount(IList<ulong> NLst, IList<ulong> DLst, IList<ulong> DELst, IList<ulong> DEXLst, string measuresetId)
        {

            HumanManager objHumanManager = new HumanManager();
            StringBuilder objFinalsb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlReqNode = null;
            PatientInsuredPlanManager objpat = new PatientInsuredPlanManager();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\" + "CatIII_Stage3_Measeure_Header_count.xml"));

            xmlReqNode = xmlDoc.GetElementsByTagName("content");
            xmlReqNode[0].InnerText = "Member of Measure Set: Clinical Quality Measure Set " + Convert.ToDateTime(dtpFromDate.SelectedDate).ToString("yyyy-MMM-dd") + " - " + Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MMM-dd") + " - " + measuresetId;

            IList<Human> lstHumanDenomi = new List<Human>();

            IList<Human> lstHumanNumer = new List<Human>();
            IList<Human> lstHumanExclusion = new List<Human>();

            IList<Human> lstHumanException = new List<Human>();
            IList<Human> lsthumantemp = new List<Human>();
            ulong denom_medicare_count = 0, Num_medicare_count = 0, DE_medicare_count = 0;

            ulong denom_medicaid_count = 0, Num_medicaid_count = 0, DE_medicaid_count = 0;

            ulong Dex_medicare_count = 0, Dex_medicaid_count = 0, DeX_other_count = 0;
            ulong denom_other_count = 0, Num_other_count = 0, DE_other_count = 0;
            if (DLst.Count > 0)
            {
                lstHumanDenomi = objHumanManager.GetHumanListById(DLst);

                denom_medicare_count = objpat.GetHumanCountbycarrierID(DLst);
                denom_medicaid_count = objpat.GetHumanCountbycarrierIDMedicaid(DLst);
                denom_other_count = objpat.GetHumanCountbycarrierIDOther(DLst);


            }
            if (NLst.Count > 0)
            {
                lstHumanNumer = objHumanManager.GetHumanListById(NLst);
                Num_medicare_count = objpat.GetHumanCountbycarrierID(NLst);
                Num_medicaid_count = objpat.GetHumanCountbycarrierIDMedicaid(NLst);
                Num_other_count = objpat.GetHumanCountbycarrierIDOther(NLst);
            }
            if (DELst.Count > 0)
            {
                lstHumanExclusion = objHumanManager.GetHumanListById(DELst);
                DE_medicare_count = objpat.GetHumanCountbycarrierID(DELst);
                DE_medicaid_count = objpat.GetHumanCountbycarrierIDMedicaid(DELst);
                DE_other_count = objpat.GetHumanCountbycarrierIDOther(DELst);
            }

            if (DEXLst.Count > 0)
            {
                lstHumanException = objHumanManager.GetHumanListById(DEXLst);
                Dex_medicare_count = objpat.GetHumanCountbycarrierID(DEXLst);
                Dex_medicaid_count = objpat.GetHumanCountbycarrierIDMedicaid(DEXLst);
                DeX_other_count = objpat.GetHumanCountbycarrierIDOther(DEXLst);
            }

            double percentage = 0;
            if (DLst.Count > 0)
                percentage = Convert.ToDouble(Math.Round(decimal.Divide(Convert.ToDecimal(NLst.Count), Convert.ToDecimal(DLst.Count)) * 100, 2));

            xmlReqNode = xmlDoc.GetElementsByTagName("list");

            xmlReqNode[0].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", percentage.ToString() + "%");
            xmlReqNode[0].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", percentage.ToString() + "%");

            if (xmlReqNode[0].ChildNodes[2].InnerText.Contains("Initial Patient Population"))
            {
                if (lstHumanDenomi.Count > 0 || lstHumanExclusion.Count > 0 || DEXLst.Count > 0)
                {
                    IList<Human> lstipp = lstHumanDenomi.Union(lstHumanExclusion).Union(lstHumanException).ToList<Human>();

                    ulong IPP_medicare_count = objpat.GetHumanCountbycarrierID(DLst.Union(DELst).Union(DEXLst).ToList<ulong>());
                    ulong IPP_medicaid_count = objpat.GetHumanCountbycarrierIDMedicaid(DLst.Union(DELst).Union(DEXLst).ToList<ulong>());
                    ulong IPP_other_count = objpat.GetHumanCountbycarrierIDOther(DLst.Union(DELst).Union(DEXLst).ToList<ulong>());

                    xmlReqNode[0].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", lstipp.Count.ToString());
                    lsthumantemp = (from m in lstipp where m.Sex.ToUpper() == "MALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstipp where m.Sex.ToUpper() == "FEMALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstipp where m.Ethnicity == "Not Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());
                    lsthumantemp = (from m in lstipp where m.Ethnicity == "Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstipp where m.Race == "Black or African American" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstipp where m.Race == "White" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstipp where m.Race == "Asian" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstipp where m.Race == "Declined to specify" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstipp where m.Race == "Native Hawaiian or Other Pacific Islander" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstipp where m.Race == "American Indian or Alaska Native" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());



                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", IPP_medicare_count.ToString());


                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", IPP_medicaid_count.ToString());

                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", IPP_other_count.ToString());


                    lsthumantemp = (from m in lstipp where m.ZipCode == "91767" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                }
                else
                {
                    xmlReqNode[0].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", "0");

                    xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[2].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", "0");
                }



            }
            if (xmlReqNode[0].ChildNodes[3].InnerText.Contains("Denominator"))
            {
                if (lstHumanDenomi.Count > 0)
                {
                    xmlReqNode[0].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", lstHumanDenomi.Count.ToString());
                    lsthumantemp = (from m in lstHumanDenomi where m.Sex.ToUpper() == "MALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Sex.ToUpper() == "FEMALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Ethnicity == "Not Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());
                    lsthumantemp = (from m in lstHumanDenomi where m.Ethnicity == "Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "Black or African American" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "White" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "Asian" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "Declined to specify" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "Native Hawaiian or Other Pacific Islander" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "American Indian or Alaska Native" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanDenomi where m.Race == "" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", denom_medicare_count.ToString());


                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", denom_medicaid_count.ToString());

                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", denom_other_count.ToString());


                    lsthumantemp = (from m in lstHumanDenomi where m.ZipCode == "91767" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                }
                else
                {
                    xmlReqNode[0].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", "0");

                    xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[3].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", "0");

                }

            }
            if (xmlReqNode[0].ChildNodes[4].InnerText.Contains("Numerator"))
            {
                if (lstHumanNumer.Count > 0)
                {
                    xmlReqNode[0].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", lstHumanNumer.Count.ToString());
                    lsthumantemp = (from m in lstHumanNumer where m.Sex.ToUpper() == "MALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanNumer where m.Sex.ToUpper() == "FEMALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanNumer where m.Ethnicity == "Not Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());
                    lsthumantemp = (from m in lstHumanNumer where m.Ethnicity == "Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanNumer where m.Race == "Black or African American" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanNumer where m.Race == "White" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanNumer where m.Race == "Asian" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanNumer where m.Race == "Declined to specify" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanNumer where m.Race == "Native Hawaiian or Other Pacific Islander" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanNumer where m.Race == "American Indian or Alaska Native" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", Num_medicare_count.ToString());


                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", Num_medicaid_count.ToString());


                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", Num_other_count.ToString());

                    lsthumantemp = (from m in lstHumanNumer where m.ZipCode == "91767" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                }
                else
                {
                    xmlReqNode[0].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", "0");

                    xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[4].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", "0");

                }

            }
            if (xmlReqNode[0].ChildNodes[5].InnerText.Contains("Denominator Exclusions"))
            {
                if (lstHumanExclusion.Count > 0)
                {
                    xmlReqNode[0].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", lstHumanExclusion.Count.ToString());
                    lsthumantemp = (from m in lstHumanExclusion where m.Sex.ToUpper() == "MALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.Sex.ToUpper() == "FEMALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.Ethnicity == "Not Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());
                    lsthumantemp = (from m in lstHumanExclusion where m.Ethnicity == "Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.Race == "Black or African American" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.Race == "White" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.Race == "Asian" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.Race == "Declined to specify" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanExclusion where m.Race == "Native Hawaiian or Other Pacific Islander" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanExclusion where m.Race == "American Indian or Alaska Native" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());



                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", DE_medicare_count.ToString());


                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", DE_medicaid_count.ToString());


                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", DE_other_count.ToString());

                    lsthumantemp = (from m in lstHumanExclusion where m.ZipCode == "91767" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                }
                else
                {
                    xmlReqNode[0].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", "0");

                    xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", "0");

                }

            }

            if (xmlReqNode[0].ChildNodes[6].InnerText.Contains("Denominator Exceptions"))
            {
                if (lstHumanException.Count > 0)
                {
                    xmlReqNode[0].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", lstHumanException.Count.ToString());
                    lsthumantemp = (from m in lstHumanException where m.Sex.ToUpper() == "MALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.Sex.ToUpper() == "FEMALE" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.Ethnicity == "Not Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[5].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());
                    lsthumantemp = (from m in lstHumanException where m.Ethnicity == "Hispanic or Latino" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.Race == "Black or African American" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.Race == "White" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.Race == "Asian" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.Race == "Declined to specify" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanException where m.Race == "Native Hawaiian or Other Pacific Islander" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());


                    lsthumantemp = (from m in lstHumanException where m.Race == "American Indian or Alaska Native" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());



                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", Dex_medicare_count.ToString());


                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", Dex_medicaid_count.ToString());


                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", DeX_other_count.ToString());

                    lsthumantemp = (from m in lstHumanException where m.ZipCode == "91767" select m).ToList<Human>();
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", lsthumantemp.Count.ToString());

                }
                else
                {
                    xmlReqNode[0].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[1].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[2].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[4].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[5].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[6].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[7].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[8].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[9].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[10].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[11].ChildNodes[1].InnerText.Replace("XTYR", "0");
                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[12].ChildNodes[1].InnerText.Replace("XTYR", "0");

                    xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText = xmlReqNode[0].ChildNodes[6].ChildNodes[2].ChildNodes[13].ChildNodes[1].InnerText.Replace("XTYR", "0");

                }

            }
            objFinalsb = new StringBuilder(xmlDoc.InnerXml.ToString());
            objFinalsb.Remove(0, 38);

            objFinalsb.Replace("<MeaseureCount>", "").Replace("</MeaseureCount>", "");

            return objFinalsb;

        }
        public StringBuilder IPPAndDenametorLoadStage3(IList<ulong> HumanLst, string RootId, string CMSName)
        {
            StringBuilder objFinalsb = new StringBuilder();
            StaticLookupManager objStaticLookupManager = new StaticLookupManager();
            HumanManager objHumanManager = new HumanManager();
            IList<Human> lstHuman = objHumanManager.GetHumanListById(HumanLst);

            var xDox = new XDocument();
            if (lstHuman != null && lstHuman.Count > 0)
            {
                IList<Human> Malelst = lstHuman.Where(a => a.Sex == "MALE").ToList<Human>();
                IList<Human> Femalelst = lstHuman.Where(a => a.Sex == "FEMALE").ToList<Human>();

                StringBuilder objSexsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Sex_Supplement_Stage3.xml"));
                objSexsb = new StringBuilder(xDox.ToString());
                objSexsb.Remove(0, 157);
                objSexsb.Remove(objSexsb.Length - 20, 20);

                StringBuilder objFinalSexsb = new StringBuilder();
                StringBuilder objSexMalesb = new StringBuilder();
                if (Malelst != null && Malelst.Count > 0)
                {

                    objSexMalesb.Append(objSexsb.ToString());
                    objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "M");
                    objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("value=") + 7, Malelst.Count.ToString());

                    if (objSexMalesb.ToString().IndexOf("AdministrativeSex") > -1)
                        objSexMalesb.Insert(objSexMalesb.ToString().IndexOf(@"AdministrativeSex") + 32, "MALE"); ;
                    objFinalSexsb.Append(objSexMalesb.ToString());
                }
                StringBuilder objSexFemalesb = new StringBuilder();
                if (Femalelst != null && Femalelst.Count > 0)
                {

                    objSexFemalesb.Append(objSexsb.ToString());
                    objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "F");
                    if (objSexFemalesb.ToString().IndexOf(@"AdministrativeSex") > -1)
                        objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf(@"AdministrativeSex") + 32, "FEMALE");

                    objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("value=") + 7, Femalelst.Count.ToString());
                    objFinalSexsb.Append(objSexFemalesb.ToString());
                }

                //Ethinicity_Supplement
                //var aryEthinicity = lstHuman.Select(a => a.Ethnicity).GroupBy(a=>a)
                StringBuilder objEthinicitysb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\EhinicityStage3.xml"));
                objEthinicitysb = new StringBuilder(xDox.ToString());
                objEthinicitysb.Remove(0, 157);
                objEthinicitysb.Remove(objEthinicitysb.Length - 20, 20);


                var groups = from h in lstHuman
                             where h.Ethnicity == "Not Hispanic or Latino" || h.Ethnicity == "Hispanic or Latino"
                             group h by h.Ethnicity into g
                             select new { GroupName = g.Key, Members = g };

                IList<string> lstEthi = new List<string>();

                foreach (var item in groups)
                    lstEthi.Add(item.GroupName);

                StringBuilder objFinalEthinicitysb = new StringBuilder();

                foreach (string items in lstEthi)
                {
                    StringBuilder objEthisb = new StringBuilder();
                    IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldNameEthinicity("ETHNICITY");
                    objEthisb.Append(objEthinicitysb);
                    objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>().Count > 0 ? lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>()[0].Default_Value : string.Empty);
                    objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, items);
                    objEthisb.Insert(objEthisb.ToString().LastIndexOf("value=") + 7, lstHuman.Count(a => a.Ethnicity.Trim() == items.Trim()));
                    objFinalEthinicitysb.Append(objEthisb.ToString());

                }

                //Race Supplement

                StringBuilder objRacesb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Race_Stage3.xml"));
                objRacesb = new StringBuilder(xDox.ToString());
                objRacesb.Remove(0, 157);
                objRacesb.Remove(objRacesb.Length - 20, 20);


                var Race = from h in lstHuman
                               //  where h.Race == "Black or African American" || h.Race == "White" || h.Race == "Asian"
                           group h by h.Race into g
                           select new { GroupName = g.Key, Members = g };

                IList<string> lstRace = new List<string>();

                foreach (var item in Race)
                    lstRace.Add(item.GroupName);
                StringBuilder objFinalRacesb = new StringBuilder();
                foreach (string items in lstRace)
                {
                    StringBuilder objRCsb = new StringBuilder();
                    IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldNameRace("RACE");
                    objRCsb.Append(objRacesb);
                    objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>().Count > 0 ? lsdstLookUp.Where(a => a.Value.Trim() == items.Trim()).ToList<StaticLookup>()[0].Default_Value : string.Empty);
                    objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, items);
                    objRCsb.Insert(objRCsb.ToString().LastIndexOf("value=") + 7, lstHuman.Count(a => a.Race.Trim() == items.Trim()));

                    objFinalRacesb.Append(objRCsb.ToString());

                }

                /////Payer

                StringBuilder objPayersb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Payer_Stage3.xml"));
                objPayersb = new StringBuilder(xDox.ToString());
                objPayersb.Remove(0, 157);
                objPayersb.Remove(objPayersb.Length - 20, 20);

                PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
                IList<PatientInsuredPlan> objPatientInsuredPlan = objPatientInsuredPlanManager.GetPatInsPlanDetails(HumanLst);

                PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();
                //IList<PQRI_Data> lstPQRI = objPQRI_DataManager.GetPQRIListByStandardConceptAndPQRIType("Payer",CMSName);
                StringBuilder objFinalPayersb = new StringBuilder();
                int iCount = 0;
                if (objPatientInsuredPlan.Count > 0)
                {

                    //var results = from p in objPatientInsuredPlan
                    //              group p.Insurance_Plan_ID by p.Insurance_Plan_ID into g
                    //              //select new { id = g.ToList() };


                    IList<ulong> InsurancePlanID = objPatientInsuredPlan.Select(a => a.Insurance_Plan_ID).Distinct().ToList<ulong>();
                    InsurancePlanManager objins = new InsurancePlanManager();
                    IList<InsurancePlan> lstplan = objins.GetInsDetails(InsurancePlanID);

                    for (int i = 0; i < lstplan.Count; i++)
                    {
                        StringBuilder objPayerNewsb = new StringBuilder();
                        objPayerNewsb.Append(objPayersb);
                        objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 29, lstplan[i].External_Plan_Number);
                        objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 14, lstplan[i].Ins_Plan_Name);
                        objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("value=") + 7, objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == lstplan[i].Id.ToString().Trim()));
                        objFinalPayersb.Append(objPayerNewsb.ToString());
                        // iCount += objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.PQRI_Value.Trim())
                    }

                    //IList<PatientInsuredPlan> lstpat = objPatientInsuredPlan.GroupBy(a => a.Insurance_Plan_ID).ToList<PatientInsuredPlan>();



                    //            objPayerNewsb.Append(objPayersb);
                    //            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 29, item.PQRI_Value);
                    //            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 14, item.PQRI_Description);
                    //            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("value=") + 7, objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.PQRI_Value.Trim()));
                    //            objFinalPayersb.Append(objPayerNewsb.ToString());
                    //            iCount += objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.PQRI_Value.Trim());
                    //        }

                }

                //foreach (PQRI_Data item in lstPQRI)
                //{

                //    if (item.PQRI_Value.if(Trim() != string.Empty)
                //    {
                //        if (objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.PQRI_Value.Trim()) > 0)
                //        {
                //            StringBuilder objPayerNewsb = new StringBuilder();
                //            objPayerNewsb.Append(objPayersb);
                //            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 29, item.PQRI_Value);
                //            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 14, item.PQRI_Description);
                //            objPayerNewsb.Insert(objPayerNewsb.ToString().IndexOf("value=") + 7, objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.PQRI_Value.Trim()));
                //            objFinalPayersb.Append(objPayerNewsb.ToString());
                //            iCount += objPatientInsuredPlan.Count(a => a.Insurance_Plan_ID.ToString().Trim() == item.PQRI_Value.Trim());
                //        }

                //    }

                //}

                //if (objPatientInsuredPlan != null && (objPatientInsuredPlan.Count - iCount++) != 0)
                //{
                //    IList<PQRI_Data> lstPqri = lstPQRI.Where(a => a.NQF_Number.Trim() == string.Empty).ToList<PQRI_Data>();
                //    if (lstPqri != null && lstPqri.Count>0)
                //    {

                //        StringBuilder objPayerCountsb = new StringBuilder();
                //        objPayerCountsb.Append(objPayersb);
                //        objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 29, lstPqri[0].PQRI_Value);
                //        objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 14, lstPqri[0].PQRI_Type);
                //        objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("value=") + 7, objPatientInsuredPlan.Count - iCount++);
                //        objFinalPayersb.Append(objPayerCountsb.ToString());
                //    }

                //}




                objFinalsb.Append(objFinalSexsb.ToString());
                objFinalsb.Append(objFinalEthinicitysb.ToString());
                objFinalsb.Append(objFinalRacesb.ToString());
                objFinalsb.Append(objFinalPayersb.ToString());

                ///Reference_Obs
                if (!CMSName.StartsWith("CMS155"))
                {
                    StringBuilder objRefsb = new StringBuilder();
                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reference_Obs.xml"));
                    objRefsb = new StringBuilder(xDox.ToString());
                    objRefsb.Remove(0, 157);
                    objRefsb.Remove(objRefsb.Length - 20, 20);

                    objRefsb.Insert(objRefsb.ToString().IndexOf("root=") + 6, RootId);
                    objFinalsb.Append(objRefsb.ToString());
                }


            }
            else /////Human Id =0;
            {
                //objFinalsb.Append(CATIIILoadForEmptyXML().ToString());
            }
            return objFinalsb;

        }
        public StringBuilder CATIIILoadForEmptyXMLStage3(string RootId, string CMSName)
        {

            var xDox = new XDocument();
            //Sex_Supplement
            StringBuilder objSexsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Sex_Supplement_Stage3.xml"));
            objSexsb = new StringBuilder(xDox.ToString());
            objSexsb.Remove(0, 157);
            objSexsb.Remove(objSexsb.Length - 20, 20);

            StringBuilder objFinalSexsb = new StringBuilder();
            StringBuilder objSexMalesb = new StringBuilder();

            objSexMalesb.Append(objSexsb.ToString());
            objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "M");
            objSexMalesb.Insert(objSexMalesb.ToString().IndexOf(@"AdministrativeSex") + 32, "MALE");
            objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("value=") + 7, "0");
            objFinalSexsb.Append(objSexMalesb.ToString());

            StringBuilder objSexFemalesb = new StringBuilder();
            objSexFemalesb.Append(objSexsb.ToString());
            objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "F");
            objSexFemalesb.Insert(objSexMalesb.ToString().IndexOf(@"AdministrativeSex") + 32, "FEMALE");
            objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("value=") + 7, "0");
            objFinalSexsb.Append(objSexFemalesb.ToString());

            //Ethinicity_Supplement
            StringBuilder objEthinicitysb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\EhinicityStage3.xml"));
            objEthinicitysb = new StringBuilder(xDox.ToString());
            objEthinicitysb.Remove(0, 157);
            objEthinicitysb.Remove(objEthinicitysb.Length - 20, 20);

            StringBuilder objFinalEthinicitysb = new StringBuilder();

            StringBuilder objEthisb = new StringBuilder();
            //IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("ETHNICITY");
            objEthisb.Append(objEthinicitysb);
            objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, "0");
            objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, "Ethnicity Not indicated");
            objEthisb.Insert(objEthisb.ToString().LastIndexOf("value=") + 7, "0");
            objFinalEthinicitysb.Append(objEthisb.ToString());

            //Race Supplement

            StringBuilder objRacesb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Race_Stage3.xml"));
            objRacesb = new StringBuilder(xDox.ToString());
            objRacesb.Remove(0, 157);
            objRacesb.Remove(objRacesb.Length - 20, 20);

            StringBuilder objFinalRacesb = new StringBuilder();
            StringBuilder objRCsb = new StringBuilder();
            //IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("RACE");
            objRCsb.Append(objRacesb);
            objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, "0");
            objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, "Race Not Indicated");
            objRCsb.Insert(objRCsb.ToString().LastIndexOf("value=") + 7, "0");
            objFinalRacesb.Append(objRCsb.ToString());

            /////Payer

            StringBuilder objPayersb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Payer_Stage3.xml"));
            objPayersb = new StringBuilder(xDox.ToString());
            objPayersb.Remove(0, 157);
            objPayersb.Remove(objPayersb.Length - 20, 20);

            //PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
            //IList<PatientInsuredPlan> objPatientInsuredPlan = objPatientInsuredPlanManager.GetPatInsPlanDetails(HumanLst);

            PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();
            //IList<PQRI_Data> lstPQRI = objPQRI_DataManager.GetPQRIListByStandardConcept("Payer");
            StringBuilder objFinalPayersb = new StringBuilder();
            StringBuilder objPayerCountsb = new StringBuilder();
            objPayerCountsb.Append(objPayersb);

            objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 29, "D");

            objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.249.12") - 14, "Other");
            //objPayerCountsb.Replace(@"code="" displayName="" codeSystem=""2.16.840.1.113883.3.249.12"" codeSystemName=""CMS Clinical Codes""/>", @"nullFlavor=""NA"" />");
            objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("value=") + 7, "0");
            objFinalPayersb.Append(objPayerCountsb.ToString());

            /////Reference_Obs



            StringBuilder objFinalsb = new StringBuilder();
            objFinalsb.Append(objFinalSexsb.ToString());
            objFinalsb.Append(objFinalEthinicitysb.ToString());
            objFinalsb.Append(objFinalRacesb.ToString());
            objFinalsb.Append(objFinalPayersb.ToString());


            if (!CMSName.StartsWith("CMS155"))
            {
                StringBuilder objRefsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reference_Obs.xml"));
                objRefsb = new StringBuilder(xDox.ToString());
                objRefsb.Remove(0, 157);
                objRefsb.Remove(objRefsb.Length - 20, 20);

                objRefsb.Insert(objRefsb.ToString().IndexOf("root=") + 6, RootId);
                objFinalsb.Append(objRefsb.ToString());
            }



            return objFinalsb;
        }
        public StringBuilder CATIIILoadForEmptyXML(string RootId, string CMSName)
        {

            var xDox = new XDocument();
            //Sex_Supplement
            StringBuilder objSexsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Sex_Supplement.xml"));
            objSexsb = new StringBuilder(xDox.ToString());
            objSexsb.Remove(0, 157);
            objSexsb.Remove(objSexsb.Length - 20, 20);

            StringBuilder objFinalSexsb = new StringBuilder();
            StringBuilder objSexMalesb = new StringBuilder();

            objSexMalesb.Append(objSexsb.ToString());
            objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "M");
            objSexMalesb.Insert(objSexMalesb.ToString().IndexOf("value=") + 7, "0");
            objFinalSexsb.Append(objSexMalesb.ToString());

            StringBuilder objSexFemalesb = new StringBuilder();
            objSexFemalesb.Append(objSexsb.ToString());
            objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("2.16.840.1.113883.5.1") - 14, "F");
            objSexFemalesb.Insert(objSexFemalesb.ToString().IndexOf("value=") + 7, "0");
            objFinalSexsb.Append(objSexFemalesb.ToString());

            //Ethinicity_Supplement
            StringBuilder objEthinicitysb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Ethinicity_Supplement.xml"));
            objEthinicitysb = new StringBuilder(xDox.ToString());
            objEthinicitysb.Remove(0, 157);
            objEthinicitysb.Remove(objEthinicitysb.Length - 20, 20);

            StringBuilder objFinalEthinicitysb = new StringBuilder();

            StringBuilder objEthisb = new StringBuilder();
            //IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("ETHNICITY");
            objEthisb.Append(objEthinicitysb);
            objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, "0");
            objEthisb.Insert(objEthisb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, "Ethnicity Not indicated");
            objEthisb.Insert(objEthisb.ToString().LastIndexOf("value=") + 7, "0");
            objFinalEthinicitysb.Append(objEthisb.ToString());

            //Race Supplement

            StringBuilder objRacesb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Race_Supplement.xml"));
            objRacesb = new StringBuilder(xDox.ToString());
            objRacesb.Remove(0, 157);
            objRacesb.Remove(objRacesb.Length - 20, 20);

            StringBuilder objFinalRacesb = new StringBuilder();
            StringBuilder objRCsb = new StringBuilder();
            //IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("RACE");
            objRCsb.Append(objRacesb);
            objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 29, "0");
            objRCsb.Insert(objRCsb.ToString().IndexOf("2.16.840.1.113883.6.238") - 14, "Race Not Indicated");
            objRCsb.Insert(objRCsb.ToString().LastIndexOf("value=") + 7, "0");
            objFinalRacesb.Append(objRCsb.ToString());

            /////Payer

            StringBuilder objPayersb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Payer_Supplement.xml"));
            objPayersb = new StringBuilder(xDox.ToString());
            objPayersb.Remove(0, 157);
            objPayersb.Remove(objPayersb.Length - 20, 20);

            //PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
            //IList<PatientInsuredPlan> objPatientInsuredPlan = objPatientInsuredPlanManager.GetPatInsPlanDetails(HumanLst);

            PQRI_DataManager objPQRI_DataManager = new PQRI_DataManager();
            //IList<PQRI_Data> lstPQRI = objPQRI_DataManager.GetPQRIListByStandardConcept("Payer");
            StringBuilder objFinalPayersb = new StringBuilder();
            StringBuilder objPayerCountsb = new StringBuilder();
            objPayerCountsb.Append(objPayersb);
            objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.221.5") - 29, "349");
            objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("2.16.840.1.113883.3.221.5") - 14, "Other");
            objPayerCountsb.Insert(objPayerCountsb.ToString().IndexOf("value=") + 7, "0");
            objFinalPayersb.Append(objPayerCountsb.ToString());

            /////Reference_Obs



            StringBuilder objFinalsb = new StringBuilder();
            objFinalsb.Append(objFinalSexsb.ToString());
            objFinalsb.Append(objFinalEthinicitysb.ToString());
            objFinalsb.Append(objFinalRacesb.ToString());
            objFinalsb.Append(objFinalPayersb.ToString());


            if (!CMSName.StartsWith("CMS155"))
            {
                StringBuilder objRefsb = new StringBuilder();
                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reference_Obs.xml"));
                objRefsb = new StringBuilder(xDox.ToString());
                objRefsb.Remove(0, 157);
                objRefsb.Remove(objRefsb.Length - 20, 20);

                objRefsb.Insert(objRefsb.ToString().IndexOf("root=") + 6, RootId);
                objFinalsb.Append(objRefsb.ToString());
            }



            return objFinalsb;
        }

        #endregion CAT III End

        public DateTime ConvertToLocal(DateTime inputDateTime)
        {
            //inputDateTime = inputDateTime.ToLocalTime();

            //return inputDateTime;

            var request = hdnLocalTime.Value.ToString();
            DateTime dt;
            //if (IsCookieDefined(request))
            //{
            if (inputDateTime.ToString("dd-MM-yyyy hh:mm:ss tt") != "01-01-0001 12:00:00 AM")
            {
                dt = inputDateTime.AddMinutes(-(double.Parse(request)));
            }
            else
            {
                dt = inputDateTime;
            }

            return dt;
        }


        public void DownLoadZIPFormate(string DirName)
        {


            //DirName =Server.MapPath("Documents/y32wr0u4s2rr0wji4itvyb55/CQMI");
            // string basePath = DirName;
            //Queue<string> subDirectories = new Queue<string>();
            //subDirectories.Enqueue(basePath);
            //while (subDirectories.Count > 0)
            //{
            //     DirectoryInfo directorySelected = new DirectoryInfo(DirName);
            //    DirectoryInfo[] diArr = directorySelected.GetDirectories();
            //    foreach (DirectoryInfo itemDir in diArr)
            //    {
            //        subDirectories.Enqueue(itemDir.FullName);
            //    }

            //    //foreach (var subDirectory in Directory.EnumerateDirectories(basePath))
            //    //{
            //    //    subDirectories.Enqueue(subDirectory);
            //    //}
            //    break;
            //}
            //return;

            #region  Old
            //using (ZipFile zip = new ZipFile())
            //{
            //    zip.AlternateEncodingUsage = ZipOption.AsNecessary;


            //    DirectoryInfo directorySelected = new DirectoryInfo(DirName);
            //    DirectoryInfo[] diArr = directorySelected.GetDirectories();

            //    foreach (DirectoryInfo itemDir in diArr)
            //    {

            //        foreach (FileInfo fileToCompress in itemDir.GetFiles("*.xml"))
            //        {

            //            //string filePath = fileToCompress.FullName;
            //            //zip.AddFile(filePath, itemDir.FullName.Substring(itemDir.FullName.LastIndexOf("\\") + 1));
            //            //string fileName = fileToCompress.Name;
            //            //string filePath = Server.MapPath("~/Documents/" + Session.SessionID + "/CQMI/" + itemDir.FullName.Substring(itemDir.FullName.LastIndexOf("\\") + 1)+"/"+fileName); //fileToCompress.FullName;
            //            //string fileName = "CQMCATIII.xml";
            //            string filePath = fileToCompress.FullName;//Server.MapPath("~/Documents/" + Session.SessionID + "/CQMI/" + fileName); //fileToCompress.FullName;
            //            zip.AddFile(filePath, "");
            //        }

            //    }




            //    Response.Clear();
            //    Response.BufferOutput = false;
            //    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
            //    Response.ContentType = "application/zip";
            //    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
            //    zip.Save(Response.OutputStream);
            //    Response.End();
            //}
            #endregion


            //using (ZipFile zip = new ZipFile())
            //{
            //    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
            //    DirectoryInfo directorySelected = new DirectoryInfo(DirName);
            //    DirectoryInfo[] diArr = directorySelected.GetDirectories();
            //    IList<string>lst=new List<string>();
            //    foreach (DirectoryInfo itemDir in diArr)
            //    {
            //        ////zip.AddDirectoryByName(itemDir.FullName.Substring(itemDir.FullName.LastIndexOf("\\") + 1));
            //        foreach (FileInfo fileToCompress in itemDir.GetFiles("*.xml"))
            //        {
            //            string filePath = fileToCompress.FullName;
            //            ////zip.AddFile(filePath, itemDir.FullName.Substring(itemDir.FullName.LastIndexOf("\\") + 1));
            //            //zip.AddFiles(filePath);//.AddFile(filePath, "");
            //            //break;
            //        }
            //    }



            string folder_Name = DirName;
            if (Directory.Exists(folder_Name))
            {
                string[] sub_folder_Names = Directory.GetDirectories(folder_Name);
                if (sub_folder_Names.Length > 0)
                {
                    foreach (string subFolder in sub_folder_Names)
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddDirectory(subFolder);
                            zip.Save(subFolder + ".zip");
                            Directory.Delete(subFolder, true);
                        }
                    }
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddDirectory(folder_Name);
                        Response.Clear();
                        Response.BufferOutput = false;
                        string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                        //zip.Save(folder_Name + ".zip");
                        //Directory.Delete(folder_Name, true);
                    }
                }
            }
            //}
        }


        public void DownLoadZIPFormateCATII(string DirName)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                DirectoryInfo directorySelected = new DirectoryInfo(DirName);
                string fileName = "CQMCATIII.xml";
                string filePath = Server.MapPath("~/Documents/" + Session.SessionID + "/CQMIII/" + fileName); //fileToCompress.FullName;
                zip.AddFile(filePath, "");
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = "CATIII.zip";
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }


        public StringBuilder Load155SubXml(int x, int y, string RootId)
        {
            StringBuilder objStringBuilder = new StringBuilder();

            var xDox = new XDocument();
            StringBuilder objReportingsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reporting Stratum.xml"));
            objReportingsb = new StringBuilder(xDox.ToString());
            objReportingsb.Remove(0, 157);
            objReportingsb.Remove(objReportingsb.Length - 20, 20);
            objReportingsb.Insert(objReportingsb.ToString().IndexOf("value=") + 7, x);
            objStringBuilder.Append(objReportingsb.ToString());

            StringBuilder objReportingsb1 = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reporting Stratum1.xml"));
            objReportingsb1 = new StringBuilder(xDox.ToString());
            objReportingsb1.Remove(0, 157);
            objReportingsb1.Remove(objReportingsb1.Length - 20, 20);
            objReportingsb1.Insert(objReportingsb1.ToString().IndexOf("value=") + 7, y);
            objStringBuilder.Append(objReportingsb1.ToString());


            StringBuilder objRefsb = new StringBuilder();
            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Reference_Obs.xml"));
            objRefsb = new StringBuilder(xDox.ToString());
            objRefsb.Remove(0, 157);
            objRefsb.Remove(objRefsb.Length - 20, 20);
            objRefsb.Insert(objRefsb.ToString().IndexOf("root=") + 6, RootId);
            objStringBuilder.Append(objRefsb.ToString());
            return objStringBuilder;
        }

        protected void btnCATIZIP_Click(object sender, EventArgs e)
        {
            IList<PQRIResultDTO> ResultPqri = new List<PQRIResultDTO>();
            ResultPqri = (IList<PQRIResultDTO>)Session["CQMList"];
            if (cboStage.Text == "Stage 2")
                LoadXML(ResultPqri);
            if (cboStage.Text == "Stage 3")//Stage 3 Measure
                LoadXMLCATIStageThreeMeasure(ResultPqri);
            //this part moved to btndownloadcat1 buttonexclusi
            //DownLoadZIPFormate(Server.MapPath("Documents/" + Session.SessionID + "/CQMI"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Download", "DownloadCaTIFile();", true);
            btnCATIZIP.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "StopLoading", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void grdMeasureCalculation_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                string MeasureName = e.Item.Cells[2].Text;
                string Num_Count = e.Item.Cells[8].Text;
                string Den_Count = e.Item.Cells[5].Text;
                string Num_Human_id = e.Item.Cells[15].Text;
                string Den_Human_id = e.Item.Cells[14].Text;
                if (Den_Human_id == string.Empty)
                    Den_Human_id = "0";
                Session["Num_Hum_IDs"] = Num_Human_id;
                Session["Den_Hum_IDs"] = Den_Human_id;
                PQRIMeasureWindow.VisibleStatusbar = false;
                PQRIMeasureWindow.Visible = true;
                PQRIMeasureWindow.VisibleOnPageLoad = true;
                PQRIMeasureWindow.Width = 1150;
                PQRIMeasureWindow.Height = 670;
                //Commented by naveena .View from birt tool
                //  PQRIMeasureWindow.NavigateUrl = "frmEhrMeasureHumanDetail.aspx?MeasureName=" + MeasureName + "&Num_Count=" + Num_Count + "&Den_Count=" + Den_Count;
                PQRIMeasureWindow.NavigateUrl = "frmPQRIBirtViewDetail.aspx?";

            }
        }

        #region CAT I Stage Three

        //public void LoadXMLCATIStageThreeMeasure(IList<PQRIResultDTO> PQRIResultDTO)
        //{
        //    sClientTin = System.Configuration.ConfigurationSettings.AppSettings["ClientTIN"];
        //    sClientName = System.Configuration.ConfigurationSettings.AppSettings["ClientName"];
        //    IList<PQRIResultDTO> PQRIResultDTOList = PQRIResultDTO;
        //    EncounterManager objEncounterManager = new EncounterManager();
        //    var Measures = PQRIResultDTOList.Select(a => a.MeasureNo).Distinct();//.GroupBy(a => a.MeasureNo);
        //    IDictionary<string, string> DMeasureNamesLst = (IDictionary<string, string>)Session["MeasurePercentage"];
        //    PQRI_DataManager objPQRIManager = new PQRI_DataManager();
        //    IList<PQRI_Data> lst_Pqri_Data_FullList = objPQRIManager.GetPQRIListBySectionForStage3();
        //    IList<Encounter> EncLst = objEncounterManager.GetEncoutnerListByPhyID((int)ClientSession.PhysicianId);
        //    StaticLookupManager objStaticLookupManager = new StaticLookupManager();
        //    IList<StaticLookup> lststaticlookup = new List<StaticLookup>();
        //    if (lstfacility == null || (lstfacility != null && lstfacility.Count == 0))//BugID:54511
        //    {
        //        lstfacility = objfacility.GetFacilityListByFacilityName(ClientSession.FacilityName);
        //    }
        //    foreach (var MeasureName in DMeasureNamesLst)
        //    {
        //        string Name = MeasureName.Key.Replace(" ", "");


        //        IList<ulong> lstHuman = PQRIResultDTOList.Where(a => a.MeasureName == MeasureName.Key.Replace(" ", "")).Select(a => a.HumanID).Distinct().ToList<ulong>();
        //        IList<PQRI_Data> lst_Pqri_Data = lst_Pqri_Data_FullList.Where(a => a.NQF_Number == Name).ToList<PQRI_Data>();
        //        foreach (ulong Item_Human_ID in lstHuman)
        //        {
        //            flagmedication = 0;
        //            iRandomNo++;
        //            StringBuilder XMLInsertsb = new StringBuilder();
        //            HumanManager objHumanManager = new HumanManager();


        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\XML Master Template_Stage3Measure_v10.xml"));

        //            string FolderName = string.Empty;

        //            if (MeasureName.Key.Trim() == "CMS 69.1")
        //                FolderName = "CMS 69";
        //            else
        //                FolderName = MeasureName.Key;

        //            DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName));
        //            if (!objdirect.Exists)
        //                objdirect.Create();

        //            Human HumanLst = objHumanManager.GetById(Item_Human_ID);

        //            #region
        //            XmlNodeList xmlReqNode = null;
        //            xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
        //            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");

        //            PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
        //            IList<PatientInsuredPlan> PatientInsuredPlanlst = objPatientInsuredPlanManager.GetActiveInsurancePoliciesByHumanId(Item_Human_ID);
        //            if (PatientInsuredPlanlst.Count > 0)
        //            {
        //                xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
        //                xmlReqNode[0].ChildNodes[1].Attributes[0].Value = PatientInsuredPlanlst[0].Policy_Holder_ID;
        //            }
        //            else
        //            {
        //                xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
        //                xmlReqNode[0].ChildNodes[1].Attributes[0].Value = "0";

        //            }
        //            xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
        //            xmlReqNode[0].InnerText = HumanLst.Street_Address1;
        //            xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
        //            xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
        //            xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
        //            xmlReqNode[4].InnerText = ClientSession.PhysicainDetails[0].PhyAddress2;
        //            xmlReqNode[5].InnerText = lstfacility[0].Fac_Address1;
        //            //xmlReqNode[5].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
        //            xmlReqNode[6].InnerText = HumanLst.Street_Address1;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("city");
        //            xmlReqNode[0].InnerText = HumanLst.City;
        //            xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
        //            xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
        //            xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
        //            xmlReqNode[5].InnerText = lstfacility[0].Fac_City;
        //            xmlReqNode[4].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
        //            xmlReqNode[6].InnerText = HumanLst.City;



        //            xmlReqNode = xmlDoc.GetElementsByTagName("state");
        //            xmlReqNode[0].InnerText = HumanLst.State;
        //            xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyState;
        //            xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyState;
        //            xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyState;
        //            xmlReqNode[5].InnerText = lstfacility[0].Fac_State;
        //            xmlReqNode[4].InnerText = ClientSession.PhysicainDetails[0].PhyState;
        //            xmlReqNode[6].InnerText = HumanLst.State;


        //            xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
        //            xmlReqNode[0].InnerText = HumanLst.ZipCode;
        //            xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
        //            xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
        //            xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
        //            xmlReqNode[5].InnerText = lstfacility[0].Fac_Zip;
        //            xmlReqNode[4].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
        //            xmlReqNode[6].InnerText = HumanLst.ZipCode;


        //            xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
        //            if (HumanLst.Work_Phone_No.Trim() != "" && HumanLst.Work_Phone_No.Trim() != "0")
        //            {
        //                xmlReqNode[0].Attributes[1].Value = HumanLst.Work_Phone_No;
        //                xmlReqNode[6].Attributes[1].Value = HumanLst.Home_Phone_No;
        //                // xmlReqNode[6].Attributes[1].Value = HumanLst.Work_Phone_No;
        //            }
        //            else
        //            {
        //                xmlReqNode[0].Attributes[1].Value = HumanLst.Home_Phone_No;
        //                xmlReqNode[6].Attributes[1].Value = HumanLst.Home_Phone_No;
        //                //  xmlReqNode[6].Attributes[1].Value = HumanLst.Home_Phone_No;
        //            }
        //            xmlReqNode[1].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyTelephone;
        //            xmlReqNode[2].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyTelephone;
        //            xmlReqNode[3].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyTelephone;
        //            xmlReqNode[4].Attributes[1].Value = lstfacility[0].Fac_Telephone;
        //            xmlReqNode[5].Attributes[1].Value = lstfacility[0].Fac_Telephone;

        //            bool isDeleteGiven = false;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("given");
        //            xmlReqNode[0].InnerText = HumanLst.First_Name;
        //            if (HumanLst.MI.Trim() != "")
        //            {
        //                xmlReqNode[1].InnerText = HumanLst.MI;
        //                // xmlReqNode[6].InnerText = HumanLst.MI;
        //            }
        //            else
        //            {
        //                xmlReqNode[1].ParentNode.RemoveChild(xmlReqNode[1]);//If middle name not exists delete last given
        //                xmlReqNode[5].ParentNode.RemoveChild(xmlReqNode[5]);
        //                isDeleteGiven = true;
        //            }
        //            if (isDeleteGiven == false)
        //            {
        //                xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
        //                xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
        //                xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;

        //                xmlReqNode[4].InnerText = HumanLst.First_Name;

        //            }
        //            else
        //            {
        //                xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
        //                xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;

        //                xmlReqNode[3].InnerText = HumanLst.First_Name;

        //            }

        //            xmlReqNode = xmlDoc.GetElementsByTagName("family");
        //            xmlReqNode[0].InnerText = HumanLst.Last_Name;
        //            xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyLastName;
        //            xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyLastName;
        //            xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
        //            xmlReqNode[4].InnerText = HumanLst.Last_Name;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
        //            xmlReqNode[0].InnerText = ClientSession.PhysicainDetails[0].PhySuffix;
        //            xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhySuffix;


        //            xmlReqNode = xmlDoc.GetElementsByTagName("representedCustodianOrganization");

        //            xmlReqNode[0].ChildNodes[1].Attributes[1].Value = lstfacility[0].Fac_NPI;
        //            xmlReqNode[0].ChildNodes[7].Attributes[1].Value = sClientTin;
        //            IList<XmlElement> lst = xmlReqNode[0].OfType<XmlElement>().Where(Z => Z.Name == "name").ToList();
        //            lst[0].InnerText = sClientName;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("representedOrganization");
        //            lst = xmlReqNode[0].OfType<XmlElement>().Where(Z => Z.Name == "name").ToList();
        //            lst[0].InnerText = sClientName;

        //            //id extension filled //id root="2.16.840.1.113883.4.6"
        //            //  xmlReqNode[1].ParentNode.ChildNodes[2].Attributes[1].Value = lstfacility[0].Fac_NPI;

        //            // <id root="2.16.840.1.113883.4.2"
        //            //xmlReqNode[1].ChildNodes[1].Attributes[1].Value = sClientTin;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
        //            if (HumanLst.Sex.Trim() != "")
        //                xmlReqNode[0].Attributes[0].Value = HumanLst.Sex.Substring(0, 1).ToUpper();//(HumanLst.Sex.ToUpper() == "MALE") ? "M" : "F";

        //            xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
        //            xmlReqNode[0].Attributes[0].Value = HumanLst.Birth_Date.ToString("yyyyMMdd");


        //            if (HumanLst.Marital_Status.Trim() != "")
        //            {
        //                xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
        //                xmlReqNode[0].Attributes[0].Value = HumanLst.Marital_Status.Substring(0, 1).ToUpper();
        //                xmlReqNode[0].Attributes[1].Value = HumanLst.Marital_Status;
        //            }
        //            else
        //            {
        //                xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
        //                xmlReqNode[0].Attributes.RemoveAll();
        //                XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
        //                xAttribute.Value = "NI";
        //                xmlReqNode[0].Attributes.Append(xAttribute);
        //            }

        //            xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
        //            lststaticlookup = objStaticLookupManager.getStaticLookupByFieldName("RACE");

        //            lststaticlookup = lststaticlookup.Where(a => a.Value.Trim() == HumanLst.Race).ToList<StaticLookup>();
        //            if (lststaticlookup.Count > 0)
        //                xmlReqNode[0].Attributes[0].Value = lststaticlookup[0].Default_Value;

        //            xmlReqNode[0].Attributes[1].Value = HumanLst.Race;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
        //            xmlReqNode[0].Attributes[0].Value = Convert.ToString(HumanLst.E


        //            //xmlReqNode = xmlDoc.GetElementsByTagName("languageCode");
        //            //xmlReqNode[1].Attributes[0].Value = HumanLst.Preferred_Language;

        //            //Physician
        //            xmlReqNode = xmlDoc.GetElementsByTagName("time");
        //            xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
        //            xmlReqNode[1].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
        //            xmlReqNode[2].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");

        //            xmlReqNode = xmlDoc.GetElementsByTagName("id");

        //            xmlReqNode[3].Attributes[0].Value = ClientSession.PhysicainDetails[0].PhyNPI;
        //            xmlReqNode[5].Attributes[0].Value = ClientSession.PhysicainDetails[0].PhyNPI;
        //            xmlReqNode[6].Attributes[1].Value = lstfacility[0].Fac_NPI;
        //            xmlReqNode[8].Attributes[1].Value = sClientTin;
        //            xmlReqNode[13].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyNPI;
        //            xmlReqNode[14].Attributes[1].Value = sClientTin;

        //            xmlReqNode = xmlDoc.GetElementsByTagName("assignedAuthor");
        //            lst = xmlReqNode[0].ChildNodes.OfType<XmlElement>().Where(Z => Z.Name == "code").ToList();
        //            lst[0].Attributes[0].Value = ClientSession.PhysicainDetails[0].Taxonomy_Code;
        //            lst[0].Attributes[2].Value = ClientSession.PhysicainDetails[0].Taxonomy_Description;

        //            //legalAuthenticator
        //            xmlReqNode = xmlDoc.GetElementsByTagName("legalAuthenticator");
        //            lst = xmlReqNode[0].OfType<XmlElement>().Where(Z => Z.Name == "assignedEntity").ToList();
        //            lst = lst[0].OfType<XmlElement>().Where(Z => Z.Name == "code").ToList();
        //            lst[0].Attributes[0].Value = ClientSession.PhysicainDetails[0].Taxonomy_Code;
        //            lst[0].Attributes[1].Value = ClientSession.PhysicainDetails[0].Taxonomy_Description;

        //            //serviceEvent

        //            string sLow = string.Empty;
        //            string sHigh = string.Empty;
        //            //  string  sLow = string.Empty;
        //            if (EncLst != null && EncLst.Count > 0)
        //            {
        //                sLow = ConvertToLocal(EncLst.OrderBy(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
        //                sHigh = ConvertToLocal(EncLst.OrderByDescending(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
        //            }
        //            xmlReqNode = xmlDoc.GetElementsByTagName("low");
        //            xmlReqNode[0].Attributes[0].Value = sLow;
        //            xmlReqNode = xmlDoc.GetElementsByTagName("high");
        //            xmlReqNode[0].Attributes[0].Value = sHigh;

        //            //performer
        //            xmlReqNode = xmlDoc.GetElementsByTagName("low");
        //            xmlReqNode[1].Attributes[0].Value = sLow;
        //            xmlReqNode = xmlDoc.GetElementsByTagName("high");
        //            xmlReqNode[1].Attributes[0].Value = sHigh;

        //            //serviceEvent
        //            xmlReqNode = xmlDoc.GetElementsByTagName("performer");
        //            lst = xmlReqNode[0].ChildNodes[1].OfType<XmlElement>().Where(Z => Z.Name == "code").ToList();
        //            lst[0].Attributes[0].Value = ClientSession.PhysicainDetails[0].Taxonomy_Code;
        //            lst[0].Attributes[1].Value = ClientSession.PhysicainDetails[0].Taxonomy_Description;


        //            // CHOSEN IN MEAsURE CALCULATOR
        //           // xmlReqNode = xmlDoc.GetElementsByTagName("item");
        //           // xmlReqNode[0].InnerText = "Reporting Parameter " + dtpFromDate.SelectedDate.Value.ToString("yyyyMMddhhmmss") + " To " + dtpToDate.SelectedDate.Value.ToString("yyyyMMddhhmmss");
        //            #endregion

        //            //1.Encounter Xml File Indert
        //            IList<Encounter> EncList = new List<Encounter>();
        //            //IList<ulong> Items_Enc = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.EncounterID != 0).Select(a => a.EncounterID).ToList<ulong>(); //(from e in PQRIResultDTOList where e.HumanID == Item_Human_ID select  new {e.ICD}).ToList();//.ToList<string>();
        //            IList<string> Items_Enc = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.EncounterID != 0).Select(a => a.EncounterID.ToString()).ToList<string>(); //(from e in PQRIResultDTOList where e.HumanID == Item_Human_ID select  new {e.ICD}).ToList();//.ToList<string>();
        //            if (Items_Enc != null && Items_Enc.Count > 0)
        //            {
        //                //EncList = objEncounterManager.GetEncounterList(Items_Enc);
        //                EncList = objEncounterManager.GetEncounterListArchive(Items_Enc);
        //            }

        //            StreamWriter file = new StreamWriter(HttpContext.Current.Server.MapPath("SampleXML" + "\\Test.txt"));



        //            IList<PQRIResultDTO> objICDlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.ICD.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //            IList<string[]> ICD = new List<string[]>();
        //            //Dictionary<string, string> DICD = new Dictionary<string, string>();
        //            if (HumanLst.Last_Name == "Estrada" && HumanLst.First_Name == "Jeff" && MeasureName.Key.ToString().Replace(" ", "") == "CMS22v5")
        //            {
        //                PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(p => p.NQF_Number == "CMS22v5" && p.PQRI_Type == "EXCEPTION").ToList<PQRI_Data>()[0];
        //                XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, null, null, "", HumanLst).ToString());
        //                xmlReqNode = xmlDoc.GetElementsByTagName("Test");
        //                xmlReqNode[0].InnerXml = XMLInsertsb.ToString();
        //                xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<Test>", "").Replace("</Test>", "");
        //                xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName) + "\\" + HumanLst.Id + "_" + HumanLst.First_Name + "_" + HumanLst.Last_Name + ".xml");
        //                file.Close();
        //                continue;
        //            }
        //            if (HumanLst.Last_Name == "Arnold" && HumanLst.First_Name == "Sarah" && MeasureName.Key.ToString().Replace(" ", "") == "CMS22v5")
        //            {
        //                PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(p => p.NQF_Number == "CMS22v5" && p.PQRI_Type == "EXCEPTION_SNOMED").ToList<PQRI_Data>()[0];
        //                XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, null, null, "", HumanLst).ToString());
        //                xmlReqNode = xmlDoc.GetElementsByTagName("Test");
        //                xmlReqNode[0].InnerXml = XMLInsertsb.ToString();
        //                xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<Test>", "").Replace("</Test>", "");
        //                xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName) + "\\" + HumanLst.Id + "_" + HumanLst.First_Name + "_" + HumanLst.Last_Name + ".xml");
        //                file.Close();
        //                continue;
        //            }


        //            foreach (PQRIResultDTO item in objICDlst)
        //            {

        //                if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.ICD.Trim() && item.MeasureName == a.NQF_Number) && !ICD.Any(a => a[0].Trim() == item.ICD.Trim()))// && a[1].Trim() == item.EncounterID.ToString().Trim()))
        //                {
        //                    ICD.Add(new string[] { item.ICD.Trim(), item.EncounterID.ToString().Trim() });
        //                    PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
        //                    Encounter objICD_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
        //                    string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                    if (MeasureName.Key.Replace(" ", "") == "CMS138v5" && item.ICD.Trim().ToString() == "R69")
        //                    {
        //                        iRandomNo++;
        //                        ICD_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                        sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                        ICD_Item_lst.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ICD.Trim().ToString());
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, item, objICD_Enc, sValueSet, HumanLst).ToString());
        //                        ICD_Item_lst.PQRI_Value = item.ICD.Trim();
        //                    }
        //                    else if (MeasureName.Key.Replace(" ", "") == "CMS147v6" && (item.ICD.Trim().ToString() == "Z88.7" || item.ICD.Trim().ToString() == "T50.B95A" || item.ICD.Trim().ToString() == "T50.B95D" || item.ICD.Trim().ToString() == "Z28.29"))// || item.ICD.Trim().ToString() == "Z28.21"))
        //                    {
        //                        iRandomNo++;
        //                        ICD_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                        sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                        ICD_Item_lst.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ICD.Trim().ToString());
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, item, objICD_Enc, sValueSet, HumanLst).ToString());
        //                        ICD_Item_lst.PQRI_Value = item.ICD.Trim();
        //                    }
        //                    else
        //                    {
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, item, objICD_Enc, sValueSet, HumanLst).ToString());
        //                    }

        //                }
        //                else
        //                {
        //                    file.WriteLine(item.ICD.Trim());
        //                }

        //            }

        //            //IList<string> Loinc = new List<string>();
        //            IList<string[]> Loinc = new List<string[]>();
        //            IList<PQRIResultDTO> objLoinclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.LoincIdentifier.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //            string sValueSetLoinc = string.Empty;
        //            foreach (PQRIResultDTO item in objLoinclst)
        //            {
        //                if (item.LoincIdentifier.Trim() == "55284-4")
        //                {
        //                    //try
        //                    //{
        //                    PQRI_Data Dia_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8462-4").ToList<PQRI_Data>()[0];
        //                    sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8462-4" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                    XMLInsertsb.Append(SubXMLLoadForSysAndDia(Dia_Item_lst, item, "55284-4", sValueSetLoinc).ToString());

        //                    PQRI_Data Sys_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8480-6").ToList<PQRI_Data>()[0];
        //                    sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "8480-6" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                    XMLInsertsb.Append(SubXMLLoadForSysAndDia(Sys_Item_lst, item, "55284-4", sValueSetLoinc).ToString());
        //                    //}
        //                    //catch(Exception ex)
        //                    //{

        //                    //}
        //                }
        //                else if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim()) && !Loinc.Any(a => a[0].Trim() == item.LoincIdentifier.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
        //                {
        //                    //Loinc.Add(item.LoincIdentifier.Trim());
        //                    //try
        //                    //{
        //                    Loinc.Add(new string[] { item.LoincIdentifier.Trim(), item.EncounterID.ToString().Trim() });
        //                    PQRI_Data Loinc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == item.MeasureName).ToList<PQRI_Data>()[0];
        //                    Encounter objLoinc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];

        //                    sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>().Count > 0 ? lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set : string.Empty;
        //                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Loinc_Item_lst, item, objLoinc_Enc, sValueSetLoinc, HumanLst).ToString());
        //                    //}
        //                    //catch(Exception ex)
        //                    //{

        //                    //}
        //                }
        //                else
        //                {
        //                    file.WriteLine(item.LoincIdentifier.Trim());

        //                }

        //            }


        //            IList<string[]> Procedure = new List<string[]>();


        //            IList<PQRIResultDTO> objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //            //if (MeasureName.Key == "CMS127N")
        //            //    objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //            if (objProclst != null && objProclst.Count > 0 && (objProclst[0].MeasureNo.ToString().ToUpper().Trim().IndexOf("CMS68") != -1))//|| objProclst[0].MeasureNo.ToString().ToUpper().Trim().IndexOf("CMS69") != -1))
        //            {
        //                int k = 100;
        //                if (objProclst[0].MeasureNo.ToString().ToUpper().Trim().IndexOf("CMS68") != -1)
        //                    lst_Pqri_Data = lst_Pqri_Data.Where(a => a.NQF_Number == "CMS68v6").ToList<PQRI_Data>();
        //                else if (objProclst[0].MeasureNo.ToString().ToUpper().Trim().IndexOf("CMS69") != -1)
        //                    lst_Pqri_Data = lst_Pqri_Data.Where(a => a.NQF_Number == "CMS69v5").ToList<PQRI_Data>();
        //                IList<ulong> dis_Encounters = objProclst.Select(a => a.EncounterID).Distinct().ToList<ulong>();
        //                foreach (ulong encId in dis_Encounters)
        //                {


        //                    IList<PQRIResultDTO> objprocLists = objProclst.Where(a => a.EncounterID == encId).ToList<PQRIResultDTO>();

        //                    IList<PQRIResultDTO> objprocVisitLst1 = objprocLists.Where(a => System.Text.RegularExpressions.Regex.IsMatch(a.ProcedureCode, @"^[0-9]+$") && Convert.ToInt64(a.ProcedureCode) >= 99201 && Convert.ToInt64(a.ProcedureCode) <= 99215).ToList<PQRIResultDTO>();
        //                    IList<PQRIResultDTO> objOtherLst1 = objprocLists.Where(a => System.Text.RegularExpressions.Regex.IsMatch(a.ProcedureCode, @"^[G][0-9]+$")).ToList<PQRIResultDTO>();

        //                    if (objprocLists.Count > 1 && objprocVisitLst1.Count > 1 && objOtherLst1.Count == 0)
        //                    {
        //                        if (objprocVisitLst1.Count == 2)
        //                        {
        //                            IList<PQRIResultDTO> objprocVisitLst2 = objprocLists.Where(a => a.ProcedureCode == "99215").ToList<PQRIResultDTO>();
        //                            IList<PQRIResultDTO> objprocVisitLst3 = objprocLists.Where(a => a.ProcedureCode != "99215").OrderBy(a => a.ProcedureCode).ToList<PQRIResultDTO>();
        //                            UtilityManager objUtManager = new UtilityManager();
        //                            if (objprocVisitLst2.Count > 0)
        //                            {
        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst3[0].ProcedureCode).ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst2[0].ProcedureCode).ToList<PQRI_Data>()[0];
        //                                    Proc_Item_lst.PQRI_Value = objUtManager.GetSnomedForCPTCATIMeasure(objprocVisitLst2[0].ProcedureCode);
        //                                    string flag = Proc_Item_lst.PQRI_Type;
        //                                    Proc_Item_lst.PQRI_Type = "Visit";
        //                                    Encounter objProc_Enc = EncList.Where(a => a.Id == encId).ToList<Encounter>()[0];
        //                                    string sValueSet = objprocVisitLst3[0].ProcedureCode.ToString();
        //                                    PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = objprocVisitLst2[0].ProcedureCode;
        //                                    Proc_Item_lst.PQRI_Type = flag;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst3[0].ProcedureCode).ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst3[0].ProcedureCode).ToList<PQRI_Data>()[0];
        //                                    // Proc_Item_lst.PQRI_Value = objUtManager.GetSnomedForCPTCATIMeasure(objprocVisitLst2[0].ProcedureCode);
        //                                    string flag = Proc_Item_lst.PQRI_Type;
        //                                    Proc_Item_lst.PQRI_Type = "Visit";
        //                                    Encounter objProc_Enc = EncList.Where(a => a.Id == encId).ToList<Encounter>()[0];
        //                                    string sValueSet = objprocVisitLst3[1].ProcedureCode.ToString();
        //                                    PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Type = flag;
        //                                }

        //                            }

        //                        }
        //                    }
        //                    else if (objprocLists.Count > 1 && objprocVisitLst1.Count > 1 && objOtherLst1.Count > 0)
        //                    {

        //                        IList<PQRIResultDTO> objprocVisitLst2 = objprocLists.Where(a => a.ProcedureCode == "99215").ToList<PQRIResultDTO>();
        //                        IList<PQRIResultDTO> objprocVisitLst3 = objprocLists.Where(a => a.ProcedureCode != "99215").OrderBy(a => a.ProcedureCode).ToList<PQRIResultDTO>();
        //                        UtilityManager objUtManager = new UtilityManager();
        //                        if (objprocVisitLst2.Count > 0)
        //                        {
        //                            if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst2[0].ProcedureCode).ToList<PQRI_Data>().Count > 0)
        //                            {
        //                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst2[0].ProcedureCode).ToList<PQRI_Data>()[0];
        //                                Proc_Item_lst.PQRI_Value = objUtManager.GetSnomedForCPTCATIMeasure(objprocVisitLst2[0].ProcedureCode);
        //                                string flag = Proc_Item_lst.PQRI_Type;
        //                                Proc_Item_lst.PQRI_Type = "VisitCode";
        //                                Encounter objProc_Enc = EncList.Where(a => a.Id == encId).ToList<Encounter>()[0];
        //                                string sValueSet = objprocVisitLst3[0].ProcedureCode.ToString() + "|" + objOtherLst1[0].ProcedureCode; ;
        //                                PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                Proc_Item_lst.PQRI_Value = objprocVisitLst2[0].ProcedureCode;
        //                                Proc_Item_lst.PQRI_Type = flag;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst3[0].ProcedureCode).ToList<PQRI_Data>().Count > 0)
        //                            {

        //                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst3[0].ProcedureCode).ToList<PQRI_Data>()[0];
        //                                // Proc_Item_lst.PQRI_Value = objUtManager.GetSnomedForCPTCATIMeasure(objprocVisitLst2[0].ProcedureCode);
        //                                string flag = Proc_Item_lst.PQRI_Type;
        //                                Proc_Item_lst.PQRI_Type = "VisitCode";
        //                                Encounter objProc_Enc = EncList.Where(a => a.Id == encId).ToList<Encounter>()[0];
        //                                string sValueSet = objprocVisitLst3[1].ProcedureCode.ToString() + "|" + objOtherLst1[0].ProcedureCode;
        //                                PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                Proc_Item_lst.PQRI_Type = flag;
        //                            }

        //                        }

        //                    }

        //                    else
        //                    {
        //                        IList<PQRIResultDTO> objprocLsts = (from objproc in objprocLists where lst_Pqri_Data.Any(b => b.PQRI_Value.Trim() == objproc.ProcedureCode.Trim() && objproc.MeasureName == b.NQF_Number) select objproc).ToList<PQRIResultDTO>();

        //                        IList<PQRIResultDTO> objprocNullFlvr = objprocLsts.Where(a => a.ProcedureCode == "1150F").ToList<PQRIResultDTO>();
        //                        Encounter objProc_Enc = EncList.Where(a => a.Id == encId).ToList<Encounter>()[0];
        //                        if (objprocNullFlvr != null && objprocNullFlvr.Count > 0)
        //                        {
        //                            objprocLsts.Remove(objprocNullFlvr[0]);
        //                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocNullFlvr[0].ProcedureCode.Trim()).ToList<PQRI_Data>()[0];
        //                            Proc_Item_lst.PQRI_Value = "NullFlvr";
        //                            string sValueSet = string.Empty;
        //                            PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                            XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                        }
        //                        IList<PQRIResultDTO> objprocVisitLst = objprocLsts.Where(a => System.Text.RegularExpressions.Regex.IsMatch(a.ProcedureCode, @"^[0-9]+$") && Convert.ToInt64(a.ProcedureCode) >= 99201 && Convert.ToInt64(a.ProcedureCode) <= 99499).ToList<PQRIResultDTO>();
        //                        IList<PQRIResultDTO> objOtherLst = (from objproc in objprocLsts where !objprocVisitLst.Any(a => a.ProcedureCode.Equals(objproc.ProcedureCode)) && objproc.ProcedureCode != "1150F" select objproc).ToList<PQRIResultDTO>();
        //                        int ViSitCPTCnt = 0;
        //                        int OtherCPTCnt = 0;


        //                        if (objprocVisitLst != null && objprocVisitLst.Count > 0)
        //                        {
        //                            ViSitCPTCnt = objprocVisitLst.Count;
        //                        }
        //                        if (objOtherLst != null && objOtherLst.Count > 0)
        //                        {
        //                            OtherCPTCnt = objOtherLst.Count;
        //                        }
        //                        UtilityManager objUtManager = new UtilityManager();

        //                        while (ViSitCPTCnt > 0 || OtherCPTCnt > 0)
        //                        {
        //                            if (ViSitCPTCnt > 0 && OtherCPTCnt > 0)
        //                            {
        //                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode.Trim()).ToList<PQRI_Data>()[0];
        //                                objProc_Enc = EncList.Where(a => a.Id == objprocVisitLst[ViSitCPTCnt - 1].EncounterID).ToList<Encounter>()[0];
        //                                string sValueSet = string.Empty;

        //                                sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                                Proc_Item_lst.PQRI_Value = objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode + "|" + objOtherLst[OtherCPTCnt - 1].ProcedureCode + "&" + k;
        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, objprocVisitLst[ViSitCPTCnt - 1], objProc_Enc, sValueSet, HumanLst).ToString());
        //                                Proc_Item_lst.PQRI_Value = objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode;
        //                                ViSitCPTCnt--; OtherCPTCnt--; k++;


        //                            }
        //                            else if (ViSitCPTCnt > 0)
        //                            {
        //                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode.Trim()).ToList<PQRI_Data>()[0];
        //                                objProc_Enc = EncList.Where(a => a.Id == objprocVisitLst[ViSitCPTCnt - 1].EncounterID).ToList<Encounter>()[0];
        //                                string sValueSet = string.Empty;
        //                                sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;

        //                                if (objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode == "99202" || objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode == "99215" || objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode == "99205")
        //                                {
        //                                    Proc_Item_lst.PQRI_Value = objUtManager.GetSnomedForCPTCATIMeasure(objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode) + "&" + k;
        //                                    Proc_Item_lst.PQRI_Type = "SNOMEDCT";
        //                                }
        //                                else
        //                                {
        //                                    Proc_Item_lst.PQRI_Value = objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode + "&" + k;
        //                                }
        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, objprocVisitLst[ViSitCPTCnt - 1], objProc_Enc, sValueSet, HumanLst).ToString());
        //                                Proc_Item_lst.PQRI_Value = objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode;
        //                                if (objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode == "99202" || objprocVisitLst[ViSitCPTCnt - 1].ProcedureCode == "99215")
        //                                {
        //                                    Proc_Item_lst.PQRI_Type = "CPT";
        //                                }
        //                                ViSitCPTCnt--; k++;

        //                            }
        //                            else if (OtherCPTCnt > 0)
        //                            {
        //                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objOtherLst[OtherCPTCnt - 1].ProcedureCode.Trim()).ToList<PQRI_Data>()[0];
        //                                objProc_Enc = EncList.Where(a => a.Id == objOtherLst[OtherCPTCnt - 1].EncounterID).ToList<Encounter>()[0];
        //                                string sValueSet = string.Empty;

        //                                sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == objOtherLst[OtherCPTCnt - 1].ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                                Proc_Item_lst.PQRI_Value = objOtherLst[OtherCPTCnt - 1].ProcedureCode + "&" + k;
        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, objOtherLst[OtherCPTCnt - 1], objProc_Enc, sValueSet, HumanLst).ToString());
        //                                Proc_Item_lst.PQRI_Value = objOtherLst[OtherCPTCnt - 1].ProcedureCode;
        //                                OtherCPTCnt--; k++;

        //                            }
        //                        }
        //                        if (objProc_Enc.Snomed_Reason_Not_Performed_Med_Reviewed.Trim() != string.Empty)
        //                        {
        //                            PQRI_Data Proc_Item_lst = new PQRI_Data();
        //                            Proc_Item_lst.PQRI_Value = "NullFlvr";
        //                            Proc_Item_lst.PQRI_Type = "";
        //                            Proc_Item_lst.PQRI_Selection_XML = "Current_Medications_CAT_I_Stage3.xml";
        //                            string sValueSet = string.Empty;
        //                            PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                            XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                        }
        //                        if (objProc_Enc.Is_Medication_Reviewed.Trim() == "Y")
        //                        {
        //                            PQRI_Data Proc_Item_lst = new PQRI_Data();
        //                            Proc_Item_lst.PQRI_Value = "Medication";
        //                            Proc_Item_lst.PQRI_Type = "";
        //                            Proc_Item_lst.PQRI_Selection_XML = "Current_Medications_CAT_I_Stage3.xml";
        //                            string sValueSet = string.Empty;
        //                            PQRIResultDTO pqri_nullResult = new PQRIResultDTO();
        //                            XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, pqri_nullResult, objProc_Enc, sValueSet, HumanLst).ToString());
        //                        }

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (PQRIResultDTO item in objProclst)
        //                {
        //                    if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number) && !Procedure.Any(a => a[0].Trim() == item.ProcedureCode.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
        //                    {
        //                        bool bmammogram = false;

        //                        if (item.MeasureNo != "CMS127N")
        //                        {
        //                            //Procedure.Add(item.ProcedureCode.Trim());
        //                            Procedure.Add(new string[] { item.ProcedureCode.Trim(), item.EncounterID.ToString().Trim() });
        //                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
        //                            Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
        //                            string sValueSet = string.Empty;
        //                            if (item.ProcedureCode.Trim() == "90732")
        //                                sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "33" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                            else
        //                                sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;



        //                            if (MeasureName.Key.Replace(" ", "") == "CMS125v5" && (item.ProcedureCode.Trim().ToString() == "99215" || item.ProcedureCode.Trim().ToString() == "19220" || item.ProcedureCode.Trim().ToString() == "19200"))
        //                            {
        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    bmammogram = true; //For snomed code we dont need to repeat twice ...
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }
        //                            if (MeasureName.Key.Replace(" ", "") == "CMS69v5" && (item.ProcedureCode.Trim().ToString() == "99205" || item.ProcedureCode.Trim().ToString() == "99381"))
        //                            {
        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }
        //                            else if (MeasureName.Key.Replace(" ", "") == "CMS138v5" && (item.ProcedureCode.Trim().ToString() == "99215" || item.ProcedureCode.Trim().ToString() == "G9642" || item.ProcedureCode.Trim().ToString() == "99304" || item.ProcedureCode.Trim().ToString() == "99205" || item.ProcedureCode.Trim().ToString() == "99202" || item.ProcedureCode.Trim().ToString() == "R69" || item.ProcedureCode.Trim().ToString() == "1032F" || item.ProcedureCode.Trim().ToString() == "1034F" || item.ProcedureCode.Trim().ToString() == "99401" || item.ProcedureCode.Trim().ToString() == "1036F" || item.ProcedureCode.Trim().ToString() == "99381"))
        //                            {
        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }
        //                            else if (MeasureName.Key.Replace(" ", "") == "CMS147v6" && (item.ProcedureCode.Trim().ToString() == "99215" || item.ProcedureCode.Trim().ToString() == "99381") || item.ProcedureCode.Trim().ToString() == "99284" || item.ProcedureCode.Trim().ToString() == "90935" || item.ProcedureCode.Trim().ToString() == "99205")
        //                            {

        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = new PQRI_Data();
        //                                    Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }

        //                            else if (MeasureName.Key.Replace(" ", "") == "CMS165v5" && (item.ProcedureCode.Trim().ToString() == "90935" || item.ProcedureCode.Trim().ToString() == "99215" || item.ProcedureCode.Trim().ToString() == "S2065" || item.ProcedureCode.Trim().ToString() == "36800" || item.ProcedureCode.Trim().ToString() == "90945" || item.ProcedureCode.Trim().ToString() == "90989"))
        //                            {

        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = new PQRI_Data();
        //                                    Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }
        //                            else if (MeasureName.Key.Replace(" ", "") == "CMS122v5" && item.ProcedureCode.Trim().ToString() == "99202")
        //                            {

        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = new PQRI_Data();
        //                                    Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }

        //                            else if (MeasureName.Key.Replace(" ", "") == "CMS22v5" && (item.ProcedureCode.Trim().ToString() == "99284" || item.ProcedureCode.Trim().ToString() == "99215" || item.ProcedureCode.Trim().ToString() == "99204" || item.ProcedureCode.Trim().ToString() == "S9451"))
        //                            {

        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = new PQRI_Data();
        //                                    Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }
        //                            else if (MeasureName.Key.Replace(" ", "") == "CMS127v5" && item.ProcedureCode.Trim().ToString() == "99215")
        //                            {
        //                                if (lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>().Count > 0)
        //                                {
        //                                    iRandomNo++;
        //                                    Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0];
        //                                    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name && a.PQRI_Type == "SNOMED").ToList<PQRI_Data>()[0].Value_Set;
        //                                    PQRI_Data Proc_Item_lst_temp = new PQRI_Data();
        //                                    Proc_Item_lst_temp = Proc_Item_lst;
        //                                    Proc_Item_lst_temp.PQRI_Value = objutilitymngr.GetSnomedForCPTCATIMeasure(item.ProcedureCode.Trim().ToString());
        //                                    XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst_temp, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                                    Proc_Item_lst.PQRI_Value = item.ProcedureCode.Trim();
        //                                }
        //                            }
        //                            else
        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());

        //                            //Mammogram 
        //                            if (MeasureName.Key.Replace(" ", "") == "CMS125v5" && bmammogram == false && (item.ProcedureCode.Trim().ToString() == "19200" || item.ProcedureCode.Trim().ToString() == "19220" || item.ProcedureCode.Trim().ToString() == "19240"
        //                                || item.ProcedureCode.Trim().ToString() == "19303" || item.ProcedureCode.Trim().ToString() == "19304" || item.ProcedureCode.Trim().ToString() == "19305" || item.ProcedureCode.Trim().ToString() == "19306" || item.ProcedureCode.Trim().ToString() == "19307"))
        //                            {
        //                                iRandomNo++;

        //                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            file.WriteLine(item.ProcedureCode.Trim());

        //                        }
        //                    }
        //                }

        //            }

        //            if (HumanLst.Last_Name != "Ballard" && HumanLst.First_Name != "Anthony" && MeasureName.Key.ToString().Replace(" ", "") == "CMS127v5")
        //            {
        //                IList<PQRIResultDTO> objProclst127 = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //                foreach (PQRIResultDTO item in objProclst127)
        //                {
        //                    if (item.MeasureNo == "CMS127N" && !Procedure.Any(a => a[0].Trim() == item.ProcedureCode.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
        //                    {
        //                        Procedure.Add(new string[] { item.ProcedureCode.Trim(), item.HumanID.ToString().Trim() });
        //                        PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == item.MeasureName && a.PQRI_Type == "CPT").ToList<PQRI_Data>()[0];
        //                        string sValueSet = string.Empty;
        //                        Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
        //                        //if (item.ProcedureCode.Trim() == "90732")
        //                        //    sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == "33" && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                        //else
        //                        sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;

        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                    }
        //                }
        //            }
        //            if (MeasureName.Key.Replace(" ", "") == "CMS138v5")
        //            {
        //                IList<string> Pqri_Values = new List<string>();
        //                Encounter recordEnc = new Encounter();
        //                GetTobaccoRelatedDataforCATI(Item_Human_ID, out Pqri_Values, out recordEnc);
        //                if (Pqri_Values != null && Pqri_Values.Count > 0)
        //                {
        //                    foreach (string s in Pqri_Values)
        //                    {
        //                        iRandomNo++;
        //                        PQRI_Data Item_lst = new PQRI_Data();
        //                        Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == s.Trim() && MeasureName.Key.Replace(" ", "") == a.NQF_Number).ToList<PQRI_Data>()[0];
        //                        string sValueSet = string.Empty;
        //                        sValueSet = (Item_lst != null && Item_lst.Id != 0) ? Item_lst.Value_Set : string.Empty;

        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Item_lst, null, recordEnc, sValueSet, HumanLst).ToString());
        //                    }
        //                }
        //            }
        //            if (MeasureName.Key.Replace(" ", "") == "CMS69v5")
        //            {
        //                IList<string> Pqri_Values = new List<string>();
        //                Encounter recordEnc = new Encounter();
        //                GetBMIRelatedDataforCATI(Item_Human_ID, out Pqri_Values, out recordEnc);
        //                if (Pqri_Values != null && Pqri_Values.Count > 0)
        //                {
        //                    foreach (string s in Pqri_Values)
        //                    {
        //                        iRandomNo++;
        //                        PQRI_Data Item_lst = new PQRI_Data();
        //                        Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == s.Trim() && MeasureName.Key.Replace(" ", "") == a.NQF_Number).ToList<PQRI_Data>()[0];
        //                        string sValueSet = string.Empty;
        //                        sValueSet = (Item_lst != null && Item_lst.Id != 0) ? Item_lst.Value_Set : string.Empty;

        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Item_lst, null, recordEnc, sValueSet, HumanLst).ToString());
        //                    }
        //                }
        //            }


        //            //Recodes

        //            IList<string[]> Recodes = new List<string[]>();
        //            IList<PQRIResultDTO> objRecodeslst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.Recodes.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //            foreach (PQRIResultDTO item in objRecodeslst)
        //            {
        //                if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && item.MeasureName == a.NQF_Number) && !Recodes.Any(a => a[0].Trim() == item.Recodes.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
        //                {
        //                    //Procedure.Add(item.ProcedureCode.Trim());
        //                    Recodes.Add(new string[] { item.Recodes.Trim(), item.HumanID.ToString().Trim() });
        //                    PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
        //                    string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                    if (item.EncounterID == 0)
        //                    {
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, null, sValueSet, HumanLst).ToString());
        //                    }
        //                    else
        //                    {
        //                        Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    file.WriteLine(item.ProcedureCode.Trim());


        //                }
        //            }

        //            //NDC_ID

        //            IList<string[]> NDC_ID = new List<string[]>();
        //            IList<PQRIResultDTO> objNDC_IDlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.NDCID.Trim() != string.Empty).ToList<PQRIResultDTO>();
        //            foreach (PQRIResultDTO item in objNDC_IDlst)
        //            {

        //                if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.NDCID.Trim()) && !NDC_ID.Any(a => a[0].Trim() == item.NDCID.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
        //                {
        //                    //Procedure.Add(item.ProcedureCode.Trim());
        //                    NDC_ID.Add(new string[] { item.NDCID.Trim(), item.HumanID.ToString().Trim() });
        //                    PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.NDCID.Trim() && a.NQF_Number == item.MeasureName).ToList<PQRI_Data>()[0];
        //                    string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.NDCID.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
        //                    if (item.EncounterID == 0)
        //                    {
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, null, sValueSet, HumanLst).ToString());
        //                    }
        //                    else
        //                    {
        //                        Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
        //                        XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    file.WriteLine(item.ProcedureCode.Trim());
        //                }
        //            }
        //            if (MeasureName.Key.Replace(" ", "") == "CMS22v5")
        //            {
        //                IList<CarePlan> lstplan = new List<CarePlan>();
        //                CarePlanManager obj = new CarePlanManager();
        //                lstplan = obj.GetCarePlanByHuman(Item_Human_ID);

        //                for (int i = 0; i < lstplan.Count; i++)
        //                {
        //                    var temencounter = EncList.Where(a => a.Id == lstplan[0].Encounter_ID).ToList<Encounter>();
        //                    if (temencounter.Count > 0)
        //                    {
        //                        Encounter objProc_Enc = EncList.Where(a => a.Id == lstplan[0].Encounter_ID).ToList<Encounter>()[0];
        //                        IList<PQRI_Data> lstdata = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == lstplan[i].Snomed_Code && a.NQF_Number == Name && a.PQRI_Type == "CAREPLAN").ToList<PQRI_Data>();
        //                        if (lstdata.Count > 0)
        //                        {

        //                            XMLInsertsb.Append(SubXMLLoadCATIStageThree(lstdata[0], null, objProc_Enc, "", HumanLst).ToString());
        //                        }
        //                    }


        //                }


        //            }
        //            if (HumanLst.Last_Name == "Ballard" && HumanLst.First_Name == "Anthony" && MeasureName.Key.ToString().Replace(" ", "") == "CMS127v5")
        //            {
        //                PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(p => p.NQF_Number == "CMS127v5" && p.PQRI_Type == "EXCEPTION_SNOMED").ToList<PQRI_Data>()[0];
        //                XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, null, null, "", HumanLst).ToString());
        //            }
        //            //Past encounter
        //            if (HumanLst.Last_Name == "Castro" && HumanLst.First_Name == "Gordon" && MeasureName.Key.ToString().Replace(" ", "") == "CMS127v5")
        //            {
        //                PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(p => p.NQF_Number == "CMS127v5" && p.PQRI_Type == "DATA").ToList<PQRI_Data>()[0];
        //                XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, null, null, "", HumanLst).ToString());
        //            }
        //            if (HumanLst.Last_Name == "Craig" && HumanLst.First_Name == "Kristina" && MeasureName.Key.ToString().Replace(" ", "") == "CMS69v5")
        //            {
        //                PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(p => p.NQF_Number == "CMS69v5" && p.PQRI_Type == "DATA").ToList<PQRI_Data>()[0];
        //                XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, null, null, "", HumanLst).ToString());
        //            }
        //            if (HumanLst.Last_Name == "Barnes" && HumanLst.First_Name == "Amanda" && MeasureName.Key.ToString().Replace(" ", "") == "CMS147v6")
        //            {
        //                PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(p => p.NQF_Number == "CMS147v6" && p.PQRI_Type == "DATA").ToList<PQRI_Data>()[0];
        //                XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, null, null, "", HumanLst).ToString());
        //            }
        //            xmlReqNode = xmlDoc.GetElementsByTagName("text");
        //            xmlReqNode[0].InnerXml = XMLInsertsb.ToString();
        //            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<text>", "").Replace("</text>", "");
        //            xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName) + "\\" + HumanLst.Id + "_" + HumanLst.First_Name + "_" + HumanLst.Last_Name + ".xml");
        //            file.Close();
        //        }
        //    }
        //}
        public void LoadXMLCATIStageThreeMeasure(IList<PQRIResultDTO> PQRIResultDTO)
        {
            sClientTin = System.Configuration.ConfigurationSettings.AppSettings["ClientTIN"];
            sClientName = System.Configuration.ConfigurationSettings.AppSettings["ClientName"];
            IList<PQRIResultDTO> PQRIResultDTOList = PQRIResultDTO;
            EncounterManager objEncounterManager = new EncounterManager();
            var Measures = PQRIResultDTOList.Select(a => a.MeasureNo).Distinct();//.GroupBy(a => a.MeasureNo);
            IDictionary<string, string> DMeasureNamesLst = (IDictionary<string, string>)Session["MeasurePercentage"];
            PQRI_DataManager objPQRIManager = new PQRI_DataManager();
            IList<PQRI_Data> lst_Pqri_Data_FullList = objPQRIManager.GetPQRIListBySectionForStage3();
            IList<Encounter> EncLst = objEncounterManager.GetEncoutnerListByPhyID((int)ClientSession.PhysicianId);
            if (lstfacility == null || (lstfacility != null && lstfacility.Count == 0))//BugID:54511
            {
                lstfacility = objfacility.GetFacilityListByFacilityName(ClientSession.FacilityName);
            }
            foreach (var MeasureName in DMeasureNamesLst)

            {
                string Name = MeasureName.Key.Replace(" ", "");


                IList<ulong> lstHuman = PQRIResultDTOList.Where(a => a.MeasureName == MeasureName.Key.Replace(" ", "")).Select(a => a.HumanID).Distinct().ToList<ulong>();
                IList<PQRI_Data> lst_Pqri_Data = lst_Pqri_Data_FullList.Where(a => a.NQF_Number == Name).ToList<PQRI_Data>();
                foreach (ulong Item_Human_ID in lstHuman)
                {
                    flagmedication = 0;
                    iRandomNo++;
                    StringBuilder XMLInsertsb = new StringBuilder();
                    HumanManager objHumanManager = new HumanManager();


                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\eCQM_CAT_I_Header_PY2024.xml"));

                    string FolderName = string.Empty;

                    if (MeasureName.Key.Trim() == "CMS 69.1")
                        FolderName = "CMS 69";
                    else
                        FolderName = MeasureName.Key;

                    DirectoryInfo objdirect = new DirectoryInfo(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName));
                    if (!objdirect.Exists)
                        objdirect.Create();

                    Human HumanLst = objHumanManager.GetById(Item_Human_ID);

                    #region
                    XmlNodeList xmlReqNode = null;
                    xmlReqNode = xmlDoc.GetElementsByTagName("effectiveTime");
                    xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");

                    PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
                    IList<PatientInsuredPlan> PatientInsuredPlanlst = objPatientInsuredPlanManager.GetActiveInsurancePoliciesByHumanId(Item_Human_ID);
                    if (PatientInsuredPlanlst.Count > 0)
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
                        xmlReqNode[0].ChildNodes[1].Attributes[0].Value = PatientInsuredPlanlst[0].Policy_Holder_ID;
                    }
                    else
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("patientRole");
                        xmlReqNode[0].ChildNodes[1].Attributes[0].Value = "0";

                    }
                    xmlReqNode = xmlDoc.GetElementsByTagName("streetAddressLine");
                    xmlReqNode[0].InnerText = HumanLst.Street_Address1;
                    xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
                    //    xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
                    xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
                    xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyAddress2;
                    xmlReqNode[4].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
                    xmlReqNode[5].InnerText = ClientSession.PhysicainDetails[0].PhyAddress1;
                    xmlReqNode[6].InnerText = HumanLst.Street_Address1;

                    //StaticLookupManager objStaticLookupManager = new StaticLookupManager();

                    //IList<StaticLookup> lsdstLookUp = objStaticLookupManager.getStaticLookupByFieldName("preferred language");
                    //lsdstLookUp = (from m in lsdstLookUp where m.Value == HumanLst.Preferred_Language select m).ToList<StaticLookup>();
                    //if (lsdstLookUp != null && lsdstLookUp.Count > 0)
                    //{
                    //    xmlReqNode = xmlDoc.GetElementsByTagName("languageCode");
                    //    xmlReqNode[1].Attributes[0].Value = lsdstLookUp[0].Description;

                    //}

                    xmlReqNode = xmlDoc.GetElementsByTagName("city");
                    xmlReqNode[0].InnerText = HumanLst.City;
                    xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
                    xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
                    xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
                    xmlReqNode[4].InnerText = lstfacility[0].Fac_City;
                    xmlReqNode[5].InnerText = ClientSession.PhysicainDetails[0].PhyCity;
                    xmlReqNode[6].InnerText = HumanLst.City;



                    xmlReqNode = xmlDoc.GetElementsByTagName("state");
                    xmlReqNode[0].InnerText = HumanLst.State;
                    xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyState;
                    xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyState;
                    xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyState;
                    xmlReqNode[4].InnerText = lstfacility[0].Fac_State;
                    xmlReqNode[5].InnerText = ClientSession.PhysicainDetails[0].PhyState;
                    xmlReqNode[6].InnerText = HumanLst.State;


                    xmlReqNode = xmlDoc.GetElementsByTagName("postalCode");
                    xmlReqNode[0].InnerText = HumanLst.ZipCode;
                    xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
                    xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
                    xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
                    xmlReqNode[4].InnerText = lstfacility[0].Fac_Zip;
                    xmlReqNode[5].InnerText = ClientSession.PhysicainDetails[0].PhyZip;
                    xmlReqNode[6].InnerText = HumanLst.ZipCode;


                    xmlReqNode = xmlDoc.GetElementsByTagName("telecom");
                    if (HumanLst.Work_Phone_No.Trim() != "" && HumanLst.Work_Phone_No.Trim() != "0")
                    {
                        xmlReqNode[0].Attributes[1].Value = HumanLst.Work_Phone_No;
                        xmlReqNode[6].Attributes[1].Value = HumanLst.Work_Phone_No;
                    }
                    else
                    {
                        xmlReqNode[0].Attributes[1].Value = HumanLst.Home_Phone_No;
                        xmlReqNode[6].Attributes[1].Value = HumanLst.Home_Phone_No;
                    }
                    xmlReqNode[1].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyTelephone;
                    xmlReqNode[2].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyTelephone;
                    xmlReqNode[3].Attributes[1].Value = ClientSession.PhysicainDetails[0].PhyTelephone;
                    xmlReqNode[4].Attributes[1].Value = lstfacility[0].Fac_Telephone;
                    xmlReqNode[5].Attributes[1].Value = lstfacility[0].Fac_Telephone;

                    bool isDeleteGiven = false;

                    xmlReqNode = xmlDoc.GetElementsByTagName("given");
                    xmlReqNode[0].InnerText = HumanLst.First_Name;
                    if (HumanLst.MI.Trim() != "")
                    {
                        xmlReqNode[1].InnerText = HumanLst.MI;
                        xmlReqNode[6].InnerText = HumanLst.MI;
                    }
                    else
                    {
                        xmlReqNode[1].ParentNode.RemoveChild(xmlReqNode[1]);//If middle name not exists delete last given
                        xmlReqNode[5].ParentNode.RemoveChild(xmlReqNode[5]);
                        isDeleteGiven = true;
                    }
                    if (isDeleteGiven == false)
                    {
                        xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
                        xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
                        xmlReqNode[4].InnerText = ClientSession.PhysicainDetails[0].PhyLastName;
                        xmlReqNode[5].InnerText = HumanLst.First_Name;
                        //xmlReqNode[6].InnerText = HumanLst.Last_Name;
                    }
                    else
                    {
                        xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
                        xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
                        xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyLastName;
                        xmlReqNode[4].InnerText = HumanLst.First_Name;
                        //xmlReqNode[5].InnerText = HumanLst.Last_Name;
                    }

                    xmlReqNode = xmlDoc.GetElementsByTagName("family");
                    xmlReqNode[0].InnerText = HumanLst.Last_Name;
                    xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhyLastName;
                    xmlReqNode[2].InnerText = ClientSession.PhysicainDetails[0].PhyLastName;
                    xmlReqNode[3].InnerText = ClientSession.PhysicainDetails[0].PhyFirstName;
                    xmlReqNode[4].InnerText = HumanLst.Last_Name;

                    xmlReqNode = xmlDoc.GetElementsByTagName("suffix");
                    xmlReqNode[0].InnerText = ClientSession.PhysicainDetails[0].PhySuffix;
                    xmlReqNode[1].InnerText = ClientSession.PhysicainDetails[0].PhySuffix;


                    xmlReqNode = xmlDoc.GetElementsByTagName("representedCustodianOrganization");
                    IList<XmlElement> lst = xmlReqNode[0].OfType<XmlElement>().Where(Z => Z.Name == "name").ToList();
                    lst[0].InnerText = sClientName;

                    xmlReqNode = xmlDoc.GetElementsByTagName("representedOrganization");
                    lst = xmlReqNode[0].OfType<XmlElement>().Where(Z => Z.Name == "name").ToList();
                    lst[0].InnerText = sClientName;

                    //id extension filled //id root="2.16.840.1.113883.4.6"
                    xmlReqNode[1].ParentNode.ChildNodes[2].Attributes[1].Value = lstfacility[0].Fac_NPI;

                    // <id root="2.16.840.1.113883.4.2"
                    xmlReqNode[1].ChildNodes[1].Attributes[1].Value = sClientTin;

                    xmlReqNode = xmlDoc.GetElementsByTagName("administrativeGenderCode");
                    if (HumanLst.Sex.Trim() != "")
                        xmlReqNode[0].Attributes[0].Value = HumanLst.Sex.Substring(0, 1).ToUpper();//(HumanLst.Sex.ToUpper() == "MALE") ? "M" : "F";

                    xmlReqNode = xmlDoc.GetElementsByTagName("birthTime");
                    xmlReqNode[0].Attributes[0].Value = HumanLst.Birth_Date.ToString("yyyyMMdd");


                    if (HumanLst.Marital_Status.Trim() != "")
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
                        xmlReqNode[0].Attributes[0].Value = HumanLst.Marital_Status.Substring(0, 1).ToUpper();
                        xmlReqNode[0].Attributes[1].Value = HumanLst.Marital_Status;
                    }
                    else
                    {
                        xmlReqNode = xmlDoc.GetElementsByTagName("maritalStatusCode");
                        xmlReqNode[0].Attributes.RemoveAll();
                        XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
                        xAttribute.Value = "NI";
                        xmlReqNode[0].Attributes.Append(xAttribute);
                    }

                    xmlReqNode = xmlDoc.GetElementsByTagName("raceCode");
                    xmlReqNode[0].Attributes[0].Value = HumanLst.Race_No;
                    xmlReqNode[0].Attributes[1].Value = HumanLst.Race;

                    xmlReqNode = xmlDoc.GetElementsByTagName("ethnicGroupCode");
                    xmlReqNode[0].Attributes[0].Value = Convert.ToString(HumanLst.Ethnicity_No);
                    xmlReqNode[0].Attributes[1].Value = HumanLst.Ethnicity;

                    //xmlReqNode = xmlDoc.GetElementsByTagName("languageCode");
                    //xmlReqNode[1].Attributes[0].Value = HumanLst.Preferred_Language;

                    //Physician
                    xmlReqNode = xmlDoc.GetElementsByTagName("time");
                    xmlReqNode[0].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
                    xmlReqNode[1].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");
                    xmlReqNode[2].Attributes[0].Value = DateTime.Now.ToString("yyyyMMddhhmmss");

                    xmlReqNode = xmlDoc.GetElementsByTagName("id");
                    xmlReqNode[1].Attributes[0].Value = ClientSession.PhysicainDetails[0].PhyNPI;
                    xmlReqNode[3].Attributes[0].Value = ClientSession.PhysicainDetails[0].PhyNPI;
                    xmlReqNode[5].Attributes[0].Value = ClientSession.PhysicainDetails[0].PhyNPI;
                    xmlReqNode[6].Attributes[1].Value = lstfacility[0].Fac_NPI;
                    xmlReqNode[8].Attributes[1].Value = sClientTin;
                    xmlReqNode[11].Attributes[1].Value = System.Configuration.ConfigurationSettings.AppSettings["ClientNPI"];
                    xmlReqNode[12].Attributes[1].Value = sClientTin;
                    //  xmlReqNode[14].Attributes[1].Value = sClientTin;

                    xmlReqNode = xmlDoc.GetElementsByTagName("assignedAuthor");
                    lst = xmlReqNode[0].ChildNodes.OfType<XmlElement>().Where(Z => Z.Name == "code").ToList();
                    lst[0].Attributes[0].Value = ClientSession.PhysicainDetails[0].Taxonomy_Code;
                    lst[0].Attributes[2].Value = ClientSession.PhysicainDetails[0].Taxonomy_Description;

                    //legalAuthenticator
                    xmlReqNode = xmlDoc.GetElementsByTagName("legalAuthenticator");
                    lst = xmlReqNode[0].OfType<XmlElement>().Where(Z => Z.Name == "assignedEntity").ToList();
                    lst = lst[0].OfType<XmlElement>().Where(Z => Z.Name == "code").ToList();
                    lst[0].Attributes[0].Value = ClientSession.PhysicainDetails[0].Taxonomy_Code;
                    lst[0].Attributes[1].Value = ClientSession.PhysicainDetails[0].Taxonomy_Description;

                    //serviceEvent

                    string sLow = string.Empty;
                    string sHigh = string.Empty;
                    //  string  sLow = string.Empty;
                    if (EncLst != null && EncLst.Count > 0)
                    {
                        sLow = ConvertToLocal(EncLst.OrderBy(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
                        sHigh = ConvertToLocal(EncLst.OrderByDescending(a => a.Date_of_Service).ToList<Encounter>()[0].Date_of_Service).ToString("yyyyMMddhhmmss");
                        if (sLow.StartsWith("0001") == true)
                        {
                            sLow = sHigh;
                        }
                    }
                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    xmlReqNode[0].Attributes[0].Value = sLow;
                    xmlReqNode = xmlDoc.GetElementsByTagName("high");
                    xmlReqNode[0].Attributes[0].Value = sHigh;

                    //performer
                    xmlReqNode = xmlDoc.GetElementsByTagName("low");
                    xmlReqNode[1].Attributes[0].Value = sLow;
                    xmlReqNode = xmlDoc.GetElementsByTagName("high");
                    xmlReqNode[1].Attributes[0].Value = sHigh;

                    //serviceEvent
                    xmlReqNode = xmlDoc.GetElementsByTagName("performer");
                    lst = xmlReqNode[0].ChildNodes[1].OfType<XmlElement>().Where(Z => Z.Name == "code").ToList();
                    lst[0].Attributes[0].Value = ClientSession.PhysicainDetails[0].Taxonomy_Code;
                    lst[0].Attributes[1].Value = ClientSession.PhysicainDetails[0].Taxonomy_Description;


                    // PatientInsuredPlanManager objPatientInsuredPlanManager = new PatientInsuredPlanManager();
                    IList<PatientInsuredPlan> objPatientInsuredPlan = objPatientInsuredPlanManager.GetActiveInsurancePoliciesByHumanId(HumanLst.Id, "PRIMARY");

                    // IList<ulong> InsurancePlanID = objPatientInsuredPlan.Select(a => a.Insurance_Plan_ID).Distinct().ToList<ulong>();
                    InsurancePlanManager objins = new InsurancePlanManager();
                    if (objPatientInsuredPlan != null && objPatientInsuredPlan.Count > 0)
                    {
                        IList<InsurancePlan> lstplan = objins.GetInsurancebyID(objPatientInsuredPlan[0].Insurance_Plan_ID);
                        if (lstplan.Count > 0)
                        {
                            xmlReqNode = xmlDoc.GetElementsByTagName("value");

                            xmlReqNode[0].Attributes[1].Value = lstplan[0].External_Plan_Number;
                            xmlReqNode[0].Attributes[4].Value = lstplan[0].Ins_Plan_Name;
                        }
                    }

                    // CHOSEN IN MEAsURE CALCULATOR
                    //xmlReqNode = xmlDoc.GetElementsByTagName("item");
                    //xmlReqNode[0].InnerText = "Reporting Parameter " + dtpFromDate.SelectedDate.Value.ToString("yyyyMMddhhmmss") + " To " + dtpToDate.SelectedDate.Value.ToString("yyyyMMddhhmmss");
                    #endregion

                    //1.Encounter Xml File Indert
                    IList<Encounter> EncList = new List<Encounter>();
                    //IList<ulong> Items_Enc = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.EncounterID != 0).Select(a => a.EncounterID).ToList<ulong>(); //(from e in PQRIResultDTOList where e.HumanID == Item_Human_ID select  new {e.ICD}).ToList();//.ToList<string>();
                    IList<string> Items_Enc = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.EncounterID != 0).Select(a => a.EncounterID.ToString()).ToList<string>(); //(from e in PQRIResultDTOList where e.HumanID == Item_Human_ID select  new {e.ICD}).ToList();//.ToList<string>();
                    if (Items_Enc != null && Items_Enc.Count > 0)
                    {
                        //EncList = objEncounterManager.GetEncounterList(Items_Enc);
                        EncList = objEncounterManager.GetEncounterListArchive(Items_Enc);
                    }

                    StreamWriter file = new StreamWriter(HttpContext.Current.Server.MapPath("SampleXML" + "\\Test.txt"));



                    IList<PQRIResultDTO> objICDlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.ICD.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    IList<string[]> ICD = new List<string[]>();
                    //Dictionary<string, string> DICD = new Dictionary<string, string>();

                    Session["PrimaryICD"] = "";

                    foreach (PQRIResultDTO item in objICDlst)
                    {
                        Session["PrimaryICD"] = "";

                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.ICD.Trim() && item.MeasureName == a.NQF_Number) && !ICD.Any(a => a[0].Trim() == item.ICD.Trim()))// && a[1].Trim() == item.EncounterID.ToString().Trim()))
                        {
                            ICD.Add(new string[] { item.ICD.Trim(), item.EncounterID.ToString().Trim() });
                            PQRI_Data ICD_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
                            Encounter objICD_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ICD.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;

                            XMLInsertsb.Append(SubXMLLoadCATIStageThree(ICD_Item_lst, item, objICD_Enc, sValueSet, HumanLst).ToString());


                        }
                        else
                        {
                            file.WriteLine(item.ICD.Trim());
                        }

                    }

                    //IList<string> Loinc = new List<string>();
                    IList<string[]> Loinc = new List<string[]>();
                    IList<PQRIResultDTO> objLoinclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.LoincIdentifier.Trim() != string.Empty).ToList<PQRIResultDTO>();

                    IList<PQRIResultDTO> objsnomedlsttemp = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.Value.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    string sValueSetLoinc = string.Empty;
                    if (objLoinclst.Count > 0 && objsnomedlsttemp.Count > 0)
                    {
                        Session["PrimaryICD"] = "";
                        foreach (PQRIResultDTO item in objLoinclst)
                        {
                            if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim()) &&
                                !Loinc.Any(a => a[0].Trim() == item.LoincIdentifier.Trim() &&
                                a[1].Trim() == item.EncounterID.ToString().Trim()))
                            {
                                Loinc.Add(new string[] { item.LoincIdentifier.Trim(), item.EncounterID.ToString().Trim() });
                               
                                PQRI_Data Loinc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == item.MeasureName).ToList<PQRI_Data>()[0];
                               
                                Encounter objLoinc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0]; 
                                
                                objsnomedlsttemp = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.EncounterID==item.EncounterID
                                && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.Value.Trim() != string.Empty ).ToList<PQRIResultDTO>();

                               IList<PQRIResultDTO> ilistPQRIsnomed = (objsnomedlsttemp.GroupBy(x => x.Value).Select(x => x.FirstOrDefault())).ToList<PQRIResultDTO>();

                                // PQRI_Data snomed_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == item.MeasureName).ToList<PQRI_Data>()[0];



                                sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>().Count > 0 ? lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set : string.Empty;
                                XMLInsertsb.Append(SubXMLLoadCATIStageThreeLoinc(Loinc_Item_lst, item, objLoinc_Enc, sValueSetLoinc, HumanLst, ilistPQRIsnomed).ToString());


                            }
                        }

                    }
                    else
                    {


                        foreach (PQRIResultDTO item in objLoinclst)
                        {
                            if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim()) && !Loinc.Any(a => a[0].Trim() == item.LoincIdentifier.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
                            {
                                //Loinc.Add(item.LoincIdentifier.Trim());
                                //try
                                //{
                                Loinc.Add(new string[] { item.LoincIdentifier.Trim(), item.EncounterID.ToString().Trim() });
                                PQRI_Data Loinc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == item.MeasureName).ToList<PQRI_Data>()[0];
                                Encounter objLoinc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                                Session["PrimaryICD"] = "";
                                sValueSetLoinc = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>().Count > 0 ? lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.LoincIdentifier.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set : string.Empty;
                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Loinc_Item_lst, item, objLoinc_Enc, sValueSetLoinc, HumanLst).ToString());
                                //}
                                //catch(Exception ex)
                                //{

                                //}
                            }
                            else
                            {
                                file.WriteLine(item.LoincIdentifier.Trim());

                            }

                        }
                    }


                    IList<string[]> Procedure = new List<string[]>();


                    IList<PQRIResultDTO> objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    //if (MeasureName.Key == "CMS127N")
                    //    objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();


                    foreach (PQRIResultDTO item in objProclst)
                    {
                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number) && !Procedure.Any(a => a[0].Trim() == item.ProcedureCode.Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
                        {
                            //Procedure.Add(item.ProcedureCode.Trim());
                            Procedure.Add(new string[] { item.ProcedureCode.Trim(), item.EncounterID.ToString().Trim() });
                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.ProcedureCode.Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
                            Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                            string sValueSet = string.Empty;
                            AssessmentManager objas = new AssessmentManager();
                            IList<Assessment> lstass = new List<Assessment>();
                            lstass = objas.GetAssessmentUsingEncounterID(Convert.ToUInt64(item.EncounterID));
                           if(lstass.Count>0)
                            {
                                lstass = (from a in lstass where a.Primary_Diagnosis == "Y" select a).ToList<Assessment>();
                               Session["PrimaryICD"] = lstass[0].ICD;
                            }
                            else 
                            {
                                Session["PrimaryICD"] = "Nullflavor";
                            }

                            XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());


                        }
                    }

                    //snomedcode

                    IList<string[]> ilistsnomed = new List<string[]>();


                    IList<PQRIResultDTO> objsnomedlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.EncounterID != 0 && a.Value.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    //if (MeasureName.Key == "CMS127N")
                    //    objProclst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.ProcedureCode.Trim() != string.Empty).ToList<PQRIResultDTO>();


                    foreach (PQRIResultDTO item in objsnomedlst)
                    {

                        string[] sarray = item.Value.Trim().Split(',');

                        for (int icount = 0; icount < sarray.Length; icount++)
                        {

                            if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == sarray[icount].Trim() && item.MeasureName == a.NQF_Number) && !ilistsnomed.Any(a => a[0].Trim() == sarray[icount].Trim() && a[1].Trim() == item.EncounterID.ToString().Trim()))
                            {
                                //Procedure.Add(item.ProcedureCode.Trim());
                                ilistsnomed.Add(new string[] { sarray[icount].Trim(), item.EncounterID.ToString().Trim() });
                                PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == sarray[icount].Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
                                Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                                string sValueSet = string.Empty;

                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());


                            }
                        }
                    }



                    //Recodes

                    IList<string[]> Recodes = new List<string[]>();
                    IList<PQRIResultDTO> objRecodeslst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.Recodes.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    foreach (PQRIResultDTO item in objRecodeslst)
                    {
                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && item.MeasureName == a.NQF_Number) && !Recodes.Any(a => a[0].Trim() == item.Recodes.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
                        {
                            //Procedure.Add(item.ProcedureCode.Trim());
                            Recodes.Add(new string[] { item.Recodes.Trim(), item.HumanID.ToString().Trim() });
                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && item.MeasureName == a.NQF_Number).ToList<PQRI_Data>()[0];
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.Recodes.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            if (item.EncounterID == 0)
                            {
                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, null, sValueSet, HumanLst).ToString());
                            }
                            else
                            {
                                Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());
                            }
                        }
                        else
                        {
                            file.WriteLine(item.ProcedureCode.Trim());


                        }
                    }

                    //NDC_ID

                    IList<string[]> NDC_ID = new List<string[]>();
                    IList<PQRIResultDTO> objNDC_IDlst = PQRIResultDTOList.Where(a => a.HumanID == Item_Human_ID && a.MeasureName == MeasureName.Key.Replace(" ", "") && a.NDCID.Trim() != string.Empty).ToList<PQRIResultDTO>();
                    foreach (PQRIResultDTO item in objNDC_IDlst)
                    {

                        if (lst_Pqri_Data.Any(a => a.PQRI_Value.Trim() == item.NDCID.Trim()) && !NDC_ID.Any(a => a[0].Trim() == item.NDCID.Trim() && a[1].Trim() == item.HumanID.ToString().Trim()))
                        {
                            //Procedure.Add(item.ProcedureCode.Trim());
                            NDC_ID.Add(new string[] { item.NDCID.Trim(), item.HumanID.ToString().Trim() });
                            PQRI_Data Proc_Item_lst = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.NDCID.Trim() && a.NQF_Number == item.MeasureName).ToList<PQRI_Data>()[0];
                            string sValueSet = lst_Pqri_Data.Where(a => a.PQRI_Value.Trim() == item.NDCID.Trim() && a.NQF_Number == Name).ToList<PQRI_Data>()[0].Value_Set;
                            if (item.EncounterID == 0)
                            {
                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, null, sValueSet, HumanLst).ToString());
                            }
                            else
                            {
                                Encounter objProc_Enc = EncList.Where(a => a.Id == item.EncounterID).ToList<Encounter>()[0];
                                XMLInsertsb.Append(SubXMLLoadCATIStageThree(Proc_Item_lst, item, objProc_Enc, sValueSet, HumanLst).ToString());
                            }
                        }
                        else
                        {
                            file.WriteLine(item.ProcedureCode.Trim());
                        }
                    }


                    xmlReqNode = xmlDoc.GetElementsByTagName("test");
                    xmlReqNode[0].InnerXml = XMLInsertsb.ToString();
                    xmlDoc.InnerXml = xmlDoc.InnerXml.Replace("<test>", "").Replace("</test>", "");
                    xmlDoc.Save(Server.MapPath("Documents/" + Session.SessionID + "/CQMI/" + FolderName) + "\\" + HumanLst.Id + "_" + HumanLst.First_Name + "_" + HumanLst.Last_Name + ".xml");
                    file.Close();
                }
            }
        }
        public void GetTobaccoRelatedDataforCATI(ulong human_id, out IList<string> Pqri_values, out Encounter encRecord)
        {
            Pqri_values = new List<string>();
            Rcopia_MedicationManager rmedManager = new Rcopia_MedicationManager();
            IList<Rcopia_Medication> rcopiaMed = new List<Rcopia_Medication>();
            IList<string> Rxnorm_MedValues = new List<string>() { "993557", "993541" };
            rcopiaMed = rmedManager.GetRCopiaMedByHumanID(human_id);
            if (rcopiaMed != null && rcopiaMed.Count > 0)
            {
                IList<Rcopia_Medication> rcopMed = new List<Rcopia_Medication>();
                rcopMed = rcopiaMed.Where(a => Rxnorm_MedValues.Contains(a.Rxnorm_ID.ToString())).ToList<Rcopia_Medication>();
                if (rcopMed != null && rcopMed.Count > 0)
                    Pqri_values.Add(rcopMed[0].Rxnorm_ID.ToString());
            }
            IList<string> RecodesLst = new List<string>() { "105539002", "160603005", "266922007", "228518000" };
            IList<string> SnomedLst = new List<string>() { "170969009" };
            SocialHistoryManager sochisManager = new SocialHistoryManager();
            IList<SocialHistory> socHis = new List<SocialHistory>();
            IList<SocialHistory> socHisRecodeList = new List<SocialHistory>();
            IList<SocialHistory> socHisSnomedList = new List<SocialHistory>();
            socHis = sochisManager.GetSocHisByHumanID(human_id);
            if (socHis != null && socHis.Count > 0)
            {
                socHisRecodeList = socHis.Where(a => RecodesLst.Contains(a.Recodes)).ToList<SocialHistory>();
                if (socHisRecodeList != null && socHisRecodeList.Count > 0)
                {
                    foreach (SocialHistory objSoc in socHisRecodeList)
                    {
                        if (Pqri_values.IndexOf(objSoc.Recodes) == -1)
                            Pqri_values.Add(objSoc.Recodes);
                    }
                }
                socHisSnomedList = socHis.Where(a => SnomedLst.Contains(a.Snomed_Reason_Not_Performed)).ToList<SocialHistory>();
                if (socHisSnomedList != null && socHisSnomedList.Count > 0)
                {
                    foreach (SocialHistory objSoc in socHisSnomedList)
                    {
                        if (Pqri_values.IndexOf(objSoc.Snomed_Reason_Not_Performed) == -1)
                            Pqri_values.Add(objSoc.Snomed_Reason_Not_Performed);
                    }
                }
            }
            CarePlanManager cpManager = new CarePlanManager();
            IList<CarePlan> careplan = new List<CarePlan>();
            IList<string> CarePlanSnomedLst = new List<string>() { "170969009", "225323000" };
            careplan = cpManager.GetCarePlanByHuman(human_id);
            if (careplan != null && careplan.Count > 0)
            {
                IList<CarePlan> careplanTobacco = new List<CarePlan>();
                careplanTobacco = careplan.Where(a => a.Care_Name == "Screening Tests"
                    && a.Care_Name_Value == "Preventive Care and Screening: Tobacco Use: Screening and Cessation Intervention"
                    && CarePlanSnomedLst.Contains(a.Snomed_Code)).ToList<CarePlan>();
                if (careplanTobacco != null && careplanTobacco.Count > 0)
                    if (Pqri_values.IndexOf(careplanTobacco[0].Snomed_Code) == -1)
                        Pqri_values.Add(careplanTobacco[0].Snomed_Code);
            }
            EncounterManager encManager = new EncounterManager();
            IList<Encounter> lstEnc = new List<Encounter>();
            encRecord = new Encounter();
            lstEnc = encManager.GetEncounterUsingHumanID(human_id);
            if (lstEnc != null && lstEnc.Count > 0)
                encRecord = lstEnc[0];
            HumanManager humanManager = new HumanManager();
            Human human = new Human();
            human = humanManager.GetById(human_id);
            if (human != null && human.Id != 0 && human.Patient_Status.ToUpper() == "DECEASED")
                Pqri_values.Add("419099009");

        }
        public void GetBMIRelatedDataforCATI(ulong human_id, out IList<string> Pqri_values, out Encounter encRecord)
        {
            Pqri_values = new List<string>();
            IList<string> Rxnorm_MedValues = new List<string>() { "860221", "692876" };
            Rcopia_MedicationManager rmedManager = new Rcopia_MedicationManager();
            IList<Rcopia_Medication> rcopiaMed = new List<Rcopia_Medication>();
            rcopiaMed = rmedManager.GetRCopiaMedByHumanID(human_id);
            if (rcopiaMed != null && rcopiaMed.Count > 0)
            {
                IList<Rcopia_Medication> rcopMed = new List<Rcopia_Medication>();
                rcopMed = rcopiaMed.Where(a => Rxnorm_MedValues.Contains(a.Rxnorm_ID.ToString())).ToList<Rcopia_Medication>();
                if (rcopMed != null && rcopMed.Count > 0)
                {
                    if (Pqri_values.IndexOf(rcopMed[0].Rxnorm_ID.ToString()) == -1)
                        Pqri_values.Add(rcopMed[0].Rxnorm_ID.ToString());
                }
            }
            CarePlanManager cpManager = new CarePlanManager();
            IList<CarePlan> careplan = new List<CarePlan>();
            IList<string> CarePlanSnomedLst = new List<string>() { "103699006", "308470006" };
            careplan = cpManager.GetCarePlanByHuman(human_id);
            if (careplan != null && careplan.Count > 0)
            {
                IList<CarePlan> careplanTobacco = new List<CarePlan>();
                careplanTobacco = careplan.Where(a => a.Care_Name == "patient_details"
                    && a.Care_Name_Value == "BP Sys/Dia"
                    && CarePlanSnomedLst.Contains(a.Snomed_Code)).ToList<CarePlan>();
                if (careplanTobacco != null && careplanTobacco.Count > 0)
                    if (Pqri_values.IndexOf(careplanTobacco[0].Snomed_Code) == -1)
                        Pqri_values.Add(careplanTobacco[0].Snomed_Code);
            }
            TreatmentPlanManager TpManager = new TreatmentPlanManager();
            IList<TreatmentPlan> Tplan = new List<TreatmentPlan>();
            IList<string> TPlanSnomedLst = new List<string>() { "304549008", "410177006", "370847001" };
            Tplan = TpManager.GetTreatmentPlanUsingWithOutEncounterId(human_id);
            if (Tplan != null && Tplan.Count > 0)
            {
                IList<TreatmentPlan> TreatmntPlan = new List<TreatmentPlan>();
                TreatmntPlan = Tplan.Where(a => TPlanSnomedLst.Contains(a.Followup_Plan_Snomed)).ToList<TreatmentPlan>();
                if (TreatmntPlan != null && TreatmntPlan.Count > 0)
                    if (Pqri_values.IndexOf(TreatmntPlan[0].Followup_Plan_Snomed) == -1)
                        Pqri_values.Add(TreatmntPlan[0].Followup_Plan_Snomed);
            }
            EncounterManager encManager = new EncounterManager();
            IList<Encounter> lstEnc = new List<Encounter>();
            encRecord = new Encounter();
            lstEnc = encManager.GetEncounterUsingHumanID(human_id);
            if (lstEnc != null && lstEnc.Count > 0)
                encRecord = lstEnc[0];
        }
        //public StringBuilder SubXMLLoadCATIStageThree(PQRI_Data PQRI_Data_value, PQRIResultDTO PQRIResultDTO_Value, Encounter EncLst, string sValueSet, Human Humanlst)
        //{

        //    iRandomNo++;
        //    StringBuilder SubXMLsb = new StringBuilder();
        //    StringBuilder objEncMainsb;
        //    StringBuilder AddEncountersb;
        //    StringBuilder Loopsb;
        //    var xDox = new XDocument();

        //    switch (PQRI_Data_value.PQRI_Selection_XML.Trim())
        //    {

        //        case "Encounter_Section_CAT_I_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_Section_CAT_I_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 35, iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 35, iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            if (PQRI_Data_value.NQF_Number == "CMS69v5")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddSeconds(30).ToString("yyyyMMddhhmmss"));
        //            }
        //            else if (PQRI_Data_value.NQF_Number == "CMS165v5")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddHours(1).ToString("yyyyMMddhhmmss"));
        //            }
        //            else
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            }

        //            if (PQRI_Data_value.NQF_Number == "CMS125v5" || PQRI_Data_value.NQF_Number == "CMS22v5")
        //            {
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "390906007")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "108224003")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "185349003")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");


        //            }

        //            if (PQRI_Data_value.PQRI_Value.Trim() == "D7140" && PQRI_Data_value.NQF_Number == "CMS22v5")
        //            {
        //                Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.13");
        //            }

        //            if ((PQRI_Data_value.PQRI_Value.Trim() == "G0101" || PQRI_Data_value.PQRI_Value.Trim() == "G0402"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "G0438" || PQRI_Data_value.PQRI_Value.Trim() == "G0439"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "S9452" || PQRI_Data_value.PQRI_Value.Trim() == "S9470"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "G0402" || PQRI_Data_value.PQRI_Value.Trim() == "S9449"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "S9451") && PQRI_Data_value.NQF_Number == "CMS22v5")
        //            {
        //                Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");
        //            }

        //            if ((PQRI_Data_value.PQRI_Value.Trim() == "G0101" || PQRI_Data_value.PQRI_Value.Trim() == "G0108"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "G0270" || PQRI_Data_value.PQRI_Value.Trim() == "G0271"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "G0402" || PQRI_Data_value.PQRI_Value.Trim() == "G0438"
        //               || PQRI_Data_value.PQRI_Value.Trim() == "G0439" || PQRI_Data_value.PQRI_Value.Trim() == "G0447"
        //              ) && PQRI_Data_value.NQF_Number == "CMS69v5")
        //            {
        //                Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");
        //            }


        //            if (PQRI_Data_value.NQF_Number == "CMS138v5")
        //            {
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "390906007" || PQRI_Data_value.PQRI_Value.Trim() == "185349003")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "G0439" || PQRI_Data_value.PQRI_Value.Trim() == "G0438")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");
        //                else if (PQRI_Data_value.PQRI_Value.Trim() == "18170008" || PQRI_Data_value.PQRI_Value.Trim() == "19681004" || PQRI_Data_value.PQRI_Value.Trim() == "308335008")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");

        //            }
        //            if (PQRI_Data_value.NQF_Number == "CMS69v5")
        //            {
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "308335008" || PQRI_Data_value.PQRI_Value.Trim() == "185349003")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //            }
        //            if (PQRI_Data_value.NQF_Number == "CMS127v5")
        //            {
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "390906007")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //            }
        //            if (PQRI_Data_value.NQF_Number == "CMS147v6")
        //            {
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "390906007" || PQRI_Data_value.PQRI_Value.Trim() == "185349003" || PQRI_Data_value.PQRI_Value.Trim() == "420113004" || PQRI_Data_value.PQRI_Value.Trim() == "315640000")//|| PQRI_Data_value.PQRI_Value.Trim() == "86198006" 
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "G0439" || PQRI_Data_value.PQRI_Value.Trim() == "G0438")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");

        //            }
        //            if (PQRI_Data_value.NQF_Number == "CMS122v5")
        //            {
        //                if (PQRI_Data_value.PQRI_Value.Trim() == "G0438")
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");

        //            }

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            if (PQRI_Data_value.NQF_Number == "CMS22v5" && flagmedication == 0)
        //            {
        //                flagmedication = 1;

        //                IList<Rcopia_Medication> lstrcopia = new List<Rcopia_Medication>();
        //                Rcopia_MedicationManager obj = new Rcopia_MedicationManager();

        //                lstrcopia = obj.GetRxNormByHumanID(PQRIResultDTO_Value.HumanID);

        //                for (int i = 0; i < lstrcopia.Count; i++)
        //                {
        //                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS22_Medication_cat1_Stage3.xml"));
        //                    objEncMainsb = new StringBuilder(xDox.ToString());
        //                    objEncMainsb.Remove(0, 157);
        //                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                    AddEncountersb = new StringBuilder();

        //                    Loopsb = new StringBuilder();
        //                    Loopsb.Append(objEncMainsb.ToString());
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("5a394c12636239045231877c") + 24, iRandomNo);

        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, lstrcopia[i].Rxnorm_ID);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("time value") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                    AddEncountersb.AppendFormat(Loopsb.ToString());
        //                    SubXMLsb.Append(AddEncountersb.ToString());
        //                }
        //            }
        //            if ((PQRI_Data_value.PQRI_Value.Trim() == "99202" || PQRI_Data_value.PQRI_Value.Trim() == "99381") && PQRI_Data_value.NQF_Number != "CMS22v5")
        //            {

        //                StringBuilder AddEncountersb1 = new StringBuilder();

        //                StringBuilder Loopsb1 = new StringBuilder();
        //                Loopsb1.Append(objEncMainsb.ToString());

        //                Loopsb1.Insert(Loopsb1.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, "185349003");

        //                Loopsb1.Insert(Loopsb1.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb1.Insert(Loopsb1.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                AddEncountersb1.AppendFormat(Loopsb1.ToString());
        //                SubXMLsb.Append(AddEncountersb1.ToString());
        //                AddEncountersb.AppendFormat(Loopsb.ToString());
        //                SubXMLsb.Append(AddEncountersb.ToString());
        //            }
        //            //Influenza
        //            if (PQRIResultDTO_Value.MeasureName.ToUpper().Replace(" ", "") == "CMS147V6" && (PQRIResultDTO_Value.ProcedureCode == "G0438" || PQRIResultDTO_Value.ProcedureCode == "99212" || PQRIResultDTO_Value.ProcedureCode == "99213" || PQRIResultDTO_Value.ProcedureCode == "99203" || PQRIResultDTO_Value.ProcedureCode == "99215" || PQRIResultDTO_Value.ProcedureCode == "99341" || PQRIResultDTO_Value.ProcedureCode == "99381" || PQRIResultDTO_Value.ProcedureCode == "99393" || PQRIResultDTO_Value.ProcedureCode == "99402" || PQRIResultDTO_Value.ProcedureCode == "99242" || PQRIResultDTO_Value.ProcedureCode == "99412" || PQRIResultDTO_Value.ProcedureCode == "99384" || PQRIResultDTO_Value.ProcedureCode == "99394" || PQRIResultDTO_Value.ProcedureCode == "99411" || PQRIResultDTO_Value.ProcedureCode == "99347"))
        //            {
        //                EAndMCodingManager em = new EAndMCodingManager();
        //                IList<EAndMCoding> lst = new List<EAndMCoding>();
        //                lst = em.GetDetailsByEncounterID(EncLst.Id);
        //                IList<EAndMCoding> lst1 = new List<EAndMCoding>();

        //                lst1 = (from m in lst where m.Procedure_Code == PQRIResultDTO_Value.ProcedureCode select m).ToList<EAndMCoding>();
        //                if (lst1 != null && lst1.Count == 2)
        //                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_Section_CAT_I_Stage3.xml"));
        //                objEncMainsb = new StringBuilder(xDox.ToString());
        //                objEncMainsb.Remove(0, 157);
        //                objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                AddEncountersb = new StringBuilder();

        //                Loopsb = new StringBuilder();
        //                Loopsb.Append(objEncMainsb.ToString());
        //                if (PQRIResultDTO_Value.ProcedureCode == "99215")
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.464.1003.101.12.1048");
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                    if (PQRI_Data_value.PQRI_Type == "SNOMED")
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, "390906007");
        //                    else
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);

        //                }
        //                else if (PQRIResultDTO_Value.ProcedureCode == "99381")
        //                {
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.526.3.1012");
        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                }
        //                else
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.526.3.1252");
        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                }
        //                if (PQRIResultDTO_Value.ProcedureCode == "G0438")
        //                {
        //                    Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");
        //                }
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 35, iRandomNo);
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 35, iRandomNo);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                AddEncountersb.AppendFormat(Loopsb.ToString());
        //                SubXMLsb.Append(AddEncountersb.ToString());

        //                if (PQRIResultDTO_Value.ProcedureCode == "99381" || PQRIResultDTO_Value.ProcedureCode == "99215")
        //                {
        //                    if (lst1 != null && lst1.Count == 3)
        //                        xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_Section_CAT_I_Stage3.xml"));
        //                    objEncMainsb = new StringBuilder(xDox.ToString());
        //                    objEncMainsb.Remove(0, 157);
        //                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                    AddEncountersb = new StringBuilder();
        //                    Loopsb = new StringBuilder();
        //                    Loopsb.Append(objEncMainsb.ToString());
        //                    if (PQRIResultDTO_Value.ProcedureCode == "99215")
        //                    {
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.526.3.1252");
        //                        Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                        if (PQRI_Data_value.PQRI_Type == "SNOMED")
        //                            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, "390906007");
        //                        else
        //                            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);

        //                    }
        //                    else
        //                    {
        //                        Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.96");
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.464.1003.101.12.1048");
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                    }
        //                    if (PQRIResultDTO_Value.ProcedureCode == "G0438")
        //                    {
        //                        Loopsb.Replace("2.16.840.1.113883.6.12", "2.16.840.1.113883.6.285");
        //                    }
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 35, iRandomNo);
        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 35, iRandomNo);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                    AddEncountersb.AppendFormat(Loopsb.ToString());
        //                    SubXMLsb.Append(AddEncountersb.ToString());
        //                }
        //            }
        //            break;
        //        case "Surgery.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Surgery.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Hemodialysis.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Hemodialysis.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Hemodialysis2.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Hemodialysis2.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "PreviousRec.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\PreviousRec.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Hba1cSnomed.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Hba1cSnomed.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "RiskCategory.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\RiskCategory.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            //Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Influenza_encounter.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Influenza_encounter.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            //Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            //Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Communication147.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Communication147.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Encounter147.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter147.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Encounter_69.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_69.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "HP_Exception_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_Exception_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "HP_Exception_Snomed_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_Exception_Snomed_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "HP_SnomedCode_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_SnomedCode_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "HP_SnomedCodeVisit_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_SnomedCodeVisit_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, +iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("time value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "HP_FollowpSnomed_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_FollowpSnomed_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, PQRI_Data_value.Value_Set);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, +iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("time value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "OBS_High_BP_Followup_Snomed_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_High_BP_Followup_Snomed_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        case "DiabetesHba1c.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\DiabetesHba1c.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;


        //        case "OBS_HB1AC_LOINC_STAGE3.xml":

        //            if (PQRIResultDTO_Value.LoincIdentifier != "")
        //            {
        //                VitalsManager objVitalsManager = new VitalsManager();
        //                PatientResults objPatientResults = objVitalsManager.GetResultByLoincObservationAndEncounterId(PQRIResultDTO_Value.LoincIdentifier, PQRIResultDTO_Value.HumanID, PQRIResultDTO_Value.EncounterID);//we want to change
        //                string value = "";
        //                if (objPatientResults != null)
        //                {
        //                    value = objPatientResults.Value.ToString();
        //                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_HB1AC_LOINC_STAGE3.xml"));
        //                    objEncMainsb = new StringBuilder(xDox.ToString());
        //                    objEncMainsb.Remove(0, 157);
        //                    objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                    AddEncountersb = new StringBuilder();

        //                    Loopsb = new StringBuilder();
        //                    Loopsb.Append(objEncMainsb.ToString());
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                    //Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, objPatientResults.Encounter_ID.ToString());
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //                    //Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(objPatientResults.Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(objPatientResults.Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, value);
        //                    AddEncountersb.AppendFormat(Loopsb.ToString());
        //                    SubXMLsb.Append(AddEncountersb.ToString());
        //                }
        //            }

        //            break;

        //        case "hbalcwithoutnegnation.xml":

        //            if (PQRIResultDTO_Value.LoincIdentifier != "")
        //            {
        //                VitalsManager objVitalsManager = new VitalsManager();
        //                PatientResults objPatientResults = objVitalsManager.GetResultByLoincObservationAndEncounterId(PQRIResultDTO_Value.LoincIdentifier, PQRIResultDTO_Value.HumanID, PQRIResultDTO_Value.EncounterID);//we want to change
        //                string value = objPatientResults.Value.ToString();
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\hbalcwithoutnegnation.xml"));
        //                objEncMainsb = new StringBuilder(xDox.ToString());
        //                objEncMainsb.Remove(0, 157);
        //                objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                AddEncountersb = new StringBuilder();

        //                Loopsb = new StringBuilder();
        //                Loopsb.Append(objEncMainsb.ToString());
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(objPatientResults.Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(objPatientResults.Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, value);
        //                AddEncountersb.AppendFormat(Loopsb.ToString());
        //                SubXMLsb.Append(AddEncountersb.ToString());
        //            }

        //            break;



        //        case "HighBPI10.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HighBPI10.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;


        //        case "PatientTransfer.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\PatientTransfer.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "OBS_controlling_BP_Stage3.xml":

        //            if (PQRIResultDTO_Value.LoincIdentifier != "")
        //            {
        //                VitalsManager objVitalsManager = new VitalsManager();
        //                IList<PatientResults> lstpatientRresults = new List<PatientResults>();

        //                lstpatientRresults = objVitalsManager.GetResultByLoincObservationAndEncounterIdStage3(PQRIResultDTO_Value.LoincIdentifier, PQRIResultDTO_Value.HumanID, Convert.ToDateTime(dtpFromDate.SelectedDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd"));
        //                for (int i = 0; i < lstpatientRresults.Count; i++)
        //                {
        //                    if (lstpatientRresults[i].Value != "")
        //                    {
        //                        string value = lstpatientRresults[i].Value.ToString();
        //                        string[] test = value.Split('/');
        //                        xDox = new XDocument();
        //                        xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_controlling_BP_Stage3.xml"));
        //                        objEncMainsb = new StringBuilder(xDox.ToString());
        //                        objEncMainsb.Remove(0, 157);
        //                        objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                        AddEncountersb = new StringBuilder();

        //                        Loopsb = new StringBuilder();
        //                        Loopsb.Append(objEncMainsb.ToString());

        //                        if (test[0] != "")
        //                        {
        //                            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=", Loopsb.ToString().IndexOf("extension=", Loopsb.ToString().IndexOf("extension=") + 1) + 1) + 34, i);
        //                            //Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //                            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //                            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(lstpatientRresults[i].Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(lstpatientRresults[i].Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                            Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, test[0]);
        //                        }
        //                        else
        //                        {
        //                            int index1 = Loopsb.ToString().IndexOf("<entry>");
        //                            int index2 = Loopsb.ToString().IndexOf("</entry>");
        //                            Loopsb.Remove(index1, 799);

        //                        }

        //                        if (test.Length > 1)
        //                        {
        //                            if (test[1] != "")
        //                            {
        //                                Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, i);
        //                                Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(lstpatientRresults[i].Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                                Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(lstpatientRresults[i].Captured_date_and_time).ToString("yyyyMMddhhmmss"));
        //                                Loopsb.Insert(Loopsb.ToString().LastIndexOf("unit") - 2, test[1]);
        //                            }
        //                            else
        //                            {
        //                                int index1 = Loopsb.ToString().LastIndexOf("<entry>");
        //                                int index2 = Loopsb.ToString().LastIndexOf("</entry>");
        //                                Loopsb.Remove(index1, 928);

        //                            }
        //                        }
        //                        else
        //                        {
        //                            int index1 = Loopsb.ToString().LastIndexOf("<entry>");
        //                            int index2 = Loopsb.ToString().LastIndexOf("</entry>");
        //                            Loopsb.Remove(index1, 928);
        //                        }


        //                        AddEncountersb.AppendFormat(Loopsb.ToString());
        //                        SubXMLsb.Append(AddEncountersb.ToString());
        //                    }

        //                    else
        //                    {
        //                        xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_BP_NotPerformed_Cat1_Stage3.xml"));
        //                        objEncMainsb = new StringBuilder(xDox.ToString());
        //                        objEncMainsb.Remove(0, 157);
        //                        objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                        AddEncountersb = new StringBuilder();
        //                        Loopsb = new StringBuilder();
        //                        Loopsb.Append(objEncMainsb.ToString());

        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, +iRandomNo);
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("code=") + 6, lstpatientRresults[i].Snomed_Code);

        //                        AddEncountersb.AppendFormat(Loopsb.ToString());
        //                        SubXMLsb.Append(AddEncountersb.ToString());


        //                        xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\HP_Reason_Cat1_Stage3.xml"));
        //                        objEncMainsb = new StringBuilder(xDox.ToString());
        //                        objEncMainsb.Remove(0, 157);
        //                        objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                        AddEncountersb = new StringBuilder();
        //                        Loopsb = new StringBuilder();
        //                        Loopsb.Append(objEncMainsb.ToString());

        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, +iRandomNo);

        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("code=") + 6, lstpatientRresults[i].Snomed_Code);
        //                        AddEncountersb.AppendFormat(Loopsb.ToString());
        //                        SubXMLsb.Append(AddEncountersb.ToString());
        //                    }
        //                }
        //            }

        //            break;
        //        case "Medication_Admin_Pneu_Vac_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Medication_Admin_Pneu_Vac_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, (EncLst == null) ? PQRIResultDTO_Value.HumanID.ToString() : EncLst.Id.ToString());//PQRIResultDTO_Value
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, (EncLst == null) ? ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss") : ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, (EncLst == null) ? ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss") : ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Encounter_Section_Mammogram.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Encounter_Section_Mammogram.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            //Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            if (PQRIResultDTO_Value.MeasureName.ToUpper().Replace(" ", "") == "CMS165V5" && PQRIResultDTO_Value.ProcedureCode == "G0438")
        //            {
        //                EAndMCodingManager em = new EAndMCodingManager();
        //                IList<EAndMCoding> lst = new List<EAndMCoding>();
        //                lst = em.GetDetailsByEncounterID(EncLst.Id);
        //                IList<EAndMCoding> lst1 = (from m in lst where m.Procedure_Code == "G0438" select m).ToList<EAndMCoding>();
        //                IList<EAndMCoding> lst2 = (from m in lst where m.Procedure_Code == "99213" select m).ToList<EAndMCoding>();
        //                if (lst1.Count == 2 && lst2.Count == 2)
        //                    xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ControllingBP_Gcode_Cat1_Stage3.xml"));
        //                objEncMainsb = new StringBuilder(xDox.ToString());
        //                objEncMainsb.Remove(0, 157);
        //                objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //                AddEncountersb = new StringBuilder();

        //                Loopsb = new StringBuilder();
        //                Loopsb.Append(objEncMainsb.ToString());

        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                AddEncountersb.AppendFormat(Loopsb.ToString());
        //                SubXMLsb.Append(AddEncountersb.ToString());

        //            }

        //            break;
        //        case "Unilateral_Mastectomy.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Unilateral_Mastectomy.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "RightMastectomy.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\RightMastectomy.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "LeftMastectomy.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\LeftMastectomy.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Bilateral_Mastectomy.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Bilateral_Mastectomy.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Historyofbilateralmastectomy.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Historyofbilateralmastectomy.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "UnspecifiedLateralityMas.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\UnspecifiedLateralityMas.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "MammogramLoinc1.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\MammogramLoinc1.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "MammogramLoinc2.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\MammogramLoinc2.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "FacetoFace.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\FacetoFace.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Influenza.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Influenza.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Refusednegation.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Refusednegation.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "InfluenzaNotNull.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\InfluenzaNotNull.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "AllergytoEggs.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\AllergytoEggs.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Influenzafavour.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Influenzafavour.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Controlling_BP_Snomed_CAT_I_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Controlling_BP_Snomed_CAT_I_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);

        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "ControllingBP_Kidney_Edu_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ControllingBP_Kidney_Edu_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Controlling_HP_Kidney_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Controlling_HP_Kidney_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<translation code=") + 19, PQRIResultDTO_Value.ProcedureCode);
        //            if (PQRIResultDTO_Value.ProcedureCode == "36800")
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("codeSystem=") + 12, "2.16.840.1.113883.6.12");
        //            else
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("codeSystem=") + 12, "2.16.840.1.113883.6.285");

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Controlling_BP_Snomed_Visit_CAT1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Controlling_BP_Snomed_Visit_CAT1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);

        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Influenza_CVX_Cat1_Stage3.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Influenza_CVX_Cat1_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);

        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "InfluenzaAllergy.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\InfluenzaAllergy.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "InfluenzaSnomedCAT1.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\InfluenzaSnomedCAT1.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "ICD_influenza.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ICD_influenza.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, "5a169a471d41c80456956" + iRandomNo);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Refused_Influenza.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Refused_Influenza.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Refused_medical_reason.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Refused_medical_reason.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "Current_Smoker.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Current_Smoker.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "Tobbacco.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Tobbacco.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "OBS_Tobacco use.xml"://Encoutner Id=0
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Tobacco use.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            //SocialHistoryManager objSocialHistoryManager = new SocialHistoryManager();
        //            //objSocialHistoryManager.GetSocialHistoryByHumanID(


        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, PQRIResultDTO_Value.HumanID);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());


        //            break;

        //        case "Substance administration.xml"://NCIDC
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Substance administration.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, PQRIResultDTO_Value.HumanID.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("code code=") + 11, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        case "ProcedurePerformed.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\ProcedurePerformed.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        case "OBS_Diabetes.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Diabetes.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_HB1AC_CMS_122_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_HB1AC_CMS_122_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_HBP_Followup_CMS22_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_HBP_Followup_CMS22_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);

        //            if (PQRI_Data_value.NQF_Number != "CMS22v5")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(20).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(20).ToString("yyyyMMddhhmmss"));
        //            }
        //            else
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(PQRIResultDTO_Value.ResultDateandTime).ToString("yyyyMMddhhmmss"));
        //            }
        //            //Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_Contoll_HBP_Exclusion_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Contoll_HBP_Exclusion_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));


        //            if (PQRI_Data_value.PQRI_Value == "N18.5")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<value code=") + 13, "433146000");
        //            }
        //            else if (PQRI_Data_value.PQRI_Value == "N18.6")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<value code=") + 13, "236435004");
        //            }
        //            else
        //            {
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<value code=") + 13, "47200007");

        //            }

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        case "OBS_BMI_Cat1_Stag3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_BMI_Cat1_Stag3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_BMI_ICD_CATI_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_BMI_ICD_CATI_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("time value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_Influenza_CatI_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Influenza_CatI_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));

        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));

        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));

        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_BMI_Diet_CatI_Stage3.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_BMI_Diet_CatI_Stage3.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            // Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("<time value=") + 13, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(-1).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;


        //        case "OBS_PREGNANCY_DX.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_PREGNANCY_DX.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value.Trim() == "661.3" ? "O62.3" : PQRI_Data_value.PQRI_Value.Trim());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_PREGNANCY.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_PREGNANCY.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;

        //        case "OBS_Phy Exam_Finding_BMI.xml"://Doubt
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Phy Exam_Finding_BMI.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("unit") - 2, PQRIResultDTO_Value.Value);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        case "Procedure_Activity_Act.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedure_Activity_Act.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "OBS_Diagnosis Active.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Diagnosis Active.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        //case "OBS_Physical Exam_Finding_BP_Diastolic.xml":

        //        //    break;
        //        //case "OBS_Physical Exam_Finding_BP_Systolic.xml":

        //        //    break;
        //        case "OBS_Lab result.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Lab result.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());


        //            break;

        //        case "OBS_Dental Carries.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\OBS_Dental Carries.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("value code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;




        //        case "Procedure Performed _current med_documented.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Procedure Performed _current med_documented.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;

        //        case "CMS138v5_stage3_MedicationOrder.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_MedicationOrder.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394b89636239045aeb762c") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("time value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_MedicationOrder_I.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_MedicationOrder_I.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394b89636239045aeb75f9") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_Nullflavor.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_Nullflavor.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394b89636239045aeb7672") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_Assertion.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_Assertion.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<value code=") + 13, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_patientDead.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_patientDead.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_procActivity.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_procActivity.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            if (Humanlst.First_Name.ToUpper().Equals("MAXINE") && Humanlst.Last_Name.ToUpper().Equals("RAMOS"))
        //            {
        //                Loopsb.Replace("225323000", "185795007");
        //            }
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_Loinc.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_Loinc.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS138v5_stage3_ProblemConcern.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS138v5_stage3_ProblemConcern.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS69v5_stage3_I.xml":
        //            bool repeat_Case = false;
        //        l: xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS69v5_stage3_I.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);

        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394ae26362390456393ee8") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("time value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            if (PQRI_Data_value.PQRI_Value == "103699006" || PQRI_Data_value.PQRI_Value == "308470006")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //                int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                Loopsb.Remove(index_Remove, 288);
        //            }
        //            if (PQRI_Data_value.PQRI_Value == "370847001")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            }
        //            if (PQRI_Data_value.PQRI_Value == "304549008")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //                int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                Loopsb.Remove(index_Remove, 64);
        //                int indexLast_Remove = Loopsb.ToString().LastIndexOf("<translation");
        //                Loopsb.Remove(indexLast_Remove, 64);
        //                Loopsb.Replace("S9470", "S9451");
        //                Loopsb.Replace("V65.3", "V65.41");
        //            }
        //            if (PQRI_Data_value.PQRI_Value == "410177006")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //                int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                Loopsb.Remove(index_Remove, 64);
        //                if (!repeat_Case)
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1525");
        //                    repeat_Case = true;
        //                }
        //                else
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1528");
        //                    repeat_Case = false;
        //                }

        //            }
        //            if (PQRI_Data_value.PQRI_Value == "390906007")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("code=") + 6, PQRI_Data_value.PQRI_Value);
        //            }
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            if (repeat_Case)
        //                goto l;
        //            break;
        //        case "CMS69v5_stage3_LoincBMI.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS69v5_stage3_LoincBMI.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("value=") + 7, PQRIResultDTO_Value.Value);
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS69v5_stage3_MedOrder.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS69v5_stage3_MedOrder.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            if (PQRI_Data_value.PQRI_Value == "860221")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("unit=") + 6, "tbl");
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.2388");
        //            }
        //            else if (PQRI_Data_value.PQRI_Value == "692876")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("unit=") + 6, "Capsule");
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.2387");
        //            }
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394ae26362390456393ee5") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("time value") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS69v5_stage3_MedOrderNA.xml":
        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS69v5_stage3_MedOrderNA.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            int Loinc_value;
        //            VitalsManager vm = new VitalsManager();
        //            IList<PatientResults> patResult = new List<PatientResults>();
        //            patResult = vm.GetVitalsByHumanEncounter(PQRIResultDTO_Value.EncounterID, PQRIResultDTO_Value.HumanID);
        //            if (patResult != null && patResult.Count > 0)
        //            {
        //                IList<PatientResults> rcopMed = new List<PatientResults>();
        //                rcopMed = patResult.Where(a => a.Loinc_Identifier == "39156-5").ToList<PatientResults>();
        //                if (rcopMed != null && rcopMed.Count > 0)
        //                {
        //                    PQRIResultDTO_Value.Value = rcopMed[0].Value;
        //                }
        //            }
        //            if (int.TryParse(PQRIResultDTO_Value.Value, out Loinc_value))
        //            {

        //                if (Convert.ToInt32(Loinc_value) <= 20)
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1499");
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, "Medication, Order: Below Normal Medications");
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, "Medication, Order: Below Normal Medications");
        //                }
        //                else if (Convert.ToInt32(Loinc_value) > 20)
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1498");
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, "Medication, Order: Above Normal Medications");
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, "Medication, Order: Above Normal Medications");
        //                }
        //            }


        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394ae16362390456393e8a") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        case "CMS69v5_stage3_PlanfCareNA.xml":
        //            bool repeatNA_Case = false;
        //        m: xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS69v5_stage3_PlanfCareNA.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);

        //            AddEncountersb = new StringBuilder();
        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("5a394ae16362390456393e93") + 15, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("time value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            if (Humanlst.Id != 0)
        //            {
        //                if (Humanlst.Last_Name.ToUpper().Equals("PARK") && Humanlst.First_Name.ToUpper().Equals("GREG"))
        //                {
        //                    if (!repeatNA_Case)
        //                    {
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1525");
        //                        repeatNA_Case = true;
        //                    }
        //                    else
        //                    {
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1528");
        //                        int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                        Loopsb.Remove(index_Remove, 191);
        //                        repeatNA_Case = false;
        //                    }

        //                }
        //                else if (Humanlst.Last_Name.ToUpper().Equals("SPENCER") && Humanlst.First_Name.ToUpper().Equals("MARILYN"))
        //                {
        //                    int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                    Loopsb.Remove(index_Remove, 254);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1527");

        //                }
        //                else if (Humanlst.Last_Name.ToUpper().Equals("HOGAN") && Humanlst.First_Name.ToUpper().Equals("STELLA"))
        //                {
        //                    if (!repeatNA_Case)
        //                    {
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1525");
        //                        int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                        Loopsb.Remove(index_Remove, 129);
        //                        Loopsb.Replace("Z71.3", "S9449");
        //                        Loopsb.Replace("2.16.840.1.113883.6.90", "2.16.840.1.113883.6.285");
        //                        repeatNA_Case = true;
        //                    }
        //                    else
        //                    {
        //                        Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, "2.16.840.1.113883.3.600.1.1528");
        //                        int index_Remove = Loopsb.ToString().IndexOf("<translation");
        //                        Loopsb.Remove(index_Remove, 64);
        //                        Loopsb.Replace("S9470", "S9449");
        //                        repeatNA_Case = false;
        //                    }
        //                }

        //            }
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            if (repeatNA_Case)
        //                goto m;
        //            break;
        //        case "CMS69v5_stage3_ProblemConcern.xml":

        //            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CMS69v5_stage3_ProblemConcern.xml"));
        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());

        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, (EncLst.Id + Convert.ToUInt32(iRandomNo)).ToString());
        //            Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //            Loopsb.Insert(Loopsb.ToString().LastIndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());

        //            break;
        //        case "Current_Medications_CAT_I_Stage3.xml":
        //            if (PQRI_Data_value.PQRI_Value == "NullFlvr")
        //            {
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Current_Medications_CAT_I_Stage3_II_NullFlvr.xml"));
        //            }
        //            else if (PQRI_Data_value.PQRI_Value.IndexOf('|') == -1 && !PQRI_Data_value.PQRI_Type.Contains("Visit") && PQRI_Data_value.PQRI_Value != "Medication")
        //            {
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Current_Medications_CAT_I_Stage3_I.xml"));
        //            }
        //            else if (PQRI_Data_value.PQRI_Type == "Visit")
        //            {
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CurrentMedication_Visit_Cat1_Stage3.xml"));
        //            }
        //            else if (PQRI_Data_value.PQRI_Type == "VisitCode")
        //            {
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CurrentMedication_VisitCode_Cat1_Stage3.xml"));
        //            }
        //            else if (PQRI_Data_value.PQRI_Value == "Medication")
        //            {
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\CurrentMedication_Reviewed_Cat1_Stage3.xml"));
        //            }
        //            else
        //            {
        //                xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\Current_Medications_CAT_I_Stage3.xml"));
        //            }

        //            objEncMainsb = new StringBuilder(xDox.ToString());
        //            objEncMainsb.Remove(0, 157);
        //            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
        //            AddEncountersb = new StringBuilder();

        //            Loopsb = new StringBuilder();
        //            Loopsb.Append(objEncMainsb.ToString());
        //            if (PQRI_Data_value.PQRI_Value != "NullFlvr" && !PQRI_Data_value.PQRI_Type.Contains("Visit") && PQRI_Data_value.PQRI_Value != "Medication")
        //            {
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                int num = Convert.ToInt32(PQRI_Data_value.PQRI_Value.Split('&')[1]);
        //                PQRI_Data_value.PQRI_Value = PQRI_Data_value.PQRI_Value.Split('&')[0];
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 35, num.ToString());
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 35, num.ToString());
        //                if (PQRI_Data_value.PQRI_Value.IndexOf('|') == -1)
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                    if (PQRI_Data_value.PQRI_Type == "SNOMEDCT")
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("codeSystem=") + 12, "2.16.840.1.113883.6.96");
        //                    else if (PQRI_Data_value.PQRI_Type == "CPT")
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("codeSystem=") + 12, "2.16.840.1.113883.6.12");
        //                    else if (PQRI_Data_value.PQRI_Type == "HCPCS")
        //                        Loopsb.Insert(Loopsb.ToString().LastIndexOf("codeSystem=") + 12, "2.16.840.1.113883.6.285");
        //                }
        //                else
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value.Split('|')[0]);
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("<translation code=") + 19, PQRI_Data_value.PQRI_Value.Split('|')[1]);
        //                }

        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.PQRI_Description);
        //                if (PQRI_Data_value.PQRI_Value == "99203")
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, "201603011500");
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, "201603011600");
        //                }
        //                else
        //                {
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                    Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).AddMinutes(10).ToString("yyyyMMddhhmmss"));
        //                }
        //            }
        //            else if (PQRI_Data_value.PQRI_Type == "Visit")
        //            {
        //                Loopsb = new StringBuilder();
        //                Loopsb.Append(objEncMainsb.ToString());
        //                // Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, sValueSet);

        //            }

        //            else if (PQRI_Data_value.PQRI_Type == "VisitCode")
        //            {
        //                Loopsb = new StringBuilder();
        //                Loopsb.Append(objEncMainsb.ToString());
        //                //  Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("extension=") + 11, EncLst.Id.ToString());
        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("<code code=") + 12, PQRI_Data_value.PQRI_Value);
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("<translation code=") + 19, sValueSet.Split('|')[0]);
        //                if (sValueSet.Split('|').Count() > 0)
        //                    Loopsb.Insert(Loopsb.ToString().LastIndexOf("<translation code=") + 19, sValueSet.Split('|')[1]);


        //            }

        //            else if (PQRI_Data_value.PQRI_Value == "Medication")
        //            {
        //                Loopsb = new StringBuilder();
        //                Loopsb.Append(objEncMainsb.ToString());

        //                Loopsb.Insert(Loopsb.ToString().LastIndexOf("extension=") + 11, EncLst.Id.ToString());

        //                Loopsb.Insert(Loopsb.ToString().IndexOf("low value=") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
        //                Loopsb.Insert(Loopsb.ToString().IndexOf("high value=") + 12, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

        //            }
        //            AddEncountersb.AppendFormat(Loopsb.ToString());
        //            SubXMLsb.Append(AddEncountersb.ToString());
        //            break;
        //        default:
        //            break;
        //    }
        //    return SubXMLsb;
        //}_Snomed

        public StringBuilder SubXMLLoadCATIStageThree(PQRI_Data PQRI_Data_value, PQRIResultDTO PQRIResultDTO_Value, Encounter EncLst, string sValueSet, Human Humanlst)
        {

            iRandomNo++;
            StringBuilder SubXMLsb = new StringBuilder();
            StringBuilder objEncMainsb;
            StringBuilder AddEncountersb;
            StringBuilder Loopsb;
            var xDox = new XDocument();

            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\" + PQRI_Data_value.PQRI_Selection_XML));
            objEncMainsb = new StringBuilder(xDox.ToString());
            objEncMainsb.Remove(0, 157);
            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
            AddEncountersb = new StringBuilder();
            Loopsb = new StringBuilder();
            Loopsb.Append(objEncMainsb.ToString());



            if (Session["PrimaryICD"].ToString() != "")
            {
                //<value xsi:type="CD" code="{}"

                if (Session["PrimaryICD"].ToString() != "Nullflavor")
                {
                    if (Loopsb.ToString().IndexOf(@"<value xsi:type=""CD"" code=""{}""") > -1)

                        Loopsb.Insert(Loopsb.ToString().IndexOf(@"<value xsi:type=""CD"" code=""{}""") + 29, Session["PrimaryICD"].ToString());
                }
                else
                {
                    if (Loopsb.ToString().IndexOf(@"<value xsi:type=""CD"" code=""{}""") > -1)
                    {
                        Loopsb.Insert(Loopsb.ToString().IndexOf(@"<value xsi:type=""CD"" code=""{}""") + 21,
                            @"nullFlavor=""NA""");
                        int startindex = Loopsb.ToString().IndexOf(@"code=""{}"" codeSystem=""2.16.840.1.113883.6.90"" codeSystemName=""ICD10CM""");
                        int endindex = Loopsb.ToString().IndexOf("ICD10CM")+8;
                        Loopsb.Replace(Loopsb.ToString().Substring(startindex, endindex - startindex), "");
                        // Loopsb.ToString().Replace(@"code=""{}"" codeSystem=""2.16.840.1.113883.6.90"" codeSystemName=""ICD10CM""", "");
                       // Loopsb.ToString().Replace(, "");
                    }
                    //    xmlReqNode = xDox.GetElementsByTagName("value");
                    //    xmlReqNode[0].Attributes.RemoveAll();
                    //    XmlAttribute xAttribute = xmlReqNode[0].OwnerDocument.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
                    //    xAttribute.Value = "REAL";
                    //    xmlReqNode[0].Attributes.Append(xAttribute);
                    //    XmlAttribute xAttributenull = xmlReqNode[0].OwnerDocument.CreateAttribute("nullFlavor");
                    //    xAttributenull.Value = "NA";
                    //    xmlReqNode[0].Attributes.Append(xAttributenull);
                }


                //= "Nullflavor";
            }


            if (Loopsb.ToString().IndexOf(@"<time value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<time value=""{}""") + 13, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

            if (Loopsb.ToString().IndexOf("<text>") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.Standard_concept);
            if (Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") + 22, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
            if (Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") + 22, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
            if (Loopsb.ToString().IndexOf(@"<code code=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<code code=""{}""") + 12, PQRI_Data_value.PQRI_Value);
            if (Loopsb.ToString().IndexOf(@"displayName=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"displayName=""{}""") + 13, PQRI_Data_value.PQRI_Description);
            if (Loopsb.ToString().IndexOf(@"low value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"low value=""{}""") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
            if (Loopsb.ToString().IndexOf(@"low value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"low value=""{}""") + 11, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

            if (Loopsb.ToString().IndexOf(@"high value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"high value=""{}""") + 12, ConvertToLocal(EncLst.Date_of_Service).AddHours(1).ToString("yyyyMMddhhmmss"));

            if (Loopsb.ToString().IndexOf(@"high value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"high value=""{}""") + 12, ConvertToLocal(EncLst.Date_of_Service).AddHours(1).ToString("yyyyMMddhhmmss"));
            if (Loopsb.ToString().IndexOf(@"<value code=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<value code=""{}""") + 13, PQRI_Data_value.PQRI_Value);
            if (Loopsb.ToString().LastIndexOf(@"code=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().LastIndexOf(@"code=""{}""") + 6, PQRI_Data_value.PQRI_Value);

            if (Loopsb.ToString().IndexOf("valueSet=") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf("valueSet=") + 10, sValueSet);


            if (Loopsb.ToString().IndexOf("<originalText>") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf("<originalText>") + 14, PQRI_Data_value.PQRI_Description);


            AddEncountersb.AppendFormat(Loopsb.ToString().Replace("{}", ""));
            SubXMLsb.Append(AddEncountersb.ToString());

            return SubXMLsb;
        }
        public StringBuilder SubXMLLoadCATIStageThreeLoinc(PQRI_Data PQRI_Data_value, PQRIResultDTO PQRIResultDTO_Value,
            Encounter EncLst, string sValueSet, Human Humanlst, IList<PQRIResultDTO> PQRI_Data_value_snomed)
        {
            IList<StaticLookup> objstaticlookup = new List<StaticLookup>();
            StaticLookupManager stmMngr = new StaticLookupManager();
            objstaticlookup = stmMngr.getStaticLookupByFieldNameAndValue("fobtsnomed", PQRI_Data_value_snomed[0].Value);

            iRandomNo++;
            StringBuilder SubXMLsb = new StringBuilder();
            StringBuilder objEncMainsb;
            StringBuilder AddEncountersb;
            StringBuilder Loopsb;
            var xDox = new XDocument();

            xDox = XDocument.Load(HttpContext.Current.Server.MapPath("SampleXML" + "\\" + PQRI_Data_value.PQRI_Selection_XML));
            objEncMainsb = new StringBuilder(xDox.ToString());
            objEncMainsb.Remove(0, 157);
            objEncMainsb.Remove(objEncMainsb.Length - 20, 20);
            AddEncountersb = new StringBuilder();
            Loopsb = new StringBuilder();
            Loopsb.Append(objEncMainsb.ToString());


            if (Loopsb.ToString().IndexOf(@"<time value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<time value=""{}""") + 13, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
            if (objstaticlookup.Count > 0)
            {
                if (Loopsb.ToString().IndexOf(@"displayName=""{}""") > -1)
                    Loopsb.Insert(Loopsb.ToString().IndexOf(@"displayName=""{}""") + 13, objstaticlookup[0].Description);
            }
           if (Loopsb.ToString().IndexOf(@"code=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"code=""{}""") + 6, PQRI_Data_value.PQRI_Value);
            if(Loopsb.ToString().IndexOf(@"code=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"code=""{}""") + 6, PQRI_Data_value.PQRI_Value);
            if (Loopsb.ToString().LastIndexOf(@"code=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().LastIndexOf(@"code=""{}""") + 6, PQRI_Data_value_snomed[0].Value);
            if (Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") + 22, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));
            if (Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf(@"<effectiveTime value=""{}""") + 22, ConvertToLocal(EncLst.Date_of_Service).ToString("yyyyMMddhhmmss"));

            if (Loopsb.ToString().IndexOf("<text>") > -1)
                Loopsb.Insert(Loopsb.ToString().IndexOf("<text>") + 6, PQRI_Data_value.Standard_concept);

            AddEncountersb.AppendFormat(Loopsb.ToString().Replace("{}", ""));
            SubXMLsb.Append(AddEncountersb.ToString());

            return SubXMLsb;
        }

        
        #endregion

        protected void btndownloadCATI_Click(object sender, EventArgs e)
        {
            DownLoadZIPFormate(Server.MapPath("Documents/" + Session.SessionID + "/CQMI"));
            //string PhiMailDirectory = System.Configuration.ConfigurationManager.AppSettings["phiMailDownloadDirectory"].ToString() + "\\PQRI_Measure\\" + ClientSession.PhysicianId;
            //if (!Directory.Exists(PhiMailDirectory))
            //{
            //    Directory.CreateDirectory(PhiMailDirectory);
            //}
            //if (Directory.Exists(PhiMailDirectory))
            //{
            //    string folder_Name = PhiMailDirectory;
            //    if (Directory.Exists(folder_Name))
            //    {
            //        string[] sub_folder_Names = Directory.GetDirectories(folder_Name);
            //        if (sub_folder_Names.Length > 0)
            //        {
            //            foreach (string subFolder in sub_folder_Names)
            //            {
            //                string[] fileEntries = Directory.GetFiles(subFolder);
            //                foreach (string fileName in fileEntries)
            //                {
            //                    File.SetCreationTime(fileName, DateTime.Now);
            //                    File.SetLastWriteTime(fileName, DateTime.Now);
            //                    File.SetLastAccessTime(fileName, DateTime.Now);
            //                }

            //                using (ZipFile zip = new ZipFile())
            //                {
            //                    zip.AddDirectory(subFolder);
            //                    zip.Save(subFolder + ".zip");
            //                    Directory.Delete(subFolder, true);
            //                }
            //            }
            //            using (ZipFile zip = new ZipFile())
            //            {
            //                zip.AddDirectory(folder_Name);
            //                Response.Clear();
            //                Response.BufferOutput = false;
            //                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
            //                Response.ContentType = "application/zip";
            //                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
            //                zip.Save(Response.OutputStream);
            //                Response.End();
            //            }
            //        }
            //    }
            //}
        }

        protected void grdPQRIMeasure_PreRender(object sender, EventArgs e)
        {
            if (cboStage.Text == "Stage 3")
            {
                grdPQRIMeasure.MasterTableView.GetColumn("RateRequired").Visible = false;
                grdPQRIMeasure.MasterTableView.GetColumn("Cleared").Visible = false;
            }
            else
            {
                grdPQRIMeasure.MasterTableView.GetColumn("RateRequired").Visible = true;
                grdPQRIMeasure.MasterTableView.GetColumn("Cleared").Visible = true;
            }
            grdPQRIMeasure.Rebind();
        }

        public ulong Dx_medicaid_count { get; set; }

        protected void chkProviderName_CheckedChanged(object sender, EventArgs e)
        {
            FillPhysicianUser PhyUserList;
            cboProviderName.Items.Clear();
            if (chkProviderName.Checked == true)
                PhyUserList = phyMngr.GetPhysicianandUser(false, string.Empty, ClientSession.LegalOrg);
            else
                PhyUserList = phyMngr.GetPhysicianandUser(true, ClientSession.FacilityName, ClientSession.LegalOrg);
            if (PhyUserList != null && PhyUserList.PhyList.Count > 0) //code added by balaji.tj 2015-12-09
            {
                for (int i = 0; i < PhyUserList.PhyList.Count; i++)
                {
                    //Old Code
                    //string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
                    //Gitlab# 2485 - Physician Name Display Change
                    string sPhyName = string.Empty;
                    if (PhyUserList.PhyList[i].PhyLastName != String.Empty)
                        sPhyName += PhyUserList.PhyList[i].PhyLastName;
                    if (PhyUserList.PhyList[i].PhyFirstName != String.Empty)
                    {
                        if (sPhyName != String.Empty)
                            sPhyName += "," + PhyUserList.PhyList[i].PhyFirstName;
                        else
                            sPhyName += PhyUserList.PhyList[i].PhyFirstName;
                    }
                    if (PhyUserList.PhyList[i].PhyMiddleName != String.Empty)
                        sPhyName += " " + PhyUserList.PhyList[i].PhyMiddleName;
                    if (PhyUserList.PhyList[i].PhySuffix != String.Empty)
                        sPhyName += "," + PhyUserList.PhyList[i].PhySuffix;
                    //Old Code
                    //cboProviderName.Items.Add(PhyUserList.UserList[i].user_name.ToString() + " - " + sPhyName);
                    cboProviderName.Items.Add(sPhyName);
                    cboProviderName.Items[i].Value = PhyUserList.PhyList[i].Id.ToString();
                    cboProviderName.ToolTip = cboProviderName.Items[i].Text;

                    if (Convert.ToUInt64(cboProviderName.Items[i].Value) == ClientSession.PhysicianId)
                        cboProviderName.SelectedIndex = i;
                    else
                    { }
                }
            }
            if (chkProviderName.Checked == true)
            {
                var phyName = from p in PhyUserList.PhyList where (p.Id == ClientSession.PhysicianId) select p;
                if (phyName.Count() != 0)
                {
                    string PhysicianName = phyName.ToList<PhysicianLibrary>()[0].PhyPrefix + " " + phyName.ToList<PhysicianLibrary>()[0].PhyFirstName + " " + phyName.ToList<PhysicianLibrary>()[0].PhyMiddleName + " " + phyName.ToList<PhysicianLibrary>()[0].PhyLastName + " " + phyName.ToList<PhysicianLibrary>()[0].PhySuffix;
                    cboProviderName.Text = ClientSession.UserName + " - " + PhysicianName;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
    }
}
