using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Yaml.Tokenization
{
    public class TokenQueue
    {
        private readonly List<Token> _tokens;

        public int Count => _tokens.Count;

        public TokenQueue()
        {
            _tokens = new List<Token>();
        }

        public void Append(Token token)
        {
            _tokens.Add(token);
        }

        public void InsertAt(int index, Token token)
        {
            _tokens.Insert(index, token);
        }

        public Token Take()
        {
            if (!_tokens.Any())
            {
                throw new InvalidOperationException($"Cannot execute {nameof(Take)} operation on empty queue.");
            }
            var first = _tokens[0];
            _tokens.RemoveAt(0);
            return first;
        }

        public Token Peek()
        {
            if (!_tokens.Any())
            {
                throw new InvalidOperationException($"Cannot execute {nameof(Peek)} operation on empty queue.");
            }
            return _tokens[0];
        }
    }
}
