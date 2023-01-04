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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Telerik.Charting;
using Telerik.Web.UI;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Xml;

namespace Acurus.Capella.UI
{
    public partial class frmGrowthChart : System.Web.UI.Page
    {
        VitalsManager vitalMngr = new VitalsManager();
        GrowthChartLookupManager GrowthMngr = new GrowthChartLookupManager();
        PatientResultsDTO vitalDTO = new PatientResultsDTO();
        UIManager objUIManager = new UIManager();
        DataTable dtGrowthChart = new DataTable();
        DataTable dtGrowthChartTemp = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            chrtGrowth.ChartTitle.TextBlock.Text = string.Empty;
            if (!IsPostBack)
            {
                dtGrowthChart.Columns.Add("Captured DateTime", typeof(DateTime));
                grdGrowthChart.DataSource = dtGrowthChart;
                grdGrowthChart.DataBind();
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
            }
            ModalWindow.VisibleOnPageLoad = false;
        }

        protected void chkWeightAgeBirth_CheckedChanged(object sender, EventArgs e)
        {
            InitializeScreen();

            if (chkWeightAgeBirth.Checked == false)
            {
                return;
            }
            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Weight - Age Chart:Boys(Birth to 36 Months)", "Age (Months)", "Weight (lbs)", "Birth to 36 Months", "Age", "Weight");
                }
                else
                {
                    PlotChart("Weight - Age Chart:Girls(Birth to 36 Months)", "Age (Months)", "Weight (lbs)", "Birth to 36 Months", "Age", "Weight");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkWeightLengthBirth_CheckedChanged(object sender, EventArgs e)
        {
            InitializeScreen();

            if (chkWeightLengthBirth.Checked == false)
            {
                return;
            }
            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Weight - Length Chart:Boys(Birth to 36 Months)", "Length (Inches)", "Weight (lbs)", "Birth to 36 Months", "Length", "Weight");
                }
                else
                {
                    PlotChart("Weight - Length Chart:Girls(Birth to 36 Months)", "Length (Inches)", "Weight (lbs)", "Birth to 36 Months", "Length", "Weight");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkHCAgeBirth_CheckedChanged(object sender, EventArgs e)
        {

            InitializeScreen();

            if (chkHCAgeBirth.Checked == false)
            {
                return;
            }

            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Head Circumference - Age Chart:Boys(Birth to 36 Months)", "Age (Months)", "HC (Inches)", "Birth to 36 Months", "Age", "HC");
                }
                else
                {
                    PlotChart("Head Circumference - Age Chart:Girls(Birth to 36 Months)", "Age (Months)", "HC (Inches)", "Birth to 36 Months", "Age", "HC");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }

        protected void chkLengthAgeBirth_CheckedChanged(object sender, EventArgs e)
        {

            InitializeScreen();

            if (chkLengthAgeBirth.Checked == false)
            {
                return;
            }

            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Length - Age Chart:Boys(Birth to 36 Months)", "Age (Months)", "Length (Inches)", "Birth to 36 Months", "Age", "Length");
                }
                else
                {
                    PlotChart("Length - Age Chart:Girls(Birth to 36 Months)", "Age (Months)", "Length (Inches)", "Birth to 36 Months", "Age", "Length");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }

        protected void chkWeightAge_CheckedChanged(object sender, EventArgs e)
        {

            InitializeScreen();

            if (chkWeightAge.Checked == false)
            {
                return;
            }

            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Weight - Age Chart:Boys(2 to 20 Years)", "Age (Years)", "Weight (lbs)", "2 to 20 Years", "Age", "Weight");

                }
                else
                {
                    PlotChart("Weight - Age Chart:Girls(2 to 20 Years)", "Age (Years)", "Weight (lbs)", "2 to 20 Years", "Age", "Weight");

                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }

        protected void chkBMIAge_CheckedChanged(object sender, EventArgs e)
        {
            InitializeScreen();
            if (chkBMIAge.Checked == false)
            {
                return;
            }
            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Body Mass Index - Age Chart:Boys(2 to 20 Years)", "Age (Years)", "BMI", "2 to 20 Years", "Age", "BMI");
                }
                else
                {
                    PlotChart("Body Mass Index - Age Chart:Girls(2 to 20 Years)", "Age (Years)", "BMI", "2 to 20 Years", "Age", "BMI");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkStatureAge_CheckedChanged(object sender, EventArgs e)
        {

            InitializeScreen();
            if (chkStatureAge.Checked == false)
            {
                return;
            }
            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Stature - Age Chart:Boys(2 to 20 Years)", "Age (Years)", "Stature (Inches)", "2 to 20 Years", "Age", "Stature");
                }
                else
                {
                    PlotChart("Stature - Age Chart:Girls(2 to 20 Years)", "Age (Years)", "Stature (Inches)", "2 to 20 Years", "Age", "Stature");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkWeightStature_CheckedChanged(object sender, EventArgs e)
        {


            InitializeScreen();

            if (chkWeightStature.Checked == false)
            {
                return;
            }

            if (ClientSession.FillPatientChart != null && ClientSession.FillPatientChart.PatChartList.Count != 0)
            {
                if (ClientSession.FillPatientChart.PatChartList[0].Sex == "MALE")
                {
                    PlotChart("Weight - Stature Chart:Boys(2 to 5 Years)", "Stature (Inches)", "Weight (Lbs)", "2 to 5 Years", "Stature", "Weight");
                }
                else
                {
                    PlotChart("Weight - Stature Chart:Girls(2 to 5 Years)", "Stature (Inches)", "Weight (Lbs)", "2 to 5 Years", "Stature", "Weight");
                }
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);


        }

        void PlotChart(string ChartTitle, string XAxisTitle, string YAxisTitle, string Category, string X_Axis_Unit, string Y_Axis_Unit)
        {
            if (ClientSession.FillPatientChart.PatChartList[0].Sex.ToUpper() == "UNKNOWN")
            {
                ScriptManager.RegisterStartupScript(this, typeof(frmEncounter), "ErrorMessage", "DisplayErrorMessage('1010001','','');", true);
                return;
            }

            chrtGrowth.ChartTitle.TextBlock.Text = ChartTitle;

            // Set text and line for X axis
            chrtGrowth.PlotArea.XAxis.AxisLabel.Visible = true;
            chrtGrowth.PlotArea.YAxis.AxisLabel.Visible = true;
            chrtGrowth.PlotArea.XAxis.AxisLabel.TextBlock.Text = XAxisTitle;
            chrtGrowth.PlotArea.YAxis.AxisLabel.TextBlock.Text = YAxisTitle;

            IList<GrowthChart_Lookup> GrowthList = null;
            IList<GrowthChart_Lookup> CurrentGrowth = null;
            GrowthList = GrowthMngr.GetGrowthChartLookup(ClientSession.FillPatientChart.PatChartList[0].Sex, Category, X_Axis_Unit, Y_Axis_Unit);

            // Create a ChartSeries and assign its name and chart type
            ChartSeries chartSeriesPatient = new ChartSeries();
            chartSeriesPatient.Name = "Patient";
            chartSeriesPatient.Type = ChartSeriesType.Line;
            chartSeriesPatient.Appearance.PointMark.Visible = true;
            chartSeriesPatient.Appearance.PointMark.Dimensions.Width = 5;
            chartSeriesPatient.Appearance.PointMark.Dimensions.Height = 5;
            chartSeriesPatient.Appearance.PointMark.FillStyle.MainColor = System.Drawing.Color.Red;
            chartSeriesPatient.Appearance.PointMark.Figure = Telerik.Charting.Styles.DefaultFigures.Cross;

            ChartSeries chartSeries3 = new ChartSeries();
            chartSeries3.Name = "3rd";
            chartSeries3.Type = ChartSeriesType.Line;
            chartSeries3.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries3.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries5 = new ChartSeries();
            chartSeries5.Name = "5th";
            chartSeries5.Type = ChartSeriesType.Line;
            chartSeries5.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries5.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries10 = new ChartSeries();
            chartSeries10.Name = "10th";
            chartSeries10.Type = ChartSeriesType.Line;
            chartSeries10.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries10.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries25 = new ChartSeries();
            chartSeries25.Name = "25th";
            chartSeries25.Type = ChartSeriesType.Line;
            chartSeries25.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries25.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries50 = new ChartSeries();
            chartSeries50.Name = "50th";
            chartSeries50.Type = ChartSeriesType.Line;
            chartSeries50.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries50.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries75 = new ChartSeries();
            chartSeries75.Name = "75th";
            chartSeries75.Type = ChartSeriesType.Line;
            chartSeries75.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries75.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries85 = new ChartSeries();
            chartSeries85.Name = "85th";
            chartSeries85.Type = ChartSeriesType.Line;
            chartSeries85.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries85.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries90 = new ChartSeries();
            chartSeries90.Name = "90th";
            chartSeries90.Type = ChartSeriesType.Line;
            chartSeries90.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries90.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries95 = new ChartSeries();
            chartSeries95.Name = "95th";
            chartSeries95.Type = ChartSeriesType.Line;
            chartSeries95.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries95.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            ChartSeries chartSeries97 = new ChartSeries();
            chartSeries97.Name = "97th";
            chartSeries97.Type = ChartSeriesType.Line;
            chartSeries97.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
            chartSeries97.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right;

            IList<PatientResults> WeightGrowth = new List<PatientResults>();
            grdGrowthChart.DataSource = null;
            dtGrowthChart.Columns.Add("Captured DateTime", typeof(DateTime));
            dtGrowthChart.Columns.Add(XAxisTitle, typeof(string));
            dtGrowthChart.Columns.Add(YAxisTitle, typeof(string));
            chrtGrowth.PlotArea.XAxis.AutoScale = false;
            chrtGrowth.PlotArea.XAxis.AddRange(GrowthList[0].X_Axis_Start, GrowthList[0].X_Axis_End, GrowthList[0].X_Axis_Interval);
            chrtGrowth.PlotArea.YAxis.AutoScale = false;
            chrtGrowth.PlotArea.YAxis.AddRange(GrowthList[0].Y_Axis_Start, GrowthList[0].Y_Axis_End, GrowthList[0].Y_Axis_Interval);
            chrtGrowth.Legend.Visible = false;

            for (int i = GrowthList[0].X_Axis_End; i >= GrowthList[0].X_Axis_Start; i--)
            {
                WeightGrowth = new List<PatientResults>();
                if (X_Axis_Unit.Contains("Age") == true)
                {

                    if (Category.ToUpper().Contains("YEARS") == true)
                    {
                        var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == Y_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && v.Internal_Property_Year == i select v;
                        WeightGrowth = vital.ToList<PatientResults>();
                    }
                    else
                    {
                        var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == Y_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height").Replace("HC", "Head Circumference") && v.Internal_Property_Month == i select v;
                        WeightGrowth = vital.ToList<PatientResults>();
                    }
                }
                else if (X_Axis_Unit.Contains("Stature") == true)
                {
                    try
                    {
                        var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == X_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && Math.Round(Convert.ToDecimal(v.Value)) == i select v;
                        WeightGrowth = vital.ToList<PatientResults>();
                    }
                    catch
                    {

                    }
                }
                else if (XAxisTitle.Contains("(Inches)") == true)
                {
                    if (XAxisTitle.Contains("(Inches)") == true && Y_Axis_Unit.ToUpper().Contains("WEIGHT") == true)
                    {
                        try
                        {
                            var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == X_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && Math.Round(Convert.ToDecimal(v.Value)) == i && v.Internal_Property_Month <= 36 select v;
                            WeightGrowth = vital.ToList<PatientResults>();
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == X_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && Math.Round(Convert.ToDecimal(v.Value)) == i select v;
                            WeightGrowth = vital.ToList<PatientResults>();
                        }
                        catch
                        {

                        }
                    }
                }

                else
                {
                    try
                    {
                        var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == Y_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && Math.Round(Convert.ToDecimal(v.Value)) == i select v;
                        WeightGrowth = vital.ToList<PatientResults>();
                    }
                    catch
                    {

                    }
                }
                ChartSeriesItem csi = new ChartSeriesItem();
                if (WeightGrowth.Count > 0)
                {
                    for (int k = 0; k < WeightGrowth.Count; k++)
                    {
                        DataRow dtRow = dtGrowthChart.NewRow();
                        dtRow["Captured DateTime"] = UtilityManager.ConvertToLocal(WeightGrowth[k].Captured_date_and_time).ToString("dd-MMM-yyyy hh:mm:ss tt");

                        if (X_Axis_Unit.Contains("Age") == true)
                        {
                            if (Category.ToUpper().Contains("YEARS") == true)
                            {
                                dtRow[XAxisTitle] = CalculateAge(ClientSession.PatientPaneList[0].Birth_Date, DateTime.Today).ToString();
                            }
                            else
                            {
                                dtRow[XAxisTitle] = WeightGrowth[k].Internal_Property_Month.ToString();
                            }
                        }
                        else if (XAxisTitle.Contains("(Inches)") == true)
                        {
                            try
                            {
                                dtRow[XAxisTitle] = Math.Round(Convert.ToDecimal(WeightGrowth[k].Value)).ToString();



                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            try
                            {
                                dtRow[XAxisTitle] = WeightGrowth[k].Value.ToString();
                            }
                            catch
                            {

                            }
                        }
                        if (Y_Axis_Unit == "Weight" && XAxisTitle.Contains("(Inches)"))
                        {

                            try
                            {
                                UtilityManager utilMngr = new UtilityManager();
                                csi = new ChartSeriesItem();
                                IList<PatientResults> WeightGrowthList = new List<PatientResults>();
                                if (XAxisTitle == "Stature (Inches)")
                                {
                                    var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == "Weight" && v.Vitals_Group_ID == WeightGrowth[k].Vitals_Group_ID select v;
                                    WeightGrowthList = vital.ToList<PatientResults>();


                                    dtRow[YAxisTitle] = WeightGrowthList[0].Value;


                                    csi.XValue = Convert.ToDouble(dtRow[XAxisTitle]);
                                    csi.YValue = Convert.ToDouble(dtRow[YAxisTitle]);

                                    chartSeriesPatient.AddItem(csi);
                                }
                                else
                                {
                                    var vital = from v in vitalDTO.VitalsList where v.Loinc_Observation == X_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && v.Vitals_Group_ID == WeightGrowth[k].Vitals_Group_ID select v;
                                    WeightGrowthList = vital.ToList<PatientResults>();
                                    dtRow[XAxisTitle] = WeightGrowthList[0].Value;

                                    var vitals = from v in vitalDTO.VitalsList where v.Loinc_Observation == Y_Axis_Unit.Replace("Length", "Height").Replace("Stature", "Height") && v.Vitals_Group_ID == WeightGrowth[k].Vitals_Group_ID select v;
                                    WeightGrowthList = vitals.ToList<PatientResults>();
                                    dtRow[YAxisTitle] = WeightGrowthList[0].Value;
                                    csi.XValue = Convert.ToDouble(dtRow[XAxisTitle]);
                                    csi.YValue = Convert.ToDouble(dtRow[YAxisTitle]);

                                    chartSeriesPatient.AddItem(csi);

                                }

                            }
                            catch
                            {

                            }
                        }
                        else if (Y_Axis_Unit == "Weight")
                        {
                            UtilityManager utilMngr = new UtilityManager();
                            csi = new ChartSeriesItem();
                            csi.XValue = Convert.ToDouble(dtRow[XAxisTitle]);
                            try
                            {
                                csi.YValue = Convert.ToDouble(WeightGrowth[k].Value);
                                dtRow[YAxisTitle] = WeightGrowth[k].Value;
                            }
                            catch
                            {

                            }
                            chartSeriesPatient.AddItem(csi);
                        }
                        else if (Y_Axis_Unit == "Length" || Y_Axis_Unit == "Stature")
                        {
                            try
                            {
                                UtilityManager utilMngr = new UtilityManager();
                                csi = new ChartSeriesItem();
                                csi.XValue = Convert.ToDouble(dtRow[XAxisTitle]);
                                csi.YValue = Convert.ToDouble(WeightGrowth[k].Value);
                                chartSeriesPatient.AddItem(csi);
                                dtRow[YAxisTitle] = WeightGrowth[k].Value;
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            try
                            {
                                csi = new ChartSeriesItem();
                                csi.XValue = Convert.ToDouble(dtRow[XAxisTitle]);
                                csi.YValue = Convert.ToDouble(WeightGrowth[k].Value);
                                chartSeriesPatient.AddItem(csi);
                                dtRow[YAxisTitle] = WeightGrowth[k].Value;
                            }
                            catch
                            {

                            }
                        }

                        dtGrowthChart.Rows.Add(dtRow);
                        if (dtGrowthChart.Rows.Count > 0)
                        {
                            dtGrowthChart.DefaultView.Sort = "Captured DateTime desc";
                            grdGrowthChart.DataSource = dtGrowthChart;
                        }
                        grdGrowthChart.DataBind();
                    }
                }
                else
                {
                    if (dtGrowthChart.Rows.Count > 0)
                    {
                        dtGrowthChart.DefaultView.Sort = "Captured DateTime desc";
                        DataTable dt = new DataTable();

                        grdGrowthChart.DataSource = dtGrowthChart;
                    }
                    grdGrowthChart.DataBind();
                }

                if (grdGrowthChart.MasterTableView.AutoGeneratedColumns.Count() > 0)
                {
                    foreach (GridColumn col in grdGrowthChart.MasterTableView.AutoGeneratedColumns)
                    {
                        if (col.HeaderText == "Captured DateTime")
                        {
                            GridDateTimeColumn dv = (GridDateTimeColumn)col;
                            dv.DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}";
                        }
                        col.ItemStyle.Wrap = true;
                    }
                }
                grdGrowthChart.DataBind();

                var percent = from p in GrowthList where Convert.ToInt32(p.X_Axis_Value) == i select p;
                CurrentGrowth = percent.ToList<GrowthChart_Lookup>();

                if (CurrentGrowth.Count > 0)
                {
                    string sLabel = " ";

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_3)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_3)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_3);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries3.Name;
                    SeriesItemLabel s = new SeriesItemLabel();
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries3.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_5)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_5)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_5);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries5.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries5.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_10)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_10)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_10);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries10.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries10.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_25)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_25)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_25);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries25.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries25.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_50)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_50)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_50);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries50.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries50.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_75)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_75)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_75);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries75.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries75.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_85)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_85)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_85);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries85.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries85.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_90)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_90)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_90);
                    }
                    //if (i == iXEnd)
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries90.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries90.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_95)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_95)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_95);
                    }
                    //if (i == iXEnd)
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries95.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries95.AddItem(csi);

                    csi = new ChartSeriesItem();
                    csi.XValue = i;
                    if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "LENGTH" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "HC" || CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "STATURE")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertCMToInches(Convert.ToString(CurrentGrowth[0].Percentile_97)));
                    }
                    else if (CurrentGrowth[0].Y_Axis_Unit.ToUpper() == "WEIGHT")
                    {
                        csi.YValue = Convert.ToDouble(objUIManager.ConvertKgToLbs(Convert.ToString(CurrentGrowth[0].Percentile_97)));
                    }
                    else
                    {
                        csi.YValue = Convert.ToDouble(CurrentGrowth[0].Percentile_97);
                    }
                    if (i == GrowthList[0].X_Axis_End)
                        sLabel = chartSeries97.Name;
                    csi.Label.TextBlock.Text = sLabel;
                    chartSeries97.AddItem(csi);
                }
            }
            chartSeriesPatient.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Red;
            if (ChartTitle.Contains("Boys") == true)
            {
                chartSeries3.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries5.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries10.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries25.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries50.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.DarkBlue;
                chartSeries75.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries85.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries90.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries95.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
                chartSeries97.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Blue;
            }
            else
            {
                chartSeries3.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries5.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries10.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries25.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries50.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.DeepPink;
                chartSeries75.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries85.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries90.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries95.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
                chartSeries97.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Pink;
            }
            chrtGrowth.Series.Add(chartSeriesPatient);
            chrtGrowth.Series.Add(chartSeries3);
            chrtGrowth.Series.Add(chartSeries5);
            chrtGrowth.Series.Add(chartSeries10);
            chrtGrowth.Series.Add(chartSeries25);
            chrtGrowth.Series.Add(chartSeries50);
            chrtGrowth.Series.Add(chartSeries75);
            chrtGrowth.Series.Add(chartSeries85);
            chrtGrowth.Series.Add(chartSeries90);
            chrtGrowth.Series.Add(chartSeries95);
            chrtGrowth.Series.Add(chartSeries97);
            divLoading.Style.Add("display", "none");
        }

        void InitializeScreen()
        {
            chrtGrowth.Series.Clear();
            chrtGrowth.ChartTitle.TextBlock.Text = string.Empty;
            chrtGrowth.PlotArea.EmptySeriesMessage.TextBlock.Text = string.Empty;
            if (grdGrowthChart.Columns.Count > 0)
            {
                for (int i = 1; i < grdGrowthChart.Columns.Count; i++)
                {
                    grdGrowthChart.Columns.RemoveAt(i);
                }
            }
            if (ClientSession.HumanId != 0)
                vitalDTO = vitalMngr.GetPastVitalDetailsByPatientforGrowthChart(ClientSession.HumanId, 1, 100000);

            if (vitalDTO.VitalsList.Count > 0)
            {
                for (int i = 0; i < vitalDTO.VitalsList.Count; i++)
                {
                    if (vitalDTO.VitalsList[i].Loinc_Observation == "Height")
                    {
                        string sValue = string.Empty;
                        if (vitalDTO.VitalsList[i].Value.Contains("'") == true && vitalDTO.VitalsList[i].Value.Contains("''") == true)
                        {
                            string[] val = vitalDTO.VitalsList[i].Value.Split('\'');
                            sValue = objUIManager.ConvertFeetInchToInch(val[0], val[1]);
                        }
                        else
                        {
                            sValue = vitalDTO.VitalsList[i].Value;
                        }

                        vitalDTO.VitalsList[i].Value = sValue;
                    }

                }
                vitalDTO.VitalsList.OrderByDescending(v => v.Captured_date_and_time);
            }
        }

        int CalculateAge(DateTime birthDate, DateTime ToDate)
        {
            // get the difference in years
            int years = ToDate.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (ToDate.Month < birthDate.Month || (ToDate.Month == birthDate.Month && ToDate.Day < birthDate.Day))
                --years;
            return years;
        }

        protected void btnPrintChart_Click(object sender, EventArgs e)
        {
            string FileName = string.Empty;
            if (chkBMIAge.Checked == true)
            {
                chkBMIAge_CheckedChanged(sender, e);
            }
            else if (chkHCAgeBirth.Checked == true)
            {
                chkHCAgeBirth_CheckedChanged(sender, e);
            }
            else if (chkLengthAgeBirth.Checked == true)
            {
                chkLengthAgeBirth_CheckedChanged(sender, e);
            }
            else if (chkStatureAge.Checked == true)
            {
                chkStatureAge_CheckedChanged(sender, e);
            }
            else if (chkWeightAge.Checked == true)
            {
                chkWeightAge_CheckedChanged(sender, e);
            }
            else if (chkWeightAgeBirth.Checked == true)
            {
                chkWeightAgeBirth_CheckedChanged(sender, e);
            }
            else if (chkWeightLengthBirth.Checked == true)
            {
                chkWeightLengthBirth_CheckedChanged(sender, e);
            }
            else if (chkWeightStature.Checked == true)
            {
                chkWeightStature_CheckedChanged(sender, e);
            }
            FileName = String.Format("/ChartImage.png", 0);
            RadChart RadChart1 = this.Page.FindControl("chrtGrowth") as RadChart;
            string sDirPath = Server.MapPath("Documents/" + Session.SessionID);
            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);
            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            using (var m = new MemoryStream())
            {
                RadChart1.Save(m, ImageFormat.Png);
                var img = System.Drawing.Image.FromStream(m);
                img.Save(Server.MapPath("Documents/" + Session.SessionID) + FileName);
            }

            string sFaxSubject = string.Empty;
            if (ConfigurationSettings.AppSettings["IsEFax"] != null && ConfigurationSettings.AppSettings["IsEFax"].ToString().ToUpper() == "Y")
            {
                string sFaxFirstname = string.Empty;
                string sFaxLastName = string.Empty;

                IList<string> ilstHumanTag = new List<string>();
                ilstHumanTag.Add("HumanList");

                IList<object> ilstHumanBlobList = new List<object>();
                ilstHumanBlobList = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHumanTag);

                Human objFillHuman = new Human();

                if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
                {
                    if (ilstHumanBlobList[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                        {
                            objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                            sFaxFirstname = objFillHuman.First_Name;
                            sFaxLastName = objFillHuman.Last_Name;
                        }
                    }
                }

                //string human_id = "Human" + "_" + ClientSession.HumanId.ToString() + ".xml";
                //string strXmlHumanPath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], human_id);
                //if (File.Exists(strXmlHumanPath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlHumanPath);
                //    using (FileStream fs = new FileStream(strXmlHumanPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("HumanList")[0] != null)
                //        {
                //            if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes.Count > 0)
                //            {
                //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value != null)
                //                    sFaxFirstname = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("First_Name").Value.ToString();
                //                if (itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value != null)
                //                    sFaxLastName = "_" + itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes[0].Attributes.GetNamedItem("Last_Name").Value.ToString();
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
                sFaxSubject = "Growth_Chart" + sFaxLastName + sFaxFirstname + "_" + DateTime.Now.ToString("dd-MMM-yyyy");
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "OpenViewer", "printchart('" + FileName + "','" + sFaxSubject + "');", true);
        }
        //    ModalWindow.Visible = true;
        //    ModalWindow.VisibleOnPageLoad = true;
        //    ModalWindow.NavigateUrl = "frmPrintPDF.aspx?SI=" + FileName + "&Location=CHART";
        //    ModalWindow.VisibleStatusbar = false;
        //    ModalWindow.Height = Unit.Pixel(830);
        //    ModalWindow.Width = Unit.Pixel(785);
        //    ModalWindow.CenterIfModal = true;
        //    ModalWindow.KeepInScreenBounds = true;
        //    ModalWindow.Behaviors = WindowBehaviors.Close | WindowBehaviors.Resize;
        //}

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}", true);
            if (chkBMIAge.Checked == true)
            {
                chkBMIAge_CheckedChanged(sender, e);
            }
            else if (chkHCAgeBirth.Checked == true)
            {
                chkHCAgeBirth_CheckedChanged(sender, e);
            }
            else if (chkLengthAgeBirth.Checked == true)
            {
                chkLengthAgeBirth_CheckedChanged(sender, e);
            }
            else if (chkStatureAge.Checked == true)
            {
                chkStatureAge_CheckedChanged(sender, e);
            }
            else if (chkWeightAge.Checked == true)
            {
                chkWeightAge_CheckedChanged(sender, e);
            }
            else if (chkWeightAgeBirth.Checked == true)
            {
                chkWeightAgeBirth_CheckedChanged(sender, e);
            }
            else if (chkWeightLengthBirth.Checked == true)
            {
                chkWeightLengthBirth_CheckedChanged(sender, e);
            }
            else if (chkWeightStature.Checked == true)
            {
                chkWeightStature_CheckedChanged(sender, e);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        private MemoryStream drawChart()
        {
            RadChart chart = this.Page.FindControl("chrtGrowth") as RadChart;
            // Do the necessary coding in order to generate the chart.
            //If more than one chart  is being used call the same page with necessary inputs to generate a new image.
            MemoryStream mStream = new MemoryStream();
            chart.Save(mStream, ImageFormat.Jpeg);
            return mStream;
        }

    }
}
