using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// DataObject implementation for storing <see cref="TimeSpan"/>
    /// </summary>
    [Serializable]
    internal class TimeSpanDataObject : DataObject<TimeSpan>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.TimeSpan;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public TimeSpanDataObject(string name, TimeSpan value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public TimeSpanDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
