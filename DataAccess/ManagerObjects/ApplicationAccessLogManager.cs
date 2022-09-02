using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IApplicationAccessLogManager : IManagerBase<ApplicationAccessLog, uint>
    {
        void SaveApplicationAccessLog(IList<ApplicationAccessLog> SaveActivity, string sMacAddress);
    }
    public partial class ApplicationAccessLogManager : ManagerBase<ApplicationAccessLog, uint>, IApplicationAccessLogManager
    {
        #region Constructors

        public ApplicationAccessLogManager()
            : base()
        {

        }
        public ApplicationAccessLogManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion
        #region Methods
        public void SaveApplicationAccessLog(IList<ApplicationAccessLog> SaveList, string sMacAddress)
        {
            IList<ApplicationAccessLog> updateList = null;
            IList<ApplicationAccessLog> deleteList = null;
            SaveUpdateDelete_DBAndXML_WithTransaction(ref SaveList, ref updateList, deleteList, sMacAddress, false, false, Convert.ToUInt32(SaveList[0].Patient_ID), string.Empty);
        }
        #endregion

    }
}
