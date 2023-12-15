using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using Acurus.Capella.Core.DTO;
using System.Web;
using System.IO;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IUserScnTabManager : IManagerBase<user_scn_tab, int>
    {
        bool IsNewUser(string userName);
        IList<user_scn_tab> GetAllScreensUsingscreenid(string User_Name);
        IList<user_scn_tab> GetFocusedExamDisableListByID(ulong scanID, string sUserName);
        IList<user_scn_tab> GetUserScreenTabByID(ulong scanID, string sUserName);
    }

    public partial class UserScnTabManager : ManagerBase<user_scn_tab, int>, IUserScnTabManager
    {

        #region Constructors

        public UserScnTabManager()
            : base()
        {

        }
        public UserScnTabManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Get Methods

        public bool IsNewUser(string userName)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            bool bture;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                bture = iMySession.CreateCriteria(typeof(user_scn_tab)).Add(Expression.Eq("User_Name", userName)).List<user_scn_tab>().Count == 0 ? true : false;
                iMySession.Close();
            }
            return bture;

        }
        public IList<user_scn_tab> GetAllScreensUsingscreenid(string User_Name)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<user_scn_tab> list = new List<user_scn_tab>();

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query1 = iMySession.GetNamedQuery("UserTab.GetAllScreensUsingUserid");
                query1.SetString(0, User_Name);
                ArrayList listArray = new ArrayList(query1.List());
                user_scn_tab tab;
                if (listArray.Count > 0)
                {
                    foreach (object[] obj in listArray)
                    {
                        tab = new user_scn_tab();
                        tab.scn_id = Convert.ToUInt64(obj[0]);
                        tab.Permission = obj[1].ToString();
                        tab.user_name = obj[2].ToString();
                        list.Add(tab);
                    }
                }
                iMySession.Close();
            }
            return list;
        }

        public IList<user_scn_tab> GetFocusedExamDisableListByID(ulong scanID,string sUserName)
        {
            IList<user_scn_tab> listscan = new List<user_scn_tab>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(user_scn_tab)).Add(Expression.Eq("scn_id", scanID)).Add(Expression.Eq("user_name", sUserName)).Add(Expression.Eq("Permission", "R"));
                listscan = crit.List<user_scn_tab>();
                iMySession.Close();
            }
            return listscan;
        }

        public IList<user_scn_tab> GetUserScreenTabByID(ulong scanID, string sUserName)
        {
            IList<user_scn_tab> listscan = new List<user_scn_tab>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(user_scn_tab)).Add(Expression.Eq("scn_id", scanID)).Add(Expression.Eq("user_name", sUserName)).Add(Expression.Eq("Permission", "U"));
                listscan = crit.List<user_scn_tab>();
                iMySession.Close();
            }
            return listscan;
        }

        #endregion
    }
}
