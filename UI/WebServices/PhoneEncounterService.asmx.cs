using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for PhoneEncounterService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PhoneEncounterService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string LoadPhoneEncounter(string sHumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            ulong ulHumanID = Convert.ToUInt64(sHumanID);
            Human objFillHuman = new Human();
            IList<Human> lstHuman = new List<Human>();
            string sBirth_Date = string.Empty;
            string sPatientstrip = string.Empty;
            XmlTextReader XmlText = null;
            if (ulHumanID != 0)
            {
                 ln:

                IList<String> ilstPhoneEncounterTaglist = new List<String>();
                ilstPhoneEncounterTaglist.Add("HumanList");

                IList<object> ilstPhoneEncounterFinal = new List<object>();
                ilstPhoneEncounterFinal = UtilityManager.ReadBlob(ulHumanID, ilstPhoneEncounterTaglist);
                try { 
                if (ilstPhoneEncounterFinal != null && ilstPhoneEncounterFinal.Count > 0)
                {
                    if (ilstPhoneEncounterFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstPhoneEncounterFinal[0]).Count; iCount++)
                        {
                            objFillHuman = ((Human)((IList<object>)ilstPhoneEncounterFinal[0])[iCount]);
                            lstHuman.Add(objFillHuman);

                        }
                    }

                }



                //string FileName = "Human" + "_" + ulHumanID + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //try
                //{
                //    if (File.Exists(strXmlFilePath) == true)
                //    {
                //        XmlDocument itemDoc = new XmlDocument();
                //         XmlText = new XmlTextReader(strXmlFilePath);
                //        XmlNodeList xmlTagName = null;
                //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //        {
                //            itemDoc.Load(fs);
                //            XmlText.Close();

                //            if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
                //            {
                //                xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

                //                if (xmlTagName != null)
                //                {
                //                    for (int j = 0; j < xmlTagName.Count; j++)
                //                    {
                //                        if (xmlTagName[j].Attributes["Id"].Value == ulHumanID.ToString())
                //                        {
                //                            objFillHuman.Birth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value);
                //                            sBirth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value).ToString("dd-MMM-yyyy");
                //                            objFillHuman.Id = Convert.ToUInt32(xmlTagName[j].Attributes["Id"].Value);
                //                            objFillHuman.Last_Name = xmlTagName[j].Attributes["Last_Name"].Value;
                //                            objFillHuman.First_Name = xmlTagName[j].Attributes["First_Name"].Value;
                //                            objFillHuman.MI = xmlTagName[j].Attributes["MI"].Value;
                //                            objFillHuman.Suffix = xmlTagName[j].Attributes["Suffix"].Value;
                //                            objFillHuman.Sex = xmlTagName[j].Attributes["Sex"].Value;
                //                            objFillHuman.Work_Phone_No = xmlTagName[j].Attributes["Work_Phone_No"].Value;
                //                            objFillHuman.Work_Phone_Ext = xmlTagName[j].Attributes["Work_Phone_Ext"].Value;
                //                            objFillHuman.Home_Phone_No = xmlTagName[j].Attributes["Home_Phone_No"].Value;
                //                            objFillHuman.Cell_Phone_Number = xmlTagName[j].Attributes["Cell_Phone_Number"].Value;
                //                            if (xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != null && xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value != string.Empty)
                //                                objFillHuman.ACO_Is_Eligible_Patient = xmlTagName[j].Attributes.GetNamedItem("ACO_Is_Eligible_Patient").Value.ToString();
                //                            else
                //                                objFillHuman.ACO_Is_Eligible_Patient = "";
                //                            objFillHuman.Human_Type = xmlTagName[j].Attributes["Human_Type"].Value;

                //                            lstHuman.Add(objFillHuman);
                //                        }
                //                    }

                string phoneno = "";

                if (lstHuman != null && lstHuman.Count > 0)
                {

                    if (objFillHuman.Home_Phone_No.Length == 14)
                    {
                        phoneno = objFillHuman.Home_Phone_No;
                    }
                    else
                    {
                        phoneno = objFillHuman.Cell_Phone_Number;
                    }

                }

                string sPatientSex = string.Empty;


                if (objFillHuman.Sex != string.Empty)
                {
                    if (objFillHuman.Sex.Substring(0, 1).ToUpper() == "U")
                    {
                            //Cap - 596
                            //sPatientSex = "UNK";
                            sPatientSex = "UN";
                        }
                    else
                    {
                        sPatientSex = objFillHuman.Sex.Substring(0, 1);
                    }
                }
                else
                {
                    sPatientSex = "";
                }

                string sAcoEligiblePatient = string.Empty;
                sAcoEligiblePatient = objFillHuman.ACO_Is_Eligible_Patient;

                sPatientstrip = " " + objFillHuman.Last_Name + "," + objFillHuman.First_Name
                    + "  " + objFillHuman.MI + "  " + objFillHuman.Suffix + " | " +
objFillHuman.Birth_Date.ToString("dd-MMM-yyyy") + " | " +
(CalculateAge(objFillHuman.Birth_Date)).ToString() +
"  year(s) | " + sPatientSex + " | Acc #:" + ulHumanID.ToString() +
" | " + "Med Rec #:" + objFillHuman.Medical_Record_Number + " | " +
"Phone #:" + phoneno + " | Patient Type:" + objFillHuman.Human_Type + " | ";

                    if (objFillHuman.Is_Translator_Required != null && objFillHuman.Is_Translator_Required.ToUpper() == "Y")
                    {
                        sPatientstrip += objFillHuman.Preferred_Language + " req." + "   |   ";
                    }

                    if (sAcoEligiblePatient != null && sAcoEligiblePatient != string.Empty && sAcoEligiblePatient != "N")
                {
                    sPatientstrip += sAcoEligiblePatient + "   |   ";
                }
            }
            catch (Exception ex)
            {
                //XmlText.Close();
                //Thread.Sleep(5000);
                UtilityManager.GenerateXML(ulHumanID.ToString(), "Human");

                goto ln;
            }
            //                    }
            //                }
            //                fs.Close();
            //                fs.Dispose();
            //            }
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        XmlText.Close();
            //        //Thread.Sleep(5000);
            //       UtilityManager.GenerateXML(ulHumanID.ToString(), "Human");

            //        goto ln;
            //    }
        }
            IList<StaticLookup> lstStaticCallDuration = new List<StaticLookup>();
            StaticLookupManager objStaticLookup = new StaticLookupManager();
            lstStaticCallDuration = objStaticLookup.getStaticLookupByFieldName("PHONE ENCOUNTER CALL DURATION");
            if (ClientSession.UserRole != null && (ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT" && ClientSession.UserRole.ToUpper() != "PHYSICIAN"))
            {
                // var Enc = from p in lstStaticCallDuration where p.Default_Value.ToUpper() == "ALL" select p;
                var Enc = from p in lstStaticCallDuration where p.Default_Value.ToUpper() == "NONPROVIDER" select p;
                lstStaticCallDuration = new List<StaticLookup>();
                if (Enc != null && Enc.ToList<StaticLookup>().Count() > 0)
                {
                    lstStaticCallDuration = Enc.ToList<StaticLookup>();
                }
            }
            else
            {
                var Enc = from p in lstStaticCallDuration where p.Default_Value.ToUpper() == "PROVIDER" select p;
                lstStaticCallDuration = new List<StaticLookup>();
                if (Enc != null && Enc.ToList<StaticLookup>().Count() > 0)
                {
                    lstStaticCallDuration = Enc.ToList<StaticLookup>();
                }
            }

            IList<string> lstEncDOSPhyName = new List<string>();
            lstEncDOSPhyName = GetAllEncountersFromHumanXml(ulHumanID.ToString(), "90");

            //
            EncounterManager objEncounterManager = new EncounterManager();

            bool bSaveEnable = false;
            string bsaveenable = "false";
            //DateTime EncRcrdDOS = new DateTime();
            string btnDelete = "Resources/Delete-Blue.png";
            string btnDeleteAdditionalICD = "Resources/Delete-Blue.png";
            EAndMCodingManager EandMMngr = new EAndMCodingManager();
            string EnableScreen = string.Empty, EnablePriRbtn = string.Empty;
            IList<string> ProcedureList = new List<String>();

            IList<Encounter> objEncount = new List<Encounter>();


            string sCurrentProcess = ClientSession.UserCurrentProcess.ToUpper();



            EAndMCodingManager EandMCodingMngr = new EAndMCodingManager();
            FillEandMCoding eandmDTO = new FillEandMCoding();//EandMCodingMngr.LoadEandMCodingNew(Convert.ToUInt64(ClientSession.EncounterId), Convert.ToUInt64(ClientSession.HumanId), EncRcrdDOS, out bSaveEnable, Is_CMG_Ancillary);//added for CMG Ancilliary
            if (lstEncDOSPhyName.Count > 0)
                eandmDTO.ICDList = GetAssessmentDetails(lstEncDOSPhyName[0].Split('|')[0]);

            //BugID:51570
            #region Modifiers
            IList<string> ModifiersList = new List<string>();
            //XmlDocument xmldocModifier = new XmlDocument();
            //xmldocModifier.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "staticlookup" + ".xml");
            //XmlNodeList xmlNodeModifierList = xmldocModifier.GetElementsByTagName("StaticLookUp");
            //ModifiersList = xmlNodeModifierList.Cast<XmlNode>().Where(a => a.Attributes["Name"].Value.ToUpper() == "MODIFIER").OrderBy(a => Convert.ToInt16(a.Attributes["Sort_order"].Value.ToString())).Select(a => a.Attributes["value"].Value.ToString()).ToList<string>();

            StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
            if (staticLookupList != null)
            {
                ModifiersList = staticLookupList.StaticLookUpList.StaticLookUp.Where(x=>x.Name.ToUpper() == "MODIFIER").OrderBy(a => Convert.ToInt16(a.Sort_Order)).Select(a => a.value.ToString()).ToList<string>();
            }
            #endregion

            IList<string> EMDTOIcdlst = new List<string>();
            EMDTOIcdlst = eandmDTO.EandMCodingICDList.Select(a => a.ICD).Distinct().ToList<string>();//BugID:46020


            IList<string> ProList = eandmDTO.ProcList.OrderBy(s => s.ToString().Split('~')[0]).ToList();
            for (int i = 0; i < ProList.Count; i++)
            {
                ProcedureList.Add(ProList[i] + "~" + (i + 1));
            }

            if (EnableScreen == "disabled")
            {
                IList<string> EandMCPT = new List<string>();
                IList<string> EandMICD = new List<string>();
                IList<string> CPTtoremove = new List<string>();
                IList<string> ICDtoremove = new List<string>();
                foreach (EAndMCoding eandm in eandmDTO.EandMCodingList)
                {
                    EandMCPT.Add(eandm.Procedure_Code);
                }
                foreach (EandMCodingICD eandm in eandmDTO.EandMCodingICDList)
                {
                    EandMICD.Add(eandm.ICD);
                }
                foreach (string s in ProcedureList)
                {
                    if (EandMCPT.IndexOf(s.Split('~')[0]) == -1)
                    {
                        CPTtoremove.Add(s);
                    }
                }
                foreach (string s in eandmDTO.ICDList)
                {
                    if (EandMICD.IndexOf(s.Split('~')[1]) == -1)
                    {
                        ICDtoremove.Add(s);
                    }
                }
                foreach (string s in CPTtoremove)
                {
                    ProcedureList.Remove(s);
                }
                foreach (string s in ICDtoremove)
                {
                    eandmDTO.ICDList.Remove(s);
                }
            }
            if (eandmDTO.EandMCodingList != null && ProcedureList != null)
            {
                if (eandmDTO.EandMCodingList.Count != ProcedureList.Count)
                {
                    bsaveenable = "true";
                }
            }
            if (EMDTOIcdlst != null && eandmDTO.ICDList != null)
            {
                if (EMDTOIcdlst.Count != eandmDTO.ICDList.Count)
                {
                    bsaveenable = "true";
                }
            }
            if (bSaveEnable)
            {
                bsaveenable = "true";
            }
            var EandMCodingListCPT = ProcedureList.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1], EandMCPTID = a.Split('~')[2], Units = a.Split('~')[3], Modifier1 = a.Split('~')[4], Modifier2 = a.Split('~')[5], Modifier3 = a.Split('~')[6], Modifier4 = a.Split('~')[7], CPTCheck = a.Split('~')[8], EnableScreen = EnableScreen, CPTVersion = a.Split('~')[9], btnDelete = btnDelete, Order = a.Split('~')[10] });
            HttpContext.Current.Session["EnablePriRbtn"] = EnablePriRbtn;
            var EandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "EMICD").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], btnDelete = btnDeleteAdditionalICD, EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], IsPrimary = a.Split('~')[4], EnableScreen = EnableScreen, EnablePriRbtn = EnablePriRbtn });
            string modifierlst = new JavaScriptSerializer().Serialize(ModifiersList);

            if (eandmDTO.EandMCodingList != null)
                HttpContext.Current.Session["EandMList"] = eandmDTO.EandMCodingList;
            if (eandmDTO.EandMCodingICDList != null)
                HttpContext.Current.Session["EandMICDList"] = eandmDTO.EandMCodingICDList;



            //IList<string> AssPriCount = new List<string>();
            //AssPriCount = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT" && a.Split('~')[4] == "Pri").ToList<string>();
            //if (AssPriCount != null && AssPriCount.Count > 0)
            //{
            //    EnablePriRbtn = "disabled";
            //}
            var AssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
            //var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, IsCMGAncillary = Is_CMG_Ancillary, EncounterDetails = sResult };




            //var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = string.Empty, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, HumanDetails = lstHuman, FacilityName = ClientSession.FacilityName, ApptProvIDORLoginID = sApptproviderID, Birth_Date = sBirth_Date, FacilityDOS = sFacilityDOS, StaticCallDuration = lstStaticCallDuration };
            //CAP-713 - Get Person Name
            string PersonName = string.Empty;
            //CAP-2788
            UserList ilstUserList = ConfigureBase<UserList>.ReadJson("User.json");
            if (ilstUserList?.User != null)
            {
                var filteredData = ilstUserList?.User.FirstOrDefault(a => a.User_Name == ClientSession.UserName);
                if (filteredData != null && !string.IsNullOrEmpty(filteredData.person_name))
                {
                    PersonName = filteredData.person_name;
                }
            }
            var result = new { ProcedureList = EandMCodingListCPT, ICDList = AssEandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = string.Empty, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, HumanDetails = lstHuman, FacilityName = ClientSession.FacilityName, Birth_Date = sBirth_Date, StaticCallDuration = lstStaticCallDuration, EncDOSPhyIDList = lstEncDOSPhyName, PatientDetails = sPatientstrip, PersonName };
            return JsonConvert.SerializeObject(result);

        }

        #region commentload
        //EncounterManager objEncounterManager = new EncounterManager();
        //bool Is_CMG_Ancillary = false;
        //bool bSaveEnable = false;
        //string bsaveenable = "false";
        //DateTime EncRcrdDOS = new DateTime();
        //string btnDelete = "Resources/Delete-Blue.png";
        //string btnDeleteAdditionalICD = "Resources/Delete-Blue.png";
        //EAndMCodingManager EandMMngr = new EAndMCodingManager();
        //string EnableScreen = string.Empty;//, EnablePriRbtn = string.Empty;
        //IList<string> ProcedureList = new List<String>();

        //IList<Encounter> objEncount = new List<Encounter>();
        //objEncount = objEncounterManager.GetPhoneEncounter(ulHumanID);
        //objEncount = objEncounterManager.GetEncounterUsingHumanIDOrderByEncID(ulHumanID);


        //if (objEncount.Count > 0)
        //{
        //    if (objEncount[0].Call_Spoken_To != string.Empty)
        //    {
        //        sResult += objEncount[0].Call_Spoken_To + "|";
        //    }

        //    if (objEncount[0].Call_Duration != 0)
        //        sResult += Convert.ToString(objEncount[0].Call_Duration + "|");
        //    if (objEncount[0].Duration_Minutes != 0)
        //        sResult += Convert.ToString(objEncount[0].Duration_Minutes + "|");

        //    sResult += objEncount[0].Relationship + "|";


        //    if (objEncount[0].Date_of_Service.ToString() != "0001-01-01")
        //        sResult += UtilityManager.ConvertToLocal(objEncount[0].Date_of_Service).ToString("dd-MMM-yyyy hh:mm tt") + "|";

        //    if (ulHumanID != 0)
        //    {
        //        TreatmentPlanManager objTreatmentPlanManager = new TreatmentPlanManager();
        //        IList<TreatmentPlan> TreatmentPlanlst = objTreatmentPlanManager.GetTreatmentPlanUsingEncounterId(Convert.ToUInt32(objEncount[0].Id), Convert.ToUInt32(ulHumanID));
        //        if (TreatmentPlanlst != null && TreatmentPlanlst.Count > 0 && TreatmentPlanlst.Where(a => a.Plan_Type == "PLAN").ToList<TreatmentPlan>().Count > 0)
        //        {
        //            TreatmentPlan objTreatmentPlan = TreatmentPlanlst.Where(a => a.Plan_Type == "PLAN").ToList<TreatmentPlan>()[0];
        //            sResult += objTreatmentPlan.Plan;
        //        }
        //    }

        //}

        //string Ancilliary_FacilityName = string.Empty;
        //string sCurrentProcess = ClientSession.UserCurrentProcess.ToUpper();
        //if (System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"] != null)
        //    Ancilliary_FacilityName = System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"].ToString();


        //    if (Ancilliary_FacilityName.ToString().ToUpper() == objEncount[0].Facility_Name.ToString().ToUpper())
        //    {
        //        Is_CMG_Ancillary = true;
        //    }


        //EAndMCodingManager EandMCodingMngr = new EAndMCodingManager();
        //FillEandMCoding eandmDTO = EandMCodingMngr.LoadEandMCodingNew(Convert.ToUInt64(ClientSession.EncounterId), Convert.ToUInt64(ClientSession.HumanId), EncRcrdDOS, out bSaveEnable, Is_CMG_Ancillary);//added for CMG Ancilliary



        //BugID:51570
        //#region Modifiers
        //IList<string> ModifiersList = new List<string>();
        //XmlDocument xmldocModifier = new XmlDocument();
        //xmldocModifier.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "staticlookup" + ".xml");
        //XmlNodeList xmlNodeModifierList = xmldocModifier.GetElementsByTagName("StaticLookUp");
        //ModifiersList = xmlNodeModifierList.Cast<XmlNode>().Where(a => a.Attributes["Name"].Value.ToUpper() == "MODIFIER").OrderBy(a => Convert.ToInt16(a.Attributes["Sort_order"].Value.ToString())).Select(a => a.Attributes["value"].Value.ToString()).ToList<string>();
        //string modifierlst = new JavaScriptSerializer().Serialize(ModifiersList);
        //#endregion

        //IList<string> EMDTOIcdlst = new List<string>();
        //EMDTOIcdlst = eandmDTO.EandMCodingICDList.Select(a => a.ICD).Distinct().ToList<string>();//BugID:46020


        //IList<string> ProList = eandmDTO.ProcList.OrderBy(s => s.ToString().Split('~')[0]).ToList();
        //for (int i = 0; i < ProList.Count; i++)
        //{
        //    ProcedureList.Add(ProList[i] + "~" + (i + 1));
        //}

        //if (EnableScreen == "disabled")
        //{
        //    IList<string> EandMCPT = new List<string>();
        //    IList<string> EandMICD = new List<string>();
        //    IList<string> CPTtoremove = new List<string>();
        //    IList<string> ICDtoremove = new List<string>();
        //    foreach (EAndMCoding eandm in eandmDTO.EandMCodingList)
        //    {
        //        EandMCPT.Add(eandm.Procedure_Code);
        //    }
        //    foreach (EandMCodingICD eandm in eandmDTO.EandMCodingICDList)
        //    {
        //        EandMICD.Add(eandm.ICD);
        //    }
        //    foreach (string s in ProcedureList)
        //    {
        //        if (EandMCPT.IndexOf(s.Split('~')[0]) == -1)
        //        {
        //            CPTtoremove.Add(s);
        //        }
        //    }
        //    foreach (string s in eandmDTO.ICDList)
        //    {
        //        if (EandMICD.IndexOf(s.Split('~')[1]) == -1)
        //        {
        //            ICDtoremove.Add(s);
        //        }
        //    }
        //    foreach (string s in CPTtoremove)
        //    {
        //        ProcedureList.Remove(s);
        //    }
        //    foreach (string s in ICDtoremove)
        //    {
        //        eandmDTO.ICDList.Remove(s);
        //    }
        //}
        //if (eandmDTO.EandMCodingList != null && ProcedureList != null)
        //{
        //    if (eandmDTO.EandMCodingList.Count != ProcedureList.Count)
        //    {
        //        bsaveenable = "true";
        //    }
        //}
        //if (EMDTOIcdlst != null && eandmDTO.ICDList != null)
        //{
        //    if (EMDTOIcdlst.Count != eandmDTO.ICDList.Count)
        //    {
        //        bsaveenable = "true";
        //    }
        //}
        //if (bSaveEnable)
        //{
        //    bsaveenable = "true";
        //}
        //var EandMCodingListCPT = ProcedureList.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1], EandMCPTID = a.Split('~')[2], Units = a.Split('~')[3], Modifier1 = a.Split('~')[4], Modifier2 = a.Split('~')[5], Modifier3 = a.Split('~')[6], Modifier4 = a.Split('~')[7], CPTCheck = a.Split('~')[8], EnableScreen = EnableScreen, CPTVersion = a.Split('~')[9], btnDelete = btnDelete, Order = a.Split('~')[10] });

        //var EandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "EMICD").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], btnDelete = btnDeleteAdditionalICD, EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], IsPrimary = a.Split('~')[4], EnableScreen = EnableScreen, EnablePriRbtn = EnablePriRbtn });
        //string modifierlst = new JavaScriptSerializer().Serialize(ModifiersList);

        //if (eandmDTO.EandMCodingList != null)
        //    HttpContext.Current.Session["EandMList"] = eandmDTO.EandMCodingList;
        //if (eandmDTO.EandMCodingICDList != null)
        //    HttpContext.Current.Session["EandMICDList"] = eandmDTO.EandMCodingICDList;


        //if (!Is_CMG_Ancillary)
        //{
        //    IList<string> AssPriCount = new List<string>();
        //    AssPriCount = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT" && a.Split('~')[4] == "Pri").ToList<string>();
        //    if (AssPriCount != null && AssPriCount.Count > 0)
        //    {
        //        EnablePriRbtn = "disabled";
        //    }
        //    var AssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
        //    var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, IsCMGAncillary = Is_CMG_Ancillary, EncounterDetails = sResult, FacilityName = ClientSession.FacilityName, LoginUserName = ClientSession.UserName };

        //    HttpContext.Current.Session["SuggCPTList"] = ProcedureList;

        //    HttpContext.Current.Session["EnablePriRbtn"] = EnablePriRbtn;
        //    return JsonConvert.SerializeObject(result);
        //}
        //else
        //{
        //    var OrdersAssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ORDERS_ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
        //    var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = OrdersAssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, IsCMGAncillary = Is_CMG_Ancillary, EncounterDetails = sResult,FacilityName=ClientSession.FacilityName,LoginUserName=ClientSession.UserName };
        //    if (eandmDTO.EandMCodingList != null)
        //        HttpContext.Current.Session["EandMList"] = eandmDTO.EandMCodingList;
        //    if (eandmDTO.EandMCodingICDList != null)
        //        HttpContext.Current.Session["EandMICDList"] = eandmDTO.EandMCodingICDList;
        //    HttpContext.Current.Session["EnablePriRbtn"] = EnablePriRbtn;
        //    HttpContext.Current.Session["SuggCPTList"] = ProcedureList;
        //    return JsonConvert.SerializeObject(result);
        //}



        //}

        //else
        //{
        //    return JsonConvert.SerializeObject(sResult);
        //}

        #endregion

        [System.Web.Services.WebMethod(EnableSession = true)]
        public string SavePhoneEncounterPlanEandMCoding(object[] arylstCPT, object[] arylstICD, object[] arylstDelCPT, object[] arylstDelICD, ulong ulHumanID, string sCallMins, string sCallerName, string sCallSpokenTo, string sCallDate, string sNotes, string sSubmitMode, string sPhyID, string sDOSPhyName, string sPhoneEncounterOwner, string sPhoneEncounterSignedDate, string sIsSignedAkidoNote)
        {
            string sResult = Save(arylstCPT, arylstICD, arylstDelCPT, arylstDelICD, ulHumanID, sCallMins, sCallerName, sCallSpokenTo, sCallDate, sNotes, sSubmitMode, ClientSession.UserName, sPhyID, sDOSPhyName, sPhoneEncounterOwner, sPhoneEncounterSignedDate, sIsSignedAkidoNote);
            return sResult;
        }

        string Save(object[] arylstCPT, object[] arylstICD, object[] arylstDelCPT, object[] arylstDelICD, ulong ulHumanID, string sCallMins, string sCallerName, string sCallSpokenTo, string sCallDate, string sNotes, string sSubmitMode, string sPhyName, string sPhyID, string sDOSPhyName,string sPhoneEncounterOwner, string sPhoneEncounterSignedDate,string sIsSignedAkidoNote)
        {
            EAndMCodingManager objEandMManager = new EAndMCodingManager();
            UserManager userManger = new UserManager();

            IList<Encounter> EncounterLst = new List<Encounter>();
            IList<TreatmentPlan> PlanLst = new List<TreatmentPlan>();

            Encounter EncRecord = new Encounter();
            TreatmentPlan objTrtPlan = new TreatmentPlan();

            EncRecord.Created_By = ClientSession.UserName;
            EncRecord.Created_Date_and_Time = UtilityManager.ConvertToUniversal();
            EncRecord.Is_Phone_Encounter = "Y";
            if (ulHumanID != 0)
            {
                EncRecord.Human_ID = Convert.ToUInt32(ulHumanID);
            }
            EncRecord.Facility_Name = ClientSession.FacilityName;
            EncRecord.Encounter_Provider_ID = Convert.ToInt32(sPhyID);
            EncRecord.Appointment_Provider_ID = Convert.ToInt32(sPhyID);
            EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToUniversal();
            EncRecord.Referring_Address = sDOSPhyName;// Used in batch automation

            //if (sCallHrs != string.Empty)
            //    EncRecord.Call_Duration = Convert.ToInt32(sCallHrs);
            //if (sCallMins != string.Empty)
            //    EncRecord.Duration_Minutes = Convert.ToInt32(sCallMins);

            if (sCallMins != string.Empty)
                EncRecord.Call_Duration = sCallMins;

            EncRecord.Relationship = sCallSpokenTo;
            EncRecord.Call_Spoken_To = sCallerName;
            //code added by balaji
            if ((sCallDate != null) && (sCallDate != "0001-01-01"))
            {
                EncRecord.Date_of_Service = UtilityManager.ConvertToUniversal(Convert.ToDateTime(sCallDate));
                EncRecord.Appointment_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(sCallDate));//Convert.ToDateTime(sCallDate.Split('G').ElementAt(0).ToString());
            }
            EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
            EncRecord.Is_EandM_Submitted = "Y";
            EncRecord.E_M_Submitted_Date_And_Time = DateTime.Now;
            //Cap - 3582
            if (sIsSignedAkidoNote == "True")
            {
                EncRecord.Phone_Encounter_Owner = sPhoneEncounterOwner;
                EncRecord.Encounter_Provider_Signed_Date = UtilityManager.ConvertToUniversal(Convert.ToDateTime(sPhoneEncounterSignedDate));
                EncRecord.Is_Signed_in_Akido_Note = "Y";
            }

            //if (EncRecord.Is_EandM_Submitted == "Y")//By naveena For Submitting esuper bill second time
            //{
            //    if (EncRecord.Batch_Status.ToUpper() == "CLOSED")
            //    {
            //        EncRecord.Batch_Status = "REOPEN";
            //    }
            //    else
            //    {
            //        if (EncRecord.Batch_Status.ToUpper() != "REOPEN")//For BUg Id 52918 
            //            EncRecord.Batch_Status = "MODIFIED";
            //    }
            //}
            EncounterLst.Add(EncRecord);

            if (ulHumanID != 0)
            {
                objTrtPlan.Human_ID = Convert.ToUInt32(ulHumanID);
            }
            objTrtPlan.Created_By = ClientSession.UserName;
            objTrtPlan.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            objTrtPlan.Local_Time = UtilityManager.ConvertToLocal(objTrtPlan.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");
            objTrtPlan.Physician_Id = Convert.ToUInt64(sPhyID);
            objTrtPlan.Plan_Type = "PLAN";
            objTrtPlan.Plan = sNotes;
            PlanLst.Add(objTrtPlan);
            EncounterManager objEncounterManager = new EncounterManager();
            ulong uEncounterid = objEncounterManager.SaveUpdatePhoneEncounter(EncounterLst, PlanLst, string.Empty);
            //End Save Encounter and Plan

            IList<EAndMCoding> EAndMCPTTempList = new List<EAndMCoding>();
            IList<EAndMCoding> EAndMCPTSaveList = new List<EAndMCoding>();
            IList<EAndMCoding> EAndMCPTUpdateList = new List<EAndMCoding>();
            IList<EAndMCoding> EAndMCPTDeleteList = new List<EAndMCoding>();
            IList<EandMCodingICD> EAndMICDSaveList = new List<EandMCodingICD>();
            IList<EandMCodingICD> EAndMICDTempList = new List<EandMCodingICD>();
            IList<EandMCodingICD> EAndMICDUpdateList = new List<EandMCodingICD>();

            EAndMCPTTempList = (IList<EAndMCoding>)HttpContext.Current.Session["EandMList"];
            EAndMICDTempList = (IList<EandMCodingICD>)HttpContext.Current.Session["EandMICDList"];

            //bool Is_CMG_Ancillary = false;
            //string Ancilliary_FacilityName = string.Empty;
            //if (System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"] != null)
            //    Ancilliary_FacilityName = System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"].ToString();

            //if (Ancilliary_FacilityName.ToString().ToUpper() == ClientSession.FacilityName.ToString().ToUpper())
            //{
            //    Is_CMG_Ancillary = true;
            //}

            //if (EAndMCPTTempList != null && EAndMCPTTempList.Count > 0)
            //    EAndMCPTDeleteList = EAndMCPTTempList.Where(e => !arylstCPT.Any(b => b.ToString().Split('~')[8] == Convert.ToString(e.Id) && b.ToString().Split('~')[8] != string.Empty)).ToList<EAndMCoding>();
            //foreach (EAndMCoding cpt in EAndMCPTDeleteList)
            //{
            //    cpt.Is_Delete = "Y";
            //    EAndMCPTUpdateList.Add(cpt);
            //}

            //if (EAndMICDTempList != null && EAndMICDTempList.Count > 0)
            //{
            //    EAndMICDTempList = EAndMICDTempList.Where(e => arylstDelICD.Contains(e.ICD.Trim()) && e.Source.ToUpper() == "EMICD").ToList<EandMCodingICD>();
            //}
            //foreach (EandMCodingICD icd in EAndMICDTempList)
            //{
            //    icd.Is_Delete = "Y";
            //    EAndMICDUpdateList.Add(icd);
            //}
            EAndMCPTDeleteList = null;
            //comment by balaji.TJ
            //object[] arrTempCPT = arylstCPT.Where(a => a.ToString().Split('~')[8] == string.Empty).ToArray();//SaveList
            //Modiifed by balaji.TJ
            object[] arrTempCPT = arylstCPT.Where(a => a.ToString().Split('~')[13] == string.Empty).ToArray();//SaveList
            foreach (object objCPT in arylstCPT)
            {
                EAndMCoding objEandMCoding = null;
                IList<EAndMCoding> eandmCPTList = null;
                //comment by balaji.TJ
                //var eandmList = from eandm in EAndMCPTTempList where Convert.ToString(eandm.Id) == objCPT.ToString().Split('~')[8] select eandm;
                //Modiifed by balaji.TJ
                var eandmList = from eandm in EAndMCPTTempList where Convert.ToString(eandm.Id) == objCPT.ToString().Split('~')[13] select eandm;
                eandmCPTList = eandmList.ToList<EAndMCoding>();

                if (eandmCPTList.Count > 0)
                {
                    objEandMCoding = eandmCPTList[0];
                }
                else
                {
                    objEandMCoding = new EAndMCoding();
                }

                objEandMCoding.Encounter_ID = uEncounterid;
                objEandMCoding.Human_ID = ulHumanID;
                objEandMCoding.Physician_ID = Convert.ToUInt32(sPhyID);
                objEandMCoding.Procedure_Code = objCPT.ToString().Split('~')[0];
                objEandMCoding.Procedure_Code_Description = objCPT.ToString().Split('~')[1];
                if (objCPT.ToString().Split('~')[2] != "")
                    objEandMCoding.Units = Convert.ToInt32(objCPT.ToString().Split('~')[2]);
                objEandMCoding.Modifier1 = objCPT.ToString().Split('~')[3];
                objEandMCoding.Modifier2 = objCPT.ToString().Split('~')[4];
                objEandMCoding.Modifier3 = objCPT.ToString().Split('~')[5];
                objEandMCoding.Modifier4 = objCPT.ToString().Split('~')[6];
                //Modiifed by balaji.TJ
                objEandMCoding.Diagnosis_Pointer_1 = objCPT.ToString().Split('~')[7];
                objEandMCoding.Diagnosis_Pointer_2 = objCPT.ToString().Split('~')[8];
                objEandMCoding.Diagnosis_Pointer_3 = objCPT.ToString().Split('~')[9];
                objEandMCoding.Diagnosis_Pointer_4 = objCPT.ToString().Split('~')[10];
                objEandMCoding.Diagnosis_Pointer_5 = objCPT.ToString().Split('~')[11];
                objEandMCoding.Diagnosis_Pointer_6 = objCPT.ToString().Split('~')[12];


                ////if (objCPT.ToString().Split('~')[7] != "")
                ////    objEandMCoding.Sequence = Convert.ToInt32(objCPT.ToString().Split('~')[7]);
                objEandMCoding.Is_Delete = "N";
                ////objEandMCoding.CPT_Order = Convert.ToInt32(objCPT.ToString().Split('~')[10]);

                ////if (eandmCPTList.Count > 0)
                ////{
                ////    if (objCPT.ToString().Split('~')[9] != "")
                ////        objEandMCoding.Version = Convert.ToInt32(objCPT.ToString().Split('~')[9]);
                ////    objEandMCoding.Modified_By = ClientSession.UserName;
                ////    objEandMCoding.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                ////    EAndMCPTUpdateList.Add(objEandMCoding);
                ////}
                ////else
                ////{
                objEandMCoding.Created_By = ClientSession.UserName;
                objEandMCoding.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                EAndMCPTSaveList.Add(objEandMCoding);
                ////}
            }
            //Added for BugID:49118 - All deleted ICDs even if autosuggested/ entered and deleted will be saved in E_M_Coding table with Is_delete = 'Y'.
            foreach (object objCPT in arylstDelCPT)
            {
                //comment by balaji.TJ
                //if (objCPT.ToString().Split('~')[8].Trim() == string.Empty)
                //Modiifed by balaji.TJ
                if (objCPT.ToString().Split('~')[2].Trim() == string.Empty)
                {
                    EAndMCoding objEandMCoding = null;

                    objEandMCoding = new EAndMCoding();

                    objEandMCoding.Encounter_ID = uEncounterid;
                    objEandMCoding.Human_ID = ulHumanID;
                    objEandMCoding.Physician_ID = Convert.ToUInt32(sPhyID);
                    objEandMCoding.Procedure_Code = objCPT.ToString().Split('~')[0];
                    objEandMCoding.Procedure_Code_Description = objCPT.ToString().Split('~')[1];
                    if (objCPT.ToString().Split('~')[2] != "")
                        objEandMCoding.Units = Convert.ToInt32(objCPT.ToString().Split('~')[2]);
                    objEandMCoding.Modifier1 = objCPT.ToString().Split('~')[3];
                    objEandMCoding.Modifier2 = objCPT.ToString().Split('~')[4];
                    objEandMCoding.Modifier3 = objCPT.ToString().Split('~')[5];
                    objEandMCoding.Modifier4 = objCPT.ToString().Split('~')[6];
                    //Modiifed by balaji.TJ
                    objEandMCoding.Diagnosis_Pointer_1 = objCPT.ToString().Split('~')[7];
                    objEandMCoding.Diagnosis_Pointer_2 = objCPT.ToString().Split('~')[8];
                    objEandMCoding.Diagnosis_Pointer_3 = objCPT.ToString().Split('~')[9];
                    objEandMCoding.Diagnosis_Pointer_4 = objCPT.ToString().Split('~')[10];
                    objEandMCoding.Diagnosis_Pointer_5 = objCPT.ToString().Split('~')[11];
                    objEandMCoding.Diagnosis_Pointer_6 = objCPT.ToString().Split('~')[12];
                    //if (objCPT.ToString().Split('~')[7] != "")
                    //    objEandMCoding.Sequence = Convert.ToInt32(objCPT.ToString().Split('~')[7]);
                    objEandMCoding.Is_Delete = "Y";
                    //objEandMCoding.CPT_Order = Convert.ToInt32(objCPT.ToString().Split('~')[10]);

                    objEandMCoding.Created_By = ClientSession.UserName;
                    objEandMCoding.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    EAndMCPTSaveList.Add(objEandMCoding);
                }

            }

            //Added by balaji.TJ    
            //Add ICDS
            EandMCodingICD objEandMCodingICD = new EandMCodingICD();
            if (arylstICD.Length > 0)
            {
                foreach (var item in arylstICD)
                {
                    objEandMCodingICD = new EandMCodingICD();
                    objEandMCodingICD.Encounter_ID = uEncounterid;
                    objEandMCodingICD.Human_ID = ulHumanID;
                    objEandMCodingICD.ICD = item.ToString().Split('~')[0];
                    objEandMCodingICD.ICD_Description = item.ToString().Split('~')[1];
                    objEandMCodingICD.ICD_Category = item.ToString().Split('~')[2] == "Y" ? "Primary" : "None";
                    objEandMCodingICD.Source = item.ToString().Split('~')[5];
                    objEandMCodingICD.Created_By = ClientSession.UserName;
                    objEandMCodingICD.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objEandMCodingICD.ICD_Type = "";
                    objEandMCodingICD.Is_Delete = "N";
                    objEandMCodingICD.Sequence = item.ToString().Split('~')[6];
                    if (item.ToString().Split('~')[4] != "")
                        objEandMCodingICD.Version = Convert.ToInt32(item.ToString().Split('~')[4]);
                    EAndMICDSaveList.Add(objEandMCodingICD);
                }
            }
            //Update ICDS

            //foreach (var item in arylstDelICD)
            //{

            //}

            // objEandMManager.SaveUpdateEandMCodingForPhoneEncounter(EncounterLst, PlanLst, EAndMCPTSaveList, EAndMCPTUpdateList, arylstICD, (IList<EandMCodingICD>)HttpContext.Current.Session["EandMICDList"], ClientSession.UserName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList, string.Empty);
            //Comment by balaji.TJ
            //objEandMManager.SaveUpdateEandMCodingForPhoneEncounter(EAndMCPTSaveList, EAndMCPTUpdateList, (IList<EandMCodingICD>)HttpContext.Current.Session["EandMICDList"], sPhyName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList);//, string.Empty, null, null, null);
            //Modified by balaji
            objEandMManager.SaveUpdateEandMCodingForPhoneEncounter(EAndMCPTSaveList, EAndMCPTUpdateList, EAndMICDSaveList, sPhyName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList);//, string.Empty, null, null, null);
            // Jira cap - 499 - Need to Implement Phone Encounter Locking                                                                                                                                                                             //Jira CAP-340
            EncounterBlobManager objEncblobmngr = new EncounterBlobManager();
            objEncblobmngr.LockEncounter(uEncounterid, ulHumanID, ClientSession.UserName, UtilityManager.ConvertToUniversal());
            return "Success";

        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public IList<string> GetAllEncountersFromHumanXml(string ulHumanID, string sNoOfDays)
        {
            IList<Encounter> lstencounterFinal = new List<Encounter>();

            IList<String> ilstDOCTaglist = new List<String>();
            ilstDOCTaglist.Add("EncounterList");
            IList<Encounter> lstEncounter = new List<Encounter>();
            IList<object> ilstDOCFinal = new List<object>();
            ilstDOCFinal = UtilityManager.ReadBlob(Convert.ToUInt64(ulHumanID), ilstDOCTaglist, "Blob_Human");

            if (ilstDOCFinal != null && ilstDOCFinal.Count > 0)
            {
                if (ilstDOCFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstDOCFinal[0]).Count; iCount++)
                    {
                        lstEncounter.Add((Encounter)((IList<object>)ilstDOCFinal[0])[iCount]);
                    }
                }
            }


            //string sDOCFacility = string.Empty;
            //string FileName = "Human" + "_" + ulHumanID + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //IList<Encounter> lstEncounter = new List<Encounter>();
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);
            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("EncounterList") != null && itemDoc.GetElementsByTagName("EncounterList").Count > 0)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;
            //            if (xmlTagName != null && xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Encounter));
            //                    Encounter Encounter = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Encounter;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((Encounter)Encounter).GetType().GetProperties() select obji;
            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            if (propInfo != null)
            //                            {
            //                                foreach (PropertyInfo property in propInfo)
            //                                {
            //                                    if (property.Name == nodevalue.Name)
            //                                    {
            //                                        try
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(Encounter, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(Encounter, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(Encounter, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(Encounter, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(Encounter, nodevalue.Value, null);
            //                                        }
            //                                        catch (Exception ex)
            //                                        { }

            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    lstEncounter.Add(Encounter);
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }
            //}

            IList<string> FinalLst = new List<string>();
            if (lstEncounter != null && lstEncounter.Count > 0)
            {
                if (sNoOfDays != "")
                {
                    DateTime dtDOS = DateTime.Now.AddDays(-90);
                    // var Enc = from p in lstEncounter where p.Date_of_Service != Convert.ToDateTime("0001-01-01 00:00:00") && p.Is_Phone_Encounter != "Y" && p.Date_of_Service >= Convert.ToDateTime(dtDOS) orderby p.Date_of_Service descending select p;

                    var Enc = from p in lstEncounter where p.Date_of_Service != Convert.ToDateTime("0001-01-01 00:00:00") && p.Is_Phone_Encounter != "Y" && p.Facility_Name == ClientSession.FacilityName orderby p.Date_of_Service descending select p;
                    if (Enc != null && Enc.ToList<Encounter>().Count() > 0)
                    {

                        lstencounterFinal = Enc.ToList<Encounter>();
                        FinalLst = GetEncDetailsandPhysicianName(lstencounterFinal);
                    }
                }
                else
                {
                    var Enc = from p in lstEncounter where p.Is_Phone_Encounter != "Y" && p.Date_of_Service != Convert.ToDateTime("0001-01-01 00:00:00") orderby p.Date_of_Service descending select p;
                    if (Enc != null && Enc.ToList<Encounter>().Count() > 0)
                    {

                        lstencounterFinal = Enc.ToList<Encounter>();
                        FinalLst = GetEncDetailsandPhysicianName(lstencounterFinal);

                    }
                }
            }
            // return sDOCFacility;
            //var result = new { FinalList = FinalLst, AssessmentList = lstAssessmentFinal };
            //return JsonConvert.SerializeObject(result);
            return FinalLst;
        }

        IList<string> GetEncDetailsandPhysicianName(IList<Encounter> lstEncounter)
        {

            IList<string> lstDetails = new List<string>();
            string sPhysicianName = string.Empty;


            for (int j = 0; j < lstEncounter.Count; j++)
            {
                string sEncID = lstEncounter[j].Id.ToString();
                string sDOS = UtilityManager.ConvertToLocal(lstEncounter[j].Date_of_Service).ToString("dd-MMM-yyyy hh:mm tt");
                string sApptPrividerID = lstEncounter[j].Encounter_Provider_ID.ToString();
                if (lstEncounter[j].Encounter_Provider_ID != 0)
                {
                    //XmlDocument xmldoc1 = new XmlDocument();
                    //string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                    //if (File.Exists(strXmlFilePath1) == true)
                    //{
                    //    xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                    //    XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + lstEncounter[j].Encounter_Provider_ID);
                    //    if (nodeMatchingPhysicianAddress != null)
                    //    {
                    //        sPhysicianName = nodeMatchingPhysicianAddress.Attributes["Physician_prefix"].Value.ToString() + " " +
                    //        nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString() + " " +
                    //        nodeMatchingPhysicianAddress.Attributes["Physician_Middle_Name"].Value.ToString() + " " +
                    //        nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString() + " " +
                    //        nodeMatchingPhysicianAddress.Attributes["Physician_Suffix"].Value.ToString();
                    //    }
                    //}
                    //CAP-2780
                    PhysicianAddressDetailsList physicianAddressDetailsList = ConfigureBase<PhysicianAddressDetailsList>.ReadJson("PhysicianAddressDetails.json");
                    if (physicianAddressDetailsList != null)
                    {
                        var matchingAddress = physicianAddressDetailsList.PhysicianAddress
                                                         .FirstOrDefault(address => address.Physician_Library_ID == lstEncounter[j].Encounter_Provider_ID.ToString());

                        if (matchingAddress != null)
                        {
                            sPhysicianName = $"{matchingAddress.Physician_prefix} {matchingAddress.Physician_First_Name} " +
                                             $"{matchingAddress.Physician_Middle_Name} {matchingAddress.Physician_Last_Name} " +
                                             $"{matchingAddress.Physician_Suffix}";
                        }
                    }
                }
                //Jira cap - 2967
                string sFinal = string.Empty;
                if (sPhysicianName != "")
                {
                    sFinal = sEncID + "|" + sDOS + " - " + sPhysicianName + "|" + sApptPrividerID;
                }                 
                lstDetails.Add(sFinal);
            }
            return lstDetails;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchCPTDescrptionText(string text, string sCallDate)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            ProcedureCodeLibraryManager objProcedureCodeLibraryMngr = new ProcedureCodeLibraryManager();
            string data = string.Empty;
            string type = string.Empty;
            string CPTType = string.Empty;
            if (text != string.Empty)
            {
                data = text.Split('|')[0];
                type = text.Split('|')[1];
            }
            string sTodate = Convert.ToDateTime(sCallDate).ToString("yyyy-MM-dd");//ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
            string[] ResultDescpList = objProcedureCodeLibraryMngr.GetCPTDescriptionList(data, type, sTodate).ToArray();
            //var jsonString = JsonConvert.SerializeObject(ResultDescpList);
            //return jsonString;



            var ResultRecord1 = ResultDescpList.Select(a => new { label = a.Split('~')[0] + "~" + a.Split('~')[1], value = a.Split('~')[2] });
            // json = new JavaScriptSerializer().Serialize(ResultRecord1);
            var jsonString = JsonConvert.SerializeObject(ResultRecord1);
            return jsonString;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchICDDescrptionText(string text, string sCallDate)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string ICDType = string.Empty;
            //if (ClientSession.FillEncounterandWFObject != null)
            //{
            //    if (ClientSession.FillEncounterandWFObject.EncRecord != null)
            //    {
            //        Encounter EncRcrd = ClientSession.FillEncounterandWFObject.EncRecord;
            //        if (EncRcrd.Date_of_Service >= DateTime.Parse("01-Oct-2015"))
            //        {
            //            ICDType = "ICD_10";
            //        }
            //        else
            //        {
            //            ICDType = "ICD_9";
            //        }
            //    }
            //}
            if (sCallDate != "")
            {
                sCallDate = Convert.ToDateTime(sCallDate).ToString("dd-MMM-yyyy");
                if (Convert.ToDateTime(sCallDate) >= DateTime.Parse("01-Oct-2015"))
                {
                    ICDType = "ICD_10";
                }
                else
                {
                    ICDType = "ICD_9";
                }

            }

            AllICD_9Manager objAllICD_9Manager = new AllICD_9Manager();
            string data = string.Empty;
            string type = string.Empty;
            if (text != string.Empty)
            {
                data = text.Split('|')[0];
                type = text.Split('|')[1];
            }
            string sTodate = Convert.ToDateTime(sCallDate).ToString("yyyy-MM-dd");
            string[] ResultDescpList = objAllICD_9Manager.GetDescriptionList(data, type, ICDType, sTodate).ToArray();
            var jsonString = JsonConvert.SerializeObject(ResultDescpList);
            return jsonString;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public IList<string> GetAssessmentDetails(string uEncounterID)
        {
            IList<string> lstAssessmentFinal = new List<string>();
            if (uEncounterID != "" && uEncounterID != "0")
            {

                IList<Assessment> lstAssessment = new List<Assessment>();

                IList<string> ilstPhoneAssessnebtTagList = new List<string>();
                ilstPhoneAssessnebtTagList.Add("AssessmentList");
                IList<object> ilstAssessnebtFinal = new List<object>();
                ilstAssessnebtFinal = UtilityManager.ReadBlob(Convert.ToUInt64(uEncounterID), ilstPhoneAssessnebtTagList);

                if (ilstAssessnebtFinal != null && ilstAssessnebtFinal.Count > 0)
                {
                    if (ilstAssessnebtFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstAssessnebtFinal[0]).Count; iCount++)
                        {
                            lstAssessment.Add((Assessment)((IList<object>)ilstAssessnebtFinal[0])[iCount]);
                        }
                    }
                }


                //string FileEncName = "Encounter" + "_" + uEncounterID + ".xml";
                //string strXmlEncFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileEncName);
                //IList<Assessment> lstAssessment = new List<Assessment>();
                //if (File.Exists(strXmlEncFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlEncFilePath);
                //    XmlNodeList xmlTagName = null;
                //    using (FileStream fs = new FileStream(strXmlEncFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);
                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("AssessmentList") != null && itemDoc.GetElementsByTagName("AssessmentList").Count > 0)
                //        {
                //            xmlTagName = itemDoc.GetElementsByTagName("AssessmentList")[0].ChildNodes;
                //            if (xmlTagName != null && xmlTagName.Count > 0)
                //            {
                //                for (int j = 0; j < xmlTagName.Count; j++)
                //                {
                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Assessment));
                //                    Assessment Aassessment = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Assessment;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    propInfo = from obji in ((Assessment)Aassessment).GetType().GetProperties() select obji;
                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            if (propInfo != null)
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name == nodevalue.Name)
                //                                    {
                //                                        try
                //                                        {
                //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                                property.SetValue(Aassessment, Convert.ToUInt64(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                property.SetValue(Aassessment, Convert.ToString(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                property.SetValue(Aassessment, Convert.ToDateTime(nodevalue.Value), null);
                //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                property.SetValue(Aassessment, Convert.ToInt32(nodevalue.Value), null);
                //                                            else
                //                                                property.SetValue(Aassessment, nodevalue.Value, null);
                //                                        }
                //                                        catch (Exception ex)
                //                                        { }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    lstAssessment.Add(Aassessment);
                //                }
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
                if (lstAssessment != null && lstAssessment.Count > 0)
                {
                    if (lstAssessment != null && lstAssessment.Count > 0)
                    {
                        foreach (Assessment assICD in lstAssessment)
                        {
                            string sAssesmentPrimary = string.Empty;
                            if (assICD.Primary_Diagnosis.ToUpper() == "Y")
                            {
                                sAssesmentPrimary = "Pri";
                                lstAssessmentFinal.Add("ASSESSMENT" + "~" + assICD.ICD + "~" + assICD.ICD_Description + "~" + 0 + "~" + sAssesmentPrimary + "~" + "0" + "~" + "6" + "~" + "");
                            }
                        }
                    }
                }
            }
            return lstAssessmentFinal;
        }

        public int CalculateAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int years = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;
            return years;
        }



        [WebMethod(EnableSession = true)]
        public string GetFavouriteICDS(string uPhysicianID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string json = string.Empty;
            PhysicianICD_9Manager phyICd9Mngr = new PhysicianICD_9Manager();
            ulong uPhyID = 0;
            if (ClientSession.UserRole != null && (ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT" && ClientSession.UserRole.ToUpper() != "PHYSICIAN"))
            {
                uPhyID = Convert.ToUInt32(uPhysicianID);
            }
            else
            {
                uPhyID = ClientSession.PhysicianId;
            }
            IList<PhysicianICD_9> phyICD9List = phyICd9Mngr.GetPhyLeafICDsandCategory(uPhyID, ClientSession.LegalOrg);
            if (phyICD9List != null && phyICD9List.Count > 0)
            {
                var lstphyICDsResult = phyICD9List.Select(a => new
                {
                    ICD10Code = a.ICD_9,
                    ICD10Desc = a.ICD_9_Description,
                    Category = a.ICD_Category

                });
                json = new JavaScriptSerializer().Serialize(lstphyICDsResult);
            }
            return json;
        }


    }
}
