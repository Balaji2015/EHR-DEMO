using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using Acurus.Capella.Core.DTO;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IOrdersRequiredFormsManager : IManagerBase<OrdersRequiredForms, ulong>
    {
        int BatchOperationsToOrdersRequiredForms(IList<OrdersRequiredForms> saveList, IList<OrdersRequiredForms> updtList, IList<OrdersRequiredForms> delList, ISession MySession, string MACAddress);
        //IList<Orders> ilstOrderList(ulong OrdersSubmitID,IList<string> sCPTs);
        string sPreferred_ReadingPhysician(ulong OrdersSubmitID);
        IList<OrdersRequiredForms> ilstOrderList(ulong OrdersSubmitID);
    }
    public partial class OrdersRequiredFormsManager : ManagerBase<OrdersRequiredForms, ulong>, IOrdersRequiredFormsManager
    {
        #region Constructors

        public OrdersRequiredFormsManager()
            : base()
        {

        }
        public OrdersRequiredFormsManager
            (INHibernateSession session)
            : base(session)
        {
        }

        #endregion

        #region IOrdersRequiredFormsManager Members

        public int BatchOperationsToOrdersRequiredForms(IList<OrdersRequiredForms> saveList, IList<OrdersRequiredForms> updtList, IList<OrdersRequiredForms> delList, ISession MySession, string MACAddress)
        {
            //return SaveUpdateDeleteWithoutTransaction(ref saveList, updtList, delList, MySession, MACAddress);
            GenerateXml XMLObj = new GenerateXml();
            return SaveUpdateDelete_DBAndXML_WithoutTransaction(ref saveList, ref updtList, delList, MySession, MACAddress, false, true, 0, string.Empty, ref XMLObj);
        }

        //public IList<Orders> ilstOrderList(ulong OrdersSubmitID, IList<string> sCPTs)
        //{
        //    IList<Orders> objOrders = new List<Orders>();
        //    IList<OrdersDTO> ilstRequiredOrder = new List<OrdersDTO>();
        //    string sLabProcedure=string.Empty;
        //    if(sCPTs.Count>0)
        //    {
        //        foreach(string slab in sCPTs)
        //        {
        //            if(sLabProcedure==string.Empty)
        //            {
        //                sLabProcedure=slab;
        //            }
        //            else
        //            {
        //                sLabProcedure+=","+slab;
        //            }
        //        }
        //    }

        //    ISQLQuery sqlquery = session.GetISession().CreateSQLQuery("Select O.* from Orders O where O.order_submit_ID='" + OrdersSubmitID + "' and O.Lab_Procedure in ('" + sLabProcedure + "')").AddEntity("O", typeof(Orders));
        //    objOrders = sqlquery.List<Orders>();
        //    return objOrders;
        //}

        public string sPreferred_ReadingPhysician(ulong OrdersSubmitID)
        {
            //Cap - 2505
            string sPhysician_id ="";
            if (OrdersSubmitID == 0)
            {
                return sPhysician_id;
            }
            IList<OrdersSubmit> objOrders = new List<OrdersSubmit>();
            
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("Select O.* from orders_submit O where O.order_submit_ID='" + OrdersSubmitID + "'").AddEntity("O", typeof(OrdersSubmit));
                objOrders = sqlquery.List<OrdersSubmit>();
                sPhysician_id = objOrders[0].Prefered_Reading_Provider_ID.ToString();
                iMySession.Close();
            }
            return sPhysician_id;
        }
        public IList<OrdersRequiredForms> ilstOrderList(ulong OrdersSubmitID)
        {
            IList<Orders> objOrders = new List<Orders>();
            IList<OrdersRequiredForms> ilstRequiredOrder = new List<OrdersRequiredForms>();
            string sLabProcedure = string.Empty;
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sqlquery = iMySession.CreateSQLQuery("Select O.* from Orders O where O.order_submit_ID='" + OrdersSubmitID + "'").AddEntity("O", typeof(Orders));
                objOrders = sqlquery.List<Orders>();
                if (objOrders.Count > 0)
                {
                    ISQLQuery sqlquery1 = iMySession.CreateSQLQuery("Select O.* from orders_required_forms O where O.order_id='" + objOrders[0].Id + "'").AddEntity("O", typeof(OrdersRequiredForms));
                    ilstRequiredOrder = sqlquery1.List<OrdersRequiredForms>();
                }
                iMySession.Close();
            }
            return ilstRequiredOrder;
        }

        #endregion

    }
}
