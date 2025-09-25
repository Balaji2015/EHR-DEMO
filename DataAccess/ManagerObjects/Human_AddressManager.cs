using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Mail;
using System.Data;
using System.Linq;
using System.Xml;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IHuman_AddressManager : IManagerBase<Human_Address, ulong>
    {
        IList<Human_Address> GetHumanAddressByHuman(string ulHumanID);
        IList<Human_Address> GetHumanAddressByHumanAddressID(string sHumanAddressID);

        void SaveHuman_AddressWithTransaction(IList<Human_Address> ilstHuman_Address, IList<Human_Address> ListToUpdateHuman_Address, string MACAddress);
        IList<Human_Address> DeleteHumanAddress(string sHumanAddressID);
    }

    public partial class Human_AddressManager : ManagerBase<Human_Address, ulong>, IHuman_AddressManager
    {
        #region Constructors

        public Human_AddressManager()
            : base()
        {

        }
        public Human_AddressManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods


        public IList<Human_Address> GetHumanAddressByHuman(string sHumanID)
        {
            IList<Human_Address> ilstHuman_Address = new List<Human_Address>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(Human_Address)).Add(Expression.Eq("Human_ID", Convert.ToUInt64(sHumanID)));
            ilstHuman_Address = crit.List<Human_Address>();

            return ilstHuman_Address;
            
        }

        public IList<Human_Address> GetHumanAddressByHumanAddressID(string sHumanAddressID)
        {
            IList<Human_Address> ilstHuman_Address = new List<Human_Address>();
            session.GetISession().Close();
            ICriteria crit = session.GetISession().CreateCriteria(typeof(Human_Address)).Add(Expression.Eq("Id", Convert.ToUInt64(sHumanAddressID)));
            ilstHuman_Address = crit.List<Human_Address>();

            return ilstHuman_Address;

        }

        public IList<Human_Address> DeleteHumanAddress(string sHumanAddressID)
        {
            IList<Human_Address> ilstHuman_Address = new List<Human_Address>();
            session.GetISession().Close();
            ISQLQuery sql = session.GetISession().CreateSQLQuery($"delete from human_address where Human_Address_ID = {Convert.ToUInt64(sHumanAddressID)};");

            ArrayList ilistObj = new ArrayList(sql.List());

            return ilstHuman_Address;
            
        }

        public void SaveHuman_AddressWithTransaction(IList<Human_Address> ilstHuman_Address , IList<Human_Address> ListToUpdateHuman_Address, string MACAddress)
        {
            SaveUpdateDeleteWithTransaction(ref ilstHuman_Address, ListToUpdateHuman_Address, null, MACAddress);
        }
        #endregion
    }
}
