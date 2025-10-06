using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.Core.DomainObjects;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Acurus.Capella.UI;
using Acurus.Capella.UI.UserControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Text.RegularExpressions;



namespace Acurus.Capella.UI
{
    public partial class frmHealthQuestionnaire : System.Web.UI.Page
    {
        IList<FillHealthcareQuestionnaire> healthQuestionScreenList;
        IList<FillHealthcareQuestionnaire> healthQuestionScreenList_CurrEnc;
        HealthcareQuestionnaireManager healthQuestionMngr = new HealthcareQuestionnaireManager();
        //Hashtable HashHealthQuestionnaireID = new Hashtable();
        //Hashtable HashVersion = new Hashtable();
        //Hashtable HashQuestionnaireLookupID = new Hashtable();
        DateTime PatientDOB = DateTime.MinValue;
        DateTime dtpAppointmentDate = DateTime.MinValue;
        public Boolean bIspostback = false;
        string sMyCategory = string.Empty;
        string strQuestionType = string.Empty;
        TextBox txtTotalScore = null;
        TextBox txtPercentage = null;
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    if (IsPostBack)
        //    {
        //        if (Session["HealthQuestionnaireTable"] != null)
        //        {
        //            Table testTable = (Table)Session["HealthQuestionnaireTable"];
        //            try
        //            {
        //                divHealthQuestionnaire.Controls.Add(testTable);

        //            }
        //            catch
        //            {

        //            }
        //        }
        //    }

        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientSession.FlushSession();
                DateTime PatientDOB = DateTime.MinValue;
                DateTime dtpAppointmentDate = DateTime.MinValue;
                btnSave.Disabled = true;

                decimal ageInDays;
                QuestionnaireLookupManager questionnaireMngr = new QuestionnaireLookupManager();
                if (Request["TabName"] != null)
                    sMyCategory = Request["TabName"].ToString();

                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                {
                    IList<PatientPane> PatientPaneList = ClientSession.PatientPaneList.Where(a => a.Encounter_ID == ClientSession.EncounterId).ToList<PatientPane>();
                    if (PatientPaneList.Count > 0)
                    {
                        dtpAppointmentDate = PatientPaneList[0].Date_of_Service;
                        PatientDOB = PatientPaneList[0].Birth_Date;
                    }
                }
                //CAP-3628
                //healthQuestionScreenList = questionnaireMngr.GetHealthQuestionLookupListFromServer(sMyCategory, PatientDOB, dtpAppointmentDate, ClientSession.PhysicianUserName, ClientSession.EncounterId, true);
                healthQuestionScreenList = questionnaireMngr.GetHealthQuestionLookupListFromServer(sMyCategory, PatientDOB, dtpAppointmentDate, ClientSession.PhysicianUserName, ClientSession.EncounterId, true, ClientSession.HumanId);
                Session["healthQuestionScreenList"] = healthQuestionScreenList;
                IList<StaticLookup> ilstStaticlookup = new List<StaticLookup>();
                StaticLookupManager objStaticLookupMngr = new StaticLookupManager();
                //ilstStaticlookup = objStaticLookupMngr.getStaticLookupByFieldName("QUESTIONNAIRE-PHQ-9");
                ilstStaticlookup = objStaticLookupMngr.getStaticLookupByFieldName("QUESTIONNAIRE SCORE DESCRIPTION-" + sMyCategory);
                for (int i = 0; i < ilstStaticlookup.Count; i++)
                {
                    if (i == 0)
                        hdnScorelevel.Value = ilstStaticlookup[i].Value + "$" + ilstStaticlookup[i].Description;
                    else
                        hdnScorelevel.Value += "|" + ilstStaticlookup[i].Value + "$" + ilstStaticlookup[i].Description;
                }
                IList<StaticLookup> ilstStaticlookupPercentage = new List<StaticLookup>();
                ilstStaticlookupPercentage = objStaticLookupMngr.getStaticLookupByFieldName("QUESTIONNAIRE PERCENTAGE DESCRIPTION-" + sMyCategory);
                for (int i = 0; i < ilstStaticlookupPercentage.Count; i++)
                {
                    if (i == 0)
                        hdnPercentageLevel.Value = ilstStaticlookupPercentage[i].Value + "$" + ilstStaticlookupPercentage[i].Description;
                    else
                        hdnPercentageLevel.Value += "|" + ilstStaticlookupPercentage[i].Value + "$" + ilstStaticlookupPercentage[i].Description;
                }
            }
            //if (Request.Form["__EVENTTARGET"] == "btnCopyPrevious" || HdnCopyButton.Value == "CheckSave")
            //{

            //    CopyPreviousEncounter(false);
            //    CopyPreviousEncounter(false);

            //}
            //else
            //{
            //    if (HdnCopyButton.Value != "")
            //    {
            if (Session["healthQuestionScreenList"] != null)
                healthQuestionScreenList = (IList<FillHealthcareQuestionnaire>)Session["healthQuestionScreenList"];

