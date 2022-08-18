
using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface ICQMDetailManager : IManagerBase<CQMDetail, ulong>
    {
        IList<CQMDetail> AppendCQMDetail(IList<CQMDetail> CQMDetailList, string MACAddress);

    }
    public partial class CQMDetailManager : ManagerBase<CQMDetail, ulong>, ICQMDetailManager
    {
        #region Constructors

        public CQMDetailManager()
            : base()
        {

        }
        public CQMDetailManager(INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Get Methods

        public IList<CQMDetail> AppendCQMDetail(IList<CQMDetail> CQMDetailList, string MACAddress)
        {
            IList<CQMDetail> nullList = null;

            SaveUpdateDelete_DBAndXML_WithTransaction(ref CQMDetailList, ref nullList, null, MACAddress,false, false,0, String.Empty);
            //  
            //IList<CQMDetail> CQMDetailList = new List<CQMDetail>();
            //IList<Examination> nullList = null;
            //string ExamTabName = string.Empty;
            //if (ExamList.Count > 0)
            //{
            //    if (ExamList[0].Category.ToUpper() == "GENERAL WITH SPECIALTY")
            //        ExamTabName = "General";
            //    else if (ExamList[0].Category.ToUpper() == "FOCUSED")
            //        ExamTabName = "Focused";
            //    SaveUpdateDelete_DBAndXML_WithTransaction(ref ExamList, ref nullList, null, MACAddress, true, true, ExamList[0].Encounter_ID, ExamTabName);
            //    examScreenList = ExamList;
            //}
            return CQMDetailList;
        }


        #endregion
    }
}
