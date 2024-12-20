using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class Physician_POVList
    {
        public Physician_POVList()
        {
            Physician_POV = new List<Physician_POV>();
        }
        public List<Physician_POV> Physician_POV { get; set; }
    }
    public class Physician_POV
    {
        public string Physician_Library_ID { get; set; }
        public string Purpose_of_Visit { get; set; }
        public string Duration { get; set; }
        public string Sort_Order { get; set; }
        public string Description { get; set; }
        public string Default_Value { get; set; }
        public string Legal_Org { get; set; }
    }
}
