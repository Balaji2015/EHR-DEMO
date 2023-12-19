using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class ProcedureModifierLookup : BusinessBase<ulong>
    {
        #region Declarion
        private string _Procedure_Code = string.Empty;
        private string _Modifier = string.Empty;
        private int _sort_order = 0;
        private double _RVU = 0;
        #endregion

        #region Constructors

        public ProcedureModifierLookup() { }

        #endregion

        #region HashCode Value
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Modifier);
            sb.Append(_Procedure_Code);
            sb.Append(_sort_order);
            sb.Append(_RVU);
            return sb.ToString().GetHashCode();
        }
        #endregion
        #region Properties

        [DataMember]
        public virtual string Modifier
        {
            get { return _Modifier; }
            set
            {
                _Modifier = value;
            }
        }
        [DataMember]
        public virtual string Procedure_Code
        {
            get { return _Procedure_Code; }
            set
            {
                _Procedure_Code = value;
            }
        }
        
        [DataMember]
        public virtual int Sort_Order
        {
            get { return _sort_order; }
            set
            {
                _sort_order = value;
            }
        }
        [DataMember]
        public virtual double RVU
        {
            get { return _RVU; }
            set
            {
                _RVU = value;
            }
        }
        #endregion
    }
}
