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
using System.Collections.Generic;
using Telerik.Web.UI;
using Telerik.Charting;
using Telerik.Charting.Styles;
using System.Drawing;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.IO;
using System.Drawing.Imaging;
using System.Xml;

namespace Acurus.Capella.UI
{
    public partial class frmFlowSheetGraph : System.Web.UI.Page
    {
        ArrayList ary = new ArrayList();

        ArrayList aryOne = new ArrayList();
        ArrayList aryTwo = new ArrayList();
        ArrayList aryThree = new ArrayList();

        ArrayList aryList = new ArrayList();

        DataSet ds = new DataSet();

        IList<string> graphList = new List<string>();

        FlowSheetTemplateManager flowsheetMngr = new FlowSheetTemplateManager();

        string sUnit = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] SplitCharater = new string[] { "  " };
                IList<string> dateList = new List<string>();
                IList<string> valueList = new List<string>();

                DataTable dtFlowSheet = new DataTable();

                dtFlowSheet.Columns.Add("Category", typeof(string));

                UIManager UIMngr = new UIManager();
                IList<PatientResults> lstPatientResults = new List<PatientResults>();
                //Changed by Vaishali for bug ID 36830
                //ulong physicianid = ClientSession.PhysicianId == 0 ?
                //   Convert.ToUInt32(Convert.ToString(Request["PhyID"])) : ClientSession.PhysicianId;
                ulong physicianid = string.IsNullOrEmpty(Request["PhyID"]) ?
                                  ClientSession.PhysicianId :
                                  Convert.ToUInt32(Request["PhyID"]);
                var selectCriteria = Convert.ToString(Request["Selected"]);
                var selectedTemplate = Convert.ToString(Request["FTN"]);
                var fromDate = Convert.ToString(Request["FD"]);
                var toDate = Convert.ToString(Request["TD"]);

                if (string.Compare(selectCriteria, "ALL", true) == 0)
                {
                    lstPatientResults = flowsheetMngr.GetFlowSheetDetails(ClientSession.HumanId, selectedTemplate, physicianid);
                }
                else
                {
                    lstPatientResults = flowsheetMngr.GetFlowSheetDetailsByDate(ClientSession.HumanId, selectedTemplate,
                                                                            fromDate, toDate, physicianid);
                }

                DataRow dtNewRow;
                IList<DateTime> DOSList = new List<DateTime>();

                if (lstPatientResults.Count != 0)
                {
                    IList<DateTime> TempDOSList = new List<DateTime>();

                    var enc = (from en in lstPatientResults orderby en.Captured_date_and_time select en.Captured_date_and_time);//.Distinct();
                    TempDOSList = enc.ToList<DateTime>();

                    for (int j = 0; j < TempDOSList.Count; j++)
                    {
                        if (j != 0)
                        {
                            if (TempDOSList[j] == TempDOSList[j - 1])
                            {
                                continue;
                            }
                        }
                        DOSList.Add(UtilityManager.ConvertToLocal(TempDOSList[j]));
                    }

                }
                //Changed by vaishali on 05-01-2015 for bug ID 36852
                var DOSAsendingList = DOSList.OrderBy(c => c.Date).ThenBy(n => n.TimeOfDay);

                DOSList = DOSAsendingList.ToList<DateTime>();
                for (int i = 0; i < DOSList.Count; i++)
                {

                    GridBoundColumn d = new GridBoundColumn();
                    d.HeaderText = DOSList[i].ToString("dd-MMM-yyyy hh:mm tt");
                    var Result = (from c in dtFlowSheet.Columns.Cast<DataColumn>() where c.ColumnName == d.HeaderText select c);
                    if (Result.Count() == 0)
                    {
                        dtFlowSheet.Columns.Add(d.HeaderText, typeof(string));
                    }
                }

                string sRowIndex = string.Empty;
                IList<FlowSheetTemplate> flowList = new List<FlowSheetTemplate>();

                flowList = flowsheetMngr.GetFlowSheetTemplate(physicianid);

                var query = from c in flowList where c.Template_Name.ToUpper() == Request["FTN"].ToString().ToUpper() select c;
                IList<FlowSheetTemplate> flowsheet = query.ToList<FlowSheetTemplate>();
                string[] strVitalList = null;
                IList<string> vitalList = new List<string>();
                for (int j = 0; j < flowsheet.Count; j++)
                {
                    strVitalList = flowsheet[j].Acurus_Result_Code.Split('|');
                    for (int i = 0; i < strVitalList.Length; i++)
                    {
                        vitalList.Add(strVitalList[i].ToString());
                    }
                }

