using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AdministrativeGenderCodeList
    {
        public List<Code> Code { get; set; }
    }

    public class Code
    {
        public string value { get; set; }
    }

    public class CodeList
    {
        public List<Code> Code { get; set; }
    }

    public class ConfidentialityCodeList
    {
        public List<Code> Code { get; set; }
    }

    public class EthnicGroupCodeList
    {
        public List<Code> Code { get; set; }
    }

    public class InvalidList
    {
        public CodeList CodeList { get; set; }
        public ValueCodeList ValueCodeList { get; set; }
        public RouteCodeList RouteCodeList { get; set; }
        public UnitList UnitList { get; set; }
    }

    public class LanguageCodeList
    {
        public List<Code> Code { get; set; }
    }

    public class RaceCodeList
    {
        public List<Code> Code { get; set; }
    }

    public class LookupList_CCDAErrors
    {
        public InvalidList InvalidList { get; set; }
        public ValidList ValidList { get; set; }
    }

    public class RouteCodeList
    {
        public Code Code { get; set; }
    }

    public class Units
    {
        public string value { get; set; }
    }

    public class UnitList
    {
        public List<Units> Unit { get; set; }
    }

    public class ValidList
    {
        public ConfidentialityCodeList ConfidentialityCodeList { get; set; }
        public LanguageCodeList LanguageCodeList { get; set; }
        public AdministrativeGenderCodeList AdministrativeGenderCodeList { get; set; }
        public RaceCodeList RaceCodeList { get; set; }
        public EthnicGroupCodeList EthnicGroupCodeList { get; set; }
    }

    public class ValueCodeList
    {
        public List<Code> Code { get; set; }
    }


}