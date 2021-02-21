using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;
using DataContainer.Utils;
using KEI.Infrastructure;
using KEI.Infrastructure.Validation;
using System.Linq;

namespace DataContainer.Tests
{
    public class DataContainerBase_AccessAndManipulation
    {
        [Fact]
        public void DataContainerBase_CanAccessChildDataFromRoot()
        {
            DataContainerBase Root = (DataContainerBase)DataContainerBuilder.Create("Root")
                .DataContainer("Child", b => b
                    .DataContainer("GrandChild", c => c
                        .Data("A", 1)
                        .Data("B", 2)))
                .Build();

            int a = Root.GetValue(new Key<int>("Child.GrandChild.A"));
            int b = Root.GetValue(new Key<int>("Child.GrandChild.B"));

            Assert.Equal(1, a);
            Assert.Equal(2, b);
        }

        [Theory]
        [InlineData(typeof(POCO))]
        public void DataContainerBase_Morph_MustRecreatesSameObject(Type t)
        {
            var obj = Activator.CreateInstance(t);

            DataContainerBase data = (DataContainerBase)DataContainerBuilder.CreateObject("Untitiled", obj);

            var morphedData = data.Morph();

            foreach (var prop in t.GetProperties())
            {
                var expected = prop.GetValue(obj);
                var actualData = prop.GetValue(morphedData);

                Assert.Equal(expected, actualData);
            }

        }

        [Fact]
        public void DataContainerBase_Morph_MustRecreateObjectWithIDataContainerProperty()
        {
            var obj = new Component();

            IDataContainer dc = PropertyContainerBuilder.CreateObject("", obj);

            string xml = XmlHelper.SerializeToString(dc);

            IDataContainer dc2 = XmlHelper.DeserializeFromString<PropertyContainer>(xml);

            Component obj2 = dc2.Morph<Component>();

            Assert.NotNull(obj2.TestParameters);
            Assert.Equal(obj.TestParameters.Count, obj2.TestParameters.Count);

        }


        [Theory]
        [InlineData(typeof(POCO))]
        public void DataContainerBase_Store_MustDeserialize(Type t)
        {
            var obj = Activator.CreateInstance(t);

            DataContainerBase data = (DataContainerBase)DataContainerBuilder.CreateObject("Untitiled", obj);

            var serializedData = XmlHelper.SerializeToString(data);

            var recreatedData = XmlHelper.DeserializeFromString<KEI.Infrastructure.DataContainer>(serializedData);

            Assert.NotNull(recreatedData);

            var morphedData = recreatedData.Morph();

            foreach (var prop in t.GetProperties())
            {
                var expected = prop.GetValue(obj);
                var actualData = prop.GetValue(morphedData);

                Assert.Equal(expected, actualData);
            }

        }

        [Fact]
        public void DataContainerBase_Get_MustGetCorrectValue()
        {
            const string PROP_NAME = "IntProperty";

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, 42)
                .Build();

            int prop = 0;
            bool containsProperty = property.GetValue(PROP_NAME, ref prop);

            Assert.True(containsProperty);
            Assert.NotEqual(0, prop);
            Assert.Equal(42, prop);

            int prop2 = property.GetValue<int>(PROP_NAME);

            Assert.NotEqual(0, prop2);
            Assert.Equal(42, prop2);
        }

        [Fact]
        public void DataContainerBase_Get_MustReturnDefaultIfNotPresent()
        {
            const string PROP_NAME = "IntProperty";

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, 42)
                .Build();

            int prop = 0;
            bool containsProperty = property.GetValue("blah", ref prop);

            Assert.False(containsProperty);
            Assert.Equal(default, prop);

            int prop2 = property.GetValue<int>("blah");

