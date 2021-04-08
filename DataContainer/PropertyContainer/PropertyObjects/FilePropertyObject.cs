using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace KEI.Infrastructure
{
    [Serializable]
    public class Filter
    {
        public string Description { get; set; }
        public string Extension { get; set; }

        public Filter(string desc, string ext)
        {
            Description = desc;
            Extension = ext;
        }
    }

    [Serializable]
    public class FilterCollection : List<Filter> { }

    public interface IFileProperty
    {
        FilterCollection Filters { get; }
    }

    /// <summary>
    /// PropertyObject implementation for storing file paths
    /// There is no corresponding DataObject implementation, since the only
    /// Difference between <see cref="StringPropertyObject"/> <see cref="FilePropertyObject"/>
    /// is the Editor in PropertyGrid. <see cref="StringDataObject"/> should be used for storing filepaths
    /// in <see cref="DataContainer"/>
    /// </summary>
    [Serializable]
    internal class FilePropertyObject : StringPropertyObject, IFileProperty, ICustomTypeProvider, ICustomValueProvider
    {
        const string FILTER_DESCRIPTION_ATTRIBUTE = "desc";
        const string FILTER_EXTENSTION_ATTRIBUTE = "ext";
        const string FILTER_ELEMENT = "Filter";
        private IDataContainer customData;

        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.File;

        /// <summary>
        /// Used by the editor, which should be a file browse dialog
        /// <see cref="Tuple{T1, T2}.Item1"/> is Description of filter
        /// <see cref="Tuple{T1, T2}.Item2"/> is Extension of filter
        /// </summary>
        public FilterCollection Filters { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="filters"></param>
        public FilePropertyObject(string name, string value) : base(name, value)
        {
            Filters = new FilterCollection();
            customData = new DataContainer
            {
                { "ActualValue", Value },
                { "Filters", Filters }
            };
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FilePropertyObject(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Filters = (FilterCollection)info.GetValue(nameof(Filters), typeof(FilterCollection));
        }

        /// <summary>
        /// Implmentation for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Filters), Filters);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.WriteXmlContent(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteXmlContent(XmlWriter writer)
        {
            base.WriteXmlContent(writer);

            foreach (var filter in Filters)
            {
                writer.WriteStartElement(FILTER_ELEMENT);
                writer.WriteAttributeString(FILTER_DESCRIPTION_ATTRIBUTE, filter.Description);
                writer.WriteAttributeString(FILTER_EXTENSTION_ATTRIBUTE, filter.Extension);
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.ReadXmlElement(string, XmlReader)"/>
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool ReadXmlElement(string elementName, XmlReader reader)
        {
            if (base.ReadXmlElement(elementName, reader) == true)
            {
                return true;
            }

            if (elementName == FILTER_ELEMENT)
            {
                string desc = reader.GetAttribute(FILTER_DESCRIPTION_ATTRIBUTE);
                string ext = reader.GetAttribute(FILTER_EXTENSTION_ATTRIBUTE);

                Filters.Add(new Filter(desc, ext));

                reader.Read();

                return true;
            }

            return false;
        }

        protected override void OnXmlReadingCompleted()
        {
            customData.PutValue("ActualValue", Value);
            customData.PutValue("Filters", Filters);

            customData.SetBinding(() => Value, BindingMode.OneWay);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.InitializeObject"/>
        /// </summary>
        protected override void InitializeObject()
        {
            Filters = new FilterCollection();
            customData = new DataContainer();
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeProvider.GetCustomType"/>
        /// Can be used to use custom editor with property grid implementations
        /// </summary>
        /// <returns></returns>
        public Type GetCustomType()
        {
            return typeof(FilePath);
        }

        public object GetCustomValue()
        {
            customData.PutValue("ActualValue", Value);
            customData.PutValue("Filters", Filters);

            return customData;
        }
    }
}
