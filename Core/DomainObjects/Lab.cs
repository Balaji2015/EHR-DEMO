using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial class Lab : BusinessBase<ulong>
    {



        #region Declarations

        private ulong _Lab_Id=0;
        private string _Lab_Name = string.Empty;
        private string _Created_By = string.Empty;
        private DateTime _Created_Date_And_Time = DateTime.MinValue;
        private string _Modified_By = string.Empty;
        private DateTime _Modified_Date_And_Time = DateTime.MinValue;
        private string _Lab_Type = string.Empty;
        //Janani - Main - 30 Jul 2011 - Start
        private int _Sort_Order = 0;
        //Janani - Main - 30 Jul 2011 - End
        private string _Category = string.Empty;
        private string _Is_Task_Notification_Required = string.Empty;   
        #endregion

        #region Constructors

        public Lab() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Lab_Id);
            sb.Append(_Lab_Name);
            sb.Append(_Created_By);
            sb.Append(_Created_Date_And_Time);
            sb.Append(_Modified_By);
            sb.Append(_Modified_Date_And_Time);
            sb.Append(_Lab_Type);
            //Janani - Main - 30 Jul 2011 - Start
            sb.Append(_Sort_Order);
            //Janani - Main - 30 Jul 2011 - End
            sb.Append(_Category);
            sb.Append(_Is_Task_Notification_Required);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual ulong Lab_ID
        {
            get { return _Lab_Id; }
            set { _Lab_Id = value; }
        }
        [DataMember]
        public virtual string Lab_Name
        {
            get { return _Lab_Name; }
            set { _Lab_Name = value; }

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
            set { _Created_Date_And_Time = value; }
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
            set { _Modified_Date_And_Time = value; }
        }
        [DataMember]
        public virtual string Lab_Type
        {
            get { return _Lab_Type; }
            set { _Lab_Type = value; }
        }
        //Janani - Main - 30 Jul 2011 - Start
        [DataMember]
        public virtual int Sort_Order
        {
            get { return _Sort_Order; }
            set { _Sort_Order = value; }
        }
        //Janani - Main - 30 Jul 2011 - End
        [DataMember]
        public virtual string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        [DataMember]
        public virtual string Is_Task_Notification_Required
        {
            get { return _Is_Task_Notification_Required; }
            set { _Is_Task_Notification_Required = value; }
        }

        #endregion

    }
}
