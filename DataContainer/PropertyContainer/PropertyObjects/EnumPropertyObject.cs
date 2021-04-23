using System.Configuration.Validation;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Configuration
{
    /// <summary>
    /// PropertyObject implementation for <see cref="enum"/>
    /// </summary>
    [Serializable]
    internal class EnumPropertyObject : PropertyObject<Enum>
    {
        public EnumPropertyObject(string name, Enum value)
        {
            Name = name;
            Value = value;
            EnumType = value?.GetType();
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EnumPropertyObject(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString(nameof(Name));
            Category = info.GetString(nameof(Category));
            Description = info.GetString(nameof(Description));
            DisplayName = info.GetString(nameof(DisplayName));
            BrowseOption = (BrowseOptions)info.GetValue(nameof(BrowseOption), typeof(BrowseOptions));

            //TODO Add validation
            Validation = (ValidatorGroup)info.GetValue(nameof(Validation), typeof(ValidatorGroup));

            EnumType = (TypeInfo)info.GetValue("Type", typeof(TypeInfo));
            Value = (Enum)info.GetValue(nameof(Value), EnumType);
        }

        /// <summary>
        /// Type of enum held by this object
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Enum;

        /// <summary>
        /// Implementation for <see cref="DataObject.CanConvertFromString(string)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool CanConvertFromString(string value)
        {
            return TryParse(value, out _);
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
        /// Implementation for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Type", new TypeInfo(Value.GetType()));
        }

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
        /// Implementation for <see cref="DataObject.OnStringValueChanged(string)"/>
        /// </summary>
        /// <param name="value"></param>
        protected override void OnStringValueChanged(string value)
        {
            if (EnumType != null &&
                CanConvertFromString(value))
            {
                _value = (Enum)ConvertFromString(value);
                RaisePropertyChanged(nameof(Value));
            }
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.GetDataType"/>
        /// </summary>
        /// <returns></returns>
        public override Type GetDataType()
        {
            return EnumType;
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
    }
}
