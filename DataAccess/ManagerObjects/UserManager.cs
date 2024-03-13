using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Security.Cryptography;
using System.Text;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Web;
using System.IO;
using System.DirectoryServices;

namespace Acurus.Capella.DataAccess.ManagerObjects
{


    public partial interface ILoginManager : IManagerBase<User, uint>
    {
        IList<User> CheckUser(string UserName, string Password, out bool Base64Password);
        IList<User> GetUser(string UserName);
        IList<User> GetUserList(string sLegalOrg);
        void UpdatePassword(IList<User> userList, string MACAddress);
        bool CheckIfValidPwd(string UserName, string Password);//BugID:54512
        string GetConnectionStringDetails();
        string GetLocalConnectionStringDetails();
        string GetDefaultFacilityForLogin(string sUserName);
        IList<User> GetUserAndFacilityList();
        IList<User> getUserByPHYID(ulong Phyid);
        void UpdateUserDetails(IList<User> userList, string MACAddress);
        //Added by srividhya for ACO on 3-Sep-2014
        IList<User> GetUserBasedonRole(string sRole);
        IList<User> GetUserbyPhysicianLibraryID(ulong Physician_Library_ID);
        IList<User> UserPreference();
        LoginDTO CheckUserDetailsWithoutPassword(string UserName, Boolean bIsScnTabLoad);
        void SaveLastSuccessfulyLoginDate(string sUserName, DateTime dtLoginDate);
        LoginDTO GetUserDetailsByOktaEmailAddress(string sEmailAddress, bool bIsScnTabLoad);
        IList<User> GetUserByEmailAddress(string sEmailAddress);
        LoginDTO CheckUserDetailsLegalOrg(string sUserName, Boolean bIsScnTabLoad);
        IList<User> CheckLegalOrgUser(string sUserName);
        IList<User> CheckImpersonateUserWithPassword(string UserName, string Password, out bool Base64Password);
        IList<User> getImpersonateUser(string sUserName, string sLegalOrg);
        LoginDTO CheckImpersonateUserDetails(string UserName, Boolean bIsScnTabLoad);
        IList<User> CheckImpersonateUser(string UserName);

    }
    public partial class UserManager : ManagerBase<User, uint>, ILoginManager
    {
        #region Constructors

        public UserManager()
            : base()
        {

        }
        public UserManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods


