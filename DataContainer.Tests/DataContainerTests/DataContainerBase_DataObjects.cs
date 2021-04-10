using System.Xml;
using Xunit;
using KEI.Infrastructure;
using System.ComponentModel;
using System;
using System.IO;
using System.Runtime.Serialization;
using KEI.Infrastructure.Helpers;
using DataContainer.Utils;

namespace DataContainer.Tests
{
    public class DataContainerBase_DataObjects
    {
        [Theory]
        [MemberData(nameof(Inputs.PrimitiveTypeValues), MemberType = typeof(Inputs))]
        public void DataObject_XML_Serialization_Primitive(object value)
        {
            DataObject orig = DataObjectFactory.GetPropertyObjectFor("A", value);

            // create xml
            StringWriter sw = new StringWriter();
            var writer = new XmlTextWriter(sw);
            orig.WriteXml(writer);

            // deserialize xml
            DataObject deserialized = (DataObject)FormatterServices.GetUninitializedObject(orig.GetType());
            var reader = new XmlTextReader(new StringReader(sw.ToString()));
            reader.Read();
            deserialized.ReadXml(reader);

            // assert equal
            object origValue = orig.GetValue();
            object newValue = deserialized.GetValue();

            if(origValue is DateTime o && newValue is DateTime n)
            {
                // check time and date separately, 
                Assert.Equal(o.Date, n.Date);
                Assert.Equal(o.TimeOfDay.Hours, n.TimeOfDay.Hours);
                Assert.Equal(o.TimeOfDay.Minutes, n.TimeOfDay.Minutes);
                Assert.Equal(o.TimeOfDay.Seconds, n.TimeOfDay.Seconds);

                //not serializing milliseconds so do check that
            }
            else
            {
                Assert.Equal(origValue, newValue);
            }

        }

        [Theory]
        [InlineData(typeof(ContainerDataObject))]
        [InlineData(typeof(XmlDataObject))]
        [InlineData(typeof(JsonDataObject))]
        [InlineData(typeof(ContainerPropertyObject))]
        [InlineData(typeof(XmlPropertyObject))]
        [InlineData(typeof(JsonPropertyObject))]
        public void DataObject_XML_Serialization_Object(Type type)
        {
            BindingTestObject origValue = new BindingTestObject();
            origValue.IntProperty = 35;

            DataObject orig = (DataObject)Activator.CreateInstance(type, "A", origValue);

            // create xml
            StringWriter sw = new StringWriter();
            var writer = new XmlTextWriter(sw);
            orig.WriteXml(writer);

            // deserialize xml
            DataObject deserialized = (DataObject)FormatterServices.GetUninitializedObject(type);
            var reader = new XmlTextReader(new StringReader(sw.ToString()));
            reader.Read();
            deserialized.ReadXml(reader);

            BindingTestObject deserializedValue = deserialized.GetValue() as BindingTestObject;

            // assert equal
            Assert.Equal(origValue.IntProperty, deserializedValue.IntProperty);
        }

        [Theory]
        [InlineData(typeof(PasswordDataObject))]
        [InlineData(typeof(PasswordPropertyObject))]
        public void DataObject_XML_Serialization_Password(Type type)
        {
            string password = "password";
            string encryptedPassword = EncryptionHelper.Encrypt(password);

            DataObject orig = (DataObject)Activator.CreateInstance(type, "A", password);

            // create xml
            StringWriter sw = new StringWriter();
            var writer = new XmlTextWriter(sw);
            orig.WriteXml(writer);

            // deserialize xml
            DataObject deserialized = (DataObject)FormatterServices.GetUninitializedObject(type);
            var reader = new XmlTextReader(new StringReader(sw.ToString()));
            reader.Read();
            deserialized.ReadXml(reader);

            // read enrypted string
            var reader2 = new XmlTextReader(new StringReader(sw.ToString()));
            reader2.Read();
            string storedValue = reader2.GetAttribute("value");

            Assert.Equal(password, deserialized.GetValue());
            Assert.Equal(encryptedPassword, storedValue);
        }

