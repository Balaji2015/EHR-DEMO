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
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;

namespace Acurus.Capella.UI
{
    public partial class frmReviewOfQuestionnaire : SessionExpired
    {
        IList<FillHealthcareQuestionnaire> roqList;
        IList<FillHealthcareQuestionnaire> symptomNamesLookUp = new List<FillHealthcareQuestionnaire>();
        IList<FillHealthcareQuestionnaire> systemNames = new List<FillHealthcareQuestionnaire>();
        TableRow objPanelTableRow = new TableRow();
        TableCell objPanelTableCell = new TableCell();
        string systemNamesForEvent = string.Empty;
        IList<string> symptomNames = new List<string>();
        int symptomWidth = 100;
        IList<string> questionNames;
        Hashtable HashQuestionnaireLookupID = new Hashtable();
        Hashtable HashVersion = new Hashtable();
        Hashtable HashHealthQuestionnaireID = new Hashtable();
        string sMyCategory = string.Empty;
        IList<Panel> groupBox = new List<Panel>();
        //Cap - 3059
        DateTime PatientDOB = DateTime.MinValue;
        DateTime dtpAppointmentDate = DateTime.MinValue;
        IList<int> noOfSymptomRows = new List<int>();
        IList<int> symptomMaxHeight = new List<int>();


        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    if (Session["ScreenControls"]!= null)
        //    {
        //        Table testTable = (Table)Session["ScreenControls"];
        //        try
        //        {
        //            pnlReviewOfQuestionnaire.Controls.Add(testTable);

        //        }
        //        catch
        //        {

        //        }
        //    }

        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //Cap - 3059
            //DateTime PatientDOB = DateTime.MinValue;
            //DateTime dtpAppointmentDate = DateTime.MinValue;
            //IList<int> noOfSymptomRows = new List<int>();
            //IList<int> symptomMaxHeight = new List<int>();

            if (Request["TabName"] != null)
                sMyCategory = Request["TabName"].ToString();
            if (!IsPostBack)
            {
                ClientSession.FlushSession();
                if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                {
                    IList<PatientPane> PatientPaneList = ClientSession.PatientPaneList.Where(a => a.Encounter_ID == ClientSession.EncounterId).ToList<PatientPane>();
                    if (PatientPaneList.Count > 0)
                    {
                        dtpAppointmentDate = PatientPaneList[0].Appointment_Date_Time;
                        PatientDOB = PatientPaneList[0].Birth_Date;
                    }
                }
                QuestionnaireLookupManager RoqMngr = new QuestionnaireLookupManager();
                roqList = RoqMngr.GetHealthQuestionLookupListFromServer(sMyCategory, PatientDOB, dtpAppointmentDate, ClientSession.PhysicianUserName, ClientSession.EncounterId, false);
                Session["fillRoq"] = roqList;
            }

            if (Session["fillRoq"] != null)
            {
                roqList = (IList<FillHealthcareQuestionnaire>)Session["fillRoq"];

                groupBoxCreation(roqList, noOfSymptomRows, symptomMaxHeight);

                checkBoxAndOtherControlsCreation(noOfSymptomRows, symptomMaxHeight);

                chkAllOtherSystemsNormal.Attributes.Add("OnClick", "chkAllOtherSystemsNormalClick('" + systemNamesForEvent + "')");

                for (int k = 0; k < roqList.Count; k++)
                {
                    HashHealthQuestionnaireID[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].Questionnaire_Lookup_ID;
                    HashVersion[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].Version;
                }
            }


            if (!IsPostBack)
            {
                ClientSession.processCheck = true;
                SecurityServiceUtility objSecurity = new SecurityServiceUtility();
                objSecurity.ApplyUserPermissions(this.Page);
                btnSave.Enabled = false;
            }


        }
        public void groupBoxCreation(IList<FillHealthcareQuestionnaire> fillQuestion, IList<int> noOfSymptomRows, IList<int> symptomMaxHeight)
        {
            IList<string> symptomCountList = new List<string>();
            IList<int> symptomHeight = new List<int>();
            IList<string> systemListString = new List<string>();
            IList<string> symptomListString = new List<string>();

            QuestionnaireLookupManager RoqMngr = new QuestionnaireLookupManager();

            Table objPanelTable = new Table();

            string sPatientSex = "";

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                sPatientSex = ClientSession.PatientPaneList[0].Sex;
            }

            symptomNamesLookUp = new List<FillHealthcareQuestionnaire>();

            int noOfSymptoms = 0;
            int currentSystemNumber = 0;

