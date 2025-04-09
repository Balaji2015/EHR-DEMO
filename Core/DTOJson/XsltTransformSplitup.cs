using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class XsltTransformSplitupList
    {
        public XsltTransformSplitupList()
        {
            XsltTransformSplitup = new List<XsltTransformSplitup>();
        }
        public IList<XsltTransformSplitup> XsltTransformSplitup { get; set; }
    }

    public class XsltTransformSplitup
    {
        public string SplitOrder { get; set; }
        public string TagNames { get; set; }
        public string AddAndModifyTags { get; set; }
    }
}
