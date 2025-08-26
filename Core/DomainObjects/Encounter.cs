using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class Encounter : BusinessBase<ulong>
    {
        #region Declarations

        private string _FacName = string.Empty;
        private ulong _Human_ID = 0;
        private DateTime _Date_of_Service = DateTime.MinValue;
        private string _Assigned_Med_Asst_User_Name = string.Empty;
        private string _Assigned_Room = string.Empty;
        private DateTime _Appointment_Date = DateTime.MinValue;
        private int _Duration_Minutes = 0;
        // private string _Method_of_Payment = string.Empty;
        //private string _Check_Card_No = string.Empty;
        //private DateTime _Check_Date;
        //private string _Auth_no = string.Empty;
        //private decimal _Patient_Payment;
        private string _Payment_Note = string.Empty;
        private string _Purpose_of_Visit = string.Empty;
        private string _Visit_Type = string.Empty;
        private string _Reason_for_Cancelation = string.Empty;
        private string _Cancelation_Reason_Code = string.Empty;
        private string _createdby = string.Empty;
        private DateTime _createddateandtime = DateTime.MinValue;
        private string _modifiedby = string.Empty;
        private DateTime _modifieddateandtime = DateTime.MinValue;
        private int _version = 0;
        private string _Is_PFSH_Verified = string.Empty;
        private string _Source_Of_Information = string.Empty;
        private string _If_Source_Of_Information_Others = string.Empty;
        private string _Is_Hospitalization_Denied = string.Empty;
        private string _Is_Medication_History_Verified = string.Empty;
        private string _Check_Out_Notes = string.Empty;
        private string _Exam_Room = string.Empty;
        private string _Referring_Facility = string.Empty;
        private string _Referring_Provider = string.Empty;
        private string _Call_Spoken_To = string.Empty;
        private string _Relationship = string.Empty;
       // private int _Call_Duration = 0; //For phone encounter
        private string _Call_Duration = string.Empty;
        private int _Insurance_Plan_ID = 0;
        private string _Place_Of_Service = string.Empty;
        private string _External_Plan_Number = string.Empty;
        private string _Policy_Holder_ID = string.Empty;
        private string _Reschedule_Reason_Code = string.Empty;
        private string _Reschedule_Reason_Text = string.Empty;
        private int _Appointment_Provider_ID = 0;
        private int _Encounter_Provider_ID = 0;
        private DateTime _Encounter_Provider_Signed_Date = DateTime.MinValue;
        private int _Encounter_Provider_Review_ID = 0;
        private DateTime _Encounter_Provider_Review_Signed_Date = DateTime.MinValue;
        //private string _Return_In = string.Empty;
        private DateTime _Due_On = DateTime.MinValue.Date;
        private string _Follow_Reason_Notes = string.Empty;
        //private string _Documents_To_Be_Print = string.Empty;
        //private string _Print_Documents_Given_To = string.Empty;

        private string _Is_Physician_Asst_Process = string.Empty;
        private ulong _Encounter_ID = 0;

        private string _Is_Prescription_to_be_Pushed = string.Empty;

        private string _Notes = string.Empty;
        private string _Is_Document_Given = string.Empty;
        private string _Address = string.Empty;
        private string _PhoneNo = string.Empty;
        private string _FaxNo = string.Empty;
        string _Is_Phone_Encounter = string.Empty;
        string _Is_Previous_Encounter_Copied = "N";
        private int _Return_In_Months = 0;
        private int _Return_In_Weeks = 0;
        private int _Return_In_Days = 0;
        private ulong _Technician_ID = 0;
        private ulong _Reading_Provider_ID = 0;
        private ulong _File_Reference_Number = 0;
        //private string _Encounter_Mode = string.Empty;
        private string _Is_Encounter_SuperBill = "N";
        private string _Willing_For_Prior_Appointment = "N";
        private string _Referring_Provider_NPI = string.Empty;
        private ulong _Message_ID = 0;
        private ulong _No_of_Med_Orders_In_Paper = 0;
        private string _Is_Moved_to_MA = string.Empty;
        private string _Is_Progressnote_Generated = "N";
        private string _PCP_Facility = string.Empty;
        private string _PCP_Provider = string.Empty;
        private string _PCP_Address = string.Empty;
        private string _PCP_PhoneNo = string.Empty;
        private string _PCP_FaxNo = string.Empty;
        private string _PCP_Provider_NPI = string.Empty;
        private string _Is_EandM_Submitted = "N";
        private string _Is_Batch_Created = "N";
        private string _Billing_Instruction = string.Empty;
        private string _Local_Time = string.Empty;
        private string _Batch_Status = "OPEN";
        private string _Ext_Apt_Uniq = string.Empty;
        private string _Is_Medication_Reviewed = string.Empty;
        private string _Reason_Not_Performed_Medication_Reviewed = string.Empty;
        private string _Snomed_Reason_Not_Performed_Med_Reviewed = string.Empty;

       

        private int _Order_Submit_ID = 0;
        private int _Machine_Technician_Library_ID = 0;

        private string _Authorization_Number = string.Empty;
        private string _Valid_From = string.Empty;
        private string _Valid_To = string.Empty;
        private int _Auth_Insurance_Plan_ID =0;

        private string _is_assessment_saved = "N";
        private string _is_serviceprocedure_saved = "N";


        private string _Proceed_with_Surgery_Planned = "N";
        private string _Authorization_CPT = string.Empty;

        private string _Is_PRN = "N";
        private string _Is_After_Studies = "N";
        private string _Is_Self_Referred = "N";
        private string _Assigned_Scribe_User_Name = string.Empty;
        private DateTime _E_M_Submitted_Date_And_Time = DateTime.MinValue;
        private string _Is_Signed_in_Akido_Note = "N";
        private string _Is_Auth_Verified = "N";
        private string _Phone_Encounter_Owner = string.Empty;
        #endregion

        #region Constructors

        public Encounter() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Authorization_CPT);
            sb.Append(_Proceed_with_Surgery_Planned);
            sb.Append(_is_assessment_saved);
            sb.Append(_is_serviceprocedure_saved);
            sb.Append(_FacName);
            sb.Append(_Human_ID);
            sb.Append(_Date_of_Service);
            sb.Append(_Assigned_Med_Asst_User_Name);
            sb.Append(_Assigned_Room);
            sb.Append(_Source_Of_Information);
            sb.Append(_Is_PFSH_Verified);
            sb.Append(_If_Source_Of_Information_Others);
            sb.Append(_Appointment_Date);
            sb.Append(_Duration_Minutes);
            sb.Append(_Payment_Note);
            sb.Append(_Purpose_of_Visit);
            sb.Append(_Visit_Type);
            sb.Append(_Reason_for_Cancelation);
            sb.Append(_Cancelation_Reason_Code);
            sb.Append(_version);
            sb.Append(_Is_Hospitalization_Denied);
            sb.Append(_createdby = string.Empty);
            sb.Append(_createddateandtime);
            sb.Append(_modifiedby = string.Empty);
            sb.Append(_modifieddateandtime);
            sb.Append(_Check_Out_Notes);
            sb.Append(_Is_Medication_History_Verified);
            sb.Append(_Exam_Room);
            sb.Append(_Call_Spoken_To);
            sb.Append(_Relationship);
            sb.Append(_Call_Duration);
            sb.Append(_Insurance_Plan_ID);
            sb.Append(_Place_Of_Service);
            sb.Append(_External_Plan_Number);
            sb.Append(_Policy_Holder_ID);
            sb.Append(_Reschedule_Reason_Code);
            sb.Append(_Reschedule_Reason_Text);
            sb.Append(_Appointment_Provider_ID);
            sb.Append(_Encounter_Provider_ID);
            sb.Append(_Encounter_Provider_Signed_Date);
            sb.Append(_Encounter_Provider_Review_ID);
            sb.Append(_Encounter_Provider_Review_Signed_Date);
            sb.Append(_Is_Physician_Asst_Process);
            //sb.Append(_Return_In);
            sb.Append(_Due_On);
            sb.Append(_Follow_Reason_Notes);
            sb.Append(_Is_Prescription_to_be_Pushed);
            sb.Append(_Encounter_ID);
            sb.Append(_Notes);
            //sb.Append(_Documents_To_Be_Print);
            //sb.Append(_Print_Documents_Given_To);

            sb.Append(_Is_Document_Given);
            sb.Append(_Address);
            sb.Append(_PhoneNo);
            sb.Append(_FaxNo);
            sb.Append(_Is_Phone_Encounter);
            sb.Append(_Is_Previous_Encounter_Copied);
            sb.Append(_Return_In_Months);
            sb.Append(_Return_In_Weeks);
            sb.Append(_Return_In_Days);
            sb.Append(_Technician_ID);
            sb.Append(_Reading_Provider_ID);
            sb.Append(_File_Reference_Number);
            //sb.Append(_Encounter_Mode);
            sb.Append(_Is_Encounter_SuperBill);
            sb.Append(_Willing_For_Prior_Appointment);
            sb.Append(_Referring_Provider_NPI);
            sb.Append(_Message_ID);
            sb.Append(_No_of_Med_Orders_In_Paper);
            sb.Append(_Is_Moved_to_MA);
            sb.Append(_Is_Progressnote_Generated);
            sb.Append(_PCP_Facility);
            sb.Append(_PCP_Provider);
            sb.Append(_PCP_Address);
            sb.Append(_PCP_PhoneNo);
            sb.Append(_PCP_FaxNo);
            sb.Append(_PCP_Provider_NPI);
            sb.Append(_Is_EandM_Submitted);
            sb.Append(_Is_Batch_Created);
            sb.Append(_Billing_Instruction);
            sb.Append(_Local_Time);
            sb.Append(_Batch_Status);
            sb.Append(_Ext_Apt_Uniq);
            sb.Append(_Is_Medication_Reviewed);
            sb.Append(_Reason_Not_Performed_Medication_Reviewed);
            sb.Append(_Snomed_Reason_Not_Performed_Med_Reviewed);


            sb.Append(_Order_Submit_ID);
            sb.Append(_Machine_Technician_Library_ID);


            sb.Append(_Authorization_Number);
            sb.Append(_Valid_From);
            sb.Append(_Valid_To);
            sb.Append(_Auth_Insurance_Plan_ID);

            sb.Append(_Is_PRN);
            sb.Append(_Is_After_Studies);
            sb.Append(_Is_Self_Referred);
            sb.Append(_Assigned_Scribe_User_Name);
            sb.Append(_E_M_Submitted_Date_And_Time);
            sb.Append(_Is_Signed_in_Akido_Note);
            sb.Append(_Is_Auth_Verified);
            sb.Append(_Phone_Encounter_Owner);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual string Assigned_Scribe_User_Name
        {
            get { return _Assigned_Scribe_User_Name; }
            set
            {
                _Assigned_Scribe_User_Name = value;
            }
        }


                    [DataMember]
        public virtual string Authorization_CPT
        {
            get { return _Authorization_CPT; }
            set
            {
                _Authorization_CPT = value;
            }
        }

             [DataMember]
        public virtual string Proceed_with_Surgery_Planned
        {
            get { return _Proceed_with_Surgery_Planned; }
            set
            {
                _Proceed_with_Surgery_Planned = value;
            }
        }

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
        public virtual string Facility_Name
        {
            get { return _FacName; }
            set
            {
                _FacName = value;
            }
        }

        [DataMember]
        public virtual DateTime Date_of_Service
        {
            get { return _Date_of_Service; }
            set
            {
                _Date_of_Service = value;
            }
        }
        [DataMember]
        public virtual string Local_Time
        {
            get { return _Local_Time; }
            set
            {
                _Local_Time = value;
            }
        }

        [DataMember]
        public virtual string Assigned_Med_Asst_User_Name
        {
            get { return _Assigned_Med_Asst_User_Name; }
            set
            {
                _Assigned_Med_Asst_User_Name = value;
            }
        }
        [DataMember]
        public virtual string Assigned_Room
        {
            get { return _Assigned_Room; }
            set
            {
                _Assigned_Room = value;
            }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _createdby; }
            set
            {
                _createdby = value;
            }
        }
        [DataMember]
        public virtual DateTime Created_Date_and_Time
        {
            get { return _createddateandtime; }
            set
            {
                _createddateandtime = value;
            }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _modifiedby; }
            set { _modifiedby = value; }
        }
        [DataMember]
        public virtual DateTime Modified_Date_and_Time
        {
            get { return _modifieddateandtime; }
            set
            {
                _modifieddateandtime = value;
            }
        }

        [DataMember]
        public virtual DateTime Appointment_Date
        {
            get { return _Appointment_Date; }
            set
            {
                _Appointment_Date = value;
            }
        }
        [DataMember]
        public virtual int Duration_Minutes
        {
            get
            {
                return _Duration_Minutes;
            }
            set
            {
                _Duration_Minutes = value;
            }
        }

        [DataMember]
        public virtual string Payment_Note
        {
            get
            {
                return _Payment_Note;
            }
            set
            {
                _Payment_Note = value;
            }
        }
        [DataMember]
        public virtual string Purpose_of_Visit
        {
            get
            {
                return _Purpose_of_Visit;
            }
            set
            {
                _Purpose_of_Visit = value;
            }
        }
        [DataMember]
        public virtual string Visit_Type
        {
            get
            {
                return _Visit_Type;
            }
            set
            {
                _Visit_Type = value;
            }
        }
        [DataMember]
        public virtual string Reason_for_Cancelation
        {
            get
            {
                return _Reason_for_Cancelation;
            }
            set
            {

                _Reason_for_Cancelation = value;
            }
        }

        [DataMember]
        public virtual string Cancelation_Reason_Code
        {
            get
            {
                return _Cancelation_Reason_Code;
            }
            set
            {

                _Cancelation_Reason_Code = value;
            }
        }
        [DataMember]
        public virtual string Is_Hospitalization_Denied
        {
            get
            {
                return _Is_Hospitalization_Denied;
            }
            set
            {
                _Is_Hospitalization_Denied = value;
            }
        }

        [DataMember]
        public virtual int Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        [DataMember]
        public virtual string Is_PFSH_Verified
        {
            get
            {
                return _Is_PFSH_Verified;
            }
            set
            {

                _Is_PFSH_Verified = value;
            }
        }
        [DataMember]
        public virtual string Source_Of_Information
        {
            get
            {
                return _Source_Of_Information;
            }
            set
            {

                _Source_Of_Information = value;
            }
        }
        [DataMember]
        public virtual string If_Source_Of_Information_Others
        {
            get
            {
                return _If_Source_Of_Information_Others;
            }
            set
            {

                _If_Source_Of_Information_Others = value;
            }
        }

        [DataMember]
        public virtual string Check_Out_Notes
        {
            get
            {
                return _Check_Out_Notes;
            }
            set
            {

                _Check_Out_Notes = value;
            }
        }

        [DataMember]
        public virtual string Is_Medication_History_Verified
        {
            get
            {
                return _Is_Medication_History_Verified;
            }
            set
            {
                _Is_Medication_History_Verified = value;
            }
        }
        [DataMember]
        public virtual string Exam_Room
        {
            get
            {
                return _Exam_Room;
            }
            set
            {
                _Exam_Room = value;
            }
        }
        [DataMember]
        public virtual string Referring_Facility
        {
            get
            {
                return _Referring_Facility;
            }
            set
            {
                _Referring_Facility = value;
            }
        }

        [DataMember]
        public virtual string Referring_Physician
        {
            get
            {
                return _Referring_Provider;
            }
            set
            {
                _Referring_Provider = value;
            }
        }

        [DataMember]
        public virtual string Call_Spoken_To
        {
            get
            {
                return _Call_Spoken_To;
            }
            set
            {
                _Call_Spoken_To = value;
            }
        }

        [DataMember]
        public virtual string Relationship
        {
            get
            {
                return _Relationship;
            }
            set
            {
                _Relationship = value;
            }
        }

        [DataMember]
        public virtual string Call_Duration
        {
            get
            {
                return _Call_Duration;
            }
            set
            {
                _Call_Duration = value;
            }
        }

        [DataMember]
        public virtual int Insurance_Plan_ID
        {
            get
            {
                return _Insurance_Plan_ID;
            }
            set
            {
                _Insurance_Plan_ID = value;
            }
        }

        [DataMember]
        public virtual string Place_Of_Service
        {
            get
            {
                return _Place_Of_Service;
            }
            set
            {
                _Place_Of_Service = value;
            }
        }

        [DataMember]
        public virtual string External_Plan_Number
        {
            get
            {
                return _External_Plan_Number;
            }
            set
            {
                _External_Plan_Number = value;
            }
        }

        [DataMember]
        public virtual string Policy_Holder_ID
        {
            get
            {
                return _Policy_Holder_ID;
            }
            set
            {
                _Policy_Holder_ID = value;
            }
        }
        [DataMember]
        public virtual string Reschedule_Reason_Code
        {
            get
            {
                return _Reschedule_Reason_Code;
            }
            set
            {
                _Reschedule_Reason_Code = value;
            }
        }

        [DataMember]
        public virtual string Reschedule_Reason_Text
        {
            get
            {
                return _Reschedule_Reason_Text;
            }
            set
            {
                _Reschedule_Reason_Text = value;
            }
        }

        [DataMember]
        public virtual int Appointment_Provider_ID
        {
            get
            {
                return _Appointment_Provider_ID;
            }
            set
            {
                _Appointment_Provider_ID = value;
            }
        }

        [DataMember]
        public virtual int Encounter_Provider_ID
        {
            get
            {
                return _Encounter_Provider_ID;
            }
            set
            {
                _Encounter_Provider_ID = value;
            }
        }

        [DataMember]
        public virtual DateTime Encounter_Provider_Signed_Date
        {
            get
            {
                return _Encounter_Provider_Signed_Date;
            }
            set
            {
                _Encounter_Provider_Signed_Date = value;
            }
        }

        [DataMember]
        public virtual int Encounter_Provider_Review_ID
        {
            get
            {
                return _Encounter_Provider_Review_ID;
            }
            set
            {
                _Encounter_Provider_Review_ID = value;
            }
        }

        [DataMember]
        public virtual DateTime Encounter_Provider_Review_Signed_Date
        {
            get
            {
                return _Encounter_Provider_Review_Signed_Date;
            }
            set
            {
                _Encounter_Provider_Review_Signed_Date = value;
            }
        }

        [DataMember]
        public virtual string Is_Physician_Asst_Process
        {
            get
            {
                return _Is_Physician_Asst_Process;
            }
            set
            {
                _Is_Physician_Asst_Process = value;
            }
        }

        //[DataMember]
        //public virtual string Return_In
        //{
        //    get
        //    {
        //        return _Return_In;
        //    }
        //    set
        //    {
        //        _Return_In = value;
        //    }
        //}

        [DataMember]
        public virtual DateTime Due_On
        {
            get
            {
                return _Due_On;
            }
            set
            {
                _Due_On = value;
            }
        }

        [DataMember]
        public virtual string Follow_Reason_Notes
        {
            get
            {
                return _Follow_Reason_Notes;
            }
            set
            {
                _Follow_Reason_Notes = value;
            }
        }
        //[DataMember]
        //public virtual string Print_Documents_Given_To
        //{
        //    get
        //    {
        //        return _Print_Documents_Given_To;
        //    }
        //    set
        //    {
        //        _Print_Documents_Given_To = value;
        //    }
        //}
        [DataMember]
        public virtual ulong Encounter_ID
        {
            get
            {
                return _Encounter_ID;
            }
            set
            {
                _Encounter_ID = value;
            }
        }
        [DataMember]
        public virtual string Is_Prescription_To_Be_Moved
        {
            get
            {
                return _Is_Prescription_to_be_Pushed;
            }
            set
            {
                _Is_Prescription_to_be_Pushed = value;
            }
        }
        [DataMember]
        public virtual string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                _Notes = value;
            }
        }
        //[DataMember]
        //public virtual string Documents_To_Be_Print
        //{
        //    get
        //    {
        //        return _Documents_To_Be_Print;
        //    }
        //    set
        //    {
        //        _Documents_To_Be_Print = value;
        //    }
        //}
        [DataMember]
        public virtual string Is_Document_Given
        {
            get
            {
                return _Is_Document_Given;
            }
            set
            {
                _Is_Document_Given = value;
            }
        }
        [DataMember]
        public virtual string Referring_Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
            }
        }
        [DataMember]
        public virtual string Referring_Phone_No
        {
            get
            {
                return _PhoneNo;
            }
            set
            {
                _PhoneNo = value;
            }
        }
        [DataMember]
        public virtual string Referring_Fax_No
        {
            get
            {
                return _FaxNo;
            }
            set
            {
                _FaxNo = value;
            }
        }

        [DataMember]
        public virtual string Is_Phone_Encounter
        {
            get
            {
                return _Is_Phone_Encounter;
            }
            set
            {
                _Is_Phone_Encounter = value;
            }
        }
        [DataMember]
        public virtual string Is_Previous_Encounter_Copied
        {
            get
            {
                return _Is_Previous_Encounter_Copied;
            }
            set
            {
                _Is_Previous_Encounter_Copied = value;
            }
        }
        [DataMember]
        public virtual int Return_In_Months
        {
            get
            {
                return _Return_In_Months;
            }
            set
            {
                _Return_In_Months = value;
            }
        }
        [DataMember]
        public virtual int Return_In_Weeks
        {
            get
            {
                return _Return_In_Weeks;
            }
            set
            {
                _Return_In_Weeks = value;
            }
        }
        [DataMember]
        public virtual int Return_In_Days
        {
            get
            {
                return _Return_In_Days;
            }
            set
            {
                _Return_In_Days = value;
            }
        }
        [DataMember]
        public virtual ulong Technician_ID
        {
            get
            {
                return _Technician_ID;
            }
            set
            {
                _Technician_ID = value;
            }
        }
        [DataMember]
        public virtual ulong Reading_Provider_ID
        {
            get
            {
                return _Reading_Provider_ID;
            }
            set
            {
                _Reading_Provider_ID = value;
            }
        }

        [DataMember]
        public virtual ulong File_Reference_Number
        {
            get
            {
                return _File_Reference_Number;
            }
            set
            {
                _File_Reference_Number = value;
            }
        }
        //[DataMember]
        //public virtual string Encounter_Mode
        //{
        //    get { return _Encounter_Mode; }
        //    set
        //    {
        //        _Encounter_Mode = value;
        //    }
        //}

        [DataMember]
        public virtual string Is_Encounter_SuperBill
        {
            get { return _Is_Encounter_SuperBill; }
            set
            {
                _Is_Encounter_SuperBill = value;
            }
        }

        [DataMember]
        public virtual string Willing_For_Prior_Appointment
        {
            get { return _Willing_For_Prior_Appointment; }
            set
            {
                _Willing_For_Prior_Appointment = value;
            }
        }

        [DataMember]
        public virtual string Referring_Provider_NPI
        {
            get { return _Referring_Provider_NPI; }
            set
            {
                _Referring_Provider_NPI = value;
            }
        }
        [DataMember]
        public virtual ulong Message_ID
        {
            get { return _Message_ID; }
            set
            {
                _Message_ID = value;
            }
        }

        [DataMember]
        public virtual ulong No_of_Med_Orders_In_Paper
        {
            get
            {
                return _No_of_Med_Orders_In_Paper;
            }
            set
            {
                _No_of_Med_Orders_In_Paper = value;
            }
        }


        [DataMember]
        public virtual string Is_Moved_to_MA
        {
            get { return _Is_Moved_to_MA; }
            set
            {
                _Is_Moved_to_MA = value;
            }
        }

        [DataMember]
        public virtual string Is_Progressnote_Generated
        {
            get
            {
                return _Is_Progressnote_Generated;
            }
            set
            {
                _Is_Progressnote_Generated = value;
            }
        }

        [DataMember]
        public virtual string PCP_Facility
        {
            get
            {
                return _PCP_Facility;
            }
            set
            {
                _PCP_Facility = value;
            }
        }

        [DataMember]
        public virtual string PCP_Physician
        {
            get
            {
                return _PCP_Provider;
            }
            set
            {
                _PCP_Provider = value;
            }
        }
        [DataMember]
        public virtual string PCP_Address
        {
            get
            {
                return _PCP_Address;
            }
            set
            {
                _PCP_Address = value;
            }
        }
        [DataMember]
        public virtual string PCP_Phone_No
        {
            get
            {
                return _PCP_PhoneNo;
            }
            set
            {
                _PCP_PhoneNo = value;
            }
        }
        [DataMember]
        public virtual string PCP_Fax_No
        {
            get
            {
                return _PCP_FaxNo;
            }
            set
            {
                _PCP_FaxNo = value;
            }
        }
        [DataMember]
        public virtual string PCP_Provider_NPI
        {
            get { return _PCP_Provider_NPI; }
            set
            {
                _PCP_Provider_NPI = value;
            }
        }

        [DataMember]
        public virtual string Is_EandM_Submitted
        {
            get { return _Is_EandM_Submitted; }
            set
            {
                _Is_EandM_Submitted = value;
            }
        }

        [DataMember]
        public virtual string Is_Batch_Created
        {
            get { return _Is_Batch_Created; }
            set
            {
                _Is_Batch_Created = value;
            }
        }
        [DataMember]
        public virtual string Billing_Instruction
        {
            get { return _Billing_Instruction; }
            set
            {
                _Billing_Instruction = value;
            }
        }
        [DataMember]
        public virtual string Batch_Status
        {
            get { return _Batch_Status; }
            set
            {
                _Batch_Status = value;
            }
        }
        [DataMember]
        public virtual string Ext_Apt_Uniq
        {
            get { return _Ext_Apt_Uniq; }
            set
            {
                _Ext_Apt_Uniq = value;
            }
        }
        [DataMember]
        public virtual string Is_Medication_Reviewed
        {
            get { return _Is_Medication_Reviewed; }
            set
            {
                _Is_Medication_Reviewed = value;
            }
        }
        [DataMember]
        public virtual string Reason_Not_Performed_Medication_Reviewed
        {
            get { return _Reason_Not_Performed_Medication_Reviewed; }
            set
            {
                _Reason_Not_Performed_Medication_Reviewed = value;
            }
        }
        [DataMember]
        public virtual string Snomed_Reason_Not_Performed_Med_Reviewed
        {
            get { return _Snomed_Reason_Not_Performed_Med_Reviewed; }
            set
            {
                _Snomed_Reason_Not_Performed_Med_Reviewed = value;
            }
        }

        [DataMember]
        public virtual int Order_Submit_ID
        {
            get { return _Order_Submit_ID; }
            set
            {
                _Order_Submit_ID = value;
            }
        }
        [DataMember]
        public virtual int Machine_Technician_Library_ID
        {
            get { return _Machine_Technician_Library_ID; }
            set
            {
                _Machine_Technician_Library_ID = value;
            }
        }

        [DataMember]
        public virtual string Authorization_Number
        {
            get { return _Authorization_Number; }
            set
            {
                _Authorization_Number = value;
            }
        }
         [DataMember]
        public virtual string Valid_From
        {
            get { return _Valid_From; }
            set
            {
                _Valid_From = value;
            }
        }
         [DataMember]
        public virtual string Valid_To
        {
            get { return _Valid_To; }
            set
            {
                _Valid_To = value;
            }
        }
         [DataMember]
        public virtual int Auth_Insurance_Plan_ID
        {
            get { return _Auth_Insurance_Plan_ID; }
            set
            {
                _Auth_Insurance_Plan_ID = value;
            }
        }
         [DataMember]
         public virtual string is_serviceprocedure_saved
         {
             get { return _is_serviceprocedure_saved; }
             set
             {
                 _is_serviceprocedure_saved = value;
             }
         }

         [DataMember]
         public virtual string is_assessment_saved
         {
             get { return _is_assessment_saved; }
             set
             {
                 _is_assessment_saved = value;
             }
         }



         [DataMember]
         public virtual string Is_PRN
         {
             get { return _Is_PRN; }
             set
             {
                 _Is_PRN = value;
             }
         }
        [DataMember]
         public virtual string Is_After_Studies
        {
            get { return _Is_After_Studies; }
            set
            {
                _Is_After_Studies = value;
            }
        }
        [DataMember]
        public virtual string Is_Self_Referred
        {
            get { return _Is_Self_Referred; }
            set
            {
                _Is_Self_Referred = value;
            }
        }

        [DataMember]
        public virtual DateTime E_M_Submitted_Date_And_Time
        {
            get { return _E_M_Submitted_Date_And_Time; }
            set
            {
                _E_M_Submitted_Date_And_Time = value;
            }
        }
        [DataMember]
        public virtual string Is_Signed_in_Akido_Note
        {
            get { return _Is_Signed_in_Akido_Note; }
            set
            {
                _Is_Signed_in_Akido_Note = value;
            }
        }

        [DataMember]
        public virtual string Is_Auth_Verified
        {
            get { return _Is_Auth_Verified; }
            set
            {
                _Is_Auth_Verified = value;
            }
        }
        [DataMember]
        public virtual string Phone_Encounter_Owner
        {
            get { return _Phone_Encounter_Owner; }
            set { _Phone_Encounter_Owner = value; }
        }
       
    }


        #endregion
    }

