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
using Acurus.Capella.Core.DTO;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace Acurus.Capella.UI
{
    public partial class frmFindReferralPhysician : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Find/Add Provider - " + ClientSession.UserName;
        }
        #endregion

        #region Web Methods
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetProviderDetailsByTokens(string text_searched , string IsMenulevel)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {
                if (HttpContext.Current.Session["PreviousProviderKeywordCriteria"] != null
                    && HttpContext.Current.Session["PreviousProviderList"] != null
                    && HttpContext.Current.Session["PreviousProviderKeywordCriteria"].ToString().Trim().ToLower() == text_searched.ToLower())
                {
                    var lstResult = JsonConvert.DeserializeObject(HttpContext.Current.Session["PreviousProviderList"].ToString());// HttpContext.Current.Session["PreviousProviderList"];
                    
                    var lstFinalResult = new
                    {
                        Matching_Result = lstResult
                    };
                    return JsonConvert.SerializeObject(lstFinalResult);
                }
                else
                {
                    FindPhysican physician_dto = new FindPhysican();
                    PhysicianManager objPhysicianManager = new PhysicianManager();
                    physician_dto = objPhysicianManager.FindPhysician(text_searched, IsMenulevel);
                
                    
                    PhysicianFacilityDTO obj1=new PhysicianFacilityDTO();
                  //  obj1.PhyFirstName="Click here to add physician";
                    //physician_dto.PhyList.Add(obj1);
                    var lstResult = (from Phy in physician_dto.PhyList
                                     select
                                     new
                                     {
                                         label =
                                            Phy.PhyPrefix + " " + Phy.PhyFirstName + " " + Phy.PhyMiddleName + (string.IsNullOrWhiteSpace(Phy.PhyLastName)? " " : ", " + Phy.PhyLastName) + (string.IsNullOrWhiteSpace(Phy.PhySuffix) ? string.Empty : "(" + Phy.PhySuffix + ")") + " | " +
                                                              "NPI:" + Phy.PhyNPI +" | " +
                                                              Phy.PhySpecialtyCode + " | " +
                                                              "Facility:" + Phy.PhyFacility + " | " +
                                                              "Address: " + Phy.PhyAddrs + ", " +
                                                              Phy.PhyCity + "," +
                                                              Phy.PhyState + " " +
                                                              Phy.PhyZip + " | " +
                                                              ((Phy.PhyPhone.Trim()) != "" ? "Phone No:" + Phy.PhyPhone + " | " : "") +
                                                              (Phy.PhyFax.Trim() != "" ? "Fax No:" + Phy.PhyFax : ""),
                                         value = new
                                         {
                                             ulPhyId = Phy.PhyId,
                                             //CAP-2008
                                             sPhyName = Phy.PhyPrefix + " " + Phy.PhyFirstName + " " + Phy.PhyMiddleName + (string.IsNullOrWhiteSpace(Phy.PhyLastName) ? " " : ", " + Phy.PhyLastName) + " " + Phy.PhySuffix,
                                             sPhySuffix = Phy.PhySuffix,
                                             sPhyshortName = (string.IsNullOrWhiteSpace(Phy.PhyPrefix)?"":Phy.PhyPrefix.Replace(".","") + ". ") + Phy.PhyFirstName + " " + Phy.PhyMiddleName + (string.IsNullOrWhiteSpace(Phy.PhyLastName) ? " " : ", " + Phy.PhyLastName),
                                             sPhyNPI = Phy.PhyNPI,
                                             sPhySpecialty = Phy.PhySpecialtyCode,
                                             sPhyFacility = Phy.PhyFacility,
                                             ulPhySplID = Phy.PhySpecialtyID,
                                             sPhyAddress = Phy.PhyAddrs,
                                             sPhyCity = Phy.PhyCity,
                                             sPhyState = Phy.PhyState,
                                             sPhyZip = Phy.PhyZip,
                                             sPhyFax = Phy.PhyFax,
                                             sPhyPhone = Phy.PhyPhone,
                                             sCategory = Phy.Category

                                         }
                                     });
                    if (lstResult.Count() == 0)
                    {
                        var lstFinalResult = new
                        {
                            Result = "No matches found."
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                    else
                    {
                        HttpContext.Current.Session.Add("PreviousProviderKeywordCriteria", text_searched);
                        HttpContext.Current.Session.Add("PreviousProviderList", JsonConvert.SerializeObject(lstResult));
                        var lstFinalResult = new
                        {
                            Matching_Result = lstResult
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                }
            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }


        //#region Web Methods
        //[System.Web.Script.Services.ScriptMethod()]
        //[System.Web.Services.WebMethod(EnableSession = true)]
        //public static string GetProviderDetailsByTokensPCP(string text_searched)
        //{
        //    if (ClientSession.UserName == string.Empty)
        //    {
        //        HttpContext.Current.Response.StatusCode = 999;
        //        HttpContext.Current.Response.Status = "999 Session Expired";
        //        HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
        //        return "Session Expired";
        //    }
        //    try
        //    {
        //        if (HttpContext.Current.Session["PreviousProviderKeywordCriteria"] != null
        //            && HttpContext.Current.Session["PreviousProviderList"] != null
        //            && HttpContext.Current.Session["PreviousProviderKeywordCriteria"].ToString().Trim().ToLower() == text_searched.ToLower())
        //        {
        //            var lstResult = JsonConvert.DeserializeObject(HttpContext.Current.Session["PreviousProviderList"].ToString());// HttpContext.Current.Session["PreviousProviderList"];

        //            var lstFinalResult = new
        //            {
        //                Matching_Result = lstResult
        //            };
        //            return JsonConvert.SerializeObject(lstFinalResult);
        //        }
        //        else
        //        {
        //            FindPhysican physician_dto = new FindPhysican();
        //            PhysicianManager objPhysicianManager = new PhysicianManager();
        //            physician_dto = objPhysicianManager.FindPhysician(text_searched);


        //            PhysicianFacilityDTO obj1 = new PhysicianFacilityDTO();
        //            //  obj1.PhyFirstName="Click here to add physician";
        //            //physician_dto.PhyList.Add(obj1);
        //            var lstResult = (from Phy in physician_dto.PhyList
        //                             select
        //                             new
        //                             {
        //                                 label =
        //                               Phy.PhyPrefix + " " + Phy.PhyFirstName + " " + Phy.PhyMiddleName + " "+ Phy.PhyLastName + "(" + Phy.PhySuffix + ")" + " | " +
        //                                                      "NPI:" + Phy.PhyNPI + " | " +
        //                                                      Phy.PhySpecialtyCode + " | " +
        //                                                      "FACILITY:" + Phy.PhyFacility + " | " +
        //                                                      "ADDR: " + Phy.PhyAddrs + ", " +
        //                                                      Phy.PhyCity + "," +
        //                                                      Phy.PhyState + " " +
        //                                                      Phy.PhyZip + " | " +
        //                                                      ((Phy.PhyPhone.Trim()) != "" ? "PH:" + Phy.PhyPhone + " | " : "") +
        //                                                      (Phy.PhyFax.Trim() != "" ? "FAX:" + Phy.PhyFax : ""),
        //                                 value = new
        //                                 {
        //                                     ulPhyId = Phy.PhyId,
        //                                     sPhyName = Phy.PhyPrefix + " " + Phy.PhyFirstName + " " + Phy.PhyMiddleName + " "  + Phy.PhyLastName + " " + Phy.PhySuffix,
        //                                     sPhyFirstName = Phy.PhyPrefix +" "+Phy.PhyFirstName + " " + Phy.PhyMiddleName + " " + Phy.PhyLastName,
        //                                     sPhyNPI = Phy.PhyNPI,
        //                                     sPhySpecialty = Phy.PhySpecialtyCode,
        //                                     sPhyFacility = Phy.PhyFacility,
        //                                     ulPhySplID = Phy.PhySpecialtyID,
        //                                     sPhyAddress = Phy.PhyAddrs,
        //                                     sPhyFax = Phy.PhyFax,
        //                                     sPhyPhone = Phy.PhyPhone

        //                                 }
        //                             });
        //            if (lstResult.Count() == 0)
        //            {
        //                var lstFinalResult = new
        //                {
        //                    Result = "No matches found."
        //                };
        //                return JsonConvert.SerializeObject(lstFinalResult);
        //            }
        //            else
        //            {
        //                HttpContext.Current.Session.Add("PreviousProviderKeywordCriteria", text_searched);
        //                HttpContext.Current.Session.Add("PreviousProviderList", JsonConvert.SerializeObject(lstResult));
        //                var lstFinalResult = new
        //                {
        //                    Matching_Result = lstResult
        //                };
        //                return JsonConvert.SerializeObject(lstFinalResult);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var lstFinalResult = new
        //        {
        //            Error = "The following error occurred :" + exception.Message + ". Please contact support."
        //        };
        //        return JsonConvert.SerializeObject(lstFinalResult);
        //    }
        //}
        //#endregion


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetProviderDetailsFaxByTokens(string text_searched)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {
                if (HttpContext.Current.Session["PreviousProviderKeywordCriteria"] != null
                    && HttpContext.Current.Session["PreviousProviderList"] != null
                    && HttpContext.Current.Session["PreviousProviderKeywordCriteria"].ToString().Trim().ToLower() == text_searched.ToLower())
                {
                    var lstResult = JsonConvert.DeserializeObject(HttpContext.Current.Session["PreviousProviderList"].ToString());
                    //HttpContext.Current.Session["PreviousProviderList"];

                    var lstFinalResult = new
                    {
                        Matching_Result = lstResult
                    };
                    return JsonConvert.SerializeObject(lstFinalResult);
                }
                else
                {
                    FindPhysican physician_dto = new FindPhysican();
                    PhysicianManager objPhysicianManager = new PhysicianManager();
                    physician_dto = objPhysicianManager.FindPhysicianFax(text_searched);

                  
                    PhysicianFacilityDTO obj1 = new PhysicianFacilityDTO();
                    //  obj1.PhyFirstName="Click here to add physician";
                    //physician_dto.PhyList.Add(obj1);
                    var lstResult = (from Phy in physician_dto.PhyList
                                     select
                                     new
                                     {
                                         label =
                                       Phy.PhyPrefix + " " + Phy.PhyFirstName + " " + Phy.PhyMiddleName + " " + Phy.PhyLastName + " " + Phy.PhySuffix + " " + " | " +
                                                              "NPI:" + Phy.PhyNPI + " | " +
                                                              Phy.PhySpecialtyCode + " | " +
                                                              "Facility:" + Phy.PhyFacility + " | " +
                                                              "Address: " + Phy.PhyAddrs + ", " +
                                                              Phy.PhyCity + "," +
                                                              Phy.PhyState + " " +
                                                              Phy.PhyZip + " | " +
                                                              ((Phy.PhyPhone.Trim()) != "" ? "Phone No:" + Phy.PhyPhone + " | " : "") +
                                                               ((Phy.PhyEmail.Trim()) != "" ? "Email:" + Phy.PhyEmail + " | " : "") +
                                                                 ((Phy.PhyCompany.Trim()) != "" ? "Company:" + Phy.PhyCompany + " | " : "") +
                                                              (Phy.PhyFax.Trim() != "" ? "Fax No:" + Phy.PhyFax : ""),
                                         value = new
                                         {
                                             ulPhyId = Phy.PhyId,
                                             //CAP-2008
                                             sPhyName = (string.IsNullOrWhiteSpace(Phy.PhyPrefix) ? "" : Phy.PhyPrefix.Replace(".", "") + ". ") + Phy.PhyFirstName + " " + Phy.PhyMiddleName + " " + Phy.PhyLastName + " " + Phy.PhySuffix,
                                             sPhyNPI = Phy.PhyNPI,
                                             sPhyFacility = Phy.PhyFacility,
                                             ulPhySplID = Phy.PhySpecialtyID,
                                             sPhyAddress = Phy.PhyAddrs,
                                             sPhyFax = Phy.PhyFax,
                                             sPhyPhone = Phy.PhyPhone,
                                             sphyEmail = Phy.PhyEmail,
                                             sPhyCompany = Phy.PhyCompany,
                                             sCategory = Phy.Category
                                         }
                                     });
                    if (lstResult.Count() == 0)
                    {
                        var lstFinalResult = new
                        {
                            Result = "No matches found."
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                    else
                    {
                        HttpContext.Current.Session.Add("PreviousProviderKeywordCriteria", text_searched);
                        HttpContext.Current.Session.Add("PreviousProviderList", JsonConvert.SerializeObject(lstResult));
                        var lstFinalResult = new
                        {
                            Matching_Result = lstResult
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                }
            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetProviderDetailsByPhyId(string vPhyId)
        {
           if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            try
            {
                
                    FindPhysican physician_dto = new FindPhysican();
                    PhysicianManager objPhysicianManager = new PhysicianManager();
                    physician_dto = objPhysicianManager.FindPhysicianByID(Convert.ToUInt64(vPhyId));

                    PhysicianFacilityDTO obj1 = new PhysicianFacilityDTO();
                    //  obj1.PhyFirstName="Click here to add physician";
                    //physician_dto.PhyList.Add(obj1);
                    var lstResult = (from Phy in physician_dto.PhyList
                                     select
                                     new
                                     {
                                         label =
                                       Phy.PhyPrefix + " " + Phy.PhyFirstName + " " + Phy.PhyMiddleName + " " + Phy.PhyLastName + " " + Phy.PhySuffix + " " + " | " +
                                                              "NPI:" + Phy.PhyNPI + " | " +
                                                              Phy.PhySpecialtyCode + " | " +
                                                              "Facility:" + Phy.PhyFacility + " | " +
                                                              "Address: " + Phy.PhyAddrs + ", " +
                                                              Phy.PhyCity + "," +
                                                              Phy.PhyState + " " +
                                                              Phy.PhyZip + " | " +
                                                              ((Phy.PhyPhone.Trim()) != "" ? "Phone No:" + Phy.PhyPhone + " | " : "") +
                                                               ((Phy.PhyEmail.Trim()) != "" ? "Email:" + Phy.PhyEmail + " | " : "") +
                                                                 ((Phy.PhyCompany.Trim()) != "" ? "Company:" + Phy.PhyCompany + " | " : "") +
                                                              (Phy.PhyFax.Trim() != "" ? "Fax No:" + Phy.PhyFax : ""),
                                         value = new
                                         {
                                             ulPhyId = Phy.PhyId,
                                             //CAP-2008
                                             sPhyName = (string.IsNullOrWhiteSpace(Phy.PhyPrefix) ? "" : Phy.PhyPrefix.Replace(".", "") + ". ") + Phy.PhyFirstName + " " + Phy.PhyMiddleName + " " + Phy.PhyLastName + " " + Phy.PhySuffix,
                                             sPhyNPI = Phy.PhyNPI,
                                             sPhyFacility = Phy.PhyFacility,
                                             ulPhySplID = Phy.PhySpecialtyID,
                                             sPhyAddress = Phy.PhyAddrs,
                                             sPhyFax = Phy.PhyFax,
                                             sPhyPhone = Phy.PhyPhone,
                                             sphyEmail = Phy.PhyEmail,
                                             sPhyCompany = Phy.PhyCompany,
                                             sCategory = Phy.Category,
                                             sPhySpecialty = Phy.PhySpecialtyCode,
                                             sPhyCity = Phy.PhyCity,
                                             sPhyState = Phy.PhyState,
                                             sPhyZip = Phy.PhyZip
                                         }
                                     });
                    if (lstResult.Count() == 0)
                    {
                        var lstFinalResult = new
                        {
                            Result = "No matches found."
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                    else
                    {
                        var lstFinalResult = new
                        {
                            Matching_Result = lstResult
                        };
                        return JsonConvert.SerializeObject(lstFinalResult);
                    }
                
            }
            catch (Exception exception)
            {
                var lstFinalResult = new
                {
                    Error = "The following error occurred :" + exception.Message + ". Please contact support."
                };
                return JsonConvert.SerializeObject(lstFinalResult);
            }
        }

        
        #endregion
    }

}
