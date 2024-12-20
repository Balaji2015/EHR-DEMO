using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class doctypeList
    {
        public List<Doctype> DocType { get; set; }
        public doctypeList()
        {
            DocType = new List<Doctype>();
        }        
    }

    public class Doctype
    {
        public List<subDoc> subDoc { get; set; }
        public string name { get; set; }
        public string Sort { get; set; }
        public string Default_Value { get; set; }
    }

    public class subDoc
    {
        public string name { get; set; }
        public string Default_Value { get; set; }
    }

}
