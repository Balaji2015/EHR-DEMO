using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
     public partial class CarePlanLookup:BusinessBase<ulong>
    {
        #region Decleration

        private string _Field_Name = string.Empty;
        private string _Value = string.Empty;
        private int _Sort_Order=0;
        private ulong _Parent_Care_Plan_ID = 0;
        private string _Options = string.Empty;
        private string _Status = string.Empty;
        private string _Controls = string.Empty;
        private ulong _Measure_Rule_Master_ID = 0;
        private string _Control_Name = string.Empty;
        private string _Gender = string.Empty;
        private string _From_Age = string.Empty;
        private string _To_Age = string.Empty;
        private string _Care_Plan_Loinc_Code = string.Empty;
        private string _Options_Loinc_Code = string.Empty;

        #endregion

        #region Constructors

        public CarePlanLookup() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Field_Name);
            sb.Append(_Value);
            sb.Append(_Sort_Order);
            sb.Append(_Parent_Care_Plan_ID);
            sb.Append(_Options);
            sb.Append(_Status);
            sb.Append(_Controls);
            sb.Append(_Measure_Rule_Master_ID);
            sb.Append(_Control_Name);
            sb.Append(_Gender);
            sb.Append(_From_Age);
            sb.Append(_To_Age);
            sb.Append(_Care_Plan_Loinc_Code);
            sb.Append(_Options_Loinc_Code);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Implementation

        [DataMember]
        public virtual ulong Parent_Care_Plan_ID
        {
            get { return _Parent_Care_Plan_ID; }
            set { _Parent_Care_Plan_ID = value; }
        }
        [DataMember]
        public virtual string Field_Name
        {
            get { return _Field_Name; }
            set { _Field_Name = value; }
        }
        [DataMember]
        public virtual string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        [DataMember]
        public virtual int Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }
        }
        [DataMember]
        public virtual string Options
        {
            get { return _Options; }
            set { _Options = value; }
        }
        [DataMember]
        public virtual string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        [DataMember]
        public virtual string Controls
        {
            get { return _Controls; }
            set { _Controls = value; }
        }
        [DataMember]
        public virtual ulong Measure_Rule_Master_ID
        {
            get { return _Measure_Rule_Master_ID; }
            set { _Measure_Rule_Master_ID = value; }
        }
        [DataMember]
        public virtual string Control_Name
        {
            get { return _Control_Name; }
            set { _Control_Name = value; }
        }

        [DataMember]
        public virtual string Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        [DataMember]
        public virtual string From_Age
        {
            get { return _From_Age; }
            set { _From_Age = value; }
        }
        [DataMember]
        public virtual string To_Age
        {
            get { return _To_Age; }
            set { _To_Age = value; }
        }
        [DataMember]
        public virtual string Care_Plan_Loinc_Code
        {
            get { return _Care_Plan_Loinc_Code; }
            set { _Care_Plan_Loinc_Code = value; }
        }
        [DataMember]
        public virtual string Options_Loinc_Code
        {
            get { return _Options_Loinc_Code; }
            set { _Options_Loinc_Code = value; }
        }
        #endregion
    }
}
