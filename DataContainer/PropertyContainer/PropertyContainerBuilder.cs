using System;
using System.Reflection;
using KEI.Infrastructure.Helpers;

namespace KEI.Infrastructure
{

    /// <summary>
    /// Class use to build <see cref="IPropertyContainer"/> Objects
    /// </summary>
    public class PropertyContainerBuilder
    {
        #region Fields

        /// <summary>
        /// Object the builder uses
        /// </summary>
        protected readonly IPropertyContainer config;


        #endregion

        #region Creation

        /// <summary>
        /// Constructor
        /// </summary>
        internal PropertyContainerBuilder()
        {
            config = new PropertyContainer();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configName"></param>
        internal PropertyContainerBuilder(string configName)
        {
            config = new PropertyContainer { Name = configName };
        }

        /// <summary>
        /// External libraries should use this method to create objects
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyContainerBuilder Create(string name)
            => new PropertyContainerBuilder(name);

        /// <summary>
        /// External libraries should use this method to create objects
        /// </summary>
        /// <returns></returns>
        public static PropertyContainerBuilder Create()
            => new PropertyContainerBuilder();

        /// <summary>
        /// Create <see cref="IPropertyContainer"/> from xml file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IPropertyContainer FromXmlFile(string path) => Infrastructure.PropertyContainer.FromXmlFile(path);

        /// <summary>
        /// Create <see cref="IPropertyContainer"/> from binary file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IPropertyContainer FromBinaryFile(string path) => Infrastructure.PropertyContainer.FromBinaryFile(path);

        #endregion

        /// <summary>
        /// Sets the underlying type of the PropertyContainer being built
        /// </summary>
        /// <param name="t">Type object</param>
        public void SetUnderlyingType(Type t) => config.UnderlyingType = t;

        /// <summary>
        /// Create a nested property container.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder PropertyContainer(string name, Action<PropertyContainerBuilder> containerBuilder)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            var builder = Create(name);
            
            containerBuilder?.Invoke(builder);

            config.Add(new ContainerPropertyObject(name, builder.Build()));

            return this;
        }

        /// <summary>
        /// Creates a DataObject implementation that serializes to <paramref name="format"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Property<T>(string name, T value, SerializationFormat format, Action<PropertyObjectBuilder> propertyBuilder = null)
            where T : class, new()
        {
            if (config.ContainsData(name) || value is null)
            {
                return this;
            }

            PropertyObject obj;
            if (format == SerializationFormat.Container)
            {
                obj = DataObjectFactory.GetPropertyObjectFor(name, value);
            }
            else if(format == SerializationFormat.Json)
            {
                obj = new JsonPropertyObject(name, value);
            }
            else if(format == SerializationFormat.Xml)
            {
                obj = new XmlPropertyObject(name, value);
            }
            else
            {
                throw new NotImplementedException();
            }

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Generic method to create all types of objects
        /// Addes a different implementation of <see cref="PropertyObject"/> based on <see cref="Type"/>
        /// of <paramref name="value"/>
        /// </summary>
        /// <param name="name">name of property to be added</param>
        /// <param name="value">value of property</param>
        /// <param name="propertyBuilder">builder to set additional properties of <see cref="PropertyObject"/></param>
        /// <returns></returns>
        public PropertyContainerBuilder Property(string name, object value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name) || value is null)
            {
                return this;
            }

            if (DataObjectFactory.GetPropertyObjectFor(name, value) is PropertyObject obj)
            {
                propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

                config.Add(obj);
            }

            return this;
        }

