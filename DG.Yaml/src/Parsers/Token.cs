﻿namespace DG.Yaml.Parsers
{
    public class Token
    {
        private readonly TokenType _type;
        private string _content;

        public TokenType Type => _type;
        public bool HasContent => _content != null;

        public Token(TokenType type, string content)
        {
            _type = type;
            _content = content;
        }

        public static Token ForStreamStart()
        {
            return new Token(TokenType.StreamStart, null);
        }
    }
}
