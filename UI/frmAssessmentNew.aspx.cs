using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Configuration;

namespace Acurus.Capella.UI
{
    public partial class frmAssessmentNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        static int isval = 0;
        static IDictionary<string, IList<AllICDColorCoding>> temp = new Dictionary<string, IList<AllICDColorCoding>>();

        public class VitalsAssesment
        {
            public string ICD_9;
            public string Description;
            public string Primary_Diagnosis;
            public ulong ProblemListId;
            public string Notes;

            public ulong AssessmentID;
            public int iVersion;
            public int iProblemListVersion;
            public string CheckBoxCheck;
            public string StatusSelected;
            public string ICD9Code;
            public string ICD9Desc;
            public string sCreatedBy;
            public string sCreatedDateTime;
        }


        public class AllICDColorCoding
        {
            public string ICD_Codes;
            public string Leaf_Node;
            public string Mutually_Exclusive;
            public string Parent_ICD;
            public string currentLeaf_Node;
            public string currentMutually_Exclusive;
        }


        [WebMethod(EnableSession = true)]
        public static string GetFavouriteICDS()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string json = "";
            PhysicianICD_9Manager phyICd9Mngr = new PhysicianICD_9Manager();
            IList<PhysicianICD_9> phyICD9List = phyICd9Mngr.GetPhyICDsandCategory(ClientSession.PhysicianId, ClientSession.LegalOrg);
            if (phyICD9List != null)
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
        public static string LoadAssessmentTable(string strAssessment)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HttpContext.Current.Session["Is_Assessment_CopyPrevious"] = "No";
            AssessmentManager objAssessmentManager = new AssessmentManager();
            FillAssessment assessmentLoadList = new FillAssessment();
            AllICD_9Manager objAllIcdMgr = new AllICD_9Manager();
            IList<ProblemList> pblmMedList = new List<ProblemList>();
            IList<AllICD_9> allICD9ForVitalsProblemListPFSH = new List<AllICD_9>();
            IList<string> strICDDesc = new List<string>();
            IList<string> strICD9CodeDesc = new List<string>();

            IList<string> strICD910Code = new List<string>();
            IList<VitalsAssesment> vitalsproblemList = new List<VitalsAssesment>();

            //BugID:49118
            #region AssessmentStatusLoad
            IDictionary<string, string> DefaultStatusList = new Dictionary<string, string>();
            IList<string> Statuslst = new List<string>();
            XmlDocument xml_doc = new XmlDocument();
            bool Default_Ass_Status = false;
            if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\staticlookup.xml"))
            {
                xml_doc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\staticlookup.xml");
                XmlNodeList xml_nodelst = xml_doc.GetElementsByTagName("AssessmentStatus");
                if (xml_nodelst != null && xml_nodelst.Count > 0)
                {
                    foreach (XmlNode xml_node in xml_nodelst)
                    {
                        if (xml_node.Attributes.GetNamedItem("is_required").Value.ToUpper() == "YES")
                            Statuslst.Add(xml_node.Attributes.GetNamedItem("value").Value);
                    }
                }

                XmlNodeList xml_nodeSetDefault = xml_doc.GetElementsByTagName("AssessmentStatusDefaulted");
                if (xml_nodeSetDefault != null && xml_nodeSetDefault.Count > 0)
                {
                    if (xml_nodeSetDefault[0].Attributes.GetNamedItem("value").Value.ToUpper() == "YES")
                        Default_Ass_Status = true;
                }
                if (Default_Ass_Status)
                {
                    XmlNodeList xml_nodelstDefaultStatus = xml_doc.GetElementsByTagName("AssessmentStatusDefault");
                    if (xml_nodelstDefaultStatus != null && xml_nodelstDefaultStatus.Count > 0)
                    {
                        foreach (XmlNode xml_node in xml_nodelstDefaultStatus)
                        {
                            DefaultStatusList.Add(xml_node.Attributes.GetNamedItem("Name").Value, xml_node.Attributes.GetNamedItem("value").Value);
                        }
                    }
                }
                HttpContext.Current.Session["DefaultStatusList"] = DefaultStatusList;
            }
            #endregion

            string Ass_Status = string.Empty, Other_Status = string.Empty;
            bool bSuggestIcds = true;
            IDictionary<string, string> idicDefaultStatuslst = (Dictionary<string, string>)HttpContext.Current.Session["DefaultStatusList"];
            if (idicDefaultStatuslst != null && idicDefaultStatuslst.Count > 0)
            {
                if (idicDefaultStatuslst.ContainsKey("OTHERS"))
                    Other_Status = idicDefaultStatuslst["OTHERS"];
                if (idicDefaultStatuslst.ContainsKey("ASSESSMENT"))
                    Ass_Status = idicDefaultStatuslst["ASSESSMENT"];
            }

