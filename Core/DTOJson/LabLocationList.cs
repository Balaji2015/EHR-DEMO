using System.Collections.Generic;

namespace Acurus.Capella.Core.DTOJson
{
    public class LabLocations
    {
        public string id { get; set; }
        public string locationname { get; set; }
        public string labid { get; set; }
        public string city { get; set; }
        public string fax_no { get; set; }
    }

    public class LabLocationList
    {
        public LabLocationList()
        {
            LabLocation = new List<LabLocations>();
        }
        public List<LabLocations> LabLocation { get; set; }
    }
}
