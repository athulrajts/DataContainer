using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Configuration
{
    /// <summary>
    /// DataObject implementation for <see cref="enum"/>
    /// </summary>
    [Serializable]
    internal class EnumDataObject : DataObject<Enum>, IWriteToBinaryStream
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public EnumDataObject(string name, Enum value)
        {
            Name = name;
            Value = value;
            EnumType = value?.GetType();
        }

        /// <summary>
        /// Constructor for binary Deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EnumDataObject(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString(nameof(Name));
            EnumType = (TypeInfo)info.GetValue("Type", typeof(TypeInfo));
            Value = (Enum)info.GetValue("Value", EnumType);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Enum;

        /// <summary>
        /// contains Type of enum stored in this object
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Implementation for <see cref="DataObject.ReadXmlElement(string, XmlReader)"/>
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool ReadXmlElement(string elementName, XmlReader reader)
        {
            if (base.ReadXmlElement(elementName, reader) == true)
            {
                return true;
            }

            if (elementName == nameof(TypeInfo))
            {
                EnumType = reader.ReadObjectXml<TypeInfo>();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.OnXmlReadingCompleted"/>
        /// </summary>
        protected override void OnXmlReadingCompleted()
        {
            Value = (Enum)Enum.Parse(EnumType, StringValue);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.WriteXmlContent(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteXmlContent(XmlWriter writer)
        {
            base.WriteXmlContent(writer);

            writer.WriteObjectXml(new TypeInfo(EnumType));
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.CanConvertFromString(string)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool CanConvertFromString(string value)
        {
            return TryParse(value, out object _);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.ConvertFromString(string)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFromString(string value)
        {
            if (EnumType is null)
            {
                return null;
            }

            return TryParse(value, out object tmp)
                ? tmp
                : null;
        }

        /// <summary>
        /// Implementation for <see cref="IWriteToBinaryStream.WriteBytes(BinaryWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Convert.ToInt32(Value));
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.GetDataType"/>
        /// </summary>
        /// <returns></returns>
        public override Type GetDataType()
        {
            return EnumType;
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.OnStringValueChanged(string)"/>
        /// </summary>
        /// <param name="value"></param>
        protected override void OnStringValueChanged(string value)
        {
            if (EnumType != null && CanConvertFromString(value))
            {
                _value = (Enum)ConvertFromString(value);
                RaisePropertyChanged(nameof(Value));
            }
        }

        private bool TryParse(string value, out object obj)
        {
            obj = null;
            try
            {
                obj = Enum.Parse(EnumType, value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Type", new TypeInfo(EnumType));
        }
    }
}
