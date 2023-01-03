using System;
using System.Runtime.Serialization;

namespace Acurus.Capella.Core.DomainObjects
{
    [Serializable]
    public partial class MapXMLBlob : BusinessBase<ulong>
    {



        #region Declarations

        private ulong _Map_XML_Blob_ID = 0;
        private string _XML_Tag_Name = string.Empty;
        private string _Table_Name = string.Empty;
        private string _Condition = string.Empty;
        #endregion

        #region Constructors

        public MapXMLBlob() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Map_XML_Blob_ID);
            sb.Append(_XML_Tag_Name);
            sb.Append(_Table_Name);
            sb.Append(_Condition);
            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        [DataMember]
        public virtual ulong Map_XML_Blob_ID
        {
            get { return _Map_XML_Blob_ID; }
            set { _Map_XML_Blob_ID = value; }
        }
        [DataMember]
        public virtual string XML_Tag_Name
        {
            get { return _XML_Tag_Name; }
            set { _XML_Tag_Name = value; }

        }
        [DataMember]
        public virtual string Table_Name
        {
            get { return _Table_Name; }
            set { _Table_Name = value; }
        }
        [DataMember]
        public virtual string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        #endregion

    }
}
