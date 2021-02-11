using System;

namespace KEI.Infrastructure
{
    /// <summary>
    /// Ignores the property during creating from <see cref="DataContainerBuilder.CreateObject(string, object)"/>
    /// or <see cref="PropertyContainerBuilder.CreateObject(string, object)"/>
    /// </summary>
    [AttributeUsage(validOn:AttributeTargets.Property)]
    public class DataContainerIgnoreAttribute : Attribute { }

    /// <summary>
    /// <see cref="DataContainerBuilder.CreateObject(string, object)"/> will call this method
    /// if implemented
    /// </summary>
    public interface IDataContainerSource
    {
        IDataContainer ToDataContainer();
    }

    /// <summary>
    /// <see cref="PropertyContainerBuilder.CreateObject(string, object)"/> will call this method
    /// if implemented
    /// </summary>
    public interface IPropertyContainerSource
    {
        IPropertyContainer ToPropertyContainer();
    }
}
