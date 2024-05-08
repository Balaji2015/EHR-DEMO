using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for PhysicianLibraryServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PhysicianLibraryServices : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string LoadPhysicianLibrary(string Category)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<string> ilstCategory = new List<string>();
            if (Category != null && Category.Contains(','))
            {
                ilstCategory = Category.Split(',').ToList();
            }
            PhysicianManager phymngrcategory = new PhysicianManager();
            XmlDocument xmldoc = new XmlDocument();
            IList<FillPhysicianLibrary> PhysicianResult = new List<FillPhysicianLibrary>();
            PhysicianResult = phymngrcategory.GetPhysicianByCategory(ilstCategory);
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //    xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
            //    XmlNodeList xmlPhysicianAddressList = xmldoc.GetElementsByTagName("PhysicianAddress");

            //    var PhysicianAddressNodes = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
            //    //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
            //    var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = (p.Attributes["Category"] != null ? p.Attributes["Category"].Value : string.Empty), Name = ((p.Attributes["Physician_prefix"] != null ? p.Attributes["Physician_prefix"].Value : string.Empty) + " " + (p.Attributes["Physician_First_Name"] != null ? p.Attributes["Physician_First_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Middle_Name"] != null ? p.Attributes["Physician_Middle_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Last_Name"] != null ? p.Attributes["Physician_Last_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Suffix"] != null ? p.Attributes["Physician_Suffix"].Value : string.Empty)), Specialty = (p.Attributes["Specialties"] != null ? p.Attributes["Specialties"].Value : string.Empty), NPI = (p.Attributes["Physician_NPI"] != null ? p.Attributes["Physician_NPI"].Value : string.Empty), Facility = (p.Attributes["Facility_Name"] != null ? p.Attributes["Facility_Name"].Value : string.Empty), Physician_Library_ID = (p.Attributes["Physician_Library_ID"] != null ? p.Attributes["Physician_Library_ID"].Value : string.Empty), Physician_Type = (p.Attributes["Physician_Type"] != null ? p.Attributes["Physician_Type"].Value : string.Empty), Company = (p.Attributes["Company"] != null ? p.Attributes["Company"].Value : string.Empty), Physician_Address1 = (p.Attributes["Physician_Address1"] != null ? p.Attributes["Physician_Address1"].Value : string.Empty), Physician_Address2 = (p.Attributes["Physician_Address2"] != null ? p.Attributes["Physician_Address2"].Value : string.Empty), Physician_City = (p.Attributes["Physician_City"] != null ? p.Attributes["Physician_City"].Value : string.Empty), Physician_State = (p.Attributes["Physician_State"] != null ? p.Attributes["Physician_State"].Value : string.Empty), Physician_Zip = (p.Attributes["Physician_Zip"] != null ? p.Attributes["Physician_Zip"].Value : string.Empty), Physician_Telephone = (p.Attributes["Physician_Telephone"] != null ? p.Attributes["Physician_Telephone"].Value : string.Empty), Physician_Fax = (p.Attributes["Physician_Fax"] != null ? p.Attributes["Physician_Fax"].Value : string.Empty), Physician_EMail = (p.Attributes["Physician_EMail"] != null ? p.Attributes["Physician_EMail"].Value : string.Empty) };

            //    var resultFinal = from p in result where ilstCategory.Contains(p.Category) select p;
            if (PhysicianResult.Count > 0)
            {
                var resultFinal = from p in PhysicianResult select new { Category = (p.Category != null ? p.Category : string.Empty), Name = ((p.Physician_prefix != null ? p.Physician_prefix : string.Empty) + " " + (p.Physician_First_Name != null ? p.Physician_First_Name : string.Empty) + " " + (p.Physician_Middle_Name != null ? p.Physician_Middle_Name : string.Empty) + " " + (p.Physician_Last_Name != null ? p.Physician_Last_Name : string.Empty) + " " + (p.Physician_Suffix != null ? p.Physician_Suffix : string.Empty)), Specialty = (p.Specialties != null ? p.Specialties : string.Empty), NPI = (p.Physician_NPI != null ? p.Physician_NPI : string.Empty), Facility = (p.Facility_Name != null ? p.Facility_Name : string.Empty), Physician_Library_ID = (p.Physician_Library_ID != null ? p.Physician_Library_ID : string.Empty), Physician_Type = (p.Physician_Type != null ? p.Physician_Type : string.Empty), Company = (p.Company != null ? p.Company : string.Empty), Physician_Address1 = (p.Physician_Address1 != null ? p.Physician_Address1 : string.Empty), Physician_Address2 = (p.Physician_Address2 != null ? p.Physician_Address2 : string.Empty), Physician_City = (p.Physician_City != null ? p.Physician_City : string.Empty), Physician_State = (p.Physician_State != null ? p.Physician_State : string.Empty), Physician_Zip = (p.Physician_Zip != null ? p.Physician_Zip : string.Empty), Physician_Telephone = (p.Physician_Telephone != null ? p.Physician_Telephone : string.Empty), Physician_Fax = (p.Physician_Fax != null ? p.Physician_Fax : string.Empty), Physician_EMail = (p.Physician_EMail != null ? p.Physician_EMail : string.Empty) };

                return JsonConvert.SerializeObject(resultFinal);
            }
            else
            {
                return JsonConvert.SerializeObject("");
            }

        }
        [WebMethod(EnableSession = true)]
        public string LoadCategoryAndSpecialites()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            HttpContext.Current.Session.Remove("PreviousProviderList");
            HttpContext.Current.Session.Remove("PreviousProviderKeywordCriteria");

            XmlDocument xmldoc = new XmlDocument();
            string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
            if (File.Exists(strXmlFilePath) == true)
            {
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "staticlookup" + ".xml");
                XmlNodeList xmlPhysicianAddressList = xmldoc.GetElementsByTagName("SpecialtyList");

                var PhysicianSpecialty = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
                var Specialty = from p in PhysicianSpecialty.ChildNodes.OfType<XmlElement>() select new { Specialty = (p.Attributes["Value"] != null ? p.Attributes["Value"].Value : string.Empty) };
                xmlPhysicianAddressList = xmldoc.GetElementsByTagName("CategoryList");
                var PhysicianCategory = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
                var Category = from p in PhysicianCategory.ChildNodes.OfType<XmlElement>() select new { Category = (p.Attributes["Value"] != null ? p.Attributes["Value"].Value : string.Empty) };

                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
                xmlPhysicianAddressList = xmldoc.GetElementsByTagName("FacilityList");
                var PhysicianFacilityList = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                var FacilityName = from p in PhysicianFacilityList.ChildNodes.OfType<XmlElement>() select new { FacilityName = (p.Attributes["Name"] != null ? p.Attributes["Name"].Value : string.Empty) };

                var result = new { SpecialtyList = Specialty, CategoryList = Category, FacilityNameList = FacilityName };
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                return JsonConvert.SerializeObject("");
            }

        }

        [WebMethod(EnableSession = true)]
        public string AddProvider(string Category, string Specialty, string Prefix, string Facility, string PhyLastName, string PhyFirstName, string Suffix, string PhyNPI, string Company, string PhyAddress1, string PhyAddress2, string PhyState, string PhyCity, string Zip, string Phone, string Fax, string Email, string ButtonType, string PhysicianId, string PhysicianType, string PhyMI, string grddisplay, string CheckDuplicatefax, string sCategory)
        {
            PhysicianManager PhyMngr = new PhysicianManager();
            ulong ProviderID = 0;
            IList<string> ilstCategory = new List<string>();
            if (sCategory != null && sCategory.Contains(','))
            {
                ilstCategory = sCategory.Split(',').ToList();
            }
            if (ButtonType == "Add")
            {
                //NPI Check
                if (Category == "NON CAPELLA USER(Physician)")
                {
                    if (PhyNPI != string.Empty)
                    {
                        //Cap - 1989
                        //XmlDocument xmldocNPI = new XmlDocument();
                        //string strXmlFilePathNPI = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                        //if (File.Exists(strXmlFilePathNPI) == true)
                        //{

                        //    string strPathphysiicanaddress = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");

                        //    using (FileStream fs = new FileStream(strPathphysiicanaddress, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        //    {
                        //        //System.Xml.Linq.XDocument xDocMapFacility = System.Xml.Linq.XDocument.Load(strPath2);
                        //        XmlDocument xDocMapFacility = new XmlDocument();
                        //        xmldocNPI.Load(strPathphysiicanaddress);

                        //        //  xmldocNPI.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        //        XmlNodeList xmlPhysicianAddressList = xmldocNPI.GetElementsByTagName("PhysicianAddress");

                        //        var PhysicianAddressNodes = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                        //        //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
                        //        var result = (from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() where p.Attributes["Physician_NPI"].Value == PhyNPI select new { ExistNPI = p.Attributes["Physician_NPI"].Value }).ToList();
                         IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                        ilstPhysicianLibrary = PhyMngr.GetPhysicianByNPI(PhyNPI);
                        var result = (from p in ilstPhysicianLibrary select new { ExistNPI = p.PhyNPI }).ToList();
                        if (result.Count > 0)
                        {
                            if (result.Count > 0)
                            {
                                return JsonConvert.SerializeObject(result);
                            }
                            //        fs.Close();
                            //        fs.Dispose();
                            //    }
                        }
                    }

                }

                else
                {
                    if (Fax != string.Empty && CheckDuplicatefax != "Y")
                    {
                        //Cap - 1989
                        //XmlDocument xmldocNPI = new XmlDocument();
                        //string strXmlFilePathFax = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                        //if (File.Exists(strXmlFilePathFax) == true)
                        //{

                        //string strPathphysiicanaddress = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");

                        //using (FileStream fs = new FileStream(strPathphysiicanaddress, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        //{
                        //    //System.Xml.Linq.XDocument xDocMapFacility = System.Xml.Linq.XDocument.Load(strPath2);
                        //    XmlDocument xDocMapFacility = new XmlDocument();
                        //    xmldocNPI.Load(strPathphysiicanaddress);

                        //    //  xmldocNPI.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        //    XmlNodeList xmlPhysicianAddressList = xmldocNPI.GetElementsByTagName("PhysicianAddress");

                        //    var PhysicianAddressNodes = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                        //    //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
                        //    var result = (from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() where p.Attributes["Physician_Fax"].Value == Fax select new { ExistFax = p.Attributes["Physician_Fax"].Value }).ToList();
                        IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                        ilstPhysicianLibrary = PhyMngr.GetPhysicianByFax(Fax);
                        if (ilstPhysicianLibrary.Count > 0)
                        {
                            var result = (from p in ilstPhysicianLibrary select new { ExistFax = p.PhyFax }).ToList();
                            return JsonConvert.SerializeObject(result);
                        }
                        //fs.Close();
                        //fs.Dispose();
                        //    }
                        //}
                    }
                }

                //End



                #region DB Insert

                PhysicianLibrary objPhyLib = new PhysicianLibrary();
                IList<PhysicianLibrary> SavePhyList = new List<PhysicianLibrary>();
                IList<PhysicianLibrary> SavedPhyList = new List<PhysicianLibrary>();
                objPhyLib.PhyLastName = PhyLastName;
                objPhyLib.PhyFirstName = PhyFirstName;
                objPhyLib.PhyAddress1 = PhyAddress1;
                objPhyLib.PhyNPI = PhyNPI;
                objPhyLib.PhyState = PhyState;
                objPhyLib.PhyCity = PhyCity;
                objPhyLib.PhyAddress2 = PhyAddress2;
                objPhyLib.Company = Company;
                objPhyLib.PhyType = PhysicianType;
                objPhyLib.PhyMiddleName = PhyMI;
                objPhyLib.PhySuffix = Suffix;
                //if (msktxtFaxNumber.TextWithLiterals != "() -")
                //{
                objPhyLib.PhyFax = Fax;
                //}

                //if (msktxtPhoneNum.TextWithLiterals != "() -")
                //{
                objPhyLib.PhyTelephone = Phone;
                //}
                //if (msktxtZip.TextWithLiterals != "() -")
                //{
                objPhyLib.PhyZip = Zip;
                //}
                objPhyLib.PhyPrefix = Prefix;
                objPhyLib.PhyEMail = Email;
                objPhyLib.CreatedDateAndTime = UtilityManager.ConvertToUniversal();
                objPhyLib.Created_By = ClientSession.UserName;
                objPhyLib.Sort_Order = 9999;
                objPhyLib.Category = Category;

                SavePhyList.Add(objPhyLib);

                SavedPhyList = PhyMngr.SaveUpdateDeletePhysicianLibrary(SavePhyList, null, null, string.Empty);

                IList<PhysicianSpecialty> PhySpecSaveList = new List<PhysicianSpecialty>();
                PhysicianSpecialtyManager PhySpcMng = new PhysicianSpecialtyManager();
                IList<MapFacilityPhysician> MapFacPhysList = new List<MapFacilityPhysician>();

                MapFacilityPhysicianManager MapFacPhyMangr = new MapFacilityPhysicianManager();
                string sXmlSpecialty = string.Empty;
                if (SavedPhyList.Count > 0)
                {
                    IList<string> ilstSpecialty = new List<string>();
                    ilstSpecialty = Specialty.Split(',').Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList<string>();
                    for (int i = 0; i < ilstSpecialty.Count; i++)
                    {
                        PhysicianSpecialty objPhysSpec = new PhysicianSpecialty();
                        if (sXmlSpecialty == string.Empty)
                            sXmlSpecialty = ilstSpecialty[i].ToString();
                        else
                            sXmlSpecialty += "," + ilstSpecialty[i].ToString();
                        objPhysSpec.Physician_ID = SavedPhyList[0].Id;
                        objPhysSpec.Specialty = ilstSpecialty[i].ToString();
                        objPhysSpec.Created_By = ClientSession.UserName;
                        objPhysSpec.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                        objPhysSpec.Sort_Order = 9999;
                        PhySpecSaveList.Add(objPhysSpec);
                    }

                    PhySpcMng.SaveUpdateDeleteWithTransaction(ref PhySpecSaveList, null, null, string.Empty);
                }
                string sXmlFacility = string.Empty;
                if (SavedPhyList.Count > 0 && Facility != string.Empty)
                {
                    IList<string> ilstFacility = new List<string>();
                    ilstFacility = Facility.Split('|').Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList<string>();
                    for (int i = 0; i < ilstFacility.Count; i++)
                    {
                        MapFacilityPhysician objMapFacPhy = new MapFacilityPhysician();
                        if (sXmlFacility == string.Empty)
                            sXmlFacility = ilstFacility[i].ToString();
                        else
                            sXmlFacility += "|" + ilstFacility[i].ToString();
                        objMapFacPhy.Facility_Name = ilstFacility[i].ToString();
                        objMapFacPhy.Phy_Rec_ID = SavedPhyList[0].Id;
                        objMapFacPhy.Status = "Y";
                        objMapFacPhy.Sort_Order = 9999;
                        objMapFacPhy.Machine_Technician_ID = "0";
                        MapFacPhysList.Add(objMapFacPhy);
                    }

                    MapFacPhyMangr.SaveUpdateDeleteWithTransaction(ref MapFacPhysList, null, null, string.Empty);
                }
                #endregion

                #region XML Insert
                PhysicianLibrary objPhy = SavedPhyList.Count > 0 ? SavedPhyList[0] : null;
                PhysicianSpecialty objPhySpec = PhySpecSaveList.Count > 0 ? PhySpecSaveList[0] : null;
                MapFacilityPhysician objPhyFac = MapFacPhysList.Count > 0 ? MapFacPhysList[0] : null;
                ProviderID = objPhy.Id;
                //Cap - 1989
                //InsertPhysicianIntoXMLs(objPhy, sXmlSpecialty, sXmlFacility);
                #endregion
            }
            else if (ButtonType == "Update")
            {
                //Cap - 1989
                //string strXmlPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");

                //NPI Check - Bug id 59492 - Included by Selvaraman - In case of Update, NPI is checked only with the providers other than the current request provider
                if (Category == "NON CAPELLA USER(Physician)")
                {
                    if (PhyNPI != string.Empty)
                    {
                        //Cap - 1989
                        //XmlDocument xmldocNPI = new XmlDocument();
                        //string strPathphysiicanaddress = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml";
                        //string strXmlFilePathNPI = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                        //if (File.Exists(strXmlFilePathNPI) == true)
                        //{

                        //    using (FileStream fs = new FileStream(strPathphysiicanaddress, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        //    {
                        //        //System.Xml.Linq.XDocument xDocMapFacility = System.Xml.Linq.XDocument.Load(strPath2);
                        //        XmlDocument xDocMapFacility = new XmlDocument();
                        //        xmldocNPI.Load(strPathphysiicanaddress);

                        //        // xmldocNPI.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        //        XmlNodeList xmlPhysicianAddressList = xmldocNPI.GetElementsByTagName("PhysicianAddress");

                        //        var PhysicianAddressNodes = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                        //        //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
                        //        var result = (from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() where p.Attributes["Physician_NPI"].Value == PhyNPI && p.Attributes["Physician_Library_ID"].Value != PhysicianId select new { ExistNPI = p.Attributes["Physician_NPI"].Value }).ToList();
                        PhysicianManager phymngrnpi = new PhysicianManager();
                        IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                        ilstPhysicianLibrary = phymngrnpi.GetPhysicianByNPI(PhyNPI);
                        var result = (from p in ilstPhysicianLibrary where p.Id != Convert.ToUInt64(PhysicianId) select new { ExistNPI = p.PhyNPI }).ToList();
                        if (result.Count > 0)
                        {
                            return JsonConvert.SerializeObject(result);
                        }
                        //fs.Close();
                        //fs.Dispose();
                    }
                    //}
                    //}
                }
                else
                {
                    if (Fax != string.Empty && CheckDuplicatefax != "Y")
                    {
                        //Cap - 1989
                        //XmlDocument xmldocNPI = new XmlDocument();
                        //string strXmlFilePathFax = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                        //if (File.Exists(strXmlFilePathFax) == true)
                        //{

                        //    string strPathphysiicanaddress = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");

                        //    using (FileStream fs = new FileStream(strPathphysiicanaddress, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        //    {
                        //        //System.Xml.Linq.XDocument xDocMapFacility = System.Xml.Linq.XDocument.Load(strPath2);
                        //        XmlDocument xDocMapFacility = new XmlDocument();
                        //        xmldocNPI.Load(strPathphysiicanaddress);

                        //        //  xmldocNPI.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                        //        XmlNodeList xmlPhysicianAddressList = xmldocNPI.GetElementsByTagName("PhysicianAddress");

                        //        var PhysicianAddressNodes = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
                        //        //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = "Category", Name = p.Attributes["Physician_First_Name"].Value, Specialty = p.Attributes["Specialties"].Value, NPI = p.Attributes["Physician_NPI"].Value, Facility = p.Attributes["Physician_Address1"].Value };
                        //        var result = (from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() where p.Attributes["Physician_Fax"].Value == Fax select new { ExistFax = p.Attributes["Physician_Fax"].Value }).ToList();
                        PhysicianManager phymngrnpi = new PhysicianManager();
                        IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                        ilstPhysicianLibrary = phymngrnpi.GetPhysicianByNPI(PhyNPI);
                        var result = (from p in ilstPhysicianLibrary where p.PhyId != Convert.ToUInt64(PhysicianId) select new { ExistNPI = p.PhyNPI }).ToList();
                        if (result.Count > 0)
                        {
                            return JsonConvert.SerializeObject(result);
                        }
                        //fs.Close();
                        //fs.Dispose();
                        //    }
                        //}
                    }
                }
                //FileStream infile = new FileStream("strXmlPath", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //FileStream outf = new FileStream("strXmlPath", FileMode.Create);
                //int a;
                //while ((a = infile.ReadByte()) != -1)
                //{
                //    outf.WriteByte((byte)a);
                //}
                //infile.Close();
                //infile.Dispose();
                //outf.Close();
                //outf.Dispose();
                //Cap - 1989
                //if (File.Exists(strXmlPath) == true)
                //{
                //using (var sr = new StreamReader(strXmlPath))

                PhysicianLibrary objPhyLib = new PhysicianLibrary();
                IList<PhysicianLibrary> updatePhyList = new List<PhysicianLibrary>();
                IList<PhysicianLibrary> GetPhyList = new List<PhysicianLibrary>();
               
                ulong ulPhysicianId = 0;

                if (PhysicianId != string.Empty)
                {
                    ulPhysicianId = Convert.ToUInt32(PhysicianId);
                }

                GetPhyList = PhyMngr.GetphysiciannameByPhyID(ulPhysicianId);

                if (GetPhyList.Count > 0)
                {
                    objPhyLib = GetPhyList[0];
                    objPhyLib.PhyLastName = PhyLastName;
                    objPhyLib.PhyFirstName = PhyFirstName;
                    objPhyLib.PhyAddress1 = PhyAddress1;
                    objPhyLib.PhyNPI = PhyNPI;
                    objPhyLib.PhyState = PhyState;
                    objPhyLib.PhyCity = PhyCity;
                    objPhyLib.PhyAddress2 = PhyAddress2;
                    objPhyLib.Company = Company;
                    objPhyLib.PhyType = PhysicianType;
                    objPhyLib.PhyMiddleName = PhyMI;
                    objPhyLib.PhySuffix = Suffix;
                    //if (msktxtFaxNumber.TextWithLiterals != "() -")
                    //{
                    objPhyLib.PhyFax = Fax;
                    //}

                    //if (msktxtPhoneNum.TextWithLiterals != "() -")
                    //{
                    objPhyLib.PhyTelephone = Phone;
                    //}
                    //if (msktxtZip.TextWithLiterals != "() -")
                    //{
                    objPhyLib.PhyZip = Zip;
                    //}
                    objPhyLib.PhyPrefix = Prefix;
                    objPhyLib.PhyEMail = Email;
                    objPhyLib.ChangedDateAndTime = UtilityManager.ConvertToUniversal();
                    objPhyLib.Modified_By = ClientSession.UserName;
                    objPhyLib.Sort_Order = 9999;
                    objPhyLib.Category = Category;
                    updatePhyList.Add(objPhyLib);
                    PhyMngr.UpdatePhysicians(objPhyLib, string.Empty);


                    IList<PhysicianSpecialty> PhySpecUpdateList = new List<PhysicianSpecialty>();
                    IList<PhysicianSpecialty> PhySpecSaveList = new List<PhysicianSpecialty>();
                    IList<PhysicianSpecialty> PhySpeDeleteList = new List<PhysicianSpecialty>();
                    IList<PhysicianSpecialty> GetPhySpec = new List<PhysicianSpecialty>();
                    PhysicianSpecialtyManager PhySpcMng = new PhysicianSpecialtyManager();
                    IList<MapFacilityPhysician> MapFacPhysList = new List<MapFacilityPhysician>();
                    IList<MapFacilityPhysician> MapFacUpdateList = new List<MapFacilityPhysician>();
                    IList<MapFacilityPhysician> MapFacDeletesList = new List<MapFacilityPhysician>();
                    IList<MapFacilityPhysician> GetMapFacList = new List<MapFacilityPhysician>();
                    MapFacilityPhysician objMapFacPhy = new MapFacilityPhysician();
                    MapFacilityPhysicianManager MapFacPhyMangr = new MapFacilityPhysicianManager();
                    string sXmlSpecialty = string.Empty;
                    if (updatePhyList.Count > 0)
                    {

                        IList<string> ilstSpecialty = new List<string>();
                        ilstSpecialty = Specialty.Split(',').Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList<string>();
                        GetPhySpec = PhySpcMng.GetPhysicianSpecialtybyPhysicianID(ulPhysicianId, ilstSpecialty, ClientSession.LegalOrg);
                        IList<string> AddListSpecialty = ilstSpecialty.Where(aa => !GetPhySpec.Any(bb => bb.Specialty == aa.ToString())).ToList();
                        if (ilstSpecialty.Count != GetPhySpec.Count)
                        {
                            for (int i = 0; i < AddListSpecialty.Count; i++)
                            {
                                if (sXmlSpecialty == string.Empty)
                                    sXmlSpecialty = AddListSpecialty[i].ToString();
                                else
                                    sXmlSpecialty += "," + AddListSpecialty[i].ToString();
                                PhysicianSpecialty objPhysSpec = new PhysicianSpecialty();
                                objPhysSpec.Physician_ID = objPhyLib.Id;
                                objPhysSpec.Specialty = AddListSpecialty[i].ToString();
                                objPhysSpec.Created_By = ClientSession.UserName;
                                objPhysSpec.Created_Date_And_Time = UtilityManager.ConvertToUniversal();
                                objPhysSpec.Sort_Order = 9999;
                                PhySpecSaveList.Add(objPhysSpec);
                            }
                        }
                        IList<string> ilstupdateSpecialty = new List<string>();
                        ilstupdateSpecialty = ilstSpecialty.Except(AddListSpecialty).ToList();
                        for (int i = 0; i < ilstupdateSpecialty.Count; i++)
                        {
                            if (sXmlSpecialty == string.Empty)
                                sXmlSpecialty = ilstupdateSpecialty[i].ToString();
                            else
                                sXmlSpecialty += "," + ilstupdateSpecialty[i].ToString();
                            PhysicianSpecialty objPhysSpec = new PhysicianSpecialty();
                            objPhysSpec = GetPhySpec.Where(aa => aa.Specialty.ToUpper() == ilstupdateSpecialty[i].ToUpper().ToString()).Select(aa => aa).ToList().FirstOrDefault();
                            objPhysSpec.Physician_ID = objPhyLib.Id;
                            objPhysSpec.Specialty = ilstupdateSpecialty[i].ToString();
                            objPhysSpec.Modified_By = ClientSession.UserName;
                            objPhysSpec.Modified_Date_And_Time = UtilityManager.ConvertToUniversal();
                            objPhysSpec.Sort_Order = 9999;
                            PhySpecUpdateList.Add(objPhysSpec);
                        }

                        PhySpeDeleteList = GetPhySpec.Except(PhySpecUpdateList).ToList();
                        PhySpcMng.SaveUpdateDeleteWithTransaction(ref PhySpecSaveList, PhySpecUpdateList, PhySpeDeleteList, string.Empty);
                    }
                    string sXmlFacility = string.Empty;
                    IList<string> ilstFacility = new List<string>();
                    if (updatePhyList.Count > 0 && Facility != string.Empty)
                    {

                        ilstFacility = Facility.Split('|').Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList<string>();
                        GetMapFacList = MapFacPhyMangr.GetPhyisician_ListbyFacNameList(ilstFacility, ulPhysicianId);
                        IList<string> AddListFacility = ilstFacility.Where(aa => !GetMapFacList.Any(bb => bb.Facility_Name == aa.ToString())).ToList();
                        if (ilstFacility.Count != GetMapFacList.Count)
                        {
                            for (int i = 0; i < AddListFacility.Count; i++)
                            {
                                if (sXmlFacility == string.Empty)
                                    sXmlFacility = ilstFacility[i].ToString();
                                else
                                    sXmlFacility += "|" + ilstFacility[i].ToString();
                                objMapFacPhy = new MapFacilityPhysician();
                                objMapFacPhy.Facility_Name = AddListFacility[i].ToString();
                                objMapFacPhy.Phy_Rec_ID = objPhyLib.Id;
                                objMapFacPhy.Status = "Y";
                                objMapFacPhy.Sort_Order = 9999;
                                objMapFacPhy.Machine_Technician_ID = "0";
                                MapFacPhysList.Add(objMapFacPhy);
                            }
                        }
                        IList<string> ilstupdateFacility = new List<string>();
                        ilstupdateFacility = ilstFacility.Except(AddListFacility).ToList();
                        for (int i = 0; i < ilstupdateFacility.Count; i++)
                        {
                            if (sXmlFacility == string.Empty)
                                sXmlFacility = ilstupdateFacility[i].ToString();
                            else
                                sXmlFacility += "|" + ilstupdateFacility[i].ToString();
                            objMapFacPhy = new MapFacilityPhysician();
                            objMapFacPhy = GetMapFacList.Where(aa => aa.Facility_Name.ToUpper() == ilstupdateFacility[i].ToUpper().ToString()).Select(aa => aa).ToList().FirstOrDefault();
                            objMapFacPhy.Facility_Name = ilstupdateFacility[i].ToString();
                            objMapFacPhy.Phy_Rec_ID = updatePhyList[0].Id;
                            objMapFacPhy.Status = "Y";
                            objMapFacPhy.Sort_Order = 9999;
                            objMapFacPhy.Machine_Technician_ID = "0";
                            MapFacUpdateList.Add(objMapFacPhy);
                        }

                        MapFacDeletesList = GetMapFacList.Except(MapFacUpdateList).ToList();
                        MapFacPhyMangr.SaveUpdateDeleteWithTransaction(ref MapFacPhysList, MapFacUpdateList, MapFacDeletesList, string.Empty);
                    }

                    //Cap - 1989
                    //using (FileStream fs = new FileStream(strXmlPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    //{
                    //    XmlDocument itemDoc = new XmlDocument();
                    //    XmlTextReader XmlText = new XmlTextReader(strXmlPath);
                    //    itemDoc.Load(XmlText);
                    //    XmlText.Close();
                    //    XmlNode PhysicianAddressNode = itemDoc.SelectSingleNode("/PhysicianAddress");
                    //    Boolean bisPhysicianIDAddorNot = false;
                    //    //itemDoc.Load(strXmlPath);
                    //    for (int i = 0; i < PhysicianAddressNode.ChildNodes.Count; i++)
                    //    {
                    //        if (PhysicianAddressNode.ChildNodes[i].Attributes["Physician_Library_ID"].Value == PhysicianId)
                    //        {
                    //            bisPhysicianIDAddorNot = true;
                    //            XmlNode oldNode = (XmlNode)PhysicianAddressNode.ChildNodes[i];
                    //            XmlNode Newnode = null;
                    //            Newnode = itemDoc.CreateNode(XmlNodeType.Element, "p" + PhysicianId.ToString(), "");
                    //            XmlAttribute attlabel = null;

                    //            attlabel = itemDoc.CreateAttribute("Physician_Address1");
                    //            attlabel.Value = PhyAddress1.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Address2");
                    //            attlabel.Value = PhyAddress2.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_City");
                    //            attlabel.Value = PhyCity.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_State");
                    //            attlabel.Value = PhyState.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Zip");
                    //            attlabel.Value = Zip.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Telephone");
                    //            attlabel.Value = Phone.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Fax");
                    //            attlabel.Value = Fax.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Specialties");
                    //            attlabel.Value = sXmlSpecialty.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_NPI");
                    //            attlabel.Value = PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_EMail");
                    //            attlabel.Value = Email.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_prefix");
                    //            attlabel.Value = Prefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_First_Name");
                    //            attlabel.Value = PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Last_Name");
                    //            attlabel.Value = PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Type");
                    //            attlabel.Value = PhysicianType.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Library_ID");
                    //            attlabel.Value = PhysicianId;
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Facility_Name");
                    //            attlabel.Value = sXmlFacility.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Company");
                    //            attlabel.Value = Company.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Category");
                    //            attlabel.Value = Category.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);


                    //            attlabel = itemDoc.CreateAttribute("Physician_Middle_Name");
                    //            attlabel.Value = PhyMI.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            attlabel = itemDoc.CreateAttribute("Physician_Suffix");
                    //            attlabel.Value = Suffix.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //            Newnode.Attributes.Append(attlabel);

                    //            PhysicianAddressNode.ReplaceChild(Newnode, oldNode);
                    //            itemDoc.Save(strXmlPath);
                    //        }
                    //    }
                    //    if (!bisPhysicianIDAddorNot)
                    //    {
                    //        XmlNode Newnode = null;
                    //        Newnode = itemDoc.CreateNode(XmlNodeType.Element, "p" + PhysicianId.ToString(), "");
                    //        XmlAttribute attlabel = null;

                    //        attlabel = itemDoc.CreateAttribute("Physician_Address1");
                    //        attlabel.Value = PhyAddress1.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Address2");
                    //        attlabel.Value = PhyAddress2.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_City");
                    //        attlabel.Value = PhyCity.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_State");
                    //        attlabel.Value = PhyState.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Zip");
                    //        attlabel.Value = Zip.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Telephone");
                    //        attlabel.Value = Phone.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Fax");
                    //        attlabel.Value = Fax.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Specialties");
                    //        attlabel.Value = sXmlSpecialty.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_NPI");
                    //        attlabel.Value = PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_EMail");
                    //        attlabel.Value = Email.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_prefix");
                    //        attlabel.Value = Prefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_First_Name");
                    //        attlabel.Value = PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Last_Name");
                    //        attlabel.Value = PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Type");
                    //        attlabel.Value = PhysicianType.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Library_ID");
                    //        attlabel.Value = PhysicianId;
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Facility_Name");
                    //        attlabel.Value = sXmlFacility.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Company");
                    //        attlabel.Value = Company.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Category");
                    //        attlabel.Value = Category.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);


                    //        attlabel = itemDoc.CreateAttribute("Physician_Middle_Name");
                    //        attlabel.Value = PhyMI.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        attlabel = itemDoc.CreateAttribute("Physician_Suffix");
                    //        attlabel.Value = Suffix.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    //        Newnode.Attributes.Append(attlabel);

                    //        PhysicianAddressNode.AppendChild(Newnode);
                    //        itemDoc.Save(strXmlPath);
                    //    }
                    //    fs.Close();
                    //    fs.Dispose();
                    //}






                    //Add Facility
                    string strPath2 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");

                    using (FileStream fs = new FileStream(strPath2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        //System.Xml.Linq.XDocument xDocMapFacility = System.Xml.Linq.XDocument.Load(strPath2);
                        XmlDocument xDocMapFacility = new XmlDocument();
                        xDocMapFacility.Load(strPath2);

                        IList<string> finallist = new List<string>();
                        //finallist = ilstFacility;
                        for (int k = 0; k < ilstFacility.Count; k++)
                        {
                            finallist.Add(ilstFacility[k]);
                        }

                        for (int k = 0; k < ilstFacility.Count; k++)
                        {
                            XmlNode PhysicianFacilityNode = xDocMapFacility.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + ilstFacility[k].ToString().ToUpper().Trim() + "']");

                            if (PhysicianFacilityNode != null)
                            {
                                for (int i = 0; i < PhysicianFacilityNode.ChildNodes.Count; i++)
                                {
                                    if (PhysicianFacilityNode.ChildNodes[i].Attributes["ID"].Value == PhysicianId)
                                    {
                                        finallist.Remove(ilstFacility[k].ToString().ToUpper());

                                        XmlNode oldNode = (XmlNode)PhysicianFacilityNode.ChildNodes[i];
                                        XmlNode Newnode = null;
                                        Newnode = xDocMapFacility.CreateNode(XmlNodeType.Element, "Physician", "");
                                        XmlAttribute attlabel = null;

                                        attlabel = xDocMapFacility.CreateAttribute("prefix");
                                        attlabel.Value = Prefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("firstname");
                                        attlabel.Value = PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("middlename");
                                        attlabel.Value = PhyMI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("lastname");
                                        attlabel.Value = PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("suffix");
                                        attlabel.Value = Suffix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("username");
                                        attlabel.Value = "";
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("ID");
                                        attlabel.Value = ulPhysicianId.ToString();
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("status");
                                        attlabel.Value = "Y";
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("npi");
                                        attlabel.Value = PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = xDocMapFacility.CreateAttribute("machine_technician_id");
                                        attlabel.Value = "0";
                                        Newnode.Attributes.Append(attlabel);

                                        PhysicianFacilityNode.ReplaceChild(Newnode, oldNode);
                                        xDocMapFacility.Save(strPath2);
                                    }
                                }

                            }

                        }
                        for (int j = 0; j < finallist.Count; j++)
                        {
                            XmlNode PhysicianFacilityNode = xDocMapFacility.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + finallist[j].ToString().ToUpper().Trim() + "']");

                            if (PhysicianFacilityNode != null)
                            {
                                XmlNode Newnode = null;
                                Newnode = xDocMapFacility.CreateNode(XmlNodeType.Element, "Physician", "");
                                XmlAttribute attlabel = null;
                                attlabel = xDocMapFacility.CreateAttribute("prefix");
                                attlabel.Value = Prefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("firstname");
                                attlabel.Value = PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("middlename");
                                attlabel.Value = "";
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("lastname");
                                attlabel.Value = PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("suffix");
                                attlabel.Value = Suffix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("username");
                                attlabel.Value = "";
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("ID");
                                attlabel.Value = ulPhysicianId.ToString();
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("status");
                                attlabel.Value = "Y";
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("npi");
                                attlabel.Value = PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = xDocMapFacility.CreateAttribute("machine_technician_id");
                                attlabel.Value = "0";
                                Newnode.Attributes.Append(attlabel);

                                PhysicianFacilityNode.AppendChild(Newnode);
                                xDocMapFacility.Save(strPath2);
                            }
                        }


                        //XmlDocument itemDoc = new XmlDocument();
                        //itemDoc.Load(strPath2);
                        //for (int f = 0; f < ilstFacility.Count; f++)
                        //{
                        //    int cnt = itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician").Count;
                        //    for (int k = 0; k < cnt; k++)
                        //    {
                        //        if (itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["ID"].Value == ulPhysicianId.ToString())
                        //        {
                        //            itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["firstname"].Value = PhyFirstName.ToString();
                        //            //itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["middlename"].Value = phym.ToString();

                        //            itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["lastname"].Value = PhyLastName.ToString();

                        //            itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["suffix"].Value = Suffix.ToString();

                        //            //itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["username"].Value = ulPhysicianId.ToString();

                        //            itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["ID"].Value = ulPhysicianId.ToString();

                        //            //itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["status"].Value = ulPhysicianId.ToString();

                        //            itemDoc.SelectNodes("/ROOT/PhyList/Facility[@name='" + ilstFacility[f].ToString() + "']")[0].SelectNodes("Physician")[k].Attributes["npi"].Value = PhyNPI.ToString();
                        //            itemDoc.Save(strPath2);
                        //        }
                        //    }
                        //}
                        fs.Close();
                        fs.Dispose();

                    }

                    //End
                }




                //SavePhyList.Add(objPhyLib);

                //SavedPhyList = PhyMngr.SaveUpdateDeletePhysicianLibrary(SavePhyList, null, null, string.Empty);

                //}
            }
            // XmlDocument xmldoc = new XmlDocument();
            //Cap - 1989
            //XmlDocument xmldoc = new XmlDocument();
            //string strXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            //if (File.Exists(strXmlFilePath) == true)
            //{
            //if (grddisplay != "none")
            //{

            //        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //        {
            //            xmldoc.Load(strXmlFilePath);
            //            XmlNodeList xmlPhysicianAddressList = xmldoc.GetElementsByTagName("PhysicianAddress");

            //            var PhysicianAddressNodes = xmlPhysicianAddressList.Cast<XmlNode>().AsEnumerable().ElementAtOrDefault(0);
            //            //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = (p.Attributes["Category"] != null ? p.Attributes["Category"].Value : string.Empty), Name = ((p.Attributes["Physician_prefix"] != null ? p.Attributes["Physician_prefix"].Value : string.Empty) + "@" + (p.Attributes["Physician_First_Name"] != null ? p.Attributes["Physician_First_Name"].Value : string.Empty) + "@" + (p.Attributes["Physician_Middle_Name"] != null ? p.Attributes["Physician_Middle_Name"].Value : string.Empty) + "@" + (p.Attributes["Physician_Last_Name"] != null ? p.Attributes["Physician_Last_Name"].Value : string.Empty)), Specialty = (p.Attributes["Specialties"] != null ? p.Attributes["Specialties"].Value : string.Empty), NPI = (p.Attributes["Physician_NPI"] != null ? p.Attributes["Physician_NPI"].Value : string.Empty), Facility = (p.Attributes["Physician_Address1"] != null ? p.Attributes["Physician_Address1"].Value : string.Empty), Physician_Library_ID = (p.Attributes["Physician_Library_ID"] != null ? p.Attributes["Physician_Library_ID"].Value : string.Empty) };
            //            //var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = (p.Attributes["Category"] != null ? p.Attributes["Category"].Value : string.Empty), Name = ((p.Attributes["Physician_prefix"] != null ? p.Attributes["Physician_prefix"].Value : string.Empty) + " " + (p.Attributes["Physician_First_Name"] != null ? p.Attributes["Physician_First_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Middle_Name"] != null ? p.Attributes["Physician_Middle_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Last_Name"] != null ? p.Attributes["Physician_Last_Name"].Value : string.Empty)), Specialty = (p.Attributes["Specialties"] != null ? p.Attributes["Specialties"].Value : string.Empty), NPI = (p.Attributes["Physician_NPI"] != null ? p.Attributes["Physician_NPI"].Value : string.Empty), Facility = (p.Attributes["Physician_Address1"] != null ? p.Attributes["Physician_Address1"].Value : string.Empty), Physician_Library_ID = (p.Attributes["Physician_Library_ID"] != null ? p.Attributes["Physician_Library_ID"].Value : string.Empty), Physician_Type = (p.Attributes["Physician_Type"] != null ? p.Attributes["Physician_Type"].Value : string.Empty), Company = (p.Attributes["Company"] != null ? p.Attributes["Company"].Value : string.Empty), Physician_Address1 = (p.Attributes["Physician_Address1"] != null ? p.Attributes["Physician_Address1"].Value : string.Empty), Physician_Address2 = (p.Attributes["Physician_Address2"] != null ? p.Attributes["Physician_Address2"].Value : string.Empty), Physician_City = (p.Attributes["Physician_City"] != null ? p.Attributes["Physician_City"].Value : string.Empty), Physician_State = (p.Attributes["Physician_State"] != null ? p.Attributes["Physician_State"].Value : string.Empty), Physician_Zip = (p.Attributes["Physician_Zip"] != null ? p.Attributes["Physician_Zip"].Value : string.Empty), Physician_Telephone = (p.Attributes["Physician_Telephone"] != null ? p.Attributes["Physician_Telephone"].Value : string.Empty), Physician_Fax = (p.Attributes["Physician_Fax"] != null ? p.Attributes["Physician_Fax"].Value : string.Empty), Physician_EMail = (p.Attributes["Physician_EMail"] != null ? p.Attributes["Physician_EMail"].Value : string.Empty) };
            //            var result = from p in PhysicianAddressNodes.ChildNodes.OfType<XmlElement>() select new { Category = (p.Attributes["Category"] != null ? p.Attributes["Category"].Value : string.Empty), Name = ((p.Attributes["Physician_prefix"] != null ? p.Attributes["Physician_prefix"].Value : string.Empty) + " " + (p.Attributes["Physician_First_Name"] != null ? p.Attributes["Physician_First_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Middle_Name"] != null ? p.Attributes["Physician_Middle_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Last_Name"] != null ? p.Attributes["Physician_Last_Name"].Value : string.Empty) + " " + (p.Attributes["Physician_Suffix"] != null ? p.Attributes["Physician_Suffix"].Value : string.Empty)), Specialty = (p.Attributes["Specialties"] != null ? p.Attributes["Specialties"].Value : string.Empty), NPI = (p.Attributes["Physician_NPI"] != null ? p.Attributes["Physician_NPI"].Value : string.Empty), Facility = (p.Attributes["Facility_Name"] != null ? p.Attributes["Facility_Name"].Value : string.Empty), Physician_Library_ID = (p.Attributes["Physician_Library_ID"] != null ? p.Attributes["Physician_Library_ID"].Value : string.Empty), Physician_Type = (p.Attributes["Physician_Type"] != null ? p.Attributes["Physician_Type"].Value : string.Empty), Company = (p.Attributes["Company"] != null ? p.Attributes["Company"].Value : string.Empty), Physician_Address1 = (p.Attributes["Physician_Address1"] != null ? p.Attributes["Physician_Address1"].Value : string.Empty), Physician_Address2 = (p.Attributes["Physician_Address2"] != null ? p.Attributes["Physician_Address2"].Value : string.Empty), Physician_City = (p.Attributes["Physician_City"] != null ? p.Attributes["Physician_City"].Value : string.Empty), Physician_State = (p.Attributes["Physician_State"] != null ? p.Attributes["Physician_State"].Value : string.Empty), Physician_Zip = (p.Attributes["Physician_Zip"] != null ? p.Attributes["Physician_Zip"].Value : string.Empty), Physician_Telephone = (p.Attributes["Physician_Telephone"] != null ? p.Attributes["Physician_Telephone"].Value : string.Empty), Physician_Fax = (p.Attributes["Physician_Fax"] != null ? p.Attributes["Physician_Fax"].Value : string.Empty), Physician_EMail = (p.Attributes["Physician_EMail"] != null ? p.Attributes["Physician_EMail"].Value : string.Empty) };
            //            var resultFinal = from p in result where ilstCategory.Contains(p.Category) select p;
            PhysicianManager phymngr = new PhysicianManager();
            IList<FillPhysicianLibrary> PhysicianResult = new List<FillPhysicianLibrary>();
            PhysicianResult = phymngr.GetPhysicianByCategory(ilstCategory);
            if (PhysicianResult.Count > 0)
            {
                if (grddisplay != "none")
                {
                    var resultFinal = from p in PhysicianResult select new { Category = (p.Category != null ? p.Category : string.Empty), Name = ((p.Physician_prefix != null ? p.Physician_prefix : string.Empty) + " " + (p.Physician_First_Name != null ? p.Physician_First_Name : string.Empty) + " " + (p.Physician_Middle_Name != null ? p.Physician_Middle_Name : string.Empty) + " " + (p.Physician_Last_Name != null ? p.Physician_Last_Name : string.Empty) + " " + (p.Physician_Suffix != null ? p.Physician_Suffix : string.Empty)), Specialty = (p.Specialties != null ? p.Specialties : string.Empty), NPI = (p.Physician_NPI != null ? p.Physician_NPI : string.Empty), Facility = (p.Facility_Name != null ? p.Facility_Name : string.Empty), Physician_Library_ID = (p.Physician_Library_ID != null ? p.Physician_Library_ID : string.Empty), Physician_Type = (p.Physician_Type != null ? p.Physician_Type : string.Empty), Company = (p.Company != null ? p.Company : string.Empty), Physician_Address1 = (p.Physician_Address1 != null ? p.Physician_Address1 : string.Empty), Physician_Address2 = (p.Physician_Address2 != null ? p.Physician_Address2 : string.Empty), Physician_City = (p.Physician_City != null ? p.Physician_City : string.Empty), Physician_State = (p.Physician_State != null ? p.Physician_State : string.Empty), Physician_Zip = (p.Physician_Zip != null ? p.Physician_Zip : string.Empty), Physician_Telephone = (p.Physician_Telephone != null ? p.Physician_Telephone : string.Empty), Physician_Fax = (p.Physician_Fax != null ? p.Physician_Fax : string.Empty), Physician_EMail = (p.Physician_EMail != null ? p.Physician_EMail : string.Empty) };

                    return JsonConvert.SerializeObject(resultFinal);
                    // }
                }

                else
                {
                    return JsonConvert.SerializeObject("None|" + ProviderID);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("");
            }




        }

        void InsertPhysicianIntoXMLs(PhysicianLibrary objPhy, string objPhySpec, string objPhyFac)
        {
            if (objPhy != null)
            {
                #region Save into PhysicianAddressDetails.xml
                string strPath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
                if (File.Exists(strPath1))
                {
                    try
                    {
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlText = new XmlTextReader(strPath1);
                        itemDoc.Load(XmlText);
                        XmlText.Close();
                        XmlNode PhysicianAddressNode = itemDoc.SelectSingleNode("/PhysicianAddress");

                        if (PhysicianAddressNode != null)
                        {
                            XmlNode Newnode = null;
                            Newnode = itemDoc.CreateNode(XmlNodeType.Element, "p" + objPhy.Id.ToString(), "");
                            XmlAttribute attlabel = null;

                            attlabel = itemDoc.CreateAttribute("Physician_Address1");
                            attlabel.Value = objPhy.PhyAddress1.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Address2");
                            attlabel.Value = objPhy.PhyAddress2.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_City");
                            attlabel.Value = objPhy.PhyCity.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_State");
                            attlabel.Value = objPhy.PhyState.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Zip");
                            attlabel.Value = objPhy.PhyZip.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Telephone");
                            attlabel.Value = objPhy.PhyTelephone.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Fax");
                            attlabel.Value = objPhy.PhyFax.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Specialties");
                            attlabel.Value = objPhySpec.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            //attlabel = itemDoc.CreateAttribute("NPI");//Commented for bugID: 48345
                            attlabel = itemDoc.CreateAttribute("Physician_NPI");
                            attlabel.Value = objPhy.PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_EMail");
                            attlabel.Value = objPhy.PhyEMail.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_prefix");
                            attlabel.Value = objPhy.PhyPrefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_First_Name");
                            attlabel.Value = objPhy.PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Last_Name");
                            attlabel.Value = objPhy.PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Type");
                            attlabel.Value = objPhy.PhyType.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Library_ID");
                            attlabel.Value = objPhy.Id.ToString();
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Facility_Name");
                            attlabel.Value = objPhyFac.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Company");
                            attlabel.Value = objPhy.Company.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Category");
                            attlabel.Value = objPhy.Category.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);


                            attlabel = itemDoc.CreateAttribute("Physician_Middle_Name");
                            attlabel.Value = objPhy.PhyMiddleName.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            attlabel = itemDoc.CreateAttribute("Physician_Suffix");
                            attlabel.Value = objPhy.PhySuffix.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                            Newnode.Attributes.Append(attlabel);

                            PhysicianAddressNode.AppendChild(Newnode);
                            itemDoc.Save(strPath1);
                        }
                    }
                    catch
                    {
                        //iCount = iCount + 1;
                        //if (iCount <= 8)
                        //{
                        //    InsertPhysicianIntoXMLs(objPhy, objPhySpec, objPhyFac);
                        //}
                    }
                }
                #endregion
                #region Save into PhysicianFacilityMapping.xml

                string strPath2 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianFacilityMapping.xml");
                if (File.Exists(strPath2))
                {
                    try
                    {
                        XmlDocument itemDoc = new XmlDocument();
                        XmlTextReader XmlText = new XmlTextReader(strPath2);
                        itemDoc.Load(XmlText);
                        XmlText.Close();
                        if (objPhyFac != null)
                        {
                            IList<string> ilstFacility = objPhyFac.Split('|').ToList();

                            if (ilstFacility.Count > 0)
                            {
                                for (int i = 0; i < ilstFacility.Count; i++)
                                {
                                    XmlNode PhysicianFacilityNode = itemDoc.SelectSingleNode("/ROOT/PhyList/Facility[@name='" + ilstFacility[i].ToString().ToUpper().Trim() + "']");

                                    if (PhysicianFacilityNode != null)
                                    {
                                        XmlNode Newnode = null;
                                        Newnode = itemDoc.CreateNode(XmlNodeType.Element, "Physician", "");
                                        XmlAttribute attlabel = null;

                                        attlabel = itemDoc.CreateAttribute("prefix");
                                        attlabel.Value = objPhy.PhyPrefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("firstname");
                                        attlabel.Value = objPhy.PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("middlename");
                                        attlabel.Value = objPhy.PhyMiddleName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("lastname");
                                        attlabel.Value = objPhy.PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("suffix");
                                        attlabel.Value = objPhy.PhySuffix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("username");
                                        attlabel.Value = "";
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("ID");
                                        attlabel.Value = objPhy.Id.ToString();
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("status");
                                        attlabel.Value = "Y";
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("npi");
                                        attlabel.Value = objPhy.PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                        Newnode.Attributes.Append(attlabel);

                                        attlabel = itemDoc.CreateAttribute("machine_technician_id");
                                        attlabel.Value = "0";
                                        Newnode.Attributes.Append(attlabel);

                                        PhysicianFacilityNode.AppendChild(Newnode);
                                        itemDoc.Save(strPath2);
                                    }
                                }
                            }

                        }
                        else
                        {
                            XmlNode UnMappedPhyListNode = itemDoc.SelectSingleNode("/ROOT/UnMappedPhyList");

                            if (UnMappedPhyListNode != null)
                            {
                                XmlNode Newnode = null;
                                Newnode = itemDoc.CreateNode(XmlNodeType.Element, "Physician", "");
                                XmlAttribute attlabel = null;

                                attlabel = itemDoc.CreateAttribute("prefix");
                                attlabel.Value = objPhy.PhyPrefix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("firstname");
                                attlabel.Value = objPhy.PhyFirstName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("middlename");
                                attlabel.Value = objPhy.PhyMiddleName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("lastname");
                                attlabel.Value = objPhy.PhyLastName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("suffix");
                                attlabel.Value = objPhy.PhySuffix.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("username");
                                attlabel.Value = "";
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("ID");
                                attlabel.Value = objPhy.Id.ToString();
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("status");
                                attlabel.Value = "Y";
                                Newnode.Attributes.Append(attlabel);

                                attlabel = itemDoc.CreateAttribute("npi");
                                attlabel.Value = objPhy.PhyNPI.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
                                Newnode.Attributes.Append(attlabel);

                                UnMappedPhyListNode.AppendChild(Newnode);
                                itemDoc.Save(strPath2);
                            }
                        }
                    }
                    catch
                    {
                        //iCount = iCount + 1;
                        //if (iCount <= 8)
                        //{
                        //    InsertPhysicianIntoXMLs(objPhy, objPhySpec, objPhyFac);
                        //}
                    }
                }

                #endregion
            }
        }
    }
}

