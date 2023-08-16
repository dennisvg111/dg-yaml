namespace DG.Yaml.Tokenization
{
    public interface ITokenizationState
    {
        bool StreamStartTokenized { get; }
        bool CanRead { get; }
        char CurrentCharacter { get; }

        int CurrentLine { get; }
        int CurrentColumn { get; }

        void Advance(int count);

        bool TryPeekNextCharacter(out char ch);

        bool IsNext(char c);
        bool IsNext(string input);
        bool IsCurrent(string input);

        /// <summary>
        /// Advances through the current newline, and returns the amount of characters advanced.
        /// </summary>
        /// <returns></returns>
        int AdvanceNewline();
    }
}
