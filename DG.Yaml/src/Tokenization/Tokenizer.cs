using System.Collections.Generic;

namespace DG.Yaml.Tokenization
{
    public class Tokenizer
    {
        private readonly CharacterReader _reader;
        private readonly Queue<Token> _tokens;
        private readonly TokenizationState _state;

        private readonly PlainScalarTokenizer _plainScalarTokenizer;

        public Token ConsumeToken()
        {
            return _tokens.Dequeue();
        }


        public Tokenizer(CharacterReader reader)
        {
            _reader = reader;
            _tokens = new Queue<Token>();
            _state = new TokenizationState(reader);

            _plainScalarTokenizer = new PlainScalarTokenizer(_state);
        }

        public bool TryRead()
        {
            GetTokensWhileNeeded();
            return _state.CanRead;
        }

        private void GetTokensWhileNeeded()
        {
            GetNextToken();
            //if more tokens needed, try again.
        }

        private void GetNextToken()
        {
            if (!_state.StreamStartTokenized)
            {
                _state.SetStreamStartTokenized();
                _tokens.Enqueue(Token.ForStreamStart());
                _state.Advance(1);
                return;
            }

            SkipToTokenStart();

            if (!_state.CanRead)
            {
                _tokens.Enqueue(Token.ForStreamEnd());
                return;
            }

            _tokens.Enqueue(_plainScalarTokenizer.GetToken());

            return;
        }

        private void SkipToTokenStart()
        {
            while (_state.CanRead)
            {
                switch (_state.CurrentCharacter)
                {
                    case ' ':
                    case '\r':
                    case '\n':
                        break;
                    default:
                        return;
                }
                _state.Advance(1);
            }
        }
    }
}
