using System.Configuration.Helpers;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Configuration
{
    [Serializable]
    internal class PasswordPropertyObject : StringPropertyObject
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Password;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public PasswordPropertyObject(string name, string value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PasswordPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.WriteXmlAttributes(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteXmlAttributes(XmlWriter writer)
        {
            writer.WriteAttributeString(TYPE_ID_ATTRIBUTE, Type);
            writer.WriteAttributeString(KEY_ATTRIBUTE, Name);
            writer.WriteAttributeString(VALUE_ATTRIBUTE, EncryptionHelper.Encrypt(Value));
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.ReadXmlAttributes(XmlReader)"/>
        /// </summary>
        /// <param name="reader"></param>
        protected override void ReadXmlAttributes(XmlReader reader)
        {
            Name = reader.GetAttribute(KEY_ATTRIBUTE);
            if (reader.GetAttribute(VALUE_ATTRIBUTE) is string attr)
            {
                Value = EncryptionHelper.Decrypt(attr);
            }
        }
    }
}
