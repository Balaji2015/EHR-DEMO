using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class Document_Sub_Type_Lookup_List
    {
        public Document_Sub_Type_Lookup_List()
        {
            Document_Sub_Type_Lookup = new List<Document_Sub_Type_Lookup>();
        }
        public List<Document_Sub_Type_Lookup> Document_Sub_Type_Lookup { get; set; }
    }
    public class Document_Sub_Type_Lookup
    {
        public string File_Report_Type { get; set; }
        public string Document_Sub_Type { get; set; }
    }
}
