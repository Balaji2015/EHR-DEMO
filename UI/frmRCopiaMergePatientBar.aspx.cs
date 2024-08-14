using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acurus.Capella.UI.RCopia;

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
                lblScreenDis.InnerText = "List of Duplicate Medications";

                //Remove the ExactDuplicates
                Rcopia_MedicationManager mngrRcpiaMedi = new Rcopia_MedicationManager();
                IList<Rcopia_Medication> ilstExactDuplicateRcopiaMedication = mngrRcpiaMedi.GetMedicationWithExactDuplicates(ulHuman_id);

                //Delete RCopia
                if (ilstExactDuplicateRcopiaMedication.Count > 0)
                {
                    IList<ulong> ilstRcopiaID = new List<ulong>();
                    ilstRcopiaID = ilstExactDuplicateRcopiaMedication.Select(x => x.Id).ToList<ulong>();

                    string sStatus = mngrRcpiaMedi.UpdateRcopiaMedication(ilstRcopiaID, ClientSession.HumanId, ClientSession.FacilityName, ClientSession.LegalOrg, ClientSession.UserName);

                }
            }
                ScriptManager.RegisterStartupScript(this, this.Page.GetType(), string.Empty, "{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
            
        }
    }
}