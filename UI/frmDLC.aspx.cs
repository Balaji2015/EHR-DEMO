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
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTO;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.IO;


namespace Acurus.Capella.UI
{
    public partial class frmDLC : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                if (Request["fieldname"] == "Login")
                {
                    if (Session["ClientFillLogin"] != null)
                    {
                        FillLogin fillLogin = (FillLogin)Session["ClientFillLogin"];
                        if (Request["UserName"] != null)
                        {
                            if (Request["FacilityName"] != null && Request["FacilityName"].ToString() != "")
                            {
                                Response.Write(Request["FacilityName"].ToString().Replace("HASHSYMBOL", "#").Replace("AMPERSAND", "&") + "@");
                            }                            
                            else //get the facility from the user table
                            {
                                IList<User> UserList = (fillLogin.UserList.Where(a => a.user_name.ToUpper() == Request["UserName"].ToUpper())).ToList<User>();
                                if (UserList.Count > 0)
                                {
                                    string sFacilityName = UserList[0].Default_Facility;
                                    Response.Write(sFacilityName + "@");
                                }
                            }
                        }
                    }

                }              
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetListBoxValuesReason(string fieldName, string value)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            fieldName = fieldName.Replace("&apos;", "'");
            UserLookupManager objUserLookupManager = new UserLookupManager();
            StaticLookupManager objStaticLookupManager = new StaticLookupManager();
            IList<UserLookup> userLookup = new List<UserLookup>();
            IList<StaticLookup> staticLookupvalue = new List<StaticLookup>();
            IList<StaticLookup> staticlookupTestValues = new List<StaticLookup>();
            string Field_value = string.Empty;
            string Field_Name = fieldName.Replace("|LAST MAMMOGRAM", "");//BugID:47706
            if (fieldName == "CHECK OUT NOTES" || (fieldName.Contains("VITALS NOTES") && ClientSession.PhysicianId == 0 && ClientSession.UserRole == "Medical Assistant") || fieldName == "FOLLOW_UP_REASON_NOTES")// || fieldName == "HPI_NOTES")Commented The condition for Bug Id: 30293
            {
                userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, Field_Name.ToUpper());
            }
            else
            {
                //if(ClientSession.PhysicianId!=0 && ClientSession.UserRole!="Medical Assistant")
                userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, Field_Name.ToUpper());
            }

            if (fieldName.IndexOf("LAST MAMMOGRAM") != -1)
                Field_value = fieldName.Replace("VITALS NOTES|","").ToUpper().Trim() + " TEST";
            else
                Field_value = "REASON NOT PERFORMED FOR " + value.Replace("RELOAD#$&^", "").ToUpper();

            if (value != "" && !value.Contains("RELOAD#$&^") || (value.Trim() == "" && !value.Contains("RELOAD#$&^") && fieldName.IndexOf("LAST MAMMOGRAM") != -1))
                staticLookupvalue = objStaticLookupManager.getStaticLookupByFieldNameForreason(Field_value);
            if (value != "" && value.Contains("RELOAD#$&^"))
                staticLookupvalue = objStaticLookupManager.getStaticLookupByFieldName(Field_value);

            //else
            //    userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
            //Commented for Bug ID:27478
            string val = string.Empty;


            for (int i = 0; i < staticLookupvalue.Count; i++)
            {

                if (i == 0)
                {
                    if (Field_value.IndexOf("REASON NOT PERFORMED FOR") != -1)
                        val += "$Reason Not Performed" + "|";
                    else
                        val += "$Mammogram Tests" + "|";

                    val += staticLookupvalue[i].Value + "|";
                }

                else
                {
                    val += staticLookupvalue[i].Value + "|";
                }
            }


            //if (userLookup != null && userLookup.Count > 0 && (staticLookupvalue != null && staticLookupvalue.Count > 0) || (staticlookupTestValues != null && staticlookupTestValues.Count > 0))
            //{
            //    val += "$Comments" + "|";
            //}

            for (int i = 0; i < userLookup.Count; i++)
            {
                if (i == 0)
                {
                    val += "$Comments" + "|";
                    val += userLookup[i].Value + "|";
                }
                else
                {
                    val += userLookup[i].Value + "|";
                }
            }
            if (userLookup.Count == 0)
            {
                val += "$Comments" + "|";
            }
          
            return val;
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetListBoxValues(string fieldName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            fieldName = fieldName.Replace("&apos;", "'");
            UserLookupManager objUserLookupManager = new UserLookupManager();
            IList<UserLookup> userLookup = new List<UserLookup>();
            IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
            if (fieldName.ToUpper() == "EMAIL")
            {
                PhysicianManager objPhyMngr=new PhysicianManager();
                ilstPhysicianLibrary = objPhyMngr.GetAll();
            }
            else if (fieldName.ToUpper() == "AMENDMENT NOTES")
            {
                if (ClientSession.UserRole.ToUpper() != "MEDICAL ASSISTANT")
                    userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
                else
                    userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, fieldName.ToUpper());
            }
            //CAP-1715
            else
                if (fieldName == "CHECK OUT NOTES" || (fieldName == "VITALS NOTES" && ClientSession.PhysicianId == 0 && ClientSession.UserRole == "Medical Assistant") || fieldName == "FOLLOW_UP_REASON_NOTES" || fieldName == "PROVIDER NOTES")// || fieldName == "HPI_NOTES")Commented The condition for Bug Id: 30293
            {
                userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
            }
            else
            {
                //if(ClientSession.PhysicianId!=0 && ClientSession.UserRole!="Medical Assistant")
                //CAP-2429
                //userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, fieldName.ToUpper());
                if(fieldName.ToUpper() == "MESSAGE NOTES")
                {
                    if (ClientSession.PhysicianId != 0)
                    { userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, fieldName.ToUpper()); }
                    else
                    { userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper(), ""); }
                }
                else
                {
                    userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, fieldName.ToUpper());
                }
            }

            //else
            //    userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
            //Commented for Bug ID:27478
            string val = string.Empty;
            if (fieldName.ToUpper() == "EMAIL")
            {
                for (int i = 0; i < ilstPhysicianLibrary.Count; i++)
                    if (ilstPhysicianLibrary[i].PhyEMail!=string.Empty)
                        val += ilstPhysicianLibrary[i].PhyEMail + "^" + ilstPhysicianLibrary[i].Id + "|";
            }
            else if (fieldName == "CHIEF_COMPLAINTS")
            {
                for (int i = 0; i < userLookup.Count; i++)
                    val += userLookup[i].Value + "^" + userLookup[i].Description + "|";
            }
            else if (fieldName.ToUpper().Trim() == "FOOD")
            {
                for (int i = 0; i < userLookup.Count; i++)
                    val += userLookup[i].Value + "^" + userLookup[i].Doc_Type + "^" + userLookup[i].Doc_Sub_Type + "|";
            }
            else{
                for (int i = 0; i < userLookup.Count; i++)
                    val += userLookup[i].Value + "|";
            }

            return val;
        }
         [System.Web.Services.WebMethod(EnableSession = true)]
        
        public static string GetListBoxValuesPlanFollowup(string fieldName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            fieldName = fieldName.Replace("&apos;", "'");
            string fieldName_StaticLookup = string.Empty;
            if (fieldName.ToUpper().IndexOf("BMI") != -1)
            {
                fieldName = fieldName.Replace("BMI~","");
                fieldName_StaticLookup = "BMI";
            }
            UserLookupManager objUserLookupManager = new UserLookupManager();
            IList<UserLookup> userLookup = new List<UserLookup>();
            IList<StaticLookup> staticLookup = new List<StaticLookup>();
            StaticLookupManager objStaticLookupManager = new StaticLookupManager();

            String[] FieldNames = new String[2];
            FieldNames[0] = "FOLLOWUP FOR " + fieldName_StaticLookup;
            FieldNames[1] = "FOLLOWUP REASON NOT PERFORMED FOR " + fieldName_StaticLookup;
            staticLookup = objStaticLookupManager.getStaticLookupByFieldName(FieldNames);
           
            if (ClientSession.PhysicianId == 0 && ClientSession.UserRole == "Medical Assistant")// || fieldName == "HPI_NOTES")Commented The condition for Bug Id: 30293
            {
                userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
            }
            else
            {
                //if(ClientSession.PhysicianId!=0 && ClientSession.UserRole!="Medical Assistant")
                userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, fieldName.ToUpper());
            }


            string val = string.Empty;
            IList<string> Reason_Not_PerformedList = new List<string>();
            IList<string> FollowupList = new List<string>();
            IList<string> CommentsList = new List<string>();

            foreach (StaticLookup s in staticLookup)
            {
                if (s.Field_Name.IndexOf("FOLLOWUP REASON NOT PERFORMED FOR ") != -1)
                    Reason_Not_PerformedList.Add(s.Value + "|" + s.Doc_Sub_Type);
                else if (s.Field_Name.IndexOf("FOLLOWUP FOR ") != -1)
                    FollowupList.Add(s.Value + "|" + s.Doc_Sub_Type);
            }
            foreach (UserLookup s in userLookup)
            {
                CommentsList.Add(s.Value);
            }
            var objFollowupList = new
            {
                ReasonNotPerformed = JsonConvert.SerializeObject(Reason_Not_PerformedList),
                FollowupList = JsonConvert.SerializeObject(FollowupList),
                CommentsList = JsonConvert.SerializeObject(CommentsList)
            };

            val = JsonConvert.SerializeObject(objFollowupList);
            return val;
        }


         [System.Web.Services.WebMethod(EnableSession = true)]
         public static string GetListBoxcareplanReason(string fieldName)
         {
             if (ClientSession.UserName == string.Empty)
             {
                 HttpContext.Current.Response.StatusCode = 999;
                 HttpContext.Current.Response.Status = "999 Session Expired";
                 HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                 return "Session Expired";
             }
             string valueuser = fieldName.Replace("care_plan_value-", "");
             fieldName = fieldName.Replace("&apos;", "'");
             UserLookupManager objUserLookupManager = new UserLookupManager();
             StaticLookupManager objStaticLookupManager = new StaticLookupManager();
             IList<UserLookup> userLookup = new List<UserLookup>();
             IList<StaticLookup> staticLookupvalue = new List<StaticLookup>();
             IList<StaticLookup> staticLookupvaluefollowup = new List<StaticLookup>();
             if (fieldName == "CHECK OUT NOTES" || (fieldName == "VITALS NOTES" && ClientSession.PhysicianId == 0 && ClientSession.UserRole == "Medical Assistant") || fieldName == "FOLLOW_UP_REASON_NOTES")// || fieldName == "HPI_NOTES")Commented The condition for Bug Id: 30293
             {
                 userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
             }
             else
             {
                 //if(ClientSession.PhysicianId!=0 && ClientSession.UserRole!="Medical Assistant")
                 userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.PhysicianId, fieldName.ToUpper());
             }

             staticLookupvalue = objStaticLookupManager.getStaticLookupByFieldName("REASON NOT PERFORMED FOR FOLLOWUP" + valueuser.ToUpper());
             staticLookupvaluefollowup = objStaticLookupManager.getStaticLookupByFieldName("FOLLOWUP FOR " + valueuser.ToUpper() );
             //else
             //    userLookup = objUserLookupManager.GetFieldLookupList(ClientSession.UserName, fieldName.ToUpper());
             //Commented for Bug ID:27478
             string val = string.Empty;


             for (int i = 0; i < userLookup.Count; i++)
             {
                 if (i == 0)
                 {
                     //val = "Comments" + "|";
                     //val += userLookup[i].Value + "|";
                     val = userLookup[i].Value + "|";
                 }
                 else
                 {
                     val += userLookup[i].Value + "|";
                 }
             }
             for (int i = 0; i < staticLookupvaluefollowup.Count; i++)
             {

                 if (i == 0)
                 {
                     val += "Follow Up" + "|";
                     val += staticLookupvaluefollowup[i].Value + "$" + staticLookupvaluefollowup[i].Doc_Sub_Type + "|";
                 }
                 else if (i == staticLookupvaluefollowup.Count - 1)
                 {
                     val += staticLookupvaluefollowup[i].Value + "$" + staticLookupvaluefollowup[i].Doc_Sub_Type + "|" + "Click to view more" +"|";//BugID:47383
                 }
                 else
                 {
                     val += staticLookupvaluefollowup[i].Value + "$" + staticLookupvaluefollowup[i].Doc_Sub_Type + "|";
                 }
             }
             for (int i = 0; i < staticLookupvalue.Count; i++)
             {

                 if (i == 0)
                 {
                     val += "Reason for Not Performed for Follow Up" + "|";
                     val += staticLookupvalue[i].Value + "$" + staticLookupvalue[i].Doc_Sub_Type + "|";
                 }
                 else if (i == staticLookupvalue.Count - 1)
                 {
                     val += staticLookupvalue[i].Value + "$" + staticLookupvalue[i].Doc_Sub_Type + "|" + "Click to view more"+"|";//BugID:47383
                 }
                 else
                 {
                     val += staticLookupvalue[i].Value + "$" + staticLookupvalue[i].Doc_Sub_Type + "|";
                 }

             }

             return val;
         }   
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetListBoxValuesRCM(string fieldName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            UserLookupManager objUserLookupManager = new UserLookupManager();
            IList<UserLookup> userLookup = objUserLookupManager.GetFieldLookupListRCM("ALL", fieldName, string.Empty);
            string val = string.Empty;

            for (int i = 0; i < userLookup.Count; i++)
                val += userLookup[i].Value + "|";


            return val;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static IList<string> SearchDescrption(string text)
        {
            ClientSession.HumanId = Convert.ToUInt64(text.Split('|')[0]);
            text = text.Split('|')[1];
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return new string[] { "Session Expired" };
            }
            string sInterpretationSubDocType = ConfigurationManager.AppSettings["TemplateNotes"].ToString();
            EncounterManager objUserLookupManager = new EncounterManager();
            IList<string> userLookup = objUserLookupManager.GetEncounterListArray(ClientSession.HumanId, text, sInterpretationSubDocType);
            IList<string> lstuserLookup = new List<string>();
            DateTime dtTime = DateTime.MinValue;
            for (int i = 0; i < userLookup.Count; i++)
            {
                if (userLookup[i].Split('^')[0] == "Encounters" && text == "Encounters")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].Split(';')[0].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    if (strDate.ToUpper().Contains("01-JAN-0001"))
                    {
                        HttpContext.Current.Response.StatusCode = 999;
                        HttpContext.Current.Response.Status = "999 Session Expired";
                        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                        return new string[] { "Session Expired" };
                    }
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + ";" + userLookup[i].Split('^')[2].Split(';')[1].ToString() + "^" + ClientSession.Selectedencounterid + "^" + ClientSession.SelectedFrom + "^" + userLookup[i].Split('^')[3]);
                }
                else if (userLookup[i].Split('^')[0] == "Patient Task" && text == "Patient Task")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[1].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + strDate + "^" + userLookup[i].Split('^')[2]);

                }
                else if (userLookup[i].Split('^')[0] == "Phone Encounter" && text == "Encounters")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[1].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + strDate + "^" + userLookup[i].Split('^')[2]);
                }
                else if (userLookup[i].Split('^')[0] == "Summary of Care" && text == "Summary of Care")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].Split(';')[0].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + ";" + userLookup[i].Split('^')[2].Split(';')[1].ToString());
                }
                else if (userLookup[i].Split('^')[0] == "Results" && text == "Results")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].Split('|')[0].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    if (userLookup[i].Split('^').Length > 3)
                    {//array[i].split('^')[0] + "^" + array[i].split('^')[1] + "^" + array[i].split('^')[4] + "^" + array[i].split('^')[5];
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + ClientSession.HumanId);
                    }
                    else
                    {
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^0^" + strDate + "^" + userLookup[i].Split('^')[1] + "^0^" + userLookup[i].Split('^')[2].Split('|')[1].ToString() + "^" + userLookup[i].Split('^')[2].Split('|')[2].ToString() + "^" + ClientSession.HumanId);
                    }
                }

                else if (userLookup[i].Split('^')[0] == "EncountersSub" && text == "Encounters")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + "" + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[5]+ "^" + userLookup[i].Split('^')[6]);
                }
                else if (userLookup[i].Split('^')[0] == "Others" && (text == "Document" || text == "Results"))
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    if (userLookup[i].Split('^')[5] == "Results")
                    {
                        if (userLookup[i].Split('^').Length == 7)
                        {
                            string str = userLookup[i].Split('^')[3].Split('_')[0];
                            if (userLookup[i].Split('^')[4].Split('-')[0].Trim().ToUpper() == "PATIENT DISCHARGE SUMMARY" || userLookup[i].Split('^')[4].Split('-')[0].Trim().ToUpper() == "PATIENT CLINICAL SUMMARY")
                            {
                                str = userLookup[i].Split('^')[4].Split('-')[0].Trim();
                            }
                            else if (userLookup[i].Split('^')[4].Split('-')[0].Trim().ToUpper() == "PAT EDU")
                            {
                                str = "Patient Education";
                            }
                            if (str == "Laboratory")
                            {
                                strDate = dtTime.ToString("dd-MMM-yyyy hh:mm tt");
                            }
                            lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + str + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6]);
                        }
                        else if (userLookup[i].Split('^').Length == 8)
                        {
                            if (userLookup[i].Contains("$Dateonly"))//For Bug Id 54261//To show document date without time
                                lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy") + "^" + userLookup[i].Split('^')[3].Split('_')[0] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6] + "^" + userLookup[i].Split('^')[7].Replace("$Dateonly", ""));
                            else
                                lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3].Split('_')[0] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6] + "^" + userLookup[i].Split('^')[7]);
                        }
                        else if (userLookup[i].Split('^').Length == 9)
                        {
                            lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3].Split('_')[0] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6] + "^" + userLookup[i].Split('^')[7] + "^" + userLookup[i].Split('^')[8]);
                        }
                        else
                        {
                            lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3].Split('_')[0] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId);
                        }
                    }
                    else
                    {
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6]);
                    }
                }

                else if (userLookup[i].Split('^')[0] == "Exam" && text == "Exam")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");

                    if (userLookup[i].Split('^').Length > 3)
                    {//array[i].split('^')[0] + "^" + array[i].split('^')[1] + "^" + array[i].split('^')[4] + "^" + array[i].split('^')[5];
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + ClientSession.HumanId);
                    }
                    else
                    {
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^0^" + strDate + "^" + userLookup[i].Split('^')[1] + "^0^" + userLookup[i].Split('^')[2].Split('|')[1].ToString() + "^" + userLookup[i].Split('^')[2].Split('|')[2].ToString() + "^" + ClientSession.HumanId);
                    }

                }
                else if (userLookup[i].Split('^')[0] == "StressTestReport" && text == "Results")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy");
                    //lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6]);

                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + " 00:00 AM^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[7] + "^" + userLookup[i].Split('^')[5]);
                }
               
            }

            return lstuserLookup;
        }



        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static IList<string> SearchDescrptionWebportal(string text)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return new string[] { "Session Expired" };
            }
            ClientSession.HumanId = Convert.ToUInt64(text);
            EncounterManager objUserLookupManager = new EncounterManager();
            IList<string> userLookup = objUserLookupManager.GetEncounterListArrayPatientPortal(Convert.ToUInt64(text));
            IList<string> lstuserLookup = new List<string>();
            DateTime dtTime = DateTime.MinValue;
            for (int i = 0; i < userLookup.Count; i++)
            {
                if (userLookup[i].Split('^')[0] == "Encounters")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].Split(';')[0].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add("Summary Of Care" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + ";" + userLookup[i].Split('^')[2].Split(';')[1].ToString() + "^" + ClientSession.Selectedencounterid + "^" + ClientSession.SelectedFrom);
                }
                else if (userLookup[i].Split('^')[0] == "Patient Task")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[1].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + strDate + "^" + userLookup[i].Split('^')[2]);

                }
                else if (userLookup[i].Split('^')[0] == "Phone Encounter")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[1].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + strDate + "^" + userLookup[i].Split('^')[2]);
                }
                else if (userLookup[i].Split('^')[0] == "Summary of Care")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].Split(';')[0].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + ";" + userLookup[i].Split('^')[2].Split(';')[1].ToString());
                }
                else if (userLookup[i].Split('^')[0] == "Results")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].Split('|')[0].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    if (userLookup[i].Split('^').Length > 3)
                    {//array[i].split('^')[0] + "^" + array[i].split('^')[1] + "^" + array[i].split('^')[4] + "^" + array[i].split('^')[5];
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + ClientSession.HumanId);
                    }
                    else
                    {
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^0^" + strDate + "^" + userLookup[i].Split('^')[1] + "^0^" + userLookup[i].Split('^')[2].Split('|')[1].ToString() + "^" + userLookup[i].Split('^')[2].Split('|')[2].ToString() + "^" + ClientSession.HumanId);
                    }
                }

                else if (userLookup[i].Split('^')[0] == "EncountersSub")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy");
                    lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + "" + "^" + ClientSession.HumanId);
                }
                else if (userLookup[i].Split('^')[0] == "Others")
                {
                    dtTime = Convert.ToDateTime(userLookup[i].Split('^')[2].ToString());
                    string strDate = UtilityManager.ConvertToLocal(dtTime).ToString("dd-MMM-yyyy hh:mm tt");
                    if (userLookup[i].Split('^')[5] == "Results")
                    {
                        if (userLookup[i].Split('^').Length == 7)
                        {
                            string str = userLookup[i].Split('^')[3].Split('_')[0];
                            if (userLookup[i].Split('^')[4].Split('-')[0].Trim().ToUpper() == "PATIENT DISCHARGE SUMMARY" || userLookup[i].Split('^')[4].Split('-')[0].Trim().ToUpper() == "PATIENT CLINICAL SUMMARY")
                            {
                                str = userLookup[i].Split('^')[4].Split('-')[0].Trim();
                            }
                            else if (userLookup[i].Split('^')[4].Split('-')[0].Trim().ToUpper() == "PAT EDU")
                            {
                                str = "Patient Education";
                            }
                            lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + str + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6]);
                        }
                        else if (userLookup[i].Split('^').Length == 8)
                        {
                            lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3].Split('_')[0] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId + "^" + userLookup[i].Split('^')[6] + "^" + userLookup[i].Split('^')[7]);
                        }
                        else
                        {
                            lstuserLookup.Add("ResultsSub" + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3].Split('_')[0] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId);
                        }
                    }
                    else
                    {
                        lstuserLookup.Add(userLookup[i].Split('^')[0] + "^" + userLookup[i].Split('^')[1] + "^" + strDate + "^" + userLookup[i].Split('^')[3] + "^" + userLookup[i].Split('^')[4] + "^" + userLookup[i].Split('^')[5] + "^" + ClientSession.HumanId);
                    }
                }
            }

            return lstuserLookup;
        }
        //BugID:47706
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string FindIfInMammogramTestList(string SelectedText,string TextBoxValue,string Type)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired" ;
            }
            UtilityManager um =new UtilityManager();
            string value_TextBox = um.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", TextBoxValue);
            string value_selected = um.GetSnomedfromStaticLookup("MammogramTypeList", "LAST MAMMOGRAM TEST", SelectedText);
            var showAlert = string.Empty;
            if (Type == "MandatoryAlert")
            {
                if (value_TextBox.Trim() == "")
                {
                    showAlert = "showAlert";
                }
            }
            //BugID:48015
            if (Type == "DefaultTest")
            {
                StaticLookupManager slm = new StaticLookupManager();
                IList<StaticLookup> slList = new List<StaticLookup>();
                slList = slm.getStaticLookupByFieldNameDefaultValue("LAST MAMMOGRAM TEST", "Y");

                if (value_TextBox.Trim() == "")
                {
                    if (slList != null && slList.Count > 0)
                        showAlert = slList[0].Value.Trim();
                }
            }
            else if (Type == "")
            {
                if (value_TextBox.Trim() != "" && value_selected.Trim() != "")
                {
                    showAlert = "showAlert";
                }
            }

            return showAlert;
        }
    }
}
