using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// DataObject implmementation to store Color data
    /// </summary>
    [Serializable]
    internal class ColorDataObject : DataObject<Color>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public ColorDataObject(string name, Color color)
        {
            Name = name;
            Value = color;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ColorDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Color;
    }
}