            string jsons = "";
            bool sSaveEnableXmlMisMatch = false;
            if (strAssessment == "Load")
            {
                assessmentLoadList = objAssessmentManager.LoadAssessment(ClientSession.EncounterId, ClientSession.HumanId, "N", ClientSession.UserName, "load");

                //CAP-2128
                assessmentLoadList.Problem_List = new List<ProblemList>();
                if (assessmentLoadList.Assessment != null && assessmentLoadList.Assessment.Count() > 0)
                {
                    //string FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
                    //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                    //try
                    //{
                    //    if (File.Exists(strXmlFilePath) == true)
                    //    {
                    //        XmlDocument itemDoc = new XmlDocument();
                    //        XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
                    //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    //        {
                    //            itemDoc.Load(fs);
                    //            XmlText.Close();
                    //            if (itemDoc.GetElementsByTagName("AssessmentList")[0] == null)
                    //            {
                    //                sSaveEnableXmlMisMatch = true;
                    //            }
                    //            fs.Dispose();
                    //            fs.Close();
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception(ex.Message + " - " + strXmlFilePath);
                    //}
                    IList<string> ilsAssessmentEncounterTagList = new List<string>();
                    ilsAssessmentEncounterTagList.Add("AssessmentList");



                    IList<object> ilstAsEncounterBlobFinal = new List<object>();

                    ilstAsEncounterBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilsAssessmentEncounterTagList);

                    if (ilstAsEncounterBlobFinal != null && ilstAsEncounterBlobFinal.Count > 0)
                    {
                        if (ilstAsEncounterBlobFinal[0] == null)
                        {
                            sSaveEnableXmlMisMatch = true;

                            // objPatientList = (from m in lsttemppatientresults where m.Results_Type == "Vitals" select m).ToList<PatientResults>();
                        }


                    }


                }
            }
            else if (strAssessment == "CopyPrevious")
            {
                DateTime dtDOS = DateTime.MinValue;
                dtDOS = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service);
                assessmentLoadList = objAssessmentManager.GetAssesmentForPastEncounter(ClientSession.EncounterId, ClientSession.HumanId, ClientSession.PhysicianId, dtDOS);
                IList<Assessment> _Assessment = new List<Assessment>();
                if (!Default_Ass_Status)
                {
                    _Assessment = assessmentLoadList.Assessment.Select(a => new Assessment()
                    {
                        Id = 0,
                        Encounter_ID = ClientSession.EncounterId,
                        Assessment_Notes = a.Assessment_Notes,
                        Assessment_Type = a.Assessment_Type,
                        Chronic_Problem = a.Chronic_Problem,
                        Created_By = a.Created_By,
                        Created_Date_And_Time = a.Created_Date_And_Time,
                        ICD_Description = a.ICD_Description,
                        Diagnosis_Source = a.Diagnosis_Source,
                        Human_ID = a.Human_ID,
                        ICD = a.ICD,
                        ICD_9_Description = a.ICD_9_Description,
                        ICD_9 = a.ICD_9,
                        Modified_By = a.Modified_By,
                        Modified_Date_And_Time = a.Modified_Date_And_Time,
                        Physician_ID = a.Physician_ID,
                        Primary_Diagnosis = a.Primary_Diagnosis,
                        Internal_Property_ProblemListID = a.Internal_Property_ProblemListID,
                        Internal_Property_ProblemListVersion = a.Internal_Property_ProblemListVersion,
                        Snomed_Code = a.Snomed_Code,
                        Snomed_Code_Description = a.Snomed_Code_Description,
                        Assessment_Status = a.Assessment_Status,
                        Version = a.Version,
                        Version_Year = a.Version_Year,
                        Parent_ICD = a.Parent_ICD

                    }).ToList();
                }
                else
                {
                    _Assessment = assessmentLoadList.Assessment.Select(a => new Assessment()
                    {
                        Id = 0,
                        Encounter_ID = ClientSession.EncounterId,
                        Assessment_Notes = a.Assessment_Notes,
                        Assessment_Type = a.Assessment_Type,
                        Chronic_Problem = a.Chronic_Problem,
                        Created_By = a.Created_By,
                        Created_Date_And_Time = a.Created_Date_And_Time,
                        ICD_Description = a.ICD_Description,
                        Diagnosis_Source = a.Diagnosis_Source,
                        Human_ID = a.Human_ID,
                        ICD = a.ICD,
                        ICD_9_Description = a.ICD_9_Description,
                        ICD_9 = a.ICD_9,
                        Modified_By = a.Modified_By,
                        Modified_Date_And_Time = a.Modified_Date_And_Time,
                        Physician_ID = a.Physician_ID,
                        Primary_Diagnosis = a.Primary_Diagnosis,
                        Internal_Property_ProblemListID = a.Internal_Property_ProblemListID,
                        Internal_Property_ProblemListVersion = a.Internal_Property_ProblemListVersion,
                        Snomed_Code = a.Snomed_Code,
                        Snomed_Code_Description = a.Snomed_Code_Description,
                        Assessment_Status = Other_Status,
                        Version = a.Version,
                        Version_Year = a.Version_Year,
                        Parent_ICD = a.Parent_ICD

                    }).ToList();
                }


                IList<GeneralNotes> _General_Notes = assessmentLoadList.General_Notes.Select(a => new GeneralNotes()
                {
                    Id = 0,
                    Encounter_ID = ClientSession.EncounterId,
                    Created_By = a.Created_By,
                    Created_Date_And_Time = a.Created_Date_And_Time,
                    Human_ID = a.Human_ID,
                    Modified_By = a.Modified_By,
                    Modified_Date_And_Time = a.Modified_Date_And_Time,
                    Name_Of_The_Field = a.Name_Of_The_Field,
                    Notes = a.Notes,
                    Parent_Field = a.Parent_Field,
                    Version = a.Version

                }).ToList();

                assessmentLoadList.Assessment = _Assessment;
                assessmentLoadList.General_Notes = _General_Notes;
            }

            if (assessmentLoadList.Assessment != null && assessmentLoadList.Assessment.Count > 0)//BugID:53007 
                bSuggestIcds = false;

            IList<string> lstParent_ICD = new List<string>();
            lstParent_ICD = assessmentLoadList.Assessment.Select(a => a.Parent_ICD.Trim()).Distinct().ToList<string>();
            if (strAssessment == "Load" || strAssessment == "CopyPrevious")
            {
                if (strAssessment == "CopyPrevious")
                {
                    if (assessmentLoadList.Assessment != null)
                    {
                        if (Convert.ToUInt32(assessmentLoadList.PEncID) == 0)
                        {
                            jsons = "Message-" + "210010";
                            return jsons;
                        }
                        else if (!Convert.ToBoolean(assessmentLoadList.Physician_Process))
                        {
                            jsons = "Message-" + "210016";
                            return jsons;
                        }
                        else if (assessmentLoadList.Assessment != null && assessmentLoadList.Assessment.Count == 0)
                        {
                            jsons = "Message-" + "170014";
                            return jsons;
                        }
                        else
                        {
                            //goto l;
                            //Gitlab #1590 - Removed goto and added the below line
                            bSuggestIcds = true;
                        }
                    }
                }
                IList<string> problemListCodesWithParentCodesTemp = new List<string>();
                IList<string> pblmListParentICD = new List<string>();
                IList<string> pblmListCodes = new List<string>();
                if (bSuggestIcds)//BugID:54773
                {
                    if (assessmentLoadList.Problem_List != null && assessmentLoadList.Problem_List.Count > 0)
                    {
                        // bugId:65363
                        //   pblmMedList = assessmentLoadList.Problem_List.Where(a => a.Status.ToUpper() == "ACTIVE" && a.Is_Active == "Y" && !a.Reference_Source.Contains("Deleted")).ToList();
                        pblmMedList = assessmentLoadList.Problem_List.Where(a => a.Status.ToUpper() == "ACTIVE" && a.Is_Active == "Y" || (a.Reference_Source.Contains("Problem List|Deleted"))).ToList();
                        if (pblmMedList.Count > 0)
                        {
                            foreach (ProblemList obj in pblmMedList)
                                pblmListCodes.Add(obj.ICD.Trim());
                        }
                        string sICD = string.Empty;
                        for (int i = 0; i < pblmListCodes.Count; i++)
                        {
                            if (sICD == string.Empty)
                                sICD = pblmListCodes[i].ToString();
                            else
                                sICD += '|' + pblmListCodes[i].ToString();

                        }

                        AllICD_9Manager objAllIcdMngr = new AllICD_9Manager();
                        foreach (string s in pblmListCodes)
                            lstParent_ICD.Remove(s);//to prevent removal of Past_medical_history ICDs which need to be reflected in case of change in its status.
                        pblmListParentICD = objAllIcdMngr.TakeParentICD(sICD);
                    }

                    if (assessmentLoadList.VitalsBasedICD_List != null && assessmentLoadList.VitalsBasedICD_List.Count > 0)
                    {
                        #region commented
                        //FillPatientSummaryBarDTO objSummaryDTO = new FillPatientSummaryBarDTO();
                        //IList<string> assessVitalsList = new List<string>();
                        //assessVitalsList = assessmentLoadList.VitalsBasedICD_List;

                        //for (int i = 0; i < assessVitalsList.Count; i++)
                        //{
                        //    var exist = (from assess in pblmListParentICD where assess.Contains(assessVitalsList[i]) == true select assess);
                        //    if (exist.Count() == 0)
                        //        pblmListParentICD.Add(assessVitalsList[i]);
                        //}
                        #endregion
                        //to find the exact match for ICD(Exists used instead of Contains)
                        IList<string> assessVitalsList = new List<string>();
                        assessVitalsList = assessmentLoadList.VitalsBasedICD_List;
                        List<string> ICDList = new List<string>();
                        for (int y = 0; y < pblmListParentICD.Count; y++)
                        {
                            string[] str = pblmListParentICD[y].Split('!');
                            foreach (string s in str)
                            {
                                ICDList.Add(s);
                            }
                        }
                        for (int i = 0; i < assessVitalsList.Count; i++)
                        {
                            string s = assessVitalsList[i];
                            bool val = (ICDList.Exists(a => a == s));
                            if (val == false)
                                pblmListParentICD.Add(assessVitalsList[i]);
                        }
                    }

                    if (pblmListParentICD != null && pblmListParentICD.Count > 0)
                    {
                        var distinct = from h in pblmListParentICD group h by new { h } into g select new { g.Key.h };

                        foreach (var code in distinct)
                        {
                            var duplicate1 = problemListCodesWithParentCodesTemp.Where(h => h.Contains(code.h)).Select(s => s).ToList();
                            var duplicate = (from dup in problemListCodesWithParentCodesTemp where dup.Contains(code.h) select dup).ToList();

                            if (duplicate.Count() == 0)
                            {
                                if (code.h != string.Empty)
                                    problemListCodesWithParentCodesTemp.Add(code.h);
                            }
                            else
                            {
                                if (code.h.Split('!')[0] != duplicate.First().Split('!')[0])
                                    problemListCodesWithParentCodesTemp.Add(code.h);
                            }
                        }
                    }
                }
                else
                {
                    if (assessmentLoadList.Problem_List != null && assessmentLoadList.Problem_List.Count > 0)
                    {
                        pblmMedList = assessmentLoadList.Problem_List.Where(a => a.Status.ToUpper() == "ACTIVE" && a.Is_Active == "Y" && !a.Reference_Source.Contains("Deleted")).ToList();


                        if (pblmMedList.Count > 0)
                        {
                            foreach (ProblemList obj in pblmMedList)
                                pblmListCodes.Add(obj.ICD.Trim());
                        }
                    }

                    //CAP-1671
                    if (assessmentLoadList.VitalsBasedICD_List != null && assessmentLoadList.VitalsBasedICD_List.Count > 0)
                    {
                        #region commented
                        //FillPatientSummaryBarDTO objSummaryDTO = new FillPatientSummaryBarDTO();
                        //IList<string> assessVitalsList = new List<string>();
                        //assessVitalsList = assessmentLoadList.VitalsBasedICD_List;

                        //for (int i = 0; i < assessVitalsList.Count; i++)
                        //{
                        //    var exist = (from assess in pblmListParentICD where assess.Contains(assessVitalsList[i]) == true select assess);
                        //    if (exist.Count() == 0)
                        //        pblmListParentICD.Add(assessVitalsList[i]);
                        //}
                        #endregion
                        //to find the exact match for ICD(Exists used instead of Contains)
                        IList<string> assessVitalsList = new List<string>();
                        assessVitalsList = assessmentLoadList.VitalsBasedICD_List;
                        List<string> ICDList = new List<string>();
                        for (int y = 0; y < pblmListParentICD.Count; y++)
                        {
                            string[] str = pblmListParentICD[y].Split('!');
                            foreach (string s in str)
                            {
                                ICDList.Add(s);
                            }
                        }
                        for (int i = 0; i < assessVitalsList.Count; i++)
                        {
                            string s = assessVitalsList[i];
                            bool val = (ICDList.Exists(a => a == s));
                            if (val == false)
                                pblmListParentICD.Add(assessVitalsList[i]);
                        }
                    }

                    if (pblmListParentICD != null && pblmListParentICD.Count > 0)
                    {
                        var distinct = from h in pblmListParentICD group h by new { h } into g select new { g.Key.h };

                        foreach (var code in distinct)
                        {
                            var duplicate1 = problemListCodesWithParentCodesTemp.Where(h => h.Contains(code.h)).Select(s => s).ToList();
                            var duplicate = (from dup in problemListCodesWithParentCodesTemp where dup.Contains(code.h) select dup).ToList();

                            if (duplicate.Count() == 0)
                            {
                                if (code.h != string.Empty)
                                    problemListCodesWithParentCodesTemp.Add(code.h);
                            }
                            else
                            {
                                if (code.h.Split('!')[0] != duplicate.First().Split('!')[0])
                                    problemListCodesWithParentCodesTemp.Add(code.h);
                            }
                        }
                    }
                }

                if (problemListCodesWithParentCodesTemp != null && problemListCodesWithParentCodesTemp.Count > 0)
                {
                    allICD9ForVitalsProblemListPFSH = objAllIcdMgr.GetProblemListCodeUsingCode(problemListCodesWithParentCodesTemp.Select(p => p.Split('!')[0]).ToArray<string>());
                }
                else
                {
                    if (pblmListCodes != null && pblmListCodes.Count > 0)
                        allICD9ForVitalsProblemListPFSH = objAllIcdMgr.GetProblemListCodeUsingCode(pblmListCodes.Select(p => p).ToArray<string>());
                }


                if (allICD9ForVitalsProblemListPFSH != null && allICD9ForVitalsProblemListPFSH.Count > 0)
                {
                    for (int i = 0; i < allICD9ForVitalsProblemListPFSH.Count; i++)
                    {
                        if (allICD9ForVitalsProblemListPFSH[i].Leaf_Node != "N" && !assessmentLoadList.Assessment.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9) && ((assessmentLoadList.Assessment.Where(s => s.Diagnosis_Source.ToUpper() != "VITALS|DELETED").Count() == 0 ? (assessmentLoadList.Problem_List.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                            : (assessmentLoadList.Problem_List.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9))) || assessmentLoadList.VitalsBasedICD_List.Any(a => a.Split('!')[0].ToString() == allICD9ForVitalsProblemListPFSH[i].ICD_9)))//|| currentVitalsBasedICDList.Any(c => c.Split('!')[0].ToString() == allICD9ForVitalsProblemListPFSH[i].ICD_9)                        
                        {
                            if (assessmentLoadList.Assessment.Count() > 0 && assessmentLoadList.Assessment.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9 && a.Diagnosis_Source.ToUpper() == "VITALS|DELETED"))
                                continue;
                            if (assessmentLoadList.Assessment.Count() > 0 && assessmentLoadList.Assessment.Any(a => a.ICD_9 == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                                continue;
                            VitalsAssesment assMngr = new VitalsAssesment();
                            assMngr.ICD_9 = allICD9ForVitalsProblemListPFSH[i].ICD_9;
                            assMngr.Description = allICD9ForVitalsProblemListPFSH[i].ICD_9_Description;
                            ulong pId = assessmentLoadList.Problem_List.Where(p => p.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9 && p.Is_Active == "Y" && !p.Reference_Source.Contains("Deleted")).Select(d => d.Id).FirstOrDefault();
                            string status = string.Empty;

                            if (assessmentLoadList.Problem_List.Any(v => v.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                                //Cap - 1566
                                //status = "Problem List";
                                continue;
                            if (assessmentLoadList.VitalsBasedICD_List.Select(p => p.Split('!')[0]).Any(s => s == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                                status = "Vitals";


                            assMngr.Notes = status;
                            assMngr.ProblemListId = pId;
                            assMngr.AssessmentID = 0;
                            assMngr.sCreatedBy = "";
                            assMngr.sCreatedDateTime = "";
                            int pVersion = assessmentLoadList.Problem_List.Where(p => p.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9 && p.Is_Active == "Y" && !p.Reference_Source.Contains("Deleted")).Select(d => d.Version).FirstOrDefault();
                            assMngr.iVersion = pVersion;
                            assMngr.StatusSelected = Other_Status;

                            if (allICD9ForVitalsProblemListPFSH[i].Version_Year == "ICD_9")
                                strICD9CodeDesc.Add(allICD9ForVitalsProblemListPFSH[i].ICD_9 + "~" + allICD9ForVitalsProblemListPFSH[i].ICD_9_Description + "~" + status + "~" + "Assessment" + "~" + pId + "~" + pVersion + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "" + "~" + Other_Status + "~" + "" + "~" + "");//BugID:47478

                            vitalsproblemList.Add(assMngr);
                        }
                    }

                    //if (allICD9ForVitalsProblemListPFSH.Where(s => s.Leaf_Node == "N").ToList().Count > 0)
                    //{
                    //    foreach (var item in allICD9ForVitalsProblemListPFSH.Where(s => s.Leaf_Node == "N").ToList())
                    //    {
                    //        if (assessmentLoadList.Assessment.Where(s => s.Diagnosis_Source.ToUpper() != "VITALS|DELETED").Count() == 0 ? assessmentLoadList.Problem_List.Any(a => a.ICD == item.ICD_9)
                    //          : assessmentLoadList.Problem_List.Any(a => a.ICD == item.ICD_9))
                    //        {

                    //            int iCount = vitalsproblemList.Where(a => a.ICD_9 == item.ICD_9).ToList().Count;

                    //            if (iCount > 0)
                    //                continue;


                    //            if (item.Version_Year == "ICD_10")
                    //            {
                    //                IList<ProblemList> ResultProblemList = assessmentLoadList.Problem_List.Where(a => a.ICD == item.ICD_9).ToList<ProblemList>();
                    //                if (ResultProblemList != null)
                    //                {
                    //                    if (ResultProblemList.Count > 0)
                    //                    {
                    //                        if (ResultProblemList[0].Version_Year == "ICD_10")
                    //                            strICDDesc.Add(item.ICD_9 + "-" + item.ICD_9_Description);
                    //                        else
                    //                            strICD9CodeDesc.Add(item.ICD_9 + "~" + item.ICD_9_Description + "~" + "" + "IncompleteProblemList" + "~" + "IncompleteProblemList" + "~" + 0 + "~" + 0 + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "");
                    //                    }

                    //                }

                    //            }
                    //            else
                    //                strICD9CodeDesc.Add(item.ICD_9 + "~" + item.ICD_9_Description + "~" + "" + "IncompleteProblemList" + "~" + "IncompleteProblemList" + "~" + 0 + "~" + 0 + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "");
                    //        }
                    //    }
                    //}
                    if (allICD9ForVitalsProblemListPFSH.Where(s => s.Leaf_Node == "N").ToList().Count > 0)
                    {
                        foreach (var item in allICD9ForVitalsProblemListPFSH.Where(s => s.Leaf_Node == "N").ToList())
                        {
                            int iCount = vitalsproblemList.Where(a => a.ICD_9 == item.ICD_9).ToList().Count;


                            if (iCount > 0)
                                continue;


                            if (item.Version_Year == "ICD_10")
                            {
                                strICDDesc.Add(item.ICD_9 + "-" + item.ICD_9_Description);
                            }
                            else
                                strICD9CodeDesc.Add(item.ICD_9 + "~" + item.ICD_9_Description + "~" + "" + "IncompleteProblemList" + "~" + "IncompleteProblemList" + "~" + 0 + "~" + 0 + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "" + "~" + "" + "~" + "" + "~" + "");//BugID:47478
                        }
                    }
                }

            l:
                if (assessmentLoadList.Assessment != null)
                {
                    for (int i = 0; i < assessmentLoadList.Assessment.Count; i++)
                    {

                        if (assessmentLoadList.Assessment[i].Diagnosis_Source.ToUpper() == "VITALS|DELETED")
                            continue;
                        IList<ProblemList> problemList = assessmentLoadList.Problem_List.Where(a => a.ICD == assessmentLoadList.Assessment[i].ICD).ToList<ProblemList>();
                        if (problemList != null)
                        {
                            if (problemList.Count == 0)
                            {
                                if (assessmentLoadList.Assessment[i].ICD_9 != "")
                                    problemList = assessmentLoadList.Problem_List.Where(a => a.ICD == assessmentLoadList.Assessment[i].ICD_9).ToList<ProblemList>();
                            }
                            if (problemList.Count > 0)
                            {

                                if (!assessmentLoadList.Assessment.Any(a => a.Internal_Property_ProblemListID == problemList[0].Id))
                                {
                                    assessmentLoadList.Assessment[i].Internal_Property_ProblemListID = problemList[0].Id;
                                    assessmentLoadList.Assessment[i].Internal_Property_ProblemListVersion = problemList[0].Version;
                                }
                            }
                        }

                        if (strAssessment == "CopyPrevious" && assessmentLoadList.Assessment[i].Version_Year == "ICD_9")
                            strICD9CodeDesc.Add(assessmentLoadList.Assessment[i].ICD + "~" + assessmentLoadList.Assessment[i].ICD_Description + "~" + assessmentLoadList.Assessment[i].Assessment_Notes + "~" + "Assessment" + "~" + assessmentLoadList.Assessment[i].Internal_Property_ProblemListID + "~" + assessmentLoadList.Assessment[i].Internal_Property_ProblemListVersion + "~" + 0 + "~" + 0 + "~" + assessmentLoadList.Assessment[i].Chronic_Problem + "~" + assessmentLoadList.Assessment[i].Primary_Diagnosis + "~" + Ass_Status + "~" + ((assessmentLoadList.Assessment[i].Created_By.Trim() != string.Empty) ? assessmentLoadList.Assessment[i].Created_By : "") + "~" + ((assessmentLoadList.Assessment[i].Created_Date_And_Time.ToString().Trim() != "0001-01-01 00:00:00") ? assessmentLoadList.Assessment[i].Created_Date_And_Time.ToString() : ""));
                        else if (assessmentLoadList.Assessment[i].Version_Year == "ICD_9")
                            strICD9CodeDesc.Add(assessmentLoadList.Assessment[i].ICD + "~" + assessmentLoadList.Assessment[i].ICD_Description + "~" + assessmentLoadList.Assessment[i].Assessment_Notes + "~" + "Assessment" + "~" + assessmentLoadList.Assessment[i].Internal_Property_ProblemListID + "~" + assessmentLoadList.Assessment[i].Internal_Property_ProblemListVersion + "~" + assessmentLoadList.Assessment[i].Id + "~" + assessmentLoadList.Assessment[i].Version + "~" + assessmentLoadList.Assessment[i].Chronic_Problem + "~" + assessmentLoadList.Assessment[i].Primary_Diagnosis + "~" + Ass_Status + "~" + ((assessmentLoadList.Assessment[i].Created_By.Trim() != string.Empty) ? assessmentLoadList.Assessment[i].Created_By : "") + "~" + ((assessmentLoadList.Assessment[i].Created_Date_And_Time.ToString().Trim() != "0001-01-01 00:00:00") ? assessmentLoadList.Assessment[i].Created_Date_And_Time.ToString() : ""));
                    }
                }
                IList<string> templstProblemListICD9 = new List<string>();

                if (strAssessment == "CopyPrevious")
                {
                    if (assessmentLoadList.Problem_List != null && assessmentLoadList.Problem_List.Count > 0)
                    {
                        for (int j = 0; j < assessmentLoadList.Problem_List.Count; j++)
                        {
                            if (assessmentLoadList.Problem_List[j].Version_Year == "ICD_9")
                            {
                                templstProblemListICD9.Add(assessmentLoadList.Problem_List[j].ICD);

                                strICD9CodeDesc.Add(assessmentLoadList.Problem_List[j].ICD + "~"
                                    + assessmentLoadList.Problem_List[j].Problem_Description + "~"
                                    + "" + "~" + "Assessment" + "~" + assessmentLoadList.Problem_List[j].Id + "~"
                                    + assessmentLoadList.Problem_List[j].Version + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "" + "~" + Other_Status + "~" + "" + "~" + "");//BugID:47478
                            }
                        }
                    }
                }

                string ICD_10 = string.Empty;
                IList<string> ICD9singleMapping = new List<string>();
                IList<string> ICD10MutipleMapping = new List<string>();
                for (int i = 0; i < strICD9CodeDesc.Count; i++)
                {
                    bool bcheck = false;
                    XmlDocument xmldoc = new XmlDocument();
                    if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\icd_9_10_mapping.xml"))
                    {
                        xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\icd_9_10_mapping.xml");
                        XmlNodeList xmlMappingList = xmldoc.GetElementsByTagName("icd_9_10_mapping");
                        foreach (XmlNode item in xmlMappingList)
                        {
                            if (item != null)
                            {

                                if (item.Attributes[0].Value == strICD9CodeDesc[i].Split('~')[0])
                                {
                                    ICD10MutipleMapping.Add(strICD9CodeDesc[i] + "^" + item.Attributes[1].Value);
                                    bcheck = true;
                                }
                            }
                        }
                        if (!bcheck)
                        {
                            ICD9singleMapping.Add(strICD9CodeDesc[i]);
                        }
                    }

                }

                IList<string> ResultICD10MappingICDs = new List<string>();

                if (ICD10MutipleMapping.Count > 0)
                {
                    for (int z = 0; z < ICD10MutipleMapping.Count; z++)
                    {
                        ResultICD10MappingICDs.Add(ICD10MutipleMapping[z].Split('~')[0] + "~" + ICD10MutipleMapping[z].Split('~')[1]);
                    }

                }

                assessmentLoadList.Assessment = (from b in assessmentLoadList.Assessment where !strICD9CodeDesc.Any(a => a.Split('~')[0] == b.ICD) select b).ToList();
                vitalsproblemList = (from b in vitalsproblemList where !strICD9CodeDesc.Any(a => a.Split('~')[0] == b.ICD_9) select b).ToList();
                if (ICD9singleMapping != null && ICD9singleMapping.Count > 0)
                {
                    IList<ICD9ICD10Mapping> ICD10ICDDescList = new List<ICD9ICD10Mapping>();
                    AllICD_9Manager objAllICDsMgr = new AllICD_9Manager();

                    ICD10ICDDescList = objAllICDsMgr.GetICD10CodeDesc(ICD9singleMapping.Select(a => a.Split('~')[0]).ToArray());
                    IList<string> ResultString = new List<string>();
                    if (ICD10ICDDescList != null)
                    {
                        for (int i = 0; i < ICD10ICDDescList.Count; i++)
                        {
                            if (ICD9singleMapping != null)
                                ResultString = ICD9singleMapping.Where(a => a.Split('~')[0] == ICD10ICDDescList[i].ICD9).ToList();

                            if (vitalsproblemList != null)
                            {
                                if (vitalsproblemList.Any(a => a.ICD_9 == ICD10ICDDescList[i].ICD10))
                                    vitalsproblemList = vitalsproblemList.Where(a => a.ICD_9 != ICD10ICDDescList[i].ICD10).ToList();
                            }
                            if (ICD10ICDDescList[i].MappingRule != "2000")
                            {
                                VitalsAssesment assICD10Mngr = new VitalsAssesment();
                                assICD10Mngr.ICD_9 = ICD10ICDDescList[i].ICD10;
                                assICD10Mngr.Description = ICD10ICDDescList[i].Long_Description;

                                if (ResultString != null && ResultString.Count > 0)
                                {
                                    assICD10Mngr.Notes = ResultString[0].Split('~')[2];

                                    assICD10Mngr.ProblemListId = Convert.ToUInt32(ResultString[0].Split('~')[4]);
                                    assICD10Mngr.AssessmentID = Convert.ToUInt32(ResultString[0].Split('~')[6]);

                                    assICD10Mngr.iProblemListVersion = Convert.ToInt32(ResultString[0].Split('~')[5]);
                                    assICD10Mngr.iVersion = Convert.ToInt32(ResultString[0].Split('~')[7]);
                                    assICD10Mngr.CheckBoxCheck = ResultString[0].Split('~')[8].ToString();
                                    assICD10Mngr.Primary_Diagnosis = ResultString[0].Split('~')[9].ToString();
                                    assICD10Mngr.StatusSelected = ResultString[0].Split('~')[10].ToString();
                                    assICD10Mngr.sCreatedBy = ResultString[0].Split('~')[11].ToString().Trim();
                                    assICD10Mngr.sCreatedDateTime = ResultString[0].Split('~')[12].ToString().Trim();
                                }
                                assICD10Mngr.ICD9Code = ICD10ICDDescList[i].ICD9;
                                assICD10Mngr.ICD9Desc = ICD10ICDDescList[i].ICD_9_Description;

                                vitalsproblemList.Add(assICD10Mngr);
                            }
                            else
                            {
                                IList<string> sReomveICDDesc = new List<string>();
                                if (strICDDesc != null)
                                    sReomveICDDesc = strICDDesc.Select(a => a.Split('-')[0]).ToList();
                                if (sReomveICDDesc != null)
                                {
                                    int iIndex = sReomveICDDesc.IndexOf(ICD10ICDDescList[i].ICD9.Trim());
                                    if (iIndex > -1)
                                        strICDDesc.RemoveAt(iIndex);
                                }
                                if (strICDDesc != null)
                                {
                                    if (!strICDDesc.Contains(ICD10ICDDescList[i].ICD10 + "-" + ICD10ICDDescList[i].Long_Description.Trim()))
                                    {
                                        strICDDesc.Add(ICD10ICDDescList[i].ICD10 + "-" + ICD10ICDDescList[i].Long_Description);
                                        strICD910Code.Add(ICD10ICDDescList[i].ICD10 + "-" + ICD10ICDDescList[i].ICD9);
                                    }
                                }
                            }
                        }
                    }
                }

                if (assessmentLoadList.Assessment != null && assessmentLoadList.Assessment.Count > 0)
                {
                    assessmentLoadList.Assessment = assessmentLoadList.Assessment.Where(a => a.Diagnosis_Source != "VITALS|DELETED").ToList();

                    if (vitalsproblemList.Count > 0)
                    {
                        for (int o = 0; o < assessmentLoadList.Assessment.Count; o++)
                        {
                            IList<VitalsAssesment> ResultVitalsProblemList = vitalsproblemList.Where(a => a.ICD_9 == assessmentLoadList.Assessment[o].ICD).ToList();

                            if (ResultVitalsProblemList.Count > 0)
                            {
                                assessmentLoadList.Assessment[o].Internal_Property_ProblemListID = ResultVitalsProblemList[0].ProblemListId;
                                assessmentLoadList.Assessment[o].Internal_Property_ProblemListVersion = ResultVitalsProblemList[0].iProblemListVersion;
                            }


                        }

                        if (strAssessment == "CopyPrevious")
                        {
                            if (vitalsproblemList != null && vitalsproblemList.Count > 0)
                                HttpContext.Current.Session["VitalsProblemList"] = vitalsproblemList;

                            vitalsproblemList = (from b in vitalsproblemList
                                                 where !templstProblemListICD9.Any(a => a == b.ICD9Code)
                                                 select b).ToList();
                        }

                        vitalsproblemList = (from b in vitalsproblemList where !assessmentLoadList.Assessment.Any(a => a.ICD == b.ICD_9) select b).ToList();
                    }
                }

                //CAP-1671
                var ListAssessment = assessmentLoadList.Assessment.Select(a => new { ICDCode = a.ICD, ICDDescription = a.ICD_Description, IsPrimary = a.Primary_Diagnosis, ProblemListID = a.Internal_Property_ProblemListID, Notes = a.Assessment_Notes, AssessmentID = a.Id, iVersion = a.Version, iProblemListVersion = a.Internal_Property_ProblemListVersion, CheckBoxCheck = a.Chronic_Problem, StatusSelected = a.Assessment_Status, IncompleteICDCode = "", ICD9Code = a.ICD_9, ICD9Desc = a.ICD_9_Description, ParentICD = a.Parent_ICD, Created_by = a.Created_By, Created_date = a.Created_Date_And_Time.ToString(), Updated = "N", Orig_Status = a.Assessment_Status }).ToList();
                var ListVitalsProblemList = vitalsproblemList.Select(a => new { ICDCode = a.ICD_9, ICDDescription = a.Description, IsPrimary = a.Primary_Diagnosis, ProblemListID = a.ProblemListId, Notes = a.Notes, AssessmentID = a.AssessmentID, iVersion = a.iVersion, iProblemListVersion = a.iVersion, CheckBoxCheck = a.CheckBoxCheck, StatusSelected = a.StatusSelected, IncompleteICDCode = "", ICD9Code = a.ICD9Code, ICD9Desc = a.ICD9Desc, ParentICD = "", Created_by = a.sCreatedBy, Created_date = a.sCreatedDateTime, Updated = "Y", Orig_Status = a.StatusSelected }).ToList();

                bool bPotentialEnable = false;
                if (assessmentLoadList.Potential_Diagnosis != null && assessmentLoadList.Potential_Diagnosis.Count > 0)
                {
                    bPotentialEnable = true;
                }

                var ListPotentailDiagnosis = assessmentLoadList.Potential_Diagnosis.Where(a => a.Move_To_Assessment != "Y").Select(a => new { ICDCode = a.ICD_Code, ICDDescription = a.ICD_Description, iVersion = a.Version });

                bool bSave = true;

                if (vitalsproblemList != null && vitalsproblemList.Count > 0)
                {
                    if (vitalsproblemList.Where(a => a.Notes == "Problem List" || a.Notes == "Vitals").Count() > 0)
                        bSave = false;
                }
                //BugID:53007 
                if (bSuggestIcds)
                    ListAssessment = ListAssessment.Concat(ListVitalsProblemList).ToList();
                    
                if (!bSuggestIcds)
                {
                    strICDDesc = new List<string>();
                    ICD10MutipleMapping = new List<string>();
                    //CAP-1671
                    if (ListVitalsProblemList.Count() > 0)
                    {
                        AssessmentVitalsLookupManager assessmentVitalsLookupManager = new AssessmentVitalsLookupManager();
                        var assesmentVitals = assessmentVitalsLookupManager.GetAll();
                        var copyListVitalsProblemList = ListVitalsProblemList;
                        var assessmentListToDelete = new List<Assessment>();
                        foreach (var item in copyListVitalsProblemList)
                        {
                            if(!ListAssessment.Any(x=>x.ICDCode == item.ICDCode))
                            {
                                var vitalLookUpType = assesmentVitals.FirstOrDefault(x => x.ICD_10 == item.ICDCode);
                                var assesmentLookUpType = assesmentVitals.FirstOrDefault(x => ListAssessment.Any(y => y.ICDCode == x.ICD_10) && x.Field_Name == vitalLookUpType.Field_Name);
                                //CAP-2158
                                //if ((vitalLookUpType?.Field_Name ?? "") == (assesmentLookUpType?.Field_Name ?? ""))
                                //{
                                //    var oldAssementVital = ListAssessment.FirstOrDefault(x => x.ICDCode == assesmentLookUpType.ICD_10);
                                //    ListAssessment.Remove(oldAssementVital);
                                //    var assessment = objAssessmentManager.GetAssesmentUsingAssesmentId(oldAssementVital.AssessmentID);
                                //    assessmentListToDelete.Add(assessment);
                                //}
                            }
                            //CAP-2158
                            //else
                            //{
                            //    ListVitalsProblemList.Remove(item);
                            //}
                        }


                        objAssessmentManager.BatchOperationsToAssessment(new List<Assessment>(),
                        new List<Assessment>(), assessmentListToDelete.ToArray<Assessment>(),
                        new List<ProblemList>(), new List<ProblemList>(),
                        new List<ProblemList>(), string.Empty,
                        null, new TreatmentPlan(), ClientSession.UserName, ClientSession.EncounterId, ClientSession.HumanId,
                        ClientSession.PhysicianId, new List<string>(), "No", "", ClientSession.LegalOrg, new List<EandMCodingICD>(), new List<EandMCodingICD>());
                    }
                    //CAP-1671
                    if (assessmentLoadList.VitalsBasedICD_List.Count > 0)
                    {
                        AssessmentVitalsLookupManager assessmentVitalsLookupManager = new AssessmentVitalsLookupManager();
                        var assesmentVitals = assessmentVitalsLookupManager.GetAll();
                        var copyListVitalsProblemList = assessmentLoadList.VitalsBasedICD_List;
                        var assessmentListToDelete = new List<Assessment>();
                        foreach (var item in copyListVitalsProblemList)
                        {
                            var lstFieldType = assesmentVitals.FirstOrDefault(x=> x.ICD_10 == item);
                            var assessmentLookUpType = assesmentVitals.FirstOrDefault(x => ListAssessment.Any(y => y.ICDCode == x.ICD_10) && x.Field_Name == (lstFieldType?.Field_Name ?? ""));
                            if ((lstFieldType?.Field_Name??"") == (assessmentLookUpType?.Field_Name ?? ""))
                            {
                                var oldAssementVital = ListAssessment.FirstOrDefault(x => x.ICDCode == assessmentLookUpType.ICD_10 && x.ICDCode != item);
                                //CAP-2158
                                //if (oldAssementVital != null)
                                //{
                                //    ListAssessment.Remove(oldAssementVital);
                                //    var assessment = objAssessmentManager.GetAssesmentUsingAssesmentId(oldAssementVital.AssessmentID);
                                //    if (!assessmentListToDelete.Any(x => x.Id == assessment.Id))
                                //    {
                                //        assessmentListToDelete.Add(assessment);
                                //    }
                                //}
                            }
                        }

                        objAssessmentManager.BatchOperationsToAssessment(new List<Assessment>(),
                        new List<Assessment>(), assessmentListToDelete.ToArray<Assessment>(),
                        new List<ProblemList>(), new List<ProblemList>(),
                        new List<ProblemList>(), string.Empty,
                        null, new TreatmentPlan(), ClientSession.UserName, ClientSession.EncounterId, ClientSession.HumanId,
                        ClientSession.PhysicianId, new List<string>(), "No", "", ClientSession.LegalOrg, new List<EandMCodingICD>(), new List<EandMCodingICD>());
                    }
                    ListAssessment = ListAssessment.Concat(ListVitalsProblemList).ToList();
                }
                strICDDesc = (from val in strICDDesc where !lstParent_ICD.Any(a => a.Trim() == val.Split('-')[0].Trim()) select val).ToList<string>();//to prevent a previously added and moved(to assessment grid) parentICD from being added to incomplete problem list(ROS,VITALS,RCopia_MEd ICDs)
                var IncompleteProblemList = strICDDesc.Select(a => new { ICDCODE = a.Split('-')[0], ICDDescription = a.Split('-')[1] });
                string json = new JavaScriptSerializer().Serialize(ListAssessment);
                string jsonIncompleteProblemList = new JavaScriptSerializer().Serialize(IncompleteProblemList);
                string sICD10Mapping = new JavaScriptSerializer().Serialize(ICD10MutipleMapping);
                string jsonStatusList = new JavaScriptSerializer().Serialize(DefaultStatusList);
                string jsonStatuslst = new JavaScriptSerializer().Serialize(Statuslst);
                string jsonPotentailDiagnosis = new JavaScriptSerializer().Serialize(ListPotentailDiagnosis);
                string jsonPotentialEnable = new JavaScriptSerializer().Serialize(bPotentialEnable);
                string GeneralNotes = "";

                var generalNotes = strAssessment == "CopyPrevious" ? assessmentLoadList.General_Notes : assessmentLoadList.General_NotesCurrentList;

                if (generalNotes != null && generalNotes.Count > 0)
                    GeneralNotes = new JavaScriptSerializer().Serialize(generalNotes[0].Notes);
                else
                    GeneralNotes = new JavaScriptSerializer().Serialize(GeneralNotes);

                string sSave = "";

                sSave = new JavaScriptSerializer().Serialize(bSave);

                string sDisable = "";
                if ((ClientSession.UserRole.ToUpper() == "SCRIBE" || ClientSession.UserRole.ToUpper() == "PHYSICIAN" || ClientSession.UserRole.ToUpper() == "PHYSICIAN ASSISTANT") && (ClientSession.UserCurrentProcess != "PROVIDER_REVIEW") && (ClientSession.UserCurrentProcess != "DISABLE"))
                {
                    sDisable = new JavaScriptSerializer().Serialize("Enable");
                }
                //Jira CAP-1260
                else if (ClientSession.UserCurrentProcess == "AKIDO_REVIEW_CODING")
                {
                    sDisable = new JavaScriptSerializer().Serialize("Enable");
                }
                else
                {
                    sDisable = new JavaScriptSerializer().Serialize("Disable");
                }

                if (strAssessment == "CopyPrevious")
                {
                    jsons = "{\"AssessmentList\" :" + json + "," + "\"ResultGeneralNotes\":" + GeneralNotes + "," + "\"ICD10Tool\":" + sICD10Mapping + "}";
                }
                else
                {
                    jsons = "{\"AssessmentList\" :" + json + "," +
                                      "\"FromProblemList\":" + jsonIncompleteProblemList + "," + "\"PotentialDiagnosisList\":" + jsonPotentailDiagnosis + "," + "\"btnPotentialDiagnosis\":" + jsonPotentialEnable + "," +
                                      "\"ICD10Tool\":" + sICD10Mapping + "," + "\"ResultGeneralNotes\":" + GeneralNotes + "," + "\"SaveEnableDisable\":" + sSave + "," + "\"DisableScreen\":" + sDisable + "," + "\"StatusDefaultList\":" + jsonStatusList + "," + "\"StatusList\":" + jsonStatuslst + "," + "\"SaveEnableXmlMismatch\":" + new JavaScriptSerializer().Serialize(sSaveEnableXmlMisMatch) + "}";
                    HttpContext.Current.Session["GeneralNotes"] = assessmentLoadList.General_NotesCurrentList;
                }

                HttpContext.Current.Session["ProblemList"] = assessmentLoadList.Problem_List;
                //CAP-1671
                HttpContext.Current.Session["VitalsList"] = !bSuggestIcds ? new List<string>() : assessmentLoadList.VitalsBasedICD_List;
                HttpContext.Current.Session["ICD910Code"] = strICD910Code;
                if (strAssessment != "CopyPrevious")
                {
                    //CAP-1671
                    HttpContext.Current.Session["VitalsProblemList"] = !bSuggestIcds ? new List<VitalsAssesment>() : vitalsproblemList;
                    HttpContext.Current.Session["MultiMappingProblemList"] = ResultICD10MappingICDs;
                    HttpContext.Current.Session["Is_Assessment_CopyPrevious"] = "No";
                }
                if (strAssessment == "CopyPrevious")
                {
                    HttpContext.Current.Session["Is_Assessment_CopyPrevious"] = "Yes";
                }
            }

            return jsons;
        }



        [WebMethod(EnableSession = true)]
        public static string SaveAssessmentTable(object[] name, string sGeneralNotes)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //Check Truncated ICD Codes:
            IList<string> lstICD = new List<string>();
            foreach (object[] oj in name)
            {
                if (oj.Count() == 0)
                {
                    continue;
                }
                if (oj[0].ToString().Trim() != "Del")
                {
                    lstICD.Add(oj[4].ToString().Trim());
                }
            }
            if (lstICD.Count() > 0)
            {
                string sTodate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
                if (sTodate.Equals("0001-01-01"))
                    sTodate = DateTime.Now.ToString("yyyy-MM-dd");
                IList<string> lstTruncatedICD = new List<string>();
                AllICD_9Manager objAllICDMgr = new AllICD_9Manager();
                lstTruncatedICD = objAllICDMgr.CheckTruncatedICD10Codes(lstICD, sTodate);
                if (lstTruncatedICD != null && lstTruncatedICD.Count() > 0)
                {
                    return "{\"TruncatedICDList\" :" + new JavaScriptSerializer().Serialize(lstTruncatedICD) + "}"; ;
                }
            }

            IList<Assessment> assementInsertList = new List<Assessment>();
            IList<Assessment> assessmentListToUpdate = new List<Assessment>();
            IList<Assessment> assessmentListToDelete = new List<Assessment>();
            IList<ProblemList> probListToAdd = new List<ProblemList>();
            IList<ProblemList> probListToUpdate = new List<ProblemList>();
            IList<ProblemList> probListIdToDelete = new List<ProblemList>();
            GeneralNotes objGeneralNotes = null;
            TreatmentPlan SaveTreatmentPlan = new TreatmentPlan();
            IList<ulong> ulProbIDList = new List<ulong>();
            IList<string> sIncompleteList = new List<string>();
            IList<string> sMacraICDChkList = new List<string>();
            IList<ProblemList> sessionProblemList = (IList<ProblemList>)HttpContext.Current.Session["ProblemList"];
            string strIcd = string.Empty;
            string sPrimaryIcd = string.Empty;
            string sIs_Assessment_CopyPrevious = HttpContext.Current.Session["Is_Assessment_CopyPrevious"].ToString();
            IList<string> sessionVitalsList = (IList<string>)HttpContext.Current.Session["VitalsList"];

            IList<VitalsAssesment> sessionVitalsProblemList = (IList<VitalsAssesment>)HttpContext.Current.Session["VitalsProblemList"];
            IList<string> sessionMultiMappingProblemList = (IList<string>)HttpContext.Current.Session["MultiMappingProblemList"];

            string sLocalTime = string.Empty;
            foreach (object[] oj in name)
            {
                if (oj.Count() == 0)
                {
                    continue;
                }

                if (oj[0].ToString().Trim() == "Del")
                {
                    if (Convert.ToUInt32(oj[9]) != 0)
                    {
                        Assessment assessmentDelete = new Assessment();
                        assessmentDelete.Id = Convert.ToUInt32(oj[9]);

                        assessmentDelete.Human_ID = ClientSession.HumanId;
                        assessmentDelete.Encounter_ID = ClientSession.EncounterId;
                        assessmentDelete.Version = Convert.ToInt32(oj[10]);
                        assessmentDelete.ICD = oj[4].ToString();
                        assessmentDelete.ICD_Description = oj[5].ToString();
                        assessmentDelete.Modified_By = ClientSession.UserName;
                        assessmentDelete.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        assessmentListToDelete.Add(assessmentDelete);
                    }
                    if (Convert.ToUInt32(oj[9]) == 0)
                    {
                        if (sessionVitalsList.Any(a => a.Split('!')[0].ToString() == oj[4].ToString()))
                        {
                            Assessment assessmentAdd = new Assessment();
                            assessmentAdd.Encounter_ID = ClientSession.EncounterId;
                            assessmentAdd.Human_ID = ClientSession.HumanId;
                            assessmentAdd.Physician_ID = ClientSession.PhysicianId;
                            assessmentAdd.ICD = oj[4].ToString();
                            assessmentAdd.ICD_Description = oj[5].ToString();
                            assessmentAdd.Primary_Diagnosis = Convert.ToBoolean(oj[1]) == true ? "Y" : "N";
                            assessmentAdd.Diagnosis_Source = "VITALS|DELETED";
                            assessmentAdd.Assessment_Type = "Selected";

                            if (Convert.ToBoolean(oj[2]) == true && Convert.ToBoolean(oj[3]) == true)
                                assessmentAdd.Chronic_Problem = "BOTH";
                            else if (Convert.ToBoolean(oj[2]) == true && Convert.ToBoolean(oj[3]) == false)
                                assessmentAdd.Chronic_Problem = "CHRONIC";
                            else if (Convert.ToBoolean(oj[2]) == false && Convert.ToBoolean(oj[3]) == true)
                                assessmentAdd.Chronic_Problem = "PROBLEM";


                            assessmentAdd.Created_By = ClientSession.UserName;
                            assessmentAdd.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            sLocalTime = UtilityManager.ConvertToUniversal(assessmentAdd.Created_Date_And_Time).ToString("yyyy-MM-dd hh:mm:ss tt");

                            assessmentAdd.Assessment_Status = oj[6].ToString();
                            assessmentAdd.Assessment_Notes = Convert.ToString(oj[7]);
                            assessmentAdd.Version_Year = "ICD_10";
                            if (oj[13] != null)
                                assessmentAdd.ICD_9 = oj[13].ToString();

                            if (oj[14] != null)
                                assessmentAdd.ICD_9_Description = oj[14].ToString();
                            assementInsertList.Add(assessmentAdd);

                        }


                    }
                }
                else if (Convert.ToInt32(oj[9]) == 0)
                {
                    Assessment assessmentAdd = new Assessment();
                    assessmentAdd.Encounter_ID = ClientSession.EncounterId;
                    assessmentAdd.Human_ID = ClientSession.HumanId;
                    assessmentAdd.Physician_ID = ClientSession.PhysicianId;
                    assessmentAdd.ICD = oj[4].ToString();
                    assessmentAdd.ICD_Description = oj[5].ToString();
                    assessmentAdd.Primary_Diagnosis = Convert.ToBoolean(oj[1]) == true ? "Y" : "N";
                    assessmentAdd.Diagnosis_Source = "NONE";
                    assessmentAdd.Assessment_Type = "Selected";

                    if (Convert.ToBoolean(oj[2]) == true && Convert.ToBoolean(oj[3]) == true)
                        assessmentAdd.Chronic_Problem = "BOTH";
                    else if (Convert.ToBoolean(oj[2]) == true && Convert.ToBoolean(oj[3]) == false)
                        assessmentAdd.Chronic_Problem = "CHRONIC";
                    else if (Convert.ToBoolean(oj[2]) == false && Convert.ToBoolean(oj[3]) == true)
                        assessmentAdd.Chronic_Problem = "PROBLEM";


                    assessmentAdd.Created_By = ClientSession.UserName;
                    assessmentAdd.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    assessmentAdd.Assessment_Status = oj[6].ToString();
                    assessmentAdd.Assessment_Notes = Convert.ToString(oj[7]);
                    assessmentAdd.Version_Year = "ICD_10";
                    if (oj[13] != null)
                        assessmentAdd.ICD_9 = oj[13].ToString();

                    if (oj[14] != null)
                        assessmentAdd.ICD_9_Description = oj[14].ToString();
                    if (oj[12] != null && oj[12].ToString().Trim() != "")
                    {
                        assessmentAdd.Parent_ICD = oj[12].ToString().Trim();
                        sIncompleteList.Add(oj[12].ToString().Trim());
                        sMacraICDChkList.Add(oj[12].ToString().Trim() + "|" + assessmentAdd.ICD);
                    }
                    assementInsertList.Add(assessmentAdd);

                    //if (assessmentAdd.Primary_Diagnosis == "Y")
                    //{
                    //    sPrimaryIcd += "* " + oj[5].ToString() + "(ICD- " + oj[4].ToString() + ")";
                    //    if (Convert.ToString(oj[7]).Trim() != string.Empty)
                    //    {
                    //        sPrimaryIcd += " - " + Convert.ToString(oj[7]);
                    //    }
                    //    if (oj[6].ToString().Trim() != string.Empty)
                    //        sPrimaryIcd += " - " + oj[6].ToString();

                    //    sPrimaryIcd += Environment.NewLine;
                    //}
                    //else
                    //{
                    //    strIcd += "* " + oj[5].ToString() + "(ICD- " + oj[4].ToString() + ")";
                    //    if (Convert.ToString(oj[7]).Trim() != string.Empty)
                    //    {
                    //        strIcd += " - " + Convert.ToString(oj[7]);
                    //    }
                    //    if (oj[6].ToString().Trim() != string.Empty)
                    //        strIcd += " - " + oj[6].ToString();

                    //    strIcd += Environment.NewLine;
                    //}
                }
                else
                {
                    Assessment assessmentUpdate = new Assessment();

                    assessmentUpdate.Encounter_ID = ClientSession.EncounterId;
                    assessmentUpdate.Human_ID = ClientSession.HumanId;
                    assessmentUpdate.Physician_ID = ClientSession.PhysicianId;
                    assessmentUpdate.ICD = oj[4].ToString();
                    assessmentUpdate.ICD_Description = oj[5].ToString();
                    assessmentUpdate.Primary_Diagnosis = Convert.ToBoolean(oj[1]) == true ? "Y" : "N";

                    if (Convert.ToBoolean(oj[2]) == true && Convert.ToBoolean(oj[3]) == true)
                        assessmentUpdate.Chronic_Problem = "BOTH";
                    else if (Convert.ToBoolean(oj[2]) == true && Convert.ToBoolean(oj[3]) == false)
                        assessmentUpdate.Chronic_Problem = "CHRONIC";
                    else if (Convert.ToBoolean(oj[2]) == false && Convert.ToBoolean(oj[3]) == true)
                        assessmentUpdate.Chronic_Problem = "PROBLEM";

                    assessmentUpdate.Diagnosis_Source = "NONE";
                    assessmentUpdate.Assessment_Type = "Selected";
                    assessmentUpdate.Assessment_Status = oj[6].ToString();
                    assessmentUpdate.Assessment_Notes = Convert.ToString(oj[7]);
                    assessmentUpdate.Id = Convert.ToUInt32(oj[9]);
                    assessmentUpdate.Version = Convert.ToInt32(oj[10]);

                    assessmentUpdate.Version = Convert.ToInt32(oj[10]);// 

                    assessmentUpdate.Version_Year = "ICD_10";
                    assessmentUpdate.Created_By = oj[18].ToString();
                    assessmentUpdate.Created_Date_And_Time = Convert.ToDateTime(oj[19].ToString());
                    assessmentUpdate.Modified_By = ClientSession.UserName;
                    assessmentUpdate.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                    if (oj[13] != null)
                        assessmentUpdate.ICD_9 = oj[13].ToString();

                    if (oj[14] != null)
                        assessmentUpdate.ICD_9_Description = oj[14].ToString();
                    if (oj[12] != null && oj[12].ToString().Trim() != "")
                    {
                        assessmentUpdate.Parent_ICD = oj[12].ToString().Trim();
                        sIncompleteList.Add(oj[12].ToString().Trim());
                        sMacraICDChkList.Add(oj[12].ToString().Trim() + "|" + assessmentUpdate.ICD);
                    }
                    assessmentListToUpdate.Add(assessmentUpdate);
                }
                if (oj[12] != null)
                {
                    if (oj[12].ToString() != "")
                    {
                        IList<ProblemList> saveProblemList = sessionProblemList.Where(a => a.ICD.Trim() == oj[12].ToString().Trim()).ToList();

                        if (saveProblemList.Count == 0)
                        {
                            IList<string> sICD910CodeList = (IList<string>)HttpContext.Current.Session["ICD910Code"];

                            IList<string> sResultICDCodeList = sICD910CodeList.Where(a => a.Split('-')[0].Trim() == oj[12].ToString().Trim()).ToList();
                            saveProblemList = (from b in sessionProblemList where sResultICDCodeList.Any(a => a.Split('-')[1] == b.ICD) select b).ToList<ProblemList>();
                        }
                        if (saveProblemList.Count > 0)
                        {
                            if (!ulProbIDList.Any(a => a == saveProblemList[0].Id))
                            {
                                saveProblemList[0].ICD_9 = saveProblemList[0].ICD_9;
                                saveProblemList[0].ICD_9_Description = saveProblemList[0].Problem_Description;
                                saveProblemList[0].Encounter_ID = ClientSession.EncounterId;
                                saveProblemList[0].Human_ID = ClientSession.HumanId;
                                saveProblemList[0].Physician_ID = ClientSession.PhysicianId;
                                saveProblemList[0].ICD = oj[12].ToString().Trim();
                                saveProblemList[0].Problem_Description = saveProblemList[0].Problem_Description;
                                saveProblemList[0].Status = "Active";
                                saveProblemList[0].Is_Active = "N";
                                if (!saveProblemList[0].Reference_Source.Contains("Assessment"))
                                    saveProblemList[0].Reference_Source = saveProblemList[0].Reference_Source + "|" + "Assessment";
                                saveProblemList[0].Version_Year = "ICD_10";
                                saveProblemList[0].Modified_By = ClientSession.UserName;
                                saveProblemList[0].Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                saveProblemList[0].Date_Diagnosed = saveProblemList[0].Date_Diagnosed;
                                ulProbIDList.Add(saveProblemList[0].Id);

                                if (!probListToUpdate.Any(a => a.ICD == saveProblemList[0].ICD))
                                    probListToUpdate = probListToUpdate.Concat(saveProblemList).ToList();

                                sIncompleteList.Add(saveProblemList[0].ICD);
                            }
                        }
                    }
                }
                bool sCheck = false;
                string date_diagnosed = "";
                ulong iPrimaryID = 0;
                string sReference_Source = "";
                bool problemlistUpdate = false;
                if (sessionVitalsProblemList.Count > 0)
                {
                    IList<VitalsAssesment> ResultVitalsProblemList = sessionVitalsProblemList.Where(a => a.ICD_9 == Convert.ToString(oj[4])).ToList<VitalsAssesment>();
                    if (ResultVitalsProblemList.Count > 0 && Convert.ToInt32(oj[8]) != 0)
                        problemlistUpdate = true;
                }
                if (!problemlistUpdate)
                {
                    IList<string> ResultMultiMappingProblemList = sessionMultiMappingProblemList.Where(a => a.Split('~')[0] == Convert.ToString(oj[13])).ToList<string>();
                    if (ResultMultiMappingProblemList.Count > 0 && Convert.ToInt32(oj[8]) != 0)
                        problemlistUpdate = true;
                }

                if (Convert.ToInt32(oj[8]) == 0 && Convert.ToBoolean(oj[3]) && oj[0].ToString().Trim() != "Del")
                {
                    IList<ProblemList> ICDCodeList = sessionProblemList.Where(a => a.ICD == oj[4].ToString()).ToList();

                    if (ICDCodeList.Count > 0)
                    {
                        date_diagnosed = ICDCodeList[0].Date_Diagnosed;
                        sReference_Source = ICDCodeList[0].Reference_Source;
                        iPrimaryID = ICDCodeList[0].Id;
                        sCheck = true;
                        goto m;

                    }
                    ProblemList problemAdd = new ProblemList();
                    problemAdd.Encounter_ID = ClientSession.EncounterId;
                    problemAdd.Human_ID = ClientSession.HumanId;
                    problemAdd.Physician_ID = ClientSession.PhysicianId;
                    problemAdd.ICD = oj[4].ToString();
                    problemAdd.Problem_Description = oj[5].ToString();
                    problemAdd.Status = "Active";
                    problemAdd.Is_Active = "Y";
                    problemAdd.Reference_Source = "Assessment";
                    problemAdd.Version_Year = "ICD_10";

                    problemAdd.Created_By = ClientSession.UserName;
                    problemAdd.Created_Date_And_Time = UtilityManager.ConvertToUniversal();

                    if (oj[13] != null)
                        problemAdd.ICD_9 = oj[13].ToString();

                    if (oj[14] != null)
                        problemAdd.ICD_9_Description = oj[14].ToString();
                    probListToAdd.Add(problemAdd);

                m:
                    string bCheck = "";
                }

                if ((Convert.ToInt32(oj[8]) != 0 && Convert.ToBoolean(oj[3])) || (oj[0].ToString().Trim() == "Del" && Convert.ToInt32(oj[8]) != 0) || sCheck || problemlistUpdate)
                {

                    ProblemList problemUpdate = new ProblemList();
                    problemUpdate.Encounter_ID = ClientSession.EncounterId;
                    problemUpdate.Human_ID = ClientSession.HumanId;
                    problemUpdate.Physician_ID = ClientSession.PhysicianId;
                    problemUpdate.ICD = oj[4].ToString();
                    problemUpdate.Problem_Description = oj[5].ToString();
                    problemUpdate.Status = "Active";
                    problemUpdate.Is_Active = "Y";


                    problemUpdate.Version = Convert.ToInt32(oj[11]);
                    problemUpdate.Modified_By = ClientSession.UserName;
                    problemUpdate.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                    if (oj[13] != null)
                        problemUpdate.ICD_9 = oj[13].ToString();

                    if (oj[14] != null)
                        problemUpdate.ICD_9_Description = oj[14].ToString();


                    if (!sCheck)
                    {
                        problemUpdate.Id = Convert.ToUInt32(oj[8]);
                        IList<ProblemList> ReferenceList = sessionProblemList.Where(a => a.Id == Convert.ToUInt32(oj[8])).ToList();
                        if (ReferenceList.Count > 0)
                        {
                            if (oj[0].ToString().Trim() == "Del")
                            {
                                if (!string.IsNullOrEmpty(ReferenceList[0].Reference_Source) && !ReferenceList[0].Reference_Source.Contains("Deleted"))
                                    problemUpdate.Reference_Source = ReferenceList[0].Reference_Source + "|Deleted";
                                else
                                    problemUpdate.Reference_Source = ReferenceList[0].Reference_Source;
                            }
                            else
                            {
                                if (!ReferenceList[0].Reference_Source.Contains("Assessment"))
                                    problemUpdate.Reference_Source += string.IsNullOrEmpty(ReferenceList[0].Reference_Source) ? "Assessment" : "|Assessment";
                                else
                                    problemUpdate.Reference_Source = ReferenceList[0].Reference_Source.Replace("|Deleted", "").Replace("Deleted|", "").Replace("Deleted", "");

                                problemUpdate.Reference_Source = ReferenceList[0].Reference_Source.Replace("|Deleted", "").Replace("Deleted|", "").Replace("Deleted", "");
                            }

                            problemUpdate.Date_Diagnosed = ReferenceList[0].Date_Diagnosed;


                            if (oj[13] == null)
                                problemUpdate.ICD_9 = ReferenceList[0].ICD_9;
                            if (oj[14] == null)
                                problemUpdate.ICD_9_Description = ReferenceList[0].ICD_9_Description;
                            problemUpdate.Created_By = ReferenceList[0].Created_By;
                            problemUpdate.Created_Date_And_Time = ReferenceList[0].Created_Date_And_Time;
                        }

                    }

                    else if (sCheck)
                    {
                        problemUpdate.Date_Diagnosed = date_diagnosed;
                        problemUpdate.Id = iPrimaryID;
                        problemUpdate.Reference_Source = sReference_Source.Replace("|Deleted", "").Replace("Deleted|", "").Replace("Deleted", "");
                    }

                    if (problemUpdate.Id == 0 || problemUpdate.Reference_Source == "")
                    {
                        problemUpdate.Id = Convert.ToUInt32(oj[8]);
                        IList<ProblemList> ReferenceList = sessionProblemList.Where(a => a.Id == Convert.ToUInt32(oj[8])).ToList();
                        if (ReferenceList.Count > 0)
                        {
                            if (oj[0].ToString().Trim() == "Del")
                            {
                                if (!string.IsNullOrEmpty(ReferenceList[0].Reference_Source) && !ReferenceList[0].Reference_Source.Contains("Deleted"))
                                    problemUpdate.Reference_Source = ReferenceList[0].Reference_Source + "|Deleted";
                                else
                                    problemUpdate.Reference_Source = ReferenceList[0].Reference_Source;
                            }
                            else
                            {
                                if (!ReferenceList[0].Reference_Source.Contains("Assessment"))
                                    problemUpdate.Reference_Source += string.IsNullOrEmpty(ReferenceList[0].Reference_Source) ? "Assessment" : "|Assessment";
                                else
                                    problemUpdate.Reference_Source = ReferenceList[0].Reference_Source.Replace("|Deleted", "").Replace("Deleted|", "").Replace("Deleted", "");

                                problemUpdate.Reference_Source = ReferenceList[0].Reference_Source.Replace("|Deleted", "").Replace("Deleted|", "").Replace("Deleted", "");
                            }


                            problemUpdate.Date_Diagnosed = ReferenceList[0].Date_Diagnosed;


                            if (oj[13] == null)
                                problemUpdate.ICD_9 = ReferenceList[0].ICD_9;
                            if (oj[14] == null)
                                problemUpdate.ICD_9_Description = ReferenceList[0].ICD_9_Description;
                            problemUpdate.Created_By = ReferenceList[0].Created_By;
                            problemUpdate.Created_Date_And_Time = ReferenceList[0].Created_Date_And_Time;
                        }

                    }
                    if (problemUpdate.Reference_Source == "")
                        problemUpdate.Reference_Source = "Assessment";

                    problemUpdate.Version_Year = "ICD_10";
                    if (!probListToUpdate.Any(a => a.Id == problemUpdate.Id))
                        probListToUpdate.Add(problemUpdate);

                }
                // string test = oj[7].ToString();
            }


            IList<GeneralNotes> GeneralNotesList = new List<GeneralNotes>();

            if (HttpContext.Current.Session["GeneralNotes"] != null)
            {
                GeneralNotesList = (IList<GeneralNotes>)HttpContext.Current.Session["GeneralNotes"];
            }

            if (sGeneralNotes.Trim() != string.Empty && GeneralNotesList.Count == 0)
            {
                objGeneralNotes = new GeneralNotes();
                objGeneralNotes.Created_By = ClientSession.UserName;
                objGeneralNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objGeneralNotes.Encounter_ID = ClientSession.EncounterId;
                objGeneralNotes.Human_ID = ClientSession.HumanId;
                objGeneralNotes.Name_Of_The_Field = string.Empty;
                objGeneralNotes.Notes = sGeneralNotes;
                objGeneralNotes.Parent_Field = "Selected Assessment";
            }
            else if (GeneralNotesList.Count == 1 && sGeneralNotes.Trim() != GeneralNotesList[0].Notes.Trim())
            {
                objGeneralNotes = GeneralNotesList[0];
                objGeneralNotes.Modified_By = ClientSession.UserName;
                objGeneralNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();

                if (objGeneralNotes.Id == 0)
                {
                    objGeneralNotes = new GeneralNotes();
                    objGeneralNotes.Created_By = ClientSession.UserName;
                    objGeneralNotes.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    objGeneralNotes.Encounter_ID = ClientSession.EncounterId;
                    objGeneralNotes.Human_ID = ClientSession.HumanId;
                    objGeneralNotes.Name_Of_The_Field = string.Empty;
                    objGeneralNotes.Notes = sGeneralNotes;
                    objGeneralNotes.Parent_Field = "Selected Assessment";

                }
                else if (string.Compare(sGeneralNotes, objGeneralNotes.Notes, true) != 0)
                {
                    objGeneralNotes.Notes = sGeneralNotes;
                }
            }
            else
            {
                foreach (var objGeneralNote in GeneralNotesList)
                {
                    objGeneralNotes = objGeneralNote;
                    objGeneralNotes.Modified_By = ClientSession.UserName;
                    objGeneralNotes.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();


                    if (sGeneralNotes != objGeneralNote.Notes)
                    {
                        objGeneralNotes.Notes = sGeneralNotes;
                    }
                }
            }


            //if (sPrimaryIcd != "" || strIcd != "")
            //{
            //    SaveTreatmentPlan = new TreatmentPlan();
            //    SaveTreatmentPlan.Encounter_Id = ClientSession.EncounterId;
            //    SaveTreatmentPlan.Human_ID = ClientSession.HumanId;
            //    SaveTreatmentPlan.Physician_Id = ClientSession.PhysicianId;
            //    SaveTreatmentPlan.Plan = sPrimaryIcd + strIcd;
            //    SaveTreatmentPlan.Created_By = ClientSession.EandMCodingICD;
            //    SaveTreatmentPlan.Created_Date_And_Time = UtilityManager.ConvertToUniversal(DateTime.Now);
            //}

            AssessmentManager objAssessmentManager = new AssessmentManager();


            IList<EandMCodingICD> eandmicdinsert = new List<EandMCodingICD>();
            IList<EandMCodingICD> eandmicd = new List<EandMCodingICD>();
            IList<EandMCodingICD> eandmicddelete = new List<EandMCodingICD>();
            //Cap - 1280
            IList<EandMCodingICD> eanmicdoverallicd = new List<EandMCodingICD>();
            AllICD_9Manager objAllICDMgrGetICD = new AllICD_9Manager();

            EandMCodingICDManager objeandm = new EandMCodingICDManager();
            eandmicd= objeandm.GetEandMcodingICDListbyEncounterID(ClientSession.EncounterId);
            //Cap - 1280
            IList<EandMCodingICD> iEandMICDList = new List<EandMCodingICD>();
            iEandMICDList = (from m in eandmicd where m.Source != "ASSESSMENT" select m).ToList<EandMCodingICD>();
            eandmicddelete = eandmicd;
            IList<Assessment> lstass = new List<Assessment>();
            lstass = assementInsertList.Concat(assessmentListToUpdate).ToList<Assessment>();

            //if (eandmicd.Count > 0)
            // {
            //    eandmicd = eandmicd.OrderBy(a => Convert.ToUInt32(a.Sequence.Replace("A", ""))).ToList<EandMCodingICD>();
            //    int seqence = Convert.ToInt32(eandmicd[eandmicd.Count - 1].Sequence.Replace("A",""))+1;
            //    for (int k=0;k< lstass.Count;k++)
            //    {
            //        EandMCodingICD obj = new EandMCodingICD();
            //        obj.ICD = lstass[k].ICD;
            //        obj.ICD_Description = lstass[k].ICD_Description;
            //        obj.Is_Delete = "N";
            //        obj.Human_ID = lstass[k].Human_ID;
            //        obj.Encounter_ID = lstass[k].Encounter_ID;
            //        obj.Source = "ASSESSMENT";
            //        if (lstass[k].Primary_Diagnosis.ToUpper() == "Y")
            //            obj.ICD_Category = "Primary";
            //        else
            //            obj.ICD_Category = "None";

            //        IList<EandMCodingICD> eandmicdtemp = (from m in eandmicd where m.ICD.Trim() == lstass[k].ICD.Trim() select m).ToList<EandMCodingICD>();
            //        if(eandmicdtemp.Count>0)
            //        {
            //            obj.Sequence = eandmicdtemp[0].Sequence;

            //        }
            //        else
            //        {
            //            obj.Sequence = "A" + seqence.ToString();
            //            seqence++;
            //        }

            //        obj.Created_By = ClientSession.UserName;
            //        obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        eandmicdinsert.Add(obj);



            //    }
            //    eandmicddelete = eandmicd;

            //}
            //else
            // {
            for (int k = 0; k < lstass.Count; k++)
            {
                //Jira Cap - 1900,984
                IList<string> lstStatus_Assessment = new List<string>();
                if (System.Configuration.ConfigurationSettings.AppSettings["ServProcCodeExclutionStatus"] != null)
                    lstStatus_Assessment = System.Configuration.ConfigurationSettings.AppSettings["ServProcCodeExclutionStatus"].ToString().Split(',');

                if (!(lstStatus_Assessment.Contains(lstass[k].Assessment_Status)))
                {
                EandMCodingICD obj = new EandMCodingICD();
                obj.ICD = lstass[k].ICD;
                obj.ICD_Description = lstass[k].ICD_Description;
                obj.Is_Delete = "N";
                obj.Human_ID = lstass[k].Human_ID;
                obj.Encounter_ID = lstass[k].Encounter_ID;
                obj.Source = "ASSESSMENT";
                if (lstass[k].Primary_Diagnosis.ToUpper() == "Y")
                {
                    obj.ICD_Category = "Primary";
                }
                else
                {
                    obj.ICD_Category = "None";
                }

                obj.Sequence = "";
                obj.Created_By = ClientSession.UserName;
                obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                eanmicdoverallicd.Add(obj);
                // break;
                }



            }
            //Cap - 1280
            eanmicdoverallicd = eanmicdoverallicd.Concat(iEandMICDList).ToList<EandMCodingICD>();


            List<string> lsteanmdprimaryicdICDs = new List<string>();
            List<string> lstassprimaryicdICDs = new List<string>();

            List<EandMCodingICD> lstpriamryICDs = (from m in eanmicdoverallicd where m.ICD_Category == "Primary" select m).ToList<EandMCodingICD>();
            if (lstpriamryICDs.Count > 1)
            {
                lsteanmdprimaryicdICDs = (from m in lstpriamryICDs where m.Source == "EMICD" select m.ICD).ToList<string>();

            }
            lstassprimaryicdICDs = (from m in lstpriamryICDs where m.Source != "EMICD" select m.ICD).ToList<string>();
            List<string> lstICDs = (from m in eanmicdoverallicd where m.ICD_Category != "Primary" select m.ICD).ToList<string>();

            IList<AllICD_9> iListIcd = new List<AllICD_9>();
            IList<AllICD_9> iListIcdbyorder = new List<AllICD_9>();
            IList<AllICD_9> iListIcdtemp = new List<AllICD_9>();
            IList<AllICD_9> iListIcdzerohcc = new List<AllICD_9>();
            AllICD_9Manager objAllICDMngr = new AllICD_9Manager();
            if (lsteanmdprimaryicdICDs.Count > 0)
            {
                lstICDs.AddRange(lsteanmdprimaryicdICDs);

            }
            iListIcd = objAllICDMngr.GetICDList(lstICDs);
            //Cap - 1815
            //iListIcdtemp = (from m in iListIcd where m.HCC_Value > 0 select m).GroupBy(a => a.HCC_Value).Select(x => x.First()).ToList<AllICD_9>();
            iListIcdtemp = (from m in iListIcd where m.HCC_Value > 0 select m).GroupBy(a => new { a.HCC_Value, a.HCC_Category }).Select(x => x.First()).ToList<AllICD_9>();

            iListIcdzerohcc = (from m in iListIcd where m.HCC_Value.ToString() == "0" select m).OrderBy(a => a.ICD_9).ToList<AllICD_9>();

            iListIcdbyorder = iListIcdtemp;
            iListIcdbyorder = iListIcdbyorder.Concat(iListIcdzerohcc).ToList<AllICD_9>();
            IList<AllICD_9> iListIcdwithoutzerohcc = new List<AllICD_9>();
            //Cap - 1806
            //iListIcdwithoutzerohcc = iListIcd.Except(iListIcdbyorder).ToList<AllICD_9>().OrderByDescending(m => m.HCC_Value).OrderBy(n => n.ICD_9).ToList<AllICD_9>();
            //Cap - 1815
            //iListIcdwithoutzerohcc = iListIcd.Except(iListIcdbyorder).ToList<AllICD_9>().OrderByDescending(m => m.HCC_Value).ThenBy(n => n.ICD_9).ToList<AllICD_9>();
            iListIcdwithoutzerohcc = iListIcd.Except(iListIcdbyorder).ToList<AllICD_9>().OrderByDescending(m => m.HCC_Value).ThenByDescending(m => m.HCC_Category).ThenBy(n => n.ICD_9).ToList<AllICD_9>();
            iListIcdbyorder = iListIcdbyorder.Concat(iListIcdwithoutzerohcc).ToList<AllICD_9>();


            // iListIcdwithoutzerohcc = iListIcd.Except(iListIcdzerohcc).ToList<AllICD_9>();
            //while (iListIcd.Count != (iListIcdbyorder.Count + iListIcdzerohcc.Count))
            //while (iListIcdwithoutzerohcc.Count != (iListIcdbyorder.Count))

            //{
            //    iListIcdtemp = iListIcdwithoutzerohcc.Except(iListIcdbyorder).ToList<AllICD_9>().OrderByDescending(m => m.HCC_Value).GroupBy(a => a.HCC_Value).Select(x => x.First()).ToList<AllICD_9>();
            //    iListIcdbyorder = iListIcdbyorder.Concat(iListIcdtemp).ToList<AllICD_9>();
            //}
            //iListIcdbyorder = iListIcdbyorder.Concat(iListIcdzerohcc).ToList<AllICD_9>();
            int flag = 0;
            for (int k = 0; k < eanmicdoverallicd.Count; k++)
            {
                //Cap - 1804
                //if (eanmicdoverallicd[k].ICD_Category.ToUpper() == "PRIMARY" && (eanmicdoverallicd[k].ICD == lstassprimaryicdICDs[0].Trim().ToString().Trim()))
                if (eanmicdoverallicd[k].ICD_Category.ToUpper() == "PRIMARY" && (eanmicdoverallicd[k].ICD.Trim() == lstassprimaryicdICDs[0].Trim().ToString().Trim()))
                {
                    flag = 1;
                    EandMCodingICD obj = new EandMCodingICD();
                    //Cap - 1280
                    //obj.ICD = lstass[k].ICD;
                    //obj.ICD_Description = lstass[k].ICD_Description;
                    //obj.Is_Delete = "N";
                    //obj.Human_ID = lstass[k].Human_ID;
                    //obj.Encounter_ID = lstass[k].Encounter_ID;
                    //obj.Source = "ASSESSMENT";
                    //obj.ICD_Category = "None";
                    //obj.Sequence = "A" + SEQ.ToString();
                    obj.ICD = eanmicdoverallicd[k].ICD;
                    obj.ICD_Description = eanmicdoverallicd[k].ICD_Description;
                    obj.Is_Delete = eanmicdoverallicd[k].Is_Delete;
                    obj.Human_ID = eanmicdoverallicd[k].Human_ID;
                    obj.Encounter_ID = eanmicdoverallicd[k].Encounter_ID;
                    obj.Source = eanmicdoverallicd[k].Source;
                    obj.ICD_Category = eanmicdoverallicd[k].ICD_Category;
                    obj.Sequence = "A1";
                    obj.Created_By = ClientSession.UserName;
                    obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    eandmicdinsert.Add(obj);

                    break;

                }

            }

            int SEQ = 2;
           
            if (flag==1)
            {
                SEQ = 2;
            }
            else
            {
                SEQ = 1;
            }
            for (int k = 0; k < iListIcdbyorder.Count; k++)
            {
                IList<EandMCodingICD> eanmicdtemp = new List<EandMCodingICD>();
                eanmicdtemp = (from m in eanmicdoverallicd where m.ICD.Trim() == iListIcdbyorder[k].ICD_9.Trim() select m).ToList<EandMCodingICD>();
                if (eanmicdtemp.Count > 0)
                {
                    EandMCodingICD objFinal = new EandMCodingICD();
                    objFinal.ICD = eanmicdtemp[0].ICD;
                    objFinal.ICD_Description = eanmicdtemp[0].ICD_Description;
                    objFinal.Is_Delete = eanmicdtemp[0].Is_Delete;
                    objFinal.Human_ID = eanmicdtemp[0].Human_ID;
                    objFinal.Encounter_ID = eanmicdtemp[0].Encounter_ID;
                    objFinal.Source = eanmicdtemp[0].Source;
                    objFinal.ICD_Category = "None";
                    objFinal.Sequence = "A" + SEQ.ToString();
                    objFinal.Created_By = ClientSession.UserName;
                    objFinal.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                    eandmicdinsert.Add(objFinal);
                    SEQ++;
                }


            }

            //Cap - 1280

            // }


            //int SEQ = 2;
            //for (int k = 0; k < lstass.Count; k++)
            //{
            //    if (lstass[k].Primary_Diagnosis.ToUpper() != "Y")
            //    {
            //        EandMCodingICD obj = new EandMCodingICD();
            //        obj.ICD = lstass[k].ICD;
            //        obj.ICD_Description = lstass[k].ICD_Description;
            //        obj.Is_Delete = "N";
            //        obj.Human_ID = lstass[k].Human_ID;
            //        obj.Encounter_ID = lstass[k].Encounter_ID;
            //        obj.Source = "ASSESSMENT";
            //        obj.ICD_Category = "None";
            //        obj.Sequence = "A" + SEQ.ToString();
            //        obj.Created_By = ClientSession.UserName;
            //        obj.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
            //        eandmicdinsert.Add(obj);
            //        SEQ++;


            //}

            //}
            // }






            FillAssessment assessmentLoadList = objAssessmentManager.BatchOperationsToAssessment(assementInsertList.ToArray<Assessment>(),
                       assessmentListToUpdate.ToArray<Assessment>(), assessmentListToDelete.ToArray<Assessment>(),
                        probListToAdd.ToArray<ProblemList>(), probListToUpdate.ToArray<ProblemList>(),
                       probListIdToDelete.ToArray<ProblemList>(), string.Empty,
                       objGeneralNotes, SaveTreatmentPlan, ClientSession.UserName, ClientSession.EncounterId, ClientSession.HumanId,
                       ClientSession.PhysicianId, sMacraICDChkList, sIs_Assessment_CopyPrevious, sLocalTime, ClientSession.LegalOrg, eandmicdinsert,eandmicddelete);


            //FillAssessment assessmentLoadList = objAssessmentManager.BatchOperationsToAssessment(assementInsertList.ToArray<Assessment>(),
            //          assessmentListToUpdate.ToArray<Assessment>(), assessmentListToDelete.ToArray<Assessment>(),
            //           probListToAdd.ToArray<ProblemList>(), probListToUpdate.ToArray<ProblemList>(),
            //          probListIdToDelete.ToArray<ProblemList>(), string.Empty,
            //          objGeneralNotes, SaveTreatmentPlan, ClientSession.UserName, ClientSession.EncounterId, ClientSession.HumanId, ClientSession.FillPatientChart.PatChartList[0].Is_Sent_To_RCopia, ClientSession.PhysicianId, sMacraICDChkList, sIs_Assessment_CopyPrevious);


            HttpContext.Current.Session["Is_Assessment_CopyPrevious"] = "No";
            for (int i = 0; i < assessmentLoadList.Assessment.Count; i++)
            {
                if (assessmentLoadList.Assessment[i].Diagnosis_Source.ToUpper() == "VITALS|DELETED")
                    continue;
                IList<ProblemList> problemList = assessmentLoadList.Problem_List.Where(a => a.ICD == assessmentLoadList.Assessment[i].ICD).ToList<ProblemList>();

                if (problemList.Count == 0)
                    problemList = assessmentLoadList.Problem_List.Where(a => a.ICD == assessmentLoadList.Assessment[i].ICD_9).ToList<ProblemList>();
                if (problemList.Count > 0)
                {
                    assessmentLoadList.Assessment[i].Internal_Property_ProblemListID = problemList[0].Id;
                    assessmentLoadList.Assessment[i].Internal_Property_ProblemListVersion = problemList[0].Version;
                }
            }

            IList<ProblemList> SummaryBarRefreshlist = new List<ProblemList>();
            DateTime CurrentDOS = DateTime.MinValue;
            IList<string> ilsAssessmentTagList = new List<string>();

            ilsAssessmentTagList.Add("ProblemListList");



            IList<object> ilstAsshumanBlobFinal = new List<object>();

            ilstAsshumanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilsAssessmentTagList);

            if (ilstAsshumanBlobFinal != null && ilstAsshumanBlobFinal.Count > 0)
            {
                if (ilstAsshumanBlobFinal[0] != null)
                {


                    for (int iCount = 0; iCount < ((IList<object>)ilstAsshumanBlobFinal[0]).Count; iCount++)
                    {
                        SummaryBarRefreshlist.Add((ProblemList)((IList<object>)ilstAsshumanBlobFinal[0])[iCount]);
                    }

                }
            }
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            // string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    //itemDoc.Load(XmlTe
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("ProblemListList")[0] != null)
            //        {
            //            xmlTagName = itemDoc.GetElementsByTagName("ProblemListList")[0].ChildNodes;

            //            if (xmlTagName.Count > 0)
            //            {
            //                for (int j = 0; j < xmlTagName.Count; j++)
            //                {

            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(ProblemList));
            //                    ProblemList ProblemList = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as ProblemList;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((ProblemList)ProblemList).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name == nodevalue.Name)
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(ProblemList, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(ProblemList, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(ProblemList, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(ProblemList, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(ProblemList, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    SummaryBarRefreshlist.Add(ProblemList);
            //                }
            //            }
            //        }
            //        fs.Close();
            //        fs.Dispose();
            //    }

            //}
            if (ClientSession.FillEncounterandWFObject != null)
            {
                if (ClientSession.FillEncounterandWFObject.EncRecord != null)
                {
                    ClientSession.FillEncounterandWFObject.EncRecord.is_assessment_saved = "Y";
                    ClientSession.FillEncounterandWFObject.EncRecord.is_serviceprocedure_saved = "N";
                    IList<Encounter> lst = new List<Encounter>();
                    IList<Encounter> lsttemp = new List<Encounter>();
                    lst.Add(ClientSession.FillEncounterandWFObject.EncRecord);
                    EncounterManager obj = new EncounterManager();
                    obj.SaveUpdateDelete_DBAndXML_WithTransaction(ref lsttemp, ref lst, null, string.Empty, true, false, ClientSession.FillEncounterandWFObject.EncRecord.Id, string.Empty);
                    ClientSession.FillEncounterandWFObject.EncRecord = lst[0];
                }
            }

            string[] strarray = new string[2];
            UtilityManager objmngr = new UtilityManager();
            var toolTipProblem = string.Empty;
            string strProblmlist = objmngr.GetProblemList(SummaryBarRefreshlist, out toolTipProblem);
            strarray[0] = strProblmlist;
            strarray[1] = toolTipProblem;

            if (assessmentLoadList.Assessment.Count > 0)
                assessmentLoadList.Assessment = assessmentLoadList.Assessment.Where(a => a.Diagnosis_Source != "VITALS|DELETED" && a.Version_Year == "ICD_10").ToList();
            IList<string> lstParent_ICD = new List<string>();
            lstParent_ICD = assessmentLoadList.Assessment.Select(a => a.Parent_ICD.Trim()).Distinct().ToList<string>();


            HttpContext.Current.Session["ProblemList"] = assessmentLoadList.Problem_List;
            HttpContext.Current.Session["VitalsList"] = assessmentLoadList.VitalsBasedICD_List;
            HttpContext.Current.Session["GeneralNotes"] = assessmentLoadList.General_NotesCurrentList;

            var ListAssessment = assessmentLoadList.Assessment.Select(a => new { ICDCode = a.ICD, ICDDescription = a.ICD_Description, IsPrimary = a.Primary_Diagnosis, ProblemListID = a.Internal_Property_ProblemListID, Notes = a.Assessment_Notes, AssessmentID = a.Id, iVersion = a.Version, iProblemListVersion = a.Internal_Property_ProblemListVersion, CheckBoxCheck = a.Chronic_Problem, StatusSelected = a.Assessment_Status, ICD9Code = a.ICD_9, ICD9Desc = a.ICD_9_Description, ParentICD = a.Parent_ICD, Created_by = a.Created_By, Created_date = a.Created_Date_And_Time.ToString(), Updated = "N", Orig_Status = a.Assessment_Status });
            string json = new JavaScriptSerializer().Serialize(ListAssessment);

            string jsonArray = new JavaScriptSerializer().Serialize(strarray);

            string jsonIncompleteProblemList = new JavaScriptSerializer().Serialize(sIncompleteList);

            //Jira CAP-1183
            //string jsonIsAssessmentRAFUpdate = new JavaScriptSerializer().Serialize(System.Configuration.ConfigurationSettings.AppSettings["Is_AssessmentRAFUpdate"]);

            string jsonIsAssessmentRAFUpdate = new JavaScriptSerializer().Serialize(System.Configuration.ConfigurationSettings.AppSettings["Is_AssessmentRAFUpdate_" + ClientSession.LegalOrg]);

            string jsons = "{\"AssessmentList\" :" + json + "," + "\"ProblemList\" :" + jsonArray + "," + "\"InCompleteProblemList\" :" + jsonIncompleteProblemList + "," + "\"IsAssessmentRAFUpdate\" :" + jsonIsAssessmentRAFUpdate +
                             "}";
            return jsons;
        }

        [WebMethod(EnableSession = true)]
        public static string SearchQuestionnaire(string sICD)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<QuestionsDTO> QuestionList = new List<QuestionsDTO>();
            AllICD_9Manager objAllICDMgr = new AllICD_9Manager();
            string SourceTable = "ALLICD";
            string sMutuallyExclusive = "";
            string sQuestionList = "";

            if (sICD.Contains('-'))
            {
                if (sICD.Split('-')[1].Trim() == "Y")
                {
                    SourceTable = "ASSOCIATEDPRIMARYICD";
                    sICD = sICD.Split('-')[0];
                }

            }
            if (sICD != "")
            {
                QuestionList = objAllICDMgr.GetQuestionMasterRecord(sICD.Trim(), ref sMutuallyExclusive, ref SourceTable);

                if (QuestionList.Count > 0)
                {
                    var ListQuestionaire = QuestionList.Select(a => new { ICDCode = a.ICD_9, ICDDescription = a.Diagnosis_Description, LeafNode = a.Leaf_Node, MutuallyExclusive = sMutuallyExclusive });
                    string json = new JavaScriptSerializer().Serialize(ListQuestionaire);
                    sQuestionList = "{\"Questionnaire\" :" + json + "}";
                }
            }
            return sQuestionList;
        }

        [WebMethod(EnableSession = true)]
        public static string SearchQuestionnairelIST(string sICDS)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<QuestionsDTO> QuestionList = new List<QuestionsDTO>();
            AllICD_9Manager objAllICDMgr = new AllICD_9Manager();
            string SourceTable = "ALLICD";
            string sMutuallyExclusive = "";
            string sQuestionList = "";
            if (sICDS.Split('-')[1].Trim() == "Y")
                SourceTable = "ASSOCIATEDPRIMARYICD";
            if (sICDS != "")
            {
                QuestionList = objAllICDMgr.GetQuestionMasterRecord(sICDS.Split('-')[0].Trim(), ref sMutuallyExclusive, ref SourceTable);

                if (QuestionList.Count > 0)
                {
                    var ListQuestionaire = QuestionList.Select(a => new { ICDCode = a.ICD_9, ICDDescription = a.Diagnosis_Description, LeafNode = a.Leaf_Node, MutuallyExclusive = sMutuallyExclusive });
                    string json = new JavaScriptSerializer().Serialize(ListQuestionaire);

                    sQuestionList = "{\"Questionnaire\" :" + json + "}";
                }
            }

            return sQuestionList;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SearchDescrptionText(string text)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            AllICD_9Manager objAllICD_9Manager = new AllICD_9Manager();
            string data = text.Split('|')[0];
            string type = text.Split('|')[1];
            string ICDType = text.Split('|')[2];
            string sTodate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
            if (sTodate.Equals("0001-01-01"))//BugID:53392
                sTodate = DateTime.Now.ToString("yyyy-MM-dd");
            string[] ResultDescpList = objAllICD_9Manager.GetDescriptionList(data, type, ICDType, sTodate).ToArray();

            var jsonString = JsonConvert.SerializeObject(ResultDescpList);
            return jsonString;
            //return new JavaScriptSerializer().Serialize(ResultDescpList);
        }


        [WebMethod(EnableSession = true)]
        public static string GetAssociatedICDList(string sAssociateICD)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sAsscoiateICDS = "";
            IList<AssociatedPrimaryICD> AssociaPrimaryList = new List<AssociatedPrimaryICD>();
            AssociatedPrimaryICDManager objAssessmentQuestionsMgr = new AssociatedPrimaryICDManager();

            AssociaPrimaryList = objAssessmentQuestionsMgr.GetAssociatedICDCodes(sAssociateICD.Split('~')[0]);

            if (AssociaPrimaryList.Count > 0)
            {
                var ListQuestionaire = AssociaPrimaryList.Select(a => new { ICDCode = a.Associated_ICD_9, ICDDescription = a.Prefix.Trim() == string.Empty ? a.Associated_ICD_9_Description : a.Prefix + " " + a.Associated_ICD_9_Description, LeafNode = a.Leaf_Node, MutuallyExclusive = a.Mutually_Exclusive });
                string json = new JavaScriptSerializer().Serialize(ListQuestionaire);

                sAsscoiateICDS = "{\"Questionnaire\" :" + json + "}";

            }


            return sAsscoiateICDS;
        }


        [WebMethod(EnableSession = true)]
        public static string GetFormviewICDs(string sFormviewICD)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sAsscoiateICDS = "", Ass_Status = string.Empty;
            IList<AssociatedPrimaryICD> AssociaPrimaryList = new List<AssociatedPrimaryICD>();
            AssociatedPrimaryICDManager objAssessmentQuestionsMgr = new AssociatedPrimaryICDManager();
            IDictionary<string, string> idicDefaultStatuslst = (Dictionary<string, string>)HttpContext.Current.Session["DefaultStatusList"];
            if (idicDefaultStatuslst != null && idicDefaultStatuslst.Count > 0)
            {
                if (idicDefaultStatuslst.ContainsKey("ASSESSMENT"))
                    Ass_Status = idicDefaultStatuslst["ASSESSMENT"];
            }
            AssociaPrimaryList = objAssessmentQuestionsMgr.GetAllAssociatedICDCodes(sFormviewICD.Split('|').Select(a => a.Split('~')[0]).ToArray());

            if (AssociaPrimaryList.Count > 0)
            {
                var Record1 = (from b in sFormviewICD.Split('|').ToArray() where AssociaPrimaryList.Any(a => a.ICD_9 == b.Split('~')[0]) select b).ToList();

                AssociaPrimaryList = AssociaPrimaryList.Where(a => a.ICD_9 == Record1[0].Split('~')[0]).ToList();
                var ListQuestionaire = AssociaPrimaryList.Select(a => new { ICDCode = a.Associated_ICD_9, ICDDescription = a.Prefix.Trim() == string.Empty ? a.Associated_ICD_9_Description : a.Prefix + " " + a.Associated_ICD_9_Description, LeafNode = a.Leaf_Node, MutuallyExclusive = a.Mutually_Exclusive, ICDCodeDesc = Record1[0].Replace("~", "-") });
                string json = new JavaScriptSerializer().Serialize(ListQuestionaire);

                var NoRecord1 = (from b in sFormviewICD.Split('|').ToArray() where !AssociaPrimaryList.Any(a => a.ICD_9 == b.Split('~')[0]) select b).ToList();
                var NoResultRecord1 = NoRecord1.Select(a => new { ICDCode = a.Split('~')[0], ICDDescription = a.Split('~')[1], AssessmentID = 0, iVersion = 0, iProblemListVersion = 0, ProblemListID = 0, Notes = "", Created_by = "", Created_date = "", Updated = "Y", StatusSelected = Ass_Status });
                string jsonNoQuestionnaire = new JavaScriptSerializer().Serialize(NoResultRecord1);
                IList<string> sMainQuestion = new List<string>();
                for (int i = 0; i < Record1.Count; i++)
                {
                    if (i > 0)
                        sMainQuestion.Add(Record1[i] + "-" + "Y");
                }

                string jsonMainQuestionnaire = new JavaScriptSerializer().Serialize(sMainQuestion);
                sAsscoiateICDS = "{\"Questionnaire\" :" + json + "," +
                               "\"NoQuestionnaire\":" + jsonNoQuestionnaire +
                               "," + "\"MainQuestionnaire\":" + jsonMainQuestionnaire + "}";

            }
            else
            {
                var Record1 = (from b in sFormviewICD.Split('|').ToArray() where !AssociaPrimaryList.Any(a => a.ICD_9 == b.Split('~')[0]) select b).ToList();
                var ResultRecord1 = Record1.Select(a => new { ICDCode = a.Split('~')[0], ICDDescription = a.Split('~')[1], AssessmentID = 0, iVersion = 0, iProblemListVersion = 0, ProblemListID = 0, Notes = "", Created_by = "", Created_date = "", Updated = "Y", StatusSelected = Ass_Status });
                string json = new JavaScriptSerializer().Serialize(ResultRecord1);

                sAsscoiateICDS = "{\"NoQuestionnaire\" :" + json + "}";
            }


            return sAsscoiateICDS;
        }



        [WebMethod(EnableSession = true)]
        public static string GetICD10CodeDesc(string ICDDesc)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sICD10Codess = "";
            IList<ICD9ICD10Mapping> ICD10ICDDescList = new List<ICD9ICD10Mapping>();
            AllICD_9Manager objAllICDsMgr = new AllICD_9Manager();

            ICD10ICDDescList = objAllICDsMgr.GetCodesDesc(ICDDesc);

            if (ICD10ICDDescList.Count > 1)
            {
                var ResultRecord1 = ICD10ICDDescList.Select(a => new { ICDCode = a.ICD10, ICDDescription = a.Long_Description });
                string json = new JavaScriptSerializer().Serialize(ResultRecord1);

                sICD10Codess = "{\"ICD10CodeDescMulti\" :" + json + "}";
            }
            else
            {
                var ResultRecord1 = ICD10ICDDescList.Select(a => new { ICDCode = a.ICD10, ICDDescription = a.Long_Description });
                string json = new JavaScriptSerializer().Serialize(ResultRecord1);

                sICD10Codess = "{\"ICD10CodeDesc\" :" + json + "}";
            }


            return sICD10Codess;
        }

        [WebMethod(EnableSession = true)]
        public static string RefershTable()
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

        [WebMethod(EnableSession = true)]
        public static string ColorCodingforMutuallyExclusive(object[] ICDCodes)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IDictionary<string, IList<AllICDColorCoding>> icdGroupDictionairy = new Dictionary<string, IList<AllICDColorCoding>>();
            int arrayIndex = 0;
            string[] ColorList = new string[] { "#0000FF", "#008B8B", "#B8860B", "#8B008B", "#FF8C00", "#483D8B", "#FF1493", "#800000", "#FF00FF", "#CD5C5C", "#4B0082", "#006400", "#8A2BE2", "#A52A2A", "#5F9EA0", "#D2691E", "#FF7F50", "#6495ED", "#DC143C", "#00008B", "#00FFFF", "#3F00FF", "#097969", "#228B22", "#808000", "#CC5500", "#FF4433", "#DE3163", "#800080", "#5D3FD3", "#FF3131", "#93C572", "#F4BB44", "#B4C424", "#702963", "#7F00FF", "#F88379", "#DA70D6", "#E30B5C", "#FFBF00" };
            //ColorList[]
            string sColorCoding = "";
            string sICD = "";
            for (int i = 0; i < ICDCodes.Count(); i++)
            {
                if ((ICDCodes[i]).ToString() != "0000")
                {
                    if (sICD == string.Empty)
                        sICD = ICDCodes[i].ToString();
                    else
                        sICD += '|' + ICDCodes[i].ToString();
                }
            }
            IList<string> pblmListParentICD = new List<string>();
            AllICD_9Manager objAllIcdMngr = new AllICD_9Manager();
            pblmListParentICD = objAllIcdMngr.TakeParentICDLeafandMutuallyExclusive(sICD);

            IList<string> ResultParentICD = pblmListParentICD;

            IList<string> ResultColorCoding = new List<string>();
            bool isColorApplied = false;
            IList<string> ResultAlreadyColored = new List<string>();

            for (int iCount = 0; iCount < ResultParentICD.Count; iCount++)
            {
                if (ResultParentICD[iCount].ToString().Split('!').Length > 1 && ResultParentICD[iCount].ToString().Split('!')[1] != string.Empty)
                {
                    isColorApplied = false;

                    var lookup1 = sICD.Replace(" ", "").Split('|');
                    var lookup2 = ResultParentICD[iCount].ToString().Split('!')[1].Replace(" ", "").Split('|');
                    //var output = lookup1.SelectMany(i => i.Take(i.Count() - lookup2[i.Key].Count())).ToArray();
                    var output = lookup1.Intersect(lookup2).ToArray();

                    for (int j = 0; j < output.Length; j++)
                    {
                        if (ResultAlreadyColored.Contains(ResultParentICD[iCount].ToString().Split('!')[0]) == false && arrayIndex < ColorList.Length)
                        {
                            ResultColorCoding.Add(ResultParentICD[iCount].ToString().Split('!')[0] + "~" + ColorList[arrayIndex]);
                            ResultColorCoding.Add(output[j] + "~" + ColorList[arrayIndex]);
                            ResultAlreadyColored.Add(output[j]);
                            isColorApplied = true;
                        }
                    }

                    if (isColorApplied)
                    {
                        if (arrayIndex + 1 < ColorList.Length)
                            arrayIndex++;
                        else
                            arrayIndex = 0;
                    }
                }
            }

            //Old Code - Start
            //    IList<string> ResultParentICD = pblmListParentICD;
            //k:
            //    IList<AllICDColorCoding> CheckColorCoding = new List<AllICDColorCoding>();
            //    for (int i = 0; i < ResultParentICD.Count; i++)
            //    {
            //        //if (currentSelectedTreeNodeGroup.Count() > 0)
            //        //{

            //        for (int j = 0; j < ResultParentICD[i].Split('!')[0].Length; j++)
            //        {
            //            if (j > 0)
            //            {
            //                if (ResultParentICD[i].Contains("!"))
            //                    ResultParentICD[i] = ResultParentICD[i].Replace(ResultParentICD[i].Split('!')[0] + "!", "");
            //                // ResultParentICD[i] = ResultParentICD[i].Replace(CheckColorCoding[i].ICD_Codes + "-" + CheckColorCoding[i].currentLeaf_Node + "-" + CheckColorCoding[i].currentMutually_Exclusive + "!", "");
            //                goto l;
            //            }
            //            else
            //            {
            //                if (ResultParentICD[i].Contains("!"))
            //                {
            //                    AllICDColorCoding colorCoding = new AllICDColorCoding();
            //                    colorCoding.ICD_Codes = ResultParentICD[i].Split('!')[j].Split('-')[0];
            //                    colorCoding.Leaf_Node = ResultParentICD[i].Split('!')[j + 1].Split('-')[1];
            //                    colorCoding.Mutually_Exclusive = ResultParentICD[i].Split('!')[j + 1].Split('-')[2];
            //                    colorCoding.Parent_ICD = ResultParentICD[i].Split('!')[j + 1].Split('-')[0];
            //                    colorCoding.currentLeaf_Node = ResultParentICD[i].Split('!')[j].Split('-')[1];
            //                    colorCoding.currentMutually_Exclusive = ResultParentICD[i].Split('!')[j].Split('-')[2];
            //                    CheckColorCoding.Add(colorCoding);
            //                }
            //            }


            //        }


            //    l:
            //        string scheck = "";

            //        // getICDGroupDictionairy(icdKey);
            //    }

            //if (CheckColorCoding.Count > 0)
            //{
            //    var currentSelectedTreeNodeGroup = CheckColorCoding
            //                                 .GroupBy(g => g.Parent_ICD).ToList();



            //    IList<string> icdKey = new List<string>();
            //    foreach (var item in currentSelectedTreeNodeGroup)
            //    {
            //        if (item.Key.Split('.')[0].Length == 1)
            //            goto m;
            //        if (item.Key.Split('.').Count() > 1)
            //            icdKey.Add(item.Key);
            //        if (item.Key.Split('.').Count() > 0)
            //        {
            //            if (icdGroupDictionairy.ContainsKey(item.Key.Split('.')[0]))
            //            {
            //                foreach (var val in item)
            //                    icdGroupDictionairy[item.Key.Split('.')[0]].Add(val);
            //            }
            //            else
            //                icdGroupDictionairy.Add(item.Key.Split('.')[0], item.ToList());
            //        }
            //    m:
            //        string dCheck = "";
            //    }
            //    if (icdKey.Count > 0)
            //    {

            //        goto k;
            //        //string str = "";
            //    }
            //}


            //IList<string> ResultColorCoding = new List<string>();



            //foreach (var keyGroup in icdGroupDictionairy.OrderBy(o => o.Key).ToList())
            //{
            //    bool isColorApplied = false;
            //    foreach (var group in keyGroup.Value.Where(s => (s.Leaf_Node == "Y")).OrderBy(o => o.ICD_Codes).ToList())
            //    {

            //        if (keyGroup.Value.Where(s => s.Leaf_Node == "Y").ToList().All(s => s.Parent_ICD == group.Parent_ICD))
            //        {
            //            if (keyGroup.Value.Where(s => s.Leaf_Node == "Y").Count() > 1 && group.Mutually_Exclusive == "Y")
            //            {
            //                ResultColorCoding.Add(group.ICD_Codes + "~" + ColorList[arrayIndex]);
            //                isColorApplied = true;
            //            }
            //        }
            //        else
            //        {
            //            isval = 0;
            //            callre(keyGroup.Value.Where(d => d.Leaf_Node == "Y").ToList().Select(f => f.ICD_Codes).ToList(), keyGroup.Value.ToList());

            //            if (isval > 1)
            //            {
            //                ResultColorCoding.Add(group.ICD_Codes + "~" + ColorList[arrayIndex]);
            //                isColorApplied = true;
            //            }
            //        }
            //    }

            //    if (isColorApplied)
            //        arrayIndex++;

            //}
            //Old Code - End
            #region Declaration

            //var Result = pblmListParentICD.GroupBy(a => a.Split('!')[a.Split('!').Length - 1].Split('-')[0]).Select(c => new { ICD = c.Key, Desc = c.Select(x => x).ToList() }).ToList();
            //for (int i = 0; i < Result.Count; i++)
            //{

            //    if (Result[i].Desc.Count >= 2 && Result[i].ICD!="")
            //    {
            //        IList<AllICDColorCoding> ResultColorCodingICDS = new List<AllICDColorCoding>();

            //        int iArray = 0;
            //        //var Rsult= Result[i].Desc.Select(a=> a.Split('!')[].Split('-'))
            //        for (int k = 0; k < Result[i].Desc.Count; k++)
            //        {
            //            for (int j = 0; j < Result[i].Desc[k].Split('!').Length; j++)
            //            {
            //                if (j == Result[i].Desc[k].Split('!').Length - 1)
            //                {
            //                }
            //                else
            //                {
            //                    AllICDColorCoding colorCoding = new AllICDColorCoding();
            //                    colorCoding.ICD_Codes = Result[i].Desc[k].Split('!')[j].Split('-')[0];
            //                    colorCoding.Leaf_Node = Result[i].Desc[k].Split('!')[j+1].Split('-')[1];
            //                    colorCoding.Mutually_Exclusive = Result[i].Desc[k].Split('!')[j+1].Split('-')[2];
            //                    colorCoding.Parent_ICD = Result[i].Desc[k].Split('!')[j + 1].Split('-')[0];
            //                    ResultColorCodingICDS.Add(colorCoding);
            //                }

            //            }



            //        }


            //        IList<AllICDColorCoding> ResultGroupColorCoding = (from b in ResultColorCodingICDS where pblmListParentICD.Any(a => a.Split('!')[0].Split('-')[0] == b.ICD_Codes) select b).ToList();

            //       // ResultColorCodingICDS.Where(a=>a.ICD_Codes==)
            //        for (int m = 0; m < ResultGroupColorCoding.Count; m++)
            //        {

            //            if (ResultColorCodingICDS.Where(s => s.Leaf_Node == "Y").ToList().All(s => s.Parent_ICD == ResultGroupColorCoding[m].Parent_ICD))
            //            {
            //                if (ResultColorCodingICDS.Where(s => s.Leaf_Node == "Y").Count() > 1 && ResultGroupColorCoding[m].Mutually_Exclusive == "Y")
            //                {
            //                    ResultColorCoding.Add(ResultGroupColorCoding[m].ICD_Codes + "~" + ColorList[arrayIndex]);
            //                    isColorApplied = true;
            //                  //  row.Cells["ICDGroupKey"].Value = keyGroup.Key;
            //                  //  isColor = true;
            //                }
            //            }
            //            else
            //            {
            //                isval = 0;
            //                callre(ResultColorCodingICDS.Where(d => d.Leaf_Node == "Y").ToList().Select(f => f.ICD_Codes).ToList(), ResultColorCodingICDS.ToList());

            //                if (isval > 1)
            //                {
            //                    ResultColorCoding.Add(ResultGroupColorCoding[m].ICD_Codes + "~" + ColorList[arrayIndex]);
            //                    isColorApplied = true;

            //                    //row.Cells["ICDGroupKey"].Value = keyGroup.Key;
            //                    //isColor = true;
            //                }
            //            }

            //        }

            //         if (isColorApplied)
            //           arrayIndex++;






            //        //var pairSequence = Result[i].Desc[k].Split('!').Select(s => s.Split('-')[2]).Select(a => new { ResultICD = a }).ToList();

            //        //ResultICDString.Add(pairSequence.Select(a => a.ResultICD).ToString());
            //        //pairSequence.
            //               //Result[i].Desc[k]

            //       // }
            //        //var iCount = Result[i].Desc.Select(a => a.Split('!')).ToList();
            //        //for (int k = 0; k < iCount.Count; k++)
            //        //{
            //        //    var Check = iCount[k];
            //        //}
            //    }

            //}
            #endregion
            var ListColor = ResultColorCoding.Select(a => new { ICDCode = a.Split('~')[0], Color = a.Split('~')[1] });
            string json = new JavaScriptSerializer().Serialize(ListColor);

            sColorCoding = "{\"ColorCoding\" :" + json + "}";

            return sColorCoding;
        }





        private static void callre(IList<string> key, IList<AllICDColorCoding> allIcd)
        {
            foreach (var item in key)
            {
                if (allIcd.Where(s => s.ICD_Codes == item).ToList().Count() > 0)
                {
                    if (temp.ContainsKey(allIcd.Where(s => s.ICD_Codes == item).ToList()[0].Parent_ICD))
                        temp[allIcd.Where(s => s.ICD_Codes == item).ToList()[0].Parent_ICD].Add(allIcd.Where(s => s.ICD_Codes == item).ToList()[0]);
                    else
                        temp.Add(allIcd.Where(s => s.ICD_Codes == item).ToList()[0].Parent_ICD, allIcd.Where(s => s.ICD_Codes == item).ToList());
                }
            }

            foreach (var item in temp)
            {
                if (allIcd.Where(s => s.ICD_Codes == item.Key).ToList().Count() > 0)
                {
                    if (allIcd.Where(s => s.ICD_Codes == item.Key).ToList()[0].Mutually_Exclusive == "Y")
                        isval++;
                }
            }

            if (isval == 1)
                callre(temp.Keys.ToList(), allIcd);
        }

        [WebMethod(EnableSession = true)]
        public static string GetEncounterDetailsforRaf(string Human_id)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sICdList = string.Empty;
            AssessmentManager objAssessmentMngr = new AssessmentManager();
            ulong Human_Id = 0;
            string sRaf_Status = string.Empty;
            string sMedicareID = string.Empty;
            string year = "";
            if (System.Configuration.ConfigurationManager.AppSettings["RafExclusionStatus"] != null)
                sRaf_Status = System.Configuration.ConfigurationManager.AppSettings["RafExclusionStatus"].ToString();
            if (System.Configuration.ConfigurationManager.AppSettings["MedicareID"] != null)
                sMedicareID = System.Configuration.ConfigurationManager.AppSettings["MedicareID"].ToString();

            if (Human_id != string.Empty)
            {
                Human_Id = Convert.ToUInt32(Human_id);
            }
            else
                Human_Id = ClientSession.HumanId;
            try
            {
                //53908
                // sICdList = objAssessmentMngr.GetAssessmentICDbyDos(Human_Id, ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Date_of_Service.ToString("yyyy"), sRaf_Status);
                if (ClientSession.SelectedFrom == "")
                {
                    year = ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Date_of_Service.ToString("yyyy");
                    if (year != "0001")
                        sICdList = objAssessmentMngr.GetAssessmentICDbyDos(Human_Id, ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Date_of_Service.ToString("yyyy"), sRaf_Status);
                    else
                    {
                        EncounterManager objEncMngr = new EncounterManager();
                        IList<Encounter> ilstEncounter = objEncMngr.GetEncounterByEncounterIDIncludeArchive(ClientSession.EncounterId);
                        if (ilstEncounter.Count > 0)
                        {
                            year = ilstEncounter[0].Date_of_Service.ToString("yyyy");
                            sICdList = objAssessmentMngr.GetAssessmentICDbyDos(Human_Id, year, sRaf_Status);
                        }
                    }

                }
                else
                {
                    sICdList = objAssessmentMngr.GetAssessmentICDbyDos(Human_Id, DateTime.Now.Year.ToString(), sRaf_Status);
                    year = DateTime.Now.Year.ToString();
                }


            }
            catch
            {
                sICdList = objAssessmentMngr.GetAssessmentICDbyDos(Human_Id, DateTime.Now.Year.ToString(), sRaf_Status);
                year = DateTime.Now.Year.ToString();
            }
            //sICdList = objAssessmentMngr.GetAssessmentICDbyDos(Human_Id, DateTime.Now.Year.ToString(), sRaf_Status);//ClientSession.FillPatientChart.Fill_Encounter_and_WFObject.EncRecord.Date_of_Service.ToString("yyyy"));

            //Read Human Xml
            string sInsurance_Plan_ID = string.Empty;
            IList<string> ilstInsurancePlan = new List<string>();
            string sOutPut = string.Empty;
            // string FileName = "Human" + "_" + Human_Id + ".xml";
            // string strXmlFilePathHuman = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            // if (File.Exists(strXmlFilePathHuman) == true)
            {
                XmlDocument itemDocHuman = new XmlDocument();
                // XmlTextReader XmlText = new XmlTextReader(strXmlFilePathHuman);
                itemDocHuman = new GenerateXml().ReadBlob("Human", Human_Id);
                // using (FileStream fs = new FileStream(strXmlFilePathHuman, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //itemDocHuman.Load(fs);

                    // XmlText.Close();
                    string xmlContent = null;
                    // XDocument documentnodeHuman = XDocument.Load(strXmlFilePathHuman);

                    if (itemDocHuman != null && itemDocHuman.GetElementsByTagName("Modules").Count > 0)
                    {
                        for (int i = 0; i < itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes.Count; i++)
                        {
                            if (itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes[i].Name == "HumanList")
                            {
                                xmlContent = itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes[i].OuterXml;
                                XmlDocument documentnode = new XmlDocument();
                                documentnode.LoadXml(xmlContent);
                                XmlNode node = documentnode.DocumentElement;
                                IEnumerator list = node.GetEnumerator();
                                XmlNode tempNode;
                                while (list.MoveNext())
                                {
                                    tempNode = (XmlNode)list.Current;

                                    if (tempNode.Attributes["Is_Medicaid"] != null)
                                    {
                                        if (tempNode.Attributes["Is_Medicaid"].Value.ToUpper() == "Y")
                                            sOutPut += "~" + "sIsMedicaid" + ":" + "YES";
                                        else
                                            sOutPut += "~" + "sIsMedicaid" + ":" + "NO";
                                    }
                                    if (tempNode.Attributes["Original_Eligibility_Due_To_Disability"] != null)
                                    {
                                        if (tempNode.Attributes["Original_Eligibility_Due_To_Disability"].Value.ToUpper() == "Y")
                                            sOutPut += "~" + "sOriginallyDisabled" + ":" + "YES";
                                        else
                                            sOutPut += "~" + "sOriginallyDisabled" + ":" + "NO";
                                    }
                                    if (tempNode.Attributes["Is_Institutional"] != null)
                                    {
                                        if (tempNode.Attributes["Is_Institutional"].Value.ToUpper() == "N" || tempNode.Attributes["Is_Institutional"].Value == "")
                                            sOutPut += "~" + "sIsCommunity" + ":" + "YES";
                                        else
                                            sOutPut += "~" + "sIsCommunity" + ":" + "NO";
                                    }
                                    if (tempNode.Attributes["Current_Eligibility_Due_To"] != null)
                                    {
                                        if (tempNode.Attributes["Current_Eligibility_Due_To"].Value.ToUpper() == "Y")
                                            sOutPut += "~" + "sIsDisabled" + ":" + "YES";
                                        else
                                            sOutPut += "~" + "sIsDisabled" + ":" + "NO";
                                    }
                                    if (tempNode.Attributes["Enrollment_Status"] != null)
                                    {
                                        sOutPut += "~" + "sEnrollmentStatus" + ":" + tempNode.Attributes["Enrollment_Status"].Value.ToString().ToUpper();
                                    }
                                }
                                // break;
                            }
                            if (itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes[i].Name == "PatientInsuredPlanList")
                            {
                                xmlContent = itemDocHuman.GetElementsByTagName("Modules")[0].ChildNodes[i].OuterXml;
                                XmlDocument documentnode = new XmlDocument();
                                documentnode.LoadXml(xmlContent);
                                XmlNode node = documentnode.DocumentElement;
                                IEnumerator list = node.GetEnumerator();
                                XmlNode tempNode;
                                while (list.MoveNext())
                                {
                                    tempNode = (XmlNode)list.Current;
                                    if (tempNode.Attributes["Insurance_Plan_ID"] != null)
                                    {
                                        //sInsurance_Plan_ID = tempNode.Attributes["Insurance_Plan_ID"].Value;
                                        ilstInsurancePlan.Add(tempNode.Attributes["Insurance_Plan_ID"].Value.ToString());
                                    }
                                }
                                //break;
                            }
                        }
                    }
                    // fs.Close();
                    // fs.Dispose();
                }
            }
            if (ilstInsurancePlan.Count > 0)
            {
                Boolean bCarrier = false;
                if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\Insurance_Plan.xml"))
                {
                    XDocument xmlFacility = XDocument.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Insurance_Plan.xml");

                    for (int i = 0; i < ilstInsurancePlan.Count; i++)
                    {
                        IEnumerable<XElement> xmlFac = xmlFacility.Element("InsurancePlanList")
                        .Elements("insuranceplan").Where(aa => aa.Attribute("insurance_plan_id").Value.ToString() == ilstInsurancePlan[i].ToString() && aa.Attribute("active").Value.ToString().ToUpper() == "Y" && aa.Attribute("carrier_id").Value.ToString().ToUpper() == sMedicareID);
                        if (xmlFac != null && xmlFac.Count() > 0)
                        {
                            bCarrier = true;
                            sInsurance_Plan_ID = xmlFac.Attributes("carrier_id").First().Value.ToString();
                            break;
                        }
                    }
                }
                if (bCarrier)
                {
                    sOutPut += "~" + "sCarrierID" + ":" + sInsurance_Plan_ID;
                }
                else
                {
                    sOutPut += "~" + "sCarrierID" + ":" + 0;
                }
            }
            else
                sOutPut += "~" + "sCarrierID" + ":" + 0;

            sOutPut += "~" + "Year" + ":" + year;

            sICdList += sOutPut;
            return JsonConvert.SerializeObject(sICdList);
        }



        [WebMethod(EnableSession = true)]
        public static string GetPotentialICDs(string sFormviewICD)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            PotentialDiagnosisManager PotentialDiagnosisMngr = new PotentialDiagnosisManager();

            string sAsscoiateICDS = "";
            IList<AssociatedPrimaryICD> AssociaPrimaryList = new List<AssociatedPrimaryICD>();
            AssociatedPrimaryICDManager objAssessmentQuestionsMgr = new AssociatedPrimaryICDManager();
            IList<AllICD_9> AllICDList = new List<AllICD_9>();
            AllICD_9Manager objAllICDMngr = new AllICD_9Manager();
            List<string> IdList = new List<string>();
            List<string> ICDlist = new List<string>();
            List<string> ICD9CodeDescList = new List<string>();

            if (sFormviewICD != string.Empty)
            {
                //List<string> IdList = sFormviewICD.Split('|').Select(a => Convert.ToString(a.Split('~')[3].TrimEnd(','))).ToList();
                //PotentialDiagnosisMngr.MoveToAssessmentList(IdList, ClientSession.HumanId, ClientSession.UserName, UtilityManager.ConvertToUniversal());

                //AssociaPrimaryList = objAssessmentQuestionsMgr.GetAllAssociatedICDCodes(sFormviewICD.Split('|').Select(a => a.Split('~')[0]).ToArray());
                //AllICDList = objAllICDMngr.GetAllICDCodes(sFormviewICD.Split('|').Select(a => a.Split('~')[0]).ToArray());
                for (int i = 0; i < sFormviewICD.Split('|').Length; i++)
                {
                    IdList.Add(sFormviewICD.Split('|')[i].Split('~')[3].TrimEnd(','));
                    ICDlist.Add(sFormviewICD.Split('|')[i].Split('~')[0].TrimEnd(','));
                    if (sFormviewICD.Split('|')[i].Split('~')[4].TrimEnd(',') == "ICD_9")
                    {
                        ICD9CodeDescList.Add(sFormviewICD.Split('|')[i].Split('~')[0].TrimEnd(',') + "~" + sFormviewICD.Split('|')[i].Split('~')[1].TrimEnd(',') + "~"
                                      + sFormviewICD.Split('|')[i].Split('~')[2].TrimEnd(',') + "~" + "" + "~" + sFormviewICD.Split('|')[i].Split('~')[3].TrimEnd(',') + "~"
                                      + sFormviewICD.Split('|')[i].Split('~')[5].TrimEnd(',') + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "");
                    }
                }
                PotentialDiagnosisMngr.MoveToAssessmentList(IdList, ClientSession.HumanId, ClientSession.UserName, UtilityManager.ConvertToUniversal());
                AssociaPrimaryList = objAssessmentQuestionsMgr.GetAllAssociatedICDCodes(sFormviewICD.Split('|').Select(a => a.Split('~')[0]).ToArray());
                AllICDList = objAllICDMngr.GetAllICDCodes(sFormviewICD.Split('|').Select(a => a.Split('~')[0]).ToArray());
            }


            IList<string> PDICD9singleMapping = new List<string>();
            IList<string> PDICD10MutipleMapping = new List<string>();
            for (int i = 0; i < ICD9CodeDescList.Count; i++)
            {
                bool bcheck = false;
                XmlDocument xmldoc = new XmlDocument();
                if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\icd_9_10_mapping.xml"))
                {
                    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "icd_9_10_mapping" + ".xml");
                    XmlNodeList xmlMappingList = xmldoc.GetElementsByTagName("icd_9_10_mapping");
                    foreach (XmlNode item in xmlMappingList)
                    {
                        if (item != null)
                        {

                            if (item.Attributes[0].Value == ICD9CodeDescList[i].Split('~')[0])
                            {
                                PDICD10MutipleMapping.Add(ICD9CodeDescList[i] + "^" + item.Attributes[1].Value);
                                bcheck = true;
                            }
                        }
                    }
                    if (!bcheck)
                    {
                        PDICD9singleMapping.Add(ICD9CodeDescList[i]);
                    }
                }
            }
            IList<ICD9ICD10Mapping> ICD10ICDDescList = new List<ICD9ICD10Mapping>();
            if (PDICD9singleMapping != null)
            {
                if (PDICD9singleMapping.Count > 0)
                {
                    AllICD_9Manager objAllICDsMgr = new AllICD_9Manager();
                    ICD10ICDDescList = objAllICDsMgr.GetICD10CodeDesc(PDICD9singleMapping.Select(a => a.Split('~')[0]).ToArray());
                }
            }
            string Ass_Status = string.Empty;
            IDictionary<string, string> idicDefaultStatuslst = (Dictionary<string, string>)HttpContext.Current.Session["DefaultStatusList"];
            if (idicDefaultStatuslst != null && idicDefaultStatuslst.Count > 0)
            {
                if (idicDefaultStatuslst.ContainsKey("ASSESSMENT"))
                    Ass_Status = idicDefaultStatuslst["ASSESSMENT"];
            }
            if (AssociaPrimaryList.Count > 0 || AllICDList.Count > 0)
            {
                var Record1 = (from b in sFormviewICD.Split('|').ToArray() where AssociaPrimaryList.Any(a => a.ICD_9 == b.Split('~')[0]) select b).ToList();
                AssociaPrimaryList = AssociaPrimaryList.Where(a => a.ICD_9 == Record1[0].Split('~')[0]).ToList();
                var ListAssoICDQuestionaire = AssociaPrimaryList.Select(a => new { ICDCode = a.Associated_ICD_9, ICDDescription = a.Prefix.Trim() == string.Empty ? a.Associated_ICD_9_Description : a.Prefix + " " + a.Associated_ICD_9_Description, LeafNode = a.Leaf_Node, MutuallyExclusive = a.Mutually_Exclusive, ICDCodeDesc = Record1[0].Replace("~", "-"), MainICDDesc = Record1[0].Split('~')[1] });

                var Record2 = (from b in sFormviewICD.Split('|').ToArray() where AllICDList.Any(a => a.ICD_9 == b.Split('~')[0]) select b).ToList();
                //AllICDList = AllICDList.Where(a => a.ICD_9 == Record2[0].Split('~')[0]).ToList();
                var ListAllICDQuestionaire = AllICDList.Select(a => new { ICDCode = a.ICD_9, ICDDescription = a.ICD_9_Description, LeafNode = a.Leaf_Node, MutuallyExclusive = a.Mutually_Exclusive, ICDCodeDesc = Record2[0].Replace("~", "-"), MainICDDesc = Record2[0].Split('~')[1] });


                string jsonAssoICD = new JavaScriptSerializer().Serialize(ListAssoICDQuestionaire);
                string jsonAllICD = new JavaScriptSerializer().Serialize(ListAllICDQuestionaire);

                var NoRecord1 = (from b in sFormviewICD.Split('|').ToArray() where !AssociaPrimaryList.Any(a => a.ICD_9 == b.Split('~')[0]) && !AllICDList.Any(c => c.ICD_9 == b.Split('~')[0]) && b.Split('~')[4] == "ICD_10" select b).ToList();
                var NoResultRecord1 = NoRecord1.Select(a => new { ICDCode = a.Split('~')[0], ICDDescription = a.Split('~')[1], AssessmentID = 0, iVersion = 0, iProblemListVersion = 0, ProblemListID = 0, Notes = a.Split('~')[2].TrimEnd(','), Created_by = "", Created_date = "", Updated = "Y", StatusSelected = Ass_Status });
                if (ICD10ICDDescList != null && ICD10ICDDescList.Count > 0)
                {
                    var ICD9SingleList = ICD10ICDDescList.Select(a => new { ICDCode = a.ICD10, ICDDescription = a.Long_Description, AssessmentID = 0, iVersion = 0, iProblemListVersion = 0, ProblemListID = 0, Notes = "", Created_by = "", Created_date = "", Updated = "Y", StatusSelected = Ass_Status });
                    NoResultRecord1 = NoResultRecord1.Concat(ICD9SingleList).ToList();
                }

                string jsonNoQuestionnaire = new JavaScriptSerializer().Serialize(NoResultRecord1);
                IList<string> sMainQuestion = new List<string>();
                for (int i = 0; i < Record1.Count; i++)
                {
                    if (i > 0)
                        sMainQuestion.Add(Record1[i] + "-" + "Y");
                }
                string sMultiICD10Mapping = new JavaScriptSerializer().Serialize(PDICD10MutipleMapping);


                string jsonMainQuestionnaire = new JavaScriptSerializer().Serialize(sMainQuestion);
                sAsscoiateICDS = "{\"AssoICDQuestionnaire\" :" + jsonAssoICD + "," +
                               "\"AllICDQuestionnaire\":" + jsonAllICD + "," +
                               "\"NoQuestionnaire\":" + jsonNoQuestionnaire +
                               "," + "\"MainQuestionnaire\":" + jsonMainQuestionnaire + "," + "\"MultiICD10Tool\":" + sMultiICD10Mapping + "}";

            }
            else
            {
                if (sFormviewICD != string.Empty)
                {
                    var Record1 = (from b in sFormviewICD.Split('|').ToArray() where !AssociaPrimaryList.Any(a => a.ICD_9 == b.Split('~')[0]) && b.Split('~')[4] == "ICD_10" select b).ToList();
                    var ResultRecord1 = Record1.Select(a => new { ICDCode = a.Split('~')[0], ICDDescription = a.Split('~')[1], AssessmentID = 0, iVersion = 0, iProblemListVersion = 0, ProblemListID = 0, Notes = a.Split('~')[2].TrimEnd(','), ParentICD = a.Split('~')[0], Created_by = "", Created_date = "", Updated = "Y", StatusSelected = Ass_Status });
                    if (ICD10ICDDescList != null && ICD10ICDDescList.Count > 0)
                    {
                        var ICD9SingleList = ICD10ICDDescList.Select(a => new { ICDCode = a.ICD10, ICDDescription = a.Long_Description, AssessmentID = 0, iVersion = 0, iProblemListVersion = 0, ProblemListID = 0, Notes = "", ParentICD = "", Created_by = "", Created_date = "", Updated = "Y", StatusSelected = Ass_Status });
                        ResultRecord1 = ResultRecord1.Concat(ICD9SingleList).ToList();
                    }
                    string json = new JavaScriptSerializer().Serialize(ResultRecord1);
                    string sMultiICD10Mapping = new JavaScriptSerializer().Serialize(PDICD10MutipleMapping);
                    sAsscoiateICDS = "{\"NoQuestionnaire\" :" + json + "," + "\"MultiICD10Tool\":" + sMultiICD10Mapping + "}";
                }
            }
            return sAsscoiateICDS;
        }
        //Cap - 1566
        [WebMethod(EnableSession = true)]
        public static string LoadProblemList(string strAssessment)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }

            AssessmentManager objAssessmentManager = new AssessmentManager();
            FillAssessment assessmentLoadList = new FillAssessment();
            AllICD_9Manager objAllIcdMgr = new AllICD_9Manager();
            IList<ProblemList> pblmMedList = new List<ProblemList>();
            IList<AllICD_9> allICD9ForVitalsProblemListPFSH = new List<AllICD_9>();
            IList<string> strICDDesc = new List<string>();
            IList<string> strICD9CodeDesc = new List<string>();

            IList<string> strICD910Code = new List<string>();
            IList<VitalsAssesment> vitalsproblemList = new List<VitalsAssesment>();

            //BugID:49118
            #region AssessmentStatusLoad
            IDictionary<string, string> DefaultStatusList = new Dictionary<string, string>();
            IList<string> Statuslst = new List<string>();
            XmlDocument xml_doc = new XmlDocument();
            bool Default_Ass_Status = false;
            bool bSuggestProblemIcds = false;
            string jsons = "";

            assessmentLoadList = objAssessmentManager.LoadProblemList(ClientSession.EncounterId, ClientSession.HumanId,"load");
            //CAP-1671
            //if (assessmentLoadList.Assessment != null && assessmentLoadList.Assessment.Count() > 0)
            //{
            //    bSuggestProblemIcds = true;
            //}
            if (bSuggestProblemIcds == false)
            {
                if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\staticlookup.xml"))
                {
                    xml_doc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\staticlookup.xml");
                    XmlNodeList xml_nodelst = xml_doc.GetElementsByTagName("AssessmentStatus");
                    if (xml_nodelst != null && xml_nodelst.Count > 0)
                    {
                        foreach (XmlNode xml_node in xml_nodelst)
                        {
                            if (xml_node.Attributes.GetNamedItem("is_required").Value.ToUpper() == "YES")
                                Statuslst.Add(xml_node.Attributes.GetNamedItem("value").Value);
                        }
                    }

                    XmlNodeList xml_nodeSetDefault = xml_doc.GetElementsByTagName("AssessmentStatusDefaulted");
                    if (xml_nodeSetDefault != null && xml_nodeSetDefault.Count > 0)
                    {
                        if (xml_nodeSetDefault[0].Attributes.GetNamedItem("value").Value.ToUpper() == "YES")
                            Default_Ass_Status = true;
                    }
                    if (Default_Ass_Status)
                    {
                        XmlNodeList xml_nodelstDefaultStatus = xml_doc.GetElementsByTagName("AssessmentStatusDefault");
                        if (xml_nodelstDefaultStatus != null && xml_nodelstDefaultStatus.Count > 0)
                        {
                            foreach (XmlNode xml_node in xml_nodelstDefaultStatus)
                            {
                                DefaultStatusList.Add(xml_node.Attributes.GetNamedItem("Name").Value, xml_node.Attributes.GetNamedItem("value").Value);
                            }
                        }
                    }
                    HttpContext.Current.Session["DefaultStatusList"] = DefaultStatusList;
                }
                #endregion

                string Ass_Status = string.Empty, Other_Status = string.Empty;
                bool bSuggestIcds = true;
                IDictionary<string, string> idicDefaultStatuslst = (Dictionary<string, string>)HttpContext.Current.Session["DefaultStatusList"];
                if (idicDefaultStatuslst != null && idicDefaultStatuslst.Count > 0)
                {
                    if (idicDefaultStatuslst.ContainsKey("OTHERS"))
                        Other_Status = idicDefaultStatuslst["OTHERS"];
                    if (idicDefaultStatuslst.ContainsKey("ASSESSMENT"))
                        Ass_Status = idicDefaultStatuslst["ASSESSMENT"];
                }

                //CAP-1671
                //if (assessmentLoadList.Assessment != null && assessmentLoadList.Assessment.Count > 0)//BugID:53007 
                //    bSuggestIcds = false;

                IList<string> lstParent_ICD = new List<string>();
                lstParent_ICD = assessmentLoadList.Assessment.Select(a => a.Parent_ICD.Trim()).Distinct().ToList<string>();
                if (strAssessment == "Load")
                {
                    IList<string> problemListCodesWithParentCodesTemp = new List<string>();
                    IList<string> pblmListParentICD = new List<string>();
                    IList<string> pblmListCodes = new List<string>();
                    if (bSuggestIcds)//BugID:54773
                    {
                        if (assessmentLoadList.Problem_List != null && assessmentLoadList.Problem_List.Count > 0)
                        {
                            // bugId:65363
                            //   pblmMedList = assessmentLoadList.Problem_List.Where(a => a.Status.ToUpper() == "ACTIVE" && a.Is_Active == "Y" && !a.Reference_Source.Contains("Deleted")).ToList();
                            pblmMedList = assessmentLoadList.Problem_List.Where(a => a.Status.ToUpper() == "ACTIVE" && a.Is_Active == "Y" || (a.Reference_Source.Contains("Problem List|Deleted"))).ToList();
                            if (pblmMedList.Count > 0)
                            {
                                foreach (ProblemList obj in pblmMedList)
                                    pblmListCodes.Add(obj.ICD.Trim());
                            }
                            string sICD = string.Empty;
                            for (int i = 0; i < pblmListCodes.Count; i++)
                            {
                                if (sICD == string.Empty)
                                    sICD = pblmListCodes[i].ToString();
                                else
                                    sICD += '|' + pblmListCodes[i].ToString();

                            }

                            AllICD_9Manager objAllIcdMngr = new AllICD_9Manager();
                            foreach (string s in pblmListCodes)
                                lstParent_ICD.Remove(s);//to prevent removal of Past_medical_history ICDs which need to be reflected in case of change in its status.
                            pblmListParentICD = objAllIcdMngr.TakeParentICD(sICD);
                        }

                        //if (assessmentLoadList.VitalsBasedICD_List != null && assessmentLoadList.VitalsBasedICD_List.Count > 0)
                        //{
                        //    #region commented
                        //    //FillPatientSummaryBarDTO objSummaryDTO = new FillPatientSummaryBarDTO();
                        //    //IList<string> assessVitalsList = new List<string>();
                        //    //assessVitalsList = assessmentLoadList.VitalsBasedICD_List;

                        //    //for (int i = 0; i < assessVitalsList.Count; i++)
                        //    //{
                        //    //    var exist = (from assess in pblmListParentICD where assess.Contains(assessVitalsList[i]) == true select assess);
                        //    //    if (exist.Count() == 0)
                        //    //        pblmListParentICD.Add(assessVitalsList[i]);
                        //    //}
                        //    #endregion
                        //    //to find the exact match for ICD(Exists used instead of Contains)
                        //    IList<string> assessVitalsList = new List<string>();
                        //    assessVitalsList = assessmentLoadList.VitalsBasedICD_List;
                        //    List<string> ICDList = new List<string>();
                        //    for (int y = 0; y < pblmListParentICD.Count; y++)
                        //    {
                        //        string[] str = pblmListParentICD[y].Split('!');
                        //        foreach (string s in str)
                        //        {
                        //            ICDList.Add(s);
                        //        }
                        //    }
                        //    for (int i = 0; i < assessVitalsList.Count; i++)
                        //    {
                        //        string s = assessVitalsList[i];
                        //        bool val = (ICDList.Exists(a => a == s));
                        //        if (val == false)
                        //            pblmListParentICD.Add(assessVitalsList[i]);
                        //    }
                        //}

                        if (pblmListParentICD != null && pblmListParentICD.Count > 0)
                        {
                            var distinct = from h in pblmListParentICD group h by new { h } into g select new { g.Key.h };

                            foreach (var code in distinct)
                            {
                                var duplicate1 = problemListCodesWithParentCodesTemp.Where(h => h.Contains(code.h)).Select(s => s).ToList();
                                var duplicate = (from dup in problemListCodesWithParentCodesTemp where dup.Contains(code.h) select dup).ToList();

                                if (duplicate.Count() == 0)
                                {
                                    if (code.h != string.Empty)
                                        problemListCodesWithParentCodesTemp.Add(code.h);
                                }
                                else
                                {
                                    if (code.h.Split('!')[0] != duplicate.First().Split('!')[0])
                                        problemListCodesWithParentCodesTemp.Add(code.h);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (assessmentLoadList.Problem_List != null && assessmentLoadList.Problem_List.Count > 0)
                        {
                            pblmMedList = assessmentLoadList.Problem_List.Where(a => a.Status.ToUpper() == "ACTIVE" && a.Is_Active == "Y" && !a.Reference_Source.Contains("Deleted")).ToList();


                            if (pblmMedList.Count > 0)
                            {
                                foreach (ProblemList obj in pblmMedList)
                                    pblmListCodes.Add(obj.ICD.Trim());
                            }
                        }
                    }

                    if (problemListCodesWithParentCodesTemp != null && problemListCodesWithParentCodesTemp.Count > 0)
                    {
                        allICD9ForVitalsProblemListPFSH = objAllIcdMgr.GetProblemListCodeUsingCode(problemListCodesWithParentCodesTemp.Select(p => p.Split('!')[0]).ToArray<string>());
                    }
                    else
                    {
                        if (pblmListCodes != null && pblmListCodes.Count > 0)
                            allICD9ForVitalsProblemListPFSH = objAllIcdMgr.GetProblemListCodeUsingCode(pblmListCodes.Select(p => p).ToArray<string>());
                    }


                    if (allICD9ForVitalsProblemListPFSH != null && allICD9ForVitalsProblemListPFSH.Count > 0)
                    {
                        for (int i = 0; i < allICD9ForVitalsProblemListPFSH.Count; i++)
                        {
                            //Cap - 1713
                            //if (allICD9ForVitalsProblemListPFSH[i].Leaf_Node != "N" && !assessmentLoadList.Assessment.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9) && ((assessmentLoadList.Assessment.Where(s => s.Diagnosis_Source.ToUpper() != "VITALS|DELETED").Count() == 0 ? (assessmentLoadList.Problem_List.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                            //   : (assessmentLoadList.Problem_List.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9))) || assessmentLoadList.VitalsBasedICD_List.Any(a => a.Split('!')[0].ToString() == allICD9ForVitalsProblemListPFSH[i].ICD_9)))//|| currentVitalsBasedICDList.Any(c => c.Split('!')[0].ToString() == allICD9ForVitalsProblemListPFSH[i].ICD_9)                        
                            //CAP-1671
                            if (allICD9ForVitalsProblemListPFSH[i].Leaf_Node != "N")
                            //    && !assessmentLoadList.Assessment.Any(a => a.ICD.Trim() == allICD9ForVitalsProblemListPFSH[i].ICD_9.Trim()) && ((assessmentLoadList.Assessment.Where(s => s.Diagnosis_Source.ToUpper() != "VITALS|DELETED").Count() == 0 ? (assessmentLoadList.Problem_List.Any(a => a.ICD.Trim() == allICD9ForVitalsProblemListPFSH[i].ICD_9.Trim()))
                            //: (assessmentLoadList.Problem_List.Any(a => a.ICD.Trim() == allICD9ForVitalsProblemListPFSH[i].ICD_9.Trim()))) || assessmentLoadList.VitalsBasedICD_List.Any(a => a.Split('!')[0].ToString() == allICD9ForVitalsProblemListPFSH[i].ICD_9.Trim())))//|| currentVitalsBasedICDList.Any(c => c.Split('!')[0].ToString() == allICD9ForVitalsProblemListPFSH[i].ICD_9)                        
                            {
                                //if (assessmentLoadList.Assessment.Count() > 0 && assessmentLoadList.Assessment.Any(a => a.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9 && a.Diagnosis_Source.ToUpper() == "VITALS|DELETED"))
                                //    continue;
                                //if (assessmentLoadList.Assessment.Count() > 0 && assessmentLoadList.Assessment.Any(a => a.ICD_9 == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                                //    continue;
                                VitalsAssesment assMngr = new VitalsAssesment();
                                assMngr.ICD_9 = allICD9ForVitalsProblemListPFSH[i].ICD_9;
                                assMngr.Description = allICD9ForVitalsProblemListPFSH[i].ICD_9_Description;
                                ulong pId = assessmentLoadList.Problem_List.Where(p => p.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9 && p.Is_Active == "Y" && !p.Reference_Source.Contains("Deleted")).Select(d => d.Id).FirstOrDefault();
                                string status = string.Empty;

                                if (assessmentLoadList.Problem_List.Any(v => v.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                                    status = "Problem List";
                                if (assessmentLoadList.VitalsBasedICD_List.Select(p => p.Split('!')[0]).Any(s => s == allICD9ForVitalsProblemListPFSH[i].ICD_9))
                                    continue;


                                assMngr.Notes = status;
                                assMngr.ProblemListId = pId;
                                assMngr.AssessmentID = 0;
                                assMngr.sCreatedBy = "";
                                assMngr.sCreatedDateTime = "";
                                int pVersion = assessmentLoadList.Problem_List.Where(p => p.ICD == allICD9ForVitalsProblemListPFSH[i].ICD_9 && p.Is_Active == "Y" && !p.Reference_Source.Contains("Deleted")).Select(d => d.Version).FirstOrDefault();
                                assMngr.iVersion = pVersion;
                                assMngr.StatusSelected = Other_Status;

                                if (allICD9ForVitalsProblemListPFSH[i].Version_Year == "ICD_9")
                                    strICD9CodeDesc.Add(allICD9ForVitalsProblemListPFSH[i].ICD_9 + "~" + allICD9ForVitalsProblemListPFSH[i].ICD_9_Description + "~" + status + "~" + "Assessment" + "~" + pId + "~" + pVersion + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "" + "~" + Other_Status + "~" + "" + "~" + "");//BugID:47478

                                vitalsproblemList.Add(assMngr);
                            }
                        }
                        //CAP-1671
                        if (allICD9ForVitalsProblemListPFSH.Where(s => s.Leaf_Node == "N").ToList().Count > 0)
                        {
                            foreach (var item in allICD9ForVitalsProblemListPFSH.Where(s => s.Leaf_Node == "N").ToList())
                            {
                                int iCount = vitalsproblemList.Where(a => a.ICD_9 == item.ICD_9).ToList().Count;


                                if (iCount > 0)
                                    continue;


                                if (item.Version_Year == "ICD_10")
                                {
                                    strICDDesc.Add(item.ICD_9 + "-" + item.ICD_9_Description);
                                }
                                else
                                    strICD9CodeDesc.Add(item.ICD_9 + "~" + item.ICD_9_Description + "~" + "" + "IncompleteProblemList" + "~" + "IncompleteProblemList" + "~" + 0 + "~" + 0 + "~" + 0 + "~" + 0 + "~" + "NONE" + "~" + "" + "~" + "" + "~" + "" + "~" + "");//BugID:47478
                            }
                        }
                    }

                    strICDDesc = (from val in strICDDesc where !lstParent_ICD.Any(a => a.Trim() == val.Split('-')[0].Trim()) select val).ToList<string>();//to prevent a previously added and moved(to assessment grid) parentICD from being added to incomplete problem list(ROS,VITALS,RCopia_MEd ICDs)
                    var IncompleteProblemList = strICDDesc.Select(a => new { ICDCODE = a.Split('-')[0], ICDDescription = a.Split('-')[1] });
                    string json;
                    var ListVitalsProblemList = vitalsproblemList.Select(a => new { ICDCode = a.ICD_9, ICDDescription = a.Description, IsPrimary = a.Primary_Diagnosis, ProblemListID = a.ProblemListId, Notes = a.Notes, AssessmentID = a.AssessmentID, iVersion = a.iVersion, iProblemListVersion = a.iVersion, CheckBoxCheck = a.CheckBoxCheck, StatusSelected = a.StatusSelected, IncompleteICDCode = "", ICD9Code = a.ICD9Code, ICD9Desc = a.ICD9Desc, ParentICD = "", Created_by = a.sCreatedBy, Created_date = a.sCreatedDateTime, Updated = "Y", Orig_Status = a.StatusSelected });
                    if (vitalsproblemList.Count > 0)
                    {
                        jsons = "{\"AssessmentList\" :" + new JavaScriptSerializer().Serialize(ListVitalsProblemList) + "," +
                                      "\"FromProblemList\":" + new JavaScriptSerializer().Serialize(IncompleteProblemList) + "}";
                    }
                    else
                    {
                        json = new JavaScriptSerializer().Serialize("220026");
                        jsons = "{\"AssessmentList\" :" + json + "}";
                    }
                }


            }
            else
            {
                string json = new JavaScriptSerializer().Serialize("220027");
                jsons = "{\"AssessmentList\" :" + json + "}";
            }
            return jsons;
        }




    }
}