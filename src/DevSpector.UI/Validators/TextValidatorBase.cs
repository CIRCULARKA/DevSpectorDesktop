using System;
using Avalonia.Data;

namespace DevSpector.Desktop.UI.Validators
{
    public class TextValidatorBase : ITextValidator
    {
        public TextValidatorBase(int minLength = 3, int maxLength = 100, string validationErrorMessage = null)
        {
            if (validationErrorMessage == null)
                ErrorMessage = $"Значение не может быть короче {minLength} и длиннее {maxLength}";
            else
                ErrorMessage = validationErrorMessage;

            if (minLength < 0 || maxLength < 0)
                throw new ArgumentException("Value can't be less than zero");

            MinLength = minLength;
            MaxLength = maxLength;
        }

        public int MaxLength { get; }

        public int MinLength { get; }

        public string ErrorMessage { get; }

        public void Validate(string text)
        {
            if (text == null) throw new DataValidationException(ErrorMessage);
            if (text.Length < MinLength) throw new DataValidationException(ErrorMessage);
            if (text.Length > MaxLength) throw new DataValidationException(ErrorMessage);
        }
    }
}
