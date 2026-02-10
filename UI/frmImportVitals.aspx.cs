using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
//using Microsoft.Office.Interop.Excel;
//using Acurus.Capella.FrontOffice;
using Acurus.Capella.DataAccess.ManagerObjects;
using Telerik.Web.UI;
using Acurus.Capella.UI.UserControls;
using System.IO;
using System.Xml;



namespace Acurus.Capella.UI
{
    public partial class frmImportVitals : System.Web.UI.Page
    {
        //ulong MyHumanID = 14410;
        ulong MyHumanID = 0;


        EncounterManager encMngr = new EncounterManager();
        IList<PatientPane> patientPane;
        bool Panelbar = true;
        ulong ulMyPhysicianID = 0;
        bool Is_Saved = false;
        PhysicianManager PhyMngr = new PhysicianManager();
        ulong Physcian_ID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClientSession.HumanId == null || ClientSession.HumanId == 0)
            {
                MyHumanID = Convert.ToUInt64(Request["MyHumanID"].ToString());
            }
            else
            {
                if (Request["MyHumanID"] != null)// && Request["MyHumanID"].ToString() != string.Empty)
                    MyHumanID = Convert.ToUInt64(Request["MyHumanID"].ToString());
                else
                    MyHumanID = ClientSession.HumanId;
            }
            ClientSession.HumanId = MyHumanID;
            if (ClientSession.PhysicianId == null || ClientSession.PhysicianId == 0)
            {
                ulMyPhysicianID = 0;
            }
            else if(ClientSession.UserRole.ToUpper() == "SCRIBE" || ClientSession.UserRole.ToUpper() == "MEDICAL ASSISTANT" || ClientSession.UserRole.ToUpper() != "OFFICE MANAGER" || ClientSession.UserRole.ToUpper() != "FRONT OFFICE" || ClientSession.UserRole.ToUpper() != "CODER")
            {
                ulMyPhysicianID = 0;
            }
            else
            {
                ulMyPhysicianID = ClientSession.PhysicianId;
            }

            //patientPane = new List<PatientPane>();
            ////Srividhya added the last parameter as set as true
            //patientPane = encMngr.FillPatientPane(MyHumanID, string.Empty, ClientSession.UserName,true);
            //var EncList = from el in patientPane where el.Date_of_Service.Date != DateTime.MinValue select el;
            //ClientSession.PatientPaneList = EncList.ToList<PatientPane>();
            //string sPriPlan = string.Empty;
            //string sSecPlan = string.Empty;
            //string sPriCarrier = string.Empty;
            //string sSecCarrier = string.Empty;

            //if (patientPane != null && patientPane.Count > 0)
            //{
            //    if (patientPane[patientPane.Count - 1].Insurance_Plan_ID != null)
            //    {

            //        for (int i = 0; i < patientPane[patientPane.Count - 1].Insurance_Plan_ID.Count; i++)
            //        {
            //            if (patientPane[patientPane.Count - 1].Insurance_Type[i].ToString() == "PRIMARY")
            //            {
            //                sPriPlan = patientPane[patientPane.Count - 1].Ins_Plan_Name[i].ToString();
            //                sPriCarrier = patientPane[patientPane.Count - 1].CarrierName[i].ToString();
            //            }

            //            if (patientPane[patientPane.Count - 1].Insurance_Type[i].ToString() == "SECONDARY")
            //            {
            //                sSecPlan = patientPane[patientPane.Count - 1].Ins_Plan_Name[i].ToString();
            //                sSecCarrier = patientPane[patientPane.Count - 1].CarrierName[i].ToString();
            //            }
            //        }
            //    }
            //    lblPatientStrip.Items[0].Text = FillPatientSummaryBarforPatientChart(patientPane[0].Last_Name, patientPane[0].First_Name, patientPane[0].MI, patientPane[0].Suffix, patientPane[0].Birth_Date, patientPane[0].Human_Id, patientPane[0].Medical_Record_Number, patientPane[0].HomePhoneNo, patientPane[0].Sex, patientPane[0].Patient_Status, patientPane[0].SSN, sPriPlan, sPriCarrier, sSecPlan, sSecCarrier);
            //    string PanelToolTip = lblPatientStrip.Items[0].Text;

