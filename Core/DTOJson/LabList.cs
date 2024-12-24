using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class Labs
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string sort_order { get; set; }
    }
    public class Lablist
    {
        public Lablist()
        {
            Lab = new List<Labs>();
        }
        public List<Labs> Lab { get; set; }
    }
}
