using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;

namespace Acurus.Capella.UI.WebServices
{
    /// <summary>
    /// Summary description for TestService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://www.acurussolutions.com", Description = "Logs the Audit Trail for EHR Modules", Name = "Enterprise Audit Logging Web Service")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TestService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string LoadTestTab()
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }
            string sCategory = string.Empty;
            IList<string> categoryList = new List<string>();
            TestLookupManager testMngr = new TestLookupManager();
             categoryList = testMngr.GetTestCategoryList(ClientSession.PhysicianUserName);
             if (categoryList != null)
             {
                 for (int i = 0; i < categoryList.Count; i++)
                 {
                     if (i == 0)
                         sCategory = categoryList[i].ToString().Replace("/","");
                     else
                         sCategory += "^" + categoryList[i].ToString().Replace("/","");
                 }
             }
            return sCategory;
        }


        [WebMethod(EnableSession = true)]
        public void InsertAuditLoggingDetails(string ProjectName, string _HumanID, string _EntityName, string _EntityID,
                                       string _AttributeName, string _OldValue, string _NewValue,
                                       string _TransactionType, string _Transaction_By, string _Transaction_Date_Time)
        {
            try
            {
                AuditLogManager objLoggingManager = new AuditLogManager();
                IList<string> HumanID = _HumanID.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> EntityName = _EntityName.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> EntityID = _EntityID.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> AttributeName = _AttributeName.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> OldValue = _OldValue.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> NewValue = _NewValue.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> TransactionType = _TransactionType.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> Transaction_By = _Transaction_By.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                IList<string> Transaction_Date_Time = _Transaction_Date_Time.Split(new string[] { "|~|" }, StringSplitOptions.None).ToList<string>();
                int no_of_Entries = HumanID.Count;
                IList<AuditLog> ilstLogging = new List<AuditLog>();
                for (int i = 0; i < no_of_Entries; i++)
                {

                    AuditLog objAuditLogging = new AuditLog();

                    objAuditLogging.Human_ID = Convert.ToInt32(HumanID[i]);
                    objAuditLogging.Entity_Id = Convert.ToUInt32(EntityID[i]);
                    objAuditLogging.Entity_Name = EntityName[i];
                    objAuditLogging.Attribute = AttributeName[i];
                    objAuditLogging.Old_Value = OldValue[i];
                    objAuditLogging.New_Value = NewValue[i];
                    objAuditLogging.Transaction_Type = TransactionType[i];
                    objAuditLogging.Transaction_By = Transaction_By[i];
                    objAuditLogging.Transaction_Date_And_Time = Convert.ToDateTime(Transaction_Date_Time[i]);
                    ilstLogging.Add(objAuditLogging);

                }
                if (ilstLogging != null && ilstLogging.Count > 0)
                    objLoggingManager.AppendToAuditLog(ilstLogging,String.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