            //    int indexPri = PanelToolTip.IndexOf("Pri Plan:");
            //    int indexSec = PanelToolTip.IndexOf("Sec Plan:");
            //    int indexSSN = PanelToolTip.IndexOf("SSN:");
            //    if (indexPri != -1)
            //    {
            //        lblPatientStrip.ToolTip = PanelToolTip.Insert(indexPri, "\n");
            //    }
            //    else if (indexSec != -1)
            //    {
            //        lblPatientStrip.ToolTip = PanelToolTip.Insert(indexSec, "\n");
            //    }
            //    else if (indexSSN != -1)
            //    {
            //        lblPatientStrip.ToolTip = PanelToolTip.Insert(indexSSN, "\n");
            //    }
            //    else
            //    {
            //        lblPatientStrip.ToolTip = lblPatientStrip.Items[0].Text;
            //    }
            //}

            Human objFillHuman = new Human();
            IList<Human> lstHuman = new List<Human>();
            string sBirth_Date = string.Empty;
            string sPatientstrip = string.Empty;
            XmlTextReader XmlText = null;
            if (MyHumanID != 0)
            {
            ln:
                try
                {
                    string sdivPatientstrip = UtilityManager.FillPatientStrip(MyHumanID);
                    if (sdivPatientstrip != null)
                    {
                        lblPatientStrip.Items[0].Text = sdivPatientstrip;
                    }
                }
                catch (Exception ex)
                {
                    XmlText.Close();
                    //Thread.Sleep(5000);
                    UtilityManager.GenerateXML(MyHumanID.ToString(), "Human");

                    goto ln;
                }


                //ln:

                //    string FileName = "Human" + "_" + MyHumanID + ".xml";
                //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //    try
                //    {
                //        if (File.Exists(strXmlFilePath) == true)
                //        {
                //            XmlDocument itemDoc = new XmlDocument();
                //            XmlText = new XmlTextReader(strXmlFilePath);
                //            XmlNodeList xmlTagName = null;
                //            using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //            {
                //                itemDoc.Load(fs);
                //                XmlText.Close();

                //                if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
                //                {
                //                    xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

                //                    if (xmlTagName != null)
                //                    {
                //                        for (int j = 0; j < xmlTagName.Count; j++)
                //                        {
                //                            if (xmlTagName[j].Attributes["Id"].Value == MyHumanID.ToString())
                //                            {
                //                                objFillHuman.Birth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value);
                //                                sBirth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value).ToString("dd-MMM-yyyy");
                //                                objFillHuman.Id = Convert.ToUInt32(xmlTagName[j].Attributes["Id"].Value);
                //                                objFillHuman.Last_Name = xmlTagName[j].Attributes["Last_Name"].Value;
                //                                objFillHuman.First_Name = xmlTagName[j].Attributes["First_Name"].Value;
                //                                objFillHuman.MI = xmlTagName[j].Attributes["MI"].Value;
                //                                objFillHuman.Suffix = xmlTagName[j].Attributes["Suffix"].Value;
                //                                objFillHuman.Sex = xmlTagName[j].Attributes["Sex"].Value;
                //                                objFillHuman.Work_Phone_No = xmlTagName[j].Attributes["Work_Phone_No"].Value;
                //                                objFillHuman.Work_Phone_Ext = xmlTagName[j].Attributes["Work_Phone_Ext"].Value;
                //                                objFillHuman.Home_Phone_No = xmlTagName[j].Attributes["Home_Phone_No"].Value;
                //                                objFillHuman.Cell_Phone_Number = xmlTagName[j].Attributes["Cell_Phone_Number"].Value;
                //                                if (xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != null && xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != string.Empty)
                //                                    objFillHuman.ACO_Is_Eligible_Patient = xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value.ToString();
                //                                else
                //                                    objFillHuman.ACO_Is_Eligible_Patient = "";

                //                                lstHuman.Add(objFillHuman);
                //                            }
                //                        }

                //                        string phoneno = "";

                //                        if (lstHuman != null && lstHuman.Count > 0)
                //                        {

                //                            if (objFillHuman.Home_Phone_No.Length == 14)
                //                            {
                //                                phoneno = objFillHuman.Home_Phone_No;
                //                            }
                //                            else
                //                            {
                //                                phoneno = objFillHuman.Cell_Phone_Number;
                //                            }

                //                        }

                //                        string sPatientSex = string.Empty;


                //                        if (objFillHuman.Sex != string.Empty)
                //                        {
                //                            if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
                //                            {
                //                                sPatientSex = "UNK";
                //                            }
                //                            else
                //                            {
                //                                sPatientSex = objFillHuman.Sex.Substring(0, 1);
                //                            }
                //                        }
                //                        else
                //                        {
                //                            sPatientSex = "";
                //                        }

                //                        string sAcoEligiblePatient = string.Empty;
                //                        sAcoEligiblePatient = objFillHuman.ACO_Is_Eligible_Patient;

                //                        sPatientstrip = " " + objFillHuman.Last_Name + "," + objFillHuman.First_Name
                //                            + "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + " | " +
                //    objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
                //   (CalculateAge(objFillHuman.Birth_Date)).ToString() +
                //   "  year(s) | " + sPatientSex + " | Acc #:" + MyHumanID.ToString() +
                //   " | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
                //   "Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type + " | ";

                //                        if (sAcoEligiblePatient != null && sAcoEligiblePatient != string.Empty && sAcoEligiblePatient != "N")
                //                        {
                //                            sPatientstrip += sAcoEligiblePatient + "   |   ";
                //                        }

                //                        lblPatientStrip.Items[0].Text = sPatientstrip;
                //                    }
                //                }
                //                fs.Close();
                //                fs.Dispose();
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        XmlText.Close();
                //        //Thread.Sleep(5000);
                //        UtilityManager.GenerateXML(MyHumanID.ToString(), "Human");

                //        goto ln;
                //    }
            }

