using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    [Serializable]
    public partial class FacilityLibrary : BusinessBase<ulong>
    {
        #region Declarations


        private string _FacName = string.Empty;
        private string _FacPrimaryCodeType = string.Empty;
        private string _FacPrimaryIDCode = string.Empty;
        private string _FacAddress1 = string.Empty;
        private string _FacAddress2 = string.Empty;
        private string _FacCity = string.Empty;
        private string _FacState = string.Empty;
        private string _FacZip = string.Empty;
        private string _FacSpecialtyCode = string.Empty;
        private string _FacTelephone = string.Empty;
        private string _FacFax = string.Empty;
        private string _FacEMail = string.Empty;
        private string _FacType = string.Empty;
        private string _FacNotes = string.Empty;
        private string _FacNPI = string.Empty;
        private string _FacQual = string.Empty;
        private string _FacOtherID = string.Empty;
        private DateTime _ChangedDateAndTime = DateTime.MinValue;
        private DateTime _CreatedDateAndTime = DateTime.MinValue;
        private int _Slot_Length = 0;
        private string _StartTime = string.Empty;
        private string _EndTime = string.Empty;
        private string _Time_Zone = string.Empty;
        private int _Version = 0;
        private string _Is_Room_In_Needed = string.Empty;
        private string _POS = string.Empty;
        private string _Pos_Description = string.Empty;
        private string _Created_By = string.Empty;
        private string _Modified_By = string.Empty;
        private string _LabCorp_Account_Number = string.Empty;
        private string _Quest_Account_Number = string.Empty;
        private int _sort_order = 0;
        private string _HosporClinic = string.Empty;
        private string _County = string.Empty;
        private string _Short_Name = string.Empty;
        private string _Fac_Type_Abbr = string.Empty;
        private string _Taxonomy_Code = string.Empty;
        private string _Taxonomy_Description = string.Empty;
        private string _Healthcare_Service_Location_Code = string.Empty;
        private string _Is_Ancillary = string.Empty;
        private string _Legal_Org = string.Empty;
        private string _Lab_Client_Information = string.Empty;
        #endregion

        #region Constructors

        public FacilityLibrary() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);

            sb.Append(_FacName);
            sb.Append(_FacPrimaryCodeType);
            sb.Append(_FacPrimaryIDCode);
            sb.Append(_FacAddress1);
            sb.Append(_FacAddress2);
            sb.Append(_FacCity);
            sb.Append(_FacState);
            sb.Append(_FacZip);
            sb.Append(_FacSpecialtyCode);
            sb.Append(_FacTelephone);
            sb.Append(_FacFax);
            sb.Append(_FacEMail);
            sb.Append(_FacType);
            sb.Append(_FacNotes);
            sb.Append(_FacNPI);
            sb.Append(_FacQual);
            sb.Append(_FacOtherID);
            sb.Append(_ChangedDateAndTime);
            sb.Append(_CreatedDateAndTime);
            sb.Append(_Slot_Length);
            sb.Append(_StartTime);
            sb.Append(_EndTime);
            sb.Append(_Time_Zone);
            sb.Append(_Version);
            sb.Append(_Is_Room_In_Needed);
            sb.Append(_POS);
            sb.Append(_Pos_Description);
            sb.Append(_Created_By);
            sb.Append(_Modified_By);
            sb.Append(_LabCorp_Account_Number);
            sb.Append(_Quest_Account_Number);
            sb.Append(_sort_order);
            sb.Append(_HosporClinic);
            sb.Append(_County);
            sb.Append(_Short_Name);
            sb.Append(_Fac_Type_Abbr);
            sb.Append(_Taxonomy_Code);
            sb.Append(_Taxonomy_Description);
            sb.Append(_Healthcare_Service_Location_Code);
            sb.Append(_Is_Ancillary);
            sb.Append(Legal_Org);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties


        [DataMember]
        public virtual string Fac_Name
        {
            get { return _FacName; }
            set
            {
                _FacName = value;
            }
        }
        [DataMember]
        public virtual string Fac_Primary_Code_Type
        {
            get { return _FacPrimaryCodeType; }
            set
            {
                _FacPrimaryCodeType = value;
            }
        }
        [DataMember]
        public virtual string Fac_Primary_ID_Code
        {
            get { return _FacPrimaryIDCode; }
            set
            {
                _FacPrimaryIDCode = value;
            }
        }
        [DataMember]
        public virtual string Fac_Address1
        {
            get { return _FacAddress1; }
            set
            {
                _FacAddress1 = value;
            }
        }
        [DataMember]
        public virtual string Fac_Address2
        {
            get { return _FacAddress2; }
            set
            {
                _FacAddress2 = value;
            }
        }
        [DataMember]
        public virtual string Fac_City
        {
            get { return _FacCity; }
            set
            {
                _FacCity = value;
            }
        }
        [DataMember]
        public virtual string Fac_State
        {
            get { return _FacState; }
            set
            {
                _FacState = value;
            }
        }
        [DataMember]
        public virtual string Fac_Zip
        {
            get { return _FacZip; }
            set
            {
                _FacZip = value;
            }
        }
        [DataMember]
        public virtual string Fac_Specality_Code
        {
            get { return _FacSpecialtyCode; }
            set
            {
                _FacSpecialtyCode = value;
            }
        }
        [DataMember]
        public virtual string Fac_Telephone
        {
            get { return _FacTelephone; }
            set
            {
                _FacTelephone = value;
            }
        }
        [DataMember]
        public virtual string Fac_Fax
        {
            get { return _FacFax; }
            set
            {
                _FacFax = value;
            }
        }
        [DataMember]
        public virtual string Fac_Email
        {
            get { return _FacEMail; }
            set
            {
                _FacEMail = value;
            }
        }
        [DataMember]
        public virtual string Fac_Type
        {
            get { return _FacType; }
            set
            {
                _FacType = value;
            }
        }
        [DataMember]
        public virtual string Fac_Notes
        {
            get { return _FacNotes; }
            set
            {
                _FacNotes = value;
            }
        }
        [DataMember]
        public virtual string Fac_NPI
        {
            get { return _FacNPI; }
            set
            {
                _FacNPI = value;
            }
        }
        [DataMember]
        public virtual string Fac_Qual
        {
            get { return _FacQual; }
            set
            {
                _FacQual = value;
            }
        }
        [DataMember]
        public virtual string Fac_Other_ID
        {
            get { return _FacOtherID; }
            set
            {
                _FacOtherID = value;
            }
        }
        [DataMember]
        public virtual DateTime Changed_Date_And_Time
        {
            get { return _ChangedDateAndTime; }
            set
            {
                _ChangedDateAndTime = value;
            }
        }
        [DataMember]
        public virtual DateTime Created_Date_And_Time
        {
            get { return _CreatedDateAndTime; }
            set
            {
                _CreatedDateAndTime = value;
            }
        }
        [DataMember]
        public virtual int Slot_Length
        {
            get { return _Slot_Length; }
            set
            {
                _Slot_Length = value;
            }
        }
        [DataMember]
        public virtual string Start_Time
        {
            get { return _StartTime; }
            set
            {
                _StartTime = value;
            }
        }
        [DataMember]
        public virtual string End_Time
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
            }
        }
        [DataMember]
        public virtual string Time_Zone
        {
            get { return _Time_Zone; }
            set
            {
                _Time_Zone = value;
            }
        }

        [DataMember]
        public virtual string Is_Room_In_Needed
        {
            get { return _Is_Room_In_Needed; }
            set
            {
                _Is_Room_In_Needed = value;
            }
        }

        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        [DataMember]
        public virtual string POS
        {
            get { return _POS; }
            set { _POS = value; }
        }

        [DataMember]
        public virtual string POS_Description
        {
            get { return _Pos_Description; }
            set { _Pos_Description = value; }
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
        public virtual string LabCorp_Account_Number
        {
            get { return _LabCorp_Account_Number; }
            set { _LabCorp_Account_Number = value; }
        }
        [DataMember]
        public virtual string Quest_Account_Number
        {
            get { return _Quest_Account_Number; }
            set { _Quest_Account_Number = value; }
        }

        [DataMember]
        public virtual int Sort_Order
        {
            get { return _sort_order; }
            set { _sort_order = value; }
        }
        [DataMember]
        public virtual string HosporClinic
        {
            get { return _HosporClinic; }
            set
            {
                _HosporClinic = value;
            }
        }
        [DataMember]
        public virtual string County
        {
            get { return _County; }
            set
            {
                _County = value;
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
        public virtual string Fac_Type_Abbr
        {
            get { return _Fac_Type_Abbr; }
            set
            {
                _Fac_Type_Abbr = value;
            }
        }
        [DataMember]
        public virtual string Taxonomy_Code
        {
            get { return _Taxonomy_Code; }
            set
            {
                _Taxonomy_Code = value;
            }
        }
        [DataMember]
        public virtual string Taxonomy_Description
        {
            get { return _Taxonomy_Description; }
            set
            {
                _Taxonomy_Description = value;
            }
        }
         [DataMember]
        public virtual string Healthcare_Service_Location_Code
        {
            get { return _Healthcare_Service_Location_Code; }
            set
            {
                _Healthcare_Service_Location_Code = value;
            }
        }
         [DataMember]
         public virtual string Is_Ancillary
         {
             get { return _Is_Ancillary; }
             set
             {
                 _Is_Ancillary = value;
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
         //CAP-3308
         [DataMember]
         public virtual string Lab_Client_Information
         {
             get { return _Lab_Client_Information; }
             set
             {
                _Lab_Client_Information = value;
             }
         }
        #endregion
    }
}
