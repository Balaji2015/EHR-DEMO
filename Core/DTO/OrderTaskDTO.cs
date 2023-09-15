using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTO
{
    public class OrderTaskDTO
    {
        private ulong _Order_Submit_ID;
        private string _Stat;
        private DateTime _Created_Date_And_Time;
        private string _Task;
        private string _Priority;
        private string _Ancillary_Order;
        private string _Created_By;
        private string _Physician_Name;
        private string _Lab_Procedure;
        private string _Lab_Procedure_Description;
        private string _Lab_Name;
        private string _ICD;
        private string _ICD_Description;
        private string _Facility_Name;
        private string _Current_Process;
        private string _Lab_Procedure_Message;
        private string _ICD_Description_Message;
        private DateTime _Modified_Date_And_Time;
        private ulong _Human_ID;

        [DataMember]
        public ulong Order_Submit_ID
        {
            get { return _Order_Submit_ID; }
            set { _Order_Submit_ID = value; }
        }
        [DataMember]
        public string Stat { get { return _Stat; } set { _Stat = value; } }
        [DataMember]
        public DateTime Created_Date_And_Time { get { return _Created_Date_And_Time; } set { _Created_Date_And_Time = value; } }
        [DataMember]
        public string Task { get { return _Task; } set { _Task = value; } }
        [DataMember]
        public string Priority { get { return _Priority; } set { _Priority = value; } }
        [DataMember]
        public string Ancillary_Order { get { return _Ancillary_Order; } set { _Ancillary_Order = value; } }
        [DataMember]
        public string Created_By { get { return _Created_By; } set { _Created_By = value; } }
        [DataMember]
        public string Physician_Name { get { return _Physician_Name; } set { _Physician_Name = value; } }
        [DataMember]
        public string Lab_Procedure { get { return _Lab_Procedure; } set { _Lab_Procedure = value; } }
        [DataMember]
        public string Lab_Procedure_Description { get { return _Lab_Procedure_Description; } set { _Lab_Procedure_Description = value; } }
        [DataMember]
        public string Lab_Name { get { return _Lab_Name; } set { _Lab_Name = value; } }
        [DataMember]
        public string ICD { get { return _ICD; } set { _ICD = value; } }
        [DataMember]
        public string ICD_Description { get { return _ICD_Description; } set { _ICD_Description = value; } }
        [DataMember]
        public string Facility_Name { get { return _Facility_Name; } set { _Facility_Name = value; } }
        [DataMember]
        public string Current_Process { get { return _Current_Process; } set { _Current_Process = value; } }
        [DataMember]
        public DateTime Modified_Date_And_Time { get { return _Modified_Date_And_Time; } set { _Modified_Date_And_Time = value; } }
        [DataMember]
        public ulong Human_ID
        {
            get { return _Human_ID; }
            set { _Human_ID = value; }
        }

        public string Lab_Procedure_Message { get { return _Lab_Procedure_Message; } set { _Lab_Procedure_Message = value; } }
        public string ICD_Description_Message { get { return _ICD_Description_Message; } set { _ICD_Description_Message = value; } }
    }
}
