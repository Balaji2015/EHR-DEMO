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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using Telerik.Web.UI;

namespace Acurus.Capella.UI
{
    public partial class frmSelectLabLocation : System.Web.UI.Page
    {
        object[] objSearchResult;
        LabLocationManager objLabLocMngr = new LabLocationManager();
        IList<LabLocation> loclist = null;
        ulong ulMyLabID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CAP-2834
                txtLabName.Text = HttpUtility.UrlDecode(Request["LabName"]);
                mpnLabLocation.Reset();
                btnSearch.Enabled = false;
                btnOk.Enabled = false;
                
            }
            ulMyLabID = Convert.ToUInt32(Request["LabID"]);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtAddress.Text.Trim() == string.Empty && txtCity.Text.Trim() == string.Empty && txtZip.Text == "_____-____" && txtState.Text.Trim() == string.Empty && txtNPI.Text.Trim() == string.Empty)
            {
               // ApplicationObject.erroHandler.DisplayErrorMessage("230119", "Select Lab Location",this.Page);
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, "DisplayErrorMessage('230119'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                return;
            }
            else
            {
                mpnLabLocation.Reset();               
                mpnLabLocation.TotalNoofDBRecords = objLabLocMngr.LabLocationCount(ulMyLabID, txtAddress.Text, txtCity.Text, txtState.Text, txtZip.TextWithLiterals, txtNPI.Text);
                FillLabLocations();
                ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
        }
        private void FillLabLocations()
        {
            grdLabLocations.DataSource = null;
            objSearchResult = objLabLocMngr.SearchLabLocationByLimit(ulMyLabID, txtAddress.Text, txtCity.Text, txtState.Text, txtZip.TextWithLiterals, txtNPI.Text, mpnLabLocation.PageNumber, mpnLabLocation.MaxResultPerPage);
            if (objSearchResult != null)
            {
                if (objSearchResult.Length == 2)
                {
                    loclist = (IList<LabLocation>)objSearchResult[0];
                    if (loclist != null)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Address", typeof(string));
                        dt.Columns.Add("City", typeof(string));
                        dt.Columns.Add("State", typeof(string));
                        dt.Columns.Add("Zip", typeof(string));
                        dt.Columns.Add("NPI", typeof(string));
                        dt.Columns.Add("Location ID", typeof(string));
                        //CAP-2385
                        dt.Columns.Add("Location Name", typeof(string));
                        foreach (LabLocation obj in loclist)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Address"] = obj.Street_Address1;
                            if (obj.Street_Address2 != string.Empty)
                                dr["Address"] += "," + obj.Street_Address2;
                            dr["City"] = obj.City;
                            dr["State"] = obj.State;
                            dr["Zip"] = obj.ZipCode;
                            dr["NPI"] = obj.Lab_NPI;
                            dr["Location ID"] = obj.Id;
                            //CAP-2385
                            dr["Location Name"] = obj.Location_Name;
                            dt.Rows.Add(dr);
                        }
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
                        grdLabLocations.DataSource = ds.Tables[0];
                        grdLabLocations.DataBind();
                        //CAP-2385
                        grdLabLocations.Columns[6].Visible = false;
                        if (loclist.Count > 0)
                        {
                            lblResult.Text = mpnLabLocation.TotalNoofDBRecords.ToString() + " Result(s) found";
                            
                            //btnOk.Enabled = true;
                        }
                        else
                        {
                            lblResult.Text = "No Result(s) found";
                            //btnOk.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                lblResult.Text = "No Result(s) found";
                //btnOk.Enabled = false;
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtNPI.Text = string.Empty;
            lblResult.Text = string.Empty;
            grdLabLocations.DataSource = null;
            grdLabLocations.DataBind();
            mpnLabLocation.Reset();
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            //btnSearch.Enabled = false;
            //btnOk.Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(frmSelectLabLocation), "closePage", "var Result=new Object();returnToParent(Result); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(frmSelectLabLocation), "closePage", " var Result=new Object(); Result.selectedLabText=document.getElementById('hdnSelectedLabText').value;sessionStorage.setItem('AllDiag_SelectedLabText', document.getElementById('hdnSelectedLabText').value);returnToParent(Result); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }

        protected void grdLabLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = true;
            GridDataItem selectedItem = (GridDataItem)grdLabLocations.SelectedItems[0];
            //CAP-2385
            //hdnSelectedLabText.Value = selectedItem["City"].Text;
            hdnSelectedLabText.Value = selectedItem["LocationName"].Text;
            hdnSelectedLocID.Value = selectedItem["LocationID"].Text;
            hdnSelectedLabAddress.Value = selectedItem["Address"].Text + " " + selectedItem["City"].Text + " " + selectedItem["State"].Text + " " + selectedItem["Zip"].Text;
            
        }

        public void FirstPageNavigator(object sender, EventArgs e)
        {
            FillLabLocations();
        }
    }
}
