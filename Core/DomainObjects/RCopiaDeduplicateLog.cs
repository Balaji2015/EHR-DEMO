using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class RCopiaDeduplicateLog : BusinessBase<ulong>
    {
        #region Declarations

        private ulong _Human_ID = 0;
        private ulong _RCopia_ID = 0;
        private string _Entity_Name = string.Empty;
        private string _Duplicate_Type = string.Empty;
        private DateTime _Created_Date_and_Time = DateTime.MinValue;
        private DateTime _Modified_Date_and_Time = DateTime.MinValue;
        private string _Created_By = string.Empty;
        private string _Modified_By = string.Empty;
        private int _Version = 0;

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Human_ID);
            sb.Append(_RCopia_ID);
            sb.Append(_Entity_Name);
            sb.Append(_Duplicate_Type);
            sb.Append(_Created_Date_and_Time);
            sb.Append(_Modified_Date_and_Time);
            sb.Append(_Created_By);
            sb.Append(_Modified_By);
            sb.Append(_Version);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set { _Human_ID = value; }
        }

        [DataMember]
        public virtual ulong RCopia_ID
        {
            get { return _RCopia_ID; }
            set { _RCopia_ID = value; }
        }


        [DataMember]
        public virtual string Entity_Name
        {
            get { return _Entity_Name; }
            set { _Entity_Name = value; }
        }

        [DataMember]
        public virtual string Duplicate_Type
        {
            get { return _Duplicate_Type; }
            set { _Duplicate_Type = value; }
        }

        [DataMember]
        public virtual DateTime Created_Date_and_Time
        {
            get { return _Created_Date_and_Time; }
            set
            {
                _Created_Date_and_Time = value;
            }
        }

        [DataMember]
        public virtual DateTime Modified_Date_and_Time
        {
            get { return _Modified_Date_and_Time; }
            set
            {
                _Modified_Date_and_Time = value;
            }
        }

        [DataMember]
        public virtual string Created_By
        {
            get { return _Created_By; }
            set { _Created_By = value; }
        }

        [DataMember]
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set { _Modified_By = value; }
        }

        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set
            {
                _Version = value;
            }
        }

        #endregion
    }
}
