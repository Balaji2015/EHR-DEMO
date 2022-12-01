using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.WebControls;
using Telerik.Web;
using Telerik.Web.UI.Grid;
using Telerik.Web.UI;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Acurus.Capella.DataAccess;


namespace Acurus.Capella.UI
{
    public partial class frmClinicalInformation : System.Web.UI.Page
    {
        IList<ProblemList> lstProblem = new List<ProblemList>();
        IList<Rcopia_Allergy> lstAllergy = new List<Rcopia_Allergy>();
        IList<Rcopia_Medication> lstMedication = new List<Rcopia_Medication>();

        IList<ProblemList> lstIncorporatedProblem = new List<ProblemList>();
        IList<Rcopia_Allergy> lstIncorporatedAllergy = new List<Rcopia_Allergy>();
        IList<Rcopia_Medication> lstIncorporatedMedication = new List<Rcopia_Medication>();

        IList<ProblemList> lstMergedProblem = new List<ProblemList>();
        IList<Rcopia_Allergy> lstMergedAllergy = new List<Rcopia_Allergy>();
        IList<Rcopia_Medication> lstMergedMedication = new List<Rcopia_Medication>();

        ulong ulHumanID = 0;
        DateTime dtDateofService = DateTime.MinValue;
        string pat_DOV = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["HumanID"] != null && Request["HumanID"] != "")
            {
                ulHumanID = Convert.ToUInt64(Request["HumanID"]);
                ClientSession.HumanId = ulHumanID;
            }
            else
                ulHumanID = ClientSession.HumanId;
            if (Request["DateofService"] != null && Request["DateofService"] != string.Empty)
                dtDateofService = UtilityManager.ConvertToUniversal(Convert.ToDateTime(Request["DateofService"]));

