using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acurus.Capella.Core.DTOJson
{


    public class CapellascribelookupList
    {
        public CapellascribelookupList()
        {
            CapellaScribeLookup = new List<Capellascribelookup>();
        }

        public List<Capellascribelookup> CapellaScribeLookup { get; set; }
    }

    public class Capellascribelookup
    {
        public string ProviderUserName { get; set; }
        public string ProviderID { get; set; }
        public string ScribeUserName { get; set; }
        public string ScribeID { get; set; }
        public string Legal_Org { get; set; }
    }
}