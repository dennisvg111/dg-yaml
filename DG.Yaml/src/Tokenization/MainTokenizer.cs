using DG.Yaml.Tokenization.State;
using DG.Yaml.Utilities;

namespace DG.Yaml.Tokenization
{
    public class MainTokenizer
    {
        private readonly CharacterReader _reader;
        private readonly TokenizationState _state;
        private readonly InsertionQueue<Token> _tokens;

        private readonly PlainScalarTokenizer _plainScalarTokenizer;

        public Token ConsumeToken()
        {
            return _tokens.Take();
        }


        public MainTokenizer(CharacterReader reader)
        {
            _reader = reader;
            _tokens = new InsertionQueue<Token>();
            _state = new TokenizationState(reader);

            _plainScalarTokenizer = new PlainScalarTokenizer(_state);
        }

        public bool TryRead()
        {
            GetTokensWhileNeeded();
            return _state.Stream.CanRead;
        }

        private void GetTokensWhileNeeded()
        {
            GetNextToken();
            //if more tokens needed, try again.
        }

        private void GetNextToken()
        {
            if (!_state.Stream.StartedReading)
            {
                _tokens.Append(Token.ForStreamStart());
                _state.Stream.Advance(1);
                return;
            }

            SkipToTokenStart();

            if (!_state.Stream.CanRead)
            {
                _tokens.Append(Token.ForStreamEnd());
                return;
            }

            _tokens.Append(_plainScalarTokenizer.GetToken());

            return;
        }

        private void SkipToTokenStart()
        {
            while (_state.Stream.CanRead)
            {
                switch (_state.Stream.CurrentCharacter)
                {
                    case ' ':
                    case '\r':
                    case '\n':
                        break;
                    default:
                        return;
                }
                _state.Stream.Advance(1);
            }
        }
    }
}
