using System.Collections.Generic;

namespace Acurus.Capella.Core.DTOJson
{
    public class UserList
    {
        public UserList()
        {
            User = new List<UserData>();
        }
        public List<UserData> User { get; set; }
    }

    public class UserData
    {
        public string User_Name { get; set; }
        public string Default_Facility { get; set; }
        public string Role { get; set; }
        public string Physician_Library_ID { get; set; }
        public string person_name { get; set; }
        public string Default_MyQ_Days { get; set; }
        public string Legal_Org { get; set; }
    }
}
