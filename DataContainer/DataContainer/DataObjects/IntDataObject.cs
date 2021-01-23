using System.IO;

namespace KEI.Infrastructure
{
    /// <summary>
    /// DataObject Implementation for <see cref="short"/>
    /// </summary>
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
    }

    /// <summary>
    /// DataObject Implementation for <see cref="int"/>
    /// </summary>
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
    }

    /// <summary>
    /// DataObject Implementation for <see cref="ushort"/>
    /// </summary>
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
    }

    /// <summary>
    /// DataObject Implementation for <see cref="uint"/>
    /// </summary>
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
    }

    /// <summary>
    /// DataObject Implementation for <see cref="ulong"/>
    /// </summary>
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
    }
}
