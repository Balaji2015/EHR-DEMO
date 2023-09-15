using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class OrdersSubmit : BusinessBase<ulong>
    {
        #region Declarations
        private ulong _Physician_ID = 0;
        private ulong _Encounter_ID = 0;
        private ulong _Human_ID = 0;
        private string _Order_Code_Type = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private int _Version = 0;
        private DateTime _Internal_Property_Signature_Date_And_Time = DateTime.MinValue;
        private string _Internal_Property_Temperature_state = string.Empty;
        private string _Internal_Property_Order_Code = string.Empty;
        private ulong _Lab_Location_ID = 0;
        private ulong _Lab_ID = 0;
        private string _Authorization_Required = string.Empty;
        private string _Order_Notes = string.Empty;
        private string _Specimen_In_House = string.Empty;
        private string _Order_Type = string.Empty;
        private string _FacName = string.Empty;
        private string _Height = string.Empty;
        private string _Weight = string.Empty;
        private string _Bill_Type = string.Empty;
        private string _Is_ABN_Signed = string.Empty;
        private string _Temperature = string.Empty;
        private string _Move_To_MA = string.Empty;
        private string _Lab_Location_Name = string.Empty;
        private string _Lab_Name = string.Empty;
        private int _TestDate_In_Months = 0;
        private int _TestDate_In_Days = 0;
        private int _TestDate_In_Weeks = 0;
        private string _Stat = "N";
        private string _Specimenn_Type = string.Empty;
        private int _Quantity = 0;
        private string _Specimen_Unit = string.Empty;
        private string _Culture_Location = string.Empty;
        private string _Fasting = string.Empty;
        private DateTime _Specimen_Collection_Date_And_Time = DateTime.MinValue;
        private string _Is_Submit_Immediately = string.Empty;

        private string _Internal_Property_FilePath = string.Empty;
        private string _TestDate = string.Empty;

        private ulong _Prefered_Reading_Provider_ID = 0;
        private string _Internal_Property_Update_Phone_Number = string.Empty;
        private DateTime _Internal_Property_EncounterDate = DateTime.MinValue;
        private string _Is_Paper_Order = string.Empty;
        private string _Is_Deleted = "N";
        private DateTime _Date_Last_Seen = DateTime.MinValue;
        private int _Duration_for_DME_Need_in_Months = 0;
        private int _Duration_for_Supplies_Need_in_Months = 0;
        private string _Is_Urgent = "N";
        private string _Is_Task_Created = "N";
        #endregion

        #region Constructors

        public OrdersSubmit() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Encounter_ID);
            sb.Append(_Human_ID);
            sb.Append(_Order_Code_Type);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Version);
            sb.Append(_Internal_Property_Signature_Date_And_Time);
            sb.Append(_Internal_Property_Temperature_state);
            sb.Append(_Internal_Property_Order_Code);
            sb.Append(_Lab_Location_ID);
            sb.Append(_Lab_ID);
            sb.Append(_Authorization_Required);
            sb.Append(_Order_Notes);
            sb.Append(_Specimen_In_House);
            sb.Append(_Order_Type);
            sb.Append(_FacName);
            sb.Append(_Height);
            sb.Append(_Weight);
            sb.Append(_Is_ABN_Signed);
            sb.Append(_Specimen_Collection_Date_And_Time);
            sb.Append(_Temperature);
            sb.Append(_Move_To_MA);
            sb.Append(_TestDate);
            sb.Append(_Physician_ID);
            sb.Append(_Lab_Location_Name);
            sb.Append(_Lab_Name);
            sb.Append(_TestDate_In_Months);
            sb.Append(_TestDate_In_Days);
            sb.Append(_TestDate_In_Weeks);
            sb.Append(_Stat);
            sb.Append(_Fasting);
            sb.Append(_Culture_Location);
            sb.Append(_Specimen_Unit);
            sb.Append(_Quantity);
            sb.Append(_Specimenn_Type);
            sb.Append(_Is_Submit_Immediately);
            sb.Append(_Prefered_Reading_Provider_ID);
            sb.Append(_Internal_Property_Update_Phone_Number);
            sb.Append(_Internal_Property_EncounterDate);
            sb.Append(_Is_Paper_Order);
            sb.Append(_Is_Deleted);
            sb.Append(_Date_Last_Seen);
            sb.Append(_Duration_for_DME_Need_in_Months);
            sb.Append(_Duration_for_Supplies_Need_in_Months);
            sb.Append(_Is_Urgent);
            sb.Append(_Is_Task_Created);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties
        [DataMember]
        public virtual string Stat
        {
            get { return _Stat; }
            set { _Stat = value; }
        }
        [DataMember]
        public virtual int TestDate_In_Months
        {
            get { return _TestDate_In_Months; }
            set { _TestDate_In_Months = value; }
        }
        [DataMember]
        public virtual int TestDate_In_Days
        {
            get { return _TestDate_In_Days; }
            set { _TestDate_In_Days = value; }
        }
        [DataMember]
        public virtual int TestDate_In_Weeks
        {
            get { return _TestDate_In_Weeks; }
            set { _TestDate_In_Weeks = value; }
        }

        [DataMember]
        public virtual string Lab_Location_Name
        {
            get { return _Lab_Location_Name; }
            set { _Lab_Location_Name = value; }
        }
        [DataMember]
        public virtual string Lab_Name
        {
            get { return _Lab_Name; }
            set { _Lab_Name = value; }
        }

        [DataMember]
        public virtual ulong Physician_ID
        {
            get { return _Physician_ID; }
            set { _Physician_ID = value; }
        }
        [DataMember]
        public virtual ulong Encounter_ID
        {
            get { return _Encounter_ID; }
            set { _Encounter_ID = value; }
        }
        [DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set { _Human_ID = value; }
        }


        [DataMember]
        public virtual string Created_By
        {
            get { return _Created_By; }
            set { _Created_By = value; }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set { _Modified_By = value; }
        }

        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _Created_Date_And_Time; }
            set { _Created_Date_And_Time = value; }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set { _Modified_Date_And_Time = value; }
        }

        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        [DataMember]
        public virtual string Order_Code_Type
        {
            get { return _Order_Code_Type; }
            set { _Order_Code_Type = value; }
        }

        [DataMember]
        public virtual DateTime Internal_Property_Signature_Date_And_Time
        {
            get { return _Internal_Property_Signature_Date_And_Time; }
            set { _Internal_Property_Signature_Date_And_Time = value; }
        }
        [DataMember]
        public virtual string Internal_Property_Temperature_state
        {
            get { return _Internal_Property_Temperature_state; }
            set { _Internal_Property_Temperature_state = value; }
        }
        [DataMember]
        public virtual string Internal_Property_Order_Code
        {
            get { return _Internal_Property_Order_Code; }
            set { _Internal_Property_Order_Code = value; }
        }
        [DataMember]
        public virtual ulong Lab_Location_ID
        {
            get { return _Lab_Location_ID; }
            set { _Lab_Location_ID = value; }
        }

        [DataMember]
        public virtual ulong Lab_ID
        {
            get { return _Lab_ID; }
            set { _Lab_ID = value; }
        }
        [DataMember]
        public virtual string Authorization_Required
        {
            get { return _Authorization_Required; }
            set { _Authorization_Required = value; }
        }

        [DataMember]
        public virtual string Order_Notes
        {
            get { return _Order_Notes; }
            set { _Order_Notes = value; }
        }
        [DataMember]
        public virtual string Specimen_In_House
        {
            get { return _Specimen_In_House; }
            set { _Specimen_In_House = value; }
        }
        [DataMember]
        public virtual string Order_Type
        {
            get { return _Order_Type; }
            set { _Order_Type = value; }
        }
        [DataMember]
        public virtual string Facility_Name
        {
            get { return _FacName; }
            set
            {
                _FacName = value;
            }
        }
        [DataMember]
        public virtual string Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        [DataMember]
        public virtual string Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
        [DataMember]
        public virtual string Bill_Type
        {
            get { return _Bill_Type; }
            set { _Bill_Type = value; }
        }

        [DataMember]
        public virtual string Is_ABN_Signed
        {
            get { return _Is_ABN_Signed; }
            set { _Is_ABN_Signed = value; }
        }
        [DataMember]
        public virtual string Move_To_MA
        {
            get { return _Move_To_MA; }
            set { _Move_To_MA = value; }
        }
        [DataMember]
        public virtual DateTime Specimen_Collection_Date_And_Time
        {
            get { return _Specimen_Collection_Date_And_Time; }
            set { _Specimen_Collection_Date_And_Time = value; }
        }
        [DataMember]
        public virtual string Temperature
        {
            get { return _Temperature; }
            set { _Temperature = value; }
        }
        [DataMember]
        public virtual string Test_Date
        {
            get { return _TestDate; }
            set { _TestDate = value; }
        }
        [DataMember]
        public virtual string Specimen_Type
        {
            get { return _Specimenn_Type; }
            set { _Specimenn_Type = value; }
        }
        [DataMember]
        public virtual string Specimen_Unit
        {
            get { return _Specimen_Unit; }
            set { _Specimen_Unit = value; }
        }
        [DataMember]
        public virtual string Culture_Location
        {
            get { return _Culture_Location; }
            set { _Culture_Location = value; }
        }
        [DataMember]
        public virtual string Fasting
        {
            get { return _Fasting; }
            set { _Fasting = value; }
        }
        [DataMember]
        public virtual int Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }
        [DataMember]
        public virtual string Is_Submit_Immediately
        {
            get { return _Is_Submit_Immediately; }
            set { _Is_Submit_Immediately = value; }
        }

        [DataMember]
        public virtual string Internal_Property_File_Path
        {
            get { return _Internal_Property_FilePath ; }
            set { _Internal_Property_FilePath = value; }
        }
        [DataMember]
        public virtual ulong Prefered_Reading_Provider_ID
        {
            get { return _Prefered_Reading_Provider_ID; }
            set { _Prefered_Reading_Provider_ID = value; }
        }
        [DataMember]
        public virtual string Internal_Property_Update_Phone_Number
        {
            get { return _Internal_Property_Update_Phone_Number; }
            set { _Internal_Property_Update_Phone_Number = value; }
        }
        [DataMember]
        public virtual DateTime Internal_Property_EncounterDate
        {
            get { return _Internal_Property_EncounterDate; }
            set { _Internal_Property_EncounterDate = value; }
        }

        [DataMember]
        public virtual string Is_Paper_Order
        {
            get { return _Is_Paper_Order; }
            set { _Is_Paper_Order = value; }
        }

        [DataMember]
        public virtual string Is_Deleted
        {
            get { return _Is_Deleted; }
            set { _Is_Deleted = value; }
        }
        [DataMember]
        public virtual DateTime Date_Last_Seen
        {
            get { return _Date_Last_Seen; }
            set { _Date_Last_Seen = value; }
        }

        [DataMember]
        public virtual int Duration_for_DME_Need_in_Months
        {
            get { return _Duration_for_DME_Need_in_Months; }
            set { _Duration_for_DME_Need_in_Months = value; }
        }

        [DataMember]
        public virtual int Duration_for_Supplies_Need_in_Months
        {
            get { return _Duration_for_Supplies_Need_in_Months; }
            set { _Duration_for_Supplies_Need_in_Months = value; }
        }
        [DataMember]
        public virtual string Is_Urgent
        {
            get { return _Is_Urgent; }
            set { _Is_Urgent = value; }
        }
        [DataMember]
        public virtual string Is_Task_Created
        {
            get { return _Is_Task_Created; }
            set { _Is_Task_Created = value; }
        }
        #endregion

    }
}
