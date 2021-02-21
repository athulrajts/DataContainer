using System;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.Specialized;
using KEI.Infrastructure.Utils;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace KEI.Infrastructure
{
    [XmlRoot("DataContainer")]
    [Serializable]
    public class PropertyContainer : PropertyContainerBase
    {
        /// <summary>
        /// Storage structure for all data stored inside this object
        /// TODO : Is there a need to use <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}"/> ??
        /// </summary>
        protected readonly Dictionary<string, DataObject> internalDictionary = new Dictionary<string, DataObject>();

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyContainer() { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PropertyContainer(SerializationInfo info, StreamingContext context) : base(info, context) { }


        /// <summary>
        /// Implementation for <see cref="IDataContainer.Count"/>
        /// </summary>
        public override int Count => internalDictionary.Count;

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
        /// Load state from xml file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IPropertyContainer FromXmlFile(string path)
        {
            if (XmlHelper.DeserializeFromFile<PropertyContainer>(path) is PropertyContainer dc)
            {
                dc.FilePath = path;

                return dc;
            }

            return null;
        }

        /// <summary>
        /// Load state from binary file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IPropertyContainer FromBinaryFile(string path)
        {
            IFormatter formatter = new BinaryFormatter();

            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    return (IPropertyContainer)formatter.Deserialize(stream);
                }
            }
            catch(Exception ex)
            {
                DataContainerEvents.NotifyError($"Error reading file :{path}, {ex}");

                return null;
            }
        }

        /// <summary>
        /// Function for List initializer
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            if (IdentifierExtensions.IsValidIdentifier(key) == false)
            {
                throw new ArgumentException($"{key} is not a valid c# identifier");
            }

            internalDictionary.Add(key, DataObjectFactory.GetPropertyObjectFor(key, value));

            RaiseCollectionChanged(NotifyCollectionChangedAction.Add, internalDictionary[key]);
        }

        /// <summary>
        /// Implementation for <see cref="IDataContainer.Find(string)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override DataObject Find(string key) => internalDictionary.ContainsKey(key) ? internalDictionary[key] : null;

        /// <summary>
        /// Implementation for <see cref="IDataContainer.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<DataObject> GetEnumerator()
            => internalDictionary.Values.Cast<DataObject>().GetEnumerator();

        /// <summary>
        /// Implementation for <see cref="IDataContainer.GetKeys"/>
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetKeys() => internalDictionary.Keys;

        /// <summary>
        /// Implementation for <see cref="IDataContainer.Remove(DataObject)"/>
        /// </summary>
        /// <param name="obj"></param>
        public override void Remove(DataObject obj) => Remove(obj.Name);

        /// <summary>
        /// Removes item from this
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
    }
}
