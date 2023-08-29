namespace DG.Yaml.Tokenization.State
{
    public interface ITokenizationState
    {
        bool StartedReading { get; }
        bool CanRead { get; }
        char CurrentCharacter { get; }

        int CharactersSinceNewline { get; }
        long Line { get; }

        void Advance(int count);

        bool TryPeekNextCharacter(out char ch);

        bool IsNext(char c);

        /// <summary>
        /// Advances through the current newline, and returns the amount of characters advanced.
        /// </summary>
        /// <returns></returns>
        int AdvanceNewline();
    }
}
