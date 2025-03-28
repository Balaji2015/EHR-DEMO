using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.IO;
using log4net.Config;
using System.Threading;
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public class ApplicationObject
    {
       // public static ErrorHandler erroHandler = new ErrorHandler();
        public static IList<FacilityLibrary> facilityLibraryList = null;
        public static IList<ProcessMaster> processMasterList = null;
        public static IList<Element> elementList = null;
        public static IList<Client> ClientList = null;

        public static IList<ScnTab> scntab = null;

        public static IList<MapXMLBlob> ilstMapXMLBlob = null;
        public static XsltTransformSplitupList XsltTransformSplitupList = null;
    }
}
