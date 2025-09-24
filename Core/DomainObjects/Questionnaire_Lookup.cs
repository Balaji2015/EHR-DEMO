using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial class Questionnaire_Lookup : BusinessBase<ulong>
    {
        #region Decleration

        private ulong _Questionnaire_Lookup_Id = 0;
        private string _User_Name = string.Empty;
        private string _Questionnaire_Category = string.Empty;
        private string _Questionnaire_Type = string.Empty;
        private string _Question = string .Empty ;
        private string _Lookup_Values = string.Empty;
        private string _Show_In_Progress_Notes = string.Empty;
        private int _Sort_Order=0;
        private double _From_Months = 0;
        private double _To_Months = 0;
        private string _Is_Calculation_Required = string.Empty;
        private string _Normal_Question_Status = string.Empty;
        private string _Is_Ros_Type = string.Empty;
        private string _Is_Notes = string.Empty;
        private string _Controls = string.Empty;
        private string _Question_Loinc_Code = string.Empty;
        private string _Options_Loinc_Code = string.Empty;
        private string _Where_Criteria = string.Empty;
        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Questionnaire_Lookup_Id);
            sb.Append(_User_Name);
            sb.Append(_Questionnaire_Category);
            sb.Append(_Questionnaire_Type);
            sb.Append(_Question);
            sb.Append(_Lookup_Values);
            sb.Append(_Show_In_Progress_Notes);
            sb.Append(_Sort_Order);
            sb.Append(_From_Months);
            sb.Append(_To_Months);
            sb.Append(_Is_Calculation_Required);
            sb.Append(_Normal_Question_Status);
            sb.Append(_Is_Ros_Type);
            sb.Append(_Is_Notes);
            sb.Append(_Controls);
            sb.Append(_Question_Loinc_Code);
            sb.Append(_Options_Loinc_Code);
            sb.Append(_Where_Criteria);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Implementation
        [DataMember]
        public virtual ulong Questionnaire_Lookup_Id
        {
            get { return _Questionnaire_Lookup_Id; }
            set { _Questionnaire_Lookup_Id = value; }
        }

        [DataMember]
        public virtual string User_Name
        {
            get { return _User_Name; }
            set { _User_Name = value; }
        }
        [DataMember]
        public virtual string Questionnaire_Category
        {
            get { return _Questionnaire_Category; }
            set { _Questionnaire_Category = value; }
        }
        [DataMember]
        public virtual string Questionnaire_Type
        {
            get { return _Questionnaire_Type; }
            set { _Questionnaire_Type = value; }
        } 
        [DataMember]
        public virtual string Question
        {
            get { return _Question; }
            set { _Question = value; }
        }
        [DataMember]
        public virtual string Lookup_Values
        {
            get { return _Lookup_Values; }
            set { _Lookup_Values = value; }
        }
        [DataMember]
        public virtual string Show_In_Progress_Notes
        {
            get { return _Show_In_Progress_Notes; }
            set { _Show_In_Progress_Notes = value; }
        }
        [DataMember]
        public virtual int Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }
        }

        [DataMember]
        public virtual double From_Months
        {
            get { return _From_Months; }
            set { _From_Months = value; }
        }
        [DataMember]
        public virtual double To_Months
        {
            get { return _To_Months; }
            set { _To_Months = value; }
        }
        [DataMember]
        public virtual string Is_Calculation_Required
        {
            get { return _Is_Calculation_Required; }
            set { _Is_Calculation_Required = value; }
        }
        [DataMember]
        public virtual string Normal_Question_Status
        {
            get { return _Normal_Question_Status; }
            set { _Normal_Question_Status = value; }
        }
        [DataMember]
        public virtual string Is_Ros_Type
        {
            get { return _Is_Ros_Type; }
            set { _Is_Ros_Type = value; }
        }
        [DataMember]
        public virtual string Is_Notes
        {
            get { return _Is_Notes; }
            set { _Is_Notes = value; }
        }
        [DataMember]
        public virtual string Controls
        {
            get { return _Controls; }
            set { _Controls = value; }
        }
        [DataMember]
        public virtual string Question_Loinc_Code
        {
            get { return _Question_Loinc_Code; }
            set { _Question_Loinc_Code = value; }
        }
        [DataMember]
        public virtual string Options_Loinc_Code
        {
            get { return _Options_Loinc_Code; }
            set { _Options_Loinc_Code = value; }
        }
        [DataMember]
        public virtual string Where_Criteria
        {
            get { return _Where_Criteria; }
            set { _Where_Criteria = value; }
        }
        #endregion
    }
}