        [Theory]
        [InlineData(typeof(ShortPropertyObject), (short)22, (short)5, (short)60)]
        [InlineData(typeof(IntPropertyObject), (int)22, (int)5, (int)60)]
        [InlineData(typeof(LongPropertyObject), (long)22, (long)5, (long)60)]
        [InlineData(typeof(UnsignedShortPropertyObject), (ushort)22, (ushort)5, (ushort)60)]
        [InlineData(typeof(UnsignedIntPropertyObject), (uint)22, (uint)5, (uint)60)]
        [InlineData(typeof(UnsignedLongPropertyObject), (ulong)22, (ulong)5, (ulong)60)]
        public void NumericDataObject_SetValueReturnsFalseIfMinMaxValidationFails(Type type, object value, object minFail, object maxFail)
        {
            DataObject orig = (DataObject)Activator.CreateInstance(type, "A", value);

            INumericPropertyObject numeric = (INumericPropertyObject)orig;
            TypeConverter converter = TypeDescriptor.GetConverter(value);

            numeric.Max = converter.ConvertTo(50, value.GetType());
            numeric.Min = converter.ConvertTo(10, value.GetType());

            Assert.False(orig.SetValue(minFail));
            Assert.False(orig.SetValue(maxFail));
        }

        [Theory]
        [InlineData(typeof(ShortPropertyObject), (short)22, (short)5, (short)60)]
        [InlineData(typeof(IntPropertyObject), (int)22, (int)5, (int)60)]
        [InlineData(typeof(LongPropertyObject), (long)22, (long)5, (long)60)]
        [InlineData(typeof(UnsignedShortPropertyObject), (ushort)22, (ushort)5, (ushort)60)]
        [InlineData(typeof(UnsignedIntPropertyObject), (uint)22, (uint)5, (uint)60)]
        [InlineData(typeof(UnsignedLongPropertyObject), (ulong)22, (ulong)5, (ulong)60)]
        public void NumericDataObject_SetValueReturnsTrueIfMinMaxValidationPasses(Type type, object value, object minFail, object maxFail)
        {
            DataObject orig = (DataObject)Activator.CreateInstance(type, "A", value);

            INumericPropertyObject numeric = (INumericPropertyObject)orig;
            TypeConverter converter = TypeDescriptor.GetConverter(value);

            numeric.Max = converter.ConvertTo(100, value.GetType());
            numeric.Min = converter.ConvertTo(0, value.GetType());

            Assert.True(orig.SetValue(minFail));
            Assert.True(orig.SetValue(maxFail));
        }

        [Theory]
        [InlineData(typeof(ShortPropertyObject), (short)22, (short)5, (short)60)]
        [InlineData(typeof(IntPropertyObject), (int)22, (int)5, (int)60)]
        [InlineData(typeof(LongPropertyObject), (long)22, (long)5, (long)60)]
        [InlineData(typeof(UnsignedShortPropertyObject), (ushort)22, (ushort)5, (ushort)60)]
        [InlineData(typeof(UnsignedIntPropertyObject), (uint)22, (uint)5, (uint)60)]
        [InlineData(typeof(UnsignedLongPropertyObject), (ulong)22, (ulong)5, (ulong)60)]
        public void NumericDataObject_SetValueReturnsTrueIfMinMaxIsNotSet(Type type, object value, object minFail, object maxFail)
        {
            DataObject orig = (DataObject)Activator.CreateInstance(type, "A", value);

            Assert.True(orig.SetValue(minFail));
            Assert.True(orig.SetValue(maxFail));
        }

        [Fact]
        public void DataContainerBuilder_CanAddEmptyStringProperty()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("StringData", "")
                .Build();

            Assert.Equal(1, dc.Count);

            Assert.Equal(string.Empty, dc["StringData"]);

        }

        [Fact]
        public void PropertyContainerBuilder_CanAddEmptyStringProperty()
        {
            IDataContainer dc = PropertyContainerBuilder.Create()
                .Property("StringData", "")
                .Build();

            Assert.Equal(1, dc.Count);
            Assert.Equal(string.Empty, dc["StringData"]);

        }

        [Fact]
        public void PropertyObject_DisplayNameIsNameIfNotSet()
        {
            IDataContainer dc = PropertyContainerBuilder.Create()
                .Property("Data", 55)
                .Build();

            PropertyObject obj = dc.Find("Data") as PropertyObject;

            Assert.NotNull(obj.DisplayName);

            Assert.Equal("Data", obj.DisplayName);

            obj.SetDisplayName("The Data");

            Assert.NotEqual(obj.Name, obj.DisplayName);
            Assert.Equal("The Data", obj.DisplayName);
        }
    }
}
