using System.Collections.Generic;

namespace DG.Yaml.Tokenization
{
    public class Tokenizer
    {
        private readonly CharacterReader _reader;
        private readonly Queue<Token> _tokens;
        private TokenizationState _state;

        public Queue<Token> Tokens => new Queue<Token>(_tokens);


        public Tokenizer(CharacterReader reader)
        {
            _reader = reader;
            _tokens = new Queue<Token>();
            _state = new TokenizationState(reader);
        }

        public bool TryRead()
        {
            GetTokens();
            return true;
        }

        private void GetTokens()
        {
            GetNextToken();
            //if more tokens needed, try again.
        }

        private void GetNextToken()
        {
            if (!_state.StartedReading)
            {
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
