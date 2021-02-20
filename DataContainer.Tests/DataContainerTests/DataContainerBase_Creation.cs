using Xunit;
using KEI.Infrastructure;
using System;
using DataContainer.Utils;

namespace DataContainer.Tests
{
    public class DataContainerBase_Creation
    {

        [Theory]
        [InlineData(typeof(POCO), 6)]
        [InlineData(typeof(NestedPOCO), 7)]
        [InlineData(typeof(UninitializedPOCO), 4)] // Should not create data for null properties, or should it ??
        public void DataContainerBuilder_CreateObject_MustCreatesObjectWithCorrectCount(Type t, int count)
        {
            var obj = Activator.CreateInstance(t);

            IDataContainer data = DataContainerBuilder.CreateObject("Untitiled", obj);

            Assert.Equal(count, data.Count);
        }

        [Theory]
        [InlineData(typeof(POCO), 6)]
        [InlineData(typeof(NestedPOCO), 7)]
        [InlineData(typeof(UninitializedPOCO), 4)] // Should not create data for null properties, or should it ??
        public void PropertyContainerBuilder_CreateObject_MustCreatesObjectWithCorrectCount(Type t, int count)
        {
            var obj = Activator.CreateInstance(t);

            IPropertyContainer property = PropertyContainerBuilder.CreateObject("Untitled", obj);

            Assert.Equal(count, property.Count);
        }

