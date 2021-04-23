using Xunit;
using System.Configuration;
using DataContainer.Utils;

namespace DataContainer.Tests
{
    public class DataContainerBase_Binding
    {
        [Fact]
        public void DataContainerBase_SetBinding_MustBindOneWay()
        {
            const string PROP_NAME = "IntProperty";
            const int VALUE = 42;
            const int NEW_VALUE = 14;

            IPropertyContainer pc = PropertyContainerBuilder.Create()
                .Property(PROP_NAME, VALUE)
                .Build();

            var bindingTarget = new BindingTestObject();

            Assert.NotEqual(VALUE, bindingTarget.IntProperty);

            pc.SetBinding(PROP_NAME, () => bindingTarget.IntProperty, BindingMode.OneWay);

            Assert.Equal(VALUE, bindingTarget.IntProperty);

            pc.SetValue(PROP_NAME, NEW_VALUE);

            Assert.Equal(NEW_VALUE, bindingTarget.IntProperty);

            bindingTarget.IntProperty = 32;

            int propertyValue = 0;
            pc.GetValue(PROP_NAME, ref propertyValue);

            pc.RemoveBinding(PROP_NAME, () => bindingTarget.IntProperty);

            Assert.NotEqual(32, propertyValue);
            Assert.Equal(NEW_VALUE, propertyValue);

        }

        [Fact]
        public void DataContainerBase_SetBinding_MustBindTwoWay()
        {
            const string PROP_NAME = "IntProperty";
            const int VALUE = 42;
            const int NEW_VALUE = 14;

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, VALUE)
                .Build();

            var bindingTarget = new BindingTestObject();

            Assert.NotEqual(VALUE, bindingTarget.IntProperty);

            property.SetBinding(PROP_NAME, () => bindingTarget.IntProperty, BindingMode.TwoWay);

            Assert.Equal(VALUE, bindingTarget.IntProperty);

            property.SetValue(PROP_NAME, NEW_VALUE);

            Assert.Equal(NEW_VALUE, bindingTarget.IntProperty);

            bindingTarget.IntProperty = 32;

            int propertyValue = 0;
            property.GetValue(PROP_NAME, ref propertyValue);

            property.RemoveBinding(PROP_NAME, () => bindingTarget.IntProperty);

            Assert.Equal(propertyValue, bindingTarget.IntProperty);
        }

        [Fact]
        public void DataContainerBase_SetBinding_MustBindOneWayToSource()
        {
            const string PROP_NAME = "IntProperty";
            const int VALUE = 42;
            const int NEW_VALUE = 14;

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, VALUE)
                .Build();

            var bindingTarget = new BindingTestObject();

            Assert.NotEqual(VALUE, bindingTarget.IntProperty);

            property.SetBinding(PROP_NAME, () => bindingTarget.IntProperty, BindingMode.OneWayToSource);

            Assert.Equal(VALUE, bindingTarget.IntProperty);

            property.SetValue(PROP_NAME, NEW_VALUE);

            Assert.NotEqual(NEW_VALUE, bindingTarget.IntProperty);
            Assert.Equal(VALUE, bindingTarget.IntProperty);

            bindingTarget.IntProperty = 32;

            int propertyValue = 0;
            property.GetValue(PROP_NAME, ref propertyValue);

            property.RemoveBinding(PROP_NAME, () => bindingTarget.IntProperty);

            Assert.Equal(propertyValue, bindingTarget.IntProperty);
        }

        [Fact]
        public void DataContainerBase_SetBinding_MustBindOneTime()
        {
            const string PROP_NAME = "IntProperty";
            const int VALUE = 42;
            const int NEW_VALUE = 14;

            DataContainerBase property = (DataContainerBase)PropertyContainerBuilder.Create()
                .Property(PROP_NAME, VALUE)
                .Build();

            var bindingTarget = new BindingTestObject();

            Assert.NotEqual(VALUE, bindingTarget.IntProperty);

            property.SetBinding(PROP_NAME, () => bindingTarget.IntProperty, BindingMode.OneTime);

            Assert.Equal(VALUE, bindingTarget.IntProperty);

            property.SetValue(PROP_NAME, NEW_VALUE);

            Assert.NotEqual(NEW_VALUE, bindingTarget.IntProperty);
            Assert.Equal(VALUE, bindingTarget.IntProperty);

            bindingTarget.IntProperty = 32;

            int propertyValue = 0;
            property.GetValue(PROP_NAME, ref propertyValue);

            property.RemoveBinding(PROP_NAME, () => bindingTarget.IntProperty);

            Assert.NotEqual(32, propertyValue);
            Assert.Equal(NEW_VALUE, propertyValue);
        }
    }
}
