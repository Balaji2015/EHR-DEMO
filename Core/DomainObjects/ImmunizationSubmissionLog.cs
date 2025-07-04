using System;
using System.Runtime.Serialization;
namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class ImmunizationSubmissionLog : BusinessBase<ulong>
    {
        #region Declarations
        private ulong _Human_ID = 0;
        private ulong _Encounter_ID = 0;
        private ulong _Physician_ID = 0;
        private string _Control_ID = string.Empty;
        private string _Submission_Result_Type = string.Empty;
        private string _Result_Message = string.Empty;
        private string _Response_Message = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Request_Message = string.Empty; 
        #endregion

        #region Constructors
        public ImmunizationSubmissionLog() { }
        #endregion


        #region HashCode Value
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Human_ID);
            sb.Append(_Encounter_ID);
            sb.Append(_Physician_ID);
            sb.Append(_Control_ID);
            sb.Append(_Submission_Result_Type);
            sb.Append(_Result_Message);
            sb.Append(_Response_Message);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Request_Message);
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
        public virtual ulong Encounter_ID
        {
            get { return _Encounter_ID; }
            set
            {
                _Encounter_ID = value;
            }
        }
        [DataMember]
        public virtual ulong Physician_ID
        {
            get { return _Physician_ID; }
            set
            {
                _Physician_ID = value;
            }
        }
        [DataMember]
        public virtual string Control_ID
        {
            get { return _Control_ID; }
            set
            {
                _Control_ID = value;
            }
        }
        [DataMember]
        public virtual string Submission_Result_Type
        {
            get { return _Submission_Result_Type; }
            set
            {
                _Submission_Result_Type = value;
            }
        }
        [DataMember]
        public virtual string Result_Message
        {
            get { return _Result_Message; }
            set
            {
                _Result_Message = value;
            }
        }
        //[DataMember]
        //public virtual string Response_Message
        //{
        //    get { return _Response_Message; }
        //    set
        //    {
        //        _Response_Message = value;
        //    }
        //}
        //[DataMember]
        //public virtual string Created_By
        //{
        //    get { return _Created_By; }
        //    set
        //    {
        //        _Created_By = value;
        //    }
        //}
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
        public virtual string Request_Message
        {
            get { return _Request_Message; }
            set
            {
                _Request_Message = value;
            }
        }
        #endregion
    }
}

