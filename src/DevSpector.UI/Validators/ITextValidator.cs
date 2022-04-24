namespace DevSpector.Desktop.UI.Validators
{
    public interface ITextValidator
    {
        public string ErrorMessage { get; }

        /// <summary>
        /// Checks out whether string contains only english letters. Throws an exception if not.
        /// </summary>
        /// <param name="text">String to validate</param>
        /// <exception cref="DataValidationException" />
        void Validate(string text);

        /// <summary>
        /// Checks out whether string contains only english letters
        /// </summary>
        /// <returns>Returns <c>true</c> if text contains only english letters and <c>false</c> otherwise</returns>
        bool IsValid(string text);
    }
}
