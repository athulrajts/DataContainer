using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace KEI.Infrastructure.Validation
{
    [XmlRoot(IsNullable = false, ElementName = "Validations")]
    [Serializable]
    public class ValidatorGroup : ValidationRule
    {
        [XmlAttribute("Cascade")]
        public bool CascadeValidations { get; set; }

        [XmlElement(IsNullable = false, ElementName = "Rule")]
        [XmlElement(IsNullable = false, Type = typeof(RangeValidator), ElementName = nameof(RangeValidator))]
        [XmlElement(IsNullable = false, Type = typeof(PathValidator), ElementName = nameof(PathValidator))]
        [XmlElement(IsNullable = false, Type = typeof(LengthValidator), ElementName = nameof(LengthValidator))]
        [XmlElement(IsNullable = false, Type = typeof(NumberSignValidator), ElementName = nameof(NumberSignValidator))]
        [XmlElement(IsNullable = false, Type = typeof(MustBeOneOfValidator), ElementName = nameof(MustBeOneOfValidator))]
        [XmlElement(IsNullable = false, Type = typeof(NotNullOrEmptyValidator), ElementName = nameof(NotNullOrEmptyValidator))]
        [XmlElement(IsNullable = false, Type = typeof(LinearInequalityValidator), ElementName = nameof(LinearInequalityValidator))]
        public ObservableCollection<ValidationRule> Rules { get; set; } = new ObservableCollection<ValidationRule>();

        public override ValidationResult Validate(object value)
        {
            var isValid = true;
            var failedValidation = new List<string>();
            foreach (var validator in Rules)
            {
                var result = validator.Validate(value);

                if (!result.IsValid)
                {
                    isValid = false;

                    if (CascadeValidations == false)
                    {
                        return new ValidationResult(false, result.ErrorMessage);
                    }
                    else
                    {
                        failedValidation.Add(validator.StringRepresentation);
                    }
                }
            }

            return isValid ? new ValidationResult(true) : new ValidationResult(false, $"{string.Join(",", failedValidation)} failed");
        }

        public ValidatorGroup()
        {

        }

        public ValidatorGroup(bool cascadeValidations = false)
        {
            CascadeValidations = cascadeValidations;
        }

        public ValidatorGroup(SerializationInfo info, StreamingContext context)
        {
            Rules = (ObservableCollection<ValidationRule>)info.GetValue(nameof(Rules), typeof(ObservableCollection<ValidationRule>));
            CascadeValidations = info.GetBoolean(nameof(CascadeValidations));
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            var other = obj as ValidatorGroup;

            if (Rules.Count != other.Rules.Count)
            {
                return false;
            }

            for (int i = 0; i < Rules.Count; i++)
            {
                if (Rules[i].Equals(other.Rules[i]) == false)
                {
                    return false;
                }
            }

            return true;

        }

        public override int GetHashCode()
        {
            return Rules.GetHashCode();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Rules), Rules);
            info.AddValue(nameof(CascadeValidations), CascadeValidations);
        }

        public static implicit operator ValidatorGroup(ValidationBuilder builder) => builder.Validator;
    }
}
