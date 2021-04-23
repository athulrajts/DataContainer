using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Configuration.Validation
{
    [Serializable]
    public class MustBeOneOfValidator : ValidationRule
    {
        public MustBeOneOfValidator()
        {

        }

        public MustBeOneOfValidator(SerializationInfo info, StreamingContext context)
        {
            AllowedValues = (List<string>)info.GetValue(nameof(AllowedValues), typeof(List<string>));
        }

        public List<string> AllowedValues { get; set; } = new List<string>();

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(AllowedValues), AllowedValues);
        }

        public override ValidationResult Validate(object value)
        {
            if (value == null)
            {
                return ValidationFailed("Value cannot be empty");
            }

            if (AllowedValues.Contains(value.ToString()) == false)
            {
                return ValidationFailed($"{value} is not one of {string.Join("|", AllowedValues)}");
            }

            return ValidationSucces();
        }
    }
}
