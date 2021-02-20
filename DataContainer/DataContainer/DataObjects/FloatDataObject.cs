using System;
using System.IO;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// DataObject Implementation for <see cref="float"/>
    /// </summary>
    [Serializable]
    internal class FloatDataObject : DataObject<float>, IWriteToBinaryStream
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public FloatDataObject(string name, float value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FloatDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Float;

        /// <summary>
        /// Implementation for <see cref="IWriteToBinaryStream.WriteBytes(BinaryWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Value);
        }
    }
}