            Assert.Equal(default, prop2);
        }

        [Fact]
        public void DataContainerBase_Set_MustSetValue()
        {
            const string PROP_NAME = "IntProperty";
            const int VALUE = 42;
            const int SET_VALUE = 14;

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, VALUE)
                .Build();

            property.SetValue(PROP_NAME, SET_VALUE);

            int value = 0;
            property.GetValue(PROP_NAME, ref value);
            int value2 = property.GetValue<int>(PROP_NAME);

            Assert.NotEqual(VALUE, value);
            Assert.NotEqual(VALUE, value2);

            Assert.Equal(SET_VALUE, value);
            Assert.Equal(SET_VALUE, value);
        }

        [Fact]
        public void DataContainerBase_Put_MustAddValueIfNotPresent()
        {
            const string PROP_NAME = "IntProperty";
            const int SET_VALUE = 14;

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create().Build();

            Assert.False(property.ContainsData(PROP_NAME));

            property.PutValue(PROP_NAME, SET_VALUE);

            Assert.True(property.ContainsData(PROP_NAME));

            int value = 0;
            property.GetValue(PROP_NAME, ref value);

            Assert.Equal(SET_VALUE, value);
        }

        [Fact]
        public void DataContainerBase_Put_MustUpdateValueIfPresent()
        {
            const string PROP_NAME = "IntProperty";
            const int VALUE = 42;
            const int SET_VALUE = 14;

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, VALUE)
                .Build();

            int originalValue = 0;
            property.GetValue(PROP_NAME, ref originalValue);

            Assert.True(property.ContainsData(PROP_NAME));

            property.PutValue(PROP_NAME, SET_VALUE);

            Assert.True(property.ContainsData(PROP_NAME));

            int value = 0;
            property.GetValue(PROP_NAME, ref value);

            Assert.NotEqual(originalValue, value);
            Assert.Equal(SET_VALUE, value);
        }

        [Fact]
        public void DataContainerBase_Morph_T_CanMoverToObject()
        {
            DataContainerBase DC = (DataContainerBase)DataContainerBuilder.Create("POCO")
                .Data(nameof(POCO.IntProperty), 12)
                .Data(nameof(POCO.StringProperty), "Hello")
                .Build();

            POCO morphed = DC.Morph<POCO>();

            Assert.Equal(12, morphed.IntProperty);
            Assert.Equal("Hello", morphed.StringProperty);
        }

        [Theory]
        [MemberData(nameof(Inputs.PrimitiveTypeValues), MemberType = typeof(Inputs))]
        public void DataContainerBase_Store_Binary_ValueTypes(object value)
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", value)
                .Build();

            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, dc);

            stream.Position = 0;

            var deserialized = (IDataContainer)formatter.Deserialize(stream);

            var oldValue = dc.Find("A");
            var newValue = deserialized.Find("A");

            Assert.Equal(oldValue.GetValue(), newValue.GetValue());
        }

        [Theory]
        [MemberData(nameof(Inputs.PrimitiveTypeValues), MemberType = typeof(Inputs))]
        public void PropertyContainerBase_Store_Binary_ValueTypes(object value)
        {
            IDataContainer dc = PropertyContainerBuilder.Create()
                .Property("A", value, p => p.SetCategory("A").SetDescription("B").SetDisplayName("c"))
                .Build();

            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, dc);

            stream.Position = 0;

            var deserialized = (IDataContainer)formatter.Deserialize(stream);

            var oldValue = dc.Find("A") as PropertyObject;
            var newValue = deserialized.Find("A") as PropertyObject;

            Assert.Equal(oldValue.GetValue(), newValue.GetValue());
            Assert.Equal(oldValue.Category, newValue.Category);
            Assert.Equal(oldValue.Description, newValue.Description);
            Assert.Equal(oldValue.DisplayName, newValue.DisplayName);
        }

        [Fact]
        public void DataContainerBase_Store_Binary_ReferenceTypes()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", new POCO { IntProperty = 55 })
                .Build();

            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, dc);

            stream.Position = 0;

            var deserialized = (IDataContainer)formatter.Deserialize(stream);

            POCO orig = dc["A"] as POCO;
            POCO @new = deserialized["A"] as POCO;

            Assert.NotSame(orig, @new);
            Assert.Equal(orig.IntProperty, @new.IntProperty);

        }

        [Fact]
        public void PropertyContainerBase_Store_Validations()
        {
            IPropertyContainer dc = PropertyContainerBuilder.Create()
                .Property("A", 1 , b => b
                    .SetValidation(ValidationBuilder.Create().Range(1,100))
                    .SetCategory("Test"))
                .Build();

            IFormatter formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, dc);


            stream.Position = 0;

            var deserialized = (IDataContainer)formatter.Deserialize(stream);

            PropertyObject obj = deserialized.Find("A") as PropertyObject;

            Assert.IsType<RangeValidator>(obj.Validation.Rules[0]);

            RangeValidator v = obj.Validation.Rules[0] as RangeValidator;

            Assert.Equal(1, v.MinValue);
            Assert.Equal(100, v.MaxValue);
        }

        [Fact]
        public void DataContainerBase_ContainsDataReturnsTrueIfPresent()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .Build();

            Assert.True(dc.ContainsData("A"));
        }

        [Fact]
        public void DataContainerBase_ContainsDataReturnsFalseIfNotPresent()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .Build();

            Assert.False(dc.ContainsData("B"));
        }

        [Fact]
        public void DataContainerBase_ContainsDataReturnsTrueIfPresent_Recursive()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .DataContainer("B", b => b
                    .DataContainer("BB", c => c
                        .Data("BB1",2)))
                .Build();

            Assert.True(dc.ContainsData("B.BB.BB1"));
        }

        [Fact]
        public void DataContainerBase_ContainsDataReturnsFalseIfNotPresent_Recursive()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .DataContainer("B", b => b
                    .DataContainer("BB", c => c
                        .Data("BB1", 2)))
                .Build();

            Assert.False(dc.ContainsData("B.BB.BB2"));
        }

        [Fact]
        public void DataContainerBase_RemoveWorks()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .Build();

            Assert.True(dc.ContainsData("A"));

            dc.Remove("A");

            Assert.False(dc.ContainsData("A"));
        }

        [Fact]
        public void DataContainerBase_RemoveWorksRecursively()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .DataContainer("B", b => b
                    .DataContainer("BB", c => c
                        .Data("BB1", 2)))
                .Build();

            Assert.True(dc.ContainsData("B.BB.BB1"));

            dc.RecursiveRemove("B.BB.BB1");

            Assert.False(dc.ContainsData("B.BB.BB1"));
        }

        [Theory]
        [MemberData(nameof(Inputs.PrimitiveTypeValues), MemberType =typeof(Inputs))]
        public void DataContainerBase_AddWorks(object value)
        {
            IDataContainer dc = DataContainerBuilder.Create().Build();

            Assert.Equal(0, dc.Count);

            dc.Add(DataObjectFactory.GetDataObjectFor("A", value));

            Assert.Equal(1, dc.Count);
            Assert.True(dc.ContainsData("A"));
        }

        [Fact]
        public void PropertyContainerBase_RemoveWorks()
        {
            IDataContainer dc = PropertyContainerBuilder.Create()
                .Property("A", 1)
                .Build();

            Assert.True(dc.ContainsData("A"));

            dc.Remove("A");

            Assert.False(dc.ContainsData("A"));
        }

        [Fact]
        public void PropertyContainerBase_RemoveWorksRecursively()
        {
            IDataContainer dc = PropertyContainerBuilder.Create()
                .Property("A", 1)
                .PropertyContainer("B", b => b
                    .PropertyContainer("BB", c => c
                        .Property("BB1", 2)))
                .Build();

            Assert.True(dc.ContainsData("B.BB.BB1"));

            dc.RecursiveRemove("B.BB.BB1");

            Assert.False(dc.ContainsData("B.BB.BB1"));
        }

        [Theory]
        [MemberData(nameof(Inputs.PrimitiveTypeValues), MemberType =typeof(Inputs))]
        public void PropertyContainerBase_AddWorks(object value)
        {
            IDataContainer dc = PropertyContainerBuilder.Create().Build();

            Assert.Equal(0, dc.Count);

            dc.Add(DataObjectFactory.GetPropertyObjectFor("A", value));

            Assert.Equal(1, dc.Count);
            Assert.True(dc.ContainsData("A"));
        }

        [Fact]
        public void DataContainer_GetKeysReturnsTopLevelKeys()
        {
            IDataContainer dc = DataContainerBuilder.Create()
                .Data("A", 1)
                .DataContainer("B", b => b
                    .DataContainer("BB", c => c
                        .Data("BB1", 2)))
                .Build();

            var keys = dc.GetKeys().ToList();

            Assert.Equal(2, keys.Count);
            Assert.DoesNotContain("B.BB.BB1", keys);
        }

        [Fact]
        public void PropertyContainer_GetKeysReturnsTopLevelKeys()
        {
            IDataContainer dc = PropertyContainerBuilder.Create()
                .Property("A", 1)
                .PropertyContainer("B", b => b
                    .PropertyContainer("BB", c => c
                        .Property("BB1", 2)))
                .Build();

            var keys = dc.GetKeys().ToList();

            Assert.Equal(2, keys.Count);
            Assert.DoesNotContain("B.BB.BB1", keys);
        }
    }
}
