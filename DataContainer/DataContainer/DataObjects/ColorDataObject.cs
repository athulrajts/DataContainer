using System.Runtime.Serialization;

namespace System.Configuration
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

        /// <summary>
        /// Doing this because TypeConverter was not working in C++/CLI
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool CanConvertFromString(string value)
        {
            return Color.TryParse(value, out _);
        }

        /// <summary>
        /// Doing this because TypeConverter was not working in C++/CLI
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFromString(string value)
        {
            Color.TryParse(value, out Color c);
            return c;
        }
    }
}
