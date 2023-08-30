namespace DG.Yaml.Tokenization.State
{
    public class TokenizationState : ITokenizationState
    {

        public TokenizationState(ICharacterReader reader)
        {
            _reader = reader;

            _canRead = true;
            _currentCharacter = '\0';
            _charactersSinceNewline = -1;
            _line = 1;
        }

        public bool TryPeekNextCharacter(out char ch)
        {
            return _reader.TryPeek(out ch);
        }

        public bool IsNext(char c)
        {
            if (!_reader.TryPeek(out char ch))
            {
                return false;
            }
            return ch == c;
        }
    }
}
