using Avalonia.Data;

namespace DevSpector.Desktop.UI.Validators
{
    public class EnglishTextValidator : TextValidatorBase
    {
        public EnglishTextValidator(
            string validationErrorMessage = null,
            string wrongTextLengthErrorMessage = null,
            int minTextLength = 3,
            int maxTextLength = 100
        ) : base(minTextLength, maxTextLength, wrongTextLengthErrorMessage)
        {
            if (validationErrorMessage == null)
                this.ErrorMessage = "Значение может содержать только латиницу и спец. символы";
            else
                this.ErrorMessage = validationErrorMessage;
        }

        public new string ErrorMessage { get; }

        public override void Validate(string text)
        {
            base.Validate(text);

            var allowedSymbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-!@#$%^&*()=;\"\'?.,/\\";

            foreach (var sym in text)
                if (!allowedSymbols.Contains(sym))
                    throw new DataValidationException(ErrorMessage);
        }

        public override bool ValidateBool(string text)
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
