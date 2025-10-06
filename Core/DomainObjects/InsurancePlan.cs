using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [DataContract]
    public partial class InsurancePlan : BusinessBase<ulong>
    {

        #region Declarations

        private string _PayerRecCode = string.Empty;
        private string _PayerAddr1= string.Empty;
        private string _PayerAddr2= string.Empty;
        private string _PayerCity= string.Empty;
        private string _PayerState= string.Empty;
        private string _PayerZip= string.Empty;
        private string _PayerPhoneNumber= string.Empty;
        private string _OfficeNumber= string.Empty;
        private string _PayerNotes= string.Empty;
        private string _ExternalPlanNumber= string.Empty;
        private string _InsPlanName= string.Empty;
        private DateTime _CreatedDateAndTime=DateTime.MinValue ;
        private DateTime _ModifiedDateAndTime = DateTime.MinValue;
        private string _ModifiedBy= string.Empty;
        private string _FormNumber= string.Empty;
        private string _FormatFileNumber= string.Empty;
        private string _Emc_ID1= string.Empty;
        private string _Emc_ID2= string.Empty;
        private string _Govt_Type= string.Empty;
        private string _FaxNumber= string.Empty;
        private string _EligibilityPhoneNumber= string.Empty;
        private string _Active= string.Empty;
        private int _CarrierID=0;
        private string _FinancialClassName= string.Empty;
        private string _CreatedBy= string.Empty;
        private int _iVersion=0;
        private IList<Carrier> _CarrierBag;
        private string _Is_Workers_Comp = string.Empty;
        private string _Claim_Attention = string.Empty;
        private string _Claim_Address = string.Empty;
        private string _Claim_City = string.Empty;
        private string _Claim_State = string.Empty;
        private string _Claim_ZipCode = string.Empty;
        private string _Health_Insurance_Type = string.Empty;
        private string _Health_Insurance_Description = string.Empty;
        private string _Coverage_Type_Code = string.Empty;
        private string _Coverage_Type_Description = string.Empty;

        #endregion

        #region Constructors

        public InsurancePlan() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_PayerRecCode);
            sb.Append(_PayerAddr1);
            sb.Append(_PayerAddr2);
            sb.Append(_PayerCity);
            sb.Append(_PayerState);
            sb.Append(_PayerZip);
            sb.Append(_PayerPhoneNumber);
            sb.Append(_OfficeNumber);
            sb.Append(_PayerNotes);
            sb.Append(_ExternalPlanNumber);
            sb.Append(_InsPlanName);
            sb.Append(_CreatedDateAndTime);
            sb.Append(_ModifiedDateAndTime);
            sb.Append(_ModifiedBy);
            sb.Append(_FormNumber);
            sb.Append(_FormatFileNumber);
            sb.Append(_Emc_ID1);
            sb.Append(_Emc_ID2);
            sb.Append(_Govt_Type);
            sb.Append(_FaxNumber);
            sb.Append(_EligibilityPhoneNumber);
            sb.Append(_Active);
            sb.Append(_CarrierID);
            sb.Append(_FinancialClassName);
            sb.Append(_CreatedBy);
            sb.Append(_iVersion);
            sb.Append(_Is_Workers_Comp);
            sb.Append(_Claim_Attention);
            sb.Append(_Claim_Address);
            sb.Append(_Claim_City);
            sb.Append(_Claim_State);
            sb.Append(_Claim_ZipCode);
            sb.Append(_Health_Insurance_Type);
            sb.Append(_Health_Insurance_Description);
            sb.Append(_Coverage_Type_Code);
            sb.Append(_Coverage_Type_Description);
           
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual string Payer_Rec_Code
        {
            get { return _PayerRecCode; }
            set
            {
                _PayerRecCode = value;
            }
        }
        [DataMember]
        public virtual string Payer_Addrress1
        {
            get { return _PayerAddr1; }
            set
            {
                _PayerAddr1 = value;
            }
        }
        [DataMember]
        public virtual string Payer_Addrress2
        {
            get { return _PayerAddr2; }
            set
            {
                _PayerAddr2 = value;
            }
        }
        [DataMember]
        public virtual string Payer_City
        {
            get { return _PayerCity; }
            set
            {
                _PayerCity = value;
            }
        }
        [DataMember]
        public virtual string Payer_State
        {
            get { return _PayerState; }
            set
            {
                _PayerState = value;
            }
        }
        [DataMember]
        public virtual string Payer_Zip
        {
            get { return _PayerZip; }
            set
            {
                _PayerZip = value;
            }
        }
        [DataMember]
        public virtual string Payer_Phone_Number
        {
            get { return _PayerPhoneNumber; }
            set
            {
                _PayerPhoneNumber = value;
            }
        }
        [DataMember]
        public virtual string Office_Number
        {
            get { return _OfficeNumber; }
            set
            {
                _OfficeNumber = value;
            }
        }
        [DataMember]
        public virtual string Payer_Notes
        {
            get { return _PayerNotes; }
            set
            {
                _PayerNotes = value;
            }
        }
        [DataMember]
        public virtual string External_Plan_Number
        {
            get { return _ExternalPlanNumber; }
            set
            {
                _ExternalPlanNumber = value;
            }
        }
        [DataMember]
        public virtual string Ins_Plan_Name
        {
            get { return _InsPlanName; }
            set
            {
                _InsPlanName = value;
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
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _ModifiedDateAndTime; }
            set
            {
                _ModifiedDateAndTime = value;
            }
        }
        [DataMember]
        public virtual string Modified_By
        {
            get { return _ModifiedBy; }
            set
            {
                _ModifiedBy = value;
            }
        }
        [DataMember]
        public virtual string Form_Number
        {
            get { return _FormNumber; }
            set
            {
                _FormNumber = value;
            }
        }
        [DataMember]
        public virtual string Format_File_Number
        {
            get { return _FormatFileNumber; }
            set
            {
                _FormatFileNumber = value;
            }
        }
        [DataMember]
        public virtual string Emc_ID1
        {
            get { return _Emc_ID1; }
            set
            {
                _Emc_ID1 = value;
            }
        }
        [DataMember]
        public virtual string Emc_ID2
        {
            get { return _Emc_ID2; }
            set
            {
                _Emc_ID2 = value;
            }
        }
        [DataMember]
        public virtual string Govt_Type
        {
            get { return _Govt_Type; }
            set
            {
                _Govt_Type = value;
            }
        }
        [DataMember]
        public virtual string Fax_Number
        {
            get { return _FaxNumber; }
            set
            {
                _FaxNumber = value;
            }
        }
        [DataMember]
        public virtual string Eligibility_Phone_Number
        {
            get { return _EligibilityPhoneNumber; }
            set
            {
                _EligibilityPhoneNumber = value;
            }
        }
        [DataMember]
        public virtual string Active
        {
            get { return _Active; }
            set
            {
                _Active = value;
            }
        }
        [DataMember]
        public virtual int Carrier_ID
        {
            get { return _CarrierID; }
            set
            {
                _CarrierID = value;
            }
        }
        [DataMember]
        public virtual string Financial_Class_Name
        {
            get { return _FinancialClassName; }
            set
            {
                _FinancialClassName = value;
            }
        }
        [DataMember]
        public virtual string Created_By
        {
            get { return _CreatedBy; }
            set
            {
                _CreatedBy = value;
            }
        }
        [DataMember]
        public virtual IList<Carrier> CarrierBag
        {
            get { return _CarrierBag; }
            set { _CarrierBag = value; }
        }
        [DataMember]
        public virtual int Version
        {
            get { return _iVersion; }
            set { _iVersion = value; }
        }

        [DataMember]
        public virtual string Is_Workers_Comp
        {
            get { return _Is_Workers_Comp; }
            set { _Is_Workers_Comp = value; }
        }
        [DataMember]
        public virtual string Claim_Attention
        {
            get { return _Claim_Attention; }
            set { _Claim_Attention = value; }
        }
        [DataMember]
        public virtual string Claim_Address
        {
            get { return _Claim_Address; }
            set { _Claim_Address = value; }
        }
        [DataMember]
        public virtual string Claim_City
        {
            get { return _Claim_City; }
            set { _Claim_City = value; }
        }
        [DataMember]
        public virtual string Claim_State
        {
            get { return _Claim_State; }
            set { _Claim_State = value; }
        }
        [DataMember]
        public virtual string Claim_ZipCode
        {
            get { return _Claim_ZipCode; }
            set { _Claim_ZipCode = value; }
        }
        [DataMember]
        public virtual string Health_Insurance_Type
        {
            get { return _Health_Insurance_Type; }
            set { _Health_Insurance_Type = value; }
        }
        [DataMember]
        public virtual string Health_Insurance_Description
        {
            get { return _Health_Insurance_Description; }
            set { _Health_Insurance_Description = value; }
        }
        [DataMember]
        public virtual string Coverage_Type_Code
        {
            get { return _Coverage_Type_Code; }
            set { _Coverage_Type_Code = value; }
        }
        [DataMember]
        public virtual string Coverage_Type_Description
        {
            get { return _Coverage_Type_Description; }
            set { _Coverage_Type_Description = value; }
        }
        #endregion

    }
}
