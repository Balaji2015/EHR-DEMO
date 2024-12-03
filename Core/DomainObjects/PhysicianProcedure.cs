using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    [DataContract]
    public partial class PhysicianProcedure : FieldLookup
    {
        #region Declarations

        private string _Physician_Procedure_Code = string.Empty;
        private ulong _Physician_ID = 0;
        private string _Procedure_Description = string.Empty;
        private string _Procedure_Type = string.Empty;
        private string _Created_By = string.Empty;
        private string _Modified_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private int _Version = 0;
        private ulong _Lab_ID = 0;
        private ulong _Sort_Order = 0;
        private string _Order_Group_Name = string.Empty;
        private string _Is_Active = string.Empty;
        private string _type_of_visit = string.Empty;
        private string _Legal_Org = string.Empty;
        private string _Is_Selection_Enabled = string.Empty;
        #endregion


        #region Constructors

        public PhysicianProcedure() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Physician_Procedure_Code);
            sb.Append(_Physician_ID);
            sb.Append(_Procedure_Description);
            sb.Append(_Procedure_Type);
            sb.Append(_Created_By);
            sb.Append(_Modified_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Version);
            sb.Append(_Lab_ID);
            sb.Append(_Sort_Order);
            sb.Append(_Order_Group_Name);
            sb.Append(_Is_Active);
            sb.Append(_Legal_Org);
            sb.Append(_Is_Selection_Enabled);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        
            [DataMember]
        public virtual string Order_Group_Name
        {
            get { return _Order_Group_Name; }
            set { _Order_Group_Name = value; }
        }
        [DataMember]
        public virtual string Physician_Procedure_Code
        {
            get { return _Physician_Procedure_Code; }
            set { _Physician_Procedure_Code = value; }
        }
        [DataMember]
        public virtual ulong Physician_ID
        {
            get { return _Physician_ID; }
            set { _Physician_ID = value; }
        }

        [DataMember]
        public virtual string Procedure_Description
        {
            get { return _Procedure_Description; }
            set { _Procedure_Description = value; }
        }

        [DataMember]
        public virtual string Procedure_Type
        {
            get { return _Procedure_Type; }
            set { _Procedure_Type = value; }
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
        public virtual DateTime Created_Date_And_Time
        {
            get { return _Created_Date_And_Time; }
            set { _Created_Date_And_Time = value; }
        }
        [DataMember]
        public virtual DateTime Modified_Date_And_Time
        {
            get { return _Modified_Date_And_Time; }
            set { _Modified_Date_And_Time = value; }
        }

        [DataMember]
        public virtual int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        [DataMember]
        public virtual ulong Lab_ID
        {
            get { return _Lab_ID; }
            set { _Lab_ID = value; }
        }


        [DataMember]
        public virtual ulong Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }
        }
        [DataMember]
        public virtual string Is_Active
        {
            get { return _Is_Active; }
            set { _Is_Active = value; }
        }

        [DataMember]
        public virtual string Type_Of_Visit
        {
            get { return _type_of_visit; }
            set { _type_of_visit = value; }
        }
        [DataMember]
        public virtual string Legal_Org
        {
            get { return _Legal_Org; }
            set { _Legal_Org = value; }
        }

        [DataMember]
        public virtual string Is_Selection_Enabled
        {
            get { return _Is_Selection_Enabled; }
            set { _Is_Selection_Enabled = value; }
        }
        #endregion
    }
}
