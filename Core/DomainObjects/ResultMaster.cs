using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{

    [Serializable]
    [DataContract]

    public partial class ResultMaster : BusinessBase<ulong>
    {
        #region Declarations

        private int _Version = 0;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private DateTime _Result_Received_Date = DateTime.MinValue;
        private ulong _Order_ID = 0;
        private string _MSH_Segment_Type_ID = string.Empty;
        private string _MSH_Delimiter = string.Empty;
        private string _MSH_Sending_Application = string.Empty;
        private string _MSH_Sending_Facility = string.Empty;
        private string _MSH_Receiving_Application = string.Empty;
        private string _MSH_Receiveing_Facility = string.Empty;
        private string _MSH_Date_And_Time_Of_Message = string.Empty;
        private string _OBR_Specimen_Collected_Date_And_Time = string.Empty;
        private string _MSH_Security = string.Empty;
        private string _MSH_Message_Type = string.Empty;
        private string _MSH_Message_Type_Inner_Segment = string.Empty;
        private string _MSH_Message_Control_ID = string.Empty;
        private string _MSH_Processing_ID = string.Empty;
        private string _MSH_HL7_Version = string.Empty;
        private string _PID_Segment_Type_ID = string.Empty;
        private string _PID_Sequence_Number = string.Empty;
        private string _PID_External_Patient_ID = string.Empty;
        private string _PID_Lab_Assigned_Patient_ID = string.Empty;
        private string _PID_Alternate_Patient_ID = string.Empty;
        private string _PID_Patient_Last_Name = string.Empty;
        private string _PID_Patient_First_Name = string.Empty;
        private string _PID_Patient_Middle_Name = string.Empty;
        private string _PID_Mother_Maiden_Name = string.Empty;
        private string _PID_Patient_Date_Of_Birth = string.Empty;
        private string _PID_Patient_Gender = string.Empty;
        private string _PID_Patient_Alias = string.Empty;
        private string _PID_Patient_Race = string.Empty;
        private string _PID_Patient_Address1 = string.Empty;
        private string _PID_Patient_Address2 = string.Empty;
        private string _PID_Patient_City = string.Empty;
        private string _PID_Patient_State = string.Empty;
        private string _PID_Patient_Zip = string.Empty;
        private string _PID_Patient_Country_Code = string.Empty;
        private string _PID_Patient_Home_Phone = string.Empty;
        private string _PID_Patient_Work_Phone = string.Empty;
        private string _PID_Patient_Language = string.Empty;
        private string _PID_Patient_Marital_Status = string.Empty;
        private string _PID_Patient_Religion = string.Empty;
        private string _PID_Labcorp_Customer_ID = string.Empty;
        private string _PID_Check_Digit = string.Empty;
        private string _PID_Check_Digit_Scheme = string.Empty;
        private string _PID_Bill_Code = string.Empty;
        private string _PID_ABN_Flag = string.Empty;
        private string _PID_Status_Of_Specimen = string.Empty;
        private string _PID_Fasting = string.Empty;
        private string _PID_Patient_SSN = string.Empty;
        private string _ORC_Segment_Type_ID = string.Empty;
        private string _ORC_Order_Control = string.Empty;
        private string _ORC_Specimen_ID = string.Empty;
        private string _ORC_Instituition_ID = string.Empty;
        private string _ORC_Filler_Accession_ID = string.Empty;
        private string _ORC_Owner_Of_Accession = string.Empty;
        private string _ORC_Placer_Group_Number = string.Empty;
        private string _ORC_Order_Status = string.Empty;
        private string _ORC_Response_Flag = string.Empty;
        private string _ORC_Quantity = string.Empty;
        private string _ORC_Parent = string.Empty;
        private string _ORC_Date_And_Time_Of_Transaction = string.Empty;
        private string _ORC_Entered_By = string.Empty;
        private string _ORC_Verified_By = string.Empty;
        private string _ORC_Ordering_Provider_ID = string.Empty;
        private string _ORC_Ordering_Provider_Last_Name = string.Empty;
        private string _ORC_Ordering_Provider_First_Initial = string.Empty;
        private string _ORC_Ordering_Provider_Middle_Initial = string.Empty;
        private string _ORC_Ordering_Provider_Suffix = string.Empty;
        private string _ORC_Ordering_Provider_Prefix = string.Empty;
        private string _ORC_Ordering_Provider_Degree = string.Empty;
        private string _ORC_Source_Table = string.Empty;
        private string _Result_Review_Comments = string.Empty;
        private string _Result_Review_Date = string.Empty;
        private string _Result_Review_By = string.Empty;
        private string _temp_property = string.Empty;
        private string _Is_Electronic_Mode = string.Empty;
        private int _Reviewed_Provider_Sign_ID = 0;
        private ulong _Lab_ID = 0;
        private string _Is_Filled = string.Empty;
        //vince  Dec-29-2012 for patient chart////// 
        private string _Order_type = string.Empty;
        private string _Is_Scan_order = string.Empty;
        private string _Orders_Description = string.Empty;
        private string _MA_Notes = string.Empty;
        private string _File_Name = string.Empty;
        private ulong _Matching_Patient_Id = 0;
        private ulong _Matching_Physician_Id = 0;
        //vince  Dec-29-2012 for patient chart//////
        #endregion

        #region Constructors

        public ResultMaster() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Version);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Result_Received_Date);
            sb.Append(_Order_ID);
            sb.Append(_MSH_Segment_Type_ID);
            sb.Append(_MSH_Delimiter);
            sb.Append(_MSH_Sending_Application);
            sb.Append(_MSH_Sending_Facility);
            sb.Append(_MSH_Receiving_Application);
            sb.Append(_MSH_Receiveing_Facility);
            sb.Append(_MSH_Date_And_Time_Of_Message);
            sb.Append(_OBR_Specimen_Collected_Date_And_Time);
            sb.Append(_MSH_Security);
            sb.Append(_MSH_Message_Type);
            sb.Append(_MSH_Message_Control_ID);
            sb.Append(_MSH_Processing_ID);
            sb.Append(_MSH_HL7_Version);
            sb.Append(_PID_Segment_Type_ID);
            sb.Append(_PID_Sequence_Number);
            sb.Append(_PID_External_Patient_ID);
            sb.Append(_PID_Lab_Assigned_Patient_ID);
            sb.Append(_PID_Alternate_Patient_ID);
            sb.Append(_PID_Patient_Last_Name);
            sb.Append(_PID_Patient_First_Name);
            sb.Append(_PID_Patient_Middle_Name);
            sb.Append(_PID_Mother_Maiden_Name);
            sb.Append(_PID_Patient_Date_Of_Birth);
            sb.Append(_PID_Patient_Gender);
            sb.Append(_PID_Patient_Alias);
            sb.Append(_PID_Patient_Race);
            sb.Append(_PID_Patient_Address1);
            sb.Append(_PID_Patient_Address2);
            sb.Append(_PID_Patient_City);
            sb.Append(_PID_Patient_State);
            sb.Append(_PID_Patient_Zip);
            sb.Append(_PID_Patient_Country_Code);
            sb.Append(_PID_Patient_Home_Phone);
            sb.Append(_PID_Patient_Work_Phone);
            sb.Append(_PID_Patient_Language);
            sb.Append(_PID_Patient_Marital_Status);
            sb.Append(_PID_Patient_Religion);
            sb.Append(_PID_Labcorp_Customer_ID);
            sb.Append(_PID_Check_Digit);
            sb.Append(_PID_Check_Digit_Scheme);
            sb.Append(_PID_Bill_Code);
            sb.Append(_PID_ABN_Flag);
            sb.Append(_PID_Status_Of_Specimen);
            sb.Append(_PID_Fasting);
            sb.Append(_PID_Patient_SSN);
            sb.Append(_ORC_Segment_Type_ID);
            sb.Append(_ORC_Order_Control);
            sb.Append(_ORC_Specimen_ID);
            sb.Append(_ORC_Instituition_ID);
            sb.Append(_ORC_Filler_Accession_ID);
            sb.Append(_ORC_Owner_Of_Accession);
            sb.Append(_ORC_Placer_Group_Number);
            sb.Append(_ORC_Order_Status);
            sb.Append(_ORC_Response_Flag);
            sb.Append(_ORC_Quantity);
            sb.Append(_ORC_Parent);
            sb.Append(_ORC_Date_And_Time_Of_Transaction);
            sb.Append(_ORC_Entered_By);
            sb.Append(_ORC_Verified_By);
            sb.Append(_ORC_Ordering_Provider_ID);
            sb.Append(_ORC_Ordering_Provider_Last_Name);
            sb.Append(_ORC_Ordering_Provider_First_Initial);
            sb.Append(_ORC_Ordering_Provider_Middle_Initial);
            sb.Append(_ORC_Ordering_Provider_Suffix);
            sb.Append(_ORC_Ordering_Provider_Prefix);
            sb.Append(_ORC_Ordering_Provider_Degree);
            sb.Append(_ORC_Source_Table);
            sb.Append(_Result_Review_Comments);
            sb.Append(_Result_Review_Date);
            sb.Append(_Result_Review_By);
            sb.Append(_MSH_Message_Type_Inner_Segment);
            sb.Append(_Reviewed_Provider_Sign_ID);
            sb.Append(_Lab_ID);
            sb.Append(_Is_Electronic_Mode);
            sb.Append(_Is_Filled);
            sb.Append(_MA_Notes);
            sb.Append(_File_Name);
            sb.Append(_Matching_Patient_Id);
            sb.Append(_Matching_Physician_Id);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties
        [DataMember]
        public virtual int Reviewed_Provider_Sign_ID
        {
            get { return _Reviewed_Provider_Sign_ID; }
            set { _Reviewed_Provider_Sign_ID = value; }
        }
        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set { _Version = value; }
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
        public virtual string Result_Review_Comments
        {
            get { return _Result_Review_Comments; }
            set { _Result_Review_Comments = value; }
        }
        [DataMember]
        public virtual string Result_Review_Date
        {
            get { return _Result_Review_Date; }
            set { _Result_Review_Date = value; }
        }
        [DataMember]
        public virtual string Result_Review_By
        {
            get { return _Result_Review_By; }
            set { _Result_Review_By = value; }
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
        public virtual DateTime Result_Received_Date
        {
            get { return _Result_Received_Date; }
            set { _Result_Received_Date = value; }
        }
        [DataMember]
        public virtual ulong Order_ID
        {
            get { return _Order_ID; }
            set { _Order_ID = value; }
        }
        [DataMember]
        public virtual string MSH_Segment_Type_ID
        {
            get { return _MSH_Segment_Type_ID; }
            set { _MSH_Segment_Type_ID = value; }
        }
        [DataMember]
        public virtual string MSH_Delimiter
        {
            get { return _MSH_Delimiter; }
            set { _MSH_Delimiter = value; }
        }
        [DataMember]
        public virtual string MSH_Sending_Application
        {
            get { return _MSH_Sending_Application; }
            set { _MSH_Sending_Application = value; }
        }
        [DataMember]
        public virtual string MSH_Sending_Facility
        {
            get { return _MSH_Sending_Facility; }
            set { _MSH_Sending_Facility = value; }
        }
        [DataMember]
        public virtual string MSH_Receiving_Application
        {
            get { return _MSH_Receiving_Application; }
            set { _MSH_Receiving_Application = value; }
        }
        [DataMember]
        public virtual string MSH_Receiveing_Facility
        {
            get { return _MSH_Receiveing_Facility; }
            set { _MSH_Receiveing_Facility = value; }
        }
        [DataMember]
        public virtual string MSH_Date_And_Time_Of_Message
        {
            get { return _MSH_Date_And_Time_Of_Message; }
            set { _MSH_Date_And_Time_Of_Message = value; }
        }
         [DataMember]
        public virtual string OBR_Specimen_Collected_Date_And_Time
        {
            get { return _OBR_Specimen_Collected_Date_And_Time; }
            set { _OBR_Specimen_Collected_Date_And_Time = value; }
        }
        [DataMember]
        public virtual string MSH_Security
        {
            get { return _MSH_Security; }
            set { _MSH_Security = value; }
        }

        [DataMember]
        public virtual string MSH_Message_Type
        {
            get { return _MSH_Message_Type; }
            set { _MSH_Message_Type = value; }
        }
        [DataMember]
        public virtual string MSH_Message_Control_ID
        {
            get { return _MSH_Message_Control_ID; }
            set { _MSH_Message_Control_ID = value; }
        }
        [DataMember]
        public virtual string MSH_Processing_ID
        {
            get { return _MSH_Processing_ID; }
            set { _MSH_Processing_ID = value; }
        }
        [DataMember]
        public virtual string MSH_HL7_Version
        {
            get { return _MSH_HL7_Version; }
            set { _MSH_HL7_Version = value; }

        }
        [DataMember]
        public virtual string PID_Segment_Type_ID
        {
            get { return _PID_Segment_Type_ID; }
            set { _PID_Segment_Type_ID = value; }
        }
        [DataMember]
        public virtual string PID_Sequence_Number
        {
            get { return _PID_Sequence_Number; }
            set { _PID_Sequence_Number = value; }
        }
        [DataMember]
        public virtual string PID_External_Patient_ID
        {
            get { return _PID_External_Patient_ID; }
            set { _PID_External_Patient_ID = value; }
        }
        [DataMember]
        public virtual string PID_Lab_Assigned_Patient_ID
        {
            get { return _PID_Lab_Assigned_Patient_ID; }
            set { _PID_Lab_Assigned_Patient_ID = value; }
        }
        [DataMember]
        public virtual string PID_Alternate_Patient_ID
        {
            get { return _PID_Alternate_Patient_ID; }
            set { _PID_Alternate_Patient_ID = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Last_Name
        {
            get { return _PID_Patient_Last_Name; }
            set { _PID_Patient_Last_Name = value; }
        }
        [DataMember]
        public virtual string PID_Patient_First_Name
        {
            get { return _PID_Patient_First_Name; }
            set { _PID_Patient_First_Name = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Middle_Name
        {
            get { return _PID_Patient_Middle_Name; }
            set { _PID_Patient_Middle_Name = value; }
        }
        [DataMember]
        public virtual string PID_Mother_Maiden_Name
        {
            get { return _PID_Mother_Maiden_Name; }
            set { _PID_Mother_Maiden_Name = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Date_Of_Birth
        {
            get { return _PID_Patient_Date_Of_Birth; }
            set { _PID_Patient_Date_Of_Birth = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Gender
        {
            get { return _PID_Patient_Gender; }
            set { _PID_Patient_Gender = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Alias
        {
            get { return _PID_Patient_Alias; }
            set { _PID_Patient_Alias = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Race
        {
            get { return _PID_Patient_Race; }
            set { _PID_Patient_Race = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Address1
        {
            get { return _PID_Patient_Address1; }
            set { _PID_Patient_Address1 = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Address2
        {
            get { return _PID_Patient_Address2; }
            set { _PID_Patient_Address2 = value; }
        }
        [DataMember]
        public virtual string PID_Patient_City
        {
            get { return _PID_Patient_City; }
            set { _PID_Patient_City = value; }
        }
        [DataMember]
        public virtual string PID_Patient_State
        {
            get { return _PID_Patient_State; }
            set { _PID_Patient_State = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Zip
        {
            get { return _PID_Patient_Zip; }
            set { _PID_Patient_Zip = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Country_Code
        {
            get { return _PID_Patient_Country_Code; }
            set { _PID_Patient_Country_Code = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Home_Phone
        {
            get { return _PID_Patient_Home_Phone; }
            set { _PID_Patient_Home_Phone = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Work_Phone
        {
            get { return _PID_Patient_Work_Phone; }
            set { _PID_Patient_Work_Phone = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Language
        {
            get { return _PID_Patient_Language; }
            set { _PID_Patient_Language = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Marital_Status
        {
            get { return _PID_Patient_Marital_Status; }
            set { _PID_Patient_Marital_Status = value; }
        }
        [DataMember]
        public virtual string PID_Patient_Religion
        {
            get { return _PID_Patient_Religion; }
            set { _PID_Patient_Religion = value; }
        }
        [DataMember]
        public virtual string PID_Labcorp_Customer_ID
        {
            get { return _PID_Labcorp_Customer_ID; }
            set { _PID_Labcorp_Customer_ID = value; }
        }
        [DataMember]
        public virtual string PID_Check_Digit
        {
            get { return _PID_Check_Digit; }
            set { _PID_Check_Digit = value; }
        }
        [DataMember]
        public virtual string PID_Check_Digit_Scheme
        {
            get { return _PID_Check_Digit_Scheme; }
            set { _PID_Check_Digit_Scheme = value; }
        }
        [DataMember]
        public virtual string PID_Bill_Code
        {
            get { return _PID_Bill_Code; }
            set { _PID_Bill_Code = value; }
        }
        [DataMember]
        public virtual string PID_ABN_Flag
        {
            get { return _PID_ABN_Flag; }
            set { _PID_ABN_Flag = value; }
        }
        [DataMember]
        public virtual string PID_Status_Of_Specimen
        {
            get { return _PID_Status_Of_Specimen; }
            set { _PID_Status_Of_Specimen = value; }
        }
        [DataMember]
        public virtual string PID_Fasting
        {
            get { return _PID_Fasting; }
            set { _PID_Fasting = value; }
        }
        [DataMember]
        public virtual string PID_Patient_SSN
        {
            get { return _PID_Patient_SSN; }
            set { _PID_Patient_SSN = value; }
        }
        [DataMember]
        public virtual string MSH_Message_Type_Inner_Segment
        {
            get { return _MSH_Message_Type_Inner_Segment; }
            set { _MSH_Message_Type_Inner_Segment = value; }
        }
        [DataMember]
        public virtual string temp_property
        {
            get { return _temp_property; }
            set { _temp_property = value; }
        }
        [DataMember]
        public virtual ulong Lab_ID
        {
            get { return _Lab_ID; }
            set { _Lab_ID = value; }
        }
        [DataMember]
        public virtual string Is_Electronic_Mode
        {
            get { return _Is_Electronic_Mode; }
            set { _Is_Electronic_Mode = value; }
        }
        [DataMember]
        public virtual string Is_Filled
        {
            get { return _Is_Filled; }
            set { _Is_Filled = value; }
        }

        [DataMember]
        public virtual string Order_Type
        {
            get { return _Order_type; }
            set { _Order_type = value; }
        }

        [DataMember]
        public virtual string Is_Scan_order
        {
            get { return _Is_Scan_order; }
            set { _Is_Scan_order = value; }
        }
        [DataMember]
        public virtual string Orders_Description
        {
            get { return _Orders_Description; }
            set { _Orders_Description = value; }
        }
        [DataMember]
        public virtual string MA_Notes
        {
            get { return _MA_Notes; }
            set { _MA_Notes = value; }
        }
        [DataMember]
        public virtual string File_Name
        {
            get { return _File_Name; }
            set { _File_Name = value; }
        }
        [DataMember]
        public virtual ulong Matching_Patient_Id
        {
            get { return _Matching_Patient_Id; }
            set { _Matching_Patient_Id = value; }
        }
        [DataMember]
        public virtual ulong Matching_Physician_Id
        {
            get { return _Matching_Physician_Id; }
            set { _Matching_Physician_Id = value; }
        }
        #endregion

        
    }
}
