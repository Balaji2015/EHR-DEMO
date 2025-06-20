using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace Acurus.Capella.UI
{
    public partial class frmPhysicianLibray : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Cap - 1989
            String sEditPhyId = string.Empty;

            if (Request["EditPhyId"] != null && Request["EditPhyId"].ToString() != "")
            {
                sEditPhyId = Request["EditPhyId"];

            }
            if (sEditPhyId != string.Empty)
            {

                PhysicianManager PhyMngr = new PhysicianManager();
                IList<PhysicianLibrary> ilstPhysicianLibrary = new List<PhysicianLibrary>();
                ilstPhysicianLibrary = PhyMngr.GetphysiciannameByPhyID(Convert.ToUInt64(sEditPhyId));
                if (ilstPhysicianLibrary.Count > 0)
                {

                    if (ilstPhysicianLibrary != null)
                    {
                        string Category = ilstPhysicianLibrary[0].Category;
                        string Prefix = ilstPhysicianLibrary[0].PhyPrefix;
                        for (int i = 0; i < ddlPrefix.Items.Count; i++)
                        {
                            //Cap - 2050
                            //if (ddlPrefix.Items[i].Text.ToUpper() == Prefix.ToUpper())
                                if (ddlPrefix.Items[i].Text.ToUpper() == Prefix.Replace(".","").ToUpper())
                            {
                                ddlPrefix.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i < ddlCategory.Items.Count; i++)
                        {
                            if (ddlCategory.Items[i].Text.ToUpper() == Category.ToUpper())
                            {
                                ddlCategory.SelectedIndex = i;
                            }
                        }
                        hdnPhysicanCategory.Value = Category.ToUpper();
                        hdnPhysicianId.Value = sEditPhyId;
                        txtLastName.Value = ilstPhysicianLibrary[0].PhyLastName;
                        txtMI.Value = ilstPhysicianLibrary[0].PhyMiddleName;
                        txtFirstName.Value = ilstPhysicianLibrary[0].PhyFirstName;
                        txtSuffix.Value = ilstPhysicianLibrary[0].PhySuffix;
                        txtNPI.Value = ilstPhysicianLibrary[0].PhyNPI;
                        txtCompany.Value = ilstPhysicianLibrary[0].Company;
                        txtAddressLine1.Value = ilstPhysicianLibrary[0].PhyAddress1;
                        txtAddressLine2.Value = ilstPhysicianLibrary[0].PhyAddress2;
                        txtCity.Value = ilstPhysicianLibrary[0].PhyCity;
                        txtState.Value = ilstPhysicianLibrary[0].PhyState;
                        //Jira CAP-2064 and CAP-2065
                        //txtZip.Value = ilstPhysicianLibrary[0].PhyZip;
                        //txtPhone.Value = ilstPhysicianLibrary[0].PhyTelephone;
                        //txtFax.Value = ilstPhysicianLibrary[0].PhyFax;
                        txtZip.Value = ilstPhysicianLibrary[0].PhyZip.Replace(" ", "");
                        txtPhone.Value = ilstPhysicianLibrary[0].PhyTelephone.Replace(" ","");
                        txtFax.Value = ilstPhysicianLibrary[0].PhyFax.Replace(" ", "");
                        txtEmail.Value = ilstPhysicianLibrary[0].PhyEMail;
                        btnSave.InnerText = "Update";
                        btnClearAll.InnerText = "Cancel";
                        //CAP-3337
                        hdnLastName.Value = ilstPhysicianLibrary[0].PhyLastName;
                        hdnMI.Value = ilstPhysicianLibrary[0].PhyMiddleName;
                        hdnFirstName.Value = ilstPhysicianLibrary[0].PhyFirstName;
                        hdnNPI.Value = ilstPhysicianLibrary[0].PhyNPI;
                    }
                    else
                    {
                        hdnPhysicanCategory.Value = "";
                    }
                }

            }

                    

            if (!IsPostBack)
            {
                if (Request["Title"] != null)
                    this.Page.Title = Request["Title"].ToString();
                SecurityServiceUtility obj = new SecurityServiceUtility();
                obj.ApplyUserPermissions(this.Page);
                //CAP-3233
                hdnShowSearchNPI.Value = System.Configuration.ConfigurationSettings.AppSettings["ProviderLibraryVersion"];
            }
        }

        //CAP-3233
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SearchNPI(string url, string specialty, string searchZip)
        {
            string resultData = string.Empty;
            List<SearchNPIResult> searchNpiResult = new List<SearchNPIResult>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string json = response.Content.ReadAsStringAsync().Result;
                        NPIResponse result = JsonConvert.DeserializeObject<NPIResponse>(json);
                        if (result != null && result.result_count > 0)
                        {
                            //CAP-3328
                            List<NPIResult> newResults = result.results;
                            if (!string.IsNullOrEmpty(specialty))
                            {
                                specialty = specialty.Trim().ToLower();
                                newResults = result.results.Where(a => a.taxonomies.Any(x => x.desc.ToLower().Contains(specialty))).ToList();
                            }
                            foreach (var item1 in newResults)
                            {
                                //CAP-3341
                                List<AddressesResult> newAddresses = item1.addresses;
                                if (!string.IsNullOrEmpty(searchZip))
                                {
                                    newAddresses = item1.addresses.Where(x => x.postal_code == searchZip).ToList();
                                }

                                string taxonomies = item1?.taxonomies?.Count > 0 ? item1?.taxonomies[0]?.desc : "";
                                foreach (var item2 in newAddresses)
                                {
                                    string postal_code = item2.postal_code;
                                    if (postal_code.Length > 5)
                                    {
                                        postal_code = postal_code.Substring(0, 5) + "-" + postal_code.Substring(5);
                                    }

                                    searchNpiResult.Add(new SearchNPIResult()
                                    {
                                        number = item1.number,
                                        telephone_number = item2.telephone_number == null || item2.telephone_number == "null" ? "" : item2.telephone_number,
                                        address_1 = item2.address_1,
                                        city = item2.city,
                                        state = item2.state,
                                        postal_code = postal_code,
                                        middle_name = item1?.basic?.middle_name ?? "",
                                        specialty = taxonomies,
                                        full_address = string.Format("{0}, {1}, {2} {3}", item2.address_1, item2.city, item2.state, item2.postal_code)
                                    });
                                }
                            }
                            searchNpiResult = searchNpiResult.GroupBy(x => new
                            {
                                x.number,
                                x.telephone_number,
                                x.address_1,
                                x.city,
                                x.state,
                                x.postal_code,
                                x.middle_name,
                                x.specialty,
                                x.full_address
                            }).Select(g => g.First()).ToList();

                            resultData = JsonConvert.SerializeObject(searchNpiResult);
                        }
                    }
                }
                catch { }
            }
            return resultData;
        }
    }
    public class NPIResponse
    {
        public int result_count { get; set; }
        public List<NPIResult> results { get; set; }
    }

    public class NPIResult
    {
        public string number { get; set; }
        public List<AddressesResult> addresses { get; set; }
        public BasicResult basic { get; set; }
        public List<TaxonomiesResult> taxonomies { get; set; }
    }

    public class AddressesResult
    {
        public string address_1 { get; set; }
        public string telephone_number { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
    }

    public class BasicResult
    {
        public string middle_name { get; set; }
    }

    public class TaxonomiesResult
    {
        public string desc { get; set; }
    }

    public class SearchNPIResult
    {
        public string number { get; set; }
        public string middle_name { get; set; }
        public string telephone_number { get; set; }
        public string address_1 { get; set; }
        public string full_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string specialty { get; set; }
    }
}