using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// PropertyObject implementation for <see cref="float"/>
    /// </summary>
    [Serializable]
    internal class FloatPropertyObject : NumericPropertyObject<float>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public FloatPropertyObject(string name, float value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FloatPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Float;

    }
}
