using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class PhysicianFacility
    {
        public List<PhysicianList> Physician { get; set; }
        public string name { get; set; }
        public string defaultphysicianid { get; set; }
        public string Legal_Org { get; set; }
    }

    public class PhysicianList
    {
        public string prefix { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string suffix { get; set; }
        public string username { get; set; }
        public string ID { get; set; }
        public string status { get; set; }
        public string npi { get; set; }
        public string machine_technician_id { get; set; }
        public string Legal_Org { get; set; }
    }

    public class PhysicianAssistant
    {
        public string ID { get; set; }
        public string Physician_ID { get; set; }
        public string Default_Physician { get; set; }
        public string Facility_Name { get; set; }
    }

    public class UnmappedPhysician
    {
        public string prefix { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string suffix { get; set; }
        public string username { get; set; }
        public string ID { get; set; }
        public string status { get; set; }
        public string npi { get; set; }
        public string machine_technician_id { get; set; }
        public string Legal_Org { get; set; }
    }


    public class PhysicianFacilityMappingList
    {
        public PhysicianFacilityMappingList()
        {
            PhysicianFacility = new List<PhysicianFacility>();
            UnmappedPhysician = new List<UnmappedPhysician>();
            PhysicianAssistant = new List<PhysicianAssistant>();
        }

        public List<PhysicianFacility> PhysicianFacility { get; set; }
        public List<UnmappedPhysician> UnmappedPhysician { get; set; }
        public List<PhysicianAssistant> PhysicianAssistant { get; set; }
    }


}
