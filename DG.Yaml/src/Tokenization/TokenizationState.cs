namespace DG.Yaml.Tokenization
{
    public class TokenizationState : ITokenizationState
    {
        private readonly CharacterReader _reader;
        private bool _canRead;
        private char _currentCharacter;
        private int _charactersSinceNewline;

        public bool StartedReading => _charactersSinceNewline >= 0;
        public bool CanRead => _canRead;
        public char CurrentCharacter => _currentCharacter;

        public int CharactersSinceNewline => _charactersSinceNewline;

        public TokenizationState(CharacterReader reader)
        {
            _reader = reader;

            _canRead = true;
            _currentCharacter = '\0';
            _charactersSinceNewline = -1;
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

        public bool IsCurrent(string input)
        {
            if (!CanRead || CurrentCharacter != input[0])
            {
                return false;
            }
            return IsNext(input.Substring(1));
        }

        public bool IsNext(string input)
        {
            return _reader.IsNext(input);
        }

        public void Advance(int count)
        {
            for (int i = 0; i < count; i++)
            {
                bool canRead = _reader.TryRead(out char ch);
                UpdateState(!canRead, ch);
            }
        }

        private void UpdateState(bool streamEnded, char character)
        {
            if (!streamEnded)
            {
                _currentCharacter = character;
                UpdatePosition();
            }
            else
            {
                _canRead = false;
            }
        }

        private void UpdatePosition()
        {
            if (_currentCharacter == '\n')
            {
                _charactersSinceNewline = 0;
                return;
            }

            _charactersSinceNewline++;
        }
    }
}
