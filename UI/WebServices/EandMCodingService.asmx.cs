
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Diagnostics;
using System.Xml.Serialization;


namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for EandMCodingService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class EandMCodingService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchCPTDescrptionText(string text)
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
            string sTodate = string.Empty;
            if (ClientSession.FillEncounterandWFObject.EncRecord == null || ClientSession.FillEncounterandWFObject.EncRecord.Id == 0)
            {
                Encounter EncRecord = null;
                EncounterManager objencManager = new EncounterManager();
                IList<Encounter> ilstEnc = objencManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                if (ilstEnc.Count > 0)
                {
                    EncRecord = ilstEnc[0];
                    sTodate = EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
                }
            }
            else if (ClientSession.FillEncounterandWFObject.EncRecord != null)
            {
                sTodate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
            }
            string[] ResultDescpList = objProcedureCodeLibraryMngr.GetCPTDescriptionList(data, type, sTodate).ToArray();
            //Cap - 1301
            //var ResultRecord1 = ResultDescpList.Select(a => new { label = a.Split('~')[0] + "~" + a.Split('~')[1], value = a.Split('~')[2] + "~" + a.Split('~')[3] });
            var ResultRecord1 = ResultDescpList.Select(a => new { label = a.Split('~')[0] + "~" + a.Split('~')[1], value = a.Split('~')[2] + "~" + a.Split('~')[3] + "~" + a.Split('~')[4] });
            // json = new JavaScriptSerializer().Serialize(ResultRecord1);
            var jsonString = JsonConvert.SerializeObject(ResultRecord1);
            return jsonString;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchICDDescrptionText(string text)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string ICDType = string.Empty;
            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    Encounter EncRcrd = ClientSession.FillEncounterandWFObject.EncRecord;
                    if (EncRcrd.Date_of_Service >= DateTime.Parse("01-Oct-2015"))
                    {
                        ICDType = "ICD_10";
                    }
                    else
                    {
                        ICDType = "ICD_9";
                    }
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
            string sTodate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
            string[] ResultDescpList = objAllICD_9Manager.GetDescriptionList(data, type, ICDType, sTodate).ToArray();
            var jsonString = JsonConvert.SerializeObject(ResultDescpList);
            return jsonString;
        }

        [WebMethod(EnableSession = true)]
        public string LoadEandMCodingCPTTable(string strEandMCodingCPT, string sEnableScreen)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sBillingInstruction = string.Empty;
            string sBatchStatus = string.Empty;
            string sFacName = string.Empty;
            bool bSaveEnable = false;
            string sCloseVisible = "false";
            DateTime EncRcrdDOS = new DateTime();
            EAndMCodingManager EandMCodingMngr = new EAndMCodingManager();
            Encounter EncRcrd = new Encounter();
            EncounterManager objEncMngr = new EncounterManager();

            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord != null && ClientSession.FillEncounterandWFObject.EncRecord.Id != 0)
                {
                    EncRcrd = ClientSession.FillEncounterandWFObject.EncRecord;
                    EncRcrdDOS = EncRcrd.Date_of_Service;
                    sBatchStatus = EncRcrd.Batch_Status;
                    sBillingInstruction = EncRcrd.Billing_Instruction;
                    sFacName = EncRcrd.Facility_Name;
                    sCloseVisible = "false";
                }
                else
                {
                    IList<Encounter> ilstEnc = objEncMngr.GetEncounterByEncounterID(ClientSession.EncounterId);
                    if (ilstEnc.Count > 0)
                    {
                        EncRcrd = ilstEnc[0];
                        EncRcrdDOS = EncRcrd.Date_of_Service;
                        sBatchStatus = EncRcrd.Batch_Status;
                        sBillingInstruction = EncRcrd.Billing_Instruction;
                        sFacName = EncRcrd.Facility_Name;
                        sCloseVisible = "true";
                    }
                    //Cap - 1355, 1181, 1356
                    else
                    {
                        ilstEnc = objEncMngr.GetEncounterByEncounterIDIncludeArchive(ClientSession.EncounterId);
                        if (ilstEnc.Count > 0)
                        {
                            EncRcrd = ilstEnc[0];
                            EncRcrdDOS = EncRcrd.Date_of_Service;
                            sBatchStatus = EncRcrd.Batch_Status;
                            sBillingInstruction = EncRcrd.Billing_Instruction;
                            sFacName = EncRcrd.Facility_Name;
                            sCloseVisible = "true";
                        }
                    }


                }
            }
            bool Is_CMG_Ancillary = false;
            //string Ancilliary_FacilityName = string.Empty;
            //if (System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"] != null)
            //    Ancilliary_FacilityName = System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"].ToString();
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == sFacName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
            // if (Ancilliary_FacilityName.ToString().ToUpper() == sFacName.ToString().ToUpper())
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                Is_CMG_Ancillary = true;
            }
            
            FillEandMCoding eandmDTO = EandMCodingMngr.LoadEandMCodingNew(Convert.ToUInt64(ClientSession.EncounterId), Convert.ToUInt64(ClientSession.HumanId), EncRcrdDOS, out bSaveEnable, Is_CMG_Ancillary);//added for CMG Ancilliary
            sBatchStatus = eandmDTO.EncounterList[0].Batch_Status;
            string EnableScreen = string.Empty, EnablePriRbtn = string.Empty;
            string btnDelete = "Resources/Delete-Blue.png";
            string btnDeleteAdditionalICD = "Resources/Delete-Blue.png";
            string sCurrentProcess = ClientSession.UserCurrentProcess.ToUpper();
            IList<string> EMDTOIcdlst = new List<string>();
            EMDTOIcdlst = eandmDTO.EandMCodingICDList.Select(a => a.ICD).Distinct().ToList<string>();//BugID:46020
            string bsaveenable = "false";

            //Jira #CAP-707
            //if ((sCurrentProcess != "ADDENDUM_CODING" && sCurrentProcess != "ADDENDUM_CODING_2" && sCurrentProcess != "ADDENDUM_CORRECTION" && sCurrentProcess != "ADDENDUM_PROCESS" && sCurrentProcess != "DICTATION_REVIEW" && sCurrentProcess != "MA_PROCESS" && sCurrentProcess != "MA_REVIEW" && sCurrentProcess != "CODER_REVIEW_CORRECTION" && sCurrentProcess != "PROVIDER_PROCESS" &&
            //   sCurrentProcess != "READING_PROVIDER_PROCESS" && sCurrentProcess != "SCRIBE_REVIEW_CORRECTION" && sCurrentProcess != "SCRIBE_PROCESS" && sCurrentProcess != "SCRIBE_CORRECTION" && sCurrentProcess != "REVIEW_CODING" && sCurrentProcess != "REVIEW_CODING_2" && sCurrentProcess != "TECHNICIAN_PROCESS" && sCurrentProcess != "") || sBatchStatus == "CLOSED" || eandmDTO.BillingWFObjCurrentProcess == "BATCHING_COMPLETE")
            //{
            if ((sCurrentProcess != "ADDENDUM_CODING" && sCurrentProcess != "ADDENDUM_CODING_2" && sCurrentProcess != "ADDENDUM_CORRECTION" && sCurrentProcess != "ADDENDUM_PROCESS" && sCurrentProcess != "DICTATION_REVIEW" && sCurrentProcess != "MA_PROCESS" && sCurrentProcess != "MA_REVIEW" && sCurrentProcess != "CODER_REVIEW_CORRECTION" && sCurrentProcess != "PROVIDER_PROCESS" &&
              sCurrentProcess != "READING_PROVIDER_PROCESS" && sCurrentProcess != "SCRIBE_REVIEW_CORRECTION" && sCurrentProcess != "SCRIBE_PROCESS" && sCurrentProcess != "SCRIBE_CORRECTION" && sCurrentProcess != "REVIEW_CODING" && sCurrentProcess != "REVIEW_CODING_2" && sCurrentProcess != "TECHNICIAN_PROCESS" && sCurrentProcess != "" && sCurrentProcess != "AKIDO_SCRIBE_PROCESS" && sCurrentProcess != "AKIDO_REVIEW_CODING") || sBatchStatus == "CLOSED" || eandmDTO.BillingWFObjCurrentProcess == "BATCHING_COMPLETE")
            {
                if (ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT")//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                {
                    EnableScreen = "disabled";
                    btnDelete = "Resources/Delete-Grey.png";
                    btnDeleteAdditionalICD = "Resources/Delete-Grey.png";
                }
            }

            if (sEnableScreen == "EnableScreen=False")
            {
                EnableScreen = "disabled";
                btnDelete = "Resources/Delete-Grey.png";
                btnDeleteAdditionalICD = "Resources/Delete-Grey.png";

            }


            //BugID:51570
            #region Modifiers
            IList<string> ModifiersList = new List<string>();
            XmlDocument xmldocModifier = new XmlDocument();
            xmldocModifier.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "staticlookup" + ".xml");
            XmlNodeList xmlNodeModifierList = xmldocModifier.GetElementsByTagName("StaticLookUp");
            ModifiersList = xmlNodeModifierList.Cast<XmlNode>().Where(a => a.Attributes["Name"].Value.ToUpper() == "MODIFIER").OrderBy(a => Convert.ToInt16(a.Attributes["Sort_order"].Value.ToString())).Select(a => a.Attributes["value"].Value.ToString()).ToList<string>();
            #endregion

            IList<string> ProcedureList = new List<String>();
            ProcedureCodeLibraryManager obj = new ProcedureCodeLibraryManager();
            IList<ProcedureCodeLibrary> lst = new List<ProcedureCodeLibrary>();

            IList<string> ProList = eandmDTO.ProcList.OrderBy(s => s.ToString().Split('~')[0]).ToList();

            List<string> procedurecode = new List<string>();
            procedurecode = eandmDTO.ProcList.Select(a => a.ToString().Split('~')[0]).ToList<string>();
            lst = obj.GetProcedureList(procedurecode);
            for (int j = 0; j < lst.Count; j++)
            {
                for (int i = 0; i < ProList.Count; i++)
                {
                    if (ProList[i].Contains(lst[j].Procedure_Code.ToString()))
                        ProcedureList.Add(ProList[i] + "~" + lst[j].Procedure_Charge);
                    // ProcedureList.Add(ProList[i] + "~" + (j + 1).ToString() + "~" + lst[j].Procedure_Charge);
                }
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
            //BugID:46020 - to not enable Save and Save&Submit by default - START -
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
            //BugID:48192
            if (bSaveEnable)
            {
                bsaveenable = "true";
            }
            //BugID:46020 - to not enable Save and Save&Submit by default - END -
            //Cap - 1301
            //var EandMCodingListCPT = ProcedureList.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1], EandMCPTID = a.Split('~')[2], Units = a.Split('~')[3], Modifier1 = a.Split('~')[4], Modifier2 = a.Split('~')[5], Modifier3 = a.Split('~')[6], Modifier4 = a.Split('~')[7], CPTCheck = a.Split('~')[8], EnableScreen = EnableScreen, CPTVersion = a.Split('~')[9], btnDelete = btnDelete, Order = Convert.ToInt32(a.Split('~')[16]), Amount = (Convert.ToInt32(a.Split('~')[3]) * Convert.ToDouble(a.Split('~')[17])).ToString(), DiaPointer1 = a.Split('~')[10], DiaPointer2 = a.Split('~')[11], DiaPointer3 = a.Split('~')[12], DiaPointer4 = a.Split('~')[13], DiaPointer5 = a.Split('~')[14], DiaPointer6 = a.Split('~')[15] });
            var EandMCodingListCPT = ProcedureList.Select(a => new
            {
                CPTCode = a.Split('~')[0],
                CPTDesc = a.Split('~')[1],
                EandMCPTID = a.Split('~')[2],
                Units = a.Split('~')[3],
                Modifier1 = a.Split('~')[4],
                Modifier2 = a.Split('~')[5],
                Modifier3 = a.Split('~')[6],
                Modifier4 = a.Split('~')[7],
                CPTCheck = a.Split('~')[8],
                EnableScreen = EnableScreen,
                CPTVersion = a.Split('~')[9],
                btnDelete = btnDelete,
                Order = Convert.ToInt32(a.Split('~')[16]),
                RVU = Convert.ToDouble(a.Split('~')[17]),
                Amount = (Convert.ToInt32(a.Split('~')[3]) * Convert.ToDouble(a.Split('~')[18])).ToString(),
                DiaPointer1 = a.Split('~')[10],
                DiaPointer2 = a.Split('~')[11],
                DiaPointer3 = a.Split('~')[12],
                DiaPointer4 = a.Split('~')[13],
                DiaPointer5 = a.Split('~')[14],
                DiaPointer6 = a.Split('~')[15]
            })
                .OrderBy(c => c.Order).ThenByDescending(n => n.RVU).ThenBy(o => o.CPTCode);
            //BugID:48668 -- ServProc REVAMP

            var EandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "EMICD").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], btnDelete = btnDeleteAdditionalICD, EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], IsPrimary = a.Split('~')[4], EnableScreen = EnableScreen, EnablePriRbtn = EnablePriRbtn });
            string modifierlst = new JavaScriptSerializer().Serialize(ModifiersList);
            if (eandmDTO.EandMCodingList != null)
                HttpContext.Current.Session["EandMList"] = eandmDTO.EandMCodingList;
            if (eandmDTO.EandMCodingICDList != null)
                HttpContext.Current.Session["EandMICDList"] = eandmDTO.EandMCodingICDList;

            //BugID:52857 -- to bring in DiagOrder ICDs & CPTs only for CMG_Ancillary facility (no Assessment ICDs)
            if (!Is_CMG_Ancillary)
            {
                IList<string> AssPriCount = new List<string>();
                AssPriCount = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT" && a.Split('~')[4] == "Pri").ToList<string>();
                if (ClientSession.UserRole.ToUpper() != "CODER" && ClientSession.UserRole.ToUpper() != "MEDICAL ASSISTANT" && ClientSession.UserRole.ToUpper() != "OFFICE MANAGER")//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                {
                    EnablePriRbtn = "disabled";
                }
                if (AssPriCount != null && AssPriCount.Count > 0)
                {
                    EnablePriRbtn = "disabled";
                }
                //var AssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen, SNo = a.Split('~')[8] });
                var AssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
                var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, BillingInstruction = sBillingInstruction, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, IsCMGAncillary = Is_CMG_Ancillary, CloseVisible = sCloseVisible };
                HttpContext.Current.Session["SuggCPTList"] = ProcedureList;

                HttpContext.Current.Session["EnablePriRbtn"] = EnablePriRbtn;
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                //var OrdersAssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ORDERS_ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen, SNo = a.Split('~')[8] });
                var OrdersAssEandMCodingListICD = eandmDTO.ICDList.Where(a => a.Split('~')[0] == "ORDERS_ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
                var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = OrdersAssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, BillingInstruction = sBillingInstruction, SaveEnable = bsaveenable, EnablePriRbtn = EnablePriRbtn, ModifierList = modifierlst, IsCMGAncillary = Is_CMG_Ancillary, CloseVisible = sCloseVisible };
                if (eandmDTO.EandMCodingList != null)
                    HttpContext.Current.Session["EandMList"] = eandmDTO.EandMCodingList;
                if (eandmDTO.EandMCodingICDList != null)
                    HttpContext.Current.Session["EandMICDList"] = eandmDTO.EandMCodingICDList;
                HttpContext.Current.Session["EnablePriRbtn"] = EnablePriRbtn;
                HttpContext.Current.Session["SuggCPTList"] = ProcedureList;
                return JsonConvert.SerializeObject(result);
            }



            /* Removed as per New Req. 
           string EnableScreenAdditionalICD = "disabled";
           */


        }


        //[WebMethod(EnableSession = true)]
        //public string GetModifierforCPT(string CPT, string CPTList)
        //{
        //    if (ClientSession.UserName == string.Empty)
        //    {
        //        HttpContext.Current.Response.StatusCode = 999;
        //        HttpContext.Current.Response.Status = "999 Session Expired";
        //        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //        return "Session Expired";
        //    }
        //    string modifier = "";
        //    //  IList<string> lst =(IList<string>)HttpContext.Current.Session["SuggCPTList"];
        //    string[] cptlist = new string[] { };

        //    cptlist = CPTList.Split('|');//lst.Select(a => a.Split('~')[0]).ToArray();
        //    VaccineAdminProcedureMappingManager obj = new VaccineAdminProcedureMappingManager();
        //    IList<VaccineAdminProcedureMapping> lstmapp = new List<VaccineAdminProcedureMapping>();
        //    if (cptlist.Length > 0)
        //    {
        //        lstmapp = obj.GetModifierbyCPTandVaccine(CPT, cptlist);
        //    }
        //    //   var temp = from m in lst select m.CPTCode;
        //    if (lstmapp.Count > 0)
        //        modifier = lstmapp[0].Modifier.ToString();

        //    return modifier;
        //}


        [WebMethod(EnableSession = true)]
        public string RefershGrid()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            return "";
        }
        //[WebMethod(EnableSession = true)]
        //public string (string CPTList)
        //{SetModifierforCPT
        //    //if (ClientSession.UserName == string.Empty)
        //    //{
        //    //    HttpContext.Current.Response.StatusCode = 999;
        //    //    HttpContext.Current.Response.Status = "999 Session Expired";
        //    //    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //    //    return "Session Expired";
        //    //}
        //    //if(CPT!="")
        //    //{

        //    //}
        //    string[] cptlist = new string[] { };
        //    cptlist = CPTList.Split('|');
        //    IList<string> cptmodifier = new List<string>();
        //    EAndMCodingManager obj = new EAndMCodingManager();
        //    cptmodifier = obj.CPTModifier(cptlist);
        //    return JsonConvert.SerializeObject(cptmodifier);

        //}
        //[WebMethod(EnableSession = true)]
        //public string DeleteModifierforCPT(string CPTList)
        //{
        //    //if (ClientSession.UserName == string.Empty)
        //    //{
        //    //    HttpContext.Current.Response.StatusCode = 999;
        //    //    HttpContext.Current.Response.Status = "999 Session Expired";
        //    //    HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //    //    return "Session Expired";
        //    //}
        //    //if(CPT!="")
        //    //{

        //    //}
        //    string[] cptlist = new string[] { };
        //    cptlist = CPTList.Split('|');
        //    IList<string> cptmodifier = new List<string>();
        //    EAndMCodingManager obj = new EAndMCodingManager();
        //    cptmodifier = obj.CPTModifierDelete(cptlist);

        //    return JsonConvert.SerializeObject(cptmodifier);

        //}
        [WebMethod(EnableSession = true)]
        public string GetFavouriteCPTs()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string json = string.Empty;
            PhysicianProcedureManager PhysicianProcedureMngr = new PhysicianProcedureManager();
            IList<PhysicianProcedure> PhyProceList = PhysicianProcedureMngr.GetPhyCPTs(ClientSession.PhysicianId, ClientSession.LegalOrg);
            if (PhyProceList != null && PhyProceList.Count > 0)
            {
                var lstphyICDsResult = PhyProceList.Select(a => new
                {
                    CPTCode = a.Physician_Procedure_Code,
                    CPTDesc = a.Procedure_Description,
                    Category = a.Procedure_Type
                });
                json = new JavaScriptSerializer().Serialize(lstphyICDsResult);
            }
            return json;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFavouriteCPTsByTypeOfVisit(string type_of_visit)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string json = string.Empty;
            PhysicianProcedureManager PhysicianProcedureMngr = new PhysicianProcedureManager();
            if (type_of_visit == string.Empty)
                type_of_visit = "ALL";
            IList<PhysicianProcedure> PhyProceList = PhysicianProcedureMngr.GetProceduresUsingPhysicianIdAndTypeOfVisit(ClientSession.PhysicianId, type_of_visit, ClientSession.LegalOrg);
            if (PhyProceList != null && PhyProceList.Count > 0)
            {
                var lstphyICDsResult = PhyProceList.Select(a => new
                {
                    CPTCode = a.Physician_Procedure_Code,
                    CPTDesc = a.Procedure_Description,
                    Category = a.Order_Group_Name
                });
                json = new JavaScriptSerializer().Serialize(lstphyICDsResult);
            }
            return json;
        }




        [WebMethod(EnableSession = true)]
        public string GetFavouriteICDS()
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
            IList<PhysicianICD_9> phyICD9List = phyICd9Mngr.GetPhyLeafICDsandCategory(ClientSession.PhysicianId, ClientSession.LegalOrg);
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

        [WebMethod(EnableSession = true)]
        public string GetBillingInstruction(string _KeyWords)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string json = string.Empty;
            EncounterManager EncounterMngr = new EncounterManager();
            IList<Encounter> EncounterList = EncounterMngr.GetBillingInstructions(ClientSession.EncounterId, _KeyWords.Trim());
            if (EncounterList != null && EncounterList.Count > 0)
            {
                var lstphyICDsResult = EncounterList.Select(a => new
                {
                    BillingInstruction = a.Billing_Instruction
                });

                json = new JavaScriptSerializer().Serialize(lstphyICDsResult);
            }
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string GetFormviewCPTs(string sFormviewCPT)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            string sProcedureList = string.Empty;
            string json = string.Empty;

            if (sFormviewCPT != string.Empty)
            {

                //IList<ProcedureCodeLibrary> ProcedureList = new List<ProcedureCodeLibrary>();
                //IList<ProcedureCodeLibrary> ProcedureListchk = new List<ProcedureCodeLibrary>();
                //List<string> procedurecode = new List<string>();
                //procedurecode = sFormviewCPT.Split('|').ToArray().Select(a => a.ToString().Split('~')[0]).ToList<string>();
                //ProcedureCodeLibraryManager obj = new ProcedureCodeLibraryManager();
                //ProcedureList = obj.GetProcedureList(procedurecode);
                //IList<string> ProcedureListfinal = new List<String>();
                //var Record1 = (from b in sFormviewCPT.Split('|').ToArray() where !ProcedureListchk.Any(a => a.Procedure_Code == b.Split('~')[0]) select b).ToList();

                //if (Record1.Count > 0)
                //{
                //    for (int i = 0; i < ProcedureList.Count; i++)
                //    {

                //        // ProcedureListfinal.Add(ProcedureList[i].Procedure_Code + "~" + ProcedureList[i].Procedure_Description + "~" + ProcedureList[i].Procedure_Charge+"~"+ i+1);
                //        ProcedureListfinal.Add(ProcedureList[i].Procedure_Code + "~" + ProcedureList[i].Procedure_Description + "~" + ProcedureList[i].Procedure_Charge + "~" + ProcedureList[i].Sort_Order);
                //    }

                //    var ResultRecord1 = ProcedureListfinal.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1].Replace('$', '\"'), btnDelete = "Resources/Delete-Blue.png", Units = "1", CPTCheck = "6", Order = a.Split('~')[3], Amount = a.Split('~')[2] });
                //    json = new JavaScriptSerializer().Serialize(ResultRecord1);
                //}
                IList<ProcedureModifierLookup> ProcedureList = new List<ProcedureModifierLookup>();
                IList<ProcedureCodeLibrary> ProcedureListchk = new List<ProcedureCodeLibrary>();
                IList<ProcedureCodeLibrary> ProcedureListlibrary = new List<ProcedureCodeLibrary>();
                List<string> procedurecode = new List<string>();
                procedurecode = sFormviewCPT.Split('|').ToArray().Select(a => a.ToString().Split('~')[0]).ToList<string>();
                ProcedureModifierLookupManager obj = new ProcedureModifierLookupManager();
                ProcedureCodeLibraryManager objman = new ProcedureCodeLibraryManager();
                ProcedureList = obj.GetProcedureList(procedurecode);
                ProcedureListlibrary = objman.GetProcedureList(procedurecode);
                IList<string> ProcedureListfinal = new List<String>();
                var Record1 = (from b in sFormviewCPT.Split('|').ToArray() where !ProcedureListchk.Any(a => a.Procedure_Code == b.Split('~')[0]) select b).ToList();

                if (Record1.Count > 0)
                {
                    for (int i = 0; i < ProcedureListlibrary.Count; i++)
                    {


                        IList<ProcedureModifierLookup> tempSort = (from m in ProcedureList where m.Procedure_Code == ProcedureListlibrary[i].Procedure_Code.ToString() && m.Modifier == String.Empty select m).ToList<ProcedureModifierLookup>();
                        if (tempSort.Count > 0)
                        {
                            // ProcedureListfinal.Add(ProcedureList[i].Procedure_Code + "~" + ProcedureList[i].Procedure_Description + "~" + ProcedureList[i].Procedure_Charge+"~"+ i+1);
                            ProcedureListfinal.Add(ProcedureListlibrary[i].Procedure_Code + "~" + ProcedureListlibrary[i].Procedure_Description + "~" + ProcedureListlibrary[i].Procedure_Charge + "~" + tempSort[0].Sort_Order + "~" + tempSort[0].RVU);
                        }
                        else
                        {
                            ProcedureListfinal.Add(ProcedureListlibrary[i].Procedure_Code + "~" + ProcedureListlibrary[i].Procedure_Description + "~" + ProcedureListlibrary[i].Procedure_Charge + "~" + 9 + "~" + 0);
                        }
                    }

                    var ResultRecord1 = ProcedureListfinal.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1].Replace('$', '\"'), btnDelete = "Resources/Delete-Blue.png", Units = "1", CPTCheck = "6", Order = a.Split('~')[3], RVU = a.Split('~')[4], Amount = a.Split('~')[2] });
                    json = new JavaScriptSerializer().Serialize(ResultRecord1);
                }
            }

            return "{\"ListofCPTs\" :" + json + "}";
        }

        [WebMethod(EnableSession = true)]
        public string GetFormviewICDs(string sFormviewICD)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<AllICD_9> ICDList = new List<AllICD_9>();
            string sICDList = string.Empty;
            string json = string.Empty;
            if (sFormviewICD != string.Empty)
            {
                var Record1 = (from b in sFormviewICD.Split('|').ToArray() where !ICDList.Any(a => a.ICD_9 == b.Split('~')[0]) select b).ToList();
                if (Record1.Count > 0)
                {
                    var ResultRecord1 = Record1.Select(a => new { ICDCode = a.Split('~')[0], ICDDescription = a.Split('~')[1].Replace('$', '\"'), IsPrimary = a.Split('~')[2], Sequence = 'B' + a.Split('~')[3], ICDVersion = '0', EandMICDID = '0', btnDelete = "Resources/Delete-Blue.png", EnablePriRbtn = HttpContext.Current.Session["EnablePriRbtn"].ToString() });
                    json = new JavaScriptSerializer().Serialize(ResultRecord1);
                }
            }
            return "{\"ListofICDs\" :" + json + "}"; ;
        }

        [WebMethod(EnableSession = true)]
        public string SaveEandMCodingTable(object[] arylstCPT, object[] arylstICD, string sBillingInstruction, object[] arylstDelCPT, object[] arylstDelICD, string sESuperbillSubmitted)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            EAndMCodingManager EandMCodingMngr = new EAndMCodingManager();
            FillEandMCoding eandmDTO = new FillEandMCoding();
            Encounter EncRcrd = null;

            IList<EAndMCoding> EAndMCPTTempList = new List<EAndMCoding>();
            IList<EAndMCoding> EAndMCPTSaveList = new List<EAndMCoding>();
            IList<EAndMCoding> EAndMCPTUpdateList = new List<EAndMCoding>();
            IList<EAndMCoding> EAndMCPTDeleteList = new List<EAndMCoding>();
            IList<EAndMCoding> TempEAndMCoding = new List<EAndMCoding>();
            IList<EandMCodingICD> TempEAndMICD = new List<EandMCodingICD>();
            IList<EandMCodingICD> EAndMICDTempList = new List<EandMCodingICD>();

            IList<EandMCodingICD> EAndMICDSaveList = new List<EandMCodingICD>();
            IList<EandMCodingICD> EAndMICDUpdateList = new List<EandMCodingICD>();
            ImmunizationDTO ImmDelDTO = new ImmunizationDTO();
            CarePlan tobacco_CarePlan = new CarePlan();
            IList<ImmunizationHistory> ImmHisDelList = new List<ImmunizationHistory>();
            IList<Immunization> ImmDelList = new List<Immunization>();
            CarePlan CareplanUpdate = new CarePlan();
            IList<string> CPT_ImmDelCodes = new List<string>();
            IList<string> CPT_ICPDelCodes = new List<string>();

            if (HttpContext.Current.Session["EandMList"] != null && HttpContext.Current.Session["EandMICDList"] != null)
            {
                EAndMCPTTempList = (IList<EAndMCoding>)HttpContext.Current.Session["EandMList"];
                EAndMICDTempList = (IList<EandMCodingICD>)HttpContext.Current.Session["EandMICDList"];
            }
            else
            {
                //string FileNames = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                //string strXmlFilePaths = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileNames);

                IList<string> ilstEandMTagList = new List<string>();
                ilstEandMTagList.Add("EAndMCodingList");
                ilstEandMTagList.Add("EandMCodingICDList");



                IList<object> ilstEandMBlobFinal = new List<object>();
                ilstEandMBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstEandMTagList);
                IList<EAndMCoding> lsteandmtemp = new List<EAndMCoding>();
                IList<EandMCodingICD> lsteandmICDblobtemp = new List<EandMCodingICD>();
                IList<string> EandMCPTs = new List<string>();
                EAndMCPTTempList = new List<EAndMCoding>();
                if (ilstEandMBlobFinal != null && ilstEandMBlobFinal.Count > 0)
                {
                    if (ilstEandMBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[0]).Count; iCount++)
                        {
                            lsteandmtemp.Add((EAndMCoding)((IList<object>)ilstEandMBlobFinal[0])[iCount]);
                        }
                        for (int i = 0; i < lsteandmtemp.Count; i++)
                        {
                            if (lsteandmtemp[0].Encounter_ID == ClientSession.EncounterId)
                            {
                                if (EandMCPTs.IndexOf(lsteandmtemp[0].Procedure_Code) == -1)
                                    EandMCPTs.Add(lsteandmtemp[0].Procedure_Code);//BugID:49118
                                if (lsteandmtemp[0].Is_Delete == "N" || lsteandmtemp[0].Is_Delete == "")
                                {
                                    //if (aryAccessCPT.Contains(EAndMCoding.Procedure_Code) == false)
                                    //{
                                    EAndMCPTTempList.Add(lsteandmtemp[0]);
                                    //ProcList.Add(EAndMCoding.Procedure_Code + "~" + EAndMCoding.Procedure_Code_Description + "~" + EAndMCoding.Id + "~" + EAndMCoding.Units + "~" + EAndMCoding.Modifier1 + "~" + EAndMCoding.Modifier2 + "~" + EAndMCoding.Modifier3 + "~" + EAndMCoding.Modifier4 + "~" + EAndMCoding.Sequence + "~" + EAndMCoding.Version);
                                    //    aryAccessCPT.Add(EAndMCoding.Procedure_Code);
                                    //}
                                }
                            }
                        }
                        HttpContext.Current.Session["EandMList"] = EAndMCPTTempList;


                    }
                    EAndMICDTempList = new List<EandMCodingICD>();
                    if (ilstEandMBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[1]).Count; iCount++)
                        {
                            lsteandmICDblobtemp.Add((EandMCodingICD)((IList<object>)ilstEandMBlobFinal[1])[iCount]);


                        }
                        EAndMICDTempList = (from m in lsteandmICDblobtemp where m.Encounter_ID == ClientSession.EncounterId && (m.Is_Delete == "N" || m.Is_Delete == "" )select m).ToList<EandMCodingICD>();

                    }

                }

                //if (File.Exists(strXmlFilePaths) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlNodeList xmlTagName = null;

                //    using (FileStream fs = new FileStream(strXmlFilePaths
                //       , FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);




                //if (itemDoc.GetElementsByTagName("EAndMCodingList") != null)
                //{
                //    if (itemDoc.GetElementsByTagName("EAndMCodingList").Count > 0)
                //    {
                //        xmlTagName = itemDoc.GetElementsByTagName("EAndMCodingList")[0].ChildNodes;
                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(EAndMCoding));
                //                EAndMCoding EAndMCoding = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EAndMCoding;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (EAndMCoding != null)
                //                {
                //                    propInfo = from obji in ((EAndMCoding)EAndMCoding).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(EAndMCoding, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(EAndMCoding, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(EAndMCoding, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(EAndMCoding, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(EAndMCoding, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(EAndMCoding, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (EAndMCoding.Encounter_ID == ClientSession.EncounterId)
                //                    {
                //                        if (EandMCPTs.IndexOf(EAndMCoding.Procedure_Code) == -1)
                //                            EandMCPTs.Add(EAndMCoding.Procedure_Code);//BugID:49118
                //                        if (EAndMCoding.Is_Delete == "N" || EAndMCoding.Is_Delete == "")
                //                        {
                //                            //if (aryAccessCPT.Contains(EAndMCoding.Procedure_Code) == false)
                //                            //{
                //                            EAndMCPTTempList.Add(EAndMCoding);
                //                            //ProcList.Add(EAndMCoding.Procedure_Code + "~" + EAndMCoding.Procedure_Code_Description + "~" + EAndMCoding.Id + "~" + EAndMCoding.Units + "~" + EAndMCoding.Modifier1 + "~" + EAndMCoding.Modifier2 + "~" + EAndMCoding.Modifier3 + "~" + EAndMCoding.Modifier4 + "~" + EAndMCoding.Sequence + "~" + EAndMCoding.Version);
                //                            //    aryAccessCPT.Add(EAndMCoding.Procedure_Code);
                //                            //}
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}


                //if (itemDoc.GetElementsByTagName("EandMCodingICDList") != null)
                //{
                //    if (itemDoc.GetElementsByTagName("EandMCodingICDList").Count > 0)
                //    {
                //        xmlTagName = itemDoc.GetElementsByTagName("EandMCodingICDList")[0].ChildNodes;
                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(EandMCodingICD));
                //                EandMCodingICD EandMCodingICD = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as EandMCodingICD;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                if (EandMCodingICD != null)
                //                {
                //                    propInfo = from obji in ((EandMCodingICD)EandMCodingICD).GetType().GetProperties() select obji;

                //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                    {
                //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                        {
                //                            foreach (PropertyInfo property in propInfo)
                //                            {
                //                                if (property.Name == nodevalue.Name)
                //                                {
                //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                        property.SetValue(EandMCodingICD, Convert.ToUInt64(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                        property.SetValue(EandMCodingICD, Convert.ToString(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                        property.SetValue(EandMCodingICD, Convert.ToDateTime(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                        property.SetValue(EandMCodingICD, Convert.ToInt32(nodevalue.Value), null);
                //                                    else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                        property.SetValue(EandMCodingICD, Convert.ToDecimal(nodevalue.Value), null);
                //                                    else
                //                                        property.SetValue(EandMCodingICD, nodevalue.Value, null);
                //                                }
                //                            }
                //                        }
                //                    }
                //                    if (EandMCodingICD.Encounter_ID == ClientSession.EncounterId && (EandMCodingICD.Is_Delete == "N" || EandMCodingICD.Is_Delete == ""))
                //                    {
                //                        EAndMICDTempList.Add(EandMCodingICD);
                //                    }

                //                }
                //            }
                //        }
                //    }
                //}
                //HttpContext.Current.Session["EandMICDList"] = EAndMICDTempList;
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
            }

            bool Is_CMG_Ancillary = false;
            //string Ancilliary_FacilityName = string.Empty;
            //if (System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"] != null)
            //    Ancilliary_FacilityName = System.Configuration.ConfigurationSettings.AppSettings["AncillaryTestClinic"].ToString();
            var facAncillary = from f in ApplicationObject.facilityLibraryList where f.Fac_Name == ClientSession.FacilityName select f;
            IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();


            //if (Ancilliary_FacilityName.ToString().ToUpper() == ClientSession.FacilityName.ToString().ToUpper())
            if (ilstFacAncillary.Count > 0 && ilstFacAncillary[0].Is_Ancillary == "Y")
            {
                Is_CMG_Ancillary = true;
            }

            if (EAndMCPTTempList != null && EAndMCPTTempList.Count > 0)
                EAndMCPTDeleteList = EAndMCPTTempList.Where(e => !arylstCPT.Any(b => b.ToString().Split('~')[8] == Convert.ToString(e.Id) && b.ToString().Split('~')[8] != string.Empty)).ToList<EAndMCoding>();
            foreach (EAndMCoding cpt in EAndMCPTDeleteList)
            {
                cpt.Is_Delete = "Y";
                //Cap - 1301

                //EAndMCPTUpdateList.Add(cpt);
                cpt.Sort_Order = 1000;
                EAndMCPTSaveList.Add(cpt);

                if (CPT_ImmDelCodes.IndexOf(cpt.Procedure_Code.Trim()) == -1)
                {
                    CPT_ImmDelCodes.Add(cpt.Procedure_Code.Trim());
                }
                /*if (cpt.Procedure_Code.Trim() == "90732" || cpt.Procedure_Code.Trim() == "90670")
                {
                    if (CPT_ImmDelCodes.IndexOf(cpt.Procedure_Code.Trim()) == -1)
                    {
                        CPT_ImmDelCodes.Add(cpt.Procedure_Code.Trim());
                    }
                }*/
                if (cpt.Procedure_Code.Trim() == "99406" || cpt.Procedure_Code.Trim() == "99407")
                {
                    if (CPT_ICPDelCodes.IndexOf(cpt.Procedure_Code.Trim()) == -1)
                    {
                        CPT_ICPDelCodes.Add(cpt.Procedure_Code.Trim());
                    }
                }
            }
            foreach (object oj in arylstDelCPT)
            {
                if (CPT_ImmDelCodes.IndexOf(oj.ToString().Split('~')[0].Trim()) == -1)
                {
                    CPT_ImmDelCodes.Add(oj.ToString().Split('~')[0].Trim());
                }
                /*if (oj.ToString().Split('~')[0].Trim() == "90732" || oj.ToString().Split('~')[0].Trim() == "90670")
                {
                    if (CPT_ImmDelCodes.IndexOf(oj.ToString().Split('~')[0].Trim()) == -1)
                    {
                        CPT_ImmDelCodes.Add(oj.ToString().Split('~')[0].Trim());
                    }
                }*/
                if (oj.ToString().Split('~')[0].Trim() == "99406" || oj.ToString().Split('~')[0].Trim() == "99407")
                {
                    if (CPT_ICPDelCodes.IndexOf(oj.ToString().Split('~')[0].Trim()) == -1)
                    {
                        CPT_ICPDelCodes.Add(oj.ToString().Split('~')[0].Trim());
                    }
                }
            }
            GetSourceDeleteList(CPT_ImmDelCodes, CPT_ICPDelCodes, out ImmDelDTO, out tobacco_CarePlan);

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



            //object[] arrTempCPT = arylstCPT.Where(a => a.ToString().Split('~')[8] == string.Empty).ToArray();//SaveList
            //Cap - 1301
            List<string> lstCPtlist = new List<string>();
            List<string> lstCPtsortOrder = new List<string>();
            foreach (object objCPT in arylstCPT)
            {
                lstCPtlist.Add(objCPT.ToString().Split('~')[0]);
                lstCPtsortOrder.Add(objCPT.ToString().Split('~')[0] + "~" + objCPT.ToString().Split('~')[3]);

            }

            IList<ProcedureModifierLookup> lstcptlib = new List<ProcedureModifierLookup>();
            IList<ProcedureModifierLookup> lstcptlibtemp = new List<ProcedureModifierLookup>();
            ProcedureModifierLookupManager objmanager = new ProcedureModifierLookupManager();

            lstcptlib = objmanager.GetProcedureList(lstCPtlist);
            int isort_order = 1;
            for (int k = 0; k < lstCPtsortOrder.Count; k++)
            {
                IList<ProcedureModifierLookup> lsttempCPT = new List<ProcedureModifierLookup>();
                lsttempCPT = (from m in lstcptlib where m.Procedure_Code == lstCPtsortOrder[k].Split('~')[0] && m.Modifier == lstCPtsortOrder[k].Split('~')[1] select m).ToList<ProcedureModifierLookup>();
                if (lsttempCPT.Count > 0)
                {

                    lstcptlibtemp.Add(lsttempCPT[0]);


                }
                else
                {
                    lsttempCPT = new List<ProcedureModifierLookup>();
                    lsttempCPT = (from m in lstcptlib where m.Procedure_Code == lstCPtsortOrder[k].Split('~')[0] && m.Modifier == String.Empty select m).ToList<ProcedureModifierLookup>();
                    if (lsttempCPT.Count > 0)
                    {

                        lstcptlibtemp.Add(lsttempCPT[0]);


                    }

                }


            }

            lstcptlibtemp = lstcptlibtemp.OrderBy(c => c.Sort_Order).ThenByDescending(n => n.RVU).ThenBy(o => o.Procedure_Code).ToList<ProcedureModifierLookup>();

            for (int k = 0; k < lstcptlibtemp.Count; k++)

            {
                lstcptlibtemp[k].Sort_Order = k + 1;
            }
            isort_order = lstcptlibtemp.Count + 1;


            foreach (object objCPT in arylstCPT)
            {
                EAndMCoding objEandMCoding = null;
                IList<EAndMCoding> eandmCPTList = null;
                //Cap - 1301
                //var eandmList = from eandm in EAndMCPTTempList where Convert.ToString(eandm.Id) == objCPT.ToString().Split('~')[8] select eandm;
                //eandmCPTList = eandmList.ToList<EAndMCoding>();

                //if (eandmCPTList.Count > 0)
                //{
                //    objEandMCoding = eandmCPTList[0];
                //}
                //else
                //{
                //    objEandMCoding = new EAndMCoding();
                //}
                objEandMCoding = new EAndMCoding();

                objEandMCoding.Encounter_ID = ClientSession.EncounterId;
                objEandMCoding.Human_ID = ClientSession.HumanId;
                objEandMCoding.Physician_ID = ClientSession.PhysicianId;
                objEandMCoding.Procedure_Code = objCPT.ToString().Split('~')[0];
                objEandMCoding.Procedure_Code_Description = objCPT.ToString().Split('~')[1];
                if (objCPT.ToString().Split('~')[2] != "")
                    objEandMCoding.Units = Convert.ToInt32(objCPT.ToString().Split('~')[2]);
                objEandMCoding.Modifier1 = objCPT.ToString().Split('~')[3];
                objEandMCoding.Modifier2 = objCPT.ToString().Split('~')[4];
                objEandMCoding.Modifier3 = objCPT.ToString().Split('~')[5];
                objEandMCoding.Modifier4 = objCPT.ToString().Split('~')[6];
                objEandMCoding.Diagnosis_Pointer_1 = objCPT.ToString().Split('~')[12];
                objEandMCoding.Diagnosis_Pointer_2 = objCPT.ToString().Split('~')[13];
                objEandMCoding.Diagnosis_Pointer_3 = objCPT.ToString().Split('~')[14];
                objEandMCoding.Diagnosis_Pointer_4 = objCPT.ToString().Split('~')[15];
                objEandMCoding.Diagnosis_Pointer_5 = objCPT.ToString().Split('~')[16];
                objEandMCoding.Diagnosis_Pointer_6 = objCPT.ToString().Split('~')[17];
                //Cap - 1301
                //objEandMCoding.Sort_Order = Convert.ToInt32(objCPT.ToString().Split('~')[18]);

                IList<ProcedureModifierLookup> lsttempCPT = new List<ProcedureModifierLookup>();
                lsttempCPT = (from m in lstcptlibtemp where m.Procedure_Code == objCPT.ToString().Split('~')[0]  && m.Modifier== objCPT.ToString().Split('~')[3] select m).ToList<ProcedureModifierLookup>();

                if (lsttempCPT.Count > 0)
                    objEandMCoding.Sort_Order = lsttempCPT[0].Sort_Order;// Convert.ToInt32(objCPT.ToString().Split('~')[18]);
               //Cap - 1604
                //else
               //    objEandMCoding.Sort_Order = 0;
                else
                {
                    lsttempCPT = (from m in lstcptlibtemp where m.Procedure_Code == objCPT.ToString().Split('~')[0] && m.Modifier == string.Empty select m).ToList<ProcedureModifierLookup>();
                    if (lsttempCPT.Count > 0)
                    {
                        objEandMCoding.Sort_Order = lsttempCPT[0].Sort_Order;
                    }
                    else
                    {
                        objEandMCoding.Sort_Order = 0;
                    }
                }

                //if (objCPT.ToString().Split('~')[7] != "")
                //    objEandMCoding.Sequence = Convert.ToInt32(objCPT.ToString().Split('~')[7]);
                objEandMCoding.Is_Delete = "N";
                //objEandMCoding.CPT_Order = Convert.ToInt32(objCPT.ToString().Split('~')[10]);
                objEandMCoding.Charge_Amount = Convert.ToDecimal(objCPT.ToString().Split('~')[11]);
                //Cap - 1301
                //if (eandmCPTList.Count > 0)
                //{
                //    if (objCPT.ToString().Split('~')[9] != "")
                //        objEandMCoding.Version = Convert.ToInt32(objCPT.ToString().Split('~')[9]);
                //    objEandMCoding.Modified_By = ClientSession.UserName;
                //    objEandMCoding.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                //    EAndMCPTUpdateList.Add(objEandMCoding);
                //}
                //else
                //{
                    objEandMCoding.Created_By = ClientSession.UserName;
                    objEandMCoding.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    EAndMCPTSaveList.Add(objEandMCoding);
                //}
            }
            //Cap - 1301
            IList<EAndMCoding> lsttempsave = new List<EAndMCoding>();
            lsttempsave = (from m in EAndMCPTSaveList where m.Sort_Order == 0 select m).ToList<EAndMCoding>();
            if (lsttempsave.Count > 0)
            {
                for (int i = 0; i < EAndMCPTSaveList.Count; i++)
                {
                    if (EAndMCPTSaveList[i].Sort_Order == 0)
                    {
                        EAndMCPTSaveList[i].Sort_Order = isort_order;
                        isort_order = isort_order + 1;
                    }
                }
            }

            //Added for BugID:49118 - All deleted ICDs even if autosuggested/ entered and deleted will be saved in E_M_Coding table with Is_delete = 'Y'.

            foreach (object objCPT in arylstDelCPT)
            {
                if (objCPT.ToString().Split('~')[8].Trim() == string.Empty)
                {
                    EAndMCoding objEandMCoding = null;

                    objEandMCoding = new EAndMCoding();

                    objEandMCoding.Encounter_ID = ClientSession.EncounterId;
                    objEandMCoding.Human_ID = ClientSession.HumanId;
                    objEandMCoding.Physician_ID = ClientSession.PhysicianId;
                    objEandMCoding.Procedure_Code = objCPT.ToString().Split('~')[0];
                    objEandMCoding.Procedure_Code_Description = objCPT.ToString().Split('~')[1];
                    if (objCPT.ToString().Split('~')[2] != "")
                        objEandMCoding.Units = Convert.ToInt32(objCPT.ToString().Split('~')[2]);
                    objEandMCoding.Modifier1 = objCPT.ToString().Split('~')[3];
                    objEandMCoding.Modifier2 = objCPT.ToString().Split('~')[4];
                    objEandMCoding.Modifier3 = objCPT.ToString().Split('~')[5];
                    objEandMCoding.Modifier4 = objCPT.ToString().Split('~')[6];
                    objEandMCoding.Diagnosis_Pointer_1 = objCPT.ToString().Split('~')[12];
                    objEandMCoding.Diagnosis_Pointer_2 = objCPT.ToString().Split('~')[13];
                    objEandMCoding.Diagnosis_Pointer_3 = objCPT.ToString().Split('~')[14];
                    objEandMCoding.Diagnosis_Pointer_4 = objCPT.ToString().Split('~')[15];
                    objEandMCoding.Diagnosis_Pointer_5 = objCPT.ToString().Split('~')[16];
                    objEandMCoding.Diagnosis_Pointer_6 = objCPT.ToString().Split('~')[17];
                    objEandMCoding.Sort_Order =1000;
                    //if (objCPT.ToString().Split('~')[7] != "")
                    //    objEandMCoding.Sequence = Convert.ToInt32(objCPT.ToString().Split('~')[7]);
                    objEandMCoding.Is_Delete = "Y";
                    //objEandMCoding.CPT_Order = Convert.ToInt32(objCPT.ToString().Split('~')[10]);
                    objEandMCoding.Charge_Amount = Convert.ToDecimal(objCPT.ToString().Split('~')[11]);
                    objEandMCoding.Created_By = ClientSession.UserName;
                    objEandMCoding.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    EAndMCPTSaveList.Add(objEandMCoding);
                }

            }


            object[] arrTempICD = arylstICD.Where(a => a.ToString().Split('~')[8] == string.Empty).ToArray();//SaveList

            IList<EandMCodingICD> OverAllICD = new List<EandMCodingICD>();

            foreach (object objICD in arylstICD)
            {
                EandMCodingICD objEandMCodingICD = null;
                IList<EandMCodingICD> eandmICDList = null;

                var eandmList = from eandmicd in EAndMICDTempList where Convert.ToString(eandmicd.ICD).Trim() == objICD.ToString().Split('~')[0].Trim() && Convert.ToString(eandmicd.Source).Trim() == objICD.ToString().Split('~')[11].Trim() select eandmicd;
                eandmICDList = eandmList.ToList<EandMCodingICD>();

                if (eandmICDList.Count > 0)
                {
                    objEandMCodingICD = eandmICDList[0];

                    if (objICD.ToString().Split('~')[2] == "Y")
                    {
                        objEandMCodingICD.ICD_Category = "Primary";
                    }
                    else
                    {
                        objEandMCodingICD.ICD_Category = "None";
                    }
                    objEandMCodingICD.Source = objICD.ToString().Split('~')[11];//BugID:48668 -- ServProc REVAMP
                    objEandMCodingICD.Sequence = objICD.ToString().Split('~')[12];
                    objEandMCodingICD.Modified_By = ClientSession.UserName;
                    objEandMCodingICD.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                    EAndMICDUpdateList.Add(objEandMCodingICD); //UpdateICD List
                    OverAllICD.Add(objEandMCodingICD);
                }
                else
                {
                    objEandMCodingICD = new EandMCodingICD();

                    objEandMCodingICD.Encounter_ID = ClientSession.EncounterId;
                    objEandMCodingICD.Human_ID = ClientSession.HumanId;
                    objEandMCodingICD.ICD = objICD.ToString().Split('~')[0];
                    objEandMCodingICD.ICD_Description = objICD.ToString().Split('~')[1];
                    objEandMCodingICD.Source = objICD.ToString().Split('~')[11];//BugID:48668 -- ServProc REVAMP
                    objEandMCodingICD.Sequence = objICD.ToString().Split('~')[12];
                    if (objICD.ToString().Split('~')[2] == "Y")
                    {
                        objEandMCodingICD.ICD_Category = "Primary";
                    }
                    else
                    {
                        objEandMCodingICD.ICD_Category = "None";
                    }
                    objEandMCodingICD.ICD_Type = "";
                    objEandMCodingICD.Is_Delete = "N";
                    objEandMCodingICD.Created_By = ClientSession.UserName;
                    objEandMCodingICD.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    EAndMICDSaveList.Add(objEandMCodingICD); // SaveICD List
                    OverAllICD.Add(objEandMCodingICD);

                }
            }
            //Commented for the Enhancement save ICD code of service procedure in Assessment tab

            //Newly Added Code - For Deleting the EandMCodingICD with Source as Assessment, if the ICD is deleted in Assessment
            //if (HttpContext.Current.Session["EandMICDList"] != null && OverAllICD != null && OverAllICD.Count > 0)
            //{
            //    IList<EandMCodingICD> ilstEandMList = (IList<EandMCodingICD>)HttpContext.Current.Session["EandMICDList"];
            //    IList<EandMCodingICD> ilstEandMTemp = new List<EandMCodingICD>();
            //    var Del = from e in ilstEandMList where e.Source == "ASSESSMENT" select e;
            //    ilstEandMTemp = Del.ToList<EandMCodingICD>();

            //    IList<EandMCodingICD> ilstNewAssessment = new List<EandMCodingICD>();
            //    IList<EandMCodingICD> ilstDeleteAssessment = new List<EandMCodingICD>();

            //    var DelAssesslist = from e in OverAllICD where e.Source == "ASSESSMENT" select e;
            //    ilstNewAssessment = DelAssesslist.ToList<EandMCodingICD>();

            //    //if (ilstEandMTemp.Count > 0)
            //    //{
            //    ilstDeleteAssessment = ilstEandMTemp.Except(ilstNewAssessment).ToList<EandMCodingICD>();

            //    foreach (EandMCodingICD icd in ilstDeleteAssessment)
            //    {
            //        icd.Is_Delete = "Y";
            //        EAndMICDUpdateList.Add(icd);
            //    }
            //    //}

            //}
            //Added for BugID:49118 - All deleted ICDs even if autosuggested/ entered and deleted will be saved in E_M_Coding table with Is_delete = 'Y'.
            //object[] arrTempDelICD = arylstDelICD.Where(a => a.ToString().Split('~')[8] == string.Empty).ToArray();//SaveList
            foreach (object objICD in arylstDelICD)
            {
                EandMCodingICD objEandMCodingICD = null;
                IList<EandMCodingICD> eandmICDList = null;

                //var eandmList = from eandmicd in EAndMICDTempList where Convert.ToString(eandmicd.Id) == objICD.ToString().Split('~')[8] select eandmicd;
                var eandmList = from eandmicd in EAndMICDTempList where eandmicd.ICD == objICD.ToString() select eandmicd;
                eandmICDList = eandmList.ToList<EandMCodingICD>();

                if (eandmICDList.Count > 0)
                {
                    objEandMCodingICD = eandmICDList[0];

                    //if (arylstICD[2] == "Y")
                    //{
                    //    objEandMCodingICD.ICD_Category = "Primary";
                    //}
                    //else
                    //{
                    //    objEandMCodingICD.ICD_Category = "None";
                    //}
                    objEandMCodingICD.Modified_By = ClientSession.UserName;
                    objEandMCodingICD.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objEandMCodingICD.Is_Delete = "Y";
                    EAndMICDUpdateList.Add(objEandMCodingICD); //UpdateICD List
                }
            }


            if (ImmDelDTO.ImmunizationHistoryList != null && ImmDelDTO.ImmunizationHistoryList.Count > 0)
            {
                ImmHisDelList = ImmDelDTO.ImmunizationHistoryList;
            }
            if (ImmDelDTO.Immunization != null && ImmDelDTO.Immunization.Count > 0)
            {
                ImmDelList = ImmDelDTO.Immunization;
            }
            if (tobacco_CarePlan != null && tobacco_CarePlan.Id != 0)
            {
                CareplanUpdate = tobacco_CarePlan;
            }
            EncounterManager objencManager = new EncounterManager();
            Encounter EncRecord = null;
            //Jira CAP-512
            EncounterBlobManager encounterBlobManager = new EncounterBlobManager();
            IList<Encounter_Blob> encounterBlobList = null;
            byte[] bytesHumanXml = null;
            encounterBlobList =encounterBlobManager.GetEncounterBlob(ClientSession.EncounterId);
            if (encounterBlobList != null && encounterBlobList.Count > 0)
            {
                bytesHumanXml = encounterBlobList[0].Human_XML;
            }
            //Check for Z00.00 or Z00.01 Validation, if G Code is added

            //ICD
            IList<EandMCodingICD> lstInsertUpdateICD = new List<EandMCodingICD>();

            lstInsertUpdateICD = EAndMICDSaveList.Where(a => a.Is_Delete.Trim().ToUpper() != "Y").ToList().
                Concat(EAndMICDUpdateList.Where(a => a.Is_Delete.Trim().ToUpper() != "Y").ToList()).ToList().Concat(EAndMICDTempList.Where(a => a.Source == "ASSESSMENT" && a.Is_Delete.Trim().ToUpper() != "Y")).ToList();

            //CPT
            IList<EAndMCoding> lstInsertUpdateCpt = new List<EAndMCoding>();
            //Cap - 1301
            // lstInsertUpdateCpt = EAndMCPTSaveList.Where(a => a.Is_Delete.Trim().ToUpper() != "Y").ToList().Concat(EAndMCPTUpdateList.Where(a => a.Is_Delete.Trim().ToUpper() != "Y").ToList()).ToList();
            lstInsertUpdateCpt = EAndMCPTSaveList.Where(a => a.Is_Delete.Trim().ToUpper() != "Y").ToList();


            Boolean bGcode = CheckGcodes(lstInsertUpdateICD, lstInsertUpdateCpt);
            string isZcode = "";
            if (bGcode == false)
            {
                //   return JsonConvert.SerializeObject("530024");
                // var result = new { IsBillableNo = "180045" };
                // return JsonConvert.SerializeObject(result);
                isZcode = "180045";
            }



            //ClientSession.FillEncounterandWFObject.EncRecord = objencManager.GetEncounterByEncounterID(ClientSession.EncounterId)[0];//BugID:51613
            //Cap - 1301
            IList<EAndMCoding> lsteandmcodingDeletelist = new List<EAndMCoding>();
            EAndMCodingManager objeandm = new EAndMCodingManager();
            lsteandmcodingDeletelist = objeandm.GetDetailsByEncounterID(ClientSession.EncounterId);
            IList<EAndMCoding> lstDeleteList = new List<EAndMCoding>();
            lstDeleteList = (from m in lsteandmcodingDeletelist where m.Is_Delete == "Y" select m).ToList<EAndMCoding>();

            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord == null || ClientSession.FillEncounterandWFObject.EncRecord.Id == 0)
                {
                    IList<Encounter> ilstEnc = objencManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                    if (ilstEnc.Count > 0)
                    {
                        EncRecord = ilstEnc[0];

                        EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                        if (EncRecord.Is_EandM_Submitted == "Y")
                        {
                            if (EncRecord.Batch_Status.ToUpper() == "CLOSED")
                            {
                                EncRecord.Batch_Status = "REOPEN";
                            }
                            else
                            {
                                if (EncRecord.Batch_Status.ToUpper() != "REOPEN")
                                    EncRecord.Batch_Status = "MODIFIED";
                            }
                        }
                        //Cap - 1301
                        //eandmDTO = EandMCodingMngr.SaveUpdateEandMCoding(EAndMCPTSaveList, EAndMCPTUpdateList, EAndMICDSaveList, ClientSession.UserName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList, EncRecord, sBillingInstruction, ImmDelList, ImmHisDelList, CareplanUpdate);
                        EAndMCPTSaveList = EAndMCPTSaveList.Concat(lstDeleteList).ToList();
                        eandmDTO = EandMCodingMngr.SaveUpdateEandMCoding(EAndMCPTSaveList, EAndMCPTUpdateList, EAndMICDSaveList, ClientSession.UserName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList, EncRecord, sBillingInstruction, ImmDelList, ImmHisDelList, CareplanUpdate, lsteandmcodingDeletelist);

                    }

                }
                else if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    ClientSession.FillEncounterandWFObject.EncRecord.Local_Time = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                    //if (ClientSession.FillEncounterandWFObject.EncRecord.Is_EandM_Submitted == "Y")
                    //ClientSession.FillEncounterandWFObject.EncRecord.Batch_Status = "MODIFIED";
                    if (ClientSession.FillEncounterandWFObject.EncRecord.Is_EandM_Submitted == "Y")//By naveena For Submitting esuper bill second time
                    {
                        if (ClientSession.FillEncounterandWFObject.EncRecord.Batch_Status.ToUpper() == "CLOSED")
                        {
                            ClientSession.FillEncounterandWFObject.EncRecord.Batch_Status = "REOPEN";
                        }
                        else
                        {
                            if (ClientSession.FillEncounterandWFObject.EncRecord.Batch_Status.ToUpper() != "REOPEN")//For BUg Id 52918 
                                ClientSession.FillEncounterandWFObject.EncRecord.Batch_Status = "MODIFIED";
                        }
                    }
                    //Cap -1301
                    //eandmDTO = EandMCodingMngr.SaveUpdateEandMCoding(EAndMCPTSaveList, EAndMCPTUpdateList, EAndMICDSaveList, ClientSession.UserName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList, ClientSession.FillEncounterandWFObject.EncRecord, sBillingInstruction, ImmDelList, ImmHisDelList, CareplanUpdate);
                    EAndMCPTSaveList = EAndMCPTSaveList.Concat(lstDeleteList).ToList();
                    eandmDTO = EandMCodingMngr.SaveUpdateEandMCoding(EAndMCPTSaveList, EAndMCPTUpdateList, EAndMICDSaveList, ClientSession.UserName, UtilityManager.ConvertToUniversal(), EAndMICDUpdateList, ClientSession.FillEncounterandWFObject.EncRecord, sBillingInstruction, ImmDelList, ImmHisDelList, CareplanUpdate, lsteandmcodingDeletelist);
                }
            }
            // ArrayList aryAccessCPT = new ArrayList();
            IList<string> ProcList = new List<string>();
            IList<string> ICDList = new List<string>();
            ArrayList aryAccessICD = new ArrayList();
            if (eandmDTO.EncounterList != null)
            {
                if (eandmDTO.EncounterList.Count > 0)
                    ClientSession.FillEncounterandWFObject.EncRecord = eandmDTO.EncounterList[0];
            }
            if (eandmDTO.EandMCodingList != null)
            {
                eandmDTO.EandMCodingList = eandmDTO.EandMCodingList.Where(a => a.Is_Delete == "N").ToList();
                for (int i = 0; i < eandmDTO.EandMCodingList.Count; i++)
                {
                    //if (aryAccessCPT.Contains(eandmDTO.EandMCodingList[i].Procedure_Code) == false)
                    //{                    
                    ProcList.Add(eandmDTO.EandMCodingList[i].Procedure_Code + "~" + eandmDTO.EandMCodingList[i].Procedure_Code_Description + "~" + eandmDTO.EandMCodingList[i].Id + "~" + eandmDTO.EandMCodingList[i].Units + "~" + eandmDTO.EandMCodingList[i].Modifier1 + "~" + eandmDTO.EandMCodingList[i].Modifier2 + "~" + eandmDTO.EandMCodingList[i].Modifier3 + "~" + eandmDTO.EandMCodingList[i].Modifier4 + "~" + string.Empty + "~" + eandmDTO.EandMCodingList[i].Version + "~" + string.Empty + "~" + eandmDTO.EandMCodingList[i].Diagnosis_Pointer_1 + "~" + eandmDTO.EandMCodingList[i].Diagnosis_Pointer_2 + "~" + eandmDTO.EandMCodingList[i].Diagnosis_Pointer_3 + "~" + eandmDTO.EandMCodingList[i].Diagnosis_Pointer_4 + "~" + eandmDTO.EandMCodingList[i].Diagnosis_Pointer_5 + "~" + eandmDTO.EandMCodingList[i].Diagnosis_Pointer_6);
                    //    aryAccessCPT.Add(eandmDTO.EandMCodingList[i].Procedure_Code);
                    //}
                }
            }
            if (eandmDTO.EandMCodingICDList != null)
            {
                eandmDTO.EandMCodingICDList = eandmDTO.EandMCodingICDList.Where(a => a.Is_Delete == "N").ToList();
                for (int j = 0; j < eandmDTO.EandMCodingICDList.Count; j++)
                {
                    string sPrimary = string.Empty;
                    if (eandmDTO.EandMCodingICDList[j].ICD_Category == "Primary")
                    {
                        sPrimary = "Pri";
                    }

                    var eandMICDList = from eandmICD in eandmDTO.EandMCodingICDList where eandmICD.ICD == eandmDTO.EandMCodingICDList[j].ICD select eandmICD;
                    IList<EandMCodingICD> templist = eandMICDList.ToList<EandMCodingICD>();
                    string sIDList = string.Empty;
                    string sICDVersion = string.Empty;
                    if (templist.Count > 0)
                    {
                        for (int k = 0; k < templist.Count; k++)
                        {
                            if (k == 0)
                            {
                                sICDVersion = templist[k].Version.ToString();
                            }
                            else
                            {
                                sICDVersion = "," + templist[k].Version.ToString();
                            }
                        }
                    }

                    //BugID:48668 -- ServProc REVAMP
                    string Source = string.Empty;
                    if (eandmDTO.EandMCodingICDList[j].Source.ToUpper() == "EMICD")
                        Source = "EMICD";
                    else if (eandmDTO.EandMCodingICDList[j].Source.ToUpper() == "ASSESSMENT")
                    {
                        Source = "ASSESSMENT";
                    }
                    else if (eandmDTO.EandMCodingICDList[j].Source.ToUpper() == "ORDERS_ASSESSMENT")
                    {
                        Source = "ORDERS_ASSESSMENT";
                    }
                    if (!ICDList.Any(a => a.Split('~')[1] == eandmDTO.EandMCodingICDList[j].ICD))
                        ICDList.Add(Source + "~" + eandmDTO.EandMCodingICDList[j].ICD + "~" + eandmDTO.EandMCodingICDList[j].ICD_Description + "~" + sICDVersion + "~" + sPrimary + "~" + eandmDTO.EandMCodingICDList[j].Id + "~" + eandmDTO.EandMCodingICDList[j].Sequence + "~" + ",");
                    else
                    {
                        IList<string> sReomveICDDesc = ICDList.Select(a => a.Split('~')[1]).ToList();
                        int iIndex = sReomveICDDesc.IndexOf(eandmDTO.EandMCodingICDList[j].ICD);
                        //if (iIndex > -1)
                        //    if (ICDList[iIndex].Split('~').Length == 8)
                        //    {
                        //        //added for 6th ICD Column Marking
                        //        if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length < 6)
                        //        {
                        //            for (int i = 0; i < eandmDTO.EandMCodingICDList[j].Sequence; i++)
                        //            {
                        //                if (eandmDTO.EandMCodingICDList[j].Sequence - 1 == i)
                        //                    ICDList[iIndex] += eandmDTO.EandMCodingICDList[j].Sequence;
                        //                else if (ICDList[iIndex].Split('~')[ICDList[iIndex].Split('~').Length - 1].Split(',').Length != eandmDTO.EandMCodingICDList[j].Sequence)
                        //                    ICDList[iIndex] += ",";
                        //            }
                        //        }
                        //    }
                    }
                    aryAccessICD.Add(eandmDTO.EandMCodingICDList[j].ICD);
                }
            }

            string EnableScreen = string.Empty;
            string btnDelete = "Resources/Delete-Blue.png";
            string btnDeleteAdditionalICD = "Resources/Delete-Blue.png";
            string sCurrentProcess = ClientSession.UserCurrentProcess;
            string sBatchStatus = string.Empty;
            string sOldBillingInstruction = string.Empty;

            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    EncRcrd = ClientSession.FillEncounterandWFObject.EncRecord;
                    sBatchStatus = EncRcrd.Batch_Status;
                    sOldBillingInstruction = EncRcrd.Billing_Instruction;
                }
            }

            //Jira #CAP-707
            //if ((sCurrentProcess != "ADDENDUM_CODING" && sCurrentProcess != "ADDENDUM_CODING_2" && sCurrentProcess != "ADDENDUM_CORRECTION" && sCurrentProcess != "ADDENDUM_PROCESS" && sCurrentProcess != "DICTATION_REVIEW" && sCurrentProcess != "MA_PROCESS" && sCurrentProcess != "MA_REVIEW" && sCurrentProcess != "CODER_REVIEW_CORRECTION" && sCurrentProcess != "PROVIDER_PROCESS" &&
            //   sCurrentProcess != "SCRIBE_CORRECTION" && sCurrentProcess != "SCRIBE_PROCESS" && sCurrentProcess != "READING_PROVIDER_PROCESS" && sCurrentProcess != "REVIEW_CODING" && sCurrentProcess != "REVIEW_CODING_2" && sCurrentProcess != "TECHNICIAN_PROCESS" && sCurrentProcess != "") || sBatchStatus == "CLOSED")
            //{
            if ((sCurrentProcess != "ADDENDUM_CODING" && sCurrentProcess != "ADDENDUM_CODING_2" && sCurrentProcess != "ADDENDUM_CORRECTION" && sCurrentProcess != "ADDENDUM_PROCESS" && sCurrentProcess != "DICTATION_REVIEW" && sCurrentProcess != "MA_PROCESS" && sCurrentProcess != "MA_REVIEW" && sCurrentProcess != "CODER_REVIEW_CORRECTION" && sCurrentProcess != "PROVIDER_PROCESS" &&
               sCurrentProcess != "SCRIBE_CORRECTION" && sCurrentProcess != "SCRIBE_PROCESS" && sCurrentProcess != "READING_PROVIDER_PROCESS" && sCurrentProcess != "REVIEW_CODING" && sCurrentProcess != "REVIEW_CODING_2" && sCurrentProcess != "TECHNICIAN_PROCESS" && sCurrentProcess != "AKIDO_SCRIBE_PROCESS" && sCurrentProcess != "AKIDO_REVIEW_CODING" && sCurrentProcess != "") || sBatchStatus == "CLOSED")
            {
                if (ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT")//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                {
                    EnableScreen = "disabled";
                    btnDelete = "Resources/Delete-Grey.png";
                    btnDeleteAdditionalICD = "Resources/Delete-Grey.png";
                }
            }
            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord != null && ClientSession.FillEncounterandWFObject.EncRecord.Id != 0)
                {
                    ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved = "Y";
                    IList<Encounter> lst = new List<Encounter>();
                    IList<Encounter> lsttemp = new List<Encounter>();
                    lst.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                    EncounterManager obj = new EncounterManager();
                    obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.FillEncounterandWFObject.EncRecord.Id, string.Empty);
                    ClientSession.FillEncounterandWFObject.EncRecord = lst[0];
                }
                else
                {
                    IList<Encounter> ilstEnc = objencManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                    if (ilstEnc.Count > 0)
                    {
                        EncRecord = ilstEnc[0];

                        EncRecord.is_serviceprocedure_saved = "Y";
                        IList<Encounter> lst = new List<Encounter>();
                        IList<Encounter> lsttemp = new List<Encounter>();
                        lst.Add(EncRecord);
                        EncounterManager obj = new EncounterManager();
                        obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, EncRecord.Id, string.Empty);
                        ClientSession.FillEncounterandWFObject.EncRecord = lst[0];
                    }
                }
            }
            /* Removed as per New Req. 
          string EnableScreenAdditionalICD = "disabled";
             */
            string EnablePriRbtn = "";
            if (!Is_CMG_Ancillary)
            {
                IList<string> AssPriCount = new List<string>();
                AssPriCount = ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT" && a.Split('~')[4] == "Pri").ToList<string>();
                if (ClientSession.UserRole.ToUpper() != "CODER" && ClientSession.UserRole.ToUpper() != "MEDICAL ASSISTANT" && ClientSession.UserRole.ToUpper() != "OFFICE MANAGER")//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
                {
                    EnablePriRbtn = "disabled";
                }
                if (AssPriCount != null && AssPriCount.Count > 0)
                {
                    EnablePriRbtn = "disabled";
                }
            }
            List<string> procedurecode = new List<string>();
            List<string> ProcedureList = new List<string>();
            procedurecode = ProcList.Select(a => a.ToString().Split('~')[0]).ToList<string>();
            IList<ProcedureCodeLibrary> lst1 = new List<ProcedureCodeLibrary>();
            ProcedureCodeLibraryManager obj1 = new ProcedureCodeLibraryManager();
            lst1 = obj1.GetProcedureList(procedurecode);
            //Cap - 1301
            IList<ProcedureModifierLookup> lstorder = new List<ProcedureModifierLookup>();
            ProcedureModifierLookupManager objorder = new ProcedureModifierLookupManager();
            lstorder = objorder.GetProcedureList(procedurecode);
            
            //for (int j = 0; j < lst1.Count; j++)
            //{
            //    for (int i = 0; i < ProcList.Count; i++)
            //    {
            //        if (ProcList[i].Contains(lst1[j].Procedure_Code.ToString()))
            //            ProcedureList.Add(ProcList[i].Split('~')[0] + "~" + ProcList[i].Split('~')[1] + "~" + ProcList[i].Split('~')[2] + "~" + ProcList[i].Split('~')[3] + "~" + ProcList[i].Split('~')[4] + "~" + ProcList[i].Split('~')[5] + "~" + ProcList[i].Split('~')[6] + "~" + ProcList[i].Split('~')[7] + "~" + ProcList[i].Split('~')[8] + "~" + ProcList[i].Split('~')[9] + "~" + lst1[j].Sort_Order + "~" + lst1[j].Procedure_Charge + "~" + ProcList[i].Split('~')[11] + "~" + ProcList[i].Split('~')[12] + "~" + ProcList[i].Split('~')[13] + "~" + ProcList[i].Split('~')[14] + "~" + ProcList[i].Split('~')[15] + "~" + ProcList[i].Split('~')[16]);
            //        //ProcedureList.Add(ProcList[i].Split('~')[0] + "~" + ProcList[i].Split('~')[1] + "~" + ProcList[i].Split('~')[2] + "~" + ProcList[i].Split('~')[3] + "~" + ProcList[i].Split('~')[4] + "~" + ProcList[i].Split('~')[5] + "~" + ProcList[i].Split('~')[6] + "~" + ProcList[i].Split('~')[7] + "~" + ProcList[i].Split('~')[8] + "~" + ProcList[i].Split('~')[9] + "~" + (Convert.ToInt32(j) + 1).ToString() + "~" + lst1[j].Procedure_Charge + "~" + ProcList[i].Split('~')[11] + "~" + ProcList[i].Split('~')[12] + "~" + ProcList[i].Split('~')[13] + "~" + ProcList[i].Split('~')[14] + "~" + ProcList[i].Split('~')[15] + "~" + ProcList[i].Split('~')[16]);
            //    }
            //}

            for (int j = 0; j < lst1.Count; j++)
            {
                for (int i = 0; i < ProcList.Count; i++)
                {

                    if (ProcList[i].Contains(lst1[j].Procedure_Code.ToString()))
                    {
                        IList<ProcedureModifierLookup> tempSort = (from m in lstorder where m.Procedure_Code == lst1[j].Procedure_Code.ToString() && m.Modifier == ProcList[i].Split('~')[4] select m).ToList<ProcedureModifierLookup>();
                        if (tempSort.Count > 0)
                        {
                            ProcedureList.Add(ProcList[i].Split('~')[0] + "~" + ProcList[i].Split('~')[1] + "~" + ProcList[i].Split('~')[2] + "~" + ProcList[i].Split('~')[3] + "~" + ProcList[i].Split('~')[4] + "~" + ProcList[i].Split('~')[5] + "~" + ProcList[i].Split('~')[6] + "~" + ProcList[i].Split('~')[7] + "~" + ProcList[i].Split('~')[8] + "~" + ProcList[i].Split('~')[9] + "~" + tempSort[0].Sort_Order + "~" + lst1[j].Procedure_Charge + "~" + ProcList[i].Split('~')[11] + "~" + ProcList[i].Split('~')[12] + "~" + ProcList[i].Split('~')[13] + "~" + ProcList[i].Split('~')[14] + "~" + ProcList[i].Split('~')[15] + "~" + ProcList[i].Split('~')[16] + "~" + tempSort[0].RVU);
                        }
                        else
                        {
                            tempSort = (from m in lstorder where m.Procedure_Code == lst1[j].Procedure_Code.ToString() && m.Modifier == string.Empty select m).ToList<ProcedureModifierLookup>();
                            if (tempSort.Count > 0)
                            {
                                ProcedureList.Add(ProcList[i].Split('~')[0] + "~" + ProcList[i].Split('~')[1] + "~" + ProcList[i].Split('~')[2] + "~" + ProcList[i].Split('~')[3] + "~" + ProcList[i].Split('~')[4] + "~" + ProcList[i].Split('~')[5] + "~" + ProcList[i].Split('~')[6] + "~" + ProcList[i].Split('~')[7] + "~" + ProcList[i].Split('~')[8] + "~" + ProcList[i].Split('~')[9] + "~" + tempSort[0].Sort_Order + "~" + lst1[j].Procedure_Charge + "~" + ProcList[i].Split('~')[11] + "~" + ProcList[i].Split('~')[12] + "~" + ProcList[i].Split('~')[13] + "~" + ProcList[i].Split('~')[14] + "~" + ProcList[i].Split('~')[15] + "~" + ProcList[i].Split('~')[16] + "~" + tempSort[0].RVU);
                            }
                            else
                            {
                                ProcedureList.Add(ProcList[i].Split('~')[0] + "~" + ProcList[i].Split('~')[1] + "~" + ProcList[i].Split('~')[2] + "~" + ProcList[i].Split('~')[3] + "~" + ProcList[i].Split('~')[4] + "~" + ProcList[i].Split('~')[5] + "~" + ProcList[i].Split('~')[6] + "~" + ProcList[i].Split('~')[7] + "~" + ProcList[i].Split('~')[8] + "~" + ProcList[i].Split('~')[9] + "~" + 9 + "~" + lst1[j].Procedure_Charge + "~" + ProcList[i].Split('~')[11] + "~" + ProcList[i].Split('~')[12] + "~" + ProcList[i].Split('~')[13] + "~" + ProcList[i].Split('~')[14] + "~" + ProcList[i].Split('~')[15] + "~" + ProcList[i].Split('~')[16] + "~" + 0);
                            }
                           
                        }
                    }//ProcedureList.Add(ProcList[i].Split('~')[0] + "~" + ProcList[i].Split('~')[1] + "~" + ProcList[i].Split('~')[2] + "~" + ProcList[i].Split('~')[3] + "~" + ProcList[i].Split('~')[4] + "~" + ProcList[i].Split('~')[5] + "~" + ProcList[i].Split('~')[6] + "~" + ProcList[i].Split('~')[7] + "~" + ProcList[i].Split('~')[8] + "~" + ProcList[i].Split('~')[9] + "~" + (Convert.ToInt32(j) + 1).ToString() + "~" + lst1[j].Procedure_Charge + "~" + ProcList[i].Split('~')[11] + "~" + ProcList[i].Split('~')[12] + "~" + ProcList[i].Split('~')[13] + "~" + ProcList[i].Split('~')[14] + "~" + ProcList[i].Split('~')[15] + "~" + ProcList[i].Split('~')[16]);
                }
            }


            //, DiaPointer1 = a.Split('~')[10], DiaPointer2 = a.Split('~')[11], DiaPointer3 = a.Split('~')[12], DiaPointer4 = a.Split('~')[13], DiaPointer5 = a.Split('~')[14], DiaPointer6 = a.Split('~')[15] });
            //Cap - 1301
            //var EandMCodingListCPT = ProcedureList.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1], EandMCPTID = a.Split('~')[2], Units = a.Split('~')[3], Modifier1 = a.Split('~')[4], Modifier2 = a.Split('~')[5], Modifier3 = a.Split('~')[6], Modifier4 = a.Split('~')[7], CPTCheck = a.Split('~')[8], EnableScreen = EnableScreen, CPTVersion = a.Split('~')[9], btnDelete = btnDelete, Order = Convert.ToInt32(a.Split('~')[10]), Amount = a.Split('~')[11], DiaPointer1 = a.Split('~')[12], DiaPointer2 = a.Split('~')[13], DiaPointer3 = a.Split('~')[14], DiaPointer4 = a.Split('~')[15], DiaPointer5 = a.Split('~')[16], DiaPointer6 = a.Split('~')[17] });  
            var EandMCodingListCPT = ProcedureList.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1], EandMCPTID = a.Split('~')[2], Units = a.Split('~')[3], Modifier1 = a.Split('~')[4], Modifier2 = a.Split('~')[5], Modifier3 = a.Split('~')[6], Modifier4 = a.Split('~')[7], CPTCheck = a.Split('~')[8], EnableScreen = EnableScreen, CPTVersion = a.Split('~')[9], btnDelete = btnDelete, Order = Convert.ToInt32(a.Split('~')[10]), RVU = Convert.ToDouble(a.Split('~')[18]), Amount = a.Split('~')[11], DiaPointer1 = a.Split('~')[12], DiaPointer2 = a.Split('~')[13], DiaPointer3 = a.Split('~')[14], DiaPointer4 = a.Split('~')[15], DiaPointer5 = a.Split('~')[16], DiaPointer6 = a.Split('~')[17] }).OrderBy(c => c.Order).ThenByDescending(n => n.RVU).ThenBy(o => o.CPTCode); 
            var EandMCodingListICD = ICDList.Where(a => a.Split('~')[0] == "EMICD").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], btnDelete = btnDeleteAdditionalICD, EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], IsPrimary = a.Split('~')[4], EnableScreen = EnableScreen, EnablePriRbtn = EnablePriRbtn });
            var AssEandMCodingListICD = ICDList.Where(a => a.Split('~')[0] == "ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
            var OrdersAssEandMCodingListICD = ICDList.Where(a => a.Split('~')[0] == "ORDERS_ASSESSMENT").Select(a => new { ICDCode = a.Split('~')[1], ICDDescription = a.Split('~')[2], ICDVersion = a.Split('~')[3], IsPrimary = a.Split('~')[4], EandMICDID = a.Split('~')[5], Sequence = a.Split('~')[6], ResultCheck = a.Split('~')[7], EnableScreen = EnableScreen });
            HttpContext.Current.Session["EandMList"] = eandmDTO.EandMCodingList;
            HttpContext.Current.Session["EandMICDList"] = eandmDTO.EandMCodingICDList;
            HttpContext.Current.Session["EnablePriRbtn"] = EnablePriRbtn;


            //SubmitESuperBill /* Start For Git Lab Id: 1666 */
            if (sESuperbillSubmitted == "Y" && arylstICD.Count() > 0)
            {
                Encounter EncRecordEsuperBill = new Encounter();
                EncounterManager ObjEncManager = new EncounterManager();
                Boolean bCheck = ObjEncManager.CheckBillableCodes(ClientSession.EncounterId);
                if (bCheck == false)
                {
                    //   return JsonConvert.SerializeObject("530024");
                    var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, BillingInstruction = sOldBillingInstruction, EnablePriRbtn = EnablePriRbtn, IsBillableNo = "530024" };
                    return JsonConvert.SerializeObject(result);
                }

                if (ClientSession.FillEncounterandWFObject != null)
                {
                    if (ClientSession.FillEncounterandWFObject.EncRecord == null || ClientSession.FillEncounterandWFObject.EncRecord.Id == 0)
                    {
                        IList<Encounter> ilstEnc = ObjEncManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                        if (ilstEnc.Count > 0)
                        {
                            EncRecordEsuperBill = ilstEnc[0];

                            EncRecordEsuperBill.Local_Time = UtilityManager.ConvertToLocal(EncRecordEsuperBill.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                            IList<Encounter> EncUpdateList = new List<Encounter>();
                            EncRecordEsuperBill.Is_EandM_Submitted = "Y";
                            EncRecordEsuperBill.E_M_Submitted_Date_And_Time = DateTime.Now;
                            //EncRecord.Batch_Status = "MODIFIED";//Commented by naveena For Submitting esuper bill second time
                            if (EncRecordEsuperBill.Is_EandM_Submitted == "Y")
                            {
                                if (EncRecordEsuperBill.Batch_Status.ToUpper() == "CLOSED")
                                    EncRecordEsuperBill.Batch_Status = "REOPEN";
                                else
                                {
                                    if (EncRecordEsuperBill.Batch_Status.ToUpper() != "REOPEN")//For BUg Id 52918 
                                        EncRecordEsuperBill.Batch_Status = "MODIFIED";
                                }
                            }
                            EncUpdateList.Add(EncRecordEsuperBill);
                            EncRecordEsuperBill = ObjEncManager.UpdateE_SuperBill(EncUpdateList, string.Empty);
                        }
                    }
                    else if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                    {
                        ClientSession.FillEncounterandWFObject.EncRecord.Local_Time = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                        EncRecordEsuperBill = ClientSession.FillEncounterandWFObject.EncRecord;
                        IList<Encounter> EncUpdateList = new List<Encounter>();
                        EncRecordEsuperBill.Is_EandM_Submitted = "Y";
                        EncRecordEsuperBill.E_M_Submitted_Date_And_Time = DateTime.Now;
                        //EncRecord.Batch_Status = "MODIFIED";//Commented by naveena For Submitting esuper bill second time
                        if (EncRecordEsuperBill.Is_EandM_Submitted == "Y")
                        {
                            if (EncRecordEsuperBill.Batch_Status.ToUpper() == "CLOSED")
                                EncRecordEsuperBill.Batch_Status = "REOPEN";
                            else
                            {
                                if (EncRecordEsuperBill.Batch_Status.ToUpper() != "REOPEN")//For BUg Id 52918 
                                    EncRecordEsuperBill.Batch_Status = "MODIFIED";
                            }
                        }
                        EncUpdateList.Add(EncRecordEsuperBill);
                        ClientSession.FillEncounterandWFObject.EncRecord = ObjEncManager.UpdateE_SuperBill(EncUpdateList, string.Empty);
                    }
                }
            }

            /*End For Git Lab Id: 1666*/
            if (!Is_CMG_Ancillary)
            {
                var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = AssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, BillingInstruction = sOldBillingInstruction, EnablePriRbtn = EnablePriRbtn, IsBillableNo = isZcode };
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                var result = new { ProcedureList = EandMCodingListCPT, ICDList = EandMCodingListICD, AssICDlist = OrdersAssEandMCodingListICD, UserRole = ClientSession.UserRole, EnableScreen = EnableScreen, BillingInstruction = sOldBillingInstruction, EnablePriRbtn = EnablePriRbtn, IsBillableNo = isZcode };
                return JsonConvert.SerializeObject(result);

            }


        }

        [WebMethod(EnableSession = true)]
        public string ESuperbillSubmitted(string sESuperbillSubmitted)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            Encounter EncRecord = new Encounter();
            EncounterManager ObjEncManager = new EncounterManager();


            Boolean bCheck = ObjEncManager.CheckBillableCodes(ClientSession.EncounterId);
            if (bCheck == false)
                return JsonConvert.SerializeObject("530024");

            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord == null || ClientSession.FillEncounterandWFObject.EncRecord.Id == 0)
                {
                    IList<Encounter> ilstEnc = ObjEncManager.GetEncounterByEncounterID(ClientSession.EncounterId);
                    if (ilstEnc.Count > 0)
                    {
                        EncRecord = ilstEnc[0];

                        EncRecord.Local_Time = UtilityManager.ConvertToLocal(EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                        IList<Encounter> EncUpdateList = new List<Encounter>();
                        EncRecord.Is_EandM_Submitted = "Y";
                        EncRecord.E_M_Submitted_Date_And_Time = DateTime.Now;
                        //EncRecord.Batch_Status = "MODIFIED";//Commented by naveena For Submitting esuper bill second time
                        if (EncRecord.Is_EandM_Submitted == "Y")
                        {
                            if (EncRecord.Batch_Status.ToUpper() == "CLOSED")
                                EncRecord.Batch_Status = "REOPEN";
                            else
                            {
                                if (EncRecord.Batch_Status.ToUpper() != "REOPEN")//For BUg Id 52918 
                                    EncRecord.Batch_Status = "MODIFIED";
                            }
                        }
                        EncUpdateList.Add(EncRecord);
                        EncRecord = ObjEncManager.UpdateE_SuperBill(EncUpdateList, string.Empty);
                    }
                }
                else if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    ClientSession.FillEncounterandWFObject.EncRecord.Local_Time = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).ToString("yyyy-MM-dd hh:mm:ss tt");
                    EncRecord = ClientSession.FillEncounterandWFObject.EncRecord;
                    IList<Encounter> EncUpdateList = new List<Encounter>();
                    EncRecord.Is_EandM_Submitted = "Y";
                    EncRecord.E_M_Submitted_Date_And_Time = DateTime.Now;
                    //EncRecord.Batch_Status = "MODIFIED";//Commented by naveena For Submitting esuper bill second time
                    if (EncRecord.Is_EandM_Submitted == "Y")
                    {
                        if (EncRecord.Batch_Status.ToUpper() == "CLOSED")
                            EncRecord.Batch_Status = "REOPEN";
                        else
                        {
                            if (EncRecord.Batch_Status.ToUpper() != "REOPEN")//For BUg Id 52918 
                                EncRecord.Batch_Status = "MODIFIED";
                        }
                    }
                    EncUpdateList.Add(EncRecord);
                    ClientSession.FillEncounterandWFObject.EncRecord = ObjEncManager.UpdateE_SuperBill(EncUpdateList, string.Empty);
                }
            }
            var EnableScreen = "disabled";
            var btnDelete = "Resources/Delete-Grey.png";
            var result = new { EnableScreen = EnableScreen, btnDelete = btnDelete };
            return JsonConvert.SerializeObject(result);
        }

        [WebMethod(EnableSession = true)]
        public string FillTypeOfVisit()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string povphysicianId = "P" + ClientSession.PhysicianId.ToString();
            XmlDocument xmldocUser = new XmlDocument();
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + @"ConfigXML\Physician_POV.xml"))
                xmldocUser.Load(HttpContext.Current.Request.PhysicalApplicationPath + @"ConfigXML\Physician_POV.xml");
            XmlNodeList xmlUserList = xmldocUser.GetElementsByTagName(povphysicianId);
            ArrayList _temp = new ArrayList();
            string[] type_of_visit = new string[xmlUserList.Count];
            int index = 0;
            if (xmlUserList.Count > 0)
            {
                foreach (XmlNode item in xmlUserList)
                {
                    if (item.Attributes[5].Value == ClientSession.LegalOrg)
                    {

                        type_of_visit[index] = item.Attributes[0].Value.ToUpper();
                    }
                    index++;
                }
            }
            _temp.Add(type_of_visit);
            _temp.Add(ClientSession.FillEncounterandWFObject.EncRecord.Visit_Type);
            return JsonConvert.SerializeObject(_temp);
        }

        public void GetSourceDeleteList(IList<string> CPT_ImmDelcode, IList<string> CPT_ICPDelcode, out ImmunizationDTO ImmDTO, out CarePlan tobacco_CarePlan)
        {
            ImmDTO = new ImmunizationDTO();
            tobacco_CarePlan = new CarePlan();
            IList<string> ICPtobacco_CodesList = new List<string>();
            #region Immunization
            if (CPT_ImmDelcode != null && CPT_ImmDelcode.Count > 0)
            {

                IList<string> ilstEandMTagList = new List<string>();
                ilstEandMTagList.Add("ImmunizationList");
                ilstEandMTagList.Add("ImmunizationHistoryList");



                IList<object> ilstEandMBlobFinal = new List<object>();
                ilstEandMBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstEandMTagList);
                IList<Immunization> lstimm = new List<Immunization>();
                IList<ImmunizationHistory> lstimmhis = new List<ImmunizationHistory>();
                if (ilstEandMBlobFinal != null && ilstEandMBlobFinal.Count > 0)
                {
                    if (ilstEandMBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[0]).Count; iCount++)
                        {
                            //GitLab #3646
                            if (Convert.ToUInt64(((Immunization)((IList<object>)ilstEandMBlobFinal[0])[iCount]).Encounter_Id) == ClientSession.EncounterId)
                            {
                                lstimm.Add((Immunization)((IList<object>)ilstEandMBlobFinal[0])[iCount]);

                                //Jira #CAP-107
                                if (CPT_ImmDelcode.IndexOf(((Immunization)((IList<object>)ilstEandMBlobFinal[0])[iCount]).Procedure_Code.Trim()) != -1)
                                {
                                    ImmDTO.Immunization.Add((Immunization)((IList<object>)ilstEandMBlobFinal[0])[iCount]);
                                }
                            }
                        }
                        //Jira #CAP-107
                        //ImmDTO.Immunization = lstimm.Where(a => !CPT_ImmDelcode.Contains(a.Procedure_Code)).ToList<Immunization>();

                    }
                    if (ilstEandMBlobFinal[1] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstEandMBlobFinal[1]).Count; iCount++)
                        {
                            //GitLab #3646
                            if (Convert.ToUInt64(((ImmunizationHistory)((IList<object>)ilstEandMBlobFinal[1])[iCount]).Encounter_ID) == ClientSession.EncounterId)
                            {
                                lstimmhis.Add((ImmunizationHistory)((IList<object>)ilstEandMBlobFinal[1])[iCount]);


                                if (CPT_ImmDelcode.IndexOf(((ImmunizationHistory)((IList<object>)ilstEandMBlobFinal[1])[iCount]).Procedure_Code.Trim()) != -1)
                                {
                                    ImmDTO.ImmunizationHistoryList.Add(((ImmunizationHistory)((IList<object>)ilstEandMBlobFinal[1])[iCount]));
                                }
                            }

                        }
                        //if (CPT_ImmDelcode.IndexOf(ImmunizationHistory.Procedure_Code.Trim()) != -1)
                        //{
                        //    ImmDTO.ImmunizationHistoryList.Add(ImmunizationHistory);
                        //}
                        //ImmDTO.ImmunizationHistoryList = lstimmhis.Where(a => !CPT_ImmDelcode.Contains(a.Procedure_Code)).ToList<ImmunizationHistory>();

                    }
                }
                //        string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //if (File.Exists(strXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                //    XmlNodeList xmlTagName = null;
                //    // itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                //if (itemDoc.GetElementsByTagName("ImmunizationList") != null)
                //{
                //    if (itemDoc.GetElementsByTagName("ImmunizationList").Count > 0)
                //    {
                //        xmlTagName = itemDoc.GetElementsByTagName("ImmunizationList")[0].ChildNodes;

                //        if (xmlTagName.Count > 0)
                //        {
                //            for (int j = 0; j < xmlTagName.Count; j++)
                //            {
                //                if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_Id").Value) == ClientSession.EncounterId)
                //                {

                //                    string TagName = xmlTagName[j].Name;
                //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Immunization));
                //                    Immunization Immunization = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Immunization;
                //                    IEnumerable<PropertyInfo> propInfo = null;
                //                    if (Immunization != null)
                //                    {
                //                        propInfo = from obji in ((Immunization)Immunization).GetType().GetProperties() select obji;

                //                        for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                        {
                //                            XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                            {
                //                                foreach (PropertyInfo property in propInfo)
                //                                {
                //                                    if (property.Name == nodevalue.Name)
                //                                    {
                //                                        if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                            property.SetValue(Immunization, Convert.ToUInt64(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                            property.SetValue(Immunization, Convert.ToString(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                            property.SetValue(Immunization, Convert.ToDateTime(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                            property.SetValue(Immunization, Convert.ToInt32(nodevalue.Value), null);
                //                                        else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                            property.SetValue(Immunization, Convert.ToDecimal(nodevalue.Value), null);
                //                                        else
                //                                            property.SetValue(Immunization, nodevalue.Value, null);
                //                                    }
                //                                }
                //                            }
                //                        }
                //                        if (CPT_ImmDelcode.IndexOf(Immunization.Procedure_Code.Trim()) != -1)
                //                        {
                //                            ImmDTO.Immunization.Add(Immunization);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //if (itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0] != null)
                //{
                //    xmlTagName = itemDoc.GetElementsByTagName("ImmunizationHistoryList")[0].ChildNodes;

                //    if (xmlTagName.Count > 0)
                //    {
                //        for (int j = 0; j < xmlTagName.Count; j++)
                //        {
                //            if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == ClientSession.EncounterId)
                //            {

                //                string TagName = xmlTagName[j].Name;
                //                XmlSerializer xmlserializer = new XmlSerializer(typeof(ImmunizationHistory));
                //                ImmunizationHistory ImmunizationHistory = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ImmunizationHistory;
                //                IEnumerable<PropertyInfo> propInfo = null;
                //                propInfo = from obji in ((ImmunizationHistory)ImmunizationHistory).GetType().GetProperties() select obji;

                //                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                {
                //                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                    {
                //                        foreach (PropertyInfo property in propInfo)
                //                        {
                //                            if (property.Name == nodevalue.Name)
                //                            {
                //                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                    property.SetValue(ImmunizationHistory, Convert.ToUInt64(nodevalue.Value), null);
                //                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                    property.SetValue(ImmunizationHistory, Convert.ToString(nodevalue.Value), null);
                //                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                    property.SetValue(ImmunizationHistory, Convert.ToDateTime(nodevalue.Value), null);
                //                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                    property.SetValue(ImmunizationHistory, Convert.ToInt32(nodevalue.Value), null);
                //                                else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                    property.SetValue(ImmunizationHistory, Convert.ToDecimal(nodevalue.Value), null);
                //                                else
                //                                    property.SetValue(ImmunizationHistory, nodevalue.Value, null);
                //                            }
                //                        }
                //                    }
                //                }
                //                if (CPT_ImmDelcode.IndexOf(ImmunizationHistory.Procedure_Code.Trim()) != -1)
                //                {
                //                    ImmDTO.ImmunizationHistoryList.Add(ImmunizationHistory);
                //                }
                //            }

                //        }
                //    }
                //}
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}
            }

            #endregion
            //Updation of Care_Plan on deletion of CPTs "99406","99407" - BugID:47386
            #region ICP
            if (CPT_ICPDelcode != null && CPT_ICPDelcode.Count > 0)
            {

                IList<string> ilstcareTagList = new List<string>();
                ilstcareTagList.Add("CarePlanList");



                IList<object> ilstcareplanBlobFinal = new List<object>();
                ilstcareplanBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilstcareTagList);
                IList<CarePlan> lstcareplan = new List<CarePlan>();

                if (ilstcareplanBlobFinal != null && ilstcareplanBlobFinal.Count > 0)
                {
                    if (ilstcareplanBlobFinal[0] != null)
                    {
                        for (int iCount = 0; iCount < ((IList<object>)ilstcareplanBlobFinal[0]).Count; iCount++)
                        {
                            lstcareplan.Add((CarePlan)((IList<object>)ilstcareplanBlobFinal[0])[iCount]);


                        }
                        lstcareplan = (from m in lstcareplan
                                       where m.Encounter_ID == ClientSession.EncounterId &&
                                      m.Care_Name == "Screening Tests" && m.Care_Name_Value == "Preventive Care and Screening: Tobacco Use: Screening and Cessation Intervention" && m.Snomed_Code != ""
                                       select m).ToList<CarePlan>();



                        if (lstcareplan.Count > 0)
                        {
                            ICPtobacco_CodesList = lstcareplan[0].Snomed_Code.ToString().Split(',');
                            foreach (string s in CPT_ICPDelcode)
                            {
                                if (ICPtobacco_CodesList.IndexOf(s) > -1)
                                {
                                    tobacco_CarePlan = lstcareplan[0];
                                }
                            }
                        }
                        // ImmDTO.Immunization = lstimm.Where(a => !CPT_ImmDelcode.Contains(a.Procedure_Code)).ToList<Immunization>();

                    }
                }
                //string FileName_Enc = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                // string strXmlFilePath_Enc = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName_Enc);
                //if (File.Exists(strXmlFilePath_Enc) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath_Enc);
                //    XmlNodeList xmlTagName = null;
                //    //itemDoc.Load(XmlText);
                //    using (FileStream fs = new FileStream(strXmlFilePath_Enc, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        itemDoc.Load(fs);

                //        XmlText.Close();
                //        if (itemDoc.GetElementsByTagName("CarePlanList") != null)
                //        {
                //            if (itemDoc.GetElementsByTagName("CarePlanList").Count > 0)
                //            {
                //                xmlTagName = itemDoc.GetElementsByTagName("CarePlanList")[0].ChildNodes;

                //                if (xmlTagName.Count > 0)
                //                {
                //                    for (int j = 0; j < xmlTagName.Count; j++)
                //                    {
                //                        if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Encounter_ID").Value) == ClientSession.EncounterId && xmlTagName[j].Attributes.GetNamedItem("Care_Name").Value == "Screening Tests" && xmlTagName[j].Attributes.GetNamedItem("Care_Name_Value").Value == "Preventive Care and Screening: Tobacco Use: Screening and Cessation Intervention" && xmlTagName[j].Attributes.GetNamedItem("Snomed_Code").Value.Trim() != "")
                //                        {
                //                            string TagName = xmlTagName[j].Name;
                //                            XmlSerializer xmlserializer = new XmlSerializer(typeof(CarePlan));
                //                            CarePlan care_plan = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as CarePlan;
                //                            IEnumerable<PropertyInfo> propInfo = null;
                //                            if (care_plan != null)
                //                            {
                //                                propInfo = from obji in ((CarePlan)care_plan).GetType().GetProperties() select obji;

                //                                for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
                //                                {
                //                                    XmlNode nodevalue = xmlTagName[j].Attributes[i];
                //                                    {
                //                                        foreach (PropertyInfo property in propInfo)
                //                                        {
                //                                            if (property.Name == nodevalue.Name)
                //                                            {
                //                                                if (property.PropertyType.Name.ToUpper() == "UINT64")
                //                                                    property.SetValue(care_plan, Convert.ToUInt64(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "STRING")
                //                                                    property.SetValue(care_plan, Convert.ToString(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "DATETIME")
                //                                                    property.SetValue(care_plan, Convert.ToDateTime(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "INT32")
                //                                                    property.SetValue(care_plan, Convert.ToInt32(nodevalue.Value), null);
                //                                                else if (property.PropertyType.Name.ToUpper() == "DECIMAL")
                //                                                    property.SetValue(care_plan, Convert.ToDecimal(nodevalue.Value), null);
                //                                                else
                //                                                    property.SetValue(care_plan, nodevalue.Value, null);
                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                                ICPtobacco_CodesList = care_plan.Snomed_Code.ToString().Split(',');
                //                                foreach (string s in CPT_ICPDelcode)
                //                                {
                //                                    if (ICPtobacco_CodesList.IndexOf(s) > -1)
                //                                    {
                //                                        tobacco_CarePlan = care_plan;
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        fs.Close();
                //        fs.Dispose();
                //    }
                //}

                if (tobacco_CarePlan != null && Convert.ToString(tobacco_CarePlan.Id) != "0")
                {
                    #region Static_Lookup
                    string strXmlFilePath_StaticLp = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                    IDictionary<string, string> idic = new Dictionary<string, string>();
                    if (File.Exists(strXmlFilePath_StaticLp) == true)
                    {
                        XDocument xdoc = XDocument.Load(strXmlFilePath_StaticLp);
                        IEnumerable<XElement> Xele = xdoc.Descendants("root").Descendants("FollowupList").Elements("Followup").Where(x => x.Attribute("Field_Name").Value.ToString() == "FOLLOWUP FOR PREVENTIVE CARE AND SCREENING: TOBACCO USE: SCREENING AND CESSATION INTERVENTION");
                        foreach (XElement ele in Xele)
                        {
                            if (ICPtobacco_CodesList.IndexOf(ele.Attribute("Description").Value.ToString()) > -1)
                            {
                                idic.Add(ele.Attribute("Description").Value.ToString().Trim(), ele.Attribute("Value").Value.ToString().Trim());
                            }
                        }
                    }
                    #endregion
                    foreach (string s in CPT_ICPDelcode)
                    {
                        tobacco_CarePlan.Snomed_Code = tobacco_CarePlan.Snomed_Code.Replace(s.Trim(), "").Replace(",,", ",");
                        tobacco_CarePlan.Care_Plan_Notes = tobacco_CarePlan.Care_Plan_Notes.Replace(idic[s].Trim(), "").Replace(",,", ",");
                        if (tobacco_CarePlan.Snomed_Code.Trim() != string.Empty && tobacco_CarePlan.Snomed_Code.Length > 0)
                        {
                            if (tobacco_CarePlan.Snomed_Code.EndsWith(","))
                                tobacco_CarePlan.Snomed_Code = tobacco_CarePlan.Snomed_Code.Remove(tobacco_CarePlan.Snomed_Code.LastIndexOf(","), 1);
                            if (tobacco_CarePlan.Snomed_Code.StartsWith(","))
                                tobacco_CarePlan.Snomed_Code = tobacco_CarePlan.Snomed_Code.Remove(tobacco_CarePlan.Snomed_Code.IndexOf(","), 1);
                        }
                        if (tobacco_CarePlan.Care_Plan_Notes.Trim() != string.Empty && tobacco_CarePlan.Care_Plan_Notes.Length > 0)
                        {
                            if (tobacco_CarePlan.Care_Plan_Notes.EndsWith(","))
                                tobacco_CarePlan.Care_Plan_Notes = tobacco_CarePlan.Care_Plan_Notes.Remove(tobacco_CarePlan.Care_Plan_Notes.LastIndexOf(","), 1);
                            if (tobacco_CarePlan.Care_Plan_Notes.StartsWith(","))
                                tobacco_CarePlan.Care_Plan_Notes = tobacco_CarePlan.Care_Plan_Notes.Remove(tobacco_CarePlan.Care_Plan_Notes.IndexOf(","), 1);
                        }
                    }
                }
            }
        }
        #endregion
    

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string SearchICDDescrptionAuthText(string text)
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
        string sTodate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");

        if (sTodate == "0001-01-01")
            sTodate = DateTime.Now.ToString("yyyy-MM-dd");

        string[] ResultDescpList = objProcedureCodeLibraryMngr.GetCPTDescriptionList(data, type, sTodate).ToArray();
        var jsonString = JsonConvert.SerializeObject(ResultDescpList);
        return jsonString;
    }

    [WebMethod(EnableSession = true)]
    public string GetFormviewCPT(string sFormviewCPT)
    {
        if (ClientSession.UserName == string.Empty)
        {
            HttpContext.Current.Response.StatusCode = 999;
            HttpContext.Current.Response.Status = "999 Session Expired";
            HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
            return "Session Expired";
        }
        IList<ProcedureCodeLibrary> ProcedureList = new List<ProcedureCodeLibrary>();
        string sProcedureList = string.Empty;
        string json = string.Empty;
        if (sFormviewCPT != string.Empty)
        {
            var Record1 = (from b in sFormviewCPT.Split('|').ToArray() where !ProcedureList.Any(a => a.Procedure_Code == b.Split('~')[0]) select b).ToList();
            if (Record1.Count > 0)
            {
                var ResultRecord1 = Record1.Select(a => new { CPTCode = a.Split('~')[0], CPTDesc = a.Split('~')[1].Replace('$', '\"'), btnDelete = "Resources/Delete-Blue.png", Units = "1", CPTCheck = "6", Order = a.Split('~')[2] });
                json = new JavaScriptSerializer().Serialize(ResultRecord1);
            }
        }

        return "{\"ListofCPTs\" :" + json + "}";
    }

    public Boolean CheckGcodes(IList<EandMCodingICD> eandmICDList, IList<EAndMCoding> EandMCodingList)

    {
        bool IsGcodePresent = false; ;
        bool bGCodeCheck = true; ;
        bool IsICDPresent = false;
        IDictionary<string, IList<string>> CPTICD = new Dictionary<string, IList<string>>();
        string sE_And_M_CPT_And_ICD = System.Configuration.ConfigurationSettings.AppSettings["E_And_M_CPT_And_ICD"].ToString();
        string sPlan = "";// System.Configuration.ConfigurationSettings.AppSettings["Primary_Plan"].ToString();
        if (sE_And_M_CPT_And_ICD != "")
        {
            string[] sVal = sE_And_M_CPT_And_ICD.Split('$');
            if (sVal.Count() > 0)
            {
                for (int i = 0; i < sVal.Count(); i++)
                {
                    CPTICD.Add(sVal[i].Split('|')[0].ToString(), sVal[i].Split('|').Skip(1).ToList());
                }
            }
        }
        var GcodeList = from g in EandMCodingList where (CPTICD.Any(c => g.Procedure_Code.Contains(c.Key))) select new { ID = g.Id, Procedure_Code = g.Procedure_Code };

        var GcodeList_New = from g in EandMCodingList where (CPTICD.Any(c => g.Procedure_Code.Contains(c.Key))) select g.Procedure_Code;

        var Procedure_code = CPTICD.Where(a => GcodeList_New.Any(b => b.ToString() == a.Key)).Select(r => r);
        //End

        //var GcodeList = from g in objMoveVerifyDTO.EandMCodingList where (g.Procedure_Code == "G0438" || g.Procedure_Code == "G0439") select g.Id;
        //if (GcodeList_New.Count()==0)
        //{
        //    objMoveVerifyDTO.CPT = CPTICD.Select(p => p.Key).ToList<string>();
        //}
        IList<string> ilstICD = new List<string>();
        IList<string> GcodeCheckList = new List<string>();

        if (GcodeList.Count() > 0)
        {
            IsGcodePresent = true;
            for (int i = 0; i < GcodeList.Count(); i++)
            {
                //eandmICDList = emICDMngr.EandMcodingList(Convert.ToUInt32(GcodeList.ElementAt(i)));

                if (eandmICDList.Count > 0)
                {
                    // GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "V70.0").ToString());
                    //GcodeCheckList.Add(eandmICDList.Any(a => a.ICD == "Z00.00").ToString());
                    //GcodeCheckList.Add(eandmICDList.Any(a => CPTICD.Any(z => z.Value.Contains(a.ICD))).ToString());
                    GcodeCheckList.Add(eandmICDList.Any(a => Procedure_code.Any(z => z.Key == (GcodeList.ElementAt(i).Procedure_Code) && z.Value.Contains(a.ICD.Trim()))).ToString());
                    if (GcodeCheckList[i].ToString().ToUpper() == "FALSE")
                    {
                        //var icd = string.Join(" or ", (Procedure_code.ElementAt(i).Value).Select(g => g.ToString()).ToArray());
                        var test = (from r in Procedure_code
                                    where r.Key == GcodeList.ElementAt(i).Procedure_Code

                                    select r.Value).ToArray();


                        if (test.Count() > 0)
                        {
                            var icd = string.Join(" or ", test[0].Select(g => g.ToString()).ToArray());
                            ilstICD.Add(icd.ToString() + '$' + GcodeList.ElementAt(i).Procedure_Code);
                        }
                    }
                }

            }

            if (GcodeCheckList.Count > 0)
            {
                IsICDPresent = GcodeCheckList.Contains("False") == true ? false : true;
            }

            if (Convert.ToBoolean(IsGcodePresent) == true && Convert.ToBoolean(IsICDPresent) == false)
            {
                bGCodeCheck = false;
            }
            else
            {
                bGCodeCheck = true;
            }
        }
        return bGCodeCheck;
    }
}
}
