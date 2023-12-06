using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using Acurus.Capella.Core;
using Acurus.Capella.Core.DTO;
using NHibernate.Criterion;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Diagnostics;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IProcUserManager : IManagerBase<ProcUser, ulong>
    {
        IList<ProcUser> GetProcUserList();
        //ProcUserDTO SaveProcUser(ProcUser objProcUser, int iPageNumber, int iMaxResult, string MACAddress);
        //ProcUserDTO UpdateProcUser(ProcUser objProcUser, int iPageNumber, int iMaxResult, string MACAddress);
        //ProcUserDTO DeleteProcUser(ulong ulProcUserId, int iPageNumber, int iMaxResult, string UserName, string MacAddress);
        ProcUserDTO GetProcUser(int iPageNumber, int iMaxResult);
        Boolean CheckAlreadyExist(string sProcessName, string sUserName);
        IList<ProcUser> GetProcUserForUpdate(ulong ulProcId);

        IList<ProcUser> GetProcUserList(string sProcessName);
        IList<ProcUser> GetUserProcessListByUserName(string sUserName);
    }

    public partial class ProcUserManager : ManagerBase<ProcUser, ulong>, IProcUserManager
    {
        #region Constructors

        public ProcUserManager()
            : base()
        {

        }
        public ProcUserManager
            (INHibernateSession session)
            : base(session)
        {

        }

        public IList<ProcUser> GetProcUserList()
        {
            IList<ProcUser> UserList = new List<ProcUser>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crt = iMySession.CreateCriteria(typeof(ProcUser));
                UserList = crt.List<ProcUser>();
                iMySession.Close();
            }
            return UserList;
        }
        //public ProcUserDTO SaveProcUser(ProcUser objProcUser, int iPageNumber, int iMaxResult, string MACAddress)
        //{
        //    ProcUserDTO objProcUserDTO = new ProcUserDTO();
        //    if (!CheckAlreadyExist(objProcUser.Process_Name, objProcUser.User_Name))
        //    {
        //        IList<ProcUser> ProcUserLst = new List<ProcUser>();
        //        ProcUserLst.Add(objProcUser);
        //        //IList<ProcUser> updatelist = null;
        //        IList<ProcUser> deleteList = null;
        //        //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //        //SaveUpdateDeleteWithTransaction(ref ProcUserLst, null, null, MACAddress);
        //        return GetProcUser(iPageNumber, iMaxResult);
        //    }
        //    else
        //    {
        //        objProcUserDTO.bCheck = true;
        //        return objProcUserDTO;
        //    }
        //}

        //public ProcUserDTO UpdateProcUser(ProcUser objProcUser, int iPageNumber, int iMaxResult, string MACAddress)
        //{
        //    ProcUserDTO objProcUserDTO = new ProcUserDTO();
        //    if (!CheckAlreadyExist(objProcUser.Process_Name, objProcUser.User_Name))
        //    {
        //        IList<ProcUser> ProcUserLst = new List<ProcUser>();
        //        ProcUserLst.Add(objProcUser);
        //       // IList<ProcUser> ProcUserLstsave = null;
        //        //IList<ProcUser> deleteList = null;
        //        //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //        //SaveUpdateDeleteWithTransaction(ref ProcUserLstsave, ProcUserLst, null, MACAddress);
        //        return GetProcUser(iPageNumber, iMaxResult);
        //    }
        //    else
        //    {
        //        objProcUserDTO.bCheck = true;
        //        return objProcUserDTO;
        //    }

        //}
        //public ProcUserDTO DeleteProcUser(ulong ulProcUserId, int iPageNumber, int iMaxResult, string UserName, string MacAddress)
        //{
        //    using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
        //    {
        //        ICriteria crt = iMySession.CreateCriteria(typeof(ProcUser)).Add(Expression.Eq("Id", ulProcUserId));
        //        IList<ProcUser> ProcUser = crt.List<ProcUser>();
        //        if (ProcUser != null && ProcUser.Count > 0)
        //        {
        //            ProcUser[0].Modified_By = UserName;
        //            //IList<ProcUser> Savelist = null;
        //            //IList<ProcUser> updatelist = null;
        //            //This method is not used in entire solution. Nijanthan commented the below line on 22-3-16.
        //            //SaveUpdateDeleteWithTransaction(ref Savelist, null, ProcUser, MacAddress); 
        //        }

        //        //IQuery query = session.GetISession().CreateSQLQuery("delete from process_user where Process_User_ID=" + ulProcUserId);
        //        //query.ExecuteUpdate();
        //        iMySession.Close();
        //    }
        //    return GetProcUser(iPageNumber, iMaxResult);

        //}
        public ProcUserDTO GetProcUser(int iPageNumber, int iMaxResult)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            ProcUserDTO objProcUserDto = new ProcUserDTO();
            ArrayList aryProcUser = null;
            IList<ProcUser> ProcUserlst = new List<ProcUser>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                IQuery query = iMySession.GetNamedQuery("Get.GetProcUserCount");
                objProcUserDto.ProcUserCount = Convert.ToInt32(query.List()[0]);

                IQuery query1 = iMySession.GetNamedQuery("Get.GetProcUser");
                int PageNumber = iPageNumber - 1;
                query1.SetInt32(0, PageNumber * iMaxResult);
                query1.SetInt32(1, iMaxResult);
                aryProcUser = new ArrayList(query1.List());
                foreach (object[] objProc in aryProcUser)
                {
                    ProcUser objProcUser = new ProcUser();
                    objProcUser.Id = Convert.ToUInt32(objProc[0]);
                    objProcUser.Process_Name = objProc[1].ToString();
                    objProcUser.User_Name = objProc[2].ToString();
                    objProcUser.Created_By = objProc[3].ToString();
                    objProcUser.Created_Date_And_Time = Convert.ToDateTime(objProc[4]);
                    ProcUserlst.Add(objProcUser);
                }
                objProcUserDto.ProcUser = ProcUserlst;
                iMySession.Close();
            }
            return objProcUserDto;
        }

        public Boolean CheckAlreadyExist(string sProcessName, string sUserName)
        {
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crt = iMySession.CreateCriteria(typeof(ProcUser)).Add(Expression.Eq("Process_Name", sProcessName)).Add(Expression.Eq("User_Name", sUserName));
                IList<ProcUser> UserList = crt.List<ProcUser>();
                if (UserList.Count > 0)
                {
                    iMySession.Close();
                    return true;
                }

                else
                {
                    iMySession.Close();
                    return false;
                }
            }
               
        }

        public IList<ProcUser> GetProcUserForUpdate(ulong ulProcId)
        {
            IList<ProcUser> UserList = new List<ProcUser>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crt = iMySession.CreateCriteria(typeof(ProcUser)).Add(Expression.Eq("Id", ulProcId));
                UserList = crt.List<ProcUser>();
                iMySession.Close();
            }
            return UserList;
        }
        public IList<ProcUser> GetProcUserList(string sProcessName)
        {
            //ISession iMySession = NHibernateSessionManager.Instance.CreateISession();
            IList<ProcUser> UserList = new List<ProcUser>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcUser)).Add(Expression.Eq("Process_Name", sProcessName)).AddOrder(Order.Asc("User_Name"));
                UserList = crit.List<ProcUser>();
                iMySession.Close();
            }
            return UserList;
        }
        public IList<ProcUser> GetUserProcessListByUserName(string sUserName)
        {
            IList<ProcUser> UserList = new List<ProcUser>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(ProcUser)).Add(Expression.Eq("User_Name", sUserName)).Add(Expression.Eq("Status", "A"));
                UserList = crit.List<ProcUser>();
                iMySession.Close();
            }
            return UserList;
        }
        #endregion



    }
}
