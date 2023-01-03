using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IMapXMLBlobManager : IManagerBase<MapXMLBlob, ulong>
    {

        IList<MapXMLBlob> GetMapXMLBlobList();
    }


    public partial class MapXMLBlobManager : ManagerBase<MapXMLBlob, ulong>, IMapXMLBlobManager
    {

        #region Constructors

        public MapXMLBlobManager()
            : base()
        {

        }
        public MapXMLBlobManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion


        #region Get Methods
        public IList<MapXMLBlob> GetMapXMLBlobList()
        {
            return GetAll();
        }

        #endregion




    }
}
