using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
   public partial class ApplicationAccessLog:BusinessBase<ulong>
    {
        #region Declarations
        private string _Token_ID = string.Empty;
        private ulong _Patient_ID = 0;
        private string _Access_Type = string.Empty;
        private string _Last_Name = string.Empty;
        private string _Birth_Date = string.Empty;
        private string _Start_Date = string.Empty;
        private string _Stop_Date = string.Empty;
        private string _Category = string.Empty;
        private string _Created_By = string.Empty;
        private string _Modified_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private string _Result_ID = string.Empty;
        private string _Client_ID = string.Empty;
        private string _Search_Info = string.Empty;

        #endregion
        #region Constructors

        public ApplicationAccessLog() { }

        #endregion

        #region Methods
        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Token_ID);
            sb.Append(_Patient_ID);
            sb.Append(_Access_Type);
            sb.Append(_Last_Name);
            sb.Append(_Birth_Date);
            sb.Append(_Start_Date);
            sb.Append(_Stop_Date);
            sb.Append(_Category);
            sb.Append(_Created_By );
            sb.Append(_Modified_By);
            sb.Append(_Created_Date_And_Time );
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Result_ID);
            return sb.ToString().GetHashCode();
        }
        #endregion


        #region Properties

        public virtual string Token_ID
        {
            get { return _Token_ID; }
            set
            {
                _Token_ID = value;
            }
        }
        public virtual ulong Patient_ID
        {
            get { return _Patient_ID; }
            set
            {
                _Patient_ID = value;
            }
        }
        public virtual string Access_Type
        {
            get { return _Access_Type; }
            set
            {
                _Access_Type = value;
            }
        }
        public virtual string Last_Name
        {
            get { return _Last_Name; }
            set
            {
                _Last_Name = value;
            }
        }
        public virtual string Category
        {
            get { return _Category; }
            set
            {
                _Category = value;
            }
        }
        public virtual string Created_By
        {
            get { return _Created_By; }
            set
            {
                _Created_By = value;
            }
        }
        public virtual string Modified_By
        {
            get { return _Modified_By; }
            set
            {
                _Modified_By = value;
            }
        }
        public virtual string Birth_Date
        {
            get { return _Birth_Date; }
            set
            {
                _Birth_Date = value;
            }
        }
        public virtual string Start_Date
        {
            get { return _Start_Date; }
            set
            {
                _Start_Date = value;
            }
        }
        public virtual string Stop_Date
        {
            get { return _Stop_Date; }
            set
            {
                _Stop_Date = value;
            }
        }
        public virtual DateTime Created_Date_And_Time
        {
            get { return _Created_Date_And_Time; }
            set
            {
                _Created_Date_And_Time = value;
            }
        }
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set
            {
                _Modified_Date_And_Time = value;
            }
        }

        public virtual string Result_ID
        {
            get { return _Result_ID; }
            set
            {
                _Result_ID = value;
            }
        }

        public virtual string Client_ID
        {
            get { return _Client_ID; }
            set
            {
                _Client_ID = value;
            }
        }

        public virtual string Search_Info
        {
            get { return _Search_Info; }
            set
            {
                _Search_Info = value;
            }
        }
        #endregion
    }
}
