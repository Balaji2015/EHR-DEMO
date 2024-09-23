using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class CDCEventTracker : BusinessBase<ulong>
    {
        #region Declarations
        private ulong _Human_ID = 0;
        private string _Error_Description = string.Empty;
        private string _Created_By = string.Empty;
        private string _Status = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time;
        private int _version = 0;
        private ulong _Entity_ID = 0;
        private string _Entity_Name = string.Empty;
        #endregion

        #region HashCode Value

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);

            //sb.Append(_Encounter_ID);
            sb.Append(_Human_ID);
            sb.Append(_Error_Description);
            sb.Append(_Status);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_version);
            sb.Append(_Entity_ID);
            sb.Append(_Entity_Name);
            return sb.ToString().GetHashCode();
        }
        #endregion


        # region Properties
        //[DataMember]
        //public virtual ulong Encounter_ID
        //{
        //    get { return _Encounter_ID; }
        //    set { _Encounter_ID = value; }
        //}
        [DataMember]
        public virtual ulong Human_ID
        {
            get { return _Human_ID; }
            set { _Human_ID = value; }
        }
        [DataMember]
        public virtual string Error_Description
        {
            get { return _Error_Description; }
            set
            {
                _Error_Description = value;
            }
        }
        [DataMember]
        public virtual string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
            }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _Created_By; }
            set { _Created_By = value; }
        }
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
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set { _Modified_By = value; }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set
            {
                _Modified_Date_And_Time = value;
            }
        }
        [DataMember]
        public virtual int Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        [DataMember]
        public virtual ulong Entity_ID
        {
            get { return _Entity_ID; }
            set
            {
                _Entity_ID = value;
            }
        }
        [DataMember]
        public virtual string Entity_Name
        {
            get { return _Entity_Name; }
            set
            {
                _Entity_Name = value;
            }
        }
        #endregion
    }
}
