using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Web.Services;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace Acurus.Capella.UI
{
    public partial class RxHistory : System.Web.UI.Page
    {
        static FillPatientSummaryBarDTO fillPatSummaryBar = new FillPatientSummaryBarDTO();
        //ulong maxid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnCurEncounterID.Value = ClientSession.EncounterId.ToString();
            hdnUserRole.Value = ClientSession.UserRole.ToString();
            hdnCurProcess.Value = ClientSession.UserCurrentProcess.ToString();
            //Rcopia_MedicationManager obj = new Rcopia_MedicationManager();
            //IList <Rcopia_Medication> lst= obj.GetMaxID();
            //if (lst.Count > 0)
            //    Session["maxid"] = lst[0].Id;
        }




        public static IList<Rcopia_Medication> GetData_Xml(bool is_delete)
        {
            ulong[] MedCurEnc_ID = { ClientSession.EncounterId };
            IList<Rcopia_Medication> lstMedication = new List<Rcopia_Medication>();


            IList<string> ilsHumanTagList = new List<string>();
            ilsHumanTagList.Add("Rcopia_MedicationList");




            // if (Is_copy_previous == true)
            IList<object> humanBlobFinal = new List<object>();
            humanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilsHumanTagList);

            if (humanBlobFinal != null && humanBlobFinal.Count > 0)
            {
                if (humanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)humanBlobFinal[0]).Count; iCount++)
                    {
                        lstMedication.Add((Rcopia_Medication)((IList<object>)humanBlobFinal[0])[iCount]);
                    }

                }

            }
            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";//"Base_XML" + "_" + "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();

            //    if (itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0] != null)
            //    {
            //        xmlTagName = itemDoc.GetElementsByTagName("Rcopia_MedicationList")[0].ChildNodes;

            //        if (xmlTagName.Count > 0)
            //        {
            //            for (int j = 0; j < xmlTagName.Count; j++)
            //            {
            //                if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == ClientSession.HumanId
            //                     && xmlTagName[j].Attributes.GetNamedItem("Status").Value.Trim().ToUpper() == "ACTIVE")
            //                {
            //                    string TagName = xmlTagName[j].Name;
            //                    XmlSerializer xmlserializer = new XmlSerializer(typeof(Rcopia_Medication));
            //                    Rcopia_Medication Medication = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as Rcopia_Medication;
            //                    IEnumerable<PropertyInfo> propInfo = null;
            //                    propInfo = from obji in ((Rcopia_Medication)Medication).GetType().GetProperties() select obji;

            //                    for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                    {
            //                        XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                        {
            //                            foreach (PropertyInfo property in propInfo)
            //                            {
            //                                if (property.Name.ToUpper() == nodevalue.Name.ToUpper())
            //                                {
            //                                    if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                        property.SetValue(Medication, Convert.ToUInt64(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                        property.SetValue(Medication, Convert.ToString(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                        property.SetValue(Medication, Convert.ToDateTime(nodevalue.Value), null);
            //                                    else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                        property.SetValue(Medication, Convert.ToInt32(nodevalue.Value), null);
            //                                    else
            //                                        property.SetValue(Medication, nodevalue.Value, null);
            //                                }
            //                            }
            //                        }

            //                    }
            //                    lstMedication.Add(Medication);
            //                }
            //            }
            //        }
            //    }
            //}
            IList<Rcopia_Medication> lstMedtempLst = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> lstCurLst = new List<Rcopia_Medication>();
            if (lstMedication != null && lstMedication.Count > 0)
            {

                //lstMedication=(from m in lstMedication where m.Deleted.ToString().ToUpper()!="Y" select m).ToList<Rcopia_Medication>();
                lstMedtempLst = (from med in lstMedication where med.Encounter_ID == ClientSession.EncounterId && med.Deleted != "Y" select med).ToList<Rcopia_Medication>();
                if (lstMedtempLst != null && lstMedtempLst.Count > 0)
                {
                    lstCurLst = lstMedtempLst;
                }
                else if (!is_delete)
                {
                    ulong[] encIds = (from med in lstMedication select med.Encounter_ID).Except(MedCurEnc_ID).Distinct().ToArray();
                    ulong maxEcnID = encIds.Max();
                    lstMedtempLst = (from med in lstMedication where med.Encounter_ID == maxEcnID && med.Deleted != "Y" select med).ToList<Rcopia_Medication>();
                    lstCurLst = lstMedtempLst;
                }
            }



            ilsHumanTagList = new List<string>();
            ilsHumanTagList.Add("EncounterList");


            IList<Encounter> lstEncounter = new List<Encounter>();

            // if (Is_copy_previous == true)
            humanBlobFinal = new List<object>();
            humanBlobFinal = UtilityManager.ReadBlob(ClientSession.EncounterId, ilsHumanTagList);

            if (humanBlobFinal != null && humanBlobFinal.Count > 0)
            {
                if (humanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)humanBlobFinal[0]).Count; iCount++)
                    {
                        lstEncounter.Add((Encounter)((IList<object>)humanBlobFinal[0])[iCount]);
                    }

                }
                for (int i = 0; i < lstEncounter.Count; i++)
                {
                    fillPatSummaryBar.EncounterIDList.Add(lstEncounter[i].Id);
                    fillPatSummaryBar.EncounterDateList.Add(lstEncounter[i].Date_of_Service);
                }


            }
            //FileName = "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();
            //    if (itemDoc.GetElementsByTagName("EncounterList")[0] != null)
            //    {
            //        xmlTagName = itemDoc.GetElementsByTagName("EncounterList")[0].ChildNodes;

            //        if (xmlTagName.Count > 0)
            //        {
            //            for (int j = 0; j < xmlTagName.Count; j++)
            //            {
            //                fillPatSummaryBar.EncounterIDList.Add(Convert.ToUInt32(xmlTagName[j].Attributes.GetNamedItem("Id").Value));

            //                fillPatSummaryBar.EncounterDateList.Add(Convert.ToDateTime(xmlTagName[j].Attributes.GetNamedItem("Date_of_Service").Value));
            //            }
            //        }
            //    }
            //}
            return lstCurLst;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadRxHistory()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<UserLookup> noteslist = new List<UserLookup>();
            IList<UserLookup> freqlist = new List<UserLookup>();
            UserLookupManager Ulm = new UserLookupManager();
            Physician_DrugManager pdm = new Physician_DrugManager();
            IList<All_Drug> Drug_dtls = new List<All_Drug>();
            IList<Rcopia_Medication> lstMedication = new List<Rcopia_Medication>();
            bool is_delete = false;


            #region Fetch Medication data XML
            lstMedication = GetData_Xml(is_delete);
            #endregion

            var Medicationlst = lstMedication.Select(a => new
            {
                DrugName = a.Brand_Name,
                Route_of_admin = a.Route,
                Strength = a.Strength,
                Frequency = a.Dose_Timing,
                From_dt = Convert.ToDateTime(a.Start_Date).ToString("yyyy-MMM-dd"),
                To_Dt = Convert.ToDateTime(a.Stop_Date).ToString("yyyy-MMM-dd"),
                Notes = a.Comments,
                AdditionalNotes = a.Patient_Notes,
                Quantity = a.Quantity,
                Quantity_Unit = a.Quantity_Unit,
                dose = a.Dose,
                dose_unit = a.Dose_Unit,
                Direction = a.Dose_Other,
                Refill = a.Refills,
                Id = a.Id,
                Version = a.Version,
                dayssupply = a.Duration,
                EncID = a.Encounter_ID,
                Pharmacy = a.Substitution_Permitted,
                Pharmacy_Notes = a.Other_Notes,
                Fill_Date = Convert.ToDateTime(a.Fill_Date).ToString("yyyy-MMM-dd"),
                Human_id = a.Human_ID,
                StopNotes = a.Stop_Notes,
                RetainNotes = a.Retain_Notes,
                Status = a.Status,
                CreatedBy = a.Created_By,
                CreatedDateTime = a.Created_Date_And_Time
            }).OrderByDescending(a => a.Id);
            HttpContext.Current.Session["MedicationList"] = lstMedication;
            freqlist = Ulm.GetFieldLookupList(ClientSession.UserName, "Medication Frequency");
            Drug_dtls = pdm.GetDrugDetailsByPhyID(ClientSession.PhysicianId);
            IList<string> Druglst = new List<string>();
            IList<string> Freqlist = new List<string>();

            Druglst = Drug_dtls.Select(a => a.Drug_Name + "~" + a.Route_Of_Administration + "~" + a.Strength).ToList<string>();
            Freqlist = freqlist.Select(a => a.Value).ToList<string>();
            string druglist = JsonConvert.SerializeObject(Druglst);
            string freqlst = JsonConvert.SerializeObject(Freqlist);


            var Result = new
            {
                DrugList = Druglst,
                FreqList = freqlist,
                Medlst = Medicationlst
            };
            return JsonConvert.SerializeObject(Result);

        }



        //[System.Web.Services.WebMethod(EnableSession = true)]
        //public static string SaveRxHistory(object data)
        //{
        //    if (ClientSession.UserName == string.Empty)
        //    {
        //        HttpContext.Current.Response.StatusCode = 999;
        //        HttpContext.Current.Response.Status = "999 Session Expired";
        //        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //        return "Session Expired";
        //    }
        //    IList<Medication> Savelst = new List<Medication>();
        //    IList<Medication> Updatelst = new List<Medication>();
        //    IList<Medication> dellist = new List<Medication>();
        //    bool is_delete = false;
        //    IList<Medication> retlst = new List<Medication>();
        //    MedicationManager mm = new MedicationManager();
        //    IList<Medication> SessionMedlist = ((IList<Medication>)HttpContext.Current.Session["MedicationList"]);

        //    Medication med = new Medication();
        //    Dictionary<string, object> objMed = new Dictionary<string, object>();
        //    objMed = (Dictionary<string, object>)data;
        //    med.Drug_Name = objMed["DrugName"].ToString();
        //    med.Human_ID = ClientSession.HumanId;
        //  //  med.Encounter_ID = Convert.ToUInt32(objMed["EncID"].ToString());
        //    med.Dosage = objMed["Strength"].ToString();
        //    med.Frequency = objMed["Frequency"].ToString();
        //    med.From_Date = objMed["From_Date"].ToString();
        //    med.To_Date = objMed["To_Date"].ToString();
        //    med.Route_Of_Administration = objMed["ROA"].ToString();
        //    med.Medication_Notes = objMed["Notes"].ToString();
        //    if ((Convert.ToString(objMed["Type"])) == "save")
        //    {
        //       // med.Encounter_ID = ClientSession.EncounterId;
        //        med.Created_By = ClientSession.UserName;
        //        med.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //        Savelst.Add(med);

        //        if (SessionMedlist != null && SessionMedlist.Count > 0)
        //        {
        //            foreach (Medication SessionMed in SessionMedlist)
        //            {
        //                //if (SessionMed.Encounter_ID != ClientSession.EncounterId)//if it is not current encounter data Save as new list for the currentEncounter
        //                //{
        //                //    Medication newMed = new Medication();
        //                //    newMed = SessionMed;
        //                //    newMed.Id = 0;
        //                //    newMed.Encounter_ID = ClientSession.EncounterId;
        //                //    newMed.Created_By = ClientSession.UserName;
        //                //    newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                //    newMed.Modified_By = "";
        //                //    newMed.Modified_Date_And_Time = DateTime.MinValue;
        //                //    newMed.Version = 0;
        //                //    Savelst.Add(newMed);
        //                //}
        //            }
        //        }
        //    }
        //    else if (Convert.ToString(objMed["Type"]) == "Update")
        //    {
        //        med.Id = Convert.ToUInt64(objMed["ID"].ToString());
        //        IList<ulong> uIDs = new List<ulong>();
        //        if (SessionMedlist != null && SessionMedlist.Count > 0)
        //        {
        //            foreach (Medication SessionMed in SessionMedlist)
        //            {
        //                //if (SessionMed.Encounter_ID != ClientSession.EncounterId)//if it is not current encounter data Save as new list for the currentEncounter
        //                //{
        //                //    Medication mednew = new Medication();

        //                //    if (SessionMed.Id == med.Id)
        //                //    {
        //                //        mednew = med;
        //                //        mednew.Id = 0;
        //                //    }
        //                //    else
        //                //    {
        //                //        mednew = SessionMed;
        //                //        mednew.Id = 0;
        //                //    }
        //                //    uIDs.Add(SessionMed.Id);
        //                //    mednew.Encounter_ID = ClientSession.EncounterId;
        //                //    mednew.Created_By = ClientSession.UserName;
        //                //    mednew.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                //    mednew.Modified_By = "";
        //                //    mednew.Modified_Date_And_Time = DateTime.MinValue;
        //                //    mednew.Version = 0;
        //                //    Savelst.Add(mednew);
        //                //}
        //            }
        //        }
        //        if (!uIDs.Any(a => a.ToString() == med.Id.ToString()))
        //        {
        //            IList<Medication> medSession = (from obj in SessionMedlist where obj.Id == med.Id select obj).ToList<Medication>();
        //            if (medSession != null && medSession.Count > 0)
        //            {
        //                med.Created_By = medSession[0].Created_By;
        //                med.Created_Date_And_Time = medSession[0].Created_Date_And_Time;
        //                med.Modified_By = ClientSession.UserName;
        //                med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                med.Version = Convert.ToInt32(objMed["Version"]);
        //                Updatelst.Add(med);
        //            }

        //        }

        //    }

        //    else if (Convert.ToString(objMed["Type"]) == "del")
        //    {
        //        med.Id = Convert.ToUInt64(objMed["ID"].ToString());
        //        med.Version = Convert.ToInt32(objMed["Version"]);
        //        is_delete = true;

        //        //if (med.Encounter_ID == ClientSession.EncounterId)
        //        //{
        //        //    dellist.Add(med);
        //        //}
        //        //else if (SessionMedlist != null && SessionMedlist.Count > 0)
        //        //{
        //        //    foreach (Medication SessionMed in SessionMedlist)
        //        //    {
        //                //if (SessionMed.Id != med.Id)//while an item belonging to past encounter is deleted, all items except the deleted one gets saved for current encounter
        //                //{
        //                //    Medication newMed = new Medication();
        //                //    newMed = SessionMed;
        //                //    newMed.Id = 0;
        //                //    newMed.Encounter_ID = ClientSession.EncounterId;
        //                //    newMed.Created_By = ClientSession.UserName;
        //                //    newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
        //                //    newMed.Modified_By = "";
        //                //    newMed.Modified_Date_And_Time = DateTime.MinValue;
        //                //    newMed.Version = 0;
        //                //    Savelst.Add(newMed);
        //                //}
        //            //}
        //       // }
        //    }

        //    fillPatSummaryBar = new FillPatientSummaryBarDTO();
        //    // mm.BatchOperationsToMedication(Savelst, Updatelst, dellist);
        //    retlst = GetData_Xml(is_delete);
        //    // fillPatSummaryBar.MedHistoryList = retlst;
        //    var Medicationlst = retlst.Select(a => new
        //    {
        //        DrugName = a.Drug_Name,
        //        Route_of_admin = a.Route_Of_Administration,
        //        Strength = a.Dosage,
        //        Frequency = a.Frequency,
        //        From_dt = a.From_Date,
        //        To_Dt = a.To_Date,
        //        Notes = a.Medication_Notes,
        //        Id = a.Id,
        //        Version = a.Version,
        //      //  EncID = a.Encounter_ID
        //    });//.Where(a => a.EncID == ClientSession.EncounterId);
        //    HttpContext.Current.Session["MedicationList"] = retlst;
        //    string[] strarray = new string[2];
        //    UtilityManager objmngr = new UtilityManager();
        //    var toolTipMedlst = string.Empty;
        //    string strMedlst = objmngr.GetMedication(fillPatSummaryBar, out toolTipMedlst);
        //    strarray[0] = strMedlst;
        //    strarray[1] = toolTipMedlst;
        //    var MedSummary = strarray;
        //    var result = new
        //    {
        //        Medlst = Medicationlst,
        //        Medtooltip = MedSummary
        //    };
        //    return JsonConvert.SerializeObject(result);

        //}
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SaveRxHistory(object data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Rcopia_Medication> Savelst = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> Updatelst = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> dellist = new List<Rcopia_Medication>();
            bool is_delete = false;
            IList<Rcopia_Medication> retlst = new List<Rcopia_Medication>();
            Rcopia_MedicationManager mm = new Rcopia_MedicationManager();
            IList<Rcopia_Medication> SessionMedlist = ((IList<Rcopia_Medication>)HttpContext.Current.Session["MedicationList"]);

            Rcopia_Medication med = new Rcopia_Medication();
            Dictionary<string, object> objMed = new Dictionary<string, object>();
            objMed = (Dictionary<string, object>)data;
            med.Brand_Name = objMed["DrugName"].ToString();
            med.Generic_Name = objMed["DrugName"].ToString();
            med.Human_ID = ClientSession.HumanId;
            med.Encounter_ID = Convert.ToUInt32(objMed["EncID"].ToString());
            med.Strength = objMed["Strength"].ToString();
            med.Dose_Timing = objMed["Frequency"].ToString();
            // med.Frequency = objMed["Frequency"].ToString();
            if (objMed["From_Date"].ToString() != "")
                med.Start_Date = Convert.ToDateTime(objMed["From_Date"].ToString());
            if (objMed["To_Date"].ToString() != "")
                med.Stop_Date = Convert.ToDateTime(objMed["To_Date"].ToString());
            med.Route = objMed["ROA"].ToString();
            med.Comments = objMed["Notes"].ToString();
            med.Dose_Other = objMed["direction"].ToString();
            med.Dose = objMed["dose"].ToString();
            med.Other_Notes = objMed["Pharmacy_Notes"].ToString();

            med.Dose_Unit = objMed["dosestren"].ToString();
            // pharm: pharm,
            med.Quantity = objMed["quant"].ToString();
            med.Quantity_Unit = objMed["quantstr"].ToString();
            med.Patient_Notes = objMed["additionalnotes"].ToString();
            med.Refills = objMed["refill"].ToString();
            med.Duration = objMed["dayssupply"].ToString();
            if (objMed["lastfill"].ToString() != "")
                med.Fill_Date = Convert.ToDateTime(objMed["lastfill"].ToString());
            med.Encounter_ID = ClientSession.EncounterId;
            med.Substitution_Permitted = objMed["pharmacy"].ToString();
            med.Stop_Notes = objMed["stopnotes"].ToString();
            med.Retain_Notes = objMed["retainnotes"].ToString();

            if ((Convert.ToString(objMed["Type"])) == "save")
            {
                med.Encounter_ID = ClientSession.EncounterId;
                med.Created_By = ClientSession.UserName;
                med.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                med.Deleted = "N";
                med.Status = "ACTIVE";
                //med.Id = Convert.ToUInt64(HttpContext.Current.Session["maxid"]) + 1;
                //HttpContext.Current.Session["maxid"] = med.Id;
                Savelst.Add(med);

                if (SessionMedlist != null && SessionMedlist.Count > 0)
                {
                    foreach (Rcopia_Medication SessionMed in SessionMedlist)
                    {
                        if (SessionMed.Encounter_ID != ClientSession.EncounterId)//if it is not current encounter data Save as new list for the currentEncounter
                        {
                            Rcopia_Medication newMed = new Rcopia_Medication();
                            newMed = SessionMed;
                            newMed.Id = 0;
                            newMed.Encounter_ID = ClientSession.EncounterId;
                            newMed.Created_By = ClientSession.UserName;
                            newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            newMed.Modified_By = "";
                            newMed.Modified_Date_And_Time = DateTime.MinValue;
                            newMed.Version = 0;
                            Savelst.Add(newMed);
                        }
                    }
                }
            }
            else if (Convert.ToString(objMed["Type"]) == "Update")
            {
                med.Id = Convert.ToUInt64(objMed["ID"].ToString());
                Rcopia_MedicationManager obj = new Rcopia_MedicationManager();
                Rcopia_Medication objmedi = new Rcopia_Medication();
                objmedi = obj.GetById(Convert.ToUInt64(objMed["ID"].ToString()));
                IList<ulong> uIDs = new List<ulong>();
                if (SessionMedlist != null && SessionMedlist.Count > 0)
                {
                    foreach (Rcopia_Medication SessionMed in SessionMedlist)
                    {
                        if (SessionMed.Encounter_ID != ClientSession.EncounterId)//if it is not current encounter data Save as new list for the currentEncounter
                        {
                            Rcopia_Medication mednew = new Rcopia_Medication();

                            if (SessionMed.Id == med.Id)
                            {
                                mednew = med;
                                mednew.Id = 0;
                            }
                            else
                            {
                                mednew = SessionMed;
                                mednew.Id = 0;
                            }
                            uIDs.Add(SessionMed.Id);
                            mednew.Encounter_ID = ClientSession.EncounterId;
                            mednew.Created_By = ClientSession.UserName;
                            mednew.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            mednew.Modified_By = "";
                            mednew.Modified_Date_And_Time = DateTime.MinValue;
                            mednew.Version = 0;
                            mednew.Status = "ACTIVE";
                            Savelst.Add(mednew);
                        }
                    }
                }
                if (!uIDs.Any(a => a.ToString() == med.Id.ToString()))
                {
                    IList<Rcopia_Medication> medSession = (from obj1 in SessionMedlist where obj1.Id == med.Id select obj1).ToList<Rcopia_Medication>();
                    if (medSession != null && medSession.Count > 0)
                    {
                        med.Created_By = medSession[0].Created_By;
                        med.Created_Date_And_Time = medSession[0].Created_Date_And_Time;
                        med.Modified_By = ClientSession.UserName;
                        med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        med.Version = Convert.ToInt32(objMed["Version"]);
                        med.Created_By = objmedi.Created_By;
                        med.Created_Date_And_Time = objmedi.Created_Date_And_Time;
                        med.Status = "INACTIVE";
                        Updatelst.Add(med);
                    }

                }

            }

            else if (Convert.ToString(objMed["Type"]) == "del")
            {
                med.Id = Convert.ToUInt64(objMed["ID"].ToString());
                med.Version = Convert.ToInt32(objMed["Version"]);
                med.Status = objMed["status"].ToString();
                med.Created_By = objMed["createdby"].ToString();
                med.Created_Date_And_Time = Convert.ToDateTime(objMed["createddatetime"].ToString());
                med.Modified_By = ClientSession.UserName;
                med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                med.Deleted = "Y";
                is_delete = true;

                Boolean bIsOldEncounterDel = false;
                IList<Rcopia_Medication> tempRCopiaMedList = new List<Rcopia_Medication>();

                var medicrecord = from m in SessionMedlist where m.Id == med.Id && m.Encounter_ID != ClientSession.EncounterId select m;
                tempRCopiaMedList = medicrecord.ToList<Rcopia_Medication>();
                if (tempRCopiaMedList != null && tempRCopiaMedList.Count > 0)
                {
                    bIsOldEncounterDel = true;
                }

                if (bIsOldEncounterDel == false)
                {
                    Updatelst.Add(med);
                }
                else
                {
                    if (SessionMedlist != null && SessionMedlist.Count > 0)
                    {
                        foreach (Rcopia_Medication SessionMed in SessionMedlist)
                        {
                            // if (SessionMed.Id != med.Id)//while an item belonging to past encounter is deleted, all items except the deleted one gets saved for current encounter
                            // {
                            Rcopia_Medication newMed = new Rcopia_Medication();
                            if (tempRCopiaMedList.Count > 0 && SessionMed.Id == tempRCopiaMedList[0].Id)
                            {
                                newMed = tempRCopiaMedList[0];
                                newMed.Deleted = "Y";
                                newMed.Encounter_ID = ClientSession.EncounterId;
                                newMed.Created_By = ClientSession.UserName;
                                newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                newMed.Modified_By = "";
                                newMed.Modified_Date_And_Time = DateTime.MinValue;
                                newMed.Version = 0;
                                newMed.Status = "ACTIVE";
                                Savelst.Add(newMed);
                            }
                            else
                            {
                                newMed = SessionMed;
                                newMed.Id = 0;
                                newMed.Encounter_ID = ClientSession.EncounterId;
                                newMed.Created_By = ClientSession.UserName;
                                newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                newMed.Modified_By = "";
                                newMed.Modified_Date_And_Time = DateTime.MinValue;
                                newMed.Version = 0;
                                newMed.Status = "ACTIVE";
                                Savelst.Add(newMed);
                            }
                            // }
                        }
                    }
                }
            }

            fillPatSummaryBar = new FillPatientSummaryBarDTO();
            mm.SaveUpdateDelete_DBAndXML_WithTransaction(ref Savelst, ref Updatelst, dellist, string.Empty, true, true, ClientSession.HumanId, string.Empty);
            retlst = GetData_Xml(is_delete);


            // fillPatSummaryBar.MedHistoryList = retlst;
            fillPatSummaryBar.MedicationList = retlst;
            var Medicationlst = retlst.Where(a => a.Encounter_ID == ClientSession.EncounterId && a.Status == "ACTIVE").Select(a => new
            {
                DrugName = a.Brand_Name,
                Route_of_admin = a.Route,
                Strength = a.Strength,
                Frequency = a.Dose_Timing,
                From_dt = Convert.ToDateTime(a.Start_Date).ToString("yyyy-MMM-dd"),
                To_Dt = Convert.ToDateTime(a.Stop_Date).ToString("yyyy-MMM-dd"),
                Notes = a.Comments,
                AdditionalNotes = a.Patient_Notes,
                Quantity = a.Quantity,
                Quantity_Unit = a.Quantity_Unit,
                dose = a.Dose,
                dose_unit = a.Dose_Unit,
                Direction = a.Dose_Other,
                Refill = a.Refills,
                Id = a.Id,
                dayssupply = a.Duration,
                Version = a.Version,
                Pharmacy = a.Substitution_Permitted,
                Pharmacy_Notes = a.Other_Notes,
                EncID = a.Encounter_ID,
                Fill_Date = Convert.ToDateTime(a.Fill_Date).ToString("yyyy-MMM-dd"),
                Human_id = a.Human_ID,
                StopNotes = a.Stop_Notes,
                RetainNotes = a.Retain_Notes,
                Status = a.Status,
                CreatedBy = a.Created_By,
                CreatedDateTime = a.Created_Date_And_Time

            }).OrderByDescending(a => a.Id);




            HttpContext.Current.Session["MedicationList"] = retlst;
            string[] strarray = new string[2];
            UtilityManager objmngr = new UtilityManager();
            var toolTipMedlst = string.Empty;
            string strMedlst = objmngr.GetMedication(fillPatSummaryBar, out toolTipMedlst);
            strarray[0] = strMedlst;
            strarray[1] = toolTipMedlst;
            var MedSummary = strarray;
            var result = new
            {
                Medlst = Medicationlst,
                Medtooltip = MedSummary
            };
            return JsonConvert.SerializeObject(result);

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string LoadDrugsDetails(string fieldName)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            Physician_DrugManager pdm = new Physician_DrugManager();
            IList<All_Drug> Drug_dtls = new List<All_Drug>();
            // Drug_dtls = pdm.GetDrugDetailsByPhyID(ClientSession.PhysicianId);
            All_DrugManager objalldrug = new All_DrugManager();
            Drug_dtls = objalldrug.SearchDrugForRxHistory(fieldName);
            IList<string> Druglst = new List<string>();
            Druglst = Drug_dtls.Select(a => a.Drug_Name + "~" + a.Route_Of_Administration + "~" + a.Strength).ToList<string>();
            // Druglst = Drug_dtls.Select(a => a.Drug_Name).ToList<string>();
            return JsonConvert.SerializeObject(Druglst);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string StopRxHistory(object data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Rcopia_Medication> Savelst = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> Updatelst = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> dellist = new List<Rcopia_Medication>();
            bool is_delete = false;
            IList<Rcopia_Medication> retlst = new List<Rcopia_Medication>();
            Rcopia_MedicationManager mm = new Rcopia_MedicationManager();
            IList<Rcopia_Medication> SessionMedlist = ((IList<Rcopia_Medication>)HttpContext.Current.Session["MedicationList"]);
            Rcopia_MedicationManager objRcopiaManager = new Rcopia_MedicationManager();
            // ulong MaxRcopiaID = 0;
            //IList<Rcopia_Medication> lst = objRcopiaManager.GetMaxID();
            //if (lst.Count > 0)
            //    MaxRcopiaID = lst[0].Id;
            Rcopia_Medication med = new Rcopia_Medication();
            Dictionary<string, object> objMed = new Dictionary<string, object>();
            objMed = (Dictionary<string, object>)data;
            if (Convert.ToString(objMed["Type"]) == "del")
            {
                med.Id = Convert.ToUInt64(objMed["ID"].ToString());
                if (objMed["EncID"].ToString() != "")
                {
                    med.Encounter_ID = Convert.ToUInt32(objMed["EncID"].ToString());
                    med.Version = Convert.ToInt32(objMed["Version"]);
                }
                else
                {
                    IList<Rcopia_Medication> medSession = (from obj in SessionMedlist where obj.Id == med.Id select obj).ToList<Rcopia_Medication>();
                    if (medSession != null && medSession.Count > 0)
                    {
                        med.Encounter_ID = medSession[0].Encounter_ID;
                        med.Version = medSession[0].Version;
                    }
                }

                //is_delete = true;

                if (med.Encounter_ID == ClientSession.EncounterId)
                {
                    IList<Rcopia_Medication> medSession = (from obj in SessionMedlist where obj.Id == med.Id select obj).ToList<Rcopia_Medication>();
                    if (medSession != null && medSession.Count > 0)
                    {
                        med = medSession[0];
                        med.Stop_Notes = medSession[0].Stop_Notes;
                        med.Status = "INACTIVE";
                        med.Modified_By = ClientSession.UserName;
                        med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        Updatelst.Add(med);
                        Rcopia_Medication medn = new Rcopia_Medication();
                        medn = (Rcopia_Medication)med.Clone();
                        // medn.Id = MaxRcopiaID+1;
                        //HttpContext.Current.Session["maxid"] = medn.Id;
                        medn.Version = 0;
                        medn.Modified_By = ClientSession.UserName;
                        medn.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                        medn.Status = "ACTIVE";
                        medn.Stop_Notes = objMed["StopNotes"].ToString();
                        medn.Stop_Date = Convert.ToDateTime(objMed["StopDate"].ToString());
                        Savelst.Add(medn);
                    }
                }
                else if (SessionMedlist != null && SessionMedlist.Count > 0)
                {
                    foreach (Rcopia_Medication SessionMed in SessionMedlist)
                    {
                        if (SessionMed.Id != med.Id)//while an item belonging to past encounter is deleted, all items except the deleted one gets saved for current encounter
                        {
                            Rcopia_Medication newMed = new Rcopia_Medication();
                            newMed = SessionMed;
                            newMed.Id = 0;
                            newMed.Encounter_ID = ClientSession.EncounterId;
                            newMed.Created_By = ClientSession.UserName;
                            newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                            newMed.Modified_By = "";
                            newMed.Modified_Date_And_Time = DateTime.MinValue;
                            newMed.Version = 0;
                            newMed.Status = "ACTIVE";
                            Savelst.Add(newMed);
                        }
                    }
                }
            }

            fillPatSummaryBar = new FillPatientSummaryBarDTO();
            mm.SaveUpdateDelete_DBAndXML_WithTransaction(ref Savelst, ref Updatelst, dellist, string.Empty, true, true, ClientSession.HumanId, string.Empty);


            is_delete = false;
            IList<Rcopia_Medication> Medlist = GetData_Xml(is_delete);
            HttpContext.Current.Session["MedicationList"] = Medlist;
            fillPatSummaryBar.MedicationList = Medlist;

            string[] strarray = new string[2];
            UtilityManager objmngr = new UtilityManager();
            var toolTipMedlst = string.Empty;
            string strMedlst = objmngr.GetMedication(fillPatSummaryBar, out toolTipMedlst);
            strarray[0] = strMedlst;
            strarray[1] = toolTipMedlst;
            var MedSummary = strarray;
            var result = new
            {
                Medtooltip = MedSummary
            };
            return JsonConvert.SerializeObject(result);
            //var result = "Stop |";
            //HttpContext.Current.Session["MedicationList"] = Medlist;

            //if (objMed["EncID"].ToString() == "")
            //{
            //    result = "RECONCILE |" + objMed["ID"].ToString();
            //}
            //var Medicationlst = Medlist.Select(a => new
            //{
            //    DrugName = a.Brand_Name,
            //    Route_of_admin = a.Route,
            //    Strength = a.Strength,
            //    Frequency = a.Dose_Timing,
            //    From_dt = Convert.ToDateTime(a.Start_Date).ToString("yyyy-MMM-dd"),
            //    To_Dt = Convert.ToDateTime(a.Stop_Date).ToString("yyyy-MMM-dd"),
            //    Notes = a.Comments,
            //    AdditionalNotes=a.Patient_Notes,
            //    Quantity=a.Quantity,
            //    Quantity_Unit=a.Quantity_Unit,
            //    dose=a.Dose,
            //    dose_unit=a.Dose_Unit,
            //    Direction=a.Dose_Other,
            //    Refill=a.Refills,
            //    Id = a.Id,
            //    dayssupply=a.Duration,
            //    Version = a.Version,
            //    Pharmacy=a.Substitution_Permitted,
            //    Pharmacy_Notes = a.Other_Notes,
            //     EncID = a.Encounter_ID,
            //    Fill_Date = Convert.ToDateTime(a.Fill_Date).ToString("yyyy-MMM-dd"),
            //    Human_id = a.Human_ID,
            //    StopNotes = a.Stop_Notes,
            //    RetainNotes = a.Retain_Notes,
            //    Status = a.Status,
            //    CreatedBy = a.Created_By,
            //    CreatedDateTime = a.Created_Date_And_Time,
            //    result = "RECONCILE |" + objMed["ID"].ToString()
            //});
            //return JsonConvert.SerializeObject(Medicationlst);
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string StopRetainRXConsile(object[] StopHistory, object[] RetainHistory)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            //ulong MaxRcopiaID = 0;
            //Rcopia_MedicationManager objRcopiaManager = new Rcopia_MedicationManager();
            //IList<Rcopia_Medication> lst = objRcopiaManager.GetMaxID();
            //if (lst.Count > 0)
            //    MaxRcopiaID = lst[0].Id;
            if (RetainHistory.Count() > 0)
            {
                IList<Rcopia_Medication> Savelst = new List<Rcopia_Medication>();
                IList<Rcopia_Medication> Updatelst = new List<Rcopia_Medication>();
                IList<Rcopia_Medication> dellist = new List<Rcopia_Medication>();
                Rcopia_MedicationManager mm = new Rcopia_MedicationManager();
                IList<Rcopia_Medication> SessionMedlist = ((IList<Rcopia_Medication>)HttpContext.Current.Session["MedicationList"]);
                for (int i = 0; i < RetainHistory.Count(); i++)
                {
                    //bool is_delete = false;
                    //IList<Medication> retlst = new List<Medication>();                                        
                    //Medication med = new Medication();
                    Dictionary<string, object> objMed = new Dictionary<string, object>();
                    objMed = (Dictionary<string, object>)RetainHistory[i];

                    ulong ID = Convert.ToUInt64(objMed["id"].ToString());
                    IList<Rcopia_Medication> medSession = (from obj in SessionMedlist where obj.Id == ID select obj).ToList<Rcopia_Medication>();
                    ulong EncounterID = ClientSession.EncounterId;

                    if (objMed["EncounterId"].ToString() != "")
                    {
                        EncounterID = Convert.ToUInt32(objMed["EncounterId"].ToString());
                        // med.Version = Convert.ToInt32(objMed["Version"]);
                    }
                    else
                    {
                        if (medSession != null && medSession.Count > 0)
                        {
                            EncounterID = medSession[0].Encounter_ID;
                            //med.Version = medSession[0].Version;
                        }
                    }

                    // is_delete = true;

                    if (EncounterID == ClientSession.EncounterId)
                    {
                        if (medSession != null && medSession.Count > 0)
                        {
                            Rcopia_Medication med = new Rcopia_Medication();
                            med = (Rcopia_Medication)medSession[0].Clone();
                            med.Encounter_ID = EncounterID;
                            med.Modified_By = ClientSession.UserName;
                            med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            med.Retain_Notes = objMed["RetainNotes"].ToString();
                            Updatelst.Add(med);
                        }
                    }

                }
                mm.SaveUpdateDelete_DBAndXML_WithTransaction(ref Savelst, ref Updatelst, dellist, string.Empty, true, true, ClientSession.HumanId, string.Empty);
            }
            if (StopHistory.Count() > 0)
            {
                Rcopia_MedicationManager mm = new Rcopia_MedicationManager();
                IList<Rcopia_Medication> Savelst = new List<Rcopia_Medication>();
                IList<Rcopia_Medication> Updatelst = new List<Rcopia_Medication>();
                IList<Rcopia_Medication> dellist = new List<Rcopia_Medication>();
                IList<Rcopia_Medication> SessionMedlist = ((IList<Rcopia_Medication>)HttpContext.Current.Session["MedicationList"]);
                for (int i = 0; i < StopHistory.Count(); i++)
                {
                    //bool is_delete = false;
                    //IList<Medication> retlst = new List<Medication>();                                       
                    //Medication med = new Medication();
                    Dictionary<string, object> objMed = new Dictionary<string, object>();
                    objMed = (Dictionary<string, object>)StopHistory[i];
                    ulong ID = Convert.ToUInt64(objMed["id"].ToString());

                    IList<Rcopia_Medication> medSession = (from obj in SessionMedlist where obj.Id == ID select obj).ToList<Rcopia_Medication>();
                    ulong EncounterID = ClientSession.EncounterId;

                    if (Convert.ToString(objMed["Type"]) == "del")
                    {
                        if (objMed["EncounterId"].ToString() != "")
                        {
                            EncounterID = Convert.ToUInt32(objMed["EncounterId"].ToString());
                        }
                        else
                        {
                            if (medSession != null && medSession.Count > 0)
                            {
                                EncounterID = medSession[0].Encounter_ID;
                            }
                        }

                        //is_delete = true;

                        if (EncounterID == ClientSession.EncounterId)
                        {
                            if (medSession != null && medSession.Count > 0)
                            {
                                Rcopia_Medication med = new Rcopia_Medication();
                                med = (Rcopia_Medication)medSession[0].Clone();
                                med.Encounter_ID = EncounterID;
                                med.Modified_By = ClientSession.UserName;
                                med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                                med.Status = "INACTIVE";
                                Updatelst.Add(med);
                                Rcopia_Medication medn = (Rcopia_Medication)medSession[0].Clone();
                                // medn.Id = MaxRcopiaID + 1;
                                // HttpContext.Current.Session["maxid"] = medn.Id;
                                medn.Version = 0;
                                medn.Human_ID = ClientSession.HumanId;
                                medn.Created_By = ClientSession.UserName;
                                medn.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                medn.Stop_Date = Convert.ToDateTime(objMed["StopDate"].ToString());
                                medn.Stop_Notes = objMed["StopNotes"].ToString();
                                medn.Status = "ACTIVE";
                                Savelst.Add(medn);
                            }
                        }
                        else if (SessionMedlist != null && SessionMedlist.Count > 0)
                        {
                            foreach (Rcopia_Medication SessionMed in SessionMedlist)
                            {
                                if (SessionMed.Id != ID)//while an item belonging to past encounter is deleted, all items except the deleted one gets saved for current encounter
                                {
                                    Rcopia_Medication newMed = new Rcopia_Medication();
                                    newMed = SessionMed;
                                    //newMed.Id = 0;
                                    newMed.Encounter_ID = ClientSession.EncounterId;
                                    newMed.Created_By = ClientSession.UserName;
                                    newMed.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                    //newMed.Modified_By = "";
                                    //newMed.Modified_Date_And_Time = DateTime.MinValue;
                                    //newMed.Version = 0;
                                    newMed.Status = "ACTIVE";
                                    Savelst.Add(newMed);
                                }
                            }
                        }
                    }

                    //fillPatSummaryBar = new FillPatientSummaryBarDTO();                    

                }
                mm.SaveUpdateDelete_DBAndXML_WithTransaction(ref Savelst, ref Updatelst, dellist, string.Empty, true, true, ClientSession.HumanId, string.Empty);
            }
            bool is_delete_flag = false;
            IList<Rcopia_Medication> Medlist = GetData_Xml(is_delete_flag);

            HttpContext.Current.Session["MedicationList"] = Medlist;

            var Medicationlst = Medlist.Select(a => new
            {
                DrugName = a.Brand_Name,
                Route_of_admin = a.Route,
                Strength = a.Strength,
                Frequency = a.Dose_Timing,
                From_dt = Convert.ToDateTime(a.Start_Date).ToString("yyyy-MMM-dd"),
                To_Dt = Convert.ToDateTime(a.Stop_Date).ToString("yyyy-MMM-dd"),
                Notes = a.Comments,
                AdditionalNotes = a.Patient_Notes,
                Quantity = a.Quantity,
                Quantity_Unit = a.Quantity_Unit,
                dose = a.Dose,
                dose_unit = a.Dose_Unit,
                Direction = a.Dose_Other,
                Refill = a.Refills,
                Id = a.Id,
                Version = a.Version,
                dayssupply = a.Duration,
                EncID = a.Encounter_ID,
                Pharmacy = a.Substitution_Permitted,
                Pharmacy_Notes = a.Other_Notes,
                Fill_Date = Convert.ToDateTime(a.Fill_Date).ToString("yyyy-MMM-dd"),
                Human_id = a.Human_ID,
                StopNotes = a.Stop_Notes,
                RetainNotes = a.Retain_Notes,
                Status = a.Status,
                CreatedBy = a.Created_By,
                CreatedDateTime = a.Created_Date_And_Time
            }).OrderByDescending(a => a.Id);
            return JsonConvert.SerializeObject(Medicationlst);

        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SavetoRxHistory(object[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Rcopia_Medication> Updatelst = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> Savelst = null;
            IList<Rcopia_Medication> dellist = null;

            IList<Rcopia_Medication> Medlist = ((IList<Rcopia_Medication>)HttpContext.Current.Session["MedicationList"]);
            Rcopia_MedicationManager mm = new Rcopia_MedicationManager();
            foreach (object[] oj in data)
            {
                Rcopia_Medication med = null;
                ulong med_id = Convert.ToUInt32(oj[0].ToString());
                med = Medlist.Where(a => a.Id == med_id).FirstOrDefault();
                if (med != null)
                {
                    med.Retain_Notes = oj[1].ToString();
                    med.Modified_By = ClientSession.UserName;
                    med.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                    Updatelst.Add(med);
                }
            }
            if (Updatelst.Count > 0)
            {
                mm.SaveUpdateDelete_DBAndXML_WithTransaction(ref Savelst, ref Updatelst, dellist, string.Empty, true, true, ClientSession.HumanId, string.Empty);
            }
            bool is_delete = false;
            Medlist = GetData_Xml(is_delete);
            HttpContext.Current.Session["MedicationList"] = Medlist;
            var Medicationlst = Medlist.Select(a => new
            {
                DrugName = a.Brand_Name,
                Route_of_admin = a.Route,
                Strength = a.Strength,
                Frequency = a.Dose_Timing,
                From_dt = Convert.ToDateTime(a.Start_Date).ToString("yyyy-MMM-dd"),
                To_Dt = Convert.ToDateTime(a.Stop_Date).ToString("yyyy-MMM-dd"),
                Notes = a.Comments,
                AdditionalNotes = a.Patient_Notes,
                Quantity = a.Quantity,
                Quantity_Unit = a.Quantity_Unit,
                dose = a.Dose,
                dose_unit = a.Dose_Unit,
                Direction = a.Dose_Other,
                Refill = a.Refills,
                Id = a.Id,
                Version = a.Version,
                dayssupply = a.Duration,
                EncID = a.Encounter_ID,
                Pharmacy = a.Substitution_Permitted,
                Pharmacy_Notes = a.Other_Notes,
                Fill_Date = Convert.ToDateTime(a.Fill_Date).ToString("yyyy-MMM-dd"),
                Human_id = a.Human_ID,
                StopNotes = a.Stop_Notes,
                RetainNotes = a.Retain_Notes,
                Status = a.Status,
                CreatedBy = a.Created_By,
                CreatedDateTime = a.Created_Date_And_Time
            }).OrderByDescending(a => a.Id);
            return JsonConvert.SerializeObject(Medicationlst);
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string RequestSentLog(object[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Request_Sent_Log> Request_sent_log = new List<Request_Sent_Log>();
            string tblSource = "Medication History";

            foreach (object obj in data)
            {
                Request_Sent_Log rsl = new Request_Sent_Log();
                rsl.Source = tblSource;
                rsl.Source_ID = Convert.ToUInt64(obj.ToString());
                rsl.Created_By = ClientSession.UserName;
                rsl.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                Request_sent_log.Add(rsl);
            }
            RequestSentLogManager rslm = new RequestSentLogManager();
            rslm.SaveToRequestSentLog(ref Request_sent_log);

            var RequestSentLogIDs = Request_sent_log.Select(a => new
            {
                SourceID = a.Source_ID,
                ReqSentLogID = a.Id

            }).OrderByDescending(a => a.SourceID);


            IList<string> ilsHumanTagList = new List<string>();
            ilsHumanTagList.Add("Rcopia_AllergyList");




            // if (Is_copy_previous == true)
            IList<object> humanBlobFinal = new List<object>();
            humanBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilsHumanTagList);
            IList<Rcopia_Allergy> lst = new List<Rcopia_Allergy>();
            IList<string> DrugAllergyList = new List<string>();
            if (humanBlobFinal != null && humanBlobFinal.Count > 0)
            {
                if (humanBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)humanBlobFinal[0]).Count; iCount++)
                    {
                        lst.Add((Rcopia_Allergy)((IList<object>)humanBlobFinal[0])[iCount]);
                    }
                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (Convert.ToUInt64(lst[j].Human_ID) == ClientSession.HumanId
                            && lst[j].Status.Trim().ToUpper() == "ACTIVE"
                            && lst[j].Deleted == "N")
                        {
                            DrugAllergyList.Add(Convert.ToString(lst[j].Allergy_Name));
                        }
                    }


                }

            }

            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";//"Base_XML" + "_" + "Encounter" + "_" + ClientSession.EncounterId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    itemDoc.Load(XmlText);
            //    XmlText.Close();

            //    if (itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0] != null)
            //    {
            //        xmlTagName = itemDoc.GetElementsByTagName("Rcopia_AllergyList")[0].ChildNodes;

            //        if (xmlTagName.Count > 0)
            //        {
            //            for (int j = 0; j < xmlTagName.Count; j++)
            //            {
            //                if (Convert.ToUInt64(xmlTagName[j].Attributes.GetNamedItem("Human_ID").Value) == ClientSession.HumanId
            //                    && xmlTagName[j].Attributes.GetNamedItem("Status").Value.Trim().ToUpper() == "ACTIVE"
            //                    && xmlTagName[j].Attributes.GetNamedItem("Deleted").Value.Equals("N"))
            //                {
            //                    DrugAllergyList.Add(Convert.ToString(xmlTagName[j].Attributes.GetNamedItem("Allergy_Name").Value));
            //                }
            //            }
            //        }
            //    }
            //}

            var jsonReqSent = JsonConvert.SerializeObject(RequestSentLogIDs);
            var jsonCreatedby = JsonConvert.SerializeObject(ClientSession.UserName);
            var jsonDrugAllergyList = JsonConvert.SerializeObject(DrugAllergyList);
            var result = "{\"RequestSentList\" :" + jsonReqSent + "," + "\"CreatedBY\":" + jsonCreatedby + "," + "\"DrugAllergyList\":" + jsonDrugAllergyList + "}";
            return JsonConvert.SerializeObject(result);
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string ResponseReceivedLog(object[] data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Response_Received_Log> Response_Received_Log = new List<Response_Received_Log>();

            foreach (object obj in data)
            {
                Response_Received_Log rsl = new Response_Received_Log();
                rsl.Response_Sent_Log_ID = Convert.ToUInt64(obj.ToString());
                rsl.Created_By = ClientSession.UserName;
                rsl.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                Response_Received_Log.Add(rsl);
            }
            ResponseReceivedLogManager rslm = new ResponseReceivedLogManager();
            rslm.SaveToResponseReceivedLog(ref Response_Received_Log);

            return "";
        }
    }
}