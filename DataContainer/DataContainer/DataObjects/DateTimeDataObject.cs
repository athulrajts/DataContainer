using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// DataObject Implementation for storing <see cref="DateTime"/>
    /// </summary>
    [Serializable]
    internal class DateTimeDataObject : DataObject<DateTime>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.DateTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public DateTimeDataObject(string name, DateTime value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DateTimeDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.OnStringValueChanged(string)"/>
        /// </summary>
        /// <param name="value"></param>
        protected override void OnStringValueChanged(string value)
        {
            if (DateTime.TryParse(value, out _value))
            {
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
