using System.Collections.Generic;
using System.ComponentModel;

namespace System.Configuration
{
    public abstract partial class DataContainerBase : ICustomTypeDescriptor
    {
        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetComponentName"/>
        /// </summary>
        /// <returns></returns>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetDefaultEvent"/>
        /// </summary>
        /// <returns></returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetClassName"/>
        /// </summary>
        /// <returns></returns>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetEvents(Attribute[])"/>
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetEvents"/>
        /// </summary>
        /// <returns></returns>
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetConverter"/>
        /// </summary>
        /// <returns></returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor)"/>
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        /// <summary>
        /// Imlplementation for <see cref="ICustomTypeDescriptor.GetAttributes"/>
        /// </summary>
        /// <returns></returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetEditor(Type)"/>
        /// </summary>
        /// <param name="editorBaseType"></param>
        /// <returns></returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetDefaultProperty"/>
        /// </summary>
        /// <returns></returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetProperties"/>
        /// </summary>
        /// <returns></returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(Array.Empty<Attribute>());
        }

        /// <summary>
        /// Implementation for <see cref="ICustomTypeDescriptor.GetProperties(Attribute[])"/>
        /// This is a called by PropertyGrid Implementations, both in WinForms and WPF.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

            // add attributes for property grid
            foreach (DataObject data in this)
            {
                var attrs = new List<Attribute>();

                // add expandable attributes for data objects holding complex objects
                if ((data.Type == DataObjectType.Xml || data.Type == DataObjectType.Container || data.Type == DataObjectType.Json)
                    && PropertyGridHelper.ExpandableAttribute != null)
                {
                    attrs.Add(PropertyGridHelper.ExpandableAttribute);
                }

                /// add editor attribute
                /// custom editors should be registered using <see cref="PropertyGridHelper.RegisterEditor{TEditor}(string)"/>
                if (PropertyGridHelper.GetEditorType(data.Type) is Type t)
                {
                    string fullname = t.FullName;
                    string assembly = t.Assembly.FullName;

                    attrs.Add(new EditorAttribute($"{fullname}, {assembly}", "System.Drawing.Design.UITypeEditor, System.Windows.Forms"));
                }

                /// add type converter attribute
                /// custom type converters should be registered using <see cref="PropertyGridHelper.RegisterConverter{TConverter}(string)"/>
                if (PropertyGridHelper.GetConverterType(data.GetType()) is Type ct)
                {
                    attrs.Add(new TypeConverterAttribute(ct));
                }

                /// if it's a property object have some additional that the property grid could make use of
                /// such as <see cref="PropertyObject.Description"/>, <see cref="PropertyObject.Category"/> <see cref="PropertyObject.DisplayName"/>
                /// also <see cref="PropertyObject.BrowseOption"/> decided whether it's readonly or whether it should be displayed in
                /// property grid.
                if (data is PropertyObject po)
                {
                    // add description attribute
                    if (string.IsNullOrEmpty(po.Description) == false)
                    {
                        attrs.Add(new DescriptionAttribute(po.Description));
                    }

                    // add display name attribute
                    if (string.IsNullOrEmpty(po.DisplayName) == false)
                    {
                        attrs.Add(new DisplayNameAttribute(po.DisplayName));
                    }

                    // add category attribute
                    if (string.IsNullOrEmpty(po.Category) == false)
                    {
                        attrs.Add(new CategoryAttribute(po.Category));
                    }

                    Attribute browseOption = new BrowsableAttribute(true);
                    if (po.BrowseOption == BrowseOptions.NonBrowsable)
                    {
                        browseOption = new BrowsableAttribute(false);
                    }
                    else if (po.BrowseOption == BrowseOptions.NonEditable)
                    {
                        browseOption = new ReadOnlyAttribute(true);
                    }

                    // add browsable/readonly attributes
                    attrs.Add(browseOption);

                }

                /// Create property descriptor
                properties.Add(new DataObjectPropertyDescriptor(data, attrs.ToArray()));
            }

            return new PropertyDescriptorCollection(properties.ToArray());
        }
    }
}
