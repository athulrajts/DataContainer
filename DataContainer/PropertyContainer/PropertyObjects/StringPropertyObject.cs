using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// PropertyObject implementation for <see cref="string"/>
    /// </summary>
    [Serializable]
    internal class StringPropertyObject : PropertyObject<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public StringPropertyObject(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public StringPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.String;

        /// <summary>
        /// Implementation for <see cref="DataObject.ConvertFromString(string)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFromString(string value)
        {
            return value;
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.OnStringValueChanged(string)"/>
        /// </summary>
        /// <param name="value"></param>
        protected override void OnStringValueChanged(string value)
        {
            _value = value;
            RaisePropertyChanged(nameof(Value));
        }
    }
}
