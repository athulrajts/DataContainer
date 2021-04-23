using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace System.Configuration
{
    /// <summary>
    /// Base class for <see cref="DataContainer"/> and <see cref="PropertyContainer"/>
    /// </summary>
    [XmlRoot("DataContainer")]
    [Serializable]
    public abstract partial class DataContainerBase : DynamicObject, IDataContainer
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataContainerBase()
        {
            CollectionChanged += Data_CollectionChanged;
        }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DataContainerBase(SerializationInfo info, StreamingContext context) : this()
        {
            Name = info.GetString(nameof(Name));
            UnderlyingType = (TypeInfo)info.GetValue(nameof(UnderlyingType), typeof(TypeInfo));
            int count = info.GetInt32(nameof(Count));

            for (int i = 0; i < count; i++)
            {
                DataObject obj = (DataObject)info.GetValue($"Data_{i}", typeof(DataObject));
                Add(obj);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the DataContainer
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path from which this object was deserialised from
        /// </summary>
        public string FilePath { get; internal set; }

        /// <summary>
        /// Represents Type that this instance can be cast into
        /// can be converted into
        /// </summary>
        public TypeInfo UnderlyingType { get; set; }

        /// <summary>
        /// Number of data objects in this container
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                DataObject obj = this.FindRecursive(key);

                if (obj is null)
                {
                    throw new KeyNotFoundException();
                }

                return obj.GetValue();
            }
            set
            {
                DataObject obj = this.FindRecursive(key);

                if (obj is null)
                {
                    throw new KeyNotFoundException();
                }

                obj.SetValue(value);
            }
        }

        /// <summary>
        /// Indicates whether instance will raise <see cref="INotifyPropertyChanged.PropertyChanged"/> event
        /// </summary>
        public bool EnableChangeNotification { get; set; } = true;

        #endregion

        #region Manipulation

        /// <summary>
        /// Gets Data Value from a DataContainer object
        /// </summary>
        /// <typeparam name="T">Type of result</typeparam>
        /// <param name="key">Key, which uniquely identifies the data</param>
        /// <param name="value">Value object passed as reference</param>
        /// <returns>Is Success</returns>
        public virtual bool GetValue<T>(string key, ref T value)
        {
            var data = this.FindRecursive(key);

            bool result = false;

            if (data != null && data.GetValue() is T val)
            {
                value = val;
                result = true;
            }
            else
            {
                DataContainerEvents.NotifyError($"Unable to find \"{key}\"");
            }

            return result;
        }

        /// <summary>
        /// Sets the Data value
        /// </summary>
        /// <param name="key">Key, which uniquely identifies the data</param>
        /// <param name="value">Value to set</param>
        public virtual bool SetValue(string key, object value)
        {
            var data = this.FindRecursive(key);

            if (data is null)
            {
                DataContainerEvents.NotifyError($"Unable to find \"{key}\"");

                return false;
            }

            return data.SetValue(value);
        }

        /// <summary>
        /// Serializes DataContainer object to an XML file to the given path
        /// </summary>
        /// <param name="path">file path to store the config</param>
        /// <returns>Is Sucess</returns>
        public bool SaveAsXml(string path)
        {
            FilePath = path;

            if (XmlHelper.SerializeToFile(this, path) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Serializes DataContainer object to an XML file to the given path
        /// </summary>
        /// <returns></returns>
        public bool SaveAsXml() => SaveAsXml(FilePath);

        /// <summary>
        /// Serializer object to a binary file in given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SaveAsBinary(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    return this.WriteToStream(stream);
                }
            }
            catch (Exception ex)
            {
                DataContainerEvents.NotifyError(ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// Serializer object to a binary file in given path
        /// </summary>
        public bool SaveAsBinary() => SaveAsBinary(FilePath);

        /// <summary>
        /// Checks if the Object contains data with given key
        /// </summary>
        /// <param name="key">data Key to search for</param>
        /// <returns>Is Found</returns>
        public virtual bool ContainsData(string key) => this.FindRecursive(key) is DataObject;

        #endregion

        #region Morphing

        /// <summary>
        /// Maps this contents of this objects to a class objected
        /// represented by <see cref="UnderlyingType"/>
        /// </summary>
        /// <returns>returns as object</returns>
        public virtual object Morph()
        {
            if (UnderlyingType is null)
            {
                throw new InvalidOperationException("Morph not supported for this instance");
            }

            return MorphToObject(UnderlyingType);
        }

        /// <summary>
        /// Maps this contents of this container to a class objected
        /// represented by <see cref="UnderlyingType"/>
        /// </summary>
        /// <returns></returns>
        public virtual IList MorphList()
        {
            if (UnderlyingType is null)
            {
                throw new InvalidOperationException("Morph not supported for this instance");
            }

            Type morphType = UnderlyingType;

            var list = Activator.CreateInstance(morphType) as IList;

            if (typeof(IList).IsAssignableFrom(morphType))
            {
                foreach (var item in this)
                {
                    if (item.GetValue() is IDataContainer dc)
                    {
                        list.Add(dc.Morph());
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Generic wrapper the casts call to <see cref="MorphList"/> to give type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> MorphList<T>() => (IList<T>)MorphList();


        /// <summary>
        /// tries Maps this contents of this objects to a class objected to given <typeparamref name="T"/>
        /// create a new instance of <typeparamref name="T"/> and assigns value from this container
        /// if they happened to be a property with the same name as a property in <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Returns as specified type</typeparam>
        /// <returns></returns>
        public T Morph<T>() => (T)MorphToObject(typeof(T));

        #endregion

        #region Abstract Functions

        /// <summary>
        /// Adds DataObject to datacontainer
        /// </summary>
        /// <param name="obj"></param>
        public abstract void Add(DataObject obj);

        /// <summary>
        /// Removes DataObject from datacontainer
        /// </summary>
        /// <param name="name"></param>
        public abstract void Remove(DataObject name);

        /// <summary>
        /// Removes all properties
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Gets a DataObject from this datacontainer
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract DataObject Find(string key);

        /// <summary>
        /// Get all keys in this object
        /// Is not recursives, will not provide keys for child <see cref="IDataContainer"/>
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<string> GetKeys();

        #endregion

        #region Binding Members

        /// <summary>
        /// Adds binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyKey">name of the <see cref="DataObject"/> to bind to</param>
        /// <param name="expression"><see cref="MemberExpression"/> that gets CLR property </param>
        /// <param name="updateSourceOnPropertyChange">Whether or not to update value inside <see cref="DataContainer"/> when Target value changes</param>
        public bool SetBinding<T>(string propertyKey, Expression<Func<T>> expression, BindingMode mode = BindingMode.TwoWay)
        {
            var property = this.FindRecursive(propertyKey);

            if (property is null)
            {
                DataContainerEvents.NotifyError($"Binding failed, key : {propertyKey} not found");

                return false;
            }

            MemberExpression memberExpression = expression.Body as MemberExpression;
            var propinfo = memberExpression.Member as PropertyInfo;

            if (memberExpression is null)
            {
                throw new InvalidOperationException("Body of Lambda expression must be a Member expression");
            }

            var target = Expression.Lambda(memberExpression.Expression).Compile().DynamicInvoke();

            if (BindingManager.Instance.GetBinding(target, propinfo.Name) is null)
            {
                var binding = new DataObjectBinding(target, property, propinfo, mode);
                if (mode != BindingMode.OneTime)
                {
                    BindingManager.Instance.AddBinding(binding);
                }
            }

            return true;
        }

        /// <summary>
        /// Adds binding based on convention, Uses the name of bound property in VM as key in IDataContainer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"><see cref="MemberExpression"/> that gets CLR property </param>
        /// <param name="updateSourceOnPropertyChange">Whether or not to update value inside <see cref="DataContainer"/> when Target value changes</param>
        public bool SetBinding<T>(Expression<Func<T>> expression, BindingMode mode = BindingMode.TwoWay)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            var propinfo = memberExpression.Member as PropertyInfo;

            if (memberExpression is null)
            {
                throw new InvalidOperationException("Body of Lambda expression must be a Member expression");
            }

            var property = this.FindRecursive(propinfo.Name);

            if (property is null)
            {
                DataContainerEvents.NotifyError($"Binding failed, key : {propinfo.Name} not found");

                return false;
            }

            var target = Expression.Lambda(memberExpression.Expression).Compile().DynamicInvoke();

            if (BindingManager.Instance.GetBinding(target, propinfo.Name) is null)
            {
                var binding = new DataObjectBinding(target, property, propinfo, mode);
                if (mode != BindingMode.OneTime)
                {
                    BindingManager.Instance.AddBinding(binding);
                }
            }

            return true;

        }

        /// <summary>
        /// Removes property binding
        /// Could cause memory leaks if not removed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyKey">name of the <see cref="PropertyObject"/> to bind to</param>
        /// <param name="expression"><see cref="MemberExpression"/> that gets CLR property </param>
        public bool RemoveBinding<T>(string propertyKey, Expression<Func<T>> expression)
        {
            var property = this.FindRecursive(propertyKey);

            if (property is null)
            {
                DataContainerEvents.NotifyError($"Remove Binding failed, key : {propertyKey} not found");

                return false;
            }

            var memberExpression = expression.Body as MemberExpression;
            var propinfo = memberExpression.Member as PropertyInfo;

            if (memberExpression is null)
            {
                throw new InvalidOperationException("Body of Lambda expression must be a Member expression");
            }

            var target = Expression.Lambda(memberExpression.Expression).Compile().DynamicInvoke();

            if (BindingManager.Instance.GetBinding(target, propinfo.Name) is DataObjectBinding pb)
            {
                BindingManager.Instance.RemoveBinding(pb);
            }

            return true;
        }

        /// <summary>
        /// Removes binding based on convention, Uses the name of bound property in VM as key in IDataContainer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool RemoveBinding<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            var propinfo = memberExpression.Member as PropertyInfo;

            if (memberExpression is null)
            {
                throw new InvalidOperationException("Body of Lambda expression must be a Member expression");
            }

            var property = this.FindRecursive(propinfo.Name);

            if (property is null)
            {
                DataContainerEvents.NotifyError($"Remove Binding failed, key : {propinfo.Name} not found");

                return false;
            }

            var target = Expression.Lambda(memberExpression.Expression).Compile().DynamicInvoke();

            if (BindingManager.Instance.GetBinding(target, propinfo.Name) is DataObjectBinding pb)
            {
                BindingManager.Instance.RemoveBinding(pb);
            }

            return true;
        }

        #endregion

        #region DynamicObect Overrides

        /// <summary>
        /// Called by WPF binding to get value of binding
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Check if it's an actual member
            bool ret = base.TryGetMember(binder, out result);

            // else It's a dynamic member
            if (ret == false)
            {
                /// find <see cref="DataObject"/> corresponding to the binding
                if (Find(binder.Name) is DataObject data)
                {
                    // get value
                    result = data.GetValue();

                    ret = true;
                }
                // don't have member
                else
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Called by WPF binding to set value of a binding
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Check if it's an actual member
            bool ret = base.TrySetMember(binder, value);

            // else It's a dynamic member
            if (ret == false)
            {
                /// find <see cref="DataObject"/> corresponding to the binding
                if (Find(binder.Name) is DataObject data)
                {
                    // set value
                    data.SetValue(value);

                    ret = true;
                }
                // don't have member
                else
                {
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Implementation for <see cref="DynamicObject.GetDynamicMemberNames"/>
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames() => GetKeys();

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
            => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, IList changedItems)
            => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItems));

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action)
            => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));

        #endregion

        #region INotifyPropertyChanged Memmbers

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string property = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Implementation for <see cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<DataObject> GetEnumerator();

        /// <summary>
        /// Implementation for <see cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Implementation for <see cref="ICloneable.Clone"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            string xml = XmlHelper.SerializeToString(this);
            return XmlHelper.DeserializeFromString(GetType(), xml);
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// tries Maps this contents of this objects to a class objected to given <paramref name="morphType"/>
        /// create a new instance of <paramref name="morphType"/> and assigns value from this container
        /// if they happened to be a property with the same name as a property in <paramref name="morphType"/>
        /// </summary>
        /// <param name="morphType"></param>
        /// <returns></returns>
        private object MorphToObject(Type morphType)
        {
            var result = Activator.CreateInstance(morphType);

            foreach (var prop in morphType.GetProperties())
            {
                if (prop.CanWrite)
                {
                    var data = Find(prop.Name);

                    if (data is null)
                    {
                        continue;
                    }

                    if (data.GetValue() is IDataContainer dc)
                    {
                        if (dc.UnderlyingType is null)
                        {
                            if (prop.PropertyType.IsAssignableFrom(data.GetDataType()))
                            {
                                prop.SetValue(result, dc);
                            }

                            continue;
                        }

                        prop.SetValue(result, dc.Morph());
                    }

                    else if (data != null)
                    {
                        prop.SetValue(result, data.GetValue());
                    }
                }
            }

            return result;
        }

        #endregion

        #region Bubble PropertyChanged Notifications

        /// <summary>
        /// Attaches PropertyChanged listeners to newly added Properties
        /// Removes PropertyChanged listeners from removed Properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems.Cast<DataObject>())
                {
                    item.PropertyChanged += OnPropertyChangedRaised;

                    if (item.GetValue() is IDataContainer dc)
                    {
                        dc.PropertyChanged += OnPropertyChangedRaised;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems.Cast<DataObject>())
                {
                    item.PropertyChanged -= OnPropertyChangedRaised;

                    if (item.GetValue() is IDataContainer dc)
                    {
                        dc.PropertyChanged -= OnPropertyChangedRaised;
                    }
                }
            }
        }

        private void OnPropertyChangedRaised(object sender, PropertyChangedEventArgs e)
        {
            // Don't raise any property changed event, if disabled.
            if (EnableChangeNotification == false)
            {
                return;
            }

            string propName = e.PropertyName;

            /// If sender is one of our children, pass the event on up the tree.
            if (sender is DataObject o && e.PropertyName == "Value")
            {
                RaisePropertyChanged(o.Name);
            }

            /// If one of our childrens value is <see cref="IDataContainer"/>
            /// We need to handle propertychanged for our grandchildren also
            else if (sender is IDataContainer dc)
            {
                var split = propName.Split('.');

                if (split.FirstOrDefault() == dc.Name)
                {
                    RaisePropertyChanged(propName);
                }
                else
                {
                    //Create full name, $(parent).$(child)
                    RaisePropertyChanged($"{dc.Name}.{propName}");
                }

            }
        }

        #endregion
    }

}
