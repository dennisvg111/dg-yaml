﻿using DG.Yaml.Parsers;
using DG.Yaml.Tokenization.State;

namespace DG.Yaml.Tokenization
{
    public class PlainScalarTokenizer : ITokenizer
    {
        private TokenizationState _state;

        public PlainScalarTokenizer(TokenizationState state)
        {
            _state = state;
        }

        public Token GetToken()
        {
            var scalar = new Scalar();
            var whitespace = new WhitespaceState();

            while (true)
            {
                if (_state.Stream.CurrentCharacter == Characters.Comment || IsDocumentIndicator())
                {
                    break;
                }

                ParseNonEmpties(scalar, whitespace);

                if (!_state.Stream.CurrentCharacter.IsWhitespace())
                {
                    break;
                }

                ParseWhitespace(whitespace);
            }

            return new Token(TokenType.PlainScalar, scalar.ToString());
        }

        private bool IsDocumentIndicator()
        {
            if (_state.Stream.CharactersSinceNewline != 0)
            {
                return false;
            }
            if (_state.Stream.IsCurrent(Indicators.DocumentStart))
            {
                //check for empty after.
            }
            if (_state.Stream.IsCurrent(Indicators.DocumentEnd))
            {
                //check for empty after.
            }
            return false;
        }

        private void ParseNonEmpties(Scalar scalar, WhitespaceState whitespace)
        {
            while (_state.Stream.CanRead && !_state.Stream.CurrentCharacter.IsWhitespace())
            {
                //check for mappings
                if (_state.Stream.CurrentCharacter == Characters.MappingValue)
                {
                    if (!_state.Stream.TryPeekNextCharacter(out char nextCharacter) || nextCharacter.IsWhitespace())
                    {
                        break;
                    }
                }

                WriteWhitespaceToScalar(scalar, whitespace);

                scalar.Write(_state.Stream.CurrentCharacter);
                _state.Stream.Advance(1);
            }
        }

        private void WriteWhitespaceToScalar(Scalar scalar, WhitespaceState whitespace)
        {
            if (!whitespace.IsLeadingBlanks && whitespace.WhitespaceLength == 0)
            {
                return;
            }
            if (!whitespace.IsLeadingBlanks)
            {
                scalar.Write(whitespace.GetWhitespace());
                whitespace.ClearWhitespace();
                return;
            }

            if (!whitespace.HasTrailingNewline)
            {
                scalar.Write(' ');
            }
            else
            {
                scalar.Write(whitespace.GetTrailingNewline());
                whitespace.SetTrailingNewlineLength(0);
            }
            whitespace.SetLeadingNewlineLength(0);
        }

        private void ParseWhitespace(WhitespaceState whitespace)
        {
            while (_state.Stream.CurrentCharacter.IsWhitespace())
            {
                if (_state.Stream.CurrentCharacter == '\r' || _state.Stream.CurrentCharacter == '\n')
                {
                    ParseNewlines(whitespace);
                }
                else
                {
                    ParseInlineWhitespace(whitespace);
                }
            }
        }

        private void ParseInlineWhitespace(WhitespaceState whitespace)
        {
            if (!whitespace.IsLeadingBlanks)
            {
                whitespace.AppendWhitespace(_state.Stream.CurrentCharacter);
            }

            _state.Stream.Advance(1);
        }

        private void ParseNewlines(WhitespaceState whitespace)
        {
            int newlineLength = _state.Stream.AdvanceNewline();
            if (!whitespace.IsLeadingBlanks)
            {
                whitespace.SetLeadingNewlineLength(newlineLength);
                whitespace.ClearWhitespace();
                return;
            }
            whitespace.SetTrailingNewlineLength(newlineLength);
        }
    }
}
