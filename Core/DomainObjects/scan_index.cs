using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial class scan_index : BusinessBase<ulong>
    {

        #region Declarations

        private ulong _Human_ID = 0;
        private ulong _physician_id = 0;
        private ulong _Scan_ID = 0;
        private ulong _Order_ID = 0;
        private DateTime _Document_Date = DateTime.MinValue;
        private string _Document_Type = string.Empty;
        private string _Document_Sub_Type = string.Empty;
        private string _Indexed_File_Path = string.Empty;
        private string _Page_Selected = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.Now;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private Scan _scan_information;
        private string _object_type = string.Empty;
        private string _physician_name = string.Empty;
        private int _Version = 0;
        private string _lab_name = string.Empty;
        private IList<StaticLookup> lookupList = new List<StaticLookup>();
        private ulong _Encounter_id = 0;
        private ulong _Workset_ID = 0;
        private DateTime _Appointment_Date = DateTime.MinValue;
        private string _Facility_Name = string.Empty;
        private ulong _Appointment_Provider_ID = 0;
        private DateTime _Document_To_Date = DateTime.MinValue;
        private string _Is_Narrative_Interpretation = string.Empty;
        private string _Is_Manually_Reviewed_And_Signed ="N";


        #endregion

        #region Constructors

        public scan_index() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Human_ID);
            sb.Append(_Order_ID);
            sb.Append(_Scan_ID);
            sb.Append(_Document_Date);
            sb.Append(_Document_Type);
            sb.Append(_Document_Sub_Type);
            sb.Append(_Indexed_File_Path);
            sb.Append(_Page_Selected);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Version);
            sb.Append(_Workset_ID);
            sb.Append(_Appointment_Date);
            sb.Append(_Facility_Name);
            sb.Append(_Appointment_Provider_ID);
            sb.Append(_Document_To_Date);
            sb.Append(_Is_Narrative_Interpretation);
            sb.Append(_Is_Manually_Reviewed_And_Signed);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties


        [DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set { _Human_ID = value; }
        }
        [DataMember]
        public virtual ulong Order_ID
        {
            get { return _Order_ID; }
            set { _Order_ID = value; }
        }

        [DataMember]
        public virtual ulong Scan_ID
        {
            get { return _Scan_ID; }
            set { _Scan_ID = value; }
        }

        [DataMember]
        public virtual DateTime Document_Date
        {
            get { return _Document_Date; }
            set { _Document_Date = value; }
        }
        [DataMember]
        public virtual string Document_Type
        {
            get { return _Document_Type; }
            set { _Document_Type = value; }

        }
        [DataMember]
        public virtual string Document_Sub_Type
        {
            get { return _Document_Sub_Type; }
            set { _Document_Sub_Type = value; }
        }

        [DataMember]
        public virtual string Indexed_File_Path
        {
            get { return _Indexed_File_Path; }
            set { _Indexed_File_Path = value; }
        }


        [DataMember]
        public virtual string Page_Selected
        {
            get { return _Page_Selected; }
            set { _Page_Selected = value; }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _Created_By; }
            set { _Created_By = value; }
        }
        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _Created_Date_And_Time; }
            set { _Created_Date_And_Time = value; }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set { _Modified_By = value; }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set { _Modified_Date_And_Time = value; }
        }

        [DataMember]
        public virtual Scan Scan_Information
        {
            get { return _scan_information; }
            set { _scan_information = value; }
        }



        [DataMember]
        public virtual string Object_Type
        {
            get { return _object_type; }
            set { _object_type = value; }
        }

        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }


        [DataMember]
        public virtual ulong Physician_ID
        {
            get { return _physician_id; }
            set { _physician_id = value; }
        }


        [DataMember]
        public virtual string Physician_Name
        {
            get { return _physician_name; }
            set { _physician_name = value; }
        }

        [DataMember]
        public virtual string lab_name
        {
            get { return _lab_name; }
            set { _lab_name = value; }
        }

        [DataMember]
        public virtual IList<StaticLookup> LookUpList
        {
            get { return lookupList; }
            set { lookupList = value; }
        }

        [DataMember]
        public virtual ulong Encounter_ID
        {
            get { return _Encounter_id; }
            set { _Encounter_id = value; }
        }
        [DataMember]
        public virtual ulong Workset_ID
        {
            get { return _Workset_ID; }
            set { _Workset_ID = value; }
        }

        [DataMember]
        public virtual DateTime Appointment_Date
        {
            get { return _Appointment_Date; }
            set { _Appointment_Date = value; }
        }
        [DataMember]
        public virtual string Facility_Name
        {
            get { return _Facility_Name; }
            set { _Facility_Name = value; }
        }
        [DataMember]
        public virtual ulong Appointment_Provider_ID
        {
            get { return _Appointment_Provider_ID; }
            set { _Appointment_Provider_ID = value; }
        }
        [DataMember]
        public virtual DateTime Document_To_Date
        {
            get { return _Document_To_Date; }
            set { _Document_To_Date = value; }
        }

        [DataMember]
        public virtual string Is_Narrative_Interpretation
        {
            get { return _Is_Narrative_Interpretation; }
            set { _Is_Narrative_Interpretation = value; }
        }
        [DataMember]
        public virtual string Is_Manually_Reviewed_And_Signed
        {
            get { return _Is_Manually_Reviewed_And_Signed; }
            set { _Is_Manually_Reviewed_And_Signed = value; }
        }


        #endregion
    }
}
