using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class PatientNotes : BusinessBase<ulong>
    {

        #region Declarations
        private ulong _Human_ID = 0;
        private string _Assigned_To = string.Empty;
        private string _Relationship = string.Empty;
        private string _Facility_Name = string.Empty;
        private string _Caller_Name = string.Empty;
        private string _Message_Orign = string.Empty;
        private DateTime _Message_Date_And_Time = DateTime.MinValue;
        private string _Message_Description = string.Empty;
        private string _Notes = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Created_By = string.Empty;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private string _Is_PatientChart = string.Empty;
        private int _Version = 0;
        private string _Type = string.Empty;
        private string _Source = string.Empty;
        private int _SourceID = 0;
        private int _CPT = 0;
        //private string _PatientName = string.Empty;
        //private DateTime _DOB = DateTime.MinValue;
        private string _Priority = string.Empty;
        private string _Is_PopupEnable = string.Empty;
        private ulong _Line_ID = 0;
        private int _Encounter_ID = 0;
        //private DateTime _Message_Date = DateTime.MinValue;
        private int _Statement_ChargeLine_ID = 0;
        private string _IsDelete = string.Empty;
        private string _Header_Type = string.Empty;
        private ulong _Header_ID = 0;
        private string _Line_Type = string.Empty;
        private string _Message_Code = string.Empty;
        private string _Claim_Number = string.Empty;
        private string _Carrier_ID = string.Empty;
        private int _Batch_ID = 0;
        private string _Is_Patient_Message = string.Empty;
        private int _Orders_Submit_ID = 0;
        #endregion
        #region Constructors

        public PatientNotes() { }

        #endregion
        #region HashCode Value

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Human_ID);
            sb.Append(_Assigned_To);
            sb.Append(_Relationship);
            sb.Append(_Facility_Name);
            sb.Append(_Caller_Name);
            sb.Append(_Message_Orign);
            sb.Append(_Message_Date_And_Time);
            sb.Append(_Message_Description);
            sb.Append(_Notes);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Created_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Version);
            sb.Append(_Is_PatientChart);
            sb.Append(_Type);
            sb.Append(_Source);
            sb.Append(_SourceID);

            sb.Append(_Encounter_ID);
            sb.Append(_Statement_ChargeLine_ID);
            sb.Append(_IsDelete);


            sb.Append(_Header_Type);
            sb.Append(_Header_ID);
            sb.Append(_Line_Type);
            sb.Append(_Line_ID);

            sb.Append(_Message_Code);
            sb.Append(_Carrier_ID);
            sb.Append(_Batch_ID);
            sb.Append(_Claim_Number);
            sb.Append(_Is_Patient_Message);
            sb.Append(_Orders_Submit_ID);
            return sb.ToString().GetHashCode();
        }
        #endregion
        #region Properties



        [DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set
            {
                _Human_ID = value;
            }
        }
        [DataMember]
        public virtual ulong Line_ID
        {
            get { return _Line_ID; }
            set
            {
                _Line_ID = value;
            }
        }
        [DataMember]
        public virtual int Statement_ChargeLine_ID
        {
            get { return _Statement_ChargeLine_ID; }
            set
            {
                _Statement_ChargeLine_ID = value;
            }
        }
        [DataMember]
        public virtual int Encounter_ID
        {
            get { return _Encounter_ID; }
            set
            {
                _Encounter_ID = value;
            }
        }
        [DataMember]
        public virtual string Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
            }
        }
        [DataMember]
        public virtual string IsDelete
        {
            get { return _IsDelete; }
            set
            {
                _IsDelete = value;
            }
        }
        [DataMember]
        public virtual int CPT
        {
            get { return _CPT; }
            set
            {
                _CPT = value;
            }
        }
        [DataMember]
        public virtual string Is_PopupEnable
        {
            get { return _Is_PopupEnable; }
            set
            {
                _Is_PopupEnable = value;
            }
        }
        [DataMember]
        public virtual string Assigned_To
        {
            get { return _Assigned_To; }
            set
            {
                _Assigned_To = value;
            }
        }
        [DataMember]
        public virtual string Relationship
        {
            get { return _Relationship; }
            set
            {
                _Relationship = value;
            }
        }
        [DataMember]
        public virtual string Facility_Name
        {
            get { return _Facility_Name; }
            set
            {
                _Facility_Name = value;
            }
        }
        [DataMember]
        public virtual string Caller_Name
        {
            get { return _Caller_Name; }
            set
            {
                _Caller_Name = value;
            }
        }
        [DataMember]
        public virtual string Message_Orign
        {
            get { return _Message_Orign; }
            set
            {
                _Message_Orign = value;
            }
        }
        [DataMember]
        public virtual DateTime Message_Date_And_Time
        {
            get { return _Message_Date_And_Time; }
            set
            {
                _Message_Date_And_Time = value;
            }
        }

        [DataMember]
        public virtual string Message_Description
        {
            get { return _Message_Description; }
            set
            {
                _Message_Description = value;
            }
        }
        [DataMember]
        public virtual string Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
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
        public virtual string Created_By
        {
            get { return _Created_By; }
            set
            {
                _Created_By = value;
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
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set
            {
                _Modified_By = value;
            }
        }
        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set
            {
                _Version = value;
            }
        }
        [DataMember]
        public virtual string Is_PatientChart
        {
            get { return _Is_PatientChart; }
            set
            {
                _Is_PatientChart = value;
            }
        }
        [DataMember]
        public virtual string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
            }
        }
        [DataMember]
        public virtual string Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
            }
        }
        [DataMember]
        public virtual int SourceID
        {
            get { return _SourceID; }
            set
            {
                _SourceID = value;
            }
        }

        [DataMember]
        public virtual string Header_Type
        {
            get { return _Header_Type; }
            set
            {
                _Header_Type = value;
            }
        }

        [DataMember]
        public virtual ulong Header_ID
        {
            get { return _Header_ID; }
            set
            {
                _Header_ID = value;
            }
        }
        [DataMember]
        public virtual string Line_Type
        {
            get { return _Line_Type; }
            set
            {
                _Line_Type = value;
            }
        }
        [DataMember]
        public virtual string Message_Code
        {
            get { return _Message_Code; }
            set
            {
                _Message_Code = value;
            }
        }

        [DataMember]
        public virtual int Batch_ID
        {
            get { return _Batch_ID; }
            set
            {
                _Batch_ID = value;
            }
        }

        [DataMember]
        public virtual string Carrier_ID
        {
            get { return _Carrier_ID; }
            set
            {
                _Carrier_ID = value;
            }
        }
        [DataMember]
        public virtual string Claim_Number
        {
            get { return _Claim_Number; }
            set
            {
                _Claim_Number = value;
            }
        }
        [DataMember]
        public virtual string Is_Patient_Message
        {
            get { return _Is_Patient_Message; }
            set
            {
                _Is_Patient_Message = value;
            }
        }
        [DataMember]
        public virtual int Orders_Submit_ID
        {
            get { return _Orders_Submit_ID; }
            set
            {
                _Orders_Submit_ID = value;
            }
        }
        #endregion

    }
}
