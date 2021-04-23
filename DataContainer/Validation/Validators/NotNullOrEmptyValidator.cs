using System.Runtime.Serialization;

namespace System.Configuration.Validation
{
    [Serializable]
    public class NotNullOrEmptyValidator : ValidationRule
    {
        public NotNullOrEmptyValidator()
        {

        }

        public NotNullOrEmptyValidator(SerializationInfo info, StreamingContext context)
        {

        }

        public override ValidationResult Validate(object value)
        {

            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationFailed("value cannot be empty");
            }

            return ValidationSucces();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            return;
        }

        public override string StringRepresentation => "NotEmpty";
    }
}
