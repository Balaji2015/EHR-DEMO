using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for AddressHistoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AddressHistoryService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string GetAllStats()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<State> iStatelist = new List<State>() ;
            StateManager stateManager = new StateManager();
            iStatelist = stateManager.Getstate();
            var vstates = iStatelist.Select(aa => aa.State_Code).Distinct().ToList();
            var states = new { states = vstates };
            return JsonConvert.SerializeObject(states);
        }

        [WebMethod(EnableSession = true)]
        public string GetAddress_HistoryByHumanID(string sHumanID)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            IList<Human_Address> ilstHuman_Addresses = new List<Human_Address>();
            Human_AddressManager Human_AddressMngr = new Human_AddressManager();

            ilstHuman_Addresses = Human_AddressMngr.GetHumanAddressByHuman(sHumanID);
            ilstHuman_Addresses = ilstHuman_Addresses.OrderByDescending(a => a.Id).ToList();

            var Human_Address = new { Address_History = ilstHuman_Addresses };
            return JsonConvert.SerializeObject(Human_Address);
        }

        [WebMethod(EnableSession = true)]
        public string SaveHumanHistoryAddress(string sHumanID, string sStreet_Address1, string sStreet_Address2,
            string sCity, string sState, string sZipCode, string sStart_Date, string sEnd_Date,
            string sHuman_Address_ID, string sInsertOrUpdate)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sStatus = "";
            IList<Human_Address> ilstHuman_Addresses = new List<Human_Address>();
            Human_AddressManager Human_AddressMngr = new Human_AddressManager();

            if (sInsertOrUpdate.ToUpper() == "ADD")
            {
                ilstHuman_Addresses.Add(new Human_Address
                {
                    Human_ID = Convert.ToUInt64(sHumanID),
                    Street_Address1 = sStreet_Address1,
                    Street_Address2 = sStreet_Address2,
                    City = sCity,
                    State = sState,
                    ZipCode = sZipCode,
                    Start_Date = sStart_Date,
                    End_Date = sEnd_Date,
                    Created_By = ClientSession.UserName,
                    Created_Date_And_Time = DateTime.UtcNow
                });

                if (ilstHuman_Addresses.Count > 0)
                {
                    Human_AddressMngr.SaveHuman_AddressWithTransaction(ilstHuman_Addresses, null, string.Empty);
                }
            }
            else if (sInsertOrUpdate.ToUpper() == "UPDATE")
            {
                ilstHuman_Addresses = Human_AddressMngr.GetHumanAddressByHumanAddressID(sHuman_Address_ID);

                if (ilstHuman_Addresses.Count > 0)
                {

                    ilstHuman_Addresses[0].Street_Address1 = sStreet_Address1;
                    ilstHuman_Addresses[0].Street_Address2 = sStreet_Address2;
                    ilstHuman_Addresses[0].City = sCity;
                    ilstHuman_Addresses[0].State = sState;
                    ilstHuman_Addresses[0].ZipCode = sZipCode;
                    ilstHuman_Addresses[0].Start_Date = sStart_Date;
                    ilstHuman_Addresses[0].End_Date = sEnd_Date;
                    ilstHuman_Addresses[0].Modified_By = ClientSession.UserName;
                    ilstHuman_Addresses[0].Modified_Date_And_Time = DateTime.UtcNow;

                    Human_AddressMngr.SaveHuman_AddressWithTransaction(null, ilstHuman_Addresses, string.Empty);
                }
            }

            var Status = new { status = sStatus };
            return JsonConvert.SerializeObject(Status);
        }

        [WebMethod(EnableSession = true)]
        public string DeleteHumanHistoryAddress(string sHuman_Address_ID) {
            string sStatus = string.Empty;
            Human_AddressManager human_AddressManager = new Human_AddressManager();
            human_AddressManager.DeleteHumanAddress(sHuman_Address_ID);
            var Status = new { status = sStatus };
            return JsonConvert.SerializeObject(Status);
        }
    }
}
