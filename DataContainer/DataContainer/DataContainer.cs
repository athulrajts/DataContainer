using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Utils;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// Concrete implementation for <see cref="IDataContainer"/>
    /// </summary>
    [XmlRoot("DataContainer")]
    [Serializable]
    public class DataContainer : DataContainerBase
    {
        /// <summary>
        /// Storage structure for all data stored inside this object
        /// TODO : Is there a need to use <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}"/> ??
        /// </summary>
        protected readonly Dictionary<string, DataObject> internalDictionary = new Dictionary<string, DataObject>();

        public DataContainer()
        {
        }

        public DataContainer(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Implementation for <see cref="IDataContainer.Count"/>
        /// </summary>
        public override int Count => internalDictionary.Count;

        /// <summary>
        /// Function to make use of list initializers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            if (IdentifierExtensions.IsValidIdentifier(key) == false)
            {
                throw new ArgumentException($"{key} is not a valid c# identifier");
            }

            internalDictionary.Add(key, DataObjectFactory.GetDataObjectFor(key, value));

            RaiseCollectionChanged(NotifyCollectionChangedAction.Add, internalDictionary[key]);
        }

        /// <summary>
        /// Alow collection initliazer using <see cref="Key{T}"/> as key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(Key<T> key, T value) => Add(key.Name, value);


        /// <summary>
        /// Clear everything
        /// </summary>
        public override void Clear()
        {
            internalDictionary.Clear();

            RaiseCollectionChanged(NotifyCollectionChangedAction.Reset);
        }


        /// <summary>
        /// Remove property with name <paramref name="key"/>
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (internalDictionary.ContainsKey(key) == false)
            {
                return;
            }

            var removedItem = internalDictionary[key];

            internalDictionary.Remove(key);

            RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem);
        }

        /// <summary>
        /// Implementation for <see cref="IDataContainer.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<DataObject> GetEnumerator() => internalDictionary.Values.Cast<DataObject>().GetEnumerator();

        /// <summary>
        /// Implementation for <see cref="IDataContainer.GetKeys"/>
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetKeys() => internalDictionary.Keys;

        /// <summary>
        /// Implementation for <see cref="IDataContainer.Add(DataObject)"/>
        /// </summary>
        /// <param name="obj"></param>
        public override void Add(DataObject obj)
        {
            if (IdentifierExtensions.IsValidIdentifier(obj.Name) == false)
            {
                throw new ArgumentException($"{obj.Name} is not a valid c# identifier");
            }

            internalDictionary.Add(obj.Name, obj);

            RaiseCollectionChanged(NotifyCollectionChangedAction.Add, internalDictionary[obj.Name]);
        }

        /// <summary>
        /// Implementation for <see cref="IDataContainer.Remove(DataObject)"/>
        /// </summary>
        /// <param name="obj"></param>
        public override void Remove(DataObject obj) => Remove(obj.Name);

        /// <summary>
        /// Implemenation for <see cref="IDataContainer.Find(string)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override DataObject Find(string key) =>
            internalDictionary.ContainsKey(key)
            ? internalDictionary[key]
            : null;

        /// <summary>
        /// Create <see cref="DataContainer"/> from XML serialized file
        /// </summary>
        /// <param name="path">Path to XML file</param>
        /// <returns><see cref="DataContainer"/> deserilized from path</returns>
        public static IDataContainer FromXmlFile(string path)
        {
            if (XmlHelper.DeserializeFromFile<DataContainer>(path) is DataContainer dc)
            {
                dc.FilePath = path;
                return dc;
            }

            return null;
        }


        public static IDataContainer FromBinaryFile(string path)
        {
            IFormatter formatter = new BinaryFormatter
            {
                Binder = new CustomizedBinder()
            };

            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    return (IDataContainer)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                DataContainerEvents.NotifyError($"Error reading file :{path}, {ex}");

                return null;
            }
        }
    }
    /// <summary>
    /// Added to fix BinarySerialization not working in C++/CLI
    /// </summary>
    sealed class CustomizedBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type returntype = null;
            string sharedAssemblyName = typeof(DataContainer).Assembly.FullName;
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            typeName = typeName.Replace(sharedAssemblyName, assemblyName);
            returntype =
                    Type.GetType(string.Format("{0}, {1}",
                    typeName, assemblyName));

            return returntype;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);
            assemblyName = typeof(DataContainer).Assembly.FullName;
        }
    }
}
