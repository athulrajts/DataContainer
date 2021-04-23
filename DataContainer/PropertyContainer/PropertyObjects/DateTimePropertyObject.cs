using System.Runtime.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// PropertyObject Implementation for storing <see cref="DateTime"/>
    /// </summary>
    [Serializable]
    internal class DateTimePropertyObject : PropertyObject<DateTime>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public DateTimePropertyObject(string name, DateTime value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binar deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DateTimePropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.DateTime;

        protected override void OnStringValueChanged(string value)
        {
            if (DateTime.TryParse(value, out _value))
            {
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
