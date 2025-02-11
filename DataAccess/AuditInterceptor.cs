/*
using System;
using System.Collections;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Type;
using Acurus.Capella.Core;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using System.Web.UI;

//Added by Selvaraman - for AuditTrail
namespace Acurus.Capella.DataAccess
{
    public partial class AuditInterceptor : EmptyInterceptor
    {
        private string tranType_Insert = "INSERT";
        private string tranType_Update = "UPDATE";
        private string tranType_Delete = "DELETE";

        //AuditLogManager auditLogManager = new AuditLogManager();

        // Invoked on Deleting an Entity
        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //IList<AuditLog> AuditLogList = new List<AuditLog>();
            Page page = new Page();
            if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
            {
                return;
            }

            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true
                && entity.ToString() != "Acurus.Capella.Core.DomainObjects.WFObject" && entity.ToString() != "Acurus.Capella.Core.DomainObjects.Documents")
            //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
            {
                DateTime dtAuditDelete = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                if (page.Session["UTCAuditDelete"] != null)
                {
                    dtAuditDelete = (DateTime)page.Session["UTCAuditDelete"];
                    page.Session.Remove("UTCAuditDelete");
                }
                MasterTableListManager mastertableMngr = new MasterTableListManager();
                string Deleted_Attribute = mastertableMngr.GetDeletedAttribute(entity.ToString());
                AuditLog audit = new AuditLog();
                audit.Entity_Name = entity.ToString();
                audit.Entity_Id = Convert.ToInt32(id);
                int iIndexName;
                string Transaction_by = string.Empty;
                try
                {
                    iIndexName = Array.IndexOf(propertyNames, "Modified_By");
                    Transaction_by = state[iIndexName].ToString();

                    if (Transaction_by == "")
                    {
                        iIndexName = Array.IndexOf(propertyNames, "Created_By");
                        Transaction_by = state[iIndexName].ToString();
                    }
                    audit.Transaction_By = Transaction_by;
                }
                catch
                {
                    //throw new Exception("Cannot identify Modified_By for Audit Trail");
                }

                audit.Transaction_Date_And_Time = dtAuditDelete;
                audit.Transaction_Type = tranType_Delete;
                audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;
                if (Deleted_Attribute != string.Empty)
                {
                    int index = Array.IndexOf(propertyNames, Deleted_Attribute);
                    audit.Attribute = propertyNames[index];
                    audit.Old_Value = state[index].ToString();
                }
                else
                {
                    audit.Attribute = "";
                    audit.Old_Value = "";
                }
                audit.New_Value = "";
                try
                {
                    int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
                    audit.Human_ID = Convert.ToInt32(state[iIndexID]);
                }
                catch
                {
                    //throw new Exception("Cannot identify Human_ID for Audit Trail");
                }

                NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
                //AuditLogList.Add(audit);


                //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
            }
        }
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //IList<AuditLog> AuditLogList = new List<AuditLog>();
            Page page = new Page();
            if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
            {
                return false;
            }

            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true
                && entity.ToString() != "Acurus.Capella.Core.DomainObjects.WFObject" && entity.ToString() != "Acurus.Capella.Core.DomainObjects.Documents")
            //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
            {
                DateTime dtAuditDelete = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                if (page.Session["UTCAuditDelete"] != null)
                {
                    dtAuditDelete = (DateTime)page.Session["UTCAuditDelete"];
                    page.Session.Remove("UTCAuditDelete");
                }
                MasterTableListManager mastertableMngr = new MasterTableListManager();
                //string Deleted_Attribute = mastertableMngr.GetDeletedAttribute(entity.ToString());
                foreach (string propName in propertyNames)
                {
                    int Iindex = Array.IndexOf(propertyNames, propName);
                    if (state[Iindex].ToString().Trim() != string.Empty)
                    {
                        AuditLog audit = new AuditLog();
                        string Deleted_Attribute = propName;

                        audit.Entity_Name = entity.ToString();
                        audit.Entity_Id = Convert.ToInt32(id);
                        int iIndexName;
                        string Transaction_by = string.Empty;
                        try
                        {
                            iIndexName = Array.IndexOf(propertyNames, "Modified_By");
                            Transaction_by = state[iIndexName].ToString();

                            if (Transaction_by == "")
                            {
                                iIndexName = Array.IndexOf(propertyNames, "Created_By");
                                Transaction_by = state[iIndexName].ToString();
                            }
                            audit.Transaction_By = Transaction_by;
                        }
                        catch
                        {
                            //throw new Exception("Cannot identify Modified_By for Audit Trail");
                        }

                        audit.Transaction_Date_And_Time = dtAuditDelete;
                        audit.Transaction_Type = tranType_Insert;
                        audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;
                        if (Deleted_Attribute != string.Empty)
                        {
                            int index = Array.IndexOf(propertyNames, Deleted_Attribute);
                            audit.Attribute = propertyNames[index];
                            audit.Old_Value = state[index].ToString();
                        }
                        else
                        {
                            audit.Attribute = "";
                            audit.Old_Value = "";
                        }
                        audit.New_Value = "";
                        try
                        {
                            int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
                            audit.Human_ID = Convert.ToInt32(state[iIndexID]);
                        }
                        catch
                        {
                            //throw new Exception("Cannot identify Human_ID for Audit Trail");
                        }

                        NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
                    }

                }
                //AuditLogList.Add(audit);


                //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
            }
            return true;
        }
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            //IList<AuditLog> AuditLogList = new List<AuditLog>();
            Page page = new Page();
            if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
            {
                return false;
            }

            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true
                && entity.ToString() != "Acurus.Capella.Core.DomainObjects.WFObject" && entity.ToString() != "Acurus.Capella.Core.DomainObjects.Documents")
            //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
            {
                DateTime dtAuditDelete = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                if (page.Session["UTCAuditDelete"] != null)
                {
                    dtAuditDelete = (DateTime)page.Session["UTCAuditDelete"];
                    page.Session.Remove("UTCAuditDelete");
                }
                MasterTableListManager mastertableMngr = new MasterTableListManager();
                //string Deleted_Attribute = mastertableMngr.GetDeletedAttribute(entity.ToString());
                foreach (string propName in propertyNames)
                {
                    bool Is_Updated=false;
                    if (propName != string.Empty)
                    {
                        int index = Array.IndexOf(propertyNames, propName);
                        string oldValue = string.Empty;
                        string newValue = string.Empty;
                        oldValue = previousState[index].ToString();
                        newValue = currentState[index].ToString();
                        if (oldValue != newValue)
                            Is_Updated = true;
                    }
                    if (Is_Updated)
                    {
                        AuditLog audit = new AuditLog();
                        string Deleted_Attribute = propName;
                        audit.Entity_Name = entity.ToString();
                        audit.Entity_Id = Convert.ToInt32(id);
                        int iIndexName;
                        string Transaction_by = string.Empty;
                        try
                        {
                            iIndexName = Array.IndexOf(propertyNames, "Modified_By");
                            Transaction_by = currentState[iIndexName].ToString();

                            if (Transaction_by == "")
                            {
                                iIndexName = Array.IndexOf(propertyNames, "Created_By");
                                Transaction_by = currentState[iIndexName].ToString();
                            }
                            audit.Transaction_By = Transaction_by;
                        }
                        catch
                        {
                            //throw new Exception("Cannot identify Modified_By for Audit Trail");
                        }

                        audit.Transaction_Date_And_Time = dtAuditDelete;
                        audit.Transaction_Type = tranType_Update;
                        audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;
                        if (Deleted_Attribute != string.Empty)
                        {
                            int index = Array.IndexOf(propertyNames, Deleted_Attribute);
                            audit.Attribute = propertyNames[index];
                            audit.Old_Value = previousState[index].ToString();
                            audit.New_Value = currentState[index].ToString();
                        }
                        try
                        {
                            int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
                            audit.Human_ID = Convert.ToInt32(currentState[iIndexID]);
                        }
                        catch
                        {
                            //throw new Exception("Cannot identify Human_ID for Audit Trail");
                        }

                        NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
                    }
                }
                
                //AuditLogList.Add(audit);


                //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
            }
            return true;
        }

        // Invoked on Updating an Entity
        //public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        //{
        //    //IList<AuditLog> AuditLogList = new List<AuditLog>();

        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
        //    {
        //        return true;
        //    }
        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true)
        //    //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
        //    {
        //        AuditLog audit = new AuditLog();
        //        audit.Entity_Name = entity.ToString();
        //        audit.Entity_Id = Convert.ToInt32(id);
        //        try
        //        {
        //            int iIndexName = Array.IndexOf(propertyNames, "Modified_By");
        //            audit.Transaction_By = currentState[iIndexName].ToString();
        //        }
        //        catch
        //        {
        //            //throw new Exception("Cannot identify Modified_By for Audit Trail");
        //        }
        //        audit.Transaction_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        //        audit.Transaction_Type = tranType_Update;
        //        audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;

        //        try
        //        {
        //            int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
        //            audit.Human_ID = Convert.ToInt32(currentState[iIndexID]);
        //        }
        //        catch
        //        {
        //            //throw new Exception("Cannot identify Human_ID for Audit Trail");
        //        }
        //        NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
        //        //AuditLogList.Add(audit);


        //        //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        // Invoked on Inserting an Entity
        //public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        //{
        //    //IList<AuditLog> AuditLogList = new List<AuditLog>();

        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
        //    {
        //        return true;
        //    }

        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true)
        //    //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
        //    {

        //        AuditLog audit = new AuditLog();
        //        audit.Entity_Name = entity.ToString();
        //        audit.Entity_Id = Convert.ToInt32(id);
        //        try
        //        {
        //            int iIndexName = Array.IndexOf(propertyNames, "Created_By");
        //            audit.Transaction_By = state[iIndexName].ToString();
        //        }
        //        catch
        //        {
        //            //throw new Exception("Cannot identify Created_By for Audit Trail");
        //        }
        //        audit.Transaction_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        //        try
        //        {
        //            int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
        //            audit.Human_ID = Convert.ToInt32(state[iIndexID]);
        //        }
        //        catch
        //        {
        //            //throw new Exception("Cannot identify Human_ID for Audit Trail");
        //        }

        //        audit.Transaction_Type = tranType_Insert;
        //        audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;

        //        NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
        //        //AuditLogList.Add(audit);


        //        //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public override void PostFlush(System.Collections.ICollection c)
        {
            int i = 0;
        }
        public override int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            int[] j = new int[10];

            {
                //IList<AuditLog> AuditLogList = new List<AuditLog>();
                Page page = new Page();
                if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
                {
                    return j;
                }

                if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true
                    && entity.ToString() != "Acurus.Capella.Core.DomainObjects.WFObject" && entity.ToString() != "Acurus.Capella.Core.DomainObjects.Documents")
                //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
                {
                    DateTime dtAuditDelete = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                    if (page.Session["UTCAuditDelete"] != null)
                    {
                        dtAuditDelete = (DateTime)page.Session["UTCAuditDelete"];
                        page.Session.Remove("UTCAuditDelete");
                    }
                    MasterTableListManager mastertableMngr = new MasterTableListManager();
                    //string Deleted_Attribute = mastertableMngr.GetDeletedAttribute(entity.ToString());
                    foreach (string propName in propertyNames)
                    {
                        int Iindex = Array.IndexOf(propertyNames, propName);
                        int check_insert = Array.IndexOf(propertyNames, "Version");
                        if (currentState[Iindex].ToString().Trim() != string.Empty && currentState[check_insert].ToString().Trim()=="1")
                        {
                            AuditLog audit = new AuditLog();
                            string Deleted_Attribute = propName;

                            audit.Entity_Name = entity.ToString();
                            audit.Entity_Id = Convert.ToInt32(id);
                            int iIndexName;
                            string Transaction_by = string.Empty;
                            try
                            {
                                iIndexName = Array.IndexOf(propertyNames, "Modified_By");
                                Transaction_by = currentState[iIndexName].ToString();

                                if (Transaction_by == "")
                                {
                                    iIndexName = Array.IndexOf(propertyNames, "Created_By");
                                    Transaction_by = currentState[iIndexName].ToString();
                                }
                                audit.Transaction_By = Transaction_by;
                            }
                            catch
                            {
                                //throw new Exception("Cannot identify Modified_By for Audit Trail");
                            }

                            audit.Transaction_Date_And_Time = dtAuditDelete;
                            audit.Transaction_Type = tranType_Insert;
                            audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;
                            if (Deleted_Attribute != string.Empty)
                            {
                                int index = Array.IndexOf(propertyNames, Deleted_Attribute);
                                audit.Attribute = propertyNames[index];
                                audit.Old_Value = currentState[index].ToString();
                            }
                            else
                            {
                                audit.Attribute = "";
                                audit.Old_Value = "";
                            }
                            audit.New_Value = "";
                            try
                            {
                                int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
                                audit.Human_ID = Convert.ToInt32(currentState[iIndexID]);
                            }
                            catch
                            {
                                //throw new Exception("Cannot identify Human_ID for Audit Trail");
                            }

                            NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
                        }

                    }
                    //AuditLogList.Add(audit);


                    //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
                }
            }
            return j;
        }
        public override void OnCollectionUpdate(object collection, object key)
        {
            int j=0;
        }
    }
}
*/

