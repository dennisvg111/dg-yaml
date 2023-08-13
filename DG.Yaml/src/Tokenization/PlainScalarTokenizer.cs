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

            while (true)
            {
                if (_state.CurrentCharacter == Characters.Comment || IsDocumentIndicator())
                {
                    break;
                }

                ParseNonEmpties(scalar);

                if (!_state.CurrentCharacter.IsEmpty())
                {
                    break;
                }

                ParseEmpties();
            }

            return new Token(TokenType.PlainScalar, scalar.ToString());
        }

        public bool IsDocumentIndicator()
        {
            if (_state.CharactersSinceNewline != 0)
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

        public void ParseNonEmpties(Scalar scalar)
        {
            while (_state.CanRead && !_state.CurrentCharacter.IsEmpty())
            {
                //check for mappings
                if (_state.CurrentCharacter == Characters.MappingValue)
                {
                    if (!_state.TryPeekNextCharacter(out char nextCharacter) || nextCharacter.IsEmpty())
                    {
                        break;
                    }
                }

                scalar.Write(_state.CurrentCharacter);
                _state.Advance(1);
            }
        }

        public void ParseEmpties()
        {
            while (_state.CurrentCharacter.IsEmpty())
            {

            }
        }
    }
}
