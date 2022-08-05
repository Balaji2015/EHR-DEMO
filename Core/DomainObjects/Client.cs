using System.Runtime.Serialization;
using System;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial class Client : BusinessBase<ulong>
    {
         #region Declarations

        private ulong _Client_ID = 0;
        private string _Client_Name = string.Empty;
        private string _Legal_Org = string.Empty;
        private string _Client_Full_Name = string.Empty;
        #endregion

        #region constuctors

        public Client() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Client_ID);
            sb.Append(_Client_Name);
            sb.Append(_Legal_Org);
            sb.Append(_Client_Full_Name);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region properties
        [DataMember]
        public virtual ulong Client_ID
        {
            get { return _Client_ID; }
            set
            {
                _Client_ID = value;
            }
        }
        [DataMember]
        public virtual string Client_Name
        {
            get { return _Client_Name; }
            set
            {
                _Client_Name = value;
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
        public virtual string Client_Full_Name
        {
            get { return _Client_Full_Name; }
            set
            {
                _Client_Full_Name = value;
            }
        }
        #endregion
    }
}
