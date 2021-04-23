using System.Configuration.Validation;

namespace System.Configuration
{
    public class PropertyObjectBuilder
    {
        private readonly PropertyObject prop;

        public PropertyObjectBuilder(PropertyObject obj)
        {
            prop = obj;
        }

        public PropertyObjectBuilder()
        {

        }

        public virtual PropertyObjectBuilder SetCategory(string category)
        {
            prop.Category = category;
            return this;
        }

        public virtual PropertyObjectBuilder SetDescription(string description)
        {
            prop.Description = description;
            return this;
        }

        public virtual PropertyObjectBuilder SetDisplayName(string displayName)
        {
            prop.DisplayName = displayName;
            return this;
        }

        public virtual PropertyObjectBuilder SetValidation(ValidatorGroup validation)
        {
            prop.Validation = validation;
            return this;
        }
    }

    public class NumericPropertyObjectBuilder<T>
    {
        private readonly PropertyObject prop;

        public NumericPropertyObjectBuilder(PropertyObject obj)
        {
            prop = obj;
        }

        public NumericPropertyObjectBuilder<T> SetCategory(string category)
        {
            prop.Category = category;
            return this;
        }

        public NumericPropertyObjectBuilder<T> SetDescription(string description)
        {
            prop.Description = description;
            return this;
        }

        public NumericPropertyObjectBuilder<T> SetDisplayName(string displayName)
        {
            prop.DisplayName = displayName;
            return this;
        }

        public NumericPropertyObjectBuilder<T> SetValidation(ValidatorGroup validation)
        {
            prop.Validation = validation;
            return this;
        }

        public NumericPropertyObjectBuilder<T> SetIncrement(T increment)
        {
            if (prop is INumericPropertyObject np)
            {
                np.Increment = increment;
            }
            return this;
        }

        public NumericPropertyObjectBuilder<T> SetMax(T max)
        {
            if (prop is INumericPropertyObject np)
            {
                np.Max = max;
            }
            return this;

        }

        public NumericPropertyObjectBuilder<T> SetMin(T min)
        {
            if (prop is INumericPropertyObject np)
            {
                np.Min = min;
            }
            return this;
        }

    }

    public class FilePropertyObjectBuilder
    {
        private readonly PropertyObject prop;

        public FilePropertyObjectBuilder(PropertyObject obj)
        {
            prop = obj;
        }

        public FilePropertyObjectBuilder SetCategory(string category)
        {
            prop.Category = category;
            return this;
        }

        public FilePropertyObjectBuilder SetDescription(string description)
        {
            prop.Description = description;
            return this;
        }

        public FilePropertyObjectBuilder SetDisplayName(string displayName)
        {
            prop.DisplayName = displayName;
            return this;
        }

        public FilePropertyObjectBuilder SetValidation(ValidatorGroup validation)
        {
            prop.Validation = validation;
            return this;
        }

        public FilePropertyObjectBuilder AddFilter(string description, string extension)
        {
            if (prop is IFileProperty fp)
            {
                fp.Filters.Add(new Filter(description, extension));
            }
            return this;
        }
    }

    public interface INumericPropertyObject
    {
        object Increment { get; set; }
        object Max { get; set; }
        object Min { get; set; }
    }
}
