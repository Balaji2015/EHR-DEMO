using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class PQRI_Data : BusinessBase<int>
    {
        #region Decleration

        private int _PQRI_Lookup_ID = 0;
        //private string _Measure_No = string.Empty;
        //private string _Measure_Name = string.Empty;
        private string _Standard_concept = string.Empty;
        private string _PQRI_Type = string.Empty;
        private string _PQRI_Value = string.Empty;
        private string _PQRI_Description = string.Empty;
        private string _PQRI_Selection_XML = string.Empty;
        private int _Sort_Order = 0;
        private string _NQF_Number = string.Empty;
        private string _Value_Set = string.Empty;
        private string _PQRI_Calculation_Method = string.Empty;
        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_PQRI_Lookup_ID);
            //sb.Append(_Measure_No);
            //sb.Append(_Measure_Name);
            sb.Append(_Standard_concept);
            sb.Append(_PQRI_Type);
            sb.Append(_PQRI_Value);
            sb.Append(_PQRI_Description);
            sb.Append(_PQRI_Selection_XML);
            sb.Append(_Sort_Order);
            sb.Append(_NQF_Number);
            sb.Append(_Value_Set);
            sb.Append(_PQRI_Calculation_Method);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Implementation
        [DataMember]
        public virtual int PQRI_Lookup_ID
        {
            get { return _PQRI_Lookup_ID; }
            set { _PQRI_Lookup_ID = value; }
        }

        //[DataMember]
        //public virtual string Measure_No
        //{
        //    get { return _Measure_No; }
        //    set { _Measure_No = value; }
        //}
        //[DataMember]
        //public virtual string Measure_Name
        //{
        //    get { return _Measure_Name; }
        //    set { _Measure_Name = value; }
        //}
        [DataMember]
        public virtual string Standard_concept
        {
            get { return _Standard_concept; }
            set { _Standard_concept = value; }
        }
        [DataMember]
        public virtual string PQRI_Type
        {
            get { return _PQRI_Type; }
            set { _PQRI_Type = value; }
        }
        [DataMember]
        public virtual string PQRI_Value
        {
            get { return _PQRI_Value; }
            set { _PQRI_Value = value; }
        }
        [DataMember]
        public virtual int Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }
        }
        [DataMember]
        public virtual string PQRI_Selection_XML
        {
            get { return _PQRI_Selection_XML; }
            set { _PQRI_Selection_XML = value; }
        }
        [DataMember]
        public virtual string PQRI_Description
        {
            get { return _PQRI_Description; }
            set { _PQRI_Description = value; }
        }
        [DataMember]
        public virtual string NQF_Number
        {
            get { return _NQF_Number; }
            set { _NQF_Number = value; }
        }
        [DataMember]
        public virtual string Value_Set
        {
            get { return _Value_Set; }
            set { _Value_Set = value; }
        }

        [DataMember]
        public virtual string PQRI_Calculation_Method
        {
            get { return _PQRI_Calculation_Method; }
            set { _PQRI_Calculation_Method = value; }
        }
        #endregion

    }
}