                IList<PatientResults> FlowsheetListForVitals = new List<PatientResults>();
                for (int k = 0; k < vitalList.Count; k++)
                {
                    var query1 = from p in lstPatientResults where p.Acurus_Result_Code == vitalList[k] select p;
                    IList<PatientResults> patientResultList = query1.ToList<PatientResults>();
                    FlowsheetListForVitals = FlowsheetListForVitals.Concat(patientResultList).ToList<PatientResults>();
                }
                for (int i = 0; i < FlowsheetListForVitals.Count; i++)
                {
                    bool bNewRow = true;

                    if (FlowsheetListForVitals[i].Value != string.Empty)
                    {
                        for (int j = 0; j < dtFlowSheet.Rows.Count; j++)
                        {
                            DataRow dtRow1 = dtFlowSheet.Rows[j];
                            if (dtRow1["Category"].ToString() == FlowsheetListForVitals[i].Acurus_Result_Description)
                            {
                                sRowIndex = j.ToString();
                                break;
                            }
                        }
                        if (sRowIndex == string.Empty)
                        {
                            dtNewRow = dtFlowSheet.NewRow();
                        }
                        else
                        {
                            bNewRow = false;
                            dtNewRow = dtFlowSheet.Rows[Convert.ToInt32(sRowIndex)];
                        }
                        sRowIndex = string.Empty;
                        dtNewRow["Category"] = FlowsheetListForVitals[i].Acurus_Result_Description;
                        string sVitalValue = FlowsheetListForVitals[i].Value;

                        switch (FlowsheetListForVitals[i].Units)
                        {
                            case "CM":
                                sVitalValue = UIMngr.ConvertInchesToCM(FlowsheetListForVitals[i].Value);
                                break;

                            case "Kg":
                                sVitalValue = UIMngr.ConvertLbsToKg(FlowsheetListForVitals[i].Value);
                                break;

                            case "Celsius":
                                sVitalValue = UIMngr.ConvertFarenheitToCelsius(FlowsheetListForVitals[i].Value);
                                break;

                            case "Ft Inch":
                                //sVitalValue = UIMngr.ConvertInchtoFeetInch(FlowsheetListForVitals[i].Value); /*changed for bug id 28382,27914
                                if (sVitalValue.Contains("'") == true && sVitalValue.Contains("''") == true)
                                {
                                    sVitalValue = FlowsheetListForVitals[i].Value;
                                    break;
                                }
                                else
                                {
                                    sVitalValue = UIMngr.ConvertInchtoFeetInch(FlowsheetListForVitals[i].Value);
                                    break;
                                }
                        }
                        dtNewRow[UtilityManager.ConvertToLocal(FlowsheetListForVitals[i].Captured_date_and_time).ToString("dd-MMM-yyyy hh:mm tt")] = sVitalValue + "  " + FlowsheetListForVitals[i].Units;
                        if (bNewRow == true)
                        {
                            dtFlowSheet.Rows.Add(dtNewRow);
                        }
                    }
                }

