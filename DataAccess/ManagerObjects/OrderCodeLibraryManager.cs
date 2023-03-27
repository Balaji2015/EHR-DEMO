using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IOrderCodeLibraryManager : IManagerBase<OrderCodeLibrary, string>
    {
        IList<OrderCodeLibrary> GetOrderCodeForCPTAndLab(string ordercode, ulong LabID);
        OrderCodeLibrary GetOrderCodeDetailsForSelectedOrderCode(string ordercode);
        IList<OrderCodeLibrary> SearchOrderCodeForCodeAndDesc(string Code, string Description, ulong LabID);
        void ImportCDCOrderCodeLibrary(IList<OrderCodeLibrary> ilstOrderCodeLibrary, string MACAddress,bool IsTruncate);
    }
    public partial class OrderCodeLibraryManager : ManagerBase<OrderCodeLibrary, string>, IOrderCodeLibraryManager
    {
        #region Constructors

        public OrderCodeLibraryManager()
            : base()
        {

        }
        public OrderCodeLibraryManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region IOrderCodeLibraryManager Members

        public IList<OrderCodeLibrary> GetOrderCodeForCPTAndLab(string ordercode, ulong LabID)
        {
            IList<OrderCodeLibrary> lstOrdrcodelib = new List<OrderCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", ordercode)).Add(Expression.Eq("Lab_ID", LabID));
                lstOrdrcodelib= crit.List<OrderCodeLibrary>();
                iMySession.Close();
            }
            return lstOrdrcodelib;
        }


        public OrderCodeLibrary GetOrderCodeDetailsForSelectedOrderCode(string ordercode)
        {
           
            OrderCodeLibrary obj = new OrderCodeLibrary();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Order_Code", ordercode));

                if (crit.List<OrderCodeLibrary>().Count > 0)
                {
                    obj = crit.List<OrderCodeLibrary>()[0];
                }
                iMySession.Close();
            }
            return obj;
        }



        public IList<OrderCodeLibrary> SearchOrderCodeForCodeAndDesc(string Code, string Description, ulong LabID)
        {
            IList<OrderCodeLibrary> retList = new List<OrderCodeLibrary>();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //Gitlab #3922  - An item with the same key has already been added
                //            ICriteria crit = iMySession.CreateCriteria(typeof(OrderCodeLibrary))
                //                .SetProjection(Projections.Distinct(Projections.ProjectionList()
                //.Add(Projections.Alias(Projections.Property("Order_Code"), "Order_Code"))
                //.Add(Projections.Alias(Projections.Property("Order_Code_Name"), "Order_Code_Name"))
                //.Add(Projections.Alias(Projections.Property("Order_Group_Name"), "Order_Group_Name"))))
                //                .Add(Expression.Like("Order_Code", Code + "%")).Add(Expression.Like("Order_Code_Name", "%" + Description + "%"))
                //                .Add(Expression.Eq("Lab_ID", LabID));

                ICriteria crit = iMySession.CreateCriteria(typeof(OrderCodeLibrary))
                   .SetProjection(Projections.Distinct(Projections.ProjectionList()
   .Add(Projections.Alias(Projections.Property("Order_Code"), "Order_Code"))
   .Add(Projections.Alias(Projections.Property("Order_Code_Name"), "Order_Code_Name"))))
                   .Add(Expression.Like("Order_Code", Code + "%")).Add(Expression.Like("Order_Code_Name", "%" + Description + "%"))
                   .Add(Expression.Eq("Lab_ID", LabID));

                crit.SetResultTransformer(
     new NHibernate.Transform.AliasToBeanResultTransformer(typeof(OrderCodeLibrary)));
               
                retList = crit.List<OrderCodeLibrary>();
                iMySession.Close();
            }
                return retList;
            
        }
        public void ImportCDCOrderCodeLibrary(IList<OrderCodeLibrary> ilstOrderCodeLibrary, string MACAddress,bool IsTruncate)
        {

            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria critLabName = iMySession.CreateCriteria(typeof(Lab)).Add(Expression.Eq("Lab_Name", "Quest Diagnostics")).Add(Expression.Eq("Lab_Type", "LAB"));
                IList<Lab> ilslab = critLabName.List<Lab>();

                ICriteria crit = iMySession.CreateCriteria(typeof(OrderCodeLibrary)).Add(Expression.Eq("Lab_ID", ilslab[0].Id));
                IList<OrderCodeLibrary> deletelist = crit.List<OrderCodeLibrary>();
                foreach (OrderCodeLibrary obj in ilstOrderCodeLibrary)
                {
                    obj.Lab_ID = 2;
                    //obj.Id = obj.Order_Code;
                    obj.CPT_Code = " ";
                    obj.CPT_Code_Description = " ";
                    obj.Order_Code_Procedure_Class = " ";
                    obj.Order_Code_Question_Set_Segment = " ";
                    obj.Order_Code_Type = " ";

                }
                IList<OrderCodeLibrary> templist = new List<OrderCodeLibrary>();

                //if (IsTruncate)
                //    SaveUpdateDeleteWithTransaction(ref templist, null, deletelist, MACAddress);
                
                //SaveUpdateDeleteWithTransaction(ref ilstOrderCodeLibrary, null, null, MACAddress);

                iMySession.Close();
            }
        }
        #endregion
    }
}
