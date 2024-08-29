using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Acurus.Capella.UI
{
    public partial class frmRCopiaDuplicateMediations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["HumanID"] != null)
                {
                    ulong ulHuman_id = Convert.ToUInt64(Request["HumanID"]);
                    string sShowAll = string.Empty;
                    if (chkShowAll.Checked)
                    {
                        sShowAll = "ALL";
                    }
                    else
                    {
                        sShowAll = "ACTIVE";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LoadPartialDuplicatesMedicationGrid", "{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } LoadPartialDuplicatesMedicationGrid(" + ulHuman_id + ",\""+ sShowAll + "\");", true);
                }
            }

        }

        [WebMethod(EnableSession = true)]
        public static string LoadMedicationGrid(ulong ulHuman_id, string sShowAll)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            Rcopia_MedicationManager RCopiaMedicationMngr = new Rcopia_MedicationManager();
            IList<Rcopia_Medication> ilstPartialDuplicateRcopiaMedication = RCopiaMedicationMngr.GetMedicationWithPartialDuplicates(ulHuman_id, sShowAll);

            var result = new { PartialDupicateMedications = ilstPartialDuplicateRcopiaMedication };
            return JsonConvert.SerializeObject(result);

        }

        [WebMethod(EnableSession = true)]
        public static string DeleteMedications(ulong[] MedicationIds)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            Rcopia_MedicationManager RCopiaMedicationMngr = new Rcopia_MedicationManager();
            string  sStatus = RCopiaMedicationMngr.UpdateRcopiaMedication(MedicationIds,ClientSession.HumanId,ClientSession.FacilityName, ClientSession.LegalOrg,ClientSession.UserName);
            
            var result = new { status = sStatus.ToString() };
            return JsonConvert.SerializeObject(result);

        }
    }
}