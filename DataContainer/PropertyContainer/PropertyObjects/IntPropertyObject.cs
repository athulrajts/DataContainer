using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// PropertyObject implementation for <see cref="short"/>
    /// </summary>
    [Serializable]
    internal class ShortPropertyObject : NumericPropertyObject<short>
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
        public ShortPropertyObject(string name, short value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ShortPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// PropertyObject implementation for <see cref="int"/>
    /// </summary>
    [Serializable]
    internal class IntPropertyObject : NumericPropertyObject<int>
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Integer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public IntPropertyObject(string name, int value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public IntPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// PropertyObject implementation for <see cref="long"/>
    /// </summary>
    [Serializable]
    internal class LongPropertyObject : NumericPropertyObject<long>
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
        public LongPropertyObject(string name, long value) : base(name, value) { }
        
        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public LongPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// PropertyObject implementation for <see cref="ushort"/>
    /// </summary>
    [Serializable]
    internal class UnsignedShortPropertyObject : NumericPropertyObject<ushort>
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
        public UnsignedShortPropertyObject(string name, ushort value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnsignedShortPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// PropertyObject implementation for <see cref="uint"/>
    /// </summary>
    [Serializable]
    internal class UnsignedIntPropertyObject : NumericPropertyObject<uint>
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
        public UnsignedIntPropertyObject(string name, uint value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnsignedIntPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// PropertyObject implementation for <see cref="ulong"/>
    /// </summary>
    [Serializable]
    internal class UnsignedLongPropertyObject : NumericPropertyObject<ulong>
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
        public UnsignedLongPropertyObject(string name, ulong value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public UnsignedLongPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
