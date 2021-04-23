using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace System.Configuration.Validation
{
    public enum PathValidationMode
    {
        File,
        Directory
    }

    [Serializable]
    public class PathValidator : ValidationRule
    {
        private PathValidationMode mode;

        public PathValidator()
        {

        }

        public PathValidator(SerializationInfo info, StreamingContext context)
        {
            Mode = (PathValidationMode)info.GetValue(nameof(Mode), typeof(PathValidationMode));
        }

        [XmlAttribute]
        public PathValidationMode Mode
        {
            get { return mode; }
            set { SetValidationProperty(ref mode, value); }
        }
        public override ValidationResult Validate(object value)
        {
            string str = value?.ToString();

            if (string.IsNullOrEmpty(str))
            {
                return ValidationFailed("Path cannot be empty");
            }

            if (Mode == PathValidationMode.File)
            {
                if (File.Exists(str) == false)
                {
                    return ValidationFailed($"File \"{str}\" does not exisit");
                }
            }
            else
            {
                if (Directory.Exists(str) == false)
                {
                    return ValidationFailed($"Directory \"{str}\" does not exisit");
                }
            }

            return ValidationSucces();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Mode), Mode);
        }

        public override string StringRepresentation => Mode.ToString();
    }
}
