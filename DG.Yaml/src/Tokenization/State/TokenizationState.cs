namespace DG.Yaml.Tokenization.State
{
    public class TokenizationState : ITokenizationState
    {
        private readonly ICharacterReader _reader;
        private bool _canRead;
        private char _currentCharacter;
        private int _charactersSinceNewline;
        private long _line;

        public bool StartedReading => _charactersSinceNewline >= 0;
        public bool CanRead => _canRead;
        public char CurrentCharacter => _currentCharacter;

        public int CharactersSinceNewline => _charactersSinceNewline;
        public long Line => _line;

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

        public void Advance(int count)
        {
            for (int i = 0; i < count; i++)
            {
                bool canRead = _reader.TryRead(out char ch);
                UpdateState(canRead, ch);
            }
        }

        /// <inheritdoc/>
        public int AdvanceNewline()
        {
            if (!_canRead)
            {
                return 0;
            }
            if (_currentCharacter == '\r')
            {
                if (IsNext('\n'))
                {
                    Advance(2);
                    return 2;
                }
                Advance(1);
                return 1;
            }
            if (_currentCharacter == '\n')
            {
                Advance(1);
                return 1;
            }
            return 0;
        }

        private void UpdateState(bool canRead, char character)
        {
            if (canRead)
            {
                _currentCharacter = character;
                UpdatePosition();
                return;
            }
            if (_canRead && _charactersSinceNewline < 0) //catch empty documents.
            {
                _charactersSinceNewline = 0;
            }
            _canRead = false;
        }

        private void UpdatePosition()
        {
            if (_currentCharacter == '\n')
            {
                _charactersSinceNewline = 0;
                _line++;
                return;
            }

            _charactersSinceNewline++;
        }
    }
}