        public IList<User> getUserByPHYID(ulong Phyid)
        {
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Phyid));
                UserList = criteria.List<User>();
                iMySession.Close();
            }
            return UserList;
        }
        public IList<User> getImpersonateUser(string sUserName, string sLegalOrg)
        {
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name <> :UserName  and u.Status = 'A' and u.Legal_Org = :LegalOrg").AddEntity("u", typeof(User));
                sql.SetParameter("UserName", sUserName);
                sql.SetParameter("LegalOrg", sLegalOrg);
                UserList = sql.List<User>();
                iMySession.Close();
            }
            return UserList;
        }

        public IList<User> CheckUser(string UserName, string Password, out bool Base64Password)
        {

            Base64Password = true;
            //To refresh the NHibernateSessionUtility
            NHibernateSessionUtility.Instance.FillInstance();
            /* Updated by Ponmozhi Vendan T for the Bug ID = 28045 */
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    //Jira CAP-1752
                    ISQLQuery sql;
                    if (System.Configuration.ConfigurationSettings.AppSettings["IsChangeMenu"] != null && System.Configuration.ConfigurationSettings.AppSettings["IsChangeMenu"].ToString() == "Y")
                    {
                        sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name= :UserName  and (u.password=sha1(:PassWord)) and u.Is_Direct_Login='Y'").AddEntity("u", typeof(User));
                    }
                    else
                    {
                        sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name= :UserName  and (u.password=sha1(:PassWord) or u.admin_password=sha1(:PassWord))").AddEntity("u", typeof(User));
                    }
                    sql.SetParameter("UserName", UserName);
                    sql.SetParameter("PassWord", Decryptionbase64Decode(Password));
                    UserList = sql.List<User>();
                    iMySession.Close();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message == "Unable to connect to any of the specified MySQL hosts.")
                    {
                        throw new Exception("Unable to connect to Databse.");
                    }
                    else
                    {
                        throw new Exception(ex.Message); ;
                    }
                }
            }

            if (UserList.Count == 0 && Decryptionbase64Decode(Password) == "0")
                Base64Password = false;
            return UserList;
        }
        //Jira CAP-1752
        public IList<User> CheckImpersonateUserWithPassword(string UserName, string Password, out bool Base64Password)
        {

            Base64Password = true;
            //To refresh the NHibernateSessionUtility
            NHibernateSessionUtility.Instance.FillInstance();
            /* Updated by Ponmozhi Vendan T for the Bug ID = 28045 */
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name= :UserName  and  u.admin_password=sha1(:PassWord)").AddEntity("u", typeof(User));
                    sql.SetParameter("UserName", UserName);
                    sql.SetParameter("PassWord", Decryptionbase64Decode(Password));
                    UserList = sql.List<User>();
                    iMySession.Close();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message == "Unable to connect to any of the specified MySQL hosts.")
                    {
                        throw new Exception("Unable to connect to Databse.");
                    }
                    else
                    {
                        throw new Exception(ex.Message); ;
                    }
                }
            }

            if (UserList.Count == 0 && Decryptionbase64Decode(Password) == "0")
                Base64Password = false;
            return UserList;
        }
        //Jira CAP-1752
        public IList<User> CheckImpersonateUser(string UserName)
        {


            //To refresh the NHibernateSessionUtility
            NHibernateSessionUtility.Instance.FillInstance();
            /* Updated by Ponmozhi Vendan T for the Bug ID = 28045 */
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name= :UserName").AddEntity("u", typeof(User));
                    sql.SetParameter("UserName", UserName);
                    UserList = sql.List<User>();
                    iMySession.Close();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message == "Unable to connect to any of the specified MySQL hosts.")
                    {
                        throw new Exception("Unable to connect to Databse.");
                    }
                    else
                    {
                        throw new Exception(ex.Message); ;
                    }
                }
            }

            //if (UserList.Count == 0 && Decryptionbase64Decode(Password) == "0")
            //    Base64Password = false;
            return UserList;
        }
        public LoginDTO CheckUserDetails(string UserName, string Password, out bool Base64Password, Boolean bIsScnTabLoad)
        {
            LoginDTO objLoginDTO = new LoginDTO();
            Base64Password = true;
            objLoginDTO.User = CheckUser(UserName, Password, out Base64Password);
            //Added for BugId : 34193 
            if (objLoginDTO.User.Count > 0)
            {
                if (objLoginDTO.User[0].status == "A" && objLoginDTO.User[0].Default_Server == string.Empty)
                {
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    objLoginDTO.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(UserName, bIsScnTabLoad);
                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {
                        ISQLQuery query1 = iMySession.CreateSQLQuery("select distinct Default_Server from user where status='A' and default_server<>''");
                        objLoginDTO.DefaultServerCount = query1.List().Count;
                    }

                    //  objLoginDTO.UserSession = userSessionMngr.GetCurrentSessionByUserName(UserName);
                    // objLoginDTO.UserSession = userSessionMngr.GetUserSessionFromXml(UserName);
                    //LastModifiedLocalLookupManager lastmodifiedManager = new LastModifiedLocalLookupManager();
                    //objLoginDTO.lstLookUp = lastmodifiedManager.GetModifiedDates();
                }
                UserSessionManager userSessionMngr = new UserSessionManager();
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    ISQLQuery query1 = iMySession.CreateSQLQuery("select Carrier_ID, Facility_Name from map_user_carrier_facility where user_name='" + UserName + "'");
                    ArrayList ilistObj = new ArrayList(query1.List());
                    if (ilistObj != null)
                    {
                        foreach (object item in ilistObj)
                        {
                            object[] obj = (object[])item;

                            if (objLoginDTO.UserCarrier == string.Empty)
                                objLoginDTO.UserCarrier = obj[0].ToString();
                            else
                                objLoginDTO.UserCarrier += "," + obj[0].ToString();
                        }
                    }

                }


            }
            //End
            return objLoginDTO;
        }
        //Jira CAP-1752
        public LoginDTO CheckImpersonateUserDetails(string UserName, Boolean bIsScnTabLoad)
        {
            LoginDTO objLoginDTO = new LoginDTO();
            objLoginDTO.User = CheckImpersonateUser(UserName);
            //Added for BugId : 34193 
            if (objLoginDTO.User.Count > 0)
            {
                if (objLoginDTO.User[0].status == "A" && objLoginDTO.User[0].Default_Server == string.Empty)
                {
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    objLoginDTO.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(UserName, bIsScnTabLoad);
                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {
                        ISQLQuery query1 = iMySession.CreateSQLQuery("select distinct Default_Server from user where status='A' and default_server<>''");
                        objLoginDTO.DefaultServerCount = query1.List().Count;
                    }

                    //  objLoginDTO.UserSession = userSessionMngr.GetCurrentSessionByUserName(UserName);
                    // objLoginDTO.UserSession = userSessionMngr.GetUserSessionFromXml(UserName);
                    //LastModifiedLocalLookupManager lastmodifiedManager = new LastModifiedLocalLookupManager();
                    //objLoginDTO.lstLookUp = lastmodifiedManager.GetModifiedDates();
                }
                UserSessionManager userSessionMngr = new UserSessionManager();
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    ISQLQuery query1 = iMySession.CreateSQLQuery("select Carrier_ID, Facility_Name from map_user_carrier_facility where user_name='" + UserName + "'");
                    ArrayList ilistObj = new ArrayList(query1.List());
                    if (ilistObj != null)
                    {
                        foreach (object item in ilistObj)
                        {
                            object[] obj = (object[])item;

                            if (objLoginDTO.UserCarrier == string.Empty)
                                objLoginDTO.UserCarrier = obj[0].ToString();
                            else
                                objLoginDTO.UserCarrier += "," + obj[0].ToString();
                        }
                    }

                }


            }
            //End
            return objLoginDTO;
        }
        public LoginDTO CheckUserDetailsLegalOrg(string sUserName, Boolean bIsScnTabLoad)
        {
            LoginDTO objLoginDTO = new LoginDTO();
            objLoginDTO.User = CheckLegalOrgUser(sUserName);
            //Added for BugId : 34193 
            if (objLoginDTO.User.Count > 0)
            {
                if (objLoginDTO.User[0].status == "A" && objLoginDTO.User[0].Default_Server == string.Empty)
                {
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    objLoginDTO.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(sUserName, bIsScnTabLoad);
                    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                    {
                        ISQLQuery query1 = iMySession.CreateSQLQuery("select distinct Default_Server from user where status='A' and default_server<>''");
                        objLoginDTO.DefaultServerCount = query1.List().Count;
                    }

                    //  objLoginDTO.UserSession = userSessionMngr.GetCurrentSessionByUserName(UserName);
                    // objLoginDTO.UserSession = userSessionMngr.GetUserSessionFromXml(UserName);
                    //LastModifiedLocalLookupManager lastmodifiedManager = new LastModifiedLocalLookupManager();
                    //objLoginDTO.lstLookUp = lastmodifiedManager.GetModifiedDates();
                }
                UserSessionManager userSessionMngr = new UserSessionManager();
                using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
                {
                    ISQLQuery query1 = iMySession.CreateSQLQuery("select Carrier_ID, Facility_Name from map_user_carrier_facility where user_name='" + sUserName + "'");
                    ArrayList ilistObj = new ArrayList(query1.List());
                    if (ilistObj != null)
                    {
                        foreach (object item in ilistObj)
                        {
                            object[] obj = (object[])item;

                            if (objLoginDTO.UserCarrier == string.Empty)
                                objLoginDTO.UserCarrier = obj[0].ToString();
                            else
                                objLoginDTO.UserCarrier += "," + obj[0].ToString();
                        }
                    }

                }


            }
            //End
            return objLoginDTO;
        }
        public IList<User> CheckLegalOrgUser(string sUserName)
        {


            //To refresh the NHibernateSessionUtility
            NHibernateSessionUtility.Instance.FillInstance();
            /* Updated by Ponmozhi Vendan T for the Bug ID = 28045 */
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name= :UserName").AddEntity("u", typeof(User));
                    sql.SetParameter("UserName", sUserName);
                    UserList = sql.List<User>();
                    iMySession.Close();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message == "Unable to connect to any of the specified MySQL hosts.")
                    {
                        throw new Exception("Unable to connect to Databse.");
                    }
                    else
                    {
                        throw new Exception(ex.Message); ;
                    }
                }
            }

            //if (UserList.Count == 0 && Decryptionbase64Decode(Password) == "0")
            //    Base64Password = false;
            return UserList;
        }
        public void SaveLastSuccessfulyLoginDate(string sUserName, DateTime dtLoginDate)
        {
            IList<User> addList = null;
            IList<User> updateList = GetUser(sUserName);
            if (updateList.Count>0)
            {
                updateList[0].Last_Successful_Login_Date_Time = dtLoginDate;
                SaveUpdateDeleteWithTransaction(ref addList, updateList, null, string.Empty);
            }
        }

        public LoginDTO CheckUserDetailsWithoutPassword(string UserName, Boolean bIsScnTabLoad)
        {
            LoginDTO objLoginDTO = new LoginDTO();

            //using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            //{
            //    ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("user_name", UserName));
            //    objLoginDTO.User = criteria.List<User>();
            //    iMySession.Close();

            //}

            ScnTabManager objScnTabmngr = new ScnTabManager();
            objLoginDTO.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(UserName, bIsScnTabLoad);
            UserSessionManager userSessionMngr = new UserSessionManager();

            return objLoginDTO;
        }

        public IList<User> GetUserList(string sLegalOrg)
        {
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Legal_Org",sLegalOrg)).AddOrder(Order.Asc("user_name"));
                UserList = criteria.List<User>();
                iMySession.Close();

            }
            return UserList;
        }

        public IList<User> UserPreference()
        {
            IList<User> returnUser = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User));
                returnUser = criteria.List<User>();

            }
            return returnUser;
        }

        public IList<User> GetUser(string UserName)
        {

            IList<User> UserList = new List<User>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("user_name", UserName)).AddOrder(Order.Asc("user_name"));
                UserList = criteria.List<User>();
                iMySession.Close();
            }
            return UserList;
        }


        public void UpdatePassword(IList<User> userList, string MACAddress)
        {
            IList<User> useraddList = null;
            IList<User> DeleteList = null;
            for (int i = 0; i < userList.Count; i++)
            {
                userList[i].password = sha1encrypt(userList[i].password).ToLower();
            }

            //SaveUpdateDeleteWithTransaction(ref useraddList, userList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref useraddList, ref userList, DeleteList, MACAddress, false, false, 0, string.Empty);
        }

        public string sha1encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA1CryptoServiceProvider sha1hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(phrase));
            return byteArrayToString(hashedDataBytes);
        }

        public string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }

        public string GetConnectionStringDetails()
        {
            string strconnecitonString = string.Empty;
            ISession ConnectionString = NHibernateSessionManager.Instance.CreateISession();
            strconnecitonString = ConnectionString.Connection.ConnectionString;
            ConnectionString.Close();
            return strconnecitonString;

        }

        public string GetLocalConnectionStringDetails()
        {
            string strconnecitonString = string.Empty;
            ISession LocalConnectionString = NHibernateSessionManager.Instance.CreateISession();
            strconnecitonString = LocalConnectionString.Connection.ConnectionString;
            LocalConnectionString.Close();
            return strconnecitonString;

        }

        //Selvaraman - Decryption
        public string Decryptionbase64Decode(string sData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            //BugID:53715-- Added condition for Base64String check and displays Invalid Password if PWD is not BASE64 string
            try
            {
                byte[] todecode_byte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                return "0";
            }

        }
        public string GetDefaultFacilityForLogin(string sUserName)
        {
            IList<User> UserList = new List<User>();
            string sFacilityName = string.Empty;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("user_name", sUserName));
                UserList = criteria.List<User>();
                if (UserList.Count > 0)
                {
                    sFacilityName = criteria.List<User>()[0].Default_Facility;
                }
                iMySession.Close();
            }
            return sFacilityName;
        }
        public IList<User> GetUserAndFacilityList()
        {
            IList<User> ilstUser = new List<User>();
            ArrayList aryuser = new ArrayList();
            User objUser = null;
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.UserName.FacilityName");
                aryuser = new ArrayList(query.List());
                iMySession.Close();
            }
            if (aryuser.Count != 0)
            {
                for (int i = 0; i < aryuser.Count; i++)
                {
                    objUser = new User();
                    object[] obj = (object[])aryuser[i];
                    if (obj[0] != null)
                    {
                        objUser.user_name = obj[0].ToString();
                    }
                    if (obj[1] != null)
                    {
                        objUser.Default_Facility = obj[1].ToString();
                    }
                    ilstUser.Add(objUser);
                }

            }
            return ilstUser;
        }



        public void UpdateUserDetails(IList<User> userList, string MACAddress)
        {
            IList<User> useraddList = null;
            IList<User> DeleteList = null;
            //SaveUpdateDeleteWithTransaction(ref useraddList, userList, null, MACAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref useraddList, ref userList, DeleteList, MACAddress, false, false, 0, string.Empty);
        }

        public IList<User> GetUserBasedonRole(string sRole)
        {
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("role", sRole)).AddOrder(Order.Asc("user_name"));
                UserList = criteria.List<User>();
                iMySession.Close();
            }
            return UserList;
        }

        public IList<User> GetUserbyPhysicianLibraryID(ulong Physician_Library_ID)
        {
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("Physician_Library_ID", Physician_Library_ID));
                UserList = criteria.List<User>();
                iMySession.Close();
            }
            return UserList;
        }
        public IList<User> GetMedicalAssistantUserList()
        {
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria criteria = iMySession.CreateCriteria(typeof(User)).Add(Expression.Eq("role", "MEDICAL ASSISTANT")).Add(Expression.Eq("status", "A"));
                UserList = criteria.List<User>();
                iMySession.Close();
            }
            return UserList;

        }
        public bool CheckIfValidPwd(string UserName, string Password)
        {
            bool isValid = false;
            bool Base64Password = false;
            IList<User> lstUser = new List<User>();
            lstUser = CheckUser(UserName, Password, out Base64Password);
            if (lstUser != null && lstUser.Count > 0)
                isValid = true;
            return isValid;
        }

        //Start LDAP
        public LoginDTO CheckUserDetailsCarePointe(string UserName, string Password, string sLDAPUserName, string sDomainName, Boolean bIsScnTabLoad)
        {

            LoginDTO objLoginDTO = new LoginDTO();
            objLoginDTO.User = CheckUserCarePointe(UserName, Password, sLDAPUserName, sDomainName);

            //Added for BugId : 34193 
            if (objLoginDTO.User.Count > 0)
            {
                if (objLoginDTO.User[0].status == "A")
                {
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    objLoginDTO.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(UserName,bIsScnTabLoad);
                    UserSessionManager userSessionMngr = new UserSessionManager();
                    //  objLoginDTO.UserSession = userSessionMngr.GetCurrentSessionByUserName(UserName);
                    // objLoginDTO.UserSession = userSessionMngr.GetUserSessionFromXml(UserName);
                    //LastModifiedLocalLookupManager lastmodifiedManager = new LastModifiedLocalLookupManager();
                    //objLoginDTO.lstLookUp = lastmodifiedManager.GetModifiedDates();
                }
            }
            //End
            return objLoginDTO;
        }
        public Boolean LDAPUserCheck(string strDomain, string strUser, string strPass, AuthenticationTypes atAuthentType)
        {

            Boolean bSuccess = false;
            // now create the directory entry to establish connection
            using (DirectoryEntry deDirEntry = new DirectoryEntry(strDomain,
                                                                 strUser,
                                                                 strPass,
                                                                 atAuthentType))
            {
                // if user is verified then it will welcome then  
                try
                {
                    // MessageBox.Show("Welcome to '" + deDirEntry.Name + "'");
                    bSuccess = true;
                    string sName = deDirEntry.Name;
                    // TODO: add your specific tasks here
                }
                catch
                {
                    //MessageBox.Show("Sorry, unable to verify your information");
                    bSuccess = false;
                }
            }
            return bSuccess;

        }
        public IList<User> CheckUserCarePointe(string UserName, string Password, string sLDAPUserName, string sDomainName)
        {

            IList<User> UserList = new List<User>();
            AuthenticationTypes atAuthentType = AuthenticationTypes.ServerBind;
            Boolean bCheck = LDAPUserCheck(sDomainName, sLDAPUserName, Password, atAuthentType);
            if (!bCheck)
            {
                return UserList;
            }

            //To refresh the NHibernateSessionUtility
            NHibernateSessionUtility.Instance.FillInstance();
            /* Updated by Ponmozhi Vendan T for the Bug ID = 28045 */
            //IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.user_name= :UserName").AddEntity("u", typeof(User));
                sql.SetParameter("UserName", UserName);
                //sql.SetParameter("PassWord", Decryptionbase64Decode(Password));
                UserList = sql.List<User>();
                iMySession.Close();
            }


            return UserList;
        }



        public LoginDTO GetUserDetailsByOktaEmailAddress(string sEmailAddress, bool bIsScnTabLoad)
        {
            LoginDTO objLoginDTO = new LoginDTO();

            //To refresh the NHibernateSessionUtility
            NHibernateSessionUtility.Instance.FillInstance();
            /* Updated by Ponmozhi Vendan T for the Bug ID = 28045 */
            IList<User> UserList = new List<User>();
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.EMail_Address= :UserName and u.status = 'A' and u.Is_Direct_Login='Y'").AddEntity("u", typeof(User));
                sql.SetParameter("UserName", sEmailAddress);
                UserList = sql.List<User>();

                objLoginDTO.User = UserList;

                if (UserList.Count > 0)
                {
                    ScnTabManager objScnTabmngr = new ScnTabManager();
                    objLoginDTO.UserPermissionDTO = objScnTabmngr.GetUserPermisssions(UserList[0].user_name, bIsScnTabLoad);


                    ISQLQuery query1 = iMySession.CreateSQLQuery("select distinct Default_Server from user where status='A' and default_server<>''");
                    objLoginDTO.DefaultServerCount = query1.List().Count;


                    //  objLoginDTO.UserSession = userSessionMngr.GetCurrentSessionByUserName(UserName);
                    // objLoginDTO.UserSession = userSessionMngr.GetUserSessionFromXml(UserName);
                    //LastModifiedLocalLookupManager lastmodifiedManager = new LastModifiedLocalLookupManager();
                    //objLoginDTO.lstLookUp = lastmodifiedManager.GetModifiedDates();

                    UserSessionManager userSessionMngr = new UserSessionManager();

                    ISQLQuery query2 = iMySession.CreateSQLQuery("select Carrier_ID, Facility_Name from map_user_carrier_facility where user_name='" + UserList[0].user_name + "'");
                    ArrayList ilistObj = new ArrayList(query2.List());
                    if (ilistObj != null)
                    {
                        foreach (object item in ilistObj)
                        {
                            object[] obj = (object[])item;

                            if (objLoginDTO.UserCarrier == string.Empty)
                                objLoginDTO.UserCarrier = obj[0].ToString();
                            else
                                objLoginDTO.UserCarrier += "," + obj[0].ToString();
                        }
                    }
                }
            }

            return objLoginDTO;
        }


        public IList<User> GetUserByEmailAddress(string sEmailAddress)
        {

            IList<User> UserList = new List<User>();
            // ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = iMySession.CreateSQLQuery("Select * from User u where u.EMail_Address= :EmailAddress AND u.status='A'").AddEntity("u", typeof(User));
                sql.SetParameter("EmailAddress", sEmailAddress);
                UserList = sql.List<User>();
                iMySession.Close();
            }
            return UserList;
        }



        //End
        #endregion
    }
}
