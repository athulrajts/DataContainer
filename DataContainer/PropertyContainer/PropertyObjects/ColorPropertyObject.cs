using System.Runtime.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// PropertyObject implementation to store Color
    /// Application UI should convert to UI color object using RGB values
    /// </summary>
    [Serializable]
    public class ColorPropertyObject : PropertyObject<Color>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public ColorPropertyObject(string name, Color color)
        {
            Name = name;
            Value = color;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ColorPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

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