            Session["HumanId"] = ulHumanID;
            if (!IsPostBack)
            {
                setThePatientDetails(ulHumanID);

                ProblemListManager obj_problemMgr = new ProblemListManager();
                lstProblem = obj_problemMgr.GetFromProblemListClinicalInformation(ulHumanID, string.Empty, true);
                IList<ProblemList> tempprob = (from doc in lstProblem where doc.Is_Active == "Y" select doc).ToList<ProblemList>();
                Session["ActualProblemList"] = tempprob;

                Rcopia_AllergyManager objRcopiaAllergy = new Rcopia_AllergyManager();
                lstAllergy = objRcopiaAllergy.GetAllergyListUsingHumanId(ulHumanID);
                Session["ActualAllergyList"] = lstAllergy;

                Rcopia_MedicationManager objMedicationAllergy = new Rcopia_MedicationManager();
                lstMedication = objMedicationAllergy.GetRCopiaMedByHumanID(ulHumanID);
                Session["ActualMedicationList"] = lstMedication;

                LoadGridProblemActualList(tempprob);
                LoadGridAllergyActualList(lstAllergy);
                LoadGridMedicationActualList(lstMedication);

                if (Request["XMLPath"] != null && Request["XMLPath"] != string.Empty)
                {
                    btnLoad_Click(new Object(), EventArgs.Empty);
                    //Panel1.Enabled = false;
                    Panel1.Visible = false;
                    UploadImage.Enabled = false;
                }
                else
                {
                    LoadEmptyGrid();
                }
            }
        }

        private void setThePatientDetails(ulong humanID)
        {
            if (humanID != 0)
            {
                HumanManager objhumanmanager = new HumanManager();
                IList<Human> humanList = null;
                humanList = objhumanmanager.GetPatientDetailsUsingPatientInformattion(humanID);
                if (humanList.Count > 0)
                {
                    txtHumanId.Text = humanID.ToString();
                    txtHumanName.Text = humanList[0].First_Name + " " + humanList[0].MI + " " + humanList[0].Last_Name;
                    txtSex.Text = humanList[0].Sex;
                    txtDOB.Text = humanList[0].Birth_Date.ToString("dd-MMM-yyyy");
                }
            }
        }



        public void LoadGridProblemActualList(IList<ProblemList> lstActualProblem)
        {
            if (lstActualProblem.Count > 0)
            {
                grdProblemActualList.DataSource = new string[] { };
                grdProblemActualList.DataBind();

                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Problem ID", typeof(string)));


                for (int i = 0; i < lstActualProblem.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Name"] = lstActualProblem[i].Problem_Description;
                    dr["Status"] = lstActualProblem[i].Status;
                    if (lstActualProblem[i].Modified_Date_And_Time == DateTime.MinValue)
                        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(lstActualProblem[i].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                    else
                        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(lstActualProblem[i].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                    dr["Problem ID"] = lstActualProblem[i].Id;
                    dt.Rows.Add(dr);
                }

                grdProblemActualList.DataSource = dt;
                grdProblemActualList.DataBind();
            }
            else
            {
                grdProblemActualList.DataSource = new string[] { };
                grdProblemActualList.DataBind();
            }
        }


        public void LoadGridAllergyActualList(IList<Rcopia_Allergy> lstActualAllergy)
        {
            if (lstActualAllergy.Count > 0)
            {
                grdAllergyActualList.DataSource = new string[] { };
                grdAllergyActualList.DataBind();

                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Reaction", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Rcopia Id", typeof(string)));


                for (int i = 0; i < lstActualAllergy.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Name"] = lstActualAllergy[i].Allergy_Name;
                    dr["Reaction"] = lstActualAllergy[i].Reaction;
                    if (lstActualAllergy[i].Deleted.ToUpper() == "N")
                        dr["Status"] = "Active";
                    else
                        dr["Status"] = "Inactive";
                    if (lstActualAllergy[i].Modified_Date_And_Time == DateTime.MinValue)
                        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(lstActualAllergy[i].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                    else
                        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(lstActualAllergy[i].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                    dr["Rcopia Id"] = lstActualAllergy[i].Id;
                    dt.Rows.Add(dr);
                }

                grdAllergyActualList.DataSource = dt;
                grdAllergyActualList.DataBind();
            }
            else
            {
                grdAllergyActualList.DataSource = new string[] { };
                grdAllergyActualList.DataBind();
            }
        }


        public void LoadGridMedicationActualList(IList<Rcopia_Medication> lstActualMedication)
        {
            if (lstActualMedication.Count > 0)
            {
                grdMedicationActualList.DataSource = new string[] { };
                grdMedicationActualList.DataBind();

                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Rcopia Id", typeof(string)));

                for (int i = 0; i < lstActualMedication.Count; i++)
                {
                    dr = dt.NewRow();
                    //dr["Name"] = lstActualMedication[i].Brand_Name + " " + lstActualMedication[i].Strength + " " + lstActualMedication[i].Form + " " + lstActualMedication[i].Route + " " + lstActualMedication[i].Dose + " " + lstActualMedication[i].Dose_Unit + " " + lstActualMedication[i].Dose_Timing + " " + lstActualMedication[i].Patient_Notes;
                    dr["Name"] = lstActualMedication[i].Brand_Name + " " + lstActualMedication[i].Strength + " " + lstActualMedication[i].Dose + " " + lstActualMedication[i].Dose_Unit + " " + lstActualMedication[i].Route + " " + lstActualMedication[i].Dose_Timing;
                    if (lstActualMedication[i].Start_Date != DateTime.MinValue && lstActualMedication[i].Stop_Date == DateTime.MinValue)
                        dr["Status"] = "Active";
                    else if (lstActualMedication[i].Start_Date == DateTime.MinValue && lstActualMedication[i].Stop_Date == DateTime.MinValue)
                        dr["Status"] = "Inactive";
                    else if ((lstActualMedication[i].Start_Date != DateTime.MinValue && lstActualMedication[i].Stop_Date != DateTime.MinValue) || (lstActualMedication[i].Start_Date == DateTime.MinValue && lstActualMedication[i].Stop_Date != DateTime.MinValue))
                    {
                        if (lstActualMedication[i].Stop_Date < UtilityManager.ConvertToLocal(DateTime.UtcNow))
                            dr["Status"] = "Inactive";
                        else
                            dr["Status"] = "Active";
                    }
                    if (lstActualMedication[i].Modified_Date_And_Time == DateTime.MinValue)
                        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(lstActualMedication[i].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                    else
                        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(lstActualMedication[i].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                    dr["Rcopia Id"] = lstActualMedication[i].Id;

                    dt.Rows.Add(dr);
                }

                grdMedicationActualList.DataSource = dt;
                grdMedicationActualList.DataBind();
            }
            else
            {
                grdMedicationActualList.DataSource = new string[] { };
                grdMedicationActualList.DataBind();
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            string sXMLPath = Request["XMLPath"];
            XmlElement medicationSection = null;
            if (UploadImage.UploadedFiles.Count > 0 || (sXMLPath != null && sXMLPath != string.Empty))
            {
                // DataSet dsAllergy = null;
                DataSet dsMedication = null;
                // DataSet dsProblem = null;
                DataSet dsEncounter = null;

                if (sXMLPath != null && sXMLPath != string.Empty)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(sXMLPath);
                    XmlElement Doc_element = xmlDoc.DocumentElement;

                    XmlNodeList Doc_parentNode = Doc_element.GetElementsByTagName("section");

                    foreach (XmlElement elemParent in Doc_parentNode)
                    {

                        for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                        {
                            //if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText == "Allergies and Adverse Reactions")
                            //{
                            //    dsAllergy = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            //    break;
                            //}
                            if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "MEDICATIONS")
                            {
                                medicationSection = elemParent;
                                dsMedication = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                break;
                            }
                            //Newly added for date of visit as last modified date for incorporated list - Pujhitha
                            else if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS DIAGNOSIS"))
                            {
                                dsEncounter = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                break;
                            }
                            //else if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText == "Problems")
                            //{
                            //    dsProblem = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            //    break;
                            //}
                        }
                    }
                    if (dsEncounter != null && dsEncounter.Tables.Count > 0)
                    {
                        for (int i = 0; i < dsEncounter.Tables[0].Rows.Count; i++)
                        {
                            string Visit_Date = string.Empty;
                            try
                            {
                                Visit_Date = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("Date"));
                            }
                            catch
                            {

                            }

                            //if (Visit_Date != string.Empty)
                            //{
                            //    pat_DOV = Visit_Date.Substring(0, 4) + '-' + Visit_Date.Substring(4, 2) + '-' + Visit_Date.Substring(6, 2);
                            //    pat_DOV = Convert.ToDateTime(pat_DOV).ToString("dd-MMM-yyyy");
                            //    break;
                            //}
                            if (Visit_Date != string.Empty)
                            {
                                string year = Visit_Date.Substring(0, 4);
                                string month = Visit_Date.Substring(4, 2);
                                string date = Visit_Date.Substring(6, 2);
                                try
                                {
                                    if (Visit_Date.Length == 8)
                                    {
                                        pat_DOV = Convert.ToDateTime(year + "-" + month + "-" + date).ToString("dd-MMM-yyyy");
                                        break;
                                    }

                                    else
                                    {
                                        pat_DOV = Visit_Date.Split(',')[0].Split(' ')[1] + "-" + Visit_Date.Split(',')[0].Split(' ')[0] + Visit_Date.Split(',')[1].Split(' ')[1];
                                        pat_DOV = Convert.ToDateTime(pat_DOV).ToString("dd-MMM-yyyy");
                                        break;
                                    }
                                }
                                catch
                                {



                                }
                            }


                        }
                    }
                    PrintAllergy(xmlDoc);
                    PrintProblems(xmlDoc);
                }
                else
                {
                    foreach (UploadedFile file in UploadImage.UploadedFiles)
                    {
                        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("atala-capture-download/" + Session.SessionID + "/SampleXML/"));
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }
                        string file_path = Server.MapPath("atala-capture-download/" + Session.SessionID + "/SampleXML/" + Path.GetFileName(file.FileName));
                        file.SaveAs(file_path, true);
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(file_path);
                        XmlElement Doc_element = xmlDoc.DocumentElement;
                        XmlNodeList Doc_parentNode_patient = Doc_element.GetElementsByTagName("patient");
                        XmlNodeList Doc_parentNode = Doc_element.GetElementsByTagName("section");
                        XmlNodeList Doc_parentNode_name = Doc_element.GetElementsByTagName("given");
                        XmlNodeList Doc_parentNode_Gender = Doc_element.GetElementsByTagName("administrativeGenderCode");
                        XmlNodeList Doc_parentNode_DOB = Doc_element.GetElementsByTagName("birthTime");
                        XmlNodeList Doc_parentNode_name_last = Doc_element.GetElementsByTagName("family");



                        string firstname = "", lastname = "", dob = "", gender = "", MI = "", finalDate = "";
                        if (Doc_parentNode_name.Count > 0)

                            firstname = Doc_parentNode_name[0].InnerText;
                        if (Doc_parentNode_patient[0].ChildNodes[0].ChildNodes[1] != null)
                        {

                            if (Doc_parentNode_patient[0].ChildNodes[0].ChildNodes[1].Name.ToString().ToUpper() == "GIVEN")
                                MI = Doc_parentNode_patient[0].ChildNodes[0].ChildNodes[1].InnerText;
                        }
                        if (Doc_parentNode_name_last.Count > 0)
                            lastname = Doc_parentNode_name_last[0].InnerText;


                        if (Doc_parentNode_Gender.Count > 0)
                            gender = Doc_parentNode_Gender[0].Attributes["displayName"].Value;
                        if (Doc_parentNode_DOB.Count > 0)
                            dob = Doc_parentNode_DOB[0].Attributes["value"].Value;
                        if (dob.Length == 8)
                            finalDate = Convert.ToDateTime(dob.Substring(0, 4) + "-" + dob.Substring(4, 2) + "-" + dob.Substring(6, 2)).ToString("dd-MMM-yyyy");

                        string textname = txtHumanName.Text.Split(' ')[0] + " " + txtHumanName.Text.Split(' ')[2];

                        if (textname.ToUpper() == firstname.ToUpper() + " " + lastname.ToUpper() && gender.ToUpper() == txtSex.Text.ToUpper() && txtDOB.Text == finalDate)
                        {
                            foreach (XmlElement elemParent in Doc_parentNode)
                            {

                                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                                {
                                    //if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText == "Allergies and Adverse Reactions")
                                    //{
                                    //    dsAllergy = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                    //    break;
                                    //}
                                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "MEDICATIONS")
                                    {
                                        dsMedication = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                        break;
                                    }
                                    //Newly added for date of visit as last modified date for incorporated list - Pujhitha
                                    else if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ENCOUNTERS DIAGNOSIS"))
                                    {
                                        dsEncounter = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                        break;
                                    }
                                    //else if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText == "Problems")
                                    //{
                                    //    dsProblem = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                                    //    break;
                                    //}
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "stopload", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "alert('Patient in the  XML Does not match with this patient') ;", true);
                            return;
                        }

                        if (dsEncounter != null && dsEncounter.Tables.Count > 0)
                        {
                            for (int i = 0; i < dsEncounter.Tables[0].Rows.Count; i++)
                            {
                                string Visit_Date = StripTagsRegex(dsEncounter.Tables[0].Rows[i].Field<string>("Date"));
                                if (Visit_Date != string.Empty)
                                {
                                    try
                                    {
                                        pat_DOV = Visit_Date.Substring(0, 4) + '-' + Visit_Date.Substring(4, 2) + '-' + Visit_Date.Substring(6, 2);
                                        pat_DOV = Convert.ToDateTime(pat_DOV).ToString("dd-MMM-yyyy");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            string[] temp = Visit_Date.Split(',');
                                            string[] temp1 = temp[0].Split(' ');
                                            string[] temp2 = temp[1].Split(' ');
                                            string final = temp1[1] + "-" + temp1[0].Substring(0, 3) + "-" + temp2[1];
                                            pat_DOV = Convert.ToDateTime(final).ToString("dd-MMM-yyyy");
                                        }
                                        catch
                                        {
                                        }

                                    }
                                    break;
                                }

                            }
                        }
                        PrintAllergy(xmlDoc);
                        PrintProblems(xmlDoc);

                    }
                }

                //if (dsAllergy != null)
                //{
                //    for (int i = 0; i < dsAllergy.Tables[0].Rows.Count; i++)
                //    {
                //        Rcopia_Allergy objallergy = new Rcopia_Allergy();
                //        objallergy.Allergy_Name = dsAllergy.Tables[0].Rows[i].Field<string>("Allergy");
                //        objallergy.NDC_ID = dsAllergy.Tables[0].Rows[i].Field<string>("Allergy NDC code");
                //        objallergy.Reaction = dsAllergy.Tables[0].Rows[i].Field<string>("Reaction");
                //        objallergy.Deleted = dsAllergy.Tables[0].Rows[i].Field<string>("Deleted");
                //        objallergy.OnsetDate = Convert.ToDateTime(dsAllergy.Tables[0].Rows[i].Field<string>("Adverse Event Date"));
                //        lstIncorporatedAllergy.Add(objallergy);
                //    }
                //    Session["IncorporatedAllergyList"] = lstIncorporatedAllergy;
                //    LoadGridAllergyIncorporatedList(lstIncorporatedAllergy);
                //}
                //else
                //{
                //    grdAllergyIncorporetedList.DataSource = new string[] { };
                //    grdAllergyIncorporetedList.DataBind();
                //}

                if (dsMedication != null && dsMedication.Tables.Count > 0)
                {
                    try
                    {
                        Boolean bMedication = false;
                        Boolean bForm = false;
                        Boolean bStrength = false;
                        Boolean bDose = false;
                        Boolean bDoseUnit = false;
                        Boolean bDoseTiming = false;
                        Boolean bRouteofAdministration = false;
                        Boolean bStartDate = false;
                        Boolean bEndDate = false;
                        Boolean bMedicationC32 = false;
                        Boolean bDoseUnitC32 = false;
                        Boolean bStartDateC32 = false;
                        if (dsMedication.Tables[0].Columns.Contains("<tr><th>Medication") == true)
                            bMedication = true;
                        if (dsMedication.Tables[0].Columns.Contains("Medication") == true)
                            bMedicationC32 = true;
                        if (dsMedication.Tables[0].Columns.Contains("Form") == true)
                            bForm = true;
                        if (dsMedication.Tables[0].Columns.Contains("Strength") == true)
                            bStrength = true;
                        //if (dsMedication.Tables[0].Columns.Contains("Dose") == true)
                        //    bDose = true;
                        if (dsMedication.Tables[0].Columns.Contains("Directions") == true)
                            bDose = true;
                        if (dsMedication.Tables[0].Columns.Contains("Dose Unit") == true)
                            bDoseUnit = true;
                        if (dsMedication.Tables[0].Columns.Contains("Dose") == true)
                            bDoseUnitC32 = true;

                        if (dsMedication.Tables[0].Columns.Contains("Dose Timing") == true)
                            bDoseTiming = true;
                        if (dsMedication.Tables[0].Columns.Contains("Route of Administration") == true)
                            bRouteofAdministration = true;
                        if (dsMedication.Tables[0].Columns.Contains("Start Date") == true)
                            bStartDate = true;
                        if (dsMedication.Tables[0].Columns.Contains("Date Started") == true)
                            bStartDateC32 = true;
                        if (dsMedication.Tables[0].Columns.Contains("End Date") == true)
                            bEndDate = true;

                        for (int i = 0; i < dsMedication.Tables[0].Rows.Count; i++)
                        {
                            Rcopia_Medication objMedication = new Rcopia_Medication();
                            if (bMedication == true)
                            {
                                string sBrandName = dsMedication.Tables[0].Rows[i].Field<string>("<tr><th>Medication").Substring(dsMedication.Tables[0].Rows[i].Field<string>("<tr><th>Medication").IndexOf(">") + 1);
                                if (sBrandName.Contains("</content>") == true)
                                    sBrandName = sBrandName.Substring(0, sBrandName.LastIndexOf("</content>"));
                                if (bMedication == true)
                                    objMedication.Brand_Name = sBrandName;
                            }
                            if (bMedicationC32 == true)
                                objMedication.Brand_Name = dsMedication.Tables[0].Rows[i].Field<string>("Medication");

                            objMedication.Generic_Name = objMedication.Brand_Name;
                            //objMedication.Form = dsMedication.Tables[0].Rows[i].Field<string>("Directions");

                            if (bForm == true)
                                objMedication.Form = dsMedication.Tables[0].Rows[i].Field<string>("Form");
                            if (bStrength == true)
                                objMedication.Strength = dsMedication.Tables[0].Rows[i].Field<string>("Strength");
                            //if (bDose == true)
                            //    objMedication.Dose = dsMedication.Tables[0].Rows[i].Field<string>("Dose");
                            if (bDose == true)
                            {
                                if (StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS NEEDED" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS NEEDED FOR PAIN" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "WITH MEALS" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AS DIRECTED" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "BETWEEN MEALS" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "ONE HOUR BEFORE MEALS" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "BEFORE EXERCISE" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "WITH A GLASS OF WATER" ||
                            StripTagsRegex(dsMedication.Tables[0].Rows[i].Field<string>("Directions")).ToUpper() == "AFTER MEALS")
                                {
                                    objMedication.Dose_Other = dsMedication.Tables[0].Rows[i].Field<string>("Directions");
                                }
                                else
                                {
                                    objMedication.Dose_Timing = dsMedication.Tables[0].Rows[i].Field<string>("Directions");
                                }

                            }
                            if (bDoseUnit == true)
                                objMedication.Dose_Unit = dsMedication.Tables[0].Rows[i].Field<string>("Dose Unit");
                            if (bDoseUnitC32 == true)
                                objMedication.Dose_Unit = dsMedication.Tables[0].Rows[i].Field<string>("Dose");
                            if (bDoseTiming == true)
                                objMedication.Dose_Timing = dsMedication.Tables[0].Rows[i].Field<string>("Dose Timing");
                            if (bRouteofAdministration == true)
                                objMedication.Route = dsMedication.Tables[0].Rows[i].Field<string>("Route of Administration");
                            if (bStartDateC32 == true)
                                try
                                {
                                    objMedication.Start_Date = Convert.ToDateTime(dsMedication.Tables[0].Rows[i].Field<string>("Date Started")).Date;
                                }
                                catch
                                {
                                    objMedication.Start_Date = DateTime.MinValue;
                                }
                            if (bStartDate == true)
                                if (dsMedication.Tables[0].Rows[i].Field<string>("Start Date").ToUpper() == "UNKNOWN" || dsMedication.Tables[0].Rows[i].Field<string>("Start Date") == "")
                                    objMedication.Start_Date = DateTime.MinValue;
                                else
                                {
                                    string StartDate = dsMedication.Tables[0].Rows[i].Field<string>("Start Date");
                                    if (StartDate.Length > 6)
                                        try
                                        {
                                            objMedication.Start_Date = Convert.ToDateTime(StartDate.Insert(4, "-").Insert(7, "-"));
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                string[] temp = StartDate.Split(',');
                                                string[] temp1 = temp[0].Split(' ');

                                                string final = temp1[1] + "-" + temp1[0].Substring(0, 3) + "-" + temp[1];
                                                objMedication.Start_Date = Convert.ToDateTime(final);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                }
                            string EndDate = "";
                            if (bEndDate == true)
                            {
                                dsMedication.Tables[0].Rows[i].Field<string>("End Date");
                                EndDate = dsMedication.Tables[0].Rows[i].Field<string>("End Date");
                                if (dsMedication.Tables[0].Rows[i].Field<string>("End Date").ToUpper() == "UNKNOWN" || dsMedication.Tables[0].Rows[i].Field<string>("End Date") == "")
                                    objMedication.Stop_Date = DateTime.MinValue;
                            }

                            else if (EndDate.Length > 6)
                                objMedication.Stop_Date = Convert.ToDateTime(EndDate.Insert(4, "-").Insert(7, "-"));
                            //if (dtDateofService != DateTime.MinValue && objMedication.Start_Date == DateTime.MinValue)
                            //    objMedication.Start_Date = dtDateofService;
                            if (dtDateofService != DateTime.MinValue)
                                objMedication.Created_Date_And_Time = dtDateofService;

                            objMedication.Dose = medicationSection.GetElementsByTagName("doseQuantity")[i].Attributes.GetNamedItem("value").Value;

                            //objMedication.NDC_ID = dsMedication.Tables[0].Rows[i].Field<string>("Status");
                            //objMedication.Generic_Name = dsMedication.Tables[0].Rows[i].Field<string>("Indications");
                            //objMedication.Fill_Date = Convert.ToDateTime(dsMedication.Tables[0].Rows[i].Field<string>("Fill Date"));
                            objMedication.Stop_Reason = "";// dsMedication.Tables[0].Rows[i].Field<string>("Fill Instructions");
                            objMedication.Rxnorm_ID = Convert.ToUInt64(medicationSection.GetElementsByTagName("manufacturedMaterial")[i].ChildNodes[0].Attributes.GetNamedItem("code").Value);
                            objMedication.Rxnorm_ID_Type = "SBD";
                            lstIncorporatedMedication.Add(objMedication);

                        }
                        Session["IncorporatedMedicationList"] = lstIncorporatedMedication;
                        LoadGridMedicationIncorporatedList(lstIncorporatedMedication);
                    }
                    catch
                    {
                        //do nothing
                    }
                }
                else
                {
                    grdMedicationIncorporetedList.DataSource = new string[] { };
                    grdMedicationIncorporetedList.DataBind();
                }


                //if (dsProblem != null)
                //{
                //    for (int i = 0; i < dsProblem.Tables[0].Rows.Count; i++)
                //    {
                //        ProblemList objProblem = new ProblemList();
                //        objProblem.ICD_Code = dsProblem.Tables[0].Rows[i].Field<string>("ICD-9 Code");
                //        objProblem.Problem_Description = dsProblem.Tables[0].Rows[i].Field<string>("Problem Name");
                //        objProblem.Status = dsProblem.Tables[0].Rows[i].Field<string>("Problem Status");
                //        objProblem.Date_Diagnosed = dsProblem.Tables[0].Rows[i].Field<string>("Date Diagnosed");
                //        lstIncorporatedProblem.Add(objProblem);
                //    }
                //    Session["IncorporatedProblemList"] = lstIncorporatedProblem;
                //    LoadGridProblemIncorporatedList(lstIncorporatedProblem);
                //}
                //else
                //{
                //    grdProblemIncorporatedList.DataSource = new string[] { };
                //    grdProblemIncorporatedList.DataBind();
                //}

            }
            else
            {
                //validation message
            }

        }


        //Add
        public void PrintAllergy(XmlDocument xmldoc)
        {
            #region Allergy

            DataSet dsAllergy = null;
            XmlNodeList Doc_AllergyNode = xmldoc.GetElementsByTagName("section");
            foreach (XmlElement elemParent in Doc_AllergyNode)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && (elemParent.ChildNodes[i].InnerText.ToUpper() == "ALLERGIES, ADVERSE REACTIONS, ALERTS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ALLERGIES AND ADVERSE REACTIONS" || elemParent.ChildNodes[i].InnerText.ToUpper() == "ALLERGIES"))
                    {
                        dsAllergy = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);

                        is_break = true;
                        break;
                    }
                }
                if (is_break == true)
                    break;
            }

            if (dsAllergy != null && dsAllergy.Tables.Count > 0)
            {
                try
                {
                    Boolean bAllergyName = false;
                    Boolean bReaction = false;
                    Boolean bSeverity = false;
                    Boolean bNDCID = false;
                    Boolean bStatus = false;
                    Boolean bAllergyNameC32 = false;

                    if (dsAllergy.Tables[0].Columns.Contains("<tr><th>Substance") == true)
                        bAllergyName = true;
                    if (dsAllergy.Tables[0].Columns.Contains("Substance") == true)
                        bAllergyNameC32 = true;
                    if (dsAllergy.Tables[0].Columns.Contains("Reaction") == true)
                        bReaction = true;
                    if (dsAllergy.Tables[0].Columns.Contains("Severity") == true)
                        bSeverity = true;
                    if (dsAllergy.Tables[0].Columns.Contains("NDCID") == true)
                        bNDCID = true;
                    if (dsAllergy.Tables[0].Columns.Contains("Status") == true)
                        bStatus = true;

                    for (int i = 0; i < dsAllergy.Tables[0].Rows.Count; i++)
                    {
                        Rcopia_Allergy objallergy = new Rcopia_Allergy();
                        if (bAllergyName == true)
                            objallergy.Allergy_Name = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("<tr><th>Substance"));
                        if (bAllergyNameC32 == true)
                            objallergy.Allergy_Name = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Substance"));
                        if (bReaction == true)
                            objallergy.Reaction = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Reaction"));
                        //if (bSeverity == true)
                        //{
                        //    if (bReaction == true)
                        //        objallergy.Reaction = objallergy.Reaction + ": " + StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Severity"));
                        //    else
                        //        objallergy.Reaction = " : " + StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Severity"));
                        //}
                        if (bSeverity == true)
                        {
                            objallergy.Severity = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Severity"));
                        }
                        if (bNDCID == true)
                            objallergy.NDC_ID = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("NDCID"));
                        if (bStatus == true)
                            objallergy.Status = StripTagsRegex(dsAllergy.Tables[0].Rows[i].Field<string>("Status"));
                        objallergy.OnsetDate = DateTime.Now;
                        objallergy.Created_By = ClientSession.UserName;
                        lstIncorporatedAllergy.Add(objallergy);
                    }
                    Session["IncorporatedAllergyList"] = lstIncorporatedAllergy;
                    LoadGridAllergyIncorporatedList(lstIncorporatedAllergy);
                }
                catch
                {
                    //do nothing
                }
            }
            else
            {
                grdAllergyIncorporetedList.DataSource = new string[] { };
                grdAllergyIncorporetedList.DataBind();
            }

            #endregion
        }
        public static string StripTagsRegex(string source)
        {
            if (source == null)
                return string.Empty;
            else
                return Regex.Replace(source, "<.*?>", string.Empty);
        }



        public void PrintProblems(XmlDocument xmldoc)
        {
            #region ProblemList

            XmlNodeList Doc_Problem_Node = xmldoc.GetElementsByTagName("section");
            DataSet dsProblem = null;
            int iCountResult = 0;
            int iPrevCount = 0;
            AllICD_9Manager objicd = new AllICD_9Manager();
            foreach (XmlElement elemParent in Doc_Problem_Node)
            {
                bool is_break = false;
                for (int i = 0; i < elemParent.ChildNodes.Count; i++)
                {
                    if (elemParent.ChildNodes[i].Name == "title" && elemParent.ChildNodes[i].InnerText.ToUpper() == "PROBLEMS")
                    {

                        if (elemParent.ChildNodes[i + 1].ChildNodes[1] != null)
                        {

                            // int list_count = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes.Count;
                            int list_count = elemParent.ChildNodes[i + 1].ChildNodes.Count;

                            for (int j = 0; j < list_count; j++)
                            {
                                try
                                {
                                    if (elemParent.ChildNodes[i + 1].ChildNodes[j].Name == "list" && j != iPrevCount)
                                    {
                                        string columnvalue = string.Empty;
                                        if (elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes.Count > 1)
                                        {
                                            columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes[1].InnerText;
                                        }
                                        else if (elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes.Count == 1)
                                        {
                                            columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j].ChildNodes[0].ChildNodes[0].InnerText;
                                        }
                                          
                                                                           
                                        //for bug id: 28517
                                        // columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].InnerXml;

                                        IList<XmlElement> lst = elemParent.ChildNodes.OfType<XmlElement>().Where(Z => Z.Name == "entry").ToList();

                                        try
                                        {


                                            ProblemList objProblem = new ProblemList();
                                            objProblem.Problem_Description = columnvalue;
                                            string sStatus = lst[iCountResult].GetElementsByTagName("statusCode")[0].Attributes.GetNamedItem("code").Value;
                                            if (sStatus != "")
                                            {
                                                if (sStatus.ToUpper() == "COMPLETED")
                                                    objProblem.Status = "Resolved";
                                                else
                                                {


                                                    objProblem.Status = sStatus.First().ToString().ToUpper() + sStatus.Substring(1);
                                                }
                                            }
                                            if (lst[iCountResult].GetElementsByTagName("low") != null)
                                            {
                                                if (lst[iCountResult].GetElementsByTagName("low")[0].ParentNode.Name == "effectiveTime")
                                                {
                                                    if (lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Length == 8)
                                                    {
                                                        string sDate = lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(0, 4) + "-" + lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(4, 2) + "-" + lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(6, 2);
                                                        objProblem.Date_Diagnosed = Convert.ToDateTime(sDate).ToString("dd-MMM-yyyy");
                                                    }
                                                }
                                            }
                                            objProblem.Snomed_Code = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("code").Value;
                                            objProblem.Snomed_Code_Description = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("displayName").Value;
                                            objProblem.Created_By = ClientSession.UserName;
                                            if (objProblem.Snomed_Code != "")
                                            {
                                                objProblem.ICD = objicd.GetBySnomed(objProblem.Snomed_Code);
                                            }
                                            lstIncorporatedProblem.Add(objProblem);
                                            iCountResult++;


                                            //ProblemList objProblem = new ProblemList();
                                            //string reason = StripTagsRegex(columnvalue);
                                            //string sval = reason.Replace("\n", " ");

                                            //string[] svalue = sval.Split(new string[] { "Status" }, StringSplitOptions.None);
                                            //objProblem.Problem_Description = svalue[0].ToString();
                                            //if (svalue[1].Replace("\r", "").Replace("\t", "").Split(' ')[2].Contains(',') == true)
                                            //    objProblem.Status = svalue[1].Replace("\r", "").Replace("\t", "").Split(' ')[2].Split(',')[0];
                                            //else
                                            //    objProblem.Status = svalue[1].Replace("\r", "").Replace("\t", "").Split(' ')[2];

                                            //objProblem.ICD = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("code").Value;
                                            //objProblem.Date_Diagnosed = string.Empty;//DateTime.Now.ToString("dd-MMM-yyyy");
                                            //objProblem.Created_By = ClientSession.UserName;
                                            //lstIncorporatedProblem.Add(objProblem);
                                            //iCountResult++;
                                        }
                                        catch
                                        {
                                            //do nothing
                                        }
                                    }


                                    else if (j + 1 < list_count && elemParent.ChildNodes[i + 1].ChildNodes[j + 1].Name == "list")
                                    {
                                        iPrevCount = j + 1;
                                        try
                                        {
                                            ProblemList objProblem = new ProblemList();
                                            string text = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].InnerText;
                                            if (text.Contains(':'))
                                            {
                                                string[] desc = text.Split(':');
                                                objProblem.Problem_Description = desc[0].ToString();
                                                objProblem.Status = "Active";
                                                IList<XmlElement> lst = elemParent.ChildNodes.OfType<XmlElement>().Where(Z => Z.Name == "entry").ToList();
                                                if (lst[iCountResult].GetElementsByTagName("low") != null)
                                                {
                                                    if (lst[iCountResult].GetElementsByTagName("low")[0].ParentNode.Name == "effectiveTime")
                                                    {
                                                        if (lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Length == 8)
                                                        {
                                                            string sDate = lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(0, 4) + "-" + lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(4, 2) + "-" + lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(6, 2);
                                                            objProblem.Date_Diagnosed = Convert.ToDateTime(sDate).ToString("dd-MMM-yyyy");
                                                        }
                                                    }
                                                }
                                                objProblem.Snomed_Code = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("code").Value;
                                                objProblem.Snomed_Code_Description = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("displayName").Value;
                                                objProblem.Created_By = ClientSession.UserName;
                                                if (objProblem.Snomed_Code != "")
                                                {
                                                    objProblem.ICD = objicd.GetBySnomed(objProblem.Snomed_Code);
                                                }
                                                lstIncorporatedProblem.Add(objProblem);
                                                iCountResult++;
                                            }
                                            else
                                            {
                                                int list_count_1 = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes.Count;

                                                for (int k = 0; k < list_count_1; k++)
                                                {
                                                    try
                                                    {
                                                        if (elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[1].Name == "list")
                                                        {
                                                            string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[j + 1].ChildNodes[k].ChildNodes[1].InnerText;
                                                            //for bug id: 28517
                                                            // columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].ChildNodes[0].InnerXml;

                                                            IList<XmlElement> lst = elemParent.ChildNodes.OfType<XmlElement>().Where(Z => Z.Name == "entry").ToList();

                                                            try
                                                            {


                                                                ProblemList objProblem1 = new ProblemList();
                                                                objProblem1.Problem_Description = columnvalue;
                                                                string sStatus = lst[iCountResult].GetElementsByTagName("statusCode")[0].Attributes.GetNamedItem("code").Value;
                                                                if (sStatus != "")
                                                                {
                                                                    if (sStatus.ToUpper() == "COMPLETED")
                                                                        objProblem1.Status = "Resolved";
                                                                    else
                                                                    {


                                                                        objProblem1.Status = sStatus.First().ToString().ToUpper() + sStatus.Substring(1);
                                                                    }
                                                                }
                                                                if (lst[iCountResult].GetElementsByTagName("low") != null)
                                                                {
                                                                    if (lst[iCountResult].GetElementsByTagName("low")[0].ParentNode.Name == "effectiveTime")
                                                                    {
                                                                        if (lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Length == 8)
                                                                        {
                                                                            string sDate = lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(0, 4) + "-" + lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(4, 2) + "-" + lst[iCountResult].GetElementsByTagName("low")[0].Attributes[0].Value.Substring(6, 2);
                                                                            objProblem1.Date_Diagnosed = Convert.ToDateTime(sDate).ToString("dd-MMM-yyyy");
                                                                        }
                                                                    }
                                                                }
                                                                objProblem1.Snomed_Code = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("code").Value;
                                                                objProblem1.Snomed_Code_Description = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("displayName").Value;
                                                                objProblem1.Created_By = ClientSession.UserName;
                                                                if (objProblem1.Snomed_Code != "")
                                                                {
                                                                    objProblem1.ICD = objicd.GetBySnomed(objProblem1.Snomed_Code);
                                                                }
                                                                lstIncorporatedProblem.Add(objProblem1);
                                                                iCountResult++;
                                                            }
                                                            catch
                                                            {
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                               // break;
                                            }

                                        }
                                        catch
                                        {
                                        }

                                    }

                                }
                                catch
                                {
                                    try
                                    {
                                        string columnvalue = elemParent.ChildNodes[i + 1].ChildNodes[1].ChildNodes[j].InnerXml;
                                        //for bug id: 28517
                                        try
                                        {
                                            ProblemList objProblem = new ProblemList();
                                            string reason = StripTagsRegex(columnvalue);
                                            string sval = reason.Replace("\n", " ");
                                            string[] svalue = sval.Split(new string[] { "Status" }, StringSplitOptions.None);
                                            objProblem.Problem_Description = svalue[0].ToString();
                                            objProblem.Status = svalue[1].Split('\r')[0].Replace("-", "");
                                            objProblem.Date_Diagnosed = string.Empty;//DateTime.Now.ToString("dd-MMM-yyyy");
                                            objProblem.Created_By = ClientSession.UserName;
                                            lstIncorporatedProblem.Add(objProblem);
                                        }
                                        catch
                                        {

                                            int count = elemParent.ChildNodes[i + 1].ChildNodes.Count;
                                            try
                                            {
                                                for (int k = 0; k < count; k++)
                                                {
                                                    if (elemParent.ChildNodes[i + 1].ChildNodes[k].OuterXml.ToUpper().Contains("LIST"))
                                                    {
                                                        ProblemList objProblem = new ProblemList();
                                                        string reason = StripTagsRegex(columnvalue);

                                                        objProblem.Problem_Description = elemParent.ChildNodes[i + 1].ChildNodes[k].ChildNodes[0].ChildNodes[1].InnerText;
                                                        IList<XmlElement> lst = elemParent.ChildNodes.OfType<XmlElement>().Where(Z => Z.Name == "entry").ToList();
                                                        objProblem.ICD = lst[iCountResult].GetElementsByTagName("value")[0].Attributes.GetNamedItem("code").Value;
                                                        objProblem.Status = "Active";
                                                        objProblem.Date_Diagnosed = string.Empty;//DateTime.Now.ToString("dd-MMM-yyyy");
                                                        objProblem.Created_By = ClientSession.UserName;
                                                        lstIncorporatedProblem.Add(objProblem);
                                                    }

                                                }
                                            }
                                            catch
                                            {

                                            }
                                            //do nothing
                                        }
                                    }
                                    catch
                                    {
                                        //do nothing
                                    }
                                }
                            }

                            is_break = true;
                            break;
                        }

                        else
                        {
                            dsProblem = ConvertHTMLTablesToDataSet(elemParent.ChildNodes[i + 1].InnerXml);
                            if (dsProblem != null && dsProblem.Tables.Count > 0)
                            {
                                foreach (DataRow row in dsProblem.Tables[0].Rows)
                                {
                                    foreach (DataColumn column in dsProblem.Tables[0].Columns)
                                    {
                                        string ColumnData = StripTagsRegex(row[column].ToString());
                                    }
                                }
                            }
                            if (dsProblem != null && dsProblem.Tables.Count > 0)
                            {
                                IList<ProblemList> lstproblem = new List<ProblemList>();

                                Boolean bMedication = false;
                                Boolean problem = false;
                                Boolean status = false;
                                Boolean datediagonosis = false;

                                if (dsProblem.Tables[0].Columns.Contains("Problem") == true)
                                    problem = true;
                                if (dsProblem.Tables[0].Columns.Contains("Status") == true)
                                    status = true;
                                if (dsProblem.Tables[0].Columns.Contains("Date Diagnosed") == true)
                                    datediagonosis = true;


                                for (int k = 0; k < dsProblem.Tables[0].Rows.Count; k++)
                                {
                                    ProblemList objProblem = new ProblemList();
                                    Rcopia_Medication objMedication = new Rcopia_Medication();
                                    if (bMedication == true)
                                    {
                                        if (problem == true)

                                            objProblem.Problem_Description = dsProblem.Tables[0].Rows[i].Field<string>("Problem");
                                        if (status == true)
                                            objProblem.Status = dsProblem.Tables[0].Rows[i].Field<string>("Status");
                                        if (datediagonosis == true)
                                            objProblem.Created_By = ClientSession.UserName;
                                        try
                                        {
                                            objProblem.Date_Diagnosed = Convert.ToDateTime(dsProblem.Tables[0].Rows[i].Field<string>("Date Diagnosed")).ToString("yyyy-MM-dd");
                                        }
                                        catch
                                        {
                                            objProblem.Date_Diagnosed = string.Empty;
                                        }
                                        lstIncorporatedProblem.Add(objProblem);
                                    }



                                }
                                is_break = true;


                            }

                            if (is_break == true)
                                break;
                        }
                    }
                    if (is_break == true)
                        break;

                }
            }



            //if (dsProblem != null)
            //{
            //    for (int i = 0; i < dsProblem.Tables[0].Rows.Count; i++)
            //    {
            //        ProblemList objProblem = new ProblemList();
            //        objProblem.ICD_Code = dsProblem.Tables[0].Rows[i].Field<string>("ICD-9 Code");
            //        objProblem.Problem_Description = dsProblem.Tables[0].Rows[i].Field<string>("Problem Name");
            //        objProblem.Status = dsProblem.Tables[0].Rows[i].Field<string>("Problem Status");
            //        objProblem.Date_Diagnosed = dsProblem.Tables[0].Rows[i].Field<string>("Date Diagnosed");
            //        lstIncorporatedProblem.Add(objProblem);
            //    }
            //    Session["IncorporatedProblemList"] = lstIncorporatedProblem;
            //    LoadGridProblemIncorporatedList(lstIncorporatedProblem);
            //}
            //else
            //{
            //    grdProblemIncorporatedList.DataSource = new string[] { };
            //    grdProblemIncorporatedList.DataBind();
            //}




            if (lstIncorporatedProblem.Count > 0)
            {
                Session["IncorporatedProblemList"] = lstIncorporatedProblem;
                LoadGridProblemIncorporatedList(lstIncorporatedProblem);
            }
            else
            {
                grdProblemIncorporatedList.DataSource = new string[] { };
                grdProblemIncorporatedList.DataBind();
            }
            #endregion
        }

        //end add


        public void LoadGridProblemIncorporatedList(IList<ProblemList> lstProblemIncorporated)
        {
            if (lstProblemIncorporated.Count > 0)
            {
                grdProblemIncorporatedList.DataSource = new string[] { };
                grdProblemIncorporatedList.DataBind();

                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Problem ID", typeof(string)));

                for (int i = 0; i < lstProblemIncorporated.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Name"] = lstProblemIncorporated[i].Problem_Description;
                    dr["Status"] = lstProblemIncorporated[i].Status;
                    dr["Last Modified Date"] = (pat_DOV != string.Empty) ? pat_DOV : UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy");//lstProblemIncorporated[i].Date_Diagnosed;
                    dr["Problem ID"] = lstProblemIncorporated[i].Id;
                    dt.Rows.Add(dr);
                }

                grdProblemIncorporatedList.DataSource = dt;
                grdProblemIncorporatedList.DataBind();
            }
            else
            {
                grdProblemIncorporatedList.DataSource = new string[] { };
                grdProblemIncorporatedList.DataBind();
            }
        }

        public void LoadGridAllergyIncorporatedList(IList<Rcopia_Allergy> lstAllergyIncorporated)
        {
            grdAllergyIncorporetedList.DataSource = new string[] { };
            grdAllergyIncorporetedList.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Reaction", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Rcopia Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Data From", typeof(string)));

            for (int i = 0; i < lstAllergyIncorporated.Count; i++)
            {
                dr = dt.NewRow();
                dr["Name"] = lstAllergyIncorporated[i].Allergy_Name;
                dr["Reaction"] = lstAllergyIncorporated[i].Reaction;
                dr["Status"] = lstIncorporatedAllergy[i].Status;
                //if (lstAllergyIncorporated[i].Deleted == "N")
                //    dr["Status"] = "Active";
                //else
                //    dr["Status"] = "Inactive";
                dr["Last Modified Date"] = (pat_DOV != string.Empty) ? pat_DOV : UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy");//UtilityManager.ConvertToLocal(lstAllergyIncorporated[i].OnsetDate).ToString("dd-MMM-yyyy");
                dr["Rcopia Id"] = lstAllergyIncorporated[i].Id;
                dr["Data From"] = "Incorporated";
                dt.Rows.Add(dr);
            }

            grdAllergyIncorporetedList.DataSource = dt;
            grdAllergyIncorporetedList.DataBind();
        }

        public void LoadGridMedicationIncorporatedList(IList<Rcopia_Medication> lstMedicationIncorporated)
        {
            grdMedicationIncorporetedList.DataSource = new string[] { };
            grdMedicationIncorporetedList.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Rcopia Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Data From", typeof(string)));

            for (int i = 0; i < lstMedicationIncorporated.Count; i++)
            {
                dr = dt.NewRow();
                //dr["Name"] = lstMedicationIncorporated[i].Brand_Name + " " + lstMedicationIncorporated[i].Form + " " + lstMedicationIncorporated[i].Patient_Notes;
                dr["Name"] = lstMedicationIncorporated[i].Brand_Name + " " + lstMedicationIncorporated[i].Strength + " " + lstMedicationIncorporated[i].Dose + " " + lstMedicationIncorporated[i].Dose_Unit + " " + lstMedicationIncorporated[i].Route + " " + lstMedicationIncorporated[i].Dose_Timing;
                if (lstMedicationIncorporated[i].Start_Date != DateTime.MinValue && lstMedicationIncorporated[i].Stop_Date == DateTime.MinValue)
                    dr["Status"] = "Active";
                else if (lstMedicationIncorporated[i].Start_Date == DateTime.MinValue && lstMedicationIncorporated[i].Stop_Date == DateTime.MinValue)
                    dr["Status"] = "Inactive";
                else if ((lstMedicationIncorporated[i].Start_Date != DateTime.MinValue && lstMedicationIncorporated[i].Stop_Date != DateTime.MinValue) || (lstMedicationIncorporated[i].Start_Date == DateTime.MinValue && lstMedicationIncorporated[i].Stop_Date != DateTime.MinValue))
                {
                    if (lstMedicationIncorporated[i].Stop_Date < UtilityManager.ConvertToLocal(DateTime.UtcNow))
                        dr["Status"] = "Inactive";
                    else
                        dr["Status"] = "Active";
                }

                dr["Last Modified Date"] = (pat_DOV != string.Empty) ? pat_DOV : UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy");
                dr["Rcopia Id"] = lstMedicationIncorporated[i].Id;
                dr["Data From"] = "Incorporated";
                dt.Rows.Add(dr);
            }

            grdMedicationIncorporetedList.DataSource = dt;
            grdMedicationIncorporetedList.DataBind();

        }


        private DataSet ConvertHTMLTablesToDataSet(string HTML)
        {
            // Declarations 
            DataSet ds = new DataSet();
            DataTable dt = null;
            DataRow dr = null;
            //DataColumn dc = null;
            string TableExpression = "<table[^>]*>(.*?)</table>";
            string HeaderExpression = "<th[^>]*>(.*?)</th>";
            string RowExpression = "<tr[^>]*>(.*?)</tr>";
            string ColumnExpression = "<td[^>]*>(.*?)</td>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;

            // Get a match for all the tables in the HTML 
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Loop through each table element 
            foreach (Match Table in Tables)
            {
                // Reset the current row counter and the header flag 
                iCurrentRow = 0;
                HeadersExist = false;

                // Add a new table to the DataSet 
                dt = new DataTable();

                //Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names) 
                if (Table.Value.Contains("<th"))
                {
                    // Set the HeadersExist flag 
                    HeadersExist = true;

                    // Get a match for all the rows in the table 
                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    // Loop through each header element 
                    foreach (Match Header in Headers)
                    {
                        dt.Columns.Add(Header.Groups[1].ToString());
                    }
                }
                else
                {
                    for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                    {
                        dt.Columns.Add("Column " + iColumns);
                    }
                }


                //Get a match for all the rows in the table 

                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                // Loop through each row element 
                foreach (Match Row in Rows)
                {
                    // Only loop through the row if it isn't a header row 
                    if (!(iCurrentRow == 0 && HeadersExist))
                    {
                        // Create a new row and reset the current column counter 
                        dr = dt.NewRow();
                        iCurrentColumn = 0;

                        // Get a match for all the columns in the row 
                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                        // Loop through each column element 
                        foreach (Match Column in Columns)
                        {
                            // Add tohe value to the DataRow 
                            dr[iCurrentColumn] = Column.Groups[1].ToString();

                            // Increase the current column  
                            iCurrentColumn++;
                        }

                        // Add the DataRow to the DataTable 
                        dt.Rows.Add(dr);

                    }

                    // Increase the current row counter 
                    iCurrentRow++;
                }


                // Add the DataTable to the DataSet 
                ds.Tables.Add(dt);

            }

            return ds;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            bool is_save = false;

            IList<Rcopia_Medication> lstMedicationSave = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> lstMedicationUpdate = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> lstMedicationDelete = new List<Rcopia_Medication>();


            if (grdAllergyMergedList.Items.Count > 0 || grdMedicationMergedList.Items.Count > 0 || grdProblemMergedList.Items.Count > 0)
            {
                is_save = true;
                ulHumanID = (ulong)Session["HumanId"];

                #region Problem
                if (grdProblemMergedList.Items.Count > 0)
                {
                    IList<ProblemList> listProblemtoSave = new List<ProblemList>();
                    IList<ProblemList> listProblemtoUpdate = new List<ProblemList>();
                    IList<ProblemList> UpdatedMergedList = new List<ProblemList>();

                    if (Session["ActualProblemList"] != null)
                    {
                        lstProblem = (IList<ProblemList>)Session["ActualProblemList"];
                        for (int i = 0; i < lstProblem.Count; i++)
                        {
                            lstProblem[i].Is_Active = "N";
                            lstProblem[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                            lstProblem[i].Modified_By = ClientSession.UserName;
                            listProblemtoUpdate.Add(lstProblem[i]);
                        }
                    }

                    if (Session["MergedProblemList"] != null)
                    {
                        lstMergedProblem = (IList<ProblemList>)Session["MergedProblemList"];

                        for (int i = 0; i < lstMergedProblem.Count; i++)
                        {
                            System.Web.UI.WebControls.DropDownList ddlSelectRow = ((System.Web.UI.WebControls.DropDownList)grdProblemMergedList.Items[i].Cells[2].FindControl("ddlProblemStatus"));
                            lstMergedProblem[i].Status = ddlSelectRow.SelectedItem.Value;
                            if (lstMergedProblem[i].Id != 0)
                            {
                                lstMergedProblem[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                                lstMergedProblem[i].Modified_By = ClientSession.UserName;
                                //lstMergedProblem[i].Status = "Active";
                                lstMergedProblem[i].Is_Active = "Y";
                                // lstMergedProblem[i].ICD_Code = "V70.0";
                                listProblemtoUpdate.Add(lstMergedProblem[i]);
                            }
                            else
                            {
                                //lstMergedProblem[i].Status = "Active";
                                IList<AllICD_9> lstallicd = new List<AllICD_9>();
                                AllICD_9Manager objallicdmanager = new AllICD_9Manager();
                                lstallicd = objallicdmanager.GetBysnomedcode(lstMergedProblem[i].ICD, lstMergedProblem[i].Problem_Description);
                                if (lstallicd.Count > 0)
                                {
                                    lstMergedProblem[i].ICD = lstallicd[0].ICD_9;
                                    lstMergedProblem[i].Problem_Description = lstallicd[0].ICD_9_Description;
                                }
                                lstMergedProblem[i].Is_Active = "Y";
                                lstMergedProblem[i].Human_ID = ulHumanID;
                                lstMergedProblem[i].Created_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                                lstMergedProblem[i].Created_By = ClientSession.UserName;
                                lstMergedProblem[i].Physician_ID = ClientSession.PhysicianId;

                                // lstMergedProblem[i].ICD_Code = "V70.0";
                                listProblemtoSave.Add(lstMergedProblem[i]);
                            }
                        }
                    }

                    if (listProblemtoSave.Count > 0 || listProblemtoUpdate.Count > 0)
                    {
                        ProblemListManager obj_problemMgr = new ProblemListManager();
                        lstProblem = obj_problemMgr.InsertorUpdateIntoProblemList(listProblemtoSave.ToArray(), listProblemtoUpdate.ToArray(), ulHumanID, "", true,ClientSession.LegalOrg);
                        lstProblem = obj_problemMgr.GetFromProblemListClinicalInformation(ulHumanID, string.Empty, true);
                    }
                    else
                    {
                        ProblemListManager obj_problemMgr = new ProblemListManager();
                        lstProblem = obj_problemMgr.GetFromProblemListClinicalInformation(ulHumanID, string.Empty, true);
                    }

                    IList<ProblemList> tempprob = (from doc in lstProblem where doc.Is_Active == "Y" select doc).ToList<ProblemList>();

                    Session["ActualProblemList"] = tempprob;
                    lstMergedProblem = new List<ProblemList>();
                    Session["MergedProblemList"] = null;
                    lstIncorporatedProblem = new List<ProblemList>();
                    Session["IncorporatedProblemList"] = lstIncorporatedProblem;

                    grdProblemActualList.DataSource = new string[] { };
                    grdProblemActualList.DataBind();

                    grdProblemIncorporatedList.DataSource = new string[] { };
                    grdProblemIncorporatedList.DataBind();

                    grdProblemMergedList.DataSource = new string[] { };
                    grdProblemMergedList.DataBind();

                    LoadGridProblemActualList(tempprob);

                }

                #endregion


                #region Medication
                if (grdMedicationMergedList.Items.Count > 0)
                {
                    if (Session["ActualMedicationList"] != null)
                    {
                        lstMedication = (IList<Rcopia_Medication>)Session["ActualMedicationList"];
                        for (int i = 0; i < lstMedication.Count; i++)
                        {
                            lstMedication[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                            lstMedication[i].Modified_By = ClientSession.UserName;
                            lstMedication[i].Deleted = "Y";
                            lstMedication[i].Last_Modified_By = ClientSession.RCopiaUserName;
                            lstMedicationDelete.Add(lstMedication[i]);
                        }
                    }

                    if (Session["MergedMedicationList"] != null)
                    {
                        lstMergedMedication = (IList<Rcopia_Medication>)Session["MergedMedicationList"];
                        for (int i = 0; i < lstMergedMedication.Count; i++)
                        {
                            System.Web.UI.WebControls.DropDownList ddlSelectRow = ((System.Web.UI.WebControls.DropDownList)grdMedicationMergedList.Items[i].Cells[2].FindControl("ddlMedicationStatus"));
                            if (ddlSelectRow.SelectedItem.Value.ToUpper() == "ACTIVE")
                                lstMergedMedication[i].Deleted = "N";
                            else if (ddlSelectRow.SelectedItem.Value.ToUpper() == "INACTIVE")
                                lstMergedMedication[i].Deleted = "Y";
                            if (lstMergedMedication[i].Id != 0 && lstMergedMedication[i].Last_Modified_By.Trim() != "")
                            {
                                lstMergedMedication[i].Generic_Name = "";
                                lstMergedMedication[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                                lstMergedMedication[i].Modified_By = ClientSession.UserName;
                                //Commented for BugID:30414 -Pujhitha
                                //lstMergedMedication[i].Start_Date = Convert.ToDateTime(hdnLocalDate.Value);
                                //lstMergedMedication[i].Stop_Date = DateTime.MinValue;
                                //lstMergedMedication[i].Deleted = "N";
                                lstMergedMedication[i].Last_Modified_By = ClientSession.RCopiaUserName;
                                if (lstMergedMedication[i].Deleted.ToUpper() == "Y")
                                    lstMedicationDelete.Add(lstMergedMedication[i]);
                                else
                                    lstMedicationUpdate.Add(lstMergedMedication[i]);
                            }
                            else if (lstMergedMedication[i].Id != 0 && lstMergedMedication[i].Last_Modified_By.Trim() == "")
                            {
                                IList<Rcopia_Medication> lstrcopia = new List<Rcopia_Medication>();

                                lstMedicationDelete.Add(lstMergedMedication[i]);
                                ulong id = lstMedicationDelete[0].Id;
                                lstrcopia.Add(lstMergedMedication[i]);
                                lstrcopia[0].Id = 0;
                                lstMedicationSave.Add(lstrcopia[0]);
                                lstMedicationDelete[0].Id = id;

                            }
                            else if (lstMergedMedication[i].Id == 0)
                            {
                                lstMergedMedication[i].Human_ID = ulHumanID;
                                if (dtDateofService != DateTime.MinValue)
                                    lstMergedMedication[i].Created_Date_And_Time = dtDateofService;
                                else
                                    lstMergedMedication[i].Created_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                                lstMergedMedication[i].Created_By = ClientSession.UserName;
                                lstMergedMedication[i].Last_Modified_By = ClientSession.RCopiaUserName;
                                //lstMergedMedication[i].Deleted = "N";
                                //Commented for BugID:30414 -Pujhitha
                                //if (lstMergedMedication[i].Start_Date == DateTime.MinValue && lstMergedMedication[i].Stop_Date == DateTime.MinValue)// || (lstMergedMedication[i].Start_Date != DateTime.MinValue && lstMergedMedication[i].Stop_Date != DateTime.MinValue))
                                //{
                                //    lstMergedMedication[i].Start_Date = Convert.ToDateTime(hdnLocalDate.Value);
                                //    lstMergedMedication[i].Stop_Date = DateTime.MinValue;
                                //}
                                if (lstMergedMedication[i].Deleted.ToUpper() == "Y")
                                    lstMedicationDelete.Add(lstMergedMedication[i]);
                                else
                                    lstMedicationSave.Add(lstMergedMedication[i]);
                            }
                        }
                    }

                    if (lstMedicationSave.Count > 0 || lstMedicationUpdate.Count > 0 || lstMedicationDelete.Count > 0)
                    {
                        RCopiaGenerateXML objMedicationmanager = new RCopiaGenerateXML();
                        objMedicationmanager.creteSendMedicationXml(lstMedicationSave.ToArray(), lstMedicationUpdate.ToArray(), lstMedicationDelete.ToArray(),ClientSession.LegalOrg);
                        //Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                        //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, ClientSession.UserName, ApplicationObject.macAddress, Convert.ToDateTime(hdnLocalDate.Value));

                    }



                }

                #endregion

                #region Allergy

                if (grdAllergyMergedList.Items.Count > 0)
                {
                    IList<Rcopia_Allergy> lstAllergySave = new List<Rcopia_Allergy>();
                    IList<Rcopia_Allergy> lstAllergyUpdate = new List<Rcopia_Allergy>();
                    IList<Rcopia_Allergy> lstAllergyDelete = new List<Rcopia_Allergy>();

                    if (Session["ActualAllergyList"] != null)
                    {
                        lstAllergy = (IList<Rcopia_Allergy>)Session["ActualAllergyList"];
                        for (int i = 0; i < lstAllergy.Count; i++)
                        {
                            lstAllergy[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                            lstAllergy[i].Modified_By = ClientSession.UserName;
                            lstAllergy[i].Deleted = "Y";
                            lstAllergyDelete.Add(lstAllergy[i]);
                        }
                    }

                    if (Session["MergedAllergyList"] != null)
                    {
                        lstMergedAllergy = (IList<Rcopia_Allergy>)Session["MergedAllergyList"];
                        for (int i = 0; i < lstMergedAllergy.Count; i++)
                        {
                            System.Web.UI.WebControls.DropDownList ddlSelectRow = ((System.Web.UI.WebControls.DropDownList)grdAllergyMergedList.Items[i].Cells[2].FindControl("ddlAllergyStatus"));
                            lstMergedAllergy[i].Status = ddlSelectRow.SelectedItem.Value;
                            if (lstMergedAllergy[i].Id != 0)
                            {
                                lstMergedAllergy[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                                lstMergedAllergy[i].Modified_By = ClientSession.UserName;
                                lstMergedAllergy[i].Deleted = "N";
                                lstAllergyUpdate.Add(lstMergedAllergy[i]);
                            }
                            else
                            {
                                lstMergedAllergy[i].Human_ID = ulHumanID;
                                lstMergedAllergy[i].Created_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                                lstMergedAllergy[i].Created_By = ClientSession.UserName;
                                lstMergedAllergy[i].Deleted = "N";
                                lstAllergySave.Add(lstMergedAllergy[i]);
                            }
                        }
                    }

                    if (lstAllergySave.Count > 0 || lstAllergyUpdate.Count > 0 || lstAllergyDelete.Count > 0)
                    {
                        RCopiaGenerateXML objMedicationmanager = new RCopiaGenerateXML();
                        objMedicationmanager.creteSendAllergyXml(lstAllergySave.ToArray(), lstAllergyUpdate.ToArray(), lstAllergyDelete.ToArray(),ClientSession.LegalOrg);


                    }


                }


                #endregion
            }
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
            ulong EncounterID = (Session["ReconciledEncID"] != null && Session["ReconciledEncID"].ToString() != "0") ? Convert.ToUInt64(Session["ReconciledEncID"]) : ClientSession.EncounterId;
            Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
            // objUpdateInfoMngr.DownloadRCopiaInfo(ClientSession.UserName, ApplicationObject.macAddress, Convert.ToDateTime(hdnLocalDate.Value), ClientSession.FacilityName, EncounterID);
            //Patient Level RCopia Download - Commented
            objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, Convert.ToDateTime(hdnLocalDate.Value), ClientSession.FacilityName, EncounterID, ulHumanID,ClientSession.LegalOrg);
            //Old
            //objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, Convert.ToDateTime(hdnLocalDate.Value), ClientSession.FacilityName, EncounterID);

            //  objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.UploadAddress, ClientSession.UserName, ApplicationObject.macAddress, Convert.ToDateTime(hdnLocalDate.Value), ClientSession.FacilityName, EncounterID);

            Rcopia_MedicationManager objMedicationMngr = new Rcopia_MedicationManager();

            IList<Rcopia_Medication> ilstRCopiaMed = new List<Rcopia_Medication>();
            ilstRCopiaMed = objMedicationMngr.GetRCopiaMedByHumanID(ulHumanID);
            for (int i = 0; i <= ilstRCopiaMed.Count - 1; i++)
            {
                for (int j = 0; j <= lstMedicationSave.Count - 1; j++)
                {
                    if (ilstRCopiaMed[i].Brand_Name == lstMedicationSave[j].Brand_Name)
                    {
                        ilstRCopiaMed[i].Rxnorm_ID = lstMedicationSave[j].Rxnorm_ID;
                        objMedicationMngr.UpdateToRcopia_Medication(ilstRCopiaMed[i], null);
                        break;
                    }
                }
            }

            lstMedication = objMedicationMngr.GetRCopiaMedByHumanID(ulHumanID);
            lstMergedMedication = new List<Rcopia_Medication>();
            Session["MergedMedicationList"] = null;
            lstIncorporatedMedication = new List<Rcopia_Medication>();
            Session["IncorporatedMedicationList"] = lstIncorporatedMedication;
            Session["ActualMedicationList"] = lstMedication;

            grdMedicationActualList.DataSource = new string[] { };
            grdMedicationActualList.DataBind();

            grdMedicationIncorporetedList.DataSource = new string[] { };
            grdMedicationIncorporetedList.DataBind();

            grdMedicationMergedList.DataSource = new string[] { };
            grdMedicationMergedList.DataBind();

            LoadGridMedicationActualList(lstMedication);


            Rcopia_AllergyManager objAllergy = new Rcopia_AllergyManager();
            lstAllergy = objAllergy.GetAllergyListUsingHumanId(ulHumanID);

            lstMergedAllergy = new List<Rcopia_Allergy>();
            Session["MergedAllergyList"] = null;
            lstIncorporatedAllergy = new List<Rcopia_Allergy>();
            Session["IncorporatedAllergyList"] = lstIncorporatedAllergy;
            Session["ActualAllergyList"] = lstAllergy;

            grdAllergyActualList.DataSource = new string[] { };
            grdAllergyActualList.DataBind();

            grdAllergyIncorporetedList.DataSource = new string[] { };
            grdAllergyIncorporetedList.DataBind();

            grdAllergyMergedList.DataSource = new string[] { };
            grdAllergyMergedList.DataBind();

            LoadGridAllergyActualList(lstAllergy);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ClinicalInformation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            if (is_save == true)
            {
                string ProbListText = "";
                string AllergyText = "";
                string MedicationText = "";
                List<string> lstToolTip = new List<string>();

                //var summaryList = new UtilityManager().LoadPatientSummaryList(out lstToolTip);
                IList<int> ilstChangeSummaryBar = new List<int>() { 1, 4, 5 };
                var summaryList = new UtilityManager().LoadPatientSummaryUsingList(ilstChangeSummaryBar, out lstToolTip);

                var sProblemList = summaryList.Where(a => a.ToUpper().StartsWith("PROBLEMLIST-")).ToList();
                if (sProblemList.Count > 0)
                    ProbListText = sProblemList[0].Replace("ProblemList-", "");

                var AllergyList = summaryList.Where(a => a.ToUpper().StartsWith("ALLERGY-")).ToList();
                if (AllergyList.Count > 0)
                    AllergyText = AllergyList[0].Replace("Allergy-", "");

                var MedicationList = summaryList.Where(a => a.ToUpper().StartsWith("MEDICATION-")).ToList();
                if (MedicationList.Count > 0)
                    MedicationText = MedicationList[0].Replace("Medication-", "");
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Test", "SetSummaryBar('" + ProbListText + "','" + AllergyText + "','" + MedicationText + "','" + (lstToolTip.Count > 0 ? lstToolTip[0] : string.Empty) + "');DisplayErrorMessage('115009');", true);
            }
            AuditLogManager alManager = new AuditLogManager();
            string TransactionType = "SAVE";
            alManager.InsertIntoAuditLog("CLINICAL RECONCILIATION", TransactionType, Convert.ToInt32(ClientSession.HumanId), ClientSession.UserName);//BugID:49685


            IList<ActivityLog> ActivityLogList = new List<ActivityLog>();
            ActivityLogManager ActivitylogMngr = new ActivityLogManager();
            ActivityLog activity = new ActivityLog();
            activity.Activity_Type = "CCD Reconcile";
            activity.Activity_Date_And_Time = DateTime.UtcNow;
            activity.Encounter_ID = (Session["ReconciledEncID"] != null && Session["ReconciledEncID"].ToString() != "0") ? Convert.ToUInt64(Session["ReconciledEncID"]) : ClientSession.EncounterId;//BugID:51164
            activity.Subject = Session["sFileNameBind"].ToString();
            activity.Human_ID = ClientSession.HumanId;
            activity.Activity_By = ClientSession.UserName;
            ActivityLogList.Add(activity);
            ActivitylogMngr.SaveActivityLogManager(ActivityLogList, string.Empty);


            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "btnClose_Clicked();", true);


        }

        protected void btnMedicationMerge_Click(object sender, EventArgs e)
        {

            if (grdMedicationMergedList.Items.Count > 0)
            {
                if (Session["MergedMedicationList"] != null)
                {
                    lstMergedMedication = (IList<Rcopia_Medication>)Session["MergedMedicationList"];
                }
            }

            if (grdMedicationIncorporetedList.Items.Count > 0)
            {

                IList<Rcopia_Medication> tempIncorpotatedList = (IList<Rcopia_Medication>)Session["IncorporatedMedicationList"];
                for (int i = 0; i < grdMedicationIncorporetedList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdMedicationIncorporetedList.Items[i].Cells[0].FindControl("chkMedicationIncorporated"));
                    if (chkSelectRow.Checked == true)
                    {
                        //Commented for BugID:30414 -Pujhitha
                        //if (tempIncorpotatedList[i].Start_Date == DateTime.MinValue && tempIncorpotatedList[i].Stop_Date == DateTime.MinValue)// || (tempIncorpotatedList[i].Start_Date != DateTime.MinValue && tempIncorpotatedList[i].Stop_Date != DateTime.MinValue))
                        //{
                        //    tempIncorpotatedList[i].Start_Date = Convert.ToDateTime(hdnLocalDate.Value);
                        //    tempIncorpotatedList[i].Stop_Date = DateTime.MinValue;
                        //}
                        tempIncorpotatedList[i].DataFrom = "Incorporated";
                        lstMergedMedication.Add(tempIncorpotatedList[i]);

                    }
                    else
                    {
                        lstIncorporatedMedication.Add(tempIncorpotatedList[i]);
                    }

                }

                //LoadGridMedicationIncorporatedList(lstIncorporatedMedication);
                Session["IncorporatedMedicationList"] = lstIncorporatedMedication;
                Session["MergedMedicationList"] = lstMergedMedication;
            }
            if (grdMedicationActualList.Items.Count > 0)
            {
                IList<Rcopia_Medication> tempActualList = (IList<Rcopia_Medication>)Session["ActualMedicationList"];
                for (int i = 0; i < grdMedicationActualList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdMedicationActualList.Items[i].Cells[0].FindControl("chkMedicationActual"));
                    if (chkSelectRow.Checked == true)
                    {

                        IList<Rcopia_Medication> ResultMedicationList = lstMergedMedication.Where(a => a.Generic_Name.Trim() + " " + a.Brand_Name.Trim() + " " + a.Strength.Trim() == tempActualList[i].Generic_Name.Trim() + " " + tempActualList[i].Brand_Name.Trim() + " " + tempActualList[i].Strength.Trim()).ToList<Rcopia_Medication>();
                        if (ResultMedicationList.Count > 0)
                        {
                            lstIncorporatedMedication.Add(ResultMedicationList[0]);
                            lstMergedMedication = lstMergedMedication.Where(b => b.Generic_Name.Trim() + " " + b.Brand_Name.Trim() + " " + b.Strength.Trim() != ResultMedicationList[0].Generic_Name.Trim() + " " + ResultMedicationList[0].Brand_Name.Trim() + " " + ResultMedicationList[0].Strength.Trim()).ToList<Rcopia_Medication>();
                        }
                        //Commented for BugID:30414 -Pujhitha
                        //tempActualList[i].Start_Date = Convert.ToDateTime(hdnLocalDate.Value);
                        //tempActualList[i].Stop_Date = DateTime.MinValue;
                        tempActualList[i].DataFrom = "Actual";
                        lstMergedMedication.Add(tempActualList[i]);
                    }
                    else
                    {
                        lstMedication.Add(tempActualList[i]);
                    }
                }

                Session["ActualMedicationList"] = lstMedication;
                Session["MergedMedicationList"] = lstMergedMedication;
            }
            LoadGridMedicationIncorporatedList(lstIncorporatedMedication);

            LoadGridMedicationActualList(lstMedication);
            if (lstMergedMedication.Count == 0)
            {
                grdMedicationMergedList.DataSource = new string[] { };
                grdMedicationMergedList.DataBind();
            }
            else
            {
                LoadGridMedicationMergedList(lstMergedMedication);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ClinicalInformation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnMedicationMerge_Recall(object sender, EventArgs e)
        {
            if (grdMedicationIncorporetedList.Items.Count > 0)
            {
                if (Session["IncorporatedMedicationList"] != null)
                {
                    lstIncorporatedMedication = (IList<Rcopia_Medication>)Session["IncorporatedMedicationList"];
                }
            }
            if (grdMedicationActualList.Items.Count > 0)
            {
                if (Session["ActualMedicationList"] != null)
                {
                    lstMedication = (IList<Rcopia_Medication>)Session["ActualMedicationList"];
                }
            }

            if (grdMedicationMergedList.Items.Count > 0)
            {
                IList<Rcopia_Medication> tempMergedList = (IList<Rcopia_Medication>)Session["MergedMedicationList"];
                for (int i = 0; i < grdMedicationMergedList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdMedicationMergedList.Items[i].Cells[0].FindControl("chkMedicationMerged"));
                    string DataFrom = tempMergedList[i].DataFrom;
                    if (DataFrom == "Incorporated")
                    {
                        if (chkSelectRow.Checked == true)
                        {
                            //Commented for BugID:30414 -Pujhitha
                            //if (tempIncorpotatedList[i].Start_Date == DateTime.MinValue && tempIncorpotatedList[i].Stop_Date == DateTime.MinValue)// || (tempIncorpotatedList[i].Start_Date != DateTime.MinValue && tempIncorpotatedList[i].Stop_Date != DateTime.MinValue))
                            //{
                            //    tempIncorpotatedList[i].Start_Date = Convert.ToDateTime(hdnLocalDate.Value);
                            //    tempIncorpotatedList[i].Stop_Date = DateTime.MinValue;
                            //}
                            lstIncorporatedMedication.Add(tempMergedList[i]);
                        }
                        else
                        {
                            lstMergedMedication.Add(tempMergedList[i]);
                        }
                    }
                    else
                    {

                        if (chkSelectRow.Checked == true)
                        {
                            //IList<Rcopia_Medication> ResultMedicationList = lstMergedMedication.Where(a => a.Generic_Name.Trim() + " " + a.Brand_Name.Trim() + " " + a.Strength.Trim() == tempActualList[i].Generic_Name.Trim() + " " + tempActualList[i].Brand_Name.Trim() + " " + tempActualList[i].Strength.Trim()).ToList<Rcopia_Medication>();
                            //if (ResultMedicationList.Count > 0)
                            // {
                            //          lstIncorporatedMedication.Add(ResultMedicationList[0]);
                            //          lstMergedMedication = lstMergedMedication.Where(b => b.Generic_Name.Trim() + " " + b.Brand_Name.Trim() + " " + b.Strength.Trim() != ResultMedicationList[0].Generic_Name.Trim() + " " + ResultMedicationList[0].Brand_Name.Trim() + " " + ResultMedicationList[0].Strength.Trim()).ToList<Rcopia_Medication>();
                            // }
                            lstMedication.Add(tempMergedList[i]);
                        }
                        else
                        {
                            lstMergedMedication.Add(tempMergedList[i]);
                        }
                    }

                }

                //LoadGridMedicationIncorporatedList(lstIncorporatedMedication);
                Session["IncorporatedMedicationList"] = lstIncorporatedMedication;
                Session["MergedMedicationList"] = lstMergedMedication;
                Session["ActualMedicationList"] = lstMedication;
                LoadGridMedicationMergedList(lstMergedMedication);

            }

            if (lstIncorporatedMedication.Count == 0)
            {
                grdMedicationIncorporetedList.DataSource = new string[] { };
                grdMedicationIncorporetedList.DataBind();
            }
            else
            {
                LoadGridMedicationIncorporatedList(lstIncorporatedMedication);
            }

            if (lstMedication.Count == 0)
            {
                grdMedicationActualList.DataSource = new string[] { };
                grdMedicationActualList.DataBind();
            }
            else
            {
                LoadGridMedicationActualList(lstMedication);
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "ClinicalInformation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }


        protected void btnAllergyMerge_Click(object sender, EventArgs e)
        {
            if (grdAllergyMergedList.Items.Count > 0)
            {
                if (Session["MergedAllergyList"] != null)
                {
                    lstMergedAllergy = (IList<Rcopia_Allergy>)Session["MergedAllergyList"];
                }
            }

            if (grdAllergyIncorporetedList.Items.Count > 0)
            {
                IList<Rcopia_Allergy> tempIncorpotatedList = (IList<Rcopia_Allergy>)Session["IncorporatedAllergyList"];
                for (int i = 0; i < grdAllergyIncorporetedList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdAllergyIncorporetedList.Items[i].Cells[0].FindControl("chkAllergyIncorporated"));
                    if (tempIncorpotatedList != null && tempIncorpotatedList.Count > 0)
                    {
                        if (chkSelectRow.Checked == true)
                        {
                            tempIncorpotatedList[i].DataFrom = "Incorporated";
                            lstMergedAllergy.Add(tempIncorpotatedList[i]);
                        }
                        else
                        {
                            lstIncorporatedAllergy.Add(tempIncorpotatedList[i]);
                        }
                    }
                }


                Session["IncorporatedAllergyList"] = lstIncorporatedAllergy;
                Session["MergedAllergyList"] = lstMergedAllergy;
            }
            if (grdAllergyActualList.Items.Count > 0)
            {
                IList<Rcopia_Allergy> tempActualList = (IList<Rcopia_Allergy>)Session["ActualAllergyList"];
                for (int i = 0; i < grdAllergyActualList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdAllergyActualList.Items[i].Cells[0].FindControl("chkAllergyActual"));
                    if (chkSelectRow.Checked == true)
                    {
                        IList<Rcopia_Allergy> ResultAllergyList = lstMergedAllergy.Where(a => a.Allergy_Name == tempActualList[i].Allergy_Name).ToList<Rcopia_Allergy>();
                        if (ResultAllergyList.Count > 0)
                        {

                            lstIncorporatedAllergy.Add(ResultAllergyList[0]);
                            lstMergedAllergy = lstMergedAllergy.Where(a => a.Allergy_Name != ResultAllergyList[0].Allergy_Name).ToList<Rcopia_Allergy>();
                        }
                        tempActualList[i].DataFrom = "Actual";
                        lstMergedAllergy.Add(tempActualList[i]);
                    }
                    else
                    {
                        lstAllergy.Add(tempActualList[i]);
                    }

                }

                Session["ActualAllergyList"] = lstAllergy;
                Session["MergedAllergyList"] = lstMergedAllergy;
            }
            LoadGridAllergyIncorporatedList(lstIncorporatedAllergy);
            LoadGridAllergyActualList(lstAllergy);
            if (lstMergedAllergy.Count == 0)
            {
                grdAllergyMergedList.DataSource = new string[] { };
                grdAllergyMergedList.DataBind();
            }
            else
            {
                LoadGridAllergyMergedList(lstMergedAllergy);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Merge Alergy", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnAllergyMerge_Recall(object sender, EventArgs e)
        {
            if (grdAllergyIncorporetedList.Items.Count > 0)
            {
                if (Session["IncorporatedAllergyList"] != null)
                {
                    lstIncorporatedAllergy = (IList<Rcopia_Allergy>)Session["IncorporatedAllergyList"];
                }
            }
            if (grdAllergyActualList.Items.Count > 0)
            {
                if (Session["ActualAllergyList"] != null)
                {
                    lstAllergy = (IList<Rcopia_Allergy>)Session["ActualAllergyList"];
                }
            }

            if (grdAllergyMergedList.Items.Count > 0)
            {
                IList<Rcopia_Allergy> tempMergedList = (IList<Rcopia_Allergy>)Session["MergedAllergyList"];
                for (int i = 0; i < grdAllergyMergedList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdAllergyMergedList.Items[i].Cells[0].FindControl("chkAllergyMerged"));
                    string DataFrom = tempMergedList[i].DataFrom;
                    if (DataFrom == "Incorporated")
                    {
                        if (chkSelectRow.Checked == true)
                        {

                            lstIncorporatedAllergy.Add(tempMergedList[i]);
                        }
                        else
                        {
                            lstMergedAllergy.Add(tempMergedList[i]);
                        }
                    }
                    else
                    {

                        if (chkSelectRow.Checked == true)
                        {

                            lstAllergy.Add(tempMergedList[i]);
                        }
                        else
                        {
                            lstMergedAllergy.Add(tempMergedList[i]);
                        }
                    }

                }


                Session["IncorporatedAllergyList"] = lstIncorporatedAllergy;
                Session["MergedAllergyList"] = lstMergedAllergy;
                Session["ActualAllergyList"] = lstAllergy;
                LoadGridAllergyMergedList(lstMergedAllergy);

            }

            if (lstIncorporatedAllergy.Count == 0)
            {
                grdAllergyIncorporetedList.DataSource = new string[] { };
                grdAllergyIncorporetedList.DataBind();
            }
            else
            {
                LoadGridAllergyIncorporatedList(lstIncorporatedAllergy);
            }

            if (lstAllergy.Count == 0)
            {
                grdAllergyActualList.DataSource = new string[] { };
                grdAllergyActualList.DataBind();
            }
            else
            {
                LoadGridAllergyActualList(lstAllergy);
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "ClinicalInformation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnProblemMerge_Click(object sender, EventArgs e)
        {


            if (grdProblemMergedList.Items.Count > 0)
            {
                if (Session["MergedProblemList"] != null)
                {
                    lstMergedProblem = (IList<ProblemList>)Session["MergedProblemList"];
                }
            }

            if (grdProblemIncorporatedList.Items.Count > 0)
            {
                lstIncorporatedProblem = new List<ProblemList>();
                IList<ProblemList> tempIncorpotatedList = (IList<ProblemList>)Session["IncorporatedProblemList"];
                for (int i = 0; i < grdProblemIncorporatedList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdProblemIncorporatedList.Items[i].Cells[0].FindControl("chkProblemIncorporated"));

                    if (tempIncorpotatedList != null && tempIncorpotatedList.Count > 0)
                    {
                        if (chkSelectRow.Checked == true)
                        {
                            //tempIncorpotatedList[i].Status = "Active";
                            tempIncorpotatedList[i].DataFrom = "Incorporated";
                            lstMergedProblem.Add(tempIncorpotatedList[i]);
                        }
                        else
                        {
                            lstIncorporatedProblem.Add(tempIncorpotatedList[i]);
                        }
                    }
                }

                Session["IncorporatedProblemList"] = lstIncorporatedProblem;
                Session["MergedProblemList"] = lstMergedProblem;
            }
            if (grdProblemActualList.Items.Count > 0)
            {
                lstProblem = new List<ProblemList>();
                IList<ProblemList> tempActualList = (IList<ProblemList>)Session["ActualProblemList"];
                for (int i = 0; i < grdProblemActualList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdProblemActualList.Items[i].Cells[0].FindControl("chkProblemActual"));
                    if (chkSelectRow.Checked == true)
                    {
                        IList<ProblemList> ResultProblemList = lstMergedProblem.Where(a => a.Problem_Description == tempActualList[i].Problem_Description).ToList<ProblemList>();
                        if (ResultProblemList.Count > 0)
                        {
                            lstIncorporatedProblem.Add(ResultProblemList[0]);
                            lstMergedProblem = lstMergedProblem.Where(a => a.Problem_Description != ResultProblemList[0].Problem_Description).ToList<ProblemList>();
                        }
                        //tempActualList[i].Status = "Active";
                        tempActualList[i].Modified_Date_And_Time = Convert.ToDateTime(hdnLocalDate.Value);
                        tempActualList[i].DataFrom = "Actual";
                        lstMergedProblem.Add(tempActualList[i]);
                    }
                    else
                    {
                        lstProblem.Add(tempActualList[i]);
                    }

                }
                Session["ActualProblemList"] = lstProblem;
                Session["MergedProblemList"] = lstMergedProblem;
            }
            LoadGridProblemIncorporatedList(lstIncorporatedProblem);
            LoadGridProblemActualList(lstProblem);
            if (lstMergedProblem.Count == 0)
            {
                grdProblemMergedList.DataSource = new string[] { };
                grdProblemMergedList.DataBind();
            }
            else
            {
                LoadGridProblemMergedList(lstMergedProblem);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Merge ProblemList", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        protected void btnProblemMerge_Recall(object sender, EventArgs e)
        {

            if (grdProblemIncorporatedList.Items.Count > 0)
            {
                if (Session["IncorporatedProblemList"] != null)
                {
                    lstIncorporatedProblem = (IList<ProblemList>)Session["IncorporatedProblemList"];
                }
            }
            if (grdProblemActualList.Items.Count > 0)
            {
                if (Session["ActualProblemList"] != null)
                {
                    lstProblem = (IList<ProblemList>)Session["ActualProblemList"];
                }
            }

            if (grdProblemMergedList.Items.Count > 0)
            {
                IList<ProblemList> tempMergedList = (IList<ProblemList>)Session["MergedProblemList"];
                for (int i = 0; i < grdProblemMergedList.Items.Count; i++)
                {
                    System.Web.UI.WebControls.CheckBox chkSelectRow = ((System.Web.UI.WebControls.CheckBox)grdProblemMergedList.Items[i].Cells[0].FindControl("chkProblemMerged"));
                    string DataFrom = tempMergedList[i].DataFrom;
                    if (DataFrom == "Incorporated")
                    {
                        if (chkSelectRow.Checked == true)
                        {

                            lstIncorporatedProblem.Add(tempMergedList[i]);
                        }
                        else
                        {
                            lstMergedProblem.Add(tempMergedList[i]);
                        }
                    }
                    else
                    {

                        if (chkSelectRow.Checked == true)
                        {

                            lstProblem.Add(tempMergedList[i]);
                        }
                        else
                        {
                            lstMergedProblem.Add(tempMergedList[i]);
                        }
                    }

                }


                Session["IncorporatedProblemList"] = lstIncorporatedProblem;
                Session["MergedProblemList"] = lstMergedProblem;
                Session["ActualProblemList"] = lstProblem;
                LoadGridProblemMergedList(lstMergedProblem);

            }

            if (lstIncorporatedProblem.Count == 0)
            {
                grdProblemIncorporatedList.DataSource = new string[] { };
                grdProblemIncorporatedList.DataBind();
            }
            else
            {
                LoadGridProblemIncorporatedList(lstIncorporatedProblem);
            }

            if (lstProblem.Count == 0)
            {
                grdProblemActualList.DataSource = new string[] { };
                grdProblemActualList.DataBind();
            }
            else
            {
                LoadGridProblemActualList(lstProblem);
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "ClinicalInformation", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        public void LoadGridProblemMergedList(IList<ProblemList> MergedProblemlist)
        {
            grdProblemMergedList.DataSource = new string[] { };
            grdProblemMergedList.DataBind();

            if (MergedProblemlist.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Name", typeof(string)));

                dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Problem ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Data From", typeof(string)));
                for (int i = 0; i < MergedProblemlist.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["Name"] = MergedProblemlist[i].Problem_Description;

                    //if (MergedProblemlist[i].Id != 0)
                    //{
                    //    if (MergedProblemlist[i].Modified_Date_And_Time == DateTime.MinValue)
                    //        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedProblemlist[i].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                    //    else
                    //        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedProblemlist[i].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                    //}
                    //else
                    //    dr["Last Modified Date"] = MergedProblemlist[i].Date_Diagnosed;
                    dr["Last Modified Date"] = UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy");
                    dr["Problem ID"] = MergedProblemlist[i].Id;
                    dr["Data From"] = MergedProblemlist[i].DataFrom;
                    dt.Rows.Add(dr);
                }

                grdProblemMergedList.DataSource = dt;
                grdProblemMergedList.DataBind();
            }
            //Added Status ComboBox to allow change of Reconciled Item Status _ for MACRA MEASURES
            for (int i = 0; i < MergedProblemlist.Count; i++)
            {
                string status = string.Empty;
                status = MergedProblemlist[i].Status;

                System.Web.UI.WebControls.DropDownList ddlSelectRow = ((System.Web.UI.WebControls.DropDownList)grdProblemMergedList.Items[i].Cells[2].FindControl("ddlProblemStatus"));
                for (int j = 0; j < ddlSelectRow.Items.Count; j++)
                {
                    if (ddlSelectRow.Items[j].Value.ToUpper() == status.ToUpper())
                    {
                        ddlSelectRow.Items[j].Selected = true;
                    }
                }
            }
        }

        public void LoadGridAllergyMergedList(IList<Rcopia_Allergy> MergedAllergylist)
        {

            grdAllergyMergedList.DataSource = new string[] { };
            grdAllergyMergedList.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Reaction", typeof(string)));

            dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Rcopia Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Data From", typeof(string)));

            for (int i = 0; i < MergedAllergylist.Count; i++)
            {
                dr = dt.NewRow();
                dr["Name"] = MergedAllergylist[i].Allergy_Name;
                dr["Reaction"] = MergedAllergylist[i].Reaction;

                //if (MergedAllergylist[i].Id != 0)
                //{
                //    if (MergedAllergylist[i].Modified_Date_And_Time == DateTime.MinValue)
                //        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedAllergylist[i].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                //    else
                //        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedAllergylist[i].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                //}
                //else
                //    dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedAllergylist[i].OnsetDate).ToString("dd-MMM-yyyy");
                dr["Last Modified Date"] = UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy");
                dr["Rcopia Id"] = MergedAllergylist[i].Id;
                dr["Data From"] = MergedAllergylist[i].DataFrom;
                dt.Rows.Add(dr);
            }

            grdAllergyMergedList.DataSource = dt;
            grdAllergyMergedList.DataBind();
            //Added Status ComboBox to allow change of Reconciled Item Status _ for MACRA MEASURES
            for (int i = 0; i < MergedAllergylist.Count; i++)
            {
                string status = string.Empty;
                if (MergedAllergylist[i].Deleted == "")
                    status = MergedAllergylist[i].Status;
                else if (MergedAllergylist[i].Deleted.ToUpper() == "N")
                    status = "Active";
                else
                    status = "Inactive";

                System.Web.UI.WebControls.DropDownList ddlSelectRow = ((System.Web.UI.WebControls.DropDownList)grdAllergyMergedList.Items[i].Cells[2].FindControl("ddlAllergyStatus"));
                for (int j = 0; j < ddlSelectRow.Items.Count; j++)
                {
                    if (ddlSelectRow.Items[j].Value.ToUpper() == status.ToUpper())
                    {
                        ddlSelectRow.Items[j].Selected = true;
                    }
                }
            }
        }

        public void LoadGridMedicationMergedList(IList<Rcopia_Medication> MergedMedicationList)
        {

            grdMedicationMergedList.DataSource = new string[] { };
            grdMedicationMergedList.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Last Modified Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Rcopia Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Data From", typeof(string)));
            for (int i = 0; i < MergedMedicationList.Count; i++)
            {
                dr = dt.NewRow();//sa

                dr["Name"] = MergedMedicationList[i].Brand_Name + " " + MergedMedicationList[i].Strength + " " + MergedMedicationList[i].Form + " " + MergedMedicationList[i].Route + " " + MergedMedicationList[i].Dose + " " + MergedMedicationList[i].Dose_Unit + " " + MergedMedicationList[i].Dose_Timing + " " + MergedMedicationList[i].Patient_Notes;

                //if (MergedMedicationList[i].Id != 0)
                //{
                //    if (MergedMedicationList[i].Modified_Date_And_Time == DateTime.MinValue)
                //        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedMedicationList[i].Created_Date_And_Time).ToString("dd-MMM-yyyy");
                //    else
                //        dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedMedicationList[i].Modified_Date_And_Time).ToString("dd-MMM-yyyy");
                //}
                //else
                //    dr["Last Modified Date"] = UtilityManager.ConvertToLocal(MergedMedicationList[i].Start_Date).ToString("dd-MMM-yyyy");
                dr["Last Modified Date"] = UtilityManager.ConvertToLocal(DateTime.UtcNow).ToString("dd-MMM-yyyy");
                dr["Rcopia Id"] = MergedMedicationList[i].Id;
                dr["Data From"] = MergedMedicationList[i].DataFrom;
                dt.Rows.Add(dr);

            }

            grdMedicationMergedList.DataSource = dt;
            grdMedicationMergedList.DataBind();
            //Added Status ComboBox to allow change of Reconciled Item Status _ for MACRA MEASURES
            for (int i = 0; i < MergedMedicationList.Count; i++)
            {
                string status = string.Empty;
                if (MergedMedicationList[i].Start_Date != DateTime.MinValue && MergedMedicationList[i].Stop_Date == DateTime.MinValue)
                    status = "Active";
                else if (MergedMedicationList[i].Start_Date == DateTime.MinValue && MergedMedicationList[i].Stop_Date == DateTime.MinValue)
                    status = "Inactive";
                else if ((MergedMedicationList[i].Start_Date != DateTime.MinValue && MergedMedicationList[i].Stop_Date != DateTime.MinValue) || (MergedMedicationList[i].Start_Date == DateTime.MinValue && MergedMedicationList[i].Stop_Date != DateTime.MinValue))
                {
                    if (MergedMedicationList[i].Stop_Date < UtilityManager.ConvertToLocal(DateTime.UtcNow))
                        status = "Inactive";
                    else
                        status = "Active";
                }
                System.Web.UI.WebControls.DropDownList ddlSelectRow = ((System.Web.UI.WebControls.DropDownList)grdMedicationMergedList.Items[i].Cells[2].FindControl("ddlMedicationStatus"));
                for (int j = 0; j < ddlSelectRow.Items.Count; j++)
                {
                    if (ddlSelectRow.Items[j].Value.ToUpper() == status.ToUpper())
                    {
                        ddlSelectRow.Items[j].Selected = true;
                    }
                }
            }
        }


        public void LoadEmptyGrid()
        {
            grdMedicationIncorporetedList.DataSource = new string[] { };
            grdMedicationIncorporetedList.DataBind();

            grdMedicationMergedList.DataSource = new string[] { };
            grdMedicationMergedList.DataBind();

            grdAllergyIncorporetedList.DataSource = new string[] { };
            grdAllergyIncorporetedList.DataBind();

            grdAllergyMergedList.DataSource = new string[] { };
            grdAllergyMergedList.DataBind();

            grdProblemIncorporatedList.DataSource = new string[] { };
            grdProblemIncorporatedList.DataBind();

            grdProblemMergedList.DataSource = new string[] { };
            grdProblemMergedList.DataBind();
        }



    }
}
