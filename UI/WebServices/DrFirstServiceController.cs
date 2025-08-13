using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Acurus.Capella.UI.WebServices.API
{
    //CAP-3524
    public class DrFirstServiceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult DownloadDrFirstData(string sHumanID, string sFacilityName, string sLegalOrg)
        {
            try
            {
                if (!VerifyToken())
                {
                    return Json(new { status = "Unauthorized", ErrorDescription = "The remote server returned an error: (403) Forbidden." });
                }
                if (string.IsNullOrEmpty(sLegalOrg))
                {
                    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "LegalOrg is not valid. Cannot download DrFirst data." });
                }
                if (string.IsNullOrEmpty(sHumanID) || sHumanID == "0")
                {
                    return Json(new { HumanID = sHumanID, status = "ValidationError", ErrorDescription = "HumanID is not valid. Cannot download DrFirst data." });
                }

                string sErrorMessage = string.Empty;
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);

                string downloadAddress = "";
                sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(downloadAddress, "AcurusAPI", string.Empty, DateTime.UtcNow, sFacilityName, 0, Convert.ToUInt64(sHumanID), sLegalOrg);
            }
            catch (Exception ex)
            {
                return Json(new { HumanID = sHumanID, status = "Error", ErrorDescription = "Error in processing the request. " + ex.Message });
            }
            return Json(new { HumanID = sHumanID, status = "Acknowledged" });
        }

        private bool VerifyToken()
        {
            var authorization = Request.Headers.GetValues("Authorization");
            string token = authorization.Any() ? authorization.FirstOrDefault() : "";
            token = token.Replace("Bearer ", "");
            var endPointToken = ConfigurationSettings.AppSettings["EndPointToken"] ?? "";
            if (token == null || string.IsNullOrEmpty(token.ToString()) || string.IsNullOrEmpty(endPointToken) || token.ToString() != endPointToken)
            {
                return false;
            }
            return true;
        }
    }
}