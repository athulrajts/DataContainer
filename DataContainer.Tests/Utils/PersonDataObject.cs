using KEI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DataContainer.Tests.Utils
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PersonDataObject : DataObject
    {
        public PersonDataObject(string name, Person value)
        {
            Name = name;
            Value = value;
        }

        public PersonDataObject(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            Value = new Person();
            Value.FirstName = info.GetString(nameof(Person.FirstName));
            Value.LastName = info.GetString(nameof(Person.LastName));
        }

        private Person _value;
        public Person Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public override string Type => "prsn";

        public override Type GetDataType() => typeof(Person);

        public override object GetValue()
        {
            return Value;
        }

        public override bool SetValue(object value)
        {
            if(value is Person p)
            {
                Value = p;
                return true;
            }

            return false;
        }

        protected override bool CanWriteValueAsXmlAttribute() => false;

        protected override void WriteXmlAttributes(XmlWriter writer)
        {
            base.WriteXmlAttributes(writer);

            writer.WriteAttributeString(nameof(Person.FirstName), Value.FirstName);
            writer.WriteAttributeString(nameof(Person.LastName), Value.LastName);
        }

        protected override void InitializeObject()
        {
            Value = new Person();
        }

        protected override void ReadXmlAttributes(XmlReader reader)
        {
            base.ReadXmlAttributes(reader);

            Value.FirstName = reader.GetAttribute(nameof(Person.FirstName));
            Value.LastName = reader.GetAttribute(nameof(Person.LastName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Person.FirstName), Value.FirstName);
            info.AddValue(nameof(Person.LastName), Value.LastName);
        }
    }
}