        [Theory]
        [InlineData(typeof(POCO))]
        public void DataContainerBuilder_CreateObject_MustCreateObjectWithCorrectValue(Type t)
        {
            var obj = Activator.CreateInstance(t);

            IDataContainer data = DataContainerBuilder.CreateObject("Untitiled", obj);

            foreach (var item in data)
            {
                var expected = t.GetProperty(item.Name).GetValue(obj);
                var actual = item.GetValue();
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(typeof(POCO))]
        public void PropertyContainerBuilder_CreateObject_MustCreateObjectWithCorrectValue(Type t)
        {
            var obj = Activator.CreateInstance(t);

            IPropertyContainer property = PropertyContainerBuilder.CreateObject("Untitled", obj);

            foreach (var item in property)
            {
                var expected = t.GetProperty(item.Name).GetValue(obj);
                var actual = item.GetValue();
                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [InlineData(DataObjectType.Short, typeof(ShortDataObject), typeof(short))]
        [InlineData(DataObjectType.Integer, typeof(IntDataObject), typeof(int))]
        [InlineData(DataObjectType.Long, typeof(LongDataObject), typeof(long))]
        [InlineData(DataObjectType.UShort, typeof(UnsignedShortDataObject), typeof(ushort))]
        [InlineData(DataObjectType.UInteger, typeof(UnsignedIntDataObject), typeof(uint))]
        [InlineData(DataObjectType.ULong, typeof(UnsignedLongDataObject), typeof(ulong))]
        [InlineData(DataObjectType.Float, typeof(FloatDataObject), typeof(float))]
        [InlineData(DataObjectType.Double, typeof(DoubleDataObject), typeof(double))]
        [InlineData(DataObjectType.Char, typeof(CharDataObject), typeof(char))]
        [InlineData(DataObjectType.String, typeof(StringDataObject), typeof(string))]
        [InlineData(DataObjectType.Password, typeof(PasswordDataObject), typeof(string))]
        [InlineData(DataObjectType.DateTime, typeof(DateTimeDataObject), typeof(DateTime))]
        [InlineData(DataObjectType.TimeSpan, typeof(TimeSpanDataObject), typeof(TimeSpan))]
        [InlineData(DataObjectType.Color, typeof(ColorDataObject), typeof(Color))]
        [InlineData(DataObjectType.Point, typeof(PointDataObject), typeof(Point))]
        [InlineData(DataObjectType.Array1D, typeof(Array1DDataObject), typeof(int[]))]
        [InlineData(DataObjectType.Array2D, typeof(Array2DDataObject), typeof(int[,]))]
        public void DataObjectFactory_GetDataObject_CreatesCorrectObject(string type, Type resultType, Type valueType)
        {
            var valueProvider = new DefaultValueProvider();
            object defaultValue = valueProvider.GetValue(valueType);

            DataObject obj = DataObjectFactory.GetDataObject(type, "a", defaultValue);

            Assert.Equal(resultType, obj.GetType());
            Assert.Equal(defaultValue, obj.GetValue());
        }

        [Theory]
        [InlineData(DataObjectType.Short, typeof(ShortPropertyObject), typeof(short))]
        [InlineData(DataObjectType.Integer, typeof(IntPropertyObject), typeof(int))]
        [InlineData(DataObjectType.Long, typeof(LongPropertyObject), typeof(long))]
        [InlineData(DataObjectType.UShort, typeof(UnsignedShortPropertyObject), typeof(ushort))]
        [InlineData(DataObjectType.UInteger, typeof(UnsignedIntPropertyObject), typeof(uint))]
        [InlineData(DataObjectType.ULong, typeof(UnsignedLongPropertyObject), typeof(ulong))]
        [InlineData(DataObjectType.Float, typeof(FloatPropertyObject), typeof(float))]
        [InlineData(DataObjectType.Double, typeof(DoublePropertyObject), typeof(double))]
        [InlineData(DataObjectType.Char, typeof(CharPropertyObject), typeof(char))]
        [InlineData(DataObjectType.String, typeof(StringPropertyObject), typeof(string))]
        [InlineData(DataObjectType.File, typeof(FilePropertyObject), typeof(string))]
        [InlineData(DataObjectType.Folder, typeof(FolderPropertyObject), typeof(string))]
        [InlineData(DataObjectType.Password, typeof(PasswordPropertyObject), typeof(string))]
        [InlineData(DataObjectType.DateTime, typeof(DateTimePropertyObject), typeof(DateTime))]
        [InlineData(DataObjectType.TimeSpan, typeof(TimeSpanPropertyObject), typeof(TimeSpan))]
        [InlineData(DataObjectType.Color, typeof(ColorPropertyObject), typeof(Color))]
        [InlineData(DataObjectType.Point, typeof(PointPropertyObject), typeof(Point))]
        [InlineData(DataObjectType.Array1D, typeof(Array1DPropertyObject), typeof(int[]))]
        [InlineData(DataObjectType.Array2D, typeof(Array2DPropertyObject), typeof(int[,]))]
        public void DataObjectFactory_GetPropertyObject_CreatesCorrectObject(string type, Type resultType, Type valueType)
        {
            var valueProvider = new DefaultValueProvider();
            object defaultValue = valueProvider.GetValue(valueType);

            DataObject obj = DataObjectFactory.GetPropertyObject(type, "a", defaultValue);

            Assert.Equal(resultType, obj.GetType());
            Assert.Equal(defaultValue, obj.GetValue());
        }

        [Theory]
        [InlineData(DataObjectType.Short, typeof(short))]
        [InlineData(DataObjectType.Integer, typeof(int))]
        [InlineData(DataObjectType.Long, typeof(long))]
        [InlineData(DataObjectType.UShort, typeof(ushort))]
        [InlineData(DataObjectType.UInteger, typeof(uint))]
        [InlineData(DataObjectType.ULong, typeof(ulong))]
        [InlineData(DataObjectType.Float, typeof(float))]
        [InlineData(DataObjectType.Double, typeof(double))]
        [InlineData(DataObjectType.String, typeof(string))]
        [InlineData(DataObjectType.DateTime, typeof(DateTime))]
        [InlineData(DataObjectType.TimeSpan, typeof(TimeSpan))]
        [InlineData(DataObjectType.Point, typeof(Point))]
        [InlineData(DataObjectType.Color, typeof(Color))]
        [InlineData(DataObjectType.Array1D, typeof(int[]))]
        [InlineData(DataObjectType.Array2D, typeof(int[,]))]
        public void DataObjectFactory_GetDataObjectFor_CreatesCorrectObject(string dataObjectType, Type valueType)
        {
            var valueProvider = new DefaultValueProvider();
            object defaultValue = valueProvider.GetValue(valueType);

            DataObject obj = DataObjectFactory.GetDataObjectFor("name", defaultValue);
            Assert.Equal(dataObjectType, obj.Type);
        }

        [Theory]
        [InlineData(DataObjectType.Short, typeof(short))]
        [InlineData(DataObjectType.Integer, typeof(int))]
        [InlineData(DataObjectType.Long, typeof(long))]
        [InlineData(DataObjectType.UShort, typeof(ushort))]
        [InlineData(DataObjectType.UInteger, typeof(uint))]
        [InlineData(DataObjectType.ULong, typeof(ulong))]
        [InlineData(DataObjectType.Float, typeof(float))]
        [InlineData(DataObjectType.Double, typeof(double))]
        [InlineData(DataObjectType.String, typeof(string))]
        [InlineData(DataObjectType.DateTime, typeof(DateTime))]
        [InlineData(DataObjectType.TimeSpan, typeof(TimeSpan))]
        [InlineData(DataObjectType.Point, typeof(Point))]
        [InlineData(DataObjectType.Color, typeof(Color))]
        [InlineData(DataObjectType.Array1D, typeof(int[]))]
        [InlineData(DataObjectType.Array2D, typeof(int[,]))]
        public void DataObjectFactory_GetPropertyObjectFor_CreatesCorrectObject(string dataObjectType, Type valueType)
        {
            var valueProvider = new DefaultValueProvider();
            object defaultValue = valueProvider.GetValue(valueType);

            DataObject obj = DataObjectFactory.GetPropertyObjectFor("name", defaultValue);
            Assert.Equal(dataObjectType, obj.Type);
        }
    }
}
