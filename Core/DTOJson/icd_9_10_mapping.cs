

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class icd_9_10_mapping
    {
        public icd_9_10_mapping() {
            Icd_9_10_Mapping = new List<Icd_9_10_Mapping>();
        }
        public List<Icd_9_10_Mapping> Icd_9_10_Mapping { get; set; }

    }
    public class Icd_9_10_Mapping
    {
        public string icd_9 { get; set; }
        public string icd_choices { get; set; }
    }
}