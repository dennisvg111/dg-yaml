namespace DG.Yaml.Tokenization.State
{
    public class StreamState
    {
        private readonly ICharacterReader _reader;
        private bool _canRead;
        private char _currentCharacter;
        private char? _nextCharacter;
        private int _charactersSinceNewline;
        private long _line;

        public bool StartedReading => _charactersSinceNewline >= 0;
        public bool CanRead => _canRead;
        public char CurrentCharacter => _currentCharacter;

        public int CharactersSinceNewline => _charactersSinceNewline;
        public long Line => _line;

        public StreamState(ICharacterReader characterReader)
        {
            _reader = characterReader;
        }

        public bool TryPeekNextCharacter(out char ch)
        {
            ch = _nextCharacter ?? '\0';
            return _nextCharacter.HasValue;
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
            int count = input.Length;
            if (_reader.TryPeek(count, out char[] chars) < count)
            {
                return false;
            }
            for (int i = 0; i < count; i++)
            {
                if (input[i] != chars[i])
                {
                    return false;
                }
            }
            return true;
        }

        #region advance
        public void Advance(int count)
        {
            for (int i = 0; i < count; i++)
            {
                bool canRead = _reader.TryRead(out char ch);
                UpdateState(canRead, ch);
            }
        }

        /// <summary>
        /// Advances through the current newline, and returns the amount of characters advanced.
        /// </summary>
        /// <returns></returns>
        public int AdvanceNewline()
        {
            if (!_canRead)
            {
                return 0;
            }
            if (_currentCharacter == '\r')
            {
                int newlineSize = (_nextCharacter.HasValue && _nextCharacter.Value == '\n') ? 2 : 1;
                Advance(newlineSize);
                return newlineSize;
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
                bool hasNext = _reader.TryPeek(out char nextCharacter);
                _nextCharacter = hasNext ? nextCharacter : (char?)null;
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
        #endregion
    }
}
