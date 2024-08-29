using Acurus.Capella.DataAccess;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Services;
using System.Windows.Forms;

namespace Acurus.Capella.UI
{
    public partial class frmRCopiaPatientMerge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
            IList<Rcopia_Settings> rcopiaSettings = new List<Rcopia_Settings>();
            Rcopia_SettingsManager objRCopiaManager = new Rcopia_SettingsManager();
            rcopiaSettings = objRCopiaManager.GetRcopia_Settings(ClientSession.LegalOrg);
            if (rcopiaSettings == null)
            {
                rcopiaSettings = objRCopiaManager.GetRcopia_Settings(ClientSession.LegalOrg);
            }
            var temp = from g in rcopiaSettings where g.Command == "get_url" select g;
            IList<Rcopia_Settings> TempList = temp.ToList<Rcopia_Settings>();

            Rcopia_Settings objRcopSettings = new Rcopia_Settings();

            if (TempList.Count > 0)
            {
                objRcopSettings = TempList[0];
            }
            string RCopiaSystemName = objRcopSettings.System_Name;
            string RCopiaPracticeName = objRcopSettings.Practice_Name;
            string RCopiaPatientSystemName = objRcopSettings.System_Name;
            string RCopiaAccount = objRcopSettings.Vendor_Password;
            hdnLocalTime.Value = DateTime.Now.ToString();
            if (rcopiaSessionMngr.sGetURL.Contains("RCExtResponse") == true)
            {
                string sRCopiaUser = string.Empty;
                sRCopiaUser = ClientSession.RCopiaUserName;

                //string sUrl = "action=login&service=rcopia&startup_screen=patient&rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&rcopia_patient_system_name=" + RCopiaPatientSystemName + "&rcopia_patient_external_id=" + ulMyHumanID.ToString() + "&close_window=n&allow_popup_screens=n&logout_url=https://cert.drfirst.com&time=";
                string sUrl = "rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&service=rcopia&action=login&startup_screen=patient&override_single_patient=y&skip_auth=n&time=";

                string sGMT = Convert.ToDateTime(hdnLocalTime.Value).ToString("MMddyyHHmmss");
                sUrl = sUrl + sGMT + "&skip_auth=n" + RCopiaAccount;
                string sOutput = GetMD5Hash(sUrl);
                //CAP-819
                if (RCopiaAccount?.Length > 0)
                    sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
            }
            else
            {
                string sRCopiaUser = string.Empty;
                sRCopiaUser = ClientSession.RCopiaUserName;

                string sUrl = "rcopia_portal_system_name=" + RCopiaSystemName + "&rcopia_practice_user_name=" + RCopiaPracticeName + "&rcopia_user_id=" + sRCopiaUser + "&service=rcopia&action=login&startup_screen=patient&override_single_patient=y&skip_auth=n&time=";

                string sGMT = DateTime.UtcNow.ToString("MMddyyHHmmss");
                sUrl = sUrl + sGMT + RCopiaAccount;

                string sOutput = GetMD5Hash(sUrl);
                //CAP-819
                if (RCopiaAccount?.Length > 0)
                    sOutput = sUrl.Replace(RCopiaAccount, "&MAC=" + sOutput.ToUpper());

                radBrowser.Attributes.Add("src", rcopiaSessionMngr.RCopiaSessionAddress + sOutput);
            }
        }

        public string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        [WebMethod(EnableSession = true)]
        public static string DownloadRcoipa(DateTime dtClientDate, ulong ulHumanIDKeep, ulong ulHumanIDMerge)
        {
            string responceMsg = string.Empty;
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return string.Empty;
            }

            string sErrorMessage = string.Empty;
            Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
            Rcopia_MedicationManager rcopia_MedicationManager = new Rcopia_MedicationManager();
            Rcopia_AllergyManager rcopia_AllergyManager = new Rcopia_AllergyManager();
            
            if (ClientSession.UserName != null && ClientSession.FacilityName != null)
            {
                sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, 0, ulHumanIDKeep, ClientSession.LegalOrg);
            }

            //Update in human_blob
            IList<ulong> ilstHuman = new List<ulong>();
            ilstHuman.Add(ulHumanIDKeep);
            ilstHuman.Add(ulHumanIDMerge);
            responceMsg = rcopia_MedicationManager.UpdateMedicationInHumanBlob(ilstHuman);
            if (responceMsg == "Success" || string.IsNullOrEmpty(responceMsg))
            {
                responceMsg = rcopia_AllergyManager.UpdateAllergyInHumanBlob(ilstHuman);
            }
            if (responceMsg == "Success" || string.IsNullOrEmpty(responceMsg))
            {
                //Remove Duplicates
                IList<Rcopia_Medication> ilstRcopiaMedicationKeep = new List<Rcopia_Medication>();
                IList<ulong> ilstMedicationIDKeep = new List<ulong>();
                ilstRcopiaMedicationKeep = rcopia_MedicationManager.GetMedicationWithExactDuplicates(ulHumanIDKeep,"ALL");
                ilstMedicationIDKeep = ilstRcopiaMedicationKeep.Select(a => a.Id).ToList();
                if (ilstMedicationIDKeep.Any())
                {
                    responceMsg = rcopia_MedicationManager.UpdateRcopiaMedication(ilstMedicationIDKeep, ulHumanIDKeep, ClientSession.FacilityName, ClientSession.LegalOrg, ClientSession.UserName);
                }

                if (responceMsg == "Success" || string.IsNullOrEmpty(responceMsg))
                {

                    IList<Rcopia_Allergy> ilstRcopiaAllergyKeep = new List<Rcopia_Allergy>();
                    IList<ulong> ilstAllergyIDKeep = new List<ulong>();
                    ilstRcopiaAllergyKeep = rcopia_AllergyManager.GetAllergyWithExactDuplicates(ulHumanIDKeep, "Active");
                    ilstAllergyIDKeep = ilstRcopiaAllergyKeep.Select(a => a.Id).ToList();
                    if (ilstAllergyIDKeep.Any())
                    {
                        responceMsg = rcopia_AllergyManager.UpdateRcopiaAllergy(ilstAllergyIDKeep, ulHumanIDKeep, ClientSession.FacilityName, ClientSession.LegalOrg, ClientSession.UserName);
                    }
                }

                //string sErrorMessage = string.Empty;
                //Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                //RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(ClientSession.LegalOrg);
                //if (ClientSession.UserName != null && ClientSession.FacilityName != null)
                //{
                //    //Commented the Patient Level RCopia Download
                //    sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, ClientSession.UserName, string.Empty, dtClientDate, ClientSession.FacilityName, 0, ulHumanIDMerge, ClientSession.LegalOrg);
                //}
            }
            return responceMsg;
        }
    }
}