            if (fillQuestion != null && fillQuestion.Count > 0)
            {
                roqList = fillQuestion;
                for (int k = 0; k < fillQuestion.Count; k++)
                {
                    HashHealthQuestionnaireID[fillQuestion[k].Question.ToString() + "-" + fillQuestion[k].Questionnaire_Type.ToString()] = fillQuestion[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[fillQuestion[k].Question.ToString() + "-" + fillQuestion[k].Questionnaire_Type.ToString()] = fillQuestion[k].Questionnaire_Lookup_ID;
                    HashVersion[fillQuestion[k].Question.ToString() + "-" + fillQuestion[k].Questionnaire_Type.ToString()] = fillQuestion[k].Version;
                }
            }

            if (fillQuestion != null && fillQuestion.Count == 0)
            {
                systemNames = RoqMngr.GetHealthQuestionLookupListFromLocal(sMyCategory, DateTime.MinValue, DateTime.MinValue, ClientSession.PhysicianUserName, ClientSession.EncounterId, false);
                for (int k = 0; k < systemNames.Count; k++)
                {

                    HashHealthQuestionnaireID[systemNames[k].Question.ToString() + "-" + systemNames[k].Questionnaire_Type.ToString()] = systemNames[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[systemNames[k].Question.ToString() + "-" + systemNames[k].Questionnaire_Type.ToString()] = systemNames[k].Questionnaire_Lookup_ID;
                    HashVersion[systemNames[k].Question.ToString() + "-" + systemNames[k].Questionnaire_Type.ToString()] = systemNames[k].Version;
                }
            }
            if (roqList != null)
            {
                if (roqList.Count == 0)
                {
                    string sex = sPatientSex.ToUpper() == "MALE" ? "FEMALE" : "MALE";

                    systemNamesForEvent = arrayListToString(systemNames.Select(s => s.Questionnaire_Type).Distinct().ToArray());


                    if (systemNames != null)
                    {
                        string[] systemNameListString = systemNames.Select(s => (s.Questionnaire_Type)).ToArray();
                        symptomNamesLookUp = systemNames;
                        questionNames = systemNames.Select(a => a.Questionnaire_Type).Distinct().ToList<string>();

                        for (int i = 0; i < questionNames.Count; i++)
                        {

                            noOfSymptoms = symptomNamesLookUp.Count(c => c.Questionnaire_Type == questionNames[i]);
                            if (noOfSymptoms == 0)
                                continue;
                            systemGroupBoxCreation(questionNames[i]);
                            symptomCountList.Add(noOfSymptoms.ToString());
                            currentSystemNumber++;

                            int tempNoOfSymptoms = noOfSymptoms % 2 == 0 ? (int)(noOfSymptoms / 2) : (int)(noOfSymptoms / 2) + 1;
                            noOfSymptomRows.Add(tempNoOfSymptoms);

                            symptomListString = symptomNamesLookUp.Where(n => n.Questionnaire_Type == (questionNames[i])).Select(s => s.Question).ToList();

                            foreach (var item in symptomListString)
                            {
                                string txt = item;
                                Font f = new Font(FontFamily.GenericSansSerif, 8.2f, FontStyle.Regular);
                                Bitmap bp = new Bitmap(1, 1);
                                Graphics gp = Graphics.FromImage(bp);
                                int height = (int)gp.MeasureString(txt, f, symptomWidth).Height;
                                symptomHeight.Add(height - 15);
                            }


                            if (currentSystemNumber != 0 && currentSystemNumber % 3 == 0)
                            {
                                if (symptomHeight.Count > 0)
                                    symptomMaxHeight.Add(symptomHeight.Max());

                                objPanelTable.Controls.Add(objPanelTableRow);
                                objPanelTableRow = new TableRow();

                                groupBox.Clear();
                                symptomCountList.Clear();
                                currentSystemNumber = 0;
                            }


                        }

                        if (currentSystemNumber < 3)
                        {
                            if (symptomHeight.Count > 0)
                                symptomMaxHeight.Add(symptomHeight.Max());

                            symptomHeight.Clear();
                            currentSystemNumber = 0;
                            objPanelTable.Controls.Add(objPanelTableRow);
                        }

                        this.pnlReviewOfQuestionnaire.Controls.Add(objPanelTable);
                        Session["ScreenControls"] = objPanelTable;
                        Session["Version"] = HashVersion;
                        Session["HealthQuestionnaireID"] = HashHealthQuestionnaireID;
                        Session["QuestionnaireLookupID"] = HashQuestionnaireLookupID;
                        if (currentSystemNumber > 0)
                            groupBox.Clear();


                    }
                }
                else
                {
                    systemListString = roqList.Select(s => s.Questionnaire_Type).Distinct().ToList();

                    if (sPatientSex.ToUpper() == "MALE")
                        systemListString = systemListString.Where(a => a.ToUpper() != "GYNECOLOGICAL FEMALE").ToList<string>();
                    systemNamesForEvent = arrayListToString(systemListString.ToArray());


                    if (systemListString != null)
                    {
                        for (int i = 0; i < systemListString.Count; i++)
                        {

                            symptomListString = roqList.Where(s => s.Questionnaire_Type == systemListString[i]).Select(s => s.Question).ToList();

                            noOfSymptoms = symptomListString.Count;
                            systemGroupBoxCreation(systemListString[i]);
                            symptomCountList.Add(noOfSymptoms.ToString());
                            currentSystemNumber++;

                            int tempNoOfSymptoms = noOfSymptoms % 2 == 0 ? (int)(noOfSymptoms / 2) : (int)(noOfSymptoms / 2) + 1;
                            noOfSymptomRows.Add(tempNoOfSymptoms);

                            foreach (var item in symptomListString)
                            {
                                string txt = item;
                                Font f = new Font(FontFamily.GenericSansSerif, 8.2f, FontStyle.Regular);
                                Bitmap bp = new Bitmap(1, 1);
                                Graphics gp = Graphics.FromImage(bp);
                                int height = (int)gp.MeasureString(txt, f, symptomWidth).Height;
                                symptomHeight.Add(height - 15);
                            }

                            if (currentSystemNumber != 0 && currentSystemNumber % 3 == 0)
                            {
                                if (symptomHeight.Count > 0)
                                    symptomMaxHeight.Add(symptomHeight.Max());

                                objPanelTable.Controls.Add(objPanelTableRow);
                                objPanelTableRow = new TableRow();

                                groupBox.Clear();
                                symptomCountList.Clear();
                                symptomHeight.Clear();
                                currentSystemNumber = 0;
                            }

                        }

                        if (currentSystemNumber < 3)
                        {
                            if (symptomHeight.Count > 0)
                                symptomMaxHeight.Add(symptomHeight.Max());
                            symptomHeight.Clear();
                            currentSystemNumber = 0;
                            objPanelTable.Controls.Add(objPanelTableRow);
                        }

                        this.pnlReviewOfQuestionnaire.Controls.Add(objPanelTable);
                        Session["ScreenControls"] = objPanelTable;
                        Session["Version"] = HashVersion;
                        Session["HealthQuestionnaireID"] = HashHealthQuestionnaireID;
                        Session["QuestionnaireLookupID"] = HashQuestionnaireLookupID;
                    }
                    if (currentSystemNumber > 0)
                        groupBox.Clear();
                }
            }
        }

        public void groupBoxCreationCopyPrevious(IList<FillHealthcareQuestionnaire> fillQuestion, IList<int> noOfSymptomRows, IList<int> symptomMaxHeight)
        {
            IList<string> symptomCountList = new List<string>();
            IList<int> symptomHeight = new List<int>();
            IList<string> systemListString = new List<string>();
            IList<string> symptomListString = new List<string>();

            QuestionnaireLookupManager RoqMngr = new QuestionnaireLookupManager();

            Table objPanelTable = new Table();

            string sPatientSex = "";

            if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
            {
                sPatientSex = ClientSession.PatientPaneList[0].Sex;
            }

            symptomNamesLookUp = new List<FillHealthcareQuestionnaire>();

            int noOfSymptoms = 0;
            int currentSystemNumber = 0;

            if (fillQuestion != null && fillQuestion.Count > 0)
            {
                roqList = fillQuestion;
                for (int k = 0; k < fillQuestion.Count; k++)
                {
                    HashHealthQuestionnaireID[fillQuestion[k].Question.ToString() + "-" + fillQuestion[k].Questionnaire_Type.ToString()] = fillQuestion[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[fillQuestion[k].Question.ToString() + "-" + fillQuestion[k].Questionnaire_Type.ToString()] = fillQuestion[k].Questionnaire_Lookup_ID;
                    HashVersion[fillQuestion[k].Question.ToString() + "-" + fillQuestion[k].Questionnaire_Type.ToString()] = fillQuestion[k].Version;
                }
            }

            if (fillQuestion != null && fillQuestion.Count == 0)
            {
                systemNames = RoqMngr.GetHealthQuestionLookupListFromLocal(sMyCategory, DateTime.MinValue, DateTime.MinValue, ClientSession.PhysicianUserName, ClientSession.EncounterId, false);
                for (int k = 0; k < systemNames.Count; k++)
                {

                    HashHealthQuestionnaireID[systemNames[k].Question.ToString() + "-" + systemNames[k].Questionnaire_Type.ToString()] = systemNames[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[systemNames[k].Question.ToString() + "-" + systemNames[k].Questionnaire_Type.ToString()] = systemNames[k].Questionnaire_Lookup_ID;
                    HashVersion[systemNames[k].Question.ToString() + "-" + systemNames[k].Questionnaire_Type.ToString()] = systemNames[k].Version;
                }
            }
            if (roqList != null)
            {
                if (roqList.Count == 0)
                {
                    string sex = sPatientSex.ToUpper() == "MALE" ? "FEMALE" : "MALE";

                    systemNamesForEvent = arrayListToString(systemNames.Select(s => s.Questionnaire_Type).Distinct().ToArray());


                    if (systemNames != null)
                    {
                        string[] systemNameListString = systemNames.Select(s => (s.Questionnaire_Type)).ToArray();
                        symptomNamesLookUp = systemNames;
                        questionNames = systemNames.Select(a => a.Questionnaire_Type).Distinct().ToList<string>();

                        for (int i = 0; i < questionNames.Count; i++)
                        {

                            noOfSymptoms = symptomNamesLookUp.Count(c => c.Questionnaire_Type == questionNames[i]);
                            if (noOfSymptoms == 0)
                                continue;
                            systemGroupBoxCreation(questionNames[i]);
                            symptomCountList.Add(noOfSymptoms.ToString());
                            currentSystemNumber++;

                            int tempNoOfSymptoms = noOfSymptoms % 2 == 0 ? (int)(noOfSymptoms / 2) : (int)(noOfSymptoms / 2) + 1;
                            noOfSymptomRows.Add(tempNoOfSymptoms);

                            symptomListString = symptomNamesLookUp.Where(n => n.Questionnaire_Type == (questionNames[i])).Select(s => s.Question).ToList();

                            foreach (var item in symptomListString)
                            {
                                string txt = item;
                                Font f = new Font(FontFamily.GenericSansSerif, 8.2f, FontStyle.Regular);
                                Bitmap bp = new Bitmap(1, 1);
                                Graphics gp = Graphics.FromImage(bp);
                                int height = (int)gp.MeasureString(txt, f, symptomWidth).Height;
                                symptomHeight.Add(height - 15);
                            }


                            if (currentSystemNumber != 0 && currentSystemNumber % 3 == 0)
                            {
                                if (symptomHeight.Count > 0)
                                    symptomMaxHeight.Add(symptomHeight.Max());

                                objPanelTable.Controls.Add(objPanelTableRow);
                                objPanelTableRow = new TableRow();

                                groupBox.Clear();
                                symptomCountList.Clear();
                                currentSystemNumber = 0;
                            }


                        }

                        if (currentSystemNumber < 3)
                        {
                            if (symptomHeight.Count > 0)
                                symptomMaxHeight.Add(symptomHeight.Max());

                            symptomHeight.Clear();
                            currentSystemNumber = 0;
                            objPanelTable.Controls.Add(objPanelTableRow);
                        }
                        //this.pnlReviewOfQuestionnaire.Controls.Add(objPanelTable);
                        Session["ScreenControls"] = objPanelTable;
                        Session["Version"] = HashVersion;
                        Session["HealthQuestionnaireID"] = HashHealthQuestionnaireID;
                        Session["QuestionnaireLookupID"] = HashQuestionnaireLookupID;
                        if (currentSystemNumber > 0)
                            groupBox.Clear();


                    }
                }
                else
                {
                    systemListString = roqList.Select(s => s.Questionnaire_Type).Distinct().ToList();

                    if (sPatientSex.ToUpper() == "MALE")
                        systemListString = systemListString.Where(a => a.ToUpper() != "GYNECOLOGICAL FEMALE").ToList<string>();
                    systemNamesForEvent = arrayListToString(systemListString.ToArray());


                    if (systemListString != null)
                    {
                        for (int i = 0; i < systemListString.Count; i++)
                        {

                            symptomListString = roqList.Where(s => s.Questionnaire_Type == systemListString[i]).Select(s => s.Question).ToList();

                            noOfSymptoms = symptomListString.Count;
                            systemGroupBoxCreation(systemListString[i]);
                            symptomCountList.Add(noOfSymptoms.ToString());
                            currentSystemNumber++;

                            int tempNoOfSymptoms = noOfSymptoms % 2 == 0 ? (int)(noOfSymptoms / 2) : (int)(noOfSymptoms / 2) + 1;
                            noOfSymptomRows.Add(tempNoOfSymptoms);

                            foreach (var item in symptomListString)
                            {
                                string txt = item;
                                Font f = new Font(FontFamily.GenericSansSerif, 8.2f, FontStyle.Regular);
                                Bitmap bp = new Bitmap(1, 1);
                                Graphics gp = Graphics.FromImage(bp);
                                int height = (int)gp.MeasureString(txt, f, symptomWidth).Height;
                                symptomHeight.Add(height - 15);
                            }

                            if (currentSystemNumber != 0 && currentSystemNumber % 3 == 0)
                            {
                                if (symptomHeight.Count > 0)
                                    symptomMaxHeight.Add(symptomHeight.Max());

                                objPanelTable.Controls.Add(objPanelTableRow);
                                objPanelTableRow = new TableRow();

                                groupBox.Clear();
                                symptomCountList.Clear();
                                symptomHeight.Clear();
                                currentSystemNumber = 0;
                            }

                        }

                        if (currentSystemNumber < 3)
                        {
                            if (symptomHeight.Count > 0)
                                symptomMaxHeight.Add(symptomHeight.Max());
                            symptomHeight.Clear();
                            currentSystemNumber = 0;
                            objPanelTable.Controls.Add(objPanelTableRow);
                        }
                        // this.pnlReviewOfQuestionnaire.Controls.Add(objPanelTable);
                        Session["ScreenControls"] = objPanelTable;
                        Session["Version"] = HashVersion;
                        Session["HealthQuestionnaireID"] = HashHealthQuestionnaireID;
                        Session["QuestionnaireLookupID"] = HashQuestionnaireLookupID;
                    }
                    if (currentSystemNumber > 0)
                        groupBox.Clear();
                }
            }
        }


        private string arrayListToString(string[] arrayList)
        {
            string strFinal = string.Empty;
            for (int i = 0; i <= arrayList.Count() - 1; i++)
            {
                if (i > 0)
                    strFinal += "|";
                strFinal += arrayList[i].ToString();
            }
            return strFinal;
        }
        private void systemGroupBoxCreation(string systemName)
        {
            Panel gb = new Panel();
            gb.ID = "gb_" + systemName;
            gb.GroupingText = systemName;
            gb.Font.Name = FontFamily.GenericSansSerif.ToString();
            gb.Font.Size = new FontUnit("8.5pt");

            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(gb);
            objPanelTableCell.VerticalAlign = VerticalAlign.Top;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            groupBox.Add(gb);
        }



        public void checkBoxAndOtherControlsCreation(IList<int> noOfSymptomRows, IList<int> symptomMaxHeight)
        {
            var tempNoOfSymptomRows = noOfSymptomRows.Select((val, index) => new { Index = index, Value = val }).GroupBy(x => x.Index / 3).Select(x => x.Select(v => v.Value).ToList()).ToList();

            for (int i = 0; i < pnlReviewOfQuestionnaire.Controls.Count; i++)
            {
                if (pnlReviewOfQuestionnaire.Controls[i].GetType().ToString().Contains("Table") == true)
                {
                    int co = 0;

                    foreach (TableRow row in ((Table)pnlReviewOfQuestionnaire.Controls[i]).Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                            DynamicCheckBoxCreation((Panel)cell.Controls[0], ((Panel)cell.Controls[0]).GroupingText, symptomNamesLookUp.Where(n => n.Questionnaire_Type == (((Panel)cell.Controls[0]).GroupingText)).Select(s => s.Question).ToList(), tempNoOfSymptomRows[co].Max(), symptomMaxHeight.Count > 0 ? symptomMaxHeight[co] + 15 : 15);
                        co++;
                    }
                }
            }
        }

        private void DynamicCheckBoxCreation(Panel grp, string system, IList<string> sympNames, int maxSymptomRow, int symptomMaxHeight)
        {
            IList<string> symptomListString = new List<string>();
            IList<string> symptomStatusListString = new List<string>();

            if (grp.Controls != null)
                grp.Controls.Clear();
            if (roqList != null)
            {
                if (roqList.Count == 0)
                    symptomNames = sympNames;
                else
                {
                    symptomListString = roqList.Where(s => s.Questionnaire_Type == grp.GroupingText).Select(s => s.Question).ToList();
                    symptomStatusListString = roqList.Where(s => s.Questionnaire_Type == grp.GroupingText).Select(s => s.Selected_Option).ToList();
                }
            }


            Table objPanelTable = new Table();
            objPanelTable.ID = "table_" + grp.GroupingText;
            objPanelTableRow = new TableRow();

            objPanelTableCell = new TableCell();

            objPanelTableCell.Width = symptomWidth;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblAll = new Label();
            lblAll.ID = "lblAll" + grp.GroupingText;
            lblAll.Text = "All";
            lblAll.EnableViewState = false;
            //lblAll.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lblAll.Font.Size = new FontUnit("8.5pt");
            lblAll.Attributes.Add("class", "Editabletxtbox");
            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = 10;

            objPanelTableCell.Controls.Add(lblAll);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblOthers = new Label();
            lblOthers.ID = "lblOthers" + grp.GroupingText;
            lblOthers.Text = "Others";
            lblOthers.EnableViewState = false;
            //lblOthers.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lblOthers.Font.Size = new FontUnit("8.5pt");
            lblOthers.Attributes.Add("class", "Editabletxtbox");
            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = 10;

            objPanelTableCell.Controls.Add(lblOthers);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();
            objPanelTableCell.Width = symptomWidth;

            objPanelTableRow.Controls.Add(objPanelTableCell);

            if (symptomNames != null && (symptomNames.Count > 1 || symptomListString.Count > 1))
            {
                //Label lblAll1 = new Label();
                ////lblAll1.ID = "lblAll1" + grp.GroupingText;
                //lblAll1.ID = "lblAllNew" + grp.GroupingText;
                //lblAll1.Text = "All";
                //lblAll1.EnableViewState = false;
                //lblAll1.Font.Name = FontFamily.GenericSansSerif.ToString();
                //lblAll1.Font.Size = new FontUnit("8.5pt");
                //objPanelTableCell = new TableCell();
                //objPanelTableCell.Width = 10;


                Label lblAllNew = new Label();
                lblAllNew.ID = "lblAllNew" + grp.GroupingText;
                lblAllNew.Text = "All";
                lblAllNew.EnableViewState = false;
                //lblAllNew.Font.Name = FontFamily.GenericSansSerif.ToString();
                //lblAllNew.Font.Size = new FontUnit("8.5pt");
                lblAllNew.Attributes.Add("class", "Editabletxtbox");
                objPanelTableCell = new TableCell();
                objPanelTableCell.Width = 10;
                objPanelTableCell.Controls.Add(lblAllNew);
                objPanelTableRow.Controls.Add(objPanelTableCell);

                Label lblOthers1 = new Label();
                //lblOthers1.ID = "lblOthers1" + grp.GroupingText;
                lblOthers1.ID = "lblOthersNew" + grp.GroupingText;
                lblOthers1.Text = "Others";
                lblOthers1.EnableViewState = false;
                //lblOthers1.Font.Name = FontFamily.GenericSansSerif.ToString();
                //lblOthers1.Font.Size = new FontUnit("8.5pt");
                lblOthers1.Attributes.Add("class", "Editabletxtbox");
                objPanelTableCell = new TableCell();
                objPanelTableCell.Width = 10;

                objPanelTableCell.Controls.Add(lblOthers1);
                objPanelTableRow.Controls.Add(objPanelTableCell);
            }
            objPanelTable.Controls.Add(objPanelTableRow);

            objPanelTableRow = new TableRow();
            objPanelTableCell = new TableCell();

            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblYesAll1 = new Label();
            lblYesAll1.Text = "Y";
            lblYesAll1.ID = "lblYes_" + grp.GroupingText + "_1";
            lblYesAll1.EnableViewState = false;
            //lblYesAll1.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lblYesAll1.Font.Size = new FontUnit("8.5pt");
            lblYesAll1.Attributes.Add("class", "Editabletxtbox");
            objPanelTableCell = new TableCell();

            objPanelTableCell.Controls.Add(lblYesAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            Label lblNoAll1 = new Label();
            lblNoAll1.Text = "N";
            lblNoAll1.ID = "lblNo_" + grp.GroupingText + "_1";
            lblNoAll1.EnableViewState = false;
            //lblNoAll1.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lblNoAll1.Font.Size = new FontUnit("8.5pt");
            lblNoAll1.Attributes.Add("class", "Editabletxtbox");
            objPanelTableCell = new TableCell();

            objPanelTableCell.Controls.Add(lblNoAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();

            objPanelTableRow.Controls.Add(objPanelTableCell);
            if (symptomNames != null && (symptomNames.Count > 1 || symptomListString.Count > 1))
            {
                Label lblYesAll2 = new Label();
                lblYesAll2.Text = "Y";
                lblYesAll2.ID = "lblYes_" + grp.GroupingText + "_2";
                lblYesAll2.EnableViewState = false;
                //lblYesAll2.Font.Name = FontFamily.GenericSansSerif.ToString();
                //lblYesAll2.Font.Size = new FontUnit("8.5pt");
                lblYesAll2.Attributes.Add("class", "Editabletxtbox");
                objPanelTableCell = new TableCell();

                objPanelTableCell.Controls.Add(lblYesAll2);
                objPanelTableRow.Controls.Add(objPanelTableCell);

                Label lblNoAll2 = new Label();
                lblNoAll2.Text = "N";
                lblNoAll2.ID = "lblNo_" + grp.GroupingText + "_2";
                lblNoAll2.EnableViewState = false;
                //lblNoAll2.Font.Name = FontFamily.GenericSansSerif.ToString();
                //lblNoAll2.Font.Size = new FontUnit("8.5pt");
                lblNoAll2.Attributes.Add("class", "Editabletxtbox");
                objPanelTableCell = new TableCell();

                objPanelTableCell.Controls.Add(lblNoAll2);
                objPanelTableRow.Controls.Add(objPanelTableCell);
            }

            objPanelTable.Controls.Add(objPanelTableRow);

            objPanelTableRow = new TableRow();
            objPanelTableCell = new TableCell();

            objPanelTableRow.Controls.Add(objPanelTableCell);

            CheckBox chkYesAll1 = new CheckBox();
            chkYesAll1.ID = "chkYes_" + grp.GroupingText + "_All";
            chkYesAll1.EnableViewState = false;
            chkYesAll1.Attributes.Add("OnClick", "checkStateWhenAllSymptomsCheckBoxChecked('" + chkYesAll1.ID + "')");
            objPanelTableCell = new TableCell();

            objPanelTableCell.Controls.Add(chkYesAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            CheckBox chkNoAll1 = new CheckBox();
            chkNoAll1.ID = "chkNo_" + grp.GroupingText + "_All";
            chkNoAll1.EnableViewState = false;
            chkNoAll1.Attributes.Add("OnClick", "checkStateWhenAllSymptomsCheckBoxChecked('" + chkNoAll1.ID + "')");
            objPanelTableCell = new TableCell();

            objPanelTableCell.Controls.Add(chkNoAll1);
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTableCell = new TableCell();

            objPanelTableRow.Controls.Add(objPanelTableCell);


            objPanelTableCell = new TableCell();

            objPanelTableRow.Controls.Add(objPanelTableCell);


            objPanelTableCell = new TableCell();

            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTable.Controls.Add(objPanelTableRow);

            if (symptomNames != null && symptomNames.Count > 0)
                symptomLabelsAndCheckBox(grp, objPanelTable, symptomNames, symptomStatusListString, maxSymptomRow, symptomMaxHeight);

            if (symptomListString != null && symptomListString.Count > 0)
            {
                IList<string> symtpom = new List<string>();

                for (int j = 0; j < symptomListString.Count; j++)
                    symtpom.Add(symptomListString[j]);

                symptomLabelsAndCheckBox(grp, objPanelTable, symtpom, symptomStatusListString, maxSymptomRow, symptomMaxHeight);


            }
            grp.Controls.Add(objPanelTable);

        }

        private void symptomLabelsAndCheckBox(Panel grp, Table objPanelTable, IList<string> symptoms, IList<string> symptomsStatus, int maxSymptomRow, int symptomMaxHeight)
        {
            if (symptoms != null)
            {
                Label grwLbl;
                CheckBox chkYes;
                CheckBox chkNo;

                for (int i = 0; i < symptoms.Count; i++)
                {
                    objPanelTableRow = new TableRow();

                    grwLbl = new Label();
                    grwLbl.Text = symptoms[i];
                    grwLbl.EnableViewState = false;
                    grwLbl.ID = ("lblSymptom_" + grp.GroupingText + "_" + grwLbl.Text).Replace(":","");
                    //grwLbl.Font.Name = FontFamily.GenericSansSerif.ToString();
                    //grwLbl.Font.Size = new FontUnit("8.5pt");
                    grwLbl.Attributes.Add("class", "Editabletxtbox");
                    grwLbl.Height = symptomMaxHeight;
                    objPanelTableCell = new TableCell();
                    objPanelTableCell.Controls.Add(grwLbl);

                    objPanelTableRow.Controls.Add(objPanelTableCell);

                    chkYes = new CheckBox();
                    chkYes.ID = ("chkYes_" + grp.GroupingText + "_" + grwLbl.Text).Replace(":", "");
                    chkYes.EnableViewState = false;
                    chkYes.Checked = symptomsStatus.Count > 0 && symptomsStatus[i].ToUpper() == "YES" ? true : false;
                    chkYes.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkYes.ID + "')");
                    objPanelTableCell = new TableCell();
                    objPanelTableCell.Controls.Add(chkYes);

                    objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                    objPanelTableRow.Controls.Add(objPanelTableCell);

                    chkNo = new CheckBox();
                    chkNo.ID = ("chkNo_" + grp.GroupingText + "_" + grwLbl.Text).Replace(":", "");
                    chkNo.EnableViewState = false;
                    chkNo.Checked = symptomsStatus.Count > 0 && symptomsStatus[i].ToUpper() == "NO" ? true : false;
                    chkNo.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkNo.ID + "')");
                    objPanelTableCell = new TableCell();
                    objPanelTableCell.Controls.Add(chkNo);

                    objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                    objPanelTableRow.Controls.Add(objPanelTableCell);

                    i++;

                    if (i < symptoms.Count)
                    {
                        grwLbl = new Label();
                        grwLbl.Text = symptoms[i];
                        grwLbl.EnableViewState = false;
                        grwLbl.ID = ("lblSymptom_" + grp.GroupingText + "_" + grwLbl.Text).Replace(":", "");
                        //grwLbl.Font.Name = FontFamily.GenericSansSerif.ToString();
                        //grwLbl.Font.Size = new FontUnit("8.5pt");
                        grwLbl.Attributes.Add("class", "Editabletxtbox");
                        grwLbl.Height = symptomMaxHeight;
                        objPanelTableCell = new TableCell();
                        objPanelTableCell.Controls.Add(grwLbl);

                        objPanelTableRow.Controls.Add(objPanelTableCell);

                        chkYes = new CheckBox();
                        chkYes.ID = ("chkYes_" + grp.GroupingText + "_" + grwLbl.Text).Replace(":", "");
                        chkYes.EnableViewState = false;
                        chkYes.Checked = symptomsStatus.Count > 0 && symptomsStatus[i].ToUpper() == "YES" ? true : false;
                        chkYes.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkYes.ID + "')");
                        objPanelTableCell = new TableCell();
                        objPanelTableCell.Controls.Add(chkYes);

                        objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                        objPanelTableRow.Controls.Add(objPanelTableCell);

                        chkNo = new CheckBox();
                        chkNo.ID = ("chkNo_" + grp.GroupingText + "_" + grwLbl.Text).Replace(":", "");
                        chkNo.EnableViewState = false;
                        chkNo.Checked = symptomsStatus.Count > 0 && symptomsStatus[i].ToUpper() == "NO" ? true : false;
                        chkNo.Attributes.Add("OnClick", "chkYesNoToggleStateChanged('" + chkNo.ID + "')");
                        objPanelTableCell = new TableCell();
                        objPanelTableCell.Controls.Add(chkNo);

                        objPanelTableCell.VerticalAlign = VerticalAlign.Top;
                        objPanelTableRow.Controls.Add(objPanelTableCell);
                    }

                    objPanelTable.Controls.Add(objPanelTableRow);
                }
            }

            int tempNoOfSymptoms = symptoms.Count % 2 == 0 ? (int)(symptoms.Count / 2) : (int)(symptoms.Count / 2) + 1;
            int length = maxSymptomRow - tempNoOfSymptoms;

            for (int i = 0; i < length; i++)
            {
                objPanelTableRow = new TableRow();
                Label lblSpace2 = new Label();
                lblSpace2.ID = "lblSpace2_" + i.ToString() + "_" + grp.GroupingText;
                lblSpace2.Height = symptomMaxHeight;
                lblSpace2.EnableViewState = false;
                objPanelTableCell = new TableCell();
                objPanelTableCell.Controls.Add(lblSpace2);
                objPanelTableCell.ColumnSpan = 6;

                objPanelTableRow.Controls.Add(objPanelTableCell);
                objPanelTable.Controls.Add(objPanelTableRow);
            }


            GroupBoxOtherControlsCreation(grp, objPanelTable);
        }

        private void GroupBoxOtherControlsCreation(Panel grp, Table objPanelTable)
        {
            objPanelTableRow = new TableRow();

            Label lbl = new Label();
            lbl.ID = "lbl_" + grp.GroupingText;
            lbl.Text = "Notes";
            lbl.EnableViewState = false;
            //lbl.BackColor = Color.White;
            //lbl.Font.Name = FontFamily.GenericSansSerif.ToString();
            //lbl.Font.Size = new FontUnit("8.5pt");
            lbl.Attributes.Add("class", "Editabletxtbox");



            objPanelTableRow = new TableRow();
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(lbl);
            objPanelTableRow.Controls.Add(objPanelTableCell);
            objPanelTable.Controls.Add(objPanelTableRow);



            objPanelTableRow = new TableRow();

            CustomDLCNew dlc = (CustomDLCNew)LoadControl("~/UserControls/customDLCNew.ascx");
            dlc.ID = "dlc_" + grp.GroupingText.Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", "");
            dlc.TextboxHeight = new Unit("40px");
            dlc.TextboxWidth = new Unit("395px");
            dlc.Value = "NOTES FOR " + grp.GroupingText.ToUpper();
            //new
            dlc.txtDLC.Attributes.Add("UserRole", ClientSession.UserRole);
            //End new
            if (roqList != null && roqList.Count > 0)
            {
                var gnrlNotes = (from notes in roqList
                                 where notes.Questionnaire_Type == grp.GroupingText
                                 select notes);
                if (gnrlNotes != null && gnrlNotes.Count() != 0)
                {
                    foreach (var gnr in gnrlNotes)
                    {
                        dlc.txtDLC.Text = gnr.Notes;
                    }
                }
            }
            dlc.txtDLC.Attributes.Add("onkeypress", "EnableSave(event);");
            dlc.txtDLC.Attributes.Add("onchange", "EnableSave(event);");
            dlc.txtDLC.Attributes.Add("class", "pbDropdownBackground");
            objPanelTableCell = new TableCell();
            objPanelTableCell.Controls.Add(dlc);
            objPanelTableCell.ColumnSpan = 6;
            objPanelTableRow.Controls.Add(objPanelTableCell);

            objPanelTable.Controls.Add(objPanelTableRow);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            checkAndInsert();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('118501');", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('118501');top.window.document.getElementById('ctl00_Loading').style.display = 'none';", true);
            //divLoading.Style.Add("display", "none");

        }
        private void checkAndInsert()
        {
            IList<Healthcare_Questionnaire> roqListOfSystemAndSymptom = new List<Healthcare_Questionnaire>();
            Healthcare_Questionnaire roqObjSystemAndSymptom = null;
            CheckBox chkYesSymptom = new CheckBox();
            CheckBox chkNoSymptom = new CheckBox();
            Panel gr = new Panel();
            RadTextBox txt = new RadTextBox();
            CheckBox chkSystemAndSymptom = new CheckBox();
            Panel grSystemAndSymptom = new Panel();
            IList<Healthcare_Questionnaire> roqListToInsert = new List<Healthcare_Questionnaire>();
            IList<Healthcare_Questionnaire> roqListToUpdate = new List<Healthcare_Questionnaire>();
            IList<string> strListCheckBoxSymptomName = new List<string>();
            IList<string> strListUnchecked = new List<string>();
            IList<string> strListChecked = new List<string>();
            HealthcareQuestionnaireManager healthMngr = new HealthcareQuestionnaireManager();

            int symptomGrowLabelCount = 0;
            Label grwSymptom = new Label();

            IList<Healthcare_Questionnaire> roqListOfTypeAndQuestion = new List<Healthcare_Questionnaire>();

            bool insertFlag = false;
            bool updateFlag = false;
            string strStatus = string.Empty;
            if (roqList != null)
            {
                if (roqList.Count == 0)
                {
                    for (int i = 0; i < questionNames.Count; i++)
                    {
                        grSystemAndSymptom = (Panel)pnlReviewOfQuestionnaire.FindControl("gb_" + questionNames[i]);

                        string[] systemNameListString = systemNames.Select(s => (s.Questionnaire_Type)).ToArray();
                        symptomNamesLookUp = systemNames.Where(n => n.Questionnaire_Type == (questionNames[i])).ToList<FillHealthcareQuestionnaire>();
                        questionNames = systemNames.Select(a => a.Questionnaire_Type).Distinct().ToList<string>();

                        chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_All");

                        if (chkSystemAndSymptom.Checked == false)
                            strListUnchecked.Add(chkSystemAndSymptom.ID);
                        else
                            strListChecked.Add(chkSystemAndSymptom.ID);

                        chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_All");
                        if (chkSystemAndSymptom.Checked == false)
                            strListUnchecked.Add(chkSystemAndSymptom.ID);
                        else
                            strListChecked.Add(chkSystemAndSymptom.ID);

                        for (int j = 0; j < symptomNamesLookUp.Count; j++)
                        {
                            grwSymptom = (Label)pnlReviewOfQuestionnaire.FindControl(("lblSymptom_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUp[j].Question).Replace(":", ""));
                            symptomGrowLabelCount++;
                            roqObjSystemAndSymptom = new Healthcare_Questionnaire();
                            roqObjSystemAndSymptom.Questionnaire_Type = grSystemAndSymptom.GroupingText;
                            roqObjSystemAndSymptom.Question = grwSymptom.Text;
                            roqListOfSystemAndSymptom.Add(roqObjSystemAndSymptom);

                            chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl(("chkYes_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUp[j].Question).Replace(":", ""));
                            if (chkSystemAndSymptom.Checked == false)
                                strListUnchecked.Add(chkSystemAndSymptom.ID);
                            else
                                strListChecked.Add(chkSystemAndSymptom.ID);

                            chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl(("chkNo_" + grSystemAndSymptom.GroupingText + "_" + symptomNamesLookUp[j].Question).Replace(":", ""));
                            if (chkSystemAndSymptom.Checked == false)
                                strListUnchecked.Add(chkSystemAndSymptom.ID);
                            else
                                strListChecked.Add(chkSystemAndSymptom.ID);
                        }


                    }

                }
                else
                {
                    IList<string> systemListString = new List<string>();
                    IList<string> symptomListString = new List<string>();
                    string sPatientSex = "";

                    if (ClientSession.PatientPaneList != null && ClientSession.PatientPaneList.Count > 0)
                    {
                        sPatientSex = ClientSession.PatientPaneList[0].Sex;
                    }
                    systemListString = roqList.Select(r => r.Questionnaire_Type).Distinct().ToList();
                    if (sPatientSex.ToUpper() == "MALE")
                        systemListString = systemListString.Where(a => a.ToUpper() != "GYNECOLOGICAL FEMALE").ToList<string>();

                    for (int i = 0; i < systemListString.Count; i++)
                    {
                        grSystemAndSymptom = (Panel)pnlReviewOfQuestionnaire.FindControl("gb_" + systemListString[i]);

                        symptomListString = roqList.Where(r => r.Questionnaire_Type == systemListString[i]).Select(s => s.Question).ToList();

                        chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl("chkYes_" + grSystemAndSymptom.GroupingText + "_All");
                        if (chkSystemAndSymptom.Checked == false)
                            strListUnchecked.Add(chkSystemAndSymptom.ID);
                        else
                            strListChecked.Add(chkSystemAndSymptom.ID);

                        chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl("chkNo_" + grSystemAndSymptom.GroupingText + "_All");
                        if (chkSystemAndSymptom.Checked == false)
                            strListUnchecked.Add(chkSystemAndSymptom.ID);
                        else
                            strListChecked.Add(chkSystemAndSymptom.ID);

                        for (int j = 0; j < symptomListString.Count; j++)
                        {
                            grwSymptom = (Label)pnlReviewOfQuestionnaire.FindControl(("lblSymptom_" + grSystemAndSymptom.GroupingText + "_" + symptomListString[j]).Replace(":", ""));
                            symptomGrowLabelCount++;
                            roqObjSystemAndSymptom = new Healthcare_Questionnaire();
                            roqObjSystemAndSymptom.Questionnaire_Type = grSystemAndSymptom.GroupingText;
                            roqObjSystemAndSymptom.Question = grwSymptom.Text;
                            roqListOfSystemAndSymptom.Add(roqObjSystemAndSymptom);

                            chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl(("chkYes_" + grSystemAndSymptom.GroupingText + "_" + symptomListString[j]).Replace(":", ""));
                            if (chkSystemAndSymptom.Checked == false)
                                strListUnchecked.Add(chkSystemAndSymptom.ID);
                            else
                                strListChecked.Add(chkSystemAndSymptom.ID);

                            chkSystemAndSymptom = (CheckBox)pnlReviewOfQuestionnaire.FindControl(("chkNo_" + grSystemAndSymptom.GroupingText + "_" + symptomListString[j]).Replace(":", ""));
                            if (chkSystemAndSymptom.Checked == false)
                                strListUnchecked.Add(chkSystemAndSymptom.ID);
                            else
                                strListChecked.Add(chkSystemAndSymptom.ID);

                        }



                    }
                }
            }


            if (roqListOfSystemAndSymptom != null && symptomGrowLabelCount != null && symptomGrowLabelCount == roqListOfSystemAndSymptom.Count)
            {
                for (int j = 0; j < roqListOfSystemAndSymptom.Count; j++)
                {
                    strStatus = string.Empty;
                    Control grpctrl = pnlReviewOfQuestionnaire.FindControl("gb_" + roqListOfSystemAndSymptom[j].Questionnaire_Type);
                    gr = (Panel)grpctrl;
                    Control chkControlYes = gr.FindControl(("chkYes_" + roqListOfSystemAndSymptom[j].Questionnaire_Type + "_" + roqListOfSystemAndSymptom[j].Question).Replace(":", ""));
                    chkYesSymptom = (CheckBox)chkControlYes;
                    string[] strCheckStatusYes = chkYesSymptom.ID.Split('_');

                    if (chkYesSymptom.Checked == false)
                    {
                        Control chkControlNo = gr.FindControl(("chkNo_" + roqListOfSystemAndSymptom[j].Questionnaire_Type + "_" + roqListOfSystemAndSymptom[j].Question).Replace(":", ""));
                        chkNoSymptom = (CheckBox)chkControlNo;
                        string[] strCheckStatusNo = chkNoSymptom.ID.Split('_');

                        if (chkNoSymptom.Checked == false)
                        {
                            strStatus = " ";
                            if (roqList != null && roqList.Count > 0)
                            {
                                insertFlag = false;
                                updateFlag = true;
                            }
                            else
                            {
                                insertFlag = true;
                                updateFlag = false;
                            }
                        }
                        else
                        {
                            strStatus = "No";
                            if (roqList != null && roqList.Count > 0)
                            {
                                insertFlag = false;
                                updateFlag = true;
                            }
                            else
                            {
                                insertFlag = true;
                                updateFlag = false;
                            }
                        }
                    }
                    else
                    {
                        strStatus = "Yes";
                        if (roqList != null && roqList.Count > 0)
                        {
                            insertFlag = false;
                            updateFlag = true;
                        }
                        else
                        {
                            insertFlag = true;
                            updateFlag = false;
                        }
                    }
                    if (insertFlag == true)
                    {
                        Healthcare_Questionnaire roqobj = new Healthcare_Questionnaire();
                        roqobj.Encounter_ID = ClientSession.EncounterId;
                        roqobj.Human_ID = ClientSession.HumanId;
                        roqobj.Physician_ID = ClientSession.PhysicianId;
                        roqobj.Questionnaire_Category = sMyCategory;
                        roqobj.Questionnaire_Type = roqListOfSystemAndSymptom[j].Questionnaire_Type;
                        roqobj.Question = roqListOfSystemAndSymptom[j].Question;
                        roqobj.Questionnaire_Lookup_ID = Convert.ToUInt64(HashQuestionnaireLookupID[roqListOfSystemAndSymptom[j].Question.ToString() + "-" + roqListOfSystemAndSymptom[j].Questionnaire_Type.ToString()]);
                        roqobj.Notes = ((CustomDLCNew)pnlReviewOfQuestionnaire.FindControl("dlc_" + (roqListOfSystemAndSymptom[j].Questionnaire_Type.ToString()).Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", ""))).txtDLC.Text;
                        roqobj.Selected_Option = strStatus;
                        roqobj.Created_By = ClientSession.UserName;
                        roqobj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        roqListToInsert.Add(roqobj);
                        roqList[j].Created_By = roqobj.Created_By;
                        roqList[j].Created_Date_And_Time = roqobj.Created_Date_And_Time;
                    }
                    else if (updateFlag == true)
                    {
                        Healthcare_Questionnaire roqobj = new Healthcare_Questionnaire();
                        roqobj.Encounter_ID = ClientSession.EncounterId;
                        roqobj.Human_ID = ClientSession.HumanId;
                        roqobj.Physician_ID = ClientSession.PhysicianId;
                        roqobj.Questionnaire_Category = sMyCategory;
                        roqobj.Questionnaire_Type = roqListOfSystemAndSymptom[j].Questionnaire_Type;
                        roqobj.Question = roqListOfSystemAndSymptom[j].Question;
                        roqobj.Id = Convert.ToUInt64(HashHealthQuestionnaireID[roqListOfSystemAndSymptom[j].Question.ToString() + "-" + roqListOfSystemAndSymptom[j].Questionnaire_Type.ToString()]);
                        roqobj.Questionnaire_Lookup_ID = Convert.ToUInt64(HashQuestionnaireLookupID[roqListOfSystemAndSymptom[j].Question.ToString() + "-" + roqListOfSystemAndSymptom[j].Questionnaire_Type.ToString()]);
                        roqobj.Version = Convert.ToInt32(HashVersion[roqListOfSystemAndSymptom[j].Question.ToString() + "-" + roqListOfSystemAndSymptom[j].Questionnaire_Type.ToString()]);
                        roqobj.Notes = ((CustomDLCNew)pnlReviewOfQuestionnaire.FindControl("dlc_" + (roqListOfSystemAndSymptom[j].Questionnaire_Type.ToString().Replace(" ", "").Replace("?", "").Replace("(", "").Replace(")", "").Replace(",", "")))).txtDLC.Text;
                        roqobj.Selected_Option = strStatus;
                        roqobj.Modified_By = ClientSession.UserName;
                        roqobj.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        roqobj.Created_By = roqListOfSystemAndSymptom[j].Created_By;
                        roqobj.Created_Date_And_Time = roqListOfSystemAndSymptom[j].Created_Date_And_Time;
                        roqListToUpdate.Add(roqobj);
                        roqList[j].Modified_By = roqobj.Modified_By;
                        roqList[j].Modified_Date_And_Time = roqobj.Modified_Date_And_Time;
                    }
                }

                if (insertFlag == true)
                {
                    if (Session["fillRoq"] != null)
                    {
                        Session["fillRoq"] = healthMngr.AppendHealthQuestion(roqListToInsert.ToArray(), DateTime.MinValue, DateTime.MinValue, string.Empty, false, roqList);
                        //rsmReviewOfQuestionnaire.AsyncPostBackErrorMessage = "true";
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "SavedSuccessfully();", true);
                        btnSave.Enabled = false;
                    }
                }
                else if (updateFlag == true)
                {
                    if (Session["fillRoq"] != null)
                    {
                        Session["fillRoq"] = healthMngr.UpdateHealthQuestionforRos(roqListToUpdate.ToArray(), DateTime.MinValue, DateTime.MinValue, string.Empty, false, roqList);
                        //rsmReviewOfQuestionnaire.AsyncPostBackErrorMessage = "true";
                        ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "SavedSuccessfully();", true);
                        btnSave.Enabled = false;
                    }
                }
            }
        }

        protected void CopyPreviousEncounter(bool bsaved)
        {
            QuestionnaireLookupManager RoqMngr = new QuestionnaireLookupManager();
            HealthcareQuestionnaireManager questionnaireManager = new HealthcareQuestionnaireManager();
            IList<FillHealthcareQuestionnaire> questionnaireDTO = new List<FillHealthcareQuestionnaire>();
            bool isPhysicianProcess = false;
            bool isPrevEncQuestionnairePresent = false;
            bool isPrevEncPresent = false;
            bool isFromArchive = false;
            ulong prevEncID = 0;
            bool isAlert = false;
            noOfSymptomRows.Clear();

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
                roqList = (IList<FillHealthcareQuestionnaire>)Session["fillRoq"];

                groupBoxCreationCopyPrevious(roqList, noOfSymptomRows, symptomMaxHeight);

                checkBoxAndOtherControlsCreation(noOfSymptomRows, symptomMaxHeight);

                chkAllOtherSystemsNormal.Attributes.Add("OnClick", "chkAllOtherSystemsNormalClick('" + systemNamesForEvent + "')");

                for (int k = 0; k < roqList.Count; k++)
                {
                    HashHealthQuestionnaireID[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].Questionnaire_Lookup_ID;
                    HashVersion[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].Version;
                }
            }
            else
            {
                roqList = RoqMngr.GetHealthQuestionLookupListFromServer(sMyCategory, PatientDOB, dtpAppointmentDate, ClientSession.PhysicianUserName, prevEncID, false);

                groupBoxCreationCopyPrevious(roqList, noOfSymptomRows, symptomMaxHeight);

                checkBoxAndOtherControlsCreation(noOfSymptomRows, symptomMaxHeight);

                chkAllOtherSystemsNormal.Attributes.Add("OnClick", "chkAllOtherSystemsNormalClick('" + systemNamesForEvent + "')");

                for (int k = 0; k < roqList.Count; k++)
                {
                    HashHealthQuestionnaireID[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].HealthCare_Questionnaire_ID;
                    HashQuestionnaireLookupID[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].Questionnaire_Lookup_ID;
                    HashVersion[roqList[k].Question.ToString() + "-" + roqList[k].Questionnaire_Type.ToString()] = roqList[k].Version;
                }
                btnSave.Enabled = true;
            }

            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Login", "sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();", true);
        }
        protected void btnCopyPrevious_Click(object sender, EventArgs e)
        {
            CopyPreviousEncounter(false);
        }


    }
}
