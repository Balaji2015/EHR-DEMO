using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class IndexingExceptionLog : BusinessBase<ulong>
    {
        private string _File_Name = string.Empty;
        private ulong _Scan_ID = 0;
        private string _Reason_Code = string.Empty;
        private string _Reason_Description = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time;
        private int _Version = 0;
        private int _No_of_Pages = 0;
        private string _Patient_MRN = string.Empty;
        private string _Patient_First_Name = string.Empty;
        private string _Patient_Last_Name = string.Empty;
        private string _Patient_MI_Name = string.Empty;
        private string _Patient_DOB = string.Empty;
        private string _Document_Type = string.Empty;
        private string _Document_Sub_Type = string.Empty;
        private string _Document_Date = string.Empty;
        private string _Order_Number = string.Empty;
        private string _Order_Date = string.Empty;
        private string _Encounter_Number = string.Empty;
        private string _Provider_ID = string.Empty;
        private string _Is_Active = "Y";

        #region HashCode Value
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(_File_Name);
            sb.Append(_Scan_ID);
            sb.Append(_Reason_Code);
            sb.Append(_Reason_Description);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Version);
            sb.Append(_No_of_Pages);
            sb.Append(_Patient_MRN);
            sb.Append(_Patient_First_Name);
            sb.Append(_Patient_Last_Name);
            sb.Append(_Patient_MI_Name);
            sb.Append(_Patient_DOB);
            sb.Append(_Document_Type);
            sb.Append(_Document_Sub_Type);
            sb.Append(_Document_Date);
            sb.Append(_Order_Number);
            sb.Append(_Order_Date);
            sb.Append(_Encounter_Number);
            sb.Append(_Provider_ID);
            sb.Append(_Is_Active);
            return sb.ToString().GetHashCode();
        }
        #endregion

        [DataMember]
        public virtual string File_Name
        {
            get { return _File_Name; }
            set
            {
                _File_Name = value;
            }
        }
        [DataMember]
        public virtual ulong Scan_ID
        {
            get { return _Scan_ID; }
            set
            {
                _Scan_ID = value;
            }
        }
        [DataMember]
        public virtual string Reason_Code
        {
            get { return _Reason_Code; }
            set
            {
                _Reason_Code = value;
            }
        }
        [DataMember]
        public virtual string Reason_Description
        {
            get { return _Reason_Description; }
            set
            {
                _Reason_Description = value;
            }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _Created_By; }
            set
            {
                _Created_By = value;
            }
        }
        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _Created_Date_And_Time; }
            set
            {
                _Created_Date_And_Time = value;
            }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set
            {
                _Modified_By = value;
            }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set
            {
                _Modified_Date_And_Time = value;
            }
        }
        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        [DataMember]
        public virtual int No_of_Pages
        {
            get { return _No_of_Pages; }
            set
            {
                _No_of_Pages = value;
            }
        }
        [DataMember]
        public virtual string Patient_MRN
        {
            get { return _Patient_MRN; }
            set
            {
                _Patient_MRN = value;
            }
        }
        [DataMember]
        public virtual string Patient_First_Name
        {
            get { return _Patient_First_Name; }
            set
            {
                _Patient_First_Name = value;
            }
        }
        [DataMember]
        public virtual string Patient_Last_Name
        {
            get { return _Patient_Last_Name; }
            set
            {
                _Patient_Last_Name = value;
            }
        }
        [DataMember]
        public virtual string Patient_MI_Name
        {
            get { return _Patient_MI_Name; }
            set
            {
                _Patient_MI_Name = value;
            }
        }
        [DataMember]
        public virtual string Patient_DOB
        {
            get { return _Patient_DOB; }
            set
            {
                _Patient_DOB = value;
            }
        }
        [DataMember]
        public virtual string Document_Type
        {
            get { return _Document_Type; }
            set
            {
                _Document_Type = value;
            }
        }
        [DataMember]
        public virtual string Document_Sub_Type
        {
            get { return _Document_Sub_Type; }
            set { _Document_Sub_Type = value; }
        }
        [DataMember]
        public virtual string Document_Date
        {
            get { return _Document_Date; }
            set { _Document_Date = value; }
        }
        [DataMember]
        public virtual string Order_Number
        {
            get { return _Order_Number; }
            set { _Order_Number = value; }
        }
        [DataMember]
        public virtual string Order_Date
        {
            get { return _Order_Date; }
            set { _Order_Date = value; }
        }
        [DataMember]
        public virtual string Encounter_Number
        {
            get { return _Encounter_Number; }
            set { _Encounter_Number = value; }
        }
        [DataMember]
        public virtual string Provider_ID
        {
            get { return _Provider_ID; }
            set { _Provider_ID = value; }
        }
        [DataMember]
        public virtual string Is_Active
        {
            get { return _Is_Active; }
            set { _Is_Active = value; }
        }
    }
}