﻿using System.Configuration.Validation.Attributes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace System.Configuration.Validation
{
    [Serializable]
    public class LengthValidator : ValidationRule
    {
        private int min;
        private int max;

        public LengthValidator()
        {

        }

        public LengthValidator(SerializationInfo info, StreamingContext context)
        {
            Min = info.GetInt32(nameof(Min));
            Max = info.GetInt32(nameof(Max));
        }

        [XmlAttribute]
        [Positive]
        public int Min
        {
            get { return min; }
            set { SetValidationProperty(ref min, value); }
        }

        [XmlAttribute]
        [Positive]
        public int Max
        {
            get { return max; }
            set { SetValidationProperty(ref max, value); }
        }

        public override ValidationResult Validate(object value)
        {
            if (value is string str)
            {
                if (Max == Min && str.Length != Max)
                {
                    return ValidationFailed($"Length must be equal to {Max}");
                }

                string commonErrMsg = $"Length should be between ({Min} - {Max}) Characters";

                if (str.Length > Max && Max >= 0)
                {
                    return ValidationFailed(Min >= 0 ? commonErrMsg : $"Length must be lesser than {Max}");
                }


                if (str.Length < Min && Min >= 0)
                {
                    return ValidationFailed(Max >= 0 ? commonErrMsg : $"Length must be greater than {Min}");
                }

                return ValidationSucces();
            }
            else
            {
                return ValidationFailed("Value must be string");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Min), Min);
            info.AddValue(nameof(Max), Max);
        }

        public override string StringRepresentation
        {
            get
            {
                if (Min < 0)
                    return $"MaxLength - {Max}";
                else if (Max == int.MaxValue)
                    return $"MinLength - {Min}";
                else if (max == min)
                    return $"Length = {Max}";
                else if (Min != int.MinValue && Max != int.MaxValue)
                    return $"Length[{Min} - {Max}]";

                return string.Empty;
            }
        }
    }
}
