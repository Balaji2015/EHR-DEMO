using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using System;
using System.Web.Http;

namespace Acurus.Capella.UI.WebServices.API
{
    //CAP-3524
    public class DrFirstServiceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult DownloadDrFirstData(ulong HumanID, string FacilityName, string UserName, string LegalOrg)
        {
            try
            {
                string sErrorMessage = string.Empty;
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(LegalOrg);

                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(LegalOrg) && HumanID > 0)
                {
                    string downloadAddress = "";
                    sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(downloadAddress, UserName, string.Empty, DateTime.UtcNow, FacilityName, 0, HumanID, LegalOrg);
                }
            }
            catch (Exception ex)
            {
                return Json(new { HumanID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            return Json(new { HumanID, status = "Acknowledged" });
        }
    }
}