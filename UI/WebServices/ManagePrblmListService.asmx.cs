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
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for ManagePrblmListService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ManagePrblmListService : System.Web.Services.WebService
    {
       
        [WebMethod(EnableSession = true)]
       
        public string LoadManageProblemList(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sMacAddress = string.Empty;
            IList<ProblemList> LoadproblemListGrid = new List<ProblemList>();
            ProblemListManager obj_problemMgr = new ProblemListManager();
            IList<ProblemList> ResultLoadproblemListGrid = new List<ProblemList>();
            IList<ProblemList> pblmMedList = new List<ProblemList>();
            IList<AllICD_9> allICD9ForVitalsProblemListPFSH=new List<AllICD_9>();
            AllICD_9Manager objAllIcdMgr=new AllICD_9Manager();
            IList<ProblemList> PrblmIcd0 = new List<ProblemList>();
            IList<String> prblmicd = new List<String>();
            bool val;
            if(data=="true")
                val=true;
            else
             val=false;
            LoadproblemListGrid = obj_problemMgr.GetFromProblemList(ClientSession.HumanId, sMacAddress, val);
            HttpContext.Current.Session["ProblemListFull"] = LoadproblemListGrid;
            IList<ProblemList> prob = LoadproblemListGrid.Where(x => x.ICD == "0000").ToList();
            HttpContext.Current.Session["ProblemList00"] = prob;
            string jsons = "";
         
            if(val==true)
            {
                var pr = (from problem in LoadproblemListGrid
                          where problem.Is_Active == "Y" && problem.ICD.Trim() != "" && problem.Problem_Description != "" && problem.Status == "Active"
                          select problem);
                ResultLoadproblemListGrid = pr.ToList<ProblemList>();
            }
            else
            {
                ResultLoadproblemListGrid = LoadproblemListGrid.Where(a => a.ICD.Trim() != "" && a.Is_Active == "Y").ToList();
            }

            IList<ProblemList> prblmLst = new List<ProblemList>();
            prblmLst = ResultLoadproblemListGrid.Where(a => a.Version_Year == "ICD_9").ToList();
           
            string ICD_10 = string.Empty;
            string unsavedData = "false";
            IList<string> ICD9singleMapping = new List<string>();
            IList<string> ICD10MutipleMapping = new List<string>();
            for (int i = 0; i < prblmLst.Count; i++)
            {
                if (prblmLst[i].Version_Year == "ICD_9")
                {
                    bool bcheck = false;
                    //Jira cap - 2770 - XML to JSON
                    //XmlDocument xmldoc = new XmlDocument();
                    //xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "icd_9_10_mapping" + ".xml");
                    //XmlNodeList xmlMappingList = xmldoc.GetElementsByTagName("icd_9_10_mapping");
                    //foreach (XmlNode item in xmlMappingList)
                    //{
                    //    if (item != null)
                    //    {

                    //        if (item.Attributes[0].Value == prblmLst[i].ICD)
                    //        {
                    //            ICD10MutipleMapping.Add(prblmLst[i].ICD+"~"+prblmLst[i].Problem_Description+"~"+prblmLst[i].Id+"~"+prblmLst[i].Version+"~"+prblmLst[i].Status+ "^" + item.Attributes[1].Value);
                    //            ResultLoadproblemListGrid.Remove(prblmLst[i]);
                    //            bcheck = true;
                    //        }
                    //    }
                    //}
                    //if (!bcheck)
                    //{
                    //    ICD9singleMapping.Add(prblmLst[i].ICD);
                    //    ResultLoadproblemListGrid.Remove(prblmLst[i]);
                    //}


                    IList<Icd_9_10_Mapping> iListIcd_9_10_Mapping = new List<Icd_9_10_Mapping>();
                    icd_9_10_mapping ilisticd_9_10_mapping = new icd_9_10_mapping();
                    ilisticd_9_10_mapping = ConfigureBase<icd_9_10_mapping>.ReadJson("icd_9_10_mapping.json");
                    if (ilisticd_9_10_mapping != null)
                    {
                        iListIcd_9_10_Mapping = ilisticd_9_10_mapping.Icd_9_10_Mapping;
                    }
                    if ((iListIcd_9_10_Mapping?.Count ?? 0) > 0)
                    {
                        for (int j = 0; j < iListIcd_9_10_Mapping.Count; j++)
                        {
                            if (iListIcd_9_10_Mapping != null)
                            {

                                if (iListIcd_9_10_Mapping[j].icd_9 == prblmLst[i].ICD)
                                {
                                    ICD10MutipleMapping.Add(prblmLst[i].ICD + "~" + prblmLst[i].Problem_Description + "~" + prblmLst[i].Id + "~" + prblmLst[i].Version + "~" + prblmLst[i].Status + "^" + iListIcd_9_10_Mapping[j].icd_choices);
                                    ResultLoadproblemListGrid.Remove(prblmLst[i]);
                                    bcheck = true;
                                }
                            }
                        }
                        if (!bcheck)
                        {
                            ICD9singleMapping.Add(prblmLst[i].ICD);
                            ResultLoadproblemListGrid.Remove(prblmLst[i]);
                        }
                    }


                }

            }

            for (int i = 0;i< ResultLoadproblemListGrid.Count; i++)
            {
                prblmicd.Add(ResultLoadproblemListGrid[i].ICD);
            }
           
             IList<ICD9ICD10Mapping> ICD10ICDDescList = new List<ICD9ICD10Mapping>();
            if (ICD9singleMapping.Count > 0)
            {
               
                AllICD_9Manager objAllICDsMgr = new AllICD_9Manager();

                ICD10ICDDescList = objAllICDsMgr.GetICD10CodeDesc(ICD9singleMapping.Select(a => a.Split('~')[0]).ToArray());
            }


            for (int i = 0; i < ICD10ICDDescList.Count; i++) 
            {
                    for (int j = 0; j < prblmLst.Count; j++)
                {
                    if (prblmLst[j].ICD == ICD10ICDDescList[i].ICD9)
                    {
                        prblmLst[j].ICD = ICD10ICDDescList[i].ICD10;
                        prblmLst[j].Problem_Description = ICD10ICDDescList[i].Long_Description;
                        prblmLst[j].Version_Year = "ICD_10";
                        if (!(prblmicd.Contains(prblmLst[j].ICD)))
                        {
                            ResultLoadproblemListGrid.Add(prblmLst[j]);
                             unsavedData = "true";
                        }
                    }
                }
            }

           


            var ListProblemList = ResultLoadproblemListGrid.Select(a => new
            {
                CheckBoxCheck=a.Is_Health_Concern,
                ICDCode = a.ICD,
                ICDDescription = a.Problem_Description,
                Status = a.Status,
                Year = (a.Date_Diagnosed.Length > 8) ? a.Date_Diagnosed.Split('-')[2] : (a.Date_Diagnosed.Length == 8) ? a.Date_Diagnosed.Split('-')[1] : (a.Date_Diagnosed.Length == 4) ? a.Date_Diagnosed : string.Empty,
                Month= (a.Date_Diagnosed.Length>8)?a.Date_Diagnosed.Split('-')[1]:(a.Date_Diagnosed.Length==8)?a.Date_Diagnosed.Split('-')[0]:string.Empty,
                           Date= (a.Date_Diagnosed.Length>8)?a.Date_Diagnosed.Split('-')[0]:string.Empty,
                EndYear = (a.Resolved_Date.Length > 8) ? a.Resolved_Date.Split('-')[2] : (a.Resolved_Date.Length == 8) ? a.Resolved_Date.Split('-')[1] : (a.Resolved_Date.Length == 4) ? a.Resolved_Date : string.Empty,
                EndMonth = (a.Resolved_Date.Length > 8) ? a.Resolved_Date.Split('-')[1] : (a.Resolved_Date.Length == 8) ? a.Resolved_Date.Split('-')[0] : string.Empty,
                Enddate = (a.Resolved_Date.Length > 8) ? a.Resolved_Date.Split('-')[0] : string.Empty,
                           ProblemListID = a.Id, iProblemListVersion = a.Version,isActive=a.Is_Active,
                RefSource = a.Reference_Source,
                Version_Yr=a.Version_Year,

            });
            HttpContext.Current.Session["ProblemList"] = ResultLoadproblemListGrid;
            string json = new JavaScriptSerializer().Serialize(ListProblemList);
            string sICD10Mapping = new JavaScriptSerializer().Serialize(ICD10MutipleMapping);
         
            jsons = "{\"ICDList\" :" + json + "," +
                                     "\"ICD10Tool\":" + sICD10Mapping + "," + "\"UnsavedData\":" + unsavedData + "}";
            return jsons;
        }



        [WebMethod(EnableSession = true)]
        public string SaveProblemList(object[] name, string NoKnwmActvPblm, string chkShowActive)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<ProblemList> probListToAdd = new List<ProblemList>();
            IList<ProblemList> probListToUpdate = new List<ProblemList>();
            IList<ProblemList> probListIdToDelete = new List<ProblemList>();
            IList<ulong> ulProbIDList = new List<ulong>();
            string sMacAddress = string.Empty;
            bool val=true;
            IList<ProblemList> LoadproblemListGridFull = new List<ProblemList>();
            IList<ProblemList> ResultLoadproblemListGrid = new List<ProblemList>();
            IList<ProblemList> sessionProblemListFull = (IList<ProblemList>)HttpContext.Current.Session["ProblemListFull"];
            IList<ProblemList> sessionProblemList = (IList<ProblemList>)HttpContext.Current.Session["ProblemList"];
            IList<ProblemList> sessionProblemList00 = (IList<ProblemList>)HttpContext.Current.Session["ProblemList00"];
            ProblemListManager obj_problemMgr = new ProblemListManager();

            foreach (object[] oj in name)
            {
                ProblemList problemlist;

                string year = Convert.ToString(oj[5]);
                string month = Convert.ToString(oj[6]);
                string date = Convert.ToString(oj[7]);

                string ryear = Convert.ToString(oj[8]);
                string rmonth = Convert.ToString(oj[9]);
                string rdate = Convert.ToString(oj[10]);

                if (date.IndexOf('?') != -1)
                    date = string.Empty;
                if (year.IndexOf('?') != -1)
                    year = string.Empty;
                if (month.IndexOf('?') != -1)
                    month = string.Empty;

                if (rdate.IndexOf('?') != -1)
                    rdate = string.Empty;
                if (ryear.IndexOf('?') != -1)
                    ryear = string.Empty;
                if (rmonth.IndexOf('?') != -1)
                    rmonth = string.Empty;

                if (Convert.ToString(oj[0]) == "Del")
                {
                    if (Convert.ToString(oj[11]) != String.Empty && Convert.ToUInt32(Convert.ToString(oj[11])) != 0)
                    {
                        problemlist = new ProblemList();
                        ulong id = Convert.ToUInt64(Convert.ToUInt64(Convert.ToString(oj[11])));
                        var problemList = (from prob in sessionProblemList
                                           where prob.Id == id && prob.ICD == Convert.ToString(oj[2])
                                           select prob).ToList();
                        if (problemList.Count() != 0)
                        {
                            foreach (var pr in problemList)
                            {
                                problemlist = pr;
                            }
                        }
                        problemlist.Human_ID = ClientSession.HumanId;
                        problemlist.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        problemlist.Modified_By = ClientSession.UserName;

                        string sReferencesource = Convert.ToString(oj[14]);
                        if (sReferencesource.Contains("Deleted"))
                        {
                            int iRef = sReferencesource.IndexOf("|Deleted");
                            if (iRef > 0)
                            {
                                sReferencesource = sReferencesource.Remove(iRef);
                                problemlist.Reference_Source = sReferencesource;
                            }
                            else
                            {
                                int Result = sReferencesource.IndexOf("Deleted");
                                sReferencesource = sReferencesource.Replace("Deleted|", "");//.Remove(Result);
                                problemlist.Reference_Source = sReferencesource;

                            }

                        }
                        if (sReferencesource.Contains("Problem List") == false)
                        {

                            if (sReferencesource != "")
                            {
                                problemlist.Reference_Source = sReferencesource + "|" + "Problem List";
                            }
                            else
                            {
                                problemlist.Reference_Source = "Problem List";
                            }
                        }
                        problemlist.Is_Active = "N";

                        problemlist.Version = Convert.ToInt32(Convert.ToString(oj[12]));
                        problemlist.Version_Year = Convert.ToString(oj[15]);
                        probListIdToDelete.Add(problemlist);
                    }
                }
                else
                {
                    if (Convert.ToString(oj[11]) != String.Empty && Convert.ToUInt64(Convert.ToString(oj[11])) == 0)
                    {
                        problemlist = new ProblemList();
                        var problemList = (from prob in sessionProblemListFull
                                           where (prob.ICD == Convert.ToString(oj[2]))
                                           select prob).ToList();
                        if (problemList.Count() != 0)
                        {
                            foreach (var pr in problemList)
                            {
                                problemlist = pr;
                            }
                            problemlist.ICD = Convert.ToString(oj[2]);
                            problemlist.Problem_Description = Convert.ToString(oj[3]);
                            problemlist.Human_ID = ClientSession.HumanId;
                            problemlist.Physician_ID = ClientSession.PhysicianId;
                            //CAP-1620
                            //problemlist.Encounter_ID = ClientSession.EncounterId;
                            problemlist.Encounter_ID = 0;
                            problemlist.Reference_Source = "Problem List";
                            problemlist.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            problemlist.Is_Active = "Y";
                            problemlist.Modified_By = ClientSession.UserName;
                           // problemlist.Version = Convert.ToInt32(oj[8]);
                            problemlist.Version_Year = Convert.ToString(oj[15]);
                            problemlist.Status = Convert.ToString(oj[4]);

                            if (Convert.ToBoolean(oj[1]) == true)
                                problemlist.Is_Health_Concern = "Y";
                            else if (Convert.ToBoolean(oj[1]) == false)
                                problemlist.Is_Health_Concern = "N";

                            if (year != "" && month != "" && date != "")
                                problemlist.Date_Diagnosed = date + '-' + month + '-' + year;
                            else
                                if (year != "" && month != "" && date == "")
                                    problemlist.Date_Diagnosed = month + '-' + year;
                                else
                                    if (month == "" && date == "")
                                        problemlist.Date_Diagnosed = year;
                                    else
                                        problemlist.Date_Diagnosed = string.Empty;
                            if (ryear != "" && rmonth != "" && rdate != "")
                                problemlist.Resolved_Date = rdate + '-' + rmonth + '-' + ryear;
                            else
                                if (ryear != "" && rmonth != "" && rdate == "")
                                    problemlist.Resolved_Date = rmonth + '-' + ryear;
                                else
                                    if (rmonth == "" && rdate == "")
                                        problemlist.Resolved_Date = ryear;
                                    else
                                        problemlist.Resolved_Date = string.Empty;
                            probListToUpdate.Add(problemlist);
                        }
                        else
                        {
                            problemlist.ICD = Convert.ToString(oj[2]);
                            problemlist.Problem_Description = Convert.ToString(oj[3]);
                            problemlist.Human_ID = ClientSession.HumanId;
                            problemlist.Physician_ID = ClientSession.PhysicianId;
                            //CAP-1620
                            //problemlist.Encounter_ID = ClientSession.EncounterId;
                            problemlist.Encounter_ID = 0;
                            problemlist.Reference_Source = "Problem List";
                            problemlist.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            problemlist.Is_Active = "Y";
                            problemlist.Created_By = ClientSession.UserName;
                            problemlist.Version = Convert.ToInt32(Convert.ToString(oj[12]));
                            problemlist.Version_Year = Convert.ToString(oj[15]);
                            problemlist.Status = Convert.ToString(oj[4]);

                            if (Convert.ToBoolean(oj[1]) == true)
                                problemlist.Is_Health_Concern = "Y";
                            else if (Convert.ToBoolean(oj[1]) == false)
                                problemlist.Is_Health_Concern = "N";

                            if (year != "" && month != "" && date != "")
                                problemlist.Date_Diagnosed = date + '-' + month + '-' + year;
                            else
                                if (year != "" && month != "" && date == "")
                                    problemlist.Date_Diagnosed = month + '-' + year;
                                else
                                    if (month == "" && date == "")
                                        problemlist.Date_Diagnosed = year;
                                    else
                                        problemlist.Date_Diagnosed = string.Empty;
                            if (ryear != "" && rmonth != "" && rdate != "")
                                problemlist.Resolved_Date = rdate + '-' + rmonth + '-' + ryear;
                            else
                                if (ryear != "" && rmonth != "" && rdate == "")
                                    problemlist.Resolved_Date = rmonth + '-' + ryear;
                                else
                                    if (rmonth == "" && rdate == "")
                                        problemlist.Resolved_Date = ryear;
                                    else
                                        problemlist.Resolved_Date = string.Empty;

                            probListToAdd.Add(problemlist);
                        }
                      
                    }
                    else
                        if (Convert.ToString(oj[11]) != String.Empty && Convert.ToUInt64(Convert.ToString(oj[11])) != 0)
                        {
                            problemlist = new ProblemList();
                            ulong id = Convert.ToUInt64(Convert.ToUInt64(Convert.ToString(oj[11])));

                            var problemList = (from prob in sessionProblemListFull
                                               where ((prob.Id == id && prob.ICD == Convert.ToString(oj[2])) || (prob.Id == id))
                                               select prob).ToList();
                            if (problemList.Count() != 0)
                            {
                                foreach (var pr in problemList)
                                {
                                    problemlist = pr;
                                }
                            }

                            string status = Convert.ToString(oj[4]);
                            string diag_date;
                            if (year != "" && month != "" && date != "")
                                diag_date = date + '-' + month + '-' + year;
                            else
                                if (year != "" && month != "" && date == "")
                                    diag_date = month + '-' + year;
                                else
                                    if (month == "" && date == "")
                                        diag_date = year;
                                    else
                                        diag_date = string.Empty;
                             string rdiag_date;
                            if (ryear != "" && rmonth != "" && rdate != "")
                                rdiag_date = rdate + '-' + rmonth + '-' + ryear;
                            else
                                if (ryear != "" && rmonth != "" && rdate == "")
                                    rdiag_date = rmonth + '-' + ryear;
                                else
                                    if (rmonth == "" && rdate == "")
                                        rdiag_date = ryear;
                                    else
                                        rdiag_date = string.Empty;
                            problemlist.ICD = Convert.ToString(oj[2]);
                            problemlist.Problem_Description = Convert.ToString(oj[3]);
                            problemlist.Human_ID = ClientSession.HumanId;
                            problemlist.Physician_ID = ClientSession.PhysicianId;
                            //CAP-1620
                            //problemlist.Encounter_ID = ClientSession.EncounterId;
                            problemlist.Encounter_ID = 0;//ClientSession.EncounterId;
                            problemlist.Date_Diagnosed = diag_date;
                            problemlist.Resolved_Date = rdiag_date;
                            string sReferencesource = problemlist.Reference_Source;
                            problemlist.Version = Convert.ToInt32(Convert.ToString(oj[12]));
                            problemlist.Version_Year = Convert.ToString(oj[15]);
                            int ResultProblemList = (from problem in sessionProblemList
                                                     where problem.Id == id && problem.ICD == Convert.ToString(oj[2])
                                                     && problem.Status == status
                                                     && problem.Date_Diagnosed == diag_date 
                                                     select problem).Count();

                            if (Convert.ToBoolean(oj[1]) == true)
                                problemlist.Is_Health_Concern = "Y";
                            else if (Convert.ToBoolean(oj[1]) == false)
                                problemlist.Is_Health_Concern = "N";

                            //&& problem.Resolved_Date==rdiag_date
                            if (ResultProblemList == 0)
                            {
                                if (sReferencesource.Contains("Deleted"))
                                {
                                    int iRef = sReferencesource.IndexOf("|Deleted");
                                    if (iRef > 0)
                                    {
                                        sReferencesource = sReferencesource.Remove(iRef);
                                        problemlist.Reference_Source = sReferencesource;
                                    }
                                    else
                                    {
                                        int Result = sReferencesource.IndexOf("Deleted");
                                        sReferencesource = sReferencesource.Replace("Deleted|", "");//.Remove(Result);
                                        problemlist.Reference_Source = sReferencesource;

                                    }

                                }
                                if (sReferencesource.Contains("Problem List") == false)
                                {

                                    if (sReferencesource != "")
                                    {
                                        problemlist.Reference_Source = sReferencesource + "|" + "Problem List";
                                    }
                                    else
                                    {
                                        problemlist.Reference_Source = "Problem List";
                                    }
                                }
                            }
                            problemlist.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            problemlist.Is_Active = "Y";
                            problemlist.Modified_By = ClientSession.UserName;
                            problemlist.Status = status;
                            probListToUpdate.Add(problemlist);
                        }
                }
            }

            var NoActive = sessionProblemList.Where(x => x.ICD == "0000");
            if(NoActive.ToList().Count==0)
                NoActive = sessionProblemList00.Where(x => x.ICD == "0000");
           //var NoActive = from icdCode in sessionProblemList where (icdCode.ICD_Code.IndexOf("0000")!=-1) select icdCode;
            
          // NoActive = from icdCode in sessionProblemList00 where icdCode.ICD_Code == "0000" select icdCode;

            if (NoActive.ToList().Count > 0)
            {
                ProblemList probList = NoActive.ToList<ProblemList>()[0];
                if (NoKnwmActvPblm == "true" || (probListToUpdate.Count==0 && probListToAdd.Count==0))
                    probList.Is_Active = "Y";
                else
                    if (NoKnwmActvPblm == "false")
                    probList.Is_Active = "N";
                probList.Version_Year = "ICD_10";
                probList.Modified_By = ClientSession.UserName;
                probList.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                var id = probListToUpdate.Where(x => x.Id == probList.Id);
               if(id.ToList().Count==0)
                   id = probListIdToDelete.Where(x => x.Id == probList.Id);
               if (id.ToList().Count == 0)
                 probListToUpdate.Add(probList);
            }
            else
            {
                ProblemList probList = new ProblemList();
                probList.ICD = "0000";
                probList.Date_Diagnosed = "";
                probList.Resolved_Date = "";
                probList.Version_Year = "ICD_10";
                probList.Human_ID = ClientSession.HumanId;
                probList.Physician_ID = ClientSession.PhysicianId;
                //CAP-1620
                //probList.Encounter_ID =  ClientSession.EncounterId;
                probList.Encounter_ID = 0;
                probList.Reference_Source = "Problem List";
                probList.Problem_Description = "No known Active Problems";
                probList.Status = "Active";
                if (NoKnwmActvPblm == "true")
                    probList.Is_Active = "Y";
                else
                    probList.Is_Active = "N";
                probList.Created_By = ClientSession.UserName;
                probList.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                probList.Is_Health_Concern = "N";
                probListToAdd.Add(probList);
            }
          
            IList<ProblemList> LoadproblemListGrid = new List<ProblemList>();

           

                if (probListToAdd.Count > 0 || probListToUpdate.Count > 0 || probListIdToDelete.Count > 0)
                {

                    if (probListIdToDelete.Count > 0 && probListIdToDelete != null)
                    {
                        probListToUpdate = probListToUpdate.Concat(probListIdToDelete).ToList<ProblemList>();
                    }

                    if (chkShowActive == "true")
                    {
                        LoadproblemListGrid = obj_problemMgr.InsertorUpdateIntoProblemList(probListToAdd.ToArray(), probListToUpdate.ToArray(), ClientSession.HumanId, "", true,ClientSession.LegalOrg);
                    }
                    else
                    {
                        LoadproblemListGrid = obj_problemMgr.InsertorUpdateIntoProblemList(probListToAdd.ToArray(), probListToUpdate.ToArray(), ClientSession.HumanId, "", false,ClientSession.LegalOrg);

                    }
                }
            IList<ProblemList> SummaryBarRefreshlist = new List<ProblemList>();
            IList<object> ilstProblemListBlobFinal = new List<object>();
            IList<string> ilstProblemListTagList = new List<string>();
            DateTime CurrentDOS = DateTime.MinValue;
            #region Commented By Deepak

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //   // itemDoc.Load(XmlText);
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
            #endregion

            ilstProblemListTagList.Add("ProblemListList");
            ilstProblemListBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstProblemListTagList);

            if (ilstProblemListBlobFinal != null && ilstProblemListBlobFinal.Count > 0)
            {
                if (ilstProblemListBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstProblemListBlobFinal[0]).Count; iCount++)
                    {
                        SummaryBarRefreshlist.Add((ProblemList)((IList<object>)ilstProblemListBlobFinal[0])[iCount]);
                    }
                }
            }

            string[] strarray = new string[2];
            UtilityManager objmngr = new UtilityManager();
            var toolTipProblem = string.Empty;
            string strProblmlist = objmngr.GetProblemList(SummaryBarRefreshlist, out toolTipProblem);
            strarray[0] = strProblmlist;
            strarray[1] = toolTipProblem;

            IList<ProblemList> problst = LoadproblemListGrid.Where(x => x.ICD == "0000").ToList();
            HttpContext.Current.Session["ProblemList00"] = problst;
            HttpContext.Current.Session["ProblemList"] = LoadproblemListGrid;
            LoadproblemListGridFull = obj_problemMgr.GetFromProblemList(ClientSession.HumanId, sMacAddress, val);
            HttpContext.Current.Session["ProblemListFull"] = LoadproblemListGridFull;
            if (chkShowActive == "true")
            {
                var pr = (from problem in LoadproblemListGrid
                          where problem.Is_Active == "Y" && problem.ICD.Trim() != "" && problem.Problem_Description != "" && problem.Status == "Active"
                          select problem);
                ResultLoadproblemListGrid = pr.ToList<ProblemList>();
            }
            else
            {
                ResultLoadproblemListGrid = LoadproblemListGrid.Where(a => a.ICD.Trim() != "" && a.Is_Active == "Y").ToList();
            }

            var ListProblemList = ResultLoadproblemListGrid.Select(a => new
                {
                    CheckBoxCheck = a.Is_Health_Concern,
                    ICDCode = a.ICD,
                    ICDDescription = a.Problem_Description,
                    Status = a.Status,
                    Year = (a.Date_Diagnosed.Length > 8) ? a.Date_Diagnosed.Split('-')[2] : (a.Date_Diagnosed.Length == 8) ? a.Date_Diagnosed.Split('-')[1] : (a.Date_Diagnosed.Length == 4) ? a.Date_Diagnosed : string.Empty,
                    Month = (a.Date_Diagnosed.Length > 8) ? a.Date_Diagnosed.Split('-')[1] : (a.Date_Diagnosed.Length == 8) ? a.Date_Diagnosed.Split('-')[0] : string.Empty,
                    Date = (a.Date_Diagnosed.Length > 8) ? a.Date_Diagnosed.Split('-')[0] : string.Empty,
                    EndYear = (a.Resolved_Date.Length > 8) ? a.Resolved_Date.Split('-')[2] : (a.Resolved_Date.Length == 8) ? a.Resolved_Date.Split('-')[1] : (a.Resolved_Date.Length == 4) ? a.Resolved_Date : string.Empty,
                    EndMonth = (a.Resolved_Date.Length > 8) ? a.Resolved_Date.Split('-')[1] : (a.Resolved_Date.Length == 8) ? a.Resolved_Date.Split('-')[0] : string.Empty,
                    Enddate = (a.Resolved_Date.Length > 8) ? a.Resolved_Date.Split('-')[0] : string.Empty,
                    ProblemListID = a.Id,
                    iProblemListVersion = a.Version,
                    isActive = a.Is_Active,
                    RefSource = a.Reference_Source,
                    Version_Yr = a.Version_Year
                });
                string jsons="";
                string json = new JavaScriptSerializer().Serialize(ListProblemList);
                string jsonArray = new JavaScriptSerializer().Serialize(strarray);
                jsons = "{\"ProblemList\" :" + json + "," + "\"ToolTip\" :" + jsonArray + "}";
                return jsons;
            
        }

        
    }
}