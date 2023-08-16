using DG.Yaml.Parsers;

namespace DG.Yaml.Tokenization
{
    public class PlainScalarTokenizer
    {
        private ITokenizationState _state;

        public PlainScalarTokenizer(ITokenizationState state)
        {
            _state = state;
        }

        public Token GetToken()
        {
            var scalar = new Scalar();
            var whitespace = new WhitespaceState();

            while (true)
            {
                if (_state.CurrentCharacter == Characters.Comment || IsDocumentIndicator())
                {
                    break;
                }

                ParseNonEmpties(scalar);

                if (!_state.CurrentCharacter.IsWhitespace())
                {
                    break;
                }

                ParseWhitespace(whitespace);
            }

            return new Token(TokenType.PlainScalar, scalar.ToString());
        }

        private bool IsDocumentIndicator()
        {
            if (_state.CurrentColumn != 0)
            {
                return false;
            }
            if (_state.IsCurrent(Indicators.DocumentStart))
            {
                //check for empty after.
            }
            if (_state.IsCurrent(Indicators.DocumentEnd))
            {
                //check for empty after.
            }
            return false;
        }

        private void ParseNonEmpties(Scalar scalar)
        {
            while (_state.CanRead && !_state.CurrentCharacter.IsWhitespace())
            {
                //check for mappings
                if (_state.CurrentCharacter == Characters.MappingValue)
                {
                    if (!_state.TryPeekNextCharacter(out char nextCharacter) || nextCharacter.IsWhitespace())
                    {
                        break;
                    }
                }

                scalar.Write(_state.CurrentCharacter);
                _state.Advance(1);
            }
        }

        private void ParseWhitespace(WhitespaceState whitespace)
        {
            while (_state.CurrentCharacter.IsWhitespace())
            {
                if (_state.CurrentCharacter.IsInlineWhitespace())
                {
                    ParseInlineWhitespace(whitespace);
                }
                else
                {
                    ParseNewlines(whitespace);
                }
            }
        }

        private void ParseInlineWhitespace(WhitespaceState whitespace)
        {

            _state.Advance(1);
        }

        private void ParseNewlines(WhitespaceState whitespace)
        {
            int newlineLength = _state.AdvanceNewline();
            if (!whitespace.IsLeadingBlanks)
            {

            }
        }

        private class WhitespaceState
        {
            private int _leadingNewlineLength;
            private int _trailingNewlineLength;

            public bool IsLeadingBlanks { get; set; }
            public int WhitespaceLength { get; set; }

            public WhitespaceState()
            {

            }

            public void SetLeadingNewlineLength(int leadingNewlineLength)
            {
                _leadingNewlineLength = leadingNewlineLength;
            }

            public void SetTrailingNewlineLength(int trailingNewlineLength)
            {
                _trailingNewlineLength = trailingNewlineLength;
            }
        }
    }
}
