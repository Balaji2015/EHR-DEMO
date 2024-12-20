using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class AssessmentVitalsLookupList
    {
        public AssessmentVitalsLookupList()
        {
            Assessment_vitals_lookup = new List<Assessment_vitals_lookup>();
        }
        public List<Assessment_vitals_lookup> Assessment_vitals_lookup { get; set; }
    }


    public class Assessment_vitals_lookup
    {

        public string Id { get; set; }
        public string Field_Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string ICD { get; set; }
        public string Sort_Order { get; set; }
        public string ICD_10 { get; set; }
        public string ICD_10_Description { get; set; }
        public string Entity_Name { get; set; }
        public string Hirarrchy { get; set; }
        public string Is_Current_Encounter { get; set; }
        public string Is_Mutually_Exclusive { get; set; }
        public string Is_Macra_Field { get; set; }
    }
}