#region OldCode
using System;
using System.Collections;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Type;
using Acurus.Capella.Core;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Reflection;

//Added by Selvaraman - for AuditTrail
namespace Acurus.Capella.DataAccess
{
    public partial class AuditInterceptor : EmptyInterceptor
    {
        //private string tranType_Insert = "INSERT";
        private string tranType_Update = "UPDATE";
        private string tranType_Delete = "DELETE";

        AuditLogManager auditLogManager = new AuditLogManager();
        IList<string> IgnorePropList = new List<string> { "VERSION", "CREATED_BY", "CREATED_DATE_AND_TIME", "MODIFIED_BY", "MODIFIED_DATE_AND_TIME", "ID" };
        // Invoked on Deleting an Entity
        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //IList<AuditLog> AuditLogList = new List<AuditLog>();
            string Is_Audit_log = "N";
            if (System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"] != null)
                Is_Audit_log = System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"];

            if (Is_Audit_log != "Y")
                return;

            if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
            {
                return;
            }

            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true)
            //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
            {
                if (propertyNames.Length > 0)
                {
                    AuditLog audit = new AuditLog();
                    audit.Entity_Name = entity.ToString();
                    audit.Attribute = "Id";
                    audit.Entity_Id = Convert.ToUInt64(id);
                    try
                    {
                        int iIndexName = Array.IndexOf(propertyNames, "Modified_By");
                        if(iIndexName!=-1)
                        audit.Transaction_By = state[iIndexName].ToString();
                        if(audit.Transaction_By.Trim()==string.Empty)
                        {
                            int iNIndexName = Array.IndexOf(propertyNames, "Created_By");
                            if(iNIndexName!=-1)
                            audit.Transaction_By = state[iNIndexName].ToString();
                        }
                    }
                    catch
                    {
                        throw new Exception("Cannot identify Modified_By for Audit Trail");
                    }
                    audit.Transaction_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                    audit.Transaction_Type = tranType_Delete;
                    audit.Old_Value = string.Empty;
                    audit.New_Value = string.Empty;

                    try
                    {
                        if ((Array.IndexOf(propertyNames, "Human_ID") != -1 || Array.IndexOf(propertyNames, "Human_Id") != -1))
                        {
                            int iIndexID;
                            iIndexID = Array.IndexOf(propertyNames, "Human_ID");
                            if (iIndexID == -1)
                                iIndexID = Array.IndexOf(propertyNames, "Human_Id");
                            audit.Human_ID = Convert.ToInt32(state[iIndexID]);
                        }

                    }
                    catch
                    {
                        //throw new Exception("Cannot identify Human_ID for Audit Trail");
                    }
                    //AuditLogList.Add(audit);

                    //CAP-2891
                    if (audit == null)
                    {
                        string methodName = MethodBase.GetCurrentMethod().Name;

                        var data = new StringBuilder();
                        for (int index = 0; index < propertyNames.Length; index++)
                        {
                            var propertyName = propertyNames.Length > index ? propertyNames[index] : "";
                            var currentStateValue = state.Length > index ? state[index] : "";

                            data.AppendLine($"property name: {propertyName}, current value: {currentStateValue}");

                        }

                        string lineNo = $"Entity: {entity} Id: {id} LogData: {data.ToString()}";
                        AuditLogErrorMessege(methodName, "Audit Log null entry", lineNo);
                    }
                    else
                    {
                        NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
                    }
                }

                //    }

                //}
                //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
            }
        }