        /// <summary>
        /// Method to add password data, need separate function to avoid ambiguity between <see cref="StringDataObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Password(string name, string value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name) || value is null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return this;
            }

            var obj = new PasswordPropertyObject(name, value);
            
            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Adds <see cref="BoolPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Bool(string name, bool value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            var obj = new BoolPropertyObject(name, value);

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Adds <see cref="PointPropertyObject"/> passing <paramref name="x"/> and <paramref name="y"/> to <see cref="Infrastructure.Point"/> constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Point(string name, double x, double y, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            return Point(name, new Point(x, y), propertyBuilder);
        }


        /// <summary>
        /// Adds <see cref="PointPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Point(string name, Point value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            var obj = new PointPropertyObject(name, value);

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Adds <see cref="TimeSpanPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Time(string name, TimeSpan value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            var obj = new TimeSpanPropertyObject(name, value);

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Adds <see cref="DateTimePropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder DateTime(string name, DateTime value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            var obj = new DateTimePropertyObject(name, value);

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Adds <see cref="FilePropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder File(string name, string value, Action<FilePropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name) || value is null)
            {
                return this;
            }

            var obj = new FilePropertyObject(name, value);

            propertyBuilder?.Invoke(new FilePropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Adds <see cref="ColorPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="c"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Color(string name, Color c, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            var obj = new ColorPropertyObject(name, c);

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Creates <see cref="Infrastructure.Color"/> from hex value and adds <see cref="ColorPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hex"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Color(string name, string hex, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (Infrastructure.Color.Parse(hex) is Color c)
            {
                Color(name, c, propertyBuilder);
            }

            return this;
        }

        /// <summary>
        /// Creates <see cref="Infrastructure.Color"/> from R,G,B values and adds <see cref="ColorPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Color(string name, byte R, byte G, byte B, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            return Color(name, new Color(R, G, B), propertyBuilder);
        }

        /// <summary>
        /// Adds <see cref="Array1DPropertyObject"/> or <see cref="Array2DPropertyObject"/>
        /// Throws <see cref="NotSupportedException"/> if <see cref="System.Array"/> of <see cref="System.Array.Rank"/> greater than 2 is given
        /// </summary>
        /// <param name="name"></param>
        /// <param name="a"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Array(string name, Array a, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            if (a.GetType().GetElementType().IsPrimitive == false)
            {
                throw new NotSupportedException("Arrays of non primitive types not supported");
            }

            PropertyObject obj = null;
            if(a.Rank == 1)
            {
                obj = new Array1DPropertyObject(name, a);
            }
            else if(a.Rank == 2)
            {
                obj = new Array2DPropertyObject(name, a);
            }
            else
            {
                throw new NotSupportedException("Arrays of more than 2 dimensions not supported");
            }

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Generic method for adding numeric types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Number<T>(string name, T value, Action<NumericPropertyObjectBuilder<T>> propertyBuilder = null)
            where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (config.ContainsData(name))
            {
                return this;
            }

            if (DataObjectFactory.GetPropertyObjectFor(name, value) is PropertyObject obj)
            {
                propertyBuilder?.Invoke(new NumericPropertyObjectBuilder<T>(obj));

                config.Add(obj);
            }

            return this;
        }

        /// <summary>
        /// Adds <see cref="FolderPropertyObject"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public PropertyContainerBuilder Folder(string name, string value, Action<PropertyObjectBuilder> propertyBuilder = null)
        {
            if (config.ContainsData(name) || value is null)
            {
                return this;
            }

            var obj = new FolderPropertyObject(name, value);

            propertyBuilder?.Invoke(new PropertyObjectBuilder(obj));

            config.Add(obj);

            return this;
        }

        /// <summary>
        /// Creates a <see cref="IPropertyContainer"/> which was the same properties and hierarchy as <paramref name="value"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IPropertyContainer CreateObject(string name, object value)
        {
            if (value is null)
            {
                return null;
            }
            else if (value is IPropertyContainerSource ips)
            {
                return ips.ToPropertyContainer();
            }

            var objContainer = new PropertyContainerBuilder(name);

            objContainer.SetUnderlyingType(value.GetType());

            var props = value.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.CanWrite == false)
                {
                    continue;
                }
                else if (prop.GetCustomAttribute<DataContainerIgnoreAttribute>() is DataContainerIgnoreAttribute)
                {
                    continue;
                }

                objContainer.Property(prop, value);
            }

            return objContainer.Build();
        }

        /// <summary>
        /// Returns the config with data and configs specified by the builder
        /// </summary>
        /// <returns>DataContainer Object</returns>
        public IPropertyContainer Build() => config;

        #region Private/Internal Methods

        /// <summary>
        /// Method used internally to create <see cref="IPropertyContainer"/> from CLR objects
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private PropertyContainerBuilder Property(PropertyInfo pi, object obj)
        {
            if (pi.GetValue(obj) is object value)
            {
                var option = pi.GetBrowseOption();
                var description = pi.GetDescription();
                var category = pi.GetCategory();
                var displayName = pi.GetDisplayName();

                // Why is this here ??
                if (value is IDataContainer dc && dc.Count == 0)
                {
                    return this;
                }

                config.Add(DataObjectFactory.GetPropertyObjectFor(pi.Name, value)?
                    .SetBrowsePermission(option)
                    .SetDescription(description)
                    .SetCategory(category)
                    .SetDisplayName(displayName));
            }

            return this;
        }

        #endregion
    }
}
