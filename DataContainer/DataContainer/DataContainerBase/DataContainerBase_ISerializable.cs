using System.Runtime.Serialization;

namespace System.Configuration
{
    public abstract partial class DataContainerBase : ISerializable
    {

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Name), Name);
            info.AddValue(nameof(UnderlyingType), UnderlyingType);
            info.AddValue(nameof(Count), Count);

            int count = 0;
            foreach (var item in this)
            {
                info.AddValue($"Data_{count++}", item);
            }
        }
    }
}