        // Invoked on Updating an Entity
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            string Is_Audit_log = "N";
           // bool UpdatesPresent = false;
            if (System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"] != null)
                Is_Audit_log = System.Configuration.ConfigurationManager.AppSettings["Is_Audit_Log"];

            if (Is_Audit_log != "Y")
                return true;
            if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
            {
                return true;
            }

            //IList<AuditLog> AuditLogList = new List<AuditLog>();

            if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true)
            //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
            {
                if (propertyNames.Length > 0)
                {
                    for (int i = 0; i < propertyNames.Length; i++)
                    {
                        bool Is_Updated = false;
                        if (propertyNames[i] != string.Empty && !IgnorePropList.Contains(propertyNames[i].ToString().ToUpper()) && !propertyNames[i].ToString().Trim().Contains("System.Collections.Generic.List"))//BugID:50203
                        {
                            int index = Array.IndexOf(propertyNames, propertyNames[i]);
                            string oldValue = string.Empty;
                            string newValue = string.Empty;
                            oldValue = previousState[index].ToString();
                            newValue = currentState[index].ToString();
                            if (oldValue != newValue)
                                Is_Updated = true;
                        }
                        if (Is_Updated)
                        {
                            AuditLog audit = new AuditLog();
                            audit.Entity_Name = entity.ToString();
                            audit.Attribute = propertyNames[i];
                            audit.Entity_Id = Convert.ToUInt64(id);
                            try
                            {
                                int iIndexName = Array.IndexOf(propertyNames, "Modified_By");
                                if (iIndexName != -1)
                                    audit.Transaction_By = currentState[iIndexName].ToString();
                                if (audit.Transaction_By.Trim() == string.Empty)
                                {
                                    int iNIndexName = Array.IndexOf(propertyNames, "Created_By");
                                    if (iNIndexName != -1)
                                        audit.Transaction_By = currentState[iNIndexName].ToString();
                                }
                            }
                            catch
                            {
                                throw new Exception("Cannot identify Modified_By for Audit Trail");
                            }
                            audit.Transaction_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                            audit.Transaction_Type = tranType_Update;
                            if (currentState != null && currentState[i] != null)
                            {
                                audit.New_Value = currentState[i].ToString();
                            }
                            else
                            {
                                audit.New_Value = "";
                            }
                            if (previousState != null && previousState[i] != null)
                            {
                                audit.Old_Value = previousState[i].ToString();
                            }
                            else
                            {
                                audit.Old_Value = "";
                            }

                            try
                            {
                                if ((Array.IndexOf(propertyNames, "Human_ID") != -1 || Array.IndexOf(propertyNames, "Human_Id") != -1))
                                {
                                    int iIndexID;
                                    iIndexID = Array.IndexOf(propertyNames, "Human_ID");
                                    if (iIndexID == -1)
                                        iIndexID = Array.IndexOf(propertyNames, "Human_Id");
                                    audit.Human_ID = Convert.ToInt32(currentState[iIndexID]);
                                }
                                if(entity.ToString().Equals("Acurus.Capella.Core.DomainObjects.Human")){
                                    audit.Human_ID = Convert.ToInt32(id);
                                }
                            }
                            catch
                            {
                                //throw new Exception("Cannot identify Human_ID for Audit Trail");
                            }
                            //AuditLogList.Add(audit);

                            //CAP-2891
                            if (audit == null)
                            {
                                var data = new StringBuilder();
                                for(int index = 0; index < propertyNames.Length; index++)
                                {
                                    var propertyName = propertyNames.Length > index ? propertyNames[index] : "";
                                    var currentStateValue = currentState.Length > index ? currentState[index] : "";
                                    var previousStateValue = previousState.Length > index ? previousState[index] : "";

                                    data.AppendLine($"property name: {propertyName}, current value: {currentStateValue}, previous value: {previousStateValue}");

                                }

                                string methodName = MethodBase.GetCurrentMethod().Name;
                                string lineNo = $"Entity: {entity} Id: {id} LogData: {data.ToString()}";
                                AuditLogErrorMessege(methodName, "Audit Log null entry", lineNo);
                            }
                            else
                            {
                                NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
                            }

                        }

                    }
                    //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
                }
                else
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //CAP-2891
        public static void AuditLogErrorMessege(string methodName, string logMessage = "", string lineNumber = "")
        {
            var version = ConfigurationSettings.AppSettings["VersionConfiguration"].ToString();

            string[] server = version.Split('|');
            string serverno = "";
            if (server.Length > 1)
                serverno = server[1].Trim();

            string insertQuery = "insert into stats_apperrorlog values(0,'" + logMessage + ", Method Name: " + methodName + "','" + serverno + "','" + DateTime.Now + "','','0','0','0','" + lineNumber + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";

            string ConnectionData;
            ConnectionData = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(ConnectionData))
            {
                using (MySqlCommand cmd = new MySqlCommand(insertQuery))
                {
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }

        // Invoked on Inserting an Entity
        //public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        //{
        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList == null)
        //    {
        //        return true;
        //    }

        //    IList<AuditLog> AuditLogList = new List<AuditLog>();

        //    if (NHibernateSessionUtility.Instance.AuditTrailTableList.Contains(entity.ToString()) == true)
        //    //if (NHibernateSessionUtility.Instance.bAuditTrailOn == true)
        //    {
        //        if (propertyNames.Length > 0)
        //        {
        //            for (int i = 0; i < propertyNames.Length; i++)
        //            {
        //                AuditLog audit = new AuditLog();
        //                audit.Entity_Name = entity.ToString();
        //                audit.Attribute = propertyNames[i];
        //                audit.Entity_Id = Convert.ToInt32(id);
        //                try
        //                {
        //                    int iIndexName = Array.IndexOf(propertyNames, "Created_By");
        //                    audit.Transaction_By = state[iIndexName].ToString();
        //                }
        //                catch
        //                {
        //                    throw new Exception("Cannot identify Created_By for Audit Trail");
        //                }
        //                audit.Transaction_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
        //                if (state[i] != null)
        //                {
        //                    audit.New_Value = state[i].ToString();
        //                }
        //                else
        //                {
        //                    audit.New_Value = "";
        //                }
        //                try
        //                {
        //                    int iIndexID = Array.IndexOf(propertyNames, "Human_ID");
        //                    audit.Human_ID = Convert.ToInt32(state[iIndexID]);
        //                }
        //                catch
        //                {
        //                    //throw new Exception("Cannot identify Human_ID for Audit Trail");
        //                }

        //                audit.Transaction_Type = tranType_Insert;
        //                audit.MAC_Address = NHibernateSessionUtility.Instance.MACAddress;

        //                AuditLogList.Add(audit);
        //                NHibernateSessionUtility.Instance.MyAuditLogList.Add(audit);
        //            }
        //            //auditLogManager.AppendToAuditLog(AuditLogList, NHibernateSessionUtility.Instance.MACAddress);
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
#endregion
