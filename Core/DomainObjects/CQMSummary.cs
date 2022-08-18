using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class CQMSummary : BusinessBase<ulong>
    {
        #region Declarations
       
        private int _Measurement_Year = 0;
        private string _Measurement_No = string.Empty;
        private string _Measurement_Name = string.Empty;
        private int _Initial_Population = 0;
        private int _Denominator = 0;
        private int _Denominator_Exclusion = 0;
        private int _Denominator_Exception = 0;
        private int _Numerator = 0;
        private int _Numerator_Exclusion = 0;
        private decimal _Rate=0;
        private string _Legal_Org = string.Empty;
        private ulong _Physician_ID = 0;
        
        #endregion

        #region Constructors

        public CQMSummary() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Measurement_Year);
            sb.Append(_Measurement_No);
            sb.Append(_Measurement_Name);
            sb.Append(_Initial_Population);
            sb.Append(_Denominator);
            sb.Append(_Denominator_Exclusion);
            sb.Append(_Denominator_Exception);
            sb.Append(_Numerator);
            sb.Append(_Numerator_Exclusion);
            sb.Append(_Rate);
            sb.Append(_Legal_Org);
            sb.Append(_Physician_ID);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties


        [DataMember]
        public virtual int Measurement_Year
        {
            get { return _Measurement_Year; }
            set
            {
                _Measurement_Year = value;
            }
        }
        [DataMember]
        public virtual string Measurement_No
        {
            get { return _Measurement_No; }
            set
            {
                _Measurement_No = value;
            }
        }
        [DataMember]
        public virtual string Measurement_Name
        {
            get { return _Measurement_Name; }
            set
            {
                _Measurement_Name = value;
            }
        }
        [DataMember]
        public virtual int Initial_Population
        {
            get { return _Initial_Population; }
            set
            {
                _Initial_Population = value;
            }
        }
        [DataMember]
        public virtual int Denominator
        {
            get { return _Denominator; }
            set
            {
                _Denominator = value;
            }
        }
        [DataMember]
        public virtual int Denominator_Exception
        {
            get { return _Denominator_Exception; }
            set
            {
                _Denominator_Exception = value;
            }
        }
        [DataMember]
        public virtual int Denominator_Exclusion
        {
            get { return _Denominator_Exclusion; }
            set
            {
                _Denominator_Exclusion = value;
            }
        }
        [DataMember]
        public virtual int Numerator
        {
            get { return _Numerator; }
            set
            {
                _Numerator = value;
            }
        }
        [DataMember]
        public virtual int Numerator_Exclusion
        {
            get { return _Numerator_Exclusion; }
            set
            {
                _Numerator_Exclusion = value;
            }
        }
        [DataMember]
        public virtual decimal Rate
        {
            get { return _Rate; }
            set
            {
                _Rate = value;
            }
        }
        [DataMember]
        public virtual string Legal_Org
        {
            get { return _Legal_Org; }
            set
            {
                _Legal_Org = value;
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

        #endregion



    }
}