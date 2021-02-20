using System;
using System.IO;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// DataObject implementation for <see cref="byte"/>
    /// </summary>
    [Serializable]
    internal class ByteDataObject : DataObject<byte>, IWriteToBinaryStream
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ByteDataObject(string name, byte value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ByteDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Byte;

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
