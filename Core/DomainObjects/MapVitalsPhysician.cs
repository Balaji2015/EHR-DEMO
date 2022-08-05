using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class MapVitalsPhysician : BusinessBase<ulong>
    {
        #region Declarations

        private ulong _Physician_ID=0;
        private ulong _Master_Vitals_ID=0;
        private string _Vital_Text = string.Empty;
        private string _Age_Condition_In_Years = string.Empty;
        private string _Sex = string.Empty;
        private int _Sort_Order=0;
        private string _BP_Status = string.Empty;
        private string _Legal_Org = string.Empty;

        #endregion

        #region Constructors

        public MapVitalsPhysician() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Physician_ID);
            sb.Append(_Master_Vitals_ID);
            sb.Append(_Vital_Text);
            sb.Append(_Age_Condition_In_Years);
            sb.Append(_Sex);
            sb.Append(_Sort_Order);
            sb.Append(_BP_Status);
            sb.Append(_Legal_Org);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual ulong Physician_ID
        {
            get { return _Physician_ID; }
            set { _Physician_ID = value; }
        }

        [DataMember]
        public virtual ulong Master_Vitals_ID
        {
            get { return _Master_Vitals_ID; }
            set { _Master_Vitals_ID = value; }
        }

        [DataMember]
        public virtual string Vital_Text
        {
            get { return _Vital_Text; }
            set { _Vital_Text = value; }
        }

        [DataMember]
        public virtual string Age_Condition_In_Years
        {
            get { return _Age_Condition_In_Years; }
            set { _Age_Condition_In_Years = value; }
        }
        [DataMember]
        public virtual string Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        [DataMember]
        public virtual int Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }            
        }
        [DataMember]
        public virtual string BP_Status
        {
            get { return _BP_Status; }
            set { _BP_Status = value; }
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
        #endregion
    }
}
