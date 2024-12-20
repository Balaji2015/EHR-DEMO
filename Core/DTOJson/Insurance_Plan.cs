using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class Insurance_PlanList
    {
        public Insurance_PlanList()
        {
            InsurancePlan = new List<Insurance_Plan>();
        }
        public List<Insurance_Plan> InsurancePlan { get; set; }
    }


    public class Insurance_Plan
    {
        public string insurance_plan_id { get; set; }
        public string insurance_plan_name { get; set; }
        public string carrier_id { get; set; }
        public string active { get; set; }
    }
}