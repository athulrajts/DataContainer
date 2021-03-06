﻿using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace System.Configuration
{
    /// <summary>
    /// Base class for storing <see cref="System.Array"/> of primitive types
    /// </summary>
    [Serializable]
    internal abstract class ArrayPropertyObject : PropertyObject
    {
        /// <summary>
        /// Holds array object
        /// </summary>
        private Array _value;
        public Array Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        /// <summary>
        /// Element type of array
        /// </summary>
        public Type ElementType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ArrayPropertyObject(string name, Array value)
        {
            Name = name;
            Value = value;
            ElementType = value.GetType().GetElementType();
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ArrayPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Type arrayType = (TypeInfo)info.GetValue("Type", typeof(TypeInfo));
            Value = (Array)info.GetValue(nameof(Value), arrayType);
            ElementType = Value.GetType().GetElementType();
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.GetDataType"/>
        /// </summary>
        /// <returns></returns>
        public override Type GetDataType()
        {
            return Value.GetType();
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.GetValue"/>
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.SetValue(object)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool SetValue(object value)
        {
            if (value is Array a)
            {
                Value = a;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.GetObjectData(SerializationInfo, StreamingContext)"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Type", new TypeInfo(Value.GetType()));
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.ReadXmlElement(string, XmlReader)"/>
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool ReadXmlElement(string elementName, XmlReader reader)
        {
            if (base.ReadXmlElement(elementName, reader))
            {
                return true;
            }
            if (elementName == nameof(Value))
            {
                reader.Read();

                if (reader.NodeType == XmlNodeType.CDATA)
                {
                    StringValue = reader.Value;
                }
            }
            else if (elementName == nameof(TypeInfo))
            {
                ElementType = reader.ReadObjectXml<TypeInfo>();
            }

            return false;
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.CanWriteValueAsXmlAttribute"/>
        /// </summary>
        /// <returns></returns>
        protected override bool CanWriteValueAsXmlAttribute() { return false; }
    }

    /// <summary>
    /// Property Object implementation to store <see cref="Array"/> of dimension 1
    /// </summary>
    [Serializable]
    internal class Array1DPropertyObject : ArrayPropertyObject
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Array1D;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Array1DPropertyObject(string name, Array value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public Array1DPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.WriteXmlContent(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteXmlContent(XmlWriter writer)
        {
            base.WriteXmlContent(writer);

            if (Value != null)
            {
                StringBuilder sb = new StringBuilder();

                int length = Value.GetLength(0);

                // convert array to string
                // each value separated by ','
                for (int i = 0; i < length; i++)
                {
                    sb.Append(Value.GetValue(i));

                    if (i != length - 1)
                    {
                        sb.Append(',');
                    }
                }

                // write xml
                writer.WriteStartElement(nameof(Value));
                writer.WriteCData(sb.ToString());
                writer.WriteEndElement();

                // Write element type
                writer.WriteObjectXml(new TypeInfo(Value.GetType().GetElementType()));
            }
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.OnXmlReadingCompleted"/>
        /// </summary>
        protected override void OnXmlReadingCompleted()
        {
            var converter = TypeDescriptor.GetConverter(ElementType);

            var values = StringValue.Split(',');
            Value = Array.CreateInstance(ElementType, values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                Value.SetValue(converter.ConvertFromString(values[i]), i);
            }
        }
    }

    /// <summary>
    /// Property Object implementation to store <see cref="Array"/> of dimension 2
    /// </summary>
    [Serializable]
    internal class Array2DPropertyObject : ArrayPropertyObject
    {
        /// <summary>
        /// Implementation for <see cref="DataObject.Type"/>
        /// </summary>
        public override string Type => DataObjectType.Array2D;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Array2DPropertyObject(string name, Array value) : base(name, value) { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public Array2DPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="DataObject.WriteXmlContent(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteXmlContent(XmlWriter writer)
        {
            base.WriteXmlContent(writer);

            if (Value != null)
            {
                StringBuilder sb = new StringBuilder();

                int rows = Value.GetLength(0);
                int columns = Value.GetLength(1);

                // convert array to string
                // each row is separated by '\n'
                // each column separated by ','
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        sb.Append(Value.GetValue(i, j));

                        if (j != columns - 1)
                        {
                            sb.Append(',');
                        }
                    }

                    sb.Append('\n');
                }

                // write array string
                writer.WriteStartElement(nameof(Value));
                writer.WriteCData(sb.ToString());
                writer.WriteEndElement();

                // Write element type
                writer.WriteObjectXml(new TypeInfo(Value.GetType().GetElementType()));
            }
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.OnXmlReadingCompleted"/>
        /// </summary>
        protected override void OnXmlReadingCompleted()
        {
            var converter = TypeDescriptor.GetConverter(ElementType);

            var rows = StringValue.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int rowCount = rows.Length;
            int colCount = rows[0].Split(',').Length;

            Value = Array.CreateInstance(ElementType, rowCount, colCount);

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var cols = rows[rowIndex].Split(',');

                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    Value.SetValue(converter.ConvertFromString(cols[colIndex]), rowIndex, colIndex);
                }
            }
        }

    }
}
