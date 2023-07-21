using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class Rcopia_Update_info : BusinessBase<ulong>
    {

        #region Declarations

        private string _Command=string.Empty;
        private DateTime _Last_Updated_Date_Time = DateTime.MinValue;
        private string _Value = string.Empty;
        private string _Legal_Org = string.Empty;
        #endregion

        #region Constructors

        public Rcopia_Update_info() { }

        #endregion


        #region HashCode Value

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Command);
            sb.Append(_Last_Updated_Date_Time);
            sb.Append(_Value);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties
        [DataMember]
        public virtual string Command
        {
            get { return _Command; }
            set
            {
                _Command = value;
            }
        }
        [DataMember]
        public virtual DateTime Last_Updated_Date_Time
        {
            get { return _Last_Updated_Date_Time; }
            set
            {
                _Last_Updated_Date_Time = value;
            }
        }

        [DataMember]
        public virtual string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
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
        #endregion

    }
}
