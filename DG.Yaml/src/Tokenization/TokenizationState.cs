namespace DG.Yaml.Tokenization
{
    public class TokenizationState : ITokenizationState
    {
        private readonly CharacterReader _reader;
        private bool _streamStartTokenized;
        private bool _canRead;
        private char _currentCharacter;
        private int _currentLine;
        private int _currentColumn;

        public bool StreamStartTokenized => _streamStartTokenized;
        public bool CanRead => _canRead;
        public char CurrentCharacter => _currentCharacter;

        public int CurrentLine => _currentLine;
        public int CurrentColumn => _currentColumn;

        public TokenizationState(CharacterReader reader)
        {
            _reader = reader;

            _streamStartTokenized = false;
            _canRead = true;
            _currentCharacter = '\0';
            _currentLine = 0;
            _currentColumn = -1;
        }

        public void SetStreamStartTokenized()
        {
            _streamStartTokenized = true;
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
            _canRead = false;
        }

        private void UpdatePosition()
        {
            if (_currentCharacter == '\n')
            {
                _currentLine++;
                _currentColumn = 0;
                return;
            }

            _currentColumn++;
        }
    }
}
