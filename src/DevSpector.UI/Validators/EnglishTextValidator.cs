using Avalonia.Data;

namespace DevSpector.Desktop.UI.Validators
{
    public class EnglishTextValidator : ITextValidator
    {
        private const string _AllowedSymbols =
            "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-!@#$%^&*()=;\"\'?.,/\\";

        public EnglishTextValidator(string validationErrorMessage = null)
        {
            if (validationErrorMessage == null)
                this.ErrorMessage = "Значение может содержать только латиницу и спец. символы";
            else
                this.ErrorMessage = validationErrorMessage;
        }

        public string ErrorMessage { get; }

        public void Validate(string text)
        {
            foreach (var sym in text)
                if (!_AllowedSymbols.Contains(sym))
                    throw new DataValidationException(ErrorMessage);
        }

        public bool IsValid(string text)
        {
            try
            {
                this.Validate(text);
                return true;
            }
            catch { return false; }
        }
    }
}
