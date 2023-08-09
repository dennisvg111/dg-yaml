using System.Runtime.CompilerServices;

namespace DG.Yaml.Tokenization
{
    public class TokenizationState
    {
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

        public TokenizationState()
        {
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

        public void UpdateState(bool streamEnded, char character)
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
                _currentLine++;
                _currentColumn = 0;
                return;
            }

            _currentColumn++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty()
        {
            return _currentCharacter == ' '
                || _currentCharacter == '\t'
                || _currentCharacter == '\r'
                || _currentCharacter == '\n';
        }
    }
}
