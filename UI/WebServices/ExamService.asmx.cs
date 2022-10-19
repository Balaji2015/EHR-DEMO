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

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for ExamService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ExamService : System.Web.Services.WebService
    {
        #region LoadExamTab
        [WebMethod(EnableSession = true)]
        public string LoadExamTab()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<user_scn_tab> lstUserScn = new List<user_scn_tab>();
            UserScnTabManager objUScnMngr = new UserScnTabManager();
           lstUserScn= objUScnMngr.GetFocusedExamDisableListByID(Convert.ToUInt32("101129"),ClientSession.UserName);
           string sDisableTab = "False";
           if (lstUserScn != null && lstUserScn.Count() > 0)
           {
               sDisableTab = "True";
           }
            string ExamValues = string.Empty;
            if ((ClientSession.EncounterId != 0 || ClientSession.EncounterId != null) && ClientSession.UserCurrentProcess != string.Empty && ClientSession.PhysicianUserName != string.Empty && (ClientSession.HumanId != 0 || ClientSession.HumanId != null) && (ClientSession.PhysicianId != 0 || ClientSession.PhysicianId != null) && ClientSession.UserName != string.Empty)
            {
                ExamValues = ClientSession.EncounterId + "&" + ClientSession.UserCurrentProcess + "&" + ClientSession.PhysicianUserName + "&" + ClientSession.HumanId + "&" + ClientSession.PhysicianId + "&" + ClientSession.UserName + "&version=" + System.Configuration.ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ", "");
            }
            else
            {
                ExamValues = "error";
            }
          //  return ExamValues;

            var result = new
            {
                sExamValues = ExamValues,
                DisableTab=sDisableTab

            };
            return JsonConvert.SerializeObject(result);

        }
        #endregion
        [WebMethod(EnableSession = true)]
        public string LoadExamination(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Examination> examScreenList = new List<Examination>();
            ExaminationManager examMngr = new ExaminationManager();
            examScreenList = examMngr.GetExamList(ClientSession.EncounterId, data, false);

            HttpContext.Current.Session["ExamGeneral"] = examScreenList;

            return JsonConvert.SerializeObject(examScreenList);
        }

        [WebMethod(EnableSession = true)]
        public string SaveExamination(object[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Examination> ExamList = ((IList<Examination>)HttpContext.Current.Session["ExamGeneral"]);
            ExaminationManager examMngr = new ExaminationManager();
            IList<Examination> examSaveList = new List<Examination>();
            IList<Examination> examUpdateList = new List<Examination>();
            IList<Examination> examScreenList = new List<Examination>();
            IList<Examination> SaveexamList = new List<Examination>();
            IList<Examination> UpdateexamList = new List<Examination>();
            IList<Examination> TempList = new List<Examination>();
            ulong exam_id;

            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    Examination objExam = new Examination();
                    Dictionary<string, object> ExamValues = new Dictionary<string, object>();
                    ExamValues = (Dictionary<string, object>)data[i];
                    objExam.Encounter_ID = Convert.ToUInt64(ClientSession.EncounterId);
                    objExam.Human_ID = Convert.ToUInt64(ClientSession.HumanId);
                    objExam.Physician_ID = ClientSession.PhysicianId;
                    objExam.System_Name = ExamValues["System"].ToString();
                    objExam.Condition_Name = ExamValues["Condition"].ToString();
                    objExam.Category = ExamValues["Category"].ToString();
                    objExam.Status = ExamValues["Status"].ToString();
                    objExam.Examination_Notes = ExamValues["Notes"].ToString();
                    objExam.Created_By = ExamValues["createdby"].ToString();
                    objExam.Version = Convert.ToInt32(ExamValues["Version"].ToString());
                    objExam.Created_Date_And_Time = Convert.ToDateTime(ExamValues["createddandt"].ToString());

                    exam_id = Convert.ToUInt32(ExamValues["ExamId"].ToString());

                    if (exam_id == 0)
                    {
                        if (ExamList.Count == 0)// New Encounter
                        {
                            objExam.Version = 1;
                            objExam.Created_By = ClientSession.UserName;
                            objExam.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            SaveexamList.Add(objExam);
                        }
                        else
                        {
                            // CopyPrev-Update
                            if (i > ExamList.Count - 1)// New Encounter
                            {
                                objExam.Version = 1;
                                objExam.Created_By = ClientSession.UserName;
                                objExam.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                SaveexamList.Add(objExam);
                            }
                            else
                            {
                                objExam.Id = ExamList[i].Id;
                                objExam.Version = ExamList[i].Version;
                                objExam.Created_By = ExamList[i].Created_By;
                                objExam.Created_Date_And_Time = ExamList[i].Created_Date_And_Time;
                                objExam.Modified_By = ClientSession.UserName;
                                objExam.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                UpdateexamList.Add(objExam);
                            }
                        }
                    }
                    else
                    {
                        // Update
                        IList<Examination> exam = (from a in ExamList where a.Id == exam_id select a).ToList<Examination>();
                        if (exam != null)
                        {
                            if (exam.Count == 0)// Change of Physician
                            {
                                objExam.Created_By = ClientSession.UserName;
                                objExam.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                SaveexamList.Add(objExam);
                            }
                            else
                            {
                                objExam.Id = exam[0].Id;
                                objExam.Version = exam[0].Version;
                                objExam.Created_By = exam[0].Created_By;
                                objExam.Created_Date_And_Time = exam[0].Created_Date_And_Time;
                                objExam.Modified_By = ClientSession.UserName;
                                objExam.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                UpdateexamList.Add(objExam);
                            }
                        }
                    }
                }
            }

            if (SaveexamList.Count > 0)
                examSaveList = examMngr.AppendExamination(SaveexamList.ToArray(), string.Empty);

            if (UpdateexamList.Count > 0)
                examUpdateList = examMngr.UpdateExams(UpdateexamList.ToArray(), string.Empty);

            examScreenList = examSaveList.Concat(examUpdateList)
                                       .ToList<Examination>();

            HttpContext.Current.Session["ExamGeneral"] = examScreenList;

            return JsonConvert.SerializeObject(examScreenList);

        }

        [WebMethod(EnableSession = true)]
        public string CopyPreviousExamination(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string examCategory = Convert.ToString(data);

            IList<FillExaminationScreen> lstPrevExam = new List<FillExaminationScreen>();
            ExaminationManager objExamMngr = new ExaminationManager();

            ulong encounterId = ClientSession.EncounterId;
            ulong humanId = ClientSession.HumanId;
            ulong physicianId = ClientSession.PhysicianId;

            lstPrevExam = objExamMngr.GetExamforPastEncounter(humanId, encounterId, physicianId, examCategory);

            return JsonConvert.SerializeObject(lstPrevExam);
        }
    }
}