                ViewState["GridData"] = dtFlowSheet;
                //Changed by vaishali for bug ID: 36851
                DataRow[] dtRowCollection = dtFlowSheet.Select("Category = '" + Request["Category"] + "'");
                // DataRow dtRow = dtFlowSheet.Rows[Convert.ToInt32(Request["Index"])];           
                if (dtRowCollection.Length > 0)
                {
                    DataRow dtRow = dtRowCollection[0];
                    string sCategory = dtRow[0].ToString();

                    for (int i = 1; i < dtFlowSheet.Rows.Count; i++)
                    {
                        if (sUnit == "")
                        {
                            sUnit = dtRow[i].ToString();
                        }
                    }

                    for (int i = 1; i < dtFlowSheet.Columns.Count; i++)
                    {
                        string sDate = dtFlowSheet.Columns[i].ColumnName;
                        if (dtRow[i].ToString() != string.Empty && dtRow[i].ToString().Contains('>') == false && dtRow[i].ToString().Contains('<') == false)
                        {
                            dateList.Add(sDate);
                        }
                    }
                    for (int i = 1; i < dtFlowSheet.Columns.Count; i++)
                    {
                        string sValue = string.Empty;
                        sValue = dtRow[i].ToString();
                        if (sValue != string.Empty)
                        {
                            if (sValue.Contains('>') == false && sValue.Contains('<') == false)
                            {
                                valueList.Add(sValue);
                            }
                        }
                    }
                    chkDataitems.Items.Clear();
                    for (int i = 0; i < dateList.Count; i++)
                    {
                        string sCollections = string.Empty;
                        if (valueList[i].Trim() == string.Empty && valueList[i] == "&nbsp;")
                            sCollections = dateList[i] + "|" + valueList[i] + "|" + (i + 1);
                        else
                            sCollections = dateList[i] + "|" + valueList[i].Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0] + "|" + (i + 1);
                        graphList.Add(sCollections);

                    }
                    try
                    {
                        ds = GetSampleData(graphList, valueList, sCategory);

                        if (graphList.Count == 1)
                        {
                            chrtFlowSheet.DefaultType = ChartSeriesType.Point;
                        }
                        ArrayList aryList = new ArrayList();
                        for (int j = 0; j < dtFlowSheet.Rows.Count; j++)
                        {
                            dtRow = dtFlowSheet.Rows[j];
                            chkDataitems.Items.Add(new RadListBoxItem(dtRow["Category"].ToString()));
                        }
                        //Changed by vaishali for bug id:36851
                        // DataRow selectedRow = dtFlowSheet.Rows[Convert.ToInt32(Request["Index"])];
                        int itemIndex = chkDataitems.FindItemByText(Request["Category"].ToString()).Index;
                        if (itemIndex != null)
                        {
                            chkDataitems.Items[itemIndex].Selected = true;
                            chkDataitems.Items[itemIndex].Checked = true;

                            ary.Add(chkDataitems.SelectedValue);
                            SelectedItem.Value = chkDataitems.SelectedValue;
                        }
                        chrtFlowSheet.ChartTitle.TextBlock.Text = sCategory;
                        chrtFlowSheet.ChartTitle.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                        chrtFlowSheet.ChartTitle.TextBlock.Appearance.AutoTextWrap = AutoTextWrap.True;
                        chrtFlowSheet.AutoLayout = true;

                        chrtFlowSheet.PlotArea.XAxis.AxisLabel.Visible = true;
                        chrtFlowSheet.PlotArea.YAxis.AxisLabel.Visible = true;
                        chrtFlowSheet.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                        chrtFlowSheet.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                        chrtFlowSheet.PlotArea.XAxis.DataLabelsColumn = "OADate";
                        chrtFlowSheet.PlotArea.XAxis.LabelStep = 1;
                        chrtFlowSheet.PlotArea.XAxis.MinValue = 1D;
                        chrtFlowSheet.PlotArea.XAxis.MaxValue = 100D;
                        chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = AutoTextWrap.True;
                        chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;

                        if (sUnit.Contains(" ") == true)
                        {
                            string[] strUnit = sUnit.Split(' ');
                            sUnit = string.Empty;
                            if (strUnit.Length == 3)
                            {
                                for (int k = 1; k < strUnit.Length; k++)
                                {
                                    if (sUnit == string.Empty)
                                        sUnit = strUnit[k].ToString();
                                    else
                                        sUnit = sUnit + strUnit[k].ToString();

                                }
                            }
                            if (strUnit.Length == 5)
                            {
                                for (int k = 3; k < strUnit.Length; k++)
                                {
                                    if (sUnit == string.Empty)
                                        sUnit = strUnit[k].ToString();
                                    else
                                        sUnit = sUnit + strUnit[k].ToString();

                                }
                            }
                        }


                        if (sUnit.Contains("Vitals") == true && sUnit.Contains("Ft") == false)
                        {
                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;

                        }
                        else if (sUnit.Contains("FtInch") == true)
                        {
                            sUnit = "Ft";//sUnit = "FtInch";Bug Id: 28871
                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;

                        }
                        else if (sUnit.Contains("Ft") == true)
                        {
                            sUnit = "Ft";//sUnit = "FtInch";Bug Id: 28871
                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                        }


                        else
                        {
                            if (sUnit != string.Empty)
                            {
                                chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                                chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                            }
                            else
                            {
                                chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory;
                                chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                            }
                        }
                        chrtFlowSheet.DataManager.DataSource = ds;


                        chrtFlowSheet.DataManager.DataMember = "Graph";
                        chrtFlowSheet.DataBind();
                        if (sCategory.Contains("BP") == true)
                        {
                            GetBPDetails(graphList);
                            chrtFlowSheet.Series[0].Name = "BP-Sitting(Sys)";
                            chrtFlowSheet.Series[1].Name = "BP-Sitting(Dia)";
                            chrtFlowSheet.Legend.Visible = true;
                        }
                        else
                        {
                            chrtFlowSheet.Legend.Visible = true;

                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException)
                        {
                            // ApplicationObject.erroHandler.DisplayErrorMessage("102011", this.Text);
                            chrtFlowSheet.Clear();

                            chrtFlowSheet.ChartTitle.TextBlock.Text = string.Empty;

                        }
                    }
                }
            }
        }

        public void GetBPDetails(IList<string> GraphList)
        {
            ChartSeries BPSysSeries = new ChartSeries();
            ChartSeries BPDiaSeries = new ChartSeries();
            for (int i = 0; i < GraphList.Count; i++)
            {

                string[] sItems = GraphList[i].Split('|');
                string[] sBP = sItems[1].Split('/');
                BPSysSeries.Items.Add(new ChartSeriesItem(Convert.ToDouble(sItems[2]), Convert.ToDouble(sBP[0])));
                BPDiaSeries.Items.Add(new ChartSeriesItem(Convert.ToDouble(sItems[2]), Convert.ToDouble(sBP[1])));
                BPSysSeries.Type = ChartSeriesType.Line;
                BPSysSeries.Appearance.LabelAppearance.Visible = true;
                BPSysSeries.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.Red;
                BPDiaSeries.Type = ChartSeriesType.Line;
                BPDiaSeries.Appearance.LabelAppearance.Visible = true;
                BPDiaSeries.Appearance.LineSeriesAppearance.Color = System.Drawing.Color.SteelBlue;
                chrtFlowSheet.PlotArea.YAxis.AutoScale = false;
                chrtFlowSheet.PlotArea.YAxis.MinValue = 35;
                chrtFlowSheet.PlotArea.YAxis.MaxValue = 230;
                chrtFlowSheet.PlotArea.YAxis.Step = Convert.ToDouble((230 - chrtFlowSheet.PlotArea.YAxis.MinValue) / 10);


                if (graphList.Count == 1)
                {
                    BPSysSeries.Type = ChartSeriesType.Point;
                    BPDiaSeries.Type = ChartSeriesType.Point;
                }
            }
            chrtFlowSheet.Series.Add(BPSysSeries);
            chrtFlowSheet.Series.Add(BPDiaSeries);
        }

        public DataSet GetSampleData(IList<string> GraphList, IList<string> ValueList, string Category)
        {
            string[] SplitCharater = new string[] { "  " };

            DataTable dt = new DataTable("Graph");
            dt.Clear();
            dt.Columns.Add("Date", typeof(string));
            if (Category.Contains("BP") == false)
            {
                dt.Columns.Add(Category, typeof(double));
            }
            DataRow drv = dt.NewRow();

            for (int i = 0; i < GraphList.Count; i++)
            {
                string[] sItems = GraphList[i].Split('|');
                if (sItems[0] != null)
                {
                    drv[0] = sItems[0];

                }

                if ((sItems[1] != string.Empty))//*
                {
                    if (!sItems[1].ToString().Contains("-"))//* FOR BUG ID 27914
                    {
                        if (Category == "Height")
                        {
                            string[] sHeight = sItems[1].Split('\'');
                            var maxHeight = (from h in ValueList where h.ToString() != "&nbsp;" select h).Max();
                            string[] sMax = (maxHeight.ToString()).Split('\'');
                            double maxInchandheight = Convert.ToDouble(sMax[1]) / 12 + Convert.ToDouble(sMax[0]);
                            double height = Convert.ToDouble(sHeight[1]) / 12 + Convert.ToDouble(sHeight[0]);
                            chrtFlowSheet.PlotArea.YAxis.AutoScale = false;
                            chrtFlowSheet.PlotArea.YAxis.MinValue = 0;
                            chrtFlowSheet.PlotArea.YAxis.Step = Convert.ToDouble((Math.Round(maxInchandheight, 2)) - chrtFlowSheet.PlotArea.YAxis.MinValue) / 10;
                            chrtFlowSheet.PlotArea.YAxis.MaxValue = maxInchandheight + chrtFlowSheet.PlotArea.YAxis.Step;
                            chrtFlowSheet.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Black;
                            drv[1] = Math.Round(height, 2);
                        }
                        else if (Category.Contains("BP") == false)
                        {
                            var maxValue = (from m in ValueList where m.ToString() != "&nbsp;" select Convert.ToDouble(m.Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0].ToString())).Max();
                            chrtFlowSheet.PlotArea.YAxis.AutoScale = false;
                            chrtFlowSheet.PlotArea.YAxis.MinValue = 0;
                            chrtFlowSheet.PlotArea.YAxis.Step = Convert.ToDouble((Convert.ToDouble(maxValue) - chrtFlowSheet.PlotArea.YAxis.MinValue) / 10);
                            chrtFlowSheet.PlotArea.YAxis.MaxValue = Convert.ToDouble(maxValue) + chrtFlowSheet.PlotArea.YAxis.Step;
                            chrtFlowSheet.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Black;
                            if (sItems[1] != "&nbsp;")
                            {
                                drv[1] = sItems[1];
                            }
                        }
                    }
                }
                dt.Rows.Add(drv);
                drv = dt.NewRow();

            }
            ds = new DataSet();
            ds.Tables.Add(new DataTable("Vitals"));
            ds.Tables.Add(dt);
            return ds;

        }

        protected void chkDataitems_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<string> SelectedItems = (chkDataitems.Items.Cast<RadListBoxItem>().Where(a => a.Checked).Select(b => b.Text)).ToList<string>();

            if (SelectedItems.Count > 0)
            {
                PlotGraph(chkDataitems.SelectedValue);
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('180053');  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void chkDataitems_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            IList<string> SelectedItems = (chkDataitems.Items.Cast<RadListBoxItem>().Where(a => a.Checked).Select(b => b.Text)).ToList<string>();

            if (SelectedItems.Count > 0)
            {
                if (e.Item.Checked == true)
                {
                    e.Item.Selected = true;
                }
                PlotGraph(e.Item.Text);
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('180053');  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                e.Item.Checked = true;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        public void PlotGraph(string SelectedValue)
        {
            if (SelectedItem.Value.Trim() != string.Empty && SelectedItem.Value.Contains(SelectedValue) == false)
            {
                SelectedItem.Value += "|" + SelectedValue;
                string[] SplitItems = SelectedItem.Value.Split('|');
                if (SplitItems.Length > 2)
                {
                    chkDataitems.Items[chkDataitems.FindItemByText(SplitItems[0].ToString()).Index].Checked = false;
                    chkDataitems.Items[chkDataitems.FindItemByText(SplitItems[0].ToString()).Index].Selected = false;
                    SelectedItem.Value = SelectedItem.Value.Replace(SplitItems[0].ToString() + "|", "");
                }
            }
            IList<string> SelectedItems = (chkDataitems.Items.Cast<RadListBoxItem>().Where(a => a.Checked).Select(b => b.Text)).ToList<string>();

            string sUnit = string.Empty;
            bool IsInsideSelectedIndexChanged = false;
            string[] SplitCharater = new string[] { "  " };

            if (ViewState["GridData"] == null)
            {
                return;
            }
            DataTable MyGridView = (DataTable)ViewState["GridData"];

            if (!IsInsideSelectedIndexChanged)
            {
                if (SelectedItems.Count != 0)
                {
                    IsInsideSelectedIndexChanged = true;

                    if (SelectedItems.Count > 0)
                    {
                        aryList.Add(SelectedItems[0].ToString());
                    }
                    else if (aryList.Contains(chkDataitems.SelectedItem))
                        aryList.Remove(chkDataitems.SelectedItem);

                    IsInsideSelectedIndexChanged = false;

                    #region Plot Graph
                    IList<string> dateList = new List<string>();
                    IList<string> valueList = new List<string>();
                    string sModule = string.Empty;
                    string sCategory = string.Empty;
                    try
                    {
                        if (SelectedItems.Count > 0)
                        {
                            if (SelectedItems.Count == 1)
                            {
                                sModule = string.Empty;
                                sCategory = string.Empty;
                                sUnit = string.Empty;
                                foreach (DataRow dtRow in MyGridView.Rows)
                                {
                                    if (SelectedItems[0].ToString() == dtRow[0].ToString()) //MyGridView.Rows[i].Cells[1].Text.ToString())
                                    {
                                        sCategory = dtRow[0].ToString();

                                        for (int i = 1; i < MyGridView.Rows.Count; i++)
                                        {
                                            if (sUnit == "")
                                            {
                                                sUnit = dtRow[i].ToString();
                                            }
                                        }
                                        for (int j = 1; j < MyGridView.Columns.Count; j++)
                                        {
                                            string sDate = MyGridView.Columns[j].ColumnName;
                                            if (dtRow[j].ToString() != string.Empty && dtRow[j].ToString().Contains('>') == false && dtRow[j].ToString().Contains('<') == false)
                                            {
                                                dateList.Add(sDate);
                                            }
                                        }
                                        for (int k = 1; k < MyGridView.Columns.Count; k++)
                                        {
                                            string sValue = string.Empty;
                                            sValue = dtRow[k].ToString();
                                            if (sValue != string.Empty)
                                            {
                                                if (sValue.Contains('>') == false && sValue.Contains('<') == false)
                                                {
                                                    valueList.Add(sValue.Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0]);
                                                }

                                            }
                                        }


                                    }


                                }
                                graphList.Clear();
                                for (int i = 0; i < dateList.Count; i++)
                                {
                                    string sCollections = dateList[i] + "|" + valueList[i] + "|" + (i + 1);
                                    graphList.Add(sCollections);
                                }
                                ds.Clear();
                                ds = GetSampleData(graphList, valueList, sCategory);
                                if (graphList.Count == 1)
                                {
                                    chrtFlowSheet.DefaultType = ChartSeriesType.Point;
                                }
                                else
                                {
                                    chrtFlowSheet.DefaultType = ChartSeriesType.Line;
                                }

                                graphList.Clear();
                                for (int i = 0; i < dateList.Count; i++)
                                {
                                    string sCollections = dateList[i] + "|" + valueList[i] + "|" + (i + 1);
                                    graphList.Add(sCollections);
                                }

                                chrtFlowSheet.Clear();
                                chrtFlowSheet.ChartTitle.TextBlock.Text = sCategory;
                                chrtFlowSheet.ChartTitle.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                                chrtFlowSheet.ChartTitle.TextBlock.Appearance.AutoTextWrap = AutoTextWrap.True;
                                chrtFlowSheet.AutoLayout = true;
                                chrtFlowSheet.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                                chrtFlowSheet.PlotArea.XAxis.DataLabelsColumn = "OADate";
                                chrtFlowSheet.PlotArea.XAxis.LabelStep = 1;
                                chrtFlowSheet.PlotArea.XAxis.MinValue = 1D;
                                chrtFlowSheet.PlotArea.XAxis.MaxValue = 100D;
                                chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = AutoTextWrap.True;
                                chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                if (sUnit.Contains(" ") == true)
                                {
                                    string[] strUnit = sUnit.Split(' ');
                                    sUnit = string.Empty;
                                    for (int k = 1; k < strUnit.Length; k++)
                                    {
                                        if (sUnit == string.Empty)
                                            sUnit = strUnit[k].ToString();
                                        else
                                            sUnit = sUnit + strUnit[k].ToString();

                                    }
                                }

                                if (sUnit.Contains("Ft") == false)
                                {
                                    chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                                    chrtFlowSheet.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                    chrtFlowSheet.DataManager.DataSource = ds;
                                    chrtFlowSheet.DataManager.DataMember = "Graph";
                                    chrtFlowSheet.DataBind();
                                }
                                else if (sUnit.Contains("Ft") == true)
                                {
                                    sUnit = "Ft";
                                    chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                                    chrtFlowSheet.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                    chrtFlowSheet.DataManager.DataSource = ds;
                                    chrtFlowSheet.DataManager.DataMember = "Graph";
                                    chrtFlowSheet.DataBind();
                                }
                                else
                                {
                                    if (sUnit != string.Empty)
                                    {
                                        sUnit = "Ft";
                                        chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                                        chrtFlowSheet.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        if (sUnit != string.Empty)
                                        {
                                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + "(" + sUnit + ")";
                                            chrtFlowSheet.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                        }
                                        else
                                        {
                                            chrtFlowSheet.PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory;
                                            chrtFlowSheet.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                        }
                                    }
                                }
                                chrtFlowSheet.DataManager.DataSource = ds;
                                chrtFlowSheet.DataManager.DataMember = "Graph";
                                chrtFlowSheet.DataBind();
                                if (sCategory.Contains("BP") == true)
                                {
                                    GetBPDetails(graphList);
                                    chrtFlowSheet.Series[0].Name = "BP-Sitting(Sys)";
                                    chrtFlowSheet.Series[1].Name = "BP-Sitting(Dia)";
                                    chrtFlowSheet.Legend.Visible = true;

                                }
                                else
                                {
                                    chrtFlowSheet.Legend.Visible = true;

                                }

                            }
                            else if (SelectedItems.Count == 2)
                            {
                                IList<DateTime> ilstDateTime = new List<DateTime>();
                                string sModule1 = string.Empty;
                                string sCategory1 = string.Empty;
                                string sUnit1 = string.Empty;
                                IList<string> ilstDateAndValue = new List<string>();
                                IList<string> ilstDateAndValue1 = new List<string>();
                                sModule = string.Empty;
                                sCategory = string.Empty;
                                sUnit = string.Empty;
                                foreach (DataRow dtRow in MyGridView.Rows)
                                {
                                    if (SelectedItems[0].ToString() == dtRow[0].ToString())
                                    {
                                        sCategory = dtRow[0].ToString();

                                        for (int i = 1; i < MyGridView.Rows.Count; i++)
                                        {
                                            if (sUnit == "")
                                            {
                                                sUnit = dtRow[i].ToString();
                                            }
                                        }
                                        for (int j = 1; j < MyGridView.Columns.Count; j++)
                                        {
                                            string sDate = MyGridView.Columns[j].ColumnName;
                                            if (dtRow[j].ToString() != string.Empty && dtRow[j].ToString().Contains('>') == false && dtRow[j].ToString().Contains('<') == false)
                                            {
                                                ilstDateTime.Add(Convert.ToDateTime(sDate));
                                                dateList.Add(sDate);
                                            }
                                        }

                                        for (int k = 1; k < MyGridView.Columns.Count; k++)
                                        {
                                            string sValue = string.Empty;
                                            string sDate = MyGridView.Columns[k].ColumnName;
                                            sValue = dtRow[k].ToString();
                                            if (sValue != string.Empty)
                                            {
                                                if (sValue.Contains('>') == false && sValue.Contains('<') == false)
                                                {
                                                    ilstDateAndValue.Add(sDate + "|" + sValue.Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0]);
                                                    valueList.Add(sValue.Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0]);
                                                }
                                            }
                                        }

                                    }
                                    else if (SelectedItems[1].ToString() == dtRow[0].ToString())
                                    {
                                        sCategory1 = dtRow[0].ToString();

                                        for (int i = 1; i < MyGridView.Rows.Count; i++)
                                        {
                                            if (sUnit1 == "")
                                            {
                                                sUnit1 = dtRow[i].ToString();
                                            }
                                        }
                                        for (int j = 1; j < MyGridView.Columns.Count; j++)
                                        {
                                            string sValue = string.Empty;
                                            string sDate = MyGridView.Columns[j].ColumnName;
                                            if (dtRow[j].ToString() != string.Empty && dtRow[j].ToString().Contains('>') == false && dtRow[j].ToString().Contains('<') == false)
                                            {
                                                ilstDateTime.Add(Convert.ToDateTime(sDate));

                                                dateList.Add(sDate);
                                            }
                                        }
                                        for (int k = 1; k < MyGridView.Columns.Count; k++)
                                        {
                                            string sValue = string.Empty;
                                            string sDate = MyGridView.Columns[k].ColumnName;
                                            sValue = dtRow[k].ToString();
                                            if (sValue != string.Empty)
                                            {
                                                if (sValue.Contains('>') == false && sValue.Contains('<') == false)
                                                {
                                                    ilstDateAndValue1.Add(sDate + "|" + sValue.Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0]);
                                                    valueList.Add(sValue.Split(SplitCharater, StringSplitOptions.RemoveEmptyEntries)[0]);
                                                }
                                            }
                                        }
                                    }
                                }
                                //Changed by vaishali on 05-01-2015 for bug ID 36852
                                ilstDateTime = ilstDateTime.Distinct().OrderBy(a => a.Date).ThenBy(n => n.TimeOfDay).ToList<DateTime>();

                                DataTable tbl = new DataTable();
                                tbl.TableName = sCategory + " and " + sCategory1;
                                DataColumn col = new DataColumn("Value");
                                col = new DataColumn("Date1");
                                col.DataType = typeof(string);
                                tbl.Columns.Add(col);

                                col = new DataColumn(sCategory);
                                col.DataType = typeof(double);
                                tbl.Columns.Add(col);

                                col = new DataColumn(sCategory1);
                                col.DataType = typeof(double);
                                tbl.Columns.Add(col);
                                int IsBp = 0;
                                if (sCategory1.Contains("BP") && sCategory.Contains("BP"))
                                {
                                    col = new DataColumn(sCategory.Split(' ')[0] + "(Dia)");
                                    col.DataType = typeof(double);
                                    tbl.Columns.Add(col);

                                    col = new DataColumn(sCategory1.Split(' ')[0] + "(Dia)");
                                    col.DataType = typeof(double);
                                    tbl.Columns.Add(col);

                                    IsBp = 2;
                                    tbl.Columns[2].ColumnName = sCategory1.Split(' ')[0] + "(Sys)";
                                    tbl.Columns[1].ColumnName = sCategory.Split(' ')[0] + "(Sys)";
                                }
                                else if (sCategory1.Contains("BP") || sCategory.Contains("BP"))
                                {
                                    string ColumName = string.Empty;
                                    if (sCategory1.Contains("BP"))
                                    {
                                        ColumName = sCategory1.Split(' ')[0] + "(Dia)";
                                    }
                                    else
                                    {
                                        ColumName = sCategory.Split(' ')[0] + "(Dia)";

                                    }
                                    col = new DataColumn(ColumName);
                                    col.DataType = typeof(double);
                                    tbl.Columns.Add(col);
                                    IsBp = 1;
                                    if (sCategory1.Contains("BP"))
                                    {
                                        tbl.Columns[2].ColumnName = sCategory1.Split(' ')[0] + "(Sys)";
                                    }
                                    else
                                        tbl.Columns[1].ColumnName = sCategory.Split(' ')[0] + "(Sys)";
                                }
                                foreach (DateTime dt in ilstDateTime)
                                {
                                    object obj1;
                                    object obj2;
                                    object obj3 = null;
                                    object obj4 = null;
                                    if (ilstDateAndValue.Any(a => dt == Convert.ToDateTime(a.Split('|')[0])))
                                    {
                                        string sTemp = ilstDateAndValue.Where(a => dt == Convert.ToDateTime(a.Split('|')[0])).Select(a => a.Split('|')[1]).SingleOrDefault();
                                        if (sTemp.Contains('\''))
                                        {
                                            string[] sHeight = sTemp.Split('\'');
                                            obj1 = Math.Round(Convert.ToDouble(sHeight[1]) / 12 + Convert.ToDouble(sHeight[0]), 2);
                                        }
                                        else if (sTemp.Contains('/'))
                                        {
                                            string[] Bps = sTemp.Split('/');
                                            obj1 = Convert.ToDouble(Bps[0]);
                                            obj3 = Convert.ToDouble(Bps[1]);

                                        }
                                        else
                                            obj1 = Convert.ToDouble(sTemp);
                                    }
                                    else
                                    {
                                        if (sCategory == "BP")
                                            obj3 = null;
                                        obj1 = null;


                                    }
                                    if (ilstDateAndValue1.Any(a => dt == Convert.ToDateTime(a.Split('|')[0])))
                                    {
                                        string sTemp = ilstDateAndValue1.Where(a => dt == Convert.ToDateTime(a.Split('|')[0])).Select(a => a.Split('|')[1]).SingleOrDefault();
                                        if (sTemp.Contains('\''))
                                        {
                                            string[] sHeight = sTemp.Split('\'');
                                            obj2 = Math.Round(Convert.ToDouble(sHeight[1]) / 12 + Convert.ToDouble(sHeight[0]), 2);
                                        }
                                        else if (sTemp.Contains('/'))
                                        {
                                            string[] Bps = sTemp.Split('/');
                                            obj2 = Convert.ToDouble(Bps[0]);
                                            obj4 = Convert.ToDouble(Bps[1]);

                                        }
                                        else
                                            obj2 = Convert.ToDouble(sTemp);
                                    }
                                    else
                                    {
                                        if (sCategory1 == "BP")
                                            obj4 = null;
                                        obj2 = null;
                                    }
                                    if (IsBp == 0)
                                        tbl.Rows.Add(new object[] { dt.ToString("dd-MMM-yyyy hh:mm tt"), obj1, obj2 });
                                    else if (IsBp == 2)
                                        tbl.Rows.Add(new object[] { dt.ToString("dd-MMM-yyyy hh:mm tt"), obj1, obj2, obj3, obj4 });
                                    else if (IsBp == 1)
                                    {
                                        if (tbl.Columns[1].ColumnName.Contains("(Sys)"))
                                            tbl.Rows.Add(new object[] { dt.ToString("dd-MMM-yyyy hh:mm tt"), obj1, obj2, obj3 });
                                        if (tbl.Columns[2].ColumnName.Contains("(Sys)"))
                                            tbl.Rows.Add(new object[] { dt.ToString("dd-MMM-yyyy hh:mm tt"), obj1, obj2, obj4 });
                                    }
                                }
                                chrtFlowSheet.Clear();
                                chrtFlowSheet.PlotArea.XAxis.Appearance.ValueFormat = Telerik.Charting.Styles.ChartValueFormat.ShortDate;
                                chrtFlowSheet.PlotArea.XAxis.DataLabelsColumn = "OADate";
                                chrtFlowSheet.PlotArea.XAxis.DataLabelsColumn = "OADate";
                                chrtFlowSheet.PlotArea.XAxis.LabelStep = 1;
                                chrtFlowSheet.PlotArea.XAxis.MinValue = 1D;
                                chrtFlowSheet.PlotArea.XAxis.MaxValue = 100D;
                                chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = AutoTextWrap.True;
                                chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color = System.Drawing.Color.Black;
                                chrtFlowSheet.AutoLayout = true;

                                chrtFlowSheet.DefaultType = ChartSeriesType.Line;

                                chrtFlowSheet.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = AutoTextWrap.True;

                                this.chrtFlowSheet.DataSource = tbl;
                                this.chrtFlowSheet.DataBind();

                                if (tbl.Columns.Count > 4)
                                {
                                    this.chrtFlowSheet.Series[3].Appearance.LineSeriesAppearance.Color = Color.IndianRed;
                                }
                                chrtFlowSheet.ChartTitle.TextBlock.Text = sCategory + " and " + sCategory1;
                                chrtFlowSheet.ChartTitle.TextBlock.Appearance.AutoTextWrap = AutoTextWrap.True;

                                if (IsBp == 1)
                                {
                                    if (tbl.Columns[1].ColumnName.Contains("(Sys)"))
                                        this.chrtFlowSheet.Series[2].YAxisType = ChartYAxisType.Primary;
                                    else
                                        this.chrtFlowSheet.Series[2].YAxisType = ChartYAxisType.Secondary;
                                }
                                else if (IsBp == 2)
                                {
                                    this.chrtFlowSheet.Series[2].YAxisType = ChartYAxisType.Primary;
                                    this.chrtFlowSheet.Series[3].YAxisType = ChartYAxisType.Secondary;
                                }

                                this.chrtFlowSheet.Series[0].PlotArea.YAxis.MinValue = 0;
                                double maxInchandheight = 0;
                                if (tbl.Columns.Count > 3)
                                {
                                    if (tbl.Columns[1].ColumnName.Contains("(Sys)"))
                                    {
                                        maxInchandheight = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[1] != DBNull.Value).Max(a => a[1])), 2));
                                        double maxInchandheighttemp = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[3] != DBNull.Value).Max(a => a[3])), 2));
                                        if (maxInchandheighttemp > maxInchandheight)
                                            maxInchandheight = maxInchandheighttemp;
                                    }
                                    else
                                    {
                                        maxInchandheight = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[1] != DBNull.Value).Max(a => a[1])), 2));
                                    }
                                }
                                else
                                {
                                    maxInchandheight = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[1] != DBNull.Value).Max(a => a[1])), 2));
                                }
                                double YAxisstepValue = Convert.ToDouble(maxInchandheight - this.chrtFlowSheet.Series[0].PlotArea.YAxis.MinValue) / 10;
                                this.chrtFlowSheet.Series[0].PlotArea.YAxis.Step = YAxisstepValue != 0 ? YAxisstepValue : 1;
                                this.chrtFlowSheet.Series[0].PlotArea.YAxis.MaxValue = maxInchandheight + this.chrtFlowSheet.Series[0].PlotArea.YAxis.Step;
                                this.chrtFlowSheet.Series[1].YAxisType = ChartYAxisType.Secondary;

                                this.chrtFlowSheet.Series[1].PlotArea.YAxis.MinValue = 0;
                                double maxInchandheight1 = 0;
                                if (tbl.Columns.Count > 3)
                                {
                                    if (tbl.Columns.Count == 4 && tbl.Columns[2].ColumnName.Contains("(Sys)"))
                                    {
                                        maxInchandheight1 = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[2] != DBNull.Value).Max(a => a[2])), 2));
                                        double maxInchandheight1temp = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[3] != DBNull.Value).Max(a => a[3])), 2));
                                        if (maxInchandheight1temp > maxInchandheight1)
                                            maxInchandheight1 = maxInchandheight1temp;
                                    }
                                    else if (tbl.Columns.Count == 5 && tbl.Columns[2].ColumnName.Contains("(Sys)"))
                                    {
                                        maxInchandheight1 = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[2] != DBNull.Value).Max(a => a[2])), 2));
                                        double maxInchandheight1temp = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[4] != DBNull.Value).Max(a => a[4])), 2));
                                        if (maxInchandheight1temp > maxInchandheight1)
                                            maxInchandheight1 = maxInchandheight1temp;
                                    }
                                    else
                                    {
                                        maxInchandheight1 = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[2] != DBNull.Value).Max(a => a[2])), 2));
                                    }
                                }
                                else
                                {
                                    maxInchandheight1 = (Math.Round(Convert.ToDouble(tbl.Rows.Cast<DataRow>().Where(a => a[2] != DBNull.Value).Max(a => a[2])), 2));
                                }

                                if (sCategory.Length > 100 || sCategory1.Length > 100)
                                {
                                    chrtFlowSheet.Legend.Appearance.Dimensions.AutoSize = false;
                                    chrtFlowSheet.Legend.Appearance.Dimensions.Width = 150f;
                                    chrtFlowSheet.Legend.Appearance.Dimensions.Height = 150f;
                                }

                                double YAxis2stepValue = Math.Round(Convert.ToDouble(maxInchandheight1 - this.chrtFlowSheet.Series[1].PlotArea.YAxis2.MinValue) / 10, MidpointRounding.AwayFromZero);
                                this.chrtFlowSheet.Series[1].PlotArea.YAxis2.Step = YAxis2stepValue != 0 ? YAxis2stepValue : 1;
                                this.chrtFlowSheet.Series[1].PlotArea.YAxis2.MaxValue = maxInchandheight1 + this.chrtFlowSheet.Series[1].PlotArea.YAxis2.Step;
                                this.chrtFlowSheet.Series[1].Name = sCategory1;
                                if (sUnit1.Trim() != string.Empty)
                                {
                                    if (sUnit1.Contains(" ") == true)
                                    {
                                        string[] strUnit = sUnit1.Split(' ');
                                        sUnit1 = string.Empty;
                                        for (int k = 1; k < strUnit.Length; k++)
                                        {
                                            if (sUnit1 == string.Empty)
                                                sUnit1 = strUnit[k].ToString();
                                            else
                                                sUnit1 = sUnit1 + strUnit[k].ToString();

                                        }
                                        if (sUnit1.Contains("FtInch") == true)
                                        {
                                            sUnit1 = "Ft";
                                        }
                                        this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.TextBlock.Text = sCategory1 + " (" + sUnit1 + ")";
                                        this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.TextBlock.Appearance.AutoTextWrap = AutoTextWrap.True;
                                        this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                                        this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.Visible = true;
                                    }

                                }
                                else
                                {
                                    this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.TextBlock.Text = sCategory1;
                                    this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.TextBlock.Appearance.AutoTextWrap = AutoTextWrap.True;
                                    this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                                    this.chrtFlowSheet.Series[1].PlotArea.YAxis2.AxisLabel.Visible = true;
                                }

                                if (sUnit.Trim() != string.Empty)
                                {
                                    if (sUnit.Contains(" ") == true)
                                    {
                                        string[] strUnit = sUnit.Split(' ');
                                        sUnit = string.Empty;
                                        if (strUnit.Length == 3)
                                        {
                                            for (int k = 1; k < strUnit.Length; k++)
                                            {
                                                if (sUnit == string.Empty)
                                                    sUnit = strUnit[k].ToString();
                                                else
                                                    sUnit = sUnit + strUnit[k].ToString();

                                            }
                                        }
                                        if (strUnit.Length == 5)
                                        {
                                            for (int k = 3; k < strUnit.Length; k++)
                                            {
                                                if (sUnit == string.Empty)
                                                    sUnit = strUnit[k].ToString();
                                                else
                                                    sUnit = sUnit + strUnit[k].ToString();

                                            }
                                        }
                                        if (sUnit.Contains("FtInch") == true)
                                        {
                                            sUnit = "Ft";
                                            this.chrtFlowSheet.Series[0].PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + " (" + sUnit + ")";
                                            this.chrtFlowSheet.Series[0].PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                                        }
                                        else
                                        {
                                            this.chrtFlowSheet.Series[0].PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory + " (" + sUnit + ")";
                                            this.chrtFlowSheet.Series[0].PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                                        }

                                    }
                                }
                                else
                                {
                                    this.chrtFlowSheet.Series[0].PlotArea.YAxis.AxisLabel.TextBlock.Text = sCategory;
                                    this.chrtFlowSheet.Series[0].PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color = System.Drawing.Color.Black;
                                }

                                if (ilstDateTime.Count == 1)
                                {
                                    if (tbl.Rows.Cast<DataRow>().Where(a => a[1] != DBNull.Value).Count() == 1)
                                    {
                                        this.chrtFlowSheet.Series[0].Type = ChartSeriesType.Point;

                                    }
                                    if (tbl.Rows.Cast<DataRow>().Where(a => a[2] != DBNull.Value).Count() == 1)
                                        this.chrtFlowSheet.Series[1].Type = ChartSeriesType.Point;
                                    if (tbl.Columns.Count > 3)
                                    {
                                        if (tbl.Rows.Cast<DataRow>().Where(a => a[3] != DBNull.Value).Count() == 1)
                                            this.chrtFlowSheet.Series[2].Type = ChartSeriesType.Point;
                                    }
                                    if (tbl.Columns.Count > 4)
                                    {
                                        if (tbl.Rows.Cast<DataRow>().Where(a => a[4] != DBNull.Value).Count() == 1)
                                            this.chrtFlowSheet.Series[3].Type = ChartSeriesType.Point;
                                    }
                                }
                            }
                        }
                        else
                        {
                            chrtFlowSheet.Clear();
                            chrtFlowSheet.ChartTitle.TextBlock.Text = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException)
                        {
                            // ApplicationObject.erroHandler.DisplayErrorMessage("102011", this.Text);
                            chrtFlowSheet.Clear();

                            chrtFlowSheet.ChartTitle.TextBlock.Text = string.Empty;
                        }
                    }
                    #endregion
                }
                else
                {

                    chrtFlowSheet.Clear();
                    chrtFlowSheet.ChartTitle.TextBlock.Text = string.Empty;

                }
            }
        }


        protected void btnPrintChart_Click(object sender, EventArgs e)
        {
            string FileName = String.Format("/ChartImage.png", 0);

            RadChart RadChart1 = this.Page.FindControl("chrtFlowSheet") as RadChart;

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

                sFaxSubject = "Flow_Sheet" + sFaxLastName + sFaxFirstname + "_" + DateTime.Now.ToString("dd-MMM-yyyy");

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "OpenPDF('" + FileName + "','" + sFaxSubject + "');", true);
        }
    }
}
