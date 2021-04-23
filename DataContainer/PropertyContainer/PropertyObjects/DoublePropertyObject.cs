using System.Runtime.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// PropertyObject implementation for <see cref="double"/>
    /// </summary>
    [Serializable]
    internal class DoublePropertyObject : NumericPropertyObject<double>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public DoublePropertyObject(string name, double value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DoublePropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Double;

    }
}
