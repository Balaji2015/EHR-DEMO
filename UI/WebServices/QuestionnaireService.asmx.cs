using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System.Xml.Linq;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for QuestionnaireService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class QuestionnaireService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public string LoadQuestionnarieTab()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
         //   IList<Questionnaire_Lookup> categoryList = null;
          //  QuestionnaireLookupManager questionMngr = new QuestionnaireLookupManager();
          //  categoryList = questionMngr.GetHealthQuestionCategoryList(ClientSession.PhysicianUserName);
            string sCategory = string.Empty;

            //Added for Performance
            //if (File.Exists(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\QuestionnaireTab_Lookup.xml"))
            //{
            //XDocument xmlQuestionnaireLookup = XDocument.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath+"ConfigXML\\QuestionnaireTab_Lookup.xml");
           
            //    IEnumerable<XElement> ilstQuestionnaire = xmlQuestionnaireLookup.Element("questionnairelookuplist")
            //        .Elements("questionnaire").Where(aa => aa.Attribute("user_name").Value.ToString() == ClientSession.PhysicianUserName);
            //    if (ilstQuestionnaire != null && ilstQuestionnaire.Count() > 0)
            //    {
            //        string[] sQuestionnaire_Category = ilstQuestionnaire.Attributes("Questionnaire_category").First().Value.ToString().Split(',');
            //        for (int i = 0; i < sQuestionnaire_Category.Count(); i++)
            //        {
            //            if (sQuestionnaire_Category[i].Split('|').Count() > 0)
            //            {
            //                if (sQuestionnaire_Category[i].Split('|')[1].ToUpper() == "Y")
            //                {
            //                    if (i == 0)
            //                    {
            //                        sCategory = sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$Y";
            //                    }
            //                    else
            //                    {
            //                        sCategory = sCategory + "^" + sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$Y";
            //                    }
            //                }
            //                else
            //                {
            //                    if (i == 0)
            //                    {
            //                        sCategory = sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$N";
            //                    }
            //                    else
            //                    {
            //                        sCategory = sCategory + "^" + sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$N";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}


            //CAP-2784
            QuestionnaireList objques = new QuestionnaireList();
            objques = ConfigureBase<QuestionnaireList>.ReadJson("QuestionnaireTab_Lookup.json");

            if (objques != null)
            {
                if (objques.questionnaire != null && objques.questionnaire.Count > 0)
                {
                    List<Questionnaire> listquestionnaire = new List<Questionnaire>();
                    listquestionnaire = objques.questionnaire.Where(a => a.user_name == ClientSession.PhysicianUserName).ToList();
                    if (listquestionnaire != null && listquestionnaire.Count() > 0)
                    {
                        string[] sQuestionnaire_Category = listquestionnaire.Select(s => s.Questionnaire_category).First().ToString().Split(',');
                        for (int i = 0; i < sQuestionnaire_Category.Count(); i++)
                        {
                            if (sQuestionnaire_Category[i].Split('|').Count() > 0)
                            {
                                if (sQuestionnaire_Category[i].Split('|')[1].ToUpper() == "Y")
                                {
                                    if (i == 0)
                                    {
                                        sCategory = sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$Y";
                                    }
                                    else
                                    {
                                        sCategory = sCategory + "^" + sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$Y";
                                    }
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        sCategory = sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$N";
                                    }
                                    else
                                    {
                                        sCategory = sCategory + "^" + sQuestionnaire_Category[i].Split('|')[0].ToString().Replace("/", "") + "$N";
                                    }
                                }
                            }
                        }
                    }
                }
            }
















                //for (int i = 0; i < categoryList.Count; i++)
                //{
                //    if (categoryList[i].Is_Ros_Type == "Y")
                //    {
                //        if (i == 0)
                //        {
                //            sCategory = categoryList[i].Questionnaire_Category.Replace("/", "") + "$Y";
                //        }
                //        else
                //        {
                //            sCategory = sCategory + "^" + categoryList[i].Questionnaire_Category.Replace("/", "") + "$Y";
                //        }
                //    }
                //    else
                //    {
                //        if (i == 0)
                //        {
                //            sCategory = categoryList[i].Questionnaire_Category.Replace("/", "") + "$N";
                //        }
                //        else
                //        {
                //            sCategory = sCategory + "^" + categoryList[i].Questionnaire_Category.Replace("/", "") + "$N";
                //        }
                //    }
                //}
                return sCategory;
        }
    }
}
