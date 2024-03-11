using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acurus.Capella.UI.OktaResponseModel
{
    public class Cancel
    {
        public string href { get; set; }
        public Hints hints { get; set; }
    }

    public class Embedded
    {
        public UserInfo user { get; set; }
    }

    public class Hints
    {
        public List<string> allow { get; set; }
    }

    public class Links
    {
        public Cancel cancel { get; set; }
    }

    public class Profile
    {
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string locale { get; set; }
        public string timeZone { get; set; }
    }

    public class OktaUserResponseModel
    {
        public DateTime expiresAt { get; set; }
        public string status { get; set; }
        public string sessionToken { get; set; }
        public Embedded _embedded { get; set; }
        public Links _links { get; set; }
    }

    public class UserInfo
    {
        public string id { get; set; }
        public DateTime passwordChanged { get; set; }
        public Profile profile { get; set; }
    }
}