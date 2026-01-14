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
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Drawing;
namespace Acurus.Capella.UI
{
    public partial class frmAllProcedures : System.Web.UI.Page
    {

        OrderCodeLibraryManager ordercodelibMngr = new OrderCodeLibraryManager();
        IList<PhysicianProcedure> procedureList = new List<PhysicianProcedure>();




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnMoveToProcedure.Enabled = false;
                if (Request["SourceScreen"] != null && Request["SourceScreen"].ToUpper() == "MRE")
                {
                    hdnScreenName.Value = Request["SourceScreen"].ToString();
                    if (Request["PhyId"] != null && Request["selectedLabID"] != null)
                    {
                        PhysicianProcedureManager phyProcMngr = new PhysicianProcedureManager();
                        procedureList = phyProcMngr.GetProceduresUsingPhysicianIDAndLabID(Convert.ToUInt64(Convert.ToString(Request["PhyId"])), "LAB PROCEDURE", Convert.ToUInt64(Convert.ToString(Request["selectedLabID"])),ClientSession.LegalOrg);
                        Session["PreLoadProcedure"] = procedureList;
                    }
                }
                chklstcptanddesclist.Attributes.Add("onclick", "ChkBoxSelected();");
            }
            // hdnPreloadedICD.Value = Request.QueryString["hdnPreloadedICD"].ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IList<OrderCodeLibrary> ilstorderCodeLibrary = new List<OrderCodeLibrary>();
            ulong uLab_ID = 0;
            int ResultCount = 0;
            bool IsOrderCodeLibary = false;
            string sLabName = string.Empty;
            string sDate = string.Empty;
            if (ClientSession.EncounterId != 0)
            {
                sDate = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service.ToString("yyyy-MM-dd");
                //Cap-4057
                if(sDate == "0001-01-01")
                {
                    sDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            else
                sDate = DateTime.Now.ToString("yyyy-MM-dd");
            if (Request["SelectedLab"] != null)
            {
                sLabName = Request["SelectedLab"];
                uLab_ID = Convert.ToUInt16(Request["selectedLabID"]);
            }
            if (sLabName == "LabCorp" || sLabName == "Quest Diagnostics")
                IsOrderCodeLibary = true;
            IList<ProcedureCodeLibrary> procedurecodeList = null;
            ProcedureCodeLibraryManager proccodelibMngr = new ProcedureCodeLibraryManager();
            string code = txtEnterCPTCode.Text;
            string descrp = txtEnterDescription.Text;
            if (txtEnterDescription.Text.Contains(" "))
            {
                descrp = txtEnterDescription.Text.Replace(' ', '%');
            }
            if (((code != string.Empty) || (descrp != string.Empty)) && IsOrderCodeLibary == false)
            {
                procedurecodeList = proccodelibMngr.GetCPTAndDescriptionBasedOnProcedureType("%" + code + "%", "%" + descrp + "%", sDate);
            }
            else if ((txtEnterCPTCode.Text == string.Empty) && (txtEnterDescription.Text == string.Empty))
            {
                //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), string.Empty, " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('220202');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MRE AllProcedure", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}DisplayErrorMessage('220202');", true);
                lblError1.Visible = false;
                return;
            }
            else if (IsOrderCodeLibary)
            {
                ilstorderCodeLibrary = ordercodelibMngr.SearchOrderCodeForCodeAndDesc(txtEnterCPTCode.Text.Trim(), txtEnterDescription.Text.Trim().Replace(' ', '%'), uLab_ID);
            }

            if (IsOrderCodeLibary == false)
                ResultCount = FillCPTandDescription(procedurecodeList);
            else
                ResultCount = FillCPTandDescription(ilstorderCodeLibrary);

            if (ResultCount > 0)
            {
                lblError1.Text = ResultCount + " Result(s) Found";
                //btnMoveToProcedure.Enabled = true;
            }
            else
            {
                lblError1.Text = "No Result(s) Found";
                btnMoveToProcedure.Enabled = false;
            }
            lblError1.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MRE AllProcedure", " {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
        private int FillCPTandDescription(IList<ProcedureCodeLibrary> cptanddescist)
        {
            chklstcptanddesclist.Items.Clear();

            if (cptanddescist != null)
            {
                if (cptanddescist.Count > 0)
                {
                    for (int i = 0; i < cptanddescist.Count; i++)
                    {
                        chklstcptanddesclist.Items.Add((cptanddescist[i].Procedure_Code + "-" + cptanddescist[i].Procedure_Description).ToString());
                    }
                }
            }
            return cptanddescist.Count;
        }
        private int FillCPTandDescription(IList<OrderCodeLibrary> cptanddescist)
        {
            chklstcptanddesclist.Items.Clear();

            if (cptanddescist != null)
            {
                if (cptanddescist.Count > 0)
                {
                    for (int i = 0; i < cptanddescist.Count; i++)
                    {
                        chklstcptanddesclist.Items.Add((cptanddescist[i].Order_Code + "-" + cptanddescist[i].Order_Code_Name).ToString());
                    }
                }
            }
            return cptanddescist.Count;
        }

        protected void chklstcptanddesclist_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnMessageType.Value = string.Empty;
            //if(chklstcptanddesclist.SelectedIndexChanged==true)
            List<string> CheckedItem = chklstcptanddesclist.Items.Cast<ListItem>().Where(a => a.Selected).Select(a => a.Text).ToList<string>();
            if (CheckedItem.Count > 0)
            {
                btnMoveToProcedure.Enabled = true;
            }
            else
            {
                btnMoveToProcedure.Enabled = false;
            }
        }
        //protected void chklstcptanddesclist_CheckedChanged(object sender, EventArgs e)
        //{
        //    btnMoveToProcedure.Enabled = true;
        //}


        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            txtEnterCPTCode.Text = string.Empty;
            txtEnterDescription.Text = string.Empty;
            chklstcptanddesclist.Items.Clear();
            lblError1.Text = string.Empty;
            btnMoveToProcedure.Enabled = false;
            // Added by priyangha on 26/12/2012 Bugid:13131
            txtEnterDescription.Focus();

        }

        protected void btnMoveToProcedure_Click(object sender, EventArgs e)
        {
            if (hdnScreenName.Value.ToUpper() == "MRE")
            {
                procedureList = (IList<PhysicianProcedure>)Session["PreLoadProcedure"];
                List<string> SelectedItem = chklstcptanddesclist.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => item.Text).ToList<string>();
                if (SelectedItem.Count>0)
                {
                    if (SelectedItem.Any(a => procedureList.Any(b => b.Physician_Procedure_Code == a.Substring(0, a.IndexOf('-')))))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MRE AllProcedure", "DisplayErrorMessage('220209'); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                    else 
                    {
                        btnMoveToProcedure.Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MRE AllProcedure", "GetCPTValueMoveToProcedure(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
                    }
                    
                }
            
            }
            else
            {
                btnMoveToProcedure.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MRE AllProcedure", "GetCPTValueMoveToProcedure(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
            }
            //string sResultProcedure = string.Empty;
            //IList<string> checkedList = new List<string>();
            //for (int j = 0; j < chklstcptanddesclist.Items.Count; j++)
            //{
            //    if (chklstcptanddesclist.Items[j].Selected == true)
            //    {
            //        checkedList.Add(chklstcptanddesclist.Items[j].Text);
            //    }

            //}

            //string[] sMyPreloadedICD = hdnPreloadedICD.Value.Split('|');
            //bool bCheck = false;
            //for (int i = 0; i < checkedList.Count; i++)
            //{
            //    var query = from c in sMyPreloadedICD where c.Split('-')[0].Contains(checkedList[i].Split('-')[0].ToString()) == true select c;
            //    if (query.ToList<string>().Count > 0)
            //    {


            //        if (bCheck != false)
            //        {
            //            bCheck = true;
            //        }
            //    }
            //    else
            //    {
            //        bCheck = false;
            //    }
            //}
            //if (bCheck == true)
            //{
            //    ApplicationObject.erroHandler.DisplayErrorMessage("220209", this.Page.Title.ToString(),this);
            //}
            //else
            //{
            //    ApplicationObject.erroHandler.DisplayErrorMessage("220204", this.Page.Title.ToString(),this);
            //}

            //hdnSelectedCPT.Value = string.Join("|", checkedList.ToArray());

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //if(btnMoveToProcedure.Enabled==true)
            //ScriptManager.RegisterStartupScript(this, typeof(frmAllProcedures), "ErrorMessage", "DisplayErrorMessage('220206');", true);  
        }

    }
}
