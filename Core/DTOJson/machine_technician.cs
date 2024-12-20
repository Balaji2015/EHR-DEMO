using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{

    public class MachinetechnicianList
    {
        public MachinetechnicianList()
        {
            MachineTechnician = new List<Machinetechnician>();
        }
        public List<Machinetechnician> MachineTechnician { get; set; }
    }

    public class Machinetechnician
    {
        public string machine_technician_library_id { get; set; }
        public string machine_name { get; set; }
        public string Physician_Library_ID { get; set; }
        public string status { get; set; }
        public string Is_General_Queue_Appoinment { get; set; }
    }

}
