using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    /// <summary>
    /// PropertyObject implementation for storing folder paths
    /// There is no corresponding DataObject implementation, since the only
    /// Difference between <see cref="StringPropertyObject"/> <see cref="FilePropertyObject"/>
    /// is the Editor in PropertyGrid. <see cref="StringDataObject"/> should be used for storing folderpaths
    /// in <see cref="DataContainer"/>
    /// </summary>
    [Serializable]
    internal class FolderPropertyObject : StringPropertyObject, ICustomTypeProvider
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Folder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public FolderPropertyObject(string name, string value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FolderPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeProvider.GetCustomType"/>
        /// Can be used to use custom editor with property grid implementations
        /// </summary>
        /// <returns></returns>
        public Type GetCustomType()
        {
            return typeof(FolderPath);
        }
    }

}
