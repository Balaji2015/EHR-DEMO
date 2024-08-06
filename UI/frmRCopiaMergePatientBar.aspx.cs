using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Acurus.Capella.UI
{
    public partial class frmRCopiaMergePatientBar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPatientStrip = string.Empty;
                ulong ulHuman_id = 0;
                if (Request["HumanID"] != null)
                {
                    ulHuman_id = Convert.ToUInt64(Request["HumanID"]);
                }
                sPatientStrip = UtilityManager.FillPatientStrip(ulHuman_id);
                lblPatientStrip.InnerText = sPatientStrip;
                ifrmRcopiaDuplicateScreen.Src = "frmRCopiaDuplicateMediations.aspx?HumanID=" + ulHuman_id;
                lblScreenDis.InnerText = "List of Duplicate Medications in Keep Account";
            }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
            
        }
    }
}