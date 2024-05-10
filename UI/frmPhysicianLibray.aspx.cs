using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            }
        }
    }
}