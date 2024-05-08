using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTO
{
    public partial class FillPhysicianLibrary
    {

        #region Declarations

        private string _Category = string.Empty;
        private string _Physician_prefix = string.Empty;
        private string _Physician_First_Name = string.Empty;
        private string _Physician_Middle_Name = string.Empty;
        private string _Physician_Last_Name = string.Empty;
        private string _Physician_Suffix = string.Empty;
        private string _Specialties = string.Empty;
        private string _Physician_NPI = string.Empty;
        private string _Facility_Name = string.Empty;
        private string _Physician_Library_ID = string.Empty;
        private string _Physician_Type = string.Empty;
        private string _Company = string.Empty;
        private string _Physician_Address1 = string.Empty;
        private string _Physician_Address2 = string.Empty;
        private string _Physician_City = string.Empty;
        private string _Physician_State = string.Empty;
        private string _Physician_Zip = string.Empty;
        private string _Physician_Telephone = string.Empty;
        private string _Physician_Fax = string.Empty;
        private string _Physician_EMail = string.Empty;
        #endregion

        #region Constructor

        public FillPhysicianLibrary() { }

        #endregion

        #region Properties

        [DataMember]
        public virtual string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }




        [DataMember]
        public virtual string Physician_prefix
        {
            get { return _Physician_prefix; }
            set { _Physician_prefix = value; }
        }



        [DataMember]
        public virtual string Physician_First_Name
        {
            get { return _Physician_First_Name; }
            set { _Physician_First_Name = value; }
        }

        [DataMember]
        public virtual string Physician_Middle_Name
        {
            get { return _Physician_Middle_Name; }
            set { _Physician_Middle_Name = value; }
        }


        [DataMember]
        public virtual string Physician_Last_Name
        {
            get { return _Physician_Last_Name; }
            set { _Physician_Last_Name = value; }
        }

        [DataMember]
        public virtual string Physician_Suffix
        {
            get { return _Physician_Suffix; }
            set { _Physician_Suffix = value; }
        }
        [DataMember]
        public virtual string Specialties
        {
            get { return _Specialties; }
            set { _Specialties = value; }
        }
        [DataMember]
        public virtual string Physician_NPI
        {
            get { return _Physician_NPI; }
            set { _Physician_NPI = value; }
        }
        [DataMember]
        public virtual string Facility_Name
        {
            get { return _Facility_Name; }
            set { _Facility_Name = value; }
        }
        [DataMember]
        public virtual string Physician_Library_ID
        {
            get { return _Physician_Library_ID; }
            set { _Physician_Library_ID = value; }
        }
        [DataMember]
        public virtual string Physician_Type
        {
            get { return _Physician_Type; }
            set { _Physician_Type = value; }
        }
        [DataMember]
        public virtual string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }
        [DataMember]
        public virtual string Physician_Address1
        {
            get { return _Physician_Address1; }
            set { _Physician_Address1 = value; }
        }
        [DataMember]
        public virtual string Physician_Address2
        {
            get { return _Physician_Address2; }
            set { _Physician_Address2 = value; }
        }
        [DataMember]
        public virtual string Physician_City
        {
            get { return _Physician_City; }
            set { _Physician_City = value; }
        }
        [DataMember]
        public virtual string Physician_State
        {
            get { return _Physician_State; }
            set { _Physician_State = value; }
        }
        [DataMember]
        public virtual string Physician_Zip
        {
            get { return _Physician_Zip; }
            set { _Physician_Zip = value; }
        }
        [DataMember]
        public virtual string Physician_Telephone
        {
            get { return _Physician_Telephone; }
            set { _Physician_Telephone = value; }
        }
        [DataMember]
        public virtual string Physician_Fax
        {
            get { return _Physician_Fax; }
            set { _Physician_Fax = value; }
        }

        [DataMember]
        public virtual string Physician_EMail
        {
            get { return _Physician_EMail; }
            set { _Physician_EMail = value; }
        }

        #endregion
    }
}
