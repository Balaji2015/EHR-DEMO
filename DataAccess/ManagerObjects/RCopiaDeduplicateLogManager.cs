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

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IRCopiaDeduplicateLogManager : IManagerBase<RCopiaDeduplicateLog, ulong>
    {
        void SaveRcopia_deduplicate_logWithTransaction(IList<RCopiaDeduplicateLog> ilstRcopia_deduplicate_log, IList<RCopiaDeduplicateLog> ListToUpdateRcopia_deduplicate_log, string MACAddress);
    }

    public partial class RCopiaDeduplicateLogManager : ManagerBase<RCopiaDeduplicateLog, ulong>, IRCopiaDeduplicateLogManager
    {
        #region Constructors

        public RCopiaDeduplicateLogManager()
            : base()
        {

        }
        public RCopiaDeduplicateLogManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public void SaveRcopia_deduplicate_logWithTransaction(IList<RCopiaDeduplicateLog> ilstRcopia_deduplicate_log, IList<RCopiaDeduplicateLog> ListToUpdateRcopia_deduplicate_log, string MACAddress)
        {
            SaveUpdateDeleteWithTransaction(ref ilstRcopia_deduplicate_log, ListToUpdateRcopia_deduplicate_log, null, MACAddress);
        }
        #endregion
    }
}
