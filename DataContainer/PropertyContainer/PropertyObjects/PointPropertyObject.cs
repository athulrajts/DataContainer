using System.Runtime.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// Property object implementation for storing <see cref="Point"/>
    /// </summary>
    [Serializable]
    internal class PointPropertyObject : PropertyObject<Point>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public PointPropertyObject(string name, Point value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PointPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Point;


        /// <summary>
        /// Doing this because TypeConverter was not working in C++/CLI
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool CanConvertFromString(string value)
        {
            return Point.TryParse(value, out _);
        }

        /// <summary>
        /// Doing this because TypeConverter was not working in C++/CLI
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFromString(string value)
        {
            Point.TryParse(value, out Point p);
            return p;
        }
    }
}
