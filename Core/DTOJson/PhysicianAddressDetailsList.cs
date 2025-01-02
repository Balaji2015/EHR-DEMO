using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class PhysicianAddress
    {
        public string Physician_Address1 { get; set; }
        public string Physician_Address2 { get; set; }
        public string Physician_City { get; set; }
        public string Physician_State { get; set; }
        public string Physician_Zip { get; set; }
        public string Physician_Telephone { get; set; }
        public string Physician_Fax { get; set; }
        public string Specialties { get; set; }
        public string Physician_NPI { get; set; }
        public string Physician_EMail { get; set; }
        public string Physician_prefix { get; set; }
        public string Physician_First_Name { get; set; }
        public string Physician_Last_Name { get; set; }
        public string Physician_Type { get; set; }
        public string Physician_Library_ID { get; set; }
        public string Facility_Name { get; set; }
        public string Company { get; set; }
        public string Category { get; set; }
        public string Physician_Middle_Name { get; set; }
        public string Physician_Suffix { get; set; }
    }

    public class PhysicianAddressDetailsList
    {
        public PhysicianAddressDetailsList()
        {
            PhysicianAddress = new List<PhysicianAddress>();
        }
        public List<PhysicianAddress> PhysicianAddress { get; set; }
    }


}
