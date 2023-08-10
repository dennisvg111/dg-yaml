using DG.Yaml.Parsers;
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
            _state = new TokenizationState();
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
            if (!_state.StreamStartTokenized)
            {
                _state.SetStreamStartTokenized();
                _tokens.Enqueue(Token.ForStreamStart());
                AdvanceState(1);
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

        private void GetPlainScalarToken()
        {
            var scalar = new Scalar();

            while (true)
            {

                while (_state.CanRead && !_state.CurrentCharacter.IsEmpty())
                {
                    if (_state.CurrentCharacter == Characters.MappingValue)
                    {
                        bool hasNextCharacter = _reader.TryPeek(out char nextCharacter);
                        if (!hasNextCharacter || nextCharacter.IsEmpty())
                        {
                            break;
                        }
                    }

                    scalar.Write(_state.CurrentCharacter);
                    AdvanceState(1);
                }

                if (!_state.CurrentCharacter.IsEmpty())
                {
                    break;
                }

                while (_state.CurrentCharacter.IsEmpty())
                {

                }
            }
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
                AdvanceState(1);
            }
        }

        private void AdvanceState(int count)
        {
            for (int i = 0; i < count; i++)
            {
                bool canRead = _reader.TryRead(out char ch);
                _state.UpdateState(!canRead, ch);
            }
        }
    }
}
