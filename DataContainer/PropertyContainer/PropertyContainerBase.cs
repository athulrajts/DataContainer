using KEI.Infrastructure.Validation;
using System;
using System.Runtime.Serialization;

namespace KEI.Infrastructure
{
    [Serializable]
    public abstract class PropertyContainerBase : DataContainerBase, IPropertyContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyContainerBase() { }

        /// <summary>
        /// Constructor for binary deserialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PropertyContainerBase(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Set <see cref="PropertyObject.BrowseOption"/> of the <see cref="PropertyObject"/>
        /// identified by name.
        /// </summary>
        /// <param name="property">name of <see cref="PropertyObject"/> to update BrowseOption</param>
        /// <param name="option"></param>
        public bool SetBrowseOptions(string property, BrowseOptions option)
        {
            if (this.FindRecursive(property) is PropertyObject di)
            {
                di.BrowseOption = option;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Set <see cref="PropertyObject.Validation"/> of the <see cref="PropertyObject"/>
        /// identified by name.
        /// </summary>
        /// <param name="property">name of <see cref="PropertyObject"/> to update Validation</param>
        /// <param name="option"></param>
        public bool SetValidation(string property, ValidatorGroup validation)
        {
            if (this.FindRecursive(property) is PropertyObject di)
            {
                di.Validation = validation;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Implementation for <see cref="DataContainerBase.GetUnitializedDataObject(string)"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override DataObject GetUnitializedDataObject(string type)
        {
            return DataObjectFactory.GetPropertyObject(type);
        }
    }
}
