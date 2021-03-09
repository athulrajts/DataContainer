using Xunit;
using KEI.Infrastructure;
using DataContainer.Utils;
using System.Collections.Specialized;

namespace DataContainer.Tests
{
    public class DataContainerBase_ChangeNotification
    {

        [Fact]
        public void IDataContainer_ShouldRaisePropertyChanged()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Build();

            var listener = new PropertyChangedListener(A);

            A["A"] = 55;
            Assert.Equal("A", listener.LastChangedProperty);
            Assert.Single(listener.PropertiesChanged);

            A["B"] = 23;
            Assert.Equal("B", listener.LastChangedProperty);
            Assert.Equal(2, listener.PropertiesChanged.Count);

            A["C"] = 29;
            Assert.Equal("C", listener.LastChangedProperty);
            Assert.Equal(3, listener.PropertiesChanged.Count);

            Assert.Contains("A", listener.PropertiesChanged);
            Assert.Contains("B", listener.PropertiesChanged);
            Assert.Contains("C", listener.PropertiesChanged);
        }

        [Fact]
        public void IDataContainer_ShouldRaisePropertyChangedForNestedContainers()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("AA", b => b
                    .Data("A1", 23)
                    .Data("A2", 24)
                    .DataContainer("AAA", b => b
                        .Data("AA1", 3)))
                .Build();

            var listener = new PropertyChangedListener(A);

            A["A"] = 55;
            Assert.Equal("A", listener.LastChangedProperty);
            Assert.Single(listener.PropertiesChanged);

            IDataContainer AA = (IDataContainer)A["AA"];
            AA["A2"] = 42;
            Assert.Equal("AA.A2", listener.LastChangedProperty);
            Assert.Equal(2, listener.PropertiesChanged.Count);

            IDataContainer AAA = (IDataContainer)AA["AAA"];
            AAA["AA1"] = 5;
            Assert.Equal("AA.AAA.AA1", listener.LastChangedProperty);
            Assert.Equal(3, listener.PropertiesChanged.Count);

        }

        [Fact]
        public void IDataContainer_ShouldNotRaisePropertyChangedIfValueSame()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Build();

            var listener = new PropertyChangedListener(A);

            A["A"] = 1;
            Assert.True(string.IsNullOrEmpty(listener.LastChangedProperty));
            Assert.Empty(listener.PropertiesChanged);

            A["A"] = 2;
            Assert.Equal("A", listener.LastChangedProperty);
            Assert.NotEmpty(listener.PropertiesChanged);
        }

        [Fact]
        public void IDataContainer_ObjectDataObjectRaisesPropertyChangedWhenCLRObjectChanges()
        {
            BindingTestObject obj = new BindingTestObject();

            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("Obj1", obj, SerializationFormat.Container)
                .Data("Obj2", obj, SerializationFormat.Xml)
                .Data("Obj3", obj, SerializationFormat.Json)
                .Build();

            var listener = new PropertyChangedListener(A);

            obj.IntProperty = 44;

            Assert.Contains("Obj1", listener.PropertiesChanged);
            Assert.Contains("Obj2", listener.PropertiesChanged);
            Assert.Contains("Obj3", listener.PropertiesChanged);
            Assert.Equal("Obj3", listener.LastChangedProperty);
        }

        [Fact]
        public void IDataContainer_RaisesCollectionChangedOnAdd()
        {
            IDataContainer A = DataContainerBuilder.Create().Build();

            var listener = new CollectionChangedListener(A);

            Assert.Null(listener.LastChange);

            DataObject newObject = DataObjectFactory.GetDataObjectFor("A", 1);

            A.Add(newObject);

            Assert.NotNull(listener.LastChange);
            Assert.Equal(NotifyCollectionChangedAction.Add, listener.LastChange.Action);
            Assert.Single(listener.LastChange.NewItems);

            Assert.Same(newObject, listener.LastChange.NewItems[0]);
        }

        [Fact]
        public void IDataContainer_RaisesCollectionChangedOnRemove()
        {
            IDataContainer A = DataContainerBuilder.Create()
                .Data("A", 1)
                .Build();

            var listener = new CollectionChangedListener(A);
            Assert.Null(listener.LastChange);

            DataObject obj = A.Find("A");

            A.Remove(obj);
            
            Assert.NotNull(listener.LastChange);
            Assert.Equal(NotifyCollectionChangedAction.Remove, listener.LastChange.Action);
            Assert.Single(listener.LastChange.OldItems);

            Assert.Same(obj, listener.LastChange.OldItems[0]);
        }

        [Fact]
        public void IDataContainer_RaisesCollectionChangedOnClear()
        {
            IDataContainer A = DataContainerBuilder.Create()
                .Data("A", 1)
                .Data("B", 2)
                .Build();

            var listener = new CollectionChangedListener(A);
            Assert.Null(listener.LastChange);

            A.Clear();

            Assert.NotNull(listener.LastChange);
            Assert.Equal(NotifyCollectionChangedAction.Reset, listener.LastChange.Action);
        }

        [Fact]
        public void IDataContainer_EnableChangeNotificationWorks()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Build();

            A.EnableChangeNotification = false;

            var listener = new PropertyChangedListener(A);

            A["A"] = 55;
            Assert.Null(listener.LastChangedProperty);
            Assert.Empty(listener.PropertiesChanged);

            A.EnableChangeNotification = true;

            A["B"] = 23;
            Assert.Equal("B", listener.LastChangedProperty);
            Assert.Single(listener.PropertiesChanged);

        }
    }
}
