using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acurus.Capella.UI.OktaResponseModel
{
        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
            public Titles titles { get; set; }
            public Properties properties { get; set; }
        }

        public class Properties
        {
            [JsonProperty("okta:idp:metadata")]
            public string oktaidpmetadata { get; set; }

            [JsonProperty("okta:idp:type")]
            public string oktaidptype { get; set; }

            [JsonProperty("okta:idp:id")]
            public string oktaidpid { get; set; }
        }

        public class OktaUserIDPModel
        {
            public string subject { get; set; }
            public List<Link> links { get; set; }
        }

        public class Titles
        {
            public string und { get; set; }
        }
}