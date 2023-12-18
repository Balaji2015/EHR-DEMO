using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
//using Microsoft.Office.Interop.Excel;
//using Acurus.Capella.FrontOffice;
using Acurus.Capella.DataAccess.ManagerObjects;

using Acurus.Capella.UI.UserControls;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace Acurus.Capella.UI
{
    public partial class frmVitals : System.Web.UI.Page
    {
        VitalsManager vitalmngr = new VitalsManager();
        UIManager objUiManager = new UIManager();
        UtilityManager objUtilMngr = new UtilityManager();
        StaticLookupManager staticLookupManager = new StaticLookupManager();
        UserLookupManager localFieldLookupManager = new UserLookupManager();
        UtilityManager objUtilityManager = new UtilityManager();
        DataSet ds = new DataSet();
        DataTable dtTable = new DataTable();
        DataRow dr = null;

        ArrayList aryNotesFilled = new ArrayList();
        ArrayList aryLabelFilled = new ArrayList();
        ArrayList arynumericFilled = new ArrayList();
        ArrayList arynumericcreationFilled = new ArrayList();
        ArrayList arydatetimeFilled = new ArrayList();
        ArrayList arydatetimeIDlist = new ArrayList();
        IList<Control> mandatoryCtrls = null;
        IList<StaticLookup> ilstLookup = new List<StaticLookup>();
        IList<StaticLookup> ilstLookupRefuse = new List<StaticLookup>();
        bool allEmpty = false;
        double humanAgeInMonths = 0;
        bool IsLoad = false;
        double human_AgeInMonths = 0;
        string human_Sex = string.Empty;
        ulong HumanID = 0;
        Table objTable = new Table();
        DateTime utc = DateTime.MinValue;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            if (IsPostBack)
            {
                if (Session["dynamicScreenList"] != null)
                {
                    IList<DynamicScreen> dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];
                    CreateControls(dynamicScreenList);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                }
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            //CurrentDate for vitals set in cookie(VitalCurrentDate) instead of querystring
            PatientResultsDTO objVitalDTO = null;
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            IList<MasterVitals> mastervitalslist = new List<MasterVitals>();
            IList<MapVitalsPhysician> mapVitalPhyList = new List<MapVitalsPhysician>(); ;
            IList<PatientResults> vitalList = new List<PatientResults>();
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            IList<PatientPane> lstPatientPane = new List<PatientPane>();

            double percentileIncrement = 0;
            DateTime BirthDate = DateTime.MinValue;
            string openingfrom = string.Empty;
            inpbtnClearall.Attributes.Add("onclick", "ClearAll('" + inpbtnClearall.ClientID + "')");
            if (Request["openingfrom"] != null && Request["openingfrom"].ToString() != "")
            {
                openingfrom = Request["openingfrom"].ToString();
                hdnOpeningFrom.Value = openingfrom;
            }
            DateTime VitalTakenDate = DateTime.MinValue;
            DateTime VitalTakenDateValue = DateTime.MinValue;
            if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.Trim() != string.Empty)
            {
                if (openingfrom.ToLower() != "menu")
                {
                    VitalTakenDate = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                    VitalTakenDateValue = VitalTakenDate;//DateTime.Now;
                }
                else
                {
                    VitalTakenDate = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                    VitalTakenDateValue = VitalTakenDate;//DateTime.Now;
                }
            }

            if (openingfrom == "Queue")
            {
                if (Request["HumanID"].ToString() != "")
                    HumanID = Convert.ToUInt32(Request["HumanID"].ToString());
                else
                    HumanID = ClientSession.HumanId;


            }

            if (openingfrom != "Queue")
            {

                HumanID = ClientSession.HumanId;


            }

            IList<object> objList = new List<object>();
            ulong ScreenID = 2000;

            decimal InchMaxValue = 0, InchMinValue = 0;

            string humanSex = string.Empty;
            string Loinc_observation = string.Empty;

            // btnSave.Attributes.Add("onclick", "setWaitCursor('true');");
            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                BirthDate = ClientSession.PatientPaneList[0].Birth_Date;
                humanSex = ClientSession.PatientPaneList[0].Sex;
                human_Sex = humanSex;//BugID:51648
                birthdate.Value = (ClientSession.PatientPaneList[0].Birth_Date).ToString();
            }
            Session["BirthDate"] = BirthDate;

            humanAgeInMonths = ((VitalTakenDate.Date - BirthDate.Date).TotalDays) / 30.4375;
            human_AgeInMonths = humanAgeInMonths;//BugID:51648

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                var PatientPaneList = from d in ClientSession.PatientPaneList where d.Encounter_ID == ClientSession.EncounterId select d;
                if (PatientPaneList.Count() > 0)
                {
                    lstPatientPane = PatientPaneList.ToList<PatientPane>();
                    if (lstPatientPane != null && lstPatientPane.Count > 0)
                        VitalTakenDate = lstPatientPane[0].Date_of_Service;
                }
            }

            Session["VitalTakenDate"] = VitalTakenDate;
            if (ClientSession.EncounterId != 0 && openingfrom != "Menu")
            {
                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                {
                    if (lstPatientPane != null && lstPatientPane.Count > 0)
                        VitalTakenDateValue = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service);
                    else
                    {
                        var PatientPaneList = from d in ClientSession.PatientPaneList where d.Encounter_ID == ClientSession.EncounterId select d;
                        if (PatientPaneList.Count() > 0)
                        {
                            lstPatientPane = PatientPaneList.ToList<PatientPane>();
                            if (lstPatientPane != null && lstPatientPane.Count > 0)
                                VitalTakenDateValue = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service);
                        }

                    }
                }
            }
            btnSaveVitals.Attributes.Add("onclick", "return btnSave_Clicked();");
            Session["VitalTakenDateValue"] = VitalTakenDateValue;

            if (!IsPostBack)
            {

                if (openingfrom == "Menu")
                {

                    if (ClientSession.HumanId == 0)
                        ClientSession.HumanId = Convert.ToUInt64(Request["HumanID"].ToString());
                    objVitalDTO = vitalmngr.GetPastVitalDetailsByPatient(ClientSession.HumanId, 1, 25, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID);
                    objList = objVitalDTO.ObjList;
                    vitalList = objVitalDTO.VitalsList;
                    Session["objVitalDTO"] = objVitalDTO;
                    Session["vitalList"] = objVitalDTO.VitalsList;
                    Session["objList"] = objList;
                    btnSaveVitals.Value = "Add";

                }
                else if (openingfrom == "ChildScreen")
                {
                    objVitalDTO = vitalmngr.GetPastVitalDetailsByEncounterID(ClientSession.EncounterId, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, ClientSession.HumanId);
                    objList = objVitalDTO.ObjList;
                    vitalList = objVitalDTO.VitalsList;
                    Session["objVitalDTO"] = objVitalDTO;
                    Session["vitalList"] = objVitalDTO.VitalsList;
                    Session["objList"] = objList;
                }
                else
                {

                    //objVitalDTO = vitalmngr.GetPastVitalDetailsByEncounterID(ClientSession.EncounterId, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, ClientSession.HumanId);
                    objVitalDTO = vitalmngr.GetPastVitalDetailsByEncounterID(ClientSession.EncounterId, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, HumanID);
                    objList = objVitalDTO.ObjList;
                    vitalList = objVitalDTO.VitalsList;
                    Session["objVitalDTO"] = objVitalDTO;
                    Session["vitalList"] = objVitalDTO.VitalsList;
                    Session["objList"] = objList;
                }
                StaticLookUpList = staticLookupManager.getStaticLookupByFieldName(new string[] { "HEIGHT INCH MAXIMUM VALUE", "BP DEFAULT VALUES", "HBA1C STATUS", "VITALS LIMIT LEVEL", "GFR STATUS", "BLOOD SUGAR-FASTING STATUS", "BLOOD SUGAR-POST PRANDIAL STATUS", "BMI STATUS", "BMI PERCENTILE STATUS", "URINE FOR MICROALBUMINSTATUS", "ABI TESTSTATUS", "BP SYSTOLIC STATUS", "BP DIASTOLIC STATUS", "REASON NOT PERFORMED FOR HEIGHT", "REASON NOT PERFORMED FOR WEIGHT", "REASON NOT PERFORMED FOR BP-SITTING SYS/DIA", "HGB STATUS" });
                Session["StaticLookUpList"] = StaticLookUpList;
                EnableDisbaleSave(false);
                string[] names = { "REASON NOT PERFORMED FOR HEIGHT", "REASON NOT PERFORMED FOR WEIGHT", "REASON NOT PERFORMED FOR BP-SITTING SYS/DIA" };

                var results = (from n in StaticLookUpList
                               where names.Contains(n.Field_Name)
                               select n).ToList();

                hdnreason.Value = "";
                for (int i = 0; i < results.Count; i++)
                {
                    if (i == 0)
                        hdnreason.Value = results[i].Field_Name + "|" + results[i].Value;
                    else
                        hdnreason.Value = hdnreason.Value + "~" + results[i].Field_Name + "|" + results[i].Value;
                }

                if (objList[1] != null && objList[1].GetType().ToString().ToUpper().Contains("DYNAMICSCREEN"))
                    dynamicScreenList = (IList<DynamicScreen>)objList[1];

                //var val = (from d in dynamicScreenList where d.Control_Type.ToUpper() == "COMBOBOX" select d.Control_Name_Thin_Client.ToString());
                //string[] sFieldsCombobox = ((IEnumerable)val).Cast<object>()
                //                     .Select(x => x.ToString().ToUpper())
                //                     .ToArray();
                //string[] lookup = new string[] { "HEIGHT INCH MAXIMUM VALUE", "BP DEFAULT VALUES", "REASON FOR REFUSE", "HBA1C STATUS", "VITALS LIMIT LEVEL", "GFR STATUS", "BLOOD SUGAR-FASTING STATUS", "BLOOD SUGAR-POST PRANDIAL STATUS", "BMI STATUS", "BMI PERCENTILE STATUS", "URINE FOR MICROALBUMINSTATUS", "ABI TESTSTATUS" };



                //string[] newArray = new string[sFieldsCombobox.Length + lookup.Length];



                //sFieldsCombobox.CopyTo(newArray, 0);
                //lookup.CopyTo(newArray, sFieldsCombobox.Length);

                //ilstLookup = staticLookupManager.getStaticLookupByFieldName(newArray);

                //StaticLookUpList = (from a in ilstLookup where lookup.Contains(a.Field_Name) select a).ToList<StaticLookup>();
                //Session["StaticLookUpList"] = StaticLookUpList;

                // ilstLookup = (from a in ilstLookup where sFieldsCombobox.Contains(a.Field_Name) select a).ToList<StaticLookup>();
                EncounterManager objencounter = new EncounterManager();
                IList<Encounter> lstencounter = objencounter.GetEncounterByEncounterID(ClientSession.EncounterId);
                IList<PatientResults> lsvirals = new List<PatientResults>();
                string encounter = "";
                if (lstencounter != null && lstencounter.Count > 0)
                {
                    encounter = vitalmngr.GetAssesmentDetails(lstencounter[0].Human_ID, lstencounter[0].Date_of_Service);

                    if (encounter == "")
                    {
                        if (lstencounter[0].Date_of_Service != DateTime.MinValue)
                        {
                            //Jira #CAP-630
                            //lsvirals = vitalmngr.GetPatientListByDos(lstencounter[0].Date_of_Service.AddYears(-1).ToString("yyyy-MM-dd HH:mm"), lstencounter[0].Date_of_Service.ToString("yyyy-MM-dd HH:mm"), lstencounter[0].Human_ID);
                            lsvirals = vitalmngr.GetPatientListByDos(lstencounter[0].Date_of_Service.AddYears(-1).ToString("yyyy-MM-dd HH:mm"), lstencounter[0].Date_of_Service.ToString("yyyy-MM-dd HH:mm"), lstencounter[0].Human_ID, ClientSession.EncounterId);

                            hdnBPValue.Value = "";
                            for (int i = 0; i < lsvirals.Count; i++)
                            {
                                if (i == 0)
                                    hdnBPValue.Value = lsvirals[i].Loinc_Observation + ":" + lsvirals[i].Value;
                                else
                                    hdnBPValue.Value = hdnBPValue.Value + "|" + lsvirals[i].Loinc_Observation + ":" + lsvirals[i].Value;
                            }
                        }
                    }
                    else
                    {
                        hdnBPValue.Value = "I10";
                    }

                }


            }

            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }
            if (Session["objList"] != null)
            {
                objList = (IList<object>)Session["objList"];
            }
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }

            IList<PercentileLookUp> percentileListForHC = new List<PercentileLookUp>();
            IList<PercentileLookUp> percentileListForBMI = new List<PercentileLookUp>();


            percentileIncrement = humanAgeInMonths - Convert.ToInt16(humanAgeInMonths + 0.5) + 0.5;


            if (objList != null && objList.Count > 0)
            {
                IList<PercentileLookUp> perList = null;
                if (objList[0] != null && objList[0].GetType().ToString().ToUpper().Contains("MAPVITALSPHYSICIAN"))
                    mapVitalPhyList = (IList<MapVitalsPhysician>)objList[0];
                if (objList[1] != null && objList[1].GetType().ToString().ToUpper().Contains("DYNAMICSCREEN"))
                    dynamicScreenList = (IList<DynamicScreen>)objList[1];
                if (objList[3] != null && objList[3].GetType().ToString().ToUpper().Contains("PERCENTILELOOKUP"))
                    perList = (IList<PercentileLookUp>)objList[3];
                if (objList[2] != null && objList[2].GetType().ToString().ToUpper().Contains("MASTERVITALS"))
                    mastervitalslist = (IList<MasterVitals>)objList[2];

                if (perList != null && perList.Count > 0)
                {
                    percentileListForBMI = (from o in perList where o.Category == "BMI-AGE" select o).ToList<PercentileLookUp>();
                    percentileListForHC = (from o in perList where o.Category == "HC-AGE" select o).ToList<PercentileLookUp>();
                }

            }



            //IList<StaticLookup> iFieldLookupList = staticLookupManager.getStaticLookupByFieldName("HEIGHT INCH MAXIMUM VALUE");
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                IList<StaticLookup> iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "HEIGHT INCH MAXIMUM VALUE").ToList();
                if (iFieldLookupList != null && iFieldLookupList.Count > 0)
                {
                    string[] str = iFieldLookupList.First().Value.Split('-');
                    if (str.Length == 2)
                    {
                        InchMinValue = Convert.ToDecimal(str[0]);
                        InchMaxValue = Convert.ToDecimal(str[1]);
                    }
                }
            }

            //if (!Page.IsPostBack )
            //    pnlVitals.Controls.Clear();
            Session["dynamicScreenList"] = dynamicScreenList;
            Session["mastervitalslist"] = mastervitalslist;
            Session["mapVitalPhyList"] = mapVitalPhyList;
            Session["percentileListForBMI"] = percentileListForBMI;
            Session["percentileListForHC"] = percentileListForHC;
            Session["percentileIncrement"] = percentileIncrement;


            if (openingfrom == "Menu")
            {
                IList<DynamicScreen> Removelst = new List<DynamicScreen>();
                for (int i = 0; i < dynamicScreenList.Count; i++)
                {
                    if (dynamicScreenList[i].Control_Name_Thin_Client.Contains("Second"))
                    {
                        Removelst.Add(dynamicScreenList[i]);
                    }
                }
                if (Removelst.Count > 0)
                {
                    dynamicScreenList = dynamicScreenList.Except(Removelst).ToList<DynamicScreen>();
                    Session["dynamicScreenList"] = dynamicScreenList;
                }

                pnlVitals.Controls.Clear();
                //grdPastVitals.Columns.Clear();
                CreateControls(dynamicScreenList);
                if (Session["objVitalDTO"] != null)
                {
                    objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
                }
                if (!IsPostBack)
                {
                    PastVitals(objVitalDTO.VitalsList);
                }
                pnlVitals.Style.Add("height", "320px");
                //btnSave.Text = "Add";
                //ClientSession.EncounterId = 0;

                GridDiv.Visible = true;
                BtnClose1.Style.Add("display", "block");
            }
            else if (openingfrom == "ChildScreen")
            {
                IList<DynamicScreen> Removelst = new List<DynamicScreen>();
                for (int i = 0; i < dynamicScreenList.Count; i++)
                {
                    if (dynamicScreenList[i].Control_Name_Thin_Client.Contains("Second"))
                    {
                        Removelst.Add(dynamicScreenList[i]);
                    }
                }
                if (Removelst.Count > 0)
                {
                    //dynamicScreenList = dynamicScreenList.Except(Removelst).ToList<DynamicScreen>();

                    DataSet dsGetVitals = new DataSet();
                    if (objVitalDTO.dsLatestResults.Tables.Count > 0)
                    {
                        dsGetVitals = objVitalDTO.dsLatestResults;
                        if (dsGetVitals.Tables != null)
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            dt = dsGetVitals.Tables[0];
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Loinc_observation.Trim() == string.Empty)
                                {
                                    Loinc_observation = dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                                }
                                else
                                {
                                    Loinc_observation += ", " + dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                                }
                            }
                        }
                    }
                    Session["Loinc_observation"] = Loinc_observation;
                    hdnLabResults.Value = Loinc_observation;
                    if (!IsPostBack)
                        IsLoad = true;
                    if (!Page.IsPostBack)
                    {
                        pnlVitals.Controls.Clear();

                        CreateControls(Removelst);


                    }
                    if (!IsPostBack)
                        EditCellClickforVitals(false);
                    GridDiv.Visible = false;
                    BtnClose1.Style.Add("display", "none");
                }

            }
            else
            {
                DataSet dsGetVitals = new DataSet();
                if (objVitalDTO != null && objVitalDTO.dsLatestResults != null && objVitalDTO.dsLatestResults.Tables != null && objVitalDTO.dsLatestResults.Tables.Count > 0)
                {
                    dsGetVitals = objVitalDTO.dsLatestResults;

                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt = dsGetVitals.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Loinc_observation.Trim() == string.Empty)
                        {
                            Loinc_observation = dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                        }
                        else
                        {
                            Loinc_observation += ", " + dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                        }
                    }

                }
                Session["Loinc_observation"] = Loinc_observation;
                hdnLabResults.Value = Loinc_observation;
                if (!IsPostBack)
                    IsLoad = true;
                if (!Page.IsPostBack)
                {
                    CreateControls(dynamicScreenList);
                    EditCellClickforVitals(false);

                }


                GridDiv.Visible = false;
                BtnClose1.Style.Add("display", "none");
            }

            if (!IsPostBack)
                EnableDisbaleSave(false);

            pnlButtons.Visible = true;
            if (!IsPostBack)
            {
                if (openingfrom != "Menu")
                {
                    SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                    objSecurity.ApplyUserPermissions(this.Page);
                    btnSaveVitals.Disabled = true; ;
                    if (string.Compare(Convert.ToString(Session["Client_FromTab"]), "VITALS", true) == 0)
                    {
                        btnSaveVitals.Disabled = false;
                        Session["Client_FromTab"] = null;
                    }
                }
            }
            if (openingfrom == "Menu")
                divLoading.Style.Add("display", "none");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);

            // PastVitals(objVitalDTO.VitalsList);
            //string date;


            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        public void PastVitals(IList<PatientResults> vitalList)
        {
            ulong GroupID = 0;
            PatientResultsDTO objVitalDTO = null;
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();

            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }

            if (vitalList == null)
            {
                if (Session["vitalList"] != null)
                {
                    vitalList = (IList<PatientResults>)Session["vitalList"];
                }
            }
            if (dtTable.Rows.Count > 0)
                dtTable.Rows.Clear();


            if (vitalList != null)
            {
                if (vitalList.Count > 0)
                {

                    var grpIds = (from vitalGrpId in vitalList orderby vitalGrpId.Modified_Date_And_Time descending select new { vitalGrpId.Vitals_Group_ID }).Distinct();
                    if (!dtTable.Columns.Contains("Entered Date Time"))
                    {
                        dtTable.Columns.Add("Entered Date Time");
                        if (dtTable.Columns.Count > 2)
                            dtTable.Columns["Entered Date Time"].SetOrdinal(2);
                    }
                    if (!dtTable.Columns.Contains("GroupID"))
                        dtTable.Columns.Add("GroupID");
                    if (!dtTable.Columns.Contains("EncounterID"))
                        dtTable.Columns.Add("EncounterID");


                    foreach (var item in grpIds)
                    {
                        dr = dtTable.NewRow();

                        dr["GroupID"] = item.Vitals_Group_ID;

                        var vitalGrpList = (from vital in vitalList where vital.Vitals_Group_ID == item.Vitals_Group_ID select new { vital });
                        foreach (var vitalObj in vitalGrpList)
                        {
                            dr["EncounterID"] = vitalObj.vital.Encounter_ID;
                            dr["Entered Date Time"] = UtilityManager.ConvertToLocal(vitalObj.vital.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt");

                            if (vitalObj.vital.Loinc_Observation.Contains("Second"))
                            {
                                vitalObj.vital.Loinc_Observation = vitalObj.vital.Loinc_Observation.Replace("Second", "");
                            }
                            try
                            {
                                //DataColumn grddataCol = dtTable.Columns.Contains(vitalObj.vital.Loinc_Observation);
                                if (dtTable.Columns.Contains(vitalObj.vital.Loinc_Observation))
                                {
                                    // GridViewDataColumn grddataCol = grdPastVitals.Columns.FindByUniqueName(vitalObj.vital.Loinc_Observation);
                                    //if (grddataCol != null)
                                    //{
                                    if (vitalObj.vital.Value != string.Empty)
                                    {
                                        if (htRetriveUnitMthds.Contains(vitalObj.vital.Loinc_Observation))
                                        {
                                            if (Convert.ToDateTime(dr["Entered Date Time"]) == Convert.ToDateTime(UtilityManager.ConvertToLocal(vitalObj.vital.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")))
                                            {
                                                if (dr[vitalObj.vital.Loinc_Observation] == DBNull.Value)
                                                {
                                                    string sValue = string.Empty;
                                                    if (vitalObj.vital.Value.Contains("'") == true && vitalObj.vital.Value.Contains("''") == true)
                                                        sValue = vitalObj.vital.Value;
                                                    else
                                                        sValue = ConversionOnRetrieval(vitalObj.vital.Loinc_Observation, vitalObj.vital.Value);

                                                    dr[vitalObj.vital.Loinc_Observation] = sValue;
                                                }
                                                else
                                                {
                                                    string sValue = string.Empty;
                                                    if (vitalObj.vital.Value.Contains("'") == true && vitalObj.vital.Value.Contains("''") == true)
                                                        sValue = vitalObj.vital.Value;
                                                    else
                                                        sValue = ConversionOnRetrieval(vitalObj.vital.Loinc_Observation, vitalObj.vital.Value);
                                                    dr[vitalObj.vital.Loinc_Observation] += ", " + sValue;
                                                }
                                            }
                                            else
                                            {
                                                string sValue = string.Empty;
                                                if (vitalObj.vital.Value.Contains("'") == true && vitalObj.vital.Value.Contains("''") == true)
                                                    sValue = vitalObj.vital.Value;
                                                else
                                                    sValue = ConversionOnRetrieval(vitalObj.vital.Loinc_Observation, vitalObj.vital.Value);

                                                dr[vitalObj.vital.Loinc_Observation] = sValue;
                                            }



                                            //if (vitalObj.vital.Loinc_Observation.ToUpper().Contains("STATUS"))
                                            //{
                                            //    grddataCol.TextAlignment = ContentAlignment.MiddleLeft;
                                            //}
                                        }
                                        else
                                        {
                                            if (Convert.ToDateTime(dr["Entered Date Time"]) == Convert.ToDateTime(UtilityManager.ConvertToLocal(vitalObj.vital.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")))
                                            {
                                                if (dr[vitalObj.vital.Loinc_Observation] == DBNull.Value)
                                                {
                                                    dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Value;
                                                }
                                                else
                                                {
                                                    dr[vitalObj.vital.Loinc_Observation] += ", " + vitalObj.vital.Value;
                                                }
                                            }
                                            else
                                            {
                                                dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Value;
                                            }
                                            //if (vitalObj.vital.Loinc_Observation.ToUpper().Contains("STATUS"))
                                            //{
                                            //    grddataCol.TextAlignment = ContentAlignment.MiddleLeft;
                                            //}
                                        }
                                    }
                                    else
                                    {
                                        //dr[vitalObj.vital.Loinc_Observation].Value = vitalObj.vital.Reason_For_Refuse;
                                        if (Convert.ToDateTime(dr["Entered Date Time"]) == Convert.ToDateTime(UtilityManager.ConvertToLocal(vitalObj.vital.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")))
                                        {
                                            if (dr[vitalObj.vital.Loinc_Observation] == DBNull.Value)
                                            {
                                                dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Value;
                                            }
                                            else
                                            {
                                                dr[vitalObj.vital.Loinc_Observation] += ", " + vitalObj.vital.Value;
                                            }
                                        }
                                        else
                                        {
                                            dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Value;
                                        }
                                    }

                                }

                            }
                            catch
                            {
                                if ((vitalObj.vital.Loinc_Observation.ToUpper().Contains("BP") && vitalObj.vital.Loinc_Observation.ToUpper().Contains("LOCATION")) == false)
                                {
                                    dtTable.Columns.Add(vitalObj.vital.Loinc_Observation);
                                    //GridViewTextBoxColumn col = new GridViewTextBoxColumn();
                                    //col.UniqueName = vitalObj.vital.Loinc_Observation;
                                    //col.HeaderText = vitalObj.vital.Loinc_Observation;
                                    //col.Width = col.HeaderText.Length * 8;
                                    //col.WrapText = true;
                                    //col.TextAlignment = ContentAlignment.MiddleCenter;
                                    //grdPastVitals.Columns.Insert(grdPastVitals.ColumnCount, col);
                                    if (vitalObj.vital.Value != string.Empty)
                                    {
                                        if (htRetriveUnitMthds.Contains(vitalObj.vital.Loinc_Observation))
                                        {
                                            if (Convert.ToDateTime(dr["Entered Date Time"]) == Convert.ToDateTime(UtilityManager.ConvertToLocal(vitalObj.vital.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")))
                                            {
                                                if (dr[vitalObj.vital.Loinc_Observation] == DBNull.Value)
                                                {
                                                    dr[vitalObj.vital.Loinc_Observation] = ConversionOnRetrieval(vitalObj.vital.Loinc_Observation, vitalObj.vital.Value);
                                                }
                                                else
                                                {
                                                    dr[vitalObj.vital.Loinc_Observation] += ", " + ConversionOnRetrieval(vitalObj.vital.Loinc_Observation, vitalObj.vital.Value);
                                                }
                                            }
                                            else
                                            {
                                                dr[vitalObj.vital.Loinc_Observation] = ConversionOnRetrieval(vitalObj.vital.Loinc_Observation, vitalObj.vital.Value);
                                            }
                                        }
                                        else
                                        {
                                            // dr[vitalObj.vital.Loinc_Observation].Value = vitalObj.vital.Value;

                                            if (Convert.ToDateTime(dr["Entered Date Time"]) == Convert.ToDateTime(UtilityManager.ConvertToLocal(vitalObj.vital.Created_Date_And_Time).ToString("dd-MMM-yyyy hh:mm tt")))
                                            {
                                                if (dr[vitalObj.vital.Loinc_Observation] == DBNull.Value)
                                                {
                                                    dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Value;
                                                }
                                                else
                                                {
                                                    dr[vitalObj.vital.Loinc_Observation] += ", " + vitalObj.vital.Value;
                                                }
                                            }
                                            else
                                            {
                                                dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Value;
                                            }
                                        }
                                    }
                                    else
                                        dr[vitalObj.vital.Loinc_Observation] = vitalObj.vital.Notes;



                                }

                            }
                        }
                        dtTable.Rows.Add(dr);
                    }
                    // grdPastVitals.AutoScroll = true;
                }
            }
            GroupID = objVitalDTO.MaxGroupId;
            Session["GroupID"] = GroupID;
            //if (ds.Tables.Count == 0)
            //    ds.Tables.Add(dtTable);
            // if (Session["PastVitalGrid"]!=null)
            Session["PastVitalGrid"] = dtTable;
            //grdPastVitals.Columns.Clear();
            string[] columnNames = (from dc in dtTable.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();

            // ScriptManager.RegisterStartupScript(this, this.GetType(), "LoadHeader", "LoadTableHeader('"+columnNames+"');", true);

            // hdnHeader.Value = columnNames;
            if (dtTable.Rows.Count > 0)
            {
                gvEnterDetails.DataSource = dtTable;
                gvEnterDetails.DataBind();

            }
            else
            {
                dr = dtTable.NewRow();

                dtTable.Columns.Add("EncounterID");
                dr["EncounterID"] = "";
                dtTable.Columns.Add("GroupID");
                dr["GroupID"] = "";
                dtTable.Columns.Add("Entered Date Time");
                dr["Entered Date Time"] = "";

                dtTable.Rows.Add(dr);
                gvEnterDetails.DataSource = dtTable;

                gvEnterDetails.DataBind();

                gvEnterDetails.Rows[0].Visible = false;
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvEnterDetails.ClientID + "', 200, 1140 , 40 ,true); </script>", false);
            int grdcount = gvEnterDetails.Columns.Count;
            for (int i = gvEnterDetails.Columns.Count; i >= 0; i--)
            {
                if (i != 0 && i != -1)
                {
                    if (gvEnterDetails.Columns[i - 1].HeaderText == "")
                        gvEnterDetails.Columns.RemoveAt(i - 1);
                }

            }
            ArrayList vitalArray = new ArrayList();
            if (Session["vitalArray"] != null)
            {
                vitalArray = (ArrayList)Session["vitalArray"];
            }
            //if (openingfrom == "Menu")
            //{
            int columnCount = 0;
            int columncountgrd = 0;

            if (Session["columnCount"] == null)
                columnCount = dtTable.Columns.Count;
            else
                columnCount = (int)Session["columnCount"];

            if (Session["columncountgrd"] == null)
                columncountgrd = gvEnterDetails.Columns.Count;
            else
                columncountgrd = (int)Session["columncountgrd"];

            Session["columnCount"] = columnCount;
            Session["columncountgrd"] = columncountgrd;

            //if ((DataTable)ViewState["dtTable"] != null)
            //    dtTable = (DataTable)ViewState["dtTable"];

            // dtTable.Columns.Clear();

            foreach (object obj in vitalArray)
            {
                string vital = obj.ToString();
                if ((obj.ToString().ToUpper().Contains("BP") && (obj.ToString().ToUpper().Contains("LEFT") || obj.ToString().ToUpper().Contains("RIGHT"))) == false)
                {
                    if (!vital.Contains("StatusRemove"))
                    {
                        if (!dtTable.Columns.Contains(vital))
                        {
                            dtTable.Columns.Add(vital);
                            columnCount++;
                        }
                    }

                    //GridBoundColumn col = new GridBoundColumn();
                    //col.HeaderText = vital;
                    //col.DataField = vital;
                    //col.ItemStyle.Wrap = true;
                    //col.HeaderStyle.Wrap = false;
                    //col.ItemStyle.Width = col.HeaderText.Length * 7 + 100;

                    //if (!vital.Contains("StatusRemove"))
                    //{
                    //    try
                    //    {
                    //        grdPastVitals.MasterTableView.GetColumn(col.HeaderText);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        grdPastVitals.Columns.Insert(columncountgrd, col);
                    //        columncountgrd++;
                    //    }
                    //}


                }

            }

            //}

            Session["dtTable"] = dtTable;
            if (gvEnterDetails.Columns.Count > 0)
            {
                for (int i = 0; i < gvEnterDetails.Columns.Count; i++)
                {
                    if (gvEnterDetails.Columns[i].HeaderText.ToUpper() == "ENCOUNTERID")
                    {
                        //grdPastVitals.Columns[i].Visible = false;
                        //grdPastVitals.Columns[i].h = false;
                    }
                    else if (gvEnterDetails.Columns[i].HeaderText.ToUpper() == "GROUPID")
                    {
                        gvEnterDetails.Columns[i].Visible = false;
                    }
                }
            }

        }

        public void CreateControls(IList<DynamicScreen> dynScnlist)
        {
            string mandatoryFeidId = "";
            bool formLoad = false;
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();
            mandatoryCtrls = new List<Control>();
            ArrayList vitalArray = new ArrayList();
            DateTime BirthDate = DateTime.MinValue;
            int humanAgeInYears = 0;
            formLoad = true;
            //int tab = 1;
            Table objTable = new Table();
            objTable.BackColor = Color.White;
            IList<PatientPane> lstPatientPane = new List<PatientPane>();
            TableCell tc = null;
            if (Session["BirthDate"] != null)
                BirthDate = Convert.ToDateTime(Session["BirthDate"]);
            humanAgeInYears = UtilityManager.CalculateAge(BirthDate);
            IList<MapVitalsPhysician> mapVitalPhyList = new List<MapVitalsPhysician>();
            if (Session["mapVitalPhyList"] != null)
                mapVitalPhyList = (IList<MapVitalsPhysician>)Session["mapVitalPhyList"];
            string Loinc_observation = string.Empty;
            if (Session["Loinc_observation"] != null)
            {
                Loinc_observation = Session["Loinc_observation"].ToString();
                hdnLabResults.Value = Loinc_observation;
            }

            string openingfrom = string.Empty;
            openingfrom = Request["openingfrom"].ToString();
            if (openingfrom == "ChildScreen")
            {
                objTable.Width = 1200;
            }
            else
            {
                objTable.Width = 1400;
            }
            objTable.ID = "tblVitalControls";
            string MandValdInjection = string.Empty;
            IList<string> AvoidDuplicate = new List<string>();
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            if (Session["BPUpperLimit"] == null || Session["BPLowerLimit"] == null)
            {
                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                {

                    IList<StaticLookup> defValues = StaticLookUpList.Where(q => q.Field_Name == "BP DEFAULT VALUES").ToList();
                    //IList<StaticLookup> defValues = staticLookupManager.getStaticLookupByFieldName("BP DEFAULT VALUES");
                    if (defValues.Count > 0)
                    {
                        Session["BPUpperLimit"] = defValues[0].Value.ToString();
                        Session["BPLowerLimit"] = defValues[1].Value.ToString();
                    }
                }
            }
            string GeneralObjs = string.Empty;
            //System.Web.HttpContext.Current.Response.Flush();
            //  var val = (from d in dynScnlist where d.Control_Type.ToUpper() == "COMBOBOX" && (!d.Control_Name_Thin_Client.ToString().ToUpper().StartsWith("PATIENT-REASON-REFUSED")) select d.Control_Name_Thin_Client.ToString());

            var val = (from d in dynScnlist where d.Control_Type.ToUpper() == "COMBOBOX" select d.Control_Name_Thin_Client.ToString());

            string[] sFieldsCombobox = ((IEnumerable)val).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
            Array.Resize(ref sFieldsCombobox, sFieldsCombobox.Length + 1);
            // sFieldsCombobox[sFieldsCombobox.Length - 1] = "PATIENT-REASON-REFUSED";
            IList<StaticLookup> ilstLookup = staticLookupManager.getStaticLookupByFieldName(sFieldsCombobox);
            ilstLookupRefuse = staticLookupManager.getStaticLookupByFieldNamelikeSorder("REASON-NOT-PERFORMED");
            string controlname = "";
            foreach (MapVitalsPhysician objMapVital in mapVitalPhyList)
            {
                controlname = "";
                TableRow tr = new TableRow();
                //tc = new TableCell();
                IList<DynamicScreen> list = (from obj in dynScnlist where obj.Master_Vitals_ID == objMapVital.Master_Vitals_ID select obj).ToList<DynamicScreen>();
                if (list != null)
                {
                    if (objMapVital.Age_Condition_In_Years != "ALL")
                    {
                        if (objMapVital.Age_Condition_In_Years.Contains("<"))
                        {
                            if (!(Convert.ToDecimal(humanAgeInYears) < Convert.ToDecimal(objMapVital.Age_Condition_In_Years.Replace('<', ' ').Trim())))
                            {
                                continue;
                            }
                        }
                        else if (objMapVital.Age_Condition_In_Years.Contains(">"))
                        {
                            if (!(Convert.ToDecimal(humanAgeInYears) >= Convert.ToDecimal(objMapVital.Age_Condition_In_Years.Replace('>', ' ').Trim())))
                            {
                                continue;
                            }
                        }
                        else if (objMapVital.Age_Condition_In_Years.Contains("-"))
                        {
                            string[] splitCon = objMapVital.Age_Condition_In_Years.Split('-');
                            if (splitCon != null && splitCon.Length == 2)
                            {
                                if (!((Convert.ToDecimal(humanAgeInYears) >= Convert.ToDecimal(splitCon[0])) && (Convert.ToDecimal(humanAgeInYears) <= Convert.ToInt16(splitCon[1]))))
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    if (objMapVital.Sex.Trim() != string.Empty)
                    {
                        if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                        {
                            if (objMapVital.Sex.ToUpper() != ClientSession.PatientPaneList[0].Sex.ToUpper())
                            {
                                continue;
                            }
                        }
                    }
                    //HtmlInputText grwLblVitalName = null;
                    //RadTextBox txtbox = null;
                    HtmlInputText txtbox = null;
                    HtmlInputText label = null;
                    HtmlSelect cboReasonForRefuse = null;

                    ImageButton picBox = null;
                    IList<ImageButton> picList = new List<ImageButton>();
                    //for (int k = 0; k < 8; k++)
                    //{
                    tc = new TableCell();
                    tc.Width = new Unit(570, UnitType.Pixel);
                    // tr.Cells.Add(tc);
                    tr.Style.Add("width", "100%");
                    objTable.Rows.Add(tr);
                    // }

                    IList<DynamicScreen> dsCombobox = list.Where(d => d.Control_Type.ToUpper() == "COMBOBOX").ToList();
                    String[] sFieldNames = new String[dsCombobox.Count];
                    int i = 0;
                    foreach (DynamicScreen item in dsCombobox)
                    {
                        sFieldNames[i] = item.Control_Name_Thin_Client.Trim().ToUpper();
                        i++;
                    }
                    IList<StaticLookup> lstComboBox = new List<StaticLookup>();

                    if (sFieldNames.Length == 1)
                    {
                        lstComboBox = (from p in ilstLookup where (sFieldNames[0].ToString().Contains(p.Field_Name)) select p).ToList();
                        //lstComboBox = staticLookupManager.getStaticLookupByFieldName(sFieldNames)
                    }
                    else if (sFieldNames.Length > 1)
                    {
                        lstComboBox = (from p in ilstLookup where (sFieldNames[0].ToString().Contains(p.Field_Name) || sFieldNames[1].ToString().Contains(p.Field_Name)) select p).ToList();
                    }


                    foreach (DynamicScreen DsObj in list)
                    {

                        switch (DsObj.Control_Type.ToUpper())
                        {
                            case "VITALLABEL":

                                if (DsObj.Control_Name_Thin_Client.Trim() == "lblVitalLatest Lab Results")
                                {
                                    txtbox = null;
                                    label = null;
                                    cboReasonForRefuse = null;
                                    picBox = null;
                                    HtmlInputText grwLblVitalName = new HtmlInputText();
                                    grwLblVitalName.ID = DsObj.Control_Name_Thin_Client.Trim();
                                    grwLblVitalName.EnableViewState = false;
                                    if (ClientSession.EncounterId != 0 && openingfrom != "Menu")
                                    {
                                        if (DsObj.Mandatory.ToUpper() == "Y")
                                        {

                                            // commented common style sheet added

                                            //grwLblVitalName.Style.Add("color", "Red");
                                            //grwLblVitalName.Style.Add("font-weight", "bold");

                                            if (DsObj.Control_Name_Thin_Client.Contains("Bp") && humanAgeInYears < 3)
                                            {
                                                // grwLblVitalName.Style.Add("color", "Black");
                                                DsObj.Mandatory = "";
                                            }
                                        }


                                        //else
                                        //{
                                        grwLblVitalName.Style.Add("color", "Black");
                                        if (Loinc_observation != string.Empty)
                                            grwLblVitalName.Value = objMapVital.Vital_Text;
                                        grwLblVitalName.Style.Add("font-weight", "bold");
                                        //}
                                    }

                                    // grwLblVitalName.Style.Add("background-color", "White");
                                    tc.ColumnSpan = 0;
                                    // grwLblVitalName.Style.Add("font-family", "Serif");
                                    grwLblVitalName.Style.Add("Width", DsObj.Column_Span_Thin_Client + "px");
                                    // grwLblVitalName.Style.Add("font-size", "small");
                                    grwLblVitalName.Attributes.Add("readonly", "readonly");
                                    // grwLblVitalName.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
                                    grwLblVitalName.Attributes.Add("disabled", "disabled");
                                    grwLblVitalName.Attributes.Add("Mand", "No");
                                    tc.Controls.Add(grwLblVitalName);
                                    tr.Cells.Add(tc);
                                    objTable.Rows.Add(tr);
                                    if (grwLblVitalName.Value != string.Empty)
                                        aryLabelFilled.Add(grwLblVitalName.ID.ToString());
                                    grwLblVitalName.Dispose();


                                }
                                else if (DsObj.Control_Name_Thin_Client.Trim() != "lblVitalLatest Lab Results")
                                {
                                    controlname = DsObj.Control_Name_Thin_Client.Replace(" ", "").Replace("/", "").Replace("-", "");

                                    //  HtmlInputText grwLblVitalName = new HtmlInputText();
                                    Label grwLblVitalName = new Label();
                                    txtbox = null;
                                    label = null;
                                    cboReasonForRefuse = null;
                                    picBox = null;
                                    // grwLblVitalName = new HtmlInputText();
                                    grwLblVitalName.EnableViewState = false;
                                    grwLblVitalName.ID = DsObj.Control_Name_Thin_Client.Trim().Replace(" ", "").Replace("/", "").Replace("-", ""); ;
                                    if (ClientSession.EncounterId != 0 && openingfrom != "Menu")
                                    {
                                        if (DsObj.Is_Macra_Field.ToUpper() == "Y")
                                        {
                                            // grwLblVitalName.Style.Add("color", "#6504d0");
                                            grwLblVitalName.Attributes.Add("Mand", "Macra");


                                        }

                                        if (DsObj.Mandatory.ToUpper() == "Y")
                                        {

                                            // commented common style sheet added
                                            // grwLblVitalName.Style.Add("color", "Red");
                                            grwLblVitalName.Attributes.Add("Mand", "Yes");//by valli
                                            grwLblVitalName.Text = objMapVital.Vital_Text + "*";

                                            if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("BP") && humanAgeInYears < 3)
                                            {
                                                grwLblVitalName.Style.Add("color", "Black");
                                                grwLblVitalName.Text = objMapVital.Vital_Text;
                                                DsObj.Mandatory = "";
                                            }
                                        }

                                        else
                                        {
                                            // grwLblVitalName.Style.Add("color", "Black");
                                            grwLblVitalName.Text = objMapVital.Vital_Text;
                                            if (DsObj.Is_Macra_Field.ToUpper() != "Y")
                                                grwLblVitalName.Attributes.Add("Mand", "No");
                                        }

                                        grwLblVitalName.Style.Add("display", "inline-block");

                                        // commented common style sheet added
                                        if (DsObj.Display_Text.ToUpper() == "BMI")
                                        {
                                            grwLblVitalName.Text = objMapVital.Vital_Text + "*";
                                            grwLblVitalName.Attributes.Add("Mand", "Yes");//by valli
                                            //  grwLblVitalName.Style.Add("color", "Red");
                                        }
                                    }
                                    else
                                    {
                                        // grwLblVitalName.Style.Add("color", "Black");
                                        grwLblVitalName.Text = objMapVital.Vital_Text;
                                        grwLblVitalName.Attributes.Add("Mand", "No");//byvalli
                                        grwLblVitalName.Style.Add("display", "inline-block");
                                    }

                                    if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("BP"))
                                    {
                                        tr.ID = "tr" + DsObj.Control_Name_Thin_Client.Replace("lblVital", "").Replace(" ", "").Replace("/", "").Replace("-", "");
                                    }


                                    if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("BP") && DsObj.Control_Name_Thin_Client.ToUpper().Contains("SITTING") && DsObj.Control_Name_Thin_Client.ToUpper().Contains("SECOND"))
                                    {
                                        var spanUp = new HtmlGenericControl("span");

                                        spanUp.Attributes["class"] = "glyphicon glyphicon-plus-sign";
                                        spanUp.Attributes["Id"] = "spanplus";
                                        spanUp.Attributes["onclick"] = "test()";
                                        spanUp.Attributes["title"] = "Please click here to view more BP elements";
                                        spanUp.Style.Add("width", "10px");
                                        tc.Controls.Add(spanUp);
                                        // grwLblVitalName.Style.Add("Width", "103px");

                                    }
                                    //   grwLblVitalName.Style.Add("background-color", "White");
                                    //  grwLblVitalName.Style.Add("font-family", "Serif");
                                    if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("BP") && DsObj.Control_Name_Thin_Client.ToUpper().Contains("SITTING") && DsObj.Control_Name_Thin_Client.ToUpper().Contains("SECOND"))
                                    {
                                        grwLblVitalName.Style.Add("padding-left", "0px");
                                        grwLblVitalName.Style.Add("Width", Convert.ToInt32(DsObj.Column_Span_Thin_Client) + "px");
                                    }
                                    else
                                    {
                                        grwLblVitalName.Style.Add("padding-left", "7px");
                                        grwLblVitalName.Style.Add("Width", Convert.ToInt32(DsObj.Column_Span_Thin_Client) + 10 + "px");
                                    }
                                    // grwLblVitalName.Style.Add("font-size", "small");
                                    grwLblVitalName.Attributes.Add("readonly", "readonly");
                                    grwLblVitalName.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
                                    tc.ColumnSpan = 0;
                                    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                    grwLblVitalName.Attributes.Add("disabled", "disabled");
                                    tc.Controls.Add(grwLblVitalName);
                                    tr.Cells.Add(tc);

                                    // objTable.Rows.Add(tr);
                                    grwLblVitalName.Dispose();
                                }
                                break;
                            case "REASONCOMBOBOX":
                                cboReasonForRefuse = new HtmlSelect();
                                cboReasonForRefuse.ID = DsObj.Control_Name_Thin_Client.Trim();
                                cboReasonForRefuse.Style.Add("font-family", "Serif");
                                cboReasonForRefuse.Style.Add("font-size", "small");
                                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                {
                                    iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "REASON FOR REFUSE").ToList();
                                }
                                if (cboReasonForRefuse != null)
                                {
                                    cboReasonForRefuse.Items.Clear();
                                    cboReasonForRefuse.Items.Add(new ListItem(""));
                                    if (iFieldLookupList != null)
                                    {
                                        for (int j = 0; j < iFieldLookupList.Count; j++)
                                        {
                                            ListItem tempItem = new ListItem();
                                            tempItem.Text = iFieldLookupList[j].Value;
                                            cboReasonForRefuse.Items.Add(tempItem);
                                        }
                                    }
                                }
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(cboReasonForRefuse);
                                tr.Cells.Add(tc);
                                cboReasonForRefuse.Dispose();
                                break;

                            case "COMBOBOX":
                                cboReasonForRefuse = new HtmlSelect();
                                cboReasonForRefuse.ID = DsObj.Control_Name_Thin_Client.Trim();
                                cboReasonForRefuse.Style.Add("Width", DsObj.Column_Span_Thin_Client + "px");
                                cboReasonForRefuse.Style.Add("font-family", "Serif");
                                cboReasonForRefuse.Style.Add("font-size", "small");
                                // iFieldLookupList = lstComboBox.Where(d => DsObj.Control_Name_Thin_Client.Trim().ToUpper().StartsWith(d.Field_Name.Trim().ToUpper())).ToList<StaticLookup>(); if (cboReasonForRefuse != null)

                                //macra
                                //if (DsObj.Control_Name_Thin_Client.Trim().ToUpper().Contains("PATIENT-REASON-REFUSED"))
                                //{
                                //    iFieldLookupList = ilstLookupRefuse.Where(d => DsObj.Control_Name_Thin_Client.Trim().ToUpper() == d.Field_Name.Trim().ToUpper()).ToList<StaticLookup>();
                                //    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //    tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //}
                                //else

                                iFieldLookupList = lstComboBox.Where(d => DsObj.Control_Name_Thin_Client.Trim().ToUpper() == d.Field_Name.Trim().ToUpper()).OrderBy(a => a.Sort_Order).ToList<StaticLookup>();
                                if (cboReasonForRefuse != null)
                                {
                                    cboReasonForRefuse.Items.Clear();
                                    cboReasonForRefuse.Items.Add(new ListItem(""));
                                    if (iFieldLookupList != null)
                                    {
                                        for (int j = 0; j < iFieldLookupList.Count; j++)
                                        {
                                            ListItem tempItem = new ListItem();
                                            tempItem.Text = iFieldLookupList[j].Value;
                                            cboReasonForRefuse.Items.Add(tempItem);
                                        }
                                    }
                                }
                                cboReasonForRefuse.Attributes.Add("onkeypress", "EnableSave(event);");
                                cboReasonForRefuse.Attributes.Add("onchange", "EnableSave(event);");
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(cboReasonForRefuse);
                                tr.Cells.Add(tc);
                                cboReasonForRefuse.Dispose();
                                break;



                            case "TEXTBOX":
                                if (DsObj.Control_Content_Type.ToUpper() == "TEXT")
                                {
                                    //  txtbox.TextMode = TextBoxMode.MultiLine;
                                    HtmlTextArea txtarea = new HtmlTextArea();
                                    txtarea.ID = DsObj.Control_Name_Thin_Client.Trim();
                                    string FunctionName = DsObj.Control_Name_Thin_Client.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim() + "Validation";
                                    string injectingScript = string.Empty;
                                    txtarea.Style.Add(HtmlTextWriterStyle.Width, DsObj.Column_Span_Thin_Client + "px");
                                    txtarea.Style.Add(HtmlTextWriterStyle.Height, "30px");
                                    txtarea.Attributes.Add("height", "75px");
                                    txtarea.Attributes.Add("onkeyup", "EnableSave(event);");
                                    txtarea.Attributes.Add("onkeypress", "EnableSave(event);");
                                    txtarea.Attributes.Add("onchange", "EnableSave(event);");
                                    tc.Controls.Add(txtarea);
                                    txtarea.Style.Add("resize", "none");
                                    tr.Cells.Add(tc);
                                    txtarea.Dispose();
                                }
                                else
                                {

                                    txtbox = new HtmlInputText();
                                    txtbox.ID = DsObj.Control_Name_Thin_Client.Trim();
                                    string FunctionName = DsObj.Control_Name_Thin_Client.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim() + "Validation";
                                    string injectingScript = string.Empty;
                                    txtbox.Style.Add(HtmlTextWriterStyle.Width, DsObj.Column_Span_Thin_Client + "px");
                                    string perInc = "var percentileIncrement=";
                                    if (DsObj.Control_Name_Thin_Client.ToUpper() == "BMI")
                                    {
                                        if (Session["percentileIncrement"] != null)
                                        {

                                            perInc += Session["percentileIncrement"].ToString();
                                            perInc += ";";
                                        }
                                        else
                                        {
                                            perInc += (humanAgeInMonths - Convert.ToInt16(humanAgeInMonths + 0.5) + 0.5).ToString();
                                            // perInc += "";
                                            perInc += ";";
                                        }
                                        string BMIStatInject = "var S_BMI_Stat=";
                                        BMIStatInject += StaticLookupValueForClientSide("BMI STATUS");
                                        BMIStatInject += ";";

                                        string bmiPerStatus = string.Empty;
                                        bmiPerStatus = "var BMI_PERCENTILE_STATUS=";
                                        bmiPerStatus += StaticLookupValueForClientSide("BMI PERCENTILE STATUS");
                                        bmiPerStatus += ";";


                                        string bmiStatusColor = string.Empty;
                                        bmiStatusColor = "var BMI_STATUS_COLOR=";
                                        bmiStatusColor += StaticLookupValueForClientSide("VITALS LIMIT LEVEL");
                                        bmiStatusColor += ";";

                                        IList<PercentileLookUp> percentileListForBMI = new List<PercentileLookUp>();
                                        if (Session["percentileListForBMI"] != null)
                                            percentileListForBMI = (IList<PercentileLookUp>)Session["percentileListForBMI"];
                                        StringBuilder perListForBMI = new StringBuilder();
                                        if (percentileListForBMI.Count > 0)
                                        {
                                            perListForBMI.Append("[");
                                            foreach (PercentileLookUp obj in percentileListForBMI)
                                            {
                                                
                                                perListForBMI.Append("{'Age_In_Months':'" + obj.Age_In_Months + "','Category':'" + obj.Category + "','L':'" + obj.L + "','S':'" + obj.S + "','Sex':'" + obj.Sex + "','M':'" + obj.M + "'},");
                                            }
                                            perListForBMI = perListForBMI.Remove(perListForBMI.Length - 1, 1);
                                            perListForBMI.Append("]");
                                        }
                                        string perLookup = "var SpercentileIncrement=";
                                        if (percentileListForBMI.Count == 0)
                                        {
                                            perLookup += "null;";
                                        }
                                        else
                                        {
                                            perLookup += perListForBMI.ToString() + ";";
                                        }

                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ObjectsInjection", bmiPerStatus + perLookup + perInc + BMIStatInject + bmiStatusColor, true);
                                    }
                                    //CAP-781 Cannot read properties of null - frmVitals
                                    if (DsObj.Maximum_Value != string.Empty || DsObj.Minimum_Value != string.Empty)
                                    {
                                        txtbox.Attributes.Add("onblur", FunctionName + "(this, event)");
                                        injectingScript = "function " + FunctionName + "(sender, eventArgs){var passedBoundryValue=false;var uservalue=sender?.value;var min=" + DsObj.Minimum_Value + ";var max=" + DsObj.Maximum_Value + ";";

                                        if (DsObj.Control_Name_Thin_Client.ToUpper() == "HEIGHTINCH")
                                        {
                                            string InMinValue = string.Empty;
                                            string InMaxValue = string.Empty;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "HEIGHT INCH MAXIMUM VALUE").ToList();
                                            }
                                            if (iFieldLookupList != null && iFieldLookupList.Count > 0)
                                            {
                                                string[] str = iFieldLookupList.First().Value.Split('-');
                                                if (str.Length == 2)
                                                {
                                                    InMinValue = str[0];
                                                    InMaxValue = str[1];
                                                }
                                            }
                                            string InjectMinMax = "min=" + InMinValue + ";max=" + InMaxValue + ";";
                                            injectingScript += @"var ctrhtval =document.getElementById('Height');var InchFlag=false;if(ctrhtval?.value==''||ctrhtval?.value=='0'){InchFlag=true;";
                                            injectingScript += InjectMinMax + "}";

                                        }


                                        if (DsObj.Minimum_Value != string.Empty)
                                        {
                                            injectingScript += "if(uservalue ==''){passedBoundryValue = true;}else{ if( uservalue >=min){passedBoundryValue = true;}else{passedBoundryValue = false;}}";
                                        }
                                        if (DsObj.Maximum_Value != string.Empty)
                                        {
                                            injectingScript += "if(uservalue ==''){passedBoundryValue = true;}else{if( uservalue <=max && uservalue >=min){passedBoundryValue = true;}else{passedBoundryValue = false;}}";
                                        }

                                        //CAP-273 - fix focus function focus options
                                        if (DsObj.Control_Name_Thin_Client.ToUpper() == "HBA1C")
                                            injectingScript += "if(passedBoundryValue==false){ DisplayErrorMessage('200030','',min+'-'+max);document.getElementById('" + txtbox.ID + "').value='';document.getElementById('" + "HbA1C Status" + "').value='';document.getElementById('" + txtbox.ID + "').focus({focusVisible:true});return;}";
                                        else
                                            injectingScript += "if(passedBoundryValue==false){DisplayErrorMessage('200030','',min+'-'+max);document.getElementById('" + txtbox.ID + "').value='';document.getElementById('" + txtbox.ID + "').focus({focusVisible:true});return;}";

                                        if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("HEIGHT") || DsObj.Control_Name_Thin_Client.ToUpper().Contains("WEIGHT"))
                                        {
                                            injectingScript += "CalculateBMIOnFtInchAndLBS(document?.getElementById('Height')?.value,document?.getElementById('HeightInch')?.value,document?.getElementById('Weight')?.value);";
                                            injectingScript += "CheckIsBMIIsValid();";
                                            injectingScript += "SetBMIStatus(document?.getElementById('BMI')?.value);";

                                        }
                                        if (DsObj.Control_Name_Thin_Client.ToUpper() == "HEIGHTINCH")
                                        {
                                            string HeightInchConverstion = @"var ctrhtval =document.getElementById('Height');var InchFlag=false;if(ctrhtval.value==''||ctrhtval.value=='0'){InchFlag=true;}if(InchFlag){if(ctrhtval!=null){var convValue =	ConvertInchtoFeetInch(uservalue);if(convValue.length>0){document.getElementById('Height').value=convValue[0];document.getElementById('HeightInch').value=convValue[1];}}}";
                                            injectingScript += HeightInchConverstion;
                                            injectingScript += "CalculateBMIOnFtInchAndLBS(document?.getElementById('Height')?.value,document?.getElementById('HeightInch')?.value,document?.getElementById('Weight')?.value);";
                                            injectingScript += "CheckIsBMIIsValid();";
                                            injectingScript += "SetBMIStatus(document?.getElementById('BMI')?.value);";
                                        }

                                        else if (DsObj.Control_Name_Thin_Client.Contains("HbA1C"))
                                        {
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "HBA1C STATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "HBA1C_STATUS=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";

                                            }
                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }
                                            string ColorSetting = string.Empty;
                                            ColorSetting += "HBA1C_COLOR=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                ColorSetting += "{'description':'" + obj.Description + "','Value':'" + obj.Value + "'},";
                                            }
                                            ColorSetting = ColorSetting.Substring(0, ColorSetting.Length - 1);
                                            ColorSetting += "];";
                                            injectingScript += ColorSetting;

                                            if (!DsObj.Control_Name_Thin_Client.Contains("STATUSSecond"))
                                                injectingScript += "SetHBA1CStatus(sender?.value,sender.id,'HbA1C Status');";
                                            else
                                                injectingScript += "SetHBA1CStatus(sender?.value,sender.id,'HbA1C STATUSSecond');";

                                        }
                                        else if (DsObj.Control_Name_Thin_Client.Contains("Hgb"))
                                        {
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "HGB STATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "HGB_STATUS=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";

                                            }
                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }
                                            string ColorSetting = string.Empty;
                                            ColorSetting += "HGB_COLOR=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                ColorSetting += "{'description':'" + obj.Description + "','Value':'" + obj.Value + "'},";
                                            }
                                            ColorSetting = ColorSetting.Substring(0, ColorSetting.Length - 1);
                                            ColorSetting += "];";
                                            injectingScript += ColorSetting;

                                            if (!DsObj.Control_Name_Thin_Client.Contains("STATUSSecond"))
                                                injectingScript += "SetHGBStatus(sender?.value,sender.id,'Hgb Status');";
                                            else
                                                injectingScript += "SetHGBStatus(sender?.value,sender.id,'Hgb STATUSSecond');";

                                        }
                                        else if (DsObj.Control_Name_Thin_Client.Contains("Urine for Microalbumin"))
                                        {
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "URINE FOR MICROALBUMINSTATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "URINE_FOR_MICROALBUMINSTATUS=[";

                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                if (obj.Value.Contains("-"))
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                                else
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value + "'},";
                                            }

                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }

                                            if (!DsObj.Control_Name_Thin_Client.Contains("Second"))
                                                injectingScript += "SetUrineforMicroalbuminStatus(sender?.value,sender.id,'Urine for Microalbumin Status');";
                                        }
                                        else if (DsObj.Control_Name_Thin_Client.Contains("ABI Test"))
                                        {
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "ABI TESTSTATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "ABI_TESTSTATUS=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                if (obj.Value.Contains("-"))
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                                else if (obj.Value.Contains(">"))
                                                    StatusSetting += "{'description':'" + obj.Description + "','upperLimit':'" + obj.Value.Replace(">", "") + "'},";
                                                else if (obj.Value.Contains("<"))
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Replace("<", "") + "'},";
                                            }
                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }

                                            if (!DsObj.Control_Name_Thin_Client.Contains("Second"))
                                                injectingScript += "SetABITestStatus(sender?.value,sender?.id,'ABI Test Status');";
                                        }
                                        else if (DsObj.Control_Name_Thin_Client.Contains("eGFR"))
                                        {
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "GFR STATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "EGFR_STATUS=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                            }
                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }

                                            string ColorSetting = string.Empty;
                                            ColorSetting += "EGFR_COLOR=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                ColorSetting += "{'description':'" + obj.Description + "','Value':'" + obj.Value + "'},";
                                            }
                                            ColorSetting = ColorSetting.Substring(0, ColorSetting.Length - 1);
                                            ColorSetting += "];";
                                            injectingScript += ColorSetting;

                                            if (!DsObj.Control_Name_Thin_Client.Contains("Second"))
                                                injectingScript += "SetEGFRStatus(sender?.value,sender?.id,'eGFR Status');";
                                            else
                                                injectingScript += "SetEGFRStatus(sender?.value,sender?.id,'eGFR StatusSecond');";

                                        }
                                        else if (DsObj.Control_Name_Thin_Client.Contains("Blood Sugar-Fasting"))//For Bug id:46800
                                        //else if (DsObj.Control_Name_Thin_Client.Contains("BloodSugar-Fasting"))
                                        {
                                           
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "BLOOD SUGAR-FASTING STATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "BLOOD_SUGAR_FASTING_STATUS=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                if (obj.Value.Contains("-"))
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                                else
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value + "'},";
                                            }
                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }
                                            string ColorSetting = string.Empty;
                                            ColorSetting += "BLOOD_SUGAR_FASTING_COLOR=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                ColorSetting += "{'description':'" + obj.Description + "','Value':'" + obj.Value + "'},";
                                            }
                                            ColorSetting = ColorSetting.Substring(0, ColorSetting.Length - 1);
                                            ColorSetting += "];";
                                            injectingScript += ColorSetting;

                                            if (!DsObj.Control_Name_Thin_Client.Contains("Second"))
                                                injectingScript += "SetBloodSugarFastingStatus(sender?.value,sender.id,'Blood Sugar-Fasting Status');";
                                            else
                                                injectingScript += "SetBloodSugarFastingStatus(sender?.value,sender.id,'Blood Sugar-Fasting StatusSecond');";
                                        }
                                        else if (DsObj.Control_Name_Thin_Client.Contains("Blood Sugar-Post Prandial"))//For Bug id:46800
                                        // else if (DsObj.Control_Name_Thin_Client.Contains("BloodSugar-PostPrandial"))
                                        {
                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "BLOOD SUGAR-POST PRANDIAL STATUS").ToList();
                                            }
                                            string StatusSetting = string.Empty;
                                            StatusSetting += "BLOOD_SUGAR_POST_PRANDIAL_STATUS=[";
                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                if (obj.Value.Contains("-"))
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                                else
                                                    StatusSetting += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value + "'},";
                                            }
                                            StatusSetting = StatusSetting.Substring(0, StatusSetting.Length - 1);
                                            StatusSetting += "];";
                                            injectingScript += StatusSetting;

                                            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                            {
                                                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                            }

                                            string ColorSetting = string.Empty;

                                            ColorSetting += "BLOOD_SUGAR_POST_PRANDIAL_COLOR=[";

                                            foreach (StaticLookup obj in iFieldLookupList)
                                            {
                                                ColorSetting += "{'description':'" + obj.Description + "','Value':'" + obj.Value + "'},";
                                            }

                                            ColorSetting = ColorSetting.Substring(0, ColorSetting.Length - 1);
                                            ColorSetting += "];";
                                            injectingScript += ColorSetting;

                                            if (!DsObj.Control_Name_Thin_Client.Contains("Second"))
                                                injectingScript += "SetBloodSugarPostPrandialStatus(sender?.value,sender.id,'Blood Sugar-Post Prandial Status');";
                                            else
                                                injectingScript += "SetBloodSugarPostPrandialStatus(sender?.value,sender.id,'Blood Sugar-Post Prandial StatusSecond');";
                                        }
                                        injectingScript += "}";

                                        ScriptManager.RegisterStartupScript(this, this.GetType(), FunctionName, injectingScript, true);
                                    }

                                    txtbox.MaxLength = DsObj.Maximum_Length + 4;
                                    txtbox.Attributes.Add("onkeyup", "EnableSave(event);");
                                    txtbox.Attributes.Add("onkeypress", "EnableSave(event);");
                                    txtbox.Attributes.Add("onchange", "EnableSave(event);");
                                    txtbox.Style["text-align"] = "right";

                                    if (DsObj.Is_Editable == "N")
                                    {
                                        txtbox.Attributes.Add("readonly", "readonly");
                                        txtbox.Style.Add("ReadOnly", "true");
                                        // txtbox.Style.Add("border-color", "Black");

                                        txtbox.Attributes.Add("Editable", "No");
                                        // txtbox.Style.Add("background-color", "#9DCEFF");
                                    }
                                    else
                                    {
                                        txtbox.Attributes.Add("Editable", "Yes");
                                    }
                                    if (DsObj.Mandatory == "Y" && ClientSession.EncounterId != 0 && openingfrom != "Menu")
                                    {
                                        //   MandValdInjection += "if(document.getElementById('" + txtbox.ID + "').value!=undefined && document.getElementById('" + txtbox.ID + "').value=='' && document.getElementById('txtNotes" + DsObj.Control_Name_Thin_Client.Trim() + "_txtDLC').value==''){SaveUnsuccessful();DisplayErrorMessage('200031','','" + DsObj.Control_Name_Thin_Client.Trim() + "'); return false;}";
                                        mandatoryCtrls.Add(txtbox);
                                        mandatoryFeidId = mandatoryFeidId + "~" + txtbox.ID;
                                    }

                                    if (DsObj.Control_Content_Type.ToUpper() == "NUMERIC")
                                    {
                                        if (DsObj.Allow_Decimal == "Y")
                                        {
                                            txtbox.Attributes.Add("onkeypress", "return AllowNumbers('" + Events + "')");
                                        }
                                        else
                                        {
                                            txtbox.Attributes.Add("onkeypress", "return NotAllowdecimal('" + Events + "')");
                                        }
                                    }

                                    if (tc.Controls.Count >= 1)
                                    {
                                        tc.ColumnSpan = 0;
                                        tc.Controls.Add(new LiteralControl("&nbsp;"));
                                    }


                                    tc.Controls.Add(txtbox);
                                    tr.Cells.Add(tc);
                                    txtbox.Dispose();
                                }
                                break;

                            case "NOTESTEXTBOX":

                                TableCell tc1 = new TableCell();
                                CustomDLCNew userCtrl = (CustomDLCNew)LoadControl("~/UserControls/CustomDLCNew.ascx");
                                string SpintxtName = DsObj.Control_Name_Thin_Client;
                                if ((SpintxtName.Contains("-")) || (SpintxtName.Contains("/")))
                                {
                                    if (SpintxtName.Contains("-"))
                                    {
                                        SpintxtName = SpintxtName.Replace("-", "");
                                    }
                                    if (SpintxtName.Contains("/"))
                                    {
                                        SpintxtName = SpintxtName.Replace("/", "");
                                    }

                                    if (SpintxtName.Contains(" "))
                                    {
                                        SpintxtName = SpintxtName.Replace(" ", "");
                                    }
                                }
                                else
                                {
                                    if (SpintxtName.Contains(" "))
                                        SpintxtName = SpintxtName.Replace(" ", "");
                                }

                                userCtrl.ID = SpintxtName;
                                userCtrl.TextControlID = DsObj.Control_Name_Thin_Client.Replace("txtNotes", "");

                                userCtrl.TextboxHeight = new Unit("25px");
                                userCtrl.TextboxWidth = new Unit("300px");
                                userCtrl.ListboxHeight = new Unit("100px");
                                userCtrl.EnableViewState = true;
                                userCtrl.Value = "VITALS NOTES";
                                userCtrl.txtDLC.Attributes.Add("onkeyup", "EnableSave(event);");
                                userCtrl.txtDLC.Attributes.Add("onchange", "EnableSave(event);");

                                if (DsObj.Mandatory == "Y" && ClientSession.EncounterId != 0 && openingfrom != "Menu")
                                {
                                    if (!(DsObj.Control_Name_Thin_Client.ToUpper().Contains("BP") && humanAgeInYears < 3))
                                    {
                                        mandatoryCtrls.Add(userCtrl);
                                    }
                                }
                                if (DsObj.Is_Editable == "N")
                                {
                                    userCtrl.txtDLC.Attributes.Add("readonly", "readonly");
                                    userCtrl.txtDLC.Style.Add("ReadOnly", "true");
                                    // userCtrl.txtDLC.Style.Add("border-color", "Black");
                                    // userCtrl.txtDLC.Style.Add("background-color", "#9DCEFF");
                                    userCtrl.txtDLC.Attributes.Add("Editable", "No");
                                    userCtrl.pbDropdown.Style.Add("disabled", "true");
                                }
                                else
                                {
                                    userCtrl.txtDLC.Attributes.Add("Editable", "Yes");

                                }
                                tc1.Width = new Unit(200, UnitType.Pixel);
                                tc1.Style.Add("padding-left", "15px");
                                tc1.Controls.Add(userCtrl);
                                tr.Cells.Add(tc1);
                                userCtrl.txtDLC.Dispose();
                                //userCtrl.listDLC.Dispose();


                                break;

                            case "LABEL":
                                label = new HtmlInputText();
                                label.EnableViewState = false;
                                label.Value = DsObj.Display_Text;
                                // label.Style.Add("font-size", "small");
                                // label.Style.Add("color", "Black");
                                //BugID:48016
                                //if (DsObj.Is_Macra_Field.ToUpper() == "Y")
                                //{
                                //    label.Style.Add("color", "#6504d0");
                                //}

                                if (DsObj.Mandatory.ToUpper() == "Y")
                                {
                                    label.Attributes.Add("Mand", "Yes");
                                }
                                else
                                {
                                    if (DsObj.Is_Macra_Field.ToUpper() == "Y")
                                        label.Attributes.Add("Mand", "Macra");
                                    else
                                        label.Attributes.Add("Mand", "No");

                                }

                                //  label.Style.Add("background-color", "White");
                                // label.Style.Add("font-family", "Serif");
                                label.Style.Add("Width", DsObj.Column_Span_Thin_Client + "px");
                                label.Attributes.Add("readonly", "readonly");
                                // label.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
                                label.Attributes.Add("disabled", "disabled");
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(label);
                                tr.Cells.Add(tc);
                                label.Dispose();
                                break;
                            case "RESULTLABEL":
                                label = new HtmlInputText();
                                label.ID = DsObj.Control_Name_Thin_Client.Trim();
                                label.EnableViewState = false;
                                if (ClientSession.EncounterId != 0 && openingfrom != "Menu")
                                    label.Value = Loinc_observation;
                                label.Style.Add("font-size", "small");
                                label.Style.Add("color", "Black");
                                label.Style.Add("background-color", "White");
                                label.Style.Add("font-family", "Serif");
                                label.Style.Add("Width", DsObj.Column_Span_Thin_Client + "px");
                                label.Style.Add("font-weight", "bold");
                                label.Attributes.Add("readonly", "readonly");
                                label.Attributes.Add("disabled", "disabled");
                                label.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
                                tc.ColumnSpan = 8;
                                tc.Controls.Add(label);
                                tr.Cells.Add(tc);
                                label.Dispose();

                                break;
                            case "STATUSLABEL":
                                HtmlInputText statuslabel = null;
                                statuslabel = new HtmlInputText();
                                // HtmlTextArea statuslabel = new HtmlTextArea();

                                if (DsObj.Control_Name_Thin_Client.Trim().ToUpper().Contains("BP"))
                                {
                                    statuslabel.ID = DsObj.Control_Name_Thin_Client.Trim().Replace(" ", "").Replace("-", "").Replace("/", ""); ;
                                }
                                else
                                    statuslabel.ID = DsObj.Control_Name_Thin_Client.Trim();

                                statuslabel.EnableViewState = true;
                                statuslabel.Value = DsObj.Display_Text;
                                statuslabel.Style.Add("color", "Black");

                                if (statuslabel.ID.ToUpper().Contains("BMI") && hdnBMI.Value != string.Empty)
                                {
                                    if (hdnBMI.Value.Contains("+"))
                                    {
                                        string[] str = hdnBMI.Value.Split('+');
                                        if (str.Length == 2)
                                        {
                                            statuslabel.Value = str[0];

                                            if (str[1].Contains("black"))
                                                statuslabel.Style.Add("color", "Black");
                                            else
                                                statuslabel.Style.Add("color", "Red");
                                        }
                                    }
                                }

                                if (statuslabel.ID.ToUpper().Contains("BP") && hdnBP.Value != string.Empty)
                                {
                                    if (hdnBP.Value.Contains("+"))
                                    {
                                        string[] str = hdnBP.Value.Split('+');
                                        if (str.Length == 2)
                                        {
                                            statuslabel.Value = str[0];

                                            if (str[1].Contains("black"))
                                                statuslabel.Style.Add("color", "Black");
                                            else
                                                statuslabel.Style.Add("color", "Red");
                                        }
                                    }
                                }
                                if (statuslabel.ID.ToUpper().Contains("HBA1C") && hdnHbA1c.Value != string.Empty)
                                {
                                    if (hdnHbA1c.Value.Contains("+"))
                                    {
                                        string[] str = hdnHbA1c.Value.Split('+');
                                        if (str.Length == 2)
                                        {
                                            statuslabel.Value = str[0];

                                            if (str[1].Contains("black"))
                                                statuslabel.Style.Add("color", "Black");
                                            else
                                                statuslabel.Style.Add("color", "Red");
                                        }
                                    }
                                }
                                if (statuslabel.ID.ToUpper().Contains("HGB") && hdnHgb.Value != string.Empty)
                                {
                                    if (hdnHgb.Value.Contains("+"))
                                    {
                                        string[] str = hdnHgb.Value.Split('+');
                                        if (str.Length == 2)
                                        {
                                            statuslabel.Value = str[0];

                                            if (str[1].Contains("black"))
                                                statuslabel.Style.Add("color", "Black");
                                            else
                                                statuslabel.Style.Add("color", "Red");
                                        }
                                    }
                                }
                                if (statuslabel.ID.ToUpper().Contains("Urine for Microalbumin") && hdnUrineforMicroalbumin.Value != string.Empty)
                                {
                                    if (hdnUrineforMicroalbumin.Value.Contains("+"))
                                    {
                                        string[] str = hdnUrineforMicroalbumin.Value.Split('+');
                                        if (str.Length == 2)
                                        {
                                            statuslabel.Value = str[0];

                                            if (str[1].Contains("black"))
                                                statuslabel.Style.Add("color", "Black");
                                            else
                                                statuslabel.Style.Add("color", "Red");
                                        }
                                    }
                                }
                                if (statuslabel.ID.ToUpper().Contains("EGFR"))
                                    if (statuslabel.ID.Contains("Second"))
                                    {
                                        if (hdnegfrSecond.Value.Contains("+"))
                                        {
                                            string[] str = hdnegfrSecond.Value.Split('+');
                                            if (str.Length == 2)
                                            {
                                                statuslabel.Value = str[0];

                                                if (str[1].Contains("black"))
                                                    statuslabel.Style.Add("color", "Black");
                                                else
                                                    statuslabel.Style.Add("color", "Red");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (hdnegfr.Value.Contains("+"))
                                        {
                                            string[] str = hdnegfr.Value.Split('+');
                                            if (str.Length == 2)
                                            {
                                                statuslabel.Value = str[0];

                                                if (str[1].Contains("black"))
                                                    statuslabel.Style.Add("color", "Black");
                                                else
                                                    statuslabel.Style.Add("color", "Red");
                                            }
                                        }
                                    }
                                if (statuslabel.ID.ToUpper().Contains("FASTING"))
                                    if (statuslabel.ID.Contains("Second"))
                                    {
                                        if (hdnBloodFastingSecond.Value.Contains("+"))
                                        {
                                            string[] str = hdnBloodFastingSecond.Value.Split('+');
                                            if (str.Length == 2)
                                            {
                                                statuslabel.Value = str[0];

                                                if (str[1].Contains("black"))
                                                    statuslabel.Style.Add("color", "Black");
                                                else
                                                    statuslabel.Style.Add("color", "Red");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (hdnBloodFasting.Value.Contains("+"))
                                        {
                                            string[] str = hdnBloodFasting.Value.Split('+');
                                            if (str.Length == 2)
                                            {
                                                statuslabel.Value = str[0];

                                                if (str[1].Contains("black"))
                                                    statuslabel.Style.Add("color", "Black");
                                                else
                                                    statuslabel.Style.Add("color", "Red");
                                            }
                                        }
                                    }
                                if (statuslabel.ID.ToUpper().Contains("PRANDIAL"))
                                    if (statuslabel.ID.Contains("Second"))
                                    {
                                        if (hdnBloodPostSecond.Value.Contains("+"))
                                        {
                                            string[] str = hdnBloodPostSecond.Value.Split('+');
                                            if (str.Length == 2)
                                            {
                                                statuslabel.Value = str[0];

                                                if (str[1].Contains("black"))
                                                    statuslabel.Style.Add("color", "Black");
                                                else
                                                    statuslabel.Style.Add("color", "Red");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (hdnBloodPost.Value.Contains("+"))
                                        {
                                            string[] str = hdnBloodPost.Value.Split('+');
                                            if (str.Length == 2)
                                            {
                                                statuslabel.Value = str[0];

                                                if (str[1].Contains("black"))
                                                    statuslabel.Style.Add("color", "Black");
                                                else
                                                    statuslabel.Style.Add("color", "Red");
                                            }
                                        }
                                    }


                                //  statuslabel.Attributes["tittle"] = statuslabel.Value;
                                // statuslabel.Style.Add("font-size", "small");
                                statuslabel.Style.Add("background-color", "White");
                                // statuslabel.Style.Add("font-family", "Serif");
                                statuslabel.Style.Add("Width", (Convert.ToInt32(DsObj.Column_Span_Thin_Client) - 5).ToString() + "px");
                                statuslabel.Attributes.Add("class", "Editabletxtbox");
                                if (statuslabel.Value.Length > 15)
                                    statuslabel.Style.Add("Height", "29px");
                                //else
                                //statuslabel.Style.Add("Height", "18px");
                                statuslabel.Style.Add("resize", "none");
                                statuslabel.Style.Add("overflow", "hidden");
                                statuslabel.Attributes.Add("readonly", "readonly");
                                statuslabel.Attributes.Add("disabled", "disabled");
                                statuslabel.Attributes.Add("wrap", "hard");
                                statuslabel.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
                                tc.ColumnSpan = 0;
                                if (statuslabel.Value != string.Empty)
                                    aryLabelFilled.Add(statuslabel.ID.ToString());
                                tc.Controls.Add(statuslabel);
                                tr.Cells.Add(tc);
                                statuslabel.Dispose();
                                break;
                            case "NUMERICUPDOWN":
                                HtmlInputText spin = new HtmlInputText();


                                // var spanUp = new HtmlGenericControl("span");
                                //  span.InnerHtml = "From<br/>Date";
                                //spanUp.Attributes["class"] = "glyphicon glyphicon-menu-up";
                                //spanUp.Style.Add("width", "2px");
                                //spanUp.Style.Add("height", "2px");


                                //    var spanDown = new HtmlGenericControl("span");
                                ////  span.InnerHtml = "From<br/>Date";
                                //    spanDown.Attributes["class"] = "glyphicon glyphicon-menu-down";
                                //    spanDown.Style.Add("width", "2px");
                                //    spanUp.Style.Add("height", "2px");
                                //    spanDown.Style.Add(" padding-top", "50px");
                                //RadNumericTextBox spin = new RadNumericTextBox();
                                string SpinName = DsObj.Control_Name_Thin_Client.Trim();
                                if ((SpinName.Contains("-")) || (SpinName.Contains("/")))
                                {
                                    if (SpinName.Contains("-"))
                                    {
                                        SpinName = SpinName.Replace("-", string.Empty);
                                    }
                                    if (SpinName.Contains("/"))
                                    {
                                        SpinName = SpinName.Replace("/", string.Empty);
                                    }
                                    if (SpinName.Contains(" "))
                                    {
                                        SpinName = SpinName.Replace(" ", string.Empty);
                                    }
                                }
                                else
                                {
                                    SpinName = SpinName.Replace(" ", string.Empty);
                                }
                                spin.ID = SpinName;
                                spin.MaxLength = 4;
                                spin.Attributes.Add("class", "NumericUpDown");
                                spin.Style.Add("width", "30px");
                                spin.Style.Add("height", "17px");

                                //spin.SkinID = DsObj.Control_Name_Thin_Client;
                                //spin.NumberFormat.DecimalDigits = 0;
                                //arynumericcreationFilled.Add(spin.ID.ToString());
                                //spin.Width = Convert.ToInt32(DsObj.Column_Span_Thin_Client) + 20;
                                //spin.Font.Name = "GenericSansSerif";
                                //spin.Font.Size = FontUnit.Small;
                                //spin.AutoCompleteType = AutoCompleteType.Disabled;
                                //spin.MinValue = Convert.ToDouble(DsObj.Minimum_Value);
                                //spin.MaxValue = Convert.ToDouble(DsObj.Maximum_Value);
                                //spin.ShowSpinButtons = true;
                                //spin.ClientEvents.OnError = "RangeError";

                                // spin.Attributes.Add("onkeypress", "EnableSave(event);");
                                spin.Attributes.Add("onchange", "EnableSave(event);");
                                spin.Attributes.Add("onkeypress", "return NotAllowdecimal('" + Events + "')");
                                if (spin.ID.ToUpper().Contains("SYSDIA"))
                                    spin.Attributes.Add("onblur", "CheckMaxValue(" + DsObj.Minimum_Value + "," + DsObj.Maximum_Value + ",'" + SpinName + "'); setBPStatus('" + spin.ID + "','" + spin.ID.Replace("SysDia", "Diastolic") + "')");

                                else
                                    spin.Attributes.Add("onblur", "CheckMaxValue(" + DsObj.Minimum_Value + "," + DsObj.Maximum_Value + ",'" + SpinName + "'); setBPStatus('" + spin.ID.Replace("Diastolic", "SysDia") + "','" + spin.ID + "')");

                                //  spin.Attributes.Add("onload", "ChangeTextSpinner();");
                                //spin.Style["text-align"] = "right";
                                string BPStatus = string.Empty;
                                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                {
                                    iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "BP SYSTOLIC STATUS").ToList();
                                }

                                BPStatus += "var BP_SYSTOLIC_STATUS=[";
                                foreach (StaticLookup obj in iFieldLookupList)
                                {
                                    if (obj.Value.Contains("-"))
                                        BPStatus += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                    else
                                        BPStatus += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value + "'},";
                                }
                                BPStatus = BPStatus.Substring(0, BPStatus.Length - 1);
                                BPStatus += "];";

                                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                {
                                    iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "BP DIASTOLIC STATUS").ToList();
                                }

                                BPStatus += "var BP_DIASTOLIC_STATUS=[";
                                foreach (StaticLookup obj in iFieldLookupList)
                                {
                                    if (obj.Value.Contains("-"))
                                        BPStatus += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value.Split('-')[0] + "','upperLimit':'" + obj.Value.Split('-')[1] + "'},";
                                    else
                                        BPStatus += "{'description':'" + obj.Description + "','lowerLimit':'" + obj.Value + "'},";
                                }
                                BPStatus = BPStatus.Substring(0, BPStatus.Length - 1);
                                BPStatus += "];";
                                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                                {
                                    iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
                                }



                                if (DsObj.Mandatory == "Y" && ClientSession.EncounterId != 0 && openingfrom != "Menu")
                                {
                                    if (humanAgeInYears > 2)
                                    {
                                        string pairedControl = SpinName.Replace("SysDia", "Diastolic");
                                        // MandValdInjection += "if(document.getElementById('" + spin.ID + "').value!=undefined && document.getElementById('" + pairedControl + "').value!=undefined ){ if(document.getElementById('" + spin.ID + "').value=='' && document.getElementById('" + pairedControl + "').value=='' && document.getElementById('txtNotes" + SpinName + "_txtDLC').value==''){SaveUnsuccessful();;DisplayErrorMessage('200031','','" + SpinName + "'); return false;}else if(document.getElementById('txtNotesBPSittingSysDia_txtDLC').value==''){if(document.getElementById('" + spin.ID + "').value=='' ){DisplayErrorMessage('200032','','" + spin.ID + "');return false;}else if(document.getElementById('" + pairedControl + "').value==''){DisplayErrorMessage('200032','','" + pairedControl + "');return false;}}}";


                                        mandatoryFeidId = mandatoryFeidId + "~" + spin.ID + "-spin";
                                        mandatoryCtrls.Add(spin);

                                        AvoidDuplicate.Add(pairedControl);
                                        AvoidDuplicate.Add(spin.ID);
                                    }
                                }
                                else if (!AvoidDuplicate.Contains(spin.ID))
                                {
                                    string pairedControl = SpinName.Replace("SysDia", "Diastolic");
                                    MandValdInjection += "if(document.getElementById('" + spin.ID + "').value!=undefined && document.getElementById('" + pairedControl + "').value!=undefined ){if(document.getElementById('" + spin.ID + "').value!='' || document.getElementById('" + pairedControl + "').value!=''){if(document.getElementById('" + spin.ID + "').value=='' ){SaveUnsuccessful();;DisplayErrorMessage('200031','','" + spin.ID.Replace("Second", "") + "');return false;}else if(document.getElementById('" + pairedControl + "').value==''){DisplayErrorMessage('200032','','" + pairedControl.Replace("Second", "") + "');return false;}}}";
                                    AvoidDuplicate.Add(pairedControl);
                                    AvoidDuplicate.Add(spin.ID);
                                }
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "BPSTATUS", BPStatus, true);
                                // tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(spin);
                                // tc.Controls.Add(new LiteralControl("&nbsp;"));
                                //tc.Controls.Add(spanUp);
                                //tc.Controls.Add(spanDown);
                                tr.Cells.Add(tc);
                                spin.Dispose();
                                break;

                            case "PICTUREBOX":
                                picBox = new ImageButton();
                                picBox.ImageUrl = "~/Resources/Default_Icon.jpg";
                                picBox.ID = DsObj.Control_Name_Thin_Client.Trim();
                                picBox.Style.Add("margin-top", "2px");
                                string AssociatedIDOne = DsObj.Control_Name_Thin_Client.Trim().Replace("-", "") + "SysDia";
                                string AssociatedIDTwo = DsObj.Control_Name_Thin_Client.Trim().Replace("-", "") + "Diastolic";
                                string DefaultValueScriptInjection = string.Empty;
                                //Jira #CAP-733
                                //DefaultValueScriptInjection = "function SetDefaultValue" + AssociatedIDOne + "( controlOne,controlTwo,Syst,Dias){  var controlOneText=document.getElementById(controlOne).value;var controlTwoText=document.getElementById(controlTwo).value;if(controlOneText=='' && controlTwoText=='' ){document.getElementById(controlOne).value=Syst;document.getElementById(controlTwo).value=Dias;if(document.getElementById('hdnBPValue').value!='I10') {document.getElementById(controlOne+'Status').value='Pre-Hypertensive';setBPStatus('','');document.getElementById(controlOne+'Status').style.color = 'red';RemoveBPReason(controlOne,controlTwo);}EnableSave(true);}else {if(document.getElementById(controlOne).value !=Syst || document.getElementById(controlTwo).value !=Dias){if(confirm('You have entered some values. Do you want to replace it with default values ?')){document.getElementById(controlOne).value=Syst;document.getElementById(controlTwo).value=Dias;if(document.getElementById('hdnBPValue').value!='I10' ){document.getElementById(controlOne+'Status').value='Pre-Hypertensive';document.getElementById(controlOne+'Status').style.color = 'red';setBPStatus('','');RemoveBPReason(controlOne,controlTwo);}EnableSave(true);}}}}";
                                DefaultValueScriptInjection = "function SetDefaultValue" + AssociatedIDOne + "( controlOne,controlTwo,Syst,Dias){  var controlOneText=document.getElementById(controlOne).value;var controlTwoText=document.getElementById(controlTwo).value;if(controlOneText=='' && controlTwoText=='' ){document.getElementById(controlOne).value=Syst;document.getElementById(controlTwo).value=Dias;if(document.getElementById('hdnBPValue').value!='I10') {document.getElementById(controlOne+'Status').value='Pre-Hypertensive';setBPStatus('','');document.getElementById(controlOne+'Status').style.color = 'red';RemoveBPReason(controlOne,controlTwo);}if(document.getElementById('hdnVisittype').value=='TELEMEDICINE' && controlOne=='BPSittingSysDia'){ document.getElementById('txtNotesBPSittingSysDia_txtDLC').value ='';} EnableSave(true);}else {if(document.getElementById(controlOne).value !=Syst || document.getElementById(controlTwo).value !=Dias){if(confirm('You have entered some values. Do you want to replace it with default values ?')){document.getElementById(controlOne).value=Syst;document.getElementById(controlTwo).value=Dias;if(document.getElementById('hdnBPValue').value!='I10' ){document.getElementById(controlOne+'Status').value='Pre-Hypertensive';document.getElementById(controlOne+'Status').style.color = 'red';setBPStatus('','');RemoveBPReason(controlOne,controlTwo);}if(document.getElementById('hdnVisittype').value=='TELEMEDICINE' && controlOne=='BPSittingSysDia'){ document.getElementById('txtNotesBPSittingSysDia_txtDLC').value ='';} EnableSave(true);}}}}";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetDefaultValue" + AssociatedIDOne, DefaultValueScriptInjection, true);
                                //  if (ClientSession.UserRole.Trim().ToUpper() == "MEDICAL ASSISTANT" && ClientSession.UserPermission.Trim().ToUpper() == "U" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN" && ClientSession.UserPermission.Trim().ToUpper() == "U" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserPermission.Trim().ToUpper() == "U" || ClientSession.UserRole.Trim().ToUpper() == "TECHNICIAN" && ClientSession.UserPermission.Trim().ToUpper() == "U")
                                //  {
                                string sLowlimit=string.Empty;
                                string sUpperlimit = string.Empty;
                                if (Session["BPLowerLimit"]!=null)
                                    sLowlimit= Session["BPLowerLimit"].ToString();
                                if (Session["BPUpperLimit"] != null)
                                    sUpperlimit = Session["BPUpperLimit"].ToString();
                                picBox.Attributes.Add("onclick", "SetDefaultValue" + AssociatedIDOne + "('" + AssociatedIDOne + "','" + AssociatedIDTwo + "','" + sUpperlimit + "','" + sLowlimit + "');return false;");
                                // }
                                if (DsObj.Column_Span_Thin_Client != null && DsObj.Column_Span_Thin_Client!="")
                                    picBox.Width = Convert.ToInt32(DsObj.Column_Span_Thin_Client);
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                picList.Add(picBox);
                                tc.Controls.Add(picBox);

                                tr.Cells.Add(tc);
                                picBox.Dispose();
                                break;

                            case "GROUPLABEL":
                                HtmlInputText grouplbl = new HtmlInputText();
                                grouplbl.Value = DsObj.Display_Text;
                                grouplbl.EnableViewState = false;
                                // grouplbl.Style.Add("font-size", "small");
                                // grouplbl.Style.Add("color", "Black");
                                // grouplbl.Style.Add("background-color", "#BFDBFF");
                                //  grouplbl.Style.Add("font-family", "Serif");
                                // grouplbl.Style.Add("font-weight", "bold");
                                grouplbl.Attributes.Add("GroupLabelstyle", "Y");

                                grouplbl.Style.Add("Width", lblHeaderLine.Width.Value + "px");
                                //  grouplbl.Style.Add(HtmlTextWriterStyle.BorderWidth, "1");
                                grouplbl.Attributes.Add("readonly", "readonly");
                                tc.ColumnSpan = 5;
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                grouplbl.Attributes.Add("disabled", "disabled");
                                tc.Controls.Add(grouplbl);
                                tr.Cells.Add(tc);
                                grouplbl.Dispose();
                                break;

                            case "CHECKBOX":
                                CheckBox chkBox = new CheckBox();
                                chkBox.Text = DsObj.Display_Text;
                                chkBox.ID = DsObj.Control_Name_Thin_Client;
                                chkBox.ForeColor = Color.Black;
                                chkBox.EnableViewState = true;
                                chkBox.Width = Convert.ToInt32(DsObj.Column_Span_Thin_Client);
                                chkBox.Font.Name = "GenericSansSerif";
                                chkBox.Font.Size = FontUnit.Small;
                                chkBox.Attributes.Add("onclick", "enableField('" + chkBox.ID + "');");
                                chkBox.Attributes.Add("onkeypress", "EnableSave(event);");
                                chkBox.Attributes.Add("onchange", "EnableSave(event);");
                                tc.Controls.Add(chkBox);





                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("SITTING") && DsObj.Control_Name_Thin_Client.ToUpper().Contains("SECOND"))
                                {
                                    // tc.Controls.Add(new LiteralControl("&nbsp;"));
                                    //tc.Controls.Add(new LiteralControl("&nbsp;"));

                                }
                                tr.Cells.Add(tc);
                                if (objMapVital.BP_Status != null && objMapVital.BP_Status != string.Empty)
                                {
                                    if (objMapVital.BP_Status.ToUpper() == "LEFT")
                                    {
                                        if (chkBox.ID.ToUpper().Contains("LEFT"))
                                        {
                                            chkBox.Checked = true;
                                        }
                                    }
                                    else if (objMapVital.BP_Status.ToUpper() == "RIGHT")
                                    {
                                        if (chkBox.ID.ToUpper().Contains("RIGHT"))
                                        {
                                            chkBox.Checked = true;
                                        }
                                    }
                                }

                                chkBox.Dispose();
                                break;
                            case "RADIOBUTTON":
                                RadioButton rdbtn = new RadioButton();
                                rdbtn.Text = DsObj.Display_Text;
                                rdbtn.ID = DsObj.Control_Name_Thin_Client;
                                rdbtn.ForeColor = Color.Black;
                                rdbtn.EnableViewState = true;
                                rdbtn.Width = Convert.ToInt32(DsObj.Column_Span_Thin_Client);
                                rdbtn.Attributes.Add("Mand", "No");
                                // rdbtn.Font.Name = "GenericSansSerif";
                                //rdbtn.Font.Size = FontUnit.Small;
                                rdbtn.Attributes.Add("onclick", "enableField('" + rdbtn.ID + "');");
                                rdbtn.Attributes.Add("onkeypress", "EnableSave(event);");
                                rdbtn.Attributes.Add("onchange", "EnableSave(event);");
                                tc.Controls.Add(rdbtn);
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                if (DsObj.Control_Name_Thin_Client.ToUpper().Contains("SITTING") && DsObj.Control_Name_Thin_Client.ToUpper().Contains("SECOND"))
                                {
                                    // tc.Controls.Add(new LiteralControl("&nbsp;"));
                                    //tc.Controls.Add(new LiteralControl("&nbsp;"));

                                }
                                tr.Cells.Add(tc);
                                if (objMapVital.BP_Status != null && objMapVital.BP_Status != string.Empty)
                                {
                                    if (objMapVital.BP_Status.ToUpper() == "LEFT")
                                    {
                                        if (rdbtn.ID.ToUpper().Contains("LEFT"))
                                        {
                                            rdbtn.Checked = true;
                                        }
                                    }
                                    else if (objMapVital.BP_Status.ToUpper() == "RIGHT")
                                    {
                                        if (rdbtn.ID.ToUpper().Contains("RIGHT"))
                                        {
                                            rdbtn.Checked = true;
                                        }
                                    }
                                }

                                rdbtn.Dispose();
                                break;

                            case "MASKTEXTBOX":
                                txtbox = new HtmlInputText();
                                txtbox.ID = DsObj.Control_Name_Thin_Client.Trim();

                                txtbox.Style.Add(HtmlTextWriterStyle.Width, DsObj.Column_Span_Thin_Client + "px");
                                //  txtbox.MaxLength = DsObj.Maximum_Length + 3;
                                txtbox.Attributes.Add("onkeyup", "EnableSave(event);");
                                txtbox.Attributes.Add("onkeypress", "EnableSave(event);");
                                txtbox.Attributes.Add("onchange", "EnableSave(event);");

                                txtbox.Attributes.Add("class", "mask");
                                tc.Controls.Add(txtbox);
                                tr.Cells.Add(tc);
                                txtbox.Dispose();
                                break;

                            case "VITALTAKENDATETIMEPICKER":

                                // RadDatePicker dtpTakenDate = new RadDatePicker();
                                HtmlInputText dtpTakenDate = new HtmlInputText();
                                var ctrdtpName = Regex.Replace(DsObj.Control_Name_Thin_Client, @"[\-\/\s]+", string.Empty);
                                dtpTakenDate.ID = ctrdtpName;
                                dtpTakenDate.EnableViewState = true;
                                dtpTakenDate.SkinID = DsObj.Control_Name_Thin_Client;
                                dtpTakenDate.Attributes.Add("class", "VitalDateInput");
                                // dtpTakenDate.ForeColor = Color.Black;
                                // dtpTakenDate.Font.Name = "GenericSansSerif";
                                // dtpTakenDate.Font.Size = FontUnit.Small;
                                DateTime temp = DateTime.MinValue;
                                if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
                                {
                                    if (openingfrom != "Menu")
                                    {
                                        temp = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                                        // dtpTakenDate. = temp;
                                    }
                                    else
                                    {
                                        temp = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                                        // temp = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                                        // dtpTakenDate.MaxDate = temp;
                                    }
                                }
                                dtpTakenDate.Style.Add("Width", "140px");


                                // dtpTakenDate.Width = 108;
                                // dtpTakenDate.Calendar.ShowRowHeaders = false;
                                // dtpTakenDate.ClientEvents.OnDateSelected = "onChangeDatePicker";
                                //dtpTakenDate.Calendar.DayRender += new Telerik.Web.UI.Calendar.DayRenderEventHandler(Calendar_DayRender);
                                // dtpTakenDate.Attributes.Add("onValueChanged", "EnableSave(event);");
                                dtpTakenDate.Attributes.Add("onclick", "enableField('" + dtpTakenDate.ID + "');");
                                dtpTakenDate.Attributes.Add("onkeypress", "EnableSave(event);");
                                dtpTakenDate.Attributes.Add("onchange", "EnableSave(event);");
                                dtpTakenDate.Style.Add("vertical-align", "super");
                                // dtpTakenDate.Attributes.Add("OnDateSelected", "EnableSave(event);");

                                dtpTakenDate.Attributes.Add("onclick", "Display(event)");
                                dtpTakenDate.Attributes.Add("onblur", "myClickHandler()");

                                DateTime time = new DateTime();
                                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                                {
                                    openingfrom = Request["openingfrom"].ToString();
                                    if (openingfrom != "Menu")
                                    {
                                        var PatientPaneList = from d in ClientSession.PatientPaneList where d.Encounter_ID == ClientSession.EncounterId select d;
                                        if (PatientPaneList.Count() > 0)
                                        {
                                            lstPatientPane = PatientPaneList.ToList<PatientPane>();
                                            if (lstPatientPane != null && lstPatientPane.Count > 0)
                                            {
                                                //   dtpTakenDate.SelectedDate = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service);
                                                time = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
                                        {
                                            //DateTime localDate = temp;
                                            // dtpTakenDate.SelectedDate = temp;
                                            time = temp;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
                                    {
                                        //DateTime localDate = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy h':'m':'s", null));
                                        // dtpTakenDate.SelectedDate = temp;
                                        time = temp;
                                    }
                                }
                                //if (dtpTakenDate.SelectedDate != null)
                                //    dtpTakenDate.SelectedDate = Convert.ToDateTime(dtpTakenDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
                                //dtpTakenDate.DateInput.DateFormat = "dd-MMM-yyyy";
                                //dtpTakenDate.DateInput.DisplayDateFormat = "dd-MMM-yyyy";
                                //dtpTakenDate.EnableTyping = false;

                                //dtpTakenDate.SelectedDateChanged += new Telerik.Web.UI.Calendar.SelectedDateChangedEventHandler(dtpTakenDate_SelectedDateChanged);

                                dtpTakenDate.Value = Convert.ToDateTime(time).ToString("dd-MMM-yyyy hh:mm tt");
                                //arydatetimeIDlist.Add(dtpTakenDate.ID);
                                tc.Controls.Add(dtpTakenDate);
                                tr.Cells.Add(tc);

                                dtpTakenDate.Dispose();

                                //MKB.TimePicker.TimeSelector dtpTakenTime = new MKB.TimePicker.TimeSelector();
                                //dtpTakenTime.ID = "dtpTakenTime_" + ctrdtpName;
                                //dtpTakenTime.DisplaySeconds = false;
                                //if (time.Hour == 0)
                                //{
                                //    dtpTakenTime.Hour = 12;
                                //}
                                //else
                                //{
                                //    dtpTakenTime.Hour = time.Hour;
                                //}
                                //dtpTakenTime.Minute = time.Minute;
                                //dtpTakenTime.MinuteIncrement = 1;
                                //dtpTakenTime.AmPm = time.ToString("tt") == "AM" ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                                //dtpTakenTime.Width = 88;
                                //dtpTakenTime.SelectedTimeFormat = MKB.TimePicker.TimeSelector.TimeFormat.Twelve;
                                //dtpTakenTime.Style.Add("vertical-align", "middle");
                                //dtpTakenTime.Attributes.Add("onValueChanged", "SetFocusDateTime('" + dtpTakenTime.ID + "');EnableSave(event);");
                                //dtpTakenTime.Attributes.Add("onclick", "SetFocusDateTime('" + dtpTakenTime.ID + "');EnableSave(event);");
                                //dtpTakenTime.Attributes.Add("onchange", "SetFocusDateTime('" + dtpTakenTime.ID + "');EnableSave(event);");
                                //dtpTakenTime.Attributes.Add("onclick", "EnableSaveFocus(event);");
                                //arydatetimeIDlist.Add(dtpTakenTime.ID);
                                //tc.Controls.Add(dtpTakenTime);
                                //tr.Cells.Add(tc);
                                //dtpTakenTime.Dispose();

                                break;
                            case "CUSTOMDATETIMEPICKER":
                                // RadMaskedTextBox msks = new RadMaskedTextBox();

                                HtmlInputText msks = new HtmlInputText();

                                msks.ID = DsObj.Control_Name_Thin_Client.Replace(" ", "").Replace("/", "") + "DATEPICKER";
                                // msks.Mask = "####-Lll-##";
                                //  msks.ClientEvents.OnBlur = "CallMe";
                                // msks.Width = 100;
                                //msks.EnableViewState = false;

                                msks.Attributes.Add("class", "CustomDate");
                                //  msks.Attributes.Add("onkeypress", "EnableSave(event);");
                                msks.Attributes.Add("onkeypress", "EnableSave(event);");
                                msks.Attributes.Add("onclick", "DisplayDate(event)");
                                msks.Attributes.Add("onchange", "EnableSave(event);");
                                msks.Attributes.Add("onfocusout", "DefaultTest(event);");//BugID:48015
                                if (ClientSession.UserRole.Trim() == "Coder" || ClientSession.UserPermission == "R" || ClientSession.UserCurrentProcess == "CHECK_OUT" || ClientSession.UserCurrentProcess == "CHECK_OUT_WAIT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.ToUpper().Trim() == "UNKNOWN"))
                                {
                                    msks.Disabled = true;
                                }
                                else if (ClientSession.UserRole.Trim() == "Coder" || ClientSession.UserPermission == "R" || ClientSession.UserCurrentProcess == "CHECK_OUT" || ClientSession.UserCurrentProcess == "CHECK_OUT_WAIT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && (ClientSession.UserCurrentOwner.Trim() == string.Empty || ClientSession.UserCurrentOwner.ToUpper().Trim() == "UNKNOWN")))
                                {
                                    msks.Disabled = false;
                                }
                                //Label lblDateFormat = new Label();
                                //lblDateFormat.Text = "(Format: 1987-Jan-01)";
                                //lblDateFormat.Style.Add("font-family", "Serif");
                                //lblDateFormat.Style.Add("font-size", "small");
                                //tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(new LiteralControl("&nbsp;"));
                                tc.Controls.Add(msks);
                                tc.Controls.Add(msks);
                                //tc.Controls.Add(lblDateFormat);
                                tr.Cells.Add(tc);
                                break;
                        }

                        if (DsObj.Column_Name != string.Empty && DsObj.Table_Name != string.Empty)
                        {
                            vitalArray.Add(DsObj.Control_Name_Thin_Client.Trim());
                        }
                        if (DsObj.Control_Type.ToUpper() != "LABEL" || (DsObj.Control_Type.ToUpper() == "LABEL" && DsObj.Display_Text == string.Empty))
                        {
                            htDisplayText.Add(DsObj.Control_Name_Thin_Client.Trim(), DsObj.Display_Text);
                        }
                        if (DsObj.LookUp_Field.ToUpper() == "YES" && DsObj.Column_Name != string.Empty && DsObj.Table_Name != string.Empty && DsObj.Utility_Method != string.Empty)
                        {
                            htRetriveUnitMthds.Add(DsObj.Control_Name_Thin_Client.Trim(), DsObj.Utility_Method);
                        }
                        if (DsObj.LookUp_Field.ToUpper() == "YES" && DsObj.Column_Name != string.Empty && DsObj.Table_Name != string.Empty && DsObj.LookUp_Method_Thin_Client != string.Empty)
                        {
                            htUnitConvMthds.Add(DsObj.Control_Name_Thin_Client.Trim(), DsObj.LookUp_Method_Thin_Client);
                        }

                    }
                    objTable.Rows.Add(tr);
                    TableRow tr1 = new TableRow();

                    if (controlname.ToUpper().Contains("BP"))
                    {
                        tr1.ID = "tr1" + controlname.Replace("lblVital", "").Replace(" ", "").Replace("/", "").Replace("-", "");
                    }
                    tr1.Height = 15;
                    objTable.Rows.Add(tr1);

                }
            }

            if (arydatetimeIDlist != null && arydatetimeIDlist.Count > 0)
            {
                string ControlNames = string.Empty;
                ControlNames += "DTPCONTROLNAME=[";
                foreach (object obj in arydatetimeIDlist)
                {
                    string vital = obj.ToString();
                    if (vital.Contains("dtp"))
                    {
                        ControlNames += "{'dtpName':'" + vital + "'},";
                    }
                }
                ControlNames = ControlNames.Substring(0, ControlNames.Length - 1);
                ControlNames += "];";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "DTPValidation", ControlNames, true);
            }

            hdnscript.Value = mandatoryFeidId;
            // MandValdInjection += "return true;";
            //  string MandFunctionInjection = string.Empty;
            //MandFunctionInjection = "function checkMandatoryFields(){" + MandValdInjection + "}";
            // hdnscript.Value = MandValdInjection;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "MandatoryFieldValidation", MandFunctionInjection, true);

            Session["htDisplayText"] = htDisplayText;
            Session["htRetriveUnitMthds"] = htRetriveUnitMthds;
            Session["htUnitConvMthds"] = htUnitConvMthds;
            Session["vitalArray"] = vitalArray;
            //Session["VitalsTab"] = objTable;
            Session["objTab"] = objTable;
            pnlVitals.Controls.Add(objTable);

            if (openingfrom == "Menu")
            {
                int columnCount = 0;
                int columncountgrd = 0;

                if (Session["columnCount"] == null)
                    columnCount = dtTable.Columns.Count;
                else
                    columnCount = (int)Session["columnCount"];

                if (Session["columncountgrd"] == null)
                    columncountgrd = gvEnterDetails.Columns.Count;
                else
                    columncountgrd = (int)Session["columncountgrd"];

                Session["columnCount"] = columnCount;
                Session["columncountgrd"] = columncountgrd;

                dtTable.Columns.Clear();

                foreach (object obj in vitalArray)
                {
                    string vital = obj.ToString();
                    if ((obj.ToString().ToUpper().Contains("BP") && (obj.ToString().ToUpper().Contains("LEFT") || obj.ToString().ToUpper().Contains("RIGHT"))) == false)
                    {
                        if (!vital.Contains("StatusRemove"))
                        {
                            if (!dtTable.Columns.Contains(vital))
                            {
                                dtTable.Columns.Add(vital);
                                columnCount++;
                            }
                        }

                        //GridBoundColumn col = new GridBoundColumn();
                        //col.HeaderText = vital;
                        //col.DataField = vital;
                        //col.ItemStyle.Wrap = true;
                        //col.HeaderStyle.Wrap = false;
                        //col.ItemStyle.Width = col.HeaderText.Length * 7 + 100;

                        //if (!vital.Contains("StatusRemove"))
                        //{
                        //    try
                        //    {
                        //        grdPastVitals.MasterTableView.GetColumn(col.HeaderText);
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        grdPastVitals.Columns.Insert(columncountgrd, col);
                        //        columncountgrd++;
                        //    }
                        //}
                    }
                }
            }
            formLoad = false;
        }

        void Calendar_DayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
            {
                if (Request["openingfrom"] != "Menu")
                {
                    if (e.Day.Date > UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null)).Date)
                        e.Cell.Enabled = false;
                }
                else
                {
                    if (e.Day.Date > DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null).Date)
                        e.Cell.Enabled = false;
                }
            }
        }

        //private void LoadNotes(string temp)
        //{
        //    IList<UserLookup> fieldlist = null;
        //    fieldlist = localFieldLookupManager.GetFieldLookupList(ClientSession.UserName, temp);

        //    if (fieldlist != null)
        //    {
        //        for (int j = 0; j < fieldlist.Count; j++)
        //        {
        //            RadListBoxItem tempItem = new RadListBoxItem(fieldlist[j].Value);
        //            tempItem.Text = fieldlist[j].Value;
        //            tempItem.ToolTip = "Click to add Notes";
        //        }
        //    }
        //}
        //void dtpTakenDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        //{
        //    EnableDisbaleSave(true);
        //    arydatetimeFilled.Add(((RadDatePicker)sender).ID.ToString());
        //}

        void spin_TextChanged(object sender, EventArgs e)
        {
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            if (Session["dynamicScreenList"] != null)
                dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];

            decimal MinValue = 0;
            decimal MaxValue = 0;
            Control ctrlnum = (Control)sender;
            // RadNumericTextBox ctrl = (RadNumericTextBox)ctrlnum;
            HtmlInputText ctrl = (HtmlInputText)ctrlnum;
            bool bCheckSpin = true;

            List<string> errList = new List<string>();

            if (ctrl.Value != string.Empty)
            {
                string CtrlErrorName = string.Empty;
                string ctrlName = ctrl.SkinID;
                var MinAndMaxVal = from dynScreenObj in dynamicScreenList where dynScreenObj.Control_Name_Thin_Client.Trim() == ctrlName select new { dynScreenObj.Minimum_Value, dynScreenObj.Maximum_Value };

                foreach (var item in MinAndMaxVal)
                {
                    MinValue = Convert.ToDecimal(item.Minimum_Value);
                    MaxValue = Convert.ToDecimal(item.Maximum_Value);
                }

                if (!((Convert.ToDecimal(ctrl.Value) >= MinValue) && (Convert.ToDecimal(ctrl.Value) <= MaxValue)))
                {
                    if (ctrlName.Contains("Second"))
                    {
                        CtrlErrorName = ctrlName.Replace("Second", "");
                    }
                    else
                    {
                        CtrlErrorName = ctrlName;
                    }
                    string ValidationError = string.Empty;
                    if (CtrlErrorName.Contains("-"))
                        ValidationError = CtrlErrorName.Replace("-", " ");
                    else
                        ValidationError = CtrlErrorName;

                    errList.Add(ValidationError);
                    errList.Add(MinValue.ToString());
                    errList.Add(MaxValue.ToString());

                    bCheckSpin = false;
                    string strerrlist = string.Join("-", errList.ToArray());

                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('200004','','" + strerrlist + "');", true);

                    ctrl.Focus();

                }
                arynumericFilled.Add(((HtmlInputText)sender).ID.ToString());
            }

            EnableDisbaleSave(true);
        }

        void picBox_Click(object sender, ImageClickEventArgs e)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }

            int j = 0;
            EnableDisbaleSave(true);
            string PicID = string.Empty;
            Control[] ctr = new Control[5];
            ImageButton pic = (ImageButton)sender;

            if (pic.ID != null && pic.ID != string.Empty)
            {
                PicID = pic.ID;
                Session["PicID"] = PicID;
                for (int i = 0; i < arynumericcreationFilled.Count; i++)
                {
                    if (arynumericcreationFilled[i].ToString().Contains(pic.ID.Replace("-", "")) && (pic.ID.ToUpper().Contains("SECOND")))
                    {
                        ctr[j] = pnlVitals.FindControl(arynumericcreationFilled[i].ToString());
                        j++;
                    }
                    else if (arynumericcreationFilled[i].ToString().Contains(pic.ID.Replace("-", "")))
                    {
                        ctr[j] = pnlVitals.FindControl(arynumericcreationFilled[i].ToString());
                        j++;
                    }
                }
            }

            if (ctr.Length > 0)
            {
                IList<StaticLookup> defValues = new List<StaticLookup>();
                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                {
                    defValues = StaticLookUpList.Where(q => q.Field_Name == "BP DEFAULT VALUES").ToList();
                }

                pic.Focus();

                // RadNumericTextBox ctr1 = (RadNumericTextBox)ctr[0];
                //  RadNumericTextBox ctr2 = (RadNumericTextBox)ctr[1];

                HtmlInputText ctr1 = (HtmlInputText)ctr[0];
                HtmlInputText ctr2 = (HtmlInputText)ctr[1];


                if ((ctr1.Value != string.Empty) || (ctr2.Value != string.Empty))
                {
                    if (defValues != null && defValues.Count > 0)
                    {
                        if ((ctr1.Value == defValues[0].Value.ToString()) && (ctr2.Value == defValues[1].Value.ToString()))
                        {
                            // ctr1.ForeColor = Color.Black;

                            ctr1.Style.Add("Color", "Black");
                            return;
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "LoadDefaultValues();", true);
                }
                else
                {
                    if (defValues.Count > 0)
                    {
                        ctr1.Value = defValues[0].Value.ToString();
                        ctr2.Value = defValues[1].Value.ToString();
                        //  ctr1.ForeColor = Color.Black;
                        ctr1.Style.Add("Color", "Black");

                        ctr1.Focus();
                    }
                }

            }
        }

        //void txtbox_TextChanged(object sender, EventArgs e)
        //{
        //    decimal MinValue = 0;
        //    decimal MaxValue = 0;
        //    IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
        //    if (Session["dynamicScreenList"] != null)
        //        dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];
        //    Hashtable htDisplayText = new Hashtable();
        //    Hashtable htUnitConvMthds = new Hashtable();
        //    Hashtable htRetriveUnitMthds = new Hashtable();

        //    if (Session["htDisplayText"] != null)
        //    {
        //        htDisplayText = (Hashtable)Session["htDisplayText"];
        //    }
        //    if (Session["htRetriveUnitMthds"] != null)
        //    {
        //        htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
        //    }
        //    if (Session["htUnitConvMthds"] != null)
        //    {
        //        htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
        //    }
        //    decimal InchMaxValue = 0, InchMinValue = 0;
        //    RadTextBox text = (RadTextBox)sender;
        //    bool vaild = true;
        //    bool InchFlag = false;
        //    ArrayList errList = new ArrayList();
        //    string LeaveErrorName = string.Empty;
        //    IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
        //    IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
        //    if (Session["StaticLookUpList"] != null)
        //    {
        //        StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
        //    }
        //    if (StaticLookUpList != null && StaticLookUpList.Count > 0)
        //    {
        //        iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "HEIGHT INCH MAXIMUM VALUE").ToList();
        //    }

        //    if (iFieldLookupList != null && iFieldLookupList.Count > 0)
        //    {
        //        string[] str = iFieldLookupList.First().Value.Split('-');
        //        if (str.Length == 2)
        //        {
        //            InchMinValue = Convert.ToDecimal(str[0]);
        //            InchMaxValue = Convert.ToDecimal(str[1]);
        //        }
        //    }
        //    if (text.Text != string.Empty && text.ID != "BMI")
        //    {
        //        string ctrlName = text.ID;
        //        var MinAndMaxVal = from dynScreenObj in dynamicScreenList where dynScreenObj.Control_Name_Thin_Client.Trim() == ctrlName select new { dynScreenObj.Minimum_Value, dynScreenObj.Maximum_Value, dynScreenObj.Maximum_Length };

        //        foreach (var item in MinAndMaxVal)
        //        {
        //            if (item.Maximum_Value != string.Empty && item.Minimum_Value != string.Empty)
        //            {
        //                MinValue = Convert.ToDecimal(item.Minimum_Value);
        //                if (text.ID.ToUpper() == "HEIGHT INCH")
        //                {
        //                    Control ctrHt = pnlVitals.FindControl("Height");
        //                    RadTextBox ctrhtval = (RadTextBox)ctrHt;
        //                    if (ctrhtval != null)
        //                    {
        //                        if (ctrhtval.Text == string.Empty || ctrhtval.Text == "0")
        //                        {
        //                            MinValue = InchMinValue;
        //                            MaxValue = InchMaxValue;
        //                            InchFlag = true;
        //                        }
        //                        else
        //                        {
        //                            MaxValue = Convert.ToDecimal(item.Maximum_Value);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MaxValue = Convert.ToDecimal(item.Maximum_Value);
        //                    }
        //                }
        //                else
        //                {
        //                    MaxValue = Convert.ToDecimal(item.Maximum_Value);
        //                }

        //                try
        //                {
        //                    if (!((Convert.ToDecimal(text.Text) >= MinValue) && (Convert.ToDecimal(text.Text) <= MaxValue)))
        //                    {
        //                        if (ctrlName.Contains("Second"))
        //                        {
        //                            LeaveErrorName = ctrlName.Replace("Second", "");
        //                        }
        //                        else
        //                        {
        //                            LeaveErrorName = ctrlName;
        //                        }
        //                        string LeaveError = string.Empty;
        //                        if (LeaveErrorName.Contains("-"))
        //                            LeaveError = LeaveErrorName.Replace("-", " ");
        //                        else
        //                            LeaveError = LeaveErrorName;
        //                        errList.Add(LeaveError);
        //                        errList.Add(MinValue);
        //                        errList.Add(MaxValue);
        //                        string strerrlist = string.Empty;
        //                        for (int i = 0; i < errList.Count; i++)
        //                        {
        //                            if (strerrlist == string.Empty)
        //                                strerrlist = errList[i].ToString();
        //                            else
        //                                strerrlist += "-" + errList[i].ToString();
        //                        }
        //                        vaild = false;
        //                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('200004','','" + strerrlist + "');", true);

        //                        text.Focus();
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('200009');", true);
        //                    text.Focus();
        //                }
        //            }
        //        }
        //    }

        //    if (vaild)
        //    {
        //        if (text.ID.ToUpper() == "HEIGHT INCH" && InchFlag == true)
        //        {
        //            Control ctrHt = pnlVitals.FindControl("Height");
        //            RadTextBox ctrhtval = (RadTextBox)ctrHt;
        //            if (ctrhtval != null)
        //            {
        //                string feetInchValue = objUiManager.ConvertInchtoFeetInch(text.Text);
        //                string[] Splitter = { "'", "''" };
        //                string[] feetInch = feetInchValue.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
        //                if (feetInch.Length > 0)
        //                {
        //                    ctrhtval.Text = feetInch[0].Trim();
        //                    text.Text = feetInch[1].Trim();
        //                }
        //            }
        //        }

        //        if (htUnitConvMthds.Contains("BMI") && htUnitConvMthds["BMI"].ToString().Contains(text.ID))
        //        {
        //            Control ctr = pnlVitals.FindControl("BMI");
        //            RadTextBox ctrValue = (RadTextBox)ctr;
        //            if (ctr != null && ctr.ID != string.Empty)
        //            {
        //                ctrValue.Text = ConversionOnSave("BMI").Trim();
        //                aryNotesFilled.Add(ctrValue.ID.ToString());
        //            }
        //        }
        //        if (htUnitConvMthds.Contains("Blood Sugar-Fasting Status") && htUnitConvMthds["Blood Sugar-Fasting Status"].ToString().Contains(text.ID))
        //        {
        //            Control ctr = pnlVitals.FindControl("Blood Sugar-Fasting Status");
        //            Label ctrstatus = (Label)ctr;
        //            if (ctr != null && ctr.ID != string.Empty)
        //            {

        //                ctrstatus.Text = ConversionOnSave("Blood Sugar-Fasting Status").Trim();
        //                if (ctrstatus.Text != string.Empty)
        //                    aryLabelFilled.Add(ctrstatus.ID.ToString());
        //            }
        //        }
        //        else if (htUnitConvMthds.Contains("Blood Sugar-Fasting StatusSecond") && htUnitConvMthds["Blood Sugar-Fasting StatusSecond"].ToString().Contains(text.ID))
        //        {
        //            Control ctr = pnlVitals.FindControl("Blood Sugar-Fasting StatusSecond");
        //            Label ctrstatus = (Label)ctr;
        //            if (ctr != null && ctr.ID != string.Empty)
        //            {
        //                ctrstatus.Text = ConversionOnSave("Blood Sugar-Fasting StatusSecond").Trim();
        //                if (ctrstatus.Text != string.Empty)
        //                    aryLabelFilled.Add(ctrstatus.ID.ToString());
        //            }
        //        }
        //        if (htUnitConvMthds.Contains("Blood Sugar-Post Prandial Status") && htUnitConvMthds["Blood Sugar-Post Prandial Status"].ToString().Contains(text.ID))
        //        {
        //            Control ctr = pnlVitals.FindControl("Blood Sugar-Post Prandial Status");
        //            Label ctrstatus = (Label)ctr;
        //            if (ctr != null && ctr.ID != string.Empty)
        //            {
        //                ctrstatus.Text = ConversionOnSave("Blood Sugar-Post Prandial Status").Trim();
        //                if (ctrstatus.Text != string.Empty)
        //                    aryLabelFilled.Add(ctrstatus.ID.ToString());
        //            }
        //        }
        //        else if (htUnitConvMthds.Contains("Blood Sugar-Post Prandial StatusSecond") && htUnitConvMthds["Blood Sugar-Post Prandial StatusSecond"].ToString().Contains(text.ID))
        //        {
        //            Control ctr = pnlVitals.FindControl("Blood Sugar-Post Prandial StatusSecond");
        //            Label ctrstatus = (Label)ctr;
        //            if (ctr != null && ctr.ID != string.Empty)
        //            {
        //                ctrstatus.Text = ConversionOnSave("Blood Sugar-Post Prandial StatusSecond").Trim();
        //                if (ctrstatus.Text != string.Empty)
        //                    aryLabelFilled.Add(ctrstatus.ID.ToString());
        //            }
        //        }
        //    }
        //    if (htUnitConvMthds.Contains("HbA1C Status") && htUnitConvMthds["HbA1C Status"].ToString().Contains(text.ID))
        //    {
        //        Control ctr = pnlVitals.FindControl("HbA1C Status");
        //        Label ctrstatus = (Label)ctr;
        //        if (ctr != null && ctr.ID != string.Empty)
        //        {
        //            ctrstatus.Text = ConversionOnSave("HbA1C Status").Trim();
        //            if (ctrstatus.Text != string.Empty)
        //                aryLabelFilled.Add(ctrstatus.ID.ToString());


        //        }
        //    }
        //    else if (htUnitConvMthds.Contains("HbA1C StatusSecond") && htUnitConvMthds["HbA1C StatusSecond"].ToString().Contains(text.ID))
        //    {
        //        Control ctr = pnlVitals.FindControl("HbA1C StatusSecond");
        //        Label ctrstatus = (Label)ctr;
        //        if (ctr != null && ctr.ID != string.Empty)
        //        {
        //            ctrstatus.Text = ConversionOnSave("HbA1C StatusSecond").Trim();
        //            if (ctrstatus.Text != string.Empty)
        //                aryLabelFilled.Add(ctrstatus.ID.ToString());
        //        }
        //    }
        //    if (htUnitConvMthds.Contains("eGFR Status") && htUnitConvMthds["eGFR Status"].ToString().Contains(text.ID))
        //    {
        //        Control ctr = pnlVitals.FindControl("eGFR Status");
        //        Label ctrstatus = (Label)ctr;
        //        if (ctr != null && ctr.ID != string.Empty)
        //        {
        //            ctrstatus.Text = ConversionOnSave("eGFR Status").Trim();
        //            if (ctrstatus.Text != string.Empty)
        //                aryLabelFilled.Add(ctrstatus.ID.ToString());
        //        }
        //    }
        //    else if (htUnitConvMthds.Contains("eGFR StatusSecond") && htUnitConvMthds["eGFR StatusSecond"].ToString().Contains(text.ID))
        //    {
        //        Control ctr = pnlVitals.FindControl("eGFR StatusSecond");
        //        Label ctrstatus = (Label)ctr;
        //        if (ctr != null && ctr.ID != string.Empty)
        //        {
        //            ctrstatus.Text = ConversionOnSave("eGFR StatusSecond").Trim();
        //            if (ctrstatus.Text != string.Empty)
        //                aryLabelFilled.Add(ctrstatus.ID.ToString());
        //        }
        //    }
        //    if (htUnitConvMthds.Contains("HC Percentile") && htUnitConvMthds["HC Percentile"].ToString().Contains(text.ID))
        //    {
        //        Control ctr = pnlVitals.FindControl("HC Percentile");
        //        Label ctrstatus = (Label)ctr;
        //        if (ctr != null && ctr.ID != string.Empty)
        //        {
        //            ctrstatus.Text = ConversionOnSave("HC Percentile");
        //            if (ctrstatus.Text != string.Empty)
        //                aryLabelFilled.Add(ctrstatus.ID.ToString());
        //        }
        //    }
        //    EnableDisbaleSave(true);
        //    aryNotesFilled.Add(((RadTextBox)sender).ID.ToString());
        //}

        public string ConversionOnSave(string vitalName)
        {
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();

            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }
            string SaveErrorName = string.Empty;
            allEmpty = false;
            int j = 0; string retValue = string.Empty;
            Control emptyCtrl = new Control();
            ArrayList errList = new ArrayList();
            string MethdName = htUnitConvMthds[vitalName].ToString();
            string[] Splitter = { ".", "(", ",", ")" };
            string[] MthdInfo = MethdName.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
            if (MthdInfo.Length > 0)
            {
                string[] Arguments = new string[MthdInfo.Length - 2];
                string ClassName = MthdInfo[0];
                string MethodName = MthdInfo[1];
                for (int i = 2; i < MthdInfo.Length; i++)
                {
                    if (MthdInfo[i].ToUpper().Contains("BP"))
                    {
                        if ((MthdInfo[i].Contains("-")) || (MthdInfo[i].Contains("/")))
                        {
                            if (MthdInfo[i].Contains("-"))
                                MthdInfo[i] = MthdInfo[i].Replace("-", "");
                            if (MthdInfo[i].Contains("/"))
                                MthdInfo[i] = MthdInfo[i].Replace("/", "");
                            if (MthdInfo[i].Contains(" "))
                                MthdInfo[i] = MthdInfo[i].Replace(" ", "");
                        }
                        Control ctrl = pnlVitals.FindControl(MthdInfo[i]);
                        //RadNumericTextBox ctrl1 = (RadNumericTextBox)ctrl;
                        HtmlInputText ctrl1 = (HtmlInputText)ctrl;
                        Arguments[j] = Request.Form[ctrl1.ID.Trim()];
                        if (ctrl1.Value == string.Empty)
                        {
                            emptyCtrl = ctrl1;
                        }
                        j++;
                    }
                    else
                    {
                        Control ctrl = pnlVitals.FindControl(MthdInfo[i]);
                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                        {
                            HtmlInputText ctrl1 = (HtmlInputText)ctrl;
                            Arguments[j] = Request.Form[ctrl1.ID.Trim()]; //ctrl1.Value;
                            if (ctrl1.ID.Contains("Inch") && Arguments[j] == "0")
                            {
                                Arguments[j] = "";
                            }
                            if (ctrl1.Value == string.Empty)
                            {
                                emptyCtrl = ctrl1;
                            }
                            j++;
                        }
                        else if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                        {
                            HtmlTextArea ctrl1 = (HtmlTextArea)ctrl;
                            Arguments[j] = Request.Form[ctrl1.ID.Trim()]; //ctrl1.Value;
                            if (ctrl1.ID.Contains("Inch") && Arguments[j] == "0")
                            {
                                Arguments[j] = "";
                            }
                            if (ctrl1.Value == string.Empty)
                            {
                                emptyCtrl = ctrl1;
                            }
                            j++;
                        }
                    }
                }


                for (int i = 0; i < Arguments.Length; i++)
                {
                    if (Arguments[i] == string.Empty)
                        allEmpty = true;
                    else
                    {
                        allEmpty = false;
                        break;
                    }
                }
                if (vitalName.ToUpper() == "BMI")
                {
                    if (allEmpty == true)
                        return string.Empty;
                    else
                    {
                        string BMIInSave = objUiManager.InvokeMethod(ClassName, MethodName, Arguments);
                        if (BMIInSave == string.Empty)
                        {
                            allEmpty = true;
                        }
                        SetBMIStatus(BMIInSave);
                        return BMIInSave;
                    }
                }
                if (vitalName.ToUpper() == "HBA1C STATUS")
                {
                    return SetHBA1CStatus(Arguments[0]);
                }
                else if (vitalName.ToUpper() == "HBA1C STATUSSecond")
                {
                    return SetHBA1CStatus(Arguments[0]);
                }
                else if (vitalName.ToUpper() == "HGB STATUS")
                {
                    return SetHGBStatus(Arguments[0]);
                }
                else if (vitalName.ToUpper() == "HGB STATUSSecond")
                {
                    return SetHGBStatus(Arguments[0]);
                }
                else
                {
                    retValue = objUiManager.InvokeMethod(ClassName, MethodName, Arguments);
                    if (retValue == string.Empty && allEmpty == false && Arguments.Length != 1)
                    {
                        if (emptyCtrl.ID.Contains("Second"))
                        {
                            SaveErrorName = emptyCtrl.ID.Replace("Second", "");
                        }
                        else
                        {
                            SaveErrorName = emptyCtrl.ID;
                        }
                        string SaveError = string.Empty;
                        if (SaveErrorName.Contains("-"))
                            SaveError = SaveErrorName.Replace("-", " ");
                        else
                            SaveError = SaveErrorName;

                        errList.Add(SaveError);
                        string strerrlist = string.Empty;
                        for (int i = 0; i < errList.Count; i++)
                        {
                            if (strerrlist == string.Empty)
                                strerrlist = errList[i].ToString();
                            else
                                strerrlist += "-" + errList[i].ToString();
                        }
                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                            divLoading.Style.Add("display", "none");
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty,
                                "hideLoading();DisplayErrorMessage('200010','','" + strerrlist + "');", true);
                        emptyCtrl.Focus();
                    }
                }
            }
            return retValue;
        }
        public string ConcatenateBPValues(string sys, string Dia)
        {
            string Check = string.Empty;
            if (Dia == string.Empty || sys == string.Empty)
            {
                return Check;
            }
            else
            {
                Check = sys + "/" + Dia;
            }
            return Check;
        }
        public string ConcatenateVisionValues(string firs, string sec)
        {
            string Check = string.Empty;
            if (firs == string.Empty || sec == string.Empty)
            {
                return Check;
            }
            else
            {
                Check = firs + "/" + sec;
            }
            return Check;
        }
        public string CalculateBMIOnFtInchAndLBS(string HeightFt, string HeightInch, string Weight)
        {

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();

            //    XmlNodeList xmlHeight = itemDoc.GetElementsByTagName("VitalsHeight");
            //    if (xmlHeight != null && xmlHeight.Count>0)
            //    {
            //        xmlHeight[0].Attributes[0].Value = HeightFt;
            //        xmlHeight[0].Attributes[1].Value = HeightInch;
            //        itemDoc.Save(strXmlFilePath);
            //    }
            //    else
            //    {
            //        XmlNode Newnode = itemDoc.CreateNode(XmlNodeType.Element, "VitalsHeight", "");

            //        XmlAttribute attFeet = itemDoc.CreateAttribute("Feet");
            //        attFeet.Value = HeightFt;

            //        XmlAttribute attInches = itemDoc.CreateAttribute("Inches");
            //        attInches.Value = HeightInch;

            //        Newnode.Attributes.Append(attFeet);
            //        Newnode.Attributes.Append(attInches);

            //        XmlNodeList xmlSectionList = itemDoc.GetElementsByTagName("Modules");
            //        if (xmlSectionList != null && xmlSectionList.Count > 0)
            //           xmlSectionList[0].AppendChild(Newnode);

            //        itemDoc.Save(strXmlFilePath);

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();

            //    XmlNodeList xmlHeight = itemDoc.GetElementsByTagName("VitalsHeight");
            //    if (xmlHeight != null && xmlHeight.Count>0)
            //    {
            //        xmlHeight[0].Attributes[0].Value = HeightFt;
            //        xmlHeight[0].Attributes[1].Value = HeightInch;
            //        itemDoc.Save(strXmlFilePath);
            //    }
            //    else
            //    {
            //        XmlNode Newnode = itemDoc.CreateNode(XmlNodeType.Element, "VitalsHeight", "");

            //        XmlAttribute attFeet = itemDoc.CreateAttribute("Feet");
            //        attFeet.Value = HeightFt;

            //        XmlAttribute attInches = itemDoc.CreateAttribute("Inches");
            //        attInches.Value = HeightInch;

            //        Newnode.Attributes.Append(attFeet);
            //        Newnode.Attributes.Append(attInches);

            //        XmlNodeList xmlSectionList = itemDoc.GetElementsByTagName("Modules");
            //        if (xmlSectionList != null && xmlSectionList.Count > 0)
            //           xmlSectionList[0].AppendChild(Newnode);



            //    }
            //} 

            //    }
            //} 

            decimal BMI = 0;
            decimal HtVal = 0;
            decimal WtVal = 0;
            string sHtval = string.Empty;
            try
            {
                if (HeightFt != string.Empty)
                    sHtval = objUiManager.ConvertFeetInchToInch(HeightFt, HeightInch);
                if (sHtval != string.Empty)
                    HtVal = Convert.ToDecimal(sHtval);
                else
                    return string.Empty;
                if (Weight != string.Empty)
                    WtVal = Convert.ToDecimal(Weight);
                else
                    return string.Empty;
                BMI = decimal.Round(((WtVal / (HtVal * HtVal)) * 703m), 1);
                if (BMI != 0)
                    return BMI.ToString();
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public string SetHCPercentile(string sHC)
        {
            IList<PercentileLookUp> percentileListForHC = new List<PercentileLookUp>();
            if (Session["percentileListForHC"] != null)
                percentileListForHC = (IList<PercentileLookUp>)Session["percentileListForHC"];
            double percentileIncrement = 0;
            if (Session["percentileIncrement"] != null)
                percentileIncrement = Convert.ToDouble(Session["percentileIncrement"]);
            if (sHC != string.Empty)
            {
                if (percentileListForHC != null && percentileListForHC.Count > 0)
                {
                    PercentileLookUp objFirst = new PercentileLookUp();
                    PercentileLookUp objSecond = new PercentileLookUp();
                    if (percentileListForHC.Count > 0)
                        objFirst = percentileListForHC[0];
                    if (percentileListForHC.Count > 1)
                        objSecond = percentileListForHC[1];
                    double L = ((percentileIncrement * objFirst.L) + (1 - percentileIncrement) * objSecond.L);
                    double M = ((percentileIncrement * objFirst.M) + (1 - percentileIncrement) * objSecond.M);
                    double S = ((percentileIncrement * objFirst.S) + (1 - percentileIncrement) * objSecond.S);
                    string sHCinCM = objUiManager.ConvertInchesToCM(sHC);
                    double X = Convert.ToDouble(sHCinCM);
                    double z_score = (Math.Pow((X / M), L) - 1) / (L * S);
                    Microsoft.Office.Interop.Excel.Application xl = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.WorksheetFunction iwf = xl.WorksheetFunction;
                    double normDist = iwf.NormSDist(z_score);
                    double percentile = Math.Round((normDist * 1000) / 10, 2);
                    return "HC %tile: " + percentile.ToString();
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        public string SetHBA1CStatus(string hbValue)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "HBA1C STATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("HBA1C STATUS");
            }
            string HB = string.Empty;
            try
            {

                if (hbValue != string.Empty)
                {
                    decimal hbA1C = Convert.ToDecimal(hbValue);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            string[] split = rangeText.Split('-');
                            if (split.Length == 2)
                            {
                                if (hbA1C >= Convert.ToDecimal(split[0]) && hbA1C <= Convert.ToDecimal(split[1]))
                                {
                                    HB = defValues[i].Description;
                                    break;
                                }
                            }
                            else
                            {
                                if (hbA1C >= Convert.ToDecimal(split[0]))
                                {
                                    HB = defValues[i].Description;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                    HB = string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return HB;
        }
        public string SetUrineforMicroalbuminStatus(string hbValue)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (ViewState["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)ViewState["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "URINE FOR MICROALBUMINSTATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("URINE FOR MICROALBUMINSTATUS");
            }
            string HB = string.Empty;
            try
            {

                if (hbValue != string.Empty)
                {
                    decimal hbA1C = Convert.ToDecimal(hbValue);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            string[] split = rangeText.Split('-');
                            if (split.Length == 2)
                            {
                                if (hbA1C >= Convert.ToDecimal(split[0]) && hbA1C <= Convert.ToDecimal(split[1]))
                                {
                                    HB = defValues[i].Description;
                                    break;
                                }
                            }
                            else
                            {
                                if (hbA1C >= Convert.ToDecimal(split[0]))
                                {
                                    HB = defValues[i].Description;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                    HB = string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return HB;
        }
        public string SetABITestStatus(string hbValue)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (ViewState["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)ViewState["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "ABI TESTSTATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("ABI TESTSTATUS");
            }
            string HB = string.Empty;
            try
            {
                if (hbValue != string.Empty)
                {
                    decimal hbA1C = Convert.ToDecimal(hbValue);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            if (rangeText.Contains("-"))
                            {
                                string[] split = rangeText.Split('-');
                                if (split.Length == 2)
                                {
                                    if (hbA1C >= Convert.ToDecimal(split[0]) && hbA1C <= Convert.ToDecimal(split[1]))
                                    {
                                        HB = defValues[i].Description;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (hbA1C >= Convert.ToDecimal(split[0]))
                                    {
                                        HB = defValues[i].Description;
                                        break;
                                    }
                                }
                            }
                            else if (rangeText.Contains("<"))
                            {
                                string[] split = rangeText.Split('<');
                                if (hbA1C <= Convert.ToDecimal(split[1]))
                                {
                                    HB = defValues[i].Description;
                                    break;
                                }
                            }
                            else if (rangeText.Contains(">"))
                            {
                                string[] split = rangeText.Split('>');
                                if (hbA1C >= Convert.ToDecimal(split[1]))
                                {
                                    HB = defValues[i].Description;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                    HB = string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return HB;
        }
        public void SetBMIStatus(string sBMI)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "BMI STATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("BMI STATUS");
            }
            Control bmiStat = pnlVitals.FindControl("BMI Status");
            HtmlInputText bmiStatus = (HtmlInputText)bmiStat;
            // HtmlTextArea bmiStatus = (HtmlTextArea)bmiStat;
            Control bmivalue = pnlVitals.FindControl("BMI");
            IList<PercentileLookUp> percentileListForBMI = new List<PercentileLookUp>();
            if (Session["percentileListForBMI"] != null)
                percentileListForBMI = (IList<PercentileLookUp>)Session["percentileListForBMI"];
            double percentileIncrement = 0;
            if (Session["percentileIncrement"] != null)
                percentileIncrement = Convert.ToDouble(Session["percentileIncrement"]);
            if (bmiStat != null && bmiStat.ID != null)
            {
                if (sBMI != string.Empty)
                {
                    decimal dbmi = Convert.ToDecimal(sBMI);

                    if (!(percentileListForBMI.Count > 0))
                    {
                        if (defValues != null && defValues.Count > 0)
                        {
                            for (int i = 0; i < defValues.Count; i++)
                            {
                                string rangeText = defValues[i].Value;
                                string[] split = rangeText.Split('-');
                                if (split.Length == 2)
                                {
                                    if (dbmi >= Convert.ToDecimal(split[0]) && dbmi <= Convert.ToDecimal(split[1]))
                                    {
                                        bmiStatus.Value = defValues[i].Description;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (dbmi >= Convert.ToDecimal(split[0]))
                                    {
                                        bmiStatus.Value = defValues[i].Description;
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    bmiStatus.Style.Add("color", Convert.ToString(SetColorForStatus("BMI STATUS", bmiStatus.Value)));
                }
                else
                    bmiStatus.Value = string.Empty;
            }
        }

        private string SetColorForStatus(string Vital_Status, string Value)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> iFieldLookupList = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                iFieldLookupList = StaticLookUpList.Where(q => q.Field_Name == "VITALS LIMIT LEVEL").ToList();
            }
            else
            {
                iFieldLookupList = staticLookupManager.getStaticLookupByFieldName("VITALS LIMIT LEVEL");
            }
            Color colr = Color.Red;
            if (iFieldLookupList != null)
            {
                IList<string> sList = (from h in iFieldLookupList where h.Value.ToUpper() == Vital_Status.ToUpper() select h.Description).ToList<string>();
                if (sList != null)
                {
                    if (sList.Count > 0)
                    {
                        if ((Vital_Status.Trim().IndexOf("Hgb") == 0) && Value.ToUpper().Trim().Equals(sList[0].ToUpper()))
                        {
                            colr = Color.Black;
                        }
                        else if ((Vital_Status.Trim().IndexOf("Hgb") != 0) && Value.ToUpper().Trim().Contains(sList[0].ToUpper()))
                            colr = Color.Black;
                    }
                }
            }
            return colr.Name;
        }
        public string SetFastingBloodSugarStatus(string sugarValue)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "BLOOD SUGAR-FASTING STATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("BLOOD SUGAR-FASTING STATUS");
            }
            string finalSugar = string.Empty;
            try
            {
                if (sugarValue != string.Empty)
                {
                    decimal sugar = Convert.ToDecimal(sugarValue);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            string[] split = rangeText.Split('-');
                            if (split.Length == 2)
                            {
                                if (sugar >= Convert.ToDecimal(split[0]) && sugar <= Convert.ToDecimal(split[1]))
                                {
                                    finalSugar = defValues[i].Description;
                                    break;
                                }
                            }
                            else
                            {
                                if (sugar >= Convert.ToDecimal(split[0]))
                                {
                                    finalSugar = defValues[i].Description;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                    finalSugar = string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return finalSugar;
        }
        public string SetGFRStatus(string sGFR)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "GFR STATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("GFR STATUS");
            }
            string GFRStatus = string.Empty;
            try
            {
                if (sGFR != string.Empty)
                {
                    decimal dGFR = Convert.ToDecimal(sGFR);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            string[] split = rangeText.Split('-');
                            if (split.Length == 2)
                            {
                                if (dGFR >= Convert.ToDecimal(split[0]) && dGFR <= Convert.ToDecimal(split[1]))
                                {
                                    GFRStatus = defValues[i].Description;
                                    break;
                                }
                            }
                            else
                            {
                                if (dGFR >= Convert.ToDecimal(split[0]))
                                {
                                    GFRStatus = defValues[i].Description;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                    GFRStatus = string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return GFRStatus;

        }
        public string SetPostPrandialBloodSugarStatus(string sugarValue)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "BLOOD SUGAR-POST PRANDIAL STATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("BLOOD SUGAR-POST PRANDIAL STATUS");
            }
            string finalSugar = string.Empty;
            try
            {

                if (sugarValue != string.Empty)
                {
                    decimal sugar = Convert.ToDecimal(sugarValue);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            string[] split = rangeText.Split('-');
                            if (split.Length == 2)
                            {
                                if (sugar >= Convert.ToDecimal(split[0]) && sugar <= Convert.ToDecimal(split[1]))
                                {
                                    finalSugar = defValues[i].Description;
                                    break;
                                }
                            }
                            else
                            {
                                if (sugar >= Convert.ToDecimal(split[0]))
                                {
                                    finalSugar = defValues[i].Description;
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                    finalSugar = string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return finalSugar;
        }
        private PatientResults CreateVitalsObject(ulong ulGroupID)
        {

            string openingfrom = Request["openingfrom"].ToString();
            PatientResults vitalsObj = new PatientResults();
            vitalsObj.Human_ID = HumanID;// ClientSession.HumanId;
            vitalsObj.Physician_ID = ClientSession.PhysicianId;
            if (openingfrom == "Menu")
                vitalsObj.Encounter_ID = 0;
            else
                vitalsObj.Encounter_ID = ClientSession.EncounterId;
            vitalsObj.Created_By = ClientSession.UserName;
            //vitalsObj.Created_Date_And_Time = utc;
            vitalsObj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            vitalsObj.Modified_By = ClientSession.UserName;
            //vitalsObj.Modified_Date_And_Time = utc;
            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            vitalsObj.Vitals_Group_ID = ulGroupID;
            vitalsObj.Results_Type = "Vitals";
            return vitalsObj;
        }
        private string GetCheckedValues(string vitalcheck)
        {
            try
            {
                CheckBox chkYes = (CheckBox)pnlVitals.FindControl(vitalcheck);
                CheckBox chkNo = (CheckBox)pnlVitals.FindControl("chk" + vitalcheck);

                if (chkYes.Checked)
                {
                    return "Y";
                }
                else if (chkNo.Checked)
                {
                    return "N";
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //  ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            string strtime = hdnLocalTime.Value.ToString().Split('G').ElementAt(0).ToString();
            utc = Convert.ToDateTime(strtime);

            IList<PatientResults> vitalList = new List<PatientResults>();
            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }
            EnableDisbaleSave(false);
            string openingfrom = string.Empty;
            openingfrom = Request["openingfrom"].ToString();
            //  if (Validation())
            // {
            if (openingfrom == "Menu")
            {
                if (btnSaveVitals.Value == "Add")
                {
                    AddImportVitals();
                }
                else if (btnSaveVitals.Value == "Update")
                {
                    UpdateImportVitals();
                }
            }
            else
            {
                if (btnSaveVitals.Value == "Save")
                {
                    if (vitalList.Count == 0)
                    {
                        AddVitals();
                    }
                    else
                    {
                        UpdateVitals();
                    }

                }
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
            hdnbuttonName.Value = "btnSave";
            hdnRequestDate.Value = Request.Cookies["VitalCurrentDate"].Value;
            //}
            //else
            //{
            //    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
            //        divLoading.Style.Add("display", "none");
            //    else
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //    // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
            //}
            //if (openingfrom != "Menu")
            //{
            //    List<string> lstToolTips = new List<string>();
            //    var vitalText = new UtilityManager().LoadPatientSummary(2, out lstToolTips);
            //    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "summaryMsg",
            //        "SetVitalsText('" + vitalText + "','" + lstToolTips[1] + "','" + lstToolTips[0] + "');", true);
            //}
            //CAP-967
            if (hdnType.Value == "Yes")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenValue", " closePopup();", true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void AddVitals()
        {
            ulong GroupID = 0;
            DateTime VitalTakenDate = DateTime.MinValue;
            DateTime VitalTakenDateValue = DateTime.MinValue;
            string VitalName = string.Empty;
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();
            ArrayList vitalArray = new ArrayList();
            PatientResults vitalsObj = new PatientResults();
            PatientResultsDTO objVitalDTO = null;
            IList<PatientResults> vitalList = new List<PatientResults>();
            IList<PatientResults> saveList = new List<PatientResults>();
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            IList<DynamicScreen> dynamicScreenListforCodes = new List<DynamicScreen>();
            IList<MasterVitals> mastervitalslist = new List<MasterVitals>();
            IList<MasterVitals> mastervitalslistforUnits = new List<MasterVitals>();
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            DateTime BirthDate = DateTime.MinValue;
            string humanSex = string.Empty;
            double humanAgeInMonths = 0;
            ulong ScreenID = 2000;

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                BirthDate = ClientSession.PatientPaneList[0].Birth_Date;
                humanSex = ClientSession.PatientPaneList[0].Sex;
            }
            if (hdnSystemTime.Value.Trim() != string.Empty)
                humanAgeInMonths = ((DateTime.ParseExact(hdnSystemTime.Value, "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            else if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
            {
                if (Request["openingfrom"] != "Menu")
                    humanAgeInMonths = ((UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null)).Date - BirthDate.Date).TotalDays) / 30.4375;
                else
                    humanAgeInMonths = ((DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            }
            else
                humanAgeInMonths = ((DateTime.Now.Date - BirthDate.Date).TotalDays) / 30.4375;

            if (Session["GroupID"] != null)
            {
                GroupID = (ulong)Session["GroupID"];
            }
            if (Session["vitalArray"] != null)
            {
                vitalArray = (ArrayList)Session["vitalArray"];
            }
            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }

            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }

            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }

            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);

            if (Session["VitalTakenDate"] != null)
                VitalTakenDate = Convert.ToDateTime(Session["VitalTakenDate"]);

            if (Session["VitalTakenDateValue"] != null)
                VitalTakenDateValue = Convert.ToDateTime(Session["VitalTakenDateValue"]);
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            if (Session["dynamicScreenList"] != null)
                dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];

            if (Session["mastervitalslist"] != null)
                mastervitalslist = (IList<MasterVitals>)Session["mastervitalslist"];

            foreach (object o in vitalArray)
            {
                string name = o.ToString();
                string sLoincname = name;
                // if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")) && (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")))
                {
                    if ((name.Contains("-")) || (name.Contains("/")))
                    {
                        if (name.Contains("-"))
                            name = name.Replace("-", "");
                        if (name.Contains("/"))
                            name = name.Replace("/", "");
                        if (name.Contains(" "))
                            name = name.Replace(" ", "");
                    }
                }
                Control ctrDtp = pnlVitals.FindControl(("dtp" + name).Contains(" ") ? "dtp" + name.Replace(" ", "") : "dtp" + name);

                if (ctrDtp != null)
                {
                    HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
                    if (dtpInAdd != null)
                    {
                        // Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_" + dtpInAdd.ID);
                        // if (ctrDtpTime != null)
                        //  {
                        //  MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                        // // if (dtpInAddTime != null)
                        // {
                        //string[] ctrldtpChangedvalue = Request.Form[dtpInAdd.ID.Trim()].Split('-');
                        // string[] ctrldtpChangedvalue = Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]).ToString("yyyy-MM-dd").Split('-');
                        // if (ctrldtpChangedvalue.Length > 0)
                        //{
                        //string Hour = Request.Form[dtpInAddTime.ID];
                        //string Minute = Request.Form[dtpInAddTime.ID + "_txtMinute"];
                        //string AMPM = Request.Form[dtpInAddTime.ID + "_txtAmPm"];
                        //DateTime temp = new DateTime(Convert.ToInt32(ctrldtpChangedvalue[0]), Convert.ToInt32(ctrldtpChangedvalue[1]), Convert.ToInt32(ctrldtpChangedvalue[2]), Convert.ToInt32(Hour), Convert.ToInt32(Minute), Convert.ToInt32(00));
                        //string temp1 = string.Empty;
                        //if (temp.ToString().Contains("AM"))
                        //    temp1 = temp.ToString().Replace("AM", "");
                        //else if (temp.ToString().Contains("PM"))
                        //    temp1 = temp.ToString().Replace("PM", "");
                        //else
                        //    temp1 = temp.ToString();
                        try
                        {
                            VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]));
                        }
                        catch
                        {
                            VitalTakenDate = VitalTakenDate;
                        }
                        // }
                        // }
                        // }
                    }
                }
                else if (name.ToUpper().Contains("STATUS"))
                {
                    VitalTakenDate = VitalTakenDate;
                }
                else
                {
                    VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(VitalTakenDateValue.ToString("dd-MMM-yyyy hh:mm tt")));
                }
                if (name.ToUpper().Contains("BLOOD"))
                {
                    name = sLoincname;
                }

                Control ctrl = pnlVitals.FindControl(name);
                if (ctrl == null)
                {
                    ctrl = pnlVitals.FindControl(name.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                }
                name = sLoincname;
                if (ctrl != null && ctrl.ID != string.Empty)
                {
                    //if (ctrl.GetType().ToString().ToUpper().Contains("CUSTOMDATETIMEPICKER") == true)
                    //{
                    //    CustomDateTimePicker dtpDate = (CustomDateTimePicker)ctrl;
                    //    if (dtpDate.cboDate.Text != string.Empty && dtpDate.cboMonth.Text != string.Empty && dtpDate.cboYear.Text != string.Empty)
                    //    {

                    //    }
                    //}

                    CustomDLCNew cb = null;


                    if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD") || name.ToUpper().Contains("HOLTER") ||
                        name.ToUpper().Contains("ULTRASOUND"))) // && (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                    {
                        if ((name.Contains("-")) || (name.Contains("/")))
                        {
                            if (name.Contains("-"))
                                name = name.Replace("-", "");
                            if (name.Contains("/"))
                                name = name.Replace("/", "");
                            if (name.Contains(" "))
                                name = name.Replace(" ", "");
                        }
                    }
                    Control ctrl2 = pnlVitals.FindControl("txtNotes" + name.Replace(" ", "").Replace("CDP", "").Replace("-", ""));
                    if (ctrl2 != null && ctrl2.ID != string.Empty)
                    {
                        cb = (CustomDLCNew)ctrl2;
                    }
                    // HtmlSelect reasonnotPerformed = null;
                    //Control ctrlreason = pnlVitals.FindControl("Patient-Reason-refused-" + name.Replace(" ", "").Replace("-", ""));
                    //if (ctrlreason != null && ctrlreason.ID != string.Empty)
                    //{
                    //    reasonnotPerformed = (HtmlSelect)ctrlreason;
                    //}


                    name = sLoincname;
                    vitalsObj = CreateVitalsObject(GroupID + 1);

                    vitalsObj.Loinc_Observation = name;
                    vitalsObj.Captured_date_and_time = VitalTakenDate;
                    vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                    if (ctrl.GetType().ToString().ToUpper().Contains("CHECKBOX") == true)
                    {
                        vitalsObj.Value = GetCheckedValues(name).Trim();
                        var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                        if (DynamicList.Count() > 0)
                        {
                            dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                            if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                            {
                                vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                            }
                        }
                        VitalName = string.Empty;
                        VitalName = vitalsObj.Loinc_Observation;
                        if (VitalName.Contains("Second"))
                        {
                            VitalName = VitalName.Replace("Second", "");
                        }

                        var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                        if (MasterList.Count() > 0)
                        {
                            mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                            if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                            {
                                vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;

                            }
                        }
                        if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                        {
                            if ((cb != null && Request.Form[cb.ID + "$txtDLC"] == string.Empty) || (cb == null))
                                // && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                continue;
                        }
                        else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false)
                        {
                            if (VitalName.Contains("eGFR Status"))
                            {
                                string egfrvalue = Request.Form["eGFR"].Trim();
                                if (egfrvalue != string.Empty)
                                {

                                }
                                else
                                {
                                    EnableDisbaleSave(true);
                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                        divLoading.Style.Add("display", "none");
                                    else
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                    // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                    return;
                                }
                            }
                            else if (VitalName.Contains("eGFR StatusSecond"))
                            {
                                string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                if (egfrvalue != string.Empty)
                                {

                                }
                                else
                                {
                                    EnableDisbaleSave(true);
                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                        divLoading.Style.Add("display", "none");
                                    else
                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                    return;
                                }
                            }
                            else
                            {
                                EnableDisbaleSave(true);
                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                    divLoading.Style.Add("display", "none");
                                else
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (htUnitConvMthds.Contains(name))
                        {
                            vitalsObj.Value = ConversionOnSave(name).Trim();

                            var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                            if (DynamicList.Count() > 0)
                            {
                                dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                                if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                {
                                    if (name.ToUpper() == "BMI")
                                    {
                                        // string status = Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString();
                                        if (Request.Form[pnlVitals.FindControl("BMI Status").ID] != null && dynamicScreenListforCodes[0].Snomed_Code != "" && Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString() != "")
                                        {
                                            string[] code = dynamicScreenListforCodes[0].Snomed_Code.Split('|');
                                            for (int y = 0; y < code.Length; y++)
                                            {
                                                if (code[y].Split(':').Length > 1)
                                                {
                                                    if (Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString().ToUpper().Contains(code[y].Split(':')[0]))
                                                    {
                                                        vitalsObj.Snomed_Code = code[y].Split(':')[1];
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            vitalsObj.Snomed_Code = dynamicScreenList[0].Snomed_Code;
                                        }
                                    }
                                    else
                                    {
                                        vitalsObj.Snomed_Code = dynamicScreenList[0].Snomed_Code;
                                    }

                                    vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                    vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                    vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                }
                            }
                            VitalName = string.Empty;
                            VitalName = vitalsObj.Loinc_Observation;
                            if (VitalName.Contains("Second"))
                            {
                                VitalName = VitalName.Replace("Second", "");
                            }
                            var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                            if (MasterList.Count() > 0)
                            {
                                mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                                if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                {
                                    vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;

                                }
                            }
                            if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                            {
                                if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))
                                    //&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                    continue;
                            }
                            else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false)
                            {
                                if (VitalName.Contains("eGFR Status"))
                                {
                                    string egfrvalue = Request.Form["eGFR"].Trim();
                                    if (egfrvalue != string.Empty)
                                    {

                                    }
                                    else
                                    {
                                        EnableDisbaleSave(true);
                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                            divLoading.Style.Add("display", "none");
                                        else
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                        // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                        return;
                                    }
                                }
                                else if (VitalName.Contains("eGFR StatusSecond"))
                                {
                                    string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                    if (egfrvalue != string.Empty)
                                    {

                                    }
                                    else
                                    {
                                        EnableDisbaleSave(true);
                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                            divLoading.Style.Add("display", "none");
                                        else
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                        // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    EnableDisbaleSave(true);
                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                        divLoading.Style.Add("display", "none");
                                    else
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                                    // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                            {
                                HtmlGenericControl ctrllbl = (HtmlGenericControl)ctrl;
                                if (Request.Form[ctrllbl.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrllbl.ID.Trim()];
                            }
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                            {
                                HtmlInputText ctrltxt = (HtmlInputText)ctrl;
                                if (Request.Form[ctrltxt.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                else if (ctrltxt != null)
                                    vitalsObj.Value = ctrltxt.Value;
                            }
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                            {
                                HtmlTextArea ctrltxt = (HtmlTextArea)ctrl;
                                if (Request.Form[ctrltxt.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                            }
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                            {
                                HtmlSelect ctrlcmb = (HtmlSelect)ctrl;
                                if (Request.Form[ctrlcmb.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrlcmb.ID.Trim()];
                                //vitalsObj.Value = ctrlcmb.Items[ctrlcmb.SelectedIndex].Text;
                            }
                            //if (ctrl.GetType().Name.ToUpper().Contains("RADMASKEDTEXTBOX"))
                            //{

                            //    RadMaskedTextBox ctrlcmb = ((RadMaskedTextBox)pnlVitals.FindControl(name));
                            //    string[] aryFromDate = Request.Form[ctrlcmb.ID].Replace("____", "").Replace("___", "").Replace("__", "").Split('-');
                            //    if (aryFromDate[2] != "" && aryFromDate[1] != "" && aryFromDate[0] != "")
                            //    {
                            //        vitalsObj.Value = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];
                            //    }
                            //    else if (aryFromDate[0] != "" && aryFromDate[1] != "" && aryFromDate[2] == "")
                            //    {
                            //        vitalsObj.Value = aryFromDate[1] + "-" + aryFromDate[0];
                            //    }
                            //    else if (aryFromDate[0] != "" && aryFromDate[1] == "" && aryFromDate[2] == "")
                            //    {
                            //        vitalsObj.Value = aryFromDate[0];
                            //    }
                            //    else
                            //    {
                            //        vitalsObj.Value = "";
                            //    }

                            //}

                            var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                            if (DynamicList.Count() > 0)
                            {
                                dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                                if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                {
                                    vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                    vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                    vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                }
                            }
                            VitalName = string.Empty;
                            VitalName = vitalsObj.Loinc_Observation;
                            if (VitalName.Contains("Second"))
                            {
                                VitalName = VitalName.Replace("Second", "");
                            }
                            var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                            if (MasterList.Count() > 0)
                            {
                                mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                                if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                {
                                    vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;

                                }
                            }
                            if (vitalsObj.Value.Trim() == string.Empty)
                            {
                                // if ((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null))
                                if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    if (cb != null)
                    {
                        vitalsObj.Notes = Request.Form[cb.ID + "$txtDLC"].Trim();
                        string snomedcode = "";
                        string loinc_identifier = "";
                        //BugID:47706
                        if (vitalsObj.Notes != "" && cb.ID.IndexOf("LastMammogram") == -1)
                            snomedcode = objUtilityManager.GetSnomedfromStaticLookupvitals("ReasonNotPerformedList", cb.TextControlID.ToString(), cb.txtDLC.Text.ToString());
                        else
                            loinc_identifier = objUtilityManager.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", cb.txtDLC.Text.ToString());

                        if (cb.ID.IndexOf("LastMammogram") != -1)
                            vitalsObj.Loinc_Identifier = loinc_identifier;

                        //string[] notes = Request.Form[cb.ID + "$txtDLC"].Trim().Split(',');
                        //for (int h = 0; h < notes.Length; h++)
                        //{
                        //    var test = (from m in ilstLookupRefuse where m.Value == notes[h].Trim() select m.Description).ToArray();
                        //    if (test.Count() > 0)
                        //    {
                        //        if (h == 0)
                        //            snomedcode = test[0] + ",";
                        //        else if (h == notes.Length - 1)
                        //            snomedcode = snomedcode + test[0] + ",";
                        //        else
                        //            snomedcode = snomedcode + test[0];


                        //    }

                        //}
                        if (cb.ID.ToUpper().Contains("HEIGHT") || cb.ID.ToUpper().Contains("WEIGHT") || cb.ID.ToUpper().Contains("BPSITTINGSYSDIA"))
                        {
                            if (snomedcode == "" && vitalsObj.Value == "")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "displaysnomedalert('" + cb.TextControlID.ToString() + "')", true);
                                return;
                            }
                        }
                        vitalsObj.Snomed_Code = snomedcode;
                    }
                    //if (reasonnotPerformed != null)
                    //{
                    //    vitalsObj.Reason_For_Not_Performed = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text;
                    //    vitalsObj.Snomed_Code = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Value;
                    //}
                    //Added Is_Sent_to_Rcopia for Bug ID:33607
                    vitalsObj.Is_Sent_to_Rcopia = "N";
                    //To create object for BP Location
                    if (name.ToUpper().Contains("BP") && name.ToUpper().IndexOf("STATUS") <= -1 && (vitalsObj.Value != string.Empty || (cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() != "")))
                    // (reasonnotPerformed != null && reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text != "")))
                    {
                        string[] split = name.Split(' ');
                        if (split != null && split.Length > 0)
                        {
                            try
                            {
                                //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(split[0] + " Left");
                                //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(split[0] + " Right");
                                RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(split[0] + " Left");
                                RadioButton chkRight = (RadioButton)pnlVitals.FindControl(split[0] + " Right");
                                PatientResults vitalsLocationObj = CreateVitalsObject(GroupID + 1);
                                vitalsLocationObj.Loinc_Observation = split[0] + " Location";
                                vitalsLocationObj.Captured_date_and_time = VitalTakenDate;

                                vitalsLocationObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                //Added Is_Sent_to_Rcopia for Bug ID:33607
                                vitalsLocationObj.Is_Sent_to_Rcopia = "N";
                                //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                //{
                                //    vitalsLocationObj.Value = "Left";
                                //}
                                //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                //{
                                //    vitalsLocationObj.Value = "Right";
                                //}
                                if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                {
                                    vitalsLocationObj.Value = "Left";
                                }
                                else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                {
                                    vitalsLocationObj.Value = "Right";
                                }
                                saveList.Add(vitalsLocationObj);
                            }
                            catch
                            {
                            }
                        }
                    }
                    saveList.Add(vitalsObj);
                }
            }



            if (saveList != null)
            {
                if (saveList.Count > 0)
                {
                    objVitalDTO = vitalmngr.AddVitalDetails(saveList.ToArray<PatientResults>(), saveList[0].Human_ID, 1, 20, string.Empty, saveList[0].Encounter_ID, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, ClientSession.UserName);//BugID:53796
                    EnableDisbaleSave(false);
                    // if (objVitalDTO != null && objVitalDTO.sNotification.Trim() != string.Empty)
                    //   {
                    // hdnNotification.Value = "true";
                    //ClientSession.FillPatientChart.Notification = objVitalDTO.sNotification;
                    //objVitalDTO.sNotification.Replace("\r\n", "$");
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenNotificationSuccessfully();", true);
                    //  }
                    //  else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeLabel", "ChangeStatusLabel();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "savedSuccessfully();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "showhidBP();enabledisableBMI();", true);

                }
            }
            //CAP-790 - Object reference not set to an instance of an object.
            if (objVitalDTO?.VitalsList == null)
            {
                if (saveList != null && saveList.Count > 0 && saveList[0].Encounter_ID == 0)
                {
                    objVitalDTO = vitalmngr.GetPastVitalDetailsByPatient(saveList[0].Human_ID, 1, 20, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID);
                }
                else if (saveList != null && saveList.Count > 0)
                {
                    objVitalDTO = vitalmngr.GetPastVitalDetailsByEncounterID(saveList[0].Encounter_ID, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, saveList[0].Human_ID);
                }
            }
            //Jira cap355 - BP values missing in summary, notes
            //if (objVitalDTO == null && objVitalDTO.VitalsList == null)
            if (objVitalDTO != null && objVitalDTO.VitalsList != null)
            vitalList = objVitalDTO.VitalsList;

            DataSet dsGetVitals = new DataSet();
            string Loinc_observation = string.Empty;
            // if (Session["Loinc_observation"] != null)
            // Loinc_observation = Session["Loinc_observation"].ToString();
            if (objVitalDTO.dsLatestResults.Tables.Count > 0)
            {
                dsGetVitals = objVitalDTO.dsLatestResults;
                if (dsGetVitals != null)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt = dsGetVitals.Tables[0];
                    if (Loinc_observation != string.Empty)
                    {
                        Loinc_observation = string.Empty;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Loinc_observation.Trim() == string.Empty)
                        {
                            Loinc_observation = dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                        }
                        else
                        {
                            Loinc_observation += ", " + dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                        }
                    }
                }
            }
            Session["vitalList"] = vitalList;
            EditCellClickforVitals(false);
            Session["objVitalDTO"] = objVitalDTO;
            Session["Loinc_observation"] = Loinc_observation;

            hdnLabResults.Value = Loinc_observation;
            //SetVitalsToolTip(Loinc_observation);

            SetVitalsToolTip(vitalList);


            //if (hdnNotification.Value == "true")
            //{
            //    RadScriptManager1.AsyncPostBackErrorMessage = RadScriptManager1.AsyncPostBackErrorMessage.ToString().Replace("true", "trueNotification");
            //}
            btnSaveVitals.Disabled = true;
            string openingfrom = string.Empty;
            openingfrom = Request["openingfrom"].ToString();

        }
        public void AddImportVitals()
        {
            string sForClose = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            string VitalName = string.Empty;
            ulong GroupID = 0;
            DateTime VitalTakenDate = DateTime.MinValue;
            DateTime VitalTakenDateValue = DateTime.MinValue;
            PatientResults vitalsObj = new PatientResults();
            PatientResultsDTO objVitalDTO = null;
            ArrayList vitalArray = new ArrayList();
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();
            IList<PatientResults> vitalList = new List<PatientResults>();
            IList<PatientResults> saveList = new List<PatientResults>();
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            IList<MasterVitals> mastervitalslist = new List<MasterVitals>();
            DateTime BirthDate = DateTime.MinValue;
            string humanSex = string.Empty;
            double humanAgeInMonths = 0;
            ulong ScreenID = 2000;

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                BirthDate = ClientSession.PatientPaneList[0].Birth_Date;
                humanSex = ClientSession.PatientPaneList[0].Sex;
            }
            if (hdnSystemTime.Value.Trim() != string.Empty)
                humanAgeInMonths = ((DateTime.ParseExact(hdnSystemTime.Value, "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            else if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
            {
                if (Request["openingfrom"] != "Menu")
                    humanAgeInMonths = ((UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null)).Date - BirthDate.Date).TotalDays) / 30.4375;
                else
                    humanAgeInMonths = ((DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            }
            else
                humanAgeInMonths = ((DateTime.Now.Date - BirthDate.Date).TotalDays) / 30.4375;


            if (Session["vitalArray"] != null)
            {
                vitalArray = (ArrayList)Session["vitalArray"];
            }
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }

            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }

            if (Session["dynamicScreenList"] != null)
                dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];

            if (Session["mastervitalslist"] != null)
                mastervitalslist = (IList<MasterVitals>)Session["mastervitalslist"];

            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);

            if (Session["VitalTakenDate"] != null)
                VitalTakenDate = Convert.ToDateTime(Session["VitalTakenDate"]);
            if (Session["VitalTakenDateValue"] != null)
                VitalTakenDateValue = Convert.ToDateTime(Session["VitalTakenDateValue"]);
            foreach (object o in vitalArray)
            {
                string name = o.ToString();
                string sLoincname = name;
                if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")) && (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                {
                    if ((name.Contains("-")) || (name.Contains("/")))
                    {
                        if (name.Contains("-"))
                            name = name.Replace("-", "");
                        if (name.Contains("/"))
                            name = name.Replace("/", "");
                        if (name.Contains(" "))
                            name = name.Replace(" ", "");
                    }
                }

                Control ctrDtp = pnlVitals.FindControl(("dtp" + name).Contains(" ") ? "dtp" + name.Replace(" ", "") : "dtp" + name);


                if (ctrDtp != null)
                {
                    HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
                    if (dtpInAdd != null)
                    {
                        // Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_" + dtpInAdd.ID);
                        // if (ctrDtpTime != null)
                        // {
                        //  MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                        // if (dtpInAddTime != null)
                        // {
                        //  string[] ctrldtpChangedvalue = Request.Form[dtpInAdd.ID.Trim()].Split('-');
                        //string[] ctrldtpChangedvalue = Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]).ToString("yyyy-mm-dd").Split('-');
                        // // if (ctrldtpChangedvalue.Length > 0)
                        //        {
                        //            string Hour = Request.Form[dtpInAddTime.ID];
                        //            string Minute = Request.Form[dtpInAddTime.ID + "_txtMinute"];
                        //            string AMPM = Request.Form[dtpInAddTime.ID + "_txtAmPm"];
                        //            DateTime temp = new DateTime(Convert.ToInt32(ctrldtpChangedvalue[0]), Convert.ToInt32(ctrldtpChangedvalue[1]), Convert.ToInt32(ctrldtpChangedvalue[2]), Convert.ToInt32(Hour), Convert.ToInt32(Minute), Convert.ToInt32(00));
                        //            string temp1 = string.Empty;
                        //            if (temp.ToString().Contains("AM"))
                        //                temp1 = temp.ToString().Replace("AM", "");
                        //            else if (temp.ToString().Contains("PM"))
                        //                temp1 = temp.ToString().Replace("PM", "");
                        //            else
                        //                temp1 = temp.ToString();
                        try
                        {
                            VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]));
                        }
                        catch
                        {
                            VitalTakenDate = VitalTakenDate;
                        }
                        //        }
                        //    }
                        //}
                    }
                }
                else if (name.ToUpper().Contains("STATUS"))
                {
                    VitalTakenDate = VitalTakenDate;
                }
                else
                {
                    VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(VitalTakenDateValue.ToString("dd-MMM-yyyy hh:mm tt")));
                }
                if (name.ToUpper().Contains("BLOOD"))
                {
                    name = sLoincname;
                }

                Control ctrl = pnlVitals.FindControl(name);
                name = sLoincname;
                if (ctrl == null)
                {
                    ctrl = pnlVitals.FindControl(name.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                }
                if (ctrl != null && ctrl.ID != string.Empty)
                {
                    CustomDLCNew cb = null;
                    if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")))//&& (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                    {
                        name = Regex.Replace(name, @"[\-\/\s]+", string.Empty);

                        if ((name.Contains("-")) || (name.Contains("/")))
                        {
                            if (name.Contains("-"))
                                name = name.Replace("-", "");
                            if (name.Contains("/"))
                                name = name.Replace("/", "");
                            if (name.Contains(" "))
                                name = name.Replace(" ", "");
                        }
                    }

                    Control ctrl2 = pnlVitals.FindControl("txtNotes" + name.Replace(" ", "").Replace("CDP", "").Replace("-", ""));
                    if (name == "Ultrasound - Aortic")
                        ctrl2 = pnlVitals.FindControl("txtNotes" + name.Replace(" ", "").Replace("-", "").Replace("CDP", ""));
                    if (ctrl2 != null && ctrl2.ID != string.Empty)
                    {
                        cb = (CustomDLCNew)ctrl2;
                    }
                    //HtmlSelect reasonnotPerformed = null;
                    //Control ctrlreason = pnlVitals.FindControl("Patient-Reason-refused-" + name.Replace(" ", "").Replace("-", ""));
                    //if (ctrlreason != null && ctrlreason.ID != string.Empty)
                    //{
                    //    reasonnotPerformed = (HtmlSelect)ctrlreason;
                    //}
                    name = sLoincname;
                    vitalsObj = CreateVitalsObject(GroupID + 1);

                    vitalsObj.Loinc_Observation = name;
                    vitalsObj.Captured_date_and_time = VitalTakenDate;
                    vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                    if (ctrl.GetType().ToString().ToUpper().Contains("CHECKBOX") == true)
                    {
                        vitalsObj.Value = GetCheckedValues(name).Trim();
                        for (int j = 0; j < dynamicScreenList.Count; j++)
                        {

                            if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                            {
                                vitalsObj.Loinc_Observation = dynamicScreenList[j].Loinc_Identifier;
                                vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;
                            }
                        }
                        VitalName = string.Empty;
                        VitalName = vitalsObj.Loinc_Observation;
                        if (VitalName.Contains("Second"))
                        {
                            VitalName = VitalName.Replace("Second", "");
                        }
                        for (int k = 0; k < mastervitalslist.Count; k++)
                        {

                            if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                            {
                                vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                            }
                        }
                        if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                        {
                            if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))
                                //&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                continue;
                        }
                        else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false && (!VitalName.Contains("eGFR Status")) && (!VitalName.Contains("eGFR StatusSecond")))
                        {
                            EnableDisbaleSave(true);
                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                divLoading.Style.Add("display", "none");
                            else
                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (htUnitConvMthds.Contains(name))
                        {
                            vitalsObj.Value = ConversionOnSave(name).Trim();
                            for (int j = 0; j < dynamicScreenList.Count; j++)
                            {
                                if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                {
                                    //if (name.ToUpper() == "BMI")
                                    //{
                                    //    string status = Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString();
                                    //    string[] code = dynamicScreenList[j].Snomed_Code.Split('|');
                                    //    for (int y = 0; y < code.Length; y++)
                                    //    {
                                    //        if (status.ToUpper() == code[y].Split(':')[0])
                                    //        {
                                    //            vitalsObj.Snomed_Code = code[y].Split(':')[1];
                                    //            break;
                                    //        }
                                    //    }
                                    //}
                                    // else
                                    // {
                                    // vitalsObj.Snomed_Code = dynamicScreenList[j].Snomed_Code;
                                    //}
                                    vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                    vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                    vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;
                                }
                            }

                            VitalName = string.Empty;
                            VitalName = vitalsObj.Loinc_Observation;
                            if (VitalName.Contains("Second"))
                            {
                                VitalName = VitalName.Replace("Second", "");
                            }
                            for (int k = 0; k < mastervitalslist.Count; k++)
                            {
                                if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                {
                                    vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                }
                            }


                            if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                            {
                                if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))
                                    //&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))

                                    continue;
                            }
                            else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false && (!VitalName.Contains("eGFR Status")) && (!VitalName.Contains("eGFR StatusSecond")))
                            {
                                EnableDisbaleSave(true);
                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                    divLoading.Style.Add("display", "none");
                                else
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                return;
                            }
                        }
                        else
                        {
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                            {
                                HtmlGenericControl ctrllbl = (HtmlGenericControl)ctrl;
                                if (Request.Form[ctrllbl.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrllbl.ID.Trim()];
                            }
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                            {
                                HtmlInputText ctrltxt = (HtmlInputText)ctrl;
                                if (Request.Form[ctrltxt.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                if (ctrltxt.ID != null && ctrltxt.ID.ToUpper().Contains("BMI") && ctrltxt.ID.ToUpper().Contains("STATUS"))
                                    vitalsObj.Value = ctrltxt.Value;
                            }
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                            {
                                HtmlTextArea ctrltxt = (HtmlTextArea)ctrl;
                                if (Request.Form[ctrltxt.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                            }
                            if (ctrl.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                            {
                                HtmlSelect ctrlcmb = (HtmlSelect)ctrl;
                                if (Request.Form[ctrlcmb.ID.Trim()] != null)
                                    vitalsObj.Value = Request.Form[ctrlcmb.ID.Trim()];
                                //vitalsObj.Value = ctrlcmb.Items[ctrlcmb.SelectedIndex].Text;
                            }
                            //if (ctrl.GetType().Name.ToUpper().Contains("RADMASKEDTEXTBOX"))
                            //{
                            //    RadMaskedTextBox ctrlcmb = (RadMaskedTextBox)ctrl;
                            //    string[] aryFromDate = Request.Form[ctrlcmb.ID].Replace("____", "").Replace("___", "").Replace("__", "").Split('-');
                            //    if (aryFromDate[2] != "" && aryFromDate[1] != "" && aryFromDate[0] != "")
                            //    {
                            //        vitalsObj.Value = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];
                            //    }
                            //    else if (aryFromDate[0] != "" && aryFromDate[1] != "" && aryFromDate[2] == "")
                            //    {
                            //        vitalsObj.Value = aryFromDate[1] + "-" + aryFromDate[0];
                            //        //vitalsObj.Value = "01-" + aryFromDate[1] + "-" + aryFromDate[0];
                            //    }
                            //    else if (aryFromDate[0] != "" && aryFromDate[1] == "" && aryFromDate[2] == "")
                            //    {
                            //        vitalsObj.Value = aryFromDate[0];
                            //    }
                            //    else
                            //    {
                            //        vitalsObj.Value = "";
                            //    }

                            //}

                            for (int j = 0; j < dynamicScreenList.Count; j++)
                            {
                                if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                {
                                    vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                    vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                    vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;

                                }
                            }
                            VitalName = string.Empty;
                            VitalName = vitalsObj.Loinc_Observation;
                            if (VitalName.Contains("Second"))
                            {
                                VitalName = VitalName.Replace("Second", "");
                            }
                            for (int k = 0; k < mastervitalslist.Count; k++)
                            {

                                if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                {
                                    vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                }
                            }
                            if (vitalsObj.Value.Trim() == string.Empty)
                            {
                                if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))
                                // && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    if (cb != null)
                    {
                        vitalsObj.Notes = Request.Form[cb.ID + "$txtDLC"].Trim();
                        string snomedcode = "";
                        string loinc_identifier = "";
                        //BugID:47706
                        if (vitalsObj.Notes != "" && cb.ID.IndexOf("LastMammogram") == -1)
                            snomedcode = objUtilityManager.GetSnomedfromStaticLookup("ReasonNotPerformedList", cb.TextControlID.ToString(), cb.txtDLC.Text.ToString());
                        else
                            loinc_identifier = objUtilityManager.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", cb.txtDLC.Text.ToString());

                        if (cb.ID.IndexOf("LastMammogram") != -1)
                            vitalsObj.Loinc_Identifier = loinc_identifier;
                        //string[] notes = Request.Form[cb.ID + "$txtDLC"].Trim().Split(',');
                        //for (int h = 0; h < notes.Length; h++)
                        //{
                        //    var test = (from m in ilstLookupRefuse where m.Value == notes[h].Trim() select m.Description).ToArray();
                        //    if (test.Count() > 0)
                        //    {
                        //        if (h == 0)
                        //            snomedcode = test[0] + ",";
                        //        else if (h == notes.Length - 1)
                        //            snomedcode = snomedcode + test[0] + ",";
                        //        else
                        //            snomedcode = snomedcode + test[0];


                        //    }

                        //}
                        vitalsObj.Snomed_Code = snomedcode;
                    }
                    //if (reasonnotPerformed != null)
                    //{
                    //    vitalsObj.Reason_For_Not_Performed = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text;
                    //    vitalsObj.Snomed_Code = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Value;
                    //}
                    //To create object for BP Location
                    if (name.ToUpper().Contains("BP") && name.ToUpper().IndexOf("STATUS") <= -1 && (vitalsObj.Value != string.Empty || (cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() != "")))
                    //|| (reasonnotPerformed != null && reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text != "")))
                    {
                        string[] split = name.Split(' ');
                        if (split != null && split.Length > 0)
                        {
                            try
                            {
                                //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(split[0] + " Left");
                                //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(split[0] + " Right");
                                RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(split[0] + " Left");
                                RadioButton chkRight = (RadioButton)pnlVitals.FindControl(split[0] + " Right");
                                PatientResults vitalsLocationObj = CreateVitalsObject(GroupID + 1);
                                vitalsLocationObj.Loinc_Observation = split[0] + " Location";
                                vitalsLocationObj.Captured_date_and_time = VitalTakenDate;
                                vitalsLocationObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                //{
                                //    vitalsLocationObj.Value = "Left";
                                //}
                                //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                //{
                                //    vitalsLocationObj.Value = "Right";
                                //}
                                if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                {
                                    vitalsLocationObj.Value = "Left";
                                }
                                else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                {
                                    vitalsLocationObj.Value = "Right";
                                }
                                saveList.Add(vitalsLocationObj);
                            }
                            catch
                            {
                            }
                        }
                    }
                    saveList.Add(vitalsObj);
                }
            }

            if (saveList != null)
            {
                if (saveList.Count > 0)
                {
                    objVitalDTO = vitalmngr.AddVitalDetails(saveList.ToArray<PatientResults>(), saveList[0].Human_ID, 1, 20, string.Empty, 0, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, ClientSession.UserName);

                    //SetVitalsToolTip(string.Empty);

                    EnableDisbaleSave(false);

                    if (hdnForMenuLevelCancel.Value == "Null")
                    {
                        if (sForClose == "Yes")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "vitals", "DisplayErrorMessage('200002');ForClosingVitalsForYes();CurrentSystemTime();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ImportVitals", "DisplayErrorMessage('200002');CurrentSystemTime();", true);
                        }
                    }
                    else
                    {
                        if (sForClose == "Yes")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "SaveVitals", "var which_tab = top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick').value.split('$#$')[0]; if(which_tab !='first'){SavedSuccessfully_NowProceed('EncounterTabClick');}DisplayErrorMessage('200002');ForClosingVitalsForYes();CurrentSystemTime();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Addvitals", "var which_tab = top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick').value.split('$#$')[0]; if(which_tab !='first'){SavedSuccessfully_NowProceed('EncounterTabClick');}DisplayErrorMessage('200002');CurrentSystemTime();", true);
                        }
                    }
                }
            }

            vitalList = objVitalDTO.VitalsList;

            Session["vitalList"] = vitalList;
            Session["objVitalDTO"] = objVitalDTO;

            ClearText();

            PastVitals(objVitalDTO.VitalsList);

            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                divLoading.Style.Add("display", "none");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);

            btnSaveVitals.Disabled = true;
        }
        public void UpdateVitals()
        {
            ulong GroupID = 0;
            ulong DelUpdteGroupID = 0;
            string VitalName = string.Empty;
            DateTime VitalTakenDate = DateTime.MinValue;
            DateTime VitalTakenDateValue = DateTime.MinValue;
            PatientResults vitalsObj = new PatientResults();
            PatientResultsDTO objVitalDTO = null;
            ArrayList vitalArray = new ArrayList();
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();
            IList<PatientResults> saveList = new List<PatientResults>();
            IList<PatientResults> updateList = new List<PatientResults>();
            IList<PatientResults> delList = new List<PatientResults>();
            ArrayList upDateVital = new ArrayList();
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            IList<PatientResults> vitalList = new List<PatientResults>();
            IList<MasterVitals> mastervitalslist = new List<MasterVitals>();
            IList<DynamicScreen> dynamicScreenListforCodes = new List<DynamicScreen>();
            IList<MasterVitals> mastervitalslistforUnits = new List<MasterVitals>();
            DateTime BirthDate = DateTime.MinValue;
            string humanSex = string.Empty;
            double humanAgeInMonths = 0;
            ulong ScreenID = 2000;

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                BirthDate = ClientSession.PatientPaneList[0].Birth_Date;
                humanSex = ClientSession.PatientPaneList[0].Sex;
            }
            if (hdnSystemTime.Value.Trim() != string.Empty)
                humanAgeInMonths = ((DateTime.ParseExact(hdnSystemTime.Value, "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            else if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
            {
                if (Request["openingfrom"] != "Menu")
                    humanAgeInMonths = ((UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null)).Date - BirthDate.Date).TotalDays) / 30.4375;
                else
                    humanAgeInMonths = ((DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            }
            else
                humanAgeInMonths = ((DateTime.Now.Date - BirthDate.Date).TotalDays) / 30.4375;

            if (Session["vitalArray"] != null)
            {
                vitalArray = (ArrayList)Session["vitalArray"];
            }
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }
            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }
            if (Session["dynamicScreenList"] != null)
                dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];

            if (Session["mastervitalslist"] != null)
                mastervitalslist = (IList<MasterVitals>)Session["mastervitalslist"];

            if (Session["DelUpdteGroupID"] != null)
                DelUpdteGroupID = Convert.ToUInt64(Session["DelUpdteGroupID"]);

            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);

            if (Session["VitalTakenDate"] != null)
                VitalTakenDate = Convert.ToDateTime(Session["VitalTakenDate"]);

            if (Session["VitalTakenDateValue"] != null)
                VitalTakenDateValue = Convert.ToDateTime(Session["VitalTakenDateValue"]);

            var updateVitalList = (from vital in objVitalDTO.VitalsList where vital.Vitals_Group_ID == DelUpdteGroupID select new { vital.Loinc_Observation, vital.Captured_date_and_time });

            foreach (var item in vitalList)
            {
                upDateVital.Add(item.Loinc_Observation);
            }
            foreach (object o in vitalArray)
            {
                string name = o.ToString();
                string sLoincname = name;
                if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")))// && (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                {
                    if ((name.Contains("-")) || (name.Contains("/")))
                    {
                        if (name.Contains("-"))
                            name = name.Replace("-", "");
                        if (name.Contains("/"))
                            name = name.Replace("/", "");
                        if (name.Contains(" "))
                            name = name.Replace(" ", "");
                    }
                }
                Control ctrDtp = pnlVitals.FindControl(("dtp" + name).Contains(" ") ? "dtp" + name.Replace(" ", "") : "dtp" + name);

                if (ctrDtp != null)
                {
                    HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
                    if (dtpInAdd != null)
                    {
                        // Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_" + dtpInAdd.ID);
                        // if (ctrDtpTime != null)
                        // {
                        //  MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                        // if (dtpInAddTime != null)
                        // {
                        // string[] ctrldtpChangedvalue = Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]).ToString("yyyy-MM-dd").Split('-');
                        //  if (ctrldtpChangedvalue.Length > 0)
                        // {
                        //string Hour = Request.Form[dtpInAddTime.ID];
                        //string Minute = Request.Form[dtpInAddTime.ID + "_txtMinute"];
                        //string AMPM = Request.Form[dtpInAddTime.ID + "_txtAmPm"];
                        //DateTime temp = new DateTime(Convert.ToInt32(ctrldtpChangedvalue[0]), Convert.ToInt32(ctrldtpChangedvalue[1]), Convert.ToInt32(ctrldtpChangedvalue[2]), Convert.ToInt32(Hour), Convert.ToInt32(Minute), Convert.ToInt32(00));
                        //string temp1 = string.Empty;
                        //if (temp.ToString().Contains("AM"))
                        //    temp1 = temp.ToString().Replace("AM", "");
                        //else if (temp.ToString().Contains("PM"))
                        //    temp1 = temp.ToString().Replace("PM", "");
                        //else
                        //    temp1 = temp.ToString();
                        try
                        {
                            VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]));
                        }
                        catch
                        {
                            VitalTakenDate = VitalTakenDate;
                        }
                        // }
                        //}
                        // }
                    }
                }
                else if (!name.ToUpper().Contains("STATUS"))
                {
                    VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(VitalTakenDateValue.ToString("dd-MMM-yyyy hh:mm tt")));
                }
                if (name.ToUpper().Contains("BLOOD"))
                {
                    name = sLoincname;
                }

                Control ctrl = pnlVitals.FindControl(name);
                if (ctrl == null)
                {
                    ctrl = pnlVitals.FindControl(name.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                }
                name = sLoincname;
                if (ctrl != null && ctrl.ID != string.Empty)
                {
                    CustomDLCNew cb = null;
                    PatientResults locVital = null;
                    if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD") || name.ToUpper().Contains("HOLTER") || name.ToUpper().Contains("ULTRASOUND")))///&& (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                    {
                        name = Regex.Replace(name, @"[\-\/\s]+", string.Empty);
                    }

                    Control ctrl2 = pnlVitals.FindControl("txtNotes" + name.Replace(" ", "").Replace("CDP", "").Replace("-", ""));

                    if (ctrl2 != null && ctrl2.ID != string.Empty)
                    {
                        cb = (CustomDLCNew)ctrl2;
                    }
                    //HtmlSelect reasonnotPerformed = null;
                    //Control ctrlreason = pnlVitals.FindControl("Patient-Reason-refused-" + name.Replace(" ", "").Replace("-", ""));
                    //if (ctrlreason != null && ctrlreason.ID != string.Empty)
                    //{
                    //    reasonnotPerformed = (HtmlSelect)ctrlreason;
                    //}
                    name = sLoincname;

                    if (name.ToUpper().Contains("BP") && name.ToUpper().IndexOf("STATUS") <= -1)
                    {
                        string[] split = name.Split(' ');
                        if (split != null && split.Length > 0)
                        {
                            try
                            {
                                locVital = (from li in vitalList
                                            where li.Loinc_Observation == split[0] + " Location" && li.Encounter_ID == ClientSession.EncounterId
                                            select li).ToList<PatientResults>()[0];
                            }
                            catch
                            {
                                locVital = null;
                            }
                        }
                    }

                    if (ctrl != null)
                    {
                        if (upDateVital.Contains(name))
                        {
                            var updateVitals = from vital in objVitalDTO.VitalsList
                                               where vital.Loinc_Observation == name
                                               select new { vital };
                            foreach (var item in updateVitals)
                            {
                                vitalsObj = item.vital;
                                if (ctrl.GetType().ToString().ToUpper().Contains("CHECKBOX") == true)
                                {
                                    vitalsObj.Value = GetCheckedValues(name).Trim();
                                    var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                                    if (DynamicList.Count() > 0)
                                    {
                                        dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                                        if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                        {
                                            vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                            vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                            vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                        }
                                    }
                                    VitalName = string.Empty;
                                    VitalName = vitalsObj.Loinc_Observation;
                                    if (VitalName.Contains("Second"))
                                    {
                                        VitalName = VitalName.Replace("Second", "");
                                    }
                                    var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                                    if (MasterList.Count() > 0)
                                    {
                                        mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                                        if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                        {
                                            vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;
                                        }
                                    }
                                    if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                                    {
                                        if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))
                                        //&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                        {
                                            continue;
                                        }
                                    }
                                    else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false)
                                    {
                                        if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                        {
                                            string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                EnableDisbaleSave(true);
                                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                    divLoading.Style.Add("display", "none");
                                                else
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                return;
                                            }
                                        }
                                        else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                        {
                                            string egfrvalue = Request.Form["eGFR"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                EnableDisbaleSave(true);
                                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                    divLoading.Style.Add("display", "none");
                                                else
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            EnableDisbaleSave(true);
                                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                divLoading.Style.Add("display", "none");
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                            return;
                                        }
                                    }
                                }
                                else
                                {

                                    if (htUnitConvMthds.Contains(name))
                                    {
                                        vitalsObj.Value = ConversionOnSave(name).Trim();

                                        var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                                        if (DynamicList.Count() > 0)
                                        {
                                            dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                                            if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                            {
                                                if (name.ToUpper() == "BMI")
                                                {



                                                    if (Request.Form[pnlVitals.FindControl("BMI Status").ID] != null && dynamicScreenListforCodes[0].Snomed_Code != "" && Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString() != "")
                                                    {
                                                        string[] code = dynamicScreenListforCodes[0].Snomed_Code.Split('|');
                                                        for (int y = 0; y < code.Length; y++)
                                                        {
                                                            if (code[y].Split(':').Length > 1)
                                                            {
                                                                if (Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString().ToUpper().Contains(code[y].Split(':')[0]))
                                                                {
                                                                    vitalsObj.Snomed_Code = code[y].Split(':')[1];
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        vitalsObj.Snomed_Code = dynamicScreenList[0].Snomed_Code;
                                                    }
                                                }
                                                else
                                                {
                                                    vitalsObj.Snomed_Code = dynamicScreenList[0].Snomed_Code;
                                                }
                                                vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                                vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                                vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                            }
                                        }
                                        VitalName = string.Empty;
                                        VitalName = vitalsObj.Loinc_Observation;
                                        VitalName = VitalName.Replace("Second", string.Empty);

                                        var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                                        if (MasterList.Count() > 0)
                                        {
                                            mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                                            if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                            {
                                                vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;
                                            }
                                        }

                                        vitalsObj.Captured_date_and_time = VitalTakenDate;
                                        vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        if (vitalsObj.Value.Trim() == string.Empty)
                                        {
                                            if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                            {
                                                string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                                if (egfrvalue == string.Empty)
                                                {
                                                    if (allEmpty == true)
                                                    {
                                                        //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) && 
                                                        if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                                        {
                                                            vitalsObj.Modified_By = ClientSession.UserName;
                                                            //vitalsObj.Modified_Date_And_Time = utc;
                                                            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                            delList.Add(vitalsObj);
                                                            if (locVital != null)
                                                            {
                                                                locVital.Modified_By = ClientSession.UserName;
                                                                //locVital.Modified_Date_And_Time = utc;
                                                                locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                                delList.Add(locVital);
                                                            }
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EnableDisbaleSave(true);
                                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                            divLoading.Style.Add("display", "none");
                                                        else
                                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                        return;
                                                    }
                                                }
                                            }
                                            else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                            {
                                                string egfrvalue = Request.Form["eGFR"].Trim();
                                                if (egfrvalue == string.Empty)
                                                {
                                                    if (allEmpty == true)
                                                    {
                                                        //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) && 
                                                        if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                                        {
                                                            vitalsObj.Modified_By = ClientSession.UserName;
                                                            //vitalsObj.Modified_Date_And_Time = utc;
                                                            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                            delList.Add(vitalsObj);
                                                            if (locVital != null)
                                                            {
                                                                locVital.Modified_By = ClientSession.UserName;
                                                                //locVital.Modified_Date_And_Time = utc;
                                                                locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                                delList.Add(locVital);
                                                            }
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EnableDisbaleSave(true);
                                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                            divLoading.Style.Add("display", "none");
                                                        else
                                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                        return;
                                                    }
                                                }
                                            }

                                            else
                                            {
                                                if (allEmpty == true)
                                                {
                                                    //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) && 
                                                    if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                                    {
                                                        vitalsObj.Modified_By = ClientSession.UserName;
                                                        //vitalsObj.Modified_Date_And_Time = utc;
                                                        vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                                                        delList.Add(vitalsObj);
                                                        if (locVital != null)
                                                        {
                                                            locVital.Modified_By = ClientSession.UserName;
                                                            //locVital.Modified_Date_And_Time = utc;
                                                            locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                            delList.Add(locVital);
                                                        }
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    EnableDisbaleSave(true);
                                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                        divLoading.Style.Add("display", "none");
                                                    else
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                                        {
                                            HtmlGenericControl ctrllbl = (HtmlGenericControl)ctrl;
                                            if (Request.Form[ctrllbl.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrllbl.ID.Trim()];
                                        }
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                                        {
                                            HtmlInputText ctrltxt = (HtmlInputText)ctrl;
                                            if (Request.Form[ctrltxt.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                            else if (ctrltxt != null)
                                                vitalsObj.Value = ctrltxt.Value;
                                        }
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                        {
                                            HtmlTextArea ctrltxt = (HtmlTextArea)ctrl;
                                            if (Request.Form[ctrltxt.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                        }
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                                        {
                                            HtmlSelect ctrlcmb = (HtmlSelect)ctrl;
                                            if (Request.Form[ctrlcmb.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrlcmb.ID.Trim()];
                                            //vitalsObj.Value = ctrlcmb.Items[ctrlcmb.SelectedIndex].Text;
                                        }



                                        var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                                        if (DynamicList.Count() > 0)
                                        {
                                            dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                                            if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                            {
                                                vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                                vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                                vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                            }
                                        }

                                        VitalName = string.Empty;
                                        VitalName = vitalsObj.Loinc_Observation.Replace("Second", ""); ;

                                        var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                                        if (MasterList.Count() > 0)
                                        {
                                            mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                                            if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                            {
                                                vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;
                                            }
                                        }
                                        vitalsObj.Captured_date_and_time = VitalTakenDate;
                                        vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) && 
                                        if (vitalsObj.Value == "" && ((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))
                                        {
                                            vitalsObj.Modified_By = ClientSession.UserName;
                                            //vitalsObj.Modified_Date_And_Time = utc;
                                            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                            delList.Add(vitalsObj);
                                            if (locVital != null)
                                            {
                                                locVital.Modified_By = ClientSession.UserName;
                                                //locVital.Modified_Date_And_Time = utc;
                                                locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                delList.Add(locVital);
                                            }
                                            continue;
                                        }
                                    }
                                }
                                //To create object for BP Location
                                object[] lst = upDateVital.ToArray();
                                if (item.vital.Loinc_Observation.ToUpper().Contains("SYS/DIA") && item.vital.Loinc_Observation.ToUpper().IndexOf("STATUS") <= -1)
                                {
                                    if (!lst.Any(a => a.ToString().ToUpper() == (item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7) + "Location").ToUpper())) //item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7)+"Location")
                                    {
                                        if (name.ToUpper().Contains("BP") && ((vitalsObj.Value != string.Empty) || (cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() != "")))// || (reasonnotPerformed != null && reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text != "")))
                                        {
                                            string[] split = name.Split(' ');
                                            if (split != null && split.Length > 0)
                                            {
                                                try
                                                {
                                                    Control ctllft = pnlVitals.FindControl(split[0] + " Left");
                                                    Control ctlrht = pnlVitals.FindControl(split[0] + " Right");
                                                    //CheckBox chkLeft = (CheckBox)ctllft;
                                                    //CheckBox chkRight = (CheckBox)ctlrht;
                                                    RadioButton chkLeft = (RadioButton)ctllft;
                                                    RadioButton chkRight = (RadioButton)ctlrht;
                                                    PatientResults vitalsLocationObj = CreateVitalsObject(GroupID);
                                                    vitalsLocationObj.Loinc_Observation = split[0] + " Location";
                                                    //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                                    //{
                                                    //    vitalsLocationObj.Value = "Left";
                                                    //}
                                                    //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                                    //{
                                                    //    vitalsLocationObj.Value = "Right";
                                                    //}
                                                    if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                                    {
                                                        vitalsLocationObj.Value = "Left";
                                                    }
                                                    else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                                    {
                                                        vitalsLocationObj.Value = "Right";
                                                    }
                                                    vitalsLocationObj.Captured_date_and_time = VitalTakenDate;
                                                    vitalsLocationObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                    saveList.Add(vitalsLocationObj);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else if (lst.Any(a => a.ToString().ToUpper() == (item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7) + "Location").ToUpper()))
                                    {
                                        if (name.ToUpper().Contains("BP") && vitalsObj.Value == string.Empty && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                        //&& (reasonnotPerformed == null || reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text == ""))
                                        {
                                            string Location = item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7) + "Location";
                                            if (vitalList.Any(a => a.Loinc_Observation.Trim() == Location.Trim()))
                                            {
                                                PatientResults objPatientResults = vitalList.Where(a => a.Loinc_Observation.Trim() == Location.Trim()).ToList<PatientResults>()[0];
                                                delList.Add(objPatientResults);
                                                locVital = null;
                                            }
                                        }
                                    }
                                }
                                if (cb != null)
                                {
                                    vitalsObj.Notes = Request.Form[cb.ID + "$txtDLC"].Trim();
                                    string snomedcode = "";
                                    string loinc_identifier = "";
                                    //BugID:47706
                                    if (vitalsObj.Notes != "" && cb.ID.IndexOf("LastMammogram") == -1)
                                        snomedcode = objUtilityManager.GetSnomedfromStaticLookup("ReasonNotPerformedList", cb.TextControlID.ToString(), cb.txtDLC.Text.ToString());
                                    else
                                        loinc_identifier = objUtilityManager.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", cb.txtDLC.Text.ToString());

                                    if (cb.ID.IndexOf("LastMammogram") != -1)
                                        vitalsObj.Loinc_Identifier = loinc_identifier;
                                    //string[] notes= Request.Form[cb.ID + "$txtDLC"].Trim().Split(',');
                                    //for (int h = 0; h < notes.Length; h++)
                                    //{
                                    //    var test = (from m in ilstLookupRefuse where m.Value == notes[h].Trim() select m.Description).ToArray();
                                    //    if (test.Count() > 0)
                                    //    {
                                    //        if (h == 0)
                                    //            snomedcode = test[0] + ",";
                                    //        else if (h == notes.Length - 1)
                                    //            snomedcode = snomedcode + test[0] + ",";
                                    //        else
                                    //            snomedcode = snomedcode + test[0];


                                    //    }

                                    //}
                                    if (cb.ID.ToUpper().Contains("HEIGHT") || cb.ID.ToUpper().Contains("WEIGHT") || cb.ID.ToUpper().Contains("BPSITTINGSYSDIA"))
                                    {
                                        if (snomedcode == "" && vitalsObj.Value == "")
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "displaysnomedalert('" + cb.TextControlID.ToString() + "')", true);
                                            return;
                                        }
                                    }

                                    vitalsObj.Snomed_Code = snomedcode;

                                }
                                //if (reasonnotPerformed != null)
                                //{
                                //    vitalsObj.Reason_For_Not_Performed = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text;
                                //    vitalsObj.Snomed_Code = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Value;
                                //}
                                vitalsObj.Modified_By = ClientSession.UserName;
                                //vitalsObj.Modified_Date_And_Time = utc;
                                vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                vitalsObj.Results_Type = "Vitals";
                                //Added Is_Sent_to_Rcopia for Bug ID:33607
                                vitalsObj.Is_Sent_to_Rcopia = "N";
                                updateList.Add(vitalsObj);
                                try
                                {
                                    if (locVital != null)
                                    {
                                        //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Left"));
                                        //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Right"));

                                        RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Left"));
                                        RadioButton chkRight = (RadioButton)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Right"));


                                        // if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                        if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                        {
                                            locVital.Value = "Left";
                                        }
                                        else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                        {
                                            locVital.Value = "Right";
                                        }
                                        locVital.Captured_date_and_time = VitalTakenDate;
                                        locVital.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        locVital.Modified_By = ClientSession.UserName;
                                        //locVital.Modified_Date_And_Time = utc;
                                        locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        locVital.Results_Type = "Vitals";
                                        //Added Is_Sent_to_Rcopia for Bug ID:33607
                                        locVital.Is_Sent_to_Rcopia = "N";
                                        updateList.Add(locVital);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        else if (!(upDateVital.Contains(name)))
                        {
                            vitalsObj = CreateVitalsObject(GroupID);
                            vitalsObj.Loinc_Observation = name;
                            var DynamicList = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                            if (DynamicList.Count() > 0)
                            {
                                dynamicScreenListforCodes = DynamicList.ToList<DynamicScreen>();
                                if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                {
                                    vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                    vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                    vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                }
                            }

                            VitalName = string.Empty;
                            VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                            var MasterList = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                            if (MasterList.Count() > 0)
                            {
                                mastervitalslistforUnits = MasterList.ToList<MasterVitals>();
                                if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                {
                                    vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;
                                }
                            }
                            if (ctrl.GetType().ToString().ToUpper().Contains("CHECKBOX") == true)
                            {
                                vitalsObj.Value = GetCheckedValues(name).Trim();
                                if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                                {
                                    if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))//&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                    {
                                        continue;
                                    }
                                }
                                else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false)
                                {
                                    if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                    {
                                        string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                        if (egfrvalue == string.Empty)
                                        {
                                            EnableDisbaleSave(true);
                                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                divLoading.Style.Add("display", "none");
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                            return;
                                        }
                                    }
                                    else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                    {
                                        string egfrvalue = Request.Form["eGFR"].Trim();
                                        if (egfrvalue == string.Empty)
                                        {
                                            EnableDisbaleSave(true);
                                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                divLoading.Style.Add("display", "none");
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        EnableDisbaleSave(true);
                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                            divLoading.Style.Add("display", "none");
                                        else
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (htUnitConvMthds.Contains(name))
                                {
                                    vitalsObj.Value = ConversionOnSave(name).Trim();

                                    var DynamicLists = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                                    dynamicScreenListforCodes = DynamicLists.ToList<DynamicScreen>();
                                    if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                    {
                                        if (name.ToUpper() == "BMI")
                                        {
                                            //string status = Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString();
                                            if (Request.Form[pnlVitals.FindControl("BMI Status").ID] != null && dynamicScreenListforCodes[0].Snomed_Code != "" && Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString() != "")
                                            {
                                                string[] code = dynamicScreenListforCodes[0].Snomed_Code.Split('|');
                                                for (int y = 0; y < code.Length; y++)
                                                {
                                                    if (code[y].Split(':').Length > 1)
                                                    {
                                                        if (Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString().ToUpper().Contains(code[y].Split(':')[0]))
                                                        {
                                                            vitalsObj.Snomed_Code = code[y].Split(':')[1];
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                vitalsObj.Snomed_Code = dynamicScreenList[0].Snomed_Code;
                                            }
                                        }
                                        else
                                        {
                                            vitalsObj.Snomed_Code = dynamicScreenList[0].Snomed_Code;
                                        }
                                        vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                        vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                        vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                    }

                                    VitalName = string.Empty;
                                    VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                                    var MasterLists = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                                    mastervitalslistforUnits = MasterLists.ToList<MasterVitals>();
                                    if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                    {
                                        vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;
                                    }

                                    vitalsObj.Captured_date_and_time = VitalTakenDate;
                                    vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                    if (vitalsObj.Value == string.Empty)
                                    {
                                        if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                        {
                                            string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                if (allEmpty == true)
                                                {
                                                    if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))//&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    EnableDisbaleSave(true);
                                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                        divLoading.Style.Add("display", "none");
                                                    else
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                    return;
                                                }
                                            }
                                        }
                                        else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                        {
                                            string egfrvalue = Request.Form["eGFR"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                if (allEmpty)
                                                {
                                                    if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))//&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    EnableDisbaleSave(true);
                                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                        divLoading.Style.Add("display", "none");
                                                    else
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (allEmpty)
                                            {
                                                if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                                {
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                EnableDisbaleSave(true);
                                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                    divLoading.Style.Add("display", "none");
                                                else
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                                    {
                                        HtmlGenericControl ctrllbl = (HtmlGenericControl)ctrl;
                                        if (Request.Form[ctrllbl.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrllbl.ID.Trim()];
                                    }
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                                    {
                                        HtmlInputText ctrltxt = (HtmlInputText)ctrl;
                                        if (Request.Form[ctrltxt.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                        else if (ctrltxt != null)
                                            vitalsObj.Value = ctrltxt.Value;
                                    }
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                    {
                                        HtmlTextArea ctrltxt = (HtmlTextArea)ctrl;
                                        if (Request.Form[ctrltxt.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                    }
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                                    {
                                        HtmlSelect ctrlcmb = (HtmlSelect)ctrl;
                                        if (Request.Form[ctrlcmb.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrlcmb.ID.Trim()];
                                        //vitalsObj.Value = ctrlcmb.Items[ctrlcmb.SelectedIndex].Text;
                                    }
                                    //if (ctrl.GetType().Name.ToUpper().Contains("RADMASKEDTEXTBOX"))
                                    //{
                                    //    RadMaskedTextBox ctrlcmb = ((RadMaskedTextBox)pnlVitals.FindControl(name));

                                    //    string[] aryFromDate = Request.Form[ctrlcmb.ID].Replace("____", "").Replace("___", "").Replace("__", "").Split('-');
                                    //    if (aryFromDate[2] != "" && aryFromDate[1] != "" && aryFromDate[0] != "")
                                    //    {
                                    //        vitalsObj.Value = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];
                                    //    }
                                    //    else if (aryFromDate[0] != "" && aryFromDate[1] != "" && aryFromDate[2] == "")
                                    //    {
                                    //        vitalsObj.Value = aryFromDate[1] + "-" + aryFromDate[0];
                                    //    }
                                    //    else if (aryFromDate[0] != "" && aryFromDate[1] == "" && aryFromDate[2] == "")
                                    //    {
                                    //        vitalsObj.Value = aryFromDate[0];
                                    //    }
                                    //    else
                                    //    {
                                    //        vitalsObj.Value = "";
                                    //    }
                                    //}

                                    var DynamicList1 = from d in dynamicScreenList where d.Control_Name_Thin_Client == vitalsObj.Loinc_Observation select d;
                                    dynamicScreenListforCodes = DynamicList1.ToList<DynamicScreen>();
                                    if (dynamicScreenListforCodes != null && dynamicScreenListforCodes.Count > 0)
                                    {
                                        vitalsObj.Loinc_Identifier = dynamicScreenListforCodes[0].Loinc_Identifier;
                                        vitalsObj.Acurus_Result_Code = dynamicScreenListforCodes[0].Acurus_Result_Code;
                                        vitalsObj.Acurus_Result_Description = dynamicScreenListforCodes[0].Acurus_Result_Description;
                                    }

                                    VitalName = string.Empty;
                                    VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                                    var MasterList1 = from d in mastervitalslist where d.Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper() select d;
                                    mastervitalslistforUnits = MasterList1.ToList<MasterVitals>();
                                    if (mastervitalslistforUnits != null && mastervitalslistforUnits.Count > 0)
                                    {
                                        vitalsObj.Units = mastervitalslistforUnits[0].Vital_Unit;
                                    }

                                    vitalsObj.Captured_date_and_time = VitalTakenDate;
                                    vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                    if (vitalsObj.Value == string.Empty)
                                    {
                                        if (((cb != null && cb.ID!=null && Request.Form[cb.ID + "$txtDLC"] !=null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (cb != null)
                            {

                                vitalsObj.Notes = cb.txtDLC.Text;
                                string snomedcode = "";
                                string loinc_identifier = "";
                                //BugID:47706
                                if (vitalsObj.Notes != "" && cb.ID.IndexOf("LastMammogram") == -1)
                                    snomedcode = objUtilityManager.GetSnomedfromStaticLookup("ReasonNotPerformedList", cb.TextControlID.ToString(), cb.txtDLC.Text.ToString());
                                else
                                    loinc_identifier = objUtilityManager.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", cb.txtDLC.Text.ToString());

                                if (cb.ID.IndexOf("LastMammogram") != -1)
                                    vitalsObj.Loinc_Identifier = loinc_identifier;

                                //string[] notes = Request.Form[cb.ID + "$txtDLC"].Trim().Split(',');
                                //for (int h = 0; h < notes.Length; h++)
                                //{
                                //    var test = (from m in ilstLookupRefuse where m.Value == notes[h].Trim() select m.Description).ToArray();
                                //    if (test.Count() > 0)
                                //    {
                                //        if (h == 0)
                                //            snomedcode = test[0] + ",";
                                //        else if (h == notes.Length - 1)
                                //            snomedcode = snomedcode + test[0] + ",";
                                //        else
                                //            snomedcode = snomedcode + test[0];


                                //    }

                                //}
                                if (cb.ID.ToUpper().Contains("HEIGHT") || cb.ID.ToUpper().Contains("WEIGHT") || cb.ID.ToUpper().Contains("BPSITTINGSYSDIA"))
                                {
                                    if (snomedcode == "" && vitalsObj.Value == "")
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "displaysnomedalert('" + cb.TextControlID.ToString() + "')", true);
                                        return;
                                    }
                                }
                                vitalsObj.Snomed_Code = snomedcode;
                            }

                            //if (reasonnotPerformed != null)
                            //{
                            //    vitalsObj.Reason_For_Not_Performed = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text;
                            //    vitalsObj.Snomed_Code = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Value;
                            //}
                            //Added Is_Sent_to_Rcopia for Bug ID:33607
                            vitalsObj.Is_Sent_to_Rcopia = "N";
                            //To create object for BP Location
                            if (name.ToUpper().Contains("BP") && name.ToUpper().IndexOf("STATUS") <= -1 && (vitalsObj.Value != string.Empty || (cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() != "")))//|| (reasonnotPerformed != null && reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text != "")))
                            {
                                string[] split = name.Split(' ');
                                if (split != null && split.Length > 0)
                                {
                                    try
                                    {
                                        //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(split[0] + " Left");
                                        //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(split[0] + " Right");
                                        RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(split[0] + " Left");
                                        RadioButton chkRight = (RadioButton)pnlVitals.FindControl(split[0] + " Right");
                                        PatientResults vitalsLocationObj = CreateVitalsObject(GroupID);
                                        vitalsLocationObj.Loinc_Observation = split[0] + " Location";
                                        //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                        //{
                                        //    vitalsLocationObj.Value = "Left";
                                        //}
                                        //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                        //{
                                        //    vitalsLocationObj.Value = "Right";
                                        //}

                                        if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                        {
                                            vitalsLocationObj.Value = "Left";
                                        }
                                        else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                        {
                                            vitalsLocationObj.Value = "Right";
                                        }

                                        vitalsLocationObj.Captured_date_and_time = VitalTakenDate;
                                        vitalsLocationObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        //Added Is_Sent_to_Rcopia for Bug ID:33607
                                        vitalsLocationObj.Is_Sent_to_Rcopia = "N";
                                        saveList.Add(vitalsLocationObj);

                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            saveList.Add(vitalsObj);
                        }
                    }
                }
            }
            if (updateList != null && saveList != null)
            {
                if (updateList.Count > 0 || saveList.Count > 0)
                {
                    ulong human_id = 0;
                    ulong encounter_id = 0;
                    if (updateList.Count > 0)
                    {
                        human_id = updateList[0].Human_ID;
                        encounter_id = updateList[0].Encounter_ID;
                    }
                    else
                    {
                        human_id = saveList[0].Human_ID;
                        encounter_id = saveList[0].Encounter_ID;
                    }
                    objVitalDTO = vitalmngr.UpdateVitalDetails(updateList.ToArray<PatientResults>(), saveList.ToArray<PatientResults>(), delList.ToArray<PatientResults>(), human_id, 1, 20, string.Empty, encounter_id, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, ClientSession.UserName);
                    EnableDisbaleSave(false);


                    // if (objVitalDTO != null && objVitalDTO.sNotification.Trim() != string.Empty)
                    // {
                    // hdnNotification.Value = "true";
                    // ClientSession.FillPatientChart.Notification = objVitalDTO.sNotification;
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenNotificationSuccessfully();", true);
                    // }
                    //  else
                    // {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeLabel", "ChangeStatusLabel();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "savedSuccessfully();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "showhidBP();enabledisableBMI();", true);
                    ClientSession.FillPatientChart.Notification = string.Empty;
                    //}
                }
            }
            vitalList = objVitalDTO.VitalsList;

            DataSet dsGetVitals = new DataSet();
            string Loinc_observation = string.Empty;
            if (Session["Loinc_observation"] != null)
            {
                Loinc_observation = Session["Loinc_observation"].ToString();
                hdnLabResults.Value = Loinc_observation;
            }

            if (objVitalDTO.dsLatestResults.Tables.Count > 0)
            {
                dsGetVitals = objVitalDTO.dsLatestResults;
                if (dsGetVitals != null)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt = dsGetVitals.Tables[0];
                    if (Loinc_observation != string.Empty)
                    {
                        Loinc_observation = string.Empty;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Loinc_observation.Trim() == string.Empty)
                        {
                            Loinc_observation = dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                        }
                        else
                        {
                            Loinc_observation += ", " + dt.Rows[i][0].ToString() + "-" + dt.Rows[i][1].ToString();
                        }
                    }
                }
            }
            //if (delList.Count > 0)
            //{
            //    for (int i = 0; i < delList.Count; i++)
            //    {
            //        Control ctrDtp = pnlVitals.FindControl("dtp" + delList[i].Loinc_Observation.Replace(" ", ""));

            //        if (ctrDtp != null)
            //        {
            //            HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
            //            if (dtpInAdd != null)
            //            {
            //                IList<PatientPane> lstPatientPane = new List<PatientPane>();
            //                var PatientPaneList = from d in ClientSession.PatientPaneList where d.Encounter_ID == ClientSession.EncounterId select d;
            //                if (PatientPaneList.Count() > 0)
            //                {
            //                    lstPatientPane = PatientPaneList.ToList<PatientPane>();
            //                    if (lstPatientPane != null && lstPatientPane.Count > 0)
            //                    {
            //                        dtpInAdd.Value = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service).ToString();
            //                        arydatetimeFilled.Add(dtpInAdd.ID.ToString());
            //                    }
            //                }



            //Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_dtp" + delList[i].Loinc_Observation.Replace(" ", ""));
            //if (ctrDtpTime != null)
            //{
            //    MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
            //    if (dtpInAddTime != null)
            //    {
            //        DateTime CapturedTime = new DateTime();
            //        if (lstPatientPane != null && lstPatientPane.Count > 0)
            //        {
            //            CapturedTime = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service);
            //        }
            //        else
            //            CapturedTime = UtilityManager.ConvertToLocal(ClientSession.PatientPaneList[0].Date_of_Service);
            //        dtpInAddTime.Hour = CapturedTime.Hour;
            //        dtpInAddTime.Hour = CapturedTime.Hour == 0 ? getHour(CapturedTime.ToString()) : CapturedTime.Hour;

            //        dtpInAddTime.Minute = CapturedTime.Minute;
            //        dtpInAddTime.AmPm = CapturedTime.ToString("tt") == "AM" ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM;

            //    }
            //}

            //            }
            //        }
            //    }
            //}
            Session["vitalList"] = vitalList;
            Session["objVitalDTO"] = objVitalDTO;
            Session["Loinc_observation"] = Loinc_observation;

            hdnLabResults.Value = Loinc_observation;

            EditCellClickforVitals(false);
            //SetVitalsToolTip(Loinc_observation);
            SetVitalsToolTip(vitalList);
            //if (hdnNotification.Value == "true")
            //{
            //    RadScriptManager1.AsyncPostBackErrorMessage = RadScriptManager1.AsyncPostBackErrorMessage.ToString().Replace("true", "trueNotification");
            //}
            btnSaveVitals.Disabled = true;


        }
        public void gvEnterDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //gvEnterDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

            int count = e.Row.Cells.Count;

            e.Row.Cells[7].Visible = false;

            e.Row.Cells[count - 2].Visible = false;

            e.Row.Cells[count - 1].Visible = false;




        }
        public void UpdateImportVitals()
        {
            string sForClose = hdnMessageType.Value;
            hdnMessageType.Value = string.Empty;
            string VitalName = string.Empty;
            ulong DelUpdteGroupID = 0;
            ulong GroupID = 0;
            DateTime VitalTakenDate = DateTime.MinValue;
            DateTime VitalTakenDateValue = DateTime.MinValue;
            PatientResults vitalsObj = new PatientResults();
            ArrayList vitalArray = new ArrayList();
            PatientResultsDTO objVitalDTO = null;
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();
            IList<PatientResults> vitalList = new List<PatientResults>();
            IList<PatientResults> saveList = new List<PatientResults>();
            IList<PatientResults> updateList = new List<PatientResults>();
            IList<PatientResults> delList = new List<PatientResults>();
            ArrayList upDateVital = new ArrayList();
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            IList<MasterVitals> mastervitalslist = new List<MasterVitals>();
            DateTime BirthDate = DateTime.MinValue;
            string humanSex = string.Empty;
            double humanAgeInMonths = 0;
            ulong ScreenID = 2000;
            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                BirthDate = ClientSession.PatientPaneList[0].Birth_Date;
                humanSex = ClientSession.PatientPaneList[0].Sex;
            }
            if (hdnSystemTime.Value.Trim() != string.Empty)
                humanAgeInMonths = ((DateTime.ParseExact(hdnSystemTime.Value, "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            else if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
            {
                if (Request["openingfrom"] != "Menu")
                    humanAgeInMonths = ((UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null)).Date - BirthDate.Date).TotalDays) / 30.4375;
                else
                    humanAgeInMonths = ((DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            }
            else
                humanAgeInMonths = ((DateTime.Now.Date - BirthDate.Date).TotalDays) / 30.4375;

            if (Session["vitalArray"] != null)
            {
                vitalArray = (ArrayList)Session["vitalArray"];
            }
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }
            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }
            if (Session["dynamicScreenList"] != null)
                dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];

            if (Session["mastervitalslist"] != null)
                mastervitalslist = (IList<MasterVitals>)Session["mastervitalslist"];

            if (Session["DelUpdteGroupID"] != null)
                DelUpdteGroupID = Convert.ToUInt64(Session["DelUpdteGroupID"]);

            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);

            if (Session["VitalTakenDate"] != null)
                VitalTakenDate = Convert.ToDateTime(Session["VitalTakenDate"]);

            if (Session["VitalTakenDateValue"] != null)
                VitalTakenDateValue = Convert.ToDateTime(Session["VitalTakenDateValue"]);

            var updateVitalList = (from vital in objVitalDTO.VitalsList where vital.Vitals_Group_ID == DelUpdteGroupID select new { vital.Loinc_Observation, vital.Captured_date_and_time });

            foreach (var item in updateVitalList)
            {
                upDateVital.Add(item.Loinc_Observation);
            }
            foreach (object o in vitalArray)
            {
                string name = o.ToString();
                string sLoincname = name;
                if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")))// && (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                {
                    name = Regex.Replace(name, @"[\-\/\s]+", string.Empty);
                }
                Control ctrDtp = pnlVitals.FindControl(("dtp" + name).Contains(" ") ? "dtp" + name.Replace(" ", "") : "dtp" + name);

                if (ctrDtp != null)
                {
                    HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
                    if (dtpInAdd != null)
                    {
                        //Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_" + dtpInAdd.ID);
                        // if (ctrDtpTime != null)
                        //   {
                        //MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                        // if (dtpInAddTime != null)
                        // {
                        //if (dtpInAddTime != null)
                        //  {
                        //  string[] ctrldtpChangedvalue = Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]).ToString("yyyy-mm-dd").Split('-');
                        ///  if (ctrldtpChangedvalue.Length > 0)
                        //  {
                        //string Hour = Request.Form[dtpInAddTime.ID];
                        //string Minute = Request.Form[dtpInAddTime.ID + "_txtMinute"];
                        //string AMPM = Request.Form[dtpInAddTime.ID + "_txtAmPm"];
                        //DateTime temp = new DateTime(Convert.ToInt32(ctrldtpChangedvalue[0]), Convert.ToInt32(ctrldtpChangedvalue[1]), Convert.ToInt32(ctrldtpChangedvalue[2]), Convert.ToInt32(Hour), Convert.ToInt32(Minute), Convert.ToInt32(00));
                        //string temp1 = string.Empty;
                        //if (temp.ToString().Contains("AM"))
                        //    temp1 = temp.ToString().Replace("AM", "");
                        //else if (temp.ToString().Contains("PM"))
                        //    temp1 = temp.ToString().Replace("PM", "");
                        //else
                        //    temp1 = temp.ToString();
                        try
                        {
                            VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Request.Form[dtpInAdd.ID.Trim()]));
                        }
                        catch
                        {
                            VitalTakenDate = VitalTakenDate;
                        }
                        // }
                        //  }
                        //  }
                        // }
                    }
                }
                else if (!name.ToUpper().Contains("STATUS"))
                {
                    VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(VitalTakenDateValue.ToString("dd-MMM-yyyy hh:mm tt")));
                }

                if (name.ToUpper().Contains("BLOOD"))
                {
                    name = sLoincname;
                }

                Control ctrl = pnlVitals.FindControl(name);
                if (ctrl == null)
                {
                    ctrl = pnlVitals.FindControl(name.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                }
                name = sLoincname;

                if (ctrl != null && ctrl.ID != string.Empty)
                {
                    CustomDLCNew cb = null;
                    if ((name.ToUpper().Contains("BP") || name.ToUpper().Contains("BLOOD")))// && (!name.ToUpper().Contains("PATIENT-REASON-REFUSED")))
                    {
                        name = Regex.Replace(name, @"[\-\/\s]+", string.Empty);
                    }

                    PatientResults locVital = null;
                    Control ctrl2 = pnlVitals.FindControl("txtNotes" + name.Replace(" ", "").Replace("CDP", "").Replace("-", ""));
                    if (name == "Ultrasound - Aortic")
                        ctrl2 = pnlVitals.FindControl("txtNotes" + name.Replace(" ", "").Replace("-", "").Replace("CDP", ""));
                    if (ctrl2 != null && ctrl2.ID != string.Empty)
                    {
                        cb = (CustomDLCNew)ctrl2;
                    }
                    //HtmlSelect reasonnotPerformed = null;
                    //Control ctrlreason = pnlVitals.FindControl("Patient-Reason-refused-" + name.Replace(" ", "").Replace("-", ""));
                    //if (ctrlreason != null && ctrlreason.ID != string.Empty)
                    //{
                    //    reasonnotPerformed = (HtmlSelect)ctrlreason;
                    //}
                    name = sLoincname;
                    if (name.ToUpper().Contains("BP"))
                    {
                        string[] split = name.Split(' ');
                        if (split != null && split.Length > 0)
                        {
                            try
                            {
                                locVital = (from li in vitalList
                                            where li.Loinc_Observation == split[0] + " Location" && li.Encounter_ID == ClientSession.EncounterId
                                            select li).ToList<PatientResults>()[0];
                            }
                            catch
                            {
                                locVital = null;
                            }
                        }
                    }
                    if (ctrl != null)
                    {
                        if (upDateVital.Contains(name))
                        {
                            var updateVitals = from vital in objVitalDTO.VitalsList
                                               where vital.Vitals_Group_ID == DelUpdteGroupID && vital.Loinc_Observation == name
                                               select new { vital };
                            foreach (var item in updateVitals)
                            {
                                vitalsObj = item.vital;
                                if (ctrl.GetType().ToString().ToUpper().Contains("CHECKBOX") == true)
                                {
                                    vitalsObj.Value = GetCheckedValues(name).Trim();
                                    for (int j = 0; j < dynamicScreenList.Count; j++)
                                    {
                                        if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                        {
                                            vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                            vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                            vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;

                                        }
                                    }

                                    VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                                    for (int k = 0; k < mastervitalslist.Count; k++)
                                    {
                                        if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                        {
                                            vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                        }
                                    }
                                    if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                                    {
                                        if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))//&& ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                            continue;
                                    }
                                    else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false)
                                    {
                                        if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                        {
                                            string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                EnableDisbaleSave(true);
                                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                    divLoading.Style.Add("display", "none");
                                                else
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        string.Empty, "hideLoading();", true);
                                                return;
                                            }
                                        }
                                        else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                        {
                                            string egfrvalue = Request.Form["eGFR"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                EnableDisbaleSave(true);
                                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                    divLoading.Style.Add("display", "none");
                                                else
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        string.Empty, "hideLoading();", true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            EnableDisbaleSave(true);
                                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                divLoading.Style.Add("display", "none");
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                    string.Empty, "hideLoading();", true);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if (htUnitConvMthds.Contains(name))
                                    {
                                        vitalsObj.Value = ConversionOnSave(name).Trim();
                                        for (int j = 0; j < dynamicScreenList.Count; j++)
                                        {
                                            if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                            {
                                                //if (name.ToUpper() == "BMI")
                                                //{
                                                //    string status = Request.Form[pnlVitals.FindControl("BMI Status").ID].ToString();
                                                //    string[] code = dynamicScreenList[j].Snomed_Code.Split('|');
                                                //    for (int y = 0; y < code.Length; y++)
                                                //    {
                                                //        if (status.ToUpper() == code[y].Split(':')[0])
                                                //        {
                                                //            vitalsObj.Snomed_Code = code[y].Split(':')[1];
                                                //            break;
                                                //        }
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    vitalsObj.Snomed_Code = dynamicScreenList[j].Snomed_Code;
                                                //}
                                                vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                                vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                                vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;

                                            }
                                        }
                                        VitalName = string.Empty;
                                        VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                                        for (int k = 0; k < mastervitalslist.Count; k++)
                                        {

                                            if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                            {
                                                vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                            }
                                        }
                                        vitalsObj.Captured_date_and_time = VitalTakenDate;
                                        vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        if (vitalsObj.Value.Trim() == string.Empty)
                                        {
                                            if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                            {
                                                string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                                if (egfrvalue == string.Empty)
                                                {
                                                    if (allEmpty == true)
                                                    {
                                                        //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) &
                                                        if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                                        {
                                                            vitalsObj.Modified_By = ClientSession.UserName;
                                                            //vitalsObj.Modified_Date_And_Time = utc;
                                                            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                            delList.Add(vitalsObj);
                                                            if (locVital != null)
                                                            {
                                                                locVital.Modified_By = ClientSession.UserName;
                                                                //locVital.Modified_Date_And_Time = utc;
                                                                locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                                delList.Add(locVital);
                                                            }
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EnableDisbaleSave(true);
                                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                            divLoading.Style.Add("display", "none");
                                                        else
                                                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                                string.Empty, "hideLoading();", true);
                                                        return;
                                                    }
                                                }
                                            }
                                            else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                            {
                                                string egfrvalue = Request.Form["eGFR"].Trim();
                                                if (egfrvalue == string.Empty)
                                                {
                                                    if (allEmpty == true)
                                                    {
                                                        //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) && 
                                                        if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                                        {
                                                            vitalsObj.Modified_By = ClientSession.UserName;
                                                            // vitalsObj.Modified_Date_And_Time = utc;
                                                            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                            delList.Add(vitalsObj);
                                                            if (locVital != null)
                                                            {
                                                                locVital.Modified_By = ClientSession.UserName;
                                                                //locVital.Modified_Date_And_Time = utc;
                                                                locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                                delList.Add(locVital);
                                                            }
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EnableDisbaleSave(true);
                                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                            divLoading.Style.Add("display", "none");
                                                        else
                                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                        return;
                                                    }
                                                }
                                            }

                                            else
                                            {
                                                if (allEmpty == true)
                                                {
                                                    //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) && 
                                                    if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                                    {
                                                        vitalsObj.Modified_By = ClientSession.UserName;
                                                        //vitalsObj.Modified_Date_And_Time = utc;
                                                        vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                        delList.Add(vitalsObj);
                                                        if (locVital != null)
                                                        {
                                                            locVital.Modified_By = ClientSession.UserName;
                                                            //locVital.Modified_Date_And_Time = utc;
                                                            locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                            delList.Add(locVital);
                                                        }
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    EnableDisbaleSave(true);
                                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                        divLoading.Style.Add("display", "none");
                                                    else
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                                        {
                                            HtmlGenericControl ctrllbl = (HtmlGenericControl)ctrl;
                                            if (Request.Form[ctrllbl.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrllbl.ID.Trim()];
                                        }
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                                        {
                                            HtmlInputText ctrltxt = (HtmlInputText)ctrl;
                                            if (Request.Form[ctrltxt.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                            if (ctrltxt.ID != null && ctrltxt.ID.ToUpper().Contains("BMI") && ctrltxt.ID.ToUpper().Contains("STATUS"))
                                                vitalsObj.Value = ctrltxt.Value;
                                        }
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                        {
                                            HtmlTextArea ctrltxt = (HtmlTextArea)ctrl;
                                            if (Request.Form[ctrltxt.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                        }
                                        if (ctrl.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                                        {
                                            HtmlSelect ctrlcmb = (HtmlSelect)ctrl;
                                            if (Request.Form[ctrlcmb.ID.Trim()] != null)
                                                vitalsObj.Value = Request.Form[ctrlcmb.ID.Trim()];
                                            //vitalsObj.Value = ctrlcmb.Items[ctrlcmb.SelectedIndex].Text;
                                        }
                                        //if (ctrl.GetType().Name.ToUpper().Contains("RADMASKEDTEXTBOX"))
                                        //{
                                        //    RadMaskedTextBox ctrlcmb = (RadMaskedTextBox)ctrl;
                                        //    string[] aryFromDate = Request.Form[ctrlcmb.ID].Replace("____", "").Replace("___", "").Replace("__", "").Split('-');
                                        //    if (aryFromDate[2] != "" && aryFromDate[1] != "" && aryFromDate[0] != "")
                                        //    {
                                        //        vitalsObj.Value = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];
                                        //    }
                                        //    else if (aryFromDate[0] != "" && aryFromDate[1] != "" && aryFromDate[2] == "")
                                        //    {
                                        //        vitalsObj.Value = aryFromDate[1] + "-" + aryFromDate[0];
                                        //    }
                                        //    else if (aryFromDate[0] != "" && aryFromDate[1] == "" && aryFromDate[2] == "")
                                        //    {
                                        //        vitalsObj.Value = aryFromDate[0];
                                        //    }
                                        //    else
                                        //    {
                                        //        vitalsObj.Value = "";
                                        //    }

                                        //}

                                        for (int j = 0; j < dynamicScreenList.Count; j++)
                                        {
                                            if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                            {
                                                vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                                vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                                vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;
                                            }
                                        }
                                        VitalName = string.Empty;
                                        VitalName = vitalsObj.Loinc_Observation;
                                        if (VitalName.Contains("Second"))
                                        {
                                            VitalName = VitalName.Replace("Second", "");
                                        }
                                        for (int k = 0; k < mastervitalslist.Count; k++)
                                        {
                                            if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                            {
                                                vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                            }
                                        }

                                        vitalsObj.Captured_date_and_time = VitalTakenDate;
                                        vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        //((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)) &&
                                        if (vitalsObj.Value == "" && cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                        {
                                            vitalsObj.Modified_By = ClientSession.UserName;
                                            //vitalsObj.Modified_Date_And_Time = utc;
                                            vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                            delList.Add(vitalsObj);
                                            if (locVital != null)
                                            {
                                                locVital.Modified_By = ClientSession.UserName;
                                                //locVital.Modified_Date_And_Time = utc;
                                                locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                                delList.Add(locVital);
                                            }
                                            continue;
                                        }
                                    }
                                }
                                //To create object for BP Location
                                object[] lst = upDateVital.ToArray();
                                if (item.vital.Loinc_Observation.ToUpper().Contains("SYS/DIA") && item.vital.Loinc_Observation.ToUpper().IndexOf("STATUS") <= -1)
                                {
                                    if (!lst.Any(a => a.ToString().ToUpper() == (item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7) + "Location").ToUpper())) //item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7)+"Location")
                                    {
                                        if (name.ToUpper().Contains("BP") && (vitalsObj.Value != string.Empty || (cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() != "")))// || (reasonnotPerformed != null && reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text != "")))
                                        {
                                            string[] split = name.Split(' ');
                                            if (split != null && split.Length > 0)
                                            {
                                                try
                                                {
                                                    //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(split[0] + " Left");
                                                    //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(split[0] + " Right");
                                                    RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(split[0] + " Left");
                                                    RadioButton chkRight = (RadioButton)pnlVitals.FindControl(split[0] + " Right");
                                                    PatientResults vitalsLocationObj = CreateVitalsObject(GroupID);
                                                    vitalsLocationObj.Loinc_Observation = split[0] + " Location";
                                                    //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                                    //{
                                                    //    vitalsLocationObj.Value = "Left";
                                                    //}
                                                    //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                                    //{
                                                    //    vitalsLocationObj.Value = "Right";
                                                    //}

                                                    if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                                    {
                                                        vitalsLocationObj.Value = "Left";
                                                    }
                                                    else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                                    {
                                                        vitalsLocationObj.Value = "Right";
                                                    }
                                                    vitalsLocationObj.Captured_date_and_time = VitalTakenDate;
                                                    vitalsLocationObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                                    saveList.Add(vitalsLocationObj);

                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else if (lst.Any(a => a.ToString().ToUpper() == (item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7) + "Location").ToUpper()))
                                    {
                                        if (name.ToUpper().Contains("BP") && vitalsObj.Value == string.Empty && cb == null && Request.Form[cb.ID + "$txtDLC"].Trim() == "")
                                        {
                                            string Location = item.vital.Loinc_Observation.Substring(0, item.vital.Loinc_Observation.Length - 7) + "Location";
                                            if (vitalList.Any(a => a.Loinc_Observation.Trim() == Location.Trim()))
                                            {
                                                PatientResults objPatientResults = vitalList.Where(a => a.Loinc_Observation.Trim() == Location.Trim()).ToList<PatientResults>()[0];
                                                delList.Add(objPatientResults);
                                                locVital = null;
                                            }
                                        }
                                    }
                                }
                                if (cb != null)
                                {
                                    vitalsObj.Notes = Request.Form[cb.ID + "$txtDLC"].Trim();
                                    string snomedcode = "";
                                    string loinc_identifier = "";
                                    //BugID:47706
                                    if (vitalsObj.Notes != "" && cb.ID.IndexOf("LastMammogram") == -1)
                                        snomedcode = objUtilityManager.GetSnomedfromStaticLookup("ReasonNotPerformedList", cb.TextControlID.ToString(), cb.txtDLC.Text.ToString());
                                    else
                                        loinc_identifier = objUtilityManager.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", cb.txtDLC.Text.ToString());

                                    if (cb.ID.IndexOf("LastMammogram") != -1)
                                        vitalsObj.Loinc_Identifier = loinc_identifier;
                                    //string[] notes = Request.Form[cb.ID + "$txtDLC"].Trim().Split(',');
                                    //for (int h = 0; h < notes.Length; h++)
                                    //{
                                    //    var test = (from m in ilstLookupRefuse where m.Value == notes[h].Trim() select m.Description).ToArray();
                                    //    if (test.Count() > 0)
                                    //    {
                                    //        if (h == 0)
                                    //            snomedcode = test[0] + ",";
                                    //        else if (h == notes.Length - 1)
                                    //            snomedcode = snomedcode + test[0] + ",";
                                    //        else
                                    //            snomedcode = snomedcode + test[0];


                                    //    }

                                    //}
                                    vitalsObj.Snomed_Code = snomedcode;
                                }
                                //if (reasonnotPerformed != null)
                                //{
                                //    vitalsObj.Reason_For_Not_Performed = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text;
                                //    vitalsObj.Snomed_Code = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Value;
                                //}
                                vitalsObj.Modified_By = ClientSession.UserName;
                                //vitalsObj.Modified_Date_And_Time = utc;
                                vitalsObj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                vitalsObj.Results_Type = "Vitals";
                                updateList.Add(vitalsObj);
                                try
                                {
                                    if (locVital != null)
                                    {
                                        //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Left"));
                                        //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Right"));
                                        RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Left"));
                                        RadioButton chkRight = (RadioButton)pnlVitals.FindControl(locVital.Loinc_Observation.Replace("Location", "Right"));

                                        //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                        //{
                                        //    locVital.Value = "Left";
                                        //}
                                        //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                        //{
                                        //    locVital.Value = "Right";
                                        //}


                                        if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                        {
                                            locVital.Value = "Left";
                                        }
                                        else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                        {
                                            locVital.Value = "Right";
                                        }
                                        locVital.Captured_date_and_time = VitalTakenDate;
                                        locVital.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        locVital.Modified_By = ClientSession.UserName;
                                        //locVital.Modified_Date_And_Time = utc;
                                        locVital.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                        locVital.Results_Type = "Vitals";
                                        updateList.Add(locVital);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        else if (!(upDateVital.Contains(name)))
                        {
                            vitalsObj = CreateVitalsObject(DelUpdteGroupID);
                            vitalsObj.Loinc_Observation = name;
                            for (int j = 0; j < dynamicScreenList.Count; j++)
                            {
                                if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                {
                                    vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                    vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                    vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;

                                }
                            }
                            VitalName = string.Empty;
                            VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                            for (int k = 0; k < mastervitalslist.Count; k++)
                            {

                                if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                {
                                    vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                }
                            }
                            if (ctrl.GetType().ToString().ToUpper().Contains("CHECKBOX") == true)
                            {
                                vitalsObj.Value = GetCheckedValues(name).Trim();
                                if (vitalsObj.Value.Trim() == string.Empty && allEmpty == true)
                                {
                                    if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                        continue;
                                }
                                else if (vitalsObj.Value.Trim() == string.Empty && allEmpty == false)
                                {
                                    if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                    {
                                        string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                        if (egfrvalue == string.Empty)
                                        {
                                            EnableDisbaleSave(true);
                                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                divLoading.Style.Add("display", "none");
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                            return;
                                        }
                                    }
                                    else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                    {
                                        string egfrvalue = Request.Form["eGFR"].Trim();
                                        if (egfrvalue == string.Empty)
                                        {
                                            EnableDisbaleSave(true);
                                            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                divLoading.Style.Add("display", "none");
                                            else
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        EnableDisbaleSave(true);
                                        if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                            divLoading.Style.Add("display", "none");
                                        else
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (htUnitConvMthds.Contains(name))
                                {
                                    vitalsObj.Value = ConversionOnSave(name).Trim();
                                    for (int j = 0; j < dynamicScreenList.Count; j++)
                                    {
                                        if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                        {
                                            vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                            vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                            vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;

                                        }
                                    }
                                    VitalName = string.Empty;
                                    VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                                    for (int k = 0; k < mastervitalslist.Count; k++)
                                    {

                                        if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                        {
                                            vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                        }
                                    }
                                    vitalsObj.Captured_date_and_time = VitalTakenDate;
                                    vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                    if (vitalsObj.Value == string.Empty)
                                    {
                                        if (vitalsObj.Loinc_Observation.Contains("eGFR StatusSecond"))
                                        {
                                            string egfrvalue = Request.Form["eGFRSecond"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                if (allEmpty == true)
                                                {
                                                    if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    EnableDisbaleSave(true);
                                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                        divLoading.Style.Add("display", "none");
                                                    else
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                    return;
                                                }
                                            }
                                        }
                                        else if (vitalsObj.Loinc_Observation.Contains("eGFR Status"))
                                        {
                                            string egfrvalue = Request.Form["eGFR"].Trim();
                                            if (egfrvalue == string.Empty)
                                            {
                                                if (allEmpty)
                                                {
                                                    if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    EnableDisbaleSave(true);
                                                    if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                        divLoading.Style.Add("display", "none");
                                                    else
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {

                                            if (allEmpty)
                                            {
                                                if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                                {
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                EnableDisbaleSave(true);
                                                if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                                                    divLoading.Style.Add("display", "none");
                                                else
                                                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                                    {
                                        HtmlGenericControl ctrllbl = (HtmlGenericControl)ctrl;
                                        if (Request.Form[ctrllbl.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrllbl.ID.Trim()];
                                    }
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                                    {
                                        HtmlInputText ctrltxt = (HtmlInputText)ctrl;
                                        if (Request.Form[ctrltxt.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                        if (ctrltxt.ID != null && ctrltxt.ID.ToUpper().Contains("BMI") && ctrltxt.ID.ToUpper().Contains("STATUS"))
                                            vitalsObj.Value = ctrltxt.Value;
                                    }
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                    {
                                        HtmlTextArea ctrltxt = (HtmlTextArea)ctrl;
                                        if (Request.Form[ctrltxt.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrltxt.ID.Trim()];
                                    }
                                    if (ctrl.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                                    {
                                        HtmlSelect ctrlcmb = (HtmlSelect)ctrl;
                                        if (Request.Form[ctrlcmb.ID.Trim()] != null)
                                            vitalsObj.Value = Request.Form[ctrlcmb.ID.Trim()];
                                        //vitalsObj.Value = ctrlcmb.Items[ctrlcmb.SelectedIndex].Text;
                                    }
                                    //if (ctrl.GetType().Name.ToUpper().Contains("RADMASKEDTEXTBOX"))
                                    //{
                                    //    RadMaskedTextBox ctrlcmb = (RadMaskedTextBox)ctrl;
                                    //    string[] aryFromDate = Request.Form[ctrlcmb.ID].Replace("____", "").Replace("___", "").Replace("__", "").Split('-');
                                    //    if (aryFromDate[2] != "" && aryFromDate[1] != "" && aryFromDate[0] != "")
                                    //    {
                                    //        vitalsObj.Value = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];
                                    //    }
                                    //    else if (aryFromDate[0] != "" && aryFromDate[1] != "" && aryFromDate[2] == "")
                                    //    {
                                    //        vitalsObj.Value = aryFromDate[1] + "-" + aryFromDate[0];
                                    //    }
                                    //    else if (aryFromDate[0] != "" && aryFromDate[1] == "" && aryFromDate[2] == "")
                                    //    {
                                    //        vitalsObj.Value = aryFromDate[0];
                                    //    }
                                    //    else
                                    //    {
                                    //        vitalsObj.Value = "";
                                    //    }

                                    //}

                                    for (int j = 0; j < dynamicScreenList.Count; j++)
                                    {
                                        if (dynamicScreenList[j].Control_Name_Thin_Client == vitalsObj.Loinc_Observation)
                                        {
                                            vitalsObj.Loinc_Identifier = dynamicScreenList[j].Loinc_Identifier;
                                            vitalsObj.Acurus_Result_Code = dynamicScreenList[j].Acurus_Result_Code;
                                            vitalsObj.Acurus_Result_Description = dynamicScreenList[j].Acurus_Result_Description;
                                        }
                                    }
                                    VitalName = string.Empty;
                                    VitalName = vitalsObj.Loinc_Observation.Replace("Second", "");

                                    for (int k = 0; k < mastervitalslist.Count; k++)
                                    {
                                        if (mastervitalslist[k].Vital_Name.Trim().ToUpper() == VitalName.Trim().ToUpper())
                                        {
                                            vitalsObj.Units = mastervitalslist[k].Vital_Unit;
                                        }
                                    }
                                    vitalsObj.Captured_date_and_time = VitalTakenDate;
                                    vitalsObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                    if (vitalsObj.Value == string.Empty)
                                    {
                                        if (((cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() == string.Empty) || (cb == null)))// && ((reasonnotPerformed != null && Request.Form[reasonnotPerformed.ID].Trim() == string.Empty) || (reasonnotPerformed == null)))
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            if (cb != null)
                            {
                                vitalsObj.Notes = Request.Form[cb.ID + "$txtDLC"].Trim();
                                string snomedcode = "";
                                string loinc_identifier = "";
                                //BugID:47706
                                if (vitalsObj.Notes != "" && cb.ID.IndexOf("LastMammogram") == -1)
                                    snomedcode = objUtilityManager.GetSnomedfromStaticLookup("ReasonNotPerformedList", cb.TextControlID.ToString(), cb.txtDLC.Text.ToString());
                                else
                                    loinc_identifier = objUtilityManager.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", cb.txtDLC.Text.ToString());

                                if (cb.ID.IndexOf("LastMammogram") != -1)
                                    vitalsObj.Loinc_Identifier = loinc_identifier;
                                //string[] notes = Request.Form[cb.ID + "$txtDLC"].Trim().Split(',');
                                //for (int h = 0; h < notes.Length; h++)
                                //{
                                //    var test = (from m in ilstLookupRefuse where m.Value == notes[h].Trim() select m.Description).ToArray();
                                //    if (test.Count() > 0)
                                //    {
                                //        if (h == 0)
                                //            snomedcode = test[0] + ",";
                                //        else if (h == notes.Length - 1)
                                //            snomedcode = snomedcode + test[0] + ",";
                                //        else
                                //            snomedcode = snomedcode + test[0];


                                //    }

                                //}
                                vitalsObj.Snomed_Code = snomedcode;
                            }
                            //if (reasonnotPerformed != null)
                            //{
                            //    vitalsObj.Reason_For_Not_Performed = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text;
                            //    vitalsObj.Snomed_Code = reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Value;
                            //}
                            //To create object for BP Location
                            if (name.ToUpper().Contains("BP") && name.ToUpper().IndexOf("STATUS") <= -1 && (vitalsObj.Value != string.Empty || (cb != null && Request.Form[cb.ID + "$txtDLC"].Trim() != "")))// || (reasonnotPerformed != null && reasonnotPerformed.Items[reasonnotPerformed.SelectedIndex].Text != "")))
                            {
                                string[] split = name.Split(' ');
                                if (split != null && split.Length > 0)
                                {
                                    try
                                    {
                                        //CheckBox chkLeft = (CheckBox)pnlVitals.FindControl(split[0] + " Left");
                                        //CheckBox chkRight = (CheckBox)pnlVitals.FindControl(split[0] + " Right");

                                        RadioButton chkLeft = (RadioButton)pnlVitals.FindControl(split[0] + " Left");
                                        RadioButton chkRight = (RadioButton)pnlVitals.FindControl(split[0] + " Right");


                                        PatientResults vitalsLocationObj = CreateVitalsObject(DelUpdteGroupID);
                                        vitalsLocationObj.Loinc_Observation = split[0] + " Location";
                                        //if (Request.Form[chkLeft.ID] != null && Request.Form[chkLeft.ID].Trim() == "on")
                                        //{
                                        //    vitalsLocationObj.Value = "Left";
                                        //}
                                        //else if (Request.Form[chkRight.ID] != null && Request.Form[chkRight.ID].Trim() == "on")
                                        //{
                                        //    vitalsLocationObj.Value = "Right";
                                        //}

                                        if (Request.Form[chkLeft.ID] != null && chkLeft != null && chkLeft.ID != null && chkLeft.Checked == true)
                                        {
                                            vitalsLocationObj.Value = "Left";
                                        }
                                        else if (Request.Form[chkRight.ID] != null && chkRight != null && chkRight.ID != null && chkRight.Checked == true)
                                        {
                                            vitalsLocationObj.Value = "Right";
                                        }

                                        vitalsLocationObj.Captured_date_and_time = VitalTakenDate;
                                        vitalsLocationObj.Local_Time = UtilityManager.ConvertToLocal(VitalTakenDate).ToString("yyyy-MM-dd hh:mm:ss tt");
                                        saveList.Add(vitalsLocationObj);

                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            saveList.Add(vitalsObj);
                        }
                    }
                }
            }
            if (updateList != null && saveList != null)
            {
                if (updateList.Count > 0 || saveList.Count > 0)
                {
                    ulong human_id = 0;
                    if (updateList.Count > 0)
                        human_id = updateList[0].Human_ID;
                    else
                        human_id = saveList[0].Human_ID;
                    objVitalDTO = vitalmngr.UpdateVitalDetails(updateList.ToArray<PatientResults>(), saveList.ToArray<PatientResults>(), delList.ToArray<PatientResults>(), human_id, 1, 20, string.Empty, 0, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID, ClientSession.UserName);
                    Session["vitalList"] = vitalList;
                    //SetVitalsToolTip(string.Empty);
                    //  SetVitalsToolTip(objVitalDTO.VitalsList);
                    EnableDisbaleSave(false);
                    if (sForClose == "Yes")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", "DisplayErrorMessage('200002');ForClosingVitalsForYes();CurrentSystemTime();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateVitals", "DisplayErrorMessage('200002');CurrentSystemTime();", true);
                    }
                }
            }
            vitalList = objVitalDTO.VitalsList;
            // Session["vitalList"] = vitalList;
            Session["objVitalDTO"] = objVitalDTO;

            ClearText();
            PastVitals(objVitalDTO.VitalsList);
            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                divLoading.Style.Add("display", "none");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
            btnSaveVitals.Disabled = true;
        }
        public void EditCellClickforVitals(bool Is_from_followup)
        {
            bool bIs_follow_up = false;
            PatientResultsDTO objVitalDTO = null;
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();

            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }
            IList<PatientResults> vitalList = new List<PatientResults>();
            if (Session["vitalList"] != null)
            {
                vitalList = (IList<PatientResults>)Session["vitalList"];
            }
            ulong GroupID = 0;
            if (Session["GroupID"] != null)
                GroupID = Convert.ToUInt64(Session["GroupID"]);
            bIs_follow_up = Is_from_followup;
            var vit = (from li in vitalList
                       where li.Encounter_ID == ClientSession.EncounterId
                       select li);
            if (vit != null && vit.Count() != 0)
            {
                foreach (var v in vit)
                {
                    string Loinc_observation = v.Loinc_Observation;
                    if (v.Loinc_Observation.ToUpper().Contains("BP") || v.Loinc_Observation.ToUpper().Contains("BLOOD") || v.Loinc_Observation.ToUpper().Contains("BODY"))
                    {
                        v.Loinc_Observation = Regex.Replace(v.Loinc_Observation, @"[\-\/\s]+", string.Empty);
                    }

                    Control ctrDtp = pnlVitals.FindControl("dtp" + v.Loinc_Observation.Replace(" ", string.Empty));

                    if (ctrDtp != null)
                    {
                        HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
                        if (dtpInAdd != null)
                        {
                            DateTime temp = DateTime.MinValue;
                            if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
                            {
                                try
                                {
                                    if (Request["openingfrom"] != "Menu")
                                        temp = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                                    else
                                        temp = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                                }
                                catch { }
                            }
                            if (UtilityManager.ConvertToLocal(v.Captured_date_and_time) > temp)
                            {
                                if (ClientSession.PatientPaneList.Count > 0)
                                {
                                    IList<PatientPane> lstPatientPane = new List<PatientPane>();
                                    var PatientPaneList = from d in ClientSession.PatientPaneList where d.Encounter_ID == ClientSession.EncounterId select d;
                                    if (PatientPaneList.Count() > 0)
                                    {
                                        lstPatientPane = PatientPaneList.ToList<PatientPane>();
                                        if (lstPatientPane != null && lstPatientPane.Count > 0)
                                        {
                                            dtpInAdd.Value = UtilityManager.ConvertToLocal(lstPatientPane[0].Date_of_Service).ToString();
                                            arydatetimeFilled.Add(dtpInAdd.ID.ToString());
                                        }
                                    }

                                }
                            }
                            else
                            {
                                // dtpInAdd.Value = UtilityManager.ConvertToLocal(v.Captured_date_and_time).ToString();
                                dtpInAdd.Value = UtilityManager.ConvertToLocal(v.Captured_date_and_time).ToString("dd-MMM-yyyy hh:mm tt");
                                arydatetimeFilled.Add(dtpInAdd.ID.ToString());

                                Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_dtp" + v.Loinc_Observation.Replace(" ", ""));
                                if (ctrDtpTime != null)
                                {
                                    MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                                    if (dtpInAddTime != null)
                                    {
                                        DateTime CapturedTime = new DateTime();
                                        CapturedTime = UtilityManager.ConvertToLocal(v.Captured_date_and_time);
                                        dtpInAddTime.Hour = CapturedTime.Hour;
                                        dtpInAddTime.Hour = CapturedTime.Hour == 0 ? getHour(CapturedTime.ToString()) : CapturedTime.Hour;
                                        dtpInAddTime.Minute = CapturedTime.Minute;
                                        dtpInAddTime.AmPm = CapturedTime.ToString("tt") == "AM" ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM;

                                    }
                                }
                            }
                        }
                    }
                    Control ctrDtpnew = pnlVitals.FindControl(v.Loinc_Observation.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                    if (ctrDtpnew != null && ctrDtpnew.ID != string.Empty)
                    {
                        if (ctrDtpnew.ID.ToString().ToUpper().Contains("DATEPICKER") == true && ctrDtpnew.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        // if (ct.ID.ToString().ToUpper().Contains("DATEPICKER") == true && ct.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        {
                            HtmlInputText dtp = (HtmlInputText)ctrDtpnew;
                            if (v.Value != "" && v.Value.Replace(" ", "").Length == 11)
                            {
                                //DateTime dt = DateTime.ParseExact(v.Value, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                dtp.Value = Convert.ToDateTime(v.Value).ToString("yyyy-MMM-dd");
                            }
                            else if (v.Value != "" && v.Value.Replace(" ", "").Length == 4)
                            {
                                dtp.Value = Convert.ToDateTime("01-Jan-" + v.Value).ToString("dd-MMM-yyyy");
                            }
                            else if (v.Value != "" && v.Value.Replace(" ", "").Length == 8)
                            {
                                dtp.Value = Convert.ToDateTime("01-" + v.Value).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                dtp.Value = "";
                            }
                        }
                    }
                    if (v.Loinc_Observation.ToUpper().Contains("BLOOD") || v.Loinc_Observation.ToUpper().Contains("BODY"))
                        v.Loinc_Observation = Loinc_observation;

                    Control ctBP = pnlVitals.FindControl(v.Loinc_Observation);
                    int indexOfControl = pnlVitals.Controls.IndexOf(ctBP);
                    if (ctBP != null && ctBP.ID != string.Empty)
                    {
                        if (v.Loinc_Observation == "Vision-Left")
                        {
                            HtmlInputText ctnum = (HtmlInputText)ctBP;
                            string[] visionVal = v.Value.Split('/');
                            if (visionVal.Length > 0)
                            {
                                ctnum.Value = visionVal[0];
                            }
                            Control ctrl1 = pnlVitals.FindControl("Vision-Lefts");
                            // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                            HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                            if (visionVal.Length > 1)
                                ctnum1.Value = visionVal[1];
                            else
                                ctnum1.Value = "";
                        }
                        if (v.Loinc_Observation.StartsWith("Vision-Right"))
                        {
                            HtmlInputText ctnum = (HtmlInputText)ctBP;
                            string[] visionVal = v.Value.Split('/');
                            if (visionVal.Length > 0)
                            {
                                ctnum.Value = visionVal[0];
                            }
                            Control ctrl1 = pnlVitals.FindControl("Vision-Rights");
                            // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                            HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                            if (visionVal.Length > 1)
                                ctnum1.Value = visionVal[1];
                            else
                                ctnum1.Value = "";
                        }
                        if (v.Loinc_Observation.StartsWith("Vision-Both"))
                        {
                            HtmlInputText ctnum = (HtmlInputText)ctBP;
                            string[] visionVal = v.Value.Split('/');
                            if (visionVal.Length > 0)
                            {
                                ctnum.Value = visionVal[0];
                            }
                            Control ctrl1 = pnlVitals.FindControl("Vision-Boths");
                            // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                            HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                            if (visionVal.Length > 1)
                                ctnum1.Value = visionVal[1];
                            else
                                ctnum1.Value = "";
                        }
                        if (v.Loinc_Observation.StartsWith("BP"))
                        {
                            // RadNumericTextBox ctnum = (RadNumericTextBox)ctBP;
                            if (v.Value.Contains("/"))
                            {
                                HtmlInputText ctnum = (HtmlInputText)ctBP;
                                string[] bpVal = v.Value.Split('/');
                                if (bpVal.Length > 0)
                                {
                                    ctnum.Value = bpVal[0];
                                    if (ctnum.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum.ID.ToString());

                                    }
                                    if (v.Loinc_Observation.Contains("Sitting") && !v.Loinc_Observation.Contains("Second"))
                                    {
                                        Control ctrl1 = pnlVitals.FindControl("BPSittingDiastolic");
                                        // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                                        HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                                        ctnum1.Value = bpVal[1];
                                        if (ctnum1.Value != string.Empty)
                                        {
                                            arynumericFilled.Add(ctnum1.ID.ToString());
                                        }
                                    }
                                    if (v.Loinc_Observation.Contains("Sitting") && v.Loinc_Observation.Contains("Second"))
                                    {
                                        Control ctrl1 = pnlVitals.FindControl("BPSittingSecondDiastolic");
                                        // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                        HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                        ctnum1.Value = bpVal[1];
                                        if (ctnum1.Value != string.Empty)
                                        {
                                            arynumericFilled.Add(ctnum1.ID.ToString());

                                        }
                                    }
                                    if (v.Loinc_Observation.Contains("Standing") && !v.Loinc_Observation.Contains("Second"))
                                    {
                                        Control ctrl1 = pnlVitals.FindControl("BPStandingDiastolic");

                                        HtmlInputText ctnum1 = (HtmlInputText)ctrl1;

                                        // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                        ctnum1.Value = bpVal[1];
                                        if (ctnum1.Value != string.Empty)
                                        {
                                            arynumericFilled.Add(ctnum1.ID.ToString());
                                        }
                                    }
                                    if (v.Loinc_Observation.Contains("Standing") && v.Loinc_Observation.Contains("Second"))
                                    {
                                        Control ctrl1 = pnlVitals.FindControl("BPStandingSecondDiastolic");
                                        // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                        HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                        ctnum1.Value = bpVal[1];
                                        if (ctnum1.Value != string.Empty)
                                        {
                                            arynumericFilled.Add(ctnum1.ID.ToString());
                                        }
                                    }
                                    if (v.Loinc_Observation.Contains("Lying") && !v.Loinc_Observation.Contains("Second"))
                                    {
                                        Control ctrl1 = pnlVitals.FindControl("BPLyingDiastolic");
                                        // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                        HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                        ctnum1.Value = bpVal[1];
                                        if (ctnum1.Value != string.Empty)
                                        {
                                            arynumericFilled.Add(ctnum1.ID.ToString());

                                        }
                                    }
                                    if (v.Loinc_Observation.Contains("Lying") && v.Loinc_Observation.Contains("Second"))
                                    {
                                        Control ctrl1 = pnlVitals.FindControl("BPLyingSecondDiastolic");
                                        // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                        HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                        ctnum1.Value = bpVal[1];
                                        if (ctnum1.Value != string.Empty)
                                        {
                                            arynumericFilled.Add(ctnum1.ID.ToString());
                                        }
                                    }

                                    string[] split = Loinc_observation.Split(' ');
                                    if (split != null && split.Length > 0)
                                    {
                                        try
                                        {
                                            PatientResults locVital = (from li in vitalList
                                                                       where li.Loinc_Observation == split[0] + " Location" && li.Encounter_ID == ClientSession.EncounterId
                                                                       select li).ToList<PatientResults>()[0];
                                            if (locVital != null)
                                            {
                                                if (locVital.Value.ToUpper().Contains("LEFT"))
                                                {
                                                    //CheckBox chk = (CheckBox)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                    //chk.Checked = true;
                                                    //CheckBox chk1 = (CheckBox)pnlVitals.FindControl(split[0] + " " + "Right");
                                                    //chk1.Checked = false;
                                                    RadioButton chk = (RadioButton)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                    chk.Checked = true;
                                                    RadioButton chk1 = (RadioButton)pnlVitals.FindControl(split[0] + " " + "Right");
                                                    chk1.Checked = false;
                                                }
                                                else if (locVital.Value.ToUpper().Contains("RIGHT"))
                                                {
                                                    //CheckBox chk = (CheckBox)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                    //chk.Checked = true;
                                                    //CheckBox chk1 = (CheckBox)pnlVitals.FindControl(split[0] + " " + "Left");
                                                    //chk1.Checked = false;
                                                    RadioButton chk = (RadioButton)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                    chk.Checked = true;
                                                    RadioButton chk1 = (RadioButton)pnlVitals.FindControl(split[0] + " " + "Left");
                                                    chk1.Checked = false;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                            if (v.Loinc_Observation.Contains("Status") && v.Loinc_Observation.Contains("BP"))
                            {

                                if (v.Loinc_Observation.Contains("Sitting") && !v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPSittingSysDiaStatus");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                                    ctnum1.Value = v.Value;
                                    string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctnum1.Value));
                                    ctnum1.Style.Add("color", color);
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }
                                else if (v.Loinc_Observation.Contains("Sitting") && v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPSittingSecondSysDiaStatus");

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;

                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    ctnum1.Value = v.Value;
                                    string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctnum1.Value));
                                    ctnum1.Style.Add("color", color);
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }
                                else if (v.Loinc_Observation.Contains("Standing") && !v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPStandingSysDiaStatus");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                                    ctnum1.Value = v.Value;
                                    string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctnum1.Value));
                                    ctnum1.Style.Add("color", color);
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }
                                else if (v.Loinc_Observation.Contains("Standing") && v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPStandingSecondSysDiaStatus");

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;

                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    ctnum1.Value = v.Value;
                                    string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctnum1.Value));
                                    ctnum1.Style.Add("color", color);
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }
                                else if (v.Loinc_Observation.Contains("Lying") && !v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPLyingSysDiaStatus");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                                    ctnum1.Value = v.Value;
                                    string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctnum1.Value));
                                    ctnum1.Style.Add("color", color);
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }
                                else if (v.Loinc_Observation.Contains("Lying") && v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPLyingSecondSysDiaStatus");

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;

                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    ctnum1.Value = v.Value;
                                    string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctnum1.Value));
                                    ctnum1.Style.Add("color", color);
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }
                            }
                        }
                    }

                    string Notes = v.Loinc_Observation;
                    if (v.Loinc_Observation.Contains(" ") || v.Loinc_Observation.Contains("-") || v.Loinc_Observation.Contains("/"))
                    {
                        Notes = v.Loinc_Observation.Replace(" ", "");
                        if (Notes.Contains("-"))
                            Notes = Notes.Replace("-", "");
                        else if (Notes.Contains("/"))
                            Notes = Notes.Replace("/", "");
                    }
                    Control ctl = pnlVitals.FindControl("txtNotes" + Notes.Replace("CDP", "").Replace("CDP", "").Replace("-", ""));
                    if (ctl != null && ctl.ID != string.Empty)
                    {
                        CustomDLCNew comb = (CustomDLCNew)ctl;
                        comb.txtDLC.Text = v.Notes;
                        if (comb.txtDLC.Text != string.Empty)
                        {
                            aryNotesFilled.Add(comb.ID.ToString());
                        }
                    }
                    //Control reason = pnlVitals.FindControl("Patient-Reason-refused-" + Notes.Replace("-", ""));
                    //if (reason != null && reason.ID != string.Empty)
                    //{
                    //    HtmlSelect comb = (HtmlSelect)reason;
                    //    comb.Value = v.Snomed_Code;
                    //    if (comb.Value != string.Empty)
                    //    {
                    //        aryNotesFilled.Add(comb.ID.ToString());
                    //    }
                    //}
                    v.Loinc_Observation = Loinc_observation;
                    Control ct = pnlVitals.FindControl(v.Loinc_Observation);
                    if (ct != null && ct.ID != string.Empty)
                    {
                        if (ct.GetType().ToString().ToUpper().Contains("DATEPICKER") == true && ct.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        // if (ct.ID.ToString().ToUpper().Contains("DATEPICKER") == true && ct.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        {
                            HtmlInputText dtp = (HtmlInputText)ct;
                            DateTime dt = DateTime.ParseExact(v.Value, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            dtp.Value = dt.ToString(); ;

                        }
                        else if (ct.GetType().ToString().ToUpper().Contains("CHECK") == true)
                        {

                            Control ctrChk = pnlVitals.FindControl(v.Loinc_Observation);
                            if (ctrChk != null)
                            {
                                CheckBox chkYes = (CheckBox)ctrChk;
                                if (v.Value == "Y")
                                {
                                    chkYes.Checked = true;
                                }
                            }
                            Control ctrChkNo = pnlVitals.FindControl("chk" + v.Loinc_Observation);
                            if (ctrChkNo != null)
                            {
                                CheckBox chkNo = (CheckBox)ctrChkNo;
                                if (v.Value == "N")
                                {
                                    chkNo.Checked = true;
                                }
                            }

                        }
                        else if (ct.ID.ToString().ToUpper().Contains("DATEPICKER") == true)
                        //if (ct.GetType().ToString().ToUpper().Contains("RADMASKEDTEXTBOX") == true)
                        {
                            // RadMaskedTextBox dtpDate = ((RadMaskedTextBox)pnlVitals.FindControl(ct.ID));
                            HtmlInputText dtpDate = ((HtmlInputText)pnlVitals.FindControl(ct.ID));
                            if (dtpDate != null)
                            {
                                dtpDate.Value = v.Value;
                                //if (v.Value != string.Empty)
                                //{
                                //    string[] aryFromDate = v.Value.Split('-');
                                //    if (aryFromDate.Length == 3)
                                //    {
                                //        if (aryFromDate[0].Trim() != string.Empty && aryFromDate[1].Trim() != string.Empty && aryFromDate[2].Trim() != string.Empty)
                                //        {
                                //            if (aryFromDate[0].Trim().Length == 1)
                                //                dtpDate.Text = aryFromDate[2] + "-" + aryFromDate[1] + "-" + "0" + aryFromDate[0].Trim();
                                //            else
                                //                dtpDate.Text = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];

                                //        }
                                //    }
                                //    else if (aryFromDate.Length == 2)
                                //        dtpDate.Text = aryFromDate[1] + "-" + aryFromDate[0];
                                //    else
                                //        dtpDate.Text = v.Value;
                                //}
                            }
                        }
                        else
                        {
                            if (htRetriveUnitMthds.Contains(v.Loinc_Observation))
                            {
                                string sValue = string.Empty;
                                if (v.Value.Contains("'") == true && v.Value.Contains("''") == true)
                                    sValue = v.Value;
                                else
                                    sValue = ConversionOnRetrieval(v.Loinc_Observation, v.Value);
                                if (sValue.Contains("'") == true && sValue.Contains("''") == true)
                                {
                                    string[] Splitter = { "'", "''" };
                                    string[] feetInch = sValue.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
                                    if (feetInch.Length > 0)
                                    {
                                        HtmlInputText ctrltxt = (HtmlInputText)ct;
                                        ctrltxt.Value = feetInch[0].Trim();
                                        if (ctrltxt.Value != string.Empty)
                                        {
                                            aryNotesFilled.Add(ctrltxt.ID.ToString());

                                        }
                                        Control ctrl1 = pnlVitals.FindControl("HeightInch");
                                        HtmlInputText ctrltxt1 = (HtmlInputText)ctrl1;
                                        ctrltxt1.Value = feetInch[1].Trim();
                                        if (ctrltxt1.Value != string.Empty)
                                        {
                                            aryNotesFilled.Add(ctrltxt1.ID.ToString());

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ct.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT") && ct.ID.ToString().ToUpper().IndexOf("VISION") <= -1)
                                {
                                    HtmlInputText ctrllbl = (HtmlInputText)ct;
                                    ctrllbl.Value = v.Value;
                                    if (ctrllbl.Value != string.Empty)
                                    {
                                        aryLabelFilled.Add(ctrllbl.ID.ToString());
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT") && ct.ID.ToString().ToUpper().IndexOf("VISION") <= -1)
                                {
                                    HtmlInputText ctrltxt = (HtmlInputText)ct;
                                    ctrltxt.Value = v.Value;
                                    if (ctrltxt.Value != string.Empty)
                                    {
                                        aryNotesFilled.Add(ctrltxt.ID.ToString());
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                {
                                    HtmlTextArea ctrltxtarea = (HtmlTextArea)ct;
                                    ctrltxtarea.Value = v.Value;
                                    if (ctrltxtarea.Value.Length > 15)
                                        ctrltxtarea.Style.Add("Height", "29px");
                                    else
                                        ctrltxtarea.Style.Add("Height", "18px");
                                    if (ctrltxtarea.Value != string.Empty)
                                    {
                                        aryLabelFilled.Add(ctrltxtarea.ID.ToString());
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                                {
                                    HtmlSelect ctrlcmb = (HtmlSelect)ct;
                                    for (int i = 0; i < ctrlcmb.Items.Count; i++)
                                    {
                                        if (ctrlcmb.Items[i].Text == v.Value)
                                            ctrlcmb.SelectedIndex = i;
                                    }

                                }
                            }

                            if (htUnitConvMthds.Contains(v.Loinc_Observation) && v.Loinc_Observation != "BMI" && v.Loinc_Observation != "BP-Sitting Sys/Dia")
                            {
                                if (ct.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                                {
                                    HtmlGenericControl ctrllbl = (HtmlGenericControl)ct;
                                    ctrllbl.Style.Add("color", Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctrllbl.InnerHtml)));
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                                {
                                    if (v.Loinc_Observation.ToUpper() != "HEIGHT" && !(v.Loinc_Observation.ToUpper().IndexOf("VISION") > -1))
                                    {
                                        HtmlInputText ctrltxt = (HtmlInputText)ct;
                                        ctrltxt.Style.Add("color", Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctrltxt.Value)));
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                {
                                    HtmlTextArea ctrltxt = (HtmlTextArea)ct;
                                    ctrltxt.Style.Add("color", Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctrltxt.Value)));
                                }

                            }
                            else if (v.Loinc_Observation.Contains("BMI Status"))
                            {
                                HtmlInputText lblstatus = (HtmlInputText)ct;

                                //  HtmlTextArea lblstatus = (HtmlTextArea)ct;
                                string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, lblstatus.Value));
                                lblstatus.Style.Add("color", color);
                            }
                        }
                    }




                }
            }

            // btnSaveVitals.Value = "Save";

            //System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanSave");
            // text1.InnerText = "S";
            //System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
            // text2.InnerText = "ave";

            if (IsLoad)
                EnableDisbaleSave(false);
            else
                EnableDisbaleSave(true);

            if (objVitalDTO != null)
            {
                GroupID = objVitalDTO.MaxGroupId;
                Session["GroupID"] = GroupID;
            }

        }
        public string ConversionOnRetrieval(string vitalName, string vitalValue)
        {
            Hashtable htRetriveUnitMthds = new Hashtable();
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }

            int j = 0;
            string MethdName = string.Empty;
            if (htRetriveUnitMthds.Contains(vitalName))
            {
                MethdName = htRetriveUnitMthds[vitalName].ToString();
            }
            string[] Splitter = { ".", "(", ",", ")" };
            string[] MthdInfo = MethdName.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
            if (MthdInfo.Length > 0)
            {
                string[] Arguments = new string[MthdInfo.Length - 2];
                string ClassName = MthdInfo[0];
                string MethodName = MthdInfo[1];
                Arguments[j] = vitalValue;
                j++;
                return objUiManager.InvokeMethod(ClassName, MethodName, Arguments);
            }
            else
                return string.Empty;
        }

        public void ClearText()
        {
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();
            ArrayList vitalArray = new ArrayList();
            IList<DynamicScreen> dynamicScreenList = new List<DynamicScreen>();
            PatientResultsDTO objVitalDTO = null;

            if (Session["dynamicScreenList"] != null)
                dynamicScreenList = (IList<DynamicScreen>)Session["dynamicScreenList"];

            if (Session["vitalArray"] != null)
            {
                vitalArray = (ArrayList)Session["vitalArray"];
            }
            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }
            pnlVitals.Controls.Clear();
            htDisplayText.Clear();
            htRetriveUnitMthds.Clear();
            htUnitConvMthds.Clear();
            vitalArray.Clear();
            CreateControls(dynamicScreenList);
            IsLoad = true;
            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                divLoading.Style.Add("display", "none");
            else

                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

            // ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
            btnSaveVitals.Disabled = false;

            DateTime VitalTakenDateValue = DateTime.MinValue;
            string openingfrom = string.Empty;
            openingfrom = Request["openingfrom"].ToString();

            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["VitalTakenDateValue"] != null)
                VitalTakenDateValue = Convert.ToDateTime(Session["VitalTakenDateValue"]);
            if (openingfrom == "Menu")
            {
                if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
                {
                    DateTime localDate = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                    VitalTakenDateValue = localDate;
                }
            }

            bool formLoad = false;
            formLoad = true;

            if (aryLabelFilled != null && aryLabelFilled.Count > 0)
            {
                for (int i = 0; i < aryLabelFilled.Count; i++)
                {
                    Control ctrl1 = pnlVitals.FindControl(aryLabelFilled[i].ToString());
                    if (ctrl1 != null)
                    {
                        if (ctrl1.GetType().ToString().ToUpper().Contains("HTMLTEXTAREA") == true)
                        {
                            HtmlTextArea txt = (HtmlTextArea)ctrl1;
                            txt.Value = string.Empty;
                        }
                        else if (ctrl1.GetType().ToString().ToUpper().Contains("HTMLINPUTTEXT") == true)
                        {
                            HtmlInputText txt = (HtmlInputText)ctrl1;
                            txt.Value = string.Empty;
                        }
                    }
                }
            }

            if (pnlVitals.Controls[0] != null)
            {
                for (int i = 0; i < pnlVitals.Controls[0].Controls.Count; i++)
                {
                    foreach (Control ctrls in pnlVitals.Controls[0].Controls[i].Controls)
                    {
                        foreach (Control ctrl in ctrls.Controls)
                        {
                            if (ctrl.GetType().ToString().ToUpper().Contains("DATEPICKER") == true && ctrl.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                            {
                                HtmlInputText dtp = (HtmlInputText)ctrl;
                                if (htDisplayText.Contains(dtp.ID))
                                {
                                    if (htDisplayText[dtp.ID].ToString().ToUpper().Trim() == "NOW")
                                    {
                                        if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
                                        {
                                            DateTime temp;
                                            if (openingfrom != "Menu")
                                                temp = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                                            else
                                                temp = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                                            dtp.Value = temp.ToString();
                                        }
                                    }
                                    else if (htDisplayText[dtp.ID].ToString().ToUpper().Trim() == "DOS")
                                    {
                                        if (Convert.ToDateTime(dtp.Value) < VitalTakenDateValue)
                                        {
                                            DateTime temp;
                                            if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
                                            {
                                                if (openingfrom != "Menu")
                                                {
                                                    temp = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                                                }
                                                else
                                                {
                                                    temp = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                                                }
                                            }

                                            // dtp.SelectedDate = dtp.MaxDate;
                                            //if (VitalTakenDateValue > dtp.SelectedDate)
                                            //dtp.SelectedDate = dtp.MaxDate;
                                            // else
                                            dtp.Value = VitalTakenDateValue.ToString();
                                        }
                                        else
                                        {
                                            dtp.Value = VitalTakenDateValue.ToString();
                                        }
                                    }
                                    Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_" + dtp.ID.Replace(" ", ""));
                                    if (ctrDtpTime != null)
                                    {
                                        MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                                        if (dtpInAddTime != null)
                                        {
                                            DateTime CapturedTime = new DateTime();
                                            if (htDisplayText[dtp.ID].ToString().Trim() == string.Empty)
                                            {
                                                break;
                                            }
                                            else if (htDisplayText[dtp.ID].ToString().ToUpper().Trim() == "NOW")
                                            {
                                                if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value != string.Empty)
                                                {
                                                    DateTime temp;
                                                    if (openingfrom != "Menu")
                                                        temp = UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null));
                                                    else
                                                        temp = DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null);
                                                    CapturedTime = temp;
                                                }
                                            }
                                            else if (htDisplayText[dtp.ID].ToString().ToUpper().Trim() == "DOS")
                                            {
                                                CapturedTime = VitalTakenDateValue;
                                            }
                                            dtpInAddTime.Hour = CapturedTime.Hour;
                                            dtpInAddTime.Hour = CapturedTime.Hour == 0 ? getHour(CapturedTime.ToString()) : CapturedTime.Hour;
                                            dtpInAddTime.Minute = CapturedTime.Minute;
                                            dtpInAddTime.AmPm = CapturedTime.ToString("tt") == "AM" ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM;

                                        }
                                    }
                                }
                            }
                            else if (ctrl.GetType().ToString().ToUpper().Contains("IMAGEBUTTON") == true)
                            {
                                ImageButton imgBtn = (ImageButton)ctrl;
                                string AssociatedIDOne = imgBtn.ID.Trim().Replace("-", "") + "SysDia";
                                string AssociatedIDTwo = imgBtn.ID.Trim().Replace("-", "") + "Diastolic";
                                string DefaultValueScriptInjection = string.Empty;
                                DefaultValueScriptInjection = "function SetDefaultValue" + AssociatedIDOne + "( controlOne,controlTwo,Syst,Dias){  var controlOneText=document.getElementById(controlOne).value;var controlTwoText=document.getElementById(controlTwo).value;if(controlOneText=='' && controlTwoText=='' ){document.getElementById(controlOne).value=Syst;document.getElementById(controlTwo).value=Dias;EnableSave(true);}else {if(document.getElementById(controlOne).value !=Syst || document.getElementById(controlTwo).value !=Dias){if(confirm('You have entered some values. Do you want to replace it with default values ?')){document.getElementById(controlOne).value=Syst;document.getElementById(controlTwo).value=Dias;EnableSave(true);}}}}";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetDefaultValue" + AssociatedIDOne, DefaultValueScriptInjection, true);
                                if (ClientSession.UserRole.Trim().ToUpper() == "MEDICAL ASSISTANT" && ClientSession.UserPermission.Trim().ToUpper() == "U" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN" && ClientSession.UserPermission.Trim().ToUpper() == "U" || ClientSession.UserRole.Trim().ToUpper() == "PHYSICIAN ASSISTANT" && ClientSession.UserPermission.Trim().ToUpper() == "U" || ClientSession.UserRole.Trim().ToUpper() == "TECHNICIAN" && ClientSession.UserPermission.Trim().ToUpper() == "U")
                                {
                                    imgBtn.Attributes.Add("onclick", "SetDefaultValue" + AssociatedIDOne + "('" + AssociatedIDOne + "','" + AssociatedIDTwo + "','" + Session["BPUpperLimit"].ToString() + "','" + Session["BPLowerLimit"].ToString() + "');return false;");
                                }
                            }

                        }
                    }
                }
            }

            openingfrom = Request["openingfrom"].ToString();

            if (openingfrom == "Menu")
            {
                GridDiv.Visible = true;
                BtnClose1.Style.Add("display", "block");
                btnSaveVitals.Disabled = true;
                btnSaveVitals.Value = "Add";
                //  btnSave.AccessKey = "a";
                //  System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanSave");
                //  text1.InnerText = "A";
                // System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
                // text2.InnerText = "dd";
                inpbtnClearall.Value = "Clear All";
                // System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearall.FindControl("SpanClear");
                // text3.InnerText = "C";
                // System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearall.FindControl("SpanClearAdditional");
                // text4.InnerText = "lear All";
                if (Session["objVitalDTO"] != null)
                {
                    objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
                    PastVitals(objVitalDTO.VitalsList);
                }
            }
            else
            {
                btnSaveVitals.Value = "Save";
                //  btnSave.AccessKey = "s";
                //  System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanSave");
                // text1.InnerText = "S";
                // System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
                //  text2.InnerText = "ave";
            }
            //CAP-1429
            EnableDisbaleSave(false);
        }
        public void SetFocus()
        {
            //Sets the focus to the first textbox after clear all
            for (int i = 0; i < pnlVitals.Controls.Count; i++)
            {
                if (pnlVitals.Controls[i].GetType().ToString().Contains("Label") == false &&
                   pnlVitals.Controls[i].GetType().ToString().Contains("Button") == false)
                {
                    Control ctrl = pnlVitals.Controls[i];
                    ctrl.Focus();
                    break;
                }
            }
        }




        protected void btnClearall_Click(object sender, EventArgs e)
        {


        }


        protected void btnClear_Click(object sender, EventArgs e)
        {

            ClearText();


            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                divLoading.Style.Add("display", "none");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);           // if (Request["openingfrom"].ToString().ToUpper() == "MENU")
            //  divLoading.Style.Add("display", "none");
            //  else
            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UnloadWaitCursor", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "clear", "clearLatestLabResult();", true);

        }
        public IList<PatientResults> GridCellClick(int rowindex)
        {
            ulong DelUpdteGroupID = 0;
            PatientResultsDTO objVitalDTO = null;
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            DateTime VitalTakenDate = DateTime.MinValue;
            if (Session["VitalTakenDate"] != null)
                VitalTakenDate = Convert.ToDateTime(Session["VitalTakenDate"]);
            IList<PatientResults> updateVitalList = new List<PatientResults>();
            if (dtTable.Rows.Count > 0)
            {

                DelUpdteGroupID = Convert.ToUInt64(dtTable.Rows[rowindex]["GroupID"]);
                Session["DelUpdteGroupID"] = DelUpdteGroupID;
                VitalTakenDate = UtilityManager.ConvertToUniversal(Convert.ToDateTime(dtTable.Rows[rowindex]["Entered Date Time"]));
                updateVitalList = (from vital in objVitalDTO.VitalsList where vital.Vitals_Group_ID == DelUpdteGroupID select vital).ToList<PatientResults>();

            }
            return updateVitalList;
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {

            int rowindex = Convert.ToInt32(hdnRowIndex.Value);
            if (rowindex >= 0)
            {
                if (Session["PastVitalGrid"] != null)
                    dtTable = (DataTable)Session["PastVitalGrid"];
                IList<PatientResults> updateVitalList = GridCellClick(rowindex);

                if (updateVitalList.Count > 0)
                {
                    if (updateVitalList[0].Encounter_ID != 0)
                    {
                    }
                }

                //Delete Operation
                //if (e.CommandName == "Del")
                //{
                Session["updateVitalList"] = updateVitalList;
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "CellSelected();", true);
                //On the click of edit image,values in the corressponding grid row is displayed in the text boxes

                //  }
            }
        }


        public void imgbtnEdit_Click(object sender, EventArgs e)
        {
            PatientResultsDTO objVitalDTO = new PatientResultsDTO();
            int rowindex;
            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            if (hdnEdit.Value != "")
            {
                rowindex = Convert.ToInt32(hdnEdit.Value);
                if (rowindex >= 0)
                {
                    if (Session["PastVitalGrid"] != null)
                        dtTable = (DataTable)(Session["PastVitalGrid"]);
                    IList<PatientResults> updateVitalList = GridCellClick(rowindex);
                    EditCellClick(updateVitalList);
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }

        public void imgbtnDelete_Click(object sender, EventArgs e)
        {

            int rowindex;

            if (hdnEdit.Value != "")
            {
                rowindex = Convert.ToInt32(hdnEdit.Value);
                if (rowindex >= 0)
                {
                    if (Session["PastVitalGrid"] != null)
                        dtTable = (DataTable)(Session["PastVitalGrid"]);
                    IList<PatientResults> updateVitalList = GridCellClick(rowindex);
                    Session["updateVitalList"] = updateVitalList;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Delete", "CellSelected();", true);
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);

        }
        //protected void grdPastVitals_ItemCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    PatientResultsDTO objVitalDTO = null;

        //    if (Session["objVitalDTO"] != null)
        //    {
        //        objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
        //    }

        //    if (Convert.ToString(e.CommandArgument) != string.Empty && hdnEdit.Value != "")
        //    {

        //        int rowindex = Convert.ToInt32(e.CommandArgument);

        //        if (rowindex >= 0)
        //        {
        //            if (Session["PastVitalGrid"] != null)
        //                dtTable = (DataTable)(Session["PastVitalGrid"]);
        //            IList<PatientResults> updateVitalList = GridCellClick(rowindex);

        //            if (updateVitalList.Count > 0)
        //            {
        //                if (updateVitalList[0].Encounter_ID != 0)
        //                {
        //                }
        //            }

        //            //Delete Operation
        //            if (e.CommandName == "Del")
        //            {
        //                Session["updateVitalList"] = updateVitalList;

        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Delete", "CellSelected();", true);


        //                //On the click of edit image,values in the corressponding grid row is displayed in the text boxes

        //            }
        //            if (e.CommandName == "EditRow")
        //            {
        //                EditCellClick();
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "unload", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        //            }

        //            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
        //                divLoading.Style.Add("display", "none");
        //            else
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
        //        }
        //    }
        //}

        public void EditCellClick(IList<PatientResults> lstvitals)
        {
            ClearText();
            Hashtable htDisplayText = new Hashtable();
            Hashtable htUnitConvMthds = new Hashtable();
            Hashtable htRetriveUnitMthds = new Hashtable();

            if (Session["htDisplayText"] != null)
            {
                htDisplayText = (Hashtable)Session["htDisplayText"];
            }
            if (Session["htRetriveUnitMthds"] != null)
            {
                htRetriveUnitMthds = (Hashtable)Session["htRetriveUnitMthds"];
            }
            if (Session["htUnitConvMthds"] != null)
            {
                htUnitConvMthds = (Hashtable)Session["htUnitConvMthds"];
            }
            IList<PatientResults> vitalList = lstvitals;
            //if (Session["vitalList"] != null)
            //{
            //    vitalList = (IList<PatientResults>)Session["vitalList"];
            //}
            //PastVitals(vitalList);
            ulong DelUpdteGroupID = 0;
            if (Session["DelUpdteGroupID"] != null)
                DelUpdteGroupID = Convert.ToUInt64(Session["DelUpdteGroupID"]);
            var vit = (from li in vitalList
                       where li.Encounter_ID == 0 && li.Vitals_Group_ID == DelUpdteGroupID
                       select li);
            if (vit != null && vit.Count() != 0)
            {
                foreach (var v in vit)
                {
                    string Loinc_observation = v.Loinc_Observation;
                    if (v.Loinc_Observation.ToUpper().Contains("BP") || v.Loinc_Observation.ToUpper().Contains("BLOOD") || v.Loinc_Observation.ToUpper().Contains("BODY"))
                    {
                        v.Loinc_Observation = Regex.Replace(v.Loinc_Observation, @"[\-\/\s]+", string.Empty);
                    }

                    Control ctrDtp = pnlVitals.FindControl("dtp" + v.Loinc_Observation.Replace(" ", string.Empty));

                    if (ctrDtp != null)
                    {
                        HtmlInputText dtpInAdd = (HtmlInputText)ctrDtp;
                        if (dtpInAdd != null)
                        {
                            dtpInAdd.Value = UtilityManager.ConvertToLocal(v.Captured_date_and_time).ToString("dd-MMM-yyyy hh:mm tt ");
                            arydatetimeFilled.Add(dtpInAdd.ID.ToString());

                            Control ctrDtpTime = pnlVitals.FindControl("dtpTakenTime_dtp" + v.Loinc_Observation.Replace(" ", ""));
                            if (ctrDtpTime != null)
                            {
                                MKB.TimePicker.TimeSelector dtpInAddTime = (MKB.TimePicker.TimeSelector)ctrDtpTime;
                                if (dtpInAddTime != null)
                                {
                                    DateTime CapturedTime = new DateTime();
                                    CapturedTime = UtilityManager.ConvertToLocal(v.Captured_date_and_time);
                                    dtpInAddTime.SelectedTimeFormat = MKB.TimePicker.TimeSelector.TimeFormat.Twelve;
                                    dtpInAddTime.Hour = CapturedTime.Hour == 0 ? getHour(CapturedTime.ToString()) : CapturedTime.Hour;
                                    dtpInAddTime.Minute = CapturedTime.Minute;
                                    dtpInAddTime.AmPm = CapturedTime.ToString("tt") == "AM" ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                                }
                            }
                        }
                    }
                    Control ctrDtpnew = pnlVitals.FindControl(v.Loinc_Observation.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                    if (ctrDtpnew != null && ctrDtpnew.ID != string.Empty)
                    {
                        if (ctrDtpnew.ID.ToString().ToUpper().Contains("DATEPICKER") == true && ctrDtpnew.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        // if (ct.ID.ToString().ToUpper().Contains("DATEPICKER") == true && ct.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        {
                            HtmlInputText dtp = (HtmlInputText)ctrDtpnew;
                            if (v.Value != "" && v.Value.Replace(" ", "").Length == 11)
                            {
                                //DateTime dt = DateTime.ParseExact(v.Value, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                dtp.Value = Convert.ToDateTime(v.Value).ToString("dd-MMM-yyyy");
                            }
                            else if (v.Value != "" && v.Value.Replace(" ", "").Length == 4)
                            {
                                dtp.Value = Convert.ToDateTime("01-Jan-" + v.Value).ToString("dd-MMM-yyyy");
                            }
                            else if (v.Value != "" && v.Value.Replace(" ", "").Length == 8)
                            {
                                dtp.Value = Convert.ToDateTime("01-" + v.Value).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                dtp.Value = "";
                            }

                        }
                    }
                    if (v.Loinc_Observation.ToUpper().Contains("BLOOD") || v.Loinc_Observation.ToUpper().Contains("BODY"))
                        v.Loinc_Observation = Loinc_observation;

                    Control ctBP = pnlVitals.FindControl(v.Loinc_Observation);
                    if (ctBP == null)
                    {
                        ctBP = pnlVitals.FindControl(v.Loinc_Observation.Replace(" ", "").Replace("/", "") + "DATEPICKER");
                    }
                    int indexOfControl = pnlVitals.Controls.IndexOf(ctBP);
                    if (ctBP != null && ctBP.ID != string.Empty)
                    {
                        if (v.Loinc_Observation.StartsWith("Vision-Left"))
                        {
                            HtmlInputText ctnum = (HtmlInputText)ctBP;
                            string[] visionVal = v.Value.Split('/');
                            if (visionVal.Length > 0)
                            {
                                ctnum.Value = visionVal[0];
                            }
                            Control ctrl1 = pnlVitals.FindControl("Vision-Lefts");
                            // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                            HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                            if (visionVal.Length > 1)
                                ctnum1.Value = visionVal[1];
                            else
                                ctnum1.Value = "";
                        }
                        if (v.Loinc_Observation.StartsWith("Vision-Right"))
                        {
                            HtmlInputText ctnum = (HtmlInputText)ctBP;
                            string[] visionVal = v.Value.Split('/');
                            if (visionVal.Length > 0)
                            {
                                ctnum.Value = visionVal[0];
                            }
                            Control ctrl1 = pnlVitals.FindControl("Vision-Rights");
                            // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                            HtmlInputText ctnum1 = (HtmlInputText)ctrl1;

                            if (visionVal.Length > 1)
                                ctnum1.Value = visionVal[1];
                            else
                                ctnum1.Value = "";
                        }
                        if (v.Loinc_Observation.StartsWith("Vision-Both"))
                        {
                            HtmlInputText ctnum = (HtmlInputText)ctBP;
                            string[] visionVal = v.Value.Split('/');
                            if (visionVal.Length > 0)
                            {
                                ctnum.Value = visionVal[0];
                            }
                            Control ctrl1 = pnlVitals.FindControl("Vision-Boths");
                            // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                            HtmlInputText ctnum1 = (HtmlInputText)ctrl1;


                            if (visionVal.Length > 1)
                                ctnum1.Value = visionVal[1];
                            else
                                ctnum1.Value = "";
                        }
                        if (v.Value.Contains("/") && v.Loinc_Observation.StartsWith("BP"))
                        {

                            // RadNumericTextBox ctnum = (RadNumericTextBox)ctBP;
                            HtmlInputText ctnum = (HtmlInputText)ctBP;

                            string[] bpVal = v.Value.Split('/');
                            if (bpVal.Length > 0)
                            {
                                ctnum.Value = bpVal[0];
                                if (ctnum.Value != string.Empty)
                                {
                                    arynumericFilled.Add(ctnum.ID.ToString());
                                }

                                if (v.Loinc_Observation.Contains("Sitting") && !v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPSittingDiastolic");
                                    //  RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;

                                    ctnum1.Value = bpVal[1];
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }

                                if (v.Loinc_Observation.Contains("Sitting") && v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPSittingSecondDiastolic");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;

                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                    ctnum1.Value = bpVal[1];
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }

                                if (v.Loinc_Observation.Contains("Standing") && !v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPStandingDiastolic");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                    ctnum1.Value = bpVal[1];
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }

                                if (v.Loinc_Observation.Contains("Standing") && v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPStandingSecondDiastolic");
                                    //  RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                    ctnum1.Value = bpVal[1];
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());

                                    }
                                }

                                if (v.Loinc_Observation.Contains("Lying") && !v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPLyingDiastolic");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                    ctnum1.Value = bpVal[1];
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }

                                if (v.Loinc_Observation.Contains("Lying") && v.Loinc_Observation.Contains("Second"))
                                {
                                    Control ctrl1 = pnlVitals.FindControl("BPLyingSecondDiastolic");
                                    // RadNumericTextBox ctnum1 = (RadNumericTextBox)ctrl1;
                                    HtmlInputText ctnum1 = (HtmlInputText)ctrl1;
                                    ctnum1.Value = bpVal[1];
                                    if (ctnum1.Value != string.Empty)
                                    {
                                        arynumericFilled.Add(ctnum1.ID.ToString());
                                    }
                                }

                                string[] split = Loinc_observation.Split(' ');

                                if (split != null && split.Length > 0)
                                {
                                    try
                                    {
                                        PatientResults locVital = (from li in vitalList
                                                                   where li.Loinc_Observation == split[0] + " Location" && li.Encounter_ID == ClientSession.EncounterId && li.Vitals_Group_ID == DelUpdteGroupID
                                                                   select li).ToList<PatientResults>()[0];
                                        if (locVital != null)
                                        {
                                            if (locVital.Value.ToUpper().Contains("LEFT"))
                                            {
                                                //CheckBox chk = (CheckBox)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                //chk.Checked = true;
                                                //CheckBox chk1 = (CheckBox)pnlVitals.FindControl(split[0] + " " + "Right");
                                                //chk1.Checked = false;

                                                RadioButton chk = (RadioButton)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                chk.Checked = true;
                                                RadioButton chk1 = (RadioButton)pnlVitals.FindControl(split[0] + " " + "Right");
                                                chk1.Checked = false;


                                            }
                                            else if (locVital.Value.ToUpper().Contains("RIGHT"))
                                            {
                                                //CheckBox chk = (CheckBox)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                //chk.Checked = true;
                                                //CheckBox chk1 = (CheckBox)pnlVitals.FindControl(split[0] + " " + "Left");
                                                //chk1.Checked = false;
                                                RadioButton chk = (RadioButton)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                                chk.Checked = true;
                                                RadioButton chk1 = (RadioButton)pnlVitals.FindControl(split[0] + " " + "Left");
                                                chk1.Checked = false;

                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }

                            }

                        }
                    }

                    string Notes = v.Loinc_Observation;
                    if (v.Loinc_Observation.ToUpper().Contains("LOCATION") == true)
                    {
                        string[] split = Loinc_observation.Split(' ');

                        if (split != null && split.Length > 0)
                        {
                            try
                            {
                                PatientResults locVital = (from li in vitalList
                                                           where li.Loinc_Observation == split[0].Replace("-", "") + "Location" && li.Vitals_Group_ID == DelUpdteGroupID
                                                           select li).ToList<PatientResults>()[0];
                                if (locVital != null)
                                {
                                    if (locVital.Value.ToUpper().Contains("LEFT"))
                                    {
                                        //CheckBox chk = (CheckBox)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                        RadioButton chk = (RadioButton)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                        chk.Checked = true;
                                        //CheckBox chk1 = (CheckBox)pnlVitals.FindControl(split[0] + " " + "Right");
                                        RadioButton chk1 = (RadioButton)pnlVitals.FindControl(split[0] + " " + "Right");
                                        chk1.Checked = false;
                                    }
                                    else if (locVital.Value.ToUpper().Contains("RIGHT"))
                                    {
                                        //CheckBox chk = (CheckBox)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                        RadioButton chk = (RadioButton)pnlVitals.FindControl(split[0] + " " + locVital.Value);
                                        chk.Checked = true;
                                        //CheckBox chk1 = (CheckBox)pnlVitals.FindControl(split[0] + " " + "Right");
                                        RadioButton chk1 = (RadioButton)pnlVitals.FindControl(split[0] + " " + "Left");
                                        chk1.Checked = false;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }


                    if (v.Loinc_Observation.Contains(" ") || v.Loinc_Observation.Contains("-"))
                    {
                        Notes = v.Loinc_Observation.Replace(" ", "");
                        Notes = Notes.Replace("-", "");
                    }
                    Control ctl = pnlVitals.FindControl("txtNotes" + Notes.Replace("CDP", "").Replace("CDP", "").Replace("-", ""));

                    if (ctl != null && ctl.ID != string.Empty)
                    {
                        CustomDLCNew comb = (CustomDLCNew)ctl;
                        comb.txtDLC.Text = v.Notes;
                        if (comb.txtDLC.Text != string.Empty)
                        {
                            aryNotesFilled.Add(comb.ID.ToString());
                        }
                    }
                    //Control reason = pnlVitals.FindControl("Patient-Reason-refused-" + Notes.Replace("-", ""));
                    //if (reason != null && reason.ID != string.Empty)
                    //{
                    //    HtmlSelect comb = (HtmlSelect)reason;
                    //    comb.Value = v.Reason_For_Not_Performed;
                    //    if (comb.Value != string.Empty)
                    //    {
                    //        aryNotesFilled.Add(comb.ID.ToString());
                    //    }
                    //}
                    v.Loinc_Observation = Loinc_observation;

                    Control ct = pnlVitals.FindControl(v.Loinc_Observation);

                    if (ct != null && ct.ID != string.Empty)
                    {
                        if (ct.ID.ToString().ToUpper().Contains("dtp") == true && ct.GetType().ToString().ToUpper().Contains("CUSTOM") == false)
                        {
                            HtmlInputText dtp = (HtmlInputText)ct;
                            string dd = Request.Form[dtp.UniqueID];
                            DateTime dt = DateTime.ParseExact(v.Value, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            dtp.Value = dt.ToString();
                        }
                        else if (ct.GetType().ToString().ToUpper().Contains("CHECK") == true)
                        {
                            Control ctrChk = pnlVitals.FindControl(v.Loinc_Observation);
                            if (ctrChk != null)
                            {
                                CheckBox chkYes = (CheckBox)ctrChk;
                                if (v.Value == "Y")
                                {
                                    chkYes.Checked = true;
                                }
                            }

                            Control ctrChkNo = pnlVitals.FindControl("chk" + v.Loinc_Observation);
                            if (ctrChkNo != null)
                            {
                                CheckBox chkNo = (CheckBox)ctrChkNo;
                                if (v.Value == "N")
                                {
                                    chkNo.Checked = true;
                                }
                            }

                        }
                        //else if (ct.GetType().ToString().ToUpper().Contains("RADMASKEDTEXTBOX") == true)
                        //{
                        //    RadMaskedTextBox dtpDate = ((RadMaskedTextBox)pnlVitals.FindControl(ct.ID));
                        //    if (dtpDate != null)
                        //    {
                        //        if (v.Value != string.Empty)
                        //        {
                        //            string[] aryFromDate = v.Value.Split('-');
                        //            if (aryFromDate.Length == 3)
                        //            {
                        //                if (aryFromDate[0].Trim() != string.Empty && aryFromDate[1].Trim() != string.Empty && aryFromDate[2].Trim() != string.Empty)
                        //                {
                        //                    if (aryFromDate[0].Trim().Length == 1)
                        //                        dtpDate.Text = aryFromDate[2] + "-" + aryFromDate[1] + "-" + "0" + aryFromDate[0].Trim();
                        //                    else
                        //                        dtpDate.Text = aryFromDate[2] + "-" + aryFromDate[1] + "-" + aryFromDate[0];

                        //                }
                        //            }
                        //            else if (aryFromDate.Length == 2)
                        //                dtpDate.Text = aryFromDate[1] + "-" + aryFromDate[0];
                        //            else
                        //                dtpDate.Text = v.Value;
                        //        }
                        //    }
                        //}
                        else
                        {
                            if (htRetriveUnitMthds.Contains(v.Loinc_Observation))
                            {
                                string sValue = string.Empty;
                                if (v.Value.Contains("'") == true && v.Value.Contains("''") == true)
                                    sValue = v.Value;
                                else
                                    sValue = ConversionOnRetrieval(v.Loinc_Observation, v.Value);
                                sValue = ConversionOnRetrieval(v.Loinc_Observation, v.Value);
                                if (sValue.Contains("'") == true && sValue.Contains("''") == true)
                                {
                                    string[] Splitter = { "'", "''" };
                                    string[] feetInch = sValue.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
                                    if (feetInch.Length > 0)
                                    {
                                        HtmlInputText ctrltxt = (HtmlInputText)ct;
                                        ctrltxt.Value = feetInch[0].Trim();
                                        if (ctrltxt.Value != string.Empty)
                                        {
                                            aryNotesFilled.Add(ctrltxt.ID.ToString());

                                        }
                                        Control ctrl1 = pnlVitals.FindControl("HeightInch");
                                        HtmlInputText ctrltxt1 = (HtmlInputText)ctrl1;
                                        ctrltxt1.Value = feetInch[1].Trim();
                                        if (ctrltxt1.Value != string.Empty)
                                        {
                                            aryNotesFilled.Add(ctrltxt1.ID.ToString());

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ct.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT") && ct.ID.ToString().ToUpper().IndexOf("VISION") <= -1)
                                {
                                    HtmlInputText ctrllbl = (HtmlInputText)ct;
                                    ctrllbl.Value = v.Value;
                                    if (ctrllbl.Value != string.Empty)
                                    {
                                        aryLabelFilled.Add(ctrllbl.ID.ToString());
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT") && ct.ID.ToString().ToUpper().IndexOf("VISION") <= -1)
                                {
                                    HtmlInputText ctrltxt = (HtmlInputText)ct;
                                    ctrltxt.Value = v.Value;
                                    if (ctrltxt.Value != string.Empty)
                                    {
                                        aryNotesFilled.Add(ctrltxt.ID.ToString());
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                {
                                    HtmlTextArea ctrltxt = (HtmlTextArea)ct;
                                    ctrltxt.Value = v.Value;
                                    if (ctrltxt.Value.Length > 15)
                                        ctrltxt.Style.Add("Height", "29px");
                                    else
                                        ctrltxt.Style.Add("Height", "18px");
                                    if (ctrltxt.Value != string.Empty)
                                    {
                                        aryLabelFilled.Add(ctrltxt.ID.ToString());
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLSELECT"))
                                {
                                    HtmlSelect ctrlcmb = (HtmlSelect)ct;
                                    for (int i = 0; i < ctrlcmb.Items.Count; i++)
                                    {
                                        if (ctrlcmb.Items[i].Text == v.Value)
                                            ctrlcmb.SelectedIndex = i;
                                    }

                                }
                            }

                            if (htUnitConvMthds.Contains(v.Loinc_Observation) && v.Loinc_Observation != "BMI" && v.Loinc_Observation != "BP-Sitting Sys/Dia")
                            {
                                if (ct.GetType().Name.ToUpper().Contains("HTMLGENERICCONTROL"))
                                {
                                    HtmlGenericControl ctrllbl = (HtmlGenericControl)ct;
                                    ctrllbl.Style.Add("color", Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctrllbl.InnerHtml)));
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLINPUTTEXT"))
                                {
                                    if (v.Loinc_Observation.ToUpper() != "HEIGHT" && !(v.Loinc_Observation.ToUpper().IndexOf("VISION") > -1))
                                    {
                                        HtmlInputText ctrltxt = (HtmlInputText)ct;
                                        ctrltxt.Style.Add("color", Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctrltxt.Value)));
                                    }
                                }
                                if (ct.GetType().Name.ToUpper().Contains("HTMLTEXTAREA"))
                                {
                                    HtmlTextArea ctrltxt = (HtmlTextArea)ct;
                                    ctrltxt.Style.Add("color", Convert.ToString(SetColorForStatus(v.Loinc_Observation, ctrltxt.Value)));
                                }

                            }
                            else if (v.Loinc_Observation.Contains("BMI Status"))
                            {
                                HtmlInputText lblstatus = (HtmlInputText)ct;
                                string color = Convert.ToString(SetColorForStatus(v.Loinc_Observation, lblstatus.Value));
                                lblstatus.Style.Add("color", color);
                            }
                        }
                    }
                }
            }

            btnSaveVitals.Value = "Update";
            //  btnSave.AccessKey = "u";
            inpbtnClearall.Value = "Cancel";
            //  System.Web.UI.HtmlControls.HtmlGenericControl text1 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanSave");
            //text1.InnerText = "U";
            // System.Web.UI.HtmlControls.HtmlGenericControl text2 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnSave.FindControl("SpanAdditionalword");
            // text2.InnerText = "pdate";
            // System.Web.UI.HtmlControls.HtmlGenericControl text3 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearall.FindControl("SpanClear");
            //text3.InnerText = "C";
            //System.Web.UI.HtmlControls.HtmlGenericControl text4 = (System.Web.UI.HtmlControls.HtmlGenericControl)btnClearall.FindControl("SpanClearAdditional");
            //text4.InnerText = "ancel";

            EnableDisbaleSave(true);
            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                divLoading.Style.Add("display", "none");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);


            ScriptManager.RegisterStartupScript(this, this.GetType(), "unloadwait", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void InvisibleButton_Click(object sender, EventArgs e)
        {
            DateTime BirthDate = DateTime.MinValue;
            string humanSex = string.Empty;
            double humanAgeInMonths = 0;
            ulong ScreenID = 2000;
            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                BirthDate = ClientSession.PatientPaneList[0].Birth_Date;
                humanSex = ClientSession.PatientPaneList[0].Sex;
            }
            if (hdnSystemTime.Value.Trim() != string.Empty)
                humanAgeInMonths = ((UtilityManager.ConvertToLocal(DateTime.ParseExact(hdnSystemTime.Value, "M'/'d'/'yyyy H':'m':'s", null)).Date - BirthDate.Date).TotalDays) / 30.4375;
            else if (Request.Cookies["VitalCurrentDate"] != null && Request.Cookies["VitalCurrentDate"].Value.ToString().Trim() != string.Empty)
            {
                if (Request["openingfrom"] != "Menu")
                    humanAgeInMonths = ((UtilityManager.ConvertToLocal(DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null)).Date - BirthDate.Date).TotalDays) / 30.4375;
                else
                    humanAgeInMonths = ((DateTime.ParseExact(Request.Cookies["VitalCurrentDate"].Value.ToString(), "M'/'d'/'yyyy H':'m':'s", null).Date - BirthDate.Date).TotalDays) / 30.4375;
            }
            else
                humanAgeInMonths = ((DateTime.Now.Date - BirthDate.Date).TotalDays) / 30.4375;
            PatientResultsDTO objVitalDTO = null;
            IList<PatientResults> updateVitalList = new List<PatientResults>();
            if (Session["updateVitalList"] != null)
                updateVitalList = (IList<PatientResults>)Session["updateVitalList"];

            if (Session["objVitalDTO"] != null)
            {
                objVitalDTO = (PatientResultsDTO)Session["objVitalDTO"];
            }
            foreach (PatientResults obj in updateVitalList)
            {
                obj.Modified_By = ClientSession.UserName;
                //obj.Modified_Date_And_Time = utc;
                obj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
            }
            if (updateVitalList != null && updateVitalList.Count > 0)
                objVitalDTO = vitalmngr.DeleteVitalDetails(updateVitalList.ToArray<PatientResults>(), updateVitalList[0].Human_ID, 1, 25, string.Empty, ClientSession.PhysicianId, Convert.ToInt16(humanAgeInMonths.ToString().Split('.')[0]), humanSex, "'BMI-AGE','HC-AGE'", ScreenID);
            Session["objVitalDTO"] = objVitalDTO;
            Session["vitalList"] = objVitalDTO.VitalsList;
            PastVitals(objVitalDTO.VitalsList);
            ClearText();
            if (Request["openingfrom"].ToString().ToUpper() == "MENU")
                divLoading.Style.Add("display", "none");
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "hideLoading();", true);
        }

        protected void btnLoadDefaultValues_Click(object sender, EventArgs e)
        {
            int j = 0;
            Control[] ctr = new Control[5];
            string PicID = string.Empty;
            if (Session["PicID"] != null)
                PicID = (string)Session["PicID"];

            if (PicID != null && PicID != string.Empty)
            {
                for (int i = 0; i < arynumericcreationFilled.Count; i++)
                {
                    if (arynumericcreationFilled[i].ToString().Contains(PicID.Replace("-", "")) && (PicID.ToUpper().Contains("SECOND")))
                    {
                        ctr[j] = pnlVitals.FindControl(arynumericcreationFilled[i].ToString());
                        j++;
                    }
                    else if (arynumericcreationFilled[i].ToString().Contains(PicID.Replace("-", "")))
                    {
                        ctr[j] = pnlVitals.FindControl(arynumericcreationFilled[i].ToString());
                        j++;
                    }
                }
            }

            if (ctr.Length > 0)
            {
                IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
                if (Session["StaticLookUpList"] != null)
                {
                    StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
                }
                IList<StaticLookup> defValues = new List<StaticLookup>();
                if (StaticLookUpList != null && StaticLookUpList.Count > 0)
                {
                    defValues = StaticLookUpList.Where(q => q.Field_Name == "BP DEFAULT VALUES").ToList();
                }
                //   RadNumericTextBox ctr1 = (RadNumericTextBox)ctr[0];
                // RadNumericTextBox ctr2 = (RadNumericTextBox)ctr[1];

                HtmlInputText ctr1 = (HtmlInputText)ctr[0];

                HtmlInputText ctr2 = (HtmlInputText)ctr[1];
                if ((ctr1 != null) || (ctr2 != null))
                {
                    if (defValues != null && defValues.Count > 0)
                    {
                        ctr1.Value = defValues[0].Value.ToString();
                        ctr2.Value = defValues[1].Value.ToString();
                        ctr1.Style.Add("color", "Black");

                        ctr1.Focus();
                    }
                }
            }
        }
        public string StaticLookupValueForClientSide(string FieldName)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0 && StaticLookUpList.Any(item => item.Field_Name == FieldName))
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == FieldName).ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName(FieldName);
            }

            StringBuilder ClientLookup = new StringBuilder();
            if (defValues.Count > 0)
            {
                ClientLookup.Append("[");
                foreach (StaticLookup obj in defValues)
                {
                    if (obj.Value.Contains("-"))
                        ClientLookup.Append("{'Field_Name':'" + obj.Field_Name + "','LowerLimit':'" + obj.Value.Split('-')[0] + "','UpperLimit':'" + obj.Value.Split('-')[1] + "','Description':'" + obj.Description + "'},");
                    else
                        ClientLookup.Append("{'Field_Name':'" + obj.Field_Name + "','LowerLimit':'" + obj.Value + "','UpperLimit':'" + null + "','Description':'" + obj.Description + "'},");
                }
                ClientLookup = ClientLookup.Remove(ClientLookup.Length - 1, 1);
                ClientLookup.Append("]");
            }
            return ClientLookup.ToString();
        }

        void EnableDisbaleSave(bool IsEnable)
        {
            string InjectingScript = string.Empty;
            if (IsEnable == true)
            {
                InjectingScript = "if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined) window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;";

                btnSaveVitals.Disabled = false;
            }
            else
            {
                InjectingScript = "if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined) window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;";
                btnSaveVitals.Disabled = true;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "EnableDisableSave", InjectingScript, true);

        }
        public int getHour(string DOS)
        {
            int hour = 0;
            string Time = DOS.ToString();
            string[] ctlt = Time.Split(' ');
            if (ctlt.Count() > 0)
            {
                string[] ctl1 = ctlt[1].Split(':');
                hour = Convert.ToInt32(ctl1[0]);
                return hour;
            }
            return hour;
        }

        protected void InvisibleButton1_Click(object sender, EventArgs e)
        {
            IList<PatientResults> vitalList = (IList<PatientResults>)Session["vitalList"];
            PastVitals(vitalList);
        }

        public void SetVitalsToolTip(IList<PatientResults> vitalList)//(string values)
        {
            string vitalText = "";
            string values = string.Empty;
            List<string> lstToolTips = new List<string>();
            //var svitalList = new UtilityManager().LoadPatientSummaryList(out lstToolTips);
            IList<int> ilstChangeSummaryBar = new List<int>() { 2 };
            //var svitalList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTips);
            IList<PatientResults> tempVitalList = new List<PatientResults>();
            foreach (PatientResults oldObj in vitalList)
            {
                PatientResults newObj = new PatientResults();
                newObj.Abnormal_Flags = oldObj.Abnormal_Flags;
                newObj.Acurus_Result_Code = oldObj.Acurus_Result_Code;
                newObj.Acurus_Result_Description = oldObj.Acurus_Result_Description;
                newObj.Captured_date_and_time = oldObj.Captured_date_and_time;
                newObj.Created_By = oldObj.Created_By;
                newObj.Created_Date_And_Time = oldObj.Created_Date_And_Time;
                newObj.Encounter_ID = oldObj.Encounter_ID;
                newObj.Human_ID = oldObj.Human_ID;
                newObj.Id = oldObj.Id;
                newObj.Internal_Property_Month = oldObj.Internal_Property_Month;
                newObj.Internal_Property_Year = oldObj.Internal_Property_Year;
                newObj.Is_Sent_to_Rcopia = oldObj.Is_Sent_to_Rcopia;
                newObj.Local_Time = oldObj.Local_Time;
                newObj.Loinc_Identifier = oldObj.Loinc_Identifier;
                newObj.Loinc_Observation = oldObj.Loinc_Observation;
                newObj.Modified_By = oldObj.Modified_By;
                newObj.Modified_Date_And_Time = oldObj.Modified_Date_And_Time;
                newObj.Physician_ID = oldObj.Physician_ID;
                newObj.Notes = oldObj.Notes;
                newObj.Reference_Range = oldObj.Reference_Range;
                newObj.Result_Master_ID = oldObj.Result_Master_ID;
                newObj.Result_OBX_ID = oldObj.Result_OBX_ID;
                newObj.Results_Type = oldObj.Results_Type;
                newObj.Units = oldObj.Units;
                newObj.Value = oldObj.Value.Replace("\r\n", " ");
                newObj.Version = oldObj.Version;
                newObj.Vitals_Group_ID = oldObj.Vitals_Group_ID;
                tempVitalList.Add(newObj);
            }
            var svitalList = LoadPatientSummaryUsingList(tempVitalList, out lstToolTips);
            svitalList = svitalList.Where(a => a.ToUpper().StartsWith("VITALS-")).ToList();
            if (svitalList.Count > 0)
                vitalText = svitalList[0].Replace("Vitals-", "");

            //if (string.IsNullOrEmpty(values))
            //    RadScriptManager1.AsyncPostBackErrorMessage = "~true~"
            //        + vitalText + "~"
            //        + lstToolTips[0] + "~"
            //        + lstToolTips[1];

            //else
            //RadScriptManager1.AsyncPostBackErrorMessage = "~true~"
            //    + vitalText + "~"
            //    + lstToolTips[0] + "~"
            //    + lstToolTips[1] + "!"
            //    + values;

            ////ClientSession.bAsyncError = true;
            //string message = "message: " + "~true~"
            //        + vitalText + "~"
            //        + lstToolTips[0] + "~"
            //        + lstToolTips[1] + "!"
            //        + values;
            //RadScriptManager1.RegisterDataItem(btnSave, message);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "summaryMsg",
                 "SetVitalsText('" + vitalText + "','" + (lstToolTips.Count > 0 ? lstToolTips[1] : string.Empty) + "');", true);

        }
        public IList<string> LoadPatientSummaryUsingList(IList<PatientResults> vitalList, out List<string> ilstToolTips)
        {
            IList<string> strSummary = new List<string>();

            ilstToolTips = new List<string>();

            var VitalsText = string.Empty;
            var VitalsToolTip = string.Empty;

            var toolTipText = string.Empty;

            StringBuilder sbToolTip = new StringBuilder();


            VitalsText = new UtilityManager().GetVitalsInfo(vitalList, out toolTipText);
            VitalsToolTip = toolTipText;
            sbToolTip.Append("<br/>");
            sbToolTip.Append(toolTipText);
            strSummary.Add("Vitals-" + VitalsText);


            ilstToolTips.Add(sbToolTip.ToString());
            ilstToolTips.Add(VitalsToolTip);
            return strSummary;
        }
        public string SetHGBStatus(string hbValue)
        {
            IList<StaticLookup> StaticLookUpList = new List<StaticLookup>();
            if (Session["StaticLookUpList"] != null)
            {
                StaticLookUpList = (IList<StaticLookup>)Session["StaticLookUpList"];
            }
            IList<StaticLookup> defValues = new List<StaticLookup>();
            if (StaticLookUpList != null && StaticLookUpList.Count > 0)
            {
                defValues = StaticLookUpList.Where(q => q.Field_Name == "HGB STATUS").ToList();
            }
            else
            {
                defValues = staticLookupManager.getStaticLookupByFieldName("HGB STATUS");
            }
            string HB = string.Empty;
            try
            {
                //BugID:49592
                if (hbValue != string.Empty)
                {
                    decimal hgb = Convert.ToDecimal(hbValue);
                    if (defValues != null && defValues.Count > 0)
                    {
                        for (int i = 0; i < defValues.Count; i++)
                        {
                            string rangeText = defValues[i].Value;
                            string[] sAge = defValues[i].Description.Split('$')[0].Split('|')[0].Split('-');
                            if (human_AgeInMonths >= (Convert.ToInt64(sAge[0]) * 12) && human_AgeInMonths <= (Convert.ToInt64(sAge[1]) * 12))
                            {
                                if (defValues[i].Description.Split('$')[0].Split('|')[1].Length == 1)
                                {
                                    if (defValues[i].Description.Split('$')[0].Split('|')[1] == human_Sex.ToString().Substring(0, 1).ToUpper())
                                    {
                                        string[] split = rangeText.Split('-');
                                        if (split.Length == 2)
                                        {
                                            if (hgb >= Convert.ToDecimal(split[0]) && hgb <= Convert.ToDecimal(split[1]))
                                            {
                                                HB = defValues[i].Description.Split('$')[1].Split('|')[0];
                                                break;
                                            }
                                            else
                                            {
                                                HB = defValues[i].Description.Split('$')[1].Split('|')[1];
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string[] split = rangeText.Split('-');
                                    if (split.Length == 2)
                                    {
                                        if (hgb >= Convert.ToDecimal(split[0]) && hgb <= Convert.ToDecimal(split[1]))
                                        {
                                            HB = defValues[i].Description.Split('$')[1].Split('|')[0];
                                            break;
                                        }
                                        else
                                        {
                                            HB = defValues[i].Description.Split('$')[1].Split('|')[1];
                                            break;
                                        }
                                    }
                                }

                            }

                            //else
                            //{
                            //    if (hgb >= Convert.ToDecimal(split[0]))
                            //    {
                            //        HB = defValues[i].Description;
                            //        break;
                            //    }
                            //}
                        }
                    }

                }
                else
                    HB = string.Empty;

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return HB;
        }
    }
}