                //lblPatientStrip.Items[0].Text = ClientSession.PatientPane.Replace("^", "");
                //lblPatientStrip.ToolTip = lblPatientStrip.Items[0].Text;
                txtPhysicianName.ReadOnly = true;
            txtPhysicianName.ForeColor = System.Drawing.Color.Black;
            txtPhysicianName.BorderWidth = 1;
            txtPhysicianName.BorderColor = System.Drawing.Color.Black;
            //txtPhysicianName.BackColor = Color.FromArgb(191, 219, 255);

            //FillPhysicianUser PhyUserList;
            ////PhyUserList = PhyMngr.GetPhysicianandUser(true, ClientSession.FacilityName);
            //PhyUserList = PhyMngr.GetPhysicianandUser(true, ClientSession.FacilityName);

            // for (int i = 0; i < PhyUserList.PhyList.Count; i++)
            //    {
            //        string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;

            //        if (Convert.ToUInt32(PhyUserList.UserList[i].Physician_Library_ID) == Convert.ToInt32(ulMyPhysicianID))
            //        {
            //            txtPhysicianName.Text=sPhyName;
            //        }
            //    }

            if (ulMyPhysicianID > 0 && ClientSession.UserRole != "Medical Assistant")// && cboPhysicianName.SelectedIndex >= 0)
            {
                if (ClientSession.SummaryList.Split('|').Count() > 3)
                    txtPhysicianName.Text = ClientSession.SummaryList.Split('|')[4].ToString();
                else
                {
                    PhysicianLibrary PhyName = new PhysicianLibrary();
                    IList<PhysicianLibrary> PhyList = new List<PhysicianLibrary>();
                    Physcian_ID = ulMyPhysicianID;
                    txtPhysicianName.Enabled = false;
                    PhyList = PhyMngr.GetphysiciannameByPhyID(ulMyPhysicianID);
                    if (PhyList.Count > 0)
                    {
                        if (ClientSession.FacilityName != PhyList[0].PhyAddress1 && txtPhysicianName.Text == string.Empty)
                        {

                            if (PhyList != null && PhyList.Count > 0)
                            {
                                txtPhysicianName.Text = PhyList[0].PhyPrefix + " " + PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhyLastName + " " + PhyList[0].PhySuffix;
                            }
                        }
                    }
                    else
                    {
                        if (PhyList != null && PhyList.Count > 0)
                        {
                            txtPhysicianName.Text = PhyList[0].PhyPrefix + " " + PhyList[0].PhyFirstName + " " + PhyList[0].PhyMiddleName + " " + PhyList[0].PhyLastName + " " + PhyList[0].PhySuffix;
                        }
                    }
                }


                btnGetVitals_Click(new object(), new EventArgs());
            }
            else
            {
                ulMyPhysicianID = 0;
                txtPhysicianName.Visible = false;
                lblPhysicianName.Visible = false;
                pnlImportVitals.Visible = false;
                //tableLayoutPanel1.RowStyles.RemoveAt(1); 

                btnGetVitals_Click(new object(), new EventArgs());
            }
            if (ClientSession.UserRole != "Medical Assistant")
            {
                txtPhysicianName.Enabled = false;
            }
            btnGetVitals.Visible = false;

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

