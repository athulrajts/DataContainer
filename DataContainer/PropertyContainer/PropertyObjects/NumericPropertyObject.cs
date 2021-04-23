using System.Runtime.Serialization;
using System.Xml;

namespace System.Configuration
{
    /// <summary>
    /// PropertyObject implementation for storing numeric types
    /// </summary>
    [Serializable]
    internal abstract class NumericPropertyObject<T> : PropertyObject<T>, INumericPropertyObject
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public NumericPropertyObject(string name, T value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Constructor for binary deserializatoin
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public NumericPropertyObject(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Increment = info.GetValue(nameof(Increment), typeof(T));
            Min = info.GetValue(nameof(Min), typeof(T));
            Max = info.GetValue(nameof(Max), typeof(T));
        }

        /// <summary>
        /// Increment for editors
        /// </summary>
        public object Increment { get; set; }

        /// <summary>
        /// Max value of <see cref="DataObject{T}.Value"/>
        /// </summary>
        public object Max { get; set; }

        /// <summary>
        /// Min value of <see cref="DataObject{T}.Value"/>
        /// </summary>
        public object Min { get; set; }

        /// <summary>
        /// Implementation for <see cref="DataObject.GetObjectData(SerializationInfo, StreamingContext)"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Increment), Increment);
            info.AddValue(nameof(Max), Max);
            info.AddValue(nameof(Min), Min);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject{T}.Validate(T)"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool Validate(T value)
        {
            return ValidateMin(value) && ValidateMax(value);
        }

        /// <summary>
        /// Implementation for <see cref="DataObject.WriteXmlContent(XmlWriter)"/>
        /// </summary>
        /// <param name="writer"></param>
        protected override void WriteXmlContent(XmlWriter writer)
        {
            base.WriteXmlContent(writer);

            if (Increment is T inc)
            {
                writer.WriteElementString(nameof(Increment), inc.ToString());
            }

            if (Max is T max)
            {
                writer.WriteElementString(nameof(Max), max.ToString());
            }

            if (Min is T min)
            {
                writer.WriteElementString(nameof(Min), min.ToString());
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
            if (base.ReadXmlElement(elementName, reader))
            {
                return true;
            }

            if (elementName == nameof(Increment))
            {
                Increment = reader.ReadElementContentAsInt();
                return true;
            }
            else if (elementName == nameof(Max))
            {
                Max = reader.ReadElementContentAsInt();
                return true;
            }
            else if (elementName == nameof(Min))
            {
                Min = reader.ReadElementContentAsInt();
            }

            return false;
        }

        /// <summary>
        /// Validate <paramref name="value"/> satisfies minimum value condition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ValidateMin(T value)
        {
            if (Min is null)
            {
                return true;
            }
            else if (Min is T t)
            {
                return value.CompareTo(t) > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Validate <paramref name="value"/> satisfies maximum value condition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ValidateMax(T value)
        {
            if (Min is null)
            {
                return true;
            }
            else if (Max is T t)
            {
                return value.CompareTo(t) < 0;
            }
            else
            {
                return false;
            }
        }
    }
}
