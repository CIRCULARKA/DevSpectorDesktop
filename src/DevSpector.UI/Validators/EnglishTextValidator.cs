using Avalonia.Data;

namespace DevSpector.Desktop.UI.Validators
{
    public class EnglishTextValidator : TextValidatorBase
    {
        public EnglishTextValidator(string validationErrorMessage = null)
        {
            if (validationErrorMessage == null)
                this.ErrorMessage = "Значение может содержать только латиницу и спец. символы";
            else
                this.ErrorMessage = validationErrorMessage;
        }

        public override string ErrorMessage { get; }

        public override void Validate(string text)
        {
            base.Validate(text);

            var allowedSymbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-!@#$%^&*()=;\"\'?.,/\\";

            foreach (var sym in text)
                if (!allowedSymbols.Contains(sym))
                    throw new DataValidationException(ErrorMessage);
        }
    }
}
