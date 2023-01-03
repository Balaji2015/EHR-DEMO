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

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for PotentialDiagnosisService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PotentialDiagnosisService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]

        public string LoadPotentialDiagnosis(string data)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sMacAddress = string.Empty;
            IList<PotentialDiagnosis> LoadPotentialGrid = new List<PotentialDiagnosis>();
            PotentialDiagnosisManager obj_problemMgr = new PotentialDiagnosisManager();

            IList<String> prblmicd = new List<String>();
            string sColumnHide = string.Empty;
            if (data == "")
                sColumnHide = "disabled";


            //string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
            //string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    XmlDocument itemDoc = new XmlDocument();
            //    XmlTextReader XmlText = new XmlTextReader(strXmlFilePath);
            //    XmlNodeList xmlTagName = null;
            //    //  itemDoc.Load(XmlText);
            //    using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //    {
            //        itemDoc.Load(fs);

            //        XmlText.Close();
            //        if (itemDoc.GetElementsByTagName("PotentialDiagnosisList")[0] != null)
            //        {
            //            if (itemDoc.GetElementsByTagName("PotentialDiagnosisList").Count > 0)
            //            {
            //                xmlTagName = itemDoc.GetElementsByTagName("PotentialDiagnosisList")[0].ChildNodes;

            //                if (xmlTagName.Count > 0)
            //                {
            //                    for (int j = 0; j < xmlTagName.Count; j++)
            //                    {

            //                        string TagName = xmlTagName[j].Name;
            //                        XmlSerializer xmlserializer = new XmlSerializer(typeof(PotentialDiagnosis));
            //                        PotentialDiagnosis objPotential = xmlserializer.Deserialize(new XmlNodeReader(xmlTagName[j])) as PotentialDiagnosis;
            //                        IEnumerable<PropertyInfo> propInfo = null;
            //                        if (objPotential != null)
            //                        {
            //                            propInfo = from obji in ((PotentialDiagnosis)objPotential).GetType().GetProperties() select obji;

            //                            for (int i = 0; i < xmlTagName[j].Attributes.Count; i++)
            //                            {
            //                                XmlNode nodevalue = xmlTagName[j].Attributes[i];
            //                                {
            //                                    foreach (PropertyInfo property in propInfo)
            //                                    {
            //                                        if (property.Name == nodevalue.Name)
            //                                        {
            //                                            if (property.PropertyType.Name.ToUpper() == "UINT64")
            //                                                property.SetValue(objPotential, Convert.ToUInt64(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "STRING")
            //                                                property.SetValue(objPotential, Convert.ToString(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "DATETIME")
            //                                                property.SetValue(objPotential, Convert.ToDateTime(nodevalue.Value), null);
            //                                            else if (property.PropertyType.Name.ToUpper() == "INT32")
            //                                                property.SetValue(objPotential, Convert.ToInt32(nodevalue.Value), null);
            //                                            else
            //                                                property.SetValue(objPotential, nodevalue.Value, null);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                            if ((data == "true" || data == "ShowAllFalse") && objPotential.Move_To_Assessment != "Y")
            //                            {
            //                                LoadPotentialGrid.Add(objPotential);
            //                            }
            //                            if (data == "" || data == "ShowAllTrue")
            //                            {
            //                                LoadPotentialGrid.Add(objPotential);
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
            
            IList<string> ilstPotentialDiagnosisTagList = new List<string>();
            ilstPotentialDiagnosisTagList.Add("PotentialDiagnosisList");

            IList<object> ilstPotentialDiagnosisBlobFinal = new List<object>();
            ilstPotentialDiagnosisBlobFinal = UtilityManager.ReadBlob(ClientSession.HumanId, ilstPotentialDiagnosisTagList);

            if (ilstPotentialDiagnosisBlobFinal != null && ilstPotentialDiagnosisBlobFinal.Count > 0)
            {
                if (ilstPotentialDiagnosisBlobFinal[0] != null)
                {
                    for (int iCount = 0; iCount < ((IList<object>)ilstPotentialDiagnosisBlobFinal[0]).Count; iCount++)
                    {
                        if ((data == "true" || data == "ShowAllFalse") && ((PotentialDiagnosis)((IList<object>)ilstPotentialDiagnosisBlobFinal[0])[iCount]).Move_To_Assessment != "Y")
                        {
                            LoadPotentialGrid.Add((PotentialDiagnosis)((IList<object>)ilstPotentialDiagnosisBlobFinal[0])[iCount]);
                        }
                        if (data == "" || data == "ShowAllTrue")
                        {
                            LoadPotentialGrid.Add((PotentialDiagnosis)((IList<object>)ilstPotentialDiagnosisBlobFinal[0])[iCount]);
                        }
                    }
                }
            }

                    //LoadPotentialGrid = obj_problemMgr.GetFromPotentialDiagnosisList(ClientSession.HumanId, sMacAddress);
                    string jsons = "";
            string json = new JavaScriptSerializer().Serialize(LoadPotentialGrid);
            string jsonHideColumn = new JavaScriptSerializer().Serialize(sColumnHide);
            jsons = "{\"ICD10List\" :" + json + "," + "\"EnableColumn\" :" + jsonHideColumn + "}";
            return jsons;
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

            AllICD_9Manager objAllICD_9Manager = new AllICD_9Manager();
            string data = string.Empty;
            string type = string.Empty;
            if (text != string.Empty)
            {
                data = text.Split('|')[0];
                type = text.Split('|')[1];
            }
            string[] ResultDescpList = objAllICD_9Manager.GetDescriptionListPotentialDiagnosis(data, type, "ICD_10").ToArray();
            var jsonString = JsonConvert.SerializeObject(ResultDescpList);
            return jsonString;
        }

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


        [WebMethod(EnableSession = true)]
        public string SavePotentialDiagnosisList(object[] aryInsert, object[] aryUpdate, object[] aryDelete)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            PotentialDiagnosisManager potentialDiagnosismngr = new PotentialDiagnosisManager();
            PotentialDiagnosis objPotentialDiagnosis = null;
            IList<PotentialDiagnosis> ilstPotentialDiagnosisInsert = new List<PotentialDiagnosis>();
            IList<PotentialDiagnosis> ilstPotentialDiagnosisUpdate = new List<PotentialDiagnosis>();
            IList<PotentialDiagnosis> ilstPotentialDiagnosisDelete = new List<PotentialDiagnosis>();
            foreach (object objInsertList in aryInsert)
            {
                objPotentialDiagnosis = new PotentialDiagnosis();
                objPotentialDiagnosis.ICD_Code = Convert.ToString(objInsertList.ToString().Split('~')[0]);
                objPotentialDiagnosis.ICD_Description = Convert.ToString(objInsertList.ToString().Split('~')[1]);
                objPotentialDiagnosis.Notes = Convert.ToString(objInsertList.ToString().Split('~')[2]).Trim(); ;
                objPotentialDiagnosis.Source = Convert.ToString(objInsertList.ToString().Split('~')[3]);
                objPotentialDiagnosis.Human_ID = ClientSession.HumanId;
                objPotentialDiagnosis.Created_By = ClientSession.UserName;
                objPotentialDiagnosis.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objPotentialDiagnosis.Move_To_Assessment = Convert.ToString(objInsertList.ToString().Split('~')[4]);
                objPotentialDiagnosis.Version_Year = Convert.ToString(objInsertList.ToString().Split('~')[5]);
                ilstPotentialDiagnosisInsert.Add(objPotentialDiagnosis);
            }

            foreach (object objUpdateList in aryUpdate)
            {
                objPotentialDiagnosis = new PotentialDiagnosis();
                objPotentialDiagnosis.ICD_Code = Convert.ToString(objUpdateList.ToString().Split('~')[0]);
                objPotentialDiagnosis.ICD_Description = Convert.ToString(objUpdateList.ToString().Split('~')[1]);
                objPotentialDiagnosis.Notes = Convert.ToString(objUpdateList.ToString().Split('~')[2]).Trim();
                objPotentialDiagnosis.Id = Convert.ToUInt16(objUpdateList.ToString().Split('~')[3]);
                objPotentialDiagnosis.Version = Convert.ToInt16(objUpdateList.ToString().Split('~')[4]);
                objPotentialDiagnosis.Source = Convert.ToString(objUpdateList.ToString().Split('~')[5]);
                objPotentialDiagnosis.Human_ID = ClientSession.HumanId;
                objPotentialDiagnosis.Created_By = ClientSession.UserName;
                objPotentialDiagnosis.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objPotentialDiagnosis.Modified_By = ClientSession.UserName;
                objPotentialDiagnosis.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objPotentialDiagnosis.Move_To_Assessment = Convert.ToString(objUpdateList.ToString().Split('~')[6]);
                objPotentialDiagnosis.Version_Year = Convert.ToString(objUpdateList.ToString().Split('~')[7]);
                ilstPotentialDiagnosisUpdate.Add(objPotentialDiagnosis);
            }

            foreach (object objDeleteList in aryDelete)
            {
                objPotentialDiagnosis = new PotentialDiagnosis();
                objPotentialDiagnosis.ICD_Code = Convert.ToString(objDeleteList.ToString().Split('~')[0]);
                objPotentialDiagnosis.ICD_Description = Convert.ToString(objDeleteList.ToString().Split('~')[1]);
                objPotentialDiagnosis.Notes = Convert.ToString(objDeleteList.ToString().Split('~')[2]);
                objPotentialDiagnosis.Id = Convert.ToUInt16(objDeleteList.ToString().Split('~')[3]);
                objPotentialDiagnosis.Version = Convert.ToInt16(objDeleteList.ToString().Split('~')[4]);
                objPotentialDiagnosis.Source = Convert.ToString(objDeleteList.ToString().Split('~')[5]);
                objPotentialDiagnosis.Human_ID = ClientSession.HumanId;
                objPotentialDiagnosis.Created_By = ClientSession.UserName;
                objPotentialDiagnosis.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                objPotentialDiagnosis.Modified_By = ClientSession.UserName;
                objPotentialDiagnosis.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                objPotentialDiagnosis.Move_To_Assessment = Convert.ToString(objDeleteList.ToString().Split('~')[6]);
                objPotentialDiagnosis.Version_Year = Convert.ToString(objDeleteList.ToString().Split('~')[7]);
                ilstPotentialDiagnosisDelete.Add(objPotentialDiagnosis);
            }
            IList<PotentialDiagnosis> ilstReturn = new List<PotentialDiagnosis>();
            if (ilstPotentialDiagnosisInsert.Count > 0 || ilstPotentialDiagnosisUpdate.Count > 0 || ilstPotentialDiagnosisDelete.Count > 0)
            {
                ilstReturn = potentialDiagnosismngr.SaveUpdateDeletePotentialDiagnosis(ilstPotentialDiagnosisInsert, ilstPotentialDiagnosisUpdate, ilstPotentialDiagnosisDelete, ClientSession.HumanId);
            }
            string jsons = "";
            string sColumnHide = string.Empty;
            //if(!tab)
            sColumnHide = "disabled";
            //else sColumnHide="enable"
            string json = new JavaScriptSerializer().Serialize(ilstReturn);
            string jsonHideColumn = new JavaScriptSerializer().Serialize(sColumnHide);
            jsons = "{\"ICD10List\" :" + json + "," + "\"EnableColumn\" :" + jsonHideColumn + "}";
            return jsons;
        }

    }
}
