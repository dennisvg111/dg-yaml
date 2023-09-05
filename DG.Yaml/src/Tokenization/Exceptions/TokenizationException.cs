using DG.Yaml.Tokenization.State;
using System;

namespace DG.Yaml.Tokenization.Exceptions
{
    public class TokenizationException : Exception
    {
        private readonly long _line;
        private readonly int _column;

        public long Line => _line;
        public int Column => _column;

        public TokenizationException(TokenizationState state, string message) : base($"Exception on line {state.Stream.Line}, column {state.Stream.CharactersSinceNewline}: " + message)
        {
            _line = state.Stream.Line;
            _column = state.Stream.CharactersSinceNewline;
        }

        public static TokenizationExceptionThrower WithState(TokenizationState state)
        {
            return new TokenizationExceptionThrower(state);
        }
    }

    public class TokenizationExceptionThrower
    {
        private readonly TokenizationState _state;

        public TokenizationExceptionThrower(TokenizationState state)
        {
            _state = state;
        }

        public void Throw(string message)
        {
            throw new TokenizationException(_state, message);
        }
    }
}