        protected void btnGetVitals_Click(object sender, EventArgs e)
        {
            //if (objVitals != null && objVitals.btnSaveVitals.Enabled == true)
            //{
            //    //frmImportVitals_FormClosing(new object(), new FormClosingEventArgs(CloseReason.None, false));
            //    if (Is_Saved == true)
            //    {
            //        // this.Close();
            //        this.Visible = true;
            //        Is_Saved = false;

            //    }
            //    else if (Is_Saved == false)
            //    {
            //        return;
            //    }
            //}

            if (Convert.ToInt32(ulMyPhysicianID) < 0)
            {
                ulMyPhysicianID = 0;
                //ApplicationObject.erroHandler.DisplayErrorMessage("201013", this.Text);
                //cboPhysicianName.Focus();
                //return;
            }
            else if (Convert.ToInt32(ulMyPhysicianID) >= 0)
            {
                //if (cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Tag != null)
                //{
                //ulMyPhysicianID = Physcian_ID;//Convert.ToUInt64(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Tag);
                //}
                //if (cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value != null)
                //{
                //    ulMyPhysicianID = Convert.ToUInt64(cboPhysicianName.Items[cboPhysicianName.SelectedIndex].Value);
                //}
                ulMyPhysicianID = Physcian_ID;

            }
            if (ClientSession.UserRole != "Medical Assistant")
            {
                //  cboPhysicianName.Enabled = false;
                //  chkshowallPhysicians.Enabled = false;
                //  btnGetVitals.Enabled = false;
            }
            string sDate = hdnLocalTime.Value;
            //string sDate = hdnVitalTime.Value;
            if (sDate.Trim() == string.Empty && Request["Date"] != null)
                sDate = Request["Date"].ToString();
            string Openingfrom = "Menu";


            VitalsContainer.Attributes["src"] = "frmVitals.aspx?Openingfrom=" + Openingfrom + "&EncounterID=" + ClientSession.EncounterId.ToString() + "&PhyID=" + ulMyPhysicianID.ToString() + "&HumanID=" + MyHumanID.ToString() + "&Date=" + sDate;
        }

        protected void cboPhysicianName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }

        protected void chkshowallPhysicians_CheckedChanged(object sender, EventArgs e)
        {
            //FillPhysicianUser PhyUserList;
            //cboPhysicianName.Items.Clear();

            //if (chkshowallPhysicians.Checked == true)
            //{
            //    PhyUserList = PhyMngr.GetPhysicianandUser(false,string.Empty);

            //    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
            //    {
            //        string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
            //        cboPhysicianName.Items.Add(new RadComboBoxItem(PhyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
            //        cboPhysicianName.Items[i].Value = PhyUserList.PhyList[i].Id.ToString();

            //        if (Convert.ToUInt64(cboPhysicianName.Items[i].Value) == ulMyPhysicianID)
            //        {
            //            cboPhysicianName.SelectedIndex = i;
            //        }
            //    }
            //}
            //else
            //{

            //    PhyUserList = PhyMngr.GetPhysicianandUser(true, ClientSession.FacilityName);


            //    for (int i = 0; i < PhyUserList.PhyList.Count; i++)
            //    {
            //        string sPhyName = PhyUserList.PhyList[i].PhyPrefix + " " + PhyUserList.PhyList[i].PhyFirstName + " " + PhyUserList.PhyList[i].PhyMiddleName + " " + PhyUserList.PhyList[i].PhyLastName + " " + PhyUserList.PhyList[i].PhySuffix;
            //        cboPhysicianName.Items.Add(new RadComboBoxItem(PhyUserList.UserList[i].user_name.ToString() + " - " + sPhyName));
            //        cboPhysicianName.Items[i].Value = PhyUserList.PhyList[i].Id.ToString();

            //        if (Convert.ToUInt64(cboPhysicianName.Items[i].Value) == ulMyPhysicianID)
            //        {
            //            cboPhysicianName.SelectedIndex = i;
            //        }
            //    }

            //}
        }
    }
}
