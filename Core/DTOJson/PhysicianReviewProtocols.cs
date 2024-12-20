using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class PhyAsstSpecificProtocolsList
    {
        public PhyAsstSpecificProtocolsList()
        {
            PhyAsstSpecificProtocol = new List<PhyAsstSpecificProtocols>();
        }
        public List<PhyAsstSpecificProtocols> PhyAsstSpecificProtocol { get; set; }
    }
    

    public class Physician
    {
        public string id { get; set; }
        public string value { get; set; }
        public string rule { get; set; }
    }
    public class PhyAsstSpecificProtocols
    {
        public Physician Physician { get; set; }
        public string Physician_Assiatant_id { get; set; }
        public string Physician_Assiatant_value { get; set; }
    }

}
