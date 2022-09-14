
using System.Runtime.Serialization;
using System;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial class User : BusinessBase<string>
    {

        #region Declarations

        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _status = string.Empty;
        private string _role = string.Empty;
        private string _person_name = string.Empty;
        private string _User_Class = string.Empty;
        private ulong _Physician_Library_ID = 0;
        private int _Landing_Screen_ID = 0;
        private string _Default_Facility = string.Empty;
        private DateTime _Password_Changed_Date = DateTime.MinValue;
        private string _RCopia_User_Name = string.Empty;
        private string _Is_Down_Time = string.Empty;
        private int _Default_MyQ_Days = 0;
        private string _Short_Name = string.Empty;
        private string _Is_RCopia_Notification_Required = string.Empty;
        private string _Security_Question1 = string.Empty;
        private string _Answer1 = string.Empty;
        private string _Security_Question2 = string.Empty;
        private string _Answer2 = string.Empty;
        private string _Default_Server = string.Empty;
        private string _EMail_Address = string.Empty;
        private string _Legal_Org = string.Empty;
        private DateTime _Last_Successful_Login_Date_Time = DateTime.MinValue;
        private string _super_admin_password = string.Empty;
        #endregion

        #region constuctors

        public User() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_username);
            sb.Append(_password);
            sb.Append(_status);
            sb.Append(_role);
            sb.Append(_person_name);
            sb.Append(_User_Class);
            sb.Append(_Physician_Library_ID);
            sb.Append(_Landing_Screen_ID);
            sb.Append(_Default_Facility);
            sb.Append(_Password_Changed_Date);
            sb.Append(_RCopia_User_Name);
            sb.Append(_Is_RCopia_Notification_Required);
            sb.Append(_Security_Question1);
            sb.Append(_Answer1);
            sb.Append(_Security_Question2);
            sb.Append(_Answer2);
            sb.Append(_Default_Server);
            sb.Append(_EMail_Address);
            sb.Append(_Legal_Org);
            sb.Append(_Last_Successful_Login_Date_Time);
            sb.Append(_super_admin_password);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region properties
        [DataMember]
        public virtual string user_name
        {
            get { return _username; }
            set
            {
                _username = value;
            }
        }
        [DataMember]
        public virtual string password
        {
            get { return _password; }
            set
            {
                _password = value;
            }
        }
        [DataMember]
        public virtual string status
        {
            get { return _status; }
            set
            {
                _status = value;
            }
        }
        [DataMember]
        public virtual string role
        {
            get { return _role; }
            set
            {
                _role = value;
            }
        }
        [DataMember]
        public virtual string person_name
        {
            get { return _person_name; }
            set
            {
                _person_name = value;
            }
        }
        [DataMember]
        public virtual string User_Class
        {
            get { return _User_Class; }
            set
            {
                _User_Class = value;
            }
        }
        [DataMember]
        public virtual ulong Physician_Library_ID
        {
            get { return _Physician_Library_ID; }
            set
            {
                _Physician_Library_ID = value;
            }
        }
        [DataMember]
        public virtual int Landing_Screen_ID
        {
            get { return _Landing_Screen_ID; }
            set
            {
                _Landing_Screen_ID = value;
            }
        }

        [DataMember]
        public virtual string Default_Facility
        {
            get { return _Default_Facility; }
            set
            {
                _Default_Facility = value;
            }
        }

        [DataMember]
        public virtual DateTime Password_Changed_Date
        {
            get { return _Password_Changed_Date; }
            set
            {
                _Password_Changed_Date = value;
            }
        }

        [DataMember]
        public virtual string RCopia_User_Name
        {
            get { return _RCopia_User_Name; }
            set
            {
                _RCopia_User_Name = value;
            }
        }

        [DataMember]
        public virtual string Is_Down_Time
        {
            get { return _Is_Down_Time; }
            set
            {
                _Is_Down_Time = value;
            }
        }

        [DataMember]
        public virtual int Default_MyQ_Days
        {
            get { return _Default_MyQ_Days; }
            set
            {
                _Default_MyQ_Days = value;
            }
        }

        [DataMember]
        public virtual string Short_Name
        {
            get { return _Short_Name; }
            set
            {
                _Short_Name = value;
            }
        }

        [DataMember]
        public virtual string Is_RCopia_Notification_Required
        {
            get { return _Is_RCopia_Notification_Required; }
            set
            {
                _Is_RCopia_Notification_Required = value;
            }
        }
        [DataMember]
        public virtual string Security_Question1
        {
            get { return _Security_Question1; }
            set
            {
                _Security_Question1 = value;
            }
        }

        [DataMember]
        public virtual string Answer1
        {
            get { return _Answer1; }
            set
            {
                _Answer1 = value;
            }
        }

        [DataMember]
        public virtual string Security_Question2
        {
            get { return _Security_Question2; }
            set
            {
                _Security_Question2 = value;
            }
        }


        [DataMember]
        public virtual string Answer2
        {
            get { return _Answer2; }
            set
            {
                _Answer2 = value;
            }
        }

        [DataMember]
        public virtual string Default_Server
        {
            get { return _Default_Server; }
            set
            {
                _Default_Server = value;
            }
        }
        [DataMember]
        public virtual string EMail_Address
        {
            get { return _EMail_Address; }
            set
            {
                _EMail_Address = value;
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
        public virtual DateTime Last_Successful_Login_Date_Time
        {
            get { return _Last_Successful_Login_Date_Time; }
            set
            {
                _Last_Successful_Login_Date_Time = value;
            }
        }
        [DataMember]
        public virtual string Super_Admin_Password
        {
            get { return _super_admin_password; }
            set
            {
                _super_admin_password = value;
            }
        }
        #endregion
    }
}
