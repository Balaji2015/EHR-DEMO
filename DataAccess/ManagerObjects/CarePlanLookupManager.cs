
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using Acurus.Capella.Core.DTO;
using System.Xml;
using System.IO;


namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface ICarePlanLookupManager : IManagerBase<CarePlanLookup, ulong>
    {
        IList<CarePlanDTO> GetCarePlanLookupFromLocal(string Field_Name);
        IList<CarePlanDTO> GetCarePlanLookupFromLocalForThin(string Field_Name, ulong HumanID, ulong UlEncounterID, string Gender, int iMonths);
    }
    public partial class CarePlanLookupManager : ManagerBase<CarePlanLookup, ulong>, ICarePlanLookupManager
    {
        #region Constructors

        public CarePlanLookupManager()
            : base()
        {

        }
        public CarePlanLookupManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public IList<CarePlanDTO> GetCarePlanLookupFromLocal(string Field_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<CarePlanDTO> CareDTO = new List<CarePlanDTO>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                CarePlanDTO PlanDto = null;
                ArrayList aryCare = null;
                IQuery query = iMySession.GetNamedQuery("Fill.CarePlan.Local").SetString(0, Convert.ToString(Field_Name));

                aryCare = new ArrayList(query.List());
                for (int i = 0; i < aryCare.Count; i++)
                {
                    object[] ob = (object[])aryCare[i];
                    PlanDto = new CarePlanDTO();
                    PlanDto.Care_Name = Convert.ToString(ob[0]);
                    PlanDto.Care_Name_Value = Convert.ToString(ob[1]);
                    PlanDto.Options = Convert.ToString(ob[2]);
                    PlanDto.Status = Convert.ToString(ob[3]);
                    PlanDto.Care_Plan_Notes = Convert.ToString(ob[4]);
                    PlanDto.Created_By = Convert.ToString(ob[5]);
                    PlanDto.Created_Date_And_Time = Convert.ToDateTime(ob[6]);
                    PlanDto.Care_Plan_Lookup_ID = Convert.ToUInt64(ob[7]);
                    PlanDto.Plan_Care_ID = Convert.ToUInt64(ob[8]);
                    PlanDto.Version = Convert.ToInt32(ob[9]);
                    PlanDto.Controls = Convert.ToString(ob[10]);
                    PlanDto.Plan_Date = Convert.ToString(ob[11]);
                    PlanDto.Status_Value = Convert.ToString(ob[12]);

                    CareDTO.Add(PlanDto);
                }
                iMySession.Close();
            }
            return CareDTO;
        }
        public IList<CarePlanDTO> GetCarePlanLookupFromLocalForThin(string Field_Name, ulong ulHumanID, ulong UlEncounterID, string Gender, int iMonths)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<CarePlanDTO> CareDTO = new List<CarePlanDTO>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                CarePlanDTO PlanDto = null;
                ArrayList aryCare = null;

                IList<PatientResults> VitalsValue = new List<PatientResults>();
                IList<PatientResults> LatestVitalsValue = new List<PatientResults>();
                IList<string> strArray = new List<string>();
                strArray.Add("ALL");
                strArray.Add(Gender);

                ArrayList arrayList = new ArrayList();

                Hashtable result = new Hashtable();
                Hashtable hashResultList = new Hashtable();
                string sTemp1 = string.Empty;
                string sTemp2 = string.Empty;

                IQuery query = iMySession.GetNamedQuery("Fill.CarePlan.Local");
                query.SetParameter(0, UlEncounterID.ToString());
                query.SetString(1, Convert.ToString(Field_Name));
                query.SetInt32(2, iMonths);
                query.SetParameterList("gender", strArray.ToArray<string>());

                //query.SetString(0, Convert.ToString(Field_Name));
                //query.SetInt32(1, iMonths);
                //query.SetParameterList("gender", strArray.ToArray<string>());
                aryCare = new ArrayList(query.List());

                for (int i = 0; i < aryCare.Count; i++)
                {
                    object[] ob = (object[])aryCare[i];
                    PlanDto = new CarePlanDTO();
                    PlanDto.Care_Name = Convert.ToString(ob[0]);
                    PlanDto.Care_Name_Value = Convert.ToString(ob[1]);
                    PlanDto.Options = Convert.ToString(ob[2]);
                    PlanDto.Status = Convert.ToString(ob[3]);
                    PlanDto.Care_Plan_Notes = Convert.ToString(ob[4]);
                    PlanDto.Created_By = Convert.ToString(ob[5]);
                    PlanDto.Created_Date_And_Time = Convert.ToDateTime(ob[6]);
                    PlanDto.Care_Plan_Lookup_ID = Convert.ToUInt64(ob[7]);
                    PlanDto.Plan_Care_ID = Convert.ToUInt64(ob[8]);
                    PlanDto.Version = Convert.ToInt32(ob[9]);
                    PlanDto.Controls = Convert.ToString(ob[10]);
                    PlanDto.Plan_Date = Convert.ToString(ob[11]);
                    PlanDto.Status_Value = Convert.ToString(ob[12]);
                    PlanDto.Master_ID = Convert.ToUInt64(ob[13]);
                    PlanDto.Control_Name = Convert.ToString(ob[15]);
                    PlanDto.Additional_Rule = Convert.ToString(ob[16]);
                    PlanDto.Parent_Rule_ID = Convert.ToInt32(ob[17]);
                    PlanDto.Modified_By = Convert.ToString(ob[18]);
                    PlanDto.Modified_Date_And_Time = Convert.ToDateTime(ob[19]);
                    //PlanDto.Followup_Plan = Convert.ToString(ob[20]);
                    //PlanDto.Reason_Not_Performed = Convert.ToString(ob[21]);
                    PlanDto.Snomed_Code = Convert.ToString(ob[20]);

                    if (ob[14] != null)// && PlanDto.Care_Name_Value.ToUpper() != "BMI")// && PlanDto.Care_Name_Value.ToUpper() != "BP SYS/DIA")
                    {
                        if (ob[14].ToString().Trim() != string.Empty)
                        {
                            if (!result.ContainsKey(PlanDto.Parent_Rule_ID))
                            {
                                string sQuery = Convert.ToString(ob[14]);
                                if (sQuery.Contains(":EncounterID"))
                                    sQuery = sQuery.Replace(":EncounterID", UlEncounterID.ToString());
                                sQuery = sQuery.Replace(":HumanID", ulHumanID.ToString());
                                sQuery = sQuery.Replace(":AdditionalRule", string.Empty);
                                ISQLQuery procsql1 = session.GetISession().CreateSQLQuery(sQuery);
                                arrayList = new ArrayList(procsql1.List());

                                result.Add(PlanDto.Parent_Rule_ID, arrayList.Count.ToString() + "$" + ob[14]);
                                hashResultList.Add(PlanDto.Parent_Rule_ID, arrayList);
                            }
                        }
                        else
                        {
                            if (!result.ContainsKey(PlanDto.Parent_Rule_ID))
                            {
                                object[] obj = null;
                                //object[] obj = (object[])aryCare[PlanDto.Parent_Rule_ID];
                                //if(aryCare.)
                                var objCheck = (from object[] careObj in aryCare where Convert.ToInt32(careObj[13]) == PlanDto.Parent_Rule_ID select careObj).ToList();
                                if (objCheck.Count > 0)
                                {
                                    obj = (object[])(objCheck[0]);
                                    if (Convert.ToString(obj[14]).Trim() != string.Empty)
                                    {
                                        string sQuery = Convert.ToString(obj[14]);
                                        if (sQuery.Contains(":EncounterID"))
                                            sQuery = sQuery.Replace(":EncounterID", UlEncounterID.ToString());
                                        sQuery = sQuery.Replace(":HumanID", ulHumanID.ToString());
                                        sQuery = sQuery.Replace(":AdditionalRule", string.Empty);
                                        ISQLQuery procsql1 = session.GetISession().CreateSQLQuery(sQuery);
                                        arrayList = new ArrayList(procsql1.List());

                                        result.Add(PlanDto.Parent_Rule_ID, arrayList.Count.ToString() + "$" + obj[14]);
                                        hashResultList.Add(PlanDto.Parent_Rule_ID, arrayList);
                                    }
                                }
                            }
                        }

                        int iID = PlanDto.Parent_Rule_ID;


                        if (result[iID] != null)
                        {
                            sTemp1 = (result[iID]).ToString().Split('$')[0];
                            sTemp2 = (result[iID]).ToString().Split('$')[1];
                        }
                        arrayList = (ArrayList)hashResultList[PlanDto.Parent_Rule_ID];  //(result[PlanDto.Parent_Rule_ID]).ToString().Split('$')[2];
                        if (sTemp1.Trim() != string.Empty && Convert.ToUInt64(sTemp1) > 0)
                        {
                            string sQuery = sTemp2;
                            string Year = string.Empty;
                            string codes = string.Empty;
                            sQuery = sQuery.Replace(":HumanID", ulHumanID.ToString());
                            sQuery = sQuery.Replace(":AdditionalRule", PlanDto.Additional_Rule);
                            sQuery = sQuery.Replace(":EncounterID", UlEncounterID.ToString());//BugID:51648
                            string[] strPalnDtoArray = PlanDto.Additional_Rule.Split('|');
                            if (PlanDto.Additional_Rule.IndexOf('|') == -1 && PlanDto.Additional_Rule.Trim() != string.Empty)//BugID:51648
                            {
                                ISQLQuery procsql1 = session.GetISession().CreateSQLQuery(sQuery);
                                ArrayList list = new ArrayList(procsql1.List());
                                arrayList = list;
                            }
                            else
                            {
                                if (strPalnDtoArray.Length > 2)
                                {
                                    string System_Name = PlanDto.Additional_Rule.Split('|')[0];
                                    string Satus = PlanDto.Additional_Rule.Split('|')[1];
                                    string sYear = PlanDto.Additional_Rule.Split('|')[2];
                                    if (arrayList != null)
                                    {
                                        var ResultQuery = (from object[] num in arrayList
                                                           where System_Name.Trim().Contains(num[num.Count() - 3].ToString()) && num[num.Count() - 3].ToString() != "" && num[1].ToString().Contains(sYear.Trim()) == true
                                                           && !Satus.Trim().Contains(num[num.Count() - 2].ToString()) && num[num.Count() - 2].ToString() != ""
                                                           select num).ToArray();
                                        ArrayList list = new ArrayList(ResultQuery);

                                        arrayList = list;
                                    }
                                }
                                else if (PlanDto.Additional_Rule.Contains('|'))
                                {
                                    codes = PlanDto.Additional_Rule.Split('|')[0];
                                    Year = PlanDto.Additional_Rule.Split('|')[1];
                                }
                                else
                                {
                                    codes = "";
                                    Year = "";
                                }
                                if (Year != "")
                                {
                                    if (arrayList != null)
                                    {
                                        var ResultQuery = (from object[] num in arrayList
                                                           where codes.Trim().Contains(num[num.Count() - 1].ToString()) && num[num.Count() - 1].ToString() != "" && num[1].ToString().Contains(Year.Trim()) == true
                                                           select num).ToArray();
                                        ArrayList list = new ArrayList(ResultQuery);

                                        arrayList = list;
                                    }
                                }
                                if (codes.Trim() != "")
                                {
                                    if (arrayList != null)
                                    {
                                        var ResultQuery1 = (from object[] num in arrayList
                                                            where codes.Trim().Contains(num[num.Count() - 1].ToString()) && num[num.Count() - 1].ToString() != ""
                                                            select num).ToArray();
                                        ArrayList list1 = new ArrayList(ResultQuery1);

                                        arrayList = list1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //PlanDto.Status = "No";
                            /* PlanDto.Status_Value = "";
                             PlanDto.Status = "";
                             PlanDto.Plan_Date = "";*/
                            CareDTO.Add(PlanDto);
                            continue;
                        }

                        if (arrayList != null && arrayList.Count != 0)
                        {
                            object ires = (object)arrayList[0];
                            if (((object[])ires).Count() > 4)
                            {
                                if ((((object[])ires)[3]) != null)
                                {
                                    if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("BP"))
                                    {
                                        string Value = Convert.ToString(((object[])ires)[2]);
                                        //Jira CAP-2293
                                        ////JIra CAP-2200
                                        //if (Value != string.Empty)
                                        if (Value != string.Empty && Value.Contains(','))
                                        {
                                            PlanDto.Status_Value = Value.Split(',')[0];
                                            PlanDto.Status = Value.Split(',')[1];
                                        }
                                        else if (Value != string.Empty && Char.IsNumber(Value.Replace("/", ""), 0))
                                        {
                                            PlanDto.Status_Value = Value;
                                        }
                                        else if (Value != string.Empty)
                                        {
                                            PlanDto.Status = Value;
                                        }

                                        //Jira CAP-1771
                                        //PlanDto.Plan_Date = Convert.ToDateTime(Convert.ToString(((object[])ires)[1])).ToString("yyyy-MMM-dd hh:mm:ss");
                                        PlanDto.Plan_Date = Convert.ToDateTime(Convert.ToString(((object[])ires)[1])).ToString("yyyy-MMM-dd hh:mm:ss tt");
                                        //For bug Id 55648
                                        if (PlanDto.Status == "Pre-Hypertensive" || PlanDto.Status == "First Hypertensive" || PlanDto.Status == "Second Hypertensive")
                                        {
                                            string strFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                                            if (File.Exists(strFilePath) == true)
                                            {
                                                XmlDocument xmldoc = new XmlDocument();
                                                XmlTextReader xmltext = new XmlTextReader(strFilePath);
                                                xmldoc.Load(xmltext);
                                                XmlNodeList xmlNodelst = xmldoc.GetElementsByTagName("FollowupForBPStatusList");
                                                if (xmlNodelst != null && PlanDto.Care_Plan_Notes.Trim() == "")
                                                {
                                                    PlanDto.Care_Plan_Notes = xmlNodelst[0].InnerText; //For git Id 1594 
                                                }

                                            }
                                        }
                                        //End For bug Id 55648
                                        //if (Value.Contains("/"))
                                        //{
                                        //    string[] bpVal = Value.Split('/');
                                        //    if (bpVal.Length > 0)
                                        //    {
                                        //        if (Convert.ToUInt64(bpVal[0]) > 140 && Convert.ToUInt64(bpVal[1]) > 90)
                                        //        {
                                        //            //PlanDto.Status = "No";
                                        //            PlanDto.Status = "";
                                        //            PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                        //        }
                                        //        else
                                        //        {
                                        //            PlanDto.Status = "Yes";
                                        //            PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                        //        }
                                        //    }
                                        //}
                                    }
                                    if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("BMI"))
                                    {
                                        string Value = Convert.ToString(((object[])ires)[2]);

                                        PlanDto.Status_Value = Value.Split(',')[0];
                                        if (Value.Split(',').Length > 1)
                                            PlanDto.Status = Value.Split(',')[1];
                                        //Jira CAP-1771
                                       // PlanDto.Plan_Date = Convert.ToDateTime(Convert.ToString(((object[])ires)[1])).ToString("yyyy-MMM-dd hh:mm:ss");
                                        PlanDto.Plan_Date = Convert.ToDateTime(Convert.ToString(((object[])ires)[1])).ToString("yyyy-MMM-dd hh:mm:ss tt");

                                    }
                                    else if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("HBA1C"))
                                    {
                                        try
                                        {
                                            double Value = Convert.ToDouble(((object[])ires)[2]);
                                            if (Value > 9)
                                            {
                                                //PlanDto.Status = "No";
                                                PlanDto.Status = "";
                                                PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                                PlanDto.Status_Value = ((object[])ires)[2].ToString();
                                            }
                                            else
                                            {
                                                PlanDto.Status = "Yes";
                                                PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                                PlanDto.Status_Value = ((object[])ires)[2].ToString();
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("LDL"))
                                    {
                                        double Value = 0;
                                        if (ires != null)
                                        {
                                            try
                                            {
                                                Value = Convert.ToDouble(((object[])ires)[2]);

                                                if (Value > 100)
                                                {
                                                    //PlanDto.Status = "No";
                                                    PlanDto.Status = "";
                                                    PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                                    PlanDto.Status_Value = Convert.ToString(((object[])ires)[2]);
                                                }
                                                else
                                                {
                                                    PlanDto.Status = "Yes";
                                                    PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                                    PlanDto.Status_Value = Convert.ToString(((object[])ires)[2]);
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    else if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("ADVANCE_DIRECTIVE"))
                                    {
                                        PlanDto.Status = UppercaseFirst(Convert.ToString(((object[])ires)[1]));
                                        if (PlanDto.Care_Plan_Notes.Trim() == "")
                                            PlanDto.Care_Plan_Notes = Convert.ToString(((object[])ires)[2]);//For git Id 1594 
                                    }
                                    else if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("MARITAL STATUS"))
                                    {
                                        PlanDto.Status = Convert.ToString(((object[])ires)[0]);
                                    }
                                    else if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("GENDER IDENTITY"))
                                    {
                                        PlanDto.Status = Convert.ToString(((object[])ires)[0]);
                                        if (PlanDto.Care_Plan_Notes.Trim() == "")
                                            PlanDto.Care_Plan_Notes = Convert.ToString(((object[])ires)[1]);//For git Id 1594 
                                    }
                                    else if (Convert.ToString(((object[])ires)[3]).ToUpper().Contains("SEXUAL ORIENTATION"))
                                    {
                                        PlanDto.Status = Convert.ToString(((object[])ires)[0]);
                                        if (PlanDto.Care_Plan_Notes.Trim() == "")
                                            PlanDto.Care_Plan_Notes = Convert.ToString(((object[])ires)[1]);//For git Id 1594 
                                    }
                                    //Cap - 606
                                    else if (Convert.ToString(((object[])ires)[2]).ToUpper().Contains("VACCINE"))
                                    {
                                        PlanDto.Status = Convert.ToString(((object[])ires)[3]);
                                        if (PlanDto.Status.ToUpper() == "YES")
                                            PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                        else
                                            PlanDto.Plan_Date = "";
                                    }

                                    else
                                    {
                                        PlanDto.Status = "Yes";
                                        PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                        PlanDto.Status_Value = Convert.ToString(((object[])ires)[2]);//Convert.ToString(arrlst[1]);
                                    }
                                }
                            }
                            else if (((object[])ires).Count() > 3)
                            {                               
                                    PlanDto.Status = "Yes";
                                    PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                    PlanDto.Status_Value = ((object[])ires)[2].ToString();
                            }
                            else
                            {
                                if (Convert.ToString(((object[])ires)[0]).ToUpper().Contains("TOBACCO"))
                                {
                                    PlanDto.Status = Convert.ToString(((object[])ires)[1]);
                                    PlanDto.Plan_Date = Convert.ToString(((object[])ires)[2]);
                                }
                               
                                else
                                {
                                    PlanDto.Status = "Yes";
                                    PlanDto.Plan_Date = Convert.ToString(((object[])ires)[1]);
                                }
                            }

                        }
                        else
                        {
                            //PlanDto.Status = "No";
                            /* PlanDto.Status_Value = "";
                             PlanDto.Status = "";
                             PlanDto.Plan_Date = "";*/
                        }
                    }
                    //else if (PlanDto.Care_Name_Value.ToUpper() == "BMI")
                    //{

                    //    ISQLQuery sql2 = iMySession.CreateSQLQuery("Select a.* from patient_results a where a.Encounter_ID = '" + UlEncounterID.ToString() + "' and a.human_id = '" + ulHumanID.ToString() + "' and  (a.Loinc_Observation ='BMI' or a.Loinc_Observation ='BMI STATUS') and Results_Type='Vitals' order by a.Captured_date_and_time desc,a.vitals_group_id desc").AddEntity("a", typeof(PatientResults));
                    //   LatestVitalsValue = sql2.List<PatientResults>();
                    //    if (LatestVitalsValue.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            //PlanDto.Vitals_BMI = LatestVitalsValue[0].Value;
                    //            //PlanDto.Vitals_BMI_Status_Value = LatestVitalsValue[1].Value;
                    //            //PlanDto.Plan_Date = LatestVitalsValue[1].Captured_date_and_time.ToString();

                    //            PlanDto.Status_Value = LatestVitalsValue[0].Value;
                    //            PlanDto.Status = LatestVitalsValue[1].Value;
                    //            PlanDto.Plan_Date = LatestVitalsValue[1].Captured_date_and_time.ToString();

                    //        }
                    //        catch
                    //        {
                    //        }
                    //    }

                    //}

                    //else if (PlanDto.Care_Name_Value.ToUpper() == "BP SYS/DIA")
                    //{

                    //    ISQLQuery sql2 = iMySession.CreateSQLQuery("Select a.* from patient_results a where a.Encounter_ID = '" + UlEncounterID.ToString() + "' and a.human_id = '" + ulHumanID.ToString() + "' and  (a.Loinc_Observation ='BP-Sitting Sys/Dia' or a.Loinc_Observation ='BP-Sitting Sys/Dia Status') and Results_Type='Vitals' order by a.Captured_date_and_time desc,a.vitals_group_id desc,a.Loinc_Observation asc").AddEntity("a", typeof(PatientResults));
                    //    LatestVitalsValue = sql2.List<PatientResults>();
                    //    if (LatestVitalsValue.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            //PlanDto.Vitals_BP = LatestVitalsValue[0].Value;
                    //            //PlanDto.Vitals_BP_Status_Value = LatestVitalsValue[1].Value;
                    //            //PlanDto.Plan_Date = LatestVitalsValue[1].Captured_date_and_time.ToString();
                    //            if (LatestVitalsValue[0].Loinc_Observation.ToUpper().Contains("STATUS"))
                    //            {
                    //                PlanDto.Status = LatestVitalsValue[0].Value;
                    //                PlanDto.Status_Value = LatestVitalsValue[1].Value;
                    //            }

                    //            else
                    //            {
                    //                PlanDto.Status_Value = LatestVitalsValue[0].Value;
                    //                PlanDto.Status = LatestVitalsValue[1].Value;
                    //            }
                    //            PlanDto.Plan_Date = Convert.ToDateTime(LatestVitalsValue[1].Captured_date_and_time).ToString("yyyy-MMM-dd hh:mm:ss");
                    //        }
                    //        catch
                    //        {
                    //        }
                    //    }

                    //}

                    CareDTO.Add(PlanDto);
                }
                iMySession.Close();
            }
            return CareDTO;
        }


        public string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }


        private SocialHistory GetSocialByHumanID(ulong ulHumanID)
        {
            SocialHistory socialHisList = new SocialHistory();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
                ICriteria crt = iMySession.CreateCriteria(typeof(SocialHistory)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Social_Info", "Marital Status"));

                if (crt.List<SocialHistory>().Count > 0)
                    socialHisList = crt.List<SocialHistory>()[0];

                iMySession.Close();
            }
            return socialHisList;

        }
        #endregion
    }
}
