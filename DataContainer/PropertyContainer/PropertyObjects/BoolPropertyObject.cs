using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// PropertyObject implementation for <see cref="bool"/>
    /// </summary>
    [Serializable]
    internal class BoolPropertyObject : PropertyObject<bool>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public BoolPropertyObject(string name, bool value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public BoolPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Boolean;
    }
}