            if (healthQuestionScreenList != null && healthQuestionScreenList.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "DisplayErrorMessage('118504');", true);
                return;
            }


            if (!IsPostBack)
            {
                bIspostback = false;
                FillMyQuestionList(healthQuestionScreenList);
            }
            else
            {
                bIspostback = true;
                FillMyQuestionList(healthQuestionScreenList);

                //if (Session["Version"] != null)
                //    HashVersion = (Hashtable)Session["Version"];
                //if (Session["HealthQuestionnaireID"] != null)
                //    HashHealthQuestionnaireID = (Hashtable)Session["HealthQuestionnaireID"];
                //if (Session["QuestionnaireLookupID"] != null)
                //    HashQuestionnaireLookupID = (Hashtable)Session["QuestionnaireLookupID"];
            }
            //}

            //}
            if (!IsPostBack)
            {
                ClientSession.processCheck = true;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                btnSave.Disabled = true;
            }
        }


        private void FillMyQuestionList(IList<FillHealthcareQuestionnaire> healthQuestionScreenList)
        {
            if (healthQuestionScreenList == null)
            {
                return;
            }

            FillMyEncounter(healthQuestionScreenList);
        }



        private void FillMyEncounter(IList<FillHealthcareQuestionnaire> healthQuestionScreenList)
        {
            divHealthQuestionnaire.Controls.Clear();
            Table tbldynamic = new Table();
            string sOldSystem = string.Empty;
            string sNewQuestionType = string.Empty;
            IList<string> straryOption = null;
            Panel plhRadioButton = new Panel();
            tbldynamic.BackColor = Color.White;
            Panel plhQuestion = new Panel();
            if (healthQuestionScreenList != null)
            {
                var sOptionLength = (from p in healthQuestionScreenList where p.Possible_Options.ToString() != "" select new { p.Possible_Options }).ToList();
                IList<string> sCheck = new List<string>();
                for (int i = 0; i < sOptionLength.Count(); i++)
                {
                    var sValue = sOptionLength[i].Possible_Options.Split('|').Where(a => a.Trim() != "").ToList<string>();
                    if (sValue.Count > 0)
                    {
                        for (int j = 0; j < sValue.Count; j++)
                        {
                            sCheck.Add(sValue[j].ToString());
                        }
                    }
                }

                var sLength = sCheck.OrderByDescending(s => s.Length).First();

                for (int i = 0; i < healthQuestionScreenList.Count; i++)
                {
                    if (healthQuestionScreenList[i].Question.Contains('"') || healthQuestionScreenList[i].Question.Contains("'"))
                    {
                        healthQuestionScreenList[i].Question = healthQuestionScreenList[i].Question.Replace('"', ' ').Replace("'", "");
                    }
                    if (healthQuestionScreenList[i].Is_Notes.ToUpper() != "N")
                    {
                        tdNotes.Style.Add("display", "normal");
                        tdOption.Style.Add("align", "center");
                        tdQuestion.Style.Add("align", "center");
                    }
                    else
                    {
                        tdOption.Align = "left";
                        tdQuestion.Align = "left";
                    }

                    TableCell tc = new TableCell();
                    TableRow tr1 = new TableRow();
                    TableRow tr3 = new TableRow();
                    TableRow row = new TableRow();

                    sNewQuestionType = healthQuestionScreenList[i].Questionnaire_Type;
                    HtmlInputText lbl = new HtmlInputText();
                    if (sOldSystem != sNewQuestionType)
                    {
                        tc = new TableCell();
                        //lbl.Width = 980;
                        tc.ColumnSpan = 3;
                        lbl.Value = healthQuestionScreenList[i].Questionnaire_Type;

                        lbl.EnableViewState = false;
                        //lbl.Style.Add("font-size", "small");
                        //lbl.Style.Add("font-weight", "bold");
                        //lbl.Style.Add("cssclass", "LabelStyleBold");
                        lbl.Attributes.Add("class", "Headergroupstyle");
                        //lbl.Style.Add("color", "Black");
                        //lbl.Style.Add("color", "white !important");
                        //lbl.Style.Add("background-color", "#BFDBFF");
                        //lbl.Style.Add("background-color", "#3aa04a !important");
                        //lbl.Style.Add("font-family", "Serif");
                        lbl.Style.Add("Width", "1188px");
                        //lbl.Style.Add("border-width", "1px");
                        //lbl.Style.Add("border-color", "black");
                        lbl.Attributes.Add("readonly", "readonly");
                        //lbl.EnableViewState = false;
                        //lbl.ForeColor = Color.Black;
                        //lbl.BackColor = Color.FromArgb(191, 219, 255);
                        //lbl.Font.Size = new FontUnit("8.5pt");
                        //lbl.Height = 19;
                        //lbl.BorderWidth = 1;
                        //lbl.Font.Bold = true;
                        tc.Controls.Add(lbl);
                        row.Cells.Add(tc);
                    }
                    plhQuestion = new Panel();
                    HtmlGenericControl lblquestion = new HtmlGenericControl();
                    tc = new TableCell();
                    lblquestion.ID = "lbl" + healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString();
                    if (healthQuestionScreenList[i].Question.StartsWith("       "))
                    {
                        string strTest = healthQuestionScreenList[i].Question;
                        string strresult = "";
                        for (int k = 0; k < strTest.ToCharArray().Length; k++)
                        {
                            if (strTest[k] == ' ')
                                strresult += "&nbsp";
                            else
                                break;
                        }
                        lblquestion.InnerHtml = strresult + healthQuestionScreenList[i].Question;



                    }
                    else
                        lblquestion.InnerHtml = healthQuestionScreenList[i].Question;
                    lblquestion.EnableViewState = false;
                    //lblquestion.Style.Add("font-size", "small");
                    lblquestion.Attributes.Add("class", "Editabletxtbox");
                    //lblquestion.Style.Add("color", "Black");
                    //lblquestion.Style.Add("background-color", "White");
                    //lblquestion.Style.Add("font-family", "Serif");
                    if (healthQuestionScreenList[i].Questionnaire_Category == "ADL Screening")
                        lblquestion.Style.Add("Width", "200px");
                    else if (healthQuestionScreenList[i].Questionnaire_Category == "Lawton Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Oswestry Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Neck Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Spine Intake")
                        lblquestion.Style.Add("Width", "250px");
                    else
                        lblquestion.Style.Add("Width", "270px");
                    lblquestion.Style.Add("wrap", "hard");
                    lblquestion.Style.Add("overflow", "hidden");
                    lblquestion.Style.Add("resize", "none");
                    lblquestion.Style.Add("ReadOnly", "true");
                    //lblquestion.Font.Size = new FontUnit("8.5pt");
                    //lblquestion.ForeColor = Color.Black;
                    //lblquestion.Width = 355;

                    if (healthQuestionScreenList[i].Is_Notes.ToUpper() == "N")
                    {
                        plhQuestion.Width = 230;
                        plhQuestion.Height = 30;
                        plhQuestion.Controls.Add(lblquestion);
                        tc.Controls.Add(plhQuestion);
                        tc.Style.Add(HtmlTextWriterStyle.Width, "230px");
                        tc.Style.Add(HtmlTextWriterStyle.Height, "30px");
                    }
                    else
                    {
                        if (healthQuestionScreenList[i].Question.ToUpper() == "TOTAL SCORE" || healthQuestionScreenList[i].Question.ToUpper() == "PERCENTAGE(%)")
                        {
                        }
                        else
                            lblquestion.Attributes.Add("style", "padding: 0px 0px 20px");
                        tc.Controls.Add(lblquestion);
                        if (healthQuestionScreenList[i].Questionnaire_Category == "ADL Screening")
                            tc.Style.Add(HtmlTextWriterStyle.Width, "415px");
                        else if (healthQuestionScreenList[i].Questionnaire_Category == "Lawton Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Katz Index Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Oswestry Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Neck Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Spine Intake")
                            tc.Style.Add(HtmlTextWriterStyle.Width, "355px");
                        else
                            tc.Style.Add(HtmlTextWriterStyle.Width, "485px");
                    }
                    tr1.Cells.Add(tc);

                    //if (HashVersion.Contains(lblquestion.ID.Replace("lbl", "")) == false)
                    //{
                    //    HashVersion.Add(lblquestion.ID.Replace("lbl", ""), healthQuestionScreenList[i].Version);
                    //    HashHealthQuestionnaireID.Add(lblquestion.ID.Replace("lbl", ""), healthQuestionScreenList[i].HealthCare_Questionnaire_ID);
                    //    HashQuestionnaireLookupID.Add(lblquestion.ID.Replace("lbl", ""), healthQuestionScreenList[i].Questionnaire_Lookup_ID);
                    //}
                    straryOption = healthQuestionScreenList[i].Possible_Options.Split('|').Where(a => a.Trim() != "").ToList<string>();



                    tc = new TableCell();
                    plhRadioButton = new Panel();
                    plhRadioButton.ID = "pln" + healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString();

                    if (healthQuestionScreenList[i].Is_Notes.ToUpper() != "N")
                    {
                        if (healthQuestionScreenList[i].Questionnaire_Category == "ADL Screening")
                        {
                            plhRadioButton.Width = 430;
                            tc.Controls.Add(plhRadioButton);
                            tc.Style.Add(HtmlTextWriterStyle.Width, "430px");
                        }
                        else if (healthQuestionScreenList[i].Questionnaire_Category == "Lawton Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Katz Index Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Oswestry Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Neck Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Spine Intake")
                        {
                            plhRadioButton.Width = 510;
                            if (healthQuestionScreenList[i].Question.ToUpper() == "TOTAL SCORE" || healthQuestionScreenList[i].Question.ToUpper() == "PERCENTAGE(%)")
                            {
                            }
                            else
                                plhRadioButton.Attributes.Add("style", "padding: 0px 0px 20px");
                            tc.Controls.Add(plhRadioButton);
                            tc.Style.Add(HtmlTextWriterStyle.Width, "510px");
                        }
                        else
                        {
                            plhRadioButton.Width = 360;
                            tc.Controls.Add(plhRadioButton);
                            tc.Style.Add(HtmlTextWriterStyle.Width, "360px");
                        }
                    }
                    else
                    {
                        plhRadioButton.Width = 720;
                        plhRadioButton.Height = 40;
                        tc.Controls.Add(plhRadioButton);
                        tc.Style.Add(HtmlTextWriterStyle.Width, "720px");
                        tc.Style.Add(HtmlTextWriterStyle.Height, "40px");
                    }
                    //plhRadioButton.Width = 360;
                    //tc.Controls.Add(plhRadioButton);
                    //tc.Style.Add(HtmlTextWriterStyle.Width, "360px");

                    tr1.Cells.Add(tc);


                    //if (chkQuestion.Visible == false)
                    //{
                    //    foreach (string strOpt in straryOption)
                    //    {
                    //        if (strOpt == "No")
                    //        {
                    //            chkQuestion.Visible = true;
                    //        }
                    //    }
                    //}

                    //string maxlengthRecord = straryOption.OrderByDescending(s => s.Length).First();

                    Table dt = new Table();
                    TableRow rowRadioButton = new TableRow();
                    Boolean btrue = false;
                    HtmlSelect rowComboBox = null;
                    TextBox rowTextBox = null;
                    string id = healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString();
                    id = Regex.Replace(id, @"[^a-zA-Z0-9]", "_");
                    switch (healthQuestionScreenList[i].Controls.ToUpper())
                    {
                        case "RADIOBUTTON":
                            if (straryOption.Count > 0)
                            {
                                for (int j = 0; j < straryOption.Count; j++)
                                {
                                    int icount = j % 2;
                                    if (healthQuestionScreenList[i].Questionnaire_Category == "Lawton Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Katz Index Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Oswestry Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Neck Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Spine Intake")
                                    {
                                        if (icount == 0)
                                        {
                                            rowRadioButton = new TableRow();
                                            tc = new TableCell();
                                        }
                                    }


                                    RadioButton rdbtn = new RadioButton();
                                    rdbtn.Attributes.Add("class", "Editabletxtbox");
                                    //if (maxlengthRecord.Length > 15)
                                    //    rdbtn.Width = 180;
                                    //else if (maxlengthRecord.Length < 16)
                                    //{
                                    //    rdbtn.Width = 100;
                                    //    plhRadioButton.Width = 255;
                                    //    plhRadioButton.CssClass = "AlignPanelCenter";
                                    //}

                                    if (healthQuestionScreenList[i].Is_Notes.ToUpper() != "N")
                                    {
                                        if (healthQuestionScreenList[i].Questionnaire_Category == "ADL Screening")
                                        {
                                            rdbtn.Width = 215;
                                        }
                                        else if (healthQuestionScreenList[i].Questionnaire_Category == "Lawton Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Katz Index Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Oswestry Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Neck Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Spine Intake")
                                        {

                                            if (icount == 0)
                                            {
                                                rdbtn.Width = 265;
                                                rdbtn.Style.Add("float", "left");
                                            }
                                            else
                                            {
                                                rdbtn.Width = 185;
                                                rdbtn.Style.Add("float", "right");
                                            }

                                        }
                                        else
                                        {
                                            if (sLength.Length > 15)
                                                rdbtn.Width = 180;
                                            else if (sLength.Length < 16)
                                            {
                                                rdbtn.Width = 100;
                                                plhRadioButton.Width = 255;
                                                plhRadioButton.CssClass = "AlignPanelCenter";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (j == straryOption.Count - 1)
                                            rdbtn.Style.Add("width", "100%");//.Width = 250;
                                        else
                                            rdbtn.Width = 50;
                                        plhRadioButton.Width = 720;
                                        plhRadioButton.CssClass = "AlignPanelCenter";
                                    }
                                    //if (icount == 0)
                                    //{
                                    //    rdbtn.Style.Add("float", "left");
                                    //}
                                    //else
                                    //{
                                    //    rdbtn.Style.Add("float", "right");
                                    //}
                                    rdbtn.EnableViewState = false;
                                    rdbtn.ID = "rdbtn" + " " + straryOption[j].Trim() + " " + healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString();
                                    rdbtn.Text = straryOption[j];
                                    rdbtn.Font.Size = new FontUnit("8.5pt");
                                    if (healthQuestionScreenList[i].Is_Notes.ToUpper() != "N")
                                    {
                                        rdbtn.CssClass = "radioWithProperWrap";
                                    }

                                    rdbtn.Attributes.Add("onClick", "EnableSave(event);");
                                    rdbtn.Attributes.Add("onChange", "Questionnairechanged();");

                                    if (healthQuestionScreenList[i].Selected_Option.Trim() == rdbtn.Text.Trim())
                                    {
                                        rdbtn.Checked = true;
                                    }
                                    rdbtn.GroupName = "question" + i.ToString();
                                    //plhRadioButton.Controls.Add(rdbtn);
                                    if (healthQuestionScreenList[i].Questionnaire_Category == "Lawton Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Katz Index Screening" || healthQuestionScreenList[i].Questionnaire_Category == "Oswestry Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Neck Disability Index" || healthQuestionScreenList[i].Questionnaire_Category == "Spine Intake")
                                    {
                                        tc.Controls.Add(rdbtn);
                                        rowRadioButton.Cells.Add(tc);
                                        if (icount == 1)
                                            dt.Controls.Add(rowRadioButton);
                                        else if (j == straryOption.Count - 1)
                                            dt.Controls.Add(rowRadioButton);
                                        btrue = true;
                                    }
                                    else
                                        plhRadioButton.Controls.Add(rdbtn);

                                }
                            }
                            else
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    tc = new TableCell();
                                    RadioButton rdbtn1 = new RadioButton();
                                    rdbtn1.EnableViewState = false;
                                    rdbtn1.Visible = false;
                                    plhRadioButton.Controls.Add(rdbtn1);
                                }
                            }
                            break;
                        case "COMBOBOX":

                            rowComboBox = new HtmlSelect();
                            rowComboBox.ID = "cbo_" + id;
                            rowComboBox.Style.Add("Width", 200 + "px");
                            rowComboBox.Style.Add("font-family", "Serif");
                            rowComboBox.Style.Add("font-size", "small");
                            rowComboBox.Items.Clear();
                            rowComboBox.Items.Add(new ListItem(""));
                            for (int j = 0; j < straryOption.Count; j++)
                            {
                                ListItem tempItem = new ListItem();
                                if (healthQuestionScreenList[i].Selected_Option.Trim() == straryOption[j].Trim())
                                {
                                    tempItem.Selected = true;
                                }
                                tempItem.Text = straryOption[j];
                                rowComboBox.Items.Add(tempItem);
                            }
                            rowComboBox.Attributes.Add("onkeypress", "EnableSave(event);");
                            rowComboBox.Attributes.Add("onchange", "EnableSave(event);");
                            rowComboBox.Attributes.Add("class", "combo SelectStyle spanstyle");
                            tc.Controls.Add(new LiteralControl("&nbsp;"));
                            tc.Controls.Add(rowComboBox);
                            rowComboBox.Dispose();

                            break;
                        case "READONLYTEXTBOX":

                            tc = new TableCell();
                            rowTextBox = new TextBox();
                            rowTextBox.ID = "txt_" + id;
                            rowTextBox.EnableViewState = false;
                            rowTextBox.Width = 200;
                            rowTextBox.Text = healthQuestionScreenList[i].Where_Criteria;
                            rowTextBox.ToolTip = healthQuestionScreenList[i].Where_Criteria;
                            rowTextBox.ReadOnly = true;
                            rowTextBox.Attributes.Add("class", "nonEditabletxtbox");
                            plhRadioButton.Controls.Add(rowTextBox);

                            break;
                        case "MULTISELECTTEXTBOX":

                            HtmlTextArea txtarea = new HtmlTextArea();
                            txtarea.ID = "txtArea_" + id;
                            txtarea.Style.Add(HtmlTextWriterStyle.Width, 200 + "px");
                            txtarea.Style.Add(HtmlTextWriterStyle.Height, "30px");
                            txtarea.Attributes.Add("height", "75px");
                            txtarea.Style.Add("resize", "none");
                            txtarea.Value = healthQuestionScreenList[i].Selected_Option;
                            plhRadioButton.Controls.Add(txtarea);
                            txtarea.Dispose();

                            ImageButton imageButton = new ImageButton();
                            imageButton.ID = "img_" + id;
                            imageButton.ImageUrl = "~/Resources/Dropdownimg.jpg";
                            imageButton.Style.Add("margin-bottom", "10px");
                            imageButton.Style.Add("margin-left", "5px");
                            imageButton.OnClientClick = $"return ClickMultiSelect('{id}');";
                            imageButton.Attributes.Add("isOpen", "false");
                            imageButton.CausesValidation = false;
                            plhRadioButton.Controls.Add(imageButton);

                            ListBox rowListBox = new ListBox();
                            rowListBox.ID = "lstBox_" + id;
                            rowListBox.EnableViewState = false;
                            rowListBox.Width = 200;
                            rowListBox.SelectionMode = ListSelectionMode.Multiple;
                            rowListBox.Style.Add("display", "none");
                            rowListBox.Attributes.Add("onclick", $"listMultiSelectChange(this, '{id}');");
                            rowListBox.CssClass = "multiSelectBox";
                            rowListBox.Items.Add(new ListItem("[Empty]"));
                            for (int j = 0; j < straryOption.Count; j++)
                            {
                                ListItem tempItem = new ListItem();
                                if (!string.IsNullOrEmpty(healthQuestionScreenList[i].Selected_Option)
                                    && healthQuestionScreenList[i].Selected_Option.Split(',').Any(a => a == straryOption[j]))
                                {
                                    tempItem.Selected = true;
                                }
                                tempItem.Text = straryOption[j];
                                rowListBox.Items.Add(tempItem);
                            }
                            plhRadioButton.Controls.Add(rowListBox);

                            break;
                    }
                    if (btrue)
                        plhRadioButton.Controls.Add(dt);

                    if (healthQuestionScreenList[i].Question.ToUpper() == "TOTAL SCORE")
                    {
                        tc = new TableCell();
                        txtTotalScore = new TextBox();
                        txtTotalScore.ID = "txtTotalScore";
                        txtTotalScore.EnableViewState = false;
                        txtTotalScore.Width = 200;
                        //txtTotalScore.Height = 200;
                        if (healthQuestionScreenList[i].Selected_Option != "")
                        {
                            if (hdnTotalScore.Value == string.Empty)
                                hdnTotalScore.Value = healthQuestionScreenList[i].Selected_Option;
                            txtTotalScore.Text = healthQuestionScreenList[i].Selected_Option;
                        }
                        else
                            if (hdnTotalScore.Value != string.Empty)
                            txtTotalScore.Text = hdnTotalScore.Value;
                        else
                            txtTotalScore.Text = string.Empty;
                        txtTotalScore.ReadOnly = true;
                        //txtTotalScore.Style.Add("background-color", "lightgray");
                        txtTotalScore.Attributes.Add("class", "nonEditabletxtbox");
                        if (healthQuestionScreenList[i].Is_Notes.ToUpper() == "N")
                            plhRadioButton.CssClass = "AlignPanelCenter";
                        plhRadioButton.Controls.Add(txtTotalScore);
                    }
                    if (healthQuestionScreenList[i].Question.ToUpper() == "PERCENTAGE(%)")
                    {
                        tc = new TableCell();
                        txtPercentage = new TextBox();
                        txtPercentage.ID = "txtPercentage";
                        txtPercentage.EnableViewState = false;
                        txtPercentage.Width = 200;
                        //txtTotalScore.Height = 200;
                        if (healthQuestionScreenList[i].Selected_Option != "")
                        {
                            if (hdnPercentage.Value == string.Empty)
                                hdnPercentage.Value = healthQuestionScreenList[i].Selected_Option;
                            txtPercentage.Text = healthQuestionScreenList[i].Selected_Option;
                        }
                        else
                            if (hdnPercentage.Value != string.Empty)
                            txtPercentage.Text = hdnPercentage.Value;
                        else
                            txtPercentage.Text = string.Empty;
                        txtPercentage.ReadOnly = true;
                        //txtPercentage.Style.Add("background-color", "lightgray");
                        txtPercentage.Attributes.Add("class", "nonEditabletxtbox");
                        if (healthQuestionScreenList[i].Is_Notes.ToUpper() == "N")
                            plhRadioButton.CssClass = "AlignPanelCenter";
                        plhRadioButton.Controls.Add(txtPercentage);
                    }
                    if (healthQuestionScreenList[i].Is_Notes.ToUpper() != "N")
                    {

                        CustomDLCNew userCtrl = (CustomDLCNew)LoadControl("~/UserControls/customDLCNew.ascx");
                        tc = new TableCell();
                        if (!bIspostback)
                        {
                            userCtrl.txtDLC.Text = healthQuestionScreenList[i].Notes;
                        }
                        //Cap - 971
                        //else //if (hdnTotalScoreDescription.Value != string.Empty)
                        //    userCtrl.txtDLC.Text = hdnTotalScoreDescription.Value;
                        else
                        {
                            if (healthQuestionScreenList[i].Question.ToUpper() == "TOTAL SCORE")
                            {
                                userCtrl.txtDLC.Text = hdnTotalScoreDescription.Value;
                            }
                            else
                            {
                                userCtrl.txtDLC.Text = healthQuestionScreenList[i].Notes;
                            }
                        }
                        //else
                        //    if (hdnTotalScore.Value == string.Empty || hdnTotalScore.Value=="0")
                        //        userCtrl.txtDLC.Text = hdnTotalScoreDescription.Value;
                        //    else
                        //        userCtrl.txtDLC.Text = healthQuestionScreenList[i].Notes;

                        userCtrl.txtDLC.Attributes.Add("onkeyup", "EnableSave(event);");
                        userCtrl.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
                        userCtrl.txtDLC.Attributes.Add("class", "pbDropdownBackground");
                        userCtrl.ID = "DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", "");
                        userCtrl.TextboxHeight = new Unit("40px");
                        userCtrl.TextboxWidth = new Unit("250px");
                        userCtrl.Value = "QUESTION-" + healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString();
                        //new
                        userCtrl.txtDLC.Attributes.Add("UserRole", ClientSession.UserRole);
                        //End new

                        if (healthQuestionScreenList[i].Question.ToUpper() == "TOTAL SCORE" || healthQuestionScreenList[i].Question.ToUpper() == "PERCENTAGE(%)")
                        {
                        }
                        else
                            userCtrl.Attributes.Add("style", "padding: 0px 0px 20px");
                        tc.Controls.Add(userCtrl);
                        tc.Style.Add(HtmlTextWriterStyle.Width, "310px");
                        userCtrl.txtDLC.Attributes.Add("onClick", "Enable_OR_Disable('" + userCtrl.txtDLC.ClientID + "," + "list" + "QUESTION-" + healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "')");
                        tr1.Cells.Add(tc);
                        if (healthQuestionScreenList[i].Question.ToUpper() == "TOTAL SCORE" || healthQuestionScreenList[i].Question.ToUpper() == "PERCENTAGE(%)")//BUGID:40479
                        {
                            //userCtrl.txtDLC.Attributes.Add("readonly", "readonly");
                            userCtrl.txtDLC.Attributes.Add("disabled", "disabled");
                            //userCtrl.txtDLC.Attributes.Add("style", "background-color:rgb(235, 235, 228); resize: none;");
                            userCtrl.txtDLC.Attributes.Add("class", "pbDropdownBackgrounddisable");
                            userCtrl.txtDLC.Enabled = false;
                            userCtrl.Enable = false;
                            userCtrl.pbDropdown.Disabled = true;
                        }
                    }

                    //tc = new TableCell();
                    //Label lblLine = new Label();
                    //lblLine.ID = "lbl" + i.ToString();
                    //lblLine.EnableViewState = false;
                    //lblLine.Text = "____________________________________________________________________________________________________________________________________";
                    //tc.ColumnSpan = 3;
                    //tc.Style.Add(HtmlTextWriterStyle.Width, "930px");
                    //tc.Controls.Add(lblLine);
                    //tr3.Cells.Add(tc);

                    sOldSystem = sNewQuestionType;

                    tbldynamic.Rows.Add(row);
                    tbldynamic.Rows.Add(tr1);

                    tbldynamic.Rows.Add(tr3);
                }
            }
            tbldynamic.Width = new Unit("950px");
            tbldynamic.CellPadding = 0;
            tbldynamic.CellSpacing = 0;
            //Session["Version"] = HashVersion;
            //// Session["HealthQuestionnaireTable"] = tbldynamic;
            //Session["HealthQuestionnaireID"] = HashHealthQuestionnaireID;
            //Session["QuestionnaireLookupID"] = HashQuestionnaireLookupID;

            divHealthQuestionnaire.Controls.Add(tbldynamic);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            IList<FillHealthcareQuestionnaire> TempHealthQuestionScreen = new List<FillHealthcareQuestionnaire>();
            IList<int> iVersion = new List<int>();
            if (healthQuestionScreenList != null)
                iVersion = healthQuestionScreenList.Where(a => a.Question == healthQuestionScreenList[0].Question && a.Questionnaire_Type == healthQuestionScreenList[0].Questionnaire_Type).Select(b => b.Version).ToList<int>();
            if (iVersion.Count > 0)
            {
                if (iVersion[0] == 0)
                {
                    healthQuestionScreenList = AppendHealthQuestion();
                }
                else
                {
                    TempHealthQuestionScreen = UpdateHealthQuestion();
                    if (TempHealthQuestionScreen != null && TempHealthQuestionScreen.Count > 0)
                    {
                        healthQuestionScreenList = TempHealthQuestionScreen;
                    }
                }
                if (hdnPrint.Value == "true")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "PrintQuestionnaire();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "SavedSuccessfully();", true);
                }

                // RadScriptManager1.AsyncPostBackErrorMessage = "true";
                btnSave.Disabled = true;
            }
            //if (healthQuestionScreenList != null)
            //{
            //    for (int i = 0; i < healthQuestionScreenList.Count; i++)
            //    {
            //        HashHealthQuestionnaireID[healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString()] = healthQuestionScreenList[i].HealthCare_Questionnaire_ID;
            //        HashQuestionnaireLookupID[healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString()] = healthQuestionScreenList[i].Questionnaire_Lookup_ID;
            //        HashVersion[healthQuestionScreenList[i].Question.Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString()] = healthQuestionScreenList[i].Version;
            //    }
            //}

            Session["healthQuestionScreenList"] = healthQuestionScreenList;
            if (healthQuestionScreenList != null)
            {
                var TotalScore = (from q in healthQuestionScreenList where q.Question.ToUpper() == "TOTAL SCORE" select q).ToList<FillHealthcareQuestionnaire>();
                if (TotalScore.Count() > 0)
                {
                    txtTotalScore.Text = TotalScore[0].Selected_Option;
                }
            }
            Session["IsPrevEncCopied"] = "false";
            if (HdnCopyButton.Value == "trueValidate")
            {
                bool bsaved = true;
                CopyPreviousEncounter(bsaved);
                HdnCopyButton.Value = "false";
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DLCLoad();", true);
        }

        private IList<FillHealthcareQuestionnaire> AppendHealthQuestion()
        {
            IList<Healthcare_Questionnaire> questionnaireList = new List<Healthcare_Questionnaire>();
            if (healthQuestionScreenList != null)
            {
                for (int i = 0; i < healthQuestionScreenList.Count; i++)
                {
                    Healthcare_Questionnaire questionTemp = new Healthcare_Questionnaire();
                    questionTemp.Encounter_ID = ClientSession.EncounterId;
                    questionTemp.Human_ID = ClientSession.HumanId;
                    questionTemp.Physician_ID = ClientSession.PhysicianId;
                    questionTemp.Created_By = ClientSession.UserName;
                    //if (hdnLocalTime.Value != string.Empty)
                    //    questionTemp.Created_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    questionTemp.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    questionTemp.Question = healthQuestionScreenList[i].Question;
                    if (questionTemp.Question.ToUpper().Trim() == "TOTAL SCORE")
                    {
                        questionTemp.Selected_Option = hdnTotalScore.Value.ToString();
                    }
                    if (questionTemp.Question.ToUpper().Trim() == "PERCENTAGE(%)")
                    {
                        questionTemp.Selected_Option = hdnPercentage.Value.ToString();
                    }
                    questionTemp.Questionnaire_Type = healthQuestionScreenList[i].Questionnaire_Type;

                    questionTemp.Questionnaire_Lookup_ID = healthQuestionScreenList[i].Questionnaire_Lookup_ID;

                    questionTemp.Questionnaire_Category = healthQuestionScreenList[i].Questionnaire_Category;
                    //CAP-3720
                    questionTemp.Question_Loinc_Code = healthQuestionScreenList[i].Question_Loinc_Code;
                    try
                    {
                        string controlId = healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString();
                        controlId = Regex.Replace(controlId, @"[^a-zA-Z0-9]", "_");
                        string[] sPossibleOptions = healthQuestionScreenList[i].Possible_Options.Split('|');
                        switch (healthQuestionScreenList[i].Controls.ToUpper())
                        {
                            case "RADIOBUTTON":

                                for (int j = 1; j < sPossibleOptions.Length; j++)
                                {
                                    if (((RadioButton)divHealthQuestionnaire.FindControl("rdbtn" + " " + sPossibleOptions[j].Trim() + " " + healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString())).Checked == true)
                                    {
                                        questionTemp.Selected_Option = ((RadioButton)divHealthQuestionnaire.FindControl("rdbtn" + " " + sPossibleOptions[j].Trim() + " " + healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString())).Text;
                                        break;
                                    }
                                    else
                                    {
                                        questionTemp.Selected_Option = "";
                                    }
                                }

                                break;
                            case "COMBOBOX":

                                if (!string.IsNullOrEmpty(((HtmlSelect)divHealthQuestionnaire.FindControl("cbo_" + controlId)).Value))
                                {
                                    questionTemp.Selected_Option = ((HtmlSelect)divHealthQuestionnaire.FindControl("cbo_" + controlId)).Value;
                                    //CAP-3720
                                    var possible_Options = healthQuestionScreenList[i].Possible_Options.Split('|');
                                    int selectedIndex = Array.IndexOf(possible_Options, questionTemp.Selected_Option);
                                    if (healthQuestionScreenList[i].Options_Loinc_Code?.Split('|')?.Length >= selectedIndex && selectedIndex != -1)
                                    {
                                        questionTemp.Selected_Option_Loinc_Code = healthQuestionScreenList[i].Options_Loinc_Code.Split('|')[selectedIndex];
                                    }
                                }
                                else
                                {
                                    questionTemp.Selected_Option = "";
                                }

                                break;
                            case "READONLYTEXTBOX":

                                if (!string.IsNullOrEmpty(((TextBox)divHealthQuestionnaire.FindControl("txt_" + controlId)).Text))
                                {
                                    questionTemp.Selected_Option = ((TextBox)divHealthQuestionnaire.FindControl("txt_" + controlId)).Text;
                                    //CAP-3720
                                    var possible_Options = healthQuestionScreenList[i].Possible_Options.Split('|');
                                    int selectedIndex = Array.IndexOf(possible_Options, questionTemp.Selected_Option);
                                    if (healthQuestionScreenList[i].Options_Loinc_Code?.Split('|')?.Length >= selectedIndex && selectedIndex != -1)
                                    {
                                        questionTemp.Selected_Option_Loinc_Code = healthQuestionScreenList[i].Options_Loinc_Code.Split('|')[selectedIndex];
                                    }
                                }
                                else
                                {
                                    questionTemp.Selected_Option = "";
                                }

                                break;
                            case "MULTISELECTTEXTBOX":

                                if (!string.IsNullOrEmpty(((HtmlTextArea)divHealthQuestionnaire.FindControl("txtArea_" + controlId)).Value))
                                {
                                    questionTemp.Selected_Option = ((HtmlTextArea)divHealthQuestionnaire.FindControl("txtArea_" + controlId)).Value;
                                    //CAP-3720
                                    foreach (var item in questionTemp.Selected_Option.Split(','))
                                    {
                                        var possible_Options = healthQuestionScreenList[i].Possible_Options.Split('|');
                                        int selectedIndex = Array.IndexOf(possible_Options, item);
                                        if (healthQuestionScreenList[i].Options_Loinc_Code?.Split('|')?.Length >= selectedIndex && selectedIndex != -1)
                                        {
                                            questionTemp.Selected_Option_Loinc_Code += healthQuestionScreenList[i].Options_Loinc_Code.Split('|')[selectedIndex];
                                            questionTemp.Selected_Option_Loinc_Code += ",";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(questionTemp.Selected_Option_Loinc_Code))
                                    {
                                        questionTemp.Selected_Option_Loinc_Code = questionTemp.Selected_Option_Loinc_Code.TrimEnd(',');
                                    }
                                }
                                else
                                {
                                    questionTemp.Selected_Option = "";
                                }

                                break;
                        }
                    }
                    catch
                    {
                        questionTemp.Selected_Option = "";
                    }

                    questionTemp.Version = healthQuestionScreenList[i].Version;

                    //questionTemp.Notes = ((CustomDLCNew)divHealthQuestionnaire.FindControl("DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))).txtDLC.Text;
                    questionTemp.Notes = ((CustomDLCNew)divHealthQuestionnaire.FindControl("DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))) != null ? ((CustomDLCNew)divHealthQuestionnaire.FindControl("DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))).txtDLC.Text : string.Empty;
                    healthQuestionScreenList[i].Selected_Option = questionTemp.Selected_Option;
                    healthQuestionScreenList[i].Notes = questionTemp.Notes;
                    healthQuestionScreenList[i].Created_Date_And_Time = questionTemp.Created_Date_And_Time;
                    healthQuestionScreenList[i].Created_By = questionTemp.Created_By;
                    healthQuestionScreenList[i].Encounter_ID = questionTemp.Encounter_ID;
                    questionnaireList.Add(questionTemp);
                }
            }
            return healthQuestionMngr.AppendHealthQuestion(questionnaireList.ToArray(), PatientDOB, dtpAppointmentDate, string.Empty, true, healthQuestionScreenList);
        }
        private IList<FillHealthcareQuestionnaire> UpdateHealthQuestion()
        {
            IList<Healthcare_Questionnaire> questionnaireList = new List<Healthcare_Questionnaire>();

            if (healthQuestionScreenList != null)
            {
                for (int i = 0; i < healthQuestionScreenList.Count; i++)
                {
                    Healthcare_Questionnaire questionTemp = new Healthcare_Questionnaire();
                    //questionTemp.Encounter_ID = ClientSession.EncounterId;
                    questionTemp.Human_ID = ClientSession.HumanId;
                    questionTemp.Physician_ID = ClientSession.PhysicianId;
                    //questionTemp.Modified_By = ClientSession.UserName;
                    //if (hdnLocalTime.Value != string.Empty)
                    //    questionTemp.Modified_Date_And_Time = Convert.ToDateTime(hdnLocalTime.Value);
                    //questionTemp.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    questionTemp.Question = healthQuestionScreenList[i].Question;
                    if (questionTemp.Question.ToUpper().Trim() == "TOTAL SCORE")
                    {
                        questionTemp.Selected_Option = hdnTotalScore.Value.ToString();
                    }
                    if (questionTemp.Question.ToUpper().Trim() == "PERCENTAGE(%)")
                    {
                        questionTemp.Selected_Option = hdnPercentage.Value.ToString();
                    }
                    questionTemp.Questionnaire_Type = healthQuestionScreenList[i].Questionnaire_Type;

                    questionTemp.Questionnaire_Lookup_ID = healthQuestionScreenList[i].Questionnaire_Lookup_ID;

                    questionTemp.Questionnaire_Category = healthQuestionScreenList[i].Questionnaire_Category;
                    //CAP-3720
                    questionTemp.Question_Loinc_Code = healthQuestionScreenList[i].Question_Loinc_Code;
                    //questionTemp.Created_By = healthQuestionScreenList[i].Created_By;
                    //questionTemp.Created_Date_And_Time = healthQuestionScreenList[i].Created_Date_And_Time;


                    try
                    {
                        string controlId = healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString();
                        controlId = Regex.Replace(controlId, @"[^a-zA-Z0-9]", "_");
                        string[] sPossibleOptions = healthQuestionScreenList[i].Possible_Options.Split('|');

                        switch (healthQuestionScreenList[i].Controls.ToUpper())
                        {
                            case "RADIOBUTTON":

                                for (int j = 1; j < sPossibleOptions.Length; j++)
                                {
                                    if (((RadioButton)divHealthQuestionnaire.FindControl("rdbtn" + " " + sPossibleOptions[j].Trim() + " " + healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString())).Checked == true)
                                    {
                                        questionTemp.Selected_Option = ((RadioButton)divHealthQuestionnaire.FindControl("rdbtn" + " " + sPossibleOptions[j].Trim() + " " + healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Trim().ToString() + "-" + i.ToString())).Text;
                                        break;
                                    }
                                    else
                                    {
                                        questionTemp.Selected_Option = "";
                                    }
                                }

                                break;
                            case "COMBOBOX":

                                if (!string.IsNullOrEmpty(((HtmlSelect)divHealthQuestionnaire.FindControl("cbo_" + controlId)).Value))
                                {
                                    questionTemp.Selected_Option = ((HtmlSelect)divHealthQuestionnaire.FindControl("cbo_" + controlId)).Value;
                                    //CAP-3720
                                    var possible_Options = healthQuestionScreenList[i].Possible_Options.Split('|');
                                    int selectedIndex = Array.IndexOf(possible_Options, questionTemp.Selected_Option);
                                    if (healthQuestionScreenList[i].Options_Loinc_Code?.Split('|')?.Length >= selectedIndex && selectedIndex != -1)
                                    {
                                        questionTemp.Selected_Option_Loinc_Code = healthQuestionScreenList[i].Options_Loinc_Code.Split('|')[selectedIndex];
                                    }
                                }
                                else
                                {
                                    questionTemp.Selected_Option = "";
                                }

                                break;
                            case "READONLYTEXTBOX":

                                if (!string.IsNullOrEmpty(((TextBox)divHealthQuestionnaire.FindControl("txt_" + controlId)).Text))
                                {
                                    questionTemp.Selected_Option = ((TextBox)divHealthQuestionnaire.FindControl("txt_" + controlId)).Text;
                                    //CAP-3720
                                    var possible_Options = healthQuestionScreenList[i].Possible_Options.Split('|');
                                    int selectedIndex = Array.IndexOf(possible_Options, questionTemp.Selected_Option);
                                    if (healthQuestionScreenList[i].Options_Loinc_Code?.Split('|')?.Length >= selectedIndex && selectedIndex != -1)
                                    {
                                        questionTemp.Selected_Option_Loinc_Code = healthQuestionScreenList[i].Options_Loinc_Code.Split('|')[selectedIndex];
                                    }
                                }
                                else
                                {
                                    questionTemp.Selected_Option = "";
                                }

                                break;
                            case "MULTISELECTTEXTBOX":

                                if (!string.IsNullOrEmpty(((HtmlTextArea)divHealthQuestionnaire.FindControl("txtArea_" + controlId)).Value))
                                {
                                    questionTemp.Selected_Option = ((HtmlTextArea)divHealthQuestionnaire.FindControl("txtArea_" + controlId)).Value;
                                    //CAP-3720
                                    foreach (var item in questionTemp.Selected_Option.Split(','))
                                    {
                                        var possible_Options = healthQuestionScreenList[i].Possible_Options.Split('|');
                                        int selectedIndex = Array.IndexOf(possible_Options, item);
                                        if (healthQuestionScreenList[i].Options_Loinc_Code?.Split('|')?.Length >= selectedIndex && selectedIndex != -1)
                                        {
                                            questionTemp.Selected_Option_Loinc_Code += healthQuestionScreenList[i].Options_Loinc_Code.Split('|')[selectedIndex];
                                            questionTemp.Selected_Option_Loinc_Code += ",";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(questionTemp.Selected_Option_Loinc_Code))
                                    {
                                        questionTemp.Selected_Option_Loinc_Code = questionTemp.Selected_Option_Loinc_Code.TrimEnd(',');
                                    }
                                }
                                else
                                {
                                    questionTemp.Selected_Option = "";
                                }

                                break;
                        }
                    }
                    catch
                    {
                        questionTemp.Selected_Option = "";
                    }

                    //questionTemp.Id = Convert.ToUInt64(HashHealthQuestionnaireID[questionTemp.Question.Trim().ToString() + "-" + questionTemp.Questionnaire_Type.Trim().ToString() + "-" + i.ToString()]);

                    //questionTemp.Version = Convert.ToInt32(HashVersion[questionTemp.Question.Trim().ToString() + "-" + questionTemp.Questionnaire_Type.Trim().ToString() + "-" + i.ToString()]);
                    //questionTemp.Notes = ((CustomDLCNew)divHealthQuestionnaire.FindControl("DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))).txtDLC.Text;
                    questionTemp.Notes = ((CustomDLCNew)divHealthQuestionnaire.FindControl("DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))) != null ? ((CustomDLCNew)divHealthQuestionnaire.FindControl("DLC" + (healthQuestionScreenList[i].Question.Replace(":", "").Trim().ToString() + "-" + healthQuestionScreenList[i].Questionnaire_Type.Replace(":", "").Trim().ToString() + "-" + i.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))).txtDLC.Text : string.Empty;


                    if (Session["IsPrevEncCopied"] != null && Convert.ToString(Session["IsPrevEncCopied"]) == "true")
                    {

                        /**/
                        //Check whether current encounter have Questionnaire data
                        IList<int> iVersion = new List<int>();
                        if (Session["healthQuestionScreenList_CurrEnc"] != null)
                            healthQuestionScreenList_CurrEnc = (IList<FillHealthcareQuestionnaire>)Session["healthQuestionScreenList_CurrEnc"];
                        if (healthQuestionScreenList_CurrEnc != null && healthQuestionScreenList_CurrEnc.Count > 0)
                            iVersion = healthQuestionScreenList_CurrEnc.Where(a => a.Question == healthQuestionScreenList_CurrEnc[0].Question && a.Questionnaire_Type == healthQuestionScreenList_CurrEnc[0].Questionnaire_Type).Select(b => b.Version).ToList<int>();
                        if (iVersion.Count > 0 && iVersion[0] > 0)
                        {

                            FillHealthcareQuestionnaire healthQuestionnaire = (from healthQuestionnaireChanged in healthQuestionScreenList_CurrEnc
                                                                               where healthQuestionnaireChanged.Questionnaire_Category == healthQuestionScreenList[i].Questionnaire_Category &&
                                                      healthQuestionnaireChanged.Questionnaire_Type == healthQuestionScreenList[i].Questionnaire_Type &&
                                                      healthQuestionnaireChanged.Question == healthQuestionScreenList[i].Question
                                                                               select healthQuestionnaireChanged).ToList()[0];

                            questionTemp.Version = healthQuestionnaire.Version;
                            questionTemp.Encounter_ID = ClientSession.EncounterId;
                            questionTemp.Created_By = healthQuestionnaire.Created_By;
                            questionTemp.Created_Date_And_Time = healthQuestionnaire.Created_Date_And_Time;
                            questionTemp.Modified_By = ClientSession.UserName;
                            questionTemp.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            questionTemp.Id = healthQuestionnaire.HealthCare_Questionnaire_ID;

                        }
                        else
                        {
                            questionTemp.Version = 0;
                            questionTemp.Id = 0;
                            questionTemp.Encounter_ID = ClientSession.EncounterId;
                            questionTemp.Created_By = ClientSession.UserName;
                            questionTemp.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            questionTemp.Modified_By = string.Empty;
                            questionTemp.Modified_Date_And_Time = DateTime.MinValue;
                        }

                    }
                    else
                    {
                        questionTemp.Created_By = healthQuestionScreenList[i].Created_By;
                        questionTemp.Created_Date_And_Time = healthQuestionScreenList[i].Created_Date_And_Time;
                        questionTemp.Encounter_ID = ClientSession.EncounterId;
                        questionTemp.Id = healthQuestionScreenList[i].HealthCare_Questionnaire_ID;
                        questionTemp.Version = healthQuestionScreenList[i].Version;
                        questionTemp.Modified_By = ClientSession.UserName;
                        questionTemp.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                    }
                    healthQuestionScreenList[i].Selected_Option = questionTemp.Selected_Option;
                    healthQuestionScreenList[i].Notes = questionTemp.Notes;
                    healthQuestionScreenList[i].Created_Date_And_Time = questionTemp.Created_Date_And_Time;
                    healthQuestionScreenList[i].Created_By = questionTemp.Created_By;
                    healthQuestionScreenList[i].Modified_Date_And_Time = questionTemp.Modified_Date_And_Time;
                    healthQuestionScreenList[i].Modified_By = questionTemp.Modified_By;
                    healthQuestionScreenList[i].Encounter_ID = questionTemp.Encounter_ID;
                    questionnaireList.Add(questionTemp);
                }
            }
            return healthQuestionMngr.UpdateHealthQuestion(questionnaireList.ToArray(), PatientDOB, dtpAppointmentDate, string.Empty, true, healthQuestionScreenList);
        }

        protected void chkQuestion_CheckedChanged(object sender, EventArgs e)
        {


            if (Session["healthQuestionScreenList"] != null)
                healthQuestionScreenList = (IList<FillHealthcareQuestionnaire>)Session["healthQuestionScreenList"];
            string Questionnarie_Default_option = string.Empty;
            for (int i = 0; i < healthQuestionScreenList.Count; i++)
            {
                Questionnarie_Default_option += healthQuestionScreenList[i].Question + "^" + healthQuestionScreenList[i].Normal_Question_Status.Replace("'", "#") + "^" + healthQuestionScreenList[i].Possible_Options.Replace("'", "#") + "^" + healthQuestionScreenList[i].Questionnaire_Type + "^" + healthQuestionScreenList[i].Controls + "~";
            }
            btnSave.Disabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CallAlert", "chkCheckedChange('" + Questionnarie_Default_option + "');", true);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string strPath = string.Empty;
            // string From_Date = UtilityManager.ConvertToUniversal(dtpFromDate.SelectedDate.Value.Date).ToString("yyyy-MM-dd hh:mm:ss");
            // string To_Date = UtilityManager.ConvertToUniversal(dtpTodate.SelectedDate.Value.Date.AddDays(1)).ToString("yyyy-MM-dd hh:mm:ss");
            string strtype = Request["TabName"].ToString();
            string strEncounterId = ClientSession.EncounterId.ToString();
            //string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl"].ToString();
            string sPath = string.Empty;
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            //string[] conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString).ToString().Split(';');
            string[] conString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ToString().Split(';');
            string sDataBase = string.Empty;
            string sDataSource = string.Empty;
            string sUserId = string.Empty;
            string sPassword = string.Empty;
            string sPort = "3306";
            for (int i = 0; i < conString.Length; i++)
            {
                if (conString[i].ToString().ToUpper().Contains("DATABASE=") == true)
                {
                    sDataBase = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("DATA SOURCE") == true)
                {
                    sDataSource = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("USER ID") == true)
                {
                    sUserId = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PASSWORD") == true)
                {
                    sPassword = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PORT") == true)
                {
                    sPort = conString[i].ToString().Split('=')[1];
                }
            }
            string sodaURL = string.Empty;
            string sAzure = System.Configuration.ConfigurationManager.AppSettings["Azure"].ToString();
            if (sAzure == "Y")
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase + "?useSSL=true&requireSSL=false";
            else
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;

            string sodaUser = sUserId;
            string sodaPassword = sPassword;
            string sDBConnection = "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword;
            string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl_" + ClientSession.LegalOrg].ToString();

            strPath = sBIRTReportUrl + "Healthcare_Questionnaire_Report.rptdesign" + sDBConnection + "&strEncounterId=" + strEncounterId + "&strQuestionnaire_Type=" + strtype + "&legal_org=" + ClientSession.LegalOrg + "&__title=Healthcare_Questionnaire_Reports_" + strtype + "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Report", "OpenReport('" + strPath + "');", true);

        }
        [WebMethod(EnableSession = true)]
        public static string PrintQuestionnaire(string strCategory, string strDos, string strProvider)
        {

            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string strPath, strFooterText = string.Empty;
            //CAP-3203
            //string strType = strCategory.Replace("%20", " ");
            string strType = strCategory.Replace("%20", " ").Replace("+", " ");
            string strEncounterId = ClientSession.EncounterId.ToString();
            IList<PatientPane> PatientPaneList = ClientSession.PatientPaneList.Where(a => a.Encounter_ID == ClientSession.EncounterId).ToList<PatientPane>();
            string strDemographics = PatientPaneList[0].Last_Name + ", " + PatientPaneList[0].First_Name + " " + PatientPaneList[0].MI + " " + PatientPaneList[0].Suffix + " | " + Convert.ToDateTime(PatientPaneList[0].Birth_Date).ToString("dd-MMM-yyyy") + " | " + PatientPaneList[0].Sex + " | Acc #:" + PatientPaneList[0].Human_Id + " | " + PatientPaneList[0].Patient_Type + " | " + strDos + " | " + strProvider;
            if (strType == "Asthma Control Test" || strType == "CAT Screening")
            {
                strFooterText = "All values determined from the scale of 0 to 5";
            }
            else if (strType == "Depression Test" || strType == "Pulmonary/Sleep Exam" || strType == "Epworth Sleep Score" || strType == "Depression" || strType == "Cognitive Screening" || strType == "PHQ-9 Screening")
            {
                strFooterText = "All values determined from the scale of 0 to 3";
            }
            else if (strType == "Fall Risk Screening")
            {
                strFooterText = "All values determined from the scale of 0 to 30";
            }
            else if (strType == "Pain Screening")
            {
                strFooterText = "All values determined from the scale of 0 to 10";
            }
            else
            {
                strFooterText = "No FooterText";
            }
            //string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl"].ToString();
            string sPath = string.Empty;
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration().Configure();
            //string[] conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString).ToString().Split(';');
            string[] conString = System.Configuration.ConfigurationManager.ConnectionStrings["con_" + ClientSession.LegalOrg].ToString().Split(';');

            string sDataBase = string.Empty;
            string sDataSource = string.Empty;
            string sUserId = string.Empty;
            string sPassword = string.Empty;
            string sPort = "3306";
            for (int i = 0; i < conString.Length; i++)
            {
                if (conString[i].ToString().ToUpper().Contains("DATABASE=") == true)
                {
                    sDataBase = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("DATA SOURCE") == true)
                {
                    sDataSource = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("USER ID") == true)
                {
                    sUserId = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PASSWORD") == true)
                {
                    sPassword = conString[i].ToString().Split('=')[1];
                }
                if (conString[i].ToString().ToUpper().Contains("PORT") == true)
                {
                    sPort = conString[i].ToString().Split('=')[1];
                }
            }
            string sodaURL = string.Empty;
            string sAzure = System.Configuration.ConfigurationManager.AppSettings["Azure"].ToString();
            if (sAzure == "Y")
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase + "?useSSL=true&requireSSL=false";
            else
                sodaURL = "jdbc:mysql://" + sDataSource + ":" + sPort + "/" + sDataBase;

            string sodaUser = sUserId;
            string sodaPassword = sPassword;
            string sDBConnection = "&odaURL=" + sodaURL + "&odaUser=" + sodaUser + "&odaPassword=" + sodaPassword;
            string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl_" + ClientSession.LegalOrg].ToString();


            if (strType == "CAT Screening")
            {
                strPath = sBIRTReportUrl + "Healthcare_Questionnaire_Report - CAT_Screening.rptdesign" + sDBConnection + "&strEncounterId=" + strEncounterId + "&strQuestionnaire_Type=" + strCategory + "&strDemographics=" + strDemographics.Replace("#", "%23").Replace("|", "%7C") + "&strFooterText=" + strFooterText.Replace("#", "%23").Replace("|", "%7C") + "&legal_org=" + ClientSession.LegalOrg + "&__title=" + strCategory + " " + "Questionnaire";
            }
            else
            {
                strPath = sBIRTReportUrl + "Healthcare_Questionnaire_Report.rptdesign" + sDBConnection + "&strEncounterId=" + strEncounterId + "&strQuestionnaire_Type=" + strCategory + "&strDemographics=" + strDemographics.Replace("#", "%23").Replace("|", "%7C") + "&strFooterText=" + strFooterText.Replace("#", "%23").Replace("|", "%7C") + "&legal_org=" + ClientSession.LegalOrg + "&__title=" + strCategory + " " + "Questionnaire";
            }
            return strPath;

        }
        //Added for BugID:44324
        protected void CopyPreviousEncounter(bool bsaved)
        {
            HealthcareQuestionnaireManager questionnaireManager = new HealthcareQuestionnaireManager();
            IList<FillHealthcareQuestionnaire> questionnaireDTO = new List<FillHealthcareQuestionnaire>();
            bool isPhysicianProcess = false;
            bool isPrevEncQuestionnairePresent = false;
            bool isPrevEncPresent = false;
            bool isFromArchive = false;
            ulong prevEncID = 0;
            bool isAlert = false;
            if (Request["TabName"] != null)
                sMyCategory = Request["TabName"].ToString();

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                IList<PatientPane> PatientPaneList = ClientSession.PatientPaneList.Where(a => a.Encounter_ID == ClientSession.EncounterId).ToList<PatientPane>();
                if (PatientPaneList.Count > 0)
                {
                    dtpAppointmentDate = PatientPaneList[0].Date_of_Service;
                    PatientDOB = PatientPaneList[0].Birth_Date;
                }
            }
            questionnaireManager.GetQuestionnaireforPastEncounter(ClientSession.HumanId, ClientSession.EncounterId, ClientSession.PhysicianId, sMyCategory, out isPrevEncPresent, out isPhysicianProcess, out isPrevEncQuestionnairePresent, out prevEncID, out isFromArchive);

            if (!isPrevEncPresent)
            {
                if (bsaved)
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PrevEncMissing", "savedSuccessfully(); onCopyPrevious('210010');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PrevEncMissing", "onCopyPrevious('210010'); ", true);
                isAlert = true;
            }
            else if (!isPhysicianProcess)
            {
                if (bsaved)
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PrevEncNotPhysicianProcess", "savedSuccessfully(); onCopyPrevious('210016');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PrevEncNotPhysicianProcess", "onCopyPrevious('210016');", true);
                isAlert = true;
            }
            if (!isPrevEncQuestionnairePresent)
            {
                if (bsaved)
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PrevEncQDataNotPresent", "savedSuccessfully(); onCopyPrevious('170014');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "PrevEncQDataNotPresent", "onCopyPrevious('170014');", true);
                isAlert = true;
            }
            if (isAlert)
            {
                if (Session["healthQuestionScreenList"] != null)
                    healthQuestionScreenList = (IList<FillHealthcareQuestionnaire>)Session["healthQuestionScreenList"];

                FillMyQuestionList(healthQuestionScreenList);

                return;
                //if (Session["Version"] != null)
                //    HashVersion = (Hashtable)Session["Version"];
                //if (Session["HealthQuestionnaireID"] != null)
                //    HashHealthQuestionnaireID = (Hashtable)Session["HealthQuestionnaireID"];
                //if (Session["QuestionnaireLookupID"] != null)
                //    HashQuestionnaireLookupID = (Hashtable)Session["QuestionnaireLookupID"];
            }
            else
            {
                if (Session["healthQuestionScreenList"] != null)
                    healthQuestionScreenList_CurrEnc = (IList<FillHealthcareQuestionnaire>)Session["healthQuestionScreenList"];
                Session["healthQuestionScreenList_CurrEnc"] = healthQuestionScreenList_CurrEnc;
                IList<FillHealthcareQuestionnaire> healthQuestionScreenList1 = new List<FillHealthcareQuestionnaire>();
                QuestionnaireLookupManager questionnaireMngr = new QuestionnaireLookupManager();
                //CAP-3628
                //healthQuestionScreenList1 = questionnaireMngr.GetHealthQuestionLookupListFromServer(sMyCategory, PatientDOB, dtpAppointmentDate, ClientSession.PhysicianUserName, prevEncID, true);
                healthQuestionScreenList1 = questionnaireMngr.GetHealthQuestionLookupListFromServer(sMyCategory, PatientDOB, dtpAppointmentDate, ClientSession.PhysicianUserName, prevEncID, true, ClientSession.HumanId);
                if (healthQuestionScreenList1.Count == healthQuestionScreenList_CurrEnc.Count)
                {
                    for (int i = 0; i < healthQuestionScreenList1.Count; i++)
                    {

                        healthQuestionScreenList_CurrEnc[i].Selected_Option = healthQuestionScreenList1[i].Selected_Option;
                        healthQuestionScreenList_CurrEnc[i].Notes = healthQuestionScreenList1[i].Notes;

                    }
                }

                Session["healthQuestionScreenList"] = healthQuestionScreenList_CurrEnc;
                hdnTotalScore.Value = string.Empty;//To handle case: prev enc total score is empty.
                hdnPercentage.Value = string.Empty;
                hdnTotalScoreDescription.Value = string.Empty;
                FillMyQuestionList(healthQuestionScreenList_CurrEnc);
                //if (Session["Version"] != null)
                //    HashVersion = (Hashtable)Session["Version"];
                //if (Session["HealthQuestionnaireID"] != null)
                //    HashHealthQuestionnaireID = (Hashtable)Session["HealthQuestionnaireID"];
                //if (Session["QuestionnaireLookupID"] != null)
                //    HashQuestionnaireLookupID = (Hashtable)Session["QuestionnaireLookupID"];
                Session["IsPrevEncCopied"] = "true";
                btnSave.Disabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "autosave", "{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();onCopyPrevious(''); }", true);
            }

        }

        protected void btnCopyPrevHidden_Click(object sender, EventArgs e)
        {
            CopyPreviousEncounter(false);
        }

        protected void btnCopyPrevious_Click(object sender, EventArgs e)
        {
            CopyPreviousEncounter(false);
        }

        //public static string PrintQuestionnaire(string strCategory)
        //{
        //    string strPath = string.Empty;
        //    //string strCategory = Request["TabName"].ToString();
        //    string strEncounterId = ClientSession.EncounterId.ToString();
        //    string sBIRTReportUrl = System.Configuration.ConfigurationManager.AppSettings["BIRTReportUrl"].ToString();
        //    strPath = sBIRTReportUrl + "Healthcare_Questionnaire_Report.rptdesign" + "&strEncounterId=" + strEncounterId + "&strQuestionnaire_Type=" + strCategory + "&__title=" + strCategory + " " + "Questionnaire";
        //    return strPath;

        //}

    }
}