using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Configuration
{
    public abstract partial class DataContainerBase : IXmlSerializable
    {

        /// <summary>
        /// Get an in unintialized <see cref="DataObject"/> based on <paramref name="type"/>
        /// values will be populated from xml when <see cref="DataObject.ReadXml(XmlReader)"/> is called
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual DataObject GetUnitializedDataObject(string type)
        {
            return DataObjectFactory.GetDataObject(type);
        }

        /// <summary>
        /// Implementation for <see cref="IXmlSerializable.GetSchema"/>
        /// according to internet you should return null.
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema() => null;

        /// <summary>
        /// Implementation for <see cref="IXmlSerializable.ReadXml(XmlReader)"/>
        /// </summary>
        /// <param name="reader"></param>
        public virtual void ReadXml(XmlReader reader)
        {
            // Read attribute name
            if (reader.GetAttribute(DataObject.KEY_ATTRIBUTE) is string s)
            {
                Name = s;
            }

            // read to content
            reader.Read();

            while (reader.EOF == false)
            {
                // nothing of value skip.
                if (reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                }

                // read UnderlyingType
                // for IDataContainers created from taking CLR objects as reference
                else if (reader.Name == nameof(TypeInfo))
                {
                    UnderlyingType = reader.ReadObjectXml<TypeInfo>();
                }

                // We're reading a DataObject implementation
                else
                {
                    string dataObjectType = reader.GetAttribute(DataObject.TYPE_ID_ATTRIBUTE);

                    /// Get uninitialized Object based on type attribute
                    if (GetUnitializedDataObject(dataObjectType) is DataObject obj)
                    {
                        /// need to create a new XmlReader so that <see cref="DataObject"/> implementation
                        /// can read till end.
                        using (var newReader = XmlReader.Create(new StringReader(reader.ReadOuterXml())))
                        {
                            // move to content
                            newReader.Read();

                            obj.ReadXml(newReader);

                            /// If we get a <see cref="NotSupportedDataObject"/>, don't add it, because it's just a dummy object
                            /// which reads all the xml and does nothing with it.
                            /// 3rd party implementation of <see cref="DataObject"/> should be registered by using
                            /// <see cref="DataObjectFactory.RegisterDataObject{T}"/> or <see cref="DataObjectFactory.RegisterPropertyObject{T}"/> methods
                            /// so that <see cref="DataContainerBase"/> can create those objects to read
                            if (obj.Type != DataObjectType.NotSupported)
                            {
                                Add(obj);
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Implementation for <see cref="IXmlSerializable.WriteXml(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteXml(XmlWriter writer)
        {
            // write name as attribute if we have a name
            if (string.IsNullOrEmpty(Name) == false)
            {
                writer.WriteAttributeString(DataObject.KEY_ATTRIBUTE, Name);
            }

            // write underlying type if this was created from a CLR object
            if (UnderlyingType != null)
            {
                writer.WriteObjectXml(UnderlyingType);
            }

            // write all the dataobjects
            foreach (var obj in this)
            {
                obj.WriteXml(writer);
            }
        }

    }
}
