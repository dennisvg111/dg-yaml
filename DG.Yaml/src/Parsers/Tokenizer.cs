using System.Collections.Generic;

namespace DG.Yaml.Parsers
{
    public class Tokenizer
    {
        private readonly CharacterReader _reader;
        private readonly Queue<Token> _tokens;
        private bool _streamStartReturned;


        public Tokenizer(CharacterReader reader)
        {
            _reader = reader;
            _tokens = new Queue<Token>();
        }

        public bool TryRead()
        {

            return true;
        }

        private void GetTokens()
        {
            while (true)
            {

            }
        }

        private void GetNextToken()
        {
            if (!_streamStartReturned)
            {
                _streamStartReturned = true;
                _tokens.Enqueue(Token.ForStreamStart());
                return;
            }
        }

        private void SkipToTokenStart()
        {
            while (_reader.TryPeek(out char ch))
            {
                switch (ch)
                {
                    case ' ':
                    case '\r':
                    case '\n':
                        break;
                    default:
                        return;
                }
                _reader.TryRead(out _);
            }
        }
    }
}
