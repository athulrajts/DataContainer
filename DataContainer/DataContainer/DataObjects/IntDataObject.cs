using System.IO;
using System.Runtime.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// DataObject Implementation for <see cref="short"/>
    /// </summary>
    [Serializable]
    internal class ShortDataObject : DataObject<short>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Short;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ShortDataObject(string name, short value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ShortDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// DataObject Implementation for <see cref="int"/>
    /// </summary>
    [Serializable]
    internal class IntDataObject : DataObject<int>, IWriteToBinaryStream
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public IntDataObject(string name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public IntDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Integer;

        /// <summary>
        /// Implementation for <see cref="IWriteToBinaryStream.WriteBytes(BinaryWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Value);
        }
    }

    /// <summary>
    /// DataObject Implementation for <see cref="long"/>
    /// </summary>
    [Serializable]
    internal class LongDataObject : DataObject<long>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Long;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public LongDataObject(string name, long value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public LongDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// DataObject Implementation for <see cref="ushort"/>
    /// </summary>
    [Serializable]
    internal class UnsignedShortDataObject : DataObject<ushort>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.UShort;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public UnsignedShortDataObject(string name, ushort value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnsignedShortDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// DataObject Implementation for <see cref="uint"/>
    /// </summary>
    [Serializable]
    internal class UnsignedIntDataObject : DataObject<uint>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.UInteger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public UnsignedIntDataObject(string name, uint value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnsignedIntDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// DataObject Implementation for <see cref="ulong"/>
    /// </summary>
    [Serializable]
    internal class UnsignedLongDataObject : DataObject<ulong>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.ULong;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public UnsignedLongDataObject(string name, ulong value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnsignedLongDataObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
