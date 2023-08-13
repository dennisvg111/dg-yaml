namespace DG.Yaml.Tokenization
{
    public interface ITokenizationState
    {
        bool StartedReading { get; }
        bool CanRead { get; }
        char CurrentCharacter { get; }

        int CharactersSinceNewline { get; }

        void Advance(int count);

        bool TryPeekNextCharacter(out char ch);

        bool IsNext(char c);
        bool IsNext(string input);
        bool IsCurrent(string input);
    }
}
